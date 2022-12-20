using System;
using System.Runtime.InteropServices;

namespace VRage.Platform.Windows.DShow
{
	[ComVisible(false)]
	internal class Clsid
	{
		public static readonly Guid FilterGraph = new Guid(3828804531u, 21071, 4558, 159, 83, 0, 32, 175, 11, 167, 112);

		public static readonly Guid WMVideoDecoderDMO = new Guid("{82D353DF-90BD-4382-8BC2-3F6192B76E34}");

		public static readonly Guid WMVideoDecoderDMO_cat = new Guid("{4A69B442-28BE-4991-969C-B500ADF5D8A8}");

		public static readonly Guid SampleGrabber = new Guid(3253993632u, 16136, 4563, 159, 11, 0, 96, 8, 3, 158, 55);

		public static readonly Guid NullRenderer = new Guid(3253993636u, 16136, 4563, 159, 11, 0, 96, 8, 3, 158, 55);
	}
}
