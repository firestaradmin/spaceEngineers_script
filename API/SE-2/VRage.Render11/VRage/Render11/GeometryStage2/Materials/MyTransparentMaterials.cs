using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Materials
{
	internal static class MyTransparentMaterials
	{
		public static MyRenderMaterial GetMaterial(string materialName, MyMeshDrawTechnique technique)
		{
			MyTransparentMaterial material = VRageRender.MyTransparentMaterials.GetMaterial(X.TEXT_(materialName));
			IMyStreamedTexture texture = MyManagers.Textures.GetTexture(material.Texture, MyFileTextureEnum.COLOR_METAL);
			IMyStreamedTexture texture2 = MyManagers.Textures.GetTexture(material.GlossTexture, MyFileTextureEnum.NORMALMAP_GLOSS);
			MyRenderMaterial result = default(MyRenderMaterial);
			result.DebugMaterialName = materialName;
			result.Technique = technique;
			result.Binding = new MyRenderMaterialBindings
			{
				SrvAlphamask = null,
				Srvs = new ISrvBindable[3] { texture.Texture, texture2.Texture, texture.Texture },
				TextureHandles = new IMyStreamedTexture[3] { texture, texture2, texture }
			};
			result.Material = material;
			result.IsColorAlphaTexture = true;
			result.IsFlareOccluder = material.IsFlareOccluder;
			return result;
		}
	}
}
