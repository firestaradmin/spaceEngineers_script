using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Game.Screens.Models;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyWorkshopBrowserViewModel : MyViewModelBase
	{
		private readonly uint m_maxCachedPages = 5u;

		private readonly int m_textureCheckFrame = 5;

		private int m_selectedSortIndex;

		private int m_selectedCategoryIndex;

		private int m_loadedImages;

		private int m_currentFrame;

		private List<string> m_textureList = new List<string>();

		private ConcurrentQueue<string> m_loadingTexturesQueue;

		private ConcurrentDictionary<string, MyWorkshopItemModel> m_textureModelsCache = new ConcurrentDictionary<string, MyWorkshopItemModel>();

		private MyWorkshopQuery m_query;

		private uint m_queryStartPage;

		private uint m_currentPage;

		private uint m_totalPages;

		private bool m_isQueryFinished;

		private bool m_isDetailVisible;

		private bool m_isSearchLabelVisible;

		private bool m_isFilterDirty;

		private bool m_isNotFoundTextVisible;

		private bool m_isPagingInfoVisible;

		private string m_searchText;

		private ConcurrentDictionary<uint, List<MyWorkshopItemModel>> m_pagesData = new ConcurrentDictionary<uint, List<MyWorkshopItemModel>>();

		private ObservableCollection<MyWorkshopItemModel> m_workshopItems;

		private MyWorkshopItemModel m_selectedWorkshopItem;

		private List<MyModCategoryModel> m_categories;

		private ICommand m_previousPageCommand;

		private ICommand m_nextPageCommand;

		private ICommand m_refreshCommand;

		private ICommand m_serviceCommand;

		private ICommand m_subscribeCommand;

		private ICommand m_browseWorkshopCommand;

		private ICommand m_toggleSubscriptionCommand;

		private ICommand m_clearSearchTextCommand;

		private ICommand m_openItemInWorkshopCommand;

		private IMyUGCService m_service;

		private bool m_isWorkshopAggregator;

		private int m_categoryControlTabIndexRight;

		private int m_searchControlTabIndexLeft;

		private bool m_isService0Checked;

		private bool m_isService1Checked;

		public Action OnItemSubscriptionChanged;

		private bool m_service0;

		public string AdditionalTag { get; set; }

		public int SelectedCategoryIndex
		{
			get
			{
				return m_selectedCategoryIndex;
			}
			set
			{
				SetProperty(ref m_selectedCategoryIndex, value, "SelectedCategoryIndex");
			}
		}

		public int SelectedSortIndex
		{
			get
			{
				return m_selectedSortIndex;
			}
			set
			{
				if (m_selectedSortIndex != value)
				{
					SetProperty(ref m_selectedSortIndex, value, "SelectedSortIndex");
				}
				if (IsQueryFinished)
				{
					RefreshData();
				}
				else
				{
					m_isFilterDirty = true;
				}
			}
		}

		public uint CurrentPage
		{
			get
			{
				return m_currentPage;
			}
			set
			{
				SetProperty(ref m_currentPage, value, "CurrentPage");
			}
		}

		public uint TotalPages
		{
			get
			{
				return m_totalPages;
			}
			set
			{
				SetProperty(ref m_totalPages, value, "TotalPages");
			}
		}

		public bool IsDetailVisible
		{
			get
			{
				return m_isDetailVisible;
			}
			set
			{
				SetProperty(ref m_isDetailVisible, value, "IsDetailVisible");
			}
		}

		public bool IsSearchLabelVisible
		{
			get
			{
				return m_isSearchLabelVisible;
			}
			set
			{
				SetProperty(ref m_isSearchLabelVisible, value, "IsSearchLabelVisible");
			}
		}

		public bool IsQueryFinished
		{
			get
			{
				return m_isQueryFinished;
			}
			set
			{
				SetProperty(ref m_isQueryFinished, value, "IsQueryFinished");
				RaisePropertyChanged("IsRefreshing");
			}
		}

		public bool IsRefreshing => !IsQueryFinished;

		public bool IsWorkshopAggregator
		{
			get
			{
				return m_isWorkshopAggregator;
			}
			private set
			{
				m_isWorkshopAggregator = value;
				RaisePropertyChanged("IsWorkshopAggregator");
			}
		}

		public int CategoryControlTabIndexRight
		{
			get
			{
				return m_categoryControlTabIndexRight;
			}
			private set
			{
				m_categoryControlTabIndexRight = value;
				RaisePropertyChanged("CategoryControlTabIndexRight");
			}
		}

		public int SearchControlTabIndexLeft
		{
			get
			{
				return m_searchControlTabIndexLeft;
			}
			private set
			{
				m_searchControlTabIndexLeft = value;
				RaisePropertyChanged("SearchControlTabIndexLeft");
			}
		}

		public bool Service0IsChecked
		{
			get
			{
				return m_isService0Checked;
			}
			set
			{
				SetProperty(ref m_isService0Checked, value, "Service0IsChecked");
			}
		}

		public bool Service1IsChecked
		{
			get
			{
				return m_isService1Checked;
			}
			set
			{
				SetProperty(ref m_isService1Checked, value, "Service1IsChecked");
			}
		}

		public string SearchText
		{
			get
			{
				return m_searchText;
			}
			set
			{
				SetProperty(ref m_searchText, value, "SearchText");
				IsSearchLabelVisible = string.IsNullOrEmpty(m_searchText);
<<<<<<< HEAD
				m_isFilterDirty = true;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (IsQueryFinished)
				{
					RefreshData();
				}
<<<<<<< HEAD
=======
				else
				{
					m_isFilterDirty = true;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public MyWorkshopItemModel SelectedWorkshopItem
		{
			get
			{
				return m_selectedWorkshopItem;
			}
			set
			{
				SetProperty(ref m_selectedWorkshopItem, value, "SelectedWorkshopItem");
				IsDetailVisible = m_selectedWorkshopItem != null;
			}
		}

		public ObservableCollection<MyWorkshopItemModel> WorkshopItems
		{
			get
			{
				return m_workshopItems;
			}
			set
			{
				SetProperty(ref m_workshopItems, value, "WorkshopItems");
			}
		}

		public List<MyModCategoryModel> Categories
		{
			get
			{
				return m_categories;
			}
			set
			{
				SetProperty(ref m_categories, value, "Categories");
			}
		}

		public ICommand NextPageCommand
		{
			get
			{
				return m_nextPageCommand;
			}
			set
			{
				SetProperty(ref m_nextPageCommand, value, "NextPageCommand");
			}
		}

		public ICommand PreviousPageCommand
		{
			get
			{
				return m_previousPageCommand;
			}
			set
			{
				SetProperty(ref m_previousPageCommand, value, "PreviousPageCommand");
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

		public ICommand ServiceCommand
		{
			get
			{
				return m_serviceCommand;
			}
			set
			{
				SetProperty(ref m_serviceCommand, value, "ServiceCommand");
			}
		}

		public ICommand SubscribeCommand
		{
			get
			{
				return m_subscribeCommand;
			}
			set
			{
				SetProperty(ref m_subscribeCommand, value, "SubscribeCommand");
			}
		}

		public ICommand BrowseWorkshopCommand
		{
			get
			{
				return m_browseWorkshopCommand;
			}
			set
			{
				SetProperty(ref m_browseWorkshopCommand, value, "BrowseWorkshopCommand");
			}
		}

		public ICommand ToggleSubscriptionCommand
		{
			get
			{
				return m_toggleSubscriptionCommand;
			}
			set
			{
				SetProperty(ref m_toggleSubscriptionCommand, value, "ToggleSubscriptionCommand");
			}
		}

		public ICommand ClearSearchTextCommand
		{
			get
			{
				return m_clearSearchTextCommand;
			}
			set
			{
				SetProperty(ref m_clearSearchTextCommand, value, "ClearSearchTextCommand");
			}
		}

		public ICommand OpenItemInWorkshopCommand
		{
			get
			{
				return m_openItemInWorkshopCommand;
			}
			set
			{
				SetProperty(ref m_openItemInWorkshopCommand, value, "OpenItemInWorkshopCommand");
			}
		}

		public bool IsNotFoundTextVisible
		{
			get
			{
				return m_isNotFoundTextVisible;
			}
			set
			{
				SetProperty(ref m_isNotFoundTextVisible, value, "IsNotFoundTextVisible");
				IsPagingInfoVisible = !IsNotFoundTextVisible;
			}
		}

		public bool IsPagingInfoVisible
		{
			get
			{
				return m_isPagingInfoVisible;
			}
			set
			{
				SetProperty(ref m_isPagingInfoVisible, value, "IsPagingInfoVisible");
			}
		}

		public MyWorkshopBrowserViewModel()
		{
			if (!MyGameService.AtLeastOneUGCServiceConsented)
			{
				OnWorkshopConsentClick(OnAgree, OnDisagree);
			}
			CurrentPage = 1u;
			IsSearchLabelVisible = true;
			NextPageCommand = new RelayCommand(OnNextPage);
			PreviousPageCommand = new RelayCommand(OnPreviousPage);
			RefreshCommand = new RelayCommand(OnRefresh);
			ServiceCommand = new RelayCommand(OnService);
			SubscribeCommand = new RelayCommand(OnSubscribe);
			BrowseWorkshopCommand = new RelayCommand(OnBrowseWorkshop);
			OpenItemInWorkshopCommand = new RelayCommand(OnOpenItemInWorkshop);
			ToggleSubscriptionCommand = new RelayCommand(OnToggleSubscription);
			ClearSearchTextCommand = new RelayCommand(OnClearSearchText);
		}

		public override void InitializeData()
		{
			base.InitializeData();
			if (MyGameService.IsActive)
			{
				SelectedSortIndex = 0;
				FillCategories();
				SelectedCategoryIndex = 0;
				TotalPages = 1u;
				IsNotFoundTextVisible = false;
				m_isFilterDirty = false;
				m_service0 = true;
				SetupService();
				UpdateQuery();
				m_queryStartPage = 1u;
				IsQueryFinished = false;
				if (m_query != null)
				{
					m_query.Run(m_queryStartPage);
				}
			}
		}

		private void FillCategories()
		{
<<<<<<< HEAD
=======
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<MyModCategoryModel> list = new List<MyModCategoryModel>();
			MyWorkshop.Category[] array = MyWorkshop.ModCategories;
			if (AdditionalTag == MySteamConstants.TAG_WORLDS)
			{
				array = MyWorkshop.WorldCategories;
			}
			else if (AdditionalTag == MySteamConstants.TAG_BLUEPRINTS)
			{
				array = MyWorkshop.BlueprintCategories;
			}
			else if (AdditionalTag == MySteamConstants.TAG_SCENARIOS)
			{
				array = MyWorkshop.ScenarioCategories;
			}
			MyWorkshop.Category[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				MyWorkshop.Category category = array2[i];
				if (category.IsVisibleForFilter)
				{
					MyModCategoryModel myModCategoryModel = new MyModCategoryModel
					{
						Id = category.Id,
						LocalizedName = MyTexts.GetString(category.LocalizableName)
					};
<<<<<<< HEAD
					myModCategoryModel.PropertyChanged += Model_PropertyChanged;
=======
					myModCategoryModel.PropertyChanged += new PropertyChangedEventHandler(Model_PropertyChanged);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					list.Add(myModCategoryModel);
				}
			}
			Categories = list;
		}

		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
<<<<<<< HEAD
			if (e.PropertyName == "IsChecked")
=======
			if (e.get_PropertyName() == "IsChecked")
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (IsQueryFinished)
				{
					RefreshData();
				}
				else
				{
					m_isFilterDirty = true;
				}
			}
		}

		private void OnPageQueryCompleted(MyGameServiceCallResult result, uint page)
		{
			if (m_query.ItemsPerPage == 0 || m_query.TotalResults == 0 || result != MyGameServiceCallResult.OK || m_isFilterDirty)
			{
				m_query.Stop();
				WorkshopItems = null;
				TotalPages = 0u;
				IsNotFoundTextVisible = true;
<<<<<<< HEAD
				OnQueryCompleted(result);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			TotalPages = Math.Max(1u, m_query.TotalResults / m_query.ItemsPerPage + (uint)((m_query.TotalResults % m_query.ItemsPerPage != 0) ? 1 : 0));
			if (page == m_maxCachedPages + m_currentPage)
			{
				m_query.Stop();
				return;
			}
<<<<<<< HEAD
			List<MyWorkshopItemModel> value = null;
			if (!m_pagesData.TryGetValue(page, out value))
			{
				value = new List<MyWorkshopItemModel>((int)m_query.ItemsPerPage);
				m_pagesData.TryAdd(page, value);
=======
			List<MyWorkshopItemModel> list = null;
			if (!m_pagesData.TryGetValue(page, ref list))
			{
				list = new List<MyWorkshopItemModel>((int)m_query.ItemsPerPage);
				m_pagesData.TryAdd(page, list);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			uint num = m_query.ItemsPerPage * (page - m_queryStartPage);
			int num2 = (int)(m_query.ItemsPerPage * (page - m_queryStartPage + 1));
			for (int i = (int)num; i < num2 && i < m_query.Items.Count; i++)
			{
				MyWorkshopItem workshopItem = m_query.Items[i];
				MyWorkshopItemModel myWorkshopItemModel = new MyWorkshopItemModel
				{
					WorkshopItem = workshopItem
				};
				myWorkshopItemModel.OnIsSubscribedChanged = (Action)Delegate.Combine(myWorkshopItemModel.OnIsSubscribedChanged, new Action(ItemSubscriptionChangedCallback));
<<<<<<< HEAD
				value.Add(myWorkshopItemModel);
			}
			if (CurrentPage == page)
			{
				WorkshopItems = new ObservableCollection<MyWorkshopItemModel>(value);
				DownloadPreviewImages(value);
=======
				list.Add(myWorkshopItemModel);
			}
			if (CurrentPage == page)
			{
				WorkshopItems = new ObservableCollection<MyWorkshopItemModel>(list);
				DownloadPreviewImages(list);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void ItemSubscriptionChangedCallback()
		{
			if (OnItemSubscriptionChanged != null)
			{
				OnItemSubscriptionChanged();
			}
		}

		private void DownloadPreviewImages(List<MyWorkshopItemModel> pageList)
		{
			m_loadedImages = pageList.Count;
			m_textureList.Clear();
			m_loadingTexturesQueue = new ConcurrentQueue<string>();
			m_textureModelsCache.Clear();
			foreach (MyWorkshopItemModel item in pageList)
			{
				string directory = Path.Combine(MyFileSystem.UserDataPath, "WorkshopBrowser");
				item.WorkshopItem.DownloadPreviewImage(directory, delegate(MyWorkshopItem workshopItem, bool result)
				{
					if (result)
					{
						m_textureList.Add(workshopItem.PreviewImageFile);
						m_loadingTexturesQueue.Enqueue(workshopItem.PreviewImageFile);
						m_textureModelsCache.TryAdd(workshopItem.PreviewImageFile, item);
					}
					m_loadedImages--;
					if (m_loadedImages <= 0)
					{
						MyRenderProxy.PreloadTextures(m_textureList, TextureType.GUIWithoutPremultiplyAlpha);
					}
				});
			}
		}

		public override void Update()
		{
			base.Update();
			m_currentFrame++;
<<<<<<< HEAD
			if (m_loadingTexturesQueue == null || !IsQueryFinished || m_currentFrame % m_textureCheckFrame != 0 || !m_loadingTexturesQueue.TryDequeue(out var result))
			{
				return;
			}
			if (MyRenderProxy.IsTextureLoaded(result))
			{
				if (m_textureModelsCache.TryGetValue(result, out var value))
				{
					value.OnDownloadPreviewImageCompleted(value.WorkshopItem, success: true);
=======
			string text = default(string);
			if (m_loadingTexturesQueue == null || !IsQueryFinished || m_currentFrame % m_textureCheckFrame != 0 || !m_loadingTexturesQueue.TryDequeue(ref text))
			{
				return;
			}
			if (MyRenderProxy.IsTextureLoaded(text))
			{
				MyWorkshopItemModel myWorkshopItemModel = default(MyWorkshopItemModel);
				if (m_textureModelsCache.TryGetValue(text, ref myWorkshopItemModel))
				{
					myWorkshopItemModel.OnDownloadPreviewImageCompleted(myWorkshopItemModel.WorkshopItem, success: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			else
			{
<<<<<<< HEAD
				m_loadingTexturesQueue.Enqueue(result);
=======
				m_loadingTexturesQueue.Enqueue(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void OnQueryCompleted(MyGameServiceCallResult result)
		{
			IsQueryFinished = true;
			if (m_isFilterDirty)
			{
				RefreshData();
			}
		}

		private void OnPreviousPage(object obj)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				UpdateWorkshopItemsList(CurrentPage);
			}
		}

		private void OnNextPage(object obj)
		{
			if (TotalPages > CurrentPage)
			{
				CurrentPage++;
				UpdateWorkshopItemsList(CurrentPage);
			}
		}

		private void UpdateWorkshopItemsList(uint page)
		{
			m_isFilterDirty = false;
<<<<<<< HEAD
			List<MyWorkshopItemModel> value = null;
			if (m_pagesData.TryGetValue(page, out value))
			{
				WorkshopItems = new ObservableCollection<MyWorkshopItemModel>(value);
				DownloadPreviewImages(value);
=======
			List<MyWorkshopItemModel> list = null;
			if (m_pagesData.TryGetValue(page, ref list))
			{
				WorkshopItems = new ObservableCollection<MyWorkshopItemModel>(list);
				DownloadPreviewImages(list);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (!m_query.IsRunning)
			{
				m_queryStartPage = CurrentPage;
				UpdateQuery();
				IsNotFoundTextVisible = false;
				m_query.Run(m_queryStartPage);
			}
		}

		private void OnRefresh(object obj)
		{
			RefreshData();
		}

		public void OnService(object obj)
		{
			m_service0 = !m_service0;
			SetupService();
		}

		private void SetupService()
		{
			DisposeQuery();
			Service0IsChecked = m_service0;
			Service1IsChecked = !m_service0;
			if (MyGameService.WorkshopService.GetAggregates().Count > 1)
			{
				m_service = MyGameService.WorkshopService.GetAggregates()[(!m_service0) ? 1 : 0];
				IsWorkshopAggregator = true;
				CategoryControlTabIndexRight = 11;
				SearchControlTabIndexLeft = 12;
			}
			else
			{
				m_service = MyGameService.GetDefaultUGC();
				IsWorkshopAggregator = false;
				CategoryControlTabIndexRight = 4;
				SearchControlTabIndexLeft = 3;
			}
			if (m_currentFrame > 1 && !m_service.IsConsentGiven)
			{
				m_service0 = !m_service0;
				if (m_service.ServiceName == "mod.io")
				{
					OnWorkshopConsentClick(OnAgree, OnDisagree);
				}
			}
			else
			{
				SetupServiceRunQuery();
			}
		}

		private void OnAgree()
		{
			MyScreenManager.CloseScreenNow(ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().GetMyGuiScreenBase(typeof(MyWorkshopBrowserViewModel)));
			MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);
		}

		private void OnDisagree()
		{
			MyScreenManager.CloseScreenNow(ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().GetMyGuiScreenBase(typeof(MyWorkshopBrowserViewModel)));
			MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);
		}

		private void OnWorkshopConsentClick(Action onConsentAgree = null, Action onConsentOptOut = null)
		{
			MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(onConsentAgree, onConsentOptOut);
			IMyGuiScreenFactoryService service = ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>();
			MyScreenManager.CloseScreenNow(service.GetMyGuiScreenBase(typeof(MyWorkshopBrowserViewModel)));
			service.CreateScreen(viewModel);
		}

		private void SetupServiceRunQueryFromConsentScreen()
		{
			m_service0 = !m_service0;
			SetupServiceRunQuery();
		}

		private void SetupServiceRunQuery()
		{
			m_query = m_service.CreateWorkshopQuery();
			m_query.ItemsPerPage = MyPlatformGameSettings.WORKSHOP_BROWSER_ITEMS_PER_PAGE;
			m_query.ItemType = WorkshopItemType.Mod;
			m_query.QueryCompleted += OnQueryCompleted;
			m_query.PageQueryCompleted += OnPageQueryCompleted;
			if (!MySandboxGame.Config.ExperimentalMode)
			{
				if (m_query.ExcludedTags == null)
				{
					m_query.ExcludedTags = new List<string>();
				}
				m_query.ExcludedTags.Add(MySteamConstants.TAG_EXPERIMENTAL);
			}
			RefreshData();
		}

		private void UpdateQuery()
		{
			if (m_query == null)
			{
				return;
			}
			m_query.SearchString = SearchText;
			if (SelectedSortIndex == 3)
			{
				m_query.UserId = MyGameService.UserId;
			}
			else
			{
				m_query.QueryType = (MyWorkshopQueryType)SelectedSortIndex;
				m_query.UserId = 0uL;
			}
			if (m_query.RequiredTags == null)
			{
				m_query.RequiredTags = new List<string>();
			}
			else
			{
				m_query.RequiredTags.Clear();
			}
			foreach (MyModCategoryModel category in Categories)
			{
				if (category.IsChecked)
				{
					m_query.RequiredTags.Add(category.Id);
				}
			}
			if (!string.IsNullOrEmpty(AdditionalTag))
			{
				m_query.RequiredTags.Add(AdditionalTag);
				m_query.RequireAllTags = true;
				if (!MyVRage.Platform.Scripting.IsRuntimeCompilationSupported && AdditionalTag == "mod")
				{
					m_query.RequiredTags.Add(MySteamConstants.TAG_NO_SCRIPTS);
				}
			}
		}

		private void RefreshData()
		{
			if (!m_query.IsRunning)
			{
				CleanUpPagesData();
				CurrentPage = 1u;
				IsQueryFinished = false;
				UpdateWorkshopItemsList(CurrentPage);
			}
		}

		private void CleanUpPagesData()
		{
			if (m_pagesData == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (List<MyWorkshopItemModel> value in m_pagesData.Values)
=======
			foreach (List<MyWorkshopItemModel> value in m_pagesData.get_Values())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				foreach (MyWorkshopItemModel item in value)
				{
					if (item.PreviewImage != null && item.PreviewImage.Texture != null)
					{
						item.PreviewImage.Texture.Dispose();
					}
				}
			}
			m_pagesData.Clear();
		}

		private void OnSubscribe(object obj)
		{
			if (SelectedWorkshopItem != null)
			{
				SelectedWorkshopItem.WorkshopItem.Subscribe();
			}
		}

		private void OnToggleSubscription(object obj)
		{
			if (SelectedWorkshopItem != null)
			{
				SelectedWorkshopItem.IsSubscribed = !SelectedWorkshopItem.IsSubscribed;
			}
		}

		private void OnBrowseWorkshop(object obj)
		{
			MyGuiSandbox.OpenUrlWithFallback(m_service.GetItemListUrl(AdditionalTag), m_service.ServiceName + " Workshop");
		}

		private void OnOpenItemInWorkshop(object obj)
		{
			if (SelectedWorkshopItem != null)
			{
				MyGuiSandbox.OpenUrlWithFallback(SelectedWorkshopItem.WorkshopItem.GetItemUrl(), SelectedWorkshopItem.WorkshopItem.ServiceName + " Workshop");
			}
		}

		private void OnClearSearchText(object obj)
		{
			SearchText = string.Empty;
		}

		private void DisposeQuery()
		{
			if (m_query != null)
			{
				m_query.QueryCompleted -= OnQueryCompleted;
				m_query.PageQueryCompleted -= OnPageQueryCompleted;
				m_query.Stop();
				m_query.Dispose();
			}
		}

		public override void OnScreenClosing()
		{
			CleanUpPagesData();
			DisposeQuery();
			base.OnScreenClosing();
		}
	}
}
