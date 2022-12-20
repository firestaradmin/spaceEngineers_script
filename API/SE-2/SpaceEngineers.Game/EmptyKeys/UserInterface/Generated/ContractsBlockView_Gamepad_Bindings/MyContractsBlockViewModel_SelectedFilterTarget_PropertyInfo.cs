using System;
using System.CodeDom.Compiler;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.ContractsBlockView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyContractsBlockViewModel_SelectedFilterTarget_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(MyContractTypeFilterItemModel);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyContractsBlockViewModel)obj).SelectedFilterTarget;
		}

		public object GetValue(object obj, object[] index)
		{
			return null;
		}

		public void SetValue(object obj, object value)
		{
			((MyContractsBlockViewModel)obj).SelectedFilterTarget = (MyContractTypeFilterItemModel)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
		}
	}
}
