using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSTriggersMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSTriggersMsg_003C_003ETriggers_003C_003EAccessor : IMemberAccessor<VSTriggersMsg, MyObjectBuilder_AreaTrigger[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSTriggersMsg owner, in MyObjectBuilder_AreaTrigger[] value)
			{
				owner.Triggers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSTriggersMsg owner, out MyObjectBuilder_AreaTrigger[] value)
			{
				value = owner.Triggers;
			}
		}

		protected class Sandbox_Game_Debugging_VSTriggersMsg_003C_003EParents_003C_003EAccessor : IMemberAccessor<VSTriggersMsg, long[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSTriggersMsg owner, in long[] value)
			{
				owner.Parents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSTriggersMsg owner, out long[] value)
			{
				value = owner.Parents;
			}
		}

		private class Sandbox_Game_Debugging_VSTriggersMsg_003C_003EActor : IActivator, IActivator<VSTriggersMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSTriggersMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSTriggersMsg CreateInstance()
			{
				return (VSTriggersMsg)(object)default(VSTriggersMsg);
			}

			VSTriggersMsg IActivator<VSTriggersMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public MyObjectBuilder_AreaTrigger[] Triggers;

		[ProtoMember(10)]
		public long[] Parents;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_TRGS";
		}
	}
}
