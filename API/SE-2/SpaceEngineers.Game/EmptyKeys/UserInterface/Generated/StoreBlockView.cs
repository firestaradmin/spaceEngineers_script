using System;
using System.CodeDom.Compiler;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class StoreBlockView : UIRoot
	{
		private Grid rootGrid;

		private Image e_0;

		private Grid e_1;

		private ImageButton e_2;

		private StackPanel e_3;

		private TextBlock e_4;

		private Border e_5;

		private TabControl e_6;

		public StoreBlockView()
		{
			Initialize();
		}

		public StoreBlockView(int width, int height)
			: base(width, height)
		{
			Initialize();
		}

		private void Initialize()
		{
			Style style = RootStyle.CreateRootStyle();
			style.TargetType = GetType();
			base.Style = style;
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			base.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
			base.MessageBoxOverlay = new ColorW(0, 0, 0, 240);
			InitializeElementResources(this);
			rootGrid = new Grid();
			base.Content = rootGrid;
			rootGrid.Name = "rootGrid";
			rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
			RowDefinition rowDefinition = new RowDefinition();
			rowDefinition.Height = new GridLength(0.1f, GridUnitType.Star);
			rootGrid.RowDefinitions.Add(rowDefinition);
			RowDefinition item = new RowDefinition();
			rootGrid.RowDefinitions.Add(item);
			RowDefinition rowDefinition2 = new RowDefinition();
			rowDefinition2.Height = new GridLength(0.1f, GridUnitType.Star);
			rootGrid.RowDefinitions.Add(rowDefinition2);
			Binding binding = new Binding("MaxWidth");
			binding.UseGeneratedBindings = true;
			rootGrid.SetBinding(UIElement.MaxWidthProperty, binding);
			e_0 = new Image();
			rootGrid.Children.Add(e_0);
			e_0.Name = "e_0";
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			e_0.Source = bitmapImage;
			e_0.Stretch = Stretch.Fill;
			Grid.SetRow(e_0, 1);
			Binding binding2 = new Binding("BackgroundOverlay");
			binding2.UseGeneratedBindings = true;
			e_0.SetBinding(ImageBrush.ColorOverlayProperty, binding2);
			e_1 = new Grid();
			rootGrid.Children.Add(e_1);
			e_1.Name = "e_1";
			RowDefinition rowDefinition3 = new RowDefinition();
			rowDefinition3.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition3);
			RowDefinition rowDefinition4 = new RowDefinition();
			rowDefinition4.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition4);
			RowDefinition rowDefinition5 = new RowDefinition();
			rowDefinition5.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition5);
			RowDefinition item2 = new RowDefinition();
			e_1.RowDefinitions.Add(item2);
			RowDefinition rowDefinition6 = new RowDefinition();
			rowDefinition6.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition6);
			RowDefinition rowDefinition7 = new RowDefinition();
			rowDefinition7.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition7);
			RowDefinition rowDefinition8 = new RowDefinition();
			rowDefinition8.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition8);
			RowDefinition rowDefinition9 = new RowDefinition();
			rowDefinition9.Height = new GridLength(1f, GridUnitType.Auto);
			e_1.RowDefinitions.Add(rowDefinition9);
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.Width = new GridLength(75f, GridUnitType.Pixel);
			e_1.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition columnDefinition2 = new ColumnDefinition();
			columnDefinition2.Width = new GridLength(2f, GridUnitType.Star);
			e_1.ColumnDefinitions.Add(columnDefinition2);
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition3.Width = new GridLength(75f, GridUnitType.Pixel);
			e_1.ColumnDefinitions.Add(columnDefinition3);
			Grid.SetRow(e_1, 1);
			e_2 = new ImageButton();
			e_1.Children.Add(e_2);
			e_2.Name = "e_2";
			e_2.Height = 24f;
			e_2.Width = 24f;
			e_2.Margin = new Thickness(16f, 16f, 16f, 16f);
			e_2.HorizontalAlignment = HorizontalAlignment.Right;
			e_2.VerticalAlignment = VerticalAlignment.Center;
			e_2.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage2 = new BitmapImage();
			bitmapImage2.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			e_2.ImageNormal = bitmapImage2;
			BitmapImage bitmapImage3 = new BitmapImage();
			bitmapImage3.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_2.ImageHover = bitmapImage3;
			BitmapImage bitmapImage4 = new BitmapImage();
			bitmapImage4.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_2.ImagePressed = bitmapImage4;
			Grid.SetColumn(e_2, 2);
			Binding binding3 = new Binding("ExitCommand");
			binding3.UseGeneratedBindings = true;
			e_2.SetBinding(Button.CommandProperty, binding3);
			e_3 = new StackPanel();
			e_1.Children.Add(e_3);
			e_3.Name = "e_3";
			Grid.SetColumn(e_3, 1);
			Grid.SetRow(e_3, 1);
			e_4 = new TextBlock();
			e_3.Children.Add(e_4);
			e_4.Name = "e_4";
			e_4.HorizontalAlignment = HorizontalAlignment.Center;
			e_4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionStore");
			e_5 = new Border();
			e_3.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.Height = 2f;
			e_5.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_5.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_6 = new TabControl();
			e_1.Children.Add(e_6);
			e_6.Name = "e_6";
			e_6.TabIndex = 0;
<<<<<<< HEAD
			e_6.ItemsSource = Get_e_6_Items();
