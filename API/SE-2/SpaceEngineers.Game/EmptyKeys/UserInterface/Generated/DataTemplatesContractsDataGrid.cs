using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Game.Screens.Models;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public sealed class DataTemplatesContractsDataGrid : ResourceDictionary
	{
		private static DataTemplatesContractsDataGrid singleton = new DataTemplatesContractsDataGrid();

		public static DataTemplatesContractsDataGrid Instance => singleton;

		public DataTemplatesContractsDataGrid()
		{
			InitializeResources();
		}

		private void InitializeResources()
		{
			base.MergedDictionaries.Add(Styles.Instance);
			Func<UIElement, UIElement> createMethod = r_0_dtMethod;
			Add(typeof(MyContractModelCustom), new DataTemplate(typeof(MyContractModelCustom), createMethod));
			Func<UIElement, UIElement> createMethod2 = r_1_dtMethod;
			Add(typeof(MyContractModelDeliver), new DataTemplate(typeof(MyContractModelDeliver), createMethod2));
			Func<UIElement, UIElement> createMethod3 = r_2_dtMethod;
			Add(typeof(MyContractModelEscort), new DataTemplate(typeof(MyContractModelEscort), createMethod3));
			Func<UIElement, UIElement> createMethod4 = r_3_dtMethod;
			Add(typeof(MyContractModelFind), new DataTemplate(typeof(MyContractModelFind), createMethod4));
			Func<UIElement, UIElement> createMethod5 = r_4_dtMethod;
			Add(typeof(MyContractModelHunt), new DataTemplate(typeof(MyContractModelHunt), createMethod5));
			Func<UIElement, UIElement> createMethod6 = r_5_dtMethod;
			Add(typeof(MyContractModelObtainAndDeliver), new DataTemplate(typeof(MyContractModelObtainAndDeliver), createMethod6));
			Func<UIElement, UIElement> createMethod7 = r_6_dtMethod;
			Add(typeof(MyContractModelRepair), new DataTemplate(typeof(MyContractModelRepair), createMethod7));
			ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepGain.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepLoss.png");
			FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Icon", typeof(MyContractModelCustom_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Name", typeof(MyContractModelCustom_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "RewardMoney_Formatted", typeof(MyContractModelCustom_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "CurrencyIcon", typeof(MyContractModelCustom_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "RewardReputation_Formatted", typeof(MyContractModelCustom_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "FailReputationPenalty_Formated", typeof(MyContractModelCustom_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "IsFactionIconVisible", typeof(MyContractModelCustom_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "FactionIconBackgroundColor", typeof(MyContractModelCustom_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "FactionIconTooltip", typeof(MyContractModelCustom_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "FactionIcon", typeof(MyContractModelCustom_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "FactionIconColor", typeof(MyContractModelCustom_FactionIconColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Icon", typeof(MyContractModelDeliver_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Name", typeof(MyContractModelDeliver_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "RewardMoney_Formatted", typeof(MyContractModelDeliver_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "CurrencyIcon", typeof(MyContractModelDeliver_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "RewardReputation_Formatted", typeof(MyContractModelDeliver_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "FailReputationPenalty_Formated", typeof(MyContractModelDeliver_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "IsFactionIconVisible", typeof(MyContractModelDeliver_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "FactionIconBackgroundColor", typeof(MyContractModelDeliver_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "FactionIconTooltip", typeof(MyContractModelDeliver_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "FactionIcon", typeof(MyContractModelDeliver_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "FactionIconColor", typeof(MyContractModelDeliver_FactionIconColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Icon", typeof(MyContractModelEscort_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Name", typeof(MyContractModelEscort_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "RewardMoney_Formatted", typeof(MyContractModelEscort_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "CurrencyIcon", typeof(MyContractModelEscort_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "RewardReputation_Formatted", typeof(MyContractModelEscort_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "FailReputationPenalty_Formated", typeof(MyContractModelEscort_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "IsFactionIconVisible", typeof(MyContractModelEscort_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "FactionIconBackgroundColor", typeof(MyContractModelEscort_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "FactionIconTooltip", typeof(MyContractModelEscort_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "FactionIcon", typeof(MyContractModelEscort_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "FactionIconColor", typeof(MyContractModelEscort_FactionIconColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Icon", typeof(MyContractModelFind_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Name", typeof(MyContractModelFind_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "RewardMoney_Formatted", typeof(MyContractModelFind_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "CurrencyIcon", typeof(MyContractModelFind_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "RewardReputation_Formatted", typeof(MyContractModelFind_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "FailReputationPenalty_Formated", typeof(MyContractModelFind_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "IsFactionIconVisible", typeof(MyContractModelFind_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "FactionIconBackgroundColor", typeof(MyContractModelFind_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "FactionIconTooltip", typeof(MyContractModelFind_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "FactionIcon", typeof(MyContractModelFind_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "FactionIconColor", typeof(MyContractModelFind_FactionIconColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Icon", typeof(MyContractModelHunt_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Name", typeof(MyContractModelHunt_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "RewardMoney_Formatted", typeof(MyContractModelHunt_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "CurrencyIcon", typeof(MyContractModelHunt_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "RewardReputation_Formatted", typeof(MyContractModelHunt_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "IsFactionIconVisible", typeof(MyContractModelHunt_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "FactionIconBackgroundColor", typeof(MyContractModelHunt_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "FactionIconTooltip", typeof(MyContractModelHunt_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "FactionIcon", typeof(MyContractModelHunt_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "FactionIconColor", typeof(MyContractModelHunt_FactionIconColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Icon", typeof(MyContractModelObtainAndDeliver_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Name", typeof(MyContractModelObtainAndDeliver_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "RewardMoney_Formatted", typeof(MyContractModelObtainAndDeliver_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "CurrencyIcon", typeof(MyContractModelObtainAndDeliver_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "RewardReputation_Formatted", typeof(MyContractModelObtainAndDeliver_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "IsFactionIconVisible", typeof(MyContractModelObtainAndDeliver_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "FactionIconBackgroundColor", typeof(MyContractModelObtainAndDeliver_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "FactionIconTooltip", typeof(MyContractModelObtainAndDeliver_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "FactionIcon", typeof(MyContractModelObtainAndDeliver_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "FactionIconColor", typeof(MyContractModelObtainAndDeliver_FactionIconColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Icon", typeof(MyContractModelRepair_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Name", typeof(MyContractModelRepair_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "RewardMoney_Formatted", typeof(MyContractModelRepair_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "CurrencyIcon", typeof(MyContractModelRepair_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "RewardReputation_Formatted", typeof(MyContractModelRepair_RewardReputation_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "FailReputationPenalty_Formated", typeof(MyContractModelRepair_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "IsFactionIconVisible", typeof(MyContractModelRepair_IsFactionIconVisible_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "FactionIconBackgroundColor", typeof(MyContractModelRepair_FactionIconBackgroundColor_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "FactionIconTooltip", typeof(MyContractModelRepair_FactionIconTooltip_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "FactionIcon", typeof(MyContractModelRepair_FactionIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "FactionIconColor", typeof(MyContractModelRepair_FactionIconColor_PropertyInfo));
		}

		private static UIElement r_0_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_0",
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
			image.Name = "e_1";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_2";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_3";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_4";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_5";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_6";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_7";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_8";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_9";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_10";
			textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.FontFamily = new FontFamily("InventorySmall");
			textBlock6.FontSize = 10f;
			textBlock6.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			stackPanel.Children.Add(image4);
			image4.Name = "e_11";
			image4.Width = 16f;
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
			};
			image4.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_12";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image5 = (Image)(border.Child = new Image());
			image5.Name = "FactionIcon";
			image5.Height = 32f;
			image5.Margin = new Thickness(0f, 0f, 0f, 0f);
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image5.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image5.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_1_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_13",
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
			image.Name = "e_14";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_15";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_16";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_17";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_18";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_19";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_20";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_21";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_22";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_23";
			textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.FontFamily = new FontFamily("InventorySmall");
			textBlock6.FontSize = 10f;
			textBlock6.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			stackPanel.Children.Add(image4);
			image4.Name = "e_24";
			image4.Width = 16f;
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
			};
			image4.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_25";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image5 = (Image)(border.Child = new Image());
			image5.Name = "FactionIcon";
			image5.Height = 32f;
			image5.Margin = new Thickness(0f, 0f, 0f, 0f);
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image5.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image5.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_2_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_26",
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
			image.Name = "e_27";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_28";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_29";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_30";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_31";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_32";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_33";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_34";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_35";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_36";
			textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.FontFamily = new FontFamily("InventorySmall");
			textBlock6.FontSize = 10f;
			textBlock6.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			stackPanel.Children.Add(image4);
			image4.Name = "e_37";
			image4.Width = 16f;
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
			};
			image4.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_38";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image5 = (Image)(border.Child = new Image());
			image5.Name = "FactionIcon";
			image5.Height = 32f;
			image5.Margin = new Thickness(0f, 0f, 0f, 0f);
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image5.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image5.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_3_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_39",
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
			image.Name = "e_40";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_41";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_42";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_43";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_44";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_45";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_46";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_47";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_48";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_49";
			textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.FontFamily = new FontFamily("InventorySmall");
			textBlock6.FontSize = 10f;
			textBlock6.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			stackPanel.Children.Add(image4);
			image4.Name = "e_50";
			image4.Width = 16f;
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
			};
			image4.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_51";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image5 = (Image)(border.Child = new Image());
			image5.Name = "FactionIcon";
			image5.Height = 32f;
			image5.Margin = new Thickness(0f, 0f, 0f, 0f);
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image5.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image5.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_4_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_52",
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
			image.Name = "e_53";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_54";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_55";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_56";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_57";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_58";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_59";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_60";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_61";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_62";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image4 = (Image)(border.Child = new Image());
			image4.Name = "FactionIcon";
			image4.Height = 32f;
			image4.Margin = new Thickness(0f, 0f, 0f, 0f);
			image4.Stretch = Stretch.Uniform;
			image4.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image4.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image4.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_5_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_63",
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
			image.Name = "e_64";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_65";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_66";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_67";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_68";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_69";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_70";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_71";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_72";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_73";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image4 = (Image)(border.Child = new Image());
			image4.Name = "FactionIcon";
			image4.Height = 32f;
			image4.Margin = new Thickness(0f, 0f, 0f, 0f);
			image4.Stretch = Stretch.Uniform;
			image4.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image4.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image4.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_6_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_74",
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
			image.Name = "e_75";
			image.Width = 64f;
			image.Margin = new Thickness(2f, 0f, 2f, 0f);
			image.Stretch = Stretch.Uniform;
			Grid.SetRowSpan(image, 2);
			ImageBrush.SetColorOverlay(image, new ColorW(131, 201, 226, 255));
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_76";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetColumn(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			obj.Children.Add(stackPanel);
			stackPanel.Name = "e_77";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 1);
			TextBlock textBlock2 = new TextBlock();
			stackPanel.Children.Add(textBlock2);
			textBlock2.Name = "e_78";
			textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.FontFamily = new FontFamily("InventorySmall");
			textBlock2.FontSize = 10f;
			textBlock2.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image2 = new Image();
			stackPanel.Children.Add(image2);
			image2.Name = "e_79";
			image2.Height = 14f;
			image2.Margin = new Thickness(1f, 1f, 1f, 1f);
			image2.HorizontalAlignment = HorizontalAlignment.Right;
			image2.VerticalAlignment = VerticalAlignment.Center;
			image2.Stretch = Stretch.Uniform;
			image2.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_80";
			textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Text = "|";
			textBlock3.FontFamily = new FontFamily("InventorySmall");
			textBlock3.FontSize = 10f;
			TextBlock textBlock4 = new TextBlock();
			stackPanel.Children.Add(textBlock4);
			textBlock4.Name = "e_81";
			textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			textBlock4.FontFamily = new FontFamily("InventorySmall");
			textBlock4.FontSize = 10f;
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_RepChange_Hint");
			TextBlock textBlock5 = new TextBlock();
			stackPanel.Children.Add(textBlock5);
			textBlock5.Name = "e_82";
			textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontFamily = new FontFamily("InventorySmall");
			textBlock5.FontSize = 10f;
			textBlock5.SetBinding(binding: new Binding("RewardReputation_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_83";
			image3.Width = 16f;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
			};
			image3.Stretch = Stretch.Uniform;
			TextBlock textBlock6 = new TextBlock();
			stackPanel.Children.Add(textBlock6);
			textBlock6.Name = "e_84";
			textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.FontFamily = new FontFamily("InventorySmall");
			textBlock6.FontSize = 10f;
			textBlock6.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image4 = new Image();
			stackPanel.Children.Add(image4);
			image4.Name = "e_85";
			image4.Width = 16f;
			image4.VerticalAlignment = VerticalAlignment.Center;
			image4.Source = new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
			};
			image4.Stretch = Stretch.Uniform;
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_86";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.HorizontalAlignment = HorizontalAlignment.Left;
			border.VerticalAlignment = VerticalAlignment.Top;
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Grid.SetColumn(border, 2);
			Grid.SetRowSpan(border, 2);
			border.SetBinding(binding: new Binding("IsFactionIconVisible")
			{
				UseGeneratedBindings = true
			}, property: UIElement.VisibilityProperty);
			border.SetBinding(binding: new Binding("FactionIconBackgroundColor")
			{
				UseGeneratedBindings = true
			}, property: Control.BackgroundProperty);
			Image image5 = (Image)(border.Child = new Image());
			image5.Name = "FactionIcon";
			image5.Height = 32f;
			image5.Margin = new Thickness(0f, 0f, 0f, 0f);
			image5.Stretch = Stretch.Uniform;
			image5.SetBinding(binding: new Binding("FactionIconTooltip")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image5.SetBinding(binding: new Binding("FactionIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image5.SetBinding(binding: new Binding("FactionIconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}
	}
}
