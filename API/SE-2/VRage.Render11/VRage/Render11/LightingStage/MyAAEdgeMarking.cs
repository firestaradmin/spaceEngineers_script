using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.LightingStage
{
	internal class MyAAEdgeMarking
	{
		private static MyPixelShaders.Id m_ps;

		public static void Init()
		{
			m_ps = MyPixelShaders.Create("Postprocess/EdgeDetection.hlsl");
		}

		public static void Run(MyRenderContext rc)
		{
			rc.SetDepthStencilState(MyDepthStencilStateManager.MarkEdgeInStencil, 255);
			rc.PixelShader.Set(m_ps);
			rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRoDepth);
			rc.PixelShader.SetSrvs(0, MyGBuffer.Main, MyGBufferSrvFilter.NO_STENCIL);
			MyScreenPass.DrawFullscreenQuad(rc);
			rc.SetDepthStencilState(null);
		}
	}
}
