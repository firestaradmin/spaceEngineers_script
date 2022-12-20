using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TargetingMarkersStyle : MyObjectBuilder_StatControls
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003EApplyHudScale_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EApplyHudScale_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003EPosition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in Vector2 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out Vector2 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in ConditionBase value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out ConditionBase value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003EStatStyles_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EStatStyles_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatVisualStyle[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in MyObjectBuilder_StatVisualStyle[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out MyObjectBuilder_StatVisualStyle[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_StatControls>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetingMarkersStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetingMarkersStyle owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetingMarkersStyle owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetingMarkersStyle, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TargetingMarkersStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TargetingMarkersStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TargetingMarkersStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TargetingMarkersStyle CreateInstance()
			{
				return new MyObjectBuilder_TargetingMarkersStyle();
			}

			MyObjectBuilder_TargetingMarkersStyle IActivator<MyObjectBuilder_TargetingMarkersStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
