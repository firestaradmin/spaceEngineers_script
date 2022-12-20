using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionViewMode : MyActionBase
	{
		public override void ExecuteAction()
		{
			if (IsEnabled())
			{
				if (MySession.Static.CameraController == MySpectator.Static)
				{
					MyGuiScreenGamePlay.SetSpectatorNone();
				}
				else
				{
					MyGuiScreenGamePlay.SetSpectatorFree();
				}
			}
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (MySession.Static.LocalHumanPlayer == null || !MySession.Static.HasPlayerSpectatorRights(MySession.Static.LocalHumanPlayer.Id.SteamId))
			{
				label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
			}
			if (MySession.Static.CameraController == MySpectator.Static)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleViewMode_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleViewMode_Off);
			}
			return label;
		}

		public override bool IsEnabled()
		{
			if (MySession.Static.LocalHumanPlayer != null && MySession.Static.HasPlayerSpectatorRights(MySession.Static.LocalHumanPlayer.Id.SteamId))
			{
				return true;
			}
			return false;
		}
	}
}
