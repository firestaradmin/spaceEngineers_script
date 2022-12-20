using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.AtmBlockView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class AtmBlockView_Gamepad : UIRoot
	{
		private Grid rootGrid;

		private Image e_0;

		private Grid e_1;

		private ImageButton e_2;

		private StackPanel e_3;

		private TextBlock e_4;

		private Border e_5;

		private Grid e_6;

		private Grid e_7;

		private Grid e_8;

		private ListBox e_9;

		private Grid e_14;

		private TextBlock e_15;

		private StackPanel e_16;

		private TextBlock e_17;

		private TextBlock e_18;

		private TextBlock e_19;

		private Grid e_20;

		private TextBlock e_21;

		private TextBlock e_22;

		private Image e_23;

		private TextBlock e_24;

		private Slider e_25;

		private TextBlock e_26;

		private Image e_27;

		private Border e_28;

		private Grid CashbackHelp;

		private TextBlock e_29;

		private TextBlock e_30;

		private TextBlock e_31;

		public AtmBlockView_Gamepad()
		{
			Initialize();
		}

		public AtmBlockView_Gamepad(int width, int height)
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
			rootGrid.Height = 700f;
			rootGrid.Width = 500f;
			rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
			rootGrid.VerticalAlignment = VerticalAlignment.Center;
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
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionATM");
			e_5 = new Border();
			e_3.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.Height = 2f;
			e_5.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_5.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_6 = new Grid();
			e_1.Children.Add(e_6);
			e_6.Name = "e_6";
			RowDefinition item3 = new RowDefinition();
			e_6.RowDefinitions.Add(item3);
			RowDefinition rowDefinition10 = new RowDefinition();
			rowDefinition10.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition10);
			RowDefinition rowDefinition11 = new RowDefinition();
			rowDefinition11.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition11);
			ColumnDefinition item4 = new ColumnDefinition();
			e_6.ColumnDefinitions.Add(item4);
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 3);
			Binding binding4 = new Binding("InventoryTargetViewModel");
			binding4.UseGeneratedBindings = true;
			e_6.SetBinding(UIElement.DataContextProperty, binding4);
			e_7 = new Grid();
			e_6.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_8 = new Grid();
			e_7.Children.Add(e_8);
			e_8.Name = "e_8";
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.CButton);
			Binding binding5 = new Binding("DepositCommand");
			binding5.UseGeneratedBindings = true;
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding5);
			e_8.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = e_8;
			GamepadBinding gamepadBinding2 = new GamepadBinding();
			gamepadBinding2.Gesture = new GamepadGesture(GamepadInput.DButton);
			Binding binding6 = new Binding("WithdrawCommand");
			binding6.UseGeneratedBindings = true;
			gamepadBinding2.SetBinding(InputBinding.CommandProperty, binding6);
			e_8.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = e_8;
			RowDefinition rowDefinition12 = new RowDefinition();
			rowDefinition12.Height = new GridLength(1f, GridUnitType.Auto);
			e_8.RowDefinitions.Add(rowDefinition12);
			RowDefinition rowDefinition13 = new RowDefinition();
			rowDefinition13.Height = new GridLength(1f, GridUnitType.Auto);
			e_8.RowDefinitions.Add(rowDefinition13);
			RowDefinition item5 = new RowDefinition();
			e_8.RowDefinitions.Add(item5);
			RowDefinition rowDefinition14 = new RowDefinition();
			rowDefinition14.Height = new GridLength(1f, GridUnitType.Auto);
			e_8.RowDefinitions.Add(rowDefinition14);
			RowDefinition rowDefinition15 = new RowDefinition();
			rowDefinition15.Height = new GridLength(1f, GridUnitType.Auto);
			e_8.RowDefinitions.Add(rowDefinition15);
			e_9 = new ListBox();
			e_8.Children.Add(e_9);
			e_9.Name = "e_9";
			e_9.Margin = new Thickness(0f, 5f, 0f, 0f);
			Style style = new Style(typeof(ListBox));
			Setter item6 = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item6);
			Func<UIElement, UIElement> createMethod = e_9_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod);
			Setter item7 = new Setter(Control.TemplateProperty, value);
			style.Setters.Add(item7);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsFocusedProperty;
			trigger.Value = true;
			Setter item8 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item8);
			style.Triggers.Add(trigger);
			e_9.Style = style;
			e_9.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_9.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_9.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			e_9.TabIndex = 0;
			Grid.SetRow(e_9, 2);
			GamepadHelp.SetTargetName(e_9, "CashbackHelp");
			GamepadHelp.SetTabIndexDown(e_9, 1);
			Binding binding7 = new Binding("InventoryItems");
			binding7.UseGeneratedBindings = true;
			e_9.SetBinding(ItemsControl.ItemsSourceProperty, binding7);
			InitializeElemente_9Resources(e_9);
			e_14 = new Grid();
			e_8.Children.Add(e_14);
			e_14.Name = "e_14";
			e_14.Margin = new Thickness(2f, 2f, 2f, 2f);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(1f, GridUnitType.Auto);
			e_14.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition item9 = new ColumnDefinition();
			e_14.ColumnDefinitions.Add(item9);
			Grid.SetRow(e_14, 3);
			e_15 = new TextBlock();
			e_14.Children.Add(e_15);
			e_15.Name = "e_15";
			e_15.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_15.VerticalAlignment = VerticalAlignment.Center;
			e_15.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_15.SetResourceReference(TextBlock.TextProperty, "ScreenTerminalInventory_Volume");
			e_16 = new StackPanel();
			e_14.Children.Add(e_16);
			e_16.Name = "e_16";
			e_16.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_16.HorizontalAlignment = HorizontalAlignment.Right;
			e_16.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_16, 1);
			e_17 = new TextBlock();
			e_16.Children.Add(e_17);
			e_17.Name = "e_17";
			e_17.VerticalAlignment = VerticalAlignment.Center;
			e_17.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding8 = new Binding("CurrentInventoryVolume");
			binding8.UseGeneratedBindings = true;
			e_17.SetBinding(TextBlock.TextProperty, binding8);
			e_18 = new TextBlock();
			e_16.Children.Add(e_18);
			e_18.Name = "e_18";
			e_18.Margin = new Thickness(10f, 0f, 10f, 0f);
			e_18.VerticalAlignment = VerticalAlignment.Center;
			e_18.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			e_18.Text = "/";
			e_19 = new TextBlock();
			e_16.Children.Add(e_19);
			e_19.Name = "e_19";
			e_19.VerticalAlignment = VerticalAlignment.Center;
			e_19.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding9 = new Binding("MaxInventoryVolume");
			binding9.UseGeneratedBindings = true;
			e_19.SetBinding(TextBlock.TextProperty, binding9);
			e_20 = new Grid();
			e_8.Children.Add(e_20);
			e_20.Name = "e_20";
			e_20.Margin = new Thickness(2f, 2f, 2f, 2f);
			RowDefinition rowDefinition16 = new RowDefinition();
			rowDefinition16.Height = new GridLength(1f, GridUnitType.Auto);
			e_20.RowDefinitions.Add(rowDefinition16);
			RowDefinition rowDefinition17 = new RowDefinition();
			rowDefinition17.Height = new GridLength(1f, GridUnitType.Auto);
			e_20.RowDefinitions.Add(rowDefinition17);
			RowDefinition rowDefinition18 = new RowDefinition();
			rowDefinition18.Height = new GridLength(1f, GridUnitType.Auto);
			e_20.RowDefinitions.Add(rowDefinition18);
			RowDefinition rowDefinition19 = new RowDefinition();
			rowDefinition19.Height = new GridLength(1f, GridUnitType.Auto);
			e_20.RowDefinitions.Add(rowDefinition19);
			RowDefinition rowDefinition20 = new RowDefinition();
			rowDefinition20.Height = new GridLength(65f, GridUnitType.Pixel);
			e_20.RowDefinitions.Add(rowDefinition20);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(1f, GridUnitType.Auto);
			e_20.ColumnDefinitions.Add(columnDefinition5);
			ColumnDefinition item10 = new ColumnDefinition();
			e_20.ColumnDefinitions.Add(item10);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(1f, GridUnitType.Auto);
			e_20.ColumnDefinitions.Add(columnDefinition6);
			Grid.SetRow(e_20, 4);
			e_21 = new TextBlock();
			e_20.Children.Add(e_21);
			e_21.Name = "e_21";
			e_21.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_21.VerticalAlignment = VerticalAlignment.Center;
			e_21.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_21.SetResourceReference(TextBlock.TextProperty, "Currency_Default_Account_Label");
			e_22 = new TextBlock();
			e_20.Children.Add(e_22);
			e_22.Name = "e_22";
			e_22.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_22.HorizontalAlignment = HorizontalAlignment.Right;
			e_22.VerticalAlignment = VerticalAlignment.Center;
			e_22.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(e_22, 1);
			Binding binding10 = new Binding("LocalPlayerCurrency");
			binding10.UseGeneratedBindings = true;
			e_22.SetBinding(TextBlock.TextProperty, binding10);
			e_23 = new Image();
			e_20.Children.Add(e_23);
			e_23.Name = "e_23";
			e_23.Height = 20f;
			e_23.Margin = new Thickness(4f, 2f, 2f, 2f);
			e_23.VerticalAlignment = VerticalAlignment.Center;
			e_23.Stretch = Stretch.Uniform;
			Grid.SetColumn(e_23, 2);
			Binding binding11 = new Binding("CurrencyIcon");
			binding11.UseGeneratedBindings = true;
			e_23.SetBinding(Image.SourceProperty, binding11);
			e_24 = new TextBlock();
			e_20.Children.Add(e_24);
			e_24.Name = "e_24";
			e_24.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_24.VerticalAlignment = VerticalAlignment.Center;
			e_24.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(e_24, 1);
			e_24.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_CashBack");
			e_25 = new Slider();
			e_20.Children.Add(e_25);
			e_25.Name = "e_25";
			e_25.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_25.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_25.VerticalAlignment = VerticalAlignment.Center;
			e_25.TabIndex = 1;
			e_25.Minimum = 0f;
			e_25.Maximum = 1000000f;
			e_25.IsSnapToTickEnabled = true;
			e_25.TickFrequency = 1f;
			Grid.SetColumn(e_25, 1);
			Grid.SetRow(e_25, 1);
			Grid.SetColumnSpan(e_25, 2);
			GamepadHelp.SetTargetName(e_25, "CashbackHelp");
			GamepadHelp.SetTabIndexUp(e_25, 0);
			Binding binding12 = new Binding("BalanceChangeValue");
			binding12.UseGeneratedBindings = true;
			e_25.SetBinding(RangeBase.ValueProperty, binding12);
			e_26 = new TextBlock();
			e_20.Children.Add(e_26);
			e_26.Name = "e_26";
			e_26.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_26.HorizontalAlignment = HorizontalAlignment.Right;
			e_26.VerticalAlignment = VerticalAlignment.Center;
			e_26.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(e_26, 1);
			Grid.SetRow(e_26, 2);
			Binding binding13 = new Binding("BalanceChangeValue");
			binding13.UseGeneratedBindings = true;
			e_26.SetBinding(TextBlock.TextProperty, binding13);
			e_27 = new Image();
			e_20.Children.Add(e_27);
			e_27.Name = "e_27";
			e_27.Height = 20f;
			e_27.Margin = new Thickness(4f, 2f, 2f, 2f);
			e_27.VerticalAlignment = VerticalAlignment.Center;
			e_27.Stretch = Stretch.Uniform;
			Grid.SetColumn(e_27, 2);
			Grid.SetRow(e_27, 2);
			Binding binding14 = new Binding("CurrencyIcon");
			binding14.UseGeneratedBindings = true;
			e_27.SetBinding(Image.SourceProperty, binding14);
			e_28 = new Border();
			e_20.Children.Add(e_28);
			e_28.Name = "e_28";
			e_28.Height = 2f;
			e_28.Margin = new Thickness(0f, 10f, 0f, 0f);
			e_28.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(e_28, 3);
			Grid.SetColumnSpan(e_28, 3);
			CashbackHelp = new Grid();
			e_20.Children.Add(CashbackHelp);
			CashbackHelp.Name = "CashbackHelp";
			CashbackHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition7 = new ColumnDefinition();
			columnDefinition7.Width = new GridLength(1f, GridUnitType.Auto);
			CashbackHelp.ColumnDefinitions.Add(columnDefinition7);
			ColumnDefinition columnDefinition8 = new ColumnDefinition();
			columnDefinition8.Width = new GridLength(1f, GridUnitType.Auto);
			CashbackHelp.ColumnDefinitions.Add(columnDefinition8);
			ColumnDefinition columnDefinition9 = new ColumnDefinition();
			columnDefinition9.Width = new GridLength(1f, GridUnitType.Auto);
			CashbackHelp.ColumnDefinitions.Add(columnDefinition9);
			ColumnDefinition item11 = new ColumnDefinition();
			CashbackHelp.ColumnDefinitions.Add(item11);
			Grid.SetRow(CashbackHelp, 4);
			Grid.SetColumnSpan(CashbackHelp, 3);
			e_29 = new TextBlock();
			CashbackHelp.Children.Add(e_29);
			e_29.Name = "e_29";
			e_29.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_29.HorizontalAlignment = HorizontalAlignment.Center;
			e_29.VerticalAlignment = VerticalAlignment.Center;
			e_29.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Deposit");
			e_30 = new TextBlock();
			CashbackHelp.Children.Add(e_30);
			e_30.Name = "e_30";
			e_30.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_30.HorizontalAlignment = HorizontalAlignment.Center;
			e_30.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_30, 1);
			e_30.SetResourceReference(TextBlock.TextProperty, "StoreScreen_Help_Withdraw");
			e_31 = new TextBlock();
			CashbackHelp.Children.Add(e_31);
			e_31.Name = "e_31";
			e_31.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_31.HorizontalAlignment = HorizontalAlignment.Center;
			e_31.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_31, 2);
			e_31.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "MaxWidth", typeof(MyStoreBlockViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "BackgroundOverlay", typeof(MyStoreBlockViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "ExitCommand", typeof(MyStoreBlockViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreBlockViewModel), "InventoryTargetViewModel", typeof(MyStoreBlockViewModel_InventoryTargetViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "DepositCommand", typeof(MyInventoryTargetViewModel_DepositCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "WithdrawCommand", typeof(MyInventoryTargetViewModel_WithdrawCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "InventoryItems", typeof(MyInventoryTargetViewModel_InventoryItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "CurrentInventoryVolume", typeof(MyInventoryTargetViewModel_CurrentInventoryVolume_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "MaxInventoryVolume", typeof(MyInventoryTargetViewModel_MaxInventoryVolume_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "LocalPlayerCurrency", typeof(MyInventoryTargetViewModel_LocalPlayerCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "CurrencyIcon", typeof(MyInventoryTargetViewModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetViewModel), "BalanceChangeValue", typeof(MyInventoryTargetViewModel_BalanceChangeValue_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesStoreBlock.Instance);
		}

		private static UIElement e_9_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_10",
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
			uniformGrid2.Name = "e_11";
			uniformGrid2.Margin = new Thickness(4f, 4f, 4f, 4f);
			uniformGrid2.VerticalAlignment = VerticalAlignment.Top;
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 5;
			return obj;
		}

		private static void InitializeElemente_9Resources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod = e_9r_0_s_S_1_ctMethod;
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

		private static UIElement e_9r_0_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_12"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_13";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
			contentPresenter.SetBinding(binding: new Binding(), property: ContentPresenter.ContentProperty);
			return obj;
		}
	}
}
