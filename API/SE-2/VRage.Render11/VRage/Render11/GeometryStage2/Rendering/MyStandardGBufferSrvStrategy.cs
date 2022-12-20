using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyStandardGBufferSrvStrategy : IGBufferSrvStrategy
	{
		public ISrvBindable[] GetSrvs(MyRenderContext rc, MyRenderMaterialBindings part, int lodNum)
		{
			return part.Srvs;
		}

		public ISrvBindable[] GetSrvsForTheOldPipeline(MyRenderContext rc, ISrvBindable[] srvs)
		{
			return srvs;
		}
	}
}
