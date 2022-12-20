using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.PlayerTradeView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class PlayerTradeView_Gamepad : UIRoot
	{
		private Grid rootGrid;

		private Image e_2;

		private Grid e_3;

		private ImageButton e_4;

		private StackPanel e_5;

		private TextBlock e_6;

		private Border e_7;

		private TextBlock e_8;

		private TextBlock e_9;

		private TextBlock e_10;

		private ListBox e_11;

		private ListBox e_16;

		private Grid e_23;

		private Slider e_24;

		private StackPanel e_25;

		private TextBlock e_26;

		private Image e_27;

		private Grid e_28;

		private Slider e_29;

		private TextBlock e_30;

		private Border e_31;

		private Grid e_32;

		private ListBox e_33;

		private StackPanel e_40;

		private TextBlock e_41;

		private TextBlock e_42;

		private StackPanel e_43;

		private TextBlock e_44;

		private TextBlock e_45;

		private Image e_46;

		private StackPanel e_47;

		private TextBlock e_48;

		private TextBlock e_49;

		private Grid e_50;

		private StackPanel e_51;

		private TextBlock e_52;

		private TextBlock e_53;

		private Image e_54;

		private StackPanel e_55;

		private TextBlock e_56;

		private TextBlock e_57;

		private Border e_58;

		private Grid ListHelp;

		private TextBlock e_59;

		private TextBlock e_60;

		private Grid e_61;

		private TextBlock e_62;

		private TextBlock e_63;

		private Grid SlidersHelp;

		private TextBlock e_64;

		private TextBlock e_65;

		private Grid e_66;

		private TextBlock e_67;

		private TextBlock e_68;

		public PlayerTradeView_Gamepad()
		{
			Initialize();
		}

		public PlayerTradeView_Gamepad(int width, int height)
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
			e_2 = new Image();
			rootGrid.Children.Add(e_2);
			e_2.Name = "e_2";
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			e_2.Source = bitmapImage;
			e_2.Stretch = Stretch.Fill;
			Grid.SetRow(e_2, 1);
			Binding binding2 = new Binding("BackgroundOverlay");
			binding2.UseGeneratedBindings = true;
			e_2.SetBinding(ImageBrush.ColorOverlayProperty, binding2);
			e_3 = new Grid();
			rootGrid.Children.Add(e_3);
			e_3.Name = "e_3";
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.CButton);
			Binding binding3 = new Binding("SubmitAcceptCommand");
			binding3.UseGeneratedBindings = true;
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding3);
			e_3.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = e_3;
			RowDefinition rowDefinition3 = new RowDefinition();
			rowDefinition3.Height = new GridLength(1f, GridUnitType.Auto);
			e_3.RowDefinitions.Add(rowDefinition3);
			RowDefinition rowDefinition4 = new RowDefinition();
			rowDefinition4.Height = new GridLength(1f, GridUnitType.Auto);
			e_3.RowDefinitions.Add(rowDefinition4);
			RowDefinition rowDefinition5 = new RowDefinition();
			rowDefinition5.Height = new GridLength(1f, GridUnitType.Auto);
			e_3.RowDefinitions.Add(rowDefinition5);
			RowDefinition item2 = new RowDefinition();
			e_3.RowDefinitions.Add(item2);
			RowDefinition rowDefinition6 = new RowDefinition();
			rowDefinition6.Height = new GridLength(1f, GridUnitType.Auto);
			e_3.RowDefinitions.Add(rowDefinition6);
			RowDefinition rowDefinition7 = new RowDefinition();
			rowDefinition7.Height = new GridLength(1f, GridUnitType.Auto);
			e_3.RowDefinitions.Add(rowDefinition7);
			RowDefinition rowDefinition8 = new RowDefinition();
			rowDefinition8.Height = new GridLength(1f, GridUnitType.Auto);
			e_3.RowDefinitions.Add(rowDefinition8);
			RowDefinition rowDefinition9 = new RowDefinition();
			rowDefinition9.Height = new GridLength(65f, GridUnitType.Pixel);
			e_3.RowDefinitions.Add(rowDefinition9);
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.Width = new GridLength(75f, GridUnitType.Pixel);
			e_3.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition columnDefinition2 = new ColumnDefinition();
			columnDefinition2.Width = new GridLength(4f, GridUnitType.Star);
			e_3.ColumnDefinitions.Add(columnDefinition2);
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition3.Width = new GridLength(2f, GridUnitType.Star);
			e_3.ColumnDefinitions.Add(columnDefinition3);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(1f, GridUnitType.Auto);
			e_3.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(2f, GridUnitType.Star);
			e_3.ColumnDefinitions.Add(columnDefinition5);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(75f, GridUnitType.Pixel);
			e_3.ColumnDefinitions.Add(columnDefinition6);
			Grid.SetRow(e_3, 1);
			e_4 = new ImageButton();
			e_3.Children.Add(e_4);
			e_4.Name = "e_4";
			e_4.Height = 24f;
			e_4.Width = 24f;
			e_4.Margin = new Thickness(16f, 16f, 16f, 16f);
			e_4.HorizontalAlignment = HorizontalAlignment.Right;
			e_4.VerticalAlignment = VerticalAlignment.Center;
			e_4.IsTabStop = false;
			e_4.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage2 = new BitmapImage();
			bitmapImage2.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			e_4.ImageNormal = bitmapImage2;
			BitmapImage bitmapImage3 = new BitmapImage();
			bitmapImage3.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_4.ImageHover = bitmapImage3;
			BitmapImage bitmapImage4 = new BitmapImage();
			bitmapImage4.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_4.ImagePressed = bitmapImage4;
			Grid.SetColumn(e_4, 5);
			Binding binding4 = new Binding("ExitCommand");
			binding4.UseGeneratedBindings = true;
			e_4.SetBinding(Button.CommandProperty, binding4);
			e_5 = new StackPanel();
			e_3.Children.Add(e_5);
			e_5.Name = "e_5";
			Grid.SetColumn(e_5, 1);
			Grid.SetRow(e_5, 1);
			Grid.SetColumnSpan(e_5, 4);
			e_6 = new TextBlock();
			e_5.Children.Add(e_6);
			e_6.Name = "e_6";
			e_6.HorizontalAlignment = HorizontalAlignment.Center;
			e_6.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_6.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionPlayerTrade");
			e_7 = new Border();
			e_5.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Height = 2f;
			e_7.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_7.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_8 = new TextBlock();
			e_3.Children.Add(e_8);
			e_8.Name = "e_8";
			e_8.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_8.HorizontalAlignment = HorizontalAlignment.Left;
			e_8.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(e_8, 1);
			Grid.SetRow(e_8, 2);
			e_8.SetResourceReference(TextBlock.TextProperty, "TradeScreenYoursInventory");
			e_9 = new TextBlock();
			e_3.Children.Add(e_9);
			e_9.Name = "e_9";
			e_9.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_9.HorizontalAlignment = HorizontalAlignment.Left;
			e_9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(e_9, 2);
			Grid.SetRow(e_9, 2);
			e_9.SetResourceReference(TextBlock.TextProperty, "TradeScreenYoursOffer");
			e_10 = new TextBlock();
			e_3.Children.Add(e_10);
			e_10.Name = "e_10";
			e_10.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_10.HorizontalAlignment = HorizontalAlignment.Left;
			e_10.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(e_10, 4);
			Grid.SetRow(e_10, 2);
			e_10.SetResourceReference(TextBlock.TextProperty, "TradeScreenOtherOffer");
			e_11 = new ListBox();
			e_3.Children.Add(e_11);
			e_11.Name = "e_11";
			e_11.Margin = new Thickness(0f, 10f, 10f, 10f);
			Style style = new Style(typeof(ListBox));
			Setter item3 = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item3);
			Func<UIElement, UIElement> createMethod = e_11_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod);
			Setter item4 = new Setter(Control.TemplateProperty, value);
			style.Setters.Add(item4);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsFocusedProperty;
			trigger.Value = true;
			Setter item5 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item5);
			style.Triggers.Add(trigger);
			e_11.Style = style;
			GamepadBinding gamepadBinding2 = new GamepadBinding();
			gamepadBinding2.Gesture = new GamepadGesture(GamepadInput.AButton);
			Binding binding5 = new Binding("AddItemCommand");
			binding5.UseGeneratedBindings = true;
			gamepadBinding2.SetBinding(InputBinding.CommandProperty, binding5);
			e_11.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = e_11;
			e_11.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_11.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_11.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
			e_11.TabIndex = 0;
			Grid.SetColumn(e_11, 1);
			Grid.SetRow(e_11, 3);
			Grid.SetRowSpan(e_11, 2);
			GamepadHelp.SetTargetName(e_11, "ListHelp");
			GamepadHelp.SetTabIndexRight(e_11, 1);
			DragDrop.SetIsDragSource(e_11, value: true);
			DragDrop.SetIsDropTarget(e_11, value: true);
			Binding binding6 = new Binding("InventoryItems");
			binding6.UseGeneratedBindings = true;
			e_11.SetBinding(ItemsControl.ItemsSourceProperty, binding6);
			Binding binding7 = new Binding("SelectedInventoryItem");
			binding7.UseGeneratedBindings = true;
			e_11.SetBinding(Selector.SelectedItemProperty, binding7);
			InitializeElemente_11Resources(e_11);
			e_16 = new ListBox();
			e_3.Children.Add(e_16);
			e_16.Name = "e_16";
			e_16.Margin = new Thickness(0f, 10f, 0f, 10f);
			GamepadBinding gamepadBinding3 = new GamepadBinding();
			gamepadBinding3.Gesture = new GamepadGesture(GamepadInput.AButton);
			Binding binding8 = new Binding("RemoveItemCommand");
			binding8.UseGeneratedBindings = true;
			gamepadBinding3.SetBinding(InputBinding.CommandProperty, binding8);
			e_16.InputBindings.Add(gamepadBinding3);
			gamepadBinding3.Parent = e_16;
			e_16.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_16.TabIndex = 1;
			Func<UIElement, UIElement> createMethod2 = e_16_dtMethod;
			e_16.ItemTemplate = new DataTemplate(typeof(MyInventoryItemModel), createMethod2);
			Grid.SetColumn(e_16, 2);
			Grid.SetRow(e_16, 3);
			GamepadHelp.SetTargetName(e_16, "ListHelp");
			GamepadHelp.SetTabIndexLeft(e_16, 0);
			GamepadHelp.SetTabIndexDown(e_16, 2);
			DragDrop.SetIsDragSource(e_16, value: true);
			DragDrop.SetIsDropTarget(e_16, value: true);
			Binding binding9 = new Binding("LocalPlayerOfferItems");
			binding9.UseGeneratedBindings = true;
			e_16.SetBinding(ItemsControl.ItemsSourceProperty, binding9);
			Binding binding10 = new Binding("SelectedOfferItem");
			binding10.UseGeneratedBindings = true;
			e_16.SetBinding(Selector.SelectedItemProperty, binding10);
			e_23 = new Grid();
			e_3.Children.Add(e_23);
			e_23.Name = "e_23";
			RowDefinition item6 = new RowDefinition();
			e_23.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition();
			e_23.RowDefinitions.Add(item7);
			Grid.SetColumn(e_23, 2);
			Grid.SetRow(e_23, 4);
			e_24 = new Slider();
			e_23.Children.Add(e_24);
			e_24.Name = "e_24";
			e_24.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_24.TabIndex = 2;
			e_24.Minimum = 0f;
			e_24.IsSnapToTickEnabled = true;
			e_24.TickFrequency = 1f;
			GamepadHelp.SetTargetName(e_24, "SlidersHelp");
			GamepadHelp.SetTabIndexLeft(e_24, 0);
			GamepadHelp.SetTabIndexUp(e_24, 1);
			GamepadHelp.SetTabIndexDown(e_24, 3);
			Binding binding11 = new Binding("LocalPlayerOfferCurrencyMaximum");
			binding11.UseGeneratedBindings = true;
			e_24.SetBinding(RangeBase.MaximumProperty, binding11);
			Binding binding12 = new Binding("LocalPlayerOfferCurrency");
			binding12.UseGeneratedBindings = true;
			e_24.SetBinding(RangeBase.ValueProperty, binding12);
			e_25 = new StackPanel();
			e_23.Children.Add(e_25);
			e_25.Name = "e_25";
			e_25.HorizontalAlignment = HorizontalAlignment.Right;
			e_25.Orientation = Orientation.Horizontal;
			Grid.SetRow(e_25, 1);
			e_26 = new TextBlock();
			e_25.Children.Add(e_26);
			e_26.Name = "e_26";
			e_26.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_26.HorizontalAlignment = HorizontalAlignment.Right;
			e_26.VerticalAlignment = VerticalAlignment.Center;
			e_26.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding13 = new Binding("LocalPlayerOfferCurrency");
			binding13.UseGeneratedBindings = true;
			e_26.SetBinding(TextBlock.TextProperty, binding13);
			e_27 = new Image();
			e_25.Children.Add(e_27);
			e_27.Name = "e_27";
			e_27.Height = 20f;
			e_27.Margin = new Thickness(4f, 2f, 2f, 2f);
			e_27.VerticalAlignment = VerticalAlignment.Center;
			e_27.Stretch = Stretch.Uniform;
			Binding binding14 = new Binding("CurrencyIcon");
			binding14.UseGeneratedBindings = true;
			e_27.SetBinding(Image.SourceProperty, binding14);
			e_28 = new Grid();
			e_3.Children.Add(e_28);
			e_28.Name = "e_28";
			ColumnDefinition item8 = new ColumnDefinition();
			e_28.ColumnDefinitions.Add(item8);
			ColumnDefinition columnDefinition7 = new ColumnDefinition();
			columnDefinition7.Width = new GridLength(60f, GridUnitType.Pixel);
			e_28.ColumnDefinitions.Add(columnDefinition7);
			Grid.SetColumn(e_28, 2);
			Grid.SetRow(e_28, 5);
			Binding binding15 = new Binding("IsPcuVisible");
			binding15.UseGeneratedBindings = true;
			e_28.SetBinding(UIElement.VisibilityProperty, binding15);
			e_29 = new Slider();
			e_28.Children.Add(e_29);
			e_29.Name = "e_29";
			e_29.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_29.TabIndex = 3;
			e_29.Minimum = 0f;
			e_29.Maximum = 99999f;
			e_29.IsSnapToTickEnabled = true;
			e_29.TickFrequency = 1f;
			GamepadHelp.SetTargetName(e_29, "SlidersHelp");
			GamepadHelp.SetTabIndexLeft(e_29, 0);
			GamepadHelp.SetTabIndexUp(e_29, 2);
			Binding binding16 = new Binding("LocalPlayerOfferPcu");
			binding16.UseGeneratedBindings = true;
			e_29.SetBinding(RangeBase.ValueProperty, binding16);
			e_30 = new TextBlock();
			e_28.Children.Add(e_30);
			e_30.Name = "e_30";
			e_30.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_30.HorizontalAlignment = HorizontalAlignment.Right;
			e_30.VerticalAlignment = VerticalAlignment.Center;
			e_30.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(e_30, 1);
			Binding binding17 = new Binding("LocalPlayerOfferPcu");
			binding17.UseGeneratedBindings = true;
			e_30.SetBinding(TextBlock.TextProperty, binding17);
			e_31 = new Border();
			e_3.Children.Add(e_31);
			e_31.Name = "e_31";
			e_31.Width = 2f;
			e_31.Margin = new Thickness(5f, 10f, 5f, 10f);
			e_31.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_31, 3);
			Grid.SetRow(e_31, 2);
			Grid.SetRowSpan(e_31, 4);
			e_32 = new Grid();
			e_3.Children.Add(e_32);
			e_32.Name = "e_32";
			RowDefinition item9 = new RowDefinition();
			e_32.RowDefinitions.Add(item9);
			RowDefinition rowDefinition10 = new RowDefinition();
			rowDefinition10.Height = new GridLength(1f, GridUnitType.Auto);
			e_32.RowDefinitions.Add(rowDefinition10);
			Grid.SetColumn(e_32, 4);
			Grid.SetRow(e_32, 3);
			e_33 = new ListBox();
			e_32.Children.Add(e_33);
			e_33.Name = "e_33";
			e_33.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_33.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_33.TabIndex = 4;
			Func<UIElement, UIElement> createMethod3 = e_33_dtMethod;
			e_33.ItemTemplate = new DataTemplate(typeof(MyInventoryItemModel), createMethod3);
			Grid.SetRow(e_33, 0);
			Binding binding18 = new Binding("OtherPlayerOfferItems");
			binding18.UseGeneratedBindings = true;
			e_33.SetBinding(ItemsControl.ItemsSourceProperty, binding18);
			e_40 = new StackPanel();
			e_32.Children.Add(e_40);
			e_40.Name = "e_40";
			e_40.VerticalAlignment = VerticalAlignment.Bottom;
			e_40.Orientation = Orientation.Horizontal;
			Grid.SetRow(e_40, 1);
			e_41 = new TextBlock();
			e_40.Children.Add(e_41);
			e_41.Name = "e_41";
			e_41.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_41.VerticalAlignment = VerticalAlignment.Center;
			e_41.SetResourceReference(TextBlock.TextProperty, "TradeScreenOfferState");
			e_42 = new TextBlock();
			e_40.Children.Add(e_42);
			e_42.Name = "e_42";
			e_42.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_42.VerticalAlignment = VerticalAlignment.Center;
			e_42.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding19 = new Binding("OtherPlayerAcceptState");
			binding19.UseGeneratedBindings = true;
			e_42.SetBinding(TextBlock.TextProperty, binding19);
			e_43 = new StackPanel();
			e_3.Children.Add(e_43);
			e_43.Name = "e_43";
			e_43.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_43, 4);
			Grid.SetRow(e_43, 4);
			e_44 = new TextBlock();
			e_43.Children.Add(e_44);
			e_44.Name = "e_44";
			e_44.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_44.VerticalAlignment = VerticalAlignment.Center;
			e_44.SetResourceReference(TextBlock.TextProperty, "TradeScreenMoney");
			e_45 = new TextBlock();
			e_43.Children.Add(e_45);
			e_45.Name = "e_45";
			e_45.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_45.VerticalAlignment = VerticalAlignment.Center;
			e_45.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding20 = new Binding("OtherPlayerOfferCurrency");
			binding20.UseGeneratedBindings = true;
			e_45.SetBinding(TextBlock.TextProperty, binding20);
			e_46 = new Image();
			e_43.Children.Add(e_46);
			e_46.Name = "e_46";
			e_46.Height = 20f;
			e_46.Margin = new Thickness(3f, 2f, 2f, 0f);
			e_46.VerticalAlignment = VerticalAlignment.Center;
			e_46.Stretch = Stretch.Uniform;
			Binding binding21 = new Binding("CurrencyIcon");
			binding21.UseGeneratedBindings = true;
			e_46.SetBinding(Image.SourceProperty, binding21);
			e_47 = new StackPanel();
			e_3.Children.Add(e_47);
			e_47.Name = "e_47";
			e_47.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_47, 4);
			Grid.SetRow(e_47, 5);
			Binding binding22 = new Binding("IsPcuVisible");
			binding22.UseGeneratedBindings = true;
			e_47.SetBinding(UIElement.VisibilityProperty, binding22);
			e_48 = new TextBlock();
			e_47.Children.Add(e_48);
			e_48.Name = "e_48";
			e_48.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_48.VerticalAlignment = VerticalAlignment.Center;
			e_48.SetResourceReference(TextBlock.TextProperty, "TradeScreenPcu");
			e_49 = new TextBlock();
			e_47.Children.Add(e_49);
			e_49.Name = "e_49";
			e_49.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_49.VerticalAlignment = VerticalAlignment.Center;
			e_49.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding23 = new Binding("OtherPlayerOfferPcu");
			binding23.UseGeneratedBindings = true;
			e_49.SetBinding(TextBlock.TextProperty, binding23);
			e_50 = new Grid();
			e_3.Children.Add(e_50);
			e_50.Name = "e_50";
			ColumnDefinition item10 = new ColumnDefinition();
			e_50.ColumnDefinitions.Add(item10);
			ColumnDefinition item11 = new ColumnDefinition();
			e_50.ColumnDefinitions.Add(item11);
			Grid.SetColumn(e_50, 1);
			Grid.SetRow(e_50, 5);
			e_51 = new StackPanel();
			e_50.Children.Add(e_51);
			e_51.Name = "e_51";
			e_51.HorizontalAlignment = HorizontalAlignment.Left;
			e_51.Orientation = Orientation.Horizontal;
			e_52 = new TextBlock();
			e_51.Children.Add(e_52);
			e_52.Name = "e_52";
			e_52.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_52.VerticalAlignment = VerticalAlignment.Center;
			e_52.SetResourceReference(TextBlock.TextProperty, "TradeScreenMoney");
			e_53 = new TextBlock();
			e_51.Children.Add(e_53);
			e_53.Name = "e_53";
			e_53.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_53.VerticalAlignment = VerticalAlignment.Center;
			e_53.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding24 = new Binding("LocalPlayerCurrency");
			binding24.UseGeneratedBindings = true;
			e_53.SetBinding(TextBlock.TextProperty, binding24);
			e_54 = new Image();
			e_51.Children.Add(e_54);
			e_54.Name = "e_54";
			e_54.Height = 20f;
			e_54.Margin = new Thickness(3f, 2f, 2f, 0f);
			e_54.VerticalAlignment = VerticalAlignment.Center;
			e_54.Stretch = Stretch.Uniform;
			Binding binding25 = new Binding("CurrencyIcon");
			binding25.UseGeneratedBindings = true;
			e_54.SetBinding(Image.SourceProperty, binding25);
			e_55 = new StackPanel();
			e_50.Children.Add(e_55);
			e_55.Name = "e_55";
			e_55.HorizontalAlignment = HorizontalAlignment.Left;
			e_55.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_55, 1);
			Binding binding26 = new Binding("IsPcuVisible");
			binding26.UseGeneratedBindings = true;
			e_55.SetBinding(UIElement.VisibilityProperty, binding26);
			e_56 = new TextBlock();
			e_55.Children.Add(e_56);
			e_56.Name = "e_56";
			e_56.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_56.VerticalAlignment = VerticalAlignment.Center;
			e_56.SetResourceReference(TextBlock.TextProperty, "TradeScreenPcu");
			e_57 = new TextBlock();
			e_55.Children.Add(e_57);
			e_57.Name = "e_57";
			e_57.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_57.VerticalAlignment = VerticalAlignment.Center;
			e_57.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding27 = new Binding("LocalPlayerPcu");
			binding27.UseGeneratedBindings = true;
			e_57.SetBinding(TextBlock.TextProperty, binding27);
			e_58 = new Border();
			e_3.Children.Add(e_58);
			e_58.Name = "e_58";
			e_58.Height = 2f;
			e_58.Margin = new Thickness(0f, 10f, 0f, 0f);
			e_58.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_58, 1);
			Grid.SetRow(e_58, 6);
			Grid.SetColumnSpan(e_58, 4);
			ListHelp = new Grid();
			e_3.Children.Add(ListHelp);
			ListHelp.Name = "ListHelp";
			ListHelp.Visibility = Visibility.Collapsed;
			ListHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition8 = new ColumnDefinition();
			columnDefinition8.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition8);
			ColumnDefinition columnDefinition9 = new ColumnDefinition();
			columnDefinition9.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition9);
			ColumnDefinition columnDefinition10 = new ColumnDefinition();
			columnDefinition10.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition10);
			ColumnDefinition columnDefinition11 = new ColumnDefinition();
			columnDefinition11.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition11);
			ColumnDefinition item12 = new ColumnDefinition();
			ListHelp.ColumnDefinitions.Add(item12);
			Grid.SetColumn(ListHelp, 1);
			Grid.SetRow(ListHelp, 7);
			Grid.SetColumnSpan(ListHelp, 3);
			e_59 = new TextBlock();
			ListHelp.Children.Add(e_59);
			e_59.Name = "e_59";
			e_59.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_59.HorizontalAlignment = HorizontalAlignment.Center;
			e_59.VerticalAlignment = VerticalAlignment.Center;
			e_59.SetResourceReference(TextBlock.TextProperty, "PlayerTrade_Help_Transfer");
			e_60 = new TextBlock();
			ListHelp.Children.Add(e_60);
			e_60.Name = "e_60";
			e_60.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_60.HorizontalAlignment = HorizontalAlignment.Center;
			e_60.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_60, 1);
			Binding binding28 = new Binding("IsSubmitOfferEnabled");
			binding28.UseGeneratedBindings = true;
			e_60.SetBinding(UIElement.VisibilityProperty, binding28);
			e_60.SetResourceReference(TextBlock.TextProperty, "PlayerTrade_Help_SubmitOffer");
			e_61 = new Grid();
			ListHelp.Children.Add(e_61);
			e_61.Name = "e_61";
			Grid.SetColumn(e_61, 1);
			Binding binding29 = new Binding("IsAcceptVisible");
			binding29.UseGeneratedBindings = true;
			e_61.SetBinding(UIElement.VisibilityProperty, binding29);
			e_62 = new TextBlock();
			e_61.Children.Add(e_62);
			e_62.Name = "e_62";
			e_62.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_62.HorizontalAlignment = HorizontalAlignment.Center;
			e_62.VerticalAlignment = VerticalAlignment.Center;
			Binding binding30 = new Binding("IsAcceptEnabled");
			binding30.UseGeneratedBindings = true;
			e_62.SetBinding(UIElement.VisibilityProperty, binding30);
			e_62.SetResourceReference(TextBlock.TextProperty, "PlayerTrade_Help_AcceptOffer");
			e_63 = new TextBlock();
			ListHelp.Children.Add(e_63);
			e_63.Name = "e_63";
			e_63.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_63.HorizontalAlignment = HorizontalAlignment.Center;
			e_63.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_63, 2);
			e_63.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			SlidersHelp = new Grid();
			e_3.Children.Add(SlidersHelp);
			SlidersHelp.Name = "SlidersHelp";
			SlidersHelp.Visibility = Visibility.Collapsed;
			SlidersHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition12 = new ColumnDefinition();
			columnDefinition12.Width = new GridLength(1f, GridUnitType.Auto);
			SlidersHelp.ColumnDefinitions.Add(columnDefinition12);
			ColumnDefinition columnDefinition13 = new ColumnDefinition();
			columnDefinition13.Width = new GridLength(1f, GridUnitType.Auto);
			SlidersHelp.ColumnDefinitions.Add(columnDefinition13);
			ColumnDefinition columnDefinition14 = new ColumnDefinition();
			columnDefinition14.Width = new GridLength(1f, GridUnitType.Auto);
			SlidersHelp.ColumnDefinitions.Add(columnDefinition14);
			ColumnDefinition columnDefinition15 = new ColumnDefinition();
			columnDefinition15.Width = new GridLength(1f, GridUnitType.Auto);
			SlidersHelp.ColumnDefinitions.Add(columnDefinition15);
			ColumnDefinition item13 = new ColumnDefinition();
			SlidersHelp.ColumnDefinitions.Add(item13);
			Grid.SetColumn(SlidersHelp, 1);
			Grid.SetRow(SlidersHelp, 7);
			Grid.SetColumnSpan(SlidersHelp, 3);
			e_64 = new TextBlock();
			SlidersHelp.Children.Add(e_64);
			e_64.Name = "e_64";
			e_64.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_64.HorizontalAlignment = HorizontalAlignment.Center;
			e_64.VerticalAlignment = VerticalAlignment.Center;
			e_64.SetResourceReference(TextBlock.TextProperty, "PlayerTrade_Help_ChangeValue");
			e_65 = new TextBlock();
			SlidersHelp.Children.Add(e_65);
			e_65.Name = "e_65";
			e_65.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_65.HorizontalAlignment = HorizontalAlignment.Center;
			e_65.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_65, 1);
			Binding binding31 = new Binding("IsSubmitOfferEnabled");
			binding31.UseGeneratedBindings = true;
			e_65.SetBinding(UIElement.VisibilityProperty, binding31);
			e_65.SetResourceReference(TextBlock.TextProperty, "PlayerTrade_Help_SubmitOffer");
			e_66 = new Grid();
			SlidersHelp.Children.Add(e_66);
			e_66.Name = "e_66";
			Grid.SetColumn(e_66, 1);
			Binding binding32 = new Binding("IsAcceptVisible");
			binding32.UseGeneratedBindings = true;
			e_66.SetBinding(UIElement.VisibilityProperty, binding32);
			e_67 = new TextBlock();
			e_66.Children.Add(e_67);
			e_67.Name = "e_67";
			e_67.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_67.HorizontalAlignment = HorizontalAlignment.Center;
			e_67.VerticalAlignment = VerticalAlignment.Center;
			Binding binding33 = new Binding("IsAcceptEnabled");
			binding33.UseGeneratedBindings = true;
			e_67.SetBinding(UIElement.VisibilityProperty, binding33);
			e_67.SetResourceReference(TextBlock.TextProperty, "PlayerTrade_Help_AcceptOffer");
			e_68 = new TextBlock();
			SlidersHelp.Children.Add(e_68);
			e_68.Name = "e_68";
			e_68.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_68.HorizontalAlignment = HorizontalAlignment.Center;
			e_68.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_68, 2);
			e_68.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "MaxWidth", typeof(MyPlayerTradeViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "BackgroundOverlay", typeof(MyPlayerTradeViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "SubmitAcceptCommand", typeof(MyPlayerTradeViewModel_SubmitAcceptCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "ExitCommand", typeof(MyPlayerTradeViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "AddItemCommand", typeof(MyPlayerTradeViewModel_AddItemCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "InventoryItems", typeof(MyPlayerTradeViewModel_InventoryItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "SelectedInventoryItem", typeof(MyPlayerTradeViewModel_SelectedInventoryItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "RemoveItemCommand", typeof(MyPlayerTradeViewModel_RemoveItemCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Icon", typeof(MyInventoryItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "AmountFormatted", typeof(MyInventoryItemModel_AmountFormatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "IconSymbol", typeof(MyInventoryItemModel_IconSymbol_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Name", typeof(MyInventoryItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferItems", typeof(MyPlayerTradeViewModel_LocalPlayerOfferItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "SelectedOfferItem", typeof(MyPlayerTradeViewModel_SelectedOfferItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferCurrencyMaximum", typeof(MyPlayerTradeViewModel_LocalPlayerOfferCurrencyMaximum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferCurrency", typeof(MyPlayerTradeViewModel_LocalPlayerOfferCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "CurrencyIcon", typeof(MyPlayerTradeViewModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsPcuVisible", typeof(MyPlayerTradeViewModel_IsPcuVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferPcu", typeof(MyPlayerTradeViewModel_LocalPlayerOfferPcu_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerOfferItems", typeof(MyPlayerTradeViewModel_OtherPlayerOfferItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerAcceptState", typeof(MyPlayerTradeViewModel_OtherPlayerAcceptState_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerOfferCurrency", typeof(MyPlayerTradeViewModel_OtherPlayerOfferCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerOfferPcu", typeof(MyPlayerTradeViewModel_OtherPlayerOfferPcu_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerCurrency", typeof(MyPlayerTradeViewModel_LocalPlayerCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerPcu", typeof(MyPlayerTradeViewModel_LocalPlayerPcu_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsSubmitOfferEnabled", typeof(MyPlayerTradeViewModel_IsSubmitOfferEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsAcceptVisible", typeof(MyPlayerTradeViewModel_IsAcceptVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsAcceptEnabled", typeof(MyPlayerTradeViewModel_IsAcceptEnabled_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplates.Instance);
			Style style = new Style(typeof(ListBox));
			Setter item = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod = r_0_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod);
			Setter item2 = new Setter(Control.TemplateProperty, value);
			style.Setters.Add(item2);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsFocusedProperty;
			trigger.Value = true;
			Setter item3 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item3);
			style.Triggers.Add(trigger);
			elem.Resources.Add("ListBoxGridLarge", style);
		}

		private static UIElement r_0_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_0",
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
			uniformGrid2.Name = "e_1";
			uniformGrid2.Margin = new Thickness(4f, 4f, 4f, 4f);
			uniformGrid2.VerticalAlignment = VerticalAlignment.Top;
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 7;
			return obj;
		}

		private static UIElement e_11_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_12",
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
			uniformGrid2.Name = "e_13";
			uniformGrid2.Margin = new Thickness(4f, 4f, 4f, 4f);
			uniformGrid2.VerticalAlignment = VerticalAlignment.Top;
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 7;
			return obj;
		}

		private static void InitializeElemente_11Resources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod = e_11r_0_s_S_1_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ListBoxItem), createMethod);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsMouseOverProperty;
			trigger.Value = true;
			Setter item2 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItemHover"));
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

		private static UIElement e_11r_0_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_14"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_15";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
			contentPresenter.SetBinding(binding: new Binding(), property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_16_dtMethod(UIElement parent)
		{
			Grid grid = new Grid();
			grid.Parent = parent;
			grid.Name = "e_17";
			grid.HorizontalAlignment = HorizontalAlignment.Stretch;
			MouseBinding mouseBinding = new MouseBinding();
			mouseBinding.Gesture = new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.None);
			Binding binding = new Binding("ViewModel.RemoveItemFromOfferCommand");
			binding.Source = new MyPlayerTradeViewModelLocator(isDesignMode: false);
			mouseBinding.SetBinding(InputBinding.CommandProperty, binding);
			Binding binding2 = new Binding();
			mouseBinding.SetBinding(InputBinding.CommandParameterProperty, binding2);
			grid.InputBindings.Add(mouseBinding);
			mouseBinding.Parent = grid;
			MouseBinding mouseBinding2 = new MouseBinding();
			mouseBinding2.Gesture = new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control);
			Binding binding3 = new Binding("ViewModel.RemoveStackTenToOfferCommand");
			binding3.Source = new MyPlayerTradeViewModelLocator(isDesignMode: false);
			mouseBinding2.SetBinding(InputBinding.CommandProperty, binding3);
			Binding binding4 = new Binding();
			mouseBinding2.SetBinding(InputBinding.CommandParameterProperty, binding4);
			grid.InputBindings.Add(mouseBinding2);
			mouseBinding2.Parent = grid;
			MouseBinding mouseBinding3 = new MouseBinding();
			mouseBinding3.Gesture = new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Shift);
			Binding binding5 = new Binding("ViewModel.RemoveStackHundredToOfferCommand");
			binding5.Source = new MyPlayerTradeViewModelLocator(isDesignMode: false);
			mouseBinding3.SetBinding(InputBinding.CommandProperty, binding5);
			Binding binding6 = new Binding();
			mouseBinding3.SetBinding(InputBinding.CommandParameterProperty, binding6);
			grid.InputBindings.Add(mouseBinding3);
			mouseBinding3.Parent = grid;
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.Width = new GridLength(1f, GridUnitType.Auto);
			grid.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition item = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item);
			Border border = new Border();
			grid.Children.Add(border);
			border.Name = "e_18";
			border.Height = 64f;
			border.Width = 64f;
			border.SetResourceReference(Control.BackgroundProperty, "GridItem");
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_19";
			image.Margin = new Thickness(5f, 5f, 5f, 5f);
			image.Stretch = Stretch.Fill;
			Binding binding7 = new Binding("Icon");
			binding7.UseGeneratedBindings = true;
			image.SetBinding(Image.SourceProperty, binding7);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_20";
			textBlock.Margin = new Thickness(6f, 0f, 0f, 4f);
			textBlock.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock.TextAlignment = TextAlignment.Left;
			textBlock.FontFamily = new FontFamily("InventorySmall");
			textBlock.FontSize = 10f;
			textBlock.FontStyle = FontStyle.Regular;
			Binding binding8 = new Binding("AmountFormatted");
			binding8.UseGeneratedBindings = true;
			textBlock.SetBinding(TextBlock.TextProperty, binding8);
			TextBlock textBlock2 = new TextBlock();
			grid.Children.Add(textBlock2);
			textBlock2.Name = "e_21";
			textBlock2.Margin = new Thickness(6f, 4f, 0f, 0f);
			textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock2.VerticalAlignment = VerticalAlignment.Top;
			textBlock2.TextAlignment = TextAlignment.Left;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			Binding binding9 = new Binding("IconSymbol");
			binding9.UseGeneratedBindings = true;
			textBlock2.SetBinding(TextBlock.TextProperty, binding9);
			TextBlock textBlock3 = new TextBlock();
			grid.Children.Add(textBlock3);
			textBlock3.Name = "e_22";
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock3, 1);
			Binding binding10 = new Binding("Name");
			binding10.UseGeneratedBindings = true;
			textBlock3.SetBinding(TextBlock.TextProperty, binding10);
			return grid;
		}

		private static UIElement e_33_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_34",
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			ColumnDefinition item = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			obj.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item2);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_35";
			border.Height = 64f;
			border.Width = 64f;
			border.SetResourceReference(Control.BackgroundProperty, "GridItem");
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_36";
			image.Margin = new Thickness(5f, 5f, 5f, 5f);
			image.Stretch = Stretch.Fill;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_37";
			textBlock.Margin = new Thickness(6f, 0f, 0f, 4f);
			textBlock.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock.VerticalAlignment = VerticalAlignment.Bottom;
			textBlock.TextAlignment = TextAlignment.Left;
			textBlock.FontFamily = new FontFamily("InventorySmall");
			textBlock.FontSize = 10f;
			textBlock.FontStyle = FontStyle.Regular;
			textBlock.SetBinding(binding: new Binding("AmountFormatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock2 = new TextBlock();
			obj.Children.Add(textBlock2);
			textBlock2.Name = "e_38";
			textBlock2.Margin = new Thickness(6f, 4f, 0f, 0f);
			textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock2.VerticalAlignment = VerticalAlignment.Top;
			textBlock2.TextAlignment = TextAlignment.Left;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("IconSymbol")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock3 = new TextBlock();
			obj.Children.Add(textBlock3);
			textBlock3.Name = "e_39";
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock3, 1);
			textBlock3.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}
	}
}
