using VRage.Render.Particles;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateParticleEffect : MyRenderMessageBase
	{
		public uint ID;

		public MyParticleEffectData Data;

		public string DebugName;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateParticleEffect;
	}
}
