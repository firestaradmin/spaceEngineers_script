using System;
using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Scene.Resources;

namespace VRage.Render11.Scene.Components
{
	internal class MyResourcePrioritizationComponent : MyActorComponent
	{
		private class VRage_Render11_Scene_Components_MyResourcePrioritizationComponent_003C_003EActor : IActivator, IActivator<MyResourcePrioritizationComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyResourcePrioritizationComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyResourcePrioritizationComponent CreateInstance()
			{
				return new MyResourcePrioritizationComponent();
			}

			MyResourcePrioritizationComponent IActivator<MyResourcePrioritizationComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const float MaxDistance = 1000f;

		private int m_framesTillStopUpdating;

		private bool m_hasResources;

		private bool m_resourceCacheIsComplete;

		private MyCompiledResourceBundle? m_compiledResources;

		private readonly HashSet<IMySceneResourceOwner> m_resourceOwners = new HashSet<IMySceneResourceOwner>();

		private readonly Action m_onChildResourcedChangedDelegateCache;

		private MyResourcePrioritizationComponent Parent => base.Owner?.Parent?.GetSceneResourcePrioritizationComponent();

		public MyResourcePrioritizationComponent()
		{
			m_onChildResourcedChangedDelegateCache = OnChildResourcesChanged;
		}

		public override void Construct()
		{
			base.Construct();
			m_hasResources = true;
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
			DisposeResourceCache();
			base.AllowsParallelUpdate = true;
		}

		public override void OnParentSet()
		{
<<<<<<< HEAD
			base.OnParentSet();
			base.NeedsPerFrameUpdate = false;
			MyResourcePrioritizationComponent parent = Parent;
			foreach (IMySceneResourceOwner resourceOwner in m_resourceOwners)
			{
				resourceOwner.OnResourcesChanged -= m_onChildResourcedChangedDelegateCache;
				parent.RegisterResourceOwner(resourceOwner);
=======
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			base.OnParentSet();
			base.NeedsPerFrameUpdate = false;
			MyResourcePrioritizationComponent parent = Parent;
			Enumerator<IMySceneResourceOwner> enumerator = m_resourceOwners.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMySceneResourceOwner current = enumerator.get_Current();
					current.OnResourcesChanged -= m_onChildResourcedChangedDelegateCache;
					parent.RegisterResourceOwner(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public override void OnParentRemoved()
		{
<<<<<<< HEAD
			base.OnParentRemoved();
			base.NeedsPerFrameUpdate = true;
			MyResourcePrioritizationComponent parent = Parent;
			foreach (IMySceneResourceOwner resourceOwner in m_resourceOwners)
			{
				parent.UnregisterResourceOwner(resourceOwner);
				resourceOwner.OnResourcesChanged += m_onChildResourcedChangedDelegateCache;
=======
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			base.OnParentRemoved();
			base.NeedsPerFrameUpdate = true;
			MyResourcePrioritizationComponent parent = Parent;
			Enumerator<IMySceneResourceOwner> enumerator = m_resourceOwners.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMySceneResourceOwner current = enumerator.get_Current();
					parent.UnregisterResourceOwner(current);
					current.OnResourcesChanged += m_onChildResourcedChangedDelegateCache;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void RegisterResourceOwner(IMySceneResourceOwner resourceOwner)
		{
			DisposeResourceCache();
			m_resourceOwners.Add(resourceOwner);
			MyResourcePrioritizationComponent parent = Parent;
			if (parent == null)
			{
				base.NeedsPerFrameUpdate = true;
				resourceOwner.OnResourcesChanged += m_onChildResourcedChangedDelegateCache;
			}
			else
			{
				parent.RegisterResourceOwner(resourceOwner);
			}
		}

		public void UnregisterResourceOwner(IMySceneResourceOwner resourceOwner)
		{
			DisposeResourceCache();
			m_resourceOwners.Remove(resourceOwner);
			MyResourcePrioritizationComponent parent = Parent;
			if (parent == null)
			{
				resourceOwner.OnResourcesChanged -= m_onChildResourcedChangedDelegateCache;
			}
			else
			{
				parent.UnregisterResourceOwner(resourceOwner);
			}
<<<<<<< HEAD
			if (m_resourceOwners.Count == 0)
=======
			if (m_resourceOwners.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				base.NeedsPerFrameUpdate = false;
			}
		}

		private void OnChildResourcesChanged()
		{
			m_hasResources = true;
			base.NeedsPerFrameUpdate = true;
			DisposeResourceCache();
		}

		private void DisposeResourceCache()
		{
			m_compiledResources?.Dispose();
			m_compiledResources = null;
		}

		public void WakeUp()
		{
<<<<<<< HEAD
			if (m_resourceOwners.Count > 0 && m_hasResources)
=======
			if (m_resourceOwners.get_Count() > 0 && m_hasResources)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				base.NeedsPerFrameUpdate = true;
			}
		}

		public override void OnUpdateBeforeDraw()
		{
<<<<<<< HEAD
			if (Parent != null || m_resourceOwners.Count == 0 || !base.NeedsPerFrameUpdate)
=======
			if (Parent != null || m_resourceOwners.get_Count() == 0 || !base.NeedsPerFrameUpdate)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			if (!m_resourceCacheIsComplete)
			{
				DisposeResourceCache();
			}
			MyCompiledResourceBundle? myCompiledResourceBundle = m_compiledResources;
			if (!myCompiledResourceBundle.HasValue)
			{
				m_hasResources = false;
				m_framesTillStopUpdating = 5;
				(MyCompiledResourceBundle, bool) tuple = ((MyScene11)base.Owner.Scene).ResourceCompiler.Compile(EnumerateResources());
				m_resourceCacheIsComplete = tuple.Item2;
				myCompiledResourceBundle = (m_compiledResources = tuple.Item1);
			}
			if (!m_hasResources)
			{
				base.NeedsPerFrameUpdate = false;
			}
			else
			{
				if (!myCompiledResourceBundle.HasValue)
				{
					return;
				}
<<<<<<< HEAD
				double d = base.Owner.CalculateCameraDistanceSquaredFast();
				d = Math.Max(0.0001, Math.Sqrt(d));
				int distancePriority = (int)Math.Max(1.0, Math.Ceiling(1.0 / d * 1000.0));
				myCompiledResourceBundle.Value.UpdateScenePriority(distancePriority);
				if (d > 1200.0)
=======
				double num = Math.Sqrt(base.Owner.CalculateCameraDistanceSquared());
				int distancePriority = (int)Math.Max(1.0, Math.Ceiling(1000.0 - num));
				myCompiledResourceBundle.Value.UpdateScenePriority(distancePriority);
				if (num > 1200.0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (m_framesTillStopUpdating-- == 0)
					{
						base.NeedsPerFrameUpdate = false;
					}
				}
				else
				{
					m_framesTillStopUpdating = 5;
				}
				base.OnUpdateBeforeDraw();
			}
			IEnumerable<ResourceInfo> EnumerateResources()
			{
<<<<<<< HEAD
				foreach (IMySceneResourceOwner resourceOwner in m_resourceOwners)
				{
					foreach (ResourceInfo resource in resourceOwner.GetResources())
					{
						m_hasResources = true;
						yield return resource;
					}
				}
=======
				Enumerator<IMySceneResourceOwner> enumerator = m_resourceOwners.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMySceneResourceOwner current = enumerator.get_Current();
						foreach (ResourceInfo resource in current.GetResources())
						{
							m_hasResources = true;
							yield return resource;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
