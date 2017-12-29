using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using Homeinns.Common.Configuration;
using Homeinns.Common.Util;
using Homeinns.Common.ViewModel;

namespace Homeinns.Common.Service
{
	/// <summary>
	/// 版本自动更新的服务
	/// </summary>
	public static class VersionService
	{
		/// <summary>
		/// 在App安装后，第一次安装Html的版本文件
		/// </summary>
		private static async void TryInstallWww()
		{
			try
			{
				//第一个版本通过资源文件的方式打包
				var firstVersionZipFileName = Path.Combine(NSBundle.MainBundle.BundlePath, "www.zip");
				if (!File.Exists(firstVersionZipFileName))
				{
					return;
				}

				var destFolderName = Path.Combine(FileSystemUtil.CachesFolder, "www");
				if (Directory.Exists(destFolderName))
				{
					return;
				}

				await Task.Run(() => CompressUtil.ZipUnCompress(firstVersionZipFileName, destFolderName, null));

				File.Copy(Path.Combine(NSBundle.MainBundle.BundlePath, "_blank.html"), destFolderName);

			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}
		}

		private static async Task<VersionUpdateModel> GetLatestIosVersion()
		{
			var apiUrl = "api/Update/GetUpdateInfo?clientType=2&versionCode={0}";
			apiUrl = string.Format(apiUrl, NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString());
			var latestVersion = await RestClient.GetAsync<VersionUpdateModel>(apiUrl)
				.ConfigureAwait(continueOnCapturedContext: false);
			//GlobalAppSetting.IOSVersion = latestVersion.VersionCode;
			return latestVersion;
		}

		/// <summary>
		/// 尝试检查Ios程序是否有新的版本，如果有，则更新
		/// </summary>
		public static async Task<bool> TryUpgradeIos()
		{
			try
			{
				var version = await GetLatestIosVersion();
				if (version == null)
				{
					return false;
				}

				if (!version.IsUpdate)
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return false;
			}
		}

		private static async Task<VersionUpdateModel> GetLatestWwwVersion()
		{
			var apiUrl = "api/Update/GetUpdateInfo?clientType=3&versionCode={0}";
			apiUrl = string.Format(apiUrl, AppGlobalSetting.WwwVersion);
			var latestVersion = await RestClient.GetAsync<VersionUpdateModel>(apiUrl)
				.ConfigureAwait(continueOnCapturedContext: false);
			return latestVersion;
		}

		/// <summary>
		/// 尝试检查HTML5程序是否有新的版本，如果有，则更新
		/// </summary>
		public static async Task<bool> TryUpgradeWww()
		{
			try
			{
				var version = await GetLatestWwwVersion();
				if (version == null)
				{
					return false;
				}

				if (!version.IsUpdate)
				{

					return false;
				}

				//下载 www 更新包

				var upgradeFileName = await DownloadWwwZipFile(version);
				if (string.IsNullOrWhiteSpace(upgradeFileName))
				{
					return false;
				}

				//下载的更新包路径
				var destFolderName = Path.Combine(FileSystemUtil.CachesFolder, "www");
				//如果路径已经存在删除之
				if (Directory.Exists(destFolderName))
				{
					DeleteDirectory(destFolderName);
				}
				//解压缩 www 更新包
				await Task.Run(() => CompressUtil.ZipUnCompress(upgradeFileName, destFolderName, null));

				//使用 NSUserDefaults 保存本次更新的版本号
				AppGlobalSetting.WwwVersion = version.VersionCode;
				return true;

			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return false;
			}
		}

		private static async Task<string> DownloadWwwZipFile(VersionUpdateModel v)
		{
			var apiUrl = "FileDownloadHandler.ashx?moduletype=version&fileid={0}";
			apiUrl = string.Format(apiUrl, v.FileId);
			var bytes = await RestClient.DownloadFileAsync(apiUrl);

			var updateFileName = Path.Combine(FileSystemUtil.TmpFolder, "www_upgrade_" + v.VersionCode + ".zip");
			using (var fs = new FileStream(updateFileName, FileMode.OpenOrCreate))
			{
				fs.Write(bytes, 0, bytes.Length);
			}

			return updateFileName;
		}

		/// <summary>
		/// Downloads the attachment file.
		/// </summary>
		/// <returns>The attachment file.</returns>
		/// <param name="url">URL.</param>
		/// <param name="fileId">File identifier.</param>
		/// <param name="fileExt">File ext.</param>
		public static void DownloadAttachmentFile(string url, string fileId, string fileExt, Action success, Action<string> failure)
		{
			try
			{
				RestClient.DownloadFileAsync(url, (respBytes) =>
				{
					var attachmentFileName = Path.Combine(FileSystemUtil.TmpFolder, fileId + "." + fileExt);
					File.WriteAllBytes(attachmentFileName, respBytes);
					if (success != null)
					{
						success();
					}
				}, (msg) =>
				{
					if (failure != null)
					{
						failure(msg);
					}
				});
			}
			catch (Exception ex)
			{
				if (failure != null)
				{
					failure(ex.Message);
				}
			}
		}

		public static string AppVersion
		{
			get
			{
				return RawAppVersion
				+ "(" + WwwAppVersion + ")";
			}

		}

		public static string RawAppVersion
		{
			get
			{
				return
					NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString()
				+ "." +
				NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
			}
		}

		public static string WwwAppVersion
		{
			get
			{
				return AppGlobalSetting.WwwVersion;
			}
		}

		public static string PMSWwwAppVersion
		{
			get
			{
				return AppGlobalSetting.PMSWwwVersion;
			}
		}

		private static void DeleteDirectory(string path)
		{
			if (Directory.Exists(path))
			{
				var directories = Directory.GetDirectories(path);

				foreach (var directory in directories)
				{
					DeleteDirectory(directory);
				}

				var files = Directory.GetFiles(path);
				foreach (var file in files)
				{
					File.Delete(file);
				}

				Directory.Delete(path);
			}
		}
	}
}

