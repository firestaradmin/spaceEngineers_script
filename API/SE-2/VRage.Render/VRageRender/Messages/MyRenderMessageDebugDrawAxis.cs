using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawAxis : MyDebugRenderMessage
	{
		public MatrixD Matrix;

		public float AxisLength;

		public bool DepthRead;

		public bool SkipScale;

		public Color? CustomColor;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawAxis;
	}
}
