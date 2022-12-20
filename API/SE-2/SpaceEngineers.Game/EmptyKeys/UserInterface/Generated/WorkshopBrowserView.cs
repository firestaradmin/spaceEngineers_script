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
using EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class WorkshopBrowserView : UIRoot
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

		private ImageButton e_17;

		private AnimatedImage e_18;

		private CheckBox e_19;

		private CheckBox e_23;

		private Grid e_27;

		private TextBox e_28;

		private TextBlock e_29;

		private ImageButton e_30;

		private TextBlock e_31;

		private ListBox e_32;

		private Grid e_43;

		private Grid e_44;

		private ItemsControl e_45;

		private TextBlock e_48;

		private TextBlock e_49;

		private TextBlock e_50;

		private TextBlock e_51;

		private TextBlock e_52;

		private TextBlock e_53;

		private TextBlock e_54;

		private TextBlock e_55;

		private TextBlock e_56;

		private ScrollViewer e_57;

		private Border e_70;

		private TextBlock e_71;

		private Grid e_72;

		private ImageButton e_73;

		private StackPanel e_74;

		private TextBlock e_75;

		private TextBlock e_76;

		private TextBlock e_77;

		private ImageButton e_78;

		private Border e_79;

		private Grid e_80;

		private Button e_81;

		private Button e_82;

		public WorkshopBrowserView()
		{
			Initialize();
		}

		public WorkshopBrowserView(int width, int height)
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
			rowDefinition13.Height = new GridLength(1f, GridUnitType.Auto);
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
			GamepadHelp.SetTabIndexRight(e_8, 2);
			GamepadHelp.SetTabIndexDown(e_8, 5);
			Binding binding4 = new Binding("SelectedSortIndex");
			binding4.UseGeneratedBindings = true;
			e_8.SetBinding(Selector.SelectedIndexProperty, binding4);
			e_13 = new ComboBox();
			e_7.Children.Add(e_13);
			e_13.Name = "e_13";
			e_13.Margin = new Thickness(5f, 0f, 0f, 0f);
			e_13.VerticalAlignment = VerticalAlignment.Center;
			e_13.TabIndex = 2;
			Func<UIElement, UIElement> createMethod = e_13_dtMethod;
			e_13.ItemTemplate = new DataTemplate(createMethod);
			e_13.MaxDropDownHeight = 300f;
			Grid.SetColumn(e_13, 1);
			GamepadHelp.SetTabIndexLeft(e_13, 1);
			GamepadHelp.SetTabIndexRight(e_13, 3);
			GamepadHelp.SetTabIndexDown(e_13, 5);
			Binding binding5 = new Binding("Categories");
			binding5.UseGeneratedBindings = true;
			e_13.SetBinding(ItemsControl.ItemsSourceProperty, binding5);
			Binding binding6 = new Binding("SelectedCategoryIndex");
			binding6.UseGeneratedBindings = true;
			e_13.SetBinding(Selector.SelectedIndexProperty, binding6);
			e_17 = new ImageButton();
			e_7.Children.Add(e_17);
			e_17.Name = "e_17";
			e_17.Width = 24f;
			e_17.Margin = new Thickness(4f, 0f, 4f, 0f);
			e_17.VerticalAlignment = VerticalAlignment.Center;
			e_17.TabIndex = 3;
			e_17.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage5 = new BitmapImage();
			bitmapImage5.TextureAsset = "Textures\\GUI\\Icons\\Blueprints\\Refresh.png";
			e_17.ImageNormal = bitmapImage5;
			BitmapImage bitmapImage6 = new BitmapImage();
			bitmapImage6.TextureAsset = "Textures\\GUI\\Icons\\Blueprints\\Refresh.png";
			e_17.ImageHover = bitmapImage6;
			Grid.SetColumn(e_17, 2);
			GamepadHelp.SetTabIndexLeft(e_17, 2);
			GamepadHelp.SetTabIndexDown(e_17, 5);
			Binding binding7 = new Binding("IsQueryFinished");
			binding7.UseGeneratedBindings = true;
			e_17.SetBinding(UIElement.IsEnabledProperty, binding7);
			Binding binding8 = new Binding("RefreshCommand");
			binding8.UseGeneratedBindings = true;
			e_17.SetBinding(Button.CommandProperty, binding8);
			Binding binding9 = new Binding("CategoryControlTabIndexRight");
			binding9.UseGeneratedBindings = true;
			e_17.SetBinding(GamepadHelp.TabIndexRightProperty, binding9);
			e_17.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Refresh");
			e_18 = new AnimatedImage();
			e_7.Children.Add(e_18);
			e_18.Name = "e_18";
			e_18.Width = 24f;
			e_18.HorizontalAlignment = HorizontalAlignment.Center;
			e_18.VerticalAlignment = VerticalAlignment.Center;
			BitmapImage bitmapImage7 = new BitmapImage();
			bitmapImage7.TextureAsset = "Textures\\GUI\\LoadingIconAnimated.png";
			e_18.Source = bitmapImage7;
			e_18.FrameWidth = 128;
			e_18.FrameHeight = 128;
			e_18.FramesPerSecond = 20;
			Grid.SetColumn(e_18, 3);
			Binding binding10 = new Binding("IsRefreshing");
			binding10.UseGeneratedBindings = true;
			e_18.SetBinding(UIElement.VisibilityProperty, binding10);
			e_19 = new CheckBox();
			e_7.Children.Add(e_19);
			e_19.Name = "e_19";
			e_19.IsEnabled = true;
			e_19.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_19.VerticalAlignment = VerticalAlignment.Center;
			Style style = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod2 = e_19_s_S_0_ctMethod;
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
			e_19.Style = style;
			e_19.TabIndex = 11;
			Grid.SetColumn(e_19, 4);
			GamepadHelp.SetTabIndexLeft(e_19, 3);
			GamepadHelp.SetTabIndexRight(e_19, 12);
			GamepadHelp.SetTabIndexDown(e_19, 5);
			Binding binding11 = new Binding("IsWorkshopAggregator");
			binding11.UseGeneratedBindings = true;
			e_19.SetBinding(UIElement.VisibilityProperty, binding11);
			Binding binding12 = new Binding("ServiceCommand");
			binding12.UseGeneratedBindings = true;
			e_19.SetBinding(Button.CommandProperty, binding12);
			Binding binding13 = new Binding("Service0IsChecked");
			binding13.UseGeneratedBindings = true;
			e_19.SetBinding(ToggleButton.IsCheckedProperty, binding13);
			e_19.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Service0");
			e_23 = new CheckBox();
			e_7.Children.Add(e_23);
			e_23.Name = "e_23";
			e_23.IsEnabled = true;
			e_23.Margin = new Thickness(0f, 0f, 0f, 0f);
			e_23.VerticalAlignment = VerticalAlignment.Center;
			Style style2 = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod3 = e_23_s_S_0_ctMethod;
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
			BitmapImage bitmapImage8 = new BitmapImage();
			bitmapImage8.TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png";
			imageBrush.ImageSource = bitmapImage8;
			Setter setter6 = new Setter(Control.BackgroundProperty, imageBrush);
			setter6.TargetName = "PART_Background";
			trigger5.Setters.Add(setter6);
			ImageBrush imageBrush2 = new ImageBrush();
			BitmapImage bitmapImage9 = new BitmapImage();
			bitmapImage9.TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png";
			imageBrush2.ImageSource = bitmapImage9;
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
			e_23.Style = style2;
			e_23.TabIndex = 12;
			Grid.SetColumn(e_23, 5);
			GamepadHelp.SetTabIndexLeft(e_23, 11);
			GamepadHelp.SetTabIndexRight(e_23, 4);
			GamepadHelp.SetTabIndexDown(e_23, 5);
			Binding binding14 = new Binding("IsWorkshopAggregator");
			binding14.UseGeneratedBindings = true;
			e_23.SetBinding(UIElement.VisibilityProperty, binding14);
			Binding binding15 = new Binding("ServiceCommand");
			binding15.UseGeneratedBindings = true;
			e_23.SetBinding(Button.CommandProperty, binding15);
			Binding binding16 = new Binding("Service1IsChecked");
			binding16.UseGeneratedBindings = true;
			e_23.SetBinding(ToggleButton.IsCheckedProperty, binding16);
			e_23.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Service1");
			e_27 = new Grid();
			e_7.Children.Add(e_27);
			e_27.Name = "e_27";
			Grid.SetColumn(e_27, 6);
			e_28 = new TextBox();
			e_27.Children.Add(e_28);
			e_28.Name = "e_28";
			e_28.VerticalAlignment = VerticalAlignment.Center;
			e_28.TabIndex = 4;
			GamepadHelp.SetTabIndexDown(e_28, 5);
			Binding binding17 = new Binding("SearchText");
			binding17.UseGeneratedBindings = true;
			e_28.SetBinding(TextBox.TextProperty, binding17);
			Binding binding18 = new Binding("SearchControlTabIndexLeft");
			binding18.UseGeneratedBindings = true;
			e_28.SetBinding(GamepadHelp.TabIndexLeftProperty, binding18);
			e_29 = new TextBlock();
			e_27.Children.Add(e_29);
			e_29.Name = "e_29";
			e_29.IsHitTestVisible = false;
			e_29.Margin = new Thickness(4f, 4f, 4f, 4f);
			e_29.VerticalAlignment = VerticalAlignment.Center;
			e_29.Foreground = new SolidColorBrush(new ColorW(94, 115, 127, 255));
			Binding binding19 = new Binding("IsSearchLabelVisible");
			binding19.UseGeneratedBindings = true;
			e_29.SetBinding(UIElement.VisibilityProperty, binding19);
			e_29.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Search");
			e_30 = new ImageButton();
			e_27.Children.Add(e_30);
			e_30.Name = "e_30";
			e_30.Height = 24f;
			e_30.Margin = new Thickness(0f, 0f, 4f, 0f);
			e_30.HorizontalAlignment = HorizontalAlignment.Right;
			e_30.VerticalAlignment = VerticalAlignment.Center;
			e_30.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage10 = new BitmapImage();
			bitmapImage10.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			e_30.ImageNormal = bitmapImage10;
			BitmapImage bitmapImage11 = new BitmapImage();
			bitmapImage11.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_30.ImageHover = bitmapImage11;
			BitmapImage bitmapImage12 = new BitmapImage();
			bitmapImage12.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_30.ImagePressed = bitmapImage12;
			Binding binding20 = new Binding("ClearSearchTextCommand");
			binding20.UseGeneratedBindings = true;
			e_30.SetBinding(Button.CommandProperty, binding20);
			e_31 = new TextBlock();
			e_6.Children.Add(e_31);
			e_31.Name = "e_31";
			e_31.Margin = new Thickness(0f, 10f, 10f, 10f);
			e_31.HorizontalAlignment = HorizontalAlignment.Center;
			e_31.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(e_31, 1);
			Grid.SetColumnSpan(e_31, 2);
			Binding binding21 = new Binding("IsNotFoundTextVisible");
			binding21.UseGeneratedBindings = true;
			e_31.SetBinding(UIElement.VisibilityProperty, binding21);
			e_31.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_NotFound");
			e_32 = new ListBox();
			e_6.Children.Add(e_32);
			e_32.Name = "e_32";
			e_32.Margin = new Thickness(0f, 10f, 10f, 10f);
			e_32.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_32.VerticalAlignment = VerticalAlignment.Stretch;
			Style style3 = new Style(typeof(ListBox));
			Setter item7 = new Setter(UIElement.MinHeightProperty, 80f);
			style3.Setters.Add(item7);
			Func<UIElement, UIElement> createMethod4 = e_32_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod4);
			Setter item8 = new Setter(Control.TemplateProperty, value);
			style3.Setters.Add(item8);
			Trigger trigger7 = new Trigger();
			trigger7.Property = UIElement.IsFocusedProperty;
			trigger7.Value = true;
			Setter item9 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger7.Setters.Add(item9);
			style3.Triggers.Add(trigger7);
			e_32.Style = style3;
			e_32.TabIndex = 5;
			Func<UIElement, UIElement> createMethod5 = e_32_dtMethod;
			e_32.ItemTemplate = new DataTemplate(createMethod5);
			Grid.SetRow(e_32, 1);
			GamepadHelp.SetTabIndexRight(e_32, 10);
			GamepadHelp.SetTabIndexUp(e_32, 2);
			GamepadHelp.SetTabIndexDown(e_32, 7);
			Binding binding22 = new Binding("WorkshopItems");
			binding22.UseGeneratedBindings = true;
			e_32.SetBinding(ItemsControl.ItemsSourceProperty, binding22);
			Binding binding23 = new Binding("SelectedWorkshopItem");
			binding23.UseGeneratedBindings = true;
			e_32.SetBinding(Selector.SelectedItemProperty, binding23);
			e_43 = new Grid();
			e_6.Children.Add(e_43);
			e_43.Name = "e_43";
			e_43.Background = new SolidColorBrush(new ColorW(41, 54, 62, 255));
			RowDefinition rowDefinition14 = new RowDefinition();
			rowDefinition14.Height = new GridLength(1f, GridUnitType.Auto);
			e_43.RowDefinitions.Add(rowDefinition14);
			RowDefinition item10 = new RowDefinition();
			e_43.RowDefinitions.Add(item10);
			Grid.SetColumn(e_43, 1);
			Grid.SetRow(e_43, 0);
			Grid.SetRowSpan(e_43, 3);
			Binding binding24 = new Binding("IsDetailVisible");
			binding24.UseGeneratedBindings = true;
			e_43.SetBinding(UIElement.VisibilityProperty, binding24);
			e_44 = new Grid();
			e_43.Children.Add(e_44);
			e_44.Name = "e_44";
			e_44.Margin = new Thickness(10f, 10f, 10f, 10f);
			RowDefinition rowDefinition15 = new RowDefinition();
			rowDefinition15.Height = new GridLength(1f, GridUnitType.Auto);
			e_44.RowDefinitions.Add(rowDefinition15);
			RowDefinition rowDefinition16 = new RowDefinition();
			rowDefinition16.Height = new GridLength(1f, GridUnitType.Auto);
			e_44.RowDefinitions.Add(rowDefinition16);
			RowDefinition rowDefinition17 = new RowDefinition();
			rowDefinition17.Height = new GridLength(1f, GridUnitType.Auto);
			e_44.RowDefinitions.Add(rowDefinition17);
			RowDefinition rowDefinition18 = new RowDefinition();
			rowDefinition18.Height = new GridLength(1f, GridUnitType.Auto);
			e_44.RowDefinitions.Add(rowDefinition18);
			RowDefinition rowDefinition19 = new RowDefinition();
			rowDefinition19.Height = new GridLength(1f, GridUnitType.Auto);
			e_44.RowDefinitions.Add(rowDefinition19);
			RowDefinition rowDefinition20 = new RowDefinition();
			rowDefinition20.Height = new GridLength(1f, GridUnitType.Auto);
			e_44.RowDefinitions.Add(rowDefinition20);
			ColumnDefinition columnDefinition12 = new ColumnDefinition();
			columnDefinition12.Width = new GridLength(1f, GridUnitType.Auto);
			e_44.ColumnDefinitions.Add(columnDefinition12);
			ColumnDefinition item11 = new ColumnDefinition();
			e_44.ColumnDefinitions.Add(item11);
			e_45 = new ItemsControl();
			e_44.Children.Add(e_45);
			e_45.Name = "e_45";
			ControlTemplate itemsPanel = new ControlTemplate(e_45_iptMethod);
			e_45.ItemsPanel = itemsPanel;
			Func<UIElement, UIElement> createMethod6 = e_45_dtMethod;
			e_45.ItemTemplate = new DataTemplate(createMethod6);
			Grid.SetRow(e_45, 0);
			Grid.SetColumnSpan(e_45, 2);
			Binding binding25 = new Binding("SelectedWorkshopItem.Rating");
			binding25.UseGeneratedBindings = true;
			e_45.SetBinding(ItemsControl.ItemsSourceProperty, binding25);
			e_48 = new TextBlock();
			e_44.Children.Add(e_48);
			e_48.Name = "e_48";
			e_48.Margin = new Thickness(0f, 0f, 0f, 10f);
			e_48.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_48.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(e_48, 1);
			Grid.SetColumnSpan(e_48, 2);
			Binding binding26 = new Binding("SelectedWorkshopItem.Title");
			binding26.UseGeneratedBindings = true;
			e_48.SetBinding(TextBlock.TextProperty, binding26);
			e_49 = new TextBlock();
			e_44.Children.Add(e_49);
			e_49.Name = "e_49";
			e_49.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_49.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_49.FontFamily = new FontFamily("InventorySmall");
			e_49.FontSize = 10f;
			Grid.SetRow(e_49, 2);
			e_49.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_FileSize");
			e_50 = new TextBlock();
			e_44.Children.Add(e_50);
			e_50.Name = "e_50";
			e_50.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_50.FontFamily = new FontFamily("InventorySmall");
			e_50.FontSize = 10f;
			Grid.SetColumn(e_50, 1);
			Grid.SetRow(e_50, 2);
			Binding binding27 = new Binding("SelectedWorkshopItem.Size");
			binding27.UseGeneratedBindings = true;
			binding27.StringFormat = "{0:N0} B";
			e_50.SetBinding(TextBlock.TextProperty, binding27);
			e_51 = new TextBlock();
			e_44.Children.Add(e_51);
			e_51.Name = "e_51";
			e_51.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_51.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_51.FontFamily = new FontFamily("InventorySmall");
			e_51.FontSize = 10f;
			Grid.SetRow(e_51, 3);
			e_51.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Subscribers");
			e_52 = new TextBlock();
			e_44.Children.Add(e_52);
			e_52.Name = "e_52";
			e_52.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_52.FontFamily = new FontFamily("InventorySmall");
			e_52.FontSize = 10f;
			Grid.SetColumn(e_52, 1);
			Grid.SetRow(e_52, 3);
			Binding binding28 = new Binding("SelectedWorkshopItem.NumSubscriptions");
			binding28.UseGeneratedBindings = true;
			binding28.StringFormat = "{0:N0}";
			e_52.SetBinding(TextBlock.TextProperty, binding28);
			e_53 = new TextBlock();
			e_44.Children.Add(e_53);
			e_53.Name = "e_53";
			e_53.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_53.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_53.FontFamily = new FontFamily("InventorySmall");
			e_53.FontSize = 10f;
			Grid.SetRow(e_53, 4);
			e_53.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Created");
			e_54 = new TextBlock();
			e_44.Children.Add(e_54);
			e_54.Name = "e_54";
			e_54.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_54.FontFamily = new FontFamily("InventorySmall");
			e_54.FontSize = 10f;
			Grid.SetColumn(e_54, 1);
			Grid.SetRow(e_54, 4);
			Binding binding29 = new Binding("SelectedWorkshopItem.TimeCreated");
			binding29.UseGeneratedBindings = true;
			e_54.SetBinding(TextBlock.TextProperty, binding29);
			e_55 = new TextBlock();
			e_44.Children.Add(e_55);
			e_55.Name = "e_55";
			e_55.Margin = new Thickness(0f, 0f, 0f, 2f);
			e_55.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			e_55.FontFamily = new FontFamily("InventorySmall");
			e_55.FontSize = 10f;
			Grid.SetRow(e_55, 5);
			e_55.SetResourceReference(TextBlock.TextProperty, "WorkshopBrowser_Updated");
			e_56 = new TextBlock();
			e_44.Children.Add(e_56);
			e_56.Name = "e_56";
			e_56.Margin = new Thickness(20f, 0f, 0f, 2f);
			e_56.FontFamily = new FontFamily("InventorySmall");
			e_56.FontSize = 10f;
			Grid.SetColumn(e_56, 1);
			Grid.SetRow(e_56, 5);
			Binding binding30 = new Binding("SelectedWorkshopItem.TimeUpdated");
			binding30.UseGeneratedBindings = true;
			e_56.SetBinding(TextBlock.TextProperty, binding30);
			e_57 = new ScrollViewer();
			e_43.Children.Add(e_57);
			e_57.Name = "e_57";
			e_57.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_57.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_57.VerticalAlignment = VerticalAlignment.Stretch;
			Style style4 = new Style(typeof(ScrollViewer));
			Setter item12 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style4.Setters.Add(item12);
			ImageBrush imageBrush3 = new ImageBrush();
			BitmapImage bitmapImage13 = new BitmapImage();
			bitmapImage13.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush3.ImageSource = bitmapImage13;
			Setter item13 = new Setter(Control.BorderBrushProperty, imageBrush3);
			style4.Setters.Add(item13);
			Func<UIElement, UIElement> createMethod7 = e_57_s_S_2_ctMethod;
			ControlTemplate controlTemplate3 = new ControlTemplate(typeof(ScrollViewer), createMethod7);
			Trigger trigger8 = new Trigger();
			trigger8.Property = UIElement.IsFocusedProperty;
			trigger8.Value = true;
			ImageBrush imageBrush4 = new ImageBrush();
			BitmapImage bitmapImage14 = new BitmapImage();
			bitmapImage14.TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds";
			imageBrush4.ImageSource = bitmapImage14;
			Setter item14 = new Setter(Control.BorderBrushProperty, imageBrush4);
			trigger8.Setters.Add(item14);
			controlTemplate3.Triggers.Add(trigger8);
			Trigger trigger9 = new Trigger();
			trigger9.Property = UIElement.IsFocusedProperty;
			trigger9.Value = false;
			ImageBrush imageBrush5 = new ImageBrush();
			BitmapImage bitmapImage15 = new BitmapImage();
			bitmapImage15.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush5.ImageSource = bitmapImage15;
			Setter item15 = new Setter(Control.BorderBrushProperty, imageBrush5);
			trigger9.Setters.Add(item15);
			controlTemplate3.Triggers.Add(trigger9);
			Setter item16 = new Setter(Control.TemplateProperty, controlTemplate3);
			style4.Setters.Add(item16);
			e_57.Style = style4;
			e_57.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			e_57.VerticalContentAlignment = VerticalAlignment.Stretch;
			e_57.IsTabStop = true;
			e_57.TabIndex = 10;
			Grid.SetRow(e_57, 1);
			GamepadHelp.SetTabIndexLeft(e_57, 5);
			e_70 = new Border();
			e_57.Content = e_70;
			e_70.Name = "e_70";
			e_70.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_70.VerticalAlignment = VerticalAlignment.Stretch;
			e_70.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_71 = new TextBlock();
			e_70.Child = e_71;
			e_71.Name = "e_71";
			e_71.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_71.TextWrapping = TextWrapping.Wrap;
			e_71.FontFamily = new FontFamily("InventorySmall");
			e_71.FontSize = 10f;
			Binding binding31 = new Binding("SelectedWorkshopItem.Description");
			binding31.UseGeneratedBindings = true;
			e_71.SetBinding(TextBlock.TextProperty, binding31);
			e_72 = new Grid();
			e_6.Children.Add(e_72);
			e_72.Name = "e_72";
			ColumnDefinition item17 = new ColumnDefinition();
			e_72.ColumnDefinitions.Add(item17);
			ColumnDefinition columnDefinition13 = new ColumnDefinition();
			columnDefinition13.Width = new GridLength(1f, GridUnitType.Auto);
			e_72.ColumnDefinitions.Add(columnDefinition13);
			ColumnDefinition item18 = new ColumnDefinition();
			e_72.ColumnDefinitions.Add(item18);
			ColumnDefinition columnDefinition14 = new ColumnDefinition();
			columnDefinition14.Width = new GridLength(1f, GridUnitType.Auto);
			e_72.ColumnDefinitions.Add(columnDefinition14);
			ColumnDefinition item19 = new ColumnDefinition();
			e_72.ColumnDefinitions.Add(item19);
			Grid.SetRow(e_72, 2);
			Binding binding32 = new Binding("IsPagingInfoVisible");
			binding32.UseGeneratedBindings = true;
			e_72.SetBinding(UIElement.VisibilityProperty, binding32);
			e_73 = new ImageButton();
			e_72.Children.Add(e_73);
			e_73.Name = "e_73";
			e_73.Width = 16f;
			e_73.HorizontalAlignment = HorizontalAlignment.Right;
			e_73.TabIndex = 6;
			BitmapImage bitmapImage16 = new BitmapImage();
			bitmapImage16.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds";
			e_73.ImageNormal = bitmapImage16;
			BitmapImage bitmapImage17 = new BitmapImage();
			bitmapImage17.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds";
			e_73.ImageDisabled = bitmapImage17;
			BitmapImage bitmapImage18 = new BitmapImage();
			bitmapImage18.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds";
			e_73.ImageHover = bitmapImage18;
			Grid.SetColumn(e_73, 1);
			GamepadHelp.SetTabIndexRight(e_73, 7);
			GamepadHelp.SetTabIndexUp(e_73, 5);
			GamepadHelp.SetTabIndexDown(e_73, 8);
			Binding binding33 = new Binding("IsQueryFinished");
			binding33.UseGeneratedBindings = true;
			e_73.SetBinding(UIElement.IsEnabledProperty, binding33);
			Binding binding34 = new Binding("PreviousPageCommand");
			binding34.UseGeneratedBindings = true;
			e_73.SetBinding(Button.CommandProperty, binding34);
			e_73.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_PreviousPage");
			e_74 = new StackPanel();
			e_72.Children.Add(e_74);
			e_74.Name = "e_74";
			e_74.HorizontalAlignment = HorizontalAlignment.Center;
			e_74.VerticalAlignment = VerticalAlignment.Center;
			e_74.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_74, 2);
			e_75 = new TextBlock();
			e_74.Children.Add(e_75);
			e_75.Name = "e_75";
			Binding binding35 = new Binding("CurrentPage");
			binding35.UseGeneratedBindings = true;
			e_75.SetBinding(TextBlock.TextProperty, binding35);
			e_76 = new TextBlock();
			e_74.Children.Add(e_76);
			e_76.Name = "e_76";
			e_76.Text = "/";
			e_77 = new TextBlock();
			e_74.Children.Add(e_77);
			e_77.Name = "e_77";
			Binding binding36 = new Binding("TotalPages");
			binding36.UseGeneratedBindings = true;
			e_77.SetBinding(TextBlock.TextProperty, binding36);
			e_78 = new ImageButton();
			e_72.Children.Add(e_78);
			e_78.Name = "e_78";
			e_78.Width = 16f;
			e_78.HorizontalAlignment = HorizontalAlignment.Left;
			e_78.TabIndex = 7;
			BitmapImage bitmapImage19 = new BitmapImage();
			bitmapImage19.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds";
			e_78.ImageNormal = bitmapImage19;
			BitmapImage bitmapImage20 = new BitmapImage();
			bitmapImage20.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds";
			e_78.ImageDisabled = bitmapImage20;
			BitmapImage bitmapImage21 = new BitmapImage();
			bitmapImage21.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds";
			e_78.ImageHover = bitmapImage21;
			Grid.SetColumn(e_78, 3);
			GamepadHelp.SetTabIndexLeft(e_78, 6);
			GamepadHelp.SetTabIndexUp(e_78, 5);
			GamepadHelp.SetTabIndexDown(e_78, 8);
			Binding binding37 = new Binding("IsQueryFinished");
			binding37.UseGeneratedBindings = true;
			e_78.SetBinding(UIElement.IsEnabledProperty, binding37);
			Binding binding38 = new Binding("NextPageCommand");
			binding38.UseGeneratedBindings = true;
			e_78.SetBinding(Button.CommandProperty, binding38);
			e_78.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_NextPage");
			e_79 = new Border();
			e_6.Children.Add(e_79);
			e_79.Name = "e_79";
			e_79.Height = 2f;
			e_79.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_79.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(e_79, 3);
			Grid.SetColumnSpan(e_79, 2);
			e_80 = new Grid();
			e_6.Children.Add(e_80);
			e_80.Name = "e_80";
			e_80.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition columnDefinition15 = new ColumnDefinition();
			columnDefinition15.Width = new GridLength(1f, GridUnitType.Auto);
			e_80.ColumnDefinitions.Add(columnDefinition15);
			ColumnDefinition columnDefinition16 = new ColumnDefinition();
			columnDefinition16.Width = new GridLength(1f, GridUnitType.Auto);
			e_80.ColumnDefinitions.Add(columnDefinition16);
			ColumnDefinition item20 = new ColumnDefinition();
			e_80.ColumnDefinitions.Add(item20);
			Grid.SetRow(e_80, 4);
			Grid.SetColumnSpan(e_80, 2);
			e_81 = new Button();
			e_80.Children.Add(e_81);
			e_81.Name = "e_81";
			e_81.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_81.VerticalAlignment = VerticalAlignment.Center;
			e_81.Padding = new Thickness(10f, 0f, 10f, 0f);
			e_81.TabIndex = 8;
			GamepadHelp.SetTabIndexRight(e_81, 9);
			GamepadHelp.SetTabIndexUp(e_81, 6);
			Binding binding39 = new Binding("BrowseWorkshopCommand");
			binding39.UseGeneratedBindings = true;
			e_81.SetBinding(Button.CommandProperty, binding39);
			e_81.SetResourceReference(ContentControl.ContentProperty, "ScreenLoadSubscribedWorldBrowseWorkshop");
			e_82 = new Button();
			e_80.Children.Add(e_82);
			e_82.Name = "e_82";
			e_82.Margin = new Thickness(10f, 10f, 0f, 10f);
			e_82.VerticalAlignment = VerticalAlignment.Center;
			e_82.Padding = new Thickness(10f, 0f, 10f, 0f);
			e_82.TabIndex = 9;
			Grid.SetColumn(e_82, 1);
			GamepadHelp.SetTabIndexLeft(e_82, 8);
			GamepadHelp.SetTabIndexRight(e_82, 6);
			GamepadHelp.SetTabIndexUp(e_82, 6);
			Binding binding40 = new Binding("OpenItemInWorkshopCommand");
			binding40.UseGeneratedBindings = true;
			e_82.SetBinding(Button.CommandProperty, binding40);
			e_82.SetResourceReference(ContentControl.ContentProperty, "WorkshopBrowser_OpenItem");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
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
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SelectedSortIndex", typeof(MyWorkshopBrowserViewModel_SelectedSortIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "Categories", typeof(MyWorkshopBrowserViewModel_Categories_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SelectedCategoryIndex", typeof(MyWorkshopBrowserViewModel_SelectedCategoryIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsQueryFinished", typeof(MyWorkshopBrowserViewModel_IsQueryFinished_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "RefreshCommand", typeof(MyWorkshopBrowserViewModel_RefreshCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "CategoryControlTabIndexRight", typeof(MyWorkshopBrowserViewModel_CategoryControlTabIndexRight_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsRefreshing", typeof(MyWorkshopBrowserViewModel_IsRefreshing_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsWorkshopAggregator", typeof(MyWorkshopBrowserViewModel_IsWorkshopAggregator_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "ServiceCommand", typeof(MyWorkshopBrowserViewModel_ServiceCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "Service0IsChecked", typeof(MyWorkshopBrowserViewModel_Service0IsChecked_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "Service1IsChecked", typeof(MyWorkshopBrowserViewModel_Service1IsChecked_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SearchText", typeof(MyWorkshopBrowserViewModel_SearchText_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "SearchControlTabIndexLeft", typeof(MyWorkshopBrowserViewModel_SearchControlTabIndexLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsSearchLabelVisible", typeof(MyWorkshopBrowserViewModel_IsSearchLabelVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "ClearSearchTextCommand", typeof(MyWorkshopBrowserViewModel_ClearSearchTextCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "IsNotFoundTextVisible", typeof(MyWorkshopBrowserViewModel_IsNotFoundTextVisible_PropertyInfo));
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
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "PreviousPageCommand", typeof(MyWorkshopBrowserViewModel_PreviousPageCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "CurrentPage", typeof(MyWorkshopBrowserViewModel_CurrentPage_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "TotalPages", typeof(MyWorkshopBrowserViewModel_TotalPages_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "NextPageCommand", typeof(MyWorkshopBrowserViewModel_NextPageCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "BrowseWorkshopCommand", typeof(MyWorkshopBrowserViewModel_BrowseWorkshopCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyWorkshopBrowserViewModel), "OpenItemInWorkshopCommand", typeof(MyWorkshopBrowserViewModel_OpenItemInWorkshopCommand_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			object obj = elem.Resources[typeof(ListBoxItem)];
			Style style = new Style(typeof(ListBoxItem), obj as Style);
			Setter item = new Setter(UIElement.MarginProperty, new Thickness(0f, 4f, 4f, 0f));
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
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_14",
				Margin = new Thickness(2f, 2f, 2f, 2f)
			};
			ColumnDefinition item = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			obj.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item2);
			CheckBox checkBox = new CheckBox();
			obj.Children.Add(checkBox);
			checkBox.Name = "e_15";
			checkBox.VerticalAlignment = VerticalAlignment.Center;
			checkBox.SetBinding(binding: new Binding("IsChecked"), property: ToggleButton.IsCheckedProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_16";
			textBlock.Margin = new Thickness(2f, 0f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("LocalizedName"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_19_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_20",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_21";
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
			contentPresenter.Name = "e_22";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_23_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_24",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_25";
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
			contentPresenter.Name = "e_26";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_32_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_33",
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
			uniformGrid.Name = "e_34";
			uniformGrid.Margin = new Thickness(5f, 5f, 5f, 5f);
			uniformGrid.IsItemsHost = true;
			uniformGrid.Rows = 3;
			uniformGrid.Columns = 3;
			return obj;
		}

		private static UIElement e_39_iptMethod(UIElement parent)
		{
			return new StackPanel
			{
				Parent = parent,
				Name = "e_40",
				IsItemsHost = true,
				Orientation = Orientation.Horizontal
			};
		}

		private static UIElement e_39_dtMethod(UIElement parent)
		{
			Image obj = new Image
			{
				Parent = parent,
				Name = "e_41",
				Height = 16f,
				Width = 16f,
				Margin = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding(), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_32_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_35",
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
			image.Name = "e_36";
			image.HorizontalAlignment = HorizontalAlignment.Center;
			image.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Bg16x9.png"
			};
			image.Stretch = Stretch.Uniform;
			Image image2 = new Image();
			obj.Children.Add(image2);
			image2.Name = "e_37";
			image2.HorizontalAlignment = HorizontalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("PreviewImage"), property: Image.SourceProperty);
			CheckBox checkBox = new CheckBox();
			obj.Children.Add(checkBox);
			checkBox.Name = "e_38";
			checkBox.Height = 24f;
			checkBox.Width = 24f;
			checkBox.HorizontalAlignment = HorizontalAlignment.Right;
			checkBox.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(checkBox, 1);
			checkBox.SetBinding(binding: new Binding("IsSubscribed"), property: ToggleButton.IsCheckedProperty);
			checkBox.SetResourceReference(UIElement.ToolTipProperty, "WorkshopBrowser_Subscribe");
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_39";
			itemsControl.Margin = new Thickness(4f, 2f, 2f, 2f);
			itemsControl.HorizontalAlignment = HorizontalAlignment.Left;
			itemsControl.VerticalAlignment = VerticalAlignment.Center;
			ControlTemplate controlTemplate2 = (itemsControl.ItemsPanel = new ControlTemplate(e_39_iptMethod));
			Func<UIElement, UIElement> createMethod = e_39_dtMethod;
			itemsControl.ItemTemplate = new DataTemplate(createMethod);
			Grid.SetRow(itemsControl, 1);
			itemsControl.SetBinding(binding: new Binding("Rating"), property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_42";
			textBlock.Margin = new Thickness(4f, 0f, 2f, 2f);
			Grid.SetRow(textBlock, 2);
			textBlock.SetBinding(binding: new Binding("Title"), property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement e_45_iptMethod(UIElement parent)
		{
			return new StackPanel
			{
				Parent = parent,
				Name = "e_46",
				IsItemsHost = true,
				Orientation = Orientation.Horizontal
			};
		}

		private static UIElement e_45_dtMethod(UIElement parent)
		{
			Image obj = new Image
			{
				Parent = parent,
				Name = "e_47",
				Height = 24f,
				Width = 24f,
				Margin = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding(), property: Image.SourceProperty);
			return obj;
		}

		private static UIElement e_57_s_S_2_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_58",
				BorderThickness = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding("BorderBrush")
			{
				Source = parent
			}, property: Control.BorderBrushProperty);
			Grid grid = (Grid)(obj.Child = new Grid());
			grid.Name = "e_59";
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
			grid2.Name = "e_60";
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
			border.Name = "e_61";
			border.IsHitTestVisible = false;
			border.SnapsToDevicePixels = false;
			border.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftTop");
			Border border2 = new Border();
			grid2.Children.Add(border2);
			border2.Name = "e_62";
			border2.IsHitTestVisible = false;
			border2.SnapsToDevicePixels = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterTop");
			Border border3 = new Border();
			grid2.Children.Add(border3);
			border3.Name = "e_63";
			border3.IsHitTestVisible = false;
			border3.SnapsToDevicePixels = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightTop");
			Border border4 = new Border();
			grid2.Children.Add(border4);
			border4.Name = "e_64";
			border4.IsHitTestVisible = false;
			border4.SnapsToDevicePixels = false;
			Grid.SetRow(border4, 1);
			border4.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftCenter");
			Border border5 = new Border();
			grid2.Children.Add(border5);
			border5.Name = "e_65";
			border5.IsHitTestVisible = false;
			border5.SnapsToDevicePixels = false;
			Grid.SetColumn(border5, 1);
			Grid.SetRow(border5, 1);
			border5.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenter");
			Border border6 = new Border();
			grid2.Children.Add(border6);
			border6.Name = "e_66";
			border6.IsHitTestVisible = false;
			border6.SnapsToDevicePixels = false;
			Grid.SetColumn(border6, 2);
			Grid.SetRow(border6, 1);
			border6.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightCenter");
			Border border7 = new Border();
			grid2.Children.Add(border7);
			border7.Name = "e_67";
			border7.IsHitTestVisible = false;
			border7.SnapsToDevicePixels = false;
			Grid.SetRow(border7, 2);
			border7.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftBottom");
			Border border8 = new Border();
			grid2.Children.Add(border8);
			border8.Name = "e_68";
			border8.IsHitTestVisible = false;
			border8.SnapsToDevicePixels = false;
			Grid.SetColumn(border8, 1);
			Grid.SetRow(border8, 2);
			border8.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterBottom");
			Border border9 = new Border();
			grid2.Children.Add(border9);
			border9.Name = "e_69";
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
