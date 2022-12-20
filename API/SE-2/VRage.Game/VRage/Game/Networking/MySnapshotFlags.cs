namespace VRage.Game.Networking
{
	public class MySnapshotFlags
	{
		public bool ApplyPosition;

		public bool ApplyRotation;

		public bool ApplyPhysicsAngular;

		public bool ApplyPhysicsLinear;

		public bool ApplyPhysicsLocal;

		public bool InheritRotation = true;

		public void Init(MySnapshotFlags flags)
		{
			ApplyPosition = flags.ApplyPosition;
			ApplyRotation = flags.ApplyRotation;
			ApplyPhysicsAngular = flags.ApplyPhysicsAngular;
			ApplyPhysicsLinear = flags.ApplyPhysicsLinear;
			ApplyPhysicsLocal = flags.ApplyPhysicsLocal;
			InheritRotation = flags.InheritRotation;
		}

		public void Init(bool state)
		{
			ApplyPosition = state;
			ApplyRotation = state;
			ApplyPhysicsAngular = state;
			ApplyPhysicsLinear = state;
		}
	}
}
