namespace VRageRender.Messages
{
	public class MyRenderMessagePauseTimer : MyRenderMessageBase
	{
		public bool pause;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.PauseTimer;
	}
}
