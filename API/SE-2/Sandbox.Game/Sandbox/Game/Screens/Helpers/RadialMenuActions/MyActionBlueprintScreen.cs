using Sandbox.Game.GUI;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionBlueprintScreen : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyBlueprintUtils.OpenBlueprintScreen();
		}
	}
}
