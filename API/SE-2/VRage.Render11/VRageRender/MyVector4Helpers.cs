using VRageMath;

namespace VRageRender
{
	public static class MyVector4Helpers
	{
		public static Vector4 Create(Vector3 v, float x)
		{
			return new Vector4(v.X, v.Y, v.Z, x);
		}
	}
}
