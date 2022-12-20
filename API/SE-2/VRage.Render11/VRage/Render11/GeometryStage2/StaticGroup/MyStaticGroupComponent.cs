using System.Collections.Generic;
using VRage.Generics;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Model;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.StaticGroup
{
	internal class MyStaticGroupComponent : MyActorComponent
	{
		private class VRage_Render11_GeometryStage2_StaticGroup_MyStaticGroupComponent_003C_003EActor : IActivator, IActivator<MyStaticGroupComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyStaticGroupComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyStaticGroupComponent CreateInstance()
			{
				return new MyStaticGroupComponent();
			}

			MyStaticGroupComponent IActivator<MyStaticGroupComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyObjectsPool<MyStaticGroup> m_staticGroupsPool = new MyObjectsPool<MyStaticGroup>(1);

		private static readonly MyObjectsPool<List<Matrix>> m_matricesPool = new MyObjectsPool<List<Matrix>>(1);

		private int m_currentLod;

		private MyModel m_model;

		private MyModelInstance m_modelInstance;

		private Vector3D m_translation;

		private List<Matrix> m_matrices;

		private MyGroupInstances? m_instances;

		private MyStaticGroup m_staticGroup;

		private bool m_isDummyModel;

		public string ModelFilepath { get; private set; }

		public override Color DebugColor => Color.Green;

		public void Init(MyModel model, Vector3D translation, Matrix[] matrices, string modelFilepath, bool isDummyModel)
		{
			ModelFilepath = modelFilepath;
			m_isDummyModel = isDummyModel;
			m_currentLod = int.MaxValue;
			m_model = model;
			m_modelInstance = model.GetInstance();
			m_translation = translation;
			m_matricesPool.AllocateOrCreate(out m_matrices);
			foreach (Matrix item in matrices)
			{
				m_matrices.Add(item);
			}
			Vector3 vector = Vector3.MaxValue;
			Vector3 vector2 = Vector3.MinValue;
			foreach (Matrix matrix2 in m_matrices)
			{
				Vector3 value = Vector3.Transform(Vector3.Zero, matrix2);
				vector = Vector3.Min(vector, value);
				vector2 = Vector3.Max(vector2, value);
			}
			CreateGroup();
			MatrixD matrix = MatrixD.CreateTranslation(translation);
			base.Owner.SetMatrix(ref matrix);
			base.Owner.SetLocalAabb(new BoundingBox(vector, vector2));
			base.Owner.EnableAabbUpdateBasedOnChildren = false;
			MyManagers.StaticGroups.Register(this);
		}

		public override void OnRemove(MyActor owner)
		{
			MyManagers.StaticGroups.Unregister(this);
			m_currentLod = int.MaxValue;
			m_model = null;
			m_modelInstance = null;
			m_translation = Vector3D.MaxValue;
			if (m_staticGroup != null)
			{
				m_staticGroup.Destroy(base.Owner);
				m_staticGroupsPool.Deallocate(m_staticGroup);
				m_staticGroup = null;
			}
			if (m_instances.HasValue)
			{
				m_instances.Value.Remove(base.Owner);
				m_instances = null;
			}
			m_matrices.Clear();
			m_matricesPool.Deallocate(m_matrices);
			m_matrices = null;
		}

		private bool IsRequestedStaticGroup(bool wasStaticGroup)
		{
			Vector3D cameraPosition = MyRender11.Environment.Matrices.CameraPosition;
			BoundingBoxD worldAabb = base.Owner.WorldAabb;
			float num = (float)worldAabb.Distance(cameraPosition);
			if (!m_model.GetLodStrategyInfo().IsEmpty && m_model.GetLodStrategyInfo().GetTheBestLodWithHisteresis(num, m_currentLod) == m_model.GetLodStrategyInfo().GetLodsCount() - 1)
			{
				return true;
			}
			if (wasStaticGroup)
			{
				return (double)num > worldAabb.Size.Length() / 2.0;
			}
			return (double)num > worldAabb.Size.Length() / 2.0 + 10.0;
		}

		private void CreateGroup()
		{
			m_staticGroupsPool.AllocateOrCreate(out m_staticGroup);
			m_staticGroup.Create(m_model, m_translation, m_matrices, base.Owner);
		}

		public void Update()
		{
			bool flag = IsRequestedStaticGroup(!m_instances.HasValue);
			if (m_instances.HasValue && flag)
			{
				m_instances.Value.Remove(base.Owner);
				m_instances = null;
				m_staticGroup.IsVisible = true;
			}
			else if (!m_instances.HasValue && !flag)
			{
				MyGroupInstances value = default(MyGroupInstances);
				value.Init(m_model, m_translation, m_matrices, base.Owner, ModelFilepath, m_isDummyModel);
				m_instances = value;
				m_staticGroup.IsVisible = false;
			}
		}

		public bool OnReloadModel()
		{
			return true;
		}

		public bool SetModel(MyModel model, string filePath, bool isDummyModel)
		{
			m_model = model;
			m_modelInstance = model.GetInstance();
			ModelFilepath = filePath;
			m_isDummyModel = isDummyModel;
			return true;
		}
	}
}
