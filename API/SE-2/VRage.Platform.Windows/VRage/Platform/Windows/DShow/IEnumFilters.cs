using System.Runtime.InteropServices;
using DirectShowLib;

namespace VRage.Platform.Windows.DShow
{
	[ComImport]
	[ComVisible(true)]
	[Guid("56a86893-0ad4-11ce-b03a-0020af0ba770")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IEnumFilters
	{
		[PreserveSig]
		int Next([In] int cFilters, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] out IBaseFilter[] ppFilter, out int pcFetched);

		[PreserveSig]
		int Skip([In] int cFilters);

		void Reset();

		void Clone(out IEnumFilters ppEnum);
	}
}
