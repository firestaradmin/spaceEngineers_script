using System;
using SharpDX.Mathematics.Interop;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MySSAO
	{
		internal static MySSAOSettings Params = MySSAOSettings.Default;

		private static readonly Vector2[] m_filterKernel = new Vector2[8]
		{
			new Vector2(1f, 1f),
			new Vector2(0.25f, -0.25f),
			new Vector2(-1f, -1f),
			new Vector2(-0.25f, 0.25f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.75f, -0.75f),
			new Vector2(-0.5f, -0.5f),
			new Vector2(-0.75f, 0.75f)
		};

		private const int NUM_SAMPLES = 8;

		private static readonly Vector4[] m_tmpOccluderPoints = new Vector4[8];

		private static readonly Vector4[] m_tmpOccluderPointsFlipped = new Vector4[8];

		private static MyPixelShaders.Id m_ps;

		private static void FillRandomVectors(MyMapping myMapping)
		{
			float num = -1f;
			for (uint num2 = 0u; num2 < 8; num2++)
			{
				float val = m_filterKernel[num2].Length();
				num = Math.Max(num, val);
			}
			float num3 = 1f / num;
			float num4 = 0f;
			for (uint num5 = 0u; num5 < 8; num5++)
			{
				Vector2 vector = new Vector2(m_filterKernel[num5].X * num3, m_filterKernel[num5].Y * num3);
				m_tmpOccluderPoints[num5].X = vector.X;
				m_tmpOccluderPoints[num5].Y = vector.Y;
				m_tmpOccluderPoints[num5].Z = 0f;
				m_tmpOccluderPoints[num5].W = (float)Math.Sqrt(1f - vector.X * vector.X - vector.Y * vector.Y);
				num4 += m_tmpOccluderPointsFlipped[num5].W;
				m_tmpOccluderPointsFlipped[num5].X = vector.X;
				m_tmpOccluderPointsFlipped[num5].Y = 0f - vector.Y;
			}
			for (int i = 0; i < 8; i++)
			{
				myMapping.WriteAndPosition(ref m_tmpOccluderPoints[i]);
			}
			for (int j = 0; j < 8; j++)
			{
				myMapping.WriteAndPosition(ref m_tmpOccluderPointsFlipped[j]);
			}
		}

		internal static void Init()
		{
			m_ps = MyPixelShaders.Create("Postprocess/SSAO/Ssao.hlsl");
		}

		internal static void Run(MyRenderContext rc, IRtvBindable dst, MyGBuffer gbuffer)
		{
			rc.ClearRtv(dst, new RawColor4(1f, 1f, 1f, 1f));
			IConstantBuffer objectCB = rc.GetObjectCB(288);
			MyMapping myMapping = MyMapping.MapDiscard(objectCB);
			myMapping.WriteAndPosition(ref Params.Data);
			FillRandomVectors(myMapping);
			myMapping.Unmap();
			if (!MyStereoRender.Enable)
			{
				rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else
			{
				MyStereoRender.BindRawCB_FrameConstants(rc);
			}
			rc.AllShaderStages.SetConstantBuffer(1, objectCB);
			rc.PixelShader.Set(m_ps);
			rc.SetRtv(dst);
			rc.PixelShader.SetSrvs(0, gbuffer);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.PixelShader.SetSrv(5, gbuffer.ResolvedDepthStencil.SrvDepth);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			MyScreenPass.DrawFullscreenQuad(rc);
			rc.ResetTargets();
		}
	}
}
