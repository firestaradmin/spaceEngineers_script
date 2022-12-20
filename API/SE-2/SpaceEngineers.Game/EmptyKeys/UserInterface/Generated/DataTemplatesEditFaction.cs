using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesEditFaction_Bindings;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public sealed class DataTemplatesEditFaction : ResourceDictionary
	{
		private static DataTemplatesEditFaction singleton = new DataTemplatesEditFaction();

		public static DataTemplatesEditFaction Instance => singleton;

		public DataTemplatesEditFaction()
		{
			InitializeResources();
		}

		private void InitializeResources()
		{
			base.MergedDictionaries.Add(Styles.Instance);
			Func<UIElement, UIElement> createMethod = r_0_dtMethod;
			Add(typeof(MyFactionIconModel), new DataTemplate(typeof(MyFactionIconModel), createMethod));
			Add("EditFactionIconViewModelLocator", new MyEditFactionIconViewModelLocator());
			Style style = new Style(typeof(ListBox));
			Setter item = new Setter(UIElement.MinHeightProperty, 80f);
			style.Setters.Add(item);
			Func<UIElement, UIElement> createMethod2 = r_2_s_S_1_ctMethod;
			ControlTemplate value = new ControlTemplate(typeof(ListBox), createMethod2);
			Setter item2 = new Setter(Control.TemplateProperty, value);
			style.Setters.Add(item2);
			Trigger trigger = new Trigger();
			trigger.Property = UIElement.IsFocusedProperty;
			trigger.Value = true;
			Setter item3 = new Setter(Control.BackgroundProperty, new SolidColorBrush(new ColorW(67, 81, 92, 160)));
			trigger.Setters.Add(item3);
			style.Triggers.Add(trigger);
			Add("ListBoxFactionIconItem", style);
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyFactionIconModel), "IsEnabled", typeof(MyFactionIconModel_IsEnabled_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyFactionIconModel), "TooltipText", typeof(MyFactionIconModel_TooltipText_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyFactionIconModel), "Opacity", typeof(MyFactionIconModel_Opacity_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyFactionIconModel), "Icon", typeof(MyFactionIconModel_Icon_PropertyInfo));
			GeneratedPropertyInfo.RegisterGeneratedProperty(typeof(MyFactionIconModel), "IconColor", typeof(MyFactionIconModel_IconColor_PropertyInfo));
		}

		private static UIElement r_0_dtMethod(UIElement parent)
		{
			Grid obj = new Grid
			{
				Parent = parent,
				Name = "e_0",
				Height = 58f,
				Width = 58f
			};
			obj.SetBinding(binding: new Binding("IsEnabled")
			{
				UseGeneratedBindings = true
			}, property: UIElement.IsEnabledProperty);
			Border border = new Border();
			obj.Children.Add(border);
			border.Name = "e_1";
			border.Margin = new Thickness(2f, 2f, 2f, 2f);
			border.BorderBrush = new SolidColorBrush(new ColorW(77, 99, 113, 255));
			border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
			Image image = (Image)(border.Child = new Image());
			image.Name = "e_2";
			image.Stretch = Stretch.Fill;
			image.SetBinding(binding: new Binding("TooltipText")
			{
				UseGeneratedBindings = true
			}, property: UIElement.ToolTipProperty);
			image.SetBinding(binding: new Binding("Opacity")
			{
				UseGeneratedBindings = true
			}, property: UIElement.OpacityProperty);
			image.SetBinding(binding: new Binding("Icon")
			{
				UseGeneratedBindings = true
			}, property: Image.SourceProperty);
			image.SetBinding(binding: new Binding("IconColor")
			{
				UseGeneratedBindings = true
			}, property: ImageBrush.ColorOverlayProperty);
			return obj;
		}

		private static UIElement r_2_s_S_1_ctMethod(UIElement parent)
		{
			Border obj = new Border
			{
				Parent = parent,
				Name = "e_3",
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
			scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Center;
			scrollViewer.VerticalContentAlignment = VerticalAlignment.Top;
			UniformGrid uniformGrid2 = (UniformGrid)(scrollViewer.Content = new UniformGrid());
			uniformGrid2.Name = "e_4";
			uniformGrid2.Margin = new Thickness(6f, 6f, 6f, 6f);
			uniformGrid2.IsItemsHost = true;
			uniformGrid2.Columns = 6;
			return obj;
		}
	}
}
