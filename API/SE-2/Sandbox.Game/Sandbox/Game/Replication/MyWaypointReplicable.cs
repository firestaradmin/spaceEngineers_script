using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyWaypointReplicable : MyEntityReplicableBaseEvent<MyWaypoint>
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

		public override BoundingBoxD GetAABB()
		{
			return new BoundingBoxD(new Vector3D(-999999.0), new Vector3D(999999.0));
		}
	}
}
