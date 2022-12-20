using System;
using SharpDX.DXGI;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.Resources
{
	internal class MyGlobalResources : IManager, IManagerDevice
	{
		public static IDynamicFileArrayTexture FileArrayTextureVoxelCM;

		public static IDynamicFileArrayTexture FileArrayTextureVoxelNG;

		public static IDynamicFileArrayTexture FileArrayTextureVoxelExt;

		public static IDynamicFileArrayTexture FileArrayTextureVoxelCMLow;

		public static IDynamicFileArrayTexture FileArrayTextureVoxelNGLow;

		public static IDynamicFileArrayTexture FileArrayTextureVoxelExtLow;

		public void CreateOnStartup(int lowSlicesCM, int lowSlicesNG, int lowSlicesExt)
		{
			MyDynamicFileArrayTextureManager dynamicFileArrayTextures = MyManagers.DynamicFileArrayTextures;
			FileArrayTextureVoxelCMLow = dynamicFileArrayTextures.CreateTexture("MyGlobalResources.FileArrayTextureVoxelCMLow", MyFileTextureEnum.COLOR_METAL, MyGeneratedTexturePatterns.ColorMetal_BC7_SRgb, Format.BC7_UNorm_SRgb, lowSlicesCM, keepAsLowMipMap: true);
			FileArrayTextureVoxelNGLow = dynamicFileArrayTextures.CreateTexture("MyGlobalResources.FileArrayTextureVoxelNGLow", MyFileTextureEnum.NORMALMAP_GLOSS, MyGeneratedTexturePatterns.NormalGloss_BC7, Format.BC7_UNorm, lowSlicesNG, keepAsLowMipMap: true);
			FileArrayTextureVoxelExtLow = dynamicFileArrayTextures.CreateTexture("MyGlobalResources.FileArrayTextureVoxelExtLow", MyFileTextureEnum.EXTENSIONS, MyGeneratedTexturePatterns.Extension_BC7_SRgb, Format.BC7_UNorm_SRgb, lowSlicesExt, keepAsLowMipMap: true);
			FileArrayTextureVoxelCM = dynamicFileArrayTextures.CreateTexture("MyGlobalResources.FileArrayTextureVoxelCM", MyFileTextureEnum.COLOR_METAL, MyGeneratedTexturePatterns.ColorMetal_BC7_SRgb, Format.BC7_UNorm_SRgb, 0);
			FileArrayTextureVoxelNG = dynamicFileArrayTextures.CreateTexture("MyGlobalResources.FileArrayTextureVoxelNG", MyFileTextureEnum.NORMALMAP_GLOSS, MyGeneratedTexturePatterns.NormalGloss_BC7, Format.BC7_UNorm, 0);
			FileArrayTextureVoxelExt = dynamicFileArrayTextures.CreateTexture("MyGlobalResources.FileArrayTextureVoxelExt", MyFileTextureEnum.EXTENSIONS, MyGeneratedTexturePatterns.Extension_BC7_SRgb, Format.BC7_UNorm_SRgb, 0);
			OnTextureQualityChanged();
			MyManagers.Textures.InitStreamingArray(FileArrayTextureVoxelCM);
			MyManagers.Textures.InitStreamingArray(FileArrayTextureVoxelNG);
			MyManagers.Textures.InitStreamingArray(FileArrayTextureVoxelExt);
		}

		public void OnDeviceInit()
		{
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
			MyManagers.DynamicFileArrayTextures.DisposeTex(ref FileArrayTextureVoxelCM);
			MyManagers.DynamicFileArrayTextures.DisposeTex(ref FileArrayTextureVoxelNG);
			MyManagers.DynamicFileArrayTextures.DisposeTex(ref FileArrayTextureVoxelExt);
			MyManagers.DynamicFileArrayTextures.DisposeTex(ref FileArrayTextureVoxelCMLow);
			MyManagers.DynamicFileArrayTextures.DisposeTex(ref FileArrayTextureVoxelNGLow);
			MyManagers.DynamicFileArrayTextures.DisposeTex(ref FileArrayTextureVoxelExtLow);
		}

		public void OnTextureQualityChanged()
		{
			MyTextureArrayStreamingBudget.GetVoxelMaterialsSlicesBudget(out var cmSlices, out var ngSlices, out var extSlices);
			if (FileArrayTextureVoxelCM != null)
			{
				FileArrayTextureVoxelCM.Release();
				FileArrayTextureVoxelCM.MinSlices = Math.Min(cmSlices, FileArrayTextureVoxelCMLow.MinSlices);
			}
			if (FileArrayTextureVoxelNG != null)
			{
				FileArrayTextureVoxelNG.Release();
				FileArrayTextureVoxelNG.MinSlices = Math.Min(ngSlices, FileArrayTextureVoxelNGLow.MinSlices);
			}
			if (FileArrayTextureVoxelExt != null)
			{
				FileArrayTextureVoxelExt.Release();
				FileArrayTextureVoxelExt.MinSlices = Math.Min(extSlices, FileArrayTextureVoxelExtLow.MinSlices);
			}
		}
	}
}
