using System;
using System.Runtime.InteropServices;
using VRage.NativeWrapper;
using VRageMath;

namespace VRage.Voxels.Sewing
{
	public class VrTailor : PointerHandle
	{
		public enum GeneratedVertexProtocol : byte
		{
			Disabled,
			AlwaysGenerate,
			OnlyRemap,
			Dynamic
		}

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct RemappedVertex
		{
			public Vector3I Cell;

			public ushort Index;

			public byte ProducedTriangleCount;

			public byte GenerationCorner;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct VertexRef
		{
			public byte Mesh;

			private byte _pad;

			public ushort Index;
		}

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrTailor_Create();

		[DllImport("VRage.Native.dll")]
		public static extern void VrTailor_Release(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrTailor_Sew(IntPtr instance, IntPtr* guides, VrSewOperation operations, Vector3I min, Vector3I max);

		[DllImport("VRage.Native.dll")]
		public static extern void VrTailor_ClearBuffers(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public static extern void VrTailor_SetDebug(IntPtr instance, bool debug);

		[DllImport("VRage.Native.dll")]
		public static extern void VrTailor_SetGenerate(IntPtr instance, GeneratedVertexProtocol generate);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrTailor_DebugReadGenerated(IntPtr instance, out ushort* generatedVertices, out int count);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrTailor_DebugReadRemapped(IntPtr instance, out RemappedVertex* remappedVertices, out int count);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrTailor_DebugReadStudied(IntPtr instance, out VertexRef* studiedVertices, out int count);

		public VrTailor()
			: base(VrTailor_Create())
		{
			TrackReferences = false;
		}

		public unsafe void Sew(VrSewGuide[] guides, VrSewOperation operations, Vector3I min, Vector3I max)
		{
			IntPtr* ptr = stackalloc IntPtr[8];
			for (int i = 0; i < 8; i++)
			{
				ptr[i] = guides[i]?.NativeObject ?? IntPtr.Zero;
			}
			VrTailor_Sew(base.NativeObject, ptr, operations, min, max);
			if (guides[0] != null && guides[0].Mesh == null)
			{
				guides[0].CheckCreatedMesh();
			}
			guides[0].UpdateMemoryStats();
		}

		public void Sew(VrSewGuide[] guides, VrSewOperation operations)
		{
			Sew(guides, operations, Vector3I.Zero, guides[0].Size - 1);
		}

		public void ClearBuffers()
		{
			VrTailor_ClearBuffers(base.NativeObject);
		}

		public void SetDebug(bool debug)
		{
			VrTailor_SetDebug(base.NativeObject, debug);
		}

		public void SetGenerate(GeneratedVertexProtocol generate)
		{
			VrTailor_SetGenerate(base.NativeObject, generate);
		}

		public unsafe void DebugReadGenerated(out ushort* generatedVertices, out int count)
		{
			VrTailor_DebugReadGenerated(base.NativeObject, out generatedVertices, out count);
		}

		public unsafe void DebugReadRemapped(out RemappedVertex* remappedVertices, out int count)
		{
			VrTailor_DebugReadRemapped(base.NativeObject, out remappedVertices, out count);
		}

		public unsafe void DebugReadStudied(out VertexRef* studiedVertices, out int count)
		{
			VrTailor_DebugReadStudied(base.NativeObject, out studiedVertices, out count);
		}

		protected override void Dispose(bool disposing)
		{
			VrTailor_Release(base.NativeObject);
		}
	}
}
