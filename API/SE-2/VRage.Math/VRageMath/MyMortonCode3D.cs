using System.Diagnostics;

namespace VRageMath
{
	public static class MyMortonCode3D
	{
		public static int Encode(ref Vector3I value)
		{
			return SplitBits(value.X) | (SplitBits(value.Y) << 1) | (SplitBits(value.Z) << 2);
		}

		public static void Decode(int code, out Vector3I value)
		{
			value.X = JoinBits(code);
			value.Y = JoinBits(code >> 1);
			value.Z = JoinBits(code >> 2);
		}

		/// <summary>
		/// Split 10 lowest bits of the number so that there are 3 empty slots between them.
		/// </summary>
		private static int SplitBits(int x)
		{
			x = (x | (x << 16)) & 0x30000FF;
			x = (x | (x << 8)) & 0x300F00F;
			x = (x | (x << 4)) & 0x30C30C3;
			x = (x | (x << 2)) & 0x9249249;
			return x;
		}

		/// <summary>
		/// Reverses splitBits operation.
		/// </summary>
		private static int JoinBits(int x)
		{
			x &= 0x9249249;
			x = (x | (x >> 2)) & 0x30C30C3;
			x = (x | (x >> 4)) & 0x300F00F;
			x = (x | (x >> 8)) & 0x30000FF;
			x = (x | (x >> 16)) & 0x3FF;
			return x;
		}

		[Conditional("DEBUG")]
		private static void AssertRange(Vector3I value)
		{
		}
	}
}
