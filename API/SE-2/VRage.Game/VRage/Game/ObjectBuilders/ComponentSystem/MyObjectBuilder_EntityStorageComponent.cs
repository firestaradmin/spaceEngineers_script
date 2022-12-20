using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EntityStorageComponent : MyObjectBuilder_ComponentBase
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EBoolStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, bool>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, bool> value)
			{
				owner.BoolStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, bool> value)
			{
				value = owner.BoolStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EIntStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, int> value)
			{
				owner.IntStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, int> value)
			{
				value = owner.IntStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003ELongStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, long> value)
			{
				owner.LongStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, long> value)
			{
				value = owner.LongStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EStringStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, string> value)
			{
				owner.StringStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, string> value)
			{
				value = owner.StringStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EFloatStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, float>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, float> value)
			{
				owner.FloatStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, float> value)
			{
				value = owner.FloatStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EVector3DStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, SerializableVector3D>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, SerializableVector3D> value)
			{
				owner.Vector3DStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, SerializableVector3D> value)
			{
				value = owner.Vector3DStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EBoolListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<bool>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, MySerializableList<bool>> value)
			{
				owner.BoolListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, MySerializableList<bool>> value)
			{
				value = owner.BoolListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EIntListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<int>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, MySerializableList<int>> value)
			{
				owner.IntListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, MySerializableList<int>> value)
			{
				value = owner.IntListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003ELongListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<long>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, MySerializableList<long>> value)
			{
				owner.LongListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, MySerializableList<long>> value)
			{
				value = owner.LongListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EStringListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<string>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, MySerializableList<string>> value)
			{
				owner.StringListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, MySerializableList<string>> value)
			{
				value = owner.StringListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EFloatListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<float>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, MySerializableList<float>> value)
			{
				owner.FloatListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, MySerializableList<float>> value)
			{
				value = owner.FloatListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EVector3DListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
			{
				owner.Vector3DListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
			{
				value = owner.Vector3DListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityStorageComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityStorageComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_EntityStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EntityStorageComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EntityStorageComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EntityStorageComponent CreateInstance()
			{
				return new MyObjectBuilder_EntityStorageComponent();
			}

			MyObjectBuilder_EntityStorageComponent IActivator<MyObjectBuilder_EntityStorageComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, bool> BoolStorage = new SerializableDictionary<string, bool>();

		[ProtoMember(10)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, int> IntStorage = new SerializableDictionary<string, int>();

		[ProtoMember(15)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, long> LongStorage = new SerializableDictionary<string, long>();

		[ProtoMember(20)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, string> StringStorage = new SerializableDictionary<string, string>();

		[ProtoMember(25)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, float> FloatStorage = new SerializableDictionary<string, float>();

		[ProtoMember(30)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, SerializableVector3D> Vector3DStorage = new SerializableDictionary<string, SerializableVector3D>();

		[ProtoMember(35)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, MySerializableList<bool>> BoolListStorage = new SerializableDictionary<string, MySerializableList<bool>>();

		[ProtoMember(40)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, MySerializableList<int>> IntListStorage = new SerializableDictionary<string, MySerializableList<int>>();

		[ProtoMember(45)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, MySerializableList<long>> LongListStorage = new SerializableDictionary<string, MySerializableList<long>>();

		[ProtoMember(50)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, MySerializableList<string>> StringListStorage = new SerializableDictionary<string, MySerializableList<string>>();

		[ProtoMember(55)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, MySerializableList<float>> FloatListStorage = new SerializableDictionary<string, MySerializableList<float>>();

		[ProtoMember(60)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDictionary<string, MySerializableList<SerializableVector3D>> Vector3DListStorage = new SerializableDictionary<string, MySerializableList<SerializableVector3D>>();
	}
}
