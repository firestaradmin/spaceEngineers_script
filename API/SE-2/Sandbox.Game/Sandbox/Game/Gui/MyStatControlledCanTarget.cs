using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledCanTarget : MyStatBase
	{
		public MyStatControlledCanTarget()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_can_target");
		}

		public override void Update()
		{
			base.CurrentValue = 0f;
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			IMyTargetingCapableBlock myTargetingCapableBlock;
			if (controlledEntity == null)
			{
				base.CurrentValue = 0f;
			}
			else if (controlledEntity is IMyUserControllableGun || ((myTargetingCapableBlock = controlledEntity as IMyTargetingCapableBlock) != null && myTargetingCapableBlock.IsTargetLockingEnabled() && !myTargetingCapableBlock.IsShipToolSelected()))
			{
				base.CurrentValue = 1f;
			}
		}
	}
}
