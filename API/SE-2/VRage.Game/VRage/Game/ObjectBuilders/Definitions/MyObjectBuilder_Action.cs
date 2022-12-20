using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Action
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_Action_003C_003EAction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Action, ButtonAction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Action owner, in ButtonAction value)
			{
				owner.Action = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Action owner, out ButtonAction value)
			{
				value = owner.Action;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_Action_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Action>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Action();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Action CreateInstance()
			{
				return new MyObjectBuilder_Action();
			}

			MyObjectBuilder_Action IActivator<MyObjectBuilder_Action>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public ButtonAction Action;
	}
}
