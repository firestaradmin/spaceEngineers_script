namespace VRage.GameServices
{
	public interface IMyNetworkingChat
	{
		bool IsTextChatAvailable { get; }

		bool IsCrossTextChatAvailable { get; }

		bool IsVoiceChatAvailable { get; }

		bool IsCrossVoiceChatAvailable { get; }

		int GetChatMaxMessageSize();

		void SetPlayerMuted(ulong playerId, bool muted);

		MyPlayerChatState GetPlayerChatState(ulong playerId);

		bool IsTextChatAvailableForUserId(ulong userId, bool crossUser);

		bool IsVoiceChatAvailableForUserId(ulong userId, bool crossUser);

		void UpdateChatAvailability();

		void WarmupPlayerCache(ulong userId, bool crossUser);
	}
}
