using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	[ComVisible(false)]
	internal class FilterInfo
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string achName;

		[MarshalAs(UnmanagedType.IUnknown)]
		public object pUnk;
	}
}
