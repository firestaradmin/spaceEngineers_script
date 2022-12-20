using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Messages;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyDebugMesh
	{
		public IVertexBuffer Buffer;

		public MatrixD WorldMatrix;

		public bool Edges;

		public bool Depth;

		public unsafe MyDebugMesh(MyRenderMessageDebugDrawMesh message)
		{
			Buffer = MyManagers.Buffers.CreateVertexBuffer("MyDebugMesh", message.Vertices.Count, sizeof(MyVertexFormatPositionColor), null, ResourceUsage.Dynamic);
			Update(message);
		}

		internal void Update(MyRenderMessageDebugDrawMesh message)
		{
			WorldMatrix = message.WorldMatrix;
			Edges = !message.Shaded;
			Depth = message.DepthRead;
			if (Buffer.ElementCount < message.Vertices.Count)
			{
				MyManagers.Buffers.Resize(Buffer, message.Vertices.Count);
			}
			MyMapping myMapping = MyMapping.MapDiscard(MyImmediateRC.RC, Buffer);
			for (int i = 0; i < message.Vertices.Count; i++)
			{
				MyVertexFormatPositionColor data = new MyVertexFormatPositionColor(message.Vertices[i].Position, message.Vertices[i].Color);
				myMapping.WriteAndPosition(ref data);
			}
			myMapping.Unmap();
		}

		internal void Close()
		{
			if (Buffer != null)
			{
				MyManagers.Buffers.Dispose(Buffer);
			}
			Buffer = null;
		}
	}
}
