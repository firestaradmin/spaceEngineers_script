using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ActiveContractsView_Bindings;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class ActiveContractsView : UIRoot
	{
		private Grid rootGrid;

		private Image e_0;

		private Grid e_1;

		private ImageButton e_2;

		private StackPanel e_3;

		private TextBlock e_4;

		private Border e_5;

		private Grid e_6;

		private ListBox e_7;

		private Grid e_8;

		private TextBlock e_9;

		private Border e_10;

		private ContentPresenter e_11;

		private Border e_12;

		private Grid e_13;

		private Button e_14;

		private Button e_15;

		public ActiveContractsView()
		{
			Initialize();
		}

		public ActiveContractsView(int width, int height)
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
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionActiveContracts");
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
			ColumnDefinition columnDefinition4 = new ColumnDefinition();
			columnDefinition4.Width = new GridLength(2f, GridUnitType.Star);
			e_6.ColumnDefinitions.Add(columnDefinition4);
			ColumnDefinition columnDefinition5 = new ColumnDefinition();
			columnDefinition5.Width = new GridLength(3f, GridUnitType.Star);
			e_6.ColumnDefinitions.Add(columnDefinition5);
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 3);
			e_7 = new ListBox();
			e_6.Children.Add(e_7);
			e_7.Name = "e_7";
			e_7.Margin = new Thickness(0f, 0f, 5f, 0f);
			e_7.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_7.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			e_7.TabIndex = 0;
			e_7.SelectionMode = SelectionMode.Single;
			Binding binding4 = new Binding("ActiveContracts");
			binding4.UseGeneratedBindings = true;
			e_7.SetBinding(ItemsControl.ItemsSourceProperty, binding4);
			Binding binding5 = new Binding("SelectedActiveContractIndex");
			binding5.UseGeneratedBindings = true;
			e_7.SetBinding(Selector.SelectedIndexProperty, binding5);
			Binding binding6 = new Binding("SelectedActiveContract");
			binding6.UseGeneratedBindings = true;
			e_7.SetBinding(Selector.SelectedItemProperty, binding6);
			InitializeElemente_7Resources(e_7);
			e_8 = new Grid();
			e_6.Children.Add(e_8);
			e_8.Name = "e_8";
			RowDefinition rowDefinition12 = new RowDefinition();
			rowDefinition12.Height = new GridLength(30f, GridUnitType.Pixel);
			e_8.RowDefinitions.Add(rowDefinition12);
			RowDefinition rowDefinition13 = new RowDefinition();
			rowDefinition13.Height = new GridLength(1f, GridUnitType.Auto);
			e_8.RowDefinitions.Add(rowDefinition13);
			RowDefinition rowDefinition14 = new RowDefinition();
			rowDefinition14.Height = new GridLength(1f, GridUnitType.Star);
			e_8.RowDefinitions.Add(rowDefinition14);
			Grid.SetColumn(e_8, 0);
			Grid.SetRow(e_8, 0);
			Binding binding7 = new Binding("IsNoActiveContractVisible");
			binding7.UseGeneratedBindings = true;
			e_8.SetBinding(UIElement.VisibilityProperty, binding7);
			e_9 = new TextBlock();
			e_8.Children.Add(e_9);
			e_9.Name = "e_9";
			e_9.Margin = new Thickness(5f, 5f, 5f, 5f);
			e_9.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(e_9, 1);
			e_9.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoActiveContracts");
			e_10 = new Border();
			e_6.Children.Add(e_10);
			e_10.Name = "e_10";
			e_10.VerticalAlignment = VerticalAlignment.Stretch;
			e_10.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			e_10.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_10.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(e_10, 1);
			e_11 = new ContentPresenter();
			e_10.Child = e_11;
			e_11.Name = "e_11";
			e_11.Margin = new Thickness(5f, 0f, 0f, 0f);
			Grid.SetColumn(e_11, 1);
			Binding binding8 = new Binding("SelectedActiveContract");
			binding8.UseGeneratedBindings = true;
			e_11.SetBinding(UIElement.DataContextProperty, binding8);
			Binding binding9 = new Binding("SelectedActiveContract");
			binding9.UseGeneratedBindings = true;
			e_11.SetBinding(ContentPresenter.ContentProperty, binding9);
			e_12 = new Border();
			e_6.Children.Add(e_12);
			e_12.Name = "e_12";
			e_12.Height = 2f;
			e_12.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_12.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(e_12, 1);
			Grid.SetColumnSpan(e_12, 2);
			e_13 = new Grid();
			e_6.Children.Add(e_13);
			e_13.Name = "e_13";
			e_13.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item4 = new ColumnDefinition();
			e_13.ColumnDefinitions.Add(item4);
			ColumnDefinition columnDefinition6 = new ColumnDefinition();
			columnDefinition6.Width = new GridLength(150f, GridUnitType.Pixel);
			e_13.ColumnDefinitions.Add(columnDefinition6);
			ColumnDefinition columnDefinition7 = new ColumnDefinition();
			columnDefinition7.Width = new GridLength(150f, GridUnitType.Pixel);
			e_13.ColumnDefinitions.Add(columnDefinition7);
			Grid.SetRow(e_13, 2);
			Grid.SetColumnSpan(e_13, 2);
			e_14 = new Button();
			e_13.Children.Add(e_14);
			e_14.Name = "e_14";
			e_14.Margin = new Thickness(10f, 10f, 0f, 10f);
			e_14.VerticalAlignment = VerticalAlignment.Center;
			e_14.TabIndex = 1;
			Grid.SetColumn(e_14, 1);
			Binding binding10 = new Binding("RefreshActiveCommand");
			binding10.UseGeneratedBindings = true;
			e_14.SetBinding(Button.CommandProperty, binding10);
			e_14.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Refresh");
			e_15 = new Button();
			e_13.Children.Add(e_15);
			e_15.Name = "e_15";
			e_15.Margin = new Thickness(10f, 10f, 0f, 10f);
			e_15.VerticalAlignment = VerticalAlignment.Center;
			e_15.TabIndex = 2;
			Grid.SetColumn(e_15, 2);
			Binding binding11 = new Binding("IsAbandonEnabled");
			binding11.UseGeneratedBindings = true;
			e_15.SetBinding(UIElement.IsEnabledProperty, binding11);
			Binding binding12 = new Binding("AbandonCommand");
			binding12.UseGeneratedBindings = true;
			e_15.SetBinding(Button.CommandProperty, binding12);
			e_15.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Abandon");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "MaxWidth", typeof(MyContractsActiveViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "BackgroundOverlay", typeof(MyContractsActiveViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "ExitCommand", typeof(MyContractsActiveViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "ActiveContracts", typeof(MyContractsActiveViewModel_ActiveContracts_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "SelectedActiveContractIndex", typeof(MyContractsActiveViewModel_SelectedActiveContractIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "SelectedActiveContract", typeof(MyContractsActiveViewModel_SelectedActiveContract_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "IsNoActiveContractVisible", typeof(MyContractsActiveViewModel_IsNoActiveContractVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "RefreshActiveCommand", typeof(MyContractsActiveViewModel_RefreshActiveCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "IsAbandonEnabled", typeof(MyContractsActiveViewModel_IsAbandonEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsActiveViewModel), "AbandonCommand", typeof(MyContractsActiveViewModel_AbandonCommand_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesContracts.Instance);
		}

		private static void InitializeElemente_7Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}
	}
}
