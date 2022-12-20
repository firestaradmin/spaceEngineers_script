using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Graphics.GUI;
using VRage.Input;

namespace Sandbox.Game.Screens.ViewModels
{
	public abstract class MyViewModelBase : ViewModelBase
	{
		private ColorW m_backgroundOverlay;

		private ICommand m_exitCommand;

		private float m_maxWidth;

		private bool m_isJoystickLastUsed;

		private MyGuiScreenBase m_screenBase;

		public MyGuiScreenBase ScreenBase
		{
			get
			{
				return m_screenBase;
			}
			set
			{
				m_screenBase = value;
			}
		}

		public ColorW BackgroundOverlay
		{
			get
			{
				return m_backgroundOverlay;
			}
			set
			{
				SetProperty(ref m_backgroundOverlay, value, "BackgroundOverlay");
			}
		}

		public ICommand ExitCommand
		{
			get
			{
				return m_exitCommand;
			}
			set
			{
				SetProperty(ref m_exitCommand, value, "ExitCommand");
			}
		}

		public float MaxWidth
		{
			get
			{
				return m_maxWidth;
			}
			set
			{
				SetProperty(ref m_maxWidth, value, "MaxWidth");
			}
		}

		public bool IsJoystickLastUsed
		{
			get
			{
				return m_isJoystickLastUsed;
			}
			set
			{
				SetProperty(ref m_isJoystickLastUsed, value, "IsJoystickLastUsed");
			}
		}

		public MyViewModelBase(MyGuiScreenBase scrBase = null)
		{
			m_screenBase = scrBase;
			ExitCommand = new RelayCommand(OnExit);
			IsJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
		}

		public virtual void InitializeData()
		{
		}

		public virtual bool CanExit(object parameter)
		{
			return true;
		}

		public void OnExit(object obj)
		{
			if (m_screenBase != null)
			{
				m_screenBase.CloseScreenNow();
			}
			else
			{
				MyScreenManager.GetScreenWithFocus().CloseScreen();
			}
		}

		public virtual void OnScreenClosing()
		{
		}

		public virtual void OnScreenClosed()
		{
		}

		public virtual void Update()
		{
		}
	}
}
