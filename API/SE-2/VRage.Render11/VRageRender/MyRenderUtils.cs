using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal static class MyRenderUtils
	{
		public static void BindShaderBundle(MyRenderContext rc, MyMaterialShadersBundleId id)
		{
			rc.SetInputLayout(id.IL);
			rc.VertexShader.Set(id.VS);
			rc.PixelShader.Set(id.PS);
		}

		public static void SetConstants(MyRenderContext rc, ref MyConstantsPack desc, int slot)
		{
			if ((desc.BindFlag & MyBindFlag.BIND_VS) > (MyBindFlag)0)
			{
				rc.VertexShader.SetConstantBuffer(slot, desc.CB);
			}
			if ((desc.BindFlag & MyBindFlag.BIND_PS) > (MyBindFlag)0)
			{
				rc.PixelShader.SetConstantBuffer(slot, desc.CB);
			}
		}

		internal static void SetSrvs(MyRenderContext rc, ref MySrvTable desc)
		{
			ISrvBindable[] srvsForTheOldPipeline = MyGBufferSrvStrategyFactory.GetStrategy().GetSrvsForTheOldPipeline(rc, desc.Srvs);
			if ((desc.BindFlag & MyBindFlag.BIND_VS) > (MyBindFlag)0)
			{
				rc.VertexShader.SetSrvs(desc.StartSlot, srvsForTheOldPipeline);
			}
			if ((desc.BindFlag & MyBindFlag.BIND_PS) > (MyBindFlag)0)
			{
				rc.PixelShader.SetSrvs(desc.StartSlot, srvsForTheOldPipeline);
			}
		}
	}
}
