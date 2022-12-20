using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using VRage.GameServices;
using VRage.Utils;

namespace VRage.Steam
{
	internal class MySteamInventory : IMyInventoryService
	{
		private const int MAXIMUM_CHECKDATA_TRIES = 200;

		private static readonly SteamItemDef_t m_craftingGeneratorItemID = (SteamItemDef_t)5;

		private static readonly SteamItemDef_t m_personalContainerItemID = (SteamItemDef_t)7;

		private static readonly SteamItemDef_t m_competetiveContainerItemID = (SteamItemDef_t)6;

		private static readonly SteamItemDef_t m_recyclingPointsItemID = (SteamItemDef_t)345;

		private static readonly int[] m_recyclers = new int[5] { 346, 347, 348, 349, 350 };

		private static readonly int[] m_generators = new int[5] { 240, 241, 242, 337, 338 };

		private static readonly uint[] m_craftingCosts = new uint[5] { 4u, 20u, 100u, 500u, 2500u };

		private static readonly uint[] m_recyclingRewards = new uint[5] { 1u, 5u, 25u, 125u, 625u };

		private static readonly SteamItemDef_t m_playTimeGeneratorID = (SteamItemDef_t)6;

		private static readonly bool m_enablePlayTimeGenerator = false;

		private static readonly float m_triggerItemDropsUpdate = 60f;

		private Callback<SteamInventoryResultReady_t> m_SteamInventoryResultReady;

		private Callback<SteamInventoryFullUpdate_t> m_SteamInventoryFullUpdate;

		private Callback<SteamInventoryDefinitionUpdate_t> m_SteamInventoryDefinitionUpdate;

		private ConcurrentDictionary<int, Action<byte[]>> m_checkItemDataResult = new ConcurrentDictionary<int, Action<byte[]>>();

		private MyGameInventoryItem m_ItemForCheck;

		private SteamInventoryResult_t m_triggerResult;

		private bool m_triggerActive;

		private SteamItemDetails_t[] m_SteamItemDetails;

		private SteamItemDef_t[] m_SteamItemDef;

		private byte[] m_SerializedBuffer;

		private ConcurrentDictionary<int, MyGameInventoryItemDefinition> m_itemDefinitions;

		private ConcurrentDictionary<ulong, MyGameInventoryItem> m_inventory;

		private float m_triggerItemDropsTimer;

		private bool m_init;

		private MySteamService m_service;

		public ICollection<MyGameInventoryItem> InventoryItems => m_inventory.Values;

		public IDictionary<int, MyGameInventoryItemDefinition> Definitions => m_itemDefinitions;

		public int RecycleTokens { get; private set; }

		public event EventHandler InventoryRefreshed;

		public event EventHandler<MyGameItemsEventArgs> ItemsAdded;

		public event EventHandler NoItemsReceived;

		public event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

		public MySteamInventory(MySteamService service)
		{
			m_init = true;
			m_SteamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(OnSteamInventoryResultReady);
			m_SteamInventoryFullUpdate = Callback<SteamInventoryFullUpdate_t>.Create(OnSteamInventoryFullUpdate);
			m_SteamInventoryDefinitionUpdate = Callback<SteamInventoryDefinitionUpdate_t>.Create(OnSteamInventoryDefinitionUpdate);
			m_itemDefinitions = new ConcurrentDictionary<int, MyGameInventoryItemDefinition>();
			m_inventory = new ConcurrentDictionary<ulong, MyGameInventoryItem>();
			m_service = service;
			SteamInventory.LoadItemDefinitions();
			GetAllItems();
			SteamInventory.GrantPromoItems(out var _);
		}

		public void Update()
		{
			m_triggerItemDropsTimer += 0.0166666675f;
			if (m_triggerItemDropsTimer > m_triggerItemDropsUpdate)
			{
				SteamInventory.SendItemDropHeartbeat();
				m_triggerItemDropsTimer = 0f;
				if (m_enablePlayTimeGenerator)
				{
					SteamInventory.TriggerItemDrop(out var _, m_playTimeGeneratorID);
					GetAllItems();
				}
			}
		}

		private bool IsCrossplayActive()
		{
			IMyNetworking service = MyServiceManager.Instance.GetService<IMyNetworking>();
			if (service != null)
			{
				return !(service is MySteamNetworking);
			}
			return false;
		}

