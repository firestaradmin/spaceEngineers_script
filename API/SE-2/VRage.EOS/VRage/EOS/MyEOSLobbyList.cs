using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using VRage.GameServices;

namespace VRage.EOS
{
	internal class MyEOSLobbyList<T> where T : class
	{
		private readonly MyEOSNetworking m_networking;

		private readonly string m_filename;

		private readonly int m_itemLimit;

		private readonly MyLobbySearch<T> m_search;

		private List<T> m_lobbyList;

		private List<string> m_lobbyIds;

		private bool m_lobbiesCached;

		private readonly MakeLobbyItem<T> m_makeItem;

		private readonly Action m_queryCompletion;

		private readonly Action<T> m_disposeItem;

		private readonly Queue<(string id, T item, bool add)> m_pendingOps = new Queue<(string, T, bool)>();

		private readonly object m_lock;

		private bool m_syncingCloud;

		private bool m_resyncCloud;

		private readonly object m_syncCloudLock = new object();

		public bool IsRequesting { get; private set; }

		public int ItemCount
		{
			get
			{
				lock (m_lock)
				{
					return m_lobbyList.Count;
				}
			}
		}

		public MyEOSLobbyList(MyEOSNetworking networking, string storageFilename, MakeLobbyItem<T> makeItem, Action onQueryComplete, Action<T> disposeItem, int maxItems = -1)
		{
			m_networking = networking;
			m_filename = storageFilename;
			m_makeItem = makeItem;
			m_search = new MyLobbySearch<T>(networking, makeItem);
			m_disposeItem = disposeItem;
			m_queryCompletion = onQueryComplete;
			m_lobbyList = new List<T>();
			m_lobbyIds = new List<string>();
			IsRequesting = false;
			m_lock = new object();
			m_itemLimit = maxItems;
		}

		private void SyncCloud()
		{
			lock (m_syncCloudLock)
			{
				if (m_syncingCloud)
				{
					m_resyncCloud = true;
					return;
				}
				m_syncingCloud = true;
			}
			m_networking.Service.SaveToCloudAsync(m_filename, SerializeServers(), SyncCloudComplete);
		}

		private void SyncCloudComplete(CloudResult result)
		{
			bool flag = false;
			lock (m_syncCloudLock)
			{
				m_syncingCloud = false;
				if (m_resyncCloud)
				{
					m_resyncCloud = false;
					flag = true;
				}
			}
			if (flag)
			{
				SyncCloud();
			}
		}

