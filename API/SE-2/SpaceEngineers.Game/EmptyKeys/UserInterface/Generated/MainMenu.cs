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
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Animation;
using EmptyKeys.UserInterface.Themes;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MainMenu : UIRoot
	{
		private Grid rootGrid;

		private Grid main;

		private StackPanel e_0;

		private Border brushTest;

		private GroupBox e_1;

		private StackPanel e_2;

		private CheckBox e_3;

		private TextBox e_4;

		private Slider e_5;

		private TabControl e_6;

		private DataGrid e_10;

		private ComboBox e_11;

		private ProgressBar progressBar;

		private ScrollViewer e_18;

		private TextBlock e_19;

		public MainMenu()
		{
			Initialize();
		}

		public MainMenu(int width, int height)
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
			rootGrid.Margin = new Thickness(35f, 35f, 35f, 35f);
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.Width = new GridLength(1f, GridUnitType.Star);
			rootGrid.ColumnDefinitions.Add(columnDefinition);
			ColumnDefinition columnDefinition2 = new ColumnDefinition();
			columnDefinition2.Width = new GridLength(5.6f, GridUnitType.Star);
			rootGrid.ColumnDefinitions.Add(columnDefinition2);
			main = new Grid();
			rootGrid.Children.Add(main);
			main.Name = "main";
			RowDefinition rowDefinition = new RowDefinition();
			rowDefinition.Height = new GridLength(0.1f, GridUnitType.Star);
			main.RowDefinitions.Add(rowDefinition);
			RowDefinition item = new RowDefinition();
			main.RowDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			main.ColumnDefinitions.Add(item2);
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition3.Width = new GridLength(0.15f, GridUnitType.Star);
			main.ColumnDefinitions.Add(columnDefinition3);
			Grid.SetColumn(main, 1);
			e_0 = new StackPanel();
			main.Children.Add(e_0);
			e_0.Name = "e_0";
			e_0.Width = 500f;
			e_0.Margin = new Thickness(0f, 100f, 0f, 0f);
			Grid.SetRow(e_0, 1);
			brushTest = new Border();
			e_0.Children.Add(brushTest);
			brushTest.Name = "brushTest";
			brushTest.Height = 50f;
			brushTest.Width = 100f;
			EventTrigger eventTrigger = new EventTrigger(UIElement.LoadedEvent, brushTest);
			brushTest.Triggers.Add(eventTrigger);
			BeginStoryboard beginStoryboard = new BeginStoryboard();
			beginStoryboard.Name = "brushTest_ET_0_AC_0";
			eventTrigger.AddAction(beginStoryboard);
			Storyboard storyboard2 = (beginStoryboard.Storyboard = new Storyboard());
			storyboard2.Name = "brushTest_ET_0_AC_0_SB";
			SolidColorBrushAnimation solidColorBrushAnimation = new SolidColorBrushAnimation();
			solidColorBrushAnimation.Name = "brushTest_ET_0_AC_0_SB_TL_0";
			solidColorBrushAnimation.AutoReverse = true;
			solidColorBrushAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 5, 0));
			solidColorBrushAnimation.RepeatBehavior = RepeatBehavior.Forever;
			solidColorBrushAnimation.From = new ColorW(255, 0, 0, 255);
			solidColorBrushAnimation.To = new ColorW(0, 0, 255, 255);
			SineEase sineEase = (SineEase)(solidColorBrushAnimation.EasingFunction = new SineEase());
			Storyboard.SetTargetName(solidColorBrushAnimation, "brushTest");
			Storyboard.SetTargetProperty(solidColorBrushAnimation, Control.BackgroundProperty);
			storyboard2.Children.Add(solidColorBrushAnimation);
			brushTest.Background = new SolidColorBrush(new ColorW(0, 128, 0, 255));
			brushTest.BorderBrush = new SolidColorBrush(new ColorW(255, 0, 0, 255));
			brushTest.BorderThickness = new Thickness(3f, 3f, 3f, 3f);
			e_1 = new GroupBox();
			e_0.Children.Add(e_1);
			e_1.Name = "e_1";
			e_1.Header = "Group";
			e_2 = new StackPanel();
			e_1.Content = e_2;
			e_2.Name = "e_2";
			e_3 = new CheckBox();
			e_2.Children.Add(e_3);
			e_3.Name = "e_3";
			e_3.Content = "Test";
			e_4 = new TextBox();
			e_2.Children.Add(e_4);
			e_4.Name = "e_4";
			e_5 = new Slider();
			e_2.Children.Add(e_5);
			e_5.Name = "e_5";
			e_5.Minimum = 0f;
			e_5.Maximum = 100f;
			e_5.Value = 50f;
			e_6 = new TabControl();
			e_2.Children.Add(e_6);
			e_6.Name = "e_6";
<<<<<<< HEAD
			e_6.ItemsSource = Get_e_6_Items();
