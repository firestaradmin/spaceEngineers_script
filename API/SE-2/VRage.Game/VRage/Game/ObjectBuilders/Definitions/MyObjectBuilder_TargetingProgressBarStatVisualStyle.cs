using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.GUI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TargetingProgressBarStatVisualStyle : MyObjectBuilder_StatVisualStyle
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ESegmentSizePx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector2 value)
			{
				owner.SegmentSizePx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector2 value)
			{
				value = owner.SegmentSizePx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ESegmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				owner.SegmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				value = owner.SegmentTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EBackgroudTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.BackgroudTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.BackgroudTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EFirstSegmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.FirstSegmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.FirstSegmentTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ELastSegmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.LastSegmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.LastSegmentTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EFilledTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.FilledTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.FilledTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ESegmentOrigin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector2? value)
			{
				owner.SegmentOrigin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector2? value)
			{
				value = owner.SegmentOrigin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ESpacingAngle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in float? value)
			{
				owner.SpacingAngle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out float? value)
			{
				value = owner.SpacingAngle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EAngleOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in float? value)
			{
				owner.AngleOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out float? value)
			{
				value = owner.AngleOffset;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ENumberOfSegments_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in int? value)
			{
				owner.NumberOfSegments = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out int? value)
			{
				value = owner.NumberOfSegments;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EShowEmptySegments_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in bool? value)
			{
				owner.ShowEmptySegments = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out bool? value)
			{
				value = owner.ShowEmptySegments;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EEnemyFocusSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.EnemyFocusSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.EnemyFocusSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EEnemyLockingSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.EnemyLockingSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.EnemyLockingSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ENeutralFocusSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.NeutralFocusSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.NeutralFocusSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ENeutralLockingSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.NeutralLockingSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.NeutralLockingSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EFriendlyFocusSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.FriendlyFocusSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.FriendlyFocusSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EFriendlyLockingSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.FriendlyLockingSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.FriendlyLockingSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EStatId_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EStatId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ESizePx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESizePx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector2 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector2 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EOffsetPx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EOffsetPx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in Vector2 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out Vector2 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EBlink_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyAlphaBlinkBehavior value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyAlphaBlinkBehavior value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ECategory_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ECategory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, VisualStyleCategory?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in VisualStyleCategory? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out VisualStyleCategory? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_StatVisualStyle>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingProgressBarStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingProgressBarStatVisualStyle owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingProgressBarStatVisualStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingProgressBarStatVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TargetingProgressBarStatVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TargetingProgressBarStatVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TargetingProgressBarStatVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_TargetingProgressBarStatVisualStyle();
			}

			MyObjectBuilder_TargetingProgressBarStatVisualStyle IActivator<MyObjectBuilder_TargetingProgressBarStatVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector2 SegmentSizePx;

		public MyStringHash SegmentTexture;

		public MyStringHash? BackgroudTexture;

		public MyStringHash? FirstSegmentTexture;

		public MyStringHash? LastSegmentTexture;

		public MyStringHash? FilledTexture;

		public Vector2? SegmentOrigin;

		public float? SpacingAngle;

		public float? AngleOffset;

		public int? NumberOfSegments;

		public bool? ShowEmptySegments;

		public Vector4? EnemyFocusSegmentColorMask;

		public Vector4? EnemyLockingSegmentColorMask;

		public Vector4? NeutralFocusSegmentColorMask;

		public Vector4? NeutralLockingSegmentColorMask;

		public Vector4? FriendlyFocusSegmentColorMask;

		public Vector4? FriendlyLockingSegmentColorMask;
	}
}
