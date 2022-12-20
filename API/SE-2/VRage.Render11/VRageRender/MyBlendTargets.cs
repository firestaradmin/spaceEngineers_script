using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal class MyBlendTargets
	{
		private static MyPixelShaders.Id m_copyPixelShader = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_stencilTestPixelShader = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_stencilInverseTestPixelShader = MyPixelShaders.Id.NULL;

		internal static void Init()
		{
			m_copyPixelShader = MyPixelShaders.Create("Postprocess/PostprocessCopy.hlsl");
			m_stencilTestPixelShader = MyPixelShaders.Create("Postprocess/PostprocessCopyStencil.hlsl");
			m_stencilInverseTestPixelShader = MyPixelShaders.Create("Postprocess/PostprocessCopyInverseStencil.hlsl");
		}

		internal static void Run(MyRenderContext rc, IRtvBindable dst, ISrvBindable src, IBlendState bs = null)
		{
			rc.SetBlendState(bs);
			rc.SetRasterizerState(null);
			rc.SetRtv(dst);
			rc.PixelShader.SetSrv(0, src);
			rc.PixelShader.Set(m_copyPixelShader);
			MyScreenPass.DrawFullscreenQuad(rc);
			rc.SetBlendState(null);
		}

		internal static void RunWithStencil(MyRenderContext rc, IRtvBindable destinationResource, ISrvBindable sourceResource, IBlendState blendState, IDepthStencilState depthStencilState = null, int stencilMask = 0, IDepthStencil depthStencil = null, MyViewport? customViewport = null)
		{
			rc.SetBlendState(blendState);
			rc.SetRasterizerState(null);
			if (depthStencilState == null)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
				rc.SetRtv(destinationResource);
			}
			else
			{
				rc.SetDepthStencilState(depthStencilState, stencilMask);
				rc.SetRtv((depthStencil == null) ? MyGBuffer.Main.DepthStencil.DsvRo : depthStencil.DsvRo, destinationResource);
			}
			rc.PixelShader.SetSrv(0, sourceResource);
			rc.PixelShader.Set(m_copyPixelShader);
			MyScreenPass.DrawFullscreenQuad(rc, customViewport);
			rc.PixelShader.SetSrv(0, null);
			rc.SetBlendState(null);
			rc.SetRtvNull();
		}

		internal static void RunWithPixelStencilTest(MyRenderContext rc, IRtvBindable dst, ISrvBindable src, IBlendState bs = null, bool inverseTest = false, IDepthStencil depthStencil = null)
		{
			rc.SetDepthStencilState(null);
			rc.SetBlendState(bs);
			rc.SetRasterizerState(null);
			rc.SetRtv(dst);
			rc.PixelShader.SetSrv(0, src);
			rc.PixelShader.SetSrv(1, (depthStencil == null) ? MyGBuffer.Main.DepthStencil.SrvStencil : depthStencil.SrvStencil);
			if (!inverseTest)
			{
				rc.PixelShader.Set(m_stencilTestPixelShader);
			}
			else
			{
				rc.PixelShader.Set(m_stencilInverseTestPixelShader);
			}
			MyScreenPass.DrawFullscreenQuad(rc);
			rc.SetBlendState(null);
		}
	}
}
