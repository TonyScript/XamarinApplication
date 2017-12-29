using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Homeinns.Common.Util
{
	public static class LoggingUtil
	{
		static readonly string LogFilePath;

		static LoggingUtil()
		{
			LogFilePath = Path.Combine(FileSystemUtil.TmpFolder, "log");
			if (!Directory.Exists(LogFilePath))
				Directory.CreateDirectory(LogFilePath);
		}

		/// <summary>
		/// log info
		/// </summary>
		/// <param name="msg"></param>
		public static void Info(string msg)
		{
			var logMsg = string.Format("[info][{0}]{1}", DateTime.Now.ToString("hh:MM:ss"), msg);
			Debug.WriteLine(logMsg);
			try
			{
				var fileName = "info_log_" + DateTime.Now.ToString("yyMMdd") + ".log";
				fileName = Path.Combine(LogFilePath, fileName);

				using (StreamWriter sw = new StreamWriter(fileName, append: true))
				{
					sw.WriteLine(logMsg);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// log error
		/// </summary>
		/// <param name="msg"></param>
		public static void Error(string msg)
		{
			var logMsg = string.Format("[error][{0}]{1}", DateTime.Now.ToString("hh:MM:ss"), msg);
			Debug.WriteLine(msg);
			try
			{
				var fileName = "error_log_" + DateTime.Now.ToString("yyMMdd") + ".log";
				fileName = Path.Combine(LogFilePath, fileName);

				using (StreamWriter sw = new StreamWriter(fileName, append: true))
				{
					sw.WriteLine(logMsg);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// log exception
		/// </summary>
		/// <param name="msg"></param>
		public static void Exception(Exception ex)
		{
			if (ex.InnerException != null)
			{
				Exception(ex.InnerException);
			}

			var logMsg = string.Format("[exception][{0}]{1}", DateTime.Now.ToString("hh:MM:ss"),
							 ex.Message + Environment.NewLine + ex.StackTrace);
			Debug.WriteLine(logMsg);

			try
			{
				var fileName = "exception_log_" + DateTime.Now.ToString("yyMMdd") + ".log";
				fileName = Path.Combine(LogFilePath, fileName);

				using (StreamWriter sw = new StreamWriter(fileName, append: true))
				{
					sw.WriteLine(logMsg);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// 根据类型获取log的文件名
		/// </summary>
		/// <param name="msg"></param>
		public static List<string> GetLogFileNamesByType(string type)
		{
			try
			{
				var fileFullNames = Directory.GetFiles(LogFilePath);
				var l = new List<string>(fileFullNames.Length);
				foreach (var path in fileFullNames)
				{
					var fileName = Path.GetFileName(path);
					if (fileName.StartsWith(type))
						l.Add(fileName);
				}

				return l;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		///清理所有的log文件
		/// </summary>
		public static void Clear()
		{
			try
			{
				var fileFullNames = Directory.GetFiles(LogFilePath);
				foreach (var path in fileFullNames)
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// 获取log文件的内容
		/// </summary>
		/// <param name="logFileName"></param>
		/// <returns></returns>
		public static string GetLogFileContent(string logFileName)
		{
			try
			{
				var fullLogFileName = Path.Combine(LogFilePath, logFileName);
				if (!File.Exists(fullLogFileName))
					return string.Empty;

				using (StreamReader sr = new StreamReader(fullLogFileName))
				{
					return sr.ReadToEnd();
				}
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}

