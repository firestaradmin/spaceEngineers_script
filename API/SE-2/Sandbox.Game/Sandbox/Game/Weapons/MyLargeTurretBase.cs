using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Runtime.CompilerServices;
=======
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ParallelTasks;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
<<<<<<< HEAD
=======
using VRage;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.Utils;
using VRage.Input;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TurretBase))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyLargeTurretBase),
		typeof(Sandbox.ModAPI.Ingame.IMyLargeTurretBase)
	})]
<<<<<<< HEAD
	public abstract class MyLargeTurretBase : MyUserControllableGun, IMyInventoryOwner, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyUsableEntity, IMyGunBaseUser, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyShootOrigin, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTargetingCapableBlock, IMyTurretTerminalControlReceiver, IMyTargetingReceiver, IMyTurretControllerControllable, IMyPilotable, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController
=======
	public abstract class MyLargeTurretBase : MyUserControllableGun, IMyInventoryOwner, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyUsableEntity, IMyGunBaseUser, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		[Serializable]
		private struct SyncRotationAndElevation
		{
			protected class Sandbox_Game_Weapons_MyLargeTurretBase_003C_003ESyncRotationAndElevation_003C_003ERotation_003C_003EAccessor : IMemberAccessor<SyncRotationAndElevation, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SyncRotationAndElevation owner, in float value)
				{
					owner.Rotation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SyncRotationAndElevation owner, out float value)
				{
					value = owner.Rotation;
				}
			}

			protected class Sandbox_Game_Weapons_MyLargeTurretBase_003C_003ESyncRotationAndElevation_003C_003EElevation_003C_003EAccessor : IMemberAccessor<SyncRotationAndElevation, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SyncRotationAndElevation owner, in float value)
				{
					owner.Elevation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SyncRotationAndElevation owner, out float value)
				{
					value = owner.Elevation;
				}
			}

			public float Rotation;

			public float Elevation;
		}

<<<<<<< HEAD
=======
		[Serializable]
		private struct CurrentTargetSync
		{
			protected class Sandbox_Game_Weapons_MyLargeTurretBase_003C_003ECurrentTargetSync_003C_003ETargetId_003C_003EAccessor : IMemberAccessor<CurrentTargetSync, long>
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

			protected class Sandbox_Game_Weapons_MyLargeTurretBase_003C_003ECurrentTargetSync_003C_003EIsPotential_003C_003EAccessor : IMemberAccessor<CurrentTargetSync, bool>
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
			protected class Sandbox_Game_Weapons_MyLargeTurretBase_003C_003EMyEntityWithDistSq_003C_003EEntity_003C_003EAccessor : IMemberAccessor<MyEntityWithDistSq, MyEntity>
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

			protected class Sandbox_Game_Weapons_MyLargeTurretBase_003C_003EMyEntityWithDistSq_003C_003EDistSq_003C_003EAccessor : IMemberAccessor<MyEntityWithDistSq, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityWithDistSq owner, in double value)
				{
					owner.DistSq = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityWithDistSq owner, out double value)
				{
					value = owner.DistSq;
				}
			}

			public MyEntity Entity;

			public double DistSq;

			public override string ToString()
			{
				return Entity.ToString();
			}
		}

		private interface IMyPredicionType
		{
			bool ManualTargetPosition { get; }

			Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity entity);
		}

		private class MyTargetPredictionType : IMyPredicionType
		{
			private MyLargeTurretBase m_turret;

			private MyAmmoDefinition m_turretAmmoDefinition;

			private Vector3D m_muzzleWorldPosition;

			private int m_lastQueryTimeMs;

			private Vector3D m_lastResult = Vector3D.Zero;

			private VRage.ModAPI.IMyEntity m_lastTarget;

			public bool ManualTargetPosition => false;

			public MyLargeTurretBase Turret
			{
				get
				{
					return m_turret;
				}
				set
				{
					m_turret = value;
				}
			}

			public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target)
			{
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds == m_lastQueryTimeMs && m_lastTarget == target)
				{
					return m_lastResult;
				}
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds != m_lastQueryTimeMs)
				{
					m_muzzleWorldPosition = Turret.GunBase.GetMuzzleWorldPosition();
				}
				m_lastTarget = target;
				m_lastQueryTimeMs = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				if (target == null)
				{
					m_lastResult = default(Vector3D);
					return m_lastResult;
				}
				if (target.MarkedForClose)
				{
					m_lastResult = target.PositionComp.GetPosition();
					return m_lastResult;
				}
				Vector3D center = target.PositionComp.WorldAABB.Center;
				Vector3D deltaPos = center - m_muzzleWorldPosition;
				if (DEBUG_DRAW_TARGET_PREDICTION)
				{
					MyRenderProxy.DebugDrawLine3D(m_muzzleWorldPosition, center, Color.Lime, Color.Lime, depthRead: true);
					MyRenderProxy.DebugDrawText3D(m_muzzleWorldPosition, $"Distance: {deltaPos.Length():####.00}m", Color.Lime, 0.5f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				}
				float num = 0f;
				float num2 = 0f;
				bool flag = false;
				if (m_turretAmmoDefinition == null && m_turret != null && m_turret.GunBase != null && m_turret.GunBase.CurrentAmmoMagazineDefinition != null)
				{
					m_turretAmmoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(m_turret.GunBase.CurrentAmmoMagazineDefinition.AmmoDefinitionId);
				}
				if (m_turretAmmoDefinition != null)
				{
					num = m_turretAmmoDefinition.DesiredSpeed;
					num2 = m_turretAmmoDefinition.MaxTrajectory;
					MyMissileAmmoDefinition myMissileAmmoDefinition = m_turretAmmoDefinition as MyMissileAmmoDefinition;
					if (myMissileAmmoDefinition != null)
					{
						num2 += myMissileAmmoDefinition.MissileExplosionRadius;
						flag = !myMissileAmmoDefinition.MissileSkipAcceleration;
					}
				}
				float num3 = ((num < 1E-05f) ? 1E-06f : (num2 / num));
				Vector3 vector = Vector3.Zero;
				if (target.Physics != null)
				{
					vector = target.Physics.LinearVelocityUnsafe;
				}
				else
				{
					VRage.ModAPI.IMyEntity topMostParent = target.GetTopMostParent();
					if (topMostParent != null && topMostParent.Physics != null)
					{
						vector = topMostParent.Physics.LinearVelocityUnsafe;
					}
				}
				Vector3 vector2 = ((Turret.Parent != null) ? Turret.Parent.Physics.LinearVelocityUnsafe : Vector3.Zero);
				Vector3 vector3 = vector - vector2;
				double value = Intercept(deltaPos, vector3, num);
				value = MathHelper.Clamp(value, 0.0, num3);
				center += (float)value * vector;
				if (!flag)
				{
					center -= (float)value * vector2;
				}
				else
				{
					center -= (float)(value / (double)num3) * vector2;
				}
				m_lastResult = center;
				if (DEBUG_DRAW_TARGET_PREDICTION)
				{
					MyRenderProxy.DebugDrawLine3D(m_muzzleWorldPosition, m_lastResult, Color.Orange, Color.Orange, depthRead: true);
					MyRenderProxy.DebugDrawText3D(m_muzzleWorldPosition, $"Time of Impact: {value:00.00}s", Color.Orange, 0.5f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
					MyRenderProxy.DebugDrawText3D(text: $"Distance of Impact: {(m_lastResult - m_muzzleWorldPosition).Length():####.00}m", worldCoord: m_muzzleWorldPosition, color: Color.Orange, scale: 0.5f, depthRead: true);
				}
				return m_lastResult;
			}

			private double Intercept(Vector3D deltaPos, Vector3D deltaVel, float projectileVel)
			{
				double num = Vector3D.Dot(deltaVel, deltaVel) - (double)(projectileVel * projectileVel);
				double num2 = 2.0 * Vector3D.Dot(deltaVel, deltaPos);
				double num3 = Vector3D.Dot(deltaPos, deltaPos);
				double num4 = num2 * num2 - 4.0 * num * num3;
				if (!(num4 > 0.0))
				{
					return -1.0;
				}
				return 2.0 * num3 / (Math.Sqrt(num4) - num2);
			}

			public MyTargetPredictionType(MyLargeTurretBase turret)
			{
				Turret = turret;
			}
		}

		private class MyTargetNoPredictionType : IMyPredicionType
		{
			public bool ManualTargetPosition => false;

			public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target)
			{
				return target.PositionComp.WorldAABB.Center;
			}
		}

		private class MyPositionNoPredictionType : IMyPredicionType
		{
			public bool ManualTargetPosition => true;

			public Vector3D TrackedPosition { get; set; }

			public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target)
			{
				return TrackedPosition;
			}
		}

		private class MyPositionPredictionType : IMyPredicionType
		{
			public bool ManualTargetPosition => true;

			public MyLargeTurretBase Turret { get; set; }

			public Vector3D TrackedPosition { get; set; }

			public Vector3D TrackedVelocity { get; set; }

			public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target)
			{
				Vector3D vector3D = ((!ManualTargetPosition && target != null) ? target.PositionComp.WorldMatrix.Translation : TrackedPosition);
				Vector3D vector3D2 = Vector3D.Normalize(vector3D - Turret.GunBase.GetMuzzleWorldPosition());
				float num = 0f;
				if (Turret.GunBase.CurrentAmmoMagazineDefinition != null)
				{
					MyAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(Turret.GunBase.CurrentAmmoMagazineDefinition.AmmoDefinitionId);
					num = ammoDefinition.DesiredSpeed;
					if (ammoDefinition.AmmoType == MyAmmoType.Missile)
					{
						MyMissileAmmoDefinition myMissileAmmoDefinition = (MyMissileAmmoDefinition)ammoDefinition;
						if (myMissileAmmoDefinition.MissileInitialSpeed == 100f && myMissileAmmoDefinition.MissileAcceleration == 600f && ammoDefinition.DesiredSpeed == 700f)
						{
							num = 800f - 238431f / (397.42f + (float)(vector3D - Turret.GunBase.GetMuzzleWorldPosition()).Length());
						}
					}
				}
				Vector3 vector = (Vector3)TrackedVelocity - Turret.Parent.Physics.LinearVelocityUnsafe;
				Vector3 vector2 = Vector3.Dot(vector, vector3D2) * vector3D2;
				Vector3 vector3 = vector - vector2;
				float num2 = vector3.Length();
				if (num2 > num)
				{
					return vector3D;
				}
				float num3 = (float)Math.Sqrt(num * num - num2 * num2);
				Vector3 vector4 = vector3D2 * num3;
				float num4 = vector4.Length() - vector2.Length();
				double num5 = ((num4 != 0f) ? ((Turret.PositionComp.GetPosition() - vector3D).Length() / (double)num4) : 0.0);
				Vector3 vector5 = vector4 + vector3;
				return (num5 > 0.0099999997764825821) ? (Turret.GunBase.GetMuzzleWorldPosition() + (Vector3D)vector5 * num5) : vector3D;
			}

			public MyPositionPredictionType(MyLargeTurretBase turret)
			{
				Turret = turret;
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public enum MyLargeShipGunStatus
		{
			MyWeaponStatus_Deactivated,
			MyWeaponStatus_Searching,
			MyWeaponStatus_Shooting,
			MyWeaponStatus_ShootDelaying
		}

		protected sealed class sync_ControlledEntity_Used_003C_003E : ICallSite<MyLargeTurretBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.sync_ControlledEntity_Used();
			}
		}

		protected sealed class CopyTargetServer_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyLargeTurretBase, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in long characterEntityId, in long cockpitEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CopyTargetServer(characterEntityId, cockpitEntityId);
			}
		}

		protected sealed class ForgetTargetServer_003C_003E : ICallSite<MyLargeTurretBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ForgetTargetServer();
			}
		}

		protected sealed class OnBeginShoot_003C_003EVRage_Game_ModAPI_MyShootActionEnum : ICallSite<MyLargeTurretBase, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in MyShootActionEnum action, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnBeginShoot(action);
			}
		}

		protected sealed class OnEndShoot_003C_003EVRage_Game_ModAPI_MyShootActionEnum : ICallSite<MyLargeTurretBase, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in MyShootActionEnum action, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnEndShoot(action);
			}
		}

		protected sealed class ResetTargetParams_003C_003E : ICallSite<MyLargeTurretBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ResetTargetParams();
			}
		}

		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MyLargeTurretBase, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MyLargeTurretBase, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		protected sealed class SetTargetRequest_003C_003ESystem_Int64_0023System_Boolean : ICallSite<MyLargeTurretBase, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in long entityId, in bool usePrediction, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetTargetRequest(entityId, usePrediction);
			}
		}

		protected sealed class SetTargetPosition_003C_003EVRageMath_Vector3D_0023VRageMath_Vector3_0023System_Boolean : ICallSite<MyLargeTurretBase, Vector3D, Vector3, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeTurretBase @this, in Vector3D targetPos, in Vector3 velocity, in bool usePrediction, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetTargetPosition(targetPos, velocity, usePrediction);
			}
		}

		protected class m_lateStartRandom_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lateStartRandom;
				ISyncType result = (lateStartRandom = new Sync<int, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_lateStartRandom = (Sync<int, SyncDirection.FromServer>)lateStartRandom;
				return result;
			}
		}

		protected class m_shootingRange_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType shootingRange;
				ISyncType result = (shootingRange = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_shootingRange = (Sync<float, SyncDirection.BothWays>)shootingRange;
				return result;
			}
		}

		protected class m_enableIdleRotation_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType enableIdleRotation;
				ISyncType result = (enableIdleRotation = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_enableIdleRotation = (Sync<bool, SyncDirection.BothWays>)enableIdleRotation;
				return result;
			}
		}

		protected class m_maxTargetLockingRange_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType maxTargetLockingRange;
				ISyncType result = (maxTargetLockingRange = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_maxTargetLockingRange = (Sync<float, SyncDirection.BothWays>)maxTargetLockingRange;
=======
				ISyncType rotationAndElevationSync;
				ISyncType result = (rotationAndElevationSync = new Sync<SyncRotationAndElevation, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_rotationAndElevationSync = (Sync<SyncRotationAndElevation, SyncDirection.BothWays>)rotationAndElevationSync;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class m_rotationAndElevationSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType rotationAndElevationSync;
				ISyncType result = (rotationAndElevationSync = new Sync<SyncRotationAndElevation, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_rotationAndElevationSync = (Sync<SyncRotationAndElevation, SyncDirection.BothWays>)rotationAndElevationSync;
=======
				ISyncType targetSync;
				ISyncType result = (targetSync = new Sync<CurrentTargetSync, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_targetSync = (Sync<CurrentTargetSync, SyncDirection.FromServer>)targetSync;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class m_targetFlags_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetFlags;
				ISyncType result = (targetFlags = new Sync<MyTurretTargetFlags, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_targetFlags = (Sync<MyTurretTargetFlags, SyncDirection.BothWays>)targetFlags;
				return result;
			}
		}

<<<<<<< HEAD
		protected class m_targetingGroup_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetingGroup;
				ISyncType result = (targetingGroup = new Sync<MyStringHash, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_targetingGroup = (Sync<MyStringHash, SyncDirection.BothWays>)targetingGroup;
=======
		protected class m_cachedAmmunitionAmount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType cachedAmmunitionAmount;
				ISyncType result = (cachedAmmunitionAmount = new Sync<int, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_cachedAmmunitionAmount = (Sync<int, SyncDirection.FromServer>)cachedAmmunitionAmount;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

<<<<<<< HEAD
		protected class m_fireAtLockedTarget_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fireAtLockedTarget;
				ISyncType result = (fireAtLockedTarget = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_fireAtLockedTarget = (Sync<bool, SyncDirection.BothWays>)fireAtLockedTarget;
				return result;
			}
		}

		protected class m_targetLocking_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetLocking;
				ISyncType result = (targetLocking = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_targetLocking = (Sync<bool, SyncDirection.BothWays>)targetLocking;
				return result;
			}
		}
=======
		private static uint TIMER_NORMAL_IN_FRAMES = 10u;

		private static uint TIMER_TIER1_IN_FRAMES = 0u;

		public static float GAMEPAD_ZOOM_SPEED = 0.02f;

		public static float EXACT_VISIBILITY_TEST_TRESHOLD_ANGLE = 0.85f;

		public static bool DEBUG_DRAW_TARGET_PREDICTION = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		protected class m_lockedTarget_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lockedTarget;
				ISyncType result = (lockedTarget = new Sync<long, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_lockedTarget = (Sync<long, SyncDirection.FromServer>)lockedTarget;
				return result;
			}
		}

		protected class m_isReloadStarted_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isReloadStarted;
				ISyncType result = (isReloadStarted = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_isReloadStarted = (Sync<bool, SyncDirection.FromServer>)isReloadStarted;
				return result;
			}
		}

		protected class m_cachedAmmunitionAmount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType cachedAmmunitionAmount;
				ISyncType result = (cachedAmmunitionAmount = new Sync<int, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_cachedAmmunitionAmount = (Sync<int, SyncDirection.FromServer>)cachedAmmunitionAmount;
				return result;
			}
		}

		protected class m_targetSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetSync;
				ISyncType result = (targetSync = new Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer>(P_1, P_2));
				((MyLargeTurretBase)P_0).m_targetSync = (Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer>)targetSync;
				return result;
			}
		}

		private static uint TIMER_NORMAL_IN_FRAMES = 10u;

		private static uint TIMER_TIER1_IN_FRAMES = 0u;

		public static float GAMEPAD_ZOOM_SPEED = 0.02f;

		private static readonly MyStringId ID_WEAPON_LASER = MyStringId.GetOrCompute("WeaponLaser");

		private bool m_workingFlagCombination = true;

		private bool m_isInRandomRotationDistance;

		private MyTurretController m_turretController;

		/// <summary>
		/// Replication server.
		/// </summary>
		private MyReplicationServer m_replicableServer;

		private IMyReplicable m_blocksReplicable;

		private MyParallelUpdateFlag m_parallelFlag;
