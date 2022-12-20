using System;
using System.Collections.Generic;
using Sandbox.Game.World;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
	public class MyClientCollection
	{
		private readonly Dictionary<ulong, MyNetworkClient> m_clients = new Dictionary<ulong, MyNetworkClient>();

		private HashSet<ulong> m_disconnectedClients = new HashSet<ulong>();

		private ulong m_localSteamId;

		public Action<ulong> ClientAdded;

		private object m_clientRemovedLock = new object();

		private Action<ulong> m_clientRemoved;

		public int Count => m_clients.Count;

		public MyNetworkClient LocalClient
		{
			get
			{
				MyNetworkClient value = null;
				m_clients.TryGetValue(m_localSteamId, out value);
				return value;
			}
		}

		public event Action<ulong> ClientRemoved
		{
			add
			{
				lock (m_clientRemovedLock)
				{
					m_clientRemoved = (Action<ulong>)Delegate.Combine(m_clientRemoved, value);
				}
			}
			remove
			{
				lock (m_clientRemovedLock)
				{
					m_clientRemoved = (Action<ulong>)Delegate.Remove(m_clientRemoved, value);
				}
			}
		}

		public void SetLocalSteamId(ulong localSteamId, bool createLocalClient, string userName)
		{
			m_localSteamId = localSteamId;
			if (createLocalClient && !m_clients.ContainsKey(m_localSteamId))
			{
				AddClient(m_localSteamId, userName);
			}
		}

		public void Clear()
		{
			m_clients.Clear();
			m_disconnectedClients.Clear();
		}

		public bool TryGetClient(ulong steamId, out MyNetworkClient client)
		{
			client = null;
			return m_clients.TryGetValue(steamId, out client);
		}

		public bool HasClient(ulong steamId)
		{
			return m_clients.ContainsKey(steamId);
		}

		public MyNetworkClient AddClient(ulong steamId, string senderName)
		{
			if (m_clients.ContainsKey(steamId))
			{
				MyLog.Default.WriteLine("ERROR: Added client already present: " + m_clients[steamId].DisplayName);
				return m_clients[steamId];
			}
			MyNetworkClient myNetworkClient = new MyNetworkClient(steamId, senderName);
			m_clients.Add(steamId, myNetworkClient);
			m_disconnectedClients.Remove(steamId);
			RaiseClientAdded(steamId);
			return myNetworkClient;
		}

		public void RemoveClient(ulong steamId)
		{
			m_clients.TryGetValue(steamId, out var value);
			if (value == null)
			{
				if (!m_disconnectedClients.Contains(steamId))
				{
					MyLog.Default.WriteLine("ERROR: Removed client not present: " + EndpointId.Format(steamId));
				}
			}
			else
			{
				m_clients.Remove(steamId);
				m_disconnectedClients.Add(steamId);
				RaiseClientRemoved(steamId);
			}
		}

		private void RaiseClientAdded(ulong steamId)
		{
			ClientAdded.InvokeIfNotNull(steamId);
		}

		private void RaiseClientRemoved(ulong steamId)
		{
			m_clientRemoved.InvokeIfNotNull(steamId);
		}

		public Dictionary<ulong, MyNetworkClient>.ValueCollection GetClients()
		{
			return m_clients.Values;
		}
	}
}
