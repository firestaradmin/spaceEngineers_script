using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
	public class MyCharacterAvoidanceSteering : MySteeringBase
	{
		public bool AvoidPlayer { get; set; }

		public MyCharacterAvoidanceSteering(MyBotNavigation botNavigation, float weight)
			: base(botNavigation, weight)
		{
		}

		public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
		{
			MyCharacter myCharacter;
			if (base.Parent.Speed < 0.01f || (myCharacter = base.Parent.BotEntity as MyCharacter) == null)
			{
				return;
			}
			Vector3D translation = base.Parent.PositionAndOrientation.Translation;
			BoundingBoxD boundingBox = new BoundingBoxD(translation - Vector3D.One * 3.0, translation + Vector3D.One * 3.0);
			Vector3D vector = base.Parent.ForwardVector;
			List<MyEntity> list = new List<MyEntity>();
			MyEntities.GetTopMostEntitiesInBox(ref boundingBox, list);
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
			foreach (MyEntity item in list)
			{
				MyCharacter myCharacter2;
				if ((myCharacter2 = item as MyCharacter) == null || myCharacter2 == myCharacter)
				{
					continue;
				}
				if (myCharacter2.ControllerInfo != null && !AvoidPlayer)
				{
					MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter2.ControllerInfo.ControllingIdentityId);
					if (playerFaction != null && playerFaction2 != playerFaction)
					{
						continue;
					}
				}
<<<<<<< HEAD
				Vector3 vector2 = myCharacter2.PositionComp.GetPosition() - translation;
				float value = vector2.Normalize();
				value = MathHelper.Clamp(value, 0f, 6f);
				double num = Vector3D.Dot(vector2, vector);
				Vector3 vector3 = -vector2;
				if (num > -0.807)
				{
					correction += (6f - value) * base.Weight * vector3;
=======
				Vector3D vector3D = myCharacter2.PositionComp.GetPosition() - translation;
				double value = vector3D.Normalize();
				value = MathHelper.Clamp(value, 0.0, 6.0);
				double num = Vector3D.Dot(vector3D, vector);
				Vector3D vector3D2 = -vector3D;
				if (num > -0.807)
				{
					correction += (6.0 - value) * (double)base.Weight * vector3D2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (!correction.IsValid())
				{
					Debugger.Break();
				}
			}
			list.Clear();
			weight += base.Weight;
		}

		public override void DebugDraw()
		{
		}

		public override string GetName()
		{
			return "Character avoidance steering";
		}
	}
}
