using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDrawSpriteAtlas : MySpriteDrawRenderMessage
	{
		public string Texture;

		public Vector2 Position;

		public Vector2 TextureOffset;

		public Vector2 TextureSize;

		public Vector2 RightVector;

		public Vector2 Scale;

		public Color Color;

		public Vector2 HalfSize;

		public bool IgnoreBounds;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawSpriteAtlas;

		public override (string, string) GetUsedTextures()
		{
			return (Texture, null);
		}
	}
}
