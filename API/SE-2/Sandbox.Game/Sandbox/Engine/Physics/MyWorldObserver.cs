using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath.Spatial;

namespace Sandbox.Engine.Physics
{
	internal class MyWorldObserver
	{
		private readonly Dictionary<long, int> m_entityInCluster = new Dictionary<long, int>();

		private readonly Dictionary<int, HashSet<long>> m_clusterReplicablesCount = new Dictionary<int, HashSet<long>>();

		private readonly HashSet<long> m_replicatedEntities = new HashSet<long>();

		internal MyWorldObserver(MyClusterTree clusterTree)
		{
			clusterTree.EntityAdded = (Action<long, int>)Delegate.Combine(clusterTree.EntityAdded, new Action<long, int>(OnEntityAddedToClusterTree));
			clusterTree.EntityRemoved = (Action<long, int>)Delegate.Combine(clusterTree.EntityRemoved, new Action<long, int>(OnEntityRemovedFromClusterTree));
		}

		internal void CleanUp(MyClusterTree clusterTree)
		{
			if (clusterTree != null)
			{
				clusterTree.EntityAdded = (Action<long, int>)Delegate.Remove(clusterTree.EntityAdded, new Action<long, int>(OnEntityAddedToClusterTree));
				clusterTree.EntityRemoved = (Action<long, int>)Delegate.Remove(clusterTree.EntityRemoved, new Action<long, int>(OnEntityRemovedFromClusterTree));
			}
			m_entityInCluster.Clear();
			m_clusterReplicablesCount.Clear();
		}

		private void OnEntityAddedToClusterTree(long entityId, int clusterId)
		{
			m_entityInCluster[entityId] = clusterId;
			if (m_replicatedEntities.Contains(entityId))
			{
				AddReplicatedEntity(entityId, clusterId);
			}
		}

		private void OnEntityRemovedFromClusterTree(long entityId, int clusterId)
		{
			if (m_replicatedEntities.Contains(entityId))
			{
				RemoveReplicatedEntity(entityId);
			}
			m_entityInCluster.Remove(entityId);
		}

		internal void AddReplicatedEntity(MyEntity entity)
		{
			if (m_entityInCluster.TryGetValue(entity.EntityId, out var value))
			{
				AddReplicatedEntity(entity.EntityId, value);
			}
			m_replicatedEntities.Add(entity.EntityId);
		}

		private void AddReplicatedEntity(long entityId, int clusterId)
		{
			if (m_clusterReplicablesCount.TryGetValue(clusterId, out var value))
			{
				value.Add(entityId);
				return;
			}
			value = new HashSet<long>();
			value.Add(entityId);
			m_clusterReplicablesCount[clusterId] = value;
		}

		internal void RemoveReplicatedEntity(MyEntity entity)
		{
			RemoveReplicatedEntity(entity.EntityId);
			m_replicatedEntities.Remove(entity.EntityId);
		}

		private void RemoveReplicatedEntity(long entityId)
		{
			if (m_entityInCluster.TryGetValue(entityId, out var value) && m_clusterReplicablesCount.TryGetValue(value, out var value2))
			{
				value2.Remove(entityId);
			}
		}

		internal void RemoveCluster(int clusterId)
		{
			m_clusterReplicablesCount.Remove(clusterId);
		}

		internal bool IsClusterActive(int clusterId)
		{
			if (m_clusterReplicablesCount.TryGetValue(clusterId, out var value))
			{
<<<<<<< HEAD
				return value.Count > 0;
=======
				return value.get_Count() > 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}
	}
}
