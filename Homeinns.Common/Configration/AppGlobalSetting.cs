using Foundation;
using System;

namespace Homeinns.Common.Configuration
{
	/// <summary>
	/// App级别的全局设置类
	/// </summary>
	public static class AppGlobalSetting
	{
		private static NSUserDefaults _defaults;

		/// <summary>
		/// 设置Key/Value格式的配置项目
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		public static void SetValue(string key, string value)
		{
			if (_defaults == null)
				_defaults = NSUserDefaults.StandardUserDefaults;

			_defaults.SetString(value, key);
		}

		/// <summary>
		/// 根据Key获取设置项目的值
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Value</returns>
		public static string GetValue(string key)
		{
			if (_defaults == null)
				_defaults = NSUserDefaults.StandardUserDefaults;

			return _defaults.StringForKey(key);
		}

		/// <summary>
		/// 是否是第一次打开应用，如果是，则需要设定服务器的地址
		/// </summary>
		/// <value><c>true</c> if is first open; otherwise, <c>false</c>.</value>
		public static string IsFirstOpen
		{
			get
			{
				var v = GetValue("IsFirstOpen");
				return v;
			}
			set
			{
				SetValue("IsFirstOpen", value);
			}
		}


		/// <summary>
		/// App的版本号，通过这个来控制本地数据库的版本，此版本号发生变化，则会导致创建一个新的数据库文件
		/// </summary>
		public static string LocalDbVersion
		{
			get
			{
				return "150729";
			}
		}

		/// <summary>
		/// 本地存储的用户上次登录的账号
		/// </summary>
		public static string UserCode
		{
			get
			{
				return GetValue("chat_UserCode");
			}
			set
			{
				SetValue("chat_UserCode", value);
			}
		}

		/// <summary>
		/// 带域名的登录账号
		/// </summary>
		/// <value>The domain user code.</value>
		public static string DomainUserCode
		{
			get
			{
				return GetValue("DomainUserCode");
			}
			set
			{
				SetValue("DomainUserCode", value);
			}
		}

		/// <summary>
		/// 本地存储的用户上次登陆的密码
		/// </summary>
		public static string Password
		{
			get
			{
				return GetValue("chat_Password");
			}
			set
			{
				SetValue("chat_Password", value);
			}
		}

		/// <summary>
		/// 本地存储的用户上次登录的hotelcd
		/// </summary>
		public static string HotelCD
		{
			get
			{
				return GetValue("chat_HotelCD");
			}
			set
			{
				SetValue("chat_HotelCD", value);
			}
		}

		/// <summary>
		/// 本地存储的用户上次登录的hotelname
		/// </summary>
		public static string HotelName
		{
			get
			{
				return GetValue("chat_HotelName");
			}
			set
			{
				SetValue("chat_HotelName", value);
			}
		}

		/// <summary>
		/// 本地存储的用户上次登录的BrandCd
		/// </summary>
		public static string BrandCd
		{
			get
			{
				return GetValue("chat_BrandCd");
			}
			set
			{
				SetValue("chat_BrandCd", value);
			}
		}

		/// <summary>
		/// 是否是有多个酒店
		/// </summary>
		public static bool IsMultiHotel
		{
			get
			{
				return !string.IsNullOrWhiteSpace(GetValue("IsMultiHotel")) && bool.Parse(GetValue("IsMultiHotel"));
			}
			set
			{
				SetValue("IsMultiHotel", value.ToString());
			}
		}

		/// <summary>
		/// 是否是运营经理or前厅经理
		/// </summary>
		public static bool IsRunOrVestibuleManager
		{
			get
			{
				return !string.IsNullOrWhiteSpace(GetValue("IsRunOrVestibuleManager")) && bool.Parse(GetValue("IsRunOrVestibuleManager"));
			}
			set
			{
				SetValue("IsRunOrVestibuleManager", value.ToString());
			}
		}

		/// <summary>
		/// 本地存储PMS Server URL
		/// </summary>
		public static string PmsServerUrl
		{
			get
			{
				return GetValue("chat_PmsServerUrl");
			}
			set
			{
				SetValue("chat_PmsServerUrl", value);
			}
		}

		/// <summary>
		/// 本地存储SSO Token
		/// </summary>
		public static string SsoToken
		{
			get
			{
				return GetValue("chat_SsoToken");
			}
			set
			{
				SetValue("chat_SsoToken", value);
			}
		}

		/// <summary>
		/// 本地存储mobile uuid
		/// </summary>
		public static string UUID
		{
			get
			{
				return GetValue("chat_UUID");
			}
			set
			{
				SetValue("chat_UUID", value);
			}
		}

