using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	/// <summary>
	/// Implements screen for ATM Block UI
	/// </summary>
	public class MyGuiScreenAtmBlock : MyGuiScreenMvvmBase
	{
		public MyGuiScreenAtmBlock(MyStoreBlockViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenAtmBlock OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenAtmBlock";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new AtmBlockView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new AtmBlockView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenAtmBlock CLOSE");
			return base.CloseScreen(isUnloading);
		}
	}
}