		private byte[] SerializeServers()
		{
			MySerializedServerList mySerializedServerList = new MySerializedServerList(m_lobbyIds);
			XmlSerializer orCreateSerializer = MyXmlSerializerManager.GetOrCreateSerializer(mySerializedServerList.GetType());
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					orCreateSerializer.Serialize(memoryStream, mySerializedServerList);
					return memoryStream.ToArray();
				}
			}
			catch (InvalidOperationException arg)
			{
				m_networking.Error($"Failed to serialize server list :\n{arg}");
				return Array.Empty<byte>();
			}
		}

		private void DeserializeAndLoadServers(byte[] data, MySessionSearchFilter filter)
		{
			List<string> idList;
			if (data == null)
			{
				idList = new List<string>();
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream(data);
				using (memoryStream)
				{
					MySerializedServerList mySerializedServerList = new MySerializedServerList();
					XmlSerializer orCreateSerializer = MyXmlSerializerManager.GetOrCreateSerializer(mySerializedServerList.GetType());
					try
					{
						mySerializedServerList = (MySerializedServerList)orCreateSerializer.Deserialize(memoryStream);
					}
					catch (InvalidOperationException arg)
					{
						string message = $"Failed to deserialize server list :\nException : {arg}";
						m_networking.Error(message);
						mySerializedServerList.Lobbies = null;
					}
					idList = mySerializedServerList.Lobbies;
				}
			}
			m_lobbiesCached = true;
			LoadServerListFromId(idList, filter);
		}

		public void AddServer(string connectionString)
		{
			m_networking.SearchForLobby(connectionString, delegate(LobbyDetails details, LobbyDetailsInfo info)
			{
				AddServer(connectionString, m_makeItem(details, info));
			});
		}

		public void AddServer(string connectionString, T item)
		{
			if (item == null)
			{
				return;
			}
			lock (m_lock)
			{
				if (IsRequesting)
				{
					m_pendingOps.Enqueue((connectionString, item, true));
					return;
				}
				Log("Add server [" + connectionString + "] to list");
				int num = m_lobbyIds.IndexOf(connectionString);
				if (num < 0)
				{
					m_lobbyList.Insert(0, item);
					m_lobbyIds.Insert(0, connectionString);
					if (m_itemLimit > 0 && m_lobbyList.Count > m_itemLimit)
					{
						num = m_lobbyIds.Count - 1;
						m_lobbyIds.RemoveAt(num);
						int num2 = m_lobbyList.IndexOf(item);
						if (num2 >= 0)
						{
							m_disposeItem?.Invoke(m_lobbyList[num2]);
							m_lobbyList.RemoveAt(num2);
						}
					}
				}
				else
				{
					m_lobbyIds.Swap(0, num);
					int num3 = m_lobbyList.IndexOf(item);
					if (num3 >= 0)
					{
						m_lobbyList.Swap(0, num3);
					}
				}
				SyncCloud();
			}
		}

		public void RemoveServer(string connectionString, T item)
		{
			lock (m_lock)
			{
				if (IsRequesting)
				{
					m_pendingOps.Enqueue((connectionString, item, false));
					return;
				}
				Log("Remove server [" + connectionString + "] from list");
				bool num = m_lobbyIds.Remove(connectionString);
				bool flag = m_lobbyList.Remove(item);
				m_disposeItem?.Invoke(item);
				if (num || flag)
				{
					SyncCloud();
				}
			}
		}

		public T Get(int index)
		{
			Log($"Fetch item for server #{index}.");
			lock (m_lock)
			{
				return m_lobbyList[index];
			}
		}

		public void Access(Action<IReadOnlyList<T>> resultListAccessor)
		{
			lock (m_lock)
			{
				resultListAccessor(m_lobbyList);
			}
		}

		public void QueryServerList(MySessionSearchFilter filter)
		{
			if (IsRequesting)
			{
				Log("Query server list skipped. Already fetching.");
				return;
			}
			Log("Querying server list...");
			IsRequesting = true;
			if (!m_lobbiesCached)
			{
				byte[] data = m_networking.Service.LoadFromCloud(m_filename);
				DeserializeAndLoadServers(data, filter);
			}
			else
			{
				LoadServerListFromId(m_lobbyIds, filter);
			}
		}

		private void LoadServerListFromId(List<string> idList, MySessionSearchFilter filter)
		{
			List<T> lobbyList = new List<T>();
			if (idList.Count == 0)
			{
				Deliver();
			}
			else
			{
				m_search.Search(idList.ToArray(), filter, OnResults);
			}
			void Deliver()
			{
				IsRequesting = false;
				lock (m_lock)
				{
					m_lobbyIds = idList;
					m_lobbyList = lobbyList;
					(string, T, bool) result2;
					while (m_pendingOps.TryDequeue<(string, T, bool)>(out result2))
					{
						if (result2.Item3)
						{
							AddServer(result2.Item1, result2.Item2);
						}
						else
						{
							RemoveServer(result2.Item1, result2.Item2);
						}
					}
				}
				Log($"Server list queried: {lobbyList.Count} result(s).");
				m_queryCompletion?.Invoke();
			}
			void OnResults(Result result, (string ConnectingString, T Item)[] servers)
			{
				if (result != 0)
				{
					m_networking.Error($"Lobby search was not successful: {result}");
				}
				for (int i = 0; i < servers.Length; i++)
				{
					if (m_networking.VerboseLogging)
					{
						Log("Server [" + servers[i].ConnectingString + "]: " + ((servers[i].Item == null) ? "Not Available" : "Loaded") + ".");
					}
					if (servers[i].Item != null)
					{
						lobbyList.Add(servers[i].Item);
					}
				}
				Deliver();
			}
		}

		public void Clear()
		{
			Log("Clear.");
			lock (m_lock)
			{
				if (m_disposeItem != null)
				{
					foreach (T lobby in m_lobbyList)
					{
						m_disposeItem(lobby);
					}
				}
				m_lobbyList.Clear();
				m_lobbyIds.Clear();
			}
			m_lobbiesCached = false;
		}

		private void Log(string message)
		{
			m_networking.Log("Server List " + Path.GetFileName(m_filename) + ": " + message);
		}
	}
}
