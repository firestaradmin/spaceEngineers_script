using System;
using System.Runtime.InteropServices;
using BulletXNA.LinearMath;

namespace BulletXNA
{
	public static class MathUtil
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct FloatIntUnion
		{
			[FieldOffset(0)]
			public int i;

			[FieldOffset(0)]
			public float f;
		}

		public const float SIMD_EPSILON = 1.1920929E-07f;

		public const float SIMD_INFINITY = float.MaxValue;

		public static int MaxAxis(ref IndexedVector3 a)
		{
			if (!(a.X < a.Y))
			{
				if (!(a.X < a.Z))
				{
					return 0;
				}
				return 2;
			}
			if (!(a.Y < a.Z))
			{
				return 1;
			}
			return 2;
		}

		public static void VectorMin(ref IndexedVector3 input, ref IndexedVector3 output)
		{
			output.X = Math.Min(input.X, output.X);
			output.Y = Math.Min(input.Y, output.Y);
			output.Z = Math.Min(input.Z, output.Z);
		}

		public static void VectorMax(ref IndexedVector3 input, ref IndexedVector3 output)
		{
			output.X = Math.Max(input.X, output.X);
			output.Y = Math.Max(input.Y, output.Y);
			output.Z = Math.Max(input.Z, output.Z);
		}

		public static float NextAfter(float x, float y)
		{
			if (float.IsNaN(x) || float.IsNaN(y))
			{
				return x + y;
			}
			if (x == y)
			{
				return y;
			}
			FloatIntUnion floatIntUnion = default(FloatIntUnion);
			floatIntUnion.i = 0;
			floatIntUnion.f = x;
			if (x == 0f)
			{
				floatIntUnion.i = 1;
				if (!(y > 0f))
				{
					return 0f - floatIntUnion.f;
				}
				return floatIntUnion.f;
			}
			if (x > 0f == y > x)
			{
				floatIntUnion.i++;
			}
			else
			{
				floatIntUnion.i--;
			}
			return floatIntUnion.f;
		}
	}
}
