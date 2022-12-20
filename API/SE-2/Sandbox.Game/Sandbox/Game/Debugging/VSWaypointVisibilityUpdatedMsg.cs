using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSWaypointVisibilityUpdatedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSWaypointVisibilityUpdatedMsg_003C_003EId_003C_003EAccessor : IMemberAccessor<VSWaypointVisibilityUpdatedMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointVisibilityUpdatedMsg owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointVisibilityUpdatedMsg owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Debugging_VSWaypointVisibilityUpdatedMsg_003C_003EVisible_003C_003EAccessor : IMemberAccessor<VSWaypointVisibilityUpdatedMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointVisibilityUpdatedMsg owner, in bool value)
			{
				owner.Visible = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointVisibilityUpdatedMsg owner, out bool value)
			{
				value = owner.Visible;
			}
		}

		private class Sandbox_Game_Debugging_VSWaypointVisibilityUpdatedMsg_003C_003EActor : IActivator, IActivator<VSWaypointVisibilityUpdatedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSWaypointVisibilityUpdatedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSWaypointVisibilityUpdatedMsg CreateInstance()
			{
				return (VSWaypointVisibilityUpdatedMsg)(object)default(VSWaypointVisibilityUpdatedMsg);
			}

			VSWaypointVisibilityUpdatedMsg IActivator<VSWaypointVisibilityUpdatedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		[ProtoMember(10)]
		public bool Visible;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_WVUD";
		}
	}
}
