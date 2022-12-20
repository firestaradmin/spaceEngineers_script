using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.PlayerTradeView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyPlayerTradeViewModel_SelectedOfferItem_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(MyInventoryItemModel);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyPlayerTradeViewModel)obj).SelectedOfferItem;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyPlayerTradeViewModel)obj).SelectedOfferItem = (MyInventoryItemModel)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
