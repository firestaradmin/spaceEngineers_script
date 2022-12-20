using System;
using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[Flags]
	[ComVisible(false)]
	internal enum SeekingFlags
	{
		NoPositioning = 0x0,
		AbsolutePositioning = 0x1,
		RelativePositioning = 0x2,
		IncrementalPositioning = 0x3,
		SeekToKeyFrame = 0x4,
		ReturnTime = 0x8,
		Segment = 0x10,
		NoFlush = 0x20
	}
}
