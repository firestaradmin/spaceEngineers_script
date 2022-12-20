using System.Text;
using VRageMath;

namespace Sandbox.Game
{
	public static class StringExtensions
	{
		public static int Get7bitEncodedSize(this string self)
		{
			int byteCount = Encoding.UTF8.GetByteCount(self);
			return byteCount + (MathHelper.Log2Floor(byteCount) + 6) / 7;
		}
	}
}
