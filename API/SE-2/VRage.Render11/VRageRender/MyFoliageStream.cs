using System;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyFoliageStream : IDisposable
	{
		private int m_allocationSize;

		internal bool Append;

		internal IVertexBuffer Stream { get; private set; }

		internal void Reset()
		{
			m_allocationSize = 0;
			Append = false;
			Dispose();
		}

		internal void Reserve(int x)
		{
			m_allocationSize += x;
		}

		internal void AllocateStreamOutBuffer()
		{
			if (m_allocationSize <= 0)
			{
				Dispose();
				return;
			}
			if (Stream != null)
			{
				if (m_allocationSize < Stream.ElementCount)
				{
					return;
				}
				Dispose();
			}
			m_allocationSize = Math.Min(4194304, m_allocationSize);
			Stream = MyManagers.FoliageManager.FoliagePool.Get(MathHelper.GetNearestBiggerPowerOfTwo(m_allocationSize));
			Append = false;
		}

		public void Dispose()
		{
			if (Stream != null)
			{
				MyManagers.FoliageManager.FoliagePool.Return(Stream);
				Stream = null;
			}
		}
	}
}