<<<<<<< HEAD
=======

		private float m_targetingTimeInFrames = 100f;

		private MyTargetSelectionWorkData m_targetSelectionWorkData = new MyTargetSelectionWorkData();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyEntity3DSoundEmitter m_soundEmitterForRotation;

		private MyToolbar m_toolbar;

		private bool m_playAimingSound;

		public const float MAX_DISTANCE_FOR_RANDOM_ROTATING_LARGESHIP_GUNS = 600f;

		public const float DEFAULT_MIN_RANGE = 4f;

		public const float DEFAULT_MAX_RANGE = 800f;

		private static readonly float ROTATION_MULTIPLIER_NORMAL = 1f;

		private static readonly float ROTATION_MULTIPLIER_ZOOMED = 0.3f;

		private const float MIN_FOV = 1E-05f;

		private const float MAX_FOV = (float)Math.PI * 179f / 180f;

<<<<<<< HEAD
=======
		private bool m_hidetoolbar;

		private MyToolbar m_toolbar;

		private bool m_playAimingSound;

		public const float MAX_DISTANCE_FOR_RANDOM_ROTATING_LARGESHIP_GUNS = 600f;

		private const float DEFAULT_MIN_RANGE = 4f;

		private const float DEFAULT_MAX_RANGE = 800f;

		private static readonly float ROTATION_MULTIPLIER_NORMAL = 1f;

		private static readonly float ROTATION_MULTIPLIER_ZOOMED = 0.3f;

		private const float MIN_FOV = 1E-05f;

		private const float MAX_FOV = (float)Math.PI * 179f / 180f;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static float m_minFov;

		private static float m_maxFov;

		private readonly Sync<int, SyncDirection.FromServer> m_lateStartRandom;

		protected MyLargeBarrelBase m_barrel;

		protected MyEntity m_base1;

		protected MyEntity m_base2;

		private MyLargeShipGunStatus m_status;

		private float m_rotation;

		private float m_elevation;

<<<<<<< HEAD
		public bool ReloadTimeHandledByBarrel;
=======
		private bool m_isMoving = true;

		private bool m_transformDirty = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private bool m_isMoving = true;

		private bool m_transformDirty = true;

		protected float m_rotationSpeed;

		protected float m_elevationSpeed;

		protected int m_randomStandbyChange_ms;

		protected int m_randomStandbyChangeConst_ms;

		private float m_randomStandbyRotation;

		private float m_randomStandbyElevation;

		private bool m_randomIsMoving;

		private double m_laserLength = 100.0;

		private int m_shootDelayIntervalConst_ms;

		private int m_shootIntervalConst_ms;

		private int m_shootStatusChanged_ms;

		private int m_shootDelayInterval_ms;

		private int m_shootInterval_ms;

		private bool m_isPlayerShooting;

		private MyHudNotification m_outOfAmmoNotification;

		private MyHudNotification m_outOfRangeNotification;

		private MyHudNotification m_noTargetNotification;

		private MyHudNotification m_lockingInProgressNotification;

		private float m_fov;

		private float m_targetFov;

		private bool m_gunIdleElevationAzimuthUnknown = true;

		private float m_gunIdleElevation;

		private float m_gunIdleAzimuth;

		private readonly Sync<float, SyncDirection.BothWays> m_shootingRange;

		private float m_searchingRange = 800f;

		private readonly Sync<bool, SyncDirection.BothWays> m_enableIdleRotation;

		private bool m_previousIdleRotationState = true;

		private Sync<float, SyncDirection.BothWays> m_maxTargetLockingRange;

		private MyDefinitionId m_defId;

		protected MySoundPair m_shootingCueEnum = new MySoundPair();

		protected MySoundPair m_rotatingCueEnum = new MySoundPair();

		protected Vector3D m_hitPosition;

		protected MyGunBase m_gunBase;

		private long m_targetToSet;

		private float m_minElevationRadians;

		private float m_maxElevationRadians = (float)Math.PI * 2f;

		private float m_minSinElevationRadians = -1f;

		private float m_maxSinElevationRadians = 1f;

		private float m_minAzimuthRadians;

		private float m_maxAzimuthRadians = (float)Math.PI * 2f;
<<<<<<< HEAD
=======

		private float m_minRangeMeter = 4f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private float m_maxRangeMeter = 800f;

		private MyEntity[] m_shootIgnoreEntities;

		private readonly Sync<SyncRotationAndElevation, SyncDirection.BothWays> m_rotationAndElevationSync;

		private readonly Sync<MyTurretTargetFlags, SyncDirection.BothWays> m_targetFlags;

		private readonly Sync<MyStringHash, SyncDirection.BothWays> m_targetingGroup;

		private readonly Sync<bool, SyncDirection.BothWays> m_fireAtLockedTarget;

		private readonly Sync<bool, SyncDirection.BothWays> m_targetLocking;

		private bool m_isAimed;

		private HashSet<VRage.ModAPI.IMyEntity> m_children = new HashSet<VRage.ModAPI.IMyEntity>();
<<<<<<< HEAD
=======

		private Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;

		private MatrixD m_lastTestedGridWM;

		private bool m_canStopShooting;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyLargeTurretTargetingSystem m_targetingSystem;

		private Sync<long, SyncDirection.FromServer> m_lockedTarget;

		private float m_forcedTargetRange = 5000f;

		private Sync<bool, SyncDirection.FromServer> m_isReloadStarted;

		private Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;

		private readonly Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> m_targetSync;

		private MatrixD m_lastTestedGridWM;

		private bool m_canStopShooting;

		public Vector3D ShootOrigin => GunBase.GetMuzzleWorldMatrix().Translation;

		public int LateStartRandom => m_lateStartRandom.Value;

		private float Rotation
		{
			get
			{
				return m_rotation;
			}
			set
			{
				if (m_rotation != value)
				{
					m_rotation = value;
					m_transformDirty = true;
				}
			}
		}

		private float Elevation
		{
			get
			{
				return m_elevation;
			}
			set
			{
				if (m_elevation != value)
				{
					m_elevation = value;
					m_transformDirty = true;
				}
			}
		}

<<<<<<< HEAD
		public bool CanHavePreviousControlledEntity => true;

		public MyEntity WeaponOwner { get; set; }

		public MatrixD InitializationMatrix { get; private set; }

=======
		public MyEntity WeaponOwner { get; set; }

		public MatrixD InitializationMatrix { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MatrixD InitializationBarrelMatrix { get; set; }

		public MyEntity Target => m_targetingSystem.Target;

		public bool IsAimed
		{
			get
			{
				return m_isAimed;
			}
			protected set
			{
				if (m_isAimed != value)
				{
					m_isAimed = value;
				}
			}
		}

		public MyLargeTurretTargetingSystem TargetingSystem => m_targetingSystem;

		public MyModelDummy CameraDummy { get; private set; }

		public new MyLargeTurretBaseDefinition BlockDefinition => base.BlockDefinition as MyLargeTurretBaseDefinition;

		private bool AiEnabled
		{
			get
			{
				if (BlockDefinition != null)
				{
					return BlockDefinition.AiEnabled;
				}
				return true;
			}
		}

		public MyCharacter Pilot => m_turretController.Pilot;

		public Sandbox.Game.Entities.IMyControllableEntity PreviousControlledEntity => m_turretController.PreviousControlledEntity;

		public bool IsControlledByLocalPlayer => m_turretController.IsControlledByLocalPlayer;

		public bool IsControlled
		{
			get
			{
				return m_turretController.IsControlled;
			}
			set
			{
				m_turretController.IsControlled = value;
			}
		}

<<<<<<< HEAD
		public MyStringHash TargetingGroup => m_targetingGroup.Value;

		protected float ForwardCameraOffset => BlockDefinition.ForwardCameraOffset;

		protected float UpCameraOffset => BlockDefinition.UpCameraOffset;
=======
		protected abstract float ForwardCameraOffset { get; }

		protected abstract float UpCameraOffset { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyLargeBarrelBase Barrel => m_barrel;

		public MyGunBase GunBase => m_gunBase;

		public bool EnableIdleRotation
		{
			get
			{
				return m_enableIdleRotation;
			}
			set
			{
				m_enableIdleRotation.Value = value;
			}
		}

		public MyTurretTargetFlags TargetFlags
		{
			get
			{
				return m_targetFlags;
			}
			set
			{
				m_targetFlags.Value = value;
			}
		}

		public bool IsSkinnable => false;

<<<<<<< HEAD
		public bool IsTargetLockingCapable => true;

		public override bool IsTieredUpdateSupported => true;

		public bool IsTargetLocked => m_lockedTarget.Value != 0;

		public Vector3D ShootDirection => GunBase.WorldMatrix.Forward;

		public MyGridTargeting GridTargeting => base.CubeGrid.Components.Get<MyGridTargeting>();

		public bool IsReloadStarted
		{
			get
			{
				return m_isReloadStarted.Value;
			}
			set
			{
				m_isReloadStarted.Value = value;
			}
		}

=======
		public override bool IsTieredUpdateSupported => true;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MatrixD InitializationMatrixWorld => InitializationMatrix * base.Parent.WorldMatrix;

		private bool NeedsPerFrameUpdate
		{
			get
			{
				if (!MyFakes.OPTIMIZE_GRID_UPDATES)
				{
					return true;
				}
				float randomStandbyRotation = m_randomStandbyRotation;
				float randomStandbyElevation = m_randomStandbyElevation;
				float num = randomStandbyRotation - Rotation;
				float num2 = randomStandbyElevation - Elevation;
<<<<<<< HEAD
				if ((!m_turretController.IsControlledByLocalPlayer || MySession.Static.CameraController != this) && !m_transformDirty && !m_isMoving && (m_barrel == null || !m_barrel.NeedsPerFrameUpdate) && !m_isPlayerShooting && !(num * num > 9.99999944E-11f) && !(num2 * num2 > 9.99999944E-11f))
=======
				if ((!IsControlledByLocalPlayer || MySession.Static.CameraController != this) && !m_transformDirty && !m_isMoving && (m_barrel == null || !m_barrel.NeedsPerFrameUpdate) && !m_isPlayerShooting && !(num * num > 9.99999944E-11f) && !(num2 * num2 > 9.99999944E-11f))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return m_forceShoot;
				}
				return true;
			}
		}

		public float SearchRange
		{
			get
			{
				if (m_targetingSystem.IsTargetForced)
				{
					return m_forcedTargetRange;
				}
				return m_searchingRange;
			}
		}

		public float ShootRange
		{
			get
			{
				if (m_targetingSystem.IsTargetForced)
				{
					return m_forcedTargetRange;
				}
				return m_shootingRange;
			}
		}

		public Vector3D EntityPosition => base.PositionComp.GetPosition();

		public float MechanicalDamage => GunBase.MechanicalDamage;

<<<<<<< HEAD
		public float SearchRangeSquared
		{
			get
			{
				float searchRange = SearchRange;
				return searchRange * searchRange;
			}
		}

		public float ShootRangeSquared
		{
			get
			{
				float shootRange = ShootRange;
				return shootRange * shootRange;
			}
		}
=======
		public float ShakeAmount { get; protected set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount { get; protected set; }

		public bool IsShooting => m_isShooting.Value;

		int IMyGunObject<MyGunBase>.ShootDirectionUpdateTime => 0;

<<<<<<< HEAD
		/// <inheritdoc />
		bool IMyGunObject<MyGunBase>.NeedsShootDirectionWhileAiming => false;

		/// <inheritdoc />
=======
		bool IMyGunObject<MyGunBase>.NeedsShootDirectionWhileAiming => false;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float IMyGunObject<MyGunBase>.MaximumShotLength => 0f;

		public bool PerformFailReaction
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public new MyDefinitionId DefinitionId => m_defId;

		public float ShootRangeSimple
		{
			get
			{
				return m_shootingRange;
			}
			set
			{
				m_shootingRange.Value = value;
			}
		}

		public float SearchRangeSimple => m_searchingRange;

		public bool TargetMeteors
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Asteroids) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Asteroids;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Asteroids;
				}
			}
		}

		public bool TargetMissiles
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Missiles) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Missiles;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Missiles;
				}
			}
		}

		public bool TargetSmallGrids
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.SmallShips) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.SmallShips;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.SmallShips;
				}
			}
		}

		public bool TargetLargeGrids
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.LargeShips) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.LargeShips;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.LargeShips;
				}
			}
		}

		public bool TargetCharacters
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Players) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Players;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Players;
				}
			}
		}

		public bool TargetStations
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Stations) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Stations;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Stations;
				}
			}
		}

		public bool TargetNeutrals
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.NotNeutrals) == 0;
			}
			set
			{
				if (value)
				{
					TargetFlags &= ~MyTurretTargetFlags.NotNeutrals;
				}
				else
				{
					TargetFlags |= MyTurretTargetFlags.NotNeutrals;
				}
			}
		}

		public bool TargetEnemies
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.NotEnemies) == 0;
			}
			set
			{
				if (value)
				{
					TargetFlags &= ~MyTurretTargetFlags.NotEnemies;
				}
				else
				{
					TargetFlags |= MyTurretTargetFlags.NotEnemies;
				}
			}
		}

		public bool TargetFriends
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.TargetFriends) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.TargetFriends;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.TargetFriends;
				}
			}
		}

		public bool TargetLocking
		{
			get
			{
				return m_targetLocking;
			}
			set
			{
				m_targetLocking.Value = value;
			}
		}

		public bool FireAtLockedTarget
		{
			get
			{
				return m_fireAtLockedTarget.Value;
			}
			set
			{
				m_fireAtLockedTarget.Value = value;
			}
		}

		public bool PrimaryLookaround => false;

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera { get; set; }

		bool IMyCameraController.AllowCubeBuilding => false;

		bool IMyCameraController.EnableFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public MyControllerInfo ControllerInfo => m_turretController.ControllerInfo;

		public MyEntity Entity => this;

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => this;

		public bool ForceFirstPersonCamera { get; set; }

		public float HeadLocalXAngle { get; set; }

		public float HeadLocalYAngle { get; set; }

		public bool EnabledThrusts => false;

		public bool EnabledDamping => false;

		public bool EnabledLights => false;

		public bool EnabledLeadingGears => false;

		public bool EnabledReactors => false;

		public bool EnabledBroadcasting => false;

		public bool EnabledHelmet => false;

		public MyToolbarType ToolbarType => MyToolbarType.LargeCockpit;

		public MyToolbar Toolbar => m_toolbar;

		MyEntity[] IMyGunBaseUser.IgnoreEntities => m_shootIgnoreEntities;

		MyEntity IMyGunBaseUser.Weapon
		{
			get
			{
				if (m_barrel == null)
				{
					return null;
				}
				return m_barrel.Entity;
			}
		}

		MyEntity IMyGunBaseUser.Owner => base.Parent;

		public virtual IMyMissileGunObject Launcher => this;

		MyInventory IMyGunBaseUser.AmmoInventory => this.GetInventory();

		MyDefinitionId IMyGunBaseUser.PhysicalItemId => default(MyDefinitionId);

		MyInventory IMyGunBaseUser.WeaponInventory => null;

		long IMyGunBaseUser.OwnerId => base.OwnerId;

		string IMyGunBaseUser.ConstraintDisplayName => base.BlockDefinition.DisplayNameText;

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		public MyEntity RelativeDampeningEntity { get; set; }

