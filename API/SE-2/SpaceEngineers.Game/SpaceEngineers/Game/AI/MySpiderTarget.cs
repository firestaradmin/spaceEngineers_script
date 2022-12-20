using System.Collections.Generic;
using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[TargetType("Spider")]
	public class MySpiderTarget : MyAiTargetBase
	{
		private int m_attackStart;

		private int m_attackCtr;

		private bool m_attackPerformed;

		private BoundingSphereD m_attackBoundingSphere;

<<<<<<< HEAD
=======
		private const int ATTACK_LENGTH = 1000;

		private const int ATTACK_ACTIVATION = 700;

		private const int ATTACK_DAMAGE_TO_CHARACTER = 35;

		private const int ATTACK_DAMAGE_TO_GRID = 50;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static HashSet<MySlimBlock> m_tmpBlocks = new HashSet<MySlimBlock>();

		public bool IsAttacking { get; private set; }

		public MySpiderTarget(IMyEntityBot bot)
			: base(bot)
		{
		}

		public void Attack()
		{
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity != null)
			{
				IsAttacking = true;
				m_attackPerformed = false;
				m_attackStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				ChooseAttackAnimationAndSound(out var animation, out var sound);
				agentEntity.PlayCharacterAnimation(animation, MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0f, 1f, sync: true);
				agentEntity.DisableAnimationCommands();
				agentEntity.SoundComp.StartSecondarySound(sound, sync: true);
			}
		}

		public override void Update()
		{
			base.Update();
			if (!IsAttacking)
			{
				return;
			}
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_attackStart;
<<<<<<< HEAD
			if (num > m_bot.AgentDefinition.AttackLength)
=======
			if (num > 1000)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				IsAttacking = false;
				m_bot.AgentEntity?.EnableAnimationCommands();
			}
			else if (num > m_bot.AgentDefinition.AttackLength / 2 && m_bot.AgentEntity.UseNewAnimationSystem && !m_attackPerformed)
			{
				m_bot.AgentEntity.TriggerAnimationEvent("attack");
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(m_bot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, "attack");
				}
			}
<<<<<<< HEAD
			if (!((float)num > (float)m_bot.AgentDefinition.AttackLength * 0.75f) || m_attackPerformed)
			{
				return;
			}
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity == null)
			{
				return;
			}
			Vector3D center = agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrixRef.Forward * 2.5 + agentEntity.PositionComp.WorldMatrixRef.Up * 1.0;
			m_attackBoundingSphere = new BoundingSphereD(center, m_bot.AgentDefinition.AttackRadius);
			m_attackPerformed = true;
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref m_attackBoundingSphere);
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				MyCharacter myCharacter;
				if (m_bot.AgentDefinition.CharacterDamage > 0 && (myCharacter = item as MyCharacter) != null && item != agentEntity)
				{
					if (!myCharacter.IsSitting)
					{
						BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
						double num2 = m_attackBoundingSphere.Radius + worldVolume.Radius;
						num2 *= num2;
						if (!(Vector3D.DistanceSquared(m_attackBoundingSphere.Center, worldVolume.Center) > num2))
						{
							myCharacter.DoDamage(m_bot.AgentDefinition.CharacterDamage, MyDamageType.Spider, updateSync: true, agentEntity.EntityId);
						}
					}
				}
				else
				{
					if (m_bot.AgentDefinition.GridDamage <= 0 || !(item is MyCubeGrid) || item.Physics == null)
					{
						continue;
					}
					MyCubeGrid obj = item as MyCubeGrid;
					m_tmpBlocks.Clear();
					obj.GetBlocksInsideSphere(ref m_attackBoundingSphere, m_tmpBlocks);
					foreach (MySlimBlock tmpBlock in m_tmpBlocks)
					{
						tmpBlock.DoDamage(m_bot.AgentDefinition.GridDamage, MyDamageType.Spider, sync: true, null, agentEntity.EntityId, 0L);
					}
					m_tmpBlocks.Clear();
=======
			if (num <= 750 || m_attackPerformed)
			{
				return;
			}
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity == null)
			{
				return;
			}
			Vector3D center = agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrixRef.Forward * 2.5 + agentEntity.PositionComp.WorldMatrixRef.Up * 1.0;
			m_attackBoundingSphere = new BoundingSphereD(center, 0.9);
			m_attackPerformed = true;
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref m_attackBoundingSphere);
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				MyCharacter myCharacter;
				if ((myCharacter = item as MyCharacter) != null && item != agentEntity && !myCharacter.IsSitting)
				{
					BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
					double num2 = m_attackBoundingSphere.Radius + worldVolume.Radius;
					num2 *= num2;
					if (!(Vector3D.DistanceSquared(m_attackBoundingSphere.Center, worldVolume.Center) > num2))
					{
						myCharacter.DoDamage(35f, MyDamageType.Spider, updateSync: true, agentEntity.EntityId);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			topMostEntitiesInSphere.Clear();
		}

		private void ChooseAttackAnimationAndSound(out string animation, out string sound)
		{
			m_attackCtr++;
			MyAiTargetEnum targetType = base.TargetType;
			if ((uint)(targetType - 2) > 1u && targetType == MyAiTargetEnum.CHARACTER)
			{
				MyCharacter myCharacter;
				if ((myCharacter = base.TargetEntity as MyCharacter) != null && myCharacter.IsDead)
				{
					if (m_attackCtr % 3 == 0)
					{
						animation = "AttackFrontLegs";
						sound = "ArcBotSpiderAttackClaw";
					}
					else
					{
						animation = "AttackBite";
						sound = "ArcBotSpiderAttackBite";
					}
				}
				else if (m_attackCtr % 2 == 0)
				{
					animation = "AttackStinger";
					sound = "ArcBotSpiderAttackSting";
				}
				else
				{
					animation = "AttackBite";
					sound = "ArcBotSpiderAttackBite";
				}
			}
			else
			{
				animation = "AttackFrontLegs";
				sound = "ArcBotSpiderAttackClaw";
			}
		}
	}
}
