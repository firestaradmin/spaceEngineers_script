namespace VRageRender.Messages
{
	public class MyRenderMessageRemoveDecal : MyRenderMessageBase
	{
		public uint ID;

		public bool Immediately;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.RemoveDecal;
	}
}
