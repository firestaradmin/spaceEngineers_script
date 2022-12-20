using System;

namespace VRage.Voxels.Sewing
{
	[Flags]
	public enum VrSewOperation : byte
	{
		X = 0x2,
		Y = 0x4,
		Z = 0x8,
		XY = 0x10,
		XZ = 0x20,
		YZ = 0x40,
		XYZ = 0x80,
		XFace = 0xB2,
		YFace = 0xD4,
		ZFace = 0xE8,
		All = 0xFE,
		None = 0x0
	}
}