<<<<<<< HEAD
		/// <inheritdoc />
		public MyParallelUpdateFlags UpdateFlags => m_parallelFlag.GetFlags(this);

		public Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> TargetSync => m_targetSync;

		public long OwnerIdentityId => base.OwnerId;

		public float MinRange => 4f;

		public float MaxRange => m_maxRangeMeter;

		public MyDefinitionBase GetAmmoDefinition => GunBase.CurrentAmmoDefinition;

		public MyTurretTargetingOptions HiddenTargetingOptions => BlockDefinition.HiddenTargetingOptions;

		public MyTurretController TurretController => m_turretController;

		public bool CanHavePreviousCameraEntity => false;

		public VRage.ModAPI.IMyEntity GetPreviousCameraEntity => null;

		public float MaxShootRange => m_gunBase.CurrentAmmoDefinition.MaxTrajectory;
=======
		public MyParallelUpdateFlags UpdateFlags => m_parallelFlag.GetFlags(this);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		VRage.ModAPI.IMyEntity Sandbox.ModAPI.IMyLargeTurretBase.Target => Target;

		bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.IsAimed => IsAimed;

		bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.HasTarget => Target != null;

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => ControllerInfo;

		public Vector3 LastMotionIndicator => Vector3.Zero;

		public Vector3 LastRotationIndicator { get; set; }

		float Sandbox.ModAPI.Ingame.IMyLargeTurretBase.Elevation
		{
			get
			{
				return Elevation;
			}
			set
			{
				SetManualElevation(value);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLargeTurretBase.Azimuth
		{
			get
			{
				return Rotation;
			}
			set
			{
				SetManualAzimuth(value);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.EnableIdleRotation
		{
			get
			{
				return EnableIdleRotation;
			}
			set
			{
				EnableIdleRotation = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.AIEnabled => AiEnabled;

		bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.IsUnderControl => ControllerInfo.Controller != null;

		bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.CanControl => CanControl();

		float Sandbox.ModAPI.Ingame.IMyLargeTurretBase.Range
		{
			get
			{
				return ShootRangeSimple;
			}
			set
			{
				if (!float.IsNaN(value))
				{
					ShootRangeSimple = MathHelper.Clamp(value, 0f, m_maxRangeMeter);
				}
			}
		}

		public MyStringId ControlContext => MySpaceBindingCreator.CX_SPACESHIP;

		public MyStringId AuxiliaryContext => MySpaceBindingCreator.AX_ACTIONS;

		public Dictionary<int, string> GetDummyNames()
		{
			return BlockDefinition.DummyNames;
		}

		public MyLargeShipGunStatus GetStatus()
		{
			return m_status;
		}

		public MyLargeTurretBase()
		{
			m_shootIgnoreEntities = new MyEntity[1] { this };
			CreateTerminalControls();
			m_status = MyLargeShipGunStatus.MyWeaponStatus_Deactivated;
			m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_randomStandbyRotation = 0f;
			m_randomStandbyElevation = 0f;
			Rotation = 0f;
			Elevation = 0f;
			m_rotationSpeed = 0.005f;
			m_elevationSpeed = 0.005f;
			m_shootDelayIntervalConst_ms = 200;
			m_shootIntervalConst_ms = 1200;
			m_shootStatusChanged_ms = 0;
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_soundEmitterForRotation = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_turretController = new MyTurretController(this);
			m_turretController.OnControlAcquired += AcquireControl;
			m_turretController.OnControlReleased += ReleaseControl;
			m_turretController.OnRotationUpdate += RotateModels;
			m_turretController.OnMoveAndRotationUpdate += MoveAndRotate;
			m_turretController.OnCameraOverlayUpdate += SetCameraOverlay;
			ControllerInfo.ControlReleased += OnControlReleased;
			m_gunBase = new MyGunBase();
			if (Sync.IsServer)
			{
				m_gunBase.OnAmmoAmountChanged += OnAmmoAmountChangedOnServer;
			}
			else
			{
				m_cachedAmmunitionAmount.ValueChanged += OnAmmoAmountChangedFromServer;
			}
			m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
<<<<<<< HEAD
			m_outOfRangeNotification = new MyHudNotification(MyCommonTexts.TargetOutOfRange, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_noTargetNotification = new MyHudNotification(MyCommonTexts.NoTargetLocked, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_lockingInProgressNotification = new MyHudNotification(MyCommonTexts.LockingInProgress, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (!Sync.IsDedicated)
			{
				m_parallelFlag.Enable(this);
			}
			base.SyncType.Append(m_gunBase);
			m_shootingRange.ValueChanged += delegate
			{
				ShootingRangeChanged();
			};
			m_rotationAndElevationSync.ValueChanged += delegate
			{
				RotationAndElevationSync();
			};
			InitializeTargetingSystem();
			m_targetSync.AlwaysReject();
			m_toolbar = new MyToolbar(ToolbarType);
			m_lockedTarget.ValueChanged += OnLockedTargetChanged;
			m_isReloadStarted.ValueChanged += OnStartReloadChanged;
		}

<<<<<<< HEAD
		private void OnStartReloadChanged(SyncBase obj)
=======
		private void OnAmmoAmountChangedFromServer(SyncBase obj)
		{
			GunBase.CurrentAmmo = m_cachedAmmunitionAmount.Value;
		}

		private void OnAmmoAmountChangedOnServer()
		{
			if (Sync.IsServer)
			{
				m_cachedAmmunitionAmount.Value = GunBase.CurrentAmmo;
			}
		}

		private void TargetChanged()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (m_isReloadStarted.Value && !Sync.IsServer && IsControlledByLocalPlayer)
			{
				base.ReloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + GunBase.ReloadTime;
			}
		}

		private void InitializeTargetingSystem()
		{
			if (m_targetingSystem == null)
			{
				m_targetSync.ValueChanged += delegate
				{
					m_targetingSystem.TargetChanged();
				};
				m_targetingSystem = new MyLargeTurretTargetingSystem(this);
			}
		}

		private void OnLockedTargetChanged(SyncBase obj)
		{
			m_targetingSystem.ForgetGridTarget();
			if (m_lockedTarget.Value != 0L)
			{
				m_forcedTargetRange = GunBase.CurrentAmmoDefinition.MaxTrajectory;
				MyEntity entityById = MyEntities.GetEntityById(m_lockedTarget.Value);
				if (!Sync.IsDedicated && entityById != null && (entityById.PositionComp.GetPosition() - base.PositionComp.GetPosition()).LengthSquared() > (double)(m_forcedTargetRange * m_forcedTargetRange))
				{
					MyHud.Notifications.Add(m_outOfRangeNotification);
				}
			}
		}

		private void OnAmmoAmountChangedFromServer(SyncBase obj)
		{
			GunBase.CurrentAmmo = m_cachedAmmunitionAmount.Value;
		}

		private void OnAmmoAmountChangedOnServer()
		{
			if (Sync.IsServer)
			{
				m_cachedAmmunitionAmount.Value = GunBase.CurrentAmmo;
			}
		}

		private void RotationAndElevationSync()
		{
			if (!m_turretController.IsControlledByLocalPlayer)
			{
				UpdateRotationAndElevation(m_rotationAndElevationSync.Value.Rotation, m_rotationAndElevationSync.Value.Elevation);
			}
		}

		private void ShootingRangeChanged()
		{
			AdjustSearchingRange();
			if (Sync.IsServer && base.IsWorking && AiEnabled && (MySession.Static.CreativeMode || HasEnoughAmmo()))
			{
				m_targetingSystem.CheckAndSelectNearTargetsParallel();
			}
		}

		private void AdjustSearchingRange()
		{
			m_searchingRange = m_shootingRange;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyObjectBuilder_TurretBase myObjectBuilder_TurretBase = (MyObjectBuilder_TurretBase)objectBuilder;
			MyWeaponBlockDefinition blockDefinition = BlockDefinition;
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = ((blockDefinition == null) ? new MyInventory(0.384f, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive) : new MyInventory(blockDefinition.InventoryMaxVolume, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive));
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_TurretBase.Inventory);
			}
			m_gunBase.Init(myObjectBuilder_TurretBase.GunBase, base.BlockDefinition, this);
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, 0.002f, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			if (Sync.IsServer)
			{
				m_lateStartRandom.Value = MyUtils.GetRandomInt(0, 30);
				LateStartRandom_ValueChanged(null);
			}
			m_lateStartRandom.ValueChanged += LateStartRandom_ValueChanged;
			InitializationMatrix = base.PositionComp.LocalMatrixRef;
			InitializationBarrelMatrix = MatrixD.Identity;
			m_defId = myObjectBuilder_TurretBase.GetId();
			m_shootingRange.ValidateRange(0f, BlockDefinition.MaxRangeMeters);
			m_shootingRange.SetLocalValue(Math.Min(BlockDefinition.MaxRangeMeters, Math.Max(0f, myObjectBuilder_TurretBase.Range)));
			AdjustSearchingRange();
			if (myObjectBuilder_TurretBase.Flags != null)
			{
				TargetMeteors = myObjectBuilder_TurretBase.Flags.TargetMeteors;
				TargetMissiles = myObjectBuilder_TurretBase.Flags.TargetMissiles;
				TargetCharacters = myObjectBuilder_TurretBase.Flags.TargetCharacters;
				TargetSmallGrids = myObjectBuilder_TurretBase.Flags.TargetSmallGrids;
				TargetLargeGrids = myObjectBuilder_TurretBase.Flags.TargetLargeGrids;
				TargetStations = myObjectBuilder_TurretBase.Flags.TargetStations;
				TargetNeutrals = myObjectBuilder_TurretBase.Flags.TargetNeutrals;
				TargetFriends = myObjectBuilder_TurretBase.Flags.TargetFriends;
				TargetEnemies = myObjectBuilder_TurretBase.Flags.TargetEnemies;
			}
			else
			{
				TargetMeteors = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Asteroids) != 0;
				TargetMissiles = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Missiles) != 0;
				TargetCharacters = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Players) != 0;
				TargetSmallGrids = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.SmallShips) != 0;
				TargetLargeGrids = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.LargeShips) != 0;
				TargetStations = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Stations) != 0;
				TargetNeutrals = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Neutrals) != 0;
				TargetFriends = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Friends) != 0;
				TargetEnemies = (BlockDefinition.EnabledTargetingOptions & MyTurretTargetingOptions.Enemies) != 0;
			}
			OnTargetFlagChanged();
			FireAtLockedTarget = myObjectBuilder_TurretBase.FireAtLockedTarget;
			TargetLocking = myObjectBuilder_TurretBase.TargetLocking;
			m_randomStandbyChangeConst_ms = MyUtils.GetRandomInt(GunBase.WeaponProperties.WeaponDefinition.MinimumTimeBetweenIdleRotationsMs, GunBase.WeaponProperties.WeaponDefinition.MaximumTimeBetweenIdleRotationsMs);
			m_targetingGroup.Value = myObjectBuilder_TurretBase.TargetingGroup;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			m_targetToSet = myObjectBuilder_TurretBase.Target;
			m_targetingSystem.IsPotentialTarget = myObjectBuilder_TurretBase.IsPotentialTarget;
			m_turretController.SavedPreviousControlledEntityId = myObjectBuilder_TurretBase.PreviousControlledEntityId;
			Rotation = myObjectBuilder_TurretBase.Rotation;
			Elevation = myObjectBuilder_TurretBase.Elevation;
			m_isPlayerShooting = myObjectBuilder_TurretBase.IsShooting;
			base.Render.NeedsDrawFromParent = m_isPlayerShooting;
			if (BlockDefinition != null)
			{
				m_maxRangeMeter = BlockDefinition.MaxRangeMeters;
				m_minElevationRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MinElevationDegrees));
				m_maxElevationRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MaxElevationDegrees));
				if (m_minElevationRadians > m_maxElevationRadians)
				{
					m_minElevationRadians -= (float)Math.PI * 2f;
				}
				m_minSinElevationRadians = (float)Math.Sin(m_minElevationRadians);
				m_maxSinElevationRadians = (float)Math.Sin(m_maxElevationRadians);
				m_minAzimuthRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MinAzimuthDegrees));
				m_maxAzimuthRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MaxAzimuthDegrees));
				if (m_minAzimuthRadians > m_maxAzimuthRadians)
				{
					m_minAzimuthRadians -= (float)Math.PI * 2f;
				}
				m_rotationSpeed = BlockDefinition.RotationSpeed;
				m_elevationSpeed = BlockDefinition.ElevationSpeed;
				m_enableIdleRotation.Value = BlockDefinition.IdleRotation;
				ClampRotationAndElevation();
				m_minFov = BlockDefinition.MinFov;
				m_maxFov = BlockDefinition.MaxFov;
				m_fov = BlockDefinition.MaxFov;
				m_targetFov = BlockDefinition.MaxFov;
			}
			m_enableIdleRotation.Value &= myObjectBuilder_TurretBase.EnableIdleRotation;
			m_previousIdleRotationState = myObjectBuilder_TurretBase.PreviousIdleRotationState;
			m_gunIdleElevationAzimuthUnknown = true;
			m_targetFlags.ValueChanged += delegate
			{
				OnTargetFlagChanged();
			};
			base.InvalidateOnMove = false;
			CreateUpdateTimer(GetTimerTime(0), MyTimerTypes.Frame10);
