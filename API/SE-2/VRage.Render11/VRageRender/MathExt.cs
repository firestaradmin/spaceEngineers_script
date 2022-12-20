using System;

namespace VRageRender
{
	internal static class MathExt
	{
		internal static float Saturate(float v)
		{
			return Math.Min(1f, Math.Max(0f, v));
		}
	}
}
