using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WeatherEffect
	{
		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in Vector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out Vector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in Vector3D value)
			{
				owner.Velocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out Vector3D value)
			{
				value = owner.Velocity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EWeather_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in string value)
			{
				owner.Weather = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out string value)
			{
				value = owner.Weather;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003ERadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in float value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out float value)
			{
				value = owner.Radius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003ELife_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in int value)
			{
				owner.Life = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out int value)
			{
				value = owner.Life;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EMaxLife_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in int value)
			{
				owner.MaxLife = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out int value)
			{
				value = owner.MaxLife;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EIntensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in float value)
			{
				owner.Intensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out float value)
			{
				value = owner.Intensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EStartPoint_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in Vector3D value)
			{
				owner.StartPoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out Vector3D value)
			{
				value = owner.StartPoint;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003ENextLightning_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in int value)
			{
				owner.NextLightning = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out int value)
			{
				value = owner.NextLightning;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003ENextLightningCharacter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in int value)
			{
				owner.NextLightningCharacter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out int value)
			{
				value = owner.NextLightningCharacter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003ENextLightningGrid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherEffect owner, in int value)
			{
				owner.NextLightningGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherEffect owner, out int value)
			{
				value = owner.NextLightningGrid;
			}
		}

		private class VRage_Game_MyObjectBuilder_WeatherEffect_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WeatherEffect>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WeatherEffect();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WeatherEffect CreateInstance()
			{
				return new MyObjectBuilder_WeatherEffect();
			}

			MyObjectBuilder_WeatherEffect IActivator<MyObjectBuilder_WeatherEffect>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public Vector3D Position;

		[ProtoMember(7)]
		public Vector3D Velocity;

		[ProtoMember(10)]
		public string Weather;

		[ProtoMember(15)]
		public float Radius;

		[ProtoMember(20)]
		public int Life;

		[ProtoMember(21)]
		public int MaxLife;

		[ProtoMember(25)]
		public float Intensity;

		[ProtoMember(30)]
		public Vector3D StartPoint;

		[ProtoMember(35)]
		public int NextLightning;

		[ProtoMember(40)]
		public int NextLightningCharacter;

		[ProtoMember(45)]
		public int NextLightningGrid;

		public Vector3D EndPoint => StartPoint + Velocity * ((float)MaxLife * 0.0166666675f);
	}
}
