using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WeatherPlanetData
	{
		protected class VRage_Game_MyObjectBuilder_WeatherPlanetData_003C_003EPlanetId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherPlanetData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherPlanetData owner, in long value)
			{
				owner.PlanetId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherPlanetData owner, out long value)
			{
				value = owner.PlanetId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherPlanetData_003C_003ENextWeather_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherPlanetData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherPlanetData owner, in int value)
			{
				owner.NextWeather = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherPlanetData owner, out int value)
			{
				value = owner.NextWeather;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherPlanetData_003C_003EWeathers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherPlanetData, List<MyObjectBuilder_WeatherEffect>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherPlanetData owner, in List<MyObjectBuilder_WeatherEffect> value)
			{
				owner.Weathers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherPlanetData owner, out List<MyObjectBuilder_WeatherEffect> value)
			{
				value = owner.Weathers;
			}
		}

		private class VRage_Game_MyObjectBuilder_WeatherPlanetData_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WeatherPlanetData>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WeatherPlanetData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WeatherPlanetData CreateInstance()
			{
				return new MyObjectBuilder_WeatherPlanetData();
			}

			MyObjectBuilder_WeatherPlanetData IActivator<MyObjectBuilder_WeatherPlanetData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long PlanetId;

		[ProtoMember(10)]
		public int NextWeather;

		[ProtoMember(30)]
		public List<MyObjectBuilder_WeatherEffect> Weathers = new List<MyObjectBuilder_WeatherEffect>();
	}
}
