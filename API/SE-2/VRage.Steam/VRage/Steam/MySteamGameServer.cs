using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Steamworks;
using VRage.Collections;
using VRage.FileSystem;
using VRage.GameServices;
using VRage.Network;
using VRage.Steam.Steamworks;
using VRage.Utils;

namespace VRage.Steam
{
	internal class MySteamGameServer : IMyGameServer, IDisposable
	{
		private string m_serverName;

		private Callback<ValidateAuthTicketResponse_t> m_validateAuthTickedResponse;

		private Callback<SteamServersConnected_t> m_serversConnected;

		private Callback<SteamServersDisconnected_t> m_serversDisconnected;

		private Callback<SteamServerConnectFailure_t> m_serversConnectFailure;

		private Callback<GSPolicyResponse_t> m_policyResponse;

		private Callback<GSClientGroupStatus_t> m_clientGroupStatus;

		private MySteamNetworking m_networking;

		private string m_gameDescription = string.Empty;

		private string m_productName = string.Empty;

		private Socket m_socket;

		private Thread m_serverThread;

		private readonly MyConcurrentQueue<Action> m_invokeQueue = new MyConcurrentQueue<Action>(32);

		public string GameDescription
		{
			get
			{
				return m_gameDescription;
			}
			set
			{
				m_gameDescription = value;
				SteamGameServer.SetGameDescription(value);
			}
		}

		public ulong ServerId => (ulong)SteamGameServer.GetSteamID();

		public bool Running { get; private set; }

		public event Action PlatformConnected;

		public event Action<string> PlatformDisconnected;

		public event Action<string> PlatformConnectionFailed;

		public event Action<ulong, JoinResult, ulong, string> ValidateAuthTicketResponse;

		public event Action<ulong, ulong, bool, bool> UserGroupStatusResponse;

		public event Action<sbyte> PolicyResponse;

		public MySteamGameServer(MySteamNetworking networkingService)
		{
			m_networking = networkingService;
			MyServiceManager.Instance.AddService((IMyNetworking)networkingService);
		}

		public bool Start(IPEndPoint serverEndpoint, ushort steamPort, string versionString)
		{
			uint unIP = serverEndpoint.Address.ToIPv4NetworkOrder();
			ushort usGamePort = (ushort)serverEndpoint.Port;
			if (!GameServer.Init(unIP, steamPort, usGamePort, ushort.MaxValue, EServerMode.eServerModeAuthenticationAndSecure, versionString))
			{
				MyLog.Default.WriteLineAndConsole("Error starting Steam dedicated server");
				return false;
			}
			m_validateAuthTickedResponse = Callback<ValidateAuthTicketResponse_t>.CreateGameServer(NotifyValidateAuthTicketResponse);
			m_serversConnected = Callback<SteamServersConnected_t>.CreateGameServer(NotifyPlatformConnected);
			m_serversDisconnected = Callback<SteamServersDisconnected_t>.CreateGameServer(NotifyPlatformDisconnected);
			m_serversConnectFailure = Callback<SteamServerConnectFailure_t>.CreateGameServer(NotifyPlatformConnectionFailed);
			m_policyResponse = Callback<GSPolicyResponse_t>.CreateGameServer(NotifyPolicyResponse);
			m_clientGroupStatus = Callback<GSClientGroupStatus_t>.CreateGameServer(NotifyUserGroupStatusResponse);
			StartServerListener(serverEndpoint);
			if (!Running)
			{
				return false;
			}
			SteamGameServer.SetProduct(m_networking.ProductName);
			SteamGameServer.SetServerName(m_serverName);
			if (!MySteamUgcGameServer.Instance.BInitWorkshopForGameServer((DepotId_t)MySteamService.Static.AppId, Path.Combine(MyFileSystem.ContentPath, "Workshop")))
			{
				MyLog.Default.WriteLineAndConsole("Error initializing workshop support.");
				return false;
			}
			IMyInventoryService serviceInstance = new MySteamInventoryServer();
			MyServiceManager.Instance.AddService(serviceInstance);
			MySteamUgc.Instance = MySteamUgcGameServer.Instance;
			return true;
		}

		private void StartServerListener(IPEndPoint endpoint)
		{
			m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			try
			{
				m_socket.Bind(endpoint);
			}
			catch (SocketException ex)
			{
				MyLog.Default.WriteLineAndConsole($"Error binding server endpoint: {ex.Message}");
				Shutdown();
				m_socket = null;
				return;
			}
			Running = true;
			m_serverThread = new Thread(SteamServerEntryPoint)
			{
				IsBackground = true
			};
			m_serverThread.Start(m_socket);
		}

