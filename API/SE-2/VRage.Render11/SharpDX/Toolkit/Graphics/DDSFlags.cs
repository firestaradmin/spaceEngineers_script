using System;

namespace SharpDX.Toolkit.Graphics
{
	/// <summary>
	/// Flags used by <see cref="M:SharpDX.Toolkit.Graphics.DDSHelper.LoadFromDDSMemory(System.IntPtr,System.Int32,System.Boolean,System.Nullable{System.Runtime.InteropServices.GCHandle},System.String)" />.
	/// </summary>
	[Flags]
	internal enum DDSFlags
	{
		None = 0x0,
		LegacyDword = 0x1,
		NoLegacyExpansion = 0x2,
		NoR10B10G10A2Fixup = 0x4,
		ForceRgb = 0x8,
		No16Bpp = 0x10,
		CopyMemory = 0x20,
		ForceDX10Ext = 0x10000
	}
}
