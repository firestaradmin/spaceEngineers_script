using System;

namespace VRage.Render11.GeometryStage2.Common
{
	[Flags]
	internal enum MyVisibilityExtFlags
	{
		None = 0x0,
		Gbuffer = 0x1,
		Depth = 0x2,
		Forward = 0x4,
		All = 0x7
	}
}
