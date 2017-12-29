#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 消息提示框类
****************/
#endregion

using System;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 消息提示框类
	/// </summary>
	public class Toast : IDisposable
	{
		ProgressHUD _hud;

		/// <summary>
		/// 构造函数
		/// </summary>
		public Toast()
		{
			_hud = new ProgressHUD { ForceiOS6LookAndFeel = true };
		}

		/// <summary>
		/// 转圈的消息提示
		/// </summary>
		/// <param name="msg"></param>
		public void ProgressWaiting(string msg)
		{
			_hud.ShowContinuousProgress(msg);
		}

		/// <summary>
		/// 隐藏消息试试狂
		/// </summary>
		public void Dismiss()
		{
			_hud.Dismiss();
		}

		public void Dispose()
		{
			_hud.Dismiss();
			_hud = null;
		}
	}
}

