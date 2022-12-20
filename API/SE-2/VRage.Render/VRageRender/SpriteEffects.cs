using System;

namespace VRageRender
{
	/// <summary>
	/// Defines sprite mirroring options.
	/// </summary>
	/// <remarks>
	/// Description is taken from original XNA <a href="http://msdn.microsoft.com/en-us/library/VRageMath.graphics.spriteeffects.aspx">SpriteEffects</a> class.
	/// </remarks>
	[Flags]
	public enum SpriteEffects
	{
		/// <summary>
		/// No rotations specified.
		/// </summary>
		None = 0x0,
		/// <summary>
		/// Rotate 180 degrees around the Y axis before rendering.
		/// </summary>
		FlipHorizontally = 0x1,
		/// <summary>
		/// Rotate 180 degrees around the X axis before rendering.
		/// </summary>
		FlipVertically = 0x2,
		/// <summary>
		/// Rotate 180 degress around both the X and Y axis before rendering.
		/// </summary>
		FlipBoth = 0x3
	}
}
