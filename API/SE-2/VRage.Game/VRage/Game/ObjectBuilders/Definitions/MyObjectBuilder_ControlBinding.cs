using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ControlBinding : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in SerializableDefinitionId value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out SerializableDefinitionId value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EBindingType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, MyControlBindingType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MyControlBindingType value)
			{
				owner.BindingType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MyControlBindingType value)
			{
				value = owner.BindingType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003ECondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, MyPredefinedContitions>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MyPredefinedContitions value)
			{
				owner.Condition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MyPredefinedContitions value)
			{
				value = owner.Condition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EControlType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, MyGuiControlTypeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MyGuiControlTypeEnum value)
			{
				owner.ControlType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MyGuiControlTypeEnum value)
			{
				value = owner.ControlType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EActions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, MySerializableList<MyObjectBuilder_Action>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MySerializableList<MyObjectBuilder_Action> value)
			{
				owner.Actions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MySerializableList<MyObjectBuilder_Action> value)
			{
				value = owner.Actions;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EModifiers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, MySerializableList<MyObjectBuilder_Action>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MySerializableList<MyObjectBuilder_Action> value)
			{
				owner.Modifiers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MySerializableList<MyObjectBuilder_Action> value)
			{
				value = owner.Modifiers;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EGamepadAxes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControlBinding, MySerializableList<MyGamepadAxes>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MySerializableList<MyGamepadAxes> value)
			{
				owner.GamepadAxes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MySerializableList<MyGamepadAxes> value)
			{
				value = owner.GamepadAxes;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControlBinding, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControlBinding, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControlBinding, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControlBinding, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControlBinding owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControlBinding owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControlBinding, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControlBinding_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ControlBinding>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ControlBinding();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ControlBinding CreateInstance()
			{
				return new MyObjectBuilder_ControlBinding();
			}

			MyObjectBuilder_ControlBinding IActivator<MyObjectBuilder_ControlBinding>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId Id;

		[ProtoMember(3)]
		public MyControlBindingType BindingType;

		[ProtoMember(5)]
		public MyPredefinedContitions Condition;

		[ProtoMember(7)]
		public MyGuiControlTypeEnum ControlType;

		[ProtoMember(9)]
		public MySerializableList<MyObjectBuilder_Action> Actions;

		[ProtoMember(11)]
		public MySerializableList<MyObjectBuilder_Action> Modifiers;

		[ProtoMember(13)]
		public MySerializableList<MyGamepadAxes> GamepadAxes;
	}
}
