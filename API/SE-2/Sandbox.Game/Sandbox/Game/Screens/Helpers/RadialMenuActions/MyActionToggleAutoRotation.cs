using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleAutoRotation : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyCubeBuilder.Static.AlignToDefault = !MyCubeBuilder.Static.AlignToDefault;
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MyCubeBuilder.Static.AlignToDefault)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleAutoRotation_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleAutoRotation_Off);
			}
			return label;
		}
	}
}
