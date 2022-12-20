using System;
using VRageRender;

namespace VRage.Render11.Resources.Textures
{
	public static class MyTextureArrayStreamingBudget
	{
		private const int MAX_SLICES = 256;

		private static int GetAvailableVoxelMaterialSlices()
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			switch (MyRender11.Settings.User.VoxelTextureQuality)
			{
			case MyTextureQuality.LOW:
				num = 349526uL;
				num2 = 15345664uL;
				break;
			case MyTextureQuality.MEDIUM:
				num = 1398102uL;
				num2 = 61702144uL;
				break;
			case MyTextureQuality.HIGH:
				num = 5592406uL;
				num2 = 240098304uL;
				break;
			}
			return (int)((MyRender11.Settings.VoxelTexturesStreamingPool - num2) / num);
		}

		public static void GetVoxelMaterialsSlicesBudget(out int cmSlices, out int ngSlices, out int extSlices)
		{
			int availableVoxelMaterialSlices = GetAvailableVoxelMaterialSlices();
			cmSlices = Math.Max(1, Math.Min((int)Math.Floor((double)availableVoxelMaterialSlices * 0.4), 256));
			ngSlices = Math.Max(1, Math.Min((int)Math.Floor((double)availableVoxelMaterialSlices * 0.4), 256));
			extSlices = Math.Max(1, Math.Min((int)Math.Floor((double)availableVoxelMaterialSlices * 0.2), 256));
		}
	}
}
