using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyStoreBlockViewModel_OrderedItems_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyStoreItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyStoreBlockViewModel)obj).OrderedItems;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyStoreBlockViewModel)obj).OrderedItems[(int)index[0]];
=======
			return ((Collection<MyStoreItemModel>)(object)((MyStoreBlockViewModel)obj).OrderedItems)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyStoreBlockViewModel)obj).OrderedItems = (ObservableCollection<MyStoreItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyStoreBlockViewModel)obj).OrderedItems[(int)index[0]] = (MyStoreItemModel)value;
=======
			((Collection<MyStoreItemModel>)(object)((MyStoreBlockViewModel)obj).OrderedItems)[(int)index[0]] = (MyStoreItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
