using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyNullInventoryService : IMyInventoryService
	{
		public IDictionary<int, MyGameInventoryItemDefinition> Definitions { get; }

		public ICollection<MyGameInventoryItem> InventoryItems { get; }

		public int RecycleTokens { get; }

		public event EventHandler InventoryRefreshed;

		public event EventHandler<MyGameItemsEventArgs> ItemsAdded;

		public event EventHandler NoItemsReceived;

		public event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

		protected virtual void DoInventoryRefreshed()
		{
			this.InventoryRefreshed?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void DoItemsAdded(MyGameItemsEventArgs e)
		{
			this.ItemsAdded?.Invoke(this, e);
		}

		protected virtual void DoNoItemsReceived()
		{
			this.NoItemsReceived?.Invoke(this, EventArgs.Empty);
		}

		protected void DoCheckItemDataReady(MyGameItemsEventArgs e)
		{
			this.CheckItemDataReady?.Invoke(this, e);
		}

		public void GetAllItems()
		{
		}

		public MyGameInventoryItemDefinition GetInventoryItemDefinition(string assetModifierId)
		{
			return null;
		}

		public IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot)
		{
			return new List<MyGameInventoryItemDefinition>();
		}

		public bool HasInventoryItemWithDefinitionId(int id)
		{
			return false;
		}

		public bool HasInventoryItem(ulong id)
		{
			return false;
		}

		public bool HasInventoryItem(string assetModifierId)
		{
			return false;
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

		public List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
			checkResult = false;
			return null;
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
	}
}
