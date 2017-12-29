using System;
using System.Collections.Generic;
using System.Linq;
using Homeinns.Application.ViewModel;
using Homeinns.Common.Service;

namespace Homeinns.Application.Service
{
	/// <summary>
	/// 菜单的本地数据库存储实现类
	/// </summary>
	public static class MenuDataRepository
	{
		static MenuDataRepository()
		{
			SqlDataRepository.CreateTable<ParentMenuViewModel>();
			SqlDataRepository.CreateTable<ChildMenuViewModel>();
			SqlDataRepository.CreateTable<SystemMenuSettingViewModel>();
		}

		#region menu

		public static string GetLastSyncTime()
		{
			if (!SqlDataRepository.IsOpened)
				return string.Empty;

			var setting = SqlDataRepository.Table<SystemMenuSettingViewModel>()
					.FirstOrDefault();
			if (setting == null)
				return string.Empty;

			return setting.SystemMenuSyncTime;
		}

		public static void ClearLastSyncTime()
		{
			if (!SqlDataRepository.IsOpened)
				return;

			var setting = SqlDataRepository.Table<SystemMenuSettingViewModel>()
				.FirstOrDefault();
			if (setting == null)
				return;

			setting.SystemMenuSyncTime = string.Empty;
			SqlDataRepository.Update(setting);
		}

		/// <summary>
		/// 初始化从头服务器端下载的所有的菜单
		/// </summary>
		public static void SyncSystemMenus(List<ParentMenuViewModel> pMenus, List<ChildMenuViewModel> cMenus, string syncTime)
		{
			if (!SqlDataRepository.IsOpened || pMenus == null || pMenus.Count == 0)
				return;

			try
			{
				SqlDataRepository.BeginTransaction();

				#region Clear ParentMenus
				var existsParentMenus = SqlDataRepository.Table<ParentMenuViewModel>().ToList();
				foreach (var menu in existsParentMenus)
				{
					var existsMenu = SqlDataRepository.Table<ParentMenuViewModel>()
								.AsQueryable()
								.FirstOrDefault(m => m.SystemMenuId == menu.SystemMenuId);

					if (existsMenu != null)
					{
						SqlDataRepository.Delete(menu);
					}
				}

				var existsChildMenus = SqlDataRepository.Table<ChildMenuViewModel>().ToList();
				foreach (var menu in existsChildMenus)
				{
					var existsMenu = SqlDataRepository.Table<ChildMenuViewModel>()
							.AsQueryable()
							.FirstOrDefault(m => m.SystemMenuId == menu.SystemMenuId);

					if (existsMenu != null)
					{
						SqlDataRepository.Delete(menu);
					}
				}
				#endregion

				#region load menus from api to sqlite
				foreach (var menu in pMenus)
				{

					var existsMenu = SqlDataRepository.Table<ParentMenuViewModel>()
							.AsQueryable()
							.FirstOrDefault(m => m.SystemMenuId == menu.SystemMenuId);

					if (existsMenu == null)
					{
						SqlDataRepository.Insert(menu);
					}
					else {
						SqlDataRepository.Update(menu);
					}

				}

				if (cMenus != null && cMenus.Count > 0)
				{

					foreach (var menu in cMenus)
					{

						var existsMenu = SqlDataRepository.Table<ChildMenuViewModel>()
							.AsQueryable()
							.FirstOrDefault(m => m.SystemMenuId == menu.SystemMenuId);

						if (existsMenu == null)
						{
							SqlDataRepository.Insert(menu);
						}
						else {
							SqlDataRepository.Update(menu);
						}
					}


				}
				#endregion

				#region Save Sync Time
				var setting = SqlDataRepository.Table<SystemMenuSettingViewModel>()
						.FirstOrDefault();
				if (setting == null)
				{
					SqlDataRepository.Insert(new SystemMenuSettingViewModel
					{
						Id = Guid.NewGuid().ToString(),
						SystemMenuSyncTime = syncTime
					});
				}
				else {
					setting.SystemMenuSyncTime = syncTime;
					SqlDataRepository.Update(setting);
				}
				#endregion

				SqlDataRepository.Commit();
			}
			catch (Exception)
			{
				SqlDataRepository.Rollback();
				throw;
			}
		}

		/// <summary>
		/// 更新菜单数据
		/// </summary>
		public static void AddOrUpdate(ChildMenuViewModel menu)
		{
			if (!SqlDataRepository.IsOpened || menu == null)
				return;

			var existsMenu = SqlDataRepository.Table<ChildMenuViewModel>()
					.AsQueryable()
					.FirstOrDefault(m => m.SystemMenuId == menu.SystemMenuId);

			if (existsMenu == null)
			{
				SqlDataRepository.Insert(menu);
			}
			else {
				SqlDataRepository.Update(menu);
			}

			SqlDataRepository.Commit();
		}

		/// <summary>
		/// 从数据库表中获取父菜单
		/// </summary>
		/// <returns>The menu.</returns>
		public static List<ParentMenuViewModel> GetParentMenu()
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ParentMenuViewModel>(0);

			return SqlDataRepository.Table<ParentMenuViewModel>().ToList();
		}

		/// <summary>
		/// 从数据库表中获取子菜单
		/// </summary>
		/// <returns>The menu.</returns>
		public static List<ChildMenuViewModel> GetChildMenu()
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChildMenuViewModel>(0);

			return SqlDataRepository.Table<ChildMenuViewModel>().ToList();
		}

		/// <summary>
		/// 从数据库表中根据父菜单id获取子菜单
		/// </summary>
		/// <returns>The menu.</returns>
		public static List<ChildMenuViewModel> GetChildMenuByParentMenuId(string parentId)
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChildMenuViewModel>(0);

			return SqlDataRepository.Table<ChildMenuViewModel>().Where(m => m.ParentMenuId == parentId).ToList();
		}

		/// <summary>
		/// 从数据库表中根据父菜单id获取子菜单
		/// </summary>
		/// <returns>The menu.</returns>
		public static ChildMenuViewModel GetChildMenuByCode(string code)
		{
			if (!SqlDataRepository.IsOpened)
				return null;

			return SqlDataRepository.Table<ChildMenuViewModel>()
					.FirstOrDefault(m => m.MenuCode == code);
		}

		#endregion

	}
}

