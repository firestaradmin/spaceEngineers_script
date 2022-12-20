using System;
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
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[TargetType("Wolf")]
	[StaticEventOwner]
	public class MyWolfTarget : MyAiTargetBase
	{
		protected sealed class PlayAttackAnimation_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlayAttackAnimation(entityId);
			}
		}

		private int m_attackStart;

		private bool m_attackPerformed;

		private BoundingSphereD m_attackBoundingSphere;

<<<<<<< HEAD
=======
		private const int ATTACK_LENGTH = 1000;

		private const int ATTACK_DAMAGE_TO_CHARACTER = 12;

		private const int ATTACK_DAMAGE_TO_GRID = 8;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static HashSet<MySlimBlock> m_tmpBlocks = new HashSet<MySlimBlock>();

		private static MyStringId m_stringIdAttackAction = MyStringId.GetOrCompute("attack");

		public bool IsAttacking { get; private set; }

		public MyWolfTarget(IMyEntityBot bot)
			: base(bot)
		{
		}

		public void Attack(bool playSound)
		{
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity != null)
			{
				IsAttacking = true;
				m_attackPerformed = false;
				m_attackStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				if (!agentEntity.UseNewAnimationSystem)
				{
					agentEntity.PlayCharacterAnimation("WolfAttack", MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0f, 1f, sync: true);
					agentEntity.DisableAnimationCommands();
				}
<<<<<<< HEAD
				agentEntity.SoundComp.StartSecondarySound(m_bot.AgentDefinition.AttackSound, sync: true);
=======
				agentEntity.SoundComp.StartSecondarySound("ArcBotWolfAttack", sync: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
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
			else if (num > m_bot.AgentDefinition.AttackLength / 2 && m_bot.AgentEntity.UseNewAnimationSystem)
			{
				m_bot.AgentEntity.AnimationController.TriggerAction(m_stringIdAttackAction);
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(m_bot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, m_stringIdAttackAction.String);
				}
			}
<<<<<<< HEAD
			if (num <= m_bot.AgentDefinition.AttackLength / 2 || m_attackPerformed)
			{
				return;
			}
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity == null)
			{
				return;
			}
			Vector3D center = agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrixRef.Forward * 1.1000000238418579 + agentEntity.PositionComp.WorldMatrixRef.Up * 0.44999998807907104;
			m_attackBoundingSphere = new BoundingSphereD(center, m_bot.AgentDefinition.AttackRadius);
			m_attackPerformed = true;
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref m_attackBoundingSphere);
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (m_bot.AgentDefinition.CharacterDamage > 0 && item is MyCharacter && item != agentEntity)
				{
					MyCharacter myCharacter = item as MyCharacter;
					if (!myCharacter.IsSitting)
					{
						BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
						double num2 = m_attackBoundingSphere.Radius + worldVolume.Radius;
						num2 *= num2;
						if (!(Vector3D.DistanceSquared(m_attackBoundingSphere.Center, worldVolume.Center) > num2))
						{
							myCharacter.DoDamage(m_bot.AgentDefinition.CharacterDamage, MyDamageType.Wolf, updateSync: true, agentEntity.EntityId);
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
						tmpBlock.DoDamage(m_bot.AgentDefinition.GridDamage, MyDamageType.Wolf, sync: true, null, agentEntity.EntityId, 0L);
					}
					m_tmpBlocks.Clear();
=======
			if (num <= 500 || m_attackPerformed)
			{
				return;
			}
			MyCharacter agentEntity = m_bot.AgentEntity;
			if (agentEntity == null)
			{
				return;
			}
			Vector3D center = agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrixRef.Forward * 1.1000000238418579 + agentEntity.PositionComp.WorldMatrixRef.Up * 0.44999998807907104;
			m_attackBoundingSphere = new BoundingSphereD(center, 0.5);
			m_attackPerformed = true;
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref m_attackBoundingSphere);
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (!(item is MyCharacter) || item == agentEntity)
				{
					continue;
				}
				MyCharacter myCharacter = item as MyCharacter;
				if (!myCharacter.IsSitting)
				{
					BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
					double num2 = m_attackBoundingSphere.Radius + worldVolume.Radius;
					num2 *= num2;
					if (!(Vector3D.DistanceSquared(m_attackBoundingSphere.Center, worldVolume.Center) > num2))
					{
						myCharacter.DoDamage(12f, MyDamageType.Wolf, updateSync: true, agentEntity.EntityId);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			topMostEntitiesInSphere.Clear();
		}

<<<<<<< HEAD
		[Event(null, 149)]
=======
		[Event(null, 150)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Broadcast]
		[Reliable]
		private static void PlayAttackAnimation(long entityId)
		{
			MyCharacter myCharacter;
			if (MyEntities.EntityExists(entityId) && (myCharacter = MyEntities.GetEntityById(entityId) as MyCharacter) != null)
<<<<<<< HEAD
=======
			{
				myCharacter.AnimationController.TriggerAction(m_stringIdAttackAction);
			}
		}

		public override bool IsMemoryTargetValid(MyBBMemoryTarget targetMemory)
		{
			if (targetMemory == null)
			{
				return false;
			}
			if (targetMemory.TargetType == MyAiTargetEnum.GRID)
			{
				return false;
			}
			if (targetMemory.TargetType == MyAiTargetEnum.CUBE)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				myCharacter.AnimationController.TriggerAction(m_stringIdAttackAction);
			}
		}
	}
}
