using System;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Tools;
using VRageMath;

namespace VRageRender
{
	internal class MyModernBloom
	{
		private static MyPixelShaders.Id m_preFilterShader;

		private static MyPixelShaders.Id m_preFilterAntiFlickerShader;

		private static MyPixelShaders.Id m_downsampleBlurShader;

		private static MyPixelShaders.Id m_upsampleBlurShader;

		private const int MaxBloomSteps = 10;

		private static readonly IBorrowedRtvTexture[] m_bloomCascadeDown = new IBorrowedRtvTexture[10];

		private static readonly IBorrowedRtvTexture[] m_bloomCascadeUp = new IBorrowedRtvTexture[10];

		internal static void Init()
		{
			ShaderMacro[] macros = new ShaderMacro[1]
			{
				new ShaderMacro("BLOOM_ANTIFLICKER", 1)
			};
			m_preFilterShader = MyPixelShaders.Create("Postprocess/Bloom/PreFilter.hlsl");
			m_preFilterAntiFlickerShader = MyPixelShaders.Create("Postprocess/Bloom/PreFilter.hlsl", macros);
			m_downsampleBlurShader = MyPixelShaders.Create("Postprocess/Bloom/DownsampleBlur.hlsl");
			m_upsampleBlurShader = MyPixelShaders.Create("Postprocess/Bloom/UpsampleBlur.hlsl");
		}

		internal static IBorrowedRtvTexture Run(MyRenderContext rc, ISrvBindable src, ISrvBindable srcGBuffer2, ISrvBindable srcDepth, ISrvBindable exposure)
		{
			int num = src.Size.X;
			int num2 = src.Size.Y;
			if (!MyRender11.Postprocess.HighQualityBloom)
			{
				num /= 2;
				num2 /= 2;
			}
			Format lBufferFormat = MyGBuffer.LBufferFormat;
			IBorrowedRtvTexture borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("BloomPrefilter", num, num2, lBufferFormat);
			rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.PixelShader.SetSrv(1, srcGBuffer2);
			rc.PixelShader.SetSrv(2, srcDepth);
			rc.PixelShader.SetSrv(3, exposure);
			if (MyRender11.Postprocess.BloomAntiFlickerFilter)
			{
				Blit(rc, src, borrowedRtvTexture, m_preFilterAntiFlickerShader);
			}
			else
			{
				Blit(rc, src, borrowedRtvTexture, m_preFilterShader);
			}
			IBorrowedRtvTexture borrowedRtvTexture2 = borrowedRtvTexture;
			int num3 = (int)(float)Math.Log(num2, 2.0);
			int num4 = MathHelper.Log2(256);
			int value = num3 - num4 + MathHelper.Clamp(MyRender11.Postprocess.BloomSize, 1, num4);
			value = MathHelper.Clamp(value, 1, 10);
			for (int i = 0; i < value; i++)
			{
				num /= 2;
				num2 /= 2;
				m_bloomCascadeDown[i] = MyManagers.RwTexturesPool.BorrowRtv("bloomTemp", num, num2, lBufferFormat);
				Blit(rc, borrowedRtvTexture2, m_bloomCascadeDown[i], m_downsampleBlurShader);
				borrowedRtvTexture2 = m_bloomCascadeDown[i];
			}
			for (int num5 = value - 2; num5 >= 0; num5--)
			{
				m_bloomCascadeUp[num5] = MyManagers.RwTexturesPool.BorrowRtv("bloomTemp", m_bloomCascadeDown[num5].Size.X, m_bloomCascadeDown[num5].Size.Y, lBufferFormat);
				rc.PixelShader.SetSrv(1, m_bloomCascadeDown[num5]);
				Blit(rc, borrowedRtvTexture2, m_bloomCascadeUp[num5], m_upsampleBlurShader);
				borrowedRtvTexture2 = m_bloomCascadeUp[num5];
			}
			rc.SetRtvNull();
			if (MyRender11.Settings.DisplayBloomFilter)
			{
				MyDebugTextureDisplay.Select(borrowedRtvTexture);
			}
			else if (MyRender11.Settings.DisplayBloomMin)
			{
				MyDebugTextureDisplay.Select(borrowedRtvTexture2);
			}
			borrowedRtvTexture.Release();
			for (uint num6 = 0u; num6 < 10; num6++)
			{
				if (m_bloomCascadeDown[num6] != null)
				{
					m_bloomCascadeDown[num6].Release();
					m_bloomCascadeDown[num6] = null;
				}
				if (m_bloomCascadeUp[num6] != null)
				{
					if (m_bloomCascadeUp[num6] != borrowedRtvTexture2)
					{
						m_bloomCascadeUp[num6].Release();
					}
					m_bloomCascadeUp[num6] = null;
				}
			}
			return borrowedRtvTexture2;
		}

		internal static void Blit(MyRenderContext rc, ISrvBindable source, IRtvBindable target, MyPixelShaders.Id shader)
		{
			rc.SetBlendState(null);
			rc.SetInputLayout(null);
			rc.PixelShader.Set(shader);
			rc.SetRtv(target);
			rc.PixelShader.SetSrv(0, source);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(target.Size.X, target.Size.Y));
		}
	}
}
