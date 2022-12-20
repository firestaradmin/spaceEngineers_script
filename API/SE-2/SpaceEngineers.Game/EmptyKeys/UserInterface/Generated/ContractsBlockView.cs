using System.CodeDom.Compiler;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ContractsBlockView_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class ContractsBlockView : UIRoot
	{
		private Grid rootGrid;

		private Image e_0;

		private Grid e_1;

		private ImageButton e_2;

		private StackPanel e_3;

		private TextBlock e_4;

		private Border e_5;

		private TabControl e_6;

		public ContractsBlockView()
		{
			Initialize();
		}

		public ContractsBlockView(int width, int height)
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
			e_4.SetResourceReference(TextBlock.TextProperty, "ScreenCaptionContracts");
			e_5 = new Border();
			e_3.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.Height = 2f;
			e_5.Margin = new Thickness(0f, 10f, 0f, 10f);
			e_5.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			e_6 = new TabControl();
			e_1.Children.Add(e_6);
			e_6.Name = "e_6";
			e_6.TabIndex = 0;
<<<<<<< HEAD
			e_6.ItemsSource = Get_e_6_Items();
=======
			e_6.ItemsSource = (IEnumerable)Get_e_6_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Grid.SetColumn(e_6, 1);
			Grid.SetRow(e_6, 3);
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "MaxWidth", typeof(MyContractsBlockViewModel_MaxWidth_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "BackgroundOverlay", typeof(MyContractsBlockViewModel_BackgroundOverlay_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "ExitCommand", typeof(MyContractsBlockViewModel_ExitCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "FilterTargets", typeof(MyContractsBlockViewModel_FilterTargets_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "FilterTargetIndex", typeof(MyContractsBlockViewModel_FilterTargetIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "AvailableContracts", typeof(MyContractsBlockViewModel_AvailableContracts_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "SelectedAvailableContractIndex", typeof(MyContractsBlockViewModel_SelectedAvailableContractIndex_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "SelectedAvailableContract", typeof(MyContractsBlockViewModel_SelectedAvailableContract_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "IsNoAvailableContractVisible", typeof(MyContractsBlockViewModel_IsNoAvailableContractVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "RefreshAvailableCommand", typeof(MyContractsBlockViewModel_RefreshAvailableCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "IsAcceptEnabled", typeof(MyContractsBlockViewModel_IsAcceptEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "AcceptCommand", typeof(MyContractsBlockViewModel_AcceptCommand_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "ActiveViewModel", typeof(MyContractsBlockViewModel_ActiveViewModel_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "IsAdministrationVisible", typeof(MyContractsBlockViewModel_IsAdministrationVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractsBlockViewModel), "AdministrationViewModel", typeof(MyContractsBlockViewModel_AdministrationViewModel_PropertyInfo));
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
			elem.Resources.MergedDictionaries.Add(DataTemplatesContracts.Instance);
		}

		private static ObservableCollection<object> Get_e_6_Items()
		{
<<<<<<< HEAD
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem = new TabItem();
			tabItem.Name = "e_7";
			tabItem.SetResourceReference(HeaderedContentControl.HeaderProperty, "ContractScreen_Tab_AvailableContracts");
			Grid grid2 = (Grid)(tabItem.Content = new Grid());
			grid2.Name = "e_8";
			RowDefinition item = new RowDefinition();
			grid2.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid2.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			grid3.Name = "e_9";
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid3.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition();
			grid3.RowDefinitions.Add(item7);
			Grid grid4 = new Grid();
			grid3.Children.Add(grid4);
			grid4.Name = "e_10";
			ColumnDefinition item8 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid4.ColumnDefinitions.Add(item8);
			ColumnDefinition item9 = new ColumnDefinition();
			grid4.ColumnDefinitions.Add(item9);
			TextBlock textBlock = new TextBlock();
			grid4.Children.Add(textBlock);
			textBlock.Name = "e_11";
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 0);
			textBlock.SetResourceReference(TextBlock.TextProperty, "ContractScreen_ContractFilterTitle");
			ComboBox comboBox = new ComboBox();
			grid4.Children.Add(comboBox);
			comboBox.Name = "e_12";
			comboBox.Margin = new Thickness(5f, 10f, 5f, 10f);
			comboBox.VerticalAlignment = VerticalAlignment.Center;
			comboBox.TabIndex = 1;
			comboBox.MaxDropDownHeight = 240f;
			Grid.SetColumn(comboBox, 1);
			comboBox.SetBinding(binding: new Binding("FilterTargets")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			comboBox.SetBinding(binding: new Binding("FilterTargetIndex")
			{
				UseGeneratedBindings = true
			}, property: Selector.SelectedIndexProperty);
			Grid grid5 = new Grid();
			grid3.Children.Add(grid5);
			grid5.Name = "e_13";
			Grid.SetRow(grid5, 1);
			ListBox listBox = new ListBox();
			grid5.Children.Add(listBox);
			listBox.Name = "e_14";
			listBox.Margin = new Thickness(0f, 0f, 5f, 0f);
			listBox.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox.TabIndex = 2;
			listBox.SelectionMode = SelectionMode.Single;
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
			InitializeElemente_14Resources(listBox);
			Grid grid6 = new Grid();
			grid5.Children.Add(grid6);
			grid6.Name = "e_15";
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
			textBlock2.Name = "e_16";
			textBlock2.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(textBlock2, 1);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoAvailableContracts");
			Border border = new Border();
			grid2.Children.Add(border);
			border.Name = "e_17";
			border.VerticalAlignment = VerticalAlignment.Stretch;
			border.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 1);
			ContentPresenter contentPresenter = (ContentPresenter)(border.Child = new ContentPresenter());
			contentPresenter.Name = "e_18";
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
			border2.Name = "e_19";
			border2.Height = 2f;
			border2.Margin = new Thickness(0f, 10f, 0f, 10f);
			border2.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border2, 1);
			Grid.SetColumnSpan(border2, 2);
			Grid grid7 = new Grid();
			grid2.Children.Add(grid7);
			grid7.Name = "e_20";
			grid7.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item13 = new ColumnDefinition();
			grid7.ColumnDefinitions.Add(item13);
			ColumnDefinition item14 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid7.ColumnDefinitions.Add(item14);
			ColumnDefinition item15 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid7.ColumnDefinitions.Add(item15);
			Grid.SetRow(grid7, 2);
			Grid.SetColumnSpan(grid7, 2);
			Button button = new Button();
			grid7.Children.Add(button);
			button.Name = "e_21";
			button.Margin = new Thickness(10f, 10f, 0f, 10f);
			button.VerticalAlignment = VerticalAlignment.Center;
			button.TabIndex = 3;
			Grid.SetColumn(button, 1);
			button.SetBinding(binding: new Binding("RefreshAvailableCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Refresh");
			Button button2 = new Button();
			grid7.Children.Add(button2);
			button2.Name = "e_22";
			button2.Margin = new Thickness(10f, 10f, 0f, 10f);
			button2.VerticalAlignment = VerticalAlignment.Center;
			button2.TabIndex = 4;
			Grid.SetColumn(button2, 2);
			button2.SetBinding(binding: new Binding("IsAcceptEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			button2.SetBinding(binding: new Binding("AcceptCommand")
			{
				UseGeneratedBindings = true
			}, property: Button.CommandProperty);
			button2.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Accept");
<<<<<<< HEAD
			observableCollection.Add(tabItem);
=======
			((Collection<object>)(object)obj).Add((object)tabItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem2 = new TabItem();
			tabItem2.Name = "e_23";
			tabItem2.SetResourceReference(HeaderedContentControl.HeaderProperty, "ContractScreen_Tab_AcceptedContracts");
			Grid grid9 = (Grid)(tabItem2.Content = new Grid());
			grid9.Name = "e_24";
			grid9.SetBinding(binding: new Binding("ActiveViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid10 = new Grid();
			grid9.Children.Add(grid10);
			grid10.Name = "e_25";
			Grid grid11 = new Grid();
			grid10.Children.Add(grid11);
			grid11.Name = "e_26";
			RowDefinition item16 = new RowDefinition();
			grid11.RowDefinitions.Add(item16);
			RowDefinition item17 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.RowDefinitions.Add(item17);
			RowDefinition item18 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid11.RowDefinitions.Add(item18);
			ColumnDefinition item19 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid11.ColumnDefinitions.Add(item19);
			ColumnDefinition item20 = new ColumnDefinition
			{
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid11.ColumnDefinitions.Add(item20);
			ListBox listBox2 = new ListBox();
			grid11.Children.Add(listBox2);
			listBox2.Name = "e_27";
			listBox2.Margin = new Thickness(0f, 0f, 5f, 0f);
			listBox2.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox2.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox2.TabIndex = 50;
			listBox2.SelectionMode = SelectionMode.Single;
			listBox2.SetBinding(binding: new Binding("ActiveContracts"), property: ItemsControl.ItemsSourceProperty);
			listBox2.SetBinding(binding: new Binding("SelectedActiveContractIndex"), property: Selector.SelectedIndexProperty);
			listBox2.SetBinding(binding: new Binding("SelectedActiveContract"), property: Selector.SelectedItemProperty);
			InitializeElemente_27Resources(listBox2);
			Grid grid12 = new Grid();
			grid11.Children.Add(grid12);
			grid12.Name = "e_28";
			RowDefinition item21 = new RowDefinition
			{
				Height = new GridLength(30f, GridUnitType.Pixel)
			};
			grid12.RowDefinitions.Add(item21);
			RowDefinition item22 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid12.RowDefinitions.Add(item22);
			RowDefinition item23 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid12.RowDefinitions.Add(item23);
			Grid.SetColumn(grid12, 0);
			Grid.SetRow(grid12, 0);
			grid12.SetBinding(binding: new Binding("IsNoActiveContractVisible"), property: UIElement.VisibilityProperty);
			TextBlock textBlock3 = new TextBlock();
			grid12.Children.Add(textBlock3);
			textBlock3.Name = "e_29";
			textBlock3.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock3.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(textBlock3, 1);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoActiveContracts");
			Border border3 = new Border();
			grid11.Children.Add(border3);
			border3.Name = "e_30";
			border3.VerticalAlignment = VerticalAlignment.Stretch;
			border3.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			border3.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border3, 1);
			ContentPresenter contentPresenter2 = (ContentPresenter)(border3.Child = new ContentPresenter());
			contentPresenter2.Name = "e_31";
			contentPresenter2.Margin = new Thickness(5f, 0f, 0f, 0f);
			Grid.SetColumn(contentPresenter2, 1);
			contentPresenter2.SetBinding(binding: new Binding("SelectedActiveContract"), property: UIElement.DataContextProperty);
			contentPresenter2.SetBinding(binding: new Binding("SelectedActiveContract"), property: ContentPresenter.ContentProperty);
			Border border4 = new Border();
			grid11.Children.Add(border4);
			border4.Name = "e_32";
			border4.Height = 2f;
			border4.Margin = new Thickness(0f, 10f, 0f, 10f);
			border4.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border4, 1);
			Grid.SetColumnSpan(border4, 2);
			Grid grid13 = new Grid();
			grid11.Children.Add(grid13);
			grid13.Name = "e_33";
			grid13.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item24 = new ColumnDefinition();
			grid13.ColumnDefinitions.Add(item24);
			ColumnDefinition item25 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid13.ColumnDefinitions.Add(item25);
			ColumnDefinition item26 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid13.ColumnDefinitions.Add(item26);
			ColumnDefinition item27 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid13.ColumnDefinitions.Add(item27);
			Grid.SetRow(grid13, 2);
			Grid.SetColumnSpan(grid13, 2);
			Button button3 = new Button();
			grid13.Children.Add(button3);
			button3.Name = "e_34";
			button3.Margin = new Thickness(10f, 10f, 0f, 10f);
			button3.VerticalAlignment = VerticalAlignment.Center;
			button3.TabIndex = 51;
			Grid.SetColumn(button3, 1);
			button3.SetBinding(binding: new Binding("RefreshActiveCommand"), property: Button.CommandProperty);
			button3.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Refresh");
			Button button4 = new Button();
			grid13.Children.Add(button4);
			button4.Name = "e_35";
			button4.Margin = new Thickness(10f, 10f, 0f, 10f);
			button4.VerticalAlignment = VerticalAlignment.Center;
			button4.TabIndex = 52;
			Grid.SetColumn(button4, 2);
			button4.SetBinding(binding: new Binding("IsAbandonEnabled"), property: UIElement.IsEnabledProperty);
			button4.SetBinding(binding: new Binding("AbandonCommand"), property: Button.CommandProperty);
			button4.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Abandon");
			Button button5 = new Button();
			grid13.Children.Add(button5);
			button5.Name = "e_36";
			button5.Margin = new Thickness(10f, 10f, 0f, 10f);
			button5.VerticalAlignment = VerticalAlignment.Center;
			button5.TabIndex = 53;
			Grid.SetColumn(button5, 3);
			button5.SetBinding(binding: new Binding("FinishTooltipText"), property: UIElement.ToolTipProperty);
			button5.SetBinding(binding: new Binding("IsFinishEnabled"), property: UIElement.IsEnabledProperty);
			button5.SetBinding(binding: new Binding("FinishCommand"), property: Button.CommandProperty);
			button5.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Finish");
			Grid grid14 = new Grid();
			grid10.Children.Add(grid14);
			grid14.Name = "e_37";
			KeyBinding keyBinding = new KeyBinding
			{
				Gesture = new KeyGesture(KeyCode.Escape, ModifierKeys.None, "")
			};
			keyBinding.SetBinding(binding: new Binding("ExitGridSelectionCommand"), property: InputBinding.CommandProperty);
			grid14.InputBindings.Add(keyBinding);
			keyBinding.Parent = grid14;
			RowDefinition item28 = new RowDefinition
			{
				Height = new GridLength(0.5f, GridUnitType.Star)
			};
			grid14.RowDefinitions.Add(item28);
			RowDefinition item29 = new RowDefinition
			{
				Height = new GridLength(2f, GridUnitType.Star)
			};
			grid14.RowDefinitions.Add(item29);
			RowDefinition item30 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid14.RowDefinitions.Add(item30);
			ColumnDefinition item31 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid14.ColumnDefinitions.Add(item31);
			ColumnDefinition item32 = new ColumnDefinition
			{
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid14.ColumnDefinitions.Add(item32);
			ColumnDefinition item33 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid14.ColumnDefinitions.Add(item33);
			Grid.SetColumn(grid14, 1);
			Grid.SetRow(grid14, 3);
			grid14.SetBinding(binding: new Binding("IsVisibleGridSelection"), property: UIElement.VisibilityProperty);
			Image image = new Image();
			grid14.Children.Add(image);
			image.Name = "e_38";
			image.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
			};
			image.Stretch = Stretch.Fill;
			Grid.SetColumn(image, 1);
			Grid.SetRow(image, 1);
			ImageBrush.SetColorOverlay(image, new ColorW(255, 255, 255, 255));
			Grid grid15 = new Grid();
			grid14.Children.Add(grid15);
			grid15.Name = "e_39";
			RowDefinition item34 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.RowDefinitions.Add(item34);
			RowDefinition item35 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.RowDefinitions.Add(item35);
			RowDefinition item36 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.RowDefinitions.Add(item36);
			RowDefinition item37 = new RowDefinition
			{
				Height = new GridLength(2.5f, GridUnitType.Star)
			};
			grid15.RowDefinitions.Add(item37);
			RowDefinition item38 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.RowDefinitions.Add(item38);
			RowDefinition item39 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.RowDefinitions.Add(item39);
			RowDefinition item40 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid15.RowDefinitions.Add(item40);
			ColumnDefinition item41 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid15.ColumnDefinitions.Add(item41);
			ColumnDefinition item42 = new ColumnDefinition
			{
				Width = new GridLength(12f, GridUnitType.Star)
			};
			grid15.ColumnDefinitions.Add(item42);
			ColumnDefinition item43 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid15.ColumnDefinitions.Add(item43);
			Grid.SetColumn(grid15, 1);
			Grid.SetRow(grid15, 1);
			TextBlock textBlock4 = new TextBlock();
			grid15.Children.Add(textBlock4);
			textBlock4.Name = "e_40";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock4, 1);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_GridSelection_Caption");
			Border border5 = new Border();
			grid15.Children.Add(border5);
			border5.Name = "e_41";
			border5.Height = 2f;
			border5.Margin = new Thickness(10f, 10f, 10f, 10f);
			border5.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(border5, 1);
			Grid.SetRow(border5, 2);
			TextBlock textBlock5 = new TextBlock();
			grid15.Children.Add(textBlock5);
			textBlock5.Name = "e_42";
			textBlock5.Margin = new Thickness(10f, 10f, 10f, 10f);
			textBlock5.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.TextWrapping = TextWrapping.Wrap;
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 3);
			textBlock5.SetResourceReference(TextBlock.TextProperty, "ContractScreen_GridSelection_Text");
			ComboBox comboBox2 = new ComboBox();
			grid15.Children.Add(comboBox2);
			comboBox2.Name = "e_43";
			comboBox2.Width = float.NaN;
			comboBox2.Margin = new Thickness(10f, 10f, 10f, 10f);
			comboBox2.HorizontalAlignment = HorizontalAlignment.Stretch;
			Grid.SetColumn(comboBox2, 1);
			Grid.SetRow(comboBox2, 4);
			comboBox2.SetBinding(binding: new Binding("SelectableTargets"), property: ItemsControl.ItemsSourceProperty);
			comboBox2.SetBinding(binding: new Binding("SelectedTargetIndex"), property: Selector.SelectedIndexProperty);
			Button button6 = new Button();
			grid15.Children.Add(button6);
			button6.Name = "e_44";
			button6.Width = 150f;
			button6.Margin = new Thickness(10f, 10f, 10f, 10f);
			button6.HorizontalAlignment = HorizontalAlignment.Right;
			button6.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(button6, 1);
			Grid.SetRow(button6, 5);
			button6.SetBinding(binding: new Binding("ConfirmGridSelectionCommand"), property: Button.CommandProperty);
			button6.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Confirm");
			ImageButton imageButton = new ImageButton();
			grid15.Children.Add(imageButton);
			imageButton.Name = "e_45";
			imageButton.Height = 24f;
			imageButton.Width = 24f;
			imageButton.Margin = new Thickness(16f, 16f, 16f, 16f);
			imageButton.HorizontalAlignment = HorizontalAlignment.Right;
			imageButton.VerticalAlignment = VerticalAlignment.Center;
			imageButton.ImageStretch = Stretch.Uniform;
			imageButton.ImageNormal = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
			};
			imageButton.ImageHover = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
			};
			imageButton.ImagePressed = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
			};
			Grid.SetColumn(imageButton, 2);
			imageButton.SetBinding(binding: new Binding("ExitGridSelectionCommand"), property: Button.CommandProperty);
<<<<<<< HEAD
			observableCollection.Add(tabItem2);
=======
			((Collection<object>)(object)obj).Add((object)tabItem2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TabItem tabItem3 = new TabItem
			{
				Name = "e_46"
			};
			tabItem3.SetBinding(binding: new Binding("IsAdministrationVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			tabItem3.SetResourceReference(HeaderedContentControl.HeaderProperty, "ContractScreen_Tab_Administration");
			Grid grid17 = (Grid)(tabItem3.Content = new Grid());
			grid17.Name = "e_47";
			grid17.SetBinding(binding: new Binding("AdministrationViewModel")
			{
				UseGeneratedBindings = true
			}, property: UIElement.DataContextProperty);
			Grid grid18 = new Grid();
			grid17.Children.Add(grid18);
			grid18.Name = "e_48";
			Grid grid19 = new Grid();
			grid18.Children.Add(grid19);
			grid19.Name = "e_49";
			RowDefinition item44 = new RowDefinition();
			grid19.RowDefinitions.Add(item44);
			RowDefinition item45 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid19.RowDefinitions.Add(item45);
			RowDefinition item46 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid19.RowDefinitions.Add(item46);
			RowDefinition item47 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid19.RowDefinitions.Add(item47);
			ColumnDefinition item48 = new ColumnDefinition
			{
				Width = new GridLength(2f, GridUnitType.Star)
			};
			grid19.ColumnDefinitions.Add(item48);
			ColumnDefinition item49 = new ColumnDefinition
			{
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid19.ColumnDefinitions.Add(item49);
			ListBox listBox3 = new ListBox();
			grid19.Children.Add(listBox3);
			listBox3.Name = "e_50";
			listBox3.Margin = new Thickness(0f, 0f, 5f, 0f);
			listBox3.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			listBox3.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			listBox3.SelectionMode = SelectionMode.Single;
			listBox3.SetBinding(binding: new Binding("AdministrableContracts"), property: ItemsControl.ItemsSourceProperty);
			listBox3.SetBinding(binding: new Binding("SelectedAdministrableContract"), property: Selector.SelectedItemProperty);
			InitializeElemente_50Resources(listBox3);
			Grid grid20 = new Grid();
			grid19.Children.Add(grid20);
			grid20.Name = "e_51";
			RowDefinition item50 = new RowDefinition
			{
				Height = new GridLength(30f, GridUnitType.Pixel)
			};
			grid20.RowDefinitions.Add(item50);
			RowDefinition item51 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid20.RowDefinitions.Add(item51);
			RowDefinition item52 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid20.RowDefinitions.Add(item52);
			Grid.SetColumn(grid20, 0);
			Grid.SetRow(grid20, 0);
			grid20.SetBinding(binding: new Binding("IsNoAdministrableContractVisible"), property: UIElement.VisibilityProperty);
			TextBlock textBlock6 = new TextBlock();
			grid20.Children.Add(textBlock6);
			textBlock6.Name = "e_52";
			textBlock6.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock6.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(textBlock6, 1);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NoAdministrableContracts");
			Grid grid21 = new Grid();
			grid19.Children.Add(grid21);
			grid21.Name = "e_53";
			grid21.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item53 = new ColumnDefinition();
			grid21.ColumnDefinitions.Add(item53);
			ColumnDefinition item54 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid21.ColumnDefinitions.Add(item54);
			ColumnDefinition item55 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid21.ColumnDefinitions.Add(item55);
			Grid.SetColumn(grid21, 0);
			Grid.SetRow(grid21, 2);
			Button button7 = new Button();
			grid21.Children.Add(button7);
			button7.Name = "e_54";
			button7.Margin = new Thickness(10f, 10f, 0f, 10f);
			button7.VerticalAlignment = VerticalAlignment.Center;
			button7.TabIndex = 101;
			Grid.SetColumn(button7, 1);
			button7.SetBinding(binding: new Binding("RefreshCommand"), property: Button.CommandProperty);
			button7.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_RefreshContracts");
			Button button8 = new Button();
			grid21.Children.Add(button8);
			button8.Name = "e_55";
			button8.Margin = new Thickness(10f, 10f, 5f, 10f);
			button8.VerticalAlignment = VerticalAlignment.Center;
			button8.TabIndex = 102;
			Grid.SetColumn(button8, 2);
			button8.SetBinding(binding: new Binding("IsDeleteEnabled"), property: UIElement.IsEnabledProperty);
			button8.SetBinding(binding: new Binding("DeleteCommand"), property: Button.CommandProperty);
			button8.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_DeleteContract");
			Border border6 = new Border();
			grid19.Children.Add(border6);
			border6.Name = "e_56";
			border6.VerticalAlignment = VerticalAlignment.Stretch;
			border6.Background = new SolidColorBrush(new ColorW(33, 44, 53, 255));
			border6.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border6.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border6, 1);
			Grid grid22 = (Grid)(border6.Child = new Grid());
			grid22.Name = "e_57";
			RowDefinition item56 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid22.RowDefinitions.Add(item56);
			RowDefinition item57 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid22.RowDefinitions.Add(item57);
			RowDefinition item58 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid22.RowDefinitions.Add(item58);
			StackPanel stackPanel = new StackPanel();
			grid22.Children.Add(stackPanel);
			stackPanel.Name = "e_58";
			stackPanel.Margin = new Thickness(10f, 10f, 0f, 0f);
			stackPanel.Orientation = Orientation.Vertical;
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock7 = new TextBlock();
			stackPanel.Children.Add(textBlock7);
			textBlock7.Name = "e_59";
			textBlock7.Margin = new Thickness(15f, 0f, 0f, 0f);
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock7.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_NewContract");
			Grid grid23 = new Grid();
			stackPanel.Children.Add(grid23);
			grid23.Name = "e_60";
			grid23.Margin = new Thickness(15f, 0f, 0f, 0f);
			RowDefinition item59 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid23.RowDefinitions.Add(item59);
			RowDefinition item60 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid23.RowDefinitions.Add(item60);
			RowDefinition item61 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid23.RowDefinitions.Add(item61);
			RowDefinition item62 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid23.RowDefinitions.Add(item62);
			ColumnDefinition item63 = new ColumnDefinition
			{
				Width = new GridLength(44f, GridUnitType.Pixel)
			};
			grid23.ColumnDefinitions.Add(item63);
			ColumnDefinition item64 = new ColumnDefinition
			{
				Width = new GridLength(156f, GridUnitType.Pixel)
			};
			grid23.ColumnDefinitions.Add(item64);
			ColumnDefinition item65 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid23.ColumnDefinitions.Add(item65);
			TextBlock textBlock8 = new TextBlock();
			grid23.Children.Add(textBlock8);
			textBlock8.Name = "e_61";
			textBlock8.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 0);
			Grid.SetColumnSpan(textBlock8, 2);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_MoneyReward");
			TextBlock textBlock9 = new TextBlock();
			grid23.Children.Add(textBlock9);
			textBlock9.Name = "e_62";
			textBlock9.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 0);
			Grid.SetRow(textBlock9, 1);
			Grid.SetColumnSpan(textBlock9, 2);
			textBlock9.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_StartingDeposit");
			TextBlock textBlock10 = new TextBlock();
			grid23.Children.Add(textBlock10);
			textBlock10.Name = "e_63";
			textBlock10.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock10, 0);
			Grid.SetRow(textBlock10, 2);
			Grid.SetColumnSpan(textBlock10, 2);
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_Duration");
			TextBlock textBlock11 = new TextBlock();
			grid23.Children.Add(textBlock11);
			textBlock11.Name = "e_64";
			textBlock11.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 0);
			Grid.SetRow(textBlock11, 3);
			Grid.SetColumnSpan(textBlock11, 2);
			textBlock11.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_Type");
			NumericTextBox numericTextBox = new NumericTextBox();
			grid23.Children.Add(numericTextBox);
			numericTextBox.Name = "e_65";
			numericTextBox.Margin = new Thickness(0f, 5f, 15f, 0f);
			numericTextBox.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox.TabIndex = 103;
			numericTextBox.MaxLength = 7;
			numericTextBox.Minimum = 0f;
			numericTextBox.Maximum = 9999999f;
			Grid.SetColumn(numericTextBox, 2);
			Grid.SetRow(numericTextBox, 0);
			numericTextBox.SetBinding(binding: new Binding("NewContractCurrencyReward"), property: NumericTextBox.ValueProperty);
			NumericTextBox numericTextBox2 = new NumericTextBox();
			grid23.Children.Add(numericTextBox2);
			numericTextBox2.Name = "e_66";
			numericTextBox2.Margin = new Thickness(0f, 5f, 15f, 0f);
			numericTextBox2.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox2.TabIndex = 104;
			numericTextBox2.MaxLength = 7;
			numericTextBox2.Minimum = 0f;
			numericTextBox2.Maximum = 9999999f;
			Grid.SetColumn(numericTextBox2, 2);
			Grid.SetRow(numericTextBox2, 1);
			numericTextBox2.SetBinding(binding: new Binding("NewContractStartDeposit"), property: NumericTextBox.ValueProperty);
			NumericTextBox numericTextBox3 = new NumericTextBox();
			grid23.Children.Add(numericTextBox3);
			numericTextBox3.Name = "e_67";
			numericTextBox3.Margin = new Thickness(0f, 5f, 15f, 0f);
			numericTextBox3.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox3.TabIndex = 105;
			numericTextBox3.MaxLength = 5;
			numericTextBox3.Minimum = 0f;
			numericTextBox3.Maximum = 99999f;
			Grid.SetColumn(numericTextBox3, 2);
			Grid.SetRow(numericTextBox3, 2);
			numericTextBox3.SetBinding(binding: new Binding("NewContractDurationInMin"), property: NumericTextBox.ValueProperty);
			ComboBox comboBox3 = new ComboBox();
			grid23.Children.Add(comboBox3);
			comboBox3.Name = "e_68";
			comboBox3.Margin = new Thickness(0f, 4f, 15f, 4f);
			comboBox3.TabIndex = 106;
			Grid.SetColumn(comboBox3, 2);
			Grid.SetRow(comboBox3, 3);
			comboBox3.SetBinding(binding: new Binding("ContractTypes"), property: ItemsControl.ItemsSourceProperty);
			comboBox3.SetBinding(binding: new Binding("SelectedContractTypeIndex"), property: Selector.SelectedIndexProperty);
			Border border7 = new Border();
			stackPanel.Children.Add(border7);
			border7.Name = "e_69";
			border7.Height = 2f;
			border7.Margin = new Thickness(15f, 0f, 15f, 0f);
			border7.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid grid24 = new Grid();
			stackPanel.Children.Add(grid24);
			grid24.Name = "e_70";
			grid24.Margin = new Thickness(15f, 0f, 0f, 0f);
			Grid grid25 = new Grid();
			grid24.Children.Add(grid25);
			grid25.Name = "e_71";
			RowDefinition item66 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid25.RowDefinitions.Add(item66);
			ColumnDefinition item67 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid25.ColumnDefinitions.Add(item67);
			ColumnDefinition item68 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid25.ColumnDefinitions.Add(item68);
			grid25.SetBinding(binding: new Binding("IsContractSelected_Deliver"), property: UIElement.VisibilityProperty);
			TextBlock textBlock12 = new TextBlock();
			grid25.Children.Add(textBlock12);
			textBlock12.Name = "e_72";
			textBlock12.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			textBlock12.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock12, 0);
			Grid.SetRow(textBlock12, 0);
			textBlock12.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetBlock");
			Grid grid26 = new Grid();
			grid25.Children.Add(grid26);
			grid26.Name = "e_73";
			grid26.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid26.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item69 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid26.ColumnDefinitions.Add(item69);
			ColumnDefinition item70 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid26.ColumnDefinitions.Add(item70);
			Grid.SetColumn(grid26, 1);
			Grid.SetRow(grid26, 0);
			TextBlock textBlock13 = new TextBlock();
			grid26.Children.Add(textBlock13);
			textBlock13.Name = "e_74";
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock13, 0);
			textBlock13.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button9 = new Button();
			grid26.Children.Add(button9);
			button9.Name = "e_75";
			button9.Width = 150f;
			button9.Margin = new Thickness(10f, 5f, 15f, 0f);
			button9.TabIndex = 107;
			Grid.SetColumn(button9, 1);
			button9.SetBinding(binding: new Binding("NewContractDeliverBlockSelectCommand"), property: Button.CommandProperty);
			button9.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectBlock");
			Grid grid27 = new Grid();
			grid24.Children.Add(grid27);
			grid27.Name = "e_76";
			RowDefinition item71 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid27.RowDefinitions.Add(item71);
			RowDefinition item72 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid27.RowDefinitions.Add(item72);
			RowDefinition item73 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid27.RowDefinitions.Add(item73);
			ColumnDefinition item74 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid27.ColumnDefinitions.Add(item74);
			ColumnDefinition item75 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid27.ColumnDefinitions.Add(item75);
			grid27.SetBinding(binding: new Binding("IsContractSelected_ObtainAndDeliver"), property: UIElement.VisibilityProperty);
			TextBlock textBlock14 = new TextBlock();
			grid27.Children.Add(textBlock14);
			textBlock14.Name = "e_77";
			textBlock14.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock14.VerticalAlignment = VerticalAlignment.Center;
			textBlock14.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock14, 0);
			Grid.SetRow(textBlock14, 0);
			textBlock14.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetBlock");
			TextBlock textBlock15 = new TextBlock();
			grid27.Children.Add(textBlock15);
			textBlock15.Name = "e_78";
			textBlock15.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock15.VerticalAlignment = VerticalAlignment.Center;
			textBlock15.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock15, 0);
			Grid.SetRow(textBlock15, 1);
			textBlock15.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_ItemType");
			TextBlock textBlock16 = new TextBlock();
			grid27.Children.Add(textBlock16);
			textBlock16.Name = "e_79";
			textBlock16.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock16.VerticalAlignment = VerticalAlignment.Center;
			textBlock16.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock16, 0);
			Grid.SetRow(textBlock16, 2);
			textBlock16.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_ItemAmount");
			Grid grid28 = new Grid();
			grid27.Children.Add(grid28);
			grid28.Name = "e_80";
			grid28.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid28.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item76 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid28.ColumnDefinitions.Add(item76);
			ColumnDefinition item77 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid28.ColumnDefinitions.Add(item77);
			Grid.SetColumn(grid28, 1);
			Grid.SetRow(grid28, 0);
			TextBlock textBlock17 = new TextBlock();
			grid28.Children.Add(textBlock17);
			textBlock17.Name = "e_81";
			textBlock17.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock17, 0);
			textBlock17.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button10 = new Button();
			grid28.Children.Add(button10);
			button10.Name = "e_82";
			button10.Width = 150f;
			button10.Margin = new Thickness(10f, 5f, 15f, 0f);
			button10.TabIndex = 108;
			Grid.SetColumn(button10, 1);
			button10.SetBinding(binding: new Binding("NewContractObtainAndDeliverBlockSelectCommand"), property: Button.CommandProperty);
			button10.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectBlock");
			ComboBox comboBox4 = new ComboBox();
			grid27.Children.Add(comboBox4);
			comboBox4.Name = "e_83";
			comboBox4.Margin = new Thickness(0f, 0f, 15f, 0f);
			comboBox4.TabIndex = 109;
			Grid.SetColumn(comboBox4, 1);
			Grid.SetRow(comboBox4, 1);
			comboBox4.SetBinding(binding: new Binding("DeliverableItems"), property: ItemsControl.ItemsSourceProperty);
			comboBox4.SetBinding(binding: new Binding("NewContractObtainAndDeliverSelectedItemType"), property: Selector.SelectedItemProperty);
			NumericTextBox numericTextBox4 = new NumericTextBox();
			grid27.Children.Add(numericTextBox4);
			numericTextBox4.Name = "e_84";
			numericTextBox4.Margin = new Thickness(0f, 0f, 15f, 0f);
			numericTextBox4.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox4.TabIndex = 110;
			numericTextBox4.MaxLength = 5;
			numericTextBox4.Minimum = 0f;
			numericTextBox4.Maximum = 99999f;
			Grid.SetColumn(numericTextBox4, 1);
			Grid.SetRow(numericTextBox4, 2);
			numericTextBox4.SetBinding(binding: new Binding("NewContractObtainAndDeliverItemAmount"), property: NumericTextBox.ValueProperty);
			Grid grid29 = new Grid();
			grid24.Children.Add(grid29);
			grid29.Name = "e_85";
			RowDefinition item78 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid29.RowDefinitions.Add(item78);
			RowDefinition item79 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid29.RowDefinitions.Add(item79);
			ColumnDefinition item80 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid29.ColumnDefinitions.Add(item80);
			ColumnDefinition item81 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid29.ColumnDefinitions.Add(item81);
			grid29.SetBinding(binding: new Binding("IsContractSelected_Find"), property: UIElement.VisibilityProperty);
			TextBlock textBlock18 = new TextBlock();
			grid29.Children.Add(textBlock18);
			textBlock18.Name = "e_86";
			textBlock18.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock18.VerticalAlignment = VerticalAlignment.Center;
			textBlock18.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock18, 0);
			Grid.SetRow(textBlock18, 0);
			textBlock18.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetGrid");
			TextBlock textBlock19 = new TextBlock();
			grid29.Children.Add(textBlock19);
			textBlock19.Name = "e_87";
			textBlock19.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock19.VerticalAlignment = VerticalAlignment.Center;
			textBlock19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock19, 0);
			Grid.SetRow(textBlock19, 1);
			textBlock19.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_SearchRadius");
			Grid grid30 = new Grid();
			grid29.Children.Add(grid30);
			grid30.Name = "e_88";
			grid30.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid30.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item82 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid30.ColumnDefinitions.Add(item82);
			ColumnDefinition item83 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid30.ColumnDefinitions.Add(item83);
			Grid.SetColumn(grid30, 1);
			Grid.SetRow(grid30, 0);
			TextBlock textBlock20 = new TextBlock();
			grid30.Children.Add(textBlock20);
			textBlock20.Name = "e_89";
			textBlock20.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock20, 0);
			textBlock20.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button11 = new Button();
			grid30.Children.Add(button11);
			button11.Name = "e_90";
			button11.Width = 150f;
			button11.Margin = new Thickness(10f, 5f, 15f, 0f);
			button11.VerticalAlignment = VerticalAlignment.Center;
			button11.TabIndex = 111;
			Grid.SetColumn(button11, 1);
			button11.SetBinding(binding: new Binding("NewContractFindGridSelectCommand"), property: Button.CommandProperty);
			button11.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectGrid");
			NumericTextBox numericTextBox5 = new NumericTextBox();
			grid29.Children.Add(numericTextBox5);
			numericTextBox5.Name = "e_91";
			numericTextBox5.Margin = new Thickness(0f, 0f, 15f, 0f);
			numericTextBox5.VerticalAlignment = VerticalAlignment.Center;
			numericTextBox5.TabIndex = 112;
			numericTextBox5.MaxLength = 5;
			numericTextBox5.Minimum = 0f;
			numericTextBox5.Maximum = 99999f;
			Grid.SetColumn(numericTextBox5, 1);
			Grid.SetRow(numericTextBox5, 1);
			numericTextBox5.SetBinding(binding: new Binding("NewContractFindSearchRadius"), property: NumericTextBox.ValueProperty);
			Grid grid31 = new Grid();
			grid24.Children.Add(grid31);
			grid31.Name = "e_92";
			RowDefinition item84 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid31.RowDefinitions.Add(item84);
			ColumnDefinition item85 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid31.ColumnDefinitions.Add(item85);
			ColumnDefinition item86 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid31.ColumnDefinitions.Add(item86);
			grid31.SetBinding(binding: new Binding("IsContractSelected_Repair"), property: UIElement.VisibilityProperty);
			TextBlock textBlock21 = new TextBlock();
			grid31.Children.Add(textBlock21);
			textBlock21.Name = "e_93";
			textBlock21.Margin = new Thickness(0f, 15f, 0f, 15f);
			textBlock21.VerticalAlignment = VerticalAlignment.Center;
			textBlock21.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock21, 0);
			Grid.SetRow(textBlock21, 0);
			textBlock21.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_TargetGrid");
			Grid grid32 = new Grid();
			grid31.Children.Add(grid32);
			grid32.Name = "e_94";
			grid32.Margin = new Thickness(5f, 0f, 0f, 0f);
			grid32.HorizontalAlignment = HorizontalAlignment.Right;
			ColumnDefinition item87 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid32.ColumnDefinitions.Add(item87);
			ColumnDefinition item88 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid32.ColumnDefinitions.Add(item88);
			Grid.SetColumn(grid32, 1);
			Grid.SetRow(grid32, 0);
			TextBlock textBlock22 = new TextBlock();
			grid32.Children.Add(textBlock22);
			textBlock22.Name = "e_95";
			textBlock22.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock22, 0);
			textBlock22.SetBinding(binding: new Binding("NewContractSelectionName"), property: TextBlock.TextProperty);
			Button button12 = new Button();
			grid32.Children.Add(button12);
			button12.Name = "e_96";
			button12.Width = 150f;
			button12.Margin = new Thickness(10f, 5f, 15f, 0f);
			button12.TabIndex = 113;
			Grid.SetColumn(button12, 1);
			button12.SetBinding(binding: new Binding("NewContractRepairGridSelectCommand"), property: Button.CommandProperty);
			button12.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_SelectGrid");
			Border border8 = new Border();
			grid22.Children.Add(border8);
			border8.Name = "e_97";
			border8.Height = 2f;
			border8.Margin = new Thickness(25f, 0f, 15f, 0f);
			border8.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border8, 1);
			Grid grid33 = new Grid();
			grid22.Children.Add(grid33);
			grid33.Name = "e_98";
			ColumnDefinition item89 = new ColumnDefinition
			{
				Width = new GridLength(200f, GridUnitType.Pixel)
			};
			grid33.ColumnDefinitions.Add(item89);
			ColumnDefinition item90 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid33.ColumnDefinitions.Add(item90);
			Grid.SetRow(grid33, 2);
			TextBlock textBlock23 = new TextBlock();
			grid33.Children.Add(textBlock23);
			textBlock23.Name = "e_99";
			textBlock23.Margin = new Thickness(25f, 10f, 0f, 10f);
			textBlock23.VerticalAlignment = VerticalAlignment.Center;
			textBlock23.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock23, 0);
			Grid.SetRow(textBlock23, 0);
			textBlock23.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Text_CurrentMoney");
			StackPanel stackPanel2 = new StackPanel();
			grid33.Children.Add(stackPanel2);
			stackPanel2.Name = "e_100";
			stackPanel2.Margin = new Thickness(0f, 10f, 15f, 10f);
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel2.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel2, 1);
			Grid.SetRow(stackPanel2, 0);
			TextBlock textBlock24 = new TextBlock();
			stackPanel2.Children.Add(textBlock24);
			textBlock24.Name = "e_101";
			textBlock24.Margin = new Thickness(30f, 0f, 0f, 0f);
			textBlock24.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock24.VerticalAlignment = VerticalAlignment.Center;
			textBlock24.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock24.SetBinding(binding: new Binding("CurrentMoneyFormated"), property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel2.Children.Add(image2);
			image2.Name = "e_102";
			image2.Height = 20f;
			image2.Margin = new Thickness(4f, 2f, 2f, 2f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			Grid.SetColumn(image2, 2);
			image2.SetBinding(binding: new Binding("CurrencyIcon"), property: Image.SourceProperty);
			Border border9 = new Border();
			grid19.Children.Add(border9);
			border9.Name = "e_103";
			border9.Height = 2f;
			border9.Margin = new Thickness(0f, 10f, 0f, 10f);
			border9.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetRow(border9, 1);
			Grid.SetColumnSpan(border9, 2);
			Grid grid34 = new Grid();
			grid19.Children.Add(grid34);
			grid34.Name = "e_104";
			grid34.Margin = new Thickness(0f, 0f, 0f, 30f);
			ColumnDefinition item91 = new ColumnDefinition();
			grid34.ColumnDefinitions.Add(item91);
			ColumnDefinition item92 = new ColumnDefinition
			{
				Width = new GridLength(150f, GridUnitType.Pixel)
			};
			grid34.ColumnDefinitions.Add(item92);
			Grid.SetColumn(grid34, 1);
			Grid.SetRow(grid34, 2);
			Button button13 = new Button();
			grid34.Children.Add(button13);
			button13.Name = "e_105";
			button13.Margin = new Thickness(10f, 10f, 0f, 10f);
			button13.VerticalAlignment = VerticalAlignment.Center;
			button13.TabIndex = 114;
			Grid.SetColumn(button13, 1);
			button13.SetBinding(binding: new Binding("CreateCommand"), property: Button.CommandProperty);
			button13.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_CreateContract");
			Grid grid35 = new Grid();
			grid18.Children.Add(grid35);
			grid35.Name = "e_106";
			KeyBinding keyBinding2 = new KeyBinding
			{
				Gesture = new KeyGesture(KeyCode.Escape, ModifierKeys.None, "")
			};
			keyBinding2.SetBinding(binding: new Binding("AdminSelectionExitCommand"), property: InputBinding.CommandProperty);
			grid35.InputBindings.Add(keyBinding2);
			keyBinding2.Parent = grid35;
			RowDefinition item93 = new RowDefinition
			{
				Height = new GridLength(0.5f, GridUnitType.Star)
			};
			grid35.RowDefinitions.Add(item93);
			RowDefinition item94 = new RowDefinition
			{
				Height = new GridLength(2f, GridUnitType.Star)
			};
			grid35.RowDefinitions.Add(item94);
			RowDefinition item95 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid35.RowDefinitions.Add(item95);
			ColumnDefinition item96 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid35.ColumnDefinitions.Add(item96);
			ColumnDefinition item97 = new ColumnDefinition
			{
				Width = new GridLength(3f, GridUnitType.Star)
			};
			grid35.ColumnDefinitions.Add(item97);
			ColumnDefinition item98 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid35.ColumnDefinitions.Add(item98);
			Grid.SetColumn(grid35, 1);
			Grid.SetRow(grid35, 3);
			grid35.SetBinding(binding: new Binding("IsVisibleAdminSelection"), property: UIElement.VisibilityProperty);
			Image image3 = new Image();
			grid35.Children.Add(image3);
			image3.Name = "e_107";
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
			};
			image3.Stretch = Stretch.Fill;
			Grid.SetColumn(image3, 1);
			Grid.SetRow(image3, 1);
			image3.SetBinding(binding: new Binding("BackgroundOverlay"), property: ImageBrush.ColorOverlayProperty);
			Grid grid36 = new Grid();
			grid35.Children.Add(grid36);
			grid36.Name = "e_108";
			RowDefinition item99 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid36.RowDefinitions.Add(item99);
			RowDefinition item100 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid36.RowDefinitions.Add(item100);
			RowDefinition item101 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid36.RowDefinitions.Add(item101);
			RowDefinition item102 = new RowDefinition
			{
				Height = new GridLength(2.5f, GridUnitType.Star)
			};
			grid36.RowDefinitions.Add(item102);
			RowDefinition item103 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid36.RowDefinitions.Add(item103);
			RowDefinition item104 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid36.RowDefinitions.Add(item104);
			RowDefinition item105 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Star)
			};
			grid36.RowDefinitions.Add(item105);
			ColumnDefinition item106 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Star)
			};
			grid36.ColumnDefinitions.Add(item106);
			ColumnDefinition item107 = new ColumnDefinition
			{
				Width = new GridLength(12f, GridUnitType.Star)
			};
			grid36.ColumnDefinitions.Add(item107);
			ColumnDefinition item108 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid36.ColumnDefinitions.Add(item108);
			Grid.SetColumn(grid36, 1);
			Grid.SetRow(grid36, 1);
			TextBlock textBlock25 = new TextBlock();
			grid36.Children.Add(textBlock25);
			textBlock25.Name = "e_109";
			textBlock25.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock25.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock25.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock25, 1);
			Grid.SetRow(textBlock25, 1);
			textBlock25.SetBinding(binding: new Binding("AdminSelectionCaption"), property: TextBlock.TextProperty);
			Border border10 = new Border();
			grid36.Children.Add(border10);
			border10.Name = "e_110";
			border10.Height = 2f;
			border10.Margin = new Thickness(10f, 10f, 10f, 10f);
			border10.Background = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			Grid.SetColumn(border10, 1);
			Grid.SetRow(border10, 2);
			TextBlock textBlock26 = new TextBlock();
			grid36.Children.Add(textBlock26);
			textBlock26.Name = "e_111";
			textBlock26.Margin = new Thickness(10f, 10f, 10f, 10f);
			textBlock26.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock26.VerticalAlignment = VerticalAlignment.Center;
			textBlock26.TextWrapping = TextWrapping.Wrap;
			Grid.SetColumn(textBlock26, 1);
			Grid.SetRow(textBlock26, 3);
			textBlock26.SetBinding(binding: new Binding("AdminSelectionText"), property: TextBlock.TextProperty);
			ComboBox comboBox5 = new ComboBox();
			grid36.Children.Add(comboBox5);
			comboBox5.Name = "e_112";
			comboBox5.Margin = new Thickness(10f, 10f, 10f, 10f);
			Grid.SetColumn(comboBox5, 1);
			Grid.SetRow(comboBox5, 4);
			comboBox5.SetBinding(binding: new Binding("AdminSelectionItems"), property: ItemsControl.ItemsSourceProperty);
			comboBox5.SetBinding(binding: new Binding("AdminSelectedItemIndex"), property: Selector.SelectedIndexProperty);
			Button button14 = new Button();
			grid36.Children.Add(button14);
			button14.Name = "e_113";
			button14.Width = 150f;
			button14.Margin = new Thickness(10f, 10f, 10f, 10f);
			button14.HorizontalAlignment = HorizontalAlignment.Right;
			button14.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(button14, 1);
			Grid.SetRow(button14, 5);
			button14.SetBinding(binding: new Binding("AdminSelectionConfirmCommand"), property: Button.CommandProperty);
			button14.SetResourceReference(ContentControl.ContentProperty, "ContractScreen_Button_Confirm");
			ImageButton imageButton2 = new ImageButton();
			grid36.Children.Add(imageButton2);
			imageButton2.Name = "e_114";
			imageButton2.Height = 24f;
			imageButton2.Width = 24f;
			imageButton2.Margin = new Thickness(16f, 16f, 16f, 16f);
			imageButton2.HorizontalAlignment = HorizontalAlignment.Right;
			imageButton2.VerticalAlignment = VerticalAlignment.Center;
			imageButton2.ImageStretch = Stretch.Uniform;
			imageButton2.ImageNormal = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
			};
			imageButton2.ImageHover = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
			};
			imageButton2.ImagePressed = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
			};
			Grid.SetColumn(imageButton2, 2);
			imageButton2.SetBinding(binding: new Binding("AdminSelectionExitCommand"), property: Button.CommandProperty);
<<<<<<< HEAD
			observableCollection.Add(tabItem3);
			return observableCollection;
=======
			((Collection<object>)(object)obj).Add((object)tabItem3);
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void InitializeElemente_14Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}

		private static void InitializeElemente_27Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}

		private static void InitializeElemente_50Resources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(DataTemplatesContractsDataGrid.Instance);
		}
	}
}
