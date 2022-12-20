using System;
using VRage.Utils;
using VRageMath;

namespace VRage
{
	public class MySpectator
	{
		public static MySpectator Static;

		public const float DEFAULT_SPECTATOR_LINEAR_SPEED = 0.1f;

		public const float MIN_SPECTATOR_LINEAR_SPEED = 0.0001f;

		public const float MAX_SPECTATOR_LINEAR_SPEED = 8000f;

		public const float DEFAULT_SPECTATOR_ANGULAR_SPEED = 1f;

		public const float MIN_SPECTATOR_ANGULAR_SPEED = 0.0001f;

		public const float MAX_SPECTATOR_ANGULAR_SPEED = 6f;

		public Vector3D ThirdPersonCameraDelta = new Vector3D(-10.0, 10.0, -10.0);

		private MySpectatorCameraMovementEnum m_spectatorCameraMovement;

		private Vector3D m_position;

		private Vector3D m_targetDelta = Vector3D.Forward;

		private Vector3D? m_up;

		protected float m_speedModeLinear = 0.1f;

		protected float m_speedModeAngular = 1f;

		protected MatrixD m_orientation = MatrixD.Identity;

		protected bool m_orientationDirty = true;

		protected float m_orbitY;

		protected float m_orbitX;

		public MySpectatorCameraMovementEnum SpectatorCameraMovement
		{
			get
			{
				return m_spectatorCameraMovement;
			}
			set
			{
				if (m_spectatorCameraMovement != value)
				{
					OnChangingMode(m_spectatorCameraMovement, value);
				}
				_ = m_spectatorCameraMovement;
				m_spectatorCameraMovement = value;
				this.OnModeChanged.InvokeIfNotNull(m_spectatorCameraMovement, value);
			}
		}

		public bool IsInFirstPersonView { get; set; }

		public bool ForceFirstPersonCamera { get; set; }

		public bool Initialized { get; set; }

