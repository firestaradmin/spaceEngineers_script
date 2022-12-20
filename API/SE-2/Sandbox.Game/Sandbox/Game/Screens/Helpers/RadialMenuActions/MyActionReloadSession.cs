using Sandbox.Game.Gui;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionReloadSession : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyGuiScreenGamePlay.Static.RequestSessionReload();
		}
	}
}
