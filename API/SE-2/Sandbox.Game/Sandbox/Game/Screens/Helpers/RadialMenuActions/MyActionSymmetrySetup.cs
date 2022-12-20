using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionSymmetrySetup : MyActionBase
	{
		public override void ExecuteAction()
		{
			if (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode)
			{
				MyCubeBuilder.Static?.ToggleSymmetrySetup();
			}
		}

		public override bool IsEnabled()
		{
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
			{
				return false;
			}
			return true;
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
			{
				label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
			}
<<<<<<< HEAD
			if (MyCubeBuilder.Static.IsSymmetrySetupMode())
			{
				label.Name = MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ExitSymmetrySetup);
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return label;
		}
	}
}
