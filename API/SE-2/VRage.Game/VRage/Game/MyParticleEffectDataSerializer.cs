using System.Collections.Generic;
using VRage.Render.Particles;
using VRage.Utils;

namespace VRage.Game
{
	public static class MyParticleEffectDataSerializer
	{
		public static void DeserializeFromObjectBuilder(MyParticleEffectData data, MyObjectBuilder_ParticleEffect builder)
		{
			data.Start(builder.ParticleId, builder.Id.SubtypeName);
			data.Length = builder.Length;
			data.Loop = builder.Loop;
			data.DurationMin = builder.DurationMin;
			data.DurationMax = builder.DurationMax;
			data.DistanceMax = builder.DistanceMax;
			data.Priority = builder.Priority;
<<<<<<< HEAD
			data.Enabled = builder.Enabled;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (ParticleGeneration particleGeneration in builder.ParticleGenerations)
			{
				string generationType = particleGeneration.GenerationType;
				if (!(generationType == "CPU"))
				{
					if (generationType == "GPU")
					{
						MyParticleGPUGenerationData myParticleGPUGenerationData = new MyParticleGPUGenerationData();
						myParticleGPUGenerationData.Start(data);
						myParticleGPUGenerationData.DeserializeFromObjectBuilder(particleGeneration);
						data.AddGeneration(myParticleGPUGenerationData, loading: true);
					}
				}
				else
				{
					MyLog.Default.WriteLine("CPU Particles are not supported anymore: " + data.Name + " / " + particleGeneration.Name);
				}
			}
			foreach (ParticleLight particleLight in builder.ParticleLights)
			{
				MyParticleLightData myParticleLightData = new MyParticleLightData();
				myParticleLightData.Start(data);
				myParticleLightData.DeserializeFromObjectBuilder(particleLight);
				data.AddParticleLight(myParticleLightData, loading: true);
			}
		}

		public static MyObjectBuilder_ParticleEffect SerializeToObjectBuilder(MyParticleEffectData data)
		{
			MyObjectBuilder_ParticleEffect myObjectBuilder_ParticleEffect = new MyObjectBuilder_ParticleEffect();
			myObjectBuilder_ParticleEffect.ParticleId = data.ID;
			myObjectBuilder_ParticleEffect.Id.TypeIdString = "MyObjectBuilder_ParticleEffect";
			myObjectBuilder_ParticleEffect.Id.SubtypeName = data.Name;
<<<<<<< HEAD
			myObjectBuilder_ParticleEffect.Enabled = data.Enabled;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myObjectBuilder_ParticleEffect.Length = data.Length;
			myObjectBuilder_ParticleEffect.Loop = data.Loop;
			myObjectBuilder_ParticleEffect.DurationMin = data.DurationMin;
			myObjectBuilder_ParticleEffect.DurationMax = data.DurationMax;
			myObjectBuilder_ParticleEffect.DistanceMax = data.DistanceMax;
			myObjectBuilder_ParticleEffect.ParticleGenerations = new List<ParticleGeneration>();
			foreach (MyParticleGPUGenerationData generation in data.GetGenerations())
			{
				ParticleGeneration item = generation.SerializeToObjectBuilder();
				myObjectBuilder_ParticleEffect.ParticleGenerations.Add(item);
			}
			myObjectBuilder_ParticleEffect.ParticleLights = new List<ParticleLight>();
			foreach (MyParticleLightData particleLight in data.GetParticleLights())
			{
				ParticleLight item2 = particleLight.SerializeToObjectBuilder();
				myObjectBuilder_ParticleEffect.ParticleLights.Add(item2);
			}
			return myObjectBuilder_ParticleEffect;
		}
	}
}
