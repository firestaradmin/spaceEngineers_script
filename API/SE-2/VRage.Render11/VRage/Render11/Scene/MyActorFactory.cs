<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Collections;
using VRage.Generics;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.Scene.Components;
using VRageRender;

namespace VRage.Render11.Scene
{
	public static class MyActorFactory
	{
		private static readonly MyObjectsPool<MyActor> m_pool;

		internal static bool m_removeAllInProgress;

		static MyActorFactory()
		{
			m_pool = new MyObjectsPool<MyActor>(50, () => new MyActor(MyScene11.Instance));
		}

		private static void Deallocate(MyActor actor)
		{
			if (!m_removeAllInProgress)
			{
				m_pool.Deallocate(actor);
			}
		}

		internal static MyActor Create(string debugName)
		{
			m_pool.AllocateOrCreate(out var item);
			item.OnDestruct += Deallocate;
			item.Construct(debugName);
			return item;
		}

		internal static MyActor CreateSceneObject(string debugName)
		{
			MyActor myActor = Create(debugName);
			myActor.AddComponent<MyRenderableComponent>(MyComponentFactory<MyRenderableComponent>.Create());
			return myActor;
		}

		internal static MyActor CreateInstanceObject(string debugName)
		{
			MyActor myActor = Create(debugName);
			myActor.AddComponent<MyInstanceComponent>(MyComponentFactory<MyInstanceComponent>.Create());
			return myActor;
		}

		internal static MyActor CreateCharacter(string debugName)
		{
			MyActor myActor = Create(debugName);
			myActor.AddComponent<MyRenderableComponent>(MyComponentFactory<MyRenderableComponent>.Create());
			myActor.AddComponent<MySkinningComponent>(MyComponentFactory<MySkinningComponent>.Create());
			return myActor;
		}

		public static MyActor CreateLight(string debugName)
		{
			MyActor myActor = Create(debugName);
			myActor.AddComponent<VRage.Render.Scene.Components.MyLightComponent>(MyComponentFactory<VRage.Render11.Scene.Components.MyLightComponent>.Create());
			return myActor;
		}

		internal static MyActor CreateStaticGroup(string debugName)
		{
			return Create(debugName);
		}

		internal static MyActor CreateRoot(string debugName)
		{
			MyActor myActor = Create(debugName);
			if (MyRender11.Settings.DrawMergeInstanced)
			{
				myActor.AddComponent<MyMergeGroupRootComponent>(MyComponentFactory<MyMergeGroupRootComponent>.Create());
			}
			return myActor;
		}

		internal static void RemoveAll()
		{
<<<<<<< HEAD
			m_removeAllInProgress = true;
			foreach (MyActor item in m_pool.Active)
			{
				item.Destruct();
			}
			m_removeAllInProgress = false;
			m_pool.DeallocateAll();
			m_pool.TrimInternalCollections();
=======
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			m_removeAllInProgress = true;
			Enumerator<MyActor> enumerator = m_pool.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Destruct();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_removeAllInProgress = false;
			m_pool.DeallocateAll();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static HashSetReader<MyActor> GetAll()
		{
			return m_pool.Active;
		}
	}
}
