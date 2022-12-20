using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	internal class VideoInfoHeader
	{
		public DsRECT SrcRect;

		public DsRECT TagRect;

		public int BitRate;

		public int BitErrorRate;

		public long AvgTimePerFrame;

		public DsBITMAPINFOHEADER BmiHeader;
	}
}
