using System;
using System.Runtime.InteropServices;

namespace VRage.Voxels
{
	public class VrRandomizePostprocessing : VrPostprocessing
	{
		[DllImport("VRage.Native.dll")]
		public static extern IntPtr VrRandomizePostprocessing_Create(float ammount);

		[DllImport("VRage.Native.dll")]
		public static extern void VrRandomizePostprocessing_Release(IntPtr instance);

		public VrRandomizePostprocessing(float ammount)
			: base(VrRandomizePostprocessing_Create(ammount))
		{
		}

		public override IntPtr GetStep()
		{
			return base.NativeObject;
		}

		protected override void Dispose(bool disposing)
		{
			VrRandomizePostprocessing_Release(base.NativeObject);
		}
	}
}
