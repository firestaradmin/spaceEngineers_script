using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSFolderDeletedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSFolderDeletedMsg_003C_003EPath_003C_003EAccessor : IMemberAccessor<VSFolderDeletedMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSFolderDeletedMsg owner, in string value)
			{
				owner.Path = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSFolderDeletedMsg owner, out string value)
			{
				value = owner.Path;
			}
		}

		private class Sandbox_Game_Debugging_VSFolderDeletedMsg_003C_003EActor : IActivator, IActivator<VSFolderDeletedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSFolderDeletedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSFolderDeletedMsg CreateInstance()
			{
				return (VSFolderDeletedMsg)(object)default(VSFolderDeletedMsg);
			}

			VSFolderDeletedMsg IActivator<VSFolderDeletedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string Path;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_FDEL";
		}
	}
}
