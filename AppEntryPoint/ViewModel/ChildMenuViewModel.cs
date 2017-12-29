using System;
using SQLite;

namespace Homeinns.Application.ViewModel
{
	/// <summary>
	/// 存在本地数据库子菜单数据模型
	/// </summary>
	public class ChildMenuViewModel
	{
		public int IsActive { get; set; }

		public string MenuCode { get; set; }

		public string MenuIcon { get; set; }

		public string MenuName { get; set; }

		public int MenuSeq { get; set; }

		public string MenuType { get; set; }

		public string MenuUrl { get; set; }

		public string ParentMenuCode { get; set; }

		public string ParentMenuId { get; set; }

		public int MenuBadge { get; set; }

		[PrimaryKey]
		public string SystemMenuId { get; set; }
	}
}

