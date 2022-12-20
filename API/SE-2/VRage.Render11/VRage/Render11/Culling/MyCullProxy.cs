using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling
{
	/// <summary>
	/// Contains data used for culling, but should not own any itself
	/// </summary>
	[PooledObject(2)]
	internal class MyCullProxy : IPooledObject
	{
		internal ulong[] SortingKeys;

		internal MyRenderableProxy[] RenderableProxies;

		internal int FirstFullyContainingCascadeIndex;

		internal MyRenderableComponent Parent;

		internal int Updated;

		internal MyRenderableProxyFlags RenderFlags;

		private int m_worldMatrixIndex;

		internal uint OwnerID
		{
			get
			{
				if (Parent == null)
				{
					return 0u;
				}
				return Parent.Owner.ID;
			}
		}

		void IPooledObject.Cleanup()
		{
			Clear();
		}

		internal void Clear()
		{
			SortingKeys = null;
			RenderableProxies = null;
			Updated = 0;
			FirstFullyContainingCascadeIndex = int.MaxValue;
			Parent = null;
			m_worldMatrixIndex = -1;
		}

		public void UpdateWorldMatrix()
		{
			FirstFullyContainingCascadeIndex = int.MaxValue;
			MyRenderableProxy[] renderableProxies;
			if (m_worldMatrixIndex == Parent.Owner.WorldMatrixIndex)
			{
				Vector3 localMatrixTranslation = Parent.Owner.WorldMatrix.Translation - MyRender11.Environment.Matrices.CameraPosition;
				renderableProxies = RenderableProxies;
				for (int i = 0; i < renderableProxies.Length; i++)
				{
					renderableProxies[i].CommonObjectData.LocalMatrixTranslation = localMatrixTranslation;
				}
				return;
			}
			m_worldMatrixIndex = Parent.Owner.WorldMatrixIndex;
			MatrixD m = Parent.Owner.WorldMatrix;
			Matrix localMatrix = default(Matrix);
			localMatrix.SetRotationAndScale(in m);
			localMatrix.Translation = m.Translation - MyRender11.Environment.Matrices.CameraPosition;
			localMatrix.M44 = 1f;
			renderableProxies = RenderableProxies;
			for (int i = 0; i < renderableProxies.Length; i++)
			{
				renderableProxies[i].CommonObjectData.LocalMatrix = localMatrix;
			}
		}

		public void ResetMatrixIndex()
		{
			m_worldMatrixIndex = -1;
		}
	}
}
