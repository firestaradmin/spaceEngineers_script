using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyStoreBlockViewModel_IsSellEnabled_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(bool);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyStoreBlockViewModel)obj).IsSellEnabled;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyStoreBlockViewModel)obj).IsSellEnabled = (bool)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
