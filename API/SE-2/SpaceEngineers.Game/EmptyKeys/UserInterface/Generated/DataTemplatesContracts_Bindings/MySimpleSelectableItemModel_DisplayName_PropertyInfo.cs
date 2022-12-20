using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;

namespace EmptyKeys.UserInterface.Generated.DataTemplatesContracts_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MySimpleSelectableItemModel_DisplayName_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(string);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MySimpleSelectableItemModel)obj).DisplayName;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MySimpleSelectableItemModel)obj).DisplayName = (string)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
