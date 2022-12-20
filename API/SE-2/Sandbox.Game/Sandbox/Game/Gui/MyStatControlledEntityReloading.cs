using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityReloading : MyStatBase
	{
		private int m_reloadInterval;

		private int m_reloadCompletionTime;

		public MyStatControlledEntityReloading()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_reloading");
		}

		public override void Update()
		{
			MyUserControllableGun myUserControllableGun = MySession.Static.ControlledEntity as MyUserControllableGun;
			if (myUserControllableGun == null)
			{
				m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_reloadInterval = 0;
			}
			else
			{
				m_reloadCompletionTime = myUserControllableGun.ReloadCompletionTime;
				m_reloadInterval = myUserControllableGun.ReloadTime;
			}
			int num = m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (num > 0)
			{
				base.CurrentValue = 1f - (float)num / (float)m_reloadInterval;
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
