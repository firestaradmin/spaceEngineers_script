using System;

namespace VRage.Render11.Resources
{
	[Flags]
	internal enum MyFileTextureEnum
	{
		UNSPECIFIED = 0x0,
		COLOR_METAL = 0x1,
		NORMALMAP_GLOSS = 0x2,
		EXTENSIONS = 0x4,
		ALPHAMASK = 0x8,
		GUI = 0x10,
		CUBEMAP = 0x20,
		SYSTEM = 0x40,
		CUSTOM = 0x80,
		GPUPARTICLES = 0x100
	}
}
