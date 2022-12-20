using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WeatherEffectDefinition), null)]
	public class MyWeatherEffectDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyWeatherEffectDefinition_003C_003EActor : IActivator, IActivator<MyWeatherEffectDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeatherEffectDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeatherEffectDefinition CreateInstance()
			{
				return new MyWeatherEffectDefinition();
			}

			MyWeatherEffectDefinition IActivator<MyWeatherEffectDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyFogProperties FogProperties = MyFogProperties.Default;

		public string AmbientSound = "";

		public float AmbientVolume;

		public string EffectName = "";

		public float ParticleRadius;

		public int ParticleCount;

		public float ParticleScale = 1f;

		public float LightningIntervalMin;

		public float LightningIntervalMax;

		public float LightningCharacterHitIntervalMin;

		public float LightningCharacterHitIntervalMax;

		public float LightningGridHitIntervalMin;

		public float LightningGridHitIntervalMax;

		public float ParticleAlphaMultiplier = 1f;

		public Vector3 SunColor = MyEnvironmentLightData.Default.SunColor;

		public Vector3 SunSpecularColor = MyEnvironmentLightData.Default.SunSpecularColor;

		public float SunIntensity = MySunProperties.Default.SunIntensity;

		public float ShadowFadeout = MyEnvironmentLightData.Default.ShadowFadeoutMultiplier;

		public float WindOutputModifier = 1f;

		public float SolarOutputModifier = 1f;

		public float TemperatureModifier = 1f;

		public float OxygenLevelModifier = 1f;

		public float FoliageWindModifier = 1f;

		public MyObjectBuilder_WeatherLightning Lightning;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WeatherEffectDefinition myObjectBuilder_WeatherEffectDefinition = builder as MyObjectBuilder_WeatherEffectDefinition;
			FogProperties.FogColor = myObjectBuilder_WeatherEffectDefinition.FogColor;
			FogProperties.FogDensity = myObjectBuilder_WeatherEffectDefinition.FogDensity;
			FogProperties.FogMultiplier = myObjectBuilder_WeatherEffectDefinition.FogMultiplier;
			FogProperties.FogSkybox = myObjectBuilder_WeatherEffectDefinition.FogSkyboxMultiplier;
			FogProperties.FogAtmo = myObjectBuilder_WeatherEffectDefinition.FogAtmoMultiplier;
			AmbientSound = myObjectBuilder_WeatherEffectDefinition.AmbientSound;
			AmbientVolume = myObjectBuilder_WeatherEffectDefinition.AmbientVolume;
			EffectName = myObjectBuilder_WeatherEffectDefinition.EffectName;
			ParticleRadius = myObjectBuilder_WeatherEffectDefinition.ParticleRadius;
			ParticleCount = myObjectBuilder_WeatherEffectDefinition.ParticleCount;
			ParticleScale = myObjectBuilder_WeatherEffectDefinition.ParticleScale;
			LightningIntervalMin = myObjectBuilder_WeatherEffectDefinition.LightningIntervalMin;
			LightningIntervalMax = myObjectBuilder_WeatherEffectDefinition.LightningIntervalMax;
			LightningCharacterHitIntervalMin = myObjectBuilder_WeatherEffectDefinition.LightningCharacterHitIntervalMin;
			LightningCharacterHitIntervalMax = myObjectBuilder_WeatherEffectDefinition.LightningCharacterHitIntervalMax;
			LightningGridHitIntervalMin = myObjectBuilder_WeatherEffectDefinition.LightningGridHitIntervalMin;
			LightningGridHitIntervalMax = myObjectBuilder_WeatherEffectDefinition.LightningGridHitIntervalMax;
			ParticleAlphaMultiplier = myObjectBuilder_WeatherEffectDefinition.ParticleAlphaMultiplier;
			SunColor = myObjectBuilder_WeatherEffectDefinition.SunColor;
			SunSpecularColor = myObjectBuilder_WeatherEffectDefinition.SunSpecularColor;
			SunIntensity = myObjectBuilder_WeatherEffectDefinition.SunIntensity;
			ShadowFadeout = myObjectBuilder_WeatherEffectDefinition.ShadowFadeout;
			WindOutputModifier = myObjectBuilder_WeatherEffectDefinition.WindOutputModifier;
			SolarOutputModifier = myObjectBuilder_WeatherEffectDefinition.SolarOutputModifier;
			TemperatureModifier = myObjectBuilder_WeatherEffectDefinition.TemperatureModifier;
			OxygenLevelModifier = myObjectBuilder_WeatherEffectDefinition.OxygenLevelModifier;
			FoliageWindModifier = myObjectBuilder_WeatherEffectDefinition.FoliageWindModifier;
			Lightning = myObjectBuilder_WeatherEffectDefinition.Lightning;
		}

		public void Lerp(MyWeatherEffectDefinition targetWeather, MyWeatherEffectDefinition outputWeather, float ratio)
		{
			outputWeather.FogProperties.FogColor = Vector3.Lerp(FogProperties.FogColor, targetWeather.FogProperties.FogColor, ratio);
			outputWeather.FogProperties.FogDensity = MathHelper.Lerp(FogProperties.FogDensity, targetWeather.FogProperties.FogDensity, ratio);
			outputWeather.FogProperties.FogMultiplier = MathHelper.Lerp(FogProperties.FogMultiplier, targetWeather.FogProperties.FogMultiplier, ratio);
			outputWeather.FogProperties.FogSkybox = MathHelper.Lerp(FogProperties.FogSkybox, targetWeather.FogProperties.FogSkybox, ratio);
			outputWeather.FogProperties.FogAtmo = MathHelper.Lerp(FogProperties.FogAtmo, targetWeather.FogProperties.FogAtmo, ratio);
			outputWeather.AmbientSound = ((ratio < 0.5f) ? AmbientSound : targetWeather.AmbientSound);
			outputWeather.AmbientVolume = MathHelper.Lerp(AmbientVolume, targetWeather.AmbientVolume, ratio);
			outputWeather.EffectName = ((ratio < 0.5f) ? EffectName : targetWeather.EffectName);
			outputWeather.ParticleRadius = MathHelper.Lerp(ParticleRadius, targetWeather.ParticleRadius, ratio);
			outputWeather.ParticleCount = MathHelper.RoundToInt(MathHelper.Lerp(ParticleCount, targetWeather.ParticleCount, ratio));
			outputWeather.ParticleScale = MathHelper.Lerp(ParticleScale, targetWeather.ParticleScale, ratio);
			outputWeather.ParticleAlphaMultiplier = MathHelper.Lerp(ParticleAlphaMultiplier, targetWeather.ParticleAlphaMultiplier, ratio);
			outputWeather.SunColor = Vector3.Lerp(SunColor, targetWeather.SunColor, ratio);
			outputWeather.SunSpecularColor = Vector3.Lerp(SunSpecularColor, targetWeather.SunSpecularColor, ratio);
			outputWeather.SunIntensity = MathHelper.Lerp(SunIntensity, targetWeather.SunIntensity, MathHelper.Clamp(ratio * 2f, 0f, 1f));
			outputWeather.ShadowFadeout = MathHelper.Lerp(ShadowFadeout, targetWeather.ShadowFadeout, ratio);
			outputWeather.WindOutputModifier = MathHelper.Lerp(WindOutputModifier, targetWeather.WindOutputModifier, ratio);
			outputWeather.SolarOutputModifier = MathHelper.Lerp(SolarOutputModifier, targetWeather.SolarOutputModifier, ratio);
			outputWeather.TemperatureModifier = MathHelper.Lerp(TemperatureModifier, targetWeather.TemperatureModifier, ratio);
			outputWeather.OxygenLevelModifier = MathHelper.Lerp(OxygenLevelModifier, targetWeather.OxygenLevelModifier, ratio);
			outputWeather.FoliageWindModifier = MathHelper.Lerp(FoliageWindModifier, targetWeather.FoliageWindModifier, ratio);
			outputWeather.LightningIntervalMin = MathHelper.Lerp(LightningIntervalMin, targetWeather.LightningIntervalMin, ratio);
			outputWeather.LightningIntervalMax = MathHelper.Lerp(LightningIntervalMax, targetWeather.LightningIntervalMax, ratio);
			outputWeather.LightningCharacterHitIntervalMin = MathHelper.Lerp(LightningCharacterHitIntervalMin, targetWeather.LightningCharacterHitIntervalMin, ratio);
			outputWeather.LightningCharacterHitIntervalMax = MathHelper.Lerp(LightningCharacterHitIntervalMax, targetWeather.LightningCharacterHitIntervalMax, ratio);
			outputWeather.LightningGridHitIntervalMin = MathHelper.Lerp(LightningGridHitIntervalMin, targetWeather.LightningGridHitIntervalMin, ratio);
			outputWeather.LightningGridHitIntervalMax = MathHelper.Lerp(LightningGridHitIntervalMax, targetWeather.LightningGridHitIntervalMax, ratio);
		}
	}
}
