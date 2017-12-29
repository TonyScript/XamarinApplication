using System;
using SQLite;

namespace Homeinns.Application.ViewModel
{
	public class SystemMenuSettingViewModel
	{
		[PrimaryKey]
		public string Id { get; set; }

		public string SystemMenuSyncTime { get; set; }
	}
}
