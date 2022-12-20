using VRageMath;

namespace VRage.Render11.Culling.Occlusion
{
	internal class MyFlareOcclusionData
	{
		internal MyOcclusionQuery Query;

		internal float Size;

		internal float Shift;

		internal Vector3D Position;

		internal bool Visible;

		internal float FreqMinMs;

		internal float FreqRndMs;

		internal float LastVolumeSquared;

		internal float OcclusionFactor;

		internal float LastOcclusionFactor;

		internal float NextOcclusionFactor;

		internal float QueryTime;

		internal float LastQueryTime;
	}
}
