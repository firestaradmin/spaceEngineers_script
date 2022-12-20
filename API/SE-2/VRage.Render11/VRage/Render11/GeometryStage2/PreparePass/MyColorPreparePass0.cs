using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Model;
using VRageMath.PackedVector;
using VRageRender;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	internal struct MyColorPreparePass0 : ICustomPreparePass0
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct MyVbConstantElement
		{
			public RowMatrix WorldMatrix;

			public HalfVector4 KeyColorDithering;

			public HalfVector4 ColorMultEmissivity;
		}

		private MyVbConstantElement[] m_elements;

		private int m_elementsCount;

		public unsafe int ElementSize => sizeof(MyVbConstantElement);

		public void InitInstanceElements(int elementsCount)
		{
			m_elementsCount = elementsCount;
			if (elementsCount != 0 && (m_elements == null || m_elements.Length < elementsCount))
			{
				m_elements = new MyVbConstantElement[elementsCount];
			}
		}

		public void AddInstanceIntoInstanceElements(int bufferOffset, MyInstance instance, int instanceMaterialOffsetInData, float stateData)
		{
			m_elements[bufferOffset] = new MyVbConstantElement
			{
				KeyColorDithering = new HalfVector4(instance.KeyColor.PackedValue | ((ulong)HalfUtils.Pack(stateData) << 48)),
				ColorMultEmissivity = ((instanceMaterialOffsetInData != -1) ? instance.GetInstanceMaterialPackedColorMultEmissivity(instanceMaterialOffsetInData) : MyInstanceMaterial.Default.PackedColorMultEmissivity),
				WorldMatrix = instance.TransformStrategy.RowMatrix
			};
		}

		public List<int> GetInstanceMaterialOffsetsForThePass(MyLod lod)
		{
			return lod.GetInstanceMaterialOffsets();
		}

		public int GetInstanceMaterialsCount(MyLod lod)
		{
			return lod.InstanceMaterialsCount;
		}

		public void WriteData(ref MyMapping vb)
		{
			vb.WriteAndPosition(m_elements, m_elementsCount);
		}
	}
}
