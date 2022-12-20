using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[StaticEventOwner]
<<<<<<< HEAD
	public class MyGridJumpDriveSystem : MyUpdateableGridSystem, IMyGridJumpDriveSystem
=======
	public class MyGridJumpDriveSystem : MyUpdateableGridSystem
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		public enum MyJumpFailReason
		{
			None,
			Static,
			Locked,
			ShortDistance,
			AlreadyJumping,
			NoLocation,
			Other
		}

		protected sealed class OnJumpRequested_003C_003ESystem_Int64_0023VRageMath_Vector3D_0023System_Int64_0023System_Single : ICallSite<IMyEventOwner, long, Vector3D, long, float, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3D jumpTarget, in long userId, in float jumpDriveDelay, in DBNull arg5, in DBNull arg6)
			{
				OnJumpRequested(entityId, jumpTarget, userId, jumpDriveDelay);
			}
		}

		protected sealed class OnJumpSuccess_003C_003ESystem_Int64_0023VRageMath_Vector3D_0023System_Int64_0023System_Single : ICallSite<IMyEventOwner, long, Vector3D, long, float, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3D jumpTarget, in long userId, in float jumpDriveDelay, in DBNull arg5, in DBNull arg6)
			{
				OnJumpSuccess(entityId, jumpTarget, userId, jumpDriveDelay);
			}
		}

		protected sealed class OnJumpFailure_003C_003ESystem_Int64_0023Sandbox_Game_GameSystems_MyGridJumpDriveSystem_003C_003EMyJumpFailReason : ICallSite<IMyEventOwner, long, MyJumpFailReason, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in MyJumpFailReason reason, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnJumpFailure(entityId, reason);
			}
		}

		protected sealed class OnPerformJump_003C_003ESystem_Int64_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, long, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3D jumpTarget, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPerformJump(entityId, jumpTarget);
			}
		}

		protected sealed class OnAbortJump_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnAbortJump(entityId);
			}
		}

		public const float JUMP_DRIVE_DELAY = 10f;

<<<<<<< HEAD
=======
		public const double MIN_JUMP_DISTANCE = 5000.0;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private HashSet<MyJumpDrive> m_jumpDrives = new HashSet<MyJumpDrive>();

		private List<MyEntity> m_entitiesInRange = new List<MyEntity>();

		private List<MyObjectSeed> m_objectsInRange = new List<MyObjectSeed>();

		private List<BoundingBoxD> m_obstaclesInRange = new List<BoundingBoxD>();

		private List<MyCharacter> m_characters = new List<MyCharacter>();

		private Vector3D m_selectedDestination;

		private Vector3D m_jumpDirection;

		private Vector3D m_jumpDirectionNorm;

		private Vector3 m_effectOffset = Vector3.Zero;

		private bool m_isJumping;

		private float m_prevJumpTime;

		private bool m_jumped;

		private long m_userId;

		private float m_jumpTimeLeft;

		private bool m_playEffect;

		private Vector3D? m_savedJumpDirection;

		private float? m_savedRemainingJumpTime;

		private MySoundPair m_chargingSound = new MySoundPair("ShipJumpDriveCharging");

		private MySoundPair m_jumpInSound = new MySoundPair("ShipJumpDriveJumpIn");

		private MySoundPair m_jumpOutSound = new MySoundPair("ShipJumpDriveJumpOut");

		protected MyEntity3DSoundEmitter m_soundEmitter;

		private MyEntity3DSoundEmitter m_soundEmitterJumpIn;

		private MyParticleEffect m_effect;

		public bool IsJumping => m_isJumping;

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

		public override int UpdatePriority => 5;

