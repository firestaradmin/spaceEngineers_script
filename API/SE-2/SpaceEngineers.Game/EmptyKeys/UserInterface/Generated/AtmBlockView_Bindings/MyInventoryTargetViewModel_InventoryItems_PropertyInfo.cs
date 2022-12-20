using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.AtmBlockView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyInventoryTargetViewModel_InventoryItems_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyInventoryItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyInventoryTargetViewModel)obj).InventoryItems;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyInventoryTargetViewModel)obj).InventoryItems[(int)index[0]];
=======
			return ((Collection<MyInventoryItemModel>)(object)((MyInventoryTargetViewModel)obj).InventoryItems)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyInventoryTargetViewModel)obj).InventoryItems = (ObservableCollection<MyInventoryItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyInventoryTargetViewModel)obj).InventoryItems[(int)index[0]] = (MyInventoryItemModel)value;
=======
			((Collection<MyInventoryItemModel>)(object)((MyInventoryTargetViewModel)obj).InventoryItems)[(int)index[0]] = (MyInventoryItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
