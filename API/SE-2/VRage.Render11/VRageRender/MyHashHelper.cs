namespace VRageRender
{
	internal class MyHashHelper
	{
		public static int Combine(int h0, int h1)
		{
			return (17 * 31 + h0) * 31 + h1;
		}

		public static void Combine(ref int h0, int h1)
		{
			h0 = Combine(h0, h1);
		}
	}
}