<<<<<<< HEAD
			base.ReloadTime = GunBase.ReloadTime;
			m_maxTargetLockingRange.Value = 0f;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void LateStartRandom_ValueChanged(SyncBase obj)
		{
			if (m_barrel != null)
			{
				m_barrel.LateTimeRandom = m_lateStartRandom.Value;
			}
		}

		private float NormalizeAngle(int angle)
		{
			int num = angle % 360;
			if (num == 0 && angle != 0)
			{
				return 360f;
			}
			return num;
		}

		[Event(null, 770)]
		[Reliable]
		[Broadcast]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public void sync_ControlledEntity_Used()
		{
			ReleaseControl();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TurretBase myObjectBuilder_TurretBase = (MyObjectBuilder_TurretBase)base.GetObjectBuilderCubeBlock(copy);
			MyObjectBuilder_UserControllableGun myObjectBuilder_UserControllableGun = myObjectBuilder_TurretBase;
			if (myObjectBuilder_UserControllableGun != null)
			{
				myObjectBuilder_UserControllableGun.IsLargeTurret = true;
			}
			myObjectBuilder_TurretBase.Range = m_shootingRange;
			myObjectBuilder_TurretBase.RemainingAmmo = m_gunBase.CurrentAmmo;
			myObjectBuilder_TurretBase.Target = ((Target != null) ? Target.EntityId : 0);
			myObjectBuilder_TurretBase.IsPotentialTarget = m_targetingSystem.IsPotentialTarget;
			myObjectBuilder_TurretBase.EnableIdleRotation = EnableIdleRotation;
			myObjectBuilder_TurretBase.Flags = new MyObjectBuilder_TargetingFlags();
			myObjectBuilder_TurretBase.Flags.TargetMeteors = TargetMeteors;
			myObjectBuilder_TurretBase.Flags.TargetMissiles = TargetMissiles;
			myObjectBuilder_TurretBase.Flags.TargetCharacters = TargetCharacters;
			myObjectBuilder_TurretBase.Flags.TargetSmallGrids = TargetSmallGrids;
			myObjectBuilder_TurretBase.Flags.TargetLargeGrids = TargetLargeGrids;
			myObjectBuilder_TurretBase.Flags.TargetStations = TargetStations;
			myObjectBuilder_TurretBase.Flags.TargetNeutrals = TargetNeutrals;
			myObjectBuilder_TurretBase.Flags.TargetEnemies = TargetEnemies;
			myObjectBuilder_TurretBase.TargetLocking = TargetLocking;
			myObjectBuilder_TurretBase.FireAtLockedTarget = FireAtLockedTarget;
			myObjectBuilder_TurretBase.TargetingGroup = GetTargetingGroupHash();
			if (m_turretController.PreviousControlledEntity != null)
			{
				myObjectBuilder_TurretBase.PreviousControlledEntityId = m_turretController.PreviousControlledEntity.Entity.EntityId;
				myObjectBuilder_TurretBase.Rotation = Rotation;
				myObjectBuilder_TurretBase.Elevation = Elevation;
				myObjectBuilder_TurretBase.IsShooting = m_isPlayerShooting;
			}
			myObjectBuilder_TurretBase.GunBase = m_gunBase.GetObjectBuilder();
			myObjectBuilder_TurretBase.PreviousIdleRotationState = m_previousIdleRotationState;
			return myObjectBuilder_TurretBase;
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_lateStartRandom != null)
			{
				m_lateStartRandom.ValueChanged -= LateStartRandom_ValueChanged;
			}
			m_turretController.OnControlAcquired -= AcquireControl;
			m_turretController.OnControlReleased -= ReleaseControl;
			m_turretController.OnRotationUpdate -= RotateModels;
			m_turretController.OnMoveAndRotationUpdate -= MoveAndRotate;
			m_turretController.OnCameraOverlayUpdate -= SetCameraOverlay;
			try
			{
				m_targetingSystem.FinishTasks();
			}
			catch
			{
			}
			base.ResourceSink.IsPoweredChanged -= Receiver_IsPoweredChanged;
			ReleaseControl();
			m_targetingSystem.ResetTarget();
			if (m_barrel != null)
			{
				m_barrel.Close();
				m_barrel = null;
			}
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			m_soundEmitterForRotation.StopSound(forced: true);
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			StopShootingSound();
			StopAimingSound();
			if (m_barrel != null)
			{
				m_barrel.RemoveSmoke();
			}
			if (m_turretController.IsControlled)
			{
				ReleaseControl();
			}
			m_targetingSystem.ResetTarget();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			if (base.IsWorking)
			{
				m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			else
			{
				OnStopWorking();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_targetToSet != 0L && base.IsWorking)
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(m_targetToSet, out entity))
				{
					m_targetingSystem.ResetTarget();
				}
			}
			m_turretController.UpdatePlayerControllers();
			MyCubeGrid cubeGrid = base.CubeGrid;
			MyCubeGridRenderCell orAddCell = cubeGrid.RenderData.GetOrAddCell(base.Position * cubeGrid.GridSize);
			if (orAddCell.ParentCullObject == uint.MaxValue)
			{
				orAddCell.RebuildInstanceParts(cubeGrid.Render.GetRenderFlags());
			}
			if (m_base1 != null)
			{
				m_base1.Render.SetParent(0, orAddCell.ParentCullObject);
				m_base1.NeedsWorldMatrix = false;
				m_base1.InvalidateOnMove = false;
			}
			if (m_base2 != null)
			{
				m_base2.Render.SetParent(0, orAddCell.ParentCullObject);
				m_base2.NeedsWorldMatrix = false;
				m_base2.InvalidateOnMove = false;
			}
			RotateModels();
		}

		public bool WasControllingCockpitWhenSaved()
		{
<<<<<<< HEAD
			if (m_turretController.SavedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(m_turretController.SavedPreviousControlledEntityId.Value, out var entity))
=======
			if (ControllerInfo.Controller != null && MyEntities.TryGetEntityById(m_savedPreviousControlledEntityId.Value, out var entity))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return entity is MyCockpit;
			}
			return false;
		}

		public sealed override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
		}

		public virtual void UpdateBeforeSimulationParallel()
		{
<<<<<<< HEAD
			if (m_turretController.IsControlledByLocalPlayer && MySession.Static.CameraController == this)
=======
			if (IsControlledByLocalPlayer && MySession.Static.CameraController == this)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (MyInput.Static.DeltaMouseScrollWheelValue() != 0 && MyGuiScreenToolbarConfigBase.Static == null && !MyGuiScreenTerminal.IsOpen)
				{
					ChangeZoom(MyInput.Static.DeltaMouseScrollWheelValue());
				}
				MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MyStringId.NullOrEmpty;
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_IN, MyControlStateType.PRESSED))
				{
					ChangeZoomPrecise(0f - GAMEPAD_ZOOM_SPEED);
				}
				else if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_OUT, MyControlStateType.PRESSED))
				{
					ChangeZoomPrecise(GAMEPAD_ZOOM_SPEED);
				}
			}
		}

		public virtual void UpdateAfterSimulationParallel()
		{
			if (!Sync.IsDedicated)
			{
				RotateModels();
			}
		}

		public sealed override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			m_barrel?.UpdateAfterSimulation();
			if (!base.IsWorking || base.Parent.Physics == null || !base.Parent.Physics.Enabled)
			{
				return;
			}
			if (m_turretController.IsControlledByLocalPlayer && MySession.Static.CameraController == this)
			{
				m_fov = MathHelper.Lerp(m_fov, m_targetFov, 0.5f);
				SetFov(m_fov);
			}
			bool flag = base.Render.IsVisible();
			if (flag && base.IsWorking && m_barrel != null)
			{
				if (!m_turretController.IsPlayerControlled && AiEnabled)
				{
					UpdateAiWeapon();
				}
				else if (m_isPlayerShooting)
				{
					if (CanShoot(out var status))
					{
						UpdateShooting(m_isPlayerShooting);
					}
					else if (status == MyGunStatusEnum.OutOfAmmo && m_gunBase.SwitchAmmoMagazineToFirstAvailable())
					{
						status = MyGunStatusEnum.OK;
					}
					if (m_turretController.IsControlledByLocalPlayer && status == MyGunStatusEnum.OutOfAmmo)
					{
						m_outOfAmmoNotification.SetTextFormatArguments(DisplayNameText);
						MyHud.Notifications.Add(m_outOfAmmoNotification);
					}
				}
			}
			if (!m_isShooting && !m_forceShoot && m_barrel != null)
			{
				m_barrel.ResetCurrentLateStart();
			}
			if (!flag || (!m_isShooting && !m_isPlayerShooting && (m_turretController.IsPlayerControlled || !AiEnabled)))
			{
				StopShootingSound();
			}
			if (!NeedsPerFrameUpdate)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

<<<<<<< HEAD
		public bool IsConnected(MyCubeGrid grid)
		{
			return grid.GridSystems.TerminalSystem == base.CubeGrid.GridSystems.TerminalSystem;
		}

		public MyEntity GetTargetingParent()
		{
			return GetTopMostParent();
		}

		public bool IsTargetInView(Vector3D predPos)
		{
			if (Barrel == null)
			{
				return false;
			}
			Vector3 lookAtPositionEuler = LookAt(predPos);
			if (IsInRange(ref lookAtPositionEuler))
			{
				return lookAtPositionEuler.X > Barrel.BarrelElevationMin;
			}
			return false;
		}

		private void UpdateAiWeapon()
