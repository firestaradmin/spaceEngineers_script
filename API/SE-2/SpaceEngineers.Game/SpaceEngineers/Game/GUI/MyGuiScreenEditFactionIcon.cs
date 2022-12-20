using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenEditFactionIcon : MyGuiScreenMvvmBase
	{
		public MyGuiScreenEditFactionIcon(MyEditFactionIconViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenEditFactionIcon";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new EditFactionIconView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new EditFactionIconView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer END");
			return base.CloseScreen(isUnloading);
		}
	}
}
