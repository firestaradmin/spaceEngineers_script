using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
	public class MyForwardCollisionAvoidanceSteering : MySteeringBase
	{
		private readonly Vector3D m_boxSize = Vector3D.One * 0.40000000596046448;

		private readonly List<MyEntity> m_entities = new List<MyEntity>();

		private const double DISTANCE_THRESHOLD = 4.5;

		private const float MAX_SEE_AHEAD = 1.85f;

		public MyForwardCollisionAvoidanceSteering(MyBotNavigation parent)
			: base(parent, 1f)
		{
		}

		public override string GetName()
		{
			return "Forward collision avoidance steering";
		}

		public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
		{
			MyCharacter myCharacter;
			if ((myCharacter = base.Parent.BotEntity as MyCharacter) == null)
			{
				return;
			}
			Vector3D translation = base.Parent.PositionAndOrientation.Translation;
			BoundingBoxD boundingBox = new BoundingBoxD(translation - 5.0, translation + 5f);
			MyEntities.GetTopMostEntitiesInBox(ref boundingBox, m_entities, MyEntityQueryType.Dynamic);
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
			if (m_entities.Count < 3)
			{
				return;
			}
			int num = 0;
			foreach (MyEntity entity in m_entities)
			{
				MyCharacter myCharacter2;
				if ((myCharacter2 = entity as MyCharacter) == null || myCharacter2 == myCharacter)
				{
					continue;
				}
				if (myCharacter2.ControllerInfo != null)
				{
					MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter2.ControllerInfo.ControllingIdentityId);
					if (playerFaction != null && playerFaction2 != playerFaction)
					{
						continue;
					}
				}
				Vector3D position = myCharacter2.PositionComp.GetPosition();
				Vector3D vector = translation - position;
				double num2 = vector.Normalize();
				double num3 = Vector3D.Dot(vector, base.Parent.ForwardVector);
				if (num2 < 4.5 && num3 < 0.0)
				{
					num++;
				}
			}
			if (num > 4)
			{
				base.Parent.WaitForClearPathCountdown = 20;
			}
			m_entities.Clear();
		}

		public override void DebugDraw()
		{
			MatrixD positionAndOrientation = base.Parent.PositionAndOrientation;
			Vector3 forwardVector = base.Parent.ForwardVector;
			Vector3 vector = Vector3.Cross(Vector3.Cross(positionAndOrientation.Up, forwardVector), positionAndOrientation.Up);
			Vector3D vector3D = positionAndOrientation.Translation + positionAndOrientation.Up;
			Vector3 vector2 = vector * 1.85f;
			MyRenderProxy.DebugDrawLine3D(vector3D, vector3D + vector2, Color.Teal, Color.Teal, depthRead: true);
			MyRenderProxy.DebugDrawAABB(new BoundingBoxD(vector3D + vector2 - m_boxSize, vector3D + vector2 + m_boxSize), Color.Red);
		}

		public void AccumulateCorrectionWithExactEntities(ref Vector3 correction, ref float weight)
		{
			MyCharacter myCharacter;
			if ((myCharacter = base.Parent.BotEntity as MyCharacter) == null)
			{
				return;
			}
			MatrixD positionAndOrientation = base.Parent.PositionAndOrientation;
			Vector3 forwardVector = base.Parent.ForwardVector;
			Vector3 vector = Vector3.Cross(Vector3.Cross(positionAndOrientation.Up, forwardVector), positionAndOrientation.Up);
			Vector3D vector3D = positionAndOrientation.Translation + positionAndOrientation.Up;
			Vector3 vector2 = vector * 1.85f;
			BoundingBoxD boundingBox = new BoundingBoxD(vector3D + vector2 - m_boxSize, vector3D + vector2 + m_boxSize);
			List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref boundingBox, exact: true);
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
			if (entitiesInAABB.Count < 3)
			{
				return;
			}
			int num = 0;
			foreach (MyEntity item in entitiesInAABB)
			{
				MyCharacter myCharacter2;
				if ((myCharacter2 = item as MyCharacter) == null || myCharacter2 == myCharacter)
				{
					continue;
				}
				if (myCharacter2.ControllerInfo != null)
				{
					MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter2.ControllerInfo.ControllingIdentityId);
					if (playerFaction != null && playerFaction2 != playerFaction)
					{
						continue;
					}
				}
				num++;
			}
			if (num > 2)
			{
				base.Parent.WaitForClearPathCountdown = 20;
			}
			entitiesInAABB.Clear();
		}
	}
}
