using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Generics;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Emit;
using VRage.Render11.Scene.Components;
using VRage.Render11.Scene.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Scene
{
	internal class MyScene11 : MyScene
	{
		internal static readonly MyScene11 Instance;

		public static readonly Dictionary<uint, Dictionary<MyEntityMaterialKey, RenderableFlagsChange>> EntityMaterialRenderFlagChanges;

		private static readonly MyObjectsPool<MyManualCullTreeData> m_groupDataPool;

		public Action VicinityUpdatesJob;

		private readonly MyCullResults m_cachedResults = new MyCullResults();

		private static readonly ConcurrentDictionary<byte, Func<CullDataCollector>> m_activatorCache;

		internal new static MyDynamicAABBTreeD ManualCullTree => ((MyScene)Instance).ManualCullTree;

		internal new static MyDynamicAABBTreeD MergeGroupsDBVH => ((MyScene)Instance).MergeGroupsDBVH;

		internal new static MyDynamicAABBTreeD DynamicRenderablesDBVH => ((MyScene)Instance).DynamicRenderablesDBVH;

		internal new static MyDynamicAABBTreeD DynamicRenderablesFarDBVH => ((MyScene)Instance).DynamicRenderablesFarDBVH;

		public override float FadeOutTime => MyCommon.LoddingSettings.Global.MaxTransitionInSeconds;

		public IMySceneResourceCompiler ResourceCompiler { get; }

		public MyScene11()
			: base(new MyStaticComponentFactory11(), new MyStaticActorFactory11(), new MyStaticBillboardsHelper11(), new MyDynamicAABBTreeD(Vector3.Zero), new MyDynamicAABBTreeD(Vector3.Zero), new MyDynamicAABBTreeD(Vector3.Zero), new MyDynamicAABBTreeD(Vector3.Zero))
		{
			VicinityUpdatesJob = PerformVicinityUpdates;
			ResourceCompiler = new MySceneResourceCompiler();
		}

		public override MyManualCullTreeData AllocateGroupData()
		{
			MyManualCullTreeData item = null;
			m_groupDataPool.AllocateOrCreate(out item);
			if (item.All == null)
			{
				item.All = new MyCullResults();
				item.RenderCullData = new CullData[19];
				for (int i = 0; i < 19; i++)
				{
					item.RenderCullData[i] = CullData.Create();
					item.RenderCullData[i].ActiveResults = new MyCullResults();
				}
			}
			return item;
		}

		public void Update()
		{
			Environment.FrameTime = MyCommon.FrameTime;
			Environment.LastFrameDelta = MyCommon.GetLastFrameDelta();
			Updater.Update();
		}

		public override void Clear()
		{
			base.Clear();
			EntityMaterialRenderFlagChanges.Clear();
		}

		public override void FreeGroupData(MyManualCullTreeData data)
		{
			data.Actor = null;
			data.Children.Clear();
			for (int i = 0; i < 19; i++)
			{
				ref CullData reference = ref data.RenderCullData[i];
				reference.IterationOffset = 0;
				reference.ActiveActorsLastFrame = 0;
				reference.CulledActors.ClearAndTrim(256);
				reference.ActiveActors.ClearAndTrim(256);
				((MyCullResults)reference.ActiveResults).Clear();
			}
			m_groupDataPool.Deallocate(data);
		}

		static MyScene11()
		{
			Instance = new MyScene11();
			EntityMaterialRenderFlagChanges = new Dictionary<uint, Dictionary<MyEntityMaterialKey, RenderableFlagsChange>>();
			m_groupDataPool = new MyObjectsPool<MyManualCullTreeData>(32);
			m_activatorCache = new ConcurrentDictionary<byte, Func<CullDataCollector>>();
			Register<MyInstanceDataCollector>(new string[1] { "Instances" });
			Register<MyCullProxyDataCollector>(new string[1] { "CullProxies" });
			Register<MyPointLightDataCollector>(new string[1] { "PointLights" });
			Register<MyPointSpotLightDataCollector>(new string[2] { "PointLights", "SpotLights" });
<<<<<<< HEAD
			void Register<T>(string[] fields) where T : CullDataCollector, new()
			{
				m_activatorCache.TryAdd(CullDataEmitter.MakeMask(fields), () => new T());
=======
			static void Register<T>(string[] fields) where T : CullDataCollector, new()
			{
				m_activatorCache.TryAdd(CullDataEmitter.MakeMask(fields), (Func<CullDataCollector>)(() => new T()));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public override MyChildCullTreeData CompileCullData(MyChildCullTreeData data)
		{
			data.Add(m_cachedResults, arg2: true);
			byte type = CullDataEmitter.GetType(m_cachedResults);
<<<<<<< HEAD
			if (!m_activatorCache.TryGetValue(type, out var value))
			{
				value = (m_activatorCache[type] = () => new MyDefaultCullDataCollector());
			}
			CullDataCollector cullDataCollector = value();
=======
			Func<CullDataCollector> func = default(Func<CullDataCollector>);
			if (!m_activatorCache.TryGetValue(type, ref func))
			{
				m_activatorCache.set_Item(type, func = () => new MyDefaultCullDataCollector());
			}
			CullDataCollector cullDataCollector = func();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			cullDataCollector.Init(m_cachedResults, data);
			m_cachedResults.Clear();
			return cullDataCollector;
		}

		public sealed override void SetActorParent(MyActor parent, MyActor child, Matrix? childToParent)
		{
			if (child != null && parent != null && parent.IsRoot())
			{
				MyManagers.PostponedUpdate.ApplyPostponedUpdate(child.ID);
				MyManagers.PostponedUpdate.ApplyPostponedUpdate(parent.ID);
				child.SetParent(parent);
				if (childToParent.HasValue)
				{
					Matrix m = childToParent.Value;
					child.SetRelativeTransform(ref m);
				}
				if (MyRender11.Settings.DrawMergeInstanced && child.GetMergeGroupLeaf() == null)
				{
					child.AddComponent<MyMergeGroupLeafComponent>(MyComponentFactory<MyMergeGroupLeafComponent>.Create());
				}
			}
		}

		public static void AddMaterialRenderFlagChange(uint ID, MyEntityMaterialKey materialKey, RenderFlagsChange value)
		{
			bool flag = EntityMaterialRenderFlagChanges.ContainsKey(ID);
			if (value.Add == (RenderFlags)0 && value.Remove == (RenderFlags)0)
			{
				if (flag)
				{
					EntityMaterialRenderFlagChanges[ID].Remove(materialKey);
				}
				return;
			}
			if (!flag)
			{
				EntityMaterialRenderFlagChanges.Add(ID, new Dictionary<MyEntityMaterialKey, RenderableFlagsChange>());
			}
			EntityMaterialRenderFlagChanges[ID][materialKey] = new RenderableFlagsChange
			{
				Add = MyProxiesFactory.GetRenderableProxyFlags(value.Add),
				Remove = MyProxiesFactory.GetRenderableProxyFlags(value.Remove)
			};
		}

		private void PerformVicinityUpdates()
		{
			BoundingSphereD sphere = new BoundingSphereD(Environment.CameraPosition, 1000.0);
			ManualCullTree.OverlapAllBoundingSphere(ref sphere, delegate(MyManualCullTreeData x)
			{
				x.Actor.GetSceneResourcePrioritizationComponent()?.WakeUp();
			});
			DynamicRenderablesDBVH.OverlapAllBoundingSphere(ref sphere, delegate((Action<MyCullResultsBase, bool>, object) x)
			{
				((MyActor)x.Item2).GetSceneResourcePrioritizationComponent()?.WakeUp();
			});
		}
	}
}
