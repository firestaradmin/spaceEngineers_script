using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Model;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal class MyInstanceComponent : MyActorComponent
	{
		private class VRage_Render11_GeometryStage2_Instancing_MyInstanceComponent_003C_003EActor : IActivator, IActivator<MyInstanceComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyInstanceComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInstanceComponent CreateInstance()
			{
				return new MyInstanceComponent();
			}

			MyInstanceComponent IActivator<MyInstanceComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyInstance m_instance;

		private MyCompatibilityDataForTheOldPipeline m_compatibilityDataForTheOldPipeline;

		private readonly MyChildCullTreeData m_cullData;

		public MyCompatibilityDataForTheOldPipeline CompatibilityDataForTheOldPipeline => m_compatibilityDataForTheOldPipeline;

		public override Color DebugColor => Color.Green;

		public HalfVector4 KeyColor
		{
			get
			{
				return m_instance.KeyColor;
			}
			set
			{
				m_instance.KeyColor = value;
			}
		}

		public int DepthBias
		{
			get
			{
				return m_compatibilityDataForTheOldPipeline.DepthBias;
			}
			set
			{
				m_compatibilityDataForTheOldPipeline.DepthBias = value;
			}
		}

		public MyInstanceComponent()
		{
			m_cullData = new MyChildCullTreeData
			{
				Add = delegate(MyCullResultsBase x, bool y)
				{
					((MyCullResults)x).Instances.Add(m_instance);
				},
				Remove = delegate(MyCullResultsBase x)
				{
					((MyCullResults)x).Instances.Remove(m_instance);
				},
				DebugColor = () => DebugColor
			};
		}

		public void Init(MyModel model, bool isVisible, uint actorId, MyVisibilityExtFlags visibilityExt, MyCompatibilityDataForTheOldPipeline compatibilityData, bool metalnessColorable, string modelFilepath, bool isDummyModel)
		{
			m_compatibilityDataForTheOldPipeline = compatibilityData;
			m_instance = MyManagers.Instances.CreateInstance(model, isVisible, visibilityExt, this, actorId, metalnessColorable, modelFilepath, isDummyModel);
			base.Owner.SetMatrix(ref MatrixD.Identity);
			base.Owner.AddLocalAabb(model.BoundingBox);
			base.Owner.InvalidateCullTreeData();
		}

		public override MyChildCullTreeData GetCullTreeData()
		{
			return m_cullData;
		}

		public void SetDithered(bool isHologram, float dithered)
		{
			if (dithered == 0f)
			{
				m_compatibilityDataForTheOldPipeline.Dithering = 0f;
			}
			else if (isHologram)
			{
				m_compatibilityDataForTheOldPipeline.Dithering = 0f - dithered;
			}
			else
			{
				m_compatibilityDataForTheOldPipeline.Dithering = dithered;
			}
			m_instance.SetDithered(isHologram, dithered);
		}

		public void GetMatrixCols(out RowMatrix m)
		{
			m_instance.GetMatrixCols(out m);
		}

		public MyLod GetHighlightLod()
		{
			return m_instance.GetHighlightLod();
		}

		public MyLodInstance GetHighlightLodInstance()
		{
			return m_instance.GetHighlightLodInstance();
		}

		public void SetInstanceMaterial(string materialName, float? emissivity, Color? color)
		{
			if (emissivity.HasValue || color.HasValue)
			{
				MyInstanceMaterial instanceMaterial = GetInstanceMaterial(materialName);
				if (emissivity.HasValue)
				{
					instanceMaterial.Emissivity = emissivity.Value;
				}
				if (color.HasValue)
				{
					instanceMaterial.ColorMult = color.Value;
				}
				SetInstanceMaterial(materialName, instanceMaterial);
			}
		}

		public void SetInstanceMaterial(string materialName, MyInstanceMaterial instanceMaterial)
		{
			if (!string.IsNullOrEmpty(materialName))
			{
				m_instance.SetInstanceMaterial(materialName, instanceMaterial);
			}
		}

		public MyInstanceMaterial GetInstanceMaterial(string materialName)
		{
			return m_instance.GetInstanceMaterial(materialName);
		}

		public Dictionary<string, MyInstanceMaterial> GetInstanceMaterials()
		{
			return m_instance.GetInstanceMaterials();
		}

		public bool IsUsedMaterialWithinModel(string materialName)
		{
			if (m_instance.IsDummyModel)
			{
				return true;
			}
			return m_instance.Model.IsUsedMaterial(materialName);
		}

		public override void OnRemove(MyActor owner)
		{
			MyManagers.Instances.DisposeInstance(m_instance);
			m_instance = null;
			base.OnRemove(owner);
		}

		public override void OnVisibilityChange()
		{
			base.OnVisibilityChange();
			m_instance.Visibility = base.Owner.IsVisible;
		}

		public void StartFadeIn()
		{
			m_instance.StartFadeIn();
		}

		public override bool StartFadeOut()
		{
			m_instance.StartFadeOut();
			return false;
		}

		public void AddTextureChanges(Dictionary<string, MyTextureChange> changes)
		{
			m_instance.AddTextureChanges(changes);
		}

		public Dictionary<string, MyTextureChange> GetTextureChanges()
		{
			return m_instance.GetTextureChanges();
		}

		public void UpdateWorldMatrix()
		{
			Vector3D camPosition = MyRender11.Environment.Matrices.CameraPosition;
			m_instance.UpdateWorldMatrix(ref camPosition);
		}
	}
}
