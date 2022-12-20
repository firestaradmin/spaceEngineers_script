using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionTogglePower : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchReactors();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.ControlledEntity == null)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnablePower_None);
			}
			else if (MySession.Static.ControlledEntity.EnabledReactors)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnablePower_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnablePower_Off);
			}
			return label;
		}
	}
}
