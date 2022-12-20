using System.Runtime.InteropServices;
using VRageMath;

namespace VRage.Render.Particles
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyGPUEmitterLayoutData
	{
		public Vector4 Color0;

		public Vector4 Color1;

		public Vector4 Color2;

		public Vector4 Color3;

		public float ColorKey1;

		public float ColorKey2;

		public float AmbientFactor;

		public float Scale;

		public float Intensity0;

		public float Intensity1;

		public float Intensity2;

		public float Intensity3;

		public float IntensityKey1;

		public float IntensityKey2;

		public float AccelerationKey1;

		public float AccelerationKey2;

		public Vector3 AccelerationVector;

		public float RadiusVar;

		public Vector3 EmitterSize;

		public float EmitterSizeMin;

		public Vector3 Direction;

		public float Velocity;

		public float VelocityVar;

		public float DirectionInnerCone;

		public float DirectionConeVar;

		public float RotationVelocityVar;

		public float Acceleration0;

		public float Acceleration1;

		public float Acceleration2;

		public float Acceleration3;

		public Vector3 Gravity;

		public float Bounciness;

		public float ParticleSize0;

		public float ParticleSize1;

		public float ParticleSize2;

		public float ParticleSize3;

		public float ParticleSizeKeys1;

		public float ParticleSizeKeys2;

		public int NumParticlesToEmitThisFrame;

		public float ParticleLifeSpan;

		public float SoftParticleDistanceScale;

		public float StreakMultiplier;

		public GPUEmitterFlags Flags;

		public uint TextureIndex1;

		public uint TextureIndex2;

		public float AnimationFrameTime;

		public float HueVar;

		public float OITWeightFactor;

		public Matrix3x3 Rotation;

		public Vector3 Position;

		public Vector3 PositionDelta;

		public float MotionInheritance;

		public Vector3 Angle;

		public float ParticleLifeSpanVar;

		public Vector3 AngleVar;

		public float RotationVelocity;

		public float ParticleThickness0;

		public float ParticleThickness1;

		public float ParticleThickness2;

		public float ParticleThickness3;

		public float ParticleThicknessKeys1;

		public float ParticleThicknessKeys2;

		public float EmissivityKeys1;

		public float EmissivityKeys2;

		public float Emissivity0;

		public float Emissivity1;

		public float Emissivity2;

		public float Emissivity3;

		public float RotationVelocityCollisionMultiplier;

		public uint CollisionCountToKill;

		public float DistanceScalingFactor;

		public float ShadowAlphaMultiplier;

		public float CameraSoftRadius;

		public float UserRadiusMultiplier;

		public float UserVelocityMultiplier;

		public float UserColorIntensityMultiplier;

		public float UserLifeMultiplier;

		public float UserFadeMultiplier;

		public float _padding1;

		public float _padding2;
	}
}