		private unsafe void SteamServerEntryPoint(object argument)
		{
			Socket socket = (Socket)argument;
			EndPoint localEndPoint = socket.LocalEndPoint;
			byte[] array = new byte[1500];
			while (Running)
			{
				EndPoint remoteEP = new IPEndPoint(0L, 0);
				int num = 0;
				try
				{
					num = socket.ReceiveFrom(array, ref remoteEP);
					if (!Running)
					{
						return;
					}
				}
				catch (SocketException ex)
				{
					if (!Running)
					{
						return;
					}
					try
					{
						socket.Close();
					}
					catch
					{
					}
					MyLog.Default.WriteLineAndConsole($"Received socket exception with error code: {ex.ErrorCode}, {ex.SocketErrorCode}");
					MyLog.Default.WriteLineAndConsole("Attempting to create new socket.");
					socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					try
					{
						socket.Bind(localEndPoint);
					}
					catch (SocketException ex2)
					{
						MyLog.Default.WriteLineAndConsole($"Error binding server endpoint: {ex2.Message}");
						m_socket.Close();
						Running = false;
						GameServer.Shutdown();
						return;
					}
					continue;
				}
				if (num == 0)
				{
					continue;
				}
				IPEndPoint iPEndPoint = (IPEndPoint)remoteEP;
				fixed (byte* ptr = array)
				{
					if (*(uint*)ptr == uint.MaxValue)
					{
						SteamGameServer.HandleIncomingPacket(array, num, (uint)iPEndPoint.Address.Address, (ushort)iPEndPoint.Port);
						uint pNetAdr;
						ushort pPort;
						while ((num = SteamGameServer.GetNextOutgoingPacket(array, array.Length, out pNetAdr, out pPort)) > 0)
						{
							iPEndPoint.Address.Address = pNetAdr;
							iPEndPoint.Port = pPort;
							socket.SendTo(array, num, SocketFlags.None, iPEndPoint);
						}
					}
				}
			}
		}

		private void NotifyValidateAuthTicketResponse(ValidateAuthTicketResponse_t response)
		{
			if (this.ValidateAuthTicketResponse != null)
			{
				JoinResult joinResult = JoinResult.TicketInvalid;
				switch (response.m_eAuthSessionResponse)
				{
				case EAuthSessionResponse.k_EAuthSessionResponseOK:
					joinResult = JoinResult.OK;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseUserNotConnectedToSteam:
					joinResult = JoinResult.UserNotConnected;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseNoLicenseOrExpired:
					joinResult = JoinResult.NoLicenseOrExpired;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseVACBanned:
					joinResult = JoinResult.VACBanned;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseLoggedInElseWhere:
					joinResult = JoinResult.LoggedInElseWhere;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseVACCheckTimedOut:
					joinResult = JoinResult.VACCheckTimedOut;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseAuthTicketCanceled:
					joinResult = JoinResult.TicketCanceled;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed:
					joinResult = JoinResult.TicketAlreadyUsed;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalid:
					joinResult = JoinResult.TicketInvalid;
					break;
				case EAuthSessionResponse.k_EAuthSessionResponsePublisherIssuedBan:
					joinResult = JoinResult.BannedByAdmins;
					break;
				default:
					joinResult = JoinResult.TicketInvalid;
					break;
				}
				m_invokeQueue.Enqueue(delegate
				{
					this.ValidateAuthTicketResponse((ulong)response.m_SteamID, joinResult, (ulong)response.m_OwnerSteamID, m_networking.ServiceName);
				});
			}
		}

		private void NotifyUserGroupStatusResponse(GSClientGroupStatus_t result)
		{
			if (this.UserGroupStatusResponse != null)
			{
				m_invokeQueue.Enqueue(delegate
				{
					this.UserGroupStatusResponse((ulong)result.m_SteamIDUser, (ulong)result.m_SteamIDGroup, result.m_bMember, result.m_bOfficer);
				});
			}
		}

		private void NotifyPolicyResponse(GSPolicyResponse_t result)
		{
			if (this.PolicyResponse != null)
			{
				m_invokeQueue.Enqueue(delegate
				{
					this.PolicyResponse((sbyte)result.m_bSecure);
				});
			}
		}

		private void NotifyPlatformConnectionFailed(SteamServerConnectFailure_t result)
		{
			if (this.PlatformConnectionFailed != null)
			{
				m_invokeQueue.Enqueue(delegate
				{
					Action<string> platformConnectionFailed = this.PlatformConnectionFailed;
					MyGameServiceCallResult eResult = (MyGameServiceCallResult)result.m_eResult;
					platformConnectionFailed(eResult.ToString());
				});
			}
		}

