#define VRAGE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Network;
using VRage.Render;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.GeometryStage.Geometry;
using VRage.Render11.GeometryStage.Voxel;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRage.Render11.Scene.Resources;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;
using VRageRender.Voxels;

namespace VRageRender
{
	internal sealed class MyVoxelCellComponent : MyRenderableComponent, IMyVoxelActorCell
	{
		private class VRageRender_MyVoxelCellComponent_003C_003EActor : IActivator, IActivator<MyVoxelCellComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelCellComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelCellComponent CreateInstance()
			{
				return new MyVoxelCellComponent();
			}

			MyVoxelCellComponent IActivator<MyVoxelCellComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string LoggingSymbol = "VRAGE";

		private MyRenderVoxelActor m_voxel;

		private int m_lod;

		private bool m_notify;

		/// <summary>
		/// Offset of this mesh in the container.
		/// </summary>
		private Vector3D m_offset;

		private MyRenderVoxelMesh m_voxelMesh;

		private readonly MyChildCullTreeData m_cullData;

		private Dictionary<int, int> m_materialsUsage = new Dictionary<int, int>();

		private bool m_visible;

		private const int FADE_TIME = 1000;

		public bool Visible => m_visible;

		public Vector3D Offset => m_offset;

		public int Lod => m_lod;

		public MyVoxelCellComponent()
		{
			m_cullData = new MyChildCullTreeData
			{
				Add = delegate(MyCullResultsBase q, bool y)
				{
					MyCullResults obj2 = (MyCullResults)q;
					obj2.CullProxies.Add(base.CullProxy);
					obj2.CullProxiesContained.Add(y);
				},
				Remove = delegate(MyCullResultsBase q)
				{
					MyCullResults obj = (MyCullResults)q;
					int index = obj.CullProxies.IndexOf(base.CullProxy);
					obj.CullProxies.RemoveAt(index);
					obj.CullProxiesContained.RemoveAt(index);
				},
				DebugColor = () => DebugColor,
				FarCull = true
			};
		}

		public override MyChildCullTreeData GetCullTreeData()
		{
			if (base.IsRenderedStandAlone && base.CullProxy.Parent != null)
			{
				return m_cullData;
			}
			return null;
		}

		public override void OnVisibilityChange()
		{
			Log("OnVisibilityChange " + m_visible);
			base.OnVisibilityChange();
			if (m_visible)
			{
				if (!m_mesh.IsLoaded())
				{
					m_voxelMesh.UpdateDevice();
				}
				MyManagers.FoliageManager.UpdateFoliage(base.Owner);
			}
			else
			{
				MyMeshes.UnloadMeshData(m_mesh);
			}
		}

		public override IEnumerable<ResourceInfo> GetResources()
		{
			foreach (ResourceInfo resource in base.GetResources())
			{
				yield return resource;
			}
			foreach (KeyValuePair<int, int> materialUsage in m_materialsUsage)
			{
				IEnumerable<IMyStreamedTextureArrayTile> materialTiles = MyVoxelMaterials.GetMaterialTiles(materialUsage.Key);
				if (materialTiles == null)
				{
					continue;
				}
				foreach (IMyStreamedTextureArrayTile item in materialTiles)
				{
					yield return new ResourceInfo
					{
						Resource = item,
						UseCount = materialUsage.Value
					};
				}
			}
		}

		public void Prepare(MyRenderVoxelActor voxel, Vector3D offset, int lod)
		{
			m_voxel = voxel;
			m_voxelMesh = new MyRenderVoxelMesh((Vector3I)offset, lod);
			m_lod = lod;
			m_notify = false;
			m_offset = offset;
			SetModel(m_voxelMesh.Mesh);
			UpdateParametersFromEntity();
		}

		private void CalculateWorldMatrix(out MatrixD worldMatrix)
		{
			worldMatrix = m_voxel.Owner.WorldMatrix;
			MyTransformD myTransformD = new MyTransformD(ref worldMatrix);
			myTransformD.Position += Vector3D.TransformNormal(m_offset, worldMatrix);
			worldMatrix = MatrixD.CreateFromTransformScale(myTransformD.Rotation, myTransformD.Position, new Vector3D(m_voxelMesh.LodScale));
		}

