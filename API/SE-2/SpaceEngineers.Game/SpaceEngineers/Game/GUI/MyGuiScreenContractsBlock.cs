using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenContractsBlock : MyGuiScreenMvvmBase
	{
		public MyGuiScreenContractsBlock(MyContractsBlockViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenContractsBlock OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenContractsBlock";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new ContractsBlockView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new ContractsBlockView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		protected override bool CanExit(object parameter)
		{
			return m_viewModel.CanExit(parameter);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenContractsBlock CLOSE");
			return base.CloseScreen(isUnloading);
		}
	}
}
