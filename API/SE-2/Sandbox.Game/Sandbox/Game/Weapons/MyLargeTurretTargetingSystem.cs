using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons
{
	public class MyLargeTurretTargetingSystem
	{
		public enum MyTargetingOption
		{
			/// <summary>
			/// Raycast from barrel to target. Succeeds if target, missile or projectile are hit. No ignored entities.
			/// </summary>
			RegularRaycast,
			/// <summary>
			/// Raycast from target to barrel. Succeeds if shooter, missile, projectile or ignoredEntities are hit.
			/// </summary>
			ReverseRaycast,
			/// <summary>
			/// Full raycast gathering all collisions. Succeeds if only missile, projectile, target or ignoredEntities are hit.
			/// </summary>
			FullRaycast
		}

		[Serializable]
		public struct CurrentTargetSync
		{
			protected class Sandbox_Game_Weapons_MyLargeTurretTargetingSystem_003C_003ECurrentTargetSync_003C_003ETargetId_003C_003EAccessor : IMemberAccessor<CurrentTargetSync, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CurrentTargetSync owner, in long value)
				{
					owner.TargetId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CurrentTargetSync owner, out long value)
				{
					value = owner.TargetId;
				}
			}

			protected class Sandbox_Game_Weapons_MyLargeTurretTargetingSystem_003C_003ECurrentTargetSync_003C_003EIsPotential_003C_003EAccessor : IMemberAccessor<CurrentTargetSync, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CurrentTargetSync owner, in bool value)
				{
					owner.IsPotential = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CurrentTargetSync owner, out bool value)
				{
					value = owner.IsPotential;
				}
			}

			public long TargetId;

			public bool IsPotential;
		}

		[Serializable]
		private struct MyEntityWithDistSq
		{
			protected class Sandbox_Game_Weapons_MyLargeTurretTargetingSystem_003C_003EMyEntityWithDistSq_003C_003EEntity_003C_003EAccessor : IMemberAccessor<MyEntityWithDistSq, MyEntity>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityWithDistSq owner, in MyEntity value)
				{
					owner.Entity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityWithDistSq owner, out MyEntity value)
				{
					value = owner.Entity;
				}
			}

			protected class Sandbox_Game_Weapons_MyLargeTurretTargetingSystem_003C_003EMyEntityWithDistSq_003C_003EPriority_003C_003EAccessor : IMemberAccessor<MyEntityWithDistSq, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityWithDistSq owner, in float value)
				{
					owner.Priority = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityWithDistSq owner, out float value)
				{
					value = owner.Priority;
				}
			}

			protected class Sandbox_Game_Weapons_MyLargeTurretTargetingSystem_003C_003EMyEntityWithDistSq_003C_003EDistance_003C_003EAccessor : IMemberAccessor<MyEntityWithDistSq, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityWithDistSq owner, in double value)
				{
					owner.Distance = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityWithDistSq owner, out double value)
				{
					value = owner.Distance;
				}
			}

			public MyEntity Entity;

			public float Priority;

			public double Distance;

			public override string ToString()
			{
				return Entity.ToString();
			}
		}

		public class MyPositionPrediction
		{
			public enum MyGridTargetingOption
			{
				AABBCenter,
				CenterOfMass
			}

			public const float MINIMAL_DESIRED_DEVIATION = 0.3f;

			public const float MAX_SPEED_FOR_ARTIFICIAL_PERPENDICULAR_TARGETING_PLANE = 1f;

			public const int MAXIMAL_ITERATION_NUMBER_EXCEPT_ACCELERATION = 6;

			private IMyEntity m_target;

			private IMyShootOrigin m_origin;

			private readonly FastResourceLock m_currentPredictionUsageLock = new FastResourceLock();

			private Vector3D m_lastPredictedPoint;

			private Vector3D? m_targetVelocity;

			private Vector3D? m_targetPosition;

			private long m_lastPredictedAt;

			private static bool m_debugDrawMissileInterceptionPrediction = false;

			private static DateTime m_lastTimeDraw = DateTime.Now;

			public MyGridTargetingOption PreferedGridTargetingOption = MyGridTargetingOption.CenterOfMass;

			public bool IsLastPredictedCoordinatesInRange;

			public bool UsePrediction = true;

			public IMyEntity Target
			{
				get
				{
					return m_target;
				}
				set
				{
					if (value != m_target)
					{
						m_lastPredictedPoint = Vector3D.Zero;
						m_lastPredictedAt = 0L;
						m_target = value;
					}
				}
			}

			public IMyShootOrigin Origin
			{
				get
				{
					return m_origin;
				}
				set
				{
					if (value != m_origin)
					{
						m_lastPredictedPoint = Vector3D.Zero;
						m_lastPredictedAt = 0L;
						m_origin = value;
					}
				}
			}

			public bool IsFastPrediction
			{
				get
				{
					MyMissileAmmoDefinition myMissileAmmoDefinition = CurrentAmmoDefininiton as MyMissileAmmoDefinition;
					if (myMissileAmmoDefinition != null)
					{
						if (myMissileAmmoDefinition != null)
						{
							return myMissileAmmoDefinition.MissileAcceleration == 0f;
						}
						return false;
					}
					return true;
				}
			}

			public bool IsTargetPositionManual => m_targetPosition.HasValue;

			private Vector3D ShotOrigin => Origin.ShootOrigin;

			public MyAmmoDefinition CurrentAmmoDefininiton => Origin.GetAmmoDefinition as MyAmmoDefinition;

			private Vector3D OriginLinearVelocity
			{
				get
				{
					MyCubeBlock myCubeBlock;
					if ((myCubeBlock = Origin as MyCubeBlock) != null)
					{
						return myCubeBlock.CubeGrid.LinearVelocity;
					}
					MyCubeGrid myCubeGrid;
					if ((myCubeGrid = Origin as MyCubeGrid) != null)
					{
						return myCubeGrid.LinearVelocity;
					}
					return Vector3D.Zero;
				}
			}

			/// <summary>
			/// Used for one frame only. Can be used for more precise positioning
			/// </summary>
			public Vector3D TargetLinearVelocity
			{
				get
				{
					if (m_targetVelocity.HasValue)
					{
						return m_targetVelocity.Value;
					}
					return GetTargetLinearVelocity(Target);
				}
				set
				{
					m_targetVelocity = value;
				}
			}

			/// <summary>
			/// Used for one frame only. Can be used for more precise positioning
			/// </summary>
			public Vector3D TargetPosition
			{
				get
				{
					if (m_targetPosition.HasValue)
					{
						return m_targetPosition.Value;
					}
					return GetTargetPosition(Target);
				}
				set
				{
					m_targetPosition = value;
				}
			}

			public Vector3D GetTargetLinearVelocity(IMyEntity target)
			{
				if (target == null)
				{
					return Vector3D.Zero;
				}
				MyCubeBlock myCubeBlock;
				if ((myCubeBlock = target as MyCubeBlock) != null)
				{
					return myCubeBlock.CubeGrid.LinearVelocity;
				}
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = target as MyCubeGrid) != null)
				{
					return myCubeGrid.LinearVelocity;
				}
				if (target.Physics != null)
				{
					return target.Physics.LinearVelocityUnsafe;
				}
				return Vector3D.Zero;
			}

			public Vector3D GetTargetPosition(IMyEntity target)
			{
				if (target == null)
				{
					return Vector3D.Zero;
				}
				MyCubeBlock myCubeBlock;
				if ((myCubeBlock = target as MyCubeBlock) != null)
				{
					Vector3 result = ((!myCubeBlock.BlockDefinition.AimingOffset.HasValue) ? Vector3.Zero : myCubeBlock.BlockDefinition.AimingOffset.Value);
					if (result != Vector3.Zero)
					{
						MatrixD matrix = myCubeBlock.PositionComp.WorldMatrixRef.GetOrientation();
						Vector3 normal = myCubeBlock.BlockDefinition.AimingOffset.Value;
						Vector3.TransformNormal(ref normal, ref matrix, out result);
					}
					return myCubeBlock.PositionComp.WorldMatrixRef.Translation + result;
				}
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = target as MyCubeGrid) != null)
				{
					switch (PreferedGridTargetingOption)
					{
					case MyGridTargetingOption.CenterOfMass:
						return MyGridPhysicalGroupData.GetGroupSharedProperties(myCubeGrid).CoMWorld;
					case MyGridTargetingOption.AABBCenter:
						return myCubeGrid.GetPhysicalGroupAABB().Center;
					}
				}
				MyCharacter myCharacter;
				if ((myCharacter = target as MyCharacter) != null)
				{
					Quaternion rotation = myCharacter.GetRotation();
					Vector3D vector3D = new Vector3(rotation.X, rotation.Y, rotation.Z);
					Vector3D vector3D2 = (myCharacter.IsCrouching ? new Vector3(0f, myCharacter.Definition.CrouchHeadServerOffset / 2f, 0f) : new Vector3(0f, myCharacter.Definition.HeadServerOffset / 2f, 0f));
					float w = rotation.W;
					return target.PositionComp.WorldAABB.Center + (2.0 * Vector3D.Dot(vector3D, vector3D2) * vector3D + ((double)(w * w) - Vector3D.Dot(vector3D, vector3D)) * vector3D2 + 2f * w * Vector3D.Cross(vector3D, vector3D2));
				}
				return target.PositionComp.WorldMatrixRef.Translation;
			}

			public MyPositionPrediction(IMyShootOrigin originEntity)
			{
				Origin = originEntity;
			}

			public Vector3D GetTargetCoordinates(IMyEntity currentTarget)
			{
				using (m_currentPredictionUsageLock.AcquireExclusiveUsing())
				{
					if (!UsePrediction)
					{
						IsLastPredictedCoordinatesInRange = true;
						return GetTargetPosition(currentTarget);
					}
					if (m_lastPredictedAt == MySandboxGame.TotalGamePlayTimeInMilliseconds)
					{
						return m_lastPredictedPoint;
					}
					IsLastPredictedCoordinatesInRange = true;
					m_lastPredictedAt = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					m_lastPredictedPoint = GetTargetingCoordinates(ShotOrigin, OriginLinearVelocity, CurrentAmmoDefininiton, GetTargetPosition(currentTarget), GetTargetLinearVelocity(currentTarget), out IsLastPredictedCoordinatesInRange);
					return m_lastPredictedPoint;
				}
			}

			private static Vector3D GetTargetingCoordinates(Vector3D interceptorPosition, Vector3D interceptorOriginLinearVelocity, MyAmmoDefinition interceptorAmmoDefinition, Vector3D targetPosition, Vector3D targetLinearVelocity, out bool isInRange)
			{
				if (!Sync.MultiplayerActive)
				{
					interceptorPosition -= interceptorOriginLinearVelocity * 0.01666666753590107;
					targetPosition -= targetLinearVelocity * 0.01666666753590107;
				}
				if (interceptorAmmoDefinition == null)
				{
					isInRange = true;
					return targetPosition;
				}
				isInRange = (interceptorPosition - targetPosition).Length() < (double)interceptorAmmoDefinition.MaxTrajectory;
				Vector3D vector3D = interceptorPosition;
				MyProjectileAmmoDefinition myProjectileAmmoDefinition = interceptorAmmoDefinition as MyProjectileAmmoDefinition;
				Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D);
				if (interceptorOriginLinearVelocity == Vector3D.Zero && targetLinearVelocity == Vector3D.Zero && (myProjectileAmmoDefinition == null || !MyFakes.PROJECTILES_APPLY_GRAVITY))
				{
					return targetPosition;
				}
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyFakes.LEAD_DEBUGDRAW_MISSILE_PREDICTION_SHORTCUTS_ENABLED && MyInput.Static.IsAnyAltKeyPressed())
				{
					if ((DateTime.Now - m_lastTimeDraw).Seconds > 2 && MyInput.Static.IsKeyPress(MyKeys.R))
					{
						m_debugDrawMissileInterceptionPrediction = true;
						m_lastTimeDraw = DateTime.Now;
					}
					if (MyInput.Static.IsKeyPress(MyKeys.T))
					{
						MyRenderProxy.DebugClearPersistentMessages();
					}
				}
				Vector3D vector3D2 = interceptorOriginLinearVelocity;
				Vector3D vector3D3 = targetLinearVelocity;
				Vector3D vector3D4 = targetPosition;
				Vector3D vector3D5 = targetPosition - interceptorPosition;
				Vector3D deltaVel = targetLinearVelocity - Vector3D.Normalize(vector3D4 - vector3D) * interceptorAmmoDefinition.DesiredSpeed;
				double timeToIntercept = GetTimeToIntercept(vector3D5, deltaVel, interceptorAmmoDefinition.DesiredSpeed);
				double num = -1.0;
				if (myProjectileAmmoDefinition != null)
				{
					targetLinearVelocity -= interceptorOriginLinearVelocity;
					interceptorOriginLinearVelocity = Vector3D.Zero;
					Vector3D velocity_Combined = Vector3D.Zero;
					MyProjectile.SetInitialVelocities(myProjectileAmmoDefinition, Vector3D.ClampToSphere(vector3D5, 1.0), vector3D2, out velocity_Combined);
					num = vector3D5.Length() / deltaVel.Length();
					targetPosition = FindIntersection(interceptorPosition, interceptorOriginLinearVelocity, interceptorAmmoDefinition.DesiredSpeed, targetPosition, targetLinearVelocity);
					Vector3D pointFrom = targetPosition;
					Vector3D vector3D6 = targetPosition;
					if (MyFakes.PROJECTILES_APPLY_GRAVITY && vector != Vector3D.Zero)
					{
						if (MyFakes.LEAD_INDICATOR_DEBUGDRAW_BULLET_DROP)
						{
							MyRenderProxy.DebugDrawSphere(targetPosition, 1f, Color.MediumVioletRed);
						}
						vector3D6 = vector3D;
						double num2 = num / 0.01666666753590107;
						Vector3D zero = Vector3D.Zero;
						if (MyFakes.LEAD_INDICATOR_ITERATIVE_PREDICTION)
						{
							int num3 = 0;
							while ((double)num3 < num2 - 1.0)
							{
								float num4 = 0.0166666675f * MyFakes.SIMULATION_SPEED;
								vector3D6 += (velocity_Combined + vector * num4) * num4;
								num3++;
								if (MyFakes.LEAD_INDICATOR_DEBUGDRAW_BULLET_DROP)
								{
									MyRenderProxy.DebugDrawPoint(vector3D6, Color.MediumVioletRed, depthRead: true);
								}
							}
							zero = vector3D6 - targetPosition;
						}
						else
						{
							zero = Vector3D.Normalize(vector) * 0.5 * vector.Length() * (num * num);
						}
						targetPosition -= zero;
						if (MyFakes.LEAD_INDICATOR_DEBUGDRAW_BULLET_DROP)
						{
							MyRenderProxy.DebugDrawArrow3D(pointFrom, targetPosition, Color.Green);
						}
					}
					return targetPosition;
				}
				MyMissileAmmoDefinition myMissileAmmoDefinition = interceptorAmmoDefinition as MyMissileAmmoDefinition;
				if (myMissileAmmoDefinition == null)
				{
					targetLinearVelocity -= interceptorOriginLinearVelocity;
					interceptorOriginLinearVelocity = Vector3D.Zero;
					vector3D5 = targetPosition - interceptorPosition;
					deltaVel = targetLinearVelocity - interceptorOriginLinearVelocity;
					num = GetTimeToIntercept(vector3D5, deltaVel, interceptorAmmoDefinition.DesiredSpeed);
					targetPosition = FindIntersection(interceptorPosition, interceptorOriginLinearVelocity, interceptorAmmoDefinition.DesiredSpeed, targetPosition, targetLinearVelocity);
					isInRange = (vector3D - targetPosition).Length() < (double)interceptorAmmoDefinition.MaxTrajectory;
					return targetPosition;
				}
				if (myMissileAmmoDefinition != null && myMissileAmmoDefinition.MissileAcceleration == 0f)
				{
					targetLinearVelocity -= interceptorOriginLinearVelocity;
					interceptorOriginLinearVelocity = Vector3D.Zero;
					vector3D5 = targetPosition - interceptorPosition;
					deltaVel = targetLinearVelocity - interceptorOriginLinearVelocity;
					num = GetTimeToIntercept(vector3D5, deltaVel, myMissileAmmoDefinition.MissileInitialSpeed);
					targetPosition = FindIntersection(interceptorPosition, interceptorOriginLinearVelocity, myMissileAmmoDefinition.MissileInitialSpeed, targetPosition, targetLinearVelocity);
					isInRange = (vector3D - targetPosition).Length() < (double)interceptorAmmoDefinition.MaxTrajectory;
					return targetPosition;
				}
				if (myMissileAmmoDefinition != null && myMissileAmmoDefinition.MissileAcceleration > 0f)
				{
					(interceptorPosition - targetPosition).Length();
					double num5 = double.PositiveInfinity;
					bool reachedMaxTrajectory = false;
					int num6 = (int)(timeToIntercept / 0.01666666753590107);
					targetPosition += num6 * vector3D3 * 0.01666666753590107;
					Vector3D vector3D7 = targetPosition + vector3D3 * (float)timeToIntercept;
					Vector3D position = vector3D7;
					Vector3D position2 = targetPosition;
					_ = Vector3D.Zero;
					Vector3D vector3D8 = Vector3D.Zero;
					Vector3D worldCoord = Vector3D.Zero;
					Vector3D from = Vector3D.Zero;
					Vector3D worldCoord2 = Vector3D.Zero;
					int num7 = num6;
					bool flag = true;
					bool flag2 = true;
					Vector3D vector3D9 = vector3D3;
					if (vector3D3 == Vector3D.Zero || vector3D3.Length() < 1.0)
					{
						vector3D9 = Vector3D.CalculatePerpendicularVector(Vector3D.ClampToSphere(vector3D4 - vector3D, 1.0));
					}
					Vector3D vector2 = vector3D9;
					Vector3D vector3 = vector3D4 - vector3D;
					vector3D9.Normalize();
					vector3.Normalize();
					Vector3D vector4 = Vector3D.Cross(vector3D9, vector3);
					Vector3D vector3D10 = targetPosition;
					Vector3D vector3D11 = targetPosition + Vector3D.ClampToSphere(vector2, 100.0);
					Vector3D vector3D12 = targetPosition + Vector3D.ClampToSphere(vector4, 100.0);
					PlaneD planeD = new PlaneD(vector3D10, vector3D11, vector3D12);
					if (m_debugDrawMissileInterceptionPrediction)
					{
						MyRenderProxy.DebugDrawText3D(vector3D10, "x", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
						MyRenderProxy.DebugDrawArrow3D(vector3D10, vector3D11, Color.Green, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
						MyRenderProxy.DebugDrawArrow3D(vector3D10, vector3D12, Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					}
					List<float> list = new List<float>();
					float num8 = 0f;
					bool flag3 = false;
					Vector3D vector3D13 = vector3D;
					while (!(num5 < 0.30000001192092896 || reachedMaxTrajectory))
					{
						interceptorPosition = vector3D;
						Vector3D vector3D14 = vector3D7 + (num6 - num7) * targetLinearVelocity * 0.01666666753590107;
						Vector3D vector3D15 = vector3D - vector3D14;
						Vector3 missileInitialSpeed = MyGunBase.GetMissileInitialSpeed(vector3D2, vector3D14 - vector3D, myMissileAmmoDefinition);
						vector3D13 = vector3D + missileInitialSpeed * 0.0166666675f;
						Vector3D finalInterceptorPosition;
						bool reachedMaxSpeed;
						Vector3D spatialDeviation = GetSpatialDeviation(vector3D13, vector3D2, targetPosition, myMissileAmmoDefinition, vector3D13, num6, vector3D14, out reachedMaxTrajectory, out finalInterceptorPosition, out reachedMaxSpeed);
						if (reachedMaxTrajectory || num6 - num7 > 2)
						{
							if (reachedMaxTrajectory)
							{
								isInRange = false;
							}
							if (m_debugDrawMissileInterceptionPrediction)
							{
								MyRenderProxy.DebugDrawSphere(vector3D7, 1.5f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								MyRenderProxy.DebugDrawSphere(position2, 1.5f, Color.Green, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								MyRenderProxy.DebugDrawSphere(position, 1.5f, Color.Blue, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								MyRenderProxy.DebugDrawText3D(vector3D8, "1", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
								MyRenderProxy.DebugDrawText3D(worldCoord, "2", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
								MyRenderProxy.DebugDrawText3D(worldCoord2, "3", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
								MyRenderProxy.DebugDrawSphere(from, 0.15f, Color.Orange, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								MyRenderProxy.DebugDrawSphere(vector3D, 0.2f, Color.Gold, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
							}
							m_debugDrawMissileInterceptionPrediction = false;
							return vector3D7;
						}
						if (m_debugDrawMissileInterceptionPrediction)
						{
							MyRenderProxy.DebugDrawPoint(vector3D14, Color.Red, depthRead: true, persistent: true);
							MyRenderProxy.DebugDrawPoint(targetPosition, Color.Green, depthRead: true, persistent: true);
							MyRenderProxy.DebugDrawPoint(finalInterceptorPosition, Color.Blue, depthRead: true, persistent: true);
							if (flag)
							{
								MyRenderProxy.DebugDrawSphere(vector3D7, 0.5f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								MyRenderProxy.DebugDrawSphere(position2, 0.5f, Color.Green, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								MyRenderProxy.DebugDrawSphere(finalInterceptorPosition, 0.5f, Color.Blue, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
								flag = false;
							}
						}
						double num9 = spatialDeviation.Length();
						double num10 = num9;
						vector3D15 = vector3D - finalInterceptorPosition;
						vector3D15.Normalize();
						Vector3D vector3D16 = planeD.Intersection(ref from, ref vector3D15);
						Vector3D vector3D17 = FindNearestPointOnLine(vector3D13, vector3D13 - vector3D16, targetPosition);
						Vector3D vector3D18 = FindNearestPointOnLine(vector3D4, vector3D9, vector3D16);
						float num11 = (float)(vector3D17 - targetPosition).Length();
						Vector3D vector3D19 = vector3D18 - vector3D16;
						Vector3D vector3D20 = Vector3D.ClampToSphere(targetPosition - vector3D18, num11 * 2f);
						double num12 = vector3D19.Length();
						double num13 = vector3D20.Length();
						if (num9 < num5)
						{
							worldCoord2 = vector3D17;
							from = vector3D13;
							position = finalInterceptorPosition;
							num5 = num9;
							vector3D7 = vector3D14;
							num7 = num6;
							position2 = targetPosition;
							vector3D15.Normalize();
							vector3D8 = planeD.Intersection(ref from, ref vector3D15);
							worldCoord = FindNearestPointOnLine(vector3D4, vector3D9, vector3D8);
							isInRange = !reachedMaxTrajectory;
						}
						if (num12 > 0.30000001192092896 || num13 > 0.30000001192092896)
						{
							vector3D14 += vector3D19;
							vector3D14 += vector3D20;
							if (m_debugDrawMissileInterceptionPrediction)
							{
								MyRenderProxy.DebugDrawPoint(vector3D14, Color.White, depthRead: true, persistent: true);
							}
							vector3D15 = vector3D - vector3D14;
							missileInitialSpeed = MyGunBase.GetMissileInitialSpeed(vector3D2, vector3D14 - vector3D, myMissileAmmoDefinition);
							vector3D13 = vector3D + missileInitialSpeed * 0.0166666675f;
							num9 = GetSpatialDeviation(vector3D13, vector3D2, targetPosition, myMissileAmmoDefinition, vector3D13, num6, vector3D14, out reachedMaxTrajectory, out finalInterceptorPosition, out reachedMaxSpeed).Length();
							if (num9 < num10)
							{
								num10 = num9;
							}
							if (num9 < num5)
							{
								worldCoord2 = vector3D17;
								from = vector3D13;
								num7 = num6;
								position = finalInterceptorPosition;
								vector3D7 = vector3D14;
								num5 = num9;
								position2 = targetPosition;
								vector3D8 = planeD.Intersection(ref from, ref vector3D15);
								worldCoord = FindNearestPointOnLine(vector3D4, vector3D9, vector3D8);
								worldCoord2 = FindNearestPointOnLine(vector3D13, vector3D13 - vector3D8, targetPosition);
								isInRange = !reachedMaxTrajectory;
							}
						}
						num6++;
						if (!flag3 && !flag2)
						{
							double num14 = (double)num8 - num10;
							list.Add((float)num14);
						}
						if (!flag3 && list.Count >= 3 && reachedMaxSpeed)
						{
							float num15 = 0f;
							foreach (float item in list)
							{
								num15 += item;
							}
							num15 /= (float)list.Count;
							int num16 = (int)((targetPosition - finalInterceptorPosition).Length() / (double)num15);
							if (num16 > 3)
							{
								int num17 = num16 - 3;
								num6 += num17;
								Vector3D vector3D21 = (float)num17 * 0.0166666675f * vector3D3;
								vector3D14 += vector3D21;
							}
							num7 = num6 - 1;
							flag3 = true;
						}
						targetPosition = vector3D4 + vector3D3 * ((float)num6 * 0.0166666675f);
						num8 = (float)num10;
						flag2 = false;
					}
					if (m_debugDrawMissileInterceptionPrediction)
					{
						MyRenderProxy.DebugDrawSphere(vector3D7, 1.5f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
						MyRenderProxy.DebugDrawSphere(position2, 1.5f, Color.Green, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
						MyRenderProxy.DebugDrawSphere(position, 1.5f, Color.Blue, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
						MyRenderProxy.DebugDrawText3D(vector3D8, "1", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
						MyRenderProxy.DebugDrawText3D(worldCoord, "2", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
						MyRenderProxy.DebugDrawText3D(worldCoord2, "3", Color.Gold, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
						MyRenderProxy.DebugDrawSphere(from, 0.15f, Color.Orange, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
						MyRenderProxy.DebugDrawSphere(vector3D, 0.2f, Color.Gold, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
					}
					m_debugDrawMissileInterceptionPrediction = false;
					return vector3D7;
				}
				return targetPosition;
			}

			private static Vector3D FindNearestPointOnLine(Vector3D origin, Vector3D direction, Vector3D point)
			{
				direction.Normalize();
				double num = Vector3D.Dot(point - origin, direction);
				return origin + direction * num;
			}

			private static Vector3D GetSpatialDeviation(Vector3D interceptorPositionOriginal, Vector3D interceptorVelocityOriginal, Vector3D targetPosition, MyMissileAmmoDefinition missileInterceptorAmmoDefinition, Vector3D interceptorPosition, int i, Vector3D aimPosition, out bool reachedMaxTrajectory, out Vector3D finalInterceptorPosition, out bool reachedMaxSpeed)
			{
				Vector3D vector3D = Vector3D.ClampToSphere(aimPosition - interceptorPositionOriginal, 1.0);
				Vector3 vector = MyGunBase.GetMissileInitialSpeed(interceptorVelocityOriginal, vector3D, missileInterceptorAmmoDefinition);
				int j = 0;
				reachedMaxTrajectory = false;
				reachedMaxSpeed = false;
				float num = 0f;
				bool reachedMaxSpeed2 = false;
				for (; j < i; j++)
				{
					if (num > missileInterceptorAmmoDefinition.MaxTrajectory)
					{
						reachedMaxTrajectory = true;
						finalInterceptorPosition = interceptorPosition;
						reachedMaxSpeed = true;
						return targetPosition - interceptorPosition;
					}
					if (reachedMaxSpeed2)
					{
						reachedMaxSpeed = true;
						int num2 = i - j;
						vector *= (float)num2;
						j = i;
					}
					else
					{
						vector = MyMissile.GetMissileSpeedNextFrame(vector, vector3D, missileInterceptorAmmoDefinition, out reachedMaxSpeed2);
					}
					Vector3 vector2 = vector * 0.0166666675f;
					interceptorPosition += vector2;
					num += vector2.Length();
				}
				if (num > missileInterceptorAmmoDefinition.MaxTrajectory)
				{
					reachedMaxTrajectory = true;
				}
				finalInterceptorPosition = interceptorPosition;
				return targetPosition - interceptorPosition;
			}

			private static double GetTimeToIntercept(Vector3D deltaPos, Vector3D deltaVel, double projectileVel)
			{
				double num = Vector3D.Dot(deltaVel, deltaVel) - projectileVel * projectileVel;
				double num2 = 2.0 * Vector3D.Dot(deltaVel, deltaPos);
				double num3 = Vector3D.Dot(deltaPos, deltaPos);
				double num4 = num2 * num2 - 4.0 * num * num3;
				if (!(num4 > 0.0))
				{
					return -1.0;
				}
				return 2.0 * num3 / (Math.Sqrt(num4) - num2);
			}

			private static Vector3D FindIntersection(Vector3D interceptorPosition, Vector3D interceptorOriginLinearVelocity, float interceptorAvgLinearVelocityLenght, Vector3D targetPosition, Vector3D targetLinearVelocity)
			{
				Vector3D result = targetPosition;
				Vector3D deltaPos = targetPosition - interceptorPosition;
				Vector3D deltaVel = targetLinearVelocity - interceptorOriginLinearVelocity;
				double timeToIntercept = GetTimeToIntercept(deltaPos, deltaVel, interceptorAvgLinearVelocityLenght);
				if (timeToIntercept > 0.0)
				{
					result = targetPosition + targetLinearVelocity * timeToIntercept;
				}
				return result;
			}
		}

		private class MyTargetSelectionWorkData : WorkData
		{
			public MyEntity SuggestedTarget;

			public bool SuggestedTargetIsPotential;

			public MyGridTargeting GridTargeting;

			public MyEntity Entity;
		}

		public static float EXACT_VISIBILITY_TEST_TRESHOLD_ANGLE = 0.85f;

		public static bool DEBUG_DRAW_TARGET_PREDICTION = false;

		private const int MIN_FRAMES_FOR_INTERCEPTION_PREDICTION = 60;

		private const int MAX_FRAMES_FOR_INTERCEPTION_PREDICTION = 1200;

		private const int NotVisibleCachePeriod = 4;

		private const int LongTimeNotVisibleCachePeriod = 12;

		private readonly int m_VisibleCachePeriod = 3;

		private MyTargetingOption m_raycastMode = MyTargetingOption.FullRaycast;

		private Dictionary<long, MyEntity> m_raycastIgnoredEntities = new Dictionary<long, MyEntity>();

		private List<MyPhysics.HitInfo> m_hitCollector = new List<MyPhysics.HitInfo>();

		[ThreadStatic]
		private static List<MyEntityWithDistSq> m_allPotentialTargets;

		private IMyTargetingReceiver m_targetReceiver;

		/// <summary>
		/// All targets across all turrets. Used to prevent all turrets shooting at the same block.
		/// </summary>
		private static readonly HashSet<MyEntity> m_allAimedTargets = new HashSet<MyEntity>();

		private int m_notVisibleTargetsUpdatesSinceRefresh;

		private ConcurrentDictionary<MyEntity, int> m_notVisibleTargets = new ConcurrentDictionary<MyEntity, int>();

		private ConcurrentDictionary<MyEntity, int> m_lastNotVisibleTargets = new ConcurrentDictionary<MyEntity, int>();

		private ConcurrentDictionary<MyEntity, int> m_visibleTargets = new ConcurrentDictionary<MyEntity, int>();

		private ConcurrentDictionary<MyEntity, int> m_lastVisibleTargets = new ConcurrentDictionary<MyEntity, int>();

		public MyPositionPrediction TargetPrediction;

		private MyTargetSelectionWorkData m_targetSelectionWorkData = new MyTargetSelectionWorkData();

		private bool m_parallelTargetSelectionInProcess;

		private Task m_findNearTargetsTask;

		private bool m_cancelTargetSelection;

		private readonly FastResourceLock m_targetSelectionLock = new FastResourceLock();

		private bool m_checkOtherTargets = true;

		private bool m_isPotentialTarget;

		private float[] m_distanceEntityKeys;

		private MyCubeGrid m_forcedTarget;

		private MyEntity[] m_forcedTargetListWrapped = new MyEntity[1];

		private float m_forcedTargetRange;

		private List<MyEntity> m_forcedTargetBlocks = new List<MyEntity>();

		private long m_forcedTargetLastUpdate;

		public bool IsPotentialTarget
		{
			get
			{
				return m_isPotentialTarget;
			}
			set
			{
				m_isPotentialTarget = value;
			}
		}

		public bool CheckOtherTargets
		{
			get
			{
				return m_checkOtherTargets;
			}
			set
			{
				m_checkOtherTargets = value;
			}
		}

		public MyEntity Target
		{
			get
			{
				return TargetPrediction.Target as MyEntity;
			}
			set
			{
				TargetPrediction.Target = value;
			}
		}

		public Vector3D AimPosition
		{
			get
			{
				if (Target != null)
				{
					return TargetPrediction.GetTargetCoordinates(Target);
				}
				return Vector3D.Zero;
			}
		}

		public bool IsTargetForced => m_forcedTarget != null;

		public MyLargeTurretTargetingSystem(IMyTargetingReceiver targetReceiver, MyTargetingOption raycastMode = MyTargetingOption.FullRaycast)
		{
			m_targetReceiver = targetReceiver;
			IsPotentialTarget = false;
			TargetPrediction = new MyPositionPrediction(m_targetReceiver);
			m_raycastMode = raycastMode;
		}

		public void ClearReverseRaycastIgnoredEntities()
		{
			m_raycastIgnoredEntities.Clear();
		}

		public void AddReverseRaycastIgnoredEntity(long entityId, MyEntity entity)
		{
			m_raycastIgnoredEntities[entityId] = entity;
		}

		public void RemoveReverseRaycastIgnoredEntity(long entityId)
		{
			m_raycastIgnoredEntities.Remove(entityId);
		}

		public bool IsTarget(MyEntity entity)
		{
			if (entity.Physics != null && !entity.Physics.Enabled)
			{
				return false;
			}
			if (entity is MyMissile)
			{
				return m_targetReceiver.TargetMissiles;
			}
			if (entity is MyMeteor)
			{
				return m_targetReceiver.TargetMeteors;
			}
			if (entity is MyDebrisBase)
			{
				return false;
			}
			MyCharacter myCharacter;
			if ((myCharacter = entity as MyCharacter) != null)
			{
				if (m_targetReceiver.TargetCharacters)
				{
					return !myCharacter.IsDead;
				}
				return false;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = entity as MyCubeBlock) != null)
			{
				if (!myCubeBlock.IsFunctional)
				{
					return false;
				}
				return myCubeBlock.BlockDefinition.MatchingTurretTargetingGroup(m_targetReceiver.GetTargetingGroupHash());
			}
			return false;
		}

		public bool IsValidTarget(MyEntity target)
		{
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = target as MyCubeGrid) != null)
			{
				return myCubeGrid.BigOwners.Count == 0;
			}
			IMyComponentOwner<MyIDModule> myComponentOwner;
			if ((myComponentOwner = target as IMyComponentOwner<MyIDModule>) != null)
			{
				MyCubeBlock myCubeBlock;
				if (myComponentOwner.GetComponent(out var component))
				{
					if (IsTargetIdentityEnemy(component.Owner))
					{
						return m_targetReceiver.TargetEnemies;
					}
					if (IsTargetIdentityFriend(component.Owner))
					{
						return m_targetReceiver.TargetFriends;
					}
					if (IsTargetIdentityNeutral(component.Owner))
					{
						return m_targetReceiver.TargetNeutrals;
					}
				}
				else if ((myCubeBlock = target as MyCubeBlock) != null)
				{
					List<long> bigOwners = myCubeBlock.CubeGrid.BigOwners;
					if (bigOwners.Count == 0)
					{
						return m_targetReceiver.TargetNeutrals;
					}
					switch (MyIDModule.GetRelationPlayerPlayer(bigOwners[0], m_targetReceiver.OwnerIdentityId))
					{
					case MyRelationsBetweenPlayers.Self:
					case MyRelationsBetweenPlayers.Allies:
						return m_targetReceiver.TargetFriends;
					case MyRelationsBetweenPlayers.Neutral:
						return m_targetReceiver.TargetNeutrals;
					case MyRelationsBetweenPlayers.Enemies:
						return m_targetReceiver.TargetEnemies;
					default:
						return true;
					}
				}
				return false;
			}
			MyMissile myMissile;
			if ((myMissile = target as MyMissile) != null)
			{
				return IsTargetIdentityEnemy(myMissile.Owner);
			}
			return true;
		}

		public bool IsTargetVisible()
		{
			if (!TargetPrediction.IsTargetPositionManual && Target != null && !IsTargetVisible(Target, null, useVisibilityCache: false))
			{
				return false;
			}
			return true;
		}

		public bool IsTargetVisible(MyEntity target, Vector3D? overridePredictedPos = null, bool useVisibilityCache = true)
		{
			if (target == null || target.GetTopMostParent().Physics == null || IsTargetInSafeZone(target))
			{
				return false;
			}
			Vector3D vector3D;
			if (overridePredictedPos.HasValue)
			{
				vector3D = overridePredictedPos.Value;
			}
			else
			{
				bool usePrediction = TargetPrediction.UsePrediction;
				if (!TargetPrediction.IsFastPrediction)
				{
					TargetPrediction.UsePrediction = false;
				}
				vector3D = TargetPrediction.GetTargetCoordinates(target);
				TargetPrediction.UsePrediction = usePrediction;
				if (!TargetPrediction.IsLastPredictedCoordinatesInRange)
				{
					return false;
				}
			}
			if (!overridePredictedPos.HasValue && m_notVisibleTargets.TryGetValue(target, out var value) && value > 0 && Sync.IsServer)
			{
				return false;
			}
			if (useVisibilityCache)
			{
				if (m_visibleTargets.ContainsKey(target))
				{
					return true;
				}
				SetTargetVisible(target, visible: false, 1);
			}
			Vector3D shootDirection = m_targetReceiver.ShootDirection;
			Vector3D from = m_targetReceiver.ShootOrigin;
			Vector3D to = vector3D;
			if (Vector3D.Dot(Vector3D.Normalize(to - from), shootDirection) > (double)EXACT_VISIBILITY_TEST_TRESHOLD_ANGLE)
			{
				from = m_targetReceiver.ShootOrigin + shootDirection * 0.5;
			}
			if (useVisibilityCache)
			{
				switch (m_raycastMode)
				{
				default:
					MyPhysics.CastRayParallel(ref from, ref to, 15, delegate(MyPhysics.HitInfo? physTarget)
					{
						OnVisibilityRayCastRegularCompleted(target, physTarget);
					});
					break;
				case MyTargetingOption.ReverseRaycast:
					MyPhysics.CastRayParallel(ref to, ref from, 15, delegate(MyPhysics.HitInfo? physTarget)
					{
						OnVisibilityRayCastReverseCompleted(target, physTarget);
					});
					break;
				case MyTargetingOption.FullRaycast:
					MyPhysics.CastRayParallel(ref from, ref to, m_hitCollector, 15, delegate(List<MyPhysics.HitInfo> physTargets)
					{
						OnVisibilityRayCastFullCompleted(target, physTargets, (MyEntity)m_targetReceiver);
					});
					break;
				}
				return true;
			}
			switch (m_raycastMode)
			{
			default:
			{
				MyPhysics.HitInfo? physTarget3 = MyPhysics.CastRay(from, to, 15);
				return OnVisibilityRayCastRegularCompleted(target, physTarget3);
			}
			case MyTargetingOption.ReverseRaycast:
			{
				MyPhysics.HitInfo? physTarget2 = MyPhysics.CastRay(to, from, 15);
				return OnVisibilityRayCastReverseCompleted(target, physTarget2);
			}
			case MyTargetingOption.FullRaycast:
				MyPhysics.CastRay(from, to, m_hitCollector, 15);
				return OnVisibilityRayCastFullCompleted(target, m_hitCollector, (MyEntity)m_targetReceiver);
			}
		}

		public void CheckAndSelectNearTargetsParallel()
		{
			if (m_parallelTargetSelectionInProcess)
			{
				return;
			}
			using (m_targetSelectionLock.AcquireExclusiveUsing())
			{
				m_cancelTargetSelection = false;
			}
			m_parallelTargetSelectionInProcess = true;
			m_targetSelectionWorkData.SuggestedTarget = Target;
			m_targetSelectionWorkData.SuggestedTargetIsPotential = m_isPotentialTarget;
			m_targetSelectionWorkData.GridTargeting = m_targetReceiver.GridTargeting;
			m_targetSelectionWorkData.Entity = m_targetReceiver.Entity;
			if (true)
			{
				m_findNearTargetsTask = Parallel.Start(delegate(WorkData data)
				{
					MyTargetSelectionWorkData myTargetSelectionWorkData2 = data as MyTargetSelectionWorkData;
					if (myTargetSelectionWorkData2 != null && myTargetSelectionWorkData2.Entity != null && !myTargetSelectionWorkData2.Entity.MarkedForClose)
					{
						CheckNearTargets(ref myTargetSelectionWorkData2.SuggestedTarget, ref myTargetSelectionWorkData2.SuggestedTargetIsPotential);
					}
				}, delegate(WorkData data)
				{
					using (m_targetSelectionLock.AcquireSharedUsing())
					{
						if (m_cancelTargetSelection)
						{
							m_parallelTargetSelectionInProcess = false;
						}
						else
						{
							MyTargetSelectionWorkData myTargetSelectionWorkData = data as MyTargetSelectionWorkData;
							if (myTargetSelectionWorkData != null && myTargetSelectionWorkData.Entity != null && !myTargetSelectionWorkData.Entity.MarkedForClose)
							{
								SetTarget(myTargetSelectionWorkData.SuggestedTarget, myTargetSelectionWorkData.SuggestedTargetIsPotential);
							}
							m_parallelTargetSelectionInProcess = false;
						}
					}
				}, m_targetSelectionWorkData);
			}
			else
			{
				CheckNearTargets(ref m_targetSelectionWorkData.SuggestedTarget, ref m_targetSelectionWorkData.SuggestedTargetIsPotential);
				SetTarget(m_targetSelectionWorkData.SuggestedTarget, m_targetSelectionWorkData.SuggestedTargetIsPotential);
				m_parallelTargetSelectionInProcess = false;
			}
		}

		public double GetTargetDistanceSquared()
		{
			if (Target != null)
			{
				return (Target.PositionComp.GetPosition() - m_targetReceiver.ShootOrigin).LengthSquared();
			}
			float searchRange = m_targetReceiver.SearchRange;
			return searchRange * searchRange;
		}

		public void ResetTarget()
		{
			SetTarget(null, isPotential: true);
		}

		public void TargetChanged()
		{
			MyEntity entity = null;
			if (m_targetReceiver.TargetSync.Value.TargetId != 0L)
			{
				MyEntities.TryGetEntityById(m_targetReceiver.TargetSync.Value.TargetId, out entity);
			}
			SetTarget(entity, m_targetReceiver.TargetSync.Value.IsPotential);
		}

		public void FinishTasks()
		{
			m_findNearTargetsTask.WaitOrExecute();
		}

		public void OnTargetFlagChanged()
		{
			MyEntity nearestTarget = Target;
			double minDistanceSq = 0.0;
			using (MyUtils.ReuseCollection(ref m_allPotentialTargets))
			{
				if (Target != null && !TestPotentialTarget(Target, ref nearestTarget, ref minDistanceSq, m_targetReceiver.SearchRange, m_allPotentialTargets))
				{
					using (m_targetSelectionLock.AcquireExclusiveUsing())
					{
						m_cancelTargetSelection = true;
					}
					SetTarget(null, isPotential: true);
				}
			}
		}

		public void ForceTarget(MyEntity entity, bool usePrediction, bool isPotential = false)
		{
			SetTarget(entity, isPotential);
			TargetPrediction.UsePrediction = usePrediction;
			CheckOtherTargets = false;
		}

		public bool CheckForcedTarget(MyEntity entity)
		{
			if (m_forcedTarget != null)
			{
				return m_forcedTarget == entity;
			}
			return false;
		}

		public void ForceGridTarget(MyCubeGrid entity, float range)
		{
			if (entity != null)
			{
				m_forcedTarget = entity;
				m_forcedTargetListWrapped[0] = entity;
				m_forcedTargetRange = range;
				Target = null;
				m_forcedTargetLastUpdate = 0L;
			}
			else
			{
				m_forcedTarget = null;
				m_forcedTargetListWrapped[0] = null;
				m_forcedTargetRange = 0f;
			}
		}

		private void RecomputeForcedTargetBlocks()
		{
			if (MySession.Static.GameplayFrameCounter - m_forcedTargetLastUpdate >= 100)
			{
				m_forcedTargetBlocks.Clear();
				m_targetSelectionWorkData.GridTargeting.ProcessGrid(m_forcedTarget, m_targetReceiver.SearchRange, ref m_forcedTargetBlocks);
				m_forcedTargetLastUpdate = MySession.Static.GameplayFrameCounter;
			}
		}

		public void ForgetGridTarget()
		{
			m_forcedTarget = null;
			m_forcedTargetBlocks.Clear();
			SetTarget(null, isPotential: false);
		}

		public void SetTargetPosition(Vector3D pos, Vector3D velocity, bool usePrediction)
		{
			CheckOtherTargets = false;
			ResetTarget();
			TargetPrediction.TargetPosition = pos;
			TargetPrediction.TargetLinearVelocity = velocity;
			TargetPrediction.UsePrediction = usePrediction;
			IsPotentialTarget = false;
		}

		public void ResetTargetParams()
		{
			ResetTarget();
			CheckOtherTargets = true;
		}

		public void UpdateVisibilityCache()
		{
			m_notVisibleTargetsUpdatesSinceRefresh++;
			if (!m_parallelTargetSelectionInProcess)
			{
				UpdateVisibilityCacheCounters();
			}
		}

		private void SetTarget(MyEntity target, bool isPotential)
		{
			m_isPotentialTarget = isPotential;
			if (Target == target)
			{
				return;
			}
			MyEntity target2 = Target;
			if (target2 != null)
			{
				target2.OnClose -= m_target_OnClose;
				if (target2 is MyCharacter)
				{
					(target2 as MyCharacter).CharacterDied -= m_target_OnClose;
				}
				m_allAimedTargets.Remove(target2);
				MyHud.LargeTurretTargets.UnregisterMarker(target2);
			}
			Target = target;
			if (Target != null)
			{
				Target.OnClose += m_target_OnClose;
				if (Target is MyCharacter)
				{
					(Target as MyCharacter).CharacterDied += m_target_OnClose;
				}
				if (!m_isPotentialTarget)
				{
					m_allAimedTargets.Add(Target);
				}
				MyHudEntityParams myHudEntityParams = default(MyHudEntityParams);
				myHudEntityParams.FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ICON;
				MyHudEntityParams hudParams = myHudEntityParams;
				if (MySession.Static.LocalCharacter != null && !m_isPotentialTarget && (m_targetReceiver.IsTargetLocked || Vector3D.DistanceSquared(MySession.Static.LocalCharacter.PositionComp.GetPosition(), m_targetReceiver.EntityPosition) < (double)(m_targetReceiver.ShootRangeSimple * m_targetReceiver.ShootRangeSimple)) && m_targetReceiver.HasLocalPlayerAccess() && TestTarget(target))
				{
					MyHud.LargeTurretTargets.RegisterMarker(Target, hudParams);
				}
			}
			if (target2 != Target && Sync.IsServer)
			{
				m_targetReceiver.TargetSync.Value = new CurrentTargetSync
				{
					TargetId = ((Target == null) ? 0 : Target.EntityId),
					IsPotential = m_isPotentialTarget
				};
			}
		}

		private bool OnVisibilityRayCastRegularCompleted(MyEntity target, MyPhysics.HitInfo? physTarget)
		{
			IMyEntity myEntity = null;
			if (physTarget.HasValue && physTarget.Value.HkHitInfo.Body != null && physTarget.Value.HkHitInfo.Body.UserObject != null && physTarget.Value.HkHitInfo.Body.UserObject is MyPhysicsBody)
			{
				myEntity = ((MyPhysicsBody)physTarget.Value.HkHitInfo.Body.UserObject).Entity;
			}
			if (myEntity == null || target == myEntity || target.Parent == myEntity || (target.Parent != null && target.Parent == myEntity.Parent) || myEntity is MyMissile || myEntity is MyFloatingObject)
			{
				SetTargetVisible(target, visible: true);
				return true;
			}
			SetTargetVisible(target, visible: false);
			return false;
		}

		private bool OnVisibilityRayCastReverseCompleted(MyEntity target, MyPhysics.HitInfo? physTarget)
		{
			IMyEntity myEntity = null;
			if (physTarget.HasValue && physTarget.Value.HkHitInfo.Body != null && physTarget.Value.HkHitInfo.Body.UserObject != null && physTarget.Value.HkHitInfo.Body.UserObject is MyPhysicsBody)
			{
				myEntity = ((MyPhysicsBody)physTarget.Value.HkHitInfo.Body.UserObject).Entity;
			}
			long? num = null;
			MyCubeGrid myCubeGrid;
			MyCubeBlock myCubeBlock;
			if ((myCubeGrid = myEntity as MyCubeGrid) != null)
			{
				num = myCubeGrid.EntityId;
			}
			else if ((myCubeBlock = myEntity as MyCubeBlock) != null)
			{
				num = myCubeBlock.CubeGrid.EntityId;
			}
			if (myEntity == null || (num.HasValue && m_raycastIgnoredEntities.ContainsKey(num.Value)) || myEntity is MyMissile || myEntity is MyFloatingObject)
			{
				SetTargetVisible(target, visible: true);
				return true;
			}
			SetTargetVisible(target, visible: false);
			return false;
		}

		private bool OnVisibilityRayCastFullCompleted(MyEntity target, List<MyPhysics.HitInfo> physTargets, MyEntity ignoredTarget = null)
		{
			IMyEntity myEntity = null;
			foreach (MyPhysics.HitInfo physTarget in physTargets)
			{
				if (physTarget.HkHitInfo.Body != null && physTarget.HkHitInfo.Body.UserObject != null && physTarget.HkHitInfo.Body.UserObject is MyPhysicsBody)
				{
					myEntity = ((MyPhysicsBody)physTarget.HkHitInfo.Body.UserObject).Entity;
				}
				if (ignoredTarget != null && myEntity == ignoredTarget)
				{
					continue;
				}
				long? num = null;
				MyCubeGrid myCubeGrid;
				MyCubeBlock myCubeBlock2;
				if ((myCubeGrid = myEntity as MyCubeGrid) != null)
				{
					myCubeGrid.GetTargetedBlock(physTarget.Position);
					num = myCubeGrid.EntityId;
					MyCubeBlock myCubeBlock;
					if (ignoredTarget != null && (myCubeBlock = ignoredTarget as MyCubeBlock) != null && myCubeGrid.GetTargetedBlock(physTarget.Position) == myCubeBlock.SlimBlock)
					{
						continue;
					}
				}
				else if ((myCubeBlock2 = myEntity as MyCubeBlock) != null)
				{
					num = myCubeBlock2.CubeGrid.EntityId;
				}
				if (myEntity != null && target != myEntity && target.Parent != myEntity && (target.Parent == null || target.Parent != myEntity.Parent) && (!num.HasValue || !m_raycastIgnoredEntities.ContainsKey(num.Value)) && !(myEntity is MyMissile) && !(myEntity is MyFloatingObject))
				{
					SetTargetVisible(target, visible: false);
					return false;
				}
			}
			SetTargetVisible(target, visible: true);
			return true;
		}

		private bool IsTargetInSafeZone(MyEntity target)
		{
			return !MySessionComponentSafeZones.IsActionAllowed(target.PositionComp.GetPosition(), MySafeZoneAction.Shooting, 0L, 0uL);
		}

		public void BlacklistTarget(MyEntity target, int time)
		{
			SetTargetVisible(target, visible: false, time);
		}

		private void SetTargetVisible(MyEntity target, bool visible, int? timeout = null)
		{
			if (visible)
			{
				m_notVisibleTargets.Remove(target);
				m_visibleTargets.TryAdd(target, m_VisibleCachePeriod);
			}
			else if (timeout.HasValue)
			{
				m_notVisibleTargets[target] = timeout.Value;
			}
			else if (m_notVisibleTargets.ContainsKey(target))
			{
				m_notVisibleTargets[target] = 12 + MyRandom.Instance.Next(4);
			}
			else
			{
				m_notVisibleTargets.TryAdd(target, 4 + MyRandom.Instance.Next(4));
			}
		}

		private bool IsTargetRootValid(MyEntity targetRoot, MyCubeGrid thisTopmostParent)
		{
			if (targetRoot == null || targetRoot.Closed || targetRoot.MarkedForClose)
			{
				return false;
			}
			MyEntity myEntity = targetRoot.GetTopMostParent() ?? targetRoot;
			if (myEntity.Physics == null || myEntity.MarkedForClose || !myEntity.Physics.Enabled)
			{
				return false;
			}
			bool flag = false;
			MyCubeGrid myCubeGrid = targetRoot as MyCubeGrid;
			if (myCubeGrid != null)
			{
				flag = thisTopmostParent.GridSystems.TerminalSystem == myCubeGrid.GridSystems.TerminalSystem && myCubeGrid.BigOwners.Contains(m_targetReceiver.OwnerIdentityId);
				if (MyCubeGridGroups.Static.Logical.HasSameGroup(thisTopmostParent, myCubeGrid))
				{
					return false;
				}
			}
			if (!flag && myCubeGrid != null)
			{
				if (!m_targetReceiver.TargetSmallGrids && myCubeGrid.GridSizeEnum == MyCubeSize.Small)
				{
					return false;
				}
				if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					if (!m_targetReceiver.TargetLargeGrids && !myCubeGrid.IsStatic)
					{
						return false;
					}
					if (!m_targetReceiver.TargetStations && myCubeGrid.IsStatic)
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Returns best potential target. Also stored all potential targets into given list. 
		/// </summary>
		/// <param name="rangeSq"></param>
		/// <param name="shootingRangeSq"></param>
		/// <param name="outPotentialTargets"></param>
		/// <returns></returns>
		private MyEntity FindTarget(float rangeSq, float shootingRangeSq, List<MyEntityWithDistSq> outPotentialTargets)
		{
			outPotentialTargets.Clear();
			MyCubeGrid myCubeGrid = m_targetReceiver.GetTargetingParent() as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return null;
			}
			MyEntity nearestTarget = null;
			double minDistanceSq = rangeSq;
			MyEntity[] array = ((m_forcedTarget == null) ? SortTargetRoots() : m_forcedTargetListWrapped);
			MyEntity[] array2 = array;
			foreach (MyEntity myEntity in array2)
			{
				if (myEntity == null || myEntity.Closed)
				{
					break;
				}
				if (!IsTargetRootValid(myEntity, myCubeGrid))
				{
					continue;
				}
				m_targetSelectionWorkData.GridTargeting.AllowScanning = false;
				TestPotentialTarget(myEntity, ref nearestTarget, ref minDistanceSq, rangeSq, outPotentialTargets);
				m_targetSelectionWorkData.GridTargeting.AllowScanning = true;
				if (outPotentialTargets.Count > 0)
				{
					MyEntity myEntity2 = PickOneOfTargets(outPotentialTargets);
					if (myEntity2 != null)
					{
						return myEntity2;
					}
				}
			}
			return null;
		}

		private MyEntity PickOneOfTargets(List<MyEntityWithDistSq> outPotentialTargets)
		{
			Random random = new Random();
			float num = 0f;
			if (!MyFakes.TARGETING_DISTANCE_MODIFIER_ENABLED)
			{
				for (int i = 0; i < outPotentialTargets.Count; i++)
				{
					num += outPotentialTargets[i].Priority;
				}
			}
			else
			{
				double num2 = double.MaxValue;
				double num3 = double.MinValue;
				foreach (MyEntityWithDistSq outPotentialTarget in outPotentialTargets)
				{
					num2 = Math.Min(num2, outPotentialTarget.Distance);
					num3 = Math.Max(num3, outPotentialTarget.Distance);
				}
				if (outPotentialTargets.Count == 1)
				{
					num = outPotentialTargets[0].Priority;
				}
				else
				{
					for (int j = 0; j < outPotentialTargets.Count; j++)
					{
						MyEntityWithDistSq myEntityWithDistSq = outPotentialTargets[j];
						float tARGETING_DISTANCE_MODIFIER_POWER = MyFakes.TARGETING_DISTANCE_MODIFIER_POWER;
						double num4 = Math.Pow(num3 - myEntityWithDistSq.Distance, tARGETING_DISTANCE_MODIFIER_POWER);
						double num5 = Math.Pow(num3 - num2, tARGETING_DISTANCE_MODIFIER_POWER);
						float num6 = (float)(num4 / num5) + 1f / MyFakes.TARGETING_DISTANCE_MODIFIER_POWER_LIMIT;
						MyEntityWithDistSq myEntityWithDistSq2 = default(MyEntityWithDistSq);
						myEntityWithDistSq2.Priority = myEntityWithDistSq.Priority * num6;
						myEntityWithDistSq2.Distance = myEntityWithDistSq.Distance;
						myEntityWithDistSq2.Entity = myEntityWithDistSq.Entity;
						MyEntityWithDistSq value = myEntityWithDistSq2;
						outPotentialTargets[j] = value;
						num += value.Priority;
					}
				}
			}
			if (MyFakes.ENABLE_TARGETING_CHANCE_DRAW)
			{
				MyRenderProxy.DebugClearPersistentMessages();
				float originalSumPriorities = num;
				List<MyEntityWithDistSq> testdata = new List<MyEntityWithDistSq>(outPotentialTargets);
				MySandboxGame.Static.Invoke(delegate
				{
					foreach (MyEntityWithDistSq item in testdata)
					{
						MyRenderProxy.DebugDrawText3D(item.Entity.WorldMatrix.Translation, 100f * item.Priority / originalSumPriorities + "%", Color.Red, 0.6f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
					}
				}, "DebugDrawTurretTargets");
			}
			while (outPotentialTargets.Count > 0)
			{
				int index = Index((float)random.NextDouble() * num);
				MyEntityWithDistSq myEntityWithDistSq3 = outPotentialTargets[index];
				if (TestTarget(myEntityWithDistSq3.Entity))
				{
					return myEntityWithDistSq3.Entity;
				}
				num -= myEntityWithDistSq3.Priority;
				outPotentialTargets.RemoveAt(index);
			}
			return null;
			int Index(float chance)
			{
				float num7 = 0f;
				for (int k = 0; k < outPotentialTargets.Count; k++)
				{
					float priority = outPotentialTargets[k].Priority;
					if (num7 + priority >= chance)
					{
						return k;
					}
					num7 += priority;
				}
				return outPotentialTargets.Count - 1;
			}
		}

		private MyEntity[] SortTargetRoots()
		{
			m_targetSelectionWorkData.GridTargeting.SetRelationFlags(targetEnemies: true, m_targetReceiver.TargetNeutrals, m_targetReceiver.TargetFriends);
			MyEntity[] array = m_targetSelectionWorkData.GridTargeting.TargetRoots.ToArray();
			if (array.Length > 1)
			{
				Vector3D entityPosition = m_targetReceiver.EntityPosition;
				ArrayExtensions.EnsureCapacity(ref m_distanceEntityKeys, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					MyEntity myEntity = array[i];
					float num = ((myEntity == null || myEntity.Closed) ? float.MaxValue : ((float)(myEntity.PositionComp.GetPosition() - entityPosition).LengthSquared()));
					m_distanceEntityKeys[i] = num;
				}
				Array.Sort(m_distanceEntityKeys, array, 0, array.Length, FloatComparer.Instance);
			}
			return array;
		}

		public static float NormalizeRange(float value, float minRange, float maxRange)
		{
			if (value == 0f)
			{
				return 0f;
			}
			return MathHelper.Clamp((value - minRange) / (maxRange - minRange), 0f, 1f);
		}

		public static float DenormalizeRange(float value, float minRange, float maxRange)
		{
			if (value == 0f)
			{
				return 0f;
			}
			return MathHelper.Clamp(minRange + value * (maxRange - minRange), minRange, maxRange);
		}

		public void InjectTerminalControls<T>(T block, bool allowIdleMovement) where T : MyTerminalBlock, IMyTurretTerminalControlReceiver
		{
			if (MyFakes.ENABLE_TURRET_CONTROL)
			{
				MyTerminalControlButton<T> obj = new MyTerminalControlButton<T>("Control", MySpaceTexts.ControlRemote, MySpaceTexts.Blank, delegate(T t)
				{
					t.RequestControl();
				})
				{
					Enabled = (T t) => t.CanControl(),
					Visible = (T t) => t.IsTurretTerminalVisible(),
					SupportsMultipleBlocks = false
				};
				MyTerminalAction<T> myTerminalAction = obj.EnableAction(MyTerminalActionIcons.TOGGLE);
				if (myTerminalAction != null)
				{
					myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType> { MyToolbarType.ButtonPanel };
					myTerminalAction.ValidForGroups = false;
				}
				MyTerminalControlFactory.AddControl(obj);
			}
			MyTerminalControlSlider<T> obj2 = new MyTerminalControlSlider<T>("Range", MySpaceTexts.BlockPropertyTitle_LargeTurretRadius, MySpaceTexts.BlockPropertyTitle_LargeTurretRadius)
			{
				Normalizer = (T x, float f) => NormalizeRange(f, x.MinRange, x.MaxRange),
				Denormalizer = (T x, float f) => DenormalizeRange(f, x.MinRange, x.MaxRange),
				DefaultValue = 800f,
				Getter = (T x) => x.ShootRangeSimple,
				Setter = delegate(T x, float v)
				{
					x.ShootRangeSimple = v;
				},
				Writer = delegate(T x, StringBuilder result)
				{
					result.AppendInt32((int)x.ShootRangeSimple).Append(" m");
				}
			};
			obj2.EnableActions();
			obj2.Visible = (T t) => t.IsTurretTerminalVisible();
			MyTerminalControlFactory.AddControl(obj2);
			MyTerminalControlOnOffSwitch<T> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<T>("EnableIdleMovement", MySpaceTexts.BlockPropertyTitle_LargeTurretEnableTurretIdleMovement);
			myTerminalControlOnOffSwitch.Getter = (T x) => x.EnableIdleRotation;
			myTerminalControlOnOffSwitch.Setter = delegate(T x, bool v)
			{
				x.EnableIdleRotation = v;
			};
			myTerminalControlOnOffSwitch.EnableToggleAction();
			myTerminalControlOnOffSwitch.EnableOnOffActions();
			myTerminalControlOnOffSwitch.Visible = (T t) => t.IsTurretTerminalVisible();
			if ((MyMultiplayer.Static == null || MyMultiplayer.Static.IsLobby) && allowIdleMovement)
			{
				MyTerminalControlFactory.AddControl(myTerminalControlOnOffSwitch);
			}
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>());
			MyTerminalControlOnOffSwitch<T> obj3 = new MyTerminalControlOnOffSwitch<T>("TargetMeteors", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetMeteors)
			{
				Getter = (T x) => x.TargetMeteors,
				Setter = delegate(T x, bool v)
				{
					x.TargetMeteors = v;
				}
			};
			obj3.EnableToggleAction(MyTerminalActionIcons.METEOR_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Asteroids) == 0);
			obj3.EnableOnOffActions(MyTerminalActionIcons.METEOR_ON, MyTerminalActionIcons.METEOR_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Asteroids) == 0);
			obj3.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Asteroids) == 0;
			MyTerminalControlFactory.AddControl(obj3);
			MyTerminalControlOnOffSwitch<T> obj4 = new MyTerminalControlOnOffSwitch<T>("TargetMissiles", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetMissiles)
			{
				Getter = (T x) => x.TargetMissiles,
				Setter = delegate(T x, bool v)
				{
					x.TargetMissiles = v;
				}
			};
			obj4.EnableToggleAction(MyTerminalActionIcons.MISSILE_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Missiles) == 0);
			obj4.EnableOnOffActions(MyTerminalActionIcons.MISSILE_ON, MyTerminalActionIcons.MISSILE_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Missiles) == 0);
			obj4.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Missiles) == 0;
			MyTerminalControlFactory.AddControl(obj4);
			MyTerminalControlOnOffSwitch<T> obj5 = new MyTerminalControlOnOffSwitch<T>("TargetSmallShips", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetSmallGrids)
			{
				Getter = (T x) => x.TargetSmallGrids,
				Setter = delegate(T x, bool v)
				{
					x.TargetSmallGrids = v;
				}
			};
			obj5.EnableToggleAction(MyTerminalActionIcons.SMALLSHIP_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.SmallShips) == 0);
			obj5.EnableOnOffActions(MyTerminalActionIcons.SMALLSHIP_ON, MyTerminalActionIcons.SMALLSHIP_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.SmallShips) == 0);
			obj5.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.SmallShips) == 0;
			MyTerminalControlFactory.AddControl(obj5);
			MyTerminalControlOnOffSwitch<T> obj6 = new MyTerminalControlOnOffSwitch<T>("TargetLargeShips", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetLargeGrids)
			{
				Getter = (T x) => x.TargetLargeGrids,
				Setter = delegate(T x, bool v)
				{
					x.TargetLargeGrids = v;
				}
			};
			obj6.EnableToggleAction(MyTerminalActionIcons.LARGESHIP_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.LargeShips) == 0);
			obj6.EnableOnOffActions(MyTerminalActionIcons.LARGESHIP_ON, MyTerminalActionIcons.LARGESHIP_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.LargeShips) == 0);
			obj6.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.LargeShips) == 0;
			MyTerminalControlFactory.AddControl(obj6);
			MyTerminalControlOnOffSwitch<T> obj7 = new MyTerminalControlOnOffSwitch<T>("TargetCharacters", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetCharacters)
			{
				Getter = (T x) => x.TargetCharacters,
				Setter = delegate(T x, bool v)
				{
					x.TargetCharacters = v;
				}
			};
			obj7.EnableToggleAction(MyTerminalActionIcons.CHARACTER_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Players) == 0);
			obj7.EnableOnOffActions(MyTerminalActionIcons.CHARACTER_ON, MyTerminalActionIcons.CHARACTER_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Players) == 0);
			obj7.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Players) == 0;
			MyTerminalControlFactory.AddControl(obj7);
			MyTerminalControlOnOffSwitch<T> obj8 = new MyTerminalControlOnOffSwitch<T>("TargetStations", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetStations)
			{
				Getter = (T x) => x.TargetStations,
				Setter = delegate(T x, bool v)
				{
					x.TargetStations = v;
				}
			};
			obj8.EnableToggleAction(MyTerminalActionIcons.STATION_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Stations) == 0);
			obj8.EnableOnOffActions(MyTerminalActionIcons.STATION_ON, MyTerminalActionIcons.STATION_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Stations) == 0);
			obj8.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Stations) == 0;
			MyTerminalControlFactory.AddControl(obj8);
			MyTerminalControlOnOffSwitch<T> obj9 = new MyTerminalControlOnOffSwitch<T>("TargetNeutrals", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetNeutrals)
			{
				Getter = (T x) => x.TargetNeutrals,
				Setter = delegate(T x, bool v)
				{
					x.TargetNeutrals = v;
				}
			};
			obj9.EnableToggleAction(MyTerminalActionIcons.NEUTRALS_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Neutrals) == 0);
			obj9.EnableOnOffActions(MyTerminalActionIcons.NEUTRALS_ON, MyTerminalActionIcons.NEUTRALS_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Neutrals) == 0);
			obj9.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Neutrals) == 0;
			MyTerminalControlFactory.AddControl(obj9);
			MyTerminalControlOnOffSwitch<T> obj10 = new MyTerminalControlOnOffSwitch<T>("TargetFriends", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetFriends)
			{
				Getter = (T x) => x.TargetFriends,
				Setter = delegate(T x, bool v)
				{
					x.TargetFriends = v;
				}
			};
			obj10.EnableToggleAction(MyTerminalActionIcons.NEUTRALS_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Friends) == 0);
			obj10.EnableOnOffActions(MyTerminalActionIcons.NEUTRALS_ON, MyTerminalActionIcons.NEUTRALS_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Friends) == 0);
			obj10.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Friends) == 0;
			MyTerminalControlFactory.AddControl(obj10);
			MyTerminalControlOnOffSwitch<T> obj11 = new MyTerminalControlOnOffSwitch<T>("TargetEnemies", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetEnemies)
			{
				Getter = (T x) => x.TargetEnemies,
				Setter = delegate(T x, bool v)
				{
					x.TargetEnemies = v;
				}
			};
			obj11.EnableToggleAction(MyTerminalActionIcons.NEUTRALS_TOGGLE, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Enemies) == 0);
			obj11.EnableOnOffActions(MyTerminalActionIcons.NEUTRALS_ON, MyTerminalActionIcons.NEUTRALS_OFF, (T t) => (t.HiddenTargetingOptions & MyTurretTargetingOptions.Enemies) == 0);
			obj11.Visible = (T t) => t.IsTurretTerminalVisible() && (t.HiddenTargetingOptions & MyTurretTargetingOptions.Enemies) == 0;
			MyTerminalControlFactory.AddControl(obj11);
			MyTerminalControlCheckbox<T> obj12 = new MyTerminalControlCheckbox<T>("EnableTargetLocking", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetLocking, MySpaceTexts.BlockPropertyTooltip_LargeTurretTargetLocking)
			{
				Getter = (T x) => x.TargetLocking,
				Setter = delegate(T x, bool v)
				{
					x.TargetLocking = v;
				}
			};
			obj12.EnableAction();
			MyTerminalControlFactory.AddControl(obj12);
			List<MyTargetingGroupDefinition> groups = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("TargetingGroup_Selector", MySpaceTexts.BlockPropertyText_TargetOptions, MySpaceTexts.Blank)
			{
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> list)
				{
					MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
					{
						Key = 0L,
						Value = MySpaceTexts.BlockPropertyItem_TargetOptions_Default
					};
					list.Add(item);
					long num = 1L;
					foreach (MyTargetingGroupDefinition item2 in groups)
					{
						MyStringId value = MySpaceTexts.BlockPropertyItem_TargetOptions_Unknown;
						if (item2.DisplayNameText != null)
						{
							value = MyStringId.GetOrCompute(item2.DisplayNameText);
						}
						item = new MyTerminalControlComboBoxItem
						{
							Key = num,
							Value = value
						};
						list.Add(item);
						num++;
					}
				},
				Getter = GetCurrentTargetingModeIndex,
				Setter = SetCurrentTargetingModeIndex
			});
			foreach (MyTargetingGroupDefinition group in groups)
			{
				StringBuilder stringBuilder = new StringBuilder(MyTexts.GetString(MySpaceTexts.BlockPropertyText_TargetOptions_Target));
				stringBuilder.Append(group.DisplayNameText);
				MyTerminalControlFactory.AddAction(new MyTerminalAction<T>("TargetingGroup_" + group.Id.SubtypeName, stringBuilder, group.ActionIcon)
				{
					Action = delegate(T x)
					{
						x.SetTargetingMode(group);
					},
					Writer = delegate(T x, StringBuilder to)
					{
						to.Append(group.DisplayNameText);
					}
				});
			}
			MyTerminalControlFactory.AddAction(new MyTerminalAction<T>("TargetingGroup_CycleSubsystems", MyTexts.Get(MySpaceTexts.BlockPropertyItem_TargetOptions_CycleSubsystems), MyTerminalActionIcons.SUBSYSTEM_CYCLE)
			{
				Action = SetNextTargetMode,
				Writer = delegate(T x, StringBuilder to)
				{
					to.Append(GetTargetModeName(block));
				}
			});
			MyTerminalControlButton<T> obj13 = new MyTerminalControlButton<T>("CopyTarget", MySpaceTexts.BlockActionTitle_LargeTurretCopyTarget, MySpaceTexts.BlockActionTooltip_LargeTurretCopyTarget, delegate(T t)
			{
				t.CopyTarget();
			})
			{
				Enabled = (T t) => t.IsWorking
			};
			obj13.EnableAction();
			MyTerminalControlFactory.AddControl(obj13);
			MyTerminalControlButton<T> obj14 = new MyTerminalControlButton<T>("ForgetTarget", MySpaceTexts.BlockActionTitle_LargeTurretUnlockTarget, MySpaceTexts.BlockActionTooltip_LargeTurretUnlockTarget, delegate(T t)
			{
				t.ForgetTarget();
			})
			{
				Enabled = (T t) => t.IsTargetLocked
			};
			obj14.EnableAction();
			MyTerminalControlFactory.AddControl(obj14);
		}

		private static void SetNextTargetMode<T>(T block) where T : IMyTurretTerminalControlReceiver
		{
			List<MyTargetingGroupDefinition> targetingGroupDefinitions = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			long currentTargetingModeIndex = GetCurrentTargetingModeIndex(block);
			if (currentTargetingModeIndex <= 0)
			{
				block.SetTargetingMode(targetingGroupDefinitions[0]);
				return;
			}
			currentTargetingModeIndex--;
			block.SetTargetingMode((currentTargetingModeIndex + 1 >= targetingGroupDefinitions.Count) ? null : targetingGroupDefinitions[(int)currentTargetingModeIndex + 1]);
		}

		private static void SetCurrentTargetingModeIndex<T>(T block, long index) where T : IMyTurretTerminalControlReceiver
		{
			List<MyTargetingGroupDefinition> targetingGroupDefinitions = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			if (index <= 0)
			{
				block.SetTargetingMode(null);
				return;
			}
			index--;
			block.SetTargetingMode(targetingGroupDefinitions[(int)index]);
		}

		public static long GetCurrentTargetingModeIndex<T>(T block) where T : IMyTurretTerminalControlReceiver
		{
			List<MyTargetingGroupDefinition> targetingGroupDefinitions = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			MyStringHash targetingGroup = block.TargetingGroup;
			if (targetingGroup == MyStringHash.NullOrEmpty)
			{
				return 0L;
			}
			string currentString = targetingGroup.String;
			return targetingGroupDefinitions.FindIndex((MyTargetingGroupDefinition x) => x.Id.SubtypeName == currentString) + 1;
		}

		private static string GetTargetModeName<T>(T block) where T : IMyTurretTerminalControlReceiver
		{
			List<MyTargetingGroupDefinition> targetingGroupDefinitions = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			long currentTargetingModeIndex = GetCurrentTargetingModeIndex(block);
			if (currentTargetingModeIndex <= 0)
			{
				return MyTexts.GetString(MySpaceTexts.BlockPropertyItem_TargetOptions_Default);
			}
			return targetingGroupDefinitions[(int)currentTargetingModeIndex - 1].DisplayNameText;
		}

		private bool TestPotentialTarget(MyEntity target, ref MyEntity nearestTarget, ref double minDistanceSq, double maxDistance, List<MyEntityWithDistSq> outPotentialTargets)
		{
			if (target == null)
			{
				return false;
			}
			if (target.MarkedForClose || target.Closed)
			{
				return false;
			}
			if (m_notVisibleTargets.TryGetValue(target, out var value) && value > 0)
			{
				return false;
			}
			MyCubeGrid grid;
			if ((grid = target as MyCubeGrid) != null)
			{
				return TestPotentialTargetCubeGrid(grid, ref nearestTarget, ref minDistanceSq, maxDistance, outPotentialTargets);
			}
			MyCharacter myCharacter;
			if ((myCharacter = target as MyCharacter) != null)
			{
				ulong num = MySession.Static.Players.TryGetSteamId(myCharacter.GetPlayerIdentityId());
				if (num != 0L && MySession.Static.RemoteAdminSettings.TryGetValue(num, out var value2) && value2.HasFlag(AdminSettingsEnum.Untargetable))
				{
					return false;
				}
			}
			bool num2 = IsTarget(target);
			bool flag = num2 && IsValidTarget(target);
			if (!num2 || !flag)
			{
				return false;
			}
			float num3 = 1f;
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = target as MyCubeBlock) != null)
			{
				num3 = myCubeBlock.BlockDefinition.PriorityModifier;
				MyFunctionalBlock myFunctionalBlock;
				if ((myFunctionalBlock = target as MyFunctionalBlock) != null && !myFunctionalBlock.IsWorking)
				{
					num3 *= myFunctionalBlock.BlockDefinition.NotWorkingPriorityMultiplier;
				}
			}
			if (num3 == 0f)
			{
				return false;
			}
			double num4 = Vector3D.DistanceSquared(target.PositionComp.GetPosition(), m_targetReceiver.EntityPosition);
			if (num4 > maxDistance)
			{
				return false;
			}
			MyEntityWithDistSq item;
			if (num4 >= minDistanceSq)
			{
				item = new MyEntityWithDistSq
				{
					Entity = target,
					Priority = num3,
					Distance = Math.Sqrt(num4)
				};
				outPotentialTargets.Add(item);
				return false;
			}
			if (!IsTargetAimedByOtherTurret(target))
			{
				minDistanceSq = num4;
				nearestTarget = target;
				item = new MyEntityWithDistSq
				{
					Entity = target,
					Priority = num3,
					Distance = Math.Sqrt(num4)
				};
				outPotentialTargets.Add(item);
				return true;
			}
			return false;
		}

		private bool TestPotentialTargetCubeGrid(MyCubeGrid grid, ref MyEntity nearestTarget, ref double minDistanceSq, double maxDistance, List<MyEntityWithDistSq> outPotentialTargets)
		{
			if (m_targetReceiver.IsConnected(grid) && grid.BigOwners.Contains(m_targetReceiver.OwnerIdentityId))
			{
				return false;
			}
			if (grid.PositionComp.WorldAABB.DistanceSquared(m_targetReceiver.EntityPosition) > minDistanceSq)
			{
				return false;
			}
			bool flag = false;
			List<MyEntity> list;
			if (m_forcedTarget != null)
			{
				m_targetSelectionWorkData.GridTargeting.SetRelationFlags(m_targetReceiver.TargetEnemies, m_targetReceiver.TargetNeutrals, m_targetReceiver.TargetFriends);
				RecomputeForcedTargetBlocks();
				list = m_forcedTargetBlocks;
			}
			else
			{
				m_targetSelectionWorkData.GridTargeting.SetRelationFlags(targetEnemies: true, m_targetReceiver.TargetNeutrals, m_targetReceiver.TargetFriends);
				list = m_targetSelectionWorkData.GridTargeting.TargetBlocks.GetOrDefault(grid);
			}
			if (list != null)
			{
				using (m_targetSelectionWorkData.GridTargeting.ScanLock.AcquireSharedUsing())
				{
					foreach (MyEntity item in list)
					{
						flag |= TestPotentialTarget(item, ref nearestTarget, ref minDistanceSq, maxDistance, outPotentialTargets);
					}
				}
			}
			if (flag)
			{
				return IsTargetVisible(nearestTarget);
			}
			return false;
		}

		private bool TestTarget(MyEntity target)
		{
			if (target == null || (m_notVisibleTargets.TryGetValue(target, out var value) && value > 0))
			{
				return false;
			}
			Vector3D zero = Vector3D.Zero;
			bool usePrediction = TargetPrediction.UsePrediction;
			TargetPrediction.UsePrediction = false;
			zero = TargetPrediction.GetTargetCoordinates(target);
			TargetPrediction.UsePrediction = usePrediction;
			if (!m_targetReceiver.IsTargetInView(zero) || !IsTargetVisible(target, zero))
			{
				return false;
			}
			return true;
		}

		private bool IsDecoy(MyEntity target)
		{
			MyDecoy myDecoy = target as MyDecoy;
			if (myDecoy != null && myDecoy.IsWorking)
			{
				if (target.Parent.Physics != null)
				{
					return target.Parent.Physics.Enabled;
				}
				return false;
			}
			return false;
		}

		private bool IsTargetAimedByOtherTurret(MyEntity target)
		{
			if (Target == target)
			{
				return false;
			}
			if (m_allAimedTargets.Contains(target) && target is IMyDestroyableObject)
			{
				return ((IMyDestroyableObject)target).Integrity < 2f * m_targetReceiver.MechanicalDamage;
			}
			return false;
		}

		private bool IsTargetIdentityEnemy(long targetIidentityId)
		{
			if (targetIidentityId == 0L)
			{
				return false;
			}
			return m_targetReceiver.GetUserRelationToOwner(targetIidentityId) == MyRelationsBetweenPlayerAndBlock.Enemies;
		}

		private bool IsTargetIdentityFriend(long targetIidentityId)
		{
			if (targetIidentityId == 0L)
			{
				return false;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = m_targetReceiver.GetUserRelationToOwner(targetIidentityId);
			if (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Owner && userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Friends)
			{
				return userRelationToOwner == MyRelationsBetweenPlayerAndBlock.FactionShare;
			}
			return true;
		}

		private bool IsTargetIdentityNeutral(long targetIidentityId)
		{
			if (targetIidentityId == 0L)
			{
				return true;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = m_targetReceiver.GetUserRelationToOwner(targetIidentityId);
			if (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Neutral)
			{
				return userRelationToOwner == MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			return true;
		}

		private void CheckNearTargets(ref MyEntity suggestedTarget, ref bool suggestedTargetIsOnlyPotential)
		{
			if (!m_checkOtherTargets)
			{
				return;
			}
			float shootRangeSquared = m_targetReceiver.ShootRangeSquared;
			float searchRangeSquared = m_targetReceiver.SearchRangeSquared;
			MyEntity myEntity = null;
			MyEntity myEntity2 = suggestedTarget;
			if (myEntity2 != null && (myEntity2.PositionComp.GetPosition() - m_targetReceiver.ShootOrigin).LengthSquared() < (double)Math.Min(shootRangeSquared, searchRangeSquared) && IsTarget(myEntity2) && IsTargetVisible(myEntity2))
			{
				myEntity = myEntity2;
			}
			if (myEntity == null)
			{
				using (MyUtils.ReuseCollection(ref m_allPotentialTargets))
				{
					myEntity = FindTarget(searchRangeSquared, shootRangeSquared, m_allPotentialTargets);
				}
			}
			bool flag = myEntity == null;
			if (MyFakes.FakeTarget != null)
			{
				suggestedTarget = MyFakes.FakeTarget;
				suggestedTargetIsOnlyPotential = !IsTargetVisible(MyFakes.FakeTarget, MyFakes.FakeTarget.WorldMatrix.Translation);
			}
			else
			{
				suggestedTarget = myEntity;
				suggestedTargetIsOnlyPotential = flag;
			}
		}

		private void m_target_OnClose(MyEntity obj)
		{
			SetTarget(null, isPotential: true);
		}

		private void UpdateVisibilityCacheCounters()
		{
			m_lastNotVisibleTargets.Clear();
			foreach (KeyValuePair<MyEntity, int> notVisibleTarget in m_notVisibleTargets)
			{
				int num = notVisibleTarget.Value - m_notVisibleTargetsUpdatesSinceRefresh;
				if (num > 0)
				{
					m_lastNotVisibleTargets[notVisibleTarget.Key] = num;
				}
			}
			ConcurrentDictionary<MyEntity, int> notVisibleTargets = m_notVisibleTargets;
			m_notVisibleTargets = m_lastNotVisibleTargets;
			m_lastNotVisibleTargets = notVisibleTargets;
			m_notVisibleTargetsUpdatesSinceRefresh = 0;
			m_lastVisibleTargets.Clear();
			foreach (KeyValuePair<MyEntity, int> visibleTarget in m_visibleTargets)
			{
				int num2 = visibleTarget.Value - 1;
				if (num2 > 0)
				{
					m_lastVisibleTargets.TryAdd(visibleTarget.Key, num2);
				}
			}
			ConcurrentDictionary<MyEntity, int> visibleTargets = m_visibleTargets;
			m_visibleTargets = m_lastVisibleTargets;
			m_lastVisibleTargets = visibleTargets;
		}
	}
}
