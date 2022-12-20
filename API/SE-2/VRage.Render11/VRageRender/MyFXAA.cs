using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal class MyFXAA
	{
		private static MyPixelShaders.Id m_ps;

		internal static void Init()
		{
			m_ps = MyPixelShaders.Create("Postprocess/Fxaa.hlsl");
		}

		internal static void Run(MyRenderContext rc, IRtvBindable destination, ISrvBindable source)
		{
			rc.SetBlendState(null);
			rc.SetInputLayout(null);
			rc.PixelShader.Set(m_ps);
			rc.SetRtv(destination);
			rc.PixelShader.SetSrv(0, source);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(destination.Size.X, destination.Size.Y));
			rc.PixelShader.SetSrv(0, null);
			rc.SetRtvNull();
		}
	}
}
