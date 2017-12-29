using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Homeinns.Common.Configuration;
using Homeinns.Common.Service;
using Homeinns.Common.Util;
using Homeinns.Contacts.ViewModel;

namespace Homeinns.Contacts.Service
{
	public static class ContactService
	{
		private static async Task<UIContact> GetRosters(string lastUpdateTime)
		{
			var apiUrl = string.Format("api/contact/basic?lastUpdateTime={0}", lastUpdateTime);
			return await RestClient.GetAsync<UIContact>(apiUrl)
				.ConfigureAwait(continueOnCapturedContext: false);
		}

		/// <summary>
		/// 获取联系人头像
		/// </summary>
		private static async Task<List<UiRoster>> GetAvatars(string[] userArray)
		{
			var rostersList = await RestClient.PostAsync<List<UiRoster>>("api/contact/avatar", userArray)
				.ConfigureAwait(continueOnCapturedContext: false);
			return rostersList;
		}

		//同步通讯录的间隔
		private const int _syncContactTimeSpan = 30 * 60 * 1000;

		static Timer _syncTimer;
		/// <summary>
		/// 获取所有联系人
		/// </summary>
		/// <returns>The all contacts.</returns>
		private static async Task<bool> GetAllContacts()
		{
			var _foreachtime = 0;
			try
			{
				//最后更新的时间
				var lastUpdate = ContactsAppSetting.ContactsLastUpdateTime;

				//从通讯录接口获取到的联系人
				var rosterSync = await GetRosters(lastUpdate);

				if (rosterSync == null)
				{
					return true;
				}

				if (rosterSync.Contacts == null || rosterSync.Contacts.Count <= 0)
				{
					return true;
				}

				var l = new List<ContactViewModel>(rosterSync.Contacts.Count);
				rosterSync.Contacts.ForEach(r =>
				{
					_foreachtime += 1;
					var jid = r.Code + "@" + AppGlobalSetting.HostName;
					var c = ContactsDataRepository.GetContactById(jid);
					if (r.IsDisabled && c != null)
					{
						ContactsDataRepository.Delete(c);
					}
					else {
						l.Add(new ContactViewModel
						{
							XrmUserId = r.Id,
							ContactId = r.Code + "@" + AppGlobalSetting.HostName,
							ContactName = r.Name,
							Phone = r.Telephone,
							Department = r.BusinessName,
							Position = r.Position,
							Email = r.EmailAddress,
							AvatarImageBase64String = r.Avatar,
							IsDisabled = r.IsDisabled,
							IsUpdate = false
						});
					}
				});
				ContactsDataRepository.AddOrUpdate(l);

				//从接口返回的数据插入数据库中更新最后更新时间
				if (_foreachtime == rosterSync.Contacts.Count)
				{
					ContactsAppSetting.ContactsLastUpdateTime = rosterSync.SysnTime;
				}

				#region 更新头像
				//获取本地所有未更新头像的用户
				List<ContactViewModel> contacts = ContactsDataRepository.GetUnUpateContact();
				if (contacts != null && contacts.Count > 0)
				{
					int index = 0;
					List<string> userList = new List<string>();
					foreach (var contact in contacts)
					{
						if (!AuthenticationService.IsLogOn())
							break;

						userList.Add(contact.XrmUserId);
						index = index + 1;
						if (index % 10 == 0 || index == contacts.Count)
						{
							try
							{
								var contactList = await GetAvatars(userList.ToArray());
								ContactsDataRepository.UpDateAvatr(contactList);
								//清空list中所有的数据
								userList.Clear();
							}
							catch (Exception ex)
							{
								ErrorHandlerUtil.ReportException(ex);
							}
						}
					}
				}
				#endregion

				return true;
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return false;
			}
		}

		/// <summary>
		/// 开始同步联系人
		/// </summary>
		public static void StartSyncContact()
		{
			try
			{
				_syncTimer = null;
				_syncTimer = new Timer(SyncContactTimerCallback, null, 0, _syncContactTimeSpan);
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}

		}

		/// <summary>
		/// 停止同步联系人
		/// </summary>
		public static void StopSyncContact()
		{
			try
			{
				if (_syncTimer != null)
					_syncTimer.Dispose();

				_syncTimer = null;
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}

		}

		/// <summary>
		/// 联系人timer回调函数
		/// </summary>
		/// <param name="state">State.</param>
		private static void SyncContactTimerCallback(object state)
		{
			var t = GetAllContacts();
			t.Wait();
		}

	}
}
