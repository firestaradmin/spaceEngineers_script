using System;

namespace VRage.GameServices
{
	[Flags]
	public enum MySearchConditionFlags : byte
	{
		/// <summary>Argument matches property exactly.</summary>
		Equal = 0x1,
		/// <summary>Argument text is contained in property.</summary>
		Contains = 0x2,
		/// <summary>Property value is numerically greater than or equal to argument.</summary>
		GreaterOrEqual = 0x4,
		/// <summary>Property value is numerically less than or equal to the target.</summary>
		LesserOrEqual = 0x8
	}
}