=======
			e_6.ItemsSource = (IEnumerable)Get_e_6_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 3);
			Binding binding4 = new Binding("TabSelectedIndex");
			binding4.UseGeneratedBindings = true;
			e_6.SetBinding(Selector.SelectedIndexProperty, binding4);
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "MaxWidth", typeof(MyStoreBlockViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "BackgroundOverlay", typeof(MyStoreBlockViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "ExitCommand", typeof(MyStoreBlockViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsBuyTabVisible", typeof(MyStoreBlockViewModel_IsBuyTabVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "BuyCommand", typeof(MyStoreBlockViewModel_BuyCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "RefreshCommand", typeof(MyStoreBlockViewModel_RefreshCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "OfferedItems", typeof(MyStoreBlockViewModel_OfferedItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SelectedOfferItem", typeof(MyStoreBlockViewModel_SelectedOfferItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SortingOfferedItemsCommand", typeof(MyStoreBlockViewModel_SortingOfferedItemsCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "InventoryTargetViewModel", typeof(MyStoreBlockViewModel_InventoryTargetViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "Inventories", typeof(MyInventoryTargetViewModel_Inventories_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "SelectedInventoryIndex", typeof(MyInventoryTargetViewModel_SelectedInventoryIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "InventoryItems", typeof(MyInventoryTargetViewModel_InventoryItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "CurrentInventoryVolume", typeof(MyInventoryTargetViewModel_CurrentInventoryVolume_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "MaxInventoryVolume", typeof(MyInventoryTargetViewModel_MaxInventoryVolume_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "LocalPlayerCurrency", typeof(MyInventoryTargetViewModel_LocalPlayerCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "CurrencyIcon", typeof(MyInventoryTargetViewModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "BalanceChangeValue", typeof(MyInventoryTargetViewModel_BalanceChangeValue_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "DepositCommand", typeof(MyInventoryTargetViewModel_DepositCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "WithdrawCommand", typeof(MyInventoryTargetViewModel_WithdrawCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsPreviewEnabled", typeof(MyStoreBlockViewModel_IsPreviewEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "ShowPreviewCommand", typeof(MyStoreBlockViewModel_ShowPreviewCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AmountToBuyMaximum", typeof(MyStoreBlockViewModel_AmountToBuyMaximum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AmountToBuy", typeof(MyStoreBlockViewModel_AmountToBuy_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SetAllAmountOfferCommand", typeof(MyStoreBlockViewModel_SetAllAmountOfferCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "TotalPriceToBuy", typeof(MyStoreBlockViewModel_TotalPriceToBuy_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsBuyEnabled", typeof(MyStoreBlockViewModel_IsBuyEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsJoystickLastUsed", typeof(MyStoreBlockViewModel_IsJoystickLastUsed_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsRefreshEnabled", typeof(MyStoreBlockViewModel_IsRefreshEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsSellTabVisible", typeof(MyStoreBlockViewModel_IsSellTabVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "OrderedItems", typeof(MyStoreBlockViewModel_OrderedItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SelectedOrderItem", typeof(MyStoreBlockViewModel_SelectedOrderItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SortingDemandedItemsCommand", typeof(MyStoreBlockViewModel_SortingDemandedItemsCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AmountToSell", typeof(MyStoreBlockViewModel_AmountToSell_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SetAllAmountOrderCommand", typeof(MyStoreBlockViewModel_SetAllAmountOrderCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "TotalPriceToSell", typeof(MyStoreBlockViewModel_TotalPriceToSell_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsSellEnabled", typeof(MyStoreBlockViewModel_IsSellEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SellCommand", typeof(MyStoreBlockViewModel_SellCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsAdministrationVisible", typeof(MyStoreBlockViewModel_IsAdministrationVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AdministrationViewModel", typeof(MyStoreBlockViewModel_AdministrationViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "StoreItems", typeof(MyStoreBlockAdministrationViewModel_StoreItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "SelectedStoreItem", typeof(MyStoreBlockAdministrationViewModel_SelectedStoreItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "RemoveStoreItemCommand", typeof(MyStoreBlockAdministrationViewModel_RemoveStoreItemCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderItems", typeof(MyStoreBlockAdministrationViewModel_OrderItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "SelectedOfferItem", typeof(MyStoreBlockAdministrationViewModel_SelectedOfferItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OfferAmount", typeof(MyStoreBlockAdministrationViewModel_OfferAmount_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OfferPricePerUnitMinimum", typeof(MyStoreBlockAdministrationViewModel_OfferPricePerUnitMinimum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OfferPricePerUnit", typeof(MyStoreBlockAdministrationViewModel_OfferPricePerUnit_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OfferTotalPrice", typeof(MyStoreBlockAdministrationViewModel_OfferTotalPrice_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "CurrencyIcon", typeof(MyStoreBlockAdministrationViewModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OfferListingFee", typeof(MyStoreBlockAdministrationViewModel_OfferListingFee_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OfferTransactionFee", typeof(MyStoreBlockAdministrationViewModel_OfferTransactionFee_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "LocalPlayerCurrency", typeof(MyStoreBlockAdministrationViewModel_LocalPlayerCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "IsCreateOfferEnabled", typeof(MyStoreBlockAdministrationViewModel_IsCreateOfferEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "CreateOfferCommand", typeof(MyStoreBlockAdministrationViewModel_CreateOfferCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "IsTabNewOrderVisible", typeof(MyStoreBlockAdministrationViewModel_IsTabNewOrderVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "SelectedOrderItem", typeof(MyStoreBlockAdministrationViewModel_SelectedOrderItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderAmount", typeof(MyStoreBlockAdministrationViewModel_OrderAmount_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderPricePerUnitMaximum", typeof(MyStoreBlockAdministrationViewModel_OrderPricePerUnitMaximum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderPricePerUnit", typeof(MyStoreBlockAdministrationViewModel_OrderPricePerUnit_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderTotalPrice", typeof(MyStoreBlockAdministrationViewModel_OrderTotalPrice_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderListingFee", typeof(MyStoreBlockAdministrationViewModel_OrderListingFee_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "IsCreateOrderEnabled", typeof(MyStoreBlockAdministrationViewModel_IsCreateOrderEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "CreateOrderCommand", typeof(MyStoreBlockAdministrationViewModel_CreateOrderCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "TabSelectedIndex", typeof(MyStoreBlockViewModel_TabSelectedIndex_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesStoreBlock.Instance);
		}

		private static ObservableCollection<object> Get_e_6_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem = new TabItem
			{
				Name = "e_7"
			};
			tabItem.SetBinding(binding: new Binding("IsBuyTabVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreScreenBuyHeader");
			Grid grid2 = (Grid)(tabItem.Content = new Grid());
			grid2.Name = "e_8";
			GamepadBinding gamepadBinding = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding.SetBinding(binding: new Binding("BuyCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid2.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = grid2;
			GamepadBinding gamepadBinding2 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding2.SetBinding(binding: new Binding("RefreshCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid2.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = grid2;
			RowDefinition item = new RowDefinition();
			grid2.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid2.ColumnDefinitions.Add(item4);
			ColumnDefinition item5 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid2.ColumnDefinitions.Add(item5);
			DataGrid dataGrid = new DataGrid();
			grid2.Children.Add(dataGrid);
			dataGrid.Name = "e_9";
			dataGrid.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			dataGrid.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			dataGrid.TabIndex = 1;
			dataGrid.SelectionMode = DataGridSelectionMode.Single;
			dataGrid.AutoGenerateColumns = false;
			DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn
			{
				Width = 64f,
				SortMemberPath = "Name",
				Header = new TextBlock
				{
					Name = "e_10",
					Margin = new Thickness(2f, 2f, 2f, 2f),
					Text = ""
				}
			};
			Func<UIElement, UIElement> createMethod = e_9_Col0_ct_dtMethod;
			dataGridTemplateColumn.CellTemplate = new DataTemplate(createMethod);
<<<<<<< HEAD
			dataGrid.Columns.Add(dataGridTemplateColumn);
=======
			((Collection<DataGridColumn>)(object)dataGrid.Columns).Add((DataGridColumn)dataGridTemplateColumn);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DataGridTemplateColumn dataGridTemplateColumn2 = new DataGridTemplateColumn
			{
				Width = new DataGridLength(1f, DataGridLengthUnitType.Star),
				SortMemberPath = "Name"
			};
			TextBlock textBlock = new TextBlock();
			textBlock.Name = "e_22";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Name");
			dataGridTemplateColumn2.Header = textBlock;
			Func<UIElement, UIElement> createMethod2 = e_9_Col1_ct_dtMethod;
			dataGridTemplateColumn2.CellTemplate = new DataTemplate(createMethod2);
<<<<<<< HEAD
			dataGrid.Columns.Add(dataGridTemplateColumn2);
=======
			((Collection<DataGridColumn>)(object)dataGrid.Columns).Add((DataGridColumn)dataGridTemplateColumn2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DataGridTemplateColumn dataGridTemplateColumn3 = new DataGridTemplateColumn
			{
				Width = 150f,
				SortMemberPath = "Amount"
			};
			TextBlock textBlock2 = new TextBlock();
			textBlock2.Name = "e_24";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Amount");
			dataGridTemplateColumn3.Header = textBlock2;
			Func<UIElement, UIElement> createMethod3 = e_9_Col2_ct_dtMethod;
			dataGridTemplateColumn3.CellTemplate = new DataTemplate(createMethod3);
<<<<<<< HEAD
			dataGrid.Columns.Add(dataGridTemplateColumn3);
=======
			((Collection<DataGridColumn>)(object)dataGrid.Columns).Add((DataGridColumn)dataGridTemplateColumn3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DataGridTemplateColumn dataGridTemplateColumn4 = new DataGridTemplateColumn
			{
				Width = 200f,
				SortMemberPath = "PricePerUnit"
			};
			TextBlock textBlock3 = new TextBlock();
			textBlock3.Name = "e_26";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_PricePerUnit");
			dataGridTemplateColumn4.Header = textBlock3;
			Func<UIElement, UIElement> createMethod4 = e_9_Col3_ct_dtMethod;
			dataGridTemplateColumn4.CellTemplate = new DataTemplate(createMethod4);
<<<<<<< HEAD
			dataGrid.Columns.Add(dataGridTemplateColumn4);
=======
			((Collection<DataGridColumn>)(object)dataGrid.Columns).Add((DataGridColumn)dataGridTemplateColumn4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			dataGrid.SetBinding(binding: new Binding("OfferedItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			dataGrid.SetBinding(binding: new Binding("SelectedOfferItem")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			dataGrid.SetBinding(binding: new Binding("SortingOfferedItemsCommand")
			{
				UseGeneratedBindings = true
			}, property: DataGrid.SortingCommandProperty);
			InitializeElemente_9Resources(dataGrid);
			Grid grid3 = new Grid();
			grid2.Children.Add(grid3);
			grid3.Name = "e_34";
			grid3.Margin = new Thickness(10f, 0f, 0f, 0f);
			Grid.SetColumn(grid3, 1);
			Grid.SetRow(grid3, 0);
			grid3.SetBinding(binding: new Binding("InventoryTargetViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid4 = new Grid();
			grid3.Children.Add(grid4);
			grid4.Name = "e_35";
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid4.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.RowDefinitions.Add(item10);
			TextBlock textBlock4 = new TextBlock();
			grid4.Children.Add(textBlock4);
			textBlock4.Name = "e_36";
			textBlock4.Margin = new Thickness(0f, 0f, 0f, 5f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(textBlock4, 0);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "StoreScreen_SelectInventory");
			ComboBox comboBox = new ComboBox();
			grid4.Children.Add(comboBox);
			comboBox.Name = "e_37";
			comboBox.TabIndex = 4;
			comboBox.MaxDropDownHeight = 300f;
			Grid.SetRow(comboBox, 1);
			comboBox.SetBinding(binding: new Binding("Inventories")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			comboBox.SetBinding(binding: new Binding("SelectedInventoryIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			ListBox listBox = new ListBox();
			grid4.Children.Add(listBox);
			listBox.Name = "e_38";
			listBox.Margin = new Thickness(0f, 4f, 0f, 0f);
			Style style = new Style(typeof(ListBox));
			Setter item11 = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item11);
			Setter item12 = new Setter(value: new ControlTemplate(createMethod: e_38_s_S_1_ctMethod, targetType: typeof(ListBox)), property: Control.TemplateProperty);
			style.Setters.Add(item12);
			Trigger trigger = new Trigger
			{
				Property = UIElement.IsFocusedProperty,
				Value = true
			};
			Setter item13 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item13);
			style.Triggers.Add(trigger);
			listBox.Style = style;
			listBox.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			listBox.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox.TabIndex = 5;
			Grid.SetRow(listBox, 2);
			DragDrop.SetIsDropTarget(listBox, value: true);
			listBox.SetBinding(binding: new Binding("InventoryItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			InitializeElemente_38Resources(listBox);
			Grid grid5 = new Grid();
			grid4.Children.Add(grid5);
			grid5.Name = "e_43";
			grid5.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item14 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid5.ColumnDefinitions.Add(item14);
			ColumnDefinition item15 = new ColumnDefinition();
			grid5.ColumnDefinitions.Add(item15);
			Grid.SetRow(grid5, 3);
			TextBlock textBlock5 = new TextBlock();
			grid5.Children.Add(textBlock5);
			textBlock5.Name = "e_44";
			textBlock5.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock5.SetResourceReference(TextBlock.TextProperty, "ScreenTerminalInventory_Volume");
			StackPanel stackPanel = new StackPanel();
			grid5.Children.Add(stackPanel);
			stackPanel.Name = "e_45";
			stackPanel.Margin = new Thickness(5f, 5f, 0f, 5f);
			stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_46";
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock6.SetBinding(binding: new Binding("CurrentInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock7 = new TextBlock();
			stackPanel.Children.Add(textBlock7);
			textBlock7.Name = "e_47";
			textBlock7.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock7.Text = "/";
			TextBlock textBlock8 = new TextBlock();
			stackPanel.Children.Add(textBlock8);
			textBlock8.Name = "e_48";
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock8.SetBinding(binding: new Binding("MaxInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid6 = new Grid();
			grid4.Children.Add(grid6);
			grid6.Name = "e_49";
			grid6.Margin = new Thickness(2f, 2f, 2f, 2f);
			RowDefinition item16 = new RowDefinition();
			grid6.RowDefinitions.Add(item16);
			RowDefinition item17 = new RowDefinition();
			grid6.RowDefinitions.Add(item17);
			RowDefinition item18 = new RowDefinition();
			grid6.RowDefinitions.Add(item18);
			ColumnDefinition item19 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.ColumnDefinitions.Add(item19);
			ColumnDefinition item20 = new ColumnDefinition();
			grid6.ColumnDefinitions.Add(item20);
			ColumnDefinition item21 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.ColumnDefinitions.Add(item21);
			Grid.SetRow(grid6, 4);
			TextBlock textBlock9 = new TextBlock();
			grid6.Children.Add(textBlock9);
			textBlock9.Name = "e_50";
			textBlock9.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock9.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock10 = new TextBlock();
			grid6.Children.Add(textBlock10);
			textBlock10.Name = "e_51";
			textBlock10.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock10.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock10, 1);
			textBlock10.SetBinding(binding: new Binding("LocalPlayerCurrency")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image = new Image();
			grid6.Children.Add(image);
			image.Name = "e_52";
			image.Height = 20f;
			image.Margin = new Thickness(4f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			Grid.SetColumn(image, 2);
			image.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock11 = new TextBlock();
			grid6.Children.Add(textBlock11);
			textBlock11.Name = "e_53";
			textBlock11.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock11, 1);
			textBlock11.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_CashBack");
			NumericTextBox numericTextBox = new NumericTextBox();
			grid6.Children.Add(numericTextBox);
			numericTextBox.Name = "e_54";
			numericTextBox.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox.TabIndex = 6;
			numericTextBox.MaxLength = 7;
			numericTextBox.Minimum = 0f;
			numericTextBox.Maximum = 1000000f;
			Grid.SetColumn(numericTextBox, 1);
			Grid.SetRow(numericTextBox, 1);
			numericTextBox.SetBinding(binding: new Binding("BalanceChangeValue")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			Image image2 = new Image();
			grid6.Children.Add(image2);
			image2.Name = "e_55";
			image2.Height = 20f;
			image2.Margin = new Thickness(4f, 2f, 2f, 2f);
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			Grid.SetColumn(image2, 2);
			Grid.SetRow(image2, 1);
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid7 = new Grid();
			grid6.Children.Add(grid7);
			grid7.Name = "e_56";
			ColumnDefinition item22 = new ColumnDefinition();
			grid7.ColumnDefinitions.Add(item22);
			ColumnDefinition item23 = new ColumnDefinition();
			grid7.ColumnDefinitions.Add(item23);
			Grid.SetRow(grid7, 2);
			Grid.SetColumnSpan(grid7, 3);
			Button button = new Button();
			grid7.Children.Add(button);
			button.Name = "e_57";
			button.Margin = new Thickness(0f, 5f, 5f, 5f);
			button.VerticalAlignment = VerticalAlignment.Center;
			button.TabIndex = 7;
			button.SetBinding(binding: new Binding("DepositCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button.SetResourceReference(ContentControl.ContentProperty, "FactionTerminal_Deposit_Currency");
			Button button2 = new Button();
			grid7.Children.Add(button2);
			button2.Name = "e_58";
			button2.Margin = new Thickness(5f, 5f, 0f, 5f);
			button2.VerticalAlignment = VerticalAlignment.Center;
			button2.TabIndex = 8;
			Grid.SetColumn(button2, 1);
			button2.SetBinding(binding: new Binding("WithdrawCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button2.SetResourceReference(ContentControl.ContentProperty, "FactionTerminal_Withdraw_Currency");
			Border border = new Border();
			grid2.Children.Add(border);
			border.Name = "e_59";
			border.Height = 2f;
			border.Margin = new Thickness(0f, 10f, 0f, 10f);
			border.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border, 1);
			Grid.SetColumnSpan(border, 2);
			Grid grid8 = new Grid();
			grid2.Children.Add(grid8);
			grid8.Name = "e_60";
			grid8.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item24 = new ColumnDefinition();
			grid8.ColumnDefinitions.Add(item24);
			ColumnDefinition item25 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item25);
			ColumnDefinition item26 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid8.ColumnDefinitions.Add(item26);
			ColumnDefinition item27 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid8.ColumnDefinitions.Add(item27);
			ColumnDefinition item28 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid8.ColumnDefinitions.Add(item28);
			Grid.SetRow(grid8, 2);
			Grid.SetColumnSpan(grid8, 2);
			Button button3 = new Button();
			grid8.Children.Add(button3);
			button3.Name = "e_61";
			button3.Width = 200f;
			button3.Visibility = Visibility.Collapsed;
			button3.Margin = new Thickness(0f, 10f, 10f, 10f);
			button3.VerticalAlignment = VerticalAlignment.Center;
			button3.SetBinding(binding: new Binding("IsPreviewEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button3.SetBinding(binding: new Binding("ShowPreviewCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button3.SetResourceReference(ContentControl.ContentProperty, "StoreScreen_Preview");
			StackPanel stackPanel2 = new StackPanel();
			grid8.Children.Add(stackPanel2);
			stackPanel2.Name = "e_62";
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel2.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel2, 1);
			TextBlock textBlock12 = new TextBlock();
			stackPanel2.Children.Add(textBlock12);
			textBlock12.Name = "e_63";
			textBlock12.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			textBlock12.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock12, 0);
			textBlock12.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			NumericTextBox numericTextBox2 = new NumericTextBox();
			stackPanel2.Children.Add(numericTextBox2);
			numericTextBox2.Name = "e_64";
			numericTextBox2.Width = 150f;
			numericTextBox2.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox2.TabIndex = 2;
			numericTextBox2.MaxLength = 5;
			numericTextBox2.Minimum = 0f;
			numericTextBox2.SetBinding(binding: new Binding("AmountToBuyMaximum")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.MaximumProperty);
			numericTextBox2.SetBinding(binding: new Binding("AmountToBuy")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			Button button4 = new Button();
			stackPanel2.Children.Add(button4);
			button4.Name = "e_65";
			button4.Width = 75f;
			button4.Margin = new Thickness(5f, 5f, 5f, 5f);
			button4.VerticalAlignment = VerticalAlignment.Center;
			button4.TabIndex = 3;
			button4.SetBinding(binding: new Binding("SetAllAmountOfferCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button4.SetResourceReference(ContentControl.ContentProperty, "StoreScreen_AllButton");
			StackPanel stackPanel3 = new StackPanel();
			grid8.Children.Add(stackPanel3);
			stackPanel3.Name = "e_66";
			stackPanel3.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel3.VerticalAlignment = VerticalAlignment.Center;
			stackPanel3.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel3, 2);
			TextBlock textBlock13 = new TextBlock();
			stackPanel3.Children.Add(textBlock13);
			textBlock13.Name = "e_67";
			textBlock13.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			textBlock13.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock13.SetBinding(binding: new Binding("TotalPriceToBuy")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel3.Children.Add(image3);
			image3.Name = "e_68";
			image3.Width = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			image3.SetBinding(binding: new Binding("InventoryTargetViewModel.CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Button button5 = new Button();
			grid8.Children.Add(button5);
			button5.Name = "e_69";
			button5.Margin = new Thickness(10f, 10f, 0f, 10f);
			button5.VerticalAlignment = VerticalAlignment.Center;
			button5.TabIndex = 9;
			Grid.SetColumn(button5, 3);
			button5.SetBinding(binding: new Binding("IsBuyEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button5.SetBinding(binding: new Binding("BuyCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			StackPanel stackPanel5 = (StackPanel)(button5.Content = new StackPanel());
			stackPanel5.Name = "e_70";
			stackPanel5.HorizontalAlignment = HorizontalAlignment.Center;
			stackPanel5.VerticalAlignment = VerticalAlignment.Center;
			stackPanel5.Orientation = Orientation.Horizontal;
			TextBlock textBlock14 = new TextBlock();
			stackPanel5.Children.Add(textBlock14);
			textBlock14.Name = "e_71";
			textBlock14.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock14.Text = "\ue002";
			textBlock14.SetBinding(binding: new Binding("IsJoystickLastUsed")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			TextBlock textBlock15 = new TextBlock();
			stackPanel5.Children.Add(textBlock15);
			textBlock15.Name = "e_72";
			textBlock15.SetResourceReference(TextBlock.TextProperty, "StoreScreen_BuyButton");
			Button button6 = new Button();
			grid8.Children.Add(button6);
			button6.Name = "e_73";
			button6.Margin = new Thickness(10f, 10f, 0f, 10f);
			button6.VerticalAlignment = VerticalAlignment.Center;
			button6.TabIndex = 10;
			Grid.SetColumn(button6, 4);
			button6.SetBinding(binding: new Binding("IsRefreshEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button6.SetBinding(binding: new Binding("RefreshCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			StackPanel stackPanel7 = (StackPanel)(button6.Content = new StackPanel());
			stackPanel7.Name = "e_74";
			stackPanel7.HorizontalAlignment = HorizontalAlignment.Center;
			stackPanel7.VerticalAlignment = VerticalAlignment.Center;
			stackPanel7.Orientation = Orientation.Horizontal;
			TextBlock textBlock16 = new TextBlock();
			stackPanel7.Children.Add(textBlock16);
			textBlock16.Name = "e_75";
			textBlock16.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock16.Text = "\ue004";
			textBlock16.SetBinding(binding: new Binding("IsJoystickLastUsed")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			TextBlock textBlock17 = new TextBlock();
			stackPanel7.Children.Add(textBlock17);
			textBlock17.Name = "e_76";
			textBlock17.SetResourceReference(TextBlock.TextProperty, "buttonRefresh");
<<<<<<< HEAD
			observableCollection.Add(tabItem);
=======
			((Collection<object>)(object)obj).Add((object)tabItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem2 = new TabItem
			{
				Name = "e_77"
			};
			tabItem2.SetBinding(binding: new Binding("IsSellTabVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreScreenSellHeader");
			Grid grid10 = (Grid)(tabItem2.Content = new Grid());
			grid10.Name = "e_78";
			RowDefinition item29 = new RowDefinition();
			grid10.RowDefinitions.Add(item29);
			RowDefinition item30 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid10.RowDefinitions.Add(item30);
			RowDefinition item31 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid10.RowDefinitions.Add(item31);
			ColumnDefinition item32 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid10.ColumnDefinitions.Add(item32);
			ColumnDefinition item33 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid10.ColumnDefinitions.Add(item33);
			DataGrid dataGrid2 = new DataGrid();
			grid10.Children.Add(dataGrid2);
			dataGrid2.Name = "e_79";
			dataGrid2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			dataGrid2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			dataGrid2.TabIndex = 10;
			dataGrid2.SelectionMode = DataGridSelectionMode.Single;
			dataGrid2.AutoGenerateColumns = false;
			DataGridTemplateColumn dataGridTemplateColumn5 = new DataGridTemplateColumn
			{
				Width = 64f,
				SortMemberPath = "Name",
				Header = new TextBlock
				{
					Name = "e_80",
					Margin = new Thickness(2f, 2f, 2f, 2f),
					Text = ""
				}
			};
			Func<UIElement, UIElement> createMethod6 = e_79_Col0_ct_dtMethod;
			dataGridTemplateColumn5.CellTemplate = new DataTemplate(createMethod6);
<<<<<<< HEAD
			dataGrid2.Columns.Add(dataGridTemplateColumn5);
=======
			((Collection<DataGridColumn>)(object)dataGrid2.Columns).Add((DataGridColumn)dataGridTemplateColumn5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DataGridTemplateColumn dataGridTemplateColumn6 = new DataGridTemplateColumn
			{
				Width = new DataGridLength(1f, DataGridLengthUnitType.Star),
				SortMemberPath = "Name"
			};
			TextBlock textBlock18 = new TextBlock();
			textBlock18.Name = "e_82";
			textBlock18.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock18.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Name");
			dataGridTemplateColumn6.Header = textBlock18;
			Func<UIElement, UIElement> createMethod7 = e_79_Col1_ct_dtMethod;
			dataGridTemplateColumn6.CellTemplate = new DataTemplate(createMethod7);
<<<<<<< HEAD
			dataGrid2.Columns.Add(dataGridTemplateColumn6);
=======
			((Collection<DataGridColumn>)(object)dataGrid2.Columns).Add((DataGridColumn)dataGridTemplateColumn6);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DataGridTemplateColumn dataGridTemplateColumn7 = new DataGridTemplateColumn
			{
				Width = 150f,
				SortMemberPath = "Amount"
			};
			TextBlock textBlock19 = new TextBlock();
			textBlock19.Name = "e_84";
			textBlock19.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock19.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Amount");
			dataGridTemplateColumn7.Header = textBlock19;
			Func<UIElement, UIElement> createMethod8 = e_79_Col2_ct_dtMethod;
			dataGridTemplateColumn7.CellTemplate = new DataTemplate(createMethod8);
<<<<<<< HEAD
			dataGrid2.Columns.Add(dataGridTemplateColumn7);
=======
			((Collection<DataGridColumn>)(object)dataGrid2.Columns).Add((DataGridColumn)dataGridTemplateColumn7);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DataGridTemplateColumn dataGridTemplateColumn8 = new DataGridTemplateColumn
			{
				Width = 200f,
				SortMemberPath = "PricePerUnit"
			};
			TextBlock textBlock20 = new TextBlock();
			textBlock20.Name = "e_86";
			textBlock20.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock20.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_PricePerUnit");
			dataGridTemplateColumn8.Header = textBlock20;
			Func<UIElement, UIElement> createMethod9 = e_79_Col3_ct_dtMethod;
			dataGridTemplateColumn8.CellTemplate = new DataTemplate(createMethod9);
<<<<<<< HEAD
			dataGrid2.Columns.Add(dataGridTemplateColumn8);
=======
			((Collection<DataGridColumn>)(object)dataGrid2.Columns).Add((DataGridColumn)dataGridTemplateColumn8);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			dataGrid2.SetBinding(binding: new Binding("OrderedItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			dataGrid2.SetBinding(binding: new Binding("SelectedOrderItem")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			dataGrid2.SetBinding(binding: new Binding("SortingDemandedItemsCommand")
			{
				UseGeneratedBindings = true
			}, property: DataGrid.SortingCommandProperty);
			Grid grid11 = new Grid();
			grid10.Children.Add(grid11);
			grid11.Name = "e_90";
			grid11.Margin = new Thickness(10f, 0f, 0f, 0f);
			Grid.SetColumn(grid11, 1);
			Grid.SetRow(grid11, 0);
			grid11.SetBinding(binding: new Binding("InventoryTargetViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid12 = new Grid();
			grid11.Children.Add(grid12);
			grid12.Name = "e_91";
			RowDefinition item34 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.RowDefinitions.Add(item34);
			RowDefinition item35 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.RowDefinitions.Add(item35);
			RowDefinition item36 = new RowDefinition();
			grid12.RowDefinitions.Add(item36);
			RowDefinition item37 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.RowDefinitions.Add(item37);
			RowDefinition item38 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.RowDefinitions.Add(item38);
			TextBlock textBlock21 = new TextBlock();
			grid12.Children.Add(textBlock21);
			textBlock21.Name = "e_92";
			textBlock21.Margin = new Thickness(0f, 0f, 0f, 5f);
			textBlock21.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(textBlock21, 0);
			textBlock21.SetResourceReference(TextBlock.TextProperty, "StoreScreen_SelectInventory");
			ComboBox comboBox2 = new ComboBox();
			grid12.Children.Add(comboBox2);
			comboBox2.Name = "e_93";
			comboBox2.TabIndex = 13;
			comboBox2.MaxDropDownHeight = 300f;
			Grid.SetRow(comboBox2, 1);
			comboBox2.SetBinding(binding: new Binding("Inventories")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			comboBox2.SetBinding(binding: new Binding("SelectedInventoryIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			ListBox listBox2 = new ListBox();
			grid12.Children.Add(listBox2);
			listBox2.Name = "e_94";
			listBox2.Margin = new Thickness(0f, 4f, 0f, 0f);
			Style style2 = new Style(typeof(ListBox));
			Setter item39 = new Setter(UIElement.MinHeightProperty, 80f);
			style2.Setters.Add(item39);
			Setter item40 = new Setter(value: new ControlTemplate(createMethod: e_94_s_S_1_ctMethod, targetType: typeof(ListBox)), property: Control.TemplateProperty);
			style2.Setters.Add(item40);
			Trigger trigger2 = new Trigger
			{
				Property = UIElement.IsFocusedProperty,
				Value = true
			};
			Setter item41 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger2.Setters.Add(item41);
			style2.Triggers.Add(trigger2);
			listBox2.Style = style2;
			listBox2.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			listBox2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox2.TabIndex = 14;
			Grid.SetRow(listBox2, 2);
			DragDrop.SetIsDropTarget(listBox2, value: true);
			listBox2.SetBinding(binding: new Binding("InventoryItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			InitializeElemente_94Resources(listBox2);
			Grid grid13 = new Grid();
			grid12.Children.Add(grid13);
			grid13.Name = "e_99";
			grid13.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item42 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid13.ColumnDefinitions.Add(item42);
			ColumnDefinition item43 = new ColumnDefinition();
			grid13.ColumnDefinitions.Add(item43);
			Grid.SetRow(grid13, 3);
			TextBlock textBlock22 = new TextBlock();
			grid13.Children.Add(textBlock22);
			textBlock22.Name = "e_100";
			textBlock22.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock22.VerticalAlignment = VerticalAlignment.Center;
			textBlock22.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock22.SetResourceReference(TextBlock.TextProperty, "ScreenTerminalInventory_Volume");
			StackPanel stackPanel8 = new StackPanel();
			grid13.Children.Add(stackPanel8);
			stackPanel8.Name = "e_101";
			stackPanel8.Margin = new Thickness(5f, 5f, 0f, 5f);
			stackPanel8.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel8.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel8, 1);
			TextBlock textBlock23 = new TextBlock();
			stackPanel8.Children.Add(textBlock23);
			textBlock23.Name = "e_102";
			textBlock23.VerticalAlignment = VerticalAlignment.Center;
			textBlock23.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock23.SetBinding(binding: new Binding("CurrentInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock24 = new TextBlock();
			stackPanel8.Children.Add(textBlock24);
			textBlock24.Name = "e_103";
			textBlock24.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock24.VerticalAlignment = VerticalAlignment.Center;
			textBlock24.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock24.Text = "/";
			TextBlock textBlock25 = new TextBlock();
			stackPanel8.Children.Add(textBlock25);
			textBlock25.Name = "e_104";
			textBlock25.VerticalAlignment = VerticalAlignment.Center;
			textBlock25.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock25.SetBinding(binding: new Binding("MaxInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid14 = new Grid();
			grid12.Children.Add(grid14);
			grid14.Name = "e_105";
			grid14.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item44 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item44);
			ColumnDefinition item45 = new ColumnDefinition();
			grid14.ColumnDefinitions.Add(item45);
			ColumnDefinition item46 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item46);
			Grid.SetRow(grid14, 4);
			TextBlock textBlock26 = new TextBlock();
			grid14.Children.Add(textBlock26);
			textBlock26.Name = "e_106";
			textBlock26.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock26.VerticalAlignment = VerticalAlignment.Center;
			textBlock26.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock26.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock27 = new TextBlock();
			grid14.Children.Add(textBlock27);
			textBlock27.Name = "e_107";
			textBlock27.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock27.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock27.VerticalAlignment = VerticalAlignment.Center;
			textBlock27.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock27, 1);
			textBlock27.SetBinding(binding: new Binding("LocalPlayerCurrency")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			grid14.Children.Add(image4);
			image4.Name = "e_108";
			image4.Height = 20f;
			image4.Margin = new Thickness(4f, 2f, 2f, 2f);
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Stretch = Stretch.Uniform;
			Grid.SetColumn(image4, 2);
			image4.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Border border2 = new Border();
			grid10.Children.Add(border2);
			border2.Name = "e_109";
			border2.Height = 2f;
			border2.Margin = new Thickness(0f, 10f, 0f, 10f);
			border2.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border2, 1);
			Grid.SetColumnSpan(border2, 2);
			Grid grid15 = new Grid();
			grid10.Children.Add(grid15);
			grid15.Name = "e_110";
			grid15.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item47 = new ColumnDefinition();
			grid15.ColumnDefinitions.Add(item47);
			ColumnDefinition item48 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.ColumnDefinitions.Add(item48);
			ColumnDefinition item49 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid15.ColumnDefinitions.Add(item49);
			ColumnDefinition item50 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid15.ColumnDefinitions.Add(item50);
			ColumnDefinition item51 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid15.ColumnDefinitions.Add(item51);
			Grid.SetRow(grid15, 2);
			Grid.SetColumnSpan(grid15, 2);
			StackPanel stackPanel9 = new StackPanel();
			grid15.Children.Add(stackPanel9);
			stackPanel9.Name = "e_111";
			stackPanel9.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel9.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel9, 1);
			TextBlock textBlock28 = new TextBlock();
			stackPanel9.Children.Add(textBlock28);
			textBlock28.Name = "e_112";
			textBlock28.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock28.VerticalAlignment = VerticalAlignment.Center;
			textBlock28.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock28, 0);
			textBlock28.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			NumericTextBox numericTextBox3 = new NumericTextBox();
			stackPanel9.Children.Add(numericTextBox3);
			numericTextBox3.Name = "e_113";
			numericTextBox3.Width = 150f;
			numericTextBox3.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox3.TabIndex = 11;
			numericTextBox3.MaxLength = 5;
			numericTextBox3.Minimum = 0f;
			numericTextBox3.Maximum = 99999f;
			numericTextBox3.SetBinding(binding: new Binding("AmountToSell")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			Button button7 = new Button();
			stackPanel9.Children.Add(button7);
			button7.Name = "e_114";
			button7.Width = 75f;
			button7.Margin = new Thickness(5f, 5f, 5f, 5f);
			button7.VerticalAlignment = VerticalAlignment.Center;
			button7.TabIndex = 12;
			button7.SetBinding(binding: new Binding("SetAllAmountOrderCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button7.SetResourceReference(ContentControl.ContentProperty, "StoreScreen_AllButton");
			StackPanel stackPanel10 = new StackPanel();
			grid15.Children.Add(stackPanel10);
			stackPanel10.Name = "e_115";
			stackPanel10.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel10.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel10.VerticalAlignment = VerticalAlignment.Center;
			stackPanel10.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel10, 2);
			TextBlock textBlock29 = new TextBlock();
			stackPanel10.Children.Add(textBlock29);
			textBlock29.Name = "e_116";
			textBlock29.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock29.VerticalAlignment = VerticalAlignment.Center;
			textBlock29.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock29.SetBinding(binding: new Binding("TotalPriceToSell")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image5 = new Image();
			stackPanel10.Children.Add(image5);
			image5.Name = "e_117";
			image5.Width = 20f;
			image5.Margin = new Thickness(4f, 2f, 2f, 2f);
			image5.VerticalAlignment = VerticalAlignment.Center;
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("InventoryTargetViewModel.CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Button button8 = new Button();
			grid15.Children.Add(button8);
			button8.Name = "e_118";
			button8.Margin = new Thickness(10f, 10f, 0f, 10f);
			button8.VerticalAlignment = VerticalAlignment.Center;
			button8.TabIndex = 15;
			Grid.SetColumn(button8, 3);
			button8.SetBinding(binding: new Binding("IsSellEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button8.SetBinding(binding: new Binding("SellCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button8.SetResourceReference(ContentControl.ContentProperty, "StoreScreen_SellButton");
			Button button9 = new Button();
			grid15.Children.Add(button9);
			button9.Name = "e_119";
			button9.Margin = new Thickness(10f, 10f, 0f, 10f);
			button9.VerticalAlignment = VerticalAlignment.Center;
			button9.TabIndex = 16;
			Grid.SetColumn(button9, 4);
			button9.SetBinding(binding: new Binding("IsRefreshEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button9.SetBinding(binding: new Binding("RefreshCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button9.SetResourceReference(ContentControl.ContentProperty, "buttonRefresh");
<<<<<<< HEAD
			observableCollection.Add(tabItem2);
=======
			((Collection<object>)(object)obj).Add((object)tabItem2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem3 = new TabItem
			{
				Name = "e_120",
				IsTabStop = false
			};
			tabItem3.SetBinding(binding: new Binding("IsAdministrationVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreAdministration");
			Grid grid17 = (Grid)(tabItem3.Content = new Grid());
			grid17.Name = "e_121";
			grid17.Margin = new Thickness(0f, 0f, 0f, 30f);
			RowDefinition item52 = new RowDefinition();
			grid17.RowDefinitions.Add(item52);
			RowDefinition item53 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid17.RowDefinitions.Add(item53);
			ColumnDefinition item54 = new ColumnDefinition();
			grid17.ColumnDefinitions.Add(item54);
			ColumnDefinition item55 = new ColumnDefinition();
			grid17.ColumnDefinitions.Add(item55);
			grid17.SetBinding(binding: new Binding("AdministrationViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			ListBox listBox3 = new ListBox();
			grid17.Children.Add(listBox3);
			listBox3.Name = "e_122";
			listBox3.Margin = new Thickness(0f, 10f, 10f, 0f);
			listBox3.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			listBox3.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox3.TabIndex = 30;
			listBox3.SelectionMode = SelectionMode.Single;
			Grid.SetColumn(listBox3, 0);
			GamepadHelp.SetTabIndexRight(listBox3, 32);
			GamepadHelp.SetTabIndexDown(listBox3, 31);
			listBox3.SetBinding(binding: new Binding("StoreItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			listBox3.SetBinding(binding: new Binding("SelectedStoreItem")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			Button button10 = new Button();
			grid17.Children.Add(button10);
			button10.Name = "e_123";
			button10.Width = 140f;
			button10.Margin = new Thickness(0f, 10f, 10f, 20f);
			button10.HorizontalAlignment = HorizontalAlignment.Right;
			button10.VerticalAlignment = VerticalAlignment.Center;
			button10.TabIndex = 31;
			Grid.SetRow(button10, 1);
			GamepadHelp.SetTabIndexUp(button10, 30);
			button10.SetBinding(binding: new Binding("RemoveStoreItemCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button10.SetResourceReference(ContentControl.ContentProperty, "StoreScreen_CancelButton");
			TabControl tabControl = new TabControl();
			grid17.Children.Add(tabControl);
			tabControl.Name = "e_124";
			tabControl.Margin = new Thickness(2f, 2f, 2f, 2f);
			tabControl.TabIndex = 32;
<<<<<<< HEAD
			tabControl.ItemsSource = Get_e_124_Items();
=======
			tabControl.ItemsSource = (IEnumerable)Get_e_124_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumn(tabControl, 1);
			Grid.SetRowSpan(tabControl, 2);
			GamepadHelp.SetTabIndexLeft(tabControl, 30);
			InitializeElemente_124Resources(tabControl);
<<<<<<< HEAD
			observableCollection.Add(tabItem3);
			return observableCollection;
=======
			((Collection<object>)(object)obj).Add((object)tabItem3);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static UIElement tt_e_13_Method()
		{
			Grid obj = new Grid
			{
				Name = "e_14",
				Width = 320f
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			obj.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			obj.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			obj.RowDefinitions.Add(item3);
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_15";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_16";
			textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock.SetBinding(binding: new Binding("Name"), property: TextBlock.TextProperty);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_17";
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRow(image, 1);
			image.SetBinding(binding: new Binding("TooltipImage"), property: Image.SourceProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_18";
			stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetRow(stackPanel, 2);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_19";
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreScreen_GridTooltip_Pcu");
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_20";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock3.SetBinding(binding: new Binding("PrefabTotalPcu"), property: TextBlock.TextProperty);
			TextBlock textBlock4 = new TextBlock();
			obj.Children.Add(textBlock4);
			textBlock4.Name = "e_21";
			textBlock4.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock4.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock4, 3);
			textBlock4.SetBinding(binding: new Binding("Description"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_9_Col0_ct_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_11",
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_12";
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon"), property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_13";
			grid.HorizontalAlignment = HorizontalAlignment.Stretch;
			grid.VerticalAlignment = VerticalAlignment.Stretch;
			ToolTip toolTip2 = (ToolTip)(grid.ToolTip = new ToolTip());
			toolTip2.Content = tt_e_13_Method();
			grid.SetBinding(binding: new Binding("HasTooltip"), property: UIElement.VisibilityProperty);
			return obj;
		}

		private static UIElement e_9_Col1_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_23",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("Name"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_9_Col2_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_25",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("AmountFormatted"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement tt_e_29_Method()
		{
			StackPanel obj = new StackPanel
			{
				Name = "e_30",
				Orientation = Orientation.Horizontal
			};
			obj.SetBinding(binding: new Binding("HasPricePerUnitDiscount"), property: UIElement.VisibilityProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_31";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetResourceReference(TextBlock.TextProperty, "StoreBlock_OfferDiscount");
			TextBlock textBlock2 = new TextBlock();
			obj.Children.Add(textBlock2);
			textBlock2.Name = "e_32";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.SetBinding(binding: new Binding("PricePerUnitDiscount")
			{
				StringFormat = "{0}%"
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_9_Col3_ct_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_27",
				Margin = new Thickness(0f, 0f, 4f, 0f),
				HorizontalAlignment = HorizontalAlignment.Right,
				Orientation = Orientation.Horizontal
			};
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_28";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("HasNormalPrice"), property: UIElement.VisibilityProperty);
			textBlock.SetBinding(binding: new Binding("PricePerUnitFormatted"), property: TextBlock.TextProperty);
			TextBlock textBlock2 = new TextBlock();
			obj.Children.Add(textBlock2);
			textBlock2.Name = "e_29";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			ToolTip toolTip2 = (ToolTip)(textBlock2.ToolTip = new ToolTip());
			toolTip2.Content = tt_e_29_Method();
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 44, 20, 255));
			textBlock2.SetBinding(binding: new Binding("HasPricePerUnitDiscount"), property: UIElement.VisibilityProperty);
			textBlock2.SetBinding(binding: new Binding("PricePerUnitFormatted"), property: TextBlock.TextProperty);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_33";
			image.Width = 20f;
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("CurrencyIcon"), property: Image.SourceProperty);
			return obj;
		}

		private static void InitializeElemente_9Resources(UIElement elem)
		{
			object obj = elem.Resources[typeof(DataGridRow)];
			Style style = new Style(typeof(DataGridRow), obj as Style);
			EventTrigger eventTrigger = new EventTrigger(Control.MouseDoubleClickEvent);
			style.Triggers.Add(eventTrigger);
			Binding binding = new Binding("ViewModel.OnBuyItemDoubleClickCommand");
			binding.Source = new MyStoreBlockViewModelLocator(isDesignMode: false);
			eventTrigger.SetBinding(EventTrigger.CommandProperty, binding);
			elem.Resources.Add(typeof(DataGridRow), style);
		}

		private static UIElement e_38_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_39",
				SnapsToDevicePixels = true
			};
			obj.SetBinding(binding: new Binding("BorderThickness")
			{
				Source = parent
			}, property: Control.BorderThicknessProperty);
			obj.SetBinding(binding: new Binding("BorderBrush")
			{
				Source = parent
			}, property: Control.BorderBrushProperty);
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ScrollViewer scrollViewer = (ScrollViewer)(obj.Child = new ScrollViewer());
			scrollViewer.Name = "PART_DataScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			WrapPanel wrapPanel2 = (WrapPanel)(scrollViewer.Content = new WrapPanel());
			wrapPanel2.Name = "e_40";
			wrapPanel2.Margin = new Thickness(4f, 4f, 4f, 4f);
			wrapPanel2.IsItemsHost = true;
			wrapPanel2.ItemHeight = 64f;
			wrapPanel2.ItemWidth = 64f;
			return obj;
		}

		private static void InitializeElemente_38Resources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod = e_38r_0_s_S_1_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ListBoxItem), createMethod);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsMouseOverProperty;
			trigger.Value = true;
			Setter item2 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			trigger.Setters.Add(item2);
			controlTemplate.Triggers.Add(trigger);
			Trigger trigger2 = new Trigger();
			trigger2.Property = ListBoxItem.IsSelectedProperty;
			trigger2.Value = true;
			Setter item3 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItemHover"));
			trigger2.Setters.Add(item3);
			controlTemplate.Triggers.Add(trigger2);
			Setter item4 = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item4);
			elem.Resources.Add(typeof(ListBoxItem), style);
		}

		private static UIElement e_38r_0_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_41"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_42";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
			contentPresenter.SetBinding(binding: new Binding(), property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_79_Col0_ct_dtMethod(UIElement parent)
		{
			Image obj = new Image
			{
				Parent = parent,
				Name = "e_81",
				Stretch = Stretch.Fill
			};
			obj.SetBinding(binding: new Binding("Icon"), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_79_Col1_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_83",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("Name"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_79_Col2_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_85",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("AmountFormatted"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_79_Col3_ct_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_87",
				Margin = new Thickness(0f, 0f, 4f, 0f),
				HorizontalAlignment = HorizontalAlignment.Right,
				Orientation = Orientation.Horizontal
			};
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_88";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("PricePerUnitFormatted"), property: TextBlock.TextProperty);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_89";
			image.Width = 20f;
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("CurrencyIcon"), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_94_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_95",
				SnapsToDevicePixels = true
			};
			obj.SetBinding(binding: new Binding("BorderThickness")
			{
				Source = parent
			}, property: Control.BorderThicknessProperty);
			obj.SetBinding(binding: new Binding("BorderBrush")
			{
				Source = parent
			}, property: Control.BorderBrushProperty);
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ScrollViewer scrollViewer = (ScrollViewer)(obj.Child = new ScrollViewer());
			scrollViewer.Name = "PART_DataScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			WrapPanel wrapPanel2 = (WrapPanel)(scrollViewer.Content = new WrapPanel());
			wrapPanel2.Name = "e_96";
			wrapPanel2.Margin = new Thickness(4f, 4f, 4f, 4f);
			wrapPanel2.IsItemsHost = true;
			wrapPanel2.ItemHeight = 64f;
			wrapPanel2.ItemWidth = 64f;
			return obj;
		}

		private static void InitializeElemente_94Resources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod = e_94r_0_s_S_1_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ListBoxItem), createMethod);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsMouseOverProperty;
			trigger.Value = true;
			Setter item2 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			trigger.Setters.Add(item2);
			controlTemplate.Triggers.Add(trigger);
			Trigger trigger2 = new Trigger();
			trigger2.Property = ListBoxItem.IsSelectedProperty;
			trigger2.Value = true;
			Setter item3 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItemHover"));
			trigger2.Setters.Add(item3);
			controlTemplate.Triggers.Add(trigger2);
			Setter item4 = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item4);
			elem.Resources.Add(typeof(ListBoxItem), style);
		}

		private static UIElement e_94r_0_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_97"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_98";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
			contentPresenter.SetBinding(binding: new Binding(), property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static ObservableCollection<object> Get_e_124_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem = new TabItem();
			tabItem.Name = "e_125";
			tabItem.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreAdministration_NewOffer");
			Grid grid2 = (Grid)(tabItem.Content = new Grid());
			grid2.Name = "e_126";
			RowDefinition item = new RowDefinition();
			grid2.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition();
			grid2.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item3);
			RowDefinition item4 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item4);
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item5);
			ListBox listBox = new ListBox();
			grid2.Children.Add(listBox);
			listBox.Name = "e_127";
			listBox.Margin = new Thickness(0f, 10f, 10f, 10f);
			listBox.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			listBox.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox.TabIndex = 130;
			Grid.SetColumn(listBox, 0);
			GamepadHelp.SetTabIndexLeft(listBox, 30);
			GamepadHelp.SetTabIndexDown(listBox, 131);
			listBox.SetBinding(binding: new Binding("OrderItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			listBox.SetBinding(binding: new Binding("SelectedOfferItem")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			Grid grid3 = new Grid();
			grid2.Children.Add(grid3);
			grid3.Name = "e_128";
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item10);
			RowDefinition item11 = new RowDefinition();
			grid3.RowDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.ColumnDefinitions.Add(item12);
			ColumnDefinition item13 = new ColumnDefinition();
			grid3.ColumnDefinitions.Add(item13);
			Grid.SetColumn(grid3, 0);
			Grid.SetRow(grid3, 1);
			TextBlock textBlock = new TextBlock();
			grid3.Children.Add(textBlock);
			textBlock.Name = "e_129";
			textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			NumericTextBox numericTextBox = new NumericTextBox();
			grid3.Children.Add(numericTextBox);
			numericTextBox.Name = "e_130";
			numericTextBox.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox.TabIndex = 131;
			numericTextBox.MaxLength = 5;
			numericTextBox.Minimum = 0f;
			numericTextBox.Maximum = 99999f;
			Grid.SetColumn(numericTextBox, 1);
			GamepadHelp.SetTabIndexLeft(numericTextBox, 30);
			GamepadHelp.SetTabIndexUp(numericTextBox, 130);
			GamepadHelp.SetTabIndexDown(numericTextBox, 132);
			numericTextBox.SetBinding(binding: new Binding("OfferAmount")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			TextBlock textBlock2 = new TextBlock();
			grid3.Children.Add(textBlock2);
			textBlock2.Name = "e_131";
			textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock2, 1);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_PricePerUnitLabel");
			NumericTextBox numericTextBox2 = new NumericTextBox();
			grid3.Children.Add(numericTextBox2);
			numericTextBox2.Name = "e_132";
			numericTextBox2.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox2.TabIndex = 132;
			numericTextBox2.MaxLength = 9;
			numericTextBox2.Maximum = 1E+09f;
			Grid.SetColumn(numericTextBox2, 1);
			Grid.SetRow(numericTextBox2, 1);
			GamepadHelp.SetTabIndexLeft(numericTextBox2, 30);
			GamepadHelp.SetTabIndexUp(numericTextBox2, 131);
			GamepadHelp.SetTabIndexDown(numericTextBox2, 133);
			numericTextBox2.SetBinding(binding: new Binding("OfferPricePerUnitMinimum")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.MinimumProperty);
			numericTextBox2.SetBinding(binding: new Binding("OfferPricePerUnit")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			TextBlock textBlock3 = new TextBlock();
			grid3.Children.Add(textBlock3);
			textBlock3.Name = "e_133";
			textBlock3.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 0);
			Grid.SetRow(textBlock3, 2);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_TotalPriceLabel");
			StackPanel stackPanel = new StackPanel();
			grid3.Children.Add(stackPanel);
			stackPanel.Name = "e_134";
			stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 2);
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_135";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.SetBinding(binding: new Binding("OfferTotalPrice")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image = new Image();
			stackPanel.Children.Add(image);
			image.Name = "e_136";
			image.Height = 20f;
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock5 = new TextBlock();
			grid3.Children.Add(textBlock5);
			textBlock5.Name = "e_137";
			textBlock5.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 0);
			Grid.SetRow(textBlock5, 3);
			textBlock5.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_ListingFee");
			StackPanel stackPanel2 = new StackPanel();
			grid3.Children.Add(stackPanel2);
			stackPanel2.Name = "e_138";
			stackPanel2.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel2.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel2, 1);
			Grid.SetRow(stackPanel2, 3);
			TextBlock textBlock6 = new TextBlock();
			stackPanel2.Children.Add(textBlock6);
			textBlock6.Name = "e_139";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock6.SetBinding(binding: new Binding("OfferListingFee")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel2.Children.Add(image2);
			image2.Name = "e_140";
			image2.Height = 20f;
			image2.Margin = new Thickness(2f, 2f, 2f, 2f);
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock7 = new TextBlock();
			grid3.Children.Add(textBlock7);
			textBlock7.Name = "e_141";
			textBlock7.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 0);
			Grid.SetRow(textBlock7, 4);
			textBlock7.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_TransactionFee");
			StackPanel stackPanel3 = new StackPanel();
			grid3.Children.Add(stackPanel3);
			stackPanel3.Name = "e_142";
			stackPanel3.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel3.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel3, 1);
			Grid.SetRow(stackPanel3, 4);
			TextBlock textBlock8 = new TextBlock();
			stackPanel3.Children.Add(textBlock8);
			textBlock8.Name = "e_143";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock8.SetBinding(binding: new Binding("OfferTransactionFee")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel3.Children.Add(image3);
			image3.Name = "e_144";
			image3.Height = 20f;
			image3.Margin = new Thickness(2f, 2f, 2f, 2f);
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid4 = new Grid();
			grid3.Children.Add(grid4);
			grid4.Name = "e_145";
			grid4.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item14 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item14);
			ColumnDefinition item15 = new ColumnDefinition();
			grid4.ColumnDefinitions.Add(item15);
			ColumnDefinition item16 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item16);
			Grid.SetRow(grid4, 5);
			Grid.SetColumnSpan(grid4, 2);
			TextBlock textBlock9 = new TextBlock();
			grid4.Children.Add(textBlock9);
			textBlock9.Name = "e_146";
			textBlock9.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock9.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock9.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock10 = new TextBlock();
			grid4.Children.Add(textBlock10);
			textBlock10.Name = "e_147";
			textBlock10.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock10.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock10.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock10, 1);
			textBlock10.SetBinding(binding: new Binding("LocalPlayerCurrency")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			grid4.Children.Add(image4);
			image4.Name = "e_148";
			image4.Height = 20f;
			image4.Margin = new Thickness(4f, 2f, 2f, 2f);
			image4.VerticalAlignment = VerticalAlignment.Bottom;
			image4.Stretch = Stretch.Uniform;
			Grid.SetColumn(image4, 2);
			image4.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Button button = new Button();
			grid2.Children.Add(button);
			button.Name = "e_149";
			button.Width = 150f;
			button.Margin = new Thickness(10f, 10f, 0f, 10f);
			button.HorizontalAlignment = HorizontalAlignment.Right;
			button.VerticalAlignment = VerticalAlignment.Center;
			button.TabIndex = 133;
			Grid.SetColumn(button, 0);
			Grid.SetRow(button, 3);
			GamepadHelp.SetTabIndexLeft(button, 30);
			GamepadHelp.SetTabIndexUp(button, 132);
			button.SetBinding(binding: new Binding("IsCreateOfferEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button.SetBinding(binding: new Binding("CreateOfferCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button.SetResourceReference(ContentControl.ContentProperty, "StoreBlockView_CreateOfferButton");
<<<<<<< HEAD
			observableCollection.Add(tabItem);
=======
			((Collection<object>)(object)obj).Add((object)tabItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem2 = new TabItem
			{
				Name = "e_150"
			};
			tabItem2.SetBinding(binding: new Binding("IsTabNewOrderVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreAdministration_NewOrder");
			Grid grid6 = (Grid)(tabItem2.Content = new Grid());
			grid6.Name = "e_151";
			RowDefinition item17 = new RowDefinition();
			grid6.RowDefinitions.Add(item17);
			RowDefinition item18 = new RowDefinition();
			grid6.RowDefinitions.Add(item18);
			RowDefinition item19 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.RowDefinitions.Add(item19);
			RowDefinition item20 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.RowDefinitions.Add(item20);
			RowDefinition item21 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.RowDefinitions.Add(item21);
			ListBox listBox2 = new ListBox();
			grid6.Children.Add(listBox2);
			listBox2.Name = "e_152";
			listBox2.Margin = new Thickness(0f, 10f, 10f, 10f);
			listBox2.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			listBox2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox2.TabIndex = 230;
			Grid.SetColumn(listBox2, 0);
			GamepadHelp.SetTabIndexLeft(listBox2, 30);
			GamepadHelp.SetTabIndexDown(listBox2, 231);
			listBox2.SetBinding(binding: new Binding("OrderItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			listBox2.SetBinding(binding: new Binding("SelectedOrderItem")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			Grid grid7 = new Grid();
			grid6.Children.Add(grid7);
			grid7.Name = "e_153";
			RowDefinition item22 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.RowDefinitions.Add(item22);
			RowDefinition item23 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.RowDefinitions.Add(item23);
			RowDefinition item24 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.RowDefinitions.Add(item24);
			RowDefinition item25 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.RowDefinitions.Add(item25);
			RowDefinition item26 = new RowDefinition();
			grid7.RowDefinitions.Add(item26);
			ColumnDefinition item27 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.ColumnDefinitions.Add(item27);
			ColumnDefinition item28 = new ColumnDefinition();
			grid7.ColumnDefinitions.Add(item28);
			Grid.SetColumn(grid7, 0);
			Grid.SetRow(grid7, 1);
			TextBlock textBlock11 = new TextBlock();
			grid7.Children.Add(textBlock11);
			textBlock11.Name = "e_154";
			textBlock11.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 0);
			textBlock11.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			NumericTextBox numericTextBox3 = new NumericTextBox();
			grid7.Children.Add(numericTextBox3);
			numericTextBox3.Name = "e_155";
			numericTextBox3.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox3.TabIndex = 231;
			numericTextBox3.MaxLength = 5;
			numericTextBox3.Minimum = 0f;
			numericTextBox3.Maximum = 99999f;
			Grid.SetColumn(numericTextBox3, 1);
			GamepadHelp.SetTabIndexLeft(numericTextBox3, 30);
			GamepadHelp.SetTabIndexUp(numericTextBox3, 230);
			GamepadHelp.SetTabIndexDown(numericTextBox3, 232);
			numericTextBox3.SetBinding(binding: new Binding("OrderAmount")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			TextBlock textBlock12 = new TextBlock();
			grid7.Children.Add(textBlock12);
			textBlock12.Name = "e_156";
			textBlock12.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			textBlock12.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock12, 1);
			textBlock12.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_PricePerUnitLabel");
			NumericTextBox numericTextBox4 = new NumericTextBox();
			grid7.Children.Add(numericTextBox4);
			numericTextBox4.Name = "e_157";
			numericTextBox4.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox4.TabIndex = 232;
			numericTextBox4.MaxLength = 9;
			numericTextBox4.Minimum = 0f;
			Grid.SetColumn(numericTextBox4, 1);
			Grid.SetRow(numericTextBox4, 1);
			GamepadHelp.SetTabIndexLeft(numericTextBox4, 30);
			GamepadHelp.SetTabIndexUp(numericTextBox4, 231);
			GamepadHelp.SetTabIndexDown(numericTextBox4, 233);
			numericTextBox4.SetBinding(binding: new Binding("OrderPricePerUnitMaximum")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.MaximumProperty);
			numericTextBox4.SetBinding(binding: new Binding("OrderPricePerUnit")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			TextBlock textBlock13 = new TextBlock();
			grid7.Children.Add(textBlock13);
			textBlock13.Name = "e_158";
			textBlock13.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			textBlock13.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock13, 0);
			Grid.SetRow(textBlock13, 2);
			textBlock13.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_TotalPriceLabel");
			StackPanel stackPanel4 = new StackPanel();
			grid7.Children.Add(stackPanel4);
			stackPanel4.Name = "e_159";
			stackPanel4.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel4.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel4.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel4, 1);
			Grid.SetRow(stackPanel4, 2);
			TextBlock textBlock14 = new TextBlock();
			stackPanel4.Children.Add(textBlock14);
			textBlock14.Name = "e_160";
			textBlock14.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock14.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock14.VerticalAlignment = VerticalAlignment.Center;
			textBlock14.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock14.SetBinding(binding: new Binding("OrderTotalPrice")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image5 = new Image();
			stackPanel4.Children.Add(image5);
			image5.Name = "e_161";
			image5.Height = 20f;
			image5.Margin = new Thickness(2f, 2f, 2f, 2f);
			image5.VerticalAlignment = VerticalAlignment.Center;
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock15 = new TextBlock();
			grid7.Children.Add(textBlock15);
			textBlock15.Name = "e_162";
			textBlock15.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock15.VerticalAlignment = VerticalAlignment.Center;
			textBlock15.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock15, 0);
			Grid.SetRow(textBlock15, 3);
			textBlock15.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_ListingFee");
			StackPanel stackPanel5 = new StackPanel();
			grid7.Children.Add(stackPanel5);
			stackPanel5.Name = "e_163";
			stackPanel5.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel5.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel5.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel5, 1);
			Grid.SetRow(stackPanel5, 3);
			TextBlock textBlock16 = new TextBlock();
			stackPanel5.Children.Add(textBlock16);
			textBlock16.Name = "e_164";
			textBlock16.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock16.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock16.VerticalAlignment = VerticalAlignment.Center;
			textBlock16.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock16.SetBinding(binding: new Binding("OrderListingFee")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image6 = new Image();
			stackPanel5.Children.Add(image6);
			image6.Name = "e_165";
			image6.Height = 20f;
			image6.Margin = new Thickness(2f, 2f, 2f, 2f);
			image6.VerticalAlignment = VerticalAlignment.Center;
			image6.Stretch = Stretch.Uniform;
			image6.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid8 = new Grid();
			grid7.Children.Add(grid8);
			grid8.Name = "e_166";
			grid8.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item29 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item29);
			ColumnDefinition item30 = new ColumnDefinition();
			grid8.ColumnDefinitions.Add(item30);
			ColumnDefinition item31 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item31);
			Grid.SetRow(grid8, 4);
			Grid.SetColumnSpan(grid8, 2);
			TextBlock textBlock17 = new TextBlock();
			grid8.Children.Add(textBlock17);
			textBlock17.Name = "e_167";
			textBlock17.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock17.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock17.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock17.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock18 = new TextBlock();
			grid8.Children.Add(textBlock18);
			textBlock18.Name = "e_168";
			textBlock18.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock18.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock18.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock18.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock18, 1);
			textBlock18.SetBinding(binding: new Binding("LocalPlayerCurrency")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image7 = new Image();
			grid8.Children.Add(image7);
			image7.Name = "e_169";
			image7.Height = 20f;
			image7.Margin = new Thickness(4f, 2f, 2f, 2f);
			image7.VerticalAlignment = VerticalAlignment.Bottom;
			image7.Stretch = Stretch.Uniform;
			Grid.SetColumn(image7, 2);
			image7.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Button button2 = new Button();
			grid6.Children.Add(button2);
			button2.Name = "e_170";
			button2.Width = 150f;
			button2.Margin = new Thickness(10f, 10f, 0f, 10f);
			button2.HorizontalAlignment = HorizontalAlignment.Right;
			button2.VerticalAlignment = VerticalAlignment.Center;
			button2.TabIndex = 233;
			Grid.SetColumn(button2, 0);
			Grid.SetRow(button2, 3);
			GamepadHelp.SetTabIndexLeft(button2, 30);
			GamepadHelp.SetTabIndexUp(button2, 232);
			button2.SetBinding(binding: new Binding("IsCreateOrderEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button2.SetBinding(binding: new Binding("CreateOrderCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button2.SetResourceReference(ContentControl.ContentProperty, "StoreBlockView_CreateOrderButton");
<<<<<<< HEAD
			observableCollection.Add(tabItem2);
			return observableCollection;
=======
			((Collection<object>)(object)obj).Add((object)tabItem2);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void InitializeElemente_124Resources(UIElement elem)
		{
			object obj = elem.Resources[typeof(TabControl)];
			Style style = new Style(typeof(TabControl), obj as Style);
			Setter item = new Setter(Control.BorderThicknessProperty, new Thickness(0f));
			style.Setters.Add(item);
			Setter item2 = new Setter(Control.PaddingProperty, new Thickness(0f));
			style.Setters.Add(item2);
			Setter item3 = new Setter(Control.BorderBrushProperty, new SolidColorBrush(new ColorW(255, 255, 255, 0)));
			style.Setters.Add(item3);
			elem.Resources.Add(typeof(TabControl), style);
		}
	}
}
