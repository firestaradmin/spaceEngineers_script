using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using RestSharp;
using VRage.Http;

namespace VRage.Platform.Windows.Http
{
	internal sealed class MyWindowsHttpClient : IVRageHttp
	{
		private MyVRagePlatform m_platform;

		private readonly RestClient m_restClient = new RestClient();

		public MyWindowsHttpClient(MyVRagePlatform platform)
		{
			m_platform = platform;
			m_restClient.Encoding = Encoding.UTF8;
		}

		private Method GetRestMethod(HttpMethod method)
		{
			switch (method)
			{
			case HttpMethod.GET:
				return Method.GET;
			case HttpMethod.POST:
				return Method.POST;
			case HttpMethod.PUT:
				return Method.PUT;
			case HttpMethod.DELETE:
				return Method.DELETE;
			default:
				return Method.GET;
			}
		}

		public void DownloadAsync(string url, string filename, Action<ulong> onProgress, Action<HttpStatusCode> action)
		{
			FileStream ws = File.Create(filename);
			RestRequest restRequest = new RestRequest(url, Method.GET);
			restRequest.ResponseWriter = (Action<Stream>)Delegate.Combine(restRequest.ResponseWriter, (Action<Stream>)delegate(Stream s)
			{
				OnDownloadWriter(s, ws, onProgress);
			});
			m_restClient.ExecuteAsync(restRequest, delegate(IRestResponse x)
			{
				action.InvokeIfNotNull(x.StatusCode);
			});
		}

		private static void OnDownloadWriter(Stream stream, Stream outputStream, Action<ulong> onProgress)
		{
			using (stream)
			{
				byte[] array = new byte[4096];
				while (true)
				{
					int num = stream.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					outputStream.Write(array, 0, num);
					onProgress.InvokeIfNotNull((ulong)num);
				}
				outputStream.Dispose();
			}
		}

		public HttpStatusCode SendRequest(string url, HttpData[] parameters, HttpMethod method, out string content)
		{
			HttpStatusCode tempCode = HttpStatusCode.BadRequest;
			string tempContent = null;
			bool done = false;
			SendRequestAsync(url, parameters, method, delegate(HttpStatusCode c, string s)
			{
				tempCode = c;
				tempContent = s;
				done = true;
			});
			while (!done)
			{
				Thread.Sleep(10);
			}
			content = tempContent;
			return tempCode;
		}

		public void SendRequestAsync(string url, HttpData[] parameters, HttpMethod method, Action<HttpStatusCode, string> onDone)
		{
			try
			{
				RestRequest restRequest = new RestRequest(url, GetRestMethod(method));
				if (parameters != null)
				{
					for (int i = 0; i < parameters.Length; i++)
					{
						HttpData httpData = parameters[i];
						switch (httpData.Type)
						{
						case HttpDataType.Filename:
							restRequest.AddFile(httpData.Name, (string)httpData.Value);
							break;
						case HttpDataType.GetOrPost:
							restRequest.AddParameter(httpData.Name, httpData.Value, ParameterType.GetOrPost);
							break;
						case HttpDataType.HttpHeader:
							restRequest.AddParameter(httpData.Name, httpData.Value, ParameterType.HttpHeader);
							break;
						case HttpDataType.RequestBody:
							restRequest.AddParameter(httpData.Name, httpData.Value, ParameterType.RequestBody);
							break;
						default:
							onDone(HttpStatusCode.BadRequest, null);
							return;
						}
					}
				}
				m_restClient.ExecuteAsync(restRequest, delegate(IRestResponse x)
				{
					OnRequestComplete(x, onDone);
				});
			}
			catch (Exception)
			{
				onDone(HttpStatusCode.BadRequest, null);
			}
		}

		private void OnRequestComplete(IRestResponse response, Action<HttpStatusCode, string> onDone)
		{
			string arg = MyHttpTools.ConvertToString(response.RawBytes, response.ContentEncoding);
			onDone.InvokeIfNotNull(response.StatusCode, arg);
		}
	}
}
