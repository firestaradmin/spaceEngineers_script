using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[ComImport]
	[ComVisible(true)]
	[Guid("56a868b1-0ad4-11ce-b03a-0020af0ba770")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IMediaControl
	{
		[PreserveSig]
		int Run();

		[PreserveSig]
		int Pause();

		[PreserveSig]
		int Stop();

		[PreserveSig]
		int GetState(int msTimeout, out int pfs);

		[PreserveSig]
		int RenderFile(string strFilename);

		[PreserveSig]
		int AddSourceFilter([In] string strFilename, [MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int get_FilterCollection([MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int get_RegFilterCollection([MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int StopWhenReady();
	}
}
