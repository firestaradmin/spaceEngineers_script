namespace VRageMath
{
	/// <summary>
	/// Defines the continuity of CurveKeys on a Curve.
	/// </summary>
	public enum CurveContinuity
	{
		/// <summary>
		/// Interpolation can be used between this CurveKey and the next.
		/// </summary>
		Smooth,
		/// <summary>
		/// Interpolation cannot be used between this CurveKey and the next. Specifying a position between the two points returns this point.
		/// </summary>
		Step
	}
}
