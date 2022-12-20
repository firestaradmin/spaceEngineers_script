using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SectorWeatherComponent : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003EBaseSunDirection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in SerializableVector3 value)
			{
				owner.BaseSunDirection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out SerializableVector3 value)
			{
				value = owner.BaseSunDirection;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003ESunDirectionNormalized_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in SerializableVector3 value)
			{
				owner.SunDirectionNormalized = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out SerializableVector3 value)
			{
				value = owner.SunDirectionNormalized;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003EWeatherPlanetData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_WeatherPlanetData[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in MyObjectBuilder_WeatherPlanetData[] value)
			{
				owner.WeatherPlanetData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out MyObjectBuilder_WeatherPlanetData[] value)
			{
				value = owner.WeatherPlanetData;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_SessionComponent>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_SessionComponent>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SectorWeatherComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SectorWeatherComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SectorWeatherComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SectorWeatherComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_SectorWeatherComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SectorWeatherComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SectorWeatherComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SectorWeatherComponent CreateInstance()
			{
				return new MyObjectBuilder_SectorWeatherComponent();
			}

			MyObjectBuilder_SectorWeatherComponent IActivator<MyObjectBuilder_SectorWeatherComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableVector3 BaseSunDirection;

		public SerializableVector3 SunDirectionNormalized;

		public MyObjectBuilder_WeatherPlanetData[] WeatherPlanetData;
	}
}
