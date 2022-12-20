using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenModIoConsent : MyGuiScreenMvvmBase
	{
		private int m_pauseInput;

		public MyGuiScreenModIoConsent(MyModIoConsentViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenModIoConsent OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenModIoConsent";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new ModIoConsentView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new ModIoConsentView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool Update(bool hasFocus)
		{
			bool pauseInput = MySandboxGame.Static.PauseInput;
			bool flag = MyInput.Static.IsKeyPress(MyKeys.Escape);
			if (pauseInput)
			{
				m_pauseInput = 10;
			}
			else if (m_pauseInput > 0 && !flag)
			{
				m_pauseInput--;
			}
			return base.Update(hasFocus);
		}

		protected override void Canceling()
		{
			if (m_pauseInput <= 0)
			{
				base.Canceling();
			}
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenModIoConsent CLOSE");
			return base.CloseScreen(isUnloading);
		}
	}
}