<<<<<<< HEAD
		bool IMyGridJumpDriveSystem.Jumped => m_jumped;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGridJumpDriveSystem(MyCubeGrid grid)
			: base(grid)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(base.Grid);
				m_soundEmitterJumpIn = new MyEntity3DSoundEmitter(base.Grid);
			}
		}

		public void Init(Vector3D? jumpDriveDirection, float? remainingTimeForJump)
		{
			m_savedJumpDirection = jumpDriveDirection;
			m_savedRemainingJumpTime = remainingTimeForJump;
			if (jumpDriveDirection.HasValue)
			{
				Schedule();
			}
		}

		public Vector3D? GetJumpDriveDirection()
		{
			if (m_isJumping && !m_jumped)
			{
				return m_jumpDirection;
			}
			return null;
		}

		internal float? GetRemainingJumpTime()
		{
			if (m_isJumping && !m_jumped)
			{
				return m_jumpTimeLeft;
			}
			return null;
		}

		public void RegisterJumpDrive(MyJumpDrive jumpDrive)
		{
			m_jumpDrives.Add(jumpDrive);
		}

		public void UnregisterJumpDrive(MyJumpDrive jumpDrive)
		{
			m_jumpDrives.Remove(jumpDrive);
			MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void Update()
		{
			if (m_savedJumpDirection.HasValue)
			{
				m_selectedDestination = m_savedJumpDirection.Value;
				m_isJumping = true;
				m_jumped = false;
				m_jumpTimeLeft = (m_savedRemainingJumpTime.HasValue ? m_savedRemainingJumpTime.Value : 0f);
				m_savedJumpDirection = null;
				m_savedRemainingJumpTime = null;
			}
			UpdateJumpDriveSystem();
		}

		public double GetMinJumpDistance(long userId)
		{
			double num = double.MaxValue;
			foreach (MyJumpDrive jumpDrife in m_jumpDrives)
			{
				if (jumpDrife.CanJumpIfFullAndHasAccess(userId) && jumpDrife.BlockDefinition.MinJumpDistance < num)
				{
					num = jumpDrife.BlockDefinition.MinJumpDistance;
				}
			}
			if (num == double.MaxValue)
			{
				num = 0.0;
			}
			return num;
		}

		public double GetMaxJumpDistance(long userId)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			double num = 0.0;
			double num2 = 0.0;
			double num3 = base.Grid.GetCurrentMass();
<<<<<<< HEAD
			foreach (MyJumpDrive jumpDrife in m_jumpDrives)
=======
			Enumerator<MyJumpDrive> enumerator = m_jumpDrives.GetEnumerator();
			try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				while (enumerator.MoveNext())
				{
					MyJumpDrive current = enumerator.get_Current();
					if (current.CanJumpAndHasAccess(userId))
					{
						num += current.BlockDefinition.MaxJumpDistance;
						num2 += current.BlockDefinition.MaxJumpDistance * (current.BlockDefinition.MaxJumpMass / num3);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return Math.Min(num, num2);
		}

		private void DepleteJumpDrives(double distance, long userId)
		{
<<<<<<< HEAD
			double num = base.Grid.GetCurrentMass();
			foreach (MyJumpDrive jumpDrife in m_jumpDrives)
=======
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			double num = base.Grid.GetCurrentMass();
			Enumerator<MyJumpDrive> enumerator = m_jumpDrives.GetEnumerator();
			try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				while (enumerator.MoveNext())
				{
					MyJumpDrive current = enumerator.get_Current();
					if (current.CanJumpAndHasAccess(userId))
					{
						current.IsJumping = true;
						double num2 = current.BlockDefinition.MaxJumpMass / num;
						if (num2 > 1.0)
						{
							num2 = 1.0;
						}
						double num3 = current.BlockDefinition.MaxJumpDistance * num2;
						if (!(num3 < distance))
						{
							double num4 = distance / num3;
							current.SetStoredPower(1f - (float)num4);
							break;
						}
						distance -= num3;
						current.SetStoredPower(0f);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private bool IsJumpValid(long userId, out MyJumpFailReason reason)
		{
			reason = MyJumpFailReason.None;
			if (MyFakes.TESTING_JUMPDRIVE)
			{
				return true;
			}
			if (base.Grid.MarkedForClose)
			{
				reason = MyJumpFailReason.Other;
				return false;
			}
			if (!base.Grid.CanBeTeleported(this, out reason))
			{
				return false;
			}
			if (GetMaxJumpDistance(userId) < GetMinJumpDistance(userId))
			{
				reason = MyJumpFailReason.ShortDistance;
				return false;
			}
			return true;
		}

		public void RequestAbort()
		{
			if (m_isJumping && !m_jumped)
			{
				SendAbortJump();
			}
		}

		public void RequestJump(string destinationName, Vector3D destination, long userId, BoundingBoxD? shipBox = null, float jumpDriveDelay = 10f)
		{
			if (!Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.Grid.WorldMatrix.Translation)))
			{
				MyHudNotification notification = new MyHudNotification(MySpaceTexts.NotificationCannotJumpFromGravity, 1500);
				MyHud.Notifications.Add(notification);
				return;
			}
			if (!Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(destination)) || (shipBox.HasValue && MyGravityProviderSystem.GetOverlappingGravityProviders(shipBox.Value, new List<IMyGravityProvider>()).Count > 0))
			{
				MyHudNotification notification2 = new MyHudNotification(MySpaceTexts.NotificationCannotJumpIntoGravity, 1500);
				MyHud.Notifications.Add(notification2);
				return;
			}
			if (!IsJumpValid(userId, out var reason))
			{
				ShowNotification(reason);
				return;
			}
			if (MySession.Static.Settings.WorldSizeKm > 0 && destination.Length() > (double)(MySession.Static.Settings.WorldSizeKm * 500))
			{
				MyHudNotification notification3 = new MyHudNotification(MySpaceTexts.NotificationCannotJumpOutsideWorld, 1500);
				MyHud.Notifications.Add(notification3);
				return;
			}
			m_selectedDestination = destination;
			double maxJumpDistance = GetMaxJumpDistance(userId);
<<<<<<< HEAD
			double minJumpDistance = GetMinJumpDistance(userId);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_jumpDirection = destination - base.Grid.WorldMatrix.Translation;
			Vector3D.Normalize(ref m_jumpDirection, out m_jumpDirectionNorm);
			double num = m_jumpDirection.Length();
			double num2 = num;
			if (num > maxJumpDistance)
			{
				double num3 = maxJumpDistance / num;
				num2 = maxJumpDistance;
				m_jumpDirection *= num3;
			}
<<<<<<< HEAD
			Vector3D jumpDirectionNorm = m_jumpDirectionNorm;
			Vector3D linePointA = base.Grid.WorldMatrix.Translation;
			LineD line = new LineD(linePointA, destination);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Grid, null, ignoreChildren: true, ignoreFloatingObjects: true, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: false, ignoreSubgridsOfIgnoredEntities: true);
=======
			Vector3D vector3D = Vector3D.Normalize(destination - base.Grid.WorldMatrix.Translation);
			Vector3D linePointA = base.Grid.WorldMatrix.Translation + base.Grid.PositionComp.LocalAABB.Extents.Max() * vector3D;
			LineD line = new LineD(linePointA, destination);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Grid, null, ignoreChildren: true, ignoreFloatingObjects: true, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			_ = Vector3D.Zero;
			_ = Vector3D.Zero;
			if (intersectionWithLine.HasValue)
			{
				MyEntity myEntity = intersectionWithLine.Value.Entity as MyEntity;
				Vector3D point = myEntity.WorldMatrix.Translation;
				Vector3D closestPointOnLine = MyUtils.GetClosestPointOnLine(ref linePointA, ref destination, ref point);
				if (intersectionWithLine.Value.Entity is MyPlanet)
				{
					MyHudNotification notification4 = new MyHudNotification(MySpaceTexts.NotificationCannotJumpIntoGravity, 1500);
					MyHud.Notifications.Add(notification4);
					return;
				}
<<<<<<< HEAD
				float num4 = myEntity.PositionComp.LocalAABB.HalfExtents.Length();
				destination = closestPointOnLine - jumpDirectionNorm * num4;
=======
				float num4 = myEntity.PositionComp.LocalAABB.Extents.Length();
				destination = closestPointOnLine - vector3D * (num4 + base.Grid.PositionComp.LocalAABB.HalfExtents.Length());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_selectedDestination = destination;
				m_jumpDirection = m_selectedDestination - linePointA;
				Vector3D.Normalize(ref m_jumpDirection, out m_jumpDirectionNorm);
				num2 = m_jumpDirection.Length();
			}
			if (num2 < minJumpDistance - 1E-06)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, GetWarningText(num2, minJumpDistance, intersectionWithLine.HasValue), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, GetConfirmationText(destinationName, num, num2, userId, intersectionWithLine.HasValue), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					reason = MyJumpFailReason.None;
					if (result == MyGuiScreenMessageBox.ResultEnum.YES && IsJumpValid(userId, out reason))
					{
						RequestJump(m_selectedDestination, userId, jumpDriveDelay);
					}
					else
					{
						SendAbortJump();
					}
				}, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: true, new Vector2(0.839375f, 0.3675f)));
			}
			if (MyFakes.TESTING_JUMPDRIVE)
			{
				m_jumpDirection *= 1000.0;
			}
		}

		private void ShowNotification(MyJumpFailReason reason)
		{
			if (!Sync.IsDedicated)
			{
				switch (reason)
				{
				case MyJumpFailReason.Static:
				{
					MyHudNotification notification6 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedStatic, 1500);
					MyHud.Notifications.Add(notification6);
					break;
				}
				case MyJumpFailReason.Locked:
				{
					MyHudNotification notification5 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedLocked, 1500);
					MyHud.Notifications.Add(notification5);
					break;
				}
				case MyJumpFailReason.NoLocation:
				{
					MyHudNotification notification4 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedNoLocation, 1500);
					MyHud.Notifications.Add(notification4);
					break;
				}
				case MyJumpFailReason.ShortDistance:
				{
					MyHudNotification notification3 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedShortDistance, 1500);
					MyHud.Notifications.Add(notification3);
					break;
				}
				case MyJumpFailReason.AlreadyJumping:
				{
					MyHudNotification notification2 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedAlreadyJumping, 1500);
					MyHud.Notifications.Add(notification2);
					break;
				}
				case MyJumpFailReason.Other:
				{
					MyHudNotification notification = new MyHudNotification(MySpaceTexts.NotificationJumpAborted, 1500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					MyHud.Notifications.Add(notification);
					break;
				}
				}
			}
		}

		private StringBuilder GetConfirmationText(string name, double distance, double actualDistance, long userId, bool obstacleDetected)
		{
			int count = m_jumpDrives.get_Count();
			int value = Enumerable.Count<MyJumpDrive>((IEnumerable<MyJumpDrive>)m_jumpDrives, (Func<MyJumpDrive, bool>)((MyJumpDrive x) => x.CanJumpAndHasAccess(userId)));
			distance /= 1000.0;
			actualDistance /= 1000.0;
			float num = (float)(actualDistance / distance);
			if (num > 1f)
			{
				num = 1f;
			}
			GetCharactersInBoundingBox(base.Grid.GetPhysicalGroupAABB(), m_characters);
			int num2 = 0;
			int num3 = 0;
			foreach (MyCharacter character in m_characters)
			{
				if (!character.IsDead)
				{
					num2++;
					if (character.Parent != null)
					{
						num3++;
					}
				}
			}
			m_characters.Clear();
			StringBuilder stringBuilder = new StringBuilder();
			string text = (obstacleDetected ? MyTexts.Get(MySpaceTexts.Jump_Obstacle).ToString() : "");
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_Destination)).Append(name).Append("\n");
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_Distance)).Append(distance.ToString("N")).Append(" km\n");
			stringBuilder.Append(MyTexts.Get(MySpaceTexts.Jump_Achievable).ToString() + text + ": ").Append(num.ToString("P")).Append(" (")
				.Append(actualDistance.ToString("N"))
				.Append(" km)\n");
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_Weight)).Append(base.Grid.GetCurrentMass().ToString("N")).Append(" kg\n");
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_DriveCount)).Append(value).Append("/")
				.Append(count)
				.Append("\n");
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_CrewCount)).Append(num3).Append("/")
				.Append(num2)
				.Append("\n");
			return stringBuilder;
		}

		private StringBuilder GetWarningText(double actualDistance, double minDistance, bool obstacleDetected)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (obstacleDetected)
			{
				stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_ObstacleTruncation)).Append(" \n");
			}
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_DistanceToDest)).Append(actualDistance.ToString("N")).Append(" m\n");
			stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_MinDistance)).Append(minDistance.ToString("N")).Append(" m\n");
			return stringBuilder;
		}

		private void GetCharactersInBoundingBox(BoundingBoxD boundingBox, List<MyCharacter> characters)
		{
			MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, m_entitiesInRange);
			foreach (MyEntity item in m_entitiesInRange)
			{
				MyCharacter myCharacter = item as MyCharacter;
				if (myCharacter != null)
				{
					characters.Add(myCharacter);
				}
			}
			m_entitiesInRange.Clear();
		}

		private Vector3D? FindSuitableJumpLocation(Vector3D desiredLocation)
		{
			BoundingBoxD physicalGroupAABB = base.Grid.GetPhysicalGroupAABB();
			physicalGroupAABB.Inflate(1000.0);
			BoundingBoxD box = physicalGroupAABB.GetInflated(physicalGroupAABB.HalfExtents * 10.0);
			box.Translate(desiredLocation - box.Center);
			MyProceduralWorldGenerator.Static.OverlapAllPlanetSeedsInSphere(new BoundingSphereD(box.Center, box.HalfExtents.AbsMax()), m_objectsInRange);
			Vector3D vector3D = desiredLocation;
			foreach (MyObjectSeed item2 in m_objectsInRange)
			{
				if (item2.BoundingVolume.Contains(vector3D) != 0)
				{
					Vector3D vector3D2 = vector3D - item2.BoundingVolume.Center;
					vector3D2.Normalize();
					vector3D2 *= item2.BoundingVolume.HalfExtents * 1.5;
					vector3D = item2.BoundingVolume.Center + vector3D2;
					break;
				}
			}
			m_objectsInRange.Clear();
			MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(box.Center, box.HalfExtents.AbsMax()), m_objectsInRange);
			foreach (MyObjectSeed item3 in m_objectsInRange)
			{
				m_obstaclesInRange.Add(item3.BoundingVolume);
			}
			m_objectsInRange.Clear();
			MyProceduralWorldGenerator.Static.GetAllInSphere<MyStationCellGenerator>(new BoundingSphereD(box.Center, box.HalfExtents.AbsMax()), m_objectsInRange);
			foreach (MyObjectSeed item4 in m_objectsInRange)
			{
				MyStation myStation = item4.UserData as MyStation;
				if (myStation != null)
				{
					BoundingBoxD item = new BoundingBoxD(myStation.Position - MyStation.SAFEZONE_SIZE, myStation.Position + MyStation.SAFEZONE_SIZE);
					if (item.Contains(vector3D) != 0)
					{
						m_obstaclesInRange.Add(item);
					}
				}
			}
			m_objectsInRange.Clear();
			MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, m_entitiesInRange);
			foreach (MyEntity item5 in m_entitiesInRange)
			{
				if (!(item5 is MyPlanet))
				{
					m_obstaclesInRange.Add(item5.PositionComp.WorldAABB.GetInflated(physicalGroupAABB.HalfExtents));
				}
			}
			int num = 10;
			int num2 = 0;
			BoundingBoxD? boundingBoxD = null;
			bool flag = false;
			bool flag2 = false;
			while (num2 < num)
			{
				num2++;
				flag = false;
				foreach (BoundingBoxD item6 in m_obstaclesInRange)
				{
					ContainmentType containmentType = item6.Contains(vector3D);
					if (containmentType == ContainmentType.Contains || containmentType == ContainmentType.Intersects)
					{
						if (!boundingBoxD.HasValue)
						{
							boundingBoxD = item6;
						}
						boundingBoxD = boundingBoxD.Value.Include(item6);
						boundingBoxD = boundingBoxD.Value.Inflate(1.0);
						vector3D = ClosestPointOnBounds(boundingBoxD.Value, vector3D);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					flag2 = true;
					break;
				}
			}
			m_obstaclesInRange.Clear();
			m_entitiesInRange.Clear();
			m_objectsInRange.Clear();
			if (flag2)
			{
				return vector3D;
			}
			return null;
		}

		private Vector3D ClosestPointOnBounds(BoundingBoxD b, Vector3D p)
		{
			Vector3D vector3D = (p - b.Center) / b.HalfExtents;
			switch (vector3D.AbsMaxComponent())
			{
			case 0:
				if (vector3D.X > 0.0)
				{
					p.X = b.Max.X;
				}
				else
				{
					p.X = b.Min.X;
				}
				break;
			case 1:
				if (vector3D.Y > 0.0)
				{
					p.Y = b.Max.Y;
				}
				else
				{
					p.Y = b.Min.Y;
				}
				break;
			case 2:
				if (vector3D.Z > 0.0)
				{
					p.Z = b.Max.Z;
				}
				else
				{
					p.Z = b.Min.Z;
				}
				break;
			}
			return p;
		}

		private bool IsLocalCharacterAffectedByJump(bool forceRecompute = false)
		{
			if (MySession.Static.LocalCharacter == null || !(MySession.Static.ControlledEntity is MyShipController))
			{
				m_playEffect = false;
				MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
				return false;
			}
			if (m_playEffect && !forceRecompute)
			{
				return true;
			}
			GetCharactersInBoundingBox(base.Grid.GetPhysicalGroupAABB(), m_characters);
			foreach (MyCharacter character in m_characters)
			{
				if (character == MySession.Static.LocalCharacter && character.Parent != null)
				{
					m_characters.Clear();
					m_playEffect = true;
					return true;
				}
			}
			m_characters.Clear();
			m_playEffect = false;
			return false;
		}

		private void Jump(Vector3D jumpTarget, long userId, float jumpDriveDelay = 10f)
		{
			double maxJumpDistance = GetMaxJumpDistance(userId);
			m_jumpDirection = jumpTarget - base.Grid.WorldMatrix.Translation;
			Vector3D.Normalize(ref m_jumpDirection, out m_jumpDirectionNorm);
			double num = m_jumpDirection.Length();
			if (num > maxJumpDistance)
			{
				double num2 = maxJumpDistance / num;
				m_jumpDirection *= num2;
			}
			m_selectedDestination = base.Grid.WorldMatrix.Translation + m_jumpDirection;
			m_isJumping = true;
			m_jumped = false;
<<<<<<< HEAD
			m_jumpTimeLeft = (MyFakes.TESTING_JUMPDRIVE ? 1f : jumpDriveDelay);
=======
			m_jumpTimeLeft = (MyFakes.TESTING_JUMPDRIVE ? 1f : 10f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.Grid.GridSystems.JumpSystem.m_jumpTimeLeft = m_jumpTimeLeft;
			m_soundEmitter?.PlaySound(m_chargingSound);
			m_prevJumpTime = 0f;
			m_userId = userId;
			Schedule();
		}

		private void UpdateJumpDriveSystem()
		{
			if (!m_isJumping)
			{
				return;
			}
			float jumpTimeLeft = m_jumpTimeLeft;
			if (m_effect == null)
			{
				PlayParticleEffect();
			}
			else
			{
				UpdateParticleEffect();
			}
			bool flag = false;
			foreach (MyJumpDrive jumpDrife in m_jumpDrives)
			{
				if (jumpDrife.CanJumpAndHasAccess(m_userId))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				SendAbortJump();
			}
			m_jumpTimeLeft -= 0.0166666675f;
			if (jumpTimeLeft > 0.4f)
			{
				double num = Math.Round(jumpTimeLeft);
				if (num != (double)m_prevJumpTime && IsLocalCharacterAffectedByJump(forceRecompute: true))
				{
					MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationJumpWarmupTime, 500, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 3);
					myHudNotification.SetTextFormatArguments(num);
					MyHud.Notifications.Add(myHudNotification);
				}
			}
			else if (jumpTimeLeft > 0f)
			{
				IsLocalCharacterAffectedByJump(forceRecompute: true);
				if (m_soundEmitter != null && m_soundEmitter.SoundId != m_jumpOutSound.Arcade && m_soundEmitter.SoundId != m_jumpOutSound.Realistic)
				{
					m_soundEmitter.PlaySound(m_jumpOutSound);
				}
				UpdateJumpEffect(jumpTimeLeft / 0.4f);
				if (!(jumpTimeLeft < 0.3f))
				{
				}
			}
			else if (!m_jumped)
			{
				if (Sync.IsServer)
				{
					Vector3D? vector3D = FindSuitableJumpLocation(m_selectedDestination);
					double maxJumpDistance = GetMaxJumpDistance(m_userId);
					MyJumpFailReason reason = MyJumpFailReason.None;
					if (vector3D.HasValue && m_jumpDirection.Length() <= maxJumpDistance && IsJumpValid(m_userId, out reason))
					{
						SendPerformJump(vector3D.Value);
						PerformJump(vector3D.Value);
					}
					else
					{
						SendAbortJump();
					}
				}
			}
			else if (jumpTimeLeft > -0.6f)
			{
				if (m_soundEmitterJumpIn != null && !m_soundEmitterJumpIn.IsPlaying)
				{
					m_soundEmitterJumpIn.PlaySound(m_jumpInSound);
				}
				UpdateJumpEffect(jumpTimeLeft / -0.6f);
			}
			else
			{
				CleanupAfterJump();
			}
			m_prevJumpTime = (float)Math.Round(jumpTimeLeft);
		}

		private void PlayParticleEffect()
		{
			if (m_effect == null)
			{
<<<<<<< HEAD
				MatrixD effectMatrix = MatrixD.CreateFromDir(-m_jumpDirectionNorm);
				m_effectOffset = m_jumpDirectionNorm * base.Grid.PositionComp.WorldAABB.HalfExtents.AbsMax() * 2.0;
				effectMatrix.Translation = base.Grid.PositionComp.WorldAABB.Center + m_effectOffset;
				Vector3D worldPosition = base.Grid.PositionComp.GetPosition();
				MyParticlesManager.TryCreateParticleEffect("Warp", ref effectMatrix, ref worldPosition, base.Grid.Render.ParentIDs[0], out m_effect);
=======
				MatrixD worldMatrix = MatrixD.CreateFromDir(-m_jumpDirectionNorm);
				m_effectOffset = m_jumpDirectionNorm * base.Grid.PositionComp.WorldAABB.HalfExtents.AbsMax() * 2.0;
				worldMatrix.Translation = base.Grid.PositionComp.WorldAABB.Center + m_effectOffset;
				MyParticlesManager.TryCreateParticleEffect("Warp", worldMatrix, out m_effect);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void UpdateParticleEffect()
		{
			if (m_effect != null)
			{
				Vector3D trans = base.Grid.PositionComp.WorldAABB.Center + m_effectOffset;
				m_effect.SetTranslation(ref trans);
			}
		}

		private void StopParticleEffect()
		{
			if (m_effect != null)
			{
				m_effect.StopEmitting(10f);
				m_effect = null;
			}
		}

		private void PerformJump(Vector3D jumpTarget)
		{
			m_jumpDirection = jumpTarget - base.Grid.WorldMatrix.Translation;
			Vector3D.Normalize(ref m_jumpDirection, out m_jumpDirectionNorm);
			DepleteJumpDrives(m_jumpDirection.Length(), m_userId);
			bool flag = false;
			if (IsLocalCharacterAffectedByJump())
			{
				flag = true;
			}
			if (flag)
			{
				MyThirdPersonSpectator.Static.ResetViewerAngle(null);
				MyThirdPersonSpectator.Static.ResetViewerDistance();
				MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
			}
			m_jumped = true;
			MatrixD worldMatrix = base.Grid.WorldMatrix;
			worldMatrix.Translation = base.Grid.WorldMatrix.Translation + m_jumpDirection;
			base.Grid.Teleport(worldMatrix);
			if (flag)
			{
				MyThirdPersonSpectator.Static.ResetViewerAngle(null);
				MyThirdPersonSpectator.Static.ResetViewerDistance();
				MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
			}
		}

		public void AbortJump(MyJumpFailReason reason)
		{
			StopParticleEffect();
			m_soundEmitter?.StopSound(forced: true);
			m_soundEmitterJumpIn?.StopSound(forced: true);
			if (m_isJumping && IsLocalCharacterAffectedByJump())
			{
				ShowNotification(reason);
			}
			CleanupAfterJump();
		}

		private void CleanupAfterJump()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyJumpDrive> enumerator = m_jumpDrives.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().IsJumping = false;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (IsLocalCharacterAffectedByJump())
			{
				MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
			}
			m_jumped = false;
			m_isJumping = false;
			m_effect = null;
			DeSchedule();
		}

		public void AfterGridClose()
		{
			if (m_isJumping)
			{
				m_soundEmitter?.StopSound(forced: true);
				m_soundEmitterJumpIn?.StopSound(forced: true);
				CleanupAfterJump();
			}
			if (m_soundEmitter != null)
			{
				m_soundEmitter.CleanEmitterMethods();
				m_soundEmitter = null;
			}
			if (m_soundEmitterJumpIn != null)
			{
				m_soundEmitterJumpIn.CleanEmitterMethods();
				m_soundEmitterJumpIn = null;
			}
		}

		private void UpdateJumpEffect(float t)
		{
			if (m_playEffect)
			{
				float value = MathHelper.ToRadians(170f);
				float fieldOfView = MathHelper.SmoothStep(MySandboxGame.Config.FieldOfView, value, 1f - t);
				MySector.MainCamera.FieldOfView = fieldOfView;
			}
		}

		public bool CheckReceivedCoordinates(ref Vector3D pos)
		{
			if (m_jumpTimeLeft > 1f)
			{
				return true;
			}
			if (Vector3D.DistanceSquared(base.Grid.PositionComp.GetPosition(), pos) > 100000000.0 && m_jumped)
			{
				MySandboxGame.Log.WriteLine($"Wrong position packet received, dist={Vector3D.Distance(base.Grid.PositionComp.GetPosition(), pos)}, T={m_jumpTimeLeft})");
				return false;
			}
			return true;
		}

		private void OnRequestJumpFromClient(Vector3D jumpTarget, long userId, float jumpDriveDelay)
		{
			if (!IsJumpValid(userId, out var reason))
			{
				SendJumpFailure(reason);
				return;
			}
			m_jumpDirection = jumpTarget - base.Grid.WorldMatrix.Translation;
			Vector3D.Normalize(ref m_jumpDirection, out m_jumpDirectionNorm);
			double minJumpDistance = GetMinJumpDistance(userId);
			double maxJumpDistance = GetMaxJumpDistance(userId);
			double num = (jumpTarget - base.Grid.WorldMatrix.Translation).Length();
			double num2 = num;
			if (num > maxJumpDistance)
			{
				double num3 = maxJumpDistance / num;
				num2 = maxJumpDistance;
				m_jumpDirection *= num3;
			}
			jumpTarget = base.Grid.WorldMatrix.Translation + m_jumpDirection;
<<<<<<< HEAD
			if (num2 < minJumpDistance - 200.0)
=======
			if (num2 < 4800.0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				SendJumpFailure(MyJumpFailReason.ShortDistance);
				return;
			}
			Vector3D? vector3D = FindSuitableJumpLocation(jumpTarget);
			if (!vector3D.HasValue)
			{
				SendJumpFailure(MyJumpFailReason.NoLocation);
			}
			else
			{
				SendJumpSuccess(vector3D.Value, userId, jumpDriveDelay);
			}
		}

		public void RequestJump(Vector3D jumpTarget, long userId, float jumpDriveDelay = 10f)
		{
<<<<<<< HEAD
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnJumpRequested, base.Grid.EntityId, jumpTarget, userId, jumpDriveDelay);
=======
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnJumpRequested, base.Grid.EntityId, jumpTarget, userId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyVisualScriptLogicProvider.GridJumped != null)
			{
				MyVisualScriptLogicProvider.GridJumped(userId, base.Grid.Name, base.Grid.EntityId);
			}
		}

