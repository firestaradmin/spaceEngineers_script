using System;
using VRage.Render11.Resources;

namespace VRageRender
{
	public static class MyRenderSettings1Extensions
	{
		public static bool IsMultisampled(this MyAntialiasingMode aaMode)
		{
			return false;
		}

		public static int SamplesCount(this MyAntialiasingMode aaMode)
		{
			if ((uint)aaMode <= 1u)
			{
				return 1;
			}
			return -1;
		}

		public static int ShadowCascadeResolution(this MyRenderQualityEnum shadowQuality)
		{
<<<<<<< HEAD
			switch (shadowQuality)
			{
			case MyRenderQualityEnum.LOW:
				return 512;
			case MyRenderQualityEnum.NORMAL:
				return 1024;
			case MyRenderQualityEnum.HIGH:
				return 2048;
			case MyRenderQualityEnum.EXTREME:
				return 4096;
			default:
				return -1;
			}
=======
			return shadowQuality switch
			{
				MyRenderQualityEnum.LOW => 512, 
				MyRenderQualityEnum.NORMAL => 1024, 
				MyRenderQualityEnum.HIGH => 2048, 
				MyRenderQualityEnum.EXTREME => 4096, 
				_ => -1, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static int ShadowSpotResolution(this MyRenderQualityEnum shadowQuality)
		{
<<<<<<< HEAD
			switch (shadowQuality)
			{
			case MyRenderQualityEnum.LOW:
				return 256;
			case MyRenderQualityEnum.NORMAL:
				return 512;
			case MyRenderQualityEnum.HIGH:
				return 1024;
			case MyRenderQualityEnum.EXTREME:
				return 2048;
			default:
				return -1;
			}
=======
			return shadowQuality switch
			{
				MyRenderQualityEnum.LOW => 256, 
				MyRenderQualityEnum.NORMAL => 512, 
				MyRenderQualityEnum.HIGH => 1024, 
				MyRenderQualityEnum.EXTREME => 2048, 
				_ => -1, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static int BackOffset(this MyShadowsQuality shadowQuality)
		{
			if ((uint)shadowQuality <= 4u)
			{
				return (int)MyShadowCascades.Settings.Data.ShadowCascadeZOffset;
			}
			return -1;
		}

		public static float ReflectorShadowDistance(this MyShadowsQuality shadowQuality)
		{
<<<<<<< HEAD
			switch (shadowQuality)
			{
			case MyShadowsQuality.DISABLED:
				return 0f;
			case MyShadowsQuality.LOW:
				return MyShadowCascades.Settings.Data.ReflectorShadowDistanceLow;
			case MyShadowsQuality.MEDIUM:
				return MyShadowCascades.Settings.Data.ReflectorShadowDistanceMedium;
			case MyShadowsQuality.HIGH:
				return MyShadowCascades.Settings.Data.ReflectorShadowDistanceHigh;
			case MyShadowsQuality.EXTREME:
				return MyShadowCascades.Settings.Data.ReflectorShadowDistanceExtreme;
			default:
				return 0f;
			}
=======
			return shadowQuality switch
			{
				MyShadowsQuality.DISABLED => 0f, 
				MyShadowsQuality.LOW => MyShadowCascades.Settings.Data.ReflectorShadowDistanceLow, 
				MyShadowsQuality.MEDIUM => MyShadowCascades.Settings.Data.ReflectorShadowDistanceMedium, 
				MyShadowsQuality.HIGH => MyShadowCascades.Settings.Data.ReflectorShadowDistanceHigh, 
				MyShadowsQuality.EXTREME => MyShadowCascades.Settings.Data.ReflectorShadowDistanceExtreme, 
				_ => 0f, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static float ShadowCascadeSplit(this MyShadowsQuality shadowQuality, int cascade)
		{
			if (cascade == 0)
			{
				return 1f;
			}
			float num = MyShadowCascades.Settings.Data.ShadowCascadeMaxDistance;
			switch (shadowQuality)
			{
			case MyShadowsQuality.MEDIUM:
				num *= MyShadowCascades.Settings.Data.ShadowCascadeMaxDistanceMultiplierMedium;
				break;
			case MyShadowsQuality.HIGH:
				num *= MyShadowCascades.Settings.Data.ShadowCascadeMaxDistanceMultiplierHigh;
				break;
			case MyShadowsQuality.EXTREME:
				num *= MyShadowCascades.Settings.Data.ShadowCascadeMaxDistanceMultiplierExtreme;
				break;
			}
			float shadowCascadeSpreadFactor = MyShadowCascades.Settings.Data.ShadowCascadeSpreadFactor;
			return (float)Math.Pow(num, ((float)cascade + shadowCascadeSpreadFactor) / ((float)MyShadowCascades.Settings.Data.CascadesCount + shadowCascadeSpreadFactor));
		}

		internal static int MipLevelsToSkip(this MyTextureQuality quality, int width, int height, MyFileTextureEnum type)
		{
			if (width <= 32 || height <= 32)
			{
				return 0;
			}
			switch (quality)
			{
			case MyTextureQuality.LOW:
				if ((type & MyFileTextureEnum.CUBEMAP) == 0)
				{
					return 2;
				}
				return 1;
			case MyTextureQuality.MEDIUM:
				if ((type & MyFileTextureEnum.CUBEMAP) == 0)
				{
					return 1;
				}
				return 0;
			case MyTextureQuality.HIGH:
				return 0;
			default:
				return 0;
			}
		}
	}
}