		public void UpdateParametersFromEntity()
		{
			AdditionalFlags = m_voxel.RenderFlags;
			CalculateWorldMatrix(out var worldMatrix);
			base.Owner.SetMatrix(ref worldMatrix);
			base.Owner.SetLocalAabb(m_voxelMesh.Aabb);
			if (m_lods != null)
			{
				MyRenderLod[] lods = m_lods;
				for (int i = 0; i < lods.Length; i++)
				{
					MyRenderableProxy[] renderableProxies = lods[i].RenderableProxies;
					foreach (MyRenderableProxy myRenderableProxy in renderableProxies)
					{
						if (m_voxel.Spherize)
						{
							myRenderableProxy.VoxelCommonObjectData.SpherizeTo = m_voxel.SpherizeRadius.Value;
							myRenderableProxy.VoxelCommonObjectData.SpherizeStart = m_voxel.SpherizeRadius.Value * 1.5f;
						}
						else
						{
							myRenderableProxy.VoxelCommonObjectData.SpherizeTo = float.NaN;
							myRenderableProxy.VoxelCommonObjectData.SpherizeStart = float.NaN;
						}
					}
				}
			}
			Log("UpdateParametersFromEntity");
		}

		protected override bool RebuildLodProxy(int lodNum, bool skinningEnabled, MySkinningComponent skinning, MyShaderUnifiedFlags appendFlags)
		{
			if (!base.Owner.IsVisible || !m_voxelMesh.Ready)
			{
				return false;
			}
			LodMeshId lodMesh = MyMeshes.GetLodMesh(m_mesh, 0);
			VertexLayoutId vertexLayout = lodMesh.VertexLayout;
			if (lodMesh.Info.PartsNum < 0)
			{
				return false;
			}
			MyObjectPoolManager.Init(ref m_lods[lodNum]);
			MyRenderLod myRenderLod = m_lods[lodNum];
			myRenderLod.VertexLayout1 = vertexLayout;
			if (m_voxelMesh.DeviceDirty)
			{
				m_voxelMesh.UpdateDevice();
				MyManagers.FoliageManager.UpdateFoliage(base.Owner);
			}
			else if (!m_mesh.IsLoaded())
			{
				m_voxelMesh.UpdateDevice();
			}
			myRenderLod.VertexShaderFlags = MyShaderUnifiedFlags.USE_VOXEL_DATA | MyShaderUnifiedFlags.USE_VOXEL_MORPHING;
			bool flag = true;
			MyLodMeshInfo info = lodMesh.Info;
			int num = 0;
			if (info.MultimaterialOffset > 0)
			{
				num++;
			}
			if (info.MultimaterialOffset != info.IndicesNum)
			{
				num++;
			}
			if (flag)
			{
				num++;
			}
			if (num > 0)
			{
				myRenderLod.AllocateProxies(num);
			}
			int constantBufferSize = MyRenderableComponent.GetConstantBufferSize(myRenderLod, skinningEnabled);
			int num2 = 0;
			if (true)
			{
				if (info.MultimaterialOffset > 0)
				{
					CreateRenderableProxyForPart(lodNum, constantBufferSize, num2++, 0, info.MultimaterialOffset, shadowsOnly: false, multiMaterial: false);
				}
				if (info.MultimaterialOffset != info.IndicesNum)
				{
					CreateRenderableProxyForPart(lodNum, constantBufferSize, num2++, info.MultimaterialOffset, info.IndicesNum - info.MultimaterialOffset, shadowsOnly: false, multiMaterial: true);
				}
			}
			if (flag)
			{
				CreateRenderableProxyForPart(lodNum, constantBufferSize, num2++, 0, info.IndicesNum, shadowsOnly: true, multiMaterial: false);
			}
			return true;
		}

