using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_LockableDrumDefinition : MyObjectBuilder_CubeBlockDefinition
	{
		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMinCustomRopeLength_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				owner.MinCustomRopeLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				value = owner.MinCustomRopeLength;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMaxCustomRopeLength_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				owner.MaxCustomRopeLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				value = owner.MaxCustomRopeLength;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDefaultMaxRopeLength_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				owner.DefaultMaxRopeLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				value = owner.DefaultMaxRopeLength;
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EVoxelPlacement_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVoxelPlacement_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, VoxelPlacementOverride?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in VoxelPlacementOverride? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out VoxelPlacementOverride? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECubeSize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeSize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyCubeSize value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyCubeSize value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBlockTopology_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockTopology_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyBlockTopology>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyBlockTopology value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyBlockTopology value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableVector3I value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableVector3I value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EModelOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModelOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableVector3 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableVector3 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECubeDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, PatternDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in PatternDefinition value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out PatternDefinition value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, CubeBlockComponent[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in CubeBlockComponent[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out CubeBlockComponent[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EEffects_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEffects_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, CubeBlockEffectBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in CubeBlockEffectBase[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out CubeBlockEffectBase[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECriticalComponent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalComponent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, CriticalPart>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in CriticalPart value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out CriticalPart value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMountPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MountPoint[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MountPoint[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MountPoint[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, Variant[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in Variant[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out Variant[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EEntityComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, EntityComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in EntityComponentDefinition[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out EntityComponentDefinition[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPhysicsOption_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicsOption_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyPhysicsOption>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyPhysicsOption value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyPhysicsOption value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBuildProgressModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, List<BuildProgressModel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in List<BuildProgressModel> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out List<BuildProgressModel> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBlockPairName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockPairName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECenter_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableVector3I? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableVector3I? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMirroringX_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMirroringY_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMirroringZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EEdgeType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEdgeType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDisassembleRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisassembleRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EAutorotateMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAutorotateMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyAutorotateMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyAutorotateMode value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyAutorotateMode value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMirroringBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EUseModelIntersection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseModelIntersection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPrimarySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPrimarySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EActionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBuildType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBuildMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECompoundTemplates_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundTemplates_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECompoundEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MySubBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MySubBlockDefinition[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MySubBlockDefinition[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMultiBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMultiBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ENavigationDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENavigationDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EGuiVisible_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGuiVisible_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBlockVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDirection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDirection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyBlockDirection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyBlockDirection value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyBlockDirection value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ERotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyBlockRotation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyBlockRotation value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyBlockRotation value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EGeneratedBlockType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlockType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMirrored_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirrored_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDamageEffectId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDestroyEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDestroySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ESkeleton_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESkeleton_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in List<BoneInfo> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out List<BoneInfo> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ERandomRotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERandomRotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EIsAirTight_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsAirTight_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EIsStandAlone_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsStandAlone_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EHasPhysics_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EHasPhysics_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMaxIntegrity_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMaxIntegrity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDamagedSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamagedSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDamageEffectName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EUsesDeformation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUsesDeformation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in Vector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out Vector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPCU_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCU_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in int value)
<<<<<<< HEAD
=======
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPCUConsole_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCUConsole_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in int? value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPCUConsole_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCUConsole_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in int? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
=======
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out int? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPlaceDecals_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPlaceDecals_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
<<<<<<< HEAD
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDepressurizationEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDepressurizationEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableVector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableVector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableVector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ETieredUpdateTimes_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ETieredUpdateTimes_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MySerializableList<uint>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MySerializableList<uint> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MySerializableList<uint> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ETargetingGroups_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ETargetingGroups_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPriorityModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPriorityModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ENotWorkingPriorityMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENotWorkingPriorityMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDamageMultiplierExplosion_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageMultiplierExplosion_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
=======
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

<<<<<<< HEAD
		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDamageThreshold_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageThreshold_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
=======
		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDepressurizationEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDepressurizationEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableVector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableVector3? value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
=======
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableVector3? value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

<<<<<<< HEAD
		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDetonateChance_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDetonateChance_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
=======
		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ETieredUpdateTimes_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ETieredUpdateTimes_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MySerializableList<uint>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MySerializableList<uint> value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EAmmoExplosionEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAmmoExplosionEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EAmmoExplosionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAmmoExplosionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDamageEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in Vector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out Vector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EAimingOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAimingOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in Vector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out Vector3? value)
=======
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MySerializableList<uint> value)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LockableDrumDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LockableDrumDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LockableDrumDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_LockableDrumDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_LockableDrumDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_LockableDrumDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_LockableDrumDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_LockableDrumDefinition CreateInstance()
			{
				return new MyObjectBuilder_LockableDrumDefinition();
			}

			MyObjectBuilder_LockableDrumDefinition IActivator<MyObjectBuilder_LockableDrumDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float MinCustomRopeLength;

		[ProtoMember(2)]
		public float MaxCustomRopeLength;

		[ProtoMember(3)]
		public float DefaultMaxRopeLength;
	}
}
