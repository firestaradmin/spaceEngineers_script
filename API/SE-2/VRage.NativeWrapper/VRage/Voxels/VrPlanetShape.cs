using System;
using System.Runtime.InteropServices;
using VRage.NativeWrapper;
using VRageMath;

namespace VRage.Voxels
{
	public class VrPlanetShape : PointerHandle
	{
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct Mapset
		{
			public unsafe ushort* Front;

			public unsafe ushort* Back;

			public unsafe ushort* Left;

			public unsafe ushort* Right;

			public unsafe ushort* Up;

			public unsafe ushort* Down;

			public int Resolution;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct DetailMapData
		{
			public unsafe byte* Data;

			public int Resolution;

			public float Factor;

			public float Size;

			public float Scale;

			public float m_min;

			public float m_max;

			public float m_in;

			public float m_out;

			public float m_inRecip;

			public float m_outRecip;

			public float m_mid;
		}

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrPlanetShape_Create(Vector3 translation, float radius, float hillMin, float hillMax, Mapset maps, DetailMapData detailMapData, bool useLegacyAtan);

		[DllImport("VRage.Native.dll")]
		public static extern void VrPlanetShape_Release(IntPtr instance);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void VrPlanetShape_ReadContentRange(IntPtr instance, byte* dest, int destSize, int offset, Vector3I strides, Vector3I start, Vector3I end, float lodVoxelSize, int faceHint);

		[DllImport("VRage.Native.dll")]
		public static extern int VrPlanetShape_CheckContentRange(IntPtr instance, Vector3I start, Vector3I end, float lodVoxelSize, int faceHint);

		[DllImport("VRage.Native.dll")]
		public static extern float VrPlanetShape_GetValue(IntPtr instance, Vector2 texcoords, int face, out Vector3 normal);

		[DllImport("VRage.Native.dll")]
		public static extern float VrPlanetShape_GetHeight(IntPtr instance, Vector2 texcoords, int face, out Vector3 normal);

		public VrPlanetShape(Vector3 translation, float radius, float hillMin, float hillMax, Mapset maps, DetailMapData detailMapData, bool useLegacyAtan)
			: base(VrPlanetShape_Create(translation, radius, hillMin, hillMax, maps, detailMapData, useLegacyAtan))
		{
		}

		public unsafe void ReadContentRange(byte[] dest, int offset, ref Vector3I strides, ref Vector3I start, ref Vector3I end, float lodVoxelSize, int faceHint)
		{
			fixed (byte* dest2 = dest)
			{
				VrPlanetShape_ReadContentRange(base.NativeObject, dest2, dest.Length, offset, strides, start, end, lodVoxelSize, faceHint);
			}
		}

		public float GetValue(ref Vector2 texcoords, int face, out Vector3 normal)
		{
			return VrPlanetShape_GetValue(base.NativeObject, texcoords, face, out normal);
		}

		public float GetHeight(ref Vector2 texcoords, int face, out Vector3 normal)
		{
			return VrPlanetShape_GetHeight(base.NativeObject, texcoords, face, out normal);
		}

		protected override void Dispose(bool disposing)
		{
			VrPlanetShape_Release(base.NativeObject);
		}

		public int CheckContentRange(ref Vector3I reqMinInLod, ref Vector3I reqMaxInLod, float lodVoxelSize, int face)
		{
			return VrPlanetShape_CheckContentRange(m_handle, reqMaxInLod, reqMaxInLod, lodVoxelSize, face);
		}
	}
}
