using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.ContractsBlockView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyContractsBlockViewModel_FilterTargets_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyContractTypeFilterItemModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyContractsBlockViewModel)obj).FilterTargets;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyContractsBlockViewModel)obj).FilterTargets[(int)index[0]];
=======
			return ((Collection<MyContractTypeFilterItemModel>)(object)((MyContractsBlockViewModel)obj).FilterTargets)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyContractsBlockViewModel)obj).FilterTargets = (ObservableCollection<MyContractTypeFilterItemModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyContractsBlockViewModel)obj).FilterTargets[(int)index[0]] = (MyContractTypeFilterItemModel)value;
=======
			((Collection<MyContractTypeFilterItemModel>)(object)((MyContractsBlockViewModel)obj).FilterTargets)[(int)index[0]] = (MyContractTypeFilterItemModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
