using System;
using System.Net;
using VRage.Network;

namespace VRage.GameServices
{
	public class MyNullGameServer : IMyGameServer
	{
		public string GameDescription { get; set; }

		public ulong ServerId { get; }

		public bool Running { get; }

		public event Action PlatformConnected;

		public event Action<string> PlatformDisconnected;

		public event Action<string> PlatformConnectionFailed;

		public event Action<ulong, JoinResult, ulong, string> ValidateAuthTicketResponse;

		public event Action<ulong, ulong, bool, bool> UserGroupStatusResponse;

		public event Action<sbyte> PolicyResponse;

		public bool Start(IPEndPoint serverEndpoint, ushort steamPort, string versionString)
		{
			throw new NotImplementedException();
		}

		public uint GetPublicIP()
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			throw new NotImplementedException();
		}

		public void SetKeyValue(string key, string value)
		{
			throw new NotImplementedException();
		}

		public void ClearAllKeyValues()
		{
			throw new NotImplementedException();
		}

		public void SetGameTags(string tags)
		{
			throw new NotImplementedException();
		}

		public void SetGameData(string data)
		{
			throw new NotImplementedException();
		}

		public void SetModDir(string directory)
		{
			throw new NotImplementedException();
		}

		public void SetDedicated(bool isDedicated)
		{
			throw new NotImplementedException();
		}

		public void SetMapName(string mapName)
		{
			throw new NotImplementedException();
		}

		public void SetServerName(string serverName)
		{
			throw new NotImplementedException();
		}

		public void SetMaxPlayerCount(int count)
		{
			throw new NotImplementedException();
		}

		public void SetBotPlayerCount(int count)
		{
			throw new NotImplementedException();
		}

		public void SetPasswordProtected(bool passwdProtected)
		{
			throw new NotImplementedException();
		}

		public void LogOnAnonymous()
		{
			throw new NotImplementedException();
		}

		public void LogOff()
		{
			throw new NotImplementedException();
		}

		public void Shutdown()
		{
			throw new NotImplementedException();
		}

		public bool BeginAuthSession(ulong userId, byte[] token, string serviceName)
		{
			throw new NotImplementedException();
		}

		public void EndAuthSession(ulong userId)
		{
			throw new NotImplementedException();
		}

		public void SendUserDisconnect(ulong userId)
		{
			throw new NotImplementedException();
		}

		public void EnableHeartbeats(bool enable)
		{
			throw new NotImplementedException();
		}

		public void BrowserUpdateUserData(ulong userId, string playerName, int score)
		{
			throw new NotImplementedException();
		}

		public bool RequestGroupStatus(ulong userId, ulong groupId)
		{
			throw new NotImplementedException();
		}

		public bool UserHasLicenseForApp(ulong steamId, uint appId)
		{
			throw new NotImplementedException();
		}

		public bool WaitStart(int timeOut)
		{
			return false;
		}

		public void SetServerModTemporaryDirectory()
		{
		}

		public void SetGameReady(bool state)
		{
		}

		protected virtual void OnPlatformConnected()
		{
			this.PlatformConnected?.Invoke();
		}

		protected virtual void OnPlatformDisconnected(string obj)
		{
			this.PlatformDisconnected?.Invoke(obj);
		}

		protected virtual void OnPlatformConnectionFailed(string obj)
		{
			this.PlatformConnectionFailed?.Invoke(obj);
		}

		protected virtual void OnValidateAuthTicketResponse(ulong arg1, JoinResult arg2, ulong arg3, string arg4)
		{
			this.ValidateAuthTicketResponse?.Invoke(arg1, arg2, arg3, arg4);
		}

		protected virtual void OnUserGroupStatusResponse(ulong arg1, ulong arg2, bool arg3, bool arg4)
		{
			this.UserGroupStatusResponse?.Invoke(arg1, arg2, arg3, arg4);
		}

		protected virtual void OnPolicyResponse(sbyte obj)
		{
			this.PolicyResponse?.Invoke(obj);
		}
	}
}
