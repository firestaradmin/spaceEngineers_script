using Sandbox;
using Sandbox.Game;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	public class MyWolfLogic : MyAgentLogic
	{
		private const int SELF_DESTRUCT_TIME_MS = 4000;

		private const float EXPLOSION_RADIUS = 4f;

		private const int EXPLOSION_DAMAGE = 7500;

		private const int EXPLOSION_PLAYER_DAMAGE = 0;

		private int m_selfDestructStartedInTime;

		private bool m_lastWasAttacking;

		public bool SelfDestructionActivated { get; private set; }

		public MyWolfLogic(MyAnimalBot bot)
			: base(bot)
		{
		}

		public override void Update()
		{
			base.Update();
			if (SelfDestructionActivated && MySandboxGame.TotalGamePlayTimeInMilliseconds >= m_selfDestructStartedInTime + 4000)
			{
				MyAIComponent.Static.RemoveBot(base.AgentBot.Player.Id.SerialId, removeCharacter: true);
<<<<<<< HEAD
				BoundingSphereD explosionSphere = new BoundingSphereD(base.AgentBot.Player.GetPosition(), 4.0);
=======
				BoundingSphere boundingSphere = new BoundingSphere(base.AgentBot.Player.GetPosition(), 4f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = 7500f;
				myExplosionInfo.ExplosionType = MyExplosionTypeEnum.BOMB_EXPLOSION;
<<<<<<< HEAD
				myExplosionInfo.ExplosionSphere = explosionSphere;
=======
				myExplosionInfo.ExplosionSphere = boundingSphere;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = base.AgentBot.Player.Character;
				myExplosionInfo.ParticleScale = 0.5f;
				myExplosionInfo.OwnerEntity = base.AgentBot.Player.Character;
				myExplosionInfo.Direction = Vector3.Zero;
				myExplosionInfo.VoxelExplosionCenter = base.AgentBot.Player.Character.PositionComp.GetPosition();
				myExplosionInfo.ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION;
				myExplosionInfo.VoxelCutoutScale = 0.6f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
				MyExplosionInfo explosionInfo = myExplosionInfo;
				if (base.AgentBot.Player.Character.Physics != null)
				{
					explosionInfo.Velocity = base.AgentBot.Player.Character.Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo);
			}
			MyWolfTarget myWolfTarget = base.AiTarget as MyWolfTarget;
			if (base.AgentBot.AgentDefinition.TargetGrids && !myWolfTarget.IsAttacking && myWolfTarget.TargetEntity is MyCubeGrid)
			{
				MyCubeGrid obj = myWolfTarget.TargetEntity as MyCubeGrid;
				if (obj.GetCubeBlock(obj.WorldToGridInteger(base.AgentBot.BotEntity.PositionComp.GetPosition())) != null)
				{
					myWolfTarget.Attack(playSound: true);
				}
			}
			if (base.AgentBot.Player.Character != null && !base.AgentBot.Player.Character.UseNewAnimationSystem && !myWolfTarget.IsAttacking && !m_lastWasAttacking && myWolfTarget.HasTarget() && !myWolfTarget.PositionIsNearTarget(base.AgentBot.Player.Character.PositionComp.GetPosition(), 1.5f))
			{
				if (base.AgentBot.Navigation.Stuck)
				{
					Vector3D position = base.AgentBot.Player.Character.PositionComp.GetPosition();
					Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
					Vector3D vector3D2 = base.AgentBot.Player.Character.AimedPoint - position;
					Vector3D vector3D3 = vector3D2 - vector3D * Vector3D.Dot(vector3D2, vector3D) / vector3D.LengthSquared();
					vector3D3.Normalize();
					base.AgentBot.Navigation.AimAt(null, position + 100.0 * vector3D3);
					base.AgentBot.Player.Character.PlayCharacterAnimation("WolfIdle1", MyBlendOption.Immediate, MyFrameOption.Loop, 0f);
					base.AgentBot.Player.Character.DisableAnimationCommands();
				}
				else
				{
					base.AgentBot.Player.Character.EnableAnimationCommands();
				}
			}
			m_lastWasAttacking = myWolfTarget.IsAttacking;
		}

		public override void Cleanup()
		{
			base.Cleanup();
		}

		public void ActivateSelfDestruct()
		{
			if (!SelfDestructionActivated)
			{
				m_selfDestructStartedInTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				SelfDestructionActivated = true;
				string cueName = "ArcBotCyberSelfActDestr";
				base.AgentBot.AgentEntity.SoundComp.StartSecondarySound(cueName, sync: true);
			}
		}

		public void Remove()
		{
			MyAIComponent.Static.RemoveBot(base.AgentBot.Player.Id.SerialId, removeCharacter: true);
		}
	}
}
