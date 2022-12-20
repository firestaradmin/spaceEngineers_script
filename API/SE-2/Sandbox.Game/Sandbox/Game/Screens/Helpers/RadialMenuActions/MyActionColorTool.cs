using Sandbox.Game.Entities;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionColorTool : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyCubeBuilder.Static.ActivateColorTool();
		}
	}
}
