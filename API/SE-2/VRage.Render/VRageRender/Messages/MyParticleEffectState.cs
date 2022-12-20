using VRageMath;

namespace VRageRender.Messages
{
	public struct MyParticleEffectState
	{
		public uint ID;

		public bool IsStopped;

		public bool IsSimulationPaused;

		public bool IsEmittingStopped;

		public bool InstantStop;

		public bool StopLights;

		public bool Dirty;

		public bool AnimDirty;

		public bool TransformDirty;

		public bool UserDirty;

		public MatrixD WorldMatrix;

		public uint ParentID;

		public float UserRadiusMultiplier;

		public float UserBirthMultiplier;

		public float UserFadeMultiplier;

		public float UserVelocityMultiplier;

		public float UserColorIntensityMultiplier;

		public float UserLifeMultiplier;

		public float CameraSoftRadiusMultiplier;

		public float SoftParticleDistanceScaleMultiplier;

		public float UserScale;

		public Vector4 UserColorMultiplier;

		public Vector3? Velocity;

		public float Timer;

		public bool Autodelete;

		public float ElapsedTime;
	}
}
