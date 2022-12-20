using System.CodeDom.Compiler;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ContractsBlockView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class ContractsBlockView_Gamepad : UIRoot
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

		private Grid e_106;

		private Grid e_107;

		private Grid e_108;

		private Image e_109;

		private Grid e_110;

		private TextBlock e_111;

		private Border e_112;

		private TextBlock e_113;

		private ComboBox e_114;

		private Grid AdminSelectionHelp;

		private TextBlock e_115;

		private TextBlock e_116;

		private TextBlock e_117;

		private ImageButton e_118;

		private Grid e_119;

		private Grid e_120;

		private Grid e_121;

		private Image e_122;

		private Grid e_123;

		private TextBlock e_124;

		private Border e_125;

		private TextBlock e_126;

		private ComboBox e_127;

		private Grid GridSelectionHelp;

		private TextBlock e_128;

		private TextBlock e_129;

		private TextBlock e_130;

		private ImageButton e_131;

		public ContractsBlockView_Gamepad()
		{
			Initialize();
		}

		public ContractsBlockView_Gamepad(int width, int height)
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
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionContracts");
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
			e_106 = new Grid();
			e_1.Children.Add(e_106);
			e_106.Name = "e_106";
			e_106.HorizontalAlignment = HorizontalAlignment.Center;
			e_106.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_106, 1);
			Grid.SetRow(e_106, 3);
			Binding binding4 = new Binding("AdministrationViewModel");
			binding4.UseGeneratedBindings = true;
			e_106.SetBinding(UIElement.DataContextProperty, binding4);
			e_107 = new Grid();
			e_106.Children.Add(e_107);
			e_107.Name = "e_107";
			e_108 = new Grid();
			e_107.Children.Add(e_108);
			e_108.Name = "e_108";
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.BButton);
			Binding binding5 = new Binding("AdminSelectionExitCommand");
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding5);
			e_108.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = e_108;
			GamepadBinding gamepadBinding2 = new GamepadBinding();
			gamepadBinding2.Gesture = new GamepadGesture(GamepadInput.CButton);
			Binding binding6 = new Binding("AdminSelectionConfirmCommand");
			gamepadBinding2.SetBinding(InputBinding.CommandProperty, binding6);
			e_108.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = e_108;
			KeyBinding keyBinding = new KeyBinding();
			keyBinding.Gesture = new KeyGesture(KeyCode.Escape, ModifierKeys.None, "");
			Binding binding7 = new Binding("AdminSelectionExitCommand");
			keyBinding.SetBinding(InputBinding.CommandProperty, binding7);
			e_108.InputBindings.Add(keyBinding);
			keyBinding.Parent = e_108;
			RowDefinition rowDefinition10 = new RowDefinition();
			rowDefinition10.Height = new GridLength(0.5f, GridUnitType.Star);
			e_108.RowDefinitions.Add(rowDefinition10);
			RowDefinition rowDefinition11 = new RowDefinition();
			rowDefinition11.Height = new GridLength(2f, GridUnitType.Star);
			e_108.RowDefinitions.Add(rowDefinition11);
			RowDefinition rowDefinition12 = new RowDefinition();
			rowDefinition12.Height = new GridLength(1f, GridUnitType.Star);
			e_108.RowDefinitions.Add(rowDefinition12);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(1f, GridUnitType.Star);
			e_108.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(3f, GridUnitType.Star);
			e_108.ColumnDefinitions.Add(columnDefinition5);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(1f, GridUnitType.Star);
			e_108.ColumnDefinitions.Add(columnDefinition6);
			Binding binding8 = new Binding("IsVisibleAdminSelection");
			e_108.SetBinding(UIElement.VisibilityProperty, binding8);
			e_109 = new Image();
			e_108.Children.Add(e_109);
			e_109.Name = "e_109";
			BitmapImage bitmapImage5 = new BitmapImage();
			bitmapImage5.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			e_109.Source = bitmapImage5;
			e_109.Stretch = Stretch.Fill;
			Grid.SetColumn(e_109, 1);
			Grid.SetRow(e_109, 1);
			Binding binding9 = new Binding("BackgroundOverlay");
			e_109.SetBinding(ImageBrush.ColorOverlayProperty, binding9);
			e_110 = new Grid();
			e_108.Children.Add(e_110);
			e_110.Name = "e_110";
			RowDefinition rowDefinition13 = new RowDefinition();
			rowDefinition13.Height = new GridLength(1f, GridUnitType.Auto);
			e_110.RowDefinitions.Add(rowDefinition13);
			RowDefinition rowDefinition14 = new RowDefinition();
			rowDefinition14.Height = new GridLength(1f, GridUnitType.Auto);
			e_110.RowDefinitions.Add(rowDefinition14);
			RowDefinition rowDefinition15 = new RowDefinition();
			rowDefinition15.Height = new GridLength(1f, GridUnitType.Auto);
			e_110.RowDefinitions.Add(rowDefinition15);
			RowDefinition rowDefinition16 = new RowDefinition();
			rowDefinition16.Height = new GridLength(2.5f, GridUnitType.Star);
			e_110.RowDefinitions.Add(rowDefinition16);
			RowDefinition rowDefinition17 = new RowDefinition();
			rowDefinition17.Height = new GridLength(1f, GridUnitType.Auto);
			e_110.RowDefinitions.Add(rowDefinition17);
			RowDefinition rowDefinition18 = new RowDefinition();
			rowDefinition18.Height = new GridLength(1f, GridUnitType.Auto);
			e_110.RowDefinitions.Add(rowDefinition18);
			RowDefinition rowDefinition19 = new RowDefinition();
			rowDefinition19.Height = new GridLength(1f, GridUnitType.Star);
			e_110.RowDefinitions.Add(rowDefinition19);
			ColumnDefinition columnDefinition7 = new ColumnDefinition();
			columnDefinition7.Width = new GridLength(1f, GridUnitType.Star);
			e_110.ColumnDefinitions.Add(columnDefinition7);
			ColumnDefinition columnDefinition8 = new ColumnDefinition();
			columnDefinition8.Width = new GridLength(12f, GridUnitType.Star);
			e_110.ColumnDefinitions.Add(columnDefinition8);
			ColumnDefinition columnDefinition9 = new ColumnDefinition();
			columnDefinition9.Width = new GridLength(1f, GridUnitType.Auto);
			e_110.ColumnDefinitions.Add(columnDefinition9);
			Grid.SetColumn(e_110, 1);
			Grid.SetRow(e_110, 1);
			e_111 = new TextBlock();
			e_110.Children.Add(e_111);
			e_111.Name = "e_111";
			e_111.Margin = new Thickness(2f, 2f, 2f, 2f);
			e_111.HorizontalAlignment = HorizontalAlignment.Center;
			e_111.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_111, 1);
			Grid.SetRow(e_111, 1);
			Binding binding10 = new Binding("AdminSelectionCaption");
			e_111.SetBinding(TextBlock.TextProperty, binding10);
			e_112 = new Border();
			e_110.Children.Add(e_112);
			e_112.Name = "e_112";
			e_112.Height = 2f;
			e_112.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_112.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_112, 1);
			Grid.SetRow(e_112, 2);
			e_113 = new TextBlock();
			e_110.Children.Add(e_113);
			e_113.Name = "e_113";
			e_113.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_113.HorizontalAlignment = HorizontalAlignment.Left;
			e_113.VerticalAlignment = VerticalAlignment.Center;
			e_113.TextWrapping = TextWrapping.Wrap;
			Grid.SetColumn(e_113, 1);
			Grid.SetRow(e_113, 3);
			Binding binding11 = new Binding("AdminSelectionText");
			e_113.SetBinding(TextBlock.TextProperty, binding11);
			e_114 = new ComboBox();
			e_110.Children.Add(e_114);
			e_114.Name = "e_114";
			e_114.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_114.TabIndex = 200;
			Grid.SetColumn(e_114, 1);
			Grid.SetRow(e_114, 4);
			Binding binding12 = new Binding("AdminSelectionItems");
			e_114.SetBinding(ItemsControl.ItemsSourceProperty, binding12);
			Binding binding13 = new Binding("AdminSelectedItemIndex");
			e_114.SetBinding(Selector.SelectedIndexProperty, binding13);
			AdminSelectionHelp = new Grid();
			e_110.Children.Add(AdminSelectionHelp);
			AdminSelectionHelp.Name = "AdminSelectionHelp";
			AdminSelectionHelp.Margin = new Thickness(10f, 10f, 10f, 10f);
			AdminSelectionHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition10 = new ColumnDefinition();
			columnDefinition10.Width = new GridLength(1f, GridUnitType.Auto);
			AdminSelectionHelp.ColumnDefinitions.Add(columnDefinition10);
			ColumnDefinition columnDefinition11 = new ColumnDefinition();
			columnDefinition11.Width = new GridLength(1f, GridUnitType.Auto);
			AdminSelectionHelp.ColumnDefinitions.Add(columnDefinition11);
			ColumnDefinition columnDefinition12 = new ColumnDefinition();
			columnDefinition12.Width = new GridLength(1f, GridUnitType.Auto);
			AdminSelectionHelp.ColumnDefinitions.Add(columnDefinition12);
			ColumnDefinition columnDefinition13 = new ColumnDefinition();
			columnDefinition13.Width = new GridLength(1f, GridUnitType.Auto);
			AdminSelectionHelp.ColumnDefinitions.Add(columnDefinition13);
			ColumnDefinition item3 = new ColumnDefinition();
			AdminSelectionHelp.ColumnDefinitions.Add(item3);
			Grid.SetColumn(AdminSelectionHelp, 1);
			Grid.SetRow(AdminSelectionHelp, 5);
			e_115 = new TextBlock();
			AdminSelectionHelp.Children.Add(e_115);
			e_115.Name = "e_115";
			e_115.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_115.HorizontalAlignment = HorizontalAlignment.Center;
			e_115.VerticalAlignment = VerticalAlignment.Center;
			e_115.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Select");
			e_116 = new TextBlock();
			AdminSelectionHelp.Children.Add(e_116);
			e_116.Name = "e_116";
			e_116.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_116.HorizontalAlignment = HorizontalAlignment.Center;
			e_116.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_116, 1);
			e_116.SetResourceReference(TextBlock.TextProperty, "ContractsScreenGridSelection_Help_Confirm");
			e_117 = new TextBlock();
			AdminSelectionHelp.Children.Add(e_117);
			e_117.Name = "e_117";
			e_117.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_117.HorizontalAlignment = HorizontalAlignment.Center;
			e_117.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_117, 3);
			e_117.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			e_118 = new ImageButton();
			e_110.Children.Add(e_118);
			e_118.Name = "e_118";
			e_118.Height = 24f;
			e_118.Width = 24f;
			e_118.Margin = new Thickness(16f, 16f, 16f, 16f);
			e_118.HorizontalAlignment = HorizontalAlignment.Right;
			e_118.VerticalAlignment = VerticalAlignment.Center;
			e_118.IsTabStop = false;
			e_118.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage6 = new BitmapImage();
			bitmapImage6.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			e_118.ImageNormal = bitmapImage6;
			BitmapImage bitmapImage7 = new BitmapImage();
			bitmapImage7.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_118.ImageHover = bitmapImage7;
			BitmapImage bitmapImage8 = new BitmapImage();
			bitmapImage8.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_118.ImagePressed = bitmapImage8;
			Grid.SetColumn(e_118, 2);
			Binding binding14 = new Binding("AdminSelectionExitCommand");
			e_118.SetBinding(Button.CommandProperty, binding14);
			e_119 = new Grid();
			e_1.Children.Add(e_119);
			e_119.Name = "e_119";
			e_119.HorizontalAlignment = HorizontalAlignment.Center;
			e_119.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_119, 1);
			Grid.SetRow(e_119, 3);
			Binding binding15 = new Binding("ActiveViewModel");
			binding15.UseGeneratedBindings = true;
			e_119.SetBinding(UIElement.DataContextProperty, binding15);
			e_120 = new Grid();
			e_119.Children.Add(e_120);
			e_120.Name = "e_120";
			e_121 = new Grid();
			e_120.Children.Add(e_121);
			e_121.Name = "e_121";
			GamepadBinding gamepadBinding3 = new GamepadBinding();
			gamepadBinding3.Gesture = new GamepadGesture(GamepadInput.BButton);
			Binding binding16 = new Binding("ExitGridSelectionCommand");
			gamepadBinding3.SetBinding(InputBinding.CommandProperty, binding16);
			e_121.InputBindings.Add(gamepadBinding3);
			gamepadBinding3.Parent = e_121;
			GamepadBinding gamepadBinding4 = new GamepadBinding();
			gamepadBinding4.Gesture = new GamepadGesture(GamepadInput.CButton);
			Binding binding17 = new Binding("ConfirmGridSelectionCommand");
			gamepadBinding4.SetBinding(InputBinding.CommandProperty, binding17);
			e_121.InputBindings.Add(gamepadBinding4);
			gamepadBinding4.Parent = e_121;
			KeyBinding keyBinding2 = new KeyBinding();
			keyBinding2.Gesture = new KeyGesture(KeyCode.Escape, ModifierKeys.None, "");
			Binding binding18 = new Binding("ExitGridSelectionCommand");
			keyBinding2.SetBinding(InputBinding.CommandProperty, binding18);
			e_121.InputBindings.Add(keyBinding2);
			keyBinding2.Parent = e_121;
			RowDefinition rowDefinition20 = new RowDefinition();
			rowDefinition20.Height = new GridLength(0.5f, GridUnitType.Star);
			e_121.RowDefinitions.Add(rowDefinition20);
			RowDefinition rowDefinition21 = new RowDefinition();
			rowDefinition21.Height = new GridLength(2f, GridUnitType.Star);
			e_121.RowDefinitions.Add(rowDefinition21);
			RowDefinition rowDefinition22 = new RowDefinition();
			rowDefinition22.Height = new GridLength(1f, GridUnitType.Star);
			e_121.RowDefinitions.Add(rowDefinition22);
			ColumnDefinition columnDefinition14 = new ColumnDefinition();
			columnDefinition14.Width = new GridLength(1f, GridUnitType.Star);
			e_121.ColumnDefinitions.Add(columnDefinition14);
			ColumnDefinition columnDefinition15 = new ColumnDefinition();
			columnDefinition15.Width = new GridLength(3f, GridUnitType.Star);
			e_121.ColumnDefinitions.Add(columnDefinition15);
			ColumnDefinition columnDefinition16 = new ColumnDefinition();
			columnDefinition16.Width = new GridLength(1f, GridUnitType.Star);
			e_121.ColumnDefinitions.Add(columnDefinition16);
			Binding binding19 = new Binding("IsVisibleGridSelection");
			e_121.SetBinding(UIElement.VisibilityProperty, binding19);
			e_122 = new Image();
			e_121.Children.Add(e_122);
			e_122.Name = "e_122";
			BitmapImage bitmapImage9 = new BitmapImage();
			bitmapImage9.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			e_122.Source = bitmapImage9;
			e_122.Stretch = Stretch.Fill;
			Grid.SetColumn(e_122, 1);
			Grid.SetRow(e_122, 1);
			ImageBrush.SetColorOverlay(e_122, new ColorW(255, 255, 255, 255));
			e_123 = new Grid();
			e_121.Children.Add(e_123);
			e_123.Name = "e_123";
			RowDefinition rowDefinition23 = new RowDefinition();
			rowDefinition23.Height = new GridLength(1f, GridUnitType.Auto);
			e_123.RowDefinitions.Add(rowDefinition23);
			RowDefinition rowDefinition24 = new RowDefinition();
			rowDefinition24.Height = new GridLength(1f, GridUnitType.Auto);
			e_123.RowDefinitions.Add(rowDefinition24);
			RowDefinition rowDefinition25 = new RowDefinition();
			rowDefinition25.Height = new GridLength(1f, GridUnitType.Auto);
			e_123.RowDefinitions.Add(rowDefinition25);
			RowDefinition rowDefinition26 = new RowDefinition();
			rowDefinition26.Height = new GridLength(2.5f, GridUnitType.Star);
			e_123.RowDefinitions.Add(rowDefinition26);
			RowDefinition rowDefinition27 = new RowDefinition();
			rowDefinition27.Height = new GridLength(1f, GridUnitType.Auto);
			e_123.RowDefinitions.Add(rowDefinition27);
			RowDefinition rowDefinition28 = new RowDefinition();
			rowDefinition28.Height = new GridLength(1f, GridUnitType.Auto);
			e_123.RowDefinitions.Add(rowDefinition28);
			RowDefinition rowDefinition29 = new RowDefinition();
			rowDefinition29.Height = new GridLength(1f, GridUnitType.Star);
			e_123.RowDefinitions.Add(rowDefinition29);
			ColumnDefinition columnDefinition17 = new ColumnDefinition();
			columnDefinition17.Width = new GridLength(1f, GridUnitType.Star);
			e_123.ColumnDefinitions.Add(columnDefinition17);
			ColumnDefinition columnDefinition18 = new ColumnDefinition();
			columnDefinition18.Width = new GridLength(12f, GridUnitType.Star);
			e_123.ColumnDefinitions.Add(columnDefinition18);
			ColumnDefinition columnDefinition19 = new ColumnDefinition();
			columnDefinition19.Width = new GridLength(1f, GridUnitType.Auto);
			e_123.ColumnDefinitions.Add(columnDefinition19);
			Grid.SetColumn(e_123, 1);
			Grid.SetRow(e_123, 1);
			e_124 = new TextBlock();
			e_123.Children.Add(e_124);
			e_124.Name = "e_124";
			e_124.Margin = new Thickness(2f, 2f, 2f, 2f);
			e_124.HorizontalAlignment = HorizontalAlignment.Center;
			e_124.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_124, 1);
			Grid.SetRow(e_124, 1);
			e_124.SetResourceReference(TextBlock.TextProperty, "ContractScreen_GridSelection_Caption");
			e_125 = new Border();
			e_123.Children.Add(e_125);
			e_125.Name = "e_125";
			e_125.Height = 2f;
			e_125.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_125.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(e_125, 1);
			Grid.SetRow(e_125, 2);
			e_126 = new TextBlock();
			e_123.Children.Add(e_126);
			e_126.Name = "e_126";
			e_126.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_126.HorizontalAlignment = HorizontalAlignment.Left;
			e_126.VerticalAlignment = VerticalAlignment.Center;
			e_126.TextWrapping = TextWrapping.Wrap;
			Grid.SetColumn(e_126, 1);
			Grid.SetRow(e_126, 3);
			e_126.SetResourceReference(TextBlock.TextProperty, "ContractScreen_GridSelection_Text");
			e_127 = new ComboBox();
			e_123.Children.Add(e_127);
			e_127.Name = "e_127";
			e_127.Width = float.NaN;
			e_127.Margin = new Thickness(10f, 10f, 10f, 10f);
			e_127.HorizontalAlignment = HorizontalAlignment.Stretch;
			e_127.TabIndex = 500;
			Grid.SetColumn(e_127, 1);
			Grid.SetRow(e_127, 4);
			Binding binding20 = new Binding("SelectableTargets");
			e_127.SetBinding(ItemsControl.ItemsSourceProperty, binding20);
			Binding binding21 = new Binding("SelectedTargetIndex");
			e_127.SetBinding(Selector.SelectedIndexProperty, binding21);
			GridSelectionHelp = new Grid();
			e_123.Children.Add(GridSelectionHelp);
			GridSelectionHelp.Name = "GridSelectionHelp";
			GridSelectionHelp.Margin = new Thickness(10f, 10f, 10f, 10f);
			GridSelectionHelp.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition columnDefinition20 = new ColumnDefinition();
			columnDefinition20.Width = new GridLength(1f, GridUnitType.Auto);
			GridSelectionHelp.ColumnDefinitions.Add(columnDefinition20);
			ColumnDefinition columnDefinition21 = new ColumnDefinition();
			columnDefinition21.Width = new GridLength(1f, GridUnitType.Auto);
			GridSelectionHelp.ColumnDefinitions.Add(columnDefinition21);
			ColumnDefinition columnDefinition22 = new ColumnDefinition();
			columnDefinition22.Width = new GridLength(1f, GridUnitType.Auto);
			GridSelectionHelp.ColumnDefinitions.Add(columnDefinition22);
			ColumnDefinition columnDefinition23 = new ColumnDefinition();
			columnDefinition23.Width = new GridLength(1f, GridUnitType.Auto);
			GridSelectionHelp.ColumnDefinitions.Add(columnDefinition23);
			ColumnDefinition item4 = new ColumnDefinition();
			GridSelectionHelp.ColumnDefinitions.Add(item4);
			Grid.SetColumn(GridSelectionHelp, 1);
			Grid.SetRow(GridSelectionHelp, 5);
			e_128 = new TextBlock();
			GridSelectionHelp.Children.Add(e_128);
			e_128.Name = "e_128";
			e_128.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_128.HorizontalAlignment = HorizontalAlignment.Center;
			e_128.VerticalAlignment = VerticalAlignment.Center;
			e_128.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Select");
			e_129 = new TextBlock();
			GridSelectionHelp.Children.Add(e_129);
			e_129.Name = "e_129";
			e_129.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_129.HorizontalAlignment = HorizontalAlignment.Center;
			e_129.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_129, 1);
			e_129.SetResourceReference(TextBlock.TextProperty, "ContractsScreenGridSelection_Help_Confirm");
			e_130 = new TextBlock();
			GridSelectionHelp.Children.Add(e_130);
			e_130.Name = "e_130";
			e_130.Margin = new Thickness(10f, 0f, 5f, 0f);
			e_130.HorizontalAlignment = HorizontalAlignment.Center;
			e_130.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(e_130, 3);
			e_130.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			e_131 = new ImageButton();
			e_123.Children.Add(e_131);
			e_131.Name = "e_131";
			e_131.Height = 24f;
			e_131.Width = 24f;
			e_131.Margin = new Thickness(16f, 16f, 16f, 16f);
			e_131.HorizontalAlignment = HorizontalAlignment.Right;
			e_131.VerticalAlignment = VerticalAlignment.Center;
			e_131.IsTabStop = false;
			e_131.ImageStretch = Stretch.Uniform;
			BitmapImage bitmapImage10 = new BitmapImage();
			bitmapImage10.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			e_131.ImageNormal = bitmapImage10;
			BitmapImage bitmapImage11 = new BitmapImage();
			bitmapImage11.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_131.ImageHover = bitmapImage11;
			BitmapImage bitmapImage12 = new BitmapImage();
			bitmapImage12.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			e_131.ImagePressed = bitmapImage12;
			Grid.SetColumn(e_131, 2);
			Binding binding22 = new Binding("ExitGridSelectionCommand");
			e_131.SetBinding(Button.CommandProperty, binding22);
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			FontManager.Instance.AddFont("LargeFont", 16.6f, FontStyle.Regular, "LargeFont_12.45_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "MaxWidth", typeof(MyContractsBlockViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "BackgroundOverlay", typeof(MyContractsBlockViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "ExitCommand", typeof(MyContractsBlockViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "RefreshAvailableCommand", typeof(MyContractsBlockViewModel_RefreshAvailableCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "FilterTargets", typeof(MyContractsBlockViewModel_FilterTargets_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "FilterTargetIndex", typeof(MyContractsBlockViewModel_FilterTargetIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "SelectedFilterTarget", typeof(MyContractsBlockViewModel_SelectedFilterTarget_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "AcceptCommand", typeof(MyContractsBlockViewModel_AcceptCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "AvailableContracts", typeof(MyContractsBlockViewModel_AvailableContracts_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "SelectedAvailableContractIndex", typeof(MyContractsBlockViewModel_SelectedAvailableContractIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "SelectedAvailableContract", typeof(MyContractsBlockViewModel_SelectedAvailableContract_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "IsNoAvailableContractVisible", typeof(MyContractsBlockViewModel_IsNoAvailableContractVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "IsAcceptEnabled", typeof(MyContractsBlockViewModel_IsAcceptEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "ActiveViewModel", typeof(MyContractsBlockViewModel_ActiveViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "IsAdministrationVisible", typeof(MyContractsBlockViewModel_IsAdministrationVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "AdministrationViewModel", typeof(MyContractsBlockViewModel_AdministrationViewModel_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesContracts.Instance);
		}

		private static ObservableCollection<object> Get_e_8_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem = new TabItem();
			tabItem.Name = "e_9";
			tabItem.IsTabStop = false;
			tabItem.SetResourceReference(HeaderedContentControl.HeaderProperty, "ContractScreen_Tab_AvailableContracts");
			Grid grid2 = (Grid)(tabItem.Content = new Grid());
			grid2.Name = "e_10";
			GamepadBinding gamepadBinding = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding.SetBinding(binding: new Binding("RefreshAvailableCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			grid2.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = grid2;
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
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid2.ColumnDefinitions.Add(item5);
			Grid grid3 = new Grid();
			grid2.Children.Add(grid3);
			grid3.Name = "e_11";
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition();
			grid3.RowDefinitions.Add(item7);
			Grid grid4 = new Grid();
			grid3.Children.Add(grid4);
			grid4.Name = "e_12";
			ColumnDefinition item8 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item8);
			ColumnDefinition item9 = new ColumnDefinition();
			grid4.ColumnDefinitions.Add(item9);
			TextBlock textBlock = new TextBlock();
			grid4.Children.Add(textBlock);
			textBlock.Name = "e_13";
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 0);
			textBlock.SetResourceReference(TextBlock.TextProperty, "ContractScreen_ContractFilterTitle");
			ComboBox comboBox = new ComboBox();
			grid4.Children.Add(comboBox);
			comboBox.Name = "e_14";
			comboBox.Margin = new Thickness(5f, 10f, 5f, 10f);
			comboBox.VerticalAlignment = VerticalAlignment.Center;
			comboBox.TabIndex = 0;
			comboBox.MaxDropDownHeight = 240f;
			Grid.SetColumn(comboBox, 1);
			GamepadHelp.SetTargetName(comboBox, "SelectTypeHelp");
			GamepadHelp.SetTabIndexDown(comboBox, 1);
			comboBox.SetBinding(binding: new Binding("FilterTargets")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			comboBox.SetBinding(binding: new Binding("FilterTargetIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			comboBox.SetBinding(binding: new Binding("SelectedFilterTarget")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			Grid grid5 = new Grid();
			grid3.Children.Add(grid5);
			grid5.Name = "e_15";
			Grid.SetRow(grid5, 1);
			ListBox listBox = new ListBox();
			grid5.Children.Add(listBox);
			listBox.Name = "e_16";
			listBox.Margin = new Thickness(0f, 0f, 5f, 0f);
			GamepadBinding gamepadBinding2 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.AButton)
			};
			gamepadBinding2.SetBinding(binding: new Binding("AcceptCommand")
			{
				UseGeneratedBindings = true
			}, property: InputBinding.CommandProperty);
			listBox.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = listBox;
			listBox.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox.TabIndex = 1;
			listBox.SelectionMode = SelectionMode.Single;
			GamepadHelp.SetTargetName(listBox, "ContractsHelp");
			GamepadHelp.SetTabIndexUp(listBox, 0);
			listBox.SetBinding(binding: new Binding("AvailableContracts")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			listBox.SetBinding(binding: new Binding("SelectedAvailableContractIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			listBox.SetBinding(binding: new Binding("SelectedAvailableContract")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedItemProperty);
			InitializeElemente_16Resources(listBox);
			Grid grid6 = new Grid();
			grid5.Children.Add(grid6);
			grid6.Name = "e_17";
			RowDefinition item10 = new RowDefinition
			{
				Height = new GridLength(30f, GridUnitType.Pixel)
			};
			grid6.RowDefinitions.Add(item10);
			RowDefinition item11 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid6.RowDefinitions.Add(item11);
			RowDefinition item12 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid6.RowDefinitions.Add(item12);
			grid6.SetBinding(binding: new Binding("IsNoAvailableContractVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			TextBlock textBlock2 = new TextBlock();
			grid6.Children.Add(textBlock2);
			textBlock2.Name = "e_18";
			textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(textBlock2, 1);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoAvailableContracts");
			Border border = new Border();
			grid2.Children.Add(border);
			border.Name = "e_19";
			border.VerticalAlignment = VerticalAlignment.Stretch;
			border.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 1);
			ContentPresenter contentPresenter = (ContentPresenter)(border.Child = new ContentPresenter());
			contentPresenter.Name = "e_20";
			contentPresenter.Margin = new Thickness(5f, 0f, 0f, 0f);
			contentPresenter.SetBinding(binding: new Binding("SelectedAvailableContract")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			contentPresenter.SetBinding(binding: new Binding("SelectedAvailableContract")
			{
				UseGeneratedBindings = true
			}, property: ContentPresenter.ContentProperty);
			Border border2 = new Border();
			grid2.Children.Add(border2);
			border2.Name = "e_21";
			border2.Height = 2f;
			border2.Margin = new Thickness(0f, 10f, 0f, 10f);
			border2.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border2, 1);
			Grid.SetColumnSpan(border2, 2);
			Grid grid7 = new Grid();
			grid2.Children.Add(grid7);
			grid7.Name = "ContractsHelp";
			grid7.Visibility = Visibility.Collapsed;
			grid7.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item13 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.ColumnDefinitions.Add(item13);
			ColumnDefinition item14 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.ColumnDefinitions.Add(item14);
			ColumnDefinition item15 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid7.ColumnDefinitions.Add(item15);
			ColumnDefinition item16 = new ColumnDefinition();
			grid7.ColumnDefinitions.Add(item16);
			Grid.SetRow(grid7, 2);
			Grid.SetColumnSpan(grid7, 2);
			TextBlock textBlock3 = new TextBlock();
			grid7.Children.Add(textBlock3);
			textBlock3.Name = "e_22";
			textBlock3.Margin = new Thickness(0f, 0f, 15f, 0f);
			textBlock3.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.SetBinding(binding: new Binding("IsAcceptEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Accept");
			TextBlock textBlock4 = new TextBlock();
			grid7.Children.Add(textBlock4);
			textBlock4.Name = "e_23";
			textBlock4.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock4.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Refresh");
			TextBlock textBlock5 = new TextBlock();
			grid7.Children.Add(textBlock5);
			textBlock5.Name = "e_24";
			textBlock5.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock5.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock5, 2);
			textBlock5.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid8 = new Grid();
			grid2.Children.Add(grid8);
			grid8.Name = "SelectTypeHelp";
			grid8.Visibility = Visibility.Collapsed;
			grid8.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item17 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item17);
			ColumnDefinition item18 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item18);
			ColumnDefinition item19 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid8.ColumnDefinitions.Add(item19);
			ColumnDefinition item20 = new ColumnDefinition();
			grid8.ColumnDefinitions.Add(item20);
			Grid.SetRow(grid8, 2);
			Grid.SetColumnSpan(grid8, 2);
			TextBlock textBlock6 = new TextBlock();
			grid8.Children.Add(textBlock6);
			textBlock6.Name = "e_25";
			textBlock6.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock6.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Select");
			TextBlock textBlock7 = new TextBlock();
			grid8.Children.Add(textBlock7);
			textBlock7.Name = "e_26";
			textBlock7.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock7.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock7, 1);
			textBlock7.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Refresh");
			TextBlock textBlock8 = new TextBlock();
			grid8.Children.Add(textBlock8);
			textBlock8.Name = "e_27";
			textBlock8.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock8.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock8, 2);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
<<<<<<< HEAD
			observableCollection.Add(tabItem);
=======
			((Collection<object>)(object)obj).Add((object)tabItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem2 = new TabItem();
			tabItem2.Name = "e_28";
			tabItem2.IsTabStop = false;
			tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, "ContractScreen_Tab_AcceptedContracts");
			Grid grid10 = (Grid)(tabItem2.Content = new Grid());
			grid10.Name = "e_29";
			grid10.SetBinding(binding: new Binding("ActiveViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid11 = new Grid();
			grid10.Children.Add(grid11);
			grid11.Name = "e_30";
			Grid grid12 = new Grid();
			grid11.Children.Add(grid12);
			grid12.Name = "e_31";
			GamepadBinding gamepadBinding3 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.AButton)
			};
			gamepadBinding3.SetBinding(binding: new Binding("AbandonCommand"), property: InputBinding.CommandProperty);
			grid12.InputBindings.Add(gamepadBinding3);
			gamepadBinding3.Parent = grid12;
			GamepadBinding gamepadBinding4 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding4.SetBinding(binding: new Binding("FinishCommand"), property: InputBinding.CommandProperty);
			grid12.InputBindings.Add(gamepadBinding4);
			gamepadBinding4.Parent = grid12;
			GamepadBinding gamepadBinding5 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding5.SetBinding(binding: new Binding("RefreshActiveCommand"), property: InputBinding.CommandProperty);
			grid12.InputBindings.Add(gamepadBinding5);
			gamepadBinding5.Parent = grid12;
			RowDefinition item21 = new RowDefinition();
			grid12.RowDefinitions.Add(item21);
			RowDefinition item22 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.RowDefinitions.Add(item22);
			RowDefinition item23 = new RowDefinition
			{
				Height = new GridLength(65f, GridUnitType.Pixel)
			};
			grid12.RowDefinitions.Add(item23);
			ColumnDefinition item24 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid12.ColumnDefinitions.Add(item24);
			ColumnDefinition item25 = new ColumnDefinition
			{
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid12.ColumnDefinitions.Add(item25);
			ListBox listBox2 = new ListBox();
			grid12.Children.Add(listBox2);
			listBox2.Name = "e_32";
			listBox2.Margin = new Thickness(0f, 0f, 5f, 0f);
			listBox2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox2.TabIndex = 50;
			listBox2.SelectionMode = SelectionMode.Single;
			GamepadHelp.SetTargetName(listBox2, "ActiveContractsHelp");
			listBox2.SetBinding(binding: new Binding("ActiveContracts"), property: ItemsControl.ItemsSourceProperty);
			listBox2.SetBinding(binding: new Binding("SelectedActiveContractIndex"), property: Selector.SelectedIndexProperty);
			listBox2.SetBinding(binding: new Binding("SelectedActiveContract"), property: Selector.SelectedItemProperty);
			InitializeElemente_32Resources(listBox2);
			Grid grid13 = new Grid();
			grid12.Children.Add(grid13);
			grid13.Name = "e_33";
			RowDefinition item26 = new RowDefinition
			{
				Height = new GridLength(30f, GridUnitType.Pixel)
			};
			grid13.RowDefinitions.Add(item26);
			RowDefinition item27 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid13.RowDefinitions.Add(item27);
			RowDefinition item28 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid13.RowDefinitions.Add(item28);
			Grid.SetColumn(grid13, 0);
			Grid.SetRow(grid13, 0);
			grid13.SetBinding(binding: new Binding("IsNoActiveContractVisible"), property: UIElement.VisibilityProperty);
			TextBlock textBlock9 = new TextBlock();
			grid13.Children.Add(textBlock9);
			textBlock9.Name = "e_34";
			textBlock9.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock9.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(textBlock9, 1);
			textBlock9.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoActiveContracts");
			Border border3 = new Border();
			grid12.Children.Add(border3);
			border3.Name = "e_35";
			border3.VerticalAlignment = VerticalAlignment.Stretch;
			border3.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			border3.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border3, 1);
			ContentPresenter contentPresenter2 = (ContentPresenter)(border3.Child = new ContentPresenter());
			contentPresenter2.Name = "e_36";
			contentPresenter2.Margin = new Thickness(5f, 0f, 0f, 0f);
			Grid.SetColumn(contentPresenter2, 1);
			contentPresenter2.SetBinding(binding: new Binding("SelectedActiveContract"), property: UIElement.DataContextProperty);
			contentPresenter2.SetBinding(binding: new Binding("SelectedActiveContract"), property: ContentPresenter.ContentProperty);
			Border border4 = new Border();
			grid12.Children.Add(border4);
			border4.Name = "e_37";
			border4.Height = 2f;
			border4.Margin = new Thickness(0f, 10f, 0f, 10f);
			border4.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border4, 1);
			Grid.SetColumnSpan(border4, 2);
			Grid grid14 = new Grid();
			grid12.Children.Add(grid14);
			grid14.Name = "ActiveContractsHelp";
			grid14.Visibility = Visibility.Collapsed;
			grid14.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item29 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item29);
			ColumnDefinition item30 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item30);
			ColumnDefinition item31 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item31);
			ColumnDefinition item32 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid14.ColumnDefinitions.Add(item32);
			ColumnDefinition item33 = new ColumnDefinition();
			grid14.ColumnDefinitions.Add(item33);
			Grid.SetRow(grid14, 2);
			Grid.SetColumnSpan(grid14, 2);
			TextBlock textBlock10 = new TextBlock();
			grid14.Children.Add(textBlock10);
			textBlock10.Name = "e_38";
			textBlock10.Margin = new Thickness(0f, 0f, 15f, 0f);
			textBlock10.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.SetBinding(binding: new Binding("IsAbandonEnabled"), property: UIElement.VisibilityProperty);
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ActiveContractsScreen_Help_Abandon");
			TextBlock textBlock11 = new TextBlock();
			grid14.Children.Add(textBlock11);
			textBlock11.Name = "e_39";
			textBlock11.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock11.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock11, 1);
			textBlock11.SetResourceReference(TextBlock.TextProperty, "ActiveContractsScreen_Help_Refresh");
			TextBlock textBlock12 = new TextBlock();
			grid14.Children.Add(textBlock12);
			textBlock12.Name = "e_40";
			textBlock12.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock12.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock12, 2);
			textBlock12.SetBinding(binding: new Binding("IsFinishEnabled"), property: UIElement.VisibilityProperty);
			textBlock12.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Finish");
			TextBlock textBlock13 = new TextBlock();
			grid14.Children.Add(textBlock13);
			textBlock13.Name = "e_41";
			textBlock13.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock13.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock13, 3);
			textBlock13.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