=======
		private void UpdateVisibilityCacheCounters()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (!Sync.IsServer)
			{
<<<<<<< HEAD
				return;
=======
				int num = notVisibleTarget.Value - m_notVisibleTargetsUpdatesSinceRefresh;
				if (num > 0)
				{
					m_lastNotVisibleTargets.set_Item(notVisibleTarget.Key, num);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (m_lockedTarget.Value != 0L && m_maxTargetLockingRange.Value != 0f)
			{
				MyEntity entityById = MyEntities.GetEntityById(m_lockedTarget.Value);
				if (entityById == null || Vector3D.DistanceSquared(entityById.PositionComp.GetPosition(), base.PositionComp.GetPosition()) > (double)(m_maxTargetLockingRange.Value * m_maxTargetLockingRange.Value))
				{
					m_lockedTarget.Value = 0L;
				}
			}
<<<<<<< HEAD
			m_targetingSystem.TargetPrediction.UsePrediction = m_barrel.DoesTimingAllowsShooting();
			double targetDistanceSquared = m_targetingSystem.GetTargetDistanceSquared();
=======
			ConcurrentDictionary<MyEntity, int> visibleTargets = m_visibleTargets;
			m_visibleTargets = m_lastVisibleTargets;
			m_lastVisibleTargets = visibleTargets;
		}

		private void UpdateAiWeapon()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			double targetDistanceSquared = GetTargetDistanceSquared();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGunStatusEnum status = MyGunStatusEnum.Cooldown;
			if (targetDistanceSquared < (double)SearchRangeSquared || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				bool flag = (Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual) && (m_targetingSystem.IsTargetVisible(Target) || m_targetingSystem.TargetPrediction.IsTargetPositionManual) && RotationAndElevation() && CanShoot(out status) && !m_targetingSystem.TargetPrediction.IsTargetPositionManual;
				bool flag2 = m_targetingSystem.IsValidTarget(Target);
				UpdateShooting(flag && !m_targetingSystem.IsPotentialTarget && m_targetingSystem.IsTarget(Target) && flag2);
				if (!flag2)
				{
					m_targetingSystem.ResetTarget();
				}
				return;
			}
			if (!m_isShooting)
			{
				Deactivate();
			}
			else
			{
				UpdateShooting(!m_targetingSystem.IsPotentialTarget);
			}
			if (m_isInRandomRotationDistance)
			{
				ResetRandomAiming();
				RandomMovement();
			}
			else
			{
				StopAimingSound();
			}
		}

		private void UpdateShooting(bool shouldShoot)
		{
			if (shouldShoot)
			{
				UpdateShootStatus();
				if (m_status == MyLargeShipGunStatus.MyWeaponStatus_Shooting)
				{
					m_canStopShooting = m_barrel.StartShooting() && m_soundEmitter != null && m_soundEmitter.Loop;
				}
				else if (m_status != MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying && (m_canStopShooting || (m_soundEmitter != null && m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying && m_soundEmitter.Loop)))
				{
					m_barrel.StopShooting();
					m_canStopShooting = false;
				}
			}
			else
			{
				m_status = MyLargeShipGunStatus.MyWeaponStatus_Searching;
				UpdateShootStatus();
				if (m_canStopShooting || (m_soundEmitter != null && m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying && m_soundEmitter.Loop))
				{
					m_barrel.StopShooting();
					m_canStopShooting = false;
				}
			}
		}

		private void Deactivate()
		{
			m_status = MyLargeShipGunStatus.MyWeaponStatus_Deactivated;
			if (m_soundEmitter != null && (m_canStopShooting || (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying && m_soundEmitter.Loop)))
			{
				m_barrel.StopShooting();
				m_canStopShooting = false;
			}
		}

		private void SetShootInterval(ref int shootInterval, ref int shootIntervalConst)
		{
			shootInterval = shootIntervalConst;
		}

		private void UpdateShootStatus()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			switch (m_status)
			{
			case MyLargeShipGunStatus.MyWeaponStatus_Shooting:
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_shootStatusChanged_ms > m_shootInterval_ms)
				{
					StartShootDelaying();
					m_isShooting.Value = true;
				}
				break;
			case MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying:
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_shootStatusChanged_ms > m_shootDelayInterval_ms)
				{
					StartShooting();
					m_isShooting.Value = true;
				}
				break;
			case MyLargeShipGunStatus.MyWeaponStatus_Deactivated:
			case MyLargeShipGunStatus.MyWeaponStatus_Searching:
				StartShootDelaying();
				m_isShooting.Value = false;
				break;
			}
		}

		private void StartShooting()
		{
			m_status = MyLargeShipGunStatus.MyWeaponStatus_Shooting;
			m_shootStatusChanged_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			SetShootInterval(ref m_shootInterval_ms, ref m_shootIntervalConst_ms);
		}

		private void StartShootDelaying()
		{
			m_status = MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying;
			m_shootStatusChanged_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_shootDelayIntervalConst_ms = 0;
			SetShootInterval(ref m_shootDelayInterval_ms, ref m_shootDelayIntervalConst_ms);
		}

		/// <summary>
		/// Sets the new random standy rotation and elevation.
		/// </summary>
		private void ResetRandomAiming()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_randomStandbyChange_ms > m_randomStandbyChangeConst_ms)
			{
				m_randomStandbyRotation = MyUtils.GetRandomFloat(-3.141593f, 3.141593f);
				m_randomStandbyElevation = MyUtils.GetRandomFloat(0f, (float)Math.E * 449f / 777f);
				m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void RandomMovement()
		{
			if (m_barrel == null || !m_enableIdleRotation || (bool)m_forceShoot)
			{
				return;
			}
			float randomStandbyRotation = m_randomStandbyRotation;
			float randomStandbyElevation = m_randomStandbyElevation;
			float num = m_rotationSpeed * 16f;
			bool flag = false;
			float num2 = randomStandbyRotation - Rotation;
			if (num2 * num2 > 9.99999944E-11f)
			{
				Rotation += MathHelper.Clamp(num2, 0f - num, num);
				flag = true;
			}
			if (randomStandbyElevation > m_barrel.BarrelElevationMin)
			{
				num = m_elevationSpeed * 16f;
				num2 = randomStandbyElevation - Elevation;
				if (num2 * num2 > 9.99999944E-11f)
				{
					Elevation += MathHelper.Clamp(num2, 0f - num, num);
					flag = true;
				}
			}
			m_playAimingSound = flag;
			ClampRotationAndElevation();
			if (m_randomIsMoving)
			{
				if (!flag)
				{
					SetupSearchRaycast();
					m_randomIsMoving = false;
				}
			}
			else if (flag)
			{
				m_randomIsMoving = true;
			}
		}

		private void SetupSearchRaycast()
		{
			MatrixD muzzleWorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
			Vector3D hitPosition = muzzleWorldMatrix.Translation + muzzleWorldMatrix.Forward * m_searchingRange;
			m_laserLength = m_searchingRange;
			m_hitPosition = hitPosition;
		}

		protected void GetCameraDummy()
		{
			if (m_base2.Model != null && m_base2.Model.Dummies.ContainsKey(BlockDefinition.CameraDummyName))
			{
				CameraDummy = m_base2.Model.Dummies[BlockDefinition.CameraDummyName];
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			m_transformDirty = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		protected override void RotateModels()
		{
			if (m_base1 == null || m_base2 == null || m_barrel == null || !m_base1.Render.IsChild(0))
<<<<<<< HEAD
=======
			{
				m_transformDirty = false;
				return;
			}
			if (m_transformDirty && !m_isMoving && m_replicableServer != null && Sync.IsDedicated && !m_replicableServer.IsReplicated(m_blocksReplicable) && Target == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_transformDirty = false;
				return;
			}
<<<<<<< HEAD
			if (m_transformDirty && !m_isMoving && m_replicableServer != null && Sync.IsDedicated && !m_replicableServer.IsReplicated(m_blocksReplicable) && Target == null)
			{
				m_transformDirty = false;
				return;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ClampRotationAndElevation();
			Matrix.CreateRotationY(Rotation, out var result);
			result.Translation = m_base1.PositionComp.LocalMatrixRef.Translation;
			Matrix matrix = base.PositionComp.LocalMatrixRef;
			Matrix.Multiply(ref result, ref matrix, out var result2);
			m_base1.PositionComp.SetLocalMatrix(ref result, m_base1.Physics, updateWorld: false, ref result2, forceUpdateRender: true);
			Matrix.CreateRotationX(Elevation, out var result3);
			result3.Translation = m_base2.PositionComp.LocalMatrixRef.Translation;
			Matrix.Multiply(ref result3, ref result2, out var result4);
			m_base2.PositionComp.SetLocalMatrix(ref result3, m_base2.Physics, updateWorld: true, ref result4, forceUpdateRender: true);
			m_barrel.WorldPositionChanged(ref result4);
			m_transformDirty = false;
		}

		public Vector3 LookAt(Vector3D target)
		{
			Vector3D muzzleWorldPosition = m_gunBase.GetMuzzleWorldPosition();
			Vector3.GetAzimuthAndElevation(Vector3.Normalize(Vector3D.TransformNormal(target - muzzleWorldPosition, base.PositionComp.WorldMatrixInvScaled)), out var azimuth, out var elevation);
			if (m_gunIdleElevationAzimuthUnknown)
			{
				Vector3.GetAzimuthAndElevation(m_gunBase.GetMuzzleLocalMatrix().Forward, out m_gunIdleAzimuth, out m_gunIdleElevation);
				m_gunIdleElevationAzimuthUnknown = false;
			}
			return new Vector3(elevation - m_gunIdleElevation, MathHelper.WrapAngle(azimuth - m_gunIdleAzimuth), 0f);
		}

		protected void ResetRotation()
		{
			Rotation = 0f;
			Elevation = 0f;
			ClampRotationAndElevation();
			m_randomStandbyElevation = 0f;
			m_randomStandbyRotation = 0f;
			m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public bool RotationAndElevation()
		{
			bool flag = false;
			Vector3 vector = Vector3.Zero;
			if (Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				bool usePrediction = m_targetingSystem.TargetPrediction.UsePrediction;
				m_targetingSystem.TargetPrediction.UsePrediction = m_targetingSystem.TargetPrediction.IsFastPrediction;
				Vector3D aimPosition = m_targetingSystem.AimPosition;
				m_targetingSystem.TargetPrediction.UsePrediction = usePrediction;
				vector = LookAt(aimPosition);
			}
			float y = vector.Y;
			float x = vector.X;
			float num = m_rotationSpeed * 16f;
			float num2 = MathHelper.WrapAngle(y - Rotation);
			Rotation += MathHelper.Clamp(num2, 0f - num, num);
			flag = num2 * num2 > 9.99999944E-11f;
			if (Rotation > 3.141593f)
			{
				Rotation -= (float)Math.PI * 2f;
			}
			else if (Rotation < -3.141593f)
			{
				Rotation += (float)Math.PI * 2f;
			}
			num = m_elevationSpeed * 16f;
			float num3 = Math.Max(x, m_barrel.BarrelElevationMin) - Elevation;
			Elevation += MathHelper.Clamp(num3, 0f - num, num);
			flag = (m_playAimingSound = flag || num3 * num3 > 9.99999944E-11f);
			ClampRotationAndElevation();
			RotateModels();
			if (Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				float num4 = Math.Abs(y - Rotation);
				float num5 = Math.Abs(x - Elevation);
				IsAimed = num4 <= float.Epsilon && num5 <= 0.01f;
			}
			else
			{
				IsAimed = false;
			}
			return IsAimed;
		}

		private void ClampRotationAndElevation()
		{
			Rotation = ClampRotation(Rotation);
			Elevation = ClampElevation(Elevation);
		}

		private float ClampRotation(float value)
		{
			if (IsRotationLimited())
			{
				value = Math.Min(m_maxAzimuthRadians, Math.Max(m_minAzimuthRadians, value));
			}
			return value;
		}

		private bool IsRotationLimited()
		{
			return (double)Math.Abs(m_maxAzimuthRadians - m_minAzimuthRadians - (float)Math.PI * 2f) > 0.01;
		}

		private float ClampElevation(float value)
		{
			if (IsElevationLimited())
			{
				value = Math.Min(m_maxElevationRadians, Math.Max(m_minElevationRadians, value));
			}
			return value;
		}

		private bool IsElevationLimited()
		{
			return (double)Math.Abs(m_maxElevationRadians - m_minElevationRadians - (float)Math.PI * 2f) > 0.01;
		}

		private void PlayAimingSound()
		{
			if (m_soundEmitterForRotation != null && !m_soundEmitterForRotation.IsPlaying)
			{
				m_soundEmitterForRotation.PlaySound(m_rotatingCueEnum, stopPrevious: true);
			}
		}

		public void PlayShootingSound()
		{
			if (m_soundEmitter != null)
			{
				StopAimingSound();
				m_gunBase.StartShootSound(m_soundEmitter);
			}
		}

		public void StopShootingSound()
		{
			if (m_soundEmitter != null && (m_soundEmitter.SoundId == m_shootingCueEnum.Arcade || m_soundEmitter.SoundId == m_shootingCueEnum.Realistic) && m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		internal void StopAimingSound()
		{
			if (m_soundEmitterForRotation != null && (m_soundEmitterForRotation.SoundId == m_rotatingCueEnum.Arcade || m_soundEmitterForRotation.SoundId == m_rotatingCueEnum.Realistic))
			{
				m_soundEmitterForRotation.StopSound(forced: false);
			}
		}

		public override bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			if (base.GetIntersectionWithLine(ref line, out t, flags))
			{
				return true;
			}
			if (m_barrel == null)
			{
				return false;
			}
			return m_barrel.Entity.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES);
		}

		public override bool GetIntersectionWithLine(ref LineD line, out Vector3D? v, bool useCollisionModel = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			if (base.GetIntersectionWithLine(ref line, out v, useCollisionModel, flags))
			{
				return true;
			}
			return m_barrel.Entity.GetIntersectionWithLine(ref line, out v, useCollisionModel);
		}

		public override bool CanShoot(out MyGunStatusEnum status)
		{
			if (!m_gunBase.HasAmmoMagazines || (m_turretController.IsControlledByLocalPlayer && MySession.Static.CameraController != this))
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySession.Static.CreativeMode && !HasEnoughAmmo())
			{
				status = MyGunStatusEnum.OutOfAmmo;
				m_gunBase.SwitchAmmoMagazineToFirstAvailable();
				return false;
			}
			status = MyGunStatusEnum.OK;
			return true;
		}

		protected int GetRemainingAmmo()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		protected bool HasEnoughAmmo()
		{
			return m_gunBase.HasEnoughAmmunition();
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (!HasPlayerAccess(shooter))
			{
				status = MyGunStatusEnum.AccessDenied;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (action == MyShootActionEnum.PrimaryAction)
			{
				status = MyGunStatusEnum.OK;
				return true;
			}
			status = MyGunStatusEnum.Failed;
			return false;
		}

		public virtual void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			throw new NotImplementedException();
		}

		public Vector3 GetShootDirection()
		{
			return base.WorldMatrix.Forward;
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			throw new NotImplementedException();
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			throw new NotImplementedException();
		}

		public void OnControlAcquired(IMyCharacter owner)
		{
		}

		public void OnControlReleased()
		{
		}

<<<<<<< HEAD
		public void SetInventory(MyInventory inventory, int index)
=======
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
				flag = thisTopmostParent.GridSystems.TerminalSystem == myCubeGrid.GridSystems.TerminalSystem && myCubeGrid.BigOwners.Contains(base.OwnerId);
				if (MyCubeGridGroups.Static.Logical.HasSameGroup(thisTopmostParent, myCubeGrid))
				{
					return false;
				}
			}
			if (!flag && myCubeGrid != null)
			{
				if (!TargetSmallGrids && myCubeGrid.GridSizeEnum == MyCubeSize.Small)
				{
					return false;
				}
				if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					if (!TargetLargeGrids && !myCubeGrid.IsStatic)
					{
						return false;
					}
					if (!TargetStations && myCubeGrid.IsStatic)
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool IsTarget(MyEntity entity)
		{
			if (entity is MyDebrisBase)
			{
				return false;
			}
			if (!TargetMeteors && entity is MyMeteor)
			{
				return false;
			}
			if (!TargetMissiles && entity is MyMissile)
			{
				return false;
			}
			if (entity.Physics != null && !entity.Physics.Enabled)
			{
				return false;
			}
			if (entity is MyDecoy)
			{
				return true;
			}
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				if (TargetCharacters)
				{
					return !myCharacter.IsDead;
				}
				return false;
			}
			if (TargetMeteors && entity is MyMeteor)
			{
				return true;
			}
			if (TargetMissiles && entity is MyMissile)
			{
				return true;
			}
			IMyComponentOwner<MyIDModule> myComponentOwner = entity as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out var _))
			{
				return true;
			}
			return false;
		}

		public bool IsTargetVisible()
		{
			if (!m_currentPrediction.ManualTargetPosition && Target != null && !IsTargetVisible(Target, null, useVisibilityCache: false))
			{
				return false;
			}
			return true;
		}

		private bool IsTargetVisible(MyEntity target, Vector3D? overridePredictedPos = null, bool useVisibilityCache = true)
		{
			if (target == null)
			{
				return false;
			}
			if (target.GetTopMostParent().Physics == null)
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
				using (m_currentPredictionUsageLock.AcquireExclusiveUsing())
				{
					vector3D = m_currentPrediction.GetPredictedTargetPosition(target);
				}
			}
			int num = default(int);
			if (!overridePredictedPos.HasValue && m_notVisibleTargets.TryGetValue(target, ref num) && num > 0 && Sync.IsServer)
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
			Vector3D forward = m_gunBase.WorldMatrix.Forward;
			Vector3D from = base.PositionComp.WorldMatrix.Translation;
			Vector3D to = vector3D;
			if (Vector3.Dot(Vector3.Normalize(to - from), forward) > EXACT_VISIBILITY_TEST_TRESHOLD_ANGLE)
			{
				from = m_gunBase.GetMuzzleWorldPosition() + forward * 0.5;
			}
			if (useVisibilityCache)
			{
				MyPhysics.CastRayParallel(ref from, ref to, 15, delegate(MyPhysics.HitInfo? physTarget)
				{
					OnVisibilityRayCastCompleted(target, physTarget);
				});
				return true;
			}
			MyPhysics.HitInfo? physTarget2 = MyPhysics.CastRay(from, to, 15);
			OnVisibilityRayCastCompleted(target, physTarget2);
			return true;
		}

		private void OnVisibilityRayCastCompleted(MyEntity target, MyPhysics.HitInfo? physTarget)
		{
			VRage.ModAPI.IMyEntity myEntity = null;
			if (physTarget.HasValue && physTarget.Value.HkHitInfo.Body != null && physTarget.Value.HkHitInfo.Body.UserObject != null && physTarget.Value.HkHitInfo.Body.UserObject is MyPhysicsBody)
			{
				myEntity = ((MyPhysicsBody)physTarget.Value.HkHitInfo.Body.UserObject).Entity;
			}
			if (myEntity == null || target == myEntity || target.Parent == myEntity || (target.Parent != null && target.Parent == myEntity.Parent) || myEntity is MyMissile || myEntity is MyFloatingObject)
			{
				SetTargetVisible(target, visible: true);
			}
			else
			{
				SetTargetVisible(target, visible: false);
			}
		}

		private void SetTargetVisible(MyEntity target, bool visible, int? timeout = null)
		{
			if (visible)
			{
				m_notVisibleTargets.Remove<MyEntity, int>(target);
				m_visibleTargets.TryAdd(target, m_VisibleCachePeriod);
			}
			else if (timeout.HasValue)
			{
				m_notVisibleTargets.set_Item(target, timeout.Value);
			}
			else if (m_notVisibleTargets.ContainsKey(target))
			{
				m_notVisibleTargets.set_Item(target, 12 + MyRandom.Instance.Next(4));
			}
			else
			{
				m_notVisibleTargets.TryAdd(target, 4 + MyRandom.Instance.Next(4));
			}
		}

		private MyEntity GetNearestPotentialTarget(float rangeSq, List<MyEntityWithDistSq> outPotentialTargets)
		{
			outPotentialTargets.Clear();
			MyEntity[] array = m_targetSelectionWorkData.GridTargeting.TargetRoots.ToArray();
			if (array.Length > 1)
			{
				Vector3D position = base.PositionComp.GetPosition();
				ArrayExtensions.EnsureCapacity(ref m_distanceEntityKeys, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					MyEntity myEntity = array[i];
					float num = ((myEntity == null || myEntity.Closed) ? float.MaxValue : ((float)(myEntity.PositionComp.GetPosition() - position).LengthSquared()));
					m_distanceEntityKeys[i] = num;
				}
				Array.Sort(m_distanceEntityKeys, array, 0, array.Length, FloatComparer.Instance);
			}
			MyEntity nearestTarget = null;
			double minDistanceSq = rangeSq;
			bool foundDecoy = false;
			MyCubeGrid myCubeGrid = GetTopMostParent() as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyEntity[] array2 = array;
				foreach (MyEntity myEntity2 in array2)
				{
					if (myEntity2 == null || myEntity2.Closed)
					{
						break;
					}
					if (IsTargetRootValid(myEntity2, myCubeGrid))
					{
						m_targetSelectionWorkData.GridTargeting.AllowScanning = false;
						TestPotentialTarget(myEntity2, ref nearestTarget, ref minDistanceSq, ref foundDecoy, outPotentialTargets);
						m_targetSelectionWorkData.GridTargeting.AllowScanning = true;
						if (nearestTarget != null)
						{
							break;
						}
					}
				}
			}
			return nearestTarget;
		}

		private bool TestPotentialTarget(MyEntity target, ref MyEntity nearestTarget, ref double minDistanceSq, ref bool foundDecoy, List<MyEntityWithDistSq> outPotentialTargets)
		{
			if (target == null)
			{
				return false;
			}
			if (target.MarkedForClose || target.Closed)
			{
				return false;
			}
			int num = default(int);
			if (m_notVisibleTargets.TryGetValue(target, ref num) && num > 0)
			{
				return false;
			}
			MyCharacter myCharacter = target as MyCharacter;
			if (myCharacter != null)
			{
				ulong num2 = MySession.Static.Players.TryGetSteamId(myCharacter.GetPlayerIdentityId());
				if (num2 != 0L && MySession.Static.RemoteAdminSettings.TryGetValue(num2, out var value) && value.HasFlag(AdminSettingsEnum.Untargetable))
				{
					return false;
				}
			}
			MyCubeGrid myCubeGrid = target as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (myCubeGrid.GridSystems.TerminalSystem == base.CubeGrid.GridSystems.TerminalSystem && myCubeGrid.BigOwners.Contains(base.OwnerId))
				{
					return false;
				}
				if (myCubeGrid.PositionComp.WorldAABB.DistanceSquared(base.PositionComp.GetPosition()) > minDistanceSq)
				{
					return false;
				}
				bool flag = false;
				HashSetReader<MyDecoy> decoys = myCubeGrid.Decoys;
				if (decoys.IsValid)
				{
					MyDecoy[] array = decoys.ToArray();
					foreach (MyDecoy target2 in array)
					{
						flag |= TestPotentialTarget(target2, ref nearestTarget, ref minDistanceSq, ref foundDecoy, outPotentialTargets);
					}
				}
				if (flag && IsTargetVisible(nearestTarget))
				{
					return true;
				}
				List<MyEntity> orDefault = m_targetSelectionWorkData.GridTargeting.TargetBlocks.GetOrDefault(myCubeGrid);
				if (orDefault != null)
				{
					using (m_targetSelectionWorkData.GridTargeting.ScanLock.AcquireSharedUsing())
					{
						foreach (MyEntity item2 in orDefault)
						{
							flag |= TestPotentialTarget(item2, ref nearestTarget, ref minDistanceSq, ref foundDecoy, outPotentialTargets);
						}
					}
				}
				if (flag)
				{
					return IsTargetVisible(nearestTarget);
				}
				return false;
			}
			bool flag2 = IsDecoy(target);
			if (foundDecoy && !flag2)
			{
				return false;
			}
			bool num3 = IsTarget(target);
			bool flag3 = num3 && IsTargetEnemy(target);
			if (!num3 || !flag3)
			{
				return false;
			}
			double num4 = Vector3D.DistanceSquared(target.PositionComp.GetPosition(), base.PositionComp.GetPosition());
			MyEntityWithDistSq item;
			if (num4 >= minDistanceSq)
			{
				item = new MyEntityWithDistSq
				{
					Entity = target,
					DistSq = num4
				};
				outPotentialTargets.Add(item);
				return false;
			}
			if (flag2)
			{
				nearestTarget = target;
				minDistanceSq = num4;
				foundDecoy = true;
				item = new MyEntityWithDistSq
				{
					Entity = target,
					DistSq = num4
				};
				outPotentialTargets.Add(item);
				return true;
			}
			if (!IsTargetAimedByOtherTurret(target))
			{
				minDistanceSq = num4;
				nearestTarget = target;
				item = new MyEntityWithDistSq
				{
					Entity = target,
					DistSq = num4
				};
				outPotentialTargets.Add(item);
				return true;
			}
			return false;
		}

		private MyEntity GetNearestTarget(double maxTargetDistanceSq, List<MyEntityWithDistSq> validPotentialTargets)
		{
			MyEntity myEntity = null;
			validPotentialTargets.Sort((MyEntityWithDistSq a, MyEntityWithDistSq b) => a.DistSq.CompareTo(b.DistSq));
			foreach (MyEntityWithDistSq validPotentialTarget in validPotentialTargets)
			{
				if (validPotentialTarget.DistSq > maxTargetDistanceSq)
				{
					break;
				}
				if (IsDecoy(validPotentialTarget.Entity) && TestTarget(validPotentialTarget.Entity, decoyOnly: true))
				{
					myEntity = validPotentialTarget.Entity;
					break;
				}
			}
			if (myEntity == null)
			{
				foreach (MyEntityWithDistSq validPotentialTarget2 in validPotentialTargets)
				{
					if (validPotentialTarget2.DistSq > maxTargetDistanceSq)
					{
						return myEntity;
					}
					if (!IsDecoy(validPotentialTarget2.Entity) && TestTarget(validPotentialTarget2.Entity, decoyOnly: false))
					{
						return validPotentialTarget2.Entity;
					}
				}
				return myEntity;
			}
			return myEntity;
		}

		private bool TestTarget(MyEntity target, bool decoyOnly)
		{
			int num = default(int);
			if (m_notVisibleTargets.TryGetValue(target, ref num) && num > 0)
			{
				return false;
			}
			Vector3D vector3D = Vector3D.Zero;
			using (m_currentPredictionUsageLock.AcquireExclusiveUsing())
			{
				vector3D = m_currentPrediction.GetPredictedTargetPosition(target);
			}
			if (!IsTargetInView(vector3D) || !IsTargetVisible(target, vector3D))
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
				return ((IMyDestroyableObject)target).Integrity < 2f * m_gunBase.MechanicalDamage;
			}
			return false;
		}

		private bool IsTargetInView(Vector3D predPos)
		{
			Vector3 lookAtPositionEuler = LookAt(predPos);
			if (m_barrel != null && lookAtPositionEuler.X > m_barrel.BarrelElevationMin)
			{
				return IsInRange(ref lookAtPositionEuler);
			}
			return false;
		}

		private bool IsTargetEnemy(MyEntity target)
		{
			MyCubeGrid myCubeGrid = target as MyCubeGrid;
			if (myCubeGrid != null)
			{
				return myCubeGrid.BigOwners.Count == 0;
			}
			IMyComponentOwner<MyIDModule> myComponentOwner = target as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null)
			{
				if (myComponentOwner.GetComponent(out var component))
				{
					return IsTargetIdentityEnemy(component.Owner);
				}
				return false;
			}
			MyMissile myMissile = target as MyMissile;
			if (myMissile != null)
			{
				return IsTargetIdentityEnemy(myMissile.Owner);
			}
			return true;
		}

		private bool IsTargetIdentityEnemy(long targetIidentityId)
		{
			if (TargetNeutrals && targetIidentityId == 0L)
			{
				return true;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(targetIidentityId);
			if (TargetNeutrals && userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Neutral)
			{
				return true;
			}
			if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies)
			{
				return true;
			}
			return false;
		}

		private void CheckAndSelectNearTargetsParallel()
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
			m_targetSelectionWorkData.GridTargeting = base.CubeGrid.Components.Get<MyGridTargeting>();
			m_targetSelectionWorkData.Entity = this;
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

		private void CheckNearTargets(ref MyEntity suggestedTarget, ref bool suggestedTargetIsOnlyPotential)
		{
			if (!m_checkOtherTargets)
			{
				return;
			}
			float num = (float)m_shootingRange * (float)m_shootingRange;
			float num2 = m_searchingRange * m_searchingRange;
			MyEntity myEntity = null;
			MyEntity myEntity2 = suggestedTarget;
			double num3 = num2;
			if (myEntity2 != null)
			{
				if (m_barrel != null && m_barrel.Entity != null)
				{
					num3 = (myEntity2.PositionComp.GetPosition() - m_barrel.Entity.PositionComp.GetPosition()).LengthSquared();
				}
				if (num3 < (double)Math.Min(num, num2) && IsTarget(myEntity2) && IsTargetVisible(myEntity2))
				{
					myEntity = myEntity2;
				}
			}
			if (myEntity == null)
			{
				using (MyUtils.ReuseCollection(ref m_allPotentialTargets))
				{
					if (GetNearestPotentialTarget(num2, m_allPotentialTargets) != null || m_allPotentialTargets.Count > 0)
					{
						MyEntity nearestTarget = GetNearestTarget(num, m_allPotentialTargets);
						if (nearestTarget != null)
						{
							myEntity = nearestTarget;
						}
					}
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

		public double GetTargetDistanceSquared()
		{
			if (Target != null && m_barrel != null && m_barrel.Entity != null)
			{
				return (Target.PositionComp.GetPosition() - m_barrel.Entity.PositionComp.GetPosition()).LengthSquared();
			}
			return m_searchingRange * m_searchingRange;
		}

		public void ResetTarget()
		{
			SetTarget(null, isPotential: true);
		}

		public void SetTarget(MyEntity target, bool isPotential)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			base.Components.Add((MyInventoryBase)inventory);
		}

		public int GetTotalAmmunitionAmount()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		public int GetAmmunitionAmount()
		{
			return m_gunBase.GetAmmunitionAmount();
		}

<<<<<<< HEAD
		public int GetMagazineAmount()
		{
=======
		public int GetTotalAmmunitionAmount()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		public int GetAmmunitionAmount()
		{
			return m_gunBase.GetAmmunitionAmount();
		}

		public int GetMagazineAmount()
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return m_gunBase.GetMagazineAmount();
		}

		public void RemoveAmmoPerShot()
		{
			m_gunBase.ConsumeAmmo();
		}

		public void SetTargetingMode(MyTargetingGroupDefinition definition)
		{
			m_targetingGroup.Value = ((definition == null) ? MyStringHash.NullOrEmpty : MyStringHash.GetOrCompute(definition.Id.SubtypeName));
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLargeTurretBase>())
			{
				base.CreateTerminalControls();
				InitializeTargetingSystem();
				m_targetingSystem.InjectTerminalControls(this, allowIdleMovement: true);
			}
		}

		public void CopyTarget()
		{
			if (MySession.Static.LocalCharacter == null)
			{
<<<<<<< HEAD
				return;
			}
			MyShipController myShipController = MySession.Static.LocalCharacter.Parent as MyShipController;
			if (myShipController == null && MySession.Static.ControlledEntity != null)
			{
				myShipController = MySession.Static.ControlledEntity.Entity as MyShipController;
			}
			MyTargetLockingComponent targetLockingComp = MySession.Static.LocalCharacter.TargetLockingComp;
			if (targetLockingComp == null)
			{
				return;
			}
			if (targetLockingComp.Target == null)
			{
				MyHud.Notifications.Add(m_noTargetNotification);
				return;
			}
			if (!targetLockingComp.IsTargetLocked)
			{
				MyHud.Notifications.Add(m_lockingInProgressNotification);
				return;
			}
			MyTargetFocusComponent targetFocusComp = MySession.Static.LocalCharacter.TargetFocusComp;
			if (targetFocusComp != null)
			{
				m_maxTargetLockingRange.Value = (float)targetFocusComp.FocusSearchMaxDistance;
			}
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.CopyTargetServer, MySession.Static.LocalCharacter.EntityId, myShipController.EntityId);
=======
				MyTerminalControlButton<MyLargeTurretBase> obj = new MyTerminalControlButton<MyLargeTurretBase>("Control", MySpaceTexts.ControlRemote, MySpaceTexts.Blank, delegate(MyLargeTurretBase t)
				{
					t.RequestControl();
				})
				{
					Enabled = (MyLargeTurretBase t) => t.CanControl(),
					SupportsMultipleBlocks = false
				};
				MyTerminalAction<MyLargeTurretBase> myTerminalAction = obj.EnableAction(MyTerminalActionIcons.TOGGLE);
				if (myTerminalAction != null)
				{
					myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType> { MyToolbarType.ButtonPanel };
					myTerminalAction.ValidForGroups = false;
				}
				MyTerminalControlFactory.AddControl(obj);
			}
			MyTerminalControlSlider<MyLargeTurretBase> obj2 = new MyTerminalControlSlider<MyLargeTurretBase>("Range", MySpaceTexts.BlockPropertyTitle_LargeTurretRadius, MySpaceTexts.BlockPropertyTitle_LargeTurretRadius)
			{
				Normalizer = (MyLargeTurretBase x, float f) => x.NormalizeRange(f),
				Denormalizer = (MyLargeTurretBase x, float f) => x.DenormalizeRange(f),
				DefaultValue = 800f,
				Getter = (MyLargeTurretBase x) => x.ShootingRange,
				Setter = delegate(MyLargeTurretBase x, float v)
				{
					x.ShootingRange = v;
				},
				Writer = delegate(MyLargeTurretBase x, StringBuilder result)
				{
					result.AppendInt32((int)(float)x.m_shootingRange).Append(" m");
				}
			};
			obj2.EnableActions();
			MyTerminalControlFactory.AddControl(obj2);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj3 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("EnableIdleMovement", MySpaceTexts.BlockPropertyTitle_LargeTurretEnableTurretIdleMovement)
			{
				Getter = (MyLargeTurretBase x) => x.EnableIdleRotation,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.EnableIdleRotation = v;
				}
			};
			obj3.EnableToggleAction();
			obj3.EnableOnOffActions();
			MyTerminalControlFactory.AddControl(obj3);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyLargeTurretBase>());
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj4 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetMeteors", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetMeteors)
			{
				Getter = (MyLargeTurretBase x) => x.TargetMeteors,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetMeteors = v;
				}
			};
			obj4.EnableToggleAction(MyTerminalActionIcons.METEOR_TOGGLE);
			obj4.EnableOnOffActions(MyTerminalActionIcons.METEOR_ON, MyTerminalActionIcons.METEOR_OFF);
			MyTerminalControlFactory.AddControl(obj4);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj5 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetMissiles", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetMissiles)
			{
				Getter = (MyLargeTurretBase x) => x.TargetMissiles,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetMissiles = v;
				}
			};
			obj5.EnableToggleAction(MyTerminalActionIcons.MISSILE_TOGGLE);
			obj5.EnableOnOffActions(MyTerminalActionIcons.MISSILE_ON, MyTerminalActionIcons.MISSILE_OFF);
			MyTerminalControlFactory.AddControl(obj5);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj6 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetSmallShips", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetSmallGrids)
			{
				Getter = (MyLargeTurretBase x) => x.TargetSmallGrids,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetSmallGrids = v;
				}
			};
			obj6.EnableToggleAction(MyTerminalActionIcons.SMALLSHIP_TOGGLE);
			obj6.EnableOnOffActions(MyTerminalActionIcons.SMALLSHIP_ON, MyTerminalActionIcons.SMALLSHIP_OFF);
			MyTerminalControlFactory.AddControl(obj6);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj7 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetLargeShips", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetLargeGrids)
			{
				Getter = (MyLargeTurretBase x) => x.TargetLargeGrids,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetLargeGrids = v;
				}
			};
			obj7.EnableToggleAction(MyTerminalActionIcons.LARGESHIP_TOGGLE);
			obj7.EnableOnOffActions(MyTerminalActionIcons.LARGESHIP_ON, MyTerminalActionIcons.LARGESHIP_OFF);
			MyTerminalControlFactory.AddControl(obj7);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj8 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetCharacters", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetCharacters)
			{
				Getter = (MyLargeTurretBase x) => x.TargetCharacters,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetCharacters = v;
				}
			};
			obj8.EnableToggleAction(MyTerminalActionIcons.CHARACTER_TOGGLE);
			obj8.EnableOnOffActions(MyTerminalActionIcons.CHARACTER_ON, MyTerminalActionIcons.CHARACTER_OFF);
			MyTerminalControlFactory.AddControl(obj8);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj9 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetStations", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetStations)
			{
				Getter = (MyLargeTurretBase x) => x.TargetStations,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetStations = v;
				}
			};
			obj9.EnableToggleAction(MyTerminalActionIcons.STATION_TOGGLE);
			obj9.EnableOnOffActions(MyTerminalActionIcons.STATION_ON, MyTerminalActionIcons.STATION_OFF);
			MyTerminalControlFactory.AddControl(obj9);
			MyTerminalControlOnOffSwitch<MyLargeTurretBase> obj10 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetNeutrals", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetNeutrals)
			{
				Getter = (MyLargeTurretBase x) => x.TargetNeutrals,
				Setter = delegate(MyLargeTurretBase x, bool v)
				{
					x.TargetNeutrals = v;
				}
			};
			obj10.EnableToggleAction(MyTerminalActionIcons.NEUTRALS_TOGGLE);
			obj10.EnableOnOffActions(MyTerminalActionIcons.NEUTRALS_ON, MyTerminalActionIcons.NEUTRALS_OFF);
			MyTerminalControlFactory.AddControl(obj10);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[Event(null, 2136)]
		[Reliable]
		[Server]
		public void CopyTargetServer(long characterEntityId, long cockpitEntityId)
		{
			MyCharacter myCharacter = MyEntities.GetEntityById(characterEntityId) as MyCharacter;
			MyShipController myShipController = MyEntities.GetEntityById(cockpitEntityId) as MyShipController;
			if (myCharacter == null || myShipController == null || myCharacter.GetPlayerIdentityId() != MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value) || myShipController.Pilot == null || myShipController.Pilot != myCharacter)
			{
				return;
			}
			MyTargetLockingComponent targetLockingComp = myCharacter.TargetLockingComp;
			if (targetLockingComp != null)
			{
				MyCubeGrid target = targetLockingComp.Target;
				if (target != null && targetLockingComp.IsTargetLocked)
				{
					m_lockedTarget.Value = target.EntityId;
				}
			}
		}

		public void ForgetTarget()
		{
<<<<<<< HEAD
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.ForgetTargetServer);
=======
			if (m_savedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(m_savedPreviousControlledEntityId.Value, out var entity))
			{
				return entity is MyCockpit;
			}
			return false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[Event(null, 2167)]
		[Reliable]
		[Server]
		public void ForgetTargetServer()
		{
			m_lockedTarget.Value = 0L;
			if (Pilot != null)
			{
				Pilot.TargetLockingComp.ReleaseTargetLock();
			}
		}

		public bool CanControl()
		{
			return m_turretController.CanControl();
		}

		public void RequestControl()
		{
			m_turretController.RequestControl();
		}

		private void AcquireControl(Sandbox.Game.Entities.IMyControllableEntity previousControlledEntity)
		{
			if (previousControlledEntity.ControllerInfo.Controller != null)
			{
				previousControlledEntity.SwitchControl(this);
			}
			if (m_turretController.IsControlledByLocalPlayer)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
				m_targetFov = m_maxFov;
				SetFov(m_maxFov);
				UpdateShooting(m_isPlayerShooting);
			}
			MyCharacter myCharacter = m_turretController.PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.CurrentRemoteControl = this;
			}
			OnStopAI();
			m_targetingSystem.ResetTarget();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public void ForceReleaseControl()
		{
			ReleaseControl();
		}

		private void ReleaseControl(bool previousClosed = false)
		{
			if (m_turretController.IsControlledByLocalPlayer && MyGuiScreenHudSpace.Static != null)
			{
				MyGuiScreenHudSpace.Static.SetToolbarVisible(visible: true);
			}
			if (!m_turretController.IsPlayerControlled)
			{
				return;
			}
			if (Sync.IsServer)
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
			MyCockpit myCockpit = m_turretController.PreviousControlledEntity as MyCockpit;
			if (myCockpit != null && (previousClosed || myCockpit.Pilot == null || myCockpit.MarkedForClose || myCockpit.Closed))
			{
				ReturnControl(m_turretController.CockpitPilot);
				return;
			}
			MyCharacter myCharacter = m_turretController.PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.CurrentRemoteControl = null;
			}
			base.CubeGrid.ControlledFromTurret = false;
			ReturnControl(m_turretController.PreviousControlledEntity);
		}

		private void OnControlReleased(MyEntityController controller)
		{
			RemoveCameraOverlay();
		}

