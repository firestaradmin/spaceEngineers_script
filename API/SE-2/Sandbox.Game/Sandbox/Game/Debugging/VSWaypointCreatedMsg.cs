using System.Runtime.CompilerServices;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSWaypointCreatedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSWaypointCreatedMsg_003C_003EWaypoint_003C_003EAccessor : IMemberAccessor<VSWaypointCreatedMsg, MyObjectBuilder_Waypoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointCreatedMsg owner, in MyObjectBuilder_Waypoint value)
			{
				owner.Waypoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointCreatedMsg owner, out MyObjectBuilder_Waypoint value)
			{
				value = owner.Waypoint;
			}
		}

		private class Sandbox_Game_Debugging_VSWaypointCreatedMsg_003C_003EActor : IActivator, IActivator<VSWaypointCreatedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSWaypointCreatedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSWaypointCreatedMsg CreateInstance()
			{
				return (VSWaypointCreatedMsg)(object)default(VSWaypointCreatedMsg);
			}

			VSWaypointCreatedMsg IActivator<VSWaypointCreatedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public MyObjectBuilder_Waypoint Waypoint;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_WCTD";
		}
	}
}
