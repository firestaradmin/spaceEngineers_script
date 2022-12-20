using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.IME
{
	[StructLayout(LayoutKind.Sequential)]
	internal class CANDIDATELIST
	{
		public int dwSize;

		public int dwStyle;

		public int dwCount;

		public int dwSelection;

		public int dwPageStart;

		public int dwPageSize;

		public int dwOffset;
	}
}
