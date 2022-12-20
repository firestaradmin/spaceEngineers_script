using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using VRage.Network;
using VRage.Utils;

namespace VRage.EOS
{
	internal class MyEOSUser
	{
		private readonly MyEOSNetworking m_networking;

		private ulong m_lastServiceUserId;

		private readonly bool m_isDedicated;

		private readonly ConnectInterface m_connect;

		private readonly AuthInterface m_auth;

		private EpicAccountId m_epicAccountId;

		public ProductUserId ProductUserId { get; private set; }

		public bool Connected => ProductUserId.IsValid();

		public event Action OnUserChanged;

		public MyEOSUser(MyEOSNetworking networking, bool isDedicated)
		{
			m_isDedicated = isDedicated;
			m_networking = networking;
			m_connect = m_networking.Platform.GetConnectInterface();
			m_connect.AddNotifyLoginStatusChanged(new Epic.OnlineServices.Connect.AddNotifyLoginStatusChangedOptions(), null, OnConnectChanged);
			m_connect.AddNotifyAuthExpiration(new AddNotifyAuthExpirationOptions(), null, OnConnectExpires);
			m_auth = m_networking.Platform.GetAuthInterface();
			ProductUserId = new ProductUserId(IntPtr.Zero);
			MyVRage.Platform.System.OnResuming += OnResuming;
			m_networking.Service.OnUserChanged += ServiceOnUserChanged;
			if (m_networking.Service.UserId != 0 || isDedicated)
			{
				Connect();
			}
		}

		private void OnResuming()
		{
			m_networking.Log("EOS OnResuming: " + ProductUserId.GetIdString());
			m_networking.InvokeOnNetworkThread(Connect);
		}

		private void ServiceOnUserChanged(bool differentUserLoggedIn)
		{
			if (m_networking.Service.UserId != m_lastServiceUserId)
			{
				m_networking.Log("ServiceOnUserChanged: " + EndpointId.Format(m_lastServiceUserId) + " -> " + EndpointId.Format(m_networking.Service.UserId));
				LogOut();
				m_lastServiceUserId = m_networking.Service.UserId;
				if (m_networking.Service.UserId != 0L)
				{
					Connect();
				}
			}
		}

		private void LogOut()
		{
			if (ProductUserId.IsValid())
			{
				Disconnect();
			}
		}

		private string GetDeviceModel()
		{
			uint frequency;
			uint physicalCores;
			return $"{MyVRage.Platform.System.GetInfoCPU(out frequency, out physicalCores)}, {(int)((float)MyVRage.Platform.System.GetTotalPhysicalMemory() / 1024f / 1024f / 1024f + 0.5f)}G RAM";
		}

		private void Connect()
		{
			if (m_isDedicated)
			{
				if (Environment.CommandLine.Contains("-refresh-credentials"))
				{
					MyLog.Default.WriteLineAndConsole("Refreshing device id.");
					m_connect.DeleteDeviceId(new DeleteDeviceIdOptions(), null, delegate
					{
						CreateId();
					});
				}
				else
				{
					CreateId();
				}
				return;
			}
			m_networking.Service.RequestEncryptedAppTicket(m_networking.EncryptionUrl, delegate(bool success, string token)
			{
				m_networking.InvokeOnNetworkThread(delegate
				{
					OnRequestEncryptedAppTicket(success, token);
				});
			});
			void CreateId()
			{
				CreateDeviceIdOptions options = new CreateDeviceIdOptions
				{
					DeviceModel = GetDeviceModel()
				};
				m_connect.CreateDeviceId(options, null, OnDeviceIdCreated);
			}
		}

		private void OnAuthCompleted(Epic.OnlineServices.Auth.LoginCallbackInfo data)
		{
			if (data.ResultCode == Result.Success)
			{
				m_epicAccountId = data.LocalUserId;
				RefreshEpicToken();
			}
			else
			{
				Connect();
			}
		}

