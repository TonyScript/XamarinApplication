namespace Homeinns.Common.ViewModel
{
	/// <summary>
	/// 返回的版本更新类
	/// </summary>
	public class VersionUpdateModel
	{
		/// <summary>
		/// 是否需要升级
		/// </summary>
		public bool IsUpdate { get; set; }

		/// <summary>
		/// 是否强制升级
		/// </summary>
		public bool IsForceUpdate { get; set; }

		/// <summary>
		/// 附件Id
		/// </summary>
		public string FileId { get; set; }

		/// <summary>
		/// 更新说明
		/// </summary>
		public string UpdateComment { get; set; }

		/// <summary>
		/// 版本编号
		/// </summary>
		public string VersionCode { get; set; }
	}
}

