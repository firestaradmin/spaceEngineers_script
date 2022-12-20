using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyContractsBlockActiveViewModel : MyViewModelBase
	{
		private MyContractBlock m_contractBlock;

		private long m_stationId;

		private long m_blockId;

		private int m_selectedActiveContractIndex;

		private int? m_previousSelectedActiveContractIndex;

		private int m_selectedGridIndex;

		private int m_activeContractCount;

		private int m_activeContractCountMax;

		private bool m_isFinishEnabled;

		private bool m_isAbandonEnabled;

		private bool m_isVisibleGridSelection;

		private bool m_isGridConfirmEnabled;

		private bool m_isNoActiveContractVisible;

		private ICommand m_sortingActiveContractCommand;

		private ICommand m_finishCommand;

		private ICommand m_refreshActiveCommand;

		private ICommand m_abandonCommand;

		private ICommand m_confirmGridSelectionCommand;

		private ICommand m_exitGridSelectionCommand;

		private MyContractModel m_selectedActiveContract;

		private ObservableCollection<MyContractModel> m_activeContracts;

		private ObservableCollection<MySimpleSelectableItemModel> m_selectableGrids;

		private long m_gridSelectionContractId;

		private string m_lastSortActive = string.Empty;

		private bool m_isLastSortActiveAsc;

		private bool m_isWaitingForAbandon;

		private bool m_isWaitingForFinish;

		private string m_finishTooltipText;

		public int SelectedActiveContractIndex
		{
			get
			{
				return m_selectedActiveContractIndex;
			}
			set
			{
				SetProperty(ref m_selectedActiveContractIndex, value, "SelectedActiveContractIndex");
				UpdateFinish();
				UpdateAbandon();
			}
		}

		public MyContractModel SelectedActiveContract
		{
			get
			{
				return m_selectedActiveContract;
			}
			set
			{
				SetProperty(ref m_selectedActiveContract, value, "SelectedActiveContract");
			}
		}

		public int SelectedTargetIndex
		{
			get
			{
				return m_selectedGridIndex;
			}
			set
			{
				SetProperty(ref m_selectedGridIndex, value, "SelectedTargetIndex");
				UpdateGridConfirm();
			}
		}

		private bool IsWaitingForAbandon
		{
			get
			{
				return m_isWaitingForAbandon;
			}
			set
			{
				SetProperty(ref m_isWaitingForAbandon, value, "IsWaitingForAbandon");
				UpdateAbandon();
			}
		}

		private bool IsWaitingForFinish
		{
			get
			{
				return m_isWaitingForFinish;
			}
			set
			{
				SetProperty(ref m_isWaitingForFinish, value, "IsWaitingForFinish");
				UpdateFinish();
			}
		}

		public string FinishTooltipText
		{
			get
			{
				return m_finishTooltipText;
			}
			set
			{
				SetProperty(ref m_finishTooltipText, value, "FinishTooltipText");
			}
		}

		public int ActiveContractCount
		{
			get
			{
				return m_activeContractCount;
			}
			set
			{
				SetProperty(ref m_activeContractCount, value, "ActiveContractCount");
				RaisePropertyChanged("ActiveContractCountStatus");
			}
		}

		public int ActiveContractCountMax
		{
			get
			{
				return m_activeContractCountMax;
			}
			set
			{
				SetProperty(ref m_activeContractCountMax, value, "ActiveContractCountMax");
				RaisePropertyChanged("ActiveContractCountStatus");
			}
		}

		public bool IsNoActiveContractVisible
		{
			get
			{
				return m_isNoActiveContractVisible;
			}
			set
			{
				SetProperty(ref m_isNoActiveContractVisible, value, "IsNoActiveContractVisible");
			}
		}

		public ICommand ExitGridSelectionCommand
		{
			get
			{
				return m_exitGridSelectionCommand;
			}
			set
			{
				SetProperty(ref m_exitGridSelectionCommand, value, "ExitGridSelectionCommand");
			}
		}

		public ICommand SortingActiveContractCommand
		{
			get
			{
				return m_sortingActiveContractCommand;
			}
			set
			{
				SetProperty(ref m_sortingActiveContractCommand, value, "SortingActiveContractCommand");
			}
		}

		public ICommand FinishCommand
		{
			get
			{
				return m_finishCommand;
			}
			set
			{
				SetProperty(ref m_finishCommand, value, "FinishCommand");
			}
		}

		public ICommand RefreshActiveCommand
		{
			get
			{
				return m_refreshActiveCommand;
			}
			set
			{
				SetProperty(ref m_refreshActiveCommand, value, "RefreshActiveCommand");
			}
		}

		public ICommand AbandonCommand
		{
			get
			{
				return m_abandonCommand;
			}
			set
			{
				SetProperty(ref m_abandonCommand, value, "AbandonCommand");
			}
		}

		public ICommand ConfirmGridSelectionCommand
		{
			get
			{
				return m_confirmGridSelectionCommand;
			}
			set
			{
				SetProperty(ref m_confirmGridSelectionCommand, value, "ConfirmGridSelectionCommand");
			}
		}

		public bool IsFinishEnabled
		{
			get
			{
				return m_isFinishEnabled;
			}
			set
			{
				SetProperty(ref m_isFinishEnabled, value, "IsFinishEnabled");
			}
		}

		public bool IsAbandonEnabled
		{
			get
			{
				return m_isAbandonEnabled;
			}
			set
			{
				SetProperty(ref m_isAbandonEnabled, value, "IsAbandonEnabled");
			}
		}

		public bool IsVisibleGridSelection
		{
			get
			{
				return m_isVisibleGridSelection;
			}
			set
			{
				SetProperty(ref m_isVisibleGridSelection, value, "IsVisibleGridSelection");
			}
		}

		public bool IsGridConfirmEnabled
		{
			get
			{
				return m_isGridConfirmEnabled;
			}
			set
			{
				SetProperty(ref m_isGridConfirmEnabled, value, "IsGridConfirmEnabled");
			}
		}

		public ObservableCollection<MyContractModel> ActiveContracts
		{
			get
			{
				return m_activeContracts;
			}
			set
			{
				SetProperty(ref m_activeContracts, value, "ActiveContracts");
<<<<<<< HEAD
				IsNoActiveContractVisible = value == null || value.Count == 0;
=======
				IsNoActiveContractVisible = value == null || ((Collection<MyContractModel>)(object)value).Count == 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public ObservableCollection<MySimpleSelectableItemModel> SelectableTargets
		{
			get
			{
				return m_selectableGrids;
			}
			set
			{
				SetProperty(ref m_selectableGrids, value, "SelectableTargets");
			}
		}

		public long GridSelectionContractId
		{
			get
			{
				return m_gridSelectionContractId;
			}
			set
			{
				SetProperty(ref m_gridSelectionContractId, value, "GridSelectionContractId");
			}
		}

		public string ActiveContractCountStatus => $"{ActiveContractCount}/{ActiveContractCountMax}";

		public long StationId
		{
			get
			{
				return m_stationId;
			}
			set
			{
				SetProperty(ref m_stationId, value, "StationId");
			}
		}

		public long BlockId
		{
			get
			{
				return m_blockId;
			}
			set
			{
				SetProperty(ref m_blockId, value, "BlockId");
			}
		}

		public MyContractsBlockActiveViewModel(MyContractBlock contractBlock)
		{
			m_contractBlock = contractBlock;
			SortingActiveContractCommand = new RelayCommand<DataGridSortingEventArgs>(OnSortingActiveContract);
			FinishCommand = new RelayCommand(OnFinish);
			RefreshActiveCommand = new RelayCommand(OnRefreshActive);
			AbandonCommand = new RelayCommand(OnAbandon);
			ConfirmGridSelectionCommand = new RelayCommand(OnConfirmGridSelection);
			ExitGridSelectionCommand = new RelayCommand(OnExitGridSelection);
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			ActiveContractCountMax = component.GetContractLimitPerPlayer();
			IsVisibleGridSelection = false;
			SetDefaultActiveContractIndex();
		}

		public override void InitializeData()
		{
			m_contractBlock.GetActiveContracts(MySession.Static.LocalPlayerId, OnGetActiveContracts);
		}

		private void SetDefaultActiveContractIndex()
		{
			if (m_previousSelectedActiveContractIndex.HasValue)
			{
<<<<<<< HEAD
				if (ActiveContracts == null || ActiveContracts.Count == 0)
				{
					SelectedActiveContractIndex = -1;
				}
				else if (m_previousSelectedActiveContractIndex >= ActiveContracts.Count)
				{
					SelectedActiveContractIndex = ActiveContracts.Count - 1;
=======
				if (ActiveContracts == null || ((Collection<MyContractModel>)(object)ActiveContracts).Count == 0)
				{
					SelectedActiveContractIndex = -1;
				}
				else if (m_previousSelectedActiveContractIndex >= ((Collection<MyContractModel>)(object)ActiveContracts).Count)
				{
					SelectedActiveContractIndex = ((Collection<MyContractModel>)(object)ActiveContracts).Count - 1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					SelectedActiveContractIndex = m_previousSelectedActiveContractIndex.Value;
				}
				m_previousSelectedActiveContractIndex = null;
			}
			else
			{
<<<<<<< HEAD
				SelectedActiveContractIndex = ((ActiveContracts == null || ActiveContracts.Count <= 0) ? (-1) : 0);
=======
				SelectedActiveContractIndex = ((ActiveContracts == null || ((Collection<MyContractModel>)(object)ActiveContracts).Count <= 0) ? (-1) : 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (SelectedActiveContractIndex > -1)
			{
				SelectedActiveContract = ((Collection<MyContractModel>)(object)ActiveContracts)[SelectedActiveContractIndex];
			}
			else
			{
				SelectedActiveContract = null;
			}
		}

		public void OnGetActiveContracts(List<MyObjectBuilder_Contract> contracts, long stationId, long blockId)
		{
			StationId = stationId;
			BlockId = blockId;
			ObservableCollection<MyContractModel> val = new ObservableCollection<MyContractModel>();
			foreach (MyObjectBuilder_Contract contract in contracts)
			{
				MyContractModel item = MyContractModelFactory.CreateInstance(contract);
				((Collection<MyContractModel>)(object)val).Add(item);
			}
			if (string.IsNullOrEmpty(m_lastSortActive))
			{
				ActiveContracts = val;
			}
			else
			{
				DataGridSortingEventArgs sortingArgs = new DataGridSortingEventArgs(new DataGridColumn
				{
					SortMemberPath = m_lastSortActive,
					SortDirection = ((!m_isLastSortActiveAsc) ? ListSortDirection.Descending : ListSortDirection.Ascending)
				});
				ActiveContracts = new ObservableCollection<MyContractModel>(SortContracts(val, sortingArgs, out m_lastSortActive, out m_isLastSortActiveAsc));
			}
			SetDefaultActiveContractIndex();
			ActiveContractCount = ((Collection<MyContractModel>)(object)val).Count;
		}

		public void OnRefreshActive(object obj)
		{
			m_contractBlock.GetActiveContracts(MySession.Static.LocalPlayerId, OnGetActiveContracts);
		}

		private void OnFinish(object obj)
		{
<<<<<<< HEAD
			if (!IsFinishEnabled || SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ActiveContracts.Count)
=======
			if (!IsFinishEnabled || SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ((Collection<MyContractModel>)(object)ActiveContracts).Count)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			MyContractModel myContractModel = ((Collection<MyContractModel>)(object)m_activeContracts)[SelectedActiveContractIndex];
			MyContractConditionModel myContractConditionModel = null;
			foreach (MyContractConditionModel item in (Collection<MyContractConditionModel>)(object)myContractModel.Conditions)
			{
				if ((StationId > 0 && StationId == item.StationEndId) || (StationId <= 0 && item.BlockEndId == BlockId))
				{
					myContractConditionModel = item;
					break;
				}
			}
			if (myContractConditionModel == null)
			{
				return;
			}
			if (myContractConditionModel is MyContractConditionDeliverItemModel)
			{
				IsWaitingForFinish = true;
				m_contractBlock.GetConnectedEntities(MySession.Static.LocalPlayerId, myContractModel.Id, OnFinishTargetSelectionCallback);
			}
			else if (myContractConditionModel is MyContractConditionDeliverPackageModel)
			{
				IsWaitingForFinish = true;
				long targetEntityId = 0L;
				if (MySession.Static.LocalCharacter != null)
				{
					targetEntityId = MySession.Static.LocalCharacter.EntityId;
				}
				m_contractBlock.FinishContract(MySession.Static.LocalPlayerId, myContractModel.Id, targetEntityId, OnFinishCallback);
			}
			else if (myContractConditionModel is MyContractConditionCustomModel)
			{
				IsWaitingForFinish = true;
				long targetEntityId2 = 0L;
				if (MySession.Static.LocalCharacter != null)
				{
					targetEntityId2 = MySession.Static.LocalCharacter.EntityId;
				}
				m_contractBlock.FinishContract(MySession.Static.LocalPlayerId, myContractModel.Id, targetEntityId2, OnFinishCallback);
			}
		}

		private void OnAbandon(object obj)
		{
<<<<<<< HEAD
			if (SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ActiveContracts.Count)
			{
				return;
			}
			MyContractModel cont = m_activeContracts[SelectedActiveContractIndex];
=======
			if (SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ((Collection<MyContractModel>)(object)ActiveContracts).Count)
			{
				return;
			}
			MyContractModel cont = ((Collection<MyContractModel>)(object)m_activeContracts)[SelectedActiveContractIndex];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ShowMessageBoxYesNo(MyTexts.Get(MySpaceTexts.Contracts_AbandonConfirmation_Caption), MyTexts.Get(MySpaceTexts.Contracts_AbandonConfirmation_Text), delegate(MyGuiScreenMessageBox.ResultEnum retval)
			{
				if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					IsWaitingForAbandon = true;
					m_contractBlock.AbandonContract(MySession.Static.LocalPlayerId, cont.Id, OnAbandonCallback);
				}
			});
		}

		private void OnConfirmGridSelection(object obj)
		{
			if (SelectedTargetIndex >= 0 && SelectedTargetIndex < ((Collection<MySimpleSelectableItemModel>)(object)SelectableTargets).Count)
			{
				MySimpleSelectableItemModel mySimpleSelectableItemModel = ((Collection<MySimpleSelectableItemModel>)(object)SelectableTargets)[SelectedTargetIndex];
				if (mySimpleSelectableItemModel != null)
				{
					IsWaitingForFinish = true;
					m_contractBlock.FinishContract(MySession.Static.LocalPlayerId, GridSelectionContractId, mySimpleSelectableItemModel.Id, OnFinishCallback);
					IsVisibleGridSelection = false;
					InputManager.Current.NavigateTabIndex(50, messageBoxVisible: false);
				}
			}
		}

		private void OnExitGridSelection(object obj)
		{
			IsVisibleGridSelection = false;
			InputManager.Current.NavigateTabIndex(50, messageBoxVisible: false);
		}

		public void OnFinishCallback(MyContractResults result)
		{
			if (result == MyContractResults.Success)
			{
				MyContractModel myContractModel = ((Collection<MyContractModel>)(object)m_activeContracts)[SelectedActiveContractIndex];
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MySpaceTexts.Contracts_Completed_Text, myContractModel.RewardReputation, MyBankingSystem.GetFormatedValue(myContractModel.RewardMoney, addCurrencyShortName: true));
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Completed_Caption), stringBuilder);
			}
			else
			{
				ShowErrorFailNotification(result);
			}
			m_previousSelectedActiveContractIndex = SelectedActiveContractIndex;
			OnRefreshActive(null);
			IsWaitingForFinish = false;
		}

		public void OnAbandonCallback(MyContractResults result)
		{
			if (result != 0)
			{
				ShowErrorFailNotification(result);
			}
			m_previousSelectedActiveContractIndex = SelectedActiveContractIndex;
			OnRefreshActive(null);
			IsWaitingForAbandon = false;
		}

		public void OnFinishTargetSelectionCallback(bool isSuccessful, List<MyContractBlock.MyTargetEntityInfoWrapper> availableTargets, long contractId)
		{
			if (!isSuccessful)
			{
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_NoAccess), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_NoAccess));
				return;
			}
			ObservableCollection<MySimpleSelectableItemModel> val = new ObservableCollection<MySimpleSelectableItemModel>();
			foreach (MyContractBlock.MyTargetEntityInfoWrapper availableTarget in availableTargets)
			{
				((Collection<MySimpleSelectableItemModel>)(object)val).Add(new MySimpleSelectableItemModel(availableTarget.Id, availableTarget.Name, availableTarget.DisplayName));
			}
			SelectableTargets = val;
			GridSelectionContractId = contractId;
			if (((Collection<MySimpleSelectableItemModel>)(object)SelectableTargets).Count > 0)
			{
				SelectedTargetIndex = 0;
			}
			IsVisibleGridSelection = true;
			IsWaitingForFinish = false;
			InputManager.Current.NavigateTabIndex(500, messageBoxVisible: false);
		}

		private void UpdateFinish()
		{
			if (IsWaitingForFinish || SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ((Collection<MyContractModel>)(object)ActiveContracts).Count)
			{
				IsFinishEnabled = false;
				return;
			}
			MyContractModel myContractModel = ((Collection<MyContractModel>)(object)m_activeContracts)[SelectedActiveContractIndex];
			if (myContractModel.CanBeFinishedInTerminal)
			{
				bool flag = false;
				foreach (MyContractConditionModel item in (Collection<MyContractConditionModel>)(object)myContractModel.Conditions)
				{
					if ((StationId > 0 && StationId == item.StationEndId) || (StationId <= 0 && item.BlockEndId == BlockId))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					IsFinishEnabled = true;
					FinishTooltipText = MyTexts.GetString(MySpaceTexts.Economy_Contract_FinishTooltip_YouCanFinish);
				}
				else
				{
					IsFinishEnabled = false;
					FinishTooltipText = MyTexts.GetString(MySpaceTexts.Economy_Contract_FinishTooltip_NotAFinishPoint);
				}
			}
			else
			{
				IsFinishEnabled = false;
				FinishTooltipText = MyTexts.GetString(MySpaceTexts.Economy_Contract_FinishTooltip_CannotFinishInBlock);
			}
		}

		private void UpdateAbandon()
		{
			if (IsWaitingForAbandon || SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ((Collection<MyContractModel>)(object)ActiveContracts).Count)
			{
				IsAbandonEnabled = false;
			}
			else
			{
				IsAbandonEnabled = true;
			}
		}

		private void UpdateGridConfirm()
		{
			if (IsWaitingForFinish || SelectedTargetIndex < 0 || SelectedTargetIndex >= ((Collection<MySimpleSelectableItemModel>)(object)SelectableTargets).Count)
			{
				IsGridConfirmEnabled = false;
			}
			else
			{
				IsGridConfirmEnabled = true;
			}
		}

		private void OnSortingActiveContract(DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyContractModel> enumerable = SortContracts(ActiveContracts, sortingArgs, out m_lastSortActive, out m_isLastSortActiveAsc);
			ActiveContracts = new ObservableCollection<MyContractModel>(enumerable);
			SetDefaultActiveContractIndex();
		}

		private IEnumerable<MyContractModel> SortContracts(ObservableCollection<MyContractModel> toSort, DataGridSortingEventArgs sortingArgs, out string sortParam, out bool sortAsc)
		{
			sortParam = string.Empty;
			sortAsc = false;
			IEnumerable<MyContractModel> result = null;
			DataGridColumn column = sortingArgs.Column;
			ListSortDirection? sortDirection = column.SortDirection;
			sortAsc = sortDirection.HasValue && sortDirection == ListSortDirection.Ascending;
			switch (column.SortMemberPath)
			{
			case "Name":
				sortParam = "Name";
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderBy<MyContractModel, string>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, string>)((MyContractModel u) => u.NameWithId));
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderByDescending<MyContractModel, string>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, string>)((MyContractModel u) => u.NameWithId));
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			case "Currency":
				sortParam = "Currency";
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderBy<MyContractModel, long>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, long>)((MyContractModel u) => u.RewardMoney));
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderByDescending<MyContractModel, long>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, long>)((MyContractModel u) => u.RewardMoney));
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			case "Reputation":
				sortParam = "Reputation";
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderBy<MyContractModel, int>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, int>)((MyContractModel u) => u.RewardReputation));
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderByDescending<MyContractModel, int>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, int>)((MyContractModel u) => u.RewardReputation));
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			case "Duration":
				sortParam = "Duration";
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderBy<MyContractModel, double>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, double>)((MyContractModel u) => u.RemainingTime));
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = (IEnumerable<MyContractModel>)Enumerable.OrderByDescending<MyContractModel, double>((IEnumerable<MyContractModel>)toSort, (Func<MyContractModel, double>)((MyContractModel u) => u.RemainingTime));
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			}
			return result;
		}

		private void ShowErrorFailNotification(MyContractResults state)
		{
			switch (state)
			{
			case MyContractResults.Fail_CannotAccess:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_NoAccess), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_NoAccess));
				break;
			case MyContractResults.Fail_ContractNotFound_Activation:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Activation), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Activation));
				break;
			case MyContractResults.Fail_ContractNotFound_Finish:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Finish), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Finish));
				break;
			case MyContractResults.Fail_ContractNotFound_Abandon:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Abandon), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Abandon));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationconditionNotMet));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_InsufficientFunds:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_InsufficientFunds), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_InsufficientFunds));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_InsufficientSpace:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_InsufficientSpace), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_InsufficientSpace));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_ContractLimitReached), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_ContractLimitReached));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_TargetOffline:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_TargetOffline), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_TargetOffline));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_YouAreTargetOfThisHunt), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_YouAreTargetOfThisHunt));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishingCondition), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishingCondition));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_IncorrectTargetEntity:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_IncorrectGrid), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_IncorrectGrid));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_MissingPackage:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_MissingPackage), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_MissingPackage));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_NotEnoughItems:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_NotEnoughItems), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_NotEnoughItems));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_NotEnoughSpace:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_NotEnoughSpace), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_NotEnoughSpace));
				break;
			case MyContractResults.Error_Unknown:
			case MyContractResults.Error_MissingKeyStructure:
			case MyContractResults.Error_InvalidData:
				MyLog.Default.Error(new StringBuilder("Contracts - error result: " + state));
				break;
			case MyContractResults.Fail_NotPossible:
				break;
			case MyContractResults.Success:
				break;
			}
		}

		private void ShowMessageBoxOk(StringBuilder caption, StringBuilder text)
		{
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false);
			myGuiScreenMessageBox.OkGamepadStateType = MyControlStateType.NEW_RELEASED;
			MyScreenManager.AddScreen(myGuiScreenMessageBox);
		}

		private void ShowMessageBoxYesNo(StringBuilder caption, StringBuilder text, Action<MyGuiScreenMessageBox.ResultEnum> callback)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, text, caption, null, null, null, null, callback, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}
	}
}
