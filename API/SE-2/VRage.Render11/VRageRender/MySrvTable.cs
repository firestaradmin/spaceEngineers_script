using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;

namespace VRageRender
{
	internal struct MySrvTable
	{
		internal int StartSlot;

		internal ISrvBindable[] Srvs;

		internal IMyStreamedTexture[] TextureHandles;

		internal MyBindFlag BindFlag;

		internal int Version;
	}
}
