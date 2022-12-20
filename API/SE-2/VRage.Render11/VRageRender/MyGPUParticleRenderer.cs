using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage;
using VRage.Render.Particles;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyGPUParticleRenderer
	{
		private struct Particle
		{
			internal Vector4 Params1;

			internal Vector4 Params2;

			internal Vector4 Params3;

			internal Vector4 Params4;
		}

		private struct EmitterConstantBuffer
		{
			internal int EmittersCount;

			internal Vector3 Pad;
		}

		private static bool m_resetSystem;

		private const bool CHECK_COUNTERS = false;

		private unsafe static readonly int PARTICLE_STRIDE = sizeof(Particle);

		private unsafe static readonly int EMITTERCONSTANTBUFFER_SIZE = sizeof(EmitterConstantBuffer);

		private unsafe static readonly int EMITTERDATA_SIZE = sizeof(MyGPUEmitterLayoutData);

		private static MyVertexShaders.Id m_vs = MyVertexShaders.Id.NULL;

		private static MyVertexShaders.Id m_vsShadowDebug = MyVertexShaders.Id.NULL;

		private static MyPixelShaders.Id m_ps = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psOIT = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psShadowDebugOIT = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psDebugUniformAccum = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psDebugUniformAccumOIT = MyPixelShaders.Id.NULL;

		private static MyComputeShaders.Id m_csSimulate = MyComputeShaders.Id.NULL;

		private static MyComputeShaders.Id m_csSimulationArgs = MyComputeShaders.Id.NULL;

		private static MyComputeShaders.Id m_csInitDeadList = MyComputeShaders.Id.NULL;

		private static MyComputeShaders.Id m_csEmit = MyComputeShaders.Id.NULL;

		private static MyComputeShaders.Id m_csEmitSkipFix = MyComputeShaders.Id.NULL;

		private static MyComputeShaders.Id m_csResetParticles = MyComputeShaders.Id.NULL;

		private static ISrvUavBuffer m_particleBuffer;

		private static IUavBuffer m_deadListBuffer;

		private static ISrvUavBuffer m_skippedParticleCountBuffer;

		private static IConstantBuffer m_activeListConstantBuffer;

		private static IConstantBuffer m_emitterConstantBuffer;

		private static ISrvBuffer m_emitterStructuredBuffer;

		private static ISrvUavBuffer m_aliveIndexBuffer1;

		private static ISrvUavBuffer m_aliveIndexBuffer2;

		private static IIndirectResourcesBuffer m_indirectDrawArgsBuffer;

		private static IIndirectResourcesBuffer m_indirectSimulateArgsBuffer;

		private static IIndexBuffer m_ib;

		private static IReadBuffer m_debugBuffer;

		private static IReadBuffer[] m_debugCounterBuffers = new IReadBuffer[2];

		private static int m_debugCounterBuffersIndex = 0;

		private static readonly MyGPUEmitterLayoutData[] m_emitterData = new MyGPUEmitterLayoutData[1024];

		private static ISrvBindable m_textureArraySrv;

		private static ISrvBindable m_emissiveArraySrv;

		private static int m_emitterCount;

		private static readonly uint[] m_tempBuffer = new uint[5];

<<<<<<< HEAD
=======
		private static int m_skipCounter;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private const int MAX_PARTICLE_EMIT_THREADS = 128;

		private const int MAX_EMITTERS = 8;

		internal static int Update(MyCullQuery cullQuery)
		{
			MyManagers.ParticleEffectsManager.Draw();
			return m_emitterCount = MyGPUEmitters.Gather(m_emitterData, out m_textureArraySrv, out m_emissiveArraySrv);
		}

		internal static void Run(MyRenderContext rc, ISrvBindable depthRead, ISrvBindable gbufferNormalsRead, bool immediateContext)
		{
			rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			rc.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			rc.SetInputLayout(null);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.ComputeShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.AllShaderStages.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBuffer);
			rc.ComputeShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			rc.VertexShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			rc.ComputeShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray);
			rc.VertexShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray);
			rc.VertexShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			if (m_resetSystem)
			{
				ResetInternal(rc);
				m_resetSystem = false;
			}
			ISrvUavBuffer aliveIndexBuffer = m_aliveIndexBuffer1;
			m_aliveIndexBuffer1 = m_aliveIndexBuffer2;
			m_aliveIndexBuffer2 = aliveIndexBuffer;
			rc.AllShaderStages.SetSrv(0, depthRead);
			rc.ComputeShader.SetSrv(1, m_emitterStructuredBuffer);
			rc.ComputeShader.SetSrv(2, gbufferNormalsRead);
			int num = 0;
			Emit(rc, m_emitterCount, m_emitterData);
			Simulate(rc);
			rc.CopyStructureCount(m_activeListConstantBuffer, 0, m_aliveIndexBuffer2);
			Render(rc, m_textureArraySrv, m_emissiveArraySrv);
			rc.ComputeShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
		}

		private static void InitDeadList(MyRenderContext rc)
		{
			rc.ComputeShader.Set(m_csInitDeadList);
			rc.ComputeShader.SetUav(0, m_deadListBuffer, 0);
			rc.Dispatch(Align(409600, 256) / 256, 1, 1);
		}

		private static void ResetInternal(MyRenderContext rc)
		{
			rc.ComputeShader.SetUav(0, m_particleBuffer);
			rc.ComputeShader.Set(m_csResetParticles);
			rc.Dispatch(Align(409600, 256) / 256, 1, 1);
			rc.ComputeShader.SetUav(0, m_deadListBuffer, 0);
			rc.ComputeShader.SetUav(0, m_aliveIndexBuffer1, 0);
			rc.ComputeShader.SetUav(0, m_aliveIndexBuffer2, 0);
			InitDeadList(rc);
		}

		private static void Emit(MyRenderContext rc, int emitterCount, MyGPUEmitterLayoutData[] emitterData)
		{
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_emitterStructuredBuffer);
			int num = 0;
			for (int i = 0; i < emitterCount; i++)
			{
				myMapping.WriteAndPosition(ref emitterData[i]);
				if (emitterData[i].NumParticlesToEmitThisFrame > num)
				{
					num = emitterData[i].NumParticlesToEmitThisFrame;
				}
			}
			myMapping.Unmap();
			int data = Align(num, 128) / 128;
			int threadGroupCountY = Align(emitterCount, 8) / 8;
			myMapping = MyMapping.MapDiscard(rc, m_emitterConstantBuffer);
			myMapping.WriteAndPosition(ref emitterCount);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			if (num > 0)
			{
				rc.ComputeShader.SetUav(0, m_particleBuffer);
				rc.ComputeShader.SetUav(1, m_deadListBuffer);
				rc.ComputeShader.SetUav(2, m_skippedParticleCountBuffer, 0);
				rc.ComputeShader.SetUav(3, m_aliveIndexBuffer1);
				rc.ComputeShader.SetConstantBuffer(1, m_emitterConstantBuffer);
				rc.ComputeShader.Set(m_csEmit);
				rc.Dispatch(data, threadGroupCountY, 1);
				rc.ComputeShader.SetConstantBuffer(1, null);
				rc.ComputeShader.Set(m_csEmitSkipFix);
				rc.Dispatch(1, 1, 1);
				rc.ComputeShader.SetUav(0, null);
				rc.ComputeShader.SetUav(1, null);
				rc.ComputeShader.SetUav(2, null);
				rc.ComputeShader.SetUav(3, null);
			}
		}

		private static void Simulate(MyRenderContext rc)
		{
			rc.ClearUav(m_indirectSimulateArgsBuffer, new RawInt4(1, 1, 1, 1));
			rc.CopyStructureCount(m_indirectSimulateArgsBuffer, 0, m_aliveIndexBuffer1);
			rc.ComputeShader.SetUav(2, m_indirectDrawArgsBuffer);
			rc.ComputeShader.SetUav(3, m_indirectSimulateArgsBuffer);
			rc.ComputeShader.Set(m_csSimulationArgs);
			rc.Dispatch(1, 1, 1);
			rc.ComputeShader.SetUav(0, m_particleBuffer);
			rc.ComputeShader.SetUav(1, m_deadListBuffer);
			rc.ComputeShader.SetUav(2, m_aliveIndexBuffer2, 0);
			rc.ComputeShader.SetUav(3, m_indirectDrawArgsBuffer);
			rc.ComputeShader.SetUav(4, m_aliveIndexBuffer1);
			rc.ComputeShader.Set(m_csSimulate);
			rc.DispatchIndirect(m_indirectSimulateArgsBuffer, 0);
			rc.ComputeShader.SetUav(0, null);
			rc.ComputeShader.SetUav(1, null);
			rc.ComputeShader.SetUav(2, null);
			rc.ComputeShader.SetUav(3, null);
			rc.ComputeShader.SetUav(4, null);
		}

		private static void Render(MyRenderContext rc, ISrvBindable textureArraySrv, ISrvBindable emissiveArraySrv)
		{
			MyVRage.Platform.Render.FlushIndirectArgsFromComputeShader(rc.DeviceContext);
			rc.VertexShader.Set(MyRender11.Settings.DisplayParticleShadowSplitsWithDebug ? m_vsShadowDebug : m_vs);
			if (MyTransparentRendering.DisplayTransparencyHeatMap())
			{
				rc.PixelShader.Set(MyRender11.DebugOverrides.OIT ? m_psDebugUniformAccumOIT : m_psDebugUniformAccum);
			}
			else
			{
				rc.PixelShader.Set(MyRender11.Settings.DisplayParticleShadowSplitsWithDebug ? m_psShadowDebugOIT : (MyRender11.DebugOverrides.OIT ? m_psOIT : m_ps));
			}
			rc.SetVertexBuffer(0, null);
			rc.SetIndexBuffer(m_ib);
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			rc.AllShaderStages.SetConstantBuffer(1, m_activeListConstantBuffer);
			rc.PixelShader.SetSrvs(1, textureArraySrv);
			rc.PixelShader.SetSrvs(3, emissiveArraySrv);
			rc.VertexShader.SetSrv(0, m_particleBuffer);
			rc.VertexShader.SetSrv(1, m_emitterStructuredBuffer);
			rc.VertexShader.SetSrv(2, m_aliveIndexBuffer2);
			rc.VertexShader.SetSrv(11, MyManagers.EnvironmentProbe.CloseCubemapFinal);
			if (!MyStereoRender.Enable)
			{
				rc.DrawIndexedInstancedIndirect(m_indirectDrawArgsBuffer, 0);
			}
			else
			{
				MyStereoRender.DrawIndexedInstancedIndirectGPUParticles(rc, m_indirectDrawArgsBuffer, 0);
			}
			rc.VertexShader.SetSrv(11, null);
			rc.AllShaderStages.SetSrv(0, null);
		}

		internal static void Init()
		{
			MyGPUEmitters.Init();
			m_resetSystem = true;
			m_csInitDeadList = MyComputeShaders.Create("Transparent/GPUParticles/InitDeadList.hlsl");
			m_csResetParticles = MyComputeShaders.Create("Transparent/GPUParticles/Reset.hlsl");
			m_csEmit = MyComputeShaders.Create("Transparent/GPUParticles/Emit.hlsl");
			m_csEmitSkipFix = MyComputeShaders.Create("Transparent/GPUParticles/EmitSkipFix.hlsl");
			m_csSimulate = MyComputeShaders.Create("Transparent/GPUParticles/Simulation.hlsl");
			m_csSimulationArgs = MyComputeShaders.Create("Transparent/GPUParticles/SimulationArgs.hlsl");
			ShaderMacro[] array = new ShaderMacro[2]
			{
				new ShaderMacro("STREAKS", null),
				new ShaderMacro("LIT_PARTICLE", null)
			};
			ShaderMacro[] array2 = new ShaderMacro[3]
			{
				new ShaderMacro("STREAKS", null),
				new ShaderMacro("LIT_PARTICLE", null),
				new ShaderMacro("OIT", null)
			};
			ShaderMacro[] macros = new ShaderMacro[4]
			{
				new ShaderMacro("STREAKS", null),
				new ShaderMacro("LIT_PARTICLE", null),
				new ShaderMacro("OIT", null),
				new ShaderMacro("DEBUG_SHADOWS", null)
			};
			m_vs = MyVertexShaders.Create("Transparent/GPUParticles/Render.hlsl", array);
			m_vsShadowDebug = MyVertexShaders.Create("Transparent/GPUParticles/Render.hlsl", macros);
			m_ps = MyPixelShaders.Create("Transparent/GPUParticles/Render.hlsl", array);
			m_psOIT = MyPixelShaders.Create("Transparent/GPUParticles/Render.hlsl", array2);
			m_psShadowDebugOIT = MyPixelShaders.Create("Transparent/GPUParticles/Render.hlsl", macros);
			ShaderMacro[] sm = new ShaderMacro[1]
			{
				new ShaderMacro("DEBUG_UNIFORM_ACCUM", null)
			};
			m_psDebugUniformAccum = MyPixelShaders.Create("Transparent/GPUParticles/Render.hlsl", MyShaderCompiler.ConcatenateMacros(array, sm));
			m_psDebugUniformAccumOIT = MyPixelShaders.Create("Transparent/GPUParticles/Render.hlsl", MyShaderCompiler.ConcatenateMacros(array2, sm));
			InitDevice();
		}

		private unsafe static void InitDevice()
		{
			m_particleBuffer = MyManagers.Buffers.CreateSrvUav("MyGPUParticleRenderer::particleBuffer", 409600, PARTICLE_STRIDE, null, MyUavType.Default, ResourceUsage.Default, isGlobal: true);
			m_deadListBuffer = MyManagers.Buffers.CreateUav("MyGPUParticleRenderer::deadListBuffer", 409600, 4, null, MyUavType.Append, ResourceUsage.Default, isGlobal: true);
			m_skippedParticleCountBuffer = MyManagers.Buffers.CreateSrvUav("MyGPUParticleRenderer::skippedParticleCountBuffer", 1, 4, null, MyUavType.Counter, ResourceUsage.Default, isGlobal: true);
			m_debugCounterBuffers[0] = MyManagers.Buffers.CreateRead("MyGPUParticleRenderer::debugCounterBuffers[0]", 4, 4, isGlobal: true);
			m_debugCounterBuffers[1] = MyManagers.Buffers.CreateRead("MyGPUParticleRenderer::debugCounterBuffers[1]", 4, 4, isGlobal: true);
			m_debugBuffer = MyManagers.Buffers.CreateRead("MyGPUParticleRenderer::debugBuffer", 5, 4, isGlobal: true);
			m_activeListConstantBuffer = MyManagers.Buffers.CreateConstantBuffer("MyGPUParticleRenderer::activeListConstantBuffer", 16, null, ResourceUsage.Default, isGlobal: true);
			m_emitterConstantBuffer = MyManagers.Buffers.CreateConstantBuffer("MyGPUParticleRenderer::emitterConstantBuffer", EMITTERCONSTANTBUFFER_SIZE, null, ResourceUsage.Dynamic, isGlobal: true);
			m_emitterStructuredBuffer = MyManagers.Buffers.CreateSrv("MyGPUParticleRenderer::emitterStructuredBuffer", 1024, EMITTERDATA_SIZE, null, ResourceUsage.Dynamic, isGlobal: true);
			m_aliveIndexBuffer1 = MyManagers.Buffers.CreateSrvUav("MyGPUParticleRenderer::aliveIndexBuffer1", 409600, 4, null, MyUavType.Counter, ResourceUsage.Default, isGlobal: true);
			m_aliveIndexBuffer2 = MyManagers.Buffers.CreateSrvUav("MyGPUParticleRenderer::aliveIndexBuffer2", 409600, 4, null, MyUavType.Counter, ResourceUsage.Default, isGlobal: true);
			m_indirectDrawArgsBuffer = MyManagers.Buffers.CreateIndirectArgsBuffer("MyGPUParticleRenderer::indirectDrawArgsBuffer", 5, 4, Format.R32_UInt, isGlobal: true);
			m_indirectSimulateArgsBuffer = MyManagers.Buffers.CreateIndirectArgsBuffer("MyGPUParticleRenderer::indirectSimulateArgsBuffer", 4, 4, Format.R32_UInt, isGlobal: true);
			uint[] array = new uint[2457600];
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			for (; num < 409600; num++)
			{
				array[num2] = num3;
				array[num2 + 1] = num3 + 1;
				array[num2 + 2] = num3 + 2;
				array[num2 + 3] = num3 + 2;
				array[num2 + 4] = num3 + 1;
				array[num2 + 5] = num3 + 3;
				num3 += 4;
				num2 += 6;
			}
			fixed (uint* value = array)
			{
				m_ib = MyManagers.Buffers.CreateIndexBuffer("MyGPUParticleRenderer::indexBuffer", 2457600, new IntPtr(value), MyIndexBufferFormat.UInt, ResourceUsage.Immutable, isGlobal: true);
			}
		}

		internal static void OnDeviceReset()
		{
			DoneDevice();
		}

		private static void DoneDevice()
		{
			MyManagers.Buffers.Dispose(m_ib);
			m_ib = null;
			MyManagers.Buffers.Dispose(m_activeListConstantBuffer);
			m_activeListConstantBuffer = null;
			MyManagers.Buffers.Dispose(m_indirectDrawArgsBuffer);
			m_indirectDrawArgsBuffer = null;
			MyManagers.Buffers.Dispose(m_indirectSimulateArgsBuffer);
			m_indirectSimulateArgsBuffer = null;
			MyManagers.Buffers.Dispose(m_debugCounterBuffers);
			m_debugCounterBuffers = new IReadBuffer[m_debugCounterBuffers.Length];
			MyManagers.Buffers.Dispose(m_debugBuffer);
			MyManagers.Buffers.Dispose(m_aliveIndexBuffer1);
			m_aliveIndexBuffer1 = null;
			MyManagers.Buffers.Dispose(m_aliveIndexBuffer2);
			m_aliveIndexBuffer2 = null;
			MyManagers.Buffers.Dispose(m_deadListBuffer);
			m_deadListBuffer = null;
			MyManagers.Buffers.Dispose(m_skippedParticleCountBuffer);
			m_skippedParticleCountBuffer = null;
			MyManagers.Buffers.Dispose(m_particleBuffer);
			m_particleBuffer = null;
			MyManagers.Buffers.Dispose(m_emitterConstantBuffer);
			m_emitterConstantBuffer = null;
			MyManagers.Buffers.Dispose(m_emitterStructuredBuffer);
			m_emitterStructuredBuffer = null;
		}

		internal static void OnDeviceEnd()
		{
			MyGPUEmitters.OnDeviceEnd();
			DoneDevice();
			m_ps = MyPixelShaders.Id.NULL;
			m_psOIT = MyPixelShaders.Id.NULL;
			m_psShadowDebugOIT = MyPixelShaders.Id.NULL;
			m_vs = MyVertexShaders.Id.NULL;
			m_csSimulate = MyComputeShaders.Id.NULL;
			m_csSimulationArgs = MyComputeShaders.Id.NULL;
			m_csResetParticles = MyComputeShaders.Id.NULL;
			m_csInitDeadList = MyComputeShaders.Id.NULL;
			m_csEmit = MyComputeShaders.Id.NULL;
			m_csEmitSkipFix = MyComputeShaders.Id.NULL;
			m_resetSystem = true;
		}

		internal static void OnSessionEnd()
		{
			MyGPUEmitters.OnSessionEnd();
			m_resetSystem = true;
		}

		internal static void Reset()
		{
			m_resetSystem = true;
		}

		private static int ReadCounter(MyRenderContext rc, IUavBindable uav)
		{
			return 0;
		}

		private static int ReadCounterImmediate(MyRenderContext rc, IUavBindable uav)
		{
			return 0;
		}

		private static void ReadDataImmediate(MyRenderContext rc, IUavBindable uav, uint[] buffer)
		{
		}

		private static int Align(int value, int alignment)
		{
			return (value + (alignment - 1)) & ~(alignment - 1);
		}
	}
}
