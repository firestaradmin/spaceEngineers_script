using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;

namespace Sandbox.Game.SessionComponents
{
	/// <summary>
	/// Session component taking care of DLC checking.
	/// </summary>
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 2000, typeof(MyObjectBuilder_MySessionComponentDLC), null, false)]
<<<<<<< HEAD
	public class MySessionComponentDLC : MySessionComponentBase, IMyDLCs
=======
	public class MySessionComponentDLC : MySessionComponentBase
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		protected sealed class RequestUpdateClientDLC_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestUpdateClientDLC();
			}
		}

		private HashSet<uint> m_availableDLCs;

		private Dictionary<ulong, HashSet<uint>> m_clientAvailableDLCs;

		private Dictionary<MyDLCs.MyDLC, int> m_usedUnownedDLCs;

		public DictionaryReader<MyDLCs.MyDLC, int> UsedUnownedDLCs => m_usedUnownedDLCs;

		public event Action<ulong, uint> DLCInstalled;

		event Action<ulong, uint> IMyDLCs.DLCInstalled
		{
			add
			{
				DLCInstalled += value;
			}
			remove
			{
				DLCInstalled -= value;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_availableDLCs = new HashSet<uint>();
			m_usedUnownedDLCs = new Dictionary<MyDLCs.MyDLC, int>();
			if (!Sync.IsDedicated)
			{
				UpdateLocalPlayerDLC();
				MyGameService.OnDLCInstalled += OnDLCInstalled;
			}
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				m_clientAvailableDLCs = new Dictionary<ulong, HashSet<uint>>();
				MyMultiplayer.Static.ClientJoined += UpdateClientDLC;
			}
		}

		public List<ulong> GetAvailableClientDLCsIds()
		{
<<<<<<< HEAD
			if (m_availableDLCs != null)
			{
				List<ulong> list = new List<ulong>();
				{
					foreach (uint availableDLC in m_availableDLCs)
					{
						list.Add(availableDLC);
					}
					return list;
				}
=======
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_availableDLCs != null)
			{
				List<ulong> list = new List<ulong>();
				Enumerator<uint> enumerator = m_availableDLCs.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						uint current = enumerator.get_Current();
						list.Add(current);
					}
					return list;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return new List<ulong>();
		}

		protected override void UnloadData()
		{
			if (!Sync.IsDedicated)
			{
				MyGameService.OnDLCInstalled -= OnDLCInstalled;
			}
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				MyMultiplayer.Static.ClientJoined -= UpdateClientDLC;
			}
			MyAPIGateway.DLC = null;
			base.UnloadData();
		}

		/// <summary>
		/// Cache data about local player's DLC ownership.
		/// </summary>
		private void UpdateLocalPlayerDLC()
		{
			int dLCCount = MyGameService.GetDLCCount();
			for (int i = 0; i < dLCCount; i++)
			{
				MyGameService.GetDLCDataByIndex(i, out var dlcId, out var _, out var _, 128);
				if (MyGameService.IsDlcInstalled(dlcId))
				{
					if (!m_availableDLCs.Contains(dlcId))
					{
						m_availableDLCs.Add(dlcId);
						this.DLCInstalled.InvokeIfNotNull(Sync.MyId, dlcId);
					}
					if (dlcId == MyFakes.SWITCH_DLC_FROM)
					{
						m_availableDLCs.Add(MyFakes.SWITCH_DLC_TO);
					}
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Request upating of DLC cache on server from a client.
		/// </summary>
		[Event(null, 114)]
=======
		[Event(null, 106)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void RequestUpdateClientDLC()
		{
			MySession.Static.GetComponent<MySessionComponentDLC>().UpdateClientDLC(MyEventContext.Current.Sender.Value);
		}

		private void UpdateClientDLC(ulong steamId, string userName)
		{
			UpdateClientDLC(steamId);
		}

<<<<<<< HEAD
		/// <summary>
		/// Cache data about client's DLC ownership.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void UpdateClientDLC(ulong steamId)
		{
			if (!m_clientAvailableDLCs.TryGetValue(steamId, out var value))
			{
				value = new HashSet<uint>();
				m_clientAvailableDLCs.Add(steamId, value);
			}
			foreach (uint key in MyDLCs.DLCs.Keys)
			{
				if (!Sync.IsDedicated || MyGameService.GameServer.UserHasLicenseForApp(steamId, key))
				{
					if (!value.Contains(key))
					{
						value.Add(key);
						this.DLCInstalled.InvokeIfNotNull(steamId, key);
					}
					if (key == MyFakes.SWITCH_DLC_FROM)
					{
						value.Add(MyFakes.SWITCH_DLC_TO);
					}
				}
			}
		}

		/// <summary>
		/// Return whether a player owns a DLC.
		/// </summary>
		public bool HasDLC(string DLCName, ulong steamId)
		{
			if (MyFakes.OWN_ALL_DLCS)
			{
				return true;
			}
			if (steamId == 0L)
			{
				return false;
			}
			if (steamId == Sync.MyId)
			{
				if (Sync.IsDedicated)
				{
					return false;
				}
				if (MyDLCs.TryGetDLC(DLCName, out var dlc))
				{
					return m_availableDLCs.Contains(dlc.AppId);
				}
				return false;
			}
			if (Sync.IsServer)
			{
				if (MyDLCs.TryGetDLC(DLCName, out var dlc2))
				{
					return HasClientDLC(dlc2.AppId, steamId);
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// Return whether a client owns a DLC.
		/// </summary>
		private bool HasClientDLC(uint DLCId, ulong steamId)
		{
			if (!m_clientAvailableDLCs.TryGetValue(steamId, out var value))
			{
				UpdateClientDLC(steamId);
				value = m_clientAvailableDLCs[steamId];
			}
			return value.Contains(DLCId);
		}

		/// <summary>
		/// Returns whether a player owns all DLCs required by this definition id.
		/// </summary>
		public bool HasDefinitionDLC(MyDefinitionId definitionId, ulong steamId)
		{
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(definitionId);
			return HasDefinitionDLC(definition, steamId);
		}

		/// <summary>
		/// Returns whether a player owns all DLCs required by this definition.
		/// </summary>
		public bool HasDefinitionDLC(MyDefinitionBase definition, ulong steamId)
		{
			if (definition == null)
			{
				return true;
			}
			if (definition.DLCs == null)
			{
				return true;
			}
			string[] dLCs = definition.DLCs;
			foreach (string dLCName in dLCs)
			{
				if (!HasDLC(dLCName, steamId))
				{
					return false;
				}
			}
			return true;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns whether collection contains dlcs in this definition.
		/// </summary>
		public bool ContainsRequiredDLC(MyDefinitionBase definition, List<ulong> dlcs)
		{
			if (definition?.DLCs == null || (definition.DLCs != null && definition.DLCs.Count() == 0))
=======
		public bool ContainsRequiredDLC(MyDefinitionBase definition, List<ulong> dlcs)
		{
			if (definition.DLCs == null || (definition != null && Enumerable.Count<string>((IEnumerable<string>)definition.DLCs) == 0))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return true;
			}
			if (dlcs == null)
			{
				return false;
			}
			string[] dLCs = definition.DLCs;
			for (int i = 0; i < dLCs.Length; i++)
			{
				if (MyDLCs.TryGetDLC(dLCs[i], out var dlc) && !dlcs.Contains(dlc.AppId))
				{
					return false;
				}
			}
			return true;
		}

<<<<<<< HEAD
		/// <summary>
		/// Get the first DLC a player is missing that a definition requires. Null if they have all.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyDLCs.MyDLC GetFirstMissingDefinitionDLC(MyDefinitionBase definition, ulong steamId)
		{
			if (definition.DLCs == null)
			{
				return null;
			}
			string[] dLCs = definition.DLCs;
			foreach (string text in dLCs)
			{
				if (!HasDLC(text, steamId))
				{
					MyDLCs.TryGetDLC(text, out var dlc);
					return dlc;
				}
			}
			return null;
		}

		/// <summary>
		/// Add a DLC to owned DLCs. Notify server.
		/// </summary>
		private void OnDLCInstalled(uint dlcId)
		{
			m_availableDLCs.Add(dlcId);
			if (!Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestUpdateClientDLC);
			}
		}

		/// <summary>
		/// Log that an unowned DLC has started being used in some way by the local player.
		/// </summary>
		public void PushUsedUnownedDLC(MyDLCs.MyDLC dlc)
		{
			if (m_usedUnownedDLCs.ContainsKey(dlc))
			{
				m_usedUnownedDLCs[dlc]++;
			}
			else
			{
				m_usedUnownedDLCs[dlc] = 1;
			}
		}

		/// <summary>
		/// Log that an unowned DLC has stopped being used in some way by the local player.
		/// </summary>
		public void PopUsedUnownedDLC(MyDLCs.MyDLC dlc)
		{
			if (m_usedUnownedDLCs.TryGetValue(dlc, out var value) && value > 0)
			{
				if (value > 1)
				{
					m_usedUnownedDLCs[dlc]--;
				}
				else
				{
					m_usedUnownedDLCs.Remove(dlc);
				}
			}
		}

		bool IMyDLCs.ContainsRequiredDLC(MyDefinitionBase definition, List<ulong> dlcs)
		{
			return ContainsRequiredDLC(definition, dlcs);
		}

		ListReader<uint> IMyDLCs.GetAvailableClientDLCIds()
		{
			List<ulong> availableClientDLCsIds = GetAvailableClientDLCsIds();
			List<uint> list = new List<uint>();
			for (int i = 0; i < availableClientDLCsIds.Count; i++)
			{
				list.Add((uint)availableClientDLCsIds[i]);
			}
			return list;
		}

		IMyDLC IMyDLCs.GetDLC(string name)
		{
			return MyDLCs.GetDLC(name);
		}

		IMyDLC IMyDLCs.GetDLC(uint appId)
		{
			return MyDLCs.DLCs[appId];
		}

		ListReader<IMyDLC> IMyDLCs.GetDLCs()
		{
			List<IMyDLC> list = new List<IMyDLC>();
			foreach (MyDLCs.MyDLC dLC in MyDLCs.MyDLC.DLCList)
			{
				list.Add(dLC);
			}
			return list;
		}

		IMyDLC IMyDLCs.GetFirstMissingDefinitionDLC(MyDefinitionBase definition, ulong steamId)
		{
			return GetFirstMissingDefinitionDLC(definition, steamId);
		}

		string IMyDLCs.GetRequiredDLCTooltip(string name)
		{
			return MyDLCs.GetRequiredDLCTooltip(name);
		}

		string IMyDLCs.GetRequiredDLCTooltip(uint appId)
		{
			return MyDLCs.GetRequiredDLCTooltip(appId);
		}

		bool IMyDLCs.HasDefinitionDLC(MyDefinitionId definitionId, ulong steamId)
		{
			return HasDefinitionDLC(definitionId, steamId);
		}

		bool IMyDLCs.HasDefinitionDLC(MyDefinitionBase definition, ulong steamId)
		{
			return HasDefinitionDLC(definition, steamId);
		}

		bool IMyDLCs.HasDLC(string DLCName, ulong steamId)
		{
			return HasDLC(DLCName, steamId);
		}

		bool IMyDLCs.HasDLC(uint appId, ulong steamId)
		{
			if (MyDLCs.TryGetDLC(appId, out var dlc))
			{
				return HasDLC(dlc.Name, steamId);
			}
			return false;
		}

		bool IMyDLCs.IsDLCSupported(string name)
		{
			return MyDLCs.IsDLCSupported(name);
		}

		bool IMyDLCs.TryGetDLC(uint appId, out IMyDLC dlc)
		{
			MyDLCs.MyDLC dlc2;
			bool result = MyDLCs.TryGetDLC(appId, out dlc2);
			dlc = dlc2;
			return result;
		}

		bool IMyDLCs.TryGetDLC(string name, out IMyDLC dlc)
		{
			MyDLCs.MyDLC dlc2;
			bool result = MyDLCs.TryGetDLC(name, out dlc2);
			dlc = dlc2;
			return result;
		}
	}
}
