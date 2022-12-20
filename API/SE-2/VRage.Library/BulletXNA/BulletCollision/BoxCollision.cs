using System;

namespace BulletXNA.BulletCollision
{
	internal static class BoxCollision
	{
		public static bool BT_GREATER(float x, float y)
		{
			return Math.Abs(x) > y;
		}

		public static float BT_MAX(float a, float b)
		{
			return Math.Max(a, b);
		}

		public static float BT_MIN(float a, float b)
		{
			return Math.Min(a, b);
		}
	}
}
