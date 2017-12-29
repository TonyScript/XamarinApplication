using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Homeinns.Common.Configuration;
using RestSharp;
using RestSharp.Deserializers;
using Homeinns.Common.Util;

namespace RekTec.Corelib.Rest
{
	/// <summary>
	/// WebAPI基础操作类型
	/// </summary>
	public static class RestClientForHotel
	{
		private const int ConnectionTimeOut = 30 * 1000;

		private static void TryConnection(string url)
		{
			//ObjCRuntime.Runtime.StartWWAN(new Uri(url));
		}

		private static void CheckIfFailure(IRestResponse response)
		{
			if (response.StatusCode == System.Net.HttpStatusCode.OK
				||
				response.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				return;
			}

			if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
			{
				throw new Exception(response.Content);
			}

			if (response.ResponseStatus != ResponseStatus.Completed)
			{
				TryConnection(AppGlobalSetting.AppApiBaseUrl);
				throw new Exception("[网络]连接异常，请检查网络设置或者重试！");
			}

			throw new Exception("[网络]连接异常，请检查网络设置或者重试！" + response.StatusCode);
		}

		private static T Deserialize<T>(IRestResponse response)
		{
			if (string.IsNullOrEmpty(response.Content) || response.Content == "null")
				return default(T);
			else {
				var deserializers = new JsonDeserializer();
				return deserializers.Deserialize<T>(response);
			}
		}

		private static RestSharp.RestClient GetRestClient()
		{
			NetworkUtil.CheckNetworkReachable();
			TryConnection(AppGlobalSetting.PmsServerUrl);

			var client = new RestSharp.RestClient(AppGlobalSetting.PmsServerUrl);
			//if (!string.IsNullOrEmpty (GlobalAppSetting.XrmAuthToken))
			//	client.AddDefaultHeader ("Authorization", "Basic " + GlobalAppSetting.XrmAuthToken);

			client.Timeout = ConnectionTimeOut;
			return client;
		}

		/// <summary>
		/// 异步Post的Http请求，返回Task
		/// </summary>
		/// <typeparam name="T">返回的对象类型</typeparam>
		/// <param name="url">WebAPI的Url地址</param>
		/// <returns>返回服务器端的Model对象</returns>
		public static async Task<T> PostAsync<T>(string url)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.POST);
			Debug.WriteLine("POST Request: " + url);
			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("POST Response: " + resp.Content);
			CheckIfFailure(resp);

			return Deserialize<T>(resp);
		}

		/// <summary>
		/// 异步Post的Http请求，返回Task
		/// </summary>
		/// <param name="url">WebAPI的Url地址</param>
		/// <returns>没有返回值</returns>
		public static async Task PostAsync(string url)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.POST);
			Debug.WriteLine("POST Request: " + url);
			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("POST Response: " + resp.Content);
			CheckIfFailure(resp);
		}

		/// <summary>
		/// 异步Post的Http请求，返回Task
		/// </summary>
		/// <typeparam name="T">返回的对象类型</typeparam>
		/// <param name="url">WebAPI的Url地址</param>
		/// <param name="data">要Post到服务器的对象</param>
		/// <returns>返回服务器端的Model对象</returns>
		public static async Task<T> PostAsync<T>(string url, object data)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.POST);
			if (data != null)
			{
				request.RequestFormat = DataFormat.Json;
				request.AddBody(data);
			}
			Debug.WriteLine("POST Request: " + url);
			if (data != null)
				Debug.WriteLine("POST Data: " + data);
			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("POST Response: " + resp.Content);
			CheckIfFailure(resp);

			return Deserialize<T>(resp);
		}

		/// <summary>
		/// 异步Post上传文件
		/// </summary>
		/// <param name="url">上传文件的URL地址</param>
		/// <param name="id">此文件关联的ObjectId</param>
		/// <param name="datas">文件的二进制数组</param>
		/// <returns>返回服务器端的Model对象</returns>
		public static async Task<string> PostFileAsync(string url, string id, byte[] datas)
		{
			if (string.IsNullOrWhiteSpace(id) || datas == null)
				return string.Empty;

			var client = GetRestClient();
			var request = new RestRequest(url, Method.POST);
			request.AddFile(id, datas, id);
			Debug.WriteLine("POST Request: " + url + "/" + id);

			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("POST Response: " + resp.Content);
			CheckIfFailure(resp);

			var fileId = resp.Content;
			if (fileId.StartsWith("\""))
				fileId = fileId.Substring(1);
			if (fileId.EndsWith("\""))
				fileId = fileId.Substring(0, fileId.Length - 1);

			return fileId;
		}

		/// <summary>
		///异步下载文件
		/// </summary>
		/// <param name="url">文件下载的URL地址</param>
		/// <returns>文件的二进制数组</returns>
		public static async Task<byte[]> DownloadFileAsync(string url)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.GET);
			Debug.WriteLine("POST Request: " + url);

			var resp = await client.ExecuteGetTaskAsync(request);
			Debug.WriteLine("POST Response: " + resp.Content);
			CheckIfFailure(resp);

			return resp.RawBytes;
		}

		public static async void DownloadFileAsync(string url, Action<byte[]> success, Action<string> failure)
		{
			try
			{
				var client = GetRestClient();
				var request = new RestRequest(url, Method.GET);
				Debug.WriteLine("POST Request: " + url);

				var resp = await client.ExecuteGetTaskAsync(request);
				Debug.WriteLine("POST Response: " + resp.Content);
				CheckIfFailure(resp);

				if (success != null)
				{
					success(resp.RawBytes);
				}
			}
			catch (Exception ex)
			{
				if (failure != null)
				{
					failure(ex.Message);
				}
			}
		}

		/// <summary>
		/// Post Http请求，没有返回值
		/// </summary>
		/// <param name="url">WebAPI的Url地址</param>
		/// <param name="data">要Post到服务器端的对象</param>
		/// <returns></returns>
		public static async Task<string> PostAsync(string url, object data)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.POST);
			if (data != null)
			{
				request.RequestFormat = DataFormat.Json;
				request.AddBody(data);
			}
			Debug.WriteLine("POST Request: " + url);
			if (data != null)
				Debug.WriteLine("POST Data: " + data);
			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("POST Response: " + resp.Content);
			CheckIfFailure(resp);

			return resp.Content;
		}

		/// <summary>
		/// Get Http 请求
		/// </summary>
		/// <typeparam name="T">服务器端返回的对象类型</typeparam>
		/// <param name="url">WebAPI的Url地址</param>
		/// <returns>服务器端返回的对象</returns>
		public static async Task<T> GetAsync<T>(string url)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.GET);
			Debug.WriteLine("GET Request: " + url);
			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("GET Response: " + resp.Content);
			CheckIfFailure(resp);

			return Deserialize<T>(resp);
		}

		/// <summary>
		/// Get Http 请求
		/// </summary>
		/// <param name="url">WebAPI的Url地址</param>
		/// <returns>服务器端返回的字符串</returns>
		public static async Task<string> GetAsync(string url)
		{
			var client = GetRestClient();
			var request = new RestRequest(url, Method.GET);
			Debug.WriteLine("GET Request: " + url);
			var resp = await client.ExecuteTaskAsync(request);
			Debug.WriteLine("GET Response: " + resp.Content);
			CheckIfFailure(resp);

			return resp.Content;
		}
	}
}

