using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;

namespace EmptyKeys.UserInterface.Generated.DataTemplatesContracts_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyContractModelObtainAndDeliver_Conditions_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyContractConditionModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyContractModelObtainAndDeliver)obj).Conditions;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyContractModelObtainAndDeliver)obj).Conditions[(int)index[0]];
=======
			return ((Collection<MyContractConditionModel>)(object)((MyContractModelObtainAndDeliver)obj).Conditions)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyContractModelObtainAndDeliver)obj).Conditions = (ObservableCollection<MyContractConditionModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyContractModelObtainAndDeliver)obj).Conditions[(int)index[0]] = (MyContractConditionModel)value;
=======
			((Collection<MyContractConditionModel>)(object)((MyContractModelObtainAndDeliver)obj).Conditions)[(int)index[0]] = (MyContractConditionModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
