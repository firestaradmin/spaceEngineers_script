using VRageMath;
using VRageRender.Import;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateRenderEntity : MyRenderMessageBase
	{
		public uint ID;

		public string DebugName;

		public string Model;

		public MatrixD WorldMatrix;

		public MyMeshDrawTechnique Technique;

		public RenderFlags Flags;

		public int DepthBias;

		public CullingOptions CullingOptions;

		public float MaxViewDistance;

		public float Rescale = 1f;

		public bool ForceReload;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateRenderEntity;

		public override void Close()
		{
			base.Close();
			Model = null;
			DebugName = null;
		}

		public override string ToString()
		{
			return DebugName ?? (string.Empty + ", " + Model);
		}
	}
}
