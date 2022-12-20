using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Input;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_MouseButtonAction : MyObjectBuilder_Action
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_MouseButtonAction_003C_003EMouseButton_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MouseButtonAction, MyMouseButtonsEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MouseButtonAction owner, in MyMouseButtonsEnum value)
			{
				owner.MouseButton = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MouseButtonAction owner, out MyMouseButtonsEnum value)
			{
				value = owner.MouseButton;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_MouseButtonAction_003C_003EAction_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_Action_003C_003EAction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MouseButtonAction, ButtonAction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MouseButtonAction owner, in ButtonAction value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_MouseButtonAction, MyObjectBuilder_Action>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MouseButtonAction owner, out ButtonAction value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_MouseButtonAction, MyObjectBuilder_Action>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_MouseButtonAction_003C_003EActor : IActivator, IActivator<MyObjectBuilder_MouseButtonAction>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_MouseButtonAction();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_MouseButtonAction CreateInstance()
			{
				return new MyObjectBuilder_MouseButtonAction();
			}

			MyObjectBuilder_MouseButtonAction IActivator<MyObjectBuilder_MouseButtonAction>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyMouseButtonsEnum MouseButton;

		public MyObjectBuilder_MouseButtonAction()
		{
		}

		public MyObjectBuilder_MouseButtonAction(MyMouseButtonsEnum mouseButton, ButtonAction action = ButtonAction.Pressed)
		{
			MouseButton = mouseButton;
			Action = action;
		}
	}
}
