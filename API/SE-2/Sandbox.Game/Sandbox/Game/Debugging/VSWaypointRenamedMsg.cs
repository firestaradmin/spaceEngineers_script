using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSWaypointRenamedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSWaypointRenamedMsg_003C_003EId_003C_003EAccessor : IMemberAccessor<VSWaypointRenamedMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointRenamedMsg owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointRenamedMsg owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Debugging_VSWaypointRenamedMsg_003C_003EName_003C_003EAccessor : IMemberAccessor<VSWaypointRenamedMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSWaypointRenamedMsg owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSWaypointRenamedMsg owner, out string value)
			{
				value = owner.Name;
			}
		}

		private class Sandbox_Game_Debugging_VSWaypointRenamedMsg_003C_003EActor : IActivator, IActivator<VSWaypointRenamedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSWaypointRenamedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSWaypointRenamedMsg CreateInstance()
			{
				return (VSWaypointRenamedMsg)(object)default(VSWaypointRenamedMsg);
			}

			VSWaypointRenamedMsg IActivator<VSWaypointRenamedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		[ProtoMember(10)]
		public string Name;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_WREN";
		}
	}
}
