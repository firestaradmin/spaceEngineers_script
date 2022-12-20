using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using VRage;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyBlur
	{
		internal enum MyBlurDensityFunctionType
		{
			Exponential,
			Gaussian
		}

		private struct BlurConstants
		{
			internal float DistributionWeight;

			internal int StencilRef;

			internal Vector2 _padding;
		}

		private static IConstantBuffer m_blurConstantBuffer;

		private static Dictionary<int, MyTuple<MyPixelShaders.Id, MyPixelShaders.Id>> m_blurShaders;

		internal unsafe static void Init()
		{
			m_blurShaders = new Dictionary<int, MyTuple<MyPixelShaders.Id, MyPixelShaders.Id>>();
			m_blurConstantBuffer = MyManagers.Buffers.CreateConstantBuffer("MyBlur", sizeof(BlurConstants), null, ResourceUsage.Dynamic, isGlobal: true);
		}

		private static int GetShaderKey(MyBlurDensityFunctionType densityFunctionType, int maxOffset, bool useDepthDiscard)
		{
			return ((useDepthDiscard ? 1 : 0) << 31) | ((int)(densityFunctionType + 1) << 15) | maxOffset;
		}

		public static int InitShaders(MyBlurDensityFunctionType densityFunctionType, int maxOffset, float depthDiscardThreshold)
		{
			bool flag = depthDiscardThreshold > 0f;
			int shaderKey = GetShaderKey(densityFunctionType, maxOffset, flag);
			if (!m_blurShaders.ContainsKey(shaderKey))
			{
				ShaderMacro shaderMacro = new ShaderMacro(flag ? "DEPTH_DISCARD_THRESHOLD" : "", flag ? depthDiscardThreshold : 1f);
				ShaderMacro[] macros = new ShaderMacro[4]
				{
					new ShaderMacro("HORIZONTAL_PASS", null),
					new ShaderMacro("MAX_OFFSET", maxOffset),
					new ShaderMacro("DENSITY_FUNCTION", (int)densityFunctionType),
					shaderMacro
				};
				ShaderMacro[] macros2 = new ShaderMacro[4]
				{
					new ShaderMacro("VERTICAL_PASS", null),
					new ShaderMacro("MAX_OFFSET", maxOffset),
					new ShaderMacro("DENSITY_FUNCTION", (int)densityFunctionType),
					shaderMacro
				};
				MyTuple<MyPixelShaders.Id, MyPixelShaders.Id> value = MyTuple.Create(MyPixelShaders.Create("Postprocess/Blur.hlsl", macros), MyPixelShaders.Create("Postprocess/Blur.hlsl", macros2));
				m_blurShaders.Add(shaderKey, value);
			}
			return shaderKey;
		}

		internal static void Run(MyRenderContext rc, IRtvBindable renderTarget, IRtvTexture intermediate, ISrvBindable initialResourceView, int maxOffset = 5, MyBlurDensityFunctionType densityFunctionType = MyBlurDensityFunctionType.Gaussian, float WeightParameter = 1.5f, IDepthStencilState depthStencilState = null, int stencilRef = 0, RawColor4 clearColor = default(RawColor4), float depthDiscardThreshold = 0f, MyViewport? viewport = null)
		{
			int key = InitShaders(densityFunctionType, maxOffset, depthDiscardThreshold);
			rc.PixelShader.SetConstantBuffer(5, m_blurConstantBuffer);
			rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			BlurConstants blurConstants = default(BlurConstants);
			blurConstants.DistributionWeight = WeightParameter;
			blurConstants.StencilRef = stencilRef;
			BlurConstants data = blurConstants;
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_blurConstantBuffer);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			rc.ClearRtv(intermediate, clearColor);
			rc.SetRtv(intermediate);
			rc.SetBlendState(null);
			rc.PixelShader.SetSrv(0, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			rc.PixelShader.SetSrv(5, initialResourceView);
			rc.PixelShader.Set(m_blurShaders[key].Item1);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
			rc.PixelShader.SetSrv(5, null);
			rc.ClearRtv(renderTarget, clearColor);
			if (depthStencilState == null)
			{
				rc.SetRtv(renderTarget);
			}
			else
			{
				rc.SetDepthStencilState(depthStencilState, stencilRef);
				rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRo, renderTarget);
			}
			rc.PixelShader.SetSrv(5, intermediate);
			rc.PixelShader.Set(m_blurShaders[key].Item2);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
			rc.PixelShader.SetSrv(0, null);
			rc.PixelShader.SetSrv(4, null);
			rc.PixelShader.SetSrv(5, null);
			rc.SetRtvNull();
		}
	}
}
