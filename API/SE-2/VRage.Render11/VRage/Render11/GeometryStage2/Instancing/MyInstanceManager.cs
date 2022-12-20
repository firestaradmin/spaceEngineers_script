<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using VRage.Generics;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Model;
using VRageRender;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal class MyInstanceManager : IManager, IManagerUnloadData
	{
		private readonly MyObjectsPool<MyInstance> m_instances = new MyObjectsPool<MyInstance>(1);

		private readonly List<MyInstance> m_tmpInstances = new List<MyInstance>();

		internal MyInstance CreateInstance(MyModel model, bool isVisible, MyVisibilityExtFlags visibilityExt, MyInstanceComponent component, uint actorId, bool metalnessColorable, string modelFilepath, bool isDummyModel)
		{
			m_instances.AllocateOrCreate(out var item);
			item.Init(model, isVisible, visibilityExt, component, actorId, metalnessColorable, modelFilepath, isDummyModel);
			return item;
		}

		internal MyInstance CloneInstance(MyInstance instance)
		{
			m_instances.AllocateOrCreate(out var item);
			item.CopyFrom(instance);
			return item;
		}

		internal void DisposeInstance(MyInstance instance)
		{
			instance.DisposeInternal();
			m_instances.Deallocate(instance);
		}

		public void OnReloadModels(string filePath = null, MyModel model = null, bool ignoreAsserts = false, bool dither = false)
		{
<<<<<<< HEAD
			m_tmpInstances.Clear();
			foreach (MyInstance item in m_instances.Active)
			{
				if (filePath != null && !(item.ModelFilepath == filePath))
				{
					continue;
				}
				if (!((model != null) ? item.SetModel(model, filePath, isDummyModel: false) : item.OnReloadModel()))
				{
					if (!ignoreAsserts)
					{
						MyRender11.Log.WriteLine($"File '{item.Model.Filepath}' cannot be loaded by the new pipeline because of an issue with the file, therefore it will be rendered with the old pipeline. Beware: the behaviour of this model can be changed dramatically.");
					}
					m_tmpInstances.Add(item);
				}
				else if (dither && item.Owner != null)
				{
					item.LodStrategy.StartTransition(item.GetDistance(), fadeIn: true);
				}
			}
=======
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			m_tmpInstances.Clear();
			Enumerator<MyInstance> enumerator = m_instances.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyInstance current = enumerator.get_Current();
					if (filePath != null && !(current.ModelFilepath == filePath))
					{
						continue;
					}
					if (!((model != null) ? current.SetModel(model, filePath, isDummyModel: false) : current.OnReloadModel()))
					{
						if (!ignoreAsserts)
						{
							MyRender11.Log.WriteLine($"File '{current.Model.Filepath}' cannot be loaded by the new pipeline because of an issue with the file, therefore it will be rendered with the old pipeline. Beware: the behaviour of this model can be changed dramatically.");
						}
						m_tmpInstances.Add(current);
					}
					else if (dither && current.Owner != null)
					{
						current.LodStrategy.StartTransition(current.GetDistance(), fadeIn: true);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyInstance tmpInstance in m_tmpInstances)
			{
				if (tmpInstance.Owner != null)
				{
					MyComponentConverter.ConvertActorToTheOldPipeline(tmpInstance.Owner.Owner);
				}
			}
			m_tmpInstances.Clear();
		}

		void IManagerUnloadData.OnUnloadData()
		{
		}
	}
}
