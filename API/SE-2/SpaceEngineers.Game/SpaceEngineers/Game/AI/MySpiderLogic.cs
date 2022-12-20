using System.Collections.Generic;
using Havok;
using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using VRage.Game.ModAPI;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[StaticEventOwner]
	public class MySpiderLogic : MyAgentLogic
	{
		private bool m_deburrowAnimationStarted;

		private bool m_deburrowSoundStarted;

		private int m_burrowStart;

		private int m_deburrowStart;

		private Vector3D? m_effectOnPosition;

		private const int BURROWING_TIME = 750;

		private const int BURROWING_FX_START = 300;

		private const int DEBURROWING_TIME = 1800;
<<<<<<< HEAD

		private const int DEBURROWING_ANIMATION_START = 0;

		private const int DEBURROWING_SOUND_START = 0;

		public bool IsBurrowing { get; private set; }

		public bool IsBurrowFinishedSuccessfully { get; private set; }

		public bool CanBurrow { get; private set; } = true;


		public bool IsDeburrowing { get; private set; }

=======

		private const int DEBURROWING_ANIMATION_START = 0;

		private const int DEBURROWING_SOUND_START = 0;

		public bool IsBurrowing { get; private set; }

		public bool IsBurrowFinishedSuccessfully { get; private set; }

		public bool CanBurrow { get; private set; } = true;


		public bool IsDeburrowing { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float TeleportRadius { get; set; } = 20f;


		public MySpiderLogic(MyAnimalBot bot)
			: base(bot)
		{
		}

		public override void Update()
		{
			base.Update();
			if (IsBurrowing || IsDeburrowing)
			{
				UpdateBurrowing();
				return;
			}
			MySpiderTarget mySpiderTarget = base.AiTarget as MySpiderTarget;
			if (base.AgentBot.AgentDefinition.TargetGrids && !mySpiderTarget.IsAttacking && mySpiderTarget.TargetEntity is MyCubeGrid)
			{
				MyCubeGrid obj = mySpiderTarget.TargetEntity as MyCubeGrid;
				if (obj.GetCubeBlock(obj.WorldToGridInteger(base.AgentBot.BotEntity.PositionComp.GetPosition())) != null)
				{
					mySpiderTarget.Attack();
				}
			}
		}

		public override void Cleanup()
		{
			base.Cleanup();
			DeleteBurrowingParticleFX();
		}

		public void StartBurrowing()
		{
			CanBurrow = true;
			Vector3D vector3D = base.AgentBot.AgentEntity.PositionComp.GetPosition() + base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Up;
			Vector3D vector3D2 = Vector3D.Normalize(base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Forward - base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Up * 2.0) * 6.0;
			Vector3D vector3D3 = vector3D + vector3D2 + Vector3D.Normalize(base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Forward) * 1.5;
			Vector3D vector = Vector3D.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.AgentBot.AgentEntity.PositionComp.GetPosition()));
			if (Vector3D.Dot(Vector3D.Normalize(base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Up), vector) > -0.949999988079071 || IsPositionObstacled(vector3D, vector3D3 - vector3D))
			{
				CanBurrow = false;
				return;
			}
			base.AgentBot.Navigation.StopImmediate(forceUpdate: true);
			if (base.AgentBot.AgentEntity.UseNewAnimationSystem)
			{
				base.AgentBot.AgentEntity.TriggerAnimationEvent("burrow");
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, "burrow");
				}
			}
			else if (base.AgentBot.AgentEntity.HasAnimation("Burrow"))
			{
				base.AgentBot.AgentEntity.PlayCharacterAnimation("Burrow", MyBlendOption.Immediate, MyFrameOption.Default, 0f, 1f, sync: true);
				base.AgentBot.AgentEntity.DisableAnimationCommands();
			}
			base.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotSpiderBurrowIn", sync: true);
			IsBurrowing = true;
			m_burrowStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public bool IsPositionObstacled()
		{
			return IsPositionObstacled(base.AgentBot.BotEntity.WorldMatrix.Translation, base.AgentBot.BotEntity.WorldMatrix.Up);
		}

		public bool IsPositionObstacled(Vector3D position, Vector3 normal)
		{
			List<MyPhysics.HitInfo> hits = new List<MyPhysics.HitInfo>();
			Vector3D from = position - 2f * normal;
			Vector3D to = position + 2f * normal;
			MyPhysics.CastRay(from, to, hits, 9);
			if (CollidesWithNonVoxel(ref hits))
			{
				return true;
			}
			HkShape shape = new HkBoxShape(Vector3.One);
			MatrixD transform = base.AgentBot.BotEntity.WorldMatrix;
			hits.Clear();
			MyPhysics.CastShapeReturnContactBodyDatas(to, shape, ref transform, 15u, 0f, hits);
			if (CollidesWithNonVoxel(ref hits))
			{
				return true;
			}
			return false;
		}

		private bool CollidesWithNonVoxel(ref List<MyPhysics.HitInfo> hits)
		{
			if (hits == null || hits.Count == 0)
			{
				return false;
			}
			foreach (MyPhysics.HitInfo hit in hits)
			{
				IHitInfo hitInfo = hit;
				if (!(hitInfo.HitEntity is MyVoxelBase) && hitInfo.HitEntity != base.AgentBot.BotEntity)
				{
					return true;
				}
			}
			return false;
		}

		public void StartDeburrowing()
		{
			base.AgentBot.Navigation.StopImmediate(forceUpdate: true);
			if (IsPositionObstacled())
			{
				return;
			}
			if (base.AgentBot.AgentEntity.UseNewAnimationSystem)
			{
				base.AgentBot.AgentEntity.TriggerAnimationEvent("deburrow");
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.TriggerAnimationEvent, "deburrow");
				}
			}
			IsDeburrowing = true;
			m_deburrowStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			CreateBurrowingParticleFX();
			m_deburrowAnimationStarted = false;
			m_deburrowSoundStarted = false;
		}

		private void UpdateBurrowing()
		{
			if (IsBurrowing)
			{
				int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_burrowStart;
				if (num > 300 && !m_effectOnPosition.HasValue)
				{
					CreateBurrowingParticleFX();
				}
				if (num >= 750)
				{
					IsBurrowing = false;
					IsBurrowFinishedSuccessfully = true;
					base.AgentBot.AgentEntity.Physics.Enabled = false;
					base.AgentBot.AgentEntity.Physics.Close();
					DeleteBurrowingParticleFX();
					base.AgentBot.AgentEntity.EnableAnimationCommands();
				}
			}
			if (!IsDeburrowing)
			{
				return;
			}
			int num2 = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_deburrowStart;
			if (!m_deburrowSoundStarted && num2 >= 0)
			{
				base.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotSpiderBurrowOut", sync: true);
				m_deburrowSoundStarted = true;
			}
			if (!m_deburrowAnimationStarted && num2 >= 0)
			{
				if (base.AgentBot.AgentEntity.HasAnimation("Deburrow"))
				{
					base.AgentBot.AgentEntity.EnableAnimationCommands();
					base.AgentBot.AgentEntity.PlayCharacterAnimation("Deburrow", MyBlendOption.Immediate, MyFrameOption.Default, 0f, 1f, sync: true);
					base.AgentBot.AgentEntity.DisableAnimationCommands();
				}
				m_deburrowAnimationStarted = true;
			}
			if (num2 >= 1800)
			{
				IsDeburrowing = false;
				DeleteBurrowingParticleFX();
				base.AgentBot.AgentEntity.EnableAnimationCommands();
			}
		}

		private void CreateBurrowingParticleFX()
		{
			Vector3D translation = base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Translation;
			translation += base.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Forward * 0.2;
			m_effectOnPosition = translation;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				base.AgentBot.AgentEntity.CreateBurrowingParticleFX_Client(translation);
			}
			MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.CreateBurrowingParticleFX_Client, translation);
		}

		private void DeleteBurrowingParticleFX()
		{
			if (m_effectOnPosition.HasValue && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyCharacter agentEntity = base.AgentBot.AgentEntity;
				if (agentEntity != null)
				{
					agentEntity.DeleteBurrowingParticleFX_Client(m_effectOnPosition.Value);
					MyMultiplayer.RaiseEvent(base.AgentBot.AgentEntity, (MyCharacter x) => x.DeleteBurrowingParticleFX_Client, m_effectOnPosition.Value);
				}
			}
			m_effectOnPosition = null;
		}
	}
}