		/// <summary>
		/// 本地存储SSO Token 过期时间
		/// </summary>
		public static Int32 SsoTokenExpiredTime
		{
			get
			{
				var v = GetValue("chat_SsoTokenExpiredTime");
				return Convert.ToInt32(v);
			}
			set
			{
				SetValue("chat_SsoTokenExpiredTime", value.ToString());
			}
		}

		/// <summary>
		/// 本地存储SSO Token 的 RefreshToken
		/// </summary>
		public static string ReFreshToken
		{
			get
			{
				return GetValue("chat_ReFreshToken");
			}
			set
			{
				SetValue("chat_ReFreshToken", value);
			}
		}

		/// <summary>
		/// 本地存储获取SSO Token的时间
		/// </summary>
		public static string LoginTime
		{
			get
			{
				return GetValue("chat_LoginTime");
			}
			set
			{
				SetValue("chat_LoginTime", value);
			}
		}

		/// <summary>
		/// 本地存储刷新 Token的时间
		/// </summary>
		public static string RefreshTokenTime
		{
			get
			{
				return GetValue("chat_RefreshTokenTime");
			}
			set
			{
				SetValue("chat_RefreshTokenTime", value);
			}
		}

		/// <summary>
		/// 本地存储刷新SSO Token的服务器地址
		/// </summary>
		public static string ReFreshTokenAddress
		{
			get
			{
				return GetValue("chat_ReFreshTokenAddress");
			}
			set
			{
				SetValue("chat_ReFreshTokenAddress", value);
			}
		}

		/// <summary>
		/// 本地存储的用户用于消息推送的Token
		/// </summary>
		public static string DeviceToken
		{
			get
			{
				return GetValue("chat_DeviceToken");
			}
			set
			{
				SetValue("chat_DeviceToken", value);
			}
		}

		/// <summary>
		/// 用于身份验证的Token
		/// </summary>
		public static string AppAuthToken
		{
			get
			{
				return GetValue("AppAuthToken");
			}
			set
			{
				SetValue("AppAuthToken", value);
			}
		}

		/// <summary>
		/// Base URL
		/// </summary>
		public static string AppApiBaseUrl
		{
			get
			{
				return GetValue("App_ApiBaseUrl");
			}
			set
			{
				SetValue("App_ApiBaseUrl", value);
			}
		}

		/// <summary>
		/// 是否启用消息通知
		/// </summary>
		public static bool IsNotified
		{
			get
			{
				var v = GetValue("chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotified");
				if (string.IsNullOrWhiteSpace(v))
					return true;

				return GetValue("chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotified") == "1";
			}
			set
			{
				SetValue("chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotified", value ? "1" : "0");
			}
		}

		/// <summary>
		/// 收到消息是否有声音提醒
		/// </summary>
		public static bool IsNotifiedVoice
		{
			get
			{
				var v = GetValue("chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotifiedVoice");
				if (string.IsNullOrWhiteSpace(v))
					return true;

				return GetValue("chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotifiedVoice") == "1";
			}
			set
			{
				SetValue("chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotifiedVoice", value ? "1" : "0");
			}
		}

		private static string IsNotifiedBeepKey
		{
			get
			{
				return "chat" + "_" + LocalDbVersion + "_" + UserCode + "_IsNotifiedBeep";
			}
		}

		/// <summary>
		/// 收到消息是否有震动
		/// </summary>
		public static bool IsNotifiedBeep
		{
			get
			{
				var v = GetValue(IsNotifiedBeepKey);
				if (string.IsNullOrWhiteSpace(v))
					return true;

				return GetValue(IsNotifiedBeepKey) == "1";
			}
			set
			{
				SetValue(IsNotifiedBeepKey, value ? "1" : "0");
			}
		}

		/// <summary>
		/// 菜单的样式，1代表列表，2 代表9宫格；默认是列表样式
		/// </summary>
		/// <value>The menu last update time.</value>
		public static string MenuStyle
		{
			get
			{
				var style = GetValue("menu" + "_" + UserCode + "_MenuStyle");
				if (string.IsNullOrWhiteSpace(style))
				{
					style = "1";
				}
				return style;
			}
			set
			{
				SetValue("menu" + "_" + UserCode + "_MenuStyle", value);
			}
		}

		/// <summary>
		/// 当前用户id
		/// </summary>
		/// <value>The menu last update time.</value>
		public static string UserId
		{
			get
			{
				return GetValue("chat_UserId");
			}
			set
			{
				SetValue("chat_UserId", value);
			}
		}

