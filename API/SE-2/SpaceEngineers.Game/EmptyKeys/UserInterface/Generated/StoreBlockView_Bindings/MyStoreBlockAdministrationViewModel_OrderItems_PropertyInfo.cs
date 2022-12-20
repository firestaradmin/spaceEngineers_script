using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyStoreBlockAdministrationViewModel_OrderItems_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyOrderItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyStoreBlockAdministrationViewModel)obj).OrderItems;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyStoreBlockAdministrationViewModel)obj).OrderItems[(int)index[0]];
=======
			return ((Collection<MyOrderItemModel>)(object)((MyStoreBlockAdministrationViewModel)obj).OrderItems)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyStoreBlockAdministrationViewModel)obj).OrderItems = (ObservableCollection<MyOrderItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyStoreBlockAdministrationViewModel)obj).OrderItems[(int)index[0]] = (MyOrderItemModel)value;
=======
			((Collection<MyOrderItemModel>)(object)((MyStoreBlockAdministrationViewModel)obj).OrderItems)[(int)index[0]] = (MyOrderItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
