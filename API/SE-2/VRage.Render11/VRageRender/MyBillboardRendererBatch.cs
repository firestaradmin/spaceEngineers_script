using VRage.Render11.Resources;

namespace VRageRender
{
	internal struct MyBillboardRendererBatch
	{
		internal int Offset;

		internal int Num;

		internal ISrvBindable Texture;

		internal bool Lit;

		internal bool AlphaCutout;

		internal bool SingleChannel;
	}
}
