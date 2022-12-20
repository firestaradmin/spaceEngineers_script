using Sandbox.Game.GameSystems;

namespace Sandbox.Engine.Analytics
{
	public class MyGoodBotEvent : MyAnalyticsEvent
	{
		[Required]
		public MyWorldStartEvent WorldStartProperties { get; set; }

		public ResponseType GoodBot_ResponseType { get; internal set; }

		public string GoodBot_Question { get; internal set; }

		public string GoodBot_ResponseID { get; internal set; }

		public MyGoodBotEvent(MyWorldStartEvent worldStartProperties)
		{
			WorldStartProperties = worldStartProperties;
		}

		public override string GetEventName()
		{
			return "GoodBot";
		}
	}
}
