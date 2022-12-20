namespace Sandbox.Engine.Analytics
{
	public class MyDropContainerEvent : MyAnalyticsEvent
	{
		[Required]
		public MyWorldStartEvent WorldStartProperties { get; set; }

		public bool? Competetive { get; set; }

		public MyDropContainerEvent(MyWorldStartEvent worldStartProperties)
		{
			WorldStartProperties = worldStartProperties;
		}

		public override string GetEventName()
		{
			return "DropContainer";
		}
	}
}
