using SharpDX.Direct3D11;
using VRage.Render11.Resources;

namespace VRage.Render11.RenderContext
{
	internal class MyComputeStage : MyCommonStage
	{
		private ComputeShader m_computeShader;

		private readonly UnorderedAccessView[] m_uavs = new UnorderedAccessView[5];

		private readonly int[] m_uavsInitialCount = new int[5];

		internal override void ClearState()
		{
			base.ClearState();
			m_computeShader = null;
		}

		internal void Set(ComputeShader shader)
		{
			if (shader != m_computeShader)
			{
				m_computeShader = shader;
				m_deviceContext.ComputeShader.Set(shader);
				m_statistics.SetComputeShaders++;
			}
		}

		internal void SetUav(int slot, IUavBindable uavBindable)
		{
			UnorderedAccessView unorderedAccessView = null;
			if (uavBindable != null)
			{
				unorderedAccessView = uavBindable.Uav;
			}
			if (unorderedAccessView != m_uavs[slot])
			{
				m_uavs[slot] = unorderedAccessView;
				m_deviceContext.ComputeShader.SetUnorderedAccessView(slot, unorderedAccessView);
				m_statistics.SetUavs++;
			}
		}

		internal void SetUav(int slot, IUavBindable uavBindable, int uavInitialCount)
		{
			UnorderedAccessView unorderedAccessView = null;
			if (uavBindable != null)
			{
				unorderedAccessView = uavBindable.Uav;
			}
			if (unorderedAccessView != m_uavs[slot] || uavInitialCount != m_uavsInitialCount[slot])
			{
				m_uavs[slot] = unorderedAccessView;
				m_uavsInitialCount[slot] = uavInitialCount;
				m_deviceContext.ComputeShader.SetUnorderedAccessView(slot, unorderedAccessView, uavInitialCount);
				m_statistics.SetUavs++;
			}
		}

		internal void SetUavs(int startSlot, params IUavBindable[] uavs)
		{
			for (int i = 0; i < uavs.Length; i++)
			{
				UnorderedAccessView unorderedAccessView = null;
				if (uavs[i] != null)
				{
					unorderedAccessView = uavs[i].Uav;
				}
				int num = startSlot + i;
				if (unorderedAccessView != m_uavs[num])
				{
					m_uavs[num] = unorderedAccessView;
					m_deviceContext.ComputeShader.SetUnorderedAccessView(startSlot, unorderedAccessView);
					m_statistics.SetUavs++;
				}
			}
		}
	}
}
