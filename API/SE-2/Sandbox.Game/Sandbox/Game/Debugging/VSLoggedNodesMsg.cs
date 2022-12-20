using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Game.VisualScripting;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSLoggedNodesMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSLoggedNodesMsg_003C_003ETime_003C_003EAccessor : IMemberAccessor<VSLoggedNodesMsg, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSLoggedNodesMsg owner, in int value)
			{
				owner.Time = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSLoggedNodesMsg owner, out int value)
			{
				value = owner.Time;
			}
		}

		protected class Sandbox_Game_Debugging_VSLoggedNodesMsg_003C_003ENodes_003C_003EAccessor : IMemberAccessor<VSLoggedNodesMsg, MyDebuggingNodeLog[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSLoggedNodesMsg owner, in MyDebuggingNodeLog[] value)
			{
				owner.Nodes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSLoggedNodesMsg owner, out MyDebuggingNodeLog[] value)
			{
				value = owner.Nodes;
			}
		}

		protected class Sandbox_Game_Debugging_VSLoggedNodesMsg_003C_003EStateMachines_003C_003EAccessor : IMemberAccessor<VSLoggedNodesMsg, MyDebuggingStateMachine[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSLoggedNodesMsg owner, in MyDebuggingStateMachine[] value)
			{
				owner.StateMachines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSLoggedNodesMsg owner, out MyDebuggingStateMachine[] value)
			{
				value = owner.StateMachines;
			}
		}

		private class Sandbox_Game_Debugging_VSLoggedNodesMsg_003C_003EActor : IActivator, IActivator<VSLoggedNodesMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSLoggedNodesMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSLoggedNodesMsg CreateInstance()
			{
				return (VSLoggedNodesMsg)(object)default(VSLoggedNodesMsg);
			}

			VSLoggedNodesMsg IActivator<VSLoggedNodesMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public int Time;

		[ProtoMember(10)]
		public MyDebuggingNodeLog[] Nodes;

		[ProtoMember(15)]
		public MyDebuggingStateMachine[] StateMachines;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_LGN";
		}
	}
}
