using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyWorkshopBrowserViewModel_SelectedWorkshopItem_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(MyWorkshopItemModel);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyWorkshopBrowserViewModel)obj).SelectedWorkshopItem;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyWorkshopBrowserViewModel)obj).SelectedWorkshopItem = (MyWorkshopItemModel)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
