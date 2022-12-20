using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Culling;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Scene.Components
{
	internal class MyMergeGroupRootComponent : MyActorComponent
	{
		private class VRage_Render11_Scene_Components_MyMergeGroupRootComponent_003C_003EActor : IActivator, IActivator<MyMergeGroupRootComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyMergeGroupRootComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMergeGroupRootComponent CreateInstance()
			{
				return new MyMergeGroupRootComponent();
			}

			MyMergeGroupRootComponent IActivator<MyMergeGroupRootComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Dictionary<int, MyMaterialMergeGroup> m_materialGroups;

		internal MyCullProxy_2 m_proxy;

		private int m_btreeProxy;

		private bool m_dirtyProxy;

		private int m_mergablesCounter;

		private bool m_isMerged;

		private int m_worldMatrixIndex;

		private bool m_dirtyTree;

		private const int MERGE_THRESHOLD = 4;

		public override Color DebugColor => Color.Khaki;

		public override void Construct()
		{
			base.Construct();
			m_btreeProxy = -1;
			m_dirtyProxy = false;
			m_materialGroups = new Dictionary<int, MyMaterialMergeGroup>();
			m_mergablesCounter = 0;
			m_isMerged = false;
			m_proxy = MyCullProxy_2.Allocate();
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
		}

		private void MarkDirty()
		{
			m_dirtyProxy = true;
			base.Owner.Scene.Updater.AddToNextUpdate(base.Owner);
		}

		public MyMaterialMergeGroup GetMaterialGroup(MyMeshMaterialId matId)
		{
			int representationKey = MyMeshMaterials1.Table[matId.Index].RepresentationKey;
			return m_materialGroups[representationKey];
		}

		public bool TryGetMaterialGroup(MyMeshMaterialId matId, out MyMaterialMergeGroup group)
		{
			int representationKey = MyMeshMaterials1.Table[matId.Index].RepresentationKey;
			return m_materialGroups.TryGetValue(representationKey, out group);
		}

		internal void OnDeviceReset()
		{
			foreach (MyMaterialMergeGroup value in m_materialGroups.Values)
			{
				value.MergeGroup.OnDeviceReset();
			}
			MarkDirty();
		}

		private void RebuildProxies()
		{
			if (!m_dirtyProxy)
			{
				return;
			}
			m_proxy.Extend(m_materialGroups.Count);
			foreach (KeyValuePair<int, MyMaterialMergeGroup> materialGroup in m_materialGroups)
			{
				int index = materialGroup.Value.Index;
				materialGroup.Value.BuildProxy(out m_proxy.Proxies[index], out m_proxy.SortingKeys[index]);
			}
			m_dirtyProxy = false;
		}

		private void TurnIntoMergeInstancing()
		{
			foreach (MyActor child in base.Owner.Children)
			{
				Merge(child);
			}
			m_isMerged = true;
		}

		private void TurnIntoSeparateRenderables()
		{
			foreach (MyActor child in base.Owner.Children)
			{
				MyRenderableComponent renderable = child.GetRenderable();
				MeshId model = renderable.GetModel();
				MyMeshMaterialId material = MyMeshes.GetMeshPart(model, 0, 0).Info.Material;
				bool flag = model.Info.RuntimeGenerated || model.Info.Dynamic;
				if (MyMeshMaterials1.IsMergable(material) && MyBigMeshTable.Table.IsMergable(model) && !flag && child.GetMergeGroupLeaf().MergeGroup != null)
				{
					int representationKey = MyMeshMaterials1.Table[material.Index].RepresentationKey;
					MyMaterialMergeGroup myMaterialMergeGroup = m_materialGroups.Get(representationKey);
					if (myMaterialMergeGroup != null)
					{
						renderable.IsRenderedStandAlone = true;
						child.GetMergeGroupLeaf().MergeGroup = null;
						myMaterialMergeGroup.RemoveEntity(child);
					}
				}
			}
			m_isMerged = false;
		}

		private void Merge(MyActor child)
		{
			MyRenderableComponent renderable = child.GetRenderable();
			MeshId model = renderable.GetModel();
			MyMeshMaterialId material = MyMeshes.GetMeshPart(model, 0, 0).Info.Material;
			bool flag = model.Info.RuntimeGenerated || model.Info.Dynamic;
			if (MyMeshMaterials1.IsMergable(material) && MyBigMeshTable.Table.IsMergable(model) && !flag)
			{
				int representationKey = MyMeshMaterials1.Table[material.Index].RepresentationKey;
				MyMaterialMergeGroup myMaterialMergeGroup = m_materialGroups.Get(representationKey);
				if (myMaterialMergeGroup == null)
				{
					int count = m_materialGroups.Count;
					myMaterialMergeGroup = new MyMaterialMergeGroup(MyBigMeshTable.Table, material, count);
					m_materialGroups[MyMeshMaterials1.Table[material.Index].RepresentationKey] = myMaterialMergeGroup;
				}
				renderable.IsRenderedStandAlone = false;
				child.GetMergeGroupLeaf().MergeGroup = myMaterialMergeGroup;
				myMaterialMergeGroup.AddEntity(child, model);
				myMaterialMergeGroup.UpdateEntity(child);
				MarkDirty();
			}
		}

		public override void OnUpdateBeforeDraw()
		{
			if (m_materialGroups.Count == 0)
			{
				return;
			}
			RebuildProxies();
			bool flag = base.Owner.WorldMatrixIndex != m_worldMatrixIndex;
			bool flag2 = base.Owner.DirtyProxy || m_dirtyTree;
			foreach (MyMaterialMergeGroup value in m_materialGroups.Values)
			{
				int index = value.Index;
				value.UpdateProxySubmeshes(ref m_proxy.Proxies[index], flag2);
			}
			if (flag)
			{
				foreach (MyMaterialMergeGroup value2 in m_materialGroups.Values)
				{
					value2.UpdateAll();
				}
			}
			foreach (MyMaterialMergeGroup value3 in m_materialGroups.Values)
			{
				value3.MoveToGPU();
			}
			if (flag2)
			{
				BoundingBoxD aabb = BoundingBoxD.CreateInvalid();
				foreach (MyActor child in base.Owner.Children)
				{
					if (child.IsVisible && child.GetRenderable() != null && !child.GetRenderable().IsRendered)
					{
						aabb.Include(child.CalculateAabb());
					}
				}
				if (m_materialGroups.Count > 0)
				{
					if (m_btreeProxy == -1)
					{
						m_btreeProxy = MyScene11.MergeGroupsDBVH.AddProxy(ref aabb, new MyChildCullTreeData
						{
							Add = delegate(MyCullResultsBase q, bool y)
							{
								MyCullResults obj2 = (MyCullResults)q;
								obj2.CullProxies2.Add(m_proxy);
								obj2.CullProxies2Contained.Add(y);
							},
							Remove = delegate(MyCullResultsBase q)
							{
								MyCullResults obj = (MyCullResults)q;
								int index2 = obj.CullProxies2.IndexOf(m_proxy);
								obj.CullProxies2.RemoveAt(index2);
								obj.CullProxies2Contained.RemoveAt(index2);
							},
							DebugColor = () => DebugColor
						}, 0u);
					}
					else
					{
						MyScene11.MergeGroupsDBVH.MoveProxy(m_btreeProxy, ref aabb, Vector3.Zero);
					}
				}
				m_dirtyTree = false;
			}
			m_worldMatrixIndex = base.Owner.WorldMatrixIndex;
		}

		public override void OnRemove(MyActor owner)
		{
			foreach (MyMaterialMergeGroup value in m_materialGroups.Values)
			{
				value.Release();
			}
			m_materialGroups.Clear();
			if (m_proxy != null)
			{
				MyCullProxy_2.Free(m_proxy);
				m_proxy = null;
			}
			if (m_btreeProxy != -1)
			{
				MyScene11.MergeGroupsDBVH.RemoveProxy(m_btreeProxy);
				m_btreeProxy = -1;
			}
			base.OnRemove(owner);
		}

		public void Add(MyActor child)
		{
			m_mergablesCounter++;
			if (!base.Owner.GetMergeGroupRoot().m_isMerged && m_mergablesCounter >= 4)
			{
				TurnIntoMergeInstancing();
			}
			else if (m_isMerged)
			{
				Merge(child);
			}
			m_dirtyTree = true;
		}

		public void Remove(MyActor child)
		{
			m_mergablesCounter = (child.GetMergeGroupLeaf().Mergeable ? (m_mergablesCounter - 1) : m_mergablesCounter);
			if (m_mergablesCounter < 4 && m_isMerged)
			{
				TurnIntoSeparateRenderables();
			}
			m_dirtyTree = true;
		}
	}
}
