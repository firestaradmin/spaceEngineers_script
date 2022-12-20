using SharpDX.Direct3D;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.Tools
{
	internal static class MyDebugTextureDisplay
	{
		private static MyPixelShaders.Id m_ps = MyPixelShaders.Id.NULL;

		private static IRtvTexture m_selRtvTexture;

		private static IUavTexture m_selUavTexture;

		private static IBorrowedRtvTexture m_selBorrowedRtvTexture;

		private static IBorrowedUavTexture m_selBorrowedUavTexture;

		public static void Deselect()
		{
			m_selRtvTexture = null;
			m_selUavTexture = null;
			if (m_selBorrowedRtvTexture != null)
			{
				m_selBorrowedRtvTexture.Release();
				m_selBorrowedRtvTexture = null;
			}
			if (m_selBorrowedUavTexture != null)
			{
				m_selBorrowedUavTexture.Release();
				m_selBorrowedUavTexture = null;
			}
		}

		public static void Select(IRtvTexture tex)
		{
			Deselect();
			m_selRtvTexture = tex;
		}

		public static void Select(IUavTexture tex)
		{
			Deselect();
			m_selUavTexture = tex;
		}

		public static void Select(IBorrowedRtvTexture tex)
		{
			Deselect();
			tex.AddRef();
			m_selBorrowedRtvTexture = tex;
		}

		public static void Select(IBorrowedUavTexture tex)
		{
			Deselect();
			tex.AddRef();
			m_selBorrowedUavTexture = tex;
		}

		public static void Draw(IRtvBindable renderTarget)
		{
			ISrvBindable srvBindable = null;
			if (m_selRtvTexture != null)
			{
				srvBindable = m_selRtvTexture;
			}
			if (m_selUavTexture != null)
			{
				srvBindable = m_selUavTexture;
			}
			if (m_selBorrowedRtvTexture != null)
			{
				srvBindable = m_selBorrowedRtvTexture;
			}
			if (m_selBorrowedUavTexture != null)
			{
				srvBindable = m_selBorrowedUavTexture;
			}
			if (srvBindable != null)
			{
				if (m_ps == MyPixelShaders.Id.NULL)
				{
					m_ps = MyPixelShaders.Create("Debug/DebugRt.hlsl");
				}
				MyRenderContext rC = MyImmediateRC.RC;
				rC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
				rC.SetViewport(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
				rC.SetRtv(renderTarget);
				rC.SetBlendState(null);
				rC.PixelShader.Set(m_ps);
				rC.PixelShader.SetSrv(0, srvBindable);
				rC.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				MyScreenPass.DrawFullscreenQuad(rC);
				Deselect();
			}
		}
	}
}
