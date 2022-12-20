using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesStoreBlock_Bindings;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public sealed class DataTemplatesStoreBlock : ResourceDictionary
	{
		private static DataTemplatesStoreBlock singleton = new DataTemplatesStoreBlock();

		public static DataTemplatesStoreBlock Instance => singleton;

		public DataTemplatesStoreBlock()
		{
			InitializeResources();
		}

		private void InitializeResources()
		{
			base.MergedDictionaries.Add(Styles.Instance);
			Func<UIElement, UIElement> createMethod = r_0_dtMethod;
			Add(typeof(MyInventoryItemModel), new DataTemplate(typeof(MyInventoryItemModel), createMethod));
			Func<UIElement, UIElement> createMethod2 = r_1_dtMethod;
			Add(typeof(MyInventoryTargetModel), new DataTemplate(typeof(MyInventoryTargetModel), createMethod2));
			Func<UIElement, UIElement> createMethod3 = r_2_dtMethod;
			Add(typeof(MyOrderItemModel), new DataTemplate(typeof(MyOrderItemModel), createMethod3));
			Func<UIElement, UIElement> createMethod4 = r_3_dtMethod;
			Add(typeof(MyStoreItemModel), new DataTemplate(typeof(MyStoreItemModel), createMethod4));
			Add("StoreBlockViewModelLocator", new MyStoreBlockViewModelLocator());
			FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Name", typeof(MyInventoryItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "Icon", typeof(MyInventoryItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "AmountFormatted", typeof(MyInventoryItemModel_AmountFormatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryItemModel), "IconSymbol", typeof(MyInventoryItemModel_IconSymbol_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetModel), "Icon", typeof(MyInventoryTargetModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyInventoryTargetModel), "Name", typeof(MyInventoryTargetModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyOrderItemModel), "Icon", typeof(MyOrderItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyOrderItemModel), "Name", typeof(MyOrderItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "Icon", typeof(MyStoreItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "Name", typeof(MyStoreItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "IsOffer", typeof(MyStoreItemModel_IsOffer_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "IsOrder", typeof(MyStoreItemModel_IsOrder_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "PricePerUnitFormatted", typeof(MyStoreItemModel_PricePerUnitFormatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "CurrencyIcon", typeof(MyStoreItemModel_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyStoreItemModel), "AmountFormatted", typeof(MyStoreItemModel_AmountFormatted_PropertyInfo));
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
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_0",
				Height = 64f,
				Width = 64f
			};
			ToolTip toolTip2 = (ToolTip)(obj.ToolTip = new ToolTip());
			toolTip2.Content = tt_e_0_Method();
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_2";
			image.Stretch = Stretch.Fill;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_3";
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
			textBlock2.Name = "e_4";
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
			return obj;
		}

		private static UIElement r_1_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_5",
				Orientation = Orientation.Horizontal
			};
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_6";
			image.Height = 24f;
			image.Margin = new Thickness(5f, 5f, 0f, 5f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_7";
			textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_2_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_8",
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			ColumnDefinition item = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			obj.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item2);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_9";
			image.Height = 24f;
			image.Margin = new Thickness(5f, 5f, 0f, 5f);
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_10";
			textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_3_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_11",
				Margin = new Thickness(2f, 2f, 2f, 2f),
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Center
			};
			RowDefinition item = new RowDefinition();
			obj.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition();
			obj.RowDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			obj.ColumnDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item4);
			ColumnDefinition item5 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			obj.ColumnDefinitions.Add(item5);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_12";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_13";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock2 = new TextBlock();
			obj.Children.Add(textBlock2);
			textBlock2.Name = "e_14";
			textBlock2.Margin = new Thickness(2f, 2f, 4f, 2f);
			textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock2, 2);
			textBlock2.SetBinding(binding: new Binding("IsOffer")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_OfferItem");
			TextBlock textBlock3 = new TextBlock();
			obj.Children.Add(textBlock3);
			textBlock3.Name = "e_15";
			textBlock3.Margin = new Thickness(2f, 2f, 4f, 2f);
			textBlock3.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock3, 2);
			textBlock3.SetBinding(binding: new Binding("IsOrder")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "StoreBlockView_OrderItem");
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_16";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			Grid.SetColumnSpan(stackPanel, 2);
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_17";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_PricePerUnit");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_18";
			textBlock5.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("PricePerUnitFormatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_19";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_20";
			textBlock6.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Text = "|";
			textBlock6.FontFamily = new FontFamily("InventorySmall");
			textBlock6.FontSize = 10f;
			TextBlock textBlock7 = new TextBlock();
			stackPanel.Children.Add(textBlock7);
			textBlock7.Name = "e_21";
			textBlock7.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.FontFamily = new FontFamily("InventorySmall");
			textBlock7.FontSize = 10f;
			textBlock7.SetResourceReference(TextBlock.TextProperty, "StoreBlock_Column_Amount");
			TextBlock textBlock8 = new TextBlock();
			stackPanel.Children.Add(textBlock8);
			textBlock8.Name = "e_22";
			textBlock8.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.FontFamily = new FontFamily("InventorySmall");
			textBlock8.FontSize = 10f;
			textBlock8.SetBinding(binding: new Binding("AmountFormatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}
	}
}
