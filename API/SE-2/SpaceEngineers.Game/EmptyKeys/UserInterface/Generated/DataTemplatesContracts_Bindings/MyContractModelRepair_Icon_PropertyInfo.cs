using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Game.Screens.Models;

namespace EmptyKeys.UserInterface.Generated.DataTemplatesContracts_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyContractModelRepair_Icon_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(BitmapImage);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyContractModelRepair)obj).Icon;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyContractModelRepair)obj).Icon = (BitmapImage)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
