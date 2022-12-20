using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace VRage.EOS
{
	internal class MyEOSGameServer : IMyGameServer
	{
		private struct MyAttributeKey : IEquatable<MyAttributeKey>
		{
			public bool Member;

			public string Key;

			public MyAttributeKey(bool member, string key)
			{
				Member = member;
				Key = key;
			}

			public bool Equals(MyAttributeKey other)
			{
				if (Member == other.Member)
				{
					return Key == other.Key;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				object obj2;
				if ((obj2 = obj) is MyAttributeKey)
				{
					MyAttributeKey other = (MyAttributeKey)obj2;
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (Member.GetHashCode() * 397) ^ ((Key != null) ? Key.GetHashCode() : 0);
			}

			public static bool operator ==(MyAttributeKey left, MyAttributeKey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(MyAttributeKey left, MyAttributeKey right)
			{
				return !left.Equals(right);
			}
		}

		public const string KEY_GAME_DESCRIPTION = "GAME_DESCRIPTION";

		public const string KEY_PRODUCT_NAME = "PRODUCT_NAME";

		public const string KEY_TAGS = "TAGS";

		public const string KEY_GAME_DATA = "GAME_DATA";

		public const string KEY_MAP_NAME = "MAP_NAME";

		public const string KEY_SERVER_NAME = "SERVER_NAME";

		public const string KEY_SERVER_STRINGS = "SERVER_STRINGS";

		public const string KEY_PLAYER_COUNT = "PLAYER_COUNT";

		public const string KEY_MOD_DIR = "MOD_DIR";

		public const string KEY_VERSION = "VERSION";

		public const string KEY_RULE_PREFIX = "RULE_";

		public const string KEY_IS_PASSWORD_PROTECTED = "IS_PASSWORD_PROTECTED";

		public const string KEY_OWNER_SERVICE_ID = "OWNER_SERVICE_ID";

		public const string KEY_MEMBER_SERVICE_ID = "MEMBER_SERVICE_ID";

		public const string KEY_MEMBER_SERVICE_KIND = "MEMBER_SERVICE_KIND";

		public const string KEY_OWNER_EOS_ID = "OWNER_EOS_ID";

		private readonly MyEOSNetworking m_networking;

		private readonly LobbyInterface m_lobby;

		private string m_lobbyId;

		private string m_productName = string.Empty;

		private string m_version;

		private bool m_userChangeFailed;

		private uint m_playerCount;

		private string m_gameDescription = string.Empty;

		private LobbyModification m_lobbyModificationHandle;

		private HashSet<MyAttributeKey> m_attributeChangeQueue = new HashSet<MyAttributeKey>();

		private HashSet<MyAttributeKey> m_attributeChangeQueueSwap = new HashSet<MyAttributeKey>();

		private readonly Dictionary<MyAttributeKey, AttributeDataValue> m_attributes = new Dictionary<MyAttributeKey, AttributeDataValue>();

		private readonly object m_attributeLock;

		private bool m_modificationPending;

		private uint m_maxPlayers;

		private uint m_maxPlayersReq;

		private ProductUserId LocalUserId => m_networking.Users.ProductUserId;

		public string GameDescription
		{
			get
			{
				return m_gameDescription;
			}
			set
			{
				m_gameDescription = value;
				SetAttribute("GAME_DESCRIPTION", value);
			}
		}

		public ulong ServerId { get; private set; }

		public bool Running { get; private set; }

		public event Action PlatformConnected;

		public event Action<string> PlatformDisconnected;

		public event Action<string> PlatformConnectionFailed;

		public event Action<ulong, JoinResult, ulong, string> ValidateAuthTicketResponse;

		public event Action<ulong, ulong, bool, bool> UserGroupStatusResponse;

		public event Action<sbyte> PolicyResponse;

		public MyEOSGameServer(MyEOSNetworking networking)
		{
			m_networking = networking;
			m_attributeLock = m_attributes;
			m_lobby = m_networking.Platform.GetLobbyInterface();
			m_networking.Users.OnUserChanged += OnUserChanged;
			MyServiceManager.Instance.AddService((IMyNetworking)m_networking);
			m_lobby.AddNotifyLobbyMemberStatusReceived(new AddNotifyLobbyMemberStatusReceivedOptions(), null, OnLobbyMemberStatusReceived);
			SetAttribute("PRODUCT_NAME", m_networking.ProductName);
			SetAttribute("PLAYER_COUNT", 0L);
		}

		public bool IsLobbyMember(ProductUserId id)
		{
			if (m_lobby == null)
			{
				return false;
			}
			CopyLobbyDetailsHandleOptions options = new CopyLobbyDetailsHandleOptions
			{
				LobbyId = m_lobbyId,
				LocalUserId = m_networking.Users.ProductUserId
			};
			if (m_lobby.CopyLobbyDetailsHandle(options, out var outLobbyDetailsHandle) != 0)
			{
				return false;
			}
			for (uint num = 0u; num < outLobbyDetailsHandle.GetMemberCount(new LobbyDetailsGetMemberCountOptions()); num++)
			{
				ProductUserId memberByIndex = outLobbyDetailsHandle.GetMemberByIndex(new LobbyDetailsGetMemberByIndexOptions
				{
					MemberIndex = num
				});
				if (id == memberByIndex)
				{
					outLobbyDetailsHandle.Release();
					return true;
				}
			}
			outLobbyDetailsHandle.Release();
			return false;
		}

		private void OnUserChanged()
		{
			if (!LocalUserId.IsValid())
			{
				if (ServerId != 0L)
				{
					m_lobbyModificationHandle?.Release();
					DestroyLobbyInternal(m_lobbyId);
					m_networking.InvokeOnMainThread(delegate
					{
						this.PlatformDisconnected.InvokeIfNotNull("");
					});
				}
				m_userChangeFailed = false;
			}
			else
			{
				ServerId = LocalUserId.ToUlong();
				SetAttribute("OWNER_SERVICE_ID", ServerId.ToString());
				SetAttribute("OWNER_EOS_ID", LocalUserId.GetIdString());
				if (!string.IsNullOrEmpty(m_lobbyId))
				{
					DestroyLobbyInternal(m_lobbyId);
					StartLobbyInternal();
				}
			}
		}

		public bool Start(IPEndPoint serverEndpoint, ushort steamPort, string versionString)
		{
			m_version = versionString;
			SetAttribute("VERSION", m_version);
			return true;
		}

		private void StartLobby()
		{
			m_networking.InvokeOnNetworkThread(StartLobbyInternal);
		}

		private void StartLobbyInternal()
		{
			CreateLobbyOptions options = new CreateLobbyOptions
			{
				LocalUserId = LocalUserId,
				MaxLobbyMembers = m_maxPlayersReq + 1,
				PermissionLevel = LobbyPermissionLevel.Publicadvertised,
				BucketId = m_version
			};
			m_lobby.CreateLobby(options, null, OnLobbyCreated);
		}

		private void OnLobbyCreated(CreateLobbyCallbackInfo data)
		{
			if (data.ResultCode == Result.Success)
			{
				m_lobbyId = data.LobbyId;
				lock (m_attributeLock)
				{
					m_attributeChangeQueue.UnionWith(m_attributes.Keys);
				}
				EnsureModification();
				FlushModification();
			}
			else
			{
				m_networking.Error($"Cannot start lobby: {data.ResultCode}, retrying in 5s.");
				m_networking.InvokeOnNetworkThread(StartLobbyInternal, TimeSpan.FromSeconds(5.0));
			}
		}

		private void OnLobbyMemberStatusReceived(LobbyMemberStatusReceivedCallbackInfo info)
		{
			if ((info.TargetUserId == LocalUserId && !info.CurrentStatus.IsConnected()) || (info.TargetUserId != LocalUserId && info.CurrentStatus == LobbyMemberStatus.Promoted))
			{
				bool num = m_lobbyId == info.LobbyId;
				m_networking.Error("Server disconnected from lobby. Recreating.");
				DestroyLobbyInternal(info.LobbyId);
				if (num)
				{
					StartLobbyInternal();
				}
				return;
			}
			if (m_lobby.CopyLobbyDetailsHandle(new CopyLobbyDetailsHandleOptions
			{
				LocalUserId = LocalUserId,
				LobbyId = m_lobbyId
			}, out var outLobbyDetailsHandle) == Result.Success)
			{
				uint memberCount = outLobbyDetailsHandle.GetMemberCount(new LobbyDetailsGetMemberCountOptions());
				if (memberCount != m_playerCount)
				{
					m_playerCount = memberCount;
					SetAttribute("PLAYER_COUNT", memberCount - 1);
				}
			}
			outLobbyDetailsHandle.Release();
		}

		private void DestroyLobbyInternal(string lobbyId)
		{
			if (string.IsNullOrEmpty(lobbyId))
			{
				return;
			}
			DestroyLobbyOptions options = new DestroyLobbyOptions
			{
				LobbyId = lobbyId,
				LocalUserId = LocalUserId
			};
			m_lobby.DestroyLobby(options, null, delegate(DestroyLobbyCallbackInfo info)
			{
				if (info.ResultCode != 0)
				{
					m_networking.Log($"Could not destroy lobby: {info.ResultCode}. Leaving.");
					m_lobby.LeaveLobby(new LeaveLobbyOptions
					{
						LobbyId = lobbyId,
						LocalUserId = LocalUserId
					}, null, delegate
					{
					});
				}
			});
			if (lobbyId == m_lobbyId)
			{
				m_lobbyModificationHandle?.Release();
				m_lobbyModificationHandle = null;
				m_lobbyId = null;
			}
		}

		public void Update()
		{
			if (string.IsNullOrEmpty(m_lobbyId))
			{
				return;
			}
			lock (m_attributeLock)
			{
				if (m_attributeChangeQueue.Count > 0 || m_maxPlayers != m_maxPlayersReq)
				{
					EnsureModification();
				}
			}
			FlushModification();
		}

		public bool WaitStart(int timeOut)
		{
			int num = timeOut / 100 + 1;
			while (!LocalUserId.IsValid() && !m_userChangeFailed && num > 0)
			{
				m_networking.Update();
				Thread.Sleep(100);
				num--;
			}
			Running = LocalUserId.IsValid();
			if (Running)
			{
				this.PlatformConnected.InvokeIfNotNull();
			}
			else
			{
				this.PlatformConnectionFailed.InvokeIfNotNull((!m_networking.Users.Connected) ? "User connect failed." : "Unknown error.");
			}
			return Running;
		}

		public void SetServerModTemporaryDirectory()
		{
		}

		public void SetGameReady(bool state)
		{
			StartLobby();
		}

		public void ClearAllKeyValues()
		{
		}

		public void SetGameTags(string tags)
		{
			SetAttribute("TAGS", tags);
		}

		public void SetGameData(string data)
		{
			SetAttribute("GAME_DATA", data);
		}

		public void SetMapName(string mapName)
		{
			SetAttribute("MAP_NAME", mapName);
			string text = m_attributes.GetValueOrDefault(new MyAttributeKey(member: false, "SERVER_NAME"))?.AsUtf8?.ToLower() + " " + mapName.ToLower();
			SetAttribute("SERVER_STRINGS", text);
		}

		public void SetServerName(string serverName)
		{
			SetAttribute("SERVER_NAME", serverName);
			string text = serverName.ToLower() + " " + m_attributes.GetValueOrDefault(new MyAttributeKey(member: false, "MAP_NAME"));
			SetAttribute("SERVER_STRINGS", text);
		}

		public void SetModDir(string directory)
		{
			SetAttribute("MOD_DIR", directory);
		}

		public void SetMaxPlayerCount(int count)
		{
			if (count > 32)
			{
				count = 32;
				m_networking.Error("MaxPlayer count too high, reducing to " + 32);
			}
			lock (m_attributeLock)
			{
				m_maxPlayersReq = (uint)count;
			}
		}

		public void SetPasswordProtected(bool passwordProtected)
		{
			SetAttribute("IS_PASSWORD_PROTECTED", passwordProtected.ToString());
		}

		public void SetKeyValue(string key, string value)
		{
			SetAttribute("RULE_" + key, value);
		}

		private void EnsureModification()
		{
			if (m_lobbyModificationHandle == null)
			{
				UpdateLobbyModificationOptions options = new UpdateLobbyModificationOptions
				{
					LobbyId = m_lobbyId,
					LocalUserId = LocalUserId
				};
				m_lobby.UpdateLobbyModification(options, out m_lobbyModificationHandle);
			}
		}

		private void FlushModification()
		{
			if (!(m_lobbyModificationHandle != null))
			{
				return;
			}
			if (m_modificationPending)
			{
				return;
			}
			uint playerCountSet = m_maxPlayersReq;
			lock (m_attributeLock)
			{
				foreach (MyAttributeKey item in m_attributeChangeQueue)
				{
					AttributeDataValue value = m_attributes[item];
					if (item.Member)
					{
						AddMemberAttributeInternal(item.Key, value);
					}
					else
					{
						AddAttributeInternal(item.Key, value);
					}
				}
				MyUtils.Swap(ref m_attributeChangeQueue, ref m_attributeChangeQueueSwap);
				m_attributeChangeQueue.Clear();
				if (m_maxPlayers != playerCountSet)
				{
					m_lobbyModificationHandle.SetMaxMembers(new LobbyModificationSetMaxMembersOptions
					{
						MaxMembers = playerCountSet + 1
					});
				}
			}
			m_modificationPending = true;
			UpdateLobbyOptions options = new UpdateLobbyOptions
			{
				LobbyModificationHandle = m_lobbyModificationHandle
			};
			m_lobby.UpdateLobby(options, null, delegate(UpdateLobbyCallbackInfo info)
			{
				lock (m_attributeLock)
				{
					if (info.ResultCode == Result.Success)
					{
						m_maxPlayers = playerCountSet;
					}
					else
					{
						m_attributeChangeQueue.UnionWith(m_attributeChangeQueueSwap);
					}
					m_lobbyModificationHandle?.Release();
					m_lobbyModificationHandle = null;
					m_modificationPending = false;
				}
			});
		}

		private void SetAttribute(string key, AttributeDataValue value)
		{
			SetAttribute(key, value, member: false);
		}

		private void SetMemberAttribute(string key, AttributeDataValue value)
		{
			SetAttribute(key, value, member: true);
		}

		private void SetAttribute(string key, AttributeDataValue value, bool member)
		{
			lock (m_attributeLock)
			{
				MyAttributeKey myAttributeKey = new MyAttributeKey(member, key);
				m_attributes[myAttributeKey] = value;
				m_attributeChangeQueue.Add(myAttributeKey);
			}
		}

		private void AddAttributeInternal(string key, AttributeDataValue value)
		{
			m_lobbyModificationHandle.AddAttribute(new LobbyModificationAddAttributeOptions
			{
				Attribute = new AttributeData
				{
					Key = key,
					Value = value
				},
				Visibility = LobbyAttributeVisibility.Public
			});
		}

		private void AddMemberAttributeInternal(string key, AttributeDataValue value)
		{
			m_lobbyModificationHandle.AddMemberAttribute(new LobbyModificationAddMemberAttributeOptions
			{
				Attribute = new AttributeData
				{
					Key = key,
					Value = value
				},
				Visibility = LobbyAttributeVisibility.Public
			});
		}

		public AttributeDataValue GetMemberAttribute(ProductUserId memberId, string key)
		{
			m_lobby.CopyLobbyDetailsHandle(new CopyLobbyDetailsHandleOptions
			{
				LocalUserId = LocalUserId,
				LobbyId = m_lobbyId
			}, out var outLobbyDetailsHandle);
			Epic.OnlineServices.Lobby.Attribute outAttribute;
			Result num = outLobbyDetailsHandle.CopyMemberAttributeByKey(new LobbyDetailsCopyMemberAttributeByKeyOptions
			{
				AttrKey = key,
				TargetUserId = memberId
			}, out outAttribute);
			outLobbyDetailsHandle.Release();
			if (num != 0)
			{
				return null;
			}
			return outAttribute.Data.Value;
		}

		public void SendUserDisconnect(ulong userId)
		{
		}

		private void ShutdownInternal()
		{
			m_lobbyModificationHandle?.Release();
			DestroyLobbyInternal(m_lobbyId);
			m_networking.EOSPeer2Peer.DisconnectAll();
			ServerId = 0uL;
		}

		public void Shutdown()
		{
			m_networking.InvokeOnNetworkThread(ShutdownInternal);
		}

		public void SetDedicated(bool isDedicated)
		{
		}

		public void SetBotPlayerCount(int count)
		{
		}

		public void LogOnAnonymous()
		{
		}

		public void LogOff()
		{
		}

		public bool RequestGroupStatus(ulong userId, ulong groupId)
		{
			return false;
		}

		public uint GetPublicIP()
		{
			return 0u;
		}

		public bool BeginAuthSession(ulong userId, byte[] token, string serviceName)
		{
			m_networking.InvokeOnMainThread(delegate
			{
				this.ValidateAuthTicketResponse.InvokeIfNotNull(userId, JoinResult.OK, userId, serviceName);
			});
			return true;
		}

		public void EndAuthSession(ulong userId)
		{
		}

		public void EnableHeartbeats(bool enable)
		{
		}

		public void BrowserUpdateUserData(ulong userId, string playerName, int score)
		{
		}

		public bool UserHasLicenseForApp(ulong steamId, uint appId)
		{
			return true;
		}
	}
}
