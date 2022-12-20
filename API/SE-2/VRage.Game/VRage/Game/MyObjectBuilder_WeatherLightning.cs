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
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WeatherLightning : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in Vector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out Vector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003ELife_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in byte value)
			{
				owner.Life = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out byte value)
			{
				value = owner.Life;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EMaxLife_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in short value)
			{
				owner.MaxLife = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out short value)
			{
				value = owner.MaxLife;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EBoltLength_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in float value)
			{
				owner.BoltLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out float value)
			{
				value = owner.BoltLength;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EBoltParts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in byte value)
			{
				owner.BoltParts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out byte value)
			{
				value = owner.BoltParts;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EBoltVariation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in short value)
			{
				owner.BoltVariation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out short value)
			{
				value = owner.BoltVariation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EBoltRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in float value)
			{
				owner.BoltRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out float value)
			{
				value = owner.BoltRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in int value)
			{
				owner.Damage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out int value)
			{
				value = owner.Damage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003ESound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in string value)
			{
				owner.Sound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out string value)
			{
				value = owner.Sound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in Vector4 value)
			{
				owner.Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out Vector4 value)
			{
				value = owner.Color;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EBoltImpulseMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in float value)
			{
				owner.BoltImpulseMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out float value)
			{
				value = owner.BoltImpulseMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EExplosionRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in float value)
			{
				owner.ExplosionRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out float value)
			{
				value = owner.ExplosionRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeatherLightning owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeatherLightning owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_WeatherLightning, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WeatherLightning_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WeatherLightning>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WeatherLightning();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WeatherLightning CreateInstance()
			{
				return new MyObjectBuilder_WeatherLightning();
			}

			MyObjectBuilder_WeatherLightning IActivator<MyObjectBuilder_WeatherLightning>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public Vector3D Position;

		[ProtoMember(10)]
		public byte Life;

		[ProtoMember(15)]
		public short MaxLife = 7;

		[ProtoMember(20)]
		public float BoltLength = 5000f;

		[ProtoMember(25)]
		public byte BoltParts = 50;

		[ProtoMember(30)]
		public short BoltVariation = 100;

		[ProtoMember(35)]
		public float BoltRadius = 30f;

		[ProtoMember(40)]
		public int Damage;

		[ProtoMember(45)]
		public string Sound = "WM_Lightning";

		[ProtoMember(50)]
		public Vector4 Color = new Vector4(100f, 100f, 100f, 1000f);

		[ProtoMember(55)]
		public float BoltImpulseMultiplier = 1f;

		[ProtoMember(65)]
		public float ExplosionRadius = 1f;
	}
}
