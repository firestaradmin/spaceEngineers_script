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
	public class MyObjectBuilder_ComponentDefinition : MyObjectBuilder_PhysicalItemDefinition
	{
		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMaxIntegrity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				owner.MaxIntegrity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				value = owner.MaxIntegrity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDropProbability_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in float value)
			{
				owner.DropProbability = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out float value)
			{
				value = owner.DropProbability;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDeconstructionEfficiency_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in float value)
			{
				owner.DeconstructionEfficiency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out float value)
			{
				value = owner.DeconstructionEfficiency;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in Vector3 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out Vector3 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EIconSymbol_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in float? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out float? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EModelVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in float? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out float? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EVoxelMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EHealth_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDestroyedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMaxStackAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in MyFixedPoint value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out MyFixedPoint value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in int value)
<<<<<<< HEAD
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EExtraInventoryTooltipLineId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLineId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
=======
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out int value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_PhysicalItemDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ComponentDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ComponentDefinition CreateInstance()
			{
				return new MyObjectBuilder_ComponentDefinition();
			}

			MyObjectBuilder_ComponentDefinition IActivator<MyObjectBuilder_ComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int MaxIntegrity;

		[ProtoMember(4)]
		public float DropProbability;

		[ProtoMember(7)]
		public float DeconstructionEfficiency = 1f;
	}
}
