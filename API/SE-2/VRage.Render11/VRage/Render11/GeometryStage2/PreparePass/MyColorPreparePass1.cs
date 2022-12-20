using System.Runtime.InteropServices;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Instancing;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct MyColorPreparePass1 : ICustomPreparePass1
	{
		public bool IsTransitionLodUsed(MyInstance instance)
		{
			if (instance.LodStrategy.ExplicitLodState == MyInstanceLodState.Solid)
			{
				return instance.LodStrategy.Transition != 0f;
			}
			return false;
		}

		public bool IsInstanceVisible(MyInstance instance)
		{
			if (instance.IsGBufferVisible())
			{
				instance.MarkGbufferVisible();
				return true;
			}
			return false;
		}
	}
}
