using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRageRender;

namespace VRage.Render11.Scene.Components
{
	internal class MyMergeGroupLeafComponent : MyActorComponent
	{
		private class VRage_Render11_Scene_Components_MyMergeGroupLeafComponent_003C_003EActor : IActivator, IActivator<MyMergeGroupLeafComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyMergeGroupLeafComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMergeGroupLeafComponent CreateInstance()
			{
				return new MyMergeGroupLeafComponent();
			}

			MyMergeGroupLeafComponent IActivator<MyMergeGroupLeafComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyActor m_parent;

		public MyMaterialMergeGroup MergeGroup { get; internal set; }

		public bool Mergeable { get; private set; }

		public override void Construct()
		{
			base.Construct();
			MergeGroup = null;
			Mergeable = false;
			m_parent = null;
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
		}

		public override void OnRemove(MyActor owner)
		{
			if (MergeGroup != null)
			{
				MergeGroup.RemoveEntity(base.Owner);
				MergeGroup = null;
			}
			base.OnRemove(owner);
		}

		public override void OnParentSet()
		{
			if (m_parent != null)
			{
				m_parent.GetMergeGroupRoot().Remove(base.Owner);
			}
			MyRenderableComponent renderable = base.Owner.GetRenderable();
			if (renderable != null && base.Owner.Parent != null)
			{
				MeshId model = renderable.GetModel();
				MyMeshMaterialId material = MyMeshes.GetMeshPart(model, 0, 0).Info.Material;
				bool flag = model.Info.RuntimeGenerated || model.Info.Dynamic;
				if (MyMeshMaterials1.IsMergable(material) && MyBigMeshTable.Table.IsMergable(model) && !flag)
				{
					Mergeable = true;
					MyBigMeshTable.Table.AddMesh(model);
					base.Owner.Parent.GetMergeGroupRoot().Add(base.Owner);
				}
			}
		}

		public override void OnParentRemoved()
		{
<<<<<<< HEAD
			base.Owner?.Parent?.GetMergeGroupRoot()?.Remove(base.Owner);
=======
			base.Owner.Parent.GetMergeGroupRoot().Remove(base.Owner);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