<<<<<<< HEAD
			observableCollection.Add(tabItem2);
=======
			((Collection<object>)(object)obj).Add((object)tabItem2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem3 = new TabItem
			{
				Name = "e_42",
				IsTabStop = false
			};
			tabItem3.SetBinding(binding: new Binding("IsAdministrationVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, "ContractScreen_Tab_Administration");
			Grid grid16 = (Grid)(tabItem3.Content = new Grid());
			grid16.Name = "e_43";
			grid16.SetBinding(binding: new Binding("AdministrationViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid17 = new Grid();
			grid16.Children.Add(grid17);
			grid17.Name = "e_44";
			Grid grid18 = new Grid();
			grid17.Children.Add(grid18);
			grid18.Name = "e_45";
			RowDefinition item34 = new RowDefinition();
			grid18.RowDefinitions.Add(item34);
			RowDefinition item35 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid18.RowDefinitions.Add(item35);
			RowDefinition item36 = new RowDefinition
			{
				Height = new GridLength(65f, GridUnitType.Pixel)
			};
			grid18.RowDefinitions.Add(item36);
			ColumnDefinition item37 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid18.ColumnDefinitions.Add(item37);
			ColumnDefinition item38 = new ColumnDefinition
			{
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid18.ColumnDefinitions.Add(item38);
			ListBox listBox3 = new ListBox();
			grid18.Children.Add(listBox3);
			listBox3.Name = "e_46";
			listBox3.Margin = new Thickness(0f, 0f, 5f, 0f);
			GamepadBinding gamepadBinding6 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding6.SetBinding(binding: new Binding("DeleteCommand"), property: InputBinding.CommandProperty);
			listBox3.InputBindings.Add(gamepadBinding6);
			gamepadBinding6.Parent = listBox3;
			GamepadBinding gamepadBinding7 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.DButton)
			};
			gamepadBinding7.SetBinding(binding: new Binding("RefreshCommand"), property: InputBinding.CommandProperty);
			listBox3.InputBindings.Add(gamepadBinding7);
			gamepadBinding7.Parent = listBox3;
			listBox3.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox3.TabIndex = 100;
			listBox3.SelectionMode = SelectionMode.Single;
			GamepadHelp.SetTargetName(listBox3, "AdminContractsListHelp");
			GamepadHelp.SetTabIndexRight(listBox3, 103);
			listBox3.SetBinding(binding: new Binding("AdministrableContracts"), property: ItemsControl.ItemsSourceProperty);
			listBox3.SetBinding(binding: new Binding("SelectedAdministrableContract"), property: Selector.SelectedItemProperty);
			InitializeElemente_46Resources(listBox3);
			Grid grid19 = new Grid();
			grid18.Children.Add(grid19);
			grid19.Name = "e_47";
			RowDefinition item39 = new RowDefinition
			{
				Height = new GridLength(30f, GridUnitType.Pixel)
			};
			grid19.RowDefinitions.Add(item39);
			RowDefinition item40 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid19.RowDefinitions.Add(item40);
			RowDefinition item41 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid19.RowDefinitions.Add(item41);
			grid19.SetBinding(binding: new Binding("IsNoAdministrableContractVisible"), property: UIElement.VisibilityProperty);
			TextBlock textBlock14 = new TextBlock();
			grid19.Children.Add(textBlock14);
			textBlock14.Name = "e_48";
			textBlock14.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock14.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(textBlock14, 1);
			textBlock14.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoAdministrableContracts");
			Grid grid20 = new Grid();
			grid18.Children.Add(grid20);
			grid20.Name = "AdminContractsListHelp";
			grid20.Visibility = Visibility.Collapsed;
			grid20.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item42 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.ColumnDefinitions.Add(item42);
			ColumnDefinition item43 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.ColumnDefinitions.Add(item43);
			ColumnDefinition item44 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.ColumnDefinitions.Add(item44);
			ColumnDefinition item45 = new ColumnDefinition();
			grid20.ColumnDefinitions.Add(item45);
			Grid.SetColumn(grid20, 0);
			Grid.SetRow(grid20, 2);
			TextBlock textBlock15 = new TextBlock();
			grid20.Children.Add(textBlock15);
			textBlock15.Name = "e_49";
			textBlock15.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock15.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock15.VerticalAlignment = VerticalAlignment.Center;
			textBlock15.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Delete");
			TextBlock textBlock16 = new TextBlock();
			grid20.Children.Add(textBlock16);
			textBlock16.Name = "e_50";
			textBlock16.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock16.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock16.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock16, 1);
			textBlock16.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_Refresh");
			TextBlock textBlock17 = new TextBlock();
			grid20.Children.Add(textBlock17);
			textBlock17.Name = "e_51";
			textBlock17.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock17.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock17.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock17, 2);
			textBlock17.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Border border5 = new Border();
			grid18.Children.Add(border5);
			border5.Name = "e_52";
			border5.VerticalAlignment = VerticalAlignment.Stretch;
			border5.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			border5.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border5.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border5, 1);
			Grid grid21 = (Grid)(border5.Child = new Grid());
			grid21.Name = "e_53";
			GamepadBinding gamepadBinding8 = new GamepadBinding
			{
				Gesture = new GamepadGesture(GamepadInput.CButton)
			};
			gamepadBinding8.SetBinding(binding: new Binding("CreateCommand"), property: InputBinding.CommandProperty);
			grid21.InputBindings.Add(gamepadBinding8);
			gamepadBinding8.Parent = grid21;
			RowDefinition item46 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid21.RowDefinitions.Add(item46);
			RowDefinition item47 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid21.RowDefinitions.Add(item47);
			RowDefinition item48 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid21.RowDefinitions.Add(item48);
			StackPanel stackPanel = new StackPanel();
			grid21.Children.Add(stackPanel);
			stackPanel.Name = "e_54";
			stackPanel.Margin = new Thickness(10f, 10f, 0f, 0f);
			stackPanel.Orientation = Orientation.Vertical;
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock18 = new TextBlock();
			stackPanel.Children.Add(textBlock18);
			textBlock18.Name = "e_55";
			textBlock18.Margin = new Thickness(15f, 0f, 0f, 0f);
			textBlock18.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock18.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NewContract");
			Grid grid22 = new Grid();
			stackPanel.Children.Add(grid22);
			grid22.Name = "e_56";
			grid22.Margin = new Thickness(15f, 0f, 0f, 0f);
			RowDefinition item49 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid22.RowDefinitions.Add(item49);
			RowDefinition item50 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid22.RowDefinitions.Add(item50);
			RowDefinition item51 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid22.RowDefinitions.Add(item51);
			RowDefinition item52 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid22.RowDefinitions.Add(item52);
			ColumnDefinition item53 = new ColumnDefinition
			{
				Width = new GridLength(44f, GridUnitType.Pixel)
			};
			grid22.ColumnDefinitions.Add(item53);
			ColumnDefinition item54 = new ColumnDefinition
			{
				Width = new GridLength(156f, GridUnitType.Pixel)
			};
			grid22.ColumnDefinitions.Add(item54);
			ColumnDefinition item55 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid22.ColumnDefinitions.Add(item55);
			TextBlock textBlock19 = new TextBlock();
			grid22.Children.Add(textBlock19);
			textBlock19.Name = "e_57";
			textBlock19.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock19.VerticalAlignment = VerticalAlignment.Center;
			textBlock19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock19, 0);
			Grid.SetRow(textBlock19, 0);
			Grid.SetColumnSpan(textBlock19, 2);
			textBlock19.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_MoneyReward");
			TextBlock textBlock20 = new TextBlock();
			grid22.Children.Add(textBlock20);
			textBlock20.Name = "e_58";
			textBlock20.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock20.VerticalAlignment = VerticalAlignment.Center;
			textBlock20.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock20, 0);
			Grid.SetRow(textBlock20, 1);
			Grid.SetColumnSpan(textBlock20, 2);
			textBlock20.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_StartingDeposit");
			TextBlock textBlock21 = new TextBlock();
			grid22.Children.Add(textBlock21);
			textBlock21.Name = "e_59";
			textBlock21.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock21.VerticalAlignment = VerticalAlignment.Center;
			textBlock21.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock21, 0);
			Grid.SetRow(textBlock21, 2);
			Grid.SetColumnSpan(textBlock21, 2);
			textBlock21.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_Duration");
			TextBlock textBlock22 = new TextBlock();
			grid22.Children.Add(textBlock22);
			textBlock22.Name = "e_60";
			textBlock22.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock22.VerticalAlignment = VerticalAlignment.Center;
			textBlock22.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock22, 0);
			Grid.SetRow(textBlock22, 3);
			Grid.SetColumnSpan(textBlock22, 2);
			textBlock22.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_Type");
			NumericTextBox numericTextBox = new NumericTextBox();
			grid22.Children.Add(numericTextBox);
			numericTextBox.Name = "e_61";
			numericTextBox.Margin = new Thickness(0f, 5f, 15f, 0f);
			numericTextBox.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox.TabIndex = 103;
			numericTextBox.MaxLength = 7;
			numericTextBox.Minimum = 0f;
			numericTextBox.Maximum = 9999999f;
			Grid.SetColumn(numericTextBox, 2);
			Grid.SetRow(numericTextBox, 0);
			GamepadHelp.SetTargetName(numericTextBox, "NumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox, 100);
			GamepadHelp.SetTabIndexDown(numericTextBox, 104);
			numericTextBox.SetBinding(binding: new Binding("NewContractCurrencyReward"), property: NumericTextBox.ValueProperty);
			NumericTextBox numericTextBox2 = new NumericTextBox();
			grid22.Children.Add(numericTextBox2);
			numericTextBox2.Name = "e_62";
			numericTextBox2.Margin = new Thickness(0f, 5f, 15f, 0f);
			numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox2.TabIndex = 104;
			numericTextBox2.MaxLength = 7;
			numericTextBox2.Minimum = 0f;
			numericTextBox2.Maximum = 9999999f;
			Grid.SetColumn(numericTextBox2, 2);
			Grid.SetRow(numericTextBox2, 1);
			GamepadHelp.SetTargetName(numericTextBox2, "NumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox2, 100);
			GamepadHelp.SetTabIndexUp(numericTextBox2, 103);
			GamepadHelp.SetTabIndexDown(numericTextBox2, 105);
			numericTextBox2.SetBinding(binding: new Binding("NewContractStartDeposit"), property: NumericTextBox.ValueProperty);
			NumericTextBox numericTextBox3 = new NumericTextBox();
			grid22.Children.Add(numericTextBox3);
			numericTextBox3.Name = "e_63";
			numericTextBox3.Margin = new Thickness(0f, 5f, 15f, 0f);
			numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox3.TabIndex = 105;
			numericTextBox3.MaxLength = 5;
			numericTextBox3.Minimum = 0f;
			numericTextBox3.Maximum = 99999f;
			Grid.SetColumn(numericTextBox3, 2);
			Grid.SetRow(numericTextBox3, 2);
			GamepadHelp.SetTargetName(numericTextBox3, "NumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox3, 100);
			GamepadHelp.SetTabIndexUp(numericTextBox3, 104);
			GamepadHelp.SetTabIndexDown(numericTextBox3, 106);
			numericTextBox3.SetBinding(binding: new Binding("NewContractDurationInMin"), property: NumericTextBox.ValueProperty);
			ComboBox comboBox2 = new ComboBox();
			grid22.Children.Add(comboBox2);
			comboBox2.Name = "e_64";
			comboBox2.Margin = new Thickness(0f, 4f, 15f, 4f);
			comboBox2.TabIndex = 106;
			Grid.SetColumn(comboBox2, 2);
			Grid.SetRow(comboBox2, 3);
			GamepadHelp.SetTargetName(comboBox2, "SelectHelp");
			GamepadHelp.SetTabIndexLeft(comboBox2, 100);
			GamepadHelp.SetTabIndexUp(comboBox2, 105);
			comboBox2.SetBinding(binding: new Binding("ContractTypes"), property: ItemsControl.ItemsSourceProperty);
			comboBox2.SetBinding(binding: new Binding("SelectedContractTypeIndex"), property: Selector.SelectedIndexProperty);
			comboBox2.SetBinding(binding: new Binding("TabIndexDown"), property: GamepadHelp.TabIndexDownProperty);
			Border border6 = new Border();
			stackPanel.Children.Add(border6);
			border6.Name = "e_65";
			border6.Height = 2f;
			border6.Margin = new Thickness(15f, 0f, 15f, 0f);
			border6.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid grid23 = new Grid();
			stackPanel.Children.Add(grid23);
			grid23.Name = "e_66";
			grid23.Margin = new Thickness(15f, 0f, 0f, 0f);
			Grid grid24 = new Grid();
			grid23.Children.Add(grid24);
			grid24.Name = "e_67";
			RowDefinition item56 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid24.RowDefinitions.Add(item56);
			ColumnDefinition item57 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid24.ColumnDefinitions.Add(item57);
			ColumnDefinition item58 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid24.ColumnDefinitions.Add(item58);
			grid24.SetBinding(binding: new Binding("IsContractSelected_Deliver"), property: UIElement.VisibilityProperty);
			TextBlock textBlock23 = new TextBlock();
			grid24.Children.Add(textBlock23);
			textBlock23.Name = "e_68";
			textBlock23.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock23.VerticalAlignment = VerticalAlignment.Center;
			textBlock23.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock23, 0);
			Grid.SetRow(textBlock23, 0);
			textBlock23.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetBlock");
			Grid grid25 = new Grid();
			grid24.Children.Add(grid25);
			grid25.Name = "e_69";
			grid25.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid25.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item59 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid25.ColumnDefinitions.Add(item59);
			ColumnDefinition item60 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid25.ColumnDefinitions.Add(item60);
			Grid.SetColumn(grid25, 1);
			Grid.SetRow(grid25, 0);
			TextBlock textBlock24 = new TextBlock();
			grid25.Children.Add(textBlock24);
			textBlock24.Name = "e_70";
			textBlock24.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock24, 0);
			textBlock24.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button = new Button();
			grid25.Children.Add(button);
			button.Name = "e_71";
			button.Width = 150f;
			button.Margin = new Thickness(10f, 5f, 15f, 0f);
			button.TabIndex = 107;
			Grid.SetColumn(button, 1);
			GamepadHelp.SetTargetName(button, "SelectHelp");
			GamepadHelp.SetTabIndexLeft(button, 100);
			GamepadHelp.SetTabIndexUp(button, 106);
			GamepadHelp.SetTabIndexDown(button, 114);
			button.SetBinding(binding: new Binding("NewContractDeliverBlockSelectCommand"), property: Button.CommandProperty);
			button.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectBlock");
			Grid grid26 = new Grid();
			grid23.Children.Add(grid26);
			grid26.Name = "e_72";
			RowDefinition item61 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid26.RowDefinitions.Add(item61);
			RowDefinition item62 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid26.RowDefinitions.Add(item62);
			RowDefinition item63 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid26.RowDefinitions.Add(item63);
			ColumnDefinition item64 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid26.ColumnDefinitions.Add(item64);
			ColumnDefinition item65 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid26.ColumnDefinitions.Add(item65);
			grid26.SetBinding(binding: new Binding("IsContractSelected_ObtainAndDeliver"), property: UIElement.VisibilityProperty);
			TextBlock textBlock25 = new TextBlock();
			grid26.Children.Add(textBlock25);
			textBlock25.Name = "e_73";
			textBlock25.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock25.VerticalAlignment = VerticalAlignment.Center;
			textBlock25.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock25, 0);
			Grid.SetRow(textBlock25, 0);
			textBlock25.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetBlock");
			TextBlock textBlock26 = new TextBlock();
			grid26.Children.Add(textBlock26);
			textBlock26.Name = "e_74";
			textBlock26.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock26.VerticalAlignment = VerticalAlignment.Center;
			textBlock26.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock26, 0);
			Grid.SetRow(textBlock26, 1);
			textBlock26.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_ItemType");
			TextBlock textBlock27 = new TextBlock();
			grid26.Children.Add(textBlock27);
			textBlock27.Name = "e_75";
			textBlock27.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock27.VerticalAlignment = VerticalAlignment.Center;
			textBlock27.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock27, 0);
			Grid.SetRow(textBlock27, 2);
			textBlock27.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_ItemAmount");
			Grid grid27 = new Grid();
			grid26.Children.Add(grid27);
			grid27.Name = "e_76";
			grid27.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid27.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item66 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid27.ColumnDefinitions.Add(item66);
			ColumnDefinition item67 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid27.ColumnDefinitions.Add(item67);
			Grid.SetColumn(grid27, 1);
			Grid.SetRow(grid27, 0);
			TextBlock textBlock28 = new TextBlock();
			grid27.Children.Add(textBlock28);
			textBlock28.Name = "e_77";
			textBlock28.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock28, 0);
			textBlock28.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button2 = new Button();
			grid27.Children.Add(button2);
			button2.Name = "e_78";
			button2.Width = 150f;
			button2.Margin = new Thickness(10f, 5f, 15f, 0f);
			button2.TabIndex = 108;
			Grid.SetColumn(button2, 1);
			GamepadHelp.SetTargetName(button2, "SelectHelp");
			GamepadHelp.SetTabIndexLeft(button2, 100);
			GamepadHelp.SetTabIndexUp(button2, 106);
			GamepadHelp.SetTabIndexDown(button2, 109);
			button2.SetBinding(binding: new Binding("NewContractObtainAndDeliverBlockSelectCommand"), property: Button.CommandProperty);
			button2.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectBlock");
			ComboBox comboBox3 = new ComboBox();
			grid26.Children.Add(comboBox3);
			comboBox3.Name = "e_79";
			comboBox3.Margin = new Thickness(0f, 0f, 15f, 0f);
			comboBox3.TabIndex = 109;
			Grid.SetColumn(comboBox3, 1);
			Grid.SetRow(comboBox3, 1);
			GamepadHelp.SetTargetName(comboBox3, "SelectHelp");
			GamepadHelp.SetTabIndexLeft(comboBox3, 100);
			GamepadHelp.SetTabIndexUp(comboBox3, 108);
			GamepadHelp.SetTabIndexDown(comboBox3, 110);
			comboBox3.SetBinding(binding: new Binding("DeliverableItems"), property: ItemsControl.ItemsSourceProperty);
			comboBox3.SetBinding(binding: new Binding("NewContractObtainAndDeliverSelectedItemType"), property: Selector.SelectedItemProperty);
			NumericTextBox numericTextBox4 = new NumericTextBox();
			grid26.Children.Add(numericTextBox4);
			numericTextBox4.Name = "e_80";
			numericTextBox4.Margin = new Thickness(0f, 0f, 15f, 0f);
			numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox4.TabIndex = 110;
			numericTextBox4.MaxLength = 5;
			numericTextBox4.Minimum = 0f;
			numericTextBox4.Maximum = 99999f;
			Grid.SetColumn(numericTextBox4, 1);
			Grid.SetRow(numericTextBox4, 2);
			GamepadHelp.SetTargetName(numericTextBox4, "NumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox4, 100);
			GamepadHelp.SetTabIndexUp(numericTextBox4, 109);
			GamepadHelp.SetTabIndexDown(numericTextBox4, 114);
			numericTextBox4.SetBinding(binding: new Binding("NewContractObtainAndDeliverItemAmount"), property: NumericTextBox.ValueProperty);
			Grid grid28 = new Grid();
			grid23.Children.Add(grid28);
			grid28.Name = "e_81";
			RowDefinition item68 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid28.RowDefinitions.Add(item68);
			RowDefinition item69 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid28.RowDefinitions.Add(item69);
			ColumnDefinition item70 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid28.ColumnDefinitions.Add(item70);
			ColumnDefinition item71 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid28.ColumnDefinitions.Add(item71);
			grid28.SetBinding(binding: new Binding("IsContractSelected_Find"), property: UIElement.VisibilityProperty);
			TextBlock textBlock29 = new TextBlock();
			grid28.Children.Add(textBlock29);
			textBlock29.Name = "e_82";
			textBlock29.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock29.VerticalAlignment = VerticalAlignment.Center;
			textBlock29.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock29, 0);
			Grid.SetRow(textBlock29, 0);
			textBlock29.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetGrid");
			TextBlock textBlock30 = new TextBlock();
			grid28.Children.Add(textBlock30);
			textBlock30.Name = "e_83";
			textBlock30.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock30.VerticalAlignment = VerticalAlignment.Center;
			textBlock30.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock30, 0);
			Grid.SetRow(textBlock30, 1);
			textBlock30.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_SearchRadius");
			Grid grid29 = new Grid();
			grid28.Children.Add(grid29);
			grid29.Name = "e_84";
			grid29.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid29.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item72 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid29.ColumnDefinitions.Add(item72);
			ColumnDefinition item73 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid29.ColumnDefinitions.Add(item73);
			Grid.SetColumn(grid29, 1);
			Grid.SetRow(grid29, 0);
			TextBlock textBlock31 = new TextBlock();
			grid29.Children.Add(textBlock31);
			textBlock31.Name = "e_85";
			textBlock31.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock31, 0);
			textBlock31.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button3 = new Button();
			grid29.Children.Add(button3);
			button3.Name = "e_86";
			button3.Width = 150f;
			button3.Margin = new Thickness(10f, 5f, 15f, 0f);
			button3.VerticalAlignment = VerticalAlignment.Center;
			button3.TabIndex = 111;
			Grid.SetColumn(button3, 1);
			GamepadHelp.SetTargetName(button3, "SelectHelp");
			GamepadHelp.SetTabIndexLeft(button3, 100);
			GamepadHelp.SetTabIndexUp(button3, 106);
			GamepadHelp.SetTabIndexDown(button3, 112);
			button3.SetBinding(binding: new Binding("NewContractFindGridSelectCommand"), property: Button.CommandProperty);
			button3.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectGrid");
			NumericTextBox numericTextBox5 = new NumericTextBox();
			grid28.Children.Add(numericTextBox5);
			numericTextBox5.Name = "e_87";
			numericTextBox5.Margin = new Thickness(0f, 0f, 15f, 0f);
			numericTextBox5.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox5.TabIndex = 112;
			numericTextBox5.MaxLength = 5;
			numericTextBox5.Minimum = 0f;
			numericTextBox5.Maximum = 99999f;
			Grid.SetColumn(numericTextBox5, 1);
			Grid.SetRow(numericTextBox5, 1);
			GamepadHelp.SetTargetName(numericTextBox5, "NumericHelp");
			GamepadHelp.SetTabIndexLeft(numericTextBox5, 100);
			GamepadHelp.SetTabIndexUp(numericTextBox5, 111);
			GamepadHelp.SetTabIndexDown(numericTextBox5, 114);
			numericTextBox5.SetBinding(binding: new Binding("NewContractFindSearchRadius"), property: NumericTextBox.ValueProperty);
			Grid grid30 = new Grid();
			grid23.Children.Add(grid30);
			grid30.Name = "e_88";
			RowDefinition item74 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid30.RowDefinitions.Add(item74);
			ColumnDefinition item75 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid30.ColumnDefinitions.Add(item75);
			ColumnDefinition item76 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid30.ColumnDefinitions.Add(item76);
			grid30.SetBinding(binding: new Binding("IsContractSelected_Repair"), property: UIElement.VisibilityProperty);
			TextBlock textBlock32 = new TextBlock();
			grid30.Children.Add(textBlock32);
			textBlock32.Name = "e_89";
			textBlock32.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock32.VerticalAlignment = VerticalAlignment.Center;
			textBlock32.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock32, 0);
			Grid.SetRow(textBlock32, 0);
			textBlock32.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetGrid");
			Grid grid31 = new Grid();
			grid30.Children.Add(grid31);
			grid31.Name = "e_90";
			grid31.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid31.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item77 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid31.ColumnDefinitions.Add(item77);
			ColumnDefinition item78 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid31.ColumnDefinitions.Add(item78);
			Grid.SetColumn(grid31, 1);
			Grid.SetRow(grid31, 0);
			TextBlock textBlock33 = new TextBlock();
			grid31.Children.Add(textBlock33);
			textBlock33.Name = "e_91";
			textBlock33.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock33, 0);
			textBlock33.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button4 = new Button();
			grid31.Children.Add(button4);
			button4.Name = "e_92";
			button4.Width = 150f;
			button4.Margin = new Thickness(10f, 5f, 15f, 0f);
			button4.TabIndex = 113;
			Grid.SetColumn(button4, 1);
			GamepadHelp.SetTargetName(button4, "SelectHelp");
			GamepadHelp.SetTabIndexLeft(button4, 100);
			GamepadHelp.SetTabIndexUp(button4, 106);
			GamepadHelp.SetTabIndexDown(button4, 114);
			button4.SetBinding(binding: new Binding("NewContractRepairGridSelectCommand"), property: Button.CommandProperty);
			button4.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectGrid");
			Border border7 = new Border();
			grid21.Children.Add(border7);
			border7.Name = "e_93";
			border7.Height = 2f;
			border7.Margin = new Thickness(25f, 0f, 15f, 0f);
			border7.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border7, 1);
			Grid grid32 = new Grid();
			grid21.Children.Add(grid32);
			grid32.Name = "e_94";
			ColumnDefinition item79 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid32.ColumnDefinitions.Add(item79);
			ColumnDefinition item80 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid32.ColumnDefinitions.Add(item80);
			Grid.SetRow(grid32, 2);
			TextBlock textBlock34 = new TextBlock();
			grid32.Children.Add(textBlock34);
			textBlock34.Name = "e_95";
			textBlock34.Margin = new Thickness(25f, 10f, 0f, 10f);
			textBlock34.VerticalAlignment = VerticalAlignment.Center;
			textBlock34.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock34, 0);
			Grid.SetRow(textBlock34, 0);
			textBlock34.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_CurrentMoney");
			StackPanel stackPanel2 = new StackPanel();
			grid32.Children.Add(stackPanel2);
			stackPanel2.Name = "e_96";
			stackPanel2.Margin = new Thickness(0f, 10f, 15f, 10f);
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel2.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel2, 1);
			Grid.SetRow(stackPanel2, 0);
			TextBlock textBlock35 = new TextBlock();
			stackPanel2.Children.Add(textBlock35);
			textBlock35.Name = "e_97";
			textBlock35.Margin = new Thickness(30f, 0f, 0f, 0f);
			textBlock35.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock35.VerticalAlignment = VerticalAlignment.Center;
			textBlock35.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock35.SetBinding(binding: new Binding("CurrentMoneyFormated"), property: TextBlock.TextProperty);
			Image image = new Image();
			stackPanel2.Children.Add(image);
			image.Name = "e_98";
			image.Height = 20f;
			image.Margin = new Thickness(4f, 2f, 2f, 2f);
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			Grid.SetColumn(image, 2);
			image.SetBinding(binding: new Binding("CurrencyIcon"), property: Image.SourceProperty);
			Border border8 = new Border();
			grid18.Children.Add(border8);
			border8.Name = "e_99";
			border8.Height = 2f;
			border8.Margin = new Thickness(0f, 10f, 0f, 10f);
			border8.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border8, 1);
			Grid.SetColumnSpan(border8, 2);
			Grid grid33 = new Grid();
			grid18.Children.Add(grid33);
			grid33.Name = "NumericHelp";
			grid33.Visibility = Visibility.Collapsed;
			grid33.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item81 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid33.ColumnDefinitions.Add(item81);
			ColumnDefinition item82 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid33.ColumnDefinitions.Add(item82);
			ColumnDefinition item83 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid33.ColumnDefinitions.Add(item83);
			ColumnDefinition item84 = new ColumnDefinition();
			grid33.ColumnDefinitions.Add(item84);
			Grid.SetColumn(grid33, 0);
			Grid.SetRow(grid33, 2);
			TextBlock textBlock36 = new TextBlock();
			grid33.Children.Add(textBlock36);
			textBlock36.Name = "e_100";
			textBlock36.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock36.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock36.VerticalAlignment = VerticalAlignment.Center;
			textBlock36.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_ChangeValue");
			TextBlock textBlock37 = new TextBlock();
			grid33.Children.Add(textBlock37);
			textBlock37.Name = "e_101";
			textBlock37.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock37.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock37.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock37, 1);
			textBlock37.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_CreateContract");
			TextBlock textBlock38 = new TextBlock();
			grid33.Children.Add(textBlock38);
			textBlock38.Name = "e_102";
			textBlock38.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock38.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock38.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock38, 2);
			textBlock38.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
			Grid grid34 = new Grid();
			grid18.Children.Add(grid34);
			grid34.Name = "SelectHelp";
			grid34.Visibility = Visibility.Collapsed;
			grid34.VerticalAlignment = VerticalAlignment.Center;
			ColumnDefinition item85 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid34.ColumnDefinitions.Add(item85);
			ColumnDefinition item86 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid34.ColumnDefinitions.Add(item86);
			ColumnDefinition item87 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid34.ColumnDefinitions.Add(item87);
			ColumnDefinition item88 = new ColumnDefinition();
			grid34.ColumnDefinitions.Add(item88);
			Grid.SetColumn(grid34, 0);
			Grid.SetRow(grid34, 2);
			TextBlock textBlock39 = new TextBlock();
			grid34.Children.Add(textBlock39);
			textBlock39.Name = "e_103";
			textBlock39.Margin = new Thickness(0f, 0f, 5f, 0f);
			textBlock39.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock39.VerticalAlignment = VerticalAlignment.Center;
			textBlock39.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Select");
			TextBlock textBlock40 = new TextBlock();
			grid34.Children.Add(textBlock40);
			textBlock40.Name = "e_104";
			textBlock40.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock40.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock40.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock40, 1);
			textBlock40.SetResourceReference(TextBlock.TextProperty, "ContractsScreen_Help_CreateContract");
			TextBlock textBlock41 = new TextBlock();
			grid34.Children.Add(textBlock41);
			textBlock41.Name = "e_105";
			textBlock41.Margin = new Thickness(10f, 0f, 5f, 0f);
			textBlock41.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock41.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock41, 2);
			textBlock41.SetResourceReference(TextBlock.TextProperty, "Gamepad_Help_Back");
<<<<<<< HEAD
			observableCollection.Add(tabItem3);
			return observableCollection;
=======
			((Collection<object>)(object)obj).Add((object)tabItem3);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void InitializeElemente_16Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}

		private static void InitializeElemente_32Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}

		private static void InitializeElemente_46Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}
	}
}
