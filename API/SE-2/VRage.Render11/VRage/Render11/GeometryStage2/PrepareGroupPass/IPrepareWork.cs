using VRageRender;

namespace VRage.Render11.GeometryStage2.PrepareGroupPass
{
	internal interface IPrepareWork : IPooledObject
	{
		int PassId { get; }

		void PostprocessWork();

		void DoWork();
	}
}
