using VRageMath;

namespace VRage.Game
{
	public struct MyFogProperties
	{
		private static class Defaults
		{
			public static readonly Vector3 FogColor = new Vector3(0f, 0f, 0f);

			public const float FogMultiplier = 0.13f;

			public const float FogDensity = 0.003f;

			public const float FogSkybox = 0f;

			public const float FogAtmo = 0f;
		}

		[StructDefault]
		public static readonly MyFogProperties Default;

		public float FogMultiplier;

		public float FogDensity;

		public Vector3 FogColor;

		public float FogSkybox;

		public float FogAtmo;

		public MyFogProperties Lerp(ref MyFogProperties target, float ratio)
		{
			MyFogProperties result = default(MyFogProperties);
			result.FogMultiplier = MathHelper.Lerp(FogMultiplier, target.FogMultiplier, ratio);
			result.FogDensity = MathHelper.Lerp(FogDensity, target.FogDensity, ratio);
			result.FogColor = Vector3.Lerp(FogColor, target.FogColor, ratio);
			result.FogSkybox = MathHelper.Lerp(FogSkybox, target.FogSkybox, ratio);
			result.FogAtmo = MathHelper.Lerp(FogAtmo, target.FogAtmo, ratio);
			return result;
		}

		public override bool Equals(object obj)
		{
			MyFogProperties myFogProperties = (MyFogProperties)obj;
			if (MathHelper.IsEqual(FogMultiplier, myFogProperties.FogMultiplier) && MathHelper.IsEqual(FogDensity, myFogProperties.FogDensity) && MathHelper.IsEqual(FogColor, myFogProperties.FogColor) && MathHelper.IsEqual(FogSkybox, myFogProperties.FogSkybox))
			{
				return MathHelper.IsEqual(FogAtmo, myFogProperties.FogAtmo);
			}
			return false;
		}

<<<<<<< HEAD
		public override int GetHashCode()
		{
			return FogMultiplier.GetHashCode() ^ FogDensity.GetHashCode() ^ FogColor.GetHashCode() ^ FogSkybox.GetHashCode() ^ FogAtmo.GetHashCode();
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		static MyFogProperties()
		{
			Default = new MyFogProperties
			{
				FogMultiplier = 0.13f,
				FogDensity = 0.003f,
				FogColor = Defaults.FogColor,
				FogSkybox = 0f,
				FogAtmo = 0f
			};
		}
	}
}
