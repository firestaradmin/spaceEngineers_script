using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleDampeners : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchDamping();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.ControlledEntity == null)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledDampeners_None);
			}
			else if (MySession.Static.ControlledEntity.EnabledDamping)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledDampeners_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledDampeners_Off);
			}
			return label;
		}
	}
}
