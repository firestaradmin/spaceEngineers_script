using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.LightingStage
{
	internal class MyDepthResolve
	{
		private static MyPixelShaders.Id m_ps;

		public static void Init()
		{
			m_ps = MyPixelShaders.Create("Postprocess/DepthResolve.hlsl");
		}

		public static void Run(MyRenderContext rc, IDepthStencil dst, IDepthStencil src)
		{
			rc.PixelShader.Set(m_ps);
			rc.SetRtv(dst.Dsv);
			rc.PixelShader.SetSrv(0, src.SrvDepth);
			MyScreenPass.DrawFullscreenQuad(rc);
		}
	}
}
