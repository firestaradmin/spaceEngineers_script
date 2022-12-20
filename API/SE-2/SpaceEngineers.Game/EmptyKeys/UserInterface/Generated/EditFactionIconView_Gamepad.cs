using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.EditFactionIconView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class EditFactionIconView_Gamepad : UIRoot
	{
		private Grid rootGrid;

		private Grid e_0;

		private Image e_1;

		private Grid e_2;

		private ImageButton e_3;

		private StackPanel e_4;

		private TextBlock e_5;

		private Border e_6;

		private Grid e_7;

		private Grid e_8;

		private Border e_9;

		private Image FactionIcon;

		private ListBox ListBoxIcons;

		private Grid e_14;

		private StackPanel e_15;

		private TextBlock e_16;

		private Slider e_17;

		private StackPanel e_18;

		private TextBlock e_19;

		private Slider e_20;

		private StackPanel e_21;

		private TextBlock e_22;

		private Slider e_23;

		private Grid e_24;

		private StackPanel e_25;

		private TextBlock e_26;

		private Slider e_27;

		private StackPanel e_28;

		private TextBlock e_29;

		private Slider e_30;

		private StackPanel e_31;

		private TextBlock e_32;

		private Slider e_33;

		private Border e_34;

		private Grid SelectionHelp;

		private TextBlock e_35;

		private TextBlock e_36;

		private Grid ColorHelp;

		private TextBlock e_37;

		private TextBlock e_38;

		private TextBlock e_39;

		public EditFactionIconView_Gamepad()
		{
			Initialize();
		}

		public EditFactionIconView_Gamepad(int width, int height)
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
			Binding binding = new Binding("MaxWidth");
			binding.UseGeneratedBindings = true;
			rootGrid.SetBinding(UIElement.MaxWidthProperty, binding);
			e_0 = new Grid();
			rootGrid.Children.Add(e_0);
			e_0.Name = "e_0";
			RowDefinition rowDefinition = new RowDefinition();
			rowDefinition.Height = new GridLength(3f, GridUnitType.Star);
			e_0.RowDefinitions.Add(rowDefinition);
			RowDefinition rowDefinition2 = new RowDefinition();
			rowDefinition2.Height = new GridLength(11f, GridUnitType.Star);
			e_0.RowDefinitions.Add(rowDefinition2);
			RowDefinition rowDefinition3 = new RowDefinition();
			rowDefinition3.Height = new GridLength(3f, GridUnitType.Star);
			e_0.RowDefinitions.Add(rowDefinition3);
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.Width = new GridLength(6f, GridUnitType.Star);
			e_0.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition columnDefinition2 = new ColumnDefinition();
			columnDefinition2.Width = new GridLength(16f, GridUnitType.Star);
			e_0.ColumnDefinitions.Add(columnDefinition2);
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition3.Width = new GridLength(6f, GridUnitType.Star);
			e_0.ColumnDefinitions.Add(columnDefinition3);
			e_1 = new Image();
			e_0.Children.Add(e_1);
			e_1.Name = "e_1";
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			e_1.Source = bitmapImage;
			e_1.Stretch = Stretch.Fill;
			Grid.SetColumn(e_1, 1);
			Grid.SetRow(e_1, 1);
			Binding binding2 = new Binding("BackgroundOverlay");
			binding2.UseGeneratedBindings = true;
			e_1.SetBinding(ImageBrush.ColorOverlayProperty, binding2);
			e_2 = new Grid();
			e_0.Children.Add(e_2);
			e_2.Name = "e_2";
			RowDefinition rowDefinition4 = new RowDefinition();
			rowDefinition4.Height = new GridLength(1f, GridUnitType.Auto);
			e_2.RowDefinitions.Add(rowDefinition4);
			RowDefinition rowDefinition5 = new RowDefinition();
			rowDefinition5.Height = new GridLength(1f, GridUnitType.Auto);
			e_2.RowDefinitions.Add(rowDefinition5);
			RowDefinition item = new RowDefinition();
			e_2.RowDefinitions.Add(item);
			RowDefinition rowDefinition6 = new RowDefinition();
			rowDefinition6.Height = new GridLength(1f, GridUnitType.Auto);
			e_2.RowDefinitions.Add(rowDefinition6);
			RowDefinition rowDefinition7 = new RowDefinition();
			rowDefinition7.Height = new GridLength(64f, GridUnitType.Pixel);
			e_2.RowDefinitions.Add(rowDefinition7);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(75f, GridUnitType.Pixel);
			e_2.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(1f, GridUnitType.Star);
			e_2.ColumnDefinitions.Add(columnDefinition5);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(75f, GridUnitType.Pixel);
			e_2.ColumnDefinitions.Add(columnDefinition6);
			Grid.SetColumn(e_2, 1);
			Grid.SetRow(e_2, 1);
			e_3 = new ImageButton();
			e_2.Children.Add(e_3);
			e_3.Name = "e_3";
			e_3.Height = 24f;
			e_3.Width = 24f;
			e_3.Margin = new Thickness(16f, 16f, 16f, 16f);
			e_3.HorizontalAlignment = HorizontalAlignment.Right;
			e_3.VerticalAlignment = VerticalAlignment.Center;
			e_3.IsTabStop = false;
			e_3.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage2 = new BitmapImage();
			bitmapImage2.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			e_3.ImageNormal = bitmapImage2;
			BitmapImage bitmapImage3 = new BitmapImage();
			bitmapImage3.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_3.ImageHover = bitmapImage3;
			BitmapImage bitmapImage4 = new BitmapImage();
			bitmapImage4.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_3.ImagePressed = bitmapImage4;
			Grid.SetColumn(e_3, 2);
			Grid.SetRow(e_3, 0);
			Binding binding3 = new Binding("ExitCommand");
			binding3.UseGeneratedBindings = true;
			e_3.SetBinding(Button.CommandProperty, binding3);
			e_4 = new StackPanel();
			e_2.Children.Add(e_4);
			e_4.Name = "e_4";
			Grid.SetColumn(e_4, 1);
			Grid.SetRow(e_4, 1);
			Grid.SetColumnSpan(e_4, 1);
			e_5 = new TextBlock();
			e_4.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.HorizontalAlignment = HorizontalAlignment.Center;
			e_5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_5.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionEditFaction");
			e_6 = new Border();
			e_4.Children.Add(e_6);
			e_6.Name = "e_6";
			e_6.Height = 2f;
			e_6.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_6.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_7 = new Grid();
			e_2.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_7.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_7.VerticalAlignment = VerticalAlignment.Stretch;
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.AButton);
			Binding binding4 = new Binding("OkCommand");
			binding4.UseGeneratedBindings = true;
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding4);
			e_7.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = e_7;
			RowDefinition item2 = new RowDefinition();
			e_7.RowDefinitions.Add(item2);
			RowDefinition rowDefinition8 = new RowDefinition();
			rowDefinition8.Height = new GridLength(1f, GridUnitType.Auto);
			e_7.RowDefinitions.Add(rowDefinition8);
			RowDefinition rowDefinition9 = new RowDefinition();
			rowDefinition9.Height = new GridLength(1f, GridUnitType.Auto);
			e_7.RowDefinitions.Add(rowDefinition9);
			Grid.SetColumn(e_7, 1);
			Grid.SetRow(e_7, 2);
			e_8 = new Grid();
			e_7.Children.Add(e_8);
			e_8.Name = "e_8";
			RowDefinition rowDefinition10 = new RowDefinition();
			rowDefinition10.Height = new GridLength(0.1f, GridUnitType.Star);
			e_8.RowDefinitions.Add(rowDefinition10);
			RowDefinition rowDefinition11 = new RowDefinition();
			rowDefinition11.Height = new GridLength(1f, GridUnitType.Auto);
			e_8.RowDefinitions.Add(rowDefinition11);
			RowDefinition item3 = new RowDefinition();
			e_8.RowDefinitions.Add(item3);
			RowDefinition rowDefinition12 = new RowDefinition();
			rowDefinition12.Height = new GridLength(0.2f, GridUnitType.Star);
			e_8.RowDefinitions.Add(rowDefinition12);
			ColumnDefinition columnDefinition7 = new ColumnDefinition();
			columnDefinition7.Width = new GridLength(1f, GridUnitType.Auto);
			e_8.ColumnDefinitions.Add(columnDefinition7);
			ColumnDefinition item4 = new ColumnDefinition();
			e_8.ColumnDefinitions.Add(item4);
			e_9 = new Border();
			e_8.Children.Add(e_9);
			e_9.Name = "e_9";
			e_9.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_9.HorizontalAlignment = HorizontalAlignment.Left;
			e_9.VerticalAlignment = VerticalAlignment.Top;
			e_9.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_9.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(e_9, 0);
			Grid.SetRow(e_9, 1);
			Binding binding5 = new Binding("FactionColor");
			binding5.UseGeneratedBindings = true;
			e_9.SetBinding(Control.BackgroundProperty, binding5);
			FactionIcon = new Image();
			e_9.Child = FactionIcon;
			FactionIcon.Name = "FactionIcon";
			FactionIcon.Height = 128f;
			FactionIcon.Margin = new Thickness(0f, 0f, 0f, 0f);
			FactionIcon.Stretch = Stretch.Uniform;
			Binding binding6 = new Binding("FactionIconBitmap");
			binding6.UseGeneratedBindings = true;
			FactionIcon.SetBinding(Image.SourceProperty, binding6);
			Binding binding7 = new Binding("IconColorInternal");
			binding7.UseGeneratedBindings = true;
			FactionIcon.SetBinding(ImageBrush.ColorOverlayProperty, binding7);
			ListBoxIcons = new ListBox();
			e_8.Children.Add(ListBoxIcons);
			ListBoxIcons.Name = "ListBoxIcons";
			ListBoxIcons.Margin = new Thickness(20f, 0f, 0f, 0f);
			Style style = new Style(typeof(ListBox));
			Setter item5 = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item5);
			Func<UIElement, UIElement> createMethod = ListBoxIcons_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod);
			Setter item6 = new Setter(Control.TemplateProperty, value);
			style.Setters.Add(item6);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsFocusedProperty;
			trigger.Value = true;
			Setter item7 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item7);
			style.Triggers.Add(trigger);
			ListBoxIcons.Style = style;
			ListBoxIcons.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			ListBoxIcons.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			ListBoxIcons.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			ListBoxIcons.TabIndex = 0;
			Grid.SetColumn(ListBoxIcons, 1);
			Grid.SetRow(ListBoxIcons, 1);
			Grid.SetRowSpan(ListBoxIcons, 2);
			GamepadHelp.SetTargetName(ListBoxIcons, "SelectionHelp");
			GamepadHelp.SetTabIndexDown(ListBoxIcons, 1);
			Binding binding8 = new Binding("FactionIcons");
			binding8.UseGeneratedBindings = true;
			ListBoxIcons.SetBinding(ItemsControl.ItemsSourceProperty, binding8);
			Binding binding9 = new Binding("SelectedIcon");
			binding9.UseGeneratedBindings = true;
			ListBoxIcons.SetBinding(Selector.SelectedItemProperty, binding9);
			InitializeElementListBoxIconsResources(ListBoxIcons);
			e_14 = new Grid();
			e_7.Children.Add(e_14);
			e_14.Name = "e_14";
			e_14.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_14.VerticalAlignment = VerticalAlignment.Stretch;
			ColumnDefinition columnDefinition8 = new ColumnDefinition();
			columnDefinition8.Width = new GridLength(1f, GridUnitType.Star);
			e_14.ColumnDefinitions.Add(columnDefinition8);
			ColumnDefinition columnDefinition9 = new ColumnDefinition();
			columnDefinition9.Width = new GridLength(1f, GridUnitType.Star);
			e_14.ColumnDefinitions.Add(columnDefinition9);
			ColumnDefinition columnDefinition10 = new ColumnDefinition();
			columnDefinition10.Width = new GridLength(1f, GridUnitType.Star);
			e_14.ColumnDefinitions.Add(columnDefinition10);
			Grid.SetRow(e_14, 1);
			e_15 = new StackPanel();
			e_14.Children.Add(e_15);
			e_15.Name = "e_15";
			e_15.Margin = new Thickness(0f, 0f, 10f, 0f);
			e_15.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_15.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(e_15, 0);
			e_16 = new TextBlock();
			e_15.Children.Add(e_16);
			e_16.Name = "e_16";
			e_16.Margin = new Thickness(2f, 0f, 0f, 0f);
			e_16.SetResourceReference(TextBlock.TextProperty, "EditFaction_HueSliderText");
			e_17 = new Slider();
			e_15.Children.Add(e_17);
			e_17.Name = "e_17";
			e_17.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_17.VerticalAlignment = VerticalAlignment.Stretch;
			Style style2 = new Style(typeof(Slider));
			Setter item8 = new Setter(UIElement.SnapsToDevicePixelsProperty, false);
			style2.Setters.Add(item8);
			Setter item9 = new Setter(Control.TemplateProperty, new ResourceReferenceExpression("HorizontalSliderHuePicker"));
			style2.Setters.Add(item9);
			e_17.Style = style2;
			e_17.TabIndex = 1;
			e_17.TickFrequency = 0.0027f;
			GamepadHelp.SetTargetName(e_17, "ColorHelp");
			GamepadHelp.SetTabIndexRight(e_17, 2);
			GamepadHelp.SetTabIndexUp(e_17, 0);
			GamepadHelp.SetTabIndexDown(e_17, 4);
			Binding binding10 = new Binding("Hue");
			binding10.UseGeneratedBindings = true;
			e_17.SetBinding(RangeBase.ValueProperty, binding10);
			e_18 = new StackPanel();
			e_14.Children.Add(e_18);
			e_18.Name = "e_18";
			e_18.Margin = new Thickness(0f, 0f, 10f, 0f);
			e_18.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_18.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(e_18, 1);
			e_19 = new TextBlock();
			e_18.Children.Add(e_19);
			e_19.Name = "e_19";
			e_19.Margin = new Thickness(2f, 0f, 0f, 0f);
			e_19.SetResourceReference(TextBlock.TextProperty, "EditFaction_SaturationSliderText");
			e_20 = new Slider();
			e_18.Children.Add(e_20);
			e_20.Name = "e_20";
			e_20.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_20.VerticalAlignment = VerticalAlignment.Stretch;
			e_20.TabIndex = 2;
			e_20.TickFrequency = 0.01f;
			GamepadHelp.SetTargetName(e_20, "ColorHelp");
			GamepadHelp.SetTabIndexLeft(e_20, 1);
			GamepadHelp.SetTabIndexRight(e_20, 3);
			GamepadHelp.SetTabIndexUp(e_20, 0);
			GamepadHelp.SetTabIndexDown(e_20, 5);
			Binding binding11 = new Binding("Saturation");
			binding11.UseGeneratedBindings = true;
			e_20.SetBinding(RangeBase.ValueProperty, binding11);
			e_21 = new StackPanel();
			e_14.Children.Add(e_21);
			e_21.Name = "e_21";
			e_21.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_21.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(e_21, 2);
			e_22 = new TextBlock();
			e_21.Children.Add(e_22);
			e_22.Name = "e_22";
			e_22.Margin = new Thickness(2f, 0f, 0f, 0f);
			e_22.SetResourceReference(TextBlock.TextProperty, "EditFaction_ValueSliderText");
			e_23 = new Slider();
			e_21.Children.Add(e_23);
			e_23.Name = "e_23";
			e_23.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_23.VerticalAlignment = VerticalAlignment.Stretch;
			e_23.TabIndex = 3;
			e_23.TickFrequency = 0.01f;
			GamepadHelp.SetTargetName(e_23, "ColorHelp");
			GamepadHelp.SetTabIndexLeft(e_23, 2);
			GamepadHelp.SetTabIndexUp(e_23, 0);
			GamepadHelp.SetTabIndexDown(e_23, 6);
			Binding binding12 = new Binding("ColorValue");
			binding12.UseGeneratedBindings = true;
			e_23.SetBinding(RangeBase.ValueProperty, binding12);
			e_24 = new Grid();
			e_7.Children.Add(e_24);
			e_24.Name = "e_24";
			e_24.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_24.VerticalAlignment = VerticalAlignment.Stretch;
			ColumnDefinition columnDefinition11 = new ColumnDefinition();
			columnDefinition11.Width = new GridLength(1f, GridUnitType.Star);
			e_24.ColumnDefinitions.Add(columnDefinition11);
			ColumnDefinition columnDefinition12 = new ColumnDefinition();
			columnDefinition12.Width = new GridLength(1f, GridUnitType.Star);
			e_24.ColumnDefinitions.Add(columnDefinition12);
			ColumnDefinition columnDefinition13 = new ColumnDefinition();
			columnDefinition13.Width = new GridLength(1f, GridUnitType.Star);
			e_24.ColumnDefinitions.Add(columnDefinition13);
			Grid.SetRow(e_24, 2);
			e_25 = new StackPanel();
			e_24.Children.Add(e_25);
			e_25.Name = "e_25";
			e_25.Margin = new Thickness(0f, 0f, 10f, 0f);
			e_25.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_25.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(e_25, 0);
			e_26 = new TextBlock();
			e_25.Children.Add(e_26);
			e_26.Name = "e_26";
			e_26.Margin = new Thickness(2f, 0f, 0f, 0f);
			e_26.SetResourceReference(TextBlock.TextProperty, "EditFaction_HueIconSliderText");
			e_27 = new Slider();
			e_25.Children.Add(e_27);
			e_27.Name = "e_27";
			e_27.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_27.VerticalAlignment = VerticalAlignment.Stretch;
			Style style3 = new Style(typeof(Slider));
			Setter item10 = new Setter(UIElement.SnapsToDevicePixelsProperty, false);
			style3.Setters.Add(item10);
			Setter item11 = new Setter(Control.TemplateProperty, new ResourceReferenceExpression("HorizontalSliderHuePicker"));
			style3.Setters.Add(item11);
			e_27.Style = style3;
			e_27.TabIndex = 4;
			e_27.TickFrequency = 0.0027f;
			GamepadHelp.SetTargetName(e_27, "ColorHelp");
			GamepadHelp.SetTabIndexRight(e_27, 5);
			GamepadHelp.SetTabIndexUp(e_27, 1);
			Binding binding13 = new Binding("HueIcon");
			binding13.UseGeneratedBindings = true;
			e_27.SetBinding(RangeBase.ValueProperty, binding13);
			e_28 = new StackPanel();
			e_24.Children.Add(e_28);
			e_28.Name = "e_28";
			e_28.Margin = new Thickness(0f, 0f, 10f, 0f);
			e_28.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_28.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(e_28, 1);
			e_29 = new TextBlock();
			e_28.Children.Add(e_29);
			e_29.Name = "e_29";
			e_29.Margin = new Thickness(2f, 0f, 0f, 0f);
			e_29.SetResourceReference(TextBlock.TextProperty, "EditFaction_SaturationIconSliderText");
			e_30 = new Slider();
			e_28.Children.Add(e_30);
			e_30.Name = "e_30";
			e_30.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_30.VerticalAlignment = VerticalAlignment.Stretch;
			e_30.TabIndex = 5;
			e_30.TickFrequency = 0.01f;
			GamepadHelp.SetTargetName(e_30, "ColorHelp");
			GamepadHelp.SetTabIndexLeft(e_30, 4);
			GamepadHelp.SetTabIndexRight(e_30, 6);
			GamepadHelp.SetTabIndexUp(e_30, 2);
			Binding binding14 = new Binding("SaturationIcon");
			binding14.UseGeneratedBindings = true;
			e_30.SetBinding(RangeBase.ValueProperty, binding14);
			e_31 = new StackPanel();
			e_24.Children.Add(e_31);
			e_31.Name = "e_31";
			e_31.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_31.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(e_31, 2);
			e_32 = new TextBlock();
			e_31.Children.Add(e_32);
			e_32.Name = "e_32";
			e_32.Margin = new Thickness(2f, 0f, 0f, 0f);
			e_32.SetResourceReference(TextBlock.TextProperty, "EditFaction_ValueIconSliderText");
			e_33 = new Slider();
			e_31.Children.Add(e_33);
			e_33.Name = "e_33";
			e_33.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_33.VerticalAlignment = VerticalAlignment.Stretch;
			e_33.TabIndex = 6;
			e_33.TickFrequency = 0.01f;
			GamepadHelp.SetTargetName(e_33, "ColorHelp");
			GamepadHelp.SetTabIndexLeft(e_33, 5);
			GamepadHelp.SetTabIndexUp(e_33, 3);
			Binding binding15 = new Binding("ColorValueIcon");
			binding15.UseGeneratedBindings = true;
			e_33.SetBinding(RangeBase.ValueProperty, binding15);
			e_34 = new Border();
			e_2.Children.Add(e_34);
			e_34.Name = "e_34";
			e_34.Height = 2f;
			e_34.Margin = new Thickness(0f, 30f, 0f, 0f);
			e_34.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_34, 1);
			Grid.SetRow(e_34, 3);
			Grid.SetColumnSpan(e_34, 1);
			SelectionHelp = new Grid();
			e_2.Children.Add(SelectionHelp);
			SelectionHelp.Name = "SelectionHelp";
			SelectionHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition14 = new ColumnDefinition();
			columnDefinition14.Width = new GridLength(1f, GridUnitType.Auto);
			SelectionHelp.ColumnDefinitions.Add(columnDefinition14);
			ColumnDefinition columnDefinition15 = new ColumnDefinition();
			columnDefinition15.Width = new GridLength(1f, GridUnitType.Auto);
			SelectionHelp.ColumnDefinitions.Add(columnDefinition15);
			ColumnDefinition item12 = new ColumnDefinition();
			SelectionHelp.ColumnDefinitions.Add(item12);
			Grid.SetColumn(SelectionHelp, 1);
			Grid.SetRow(SelectionHelp, 4);
			e_35 = new TextBlock();
			SelectionHelp.Children.Add(e_35);
			e_35.Name = "e_35";
			e_35.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_35.HorizontalAlignment = HorizontalAlignment.Center;
			e_35.VerticalAlignment = VerticalAlignment.Center;
			e_35.SetResourceReference(TextBlock.TextProperty, "EditFactionLogoScreen_Help_Selection");
			e_36 = new TextBlock();
			SelectionHelp.Children.Add(e_36);
			e_36.Name = "e_36";
			e_36.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_36.HorizontalAlignment = HorizontalAlignment.Center;
			e_36.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_36, 1);
			e_36.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			ColorHelp = new Grid();
			e_2.Children.Add(ColorHelp);
			ColorHelp.Name = "ColorHelp";
			ColorHelp.Visibility = Visibility.Collapsed;
			ColorHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition16 = new ColumnDefinition();
			columnDefinition16.Width = new GridLength(1f, GridUnitType.Auto);
			ColorHelp.ColumnDefinitions.Add(columnDefinition16);
			ColumnDefinition columnDefinition17 = new ColumnDefinition();
			columnDefinition17.Width = new GridLength(1f, GridUnitType.Auto);
			ColorHelp.ColumnDefinitions.Add(columnDefinition17);
			ColumnDefinition columnDefinition18 = new ColumnDefinition();
			columnDefinition18.Width = new GridLength(1f, GridUnitType.Auto);
			ColorHelp.ColumnDefinitions.Add(columnDefinition18);
			ColumnDefinition item13 = new ColumnDefinition();
			ColorHelp.ColumnDefinitions.Add(item13);
			Grid.SetColumn(ColorHelp, 1);
			Grid.SetRow(ColorHelp, 4);
			e_37 = new TextBlock();
			ColorHelp.Children.Add(e_37);
			e_37.Name = "e_37";
			e_37.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_37.HorizontalAlignment = HorizontalAlignment.Center;
			e_37.VerticalAlignment = VerticalAlignment.Center;
			e_37.SetResourceReference(TextBlock.TextProperty, "EditFactionLogoScreen_Help_Selection");
			e_38 = new TextBlock();
			ColorHelp.Children.Add(e_38);
			e_38.Name = "e_38";
			e_38.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_38.HorizontalAlignment = HorizontalAlignment.Center;
			e_38.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_38, 1);
			e_38.SetResourceReference(TextBlock.TextProperty, "EditFactionLogoScreen_Help_ChangeColor");
			e_39 = new TextBlock();
			ColorHelp.Children.Add(e_39);
			e_39.Name = "e_39";
			e_39.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_39.HorizontalAlignment = HorizontalAlignment.Center;
			e_39.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_39, 2);
			e_39.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "MaxWidth", typeof(MyEditFactionIconViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "BackgroundOverlay", typeof(MyEditFactionIconViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "ExitCommand", typeof(MyEditFactionIconViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "OkCommand", typeof(MyEditFactionIconViewModel_OkCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "FactionColor", typeof(MyEditFactionIconViewModel_FactionColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "FactionIconBitmap", typeof(MyEditFactionIconViewModel_FactionIconBitmap_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "IconColorInternal", typeof(MyEditFactionIconViewModel_IconColorInternal_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "FactionIcons", typeof(MyEditFactionIconViewModel_FactionIcons_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "SelectedIcon", typeof(MyEditFactionIconViewModel_SelectedIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "Hue", typeof(MyEditFactionIconViewModel_Hue_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "Saturation", typeof(MyEditFactionIconViewModel_Saturation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "ColorValue", typeof(MyEditFactionIconViewModel_ColorValue_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "HueIcon", typeof(MyEditFactionIconViewModel_HueIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "SaturationIcon", typeof(MyEditFactionIconViewModel_SaturationIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyEditFactionIconViewModel), "ColorValueIcon", typeof(MyEditFactionIconViewModel_ColorValueIcon_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesEditFaction.Instance);
		}

		private static UIElement ListBoxIcons_s_S_1_ctMethod(UIElement parent)
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
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Center;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Top;
			UniformGrid uniformGrid2 = (UniformGrid)(scrollViewer.Content = new UniformGrid());
			uniformGrid2.Name = "e_11";
			uniformGrid2.Margin = new Thickness(6f, 6f, 6f, 6f);
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 6;
			return obj;
		}

		private static void InitializeElementListBoxIconsResources(UIElement elem)
		{
			Style style = new Style(typeof(ListBoxItem));
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("GridItem"));
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod = ListBoxIconsr_0_s_S_1_ctMethod;
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

		private static UIElement ListBoxIconsr_0_s_S_1_ctMethod(UIElement parent)
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
