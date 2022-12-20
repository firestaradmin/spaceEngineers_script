using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDrawVideo : MySpriteDrawRenderMessage
	{
		public uint ID;

		public Rectangle Rectangle;

		public Color Color;

		public MyVideoRectangleFitMode FitMode;

		public bool IgnoreBounds;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawVideo;

		public override (string, string) GetUsedTextures()
		{
			return (null, null);
		}
	}
}
