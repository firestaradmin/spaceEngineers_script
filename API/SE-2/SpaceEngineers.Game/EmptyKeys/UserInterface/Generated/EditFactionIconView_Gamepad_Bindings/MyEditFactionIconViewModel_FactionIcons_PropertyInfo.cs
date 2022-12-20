using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;

namespace EmptyKeys.UserInterface.Generated.EditFactionIconView_Gamepad_Bindings
{
	[GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
	public class MyEditFactionIconViewModel_FactionIcons_PropertyInfo : IPropertyInfo
	{
		public Type PropertyType => typeof(ObservableCollection<MyFactionIconModel>);

		public bool IsResolved => true;

		public object GetValue(object obj)
		{
			return ((MyEditFactionIconViewModel)obj).FactionIcons;
		}

		public object GetValue(object obj, object[] index)
		{
<<<<<<< HEAD
			return ((MyEditFactionIconViewModel)obj).FactionIcons[(int)index[0]];
=======
			return ((Collection<MyFactionIconModel>)(object)((MyEditFactionIconViewModel)obj).FactionIcons)[(int)index[0]];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void SetValue(object obj, object value)
		{
			((MyEditFactionIconViewModel)obj).FactionIcons = (ObservableCollection<MyFactionIconModel>)value;
		}

		public void SetValue(object obj, object value, object[] index)
		{
<<<<<<< HEAD
			((MyEditFactionIconViewModel)obj).FactionIcons[(int)index[0]] = (MyFactionIconModel)value;
=======
			((Collection<MyFactionIconModel>)(object)((MyEditFactionIconViewModel)obj).FactionIcons)[(int)index[0]] = (MyFactionIconModel)value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
