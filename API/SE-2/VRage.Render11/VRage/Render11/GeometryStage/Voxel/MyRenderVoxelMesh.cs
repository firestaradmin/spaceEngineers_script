using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Render11.GeometryStage.Voxel
{
	internal struct MyRenderVoxelMesh
	{
		private readonly MeshId m_mesh;

		private BoundingBox m_aabb;

		public float LodScale { get; private set; }

		public MeshId Mesh => m_mesh;

		public BoundingBox Aabb => m_aabb;

		/// <summary>
		/// Whether this mesh is trully ready to use.
		/// </summary>
		public bool Ready { get; private set; }

		/// <summary>
		/// Whether the data in the device is out of date.
		/// </summary>
		public bool DeviceDirty { get; private set; }

		public MyRenderVoxelMesh(Vector3I offset, int lod)
		{
			this = default(MyRenderVoxelMesh);
			m_mesh = MyMeshes.CreateVoxelCell(offset, lod);
			LodScale = 1 << lod;
			m_aabb = BoundingBox.CreateInvalid();
			Ready = false;
		}

		public void Destroy()
		{
			MyMeshes.RemoveVoxelCell(m_mesh);
			Ready = false;
		}

		public void UpdateCell(ref MyVoxelRenderCellData data, IMyVoxelUpdateBatch updateBatch = null)
		{
			m_aabb = data.CellBounds;
			MyMeshes.UpdateVoxelCell(m_mesh, ref data, updateBatch);
			Ready = true;
			DeviceDirty = updateBatch == null;
		}

		public void UpdateDevice()
		{
			MyMeshes.UpdateVoxelCellOnDevice(m_mesh);
			DeviceDirty = false;
		}
	}
}
