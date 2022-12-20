using System;
using System.Runtime.InteropServices;
using System.Threading;
using ParallelTasks;
using VRage.Library.Memory;
using VRage.NativeWrapper;
using VRageMath;

namespace VRage.Voxels.Sewing
{
	public class VrSewGuide : PointerHandle
	{
		private static readonly MyMemorySystem m_memorySystem = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Voxels-Native");

		private VrVoxelMesh m_mesh;

		private int m_referenceCount;

		private MyMemorySystem.AllocationRecord? m_allocationRecord;

		public VrVoxelMesh Mesh => m_mesh;

		public int Lod => VrSewGuide_GetLod(base.NativeObject);

		public float Scale => VrSewGuide_GetScale(base.NativeObject);

		public Vector3I Start => VrSewGuide_GetStart(base.NativeObject);

		public Vector3I End => VrSewGuide_GetEnd(base.NativeObject);

		public Vector3I Size => VrSewGuide_GetSize(base.NativeObject);

		public bool Sewn => VrSewGuide_GetSewn(base.NativeObject);

		public int ReferenceCount => m_referenceCount;

		public int Version => VrSewGuide_GetVersion(m_handle);

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrSewGuide_Create1(int lod, Vector3I min, Vector3I max, IntPtr dataCache);

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrSewGuide_Create2(IntPtr mesh, IntPtr dataCache);

		[DllImport("VRage.Native.dll")]
		public static extern void VrSewGuide_Release(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern int VrSewGuide_GetInMemorySize(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern int VrSewGuide_GetLod(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern float VrSewGuide_GetScale(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern Vector3I VrSewGuide_GetStart(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern Vector3I VrSewGuide_GetEnd(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern Vector3I VrSewGuide_GetSize(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern bool VrSewGuide_GetSewn(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrSewGuide_Reset(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrSewGuide_SetMesh(IntPtr instance, IntPtr mesh, IntPtr dataCache);

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrSewGuide_GetMesh(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrSewGuide_InvalidateGenerated(IntPtr instance, Vector3I minRange);

		[DllImport("VRage.Native.dll")]
		public static extern int VrSewGuide_GetVersion(IntPtr instance);

		public VrSewGuide(int lod, Vector3I min, Vector3I max, VrShellDataCache dataCache)
			: base(VrSewGuide_Create1(lod, min, max, dataCache.NativeObject))
		{
			m_mesh = null;
			m_referenceCount = 1;
		}

		public VrSewGuide(VrVoxelMesh mesh, VrShellDataCache dataCache)
			: base(VrSewGuide_Create2(mesh.NativeObject, dataCache.NativeObject))
		{
			m_mesh = mesh;
			m_referenceCount = 1;
			UpdateMemoryStats();
		}

		public void Reset()
		{
			VrSewGuide_Reset(base.NativeObject);
		}

		public void SetMesh(VrVoxelMesh mesh, VrShellDataCache dataCache)
		{
			if (m_mesh != mesh)
			{
				m_mesh?.Dispose();
			}
			m_mesh = mesh;
			VrSewGuide_SetMesh(base.NativeObject, mesh?.NativeObject ?? IntPtr.Zero, dataCache.NativeObject);
			UpdateMemoryStats();
		}

		public void InvalidateGenerated(Vector3I minRange)
		{
			VrSewGuide_InvalidateGenerated(base.NativeObject, minRange);
			UpdateMemoryStats();
		}

		internal void CheckCreatedMesh()
		{
			IntPtr intPtr = VrSewGuide_GetMesh(m_handle);
			if (intPtr != IntPtr.Zero)
			{
				m_mesh = new VrVoxelMesh(intPtr);
			}
		}

		public void AddReference()
		{
			Interlocked.Increment(ref m_referenceCount);
			_ = 1;
		}

		public bool RemoveReference()
		{
			if (Interlocked.Decrement(ref m_referenceCount) == 0)
			{
				Dispose();
				return true;
			}
			_ = 0;
			return false;
		}

		public void UpdateMemoryStats()
		{
			m_allocationRecord?.Dispose();
			m_allocationRecord = m_memorySystem.RegisterAllocation("VrSewGuide" + base.NativeObject.ToInt64(), VrSewGuide_GetInMemorySize(base.NativeObject));
		}

		protected override void Dispose(bool disposing)
		{
			m_allocationRecord?.Dispose();
			m_allocationRecord = null;
			VrSewGuide_Release(base.NativeObject);
			m_mesh?.Dispose();
		}

		public override string ToString()
		{
			return $"{Start}:{End}:{Lod}";
		}
	}
}
