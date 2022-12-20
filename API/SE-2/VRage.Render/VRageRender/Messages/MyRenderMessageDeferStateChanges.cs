namespace VRageRender.Messages
{
	public class MyRenderMessageDeferStateChanges : MyRenderMessageBase
	{
		public bool Enabled;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DeferStateChanges;
	}
}
