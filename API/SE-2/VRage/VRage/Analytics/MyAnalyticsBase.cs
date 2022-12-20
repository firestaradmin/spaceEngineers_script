using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using LitJson;

namespace VRage.Analytics
{
	public abstract class MyAnalyticsBase : IMyAnalytics
	{
		protected class MyEvent
		{
			public string EventName { get; set; }

			public DateTime EventTimestamp { get; set; }

			public string UserID { get; set; }

			public string SessionID { get; set; }

			public string Platform { get; set; }

			public string ClientVersion { get; set; }

			public string Exception { get; set; }

			public MyReportTypeData ReportTypeAndArgs { get; set; }

			public Dictionary<string, object> P { get; set; }

			public string ToJSON()
			{
				JsonMapper.RegisterExporter(delegate(DateTime value, JsonWriter writer)
				{
					writer.Write(value.ToString("o", CultureInfo.InvariantCulture));
				});
				try
				{
					return JsonMapper.ToJson(this);
				}
				finally
				{
					JsonMapper.UnregisterExporters();
				}
			}
		}

		private MyObjectFileStorage m_eventStorage;

		protected MyAnalyticsBase(string eventStoragePath, int maxStoredEvents = -1)
		{
			if (eventStoragePath != null)
			{
				m_eventStorage = new MyObjectFileStorage(eventStoragePath, maxStoredEvents);
			}
		}

		protected abstract void ReportEvent(MyEvent myEvent);

		protected bool StoreUnsentEvent(MyEvent eventToStore, DateTime timestamp)
		{
			return m_eventStorage?.StoreObject(eventToStore, timestamp) ?? false;
		}

		public void ReportEvent(IMyAnalyticsEvent analyticsEvent, DateTime timestamp, string sessionID, string userID, string clientVersion, string platform, Exception exception = null)
		{
			MyEvent myEvent = new MyEvent
			{
				EventName = analyticsEvent.GetEventName(),
				EventTimestamp = timestamp,
				SessionID = sessionID,
				UserID = userID,
				ClientVersion = clientVersion,
				Platform = platform,
				Exception = BuildExceptionStackString(exception),
				ReportTypeAndArgs = analyticsEvent.GetReportTypeAndArgs(),
				P = analyticsEvent.GetPropertiesDictionary()
			};
			ReportEvent(myEvent);
		}

		public void ReportEventLater(IMyAnalyticsEvent analyticsEvent, DateTime timestamp, string sessionID, string userID, string clientVersion, string platform, Exception exception = null)
		{
			MyEvent eventToStore = new MyEvent
			{
				EventName = analyticsEvent.GetEventName(),
				EventTimestamp = timestamp,
				SessionID = sessionID,
				UserID = userID,
				ClientVersion = clientVersion,
				Platform = platform,
				Exception = BuildExceptionStackString(exception),
				ReportTypeAndArgs = analyticsEvent.GetReportTypeAndArgs(),
				P = analyticsEvent.GetPropertiesDictionary()
			};
			StoreUnsentEvent(eventToStore, timestamp);
		}

		public void ReportPostponedEvents()
		{
			foreach (MyEvent andWipeUnsentEvent in GetAndWipeUnsentEvents())
			{
				ReportEvent(andWipeUnsentEvent);
			}
		}

		private List<MyEvent> GetAndWipeUnsentEvents()
		{
			return m_eventStorage?.RetrieveStoredObjectsByType<MyEvent>(shouldWipeAfter: true) ?? new List<MyEvent>();
		}

		private string BuildExceptionStackString(Exception exception)
		{
			if (exception != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(exception?.Message ?? "Native crash");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(exception?.StackTrace);
				return stringBuilder.ToString();
			}
			return null;
		}
	}
}
