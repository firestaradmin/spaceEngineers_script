using System.Runtime.InteropServices;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Render11.LightingStage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SpotlightConstants
	{
		internal Matrix ProxyWorldViewProj;

		internal Matrix ShadowMatrix;

		public MySpotLightLayout Spotlight;
	}
}
