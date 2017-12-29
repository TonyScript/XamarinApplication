using UIKit;


namespace Homeinns.Common.Util
{
	/// <summary>
	/// 消息提示框的工具类
	/// </summary>
	public static class AlertUtil
	{
		private static readonly ProgressHUD ErrorHud = new ProgressHUD();
		private static readonly ProgressHUD SuccessHud = new ProgressHUD();
		private static readonly ProgressHUD WaitingHud = new ProgressHUD();
		private static UIViewController _vc;

		/// <summary>
		/// 初始化消息提醒绑定的ViewController
		/// </summary>
		/// <param name="vc"></param>
		public static void Initialize(UIViewController vc)
		{
			_vc = vc;
			ErrorHud.ForceiOS6LookAndFeel = true;
			SuccessHud.ForceiOS6LookAndFeel = true;
			WaitingHud.ForceiOS6LookAndFeel = true;
		}

		/// <summary>
		/// 弹出错误的消息提醒框
		/// </summary>
		/// <param name="msg">消息</param>
		public static void Error(string msg)
		{
			if (_vc == null)
				return;

			if (string.IsNullOrWhiteSpace(msg))
				return;
			_vc.InvokeOnMainThread(() =>
			{
				ErrorHud.ShowErrorWithStatus(msg, 3 * 1000);
			});
		}

		/// <summary>
		/// 弹出成功的消息提醒框
		/// </summary>
		/// <param name="msg">消息</param>
		public static void Success(string msg)
		{
			if (_vc == null)
				return;

			if (string.IsNullOrWhiteSpace(msg))
				return;

			_vc.InvokeOnMainThread(() =>
			{
				SuccessHud.ShowSuccessWithStatus(msg, 3 * 1000);
			});
		}

		/// <summary>
		/// 弹出菊花等待的消息提醒框
		/// </summary>
		/// <param name="msg"></param>
		public static void ShowWaiting(string msg)
		{
			if (_vc == null)
				return;

			_vc.InvokeOnMainThread(() =>
			{
				WaitingHud.ShowContinuousProgress(msg);
			});
		}

		/// <summary>
		/// 掩藏菊花转圈的消息提醒框
		/// </summary>
		public static void DismissWaiting()
		{
			if (_vc == null)
				return;

			_vc.InvokeOnMainThread(() =>
			{
				WaitingHud.Dismiss();
			});
		}
	}
}

