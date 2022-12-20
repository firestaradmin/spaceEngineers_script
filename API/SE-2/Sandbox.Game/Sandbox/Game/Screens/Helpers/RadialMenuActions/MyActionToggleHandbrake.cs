using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleHandbrake : MyActionBase
	{
		public override void ExecuteAction()
		{
			MySession.Static.ControlledEntity?.SwitchHandbrake();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
			if (myShipController == null)
			{
				return label;
			}
			MyGridWheelSystem myGridWheelSystem = myShipController?.CubeGrid?.GridSystems?.WheelSystem;
			if (myGridWheelSystem == null)
			{
				return label;
			}
			if (myGridWheelSystem.HandBrake)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleHandbrake_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleHandbrake_Off);
			}
			return label;
		}
	}
}
