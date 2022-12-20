using System.Collections.ObjectModel;
using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyMainMenuViewModel : ViewModelBase
	{
		private ObservableCollection<MyTest> m_tests = new ObservableCollection<MyTest>();

		public ObservableCollection<MyTest> GridData
		{
			get
			{
				return m_tests;
			}
			set
			{
				SetProperty(ref m_tests, value, "GridData");
			}
		}

		public MyMainMenuViewModel()
		{
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 1"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 2"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 3"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 4"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 5"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 6"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 7"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 8"
			});
			((Collection<MyTest>)(object)m_tests).Add(new MyTest
			{
				Text = "Line 9"
			});
		}
	}
}
