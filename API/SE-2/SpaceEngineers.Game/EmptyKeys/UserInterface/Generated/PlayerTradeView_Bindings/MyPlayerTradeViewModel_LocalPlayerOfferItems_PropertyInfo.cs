using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.PlayerTradeView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyPlayerTradeViewModel_LocalPlayerOfferItems_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyInventoryItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyPlayerTradeViewModel)obj).LocalPlayerOfferItems;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyPlayerTradeViewModel)obj).LocalPlayerOfferItems[(int)index[0]];
=======
			return ((Collection<MyInventoryItemModel>)(object)((MyPlayerTradeViewModel)obj).LocalPlayerOfferItems)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyPlayerTradeViewModel)obj).LocalPlayerOfferItems = (ObservableCollection<MyInventoryItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyPlayerTradeViewModel)obj).LocalPlayerOfferItems[(int)index[0]] = (MyInventoryItemModel)value;
=======
			((Collection<MyInventoryItemModel>)(object)((MyPlayerTradeViewModel)obj).LocalPlayerOfferItems)[(int)index[0]] = (MyInventoryItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
