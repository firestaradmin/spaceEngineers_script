using System;

namespace VRageRender.Messages
{
	[Flags]
	public enum TextureType
	{
		GUI = 0x1,
		Particles = 0x2,
		ColorMetal = 0x4,
		NormalGloss = 0x8,
		AlphaMask = 0x10,
		Extensions = 0x20,
		GUIWithoutPremultiplyAlpha = 0x40,
		CubeMap = 0x80
	}
}
