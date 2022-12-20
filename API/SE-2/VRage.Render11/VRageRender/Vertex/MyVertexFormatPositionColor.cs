using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionColor
	{
		internal Vector3 Position;

		internal Byte4 Color;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionColor);

		internal MyVertexFormatPositionColor(Vector3 position, Byte4 color)
		{
			Position = position;
			Color = color;
		}

		internal MyVertexFormatPositionColor(Vector3 position, Color color)
		{
			Position = position;
			Color = new Byte4(color.PackedValue);
		}
	}
}
