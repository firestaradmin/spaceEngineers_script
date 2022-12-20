using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionAdminMenu : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
		}

		public override bool IsEnabled()
		{
			if (!MySession.Static.IsAdminMenuEnabled)
			{
				return false;
			}
			return true;
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (!MySession.Static.IsAdminMenuEnabled)
			{
				label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_AdminOnly);
			}
			return label;
		}
	}
}
