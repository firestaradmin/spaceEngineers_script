using System.Collections.Generic;
using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[MyBehaviorDescriptor("Spider")]
	[BehaviorActionImpl(typeof(MySpiderLogic))]
	public class MySpiderActions : MyAgentActions
	{
		private MySpiderTarget SpiderTarget => base.AiTargetBase as MySpiderTarget;

		protected MySpiderLogic SpiderLogic => base.Bot.AgentLogic as MySpiderLogic;

		public MySpiderActions(MyAnimalBot bot)
			: base(bot)
		{
		}

		protected override MyBehaviorTreeState Idle()
		{
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Burrow", MyBehaviorTreeActionType.INIT)]
		protected void Init_Burrow()
		{
			SpiderLogic.StartBurrowing();
		}

		[MyBehaviorTreeAction("Burrow")]
		protected MyBehaviorTreeState Burrow()
		{
			if (SpiderLogic.IsBurrowing)
			{
				return MyBehaviorTreeState.RUNNING;
			}
			if (SpiderLogic.CanBurrow && SpiderLogic.IsBurrowFinishedSuccessfully)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.FAILURE;
		}

		[MyBehaviorTreeAction("Deburrow", MyBehaviorTreeActionType.INIT)]
		protected void Init_Deburrow()
		{
			SpiderLogic.StartDeburrowing();
		}

		[MyBehaviorTreeAction("Deburrow")]
		protected MyBehaviorTreeState Deburrow()
		{
			if (!SpiderLogic.IsDeburrowing)
			{
				return MyBehaviorTreeState.NOT_TICKED;
			}
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Teleport", ReturnsRunning = false)]
		protected MyBehaviorTreeState Teleport()
		{
			if (base.Bot.Player.Character.HasAnimation("Deburrow"))
			{
				base.Bot.Player.Character.PlayCharacterAnimation("Deburrow", MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0f, 1f, sync: true);
				base.Bot.AgentEntity.DisableAnimationCommands();
			}
			MatrixD spawnPosition;
			bool spiderSpawnPosition = MySpaceBotFactory.GetSpiderSpawnPosition(out spawnPosition, base.Bot.Player.GetPosition(), SpiderLogic.TeleportRadius);
			SpiderLogic.TeleportRadius = ((SpiderLogic.TeleportRadius > 5f) ? (SpiderLogic.TeleportRadius - 5f) : 3f);
			if (!spiderSpawnPosition)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D position = spawnPosition.Translation;
			if (SpiderLogic.IsPositionObstacled(position, base.Bot.AgentEntity.WorldMatrix.Up))
			{
				SpiderLogic.TeleportRadius += 5f;
				return MyBehaviorTreeState.NOT_TICKED;
			}
			if (SpiderLogic.IsPositionObstacled(base.Bot.AgentEntity.WorldMatrix.Translation, base.Bot.AgentEntity.WorldMatrix.Up))
			{
				SpiderLogic.TeleportRadius += 5f;
				return MyBehaviorTreeState.NOT_TICKED;
			}
			float num = (float)base.Bot.AgentEntity.PositionComp.WorldVolume.Radius;
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			if (closestPlanet != null)
			{
				closestPlanet.CorrectSpawnLocation(ref position, num);
				spawnPosition.Translation = position;
			}
			else
			{
				Vector3D? vector3D = MyEntities.FindFreePlace(spawnPosition.Translation, num, 20, 5, 0.2f);
				if (vector3D.HasValue)
				{
					spawnPosition.Translation = vector3D.Value;
				}
			}
			if (SpiderLogic.IsPositionObstacled(spawnPosition.Translation, base.Bot.AgentEntity.WorldMatrix.Up))
			{
				SpiderLogic.TeleportRadius += 5f;
				return MyBehaviorTreeState.NOT_TICKED;
			}
			base.Bot.AgentEntity.Physics.Enabled = true;
			base.Bot.AgentEntity.UpdateCharacterPhysics(forceUpdate: true);
			base.Bot.AgentEntity.WorldMatrix = spawnPosition;
			if (base.Bot.AgentEntity.Physics.CharacterProxy != null)
			{
				base.Bot.AgentEntity.Physics.CharacterProxy.SetForwardAndUp(spawnPosition.Forward, spawnPosition.Up);
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.INIT)]
		protected void Init_Attack()
		{
			SpiderTarget.AimAtTarget();
			SpiderTarget.Attack();
		}

		[MyBehaviorTreeAction("Attack")]
		protected MyBehaviorTreeState Attack()
		{
			if (!SpiderTarget.IsAttacking)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.POST)]
		protected void Post_Attack()
		{
		}

		[MyBehaviorTreeAction("IsAttacking", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsAttacking()
		{
			if (!SpiderTarget.IsAttacking)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GetTargetWithPriority")]
		protected MyBehaviorTreeState GetTargetWithPriority([BTParam] float radius, [BTInOut] ref MyBBMemoryTarget outTarget, [BTInOut] ref MyBBMemoryInt priority)
		{
			Vector3D translation = base.Bot.Navigation.PositionAndOrientation.Translation;
			BoundingSphereD boundingSphere = new BoundingSphereD(translation, radius);
			if (priority == null)
			{
				priority = new MyBBMemoryInt();
			}
			int num = priority.IntValue;
			if (num <= 0)
			{
				num = int.MaxValue;
			}
			MyBehaviorTreeState myBehaviorTreeState = IsTargetValid(ref outTarget);
			if (myBehaviorTreeState == MyBehaviorTreeState.FAILURE)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			Vector3D? memoryTargetPosition = SpiderTarget.GetMemoryTargetPosition(outTarget);
			if (!memoryTargetPosition.HasValue || Vector3D.Distance(memoryTargetPosition.Value, base.Bot.AgentEntity.PositionComp.GetPosition()) > 400.0)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(base.Bot.AgentEntity.ControllerInfo.ControllingIdentityId);
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			topMostEntitiesInSphere.ShuffleList();
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (item == base.Bot.AgentEntity || !SpiderTarget.IsEntityReachable(item))
				{
					continue;
				}
<<<<<<< HEAD
				int num2 = int.MaxValue;
				int num3 = 6;
				MyCharacter myCharacter = item as MyCharacter;
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (base.Bot.AgentDefinition.TargetCharacters && myCharacter != null && myCharacter.ControllerInfo != null)
				{
					MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
					if ((playerFaction != null && playerFaction2 == playerFaction) || myCharacter.IsDead)
					{
						continue;
					}
					MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(myCharacter.WorldMatrix.Translation - 3.0 * myCharacter.WorldMatrix.Up, myCharacter.WorldMatrix.Translation + 3.0 * myCharacter.WorldMatrix.Up, 15);
					if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != myCharacter)
					{
						num3 = 1;
						if (num3 < num)
						{
							myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
							num = num3;
							MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
						}
					}
				}
				else
				{
					if (!base.Bot.AgentDefinition.TargetGrids || myCubeGrid == null || num <= 1)
					{
						continue;
					}
					Vector3D value = myCubeGrid.WorldToGridScaledLocal(translation);
					double num4 = double.MaxValue;
					MySlimBlock mySlimBlock = null;
					foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
					{
						num3 = GetBlockTargetPriority(cubeBlock);
						if (num3 >= num2)
						{
							continue;
						}
						Vector3D value2 = ((cubeBlock.FatBlock != null) ? (new Vector3D(cubeBlock.Min + cubeBlock.Max) * 0.5) : ((Vector3D)cubeBlock.Position));
						double num5 = Vector3D.RectangularDistance(ref value2, ref value);
						if (num5 < num4)
						{
							mySlimBlock = cubeBlock;
							num4 = num5;
							num2 = num3;
							if (num4 < 10.0)
							{
								break;
							}
						}
=======
				int num2 = 6;
				MyCharacter myCharacter = item as MyCharacter;
				if (myCharacter == null || myCharacter.ControllerInfo == null)
				{
					continue;
				}
				MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
				if ((playerFaction != null && playerFaction2 == playerFaction) || myCharacter.IsDead)
				{
					continue;
				}
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(myCharacter.WorldMatrix.Translation - 3.0 * myCharacter.WorldMatrix.Up, myCharacter.WorldMatrix.Translation + 3.0 * myCharacter.WorldMatrix.Up, 15);
				if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != myCharacter)
				{
					num2 = 1;
					if (num2 < num)
					{
						myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
						num = num2;
						MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					if (mySlimBlock != null)
					{
						myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
						num = num2;
						MyBBMemoryTarget.SetTargetCube(ref outTarget, mySlimBlock.Position, myCubeGrid.EntityId);
					}
				}
			}
			topMostEntitiesInSphere.Clear();
			priority.IntValue = num;
			return myBehaviorTreeState;
		}

		/// <summary>
		/// Weapon Blocks = 2, Propulsion Blocks = 3, Power Blocks = 4, Other Blocks = 5
		/// </summary>
		/// <param name="block"></param>
		/// <returns></returns>
		private int GetBlockTargetPriority(MySlimBlock block)
		{
			if (block.FatBlock == null)
			{
				return 5;
			}
			if (block is IMyUserControllableGun || block is IMyShipToolBase)
			{
				return 2;
			}
			if (block is IMyThrust || block is IMyMotorSuspension || block is IMyWheel)
			{
				return 3;
			}
			if (block is IMyPowerProducer)
			{
				return 4;
			}
			return 5;
		}
	}
}
