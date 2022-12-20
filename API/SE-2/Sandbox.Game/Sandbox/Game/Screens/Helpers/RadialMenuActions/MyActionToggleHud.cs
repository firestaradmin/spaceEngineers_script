using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleHud : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyHud.ToggleGamepadHud();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			switch (MyHud.HudState)
			{
			case 0:
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Hud_Hidden);
				break;
			case 1:
			case 2:
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Hud_Visible);
				break;
			}
			return label;
		}
	}
}
