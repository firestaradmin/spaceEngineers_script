using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSEntitiesMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSEntitiesMsg_003C_003EWaypoints_003C_003EAccessor : IMemberAccessor<VSEntitiesMsg, List<MyObjectBuilder_Waypoint>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSEntitiesMsg owner, in List<MyObjectBuilder_Waypoint> value)
			{
				owner.Waypoints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSEntitiesMsg owner, out List<MyObjectBuilder_Waypoint> value)
			{
				value = owner.Waypoints;
			}
		}

		protected class Sandbox_Game_Debugging_VSEntitiesMsg_003C_003EFolders_003C_003EAccessor : IMemberAccessor<VSEntitiesMsg, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSEntitiesMsg owner, in string[] value)
			{
				owner.Folders = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSEntitiesMsg owner, out string[] value)
			{
				value = owner.Folders;
			}
		}

		private class Sandbox_Game_Debugging_VSEntitiesMsg_003C_003EActor : IActivator, IActivator<VSEntitiesMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSEntitiesMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSEntitiesMsg CreateInstance()
			{
				return (VSEntitiesMsg)(object)default(VSEntitiesMsg);
			}

			VSEntitiesMsg IActivator<VSEntitiesMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public List<MyObjectBuilder_Waypoint> Waypoints;

		[ProtoMember(10)]
		public string[] Folders;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_ENTS";
		}
	}
}
