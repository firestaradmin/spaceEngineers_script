namespace VRageRender.Messages
{
	public class MyRenderMessageSpriteScissorPop : MySpriteDrawRenderMessage
	{
		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.SpriteScissorPop;

		public override (string, string) GetUsedTextures()
		{
			return (null, null);
		}
	}
}
