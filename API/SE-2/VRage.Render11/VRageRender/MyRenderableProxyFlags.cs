using System;

namespace VRageRender
{
	[Flags]
	internal enum MyRenderableProxyFlags
	{
		None = 0x0,
		DepthSkipTextures = 0x1,
		DisableFaceCulling = 0x2,
		SkipInMainView = 0x4,
		SkipIfTooSmall = 0x8,
		DrawOutsideViewDistance = 0x10,
		CastShadows = 0x20,
		CastShadowsOnLow = 0x40,
		SkipInForward = 0x80,
		SkipInDepth = 0x100,
		DrawInAllCascades = 0x200,
		DistanceFade = 0x400,
		MetalnessColorable = 0x800
	}
}
