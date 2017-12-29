using System.Diagnostics;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 网络相关的工具类
	/// </summary>
	public static class NetworkUtil
	{
		private static Reachability _networkReachability;
		private static object _networkReachabilityLocker = new object();

		/// <summary>
		/// 根据IP地址检查网络是否联通
		/// </summary>
		/// <param name="ip">IP地址</param>
		public static void CheckNetworkReachable()
		{
			lock (_networkReachabilityLocker)
			{
				if (_networkReachability == null)
				{
					_networkReachability = new Reachability("www.baidu.com");
					_networkReachability.ReachabilityUpdated += (sender, e) =>
					{
						var netTypeString = _networkReachability.GetReachabilityString();
						Debug.WriteLine("当前网络连接类型：[" + netTypeString + "]");
					};
				}
			}
		}
	}
}