		private void OnSteamInventoryDefinitionUpdate(SteamInventoryDefinitionUpdate_t param)
		{
			uint punItemDefIDsArraySize = 0u;
			if (SteamInventory.GetItemDefinitionIDs(null, ref punItemDefIDsArraySize))
			{
				m_SteamItemDef = new SteamItemDef_t[punItemDefIDsArraySize];
				SteamInventory.GetItemDefinitionIDs(m_SteamItemDef, ref punItemDefIDsArraySize);
				SteamItemDef_t[] steamItemDef = m_SteamItemDef;
				for (int i = 0; i < steamItemDef.Length; i++)
				{
					SteamItemDef_t item = steamItemDef[i];
					if (!m_itemDefinitions.ContainsKey(item.m_SteamItemDef))
					{
						MyGameInventoryItemDefinition value = CreateItemDefinition(item);
						m_itemDefinitions.TryAdd(item.m_SteamItemDef, value);
					}
				}
			}
			else
			{
				MyLog.Default.WriteLine("OnSteamInventoryDefinitionUpdate: GetItemDefinitionIDs failed");
			}
		}

		private MyGameInventoryItemDefinition CreateItemDefinition(SteamItemDef_t item)
		{
			MyGameInventoryItemDefinition myGameInventoryItemDefinition = new MyGameInventoryItemDefinition();
			myGameInventoryItemDefinition.ID = item.m_SteamItemDef;
			myGameInventoryItemDefinition.Name = GetItemDefinitionProperty("name", item);
			if (string.IsNullOrEmpty(myGameInventoryItemDefinition.Name))
			{
				return null;
			}
			myGameInventoryItemDefinition.Tradable = GetItemDefinitionProperty("tradable", item);
			myGameInventoryItemDefinition.Marketable = GetItemDefinitionProperty("marketable", item);
			myGameInventoryItemDefinition.Description = GetItemDefinitionProperty("description", item);
			myGameInventoryItemDefinition.IconTexture = GetItemDefinitionProperty("icon_texture", item);
			myGameInventoryItemDefinition.DisplayType = GetItemDefinitionProperty("display_type", item);
			myGameInventoryItemDefinition.AssetModifierId = GetItemDefinitionProperty("asset_modifier_id", item);
			myGameInventoryItemDefinition.NameColor = GetItemDefinitionProperty("name_color", item);
			myGameInventoryItemDefinition.BackgroundColor = GetItemDefinitionProperty("background_color", item);
			myGameInventoryItemDefinition.ToolName = GetItemDefinitionProperty("tool_name", item);
			myGameInventoryItemDefinition.IsStoreHidden = GetItemDefinitionPropertyBool("store_hidden", item);
			myGameInventoryItemDefinition.Hidden = GetItemDefinitionPropertyBool("hidden", item);
			myGameInventoryItemDefinition.Exchange = GetItemDefinitionProperty("exchange", item);
			string itemDefinitionProperty = GetItemDefinitionProperty("price_category", item);
			string itemDefinitionProperty2 = GetItemDefinitionProperty("price", item);
			myGameInventoryItemDefinition.CanBePurchased = !myGameInventoryItemDefinition.IsStoreHidden && (!string.IsNullOrEmpty(itemDefinitionProperty) || !string.IsNullOrEmpty(itemDefinitionProperty2));
			if (Enum.TryParse<MyGameInventoryItemDefinitionType>(GetItemDefinitionProperty("type", item), out var result))
			{
				myGameInventoryItemDefinition.DefinitionType = result;
			}
			else
			{
				myGameInventoryItemDefinition.DefinitionType = MyGameInventoryItemDefinitionType.none;
			}
			if (Enum.TryParse<MyGameInventoryItemSlot>(GetItemDefinitionProperty("item_slot", item), out var result2))
			{
				myGameInventoryItemDefinition.ItemSlot = result2;
			}
			else
			{
				myGameInventoryItemDefinition.ItemSlot = MyGameInventoryItemSlot.None;
			}
			if (Enum.TryParse<MyGameInventoryItemQuality>(GetItemDefinitionProperty("item_quality", item), out var result3))
			{
				myGameInventoryItemDefinition.ItemQuality = result3;
			}
			else
			{
				myGameInventoryItemDefinition.ItemQuality = MyGameInventoryItemQuality.Common;
			}
			return myGameInventoryItemDefinition;
		}

		private void OnSteamInventoryFullUpdate(SteamInventoryFullUpdate_t param)
		{
			SteamInventoryResult_t handle = param.m_handle;
			GetResultItems(handle);
		}

