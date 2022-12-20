using System;
using System.Runtime.InteropServices;
using VRage.NativeWrapper;

namespace VRage.Voxels
{
	public abstract class VrPostprocessing : PointerHandle
	{
		[DllImport("VRage.Native.dll")]
		public static extern void VrPostprocessing_Process(IntPtr postprocess, IntPtr mesh);

		public abstract IntPtr GetStep();

		public void Process(VrVoxelMesh mesh)
		{
			VrPostprocessing_Process(GetStep(), mesh.NativeObject);
		}

		internal VrPostprocessing(IntPtr ptr)
			: base(ptr)
		{
			TrackReferences = false;
		}
	}
}
