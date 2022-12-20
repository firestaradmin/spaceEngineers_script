using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyInventoryTargetViewModel_Inventories_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyInventoryTargetModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyInventoryTargetViewModel)obj).Inventories;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyInventoryTargetViewModel)obj).Inventories[(int)index[0]];
=======
			return ((Collection<MyInventoryTargetModel>)(object)((MyInventoryTargetViewModel)obj).Inventories)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyInventoryTargetViewModel)obj).Inventories = (ObservableCollection<MyInventoryTargetModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyInventoryTargetViewModel)obj).Inventories[(int)index[0]] = (MyInventoryTargetModel)value;
=======
			((Collection<MyInventoryTargetModel>)(object)((MyInventoryTargetViewModel)obj).Inventories)[(int)index[0]] = (MyInventoryTargetModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
