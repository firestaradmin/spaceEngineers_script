using System.Runtime.InteropServices;
using VRageMath;

namespace VRageRender
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MyBillboardData
	{
		internal Vector4 Color;

		internal int CustomProjectionID;

		internal float Reflective;

		internal float AlphaSaturation;

		internal float AlphaCutout;

		internal Vector3 Normal;

		internal float SoftParticleDistanceScale;
	}
}
