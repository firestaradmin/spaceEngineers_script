using System;
using System.Net;
using System.Text;
using VRage.Http;
using VRage.Utils;

namespace VRage.Analytics
{
	public class MyElasticAnalytics : MyAnalyticsBase
	{
		private readonly string m_apiKey;

		private readonly string m_apiUrl;

		public MyElasticAnalytics(string apiUrl, string apiKeyId, string apiKey, string eventStoragePath, int maxStoredEvents)
			: base(eventStoragePath, maxStoredEvents)
		{
			m_apiKey = "ApiKey " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKeyId + ":" + apiKey));
			m_apiUrl = apiUrl;
			ReportPostponedEvents();
		}

		protected override void ReportEvent(MyEvent myEvent)
		{
			SendEventToElasticOrStore(myEvent);
		}

		private void SendEventToElasticOrStore(MyEvent eventToSend)
		{
			string value = eventToSend.ToJSON();
			HttpData[] parameters = new HttpData[3]
			{
				new HttpData("Authorization", m_apiKey, HttpDataType.HttpHeader),
				new HttpData("Content-Type", "application/json", HttpDataType.HttpHeader),
				new HttpData("application/json", value, HttpDataType.RequestBody)
			};
			MyLog.Default.WriteLine("Sending event to ElasticSearch: " + eventToSend.EventName);
			MyVRage.Platform.Http.SendRequestAsync(m_apiUrl, parameters, HttpMethod.POST, delegate(HttpStatusCode code, string response)
			{
<<<<<<< HEAD
				if (code != HttpStatusCode.OK && code != HttpStatusCode.Created)
=======
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Invalid comparison between Unknown and I4
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Invalid comparison between Unknown and I4
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if ((int)code != 200 && (int)code != 201)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyLog.Default.WriteLine($"Elastic http response error {code}: {response}");
					StoreUnsentEvent(eventToSend, eventToSend.EventTimestamp);
				}
			});
		}
	}
}