		/// <summary>
		/// Openfire聊天服务器的HostName
		/// </summary>
		public static string HostName
		{
			set
			{
				SetValue("chat_HostName", value);
			}
			get
			{
				var v = GetValue("chat_HostName");
				return string.IsNullOrWhiteSpace(v) ? "joe-nb-t430" : v;
			}
		}

		/// <summary>
		/// HTML5代码的版本号
		/// </summary>
		public static string IOSVersion
		{
			set
			{
				SetValue("IOSVersion", value);
			}
			get
			{
				var v = GetValue("IOSVersion");
				return string.IsNullOrWhiteSpace(v) ? "1.0.0.0" : v;
			}
		}

		/// <summary>
		/// HTML5代码的版本号
		/// </summary>
		public static string WwwVersion
		{
			set
			{
				SetValue("WwwVersion", value);
			}
			get
			{
				var v = GetValue("WwwVersion");
				return string.IsNullOrWhiteSpace(v) ? "1.0.0.0" : v;
			}
		}

		/// <summary>
		/// PMS HTML5代码的版本号
		/// </summary>
		public static string PMSWwwVersion
		{
			set
			{
				SetValue("PMSWwwVersion", value);
			}
			get
			{
				var v = GetValue("PMSWwwVersion");
				return string.IsNullOrWhiteSpace(v) ? "1.0.0.0" : v;
			}
		}

		/// <summary>
		/// PMSMenuUrl
		/// </summary>
		public static string PMSMenuUrl
		{
			get
			{
				return GetValue("PMSMenuUrl");
			}
			set
			{
				SetValue("PMSMenuUrl", value);
			}
		}

		/// <summary>
		/// PMS 模块是否更新
		/// </summary>
		public static bool IsPMSUpdate
		{
			set
			{
				SetValue("IsPMSUpdate", value.ToString());
			}
			get
			{
				var v = GetValue("IsPMSUpdate");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}

		/// <summary>
		/// 是否启用Debug模式，如果启用，则HTML5页面从服务器端获取
		/// </summary>
		public static bool IsHTML5Debug
		{
			set
			{
				SetValue("IsHTML5Debug", value.ToString());
			}
			get
			{
				var v = GetValue("IsHTML5Debug");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}



		/// <summary>
		/// 是否已记住密码
		/// </summary>
		public static bool IsRememberPassword
		{
			set
			{
				SetValue("IsRememberPassword", value.ToString());
			}
			get
			{
				var v = GetValue("IsRememberPassword");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}

		/// <summary>
		/// 是否是PMS用户
		/// </summary>
		public static bool IsPMSUser
		{
			set
			{
				SetValue("IsPMSUser", value.ToString());
			}
			get
			{
				var v = GetValue("IsPMSUser");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}

		/// <summary>
		/// 本地存储PMS类型
		/// </summary>
		public static string IsPmsType
		{
			get
			{
				return GetValue("IsPmsType");
			}
			set
			{
				SetValue("IsPmsType", value);
			}
		}

		/// <summary>
		/// 本地存储PermissionCode类型
		/// </summary>
		public static bool PermissionCode
		{
			set
			{
				SetValue("PermissionCode", value.ToString());
			}
			get
			{
				var v = GetValue("PermissionCode");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}

		/// <summary>
		/// 本地存储引导页更新时间
		/// </summary>
		public static string showTime
		{
			get
			{
				return GetValue("show_Time");
			}
			set
			{
				SetValue("show_Time", value);
			}
		}

		/// <summary>
		/// 本地存储引导页更新时间
		/// </summary>
		public static string showTiming
		{
			get
			{
				return GetValue("show_Time");
			}
			set
			{
				SetValue("show_Time", value);
			}
		}

		/// <summary>
		/// 本地存储 UserName
		/// </summary>
		public static string UserName
		{
			get
			{
				return GetValue("UserName");
			}
			set
			{
				SetValue("UserName", value);
			}
		}

		/// <summary>
		/// 本地存储用户是否启用 TouchID 登录
		/// </summary>
		public static bool isTouchID
		{
			set
			{
				SetValue("isTouchID", value.ToString());
			}
			get
			{
				var v = GetValue("isTouchID");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}

		/// <summary>
		/// 本地存储用户是否登录
		/// </summary>
		public static bool hasLogined
		{
			set
			{
				SetValue("hasLogined", value.ToString());
			}
			get
			{
				var v = GetValue("hasLogined");
				return string.IsNullOrWhiteSpace(v) ? false : bool.Parse(v);
			}
		}
	}
}

