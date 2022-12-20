using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SharedStorageComponent : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EBoolStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, bool>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, bool> value)
			{
				owner.BoolStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, bool> value)
			{
				value = owner.BoolStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EIntStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, int> value)
			{
				owner.IntStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, int> value)
			{
				value = owner.IntStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ELongStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, long> value)
			{
				owner.LongStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, long> value)
			{
				value = owner.LongStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EULongStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, ulong>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, ulong> value)
			{
				owner.ULongStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, ulong> value)
			{
				value = owner.ULongStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EStringStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, string> value)
			{
				owner.StringStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, string> value)
			{
				value = owner.StringStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EFloatStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, float>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, float> value)
			{
				owner.FloatStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, float> value)
			{
				value = owner.FloatStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EVector3DStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableVector3D>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableVector3D> value)
			{
				owner.Vector3DStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableVector3D> value)
			{
				value = owner.Vector3DStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EBoolListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<bool>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<bool>> value)
			{
				owner.BoolListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<bool>> value)
			{
				value = owner.BoolListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EIntListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<int>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<int>> value)
			{
				owner.IntListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<int>> value)
			{
				value = owner.IntListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ELongListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<long>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<long>> value)
			{
				owner.LongListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<long>> value)
			{
				value = owner.LongListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EULongListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<ulong>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<ulong>> value)
			{
				owner.ULongListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<ulong>> value)
			{
				value = owner.ULongListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EStringListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<string>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<string>> value)
			{
				owner.StringListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<string>> value)
			{
				value = owner.StringListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EFloatListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<float>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<float>> value)
			{
				owner.FloatListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<float>> value)
			{
				value = owner.FloatListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EVector3DListStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
			{
				owner.Vector3DListStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
			{
				value = owner.Vector3DListStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EBoolStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, bool>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, bool>> value)
			{
				owner.BoolStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, bool>> value)
			{
				value = owner.BoolStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EIntStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, int>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, int>> value)
			{
				owner.IntStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, int>> value)
			{
				value = owner.IntStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ELongStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, long>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, long>> value)
			{
				owner.LongStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, long>> value)
			{
				value = owner.LongStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EULongStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, ulong>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, ulong>> value)
			{
				owner.ULongStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, ulong>> value)
			{
				value = owner.ULongStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EStringStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, string>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, string>> value)
			{
				owner.StringStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, string>> value)
			{
				value = owner.StringStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EFloatStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, float>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, float>> value)
			{
				owner.FloatStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, float>> value)
			{
				value = owner.FloatStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EVector3DStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>> value)
			{
				owner.Vector3DStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>> value)
			{
				value = owner.Vector3DStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EBoolListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>> value)
			{
				owner.BoolListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>> value)
			{
				value = owner.BoolListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EIntListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>> value)
			{
				owner.IntListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>> value)
			{
				value = owner.IntListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ELongListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>> value)
			{
				owner.LongListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>> value)
			{
				value = owner.LongListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EULongListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>> value)
			{
				owner.ULongListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>> value)
			{
				value = owner.ULongListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EStringListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>> value)
			{
				owner.StringListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>> value)
			{
				value = owner.StringListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EFloatListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>> value)
			{
				owner.FloatListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>> value)
			{
				value = owner.FloatListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EVector3DListStorageSecondary_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>> value)
			{
				owner.Vector3DListStorageSecondary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>> value)
			{
				value = owner.Vector3DListStorageSecondary;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_SessionComponent>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_SessionComponent>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_SharedStorageComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SharedStorageComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SharedStorageComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SharedStorageComponent CreateInstance()
			{
				return new MyObjectBuilder_SharedStorageComponent();
			}

			MyObjectBuilder_SharedStorageComponent IActivator<MyObjectBuilder_SharedStorageComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDictionary<string, bool> BoolStorage = new SerializableDictionary<string, bool>();

		public SerializableDictionary<string, int> IntStorage = new SerializableDictionary<string, int>();

		public SerializableDictionary<string, long> LongStorage = new SerializableDictionary<string, long>();

		public SerializableDictionary<string, ulong> ULongStorage = new SerializableDictionary<string, ulong>();

		public SerializableDictionary<string, string> StringStorage = new SerializableDictionary<string, string>();

		public SerializableDictionary<string, float> FloatStorage = new SerializableDictionary<string, float>();

		public SerializableDictionary<string, SerializableVector3D> Vector3DStorage = new SerializableDictionary<string, SerializableVector3D>();

		public SerializableDictionary<string, MySerializableList<bool>> BoolListStorage = new SerializableDictionary<string, MySerializableList<bool>>();

		public SerializableDictionary<string, MySerializableList<int>> IntListStorage = new SerializableDictionary<string, MySerializableList<int>>();

		public SerializableDictionary<string, MySerializableList<long>> LongListStorage = new SerializableDictionary<string, MySerializableList<long>>();

		public SerializableDictionary<string, MySerializableList<ulong>> ULongListStorage = new SerializableDictionary<string, MySerializableList<ulong>>();

		public SerializableDictionary<string, MySerializableList<string>> StringListStorage = new SerializableDictionary<string, MySerializableList<string>>();

		public SerializableDictionary<string, MySerializableList<float>> FloatListStorage = new SerializableDictionary<string, MySerializableList<float>>();

		public SerializableDictionary<string, MySerializableList<SerializableVector3D>> Vector3DListStorage = new SerializableDictionary<string, MySerializableList<SerializableVector3D>>();

		public SerializableDictionary<string, SerializableDictionary<string, bool>> BoolStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, bool>>();

		public SerializableDictionary<string, SerializableDictionary<string, int>> IntStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, int>>();

		public SerializableDictionary<string, SerializableDictionary<string, long>> LongStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, long>>();

		public SerializableDictionary<string, SerializableDictionary<string, ulong>> ULongStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, ulong>>();

		public SerializableDictionary<string, SerializableDictionary<string, string>> StringStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, string>>();

		public SerializableDictionary<string, SerializableDictionary<string, float>> FloatStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, float>>();

		public SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>> Vector3DStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>> BoolListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>> IntListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>> LongListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>> ULongListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>> StringListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>> FloatListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>>();

		public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>> Vector3DListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>();
	}
}
