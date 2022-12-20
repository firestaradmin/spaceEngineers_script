using System;

namespace VRage.Network
{
	/// <summary>
	/// Indicates that event will be called only on clients in specific distance
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class DistanceRadiusAttribute : Attribute
	{
		public readonly float DistanceRadius;

		/// <summary>
		/// Creates attribute that indicates that event will be called only on clients in specific distance
		/// </summary>
		public DistanceRadiusAttribute(float distanceRadius)
		{
			DistanceRadius = distanceRadius;
		}
	}
}
