using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
	public class MyHudLocationMarkers
	{
		private SortedList<long, MyHudEntityParams> m_markerEntities = new SortedList<long, MyHudEntityParams>();

		private HashSet<long> m_markersToRemove = new HashSet<long>();

		private Dictionary<long, MyHudEntityParams> m_markersToAdd = new Dictionary<long, MyHudEntityParams>();

		public bool Visible { get; set; }

<<<<<<< HEAD
		public IList<MyHudEntityParams> MarkerEntities => m_markerEntities.Values;
=======
		public IList<MyHudEntityParams> MarkerEntities => m_markerEntities.get_Values();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyHudLocationMarkers()
		{
			Visible = true;
		}

		public void RegisterMarker(MyEntity entity, MyHudEntityParams hudParams)
		{
			if (hudParams.Entity == null)
			{
				hudParams.Entity = entity;
			}
			RegisterMarker(entity.EntityId, hudParams);
		}

		public void RegisterMarker(long entityId, MyHudEntityParams hudParams)
		{
			lock (m_markerEntities)
			{
				m_markersToRemove.Remove(entityId);
				m_markersToAdd[entityId] = hudParams;
			}
		}

		public void UnregisterMarker(MyEntity entity)
		{
			UnregisterMarker(entity.EntityId);
		}

		public void UnregisterMarker(long entityId)
		{
			lock (m_markerEntities)
			{
				m_markersToAdd.Remove(entityId);
				m_markersToRemove.Add(entityId);
			}
		}

		public void Clear()
		{
			m_markerEntities.Clear();
			lock (m_markerEntities)
			{
				m_markersToAdd.Clear();
				m_markersToRemove.Clear();
			}
		}

		public void ProcessChanges()
		{
<<<<<<< HEAD
=======
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			lock (m_markerEntities)
			{
				foreach (KeyValuePair<long, MyHudEntityParams> item in m_markersToAdd)
				{
<<<<<<< HEAD
					m_markerEntities[item.Key] = item.Value;
				}
				foreach (long item2 in m_markersToRemove)
				{
					m_markerEntities.Remove(item2);
=======
					m_markerEntities.set_Item(item.Key, item.Value);
				}
				Enumerator<long> enumerator2 = m_markersToRemove.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						long current2 = enumerator2.get_Current();
						m_markerEntities.Remove(current2);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_markersToAdd.Clear();
				m_markersToRemove.Clear();
			}
		}
	}
}
