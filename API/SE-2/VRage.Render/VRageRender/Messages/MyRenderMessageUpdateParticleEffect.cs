namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateParticleEffect : MyRenderMessageBase
	{
		public MyParticleEffectState State;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateParticleEffect;
	}
}
