using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.PlayerTradeView_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class PlayerTradeView : UIRoot
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

		private TextBlock e_8;

		private ListBox e_9;

		private ListBox e_14;

		private StackPanel e_21;

		private NumericTextBox e_22;

		private StackPanel e_23;

		private NumericTextBox e_24;

		private Border e_25;

		private Grid e_26;

		private ListBox e_27;

		private StackPanel e_34;

		private TextBlock e_35;

		private TextBlock e_36;

		private StackPanel e_37;

		private TextBlock e_38;

		private TextBlock e_39;

		private Image e_40;

		private StackPanel e_41;

		private TextBlock e_42;

		private TextBlock e_43;

		private Grid e_44;

		private StackPanel e_45;

		private TextBlock e_46;

		private TextBlock e_47;

		private Image e_48;

		private StackPanel e_49;

		private TextBlock e_50;

		private TextBlock e_51;

		private Border e_52;

		private StackPanel e_53;

		private Button e_54;

		private Button e_55;

		private Button e_56;

		public PlayerTradeView()
		{
			Initialize();
		}

		public PlayerTradeView(int width, int height)
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
			columnDefinition2.Width = new GridLength(4f, GridUnitType.Star);
			e_1.ColumnDefinitions.Add(columnDefinition2);
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition3.Width = new GridLength(2f, GridUnitType.Star);
			e_1.ColumnDefinitions.Add(columnDefinition3);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(1f, GridUnitType.Auto);
			e_1.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(2f, GridUnitType.Star);
			e_1.ColumnDefinitions.Add(columnDefinition5);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(75f, GridUnitType.Pixel);
			e_1.ColumnDefinitions.Add(columnDefinition6);
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
			Grid.SetColumn(e_2, 5);
			Binding binding3 = new Binding("ExitCommand");
			binding3.UseGeneratedBindings = true;
			e_2.SetBinding(Button.CommandProperty, binding3);
			e_3 = new StackPanel();
			e_1.Children.Add(e_3);
			e_3.Name = "e_3";
			Grid.SetColumn(e_3, 1);
			Grid.SetRow(e_3, 1);
			Grid.SetColumnSpan(e_3, 4);
			e_4 = new TextBlock();
			e_3.Children.Add(e_4);
			e_4.Name = "e_4";
			e_4.HorizontalAlignment = HorizontalAlignment.Center;
			e_4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionPlayerTrade");
			e_5 = new Border();
			e_3.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.Height = 2f;
			e_5.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_5.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_6 = new TextBlock();
			e_1.Children.Add(e_6);
			e_6.Name = "e_6";
			e_6.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_6.HorizontalAlignment = HorizontalAlignment.Left;
			e_6.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 2);
			e_6.SetResourceReference(TextBlock.TextProperty, "TradeScreenYoursInventory");
			e_7 = new TextBlock();
			e_1.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_7.HorizontalAlignment = HorizontalAlignment.Left;
			e_7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(e_7, 2);
			Grid.SetRow(e_7, 2);
			e_7.SetResourceReference(TextBlock.TextProperty, "TradeScreenYoursOffer");
			e_8 = new TextBlock();
			e_1.Children.Add(e_8);
			e_8.Name = "e_8";
			e_8.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_8.HorizontalAlignment = HorizontalAlignment.Left;
			e_8.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(e_8, 4);
			Grid.SetRow(e_8, 2);
			e_8.SetResourceReference(TextBlock.TextProperty, "TradeScreenOtherOffer");
			e_9 = new ListBox();
			e_1.Children.Add(e_9);
			e_9.Name = "e_9";
			e_9.Margin = new Thickness(0f, 10f, 10f, 10f);
			Style style = new Style(typeof(ListBox));
			Setter item3 = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item3);
			Func<UIElement, UIElement> createMethod = e_9_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod);
			Setter item4 = new Setter(Control.TemplateProperty, value);
			style.Setters.Add(item4);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsFocusedProperty;
			trigger.Value = true;
			Setter item5 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item5);
			style.Triggers.Add(trigger);
			e_9.Style = style;
			e_9.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_9.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_9.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
			e_9.TabIndex = 0;
			Grid.SetColumn(e_9, 1);
			Grid.SetRow(e_9, 3);
			Grid.SetRowSpan(e_9, 2);
			DragDrop.SetIsDragSource(e_9, value: true);
			DragDrop.SetIsDropTarget(e_9, value: true);
			Binding binding4 = new Binding("InventoryItems");
			binding4.UseGeneratedBindings = true;
			e_9.SetBinding(ItemsControl.ItemsSourceProperty, binding4);
			InitializeElemente_9Resources(e_9);
			e_14 = new ListBox();
			e_1.Children.Add(e_14);
			e_14.Name = "e_14";
			e_14.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_14.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_14.TabIndex = 1;
			Func<UIElement, UIElement> createMethod2 = e_14_dtMethod;
			e_14.ItemTemplate = new DataTemplate(typeof(MyInventoryItemModel), createMethod2);
			Grid.SetColumn(e_14, 2);
			Grid.SetRow(e_14, 3);
			DragDrop.SetIsDragSource(e_14, value: true);
			DragDrop.SetIsDropTarget(e_14, value: true);
			Binding binding5 = new Binding("LocalPlayerOfferItems");
			binding5.UseGeneratedBindings = true;
			e_14.SetBinding(ItemsControl.ItemsSourceProperty, binding5);
			e_21 = new StackPanel();
			e_1.Children.Add(e_21);
			e_21.Name = "e_21";
			Grid.SetColumn(e_21, 2);
			Grid.SetRow(e_21, 4);
			e_22 = new NumericTextBox();
			e_21.Children.Add(e_22);
			e_22.Name = "e_22";
			e_22.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_22.TabIndex = 2;
			e_22.MaxLength = 9;
			e_22.Minimum = 0f;
			Binding binding6 = new Binding("LocalPlayerOfferCurrencyMaximum");
			binding6.UseGeneratedBindings = true;
			e_22.SetBinding(NumericTextBox.MaximumProperty, binding6);
			Binding binding7 = new Binding("LocalPlayerOfferCurrency");
			binding7.UseGeneratedBindings = true;
			e_22.SetBinding(NumericTextBox.ValueProperty, binding7);
			e_23 = new StackPanel();
			e_1.Children.Add(e_23);
			e_23.Name = "e_23";
			Grid.SetColumn(e_23, 2);
			Grid.SetRow(e_23, 5);
			Binding binding8 = new Binding("IsPcuVisible");
			binding8.UseGeneratedBindings = true;
			e_23.SetBinding(UIElement.VisibilityProperty, binding8);
			e_24 = new NumericTextBox();
			e_23.Children.Add(e_24);
			e_24.Name = "e_24";
			e_24.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_24.TabIndex = 3;
			e_24.MaxLength = 5;
			e_24.Minimum = 0f;
			e_24.Maximum = 99999f;
			Binding binding9 = new Binding("LocalPlayerOfferPcu");
			binding9.UseGeneratedBindings = true;
			e_24.SetBinding(NumericTextBox.ValueProperty, binding9);
			e_25 = new Border();
			e_1.Children.Add(e_25);
			e_25.Name = "e_25";
			e_25.Width = 2f;
			e_25.Margin = new Thickness(5f, 10f, 5f, 10f);
			e_25.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_25, 3);
			Grid.SetRow(e_25, 2);
			Grid.SetRowSpan(e_25, 4);
			e_26 = new Grid();
			e_1.Children.Add(e_26);
			e_26.Name = "e_26";
			RowDefinition item6 = new RowDefinition();
			e_26.RowDefinitions.Add(item6);
			RowDefinition rowDefinition10 = new RowDefinition();
			rowDefinition10.Height = new GridLength(1f, GridUnitType.Auto);
			e_26.RowDefinitions.Add(rowDefinition10);
			Grid.SetColumn(e_26, 4);
			Grid.SetRow(e_26, 3);
			e_27 = new ListBox();
			e_26.Children.Add(e_27);
			e_27.Name = "e_27";
			e_27.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_27.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_27.TabIndex = 4;
			Func<UIElement, UIElement> createMethod3 = e_27_dtMethod;
			e_27.ItemTemplate = new DataTemplate(typeof(MyInventoryItemModel), createMethod3);
			Grid.SetRow(e_27, 0);
			Binding binding10 = new Binding("OtherPlayerOfferItems");
			binding10.UseGeneratedBindings = true;
			e_27.SetBinding(ItemsControl.ItemsSourceProperty, binding10);
			e_34 = new StackPanel();
			e_26.Children.Add(e_34);
			e_34.Name = "e_34";
			e_34.VerticalAlignment = VerticalAlignment.Bottom;
			e_34.Orientation = Orientation.Horizontal;
			Grid.SetRow(e_34, 1);
			e_35 = new TextBlock();
			e_34.Children.Add(e_35);
			e_35.Name = "e_35";
			e_35.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_35.VerticalAlignment = VerticalAlignment.Center;
			e_35.SetResourceReference(TextBlock.TextProperty, "TradeScreenOfferState");
			e_36 = new TextBlock();
			e_34.Children.Add(e_36);
			e_36.Name = "e_36";
			e_36.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_36.VerticalAlignment = VerticalAlignment.Center;
			e_36.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding11 = new Binding("OtherPlayerAcceptState");
			binding11.UseGeneratedBindings = true;
			e_36.SetBinding(TextBlock.TextProperty, binding11);
			e_37 = new StackPanel();
			e_1.Children.Add(e_37);
			e_37.Name = "e_37";
			e_37.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_37, 4);
			Grid.SetRow(e_37, 4);
			e_38 = new TextBlock();
			e_37.Children.Add(e_38);
			e_38.Name = "e_38";
			e_38.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_38.VerticalAlignment = VerticalAlignment.Center;
			e_38.SetResourceReference(TextBlock.TextProperty, "TradeScreenMoney");
			e_39 = new TextBlock();
			e_37.Children.Add(e_39);
			e_39.Name = "e_39";
			e_39.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_39.VerticalAlignment = VerticalAlignment.Center;
			e_39.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding12 = new Binding("OtherPlayerOfferCurrency");
			binding12.UseGeneratedBindings = true;
			e_39.SetBinding(TextBlock.TextProperty, binding12);
			e_40 = new Image();
			e_37.Children.Add(e_40);
			e_40.Name = "e_40";
			e_40.Height = 20f;
			e_40.Margin = new Thickness(3f, 2f, 2f, 0f);
			e_40.VerticalAlignment = VerticalAlignment.Center;
			e_40.Stretch = Stretch.Uniform;
			Binding binding13 = new Binding("CurrencyIcon");
			binding13.UseGeneratedBindings = true;
			e_40.SetBinding(Image.SourceProperty, binding13);
			e_41 = new StackPanel();
			e_1.Children.Add(e_41);
			e_41.Name = "e_41";
			e_41.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_41, 4);
			Grid.SetRow(e_41, 5);
			Binding binding14 = new Binding("IsPcuVisible");
			binding14.UseGeneratedBindings = true;
			e_41.SetBinding(UIElement.VisibilityProperty, binding14);
			e_42 = new TextBlock();
			e_41.Children.Add(e_42);
			e_42.Name = "e_42";
			e_42.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_42.VerticalAlignment = VerticalAlignment.Center;
			e_42.SetResourceReference(TextBlock.TextProperty, "TradeScreenPcu");
			e_43 = new TextBlock();
			e_41.Children.Add(e_43);
			e_43.Name = "e_43";
			e_43.Margin = new Thickness(5f, 5f, 10f, 5f);
			e_43.VerticalAlignment = VerticalAlignment.Center;
			e_43.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding15 = new Binding("OtherPlayerOfferPcu");
			binding15.UseGeneratedBindings = true;
			e_43.SetBinding(TextBlock.TextProperty, binding15);
			e_44 = new Grid();
			e_1.Children.Add(e_44);
			e_44.Name = "e_44";
			ColumnDefinition item7 = new ColumnDefinition();
			e_44.ColumnDefinitions.Add(item7);
			ColumnDefinition item8 = new ColumnDefinition();
			e_44.ColumnDefinitions.Add(item8);
			Grid.SetColumn(e_44, 1);
			Grid.SetRow(e_44, 5);
			e_45 = new StackPanel();
			e_44.Children.Add(e_45);
			e_45.Name = "e_45";
			e_45.HorizontalAlignment = HorizontalAlignment.Left;
			e_45.Orientation = Orientation.Horizontal;
			e_46 = new TextBlock();
			e_45.Children.Add(e_46);
			e_46.Name = "e_46";
			e_46.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_46.VerticalAlignment = VerticalAlignment.Center;
			e_46.SetResourceReference(TextBlock.TextProperty, "TradeScreenMoney");
			e_47 = new TextBlock();
			e_45.Children.Add(e_47);
			e_47.Name = "e_47";
			e_47.Margin = new Thickness(5f, 5f, 0f, 5f);
			e_47.VerticalAlignment = VerticalAlignment.Center;
			e_47.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding16 = new Binding("LocalPlayerCurrency");
			binding16.UseGeneratedBindings = true;
			e_47.SetBinding(TextBlock.TextProperty, binding16);
			e_48 = new Image();
			e_45.Children.Add(e_48);
			e_48.Name = "e_48";
			e_48.Height = 20f;
			e_48.Margin = new Thickness(3f, 2f, 2f, 0f);
			e_48.VerticalAlignment = VerticalAlignment.Center;
			e_48.Stretch = Stretch.Uniform;
			Binding binding17 = new Binding("CurrencyIcon");
			binding17.UseGeneratedBindings = true;
			e_48.SetBinding(Image.SourceProperty, binding17);
			e_49 = new StackPanel();
			e_44.Children.Add(e_49);
			e_49.Name = "e_49";
			e_49.HorizontalAlignment = HorizontalAlignment.Left;
			e_49.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_49, 1);
			Binding binding18 = new Binding("IsPcuVisible");
			binding18.UseGeneratedBindings = true;
			e_49.SetBinding(UIElement.VisibilityProperty, binding18);
			e_50 = new TextBlock();
			e_49.Children.Add(e_50);
			e_50.Name = "e_50";
			e_50.Margin = new Thickness(0f, 5f, 5f, 5f);
			e_50.VerticalAlignment = VerticalAlignment.Center;
			e_50.SetResourceReference(TextBlock.TextProperty, "TradeScreenPcu");
			e_51 = new TextBlock();
			e_49.Children.Add(e_51);
			e_51.Name = "e_51";
			e_51.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_51.VerticalAlignment = VerticalAlignment.Center;
			e_51.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Binding binding19 = new Binding("LocalPlayerPcu");
			binding19.UseGeneratedBindings = true;
			e_51.SetBinding(TextBlock.TextProperty, binding19);
			e_52 = new Border();
			e_1.Children.Add(e_52);
			e_52.Name = "e_52";
			e_52.Height = 2f;
			e_52.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_52.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_52, 1);
			Grid.SetRow(e_52, 6);
			Grid.SetColumnSpan(e_52, 4);
			e_53 = new StackPanel();
			e_1.Children.Add(e_53);
			e_53.Name = "e_53";
			e_53.Margin = new Thickness(0f, 0f, 0f, 30f);
			e_53.HorizontalAlignment = HorizontalAlignment.Right;
			e_53.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_53, 1);
			Grid.SetRow(e_53, 7);
			Grid.SetColumnSpan(e_53, 4);
			e_54 = new Button();
			e_53.Children.Add(e_54);
			e_54.Name = "e_54";
			e_54.Width = 200f;
			e_54.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_54.TabIndex = 5;
			Binding binding20 = new Binding("IsSubmitOfferEnabled");
			binding20.UseGeneratedBindings = true;
			e_54.SetBinding(UIElement.IsEnabledProperty, binding20);
			Binding binding21 = new Binding("SubmitOfferLabel");
			binding21.UseGeneratedBindings = true;
			e_54.SetBinding(ContentControl.ContentProperty, binding21);
			Binding binding22 = new Binding("SubmitOfferCommand");
			binding22.UseGeneratedBindings = true;
			e_54.SetBinding(Button.CommandProperty, binding22);
			e_55 = new Button();
			e_53.Children.Add(e_55);
			e_55.Name = "e_55";
			e_55.Width = 150f;
			e_55.Margin = new Thickness(10f, 10f, 0f, 10f);
			e_55.TabIndex = 6;
			Binding binding23 = new Binding("IsAcceptVisible");
			binding23.UseGeneratedBindings = true;
			e_55.SetBinding(UIElement.VisibilityProperty, binding23);
			Binding binding24 = new Binding("IsAcceptEnabled");
			binding24.UseGeneratedBindings = true;
			e_55.SetBinding(UIElement.IsEnabledProperty, binding24);
			Binding binding25 = new Binding("AcceptCommand");
			binding25.UseGeneratedBindings = true;
			e_55.SetBinding(Button.CommandProperty, binding25);
			e_55.SetResourceReference(ContentControl.ContentProperty, "TradeScreenAccept");
			e_56 = new Button();
			e_53.Children.Add(e_56);
			e_56.Name = "e_56";
			e_56.Width = 150f;
			e_56.Margin = new Thickness(10f, 10f, 0f, 10f);
			e_56.TabIndex = 7;
			Binding binding26 = new Binding("IsCancelVisible");
			binding26.UseGeneratedBindings = true;
			e_56.SetBinding(UIElement.VisibilityProperty, binding26);
			Binding binding27 = new Binding("CancelCommand");
			binding27.UseGeneratedBindings = true;
			e_56.SetBinding(Button.CommandProperty, binding27);
			e_56.SetResourceReference(ContentControl.ContentProperty, "TradeScreenCancel");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "MaxWidth", typeof(MyPlayerTradeViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "BackgroundOverlay", typeof(MyPlayerTradeViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "ExitCommand", typeof(MyPlayerTradeViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "InventoryItems", typeof(MyPlayerTradeViewModel_InventoryItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Icon", typeof(MyInventoryItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "AmountFormatted", typeof(MyInventoryItemModel_AmountFormatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "IconSymbol", typeof(MyInventoryItemModel_IconSymbol_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Name", typeof(MyInventoryItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferItems", typeof(MyPlayerTradeViewModel_LocalPlayerOfferItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferCurrencyMaximum", typeof(MyPlayerTradeViewModel_LocalPlayerOfferCurrencyMaximum_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferCurrency", typeof(MyPlayerTradeViewModel_LocalPlayerOfferCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsPcuVisible", typeof(MyPlayerTradeViewModel_IsPcuVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerOfferPcu", typeof(MyPlayerTradeViewModel_LocalPlayerOfferPcu_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerOfferItems", typeof(MyPlayerTradeViewModel_OtherPlayerOfferItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerAcceptState", typeof(MyPlayerTradeViewModel_OtherPlayerAcceptState_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerOfferCurrency", typeof(MyPlayerTradeViewModel_OtherPlayerOfferCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "CurrencyIcon", typeof(MyPlayerTradeViewModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "OtherPlayerOfferPcu", typeof(MyPlayerTradeViewModel_OtherPlayerOfferPcu_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerCurrency", typeof(MyPlayerTradeViewModel_LocalPlayerCurrency_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "LocalPlayerPcu", typeof(MyPlayerTradeViewModel_LocalPlayerPcu_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsSubmitOfferEnabled", typeof(MyPlayerTradeViewModel_IsSubmitOfferEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "SubmitOfferLabel", typeof(MyPlayerTradeViewModel_SubmitOfferLabel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "SubmitOfferCommand", typeof(MyPlayerTradeViewModel_SubmitOfferCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsAcceptVisible", typeof(MyPlayerTradeViewModel_IsAcceptVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsAcceptEnabled", typeof(MyPlayerTradeViewModel_IsAcceptEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "AcceptCommand", typeof(MyPlayerTradeViewModel_AcceptCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "IsCancelVisible", typeof(MyPlayerTradeViewModel_IsCancelVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyPlayerTradeViewModel), "CancelCommand", typeof(MyPlayerTradeViewModel_CancelCommand_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplates.Instance);
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
			WrapPanel wrapPanel2 = (WrapPanel)(scrollViewer.Content = new WrapPanel());
			wrapPanel2.Name = "e_11";
			wrapPanel2.Margin = new Thickness(4f, 4f, 4f, 4f);
			wrapPanel2.IsItemsHost = true;
			wrapPanel2.ItemHeight = 64f;
			wrapPanel2.ItemWidth = 64f;
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

		private static UIElement e_14_dtMethod(UIElement parent)
		{
			Grid grid = new Grid();
			grid.Parent = parent;
			grid.Name = "e_15";
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
			border.Name = "e_16";
			border.Height = 64f;
			border.Width = 64f;
			border.SetResourceReference(Control.BackgroundProperty, "GridItem");
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_17";
			image.Margin = new Thickness(5f, 5f, 5f, 5f);
			image.Stretch = Stretch.Fill;
			Binding binding7 = new Binding("Icon");
			binding7.UseGeneratedBindings = true;
			image.SetBinding(Image.SourceProperty, binding7);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_18";
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
			textBlock2.Name = "e_19";
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
			textBlock3.Name = "e_20";
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock3, 1);
			Binding binding10 = new Binding("Name");
			binding10.UseGeneratedBindings = true;
			textBlock3.SetBinding(TextBlock.TextProperty, binding10);
			return grid;
		}

		private static UIElement e_27_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_28",
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
			border.Name = "e_29";
			border.Height = 64f;
			border.Width = 64f;
			border.SetResourceReference(Control.BackgroundProperty, "GridItem");
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_30";
			image.Margin = new Thickness(5f, 5f, 5f, 5f);
			image.Stretch = Stretch.Fill;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_31";
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
			textBlock2.Name = "e_32";
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
			textBlock3.Name = "e_33";
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
