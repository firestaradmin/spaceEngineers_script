using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.Debugging
{
	[ProtoContract]
	public struct ACConnectToEditorMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class VRage_Game_Debugging_ACConnectToEditorMsg_003C_003EACName_003C_003EAccessor : IMemberAccessor<ACConnectToEditorMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ACConnectToEditorMsg owner, in string value)
			{
				owner.ACName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ACConnectToEditorMsg owner, out string value)
			{
				value = owner.ACName;
			}
		}

		private class VRage_Game_Debugging_ACConnectToEditorMsg_003C_003EActor : IActivator, IActivator<ACConnectToEditorMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(ACConnectToEditorMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ACConnectToEditorMsg CreateInstance()
			{
				return (ACConnectToEditorMsg)(object)default(ACConnectToEditorMsg);
			}

			ACConnectToEditorMsg IActivator<ACConnectToEditorMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string ACName;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "AC_CON";
		}
	}
}
