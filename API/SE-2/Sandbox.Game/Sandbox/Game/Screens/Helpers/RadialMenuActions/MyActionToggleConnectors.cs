using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleConnectors : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchLandingGears();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.ControlledEntity == null)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledConnectors_None);
			}
			else if (MySession.Static.ControlledEntity.EnabledLeadingGears)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledConnectors_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledConnectors_Off);
			}
			return label;
		}
	}
}
