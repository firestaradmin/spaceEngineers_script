using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
	public class MyBotNavigation
	{
		private readonly List<MySteeringBase> m_steerings;

		private readonly MyPathSteering m_pathSteering;

		private readonly MyBotAiming m_aiming;

		private readonly MyDestinationSphere m_destinationSphere;

		private Vector3 m_forwardVector;

		private Vector3 m_correction;

		private Vector3 m_upVector;

		private bool m_wasStopped;

		private float m_rotationSpeedModifier;

		private float? m_maximumRotationAngle;

		private readonly MyStuckDetection m_stuckDetection;

		private MatrixD m_worldMatrix;

		private MatrixD m_invWorldMatrix;

		private MatrixD m_aimingPositionAndOrientation;

		private MatrixD m_invAimingPositionAndOrientation;

		public int WaitForClearPathCountdown;

<<<<<<< HEAD
		/// <summary>
		/// Current wanted forward vector
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Vector3 ForwardVector => m_forwardVector;

		/// <summary>
		/// Current wanted up vector
		/// </summary>
		public Vector3 UpVector => m_upVector;

<<<<<<< HEAD
		/// <summary>
		/// Current wanted Speed
		/// </summary>
		public float Speed { get; private set; }

		/// <summary>
		/// Whether the navigation is moving the bot towards a target. Beware: the bot could still be stuck
		/// </summary>
=======
		public float Speed { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool Navigating => m_pathSteering.TargetSet;

		public bool IsWaitingForTileGeneration => m_pathSteering.IsWaitingForTileGeneration;

		public bool Stuck => m_stuckDetection.IsStuck;

		public Vector3D TargetPoint => m_destinationSphere.GetDestination();

		public MyEntity BotEntity { get; private set; }

		public float? MaximumRotationAngle
		{
			get
			{
				return m_maximumRotationAngle;
			}
			set
			{
				m_maximumRotationAngle = value;
			}
		}

		public Vector3 GravityDirection { get; private set; }

		/// <summary>
		/// Current position and orientation of the controlled entity
		/// </summary>
		public MatrixD PositionAndOrientation
		{
			get
			{
				if (BotEntity == null)
				{
					return MatrixD.Identity;
				}
				return m_worldMatrix;
			}
		}

		public MatrixD PositionAndOrientationInverted
		{
			get
			{
				if (BotEntity == null)
				{
					return MatrixD.Identity;
				}
				return m_invWorldMatrix;
			}
		}

		public MatrixD AimingPositionAndOrientation
		{
			get
			{
				if (BotEntity == null)
				{
					return MatrixD.Identity;
				}
				return m_aimingPositionAndOrientation;
			}
		}

		public MatrixD AimingPositionAndOrientationInverted
		{
			get
			{
				if (BotEntity == null)
				{
					return MatrixD.Identity;
				}
				return m_invAimingPositionAndOrientation;
			}
		}

		public bool HasRotation(float epsilon = 0.0316f)
		{
			return m_aiming.RotationHint.LengthSquared() > epsilon * epsilon;
		}

		public bool HasXRotation(float epsilon)
		{
			return Math.Abs(m_aiming.RotationHint.Y) > epsilon;
		}

		public bool HasYRotation(float epsilon)
		{
			return Math.Abs(m_aiming.RotationHint.X) > epsilon;
		}

		public MyBotNavigation(MyPlayer player)
		{
			m_steerings = new List<MySteeringBase>();
			m_pathSteering = new MyPathSteering(this);
			m_steerings.Add(m_pathSteering);
			if (player.Identity.Model.Contains("Wolf"))
			{
				m_steerings.Add(new MyForwardCollisionAvoidanceSteering(this));
			}
			else
			{
				m_steerings.Add(new MyCharacterAvoidanceSteering(this, 0.06f));
			}
			m_aiming = new MyBotAiming(this);
			m_stuckDetection = new MyStuckDetection(0.05f, MathHelper.ToRadians(1f));
			m_destinationSphere = new MyDestinationSphere(ref Vector3D.Zero, 0f);
			m_wasStopped = false;
		}

		public void Cleanup()
		{
			foreach (MySteeringBase steering in m_steerings)
			{
				steering.Cleanup();
			}
		}

		public void ChangeEntity(IMyControllableEntity newEntity)
		{
			BotEntity = newEntity?.Entity;
			if (BotEntity != null)
			{
				m_forwardVector = PositionAndOrientation.Forward;
				m_upVector = PositionAndOrientation.Up;
				Speed = 0f;
				m_rotationSpeedModifier = 1f;
			}
		}

		public void Update(int behaviorTicks)
		{
			m_stuckDetection.SetCurrentTicks(behaviorTicks);
			if (BotEntity == null)
			{
				return;
			}
			UpdateMatrices();
			GravityDirection = MyGravityProviderSystem.CalculateTotalGravityInPoint(BotEntity.PositionComp.WorldMatrixRef.Translation);
			if (!Vector3.IsZero(GravityDirection, 0.01f))
			{
<<<<<<< HEAD
				GravityDirection = Vector3.Normalize(GravityDirection);
=======
				GravityDirection = Vector3D.Normalize(GravityDirection);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
			{
				m_upVector = Vector3.Up;
			}
			else
			{
				m_upVector = -GravityDirection;
			}
			if (!Speed.IsValid())
			{
				m_forwardVector = PositionAndOrientation.Forward;
				Speed = 0f;
				m_rotationSpeedModifier = 1f;
			}
			foreach (MySteeringBase steering in m_steerings)
			{
				steering.Update();
			}
			m_aiming.Update();
			CorrectMovement(m_aiming.RotationHint);
			if (Speed < 0.1f)
			{
				Speed = 0f;
			}
			MoveCharacter();
		}

		private void UpdateMatrices()
		{
			MyCharacter myCharacter;
			if ((myCharacter = BotEntity as MyCharacter) != null)
			{
				m_worldMatrix = myCharacter.WorldMatrix;
<<<<<<< HEAD
				m_invWorldMatrix = MatrixD.Invert(m_worldMatrix);
=======
				Matrix m = Matrix.Invert(m_worldMatrix);
				m_invWorldMatrix = m;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_aimingPositionAndOrientation = myCharacter.GetHeadMatrix(includeY: true, includeX: true, forceHeadAnim: false, forceHeadBone: true);
				m_invAimingPositionAndOrientation = MatrixD.Invert(m_aimingPositionAndOrientation);
			}
			else
			{
				m_worldMatrix = BotEntity.PositionComp.WorldMatrixRef;
				m_invWorldMatrix = BotEntity.PositionComp.WorldMatrixInvScaled;
				m_aimingPositionAndOrientation = m_worldMatrix;
				m_invAimingPositionAndOrientation = m_invWorldMatrix;
			}
		}

		private void AccumulateCorrection()
		{
			m_rotationSpeedModifier = 1f;
			float weight = 0f;
			for (int i = 0; i < m_steerings.Count; i++)
			{
				m_steerings[i].AccumulateCorrection(ref m_correction, ref weight);
			}
			if (m_maximumRotationAngle.HasValue)
			{
				double num = Math.Cos(m_maximumRotationAngle.Value);
				double num2 = Vector3D.Dot(Vector3D.Normalize(m_forwardVector - m_correction), m_forwardVector);
				if (num2 < num)
				{
					float num3 = (float)Math.Acos(MathHelper.Clamp(num2, -1.0, 1.0));
					m_rotationSpeedModifier = num3 / m_maximumRotationAngle.Value;
					m_correction /= m_rotationSpeedModifier;
				}
			}
			if (weight > 1f)
			{
				m_correction /= weight;
			}
		}

		private void CorrectMovement(Vector3 rotationHint)
		{
			m_correction = Vector3.Zero;
			if (!Navigating)
			{
				Speed = 0f;
				return;
			}
			AccumulateCorrection();
			if (HasRotation(10f))
			{
				m_correction = Vector3.Zero;
				Speed = 0f;
				m_stuckDetection.SetRotating(rotating: true);
			}
			else
			{
				m_stuckDetection.SetRotating(rotating: false);
			}
			Vector3 vector = m_forwardVector * Speed;
			vector += m_correction;
			Speed = vector.Length();
			if (Speed <= 0.001f)
			{
				Speed = 0f;
				return;
			}
			m_forwardVector = vector / Speed;
			if (Speed > 1f)
			{
				Speed = 1f;
			}
		}

		private void MoveCharacter()
		{
			if (WaitForClearPathCountdown > 0)
			{
				WaitForClearPathCountdown--;
				Speed = 0f;
			}
			MyCharacter myCharacter;
			if ((myCharacter = BotEntity as MyCharacter) != null)
			{
				if (Speed != 0f)
				{
					MyCharacterJetpackComponent jetpackComp = myCharacter.JetpackComp;
					if (jetpackComp != null && !jetpackComp.TurnedOn && m_pathSteering.Flying)
					{
						jetpackComp.TurnOnJetpack(newState: true);
					}
					else if (jetpackComp != null && jetpackComp.TurnedOn && !m_pathSteering.Flying)
					{
						jetpackComp.TurnOnJetpack(newState: false);
					}
					Vector3 vector = Vector3.TransformNormal(m_forwardVector, myCharacter.PositionComp.WorldMatrixNormalizedInv);
					Vector3 vector2 = m_aiming.RotationHint * m_rotationSpeedModifier;
					if (m_pathSteering.Flying)
					{
						if (vector.Y > 0f)
						{
							myCharacter.Up();
						}
						else
						{
							myCharacter.Down();
						}
					}
					if (!(vector.Y > 0.7f))
					{
						myCharacter.MoveAndRotate(vector * Speed, new Vector2(vector2.X * 30f, vector2.Y * 30f), 0f);
					}
				}
				else if (Speed == 0f)
				{
					if (HasRotation())
					{
						float num = ((myCharacter.WantsWalk || myCharacter.IsCrouching) ? 1 : 2);
						Vector3 vector3 = m_aiming.RotationHint * m_rotationSpeedModifier;
						myCharacter.MoveAndRotate(Vector3.Zero, new Vector2(vector3.X * 20f * num, vector3.Y * 25f * num), 0f);
						m_wasStopped = false;
					}
					else if (m_wasStopped)
					{
						myCharacter.MoveAndRotate(Vector3.Zero, Vector2.Zero, 0f);
						m_wasStopped = true;
					}
				}
			}
			if (WaitForClearPathCountdown <= 0)
			{
				m_stuckDetection.Update(m_worldMatrix.Translation, m_aiming.RotationHint);
			}
		}

		public void AddSteering(MySteeringBase steering)
		{
			m_steerings.Add(steering);
		}

		public void RemoveSteering(MySteeringBase steering)
		{
			m_steerings.Remove(steering);
		}

		public bool HasSteeringOfType(Type steeringType)
		{
			foreach (MySteeringBase steering in m_steerings)
			{
				if (steering.GetType() == steeringType)
				{
					return true;
				}
			}
			return false;
		}

		public MySteeringBase GetSteeringOfType(Type steeringType)
		{
			foreach (MySteeringBase steering in m_steerings)
			{
				if (steering.GetType() == steeringType)
				{
					return steering;
				}
			}
			return null;
		}

		public void Goto(Vector3D position, float radius = 0f, MyEntity relativeEntity = null)
		{
			m_destinationSphere.Init(ref position, radius);
			Goto(m_destinationSphere, relativeEntity);
		}

		/// <summary>
		/// Tells the bot to go to the given world coordinate.
		/// If the relative entity is set, the coordinate is updated automatically as the entity moves
		/// </summary>
		public void Goto(IMyDestinationShape destination, MyEntity relativeEntity = null)
		{
			if (MyAIComponent.Static.Pathfinding != null)
			{
				IMyPath myPath = MyAIComponent.Static.Pathfinding.FindPathGlobal(PositionAndOrientation.Translation, destination, relativeEntity);
				if (myPath == null)
				{
					m_pathSteering.UnsetPath();
					return;
				}
				m_pathSteering.SetPath(myPath);
				m_stuckDetection.Reset();
			}
		}

		public void GotoNoPath(Vector3D worldPosition, float radius = 0f, MyEntity relativeEntity = null, bool resetStuckDetection = true)
		{
			m_pathSteering.SetTarget(worldPosition, radius, relativeEntity);
			if (resetStuckDetection)
			{
				m_stuckDetection.Reset();
			}
		}

		public bool CheckReachability(Vector3D worldPosition, float threshold, MyEntity relativeEntity = null)
		{
			if (MyAIComponent.Static.Pathfinding == null)
			{
				return false;
			}
			m_destinationSphere.Init(ref worldPosition, 0f);
			return MyAIComponent.Static.Pathfinding.ReachableUnderThreshold(PositionAndOrientation.Translation, m_destinationSphere, threshold);
		}

<<<<<<< HEAD
		/// <summary>
		/// Tells the bot to fly to the given world coordinate.
		/// If the relative entity is set, the coordinate is updated automatically as the entity moves
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void FlyTo(Vector3D worldPosition, MyEntity relativeEntity = null)
		{
			m_pathSteering.SetTarget(worldPosition, 1f, relativeEntity, 1f, fly: true);
			m_stuckDetection.Reset();
		}

		/// <summary>
		/// Stop the bot from moving.
		/// </summary>
		public void Stop()
		{
			if (!IsWaitingForTileGeneration)
			{
				m_pathSteering.UnsetPath();
			}
			m_stuckDetection.Stop();
		}

		public void StopImmediate(bool forceUpdate = false)
		{
			Stop();
			Speed = 0f;
			if (forceUpdate)
			{
				MoveCharacter();
			}
		}

		public void FollowPath(IMyPath path)
		{
			m_pathSteering.SetPath(path);
			m_stuckDetection.Reset();
		}

		public void AimAt(MyEntity entity, Vector3D? worldPosition = null)
		{
			if (worldPosition.HasValue)
			{
				if (entity != null)
				{
					MatrixD worldMatrixNormalizedInv = entity.PositionComp.WorldMatrixNormalizedInv;
					Vector3 value = Vector3D.Transform(worldPosition.Value, worldMatrixNormalizedInv);
					m_aiming.SetTarget(entity, value);
				}
				else
				{
					m_aiming.SetAbsoluteTarget(worldPosition.Value);
				}
			}
			else
			{
				m_aiming.SetTarget(entity);
			}
		}

		public void AimWithMovement()
		{
			m_aiming.FollowMovement();
		}

		public void StopAiming()
		{
			m_aiming.StopAiming();
		}

		[Conditional("DEBUG")]
		private void AssertIsValid()
		{
		}

		[Conditional("DEBUG")]
		public void DebugDraw()
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			m_aiming.DebugDraw(m_aimingPositionAndOrientation);
			if (MyDebugDrawSettings.DEBUG_DRAW_BOT_STEERING)
			{
				foreach (MySteeringBase steering in m_steerings)
				{
					_ = steering;
				}
			}
			if (!MyDebugDrawSettings.DEBUG_DRAW_BOT_NAVIGATION)
			{
				return;
			}
			Vector3D translation = PositionAndOrientation.Translation;
			Vector3.Cross(m_forwardVector, UpVector);
			if (Stuck)
			{
<<<<<<< HEAD
				MyRenderProxy.DebugDrawSphere(translation, 1f, Color.Red.ToVector3(), 1f, depthRead: false);
			}
			MyRenderProxy.DebugDrawArrow3D(translation, translation + ForwardVector, Color.Blue, Color.Blue, depthRead: false, 0.1, "Nav. FW");
			MyRenderProxy.DebugDrawArrow3D(translation + ForwardVector, translation + ForwardVector + m_correction, Color.LightBlue, Color.LightBlue, depthRead: false, 0.1, "Correction");
=======
				MyRenderProxy.DebugDrawSphere(vector, 1f, Color.Red.ToVector3(), 1f, depthRead: false);
			}
			MyRenderProxy.DebugDrawArrow3D(vector, vector + ForwardVector, Color.Blue, Color.Blue, depthRead: false, 0.1, "Nav. FW");
			MyRenderProxy.DebugDrawArrow3D(vector + ForwardVector, vector + ForwardVector + m_correction, Color.LightBlue, Color.LightBlue, depthRead: false, 0.1, "Correction");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_destinationSphere?.DebugDraw();
			MyCharacter myCharacter;
			if ((myCharacter = BotEntity as MyCharacter) != null)
			{
				MatrixD matrix = MatrixD.Invert(myCharacter.GetViewMatrix());
				MatrixD headMatrix = myCharacter.GetHeadMatrix(includeY: true);
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, Vector3D.Transform(Vector3D.Forward * 50.0, matrix), Color.Yellow, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(headMatrix.Translation, Vector3D.Transform(Vector3D.Forward * 50.0, headMatrix), Color.Red, Color.Red, depthRead: false);
				if (myCharacter.CurrentWeapon != null)
				{
<<<<<<< HEAD
					Vector3 vector = myCharacter.CurrentWeapon.DirectionToTarget(myCharacter.AimedPoint);
					Vector3D translation2 = (myCharacter.CurrentWeapon as MyEntity).WorldMatrix.Translation;
					MyRenderProxy.DebugDrawSphere(myCharacter.AimedPoint, 1f, Color.Yellow, 1f, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(translation2, translation2 + vector * 20f, Color.Purple, Color.Purple, depthRead: false);
=======
					Vector3 vector2 = myCharacter.CurrentWeapon.DirectionToTarget(myCharacter.AimedPoint);
					Vector3D translation = (myCharacter.CurrentWeapon as MyEntity).WorldMatrix.Translation;
					MyRenderProxy.DebugDrawSphere(myCharacter.AimedPoint, 1f, Color.Yellow, 1f, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(translation, translation + vector2 * 20f, Color.Purple, Color.Purple, depthRead: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}
	}
}
