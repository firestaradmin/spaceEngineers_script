using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDrawString : MySpriteDrawRenderMessage
	{
		public int FontIndex;

		public Vector2 ScreenCoord;

		public Color ColorMask;

		public string Text;

		public float ScreenScale;

		public float ScreenMaxWidth;

		public bool IgnoreBounds;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawString;

		public override (string, string) GetUsedTextures()
		{
			return (null, null);
		}
	}
}
