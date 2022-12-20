using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.ContractsBlockView_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyContractsBlockViewModel_AvailableContracts_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyContractModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyContractsBlockViewModel)obj).AvailableContracts;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyContractsBlockViewModel)obj).AvailableContracts[(int)index[0]];
=======
			return ((Collection<MyContractModel>)(object)((MyContractsBlockViewModel)obj).AvailableContracts)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyContractsBlockViewModel)obj).AvailableContracts = (ObservableCollection<MyContractModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyContractsBlockViewModel)obj).AvailableContracts[(int)index[0]] = (MyContractModel)value;
=======
			((Collection<MyContractModel>)(object)((MyContractsBlockViewModel)obj).AvailableContracts)[(int)index[0]] = (MyContractModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
