using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.Models
{
	public class MyModCategoryModel : BindableBase
	{
		private string m_id;

		private string m_localizedName;

		private bool m_isChecked;

		private ICommand m_toggleCategoryCommand;

		public string Id
		{
			get
			{
				return m_id;
			}
			set
			{
				SetProperty(ref m_id, value, "Id");
			}
		}

		public string LocalizedName
		{
			get
			{
				return m_localizedName;
			}
			set
			{
				SetProperty(ref m_localizedName, value, "LocalizedName");
			}
		}

		public bool IsChecked
		{
			get
			{
				return m_isChecked;
			}
			set
			{
				SetProperty(ref m_isChecked, value, "IsChecked");
			}
		}

		public ICommand ToggleCategoryCommand
		{
			get
			{
				return m_toggleCategoryCommand;
			}
			set
			{
				SetProperty(ref m_toggleCategoryCommand, value, "ToggleCategoryCommand");
			}
		}

		public MyModCategoryModel()
		{
			ToggleCategoryCommand = new RelayCommand(OnToggleCategory);
		}

		private void OnToggleCategory(object obj)
		{
			IsChecked = !IsChecked;
		}
	}
}
