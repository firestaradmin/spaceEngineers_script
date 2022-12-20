using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LitJson;
using VRage.FileSystem;

namespace VRage.GameServices
{
	public class MyMockingInventory : IMyInventoryService
	{
		private class MyItemDefinitions
		{
			public uint appid;

			public List<MyGameInventoryItemDefinitionRaw> items;
		}

		private class MyGameInventoryItemDefinitionRaw
		{
			public int itemdefid;

			public string name;

			public string type;

			public string name_color;

			public string background_color;

			public string icon_texture;

			public bool hidden;

			public bool store_hidden;

			public string item_slot;

			public int item_quality;

			public string asset_modifier_id;

			public string tool_name;

			public string exchange;

			public string promo;

			public string bundle;

			public MyGameInventoryItemDefinition CreateDefinition()
			{
				MyGameInventoryItemDefinition myGameInventoryItemDefinition = new MyGameInventoryItemDefinition();
				myGameInventoryItemDefinition.ID = itemdefid;
				myGameInventoryItemDefinition.Name = name;
				myGameInventoryItemDefinition.DisplayType = type;
				myGameInventoryItemDefinition.AssetModifierId = asset_modifier_id;
				myGameInventoryItemDefinition.BackgroundColor = background_color;
				myGameInventoryItemDefinition.IconTexture = icon_texture;
				myGameInventoryItemDefinition.NameColor = name_color;
				myGameInventoryItemDefinition.ToolName = tool_name;
				myGameInventoryItemDefinition.IsStoreHidden = store_hidden;
				myGameInventoryItemDefinition.Hidden = hidden;
				myGameInventoryItemDefinition.Exchange = exchange;
				if (Enum.TryParse<MyGameInventoryItemDefinitionType>(type, out var result))
				{
					myGameInventoryItemDefinition.DefinitionType = result;
				}
				else
				{
					myGameInventoryItemDefinition.DefinitionType = MyGameInventoryItemDefinitionType.none;
				}
				if (Enum.TryParse<MyGameInventoryItemSlot>(item_slot, out var result2))
				{
					myGameInventoryItemDefinition.ItemSlot = result2;
				}
				else
				{
					myGameInventoryItemDefinition.ItemSlot = MyGameInventoryItemSlot.None;
				}
				myGameInventoryItemDefinition.ItemQuality = (MyGameInventoryItemQuality)item_quality;
				return myGameInventoryItemDefinition;
			}
		}

		private readonly ConcurrentQueue<Action> m_updateThreadInvocationQueue = new ConcurrentQueue<Action>();

		private readonly ConcurrentDictionary<ulong, MyGameInventoryItem> m_inventory = new ConcurrentDictionary<ulong, MyGameInventoryItem>();

		private readonly ConcurrentDictionary<int, MyGameInventoryItemDefinition> m_itemDefinitions = new ConcurrentDictionary<int, MyGameInventoryItemDefinition>();

		private readonly Dictionary<int, uint> m_itemToDlc = new Dictionary<int, uint>();

		private readonly Dictionary<uint, List<MyGameInventoryItemDefinitionRaw>> m_dlcDefinitions = new Dictionary<uint, List<MyGameInventoryItemDefinitionRaw>>();

		private readonly Dictionary<string, List<MyGameInventoryItemDefinitionRaw>> m_achievementDefinitions = new Dictionary<string, List<MyGameInventoryItemDefinitionRaw>>();

		private readonly Dictionary<int, MyGameInventoryItemDefinitionRaw> m_inventoryDefinitionsDictionary = new Dictionary<int, MyGameInventoryItemDefinitionRaw>();

		private readonly HashSet<int> m_achievementItems = new HashSet<int>();

		private uint m_alwaysSupportedDlcId;

		private IMyGameService m_service;

<<<<<<< HEAD
		public ICollection<MyGameInventoryItem> InventoryItems => m_inventory.Values;

		public IDictionary<int, MyGameInventoryItemDefinition> Definitions => m_itemDefinitions;
=======
		public ICollection<MyGameInventoryItem> InventoryItems => m_inventory.get_Values();

