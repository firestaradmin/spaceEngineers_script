using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.GeometryStage2.Materials
{
	internal static class MyModelMaterials
	{
		public static MyRenderMaterialBindings GetBinding(string cmFilepath, string ngFilepath, string extFilepath, string alphamaskFilepath)
		{
			IMyStreamedTexture texture = MyManagers.Textures.GetTexture(cmFilepath, MyFileTextureEnum.COLOR_METAL);
			IMyStreamedTexture texture2 = MyManagers.Textures.GetTexture(ngFilepath, MyFileTextureEnum.NORMALMAP_GLOSS);
			IMyStreamedTexture texture3 = MyManagers.Textures.GetTexture(extFilepath, MyFileTextureEnum.EXTENSIONS);
			IMyStreamedTexture texture4 = MyManagers.Textures.GetTexture(alphamaskFilepath, MyFileTextureEnum.ALPHAMASK);
			MyRenderMaterialBindings result = default(MyRenderMaterialBindings);
			result.SrvAlphamask = texture4.Texture;
			result.Srvs = new ISrvBindable[4] { texture.Texture, texture2.Texture, texture3.Texture, texture4.Texture };
			result.TextureHandles = new IMyStreamedTexture[4] { texture, texture2, texture3, texture4 };
			return result;
		}

		public static MyRenderMaterial GetMaterial(MyMwmDataPart part)
		{
			string colorMetalFilepath = part.ColorMetalFilepath;
			string normalGlossFilepath = part.NormalGlossFilepath;
			string extensionFilepath = part.ExtensionFilepath;
			string alphamaskFilepath = part.AlphamaskFilepath;
			MyRenderMaterial result = default(MyRenderMaterial);
			result.Technique = part.Technique;
			result.Binding = GetBinding(colorMetalFilepath, normalGlossFilepath, extensionFilepath, alphamaskFilepath);
			result.IsColorMetalTexture = !string.IsNullOrEmpty(colorMetalFilepath);
			result.IsNormalGlossTexture = !string.IsNullOrEmpty(normalGlossFilepath);
			result.IsExtensionTexture = !string.IsNullOrEmpty(extensionFilepath);
			result.IsAlpamaskTexture = !string.IsNullOrEmpty(alphamaskFilepath);
			result.DebugMaterialName = part.MaterialName;
			return result;
		}
	}
}
