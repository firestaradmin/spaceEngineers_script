using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
<<<<<<< HEAD
	/// <summary> A banner in the main menu was clicked </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public class MyBannerClickEvent : MyAnalyticsEvent
	{
		[Required]
		public MySessionStartEvent SessionStartProperties { get; set; }

		public uint? banner_package_id { get; set; }

		public string banner_caption { get; set; }

		public MyBannerClickEvent(MySessionStartEvent sessionStartProperties)
		{
			SessionStartProperties = sessionStartProperties;
		}

		public override string GetEventName()
		{
			return "BannerClick";
		}

		public override MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionUndefined, "Banner", banner_caption);
		}
	}
}
