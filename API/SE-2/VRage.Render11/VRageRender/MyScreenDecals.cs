using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal static class MyScreenDecals
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct MyDecalConstants
		{
			[FieldOffset(0)]
			public Matrix WorldMatrix;

			[FieldOffset(48)]
			public float FadeAlpha;

			[FieldOffset(52)]
			public Vector3 __padding;

			[FieldOffset(64)]
			public Matrix InvWorldMatrix;
		}

		private struct MyDecalJob
		{
			public Matrix WorldMatrix;

			public float FadeAlpha;
		}

		private struct MyDecalTextures
		{
			public MyFileTextureEnum DecalType;

			public ITexture ColorMetalTexture;

			public ITexture NormalmapTexture;

			public ITexture ExtensionsTexture;

			public ITexture AlphamaskTexture;
		}

		private class MyScreenDecal
		{
			internal uint FadeTimestamp;

			internal MyDecalTopoData TopoData;

			internal uint ID;

			internal uint[] ParentIDs;

			internal MyDecalFlags Flags;

			internal string SourceTarget;

			internal string Material;

			internal MyStringId MaterialId;

			internal int MaterialIndex;

<<<<<<< HEAD
			internal int TimeUntilLive = int.MaxValue;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			internal float RenderSqDistance;

			internal float OffScreenTime;

			internal bool IsTrail;
<<<<<<< HEAD

			internal bool IgnoresLimit()
			{
				return (Flags & MyDecalFlags.IgnoreRenderLimits) != 0;
			}

			internal bool CanBeDeletedOffscreen()
			{
				return (Flags & MyDecalFlags.IgnoreOffScreenDeletion) == 0;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private struct MyMaterialIdentity : IEquatable<MyMaterialIdentity>
		{
			public MyStringId Material;

			public int Index;

			public bool Equals(MyMaterialIdentity other)
			{
				if (Material == other.Material)
				{
					return Index == other.Index;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return Material.Id ^ Index;
			}
		}

		public static float VISIBLE_DECALS_SQ_TH = 10000f;

		private const int DECAL_BATCH_SIZE = 512;

		private const uint DECAL_FADE_DURATION = 6000u;

		private const float OFFSCREEN_MAX_TIME = 40f;

		private static int m_decalsQueueSize = 1024;

		private static int m_decalsTrailsQueueSize = 1024;

		private static int[] m_maxDecalNumber = new int[3] { 512, 1024, 4096 };

		private static int[] m_maxDecalTrailsNumber = new int[3] { 1024, 2048, 8192 };

		private static float m_qualityFactor = 1f;

		private static IIndexBuffer m_ib;

		private static MyVertexShaders.Id m_vs = MyVertexShaders.Id.NULL;

		private static MyPixelShaders.Id m_psNormalMap = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psColorMap = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psColorMapTransparent = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psNormalColorMap = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_psNormalColorExtMap = MyPixelShaders.Id.NULL;

		private static readonly Dictionary<uint, LinkedListNode<MyScreenDecal>> m_nodeMap = new Dictionary<uint, LinkedListNode<MyScreenDecal>>();

		private static readonly Dictionary<uint, List<LinkedListNode<MyScreenDecal>>> m_entityDecals = new Dictionary<uint, List<LinkedListNode<MyScreenDecal>>>();

		private static readonly LinkedList<MyScreenDecal> m_decals = new LinkedList<MyScreenDecal>();

		private static readonly Dictionary<uint, LinkedListNode<MyScreenDecal>> m_nodeMapTrails = new Dictionary<uint, LinkedListNode<MyScreenDecal>>();

		private static readonly LinkedList<MyScreenDecal> m_decalsTrails = new LinkedList<MyScreenDecal>();

		private static readonly Dictionary<MyStringId, List<MyDecalTextures>> m_materials = new Dictionary<MyStringId, List<MyDecalTextures>>(MyStringId.Comparer);

		private static MyStreamedTexturePin[] m_decalTextures;

		private static readonly Dictionary<MyMaterialIdentity, List<MyScreenDecal>>[] m_materialsToDraw = new Dictionary<MyMaterialIdentity, List<MyScreenDecal>>[2];

		private static readonly DateTime m_startTime = DateTime.Now;

		private static readonly int[] m_drawCount = new int[2];

		private static IConstantBuffer m_cb;

		private static readonly Dictionary<uint, int> m_hasDecals = new Dictionary<uint, int>();

		private static MyFinishedContext m_fc;

		[ThreadStatic]
		private static List<MyDecalJob> m_jobs;

		internal static bool AnyTransparentDecalsToDraw { get; private set; }

<<<<<<< HEAD
		internal static int TotalCount => m_decals.Count;
=======
		internal static int TotalCount => m_decals.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		internal static int DrawCount => m_drawCount[0] + m_drawCount[1];

		internal static int ToRemove { get; private set; }

		internal static void Init()
		{
			m_vs = MyVertexShaders.Create("Decals/Decals.hlsl");
			ShaderMacro shaderMacro = new ShaderMacro("RENDER_TO_TRANSPARENT", null);
			m_psColorMapTransparent = MyPixelShaders.Create("Decals/Decals.hlsl", new ShaderMacro[1] { shaderMacro });
			m_psColorMap = MyPixelShaders.Create("Decals/Decals.hlsl", MyMeshMaterials1.GetMaterialTextureMacros(MyFileTextureEnum.COLOR_METAL));
			m_psNormalMap = MyPixelShaders.Create("Decals/Decals.hlsl", MyMeshMaterials1.GetMaterialTextureMacros(MyFileTextureEnum.NORMALMAP_GLOSS));
			m_psNormalColorMap = MyPixelShaders.Create("Decals/Decals.hlsl", MyMeshMaterials1.GetMaterialTextureMacros(MyFileTextureEnum.COLOR_METAL | MyFileTextureEnum.NORMALMAP_GLOSS));
			m_psNormalColorExtMap = MyPixelShaders.Create("Decals/Decals.hlsl", MyMeshMaterials1.GetMaterialTextureMacros(MyFileTextureEnum.COLOR_METAL | MyFileTextureEnum.NORMALMAP_GLOSS | MyFileTextureEnum.EXTENSIONS));
			m_materialsToDraw[0] = new Dictionary<MyMaterialIdentity, List<MyScreenDecal>>();
			m_materialsToDraw[1] = new Dictionary<MyMaterialIdentity, List<MyScreenDecal>>();
			InitIB();
		}

		internal static void OnSessionEnd()
		{
			ToRemove = 0;
			ClearDecals();
			m_materials.Clear();
			m_materialsToDraw[0].Clear();
			m_materialsToDraw[1].Clear();
		}

		internal static void OnDeviceReset()
		{
			OnDeviceEnd();
			InitIB();
		}

		internal static void OnDeviceEnd()
		{
			if (m_ib != null)
			{
				MyManagers.Buffers.Dispose(m_ib);
			}
			m_ib = null;
			if (m_cb != null)
			{
				MyManagers.Buffers.Dispose(m_cb);
			}
			m_cb = null;
		}

		private unsafe static void InitIB()
		{
			ushort[] array = new ushort[36]
			{
				0, 1, 2, 0, 2, 3, 1, 5, 6, 1,
				6, 2, 5, 4, 7, 5, 7, 6, 4, 0,
				3, 4, 3, 7, 3, 2, 6, 3, 6, 7,
				1, 0, 4, 1, 4, 5
			};
			ushort[] array2 = new ushort[512 * array.Length];
			int num = array.Length;
			for (int i = 0; i < 512; i++)
			{
				for (int j = 0; j < num; j++)
				{
					array2[i * num + j] = (ushort)(array[j] + 8 * i);
				}
			}
			if (m_ib == null)
			{
				fixed (ushort* value = array2)
				{
					m_ib = MyManagers.Buffers.CreateIndexBuffer("MyScreenDecals", array2.Length, new IntPtr(value), MyIndexBufferFormat.UShort, ResourceUsage.Immutable, isGlobal: true);
				}
			}
			m_cb = MyManagers.Buffers.CreateConstantBuffer("DecalCB", sizeof(MyDecalConstants) * 512, null, ResourceUsage.Dynamic, isGlobal: true);
		}

		private static void OnActorDestroyed(MyActor actor)
		{
			RemoveEntityDecals(actor.ID);
		}

<<<<<<< HEAD
		internal static void AddDecal(uint id, uint[] parentIDs, ref MyDecalTopoData topoData, MyDecalFlags flags, string sourceTarget, string material, int matIndex, float renderSqDistance, bool isTrail, int timeUntilLive)
=======
		internal static void AddDecal(uint id, uint[] parentIDs, ref MyDecalTopoData topoData, MyDecalFlags flags, string sourceTarget, string material, int matIndex, float renderSqDistance, bool isTrail)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			MyScreenDecal myScreenDecal = new MyScreenDecal
			{
				FadeTimestamp = uint.MaxValue,
				ID = id,
				ParentIDs = parentIDs,
				TopoData = topoData,
				Flags = flags,
<<<<<<< HEAD
				TimeUntilLive = timeUntilLive,
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				SourceTarget = sourceTarget,
				Material = material,
				MaterialId = X.TEXT_(material),
				MaterialIndex = matIndex,
				RenderSqDistance = renderSqDistance,
				OffScreenTime = 0f,
				IsTrail = isTrail
			};
			if (!CanBePlaced(myScreenDecal))
			{
				return;
			}
			LinkedList<MyScreenDecal> obj = (isTrail ? m_decalsTrails : m_decals);
<<<<<<< HEAD
			LinkedListNode<MyScreenDecal> linkedListNode = obj.First;
			if (obj.Count > GetMaxDecalsNumber(isTrail))
			{
				while (linkedListNode != null && (linkedListNode.Value.FadeTimestamp != uint.MaxValue || linkedListNode.Value.IgnoresLimit()))
				{
					linkedListNode = linkedListNode.Next;
				}
				if (linkedListNode != null)
				{
					MarkForRemove(linkedListNode, isTrail: true);
=======
			LinkedListNode<MyScreenDecal> val = obj.get_First();
			if (obj.get_Count() > GetMaxDecalsNumber(isTrail))
			{
				while (val != null && val.get_Value().FadeTimestamp != uint.MaxValue)
				{
					val = val.get_Next();
				}
				if (val != null)
				{
					MarkForRemove(val, isTrail: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			MyActor myActor = MyIDTracker<MyActor>.FindByID(parentIDs[0]);
			if (myActor != null)
			{
				myActor.OnDestruct -= OnActorDestroyed;
				myActor.OnDestruct += OnActorDestroyed;
			}
			if ((flags & MyDecalFlags.World) == 0)
			{
				if (myActor == null)
				{
					return;
				}
				MatrixD matrix = myActor.WorldMatrix;
				myScreenDecal.TopoData.WorldPosition = Vector3D.Transform(topoData.MatrixCurrent.Translation, ref matrix);
			}
<<<<<<< HEAD
			LinkedListNode<MyScreenDecal> linkedListNode2;
			if (isTrail)
			{
				linkedListNode2 = m_decalsTrails.AddLast(myScreenDecal);
				m_nodeMapTrails[id] = linkedListNode2;
			}
			else
			{
				linkedListNode2 = m_decals.AddLast(myScreenDecal);
				m_nodeMap[id] = linkedListNode2;
=======
			LinkedListNode<MyScreenDecal> val2;
			if (isTrail)
			{
				val2 = m_decalsTrails.AddLast(myScreenDecal);
				m_nodeMapTrails[id] = val2;
			}
			else
			{
				val2 = m_decals.AddLast(myScreenDecal);
				m_nodeMap[id] = val2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (!m_entityDecals.TryGetValue(parentIDs[0], out var value))
			{
				value = new List<LinkedListNode<MyScreenDecal>>();
				m_entityDecals.Add(parentIDs[0], value);
			}
			foreach (uint key in parentIDs)
			{
				if (!m_hasDecals.ContainsKey(key))
				{
					m_hasDecals[key] = 1;
				}
				else
				{
					m_hasDecals[key]++;
				}
			}
<<<<<<< HEAD
			value.Add(linkedListNode2);
=======
			value.Add(val2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void UpdateDecals(IReadOnlyList<MyDecalPositionUpdate> decals)
		{
			uint num = uint.MaxValue;
			MatrixD matrix = default(MatrixD);
			for (int i = 0; i < decals.Count; i++)
			{
				MyDecalPositionUpdate myDecalPositionUpdate = decals[i];
				if (!m_nodeMap.TryGetValue(myDecalPositionUpdate.ID, out var value))
				{
					continue;
				}
<<<<<<< HEAD
				MyScreenDecal value2 = value.Value;
=======
				MyScreenDecal value2 = value.get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if ((value2.Flags & MyDecalFlags.World) > MyDecalFlags.None)
				{
					value2.TopoData.WorldPosition = myDecalPositionUpdate.Position;
				}
				else
				{
					if (num != value2.ParentIDs[0])
					{
						matrix = MyIDTracker<MyActor>.FindByID(value2.ParentIDs[0]).WorldMatrix;
						num = value2.ParentIDs[0];
					}
					value2.TopoData.WorldPosition = Vector3D.Transform(myDecalPositionUpdate.Transform.Translation, ref matrix);
				}
<<<<<<< HEAD
				value.Value.TopoData.MatrixCurrent = myDecalPositionUpdate.Transform;
=======
				value.get_Value().TopoData.MatrixCurrent = myDecalPositionUpdate.Transform;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static int GetMaxDecalsNumber(bool isTrail)
		{
			if (!isTrail)
			{
				return m_decalsQueueSize;
			}
			return m_decalsTrailsQueueSize;
		}

		internal static void UpdateDecalsQuality()
		{
			MyRenderQualityEnum particleQuality = MyRender11.Settings.User.ParticleQuality;
			float num = 1f;
			switch (particleQuality)
			{
			case MyRenderQualityEnum.HIGH:
			case MyRenderQualityEnum.EXTREME:
				num = 1f;
				m_decalsQueueSize = m_maxDecalNumber[2];
				m_decalsTrailsQueueSize = m_maxDecalTrailsNumber[2];
				break;
			case MyRenderQualityEnum.NORMAL:
				num = 0.44444f;
				m_decalsQueueSize = m_maxDecalNumber[1];
				m_decalsTrailsQueueSize = m_maxDecalTrailsNumber[1];
				break;
			default:
				num = 0.11111f;
				m_decalsQueueSize = m_maxDecalNumber[0];
				m_decalsTrailsQueueSize = m_maxDecalTrailsNumber[0];
				break;
			}
			m_qualityFactor = num;
		}

<<<<<<< HEAD
		public static void RemoveDecal(uint id, bool immediately = false)
=======
		public static void RemoveDecal(uint id)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (m_nodeMapTrails.TryGetValue(id, out var value))
			{
				MarkForRemove(value, isTrail: true);
			}
<<<<<<< HEAD
			else if (m_nodeMap.TryGetValue(id, out value))
			{
				MarkForRemove(value, isTrail: false, immediately);
			}
		}

		private static void MarkForRemove(LinkedListNode<MyScreenDecal> decal, bool isTrail = false, bool immediately = false)
		{
			if (immediately)
			{
				RemoveDecalByNode(decal);
			}
			else if (decal.Value.FadeTimestamp == uint.MaxValue)
			{
				ToRemove++;
				uint timeStampSinceStart = GetTimeStampSinceStart();
				decal.Value.FadeTimestamp = timeStampSinceStart + 6000;
=======
			else if (m_nodeMapTrails.TryGetValue(id, out value))
			{
				MarkForRemove(value);
			}
		}

		private static void MarkForRemove(LinkedListNode<MyScreenDecal> decal, bool isTrail = false)
		{
			if (decal.get_Value().FadeTimestamp == uint.MaxValue)
			{
				ToRemove++;
				uint timeStampSinceStart = GetTimeStampSinceStart();
				decal.get_Value().FadeTimestamp = timeStampSinceStart + 6000;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ReinsertNode(decal);
			}
		}

		public static void RemoveEntityDecals(uint id)
		{
			if (!m_entityDecals.TryGetValue(id, out var value))
			{
				return;
			}
			foreach (LinkedListNode<MyScreenDecal> item in value)
			{
<<<<<<< HEAD
				if (item.Value.FadeTimestamp != uint.MaxValue)
=======
				if (item.get_Value().FadeTimestamp != uint.MaxValue)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					ToRemove--;
				}
				RemoveNodeFromLists(item);
<<<<<<< HEAD
				item.Value.Flags |= MyDecalFlags.Closed;
=======
				item.get_Value().Flags |= MyDecalFlags.Closed;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_hasDecals.Remove(id);
			m_entityDecals.Remove(id);
		}

		public static void SetDecalGlobals(MyDecalGlobals globals)
		{
			if (globals.DecalQueueSize != null && globals.DecalQueueSize.Length == 3 && globals.DecalTrailsQueueSize != null && globals.DecalTrailsQueueSize.Length == 3)
			{
				m_maxDecalNumber = globals.DecalQueueSize;
				m_maxDecalTrailsNumber = globals.DecalTrailsQueueSize;
				UpdateDecalsQuality();
			}
		}

		public static void ClearDecals()
		{
			m_nodeMap.Clear();
			m_decals.Clear();
			m_nodeMapTrails.Clear();
			m_decalsTrails.Clear();
			m_entityDecals.Clear();
			m_hasDecals.Clear();
		}

		public static bool HasEntityDecals(uint id)
		{
			return m_hasDecals.ContainsKey(id);
		}

		private static void RemoveDecalByNode(LinkedListNode<MyScreenDecal> node)
		{
<<<<<<< HEAD
			MyScreenDecal value = node.Value;
=======
			MyScreenDecal value = node.get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (value.FadeTimestamp != uint.MaxValue)
			{
				ToRemove--;
			}
			List<LinkedListNode<MyScreenDecal>> list = m_entityDecals[value.ParentIDs[0]];
			list.Remove(node);
			if (list.Count == 0)
			{
				m_entityDecals.Remove(value.ParentIDs[0]);
			}
			RemoveNodeFromLists(node);
			uint[] parentIDs = value.ParentIDs;
			foreach (uint key in parentIDs)
			{
				if (m_hasDecals[key]-- <= 1)
				{
					m_hasDecals.Remove(key);
				}
			}
			value.Flags |= MyDecalFlags.Closed;
			MyRenderProxy.RemoveMessageId(value.ID, MyRenderProxy.ObjectType.ScreenDecal);
		}

		public static void RegisterMaterials(Dictionary<string, List<MyDecalMaterialDesc>> descriptions)
		{
			m_materials.Clear();
			if (m_decalTextures != null)
			{
				MyStreamedTexturePin[] decalTextures = m_decalTextures;
				foreach (MyStreamedTexturePin myStreamedTexturePin in decalTextures)
				{
					myStreamedTexturePin.Dispose();
				}
			}
			MyTextureStreamingManager texManager = MyManagers.Textures;
			HashSet<IMyStreamedTexture> usedTextures = new HashSet<IMyStreamedTexture>();
			foreach (KeyValuePair<string, List<MyDecalMaterialDesc>> description in descriptions)
			{
				List<MyDecalTextures> list = new List<MyDecalTextures>();
				foreach (MyDecalMaterialDesc item in description.Value)
				{
					list.Add(new MyDecalTextures
					{
						DecalType = MyMeshMaterials1.GetMaterialTextureTypes(item.ColorMetalTexture, item.NormalmapTexture, item.ExtensionsTexture, null),
						ColorMetalTexture = GetTexture(item.ColorMetalTexture, MyFileTextureEnum.COLOR_METAL),
						NormalmapTexture = GetTexture(item.NormalmapTexture, MyFileTextureEnum.NORMALMAP_GLOSS),
						ExtensionsTexture = GetTexture(item.ExtensionsTexture, MyFileTextureEnum.EXTENSIONS),
						AlphamaskTexture = GetTexture(item.AlphamaskTexture, MyFileTextureEnum.ALPHAMASK)
					});
				}
				m_materials[X.TEXT_(description.Key)] = list;
			}
<<<<<<< HEAD
			m_decalTextures = usedTextures.Select((IMyStreamedTexture x) => x.Pin()).ToArray();
=======
			m_decalTextures = Enumerable.ToArray<MyStreamedTexturePin>(Enumerable.Select<IMyStreamedTexture, MyStreamedTexturePin>((IEnumerable<IMyStreamedTexture>)usedTextures, (Func<IMyStreamedTexture, MyStreamedTexturePin>)((IMyStreamedTexture x) => x.Pin())));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ITexture GetTexture(string name, MyFileTextureEnum type)
			{
				IMyStreamedTexture texture = texManager.GetTexture(name, type);
				usedTextures.Add(texture);
				return texture.Texture;
			}
		}

		public static bool GetDecalTopoData(uint decalId, out MyDecalTopoData data)
		{
			if (!m_nodeMap.TryGetValue(decalId, out var value))
			{
				data = default(MyDecalTopoData);
				return false;
			}
<<<<<<< HEAD
			data = value.Value.TopoData;
=======
			data = value.get_Value().TopoData;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return true;
		}

		private static void Clear()
		{
			for (int i = 0; i < 2; i++)
			{
				m_drawCount[i] = 0;
				foreach (KeyValuePair<MyMaterialIdentity, List<MyScreenDecal>> item in m_materialsToDraw[i])
				{
					item.Value.Clear();
				}
			}
			AnyTransparentDecalsToDraw = false;
		}

		internal static int Preprocess(MyCullQuery cullQuery)
		{
			Clear();
<<<<<<< HEAD
			if (m_decals.Count == 0 && m_decalsTrails.Count == 0)
=======
			if (m_decals.get_Count() == 0 && m_decalsTrails.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return 0;
			}
			uint timeStampSinceStart = GetTimeStampSinceStart();
			IterateDecals(null, timeStampSinceStart);
			for (int i = 0; i < 2; i++)
			{
				foreach (KeyValuePair<MyMaterialIdentity, List<MyScreenDecal>> item in m_materialsToDraw[i])
				{
					m_drawCount[i] += item.Value.Count;
				}
			}
			AnyTransparentDecalsToDraw = m_drawCount[1] > 0;
<<<<<<< HEAD
			return m_decals.Count;
=======
			return m_decals.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <returns>True if visible decals are found</returns>
		private static void IterateDecals(HashSet<uint> visibleRenderIDs, uint sinceStartTs)
		{
			IterateDecalsList(m_decals, visibleRenderIDs, sinceStartTs);
			IterateDecalsList(m_decalsTrails, visibleRenderIDs, sinceStartTs);
		}

		private static void IterateDecalsList(LinkedList<MyScreenDecal> decalsList, HashSet<uint> visibleRenderIDs, uint sinceStartTs)
		{
<<<<<<< HEAD
			int count = decalsList.Count;
			LinkedListNode<MyScreenDecal> linkedListNode = decalsList.First;
			int num = 0;
			while (linkedListNode != null && num < count)
			{
				LinkedListNode<MyScreenDecal> next = linkedListNode.Next;
				MyScreenDecal value = linkedListNode.Value;
				if (value.FadeTimestamp < sinceStartTs || (value.OffScreenTime > 40f && value.CanBeDeletedOffscreen()))
				{
					RemoveDecalByNode(linkedListNode);
					linkedListNode = next;
					continue;
				}
				if (value.TimeUntilLive <= MyRender11.GameplayFrameCounter && value.FadeTimestamp == uint.MaxValue)
				{
					uint timeStampSinceStart = GetTimeStampSinceStart();
					value.FadeTimestamp = timeStampSinceStart + 6000;
					linkedListNode = next;
=======
			int count = decalsList.get_Count();
			LinkedListNode<MyScreenDecal> val = decalsList.get_First();
			int num = 0;
			while (val != null && num < count)
			{
				LinkedListNode<MyScreenDecal> next = val.get_Next();
				MyScreenDecal value = val.get_Value();
				if (value.FadeTimestamp < sinceStartTs || value.OffScreenTime > 40f)
				{
					RemoveDecalByNode(val);
					val = next;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					continue;
				}
				if ((visibleRenderIDs == null || visibleRenderIDs.Contains(value.ParentIDs[0])) && (value.Flags & MyDecalFlags.Closed) == 0)
				{
					if (IsDecalWithinRadius(value))
					{
<<<<<<< HEAD
						linkedListNode.Value.OffScreenTime = 0f;
						AddDecalNodeForDraw(linkedListNode);
					}
					else
					{
						linkedListNode.Value.OffScreenTime += MyCommon.GetLastFrameDelta();
					}
				}
				linkedListNode = next;
=======
						val.get_Value().OffScreenTime = 0f;
						AddDecalNodeForDraw(val);
					}
					else
					{
						val.get_Value().OffScreenTime += MyCommon.GetLastFrameDelta();
					}
				}
				val = next;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				num++;
			}
		}

		private static void AddDecalNodeForDraw(LinkedListNode<MyScreenDecal> node)
		{
			ReinsertNode(node);
<<<<<<< HEAD
			AddDecalForDraw(node.Value);
=======
			AddDecalForDraw(node.get_Value());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void RemoveNodeFromLists(LinkedListNode<MyScreenDecal> node)
		{
<<<<<<< HEAD
			if (node.Value.IsTrail)
			{
				m_decalsTrails.Remove(node);
				m_nodeMapTrails.Remove(node.Value.ID);
=======
			if (node.get_Value().IsTrail)
			{
				m_decalsTrails.Remove(node);
				m_nodeMapTrails.Remove(node.get_Value().ID);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				m_decals.Remove(node);
<<<<<<< HEAD
				m_nodeMap.Remove(node.Value.ID);
=======
				m_nodeMap.Remove(node.get_Value().ID);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void ReinsertNode(LinkedListNode<MyScreenDecal> node)
		{
<<<<<<< HEAD
			if (node.Value.IsTrail)
=======
			if (node.get_Value().IsTrail)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_decalsTrails.Remove(node);
				m_decalsTrails.AddLast(node);
			}
			else
			{
				m_decals.Remove(node);
				m_decals.AddLast(node);
			}
		}

		public static int Render(MyRenderContext rc, IRtvTexture gbuffer1Copy, ISrvBindable srvDepth, bool transparent)
		{
			if (m_jobs == null)
			{
				m_jobs = new List<MyDecalJob>();
			}
			int num = (transparent ? 1 : 0);
			uint timeStampSinceStart = GetTimeStampSinceStart();
			int num2 = ((!MyStereoRender.Enable) ? 1 : 2);
			for (int i = 0; i < num2; i++)
			{
				if (!MyStereoRender.Enable)
				{
					rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
					rc.SetViewport(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
				}
				else
				{
					MyStereoRender.RenderRegion = ((i == 0) ? MyStereoRegion.LEFT : MyStereoRegion.RIGHT);
					MyStereoRender.BindRawCB_FrameConstants(rc);
					MyStereoRender.SetViewport(rc);
				}
				rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
				rc.SetIndexBuffer(m_ib);
				rc.SetInputLayout(null);
				rc.VertexShader.Set(m_vs);
				rc.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
				rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
				IRtvArrayTexture closeCubemapFinal = MyManagers.EnvironmentProbe.CloseCubemapFinal;
				IRtvArrayTexture farCubemapFinal = MyManagers.EnvironmentProbe.FarCubemapFinal;
				rc.PixelShader.SetSrv(11, closeCubemapFinal);
				rc.PixelShader.SetSrv(17, farCubemapFinal);
				rc.AllShaderStages.SetConstantBuffer(2, m_cb);
				rc.PixelShader.SetSrv(0, srvDepth);
				rc.PixelShader.SetSrv(1, gbuffer1Copy);
				if (transparent)
				{
					rc.PixelShader.Set(m_psColorMapTransparent);
				}
				else
				{
					rc.SetRtvs(MyGBuffer.Main, MyDepthStencilAccess.ReadOnly);
				}
				foreach (KeyValuePair<MyMaterialIdentity, List<MyScreenDecal>> item in m_materialsToDraw[num])
				{
					PrepareMaterialBatches(rc, item.Value, timeStampSinceStart);
					DrawBatches(rc, item.Key.Material, item.Key.Index, transparent);
					m_jobs.Clear();
				}
			}
			rc.SetBlendState(null);
			rc.PixelShader.SetSrv(0, null);
			m_jobs.Clear();
			if (MyStereoRender.Enable)
			{
				rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
				rc.SetViewport(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
				MyStereoRender.RenderRegion = MyStereoRegion.FULLSCREEN;
			}
			return m_materialsToDraw[num].Count;
		}

		internal static int RenderGbuffer(MyCullQuery cullQuery)
		{
			if (m_drawCount[0] == 0)
			{
				return 0;
			}
			MyRenderContext myRenderContext = MyManagers.DeferredRCs.AcquireRC("GbufferDecals");
			int result = Render(myRenderContext, MyGBuffer.Main.GetGbuffer1CopyRtv(), MyGBuffer.Main.DepthStencil.SrvDepth, transparent: false);
			m_fc = myRenderContext.FinishDeferredContext();
			return result;
		}

		internal static void ConsumeDrawDeferred()
		{
			if (m_drawCount[0] != 0)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("MyScreenDecals.Opaque", "ConsumeDrawDeferred", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\Geometry\\MyScreenDecals.cs");
				MyRender11.RC.ExecuteContext(ref m_fc, "ConsumeDrawDeferred", 746, "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\Geometry\\MyScreenDecals.cs");
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeDrawDeferred", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\Geometry\\MyScreenDecals.cs");
=======
				MyGpuProfiler.IC_BeginBlock("MyScreenDecals.Opaque", "ConsumeDrawDeferred", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\Geometry\\MyScreenDecals.cs");
				MyRender11.RC.ExecuteContext(ref m_fc, "ConsumeDrawDeferred", 711, "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\Geometry\\MyScreenDecals.cs");
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeDrawDeferred", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\Geometry\\MyScreenDecals.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void DrawBatches(MyRenderContext rc, MyStringId material, int matIndex, bool transparent)
		{
			if (m_jobs.Count == 0 || !m_materials.TryGetValue(material, out var value) || matIndex >= value.Count)
			{
				return;
			}
			MyDecalTextures myDecalTextures = value[matIndex];
			if (!transparent)
			{
				MyFileTextureEnum decalType = myDecalTextures.DecalType;
				switch (decalType)
				{
				case MyFileTextureEnum.NORMALMAP_GLOSS:
					rc.PixelShader.Set(m_psNormalMap);
					break;
				case MyFileTextureEnum.COLOR_METAL:
					rc.PixelShader.Set(m_psColorMap);
					break;
				case MyFileTextureEnum.COLOR_METAL | MyFileTextureEnum.NORMALMAP_GLOSS:
					rc.PixelShader.Set(m_psNormalColorMap);
					break;
				case MyFileTextureEnum.COLOR_METAL | MyFileTextureEnum.NORMALMAP_GLOSS | MyFileTextureEnum.EXTENSIONS:
					rc.PixelShader.Set(m_psNormalColorExtMap);
					break;
				default:
					throw new Exception("Unknown decal type");
				}
				IBlendState materialTextureBlendState = MyMeshMaterials1.GetMaterialTextureBlendState(decalType, premultipliedAlpha: true);
				rc.SetBlendState(materialTextureBlendState);
			}
			rc.PixelShader.SetSrv(3, myDecalTextures.AlphamaskTexture);
			rc.PixelShader.SetSrv(4, myDecalTextures.ColorMetalTexture);
			rc.PixelShader.SetSrv(5, myDecalTextures.NormalmapTexture);
			rc.PixelShader.SetSrv(6, myDecalTextures.ExtensionsTexture);
			int num = m_jobs.Count / 512 + 1;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				MyMapping myMapping = MyMapping.MapDiscard(rc, m_cb);
				int num3 = m_jobs.Count - num2;
				int num4 = ((num3 > 512) ? 512 : num3);
				for (int j = 0; j < num4; j++)
				{
					MyDecalConstants constants = default(MyDecalConstants);
					EncodeJobConstants(j + num2, ref constants);
					myMapping.WriteAndPosition(ref constants);
				}
				myMapping.Unmap();
				rc.DrawIndexed(36 * num4, 0, 0);
				num2 += 512;
			}
		}

		private static void PrepareMaterialBatches(MyRenderContext rc, List<MyScreenDecal> decals, uint sinceStartTs)
		{
			if (decals.Count == 0)
			{
				return;
			}
			foreach (MyScreenDecal decal in decals)
			{
				if ((decal.Flags & MyDecalFlags.Closed) > MyDecalFlags.None)
				{
					continue;
				}
				bool num = (decal.Flags & MyDecalFlags.World) > MyDecalFlags.None;
				MyActor myActor = null;
				Matrix worldMatrix;
				if (num)
				{
					worldMatrix = decal.TopoData.MatrixCurrent;
					worldMatrix.Translation = decal.TopoData.WorldPosition - MyRender11.Environment.Matrices.CameraPosition;
				}
				else
				{
					myActor = MyIDTracker<MyActor>.FindByID(decal.ParentIDs[0]);
					MatrixD m = (MatrixD)decal.TopoData.MatrixCurrent * myActor.WorldMatrix;
					worldMatrix = m;
					worldMatrix.Translation = m.Translation - MyRender11.Environment.Matrices.CameraPosition;
				}
				uint num2 = decal.FadeTimestamp - sinceStartTs;
				float num3 = ((decal.FadeTimestamp - sinceStartTs >= 6000) ? 1f : ((float)num2 / 6000f));
				float val = worldMatrix.Translation.LengthSquared() / (decal.RenderSqDistance * m_qualityFactor * 0.9f);
				val = Math.Min(val, 1f);
				val = Math.Max(val, 0f);
				num3 *= 1f - val;
				m_jobs.Add(new MyDecalJob
				{
					WorldMatrix = worldMatrix,
					FadeAlpha = num3
				});
				if (MyRender11.Settings.DebugDrawDecals)
				{
					MatrixD matrix;
					if (myActor == null)
					{
						matrix = decal.TopoData.MatrixCurrent;
						matrix.Translation = decal.TopoData.WorldPosition;
					}
					else
					{
						matrix = (MatrixD)decal.TopoData.MatrixCurrent * myActor.WorldMatrix;
					}
					MyRenderProxy.DebugDrawAxis(matrix, 0.2f, depthRead: false, skipScale: true);
					MyRenderProxy.DebugDrawOBB(matrix, Color.Blue, 0.1f, depthRead: false, smooth: false);
					MyRenderProxy.DebugDrawText3D(matrix.Translation, decal.SourceTarget, Color.White, 0.5f, depthRead: false);
				}
			}
		}

		private static void AddDecalForDraw(MyScreenDecal decal)
		{
			int num = (((decal.Flags & MyDecalFlags.Transparent) > MyDecalFlags.None) ? 1 : 0);
			MyMaterialIdentity myMaterialIdentity = default(MyMaterialIdentity);
			myMaterialIdentity.Material = decal.MaterialId;
			myMaterialIdentity.Index = decal.MaterialIndex;
			MyMaterialIdentity key = myMaterialIdentity;
			if (!m_materialsToDraw[num].TryGetValue(key, out var value))
			{
				value = new List<MyScreenDecal>();
				m_materialsToDraw[num][key] = value;
			}
			value.Add(decal);
		}

		private static void EncodeJobConstants(int index, ref MyDecalConstants constants)
		{
			Matrix worldMatrix = Matrix.Transpose(m_jobs[index].WorldMatrix);
			Matrix invWorldMatrix = Matrix.Transpose(Matrix.Invert(m_jobs[index].WorldMatrix));
			constants.WorldMatrix = worldMatrix;
			constants.FadeAlpha = m_jobs[index].FadeAlpha;
			constants.__padding = new Vector3(0f, 0f, 1f);
			constants.InvWorldMatrix = invWorldMatrix;
		}

		private static bool IsDecalWithinRadius(MyScreenDecal decal)
		{
			return GetDecalCameraSqDistance(decal) <= decal.RenderSqDistance * m_qualityFactor;
		}

		private static bool CanBePlaced(MyScreenDecal decal)
		{
<<<<<<< HEAD
			if (!(GetDecalCameraSqDistance(decal) <= decal.RenderSqDistance * m_qualityFactor * 4f))
			{
				return !decal.CanBeDeletedOffscreen();
			}
			return true;
=======
			return GetDecalCameraSqDistance(decal) <= decal.RenderSqDistance * m_qualityFactor * 4f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static float GetDecalCameraSqDistance(MyScreenDecal decal)
		{
			Vector3 vector;
			if ((decal.Flags & MyDecalFlags.World) > MyDecalFlags.None)
			{
				vector = decal.TopoData.WorldPosition - MyRender11.Environment.Matrices.CameraPosition;
			}
			else
			{
				MyActor myActor = MyIDTracker<MyActor>.FindByID(decal.ParentIDs[0]);
				vector = ((MatrixD)decal.TopoData.MatrixCurrent * myActor.WorldMatrix).Translation - MyRender11.Environment.Matrices.CameraPosition;
			}
			return vector.LengthSquared();
		}

		private static uint GetTimeStampSinceStart()
		{
			return (uint)(DateTime.Now - m_startTime).TotalMilliseconds;
		}
	}
}