		private void OnSteamInventoryResultReady(SteamInventoryResultReady_t param)
		{
			if (param.m_result != EResult.k_EResultOK || !SteamInventory.CheckResultSteamID(param.m_handle, SteamUser.GetSteamID()))
			{
				return;
			}
			SteamInventoryResult_t handle = param.m_handle;
			GetResultItems(handle);
			if (m_checkItemDataResult.ContainsKey(param.m_handle.m_SteamInventoryResult))
			{
				if (SteamInventory.SerializeResult(param.m_handle, null, out var punOutBufferSize))
				{
					m_SerializedBuffer = new byte[punOutBufferSize];
					SteamInventory.SerializeResult(param.m_handle, m_SerializedBuffer, out punOutBufferSize);
				}
				Action<byte[]> action = m_checkItemDataResult[param.m_handle.m_SteamInventoryResult];
				m_checkItemDataResult.Remove(param.m_handle.m_SteamInventoryResult);
				if (this.CheckItemDataReady != null && m_ItemForCheck != null)
				{
					MyGameItemsEventArgs myGameItemsEventArgs = new MyGameItemsEventArgs();
					myGameItemsEventArgs.CheckData = m_SerializedBuffer;
					myGameItemsEventArgs.NewItems = new List<MyGameInventoryItem> { m_ItemForCheck };
					this.CheckItemDataReady(this, myGameItemsEventArgs);
				}
				action?.Invoke(m_SerializedBuffer);
				m_ItemForCheck = null;
			}
			m_init = false;
			SteamInventory.DestroyResult(param.m_handle);
		}

		private void GetResultItems(SteamInventoryResult_t handle)
		{
			uint punOutItemsArraySize = 0u;
			if (SteamInventory.GetResultItems(handle, null, ref punOutItemsArraySize) && punOutItemsArraySize != 0)
			{
				m_SteamItemDetails = new SteamItemDetails_t[punOutItemsArraySize];
				SteamInventory.GetResultItems(handle, m_SteamItemDetails, ref punOutItemsArraySize);
				MyGameItemsEventArgs myGameItemsEventArgs = new MyGameItemsEventArgs();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < punOutItemsArraySize; i++)
				{
					SteamItemDetails_t steamItemDetails_t = m_SteamItemDetails[i];
					if (steamItemDetails_t.m_iDefinition.m_SteamItemDef == 0 && steamItemDetails_t.m_itemId.m_SteamItemInstanceID == 0L)
					{
						continue;
					}
					MyGameInventoryItem value = null;
					if (m_inventory.TryGetValue(steamItemDetails_t.m_itemId.m_SteamItemInstanceID, out value) || HasSteamItemFlags(steamItemDetails_t.m_unFlags, ESteamItemFlags.k_ESteamItemConsumed))
					{
						if (HasSteamItemFlags(steamItemDetails_t.m_unFlags, ESteamItemFlags.k_ESteamItemRemoved) || steamItemDetails_t.m_unQuantity == 0)
						{
							m_inventory.Remove(steamItemDetails_t.m_itemId.m_SteamItemInstanceID);
						}
						else if (value != null)
						{
							value.Quantity = steamItemDetails_t.m_unQuantity;
						}
					}
					else
					{
						if (HasSteamItemFlags(steamItemDetails_t.m_unFlags, ESteamItemFlags.k_ESteamItemRemoved))
						{
							continue;
						}
						MyGameInventoryItemDefinition value2 = null;
						if (!m_itemDefinitions.TryGetValue(steamItemDetails_t.m_iDefinition.m_SteamItemDef, out value2))
						{
							value2 = CreateItemDefinition(steamItemDetails_t.m_iDefinition);
							if (value2 == null)
							{
								continue;
							}
							m_itemDefinitions.TryAdd(value2.ID, value2);
						}
						MyGameInventoryItem myGameInventoryItem = new MyGameInventoryItem();
						myGameInventoryItem.ID = steamItemDetails_t.m_itemId.m_SteamItemInstanceID;
						myGameInventoryItem.ItemDefinition = value2;
						myGameInventoryItem.Quantity = steamItemDetails_t.m_unQuantity;
						m_inventory.TryAdd(myGameInventoryItem.ID, myGameInventoryItem);
						myGameItemsEventArgs.NewItems.Add(myGameInventoryItem);
						stringBuilder.AppendFormat("Steam Item {0} - {1} - {2} - {3} - {4}\n", i, steamItemDetails_t.m_itemId, steamItemDetails_t.m_iDefinition, steamItemDetails_t.m_unQuantity, steamItemDetails_t.m_unFlags);
					}
				}
				StackRecyclePoints();
				if (!m_checkItemDataResult.ContainsKey(handle.m_SteamInventoryResult) && this.InventoryRefreshed != null)
				{
					this.InventoryRefreshed(this, EventArgs.Empty);
				}
				if (!m_init && this.ItemsAdded != null && myGameItemsEventArgs.NewItems != null && myGameItemsEventArgs.NewItems.Count > 0)
				{
					this.ItemsAdded(this, myGameItemsEventArgs);
				}
			}
			else if (m_triggerActive && m_triggerResult == handle && this.NoItemsReceived != null)
			{
				this.NoItemsReceived(this, EventArgs.Empty);
			}
			m_triggerActive = false;
		}

