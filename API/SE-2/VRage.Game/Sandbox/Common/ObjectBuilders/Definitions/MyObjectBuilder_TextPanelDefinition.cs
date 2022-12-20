using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Common.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
<<<<<<< HEAD
	public class MyObjectBuilder_TextPanelDefinition : MyObjectBuilder_FunctionalBlockDefinition
=======
	public class MyObjectBuilder_TextPanelDefinition : MyObjectBuilder_CubeBlockDefinition
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EResourceSinkGroup_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				owner.ResourceSinkGroup = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				value = owner.ResourceSinkGroup;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ERequiredPowerInput_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				owner.RequiredPowerInput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				value = owner.RequiredPowerInput;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ETextureResolution_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				owner.TextureResolution = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				value = owner.TextureResolution;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPanelMaterialName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				owner.PanelMaterialName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				value = owner.PanelMaterialName;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EScreenWidth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				owner.ScreenWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				value = owner.ScreenWidth;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EScreenHeight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				owner.ScreenHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				value = owner.ScreenHeight;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMinFontSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				owner.MinFontSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				value = owner.MinFontSize;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMaxFontSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				owner.MaxFontSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				value = owner.MaxFontSize;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMaxChangingSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				owner.MaxChangingSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				value = owner.MaxChangingSpeed;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMaxScreenRenderDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				owner.MaxScreenRenderDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				value = owner.MaxScreenRenderDistance;
			}
		}

<<<<<<< HEAD
		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EScreenAreas_003C_003EAccessor : VRage_Game_MyObjectBuilder_FunctionalBlockDefinition_003C_003EScreenAreas_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, List<ScreenArea>>
=======
		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EScreenAreas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextPanelDefinition, List<ScreenArea>>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in List<ScreenArea> value)
			{
<<<<<<< HEAD
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_FunctionalBlockDefinition>(ref owner), in value);
=======
				owner.ScreenAreas = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out List<ScreenArea> value)
			{
<<<<<<< HEAD
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_FunctionalBlockDefinition>(ref owner), out value);
=======
				value = owner.ScreenAreas;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EVoxelPlacement_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVoxelPlacement_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, VoxelPlacementOverride?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in VoxelPlacementOverride? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out VoxelPlacementOverride? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECubeSize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeSize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyCubeSize value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyCubeSize value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBlockTopology_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockTopology_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyBlockTopology>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyBlockTopology value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyBlockTopology value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableVector3I value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableVector3I value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EModelOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModelOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableVector3 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableVector3 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECubeDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, PatternDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in PatternDefinition value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out PatternDefinition value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, CubeBlockComponent[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in CubeBlockComponent[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out CubeBlockComponent[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EEffects_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEffects_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, CubeBlockEffectBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in CubeBlockEffectBase[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out CubeBlockEffectBase[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECriticalComponent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalComponent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, CriticalPart>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in CriticalPart value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out CriticalPart value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMountPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MountPoint[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MountPoint[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MountPoint[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, Variant[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in Variant[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out Variant[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EEntityComponents_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponents_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, EntityComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in EntityComponentDefinition[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out EntityComponentDefinition[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPhysicsOption_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicsOption_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyPhysicsOption>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyPhysicsOption value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyPhysicsOption value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBuildProgressModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, List<BuildProgressModel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in List<BuildProgressModel> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out List<BuildProgressModel> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBlockPairName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockPairName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECenter_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableVector3I? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableVector3I? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMirroringX_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMirroringY_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMirroringZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MySymmetryAxisEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MySymmetryAxisEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EEdgeType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEdgeType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDisassembleRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisassembleRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EAutorotateMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAutorotateMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyAutorotateMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyAutorotateMode value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyAutorotateMode value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMirroringBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EUseModelIntersection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseModelIntersection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPrimarySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPrimarySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EActionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBuildType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBuildMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECompoundTemplates_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundTemplates_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECompoundEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MySubBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MySubBlockDefinition[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MySubBlockDefinition[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMultiBlock_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMultiBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ENavigationDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENavigationDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EGuiVisible_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGuiVisible_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBlockVariants_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockVariants_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDirection_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDirection_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyBlockDirection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyBlockDirection value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyBlockDirection value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ERotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyBlockRotation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyBlockRotation value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyBlockRotation value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableDefinitionId[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableDefinitionId[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EGeneratedBlockType_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlockType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMirrored_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirrored_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDamageEffectId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDestroyEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDestroySound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroySound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ESkeleton_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESkeleton_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in List<BoneInfo> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out List<BoneInfo> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ERandomRotation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERandomRotation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EIsAirTight_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsAirTight_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EIsStandAlone_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsStandAlone_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EHasPhysics_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EHasPhysics_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPoints_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPoints_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMaxIntegrity_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMaxIntegrity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDamagedSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamagedSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDamageEffectName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EUsesDeformation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUsesDeformation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in Vector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out Vector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPCU_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCU_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPCUConsole_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCUConsole_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in int? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out int? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPlaceDecals_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPlaceDecals_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDepressurizationEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDepressurizationEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableVector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableVector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableVector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ETieredUpdateTimes_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ETieredUpdateTimes_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MySerializableList<uint>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MySerializableList<uint> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MySerializableList<uint> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

<<<<<<< HEAD
		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ETargetingGroups_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ETargetingGroups_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPriorityModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPriorityModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ENotWorkingPriorityMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENotWorkingPriorityMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDamageMultiplierExplosion_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageMultiplierExplosion_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDamageThreshold_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageThreshold_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDetonateChance_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDetonateChance_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EAmmoExplosionEffect_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAmmoExplosionEffect_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EAmmoExplosionSound_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAmmoExplosionSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDamageEffectOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in Vector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out Vector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EAimingOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAimingOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in Vector3? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out Vector3? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_CubeBlockDefinition>(ref owner), out value);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in float value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out float value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_PhysicalModelDefinition>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextPanelDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextPanelDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextPanelDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TextPanelDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class Sandbox_Common_ObjectBuilders_Definitions_MyObjectBuilder_TextPanelDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TextPanelDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TextPanelDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TextPanelDefinition CreateInstance()
			{
				return new MyObjectBuilder_TextPanelDefinition();
			}

			MyObjectBuilder_TextPanelDefinition IActivator<MyObjectBuilder_TextPanelDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string ResourceSinkGroup;

		[ProtoMember(4)]
		public float RequiredPowerInput = 0.001f;

		[ProtoMember(7)]
		public int TextureResolution = 512;

		[ProtoMember(8)]
		public string PanelMaterialName = "ScreenArea";

		[ProtoMember(10)]
		[DefaultValue(1)]
		public int ScreenWidth = 1;

		[ProtoMember(13)]
		[DefaultValue(1)]
		public int ScreenHeight = 1;

		public float MinFontSize = 0.1f;

		public float MaxFontSize = 10f;

		public float MaxChangingSpeed = 30f;

		[ProtoMember(15)]
		[DefaultValue(180)]
		public float MaxScreenRenderDistance = 180f;
<<<<<<< HEAD
=======

		[ProtoMember(18)]
		public List<ScreenArea> ScreenAreas;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
