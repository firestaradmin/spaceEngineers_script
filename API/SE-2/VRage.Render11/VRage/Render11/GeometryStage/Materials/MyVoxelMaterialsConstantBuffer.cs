using System;
using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.GeometryStage.Materials
{
	internal class MyVoxelMaterialsConstantBuffer
	{
		public const int MAX_ENTRIES = 255;

		private readonly MyVoxelMaterialEntry[] m_entries;

		private int m_maxId;

		private bool m_cbDirty;

		public IConstantBuffer Cb { get; private set; }

		public unsafe MyVoxelMaterialsConstantBuffer()
		{
			Cb = MyManagers.Buffers.CreateConstantBuffer("VoxelMaterialConstants", sizeof(MyVoxelMaterialEntry) * 255, null, ResourceUsage.Dynamic, isGlobal: true);
			m_entries = new MyVoxelMaterialEntry[255];
		}

		internal void OnDeviceEnd()
		{
			MyManagers.Buffers.Dispose(Cb);
		}

		public void UpdateEntry(int voxelMaterialId, ref MyVoxelMaterialEntry entry)
		{
			m_maxId = Math.Max(voxelMaterialId + 1, m_maxId);
			m_entries[voxelMaterialId] = entry;
			m_cbDirty = true;
		}

		public void FeedGPU()
		{
			if (m_cbDirty)
			{
				MyMapping myMapping = MyMapping.MapDiscard(Cb);
				for (int i = 0; i < m_maxId; i++)
				{
					myMapping.WriteAndPosition(ref m_entries[i]);
				}
				myMapping.Unmap();
			}
		}
	}
}
