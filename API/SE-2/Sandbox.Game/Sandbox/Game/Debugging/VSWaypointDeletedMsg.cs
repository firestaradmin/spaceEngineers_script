using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSWaypointDeletedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSWaypointDeletedMsg_003C_003EId_003C_003EAccessor : IMemberAccessor<VSWaypointDeletedMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointDeletedMsg owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointDeletedMsg owner, out long value)
			{
				value = owner.Id;
			}
		}

		private class Sandbox_Game_Debugging_VSWaypointDeletedMsg_003C_003EActor : IActivator, IActivator<VSWaypointDeletedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSWaypointDeletedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSWaypointDeletedMsg CreateInstance()
			{
				return (VSWaypointDeletedMsg)(object)default(VSWaypointDeletedMsg);
			}

			VSWaypointDeletedMsg IActivator<VSWaypointDeletedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_WDLT";
		}
	}
}