<<<<<<< HEAD
		private void SetCameraOverlay()
=======
		[Event(null, 3311)]
		[Reliable]
		[Broadcast]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void sync_ControlledEntity_Used()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (m_turretController.IsControlledByLocalPlayer)
			{
				base.CubeGrid.GridSystems.CameraSystem.ResetCurrentCamera();
				if (BlockDefinition != null && BlockDefinition.OverlayTexture != null)
				{
					MyHudCameraOverlay.TextureName = BlockDefinition.OverlayTexture;
					MyHudCameraOverlay.Enabled = true;
				}
				else
				{
					MyHudCameraOverlay.Enabled = false;
				}
			}
		}

		private void RemoveCameraOverlay()
		{
			if (m_turretController.IsControlledByLocalPlayer)
			{
				if (MyGuiScreenHudSpace.Static != null)
				{
					MyGuiScreenHudSpace.Static.SetToolbarVisible(visible: true);
				}
				MyHudCameraOverlay.Enabled = false;
				m_turretController.GetFirstRadioReceiver()?.Clear();
				ExitView();
			}
		}

		public void ExitView()
		{
			MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			if (m_turretController.PreviousControlledEntity != null && Sync.IsServer && ControllerInfo.Controller != null && !HasPlayerAccess(ControllerInfo.Controller.Player.Identity.IdentityId))
			{
				MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.sync_ControlledEntity_Used);
			}
		}

		public void UpdateRotationAndElevation(float newRotation, float newElevation)
		{
			Rotation = newRotation;
			Elevation = newElevation;
			RotateModels();
		}

		private void ReturnControl(Sandbox.Game.Entities.IMyControllableEntity nextControllableEntity)
		{
			if (ControllerInfo.Controller != null)
			{
				this.SwitchControl(nextControllableEntity);
			}
			m_turretController.PreviousControlledEntity = null;
			m_randomStandbyElevation = Elevation;
			m_randomStandbyRotation = Rotation;
			m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

<<<<<<< HEAD
=======
		private MyCharacter GetUser()
		{
			if (PreviousControlledEntity != null)
			{
				if (PreviousControlledEntity is MyCockpit)
				{
					return (PreviousControlledEntity as MyCockpit).Pilot;
				}
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return myCharacter;
				}
			}
			return null;
		}

		private bool IsInRangeAndPlayerHasAccess()
		{
			if (ControllerInfo.Controller == null)
			{
				return false;
			}
			MyTerminalBlock myTerminalBlock = PreviousControlledEntity as MyTerminalBlock;
			if (myTerminalBlock == null)
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return MyAntennaSystem.Static.CheckConnection(myCharacter, base.CubeGrid, ControllerInfo.Controller.Player);
				}
				return true;
			}
			MyCubeGrid cubeGrid = myTerminalBlock.SlimBlock.CubeGrid;
			return MyAntennaSystem.Static.CheckConnection(cubeGrid, base.CubeGrid, ControllerInfo.Controller.Player);
		}

		private MyDataReceiver GetFirstRadioReceiver()
		{
			MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				return myCharacter.RadioReceiver;
			}
			HashSet<MyDataReceiver> output = new HashSet<MyDataReceiver>();
			MyAntennaSystem.Static.GetEntityReceivers(base.CubeGrid, ref output, 0L);
			if (output.get_Count() > 0)
			{
				return output.FirstElement<MyDataReceiver>();
			}
			return null;
		}

		private void AddPreviousControllerEvents()
		{
			m_previousControlledEntity.Entity.OnMarkForClose += Entity_OnPreviousMarkForClose;
			MyTerminalBlock myTerminalBlock = m_previousControlledEntity.Entity as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				myTerminalBlock.IsWorkingChanged += PreviousCubeBlock_IsWorkingChanged;
			}
		}

		private void PreviousCubeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (!obj.IsWorking && !obj.Closed && !obj.MarkedForClose)
			{
				ReleaseControl();
			}
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			if (PreviousControlledEntity != null && Sync.IsServer && ControllerInfo.Controller != null && !HasPlayerAccess(ControllerInfo.Controller.Player.Identity.IdentityId))
			{
				MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.sync_ControlledEntity_Used);
			}
		}

		private void Entity_OnPreviousMarkForClose(MyEntity obj)
		{
			ReleaseControl(previousClosed: true);
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public UseActionResult CanUse(UseActionEnum actionEnum, Sandbox.Game.Entities.IMyControllableEntity user)
		{
			return m_turretController.CanUse(actionEnum, user);
		}

		public void RemoveUsers(bool local)
		{
		}

		private new MatrixD GetViewMatrix()
		{
			RotateModels();
			MatrixD matrix = m_base2.WorldMatrix;
			if (CameraDummy != null)
			{
				Matrix m = Matrix.Normalize(CameraDummy.Matrix);
				matrix = MatrixD.Multiply(m, matrix);
			}
			else
			{
				matrix.Translation += matrix.Forward * ForwardCameraOffset;
				matrix.Translation += matrix.Up * UpCameraOffset;
			}
			MatrixD.Invert(ref matrix, out var result);
			return result;
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			currentCamera.SetViewMatrix(GetViewMatrix());
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
		}

		void IMyCameraController.RotateStopped()
		{
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			SetCameraOverlay();
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			RemoveCameraOverlay();
		}

		bool IMyCameraController.HandleUse()
		{
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.LocalCharacter);
				m_targetFov = m_maxFov;
				SetFov(m_maxFov);
			}
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		public override void SerializeControls(BitStream stream)
		{
			m_turretController.SerializeControls(stream);
		}

		public override void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			m_turretController.DeserializeControls(stream, outOfOrder);
		}

		private void ChangeZoom(int deltaZoom)
		{
			if (deltaZoom > 0)
			{
				m_targetFov -= 0.15f;
				if (m_targetFov < m_minFov)
				{
					m_targetFov = m_minFov;
				}
			}
			else
			{
				m_targetFov += 0.15f;
				if (m_targetFov > m_maxFov)
				{
					m_targetFov = m_maxFov;
				}
			}
			SetFov(m_fov);
		}

		private void ChangeZoomPrecise(float deltaZoom)
