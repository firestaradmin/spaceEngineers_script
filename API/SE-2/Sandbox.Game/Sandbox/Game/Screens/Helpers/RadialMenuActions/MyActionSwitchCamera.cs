using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionSwitchCamera : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyGuiScreenGamePlay.Static.SwitchCamera();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.CameraController == null)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_SwitchCamera_None);
			}
			else if (MySession.Static.CameraController.IsInFirstPersonView)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_SwitchCamera_FirstPerson);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_SwitchCamera_ThirdPerson);
			}
			return label;
		}
	}
}
