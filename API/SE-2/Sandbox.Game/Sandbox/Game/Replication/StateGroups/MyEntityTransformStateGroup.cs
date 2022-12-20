using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
	/// <summary>
	/// Simple state group that synchronizes position and orietnation of an entity.
	/// </summary>
	internal class MyEntityTransformStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		private MatrixD m_transform = MatrixD.Identity;

		private readonly IMyEntity m_entity;

		public bool IsStreaming => false;

		public bool NeedsUpdate => true;

		public bool IsHighPriority => false;

		public IMyReplicable Owner { get; private set; }

		public bool IsValid => !m_entity.MarkedForClose;

		public MyEntityTransformStateGroup(IMyReplicable ownerReplicable, IMyEntity entity)
		{
			Owner = ownerReplicable;
			m_entity = entity;
			m_transform = m_entity.WorldMatrix;
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			if (!m_entity.PositionComp.WorldMatrixRef.Equals(m_transform))
			{
				m_entity.SetWorldMatrix(m_transform);
			}
		}

		public void Destroy()
		{
			Owner = null;
		}

		public float GetGroupPriority(int frameCountWithoutSync, MyClientInfo forClient)
		{
			return 1f;
		}

		public void Serialize(BitStream stream, MyClientInfo forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				stream.Write(m_entity.PositionComp.GetPosition());
				Quaternion q = Quaternion.CreateFromRotationMatrix(in m_entity.PositionComp.WorldMatrixRef);
				stream.WriteQuaternion(q);
			}
			else
			{
				Vector3D translation = stream.ReadVector3D();
				Quaternion quaternion = stream.ReadQuaternion();
				m_transform = MatrixD.CreateFromQuaternion(quaternion);
				m_transform.Translation = translation;
			}
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
		}

		public void ForceSend(MyClientStateBase clientData)
		{
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			MyWaypoint myWaypoint;
			if ((myWaypoint = m_entity as MyWaypoint) != null)
			{
				return !myWaypoint.Freeze;
			}
			MySafeZone mySafeZone;
			if ((mySafeZone = m_entity as MySafeZone) != null && mySafeZone.IsStatic)
			{
				return false;
			}
			return true;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			return MyStreamProcessingState.None;
		}
	}
}