<<<<<<< HEAD
=======
		{
			m_targetFov += deltaZoom;
			if (deltaZoom < 0f)
			{
				if (m_targetFov < m_minFov)
				{
					m_targetFov = m_minFov;
				}
			}
			else if (m_targetFov > m_maxFov)
			{
				m_targetFov = m_maxFov;
			}
			SetFov(m_fov);
		}

		private float GetZoomNormalized()
		{
			return (m_targetFov - m_minFov) / (m_maxFov - m_minFov);
		}

		private float GetRotationMultiplier()
		{
			float zoomNormalized = GetZoomNormalized();
			return zoomNormalized * ROTATION_MULTIPLIER_NORMAL + (1f - zoomNormalized) * ROTATION_MULTIPLIER_ZOOMED;
		}

		public void ExitView()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			m_targetFov += deltaZoom;
			if (deltaZoom < 0f)
			{
				if (m_targetFov < m_minFov)
				{
					m_targetFov = m_minFov;
				}
			}
			else if (m_targetFov > m_maxFov)
			{
				m_targetFov = m_maxFov;
			}
			SetFov(m_fov);
		}

		private static void SetFov(float fov)
		{
			fov = MathHelper.Clamp(fov, 1E-05f, (float)Math.PI * 179f / 180f);
			MySector.MainCamera.FieldOfView = fov;
		}

		public MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceBoneAnim = false, bool forceHeadBone = false)
		{
			return GetViewMatrix();
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator, overrideControlCheck: false, MyInput.Static.IsAnyAltKeyPressed());
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator, bool overrideControlCheck, bool rotateShip)
		{
			LastRotationIndicator = new Vector3(rotationIndicator, rollIndicator);
			float rotationMultiplier = GetRotationMultiplier();
			m_turretController.LastNetMoveState = new MyGridClientState
			{
				Move = moveIndicator,
				Rotation = rotationIndicator * rotationMultiplier,
				Roll = rollIndicator * rotationMultiplier
			};
			m_turretController.LastNetRotateShip = rotateShip;
			bool flag = false;
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null)
			{
				bool flag2 = true;
				if (!overrideControlCheck && base.CubeGrid.HasMainCockpit() && (myShipController.Pilot == null || myShipController.Pilot != MySession.Static.LocalCharacter))
				{
					flag2 = false;
				}
				if (flag2 && (overrideControlCheck || myShipController.HasLocalPlayerAccess()))
				{
					m_turretController.LastNetCanControl = true;
					if (rotateShip)
					{
						myShipController.MoveAndRotate(moveIndicator, rotationIndicator * rotationMultiplier, rollIndicator * rotationMultiplier);
						flag = true;
					}
					else
					{
						myShipController.MoveAndRotate(moveIndicator, Vector2.Zero, rollIndicator * rotationMultiplier);
					}
					myShipController.MoveAndRotate();
					base.CubeGrid.ControlledFromTurret = true;
				}
				else
				{
					m_turretController.LastNetCanControl = false;
				}
			}
			if (!flag && (rotationIndicator.X != 0f || rotationIndicator.Y != 0f) && m_barrel != null && base.SyncObject != null)
			{
				float num = 0.05f * m_rotationSpeed * 16f;
				Rotation -= rotationIndicator.Y * num * rotationMultiplier;
				Elevation -= rotationIndicator.X * num * rotationMultiplier;
				Elevation = MathHelper.Clamp(Elevation, m_minElevationRadians, 1.5533427f);
				RotateModels();
				m_rotationAndElevationSync.Value = new SyncRotationAndElevation
				{
					Rotation = Rotation,
					Elevation = Elevation
				};
			}
		}

		private float GetRotationMultiplier()
		{
			float zoomNormalized = GetZoomNormalized();
			return zoomNormalized * ROTATION_MULTIPLIER_NORMAL + (1f - zoomNormalized) * ROTATION_MULTIPLIER_ZOOMED;
		}

		private float GetZoomNormalized()
		{
			return (m_targetFov - m_minFov) / (m_maxFov - m_minFov);
		}

		public void MoveAndRotateStopped()
		{
			RotateModels();
		}

		/// <summary>
		/// This will be called locally to start shooting with the given action
		/// </summary>
		public override void BeginShoot(MyShootActionEnum action)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.OnBeginShoot, action);
		}

		/// <summary>
		/// This will be called locally to start shooting with the given action
		/// </summary>
		public override void EndShoot(MyShootActionEnum action)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.OnEndShoot, action);
		}

<<<<<<< HEAD
		/// <summary>
		/// This will be called back from the sync object both on local and remote clients
		/// </summary>
		[Event(null, 2669)]
=======
		[Event(null, 3804)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnBeginShoot(MyShootActionEnum action)
		{
			if (action == MyShootActionEnum.PrimaryAction || action == MyShootActionEnum.TertiaryAction)
			{
				m_isPlayerShooting = true;
				base.Render.NeedsDrawFromParent = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// This will be called back from the sync object both on local and remote clients
		/// </summary>
		[Event(null, 2683)]
=======
		[Event(null, 3815)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnEndShoot(MyShootActionEnum action)
		{
			UpdateShooting(shouldShoot: false);
			m_isPlayerShooting = false;
			base.Render.NeedsDrawFromParent = false;
			if (Sync.IsServer)
			{
				m_isShooting.Value = false;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public void Use()
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.sync_ControlledEntity_Used);
		}

		public void UseContinues()
		{
		}

		public void UseFinished()
		{
		}

		public void PickUp()
		{
		}

		public void PickUpContinues()
		{
		}

		public void PickUpFinished()
		{
		}

		public void Sprint(bool enabled)
		{
		}

		public void Jump(Vector3 moveIndicator)
		{
		}

		public void SwitchWalk()
		{
		}

		public void Up()
		{
		}

		public void Crouch()
		{
		}

		public void Down()
		{
		}

		public void SwitchBroadcasting()
		{
		}

		public void ShowInventory()
		{
			MyCharacter user = m_turretController.GetUser();
			if (user != null)
			{
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, this);
			}
		}

		public void ShowTerminal()
		{
			MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalCharacter, this);
		}

		public void SwitchThrusts()
		{
		}

		public void SwitchDamping()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchDamping();
			}
		}

		public void SwitchLights()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchLights();
			}
		}

		public void SwitchLandingGears()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchLandingGears();
			}
		}

		public void SwitchHandbrake()
		{
<<<<<<< HEAD
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
=======
			MyShipController myShipController = PreviousControlledEntity as MyShipController;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchHandbrake();
			}
		}

		public void SwitchToWeapon(MyDefinitionId weaponDefinition)
		{
		}

		public void SwitchToWeapon(MyToolbarItemWeapon weapon)
		{
		}

		public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition)
		{
			return false;
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
		}

		public void SwitchReactors()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchReactors();
			}
		}

		public void SwitchReactorsLocal()
		{
			MyShipController myShipController;
			if ((myShipController = m_turretController.PreviousControlledEntity as MyShipController) != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchReactorsLocal();
			}
		}

		public void SwitchHelmet()
		{
		}

		public void Die()
		{
		}

		protected void DrawLasers()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated || m_barrel == null || !MyFakes.ENABLE_TURRET_LASERS)
			{
				return;
			}
			MyGunStatusEnum status = MyGunStatusEnum.Cooldown;
			if (!base.IsWorking)
			{
				return;
			}
			Vector4 color = Color.Green.ToVector4();
			switch (GetStatus())
			{
			case MyLargeShipGunStatus.MyWeaponStatus_Deactivated:
			case MyLargeShipGunStatus.MyWeaponStatus_Searching:
				color = Color.Green.ToVector4();
				break;
			case MyLargeShipGunStatus.MyWeaponStatus_Shooting:
			case MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying:
				color = Color.Red.ToVector4();
				break;
			}
			MyStringId iD_WEAPON_LASER = ID_WEAPON_LASER;
			float num = 0.1f;
			Vector3D zero = Vector3D.Zero;
			Vector3D zero2 = Vector3D.Zero;
			color *= 0.5f;
			if (Target != null && !m_targetingSystem.IsPotentialTarget)
			{
				if (!CanShoot(out status))
				{
					color = 0.3f * Color.DarkRed.ToVector4();
				}
				zero = m_gunBase.GetMuzzleWorldMatrix().Translation + 2.0 * m_gunBase.GetMuzzleWorldMatrix().Forward;
				MatrixD muzzleWorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				Vector3D translation = muzzleWorldMatrix.Translation;
				Vector3D vector3D = translation + muzzleWorldMatrix.Forward * m_searchingRange;
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(translation, vector3D);
				if (!hitInfo.HasValue)
				{
					m_hitPosition = vector3D;
				}
				else
				{
					m_hitPosition = hitInfo.Value.Position;
				}
				zero2 = m_hitPosition;
			}
			else
			{
				MatrixD muzzleWorldMatrix2 = m_gunBase.GetMuzzleWorldMatrix();
				m_hitPosition = muzzleWorldMatrix2.Translation + muzzleWorldMatrix2.Forward * m_laserLength;
				zero = m_barrel.Entity.PositionComp.GetPosition();
				zero2 = m_hitPosition;
			}
			Vector3D point = MySector.MainCamera.Position;
			Vector3D closestPointOnLine = MyUtils.GetClosestPointOnLine(ref zero, ref zero2, ref point);
			float val = (float)MySector.MainCamera.GetDistanceFromPoint(closestPointOnLine);
			num *= Math.Min(val, 10f) * 0.05f;
			MySimpleObjectDraw.DrawLine(zero, zero2, iD_WEAPON_LASER, ref color, num);
		}

		public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
<<<<<<< HEAD
		{
			base.Hierarchy.GetChildrenRecursive(m_children);
			foreach (MyEntity child in m_children)
=======
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			base.Hierarchy.GetChildrenRecursive(m_children);
			Enumerator<VRage.ModAPI.IMyEntity> enumerator = m_children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyEntity myEntity = (MyEntity)enumerator.get_Current();
					MyModel model = myEntity.Model;
					if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB(myEntity, ref aabb))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return base.Model?.GetTrianglePruningStructure().GetIntersectionWithAABB(this, ref aabb) ?? false;
		}

		private void OnTargetFlagChanged()
		{
			MyTurretTargetFlags myTurretTargetFlags = TargetFlags & ~MyTurretTargetFlags.NotNeutrals;
			m_workingFlagCombination = myTurretTargetFlags != (MyTurretTargetFlags)0;
			MyEntity nearestTarget = m_target;
			double minDistanceSq = 0.0;
			bool foundDecoy = false;
			using (MyUtils.ReuseCollection(ref m_allPotentialTargets))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyModel model = child.Model;
				if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB(child, ref aabb))
				{
					return true;
				}
			}
			return base.Model?.GetTrianglePruningStructure().GetIntersectionWithAABB(this, ref aabb) ?? false;
		}

		private void OnTargetFlagChanged()
		{
			MyTurretTargetFlags myTurretTargetFlags = TargetFlags & ~MyTurretTargetFlags.NotNeutrals;
			m_workingFlagCombination = myTurretTargetFlags != (MyTurretTargetFlags)0;
			m_targetingSystem.OnTargetFlagChanged();
		}

		public void SwitchAmmoMagazine()
		{
			m_gunBase.SwitchAmmoMagazineToNextAvailable();
		}

		public bool CanSwitchAmmoMagazine()
		{
			return false;
		}

