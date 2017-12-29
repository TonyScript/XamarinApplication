using UIKit;

namespace Homeinns.Common.ViewModel
{
	/// <summary>
	/// 设置界面信息对象
	/// </summary>
	public class UserInfoViewModel
	{
		/// <summary>
		/// 部门
		/// </summary>
		/// <value>The auth token.</value>
		public string BusinessUnitName { get; set; }

		/// <summary>
		/// 邮件地址
		/// </summary>
		/// <value>The type of the auth.</value>
		public string EmailAddress { get; set; }

		/// <summary>
		/// 友好名称
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string FriendlyName { get; set; }

		/// <summary>
		/// 电话
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string Phone { get; set; }

		/// <summary>
		/// 用户id
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string SystemUserId { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string UserName { get; set; }

		/// <summary>
		/// 用户编码
		/// </summary>
		/// <value>The user code.</value>
		public string UserCode { get; set; }

		private static UIImage _defaultAvatar;
		public static UIImage DefaultAvatar
		{
			get
			{
				if (_defaultAvatar == null)
					_defaultAvatar = UIImage.FromFile("contact_default_avatar.png");

				return _defaultAvatar;
			}
		}
	}
}

