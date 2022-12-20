using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityOffscreenTargetCircle : MyStatBase
	{
		private float m_maxValue = 100f;

		public override float MaxValue => m_maxValue;

		public MyStatControlledEntityOffscreenTargetCircle()
		{
			base.Id = MyStringHash.GetOrCompute("offscreen_target_circle");
		}

		public override void Update()
		{
			MyTargetLockingComponent myTargetLockingComponent = MySession.Static.LocalCharacter?.TargetLockingComp;
			bool flag = myTargetLockingComponent != null && MySession.Static.ControlledEntity is IMyTargetingCapableBlock;
			base.CurrentValue = (flag ? myTargetLockingComponent.LockingProgressPercent : 0f);
		}
	}
}
