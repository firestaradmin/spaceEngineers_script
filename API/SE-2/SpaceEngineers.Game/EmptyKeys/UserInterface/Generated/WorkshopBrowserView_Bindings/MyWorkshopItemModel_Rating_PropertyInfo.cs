using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Game.Screens.Models;

namespace EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyWorkshopItemModel_Rating_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(List<BitmapImage>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyWorkshopItemModel)obj).Rating;
		}

		public object GetValue(object obj, object[] index)
		{
			return ((MyWorkshopItemModel)obj).Rating[(int)index[0]];
		}

		public void SetValue(object obj, object value)
		{
			((MyWorkshopItemModel)obj).Rating = (List<BitmapImage>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
			((MyWorkshopItemModel)obj).Rating[(int)index[0]] = (BitmapImage)value;
		}
	}
}
