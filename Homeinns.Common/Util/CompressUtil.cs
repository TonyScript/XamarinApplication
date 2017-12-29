using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Homeinns.Common.Util
{
	public class CompressUtil
	{
		/// <summary>
		/// 封装了 zip 操作的类库
		/// </summary>
		public static void ZipUnCompress(string fileToUnzip, string savePath, string password)
		{
			if (fileToUnzip == string.Empty || !File.Exists(fileToUnzip))
			{
				throw new Exception("压缩文件不存在！");
			}

			if (string.IsNullOrWhiteSpace(savePath))
			{
				throw new Exception("文件解压后的保存文件夹路径不能为空！");
			}

			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}

			using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(fileToUnzip)))
			{
				ZipEntry theEntry;
				while ((theEntry = zipStream.GetNextEntry()) != null)
				{
					string directoryName = Path.GetDirectoryName(theEntry.Name);


					if (!string.IsNullOrWhiteSpace(directoryName)
						&&
						!Directory.Exists(Path.Combine(savePath, directoryName)))
					{
						Directory.CreateDirectory(Path.Combine(savePath, directoryName));
					}

					string fileName = Path.GetFileName(theEntry.Name);
					if (string.IsNullOrWhiteSpace(fileName))
					{
						continue;
					}

					using (var streamWriter = File.Create(Path.Combine(savePath, theEntry.Name)))
					{
						var data = new byte[2048];
						while (true)
						{
							var size = zipStream.Read(data, 0, data.Length);
							if (size <= 0)
								break;

							streamWriter.Write(data, 0, size);
						}
					}
				}
			}

		}
	}
}