<<<<<<< HEAD
=======
		private float NormalizeRange(float value)
		{
			if (value == 0f)
			{
				return 0f;
			}
			return MathHelper.Clamp((value - m_minRangeMeter) / (m_maxRangeMeter - m_minRangeMeter), 0f, 1f);
		}

		private float DenormalizeRange(float value)
		{
			if (value == 0f)
			{
				return 0f;
			}
			return MathHelper.Clamp(m_minRangeMeter + value * (m_maxRangeMeter - m_minRangeMeter), m_minRangeMeter, m_maxRangeMeter);
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void TakeControlFromTerminal()
		{
			m_targetingSystem.CheckOtherTargets = false;
			if (Sync.IsServer)
			{
				m_rotationAndElevationSync.Value = new SyncRotationAndElevation
				{
					Rotation = Rotation,
					Elevation = Elevation
				};
			}
		}

		private void ForceTarget(MyEntity entity, bool usePrediction)
		{
			m_targetingSystem.ForceTarget(entity, usePrediction);
		}

		private void SetTargetPositionInternal(Vector3D pos, Vector3 velocity, bool usePrediction)
		{
			m_targetingSystem.SetTargetPosition(pos, velocity, usePrediction);
		}

		public void ChangeIdleRotation(bool enable)
		{
			EnableIdleRotation = enable;
		}

<<<<<<< HEAD
		[Event(null, 3077)]
=======
		[Event(null, 4260)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void ResetTargetParams()
		{
			m_targetingSystem.ResetTargetParams();
			EnableIdleRotation = BlockDefinition.IdleRotation;
		}

		public void SetManualAzimuth(float value)
		{
			if (Rotation != value)
			{
				Rotation = value;
				m_targetingSystem.CheckOtherTargets = false;
				m_targetingSystem.ResetTarget();
				RotateModels();
			}
		}

		public void SetManualElevation(float value)
		{
			if (Elevation != value)
			{
				Elevation = value;
				m_targetingSystem.CheckOtherTargets = false;
				m_targetingSystem.ResetTarget();
				RotateModels();
			}
		}

		public void SetManualAzimuthAndElevation(float azimuth, float elevation)
		{
			bool flag = false;
			if (Rotation != azimuth)
			{
				Rotation = azimuth;
				flag = true;
			}
			if (Elevation != elevation)
			{
				Elevation = elevation;
				flag = true;
			}
			if (flag)
			{
				m_targetingSystem.CheckOtherTargets = false;
				m_targetingSystem.ResetTarget();
				RotateModels();
			}
		}

		public bool IsInRange(ref Vector3 lookAtPositionEuler)
		{
			float y = lookAtPositionEuler.Y;
			float x = lookAtPositionEuler.X;
			if (y > m_minAzimuthRadians && y < m_maxAzimuthRadians && x > m_minElevationRadians)
			{
				return x < m_maxElevationRadians;
			}
			return false;
		}

		public override bool CanOperate()
		{
			return CheckIsWorking();
		}

		public override void ShootFromTerminal(Vector3 direction)
		{
			base.ShootFromTerminal(direction);
			if (m_barrel != null)
			{
				if ((!m_isShooting && !m_forceShoot) || (ControllerInfo != null && ControllerInfo.Controller != null))
				{
					m_barrel.DontTimeOffsetNextShot();
				}
				if (!m_readyToShoot)
				{
					m_isShooting.SetLocalValue(newValue: true);
					base.UpdateAfterSimulation();
					m_isShooting.SetLocalValue(newValue: false);
				}
				m_barrel.StartShooting();
				m_targetingSystem.CheckOtherTargets = true;
			}
		}

		public override void StopShootFromTerminal()
		{
			if (m_barrel != null)
			{
				m_barrel.StopShooting();
			}
		}

		public override void SyncRotationAndOrientation()
		{
			m_rotationAndElevationSync.Value = new SyncRotationAndElevation
			{
				Rotation = Rotation,
				Elevation = Elevation
			};
		}

		protected override void RememberIdle()
		{
			m_previousIdleRotationState = EnableIdleRotation;
		}

		protected override void RestoreIdle()
		{
			EnableIdleRotation = m_previousIdleRotationState;
		}

<<<<<<< HEAD
		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return MyEntityExtensions.GetInventory(this, index);
		}

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public override void OnAddedToScene(object source)
=======
		[Event(null, 4340)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void RequestUseMessage(UseActionEnum useAction, long usedById)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				m_replicableServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
				m_blocksReplicable = MyExternalReplicable.FindByObject(this);
			}
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Combine(cubeGrid.IsPreviewChanged, new Action<bool>(PreviewChangedCallback));
				PreviewChangedCallback(base.CubeGrid.IsPreview);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Remove(cubeGrid.IsPreviewChanged, new Action<bool>(PreviewChangedCallback));
			}
			base.OnRemovedFromScene(source);
		}

<<<<<<< HEAD
		private void PreviewChangedCallback(bool isPreview)
		{
			if (Barrel != null)
			{
				base.IsPreview = isPreview;
				Barrel.IsPreviewChanged(isPreview);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.OnShootMissile, builder);
		}

		[Event(null, 3278)]
=======
		[Event(null, 4362)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Static.Add(builder);
			if (GunBase != null)
			{
				GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.BeforeShoot, Barrel);
				GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot, Barrel);
				PlayShootingSound();
				GunBase.MoveToNextMuzzle(GunBase.WeaponProperties.AmmoDefinition.AmmoType);
			}
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.OnRemoveMissile, entityId);
		}

		[Event(null, 3299)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Static.Remove(entityId);
		}

		public void AddPropertiesChangedCallback(Action<MyTerminalBlock> callback)
		{
			base.PropertiesChanged += callback;
		}

		public void RemovePropertiesChangedCallback(Action<MyTerminalBlock> callback)
		{
			base.PropertiesChanged -= callback;
		}

		public override void DisableUpdates()
		{
			base.DisableUpdates();
			m_parallelFlag.Disable(this);
		}

		public override void DoUpdateTimerTick()
		{
			base.DoUpdateTimerTick();
			if (base.Render.IsVisible() && base.IsWorking && Enabled)
			{
				if (!MySession.Static.CreativeMode && !HasEnoughAmmo())
				{
					m_gunBase.SwitchAmmoMagazineToNextAvailable();
				}
				m_isInRandomRotationDistance = false;
				if (m_barrel != null && !m_turretController.IsPlayerControlled && AiEnabled)
				{
					UpdateAiWeapon();
					m_isInRandomRotationDistance = MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) <= 600.0;
					if (m_isInRandomRotationDistance)
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					}
				}
				m_targetingSystem.UpdateVisibilityCache();
				if (m_turretController.IsControlled)
				{
					if (!m_turretController.IsInRangeAndPlayerHasAccess())
					{
						ReleaseControl();
						if (MyGuiScreenTerminal.IsOpen && MyGuiScreenTerminal.InteractedEntity == this)
						{
							MyGuiScreenTerminal.Hide();
						}
					}
					else
					{
						m_turretController.GetFirstRadioReceiver()?.UpdateHud(showMyself: true);
						if (m_turretController.IsControlledByLocalPlayer && MyGuiScreenHudSpace.Static != null)
						{
							MyGuiScreenHudSpace.Static.SetToolbarVisible(visible: false);
						}
					}
				}
				else if ((long)m_lockedTarget != 0L)
				{
					MyEntity entityById = MyEntities.GetEntityById(m_lockedTarget.Value);
					MyCubeGrid entity;
					if (entityById != null && !m_targetingSystem.CheckForcedTarget(entityById) && (entity = entityById as MyCubeGrid) != null)
					{
						m_targetingSystem.ForceGridTarget(entity, m_forcedTargetRange);
					}
					if (Sync.IsServer && !m_targetingSystem.TargetPrediction.IsTargetPositionManual && m_workingFlagCombination && (MySession.Static.CreativeMode || HasEnoughAmmo()))
					{
						m_targetingSystem.CheckAndSelectNearTargetsParallel();
					}
					if ((GetStatus() == MyLargeShipGunStatus.MyWeaponStatus_Deactivated && m_randomIsMoving) || (Target != null && m_targetingSystem.IsPotentialTarget))
					{
						SetupSearchRaycast();
					}
				}
				else
				{
					if (Sync.IsServer && !m_targetingSystem.TargetPrediction.IsTargetPositionManual && m_workingFlagCombination && (MySession.Static.CreativeMode || HasEnoughAmmo()))
					{
						m_targetingSystem.CheckAndSelectNearTargetsParallel();
					}
					if ((GetStatus() == MyLargeShipGunStatus.MyWeaponStatus_Deactivated && m_randomIsMoving) || (Target != null && m_targetingSystem.IsPotentialTarget))
					{
						SetupSearchRaycast();
					}
				}
				if (m_playAimingSound)
				{
					PlayAimingSound();
				}
				else
				{
					StopAimingSound();
				}
			}
			m_isMoving = false;
			if (base.CubeGrid?.Physics != null)
			{
				MatrixD worldMatrix = base.CubeGrid.WorldMatrix;
				if (!worldMatrix.EqualsFast(ref m_lastTestedGridWM))
				{
					m_isMoving = true;
					m_lastTestedGridWM = worldMatrix;
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

<<<<<<< HEAD
		private void OnStopAI()
=======
		[Event(null, 4392)]
		[Reliable]
		[Client]
		private void UseFailureCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (m_soundEmitter != null)
			{
				if (m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.StopSound(forced: true);
				}
				if (m_soundEmitterForRotation.IsPlaying)
				{
					m_soundEmitterForRotation.StopSound(forced: true);
				}
			}
		}

		public override bool GetTimerEnabledState()
		{
			return Enabled;
		}

		protected override void TiersChanged()
		{
			MyUpdateTiersPlayerPresence playerPresenceTier = base.CubeGrid.PlayerPresenceTier;
			MyUpdateTiersGridPresence gridPresenceTier = base.CubeGrid.GridPresenceTier;
			if (playerPresenceTier == MyUpdateTiersPlayerPresence.Normal || gridPresenceTier == MyUpdateTiersGridPresence.Normal)
			{
				ChangeTimerTick(GetTimerTime(0));
			}
			else if (playerPresenceTier == MyUpdateTiersPlayerPresence.Tier1 || playerPresenceTier == MyUpdateTiersPlayerPresence.Tier2 || gridPresenceTier == MyUpdateTiersGridPresence.Tier1)
			{
				ChangeTimerTick(GetTimerTime(1));
			}
		}

		protected override uint GetDefaultTimeForUpdateTimer(int index)
		{
			switch (index)
			{
<<<<<<< HEAD
			case 0:
				return TIMER_NORMAL_IN_FRAMES;
			case 1:
				return TIMER_TIER1_IN_FRAMES;
			default:
				return 0u;
=======
				m_replicableServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
				m_blocksReplicable = MyExternalReplicable.FindByObject(this);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Combine(cubeGrid.IsPreviewChanged, new Action<bool>(PreviewChangedCallback));
				PreviewChangedCallback(base.CubeGrid.IsPreview);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Remove(cubeGrid.IsPreviewChanged, new Action<bool>(PreviewChangedCallback));
			}
			base.OnRemovedFromScene(source);
		}

		private void PreviewChangedCallback(bool isPreview)
		{
			if (Barrel != null)
			{
				Barrel.IsPreviewChanged(isPreview);
			}
		}

		public Vector3D GetMuzzlePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition();
		}

		public bool IsTargetLockingEnabled()
		{
			return TargetLocking;
		}

		public void SetLockedTarget(VRage.Game.ModAPI.IMyCubeGrid target)
		{
		}

<<<<<<< HEAD
		public MatrixD GetWorldMatrix()
=======
		[Event(null, 4497)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			return Barrel.Entity.WorldMatrix;
		}

		public bool CanActiveToolShoot()
		{
			return true;
		}

<<<<<<< HEAD
		public bool IsShipToolSelected()
		{
			return false;
		}

		public Vector3D GetActiveToolPosition()
		{
			return GetMuzzlePosition();
		}

		public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long targetIidentityId)
=======
		[Event(null, 4510)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			return GetUserRelationToOwner(targetIidentityId, MyRelationsBetweenPlayerAndBlock.NoOwnership);
		}

		public bool IsTurretTerminalVisible()
		{
			return true;
		}

		public bool IsToolbarUsable()
		{
			return false;
		}

		public override void DisableUpdates()
		{
			base.DisableUpdates();
			m_parallelFlag.Disable(this);
		}

		public override void DoUpdateTimerTick()
		{
			base.DoUpdateTimerTick();
			if (base.Render.IsVisible() && base.IsWorking && base.Enabled)
			{
				if (!MySession.Static.CreativeMode && !HasEnoughAmmo())
				{
					m_gunBase.SwitchAmmoMagazineToNextAvailable();
				}
				m_isInRandomRotationDistance = false;
				if (m_barrel != null && !IsPlayerControlled && AiEnabled)
				{
					UpdateAiWeapon();
					m_isInRandomRotationDistance = MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) <= 600.0;
					if (m_isInRandomRotationDistance)
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					}
				}
				m_notVisibleTargetsUpdatesSinceRefresh++;
				if (!m_parallelTargetSelectionInProcess)
				{
					UpdateVisibilityCacheCounters();
				}
				if (IsControlled)
				{
					if (!IsInRangeAndPlayerHasAccess())
					{
						ReleaseControl();
						if (MyGuiScreenTerminal.IsOpen && MyGuiScreenTerminal.InteractedEntity == this)
						{
							MyGuiScreenTerminal.Hide();
						}
					}
					else
					{
						GetFirstRadioReceiver()?.UpdateHud(showMyself: true);
					}
				}
				else
				{
					if (Sync.IsServer && !m_currentPrediction.ManualTargetPosition && m_workingFlagCombination && (MySession.Static.CreativeMode || HasEnoughAmmo()))
					{
						CheckAndSelectNearTargetsParallel();
					}
					if ((GetStatus() == MyLargeShipGunStatus.MyWeaponStatus_Deactivated && m_randomIsMoving) || (Target != null && m_isPotentialTarget))
					{
						SetupSearchRaycast();
					}
				}
				if (m_playAimingSound)
				{
					PlayAimingSound();
				}
				else
				{
					StopAimingSound();
				}
			}
			m_isMoving = false;
			if (base.CubeGrid?.Physics != null)
			{
				MatrixD worldMatrix = base.CubeGrid.WorldMatrix;
				if (!worldMatrix.EqualsFast(ref m_lastTestedGridWM))
				{
					m_isMoving = true;
					m_lastTestedGridWM = worldMatrix;
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		public override bool GetTimerEnabledState()
		{
			return base.Enabled;
		}

		protected override void TiersChanged()
		{
			MyUpdateTiersPlayerPresence playerPresenceTier = base.CubeGrid.PlayerPresenceTier;
			MyUpdateTiersGridPresence gridPresenceTier = base.CubeGrid.GridPresenceTier;
			if (playerPresenceTier == MyUpdateTiersPlayerPresence.Normal || gridPresenceTier == MyUpdateTiersGridPresence.Normal)
			{
				ChangeTimerTick(GetTimerTime(0));
			}
			else if (playerPresenceTier == MyUpdateTiersPlayerPresence.Tier1 || playerPresenceTier == MyUpdateTiersPlayerPresence.Tier2 || gridPresenceTier == MyUpdateTiersGridPresence.Tier1)
			{
				ChangeTimerTick(GetTimerTime(1));
			}
		}

		protected override uint GetDefaultTimeForUpdateTimer(int index)
		{
			return index switch
			{
				0 => TIMER_NORMAL_IN_FRAMES, 
				1 => TIMER_TIER1_IN_FRAMES, 
				_ => 0u, 
			};
		}

		public Vector3D GetMuzzlePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition();
		}

		void Sandbox.ModAPI.IMyLargeTurretBase.TrackTarget(VRage.ModAPI.IMyEntity entity)
		{
			if (entity != null)
			{
				MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.SetTargetRequest, entity.EntityId, arg3: true);
			}
		}

		[Event(null, 41)]
		[Reliable]
		[Server]
		[Broadcast]
		private void SetTargetRequest(long entityId, bool usePrediction)
		{
			MyEntity entity = null;
			if (entityId != 0L)
			{
				MyEntities.TryGetEntityById(entityId, out entity);
			}
			ForceTarget(entity, usePrediction);
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.TrackTarget(Vector3D pos, Vector3 velocity)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.SetTargetPosition, pos, velocity, arg4: true);
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SetTarget(Vector3D pos)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.SetTargetPosition, pos, Vector3.Zero, arg4: false);
		}

		[Event(null, 63)]
		[Reliable]
		[Server]
		[Broadcast]
		private void SetTargetPosition(Vector3D targetPos, Vector3 velocity, bool usePrediction)
		{
			SetTargetPositionInternal(targetPos, velocity, usePrediction);
		}

		void Sandbox.ModAPI.IMyLargeTurretBase.SetTarget(VRage.ModAPI.IMyEntity entity)
		{
			if (entity != null)
			{
				MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.SetTargetRequest, entity.EntityId, arg3: false);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.ResetTargetingToDefault()
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeTurretBase x) => x.ResetTargetParams);
		}

		MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyLargeTurretBase.GetTargetedEntity()
		{
			return MyDetectedEntityInfoHelper.Create(Target, base.OwnerId, Target?.PositionComp.WorldAABB.Center);
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SyncEnableIdleRotation()
		{
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SyncAzimuth()
		{
			SyncRotationAndOrientation();
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SyncElevation()
		{
			SyncRotationAndOrientation();
		}

		void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SetManualAzimuthAndElevation(float azimuth, float elevation)
		{
			SetManualAzimuthAndElevation(azimuth, elevation);
		}

		public MyEntityCameraSettings GetCameraEntitySettings()
		{
			return null;
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		public bool ShouldEndShootingOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public List<string> GetTargetingGroups()
		{
			List<MyTargetingGroupDefinition> targetingGroupDefinitions = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			List<string> list = new List<string>();
			foreach (MyTargetingGroupDefinition item in targetingGroupDefinitions)
			{
				list.Add(item.Id.SubtypeName);
			}
			return list;
		}

		public MyStringHash GetTargetingGroupHash()
		{
			return m_targetingGroup.Value;
		}

		public string GetTargetingGroup()
		{
			string @string = m_targetingGroup.Value.String;
			if (string.IsNullOrEmpty(@string))
			{
				return null;
			}
			return @string;
		}

		public void SetTargetingGroup(string groupSubtypeId)
		{
			m_targetingGroup.Value = MyStringHash.Get(groupSubtypeId);
		}

		public MatrixD? GetOverridingFocusMatrix()
		{
			return null;
		}
	}
}
