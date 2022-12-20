using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using VRage.GameServices;
using VRage.Utils;

namespace VRage.Steam
{
	internal class MySteamInventoryServer : IMyInventoryService
	{
		private const int MAXIMUM_CHECKDATA_TRIES = 200;

		private readonly ConcurrentDictionary<int, MyGameInventoryItemDefinition> m_itemDefinitions;

		private SteamItemDef_t[] m_steamItemDef;

		private Callback<SteamInventoryDefinitionUpdate_t> m_steamInventoryDefinitionUpdate;

		public IDictionary<int, MyGameInventoryItemDefinition> Definitions => m_itemDefinitions;

		public ICollection<MyGameInventoryItem> InventoryItems { get; }

		public int RecycleTokens { get; }

		public event EventHandler InventoryRefreshed;

		public event EventHandler<MyGameItemsEventArgs> ItemsAdded;

		public event EventHandler NoItemsReceived;

		public event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

		public MySteamInventoryServer()
		{
			m_steamInventoryDefinitionUpdate = Callback<SteamInventoryDefinitionUpdate_t>.CreateGameServer(OnSteamInventoryDefinitionUpdate);
			m_itemDefinitions = new ConcurrentDictionary<int, MyGameInventoryItemDefinition>();
			SteamGameServerInventory.LoadItemDefinitions();
			GetAllItems();
		}

		public void GetAllItems()
		{
			SteamGameServerInventory.GetAllItems(out var _);
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

		public void TriggerPersonalContainer()
		{
		}

		public void TriggerCompetitiveContainer()
		{
		}

		public void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
		{
		}

		public void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
		{
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

		private bool GetItemDefinitionPropertyBool(string propertyName, SteamItemDef_t item)
		{
			return GetItemDefinitionProperty(propertyName, item) == "1";
		}

		private string GetItemDefinitionProperty(string propertyName, SteamItemDef_t item)
		{
			uint punValueBufferSizeOut = 2048u;
			if (!SteamGameServerInventory.GetItemDefinitionProperty(item, propertyName, out var pchValueBuffer, ref punValueBufferSizeOut))
			{
				return string.Empty;
			}
			return pchValueBuffer;
		}

		public List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
			SteamGameServerInventory.DeserializeResult(out var pOutResultHandle, checkData, (uint)checkData.Length);
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
			uint punOutItemsArraySize = 0u;
			bool flag = false;
			EResult eResult = EResult.k_EResultPending;
			int num = 200;
			while (!flag && eResult == EResult.k_EResultPending && num > 0)
			{
				num--;
				flag = SteamGameServerInventory.GetResultItems(pOutResultHandle, null, ref punOutItemsArraySize);
				eResult = SteamGameServerInventory.GetResultStatus(pOutResultHandle);
			}
			if (flag && punOutItemsArraySize != 0)
			{
				flag = ExtractItemsFromResult(punOutItemsArraySize, pOutResultHandle, list);
			}
			eResult = SteamGameServerInventory.GetResultStatus(pOutResultHandle);
			SteamGameServerInventory.DestroyResult(pOutResultHandle);
			checkResult = flag && eResult == EResult.k_EResultOK;
			return list;
		}

		public void ConsumeItem(MyGameInventoryItem item)
		{
		}

		public bool RecycleItem(MyGameInventoryItem item)
		{
			return false;
		}

		public bool CraftSkin(MyGameInventoryItemQuality quality)
		{
			return false;
		}

		public uint GetCraftingCost(MyGameInventoryItemQuality quality)
		{
			return 0u;
		}

		public uint GetRecyclingReward(MyGameInventoryItemQuality quality)
		{
			return 0u;
		}

		public void AddUnownedItems()
		{
		}

		public void Update()
		{
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
				flag = SteamGameServerInventory.GetResultItems(inventoryResult, array, ref itemsArraySize);
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

		private void OnSteamInventoryDefinitionUpdate(SteamInventoryDefinitionUpdate_t param)
		{
			uint punItemDefIDsArraySize = 0u;
			if (SteamGameServerInventory.GetItemDefinitionIDs(null, ref punItemDefIDsArraySize))
			{
				m_steamItemDef = new SteamItemDef_t[punItemDefIDsArraySize];
				SteamGameServerInventory.GetItemDefinitionIDs(m_steamItemDef, ref punItemDefIDsArraySize);
				SteamItemDef_t[] steamItemDef = m_steamItemDef;
				for (int i = 0; i < steamItemDef.Length; i++)
				{
					SteamItemDef_t item = steamItemDef[i];
					if (!m_itemDefinitions.ContainsKey(item.m_SteamItemDef))
					{
						MyGameInventoryItemDefinition value = CreateItemDefinition(item);
						m_itemDefinitions.TryAdd(item.m_SteamItemDef, value);
					}
				}
				MyLog.Default.WriteLineAndConsole($"Loaded {punItemDefIDsArraySize} Steam Inventory item definitions");
			}
			else
			{
				MyLog.Default.WriteLineAndConsole("Failed to load Steam Inventory item definitions");
			}
		}

		public IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot)
		{
			return m_itemDefinitions.Values.Where((MyGameInventoryItemDefinition e) => e.ItemSlot == slot);
		}

		public bool HasInventoryItem(string assetModifierId)
		{
			return InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ItemDefinition?.AssetModifierId == assetModifierId) != null;
		}

		protected virtual void OnInventoryRefreshed()
		{
			this.InventoryRefreshed?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnItemsAdded(MyGameItemsEventArgs e)
		{
			this.ItemsAdded?.Invoke(this, e);
		}

		protected virtual void OnNoItemsReceived()
		{
			this.NoItemsReceived?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnCheckItemDataReady(MyGameItemsEventArgs e)
		{
			this.CheckItemDataReady?.Invoke(this, e);
		}
	}
}
