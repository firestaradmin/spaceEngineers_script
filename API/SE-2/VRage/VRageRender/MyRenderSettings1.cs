using System;

namespace VRageRender
{
	/// <summary>
	/// Naming convention from DX. Newer version for Dx11 render.
	/// Put only settings that player can control (either directly or indirectly) using options here.
	/// Don't put debug crap here!
	/// </summary>
	public struct MyRenderSettings1 : IEquatable<MyRenderSettings1>
	{
		public bool HqTarget;

		public MyAntialiasingMode AntialiasingMode;

		public bool AmbientOcclusionEnabled;

		public MyShadowsQuality ShadowQuality;

		public MyRenderQualityEnum ShadowGPUQuality;

		public MyTextureQuality TextureQuality;

		public MyTextureQuality VoxelTextureQuality;

		public MyTextureAnisoFiltering AnisotropicFiltering;

		public MyRenderQualityEnum ModelQuality;

		public MyRenderQualityEnum VoxelQuality;

		public bool HqDepth;

		public MyRenderQualityEnum VoxelShaderQuality;

		public MyRenderQualityEnum AlphaMaskedShaderQuality;

		public MyRenderQualityEnum AtmosphereShaderQuality;

		public MyRenderQualityEnum LightsQuality;

		public float GrassDrawDistance;

		public float GrassDensityFactor;

		public float DistanceFade;

		public MyRenderQualityEnum ParticleQuality;

		public override int GetHashCode()
		{
<<<<<<< HEAD
			return GrassDensityFactor.GetHashCode() ^ GrassDrawDistance.GetHashCode() ^ ModelQuality.GetHashCode() ^ VoxelQuality.GetHashCode() ^ AntialiasingMode.GetHashCode() ^ ShadowQuality.GetHashCode() ^ ShadowGPUQuality.GetHashCode() ^ AmbientOcclusionEnabled.GetHashCode() ^ TextureQuality.GetHashCode() ^ VoxelTextureQuality.GetHashCode() ^ AnisotropicFiltering.GetHashCode() ^ HqDepth.GetHashCode() ^ VoxelShaderQuality.GetHashCode() ^ AlphaMaskedShaderQuality.GetHashCode() ^ AtmosphereShaderQuality.GetHashCode() ^ DistanceFade.GetHashCode() ^ ParticleQuality.GetHashCode() ^ LightsQuality.GetHashCode();
=======
			return base.GetHashCode();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		bool IEquatable<MyRenderSettings1>.Equals(MyRenderSettings1 other)
		{
			return Equals(ref other);
		}

		public override bool Equals(object other)
		{
			MyRenderSettings1 other2 = (MyRenderSettings1)other;
			return Equals(ref other2);
		}

		public bool Equals(ref MyRenderSettings1 other)
		{
<<<<<<< HEAD
			if (GrassDensityFactor.IsEqual(other.GrassDensityFactor, 0.1f) && GrassDrawDistance.IsEqual(other.GrassDrawDistance, 2f) && ModelQuality == other.ModelQuality && VoxelQuality == other.VoxelQuality && AntialiasingMode == other.AntialiasingMode && ShadowQuality == other.ShadowQuality && ShadowGPUQuality == other.ShadowGPUQuality && AmbientOcclusionEnabled == other.AmbientOcclusionEnabled && TextureQuality == other.TextureQuality && VoxelTextureQuality == other.VoxelTextureQuality && AnisotropicFiltering == other.AnisotropicFiltering && HqDepth == other.HqDepth && VoxelShaderQuality == other.VoxelShaderQuality && AlphaMaskedShaderQuality == other.AlphaMaskedShaderQuality && AtmosphereShaderQuality == other.AtmosphereShaderQuality && DistanceFade.IsEqual(other.DistanceFade, 4f) && ParticleQuality == other.ParticleQuality)
			{
				return LightsQuality == other.LightsQuality;
=======
			if (GrassDensityFactor.IsEqual(other.GrassDensityFactor, 0.1f) && GrassDrawDistance.IsEqual(other.GrassDrawDistance, 2f) && ModelQuality == other.ModelQuality && VoxelQuality == other.VoxelQuality && AntialiasingMode == other.AntialiasingMode && ShadowQuality == other.ShadowQuality && ShadowGPUQuality == other.ShadowGPUQuality && AmbientOcclusionEnabled == other.AmbientOcclusionEnabled && TextureQuality == other.TextureQuality && VoxelTextureQuality == other.VoxelTextureQuality && AnisotropicFiltering == other.AnisotropicFiltering && HqDepth == other.HqDepth && VoxelShaderQuality == other.VoxelShaderQuality && AlphaMaskedShaderQuality == other.AlphaMaskedShaderQuality && AtmosphereShaderQuality == other.AtmosphereShaderQuality && DistanceFade.IsEqual(other.DistanceFade, 4f))
			{
				return ParticleQuality == other.ParticleQuality;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}
	}
}
