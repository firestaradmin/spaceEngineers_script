namespace VRageRender.Messages
{
	public class MyRenderMessageCreateScreenDecal : MyRenderMessageBase
	{
		public uint ID;

		public uint[] ParentIDs;

		public MyDecalTopoData TopoData;

		public MyDecalFlags Flags;

		public bool IsTrail;

		public string SourceTarget;

		public string Material;

		public int MaterialIndex;

<<<<<<< HEAD
		public int TimeUntilLive = -1;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float RenderSqDistance;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateScreenDecal;
	}
}
