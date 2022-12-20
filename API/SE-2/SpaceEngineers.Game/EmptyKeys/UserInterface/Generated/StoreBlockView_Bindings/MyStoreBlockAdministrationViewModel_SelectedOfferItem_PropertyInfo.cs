using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyStoreBlockAdministrationViewModel_SelectedOfferItem_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(MyOrderItemModel);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyStoreBlockAdministrationViewModel)obj).SelectedOfferItem;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyStoreBlockAdministrationViewModel)obj).SelectedOfferItem = (MyOrderItemModel)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
