using VRage.Render11.Resources;
using VRage.Utils;

namespace VRageRender
{
	internal struct MyMaterialShadersInfo
	{
		internal MyStringId Material;

		internal MyStringId Pass;

		internal VertexLayoutId Layout;

		internal MyShaderUnifiedFlags Flags;

		internal MyFileTextureEnum TextureTypes;

		internal string Name => $"[{Pass.ToString()}][{Material.ToString()}]_{Flags}";
	}
}
