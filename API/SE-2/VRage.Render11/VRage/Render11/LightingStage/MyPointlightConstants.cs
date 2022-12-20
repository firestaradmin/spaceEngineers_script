using System.Runtime.InteropServices;
using VRageRender.Messages;

namespace VRage.Render11.LightingStage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MyPointlightConstants
	{
		public MyLightLayout Light;
	}
}
