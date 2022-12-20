using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyStoreBlockViewModel : MyViewModelBase, IMyStoreBlockViewModel
	{
		private enum SortBy
		{
			Name,
			Amount,
			PricePerUnit
		}

		private bool m_isBuyEnabled;

		private bool m_isSellEnabled;

		private bool m_isRefreshEnabled;

		private bool m_isAdministrationVisible;

		private bool m_isPreviewEnabled;

		private bool m_isBuyTabVisible = true;

		private bool m_isSellTabVisible = true;

		private long m_lastEconomyTick;

		private float m_amountToSell;

		private float m_amountToBuy;

		private float m_amountToBuyMaximum;

		private int m_tabSelectedIndex;

		private string m_totalPriceToBuy;

		private string m_totalPriceToSell;

		private string m_sortModeOffersText;

		private string m_sortModeOrdersText;

		private SortBy m_sortModeOffers;

		private SortBy m_sortModeOrders;

		private ICommand m_refreshCommand;

		private ICommand m_sellCommand;

		private ICommand m_buyCommand;

		private ICommand m_sortingOfferedItemsCommand;

		private ICommand m_sortingDemandedItemsCommand;

		private ICommand m_onBuyItemDoubleClickCommand;

		private ICommand m_setAllAmountOfferCommand;

		private ICommand m_setAllAmountOrderCommand;

		private ICommand m_showPreviewCommand;

		private ICommand m_switchSortOffersCommand;

		private ICommand m_previousInventoryCommand;

		private ICommand m_nextInventoryCommand;

		private ICommand m_switchSortOrdersCommand;

		private ObservableCollection<MyStoreItemModel> m_offeredItems = new ObservableCollection<MyStoreItemModel>();

		private ObservableCollection<MyStoreItemModel> m_orderedItems = new ObservableCollection<MyStoreItemModel>();

		private MyStoreItemModel m_selectedOfferItem;

		private MyStoreItemModel m_selectedOrderItem;

		private MyStoreBlock m_storeBlock;

		public bool IsPreviewEnabled
		{
			get
			{
				return m_isPreviewEnabled;
			}
			set
			{
				SetProperty(ref m_isPreviewEnabled, value, "IsPreviewEnabled");
			}
		}

		public bool IsAdministrationVisible
		{
			get
			{
				return m_isAdministrationVisible;
			}
			set
			{
				SetProperty(ref m_isAdministrationVisible, value, "IsAdministrationVisible");
			}
		}

		public bool IsBuyTabVisible
		{
			get
			{
				return m_isBuyTabVisible;
			}
			set
			{
				SetProperty(ref m_isBuyTabVisible, value, "IsBuyTabVisible");
			}
		}

		public bool IsSellTabVisible
		{
			get
			{
				return m_isSellTabVisible;
			}
			set
			{
				SetProperty(ref m_isSellTabVisible, value, "IsSellTabVisible");
			}
		}

		public int TabSelectedIndex
		{
			get
			{
				return m_tabSelectedIndex;
			}
			set
			{
				if (m_tabSelectedIndex != value)
				{
					SetProperty(ref m_tabSelectedIndex, value, "TabSelectedIndex");
				}
			}
		}

		public bool IsBuyEnabled
		{
			get
			{
				return m_isBuyEnabled;
			}
			set
			{
				SetProperty(ref m_isBuyEnabled, value, "IsBuyEnabled");
			}
		}

		public bool IsSellEnabled
		{
			get
			{
				return m_isSellEnabled;
			}
			set
			{
				SetProperty(ref m_isSellEnabled, value, "IsSellEnabled");
			}
		}

		public bool IsRefreshEnabled
		{
			get
			{
				return m_isRefreshEnabled;
			}
			set
			{
				SetProperty(ref m_isRefreshEnabled, value, "IsRefreshEnabled");
			}
		}

		public MyStoreItemModel SelectedOfferItem
		{
			get
			{
				return m_selectedOfferItem;
			}
			set
			{
				SetProperty(ref m_selectedOfferItem, value, "SelectedOfferItem");
				UpdateOfferedInfo();
			}
		}

		public MyStoreItemModel SelectedOrderItem
		{
			get
			{
				return m_selectedOrderItem;
			}
			set
			{
				SetProperty(ref m_selectedOrderItem, value, "SelectedOrderItem");
				UpdateOrderedInfo();
			}
		}

		public string TotalPriceToSell
		{
			get
			{
				return m_totalPriceToSell;
			}
			set
			{
				SetProperty(ref m_totalPriceToSell, value, "TotalPriceToSell");
			}
		}

		public string TotalPriceToBuy
		{
			get
			{
				return m_totalPriceToBuy;
			}
			set
			{
				SetProperty(ref m_totalPriceToBuy, value, "TotalPriceToBuy");
			}
		}

		public float AmountToSell
		{
			get
			{
				return m_amountToSell;
			}
			set
			{
				if (SelectedOrderItem != null)
				{
					UpdateTotalPriceToSell(value);
				}
				SetProperty(ref m_amountToSell, value, "AmountToSell");
			}
		}

		public float AmountToBuyMaximum
		{
			get
			{
				return m_amountToBuyMaximum;
			}
			set
			{
				SetProperty(ref m_amountToBuyMaximum, value, "AmountToBuyMaximum");
			}
		}

		public float AmountToBuy
		{
			get
			{
				return m_amountToBuy;
			}
			set
			{
				if (SelectedOfferItem != null)
				{
					UpdateTotalPriceToBuy(value);
				}
				SetProperty(ref m_amountToBuy, value, "AmountToBuy");
			}
		}

		public ICommand SellCommand
		{
			get
			{
				return m_sellCommand;
			}
			set
			{
				SetProperty(ref m_sellCommand, value, "SellCommand");
			}
		}

		public ICommand BuyCommand
		{
			get
			{
				return m_buyCommand;
			}
			set
			{
				SetProperty(ref m_buyCommand, value, "BuyCommand");
			}
		}

		public ICommand SortingOfferedItemsCommand
		{
			get
			{
				return m_sortingOfferedItemsCommand;
			}
			set
			{
				SetProperty(ref m_sortingOfferedItemsCommand, value, "SortingOfferedItemsCommand");
			}
		}

		public ICommand SortingDemandedItemsCommand
		{
			get
			{
				return m_sortingDemandedItemsCommand;
			}
			set
			{
				SetProperty(ref m_sortingDemandedItemsCommand, value, "SortingDemandedItemsCommand");
			}
		}

		public ICommand RefreshCommand
		{
			get
			{
				return m_refreshCommand;
			}
			set
			{
				SetProperty(ref m_refreshCommand, value, "RefreshCommand");
			}
		}

		public ICommand OnBuyItemDoubleClickCommand
		{
			get
			{
				return m_onBuyItemDoubleClickCommand;
			}
			set
			{
				SetProperty(ref m_onBuyItemDoubleClickCommand, value, "OnBuyItemDoubleClickCommand");
			}
		}

		public ICommand SetAllAmountOfferCommand
		{
			get
			{
				return m_setAllAmountOfferCommand;
			}
			set
			{
				SetProperty(ref m_setAllAmountOfferCommand, value, "SetAllAmountOfferCommand");
			}
		}

		public ICommand SetAllAmountOrderCommand
		{
			get
			{
				return m_setAllAmountOrderCommand;
			}
			set
			{
				SetProperty(ref m_setAllAmountOrderCommand, value, "SetAllAmountOrderCommand");
			}
		}

		public ICommand ShowPreviewCommand
		{
			get
			{
				return m_showPreviewCommand;
			}
			set
			{
				SetProperty(ref m_showPreviewCommand, value, "ShowPreviewCommand");
			}
		}

		public ObservableCollection<MyStoreItemModel> OfferedItems
		{
			get
			{
				return m_offeredItems;
			}
			set
			{
				SetProperty(ref m_offeredItems, value, "OfferedItems");
			}
		}

		public ObservableCollection<MyStoreItemModel> OrderedItems
		{
			get
			{
				return m_orderedItems;
			}
			set
			{
				SetProperty(ref m_orderedItems, value, "OrderedItems");
			}
		}

		public MyInventoryTargetViewModel InventoryTargetViewModel { get; private set; }

		public MyStoreBlockAdministrationViewModel AdministrationViewModel { get; private set; }

		public string SortModeOffersText
<<<<<<< HEAD
		{
			get
			{
				return m_sortModeOffersText;
			}
			set
			{
				SetProperty(ref m_sortModeOffersText, value, "SortModeOffersText");
			}
		}

		public ICommand SwitchSortOffersCommand
		{
			get
			{
				return m_switchSortOffersCommand;
			}
			set
			{
				SetProperty(ref m_switchSortOffersCommand, value, "SwitchSortOffersCommand");
			}
		}

		public ICommand NextInventoryCommand
		{
			get
			{
				return m_nextInventoryCommand;
			}
			set
			{
				SetProperty(ref m_nextInventoryCommand, value, "NextInventoryCommand");
			}
		}

		public ICommand PreviousInventoryCommand
		{
			get
			{
				return m_previousInventoryCommand;
			}
			set
			{
				SetProperty(ref m_previousInventoryCommand, value, "PreviousInventoryCommand");
			}
		}

		public ICommand SwitchSortOrdersCommand
		{
			get
			{
				return m_switchSortOrdersCommand;
			}
			set
			{
				SetProperty(ref m_switchSortOrdersCommand, value, "SwitchSortOrdersCommand");
			}
		}

		public string SortModeOrdersText
		{
			get
			{
=======
		{
			get
			{
				return m_sortModeOffersText;
			}
			set
			{
				SetProperty(ref m_sortModeOffersText, value, "SortModeOffersText");
			}
		}

		public ICommand SwitchSortOffersCommand
		{
			get
			{
				return m_switchSortOffersCommand;
			}
			set
			{
				SetProperty(ref m_switchSortOffersCommand, value, "SwitchSortOffersCommand");
			}
		}

		public ICommand NextInventoryCommand
		{
			get
			{
				return m_nextInventoryCommand;
			}
			set
			{
				SetProperty(ref m_nextInventoryCommand, value, "NextInventoryCommand");
			}
		}

		public ICommand PreviousInventoryCommand
		{
			get
			{
				return m_previousInventoryCommand;
			}
			set
			{
				SetProperty(ref m_previousInventoryCommand, value, "PreviousInventoryCommand");
			}
		}

		public ICommand SwitchSortOrdersCommand
		{
			get
			{
				return m_switchSortOrdersCommand;
			}
			set
			{
				SetProperty(ref m_switchSortOrdersCommand, value, "SwitchSortOrdersCommand");
			}
		}

		public string SortModeOrdersText
		{
			get
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return m_sortModeOrdersText;
			}
			set
			{
				SetProperty(ref m_sortModeOrdersText, value, "SortModeOrdersText");
			}
		}

		public MyStoreBlockViewModel(MyStoreBlock storeBlock)
		{
			BuyCommand = new RelayCommand(OnBuy);
			SellCommand = new RelayCommand(OnSell);
			RefreshCommand = new RelayCommand(OnRefresh);
			OnBuyItemDoubleClickCommand = new RelayCommand(OnBuyItemDoubleClick);
			SetAllAmountOfferCommand = new RelayCommand(OnSetAllAmountOffer);
			SetAllAmountOrderCommand = new RelayCommand(OnSetAllAmountOrder);
			ShowPreviewCommand = new RelayCommand(OnShowPreview);
			NextInventoryCommand = new RelayCommand(OnNextInventory);
			PreviousInventoryCommand = new RelayCommand(OnPreviousInventory);
			SortingOfferedItemsCommand = new RelayCommand<DataGridSortingEventArgs>(OnSortingOfferedItems);
			SwitchSortOffersCommand = new RelayCommand(OnSwitchSortOffers);
			SortingDemandedItemsCommand = new RelayCommand<DataGridSortingEventArgs>(OnSortingDemandedItems);
			SwitchSortOrdersCommand = new RelayCommand(OnSwitchSortOrders);
			m_storeBlock = storeBlock;
			InventoryTargetViewModel = new MyInventoryTargetViewModel(storeBlock);
			IsAdministrationVisible = m_storeBlock.HasLocalPlayerAccess() && m_storeBlock.OwnerId == MySession.Static.LocalPlayerId;
			AdministrationViewModel = new MyStoreBlockAdministrationViewModel(m_storeBlock);
			if (IsAdministrationVisible)
			{
				AdministrationViewModel.Initialize();
				AdministrationViewModel.OfferCreated += AdministrationViewModel_Changed;
				AdministrationViewModel.OrderCreated += AdministrationViewModel_Changed;
				AdministrationViewModel.StoreItemRemoved += AdministrationViewModel_Changed;
			}
			TotalPriceToBuy = "0";
			TotalPriceToSell = "0";
			SortModeOffersText = MyTexts.GetString("StoreBlock_Column_" + m_sortModeOffers);
			SortModeOrdersText = MyTexts.GetString("StoreBlock_Column_" + m_sortModeOrders);
			ServiceManager.Instance.AddService((IMyStoreBlockViewModel)this);
		}

		private void OnPreviousInventory(object obj)
		{
			InventoryTargetViewModel.ShowPreviousInventory();
		}

		private void OnNextInventory(object obj)
		{
			InventoryTargetViewModel.ShowNextInventory();
		}

		public override void InitializeData()
		{
			m_storeBlock.CreateGetConnectedGridInventoriesRequest(OnGetInventories);
			RefreshStoreItems();
			if (!IsBuyTabVisible && TabSelectedIndex == 0 && IsAdministrationVisible)
			{
				TabSelectedIndex = 2;
			}
		}

		private void OnShowPreview(object obj)
		{
			if (SelectedOfferItem != null)
			{
				m_storeBlock.ShowPreview(SelectedOfferItem.Id);
			}
		}

		private void OnSetAllAmountOrder(object obj)
		{
			if (SelectedOrderItem != null)
			{
				float itemAmount = InventoryTargetViewModel.GetItemAmount(SelectedOrderItem.ItemDefinitionId);
				if (itemAmount != 0f)
				{
					AmountToSell = Math.Min(SelectedOrderItem.Amount, itemAmount);
				}
			}
		}

		private void OnSetAllAmountOffer(object obj)
		{
			AmountToBuy = AmountToBuyMaximum;
		}

		private void OnBuyItemDoubleClick(object obj)
		{
			if (IsBuyEnabled)
			{
				OnBuy(obj);
			}
		}

		private void OnGetInventories(List<long> inventoryEntities)
		{
			InventoryTargetViewModel.Initialize(inventoryEntities, includeCharacterInventory: true, showAllOption: false);
		}

		private void UpdateLocalPlayerAccountBalance()
		{
			InventoryTargetViewModel.UpdateLocalPlayerCurrency();
		}

		private void OnRefresh(object obj)
		{
			RefreshStoreItems();
		}

		private void RefreshStoreItems()
		{
			IsRefreshEnabled = false;
			m_storeBlock.CreateGetStoreItemsRequest(MySession.Static.LocalPlayerId, OnGetStoreItems);
		}

		private void OnGetStoreItems(List<MyStoreItem> storeItems, long lastEconomyTick, float offersBonus, float ordersBonus)
		{
			m_lastEconomyTick = lastEconomyTick;
			IsRefreshEnabled = true;
<<<<<<< HEAD
			ObservableCollection<MyStoreItemModel> observableCollection = new ObservableCollection<MyStoreItemModel>();
			ObservableCollection<MyStoreItemModel> observableCollection2 = new ObservableCollection<MyStoreItemModel>();
			ObservableCollection<MyStoreItemModel> observableCollection3 = new ObservableCollection<MyStoreItemModel>();
=======
			ObservableCollection<MyStoreItemModel> val = new ObservableCollection<MyStoreItemModel>();
			ObservableCollection<MyStoreItemModel> val2 = new ObservableCollection<MyStoreItemModel>();
			ObservableCollection<MyStoreItemModel> val3 = new ObservableCollection<MyStoreItemModel>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyStoreItemModel selectedOfferItem = null;
			MyStoreItemModel selectedOrderItem = null;
			foreach (MyStoreItem storeItem in storeItems)
			{
				if (storeItem.Amount == 0)
<<<<<<< HEAD
				{
					continue;
				}
				MyStoreItemModel myStoreItemModel = new MyStoreItemModel
				{
=======
				{
					continue;
				}
				MyStoreItemModel myStoreItemModel = new MyStoreItemModel
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Id = storeItem.Id,
					Amount = storeItem.Amount,
					ItemType = storeItem.ItemType,
					StoreItemType = storeItem.StoreItemType
				};
				switch (storeItem.ItemType)
				{
				case ItemTypes.PhysicalItem:
				{
					MyPhysicalItemDefinition definition = null;
					if (!MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(storeItem.Item.Value, out definition))
					{
						continue;
					}
					myStoreItemModel.Name = definition.DisplayNameText;
					myStoreItemModel.Description = definition.DescriptionText;
					string[] icons2 = definition.Icons;
					if (icons2 != null && icons2.Length != 0)
					{
						myStoreItemModel.SetIcon(definition.Icons[0]);
					}
					myStoreItemModel.IsOre = definition.IsOre;
					myStoreItemModel.ItemDefinitionId = storeItem.Item.Value;
					break;
				}
				case ItemTypes.Oxygen:
					myStoreItemModel.Name = MyTexts.GetString(MySpaceTexts.DisplayName_Item_Oxygen);
					myStoreItemModel.SetIcon("Textures\\GUI\\Icons\\OxygenIcon.dds");
					break;
				case ItemTypes.Hydrogen:
					myStoreItemModel.Name = MyTexts.GetString(MySpaceTexts.DisplayName_Item_Hydrogen);
					myStoreItemModel.SetIcon("Textures\\GUI\\Icons\\HydrogenIcon.dds");
					break;
				case ItemTypes.Grid:
				{
					MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(storeItem.PrefabName);
					if (prefabDefinition != null)
					{
						if (!string.IsNullOrEmpty(prefabDefinition.DisplayNameString))
						{
							myStoreItemModel.Name = prefabDefinition.DisplayNameString;
							string[] icons = prefabDefinition.Icons;
							string icon = ((icons != null && icons.Length != 0) ? prefabDefinition.Icons[0] : string.Empty);
							myStoreItemModel.SetIcon(icon);
							myStoreItemModel.Description = prefabDefinition.DescriptionString;
							myStoreItemModel.HasTooltip = true;
							string tooltipImage = prefabDefinition.TooltipImage;
							myStoreItemModel.SetTooltipImage(tooltipImage);
						}
						else
						{
							MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
							myStoreItemModel.Name = ((cubeGrids != null && cubeGrids.Length != 0) ? prefabDefinition.CubeGrids[0].DisplayName : string.Empty);
						}
					}
					myStoreItemModel.PrefabTotalPcu = storeItem.PrefabTotalPcu;
					break;
				}
				}
				switch (storeItem.StoreItemType)
				{
				case StoreItemTypes.Offer:
					myStoreItemModel.PricePerUnit = (int)((float)storeItem.PricePerUnit * offersBonus);
					if (offersBonus < 1f)
					{
						float num = (float)storeItem.PricePerUnit * (1f + storeItem.PricePerUnitDiscount);
						float num2 = ((num > (float)myStoreItemModel.PricePerUnit) ? (1f - (float)myStoreItemModel.PricePerUnit / num) : 0f);
						myStoreItemModel.PricePerUnitDiscount = (float)Math.Round(num2 * 100f, 1);
					}
					else
					{
						myStoreItemModel.PricePerUnitDiscount = (float)Math.Round(storeItem.PricePerUnitDiscount * 100f, 1);
					}
<<<<<<< HEAD
					observableCollection.Add(myStoreItemModel);
=======
					((Collection<MyStoreItemModel>)(object)val).Add(myStoreItemModel);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (SelectedOfferItem != null && SelectedOfferItem.Id == storeItem.Id)
					{
						selectedOfferItem = myStoreItemModel;
					}
					break;
				case StoreItemTypes.Order:
					myStoreItemModel.PricePerUnit = (int)((float)storeItem.PricePerUnit * ordersBonus);
<<<<<<< HEAD
					observableCollection2.Add(myStoreItemModel);
=======
					((Collection<MyStoreItemModel>)(object)val2).Add(myStoreItemModel);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (SelectedOrderItem != null && SelectedOrderItem.Id == storeItem.Id)
					{
						selectedOrderItem = myStoreItemModel;
					}
					break;
				}
				myStoreItemModel.TotalPrice = storeItem.Amount * myStoreItemModel.PricePerUnit;
<<<<<<< HEAD
				observableCollection3.Add(myStoreItemModel);
			}
			SortOffers(observableCollection);
			SortOrders(observableCollection2);
=======
				((Collection<MyStoreItemModel>)(object)val3).Add(myStoreItemModel);
			}
			SortOffers(val);
			SortOrders(val2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (IsAdministrationVisible)
			{
				AdministrationViewModel.OfferCount = ((Collection<MyStoreItemModel>)(object)val).Count;
				AdministrationViewModel.OrderCount = ((Collection<MyStoreItemModel>)(object)val2).Count;
				AdministrationViewModel.StoreItems = val3;
			}
			SelectedOfferItem = selectedOfferItem;
			SelectedOrderItem = selectedOrderItem;
		}

		private void UpdateOfferedInfo()
		{
			if (SelectedOfferItem == null)
			{
				AmountToBuy = 0f;
				AmountToBuyMaximum = 1f;
				IsBuyEnabled = false;
				IsPreviewEnabled = false;
				return;
			}
			AmountToBuyMaximum = SelectedOfferItem.Amount;
			if (AmountToBuy == 0f)
			{
				AmountToBuy = 1f;
			}
			if (AmountToBuy > (float)SelectedOfferItem.Amount)
			{
				AmountToBuy = SelectedOfferItem.Amount;
			}
			else
			{
				UpdateTotalPriceToBuy(AmountToBuy);
			}
			IsPreviewEnabled = SelectedOfferItem.ItemType == ItemTypes.Grid;
		}

		private void UpdateOrderedInfo()
		{
			if (SelectedOrderItem == null)
			{
				AmountToSell = 0f;
				IsSellEnabled = false;
				return;
			}
			if (AmountToSell == 0f)
			{
				AmountToSell = 1f;
			}
			if (AmountToSell > (float)SelectedOrderItem.Amount)
			{
				AmountToSell = SelectedOrderItem.Amount;
			}
			else
			{
				UpdateTotalPriceToSell(AmountToSell);
			}
			IsSellEnabled = true;
		}

		private void UpdateTotalPriceToBuy(float amount)
		{
			long num = 0L;
			if (!float.IsNaN(amount))
			{
				num = (int)amount * SelectedOfferItem.PricePerUnit;
			}
			TotalPriceToBuy = MyBankingSystem.GetFormatedValue(num);
			IsBuyEnabled = InventoryTargetViewModel.LocalPlayerAccountInfo.Balance >= num && amount != 0f && !float.IsNaN(amount);
		}

		private void UpdateTotalPriceToSell(float amount)
		{
			long valueToFormat = 0L;
			if (!float.IsNaN(amount))
			{
				valueToFormat = (int)amount * SelectedOrderItem.PricePerUnit;
			}
			TotalPriceToSell = MyBankingSystem.GetFormatedValue(valueToFormat);
			IsSellEnabled = (float)SelectedOrderItem.Amount >= amount && amount != 0f && !float.IsNaN(amount);
		}

		private void OnSell(object obj)
		{
			if (IsSellEnabled && SelectedOrderItem != null)
			{
				int num = (int)AmountToSell;
<<<<<<< HEAD
				if (num <= SelectedOrderItem.Amount && !float.IsNaN(num) && InventoryTargetViewModel.SelectedInventoryIndex >= 0 && InventoryTargetViewModel.SelectedInventoryIndex < InventoryTargetViewModel.Inventories.Count)
				{
					IsSellEnabled = false;
					long entityId = InventoryTargetViewModel.Inventories[InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
=======
				if (num <= SelectedOrderItem.Amount && !float.IsNaN(num) && InventoryTargetViewModel.SelectedInventoryIndex >= 0 && InventoryTargetViewModel.SelectedInventoryIndex < ((Collection<MyInventoryTargetModel>)(object)InventoryTargetViewModel.Inventories).Count)
				{
					IsSellEnabled = false;
					long entityId = ((Collection<MyInventoryTargetModel>)(object)InventoryTargetViewModel.Inventories)[InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_storeBlock.CreateSellItemRequest(SelectedOrderItem.Id, num, entityId, m_lastEconomyTick, OnSellResult);
				}
			}
		}

		private void OnSellResult(MyStoreSellItemResult result)
		{
			switch (result.Result)
			{
			case MyStoreSellItemResults.ItemNotFound:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemNotFound));
				break;
			case MyStoreSellItemResults.WrongAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_WrongAmount));
				break;
			case MyStoreSellItemResults.ItemsTimeout:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemsTimeout), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemsTimeout));
				break;
			case MyStoreSellItemResults.NotEnoughAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughAmount), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughAmount));
				break;
			case MyStoreSellItemResults.NotEnoughMoney:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughMoney));
				break;
			case MyStoreSellItemResults.NotEnoughInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughInventorySpace));
				break;
			}
			RefreshStoreItems();
			UpdateLocalPlayerAccountBalance();
		}

		private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void OnBuy(object obj)
		{
			if (IsBuyEnabled && SelectedOfferItem != null)
			{
				int num = (int)AmountToBuy;
<<<<<<< HEAD
				if (num <= SelectedOfferItem.Amount && !float.IsNaN(num) && num * SelectedOfferItem.PricePerUnit <= InventoryTargetViewModel.LocalPlayerAccountInfo.Balance && InventoryTargetViewModel.SelectedInventoryIndex >= 0 && InventoryTargetViewModel.SelectedInventoryIndex < InventoryTargetViewModel.Inventories.Count)
				{
					IsBuyEnabled = false;
					long entityId = InventoryTargetViewModel.Inventories[InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
=======
				if (num <= SelectedOfferItem.Amount && !float.IsNaN(num) && num * SelectedOfferItem.PricePerUnit <= InventoryTargetViewModel.LocalPlayerAccountInfo.Balance && InventoryTargetViewModel.SelectedInventoryIndex >= 0 && InventoryTargetViewModel.SelectedInventoryIndex < ((Collection<MyInventoryTargetModel>)(object)InventoryTargetViewModel.Inventories).Count)
				{
					IsBuyEnabled = false;
					long entityId = ((Collection<MyInventoryTargetModel>)(object)InventoryTargetViewModel.Inventories)[InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_storeBlock.CreateBuyRequest(SelectedOfferItem.Id, num, entityId, m_lastEconomyTick, OnBuyResult);
				}
			}
		}

		private void OnBuyResult(MyStoreBuyItemResult result)
		{
			switch (result.Result)
			{
			case MyStoreBuyItemResults.ItemNotFound:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemNotFound));
				break;
			case MyStoreBuyItemResults.WrongAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongAmount));
				break;
			case MyStoreBuyItemResults.NotEnoughMoney:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughMoney));
				break;
			case MyStoreBuyItemResults.ItemsTimeout:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemsTimeout), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemsTimeout));
				break;
			case MyStoreBuyItemResults.NotEnoughInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughInventorySpace));
				break;
			case MyStoreBuyItemResults.WrongInventory:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongInventory), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongInventory));
				break;
			case MyStoreBuyItemResults.SpawnFailed:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_SpawnFailed), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_SpawnFailed));
				break;
			case MyStoreBuyItemResults.FreePositionNotFound:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_FreePositionNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_FreePositionNotFound));
				break;
			case MyStoreBuyItemResults.NotEnoughStoreBlockInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughStoreBlockInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughStoreBlockInventorySpace));
				break;
			case MyStoreBuyItemResults.NotEnoughAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughAmount));
				break;
			case MyStoreBuyItemResults.NotEnoughPCU:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughPCU), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughPCU));
				break;
			case MyStoreBuyItemResults.NotEnoughSpaceInTank:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughSpaceInTank), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughSpaceInTank));
				break;
			}
			RefreshStoreItems();
			UpdateLocalPlayerAccountBalance();
		}

		private void OnSortingDemandedItems(DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyStoreItemModel> enumerable = SortStoreItems(OrderedItems, sortingArgs);
			OrderedItems = new ObservableCollection<MyStoreItemModel>(enumerable);
		}

		private void OnSortingOfferedItems(DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyStoreItemModel> enumerable = SortStoreItems(OfferedItems, sortingArgs);
			OfferedItems = new ObservableCollection<MyStoreItemModel>(enumerable);
		}

		private void OnSwitchSortOffers(object obj)
		{
			m_sortModeOffers++;
			if (m_sortModeOffers > SortBy.PricePerUnit)
			{
				m_sortModeOffers = SortBy.Name;
			}
			SortOffers(OfferedItems);
		}

		private void SortOffers(ObservableCollection<MyStoreItemModel> toSort)
		{
			IEnumerable<MyStoreItemModel> enumerable = Sort(toSort, m_sortModeOffers.ToString(), ListSortDirection.Ascending);
			OfferedItems = new ObservableCollection<MyStoreItemModel>(enumerable);
			SortModeOffersText = MyTexts.GetString("StoreBlock_Column_" + m_sortModeOffers);
		}

		private void OnSwitchSortOrders(object obj)
		{
			m_sortModeOrders++;
			if (m_sortModeOrders > SortBy.PricePerUnit)
			{
				m_sortModeOrders = SortBy.Name;
			}
			SortOrders(OrderedItems);
		}

		private void SortOrders(ObservableCollection<MyStoreItemModel> toSort)
		{
			IEnumerable<MyStoreItemModel> enumerable = Sort(toSort, m_sortModeOrders.ToString(), ListSortDirection.Ascending);
			OrderedItems = new ObservableCollection<MyStoreItemModel>(enumerable);
			SortModeOrdersText = MyTexts.GetString("StoreBlock_Column_" + m_sortModeOrders);
		}

		private void OnSwitchSortOffers(object obj)
		{
			m_sortModeOffers++;
			if (m_sortModeOffers > SortBy.PricePerUnit)
			{
				m_sortModeOffers = SortBy.Name;
			}
			SortOffers(OfferedItems);
		}

		private void SortOffers(ObservableCollection<MyStoreItemModel> toSort)
		{
			IEnumerable<MyStoreItemModel> collection = Sort(toSort, m_sortModeOffers.ToString(), ListSortDirection.Ascending);
			OfferedItems = new ObservableCollection<MyStoreItemModel>(collection);
			SortModeOffersText = MyTexts.GetString("StoreBlock_Column_" + m_sortModeOffers);
		}

		private void OnSwitchSortOrders(object obj)
		{
			m_sortModeOrders++;
			if (m_sortModeOrders > SortBy.PricePerUnit)
			{
				m_sortModeOrders = SortBy.Name;
			}
			SortOrders(OrderedItems);
		}

		private void SortOrders(ObservableCollection<MyStoreItemModel> toSort)
		{
			IEnumerable<MyStoreItemModel> collection = Sort(toSort, m_sortModeOrders.ToString(), ListSortDirection.Ascending);
			OrderedItems = new ObservableCollection<MyStoreItemModel>(collection);
			SortModeOrdersText = MyTexts.GetString("StoreBlock_Column_" + m_sortModeOrders);
		}

		private IEnumerable<MyStoreItemModel> SortStoreItems(ObservableCollection<MyStoreItemModel> toSort, DataGridSortingEventArgs sortingArgs)
		{
			DataGridColumn column = sortingArgs.Column;
			ListSortDirection? sortDirection = column.SortDirection;
			if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
			{
				column.SortDirection = ListSortDirection.Ascending;
			}
			else
			{
				column.SortDirection = ListSortDirection.Descending;
			}
			return Sort(toSort, column.SortMemberPath, column.SortDirection);
		}

		private static IEnumerable<MyStoreItemModel> Sort(ObservableCollection<MyStoreItemModel> toSort, string column, ListSortDirection? sortOrder)
		{
			IEnumerable<MyStoreItemModel> result = null;
			switch (column)
			{
			case "Name":
<<<<<<< HEAD
				result = ((sortOrder.HasValue && sortOrder != ListSortDirection.Ascending) ? toSort.OrderByDescending((MyStoreItemModel u) => u.Name) : toSort.OrderBy((MyStoreItemModel u) => u.Name));
				break;
			case "Amount":
				result = ((sortOrder.HasValue && sortOrder != ListSortDirection.Ascending) ? toSort.OrderByDescending((MyStoreItemModel u) => u.Amount) : toSort.OrderBy((MyStoreItemModel u) => u.Amount));
				break;
			case "PricePerUnit":
				result = ((sortOrder.HasValue && sortOrder != ListSortDirection.Ascending) ? toSort.OrderByDescending((MyStoreItemModel u) => u.PricePerUnit) : toSort.OrderBy((MyStoreItemModel u) => u.PricePerUnit));
=======
				result = (IEnumerable<MyStoreItemModel>)((sortOrder.HasValue && sortOrder != ListSortDirection.Ascending) ? Enumerable.OrderByDescending<MyStoreItemModel, string>((IEnumerable<MyStoreItemModel>)toSort, (Func<MyStoreItemModel, string>)((MyStoreItemModel u) => u.Name)) : Enumerable.OrderBy<MyStoreItemModel, string>((IEnumerable<MyStoreItemModel>)toSort, (Func<MyStoreItemModel, string>)((MyStoreItemModel u) => u.Name)));
				break;
			case "Amount":
				result = (IEnumerable<MyStoreItemModel>)((sortOrder.HasValue && sortOrder != ListSortDirection.Ascending) ? Enumerable.OrderByDescending<MyStoreItemModel, int>((IEnumerable<MyStoreItemModel>)toSort, (Func<MyStoreItemModel, int>)((MyStoreItemModel u) => u.Amount)) : Enumerable.OrderBy<MyStoreItemModel, int>((IEnumerable<MyStoreItemModel>)toSort, (Func<MyStoreItemModel, int>)((MyStoreItemModel u) => u.Amount)));
				break;
			case "PricePerUnit":
				result = (IEnumerable<MyStoreItemModel>)((sortOrder.HasValue && sortOrder != ListSortDirection.Ascending) ? Enumerable.OrderByDescending<MyStoreItemModel, int>((IEnumerable<MyStoreItemModel>)toSort, (Func<MyStoreItemModel, int>)((MyStoreItemModel u) => u.PricePerUnit)) : Enumerable.OrderBy<MyStoreItemModel, int>((IEnumerable<MyStoreItemModel>)toSort, (Func<MyStoreItemModel, int>)((MyStoreItemModel u) => u.PricePerUnit)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			return result;
		}

		private void AdministrationViewModel_Changed(object sender, EventArgs e)
		{
			RefreshStoreItems();
			UpdateLocalPlayerAccountBalance();
		}

		public override void OnScreenClosing()
		{
			AdministrationViewModel.OfferCreated -= AdministrationViewModel_Changed;
			AdministrationViewModel.OrderCreated -= AdministrationViewModel_Changed;
			AdministrationViewModel.StoreItemRemoved -= AdministrationViewModel_Changed;
			InventoryTargetViewModel.UnsubscribeEvents();
			ServiceManager.Instance.RemoveService<IMyStoreBlockViewModel>();
			base.OnScreenClosing();
		}
	}
}
