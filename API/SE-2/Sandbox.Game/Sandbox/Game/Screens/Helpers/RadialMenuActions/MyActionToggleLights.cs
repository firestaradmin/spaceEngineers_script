using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleLights : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchLights();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.ControlledEntity == null)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledLights_None);
			}
			else if (MySession.Static.ControlledEntity.EnabledLights)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledLights_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledLights_Off);
			}
			return label;
		}

		public override int GetIconIndex()
		{
			if (MySession.Static.ControlledEntity.EnabledLights)
			{
				return 1;
			}
			return 0;
		}
	}
}
