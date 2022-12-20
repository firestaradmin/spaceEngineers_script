using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ModIoConsentView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class ModIoConsentView_Gamepad : UIRoot
	{
		private Grid rootGrid;

		private Image e_0;

		private Grid e_1;

		private ImageButton e_2;

		private StackPanel e_3;

		private TextBlock e_4;

		private Border e_5;

		private Grid e_6;

		private TextBlock e_7;

		private TextBlock e_8;

		private TextBlock e_9;

		private Grid e_10;

		private TextBlock e_11;

		private StackPanel e_12;

		private TextBlock e_13;

		private Image e_14;

		private TextBlock e_15;

		private TextBlock e_16;

		private StackPanel e_17;

		private StackPanel e_18;

		private TextBlock e_19;

		private Image e_20;

		private TextBlock e_21;

		private TextBlock e_22;

		private StackPanel e_23;

		private Border e_24;

		private StackPanel e_25;

		private TextBlock e_26;

		private TextBlock e_27;

		public ModIoConsentView_Gamepad()
		{
			Initialize();
		}

		public ModIoConsentView_Gamepad(int width, int height)
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
			rootGrid.VerticalAlignment = VerticalAlignment.Center;
			RowDefinition rowDefinition = new RowDefinition();
			rowDefinition.Height = new GridLength(0.1f, GridUnitType.Star);
			rootGrid.RowDefinitions.Add(rowDefinition);
			RowDefinition item = new RowDefinition();
			rootGrid.RowDefinitions.Add(item);
			RowDefinition rowDefinition2 = new RowDefinition();
			rowDefinition2.Height = new GridLength(0.1f, GridUnitType.Star);
			rootGrid.RowDefinitions.Add(rowDefinition2);
			Binding binding = new Binding("Width");
			binding.UseGeneratedBindings = true;
			rootGrid.SetBinding(UIElement.WidthProperty, binding);
			Binding binding2 = new Binding("Height");
			binding2.UseGeneratedBindings = true;
			rootGrid.SetBinding(UIElement.HeightProperty, binding2);
			e_0 = new Image();
			rootGrid.Children.Add(e_0);
			e_0.Name = "e_0";
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			e_0.Source = bitmapImage;
			e_0.Stretch = Stretch.Fill;
			Grid.SetRow(e_0, 1);
			Binding binding3 = new Binding("BackgroundOverlay");
			binding3.UseGeneratedBindings = true;
			e_0.SetBinding(ImageBrush.ColorOverlayProperty, binding3);
			e_1 = new Grid();
			rootGrid.Children.Add(e_1);
			e_1.Name = "e_1";
			GamepadBinding gamepadBinding = new GamepadBinding();
			gamepadBinding.Gesture = new GamepadGesture(GamepadInput.CButton);
			Binding binding4 = new Binding("AgreeCommand");
			binding4.UseGeneratedBindings = true;
			gamepadBinding.SetBinding(InputBinding.CommandProperty, binding4);
			e_1.InputBindings.Add(gamepadBinding);
			gamepadBinding.Parent = e_1;
			GamepadBinding gamepadBinding2 = new GamepadBinding();
			gamepadBinding2.Gesture = new GamepadGesture(GamepadInput.DButton);
			Binding binding5 = new Binding("OptOutCommand");
			binding5.UseGeneratedBindings = true;
			gamepadBinding2.SetBinding(InputBinding.CommandProperty, binding5);
			e_1.InputBindings.Add(gamepadBinding2);
			gamepadBinding2.Parent = e_1;
			GamepadBinding gamepadBinding3 = new GamepadBinding();
			gamepadBinding3.Gesture = new GamepadGesture(GamepadInput.StartButton);
			Binding binding6 = new Binding("ModioTermsOfUseCommand");
			binding6.UseGeneratedBindings = true;
			gamepadBinding3.SetBinding(InputBinding.CommandProperty, binding6);
			e_1.InputBindings.Add(gamepadBinding3);
			gamepadBinding3.Parent = e_1;
			GamepadBinding gamepadBinding4 = new GamepadBinding();
			gamepadBinding4.Gesture = new GamepadGesture(GamepadInput.SelectButton);
			Binding binding7 = new Binding("ModioPrivacyPolicyCommand");
			binding7.UseGeneratedBindings = true;
			gamepadBinding4.SetBinding(InputBinding.CommandProperty, binding7);
			e_1.InputBindings.Add(gamepadBinding4);
			gamepadBinding4.Parent = e_1;
			GamepadBinding gamepadBinding5 = new GamepadBinding();
			gamepadBinding5.Gesture = new GamepadGesture(GamepadInput.LeftStickButton);
			Binding binding8 = new Binding("SteamTermsOfUseCommand");
			binding8.UseGeneratedBindings = true;
			gamepadBinding5.SetBinding(InputBinding.CommandProperty, binding8);
			e_1.InputBindings.Add(gamepadBinding5);
			gamepadBinding5.Parent = e_1;
			GamepadBinding gamepadBinding6 = new GamepadBinding();
			gamepadBinding6.Gesture = new GamepadGesture(GamepadInput.RightStickButton);
			Binding binding9 = new Binding("SteamPrivacyPolicyCommand");
			binding9.UseGeneratedBindings = true;
			gamepadBinding6.SetBinding(InputBinding.CommandProperty, binding9);
			e_1.InputBindings.Add(gamepadBinding6);
			gamepadBinding6.Parent = e_1;
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
			e_2.TabIndex = 1;
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
			Binding binding10 = new Binding("ConsentCaption");
			binding10.UseGeneratedBindings = true;
			e_4.SetBinding(TextBlock.TextProperty, binding10);
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
			RowDefinition rowDefinition11 = new RowDefinition();
			rowDefinition11.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition11);
			RowDefinition rowDefinition12 = new RowDefinition();
			rowDefinition12.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition12);
			RowDefinition rowDefinition13 = new RowDefinition();
			rowDefinition13.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition13);
			RowDefinition rowDefinition14 = new RowDefinition();
			rowDefinition14.Height = new GridLength(1f, GridUnitType.Auto);
			e_6.RowDefinitions.Add(rowDefinition14);
			RowDefinition rowDefinition15 = new RowDefinition();
			rowDefinition15.Height = new GridLength(48f, GridUnitType.Pixel);
			e_6.RowDefinitions.Add(rowDefinition15);
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 3);
			e_7 = new TextBlock();
			e_6.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(0f, 20f, 0f, 20f);
			e_7.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(e_7, 0);
			Binding binding11 = new Binding("ConsentTextPart1");
			binding11.UseGeneratedBindings = true;
			e_7.SetBinding(TextBlock.TextProperty, binding11);
			e_8 = new TextBlock();
			e_6.Children.Add(e_8);
			e_8.Name = "e_8";
			e_8.Margin = new Thickness(20f, 20f, 20f, 20f);
			Grid.SetRow(e_8, 1);
			Binding binding12 = new Binding("ConsentTextPart2");
			binding12.UseGeneratedBindings = true;
			e_8.SetBinding(TextBlock.TextProperty, binding12);
			e_9 = new TextBlock();
			e_6.Children.Add(e_9);
			e_9.Name = "e_9";
			e_9.Margin = new Thickness(0f, 20f, 0f, 20f);
			e_9.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(e_9, 2);
			Binding binding13 = new Binding("ConsentTextPart3");
			binding13.UseGeneratedBindings = true;
			e_9.SetBinding(TextBlock.TextProperty, binding13);
			e_10 = new Grid();
			e_6.Children.Add(e_10);
			e_10.Name = "e_10";
			RowDefinition rowDefinition16 = new RowDefinition();
			rowDefinition16.Height = new GridLength(40f, GridUnitType.Pixel);
			e_10.RowDefinitions.Add(rowDefinition16);
			RowDefinition rowDefinition17 = new RowDefinition();
			rowDefinition17.Height = new GridLength(1f, GridUnitType.Auto);
			e_10.RowDefinitions.Add(rowDefinition17);
			RowDefinition rowDefinition18 = new RowDefinition();
			rowDefinition18.Height = new GridLength(1f, GridUnitType.Auto);
			e_10.RowDefinitions.Add(rowDefinition18);
			RowDefinition rowDefinition19 = new RowDefinition();
			rowDefinition19.Height = new GridLength(1f, GridUnitType.Auto);
			e_10.RowDefinitions.Add(rowDefinition19);
			RowDefinition rowDefinition20 = new RowDefinition();
			rowDefinition20.Height = new GridLength(1f, GridUnitType.Auto);
			e_10.RowDefinitions.Add(rowDefinition20);
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(1f, GridUnitType.Star);
			e_10.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(1f, GridUnitType.Star);
			e_10.ColumnDefinitions.Add(columnDefinition5);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(1f, GridUnitType.Star);
			e_10.ColumnDefinitions.Add(columnDefinition6);
			Grid.SetColumn(e_10, 0);
			Grid.SetRow(e_10, 3);
			e_11 = new TextBlock();
			e_10.Children.Add(e_11);
			e_11.Name = "e_11";
			e_11.Margin = new Thickness(3f, 10f, 3f, 10f);
			e_11.HorizontalAlignment = HorizontalAlignment.Right;
			e_11.Foreground = new SolidColorBrush(new ColorW(198, 44, 20, 255));
			e_11.TextWrapping = TextWrapping.NoWrap;
			Grid.SetColumn(e_11, 1);
			Grid.SetRow(e_11, 0);
			Grid.SetColumnSpan(e_11, 2);
			Binding binding14 = new Binding("WarningVisible");
			binding14.UseGeneratedBindings = true;
			e_11.SetBinding(UIElement.VisibilityProperty, binding14);
			e_11.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_LabelReadTOU");
			e_12 = new StackPanel();
			e_10.Children.Add(e_12);
			e_12.Name = "e_12";
			e_12.Height = 28f;
			e_12.HorizontalAlignment = HorizontalAlignment.Right;
			e_12.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_12, 0);
			Grid.SetRow(e_12, 1);
			Grid.SetColumnSpan(e_12, 3);
			e_13 = new TextBlock();
			e_12.Children.Add(e_13);
			e_13.Name = "e_13";
			e_13.Margin = new Thickness(0f, 0f, 15f, 0f);
			e_13.TextWrapping = TextWrapping.NoWrap;
			e_13.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_LabelModIo");
			e_14 = new Image();
			e_12.Children.Add(e_14);
			e_14.Name = "e_14";
			e_14.Height = 20f;
			e_14.Width = 20f;
			e_14.Margin = new Thickness(3f, 0f, 0f, 0f);
			BitmapImage bitmapImage5 = new BitmapImage();
			bitmapImage5.TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png";
			e_14.Source = bitmapImage5;
			e_14.Stretch = Stretch.Uniform;
			Binding binding15 = new Binding("ModioTOURequired");
			binding15.UseGeneratedBindings = true;
			e_14.SetBinding(UIElement.VisibilityProperty, binding15);
			e_15 = new TextBlock();
			e_12.Children.Add(e_15);
			e_15.Name = "e_15";
			e_15.TextWrapping = TextWrapping.NoWrap;
			e_15.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_TermsOfUseModioHelp");
			e_16 = new TextBlock();
			e_12.Children.Add(e_16);
			e_16.Name = "e_16";
			e_16.Margin = new Thickness(20f, 0f, 0f, 0f);
			e_16.TextWrapping = TextWrapping.NoWrap;
			e_16.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_PrivacyPolicyModioHelp");
			e_17 = new StackPanel();
			e_10.Children.Add(e_17);
			e_17.Name = "e_17";
			e_17.Height = 20f;
			Grid.SetColumn(e_17, 0);
			Grid.SetRow(e_17, 2);
			Binding binding16 = new Binding("SteamControls");
			binding16.UseGeneratedBindings = true;
			e_17.SetBinding(UIElement.VisibilityProperty, binding16);
			e_18 = new StackPanel();
			e_10.Children.Add(e_18);
			e_18.Name = "e_18";
			e_18.HorizontalAlignment = HorizontalAlignment.Right;
			e_18.Orientation = Orientation.Horizontal;
			Grid.SetColumn(e_18, 0);
			Grid.SetRow(e_18, 3);
			Grid.SetColumnSpan(e_18, 3);
			e_19 = new TextBlock();
			e_18.Children.Add(e_19);
			e_19.Name = "e_19";
			e_19.Height = 28f;
			e_19.Margin = new Thickness(0f, 0f, 15f, 0f);
			e_19.TextWrapping = TextWrapping.NoWrap;
			Binding binding17 = new Binding("SteamControls");
			binding17.UseGeneratedBindings = true;
			e_19.SetBinding(UIElement.VisibilityProperty, binding17);
			e_19.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_LabelSteam");
			e_20 = new Image();
			e_18.Children.Add(e_20);
			e_20.Name = "e_20";
			e_20.Height = 20f;
			e_20.Width = 20f;
			e_20.Margin = new Thickness(3f, 0f, 0f, 0f);
			BitmapImage bitmapImage6 = new BitmapImage();
			bitmapImage6.TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png";
			e_20.Source = bitmapImage6;
			e_20.Stretch = Stretch.Uniform;
			Binding binding18 = new Binding("SteamControls");
			binding18.UseGeneratedBindings = true;
			e_20.SetBinding(UIElement.VisibilityProperty, binding18);
			e_21 = new TextBlock();
			e_18.Children.Add(e_21);
			e_21.Name = "e_21";
			e_21.Height = 28f;
			e_21.TextWrapping = TextWrapping.NoWrap;
			Binding binding19 = new Binding("SteamControls");
			binding19.UseGeneratedBindings = true;
			e_21.SetBinding(UIElement.VisibilityProperty, binding19);
			e_21.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_TermsOfUseSteamHelp");
			e_22 = new TextBlock();
			e_18.Children.Add(e_22);
			e_22.Name = "e_22";
			e_22.Height = 28f;
			e_22.Margin = new Thickness(20f, 0f, 0f, 0f);
			e_22.TextWrapping = TextWrapping.NoWrap;
			Binding binding20 = new Binding("SteamControls");
			binding20.UseGeneratedBindings = true;
			e_22.SetBinding(UIElement.VisibilityProperty, binding20);
			e_22.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_PrivacyPolicySteamHelp");
			e_23 = new StackPanel();
			e_6.Children.Add(e_23);
			e_23.Name = "e_23";
			Grid.SetRow(e_23, 4);
			e_24 = new Border();
			e_23.Children.Add(e_24);
			e_24.Name = "e_24";
			e_24.Height = 2f;
			e_24.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_24.VerticalAlignment = VerticalAlignment.Bottom;
			e_24.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_25 = new StackPanel();
			e_6.Children.Add(e_25);
			e_25.Name = "e_25";
			e_25.HorizontalAlignment = HorizontalAlignment.Center;
			e_25.VerticalAlignment = VerticalAlignment.Center;
			e_25.Orientation = Orientation.Horizontal;
			Grid.SetRow(e_25, 5);
			e_26 = new TextBlock();
			e_25.Children.Add(e_26);
			e_26.Name = "e_26";
			e_26.Margin = new Thickness(40f, 0f, 40f, 0f);
			e_26.HorizontalAlignment = HorizontalAlignment.Center;
			Binding binding21 = new Binding("AgreeHelpTextForeground");
			binding21.UseGeneratedBindings = true;
			e_26.SetBinding(Control.ForegroundProperty, binding21);
			e_26.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_AgreeHelpText");
			e_27 = new TextBlock();
			e_25.Children.Add(e_27);
			e_27.Name = "e_27";
			e_27.Margin = new Thickness(40f, 0f, 40f, 0f);
			e_27.HorizontalAlignment = HorizontalAlignment.Center;
			e_27.SetResourceReference(TextBlock.TextProperty, "ScreenModIoConsent_OptOutHelpText");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "Width", typeof(MyModIoConsentViewModel_Width_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "Height", typeof(MyModIoConsentViewModel_Height_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "BackgroundOverlay", typeof(MyModIoConsentViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "AgreeCommand", typeof(MyModIoConsentViewModel_AgreeCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "OptOutCommand", typeof(MyModIoConsentViewModel_OptOutCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ModioTermsOfUseCommand", typeof(MyModIoConsentViewModel_ModioTermsOfUseCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ModioPrivacyPolicyCommand", typeof(MyModIoConsentViewModel_ModioPrivacyPolicyCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "SteamTermsOfUseCommand", typeof(MyModIoConsentViewModel_SteamTermsOfUseCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "SteamPrivacyPolicyCommand", typeof(MyModIoConsentViewModel_SteamPrivacyPolicyCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ConsentCaption", typeof(MyModIoConsentViewModel_ConsentCaption_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ConsentTextPart1", typeof(MyModIoConsentViewModel_ConsentTextPart1_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ConsentTextPart2", typeof(MyModIoConsentViewModel_ConsentTextPart2_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ConsentTextPart3", typeof(MyModIoConsentViewModel_ConsentTextPart3_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "WarningVisible", typeof(MyModIoConsentViewModel_WarningVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "ModioTOURequired", typeof(MyModIoConsentViewModel_ModioTOURequired_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "SteamControls", typeof(MyModIoConsentViewModel_SteamControls_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyModIoConsentViewModel), "AgreeHelpTextForeground", typeof(MyModIoConsentViewModel_AgreeHelpTextForeground_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
		}
	}
}
