using System;
using SharpDX.DXGI;
using VRageRender.Messages;

namespace VRage.Render11.Resources
{
	internal static class TextureExtensions
	{
		public static MyGeneratedTextureType ToGeneratedTextureType(this Format type)
		{
<<<<<<< HEAD
			switch (type)
			{
			case Format.R8G8B8A8_UNorm_SRgb:
				return MyGeneratedTextureType.RGBA;
			case Format.R8G8B8A8_UNorm:
				return MyGeneratedTextureType.RGBA_Linear;
			case Format.R8_UNorm:
				return MyGeneratedTextureType.Alphamask;
			default:
				throw new Exception();
			}
=======
			return type switch
			{
				Format.R8G8B8A8_UNorm_SRgb => MyGeneratedTextureType.RGBA, 
				Format.R8G8B8A8_UNorm => MyGeneratedTextureType.RGBA_Linear, 
				Format.R8_UNorm => MyGeneratedTextureType.Alphamask, 
				_ => throw new Exception(), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static MyGeneratedTextureType ToGeneratedTextureType(this MyTextureType type)
		{
<<<<<<< HEAD
			switch (type)
			{
			case MyTextureType.ColorMetal:
				return MyGeneratedTextureType.RGBA;
			case MyTextureType.NormalGloss:
				return MyGeneratedTextureType.RGBA_Linear;
			case MyTextureType.Extensions:
				return MyGeneratedTextureType.RGBA;
			case MyTextureType.Alphamask:
				return MyGeneratedTextureType.Alphamask;
			default:
				throw new Exception();
			}
=======
			return type switch
			{
				MyTextureType.ColorMetal => MyGeneratedTextureType.RGBA, 
				MyTextureType.NormalGloss => MyGeneratedTextureType.RGBA_Linear, 
				MyTextureType.Extensions => MyGeneratedTextureType.RGBA, 
				MyTextureType.Alphamask => MyGeneratedTextureType.Alphamask, 
				_ => throw new Exception(), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static bool IsTextureLoaded(this ITexture texture)
		{
			return (texture as IAsyncTexture)?.IsLoaded ?? true;
		}
	}
}
