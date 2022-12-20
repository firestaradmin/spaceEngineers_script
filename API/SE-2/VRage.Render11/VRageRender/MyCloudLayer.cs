using VRage.Library.Utils;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageRender.Messages;

namespace VRageRender
{
	internal struct MyCloudLayer
	{
		internal MeshId Mesh;

		internal ISrvBindable AlphaTexture;

		internal ISrvBindable ColorTexture;

		internal MyStreamedTexturePin ColorTextureHandle;

		internal MyStreamedTexturePin AlphaTextureHandle;

		internal MyCloudLayerSettingsRender Settings;

		internal MyTimeSpan FadeInStart;

		internal MyTimeSpan FadeOutStart;
	}
}
