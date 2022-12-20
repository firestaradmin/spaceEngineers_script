using System;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	[Serializable]
	public struct PlayerDataMsg
	{
		protected class Sandbox_Engine_Multiplayer_PlayerDataMsg_003C_003EClientSteamId_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in ulong value)
			{
				owner.ClientSteamId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out ulong value)
			{
				value = owner.ClientSteamId;
			}
		}

		protected class Sandbox_Engine_Multiplayer_PlayerDataMsg_003C_003EPlayerSerialId_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in int value)
			{
				owner.PlayerSerialId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out int value)
			{
				value = owner.PlayerSerialId;
			}
		}

		protected class Sandbox_Engine_Multiplayer_PlayerDataMsg_003C_003ENewIdentity_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in bool value)
			{
				owner.NewIdentity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out bool value)
			{
				value = owner.NewIdentity;
			}
		}

		protected class Sandbox_Engine_Multiplayer_PlayerDataMsg_003C_003EPlayerBuilder_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, MyObjectBuilder_Player>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in MyObjectBuilder_Player value)
			{
				owner.PlayerBuilder = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out MyObjectBuilder_Player value)
			{
				value = owner.PlayerBuilder;
			}
		}

		public ulong ClientSteamId;

		public int PlayerSerialId;

		public bool NewIdentity;

		public MyObjectBuilder_Player PlayerBuilder;
	}
}
