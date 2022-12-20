using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSFolderRenamedMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSFolderRenamedMsg_003C_003EOldPath_003C_003EAccessor : IMemberAccessor<VSFolderRenamedMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSFolderRenamedMsg owner, in string value)
			{
				owner.OldPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSFolderRenamedMsg owner, out string value)
			{
				value = owner.OldPath;
			}
		}

		protected class Sandbox_Game_Debugging_VSFolderRenamedMsg_003C_003ENewPath_003C_003EAccessor : IMemberAccessor<VSFolderRenamedMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSFolderRenamedMsg owner, in string value)
			{
				owner.NewPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSFolderRenamedMsg owner, out string value)
			{
				value = owner.NewPath;
			}
		}

		private class Sandbox_Game_Debugging_VSFolderRenamedMsg_003C_003EActor : IActivator, IActivator<VSFolderRenamedMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSFolderRenamedMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSFolderRenamedMsg CreateInstance()
			{
				return (VSFolderRenamedMsg)(object)default(VSFolderRenamedMsg);
			}

			VSFolderRenamedMsg IActivator<VSFolderRenamedMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string OldPath;

		[ProtoMember(10)]
		public string NewPath;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_FREN";
		}
	}
}
