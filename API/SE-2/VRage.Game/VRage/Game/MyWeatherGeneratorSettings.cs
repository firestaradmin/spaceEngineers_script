using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyWeatherGeneratorSettings
	{
		protected class VRage_Game_MyWeatherGeneratorSettings_003C_003EVoxel_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorSettings owner, in string value)
			{
				owner.Voxel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorSettings owner, out string value)
			{
				value = owner.Voxel;
			}
		}

		protected class VRage_Game_MyWeatherGeneratorSettings_003C_003EWeathers_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorSettings, List<MyWeatherGeneratorVoxelSettings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorSettings owner, in List<MyWeatherGeneratorVoxelSettings> value)
			{
				owner.Weathers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorSettings owner, out List<MyWeatherGeneratorVoxelSettings> value)
			{
				value = owner.Weathers;
			}
		}

		private class VRage_Game_MyWeatherGeneratorSettings_003C_003EActor : IActivator, IActivator<MyWeatherGeneratorSettings>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeatherGeneratorSettings();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeatherGeneratorSettings CreateInstance()
			{
				return new MyWeatherGeneratorSettings();
			}

			MyWeatherGeneratorSettings IActivator<MyWeatherGeneratorSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string Voxel;

		[ProtoMember(10)]
		[XmlArrayItem("Weather")]
		public List<MyWeatherGeneratorVoxelSettings> Weathers;
	}
}
