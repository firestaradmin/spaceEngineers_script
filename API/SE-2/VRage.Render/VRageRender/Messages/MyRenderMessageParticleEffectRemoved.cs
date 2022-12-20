namespace VRageRender.Messages
{
	public class MyRenderMessageParticleEffectRemoved : MyRenderMessageBase
	{
		public uint Id;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.ParticleEffectRemoved;
	}
}
