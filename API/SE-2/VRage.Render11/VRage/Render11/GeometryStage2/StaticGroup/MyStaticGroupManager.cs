<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Model;

namespace VRage.Render11.GeometryStage2.StaticGroup
{
	internal class MyStaticGroupManager : IManager, IManagerUpdate
	{
		private HashSet<MyStaticGroupComponent> m_registeredStaticGroups = new HashSet<MyStaticGroupComponent>();

		private readonly List<MyStaticGroupComponent> m_tmpInstances = new List<MyStaticGroupComponent>();

		public void Register(MyStaticGroupComponent component)
		{
			m_registeredStaticGroups.Add(component);
		}

		public void Unregister(MyStaticGroupComponent component)
		{
			m_registeredStaticGroups.Remove(component);
		}

		void IManagerUpdate.OnUpdate()
		{
<<<<<<< HEAD
			foreach (MyStaticGroupComponent registeredStaticGroup in m_registeredStaticGroups)
			{
				registeredStaticGroup.Update();
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyStaticGroupComponent> enumerator = m_registeredStaticGroups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Update();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnReloadModels(string filePath = null, MyModel model = null, bool ignoreAsserts = false, bool dither = false)
		{
<<<<<<< HEAD
			m_tmpInstances.Clear();
			foreach (MyStaticGroupComponent registeredStaticGroup in m_registeredStaticGroups)
			{
				if (filePath == null || registeredStaticGroup.ModelFilepath == filePath)
				{
					if (model == null)
					{
						registeredStaticGroup.OnReloadModel();
					}
					else
					{
						registeredStaticGroup.SetModel(model, filePath, isDummyModel: false);
					}
				}
			}
=======
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_tmpInstances.Clear();
			Enumerator<MyStaticGroupComponent> enumerator = m_registeredStaticGroups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyStaticGroupComponent current = enumerator.get_Current();
					if (filePath == null || current.ModelFilepath == filePath)
					{
						if (model == null)
						{
							current.OnReloadModel();
						}
						else
						{
							current.SetModel(model, filePath, isDummyModel: false);
						}
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
