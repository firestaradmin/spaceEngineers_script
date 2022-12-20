using System.Collections.Generic;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Import;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyRenderMeshInfo
	{
		internal MyVertexInputLayout VertexLayout;

		internal IVertexBuffer[] VB;

		internal IIndexBuffer IB;

		internal int Id;

		internal Dictionary<MyMeshDrawTechnique, MyDrawSubmesh[]> Parts = new Dictionary<MyMeshDrawTechnique, MyDrawSubmesh[]>();

		internal Dictionary<MyMeshDrawTechnique, MySubmeshInfo[]> PartsMetadata = new Dictionary<MyMeshDrawTechnique, MySubmeshInfo[]>();

		internal BoundingBox? BoundingBox;

		internal BoundingSphere? BoundingSphere;

		internal List<MySubmeshInfo> m_submeshes = new List<MySubmeshInfo>();

		internal int VerticesNum;

		internal int IndicesNum;

		internal ushort[] Indices;

		internal MyVertexFormatPositionH4[] VertexPositions;

		internal MyVertexFormatTexcoordNormalTangent[] VertexExtendedData;

		internal int PartsNum => Parts.Count;

		internal bool IsAnimated { get; set; }

		internal void ReleaseBuffers()
		{
			if (IB != null)
			{
				MyManagers.Buffers.Dispose(IB);
				IB = null;
			}
			if (VB != null)
			{
				IVertexBuffer[] vB = VB;
				foreach (IVertexBuffer vertexBuffer in vB)
				{
					MyManagers.Buffers.Dispose(vertexBuffer);
				}
				VB = null;
			}
		}
	}
}
