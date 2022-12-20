using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public struct MyDecalRenderInfo
	{
		public MyDecalFlags Flags;

		public Vector3D Position;

		public Vector3 Normal;

		public Vector3 Forward;

		public Vector4UByte BoneIndices;

		public Vector4 BoneWeights;

		public MyDecalBindingInfo? Binding;

		public uint[] RenderObjectIds;

		public MyStringHash PhysicalMaterial;

		public MyStringHash Source;

		public MyStringHash VoxelMaterial;

<<<<<<< HEAD
		public int AliveUntil;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float RenderDistance;

		public bool IsTrail;
	}
}
