using VRage.Library;
using VRage.Render11.Resources;

namespace VRageRender
{
	public struct MyMeshRawData
	{
		internal VertexLayoutId VertexLayout;

		internal NativeArray Indices;

		internal MyIndexBufferFormat IndicesFmt;

		internal NativeArray VertexStream0;

		internal int Stride0;

		internal NativeArray VertexStream1;

		internal int Stride1;
	}
}
