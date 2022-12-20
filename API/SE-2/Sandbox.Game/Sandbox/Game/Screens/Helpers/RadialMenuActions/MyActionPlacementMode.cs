using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents.Clipboard;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionPlacementMode : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyClipboardComponent.Static.ChangeStationRotation();
			MyCubeBuilder.Static.CycleCubePlacementMode();
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MyCubeBuilder.Static.IsActivated)
			{
				switch (MyCubeBuilder.Static.CubePlacementMode)
				{
				case MyCubeBuilder.CubePlacementModeEnum.FreePlacement:
					label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_Grid_Free);
					break;
				case MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem:
					label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_Grid_Local);
					break;
				case MyCubeBuilder.CubePlacementModeEnum.GravityAligned:
					label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_Grid_Gravity);
					break;
				}
			}
			else if (MyClipboardComponent.Static.IsActive)
			{
				if (MyClipboardComponent.Static.IsStationRotationenabled())
				{
					label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_ClipboardRoattion_Enabled);
				}
				else
				{
					label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_ClipboardRoattion_Disabled);
				}
			}
			return label;
		}
	}
}
