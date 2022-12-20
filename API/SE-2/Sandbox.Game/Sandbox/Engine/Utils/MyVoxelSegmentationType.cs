namespace Sandbox.Engine.Utils
{
	public enum MyVoxelSegmentationType
	{
		/// <summary>
		/// Fastest method, but not very efficient, there's usually 100% more shapes (compared to optimized).
		/// It's about 40x faster than optimized version and 50x faster than fast version.
		/// Often generates just long lines instead of boxes.
		/// </summary>
		ExtraSimple,
		/// <summary>
		/// Quite fast method and quite efficient, there's usually similar number of shapes (compared to optimized).
		/// It's about 3x faster than optimized version, but prefers longer boxes.
		/// </summary>
		Fast,
		/// <summary>
		/// Slowest method, generates lowest number of shapes.
		/// Prefers cubic boxes.
		/// </summary>
		Optimized,
		/// <summary>
		/// Generates a number of shapes comparable to Optimized in a time comparable to ExtraSimple.
		/// </summary>
		Simple,
		/// <summary>
		/// Little optimization added to Simple
		/// </summary>
		Simple2
	}
}
