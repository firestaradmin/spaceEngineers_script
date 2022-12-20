using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSWaypointPathUpdatedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSWaypointPathUpdatedMsg_003C_003EId_003C_003EAccessor : IMemberAccessor<VSWaypointPathUpdatedMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointPathUpdatedMsg owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointPathUpdatedMsg owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Debugging_VSWaypointPathUpdatedMsg_003C_003EPath_003C_003EAccessor : IMemberAccessor<VSWaypointPathUpdatedMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointPathUpdatedMsg owner, in string value)
			{
				owner.Path = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointPathUpdatedMsg owner, out string value)
			{
				value = owner.Path;
			}
		}

		private class Sandbox_Game_Debugging_VSWaypointPathUpdatedMsg_003C_003EActor : IActivator, IActivator<VSWaypointPathUpdatedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSWaypointPathUpdatedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSWaypointPathUpdatedMsg CreateInstance()
			{
				return (VSWaypointPathUpdatedMsg)(object)default(VSWaypointPathUpdatedMsg);
			}

			VSWaypointPathUpdatedMsg IActivator<VSWaypointPathUpdatedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		[ProtoMember(10)]
		public string Path;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_WPUD";
		}
	}
}