=======
			e_6.ItemsSource = (IEnumerable)Get_e_6_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			e_10 = new DataGrid();
			e_2.Children.Add(e_10);
			e_10.Name = "e_10";
			e_10.Height = 150f;
			e_10.AutoGenerateColumns = true;
			Binding binding = new Binding("GridData");
			e_10.SetBinding(ItemsControl.ItemsSourceProperty, binding);
			e_11 = new ComboBox();
			e_2.Children.Add(e_11);
			e_11.Name = "e_11";
<<<<<<< HEAD
			e_11.ItemsSource = Get_e_11_Items();
=======
			e_11.ItemsSource = (IEnumerable)Get_e_11_Items();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			progressBar = new ProgressBar();
			e_2.Children.Add(progressBar);
			progressBar.Name = "progressBar";
			progressBar.Height = 30f;
			EventTrigger eventTrigger2 = new EventTrigger(UIElement.LoadedEvent, progressBar);
			progressBar.Triggers.Add(eventTrigger2);
			BeginStoryboard beginStoryboard2 = new BeginStoryboard();
			beginStoryboard2.Name = "progressBar_ET_0_AC_0";
			eventTrigger2.AddAction(beginStoryboard2);
			Storyboard storyboard4 = (beginStoryboard2.Storyboard = new Storyboard());
			storyboard4.Name = "progressBar_ET_0_AC_0_SB";
			FloatAnimation floatAnimation = new FloatAnimation();
			floatAnimation.Name = "progressBar_ET_0_AC_0_SB_TL_0";
			floatAnimation.AutoReverse = true;
			floatAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 5, 0));
			floatAnimation.RepeatBehavior = RepeatBehavior.Forever;
			floatAnimation.From = 0f;
			floatAnimation.To = 100f;
			SineEase sineEase2 = (SineEase)(floatAnimation.EasingFunction = new SineEase());
			Storyboard.SetTargetName(floatAnimation, "progressBar");
			Storyboard.SetTargetProperty(floatAnimation, RangeBase.ValueProperty);
			storyboard4.Children.Add(floatAnimation);
			progressBar.Minimum = 0f;
			progressBar.Maximum = 100f;
			progressBar.Value = 50f;
			e_18 = new ScrollViewer();
			e_0.Children.Add(e_18);
			e_18.Name = "e_18";
			e_18.Height = 100f;
			e_18.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
			e_18.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			e_19 = new TextBlock();
			e_18.Content = e_19;
			e_19.Name = "e_19";
			e_19.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
		}

		private static void InitializeElementResources(UIElement elem)
		{
			elem.Resources.MergedDictionaries.Add(Styles.Instance);
		}

		private static ObservableCollection<object> Get_e_6_Items()
		{
<<<<<<< HEAD
			return new ObservableCollection<object>
			{
				new TabItem
				{
					Name = "e_7",
					Content = "TabItem1",
					Header = "Tab Item 1"
				},
				new TabItem
				{
					Name = "e_8",
					Content = "TabItem2",
					Header = "Tab Item 2"
				},
				new TabItem
				{
					Name = "e_9",
					Content = "TabItem3",
					Header = "Tab Item 3"
				}
			};
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
			((Collection<object>)(object)obj).Add((object)new TabItem
			{
				Name = "e_7",
				Content = "TabItem1",
				Header = "Tab Item 1"
			});
			((Collection<object>)(object)obj).Add((object)new TabItem
			{
				Name = "e_8",
				Content = "TabItem2",
				Header = "Tab Item 2"
			});
			((Collection<object>)(object)obj).Add((object)new TabItem
			{
				Name = "e_9",
				Content = "TabItem3",
				Header = "Tab Item 3"
			});
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static ObservableCollection<object> Get_e_11_Items()
		{
<<<<<<< HEAD
			return new ObservableCollection<object>
			{
				new ComboBoxItem
				{
					Name = "e_12",
					Content = "Test1"
				},
				new ComboBoxItem
				{
					Name = "e_13",
					Content = "Test2"
				},
				new ComboBoxItem
				{
					Name = "e_14",
					Content = "Test3"
				},
				new ComboBoxItem
				{
					Name = "e_15",
					Content = "Test4"
				},
				new ComboBoxItem
				{
					Name = "e_16",
					Content = "Test5"
				},
				new ComboBoxItem
				{
					Name = "e_17",
					Content = "Test6"
				}
			};
=======
			ObservableCollection<object> obj = new ObservableCollection<object>();
			((Collection<object>)(object)obj).Add((object)new ComboBoxItem
			{
				Name = "e_12",
				Content = "Test1"
			});
			((Collection<object>)(object)obj).Add((object)new ComboBoxItem
			{
				Name = "e_13",
				Content = "Test2"
			});
			((Collection<object>)(object)obj).Add((object)new ComboBoxItem
			{
				Name = "e_14",
				Content = "Test3"
			});
			((Collection<object>)(object)obj).Add((object)new ComboBoxItem
			{
				Name = "e_15",
				Content = "Test4"
			});
			((Collection<object>)(object)obj).Add((object)new ComboBoxItem
			{
				Name = "e_16",
				Content = "Test5"
			});
			((Collection<object>)(object)obj).Add((object)new ComboBoxItem
			{
				Name = "e_17",
				Content = "Test6"
			});
			return obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