		private void CreateRenderableProxyForPart(int lodIndex, int objectConstantsSize, int proxyIndex, int baseIndex, int indexCount, bool shadowsOnly, bool multiMaterial)
		{
			MyRenderLod myRenderLod = m_lods[lodIndex];
			MyMeshDrawTechnique technique = ((multiMaterial && !shadowsOnly) ? MyMeshDrawTechnique.VOXEL_MAP_MULTI : MyMeshDrawTechnique.VOXEL_MAP_SINGLE);
			MyRenderableProxy myRenderableProxy = myRenderLod.RenderableProxies[proxyIndex];
			myRenderableProxy.NonVoxelObjectData = MyObjectDataNonVoxel.Invalid;
			myRenderableProxy.VoxelCommonObjectData.VoxelOffset = m_offset;
			myRenderableProxy.VoxelCommonObjectData.VoxelScale = new Vector3(m_voxelMesh.LodScale);
			myRenderableProxy.VoxelCommonObjectData.VoxelLodSize = m_lod;
			myRenderableProxy.CommonObjectData.Emissive = MyModelProperties.DefaultEmissivity;
			myRenderableProxy.CommonObjectData.ColorMul = MyModelProperties.DefaultColorMul;
			myRenderableProxy.Technique = technique;
			if (m_voxel.Spherize)
			{
				myRenderableProxy.VoxelCommonObjectData.SpherizeTo = m_voxel.SpherizeRadius.Value;
				myRenderableProxy.VoxelCommonObjectData.SpherizeStart = m_voxel.SpherizeRadius.Value * 1.1f;
			}
			else
			{
				myRenderableProxy.VoxelCommonObjectData.SpherizeTo = float.NaN;
				myRenderableProxy.VoxelCommonObjectData.SpherizeStart = float.NaN;
			}
			myRenderableProxy.NonVoxelObjectData.FoliageMultiplier = 1f / (float)Math.Pow(2.0, m_lod);
			MyStringId shaderMaterial = MyMaterialShaders.MapTechniqueToShaderMaterial(technique);
			MyStringId forwardMaterial = MyMaterialShaders.MapTechniqueToShaderMaterial(MyMeshDrawTechnique.VOXEL_MAP_SINGLE);
			m_mesh.AssignLodMeshToProxy(myRenderableProxy);
			MyShaderUnifiedFlags myShaderUnifiedFlags = MyShaderUnifiedFlags.NONE;
			if (MyRender11.Settings.User.VoxelShaderQuality == MyRenderQualityEnum.LOW)
			{
				myShaderUnifiedFlags = MyShaderUnifiedFlags.LQ;
			}
			else if (MyRender11.Settings.User.VoxelShaderQuality == MyRenderQualityEnum.NORMAL)
			{
				myShaderUnifiedFlags = MyShaderUnifiedFlags.MQ;
			}
			AssignShadersToProxy(myRenderableProxy, shaderMaterial, myRenderLod.VertexLayout1, myRenderLod.VertexShaderFlags | MyRenderableComponent.MapTechniqueToShaderMaterialFlags(technique) | myShaderUnifiedFlags, forwardMaterial);
			MyDrawSubmesh myDrawSubmesh;
			MyDrawSubmesh drawSubmesh;
			if (shadowsOnly)
			{
				myDrawSubmesh = default(MyDrawSubmesh);
				myDrawSubmesh.BaseVertex = 0;
				myDrawSubmesh.StartIndex = 0;
				myDrawSubmesh.IndexCount = m_mesh.GetIndexCount();
				myDrawSubmesh.BonesMapping = null;
				myDrawSubmesh.MaterialId = MyMaterialProxyId.NULL;
				myDrawSubmesh.Flags = MyDrawSubmesh.MySubmeshFlags.Depth;
				drawSubmesh = myDrawSubmesh;
			}
			else
			{
				myDrawSubmesh = default(MyDrawSubmesh);
				myDrawSubmesh.BaseVertex = 0;
				myDrawSubmesh.StartIndex = baseIndex;
				myDrawSubmesh.IndexCount = indexCount;
				myDrawSubmesh.BonesMapping = null;
				myDrawSubmesh.MaterialId = MyMaterialProxyId.NULL;
				myDrawSubmesh.Flags = MyDrawSubmesh.MySubmeshFlags.Gbuffer | MyDrawSubmesh.MySubmeshFlags.Forward;
				drawSubmesh = myDrawSubmesh;
			}
			myRenderableProxy.DrawSubmesh = drawSubmesh;
			myRenderableProxy.SkinningMatrices = null;
			myRenderableProxy.ObjectBufferSize = objectConstantsSize;
			myRenderableProxy.ObjectBufferSizeAligned = MathHelper.Align(objectConstantsSize, 256);
			myRenderableProxy.InstanceCount = m_instanceCount;
			myRenderableProxy.StartInstance = m_startInstance;
			myRenderableProxy.Flags = MyRenderableComponent.MapTechniqueToRenderableFlags(technique) | GetRenderAdditionalFlags();
			myRenderableProxy.Type = MyRenderableComponent.MapTechniqueToMaterialType(technique);
			myRenderableProxy.Parent = this;
			myRenderableProxy.Instancing = m_instancing;
			myRenderableProxy.UpdateTechniqueGbuffer();
			bool farCull = (myRenderableProxy.Flags & MyRenderableProxyFlags.DrawOutsideViewDistance) > MyRenderableProxyFlags.None;
			m_cullData.FarCull = farCull;
			ulong key = 0uL;
			My64BitValueHelper.SetBits(ref key, 50, 1, (ulong)(int)(MyMaterialShaders.IsDecal(technique) ? 1u : 0u));
			My64BitValueHelper.SetBits(ref key, 45, 5, (ulong)MyMaterialShaders.MapTechniqueToSortKey(technique));
			My64BitValueHelper.SetBits(ref key, 20, 25, (ulong)myRenderLod.VertexShaderFlags);
			myRenderLod.SortingKeys[proxyIndex] = key;
		}

