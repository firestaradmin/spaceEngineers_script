using System;
using System.Runtime.InteropServices;
using VRageMath;

namespace VRage.Voxels.Sewing
{
	public class VrShellDataCache
	{
		private readonly IntPtr m_handle;

		public static VrShellDataCache Empty => new VrShellDataCache(VrShellDataCache_Empty());

		public static VrShellDataCache Full => new VrShellDataCache(VrShellDataCache_Full());

		internal IntPtr NativeObject => m_handle;

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrShellDataCache_FromDataCube(Vector3I size, byte[] content, byte[] material);

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrShellDataCache_Empty();

		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrShellDataCache_Full();

		public static VrShellDataCache FromDataCube(Vector3I size, byte[] content, byte[] material)
		{
			return new VrShellDataCache(VrShellDataCache_FromDataCube(size, content, material));
		}

		internal VrShellDataCache(IntPtr ptr)
		{
			m_handle = ptr;
		}
	}
}
