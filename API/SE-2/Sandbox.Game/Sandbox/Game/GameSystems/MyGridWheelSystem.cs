using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	[StaticEventOwner]
	public class MyGridWheelSystem : MyUpdateableGridSystem
	{
		public enum WheelJumpSate
		{
			Idle,
			Charging,
			Pushing,
			Restore
		}

		protected sealed class InvokeJumpInternal_003C_003ESystem_Int64_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in bool initiate, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				InvokeJumpInternal(gridId, initiate);
			}
		}

		private Vector3 m_angularVelocity;

		private const int JUMP_FULL_CHARGE_TIME = 600;

		private bool m_wheelsChanged;

		private float m_maxRequiredPowerInput;

		private readonly HashSet<MyMotorSuspension> m_wheels;

		private readonly HashSet<MyMotorSuspension> m_wheelsNeedingUpdate;

		private readonly MyResourceSinkComponent m_sinkComp;

		private bool m_handbrake;

		private bool m_brake;

		private ulong m_jumpStartTime;

		private bool m_lastJumpInput;

		public WheelJumpSate m_jumpState;

		private int m_consecutiveCorrectionFrames;

		private Vector3 m_lastPhysicsAngularVelocityLateral;

		public Vector3 AngularVelocity
		{
			get
			{
				return m_angularVelocity;
			}
			set
			{
				if (m_angularVelocity != value)
				{
					m_angularVelocity = value;
<<<<<<< HEAD
					if (m_wheels.Count > 0)
=======
					if (m_wheels.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						Schedule();
					}
				}
			}
		}

		public HashSet<MyMotorSuspension> Wheels => m_wheels;

		public int WheelCount => m_wheels.get_Count();

		public int HandbrakeWheelCount
		{
			get
			{
				int num = 0;
				foreach (MyMotorSuspension wheel in m_wheels)
				{
					if (wheel.IsParkingEnabled)
					{
						num++;
					}
				}
				return num;
			}
		}

		public bool HandBrake
		{
			get
			{
				return m_handbrake;
			}
			set
			{
<<<<<<< HEAD
				TrySetHandbrake(value);
=======
				if (m_handbrake != value)
				{
					m_handbrake = value;
					if (Sync.IsServer)
					{
						UpdateBrake();
					}
					if (value)
					{
						Schedule();
					}
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool IsParked => HandBrake;

		public bool Brake
		{
			get
			{
				return m_brake;
			}
			set
			{
				if (m_brake != value)
				{
					m_brake = value;
					UpdateBrake();
					if (value)
					{
						Schedule();
					}
				}
			}
		}

		public bool LastJumpInput => m_lastJumpInput;

<<<<<<< HEAD
		/// <inheritdoc />
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

		public override int UpdatePriority => 9;

		public event Action<MyCubeGrid> OnMotorUnregister;

		public void TrySetHandbrake(bool value)
		{
			if (HandbrakeWheelCount == 0)
			{
				value = false;
			}
			if (m_handbrake != value)
			{
				m_handbrake = value;
				if (Sync.IsServer)
				{
					UpdateBrake();
				}
				if (value)
				{
					Schedule();
				}
			}
		}
=======
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

		public override int UpdatePriority => 9;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyGridWheelSystem(MyCubeGrid grid)
			: base(grid)
		{
			m_wheels = new HashSet<MyMotorSuspension>();
			m_wheelsNeedingUpdate = new HashSet<MyMotorSuspension>();
			m_wheelsChanged = false;
			m_sinkComp = new MyResourceSinkComponent();
			m_sinkComp.Init(MyStringHash.GetOrCompute("Utility"), m_maxRequiredPowerInput, () => m_maxRequiredPowerInput, null);
			m_sinkComp.Grid = grid;
			m_sinkComp.IsPoweredChanged += ReceiverIsPoweredChanged;
			grid.OnPhysicsChanged += OnGridPhysicsChanged;
		}

		public void Register(MyMotorSuspension motor)
		{
			m_wheels.Add(motor);
			OnBlockNeedsUpdateChanged(motor);
			motor.EnabledChanged += MotorEnabledChanged;
			if (Sync.IsServer)
			{
				motor.Handbrake = m_handbrake;
			}
			MarkDirty();
		}

		public void Unregister(MyMotorSuspension motor)
		{
			if (motor != null)
			{
				if (motor.RotorGrid != null && this.OnMotorUnregister != null)
				{
					this.OnMotorUnregister(motor.RotorGrid);
				}
				m_wheels.Remove(motor);
				m_wheelsNeedingUpdate.Remove(motor);
				motor.EnabledChanged -= MotorEnabledChanged;
				MarkDirty();
			}
		}

		public void OnBlockNeedsUpdateChanged(MyMotorSuspension motor)
		{
			if (motor.NeedsPerFrameUpdate)
			{
				m_wheelsNeedingUpdate.Add(motor);
				MarkDirty();
			}
			else
			{
				m_wheelsNeedingUpdate.Remove(motor);
			}
		}

		protected override void Update()
		{
<<<<<<< HEAD
=======
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
			{
				return;
			}
			if (m_wheelsChanged)
			{
				RecomputeWheelParameters();
			}
			if (m_jumpState == WheelJumpSate.Pushing || m_jumpState == WheelJumpSate.Restore)
			{
				MyWheel.WheelExplosionLog(base.Grid, null, "JumpUpdate: " + m_jumpState);
<<<<<<< HEAD
				foreach (MyMotorSuspension wheel in Wheels)
=======
				Enumerator<MyMotorSuspension> enumerator = Wheels.GetEnumerator();
				try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current().OnSuspensionJumpStateUpdated();
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				switch (m_jumpState)
				{
				case WheelJumpSate.Pushing:
					m_jumpState = WheelJumpSate.Restore;
					break;
				case WheelJumpSate.Restore:
					m_jumpState = WheelJumpSate.Idle;
					break;
				}
			}
			MyGridPhysics physics = base.Grid.Physics;
			if (physics != null)
			{
				Vector3 linVelocityNormal = physics.LinearVelocity;
				float num = linVelocityNormal.Normalize() / MyGridPhysics.GetShipMaxLinearVelocity(base.Grid.GridSizeEnum);
				if (num > 1f)
				{
					num = 1f;
				}
				bool flag = m_angularVelocity.Z < 0f;
				bool flag2 = m_angularVelocity.Z > 0f;
				bool flag3 = flag || flag2;
<<<<<<< HEAD
				if (m_wheels.Count > 0)
=======
				if (m_wheels.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					int num2 = 0;
					bool flag4 = !base.Grid.GridSystems.GyroSystem.HasOverrideInput;
					Vector3 suspensionNormal = Vector3.Zero;
					Vector3 groundNormal = Vector3.Zero;
<<<<<<< HEAD
					foreach (MyMotorSuspension wheel2 in m_wheels)
					{
						bool flag5 = wheel2.IsWorking && wheel2.Propulsion;
						wheel2.AxleFrictionLogic(num, flag3 && flag5);
						wheel2.Update();
						if (flag4 && wheel2.LateralCorrectionLogicInfo(ref groundNormal, ref suspensionNormal))
						{
							num2++;
						}
						if (wheel2.IsWorking)
						{
							if (wheel2.Steering)
							{
								wheel2.Steer(m_angularVelocity.X, num);
							}
							if (wheel2.Propulsion)
							{
								wheel2.UpdatePropulsion(flag, flag2);
							}
						}
					}
					flag4 &= num2 != 0 && !Vector3.IsZero(ref suspensionNormal) && !Vector3.IsZero(ref groundNormal);
					bool flag6 = false;
					if (flag4)
					{
=======
					Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyMotorSuspension current = enumerator.get_Current();
							bool flag5 = current.IsWorking && current.Propulsion;
							current.AxleFrictionLogic(num, flag3 && flag5);
							current.Update();
							if (flag4 && current.LateralCorrectionLogicInfo(ref groundNormal, ref suspensionNormal))
							{
								num2++;
							}
							if (current.IsWorking)
							{
								if (current.Steering)
								{
									current.Steer(m_angularVelocity.X, num);
								}
								if (current.Propulsion)
								{
									current.UpdatePropulsion(flag, flag2);
								}
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					flag4 &= num2 != 0 && !Vector3.IsZero(ref suspensionNormal) && !Vector3.IsZero(ref groundNormal);
					bool flag6 = false;
					if (flag4)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						flag6 = LateralCorrectionLogic(ref suspensionNormal, ref groundNormal, ref linVelocityNormal);
					}
					if (flag6)
					{
						if (m_consecutiveCorrectionFrames < 10)
						{
							m_consecutiveCorrectionFrames++;
						}
					}
					else
					{
						m_consecutiveCorrectionFrames = (int)((float)m_consecutiveCorrectionFrames * 0.8f);
					}
				}
			}
<<<<<<< HEAD
			if (m_wheelsNeedingUpdate.Count == 0 && (base.Grid.Physics == null || (double)base.Grid.Physics.LinearVelocity.LengthSquared() < 0.1) && m_angularVelocity == Vector3.Zero)
=======
			if (m_wheelsNeedingUpdate.get_Count() == 0 && (base.Grid.Physics == null || (double)base.Grid.Physics.LinearVelocity.LengthSquared() < 0.1) && m_angularVelocity == Vector3.Zero)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				DeSchedule();
			}
		}

		private bool LateralCorrectionLogic(ref Vector3 gridDownNormal, ref Vector3 lateralCorrectionNormal, ref Vector3 linVelocityNormal)
		{
			if (!Sync.IsServer && !base.Grid.IsClientPredicted)
			{
				return false;
			}
			MyGridPhysics physics = base.Grid.Physics;
			bool result = false;
			MatrixD matrix = base.Grid.WorldMatrix;
			Vector3.TransformNormal(ref gridDownNormal, ref matrix, out gridDownNormal);
			gridDownNormal = Vector3.ProjectOnPlane(ref gridDownNormal, ref linVelocityNormal);
			lateralCorrectionNormal = Vector3.ProjectOnPlane(ref lateralCorrectionNormal, ref linVelocityNormal);
			Vector3 vector = Vector3.Cross(gridDownNormal, linVelocityNormal);
			gridDownNormal.Normalize();
			lateralCorrectionNormal.Normalize();
			Vector3 vec = physics.AngularVelocity;
			vec = Vector3.ProjectOnVector(ref vec, ref linVelocityNormal);
			float num = vec.Length();
			float num2 = m_lastPhysicsAngularVelocityLateral.Length();
			if (num > num2)
			{
				result = true;
			}
			float num3 = Vector3.Dot(lateralCorrectionNormal, vector) * (float)Math.Max(1, m_consecutiveCorrectionFrames);
			float value = num3 * num * num;
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
			{
				Vector3D translation = matrix.Translation;
				MyRenderProxy.DebugDrawArrow3DDir(translation, lateralCorrectionNormal * 5f, Color.Yellow);
				MyRenderProxy.DebugDrawArrow3DDir(translation, gridDownNormal * 5f, Color.Pink);
				MyRenderProxy.DebugDrawArrow3DDir(translation, vec * 5f, Color.Red);
			}
			m_lastPhysicsAngularVelocityLateral = vec;
			if (Math.Abs(value) > 0.02f)
			{
				Vector3 vector2 = linVelocityNormal * num3;
				if (Vector3.Dot(vector2, vec) > 0f)
				{
					vector2 = Vector3.ClampToSphere(vector2, vec.Length());
					if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
					{
						MyRenderProxy.DebugDrawArrow3DDir(matrix.Translation - gridDownNormal * 5f, vector2 * 100f, Color.Red);
					}
					physics.AngularVelocity -= vector2;
					result = true;
				}
			}
			return result;
		}

		public bool HasWorkingWheels(bool propulsion)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyMotorSuspension current = enumerator.get_Current();
					if (current.IsWorking)
					{
						if (!propulsion)
						{
							return true;
						}
						if (current.RotorGrid != null && current.RotorAngularVelocity.LengthSquared() > 2f)
						{
							return true;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		internal void InitControl(MyEntity controller)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().InitControl(controller);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		internal void ReleaseControl(MyEntity controller)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().ReleaseControl(controller);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			UpdateJumpControlState(isCharging: false, sync: false);
		}

		private void UpdateBrake()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
			try
			{
<<<<<<< HEAD
				wheel.Brake = m_brake;
				wheel.Handbrake = m_handbrake;
=======
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Brake = m_brake | m_handbrake;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void OnGridPhysicsChanged(MyEntity obj)
		{
			if (base.Grid.GridSystems != null && base.Grid.GridSystems.ControlSystem != null)
			{
				MyShipController shipController = base.Grid.GridSystems.ControlSystem.GetShipController();
				if (shipController != null)
				{
					InitControl(shipController);
				}
			}
			m_sinkComp.Grid = base.Grid;
		}

		private void RecomputeWheelParameters()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			m_wheelsChanged = false;
			m_maxRequiredPowerInput = 0f;
			Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyMotorSuspension current = enumerator.get_Current();
					if (IsUsed(current))
					{
						m_maxRequiredPowerInput += current.RequiredPowerInput;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_sinkComp.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, m_maxRequiredPowerInput);
			m_sinkComp.Update();
		}

		private static bool IsUsed(MyMotorSuspension motor)
		{
			if (motor.Enabled)
			{
				return motor.IsFunctional;
			}
			return false;
		}

		private void MotorEnabledChanged(MyTerminalBlock obj)
		{
			MarkDirty();
		}

		private void ReceiverIsPoweredChanged()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyMotorSuspension> enumerator = m_wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdateIsWorking();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void UpdateJumpControlState(bool isCharging, bool sync)
		{
			if (isCharging && base.Grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != 0)
			{
				isCharging = false;
			}
			bool lastJumpInput = m_lastJumpInput;
			if (lastJumpInput == isCharging)
			{
				return;
			}
			Schedule();
			if (sync || Sync.IsServer)
			{
				bool arg = !lastJumpInput;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => InvokeJumpInternal, base.Grid.EntityId, arg);
			}
			m_lastJumpInput = isCharging;
		}

<<<<<<< HEAD
		[Event(null, 483)]
=======
		[Event(null, 448)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		public static void InvokeJumpInternal(long gridId, bool initiate)
		{
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			MyCubeGrid myCubeGrid = (MyCubeGrid)MyEntities.GetEntityById(gridId);
			if (myCubeGrid == null)
			{
				_ = Sync.IsServer;
				return;
			}
			if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
			{
				MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(myCubeGrid);
				if ((controllingPlayer == null || controllingPlayer.Client.SteamUserId != MyEventContext.Current.Sender.Value) && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
				{
					controllingPlayer = MySession.Static.Players.GetPreviousControllingPlayer(myCubeGrid);
					bool kick = (controllingPlayer == null || controllingPlayer.Client.SteamUserId != MyEventContext.Current.Sender.Value) && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value);
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, kick);
					MyEventContext.ValidationFailed();
					return;
				}
			}
			MyWheel.WheelExplosionLog(myCubeGrid, null, "InvokeJump" + initiate);
			MyGridWheelSystem wheelSystem = myCubeGrid.GridSystems.WheelSystem;
			if (initiate)
			{
				wheelSystem.m_jumpState = WheelJumpSate.Charging;
				wheelSystem.m_jumpStartTime = MySandboxGame.Static.SimulationFrameCounter;
			}
			else
			{
				wheelSystem.m_jumpState = WheelJumpSate.Pushing;
			}
			Enumerator<MyMotorSuspension> enumerator = wheelSystem.Wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().OnSuspensionJumpStateUpdated();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void SetWheelJumpStrengthRatioIfJumpEngaged(ref float strength, float defaultStrength)
		{
			switch (m_jumpState)
			{
			case WheelJumpSate.Charging:
				strength = 0.0001f;
				break;
			case WheelJumpSate.Pushing:
			{
				ulong num = MySandboxGame.Static.SimulationFrameCounter - m_jumpStartTime;
				float num2 = Math.Min(1f, (float)num / 600f);
				strength = defaultStrength + (1f - defaultStrength) * num2;
				break;
			}
			}
		}

		private void MarkDirty()
		{
			m_wheelsChanged = true;
			Schedule();
		}
	}
}