		private void NotifyPlatformDisconnected(SteamServersDisconnected_t result)
		{
			if (this.PlatformDisconnected != null)
			{
				m_invokeQueue.Enqueue(delegate
				{
					Action<string> platformDisconnected = this.PlatformDisconnected;
					MyGameServiceCallResult eResult = (MyGameServiceCallResult)result.m_eResult;
					platformDisconnected(eResult.ToString());
				});
			}
		}

		private void NotifyPlatformConnected(SteamServersConnected_t result)
		{
			if (this.PlatformConnected != null)
			{
				m_invokeQueue.Enqueue(this.PlatformConnected);
			}
		}

		public uint GetPublicIP()
		{
			return SteamGameServer.GetPublicIP().ToIPAddress().ToIPv4NetworkOrder();
		}

		public void Update()
		{
			if (Running)
			{
				GameServer.RunCallbacks();
			}
		}

		public void RunCallbacks()
		{
			Action instance;
			while (m_invokeQueue.TryDequeue(out instance))
			{
				instance();
			}
		}

		public void SetKeyValue(string key, string value)
		{
			SteamGameServer.SetKeyValue(key, value);
		}

		public void ClearAllKeyValues()
		{
			SteamGameServer.ClearAllKeyValues();
		}

		public void SetGameTags(string tags)
		{
			SteamGameServer.SetGameTags(tags);
		}

		public void SetGameData(string data)
		{
			SteamGameServer.SetGameData(data);
		}

		public void SetModDir(string directory)
		{
			SteamGameServer.SetModDir(directory);
		}

		public void SetDedicated(bool isDedicated)
		{
			SteamGameServer.SetDedicatedServer(isDedicated);
		}

		public void SetMapName(string mapName)
		{
			SteamGameServer.SetMapName(mapName);
		}

		public void SetServerName(string serverName)
		{
			if (Running)
			{
				SteamGameServer.SetServerName(serverName);
			}
			m_serverName = serverName;
		}

		public void SetMaxPlayerCount(int count)
		{
			SteamGameServer.SetMaxPlayerCount(count);
		}

		public void SetBotPlayerCount(int count)
		{
			SteamGameServer.SetBotPlayerCount(count);
		}

		public void SetPasswordProtected(bool passwdProtected)
		{
			SteamGameServer.SetPasswordProtected(passwdProtected);
		}

		public void LogOnAnonymous()
		{
			SteamGameServer.LogOnAnonymous();
		}

		public void LogOff()
		{
			SteamGameServer.LogOff();
		}

		public void Shutdown()
		{
			if (Running)
			{
				m_socket.Close();
				Running = false;
				m_serverThread.Join();
				GameServer.Shutdown();
			}
		}

		public bool BeginAuthSession(ulong userId, byte[] token, string msgServiceName)
		{
			return SteamGameServer.BeginAuthSession(token, token.Length, (CSteamID)userId) == EBeginAuthSessionResult.k_EBeginAuthSessionResultOK;
		}

		public void EndAuthSession(ulong userId)
		{
			SteamGameServer.EndAuthSession((CSteamID)userId);
		}

		public void SendUserDisconnect(ulong userId)
		{
			SteamGameServer.SendUserDisconnect((CSteamID)userId);
		}

		public void EnableHeartbeats(bool enable)
		{
			SteamGameServer.EnableHeartbeats(enable);
		}

		public void BrowserUpdateUserData(ulong userId, string playerName, int score)
		{
			SteamGameServer.BUpdateUserData((CSteamID)userId, playerName, (uint)score);
		}

		public bool RequestGroupStatus(ulong userId, ulong groupId)
		{
			return SteamGameServer.RequestUserGroupStatus((CSteamID)userId, (CSteamID)groupId);
		}

		public bool UserHasLicenseForApp(ulong steamId, uint appId)
		{
			return SteamGameServer.UserHasLicenseForApp((CSteamID)steamId, (AppId_t)appId) == EUserHasLicenseForAppResult.k_EUserHasLicenseResultHasLicense;
		}

		public bool WaitStart(int timeOut)
		{
			uint num = 0u;
			int num2 = timeOut / 100 + 1;
			while (num == 0 && num2 > 0)
			{
				Update();
				Thread.Sleep(100);
				num2--;
				num = GetPublicIP();
				_ = ServerId;
			}
			return num != 0;
		}

		public void Dispose()
		{
			m_validateAuthTickedResponse.Dispose();
			m_clientGroupStatus.Dispose();
			m_policyResponse.Dispose();
			m_serversConnected.Dispose();
			m_serversDisconnected.Dispose();
			m_serversConnectFailure.Dispose();
		}

		public void SetServerModTemporaryDirectory()
		{
			SteamGameServerUGC.BInitWorkshopForGameServer((DepotId_t)MySteamService.Static.AppId, MyFileSystem.UserDataPath);
		}

		public void SetGameReady(bool state)
		{
		}
	}
}
