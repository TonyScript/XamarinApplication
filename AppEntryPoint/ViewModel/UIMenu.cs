using System.Collections.Generic;

namespace Homeinns.Application.ViewModel
{
	/// <summary>
	/// 服务器端返回菜单数据模型
	/// </summary>
	public class UIMenu
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

		public string SystemMenuId { get; set; }

		public List<MenuModel> Children { get; set; }
	}

	public class MenuModel
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

		public string SystemMenuId { get; set; }

	}
}