		private void StackRecyclePoints()
		{
			List<MyGameInventoryItem> list = m_inventory.Values.Where((MyGameInventoryItem i) => i.ItemDefinition.ID == m_recyclingPointsItemID.m_SteamItemDef).ToList();
			if (list.Count > 1)
			{
				MyGameInventoryItem myGameInventoryItem = list[0];
				list.Remove(myGameInventoryItem);
				RecycleTokens = myGameInventoryItem.Quantity;
				foreach (MyGameInventoryItem item in list)
				{
					SteamInventory.TransferItemQuantity(out var pResultHandle, (SteamItemInstanceID_t)item.ID, item.Quantity, (SteamItemInstanceID_t)myGameInventoryItem.ID);
					if (SteamInventory.GetResultStatus(pResultHandle) == EResult.k_EResultOK)
					{
						m_inventory.TryRemove(item.ID, out var _);
						SteamInventory.DestroyResult(pResultHandle);
					}
					RecycleTokens += item.Quantity;
				}
			}
			else if (list.Count == 1)
			{
				RecycleTokens = list[0].Quantity;
			}
			else
			{
				RecycleTokens = 0;
			}
		}

		private bool HasSteamItemFlags(ulong value, ESteamItemFlags flag)
		{
			return ((uint)(int)value & (uint)flag) == (uint)flag;
		}

		private bool GetItemDefinitionPropertyBool(string propertyName, SteamItemDef_t item)
		{
			return GetItemDefinitionProperty(propertyName, item) == "1";
		}

		private string GetItemDefinitionProperty(string propertyName, SteamItemDef_t item)
		{
			uint punValueBufferSizeOut = 2048u;
			if (!SteamInventory.GetItemDefinitionProperty(item, propertyName, out var pchValueBuffer, ref punValueBufferSizeOut))
			{
				return string.Empty;
			}
			return pchValueBuffer;
		}

		public void GetAllItems()
		{
			SteamInventory.GetAllItems(out var _);
		}

		public MyGameInventoryItemDefinition GetInventoryItemDefinition(string assetModifierId)
		{
			foreach (MyGameInventoryItemDefinition value in m_itemDefinitions.Values)
			{
				if (value.AssetModifierId == assetModifierId)
				{
					return value;
				}
			}
			return null;
		}

		public void TriggerPersonalContainer()
		{
			SteamInventory.TriggerItemDrop(out var pResultHandle, m_personalContainerItemID);
			m_triggerResult = pResultHandle;
			m_triggerActive = true;
			MyLog.Default.WriteLine($"Steam TriggerItemDrop: {pResultHandle}");
		}

		public void TriggerCompetitiveContainer()
		{
			SteamInventory.TriggerItemDrop(out var pResultHandle, m_competetiveContainerItemID);
			m_triggerResult = pResultHandle;
			m_triggerActive = true;
			MyLog.Default.WriteLine($"Steam TriggerItemDrop: {pResultHandle}");
		}

