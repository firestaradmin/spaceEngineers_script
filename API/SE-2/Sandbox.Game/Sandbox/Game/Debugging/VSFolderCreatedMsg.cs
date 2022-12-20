using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSFolderCreatedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSFolderCreatedMsg_003C_003EPath_003C_003EAccessor : IMemberAccessor<VSFolderCreatedMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSFolderCreatedMsg owner, in string value)
			{
				owner.Path = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSFolderCreatedMsg owner, out string value)
			{
				value = owner.Path;
			}
		}

		private class Sandbox_Game_Debugging_VSFolderCreatedMsg_003C_003EActor : IActivator, IActivator<VSFolderCreatedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSFolderCreatedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSFolderCreatedMsg CreateInstance()
			{
				return (VSFolderCreatedMsg)(object)default(VSFolderCreatedMsg);
			}

			VSFolderCreatedMsg IActivator<VSFolderCreatedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string Path;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_FCRE";
		}
	}
}
