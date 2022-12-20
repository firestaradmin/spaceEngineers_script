namespace VRage.Render11.Culling
{
	internal struct MyCullingSmallObjects
	{
		internal float ProjectionFactor;

		internal float SkipThreshold;

		internal float TSqr => SkipThreshold * SkipThreshold / (ProjectionFactor * ProjectionFactor);
	}
}
