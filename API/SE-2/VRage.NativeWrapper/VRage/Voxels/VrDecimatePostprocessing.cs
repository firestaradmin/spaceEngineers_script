using System;
using System.Runtime.InteropServices;
using VRageMath;

namespace VRage.Voxels
{
	public class VrDecimatePostprocessing : VrPostprocessing
	{
		public float FeatureAngle
		{
			get
			{
				return VrDecimatePostprocessing_GetFeatureAngle(m_handle);
			}
			set
			{
				VrDecimatePostprocessing_SetFeatureAngle(m_handle, value);
			}
		}

		public float EdgeThreshold
		{
			get
			{
				return VrDecimatePostprocessing_GetEdgeThreshold(m_handle);
			}
			set
			{
				VrDecimatePostprocessing_SetEdgeThreshold(m_handle, value);
			}
		}

		public float PlaneThreshold
		{
			get
			{
				return VrDecimatePostprocessing_GetPlaneThreshold(m_handle);
			}
			set
			{
				VrDecimatePostprocessing_SetPlaneThreshold(m_handle, value);
			}
		}

		public bool IgnoreEdges
		{
			get
			{
				return VrDecimatePostprocessing_GetIgnoreEdges(m_handle);
			}
			set
			{
				VrDecimatePostprocessing_SetIgnoreEdges(m_handle, value);
			}
		}

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrDecimatePostprocessing_Create();

		[DllImport("VRage.Native.dll")]
		public static extern void VrDecimatePostprocessing_Release(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern float VrDecimatePostprocessing_GetFeatureAngle(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrDecimatePostprocessing_SetFeatureAngle(IntPtr instance, float value);

		[DllImport("VRage.Native.dll")]
		public static extern float VrDecimatePostprocessing_GetEdgeThreshold(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrDecimatePostprocessing_SetEdgeThreshold(IntPtr instance, float value);

		[DllImport("VRage.Native.dll")]
		public static extern float VrDecimatePostprocessing_GetPlaneThreshold(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrDecimatePostprocessing_SetPlaneThreshold(IntPtr instance, float value);

		[DllImport("VRage.Native.dll")]
		public static extern bool VrDecimatePostprocessing_GetIgnoreEdges(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrDecimatePostprocessing_SetIgnoreEdges(IntPtr instance, bool value);

		[DllImport("VRage.Native.dll")]
		public static extern void VrDecimatePostprocessing_GetClassification(IntPtr instance, IntPtr mesh, IntPtr target, int count);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrDecimatePostprocessing_GetClassificationDetails(IntPtr instance, int vertex, out ushort* features, out ushort featureCount, out ushort* loop, out ushort loopSize, out float characteristicDistance);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern bool VrDecimatePostprocessing_TrySimplify(IntPtr instance, int vertex, out ushort* triangles, out ushort triangleCount, out Vector3 normal);

		[DllImport("VRage.Native.dll")]
		public static extern bool VrDecimatePostprocessing_Simplify(IntPtr instance, int vertex);

		public VrDecimatePostprocessing()
			: base(VrDecimatePostprocessing_Create())
		{
		}

		public unsafe void GetClassification(VrVoxelMesh mesh, VrVertexClassification[] target)
		{
			fixed (byte* ptr = new byte[target.Length])
			{
				VrDecimatePostprocessing_GetClassification(base.NativeObject, mesh.NativeObject, new IntPtr(ptr), target.Length);
				for (int i = 0; i < target.Length; i++)
				{
					target[i] = (VrVertexClassification)ptr[i];
				}
			}
		}

		public unsafe void GetClassificationDetails(int vertex, out ushort* features, out ushort featureCount, out ushort* loop, out ushort loopSize, out float characteristicDistance)
		{
			VrDecimatePostprocessing_GetClassificationDetails(base.NativeObject, vertex, out features, out featureCount, out loop, out loopSize, out characteristicDistance);
		}

		public unsafe bool TrySimplify(int vertex, out ushort* triangles, out ushort triangleCount, out Vector3 normal)
		{
			return VrDecimatePostprocessing_TrySimplify(base.NativeObject, vertex, out triangles, out triangleCount, out normal);
		}

		public bool Simplify(int vertex)
		{
			return VrDecimatePostprocessing_Simplify(base.NativeObject, vertex);
		}

		public override IntPtr GetStep()
		{
			return base.NativeObject;
		}

		protected override void Dispose(bool disposing)
		{
			VrDecimatePostprocessing_Release(base.NativeObject);
		}
	}
}
