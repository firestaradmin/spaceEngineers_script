using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Common;
using VRage.Render11.LightingStage;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MyGBuffer
	{
		internal static MyGBuffer Main;

		private static HDRType m_hdrType;

		private int m_samplesCount;

		private int m_samplesQuality;

		private IDepthStencil m_depthStencil;

		private IDepthStencil m_resolvedDepthStencil;

		private IRtvTexture m_gbuffer0;

		private IRtvTexture m_gbuffer1;

		private IRtvTexture m_gbuffer2;

		private IRtvTexture m_lbuffer;

		internal readonly RenderTargetView[] GbufferRtvs = new RenderTargetView[3];

		private IBorrowedRtvTexture m_gbuffer1Copy;

<<<<<<< HEAD
		internal static Format LBufferFormat
		{
			get
			{
				switch (m_hdrType)
				{
				case HDRType.LDR:
					return Format.R8G8B8A8_UNorm;
				case HDRType.HDR:
					return Format.R11G11B10_Float;
				case HDRType.HDR_HQ:
					return Format.R16G16B16A16_Float;
				default:
					return Format.Unknown;
				}
			}
		}
=======
		internal static Format LBufferFormat => m_hdrType switch
		{
			HDRType.LDR => Format.R8G8B8A8_UNorm, 
			HDRType.HDR => Format.R11G11B10_Float, 
			HDRType.HDR_HQ => Format.R16G16B16A16_Float, 
			_ => Format.Unknown, 
		};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public int SamplesCount => m_samplesCount;

		public int SamplesQuality => m_samplesQuality;

		internal IDepthStencil DepthStencil => m_depthStencil;

		internal IDepthStencil ResolvedDepthStencil => m_resolvedDepthStencil;

		internal IRtvTexture GBuffer0 => m_gbuffer0;

		internal IRtvTexture GBuffer1 => m_gbuffer1;

		internal IRtvTexture GBuffer2 => m_gbuffer2;

		internal IRtvTexture LBuffer => m_lbuffer;

		public void ResolveMultisample(MyRenderContext rc)
		{
			if (MyRender11.MultisamplingEnabled)
			{
				rc.ClearDsv(m_resolvedDepthStencil, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
				MyAAEdgeMarking.Run(rc);
				MyDepthResolve.Run(rc, m_resolvedDepthStencil, m_depthStencil);
			}
		}

		public IBorrowedRtvTexture GetGbuffer1CopyRtv()
		{
			return m_gbuffer1Copy;
		}

		public void InitFrame()
		{
			int x = MyRender11.ResolutionI.X;
			int y = MyRender11.ResolutionI.Y;
			int samplesCount = MyRender11.Settings.User.AntialiasingMode.SamplesCount();
			m_gbuffer1Copy = MyManagers.RwTexturesPool.BorrowRtv("MyGlobalResources.Gbuffer1Copy", x, y, Format.R10G10B10A2_UNorm, samplesCount, SamplesQuality);
		}

		public void DoneFrame()
		{
			m_gbuffer1Copy.Release();
			m_gbuffer1Copy = null;
		}

		public void UpdateGbuffer1CopyRtv(MyRenderContext rc)
		{
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("UpdateGbuffer1CopyRtv", "UpdateGbuffer1CopyRtv", "E:\\Repo1\\Sources\\VRage.Render11\\Resources\\MyGbuffer.cs");
			rc.CopyResource(GBuffer1, m_gbuffer1Copy);
			MyGpuProfiler.IC_EndBlock(0f, "UpdateGbuffer1CopyRtv", "E:\\Repo1\\Sources\\VRage.Render11\\Resources\\MyGbuffer.cs");
=======
			MyGpuProfiler.IC_BeginBlock("UpdateGbuffer1CopyRtv", "UpdateGbuffer1CopyRtv", "E:\\Repo3\\Sources\\VRage.Render11\\Resources\\MyGbuffer.cs");
			rc.CopyResource(GBuffer1, m_gbuffer1Copy);
			MyGpuProfiler.IC_EndBlock(0f, "UpdateGbuffer1CopyRtv", "E:\\Repo3\\Sources\\VRage.Render11\\Resources\\MyGbuffer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public IBorrowedDepthStencilTexture GetDepthStencilCopyRtv(MyRenderContext rc)
		{
			int x = MyRender11.ResolutionI.X;
			int y = MyRender11.ResolutionI.Y;
			int samplesCount = MyRender11.Settings.User.AntialiasingMode.SamplesCount();
			IBorrowedDepthStencilTexture borrowedDepthStencilTexture = MyManagers.RwTexturesPool.BorrowDepthStencil("DepthStencilCopy", x, y, MyRender11.Settings.User.HqDepth, samplesCount, SamplesQuality);
			rc.CopyResource(DepthStencil, borrowedDepthStencilTexture);
			return borrowedDepthStencilTexture;
		}

		internal void Resize(int width, int height, int samplesNum, int samplesQuality, HDRType hdrType)
		{
			Release();
			m_samplesCount = samplesNum;
			m_samplesQuality = samplesQuality;
			m_hdrType = hdrType;
			MyDepthStencilManager depthStencils = MyManagers.DepthStencils;
			m_depthStencil = depthStencils.CreateDepthStencil("MyGBuffer.DepthStencil", width, height, MyRender11.Settings.User.HqDepth, samplesNum, samplesQuality);
			if (MyRender11.MultisamplingEnabled)
			{
				m_resolvedDepthStencil = depthStencils.CreateDepthStencil("MyGBuffer.ResolvedDepth", width, height, MyRender11.Settings.User.HqDepth);
			}
			else
			{
				m_resolvedDepthStencil = m_depthStencil;
			}
			MyRwTextureManager rwTextures = MyManagers.RwTextures;
			m_gbuffer0 = rwTextures.CreateRtv("MyGBuffer.GBuffer0", width, height, Format.R8G8B8A8_UNorm_SRgb, samplesNum, samplesQuality);
			m_gbuffer1 = rwTextures.CreateRtv("MyGBuffer.GBuffer1", width, height, Format.R10G10B10A2_UNorm, samplesNum, samplesQuality);
			m_gbuffer2 = rwTextures.CreateRtv("MyGBuffer.GBuffer2", width, height, Format.R8G8B8A8_UNorm, samplesNum, samplesQuality);
			m_lbuffer = rwTextures.CreateRtv("MyGBuffer.LBuffer", width, height, LBufferFormat, samplesNum, samplesQuality);
			GbufferRtvs[0] = m_gbuffer0.Rtv;
			GbufferRtvs[1] = m_gbuffer1.Rtv;
			GbufferRtvs[2] = m_gbuffer2.Rtv;
		}

		internal void Release()
		{
			MyDepthStencilManager depthStencils = MyManagers.DepthStencils;
			MyRwTextureManager rwTextures = MyManagers.RwTextures;
			depthStencils.DisposeTex(ref m_depthStencil);
			if (MyRender11.MultisamplingEnabled)
			{
				depthStencils.DisposeTex(ref m_resolvedDepthStencil);
			}
			else
			{
				m_resolvedDepthStencil = null;
			}
			rwTextures.DisposeTex(ref m_gbuffer0);
			rwTextures.DisposeTex(ref m_gbuffer1);
			rwTextures.DisposeTex(ref m_gbuffer2);
			rwTextures.DisposeTex(ref m_lbuffer);
		}

		internal void Clear(MyRenderContext rc, Color clearcolor)
		{
			rc.ClearDsv(DepthStencil, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, MyRender11.DepthClearValue, 0);
			Vector3 vector = clearcolor.ToVector3();
			rc.ClearRtv(m_gbuffer0, new RawColor4(vector.X, vector.Y, vector.Z, 1f));
			rc.ClearRtv(m_gbuffer1, default(RawColor4));
			rc.ClearRtv(m_gbuffer2, default(RawColor4));
			rc.ClearRtv(m_lbuffer, default(RawColor4));
		}
	}
}
