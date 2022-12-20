using System;
using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[Flags]
	[ComVisible(false)]
	internal enum SeekingCapabilities
	{
		CanSeekAbsolute = 0x1,
		CanSeekForwards = 0x2,
		CanSeekBackwards = 0x4,
		CanGetCurrentPos = 0x8,
		CanGetStopPos = 0x10,
		CanGetDuration = 0x20,
		CanPlayBackwards = 0x40,
		CanDoSegments = 0x80,
		Source = 0x100
	}
}
