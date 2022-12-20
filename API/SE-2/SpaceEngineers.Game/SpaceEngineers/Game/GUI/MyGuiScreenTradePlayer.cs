using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Input;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	/// <summary>
	/// Implements player trading screen
	/// </summary>
	public class MyGuiScreenTradePlayer : MyGuiScreenMvvmBase
	{
		private MyPlayerTradeViewModel m_parentModel;

		public MyGuiScreenTradePlayer(MyPlayerTradeViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer OPEN");
			m_parentModel = viewModel;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTradePlayer";
		}

		public override UIRoot CreateView()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return new PlayerTradeView_Gamepad(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			}
			return new PlayerTradeView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen(bool isUnloading = false)
<<<<<<< HEAD
		{
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer CLOSE");
			return base.CloseScreen(isUnloading);
		}

		public override void CloseScreenNow(bool isUnloading = false)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			m_parentModel.OnScreenClosing();
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer CLOSE");
<<<<<<< HEAD
			base.CloseScreenNow(isUnloading);
=======
			return base.CloseScreen(isUnloading);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
