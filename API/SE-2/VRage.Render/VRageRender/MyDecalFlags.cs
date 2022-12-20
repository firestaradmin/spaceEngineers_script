using System;

namespace VRageRender
{
	[Flags]
	public enum MyDecalFlags
	{
		None = 0x0,
		World = 0x1,
		Transparent = 0x2,
		Closed = 0x4,
		IgnoreOffScreenDeletion = 0x8,
		IgnoreRenderLimits = 0x10
	}
}
