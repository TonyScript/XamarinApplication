#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 条码扫描的相关帮助类
****************/
#endregion

using System;
using ZXing.Mobile;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 条码扫描的相关帮助类
	/// </summary>
	public static class BarcodeUtil
	{
		private static MobileBarcodeScanner _scannner;

		public static void Scan(Action<string> success)
		{
			if (_scannner == null)
			{
				_scannner = new MobileBarcodeScanner { FlashButtonText = "闪光灯", CancelButtonText = "取消" };
			}
			_scannner.Scan()
				.ContinueWith(t =>
				{
					if (t.IsFaulted)
					{
						ErrorHandlerUtil.ReportException(t.Exception);
						return;
					}

					if (success != null)
					{
						success(t.Result.Text);
					}
				});
		}
	}
}