using VRage.Render11.Resources;

namespace VRageRender
{
	public struct MyMeshBuffers
	{
		internal IVertexBuffer VB0;

		internal IVertexBuffer VB1;

		internal IIndexBuffer IB;

		internal static readonly MyMeshBuffers Empty;

		public static bool operator ==(MyMeshBuffers left, MyMeshBuffers right)
		{
			if (left.VB0 == right.VB0 && left.VB1 == right.VB1)
			{
				return left.IB == right.IB;
			}
			return false;
		}

		public static bool operator !=(MyMeshBuffers left, MyMeshBuffers right)
		{
			if (left.VB0 == right.VB0 && left.VB1 == right.VB1)
			{
				return left.IB == right.IB;
			}
			return true;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MyMeshBuffers)
			{
				MyMeshBuffers myMeshBuffers = (MyMeshBuffers)obj2;
				if (VB0 == myMeshBuffers.VB0 && VB1 == myMeshBuffers.VB1)
				{
					return IB == myMeshBuffers.IB;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return VB0.GetHashCode() ^ VB1.GetHashCode() ^ IB.GetHashCode();
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
