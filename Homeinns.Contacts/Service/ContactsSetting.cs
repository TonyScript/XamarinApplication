using Homeinns.Common.Configuration;

namespace Homeinns.Contacts.Service
{
	/// <summary>
	///与通讯录相关的设置项目 
	/// </summary>
	public static class ContactsAppSetting
	{
		/// <summary>
		/// 联系人上次更新的时间
		/// </summary>
		public static string ContactsLastUpdateTime
		{
			get
			{
				return AppGlobalSetting.GetValue("contacts_" + AppGlobalSetting.LocalDbVersion + "_" + AppGlobalSetting.UserCode + "_ContactsLastUpdateTime");
			}
			set
			{
				AppGlobalSetting.SetValue("contacts_" + AppGlobalSetting.LocalDbVersion + "_" + AppGlobalSetting.UserCode + "_ContactsLastUpdateTime", value);
			}
		}

	}
}
