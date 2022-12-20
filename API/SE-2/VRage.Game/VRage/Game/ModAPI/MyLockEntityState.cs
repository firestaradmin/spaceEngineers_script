using VRageMath;

namespace VRage.Game.ModAPI
{
	public struct MyLockEntityState
	{
		public MatrixD LocalMatrix;

		public Vector3D LocalVector;

		public double LocalDistance;

		public Vector3D LastKnownPosition;

		public long LockEntityID;

		public string LockEntityDisplayName;
	}
}
