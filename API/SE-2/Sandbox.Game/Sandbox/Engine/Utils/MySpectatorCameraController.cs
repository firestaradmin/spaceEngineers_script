using System;
using System.Collections.Generic;
using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MySpectatorCameraController : MySpectator, IMyCameraController
	{
		private const int REFLECTOR_RANGE_MULTIPLIER = 5;

		public new static MySpectatorCameraController Static;

		private Vector3D ThirdPersonCameraOrbit = Vector3D.UnitZ * 10.0;

		private CyclingOptions m_cycling;

		private float m_cyclingMetricValue = float.MinValue;

		private long m_entityID;

		private MyEntity m_character;

		private double m_yaw;

		private double m_pitch;

		private double m_roll;

		private Vector3D m_lastRightVec = Vector3D.Right;

		private Vector3D m_lastUpVec = Vector3D.Up;

		private MatrixD m_lastOrientation = MatrixD.Identity;

		private float m_lastOrientationWeight = 1f;

		private MyLight m_light;

		private Vector3D m_velocity;

		public bool IsLightOn
		{
			get
			{
				if (m_light != null)
				{
					return m_light.LightOn;
				}
				return false;
			}
		}

		public bool AlignSpectatorToGravity { get; set; }

		public long TrackedEntity { get; set; }

		public MyEntity Entity => null;

		public Vector3D Velocity
		{
			get
			{
				return m_velocity;
			}
			set
			{
				m_velocity = value;
			}
		}

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return base.IsInFirstPersonView;
			}
			set
			{
				base.IsInFirstPersonView = value;
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				return base.ForceFirstPersonCamera;
			}
			set
			{
				base.ForceFirstPersonCamera = value;
			}
		}

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

		bool IMyCameraController.AllowCubeBuilding => true;

		public MySpectatorCameraController()
		{
			Static = this;
		}

		public override void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			UpdateVelocity();
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeAngular = Math.Min(base.SpeedModeAngular * 1.5f, 6f);
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeAngular = Math.Max(base.SpeedModeAngular / 1.5f, 0.0001f);
				}
			}
			else if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyAltKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeLinear = Math.Min(base.SpeedModeLinear * 1.5f, 8000f);
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeLinear = Math.Max(base.SpeedModeLinear / 1.5f, 0.0001f);
				}
			}
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			float num = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_SPEED_UP);
			float num2 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_SPEED_DOWN);
			float num3 = 1.1f;
			if (num2 > 0f)
			{
				float num4 = num2 * num3 + (1f - num2);
				base.SpeedModeLinear = Math.Min(base.SpeedModeLinear * num4, 8000f);
			}
			else if (num > 0f)
			{
				float num5 = num * num3 + (1f - num);
				base.SpeedModeLinear = Math.Max(base.SpeedModeLinear / num5, 0.0001f);
			}
			float num6 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_UP);
			float num7 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_DOWN);
			float num8 = 1.1f;
			if (num6 > 0f)
			{
				float num9 = num6 * num8 + (1f - num6);
				base.SpeedModeAngular = Math.Min(base.SpeedModeAngular * num9, 6f);
			}
			else if (num7 > 0f)
			{
				float num10 = num7 * num8 + (1f - num7);
				base.SpeedModeAngular = Math.Max(base.SpeedModeAngular / num10, 0.0001f);
			}
			switch (base.SpectatorCameraMovement)
			{
			case MySpectatorCameraMovementEnum.FreeMouse:
				MoveAndRotate_FreeMouse(moveIndicator, rotationIndicator, rollIndicator);
				break;
			case MySpectatorCameraMovementEnum.ConstantDelta:
				MoveAndRotate_ConstantDelta(moveIndicator, rotationIndicator, rollIndicator);
				if (IsLightOn)
				{
					UpdateLightPosition();
				}
				break;
			case MySpectatorCameraMovementEnum.UserControlled:
				MoveAndRotate_UserControlled(moveIndicator, rotationIndicator, rollIndicator);
				if (IsLightOn)
				{
					UpdateLightPosition();
				}
				break;
			case MySpectatorCameraMovementEnum.Orbit:
				base.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
				break;
			case MySpectatorCameraMovementEnum.None:
				break;
			}
		}

		public override void Update()
		{
			base.Update();
			base.Position += m_velocity * 0.01666666753590107;
		}

		private void UpdateVelocity()
		{
			if (MyInput.Static.IsAnyShiftKeyPressed())
			{
				if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Middle))
				{
					TryParentSpectator();
				}
				if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Right))
				{
					m_velocity = Vector3D.Zero;
				}
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					m_velocity *= 1.1000000238418579;
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					m_velocity /= 1.1000000238418579;
				}
			}
		}

		private void TryParentSpectator()
		{
			_ = MySector.MainCamera;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(base.Position, base.Position + base.Orientation.Forward * 1000.0, list);
			IMyEntity myEntity = ((list.Count <= 0) ? null : list[0].HkHitInfo.Body.GetEntity(list[0].HkHitInfo.GetShapeKey(0)));
			if (myEntity != null)
			{
				m_velocity = myEntity.Physics.LinearVelocity;
			}
			else
			{
				m_velocity = Vector3D.Zero;
			}
		}

		private void MoveAndRotate_UserControlled(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			float num = 1.66666675f;
			float num2 = 0.0025f * m_speedModeAngular;
			rollIndicator = MyInput.Static.GetDeveloperRoll();
			float num3 = 0f;
			if (rollIndicator != 0f)
			{
				num3 = rollIndicator * m_speedModeAngular * 0.1f;
				num3 = MathHelper.Clamp(num3, -0.02f, 0.02f);
				MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Right, out var xOut, out var yOut, num3);
				m_orientation.Right = yOut;
				m_orientation.Up = xOut;
			}
			if (AlignSpectatorToGravity)
			{
				rotationIndicator.Rotate(m_roll);
				m_yaw -= rotationIndicator.Y * num2;
				m_pitch -= rotationIndicator.X * num2;
				m_roll -= num3;
				MathHelper.LimitRadians2PI(ref m_yaw);
				m_pitch = MathHelper.Clamp(m_pitch, -Math.PI / 2.0, Math.PI / 2.0);
				MathHelper.LimitRadians2PI(ref m_roll);
				ComputeGravityAlignedOrientation(out m_orientation);
			}
			else
			{
				if (m_lastOrientationWeight < 1f)
				{
					m_orientation = MatrixD.Orthogonalize(m_orientation);
					m_orientation.Forward = Vector3D.Cross(m_orientation.Up, m_orientation.Right);
				}
				if (rotationIndicator.Y != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Right, m_orientation.Forward, out var xOut2, out var yOut2, (0f - rotationIndicator.Y) * num2);
					m_orientation.Right = xOut2;
					m_orientation.Forward = yOut2;
				}
				if (rotationIndicator.X != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Forward, out var xOut3, out var yOut3, rotationIndicator.X * num2);
					m_orientation.Up = xOut3;
					m_orientation.Forward = yOut3;
				}
				m_lastOrientation = m_orientation;
				m_lastOrientationWeight = 1f;
				m_roll = 0.0;
				m_pitch = 0.0;
			}
			Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			MyStringId context = MySpaceBindingCreator.CX_SPECTATOR;
			if (!MySession.Static.IsCameraUserControlledSpectator())
			{
				context = controlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			}
			float num4 = ((MyInput.Static.IsAnyShiftKeyPressed() || MyControllerHelper.IsControl(context, MyControlsSpace.SPECTATOR_SPEED_BOOST, MyControlStateType.PRESSED)) ? 1f : 0.35f) * (MyInput.Static.IsAnyCtrlKeyPressed() ? 0.3f : 1f);
			moveIndicator *= num4 * base.SpeedModeLinear;
			Vector3 position = moveIndicator * num;
			base.Position += Vector3.Transform(position, m_orientation);
		}

		private void ComputeGravityAlignedOrientation(out MatrixD resultOrientationStorage)
		{
			bool flag = true;
			Vector3D vector = -MyGravityProviderSystem.CalculateTotalGravityInPoint(base.Position);
			if (vector.LengthSquared() < 9.9999997473787516E-06)
			{
				vector = m_lastUpVec;
				m_lastOrientationWeight = 1f;
				flag = false;
			}
			else
			{
				m_lastUpVec = vector;
			}
			vector.Normalize();
			Vector3D vector2 = m_lastRightVec - Vector3D.Dot(m_lastRightVec, vector) * vector;
			if (vector2.LengthSquared() < 9.9999997473787516E-06)
			{
				vector2 = m_orientation.Right - Vector3D.Dot(m_orientation.Right, vector) * vector;
				if (vector2.LengthSquared() < 9.9999997473787516E-06)
				{
					vector2 = m_orientation.Forward - Vector3D.Dot(m_orientation.Forward, vector) * vector;
				}
			}
			vector2.Normalize();
			m_lastRightVec = vector2;
			Vector3D.Cross(ref vector, ref vector2, out var result);
			resultOrientationStorage = MatrixD.Identity;
			resultOrientationStorage.Right = vector2;
			resultOrientationStorage.Up = vector;
			resultOrientationStorage.Forward = result;
			resultOrientationStorage = MatrixD.CreateFromAxisAngle(Vector3D.Right, m_pitch) * resultOrientationStorage * MatrixD.CreateFromAxisAngle(vector, m_yaw);
			vector = resultOrientationStorage.Up;
			vector2 = resultOrientationStorage.Right;
			resultOrientationStorage.Right = Math.Cos(m_roll) * vector2 + Math.Sin(m_roll) * vector;
			resultOrientationStorage.Up = (0.0 - Math.Sin(m_roll)) * vector2 + Math.Cos(m_roll) * vector;
			if (flag && m_lastOrientationWeight > 0f)
			{
				m_lastOrientationWeight = Math.Max(0f, m_lastOrientationWeight - 0.0166666675f);
				resultOrientationStorage = MatrixD.Slerp(resultOrientationStorage, m_lastOrientation, MathHelper.SmoothStepStable(m_lastOrientationWeight));
				resultOrientationStorage = MatrixD.Orthogonalize(resultOrientationStorage);
				resultOrientationStorage.Forward = Vector3D.Cross(resultOrientationStorage.Up, resultOrientationStorage.Right);
			}
			if (!flag)
			{
				m_lastOrientation = resultOrientationStorage;
			}
		}

		private void MoveAndRotate_ConstantDelta(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			m_cycling.Enabled = true;
			bool flag = false;
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOOLBAR_UP) && MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyEntityCycling.FindNext(MyEntityCyclingOrder.Characters, ref m_cyclingMetricValue, ref m_entityID, findLarger: false, m_cycling);
				flag = true;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOOLBAR_DOWN) && MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyEntityCycling.FindNext(MyEntityCyclingOrder.Characters, ref m_cyclingMetricValue, ref m_entityID, findLarger: true, m_cycling);
				flag = true;
			}
			if (!MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					ThirdPersonCameraOrbit /= 1.1000000238418579;
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					ThirdPersonCameraOrbit *= 1.1000000238418579;
				}
			}
			if (flag)
			{
				MyEntities.TryGetEntityById(m_entityID, out m_character);
			}
