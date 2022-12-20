using System;
using System.Runtime.InteropServices;
using VRage.NativeWrapper;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Voxels
{
	public class VrVoxelMesh : PointerHandle
	{
		public int VertexCount
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetVertexCount(base.NativeObject);
			}
		}

		public unsafe VrVoxelVertex* Vertices
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetVertices(base.NativeObject);
			}
		}

		public int TriangleCount
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetTriangleCount(base.NativeObject);
			}
		}

		public unsafe VrVoxelTriangle* Triangles
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetTriangles(base.NativeObject);
			}
		}

		public int Lod
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetLod(base.NativeObject);
			}
		}

		public float Scale
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetScale(base.NativeObject);
			}
		}

		public Vector3I Start
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetStart(base.NativeObject);
			}
		}

		public Vector3I End
		{
			get
			{
				DisposeCheck();
				return VrVoxelMesh_GetEnd(base.NativeObject);
			}
		}

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrVoxelMesh_Create(Vector3I start, Vector3I end, int lod);

		[DllImport("VRage.Native.dll")]
		public static extern void VrVoxelMesh_Release(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern int VrVoxelMesh_GetVertexCount(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern VrVoxelVertex* VrVoxelMesh_GetVertices(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern int VrVoxelMesh_GetTriangleCount(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern VrVoxelTriangle* VrVoxelMesh_GetTriangles(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern int VrVoxelMesh_GetLod(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern float VrVoxelMesh_GetScale(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern Vector3I VrVoxelMesh_GetStart(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern Vector3I VrVoxelMesh_GetEnd(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrVoxelMesh_GetMeshData(IntPtr instance, Vector3* positions, Vector3* normals, byte* materials, Vector3I* cells, Byte4* color, VrVoxelTriangle* triangles);

		[DllImport("VRage.Native.dll")]
		public static extern void VrVoxelMesh_Clear(IntPtr instance);

		public VrVoxelMesh(Vector3I start, Vector3I end, int lod)
			: this(VrVoxelMesh_Create(start, end, lod))
		{
		}

		public unsafe void GetMeshData(Vector3* positions, Vector3* normals, byte* materials, Vector3I* cells, Byte4* color, VrVoxelTriangle* triangles)
		{
			DisposeCheck();
			VrVoxelMesh_GetMeshData(base.NativeObject, positions, normals, materials, cells, color, triangles);
		}

		public void Clear()
		{
			DisposeCheck();
			VrVoxelMesh_Clear(base.NativeObject);
		}

		internal VrVoxelMesh(IntPtr nativeMesh)
			: base(nativeMesh)
		{
		}

		protected override void Dispose(bool disposing)
		{
			VrVoxelMesh_Release(base.NativeObject);
		}

		private void DisposeCheck()
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException("VrVoxelMesh has been disposed already");
			}
		}
	}
}
