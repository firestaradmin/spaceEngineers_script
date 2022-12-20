using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	/// <summary>
	/// Session component taking care of game inventory checking.
	/// </summary>
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 2019, typeof(MyObjectBuilder_SessionComponentGameInventory), null, false)]
	public class MySessionComponentGameInventory : MySessionComponentBase
	{
		protected sealed class RequestUpdateClientGameInventory_003C_003ESystem_Byte_003C_0023_003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in byte[] checkData, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestUpdateClientGameInventory(checkData);
			}
		}

		protected sealed class UpdateClientGameInventoryResult_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool success, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateClientGameInventoryResult(success);
			}
		}

		public static bool DEBUG_REVOKE_ITEM_OWNERSHIP;

		private const int MAX_TRIES = 5;

		private HashSet<MyStringHash> m_availableArmors;

		private MyHashSetDictionary<ulong, MyStringHash> m_clientAvailableArmors;

		private CachingDictionary<ulong, byte[]> m_pendingUpdates;

		private Dictionary<ulong, int> m_triesLeft;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			if (MyGameService.Service.IsActive || Sync.IsDedicated)
			{
				base.Init(sessionComponent);
				m_availableArmors = new HashSet<MyStringHash>();
				if (!Sync.IsDedicated)
				{
					UpdateLocalPlayerGameInventory();
					MyGameService.InventoryRefreshed += MyGameServiceOnInventoryRefreshed;
				}
				if (MyMultiplayer.Static != null && Sync.IsServer)
				{
					m_pendingUpdates = new CachingDictionary<ulong, byte[]>();
					m_triesLeft = new Dictionary<ulong, int>();
					m_clientAvailableArmors = new MyHashSetDictionary<ulong, MyStringHash>();
				}
			}
		}

		private void MyGameServiceOnInventoryRefreshed(object sender, EventArgs e)
		{
			UpdateLocalPlayerGameInventory();
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!Sync.IsServer || m_pendingUpdates == null)
			{
				return;
			}
			m_pendingUpdates.ApplyChanges();
			foreach (KeyValuePair<ulong, byte[]> pendingUpdate in m_pendingUpdates)
			{
				if (m_triesLeft.TryGetValue(pendingUpdate.Key, out var value) && value > 0)
				{
					UpdateClientGameInventory(pendingUpdate.Key, pendingUpdate.Value, value > 1);
					m_triesLeft[pendingUpdate.Key] = value - 1;
				}
				else
				{
					m_triesLeft.Remove(pendingUpdate.Key);
				}
			}
		}

		protected override void UnloadData()
		{
			if (!Sync.IsDedicated && MyGameService.IsActive)
			{
				MyGameService.InventoryRefreshed -= MyGameServiceOnInventoryRefreshed;
			}
			base.UnloadData();
		}

		/// <summary>
		/// Cache data about local player's inventory item ownership.
		/// </summary>
		private void UpdateLocalPlayerGameInventory()
		{
			if (Sync.IsDedicated || MySession.Static == null)
			{
				return;
			}
			m_availableArmors.Clear();
			ICollection<MyGameInventoryItem> inventoryItems = MyGameService.InventoryItems;
			if (inventoryItems == null)
			{
				return;
			}
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
			foreach (MyGameInventoryItem item in inventoryItems)
			{
				if (item.ItemDefinition != null && item.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
				{
					list.Add(item);
					m_availableArmors.Add(MyStringHash.GetOrCompute(item.ItemDefinition.AssetModifierId));
				}
			}
			if (list.Count <= 0 || Sync.IsServer)
			{
				return;
			}
			MyGameService.GetItemsCheckData(list, delegate(byte[] checkData)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestUpdateClientGameInventory, checkData);
			});
		}

<<<<<<< HEAD
		/// <summary>
		/// Request updating of game inventory cache on server from a client.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Event(null, 138)]
		[Reliable]
		[Server]
		public static void RequestUpdateClientGameInventory(byte[] checkData)
		{
			MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
			component.m_triesLeft[MyEventContext.Current.Sender.Value] = 5;
			component.UpdateClientGameInventory(MyEventContext.Current.Sender.Value, checkData, retry: true);
		}

<<<<<<< HEAD
		/// <summary>
		/// Server response for request updating of game inventory cache on server.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Event(null, 149)]
		[Reliable]
		[Client]
		public static void UpdateClientGameInventoryResult(bool success)
		{
			if (!success)
			{
				MySession.Static.GetComponent<MySessionComponentGameInventory>().UpdateLocalPlayerGameInventory();
				MyLog.Default.Log(MyLogSeverity.Warning, "Server failed to update game inventory items.");
			}
		}

		/// <summary>
		/// Cache data about client's game inventory.
		/// </summary>
		private void UpdateClientGameInventory(ulong steamId, byte[] checkData, bool retry)
		{
			bool checkResult;
			List<MyGameInventoryItem> list = MyGameService.CheckItemData(checkData, out checkResult);
			if (!checkResult)
			{
				if (retry)
				{
					m_pendingUpdates[steamId] = checkData;
					return;
				}
				m_pendingUpdates.Remove(steamId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateClientGameInventoryResult, arg2: false, new EndpointId(steamId));
				return;
			}
			m_pendingUpdates.Remove(steamId);
			HashSet<MyStringHash> orAdd = m_clientAvailableArmors.GetOrAdd(steamId);
			orAdd.Clear();
			foreach (MyGameInventoryItem item in list)
			{
				if (item.ItemDefinition != null && item.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
				{
					orAdd.Add(MyStringHash.GetOrCompute(item.ItemDefinition.AssetModifierId));
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateClientGameInventoryResult, arg2: true, new EndpointId(steamId));
		}

		public MyStringHash ValidateArmor(MyStringHash armorId, ulong steamId)
		{
			if (!HasArmor(armorId, steamId))
			{
				return MyStringHash.NullOrEmpty;
			}
			MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), armorId));
			if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(assetModifierDefinition, steamId))
			{
				return armorId;
			}
			return MyStringHash.NullOrEmpty;
		}

		/// <summary>
		/// Return whether a player owns an armor.
		/// </summary>
		public bool HasArmor(MyStringHash armorId, ulong steamId)
		{
			if (steamId == 0L || DEBUG_REVOKE_ITEM_OWNERSHIP || m_availableArmors == null)
			{
				return false;
			}
			if (steamId == Sync.MyId)
			{
				if (Sync.IsDedicated)
				{
					return false;
				}
				return m_availableArmors.Contains(armorId);
			}
			if (Sync.IsServer)
			{
				return HasClientArmor(armorId, steamId);
			}
			return false;
		}

		/// <summary>
		/// Return whether a client owns an armor.
		/// </summary>
		private bool HasClientArmor(MyStringHash armorId, ulong steamId)
		{
			if (m_clientAvailableArmors.TryGet(steamId, out var list))
			{
				return list.Contains(armorId);
			}
			return false;
		}
	}
}
