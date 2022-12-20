using System.Collections.Generic;
using VRage.Render.Scene;
using VRage.Render11.Resources;
using VRage.Utils;
using VRageRender;

namespace VRage.Render11.Scene.Components
{
	internal class MyMaterialMergeGroup
	{
		private static readonly MyStringId STANDARD_MATERIAL = MyStringId.GetOrCompute("standard");

		private int m_rootMaterialRK;

		private MyMergeInstancing m_mergeGroup;

		private Dictionary<uint, MyActor> m_actors;

		private Dictionary<MyActor, int> m_actorIndices;

		public int Index { get; private set; }

		public MyMergeInstancing MergeGroup => m_mergeGroup;

		internal MyMaterialMergeGroup(MyMeshTableSrv meshTable, MyMeshMaterialId matId, int index)
		{
			m_mergeGroup = new MyMergeInstancing(meshTable);
			m_rootMaterialRK = MyMeshMaterials1.Table[matId.Index].RepresentationKey;
			Index = index;
			m_actors = new Dictionary<uint, MyActor>();
			m_actorIndices = new Dictionary<MyActor, int>();
		}

		public bool TryGetActorIndex(MyActor actor, out int index)
		{
			return m_actorIndices.TryGetValue(actor, out index);
		}

		internal void AddEntity(MyActor actor, MeshId model)
		{
			m_actors[actor.ID] = actor;
			m_mergeGroup.AddEntity(actor, model);
		}

		internal void RemoveEntity(MyActor actor)
		{
			m_actors.Remove(actor.ID);
			m_mergeGroup.RemoveEntity(actor);
		}

		internal void UpdateEntity(MyActor actor)
		{
			m_mergeGroup.UpdateEntity(actor, actor.GetRenderable().DepthBias);
		}

		internal void UpdateAll()
		{
			foreach (MyActor value in m_actors.Values)
			{
				UpdateEntity(value);
			}
		}

		internal void BuildProxy(out MyRenderableProxy_2 proxy, out ulong key)
		{
			MyMaterialProxyId proxyId = MyMeshMaterials1.GetProxyId(MyMeshMaterials1.MaterialRkIndex.Get(m_rootMaterialRK, MyMeshMaterialId.NULL));
			MyRenderableProxy_2 myRenderableProxy_ = new MyRenderableProxy_2
			{
				MaterialType = MyMaterialType.OPAQUE,
				ObjectConstants = default(MyConstantsPack),
				ObjectSrvs = new MySrvTable
				{
					StartSlot = 13,
					Srvs = m_mergeGroup.m_srvs,
					BindFlag = MyBindFlag.BIND_VS,
					Version = GetHashCode()
				},
				DepthShaders = GetMergeInstancing(MyMaterialShaders.DEPTH_PASS_ID, MyShaderUnifiedFlags.DEPTH_ONLY),
				HighlightShaders = GetMergeInstancing(MyMaterialShaders.HIGHLIGHT_PASS_ID),
				Shaders = GetMergeInstancing(MyMaterialShaders.GBUFFER_PASS_ID),
				ForwardShaders = GetMergeInstancing(MyMaterialShaders.FORWARD_PASS_ID, MyShaderUnifiedFlags.USE_SHADOW_CASCADES),
				RenderFlags = MyRenderableProxyFlags.DepthSkipTextures
			};
			MyDrawSubmesh_2[] array = new MyDrawSubmesh_2[1];
			MyDrawSubmesh_2 myDrawSubmesh_ = new MyDrawSubmesh_2
			{
				DrawCommand = MyDrawCommandEnum.Draw,
				Count = m_mergeGroup.VerticesNum,
				MaterialId = proxyId
			};
			array[0] = myDrawSubmesh_;
			myRenderableProxy_.Submeshes = array;
			MyDrawSubmesh_2[] array2 = new MyDrawSubmesh_2[1];
			myDrawSubmesh_ = new MyDrawSubmesh_2
			{
				DrawCommand = MyDrawCommandEnum.Draw,
				Count = m_mergeGroup.VerticesNum,
				MaterialId = proxyId
			};
			array2[0] = myDrawSubmesh_;
			myRenderableProxy_.SubmeshesDepthOnly = array2;
			myRenderableProxy_.InstanceCount = 0;
			myRenderableProxy_.StartInstance = 0;
			proxy = myRenderableProxy_;
			key = 0uL;
		}

