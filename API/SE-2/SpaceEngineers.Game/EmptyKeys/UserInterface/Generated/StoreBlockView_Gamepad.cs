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
using EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class StoreBlockView_Gamepad : UIRoot
	{
		private Grid rootGrid;

		private Image e_0;

		private Grid e_1;

		private ImageButton e_2;

		private StackPanel e_3;

		private TextBlock e_4;

		private Border e_5;

		private TextBlock e_6;

		private TextBlock e_7;

		private TabControl e_8;

		public StoreBlockView_Gamepad()
		{
			Initialize();
		}

		public StoreBlockView_Gamepad(int width, int height)
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
			e_2.IsTabStop = false;
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
			e_6 = new TextBlock();
			e_1.Children.Add(e_6);
			e_6.Name = "e_6";
			e_6.Margin = new Thickness(0f, 5f, 10f, 0f);
			e_6.HorizontalAlignment = HorizontalAlignment.Right;
			e_6.VerticalAlignment = VerticalAlignment.Top;
			e_6.FontFamily = new FontFamily("LargeFont");
			e_6.FontSize = 16.6f;
			Grid.SetRow(e_6, 3);
			e_6.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_TabControl_Left");
			e_7 = new TextBlock();
			e_1.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(10f, 5f, 0f, 0f);
			e_7.HorizontalAlignment = HorizontalAlignment.Left;
			e_7.VerticalAlignment = VerticalAlignment.Top;
			e_7.FontFamily = new FontFamily("LargeFont");
			e_7.FontSize = 16.6f;
			Grid.SetColumn(e_7, 2);
			Grid.SetRow(e_7, 3);
			e_7.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_TabControl_Right");
			e_8 = new TabControl();
			e_1.Children.Add(e_8);
			e_8.Name = "e_8";
			e_8.IsTabStop = false;
<<<<<<< HEAD
			e_8.ItemsSource = Get_e_8_Items();
=======
			e_8.ItemsSource = (IEnumerable)Get_e_8_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumn(e_8, 1);
			Grid.SetRow(e_8, 3);
			Binding binding4 = new Binding("TabSelectedIndex");
			binding4.UseGeneratedBindings = true;
			e_8.SetBinding(Selector.SelectedIndexProperty, binding4);
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			FontManager.Instance.AddFont("LargeFont", 16.6f, FontStyle.Regular, "LargeFont_12.45_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "MaxWidth", typeof(MyStoreBlockViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "BackgroundOverlay", typeof(MyStoreBlockViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "ExitCommand", typeof(MyStoreBlockViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsBuyTabVisible", typeof(MyStoreBlockViewModel_IsBuyTabVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "BuyCommand", typeof(MyStoreBlockViewModel_BuyCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "RefreshCommand", typeof(MyStoreBlockViewModel_RefreshCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SwitchSortOffersCommand", typeof(MyStoreBlockViewModel_SwitchSortOffersCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "PreviousInventoryCommand", typeof(MyStoreBlockViewModel_PreviousInventoryCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "NextInventoryCommand", typeof(MyStoreBlockViewModel_NextInventoryCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "OfferedItems", typeof(MyStoreBlockViewModel_OfferedItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SelectedOfferItem", typeof(MyStoreBlockViewModel_SelectedOfferItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SortingOfferedItemsCommand", typeof(MyStoreBlockViewModel_SortingOfferedItemsCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AmountToBuyMaximum", typeof(MyStoreBlockViewModel_AmountToBuyMaximum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AmountToBuy", typeof(MyStoreBlockViewModel_AmountToBuy_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "TotalPriceToBuy", typeof(MyStoreBlockViewModel_TotalPriceToBuy_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "InventoryTargetViewModel", typeof(MyStoreBlockViewModel_InventoryTargetViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "CurrencyIcon", typeof(MyInventoryTargetViewModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "DepositCommand", typeof(MyInventoryTargetViewModel_DepositCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "WithdrawCommand", typeof(MyInventoryTargetViewModel_WithdrawCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "BalanceChangeValue", typeof(MyInventoryTargetViewModel_BalanceChangeValue_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "Inventories", typeof(MyInventoryTargetViewModel_Inventories_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "SelectedInventoryIndex", typeof(MyInventoryTargetViewModel_SelectedInventoryIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "InventoryItems", typeof(MyInventoryTargetViewModel_InventoryItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "CurrentInventoryVolume", typeof(MyInventoryTargetViewModel_CurrentInventoryVolume_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "MaxInventoryVolume", typeof(MyInventoryTargetViewModel_MaxInventoryVolume_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "LocalPlayerCurrency", typeof(MyInventoryTargetViewModel_LocalPlayerCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SortModeOffersText", typeof(MyStoreBlockViewModel_SortModeOffersText_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsSellTabVisible", typeof(MyStoreBlockViewModel_IsSellTabVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SellCommand", typeof(MyStoreBlockViewModel_SellCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SwitchSortOrdersCommand", typeof(MyStoreBlockViewModel_SwitchSortOrdersCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "OrderedItems", typeof(MyStoreBlockViewModel_OrderedItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SelectedOrderItem", typeof(MyStoreBlockViewModel_SelectedOrderItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SortingDemandedItemsCommand", typeof(MyStoreBlockViewModel_SortingDemandedItemsCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AmountToSell", typeof(MyStoreBlockViewModel_AmountToSell_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "TotalPriceToSell", typeof(MyStoreBlockViewModel_TotalPriceToSell_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "SortModeOrdersText", typeof(MyStoreBlockViewModel_SortModeOrdersText_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "IsAdministrationVisible", typeof(MyStoreBlockViewModel_IsAdministrationVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "AdministrationViewModel", typeof(MyStoreBlockViewModel_AdministrationViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "RemoveStoreItemCommand", typeof(MyStoreBlockAdministrationViewModel_RemoveStoreItemCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "StoreItems", typeof(MyStoreBlockAdministrationViewModel_StoreItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "SelectedStoreItem", typeof(MyStoreBlockAdministrationViewModel_SelectedStoreItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "CreateOfferCommand", typeof(MyStoreBlockAdministrationViewModel_CreateOfferCommand_PropertyInfo));
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
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "IsTabNewOrderVisible", typeof(MyStoreBlockAdministrationViewModel_IsTabNewOrderVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "CreateOrderCommand", typeof(MyStoreBlockAdministrationViewModel_CreateOrderCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "SelectedOrderItem", typeof(MyStoreBlockAdministrationViewModel_SelectedOrderItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderAmount", typeof(MyStoreBlockAdministrationViewModel_OrderAmount_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderPricePerUnitMaximum", typeof(MyStoreBlockAdministrationViewModel_OrderPricePerUnitMaximum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderPricePerUnit", typeof(MyStoreBlockAdministrationViewModel_OrderPricePerUnit_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderTotalPrice", typeof(MyStoreBlockAdministrationViewModel_OrderTotalPrice_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockAdministrationViewModel), "OrderListingFee", typeof(MyStoreBlockAdministrationViewModel_OrderListingFee_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "TabSelectedIndex", typeof(MyStoreBlockViewModel_TabSelectedIndex_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesStoreBlock.Instance);
		}

		private static ObservableCollection<object> Get_e_8_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem = new TabItem
			{
				Name = "e_9",
				IsTabStop = false
			};
			tabItem.SetBinding(binding: new Binding("IsBuyTabVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreScreenBuyHeader");
			Grid grid2 = (Grid)(tabItem.Content = new Grid());
			grid2.Name = "e_10";
			RowDefinition item = new RowDefinition();
			grid2.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(65f, GridUnitType.Pixel)
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
			Grid grid3 = new Grid();
			grid2.Children.Add(grid3);
			grid3.Name = "e_11";
			GamepadBinding gamepadBinding = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding.SetBinding(binding: new Binding("BuyCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid3.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = grid3;
			GamepadBinding gamepadBinding2 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding2.SetBinding(binding: new Binding("RefreshCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid3.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = grid3;
			RowDefinition item6 = new RowDefinition();
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
			ColumnDefinition item9 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.ColumnDefinitions.Add(item9);
			ColumnDefinition item10 = new ColumnDefinition();
			grid3.ColumnDefinitions.Add(item10);
			DataGrid dataGrid = new DataGrid();
			grid3.Children.Add(dataGrid);
			dataGrid.Name = "e_12";
			GamepadBinding gamepadBinding3 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.LeftStickButton)
			};
			gamepadBinding3.SetBinding(binding: new Binding("SwitchSortOffersCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			dataGrid.InputBindings.Add(gamepadBinding3);
			gamepadBinding3.Parent = dataGrid;
			GamepadBinding gamepadBinding4 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.LeftShoulderButton)
			};
			gamepadBinding4.SetBinding(binding: new Binding("PreviousInventoryCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			dataGrid.InputBindings.Add(gamepadBinding4);
			gamepadBinding4.Parent = dataGrid;
			GamepadBinding gamepadBinding5 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.RightShoulderButton)
			};
			gamepadBinding5.SetBinding(binding: new Binding("NextInventoryCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			dataGrid.InputBindings.Add(gamepadBinding5);
			gamepadBinding5.Parent = dataGrid;
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
					Name = "e_13",
					Margin = new Thickness(2f, 2f, 2f, 2f),
					Text = ""
				}
			};
			Func<UIElement, UIElement> createMethod = e_12_Col0_ct_dtMethod;
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
			textBlock.Name = "e_25";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Name");
			dataGridTemplateColumn2.Header = textBlock;
			Func<UIElement, UIElement> createMethod2 = e_12_Col1_ct_dtMethod;
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
			textBlock2.Name = "e_27";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Amount");
			dataGridTemplateColumn3.Header = textBlock2;
			Func<UIElement, UIElement> createMethod3 = e_12_Col2_ct_dtMethod;
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
			textBlock3.Name = "e_29";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_PricePerUnit");
			dataGridTemplateColumn4.Header = textBlock3;
			Func<UIElement, UIElement> createMethod4 = e_12_Col3_ct_dtMethod;
			dataGridTemplateColumn4.CellTemplate = new DataTemplate(createMethod4);
<<<<<<< HEAD
			dataGrid.Columns.Add(dataGridTemplateColumn4);
=======
			((Collection<DataGridColumn>)(object)dataGrid.Columns).Add((DataGridColumn)dataGridTemplateColumn4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumnSpan(dataGrid, 2);
			GamepadHelp.SetTargetName(dataGrid, "DataGridHelp");
			GamepadHelp.SetTabIndexRight(dataGrid, 4);
			GamepadHelp.SetTabIndexDown(dataGrid, 2);
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
			TextBlock textBlock4 = new TextBlock();
			grid3.Children.Add(textBlock4);
			textBlock4.Name = "e_37";
			textBlock4.Margin = new Thickness(0f, 5f, 0f, 5f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			Grid grid4 = new Grid();
			grid3.Children.Add(grid4);
			grid4.Name = "e_38";
			grid4.HorizontalAlignment = HorizontalAlignment.Stretch;
			ColumnDefinition item11 = new ColumnDefinition();
			grid4.ColumnDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition
			{
				Width = new GridLength(100f, GridUnitType.Pixel)
			};
			grid4.ColumnDefinitions.Add(item12);
			ColumnDefinition item13 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid4.ColumnDefinitions.Add(item13);
			ColumnDefinition item14 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item14);
			Grid.SetColumn(grid4, 1);
			Grid.SetRow(grid4, 1);
			Slider slider = new Slider();
			grid4.Children.Add(slider);
			slider.Name = "e_39";
			slider.Margin = new Thickness(5f, 5f, 5f, 5f);
			slider.VerticalAlignment = VerticalAlignment.Center;
			slider.TabIndex = 2;
			slider.Minimum = 0f;
			slider.IsSnapToTickEnabled = true;
			slider.TickFrequency = 1f;
			Grid.SetColumn(slider, 0);
			GamepadHelp.SetTargetName(slider, "AmountHelp");
			GamepadHelp.SetTabIndexUp(slider, 1);
			GamepadHelp.SetTabIndexDown(slider, 3);
			slider.SetBinding(binding: new Binding("AmountToBuyMaximum")
			{
				UseGeneratedBindings = true
			}, property: RangeBase.MaximumProperty);
			slider.SetBinding(binding: new Binding("AmountToBuy")
			{
				UseGeneratedBindings = true
			}, property: RangeBase.ValueProperty);
			TextBlock textBlock5 = new TextBlock();
			grid4.Children.Add(textBlock5);
			textBlock5.Name = "e_40";
			textBlock5.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("AmountToBuy")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid4.Children.Add(textBlock6);
			textBlock6.Name = "e_41";
			textBlock6.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 2);
			textBlock6.SetBinding(binding: new Binding("TotalPriceToBuy")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image = new Image();
			grid4.Children.Add(image);
			image.Name = "e_42";
			image.Width = 20f;
			image.Margin = new Thickness(4f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			Grid.SetColumn(image, 3);
			image.SetBinding(binding: new Binding("InventoryTargetViewModel.CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock7 = new TextBlock();
			grid3.Children.Add(textBlock7);
			textBlock7.Name = "e_43";
			textBlock7.Margin = new Thickness(0f, 5f, 0f, 5f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 0);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_CashBack");
			Grid grid5 = new Grid();
			grid3.Children.Add(grid5);
			grid5.Name = "e_44";
			Grid.SetColumn(grid5, 1);
			Grid.SetRow(grid5, 2);
			grid5.SetBinding(binding: new Binding("InventoryTargetViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid6 = new Grid();
			grid5.Children.Add(grid6);
			grid6.Name = "e_45";
			ColumnDefinition item15 = new ColumnDefinition();
			grid6.ColumnDefinitions.Add(item15);
			ColumnDefinition item16 = new ColumnDefinition
			{
				Width = new GridLength(100f, GridUnitType.Pixel)
			};
			grid6.ColumnDefinitions.Add(item16);
			ColumnDefinition item17 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid6.ColumnDefinitions.Add(item17);
			ColumnDefinition item18 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.ColumnDefinitions.Add(item18);
			Slider slider2 = new Slider();
			grid6.Children.Add(slider2);
			slider2.Name = "e_46";
			slider2.Margin = new Thickness(5f, 5f, 5f, 5f);
			slider2.VerticalAlignment = VerticalAlignment.Center;
			GamepadBinding gamepadBinding6 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding6.SetBinding(binding: new Binding("DepositCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			slider2.InputBindings.Add(gamepadBinding6);
			gamepadBinding6.Parent = slider2;
			GamepadBinding gamepadBinding7 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding7.SetBinding(binding: new Binding("WithdrawCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			slider2.InputBindings.Add(gamepadBinding7);
			gamepadBinding7.Parent = slider2;
			slider2.TabIndex = 3;
			slider2.Minimum = 0f;
			slider2.Maximum = 1000000f;
			slider2.IsSnapToTickEnabled = true;
			slider2.TickFrequency = 1f;
			Grid.SetColumn(slider2, 0);
			GamepadHelp.SetTargetName(slider2, "CashbackHelp");
			GamepadHelp.SetTabIndexUp(slider2, 2);
			slider2.SetBinding(binding: new Binding("BalanceChangeValue")
			{
				UseGeneratedBindings = true
			}, property: RangeBase.ValueProperty);
			TextBlock textBlock8 = new TextBlock();
			grid6.Children.Add(textBlock8);
			textBlock8.Name = "e_47";
			textBlock8.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock8.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 2);
			textBlock8.SetBinding(binding: new Binding("BalanceChangeValue")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			grid6.Children.Add(image2);
			image2.Name = "e_48";
			image2.Height = 20f;
			image2.Margin = new Thickness(4f, 2f, 2f, 2f);
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			Grid.SetColumn(image2, 3);
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid7 = new Grid();
			grid2.Children.Add(grid7);
			grid7.Name = "e_49";
			grid7.Margin = new Thickness(10f, 0f, 0f, 0f);
			Grid.SetColumn(grid7, 1);
			Grid.SetRow(grid7, 0);
			grid7.SetBinding(binding: new Binding("InventoryTargetViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid8 = new Grid();
			grid7.Children.Add(grid8);
			grid8.Name = "e_50";
			RowDefinition item19 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.RowDefinitions.Add(item19);
			RowDefinition item20 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.RowDefinitions.Add(item20);
			RowDefinition item21 = new RowDefinition();
			grid8.RowDefinitions.Add(item21);
			RowDefinition item22 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.RowDefinitions.Add(item22);
			RowDefinition item23 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.RowDefinitions.Add(item23);
			TextBlock textBlock9 = new TextBlock();
			grid8.Children.Add(textBlock9);
			textBlock9.Name = "e_51";
			textBlock9.Margin = new Thickness(0f, 0f, 0f, 5f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(textBlock9, 0);
			textBlock9.SetResourceReference(TextBlock.TextProperty, "StoreScreen_SelectInventory");
			ComboBox comboBox = new ComboBox();
			grid8.Children.Add(comboBox);
			comboBox.Name = "e_52";
			comboBox.TabIndex = 4;
			comboBox.MaxDropDownHeight = 300f;
			Grid.SetRow(comboBox, 1);
			GamepadHelp.SetTargetName(comboBox, "SelectHelpOffers");
			GamepadHelp.SetTabIndexLeft(comboBox, 1);
			GamepadHelp.SetTabIndexDown(comboBox, 5);
			comboBox.SetBinding(binding: new Binding("Inventories")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			comboBox.SetBinding(binding: new Binding("SelectedInventoryIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			ListBox listBox = new ListBox();
			grid8.Children.Add(listBox);
			listBox.Name = "e_53";
			listBox.Margin = new Thickness(0f, 4f, 0f, 5f);
			Style style = new Style(typeof(ListBox));
			Setter item24 = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item24);
			Setter item25 = new Setter(value: new ControlTemplate(createMethod: e_53_s_S_1_ctMethod, targetType: typeof(ListBox)), property: Control.TemplateProperty);
			style.Setters.Add(item25);
			Trigger trigger = new Trigger
			{
				Property = UIElement.IsFocusedProperty,
				Value = true
			};
			Setter item26 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item26);
			style.Triggers.Add(trigger);
			listBox.Style = style;
			listBox.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			listBox.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox.TabIndex = 5;
			Grid.SetRow(listBox, 2);
			GamepadHelp.SetTargetName(listBox, "SelectHelpOffers");
			GamepadHelp.SetTabIndexLeft(listBox, 1);
			GamepadHelp.SetTabIndexUp(listBox, 4);
			DragDrop.SetIsDropTarget(listBox, value: true);
			listBox.SetBinding(binding: new Binding("InventoryItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			InitializeElemente_53Resources(listBox);
			Grid grid9 = new Grid();
			grid8.Children.Add(grid9);
			grid9.Name = "e_58";
			grid9.Margin = new Thickness(0f, 0f, 0f, 10f);
			ColumnDefinition item27 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid9.ColumnDefinitions.Add(item27);
			ColumnDefinition item28 = new ColumnDefinition();
			grid9.ColumnDefinitions.Add(item28);
			Grid.SetRow(grid9, 3);
			TextBlock textBlock10 = new TextBlock();
			grid9.Children.Add(textBlock10);
			textBlock10.Name = "e_59";
			textBlock10.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ScreenTerminalInventory_Volume");
			StackPanel stackPanel = new StackPanel();
			grid9.Children.Add(stackPanel);
			stackPanel.Name = "e_60";
			stackPanel.Margin = new Thickness(5f, 5f, 0f, 5f);
			stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			TextBlock textBlock11 = new TextBlock();
			stackPanel.Children.Add(textBlock11);
			textBlock11.Name = "e_61";
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock11.SetBinding(binding: new Binding("CurrentInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock12 = new TextBlock();
			stackPanel.Children.Add(textBlock12);
			textBlock12.Name = "e_62";
			textBlock12.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			textBlock12.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock12.Text = "/";
			TextBlock textBlock13 = new TextBlock();
			stackPanel.Children.Add(textBlock13);
			textBlock13.Name = "e_63";
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			textBlock13.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock13.SetBinding(binding: new Binding("MaxInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid10 = new Grid();
			grid8.Children.Add(grid10);
			grid10.Name = "e_64";
			grid10.Margin = new Thickness(0f, 5f, 0f, 5f);
			RowDefinition item29 = new RowDefinition();
			grid10.RowDefinitions.Add(item29);
			RowDefinition item30 = new RowDefinition();
			grid10.RowDefinitions.Add(item30);
			RowDefinition item31 = new RowDefinition();
			grid10.RowDefinitions.Add(item31);
			ColumnDefinition item32 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid10.ColumnDefinitions.Add(item32);
			ColumnDefinition item33 = new ColumnDefinition();
			grid10.ColumnDefinitions.Add(item33);
			ColumnDefinition item34 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid10.ColumnDefinitions.Add(item34);
			Grid.SetRow(grid10, 4);
			TextBlock textBlock14 = new TextBlock();
			grid10.Children.Add(textBlock14);
			textBlock14.Name = "e_65";
			textBlock14.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock14.VerticalAlignment = VerticalAlignment.Center;
			textBlock14.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock14.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock15 = new TextBlock();
			grid10.Children.Add(textBlock15);
			textBlock15.Name = "e_66";
			textBlock15.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock15.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock15.VerticalAlignment = VerticalAlignment.Center;
			textBlock15.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock15, 1);
			textBlock15.SetBinding(binding: new Binding("LocalPlayerCurrency")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			grid10.Children.Add(image3);
			image3.Name = "e_67";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Border border = new Border();
			grid2.Children.Add(border);
			border.Name = "e_68";
			border.Height = 2f;
			border.Margin = new Thickness(0f, 10f, 0f, 0f);
			border.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border, 1);
			Grid.SetColumnSpan(border, 2);
			Grid grid11 = new Grid();
			grid2.Children.Add(grid11);
			grid11.Name = "DataGridHelp";
			grid11.Visibility = Visibility.Collapsed;
			grid11.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item35 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.ColumnDefinitions.Add(item35);
			ColumnDefinition item36 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.ColumnDefinitions.Add(item36);
			ColumnDefinition item37 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.ColumnDefinitions.Add(item37);
			ColumnDefinition item38 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.ColumnDefinitions.Add(item38);
			ColumnDefinition item39 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.ColumnDefinitions.Add(item39);
			ColumnDefinition item40 = new ColumnDefinition();
			grid11.ColumnDefinitions.Add(item40);
			Grid.SetRow(grid11, 2);
			TextBlock textBlock16 = new TextBlock();
			grid11.Children.Add(textBlock16);
			textBlock16.Name = "e_69";
			textBlock16.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock16.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock16.VerticalAlignment = VerticalAlignment.Center;
			textBlock16.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Buy");
			TextBlock textBlock17 = new TextBlock();
			grid11.Children.Add(textBlock17);
			textBlock17.Name = "e_70";
			textBlock17.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock17.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock17.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock17, 1);
			textBlock17.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Refresh");
			StackPanel stackPanel2 = new StackPanel();
			grid11.Children.Add(stackPanel2);
			stackPanel2.Name = "e_71";
			stackPanel2.Margin = new Thickness(10f, 0f, 5f, 0f);
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Center;
			stackPanel2.VerticalAlignment = VerticalAlignment.Center;
			stackPanel2.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel2, 2);
			TextBlock textBlock18 = new TextBlock();
			stackPanel2.Children.Add(textBlock18);
			textBlock18.Name = "e_72";
			textBlock18.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_SortBy");
			TextBlock textBlock19 = new TextBlock();
			stackPanel2.Children.Add(textBlock19);
			textBlock19.Name = "e_73";
			textBlock19.SetBinding(binding: new Binding("SortModeOffersText")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock20 = new TextBlock();
			grid11.Children.Add(textBlock20);
			textBlock20.Name = "e_74";
			textBlock20.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock20.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock20.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock20, 3);
			textBlock20.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_ChangeInventory");
			TextBlock textBlock21 = new TextBlock();
			grid11.Children.Add(textBlock21);
			textBlock21.Name = "e_75";
			textBlock21.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock21.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock21.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock21, 4);
			textBlock21.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid12 = new Grid();
			grid2.Children.Add(grid12);
			grid12.Name = "AmountHelp";
			grid12.Visibility = Visibility.Collapsed;
			grid12.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item41 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.ColumnDefinitions.Add(item41);
			ColumnDefinition item42 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.ColumnDefinitions.Add(item42);
			ColumnDefinition item43 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.ColumnDefinitions.Add(item43);
			ColumnDefinition item44 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.ColumnDefinitions.Add(item44);
			ColumnDefinition item45 = new ColumnDefinition();
			grid12.ColumnDefinitions.Add(item45);
			Grid.SetRow(grid12, 2);
			TextBlock textBlock22 = new TextBlock();
			grid12.Children.Add(textBlock22);
			textBlock22.Name = "e_76";
			textBlock22.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock22.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock22.VerticalAlignment = VerticalAlignment.Center;
			textBlock22.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Buy");
			TextBlock textBlock23 = new TextBlock();
			grid12.Children.Add(textBlock23);
			textBlock23.Name = "e_77";
			textBlock23.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock23.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock23.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock23, 1);
			textBlock23.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Refresh");
			TextBlock textBlock24 = new TextBlock();
			grid12.Children.Add(textBlock24);
			textBlock24.Name = "e_78";
			textBlock24.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock24.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock24.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock24, 2);
			textBlock24.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_ChangeAmount");
			TextBlock textBlock25 = new TextBlock();
			grid12.Children.Add(textBlock25);
			textBlock25.Name = "e_79";
			textBlock25.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock25.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock25.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock25, 3);
			textBlock25.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid13 = new Grid();
			grid2.Children.Add(grid13);
			grid13.Name = "CashbackHelp";
			grid13.Visibility = Visibility.Collapsed;
			grid13.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item46 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid13.ColumnDefinitions.Add(item46);
			ColumnDefinition item47 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid13.ColumnDefinitions.Add(item47);
			ColumnDefinition item48 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid13.ColumnDefinitions.Add(item48);
			ColumnDefinition item49 = new ColumnDefinition();
			grid13.ColumnDefinitions.Add(item49);
			Grid.SetColumn(grid13, 0);
			Grid.SetRow(grid13, 2);
			TextBlock textBlock26 = new TextBlock();
			grid13.Children.Add(textBlock26);
			textBlock26.Name = "e_80";
			textBlock26.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock26.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock26.VerticalAlignment = VerticalAlignment.Center;
			textBlock26.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Deposit");
			TextBlock textBlock27 = new TextBlock();
			grid13.Children.Add(textBlock27);
			textBlock27.Name = "e_81";
			textBlock27.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock27.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock27.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock27, 1);
			textBlock27.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Withdraw");
			TextBlock textBlock28 = new TextBlock();
			grid13.Children.Add(textBlock28);
			textBlock28.Name = "e_82";
			textBlock28.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock28.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock28.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock28, 2);
			textBlock28.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid14 = new Grid();
			grid2.Children.Add(grid14);
			grid14.Name = "SelectHelpOffers";
			grid14.Visibility = Visibility.Collapsed;
			grid14.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item50 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item50);
			ColumnDefinition item51 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item51);
			ColumnDefinition item52 = new ColumnDefinition();
			grid14.ColumnDefinitions.Add(item52);
			Grid.SetColumn(grid14, 0);
			Grid.SetRow(grid14, 2);
			TextBlock textBlock29 = new TextBlock();
			grid14.Children.Add(textBlock29);
			textBlock29.Name = "e_83";
			textBlock29.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock29.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock29.VerticalAlignment = VerticalAlignment.Center;
			textBlock29.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Select");
			TextBlock textBlock30 = new TextBlock();
			grid14.Children.Add(textBlock30);
			textBlock30.Name = "e_84";
			textBlock30.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock30.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock30.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock30, 1);
			textBlock30.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
<<<<<<< HEAD
			observableCollection.Add(tabItem);
=======
			((Collection<object>)(object)obj).Add((object)tabItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem2 = new TabItem
			{
				Name = "e_85",
				IsTabStop = false
			};
			tabItem2.SetBinding(binding: new Binding("IsSellTabVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreScreenSellHeader");
			Grid grid16 = (Grid)(tabItem2.Content = new Grid());
			grid16.Name = "e_86";
			RowDefinition item53 = new RowDefinition();
			grid16.RowDefinitions.Add(item53);
			RowDefinition item54 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid16.RowDefinitions.Add(item54);
			RowDefinition item55 = new RowDefinition
			{
				Height = new GridLength(65f, GridUnitType.Pixel)
			};
			grid16.RowDefinitions.Add(item55);
			ColumnDefinition item56 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid16.ColumnDefinitions.Add(item56);
			ColumnDefinition item57 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid16.ColumnDefinitions.Add(item57);
			Grid grid17 = new Grid();
			grid16.Children.Add(grid17);
			grid17.Name = "e_87";
			GamepadBinding gamepadBinding8 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding8.SetBinding(binding: new Binding("SellCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid17.InputBindings.Add(gamepadBinding8);
			gamepadBinding8.Parent = grid17;
			GamepadBinding gamepadBinding9 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding9.SetBinding(binding: new Binding("RefreshCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid17.InputBindings.Add(gamepadBinding9);
			gamepadBinding9.Parent = grid17;
			RowDefinition item58 = new RowDefinition();
			grid17.RowDefinitions.Add(item58);
			RowDefinition item59 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid17.RowDefinitions.Add(item59);
			ColumnDefinition item60 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid17.ColumnDefinitions.Add(item60);
			ColumnDefinition item61 = new ColumnDefinition();
			grid17.ColumnDefinitions.Add(item61);
			DataGrid dataGrid2 = new DataGrid();
			grid17.Children.Add(dataGrid2);
			dataGrid2.Name = "e_88";
			GamepadBinding gamepadBinding10 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.LeftStickButton)
			};
			gamepadBinding10.SetBinding(binding: new Binding("SwitchSortOrdersCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			dataGrid2.InputBindings.Add(gamepadBinding10);
			gamepadBinding10.Parent = dataGrid2;
			GamepadBinding gamepadBinding11 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.LeftShoulderButton)
			};
			gamepadBinding11.SetBinding(binding: new Binding("PreviousInventoryCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			dataGrid2.InputBindings.Add(gamepadBinding11);
			gamepadBinding11.Parent = dataGrid2;
			GamepadBinding gamepadBinding12 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.RightShoulderButton)
			};
			gamepadBinding12.SetBinding(binding: new Binding("NextInventoryCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			dataGrid2.InputBindings.Add(gamepadBinding12);
			gamepadBinding12.Parent = dataGrid2;
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
					Name = "e_89",
					Margin = new Thickness(2f, 2f, 2f, 2f),
					Text = ""
				}
			};
			Func<UIElement, UIElement> createMethod6 = e_88_Col0_ct_dtMethod;
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
			TextBlock textBlock31 = new TextBlock();
			textBlock31.Name = "e_91";
			textBlock31.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock31.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Name");
			dataGridTemplateColumn6.Header = textBlock31;
			Func<UIElement, UIElement> createMethod7 = e_88_Col1_ct_dtMethod;
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
			TextBlock textBlock32 = new TextBlock();
			textBlock32.Name = "e_93";
			textBlock32.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock32.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Amount");
			dataGridTemplateColumn7.Header = textBlock32;
			Func<UIElement, UIElement> createMethod8 = e_88_Col2_ct_dtMethod;
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
			TextBlock textBlock33 = new TextBlock();
			textBlock33.Name = "e_95";
			textBlock33.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock33.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_PricePerUnit");
			dataGridTemplateColumn8.Header = textBlock33;
			Func<UIElement, UIElement> createMethod9 = e_88_Col3_ct_dtMethod;
			dataGridTemplateColumn8.CellTemplate = new DataTemplate(createMethod9);
<<<<<<< HEAD
			dataGrid2.Columns.Add(dataGridTemplateColumn8);
=======
			((Collection<DataGridColumn>)(object)dataGrid2.Columns).Add((DataGridColumn)dataGridTemplateColumn8);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumnSpan(dataGrid2, 2);
			GamepadHelp.SetTargetName(dataGrid2, "DataGridSellHelp");
			GamepadHelp.SetTabIndexRight(dataGrid2, 13);
			GamepadHelp.SetTabIndexDown(dataGrid2, 11);
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
			TextBlock textBlock34 = new TextBlock();
			grid17.Children.Add(textBlock34);
			textBlock34.Name = "e_99";
			textBlock34.Margin = new Thickness(0f, 5f, 0f, 5f);
			textBlock34.VerticalAlignment = VerticalAlignment.Center;
			textBlock34.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock34, 0);
			Grid.SetRow(textBlock34, 1);
			textBlock34.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			Grid grid18 = new Grid();
			grid17.Children.Add(grid18);
			grid18.Name = "e_100";
			grid18.HorizontalAlignment = HorizontalAlignment.Stretch;
			ColumnDefinition item62 = new ColumnDefinition();
			grid18.ColumnDefinitions.Add(item62);
			ColumnDefinition item63 = new ColumnDefinition
			{
				Width = new GridLength(100f, GridUnitType.Pixel)
			};
			grid18.ColumnDefinitions.Add(item63);
			ColumnDefinition item64 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid18.ColumnDefinitions.Add(item64);
			ColumnDefinition item65 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid18.ColumnDefinitions.Add(item65);
			Grid.SetColumn(grid18, 1);
			Grid.SetRow(grid18, 1);
			Slider slider3 = new Slider();
			grid18.Children.Add(slider3);
			slider3.Name = "e_101";
			slider3.Margin = new Thickness(5f, 5f, 5f, 5f);
			slider3.VerticalAlignment = VerticalAlignment.Center;
			slider3.TabIndex = 11;
			slider3.Minimum = 0f;
			slider3.Maximum = 99999f;
			slider3.IsSnapToTickEnabled = true;
			slider3.TickFrequency = 1f;
			Grid.SetColumn(slider3, 0);
			GamepadHelp.SetTargetName(slider3, "AmountSellHelp");
			GamepadHelp.SetTabIndexUp(slider3, 10);
			slider3.SetBinding(binding: new Binding("AmountToSell")
			{
				UseGeneratedBindings = true
			}, property: RangeBase.ValueProperty);
			TextBlock textBlock35 = new TextBlock();
			grid18.Children.Add(textBlock35);
			textBlock35.Name = "e_102";
			textBlock35.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock35.VerticalAlignment = VerticalAlignment.Center;
			textBlock35.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock35, 1);
			textBlock35.SetBinding(binding: new Binding("AmountToSell")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock36 = new TextBlock();
			grid18.Children.Add(textBlock36);
			textBlock36.Name = "e_103";
			textBlock36.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock36.VerticalAlignment = VerticalAlignment.Center;
			textBlock36.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock36, 2);
			textBlock36.SetBinding(binding: new Binding("TotalPriceToSell")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			grid18.Children.Add(image4);
			image4.Name = "e_104";
			image4.Width = 20f;
			image4.Margin = new Thickness(4f, 2f, 2f, 2f);
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Stretch = Stretch.Uniform;
			Grid.SetColumn(image4, 3);
			image4.SetBinding(binding: new Binding("InventoryTargetViewModel.CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid19 = new Grid();
			grid16.Children.Add(grid19);
			grid19.Name = "e_105";
			grid19.Margin = new Thickness(10f, 0f, 0f, 0f);
			Grid.SetColumn(grid19, 1);
			Grid.SetRow(grid19, 0);
			grid19.SetBinding(binding: new Binding("InventoryTargetViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid20 = new Grid();
			grid19.Children.Add(grid20);
			grid20.Name = "e_106";
			RowDefinition item66 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.RowDefinitions.Add(item66);
			RowDefinition item67 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.RowDefinitions.Add(item67);
			RowDefinition item68 = new RowDefinition();
			grid20.RowDefinitions.Add(item68);
			RowDefinition item69 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.RowDefinitions.Add(item69);
			RowDefinition item70 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.RowDefinitions.Add(item70);
			TextBlock textBlock37 = new TextBlock();
			grid20.Children.Add(textBlock37);
			textBlock37.Name = "e_107";
			textBlock37.Margin = new Thickness(0f, 0f, 0f, 5f);
			textBlock37.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(textBlock37, 0);
			textBlock37.SetResourceReference(TextBlock.TextProperty, "StoreScreen_SelectInventory");
			ComboBox comboBox2 = new ComboBox();
			grid20.Children.Add(comboBox2);
			comboBox2.Name = "e_108";
			comboBox2.TabIndex = 13;
			comboBox2.MaxDropDownHeight = 300f;
			Grid.SetRow(comboBox2, 1);
			GamepadHelp.SetTargetName(comboBox2, "SelectHelpOrders");
			GamepadHelp.SetTabIndexLeft(comboBox2, 10);
			GamepadHelp.SetTabIndexDown(comboBox2, 14);
			comboBox2.SetBinding(binding: new Binding("Inventories")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			comboBox2.SetBinding(binding: new Binding("SelectedInventoryIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			ListBox listBox2 = new ListBox();
			grid20.Children.Add(listBox2);
			listBox2.Name = "e_109";
			listBox2.Margin = new Thickness(0f, 4f, 0f, 0f);
			Style style2 = new Style(typeof(ListBox));
			Setter item71 = new Setter(UIElement.MinHeightProperty, 80f);
			style2.Setters.Add(item71);
			Setter item72 = new Setter(value: new ControlTemplate(createMethod: e_109_s_S_1_ctMethod, targetType: typeof(ListBox)), property: Control.TemplateProperty);
			style2.Setters.Add(item72);
			Trigger trigger2 = new Trigger
			{
				Property = UIElement.IsFocusedProperty,
				Value = true
			};
			Setter item73 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger2.Setters.Add(item73);
			style2.Triggers.Add(trigger2);
			listBox2.Style = style2;
			listBox2.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			listBox2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox2.TabIndex = 14;
			Grid.SetRow(listBox2, 2);
			GamepadHelp.SetTargetName(listBox2, "SelectHelpOrders");
			GamepadHelp.SetTabIndexLeft(listBox2, 10);
			GamepadHelp.SetTabIndexUp(listBox2, 13);
			listBox2.SetBinding(binding: new Binding("InventoryItems")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			InitializeElemente_109Resources(listBox2);
			Grid grid21 = new Grid();
			grid20.Children.Add(grid21);
			grid21.Name = "e_114";
			grid21.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item74 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid21.ColumnDefinitions.Add(item74);
			ColumnDefinition item75 = new ColumnDefinition();
			grid21.ColumnDefinitions.Add(item75);
			Grid.SetRow(grid21, 3);
			TextBlock textBlock38 = new TextBlock();
			grid21.Children.Add(textBlock38);
			textBlock38.Name = "e_115";
			textBlock38.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock38.VerticalAlignment = VerticalAlignment.Center;
			textBlock38.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock38.SetResourceReference(TextBlock.TextProperty, "ScreenTerminalInventory_Volume");
			StackPanel stackPanel3 = new StackPanel();
			grid21.Children.Add(stackPanel3);
			stackPanel3.Name = "e_116";
			stackPanel3.Margin = new Thickness(5f, 5f, 0f, 5f);
			stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel3.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel3, 1);
			TextBlock textBlock39 = new TextBlock();
			stackPanel3.Children.Add(textBlock39);
			textBlock39.Name = "e_117";
			textBlock39.VerticalAlignment = VerticalAlignment.Center;
			textBlock39.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock39.SetBinding(binding: new Binding("CurrentInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock40 = new TextBlock();
			stackPanel3.Children.Add(textBlock40);
			textBlock40.Name = "e_118";
			textBlock40.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock40.VerticalAlignment = VerticalAlignment.Center;
			textBlock40.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock40.Text = "/";
			TextBlock textBlock41 = new TextBlock();
			stackPanel3.Children.Add(textBlock41);
			textBlock41.Name = "e_119";
			textBlock41.VerticalAlignment = VerticalAlignment.Center;
			textBlock41.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock41.SetBinding(binding: new Binding("MaxInventoryVolume")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid22 = new Grid();
			grid20.Children.Add(grid22);
			grid22.Name = "e_120";
			grid22.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item76 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid22.ColumnDefinitions.Add(item76);
			ColumnDefinition item77 = new ColumnDefinition();
			grid22.ColumnDefinitions.Add(item77);
			ColumnDefinition item78 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid22.ColumnDefinitions.Add(item78);
			Grid.SetRow(grid22, 4);
			TextBlock textBlock42 = new TextBlock();
			grid22.Children.Add(textBlock42);
			textBlock42.Name = "e_121";
			textBlock42.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock42.VerticalAlignment = VerticalAlignment.Center;
			textBlock42.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock42.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock43 = new TextBlock();
			grid22.Children.Add(textBlock43);
			textBlock43.Name = "e_122";
			textBlock43.Margin = new Thickness(5f, 5f, 0f, 5f);
			textBlock43.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock43.VerticalAlignment = VerticalAlignment.Center;
			textBlock43.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock43, 1);
			textBlock43.SetBinding(binding: new Binding("LocalPlayerCurrency")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image5 = new Image();
			grid22.Children.Add(image5);
			image5.Name = "e_123";
			image5.Height = 20f;
			image5.Margin = new Thickness(4f, 2f, 2f, 2f);
			image5.VerticalAlignment = VerticalAlignment.Center;
			image5.Stretch = Stretch.Uniform;
			Grid.SetColumn(image5, 2);
			image5.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Border border2 = new Border();
			grid16.Children.Add(border2);
			border2.Name = "e_124";
			border2.Height = 2f;
			border2.Margin = new Thickness(0f, 10f, 0f, 10f);
			border2.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border2, 1);
			Grid.SetColumnSpan(border2, 2);
			Grid grid23 = new Grid();
			grid16.Children.Add(grid23);
			grid23.Name = "DataGridSellHelp";
			grid23.Visibility = Visibility.Collapsed;
			grid23.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item79 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid23.ColumnDefinitions.Add(item79);
			ColumnDefinition item80 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid23.ColumnDefinitions.Add(item80);
			ColumnDefinition item81 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid23.ColumnDefinitions.Add(item81);
			ColumnDefinition item82 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid23.ColumnDefinitions.Add(item82);
			ColumnDefinition item83 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid23.ColumnDefinitions.Add(item83);
			ColumnDefinition item84 = new ColumnDefinition();
			grid23.ColumnDefinitions.Add(item84);
			Grid.SetRow(grid23, 2);
			TextBlock textBlock44 = new TextBlock();
			grid23.Children.Add(textBlock44);
			textBlock44.Name = "e_125";
			textBlock44.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock44.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock44.VerticalAlignment = VerticalAlignment.Center;
			textBlock44.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Sell");
			TextBlock textBlock45 = new TextBlock();
			grid23.Children.Add(textBlock45);
			textBlock45.Name = "e_126";
			textBlock45.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock45.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock45.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock45, 1);
			textBlock45.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Refresh");
			StackPanel stackPanel4 = new StackPanel();
			grid23.Children.Add(stackPanel4);
			stackPanel4.Name = "e_127";
			stackPanel4.Margin = new Thickness(10f, 0f, 5f, 0f);
			stackPanel4.HorizontalAlignment = HorizontalAlignment.Center;
			stackPanel4.VerticalAlignment = VerticalAlignment.Center;
			stackPanel4.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel4, 2);
			TextBlock textBlock46 = new TextBlock();
			stackPanel4.Children.Add(textBlock46);
			textBlock46.Name = "e_128";
			textBlock46.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_SortBy");
			TextBlock textBlock47 = new TextBlock();
			stackPanel4.Children.Add(textBlock47);
			textBlock47.Name = "e_129";
			textBlock47.SetBinding(binding: new Binding("SortModeOrdersText")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock48 = new TextBlock();
			grid23.Children.Add(textBlock48);
			textBlock48.Name = "e_130";
			textBlock48.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock48.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock48.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock48, 3);
			textBlock48.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_ChangeInventory");
			TextBlock textBlock49 = new TextBlock();
			grid23.Children.Add(textBlock49);
			textBlock49.Name = "e_131";
			textBlock49.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock49.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock49.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock49, 4);
			textBlock49.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid24 = new Grid();
			grid16.Children.Add(grid24);
			grid24.Name = "AmountSellHelp";
			grid24.Visibility = Visibility.Collapsed;
			grid24.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item85 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid24.ColumnDefinitions.Add(item85);
			ColumnDefinition item86 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid24.ColumnDefinitions.Add(item86);
			ColumnDefinition item87 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid24.ColumnDefinitions.Add(item87);
			ColumnDefinition item88 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid24.ColumnDefinitions.Add(item88);
			ColumnDefinition item89 = new ColumnDefinition();
			grid24.ColumnDefinitions.Add(item89);
			Grid.SetRow(grid24, 2);
			TextBlock textBlock50 = new TextBlock();
			grid24.Children.Add(textBlock50);
			textBlock50.Name = "e_132";
			textBlock50.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock50.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock50.VerticalAlignment = VerticalAlignment.Center;
			textBlock50.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Sell");
			TextBlock textBlock51 = new TextBlock();
			grid24.Children.Add(textBlock51);
			textBlock51.Name = "e_133";
			textBlock51.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock51.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock51.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock51, 1);
			textBlock51.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Refresh");
			TextBlock textBlock52 = new TextBlock();
			grid24.Children.Add(textBlock52);
			textBlock52.Name = "e_134";
			textBlock52.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock52.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock52.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock52, 2);
			textBlock52.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_ChangeAmount");
			TextBlock textBlock53 = new TextBlock();
			grid24.Children.Add(textBlock53);
			textBlock53.Name = "e_135";
			textBlock53.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock53.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock53.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock53, 3);
			textBlock53.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid25 = new Grid();
			grid16.Children.Add(grid25);
			grid25.Name = "SelectHelpOrders";
			grid25.Visibility = Visibility.Collapsed;
			grid25.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item90 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid25.ColumnDefinitions.Add(item90);
			ColumnDefinition item91 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid25.ColumnDefinitions.Add(item91);
			ColumnDefinition item92 = new ColumnDefinition();
			grid25.ColumnDefinitions.Add(item92);
			Grid.SetColumn(grid25, 0);
			Grid.SetRow(grid25, 2);
			TextBlock textBlock54 = new TextBlock();
			grid25.Children.Add(textBlock54);
			textBlock54.Name = "e_136";
			textBlock54.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock54.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock54.VerticalAlignment = VerticalAlignment.Center;
			textBlock54.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Select");
			TextBlock textBlock55 = new TextBlock();
			grid25.Children.Add(textBlock55);
			textBlock55.Name = "e_137";
			textBlock55.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock55.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock55.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock55, 1);
			textBlock55.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
<<<<<<< HEAD
			observableCollection.Add(tabItem2);
=======
			((Collection<object>)(object)obj).Add((object)tabItem2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem3 = new TabItem
			{
				Name = "e_138",
				IsTabStop = false
			};
			tabItem3.SetBinding(binding: new Binding("IsAdministrationVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreAdministration");
			Grid grid27 = (Grid)(tabItem3.Content = new Grid());
			grid27.Name = "e_139";
			grid27.SetBinding(binding: new Binding("AdministrationViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid28 = new Grid();
			grid27.Children.Add(grid28);
			grid28.Name = "e_140";
			grid28.Margin = new Thickness(0f, 0f, 0f, 30f);
			RowDefinition item93 = new RowDefinition();
			grid28.RowDefinitions.Add(item93);
			RowDefinition item94 = new RowDefinition
			{
				Height = new GridLength(65f, GridUnitType.Pixel)
			};
			grid28.RowDefinitions.Add(item94);
			ColumnDefinition item95 = new ColumnDefinition();
			grid28.ColumnDefinitions.Add(item95);
			ColumnDefinition item96 = new ColumnDefinition();
			grid28.ColumnDefinitions.Add(item96);
			ListBox listBox3 = new ListBox();
			grid28.Children.Add(listBox3);
			listBox3.Name = "e_141";
			listBox3.Margin = new Thickness(0f, 10f, 10f, 0f);
			GamepadBinding gamepadBinding13 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding13.SetBinding(binding: new Binding("RemoveStoreItemCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			listBox3.InputBindings.Add(gamepadBinding13);
			gamepadBinding13.Parent = listBox3;
			listBox3.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			listBox3.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox3.TabIndex = 30;
			listBox3.SelectionMode = SelectionMode.Single;
			Grid.SetColumn(listBox3, 0);
			GamepadHelp.SetTargetName(listBox3, "AdminListHelp");
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
			Grid grid29 = new Grid();
			grid28.Children.Add(grid29);
			grid29.Name = "AdminListHelp";
			grid29.Visibility = Visibility.Collapsed;
			grid29.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item97 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid29.ColumnDefinitions.Add(item97);
			ColumnDefinition item98 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid29.ColumnDefinitions.Add(item98);
			ColumnDefinition item99 = new ColumnDefinition();
			grid29.ColumnDefinitions.Add(item99);
			Grid.SetRow(grid29, 1);
			TextBlock textBlock56 = new TextBlock();
			grid29.Children.Add(textBlock56);
			textBlock56.Name = "e_142";
			textBlock56.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock56.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock56.VerticalAlignment = VerticalAlignment.Center;
			textBlock56.SetResourceReference(TextBlock.TextProperty, "StoreScreenAdmin_Help_Delete");
			TextBlock textBlock57 = new TextBlock();
			grid29.Children.Add(textBlock57);
			textBlock57.Name = "e_143";
			textBlock57.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock57.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock57.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock57, 1);
			textBlock57.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid30 = new Grid();
			grid28.Children.Add(grid30);
			grid30.Name = "OrderListHelp";
			grid30.Visibility = Visibility.Collapsed;
			grid30.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item100 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid30.ColumnDefinitions.Add(item100);
			ColumnDefinition item101 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid30.ColumnDefinitions.Add(item101);
			ColumnDefinition item102 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid30.ColumnDefinitions.Add(item102);
			ColumnDefinition item103 = new ColumnDefinition();
			grid30.ColumnDefinitions.Add(item103);
			Grid.SetRow(grid30, 1);
			TextBlock textBlock58 = new TextBlock();
			grid30.Children.Add(textBlock58);
			textBlock58.Name = "e_144";
			textBlock58.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock58.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock58.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock58, 1);
			textBlock58.SetResourceReference(TextBlock.TextProperty, "StoreScreenAdmin_Help_CreateOrder");
			TextBlock textBlock59 = new TextBlock();
			grid30.Children.Add(textBlock59);
			textBlock59.Name = "e_145";
			textBlock59.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock59.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock59.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock59, 2);
			textBlock59.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid31 = new Grid();
			grid28.Children.Add(grid31);
			grid31.Name = "OrderNumericHelp";
			grid31.Visibility = Visibility.Collapsed;
			grid31.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item104 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid31.ColumnDefinitions.Add(item104);
			ColumnDefinition item105 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid31.ColumnDefinitions.Add(item105);
			ColumnDefinition item106 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid31.ColumnDefinitions.Add(item106);
			ColumnDefinition item107 = new ColumnDefinition();
			grid31.ColumnDefinitions.Add(item107);
			Grid.SetRow(grid31, 1);
			TextBlock textBlock60 = new TextBlock();
			grid31.Children.Add(textBlock60);
			textBlock60.Name = "e_146";
			textBlock60.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock60.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock60.VerticalAlignment = VerticalAlignment.Center;
			textBlock60.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_ChangeValue");
			TextBlock textBlock61 = new TextBlock();
			grid31.Children.Add(textBlock61);
			textBlock61.Name = "e_147";
			textBlock61.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock61.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock61.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock61, 1);
			textBlock61.SetResourceReference(TextBlock.TextProperty, "StoreScreenAdmin_Help_CreateOrder");
			TextBlock textBlock62 = new TextBlock();
			grid31.Children.Add(textBlock62);
			textBlock62.Name = "e_148";
			textBlock62.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock62.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock62.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock62, 2);
			textBlock62.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid32 = new Grid();
			grid28.Children.Add(grid32);
			grid32.Name = "OfferListHelp";
			grid32.Visibility = Visibility.Collapsed;
			grid32.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item108 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid32.ColumnDefinitions.Add(item108);
			ColumnDefinition item109 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid32.ColumnDefinitions.Add(item109);
			ColumnDefinition item110 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid32.ColumnDefinitions.Add(item110);
			ColumnDefinition item111 = new ColumnDefinition();
			grid32.ColumnDefinitions.Add(item111);
			Grid.SetRow(grid32, 1);
			TextBlock textBlock63 = new TextBlock();
			grid32.Children.Add(textBlock63);
			textBlock63.Name = "e_149";
			textBlock63.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock63.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock63.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock63, 1);
			textBlock63.SetResourceReference(TextBlock.TextProperty, "StoreScreenAdmin_Help_CreateOffer");
			TextBlock textBlock64 = new TextBlock();
			grid32.Children.Add(textBlock64);
			textBlock64.Name = "e_150";
			textBlock64.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock64.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock64.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock64, 2);
			textBlock64.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid33 = new Grid();
			grid28.Children.Add(grid33);
			grid33.Name = "OfferNumericHelp";
			grid33.Visibility = Visibility.Collapsed;
			grid33.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item112 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid33.ColumnDefinitions.Add(item112);
			ColumnDefinition item113 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid33.ColumnDefinitions.Add(item113);
			ColumnDefinition item114 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid33.ColumnDefinitions.Add(item114);
			ColumnDefinition item115 = new ColumnDefinition();
			grid33.ColumnDefinitions.Add(item115);
			Grid.SetRow(grid33, 1);
			TextBlock textBlock65 = new TextBlock();
			grid33.Children.Add(textBlock65);
			textBlock65.Name = "e_151";
			textBlock65.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock65.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock65.VerticalAlignment = VerticalAlignment.Center;
			textBlock65.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_ChangeValue");
			TextBlock textBlock66 = new TextBlock();
			grid33.Children.Add(textBlock66);
			textBlock66.Name = "e_152";
			textBlock66.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock66.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock66.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock66, 1);
			textBlock66.SetResourceReference(TextBlock.TextProperty, "StoreScreenAdmin_Help_CreateOffer");
			TextBlock textBlock67 = new TextBlock();
			grid33.Children.Add(textBlock67);
			textBlock67.Name = "e_153";
			textBlock67.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock67.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock67.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock67, 2);
			textBlock67.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			TabControl tabControl = new TabControl();
			grid28.Children.Add(tabControl);
			tabControl.Name = "e_154";
			tabControl.Margin = new Thickness(2f, 2f, 2f, 2f);
			tabControl.TabIndex = 32;
<<<<<<< HEAD
			tabControl.ItemsSource = Get_e_154_Items();
=======
			tabControl.ItemsSource = (IEnumerable)Get_e_154_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumn(tabControl, 1);
			Grid.SetRowSpan(tabControl, 2);
			GamepadHelp.SetTabIndexLeft(tabControl, 30);
			InitializeElemente_154Resources(tabControl);
<<<<<<< HEAD
			observableCollection.Add(tabItem3);
			return observableCollection;
=======
			((Collection<object>)(object)obj).Add((object)tabItem3);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static UIElement tt_e_16_Method()
		{
			Grid obj = new Grid
			{
				Name = "e_17",
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
			border.Name = "e_18";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_19";
			textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock.SetBinding(binding: new Binding("Name"), property: TextBlock.TextProperty);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_20";
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRow(image, 1);
			image.SetBinding(binding: new Binding("TooltipImage"), property: Image.SourceProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_21";
			stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetRow(stackPanel, 2);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_22";
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreScreen_GridTooltip_Pcu");
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_23";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock3.SetBinding(binding: new Binding("PrefabTotalPcu"), property: TextBlock.TextProperty);
			TextBlock textBlock4 = new TextBlock();
			obj.Children.Add(textBlock4);
			textBlock4.Name = "e_24";
			textBlock4.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock4.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock4, 3);
			textBlock4.SetBinding(binding: new Binding("Description"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_12_Col0_ct_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_14",
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_15";
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon"), property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_16";
			grid.HorizontalAlignment = HorizontalAlignment.Stretch;
			grid.VerticalAlignment = VerticalAlignment.Stretch;
			ToolTip toolTip2 = (ToolTip)(grid.ToolTip = new ToolTip());
			toolTip2.Content = tt_e_16_Method();
			grid.SetBinding(binding: new Binding("HasTooltip"), property: UIElement.VisibilityProperty);
			return obj;
		}

		private static UIElement e_12_Col1_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_26",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("Name"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_12_Col2_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_28",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("AmountFormatted"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement tt_e_32_Method()
		{
			StackPanel obj = new StackPanel
			{
				Name = "e_33",
				Orientation = Orientation.Horizontal
			};
			obj.SetBinding(binding: new Binding("HasPricePerUnitDiscount"), property: UIElement.VisibilityProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_34";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetResourceReference(TextBlock.TextProperty, "StoreBlock_OfferDiscount");
			TextBlock textBlock2 = new TextBlock();
			obj.Children.Add(textBlock2);
			textBlock2.Name = "e_35";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.SetBinding(binding: new Binding("PricePerUnitDiscount")
			{
				StringFormat = "{0}%"
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_12_Col3_ct_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_30",
				Margin = new Thickness(0f, 0f, 4f, 0f),
				HorizontalAlignment = HorizontalAlignment.Right,
				Orientation = Orientation.Horizontal
			};
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_31";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("HasNormalPrice"), property: UIElement.VisibilityProperty);
			textBlock.SetBinding(binding: new Binding("PricePerUnitFormatted"), property: TextBlock.TextProperty);
			TextBlock textBlock2 = new TextBlock();
			obj.Children.Add(textBlock2);
			textBlock2.Name = "e_32";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			ToolTip toolTip2 = (ToolTip)(textBlock2.ToolTip = new ToolTip());
			toolTip2.Content = tt_e_32_Method();
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 44, 20, 255));
			textBlock2.SetBinding(binding: new Binding("HasPricePerUnitDiscount"), property: UIElement.VisibilityProperty);
			textBlock2.SetBinding(binding: new Binding("PricePerUnitFormatted"), property: TextBlock.TextProperty);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_36";
			image.Width = 20f;
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("CurrencyIcon"), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_53_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_54",
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
			UniformGrid uniformGrid2 = (UniformGrid)(scrollViewer.Content = new UniformGrid());
			uniformGrid2.Name = "e_55";
			uniformGrid2.Margin = new Thickness(4f, 4f, 4f, 4f);
			uniformGrid2.VerticalAlignment = VerticalAlignment.Top;
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 5;
			return obj;
		}

		private static void InitializeElemente_53Resources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Setter item2 = new Setter(Control.IsTabStopProperty, false);
			style.Setters.Add(item2);
			Func<UIElement, UIElement> createMethod = e_53r_0_s_S_2_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ListBoxItem), createMethod);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsMouseOverProperty;
			trigger.Value = true;
			Setter item3 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			trigger.Setters.Add(item3);
			controlTemplate.Triggers.Add(trigger);
			Trigger trigger2 = new Trigger();
			trigger2.Property = ListBoxItem.IsSelectedProperty;
			trigger2.Value = true;
			Setter item4 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItemHover"));
			trigger2.Setters.Add(item4);
			controlTemplate.Triggers.Add(trigger2);
			Setter item5 = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item5);
			elem.Resources.Add(typeof(ListBoxItem), style);
		}

		private static UIElement e_53r_0_s_S_2_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_56"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_57";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
			contentPresenter.SetBinding(binding: new Binding(), property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_88_Col0_ct_dtMethod(UIElement parent)
		{
			Image obj = new Image
			{
				Parent = parent,
				Name = "e_90",
				Stretch = Stretch.Fill
			};
			obj.SetBinding(binding: new Binding("Icon"), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_88_Col1_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_92",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("Name"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_88_Col2_ct_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_94",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("AmountFormatted"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_88_Col3_ct_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_96",
				Margin = new Thickness(0f, 0f, 4f, 0f),
				HorizontalAlignment = HorizontalAlignment.Right,
				Orientation = Orientation.Horizontal
			};
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_97";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("PricePerUnitFormatted"), property: TextBlock.TextProperty);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_98";
			image.Width = 20f;
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("CurrencyIcon"), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_109_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_110",
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
			UniformGrid uniformGrid2 = (UniformGrid)(scrollViewer.Content = new UniformGrid());
			uniformGrid2.Name = "e_111";
			uniformGrid2.Margin = new Thickness(4f, 4f, 4f, 4f);
			uniformGrid2.VerticalAlignment = VerticalAlignment.Top;
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 5;
			return obj;
		}

		private static void InitializeElemente_109Resources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Setter item2 = new Setter(Control.IsTabStopProperty, false);
			style.Setters.Add(item2);
			Func<UIElement, UIElement> createMethod = e_109r_0_s_S_2_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ListBoxItem), createMethod);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsMouseOverProperty;
			trigger.Value = true;
			Setter item3 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			trigger.Setters.Add(item3);
			controlTemplate.Triggers.Add(trigger);
			Trigger trigger2 = new Trigger();
			trigger2.Property = ListBoxItem.IsSelectedProperty;
			trigger2.Value = true;
			Setter item4 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItemHover"));
			trigger2.Setters.Add(item4);
			controlTemplate.Triggers.Add(trigger2);
			Setter item5 = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item5);
			elem.Resources.Add(typeof(ListBoxItem), style);
		}

		private static UIElement e_109r_0_s_S_2_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_112"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_113";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
			contentPresenter.SetBinding(binding: new Binding(), property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static ObservableCollection<object> Get_e_154_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem = new TabItem();
			tabItem.Name = "e_155";
			tabItem.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreAdministration_NewOffer");
			Grid grid2 = (Grid)(tabItem.Content = new Grid());
			grid2.Name = "e_156";
			GamepadBinding gamepadBinding = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding.SetBinding(binding: new Binding("CreateOfferCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid2.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = grid2;
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
			ListBox listBox = new ListBox();
			grid2.Children.Add(listBox);
			listBox.Name = "e_157";
			listBox.Margin = new Thickness(0f, 10f, 10f, 10f);
			listBox.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			listBox.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox.TabIndex = 130;
			Grid.SetColumn(listBox, 0);
			GamepadHelp.SetTargetName(listBox, "OfferListHelp");
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
			grid3.Name = "e_158";
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item5);
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
			RowDefinition item10 = new RowDefinition();
			grid3.RowDefinitions.Add(item10);
			ColumnDefinition item11 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.ColumnDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition();
			grid3.ColumnDefinitions.Add(item12);
			Grid.SetColumn(grid3, 0);
			Grid.SetRow(grid3, 1);
			TextBlock textBlock = new TextBlock();
			grid3.Children.Add(textBlock);
			textBlock.Name = "e_159";
			textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			NumericTextBox numericTextBox = new NumericTextBox();
			grid3.Children.Add(numericTextBox);
			numericTextBox.Name = "e_160";
			numericTextBox.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox.TabIndex = 131;
			numericTextBox.MaxLength = 5;
			numericTextBox.Minimum = 0f;
			numericTextBox.Maximum = 99999f;
			Grid.SetColumn(numericTextBox, 1);
			GamepadHelp.SetTargetName(numericTextBox, "OfferNumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox, 30);
			GamepadHelp.SetTabIndexUp(numericTextBox, 130);
			GamepadHelp.SetTabIndexDown(numericTextBox, 132);
			numericTextBox.SetBinding(binding: new Binding("OfferAmount")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			TextBlock textBlock2 = new TextBlock();
			grid3.Children.Add(textBlock2);
			textBlock2.Name = "e_161";
			textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock2, 1);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_PricePerUnitLabel");
			NumericTextBox numericTextBox2 = new NumericTextBox();
			grid3.Children.Add(numericTextBox2);
			numericTextBox2.Name = "e_162";
			numericTextBox2.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox2.TabIndex = 132;
			numericTextBox2.MaxLength = 9;
			numericTextBox2.Maximum = 1E+09f;
			Grid.SetColumn(numericTextBox2, 1);
			Grid.SetRow(numericTextBox2, 1);
			GamepadHelp.SetTargetName(numericTextBox2, "OfferNumericHelp");
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
			textBlock3.Name = "e_163";
			textBlock3.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 0);
			Grid.SetRow(textBlock3, 2);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_TotalPriceLabel");
			StackPanel stackPanel = new StackPanel();
			grid3.Children.Add(stackPanel);
			stackPanel.Name = "e_164";
			stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 2);
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_165";
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
			image.Name = "e_166";
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
			textBlock5.Name = "e_167";
			textBlock5.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 0);
			Grid.SetRow(textBlock5, 3);
			textBlock5.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_ListingFee");
			StackPanel stackPanel2 = new StackPanel();
			grid3.Children.Add(stackPanel2);
			stackPanel2.Name = "e_168";
			stackPanel2.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel2.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel2, 1);
			Grid.SetRow(stackPanel2, 3);
			TextBlock textBlock6 = new TextBlock();
			stackPanel2.Children.Add(textBlock6);
			textBlock6.Name = "e_169";
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
			image2.Name = "e_170";
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
			textBlock7.Name = "e_171";
			textBlock7.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 0);
			Grid.SetRow(textBlock7, 4);
			textBlock7.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_TransactionFee");
			StackPanel stackPanel3 = new StackPanel();
			grid3.Children.Add(stackPanel3);
			stackPanel3.Name = "e_172";
			stackPanel3.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel3.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel3.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel3, 1);
			Grid.SetRow(stackPanel3, 4);
			TextBlock textBlock8 = new TextBlock();
			stackPanel3.Children.Add(textBlock8);
			textBlock8.Name = "e_173";
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
			image3.Name = "e_174";
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
			grid4.Name = "e_175";
			grid4.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item13 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item13);
			ColumnDefinition item14 = new ColumnDefinition();
			grid4.ColumnDefinitions.Add(item14);
			ColumnDefinition item15 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item15);
			Grid.SetRow(grid4, 5);
			Grid.SetColumnSpan(grid4, 2);
			TextBlock textBlock9 = new TextBlock();
			grid4.Children.Add(textBlock9);
			textBlock9.Name = "e_176";
			textBlock9.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock9.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock9.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock10 = new TextBlock();
			grid4.Children.Add(textBlock10);
			textBlock10.Name = "e_177";
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
			image4.Name = "e_178";
			image4.Height = 20f;
			image4.Margin = new Thickness(4f, 2f, 2f, 2f);
			image4.VerticalAlignment = VerticalAlignment.Bottom;
			image4.Stretch = Stretch.Uniform;
			Grid.SetColumn(image4, 2);
			image4.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
<<<<<<< HEAD
			observableCollection.Add(tabItem);
=======
			((Collection<object>)(object)obj).Add((object)tabItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem2 = new TabItem
			{
				Name = "e_179"
			};
			tabItem2.SetBinding(binding: new Binding("IsTabNewOrderVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, "StoreAdministration_NewOrder");
			Grid grid6 = (Grid)(tabItem2.Content = new Grid());
			grid6.Name = "e_180";
			GamepadBinding gamepadBinding2 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding2.SetBinding(binding: new Binding("CreateOrderCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid6.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = grid6;
			RowDefinition item16 = new RowDefinition();
			grid6.RowDefinitions.Add(item16);
			RowDefinition item17 = new RowDefinition();
			grid6.RowDefinitions.Add(item17);
			RowDefinition item18 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.RowDefinitions.Add(item18);
			RowDefinition item19 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.RowDefinitions.Add(item19);
			ListBox listBox2 = new ListBox();
			grid6.Children.Add(listBox2);
			listBox2.Name = "e_181";
			listBox2.Margin = new Thickness(0f, 10f, 10f, 10f);
			listBox2.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			listBox2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox2.TabIndex = 230;
			Grid.SetColumn(listBox2, 0);
			GamepadHelp.SetTargetName(listBox2, "OrderListHelp");
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
			grid7.Name = "e_182";
			RowDefinition item20 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.RowDefinitions.Add(item20);
			RowDefinition item21 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.RowDefinitions.Add(item21);
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
			RowDefinition item24 = new RowDefinition();
			grid7.RowDefinitions.Add(item24);
			ColumnDefinition item25 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.ColumnDefinitions.Add(item25);
			ColumnDefinition item26 = new ColumnDefinition();
			grid7.ColumnDefinitions.Add(item26);
			Grid.SetColumn(grid7, 0);
			Grid.SetRow(grid7, 1);
			TextBlock textBlock11 = new TextBlock();
			grid7.Children.Add(textBlock11);
			textBlock11.Name = "e_183";
			textBlock11.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 0);
			textBlock11.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_AmountLabel");
			NumericTextBox numericTextBox3 = new NumericTextBox();
			grid7.Children.Add(numericTextBox3);
			numericTextBox3.Name = "e_184";
			numericTextBox3.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox3.TabIndex = 231;
			numericTextBox3.MaxLength = 5;
			numericTextBox3.Minimum = 0f;
			numericTextBox3.Maximum = 99999f;
			Grid.SetColumn(numericTextBox3, 1);
			GamepadHelp.SetTargetName(numericTextBox3, "OrderNumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox3, 30);
			GamepadHelp.SetTabIndexUp(numericTextBox3, 230);
			GamepadHelp.SetTabIndexDown(numericTextBox3, 232);
			numericTextBox3.SetBinding(binding: new Binding("OrderAmount")
			{
				UseGeneratedBindings = true
			}, property: NumericTextBox.ValueProperty);
			TextBlock textBlock12 = new TextBlock();
			grid7.Children.Add(textBlock12);
			textBlock12.Name = "e_185";
			textBlock12.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			textBlock12.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock12, 1);
			textBlock12.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_PricePerUnitLabel");
			NumericTextBox numericTextBox4 = new NumericTextBox();
			grid7.Children.Add(numericTextBox4);
			numericTextBox4.Name = "e_186";
			numericTextBox4.Margin = new Thickness(5f, 5f, 5f, 5f);
			numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox4.TabIndex = 232;
			numericTextBox4.MaxLength = 9;
			numericTextBox4.Minimum = 0f;
			Grid.SetColumn(numericTextBox4, 1);
			Grid.SetRow(numericTextBox4, 1);
			GamepadHelp.SetTargetName(numericTextBox4, "OrderNumericHelp");
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
			textBlock13.Name = "e_187";
			textBlock13.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			textBlock13.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock13, 0);
			Grid.SetRow(textBlock13, 2);
			textBlock13.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_TotalPriceLabel");
			StackPanel stackPanel4 = new StackPanel();
			grid7.Children.Add(stackPanel4);
			stackPanel4.Name = "e_188";
			stackPanel4.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel4.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel4.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel4, 1);
			Grid.SetRow(stackPanel4, 2);
			TextBlock textBlock14 = new TextBlock();
			stackPanel4.Children.Add(textBlock14);
			textBlock14.Name = "e_189";
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
			image5.Name = "e_190";
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
			textBlock15.Name = "e_191";
			textBlock15.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock15.VerticalAlignment = VerticalAlignment.Center;
			textBlock15.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock15, 0);
			Grid.SetRow(textBlock15, 3);
			textBlock15.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_ListingFee");
			StackPanel stackPanel5 = new StackPanel();
			grid7.Children.Add(stackPanel5);
			stackPanel5.Name = "e_192";
			stackPanel5.Margin = new Thickness(5f, 5f, 5f, 5f);
			stackPanel5.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel5.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel5, 1);
			Grid.SetRow(stackPanel5, 3);
			TextBlock textBlock16 = new TextBlock();
			stackPanel5.Children.Add(textBlock16);
			textBlock16.Name = "e_193";
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
			image6.Name = "e_194";
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
			grid8.Name = "e_195";
			grid8.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition item27 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item27);
			ColumnDefinition item28 = new ColumnDefinition();
			grid8.ColumnDefinitions.Add(item28);
			ColumnDefinition item29 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item29);
			Grid.SetRow(grid8, 4);
			Grid.SetColumnSpan(grid8, 2);
			TextBlock textBlock17 = new TextBlock();
			grid8.Children.Add(textBlock17);
			textBlock17.Name = "e_196";
			textBlock17.Margin = new Thickness(0f, 5f, 5f, 5f);
			textBlock17.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock17.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock17.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			TextBlock textBlock18 = new TextBlock();
			grid8.Children.Add(textBlock18);
			textBlock18.Name = "e_197";
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
			image7.Name = "e_198";
			image7.Height = 20f;
			image7.Margin = new Thickness(4f, 2f, 2f, 2f);
			image7.VerticalAlignment = VerticalAlignment.Bottom;
			image7.Stretch = Stretch.Uniform;
			Grid.SetColumn(image7, 2);
			image7.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
<<<<<<< HEAD
			observableCollection.Add(tabItem2);
			return observableCollection;
=======
			((Collection<object>)(object)obj).Add((object)tabItem2);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void InitializeElemente_154Resources(UIElement elem)
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
