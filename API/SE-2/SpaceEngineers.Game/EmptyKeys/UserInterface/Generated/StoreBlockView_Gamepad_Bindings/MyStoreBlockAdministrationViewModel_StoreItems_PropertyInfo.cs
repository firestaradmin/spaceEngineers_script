using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyStoreBlockAdministrationViewModel_StoreItems_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyStoreItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyStoreBlockAdministrationViewModel)obj).StoreItems;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyStoreBlockAdministrationViewModel)obj).StoreItems[(int)index[0]];
=======
			return ((Collection<MyStoreItemModel>)(object)((MyStoreBlockAdministrationViewModel)obj).StoreItems)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyStoreBlockAdministrationViewModel)obj).StoreItems = (ObservableCollection<MyStoreItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyStoreBlockAdministrationViewModel)obj).StoreItems[(int)index[0]] = (MyStoreItemModel)value;
=======
			((Collection<MyStoreItemModel>)(object)((MyStoreBlockAdministrationViewModel)obj).StoreItems)[(int)index[0]] = (MyStoreItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
