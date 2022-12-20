using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	/// <summary>
	/// Implements screen for Store Block UI
	/// </summary>
	public class MyGuiScreenStoreBlock : MyGuiScreenMvvmBase
	{
		public MyGuiScreenStoreBlock(MyStoreBlockViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenStoreBlock OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenStoreBlock";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new StoreBlockView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new StoreBlockView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyLog.Default.WriteLine("MyGuiScreenStoreBlock CLOSE");
			return base.CloseScreen(isUnloading);
		}
	}
}