		internal override void UpdateAfterCull()
		{
			Log("Render");
			base.UpdateAfterCull();
			if (!m_voxel.Spherize)
			{
				return;
			}
			Vector3D spherizeCenter = m_voxel.SpherizeCenter;
			Vector3 spherizeCenter2 = new Vector3((float)(spherizeCenter.X - MyRender11.Environment.Matrices.CameraPosition.X), (float)(spherizeCenter.Y - MyRender11.Environment.Matrices.CameraPosition.Y), (float)(spherizeCenter.Z - MyRender11.Environment.Matrices.CameraPosition.Z));
			MyRenderLod[] lods = m_lods;
			for (int i = 0; i < lods.Length; i++)
			{
				MyRenderableProxy[] renderableProxies = lods[i].RenderableProxies;
				foreach (MyRenderableProxy obj in renderableProxies)
				{
					obj.VoxelCommonObjectData.SpherizeCenter = spherizeCenter2;
					obj.VoxelCommonObjectData.VoxelLodSize = m_lod;
				}
			}
		}

		public override void Destruct()
		{
			Log("Destruct");
			base.Destruct();
			MyAlphaTransition.Remove(this, executeCompletion: false);
			m_voxel.NotifyChildClose(this);
			m_voxel = null;
			m_voxelMesh.Destroy();
			m_voxelMesh = default(MyRenderVoxelMesh);
			m_visible = false;
		}

		private MyVoxelCellComponent Move()
		{
			MyActor myActor = MyActorFactory.Create($"Clipmap Cell Surrogate {m_offset}:{m_lod}");
			MyVoxelCellComponent myVoxelCellComponent = MyComponentFactory<MyVoxelCellComponent>.Create();
			myActor.AddComponent<MyRenderableComponent>(myVoxelCellComponent);
			myVoxelCellComponent.MoveFrom(this);
			m_voxelMesh = new MyRenderVoxelMesh((Vector3I)m_offset, m_lod);
			SetModel(m_voxelMesh.Mesh);
			return myVoxelCellComponent;
		}

		private void MoveFrom(MyVoxelCellComponent moveFrom)
		{
			m_voxel = moveFrom.m_voxel;
			m_voxelMesh = moveFrom.m_voxelMesh;
			m_lod = moveFrom.Lod;
			m_notify = false;
			m_offset = moveFrom.Offset;
			SetModel(m_voxelMesh.Mesh);
			UpdateParametersFromEntity();
		}

