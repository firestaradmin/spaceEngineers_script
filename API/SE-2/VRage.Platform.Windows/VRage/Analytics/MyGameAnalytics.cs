using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameAnalyticsSDK.Net;
using VRage.FileSystem;

namespace VRage.Analytics
{
	public class MyGameAnalytics : MyAnalyticsBase
	{
		private string m_lastUserId;

		public MyGameAnalytics(string accessToken, string gameVersion)
			: base(Path.Combine(MyFileSystem.TempPath, "GameAnalyticsEvents"), 1000)
		{
			GameAnalytics.ConfigureBuild(gameVersion);
			if (!string.IsNullOrWhiteSpace(accessToken) && accessToken.Contains(':'))
			{
				string[] array = accessToken.Split(':');
				if (array.Length != 2)
				{
					return;
				}
				string gameKey = array[0];
				string gameSecret = array[1];
				GameAnalytics.Initialize(gameKey, gameSecret);
			}
			ReportPostponedEvents();
		}

		protected override void ReportEvent(MyEvent myEvent)
		{
			SetCurrentUserId(myEvent.UserID);
			if (myEvent.Exception != null)
			{
				GameAnalytics.AddErrorEvent(EGAErrorSeverity.Critical, myEvent.Exception);
			}
			MyReportTypeData reportTypeAndArgs = myEvent.ReportTypeAndArgs;
			switch (reportTypeAndArgs.ReportType)
			{
			case MyReportType.ProgressionStart:
				GameAnalytics.AddProgressionEvent(EGAProgressionStatus.Start, reportTypeAndArgs.Arg1, reportTypeAndArgs.Arg2);
				break;
			case MyReportType.ProgressionComplete:
				GameAnalytics.AddProgressionEvent(EGAProgressionStatus.Complete, reportTypeAndArgs.Arg1, reportTypeAndArgs.Arg2);
				break;
			case MyReportType.ProgressionFailed:
				GameAnalytics.AddProgressionEvent(EGAProgressionStatus.Fail, reportTypeAndArgs.Arg1, reportTypeAndArgs.Arg2);
				break;
			case MyReportType.ProgressionUndefined:
				GameAnalytics.AddProgressionEvent(EGAProgressionStatus.Undefined, reportTypeAndArgs.Arg1, reportTypeAndArgs.Arg2);
				break;
			}
			foreach (KeyValuePair<string, object> item in myEvent.P)
			{
				if (item.Value != null)
				{
					string message = $"{item.Key}: {item.Value}";
					GameAnalytics.AddErrorEvent(EGAErrorSeverity.Info, message);
				}
			}
		}

		private void SetCurrentUserId(string userId)
		{
			if (m_lastUserId == null || userId != m_lastUserId)
			{
				m_lastUserId = userId;
				GameAnalytics.ConfigureUserId(userId);
			}
		}
	}
}
