<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyFracturedPiecesManager : MySessionComponentBase
	{
		private struct Bodies
		{
			public HkRigidBody Rigid;

			public HkdBreakableBody Breakable;
		}

		public const int FakePieceLayer = 14;

		public static MyFracturedPiecesManager Static;

		private Queue<MyFracturedPiece> m_piecesPool = new Queue<MyFracturedPiece>();

		private const int MAX_ALLOC_PER_FRAME = 50;

		private int m_allocatedThisFrame;

		private HashSet<HkdBreakableBody> m_tmpToReturn = new HashSet<HkdBreakableBody>();

		private HashSet<long> m_dbgCreated = new HashSet<long>();

		private HashSet<long> m_dbgRemoved = new HashSet<long>();

		private List<HkBodyCollision> m_rigidList = new List<HkBodyCollision>();

		private Queue<Bodies> m_bodyPool = new Queue<Bodies>();

		private const int PREALLOCATE_PIECES = 400;

		private const int PREALLOCATE_BODIES = 400;

		public HashSet<HkRigidBody> m_givenRBs = new HashSet<HkRigidBody>((IEqualityComparer<HkRigidBody>)InstanceComparer<HkRigidBody>.Default);

		public override bool IsRequiredByGame => MyPerGameSettings.Destruction;

		public override void LoadData()
		{
			base.LoadData();
			InitPools();
			Static = this;
		}

		private MyFracturedPiece AllocatePiece()
		{
			m_allocatedThisFrame++;
			MyFracturedPiece obj = MyEntities.CreateEntity(new MyDefinitionId(typeof(MyObjectBuilder_FracturedPiece)), fadeIn: false) as MyFracturedPiece;
			obj.Physics = new MyPhysicsBody(obj, RigidBodyFlag.RBF_DEBRIS);
			obj.Physics.CanUpdateAccelerations = true;
			return obj;
		}

		protected override void UnloadData()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<Bodies> enumerator = m_bodyPool.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Breakable.ClearListener();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_bodyPool.Clear();
			m_piecesPool.Clear();
			base.UnloadData();
		}

		public override void UpdateAfterSimulation()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateAfterSimulation();
			Enumerator<HkdBreakableBody> enumerator = m_tmpToReturn.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HkdBreakableBody current = enumerator.get_Current();
					ReturnToPoolInternal(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_tmpToReturn.Clear();
			while (m_bodyPool.get_Count() < 400 && m_allocatedThisFrame < 50)
			{
				m_bodyPool.Enqueue(AllocateBodies());
			}
			while (m_piecesPool.get_Count() < 400 && m_allocatedThisFrame < 50)
			{
				m_piecesPool.Enqueue(AllocatePiece());
			}
			m_allocatedThisFrame = 0;
		}

		private void RemoveInternal(MyFracturedPiece fp, bool fromServer = false)
		{
			if (fp.Physics != null && fp.Physics.RigidBody != null && fp.Physics.RigidBody.IsDisposed)
			{
				fp.Physics.BreakableBody = fp.Physics.BreakableBody;
			}
			if (fp.Physics == null || fp.Physics.RigidBody == null || fp.Physics.RigidBody.IsDisposed)
			{
				MyEntities.Remove(fp);
				return;
			}
			if (!fp.Physics.RigidBody.IsActive)
			{
				fp.Physics.RigidBody.Activate();
			}
			MyPhysics.RemoveDestructions(fp.Physics.RigidBody);
			HkdBreakableBody breakableBody = fp.Physics.BreakableBody;
			breakableBody.AfterReplaceBody -= fp.Physics.FracturedBody_AfterReplaceBody;
			ReturnToPool(breakableBody);
			fp.Physics.Enabled = false;
			MyEntities.Remove(fp);
			fp.Physics.BreakableBody = null;
			fp.Render.ClearModels();
			fp.OriginalBlocks.Clear();
			_ = Sync.IsServer;
			fp.EntityId = 0L;
			fp.Physics.BreakableBody = null;
			m_piecesPool.Enqueue(fp);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="entityId">server = 0 (it allocates new), client - should recieve from server</param>
		/// <param name="fromServer"></param>
		/// <returns></returns>
		public MyFracturedPiece GetPieceFromPool(long entityId, bool fromServer = false)
		{
			_ = Sync.IsServer;
<<<<<<< HEAD
			MyFracturedPiece myFracturedPiece = ((m_piecesPool.Count != 0) ? m_piecesPool.Dequeue() : AllocatePiece());
=======
			MyFracturedPiece myFracturedPiece = ((m_piecesPool.get_Count() != 0) ? m_piecesPool.Dequeue() : AllocatePiece());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Sync.IsServer)
			{
				myFracturedPiece.EntityId = MyEntityIdentifier.AllocateId();
			}
			return myFracturedPiece;
		}

		public void GetFracturesInSphere(ref BoundingSphereD searchSphere, ref List<MyFracturedPiece> output)
		{
			HkShape shape = new HkSphereShape((float)searchSphere.Radius);
			try
			{
				MyPhysics.GetPenetrationsShape(shape, ref searchSphere.Center, ref Quaternion.Identity, m_rigidList, 12);
				foreach (HkBodyCollision rigid in m_rigidList)
				{
					MyFracturedPiece myFracturedPiece = rigid.GetCollisionEntity() as MyFracturedPiece;
					if (myFracturedPiece != null)
					{
						output.Add(myFracturedPiece);
					}
				}
			}
			finally
			{
				m_rigidList.Clear();
				shape.RemoveReference();
			}
		}

		public void GetFracturesInBox(ref BoundingBoxD searchBox, List<MyFracturedPiece> output)
		{
			m_rigidList.Clear();
			HkShape shape = new HkBoxShape(searchBox.HalfExtents);
			try
			{
				Vector3D translation = searchBox.Center;
				MyPhysics.GetPenetrationsShape(shape, ref translation, ref Quaternion.Identity, m_rigidList, 12);
				foreach (HkBodyCollision rigid in m_rigidList)
				{
					MyFracturedPiece myFracturedPiece = rigid.GetCollisionEntity() as MyFracturedPiece;
					if (myFracturedPiece != null)
					{
						output.Add(myFracturedPiece);
					}
				}
			}
			finally
			{
				m_rigidList.Clear();
				shape.RemoveReference();
			}
		}

		private Bodies AllocateBodies()
		{
			m_allocatedThisFrame++;
			Bodies result = default(Bodies);
			result.Rigid = null;
			result.Breakable = HkdBreakableBody.Allocate();
			return result;
		}

		public void InitPools()
		{
			for (int i = 0; i < 400; i++)
			{
				m_piecesPool.Enqueue(AllocatePiece());
			}
			for (int j = 0; j < 400; j++)
			{
				m_bodyPool.Enqueue(AllocateBodies());
			}
		}

		public HkdBreakableBody GetBreakableBody(HkdBreakableBodyInfo bodyInfo)
		{
<<<<<<< HEAD
			Bodies bodies = ((m_bodyPool.Count != 0) ? m_bodyPool.Dequeue() : AllocateBodies());
=======
			Bodies bodies = ((m_bodyPool.get_Count() != 0) ? m_bodyPool.Dequeue() : AllocateBodies());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			bodies.Breakable.Initialize(bodyInfo, bodies.Rigid);
			return bodies.Breakable;
		}

		public void RemoveFracturePiece(MyFracturedPiece piece, float blendTimeSeconds, bool fromServer = false, bool sync = true)
		{
			if (blendTimeSeconds == 0f)
			{
				RemoveInternal(piece, fromServer);
			}
		}

		public void RemoveFracturesInBox(ref BoundingBoxD box, float blendTimeSeconds)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			List<MyFracturedPiece> list = new List<MyFracturedPiece>();
			GetFracturesInBox(ref box, list);
			foreach (MyFracturedPiece item in list)
			{
				RemoveFracturePiece(item, blendTimeSeconds);
			}
		}

		public void RemoveFracturesInSphere(Vector3D center, float radius)
		{
			float num = radius * radius;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (entity is MyFracturedPiece && (radius <= 0f || (center - entity.Physics.CenterOfMassWorld).LengthSquared() < (double)num))
				{
					Static.RemoveFracturePiece(entity as MyFracturedPiece, 2f);
				}
			}
		}

		public void ReturnToPool(HkdBreakableBody body)
		{
			m_tmpToReturn.Add(body);
		}

		private void ReturnToPoolInternal(HkdBreakableBody body)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			HkRigidBody rigidBody = body.GetRigidBody();
			if (rigidBody == null)
			{
				return;
			}
			rigidBody.ContactPointCallbackEnabled = false;
			m_givenRBs.Remove(rigidBody);
<<<<<<< HEAD
			foreach (Bodies item2 in m_bodyPool)
			{
				if (!(body == item2.Breakable))
				{
					_ = rigidBody == item2.Rigid;
				}
			}
			body.BreakableShape.ClearConnections();
			body.Clear();
			Bodies item = default(Bodies);
			item.Rigid = rigidBody;
			item.Breakable = body;
			body.InitListener();
			m_bodyPool.Enqueue(item);
=======
			Enumerator<Bodies> enumerator = m_bodyPool.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Bodies current = enumerator.get_Current();
					if (!(body == current.Breakable))
					{
						_ = rigidBody == current.Rigid;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			body.BreakableShape.ClearConnections();
			body.Clear();
			Bodies bodies = default(Bodies);
			bodies.Rigid = rigidBody;
			bodies.Breakable = body;
			body.InitListener();
			m_bodyPool.Enqueue(bodies);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal void DbgCheck(long createdId, long removedId)
		{
		}
	}
}
