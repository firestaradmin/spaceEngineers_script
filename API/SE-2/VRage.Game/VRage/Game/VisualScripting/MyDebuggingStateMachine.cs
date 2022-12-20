using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.VisualScripting
{
	[ProtoContract]
	public struct MyDebuggingStateMachine
	{
		protected class VRage_Game_VisualScripting_MyDebuggingStateMachine_003C_003ESMName_003C_003EAccessor : IMemberAccessor<MyDebuggingStateMachine, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDebuggingStateMachine owner, in string value)
			{
				owner.SMName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDebuggingStateMachine owner, out string value)
			{
				value = owner.SMName;
			}
		}

		protected class VRage_Game_VisualScripting_MyDebuggingStateMachine_003C_003ECursors_003C_003EAccessor : IMemberAccessor<MyDebuggingStateMachine, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDebuggingStateMachine owner, in string[] value)
			{
				owner.Cursors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDebuggingStateMachine owner, out string[] value)
			{
				value = owner.Cursors;
			}
		}

		private class VRage_Game_VisualScripting_MyDebuggingStateMachine_003C_003EActor : IActivator, IActivator<MyDebuggingStateMachine>
		{
			private sealed override object CreateInstance()
			{
				return default(MyDebuggingStateMachine);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebuggingStateMachine CreateInstance()
			{
				return (MyDebuggingStateMachine)(object)default(MyDebuggingStateMachine);
			}

			MyDebuggingStateMachine IActivator<MyDebuggingStateMachine>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string SMName;

		[ProtoMember(10)]
		public string[] Cursors;
	}
}
