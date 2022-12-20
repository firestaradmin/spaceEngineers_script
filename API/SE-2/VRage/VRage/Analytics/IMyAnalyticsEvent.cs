using System.Collections.Generic;

namespace VRage.Analytics
{
	/// <summary>
	/// Represents an event reported to analytics
	/// </summary>
	public interface IMyAnalyticsEvent
	{
		/// <summary>
		/// Returns the name of the reported event
		/// </summary>
		string GetEventName();

		/// <summary>
		/// Returns additional information associated with the event
		/// </summary>
		Dictionary<string, object> GetPropertiesDictionary();

		/// <summary>
		/// Returns info for GameAnalytics.com reporting
		/// </summary>
		MyReportTypeData GetReportTypeAndArgs();
	}
}
