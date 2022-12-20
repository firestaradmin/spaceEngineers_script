namespace VRage.Render.Particles
{
	public interface IParticleManager
	{
		void RemoveParticleEffects(MyParticleEffectData effectData);

		void RecreateParticleEffects(MyParticleEffectData effectData);
	}
}
