using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.GeometryStage2.Materials
{
	internal struct MyRenderMaterialBindings
	{
		public ISrvBindable[] Srvs;

		public ISrvBindable SrvAlphamask;

		public IMyStreamedTexture[] TextureHandles;
	}
}
