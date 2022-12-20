using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyWorkshopBrowserViewModel_WorkshopItems_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyWorkshopItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyWorkshopBrowserViewModel)obj).WorkshopItems;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyWorkshopBrowserViewModel)obj).WorkshopItems[(int)index[0]];
=======
			return ((Collection<MyWorkshopItemModel>)(object)((MyWorkshopBrowserViewModel)obj).WorkshopItems)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyWorkshopBrowserViewModel)obj).WorkshopItems = (ObservableCollection<MyWorkshopItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyWorkshopBrowserViewModel)obj).WorkshopItems[(int)index[0]] = (MyWorkshopItemModel)value;
=======
			((Collection<MyWorkshopItemModel>)(object)((MyWorkshopBrowserViewModel)obj).WorkshopItems)[(int)index[0]] = (MyWorkshopItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
