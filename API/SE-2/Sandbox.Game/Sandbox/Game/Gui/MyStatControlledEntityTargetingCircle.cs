using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityTargetingCircle : MyStatBase
	{
		private float m_maxValue = 100f;

		public override float MaxValue => m_maxValue;

		public MyStatControlledEntityTargetingCircle()
		{
			base.Id = MyStringHash.GetOrCompute("targeting_circle");
		}

		public override void Update()
		{
			MyTargetLockingComponent myTargetLockingComponent = MySession.Static.LocalCharacter?.TargetLockingComp;
			if (myTargetLockingComponent == null)
			{
				base.CurrentValue = 0f;
			}
			else
			{
				base.CurrentValue = myTargetLockingComponent.LockingProgressPercent;
			}
		}
	}
}
