using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionPasteGrid : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyClipboardComponent.Static.Paste();
		}

		public override bool IsEnabled()
		{
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
			{
				return false;
			}
			return true;
		}
	}
}
