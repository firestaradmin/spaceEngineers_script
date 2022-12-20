using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionColorPicker : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenColorPicker());
		}
	}
}
