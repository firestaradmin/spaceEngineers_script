using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	internal class DsOptInt64
	{
		public long Value;

		public DsOptInt64(long Value)
		{
			this.Value = Value;
		}
	}
}
