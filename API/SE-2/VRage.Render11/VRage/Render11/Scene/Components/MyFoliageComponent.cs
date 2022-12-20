using System;
using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Scene.Components
{
	internal class MyFoliageComponent : MyActorComponent
	{
		private class VRage_Render11_Scene_Components_MyFoliageComponent_003C_003EActor : IActivator, IActivator<MyFoliageComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyFoliageComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFoliageComponent CreateInstance()
			{
				return new MyFoliageComponent();
			}

			MyFoliageComponent IActivator<MyFoliageComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Dictionary<int, MyFoliageStream> m_streams;

		private bool m_pendingRefresh;

		private readonly MyChildCullTreeData m_cullData;

		public override Color DebugColor => Color.Brown;

		public MyFoliageComponent()
		{
			m_cullData = new MyChildCullTreeData
			{
				Add = delegate(MyCullResultsBase x, bool y)
				{
					((MyCullResults)x).Foliage.Add(this);
				},
				Remove = delegate(MyCullResultsBase x)
				{
					((MyCullResults)x).Foliage.Remove(this);
				},
				DebugColor = () => DebugColor
			};
		}

		public override void Construct()
		{
			base.Construct();
			m_streams = new Dictionary<int, MyFoliageStream>();
			m_pendingRefresh = false;
		}

		internal void Dispose()
		{
			if (m_streams.Count <= 0)
			{
				return;
			}
			m_pendingRefresh = true;
			foreach (MyFoliageStream value in m_streams.Values)
			{
				value.Dispose();
			}
			m_streams.Clear();
			MyManagers.FoliageManager.UnegisterActive(this);
		}

		public override void OnVisibilityChange()
		{
			base.OnVisibilityChange();
			if (!base.Owner.IsVisible)
			{
				RefreshStreams();
			}
		}

		public override void OnRemove(MyActor owner)
		{
			MyManagers.FoliageManager.UnegisterActive(this);
			base.OnRemove(owner);
		}

		public override void Destruct()
		{
			Dispose();
			base.Destruct();
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
			owner.InvalidateCullTreeData();
		}

		public override MyChildCullTreeData GetCullTreeData()
		{
			return m_cullData;
		}

		internal void RefreshStreams()
		{
			m_pendingRefresh = true;
			foreach (MyFoliageStream value in m_streams.Values)
			{
				value.Reset();
			}
		}

		internal void FillStreams(MyFoliageGeneratingPass foliageGeneratingPass)
		{
			if (!base.Owner.IsVisible || (m_streams.Count > 0 && !m_pendingRefresh))
			{
				return;
			}
			MyManagers.FoliageManager.RegisterActive(this);
			m_pendingRefresh = false;
			MeshId model = base.Owner.GetRenderable().GetModel();
			int partsNum = MyMeshes.GetLodMesh(model, 0).Info.PartsNum;
			BoundingBoxD worldAabb = base.Owner.WorldAabb;
			double num = Math.Max(Math.Max(worldAabb.Extents.X, worldAabb.Extents.Y), worldAabb.Extents.Z) * 1.5;
			float num2 = (float)(num * num * 1.2999999523162842);
			float num3 = MyMeshes.GetLodMesh(model, 0).Info.IndicesNum;
			float foliageMultiplier = base.Owner.GetRenderable().Lods[0].RenderableProxies[0].NonVoxelObjectData.FoliageMultiplier;
			for (int i = 0; i < partsNum; i++)
			{
				int spawnCount = 0;
				int spawnCount2 = 0;
				int spawnCount3 = 0;
				MyVoxelPartInfo1 info = MyMeshes.GetVoxelPart(model, i).Info;
				float num4 = foliageMultiplier * MyRender11.Settings.User.GrassDensityFactor * ((info.MaterialTriple.I0 != byte.MaxValue && MyVoxelMaterials.Table[info.MaterialTriple.I0].Foliage != null) ? MyVoxelMaterials.Table[info.MaterialTriple.I0].Foliage.BoxedValue.Density : 0f);
				float num5 = foliageMultiplier * MyRender11.Settings.User.GrassDensityFactor * ((info.MaterialTriple.I1 != byte.MaxValue && MyVoxelMaterials.Table[info.MaterialTriple.I1].Foliage != null) ? MyVoxelMaterials.Table[info.MaterialTriple.I1].Foliage.BoxedValue.Density : 0f);
				float num6 = foliageMultiplier * MyRender11.Settings.User.GrassDensityFactor * ((info.MaterialTriple.I2 != byte.MaxValue && MyVoxelMaterials.Table[info.MaterialTriple.I2].Foliage != null) ? MyVoxelMaterials.Table[info.MaterialTriple.I2].Foliage.BoxedValue.Density : 0f);
				if (num4 != 0f || num5 != 0f || num6 != 0f)
				{
					float num7 = (float)info.IndexCount / num3;
					spawnCount = (int)(num4 * num2 * num7);
					spawnCount2 = (int)(num5 * num2 * num7);
					spawnCount3 = (int)(num6 * num2 * num7);
				}
				int num8 = ((info.MaterialTriple.I2 != byte.MaxValue) ? 3 : 2);
				int num9 = IsGrass(info.MaterialTriple.I0) + IsGrass(info.MaterialTriple.I1) + IsGrass(info.MaterialTriple.I2);
				bool skipGrass = info.TriplanarMulti && num9 < num8;
				PrepareStream(info.MaterialTriple.I0, skipGrass, spawnCount);
				PrepareStream(info.MaterialTriple.I1, skipGrass, spawnCount2);
				PrepareStream(info.MaterialTriple.I2, skipGrass, spawnCount3);
			}
			foreach (MyFoliageStream value in m_streams.Values)
			{
				value.AllocateStreamOutBuffer();
			}
			for (int j = 0; j < partsNum; j++)
			{
				MyVoxelPartInfo1 info2 = MyMeshes.GetVoxelPart(model, j).Info;
				int num10 = ((info2.MaterialTriple.I2 != byte.MaxValue) ? 3 : 2);
				int num11 = IsGrass(info2.MaterialTriple.I0) + IsGrass(info2.MaterialTriple.I1) + IsGrass(info2.MaterialTriple.I2);
				bool skipGrass2 = info2.TriplanarMulti && num11 < num10;
				FillStream(foliageGeneratingPass, info2.MaterialTriple.I0, 0, info2, skipGrass2);
				FillStream(foliageGeneratingPass, info2.MaterialTriple.I1, 1, info2, skipGrass2);
				FillStream(foliageGeneratingPass, info2.MaterialTriple.I2, 2, info2, skipGrass2);
			}
		}

		private static int IsGrass(byte materialIdx)
		{
			if (materialIdx == byte.MaxValue || MyVoxelMaterials.Table[materialIdx].Foliage == null || MyVoxelMaterials.Table[materialIdx].Foliage.BoxedValue.Type != 0)
			{
				return 0;
			}
			return 1;
		}

		private void FillStream(MyFoliageGeneratingPass foliageGeneratingPass, byte materialIdx, int vertexMaterialIndex, MyVoxelPartInfo1 partInfo, bool skipGrass)
		{
			if (materialIdx != byte.MaxValue && MyVoxelMaterials.Table[materialIdx].HasFoliage && (!skipGrass || MyVoxelMaterials.Table[materialIdx].Foliage.BoxedValue.Type != 0))
			{
				FillStreamWithTerrainBatch(foliageGeneratingPass, materialIdx, vertexMaterialIndex, partInfo.IndexCount, partInfo.StartIndex, 0);
			}
		}

		private void PrepareStream(byte materialIdx, bool skipGrass, int spawnCount)
		{
			if (materialIdx != byte.MaxValue && MyVoxelMaterials.Table[materialIdx].HasFoliage && (!skipGrass || MyVoxelMaterials.Table[materialIdx].Foliage.BoxedValue.Type != 0))
			{
				PrepareStream(materialIdx, spawnCount);
			}
		}

		private void PrepareStream(int materialId, int spawnCount)
		{
			MyFoliageStream valueOrDefault = m_streams.GetValueOrDefault(materialId);
			if (valueOrDefault == null)
			{
				m_streams.SetDefault(materialId, new MyFoliageStream()).Reserve(spawnCount);
			}
			else
			{
				valueOrDefault.Reserve(spawnCount);
			}
		}

		private void FillStreamWithTerrainBatch(MyFoliageGeneratingPass foliageGeneratingPass, int materialId, int vertexMaterialIndex, int indexCount, int startIndex, int baseVertex)
		{
			MyRenderableComponent renderable = base.Owner.GetRenderable();
			MyRenderableProxy myRenderableProxy = renderable.Lods[0].RenderableProxies[0];
			MyFileTextureEnum textureTypes = ((!(myRenderableProxy.Material == MyMeshMaterialId.NULL)) ? myRenderableProxy.Material.Info.TextureTypes : MyFileTextureEnum.UNSPECIFIED);
			MyMaterialShadersBundleId myMaterialShadersBundleId = MyMaterialShaders.Get(MyMaterialShaders.TRIPLANAR_MULTI_MATERIAL_TAG, MyMaterialShaders.FOLIAGE_STREAMING_PASS_ID, MyMeshes.VoxelLayout, renderable.Lods[0].VertexShaderFlags & ~MyShaderUnifiedFlags.USE_VOXEL_MORPHING, textureTypes);
			foliageGeneratingPass.RecordCommands(myRenderableProxy, m_streams[materialId], materialId, myMaterialShadersBundleId.VS, myMaterialShadersBundleId.IL, vertexMaterialIndex, indexCount, startIndex, baseVertex);
		}

		internal void Render(MyFoliageRenderingPass foliageRenderer)
		{
			if (m_streams.Count == 0)
			{
				return;
			}
			MyRenderableProxy myRenderableProxy = (base.Owner.GetRenderable() as MyVoxelCellComponent).Lods[0].RenderableProxies[0];
			MyObjectDataCommon cod = myRenderableProxy.CommonObjectData;
			MatrixD matrixD = MatrixD.CreateScale(1f / myRenderableProxy.VoxelCommonObjectData.VoxelScale);
			MatrixD worldMatrix = myRenderableProxy.WorldMatrix;
			worldMatrix.Translation -= MyRender11.Environment.Matrices.CameraPosition;
			MatrixD m = matrixD * worldMatrix;
			cod.LocalMatrix = m;
			foreach (KeyValuePair<int, MyFoliageStream> stream in m_streams)
			{
				if (stream.Value.Stream != null)
				{
					foliageRenderer.RecordCommands(myRenderableProxy, ref cod, stream.Value.Stream, stream.Key);
				}
			}
		}
	}
}
