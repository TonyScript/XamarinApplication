
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homeinns.Application.ViewModel;
using Homeinns.Common.Configuration;
using Homeinns.Common.Util;
using Homeinns.Profile.Service;

namespace Homeinns.Application.Service
{
	public static class MenuService
	{
		/// <summary>
		/// 服务器端获取菜单
		/// </summary>
		private static async Task<MobileSystemMenuModel> GetMenu(string lastUpdateTime)
		{
			var apiUrl = string.Format("api/SystemMenu/GetMobileSystemMenu?lastQueryTime={0}", lastUpdateTime);
			return await RestClient.GetAsync<MobileSystemMenuModel>(apiUrl)
				.ConfigureAwait(continueOnCapturedContext: false);
		}

		private static async Task<List<SystemMenuBadgeModel>> GetBadges()
		{
			var apiUrl = "api/SystemMenu/GetBadges";
			return await RestClient.GetAsync<List<SystemMenuBadgeModel>>(apiUrl);
		}

		/// <summary>
		/// 传最后一次更新时间，先调接口获取数据，如果传回来是空则表示不需要更新菜单，从数据库获取数据，
		/// 如果接口返回数据不为空，先将数据库原有数据删除，将接口数据插入数据库，再从数据库获取菜单；
		/// </summary>
		public static async Task LoadMenusToSqlLite()
		{
			try
			{
				SystemSettingService.AddCleanupAction("menu_last_sync_time_clear", () =>
				{
					MenuDataRepository.ClearLastSyncTime();
				});
				//从服务器端抓取菜单数据
				var mobileMenu = await GetMenu(MenuDataRepository.GetLastSyncTime());
				var mobileBadges = await GetBadges();

				AppGlobalSetting.MenuStyle = mobileMenu.SystemMenuStyle;

				var menus = mobileMenu.SystemMenuList;

				//menus不为空则表示需要更新菜单，先清除数据库表数据，再将数据插入数据库表，在从数据库表中获取菜单
				if (menus != null && mobileMenu.SystemMenuList.Count >= 0)
				{
					//待保存的父菜单组
					List<ParentMenuViewModel> parentList = new List<ParentMenuViewModel>();
					//带保存的子菜单
					List<ChildMenuViewModel> childList = new List<ChildMenuViewModel>();

					#region 循环给ParentMenuViewModel和ChildMenuViewModel赋值 加入list
					foreach (var menu in menus)
					{
						ParentMenuViewModel parent = new ParentMenuViewModel()
						{
							IsActive = menu.IsActive,
							MenuCode = menu.MenuCode,
							MenuIcon = menu.MenuIcon,
							MenuName = menu.MenuName,
							MenuSeq = menu.MenuSeq,
							MenuType = menu.MenuType,
							MenuUrl = menu.MenuUrl,
							ParentMenuCode = menu.ParentMenuCode,
							ParentMenuId = menu.ParentMenuId,
							SystemMenuId = menu.SystemMenuId
						};
						parentList.Add(parent);
						if (menu.Children == null)
							continue;
						foreach (var childMenu in menu.Children)
						{
							ChildMenuViewModel child = new ChildMenuViewModel
							{
								IsActive = childMenu.IsActive,
								MenuCode = childMenu.MenuCode,
								MenuIcon = childMenu.MenuIcon,
								MenuName = childMenu.MenuName,
								MenuSeq = childMenu.MenuSeq,
								MenuType = childMenu.MenuType,
								MenuUrl = childMenu.MenuUrl,
								ParentMenuCode = childMenu.ParentMenuCode,
								ParentMenuId = childMenu.ParentMenuId,
								SystemMenuId = childMenu.SystemMenuId
							};

							//设置菜单的badge
							var badge = mobileBadges.Find(c => c.SystemMenuCode == childMenu.MenuCode);
							if (badge != null)
							{
								child.MenuBadge = badge.SystemMenuBadge;
							}

							childList.Add(child);
						}
					}
					#endregion
					MenuDataRepository.SyncSystemMenus(parentList, childList, mobileMenu.SystemMenuSyncTime);
				}
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}
		}

		/// <summary>
		/// 从服务器端加载菜单的Badge信息，写入到sqlite数据库
		/// </summary>
		/// <returns>The bages to sql lite.</returns>
		public static async Task LoadMenuBagesToSqlLite()
		{
			try
			{
				var mobileBadges = await GetBadges();
				mobileBadges.ForEach(b =>
				{
					var menu = MenuDataRepository.GetChildMenuByCode(b.SystemMenuCode);
					if (menu == null)
					{
						return;
					}

					menu.MenuBadge = b.SystemMenuBadge;
					MenuDataRepository.AddOrUpdate(menu);
				});
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
			}
		}
	}
}
