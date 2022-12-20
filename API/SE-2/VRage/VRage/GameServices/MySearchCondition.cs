namespace VRage.GameServices
{
	public enum MySearchCondition : byte
	{
		/// <summary>Argument matches property exactly.</summary>
		Equal,
		/// <summary>Argument text is contained in property.</summary>
		Contains,
		/// <summary>Property value is numerically greater than or equal to argument.</summary>
		GreaterOrEqual,
		/// <summary>Property value is numerically less than or equal to the target.</summary>
		LesserOrEqual
	}
}
