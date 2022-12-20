using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.Debugging
{
	[ProtoContract]
	public struct ACReloadInGameMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class VRage_Game_Debugging_ACReloadInGameMsg_003C_003EACName_003C_003EAccessor : IMemberAccessor<ACReloadInGameMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ACReloadInGameMsg owner, in string value)
			{
				owner.ACName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ACReloadInGameMsg owner, out string value)
			{
				value = owner.ACName;
			}
		}

		protected class VRage_Game_Debugging_ACReloadInGameMsg_003C_003EACAddress_003C_003EAccessor : IMemberAccessor<ACReloadInGameMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ACReloadInGameMsg owner, in string value)
			{
				owner.ACAddress = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ACReloadInGameMsg owner, out string value)
			{
				value = owner.ACAddress;
			}
		}

		protected class VRage_Game_Debugging_ACReloadInGameMsg_003C_003EACContentAddress_003C_003EAccessor : IMemberAccessor<ACReloadInGameMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ACReloadInGameMsg owner, in string value)
			{
				owner.ACContentAddress = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ACReloadInGameMsg owner, out string value)
			{
				value = owner.ACContentAddress;
			}
		}

		private class VRage_Game_Debugging_ACReloadInGameMsg_003C_003EActor : IActivator, IActivator<ACReloadInGameMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(ACReloadInGameMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ACReloadInGameMsg CreateInstance()
			{
				return (ACReloadInGameMsg)(object)default(ACReloadInGameMsg);
			}

			ACReloadInGameMsg IActivator<ACReloadInGameMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string ACName;

		[ProtoMember(10)]
		public string ACAddress;

		[ProtoMember(15)]
		public string ACContentAddress;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "AC_LOAD";
		}
	}
}
