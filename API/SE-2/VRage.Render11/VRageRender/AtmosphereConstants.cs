using VRageMath;

namespace VRageRender
{
	internal struct AtmosphereConstants
	{
		internal Vector3 PlanetCentre;

		internal float AtmosphereRadius;

		internal Vector3 BetaRayleighScattering;

		internal float GroundRadius;

		internal Vector3 BetaMieScattering;

		internal float MieG;

		internal Vector2 HeightScaleRayleighMie;

		internal float PlanetScaleFactor;

		internal float AtmosphereScaleFactor;

		internal float Intensity;

		internal float FogIntensity;

		internal float DesaturationFactor;

		internal float _pad0;

		internal Matrix WorldViewProj;
	}
}
