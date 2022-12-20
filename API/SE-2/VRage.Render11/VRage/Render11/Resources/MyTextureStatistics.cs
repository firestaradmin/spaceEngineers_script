using System;
using System.Collections.Generic;
using VRage.Library.Memory;

namespace VRage.Render11.Resources
{
	internal struct MyTextureStatistics
	{
		private Dictionary<ITexture, MyMemorySystem.AllocationRecord> m_allocationRecords;

		public int TexturesTotal { get; private set; }

		public long TotalTextureMemory { get; private set; }

		public int TexturesTotalPeak { get; private set; }

		public long TotalTextureMemoryPeak { get; private set; }

		private MyMemorySystem m_allocationTracker { get; }

		public MyTextureStatistics(MyMemorySystem allocationTracker)
		{
			this = default(MyTextureStatistics);
			m_allocationTracker = allocationTracker;
			m_allocationRecords = new Dictionary<ITexture, MyMemorySystem.AllocationRecord>();
		}

		public void Add(ITexture texture)
		{
			long textureByteSize = texture.GetTextureByteSize();
			lock (m_allocationRecords)
			{
				TexturesTotal++;
				TotalTextureMemory += textureByteSize;
				TexturesTotalPeak = Math.Max(TexturesTotalPeak, TexturesTotal);
				TotalTextureMemoryPeak = Math.Max(TotalTextureMemoryPeak, TotalTextureMemory);
				m_allocationRecords[texture] = m_allocationTracker.RegisterAllocation(texture.Name, textureByteSize);
			}
		}

		public void Remove(ITexture texture)
		{
			long textureByteSize = texture.GetTextureByteSize();
			lock (m_allocationRecords)
			{
				TexturesTotal--;
				TotalTextureMemory -= textureByteSize;
				if (m_allocationRecords.TryGetValue(texture, out var value))
				{
					value.Dispose();
					m_allocationRecords.Remove(texture);
				}
			}
		}
	}
}
