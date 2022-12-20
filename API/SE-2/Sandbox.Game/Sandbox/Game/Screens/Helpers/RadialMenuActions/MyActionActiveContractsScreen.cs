using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionActiveContractsScreen : MyActionBase
	{
		public override void ExecuteAction()
		{
			if (MyGuiScreenGamePlay.ActiveGameplayScreen == null)
			{
				MyContractsActiveViewModel viewModel = new MyContractsActiveViewModel();
				ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
			}
		}
	}
}
