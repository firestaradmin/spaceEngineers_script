using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesContracts_Bindings;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public sealed class DataTemplatesContracts : ResourceDictionary
	{
		private static DataTemplatesContracts singleton = new DataTemplatesContracts();

		public static DataTemplatesContracts Instance => singleton;

		public DataTemplatesContracts()
		{
			InitializeResources();
		}

		private void InitializeResources()
		{
			base.MergedDictionaries.Add(Styles.Instance);
			Func<UIElement, UIElement> createMethod = r_0_dtMethod;
			Add(typeof(MyAdminSelectionItemModel), new DataTemplate(typeof(MyAdminSelectionItemModel), createMethod));
			Func<UIElement, UIElement> createMethod2 = r_1_dtMethod;
			Add(typeof(MyContractConditionDeliverItemModel), new DataTemplate(typeof(MyContractConditionDeliverItemModel), createMethod2));
			Func<UIElement, UIElement> createMethod3 = r_2_dtMethod;
			Add(typeof(MyContractModelCustom), new DataTemplate(typeof(MyContractModelCustom), createMethod3));
			Func<UIElement, UIElement> createMethod4 = r_3_dtMethod;
			Add(typeof(MyContractModelDeliver), new DataTemplate(typeof(MyContractModelDeliver), createMethod4));
			Func<UIElement, UIElement> createMethod5 = r_4_dtMethod;
			Add(typeof(MyContractModelEscort), new DataTemplate(typeof(MyContractModelEscort), createMethod5));
			Func<UIElement, UIElement> createMethod6 = r_5_dtMethod;
			Add(typeof(MyContractModelFind), new DataTemplate(typeof(MyContractModelFind), createMethod6));
			Func<UIElement, UIElement> createMethod7 = r_6_dtMethod;
			Add(typeof(MyContractModelHunt), new DataTemplate(typeof(MyContractModelHunt), createMethod7));
			Func<UIElement, UIElement> createMethod8 = r_7_dtMethod;
			Add(typeof(MyContractModelObtainAndDeliver), new DataTemplate(typeof(MyContractModelObtainAndDeliver), createMethod8));
			Func<UIElement, UIElement> createMethod9 = r_8_dtMethod;
			Add(typeof(MyContractModelRepair), new DataTemplate(typeof(MyContractModelRepair), createMethod9));
			Func<UIElement, UIElement> createMethod10 = r_9_dtMethod;
			Add(typeof(MyContractTypeFilterItemModel), new DataTemplate(typeof(MyContractTypeFilterItemModel), createMethod10));
			Func<UIElement, UIElement> createMethod11 = r_10_dtMethod;
			Add(typeof(MyContractTypeModel), new DataTemplate(typeof(MyContractTypeModel), createMethod11));
			Func<UIElement, UIElement> createMethod12 = r_11_dtMethod;
			Add(typeof(MyDeliverItemModel), new DataTemplate(typeof(MyDeliverItemModel), createMethod12));
			Func<UIElement, UIElement> createMethod13 = r_12_dtMethod;
			Add(typeof(MySimpleSelectableItemModel), new DataTemplate(typeof(MySimpleSelectableItemModel), createMethod13));
			Func<UIElement, UIElement> createMethod14 = r_13_dtMethod;
			Add("SelectableGridTemplate", new DataTemplate(createMethod14));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyAdminSelectionItemModel), "NameCombined", typeof(MyAdminSelectionItemModel_NameCombined_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractConditionDeliverItemModel), "Icon", typeof(MyContractConditionDeliverItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractConditionDeliverItemModel), "Name", typeof(MyContractConditionDeliverItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractConditionDeliverItemModel), "ItemAmount", typeof(MyContractConditionDeliverItemModel_ItemAmount_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractConditionDeliverItemModel), "ItemVolume_Formated", typeof(MyContractConditionDeliverItemModel_ItemVolume_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Header", typeof(MyContractModelCustom_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Icon", typeof(MyContractModelCustom_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Name", typeof(MyContractModelCustom_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "RewardMoney_Formatted", typeof(MyContractModelCustom_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "CurrencyIcon", typeof(MyContractModelCustom_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "RewardReputation", typeof(MyContractModelCustom_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "FailReputationPenalty_Formated", typeof(MyContractModelCustom_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "InitialDeposit_Formated", typeof(MyContractModelCustom_InitialDeposit_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "TimeLeft", typeof(MyContractModelCustom_TimeLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Conditions", typeof(MyContractModelCustom_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelCustom), "Description", typeof(MyContractModelCustom_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Header", typeof(MyContractModelDeliver_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Icon", typeof(MyContractModelDeliver_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Name", typeof(MyContractModelDeliver_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "RewardMoney_Formatted", typeof(MyContractModelDeliver_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "CurrencyIcon", typeof(MyContractModelDeliver_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "RewardReputation", typeof(MyContractModelDeliver_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "FailReputationPenalty_Formated", typeof(MyContractModelDeliver_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "InitialDeposit_Formated", typeof(MyContractModelDeliver_InitialDeposit_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "DeliverDistance_Formatted", typeof(MyContractModelDeliver_DeliverDistance_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "TimeLeft", typeof(MyContractModelDeliver_TimeLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Conditions", typeof(MyContractModelDeliver_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelDeliver), "Description", typeof(MyContractModelDeliver_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Header", typeof(MyContractModelEscort_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Icon", typeof(MyContractModelEscort_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Name", typeof(MyContractModelEscort_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "RewardMoney_Formatted", typeof(MyContractModelEscort_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "CurrencyIcon", typeof(MyContractModelEscort_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "RewardReputation", typeof(MyContractModelEscort_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "FailReputationPenalty_Formated", typeof(MyContractModelEscort_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "PathLength_Formatted", typeof(MyContractModelEscort_PathLength_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "TimeLeft", typeof(MyContractModelEscort_TimeLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Conditions", typeof(MyContractModelEscort_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelEscort), "Description", typeof(MyContractModelEscort_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Header", typeof(MyContractModelFind_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Icon", typeof(MyContractModelFind_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Name", typeof(MyContractModelFind_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "RewardMoney_Formatted", typeof(MyContractModelFind_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "CurrencyIcon", typeof(MyContractModelFind_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "RewardReputation", typeof(MyContractModelFind_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "FailReputationPenalty_Formated", typeof(MyContractModelFind_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "TimeLeft", typeof(MyContractModelFind_TimeLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "MaxGpsOffset_Formatted", typeof(MyContractModelFind_MaxGpsOffset_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Conditions", typeof(MyContractModelFind_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelFind), "Description", typeof(MyContractModelFind_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Header", typeof(MyContractModelHunt_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Icon", typeof(MyContractModelHunt_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Name", typeof(MyContractModelHunt_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "RewardMoney_Formatted", typeof(MyContractModelHunt_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "CurrencyIcon", typeof(MyContractModelHunt_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "RewardReputation", typeof(MyContractModelHunt_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "TargetName_Formatted", typeof(MyContractModelHunt_TargetName_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "TimeLeft", typeof(MyContractModelHunt_TimeLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Conditions", typeof(MyContractModelHunt_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelHunt), "Description", typeof(MyContractModelHunt_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Header", typeof(MyContractModelObtainAndDeliver_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Icon", typeof(MyContractModelObtainAndDeliver_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Name", typeof(MyContractModelObtainAndDeliver_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "RewardMoney_Formatted", typeof(MyContractModelObtainAndDeliver_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "CurrencyIcon", typeof(MyContractModelObtainAndDeliver_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "RewardReputation", typeof(MyContractModelObtainAndDeliver_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Conditions", typeof(MyContractModelObtainAndDeliver_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelObtainAndDeliver), "Description", typeof(MyContractModelObtainAndDeliver_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Header", typeof(MyContractModelRepair_Header_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Icon", typeof(MyContractModelRepair_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Name", typeof(MyContractModelRepair_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "RewardMoney_Formatted", typeof(MyContractModelRepair_RewardMoney_Formatted_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "CurrencyIcon", typeof(MyContractModelRepair_CurrencyIcon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "RewardReputation", typeof(MyContractModelRepair_RewardReputation_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "FailReputationPenalty_Formated", typeof(MyContractModelRepair_FailReputationPenalty_Formated_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "TimeLeft", typeof(MyContractModelRepair_TimeLeft_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Conditions", typeof(MyContractModelRepair_Conditions_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractModelRepair), "Description", typeof(MyContractModelRepair_Description_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractTypeFilterItemModel), "Icon", typeof(MyContractTypeFilterItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractTypeFilterItemModel), "LocalizedName", typeof(MyContractTypeFilterItemModel_LocalizedName_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyContractTypeModel), "Name", typeof(MyContractTypeModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyDeliverItemModel), "Icon", typeof(MyDeliverItemModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyDeliverItemModel), "Name", typeof(MyDeliverItemModel_Name_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MySimpleSelectableItemModel), "DisplayName", typeof(MySimpleSelectableItemModel_DisplayName_PropertyInfo));
		}

		private static UIElement r_0_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_0",
				Orientation = Orientation.Horizontal
			};
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_1";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("NameCombined")
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
				Name = "e_2",
				Orientation = Orientation.Horizontal
			};
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_3";
			image.Width = 64f;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_4";
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item4);
			ColumnDefinition item5 = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item5);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_5";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Top;
			textBlock.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock, 0);
			Grid.SetRow(textBlock, 0);
			textBlock.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Name");
			TextBlock textBlock2 = new TextBlock();
			grid.Children.Add(textBlock2);
			textBlock2.Name = "e_6";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Top;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock2, 1);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock3 = new TextBlock();
			grid.Children.Add(textBlock3);
			textBlock3.Name = "e_7";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Top;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock3, 0);
			Grid.SetRow(textBlock3, 1);
			textBlock3.SetResourceReference(TextBlock.TextProperty, "ContractScreen_ObtainDeliver_ItemAmount");
			TextBlock textBlock4 = new TextBlock();
			grid.Children.Add(textBlock4);
			textBlock4.Name = "e_8";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Top;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock4, 1);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetBinding(binding: new Binding("ItemAmount")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock5 = new TextBlock();
			grid.Children.Add(textBlock5);
			textBlock5.Name = "e_9";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Top;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock5, 0);
			Grid.SetRow(textBlock5, 2);
			textBlock5.SetResourceReference(TextBlock.TextProperty, "ContractScreen_ObtainDeliver_ItemVolume");
			TextBlock textBlock6 = new TextBlock();
			grid.Children.Add(textBlock6);
			textBlock6.Name = "e_10";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Top;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock6, 1);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetBinding(binding: new Binding("ItemVolume_Formated")
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
				Name = "e_11"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_12";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_13";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_14";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_15";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_16";
			grid2.Margin = new Thickness(10f, 5f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition();
			grid2.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition();
			grid2.RowDefinitions.Add(item10);
			RowDefinition item11 = new RowDefinition();
			grid2.RowDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item12);
			ColumnDefinition item13 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item13);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_17";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_18";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_19";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 1);
			Grid.SetRow(textBlock3, 0);
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_20";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_21";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_22";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid2.Children.Add(textBlock6);
			textBlock6.Name = "e_23";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 0);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_FailReputationPenalty");
			TextBlock textBlock7 = new TextBlock();
			grid2.Children.Add(textBlock7);
			textBlock7.Name = "e_24";
			textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 1);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock8 = new TextBlock();
			grid2.Children.Add(textBlock8);
			textBlock8.Name = "e_25";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 3);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_InitialDeposit");
			TextBlock textBlock9 = new TextBlock();
			grid2.Children.Add(textBlock9);
			textBlock9.Name = "e_26";
			textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 1);
			Grid.SetRow(textBlock9, 3);
			textBlock9.SetBinding(binding: new Binding("InitialDeposit_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock10 = new TextBlock();
			grid2.Children.Add(textBlock10);
			textBlock10.Name = "e_27";
			textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock10, 0);
			Grid.SetRow(textBlock10, 4);
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Duration");
			TextBlock textBlock11 = new TextBlock();
			grid2.Children.Add(textBlock11);
			textBlock11.Name = "e_28";
			textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 1);
			Grid.SetRow(textBlock11, 4);
			textBlock11.SetBinding(binding: new Binding("TimeLeft")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_29";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock12 = new TextBlock();
			obj.Children.Add(textBlock12);
			textBlock12.Name = "e_30";
			textBlock12.UseLayoutRounding = true;
			textBlock12.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock12.TextAlignment = TextAlignment.Left;
			textBlock12.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock12, 3);
			textBlock12.SetBinding(binding: new Binding("Description")
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
				Name = "e_31"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_32";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_33";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_34";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_35";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_36";
			grid2.Margin = new Thickness(10f, 0f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition();
			grid2.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition();
			grid2.RowDefinitions.Add(item10);
			RowDefinition item11 = new RowDefinition();
			grid2.RowDefinitions.Add(item11);
			RowDefinition item12 = new RowDefinition();
			grid2.RowDefinitions.Add(item12);
			ColumnDefinition item13 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item13);
			ColumnDefinition item14 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item14);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_37";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_38";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_39";
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_40";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_41";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_42";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid2.Children.Add(textBlock6);
			textBlock6.Name = "e_43";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 0);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_FailReputationPenalty");
			TextBlock textBlock7 = new TextBlock();
			grid2.Children.Add(textBlock7);
			textBlock7.Name = "e_44";
			textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 1);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock8 = new TextBlock();
			grid2.Children.Add(textBlock8);
			textBlock8.Name = "e_45";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 3);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_InitialDeposit");
			TextBlock textBlock9 = new TextBlock();
			grid2.Children.Add(textBlock9);
			textBlock9.Name = "e_46";
			textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 1);
			Grid.SetRow(textBlock9, 3);
			textBlock9.SetBinding(binding: new Binding("InitialDeposit_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock10 = new TextBlock();
			grid2.Children.Add(textBlock10);
			textBlock10.Name = "e_47";
			textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock10, 0);
			Grid.SetRow(textBlock10, 4);
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Deliver_Distance");
			TextBlock textBlock11 = new TextBlock();
			grid2.Children.Add(textBlock11);
			textBlock11.Name = "e_48";
			textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 1);
			Grid.SetRow(textBlock11, 4);
			textBlock11.SetBinding(binding: new Binding("DeliverDistance_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock12 = new TextBlock();
			grid2.Children.Add(textBlock12);
			textBlock12.Name = "e_49";
			textBlock12.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock12.VerticalAlignment = VerticalAlignment.Center;
			textBlock12.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock12, 0);
			Grid.SetRow(textBlock12, 5);
			textBlock12.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Duration");
			TextBlock textBlock13 = new TextBlock();
			grid2.Children.Add(textBlock13);
			textBlock13.Name = "e_50";
			textBlock13.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			textBlock13.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock13, 1);
			Grid.SetRow(textBlock13, 5);
			textBlock13.SetBinding(binding: new Binding("TimeLeft")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_51";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock14 = new TextBlock();
			obj.Children.Add(textBlock14);
			textBlock14.Name = "e_52";
			textBlock14.UseLayoutRounding = true;
			textBlock14.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock14.TextAlignment = TextAlignment.Left;
			textBlock14.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock14, 3);
			textBlock14.SetBinding(binding: new Binding("Description")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_4_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_53"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_54";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_55";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_56";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_57";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_58";
			grid2.Margin = new Thickness(10f, 5f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition();
			grid2.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition();
			grid2.RowDefinitions.Add(item10);
			RowDefinition item11 = new RowDefinition();
			grid2.RowDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item12);
			ColumnDefinition item13 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item13);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_59";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_60";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_61";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 1);
			Grid.SetRow(textBlock3, 0);
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_62";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_63";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_64";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid2.Children.Add(textBlock6);
			textBlock6.Name = "e_65";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 0);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_FailReputationPenalty");
			TextBlock textBlock7 = new TextBlock();
			grid2.Children.Add(textBlock7);
			textBlock7.Name = "e_66";
			textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 1);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock8 = new TextBlock();
			grid2.Children.Add(textBlock8);
			textBlock8.Name = "e_67";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 3);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Deliver_Distance");
			TextBlock textBlock9 = new TextBlock();
			grid2.Children.Add(textBlock9);
			textBlock9.Name = "e_68";
			textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 1);
			Grid.SetRow(textBlock9, 3);
			textBlock9.SetBinding(binding: new Binding("PathLength_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock10 = new TextBlock();
			grid2.Children.Add(textBlock10);
			textBlock10.Name = "e_69";
			textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock10, 0);
			Grid.SetRow(textBlock10, 4);
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Duration");
			TextBlock textBlock11 = new TextBlock();
			grid2.Children.Add(textBlock11);
			textBlock11.Name = "e_70";
			textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 1);
			Grid.SetRow(textBlock11, 4);
			textBlock11.SetBinding(binding: new Binding("TimeLeft")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_71";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock12 = new TextBlock();
			obj.Children.Add(textBlock12);
			textBlock12.Name = "e_72";
			textBlock12.UseLayoutRounding = true;
			textBlock12.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock12.TextAlignment = TextAlignment.Left;
			textBlock12.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock12, 3);
			textBlock12.SetBinding(binding: new Binding("Description")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_5_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_73"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_74";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_75";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_76";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_77";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_78";
			grid2.Margin = new Thickness(10f, 5f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition();
			grid2.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition();
			grid2.RowDefinitions.Add(item10);
			RowDefinition item11 = new RowDefinition();
			grid2.RowDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item12);
			ColumnDefinition item13 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item13);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_79";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_80";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_81";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 1);
			Grid.SetRow(textBlock3, 0);
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_82";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_83";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_84";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid2.Children.Add(textBlock6);
			textBlock6.Name = "e_85";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 0);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_FailReputationPenalty");
			TextBlock textBlock7 = new TextBlock();
			grid2.Children.Add(textBlock7);
			textBlock7.Name = "e_86";
			textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 1);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock8 = new TextBlock();
			grid2.Children.Add(textBlock8);
			textBlock8.Name = "e_87";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 3);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Duration");
			TextBlock textBlock9 = new TextBlock();
			grid2.Children.Add(textBlock9);
			textBlock9.Name = "e_88";
			textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 1);
			Grid.SetRow(textBlock9, 3);
			textBlock9.SetBinding(binding: new Binding("TimeLeft")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock10 = new TextBlock();
			grid2.Children.Add(textBlock10);
			textBlock10.Name = "e_89";
			textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock10.VerticalAlignment = VerticalAlignment.Center;
			textBlock10.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock10, 0);
			Grid.SetRow(textBlock10, 4);
			textBlock10.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_MaxGpsOffset");
			TextBlock textBlock11 = new TextBlock();
			grid2.Children.Add(textBlock11);
			textBlock11.Name = "e_90";
			textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock11.VerticalAlignment = VerticalAlignment.Center;
			textBlock11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock11, 1);
			Grid.SetRow(textBlock11, 4);
			textBlock11.SetBinding(binding: new Binding("MaxGpsOffset_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_91";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock12 = new TextBlock();
			obj.Children.Add(textBlock12);
			textBlock12.Name = "e_92";
			textBlock12.UseLayoutRounding = true;
			textBlock12.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock12.TextAlignment = TextAlignment.Left;
			textBlock12.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock12, 3);
			textBlock12.SetBinding(binding: new Binding("Description")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_6_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_93"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_94";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_95";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_96";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_97";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_98";
			grid2.Margin = new Thickness(10f, 5f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition();
			grid2.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition();
			grid2.RowDefinitions.Add(item10);
			ColumnDefinition item11 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item12);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_99";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_100";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_101";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 1);
			Grid.SetRow(textBlock3, 0);
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_102";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_103";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_104";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid2.Children.Add(textBlock6);
			textBlock6.Name = "e_105";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 0);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Hunt_Target");
			TextBlock textBlock7 = new TextBlock();
			grid2.Children.Add(textBlock7);
			textBlock7.Name = "e_106";
			textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 1);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetBinding(binding: new Binding("TargetName_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock8 = new TextBlock();
			grid2.Children.Add(textBlock8);
			textBlock8.Name = "e_107";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 3);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Duration");
			TextBlock textBlock9 = new TextBlock();
			grid2.Children.Add(textBlock9);
			textBlock9.Name = "e_108";
			textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 1);
			Grid.SetRow(textBlock9, 3);
			textBlock9.SetBinding(binding: new Binding("TimeLeft")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_109";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock10 = new TextBlock();
			obj.Children.Add(textBlock10);
			textBlock10.Name = "e_110";
			textBlock10.UseLayoutRounding = true;
			textBlock10.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock10.TextAlignment = TextAlignment.Left;
			textBlock10.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock10, 3);
			textBlock10.SetBinding(binding: new Binding("Description")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_7_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_111"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_112";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_113";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_114";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_115";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_116";
			grid2.Margin = new Thickness(10f, 5f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			ColumnDefinition item9 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item9);
			ColumnDefinition item10 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item10);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_117";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_118";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_119";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 1);
			Grid.SetRow(textBlock3, 0);
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_120";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_121";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_122";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_123";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock6 = new TextBlock();
			obj.Children.Add(textBlock6);
			textBlock6.Name = "e_124";
			textBlock6.UseLayoutRounding = true;
			textBlock6.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock6.TextAlignment = TextAlignment.Left;
			textBlock6.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock6, 3);
			textBlock6.SetBinding(binding: new Binding("Description")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_8_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_125"
			};
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
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
			RowDefinition item4 = new RowDefinition();
			obj.RowDefinitions.Add(item4);
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_126";
			image.Height = 192f;
			image.HorizontalAlignment = HorizontalAlignment.Right;
			image.VerticalAlignment = VerticalAlignment.Top;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Header")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_127";
			grid.Margin = new Thickness(10f, 10f, 10f, 0f);
			grid.HorizontalAlignment = HorizontalAlignment.Left;
			grid.VerticalAlignment = VerticalAlignment.Top;
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item6);
			Image image2 = new Image();
			grid.Children.Add(image2);
			image2.Name = "e_128";
			image2.Width = 80f;
			image2.HorizontalAlignment = HorizontalAlignment.Left;
			image2.Stretch = Stretch.Uniform;
			ImageBrush.SetColorOverlay(image2, new ColorW(131, 201, 226, 255));
			image2.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			grid.Children.Add(textBlock);
			textBlock.Name = "e_129";
			textBlock.Margin = new Thickness(0f, 5f, 0f, 0f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetRow(textBlock, 1);
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Grid grid2 = new Grid();
			obj.Children.Add(grid2);
			grid2.Name = "e_130";
			grid2.Margin = new Thickness(10f, 5f, 10f, 5f);
			RowDefinition item7 = new RowDefinition();
			grid2.RowDefinitions.Add(item7);
			RowDefinition item8 = new RowDefinition();
			grid2.RowDefinitions.Add(item8);
			RowDefinition item9 = new RowDefinition();
			grid2.RowDefinitions.Add(item9);
			RowDefinition item10 = new RowDefinition();
			grid2.RowDefinitions.Add(item10);
			ColumnDefinition item11 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item11);
			ColumnDefinition item12 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item12);
			Grid.SetRow(grid2, 1);
			TextBlock textBlock2 = new TextBlock();
			grid2.Children.Add(textBlock2);
			textBlock2.Name = "e_131";
			textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock2, 0);
			Grid.SetRow(textBlock2, 0);
			textBlock2.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Currency");
			StackPanel stackPanel = new StackPanel();
			grid2.Children.Add(stackPanel);
			stackPanel.Name = "e_132";
			stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			Grid.SetRow(stackPanel, 0);
			TextBlock textBlock3 = new TextBlock();
			stackPanel.Children.Add(textBlock3);
			textBlock3.Name = "e_133";
			textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock3.VerticalAlignment = VerticalAlignment.Center;
			textBlock3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock3, 1);
			Grid.SetRow(textBlock3, 0);
			textBlock3.SetBinding(binding: new Binding("RewardMoney_Formatted")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			Image image3 = new Image();
			stackPanel.Children.Add(image3);
			image3.Name = "e_134";
			image3.Height = 20f;
			image3.Margin = new Thickness(4f, 2f, 2f, 2f);
			image3.HorizontalAlignment = HorizontalAlignment.Left;
			image3.VerticalAlignment = VerticalAlignment.Center;
			image3.Stretch = Stretch.Uniform;
			Grid.SetColumn(image3, 2);
			image3.SetBinding(binding: new Binding("CurrencyIcon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock4 = new TextBlock();
			grid2.Children.Add(textBlock4);
			textBlock4.Name = "e_135";
			textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock4, 0);
			Grid.SetRow(textBlock4, 1);
			textBlock4.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Reputation");
			TextBlock textBlock5 = new TextBlock();
			grid2.Children.Add(textBlock5);
			textBlock5.Name = "e_136";
			textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock5, 1);
			Grid.SetRow(textBlock5, 1);
			textBlock5.SetBinding(binding: new Binding("RewardReputation")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock6 = new TextBlock();
			grid2.Children.Add(textBlock6);
			textBlock6.Name = "e_137";
			textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock6.VerticalAlignment = VerticalAlignment.Center;
			textBlock6.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock6, 0);
			Grid.SetRow(textBlock6, 2);
			textBlock6.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_FailReputationPenalty");
			TextBlock textBlock7 = new TextBlock();
			grid2.Children.Add(textBlock7);
			textBlock7.Name = "e_138";
			textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock7.VerticalAlignment = VerticalAlignment.Center;
			textBlock7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock7, 1);
			Grid.SetRow(textBlock7, 2);
			textBlock7.SetBinding(binding: new Binding("FailReputationPenalty_Formated")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			TextBlock textBlock8 = new TextBlock();
			grid2.Children.Add(textBlock8);
			textBlock8.Name = "e_139";
			textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock8.VerticalAlignment = VerticalAlignment.Center;
			textBlock8.Foreground = new SolidColorBrush(new ColorW(198, 220, 228, 255));
			Grid.SetColumn(textBlock8, 0);
			Grid.SetRow(textBlock8, 3);
			textBlock8.SetResourceReference(TextBlock.TextProperty, "ContractScreen_Tooltip_Duration");
			TextBlock textBlock9 = new TextBlock();
			grid2.Children.Add(textBlock9);
			textBlock9.Name = "e_140";
			textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			textBlock9.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
			Grid.SetColumn(textBlock9, 1);
			Grid.SetRow(textBlock9, 3);
			textBlock9.SetBinding(binding: new Binding("TimeLeft")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			ItemsControl itemsControl = new ItemsControl();
			obj.Children.Add(itemsControl);
			itemsControl.Name = "e_141";
			itemsControl.Margin = new Thickness(10f, 0f, 10f, 0f);
			Grid.SetRow(itemsControl, 2);
			itemsControl.SetBinding(binding: new Binding("Conditions")
			{
				UseGeneratedBindings = true
			}, property: ItemsControl.ItemsSourceProperty);
			TextBlock textBlock10 = new TextBlock();
			obj.Children.Add(textBlock10);
			textBlock10.Name = "e_142";
			textBlock10.UseLayoutRounding = true;
			textBlock10.Margin = new Thickness(10f, 0f, 10f, 0f);
			textBlock10.TextAlignment = TextAlignment.Left;
			textBlock10.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(textBlock10, 3);
			textBlock10.SetBinding(binding: new Binding("Description")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_9_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_143",
				Margin = new Thickness(5f, 5f, 5f, 5f),
				Orientation = Orientation.Horizontal
			};
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_144";
			image.Width = 24f;
			image.Margin = new Thickness(0f, 0f, 5f, 0f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_145";
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("LocalizedName")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_10_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_146",
				Margin = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_11_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_147",
				Orientation = Orientation.Horizontal
			};
			Image image = new Image();
			obj.Children.Add(image);
			image.Name = "e_148";
			image.Width = 24f;
			image.Margin = new Thickness(2f, 2f, 2f, 2f);
			image.VerticalAlignment = VerticalAlignment.Center;
			image.Stretch = Stretch.Uniform;
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_149";
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("Name")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_12_dtMethod(UIElement parent)
		{
			TextBlock obj = new TextBlock
			{
				Parent = parent,
				Name = "e_150",
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("DisplayName")
			{
				UseGeneratedBindings = true
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_13_dtMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_151",
				Orientation = Orientation.Horizontal
			};
			TextBlock textBlock = new TextBlock();
			obj.Children.Add(textBlock);
			textBlock.Name = "e_152";
			textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.SetBinding(binding: new Binding("DisplayName"), property: TextBlock.TextProperty);
			return obj;
		}
	}
}
