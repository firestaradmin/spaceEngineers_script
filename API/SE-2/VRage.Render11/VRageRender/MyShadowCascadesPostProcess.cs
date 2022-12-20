using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyShadowCascadesPostProcess : MyImmediateRC
	{
		private static MyVertexShaders.Id m_markVS = MyVertexShaders.Id.NULL;

		private static MyPixelShaders.Id m_markPS = MyPixelShaders.Id.NULL;

		private static MyInputLayouts.Id m_inputLayout = MyInputLayouts.Id.NULL;

		private MyComputeShaders.Id m_gatherCS_LD = MyComputeShaders.Id.NULL;

		private MyComputeShaders.Id m_gatherCS_MD = MyComputeShaders.Id.NULL;

		private MyComputeShaders.Id m_gatherCS_HD = MyComputeShaders.Id.NULL;

		private IVertexBuffer m_cascadesBoundingsVertices;

		private IIndexBuffer m_cubeIB;

		private static IConstantBuffer m_inverseConstants;

		internal MyShadowCascadesPostProcess(int numberOfCascades)
		{
			Init(numberOfCascades);
		}

		private void Init(int numberOfCascades)
		{
			InitResources(numberOfCascades);
			InitShaders();
		}

		internal void Reset(int numberOfCascades)
		{
			UnloadResources();
			Init(numberOfCascades);
		}

		private void InitResources(int numberOfCascades)
		{
			InitVertexBuffer(numberOfCascades);
			InitIndexBuffer();
			InitConstantBuffer();
		}

		internal void UnloadResources()
		{
			DestroyVertexBuffer();
			DestroyIndexBuffer();
		}

		private unsafe void InitConstantBuffer()
		{
			if (m_inverseConstants == null)
			{
				m_inverseConstants = MyManagers.Buffers.CreateConstantBuffer("MyShadowCascadesPostProcess", sizeof(Matrix), null, ResourceUsage.Dynamic, isGlobal: true);
			}
		}

		private void InitShaders()
		{
			if (m_markVS == MyVertexShaders.Id.NULL)
			{
				m_markVS = MyVertexShaders.Create("Shadows/Shape.hlsl");
			}
			if (m_markPS == MyPixelShaders.Id.NULL)
			{
				m_markPS = MyPixelShaders.Create("Shadows/Shape.hlsl");
			}
			if (m_inputLayout == MyInputLayouts.Id.NULL)
			{
				m_inputLayout = MyInputLayouts.Create(m_markVS.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION3));
			}
			m_gatherCS_LD = MyComputeShaders.Create("Shadows/Shadows.hlsl", new ShaderMacro[2]
			{
				new ShaderMacro("ENABLE_PCF", null),
				new ShaderMacro("PoissonSamplesNum", 8)
			});
			m_gatherCS_MD = MyComputeShaders.Create("Shadows/Shadows.hlsl", new ShaderMacro[4]
			{
				new ShaderMacro("ENABLE_PCF", null),
				new ShaderMacro("PoissonSamplesNum", 12),
				new ShaderMacro("HIGHER_CASCADE_PREFERENCE", null),
				new ShaderMacro("ENABLE_CASCADE_BLEND", null)
			});
			m_gatherCS_HD = MyComputeShaders.Create("Shadows/Shadows.hlsl", new ShaderMacro[5]
			{
				new ShaderMacro("ENABLE_PCF", null),
				new ShaderMacro("PoissonSamplesNum", 16),
				new ShaderMacro("HIGHER_CASCADE_PREFERENCE", null),
				new ShaderMacro("ENABLE_DISTORTION", null),
				new ShaderMacro("ENABLE_CASCADE_BLEND", null)
			});
		}

		private unsafe void InitVertexBuffer(int numberOfCascades)
		{
			DestroyVertexBuffer();
			m_cascadesBoundingsVertices = MyManagers.Buffers.CreateVertexBuffer("MyShadowCascadesPostProcess", 8 * numberOfCascades, sizeof(Vector3), null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
		}

		private void DestroyVertexBuffer()
		{
			if (m_cascadesBoundingsVertices != null)
			{
				MyManagers.Buffers.Dispose(m_cascadesBoundingsVertices);
			}
			m_cascadesBoundingsVertices = null;
		}

		private unsafe void InitIndexBuffer()
		{
			DestroyIndexBuffer();
			ushort* ptr = stackalloc ushort[36];
			*ptr = 0;
			ptr[1] = 1;
			ptr[2] = 2;
			ptr[3] = 0;
			ptr[4] = 2;
			ptr[5] = 3;
			ptr[6] = 1;
			ptr[7] = 5;
			ptr[8] = 6;
			ptr[9] = 1;
			ptr[10] = 6;
			ptr[11] = 2;
			ptr[12] = 5;
			ptr[13] = 4;
			ptr[14] = 7;
			ptr[15] = 5;
			ptr[16] = 7;
			ptr[17] = 6;
			ptr[18] = 4;
			ptr[19] = 0;
			ptr[20] = 3;
			ptr[21] = 4;
			ptr[22] = 3;
			ptr[23] = 7;
			ptr[24] = 3;
			ptr[25] = 2;
			ptr[26] = 6;
			ptr[27] = 3;
			ptr[28] = 6;
			ptr[29] = 7;
			ptr[30] = 1;
			ptr[31] = 0;
			ptr[32] = 4;
			ptr[33] = 1;
			ptr[34] = 4;
			ptr[35] = 5;
			m_cubeIB = MyManagers.Buffers.CreateIndexBuffer("MyScreenDecals", 36, new IntPtr(ptr), MyIndexBufferFormat.UShort, ResourceUsage.Immutable, isGlobal: true);
		}

		private void DestroyIndexBuffer()
		{
			if (m_cubeIB != null)
			{
				MyManagers.Buffers.Dispose(m_cubeIB);
			}
			m_cubeIB = null;
		}

		private Vector2I GetThreadGroupCount(Vector2I viewportRes)
		{
			Vector2I vector2I = new Vector2I(16, 16);
			Vector2I result = new Vector2I(viewportRes.X / vector2I.X, viewportRes.Y / vector2I.Y);
			result.X += ((viewportRes.X % vector2I.X != 0) ? 1 : 0);
			result.Y += ((viewportRes.Y % vector2I.Y != 0) ? 1 : 0);
			return result;
		}

		internal void GatherArray(MyRenderContext rc, IUavTexture postprocessTarget, ISrvBindable cascadeArray, IConstantBuffer cascadeConstantBuffer)
		{
			MyRenderQualityEnum shadowGPUQuality = MyRender11.Settings.User.ShadowGPUQuality;
			switch (shadowGPUQuality)
			{
			case MyRenderQualityEnum.LOW:
				rc.ComputeShader.Set(m_gatherCS_LD);
				break;
			case MyRenderQualityEnum.NORMAL:
				rc.ComputeShader.Set(m_gatherCS_MD);
				break;
			case MyRenderQualityEnum.HIGH:
				rc.ComputeShader.Set(m_gatherCS_HD);
				break;
			default:
				rc.ComputeShader.Set(m_gatherCS_HD);
				break;
			}
			rc.ComputeShader.SetUav(0, postprocessTarget);
			rc.ComputeShader.SetSrv(0, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth);
			rc.ComputeShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			if (!MyStereoRender.Enable)
			{
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else
			{
				MyStereoRender.CSBindRawCB_FrameConstants(rc);
			}
			rc.ComputeShader.SetConstantBuffer(4, cascadeConstantBuffer);
			rc.ComputeShader.SetSrv(15, cascadeArray);
			Vector2I threadGroupCount = GetThreadGroupCount(MyRender11.ResolutionI);
			rc.Dispatch(threadGroupCount.X, threadGroupCount.Y, 1);
			rc.ComputeShader.SetUav(0, null);
			rc.ComputeShader.SetUav(1, null);
			rc.ComputeShader.SetSrv(0, null);
			rc.ComputeShader.SetSrv(1, null);
			if (shadowGPUQuality >= MyRenderQualityEnum.HIGH && MyShadowCascades.Settings.Data.EnableShadowBlur)
			{
				IBorrowedUavTexture borrowedUavTexture = MyManagers.RwTexturesPool.BorrowUav("MyShadowCascadesPostProcess.Helper", Format.R8_UNorm);
				MyBlur.Run(rc, postprocessTarget, borrowedUavTexture, postprocessTarget, 5, MyBlur.MyBlurDensityFunctionType.Gaussian, 1.5f, MyDepthStencilStateManager.IgnoreDepthStencil, 0, new RawColor4(1f, 1f, 1f, 1f), 0.1f);
				borrowedUavTexture.Release();
			}
		}
	}
}