		private void RefreshEpicToken()
		{
			m_auth.GetLoginStatus(m_epicAccountId);
			if (m_auth.CopyUserAuthToken(new CopyUserAuthTokenOptions(), m_epicAccountId, out var outUserAuthToken) == Result.Success)
			{
				Login(ExternalCredentialType.Epic, outUserAuthToken.AccessToken);
				return;
			}
			m_epicAccountId = null;
			Connect();
		}

		private void OnRequestEncryptedAppTicket(bool success, string token)
		{
			if (success)
			{
				ExternalCredentialType type;
				switch (m_networking.Service.GetServiceKind())
				{
				default:
					return;
				case ExternalAccountType.Steam:
					type = ExternalCredentialType.SteamAppTicket;
					token = BitConverter.ToString(Convert.FromBase64String(token)).Replace("-", "");
					break;
				case ExternalAccountType.Xbl:
					type = ExternalCredentialType.XblXstsToken;
					break;
				case ExternalAccountType.Epic:
					type = ExternalCredentialType.Epic;
					break;
				case ExternalAccountType.Psn:
					return;
				}
				Login(type, token);
			}
			else
			{
				m_networking.InvokeOnNetworkThread(Connect, TimeSpan.FromSeconds(30.0));
			}
		}

		private void Login(ExternalCredentialType type, string token)
		{
			Epic.OnlineServices.Connect.LoginOptions options = new Epic.OnlineServices.Connect.LoginOptions
			{
				Credentials = new Epic.OnlineServices.Connect.Credentials
				{
					Type = type,
					Token = token
				},
				UserLoginInfo = null
			};
			m_connect.Login(options, null, OnConnected);
		}

		private void OnDeviceIdCreated(CreateDeviceIdCallbackInfo info)
		{
			if (info.ResultCode == Result.Success || info.ResultCode == Result.DuplicateNotAllowed)
			{
				Epic.OnlineServices.Connect.LoginOptions options = new Epic.OnlineServices.Connect.LoginOptions
				{
					Credentials = new Epic.OnlineServices.Connect.Credentials
					{
						Type = ExternalCredentialType.DeviceidAccessToken
					},
					UserLoginInfo = new UserLoginInfo
					{
						DisplayName = m_networking.ProductName
					}
				};
				m_connect.Login(options, null, OnConnected);
			}
			else
			{
				m_networking.Error("OnDeviceIdCreated failed: " + info.ResultCode);
				this.OnUserChanged.InvokeIfNotNull();
			}
		}

		private void Disconnect()
		{
			ProductUserId = new ProductUserId(IntPtr.Zero);
		}

		private void OnConnected(Epic.OnlineServices.Connect.LoginCallbackInfo callbackInfo)
		{
			if (!(ProductUserId == callbackInfo.LocalUserId))
			{
				switch (callbackInfo.ResultCode)
				{
				case Result.InvalidUser:
				{
					CreateUserOptions options = new CreateUserOptions
					{
						ContinuanceToken = callbackInfo.ContinuanceToken
					};
					m_connect.CreateUser(options, null, OnUserCreated);
					break;
				}
				case Result.Success:
					ProductUserId = callbackInfo.LocalUserId;
					this.OnUserChanged.InvokeIfNotNull();
					break;
				default:
					m_networking.Error("Failed to connect the user: " + callbackInfo.ResultCode);
					this.OnUserChanged.InvokeIfNotNull();
					break;
				}
			}
		}

		private void OnUserCreated(CreateUserCallbackInfo callbackInfo)
		{
			if (callbackInfo.ResultCode == Result.Success)
			{
				ProductUserId = callbackInfo.LocalUserId;
				this.OnUserChanged.InvokeIfNotNull();
			}
			else
			{
				this.OnUserChanged.InvokeIfNotNull();
			}
		}

		private void OnConnectChanged(Epic.OnlineServices.Connect.LoginStatusChangedCallbackInfo data)
		{
			_ = data.CurrentStatus;
		}

		private void OnConnectExpires(AuthExpirationCallbackInfo data)
		{
			m_networking.Log("OnConnectExpires: " + data.LocalUserId.GetIdString());
			Connect();
		}
	}
}
