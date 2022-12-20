using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.SessionComponents;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World.Generator
{
	internal class MyFactionTypeBaseStrategy : IMyStoreItemsGeneratorFactionTypeStrategy
	{
		private static readonly MyDefinitionId m_datapadId = new MyDefinitionId(typeof(MyObjectBuilder_DatapadDefinition), "Datapad");

		protected MyMinimalPriceCalculator m_priceCalculator = new MyMinimalPriceCalculator();

		protected MyFactionTypeDefinition m_definition;

		public MyFactionTypeBaseStrategy(MyDefinitionId factionTypeId)
		{
			m_definition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(factionTypeId);
			if (m_definition != null)
			{
				m_priceCalculator.CalculateMinimalPrices(m_definition.OffersList, m_definition.BaseCostProductionSpeedMultiplier);
				m_priceCalculator.CalculateMinimalPrices(m_definition.OrdersList, m_definition.BaseCostProductionSpeedMultiplier);
			}
		}

		public virtual void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration)
		{
			if (m_definition == null || faction.Stations.Count == 0 || !MyBankingSystem.Static.TryGetAccountInfo(faction.FactionId, out var account))
			{
				return;
			}
			long availableBalance = account.Balance / faction.Stations.Count;
			foreach (MyStation station in faction.Stations)
			{
				int num = 0;
				int num2 = 0;
				bool hasOxygenOffer = false;
				bool hasHydrogenOffer = false;
				List<MyStoreItem> list = new List<MyStoreItem>();
				foreach (MyStoreItem storeItem in station.StoreItems)
				{
					int minimalPrice = 0;
					int minimumOfferAmount = 0;
					int maximumOfferAmount = 0;
					int minimumOrderAmount = 0;
					int maximumOrderAmount = 0;
					MyPhysicalItemDefinition definition = null;
					switch (storeItem.ItemType)
					{
					case ItemTypes.PhysicalItem:
						if (!m_priceCalculator.TryGetItemMinimalPrice(storeItem.Item.Value, out minimalPrice) || !MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(storeItem.Item.Value, out definition))
						{
							continue;
						}
						minimumOfferAmount = definition.MinimumOfferAmount;
						maximumOfferAmount = definition.MaximumOfferAmount;
						minimumOrderAmount = definition.MinimumOrderAmount;
						maximumOrderAmount = definition.MaximumOrderAmount;
						break;
					case ItemTypes.Oxygen:
						minimumOfferAmount = m_definition.MinimumOfferGasAmount;
						maximumOfferAmount = m_definition.MaximumOfferGasAmount;
						hasOxygenOffer = true;
						break;
					case ItemTypes.Hydrogen:
						minimumOfferAmount = m_definition.MinimumOfferGasAmount;
						maximumOfferAmount = m_definition.MaximumOfferGasAmount;
						hasHydrogenOffer = true;
						break;
					case ItemTypes.Grid:
						minimumOfferAmount = 1;
						maximumOfferAmount = 1;
						break;
					}
					switch (storeItem.StoreItemType)
					{
					case StoreItemTypes.Offer:
						storeItem.UpdateOfferItem(m_definition, minimalPrice, minimumOfferAmount, maximumOfferAmount);
						if (storeItem.ItemType != ItemTypes.Grid)
						{
							if (storeItem.IsActive)
							{
								num++;
							}
							else
							{
								list.Add(storeItem);
							}
						}
						else if (!storeItem.IsActive)
						{
							list.Add(storeItem);
						}
						break;
					case StoreItemTypes.Order:
						storeItem.UpdateOrderItem(m_definition, minimalPrice, minimumOrderAmount, maximumOrderAmount, availableBalance);
						if (storeItem.IsActive)
						{
							num2++;
						}
						else
						{
							list.Add(storeItem);
						}
						break;
					}
				}
				foreach (MyStoreItem item in list)
				{
					station.StoreItems.Remove(item);
				}
				GenerateNewStoreItems(station, num, num2, availableBalance, hasOxygenOffer, hasHydrogenOffer, firstGeneration);
				UpdateStationsStoreItems(station, num, num2);
			}
		}

		protected virtual void UpdateStationsStoreItems(MyStation station, int existingOffers, int existingOrders)
		{
		}

		private void GenerateNewStoreItems(MyStation station, int existingOffers, int existingOrders, long availableBalance, bool hasOxygenOffer, bool hasHydrogenOffer, bool firstGeneration)
		{
			GenerateOffers(station, existingOffers, hasOxygenOffer, hasHydrogenOffer, firstGeneration);
			GenerateOrders(station, existingOrders, availableBalance, firstGeneration);
		}

		private void GenerateOffers(MyStation station, int existingOffers, bool hasOxygenOffer, bool hasHydrogenOffer, bool firstGeneration)
		{
			int num = ((m_definition.OffersList != null) ? m_definition.OffersList.Length : 0);
			if (num == 0)
			{
				return;
			}
			float num2 = 1f;
			if (station.IsDeepSpaceStation)
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component == null)
				{
					MyLog.Default.WriteToLogAndAssert("GenerateOffers - Economy session component not found.");
					return;
				}
				num2 = 1f - component.EconomyDefinition.DeepSpaceStationStoreBonus;
			}
			int num3 = num / 2;
			if (num3 > existingOffers)
			{
				num3 -= existingOffers;
				List<SerializableDefinitionId> list = new List<SerializableDefinitionId>(m_definition.OffersList);
				foreach (MyStoreItem storeItem in station.StoreItems)
				{
					if (storeItem.Item.HasValue)
					{
						list.Remove(storeItem.Item.Value);
					}
				}
				for (int i = 0; i < num3; i++)
				{
					if (list.Count == 0)
					{
						break;
					}
					int index = MyRandom.Instance.Next(0, list.Count);
					SerializableDefinitionId serializableDefinitionId = list[index];
					long id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
					MyPhysicalItemDefinition definition = null;
					int minimalPrice = 0;
					int num4 = 0;
					if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(serializableDefinitionId, out definition))
					{
						if (m_priceCalculator.TryGetItemMinimalPrice(serializableDefinitionId, out minimalPrice))
						{
							if (minimalPrice <= 0)
							{
								MyLog.Default.WriteToLogAndAssert("Wrong price for the item - " + definition.Id);
								continue;
							}
							num4 = MyRandom.Instance.Next(definition.MinimumOfferAmount, definition.MaximumOfferAmount);
							int pricePerUnit = (int)((float)minimalPrice * m_definition.OfferPriceStartingMultiplier * num2);
							byte updateCountStart = (byte)(firstGeneration ? ((byte)MyRandom.Instance.Next(0, m_definition.OfferMaxUpdateCount)) : 0);
							MyStoreItem item = new MyStoreItem(id, serializableDefinitionId, num4, pricePerUnit, StoreItemTypes.Offer, updateCountStart);
							station.StoreItems.Add(item);
							list.Remove(serializableDefinitionId);
						}
					}
					else
					{
						list.Remove(serializableDefinitionId);
					}
				}
			}
			if (!hasHydrogenOffer && m_definition.CanSellHydrogen)
			{
				int amount = MyRandom.Instance.Next(m_definition.MinimumOfferGasAmount, m_definition.MaximumOfferGasAmount);
				long id2 = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
				int pricePerUnit2 = (int)((float)m_definition.MinimumHydrogenPrice * m_definition.OfferPriceStartingMultiplier * num2);
				MyStoreItem item2 = new MyStoreItem(id2, amount, pricePerUnit2, StoreItemTypes.Offer, ItemTypes.Hydrogen);
				station.StoreItems.Add(item2);
			}
			if (!hasOxygenOffer && m_definition.CanSellOxygen)
			{
				int amount2 = MyRandom.Instance.Next(m_definition.MinimumOfferGasAmount, m_definition.MaximumOfferGasAmount);
				long id3 = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
				int pricePerUnit3 = (int)((float)m_definition.MinimumOxygenPrice * m_definition.OfferPriceStartingMultiplier * num2);
				MyStoreItem item3 = new MyStoreItem(id3, amount2, pricePerUnit3, StoreItemTypes.Offer, ItemTypes.Oxygen);
				station.StoreItems.Add(item3);
			}
		}

		private void GenerateOrders(MyStation station, int existingOrders, long availableBalance, bool firstGeneration)
		{
			int num = ((m_definition.OrdersList != null) ? m_definition.OrdersList.Length : 0);
			if (num == 0)
			{
				return;
			}
			int num2 = num;
			long num3 = availableBalance / num2;
			if (num2 <= existingOrders)
			{
				return;
			}
			num2 -= existingOrders;
			List<SerializableDefinitionId> list = new List<SerializableDefinitionId>(m_definition.OrdersList);
			foreach (MyStoreItem storeItem in station.StoreItems)
			{
				if (storeItem.Item.HasValue)
				{
					list.Remove(storeItem.Item.Value);
				}
			}
			for (int i = 0; i < num2; i++)
			{
				if (list.Count == 0)
				{
					break;
				}
				int index = MyRandom.Instance.Next(0, list.Count);
				SerializableDefinitionId serializableDefinitionId = list[index];
				MyPhysicalItemDefinition definition = null;
				int minimalPrice = 0;
				int num4 = 0;
				int num5 = 0;
				if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(serializableDefinitionId, out definition))
				{
					if (!m_priceCalculator.TryGetItemMinimalPrice(serializableDefinitionId, out minimalPrice))
					{
						continue;
					}
					float num6 = 1f;
					if (station.IsDeepSpaceStation)
					{
						MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
						if (component == null)
						{
							MyLog.Default.WriteToLogAndAssert("GenerateOffers - Economy session component not found.");
							break;
						}
						num6 = 1f + component.EconomyDefinition.DeepSpaceStationStoreBonus;
					}
					num5 = (int)((float)minimalPrice * m_definition.OrderPriceStartingMultiplier * num6);
					int num7 = 0;
					if (num5 > 0)
					{
						num7 = Math.Min((int)(num3 / num5), definition.MaximumOrderAmount);
					}
					if (definition.MinimumOrderAmount > num7)
					{
						list.Remove(serializableDefinitionId);
						continue;
					}
					num4 = MyRandom.Instance.Next(definition.MinimumOrderAmount, num7);
					MyStoreItem item = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), updateCountStart: (byte)(firstGeneration ? ((byte)MyRandom.Instance.Next(0, m_definition.OrderMaxUpdateCount)) : 0), itemId: serializableDefinitionId, amount: num4, pricePerUnit: num5, storeItemType: StoreItemTypes.Order);
					station.StoreItems.Add(item);
					list.Remove(serializableDefinitionId);
				}
				else
				{
					list.Remove(serializableDefinitionId);
				}
			}
		}
	}
}