		public IDictionary<int, MyGameInventoryItemDefinition> Definitions => (IDictionary<int, MyGameInventoryItemDefinition>)m_itemDefinitions;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public int RecycleTokens { get; }

		public event EventHandler InventoryRefreshed;

		public event EventHandler NoItemsReceived;

		public event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

		public event EventHandler<MyGameItemsEventArgs> ItemsAdded;

		public MyMockingInventory(IMyGameService service)
		{
			m_service = service;
		}

		public void InitInventory()
		{
			InitDefinitions();
			PopulateInventoryFromDLCs();
		}

		public void InitInventoryForUser()
		{
			PopulateInventoryFromDLCs();
		}

		public void GetAllItems()
		{
			InvokeOnMainThread(delegate
			{
				this.InventoryRefreshed?.Invoke(this, EventArgs.Empty);
			});
		}

		public MyGameInventoryItemDefinition GetInventoryItemDefinition(string assetModifierId)
		{
<<<<<<< HEAD
			return m_itemDefinitions.Values.FirstOrDefault((MyGameInventoryItemDefinition x) => x.AssetModifierId == assetModifierId);
=======
			return Enumerable.FirstOrDefault<MyGameInventoryItemDefinition>((IEnumerable<MyGameInventoryItemDefinition>)m_itemDefinitions.get_Values(), (Func<MyGameInventoryItemDefinition, bool>)((MyGameInventoryItemDefinition x) => x.AssetModifierId == assetModifierId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public bool HasInventoryItem(ulong id)
		{
			return m_inventory.ContainsKey(id);
		}

		public void TriggerPersonalContainer()
		{
		}

		public void TriggerCompetitiveContainer()
		{
		}

		public void GetItemCheckData(MyGameInventoryItem item)
		{
		}

		public List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
			List<MyGameInventoryItem> list = MyInventoryHelper.CheckItemData(checkData, out checkResult);
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

		public void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
		{
			foreach (MyGameInventoryItem item in items)
			{
				m_itemDefinitions.TryAdd(item.ItemDefinition.ID, item.ItemDefinition);
			}
			byte[] data = MyInventoryHelper.GetItemsCheckData(items);
			if (completedAction != null)
			{
				InvokeOnMainThread(delegate
				{
					completedAction(data);
				});
			}
		}

		public void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
		{
			byte[] data = MyInventoryHelper.GetItemCheckData(item);
			if (item != null)
			{
				m_itemDefinitions.TryAdd(item.ItemDefinition.ID, item.ItemDefinition);
				MyGameItemsEventArgs args = new MyGameItemsEventArgs
				{
					CheckData = data,
					NewItems = new List<MyGameInventoryItem> { item }
				};
				if (this.CheckItemDataReady != null)
				{
					InvokeOnMainThread(delegate
					{
						this.CheckItemDataReady?.Invoke(this, args);
					});
				}
			}
			if (completedAction != null)
			{
				InvokeOnMainThread(delegate
				{
					completedAction(data);
				});
			}
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

		public bool HasInventoryItemWithDefinitionId(int id)
		{
<<<<<<< HEAD
			return m_inventory.Values.Any((MyGameInventoryItem x) => x.ItemDefinition.ID == id);
=======
			return Enumerable.Any<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)m_inventory.get_Values(), (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem x) => x.ItemDefinition.ID == id));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public bool HasInventoryItem(string assetModifierId)
		{
<<<<<<< HEAD
			return m_inventory.Values.Any((MyGameInventoryItem x) => x.ItemDefinition.AssetModifierId == assetModifierId);
=======
			return Enumerable.Any<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)m_inventory.get_Values(), (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem x) => x.ItemDefinition.AssetModifierId == assetModifierId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void InitDefinitions()
		{
			m_dlcDefinitions.Clear();
			m_achievementDefinitions.Clear();
			m_inventoryDefinitionsDictionary.Clear();
			m_itemDefinitions.Clear();
			m_itemToDlc.Clear();
			m_achievementItems.Clear();
			MyItemDefinitions myItemDefinitions = AddDefinitionsFromFile("DataPlatform/InventoryItems.json");
			MyItemDefinitions myItemDefinitions2 = AddDefinitionsFromFile("DataPlatform/InventoryItemsXBL.json");
			myItemDefinitions.items.AddRange(myItemDefinitions2.items);
			foreach (MyGameInventoryItemDefinitionRaw item in myItemDefinitions.items)
			{
				if (string.IsNullOrEmpty(item.promo))
				{
					continue;
				}
				string[] array = item.promo.Split(new char[1] { ':' });
				if (array.Length != 2)
				{
					continue;
				}
				if (array[0] == "owns")
				{
					if (!uint.TryParse(array[1], out var result))
					{
						continue;
					}
					bool flag = result == m_alwaysSupportedDlcId || m_service.IsDlcSupported(result);
					AddDlcItem(result, item, flag);
					if (flag)
					{
						if (m_dlcDefinitions.TryGetValue(result, out var value))
						{
							value.Add(item);
							continue;
						}
						m_dlcDefinitions.Add(result, new List<MyGameInventoryItemDefinitionRaw> { item });
					}
				}
				else if (array[0] == "ach")
				{
					if (m_achievementDefinitions.TryGetValue(array[1], out var value2))
					{
						value2.Add(item);
					}
					else
					{
						m_achievementDefinitions.Add(array[1], new List<MyGameInventoryItemDefinitionRaw> { item });
					}
					AddAchievementItem(item);
				}
			}
			foreach (MyGameInventoryItemDefinitionRaw item2 in myItemDefinitions.items)
			{
				if (!m_itemToDlc.ContainsKey(item2.itemdefid) && !m_achievementItems.Contains(item2.itemdefid))
				{
<<<<<<< HEAD
					m_itemDefinitions.Remove(item2.itemdefid);
=======
					m_itemDefinitions.Remove<int, MyGameInventoryItemDefinition>(item2.itemdefid);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_inventoryDefinitionsDictionary.Remove(item2.itemdefid);
				}
			}
		}

		private MyItemDefinitions AddDefinitionsFromFile(string dataplatformInventoryitemsJson)
		{
			MyItemDefinitions myItemDefinitions = JsonMapper.ToObject<MyItemDefinitions>(new JsonReader(new StreamReader(Path.Combine(MyFileSystem.ContentPath, dataplatformInventoryitemsJson))));
			m_alwaysSupportedDlcId = myItemDefinitions.appid;
			foreach (MyGameInventoryItemDefinitionRaw item in myItemDefinitions.items)
			{
				m_inventoryDefinitionsDictionary.ContainsKey(item.itemdefid);
				m_inventoryDefinitionsDictionary[item.itemdefid] = item;
				MyGameInventoryItemDefinition myGameInventoryItemDefinition = item.CreateDefinition();
				m_itemDefinitions.TryAdd(myGameInventoryItemDefinition.ID, myGameInventoryItemDefinition);
			}
			return myItemDefinitions;
		}

		private void AddAchievementItem(MyGameInventoryItemDefinitionRaw def)
		{
			m_achievementItems.Add(def.itemdefid);
			ParseBundle(def, delegate(MyGameInventoryItemDefinitionRaw x)
			{
				AddAchievementItem(x);
			});
		}

		private void AddDlcItem(uint dlcId, MyGameInventoryItemDefinitionRaw def, bool supported)
		{
			if (supported)
			{
				m_itemToDlc[def.itemdefid] = dlcId;
			}
			ParseBundle(def, delegate(MyGameInventoryItemDefinitionRaw x)
			{
				AddDlcItem(dlcId, x, supported);
			});
		}

		private void PopulateInventory(MyGameInventoryItemDefinitionRaw item)
		{
			MyGameInventoryItemDefinition myGameInventoryItemDefinition = item.CreateDefinition();
			MyGameInventoryItem myGameInventoryItem = new MyGameInventoryItem
			{
				ID = (ulong)myGameInventoryItemDefinition.ID,
				ItemDefinition = myGameInventoryItemDefinition,
				Quantity = 1
			};
			m_inventory.TryAdd(myGameInventoryItem.ID, myGameInventoryItem);
			ParseBundle(item, PopulateInventory);
		}

		private void ParseBundle(MyGameInventoryItemDefinitionRaw item, Action<MyGameInventoryItemDefinitionRaw> process)
		{
			if (string.IsNullOrEmpty(item.bundle))
			{
				return;
			}
			string[] array = item.bundle.Split(new char[1] { ';' });
			for (int i = 0; i < array.Length; i++)
			{
				if (int.TryParse(array[i], out var result) && m_inventoryDefinitionsDictionary.TryGetValue(result, out var value))
				{
					process(value);
				}
			}
		}

		public void PopulateInventoryFromDLCs()
		{
			m_inventory.Clear();
			if (m_dlcDefinitions.TryGetValue(m_alwaysSupportedDlcId, out var value))
			{
				foreach (MyGameInventoryItemDefinitionRaw item in value)
				{
					PopulateInventory(item);
				}
			}
			int dLCCount = m_service.GetDLCCount();
			for (int i = 0; i < dLCCount; i++)
			{
				if (!m_service.GetDLCDataByIndex(i, out var dlcId, out var _, out var _, 128) || !m_service.IsDlcInstalled(dlcId) || !m_dlcDefinitions.TryGetValue(dlcId, out value))
				{
					continue;
				}
				foreach (MyGameInventoryItemDefinitionRaw item2 in value)
				{
					PopulateInventory(item2);
				}
			}
			foreach (KeyValuePair<string, List<MyGameInventoryItemDefinitionRaw>> achievementDefinition in m_achievementDefinitions)
			{
				IMyAchievement achievement = m_service.GetAchievement(achievementDefinition.Key);
				if (achievement == null)
				{
					continue;
				}
				if (achievement.IsUnlocked)
				{
					foreach (MyGameInventoryItemDefinitionRaw item3 in achievementDefinition.Value)
					{
						PopulateInventory(item3);
					}
					continue;
				}
				string achievementName = achievementDefinition.Key;
				achievement.OnUnlocked += delegate
				{
					foreach (MyGameInventoryItemDefinitionRaw item4 in m_achievementDefinitions[achievementName])
					{
						PopulateInventory(item4);
					}
					InvokeOnMainThread(delegate
					{
						this.InventoryRefreshed?.Invoke(this, EventArgs.Empty);
					});
				};
			}
			InvokeOnMainThread(delegate
			{
				this.InventoryRefreshed?.Invoke(this, EventArgs.Empty);
			});
		}

		public void AddUnownedItems()
		{
			foreach (KeyValuePair<int, MyGameInventoryItemDefinition> definition in Definitions)
			{
				MyGameInventoryItem myGameInventoryItem = new MyGameInventoryItem
				{
					ID = (ulong)definition.Key,
					ItemDefinition = definition.Value,
					Quantity = 1
				};
				m_inventory.TryAdd(myGameInventoryItem.ID, myGameInventoryItem);
			}
		}

		private void InvokeOnMainThread(Action action)
		{
			m_updateThreadInvocationQueue.Enqueue(action);
		}

		public void Update()
		{
<<<<<<< HEAD
			Action result;
			while (m_updateThreadInvocationQueue.TryDequeue(out result))
			{
				result();
=======
			Action action = default(Action);
			while (m_updateThreadInvocationQueue.TryDequeue(ref action))
			{
				action();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot)
		{
<<<<<<< HEAD
			return m_itemDefinitions.Values.Where((MyGameInventoryItemDefinition e) => e.ItemSlot == slot);
=======
			return Enumerable.Where<MyGameInventoryItemDefinition>((IEnumerable<MyGameInventoryItemDefinition>)m_itemDefinitions.get_Values(), (Func<MyGameInventoryItemDefinition, bool>)((MyGameInventoryItemDefinition e) => e.ItemSlot == slot));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public bool ItemToDlc(int itemId, out uint dlcId)
		{
			return m_itemToDlc.TryGetValue(itemId, out dlcId);
		}

		protected virtual void OnNoItemsReceived()
		{
			this.NoItemsReceived?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnItemsAdded(MyGameItemsEventArgs e)
		{
			this.ItemsAdded?.Invoke(this, e);
		}
	}
}
