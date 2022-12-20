using VRage.Library.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal struct MyAtmosphere
	{
		internal MatrixD WorldMatrix;

		internal float AtmosphereRadius;

		internal float PlanetRadius;

		internal Vector3 BetaRayleighScattering;

		internal Vector3 BetaMieScattering;

		internal Vector2 HeightScaleRayleighMie;

		internal float PlanetScaleFactor;

		internal float AtmosphereScaleFactor;

		internal MyAtmosphereSettings Settings;

		public MyTimeSpan FadeInStart;

		public MyTimeSpan FadeOutStart;

		public bool LutsPrecomputed;
	}
}
