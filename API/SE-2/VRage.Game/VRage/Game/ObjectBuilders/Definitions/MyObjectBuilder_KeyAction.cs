using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Input;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_KeyAction : MyObjectBuilder_Action
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_KeyAction_003C_003EKey_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_KeyAction, MyKeys>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_KeyAction owner, in MyKeys value)
			{
				owner.Key = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_KeyAction owner, out MyKeys value)
			{
				value = owner.Key;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_KeyAction_003C_003EAction_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_Action_003C_003EAction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_KeyAction, ButtonAction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_KeyAction owner, in ButtonAction value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_KeyAction, MyObjectBuilder_Action>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_KeyAction owner, out ButtonAction value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_KeyAction, MyObjectBuilder_Action>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_KeyAction_003C_003EActor : IActivator, IActivator<MyObjectBuilder_KeyAction>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_KeyAction();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_KeyAction CreateInstance()
			{
				return new MyObjectBuilder_KeyAction();
			}

			MyObjectBuilder_KeyAction IActivator<MyObjectBuilder_KeyAction>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyKeys Key;

		public MyObjectBuilder_KeyAction()
		{
		}

		public MyObjectBuilder_KeyAction(MyKeys key, ButtonAction action = ButtonAction.Pressed)
		{
			Key = key;
			Action = action;
		}
	}
}