<<<<<<< HEAD
		[Event(null, 1017)]
=======
		[Event(null, 985)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnJumpRequested(long entityId, Vector3D jumpTarget, long userId, float jumpDriveDelay)
		{
			MyEntities.TryGetEntityById(entityId, out MyCubeGrid entity, allowClosed: false);
<<<<<<< HEAD
			entity?.GridSystems.JumpSystem.OnRequestJumpFromClient(jumpTarget, userId, jumpDriveDelay);
=======
			entity?.GridSystems.JumpSystem.OnRequestJumpFromClient(jumpTarget, userId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void SendJumpSuccess(Vector3D jumpTarget, long userId, float jumpDriveDelay)
		{
<<<<<<< HEAD
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnJumpSuccess, base.Grid.EntityId, jumpTarget, userId, jumpDriveDelay);
		}

		[Event(null, 1034)]
=======
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnJumpSuccess, base.Grid.EntityId, jumpTarget, userId);
		}

		[Event(null, 1002)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void OnJumpSuccess(long entityId, Vector3D jumpTarget, long userId, float jumpDriveDelay)
		{
			MyEntities.TryGetEntityById(entityId, out MyCubeGrid entity, allowClosed: false);
<<<<<<< HEAD
			entity?.GridSystems.JumpSystem.Jump(jumpTarget, userId, jumpDriveDelay);
=======
			entity?.GridSystems.JumpSystem.Jump(jumpTarget, userId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void SendJumpFailure(MyJumpFailReason reason)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnJumpFailure, base.Grid.EntityId, reason);
		}

<<<<<<< HEAD
		[Event(null, 1051)]
=======
		[Event(null, 1019)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnJumpFailure(long entityId, MyJumpFailReason reason)
		{
			MyEntities.TryGetEntityById(entityId, out MyCubeGrid _, allowClosed: false);
		}

		private void SendPerformJump(Vector3D jumpTarget)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPerformJump, base.Grid.EntityId, jumpTarget);
		}

