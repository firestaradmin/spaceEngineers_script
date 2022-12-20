using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Input;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ControllerButtonAction : MyObjectBuilder_Action
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControllerButtonAction_003C_003EGamepadButton_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerButtonAction, MyGamepadButtons>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControllerButtonAction owner, in MyGamepadButtons value)
			{
				owner.GamepadButton = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControllerButtonAction owner, out MyGamepadButtons value)
			{
				value = owner.GamepadButton;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControllerButtonAction_003C_003EAction_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_Action_003C_003EAction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerButtonAction, ButtonAction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControllerButtonAction owner, in ButtonAction value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControllerButtonAction, MyObjectBuilder_Action>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControllerButtonAction owner, out ButtonAction value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ControllerButtonAction, MyObjectBuilder_Action>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControllerButtonAction_003C_003EJoystickButton_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerButtonAction, MyJoystickButtonsEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ControllerButtonAction owner, in MyJoystickButtonsEnum value)
			{
				owner.JoystickButton = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ControllerButtonAction owner, out MyJoystickButtonsEnum value)
			{
				value = owner.JoystickButton;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ControllerButtonAction_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ControllerButtonAction>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ControllerButtonAction();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ControllerButtonAction CreateInstance()
			{
				return new MyObjectBuilder_ControllerButtonAction();
			}

			MyObjectBuilder_ControllerButtonAction IActivator<MyObjectBuilder_ControllerButtonAction>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyGamepadButtons GamepadButton;

		[NoSerialize]
		public MyJoystickButtonsEnum JoystickButton
		{
			get
			{
				return (MyJoystickButtonsEnum)GamepadButton;
			}
			set
			{
				GamepadButton = (MyGamepadButtons)value;
			}
		}

		public MyObjectBuilder_ControllerButtonAction()
		{
		}

		public MyObjectBuilder_ControllerButtonAction(MyJoystickButtonsEnum joystickButtonTranslated, ButtonAction action = ButtonAction.Pressed)
		{
			JoystickButton = joystickButtonTranslated;
			Action = action;
		}
	}
}
