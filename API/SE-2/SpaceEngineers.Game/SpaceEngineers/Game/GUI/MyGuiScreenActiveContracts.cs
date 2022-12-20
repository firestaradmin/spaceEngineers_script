using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenActiveContracts : MyGuiScreenMvvmBase
	{
		public MyGuiScreenActiveContracts(MyContractsActiveViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenActiveContracts OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenActiveContracts";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new ActiveContractsView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new ActiveContractsView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenActiveContracts CLOSE");
			return base.CloseScreen(isUnloading);
		}
	}
}