		public void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
		{
			if (IsCrossplayActive())
			{
				byte[] data = MyInventoryHelper.GetItemCheckData(item);
				if (item != null)
				{
					m_itemDefinitions.TryAdd(item.ItemDefinition.ID, item.ItemDefinition);
					MyGameItemsEventArgs args = new MyGameItemsEventArgs
					{
						CheckData = data
					};
					args.NewItems = new List<MyGameInventoryItem> { item };
					if (this.CheckItemDataReady != null)
					{
						m_service.InvokeOnMainThread(delegate
						{
							this.CheckItemDataReady?.Invoke(this, args);
						});
					}
				}
				if (completedAction != null)
				{
					m_service.InvokeOnMainThread(delegate
					{
						completedAction(data);
					});
				}
			}
			else if (m_ItemForCheck == null)
			{
				SteamItemInstanceID_t[] array = new SteamItemInstanceID_t[1] { (SteamItemInstanceID_t)item.ID };
				m_ItemForCheck = item;
				SteamInventory.GetItemsByID(out var pResultHandle, array, (uint)array.Length);
				m_checkItemDataResult.TryAdd(pResultHandle.m_SteamInventoryResult, completedAction);
			}
		}

		public void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
		{
			if (IsCrossplayActive())
			{
				foreach (MyGameInventoryItem item in items)
				{
					m_itemDefinitions.TryAdd(item.ItemDefinition.ID, item.ItemDefinition);
				}
				byte[] data = MyInventoryHelper.GetItemsCheckData(items);
				if (completedAction != null)
				{
					m_service.InvokeOnMainThread(delegate
					{
						completedAction(data);
					});
				}
			}
			else if (m_ItemForCheck == null)
			{
				SteamItemInstanceID_t[] array = new SteamItemInstanceID_t[items.Count];
				for (int i = 0; i < items.Count; i++)
				{
					array[i] = (SteamItemInstanceID_t)items[i].ID;
				}
				SteamInventory.GetItemsByID(out var pResultHandle, array, (uint)array.Length);
				m_checkItemDataResult.TryAdd(pResultHandle.m_SteamInventoryResult, completedAction);
			}
		}

