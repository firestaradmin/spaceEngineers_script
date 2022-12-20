using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplates_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public sealed class DataTemplates : ResourceDictionary
	{
		private static DataTemplates singleton = new DataTemplates();

		public static DataTemplates Instance => singleton;

		public DataTemplates()
		{
			InitializeResources();
		}

		private void InitializeResources()
		{
			base.MergedDictionaries.Add(Styles.Instance);
			Func<UIElement, UIElement> createMethod = r_0_dtMethod;
			Add(typeof(MyInventoryItemModel), new DataTemplate(typeof(MyInventoryItemModel), createMethod));
			Func<UIElement, UIElement> createMethod2 = r_1_dtMethod;
			Add("InventoryItemDetailed", new DataTemplate(typeof(MyInventoryItemModel), createMethod2));
			Func<UIElement, UIElement> createMethod3 = r_2_dtMethod;
			Add("InventoryItemDetailedWithRemove", new DataTemplate(typeof(MyInventoryItemModel), createMethod3));
			Add("PlayerTradeViewModelLocator", new MyPlayerTradeViewModelLocator());
			FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Name", typeof(MyInventoryItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Icon", typeof(MyInventoryItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "AmountFormatted", typeof(MyInventoryItemModel_AmountFormatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "IconSymbol", typeof(MyInventoryItemModel_IconSymbol_PropertyInfo));
		}

		private static UIElement tt_e_0_Method()
		{
			TextBlock obj = new TextBlock
			{
				Name = "e_1",
				Margin = new Thickness(4f, 4f, 4f, 4f),
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_0_dtMethod(UIElement parent)
		{
			Grid grid = new Grid();
			grid.Parent = parent;
			grid.Name = "e_0";
			grid.Height = 64f;
			grid.Width = 64f;
			ToolTip toolTip2 = (ToolTip)(grid.ToolTip = new ToolTip());
			toolTip2.Content = tt_e_0_Method();
			MouseBinding mouseBinding = new MouseBinding();
			mouseBinding.Gesture = new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.None);
			Binding binding = new Binding("ViewModel.AddItemToOfferCommand");
			binding.Source = new MyPlayerTradeViewModelLocator(isDesignMode: false);
			mouseBinding.SetBinding(InputBinding.CommandProperty, binding);
			Binding binding2 = new Binding();
			mouseBinding.SetBinding(InputBinding.CommandParameterProperty, binding2);
			grid.InputBindings.Add(mouseBinding);
			mouseBinding.Parent = grid;
			MouseBinding mouseBinding2 = new MouseBinding();
			mouseBinding2.Gesture = new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control);
			Binding binding3 = new Binding("ViewModel.AddStackTenToOfferCommand");
			binding3.Source = new MyPlayerTradeViewModelLocator(isDesignMode: false);
			mouseBinding2.SetBinding(InputBinding.CommandProperty, binding3);
			Binding binding4 = new Binding();
			mouseBinding2.SetBinding(InputBinding.CommandParameterProperty, binding4);
			grid.InputBindings.Add(mouseBinding2);
			mouseBinding2.Parent = grid;
			MouseBinding mouseBinding3 = new MouseBinding();
			mouseBinding3.Gesture = new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Shift);
			Binding binding5 = new Binding("ViewModel.AddStackHundredToOfferCommand");
			binding5.Source = new MyPlayerTradeViewModelLocator(isDesignMode: false);
			mouseBinding3.SetBinding(InputBinding.CommandProperty, binding5);
			Binding binding6 = new Binding();
			mouseBinding3.SetBinding(InputBinding.CommandParameterProperty, binding6);
			grid.InputBindings.Add(mouseBinding3);
			mouseBinding3.Parent = grid;
			Image image = new Image();
			grid.Children.Add(image);
			image.Name = "e_2";
			image.Stretch = Stretch.Fill;
			Binding binding7 = new Binding("Icon");
			binding7.UseGeneratedBindings = true;
			image.SetBinding(Image.SourceProperty, binding7);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_3";
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
			textBlock2.Name = "e_4";
			textBlock2.Margin = new Thickness(6f, 4f, 0f, 0f);
			textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
			textBlock2.VerticalAlignment = VerticalAlignment.Top;
			textBlock2.TextAlignment = TextAlignment.Left;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			Binding binding9 = new Binding("IconSymbol");
			binding9.UseGeneratedBindings = true;
			textBlock2.SetBinding(TextBlock.TextProperty, binding9);
			return grid;
		}

		private static UIElement r_1_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_5",
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
			border.Name = "e_6";
			border.Height = 64f;
			border.Width = 64f;
			border.SetResourceReference(Control.BackgroundProperty, "GridItem");
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_7";
			image.Margin = new Thickness(5f, 5f, 5f, 5f);
			image.Stretch = Stretch.Fill;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_8";
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
			textBlock2.Name = "e_9";
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
			textBlock3.Name = "e_10";
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock3, 1);
			textBlock3.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_2_dtMethod(UIElement parent)
		{
			Grid grid = new Grid();
			grid.Parent = parent;
			grid.Name = "e_11";
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
			border.Name = "e_12";
			border.Height = 64f;
			border.Width = 64f;
			border.SetResourceReference(Control.BackgroundProperty, "GridItem");
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_13";
			image.Margin = new Thickness(5f, 5f, 5f, 5f);
			image.Stretch = Stretch.Fill;
			Binding binding7 = new Binding("Icon");
			binding7.UseGeneratedBindings = true;
			image.SetBinding(Image.SourceProperty, binding7);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_14";
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
			textBlock2.Name = "e_15";
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
			textBlock3.Name = "e_16";
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock3, 1);
			Binding binding10 = new Binding("Name");
			binding10.UseGeneratedBindings = true;
			textBlock3.SetBinding(TextBlock.TextProperty, binding10);
			return grid;
		}
	}
}
