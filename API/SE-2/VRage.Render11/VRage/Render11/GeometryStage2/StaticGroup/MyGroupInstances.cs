using System.Collections.Generic;
using VRage.Generics;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Model;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.StaticGroup
{
	internal struct MyGroupInstances
	{
		private static readonly MyObjectsPool<List<MyInstance>> m_instancesPool = new MyObjectsPool<List<MyInstance>>(1);

		private static readonly MyObjectsPool<List<int>> m_proxiesPool = new MyObjectsPool<List<int>>(1);

		private List<MyInstance> m_instances;

		private List<int> m_proxies;

		public void Init(MyModel model, Vector3D translation, List<Matrix> matrices, MyActor root, string modelFilepath, bool isDummyModel)
		{
			if (m_instances == null)
			{
				m_instancesPool.AllocateOrCreate(out m_instances);
			}
			if (m_proxies == null)
			{
				m_proxiesPool.AllocateOrCreate(out m_proxies);
			}
			m_instances.Clear();
			Vector3D camPosition = MyRender11.Environment.Matrices.CameraPosition;
			for (int i = 0; i < matrices.Count; i++)
			{
				MyVisibilityExtFlags visibilityExt = MyVisibilityExtFlags.Gbuffer | MyVisibilityExtFlags.Depth;
				MyInstance myInstance = MyManagers.Instances.CreateInstance(model, isVisible: true, visibilityExt, null, 0u, metalnessColorable: false, modelFilepath, isDummyModel);
				MatrixD worldMatrix = matrices[i] * MatrixD.CreateTranslation(translation);
				myInstance.SetWorldMatrix(ref worldMatrix, ref camPosition);
				model.BoundingBox.Transform(matrices[i]);
			}
		}

		public void Remove(MyActor root)
		{
			if (m_instances != null)
			{
				for (int i = 0; i < m_instances.Count; i++)
				{
					MyManagers.Instances.DisposeInstance(m_instances[i]);
				}
				m_instances.Clear();
				m_instancesPool.Deallocate(m_instances);
				m_instances = null;
			}
			if (m_proxies != null)
			{
				m_proxies.Clear();
				m_proxiesPool.Deallocate(m_proxies);
				m_proxies = null;
			}
		}
	}
}
