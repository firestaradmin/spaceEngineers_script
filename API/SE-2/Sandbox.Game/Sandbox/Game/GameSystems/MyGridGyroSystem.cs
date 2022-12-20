using System;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGridGyroSystem : MyUpdateableGridSystem
	{
		private static readonly float INV_TENSOR_MAX_LIMIT = 125000f;

		private static readonly float MAX_SLOWDOWN = (MyFakes.WELD_LANDING_GEARS ? 0.8f : 0.93f);

		private static readonly float MAX_ROLL = (float)Math.E * 449f / 777f;

		private const float TORQUE_SQ_LEN_TH = 0.0001f;

		private Vector3 m_controlTorque;

		public bool AutopilotEnabled;

		private HashSet<MyGyro> m_gyros;

		private bool m_gyrosChanged;

		private MyPhysicsComponentBase m_gridPhysics;

		private bool m_scheduled;

		private float m_maxGyroForce;

		private float m_maxOverrideForce;

		private float m_maxRequiredPowerInput;

		private Vector3 m_overrideTargetVelocity;

		private int? m_overrideAccelerationRampFrames;

		public Vector3 SlowdownTorque;

		public Vector3 ControlTorque
		{
			get
			{
				return m_controlTorque;
			}
			set
			{
				if (m_controlTorque != value)
				{
					m_controlTorque = value;
<<<<<<< HEAD
					if (m_gyros.Count > 0 && ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
=======
					if (m_gyros.get_Count() > 0 && ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						Schedule();
					}
				}
			}
		}

		public bool HasOverrideInput
		{
			get
			{
				if (Vector3.IsZero(ref m_controlTorque))
				{
					return !Vector3.IsZero(ref m_overrideTargetVelocity);
				}
				return true;
			}
		}

		public bool IsDirty => m_gyrosChanged;

		public MyResourceSinkComponent ResourceSink { get; private set; }

		public int GyroCount => m_gyros.get_Count();

		/// <summary>
		/// This should not be used to modify the gyros.
		/// Use Register/Unregister for that.
		/// </summary>
		public HashSet<MyGyro> Gyros => m_gyros;

<<<<<<< HEAD
		/// <summary>
		/// Final torque (clamped by available power, added anti-gravity, slowdown).
		/// </summary>
		public Vector3 Torque { get; private set; }

		/// <inheritdoc />
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

		/// <inheritdoc />
		public override int UpdatePriority => 7;

		/// <inheritdoc />
=======
		public Vector3 Torque { get; private set; }

		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

		public override int UpdatePriority => 7;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override bool UpdateInParallel => true;

		public MyGridGyroSystem(MyCubeGrid grid)
			: base(grid)
		{
			m_gyros = new HashSet<MyGyro>();
			m_gyrosChanged = false;
			ResourceSink = new MyResourceSinkComponent();
<<<<<<< HEAD
			ResourceSink.Init(MyStringHash.GetOrCompute("Gyro"), m_maxRequiredPowerInput, () => m_maxRequiredPowerInput, null);
			ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			ResourceSink.Grid = grid;
=======
			ResourceSink.Init(MyStringHash.GetOrCompute("Gyro"), m_maxRequiredPowerInput, () => m_maxRequiredPowerInput);
			ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			grid.OnPhysicsChanged += GridOnOnPhysicsChanged;
			grid.PositionComp.OnPositionChanged += PositionCompOnPositionChanged;
			TryHookToPhysics();
		}

		private void PositionCompOnPositionChanged(MyPositionComponentBase obj)
		{
<<<<<<< HEAD
			if (m_gridPhysics != null && m_gyros.Count > 0 && !m_scheduled && !Vector3.IsZero(m_gridPhysics.AngularVelocity, 0.001f) && ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				Schedule();
			}
		}

		private void TryHookToPhysics()
		{
			MyGridPhysics physics = base.Grid.Physics;
			if (m_gridPhysics != physics)
			{
=======
			if (m_gridPhysics != null && m_gyros.get_Count() > 0 && !m_scheduled && !Vector3.IsZero(m_gridPhysics.AngularVelocity, 0.001f) && ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				Schedule();
			}
		}

		private void TryHookToPhysics()
		{
			MyGridPhysics physics = base.Grid.Physics;
			if (m_gridPhysics != physics)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_gridPhysics != null)
				{
					m_gridPhysics.OnBodyActiveStateChanged -= PhysicsOnOnBodyActiveStateChanged;
				}
				if (physics != null)
				{
					physics.OnBodyActiveStateChanged += PhysicsOnOnBodyActiveStateChanged;
				}
				m_gridPhysics = physics;
			}
		}

		private void PhysicsOnOnBodyActiveStateChanged(MyPhysicsComponentBase body, bool active)
		{
		}

		private void GridOnOnPhysicsChanged(MyEntity obj)
		{
			TryHookToPhysics();
<<<<<<< HEAD
			if (m_gyros.Count != 0 && !m_scheduled)
			{
				ResourceSink.Grid = base.Grid;
=======
			if (m_gyros.get_Count() != 0 && !m_scheduled)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (base.Grid.Physics.IsStatic || base.Grid.Physics.IsKinematic)
				{
					DeSchedule();
				}
				else if (base.Grid.Physics.IsActive)
				{
					Schedule();
				}
			}
		}

		public void Register(MyGyro gyro)
		{
			m_gyros.Add(gyro);
			m_gyrosChanged = true;
			gyro.EnabledChanged += gyro_EnabledChanged;
			gyro.SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			gyro.PropertiesChanged += gyro_PropertiesChanged;
<<<<<<< HEAD
			if (m_gyros.Count == 1)
=======
			if (m_gyros.get_Count() == 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Schedule();
			}
		}

		private void gyro_PropertiesChanged(MyTerminalBlock sender)
		{
			MarkDirty();
		}

		public void Unregister(MyGyro gyro)
		{
			m_gyros.Remove(gyro);
			m_gyrosChanged = true;
			gyro.EnabledChanged -= gyro_EnabledChanged;
			gyro.SlimBlock.ComponentStack.IsFunctionalChanged -= ComponentStack_IsFunctionalChanged;
		}

		private void UpdateGyros()
		{
			SlowdownTorque = Vector3.Zero;
			MyCubeGrid grid = base.Grid;
			MyGridPhysics physics = grid.Physics;
<<<<<<< HEAD
			if (physics == null || physics.IsKinematic || physics.IsStatic || m_gyros.Count == 0)
=======
			if (physics == null || physics.IsKinematic || physics.IsStatic || m_gyros.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				DeSchedule();
				return;
			}
			if (!ControlTorque.IsValid())
			{
				ControlTorque = Vector3.Zero;
			}
			if (Vector3.IsZero(physics.AngularVelocity, 0.001f) && Vector3.IsZero(ControlTorque, 0.001f))
<<<<<<< HEAD
			{
				DeSchedule();
				return;
			}
			float num = ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
			if (!(num > 0f) || (!physics.Enabled && !physics.IsWelded) || physics.RigidBody.IsFixed)
			{
				return;
			}
			Matrix inverseInertiaTensor = physics.RigidBody.InverseInertiaTensor;
			inverseInertiaTensor.M44 = 1f;
			MatrixD m = grid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
			Matrix matrix = m;
			Vector3 vector = Vector3.Transform(physics.AngularVelocity, ref matrix);
			float num2 = (1f - MAX_SLOWDOWN) * (1f - num) + MAX_SLOWDOWN;
			SlowdownTorque = -vector;
			float num3 = ((grid.GridSizeEnum == MyCubeSize.Large) ? MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER_LARGE_SHIP : MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER);
			Vector3 vector2 = new Vector3(m_maxGyroForce * num3);
			if (physics.IsWelded)
			{
				SlowdownTorque = Vector3.TransformNormal(SlowdownTorque, grid.WorldMatrix);
				SlowdownTorque = Vector3.TransformNormal(SlowdownTorque, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
			}
			if (!vector.IsValid())
			{
				vector = Vector3.Zero;
			}
			Vector3 vector3 = Vector3.One - Vector3.IsZeroVector(Vector3.Sign(vector) - Vector3.Sign(ControlTorque));
			SlowdownTorque *= num3;
			SlowdownTorque /= inverseInertiaTensor.Scale;
			SlowdownTorque = Vector3.Clamp(SlowdownTorque, -vector2, vector2) * vector3;
			if (SlowdownTorque.LengthSquared() > 0.0001f)
			{
				physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, SlowdownTorque * num2);
			}
			Matrix inertiaTensor = MyGridPhysicalGroupData.GetGroupSharedProperties(grid).InertiaTensor;
			float num4 = 1f / Math.Max(Math.Max(inertiaTensor.M11, inertiaTensor.M22), inertiaTensor.M33);
			float num5 = Math.Max(1f, num4 * INV_TENSOR_MAX_LIMIT);
			Torque = Vector3.Clamp(ControlTorque, -Vector3.One, Vector3.One) * m_maxGyroForce / num5;
			Torque *= ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
			Vector3 scale = physics.RigidBody.InertiaTensor.Scale;
			scale = Vector3.Abs(scale / scale.AbsMax());
			if (Torque.LengthSquared() > 0.0001f)
			{
				Vector3 vector4 = Torque;
				if (physics.IsWelded)
				{
					vector4 = Vector3.TransformNormal(vector4, grid.WorldMatrix);
					vector4 = Vector3.TransformNormal(vector4, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
				}
				physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, vector4 * scale);
			}
			if (ControlTorque == Vector3.Zero && physics.AngularVelocity != Vector3.Zero && physics.AngularVelocity.LengthSquared() < 9.000001E-08f && physics.RigidBody.IsActive)
			{
				physics.AngularVelocity = Vector3.Zero;
=======
			{
				DeSchedule();
			}
			else
			{
				if (!(ResourceSink.SuppliedRatio > 0f) || (!physics.Enabled && !physics.IsWelded) || physics.RigidBody.IsFixed)
				{
					return;
				}
				Matrix inverseInertiaTensor = physics.RigidBody.InverseInertiaTensor;
				inverseInertiaTensor.M44 = 1f;
				MatrixD m = grid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
				Matrix matrix = m;
				Vector3 vector = Vector3.Transform(physics.AngularVelocity, ref matrix);
				float num = (1f - MAX_SLOWDOWN) * (1f - ResourceSink.SuppliedRatio) + MAX_SLOWDOWN;
				SlowdownTorque = -vector;
				float num2 = ((grid.GridSizeEnum == MyCubeSize.Large) ? MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER_LARGE_SHIP : MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER);
				Vector3 vector2 = new Vector3(m_maxGyroForce * num2);
				if (physics.IsWelded)
				{
					SlowdownTorque = Vector3.TransformNormal(SlowdownTorque, grid.WorldMatrix);
					SlowdownTorque = Vector3.TransformNormal(SlowdownTorque, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
				}
				if (!vector.IsValid())
				{
					vector = Vector3.Zero;
				}
				Vector3 vector3 = Vector3.One - Vector3.IsZeroVector(Vector3.Sign(vector) - Vector3.Sign(ControlTorque));
				SlowdownTorque *= num2;
				SlowdownTorque /= inverseInertiaTensor.Scale;
				SlowdownTorque = Vector3.Clamp(SlowdownTorque, -vector2, vector2) * vector3;
				if (SlowdownTorque.LengthSquared() > 0.0001f)
				{
					physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, SlowdownTorque * num);
				}
				Matrix inertiaTensor = MyGridPhysicalGroupData.GetGroupSharedProperties(grid).InertiaTensor;
				float num3 = 1f / Math.Max(Math.Max(inertiaTensor.M11, inertiaTensor.M22), inertiaTensor.M33);
				float num4 = Math.Max(1f, num3 * INV_TENSOR_MAX_LIMIT);
				Torque = Vector3.Clamp(ControlTorque, -Vector3.One, Vector3.One) * m_maxGyroForce / num4;
				Torque *= ResourceSink.SuppliedRatio;
				Vector3 scale = physics.RigidBody.InertiaTensor.Scale;
				scale = Vector3.Abs(scale / scale.AbsMax());
				if (Torque.LengthSquared() > 0.0001f)
				{
					Vector3 vector4 = Torque;
					if (physics.IsWelded)
					{
						vector4 = Vector3.TransformNormal(vector4, grid.WorldMatrix);
						vector4 = Vector3.TransformNormal(vector4, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
					}
					physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, vector4 * scale);
				}
				if (ControlTorque == Vector3.Zero && physics.AngularVelocity != Vector3.Zero && physics.AngularVelocity.LengthSquared() < 9.000001E-08f && physics.RigidBody.IsActive)
				{
					physics.AngularVelocity = Vector3.Zero;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void UpdateOverriddenGyros()
		{
<<<<<<< HEAD
			float num = ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
			if (!(num > 0f) || !base.Grid.Physics.Enabled || base.Grid.Physics.RigidBody.IsFixed)
=======
			if (!(ResourceSink.SuppliedRatio > 0f) || !base.Grid.Physics.Enabled || base.Grid.Physics.RigidBody.IsFixed)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			MatrixD m = base.Grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
			Matrix matrix = m;
			m = base.Grid.WorldMatrix.GetOrientation();
			_ = (Matrix)m;
			Vector3 vector = Vector3.Transform(base.Grid.Physics.AngularVelocity, ref matrix);
			Torque = Vector3.Zero;
			Vector3 vector2 = m_overrideTargetVelocity - vector;
			if (!(vector2 == Vector3.Zero))
			{
				UpdateOverrideAccelerationRampFrames(vector2);
				Vector3 vector3 = vector2 * (60f / (float)m_overrideAccelerationRampFrames.Value);
				Matrix inverseInertiaTensor = base.Grid.Physics.RigidBody.InverseInertiaTensor;
				Vector3 vector4 = new Vector3(inverseInertiaTensor.M11, inverseInertiaTensor.M22, inverseInertiaTensor.M33);
				Vector3 vector5 = vector3 / vector4;
				float radius = m_maxOverrideForce + m_maxGyroForce * (1f - ControlTorque.Length());
				Vector3 vector6 = Vector3.ClampToSphere(vector5, radius);
				Torque = ControlTorque * m_maxGyroForce + vector6;
<<<<<<< HEAD
				Torque *= num;
=======
				Torque *= ResourceSink.SuppliedRatio;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!(Torque.LengthSquared() < 0.0001f))
				{
					base.Grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, Torque);
				}
			}
		}

		private void UpdateOverrideAccelerationRampFrames(Vector3 velocityDiff)
		{
			if (!m_overrideAccelerationRampFrames.HasValue)
			{
				float num = velocityDiff.LengthSquared();
				if (num > 2.467401f)
				{
					m_overrideAccelerationRampFrames = 120;
				}
				else
				{
					m_overrideAccelerationRampFrames = (int)(num * 48.2288857f) + 1;
				}
			}
			else if (m_overrideAccelerationRampFrames > 1)
			{
				m_overrideAccelerationRampFrames--;
			}
		}

		public Vector3 GetAngularVelocity(Vector3 control)
		{
<<<<<<< HEAD
			float num = ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
			if (num > 0f && base.Grid.Physics != null && base.Grid.Physics.Enabled && !base.Grid.Physics.RigidBody.IsFixed)
=======
			if (ResourceSink.SuppliedRatio > 0f && base.Grid.Physics != null && base.Grid.Physics.Enabled && !base.Grid.Physics.RigidBody.IsFixed)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MatrixD m = base.Grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
				Matrix matrix = m;
				m = base.Grid.WorldMatrix.GetOrientation();
				Matrix matrix2 = m;
				Vector3 vector = Vector3.Transform(base.Grid.Physics.AngularVelocity, ref matrix);
				Matrix inverseInertiaTensor = base.Grid.Physics.RigidBody.InverseInertiaTensor;
				Vector3 vector2 = new Vector3(inverseInertiaTensor.M11, inverseInertiaTensor.M22, inverseInertiaTensor.M33);
<<<<<<< HEAD
				float num2 = vector2.Min();
				float num3 = Math.Max(1f, num2 * INV_TENSOR_MAX_LIMIT);
				Vector3 vector3 = Vector3.Zero;
				Vector3 vector4 = (m_overrideTargetVelocity - vector) * 60f;
				float num4 = m_maxOverrideForce + m_maxGyroForce * (1f - control.Length());
				vector4 *= Vector3.Normalize(vector2);
				Vector3 vector5 = vector4 / vector2;
				float num5 = vector5.Length() / num4;
				if (num5 < 0.5f && m_overrideTargetVelocity.LengthSquared() < 2.5E-05f)
=======
				float num = vector2.Min();
				float num2 = Math.Max(1f, num * INV_TENSOR_MAX_LIMIT);
				Vector3 vector3 = Vector3.Zero;
				Vector3 vector4 = (m_overrideTargetVelocity - vector) * 60f;
				float num3 = m_maxOverrideForce + m_maxGyroForce * (1f - control.Length());
				vector4 *= Vector3.Normalize(vector2);
				Vector3 vector5 = vector4 / vector2;
				float num4 = vector5.Length() / num3;
				if (num4 < 0.5f && m_overrideTargetVelocity.LengthSquared() < 2.5E-05f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return m_overrideTargetVelocity;
				}
				if (!Vector3.IsZero(vector4, 0.0001f))
				{
<<<<<<< HEAD
					float num6 = 1f - 0.8f / (float)Math.Exp(0.5f * num5);
					vector3 = Vector3.ClampToSphere(vector5, num4) * 0.95f * num6 + vector5 * 0.05f * (1f - num6);
=======
					float num5 = 1f - 0.8f / (float)Math.Exp(0.5f * num4);
					vector3 = Vector3.ClampToSphere(vector5, num3) * 0.95f * num5 + vector5 * 0.05f * (1f - num5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (base.Grid.GridSizeEnum == MyCubeSize.Large)
					{
						vector3 *= 2f;
					}
				}
<<<<<<< HEAD
				Torque = (control * m_maxGyroForce + vector3) / num3;
				Torque *= num;
				if (Torque.LengthSquared() > 0.0001f)
				{
					Vector3 vector6 = Torque * new Vector3(num2) * 0.0166666675f;
=======
				Torque = (control * m_maxGyroForce + vector3) / num2;
				Torque *= ResourceSink.SuppliedRatio;
				if (Torque.LengthSquared() > 0.0001f)
				{
					Vector3 vector6 = Torque * new Vector3(num) * 0.0166666675f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return Vector3.Transform(vector + vector6, ref matrix2);
				}
				if (control == Vector3.Zero && m_overrideTargetVelocity == Vector3.Zero && base.Grid.Physics.AngularVelocity != Vector3.Zero && base.Grid.Physics.AngularVelocity.LengthSquared() < 9.000001E-08f && base.Grid.Physics.RigidBody.IsActive)
				{
					return Vector3.Zero;
				}
			}
			if (base.Grid.Physics != null)
			{
				return base.Grid.Physics.AngularVelocity;
			}
			return Vector3.Zero;
		}

		protected override void Update()
		{
			MySimpleProfiler.Begin("Gyro", MySimpleProfiler.ProfilingBlockType.BLOCK, "Update");
			if (m_gyrosChanged)
			{
				RecomputeGyroParameters();
			}
			if (m_maxOverrideForce == 0f)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_GYROS)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "Old gyros", Color.White, 1f);
				}
				UpdateGyros();
				MySimpleProfiler.End("Update");
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GYROS)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "New gyros", Color.White, 1f);
			}
			if (base.Grid.Physics != null)
			{
				UpdateOverriddenGyros();
			}
			MySimpleProfiler.End("Update");
		}

		public void DebugDraw()
		{
			double num = 4.5 * 0.045;
			Vector3D translation = base.Grid.WorldMatrix.Translation;
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(translation, position);
			double num2 = Math.Atan(4.5 / Math.Max(val, 0.001));
			if (!(num2 <= 0.27000001072883606))
			{
				MyRenderProxy.DebugDrawText3D(translation, $"Grid {base.Grid} Gyro System", Color.Yellow, (float)num2, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				bool flag = Torque.LengthSquared() >= 0.0001f;
				bool flag2 = SlowdownTorque.LengthSquared() > 0.0001f;
				bool flag3 = m_overrideTargetVelocity.LengthSquared() > 0.0001f;
				bool flag4 = base.Grid.Physics != null && base.Grid.Physics.AngularVelocity.LengthSquared() > 1E-05f;
				DebugDrawText($"Gyro count: {GyroCount}", translation + -1.0 * up * num, right, (float)num2);
				DebugDrawText(string.Format("Torque [above threshold - {1}]: {0}", Torque, flag), translation + -2.0 * up * num, right, (float)num2);
				DebugDrawText(string.Format("Slowdown [above threshold - {1}]: {0}", SlowdownTorque, flag2), translation + -3.0 * up * num, right, (float)num2);
				DebugDrawText(string.Format("Override [above threshold - {1}]: {0}", m_overrideTargetVelocity, flag3), translation + -4.0 * up * num, right, (float)num2);
				DebugDrawText($"Angular velocity above threshold - {flag4}", translation + -5.0 * up * num, right, (float)num2);
				if (base.Grid.Physics != null)
				{
					DebugDrawText($"Automatic deactivation enabled - {base.Grid.Physics.RigidBody.EnableDeactivation}", translation + -7.0 * up * num, right, (float)num2);
				}
			}
		}

		private void DebugDrawText(string text, Vector3D origin, Vector3D rightVector, float textSize)
		{
			Vector3D vector3D = 0.05000000074505806 * rightVector;
			Vector3D worldCoord = origin + vector3D + rightVector * 0.014999999664723873;
			MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
		}

		private void RecomputeGyroParameters()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			m_gyrosChanged = false;
			_ = m_maxRequiredPowerInput;
			m_maxGyroForce = 0f;
			m_maxOverrideForce = 0f;
			m_maxRequiredPowerInput = 0f;
			m_overrideTargetVelocity = Vector3.Zero;
			m_overrideAccelerationRampFrames = null;
			Enumerator<MyGyro> enumerator = m_gyros.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGyro current = enumerator.get_Current();
					if (IsUsed(current))
					{
						if (!current.GyroOverride || AutopilotEnabled)
						{
							m_maxGyroForce += current.MaxGyroForce;
						}
						else
						{
							m_overrideTargetVelocity += current.GyroOverrideVelocityGrid * current.MaxGyroForce;
							m_maxOverrideForce += current.MaxGyroForce;
						}
						m_maxRequiredPowerInput += current.RequiredPowerInput;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (m_maxOverrideForce != 0f)
			{
				m_overrideTargetVelocity /= m_maxOverrideForce;
			}
			ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, m_maxRequiredPowerInput);
			ResourceSink.Update();
			UpdateAutomaticDeactivation();
		}

		private bool IsUsed(MyGyro gyro)
		{
			if (gyro.Enabled)
			{
				return gyro.IsFunctional;
			}
			return false;
		}

		private void gyro_EnabledChanged(MyTerminalBlock obj)
		{
			MarkDirty();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			MarkDirty();
		}

		public void MarkDirty()
		{
			Schedule();
			m_gyrosChanged = true;
		}

		private void Receiver_IsPoweredChanged()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyGyro> enumerator = m_gyros.GetEnumerator();
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

		private void UpdateAutomaticDeactivation()
		{
			if (base.Grid.Physics != null && !base.Grid.Physics.RigidBody.IsFixed)
			{
				if (!Vector3.IsZero(m_overrideTargetVelocity) && ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
				{
					base.Grid.Physics.RigidBody.EnableDeactivation = false;
				}
				else
				{
					base.Grid.Physics.RigidBody.EnableDeactivation = true;
				}
			}
		}
	}
}
