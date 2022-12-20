using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSWaypointFreezeUpdatedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSWaypointFreezeUpdatedMsg_003C_003EId_003C_003EAccessor : IMemberAccessor<VSWaypointFreezeUpdatedMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointFreezeUpdatedMsg owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointFreezeUpdatedMsg owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Debugging_VSWaypointFreezeUpdatedMsg_003C_003EFreeze_003C_003EAccessor : IMemberAccessor<VSWaypointFreezeUpdatedMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointFreezeUpdatedMsg owner, in bool value)
			{
				owner.Freeze = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointFreezeUpdatedMsg owner, out bool value)
			{
				value = owner.Freeze;
			}
		}

		private class Sandbox_Game_Debugging_VSWaypointFreezeUpdatedMsg_003C_003EActor : IActivator, IActivator<VSWaypointFreezeUpdatedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSWaypointFreezeUpdatedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSWaypointFreezeUpdatedMsg CreateInstance()
			{
				return (VSWaypointFreezeUpdatedMsg)(object)default(VSWaypointFreezeUpdatedMsg);
			}

			VSWaypointFreezeUpdatedMsg IActivator<VSWaypointFreezeUpdatedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		[ProtoMember(10)]
		public bool Freeze;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_WFUD";
		}
	}
}