		public void SetNotify(bool notify)
		{
			m_notify = notify;
		}

		public override void TransitionStart(MyAlphaTransitionDirection direction)
		{
			Log("TransitionStart " + direction);
			base.TransitionStart(direction);
			m_voxel.RegisterForExplicitUpdate(this);
		}

		public override void TransitionComplete(MyAlphaTransitionDirection direction, Action<uint> transitionFinishedCallback = null)
		{
			Log("TransitionComplete " + direction);
			if (m_notify)
			{
				m_voxel.NotifyTransition(Lod, Offset, direction == MyAlphaTransitionDirection.FadeIn);
				m_notify = false;
			}
			m_voxel.RemoveFromExplicitUpdate(this);
			if (direction == MyAlphaTransitionDirection.FadeOut)
			{
				if (!m_voxel.Contains(this))
				{
					Log("TransitionComplete / destroy!");
					base.Owner.Destroy();
				}
				else
				{
					base.Owner.SetVisibility(visibility: false);
				}
			}
		}

		public void UpdateMesh(ref MyVoxelRenderCellData data, ref IMyVoxelUpdateBatch updateBatch)
		{
			Log(string.Concat("UpdateMesh transition ", m_voxel.TransitionMode, " / visible ", m_visible.ToString(), " / ready ", m_voxelMesh.Ready.ToString(), " / batching ", m_voxel.IsBatching.ToString()), data.CellBounds);
			if (m_voxel.TransitionMode == MyVoxelActorTransitionMode.Fade)
			{
				bool num = m_visible && m_voxelMesh.Ready;
				if (num)
				{
					MyVoxelCellComponent myVoxelCellComponent = Move();
					myVoxelCellComponent.SetVisibleImmediate(visible: true);
					MyAlphaTransition.Clone(this, myVoxelCellComponent);
					myVoxelCellComponent.SetVisible(visible: false);
					myVoxelCellComponent.Log("Cloned from " + base.Owner.GetHashCode(), data.CellBounds);
					MyManagers.FoliageManager.UpdateFoliage(myVoxelCellComponent.Owner);
				}
				m_voxelMesh.UpdateCell(ref data, GetUpdateBatch(ref updateBatch));
				base.Owner.SetLocalAabb(m_voxelMesh.Aabb);
				if (m_visible)
				{
					MarkDirty();
					MyManagers.FoliageManager.UpdateFoliage(base.Owner);
				}
				if (num)
				{
					SetVisibleInternal(visible: true);
				}
			}
			else
			{
				m_voxelMesh.UpdateCell(ref data, m_voxel.IsBatching ? null : GetUpdateBatch(ref updateBatch));
				base.Owner.SetLocalAabb(m_voxelMesh.Aabb);
				if (m_visible)
				{
					if (m_voxel.IsBatching)
					{
						m_voxel.QueueCellDataUpdate(this);
					}
					else
					{
						MarkDirty();
					}
				}
			}
			m_materialsUsage.Clear();
			MyVoxelMeshPartIndex[] parts = data.Parts;
			for (int i = 0; i < parts.Length; i++)
			{
				MyVoxelMeshPartIndex myVoxelMeshPartIndex = parts[i];
				if (myVoxelMeshPartIndex.Materials.I0 != byte.MaxValue)
				{
<<<<<<< HEAD
					m_materialsUsage[myVoxelMeshPartIndex.Materials.I0] = m_materialsUsage.GetValueOrDefault(myVoxelMeshPartIndex.Materials.I0, 0) + myVoxelMeshPartIndex.IndexCount;
				}
				if (myVoxelMeshPartIndex.Materials.I1 != byte.MaxValue)
				{
					m_materialsUsage[myVoxelMeshPartIndex.Materials.I1] = m_materialsUsage.GetValueOrDefault(myVoxelMeshPartIndex.Materials.I1, 0) + myVoxelMeshPartIndex.IndexCount;
				}
				if (myVoxelMeshPartIndex.Materials.I2 != byte.MaxValue)
				{
					m_materialsUsage[myVoxelMeshPartIndex.Materials.I2] = m_materialsUsage.GetValueOrDefault(myVoxelMeshPartIndex.Materials.I2, 0) + myVoxelMeshPartIndex.IndexCount;
				}
			}
			IMyVoxelUpdateBatch GetUpdateBatch(ref IMyVoxelUpdateBatch batch)
=======
					m_materialsUsage[myVoxelMeshPartIndex.Materials.I0] = m_materialsUsage.GetValueOrDefault((int)myVoxelMeshPartIndex.Materials.I0, 0) + myVoxelMeshPartIndex.IndexCount;
				}
				if (myVoxelMeshPartIndex.Materials.I1 != byte.MaxValue)
				{
					m_materialsUsage[myVoxelMeshPartIndex.Materials.I1] = m_materialsUsage.GetValueOrDefault((int)myVoxelMeshPartIndex.Materials.I1, 0) + myVoxelMeshPartIndex.IndexCount;
				}
				if (myVoxelMeshPartIndex.Materials.I2 != byte.MaxValue)
				{
					m_materialsUsage[myVoxelMeshPartIndex.Materials.I2] = m_materialsUsage.GetValueOrDefault((int)myVoxelMeshPartIndex.Materials.I2, 0) + myVoxelMeshPartIndex.IndexCount;
				}
			}
			static IMyVoxelUpdateBatch GetUpdateBatch(ref IMyVoxelUpdateBatch batch)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (batch == null)
				{
					batch = MyMeshes.OpenVoxelBatch();
				}
				return batch;
			}
		}

		public void SetAlpha(float alpha)
		{
			SetDithering(alpha);
		}

		[Conditional("VRAGE")]
		private void Log(string s, BoundingBox? localAabb = null)
		{
			bool flag = false;
			Vector3D point = new Vector3D(-4.16980591511625, -28.8398439040567, 102.316871691444);
			if (base.Owner == null)
			{
				return;
			}
			if (!localAabb.HasValue)
			{
				if (base.Owner.WorldAabb.Contains(point) == ContainmentType.Contains)
				{
					flag = true;
				}
				return;
			}
			CalculateWorldMatrix(out var worldMatrix);
			if (localAabb.Value.Transform(worldMatrix).Contains(point) == ContainmentType.Contains)
			{
				flag = true;
			}
		}

		public void SetVisible(bool visible, bool notify = false)
		{
			Log("SetVisible " + m_visible + " -> " + visible);
			if (m_visible != visible)
			{
				SetVisibleInternal(visible);
			}
		}

		private void SetVisibleInternal(bool visible)
		{
			m_visible = visible;
			if (m_voxelMesh.Ready)
			{
				if (m_voxel.IsBatching)
				{
					m_voxel.QueueCellVisibilityChange(this, visible);
				}
				else
				{
					UpdateVisibilityInternal(visible);
				}
			}
		}

		private void SetVisibleImmediate(bool visible)
		{
			m_visible = visible;
			base.Owner.SetVisibility(visible);
		}

		internal void UpdateVisibilityInternal(bool visible)
		{
			Log("UpdateVisibilityInternal " + visible);
			if (m_voxel.TransitionMode == MyVoxelActorTransitionMode.Fade)
			{
				if (visible)
				{
					base.Owner.SetVisibility(visibility: true);
					Log("UpdateVisibilityInternal / MyAlphaTransition FadeIn");
					MyAlphaTransition.Add(this, 1000, MyAlphaTransitionDirection.FadeIn, null, continuePrevious: true);
				}
				else
				{
					Log("UpdateVisibilityInternal / MyAlphaTransition FadeOut");
					MyAlphaTransition.Add(this, 1000, MyAlphaTransitionDirection.FadeOut, null, continuePrevious: false, finishPrevious: true);
				}
			}
			else if (!visible && !m_voxel.Contains(this))
			{
				Log("UpdateVisibilityInternal / Destroy");
				base.Owner.Destroy();
			}
			else
			{
				Log("UpdateVisibilityInternal / SetVisibility ");
				base.Owner.SetVisibility(visible);
			}
		}
	}
}
