using System;
using VRageMath;

namespace VRageRender
{
	internal static class MyVector3Helpers
	{
		internal static Vector3 Snap(this Vector3 vec, float res)
		{
			Vector3 vector = vec / res;
			return new Vector3((float)Math.Floor(vector.X), (float)Math.Floor(vector.Y), (float)Math.Floor(vector.Z)) * res;
		}

		internal static Vector3I AsCoord(this Vector3 vec, float res)
		{
			Vector3 vector = vec / res;
			return new Vector3I((int)Math.Floor(vector.X), (int)Math.Floor(vector.Y), (int)Math.Floor(vector.Z));
		}
	}
}
