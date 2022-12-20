using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[ComImport]
	[ComVisible(true)]
	[Guid("56a868b3-0ad4-11ce-b03a-0020af0ba770")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IBasicAudio
	{
		[PreserveSig]
		int put_Volume(int lVolume);

		[PreserveSig]
		int get_Volume(out int plVolume);

		[PreserveSig]
		int put_Balance(int lBalance);

		[PreserveSig]
		int get_Balance(out int plBalance);
	}
}
