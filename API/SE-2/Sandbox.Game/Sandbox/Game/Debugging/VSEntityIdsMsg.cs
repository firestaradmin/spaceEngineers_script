using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSEntityIdsMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSEntityIdsMsg_003C_003EData_003C_003EAccessor : IMemberAccessor<VSEntityIdsMsg, SimpleEntityInfo[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSEntityIdsMsg owner, in SimpleEntityInfo[] value)
			{
				owner.Data = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSEntityIdsMsg owner, out SimpleEntityInfo[] value)
			{
				value = owner.Data;
			}
		}

		private class Sandbox_Game_Debugging_VSEntityIdsMsg_003C_003EActor : IActivator, IActivator<VSEntityIdsMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSEntityIdsMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSEntityIdsMsg CreateInstance()
			{
				return (VSEntityIdsMsg)(object)default(VSEntityIdsMsg);
			}

			VSEntityIdsMsg IActivator<VSEntityIdsMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public SimpleEntityInfo[] Data;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_ENTI";
		}
	}
}
