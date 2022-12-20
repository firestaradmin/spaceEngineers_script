namespace VRageMath
{
	/// <summary>
	/// Describes the intersection between a plane and a bounding volume.
	/// </summary>    
	public enum PlaneIntersectionType
	{
		/// <summary>
		/// There is no intersection, and the bounding volume is in the positive half-space of the Plane.
		/// </summary>
		Front,
		/// <summary>
		/// There is no intersection, and the bounding volume is in the negative half-space of the Plane.
		/// </summary>
		Back,
		/// <summary>
		/// The Plane is intersected.
		/// </summary>
		Intersecting
	}
}
