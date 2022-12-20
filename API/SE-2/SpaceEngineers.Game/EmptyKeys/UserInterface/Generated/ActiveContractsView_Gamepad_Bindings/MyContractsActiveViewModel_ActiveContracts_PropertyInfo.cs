using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.ActiveContractsView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyContractsActiveViewModel_ActiveContracts_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyContractModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyContractsActiveViewModel)obj).ActiveContracts;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyContractsActiveViewModel)obj).ActiveContracts[(int)index[0]];
=======
			return ((Collection<MyContractModel>)(object)((MyContractsActiveViewModel)obj).ActiveContracts)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyContractsActiveViewModel)obj).ActiveContracts = (ObservableCollection<MyContractModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyContractsActiveViewModel)obj).ActiveContracts[(int)index[0]] = (MyContractModel)value;
=======
			((Collection<MyContractModel>)(object)((MyContractsActiveViewModel)obj).ActiveContracts)[(int)index[0]] = (MyContractModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
