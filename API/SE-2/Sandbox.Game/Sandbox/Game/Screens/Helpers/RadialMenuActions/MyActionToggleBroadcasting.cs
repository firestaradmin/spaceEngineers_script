using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleBroadcasting : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchBroadcasting();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.ControlledEntity == null)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnableBroadcasting_None);
			}
			else if (MySession.Static.ControlledEntity.EnabledBroadcasting)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnableBroadcasting_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnableBroadcasting_Off);
			}
			return label;
		}
	}
}
