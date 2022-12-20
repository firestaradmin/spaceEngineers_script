using VRageMath;

namespace Sandbox.Game.Weapons
{
	public class MyDrillCutOut
	{
		public readonly float CenterOffset;

		public readonly float Radius;

		protected BoundingSphereD m_sphere;

		public BoundingSphereD Sphere => m_sphere;

		public MyDrillCutOut(float centerOffset, float radius)
		{
			CenterOffset = centerOffset;
			Radius = radius;
			m_sphere = new BoundingSphereD(Vector3D.Zero, Radius);
		}

		public void UpdatePosition(ref MatrixD worldMatrix)
		{
			m_sphere.Center = worldMatrix.Translation + worldMatrix.Forward * CenterOffset;
		}
	}
}
