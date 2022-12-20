using System.Runtime.InteropServices;
using VRage.Render11.GeometryStage2.Instancing;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct MyDepthPreparePass1 : ICustomPreparePass1
	{
		public bool IsTransitionLodUsed(MyInstance instance)
		{
			return false;
		}

		public bool IsInstanceVisible(MyInstance instance)
		{
			return instance.IsDepthVisible();
		}
	}
}
