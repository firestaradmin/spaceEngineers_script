using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionShowProgressionTree : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, MySession.Static.ControlledEntity as MyShipController, "ResearchPage", true, null));
		}
	}
}
