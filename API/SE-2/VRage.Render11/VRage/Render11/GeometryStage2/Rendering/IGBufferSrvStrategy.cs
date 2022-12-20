using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal interface IGBufferSrvStrategy
	{
		ISrvBindable[] GetSrvs(MyRenderContext rc, MyRenderMaterialBindings part, int lodNum);

		ISrvBindable[] GetSrvsForTheOldPipeline(MyRenderContext rc, ISrvBindable[] srv);
	}
}
