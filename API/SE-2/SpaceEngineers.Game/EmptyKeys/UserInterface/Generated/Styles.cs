using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public sealed class Styles : ResourceDictionary
	{
		private static Styles singleton = new Styles();

		public static Styles Instance => singleton;

		public Styles()
		{
			InitializeResources();
		}

		private void InitializeResources()
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = "Textures\\GUI\\Icons\\Contracts\\AcquisitionContractHeader.dds";
			Add("AcquisitionContract", bitmapImage);
			BitmapImage bitmapImage2 = new BitmapImage();
			bitmapImage2.TextureAsset = "Textures\\GUI\\Bg16x9.png";
			Add("Background16x9", bitmapImage2);
			BitmapImage bitmapImage3 = new BitmapImage();
			bitmapImage3.TextureAsset = "Textures\\GUI\\Icons\\Contracts\\BountyContractHeader.dds";
			Add("BountyContract", bitmapImage3);
			BitmapImage bitmapImage4 = new BitmapImage();
			bitmapImage4.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds";
			Add("ButtonArrowLeft", bitmapImage4);
			BitmapImage bitmapImage5 = new BitmapImage();
			bitmapImage5.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds";
			Add("ButtonArrowLeftHighlight", bitmapImage5);
			BitmapImage bitmapImage6 = new BitmapImage();
			bitmapImage6.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds";
			Add("ButtonArrowRight", bitmapImage6);
			BitmapImage bitmapImage7 = new BitmapImage();
			bitmapImage7.TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds";
			Add("ButtonArrowRightHighlight", bitmapImage7);
			ImageBrush imageBrush = new ImageBrush();
			BitmapImage bitmapImage8 = new BitmapImage();
			bitmapImage8.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush.ImageSource = bitmapImage8;
			Add("ButtonBackgroundBrush", imageBrush);
			Add("ButtonBackgroundDisabledBrush", new SolidColorBrush(new ColorW(29, 39, 45, 255)));
			ImageBrush imageBrush2 = new ImageBrush();
			BitmapImage bitmapImage9 = new BitmapImage();
			bitmapImage9.TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds";
			imageBrush2.ImageSource = bitmapImage9;
			Add("ButtonBackgroundFocusedBrush", imageBrush2);
			ImageBrush imageBrush3 = new ImageBrush();
			BitmapImage bitmapImage10 = new BitmapImage();
			bitmapImage10.TextureAsset = "Textures\\GUI\\Controls\\button_default_highlight.dds";
			imageBrush3.ImageSource = bitmapImage10;
			Add("ButtonBackgroundHoverBrush", imageBrush3);
			ImageBrush imageBrush4 = new ImageBrush();
			BitmapImage bitmapImage11 = new BitmapImage();
			bitmapImage11.TextureAsset = "Textures\\GUI\\Controls\\button_default_active.dds";
			imageBrush4.ImageSource = bitmapImage11;
			Add("ButtonBackgroundPressedBrush", imageBrush4);
			ImageBrush imageBrush5 = new ImageBrush();
			BitmapImage bitmapImage12 = new BitmapImage();
			bitmapImage12.TextureAsset = "Textures\\GUI\\Controls\\button_decrease.dds";
			imageBrush5.ImageSource = bitmapImage12;
			Add("ButtonDecrease", imageBrush5);
			ImageBrush imageBrush6 = new ImageBrush();
			BitmapImage bitmapImage13 = new BitmapImage();
			bitmapImage13.TextureAsset = "Textures\\GUI\\Controls\\button_decrease_highlight.dds";
			imageBrush6.ImageSource = bitmapImage13;
			Add("ButtonDecreaseHover", imageBrush6);
			ImageBrush imageBrush7 = new ImageBrush();
			BitmapImage bitmapImage14 = new BitmapImage();
			bitmapImage14.TextureAsset = "Textures\\GUI\\Controls\\button_increase.dds";
			imageBrush7.ImageSource = bitmapImage14;
			Add("ButtonIncrease", imageBrush7);
			ImageBrush imageBrush8 = new ImageBrush();
			BitmapImage bitmapImage15 = new BitmapImage();
			bitmapImage15.TextureAsset = "Textures\\GUI\\Controls\\button_increase_highlight.dds";
			imageBrush8.ImageSource = bitmapImage15;
			Add("ButtonIncreaseHover", imageBrush8);
			Add("ButtonTextColor", new SolidColorBrush(new ColorW(198, 220, 228, 255)));
			Add("ButtonTextDisabledColor", new SolidColorBrush(new ColorW(93, 105, 110, 255)));
			Add("ButtonTextFocusedColor", new SolidColorBrush(new ColorW(33, 40, 45, 255)));
			Add("ButtonTextHoverColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("ButtonTextPressedColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			ImageBrush imageBrush9 = new ImageBrush();
			BitmapImage bitmapImage16 = new BitmapImage();
			bitmapImage16.TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked.dds";
			imageBrush9.ImageSource = bitmapImage16;
			Add("CheckBoxBackgroundBrush", imageBrush9);
			ImageBrush imageBrush10 = new ImageBrush();
			BitmapImage bitmapImage17 = new BitmapImage();
			bitmapImage17.TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked_focus.dds";
			imageBrush10.ImageSource = bitmapImage17;
			Add("CheckBoxFocusedBackgroundBrush", imageBrush10);
			ImageBrush imageBrush11 = new ImageBrush();
			BitmapImage bitmapImage18 = new BitmapImage();
			bitmapImage18.TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked_highlight.dds";
			imageBrush11.ImageSource = bitmapImage18;
			Add("CheckBoxHoverBackgroundBrush", imageBrush11);
			Style style = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod = r_24_s_S_0_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(CheckBox), createMethod);
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
			Setter item = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item);
			Add("CheckBoxServices0", style);
			Style style2 = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod2 = r_25_s_S_0_ctMethod;
			ControlTemplate controlTemplate2 = new ControlTemplate(typeof(CheckBox), createMethod2);
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
			ImageBrush imageBrush12 = new ImageBrush();
			BitmapImage bitmapImage19 = new BitmapImage();
			bitmapImage19.TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png";
			imageBrush12.ImageSource = bitmapImage19;
			Setter setter6 = new Setter(Control.BackgroundProperty, imageBrush12);
			setter6.TargetName = "PART_Background";
			trigger5.Setters.Add(setter6);
			ImageBrush imageBrush13 = new ImageBrush();
			BitmapImage bitmapImage20 = new BitmapImage();
			bitmapImage20.TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png";
			imageBrush13.ImageSource = bitmapImage20;
			Setter setter7 = new Setter(Control.BackgroundProperty, imageBrush13);
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
			Setter item2 = new Setter(Control.TemplateProperty, controlTemplate2);
			style2.Setters.Add(item2);
			Add("CheckBoxServices1", style2);
			ImageBrush imageBrush14 = new ImageBrush();
			BitmapImage bitmapImage21 = new BitmapImage();
			bitmapImage21.TextureAsset = "Textures\\GUI\\Controls\\checkbox_checked.dds";
			imageBrush14.ImageSource = bitmapImage21;
			Add("CheckImageBrush", imageBrush14);
			ImageBrush imageBrush15 = new ImageBrush();
			BitmapImage bitmapImage22 = new BitmapImage();
			bitmapImage22.TextureAsset = "Textures\\GUI\\Icons\\Browser\\SteamCB.png";
			imageBrush15.ImageSource = bitmapImage22;
			Add("CheckService0", imageBrush15);
			ImageBrush imageBrush16 = new ImageBrush();
			BitmapImage bitmapImage23 = new BitmapImage();
			bitmapImage23.TextureAsset = "Textures\\GUI\\Icons\\Browser\\SteamCBFocused.png";
			imageBrush16.ImageSource = bitmapImage23;
			Add("CheckService0Focused", imageBrush16);
			ImageBrush imageBrush17 = new ImageBrush();
			BitmapImage bitmapImage24 = new BitmapImage();
			bitmapImage24.TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCB.png";
			imageBrush17.ImageSource = bitmapImage24;
			Add("CheckService1", imageBrush17);
			ImageBrush imageBrush18 = new ImageBrush();
			BitmapImage bitmapImage25 = new BitmapImage();
			bitmapImage25.TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png";
			imageBrush18.ImageSource = bitmapImage25;
			Add("CheckService1Focused", imageBrush18);
			ImageBrush imageBrush19 = new ImageBrush();
			BitmapImage bitmapImage26 = new BitmapImage();
			bitmapImage26.TextureAsset = "Textures\\GUI\\Icons\\Browser\\Background.png";
			imageBrush19.ImageSource = bitmapImage26;
			Add("CheckServiceBack", imageBrush19);
			ImageBrush imageBrush20 = new ImageBrush();
			BitmapImage bitmapImage27 = new BitmapImage();
			bitmapImage27.TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundChecked.png";
			imageBrush20.ImageSource = bitmapImage27;
			Add("CheckServiceBackChecked", imageBrush20);
			ImageBrush imageBrush21 = new ImageBrush();
			BitmapImage bitmapImage28 = new BitmapImage();
			bitmapImage28.TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png";
			imageBrush21.ImageSource = bitmapImage28;
			Add("CheckServiceBackFocused", imageBrush21);
			ImageBrush imageBrush22 = new ImageBrush();
			BitmapImage bitmapImage29 = new BitmapImage();
			bitmapImage29.TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundHighlighted.png";
			imageBrush22.ImageSource = bitmapImage29;
			Add("CheckServiceBackHighlighted", imageBrush22);
			BitmapImage bitmapImage30 = new BitmapImage();
			bitmapImage30.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			Add("CloseScreenButtonIcon", bitmapImage30);
			BitmapImage bitmapImage31 = new BitmapImage();
			bitmapImage31.TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds";
			Add("CloseScreenButtonIconHover", bitmapImage31);
			ImageBrush imageBrush23 = new ImageBrush();
			BitmapImage bitmapImage32 = new BitmapImage();
			bitmapImage32.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_center.dds";
			imageBrush23.ImageSource = bitmapImage32;
			imageBrush23.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundCenter", imageBrush23);
			ImageBrush imageBrush24 = new ImageBrush();
			BitmapImage bitmapImage33 = new BitmapImage();
			bitmapImage33.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_focus_center.dds";
			imageBrush24.ImageSource = bitmapImage33;
			imageBrush24.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundCenterFocus", imageBrush24);
			ImageBrush imageBrush25 = new ImageBrush();
			BitmapImage bitmapImage34 = new BitmapImage();
			bitmapImage34.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_highlight_center.dds";
			imageBrush25.ImageSource = bitmapImage34;
			imageBrush25.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundCenterHighlight", imageBrush25);
			ImageBrush imageBrush26 = new ImageBrush();
			BitmapImage bitmapImage35 = new BitmapImage();
			bitmapImage35.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_left.dds";
			imageBrush26.ImageSource = bitmapImage35;
			imageBrush26.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundLeft", imageBrush26);
			ImageBrush imageBrush27 = new ImageBrush();
			BitmapImage bitmapImage36 = new BitmapImage();
			bitmapImage36.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_focus_left.dds";
			imageBrush27.ImageSource = bitmapImage36;
			imageBrush27.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundLeftFocus", imageBrush27);
			ImageBrush imageBrush28 = new ImageBrush();
			BitmapImage bitmapImage37 = new BitmapImage();
			bitmapImage37.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_highlight_left.dds";
			imageBrush28.ImageSource = bitmapImage37;
			imageBrush28.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundLeftHighlight", imageBrush28);
			ImageBrush imageBrush29 = new ImageBrush();
			BitmapImage bitmapImage38 = new BitmapImage();
			bitmapImage38.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_right.dds";
			imageBrush29.ImageSource = bitmapImage38;
			imageBrush29.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundRight", imageBrush29);
			ImageBrush imageBrush30 = new ImageBrush();
			BitmapImage bitmapImage39 = new BitmapImage();
			bitmapImage39.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_focus_right.dds";
			imageBrush30.ImageSource = bitmapImage39;
			imageBrush30.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundRightFocus", imageBrush30);
			ImageBrush imageBrush31 = new ImageBrush();
			BitmapImage bitmapImage40 = new BitmapImage();
			bitmapImage40.TextureAsset = "Textures\\GUI\\Controls\\combobox_default_highlight_right.dds";
			imageBrush31.ImageSource = bitmapImage40;
			imageBrush31.Stretch = Stretch.Fill;
			Add("ComboBoxBackgroundRightHighlight", imageBrush31);
			Add("DataGridHeaderBackground", new SolidColorBrush(new ColorW(33, 44, 53, 255)));
			Add("DataGridHeaderBackgroundPressed", new SolidColorBrush(new ColorW(63, 71, 79, 255)));
			Add("DataGridHeaderForeground", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("DataGridHeaderForegroundPressed", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("DataGridRowBackgroundSelected", new SolidColorBrush(new ColorW(91, 115, 123, 255)));
			Add("DataGridRowForeground", new SolidColorBrush(new ColorW(198, 220, 228, 255)));
			Add("DataGridRowForegroundPressed", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("DiscountPrice", new SolidColorBrush(new ColorW(198, 44, 20, 255)));
			Style style3 = new Style(typeof(NumericTextBox));
			Setter item3 = new Setter(TextBoxBase.CaretBrushProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			style3.Setters.Add(item3);
			Setter item4 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style3.Setters.Add(item4);
			Setter item5 = new Setter(TextBoxBase.SelectionBrushProperty, new SolidColorBrush(new ColorW(63, 71, 79, 255)));
			style3.Setters.Add(item5);
			Setter item6 = new Setter(TextBoxBase.TextAlignmentProperty, TextAlignment.Right);
			style3.Setters.Add(item6);
			Setter item7 = new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Right);
			style3.Setters.Add(item7);
			Func<UIElement, UIElement> createMethod3 = r_54_s_S_5_ctMethod;
			ControlTemplate controlTemplate3 = new ControlTemplate(typeof(NumericTextBox), createMethod3);
			Trigger trigger7 = new Trigger();
			trigger7.Property = UIElement.IsMouseOverProperty;
			trigger7.Value = true;
			Setter setter9 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxLeftHighlight"));
			setter9.TargetName = "PART_TextBoxLeft";
			trigger7.Setters.Add(setter9);
			Setter setter10 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxCenterHighlight"));
			setter10.TargetName = "PART_TextBoxCenter";
			trigger7.Setters.Add(setter10);
			Setter setter11 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxRightHighlight"));
			setter11.TargetName = "PART_TextBoxRight";
			trigger7.Setters.Add(setter11);
			Setter item8 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			trigger7.Setters.Add(item8);
			controlTemplate3.Triggers.Add(trigger7);
			Trigger trigger8 = new Trigger();
			trigger8.Property = UIElement.IsFocusedProperty;
			trigger8.Value = true;
			Setter setter12 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxLeftFocus"));
			setter12.TargetName = "PART_TextBoxLeft";
			trigger8.Setters.Add(setter12);
			Setter setter13 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxCenterFocus"));
			setter13.TargetName = "PART_TextBoxCenter";
			trigger8.Setters.Add(setter13);
			Setter setter14 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxRightFocus"));
			setter14.TargetName = "PART_TextBoxRight";
			trigger8.Setters.Add(setter14);
			Setter item9 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(33, 40, 45, 255)));
			trigger8.Setters.Add(item9);
			controlTemplate3.Triggers.Add(trigger8);
			Setter item10 = new Setter(Control.TemplateProperty, controlTemplate3);
			style3.Setters.Add(item10);
			Add(typeof(NumericTextBox), style3);
			BitmapImage bitmapImage41 = new BitmapImage();
			bitmapImage41.TextureAsset = "Textures\\GUI\\Icons\\Contracts\\EscortContractHeader.dds";
			Add("EscortContract", bitmapImage41);
			BitmapImage bitmapImage42 = new BitmapImage();
			bitmapImage42.TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png";
			Add("ExclamationMark", bitmapImage42);
			BitmapImage bitmapImage43 = new BitmapImage();
			bitmapImage43.TextureAsset = "Textures\\GUI\\Icons\\Rating\\FullStar.png";
			Add("FullStar", bitmapImage43);
			ImageBrush imageBrush32 = new ImageBrush();
			BitmapImage bitmapImage44 = new BitmapImage();
			bitmapImage44.TextureAsset = "Textures\\GUI\\Controls\\grid_item.dds";
			imageBrush32.ImageSource = bitmapImage44;
			Add("GridItem", imageBrush32);
			ImageBrush imageBrush33 = new ImageBrush();
			BitmapImage bitmapImage45 = new BitmapImage();
			bitmapImage45.TextureAsset = "Textures\\GUI\\Controls\\grid_item_highlight.dds";
			imageBrush33.ImageSource = bitmapImage45;
			Add("GridItemHover", imageBrush33);
			BitmapImage bitmapImage46 = new BitmapImage();
			bitmapImage46.TextureAsset = "Textures\\GUI\\Icons\\Rating\\HalfStar.png";
			Add("HalfStar", bitmapImage46);
			BitmapImage bitmapImage47 = new BitmapImage();
			bitmapImage47.TextureAsset = "Textures\\GUI\\Icons\\Contracts\\HaulingContractHeader.dds";
			Add("HaulingContract", bitmapImage47);
			Func<UIElement, UIElement> createMethod4 = r_62_ctMethod;
			ControlTemplate controlTemplate4 = new ControlTemplate(typeof(Slider), createMethod4);
			Trigger trigger9 = new Trigger();
			trigger9.Property = UIElement.IsMouseOverProperty;
			trigger9.Value = true;
			Setter setter15 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderRailLeftHighlight"));
			setter15.TargetName = "PART_SliderRailLeft";
			trigger9.Setters.Add(setter15);
			Setter setter16 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderRailCenterHighlight"));
			setter16.TargetName = "PART_SliderRailCenter";
			trigger9.Setters.Add(setter16);
			Setter setter17 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderRailRightHighlight"));
			setter17.TargetName = "PART_SliderRailRight";
			trigger9.Setters.Add(setter17);
			controlTemplate4.Triggers.Add(trigger9);
			Trigger trigger10 = new Trigger();
			trigger10.Property = UIElement.IsFocusedProperty;
			trigger10.Value = true;
			Setter setter18 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderRailLeftFocus"));
			setter18.TargetName = "PART_SliderRailLeft";
			trigger10.Setters.Add(setter18);
			Setter setter19 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderRailCenterFocus"));
			setter19.TargetName = "PART_SliderRailCenter";
			trigger10.Setters.Add(setter19);
			Setter setter20 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderRailRightFocus"));
			setter20.TargetName = "PART_SliderRailRight";
			trigger10.Setters.Add(setter20);
			controlTemplate4.Triggers.Add(trigger10);
			Add("HorizontalSlider", controlTemplate4);
			Func<UIElement, UIElement> createMethod5 = r_63_ctMethod;
			ControlTemplate controlTemplate5 = new ControlTemplate(typeof(Slider), createMethod5);
			Trigger trigger11 = new Trigger();
			trigger11.Property = UIElement.IsMouseOverProperty;
			trigger11.Value = true;
			Setter setter21 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("HueColorGradientLeftHighlight"));
			setter21.TargetName = "PART_SliderRailLeft";
			trigger11.Setters.Add(setter21);
			Setter setter22 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("HueColorGradientCenterHighlight"));
			setter22.TargetName = "PART_SliderRailCenter";
			trigger11.Setters.Add(setter22);
			Setter setter23 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("HueColorGradientRightHighlight"));
			setter23.TargetName = "PART_SliderRailRight";
			trigger11.Setters.Add(setter23);
			controlTemplate5.Triggers.Add(trigger11);
			Trigger trigger12 = new Trigger();
			trigger12.Property = UIElement.IsFocusedProperty;
			trigger12.Value = true;
			Setter setter24 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("HueColorGradientLeftHighlight"));
			setter24.TargetName = "PART_SliderRailLeft";
			trigger12.Setters.Add(setter24);
			Setter setter25 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("HueColorGradientCenterHighlight"));
			setter25.TargetName = "PART_SliderRailCenter";
			trigger12.Setters.Add(setter25);
			Setter setter26 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("HueColorGradientRightHighlight"));
			setter26.TargetName = "PART_SliderRailRight";
			trigger12.Setters.Add(setter26);
			controlTemplate5.Triggers.Add(trigger12);
			Add("HorizontalSliderHuePicker", controlTemplate5);
			Style style4 = new Style(typeof(Thumb));
			Func<UIElement, UIElement> createMethod6 = r_64_s_S_0_ctMethod;
			ControlTemplate controlTemplate6 = new ControlTemplate(typeof(Thumb), createMethod6);
			Trigger trigger13 = new Trigger();
			trigger13.Property = UIElement.IsMouseOverProperty;
			trigger13.Value = true;
			Setter setter27 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ScrollbarHorizontalThumbCenterHighlight"));
			setter27.TargetName = "PART_Center";
			trigger13.Setters.Add(setter27);
			controlTemplate6.Triggers.Add(trigger13);
			Setter item11 = new Setter(Control.TemplateProperty, controlTemplate6);
			style4.Setters.Add(item11);
			Add("HorizontalThumb", style4);
			ImageBrush imageBrush34 = new ImageBrush();
			BitmapImage bitmapImage48 = new BitmapImage();
			bitmapImage48.TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_center.dds";
			imageBrush34.ImageSource = bitmapImage48;
			imageBrush34.Stretch = Stretch.Fill;
			Add("HueColorGradientCenter", imageBrush34);
			ImageBrush imageBrush35 = new ImageBrush();
			BitmapImage bitmapImage49 = new BitmapImage();
			bitmapImage49.TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_center_highlight.dds";
			imageBrush35.ImageSource = bitmapImage49;
			imageBrush35.Stretch = Stretch.Fill;
			Add("HueColorGradientCenterHighlight", imageBrush35);
			ImageBrush imageBrush36 = new ImageBrush();
			BitmapImage bitmapImage50 = new BitmapImage();
			bitmapImage50.TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_left.dds";
			imageBrush36.ImageSource = bitmapImage50;
			imageBrush36.Stretch = Stretch.Fill;
			Add("HueColorGradientLeft", imageBrush36);
			ImageBrush imageBrush37 = new ImageBrush();
			BitmapImage bitmapImage51 = new BitmapImage();
			bitmapImage51.TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_left_highlight.dds";
			imageBrush37.ImageSource = bitmapImage51;
			imageBrush37.Stretch = Stretch.Fill;
			Add("HueColorGradientLeftHighlight", imageBrush37);
			ImageBrush imageBrush38 = new ImageBrush();
			BitmapImage bitmapImage52 = new BitmapImage();
			bitmapImage52.TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_right.dds";
			imageBrush38.ImageSource = bitmapImage52;
			imageBrush38.Stretch = Stretch.Fill;
			Add("HueColorGradientRight", imageBrush38);
			ImageBrush imageBrush39 = new ImageBrush();
			BitmapImage bitmapImage53 = new BitmapImage();
			bitmapImage53.TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_right_highlight.dds";
			imageBrush39.ImageSource = bitmapImage53;
			imageBrush39.Stretch = Stretch.Fill;
			Add("HueColorGradientRightHighlight", imageBrush39);
			BitmapImage bitmapImage54 = new BitmapImage();
			bitmapImage54.TextureAsset = "Textures\\GUI\\Icons\\HydrogenIcon.dds";
			Add("HydrogenIcon", bitmapImage54);
			Add("IconColor", new ColorW(131, 201, 226, 255));
			Add("InventoryBackground", new SolidColorBrush(new ColorW(33, 44, 53, 255)));
			Add("ItemBackgroundHoverBrush", new SolidColorBrush(new ColorW(60, 76, 82, 255)));
			Add("ItemSelectedBrush", new SolidColorBrush(new ColorW(91, 115, 123, 255)));
			Add("ItemTextColor", new SolidColorBrush(new ColorW(198, 220, 228, 255)));
			Add("ItemTextHoverColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("ItemTextSelectedColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("LighterBackground", new SolidColorBrush(new ColorW(41, 54, 62, 255)));
			Style style5 = new Style(typeof(ListBox));
			Setter item12 = new Setter(UIElement.MinHeightProperty, 80f);
			style5.Setters.Add(item12);
			Func<UIElement, UIElement> createMethod7 = r_80_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod7);
			Setter item13 = new Setter(Control.TemplateProperty, value);
			style5.Setters.Add(item13);
			Trigger trigger14 = new Trigger();
			trigger14.Property = UIElement.IsFocusedProperty;
			trigger14.Value = true;
			Setter item14 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger14.Setters.Add(item14);
			style5.Triggers.Add(trigger14);
			Add("ListBoxGrid", style5);
			Style style6 = new Style(typeof(ListBox));
			Setter item15 = new Setter(UIElement.MinHeightProperty, 80f);
			style6.Setters.Add(item15);
			Trigger trigger15 = new Trigger();
			trigger15.Property = UIElement.IsFocusedProperty;
			trigger15.Value = true;
			Setter item16 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger15.Setters.Add(item16);
			style6.Triggers.Add(trigger15);
			Add("ListBoxStandard", style6);
			Style style7 = new Style(typeof(ListBox));
			Setter item17 = new Setter(UIElement.MinHeightProperty, 80f);
			style7.Setters.Add(item17);
			Func<UIElement, UIElement> createMethod8 = r_82_s_S_1_ctMethod;
			ControlTemplate value2 = new ControlTemplate(typeof(ListBox), createMethod8);
			Setter item18 = new Setter(Control.TemplateProperty, value2);
			style7.Setters.Add(item18);
			Trigger trigger16 = new Trigger();
			trigger16.Property = UIElement.IsFocusedProperty;
			trigger16.Value = true;
			Setter item19 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger16.Setters.Add(item19);
			style7.Triggers.Add(trigger16);
			Add("ListBoxWrapPanel", style7);
			Style style8 = new Style(typeof(ListBox));
			Setter item20 = new Setter(UIElement.MinHeightProperty, 80f);
			style8.Setters.Add(item20);
			Func<UIElement, UIElement> createMethod9 = r_83_s_S_1_ctMethod;
			ControlTemplate value3 = new ControlTemplate(typeof(ListBox), createMethod9);
			Setter item21 = new Setter(Control.TemplateProperty, value3);
			style8.Setters.Add(item21);
			Trigger trigger17 = new Trigger();
			trigger17.Property = UIElement.IsFocusedProperty;
			trigger17.Value = true;
			Setter item22 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger17.Setters.Add(item22);
			style8.Triggers.Add(trigger17);
			Add("ListBoxWrapPanelWorkshopBrowser", style8);
			BitmapImage bitmapImage55 = new BitmapImage();
			bitmapImage55.TextureAsset = "Textures\\GUI\\LoadingIconAnimated.png";
			Add("LoadingIconAnimated", bitmapImage55);
			ImageBrush imageBrush40 = new ImageBrush();
			BitmapImage bitmapImage56 = new BitmapImage();
			bitmapImage56.TextureAsset = "Textures\\GUI\\Screens\\message_background_red.dds";
			imageBrush40.ImageSource = bitmapImage56;
			imageBrush40.Stretch = Stretch.Fill;
			Add("MessageBackgroundBrush", imageBrush40);
			Style style9 = new Style(typeof(Window));
			Setter item23 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 0, 0, 255)));
			style9.Setters.Add(item23);
			Func<UIElement, UIElement> createMethod10 = r_86_s_S_1_ctMethod;
			ControlTemplate value4 = new ControlTemplate(typeof(Window), createMethod10);
			Setter item24 = new Setter(Control.TemplateProperty, value4);
			style9.Setters.Add(item24);
			Add("MessageBoxWindowStyle", style9);
			BitmapImage bitmapImage57 = new BitmapImage();
			bitmapImage57.TextureAsset = "Textures\\GUI\\Icons\\Rating\\NoStar.png";
			Add("NoStar", bitmapImage57);
			BitmapImage bitmapImage58 = new BitmapImage();
			bitmapImage58.TextureAsset = "Textures\\GUI\\Icons\\OxygenIcon.dds";
			Add("OxygenIcon", bitmapImage58);
			Add("ProgressBarBackgroundBrush", new SolidColorBrush(new ColorW(33, 44, 53, 255)));
			Add("ProgressBarForegroundColor", new SolidColorBrush(new ColorW(63, 71, 79, 255)));
			ImageBrush imageBrush41 = new ImageBrush();
			BitmapImage bitmapImage59 = new BitmapImage();
			bitmapImage59.TextureAsset = "Textures\\GUI\\Controls\\button_red.dds";
			imageBrush41.ImageSource = bitmapImage59;
			Add("RedButtonBackgroundBrush", imageBrush41);
			ImageBrush imageBrush42 = new ImageBrush();
			BitmapImage bitmapImage60 = new BitmapImage();
			bitmapImage60.TextureAsset = "Textures\\GUI\\Controls\\button_red_highlight.dds";
			imageBrush42.ImageSource = bitmapImage60;
			Add("RedButtonBackgroundBrushHighlight", imageBrush42);
			BitmapImage bitmapImage61 = new BitmapImage();
			bitmapImage61.TextureAsset = "Textures\\GUI\\Icons\\Blueprints\\Refresh.png";
			Add("RefreshIcon", bitmapImage61);
			BitmapImage bitmapImage62 = new BitmapImage();
			bitmapImage62.TextureAsset = "Textures\\GUI\\Icons\\Contracts\\RepairContractHeader.dds";
			Add("RepairContract", bitmapImage62);
			Style style10 = new Style(typeof(RepeatButton));
			Setter item25 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonDecrease"));
			style10.Setters.Add(item25);
			Func<UIElement, UIElement> createMethod11 = r_95_s_S_1_ctMethod;
			ControlTemplate controlTemplate7 = new ControlTemplate(typeof(RepeatButton), createMethod11);
			Trigger trigger18 = new Trigger();
			trigger18.Property = UIElement.IsMouseOverProperty;
			trigger18.Value = true;
			Setter item26 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonDecreaseHover"));
			trigger18.Setters.Add(item26);
			controlTemplate7.Triggers.Add(trigger18);
			Setter item27 = new Setter(Control.TemplateProperty, controlTemplate7);
			style10.Setters.Add(item27);
			Add("RepeatButtonDecrease", style10);
			Style style11 = new Style(typeof(RepeatButton));
			Setter item28 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonIncrease"));
			style11.Setters.Add(item28);
			Func<UIElement, UIElement> createMethod12 = r_96_s_S_1_ctMethod;
			ControlTemplate controlTemplate8 = new ControlTemplate(typeof(RepeatButton), createMethod12);
			Trigger trigger19 = new Trigger();
			trigger19.Property = UIElement.IsMouseOverProperty;
			trigger19.Value = true;
			Setter item29 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonIncreaseHover"));
			trigger19.Setters.Add(item29);
			controlTemplate8.Triggers.Add(trigger19);
			Setter item30 = new Setter(Control.TemplateProperty, controlTemplate8);
			style11.Setters.Add(item30);
			Add("RepeatButtonIncrease", style11);
			BitmapImage bitmapImage63 = new BitmapImage();
			bitmapImage63.TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png";
			Add("ReputationGainArrow", bitmapImage63);
			BitmapImage bitmapImage64 = new BitmapImage();
			bitmapImage64.TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png";
			Add("ReputationLossArrow", bitmapImage64);
			BitmapImage bitmapImage65 = new BitmapImage();
			bitmapImage65.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			Add("ScreenBackground", bitmapImage65);
			ImageBrush imageBrush43 = new ImageBrush();
			BitmapImage bitmapImage66 = new BitmapImage();
			bitmapImage66.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_center.dds";
			imageBrush43.ImageSource = bitmapImage66;
			imageBrush43.Stretch = Stretch.Fill;
			Add("ScrollableListCenter", imageBrush43);
			ImageBrush imageBrush44 = new ImageBrush();
			BitmapImage bitmapImage67 = new BitmapImage();
			bitmapImage67.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_center_bottom.dds";
			imageBrush44.ImageSource = bitmapImage67;
			imageBrush44.Stretch = Stretch.Fill;
			Add("ScrollableListCenterBottom", imageBrush44);
			ImageBrush imageBrush45 = new ImageBrush();
			BitmapImage bitmapImage68 = new BitmapImage();
			bitmapImage68.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_center_top.dds";
			imageBrush45.ImageSource = bitmapImage68;
			imageBrush45.Stretch = Stretch.Fill;
			Add("ScrollableListCenterTop", imageBrush45);
			ImageBrush imageBrush46 = new ImageBrush();
			BitmapImage bitmapImage69 = new BitmapImage();
			bitmapImage69.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_left_bottom.dds";
			imageBrush46.ImageSource = bitmapImage69;
			imageBrush46.Stretch = Stretch.Fill;
			Add("ScrollableListLeftBottom", imageBrush46);
			ImageBrush imageBrush47 = new ImageBrush();
			BitmapImage bitmapImage70 = new BitmapImage();
			bitmapImage70.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_left_center.dds";
			imageBrush47.ImageSource = bitmapImage70;
			imageBrush47.Stretch = Stretch.Fill;
			Add("ScrollableListLeftCenter", imageBrush47);
			ImageBrush imageBrush48 = new ImageBrush();
			BitmapImage bitmapImage71 = new BitmapImage();
			bitmapImage71.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_left_top.dds";
			imageBrush48.ImageSource = bitmapImage71;
			imageBrush48.Stretch = Stretch.Fill;
			Add("ScrollableListLeftTop", imageBrush48);
			ImageBrush imageBrush49 = new ImageBrush();
			BitmapImage bitmapImage72 = new BitmapImage();
			bitmapImage72.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_right_bottom.dds";
			imageBrush49.ImageSource = bitmapImage72;
			imageBrush49.Stretch = Stretch.Fill;
			Add("ScrollableListRightBottom", imageBrush49);
			ImageBrush imageBrush50 = new ImageBrush();
			BitmapImage bitmapImage73 = new BitmapImage();
			bitmapImage73.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_right_center.dds";
			imageBrush50.ImageSource = bitmapImage73;
			imageBrush50.Stretch = Stretch.Fill;
			Add("ScrollableListRightCenter", imageBrush50);
			ImageBrush imageBrush51 = new ImageBrush();
			BitmapImage bitmapImage74 = new BitmapImage();
			bitmapImage74.TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_right_top.dds";
			imageBrush51.ImageSource = bitmapImage74;
			imageBrush51.Stretch = Stretch.Fill;
			Add("ScrollableListRightTop", imageBrush51);
			ImageBrush imageBrush52 = new ImageBrush();
			BitmapImage bitmapImage75 = new BitmapImage();
			bitmapImage75.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_center.dds";
			imageBrush52.ImageSource = bitmapImage75;
			imageBrush52.Stretch = Stretch.Fill;
			Add("ScrollbarHorizontalThumbCenter", imageBrush52);
			ImageBrush imageBrush53 = new ImageBrush();
			BitmapImage bitmapImage76 = new BitmapImage();
			bitmapImage76.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_center_highlight.dds";
			imageBrush53.ImageSource = bitmapImage76;
			imageBrush53.Stretch = Stretch.Fill;
			Add("ScrollbarHorizontalThumbCenterHighlight", imageBrush53);
			ImageBrush imageBrush54 = new ImageBrush();
			BitmapImage bitmapImage77 = new BitmapImage();
			bitmapImage77.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_left.dds";
			imageBrush54.ImageSource = bitmapImage77;
			imageBrush54.Stretch = Stretch.Fill;
			Add("ScrollbarHorizontalThumbLeft", imageBrush54);
			ImageBrush imageBrush55 = new ImageBrush();
			BitmapImage bitmapImage78 = new BitmapImage();
			bitmapImage78.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_left_highlight.dds";
			imageBrush55.ImageSource = bitmapImage78;
			imageBrush55.Stretch = Stretch.Fill;
			Add("ScrollbarHorizontalThumbLeftHighlight", imageBrush55);
			ImageBrush imageBrush56 = new ImageBrush();
			BitmapImage bitmapImage79 = new BitmapImage();
			bitmapImage79.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_right.dds";
			imageBrush56.ImageSource = bitmapImage79;
			imageBrush56.Stretch = Stretch.Fill;
			Add("ScrollbarHorizontalThumbRight", imageBrush56);
			ImageBrush imageBrush57 = new ImageBrush();
			BitmapImage bitmapImage80 = new BitmapImage();
			bitmapImage80.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_right_highlight.dds";
			imageBrush57.ImageSource = bitmapImage80;
			imageBrush57.Stretch = Stretch.Fill;
			Add("ScrollbarHorizontalThumbRightHighlight", imageBrush57);
			Style style12 = new Style(typeof(RepeatButton));
			ControlTemplate value5 = new ControlTemplate(r_115_s_S_0_ctMethod);
			Setter item31 = new Setter(Control.TemplateProperty, value5);
			style12.Setters.Add(item31);
			Add("ScrollBarPageButton", style12);
			ImageBrush imageBrush58 = new ImageBrush();
			BitmapImage bitmapImage81 = new BitmapImage();
			bitmapImage81.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom.dds";
			imageBrush58.ImageSource = bitmapImage81;
			imageBrush58.Stretch = Stretch.Fill;
			Add("ScrollbarVerticalThumbBottom", imageBrush58);
			ImageBrush imageBrush59 = new ImageBrush();
			BitmapImage bitmapImage82 = new BitmapImage();
			bitmapImage82.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom_highlight.dds";
			imageBrush59.ImageSource = bitmapImage82;
			imageBrush59.Stretch = Stretch.Fill;
			Add("ScrollbarVerticalThumbBottomHighlight", imageBrush59);
			ImageBrush imageBrush60 = new ImageBrush();
			BitmapImage bitmapImage83 = new BitmapImage();
			bitmapImage83.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_center.dds";
			imageBrush60.ImageSource = bitmapImage83;
			imageBrush60.Stretch = Stretch.Fill;
			Add("ScrollbarVerticalThumbCenter", imageBrush60);
			ImageBrush imageBrush61 = new ImageBrush();
			BitmapImage bitmapImage84 = new BitmapImage();
			bitmapImage84.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_center_highlight.dds";
			imageBrush61.ImageSource = bitmapImage84;
			imageBrush61.Stretch = Stretch.Fill;
			Add("ScrollbarVerticalThumbCenterHighlight", imageBrush61);
			ImageBrush imageBrush62 = new ImageBrush();
			BitmapImage bitmapImage85 = new BitmapImage();
			bitmapImage85.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_top.dds";
			imageBrush62.ImageSource = bitmapImage85;
			imageBrush62.Stretch = Stretch.Fill;
			Add("ScrollbarVerticalThumbTop", imageBrush62);
			ImageBrush imageBrush63 = new ImageBrush();
			BitmapImage bitmapImage86 = new BitmapImage();
			bitmapImage86.TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_top_highlight.dds";
			imageBrush63.ImageSource = bitmapImage86;
			imageBrush63.Stretch = Stretch.Fill;
			Add("ScrollbarVerticalThumbTopHighlight", imageBrush63);
			Style style13 = new Style(typeof(ScrollViewer));
			Setter item32 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style13.Setters.Add(item32);
			Func<UIElement, UIElement> createMethod13 = r_122_s_S_1_ctMethod;
			ControlTemplate value6 = new ControlTemplate(typeof(ScrollViewer), createMethod13);
			Setter item33 = new Setter(Control.TemplateProperty, value6);
			style13.Setters.Add(item33);
			Add("ScrollViewerStyle", style13);
			Style style14 = new Style(typeof(ScrollViewer));
			Setter item34 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style14.Setters.Add(item34);
			ImageBrush imageBrush64 = new ImageBrush();
			BitmapImage bitmapImage87 = new BitmapImage();
			bitmapImage87.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush64.ImageSource = bitmapImage87;
			Setter item35 = new Setter(Control.BorderBrushProperty, imageBrush64);
			style14.Setters.Add(item35);
			Func<UIElement, UIElement> createMethod14 = r_123_s_S_2_ctMethod;
			ControlTemplate controlTemplate9 = new ControlTemplate(typeof(ScrollViewer), createMethod14);
			Trigger trigger20 = new Trigger();
			trigger20.Property = UIElement.IsFocusedProperty;
			trigger20.Value = true;
			ImageBrush imageBrush65 = new ImageBrush();
			BitmapImage bitmapImage88 = new BitmapImage();
			bitmapImage88.TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds";
			imageBrush65.ImageSource = bitmapImage88;
			Setter item36 = new Setter(Control.BorderBrushProperty, imageBrush65);
			trigger20.Setters.Add(item36);
			controlTemplate9.Triggers.Add(trigger20);
			Trigger trigger21 = new Trigger();
			trigger21.Property = UIElement.IsFocusedProperty;
			trigger21.Value = false;
			ImageBrush imageBrush66 = new ImageBrush();
			BitmapImage bitmapImage89 = new BitmapImage();
			bitmapImage89.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush66.ImageSource = bitmapImage89;
			Setter item37 = new Setter(Control.BorderBrushProperty, imageBrush66);
			trigger21.Setters.Add(item37);
			controlTemplate9.Triggers.Add(trigger21);
			Setter item38 = new Setter(Control.TemplateProperty, controlTemplate9);
			style14.Setters.Add(item38);
			Add("ScrollViewerStyleTextBlock", style14);
			BitmapImage bitmapImage90 = new BitmapImage();
			bitmapImage90.TextureAsset = "Textures\\GUI\\Icons\\Contracts\\SearchContractHeader.dds";
			Add("SearchContract", bitmapImage90);
			Style style15 = new Style(typeof(RepeatButton));
			Setter item39 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style15.Setters.Add(item39);
			Setter item40 = new Setter(UIElement.FocusableProperty, false);
			style15.Setters.Add(item40);
			Func<UIElement, UIElement> createMethod15 = r_125_s_S_2_ctMethod;
			ControlTemplate value7 = new ControlTemplate(typeof(RepeatButton), createMethod15);
			Setter item41 = new Setter(Control.TemplateProperty, value7);
			style15.Setters.Add(item41);
			Add("SliderButtonStyle", style15);
			ImageBrush imageBrush67 = new ImageBrush();
			BitmapImage bitmapImage91 = new BitmapImage();
			bitmapImage91.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_center.dds";
			imageBrush67.ImageSource = bitmapImage91;
			imageBrush67.Stretch = Stretch.Fill;
			Add("SliderRailCenter", imageBrush67);
			ImageBrush imageBrush68 = new ImageBrush();
			BitmapImage bitmapImage92 = new BitmapImage();
			bitmapImage92.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_center_focus.dds";
			imageBrush68.ImageSource = bitmapImage92;
			imageBrush68.Stretch = Stretch.Fill;
			Add("SliderRailCenterFocus", imageBrush68);
			ImageBrush imageBrush69 = new ImageBrush();
			BitmapImage bitmapImage93 = new BitmapImage();
			bitmapImage93.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_center_highlight.dds";
			imageBrush69.ImageSource = bitmapImage93;
			imageBrush69.Stretch = Stretch.Fill;
			Add("SliderRailCenterHighlight", imageBrush69);
			ImageBrush imageBrush70 = new ImageBrush();
			BitmapImage bitmapImage94 = new BitmapImage();
			bitmapImage94.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_left.dds";
			imageBrush70.ImageSource = bitmapImage94;
			imageBrush70.Stretch = Stretch.Fill;
			Add("SliderRailLeft", imageBrush70);
			ImageBrush imageBrush71 = new ImageBrush();
			BitmapImage bitmapImage95 = new BitmapImage();
			bitmapImage95.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_left_focus.dds";
			imageBrush71.ImageSource = bitmapImage95;
			imageBrush71.Stretch = Stretch.Fill;
			Add("SliderRailLeftFocus", imageBrush71);
			ImageBrush imageBrush72 = new ImageBrush();
			BitmapImage bitmapImage96 = new BitmapImage();
			bitmapImage96.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_left_highlight.dds";
			imageBrush72.ImageSource = bitmapImage96;
			imageBrush72.Stretch = Stretch.Fill;
			Add("SliderRailLeftHighlight", imageBrush72);
			ImageBrush imageBrush73 = new ImageBrush();
			BitmapImage bitmapImage97 = new BitmapImage();
			bitmapImage97.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_right.dds";
			imageBrush73.ImageSource = bitmapImage97;
			imageBrush73.Stretch = Stretch.Fill;
			Add("SliderRailRight", imageBrush73);
			ImageBrush imageBrush74 = new ImageBrush();
			BitmapImage bitmapImage98 = new BitmapImage();
			bitmapImage98.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_right_focus.dds";
			imageBrush74.ImageSource = bitmapImage98;
			imageBrush74.Stretch = Stretch.Fill;
			Add("SliderRailRightFocus", imageBrush74);
			ImageBrush imageBrush75 = new ImageBrush();
			BitmapImage bitmapImage99 = new BitmapImage();
			bitmapImage99.TextureAsset = "Textures\\GUI\\Controls\\slider_rail_right_highlight.dds";
			imageBrush75.ImageSource = bitmapImage99;
			imageBrush75.Stretch = Stretch.Fill;
			Add("SliderRailRightHighlight", imageBrush75);
			ImageBrush imageBrush76 = new ImageBrush();
			BitmapImage bitmapImage100 = new BitmapImage();
			bitmapImage100.TextureAsset = "Textures\\GUI\\Controls\\slider_thumb.dds";
			imageBrush76.ImageSource = bitmapImage100;
			imageBrush76.Stretch = Stretch.Fill;
			Add("SliderThumb", imageBrush76);
			ImageBrush imageBrush77 = new ImageBrush();
			BitmapImage bitmapImage101 = new BitmapImage();
			bitmapImage101.TextureAsset = "Textures\\GUI\\Controls\\slider_thumb_highlight.dds";
			imageBrush77.ImageSource = bitmapImage101;
			imageBrush77.Stretch = Stretch.Fill;
			Add("SliderThumbHighlight", imageBrush77);
			Style style16 = new Style(typeof(Thumb));
			Setter item42 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style16.Setters.Add(item42);
			Func<UIElement, UIElement> createMethod16 = r_137_s_S_1_ctMethod;
			ControlTemplate controlTemplate10 = new ControlTemplate(typeof(Thumb), createMethod16);
			Trigger trigger22 = new Trigger();
			trigger22.Property = UIElement.IsMouseOverProperty;
			trigger22.Value = true;
			Setter setter28 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("SliderThumbHighlight"));
			setter28.TargetName = "PART_SliderThumb";
			trigger22.Setters.Add(setter28);
			controlTemplate10.Triggers.Add(trigger22);
			Setter item43 = new Setter(Control.TemplateProperty, controlTemplate10);
			style16.Setters.Add(item43);
			Add("SliderThumbStyle", style16);
			Style style17 = new Style(typeof(Slider));
			Setter item44 = new Setter(UIElement.SnapsToDevicePixelsProperty, false);
			style17.Setters.Add(item44);
			Setter item45 = new Setter(Control.TemplateProperty, new ResourceReferenceExpression("HorizontalSliderHuePicker"));
			style17.Setters.Add(item45);
			Add("SliderWithHue", style17);
			Style style18 = new Style(typeof(Button));
			ImageBrush imageBrush78 = new ImageBrush();
			BitmapImage bitmapImage102 = new BitmapImage();
			bitmapImage102.TextureAsset = "Textures\\GUI\\Controls\\button_default.dds";
			imageBrush78.ImageSource = bitmapImage102;
			Setter item46 = new Setter(Control.BackgroundProperty, imageBrush78);
			style18.Setters.Add(item46);
			Setter item47 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(198, 220, 228, 255)));
			style18.Setters.Add(item47);
			Setter item48 = new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
			style18.Setters.Add(item48);
			Setter item49 = new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center);
			style18.Setters.Add(item49);
			Setter item50 = new Setter(UIElement.HeightProperty, 40f);
			style18.Setters.Add(item50);
			Func<UIElement, UIElement> createMethod17 = r_139_s_S_5_ctMethod;
			ControlTemplate controlTemplate11 = new ControlTemplate(typeof(Button), createMethod17);
			Trigger trigger23 = new Trigger();
			trigger23.Property = Button.IsPressedProperty;
			trigger23.Value = true;
			ImageBrush imageBrush79 = new ImageBrush();
			BitmapImage bitmapImage103 = new BitmapImage();
			bitmapImage103.TextureAsset = "Textures\\GUI\\Controls\\button_default_active.dds";
			imageBrush79.ImageSource = bitmapImage103;
			Setter item51 = new Setter(Control.BackgroundProperty, imageBrush79);
			trigger23.Setters.Add(item51);
			Setter item52 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			trigger23.Setters.Add(item52);
			controlTemplate11.Triggers.Add(trigger23);
			Trigger trigger24 = new Trigger();
			trigger24.Property = UIElement.IsFocusedProperty;
			trigger24.Value = true;
			ImageBrush imageBrush80 = new ImageBrush();
			BitmapImage bitmapImage104 = new BitmapImage();
			bitmapImage104.TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds";
			imageBrush80.ImageSource = bitmapImage104;
			Setter item53 = new Setter(Control.BackgroundProperty, imageBrush80);
			trigger24.Setters.Add(item53);
			Setter item54 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(33, 40, 45, 255)));
			trigger24.Setters.Add(item54);
			controlTemplate11.Triggers.Add(trigger24);
			MultiTrigger multiTrigger = new MultiTrigger();
			TriggerCondition triggerCondition = new TriggerCondition();
			triggerCondition.Property = UIElement.IsMouseOverProperty;
			triggerCondition.Value = true;
			multiTrigger.Conditions.Add(triggerCondition);
			TriggerCondition triggerCondition2 = new TriggerCondition();
			triggerCondition2.Property = UIElement.IsEnabledProperty;
			triggerCondition2.Value = true;
			multiTrigger.Conditions.Add(triggerCondition2);
			ImageBrush imageBrush81 = new ImageBrush();
			BitmapImage bitmapImage105 = new BitmapImage();
			bitmapImage105.TextureAsset = "Textures\\GUI\\Controls\\button_default_highlight.dds";
			imageBrush81.ImageSource = bitmapImage105;
			Setter item55 = new Setter(Control.BackgroundProperty, imageBrush81);
			multiTrigger.Setters.Add(item55);
			Setter item56 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			multiTrigger.Setters.Add(item56);
			controlTemplate11.Triggers.Add(multiTrigger);
			Trigger trigger25 = new Trigger();
			trigger25.Property = UIElement.IsEnabledProperty;
			trigger25.Value = false;
			Setter item57 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(29, 39, 45, 255)));
			trigger25.Setters.Add(item57);
			Setter item58 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(93, 105, 110, 255)));
			trigger25.Setters.Add(item58);
			controlTemplate11.Triggers.Add(trigger25);
			Setter item59 = new Setter(Control.TemplateProperty, controlTemplate11);
			style18.Setters.Add(item59);
			Add(typeof(Button), style18);
			Style style19 = new Style(typeof(CheckBox));
			Func<UIElement, UIElement> createMethod18 = r_140_s_S_0_ctMethod;
			ControlTemplate controlTemplate12 = new ControlTemplate(typeof(CheckBox), createMethod18);
			Trigger trigger26 = new Trigger();
			trigger26.Property = ToggleButton.IsCheckedProperty;
			trigger26.Value = true;
			Setter setter29 = new Setter(UIElement.VisibilityProperty, Visibility.Visible);
			setter29.TargetName = "PART_CheckMark";
			trigger26.Setters.Add(setter29);
			controlTemplate12.Triggers.Add(trigger26);
			Trigger trigger27 = new Trigger();
			trigger27.Property = ToggleButton.IsCheckedProperty;
			trigger27.Value = false;
			Setter setter30 = new Setter(UIElement.VisibilityProperty, Visibility.Collapsed);
			setter30.TargetName = "PART_CheckMark";
			trigger27.Setters.Add(setter30);
			controlTemplate12.Triggers.Add(trigger27);
			Trigger trigger28 = new Trigger();
			trigger28.Property = UIElement.IsMouseOverProperty;
			trigger28.Value = true;
			Setter setter31 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("CheckBoxHoverBackgroundBrush"));
			setter31.TargetName = "PART_NotChecked";
			trigger28.Setters.Add(setter31);
			controlTemplate12.Triggers.Add(trigger28);
			Trigger trigger29 = new Trigger();
			trigger29.Property = UIElement.IsFocusedProperty;
			trigger29.Value = true;
			ImageBrush imageBrush82 = new ImageBrush();
			BitmapImage bitmapImage106 = new BitmapImage();
			bitmapImage106.TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked_focus.dds";
			imageBrush82.ImageSource = bitmapImage106;
			Setter setter32 = new Setter(Control.BackgroundProperty, imageBrush82);
			setter32.TargetName = "PART_NotChecked";
			trigger29.Setters.Add(setter32);
			controlTemplate12.Triggers.Add(trigger29);
			Setter item60 = new Setter(Control.TemplateProperty, controlTemplate12);
			style19.Setters.Add(item60);
			Add(typeof(CheckBox), style19);
			Style style20 = new Style(typeof(ComboBox));
			Setter item61 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ItemTextColor"));
			style20.Setters.Add(item61);
			Setter item62 = new Setter(UIElement.HeightProperty, 38f);
			style20.Setters.Add(item62);
			Setter item63 = new Setter(ComboBox.MaxDropDownHeightProperty, 150f);
			style20.Setters.Add(item63);
			Setter item64 = new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center);
			style20.Setters.Add(item64);
			Func<UIElement, UIElement> createMethod19 = r_141_s_S_4_ctMethod;
			ControlTemplate controlTemplate13 = new ControlTemplate(typeof(ComboBox), createMethod19);
			Trigger trigger30 = new Trigger();
			trigger30.Property = UIElement.IsFocusedProperty;
			trigger30.Value = true;
			Setter setter33 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ComboBoxBackgroundLeftFocus"));
			setter33.TargetName = "PART_ComboBoxLeft";
			trigger30.Setters.Add(setter33);
			Setter setter34 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ComboBoxBackgroundCenterFocus"));
			setter34.TargetName = "PART_ComboBoxCenter";
			trigger30.Setters.Add(setter34);
			Setter setter35 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ComboBoxBackgroundRightFocus"));
			setter35.TargetName = "PART_ComboBoxRight";
			trigger30.Setters.Add(setter35);
			Setter setter36 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextFocusedColor"));
			setter36.TargetName = "PART_Button";
			trigger30.Setters.Add(setter36);
			controlTemplate13.Triggers.Add(trigger30);
			MultiTrigger multiTrigger2 = new MultiTrigger();
			TriggerCondition triggerCondition3 = new TriggerCondition();
			triggerCondition3.Property = UIElement.IsMouseOverProperty;
			triggerCondition3.Value = true;
			multiTrigger2.Conditions.Add(triggerCondition3);
			TriggerCondition triggerCondition4 = new TriggerCondition();
			triggerCondition4.Property = UIElement.IsEnabledProperty;
			triggerCondition4.Value = true;
			multiTrigger2.Conditions.Add(triggerCondition4);
			Setter setter37 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ComboBoxBackgroundLeftHighlight"));
			setter37.TargetName = "PART_ComboBoxLeft";
			multiTrigger2.Setters.Add(setter37);
			Setter setter38 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ComboBoxBackgroundCenterHighlight"));
			setter38.TargetName = "PART_ComboBoxCenter";
			multiTrigger2.Setters.Add(setter38);
			Setter setter39 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ComboBoxBackgroundRightHighlight"));
			setter39.TargetName = "PART_ComboBoxRight";
			multiTrigger2.Setters.Add(setter39);
			Setter setter40 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextHoverColor"));
			setter40.TargetName = "PART_Button";
			multiTrigger2.Setters.Add(setter40);
			controlTemplate13.Triggers.Add(multiTrigger2);
			Setter item65 = new Setter(Control.TemplateProperty, controlTemplate13);
			style20.Setters.Add(item65);
			Add(typeof(ComboBox), style20);
			Style style21 = new Style(typeof(ComboBoxItem));
			Func<UIElement, UIElement> createMethod20 = r_142_s_S_0_ctMethod;
			ControlTemplate controlTemplate14 = new ControlTemplate(typeof(ComboBoxItem), createMethod20);
			Trigger trigger31 = new Trigger();
			trigger31.Property = ComboBoxItem.IsHighlightedProperty;
			trigger31.Value = true;
			Setter item66 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			trigger31.Setters.Add(item66);
			Setter item67 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(60, 76, 82, 255)));
			trigger31.Setters.Add(item67);
			controlTemplate14.Triggers.Add(trigger31);
			Trigger trigger32 = new Trigger();
			trigger32.Property = UIElement.IsMouseOverProperty;
			trigger32.Value = true;
			Setter item68 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			trigger32.Setters.Add(item68);
			Setter item69 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(60, 76, 82, 255)));
			trigger32.Setters.Add(item69);
			controlTemplate14.Triggers.Add(trigger32);
			Trigger trigger33 = new Trigger();
			trigger33.Property = UIElement.IsFocusedProperty;
			trigger33.Value = true;
			Setter item70 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(33, 40, 45, 255)));
			trigger33.Setters.Add(item70);
			Setter item71 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(142, 188, 207, 255)));
			trigger33.Setters.Add(item71);
			controlTemplate14.Triggers.Add(trigger33);
			Trigger trigger34 = new Trigger();
			trigger34.Property = ListBoxItem.IsSelectedProperty;
			trigger34.Value = true;
			Setter item72 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			trigger34.Setters.Add(item72);
			Setter item73 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(91, 115, 123, 255)));
			trigger34.Setters.Add(item73);
			controlTemplate14.Triggers.Add(trigger34);
			Setter item74 = new Setter(Control.TemplateProperty, controlTemplate14);
			style21.Setters.Add(item74);
			Add(typeof(ComboBoxItem), style21);
			object obj = base[typeof(DataGrid)];
			Style style22 = new Style(typeof(DataGrid), obj as Style);
			Setter item75 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(41, 54, 62, 240)));
			style22.Setters.Add(item75);
			Trigger trigger35 = new Trigger();
			trigger35.Property = UIElement.IsFocusedProperty;
			trigger35.Value = true;
			Setter item76 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(60, 76, 82, 255)));
			trigger35.Setters.Add(item76);
			style22.Triggers.Add(trigger35);
			Add(typeof(DataGrid), style22);
			object obj2 = base[typeof(DataGridRow)];
			Style style23 = new Style(typeof(DataGridRow), obj2 as Style);
			Setter item77 = new Setter(Control.BorderThicknessProperty, new Thickness(0f));
			style23.Setters.Add(item77);
			Setter item78 = new Setter(Control.BorderBrushProperty, new SolidColorBrush(new ColorW(255, 255, 255, 0)));
			style23.Setters.Add(item78);
			Setter item79 = new Setter(Control.IsTabStopProperty, false);
			style23.Setters.Add(item79);
			Add(typeof(DataGridRow), style23);
			object obj3 = base[typeof(ListBox)];
			Style style24 = new Style(typeof(ListBox), obj3 as Style);
			Trigger trigger36 = new Trigger();
			trigger36.Property = UIElement.IsFocusedProperty;
			trigger36.Value = true;
			Setter item80 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 192)));
			trigger36.Setters.Add(item80);
			style24.Triggers.Add(trigger36);
			Add(typeof(ListBox), style24);
			object obj4 = base[typeof(ListBoxItem)];
			Style style25 = new Style(typeof(ListBoxItem), obj4 as Style);
			Setter item81 = new Setter(Control.IsTabStopProperty, false);
			style25.Setters.Add(item81);
			Add(typeof(ListBoxItem), style25);
			Style style26 = new Style(typeof(ScrollBar));
			Trigger trigger37 = new Trigger();
			trigger37.Property = ScrollBar.OrientationProperty;
			trigger37.Value = Orientation.Vertical;
			Func<UIElement, UIElement> createMethod21 = r_147_s_T_0_S_0_ctMethod;
			ControlTemplate value8 = new ControlTemplate(typeof(ScrollBar), createMethod21);
			Setter item82 = new Setter(Control.TemplateProperty, value8);
			trigger37.Setters.Add(item82);
			style26.Triggers.Add(trigger37);
			Trigger trigger38 = new Trigger();
			trigger38.Property = ScrollBar.OrientationProperty;
			trigger38.Value = Orientation.Horizontal;
			Func<UIElement, UIElement> createMethod22 = r_147_s_T_1_S_0_ctMethod;
			ControlTemplate value9 = new ControlTemplate(typeof(ScrollBar), createMethod22);
			Setter item83 = new Setter(Control.TemplateProperty, value9);
			trigger38.Setters.Add(item83);
			style26.Triggers.Add(trigger38);
			Add(typeof(ScrollBar), style26);
			Style style27 = new Style(typeof(Slider));
			Setter item84 = new Setter(UIElement.SnapsToDevicePixelsProperty, false);
			style27.Setters.Add(item84);
			Setter item85 = new Setter(Control.TemplateProperty, new ResourceReferenceExpression("HorizontalSlider"));
			style27.Setters.Add(item85);
			Add(typeof(Slider), style27);
			object obj5 = base[typeof(TabControl)];
			Style style28 = new Style(typeof(TabControl), obj5 as Style);
			Setter item86 = new Setter(Control.BorderThicknessProperty, new Thickness(0f));
			style28.Setters.Add(item86);
			Setter item87 = new Setter(Control.PaddingProperty, new Thickness(0f));
			style28.Setters.Add(item87);
			Setter item88 = new Setter(Control.BorderBrushProperty, new SolidColorBrush(new ColorW(255, 255, 255, 0)));
			style28.Setters.Add(item88);
			Add(typeof(TabControl), style28);
			Style style29 = new Style(typeof(TabItem));
			Setter item89 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonBackgroundBrush"));
			style29.Setters.Add(item89);
			Setter item90 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextColor"));
			style29.Setters.Add(item90);
			Func<UIElement, UIElement> createMethod23 = r_150_s_S_2_ctMethod;
			ControlTemplate controlTemplate15 = new ControlTemplate(typeof(TabItem), createMethod23);
			Trigger trigger39 = new Trigger();
			trigger39.Property = TabItem.IsSelectedProperty;
			trigger39.Value = true;
			Setter item91 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonBackgroundPressedBrush"));
			trigger39.Setters.Add(item91);
			Setter item92 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextPressedColor"));
			trigger39.Setters.Add(item92);
			controlTemplate15.Triggers.Add(trigger39);
			Trigger trigger40 = new Trigger();
			trigger40.Property = UIElement.IsFocusedProperty;
			trigger40.Value = true;
			Setter item93 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonBackgroundFocusedBrush"));
			trigger40.Setters.Add(item93);
			Setter item94 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextFocusedColor"));
			trigger40.Setters.Add(item94);
			controlTemplate15.Triggers.Add(trigger40);
			Trigger trigger41 = new Trigger();
			trigger41.Property = UIElement.IsMouseOverProperty;
			trigger41.Value = true;
			Setter item95 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ButtonBackgroundHoverBrush"));
			trigger41.Setters.Add(item95);
			Setter item96 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextHoverColor"));
			trigger41.Setters.Add(item96);
			controlTemplate15.Triggers.Add(trigger41);
			Setter item97 = new Setter(Control.TemplateProperty, controlTemplate15);
			style29.Setters.Add(item97);
			Add(typeof(TabItem), style29);
			Style style30 = new Style(typeof(TextBox));
			Setter item98 = new Setter(TextBoxBase.CaretBrushProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			style30.Setters.Add(item98);
			Setter item99 = new Setter(TextBoxBase.SelectionBrushProperty, new SolidColorBrush(new ColorW(63, 71, 79, 255)));
			style30.Setters.Add(item99);
			Func<UIElement, UIElement> createMethod24 = r_151_s_S_2_ctMethod;
			ControlTemplate controlTemplate16 = new ControlTemplate(typeof(TextBox), createMethod24);
			Trigger trigger42 = new Trigger();
			trigger42.Property = UIElement.IsMouseOverProperty;
			trigger42.Value = true;
			Setter setter41 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxLeftHighlight"));
			setter41.TargetName = "PART_TextBoxLeft";
			trigger42.Setters.Add(setter41);
			Setter setter42 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxCenterHighlight"));
			setter42.TargetName = "PART_TextBoxCenter";
			trigger42.Setters.Add(setter42);
			Setter setter43 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxRightHighlight"));
			setter43.TargetName = "PART_TextBoxRight";
			trigger42.Setters.Add(setter43);
			Setter item100 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			trigger42.Setters.Add(item100);
			controlTemplate16.Triggers.Add(trigger42);
			Trigger trigger43 = new Trigger();
			trigger43.Property = UIElement.IsFocusedProperty;
			trigger43.Value = true;
			Setter setter44 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxLeftFocus"));
			setter44.TargetName = "PART_TextBoxLeft";
			trigger43.Setters.Add(setter44);
			Setter setter45 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxCenterFocus"));
			setter45.TargetName = "PART_TextBoxCenter";
			trigger43.Setters.Add(setter45);
			Setter setter46 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("TextBoxRightFocus"));
			setter46.TargetName = "PART_TextBoxRight";
			trigger43.Setters.Add(setter46);
			Setter item101 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(33, 40, 45, 255)));
			trigger43.Setters.Add(item101);
			controlTemplate16.Triggers.Add(trigger43);
			Setter item102 = new Setter(Control.TemplateProperty, controlTemplate16);
			style30.Setters.Add(item102);
			Add(typeof(TextBox), style30);
			object obj6 = base[typeof(ToolTip)];
			Style style31 = new Style(typeof(ToolTip), obj6 as Style);
			Setter item103 = new Setter(Control.PaddingProperty, new Thickness(0f));
			style31.Setters.Add(item103);
			Setter item104 = new Setter(Control.BorderThicknessProperty, new Thickness(0f));
			style31.Setters.Add(item104);
			Setter item105 = new Setter(UIElement.OpacityProperty, 1f);
			style31.Setters.Add(item105);
			Add(typeof(ToolTip), style31);
			Style style32 = new Style(typeof(Window));
			Func<UIElement, UIElement> createMethod25 = r_153_s_S_0_ctMethod;
			ControlTemplate value10 = new ControlTemplate(typeof(Window), createMethod25);
			Setter item106 = new Setter(Control.TemplateProperty, value10);
			style32.Setters.Add(item106);
			Add(typeof(Window), style32);
			ImageBrush imageBrush83 = new ImageBrush();
			BitmapImage bitmapImage107 = new BitmapImage();
			bitmapImage107.TextureAsset = "Textures\\GUI\\Controls\\textbox_center.dds";
			imageBrush83.ImageSource = bitmapImage107;
			Add("TextBoxCenter", imageBrush83);
			ImageBrush imageBrush84 = new ImageBrush();
			BitmapImage bitmapImage108 = new BitmapImage();
			bitmapImage108.TextureAsset = "Textures\\GUI\\Controls\\textbox_center_focus.dds";
			imageBrush84.ImageSource = bitmapImage108;
			Add("TextBoxCenterFocus", imageBrush84);
			ImageBrush imageBrush85 = new ImageBrush();
			BitmapImage bitmapImage109 = new BitmapImage();
			bitmapImage109.TextureAsset = "Textures\\GUI\\Controls\\textbox_center_highlight.dds";
			imageBrush85.ImageSource = bitmapImage109;
			Add("TextBoxCenterHighlight", imageBrush85);
			ImageBrush imageBrush86 = new ImageBrush();
			BitmapImage bitmapImage110 = new BitmapImage();
			bitmapImage110.TextureAsset = "Textures\\GUI\\Controls\\textbox_left.dds";
			imageBrush86.ImageSource = bitmapImage110;
			Add("TextBoxLeft", imageBrush86);
			ImageBrush imageBrush87 = new ImageBrush();
			BitmapImage bitmapImage111 = new BitmapImage();
			bitmapImage111.TextureAsset = "Textures\\GUI\\Controls\\textbox_left_focus.dds";
			imageBrush87.ImageSource = bitmapImage111;
			Add("TextBoxLeftFocus", imageBrush87);
			ImageBrush imageBrush88 = new ImageBrush();
			BitmapImage bitmapImage112 = new BitmapImage();
			bitmapImage112.TextureAsset = "Textures\\GUI\\Controls\\textbox_left_highlight.dds";
			imageBrush88.ImageSource = bitmapImage112;
			Add("TextBoxLeftHighlight", imageBrush88);
			ImageBrush imageBrush89 = new ImageBrush();
			BitmapImage bitmapImage113 = new BitmapImage();
			bitmapImage113.TextureAsset = "Textures\\GUI\\Controls\\textbox_right.dds";
			imageBrush89.ImageSource = bitmapImage113;
			Add("TextBoxRight", imageBrush89);
			ImageBrush imageBrush90 = new ImageBrush();
			BitmapImage bitmapImage114 = new BitmapImage();
			bitmapImage114.TextureAsset = "Textures\\GUI\\Controls\\textbox_right_focus.dds";
			imageBrush90.ImageSource = bitmapImage114;
			Add("TextBoxRightFocus", imageBrush90);
			ImageBrush imageBrush91 = new ImageBrush();
			BitmapImage bitmapImage115 = new BitmapImage();
			bitmapImage115.TextureAsset = "Textures\\GUI\\Controls\\textbox_right_highlight.dds";
			imageBrush91.ImageSource = bitmapImage115;
			Add("TextBoxRightHighlight", imageBrush91);
			Style style33 = new Style(typeof(ScrollViewer));
			Setter item107 = new Setter(UIElement.SnapsToDevicePixelsProperty, true);
			style33.Setters.Add(item107);
			Func<UIElement, UIElement> createMethod26 = r_163_s_S_1_ctMethod;
			ControlTemplate value11 = new ControlTemplate(typeof(ScrollViewer), createMethod26);
			Setter item108 = new Setter(Control.TemplateProperty, value11);
			style33.Setters.Add(item108);
			Add("TextBoxScrollViewer", style33);
			Add("TextColor", new SolidColorBrush(new ColorW(198, 220, 228, 255)));
			Add("TextColorDarkBlue", new SolidColorBrush(new ColorW(94, 115, 127, 255)));
			Add("TextColorHighlight", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
			Add("ThemedGuiLineColor", new SolidColorBrush(new ColorW(77, 99, 113, 255)));
			ImageBrush imageBrush92 = new ImageBrush();
			BitmapImage bitmapImage116 = new BitmapImage();
			bitmapImage116.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			imageBrush92.ImageSource = bitmapImage116;
			Add("ToolTipBackgroundBrush", imageBrush92);
			Style style34 = new Style(typeof(Thumb));
			Func<UIElement, UIElement> createMethod27 = r_169_s_S_0_ctMethod;
			ControlTemplate controlTemplate17 = new ControlTemplate(typeof(Thumb), createMethod27);
			Trigger trigger44 = new Trigger();
			trigger44.Property = UIElement.IsMouseOverProperty;
			trigger44.Value = true;
			Setter setter47 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("ScrollbarVerticalThumbCenterHighlight"));
			setter47.TargetName = "PART_Center";
			trigger44.Setters.Add(setter47);
			controlTemplate17.Triggers.Add(trigger44);
			Setter item109 = new Setter(Control.TemplateProperty, controlTemplate17);
			style34.Setters.Add(item109);
			Add("VerticalThumb", style34);
			ImageBrush imageBrush93 = new ImageBrush();
			BitmapImage bitmapImage117 = new BitmapImage();
			bitmapImage117.TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds";
			imageBrush93.ImageSource = bitmapImage117;
			imageBrush93.Stretch = Stretch.Fill;
			Add("WindowBackgroundBrush", imageBrush93);
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\AcquisitionContractHeader.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Bg16x9.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\BountyContractHeader.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_active.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_decrease.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_decrease_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_increase.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_increase_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_unchecked.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_unchecked_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_unchecked_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_checked.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\SteamCB.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\SteamCBFocused.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\ModioCB.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\Background.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundChecked.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundHighlighted.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_focus_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_highlight_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_focus_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_highlight_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_focus_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_highlight_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\EscortContractHeader.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Rating\\FullStar.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\grid_item.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\grid_item_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Rating\\HalfStar.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\HaulingContractHeader.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_center_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_left_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_right_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HydrogenIcon.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\LoadingIconAnimated.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\message_background_red.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Rating\\NoStar.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\OxygenIcon.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_red.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_red_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\RepairContractHeader.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepGain.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepLoss.png");
			ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_center_bottom.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_center_top.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_left_bottom.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_left_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_left_top.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_right_bottom.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_right_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_right_top.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_center_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_left_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_right_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_center_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_top.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_top_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\SearchContractHeader.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_center_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_center_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_left_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_left_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_right_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_right_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_thumb.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_thumb_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_center.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_center_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_center_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_left.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_left_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_left_highlight.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_right.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_right_focus.dds");
			ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_right_highlight.dds");
		}

		private static UIElement r_24_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_0",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_1";
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
			contentPresenter.Name = "e_2";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_25_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_3",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_4";
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
			contentPresenter.Name = "e_5";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_54_s_S_5_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_6"
			};
			Grid grid = (Grid)(obj.Child = new Grid());
			grid.Name = "e_7";
			grid.SnapsToDevicePixels = false;
			grid.UseLayoutRounding = true;
			ColumnDefinition item = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			grid.ColumnDefinitions.Add(item2);
			Grid grid2 = new Grid();
			grid.Children.Add(grid2);
			grid2.Name = "e_8";
			RowDefinition item3 = new RowDefinition
			{
				MinHeight = 36f
			};
			grid2.RowDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition
			{
				Width = new GridLength(4f, GridUnitType.Pixel)
			};
			grid2.ColumnDefinitions.Add(item4);
			ColumnDefinition item5 = new ColumnDefinition();
			grid2.ColumnDefinitions.Add(item5);
			ColumnDefinition item6 = new ColumnDefinition
			{
				Width = new GridLength(4f, GridUnitType.Pixel)
			};
			grid2.ColumnDefinitions.Add(item6);
			Border border = new Border();
			grid2.Children.Add(border);
			border.Name = "PART_TextBoxLeft";
			border.IsHitTestVisible = false;
			border.SnapsToDevicePixels = false;
			border.SetResourceReference(Control.BackgroundProperty, "TextBoxLeft");
			Border border2 = new Border();
			grid2.Children.Add(border2);
			border2.Name = "PART_TextBoxCenter";
			border2.IsHitTestVisible = false;
			border2.SnapsToDevicePixels = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "TextBoxCenter");
			Border border3 = new Border();
			grid2.Children.Add(border3);
			border3.Name = "PART_TextBoxRight";
			border3.IsHitTestVisible = false;
			border3.SnapsToDevicePixels = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "TextBoxRight");
			ScrollViewer scrollViewer = new ScrollViewer();
			grid2.Children.Add(scrollViewer);
			scrollViewer.Name = "PART_ScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
			Grid.SetColumn(scrollViewer, 1);
			TextBlock textBlock2 = (TextBlock)(scrollViewer.Content = new TextBlock());
			textBlock2.Name = "e_9";
			textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.TextAlignment = TextAlignment.Right;
			textBlock2.SetBinding(binding: new Binding("Padding")
			{
				Source = parent
			}, property: Control.PaddingProperty);
			textBlock2.SetBinding(binding: new Binding("Text")
			{
				Source = parent
			}, property: TextBlock.TextProperty);
			StackPanel stackPanel = new StackPanel();
			grid.Children.Add(stackPanel);
			stackPanel.Name = "e_10";
			stackPanel.Orientation = Orientation.Horizontal;
			Grid.SetColumn(stackPanel, 1);
			RepeatButton repeatButton = new RepeatButton();
			stackPanel.Children.Add(repeatButton);
			repeatButton.Name = "PART_DecreaseButton";
			repeatButton.Height = 32f;
			repeatButton.Width = 32f;
			repeatButton.Focusable = false;
			repeatButton.IsTabStop = false;
			repeatButton.ClickMode = ClickMode.Press;
			repeatButton.Delay = 100;
			repeatButton.Interval = 200;
			SoundManager.SetIsSoundEnabled(repeatButton, value: false);
			repeatButton.SetResourceReference(UIElement.StyleProperty, "RepeatButtonDecrease");
			RepeatButton repeatButton2 = new RepeatButton();
			stackPanel.Children.Add(repeatButton2);
			repeatButton2.Name = "PART_IncreaseButton";
			repeatButton2.Height = 32f;
			repeatButton2.Width = 32f;
			repeatButton2.Focusable = false;
			repeatButton2.IsTabStop = false;
			repeatButton2.ClickMode = ClickMode.Press;
			repeatButton2.Delay = 100;
			repeatButton2.Interval = 200;
			SoundManager.SetIsSoundEnabled(repeatButton2, value: false);
			repeatButton2.SetResourceReference(UIElement.StyleProperty, "RepeatButtonIncrease");
			return obj;
		}

		private static UIElement r_62_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_11",
				Height = 32f,
				SnapsToDevicePixels = false,
				UseLayoutRounding = true
			};
			ColumnDefinition item = new ColumnDefinition
			{
				Width = new GridLength(12f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition
			{
				Width = new GridLength(12f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item3);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "PART_SliderRailLeft";
			border.IsHitTestVisible = false;
			border.SetResourceReference(Control.BackgroundProperty, "SliderRailLeft");
			Border border2 = new Border();
			obj.Children.Add(border2);
			border2.Name = "PART_SliderRailCenter";
			border2.IsHitTestVisible = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "SliderRailCenter");
			Border border3 = new Border();
			obj.Children.Add(border3);
			border3.Name = "PART_SliderRailRight";
			border3.IsHitTestVisible = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "SliderRailRight");
			Track track = new Track();
			obj.Children.Add(track);
			track.Name = "PART_Track";
			track.Margin = new Thickness(6f, 0f, 6f, 0f);
			RepeatButton repeatButton = new RepeatButton();
			repeatButton.Name = "e_12";
			repeatButton.ClickMode = ClickMode.Press;
			repeatButton.SetResourceReference(UIElement.StyleProperty, "SliderButtonStyle");
			track.IncreaseRepeatButton = repeatButton;
			RepeatButton repeatButton2 = new RepeatButton();
			repeatButton2.Name = "e_13";
			repeatButton2.ClickMode = ClickMode.Press;
			repeatButton2.SetResourceReference(UIElement.StyleProperty, "SliderButtonStyle");
			track.DecreaseRepeatButton = repeatButton2;
			Thumb thumb = new Thumb();
			thumb.Name = "e_14";
			thumb.VerticalAlignment = VerticalAlignment.Stretch;
			thumb.VerticalContentAlignment = VerticalAlignment.Center;
			thumb.SetResourceReference(UIElement.StyleProperty, "SliderThumbStyle");
			track.Thumb = thumb;
			Grid.SetColumnSpan(track, 3);
			return obj;
		}

		private static UIElement r_63_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_15",
				Height = 32f,
				SnapsToDevicePixels = false,
				UseLayoutRounding = true
			};
			ColumnDefinition item = new ColumnDefinition
			{
				Width = new GridLength(12f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition
			{
				Width = new GridLength(12f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item3);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "PART_SliderRailLeft";
			border.IsHitTestVisible = false;
			border.SetResourceReference(Control.BackgroundProperty, "HueColorGradientLeft");
			Border border2 = new Border();
			obj.Children.Add(border2);
			border2.Name = "PART_SliderRailCenter";
			border2.IsHitTestVisible = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "HueColorGradientCenter");
			Border border3 = new Border();
			obj.Children.Add(border3);
			border3.Name = "PART_SliderRailRight";
			border3.IsHitTestVisible = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "HueColorGradientRight");
			Track track = new Track();
			obj.Children.Add(track);
			track.Name = "PART_Track";
			track.Margin = new Thickness(6f, 0f, 6f, 0f);
			RepeatButton repeatButton = new RepeatButton();
			repeatButton.Name = "e_16";
			repeatButton.ClickMode = ClickMode.Press;
			repeatButton.SetResourceReference(UIElement.StyleProperty, "SliderButtonStyle");
			track.IncreaseRepeatButton = repeatButton;
			RepeatButton repeatButton2 = new RepeatButton();
			repeatButton2.Name = "e_17";
			repeatButton2.ClickMode = ClickMode.Press;
			repeatButton2.SetResourceReference(UIElement.StyleProperty, "SliderButtonStyle");
			track.DecreaseRepeatButton = repeatButton2;
			Thumb thumb = new Thumb();
			thumb.Name = "e_18";
			thumb.VerticalAlignment = VerticalAlignment.Stretch;
			thumb.VerticalContentAlignment = VerticalAlignment.Center;
			thumb.SetResourceReference(UIElement.StyleProperty, "SliderThumbStyle");
			track.Thumb = thumb;
			Grid.SetColumnSpan(track, 3);
			return obj;
		}

		private static UIElement r_64_s_S_0_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_19",
				Margin = new Thickness(2f, 0f, 2f, 0f)
			};
			ColumnDefinition item = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "PART_Center";
			Grid.SetColumn(border, 0);
			border.SetResourceReference(Control.BackgroundProperty, "ScrollbarHorizontalThumbCenter");
			return obj;
		}

		private static UIElement r_80_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_20",
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
			ScrollViewer scrollViewer = (ScrollViewer)(obj.Child = new ScrollViewer());
			scrollViewer.Name = "PART_DataScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			UniformGrid uniformGrid2 = (UniformGrid)(scrollViewer.Content = new UniformGrid());
			uniformGrid2.Name = "e_21";
			uniformGrid2.Margin = new Thickness(4f, 4f, 4f, 4f);
			uniformGrid2.VerticalAlignment = VerticalAlignment.Top;
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 5;
			return obj;
		}

		private static UIElement r_82_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_22",
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
			ScrollViewer scrollViewer = (ScrollViewer)(obj.Child = new ScrollViewer());
			scrollViewer.Name = "PART_DataScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			WrapPanel wrapPanel2 = (WrapPanel)(scrollViewer.Content = new WrapPanel());
			wrapPanel2.Name = "e_23";
			wrapPanel2.Margin = new Thickness(4f, 4f, 4f, 4f);
			wrapPanel2.IsItemsHost = true;
			wrapPanel2.ItemHeight = 64f;
			wrapPanel2.ItemWidth = 64f;
			return obj;
		}

		private static UIElement r_83_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_24",
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
			uniformGrid.Name = "e_25";
			uniformGrid.Margin = new Thickness(5f, 5f, 5f, 5f);
			uniformGrid.IsItemsHost = true;
			uniformGrid.Rows = 3;
			uniformGrid.Columns = 3;
			return obj;
		}

		private static void InitializeElemente_27Resources(UIElement elem)
		{
			object obj = elem.Resources[typeof(Button)];
			Style style = new Style(typeof(Button), obj as Style);
			Setter item = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("RedButtonBackgroundBrush"));
			style.Setters.Add(item);
			Setter item2 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextColor"));
			style.Setters.Add(item2);
			Setter item3 = new Setter(UIElement.WidthProperty, 200f);
			style.Setters.Add(item3);
			Func<UIElement, UIElement> createMethod = e_27r_0_s_S_3_ctMethod;
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Button), createMethod);
			Trigger trigger = new Trigger();
			trigger.Property = Button.IsPressedProperty;
			trigger.Value = true;
			Setter item4 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("RedButtonBackgroundBrushHighlight"));
			trigger.Setters.Add(item4);
			Setter item5 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 0, 0, 255)));
			trigger.Setters.Add(item5);
			controlTemplate.Triggers.Add(trigger);
			Trigger trigger2 = new Trigger();
			trigger2.Property = UIElement.IsFocusedProperty;
			trigger2.Value = true;
			Setter item6 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("RedButtonBackgroundBrushHighlight"));
			trigger2.Setters.Add(item6);
			Setter item7 = new Setter(Control.ForegroundProperty, new ResourceReferenceExpression("ButtonTextFocusedColor"));
			trigger2.Setters.Add(item7);
			controlTemplate.Triggers.Add(trigger2);
			Trigger trigger3 = new Trigger();
			trigger3.Property = UIElement.IsMouseOverProperty;
			trigger3.Value = true;
			Setter item8 = new Setter(Control.BackgroundProperty, new ResourceReferenceExpression("RedButtonBackgroundBrushHighlight"));
			trigger3.Setters.Add(item8);
			Setter item9 = new Setter(Control.ForegroundProperty, new SolidColorBrush(new ColorW(255, 0, 0, 255)));
			trigger3.Setters.Add(item9);
			controlTemplate.Triggers.Add(trigger3);
			Setter item10 = new Setter(Control.TemplateProperty, controlTemplate);
			style.Setters.Add(item10);
			elem.Resources.Add(typeof(Button), style);
		}

		private static UIElement e_27r_0_s_S_3_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_28",
				SnapsToDevicePixels = true,
				Padding = new Thickness(5f, 5f, 5f, 5f)
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_29";
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_86_s_S_1_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_26"
			};
			ColumnDefinition item = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item3);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_27";
			grid.SnapsToDevicePixels = true;
			RowDefinition item4 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item4);
			RowDefinition item5 = new RowDefinition();
			grid.RowDefinitions.Add(item5);
			Grid.SetColumn(grid, 1);
			grid.SetResourceReference(Control.BackgroundProperty, "MessageBackgroundBrush");
			InitializeElemente_27Resources(grid);
			ContentPresenter contentPresenter = new ContentPresenter();
			grid.Children.Add(contentPresenter);
			contentPresenter.Name = "PART_WindowTitle";
			contentPresenter.IsHitTestVisible = false;
			contentPresenter.Margin = new Thickness(10f, 10f, 10f, 0f);
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Title")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			ContentPresenter contentPresenter2 = new ContentPresenter();
			grid.Children.Add(contentPresenter2);
			contentPresenter2.Name = "e_30";
			contentPresenter2.Margin = new Thickness(10f, 10f, 10f, 10f);
			contentPresenter2.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(contentPresenter2, 1);
			contentPresenter2.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_95_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_31",
				Margin = new Thickness(1f, 1f, 1f, 1f)
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			return obj;
		}

		private static UIElement r_96_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_32",
				Margin = new Thickness(1f, 1f, 1f, 1f)
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			return obj;
		}

		private static UIElement r_115_s_S_0_ctMethod(UIElement parent)
		{
			return new Border
			{
				Parent = parent,
				Name = "e_33"
			};
		}

		private static UIElement r_122_s_S_1_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_34"
			};
			RowDefinition item = new RowDefinition();
			obj.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			obj.RowDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition
			{
				Width = new GridLength(1f, GridUnitType.Auto)
			};
			obj.ColumnDefinitions.Add(item4);
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_35";
			RowDefinition item5 = new RowDefinition
			{
				Height = new GridLength(4f, GridUnitType.Pixel)
			};
			grid.RowDefinitions.Add(item5);
			RowDefinition item6 = new RowDefinition();
			grid.RowDefinitions.Add(item6);
			RowDefinition item7 = new RowDefinition
			{
				Height = new GridLength(4f, GridUnitType.Pixel)
			};
			grid.RowDefinitions.Add(item7);
			ColumnDefinition item8 = new ColumnDefinition
			{
				Width = new GridLength(4f, GridUnitType.Pixel)
			};
			grid.ColumnDefinitions.Add(item8);
			ColumnDefinition item9 = new ColumnDefinition();
			grid.ColumnDefinitions.Add(item9);
			ColumnDefinition item10 = new ColumnDefinition
			{
				Width = new GridLength(32f, GridUnitType.Pixel)
			};
			grid.ColumnDefinitions.Add(item10);
			Grid.SetColumnSpan(grid, 2);
			Grid.SetRowSpan(grid, 2);
			Border border = new Border();
			grid.Children.Add(border);
			border.Name = "e_36";
			border.IsHitTestVisible = false;
			border.SnapsToDevicePixels = false;
			border.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftTop");
			Border border2 = new Border();
			grid.Children.Add(border2);
			border2.Name = "e_37";
			border2.IsHitTestVisible = false;
			border2.SnapsToDevicePixels = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterTop");
			Border border3 = new Border();
			grid.Children.Add(border3);
			border3.Name = "e_38";
			border3.IsHitTestVisible = false;
			border3.SnapsToDevicePixels = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightTop");
			Border border4 = new Border();
			grid.Children.Add(border4);
			border4.Name = "e_39";
			border4.IsHitTestVisible = false;
			border4.SnapsToDevicePixels = false;
			Grid.SetRow(border4, 1);
			border4.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftCenter");
			Border border5 = new Border();
			grid.Children.Add(border5);
			border5.Name = "e_40";
			border5.IsHitTestVisible = false;
			border5.SnapsToDevicePixels = false;
			Grid.SetColumn(border5, 1);
			Grid.SetRow(border5, 1);
			border5.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenter");
			Border border6 = new Border();
			grid.Children.Add(border6);
			border6.Name = "e_41";
			border6.IsHitTestVisible = false;
			border6.SnapsToDevicePixels = false;
			Grid.SetColumn(border6, 2);
			Grid.SetRow(border6, 1);
			border6.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightCenter");
			Border border7 = new Border();
			grid.Children.Add(border7);
			border7.Name = "e_42";
			border7.IsHitTestVisible = false;
			border7.SnapsToDevicePixels = false;
			Grid.SetRow(border7, 2);
			border7.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftBottom");
			Border border8 = new Border();
			grid.Children.Add(border8);
			border8.Name = "e_43";
			border8.IsHitTestVisible = false;
			border8.SnapsToDevicePixels = false;
			Grid.SetColumn(border8, 1);
			Grid.SetRow(border8, 2);
			border8.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterBottom");
			Border border9 = new Border();
			grid.Children.Add(border9);
			border9.Name = "e_44";
			border9.IsHitTestVisible = false;
			border9.SnapsToDevicePixels = false;
			Grid.SetColumn(border9, 2);
			Grid.SetRow(border9, 2);
			border9.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightBottom");
			ScrollContentPresenter scrollContentPresenter = new ScrollContentPresenter();
			obj.Children.Add(scrollContentPresenter);
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
			obj.Children.Add(scrollBar);
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
			obj.Children.Add(scrollBar2);
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

		private static UIElement r_123_s_S_2_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_45",
				BorderThickness = new Thickness(2f, 2f, 2f, 2f)
			};
			obj.SetBinding(binding: new Binding("BorderBrush")
			{
				Source = parent
			}, property: Control.BorderBrushProperty);
			Grid grid = (Grid)(obj.Child = new Grid());
			grid.Name = "e_46";
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
			grid2.Name = "e_47";
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
			border.Name = "e_48";
			border.IsHitTestVisible = false;
			border.SnapsToDevicePixels = false;
			border.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftTop");
			Border border2 = new Border();
			grid2.Children.Add(border2);
			border2.Name = "e_49";
			border2.IsHitTestVisible = false;
			border2.SnapsToDevicePixels = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterTop");
			Border border3 = new Border();
			grid2.Children.Add(border3);
			border3.Name = "e_50";
			border3.IsHitTestVisible = false;
			border3.SnapsToDevicePixels = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightTop");
			Border border4 = new Border();
			grid2.Children.Add(border4);
			border4.Name = "e_51";
			border4.IsHitTestVisible = false;
			border4.SnapsToDevicePixels = false;
			Grid.SetRow(border4, 1);
			border4.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftCenter");
			Border border5 = new Border();
			grid2.Children.Add(border5);
			border5.Name = "e_52";
			border5.IsHitTestVisible = false;
			border5.SnapsToDevicePixels = false;
			Grid.SetColumn(border5, 1);
			Grid.SetRow(border5, 1);
			border5.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenter");
			Border border6 = new Border();
			grid2.Children.Add(border6);
			border6.Name = "e_53";
			border6.IsHitTestVisible = false;
			border6.SnapsToDevicePixels = false;
			Grid.SetColumn(border6, 2);
			Grid.SetRow(border6, 1);
			border6.SetResourceReference(Control.BackgroundProperty, "ScrollableListRightCenter");
			Border border7 = new Border();
			grid2.Children.Add(border7);
			border7.Name = "e_54";
			border7.IsHitTestVisible = false;
			border7.SnapsToDevicePixels = false;
			Grid.SetRow(border7, 2);
			border7.SetResourceReference(Control.BackgroundProperty, "ScrollableListLeftBottom");
			Border border8 = new Border();
			grid2.Children.Add(border8);
			border8.Name = "e_55";
			border8.IsHitTestVisible = false;
			border8.SnapsToDevicePixels = false;
			Grid.SetColumn(border8, 1);
			Grid.SetRow(border8, 2);
			border8.SetResourceReference(Control.BackgroundProperty, "ScrollableListCenterBottom");
			Border border9 = new Border();
			grid2.Children.Add(border9);
			border9.Name = "e_56";
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

		private static UIElement r_125_s_S_2_ctMethod(UIElement parent)
		{
			return new Border
			{
				Parent = parent,
				Name = "e_57",
				Background = new SolidColorBrush(new ColorW(255, 255, 255, 0))
			};
		}

		private static UIElement r_137_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "PART_SliderThumb",
				Height = 20f,
				Width = 20f,
				SnapsToDevicePixels = true
			};
			obj.SetBinding(binding: new Binding("HorizontalContentAlignment")
			{
				Source = parent
			}, property: UIElement.HorizontalAlignmentProperty);
			obj.SetBinding(binding: new Binding("VerticalContentAlignment")
			{
				Source = parent
			}, property: UIElement.VerticalAlignmentProperty);
			obj.SetResourceReference(Control.BackgroundProperty, "SliderThumb");
			return obj;
		}

		private static UIElement r_139_s_S_5_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_58",
				SnapsToDevicePixels = true
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_59";
			contentPresenter.SetBinding(binding: new Binding("Padding")
			{
				Source = parent
			}, property: UIElement.MarginProperty);
			contentPresenter.SetBinding(binding: new Binding("HorizontalContentAlignment")
			{
				Source = parent
			}, property: UIElement.HorizontalAlignmentProperty);
			contentPresenter.SetBinding(binding: new Binding("VerticalContentAlignment")
			{
				Source = parent
			}, property: UIElement.VerticalAlignmentProperty);
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_140_s_S_0_ctMethod(UIElement parent)
		{
			StackPanel obj = new StackPanel
			{
				Parent = parent,
				Name = "e_60",
				Orientation = Orientation.Horizontal
			};
			Grid grid = new Grid();
			obj.Children.Add(grid);
			grid.Name = "e_61";
			Border border = new Border();
			grid.Children.Add(border);
			border.Name = "PART_NotChecked";
			border.Height = 32f;
			border.Width = 32f;
			border.SetResourceReference(Control.BackgroundProperty, "CheckBoxBackgroundBrush");
			Border border2 = new Border();
			grid.Children.Add(border2);
			border2.Name = "PART_CheckMark";
			border2.Height = 32f;
			border2.Width = 32f;
			border2.Visibility = Visibility.Collapsed;
			border2.SetResourceReference(Control.BackgroundProperty, "CheckImageBrush");
			ContentPresenter contentPresenter = new ContentPresenter();
			obj.Children.Add(contentPresenter);
			contentPresenter.Name = "e_62";
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement PART_Button_s_S_0_ctMethod(UIElement parent)
		{
			ContentPresenter obj = new ContentPresenter
			{
				Parent = parent,
				Name = "e_64",
				VerticalAlignment = VerticalAlignment.Center
			};
			obj.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_141_s_S_4_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_63",
				SnapsToDevicePixels = false,
				UseLayoutRounding = true
			};
			RowDefinition item = new RowDefinition();
			obj.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			obj.RowDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition
			{
				Width = new GridLength(16f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item4);
			ColumnDefinition item5 = new ColumnDefinition
			{
				Width = new GridLength(38f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item5);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "PART_ComboBoxLeft";
			border.Height = 38f;
			border.IsHitTestVisible = false;
			border.SnapsToDevicePixels = false;
			border.SetResourceReference(Control.BackgroundProperty, "ComboBoxBackgroundLeft");
			Border border2 = new Border();
			obj.Children.Add(border2);
			border2.Name = "PART_ComboBoxCenter";
			border2.IsHitTestVisible = false;
			border2.SnapsToDevicePixels = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "ComboBoxBackgroundCenter");
			Border border3 = new Border();
			obj.Children.Add(border3);
			border3.Name = "PART_ComboBoxRight";
			border3.Height = 38f;
			border3.IsHitTestVisible = false;
			border3.SnapsToDevicePixels = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "ComboBoxBackgroundRight");
			ToggleButton toggleButton = new ToggleButton();
			obj.Children.Add(toggleButton);
			toggleButton.Name = "PART_Button";
			toggleButton.Focusable = false;
			Style style = new Style(typeof(ToggleButton));
			Setter item6 = new Setter(value: new ControlTemplate(createMethod: PART_Button_s_S_0_ctMethod, targetType: typeof(ToggleButton)), property: Control.TemplateProperty);
			style.Setters.Add(item6);
			toggleButton.Style = style;
			toggleButton.IsTabStop = false;
			toggleButton.ClickMode = ClickMode.Press;
			Grid.SetColumnSpan(toggleButton, 3);
			toggleButton.SetBinding(binding: new Binding("IsDropDownOpen")
			{
				Source = parent
			}, property: ToggleButton.IsCheckedProperty);
			ContentPresenter contentPresenter2 = (ContentPresenter)(toggleButton.Content = new ContentPresenter());
			contentPresenter2.Name = "e_65";
			contentPresenter2.IsHitTestVisible = false;
			contentPresenter2.Margin = new Thickness(4f, 0f, 40f, 0f);
			contentPresenter2.SetBinding(binding: new Binding("SelectionBoxItem")
			{
				Source = parent
			}, property: UIElement.DataContextProperty);
			contentPresenter2.SetBinding(binding: new Binding("VerticalContentAlignment")
			{
				Source = parent
			}, property: UIElement.VerticalAlignmentProperty);
			contentPresenter2.SetBinding(binding: new Binding("SelectionBoxItem")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			contentPresenter2.SetBinding(binding: new Binding("SelectionBoxItemTemplate")
			{
				Source = parent
			}, property: ContentPresenter.ContentTemplateProperty);
			Popup popup = new Popup();
			obj.Children.Add(popup);
			popup.Name = "PART_ComboBoxPopup";
			Grid.SetRow(popup, 1);
			Grid.SetColumnSpan(popup, 3);
			popup.SetBinding(binding: new Binding("MaxDropDownHeight")
			{
				Source = parent
			}, property: UIElement.MaxHeightProperty);
			popup.SetBinding(binding: new Binding("IsDropDownOpen")
			{
				Source = parent
			}, property: Popup.IsOpenProperty);
			ScrollViewer scrollViewer = (ScrollViewer)(popup.Child = new ScrollViewer());
			scrollViewer.Name = "PART_DataScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			scrollViewer.SetResourceReference(UIElement.StyleProperty, "ScrollViewerStyle");
			StackPanel stackPanel2 = (StackPanel)(scrollViewer.Content = new StackPanel());
			stackPanel2.Name = "e_66";
			stackPanel2.Margin = new Thickness(10f, 0f, 0f, 0f);
			stackPanel2.IsItemsHost = true;
			return obj;
		}

		private static UIElement r_142_s_S_0_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_67"
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_68";
			contentPresenter.SetBinding(binding: new Binding("HorizontalContentAlignment")
			{
				Source = parent
			}, property: UIElement.HorizontalAlignmentProperty);
			contentPresenter.SetBinding(binding: new Binding("VerticalContentAlignment")
			{
				Source = parent
			}, property: UIElement.VerticalAlignmentProperty);
			contentPresenter.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement e_71_s_S_0_ctMethod(UIElement parent)
		{
			return new Border
			{
				Parent = parent,
				Name = "e_72"
			};
		}

		private static UIElement r_147_s_T_0_S_0_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_69"
			};
			Track track = new Track();
			obj.Children.Add(track);
			track.Name = "PART_Track";
			track.IsDirectionReversed = true;
			RepeatButton repeatButton = new RepeatButton();
			repeatButton.Name = "e_70";
			repeatButton.IsTabStop = false;
			repeatButton.Command = ScrollBar.PageDownCommand;
			repeatButton.ClickMode = ClickMode.Press;
			repeatButton.SetResourceReference(UIElement.StyleProperty, "ScrollBarPageButton");
			track.IncreaseRepeatButton = repeatButton;
			RepeatButton repeatButton2 = new RepeatButton
			{
				Name = "e_71"
			};
			Style style = new Style(typeof(RepeatButton));
			Setter item = new Setter(value: new ControlTemplate(e_71_s_S_0_ctMethod), property: Control.TemplateProperty);
			style.Setters.Add(item);
			repeatButton2.Style = style;
			repeatButton2.IsTabStop = false;
			repeatButton2.Command = ScrollBar.PageUpCommand;
			repeatButton2.ClickMode = ClickMode.Press;
			track.DecreaseRepeatButton = repeatButton2;
			Thumb thumb = new Thumb();
			thumb.Name = "e_73";
			thumb.SetResourceReference(UIElement.StyleProperty, "VerticalThumb");
			track.Thumb = thumb;
			return obj;
		}

		private static UIElement e_76_s_S_0_ctMethod(UIElement parent)
		{
			return new Border
			{
				Parent = parent,
				Name = "e_77"
			};
		}

		private static UIElement r_147_s_T_1_S_0_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_74"
			};
			Track track = new Track();
			obj.Children.Add(track);
			track.Name = "PART_Track";
			track.IsDirectionReversed = false;
			RepeatButton repeatButton = new RepeatButton();
			repeatButton.Name = "e_75";
			repeatButton.IsTabStop = false;
			repeatButton.Command = ScrollBar.PageRightCommand;
			repeatButton.ClickMode = ClickMode.Press;
			repeatButton.SetResourceReference(UIElement.StyleProperty, "ScrollBarPageButton");
			track.IncreaseRepeatButton = repeatButton;
			RepeatButton repeatButton2 = new RepeatButton
			{
				Name = "e_76"
			};
			Style style = new Style(typeof(RepeatButton));
			Setter item = new Setter(value: new ControlTemplate(e_76_s_S_0_ctMethod), property: Control.TemplateProperty);
			style.Setters.Add(item);
			repeatButton2.Style = style;
			repeatButton2.IsTabStop = false;
			repeatButton2.Command = ScrollBar.PageLeftCommand;
			repeatButton2.ClickMode = ClickMode.Press;
			track.DecreaseRepeatButton = repeatButton2;
			Thumb thumb = new Thumb();
			thumb.Name = "e_78";
			thumb.SetResourceReference(UIElement.StyleProperty, "HorizontalThumb");
			track.Thumb = thumb;
			return obj;
		}

		private static UIElement r_150_s_S_2_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_79",
				SnapsToDevicePixels = true,
				Padding = new Thickness(5f, 5f, 5f, 5f)
			};
			obj.SetBinding(binding: new Binding("Background")
			{
				Source = parent
			}, property: Control.BackgroundProperty);
			ContentPresenter contentPresenter = (ContentPresenter)(obj.Child = new ContentPresenter());
			contentPresenter.Name = "e_80";
			contentPresenter.Margin = new Thickness(10f, 2f, 10f, 2f);
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Header")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_151_s_S_2_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_81",
				SnapsToDevicePixels = false,
				UseLayoutRounding = true
			};
			RowDefinition item = new RowDefinition
			{
				MinHeight = 36f
			};
			obj.RowDefinitions.Add(item);
			ColumnDefinition item2 = new ColumnDefinition
			{
				Width = new GridLength(4f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item2);
			ColumnDefinition item3 = new ColumnDefinition();
			obj.ColumnDefinitions.Add(item3);
			ColumnDefinition item4 = new ColumnDefinition
			{
				Width = new GridLength(4f, GridUnitType.Pixel)
			};
			obj.ColumnDefinitions.Add(item4);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "PART_TextBoxLeft";
			border.IsHitTestVisible = false;
			border.SetResourceReference(Control.BackgroundProperty, "TextBoxLeft");
			Border border2 = new Border();
			obj.Children.Add(border2);
			border2.Name = "PART_TextBoxCenter";
			border2.IsHitTestVisible = false;
			Grid.SetColumn(border2, 1);
			border2.SetResourceReference(Control.BackgroundProperty, "TextBoxCenter");
			Border border3 = new Border();
			obj.Children.Add(border3);
			border3.Name = "PART_TextBoxRight";
			border3.IsHitTestVisible = false;
			Grid.SetColumn(border3, 2);
			border3.SetResourceReference(Control.BackgroundProperty, "TextBoxRight");
			ScrollViewer scrollViewer = new ScrollViewer();
			obj.Children.Add(scrollViewer);
			scrollViewer.Name = "PART_ScrollViewer";
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(scrollViewer, 1);
			scrollViewer.SetResourceReference(UIElement.StyleProperty, "TextBoxScrollViewer");
			TextBlock textBlock2 = (TextBlock)(scrollViewer.Content = new TextBlock());
			textBlock2.Name = "e_82";
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			textBlock2.SetBinding(binding: new Binding("HorizontalContentAlignment")
			{
				Source = parent
			}, property: UIElement.HorizontalAlignmentProperty);
			textBlock2.SetBinding(binding: new Binding("Padding")
			{
				Source = parent
			}, property: Control.PaddingProperty);
			textBlock2.SetBinding(binding: new Binding("TextAlignment")
			{
				Source = parent
			}, property: TextBlock.TextAlignmentProperty);
			textBlock2.SetBinding(binding: new Binding("Text")
			{
				Source = parent
			}, property: TextBlock.TextProperty);
			return obj;
		}

		private static UIElement r_153_s_S_0_ctMethod(UIElement parent)
		{
			Grid grid = new Grid();
			grid.Parent = parent;
			grid.Name = "e_83";
			RowDefinition item = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item);
			RowDefinition item2 = new RowDefinition();
			grid.RowDefinitions.Add(item2);
			RowDefinition item3 = new RowDefinition
			{
				Height = new GridLength(1f, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(item3);
			grid.SetResourceReference(Control.BackgroundProperty, "WindowBackgroundBrush");
			Border border = new Border();
			grid.Children.Add(border);
			border.Name = "PART_WindowTitleBorder";
			ContentPresenter contentPresenter = (ContentPresenter)(border.Child = new ContentPresenter());
			contentPresenter.Name = "PART_WindowTitle";
			contentPresenter.IsHitTestVisible = false;
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.SetBinding(binding: new Binding("Title")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			ScrollViewer scrollViewer = new ScrollViewer();
			grid.Children.Add(scrollViewer);
			scrollViewer.Name = "e_84";
			scrollViewer.Margin = new Thickness(20f, 20f, 20f, 20f);
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
			scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
			scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
			Grid.SetRow(scrollViewer, 1);
			ContentPresenter contentPresenter3 = (ContentPresenter)(scrollViewer.Content = new ContentPresenter());
			contentPresenter3.Name = "e_85";
			contentPresenter3.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			Border border2 = new Border();
			grid.Children.Add(border2);
			border2.Name = "PART_WindowResizeBorder";
			border2.Height = 16f;
			border2.Width = 16f;
			border2.HorizontalAlignment = HorizontalAlignment.Right;
			border2.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
			Grid.SetRow(border2, 2);
			return grid;
		}

		private static UIElement r_163_s_S_1_ctMethod(UIElement parent)
		{
			ScrollContentPresenter obj = new ScrollContentPresenter
			{
				Parent = parent,
				Name = "PART_ScrollContentPresenter"
			};
			obj.SetBinding(binding: new Binding("Padding")
			{
				Source = parent
			}, property: UIElement.MarginProperty);
			obj.SetBinding(binding: new Binding("HorizontalContentAlignment")
			{
				Source = parent
			}, property: UIElement.HorizontalAlignmentProperty);
			obj.SetBinding(binding: new Binding("VerticalContentAlignment")
			{
				Source = parent
			}, property: UIElement.VerticalAlignmentProperty);
			obj.SetBinding(binding: new Binding("Content")
			{
				Source = parent
			}, property: ContentPresenter.ContentProperty);
			return obj;
		}

		private static UIElement r_169_s_S_0_ctMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_86",
				Margin = new Thickness(0f, 2f, 0f, 2f)
			};
			RowDefinition item = new RowDefinition();
			obj.RowDefinitions.Add(item);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "PART_Center";
			Grid.SetRow(border, 0);
			border.SetResourceReference(Control.BackgroundProperty, "ScrollbarVerticalThumbCenter");
			return obj;
		}
	}
}
