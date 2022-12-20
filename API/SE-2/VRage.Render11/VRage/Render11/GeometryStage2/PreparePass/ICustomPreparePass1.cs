using VRage.Render11.GeometryStage2.Instancing;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	internal interface ICustomPreparePass1
	{
		bool IsInstanceVisible(MyInstance instance);

		bool IsTransitionLodUsed(MyInstance instance);
	}
}
