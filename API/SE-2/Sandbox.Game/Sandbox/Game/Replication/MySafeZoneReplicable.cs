using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using VRage.Network;

namespace Sandbox.Game.Replication
{
	internal class MySafeZoneReplicable : MyEntityReplicableBaseEvent<MySafeZone>
	{
		protected override void OnDestroyClientInternal()
		{
			if (base.Instance != null && base.Instance.Save)
			{
				base.Instance.Close();
			}
		}

		protected override IMyStateGroup CreatePhysicsGroup()
		{
			return new MyEntityTransformStateGroup(this, base.Instance);
		}
	}
}
