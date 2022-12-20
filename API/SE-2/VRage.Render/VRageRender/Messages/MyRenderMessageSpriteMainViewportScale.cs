namespace VRageRender.Messages
{
	public class MyRenderMessageSpriteMainViewportScale : MyRenderMessageBase
	{
		public float ScaleFactor;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.SpriteMainViewportScale;
	}
}