		public List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
			List<MyGameInventoryItem> list;
			if (IsCrossplayActive())
			{
				list = MyInventoryHelper.CheckItemData(checkData, out checkResult);
				if (checkResult)
				{
					foreach (MyGameInventoryItem item in list)
					{
						m_itemDefinitions.TryAdd(item.ItemDefinition.ID, item.ItemDefinition);
					}
					return list;
				}
				return list;
			}
			SteamInventory.DeserializeResult(out var pOutResultHandle, checkData, (uint)checkData.Length);
			list = new List<MyGameInventoryItem>();
			uint punOutItemsArraySize = 0u;
			bool flag = false;
			EResult eResult = EResult.k_EResultPending;
			int num = 200;
			while (!flag && eResult == EResult.k_EResultPending && num > 0)
			{
				num--;
				flag = SteamInventory.GetResultItems(pOutResultHandle, null, ref punOutItemsArraySize);
				eResult = SteamInventory.GetResultStatus(pOutResultHandle);
			}
			if (flag && punOutItemsArraySize != 0)
			{
				flag = ExtractItemsFromResult(punOutItemsArraySize, pOutResultHandle, list);
			}
			eResult = SteamInventory.GetResultStatus(pOutResultHandle);
			SteamInventory.DestroyResult(pOutResultHandle);
			checkResult = flag && eResult == EResult.k_EResultOK;
			return list;
		}

		private bool ExtractItemsFromResult(uint itemsArraySize, SteamInventoryResult_t inventoryResult, List<MyGameInventoryItem> items)
		{
			SteamItemDetails_t[] array = new SteamItemDetails_t[itemsArraySize];
			bool flag = false;
			EResult eResult = EResult.k_EResultPending;
			int num = 200;
			while (!flag && eResult == EResult.k_EResultPending && num > 0)
			{
				num--;
				flag = SteamInventory.GetResultItems(inventoryResult, array, ref itemsArraySize);
			}
			for (int i = 0; i < array.Length; i++)
			{
				SteamItemDetails_t steamItemDetails_t = array[i];
				MyGameInventoryItemDefinition value = null;
				if (!m_itemDefinitions.TryGetValue(steamItemDetails_t.m_iDefinition.m_SteamItemDef, out value))
				{
					value = CreateItemDefinition(steamItemDetails_t.m_iDefinition);
					if (value == null)
					{
						continue;
					}
					m_itemDefinitions.TryAdd(value.ID, value);
				}
				MyGameInventoryItem myGameInventoryItem = new MyGameInventoryItem();
				myGameInventoryItem.ID = steamItemDetails_t.m_itemId.m_SteamItemInstanceID;
				myGameInventoryItem.ItemDefinition = value;
				myGameInventoryItem.Quantity = steamItemDetails_t.m_unQuantity;
				items.Add(myGameInventoryItem);
			}
			return flag;
		}

		public void ConsumeItem(MyGameInventoryItem item)
		{
			if (item != null)
			{
				SteamItemInstanceID_t itemConsume = (SteamItemInstanceID_t)item.ID;
				SteamInventory.ConsumeItem(out var pResultHandle, itemConsume, item.Quantity);
				if (m_inventory.ContainsKey(item.ID))
				{
					m_inventory.Remove(item.ID);
				}
				SteamInventory.DestroyResult(pResultHandle);
			}
		}

		public bool RecycleItem(MyGameInventoryItem item)
		{
			if (item == null)
			{
				return false;
			}
			SteamItemDef_t[] array = new SteamItemDef_t[1] { (SteamItemDef_t)m_recyclers[(int)item.ItemDefinition.ItemQuality] };
			uint[] punArrayGenerateQuantity = new uint[1] { 1u };
			SteamItemInstanceID_t[] array2 = new SteamItemInstanceID_t[1];
			array2[0].m_SteamItemInstanceID = item.ID;
			SteamInventoryResult_t pResultHandle;
			return SteamInventory.ExchangeItems(punArrayDestroyQuantity: new uint[1] { 1u }, pResultHandle: out pResultHandle, pArrayGenerate: array, punArrayGenerateQuantity: punArrayGenerateQuantity, unArrayGenerateLength: (uint)array.Length, pArrayDestroy: array2, unArrayDestroyLength: (uint)array2.Length);
		}

		public bool CraftSkin(MyGameInventoryItemQuality quality)
		{
			uint tokensQuantity = m_craftingCosts[(int)quality];
			MyGameInventoryItem myGameInventoryItem = m_inventory.Values.FirstOrDefault((MyGameInventoryItem i) => i.ItemDefinition.ID == m_recyclingPointsItemID.m_SteamItemDef && i.Quantity >= tokensQuantity);
			if (myGameInventoryItem == null)
			{
				return false;
			}
			SteamItemDef_t[] array = new SteamItemDef_t[1] { (SteamItemDef_t)m_generators[(int)quality] };
			uint[] punArrayGenerateQuantity = new uint[1] { 1u };
			SteamItemInstanceID_t[] array2 = new SteamItemInstanceID_t[1];
			array2[0].m_SteamItemInstanceID = myGameInventoryItem.ID;
			SteamInventoryResult_t pResultHandle;
			return SteamInventory.ExchangeItems(punArrayDestroyQuantity: new uint[1] { tokensQuantity }, pResultHandle: out pResultHandle, pArrayGenerate: array, punArrayGenerateQuantity: punArrayGenerateQuantity, unArrayGenerateLength: (uint)array.Length, pArrayDestroy: array2, unArrayDestroyLength: (uint)array2.Length);
		}

		public uint GetCraftingCost(MyGameInventoryItemQuality quality)
		{
			return m_craftingCosts[(int)quality];
		}

		public uint GetRecyclingReward(MyGameInventoryItemQuality quality)
		{
			return m_recyclingRewards[(int)quality];
		}

		public void AddUnownedItems()
		{
			Random random = new Random();
			foreach (KeyValuePair<int, MyGameInventoryItemDefinition> definition in Definitions)
			{
				ulong num = (ulong)random.Next();
				while (m_inventory.ContainsKey(num))
				{
					num = (ulong)random.Next();
				}
				if (!m_inventory.Any((KeyValuePair<ulong, MyGameInventoryItem> x) => x.Value.ItemDefinition == definition.Value))
				{
					MyGameInventoryItem value = new MyGameInventoryItem
					{
						ID = num,
						ItemDefinition = definition.Value,
						Quantity = 1
					};
					m_inventory.TryAdd(num, value);
				}
			}
		}

		public IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot)
		{
			return m_itemDefinitions.Values.Where((MyGameInventoryItemDefinition e) => e.ItemSlot == slot);
		}

		public bool HasInventoryItemWithDefinitionId(int id)
		{
			return InventoryItems.Any(delegate(MyGameInventoryItem i)
			{
				MyGameInventoryItemDefinition itemDefinition = i.ItemDefinition;
				return itemDefinition != null && itemDefinition.ID == id;
			});
		}

		public bool HasInventoryItem(ulong id)
		{
			return InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == id) != null;
		}

		public bool HasInventoryItem(string assetModifierId)
		{
			return InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ItemDefinition?.AssetModifierId == assetModifierId) != null;
		}
	}
}
