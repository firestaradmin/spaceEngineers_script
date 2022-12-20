using System;
using System.Collections.Generic;
using VRage.Library.Parallelization;

namespace VRage.GameServices
{
	public class SimpleNetworkingChat : IMyNetworkingChat
	{
		private readonly IMyGameService m_service;

		private Action m_onPrivilegeUpdateDone;

		private AtomicFlag m_updatingPrivileges;

		private readonly int m_maxMessageSize;

		private readonly HashSet<ulong> m_mutedPlayers = new HashSet<ulong>();

		private readonly Dictionary<ulong, bool> m_textChatAvailable = new Dictionary<ulong, bool>();

		private readonly Dictionary<ulong, bool> m_voiceChatAvailable = new Dictionary<ulong, bool>();

		public bool IsTextChatAvailable { get; private set; }

		public bool IsCrossTextChatAvailable { get; private set; }

		public bool IsVoiceChatAvailable { get; private set; }

		public bool IsCrossVoiceChatAvailable { get; private set; }

		public SimpleNetworkingChat(IMyGameService service, int maxMessageSize)
		{
			m_service = service;
			m_maxMessageSize = maxMessageSize;
		}

		public bool IsTextChatAvailableForUserId(ulong userId, bool crossUser)
		{
			return IsChatAvailableForUserId(userId, crossUser, IsTextChatAvailable, IsCrossTextChatAvailable, m_textChatAvailable, Permissions.CommunicationsText);
		}

		public bool IsVoiceChatAvailableForUserId(ulong userId, bool crossUser)
		{
			return IsChatAvailableForUserId(userId, crossUser, IsVoiceChatAvailable, IsCrossVoiceChatAvailable, m_voiceChatAvailable, Permissions.CommunicationsVoice);
		}

		private bool IsChatAvailableForUserId(ulong userId, bool crossUser, bool permitted, bool crossPermitted, Dictionary<ulong, bool> userPermitted, Permissions permissionType)
		{
			if (!permitted)
			{
				return false;
			}
			if (crossUser)
			{
				return crossPermitted;
			}
			if (!userPermitted.TryGetValue(userId, out var value))
			{
				m_service.RequestPermissionsWithTargetUser(permissionType, userId, delegate(PermissionResult x)
				{
					userPermitted[userId] = x == PermissionResult.Granted;
				});
				return true;
			}
			return value;
		}

		public virtual void UpdateChatAvailability()
		{
			if (!m_updatingPrivileges.Set())
			{
				return;
			}
			IsTextChatAvailable = false;
			IsCrossTextChatAvailable = false;
			IsVoiceChatAvailable = false;
			IsCrossVoiceChatAvailable = false;
			int remainingRequestChains = 2;
			m_service.RequestPermissions(Permissions.CommunicationsText, attemptResolution: false, delegate(bool granted)
			{
				if (granted)
				{
					IsTextChatAvailable = true;
					m_service.RequestPermissionsWithTargetUser(Permissions.CommunicationsText, 0uL, delegate(bool crossGranted)
					{
						IsCrossTextChatAvailable = crossGranted;
						OnChainDone();
					});
				}
				else
				{
					OnChainDone();
				}
			});
			m_service.RequestPermissions(Permissions.CommunicationsVoice, attemptResolution: false, delegate(bool granted)
			{
				if (granted)
				{
					IsVoiceChatAvailable = true;
					m_service.RequestPermissionsWithTargetUser(Permissions.CommunicationsVoice, 0uL, delegate(bool crossGranted)
					{
						IsCrossVoiceChatAvailable = crossGranted;
						OnChainDone();
					});
				}
				else
				{
					OnChainDone();
				}
			});
			void OnChainDone()
			{
				remainingRequestChains--;
				if (remainingRequestChains == 0)
				{
					m_updatingPrivileges.Clear();
					m_onPrivilegeUpdateDone.InvokeIfNotNull();
					m_onPrivilegeUpdateDone = null;
				}
			}
		}

		public void WarmupPlayerCache(ulong userId, bool crossUser)
		{
			if (m_updatingPrivileges.IsSet)
			{
				m_onPrivilegeUpdateDone = (Action)Delegate.Combine(m_onPrivilegeUpdateDone, new Action(UpdateCache));
			}
			else
			{
				UpdateCache();
			}
			void UpdateCache()
			{
				IsTextChatAvailableForUserId(userId, crossUser);
				IsVoiceChatAvailableForUserId(userId, crossUser);
			}
		}

		public int GetChatMaxMessageSize()
		{
			return m_maxMessageSize;
		}

		public virtual MyPlayerChatState GetPlayerChatState(ulong playerId)
		{
			if (!m_mutedPlayers.Contains(playerId))
			{
				return MyPlayerChatState.Silent;
			}
			return MyPlayerChatState.Muted;
		}

		public virtual void SetPlayerMuted(ulong playerId, bool muted)
		{
			if (muted)
			{
				m_mutedPlayers.Add(playerId);
			}
			else
			{
				m_mutedPlayers.Remove(playerId);
			}
		}
	}
}
