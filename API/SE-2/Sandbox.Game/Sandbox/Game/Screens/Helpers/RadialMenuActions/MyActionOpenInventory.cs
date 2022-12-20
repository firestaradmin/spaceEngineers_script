using Sandbox.Game.World;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionOpenInventory : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.ShowInventory();
		}
	}
}
