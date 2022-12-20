using VRage.Render11.Common;
using VRage.Render11.Resources.Buffers;

namespace VRage.Render11.Resources
{
	internal class MyGenericBuffer : MyBufferInternal, IVertexBuffer, IBuffer, IResource, IIndexBuffer
	{
		internal readonly int BufferSize;

		public MyIndexBufferFormat Format { get; set; }

		public MyQuery Query { get; set; }

		public MyGenericBuffer(int sizeInBytes)
		{
			BufferSize = sizeInBytes;
		}

		public void SetStride(int stride)
		{
			m_description.StructureByteStride = stride;
		}
	}
}
