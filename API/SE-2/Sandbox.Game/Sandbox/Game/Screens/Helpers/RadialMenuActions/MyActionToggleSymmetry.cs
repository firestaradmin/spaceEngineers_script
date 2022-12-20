using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionToggleSymmetry : MyActionBase
	{
		public override void ExecuteAction()
		{
			if (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode)
			{
				MyCubeBuilder.Static?.ToggleSymmetry();
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
			if (MyCubeBuilder.Static.UseSymmetry)
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Symmetry_On);
			}
			else
			{
				label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Symmetry_Off);
			}
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
			{
				label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
			}
			return label;
		}
	}
}
