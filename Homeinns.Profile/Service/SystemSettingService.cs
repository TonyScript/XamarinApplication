using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homeinns.Common.Util;
using Homeinns.Common.Configuration;
using Homeinns.Profile.ViewModel;

namespace Homeinns.Profile.Service
{
	/// <summary>
	/// 系统设置相关的服务
	/// </summary>
	public static class SystemSettingService
	{
		private static Dictionary<string, Action> _cleanupActions = new Dictionary<string, Action>();

		public static void AddCleanupAction(string name, Action action)
		{
			if (!_cleanupActions.ContainsKey(name))
			{
				_cleanupActions.Add(name, action);
			}
			else {
				_cleanupActions[name] = action;
			}
		}

		public static void Cleanup()
		{
			AppGlobalSetting.WwwVersion = string.Empty;
			foreach (var key in _cleanupActions.Keys)
			{
				_cleanupActions[key]();
			}
		}

		/// <summary>
		/// 获取当前人员信息
		/// </summary>
		public static async Task<bool> TestConnect()
		{
			try
			{
				await RestClient.GetAsync("api/app/connect")
					.ConfigureAwait(continueOnCapturedContext: false);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 获取当前人员信息
		/// </summary>
		public static async Task SaveSuggest(SuggestionModel m)
		{
			try
			{
				await RestClient.PostAsync<SuggestionModel>("api/Setting/save", m)
					.ConfigureAwait(continueOnCapturedContext: false);

			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}
		}
	}
}
