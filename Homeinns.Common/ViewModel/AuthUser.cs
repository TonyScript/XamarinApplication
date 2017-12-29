using System;
using System.Collections.Generic;

namespace Homeinns.Common.ViewModel
{
	/// <summary>
	/// 登录返回的对象
	/// </summary>
	public class AuthUser
	{
		/// <summary>
		/// token
		/// </summary>
		/// <value>The auth token.</value>
		public string AuthToken { get; set; }

		/// <summary>
		/// 用户类型
		/// </summary>
		/// <value>The type of the auth.</value>
		public int AuthType { get; set; }

		/// <summary>
		/// 友好名称
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string FriendlyName { get; set; }

		/// <summary>
		/// 是否首次登录
		/// </summary>
		/// <value>The name of the friendly.</value>
		public Boolean IsLoginFirst { get; set; }

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

		/// <summary>
		/// SSO Token
		/// </summary>
		/// <value>The SSO Token.</value>
		public AuthToken SSOToken { get; set; }
	}
	public class AuthToken
	{
		public string TokenType { get; set; }
		public string AccessToken { get; set; }
		public int ExpiresIn { get; set; }
		public string RefreshToken { get; set; }
	}
	public class HotelCode
	{
		public string IsError { get; set; }
		public string ErrorCode { get; set; }
		public string ErrorMsg { get; set; }
		public HotelCodeData Data { get; set; }
	}
	public class HotelCodeData
	{
		public string UserID { get; set; }
		public string UserName { get; set; }
		public string JobTitle { get; set; }
		public string JobID { get; set; }
		public List<HotelList> Hotels { get; set; }
	}
	public class HotelList
	{
		public string UserID { get; set; }
		public string HotelCd { get; set; }
		public string HotelName { get; set; }
		public string BrandCd { get; set; }
		public string BrandDesc { get; set; }
	}
}
