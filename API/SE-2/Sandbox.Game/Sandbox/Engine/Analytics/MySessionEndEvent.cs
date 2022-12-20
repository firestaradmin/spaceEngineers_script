using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
<<<<<<< HEAD
	/// <summary> The game is being terminated or user logged off <para />
	/// Should be the last event in an analytics session </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public class MySessionEndEvent : MyAnalyticsEvent
	{
		[Required]
		public MySessionStartEvent SessionStartProperties { get; set; }

		[Required]
		public int? session_duration { get; set; }

		[Required]
		public string game_quit_reason { get; set; }

		public string Exception { get; set; }

		public MySessionEndEvent(MySessionStartEvent sessionStartProperties)
		{
			SessionStartProperties = sessionStartProperties;
		}

		public override string GetEventName()
		{
			return "SessionEnd";
		}

		public override MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionComplete, "Game", game_quit_reason);
		}
	}
}
