using System;
using VRageMath;

namespace VRageRender
{
	internal static class MyVector3HDelpers
	{
		internal static Vector3D Snap(this Vector3D vec, float res)
		{
			Vector3D vector3D = vec / res;
			return new Vector3D(Math.Floor(vector3D.X), Math.Floor(vector3D.Y), Math.Floor(vector3D.Z)) * res;
		}

		internal static Vector3I AsCoord(this Vector3D vec, float res)
		{
			Vector3D vector3D = vec / res;
			return new Vector3I((int)Math.Floor(vector3D.X), (int)Math.Floor(vector3D.Y), (int)Math.Floor(vector3D.Z));
		}
	}
}