<<<<<<< HEAD
			if (TrackedEntity == 0L && MySession.Static.ControlledEntity != null)
			{
				MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef.Up);
				TrackedEntity = MySession.Static.ControlledEntity.Entity.EntityId;
				InitOrbitXOrbitY();
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyEntities.TryGetEntityById(TrackedEntity, out var entity);
			if (entity != null)
			{
				Vector3D position = entity.PositionComp.GetPosition();
				if (AlignSpectatorToGravity)
				{
					m_roll = 0.0;
					m_yaw = 0.0;
					m_pitch = 0.0;
					ComputeGravityAlignedOrientation(out var resultOrientationStorage);
					base.Position = position + Vector3D.Transform(ThirdPersonCameraDelta, resultOrientationStorage);
					base.Target = position;
					m_orientation.Up = resultOrientationStorage.Up;
				}
				else
				{
					Vector3D vector3D = Vector3D.Normalize(base.Position - base.Target) * ThirdPersonCameraDelta.Length();
					base.Position = position + vector3D;
					base.Target = position;
				}
			}
			bool flag2 = MyInput.Static.IsAnyAltKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed();
			UpdateCameraPosition(flag2 ? rotationIndicator : Vector2.Zero);
		}

		private void MoveAndRotate_FreeMouse(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition != null || MySessionComponentVoxelHand.Static.Enabled || MyInput.Static.IsRightMousePressed())
			{
				MoveAndRotate_UserControlled(moveIndicator, rotationIndicator, rollIndicator);
			}
			else
			{
				MoveAndRotate_UserControlled(moveIndicator, Vector2.Zero, rollIndicator);
			}
		}

		private void InitOrbitXOrbitY()
		{
			Vector3 vec = m_orientation.Forward;
			Vector3.ProjectOnPlane(ref vec, ref Vector3.Up);
			float num = Vector3.Dot(vec, Vector3.Right);
			float num2 = Vector3.Dot(vec, Vector3.Forward);
			if (num >= 0f && num2 >= 0f)
			{
				m_orbitY = (float)Math.Acos(num2);
			}
			else if (num >= 0f && num2 <= 0f)
			{
				m_orbitY = (float)(Math.PI - Math.Acos(num2));
			}
			else if (num <= 0f && num2 <= 0f)
			{
				m_orbitY = (float)(Math.PI + Math.Acos(num2));
			}
			else if (num <= 0f && num2 >= 0f)
			{
<<<<<<< HEAD
				m_orbitY = (float)(Math.PI * 2.0 - Math.Acos(num2));
=======
				ComputeGravityAlignedOrientation(out var resultOrientationStorage);
				m_orientation.Up = resultOrientationStorage.Up;
				m_orientation.Forward = Vector3D.Normalize(base.Target - base.Position);
				m_orientation.Right = Vector3D.Cross(m_orientation.Forward, m_orientation.Up);
				AlignSpectatorToGravity = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_orbitY += (float)Math.PI;
			m_orbitX = (float)Math.Asin(Vector3.Dot(m_orientation.Forward, Vector3.Up));
		}

		protected override void OnChangingMode(MySpectatorCameraMovementEnum oldMode, MySpectatorCameraMovementEnum newMode)
		{
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			currentCamera.SetViewMatrix(GetViewMatrix());
		}

		public void InitLight(bool isLightOn)
		{
			m_light = MyLights.AddLight();
			if (m_light != null)
			{
				m_light.Start("SpectatorCameraController");
				m_light.ReflectorOn = true;
				m_light.ReflectorTexture = "Textures\\Lights\\dual_reflector_2.dds";
				m_light.Range = 2f;
				m_light.ReflectorRange = 35f;
				m_light.ReflectorColor = MyCharacter.REFLECTOR_COLOR;
				m_light.ReflectorIntensity = MyCharacter.REFLECTOR_INTENSITY;
				m_light.ReflectorGlossFactor = MyCharacter.REFLECTOR_GLOSS_FACTOR;
				m_light.ReflectorDiffuseFactor = MyCharacter.REFLECTOR_DIFFUSE_FACTOR;
				m_light.Color = MyCharacter.POINT_COLOR;
				m_light.Intensity = MyCharacter.POINT_LIGHT_INTENSITY;
				m_light.UpdateReflectorRangeAndAngle(0.373f, 175f);
				m_light.LightOn = isLightOn;
				m_light.ReflectorOn = isLightOn;
			}
		}

		private void UpdateLightPosition()
		{
			if (m_light != null)
			{
				MatrixD matrixD = MatrixD.CreateWorld(base.Position, m_orientation.Forward, m_orientation.Up);
				m_light.ReflectorDirection = matrixD.Forward;
				m_light.ReflectorUp = matrixD.Up;
				m_light.Position = base.Position;
				m_light.UpdateLight();
			}
		}

		/// <summary>
		/// Switch the light of the spectator - especially relevant during night time or dark zone
		/// </summary>
		public void SwitchLight()
		{
			if (m_light != null)
			{
				m_light.LightOn = !m_light.LightOn;
				m_light.ReflectorOn = !m_light.ReflectorOn;
				m_light.UpdateLight();
			}
		}

		public void TurnLightOff()
		{
			if (m_light != null)
			{
				m_light.LightOn = false;
				m_light.ReflectorOn = false;
				m_light.UpdateLight();
			}
		}

		public void CleanLight()
		{
			if (m_light != null)
			{
				MyLights.RemoveLight(m_light);
				m_light = null;
			}
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			Rotate(rotationIndicator, rollIndicator);
		}

		void IMyCameraController.RotateStopped()
		{
			RotateStopped();
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
			TurnLightOff();
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			OnAssumeControl(previousCameraController);
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			OnReleaseControl(newCameraController);
		}

		bool IMyCameraController.HandleUse()
		{
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		public MatrixD? GetOverridingFocusMatrix()
		{
			return null;
		}
	}
}
