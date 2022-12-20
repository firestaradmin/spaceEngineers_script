using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionShowHelpScreen : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
		}
	}
}