		private static MyMergeInstancingShaderBundle GetMergeInstancing(MyStringId pass, MyShaderUnifiedFlags flags = MyShaderUnifiedFlags.NONE)
		{
			MyMergeInstancingShaderBundle result = default(MyMergeInstancingShaderBundle);
			flags |= MyShaderUnifiedFlags.USE_MERGE_INSTANCING;
			result.MultiInstance = MyMaterialShaders.Get(STANDARD_MATERIAL, pass, MyVertexLayouts.Empty, flags, MyFileTextureEnum.UNSPECIFIED);
			result.SingleInstance = MyMaterialShaders.Get(STANDARD_MATERIAL, pass, MyVertexLayouts.Empty, flags | MyShaderUnifiedFlags.USE_SINGLE_INSTANCE, MyFileTextureEnum.UNSPECIFIED);
			return result;
		}

		internal void UpdateProxySubmeshes(ref MyRenderableProxy_2 proxy, bool rootGroupDirtyTree)
		{
			if (m_mergeGroup.TableDirty)
			{
				proxy.Submeshes[0].Count = m_mergeGroup.VerticesNum;
				proxy.SubmeshesDepthOnly[0].Count = m_mergeGroup.VerticesNum;
				UpdateProxySectionSubmeshes(ref proxy);
			}
			else if (rootGroupDirtyTree)
			{
				UpdateProxySectionSubmeshes(ref proxy);
			}
		}

		private void UpdateProxySectionSubmeshes(ref MyRenderableProxy_2 proxy)
		{
			int filledSize;
			MyInstanceEntityInfo[] entityInfos = m_mergeGroup.GetEntityInfos(out filledSize);
			proxy.SectionSubmeshes = new MyDrawSubmesh_2[filledSize][];
			m_actorIndices.Clear();
			int num = 0;
			for (int i = 0; i < filledSize; i++)
			{
				MyInstanceEntityInfo myInstanceEntityInfo = entityInfos[i];
				if (myInstanceEntityInfo.EntityId.HasValue)
				{
					MyActor myActor = m_actors[myInstanceEntityInfo.EntityId.Value];
					int indexOffset = myInstanceEntityInfo.PageOffset * m_mergeGroup.TablePageSize;
					UpdateActorSubmeshes(ref proxy, myActor, num, indexOffset);
					m_actorIndices[myActor] = num;
				}
				num++;
			}
		}

		/// <returns>Actor full mesh indices count</returns>
		private static void UpdateActorSubmeshes(ref MyRenderableProxy_2 proxy, MyActor actor, int actorIndex, int indexOffset)
		{
			MyRenderableProxy myRenderableProxy = actor.GetRenderable().Lods[0].RenderableProxies[0];
			MyDrawSubmesh_2 myDrawSubmesh_ = proxy.Submeshes[0];
			MyDrawSubmesh_2[] array = new MyDrawSubmesh_2[myRenderableProxy.SectionSubmeshes.Length];
			proxy.SectionSubmeshes[actorIndex] = array;
			for (int i = 0; i < myRenderableProxy.SectionSubmeshes.Length; i++)
			{
				MyDrawSubmesh myDrawSubmesh = myRenderableProxy.SectionSubmeshes[i];
				myDrawSubmesh_.Count = myDrawSubmesh.IndexCount;
				myDrawSubmesh_.Start = indexOffset + myDrawSubmesh.StartIndex;
				array[i] = myDrawSubmesh_;
			}
		}

		internal void MoveToGPU()
		{
			m_mergeGroup.MoveToGPU();
		}

		internal void Release()
		{
			m_mergeGroup.OnDeviceReset();
		}
	}
}
