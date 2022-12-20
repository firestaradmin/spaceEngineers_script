using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenWorkshopBrowser : MyGuiScreenMvvmBase
	{
		public MyGuiScreenWorkshopBrowser(MyWorkshopBrowserViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenWorkshopBrowser OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWorkshopBrowser";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new WorkshopBrowserView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new WorkshopBrowserView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenWorkshopBrowser CLOSE");
			return base.CloseScreen(isUnloading);
		}
	}
}
