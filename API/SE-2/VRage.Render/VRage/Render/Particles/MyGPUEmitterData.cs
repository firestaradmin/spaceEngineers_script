using VRageMath;

namespace VRage.Render.Particles
{
	public struct MyGPUEmitterData
	{
		public float ParticlesPerSecond;

		public float ParticlesPerFrame;

		public string AtlasTexture;

		public Vector2I AtlasDimension;

		public int AtlasFrameOffset;

		public int AtlasFrameModulo;

		public Vector3D WorldPosition;

		public float CameraBias;

		public uint ParentID;

		public float GravityFactor;

		public float DistanceMaxSqr;

		public float PrioritySqr;

		public MyGPUEmitterLayoutData Data;
	}
}
