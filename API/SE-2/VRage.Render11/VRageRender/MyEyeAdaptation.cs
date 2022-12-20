using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
<<<<<<< HEAD
using VRage.Utils;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRageMath;

namespace VRageRender
{
	internal class MyEyeAdaptation : MyImmediateRC
	{
		private const int _histogramThreadCountX = 16;

		private const int _histogramThreadCountY = 16;

		private const int _histogramBinCount = 64;

		private static MyComputeShaders.Id m_updateHistogramShader;

		private static MyComputeShaders.Id m_updateHistogramShaderScreenCenter;

		private static MyPixelShaders.Id m_constantExposureShader;

		private static MyPixelShaders.Id m_eyeAdaptationShader;

		private static MyPixelShaders.Id m_downSampleShader;

		private static MyPixelShaders.Id m_debugHistogramShader;

		private static IRtvTexture[] m_autoExposure;

		private static ISrvUavBuffer m_histogram;

		public static IRtvTexture GetExposure()
		{
			return m_autoExposure[0];
		}

		internal static void Init(MyRenderContext rc)
		{
			m_autoExposure = new IRtvTexture[2];
			for (int i = 0; i < 2; i++)
			{
				m_autoExposure[i] = MyManagers.RwTextures.CreateRtv("AutoExposure" + i, 1, 1, Format.R32G32_Float);
				rc.ClearRtv(m_autoExposure[i], default(RawColor4));
			}
			m_histogram = MyManagers.Buffers.CreateSrvUav("Histogram", 64, 4, null, MyUavType.Default, ResourceUsage.Default, isGlobal: true);
			m_updateHistogramShader = MyComputeShaders.Create("Postprocess/EyeAdaptation/UpdateHistogram.hlsl");
			m_updateHistogramShaderScreenCenter = MyComputeShaders.Create("Postprocess/EyeAdaptation/UpdateHistogram.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("PRIORITIZE_SCREEN_CENTER", 1)
			});
			m_downSampleShader = MyPixelShaders.Create("Postprocess/EyeAdaptation/DownSample.hlsl");
			m_debugHistogramShader = MyPixelShaders.Create("Postprocess/EyeAdaptation/DebugHistogram.hlsl");
			m_eyeAdaptationShader = MyPixelShaders.Create("Postprocess/EyeAdaptation/EyeAdaptation.hlsl");
			m_constantExposureShader = MyPixelShaders.Create("Postprocess/EyeAdaptation/ConstantExposure.hlsl");
		}

		internal static void ConstantExposure(MyRenderContext rc)
		{
			rc.SetBlendState(null);
			rc.SetInputLayout(null);
			rc.PixelShader.Set(m_constantExposureShader);
			rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.SetRtv(m_autoExposure[0]);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(1f, 1f));
			rc.SetRtvNull();
		}

		internal static void Run(MyRenderContext rc, ISrvTexture src, bool createDebugHistogram, out IBorrowedRtvTexture debugHistogram)
		{
			debugHistogram = null;
			IBorrowedRtvTexture borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("HalfTexture", src.Size.X / 2, src.Size.Y / 2, src.Format);
			float num = MyRender11.Postprocess.HistogramLogMax - MyRender11.Postprocess.HistogramLogMin;
			Vector4 data = new Vector4(1f / num, (0f - MyRender11.Postprocess.HistogramLogMin) / num, num, MyRender11.Postprocess.HistogramLogMin);
			Vector4 data2 = new Vector4((float)Math.Pow(2.0, MyRender11.Postprocess.MinEyeAdaptationLogBrightness), (float)Math.Pow(2.0, MyRender11.Postprocess.MaxEyeAdaptationLogBrightness), MyRender11.Postprocess.HistogramFilterMin * 0.01f, MyRender11.Postprocess.HistogramFilterMax * 0.01f);
			Vector4 data3 = new Vector4(borrowedRtvTexture.Size.X, borrowedRtvTexture.Size.Y, 1f / (float)(borrowedRtvTexture.Size.X * borrowedRtvTexture.Size.Y), MyPostprocessSettingsWrapper.Settings.HistogramLuminanceThreshold);
			Vector4 data4 = new Vector4(MyRender11.Postprocess.HistogramSkyboxFactor, 0f, 0f, 0f);
			IConstantBuffer objectCB = rc.GetObjectCB(64);
			MyMapping myMapping = MyMapping.MapDiscard(objectCB);
			myMapping.WriteAndPosition(ref data);
			myMapping.WriteAndPosition(ref data2);
			myMapping.WriteAndPosition(ref data3);
			myMapping.WriteAndPosition(ref data4);
			myMapping.Unmap();
			rc.ClearUav(m_histogram, default(RawInt4));
			rc.SetBlendState(null);
			rc.SetInputLayout(null);
			rc.PixelShader.Set(m_downSampleShader);
			rc.SetRtv(borrowedRtvTexture);
			rc.PixelShader.SetSrv(0, src);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(borrowedRtvTexture.Size.X, borrowedRtvTexture.Size.Y));
			rc.SetRtvNull();
			rc.ComputeShader.SetConstantBuffer(1, objectCB);
			rc.ComputeShader.SetUav(0, m_histogram);
			rc.ComputeShader.SetSrv(0, borrowedRtvTexture);
			rc.ComputeShader.SetSrv(1, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth);
			if (MyPostprocessSettingsWrapper.Settings.EyeAdaptationPrioritizeScreenCenter)
			{
				rc.ComputeShader.Set(m_updateHistogramShaderScreenCenter);
			}
			else
			{
				rc.ComputeShader.Set(m_updateHistogramShader);
			}
			rc.Dispatch((int)Math.Ceiling((float)borrowedRtvTexture.Size.X / 16f), (int)Math.Ceiling((float)borrowedRtvTexture.Size.Y / 16f), 1);
			rc.ComputeShader.SetUav(0, null);
			rc.ComputeShader.SetSrv(0, null);
			borrowedRtvTexture.Release();
			rc.SetBlendState(null);
			rc.SetInputLayout(null);
			rc.PixelShader.Set(m_eyeAdaptationShader);
			rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.PixelShader.SetConstantBuffer(1, objectCB);
			rc.SetRtv(m_autoExposure[1]);
			rc.PixelShader.SetSrv(0, m_histogram);
			rc.PixelShader.SetSrv(1, m_autoExposure[0]);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(borrowedRtvTexture.Size.X, borrowedRtvTexture.Size.Y));
			rc.SetRtvNull();
			IRtvTexture rtvTexture = m_autoExposure[0];
			m_autoExposure[0] = m_autoExposure[1];
			m_autoExposure[1] = rtvTexture;
			if (createDebugHistogram)
			{
				debugHistogram = MyManagers.RwTexturesPool.BorrowRtv("DebugHistogram", 256, 128, Format.R8G8B8A8_UNorm);
				rc.SetBlendState(null);
				rc.SetInputLayout(null);
				rc.PixelShader.Set(m_debugHistogramShader);
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				rc.PixelShader.SetConstantBuffer(1, objectCB);
				rc.SetRtv(debugHistogram);
				rc.PixelShader.SetSrv(0, m_histogram);
				rc.PixelShader.SetSrv(1, m_autoExposure[0]);
				MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(debugHistogram.Size.X, debugHistogram.Size.Y));
			}
		}
<<<<<<< HEAD

		internal static void Reset()
		{
			MyLog.Default.Info("MyEyeAdaptation reset");
			for (int i = 0; i < 2; i++)
			{
				MyRender11.RC.ClearRtv(m_autoExposure[i], default(RawColor4));
			}
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