<<<<<<< HEAD
		[Event(null, 1068)]
=======
		[Event(null, 1036)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void OnPerformJump(long entityId, Vector3D jumpTarget)
		{
			MyEntities.TryGetEntityById(entityId, out MyCubeGrid entity, allowClosed: false);
			entity?.GridSystems.JumpSystem.PerformJump(jumpTarget);
		}

		private void SendAbortJump()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnAbortJump, base.Grid.EntityId);
		}

<<<<<<< HEAD
		[Event(null, 1084)]
=======
		[Event(null, 1052)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnAbortJump(long entityId)
		{
			MyEntities.TryGetEntityById(entityId, out MyCubeGrid entity, allowClosed: false);
			if (entity != null)
			{
				MyExternalReplicable myExternalReplicable = MyExternalReplicable.FindByObject(entity);
				if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
				{
					ValidationResult validationResult = ValidationResult.Passed;
					if (myExternalReplicable != null)
					{
						validationResult = myExternalReplicable.HasRights(new EndpointId(MyEventContext.Current.Sender.Value), ValidationType.Controlled);
					}
					if (validationResult != 0)
					{
						(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, validationResult.HasFlag(ValidationResult.Kick));
						MyEventContext.ValidationFailed();
						return;
					}
				}
				entity.GridSystems.JumpSystem.AbortJump(MyJumpFailReason.None);
			}
			else if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				MyEventContext.ValidationFailed();
			}
		}

		float? IMyGridJumpDriveSystem.GetRemainingJumpTime()
		{
			return GetRemainingJumpTime();
		}

		bool IMyGridJumpDriveSystem.IsJumpValid(long userId)
		{
			MyJumpFailReason reason;
			return IsJumpValid(userId, out reason);
		}

		Vector3D? IMyGridJumpDriveSystem.FindSuitableJumpLocation(Vector3D desiredLocation)
		{
			return FindSuitableJumpLocation(desiredLocation);
		}

		void IMyGridJumpDriveSystem.Jump(Vector3D jumpTarget, long userId, float jumpDriveDelay)
		{
			if (Sync.IsServer)
			{
				Jump(jumpTarget, userId, jumpDriveDelay);
			}
		}

		void IMyGridJumpDriveSystem.PerformJump(Vector3D jumpTarget)
		{
			if (Sync.IsServer)
			{
				PerformJump(jumpTarget);
			}
		}

		public Vector3D? GetJumpDriveTarget()
		{
			if (m_isJumping && !m_jumped)
			{
				return m_selectedDestination;
			}
			return null;
		}

		public Vector3D? GetJumpDriveDirectionNormalized()
		{
			if (m_isJumping && !m_jumped)
			{
				return m_jumpDirectionNorm;
			}
			return null;
		}

		void IMyGridJumpDriveSystem.AbortJump(int reason)
		{
			AbortJump((MyJumpFailReason)reason);
		}
	}
}
