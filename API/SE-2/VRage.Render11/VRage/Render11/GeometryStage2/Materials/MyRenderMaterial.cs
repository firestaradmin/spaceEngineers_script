using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Materials
{
	internal struct MyRenderMaterial
	{
		public string DebugMaterialName;

		public MyMeshDrawTechnique Technique;

		public MyRenderMaterialBindings Binding;

		public MyTransparentMaterial Material;

		public bool IsColorMetalTexture;

		public bool IsNormalGlossTexture;

		public bool IsExtensionTexture;

		public bool IsAlpamaskTexture;

		public bool IsColorAlphaTexture;

		public bool IsFlareOccluder;
	}
}
