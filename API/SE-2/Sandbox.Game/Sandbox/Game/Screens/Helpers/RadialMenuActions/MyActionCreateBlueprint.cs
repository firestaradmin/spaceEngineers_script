using Sandbox.Game.SessionComponents.Clipboard;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionCreateBlueprint : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyClipboardComponent.Static.CreateBlueprint();
		}

		public override bool IsEnabled()
		{
			return true;
		}
	}
}
