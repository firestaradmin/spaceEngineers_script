using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionShowPlayers : MyActionBase
	{
		public override void ExecuteAction()
		{
			if (Sync.MultiplayerActive)
			{
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
			}
		}

		public override bool IsEnabled()
		{
			if (!Sync.MultiplayerActive)
			{
				return false;
			}
			return true;
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (!Sync.MultiplayerActive)
			{
				label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_MultiplayerOnly);
			}
			return label;
		}
	}
}
