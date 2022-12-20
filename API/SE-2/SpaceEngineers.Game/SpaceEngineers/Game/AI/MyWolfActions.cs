using System.Collections.Generic;
<<<<<<< HEAD
using Sandbox.Engine.Physics;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
<<<<<<< HEAD
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
=======
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	[MyBehaviorDescriptor("Wolf")]
	[BehaviorActionImpl(typeof(MyWolfLogic))]
	public class MyWolfActions : MyAgentActions
	{
		private Vector3D? m_runAwayPos;

		private Vector3D? m_lastTargetedEntityPosition;

		private Vector3D? m_debugTarget;

		private MyWolfTarget WolfTarget => base.AiTargetBase as MyWolfTarget;

		protected MyWolfLogic WolfLogic => base.Bot.AgentLogic as MyWolfLogic;

		public MyWolfActions(MyAnimalBot bot)
			: base(bot)
		{
		}

		protected override MyBehaviorTreeState Idle()
		{
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("GoToPlayerDefinedTarget", ReturnsRunning = true)]
		protected MyBehaviorTreeState GoToPlayerDefinedTarget()
		{
			if (m_debugTarget != MyAIComponent.Static.DebugTarget)
			{
				m_debugTarget = MyAIComponent.Static.DebugTarget;
				if (!MyAIComponent.Static.DebugTarget.HasValue)
				{
					return MyBehaviorTreeState.FAILURE;
				}
			}
			Vector3D position = base.Bot.Player.Character.PositionComp.GetPosition();
			if (m_debugTarget.HasValue)
			{
				if (Vector3D.Distance(position, m_debugTarget.Value) <= 1.0)
				{
					return MyBehaviorTreeState.SUCCESS;
				}
				Vector3D worldCenter = m_debugTarget.Value;
				MyDestinationSphere end = new MyDestinationSphere(ref worldCenter, 1f);
				if (!MyAIComponent.Static.Pathfinding.FindPathGlobal(position, end, null).GetNextTarget(position, out var target, out var _, out var _))
				{
					return MyBehaviorTreeState.FAILURE;
				}
				if (WolfTarget.TargetPosition != target)
				{
					WolfTarget.SetTargetPosition(target);
				}
				WolfTarget.AimAtTarget();
				WolfTarget.GotoTargetNoPath(0f, resetStuckDetection: false);
			}
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.INIT)]
		protected void Init_Attack()
		{
			WolfTarget.AimAtTarget();
			WolfTarget.Attack(!WolfLogic.SelfDestructionActivated);
		}

		[MyBehaviorTreeAction("Attack")]
		protected MyBehaviorTreeState Attack()
		{
			if (!WolfTarget.IsAttacking)
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
			if (!WolfTarget.IsAttacking)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("Explode")]
		protected MyBehaviorTreeState Explode()
		{
			WolfLogic.ActivateSelfDestruct();
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("GetTargetWithPriority")]
		protected MyBehaviorTreeState GetTargetWithPriority([BTParam] float radius, [BTInOut] ref MyBBMemoryTarget outTarget, [BTInOut] ref MyBBMemoryInt priority)
		{
			if (WolfLogic.SelfDestructionActivated)
			{
				return MyBehaviorTreeState.SUCCESS;
			}
			if (base.Bot?.AgentEntity == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D translation = base.Bot.Navigation.PositionAndOrientation.Translation;
			BoundingSphereD sphere = new BoundingSphereD(translation, radius);
			if (priority == null)
			{
				priority = new MyBBMemoryInt();
			}
			int num = priority.IntValue;
			if (num <= 0 || base.Bot.Navigation.Stuck)
			{
				num = int.MaxValue;
			}
			MyBehaviorTreeState myBehaviorTreeState = IsTargetValid(ref outTarget);
			if (myBehaviorTreeState == MyBehaviorTreeState.FAILURE)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			if (WolfTarget == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			Vector3D? memoryTargetPosition = WolfTarget.GetMemoryTargetPosition(outTarget);
			if (!memoryTargetPosition.HasValue || Vector3D.DistanceSquared(memoryTargetPosition.Value, base.Bot.AgentEntity.PositionComp.GetPosition()) > 160000.0)
			{
				num = 7;
				MyBBMemoryTarget.UnsetTarget(ref outTarget);
			}
			if (memoryTargetPosition.HasValue)
			{
				Vector3D globalPos = memoryTargetPosition.Value;
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(globalPos);
				if (closestPlanet != null)
				{
					Vector3D closestSurfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
					if (Vector3D.DistanceSquared(closestSurfacePointGlobal, globalPos) > 2.25 && Vector3D.DistanceSquared(closestSurfacePointGlobal, base.Bot.AgentEntity.PositionComp.GetPosition()) < 25.0)
					{
						num = 7;
						MyBBMemoryTarget.UnsetTarget(ref outTarget);
					}
				}
			}
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(base.Bot.AgentEntity.ControllerInfo.ControllingIdentityId);
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, list);
			list.ShuffleList();
			foreach (MyEntity item in list)
			{
<<<<<<< HEAD
				if (item == base.Bot.AgentEntity || item is MyVoxelBase || item.IsPreview || item.Physics == null)
				{
					continue;
				}
				int num2 = int.MaxValue;
				int num3 = 6;
				MyCharacter myCharacter = item as MyCharacter;
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (base.Bot.AgentDefinition.TargetCharacters && myCharacter != null)
				{
					if (myCharacter.IsPlayer)
					{
						MySession.Static.RemoteAdminSettings.TryGetValue(myCharacter.ControlSteamId, out var value);
						if (value.HasFlag(AdminSettingsEnum.Untargetable))
						{
							continue;
						}
					}
					if (!WolfTarget.IsEntityReachable(item))
					{
						continue;
					}
					MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
					if ((playerFaction == null || playerFaction2 != playerFaction) && !myCharacter.IsDead)
					{
						MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(myCharacter.WorldMatrix.Translation - 3.0 * myCharacter.WorldMatrix.Up, myCharacter.WorldMatrix.Translation + 3.0 * myCharacter.WorldMatrix.Up, 15);
						if (hitInfo.HasValue && ((IHitInfo)hitInfo).HitEntity != myCharacter && num3 < num)
						{
							myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
							num = 1;
							MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
							m_lastTargetedEntityPosition = myCharacter.PositionComp.GetPosition();
						}
					}
				}
				else
				{
					if (!base.Bot.AgentDefinition.TargetGrids || myCubeGrid == null || num <= 1)
					{
						continue;
					}
					Vector3D value2 = myCubeGrid.WorldToGridScaledLocal(translation);
					double num4 = double.MaxValue;
					MySlimBlock mySlimBlock = null;
					foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
					{
						num3 = GetBlockTargetPriority(cubeBlock);
						if (num3 >= num2)
						{
							continue;
						}
						Vector3D value3 = ((cubeBlock.FatBlock != null) ? (new Vector3D(cubeBlock.Min + cubeBlock.Max) * 0.5) : ((Vector3D)cubeBlock.Position));
						double num5 = Vector3D.RectangularDistance(ref value3, ref value2);
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
				if (item == base.Bot.AgentEntity || item is MyVoxelBase || !WolfTarget.IsEntityReachable(item))
				{
					continue;
				}
				int num2 = 6;
				MyCharacter myCharacter = item as MyCharacter;
				if (myCharacter == null)
				{
					continue;
				}
				MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
				if ((playerFaction == null || playerFaction2 != playerFaction) && !myCharacter.IsDead)
				{
					num2 = 1;
					if (num2 < num)
					{
						myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
						num = num2;
						MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
						m_lastTargetedEntityPosition = myCharacter.PositionComp.GetPosition();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					if (mySlimBlock != null)
					{
						myBehaviorTreeState = MyBehaviorTreeState.SUCCESS;
						num = num2;
						MyBBMemoryTarget.SetTargetCube(ref outTarget, mySlimBlock.Position, myCubeGrid.EntityId);
						m_lastTargetedEntityPosition = mySlimBlock.WorldPosition;
					}
				}
			}
			list.Clear();
			priority.IntValue = num;
			if (outTarget.TargetType == MyAiTargetEnum.NO_TARGET)
			{
				myBehaviorTreeState = MyBehaviorTreeState.FAILURE;
			}
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

		[MyBehaviorTreeAction("IsRunningAway", ReturnsRunning = false)]
		protected MyBehaviorTreeState IsRunningAway()
		{
			if (!m_runAwayPos.HasValue)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			return MyBehaviorTreeState.SUCCESS;
		}

		[MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.INIT)]
		protected MyBehaviorTreeState RunAway_Init()
		{
			return MyBehaviorTreeState.RUNNING;
		}

		[MyBehaviorTreeAction("RunAway")]
		protected MyBehaviorTreeState RunAway([BTParam] float distance)
		{
			if (!m_runAwayPos.HasValue)
			{
				MySteeringBase steeringOfType = base.Bot.Navigation.GetSteeringOfType(typeof(MyCharacterAvoidanceSteering));
				if (steeringOfType != null)
				{
					((MyCharacterAvoidanceSteering)steeringOfType).AvoidPlayer = true;
				}
				Vector3D center = base.Bot.Player.Character.PositionComp.GetPosition();
				Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(center);
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(center);
				if (closestPlanet == null)
				{
					return MyBehaviorTreeState.FAILURE;
				}
				if (m_lastTargetedEntityPosition.HasValue)
				{
					Vector3D globalPos = m_lastTargetedEntityPosition.Value;
					globalPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
					Vector3D value = center - globalPos;
					Vector3D globalPos2 = center + Vector3D.Normalize(value) * distance;
					m_runAwayPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos2);
				}
				else
				{
					vector3D.Normalize();
					Vector3D tangent = Vector3D.CalculatePerpendicularVector(vector3D);
					Vector3D bitangent = Vector3D.Cross(vector3D, tangent);
					tangent.Normalize();
					bitangent.Normalize();
					Vector3D globalPos3 = MyUtils.GetRandomDiscPosition(ref center, distance, distance, ref tangent, ref bitangent);
					m_runAwayPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos3);
				}
				base.AiTargetBase.SetTargetPosition(m_runAwayPos.Value);
				AimWithMovement();
			}
			else if (base.Bot.Navigation.Stuck)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			base.AiTargetBase.GotoTarget();
			if (Vector3D.DistanceSquared(m_runAwayPos.Value, base.Bot.Player.Character.PositionComp.GetPosition()) < 100.0)
			{
				WolfLogic.Remove();
				return MyBehaviorTreeState.SUCCESS;
			}
			return MyBehaviorTreeState.RUNNING;
		}
	}
}
