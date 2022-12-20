using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
<<<<<<< HEAD
	/// <summary> Sent periodically to report the player is online </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public class MyHeartbeatEvent : MyAnalyticsEvent
	{
		public string Region_ISO2 { get; set; }

		public string Region_ISO3 { get; set; }

		public override string GetEventName()
		{
			return "Heartbeat";
		}

		public override MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionUndefined, null, null);
		}
	}
}
