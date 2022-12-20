using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Model;
using VRageRender;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	internal struct MyDepthPreparePass0 : ICustomPreparePass0
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct MyVbConstantElement
		{
			public RowMatrix WorldMatrix;
		}

		private int m_elementsCount;

		private MyVbConstantElement[] m_elements;

		private static readonly List<int> m_emptyInstanceMaterialOffsets = new List<int>();

		public unsafe int ElementSize => sizeof(MyVbConstantElement);

		public void InitInstanceElements(int elementsCount)
		{
			m_elementsCount = elementsCount;
			if (elementsCount != 0 && (m_elements == null || m_elements.Length < elementsCount))
			{
				m_elements = new MyVbConstantElement[elementsCount * 2];
			}
		}

		public void AddInstanceIntoInstanceElements(int bufferOffset, MyInstance instance, int instanceMaterialOffsetInData, float stateData)
		{
			m_elements[bufferOffset] = new MyVbConstantElement
			{
				WorldMatrix = instance.TransformStrategy.RowMatrix
			};
		}

		public int GetInstanceMaterialsCount(MyLod lod)
		{
			return 0;
		}

		public void WriteData(ref MyMapping vb)
		{
			vb.WriteAndPosition(m_elements, m_elementsCount);
		}

		public List<int> GetInstanceMaterialOffsetsForThePass(MyLod lod)
		{
			return m_emptyInstanceMaterialOffsets;
		}
	}
}
