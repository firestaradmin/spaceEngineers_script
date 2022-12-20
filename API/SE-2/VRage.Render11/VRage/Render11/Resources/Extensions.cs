using VRageRender.Messages;

namespace VRage.Render11.Resources
{
	public static class Extensions
	{
		internal static MyFileTextureEnum ToFileTextureEnum(this MyTextureType type)
		{
			if ((type & MyTextureType.Alphamask) > MyTextureType.Unspecified)
			{
				return MyFileTextureEnum.ALPHAMASK;
			}
			if ((type & MyTextureType.ColorMetal) > MyTextureType.Unspecified)
			{
				return MyFileTextureEnum.COLOR_METAL;
			}
			if ((type & MyTextureType.Extensions) > MyTextureType.Unspecified)
			{
				return MyFileTextureEnum.EXTENSIONS;
			}
			if ((type & MyTextureType.NormalGloss) > MyTextureType.Unspecified)
			{
				return MyFileTextureEnum.NORMALMAP_GLOSS;
			}
			return MyFileTextureEnum.UNSPECIFIED;
		}

		internal static MyFileTextureEnum ToFileTextureEnum(this TextureType type)
		{
			if ((type & TextureType.AlphaMask) > (TextureType)0)
			{
				return MyFileTextureEnum.ALPHAMASK;
			}
			if ((type & TextureType.ColorMetal) > (TextureType)0)
			{
				return MyFileTextureEnum.COLOR_METAL;
			}
			if ((type & TextureType.Extensions) > (TextureType)0)
			{
				return MyFileTextureEnum.EXTENSIONS;
			}
			if ((type & TextureType.NormalGloss) > (TextureType)0)
			{
				return MyFileTextureEnum.NORMALMAP_GLOSS;
			}
			if ((type & TextureType.GUI) > (TextureType)0)
			{
				return MyFileTextureEnum.GUI;
			}
			if ((type & TextureType.GUIWithoutPremultiplyAlpha) > (TextureType)0)
			{
				return MyFileTextureEnum.CUSTOM;
			}
			if ((type & TextureType.Particles) > (TextureType)0)
			{
				return MyFileTextureEnum.GPUPARTICLES;
			}
			if ((type & TextureType.CubeMap) > (TextureType)0)
			{
				return MyFileTextureEnum.CUBEMAP;
			}
			return MyFileTextureEnum.UNSPECIFIED;
		}
	}
}
