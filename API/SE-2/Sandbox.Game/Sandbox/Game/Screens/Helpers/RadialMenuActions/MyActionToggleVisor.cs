using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleVisor : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchHelmet();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.ControlledEntity.EnabledHelmet)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Visor_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Visor_Off);
			}
			return label;
		}

		public override int GetIconIndex()
		{
			if (MySession.Static.ControlledEntity.EnabledHelmet)
			{
				return 1;
			}
			return 0;
		}
	}
}
