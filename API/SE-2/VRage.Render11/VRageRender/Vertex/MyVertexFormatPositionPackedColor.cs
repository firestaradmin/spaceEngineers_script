using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionPackedColor
	{
		internal HalfVector4 Position;

		internal Byte4 Color;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionPackedColor);

		internal MyVertexFormatPositionPackedColor(HalfVector4 position, Byte4 color)
		{
			Position = position;
			Color = color;
		}

		internal MyVertexFormatPositionPackedColor(Vector3 position, Byte4 color)
		{
			Position = new HalfVector4(position.X, position.Y, position.Z, 1f);
			Color = color;
		}

		internal MyVertexFormatPositionPackedColor(Vector3 position, Color color)
		{
			Position = new HalfVector4(position.X, position.Y, position.Z, 1f);
			Color = new Byte4(color.PackedValue);
		}
	}
}
