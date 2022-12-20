using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatTargetLocked : MyStatBase
	{
		public MyStatTargetLocked()
		{
			base.Id = MyStringHash.GetOrCompute("target_locked");
		}

		public override void Update()
		{
			base.CurrentValue = 0f;
			MyTargetLockingComponent myTargetLockingComponent = MySession.Static.LocalCharacter?.TargetLockingComp;
			IMyTargetingCapableBlock myTargetingCapableBlock;
			bool flag = (myTargetingCapableBlock = MySession.Static.ControlledEntity as IMyTargetingCapableBlock) != null && myTargetingCapableBlock.IsShipToolSelected();
			if (myTargetLockingComponent != null && !flag)
			{
				base.CurrentValue += ((myTargetLockingComponent.Target != null) ? 1f : 0f);
				base.CurrentValue += (myTargetLockingComponent.IsTargetLocked ? 1f : 0f);
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
