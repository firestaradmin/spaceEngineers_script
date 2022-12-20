using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderObjectUpdateData
	{
		public MatrixD WorldMatrix;

		public bool HasWorldMatrix;

		public Matrix LocalMatrix;
<<<<<<< HEAD
=======

		public bool HasLocalMatrix;

		public BoundingBox LocalAABB;

		public bool HasLocalAABB;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public bool HasLocalMatrix;

		public BoundingBox LocalAABB;

		public bool HasLocalAABB;

		public void Clean()
		{
			HasWorldMatrix = false;
			HasLocalMatrix = false;
			HasLocalAABB = false;
		}
	}
}
