using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Lodding;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Resources;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage2.Instancing
{
	[GenerateActivator]
	internal class MyInstance : IMySceneResourceOwner
	{
		private class VRage_Render11_GeometryStage2_Instancing_MyInstance_003C_003EActor : IActivator, IActivator<MyInstance>
		{
			private sealed override object CreateInstance()
			{
				return new MyInstance();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInstance CreateInstance()
			{
				return new MyInstance();
			}

			MyInstance IActivator<MyInstance>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyInstanceComponent Owner;

		public uint ActorID;

		public MyModel Model;

		public MyModelInstance ModelInstance;

		public HalfVector4 KeyColor;

		private MyInstanceMaterials m_instanceMaterials;

		public readonly MyLodStrategy LodStrategy = new MyLodStrategy();

		private MyInstanceVisibilityStrategy m_visibilityStrategy;

		public MyTransformStrategy TransformStrategy;

		private int m_worldMatrixIndex;

		private long m_gbufferVisibleFrame;

		private Dictionary<string, MyTextureChange> m_modelChanges;

		public long VisibleFrameIndex { get; private set; }

		public MyVisibilityExtFlags VisibilityExt => m_visibilityStrategy.VisibilityExt;

		public bool Visibility
		{
			get
			{
				return m_visibilityStrategy.Visibility;
			}
			set
			{
				m_visibilityStrategy.Visibility = value;
			}
		}

		public Vector3 CameraTranslation => TransformStrategy.GetCameraTranslation();

		public string ModelFilepath { get; private set; }

		public bool IsDummyModel { get; private set; }

		public bool MetalnessColorable { get; private set; }

		public event Action OnResourcesChanged;

		public bool CheckMatrix(ref MatrixD mat)
		{
			return TransformStrategy.CheckMatrix(ref mat);
		}

		public void GetMatrixCols(out RowMatrix m)
		{
			TransformStrategy.GetWorldMatrixCols(out m);
		}

		public bool IsGBufferVisible()
		{
			return m_visibilityStrategy.GBufferVisibility;
		}

		public bool IsDepthVisible()
		{
			return m_visibilityStrategy.DepthVisibility;
		}

		public bool IsForwardVisible()
		{
			return m_visibilityStrategy.ForwardVisibility;
		}

		public MyLod GetHighlightLod()
		{
			return ModelInstance.Model.GetLod(0);
		}

		public MyLodInstance GetHighlightLodInstance()
		{
			return ModelInstance.LodInstances[0];
		}

		public void SetDithered(bool isHologram, float dithered)
		{
			MyInstanceLodState state;
			float stateData;
			if (dithered == 0f)
			{
				state = MyInstanceLodState.Solid;
				stateData = 0f;
			}
			else if (isHologram)
			{
				state = MyInstanceLodState.Hologram;
				stateData = 0f - dithered;
			}
			else
			{
				state = MyInstanceLodState.Dithered;
				stateData = dithered;
			}
			LodStrategy.SetExplicitLodState(state, stateData);
		}

		public MyInstanceMaterial GetInstanceMaterial(int instanceMaterialOffset)
		{
			return m_instanceMaterials.GetInstanceMaterial(instanceMaterialOffset);
		}

		public HalfVector4 GetInstanceMaterialPackedColorMultEmissivity(int instanceMaterialOffset)
		{
			return m_instanceMaterials.GetInstanceMaterialPackedColorMultEmissivity(instanceMaterialOffset);
		}

		public Dictionary<string, MyInstanceMaterial> GetInstanceMaterials()
		{
			return m_instanceMaterials.GetInstanceMaterials();
		}

		public MyInstanceMaterial GetInstanceMaterial(string materialName)
		{
			int instanceMaterialOffset = Model.GetInstanceMaterialOffset(materialName);
			if (instanceMaterialOffset == -1)
			{
				return default(MyInstanceMaterial);
			}
			return m_instanceMaterials.GetInstanceMaterial(instanceMaterialOffset);
		}

		public void SetInstanceMaterial(string materialName, MyInstanceMaterial instanceMaterial)
		{
			int instanceMaterialOffset = Model.GetInstanceMaterialOffset(materialName);
			if (instanceMaterialOffset != -1)
			{
				Model.ActivateInstanceMaterial(materialName);
			}
			m_instanceMaterials.SetInstanceMaterial(materialName, instanceMaterialOffset, instanceMaterial);
		}

		internal void Init(MyModel model, bool isVisible, MyVisibilityExtFlags visibilityExt, MyInstanceComponent component, uint actorId, bool metalnessColorable, string modelFilepath, bool isDummyModel)
		{
			Owner = component;
			ActorID = actorId;
			Model = model;
			ModelInstance = model.GetInstance();
			m_instanceMaterials.Init();
			SetModel(model, modelFilepath, isDummyModel);
			KeyColor = default(HalfVector4);
			MetalnessColorable = metalnessColorable;
			m_visibilityStrategy.Init(isVisible, visibilityExt);
			TransformStrategy = default(MyTransformStrategy);
			m_worldMatrixIndex = -1;
			Owner.Owner.GetSceneResourcePrioritizationComponent().RegisterResourceOwner(this);
		}

		internal void CopyFrom(MyInstance instance)
		{
			KeyColor = instance.KeyColor;
			SetModel(instance.Model, instance.ModelFilepath, instance.IsDummyModel);
			LodStrategy.CopyFrom(instance.LodStrategy);
			m_visibilityStrategy.Init(instance.Visibility, instance.VisibilityExt);
			Owner = instance.Owner;
		}

		internal bool OnReloadModel()
		{
			if (!Model.IsValid)
			{
				return false;
			}
			LodStrategy.Init(Model.GetLodStrategyInfo());
			MyInstanceMaterial @default = MyInstanceMaterial.Default;
			m_instanceMaterials.OnReloadModel(Model, @default);
			return true;
		}

		internal void DisposeInternal()
		{
			ModelInstance = null;
			OnInstanceChanged();
			Owner.Owner.GetSceneResourcePrioritizationComponent().UnregisterResourceOwner(this);
			Owner = null;
			Model = null;
			ModelFilepath = null;
			m_modelChanges = null;
		}

		public void SetWorldMatrix(ref MatrixD worldMatrix, ref Vector3D camPosition)
		{
			TransformStrategy.SetWorldMatrix(ref worldMatrix, ref camPosition);
		}

		public void UpdateWorldMatrix(ref Vector3D camPosition)
		{
			MyActor owner = Owner.Owner;
			VisibleFrameIndex = MyCommon.FrameCounter;
			if (owner.WorldMatrixIndex != m_worldMatrixIndex)
			{
				owner.UpdateWorldMatrix();
				m_worldMatrixIndex = owner.WorldMatrixIndex;
				TransformStrategy.SetWorldMatrix(ref owner.LastWorldMatrix, ref camPosition);
			}
			else
			{
				TransformStrategy.UpdateTranslation(ref owner.LastWorldMatrix, ref camPosition);
			}
		}

		public bool SetModel(MyModel model, string modelFilepath, bool isDummyModel)
		{
			if (!Model.IsValid)
			{
				return false;
			}
			IsDummyModel = isDummyModel;
			ModelFilepath = modelFilepath;
			Model = model;
			if (!IsDummyModel && m_modelChanges != null)
			{
				AddTextureChanges(m_modelChanges);
			}
			else
			{
				ModelInstance = Model.GetInstance();
				OnInstanceChanged();
			}
			if (OnReloadModel())
			{
				if (Owner != null)
				{
					Owner.Owner.AddLocalAabb(Model.BoundingBox);
				}
				return true;
			}
			return false;
		}

		public void MarkGbufferVisible()
		{
			Interlocked.Exchange(ref m_gbufferVisibleFrame, MyCommon.FrameCounter);
		}

		public bool CheckGbufferVisible()
		{
			return m_gbufferVisibleFrame >= MyCommon.FrameCounter - 1;
		}

		public void StartFadeIn()
		{
			LodStrategy.StartTransition(GetDistance(), fadeIn: true);
		}

		public float GetDistance()
		{
			return (float)(MyRender11.Environment.Matrices.CameraPosition - Owner.Owner.WorldMatrix.Translation).Length();
		}

		public void StartFadeOut()
		{
			LodStrategy.StartTransition(GetDistance(), fadeIn: false);
			LodStrategy.OnTransitionEndOnce += delegate
			{
				Owner.Owner.Scene.Updater.CallIn(HideInstance, MyTimeSpan.Zero);
			};
		}

		private void HideInstance()
		{
			if (Owner != null && Owner.Owner != null)
			{
				Owner.Owner.SetVisibility(visibility: false);
			}
		}

		public Dictionary<string, MyTextureChange> GetTextureChanges()
		{
			if (m_modelChanges != null)
			{
				return m_modelChanges;
			}
			return ModelInstance.Changes;
		}

		public void AddTextureChanges(Dictionary<string, MyTextureChange> changes)
		{
			m_modelChanges = changes;
			if (!IsDummyModel)
			{
				ModelInstance = Model.GetInstance(changes);
				OnInstanceChanged();
			}
		}

		public IEnumerable<ResourceInfo> GetResources()
		{
			if (ModelInstance.Resources != null)
			{
				IMySceneResource[] resources = ModelInstance.Resources;
				foreach (IMySceneResource resource in resources)
				{
					yield return new ResourceInfo
					{
						Resource = resource,
						UseCount = 1
					};
				}
			}
		}

		private void OnInstanceChanged()
		{
			this.OnResourcesChanged.InvokeIfNotNull();
		}
	}
}
