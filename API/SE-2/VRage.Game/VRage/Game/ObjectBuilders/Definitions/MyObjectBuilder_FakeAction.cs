using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_FakeAction : MyObjectBuilder_Action
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FakeAction_003C_003EFakeString_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FakeAction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FakeAction owner, in string value)
			{
				owner.FakeString = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FakeAction owner, out string value)
			{
				value = owner.FakeString;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FakeAction_003C_003EAction_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_Action_003C_003EAction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FakeAction, ButtonAction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FakeAction owner, in ButtonAction value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_FakeAction, MyObjectBuilder_Action>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FakeAction owner, out ButtonAction value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_FakeAction, MyObjectBuilder_Action>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FakeAction_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FakeAction>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FakeAction();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FakeAction CreateInstance()
			{
				return new MyObjectBuilder_FakeAction();
			}

			MyObjectBuilder_FakeAction IActivator<MyObjectBuilder_FakeAction>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string FakeString;

		public MyObjectBuilder_FakeAction()
		{
		}

		public MyObjectBuilder_FakeAction(string fakeString, ButtonAction action = ButtonAction.Pressed)
		{
			FakeString = fakeString;
			Action = action;
		}
	}
}
