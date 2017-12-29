using System;
using System.Collections.Generic;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 统一的系统错误处理类
	/// </summary>
	public static class ErrorHandlerUtil
	{
		public static List<Func<string, bool>> ErrorHandlers = new List<Func<string, bool>>();

		/// <summary>
		/// 订阅错误信息
		/// </summary>
		/// <param name="func"></param>
		public static void Subscribe(Func<string, bool> func)
		{
			ErrorHandlers.Add(func);
		}

		/// <summary>
		/// 取消订阅错误信息
		/// </summary>
		/// <param name="func"></param>
		public static void UnSubscribe(Func<string, bool> func)
		{
			ErrorHandlers.Remove(func);
		}

		/// <summary>
		/// Report Error
		/// </summary>
		/// <param name="errorMessage">错误消息</param>
		public static void ReportError(string errorMessage)
		{
			LoggingUtil.Error(errorMessage);
			try
			{
				ErrorHandlers.ForEach(handler =>
				{
					handler(errorMessage);
				});
			}
			catch (Exception ex)
			{
				LoggingUtil.Exception(ex);
			}
		}

		/// <summary>
		/// Report Exception
		/// </summary>
		/// <param name="ex">异常类</param>
		public static void ReportException(Exception ex)
		{
			LoggingUtil.Exception(ex);
			var exMessage = ex.Message;
			if (ex is System.IO.IOException || ex is System.Net.WebException)
			{
				exMessage = "[网络]连接异常，请检查网络设置或者重试！";
			}
			try
			{
				ErrorHandlers.ForEach(handler =>
				{
					handler(exMessage);
				});
			}
			catch
			{
			}
		}
	}
}