		public Vector3D Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_position = value;
			}
		}

		public float SpeedModeLinear
		{
			get
			{
				return m_speedModeLinear;
			}
			set
			{
				m_speedModeLinear = value;
			}
		}

		public float SpeedModeAngular
		{
			get
			{
				return m_speedModeAngular;
			}
			set
			{
				m_speedModeAngular = value;
			}
		}

		public Vector3D Target
		{
			get
			{
				return Position + m_targetDelta;
			}
			set
			{
				Vector3D vector3D = value - Position;
<<<<<<< HEAD
				m_orientationDirty |= (m_targetDelta - vector3D).IsZero();
=======
				m_orientationDirty = m_targetDelta != vector3D;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_targetDelta = vector3D;
				m_up = null;
			}
		}

		public MatrixD Orientation
		{
			get
			{
				if (m_orientationDirty)
				{
					UpdateOrientation();
					m_orientationDirty = false;
				}
				return m_orientation;
			}
		}

		public event Action<MySpectatorCameraMovementEnum, MySpectatorCameraMovementEnum> OnModeChanged;

		protected virtual void OnChangingMode(MySpectatorCameraMovementEnum oldMode, MySpectatorCameraMovementEnum newMode)
		{
		}

		public MySpectator()
		{
			Static = this;
		}

		public void SetTarget(Vector3D target, Vector3D? up)
		{
			Target = target;
			m_orientationDirty |= m_up != up;
			m_up = up;
		}

		public void UpdateOrientation()
		{
			Vector3D vector3D = MyUtils.Normalize(m_targetDelta);
			vector3D = ((vector3D.LengthSquared() > 0.0) ? vector3D : Vector3D.Forward);
			m_orientation = MatrixD.CreateFromDir(vector3D, m_up.HasValue ? m_up.Value : Vector3D.Up);
		}

		public void Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(Vector3.Zero, rotationIndicator, rollIndicator);
		}

		public void RotateStopped()
		{
			MoveAndRotateStopped();
		}

		public virtual void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			_ = Position;
			moveIndicator *= m_speedModeLinear;
			float num = 0.1f;
			float num2 = 0.0025f * m_speedModeAngular;
			Vector3D position = (Vector3D)moveIndicator * (double)num;
			switch (SpectatorCameraMovement)
			{
			case MySpectatorCameraMovementEnum.UserControlled:
				if (rollIndicator != 0f)
				{
					float value = rollIndicator * m_speedModeLinear * 0.1f;
					value = MathHelper.Clamp(value, -0.02f, 0.02f);
					MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Right, out var xOut, out var yOut, value);
					m_orientation.Right = yOut;
					m_orientation.Up = xOut;
				}
				if (rotationIndicator.X != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Forward, out var xOut2, out var yOut2, rotationIndicator.X * num2);
					m_orientation.Up = xOut2;
					m_orientation.Forward = yOut2;
				}
				if (rotationIndicator.Y != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Right, m_orientation.Forward, out var xOut3, out var yOut3, (0f - rotationIndicator.Y) * num2);
					m_orientation.Right = xOut3;
					m_orientation.Forward = yOut3;
				}
				Position += Vector3D.Transform(position, m_orientation);
				break;
			case MySpectatorCameraMovementEnum.Orbit:
			{
				m_orbitY += rotationIndicator.Y * 0.01f;
				m_orbitX += rotationIndicator.X * 0.01f;
				Vector3D position4 = -m_targetDelta;
				Vector3D vector3D2 = Position + m_targetDelta;
				MatrixD m = Orientation;
				Matrix m2 = Matrix.Invert(m);
				MatrixD matrix2 = m2;
				Vector3D position5 = Vector3D.Transform(position4, matrix2);
				rotationIndicator *= 0.01f;
				MatrixD matrixD2 = MatrixD.CreateRotationX(m_orbitX) * MatrixD.CreateRotationY(m_orbitY) * MatrixD.CreateRotationZ(rollIndicator);
				position4 = Vector3D.Transform(position5, matrixD2);
				Position = vector3D2 + position4;
				m_targetDelta = -position4;
				Vector3D vector3D3 = m_orientation.Right * position.X + m_orientation.Up * position.Y;
				Position += vector3D3;
				Vector3D vector3D4 = m_orientation.Forward * (0.0 - position.Z);
				Position += vector3D4;
				m_targetDelta -= vector3D4;
				m_orientation = matrixD2;
				break;
			}
			case MySpectatorCameraMovementEnum.ConstantDelta:
			{
				m_orbitY += rotationIndicator.Y * 0.01f;
				m_orbitX += rotationIndicator.X * 0.01f;
				Vector3D position2 = -m_targetDelta;
				Vector3D vector3D = Position + m_targetDelta;
				MatrixD m = Orientation;
				Matrix m2 = Matrix.Invert(m);
				MatrixD matrix = m2;
				Vector3D position3 = Vector3D.Transform(position2, matrix);
				rotationIndicator *= 0.01f;
				MatrixD matrixD = MatrixD.CreateRotationX(m_orbitX) * MatrixD.CreateRotationY(m_orbitY) * MatrixD.CreateRotationZ(rollIndicator);
				position2 = Vector3D.Transform(position3, matrixD);
				Position = vector3D + position2;
				m_targetDelta = -position2;
				m_orientation = matrixD;
				break;
			}
			case MySpectatorCameraMovementEnum.FreeMouse:
			case MySpectatorCameraMovementEnum.None:
				break;
			}
		}

		public void UpdateCameraPosition(Vector2 rotationIndicator)
		{
			m_orbitY += rotationIndicator.Y * 0.01f;
			m_orbitX += rotationIndicator.X * 0.01f;
			Vector3D position = -m_targetDelta;
			Vector3D vector3D = Position + m_targetDelta;
			MatrixD m = Orientation;
			Matrix m2 = Matrix.Invert(m);
			MatrixD matrix = m2;
			Vector3D position2 = Vector3D.Transform(position, matrix);
			MatrixD matrixD = MatrixD.CreateRotationX(m_orbitX) * MatrixD.CreateRotationY(m_orbitY);
			position = Vector3D.Transform(position2, matrixD);
			Position = vector3D + position;
			m_targetDelta = -position;
			m_orientation = matrixD;
			UpdateOrientation();
		}

		public virtual void Update()
		{
		}

		public virtual void MoveAndRotateStopped()
		{
		}

		public MatrixD GetViewMatrix()
		{
			return MatrixD.Invert(MatrixD.CreateWorld(Position, Orientation.Forward, Orientation.Up));
		}

		public void SetViewMatrix(MatrixD viewMatrix)
		{
			MatrixD matrixD = MatrixD.Invert(viewMatrix);
			Position = matrixD.Translation;
			m_orientation = MatrixD.Identity;
			m_orientation.Right = matrixD.Right;
			m_orientation.Up = matrixD.Up;
			m_orientation.Forward = matrixD.Forward;
			m_orientationDirty = false;
		}

		public void Reset()
		{
			m_position = Vector3.Zero;
			m_targetDelta = Vector3.Forward;
			ThirdPersonCameraDelta = new Vector3D(-10.0, 10.0, -10.0);
			m_orientationDirty = true;
			m_orbitX = 0f;
			m_orbitY = 0f;
		}
	}
}
