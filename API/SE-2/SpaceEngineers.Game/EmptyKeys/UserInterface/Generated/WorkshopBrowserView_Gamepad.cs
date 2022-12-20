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
using EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class WorkshopBrowserView_Gamepad : UIRoot
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

		private ComboBox e_8;

		private ComboBox e_13;

		private AnimatedImage e_17;

		private CheckBox e_18;

		private CheckBox e_22;

		private Grid e_26;

		private TextBox e_27;

		private TextBlock e_28;

		private TextBlock e_29;

		private ListBox e_30;

		private Grid e_41;

		private Grid e_42;

		private ItemsControl e_43;

		private TextBlock e_46;

		private TextBlock e_47;

		private TextBlock e_48;

		private TextBlock e_49;

		private TextBlock e_50;

		private TextBlock e_51;

		private TextBlock e_52;

		private TextBlock e_53;

		private TextBlock e_54;

		private ScrollViewer e_55;

		private Border e_68;

		private TextBlock e_69;

		private Grid e_70;

		private ImageButton e_71;

		private StackPanel e_72;

		private TextBlock e_73;

		private TextBlock e_74;

		private TextBlock e_75;

		private ImageButton e_76;

		private Border e_77;

		private Grid ListHelp;

		private TextBlock e_78;

		private TextBlock e_79;

		private TextBlock e_80;

		private TextBlock e_81;

		private TextBlock e_82;

		private TextBlock e_83;

		private Grid SortHelp;

		private TextBlock e_84;

		private TextBlock e_85;

		private TextBlock e_86;

		private Grid CategoryHelp;

		private TextBlock e_87;

		private TextBlock e_88;

		private TextBlock e_89;

		private TextBlock e_90;

		public WorkshopBrowserView_Gamepad()
		{
			Initialize();
		}

		public WorkshopBrowserView_Gamepad(int width, int height)
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
			rowDefinition.Height = new GridLength(0.04f, GridUnitType.Star);
			rootGrid.RowDefinitions.Add(rowDefinition);
			RowDefinition item = new RowDefinition();
			rootGrid.RowDefinitions.Add(item);
			RowDefinition rowDefinition2 = new RowDefinition();
			rowDefinition2.Height = new GridLength(0.04f, GridUnitType.Star);
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
			columnDefinition.Width = new GridLength(70f, GridUnitType.Pixel);
			e_1.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition columnDefinition2 = new ColumnDefinition();
			columnDefinition2.Width = new GridLength(4f, GridUnitType.Star);
			e_1.ColumnDefinitions.Add(columnDefinition2);
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition3.Width = new GridLength(70f, GridUnitType.Pixel);
			e_1.ColumnDefinitions.Add(columnDefinition3);
			Grid.SetRow(e_1, 1);
			e_2 = new ImageButton();
			e_1.Children.Add(e_2);
			e_2.Name = "e_2";
			e_2.Height = 24f;
			e_2.Width = 24f;
			e_2.Margin = new Thickness(16f, 16f, 16f, 5f);
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
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionWorkshopBrowser");
			e_5 = new Border();
			e_3.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.Height = 2f;
			e_5.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_5.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_6 = new Grid();
			e_1.Children.Add(e_6);
			e_6.Name = "e_6";
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.DButton);
			Binding binding4 = new Binding("RefreshCommand");
			binding4.UseGeneratedBindings = true;
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding4);
			e_6.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = e_6;
			RowDefinition rowDefinition10 = new RowDefinition();
			rowDefinition10.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition10);
			RowDefinition item3 = new RowDefinition();
			e_6.RowDefinitions.Add(item3);
			RowDefinition rowDefinition11 = new RowDefinition();
			rowDefinition11.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition11);
			RowDefinition rowDefinition12 = new RowDefinition();
			rowDefinition12.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition12);
			RowDefinition rowDefinition13 = new RowDefinition();
			rowDefinition13.Height = new GridLength(90f, GridUnitType.Pixel);
			e_6.RowDefinitions.Add(rowDefinition13);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(3f, GridUnitType.Star);
			e_6.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(1f, GridUnitType.Star);
			e_6.ColumnDefinitions.Add(columnDefinition5);
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 3);
			e_7 = new Grid();
			e_6.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(0f, 0f, 10f, 0f);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(200f, GridUnitType.Pixel);
			e_7.ColumnDefinitions.Add(columnDefinition6);
			ColumnDefinition columnDefinition7 = new ColumnDefinition();
			columnDefinition7.Width = new GridLength(200f, GridUnitType.Pixel);
			e_7.ColumnDefinitions.Add(columnDefinition7);
			ColumnDefinition columnDefinition8 = new ColumnDefinition();
			columnDefinition8.Width = new GridLength(1f, GridUnitType.Auto);
			e_7.ColumnDefinitions.Add(columnDefinition8);
			ColumnDefinition columnDefinition9 = new ColumnDefinition();
			columnDefinition9.Width = new GridLength(26f, GridUnitType.Pixel);
			e_7.ColumnDefinitions.Add(columnDefinition9);
			ColumnDefinition columnDefinition10 = new ColumnDefinition();
			columnDefinition10.Width = new GridLength(38f, GridUnitType.Pixel);
			e_7.ColumnDefinitions.Add(columnDefinition10);
			ColumnDefinition columnDefinition11 = new ColumnDefinition();
			columnDefinition11.Width = new GridLength(38f, GridUnitType.Pixel);
			e_7.ColumnDefinitions.Add(columnDefinition11);
			ColumnDefinition item4 = new ColumnDefinition();
			e_7.ColumnDefinitions.Add(item4);
			e_8 = new ComboBox();
			e_7.Children.Add(e_8);
			e_8.Name = "e_8";
			e_8.VerticalAlignment = VerticalAlignment.Center;
			e_8.TabIndex = 1;
<<<<<<< HEAD
			e_8.ItemsSource = Get_e_8_Items();
