using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.EditFactionIconView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyEditFactionIconViewModel_IconColorInternal_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ColorW);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyEditFactionIconViewModel)obj).IconColorInternal;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyEditFactionIconViewModel)obj).IconColorInternal = (ColorW)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
