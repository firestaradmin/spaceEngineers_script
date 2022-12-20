using SharpDX.Direct3D11;

namespace VRageRender
{
	internal struct VertexLayoutId
	{
		internal int Index;

		internal static readonly VertexLayoutId NULL = new VertexLayoutId
		{
			Index = -1
		};

		internal InputElement[] Elements => MyVertexLayouts.GetElements(this);

		internal MyVertexLayoutInfo Info => MyVertexLayouts.Layouts.Data[Index];

		internal bool HasBonesInfo => MyVertexLayouts.Layouts.Data[Index].HasBonesInfo;

		public static bool operator ==(VertexLayoutId x, VertexLayoutId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(VertexLayoutId x, VertexLayoutId y)
		{
			return x.Index != y.Index;
		}

<<<<<<< HEAD
		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is VertexLayoutId)
			{
				VertexLayoutId vertexLayoutId = (VertexLayoutId)obj2;
				return Index == vertexLayoutId.Index;
			}
			return false;
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override int GetHashCode()
		{
			return Index;
		}
	}
}
