using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VRage.Voxels
{
	public class IsoMesher
	{
		private readonly VrVoxelMesh m_mesh;

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void IsoMesher_Calculate(IntPtr mesh, int numSamples, byte* content, byte* material);

		[DllImport("VRage.Native.dll")]
		public unsafe static extern void IsoMesher_CalculateMaterials(IntPtr mesh, int numSamples, byte* content, byte* material, int materialOverride);

		public IsoMesher(VrVoxelMesh mesh)
		{
			m_mesh = mesh;
		}

		public unsafe void Calculate(int numSamples, byte* content, byte* material)
		{
			IsoMesher_Calculate(m_mesh.NativeObject, numSamples, content, material);
		}

		public unsafe void CalculateMaterials(int numSamples, byte* content, byte* material, int materialOverride)
		{
			IsoMesher_CalculateMaterials(m_mesh.NativeObject, numSamples, content, material, materialOverride);
		}

		public void PostProcess(List<VrPostprocessing> postProcessors)
		{
			foreach (VrPostprocessing postProcessor in postProcessors)
			{
				postProcessor.Process(m_mesh);
			}
		}
	}
}
