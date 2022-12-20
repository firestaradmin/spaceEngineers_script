using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WeatherEffectDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EFogColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in Vector3 value)
			{
				owner.FogColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out Vector3 value)
			{
				value = owner.FogColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EFogDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.FogDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.FogDensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EFogMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.FogMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.FogMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EFogSkyboxMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.FogSkyboxMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.FogSkyboxMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EFogAtmoMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.FogAtmoMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.FogAtmoMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EAmbientSound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				owner.AmbientSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				value = owner.AmbientSound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EAmbientVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.AmbientVolume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.AmbientVolume;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EEffectName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				owner.EffectName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				value = owner.EffectName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EParticleRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.ParticleRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.ParticleRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EParticleCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in int value)
			{
				owner.ParticleCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out int value)
			{
				value = owner.ParticleCount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EParticleScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.ParticleScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.ParticleScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightningIntervalMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.LightningIntervalMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.LightningIntervalMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightningIntervalMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.LightningIntervalMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.LightningIntervalMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightningGridHitIntervalMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.LightningGridHitIntervalMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.LightningGridHitIntervalMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightningGridHitIntervalMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.LightningGridHitIntervalMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.LightningGridHitIntervalMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightningCharacterHitIntervalMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.LightningCharacterHitIntervalMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.LightningCharacterHitIntervalMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightningCharacterHitIntervalMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.LightningCharacterHitIntervalMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.LightningCharacterHitIntervalMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EParticleAlphaMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.ParticleAlphaMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.ParticleAlphaMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ESunIntensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.SunIntensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.SunIntensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ESunColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in Vector3 value)
			{
				owner.SunColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out Vector3 value)
			{
				value = owner.SunColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ESunSpecularColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in Vector3 value)
			{
				owner.SunSpecularColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out Vector3 value)
			{
				value = owner.SunSpecularColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EShadowFadeout_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.ShadowFadeout = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.ShadowFadeout;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EWindOutputModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.WindOutputModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.WindOutputModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ESolarOutputModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.SolarOutputModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.SolarOutputModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ETemperatureModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.TemperatureModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.TemperatureModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EOxygenLevelModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.OxygenLevelModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.OxygenLevelModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ELightning_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_WeatherLightning>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in MyObjectBuilder_WeatherLightning value)
			{
				owner.Lightning = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out MyObjectBuilder_WeatherLightning value)
			{
				value = owner.Lightning;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EFoliageWindModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value)
			{
				owner.FoliageWindModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value)
			{
				value = owner.FoliageWindModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WeatherEffectDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WeatherEffectDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WeatherEffectDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WeatherEffectDefinition CreateInstance()
			{
				return new MyObjectBuilder_WeatherEffectDefinition();
			}

			MyObjectBuilder_WeatherEffectDefinition IActivator<MyObjectBuilder_WeatherEffectDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public Vector3 FogColor;

		[ProtoMember(10)]
		public float FogDensity;

		[ProtoMember(15)]
		public float FogMultiplier;

		[ProtoMember(20)]
		public float FogSkyboxMultiplier;

		[ProtoMember(25)]
		public float FogAtmoMultiplier;

		[ProtoMember(30)]
		public string AmbientSound;

		[ProtoMember(35)]
		public float AmbientVolume;

		[ProtoMember(40)]
		public string EffectName;

		[ProtoMember(45)]
		public float ParticleRadius;

		[ProtoMember(50)]
		public int ParticleCount;

		[ProtoMember(55)]
		public float ParticleScale;

		[ProtoMember(61)]
		public float LightningIntervalMin;

		[ProtoMember(62)]
		public float LightningIntervalMax;

		[ProtoMember(63)]
		public float LightningGridHitIntervalMin;

		[ProtoMember(64)]
		public float LightningGridHitIntervalMax;

		[ProtoMember(66)]
		public float LightningCharacterHitIntervalMin;

		[ProtoMember(69)]
		public float LightningCharacterHitIntervalMax;

		[ProtoMember(70)]
		public float ParticleAlphaMultiplier;

		[ProtoMember(75)]
		[DefaultValue(150)]
		public float SunIntensity = 150f;

		[ProtoMember(76)]
		public Vector3 SunColor;

		[ProtoMember(77)]
		public Vector3 SunSpecularColor;

		[ProtoMember(80)]
		public float ShadowFadeout;

		[ProtoMember(85)]
		public float WindOutputModifier = 1f;

		[ProtoMember(90)]
		public float SolarOutputModifier = 1f;

		[ProtoMember(95)]
		public float TemperatureModifier = 1f;

		[ProtoMember(100)]
		public float OxygenLevelModifier = 1f;

		[ProtoMember(105)]
		public MyObjectBuilder_WeatherLightning Lightning;

		[ProtoMember(110)]
		public float FoliageWindModifier = 1f;
	}
}
