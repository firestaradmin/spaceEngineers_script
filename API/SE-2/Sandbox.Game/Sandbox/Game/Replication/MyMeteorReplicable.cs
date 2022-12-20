using Sandbox.Game.Entities;

namespace Sandbox.Game.Replication
{
	internal class MyMeteorReplicable : MyEntityReplicableBaseEvent<MyMeteor>
	{
		protected override void OnDestroyClientInternal()
		{
			if (base.Instance != null && base.Instance.Save)
			{
				base.Instance.Close();
			}
		}
	}
}
