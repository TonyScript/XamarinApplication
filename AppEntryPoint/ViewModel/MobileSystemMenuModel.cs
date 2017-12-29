using System.Collections.Generic;

namespace Homeinns.Application.ViewModel
{
	/// <summary>
	/// 移动菜单的模型结构
	/// </summary>
	public class MobileSystemMenuModel
	{
		/// <summary>
		/// 菜单的样式
		/// </summary>
		public string SystemMenuStyle { get; set; }

		/// <summary>
		/// 菜单的列表
		/// </summary>
		public List<UIMenu> SystemMenuList { get; set; }

		/// <summary>
		/// 服务器端返回的菜单的上次修改时间
		/// </summary>
		/// <value>The system menu sync time.</value>
		public string SystemMenuSyncTime { get; set; }
	}
}

