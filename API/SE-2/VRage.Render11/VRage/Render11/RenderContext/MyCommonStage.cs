using System;
using SharpDX.Direct3D11;
using VRage.Render11.Resources;

namespace VRage.Render11.RenderContext
{
	internal class MyCommonStage
	{
		protected DeviceContext m_deviceContext;

		protected CommonShaderStage m_shaderStage;

		protected MyRenderContextStatistics m_statistics;

		private IConstantBuffer[] m_constantBuffers = new IConstantBuffer[8];

		private SamplerState[] m_samplers = new SamplerState[16];

		private ShaderResourceView[] m_srvs = new ShaderResourceView[32];

		[ThreadStatic]
		private static SamplerState[] m_tmpSamplers;

		[ThreadStatic]
		private static ShaderResourceView[] m_tmpSrvs;

		internal void Init(DeviceContext context, CommonShaderStage shaderStage, MyRenderContextStatistics statistics)
		{
			m_deviceContext = context;
			m_shaderStage = shaderStage;
			m_statistics = statistics;
		}

		internal virtual void ClearState()
		{
			for (int i = 0; i < m_constantBuffers.Length; i++)
			{
				m_constantBuffers[i] = null;
			}
			for (int j = 0; j < m_samplers.Length; j++)
			{
				m_samplers[j] = null;
			}
			for (int k = 0; k < m_srvs.Length; k++)
			{
				m_srvs[k] = null;
			}
		}

		internal void SetConstantBuffer(int slot, IConstantBuffer constantBuffer)
		{
			SharpDX.Direct3D11.Buffer constantBuffer2 = null;
			if (constantBuffer != null)
			{
				constantBuffer2 = constantBuffer.Buffer;
			}
			if (constantBuffer != m_constantBuffers[slot])
			{
				m_constantBuffers[slot] = constantBuffer;
				m_shaderStage.SetConstantBuffer(slot, constantBuffer2);
				m_statistics.SetConstantBuffers++;
			}
		}

		internal void SetSampler(int slot, ISamplerState sampler)
		{
			SamplerState samplerState = null;
			if (sampler != null)
			{
				samplerState = ((ISamplerStateInternal)sampler).Resource;
			}
			if (samplerState != m_samplers[slot])
			{
				m_samplers[slot] = samplerState;
				m_shaderStage.SetSampler(slot, samplerState);
				m_statistics.SetSamplers++;
			}
		}

		internal void SetSamplers(int startSlot, params ISamplerState[] samplers)
		{
			if (m_tmpSamplers == null)
			{
				m_tmpSamplers = new SamplerState[16];
			}
			for (int i = 0; i < samplers.Length; i++)
			{
				ISamplerState samplerState = samplers[i];
				SamplerState samplerState2 = null;
				if (samplerState != null)
				{
					samplerState2 = ((ISamplerStateInternal)samplerState).Resource;
				}
				int num = startSlot + i;
				m_samplers[num] = samplerState2;
				m_tmpSamplers[i] = samplerState2;
			}
			m_shaderStage.SetSamplers(startSlot, samplers.Length, m_tmpSamplers);
			m_statistics.SetSamplers++;
		}

		internal void SetSrv(int slot, ISrvBindable srvBind)
		{
			ShaderResourceView shaderResourceView = null;
			if (srvBind != null)
			{
				shaderResourceView = srvBind.Srv;
			}
			if (shaderResourceView != m_srvs[slot])
			{
				m_srvs[slot] = shaderResourceView;
				m_shaderStage.SetShaderResource(slot, shaderResourceView);
				m_statistics.SetSrvs++;
			}
		}

		internal void SetSrvs(int startSlot, params ISrvBindable[] srvs)
		{
			if (m_tmpSrvs == null)
			{
				m_tmpSrvs = new ShaderResourceView[12];
			}
			bool flag = false;
			for (int i = 0; i < srvs.Length; i++)
			{
				ShaderResourceView shaderResourceView = srvs[i]?.Srv;
				int num = startSlot + i;
				if (shaderResourceView != m_srvs[num])
				{
					flag = true;
				}
				m_srvs[num] = shaderResourceView;
				m_tmpSrvs[i] = shaderResourceView;
			}
			if (flag)
			{
				m_shaderStage.SetShaderResources(startSlot, srvs.Length, m_tmpSrvs);
				m_statistics.SetSrvs++;
			}
		}

		internal void SetSrvs(int startSlot, MyGBuffer gbuffer, MyGBufferSrvFilter mode = MyGBufferSrvFilter.ALL)
		{
			ISrvBindable srvBindable = null;
			if (mode == MyGBufferSrvFilter.ALL)
			{
				srvBindable = gbuffer.DepthStencil.SrvStencil;
			}
			SetSrvs(0, gbuffer.DepthStencil.SrvDepth, gbuffer.GBuffer0, gbuffer.GBuffer1, gbuffer.GBuffer2, srvBindable);
		}

		internal void ResetSrvs(int startSlot, MyGBufferSrvFilter mode)
		{
			if (mode == MyGBufferSrvFilter.ALL)
			{
				SetSrvs(startSlot, null, null, null, null, null);
			}
			else
			{
				SetSrvs(startSlot, null, null, null, null);
			}
		}
	}
}
