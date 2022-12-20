namespace VRageRender.Messages
{
	public class MyRenderMessageDestroyGeneratedTexture : MyRenderMessageBase
	{
		public string TextureName;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DestroyGeneratedTexture;
	}
}
