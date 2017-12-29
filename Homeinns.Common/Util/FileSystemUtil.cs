using System;
using System.IO;
using System.Linq;
using Foundation;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 手机操作系统的文件系统的帮助类
	/// </summary>
	public static class FileSystemUtil
	{
		/// <summary>
		/// IOS的Document文件夹的路径
		/// </summary>
		public static void Disabled_iCloudBackup()
		{
			NSFileManager.SetSkipBackupAttribute(DocumentsFolder, true);
			NSFileManager.SetSkipBackupAttribute(CachesFolder, true);
			NSFileManager.SetSkipBackupAttribute(PreferencesFolder, true);
		}

		/// <summary>
		/// iOS的Document文件夹的路径
		/// </summary>
		private static string DocumentsFolder
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			}
		}

		/// <summary>
		/// iOS的Cache文件夹的路径
		/// </summary>
		public static string CachesFolder
		{
			get
			{
				return Path.Combine(DocumentsFolder, "..", "Library", "Caches");
			}
		}

		/// <summary>
		/// iOS的Preferences文件夹的路径
		/// </summary>
		public static string PreferencesFolder
		{
			get
			{
				return Path.Combine(DocumentsFolder, "..", "Library", "Preferences");
			}
		}

		/// <summary>
		/// IOS的Temp文件夹的路径
		/// </summary>
		public static string TmpFolder
		{
			get
			{
				return Path.Combine(DocumentsFolder, "..", "tmp");
			}
		}

		/// <summary>
		/// 根据文件Id从缓存的文件中获取文件的Base64内容
		/// </summary>
		public static string GetBase64StringFromCache(string fileId)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(fileId))
					return null;

				fileId = fileId.Replace("/", "");
				var fileFullName = Path.Combine(CachesFolder, fileId);
				if (!File.Exists(fileFullName))
					return null;

				using (StreamReader sr = new StreamReader(fileFullName))
				{
					return sr.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return null;
			}
		}


		/// <summary>
		/// 将Base64格式的内容放到文件的缓存中
		/// </summary>
		public static void SaveBase64StringToCache(string fileId, string fileContent)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(fileId) || string.IsNullOrEmpty(fileContent))
					return;

				fileId = fileId.Replace("/", "");
				var fileFullName = Path.Combine(CachesFolder, fileId);
				using (StreamWriter sw = new StreamWriter(fileFullName, false))
				{
					sw.Write(fileContent);
				}
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}
		}

		/// <summary>
		/// 删除定路径文件夹里文件
		/// </summary>
		/// <param name="filePath">File path.</param>
		public static void DeleteFile(string filePath)
		{
			try
			{
				if (Directory.Exists(filePath))
				{
					foreach (string file in Directory.GetFileSystemEntries(filePath))
					{
						if (File.Exists(file))
						{
							File.Delete(file); //直接删除其中的文件  
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}
		}

		/// <summary>
		/// 获取指定路径文件夹里文件的大小
		/// </summary>
		/// <returns>The size.</returns>
		/// <param name="filePath">File path.</param>
		public static float GetFileSize(string filePath)
		{
			try
			{
				var length = 0;
				if (Directory.Exists(filePath))
				{
					length += (from file in Directory.GetFileSystemEntries(filePath) where File.Exists(file) select new FileInfo(file) into fi select (int)fi.Length).Sum();
				}
				return (float)length / 1024;
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return 0;
			}
		}
	}
}

