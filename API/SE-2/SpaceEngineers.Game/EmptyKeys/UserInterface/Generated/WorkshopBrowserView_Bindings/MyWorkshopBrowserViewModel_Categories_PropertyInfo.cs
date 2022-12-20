using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyWorkshopBrowserViewModel_Categories_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(List<MyModCategoryModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyWorkshopBrowserViewModel)obj).Categories;
		}

		public object GetValue(object obj, object[] index)
		{
			return ((MyWorkshopBrowserViewModel)obj).Categories[(int)index[0]];
		}

		public void SetValue(object obj, object value)
		{
			((MyWorkshopBrowserViewModel)obj).Categories = (List<MyModCategoryModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
			((MyWorkshopBrowserViewModel)obj).Categories[(int)index[0]] = (MyModCategoryModel)value;
		}
	}
}
