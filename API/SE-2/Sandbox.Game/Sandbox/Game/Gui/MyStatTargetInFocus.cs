using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatTargetInFocus : MyStatBase
	{
		public MyStatTargetInFocus()
		{
			base.Id = MyStringHash.GetOrCompute("target_in_focus");
		}

		public override void Update()
		{
			MyTargetFocusComponent myTargetFocusComponent = MySession.Static.LocalCharacter?.TargetFocusComp;
			IMyTargetingCapableBlock myTargetingCapableBlock;
			bool flag = (myTargetingCapableBlock = MySession.Static.ControlledEntity as IMyTargetingCapableBlock) != null && myTargetingCapableBlock.IsShipToolSelected();
			base.CurrentValue = ((myTargetFocusComponent?.CurrentTarget != null && !flag) ? 1f : 0f);
		}
	}
}