=======
			e_8.ItemsSource = (IEnumerable)Get_e_8_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			e_8.MaxDropDownHeight = 120f;
			GamepadHelp.SetTargetName(e_8, "SortHelp");
			GamepadHelp.SetTabIndexRight(e_8, 3);
			GamepadHelp.SetTabIndexDown(e_8, 5);
			Binding binding5 = new Binding("SelectedSortIndex");
			binding5.UseGeneratedBindings = true;
			e_8.SetBinding(Selector.SelectedIndexProperty, binding5);
			e_13 = new ComboBox();
			e_7.Children.Add(e_13);
			e_13.Name = "e_13";
			e_13.Margin = new Thickness(5f, 0f, 0f, 0f);
			e_13.VerticalAlignment = VerticalAlignment.Center;
			e_13.TabIndex = 3;
			Func<UIElement, UIElement> createMethod = e_13_dtMethod;
			e_13.ItemTemplate = new DataTemplate(createMethod);
			e_13.MaxDropDownHeight = 300f;
			Grid.SetColumn(e_13, 1);
			GamepadHelp.SetTargetName(e_13, "CategoryHelp");
			GamepadHelp.SetTabIndexLeft(e_13, 1);
			GamepadHelp.SetTabIndexDown(e_13, 5);
			Binding binding6 = new Binding("Categories");
			binding6.UseGeneratedBindings = true;
			e_13.SetBinding(ItemsControl.ItemsSourceProperty, binding6);
			Binding binding7 = new Binding("SelectedCategoryIndex");
			binding7.UseGeneratedBindings = true;
			e_13.SetBinding(Selector.SelectedIndexProperty, binding7);
			Binding binding8 = new Binding("CategoryControlTabIndexRight");
			binding8.UseGeneratedBindings = true;
			e_13.SetBinding(GamepadHelp.TabIndexRightProperty, binding8);
			e_17 = new AnimatedImage();
			e_7.Children.Add(e_17);
			e_17.Name = "e_17";
			e_17.Width = 24f;
			e_17.HorizontalAlignment = HorizontalAlignment.Center;
			e_17.VerticalAlignment = VerticalAlignment.Center;
			BitmapImage bitmapImage5 = new BitmapImage();
			bitmapImage5.TextureAsset = "Textures\\GUI\\LoadingIconAnimated.png";
			e_17.Source = bitmapImage5;
			e_17.FrameWidth = 128;
			e_17.FrameHeight = 128;
			e_17.FramesPerSecond = 20;
			Grid.SetColumn(e_17, 3);
			Binding binding9 = new Binding("IsRefreshing");
			binding9.UseGeneratedBindings = true;
			e_17.SetBinding(UIElement.VisibilityProperty, binding9);
			e_18 = new CheckBox();
			e_7.Children.Add(e_18);
			e_18.Name = "e_18";
			e_18.IsEnabled = true;
			e_18.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_18.VerticalAlignment = VerticalAlignment.Center;
			Style style = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod2 = e_18_s_S_0_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(CheckBox), createMethod2);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsMouseOverProperty;
			trigger.Value = true;
			Setter setter = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckServiceBackHighlighted"));
			setter.TargetName = "PART_Background";
			trigger.Setters.Add(setter);
			controlTemplate.Triggers.Add(trigger);
			Trigger trigger2 = new Trigger();
			trigger2.Property = UIElement.IsFocusedProperty;
			trigger2.Value = true;
			Setter setter2 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckServiceBackFocused"));
			setter2.TargetName = "PART_Background";
			trigger2.Setters.Add(setter2);
			Setter setter3 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckService0Focused"));
			setter3.TargetName = "PART_Icon";
			trigger2.Setters.Add(setter3);
			controlTemplate.Triggers.Add(trigger2);
			Trigger trigger3 = new Trigger();
			trigger3.Property = ToggleButton.IsCheckedProperty;
			trigger3.Value = true;
			Setter setter4 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckServiceBackChecked"));
			setter4.TargetName = "PART_Background";
			trigger3.Setters.Add(setter4);
			controlTemplate.Triggers.Add(trigger3);
			Setter item5 = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item5);
			e_18.Style = style;
			e_18.TabIndex = 11;
			Grid.SetColumn(e_18, 4);
			GamepadHelp.SetTargetName(e_18, "SortHelp");
			GamepadHelp.SetTabIndexLeft(e_18, 3);
			GamepadHelp.SetTabIndexRight(e_18, 12);
			GamepadHelp.SetTabIndexDown(e_18, 5);
			Binding binding10 = new Binding("IsWorkshopAggregator");
			binding10.UseGeneratedBindings = true;
			e_18.SetBinding(UIElement.VisibilityProperty, binding10);
			Binding binding11 = new Binding("ServiceCommand");
			binding11.UseGeneratedBindings = true;
			e_18.SetBinding(Button.CommandProperty, binding11);
			Binding binding12 = new Binding("Service0IsChecked");
			binding12.UseGeneratedBindings = true;
			e_18.SetBinding(ToggleButton.IsCheckedProperty, binding12);
			e_18.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Service0");
			e_22 = new CheckBox();
			e_7.Children.Add(e_22);
			e_22.Name = "e_22";
			e_22.IsEnabled = true;
			e_22.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_22.VerticalAlignment = VerticalAlignment.Center;
			Style style2 = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod3 = e_22_s_S_0_ctMethod;
			ControlTemplate controlTemplate2 = new ControlTemplate(typeof(CheckBox), createMethod3);
			Trigger trigger4 = new Trigger();
			trigger4.Property = UIElement.IsMouseOverProperty;
			trigger4.Value = true;
			Setter setter5 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckServiceBackHighlighted"));
			setter5.TargetName = "PART_Background";
			trigger4.Setters.Add(setter5);
			controlTemplate2.Triggers.Add(trigger4);
			Trigger trigger5 = new Trigger();
			trigger5.Property = UIElement.IsFocusedProperty;
			trigger5.Value = true;
			ImageBrush imageBrush = new ImageBrush();
			BitmapImage bitmapImage6 = new BitmapImage();
			bitmapImage6.TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png";
			imageBrush.ImageSource = bitmapImage6;
			Setter setter6 = new Setter(Control.BackgroundProperty, imageBrush);
			setter6.TargetName = "PART_Background";
			trigger5.Setters.Add(setter6);
			ImageBrush imageBrush2 = new ImageBrush();
			BitmapImage bitmapImage7 = new BitmapImage();
			bitmapImage7.TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png";
			imageBrush2.ImageSource = bitmapImage7;
			Setter setter7 = new Setter(Control.BackgroundProperty, imageBrush2);
			setter7.TargetName = "PART_Icon";
			trigger5.Setters.Add(setter7);
			controlTemplate2.Triggers.Add(trigger5);
			Trigger trigger6 = new Trigger();
			trigger6.Property = ToggleButton.IsCheckedProperty;
			trigger6.Value = true;
			Setter setter8 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckServiceBackChecked"));
			setter8.TargetName = "PART_Background";
			trigger6.Setters.Add(setter8);
			controlTemplate2.Triggers.Add(trigger6);
			Setter item6 = new Setter(Control.TemplateProperty, controlTemplate2);
			style2.Setters.Add(item6);
			e_22.Style = style2;
			e_22.TabIndex = 12;
			Grid.SetColumn(e_22, 5);
			GamepadHelp.SetTargetName(e_22, "SortHelp");
			GamepadHelp.SetTabIndexLeft(e_22, 11);
			GamepadHelp.SetTabIndexRight(e_22, 4);
			GamepadHelp.SetTabIndexDown(e_22, 5);
			Binding binding13 = new Binding("IsWorkshopAggregator");
			binding13.UseGeneratedBindings = true;
			e_22.SetBinding(UIElement.VisibilityProperty, binding13);
			Binding binding14 = new Binding("ServiceCommand");
			binding14.UseGeneratedBindings = true;
			e_22.SetBinding(Button.CommandProperty, binding14);
			Binding binding15 = new Binding("Service1IsChecked");
			binding15.UseGeneratedBindings = true;
			e_22.SetBinding(ToggleButton.IsCheckedProperty, binding15);
			e_22.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Service1");
			e_26 = new Grid();
			e_7.Children.Add(e_26);
			e_26.Name = "e_26";
			Grid.SetColumn(e_26, 6);
			e_27 = new TextBox();
			e_26.Children.Add(e_27);
			e_27.Name = "e_27";
			e_27.VerticalAlignment = VerticalAlignment.Center;
			e_27.TabIndex = 4;
			GamepadHelp.SetTargetName(e_27, "SortHelp");
			GamepadHelp.SetTabIndexDown(e_27, 5);
			Binding binding16 = new Binding("SearchText");
			binding16.UseGeneratedBindings = true;
			e_27.SetBinding(TextBox.TextProperty, binding16);
			Binding binding17 = new Binding("SearchControlTabIndexLeft");
			binding17.UseGeneratedBindings = true;
			e_27.SetBinding(GamepadHelp.TabIndexLeftProperty, binding17);
			e_28 = new TextBlock();
			e_26.Children.Add(e_28);
			e_28.Name = "e_28";
			e_28.IsHitTestVisible = false;
			e_28.Margin = new Thickness(4f, 4f, 4f, 4f);
			e_28.VerticalAlignment = VerticalAlignment.Center;
			e_28.Foreground = new SolidColorBrush(new ColorW(94, 115, 127, 255));
			Binding binding18 = new Binding("IsSearchLabelVisible");
			binding18.UseGeneratedBindings = true;
			e_28.SetBinding(UIElement.VisibilityProperty, binding18);
			e_28.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Search");
			e_29 = new TextBlock();
			e_6.Children.Add(e_29);
			e_29.Name = "e_29";
			e_29.Margin = new Thickness(0f, 10f, 10f, 10f);
			e_29.HorizontalAlignment = HorizontalAlignment.Center;
			e_29.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(e_29, 1);
			Grid.SetColumnSpan(e_29, 2);
			Binding binding19 = new Binding("IsNotFoundTextVisible");
			binding19.UseGeneratedBindings = true;
			e_29.SetBinding(UIElement.VisibilityProperty, binding19);
			e_29.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_NotFound");
			e_30 = new ListBox();
			e_6.Children.Add(e_30);
			e_30.Name = "e_30";
			e_30.Margin = new Thickness(0f, 10f, 10f, 10f);
			e_30.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_30.VerticalAlignment = VerticalAlignment.Stretch;
			Style style3 = new Style(typeof(ListBox));
			Setter item7 = new Setter(UIElement.MinHeightProperty, 80f);
			style3.Setters.Add(item7);
			Func<UIElement, UIElement> createMethod4 = e_30_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod4);
			Setter item8 = new Setter(Control.TemplateProperty, value);
			style3.Setters.Add(item8);
			Trigger trigger7 = new Trigger();
			trigger7.Property = UIElement.IsFocusedProperty;
			trigger7.Value = true;
			Setter item9 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger7.Setters.Add(item9);
			style3.Triggers.Add(trigger7);
			e_30.Style = style3;
			GamepadBinding gamepadBinding2 = new GamepadBinding();
			gamepadBinding2.Gesture = new GamepadGesture(GamepadInput.AButton);
			Binding binding20 = new Binding("ToggleSubscriptionCommand");
			binding20.UseGeneratedBindings = true;
			gamepadBinding2.SetBinding(InputBinding.CommandProperty, binding20);
			e_30.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = e_30;
			GamepadBinding gamepadBinding3 = new GamepadBinding();
			gamepadBinding3.Gesture = new GamepadGesture(GamepadInput.LeftShoulderButton);
			Binding binding21 = new Binding("PreviousPageCommand");
			binding21.UseGeneratedBindings = true;
			gamepadBinding3.SetBinding(InputBinding.CommandProperty, binding21);
			e_30.InputBindings.Add(gamepadBinding3);
			gamepadBinding3.Parent = e_30;
			GamepadBinding gamepadBinding4 = new GamepadBinding();
			gamepadBinding4.Gesture = new GamepadGesture(GamepadInput.RightShoulderButton);
			Binding binding22 = new Binding("NextPageCommand");
			binding22.UseGeneratedBindings = true;
			gamepadBinding4.SetBinding(InputBinding.CommandProperty, binding22);
			e_30.InputBindings.Add(gamepadBinding4);
			gamepadBinding4.Parent = e_30;
			GamepadBinding gamepadBinding5 = new GamepadBinding();
			gamepadBinding5.Gesture = new GamepadGesture(GamepadInput.SelectButton);
			Binding binding23 = new Binding("BrowseWorkshopCommand");
			binding23.UseGeneratedBindings = true;
			gamepadBinding5.SetBinding(InputBinding.CommandProperty, binding23);
			e_30.InputBindings.Add(gamepadBinding5);
			gamepadBinding5.Parent = e_30;
			GamepadBinding gamepadBinding6 = new GamepadBinding();
			gamepadBinding6.Gesture = new GamepadGesture(GamepadInput.StartButton);
			Binding binding24 = new Binding("OpenItemInWorkshopCommand");
			binding24.UseGeneratedBindings = true;
			gamepadBinding6.SetBinding(InputBinding.CommandProperty, binding24);
			e_30.InputBindings.Add(gamepadBinding6);
			gamepadBinding6.Parent = e_30;
			e_30.TabIndex = 5;
			Func<UIElement, UIElement> createMethod5 = e_30_dtMethod;
			e_30.ItemTemplate = new DataTemplate(createMethod5);
			Grid.SetRow(e_30, 1);
			GamepadHelp.SetTargetName(e_30, "ListHelp");
			GamepadHelp.SetTabIndexRight(e_30, 8);
			GamepadHelp.SetTabIndexUp(e_30, 3);
			Binding binding25 = new Binding("WorkshopItems");
			binding25.UseGeneratedBindings = true;
			e_30.SetBinding(ItemsControl.ItemsSourceProperty, binding25);
			Binding binding26 = new Binding("SelectedWorkshopItem");
			binding26.UseGeneratedBindings = true;
			e_30.SetBinding(Selector.SelectedItemProperty, binding26);
			e_41 = new Grid();
			e_6.Children.Add(e_41);
			e_41.Name = "e_41";
			e_41.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			RowDefinition rowDefinition14 = new RowDefinition();
			rowDefinition14.Height = new GridLength(1f, GridUnitType.Auto);
			e_41.RowDefinitions.Add(rowDefinition14);
			RowDefinition item10 = new RowDefinition();
			e_41.RowDefinitions.Add(item10);
			RowDefinition rowDefinition15 = new RowDefinition();
			rowDefinition15.Height = new GridLength(1f, GridUnitType.Auto);
			e_41.RowDefinitions.Add(rowDefinition15);
			Grid.SetColumn(e_41, 1);
			Grid.SetRow(e_41, 0);
			Grid.SetRowSpan(e_41, 3);
			Binding binding27 = new Binding("IsDetailVisible");
			binding27.UseGeneratedBindings = true;
			e_41.SetBinding(UIElement.VisibilityProperty, binding27);
			e_42 = new Grid();
			e_41.Children.Add(e_42);
			e_42.Name = "e_42";
			e_42.Margin = new Thickness(10f, 10f, 10f, 10f);
			RowDefinition rowDefinition16 = new RowDefinition();
			rowDefinition16.Height = new GridLength(1f, GridUnitType.Auto);
			e_42.RowDefinitions.Add(rowDefinition16);
			RowDefinition rowDefinition17 = new RowDefinition();
			rowDefinition17.Height = new GridLength(1f, GridUnitType.Auto);
			e_42.RowDefinitions.Add(rowDefinition17);
			RowDefinition rowDefinition18 = new RowDefinition();
			rowDefinition18.Height = new GridLength(1f, GridUnitType.Auto);
			e_42.RowDefinitions.Add(rowDefinition18);
			RowDefinition rowDefinition19 = new RowDefinition();
			rowDefinition19.Height = new GridLength(1f, GridUnitType.Auto);
			e_42.RowDefinitions.Add(rowDefinition19);
			RowDefinition rowDefinition20 = new RowDefinition();
			rowDefinition20.Height = new GridLength(1f, GridUnitType.Auto);
			e_42.RowDefinitions.Add(rowDefinition20);
			RowDefinition rowDefinition21 = new RowDefinition();
			rowDefinition21.Height = new GridLength(1f, GridUnitType.Auto);
			e_42.RowDefinitions.Add(rowDefinition21);
			ColumnDefinition columnDefinition12 = new ColumnDefinition();
			columnDefinition12.Width = new GridLength(1f, GridUnitType.Auto);
			e_42.ColumnDefinitions.Add(columnDefinition12);
			ColumnDefinition item11 = new ColumnDefinition();
			e_42.ColumnDefinitions.Add(item11);
			e_43 = new ItemsControl();
			e_42.Children.Add(e_43);
			e_43.Name = "e_43";
			ControlTemplate itemsPanel = new ControlTemplate(e_43_iptMethod);
			e_43.ItemsPanel = itemsPanel;
			Func<UIElement, UIElement> createMethod6 = e_43_dtMethod;
			e_43.ItemTemplate = new DataTemplate(createMethod6);
			Grid.SetRow(e_43, 0);
			Grid.SetColumnSpan(e_43, 2);
			Binding binding28 = new Binding("SelectedWorkshopItem.Rating");
			binding28.UseGeneratedBindings = true;
			e_43.SetBinding(ItemsControl.ItemsSourceProperty, binding28);
			e_46 = new TextBlock();
			e_42.Children.Add(e_46);
			e_46.Name = "e_46";
			e_46.Margin = new Thickness(0f, 0f, 0f, 10f);
			e_46.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_46.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(e_46, 1);
			Grid.SetColumnSpan(e_46, 2);
			Binding binding29 = new Binding("SelectedWorkshopItem.Title");
			binding29.UseGeneratedBindings = true;
			e_46.SetBinding(TextBlock.TextProperty, binding29);
			e_47 = new TextBlock();
			e_42.Children.Add(e_47);
			e_47.Name = "e_47";
			e_47.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_47.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_47.FontFamily = new FontFamily("InventorySmall");
			e_47.FontSize = 10f;
			Grid.SetRow(e_47, 2);
			e_47.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_FileSize");
			e_48 = new TextBlock();
			e_42.Children.Add(e_48);
			e_48.Name = "e_48";
			e_48.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_48.FontFamily = new FontFamily("InventorySmall");
			e_48.FontSize = 10f;
			Grid.SetColumn(e_48, 1);
			Grid.SetRow(e_48, 2);
			Binding binding30 = new Binding("SelectedWorkshopItem.Size");
			binding30.UseGeneratedBindings = true;
			binding30.StringFormat = "{0:N0} B";
			e_48.SetBinding(TextBlock.TextProperty, binding30);
			e_49 = new TextBlock();
			e_42.Children.Add(e_49);
			e_49.Name = "e_49";
			e_49.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_49.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_49.FontFamily = new FontFamily("InventorySmall");
			e_49.FontSize = 10f;
			Grid.SetRow(e_49, 3);
			e_49.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Subscribers");
			e_50 = new TextBlock();
			e_42.Children.Add(e_50);
			e_50.Name = "e_50";
			e_50.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_50.FontFamily = new FontFamily("InventorySmall");
			e_50.FontSize = 10f;
			Grid.SetColumn(e_50, 1);
			Grid.SetRow(e_50, 3);
			Binding binding31 = new Binding("SelectedWorkshopItem.NumSubscriptions");
			binding31.UseGeneratedBindings = true;
			binding31.StringFormat = "{0:N0}";
			e_50.SetBinding(TextBlock.TextProperty, binding31);
			e_51 = new TextBlock();
			e_42.Children.Add(e_51);
			e_51.Name = "e_51";
			e_51.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_51.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_51.FontFamily = new FontFamily("InventorySmall");
			e_51.FontSize = 10f;
			Grid.SetRow(e_51, 4);
			e_51.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Created");
			e_52 = new TextBlock();
			e_42.Children.Add(e_52);
			e_52.Name = "e_52";
			e_52.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_52.FontFamily = new FontFamily("InventorySmall");
			e_52.FontSize = 10f;
			Grid.SetColumn(e_52, 1);
			Grid.SetRow(e_52, 4);
			Binding binding32 = new Binding("SelectedWorkshopItem.TimeCreated");
			binding32.UseGeneratedBindings = true;
			e_52.SetBinding(TextBlock.TextProperty, binding32);
			e_53 = new TextBlock();
			e_42.Children.Add(e_53);
			e_53.Name = "e_53";
			e_53.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_53.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_53.FontFamily = new FontFamily("InventorySmall");
			e_53.FontSize = 10f;
			Grid.SetRow(e_53, 5);
			e_53.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Updated");
			e_54 = new TextBlock();
			e_42.Children.Add(e_54);
			e_54.Name = "e_54";
			e_54.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_54.FontFamily = new FontFamily("InventorySmall");
			e_54.FontSize = 10f;
			Grid.SetColumn(e_54, 1);
			Grid.SetRow(e_54, 5);
			Binding binding33 = new Binding("SelectedWorkshopItem.TimeUpdated");
			binding33.UseGeneratedBindings = true;
			e_54.SetBinding(TextBlock.TextProperty, binding33);
			e_55 = new ScrollViewer();
			e_41.Children.Add(e_55);
			e_55.Name = "e_55";
			e_55.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_55.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_55.VerticalAlignment = VerticalAlignment.Stretch;
			Style style4 = new Style(typeof(ScrollViewer));
			Setter item12 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style4.Setters.Add(item12);
			ImageBrush imageBrush3 = new ImageBrush();
			BitmapImage bitmapImage8 = new BitmapImage();
			bitmapImage8.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush3.ImageSource = bitmapImage8;
			Setter item13 = new Setter(Control.BorderBrushProperty, imageBrush3);
			style4.Setters.Add(item13);
			Func<UIElement, UIElement> createMethod7 = e_55_s_S_2_ctMethod;
			ControlTemplate controlTemplate3 = new ControlTemplate(typeof(ScrollViewer), createMethod7);
			Trigger trigger8 = new Trigger();
			trigger8.Property = UIElement.IsFocusedProperty;
			trigger8.Value = true;
			ImageBrush imageBrush4 = new ImageBrush();
			BitmapImage bitmapImage9 = new BitmapImage();
			bitmapImage9.TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds";
			imageBrush4.ImageSource = bitmapImage9;
			Setter item14 = new Setter(Control.BorderBrushProperty, imageBrush4);
			trigger8.Setters.Add(item14);
			controlTemplate3.Triggers.Add(trigger8);
			Trigger trigger9 = new Trigger();
			trigger9.Property = UIElement.IsFocusedProperty;
			trigger9.Value = false;
			ImageBrush imageBrush5 = new ImageBrush();
			BitmapImage bitmapImage10 = new BitmapImage();
			bitmapImage10.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush5.ImageSource = bitmapImage10;
			Setter item15 = new Setter(Control.BorderBrushProperty, imageBrush5);
			trigger9.Setters.Add(item15);
			controlTemplate3.Triggers.Add(trigger9);
			Setter item16 = new Setter(Control.TemplateProperty, controlTemplate3);
			style4.Setters.Add(item16);
			e_55.Style = style4;
			e_55.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			e_55.VerticalContentAlignment = VerticalAlignment.Stretch;
			e_55.IsTabStop = true;
			e_55.TabIndex = 8;
			Grid.SetRow(e_55, 1);
			GamepadHelp.SetTargetName(e_55, "SortHelp");
			GamepadHelp.SetTabIndexLeft(e_55, 5);
			e_68 = new Border();
			e_55.Content = e_68;
			e_68.Name = "e_68";
			e_68.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_68.VerticalAlignment = VerticalAlignment.Stretch;
			e_68.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_69 = new TextBlock();
			e_68.Child = e_69;
			e_69.Name = "e_69";
			e_69.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_69.TextWrapping = TextWrapping.Wrap;
			e_69.FontFamily = new FontFamily("InventorySmall");
			e_69.FontSize = 10f;
			Binding binding34 = new Binding("SelectedWorkshopItem.Description");
			binding34.UseGeneratedBindings = true;
			e_69.SetBinding(TextBlock.TextProperty, binding34);
			e_70 = new Grid();
			e_6.Children.Add(e_70);
			e_70.Name = "e_70";
			ColumnDefinition item17 = new ColumnDefinition();
			e_70.ColumnDefinitions.Add(item17);
			ColumnDefinition columnDefinition13 = new ColumnDefinition();
			columnDefinition13.Width = new GridLength(1f, GridUnitType.Auto);
			e_70.ColumnDefinitions.Add(columnDefinition13);
			ColumnDefinition item18 = new ColumnDefinition();
			e_70.ColumnDefinitions.Add(item18);
			ColumnDefinition columnDefinition14 = new ColumnDefinition();
			columnDefinition14.Width = new GridLength(1f, GridUnitType.Auto);
			e_70.ColumnDefinitions.Add(columnDefinition14);
			ColumnDefinition item19 = new ColumnDefinition();
			e_70.ColumnDefinitions.Add(item19);
			Grid.SetRow(e_70, 2);
			Binding binding35 = new Binding("IsPagingInfoVisible");
			binding35.UseGeneratedBindings = true;
			e_70.SetBinding(UIElement.VisibilityProperty, binding35);
			e_71 = new ImageButton();
			e_70.Children.Add(e_71);
			e_71.Name = "e_71";
			e_71.Width = 16f;
			e_71.HorizontalAlignment = HorizontalAlignment.Right;
			e_71.TabIndex = 6;
			BitmapImage bitmapImage11 = new BitmapImage();
			bitmapImage11.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds";
			e_71.ImageNormal = bitmapImage11;
			BitmapImage bitmapImage12 = new BitmapImage();
			bitmapImage12.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds";
			e_71.ImageDisabled = bitmapImage12;
			BitmapImage bitmapImage13 = new BitmapImage();
			bitmapImage13.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds";
			e_71.ImageHover = bitmapImage13;
			Grid.SetColumn(e_71, 1);
			Binding binding36 = new Binding("IsQueryFinished");
			binding36.UseGeneratedBindings = true;
			e_71.SetBinding(UIElement.IsEnabledProperty, binding36);
			Binding binding37 = new Binding("PreviousPageCommand");
			binding37.UseGeneratedBindings = true;
			e_71.SetBinding(Button.CommandProperty, binding37);
			e_71.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_PreviousPage");
			e_72 = new StackPanel();
			e_70.Children.Add(e_72);
			e_72.Name = "e_72";
			e_72.HorizontalAlignment = HorizontalAlignment.Center;
			e_72.VerticalAlignment = VerticalAlignment.Center;
			e_72.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_72, 2);
			e_73 = new TextBlock();
			e_72.Children.Add(e_73);
			e_73.Name = "e_73";
			Binding binding38 = new Binding("CurrentPage");
			binding38.UseGeneratedBindings = true;
			e_73.SetBinding(TextBlock.TextProperty, binding38);
			e_74 = new TextBlock();
			e_72.Children.Add(e_74);
			e_74.Name = "e_74";
			e_74.Text = "/";
			e_75 = new TextBlock();
			e_72.Children.Add(e_75);
			e_75.Name = "e_75";
			Binding binding39 = new Binding("TotalPages");
			binding39.UseGeneratedBindings = true;
			e_75.SetBinding(TextBlock.TextProperty, binding39);
			e_76 = new ImageButton();
			e_70.Children.Add(e_76);
			e_76.Name = "e_76";
			e_76.Width = 16f;
			e_76.HorizontalAlignment = HorizontalAlignment.Left;
			e_76.TabIndex = 7;
			BitmapImage bitmapImage14 = new BitmapImage();
			bitmapImage14.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds";
			e_76.ImageNormal = bitmapImage14;
			BitmapImage bitmapImage15 = new BitmapImage();
			bitmapImage15.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds";
			e_76.ImageDisabled = bitmapImage15;
			BitmapImage bitmapImage16 = new BitmapImage();
			bitmapImage16.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds";
			e_76.ImageHover = bitmapImage16;
			Grid.SetColumn(e_76, 3);
			Binding binding40 = new Binding("IsQueryFinished");
			binding40.UseGeneratedBindings = true;
			e_76.SetBinding(UIElement.IsEnabledProperty, binding40);
			Binding binding41 = new Binding("NextPageCommand");
			binding41.UseGeneratedBindings = true;
			e_76.SetBinding(Button.CommandProperty, binding41);
			e_76.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_NextPage");
			e_77 = new Border();
			e_6.Children.Add(e_77);
			e_77.Name = "e_77";
			e_77.Height = 2f;
			e_77.Margin = new Thickness(0f, 10f, 0f, 0f);
			e_77.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(e_77, 3);
			Grid.SetColumnSpan(e_77, 2);
			ListHelp = new Grid();
			e_6.Children.Add(ListHelp);
			ListHelp.Name = "ListHelp";
			ListHelp.Visibility = Visibility.Collapsed;
			ListHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition15 = new ColumnDefinition();
			columnDefinition15.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition15);
			ColumnDefinition columnDefinition16 = new ColumnDefinition();
			columnDefinition16.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition16);
			ColumnDefinition columnDefinition17 = new ColumnDefinition();
			columnDefinition17.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition17);
			ColumnDefinition columnDefinition18 = new ColumnDefinition();
			columnDefinition18.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition18);
			ColumnDefinition columnDefinition19 = new ColumnDefinition();
			columnDefinition19.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition19);
			ColumnDefinition columnDefinition20 = new ColumnDefinition();
			columnDefinition20.Width = new GridLength(1f, GridUnitType.Auto);
			ListHelp.ColumnDefinitions.Add(columnDefinition20);
			ColumnDefinition item20 = new ColumnDefinition();
			ListHelp.ColumnDefinitions.Add(item20);
			Grid.SetRow(ListHelp, 4);
			e_78 = new TextBlock();
			ListHelp.Children.Add(e_78);
			e_78.Name = "e_78";
			e_78.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_78.HorizontalAlignment = HorizontalAlignment.Center;
			e_78.VerticalAlignment = VerticalAlignment.Center;
			e_78.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_ToggleSubscribe");
			e_79 = new TextBlock();
			ListHelp.Children.Add(e_79);
			e_79.Name = "e_79";
			e_79.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_79.HorizontalAlignment = HorizontalAlignment.Center;
			e_79.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_79, 1);
			e_79.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_Paging");
			e_80 = new TextBlock();
			ListHelp.Children.Add(e_80);
			e_80.Name = "e_80";
			e_80.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_80.HorizontalAlignment = HorizontalAlignment.Center;
			e_80.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_80, 2);
			e_80.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_OpenItem");
			e_81 = new TextBlock();
			ListHelp.Children.Add(e_81);
			e_81.Name = "e_81";
			e_81.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_81.HorizontalAlignment = HorizontalAlignment.Center;
			e_81.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_81, 3);
			e_81.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_OpenWorkshop");
			e_82 = new TextBlock();
			ListHelp.Children.Add(e_82);
			e_82.Name = "e_82";
			e_82.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_82.HorizontalAlignment = HorizontalAlignment.Center;
			e_82.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_82, 4);
			e_82.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_Refresh");
			e_83 = new TextBlock();
			ListHelp.Children.Add(e_83);
			e_83.Name = "e_83";
			e_83.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_83.HorizontalAlignment = HorizontalAlignment.Center;
			e_83.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_83, 5);
			e_83.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			SortHelp = new Grid();
			e_6.Children.Add(SortHelp);
			SortHelp.Name = "SortHelp";
			SortHelp.Visibility = Visibility.Collapsed;
			SortHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition21 = new ColumnDefinition();
			columnDefinition21.Width = new GridLength(1f, GridUnitType.Auto);
			SortHelp.ColumnDefinitions.Add(columnDefinition21);
			ColumnDefinition columnDefinition22 = new ColumnDefinition();
			columnDefinition22.Width = new GridLength(1f, GridUnitType.Auto);
			SortHelp.ColumnDefinitions.Add(columnDefinition22);
			ColumnDefinition columnDefinition23 = new ColumnDefinition();
			columnDefinition23.Width = new GridLength(1f, GridUnitType.Auto);
			SortHelp.ColumnDefinitions.Add(columnDefinition23);
			ColumnDefinition columnDefinition24 = new ColumnDefinition();
			columnDefinition24.Width = new GridLength(1f, GridUnitType.Auto);
			SortHelp.ColumnDefinitions.Add(columnDefinition24);
			ColumnDefinition item21 = new ColumnDefinition();
			SortHelp.ColumnDefinitions.Add(item21);
			Grid.SetRow(SortHelp, 4);
			e_84 = new TextBlock();
			SortHelp.Children.Add(e_84);
			e_84.Name = "e_84";
			e_84.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_84.HorizontalAlignment = HorizontalAlignment.Center;
			e_84.VerticalAlignment = VerticalAlignment.Center;
			e_84.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_Select");
			e_85 = new TextBlock();
			SortHelp.Children.Add(e_85);
			e_85.Name = "e_85";
			e_85.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_85.HorizontalAlignment = HorizontalAlignment.Center;
			e_85.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_85, 2);
			e_85.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_Refresh");
			e_86 = new TextBlock();
			SortHelp.Children.Add(e_86);
			e_86.Name = "e_86";
			e_86.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_86.HorizontalAlignment = HorizontalAlignment.Center;
			e_86.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_86, 3);
			e_86.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			CategoryHelp = new Grid();
			e_6.Children.Add(CategoryHelp);
			CategoryHelp.Name = "CategoryHelp";
			CategoryHelp.Visibility = Visibility.Collapsed;
			CategoryHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition25 = new ColumnDefinition();
			columnDefinition25.Width = new GridLength(1f, GridUnitType.Auto);
			CategoryHelp.ColumnDefinitions.Add(columnDefinition25);
			ColumnDefinition columnDefinition26 = new ColumnDefinition();
			columnDefinition26.Width = new GridLength(1f, GridUnitType.Auto);
			CategoryHelp.ColumnDefinitions.Add(columnDefinition26);
			ColumnDefinition columnDefinition27 = new ColumnDefinition();
			columnDefinition27.Width = new GridLength(1f, GridUnitType.Auto);
			CategoryHelp.ColumnDefinitions.Add(columnDefinition27);
			ColumnDefinition columnDefinition28 = new ColumnDefinition();
			columnDefinition28.Width = new GridLength(1f, GridUnitType.Auto);
			CategoryHelp.ColumnDefinitions.Add(columnDefinition28);
			ColumnDefinition columnDefinition29 = new ColumnDefinition();
			columnDefinition29.Width = new GridLength(1f, GridUnitType.Auto);
			CategoryHelp.ColumnDefinitions.Add(columnDefinition29);
			ColumnDefinition item22 = new ColumnDefinition();
			CategoryHelp.ColumnDefinitions.Add(item22);
			Grid.SetRow(CategoryHelp, 4);
			e_87 = new TextBlock();
			CategoryHelp.Children.Add(e_87);
			e_87.Name = "e_87";
			e_87.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_87.HorizontalAlignment = HorizontalAlignment.Center;
			e_87.VerticalAlignment = VerticalAlignment.Center;
			e_87.SetResourceReference(TextBlock.TextProperty, "WorkhopBrowser_Help_OpenClose");
			e_88 = new TextBlock();
			CategoryHelp.Children.Add(e_88);
			e_88.Name = "e_88";
			e_88.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_88.HorizontalAlignment = HorizontalAlignment.Center;
			e_88.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_88, 1);
			e_88.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_ToggleCategory");
			e_89 = new TextBlock();
			CategoryHelp.Children.Add(e_89);
			e_89.Name = "e_89";
			e_89.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_89.HorizontalAlignment = HorizontalAlignment.Center;
			e_89.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_89, 3);
			e_89.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Help_Refresh");
			e_90 = new TextBlock();
			CategoryHelp.Children.Add(e_90);
			e_90.Name = "e_90";
			e_90.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_90.HorizontalAlignment = HorizontalAlignment.Center;
			e_90.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_90, 4);
			e_90.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\LoadingIconAnimated.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Bg16x9.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right_highlight.dds");
			FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "MaxWidth", typeof(MyWorkshopBrowserViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "BackgroundOverlay", typeof(MyWorkshopBrowserViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "ExitCommand", typeof(MyWorkshopBrowserViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "RefreshCommand", typeof(MyWorkshopBrowserViewModel_RefreshCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SelectedSortIndex", typeof(MyWorkshopBrowserViewModel_SelectedSortIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "Categories", typeof(MyWorkshopBrowserViewModel_Categories_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SelectedCategoryIndex", typeof(MyWorkshopBrowserViewModel_SelectedCategoryIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "CategoryControlTabIndexRight", typeof(MyWorkshopBrowserViewModel_CategoryControlTabIndexRight_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsRefreshing", typeof(MyWorkshopBrowserViewModel_IsRefreshing_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsWorkshopAggregator", typeof(MyWorkshopBrowserViewModel_IsWorkshopAggregator_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "ServiceCommand", typeof(MyWorkshopBrowserViewModel_ServiceCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "Service0IsChecked", typeof(MyWorkshopBrowserViewModel_Service0IsChecked_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "Service1IsChecked", typeof(MyWorkshopBrowserViewModel_Service1IsChecked_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SearchText", typeof(MyWorkshopBrowserViewModel_SearchText_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SearchControlTabIndexLeft", typeof(MyWorkshopBrowserViewModel_SearchControlTabIndexLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsSearchLabelVisible", typeof(MyWorkshopBrowserViewModel_IsSearchLabelVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsNotFoundTextVisible", typeof(MyWorkshopBrowserViewModel_IsNotFoundTextVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "ToggleSubscriptionCommand", typeof(MyWorkshopBrowserViewModel_ToggleSubscriptionCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "PreviousPageCommand", typeof(MyWorkshopBrowserViewModel_PreviousPageCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "NextPageCommand", typeof(MyWorkshopBrowserViewModel_NextPageCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "BrowseWorkshopCommand", typeof(MyWorkshopBrowserViewModel_BrowseWorkshopCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "OpenItemInWorkshopCommand", typeof(MyWorkshopBrowserViewModel_OpenItemInWorkshopCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "WorkshopItems", typeof(MyWorkshopBrowserViewModel_WorkshopItems_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SelectedWorkshopItem", typeof(MyWorkshopBrowserViewModel_SelectedWorkshopItem_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsDetailVisible", typeof(MyWorkshopBrowserViewModel_IsDetailVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "Rating", typeof(MyWorkshopItemModel_Rating_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "Title", typeof(MyWorkshopItemModel_Title_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "Size", typeof(MyWorkshopItemModel_Size_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "NumSubscriptions", typeof(MyWorkshopItemModel_NumSubscriptions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "TimeCreated", typeof(MyWorkshopItemModel_TimeCreated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "TimeUpdated", typeof(MyWorkshopItemModel_TimeUpdated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopItemModel), "Description", typeof(MyWorkshopItemModel_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsPagingInfoVisible", typeof(MyWorkshopBrowserViewModel_IsPagingInfoVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsQueryFinished", typeof(MyWorkshopBrowserViewModel_IsQueryFinished_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "CurrentPage", typeof(MyWorkshopBrowserViewModel_CurrentPage_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "TotalPages", typeof(MyWorkshopBrowserViewModel_TotalPages_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			object obj = elem.Resources[typeof(ListBoxItem)];
			Style style = new Style(typeof(ListBoxItem), obj as Style);
			Setter item = new Setter(UIElement.MarginProperty, new Thickness(0f, 8f, 8f, 0f));
			style.Setters.Add(item);
			elem.Resources.Add(typeof(ListBoxItem), style);
		}

		private static ObservableCollection<object> Get_e_8_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
			ComboBoxItem comboBoxItem = new ComboBoxItem();
			comboBoxItem.Name = "e_9";
			comboBoxItem.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_MostPopular");
			observableCollection.Add(comboBoxItem);
			ComboBoxItem comboBoxItem2 = new ComboBoxItem();
			comboBoxItem2.Name = "e_10";
			comboBoxItem2.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_MostRecent");
			observableCollection.Add(comboBoxItem2);
			ComboBoxItem comboBoxItem3 = new ComboBoxItem();
			comboBoxItem3.Name = "e_11";
			comboBoxItem3.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_MostSubscribed");
			observableCollection.Add(comboBoxItem3);
			ComboBoxItem comboBoxItem4 = new ComboBoxItem();
			comboBoxItem4.Name = "e_12";
			comboBoxItem4.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_Subscribed");
			observableCollection.Add(comboBoxItem4);
			return observableCollection;
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
			ComboBoxItem comboBoxItem = new ComboBoxItem();
			comboBoxItem.Name = "e_9";
			comboBoxItem.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_MostPopular");
			((Collection<object>)(object)obj).Add((object)comboBoxItem);
			ComboBoxItem comboBoxItem2 = new ComboBoxItem();
			comboBoxItem2.Name = "e_10";
			comboBoxItem2.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_MostRecent");
			((Collection<object>)(object)obj).Add((object)comboBoxItem2);
			ComboBoxItem comboBoxItem3 = new ComboBoxItem();
			comboBoxItem3.Name = "e_11";
			comboBoxItem3.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_MostSubscribed");
			((Collection<object>)(object)obj).Add((object)comboBoxItem3);
			ComboBoxItem comboBoxItem4 = new ComboBoxItem();
			comboBoxItem4.Name = "e_12";
			comboBoxItem4.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_Subscribed");
			((Collection<object>)(object)obj).Add((object)comboBoxItem4);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static UIElement e_13_dtMethod(UIElement parent)
		{
			Grid grid = new Grid();
			grid.Parent = parent;
			grid.Name = "e_14";
			grid.Margin = new Thickness(2f, 2f, 2f, 2f);
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.CButton);
			Binding binding = new Binding("ToggleCategoryCommand");
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding);
			grid.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = grid;
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.Width = new GridLength(1f, GridUnitType.Auto);
			grid.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition item = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item);
			CheckBox checkBox = new CheckBox();
			grid.Children.Add(checkBox);
			checkBox.Name = "e_15";
			checkBox.VerticalAlignment = VerticalAlignment.Center;
			Binding binding2 = new Binding("IsChecked");
			checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding2);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_16";
			textBlock.Margin = new Thickness(2f, 0f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			Binding binding3 = new Binding("LocalizedName");
			textBlock.SetBinding(TextBlock.TextProperty, binding3);
			return grid;
		}

		private static UIElement e_18_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_19",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_20";
			Border border = new Border();
			grid.Children.Add(border);
			border.Name = "PART_Background";
			border.Height = 38f;
			border.Width = 38f;
			border.SetResourceReference(Control.BackgroundProperty, "CheckServiceBack");
			Border border2 = new Border();
			grid.Children.Add(border2);
			border2.Name = "PART_Icon";
			border2.Height = 38f;
			border2.Width = 38f;
			border2.SetResourceReference(Control.BackgroundProperty, "CheckService0");
			ContentPresenter contentPresenter = new ContentPresenter();
			obj.Children.Add(contentPresenter);
			contentPresenter.Name = "e_21";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_22_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_23",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_24";
			Border border = new Border();
			grid.Children.Add(border);
			border.Name = "PART_Background";
			border.Height = 38f;
			border.Width = 38f;
			border.SetResourceReference(Control.BackgroundProperty, "CheckServiceBack");
			Border border2 = new Border();
			grid.Children.Add(border2);
			border2.Name = "PART_Icon";
			border2.Height = 38f;
			border2.Width = 38f;
			border2.SetResourceReference(Control.BackgroundProperty, "CheckService1");
			ContentPresenter contentPresenter = new ContentPresenter();
			obj.Children.Add(contentPresenter);
			contentPresenter.Name = "e_25";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_30_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_31",
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
			UniformGrid uniformGrid = (UniformGrid)(obj.Child = new UniformGrid());
			uniformGrid.Name = "e_32";
			uniformGrid.Margin = new Thickness(5f, 5f, 5f, 5f);
			uniformGrid.IsItemsHost = true;
			uniformGrid.Rows = 3;
			uniformGrid.Columns = 3;
			return obj;
		}

		private static UIElement e_37_iptMethod(UIElement parent)
		{
			return new StackPanel
			{
				Parent = parent,
				Name = "e_38",
				IsItemsHost = true,
				Orientation = Orientation.Horizontal
			};
		}

		private static UIElement e_37_dtMethod(UIElement parent)
		{
			Image obj = new Image
			{
				Parent = parent,
				Name = "e_39",
				Height = 16f,
				Width = 16f,
				Margin = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding(), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_30_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_33",
				Margin = new Thickness(0f, 4f, 0f, 0f)
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
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
			ColumnDefinition item4 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_34";
			image.HorizontalAlignment = HorizontalAlignment.Center;
			image.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Bg16x9.png"
			};
			image.Stretch = Stretch.Uniform;
			Image image2 = new Image();
			obj.Children.Add(image2);
			image2.Name = "e_35";
			image2.HorizontalAlignment = HorizontalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("PreviewImage"), property: Image.SourceProperty);
			CheckBox checkBox = new CheckBox();
			obj.Children.Add(checkBox);
			checkBox.Name = "e_36";
			checkBox.Height = 24f;
			checkBox.Width = 24f;
			checkBox.HorizontalAlignment = HorizontalAlignment.Right;
			checkBox.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(checkBox, 1);
			checkBox.SetBinding(binding: new Binding("IsSubscribed"), property: ToggleButton.IsCheckedProperty);
			checkBox.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Subscribe");
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_37";
			itemsControl.Margin = new Thickness(4f, 2f, 2f, 2f);
			itemsControl.HorizontalAlignment = HorizontalAlignment.Left;
			itemsControl.VerticalAlignment = VerticalAlignment.Center;
			ControlTemplate controlTemplate2 = (itemsControl.ItemsPanel = new ControlTemplate(e_37_iptMethod));
			Func<UIElement, UIElement> createMethod = e_37_dtMethod;
			itemsControl.ItemTemplate = new DataTemplate(createMethod);
			Grid.SetRow(itemsControl, 1);
			itemsControl.SetBinding(binding: new Binding("Rating"), property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_40";
			textBlock.Margin = new Thickness(4f, 0f, 2f, 2f);
			Grid.SetRow(textBlock, 2);
			textBlock.SetBinding(binding: new Binding("Title"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_43_iptMethod(UIElement parent)
		{
			return new StackPanel
			{
				Parent = parent,
				Name = "e_44",
				IsItemsHost = true,
				Orientation = Orientation.Horizontal
			};
		}

		private static UIElement e_43_dtMethod(UIElement parent)
		{
			Image obj = new Image
			{
				Parent = parent,
				Name = "e_45",
				Height = 24f,
				Width = 24f,
				Margin = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding(), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_55_s_S_2_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_56",
				BorderThickness = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding("BorderBrush")
			{
				Source = parent
			}, property: Control.BorderBrushProperty);
			Grid grid = (Grid)(obj.Child = new Grid());
			grid.Name = "e_57";
			RowDefinition item = new RowDefinition();
			grid.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid.ColumnDefinitions.Add(item4);
			Grid grid2 = new Grid();
			grid.Children.Add(grid2);
			grid2.Name = "e_58";
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(4f, GridUnitType.Pixel)
			};
			grid2.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition();
			grid2.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition
			{
				Height = new GridLength(4f, GridUnitType.Pixel)
			};
			grid2.RowDefinitions.Add(item7);
			ColumnDefinition item8 = new ColumnDefinition
			{
				Width = new GridLength(4f, GridUnitType.Pixel)
			};
			grid2.ColumnDefinitions.Add(item8);
			ColumnDefinition item9 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item9);
			ColumnDefinition item10 = new ColumnDefinition
			{
				Width = new GridLength(32f, GridUnitType.Pixel)
			};
			grid2.ColumnDefinitions.Add(item10);
			Grid.SetColumnSpan(grid2, 2);
			Grid.SetRowSpan(grid2, 2);
			Border border = new Border();
			grid2.Children.Add(border);
			border.Name = "e_59";
			border.IsHitTestVisible = false;
			border.SnapsToDevicePixels = false;
			border.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftTop");
			Border border2 = new Border();
			grid2.Children.Add(border2);
			border2.Name = "e_60";
			border2.IsHitTestVisible = false;
			border2.SnapsToDevicePixels = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterTop");
			Border border3 = new Border();
			grid2.Children.Add(border3);
			border3.Name = "e_61";
			border3.IsHitTestVisible = false;
			border3.SnapsToDevicePixels = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightTop");
			Border border4 = new Border();
			grid2.Children.Add(border4);
			border4.Name = "e_62";
			border4.IsHitTestVisible = false;
			border4.SnapsToDevicePixels = false;
			Grid.SetRow(border4, 1);
			border4.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftCenter");
			Border border5 = new Border();
			grid2.Children.Add(border5);
			border5.Name = "e_63";
			border5.IsHitTestVisible = false;
			border5.SnapsToDevicePixels = false;
			Grid.SetColumn(border5, 1);
			Grid.SetRow(border5, 1);
			border5.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenter");
			Border border6 = new Border();
			grid2.Children.Add(border6);
			border6.Name = "e_64";
			border6.IsHitTestVisible = false;
			border6.SnapsToDevicePixels = false;
			Grid.SetColumn(border6, 2);
			Grid.SetRow(border6, 1);
			border6.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightCenter");
			Border border7 = new Border();
			grid2.Children.Add(border7);
			border7.Name = "e_65";
			border7.IsHitTestVisible = false;
			border7.SnapsToDevicePixels = false;
			Grid.SetRow(border7, 2);
			border7.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftBottom");
			Border border8 = new Border();
			grid2.Children.Add(border8);
			border8.Name = "e_66";
			border8.IsHitTestVisible = false;
			border8.SnapsToDevicePixels = false;
			Grid.SetColumn(border8, 1);
			Grid.SetRow(border8, 2);
			border8.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterBottom");
			Border border9 = new Border();
			grid2.Children.Add(border9);
			border9.Name = "e_67";
			border9.IsHitTestVisible = false;
			border9.SnapsToDevicePixels = false;
			Grid.SetColumn(border9, 2);
			Grid.SetRow(border9, 2);
			border9.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightBottom");
			ScrollContentPresenter scrollContentPresenter = new ScrollContentPresenter();
			grid.Children.Add(scrollContentPresenter);
			scrollContentPresenter.Name = "PART_ScrollContentPresenter";
			scrollContentPresenter.SnapsToDevicePixels = true;
			scrollContentPresenter.SetBinding(binding: new Binding("Padding")
			{
				Source = parent
			}, property: UIElement.MarginProperty);
			scrollContentPresenter.SetBinding(binding: new Binding("HorizontalContentAlignment")
			{
				Source = parent
			}, property: UIElement.HorizontalAlignmentProperty);
			scrollContentPresenter.SetBinding(binding: new Binding("VerticalContentAlignment")
			{
				Source = parent
			}, property: UIElement.VerticalAlignmentProperty);
			scrollContentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			ScrollBar scrollBar = new ScrollBar();
			grid.Children.Add(scrollBar);
			scrollBar.Name = "PART_VerticalScrollBar";
			scrollBar.Width = 32f;
			scrollBar.Minimum = 0f;
			scrollBar.Orientation = Orientation.Vertical;
			Grid.SetColumn(scrollBar, 1);
			scrollBar.SetBinding(binding: new Binding("ComputedVerticalScrollBarVisibility")
			{
				Source = parent
			}, property: UIElement.VisibilityProperty);
			scrollBar.SetBinding(binding: new Binding("ViewportHeight")
			{
				Source = parent
			}, property: ScrollBar.ViewportSizeProperty);
			scrollBar.SetBinding(binding: new Binding("ScrollableHeight")
			{
				Source = parent
			}, property: RangeBase.MaximumProperty);
			scrollBar.SetBinding(binding: new Binding("VerticalOffset")
			{
				Source = parent
			}, property: RangeBase.ValueProperty);
			ScrollBar scrollBar2 = new ScrollBar();
			grid.Children.Add(scrollBar2);
			scrollBar2.Name = "PART_HorizontalScrollBar";
			scrollBar2.Height = 32f;
			scrollBar2.Minimum = 0f;
			scrollBar2.Orientation = Orientation.Horizontal;
			Grid.SetRow(scrollBar2, 1);
			scrollBar2.SetBinding(binding: new Binding("ComputedHorizontalScrollBarVisibility")
			{
				Source = parent
			}, property: UIElement.VisibilityProperty);
			scrollBar2.SetBinding(binding: new Binding("ViewportWidth")
			{
				Source = parent
			}, property: ScrollBar.ViewportSizeProperty);
			scrollBar2.SetBinding(binding: new Binding("ScrollableWidth")
			{
				Source = parent
			}, property: RangeBase.MaximumProperty);
			scrollBar2.SetBinding(binding: new Binding("HorizontalOffset")
			{
				Source = parent
			}, property: RangeBase.ValueProperty);
			return obj;
		}
	}
}
