using System;

namespace VRage.Analytics
{
	/// <summary>
	/// Generic interface for all analytics library integrations
	/// </summary>
	public interface IMyAnalytics
	{
		/// <summary>
		/// Tracks the specified analytics event <para />
		/// All IMyAnalytics implementations might not use all the parameters
		/// </summary>
		/// <param name="analyticsEvent"> Specifies event's name and additional properties </param>
		/// <param name="timestamp"> Time the event was triggered </param>
		/// <param name="sessionID"> Unique analytics session identifier </param>
		/// <param name="userID"> Unique user identifier </param>
		/// <param name="clientVersion"> Version of the client on which the event was triggered </param>
		/// <param name="platform"> Platform identifier </param>
		/// <param name="exception"> The exception associated with the event </param>
		void ReportEvent(IMyAnalyticsEvent analyticsEvent, DateTime timestamp, string sessionID, string userID, string clientVersion, string platform, Exception exception = null);

		/// <summary>
		/// Stores the specified analytics event to send it later <para />
		/// All IMyAnalytics implementations might not use all the parameters
		/// </summary>
		/// <param name="analyticsEvent"> Specifies event's name and additional properties </param>
		/// <param name="timestamp"> Time the event was triggered </param>
		/// <param name="sessionID"> Unique analytics session identifier </param>
		/// <param name="userID"> Unique user identifier </param>
		/// <param name="clientVersion"> Version of the client on which the event was triggered </param>
		/// <param name="platform"> Platform identifier </param>
		/// <param name="exception"> The exception associated with the event </param>
		void ReportEventLater(IMyAnalyticsEvent analyticsEvent, DateTime timestamp, string sessionID, string userID, string clientVersion, string platform, Exception exception = null);

		/// <summary>
		/// Reports previously stored events, if it has any
		/// </summary>
		void ReportPostponedEvents();
	}
}
