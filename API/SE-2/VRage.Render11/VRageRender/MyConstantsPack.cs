using VRage.Render11.Resources;

namespace VRageRender
{
	internal struct MyConstantsPack
	{
		internal byte[] Data;

		internal IConstantBuffer CB;

		internal int Version;

		internal MyBindFlag BindFlag;

		public override string ToString()
		{
			return string.Format("Data Length {0}, {1}, Version {2}, BindFlags {3}", (Data != null) ? Data.Length.ToString() : "null", (CB != null) ? $"CB Desc (BindFlags {CB.Description.BindFlags}, CpuAccessFlags {CB.Description.CpuAccessFlags}, OptionFlags {CB.Description.OptionFlags}, Usage {CB.Description.Usage}, SizeInBytes {CB.Description.SizeInBytes}, StructureByteStride {CB.Description.StructureByteStride})" : "CB null", Version, BindFlag);
		}
	}
}
