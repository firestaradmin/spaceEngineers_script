using System;
using System.Collections.Generic;

namespace VRage.Analytics
{
	/// <summary>
	/// IMyAnalytics implementation which redirects given events to other trackers
	/// </summary>
	public abstract class MyAnalyticsManager : IMyAnalytics
	{
		private List<IMyAnalytics> m_analyticsTrackers = new List<IMyAnalytics>();

		/// <summary>
		/// The given tracker will be reported the same events as the manager
		/// </summary>
		public void RegisterAnalyticsTracker(IMyAnalytics tracker)
		{
			if (tracker != null && !m_analyticsTrackers.Contains(tracker))
			{
				m_analyticsTrackers.Add(tracker);
			}
		}

		/// <summary>
		/// Send the given event to all registered analytics trackers
		/// </summary>
		void IMyAnalytics.ReportEvent(IMyAnalyticsEvent analyticsEvent, DateTime timestamp, string sessionID, string userID, string clientVersion, string platform, Exception exception)
		{
			foreach (IMyAnalytics analyticsTracker in m_analyticsTrackers)
			{
				analyticsTracker.ReportEvent(analyticsEvent, timestamp, sessionID, userID, clientVersion, platform, exception);
			}
		}

		/// <summary>
		/// Send the given event to be stored by all registered analytics trackers
		/// </summary>
		void IMyAnalytics.ReportEventLater(IMyAnalyticsEvent analyticsEvent, DateTime timestamp, string sessionID, string userID, string clientVersion, string platform, Exception exception)
		{
			foreach (IMyAnalytics analyticsTracker in m_analyticsTrackers)
			{
				analyticsTracker.ReportEventLater(analyticsEvent, timestamp, sessionID, userID, clientVersion, platform, exception);
			}
		}

		/// <summary>
		/// Tells all registered analytics trackers to report stored events
		/// </summary>
		public void ReportPostponedEvents()
		{
			foreach (IMyAnalytics analyticsTracker in m_analyticsTrackers)
			{
				analyticsTracker.ReportPostponedEvents();
			}
		}
	}
}
