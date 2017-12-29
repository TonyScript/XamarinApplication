using System;
using Homeinns.Common.Service;
using UIKit;

namespace Homeinns.Common.ViewModel
{
	public class UserModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		/// <value>The type of the auth.</value>
		public string uid { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		/// <value>The auth token.</value>
		public string pwd { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		/// <value>The auth token.</value>
		public string pluginKey { get { return "LoginLog"; } }

		/// <summary>
		/// 密码
		/// </summary>
		/// <value>The auth token.</value>
		public string pluginContent { get; set; }

		public string checkCode { get; set; }

		public string verifyStr { get; set; }
	}

	public class LoginLogModel
	{
		public string LoginLogId { get { return Guid.NewGuid().ToString(); } }

		public string OsVersion { get { return UIDevice.CurrentDevice.SystemVersion; } }

		public string AppVersion { get { return VersionService.RawAppVersion; } }

		public string Html5Version { get { return VersionService.WwwAppVersion; } }

		public int ClientType { get { return 2; } }
	}
}

