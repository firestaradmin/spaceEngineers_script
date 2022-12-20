using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSReqWaypointDeleteMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSReqWaypointDeleteMsg_003C_003EId_003C_003EAccessor : IMemberAccessor<VSReqWaypointDeleteMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSReqWaypointDeleteMsg owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSReqWaypointDeleteMsg owner, out long value)
			{
				value = owner.Id;
			}
		}

		private class Sandbox_Game_Debugging_VSReqWaypointDeleteMsg_003C_003EActor : IActivator, IActivator<VSReqWaypointDeleteMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSReqWaypointDeleteMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSReqWaypointDeleteMsg CreateInstance()
			{
				return (VSReqWaypointDeleteMsg)(object)default(VSReqWaypointDeleteMsg);
			}

			VSReqWaypointDeleteMsg IActivator<VSReqWaypointDeleteMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_RWDT";
		}
	}
}
