using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Tools;
using VRageMath;

namespace VRageRender
{
	internal class MyHdrDebugTools : MyImmediateRC
	{
		private static MyPixelShaders.Id m_drawHistogram;

		private static MyPixelShaders.Id m_drawDebugHistogram;

		private static MyComputeShaders.Id m_buildHistogram;

		private static MyPixelShaders.Id m_psDisplayHdrIntensity;

		private const int m_numthreads = 8;

		public static int NumThreads => 8;

		public static void Init()
		{
			m_buildHistogram = MyComputeShaders.Create("Debug/Histogram.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("NUMTHREADS", 8)
			});
			m_drawHistogram = MyPixelShaders.Create("Debug/DataVisualizationHistogram.hlsl");
			m_drawDebugHistogram = MyPixelShaders.Create("Debug/DataVisualizationDebugHistogram.hlsl");
			m_psDisplayHdrIntensity = MyPixelShaders.Create("Debug/DisplayHdrIntensity.hlsl");
		}

		public static IBorrowedUavTexture CreateHistogram(MyRenderContext rc, ISrvBindable texture, int samples)
		{
			Vector2I size = texture.Size;
			IBorrowedUavTexture borrowedUavTexture = MyManagers.RwTexturesPool.BorrowUav("MyHdrDebugTools.Histogram", 513, 1, Format.R32_UInt);
			rc.ClearUav(borrowedUavTexture, default(RawInt4));
			rc.ComputeShader.Set(m_buildHistogram);
			rc.ComputeShader.SetSrv(0, texture);
			rc.ComputeShader.SetUav(0, borrowedUavTexture);
			MyMapping myMapping = MyMapping.MapDiscard(rc.GetObjectCB(16));
			myMapping.WriteAndPosition(ref size.X);
			myMapping.WriteAndPosition(ref size.Y);
			myMapping.Unmap();
			rc.ComputeShader.SetConstantBuffer(1, rc.GetObjectCB(16));
			rc.Dispatch((size.X + 8 - 1) / 8, (size.Y + 8 - 1) / 8, 1);
			rc.ComputeShader.Set(null);
			return borrowedUavTexture;
		}

		public static void DisplayHistogram(MyRenderContext rc, IRtvBindable output, ISrvBindable avgLumSrv, ISrvTexture histogram)
		{
			rc.PixelShader.SetSrvs(0, histogram, avgLumSrv);
			rc.PixelShader.Set(m_drawHistogram);
			rc.SetRtv(output);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(64f, 64f, 512f, 64f));
		}

		public static void DisplayDebugHistogram(MyRenderContext rc, IRtvBindable output, ISrvTexture debugHistogram)
		{
			rc.SetBlendState(MyBlendStateManager.BlendTransparent);
			rc.PixelShader.SetSrv(0, debugHistogram);
			rc.PixelShader.Set(m_drawDebugHistogram);
			rc.SetRtv(output);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(64f, 64f, 256f, 128f));
			rc.SetBlendState(null);
		}

		public static void DisplayHdrIntensity(MyRenderContext rc, ISrvBindable srv)
		{
			rc.PixelShader.Set(m_psDisplayHdrIntensity);
			rc.PixelShader.SetSrv(5, srv);
			rc.SetBlendState(null);
			IBorrowedRtvTexture borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("MyHdrDebugTools.DisplayHdrIntensity.OutTex", srv.Size.X, srv.Size.Y, Format.B8G8R8X8_UNorm);
			MyScreenPass.RunFullscreenPixelFreq(rc, borrowedRtvTexture);
			MyDebugTextureDisplay.Select(borrowedRtvTexture);
			rc.PixelShader.SetSrv(5, null);
			rc.SetRtvs(MyGBuffer.Main, MyDepthStencilAccess.ReadOnly);
			borrowedRtvTexture.Release();
		}
	}
}
