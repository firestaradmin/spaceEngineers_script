<<<<<<< HEAD
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Game.ObjectBuilders;
=======
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_DebugSphere2 : MyObjectBuilder_FunctionalBlock
	{
		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_FunctionalBlock_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value)
<<<<<<< HEAD
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_FunctionalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_FunctionalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003ETextPanelsNew_003C_003EAccessor : VRage_Game_MyObjectBuilder_FunctionalBlock_003C_003ETextPanelsNew_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, List<MySerializedTextPanelData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in List<MySerializedTextPanelData> value)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_FunctionalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out List<MySerializedTextPanelData> value)
=======
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_FunctionalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003ECustomName_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ECustomName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EShowOnHUD_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowOnHUD_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EShowInTerminal_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInTerminal_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EShowInToolbarConfig_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInToolbarConfig_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EShowInInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_TerminalBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in long value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out long value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableVector3I value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableVector3I value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003Em_orientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableQuaternion value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableQuaternion value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EIntegrityPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EBuildPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EBlockOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableBlockOrientation value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableBlockOrientation value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EConstructionInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyObjectBuilder_Inventory value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyObjectBuilder_Inventory value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EColorMaskHSV_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableVector3 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableVector3 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003ESkinSubtypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EConstructionStockpile_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EOwner_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in long value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out long value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EBuiltBy_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in long value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out long value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EShareMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyOwnershipShareModeEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyOwnershipShareModeEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003ESubBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MySubBlockId[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MySubBlockId[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EMultiBlockId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EMultiBlockDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableDefinitionId? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableDefinitionId? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EMultiBlockIndex_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EComponentContainer_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableQuaternion value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableQuaternion value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_DebugSphere2_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DebugSphere2>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DebugSphere2();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DebugSphere2 CreateInstance()
			{
				return new MyObjectBuilder_DebugSphere2();
			}

			MyObjectBuilder_DebugSphere2 IActivator<MyObjectBuilder_DebugSphere2>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
