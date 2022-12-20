using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyContractsBlockViewModel : MyViewModelBase
	{
		private MyContractBlock m_contractBlock;

		private bool m_isNpc;

		private long m_stationId;

		private long m_blockId;

		private int m_selectedAvailableContractIndex;

		private int? m_previousSelectedAvailableContractIndex;

		private bool m_isAcceptEnabled;

		private ICommand m_sortingAvailableContractCommand;

		private ICommand m_acceptCommand;

		private ICommand m_refreshAvailableCommand;

		private MyContractModel m_selectedAvailableContract;

		private ObservableCollection<MyContractModel> m_availableContracts;

		private ObservableCollection<MyContractModel> m_availableContractsComplete;

		private bool m_isWaitingForAccept;

		private string m_lastSortAvailable = string.Empty;

		private bool m_isLastSortAvailableAsc;

		private bool m_isAdministrationVisible;

		private ObservableCollection<MyContractTypeFilterItemModel> m_filterTargets;

		private int m_filterTargetIndex = -1;

		private MyContractTypeFilterItemModel m_selectedFilterTarget;

		private bool m_isNoAvailableContractVisible;

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

		public int SelectedAvailableContractIndex
		{
			get
			{
				return m_selectedAvailableContractIndex;
			}
			set
			{
				SetProperty(ref m_selectedAvailableContractIndex, value, "SelectedAvailableContractIndex");
				UpdateAccept();
			}
		}

		public MyContractModel SelectedAvailableContract
		{
			get
			{
				return m_selectedAvailableContract;
			}
			set
			{
				SetProperty(ref m_selectedAvailableContract, value, "SelectedAvailableContract");
			}
		}

		public ICommand SortingAvailableContractCommand
		{
			get
			{
				return m_sortingAvailableContractCommand;
			}
			set
			{
				SetProperty(ref m_sortingAvailableContractCommand, value, "SortingAvailableContractCommand");
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

		public ICommand RefreshAvailableCommand
		{
			get
			{
				return m_refreshAvailableCommand;
			}
			set
			{
				SetProperty(ref m_refreshAvailableCommand, value, "RefreshAvailableCommand");
			}
		}

		private bool IsWaitingForAccept
		{
			get
			{
				return m_isWaitingForAccept;
			}
			set
			{
				SetProperty(ref m_isWaitingForAccept, value, "IsWaitingForAccept");
				UpdateAccept();
			}
		}

		public bool IsNoAvailableContractVisible
		{
			get
			{
				return m_isNoAvailableContractVisible;
			}
			set
			{
				SetProperty(ref m_isNoAvailableContractVisible, value, "IsNoAvailableContractVisible");
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

		public bool IsNpc
		{
			get
			{
				return m_isNpc;
			}
			set
			{
				SetProperty(ref m_isNpc, value, "IsNpc");
			}
		}

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

		public ObservableCollection<MyContractModel> AvailableContractsComplete
		{
			get
			{
				return m_availableContractsComplete;
			}
			set
			{
				SetProperty(ref m_availableContractsComplete, value, "AvailableContractsComplete");
				RefilterAvailableContracts();
			}
		}

		public ObservableCollection<MyContractModel> AvailableContracts
		{
			get
			{
				return m_availableContracts;
			}
			set
			{
				SetProperty(ref m_availableContracts, value, "AvailableContracts");
<<<<<<< HEAD
				IsNoAvailableContractVisible = value == null || value.Count == 0;
=======
				IsNoAvailableContractVisible = value == null || ((Collection<MyContractModel>)(object)value).Count == 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public ObservableCollection<MyContractTypeFilterItemModel> FilterTargets
		{
			get
			{
				return m_filterTargets;
			}
			set
			{
				SetProperty(ref m_filterTargets, value, "FilterTargets");
			}
		}

		public int FilterTargetIndex
		{
			get
			{
				return m_filterTargetIndex;
			}
			set
			{
				SetProperty(ref m_filterTargetIndex, value, "FilterTargetIndex");
				RefilterAvailableContracts();
			}
		}

		public MyContractTypeFilterItemModel SelectedFilterTarget
		{
			get
			{
				return m_selectedFilterTarget;
			}
			set
			{
				SetProperty(ref m_selectedFilterTarget, value, "SelectedFilterTarget");
			}
		}

		public MyContractsBlockAdministrationViewModel AdministrationViewModel { get; private set; }

		public MyContractsBlockActiveViewModel ActiveViewModel { get; private set; }

		private void RefilterAvailableContracts()
		{
			if (AvailableContractsComplete == null)
			{
				return;
			}
			MyDefinitionId? myDefinitionId = null;
<<<<<<< HEAD
			if (FilterTargetIndex >= 0 && FilterTargetIndex < FilterTargets.Count)
			{
				myDefinitionId = FilterTargets[FilterTargetIndex].ContractTypeId;
			}
			System.Collections.ObjectModel.ObservableCollection<MyContractModel> observableCollection = new System.Collections.ObjectModel.ObservableCollection<MyContractModel>();
			foreach (MyContractModel item in AvailableContractsComplete)
=======
			if (FilterTargetIndex >= 0 && FilterTargetIndex < ((Collection<MyContractTypeFilterItemModel>)(object)FilterTargets).Count)
			{
				myDefinitionId = ((Collection<MyContractTypeFilterItemModel>)(object)FilterTargets)[FilterTargetIndex].ContractTypeId;
			}
			ObservableCollection<MyContractModel> val = new ObservableCollection<MyContractModel>();
			foreach (MyContractModel item in (Collection<MyContractModel>)(object)AvailableContractsComplete)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyDefinitionId? definitionId = item.DefinitionId;
				if (!myDefinitionId.HasValue || !definitionId.HasValue || myDefinitionId.Value == definitionId.Value)
				{
<<<<<<< HEAD
					observableCollection.Add(item);
				}
			}
			AvailableContracts = observableCollection;
			if (observableCollection.Count > 0)
			{
				SelectedAvailableContract = observableCollection[0];
=======
					((Collection<MyContractModel>)(object)val).Add(item);
				}
			}
			AvailableContracts = val;
			if (((Collection<MyContractModel>)(object)val).Count > 0)
			{
				SelectedAvailableContract = ((Collection<MyContractModel>)(object)val)[0];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public MyContractsBlockViewModel(MyContractBlock contractBlock)
		{
			SortingAvailableContractCommand = new RelayCommand<DataGridSortingEventArgs>(OnSortingAvailableContract);
			AcceptCommand = new RelayCommand(OnAccept);
			RefreshAvailableCommand = new RelayCommand(OnRefreshAvailable);
			m_contractBlock = contractBlock;
			MySession.Static.GetComponent<MySessionComponentContractSystem>();
			IsAdministrationVisible = m_contractBlock.HasLocalPlayerAccess() && m_contractBlock.OwnerId == MySession.Static.LocalPlayerId;
			AdministrationViewModel = new MyContractsBlockAdministrationViewModel(m_contractBlock);
			MyContractsBlockAdministrationViewModel administrationViewModel = AdministrationViewModel;
			administrationViewModel.OnNewContractCreated = (Action)Delegate.Combine(administrationViewModel.OnNewContractCreated, new Action(NewContractCreatedCallback));
			ActiveViewModel = new MyContractsBlockActiveViewModel(m_contractBlock);
			SetDefaultAvailableContractIndex();
			PrepareContractTypeFilter();
		}

		public override void InitializeData()
		{
			m_contractBlock.GetContractBlockStatus(OnGetContractBlockStatus);
			m_contractBlock.GetAvailableContracts(OnGetAvailableContracts);
			if (AdministrationViewModel != null)
			{
				AdministrationViewModel.InitializeData();
			}
			ActiveViewModel.InitializeData();
		}

		public override bool CanExit(object parameter)
		{
			if (!ActiveViewModel.IsVisibleGridSelection)
			{
				return !AdministrationViewModel.IsVisibleAdminSelection;
			}
			return false;
		}

		private void PrepareContractTypeFilter()
		{
			ObservableCollection<MyContractTypeFilterItemModel> val = new ObservableCollection<MyContractTypeFilterItemModel>();
			DictionaryReader<MyDefinitionId, MyContractTypeDefinition> contractTypeDefinitions = MyDefinitionManager.Static.GetContractTypeDefinitions();
			MyContractTypeFilterItemModel item = new MyContractTypeFilterItemModel
			{
				ContractTypeId = null,
				Name = "All",
				LocalizedName = MyTexts.GetString(MySpaceTexts.ContractType_NameLocalizationKey_All)
			};
<<<<<<< HEAD
			observableCollection.Add(item);
=======
			((Collection<MyContractTypeFilterItemModel>)(object)val).Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<MyContractTypeFilterItemModel> list = new List<MyContractTypeFilterItemModel>();
			foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> item2 in contractTypeDefinitions)
			{
				string subtypeName = item2.Key.SubtypeName;
				string @string = MyTexts.GetString($"ContractType_NameLocalizationKey_{subtypeName}");
				BitmapImage bitmapImage = new BitmapImage();
				string[] icons = item2.Value.Icons;
				if (icons != null && icons.Length != 0)
				{
					bitmapImage.TextureAsset = item2.Value.Icons[0];
				}
				list.Add(new MyContractTypeFilterItemModel
				{
					ContractTypeId = item2.Key,
					Name = subtypeName,
					LocalizedName = (string.IsNullOrEmpty(@string) ? subtypeName : @string),
					Icon = bitmapImage
				});
			}
			list.Sort(new MyContractTypeFilterItemModel.MyComparator_LocalizedName());
			foreach (MyContractTypeFilterItemModel item3 in list)
			{
<<<<<<< HEAD
				observableCollection.Add(item3);
=======
				((Collection<MyContractTypeFilterItemModel>)(object)val).Add(item3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			FilterTargets = val;
			FilterTargetIndex = 0;
		}

		private void NewContractCreatedCallback()
		{
			OnRefreshAvailable(null);
		}

		private void SetDefaultAvailableContractIndex()
		{
			if (m_previousSelectedAvailableContractIndex.HasValue)
			{
<<<<<<< HEAD
				if (AvailableContracts == null || AvailableContracts.Count == 0)
				{
					SelectedAvailableContractIndex = -1;
				}
				else if (m_previousSelectedAvailableContractIndex >= AvailableContracts.Count)
				{
					SelectedAvailableContractIndex = AvailableContracts.Count - 1;
=======
				if (AvailableContracts == null || ((Collection<MyContractModel>)(object)AvailableContracts).Count == 0)
				{
					SelectedAvailableContractIndex = -1;
				}
				else if (m_previousSelectedAvailableContractIndex >= ((Collection<MyContractModel>)(object)AvailableContracts).Count)
				{
					SelectedAvailableContractIndex = ((Collection<MyContractModel>)(object)AvailableContracts).Count - 1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					SelectedAvailableContractIndex = m_previousSelectedAvailableContractIndex.Value;
				}
				m_previousSelectedAvailableContractIndex = null;
			}
			else
			{
<<<<<<< HEAD
				SelectedAvailableContractIndex = ((AvailableContracts == null || AvailableContracts.Count <= 0) ? (-1) : 0);
=======
				SelectedAvailableContractIndex = ((AvailableContracts == null || ((Collection<MyContractModel>)(object)AvailableContracts).Count <= 0) ? (-1) : 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (m_selectedAvailableContractIndex > -1)
			{
				SelectedAvailableContract = ((Collection<MyContractModel>)(object)AvailableContracts)[SelectedAvailableContractIndex];
			}
			else
			{
				SelectedAvailableContract = null;
			}
		}

		public void OnGetContractBlockStatus(bool isNpc)
		{
			IsNpc = isNpc;
		}

		public void OnGetAvailableContracts(List<MyObjectBuilder_Contract> contracts)
		{
			ObservableCollection<MyContractModel> val = new ObservableCollection<MyContractModel>();
			foreach (MyObjectBuilder_Contract contract in contracts)
			{
				MyContractModel item = MyContractModelFactory.CreateInstance(contract, showFactionIcons: false);
				((Collection<MyContractModel>)(object)val).Add(item);
			}
			if (string.IsNullOrEmpty(m_lastSortAvailable))
			{
				AvailableContractsComplete = val;
			}
			else
			{
				DataGridSortingEventArgs sortingArgs = new DataGridSortingEventArgs(new DataGridColumn
				{
					SortMemberPath = m_lastSortAvailable,
					SortDirection = ((!m_isLastSortAvailableAsc) ? ListSortDirection.Descending : ListSortDirection.Ascending)
				});
				AvailableContractsComplete = new ObservableCollection<MyContractModel>(SortContracts(val, sortingArgs, out m_lastSortAvailable, out m_isLastSortAvailableAsc));
			}
			SetDefaultAvailableContractIndex();
		}

		private void OnAccept(object obj)
		{
<<<<<<< HEAD
			if (SelectedAvailableContractIndex < 0 || SelectedAvailableContractIndex >= AvailableContracts.Count)
=======
			if (SelectedAvailableContractIndex < 0 || SelectedAvailableContractIndex >= ((Collection<MyContractModel>)(object)AvailableContracts).Count)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			if (ActiveViewModel.ActiveContractCount >= ActiveViewModel.ActiveContractCountMax)
			{
				MyContractResults state = MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard;
				ShowErrorFailNotification(state);
				return;
			}
<<<<<<< HEAD
			MyContractModel cont = m_availableContracts[SelectedAvailableContractIndex];
=======
			MyContractModel cont = ((Collection<MyContractModel>)(object)m_availableContracts)[SelectedAvailableContractIndex];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ShowMessageBoxYesNo(MyTexts.Get(MySpaceTexts.Contracts_AcceptConfirmation_Caption), MyTexts.Get(MySpaceTexts.Contracts_AcceptConfirmation_Text), delegate(MyGuiScreenMessageBox.ResultEnum retval)
			{
				if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					IsWaitingForAccept = true;
					m_contractBlock.AcceptContract(MySession.Static.LocalPlayerId, cont.Id, OnAcceptCallback);
				}
			});
		}

		private void OnRefreshAvailable(object obj)
		{
			m_contractBlock.GetAvailableContracts(OnGetAvailableContracts);
		}

		public void OnAcceptCallback(MyContractResults result)
		{
			m_previousSelectedAvailableContractIndex = SelectedAvailableContractIndex;
			OnRefreshAvailable(null);
			ActiveViewModel.OnRefreshActive(null);
			AdministrationViewModel.OnRefreshAdministrable(null);
			if (result != 0)
			{
				ShowErrorFailNotification(result);
			}
			IsWaitingForAccept = false;
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
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}

		private void ShowMessageBoxYesNo(StringBuilder caption, StringBuilder text, Action<MyGuiScreenMessageBox.ResultEnum> callback)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, text, caption, null, null, null, null, callback, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}

		private void UpdateAccept()
		{
			if (IsWaitingForAccept || SelectedAvailableContractIndex < 0 || SelectedAvailableContractIndex >= ((Collection<MyContractModel>)(object)AvailableContracts).Count)
			{
				IsAcceptEnabled = false;
			}
			else
			{
				IsAcceptEnabled = true;
			}
		}

		private bool CanAccept(object obj)
		{
			if (IsWaitingForAccept || SelectedAvailableContractIndex < 0 || SelectedAvailableContractIndex >= ((Collection<MyContractModel>)(object)AvailableContracts).Count)
			{
				return false;
			}
			return true;
		}

		private void OnSortingAvailableContract(DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyContractModel> enumerable = SortContracts(AvailableContracts, sortingArgs, out m_lastSortAvailable, out m_isLastSortAvailableAsc);
			AvailableContracts = new ObservableCollection<MyContractModel>(enumerable);
			SetDefaultAvailableContractIndex();
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

		public override void OnScreenClosing()
		{
			if (AdministrationViewModel != null)
			{
				MyContractsBlockAdministrationViewModel administrationViewModel = AdministrationViewModel;
				administrationViewModel.OnNewContractCreated = (Action)Delegate.Remove(administrationViewModel.OnNewContractCreated, new Action(NewContractCreatedCallback));
			}
			base.OnScreenClosing();
		}
	}
}
