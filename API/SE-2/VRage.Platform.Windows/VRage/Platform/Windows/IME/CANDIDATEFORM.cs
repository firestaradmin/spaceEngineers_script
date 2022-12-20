using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.IME
{
	[StructLayout(LayoutKind.Sequential)]
	internal class CANDIDATEFORM
	{
		public int dwIndex;

		public int dwStyle;

		public POINT_IME ptCurrentPos;

		public RECT_IME rcArea;
	}
}
