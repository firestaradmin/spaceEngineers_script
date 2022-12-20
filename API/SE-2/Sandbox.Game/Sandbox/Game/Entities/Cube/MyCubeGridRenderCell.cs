using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using VRage;
using VRage.Game;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities.Cube
{
	public class MyCubeGridRenderCell
	{
		private struct EdgeInfoNormal
		{
			public Vector3 Normal;

			public Color Color;

			public MyStringHash SkinSubtypeId;

			public MyStringHash EdgeModel;
		}

		private struct MyInstanceBucket : IEquatable<MyInstanceBucket>
		{
			public int ModelId;

			public MyStringHash SkinSubtypeId;

			public MyInstanceBucket(int modelId, MyStringHash skinSubtypeId)
			{
				ModelId = modelId;
				SkinSubtypeId = skinSubtypeId;
			}

			public bool Equals(MyInstanceBucket other)
			{
				if (ModelId == other.ModelId)
				{
					return SkinSubtypeId == other.SkinSubtypeId;
				}
				return false;
			}
		}

		private struct MyCubeInstanceMergedData
		{
			public MyCubeInstanceData CubeInstanceData;

			public ConcurrentDictionary<uint, bool> Decals;
		}

		private struct MyEdgeRenderData
		{
			public readonly int ModelId;

			public readonly MyFourEdgeInfo EdgeInfo;

			public MyEdgeRenderData(int modelId, MyFourEdgeInfo edgeInfo)
			{
				ModelId = modelId;
				EdgeInfo = edgeInfo;
			}
		}

		private static readonly MyObjectBuilderType m_edgeDefinitionType = new MyObjectBuilderType(typeof(MyObjectBuilder_EdgesDefinition));

		public readonly MyRenderComponentCubeGrid m_gridRenderComponent;

		public readonly float EdgeViewDistance;

		public string DebugName;

		private BoundingBox m_boundingBox = BoundingBox.CreateInvalid();

		private static List<MyCubeInstanceData> m_tmpInstanceData = new List<MyCubeInstanceData>();

		private static readonly Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> m_tmpInstanceParts = new Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)>();

		private static List<MyCubeInstanceDecalData> m_tmpDecalData = new List<MyCubeInstanceDecalData>();

		private uint m_parentCullObject = uint.MaxValue;

		private uint m_instanceBufferId = uint.MaxValue;

		private readonly Dictionary<MyInstanceBucket, MyRenderInstanceInfo> m_instanceInfo = new Dictionary<MyInstanceBucket, MyRenderInstanceInfo>();

		private readonly Dictionary<MyInstanceBucket, uint> m_instanceGroupRenderObjects = new Dictionary<MyInstanceBucket, uint>();

		private readonly ConcurrentDictionary<MyCubePart, ConcurrentDictionary<uint, bool>> m_cubeParts = new ConcurrentDictionary<MyCubePart, ConcurrentDictionary<uint, bool>>();

		private readonly ConcurrentDictionary<long, MyEdgeRenderData> m_edgesToRender = new ConcurrentDictionary<long, MyEdgeRenderData>();

		private readonly ConcurrentDictionary<long, MyFourEdgeInfo> m_dirtyEdges = new ConcurrentDictionary<long, MyFourEdgeInfo>();

		private readonly ConcurrentDictionary<long, MyFourEdgeInfo> m_edgeInfosNew = new ConcurrentDictionary<long, MyFourEdgeInfo>();

		private static readonly int m_edgeTypeCount = MyUtils.GetMaxValueFromEnum<MyCubeEdgeType>() + 1;

		private static readonly Dictionary<MyStringHash, int[]> m_edgeModelIdCache = new Dictionary<MyStringHash, int[]>(MyStringHash.Comparer);

		private static readonly List<EdgeInfoNormal> m_edgesToCompare = new List<EdgeInfoNormal>();

		public ConcurrentDictionary<MyCubePart, ConcurrentDictionary<uint, bool>> CubeParts => m_cubeParts;

		public uint ParentCullObject => m_parentCullObject;

		public MyCubeGridRenderCell(MyRenderComponentCubeGrid gridRender)
		{
			m_gridRenderComponent = gridRender;
			EdgeViewDistance = ((gridRender.GridSizeEnum == MyCubeSize.Large) ? 130 : 35);
		}

		public void AddCubePart(MyCubePart part)
		{
			m_cubeParts.TryAdd(part, (ConcurrentDictionary<uint, bool>)null);
		}

		public bool HasCubePart(MyCubePart part)
		{
			return m_cubeParts.ContainsKey(part);
		}

		public bool HasCubePart(MyCubePart part)
		{
			return m_cubeParts.ContainsKey(part);
		}

		public bool RemoveCubePart(MyCubePart part)
		{
			ConcurrentDictionary<uint, bool> val = default(ConcurrentDictionary<uint, bool>);
			return m_cubeParts.TryRemove(part, ref val);
		}

		internal void AddCubePartDecal(MyCubePart part, uint decalId)
		{
			m_cubeParts.GetOrAdd(part, (Func<MyCubePart, ConcurrentDictionary<uint, bool>>)((MyCubePart x) => new ConcurrentDictionary<uint, bool>())).TryAdd(decalId, true);
		}

		internal void RemoveCubePartDecal(MyCubePart part, uint decalId)
		{
<<<<<<< HEAD
			if (m_cubeParts.TryGetValue(part, out var value))
			{
				value.TryRemove(decalId, out var _);
=======
			ConcurrentDictionary<uint, bool> val = default(ConcurrentDictionary<uint, bool>);
			if (m_cubeParts.TryGetValue(part, ref val))
			{
				bool flag = default(bool);
				val.TryRemove(decalId, ref flag);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool AddEdgeInfo(long hash, MyEdgeInfo info, MySlimBlock owner)
		{
<<<<<<< HEAD
			if (!m_edgeInfosNew.TryGetValue(hash, out var value))
=======
			MyFourEdgeInfo orAdd = default(MyFourEdgeInfo);
			if (!m_edgeInfosNew.TryGetValue(hash, ref orAdd))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				orAdd = new MyFourEdgeInfo(info.LocalOrthoMatrix, info.EdgeType);
				orAdd = m_edgeInfosNew.GetOrAdd(hash, orAdd);
			}
<<<<<<< HEAD
			lock (value)
			{
				bool flag = value.AddInstance(owner.Position * owner.CubeGrid.GridSize, info.Color, owner.SkinSubtypeId, info.EdgeModel, info.PackedNormal0, info.PackedNormal1);
				if (flag)
				{
					if (value.Full)
					{
						m_dirtyEdges.Remove(hash);
						m_edgesToRender.Remove(hash);
						return flag;
					}
					m_dirtyEdges[hash] = value;
=======
			lock (orAdd)
			{
				bool flag = orAdd.AddInstance(owner.Position * owner.CubeGrid.GridSize, info.Color, owner.SkinSubtypeId, info.EdgeModel, info.PackedNormal0, info.PackedNormal1);
				if (flag)
				{
					if (orAdd.Full)
					{
						m_dirtyEdges.Remove<long, MyFourEdgeInfo>(hash);
						m_edgesToRender.Remove<long, MyEdgeRenderData>(hash);
						return flag;
					}
					m_dirtyEdges.set_Item(hash, orAdd);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return flag;
				}
				return flag;
			}
		}

		public bool RemoveEdgeInfo(long hash, MySlimBlock owner)
		{
<<<<<<< HEAD
			if (m_edgeInfosNew.TryGetValue(hash, out var value))
			{
				lock (value)
				{
					bool flag = value.RemoveInstance(owner.Position * owner.CubeGrid.GridSize);
=======
			MyFourEdgeInfo myFourEdgeInfo = default(MyFourEdgeInfo);
			if (m_edgeInfosNew.TryGetValue(hash, ref myFourEdgeInfo))
			{
				lock (myFourEdgeInfo)
				{
					bool flag = myFourEdgeInfo.RemoveInstance(owner.Position * owner.CubeGrid.GridSize);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!flag)
					{
						return flag;
					}
<<<<<<< HEAD
					if (!value.Empty)
					{
						m_dirtyEdges[hash] = value;
						return flag;
					}
					m_dirtyEdges.Remove(hash);
					m_edgeInfosNew.Remove(hash);
					m_edgesToRender.Remove(hash);
=======
					if (!myFourEdgeInfo.Empty)
					{
						m_dirtyEdges.set_Item(hash, myFourEdgeInfo);
						return flag;
					}
					m_dirtyEdges.Remove<long, MyFourEdgeInfo>(hash);
					m_edgeInfosNew.Remove<long, MyFourEdgeInfo>(hash);
					m_edgesToRender.Remove<long, MyEdgeRenderData>(hash);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return flag;
				}
			}
			return false;
		}

		private bool InstanceDataCleared(Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts)
		{
			foreach (KeyValuePair<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instancePart in instanceParts)
			{
				if (instancePart.Value.Item1.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		public void RebuildInstanceParts(RenderFlags renderFlags)
		{
			_ = MySandboxGame.Static.UpdateThread;
<<<<<<< HEAD
			_ = Thread.CurrentThread;
=======
			Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> tmpInstanceParts = m_tmpInstanceParts;
			foreach (var (myCubePart2, decals) in m_cubeParts)
			{
				AddInstancePart(tmpInstanceParts, myCubePart2.Model.UniqueId, myCubePart2.SkinSubtypeId, ref myCubePart2.InstanceData, decals, MyInstanceFlagsEnum.CastShadows | MyInstanceFlagsEnum.ShowLod1 | MyInstanceFlagsEnum.EnableColorMask);
			}
			UpdateDirtyEdges();
			AddEdgeParts(tmpInstanceParts);
			UpdateRenderInstanceData(tmpInstanceParts, renderFlags);
			ClearInstanceParts(tmpInstanceParts);
			if (m_gridRenderComponent != null)
			{
				m_gridRenderComponent.FadeIn = false;
			}
		}

		private bool IsEdgeVisible(MyFourEdgeInfo edgeInfo, out int modelId)
		{
			lock (edgeInfo)
			{
				modelId = 0;
				m_edgesToCompare.Clear();
				if (edgeInfo.Full)
				{
					return false;
				}
				for (int i = 0; i < 4; i++)
				{
					if (edgeInfo.GetNormalInfo(i, out var color, out var skinSubtypeId, out var edgeModel, out var normal, out var normal2))
					{
						_ = edgeModel == MyStringHash.NullOrEmpty;
						List<EdgeInfoNormal> edgesToCompare = m_edgesToCompare;
						EdgeInfoNormal item = new EdgeInfoNormal
						{
							Normal = Base27Directions.GetVector(normal),
							Color = color,
							SkinSubtypeId = skinSubtypeId,
							EdgeModel = edgeModel
						};
						edgesToCompare.Add(item);
						List<EdgeInfoNormal> edgesToCompare2 = m_edgesToCompare;
						item = new EdgeInfoNormal
						{
							Normal = Base27Directions.GetVector(normal2),
							Color = color,
							SkinSubtypeId = skinSubtypeId,
							EdgeModel = edgeModel
						};
						edgesToCompare2.Add(item);
					}
				}
				if (m_edgesToCompare.Count == 0)
				{
					return false;
				}
				bool flag = m_edgesToCompare.Count == 4;
				MyStringHash edgeModel2 = m_edgesToCompare[0].EdgeModel;
				for (int j = 0; j < m_edgesToCompare.Count; j++)
				{
					for (int k = j + 1; k < m_edgesToCompare.Count; k++)
					{
						if (MyUtils.IsZero(m_edgesToCompare[j].Normal + m_edgesToCompare[k].Normal, 0.1f))
						{
							m_edgesToCompare.RemoveAt(k);
							m_edgesToCompare.RemoveAt(j);
							j--;
							break;
						}
					}
				}
				if (m_edgesToCompare.Count == 1)
				{
					return false;
				}
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				if (m_edgesToCompare.Count > 0)
				{
					Color color2 = m_edgesToCompare[0].Color;
					MyStringHash skinSubtypeId2 = m_edgesToCompare[0].SkinSubtypeId;
					edgeModel2 = m_edgesToCompare[0].EdgeModel;
					for (int l = 1; l < m_edgesToCompare.Count; l++)
					{
						EdgeInfoNormal edgeInfoNormal = m_edgesToCompare[l];
						flag2 |= edgeInfoNormal.Color != color2;
						flag4 |= edgeInfoNormal.SkinSubtypeId != skinSubtypeId2;
						flag3 |= edgeModel2 != edgeInfoNormal.EdgeModel;
						if (flag2 || flag3 || flag4)
						{
							break;
						}
					}
				}
				if (m_edgesToCompare.Count == 1 || (!(flag2 || flag3 || flag4) && m_edgesToCompare.Count <= 2 && ((m_edgesToCompare.Count == 0) ? (flag ? 1 : 0) : ((!(Math.Abs(Vector3.Dot(m_edgesToCompare[0].Normal, m_edgesToCompare[1].Normal)) >= 0.85f)) ? 1 : 0)) == 0))
				{
					return false;
				}
				int edgeTypeCount = m_edgeTypeCount;
				if (!m_edgeModelIdCache.TryGetValue(edgeModel2, out var value))
				{
					MyDefinitionId id = new MyDefinitionId(m_edgeDefinitionType, edgeModel2);
					MyEdgesDefinition edgesDefinition = MyDefinitionManager.Static.GetEdgesDefinition(id);
					MyEdgesModelSet small = edgesDefinition.Small;
					MyEdgesModelSet large = edgesDefinition.Large;
					value = new int[m_edgeTypeCount * 2];
					MyCubeEdgeType[] values = MyEnum<MyCubeEdgeType>.Values;
					foreach (MyCubeEdgeType myCubeEdgeType in values)
					{
						int num;
						int num2;
						switch (myCubeEdgeType)
						{
						case MyCubeEdgeType.Horizontal:
							num = MyModel.GetId(small.Horisontal);
							num2 = MyModel.GetId(large.Horisontal);
							break;
						case MyCubeEdgeType.Horizontal_Diagonal:
							num = MyModel.GetId(small.HorisontalDiagonal);
							num2 = MyModel.GetId(large.HorisontalDiagonal);
							break;
						case MyCubeEdgeType.Vertical:
							num = MyModel.GetId(small.Vertical);
							num2 = MyModel.GetId(large.Vertical);
							break;
						case MyCubeEdgeType.Vertical_Diagonal:
							num = MyModel.GetId(small.VerticalDiagonal);
							num2 = MyModel.GetId(large.VerticalDiagonal);
							break;
						case MyCubeEdgeType.Hidden:
							num = 0;
							num2 = 0;
							break;
						default:
							throw new Exception("Unhandled edge type");
						}
						int num3 = (int)myCubeEdgeType;
						value[num3] = num;
						value[num3 + edgeTypeCount] = num2;
					}
					m_edgeModelIdCache.Add(edgeModel2, value);
				}
				int edgeType = (int)edgeInfo.EdgeType;
				int num4 = ((m_gridRenderComponent.GridSizeEnum == MyCubeSize.Large) ? edgeTypeCount : 0);
				modelId = value[num4 + edgeType];
			}
			return true;
		}

		private void UpdateDirtyEdges()
		{
			foreach (KeyValuePair<long, MyFourEdgeInfo> dirtyEdge in m_dirtyEdges)
			{
				LinqExtensions.Deconstruct(dirtyEdge, out var k, out var v);
<<<<<<< HEAD
				long key = k;
=======
				long num = k;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyFourEdgeInfo myFourEdgeInfo = v;
				int timestamp = myFourEdgeInfo.Timestamp;
				if (IsEdgeVisible(myFourEdgeInfo, out var modelId))
				{
<<<<<<< HEAD
					m_edgesToRender[key] = new MyEdgeRenderData(modelId, myFourEdgeInfo);
				}
				else
				{
					m_edgesToRender.Remove(key);
				}
				m_dirtyEdges.Remove(key);
				if (timestamp != myFourEdgeInfo.Timestamp)
				{
					m_dirtyEdges.TryAdd(key, myFourEdgeInfo);
=======
					m_edgesToRender.set_Item(num, new MyEdgeRenderData(modelId, myFourEdgeInfo));
				}
				else
				{
					m_edgesToRender.Remove<long, MyEdgeRenderData>(num);
				}
				m_dirtyEdges.Remove<long, MyFourEdgeInfo>(num);
				if (timestamp != myFourEdgeInfo.Timestamp)
				{
					m_dirtyEdges.TryAdd(num, myFourEdgeInfo);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void AddEdgeParts(Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts)
		{
			MyCubeInstanceData instance = default(MyCubeInstanceData);
			instance.ResetBones();
			instance.SetTextureOffset(new Vector4UByte(0, 0, 1, 1));
			foreach (KeyValuePair<long, MyEdgeRenderData> item in m_edgesToRender)
			{
				int modelId = item.Value.ModelId;
				MyFourEdgeInfo edgeInfo = item.Value.EdgeInfo;
				edgeInfo.GetNormalInfo(edgeInfo.FirstAvailable, out var color, out var skinSubtypeId, out var _, out var _, out var _);
				instance.PackedOrthoMatrix = edgeInfo.LocalOrthoMatrix;
				instance.ColorMaskHSV = new Vector4(color.ColorToHSVDX11(), 0f);
				AddInstancePart(instanceParts, modelId, skinSubtypeId, ref instance, null, (MyInstanceFlagsEnum)0, EdgeViewDistance);
			}
		}

		private void ClearInstanceParts(Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts)
		{
			m_boundingBox = BoundingBox.CreateInvalid();
			foreach (KeyValuePair<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instancePart in instanceParts)
			{
				instancePart.Value.Item1.Clear();
			}
		}

		private void AddInstancePart(Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts, int modelId, MyStringHash skinSubtypeId, ref MyCubeInstanceData instance, ConcurrentDictionary<uint, bool> decals, MyInstanceFlagsEnum flags, float maxViewDistance = float.MaxValue)
		{
			MyInstanceBucket key = new MyInstanceBucket(modelId, skinSubtypeId);
			if (!instanceParts.TryGetValue(key, out var value))
			{
				value = (new List<MyCubeInstanceMergedData>(), new MyInstanceInfo(flags, maxViewDistance));
				instanceParts.Add(key, value);
			}
			Vector3 translation = instance.LocalMatrix.Translation;
			BoundingBox box = new BoundingBox(translation - new Vector3(m_gridRenderComponent.GridSize), translation + new Vector3(m_gridRenderComponent.GridSize));
			m_boundingBox.Include(ref box);
			value.Item1.Add(new MyCubeInstanceMergedData
			{
				CubeInstanceData = instance,
				Decals = decals
			});
		}

		private void AddRenderObjectId(bool registerForPositionUpdates, out uint renderObjectId, uint newId)
		{
			renderObjectId = newId;
			MyEntities.AddRenderObjectToMap(newId, m_gridRenderComponent.Container.Entity);
			if (!registerForPositionUpdates)
			{
				return;
			}
			try
			{
				while (true)
				{
					uint[] renderObjectIDs = m_gridRenderComponent.RenderObjectIDs;
					for (int i = 0; i < renderObjectIDs.Length; i++)
					{
						if (renderObjectIDs[i] == uint.MaxValue)
						{
							renderObjectIDs[i] = renderObjectId;
							return;
						}
					}
					m_gridRenderComponent.ResizeRenderObjectArray(renderObjectIDs.Length + 3);
				}
			}
			finally
			{
				m_gridRenderComponent.SetVisibilityUpdates(state: true);
			}
		}

		private void RemoveRenderObjectId(bool unregisterFromPositionUpdates, ref uint renderObjectId, MyRenderProxy.ObjectType type)
		{
			MyEntities.RemoveRenderObjectFromMap(renderObjectId);
			MyRenderProxy.RemoveRenderObject(renderObjectId, type, m_gridRenderComponent.FadeOut);
			if (unregisterFromPositionUpdates)
			{
				uint[] renderObjectIDs = m_gridRenderComponent.RenderObjectIDs;
				for (int i = 0; i < renderObjectIDs.Length; i++)
				{
					if (renderObjectIDs[i] == renderObjectId)
					{
						renderObjectIDs[i] = uint.MaxValue;
						break;
					}
				}
			}
			renderObjectId = uint.MaxValue;
		}

		private void UpdateRenderInstanceData(Dictionary<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts, RenderFlags renderFlags)
		{
			if (m_parentCullObject == uint.MaxValue)
			{
				AddRenderObjectId(registerForPositionUpdates: true, out m_parentCullObject, MyRenderProxy.CreateManualCullObject(m_gridRenderComponent.Container.Entity.DisplayName + " " + m_gridRenderComponent.Container.Entity.EntityId + ", cull object", m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef));
			}
			if (m_instanceBufferId == uint.MaxValue)
			{
				AddRenderObjectId(registerForPositionUpdates: false, out m_instanceBufferId, MyRenderProxy.CreateRenderInstanceBuffer(m_gridRenderComponent.Container.Entity.DisplayName + " " + m_gridRenderComponent.Container.Entity.EntityId + ", instance buffer " + DebugName, MyRenderInstanceBufferType.Cube, m_gridRenderComponent.GetRenderObjectID()));
			}
			m_instanceInfo.Clear();
			m_tmpDecalData.AssertEmpty();
			m_tmpInstanceData.AssertEmpty();
			int num = -1;
			foreach (KeyValuePair<MyInstanceBucket, (List<MyCubeInstanceMergedData>, MyInstanceInfo)> instancePart in instanceParts)
			{
				MyInstanceBucket key = instancePart.Key;
				(List<MyCubeInstanceMergedData>, MyInstanceInfo) value = instancePart.Value;
				m_instanceInfo.Add(key, new MyRenderInstanceInfo(m_instanceBufferId, m_tmpInstanceData.Count, value.Item1.Count, value.Item2.MaxViewDistance, value.Item2.Flags));
				List<MyCubeInstanceMergedData> item = value.Item1;
				for (int i = 0; i < item.Count; i++)
				{
					num++;
					m_tmpInstanceData.Add(item[i].CubeInstanceData);
					ConcurrentDictionary<uint, bool> decals = item[i].Decals;
					if (decals == null)
					{
						continue;
					}
<<<<<<< HEAD
					foreach (uint key2 in decals.Keys)
=======
					foreach (uint key2 in decals.get_Keys())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_tmpDecalData.Add(new MyCubeInstanceDecalData
						{
							DecalId = key2,
							InstanceIndex = num
						});
					}
				}
			}
			if (m_tmpInstanceData.Count > 0)
			{
				MyRenderProxy.UpdateRenderCubeInstanceBuffer(m_instanceBufferId, ref m_tmpInstanceData, (int)((float)m_tmpInstanceData.Count * 1.2f), ref m_tmpDecalData);
			}
			m_tmpDecalData.AssertEmpty();
			m_tmpInstanceData.AssertEmpty();
			UpdateRenderEntitiesInstanceData(renderFlags, m_parentCullObject);
		}

		private void UpdateRenderEntitiesInstanceData(RenderFlags renderFlags, uint parentCullObject)
		{
			foreach (KeyValuePair<MyInstanceBucket, MyRenderInstanceInfo> item in m_instanceInfo)
			{
				uint value;
				bool flag = m_instanceGroupRenderObjects.TryGetValue(item.Key, out value);
				bool flag2 = item.Value.InstanceCount > 0 && (m_gridRenderComponent.Entity?.InScene ?? false);
				RenderFlags renderFlags2 = renderFlags;
				if (!flag && flag2)
				{
					MyDefinitionManager.MyAssetModifiers myAssetModifiers = default(MyDefinitionManager.MyAssetModifiers);
					if (item.Key.SkinSubtypeId != MyStringHash.NullOrEmpty)
					{
						myAssetModifiers = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(item.Key.SkinSubtypeId);
						renderFlags2 = ((!myAssetModifiers.MetalnessColorable) ? (renderFlags2 & ~RenderFlags.MetalnessColorable) : (renderFlags2 | RenderFlags.MetalnessColorable));
					}
					AddRenderObjectId(!MyFakes.MANUAL_CULL_OBJECTS, out value, MyRenderProxy.CreateRenderEntity("CubeGridRenderCell " + m_gridRenderComponent.Container.Entity.DisplayName + " " + m_gridRenderComponent.Container.Entity.EntityId + ", part: " + item.Key, MyModel.GetById(item.Key.ModelId), m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef, MyMeshDrawTechnique.MESH, renderFlags2, CullingOptions.Default, m_gridRenderComponent.GetDiffuseColor(), Vector3.Zero, m_gridRenderComponent.Transparency, item.Value.MaxViewDistance, 0, m_gridRenderComponent.CubeGrid.GridScale, m_gridRenderComponent.Transparency == 0f && m_gridRenderComponent.FadeIn));
					if (myAssetModifiers.SkinTextureChanges != null)
					{
						MyRenderProxy.ChangeMaterialTexture(value, myAssetModifiers.SkinTextureChanges);
					}
					m_instanceGroupRenderObjects[item.Key] = value;
					if (MyFakes.MANUAL_CULL_OBJECTS)
					{
						MyRenderProxy.SetParentCullObject(value, parentCullObject, Matrix.Identity);
					}
				}
				else if (flag && !flag2)
				{
					uint renderObjectId = m_instanceGroupRenderObjects[item.Key];
					RemoveRenderObjectId(!MyFakes.MANUAL_CULL_OBJECTS, ref renderObjectId, MyRenderProxy.ObjectType.Entity);
					m_instanceGroupRenderObjects.Remove(item.Key);
					continue;
				}
				if (flag2)
				{
					MyRenderProxy.SetInstanceBuffer(value, item.Value.InstanceBufferId, item.Value.InstanceStart, item.Value.InstanceCount, m_boundingBox);
				}
			}
		}

		public void OnRemovedFromRender()
		{
			UpdateRenderEntitiesInstanceData((RenderFlags)0, m_parentCullObject);
			if (m_parentCullObject != uint.MaxValue)
			{
				RemoveRenderObjectId(unregisterFromPositionUpdates: true, ref m_parentCullObject, MyRenderProxy.ObjectType.ManualCull);
			}
			if (m_instanceBufferId != uint.MaxValue)
			{
				RemoveRenderObjectId(unregisterFromPositionUpdates: false, ref m_instanceBufferId, MyRenderProxy.ObjectType.InstanceBuffer);
			}
		}

		internal void DebugDraw()
		{
<<<<<<< HEAD
			string text = $"CubeParts:{m_cubeParts.Count}, EdgeParts{m_edgeInfosNew.Count}";
=======
			string text = $"CubeParts:{m_cubeParts.get_Count()}, EdgeParts{m_edgeInfosNew.get_Count()}";
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyRenderProxy.DebugDrawText3D(m_boundingBox.Center + m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef.Translation, text, Color.Red, 0.75f, depthRead: false);
			MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(m_boundingBox.Size) * Matrix.CreateTranslation(m_boundingBox.Center) * m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef, Color.Red.ToVector3(), 0.25f, depthRead: true, smooth: true);
		}
	}
}
