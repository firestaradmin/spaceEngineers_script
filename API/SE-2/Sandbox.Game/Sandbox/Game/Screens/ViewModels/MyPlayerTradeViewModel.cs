using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GameSystems.Trading;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components.Trading;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
	[StaticEventOwner]
	public class MyPlayerTradeViewModel : MyViewModelBase, IMyPlayerTradeViewModel
	{
		private bool m_isAcceptEnabled;

		private bool m_isAcceptVisible;

		private bool m_isSubmitOfferEnabled;

		private bool m_isPcuVisible;

		private bool m_isCancelVisible;

		private MyAccountInfo m_localPlayerAccountInfo;

		private int m_localAvailablePCU;

		private string m_localPlayerCurrency;

		private string m_localPlayerPcu;

		private float m_localPlayerOfferCurrency;

		private float m_localPlayerOfferCurrencyMaximum;

		private float m_localPlayerOfferPcu;

		private string m_otherPlayerOfferCurrency;

		private string m_otherPlayerAcceptState;

		private string m_otherPlayerOfferPcu;

		private string m_submitOfferLabel;

		private string m_acceptTradeLabel;

		private BitmapImage m_currencyIcon;

		private ICommand m_acceptCommand;

		private ICommand m_cancelCommand;

		private ICommand m_submitOfferCommand;

		private ICommand m_removeItemFromOfferCommand;

		private ICommand m_addItemToOfferCommand;

		private ICommand m_addStackTenToOfferCommand;

		private ICommand m_addStackHundredToOfferCommand;

		private ICommand m_removeStackTenToOfferCommand;

		private ICommand m_removeStackHundredToOfferCommand;

		private ICommand m_submitAcceptCommand;

		private ICommand m_addItemCommand;

		private ICommand m_removeItemCommand;

		private MyInventoryItemModel m_selectedInventoryItem;

		private MyInventoryItemModel m_selectedOfferItem;

		private ObservableCollection<MyInventoryItemModel> m_myInventoryItems = new ObservableCollection<MyInventoryItemModel>();

		private ObservableCollection<MyInventoryItemModel> m_localPlayerOfferItems = new ObservableCollection<MyInventoryItemModel>();

		private ObservableCollection<MyInventoryItemModel> m_otherPlayerOfferItems = new ObservableCollection<MyInventoryItemModel>();

		private ulong m_otherPlayerId;

		private bool m_waitingToAcceptTrade;

		private MyInventoryItemModel m_draggedItem;

		private bool m_isLeftToRight;

		private bool m_triggerCancelEvent = true;

		public bool TriggerCancelEvent => m_triggerCancelEvent;

		public BitmapImage CurrencyIcon
		{
			get
			{
				return m_currencyIcon;
			}
			set
			{
				SetProperty(ref m_currencyIcon, value, "CurrencyIcon");
			}
		}

		public string OtherPlayerOfferPcu
		{
			get
			{
				return m_otherPlayerOfferPcu;
			}
			set
			{
				SetProperty(ref m_otherPlayerOfferPcu, value, "OtherPlayerOfferPcu");
			}
		}

		public string OtherPlayerOfferCurrency
		{
			get
			{
				return m_otherPlayerOfferCurrency;
			}
			set
			{
				SetProperty(ref m_otherPlayerOfferCurrency, value, "OtherPlayerOfferCurrency");
			}
		}

		public string OtherPlayerAcceptState
		{
			get
			{
				return m_otherPlayerAcceptState;
			}
			set
			{
				SetProperty(ref m_otherPlayerAcceptState, value, "OtherPlayerAcceptState");
			}
		}

		public float LocalPlayerOfferPcu
		{
			get
			{
				return m_localPlayerOfferPcu;
			}
			set
			{
				if (m_localPlayerOfferPcu != value)
				{
					OnLocalPlayerOfferPCUChanged(value);
				}
				SetProperty(ref m_localPlayerOfferPcu, value, "LocalPlayerOfferPcu");
			}
		}

		public float LocalPlayerOfferCurrency
		{
			get
			{
				return m_localPlayerOfferCurrency;
			}
			set
			{
				float num = (float.IsNaN(value) ? 0f : value);
				if (m_localPlayerOfferCurrency != num)
				{
					OnLocalPlayerOfferCurrencyChanged(num);
				}
				SetProperty(ref m_localPlayerOfferCurrency, num, "LocalPlayerOfferCurrency");
			}
		}

		public float LocalPlayerOfferCurrencyMaximum
		{
			get
			{
				return m_localPlayerOfferCurrencyMaximum;
			}
			set
			{
				SetProperty(ref m_localPlayerOfferCurrencyMaximum, value, "LocalPlayerOfferCurrencyMaximum");
			}
		}

		public string LocalPlayerPcu
		{
			get
			{
				return m_localPlayerPcu;
			}
			set
			{
				SetProperty(ref m_localPlayerPcu, value, "LocalPlayerPcu");
			}
		}

		public string LocalPlayerCurrency
		{
			get
			{
				return m_localPlayerCurrency;
			}
			set
			{
				SetProperty(ref m_localPlayerCurrency, value, "LocalPlayerCurrency");
			}
		}

		public bool IsAcceptEnabled
		{
			get
			{
				return m_isAcceptEnabled;
			}
			set
			{
				SetProperty(ref m_isAcceptEnabled, value, "IsAcceptEnabled");
			}
		}

		public bool IsAcceptVisible
		{
			get
			{
				return m_isAcceptVisible;
			}
			set
			{
				SetProperty(ref m_isAcceptVisible, value, "IsAcceptVisible");
			}
		}

		public bool IsSubmitOfferEnabled
		{
			get
			{
				return m_isSubmitOfferEnabled;
			}
			set
			{
				SetProperty(ref m_isSubmitOfferEnabled, value, "IsSubmitOfferEnabled");
			}
		}

		public bool IsPcuVisible
		{
			get
			{
				return m_isPcuVisible;
			}
			set
			{
				SetProperty(ref m_isPcuVisible, value, "IsPcuVisible");
			}
		}

		public bool IsCancelVisible
		{
			get
			{
				return m_isCancelVisible;
			}
			set
			{
				SetProperty(ref m_isCancelVisible, value, "IsCancelVisible");
			}
		}

		public string SubmitOfferLabel
		{
			get
			{
				return m_submitOfferLabel;
			}
			set
			{
				SetProperty(ref m_submitOfferLabel, value, "SubmitOfferLabel");
			}
		}

		public string AcceptTradeLabel
		{
			get
			{
				return m_acceptTradeLabel;
			}
			set
			{
				SetProperty(ref m_acceptTradeLabel, value, "AcceptTradeLabel");
			}
		}

		public ICommand AcceptCommand
		{
			get
			{
				return m_acceptCommand;
			}
			set
			{
				SetProperty(ref m_acceptCommand, value, "AcceptCommand");
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				return m_cancelCommand;
			}
			set
			{
				SetProperty(ref m_cancelCommand, value, "CancelCommand");
			}
		}

		public ICommand SubmitOfferCommand
		{
			get
			{
				return m_submitOfferCommand;
			}
			set
			{
				SetProperty(ref m_submitOfferCommand, value, "SubmitOfferCommand");
			}
		}

		public ICommand RemoveItemFromOfferCommand
		{
			get
			{
				return m_removeItemFromOfferCommand;
			}
			set
			{
				SetProperty(ref m_removeItemFromOfferCommand, value, "RemoveItemFromOfferCommand");
			}
		}

		public ICommand AddItemToOfferCommand
		{
			get
			{
				return m_addItemToOfferCommand;
			}
			set
			{
				SetProperty(ref m_addItemToOfferCommand, value, "AddItemToOfferCommand");
			}
		}

		public ICommand AddStackTenToOfferCommand
		{
			get
			{
				return m_addStackTenToOfferCommand;
			}
			set
			{
				SetProperty(ref m_addStackTenToOfferCommand, value, "AddStackTenToOfferCommand");
			}
		}

		public ICommand AddStackHundredToOfferCommand
		{
			get
			{
				return m_addStackHundredToOfferCommand;
			}
			set
			{
				SetProperty(ref m_addStackHundredToOfferCommand, value, "AddStackHundredToOfferCommand");
			}
		}

		public ICommand RemoveStackTenToOfferCommand
		{
			get
			{
				return m_addStackTenToOfferCommand;
			}
			set
			{
				SetProperty(ref m_removeStackTenToOfferCommand, value, "RemoveStackTenToOfferCommand");
			}
		}

		public ICommand RemoveStackHundredToOfferCommand
		{
			get
			{
				return m_addStackHundredToOfferCommand;
			}
			set
			{
				SetProperty(ref m_removeStackHundredToOfferCommand, value, "RemoveStackHundredToOfferCommand");
			}
		}

		public ICommand SubmitAcceptCommand
		{
			get
			{
				return m_submitAcceptCommand;
			}
			set
			{
				SetProperty(ref m_submitAcceptCommand, value, "SubmitAcceptCommand");
			}
		}

		public ICommand AddItemCommand
		{
			get
			{
				return m_addItemCommand;
			}
			set
			{
				SetProperty(ref m_addItemCommand, value, "AddItemCommand");
			}
		}

		public ICommand RemoveItemCommand
		{
			get
			{
				return m_removeItemCommand;
			}
			set
			{
				SetProperty(ref m_removeItemCommand, value, "RemoveItemCommand");
			}
		}

		public MyInventoryItemModel SelectedInventoryItem
		{
			get
			{
				return m_selectedInventoryItem;
			}
			set
			{
				SetProperty(ref m_selectedInventoryItem, value, "SelectedInventoryItem");
			}
		}

		public MyInventoryItemModel SelectedOfferItem
		{
			get
			{
				return m_selectedOfferItem;
			}
			set
			{
				SetProperty(ref m_selectedOfferItem, value, "SelectedOfferItem");
			}
		}

		public ObservableCollection<MyInventoryItemModel> InventoryItems
		{
			get
			{
				return m_myInventoryItems;
			}
			set
			{
				SetProperty(ref m_myInventoryItems, value, "InventoryItems");
			}
		}

		public ObservableCollection<MyInventoryItemModel> LocalPlayerOfferItems
		{
			get
			{
				return m_localPlayerOfferItems;
			}
			set
			{
				SetProperty(ref m_localPlayerOfferItems, value, "LocalPlayerOfferItems");
			}
		}

		public ObservableCollection<MyInventoryItemModel> OtherPlayerOfferItems
		{
			get
			{
				return m_otherPlayerOfferItems;
			}
			set
			{
				SetProperty(ref m_otherPlayerOfferItems, value, "OtherPlayerOfferItems");
			}
		}

		public MyPlayerTradeViewModel(ulong otherPlayerId)
		{
			m_otherPlayerId = otherPlayerId;
			InitData();
			AcceptCommand = new RelayCommand(OnAccept);
			CancelCommand = new RelayCommand(OnCancel);
			SubmitOfferCommand = new RelayCommand(OnSubmitOffer);
			RemoveItemFromOfferCommand = new RelayCommand<MyInventoryItemModel>(OnRemoveItemFromOffer);
			AddItemToOfferCommand = new RelayCommand<MyInventoryItemModel>(OnAddItemToOffer);
			AddStackTenToOfferCommand = new RelayCommand<MyInventoryItemModel>(OnAddStackTenToOffer);
			AddStackHundredToOfferCommand = new RelayCommand<MyInventoryItemModel>(OnAddStackHundredToOffer);
			RemoveStackTenToOfferCommand = new RelayCommand<MyInventoryItemModel>(OnRemoveStackTenToOffer);
			RemoveStackHundredToOfferCommand = new RelayCommand<MyInventoryItemModel>(OnRemoveStackHundredToOffer);
			SubmitAcceptCommand = new RelayCommand(OnSubmitAccept);
			AddItemCommand = new RelayCommand(OnAddItem);
			RemoveItemCommand = new RelayCommand(OnRemoveItem);
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			bitmapImage.TextureAsset = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			CurrencyIcon = bitmapImage;
			IsSubmitOfferEnabled = true;
			ServiceManager.Instance.AddService((IMyPlayerTradeViewModel)this);
		}

		private void InitData()
		{
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				OnExit(this);
				return;
			}
			MyInventory myInventory = localCharacter.GetInventoryBase() as MyInventory;
			if (myInventory == null)
			{
				OnExit(this);
				return;
			}
			List<MyInventoryItemModel> list = new List<MyInventoryItemModel>();
			foreach (MyPhysicalInventoryItem item2 in myInventory.GetItems())
			{
				MyInventoryItemModel item = new MyInventoryItemModel(item2, myInventory);
				list.Add(item);
			}
			InventoryItems = new ObservableCollection<MyInventoryItemModel>(list);
			MyBankingSystem.Static.TryGetAccountInfo(localHumanPlayer.Identity.IdentityId, out m_localPlayerAccountInfo);
			LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(m_localPlayerAccountInfo.Balance);
			LocalPlayerOfferCurrencyMaximum = m_localPlayerAccountInfo.Balance;
			OtherPlayerOfferCurrency = MyBankingSystem.GetFormatedValue(0L);
			switch (MySession.Static.BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.NONE:
				IsPcuVisible = false;
				break;
			case MyBlockLimitsEnabledEnum.GLOBALLY:
				IsPcuVisible = false;
				break;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
			{
				MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(localHumanPlayer.Identity.IdentityId);
				MyPlayer playerById = MySession.Static.Players.GetPlayerById(new MyPlayer.PlayerId(m_otherPlayerId));
				MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(playerById.Identity.IdentityId);
				bool num = playerFaction?.IsLeader(localHumanPlayer.Identity.IdentityId) ?? false;
				bool flag = playerFaction2?.IsLeader(playerById.Identity.IdentityId) ?? false;
				bool flag2 = num && flag;
				IsPcuVisible = flag2 && MySession.Static.Settings.EnablePcuTrading;
<<<<<<< HEAD
				if (IsPcuVisible && playerFaction != null)
=======
				if (IsPcuVisible)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_localAvailablePCU = playerFaction.BlockLimits.PCU;
					LocalPlayerPcu = m_localAvailablePCU.ToString();
				}
				break;
			}
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
			{
				IsPcuVisible = MySession.Static.Settings.EnablePcuTrading;
				MyBlockLimits blockLimits = localHumanPlayer.Identity.BlockLimits;
				m_localAvailablePCU = blockLimits.PCU;
				LocalPlayerPcu = m_localAvailablePCU.ToString();
				break;
			}
			}
			SubmitOfferLabel = MyTexts.GetString(MySpaceTexts.TradeScreenSubmitOffer);
			IsAcceptVisible = true;
			OtherPlayerAcceptState = MyTexts.GetString(MySpaceTexts.TradeScreenNotAccepted);
			LocalPlayerOfferItems.add_CollectionChanged(new NotifyCollectionChangedEventHandler(OnLocalOfferItemsChanged));
			DragDrop.Instance.DropStarting += DropStarting;
		}

		private void OnLocalPlayerOfferCurrencyChanged(float value)
		{
			LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(m_localPlayerAccountInfo.Balance - (long)value);
			UpdateButtons(offerSubmitted: false);
		}

		private void OnLocalPlayerOfferPCUChanged(float value)
		{
			if (float.IsNaN(value))
			{
				LocalPlayerPcu = "0";
			}
			else
			{
				LocalPlayerPcu = (m_localAvailablePCU - (int)value).ToString();
			}
			UpdateButtons(offerSubmitted: false);
		}

		private void OnAddItem(object obj)
		{
			if (SelectedInventoryItem != null)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON, MyControlStateType.PRESSED))
				{
					OnAddStackTenToOffer(SelectedInventoryItem);
				}
				else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON, MyControlStateType.PRESSED))
				{
					OnAddStackHundredToOffer(SelectedInventoryItem);
				}
				else
				{
					OnAddItemToOffer(SelectedInventoryItem);
				}
			}
		}

		private void OnAddItemToOffer(MyInventoryItemModel item)
		{
<<<<<<< HEAD
			InventoryItems.Remove(item);
			SelectedInventoryItem = null;
			LocalPlayerOfferItems.Add(item);
=======
			((Collection<MyInventoryItemModel>)(object)InventoryItems).Remove(item);
			SelectedInventoryItem = null;
			((Collection<MyInventoryItemModel>)(object)LocalPlayerOfferItems).Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			UpdateButtons(offerSubmitted: false);
		}

		private void OnAddStackHundredToOffer(MyInventoryItemModel item)
		{
			m_isLeftToRight = true;
			m_draggedItem = item;
			if (item.Amount >= 100f)
			{
				OnAmountChanged(100f);
			}
			else
			{
				OnAmountChanged(item.Amount);
			}
		}

		private void OnAddStackTenToOffer(MyInventoryItemModel item)
		{
			m_isLeftToRight = true;
			m_draggedItem = item;
			if (item.Amount >= 10f)
			{
				OnAmountChanged(10f);
			}
			else
			{
				OnAmountChanged(item.Amount);
			}
		}

		private void OnRemoveItem(object obj)
		{
			if (SelectedOfferItem != null)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON, MyControlStateType.PRESSED))
				{
					OnRemoveStackTenToOffer(SelectedOfferItem);
				}
				else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON, MyControlStateType.PRESSED))
				{
					OnRemoveStackHundredToOffer(SelectedOfferItem);
				}
				else
				{
					OnRemoveItemFromOffer(SelectedOfferItem);
				}
			}
		}

		private void OnRemoveStackHundredToOffer(MyInventoryItemModel item)
		{
			m_isLeftToRight = false;
			m_draggedItem = item;
			if (item.Amount >= 100f)
			{
				OnAmountChanged(100f);
			}
			else
			{
				OnAmountChanged(item.Amount);
			}
		}

		private void OnRemoveStackTenToOffer(MyInventoryItemModel item)
		{
			m_isLeftToRight = false;
			m_draggedItem = item;
			if (item.Amount >= 10f)
			{
				OnAmountChanged(10f);
			}
			else
			{
				OnAmountChanged(item.Amount);
			}
		}

		private void OnRemoveItemFromOffer(MyInventoryItemModel item)
		{
<<<<<<< HEAD
			LocalPlayerOfferItems.Remove(item);
			SelectedOfferItem = null;
			InventoryItems.Add(item);
=======
			((Collection<MyInventoryItemModel>)(object)LocalPlayerOfferItems).Remove(item);
			SelectedOfferItem = null;
			((Collection<MyInventoryItemModel>)(object)InventoryItems).Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			UpdateButtons(offerSubmitted: false);
		}

		private void OnLocalOfferItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateButtons(offerSubmitted: false);
		}

		private void OnSubmitOffer(object obj)
		{
			UpdateButtons(offerSubmitted: true);
			SubmitOfferLabel = MyTexts.GetString(MySpaceTexts.TradeScreenOfferSubmited);
			List<MyObjectBuilder_InventoryItem> list = new List<MyObjectBuilder_InventoryItem>(((Collection<MyInventoryItemModel>)(object)LocalPlayerOfferItems).Count);
			foreach (MyInventoryItemModel item in (Collection<MyInventoryItemModel>)(object)LocalPlayerOfferItems)
			{
				list.Add(item.InventoryItem.GetObjectBuilder());
			}
			MyObjectBuilder_SubmitOffer myObjectBuilder_SubmitOffer = new MyObjectBuilder_SubmitOffer();
			myObjectBuilder_SubmitOffer.InventoryItems = list;
			myObjectBuilder_SubmitOffer.CurrencyAmount = (long)LocalPlayerOfferCurrency;
			if (float.IsNaN(LocalPlayerOfferPcu))
			{
				myObjectBuilder_SubmitOffer.PCUAmount = 0;
			}
			else
			{
				myObjectBuilder_SubmitOffer.PCUAmount = (int)LocalPlayerOfferPcu;
			}
			MyTradingManager.Static.SubmitTradingOffer_Client(myObjectBuilder_SubmitOffer);
		}

		private void OnSubmitAccept(object obj)
		{
			if (IsAcceptEnabled)
			{
				OnAccept(null);
			}
			else if (IsSubmitOfferEnabled)
			{
				OnSubmitOffer(null);
			}
		}

		private void UpdateButtons(bool offerSubmitted)
		{
			IsSubmitOfferEnabled = !offerSubmitted;
			IsAcceptEnabled = offerSubmitted;
			SubmitOfferLabel = (offerSubmitted ? MyTexts.GetString(MySpaceTexts.TradeScreenOfferSubmited) : MyTexts.GetString(MySpaceTexts.TradeScreenSubmitOffer));
			if (m_waitingToAcceptTrade)
			{
				AbortOffer();
			}
		}

		internal void OnOfferRecieved(MyObjectBuilder_SubmitOffer obOffer)
		{
			List<MyInventoryItemModel> list = new List<MyInventoryItemModel>();
			foreach (MyObjectBuilder_InventoryItem inventoryItem in obOffer.InventoryItems)
			{
				MyInventoryItemModel item = new MyInventoryItemModel(new MyPhysicalInventoryItem(inventoryItem), null);
				list.Add(item);
			}
			OtherPlayerOfferItems = new ObservableCollection<MyInventoryItemModel>(list);
			OtherPlayerOfferCurrency = MyBankingSystem.GetFormatedValue(obOffer.CurrencyAmount);
			OtherPlayerOfferPcu = obOffer.PCUAmount.ToString();
			AbortOffer_Internal();
		}

		internal void OnOfferAbortedRecieved()
		{
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(new MyPlayer.PlayerId(m_otherPlayerId));
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(playerById.DisplayName + " inventory is full"), new StringBuilder("Offer No Accepted"), null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
			UpdateButtons(offerSubmitted: false);
		}

		private void OnAccept(object obj)
		{
			if (!m_waitingToAcceptTrade)
			{
				IsCancelVisible = true;
				IsAcceptVisible = false;
				m_waitingToAcceptTrade = true;
				MyTradingManager.Static.AcceptOffer_Client(isAccepted: true);
			}
			else
			{
				MyLog.Default.Error("Bad button state. Please check.");
			}
		}

		private void OnCancel(object obj)
		{
			if (m_waitingToAcceptTrade)
			{
				AbortOffer();
			}
			else
			{
				MyLog.Default.Error("Bad button state. Please check.");
			}
		}

		/// <summary>
		/// Aborts the offer and sends message to server that the offer is aborted.
		/// </summary>
		private void AbortOffer()
		{
			AbortOffer_Internal();
			MyTradingManager.Static.AcceptOffer_Client(isAccepted: false);
		}

		/// <summary>
		/// Aborts the offer locally. Does not inform the server, only updates internal data.
		/// </summary>
		private void AbortOffer_Internal()
		{
			m_waitingToAcceptTrade = false;
			IsCancelVisible = false;
			IsAcceptVisible = true;
		}

		internal void OnOfferAcceptStateChange(bool isAccepted)
		{
			if (isAccepted)
			{
				OtherPlayerAcceptState = MyTexts.GetString(MySpaceTexts.TradeScreenAccepted);
			}
			else
			{
				OtherPlayerAcceptState = MyTexts.GetString(MySpaceTexts.TradeScreenNotAccepted);
			}
		}

		public void CloseScreenLocal()
		{
			m_triggerCancelEvent = false;
			MyScreenManager.GetScreenWithFocus().CloseScreen();
		}

		public override void OnScreenClosing()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (m_triggerCancelEvent)
			{
				MyTradingManager.Static.TradeCancel_Client();
			}
			ServiceManager.Instance.RemoveService<IMyPlayerTradeViewModel>();
			LocalPlayerOfferItems.remove_CollectionChanged(new NotifyCollectionChangedEventHandler(OnLocalOfferItemsChanged));
			DragDrop.Instance.DropStarting -= DropStarting;
			base.OnScreenClosing();
		}

		private void DropStarting(object sender, DragDropEventArgs e)
		{
			e.Cancel = true;
			if ((sender as ListBox).ItemsSource == m_myInventoryItems)
			{
				m_isLeftToRight = false;
			}
			else
			{
				m_isLeftToRight = true;
			}
			MyInventoryItemModel myInventoryItemModel = (m_draggedItem = e.Data as MyInventoryItemModel);
			if (e.MouseButton == MouseButton.Right)
			{
				MyObjectBuilderType typeId = myInventoryItemModel.InventoryItem.Content.TypeId;
				bool parseAsInteger = true;
				if (typeId == typeof(MyObjectBuilder_Ore) || typeId == typeof(MyObjectBuilder_Ingot))
				{
					parseAsInteger = false;
				}
				MyGuiScreenDialogAmount myGuiScreenDialogAmount = new MyGuiScreenDialogAmount(0f, myInventoryItemModel.Amount, MyCommonTexts.DialogAmount_AddAmountCaption, 0, parseAsInteger, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity);
				myGuiScreenDialogAmount.OnConfirmed += OnAmountChanged;
				MyGuiSandbox.AddScreen(myGuiScreenDialogAmount);
			}
			else
			{
				OnAmountChanged(m_draggedItem.Amount);
			}
		}

		private void OnAmountChanged(float amount)
		{
			if (amount == 0f)
			{
				UpdateButtons(offerSubmitted: false);
				m_draggedItem = null;
				return;
			}
			ObservableCollection<MyInventoryItemModel> val = InventoryItems;
			ObservableCollection<MyInventoryItemModel> val2 = LocalPlayerOfferItems;
			if (m_isLeftToRight)
			{
				val = LocalPlayerOfferItems;
				val2 = InventoryItems;
			}
			float num = m_draggedItem.Amount - amount;
			if (num == 0f)
			{
<<<<<<< HEAD
				observableCollection2.Remove(m_draggedItem);
				if (observableCollection2 == LocalPlayerOfferItems)
=======
				((Collection<MyInventoryItemModel>)(object)val2).Remove(m_draggedItem);
				if (val2 == LocalPlayerOfferItems)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					SelectedOfferItem = null;
				}
				else
				{
					SelectedInventoryItem = null;
				}
			}
			else
			{
				m_draggedItem.Amount = num;
			}
			bool flag = false;
			foreach (MyInventoryItemModel item in (Collection<MyInventoryItemModel>)(object)val)
			{
				if (item.InventoryItem.ItemId == m_draggedItem.InventoryItem.ItemId)
				{
					item.Amount += amount;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MyInventoryItemModel myInventoryItemModel = new MyInventoryItemModel(m_draggedItem.InventoryItem, null);
				myInventoryItemModel.Amount = amount;
				((Collection<MyInventoryItemModel>)(object)val).Add(myInventoryItemModel);
			}
			UpdateButtons(offerSubmitted: false);
			m_draggedItem = null;
		}
	}
}
