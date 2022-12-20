using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRageRender.Animations;

namespace VRage.Render.Particles
{
	[ProtoContract]
	public class ParticleEmitter
	{
		protected class VRage_Render_Particles_ParticleEmitter_003C_003EVersion_003C_003EAccessor : IMemberAccessor<ParticleEmitter, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleEmitter owner, in int value)
			{
				owner.Version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleEmitter owner, out int value)
			{
				value = owner.Version;
			}
		}

		protected class VRage_Render_Particles_ParticleEmitter_003C_003EProperties_003C_003EAccessor : IMemberAccessor<ParticleEmitter, List<GenerationProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleEmitter owner, in List<GenerationProperty> value)
			{
				owner.Properties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleEmitter owner, out List<GenerationProperty> value)
			{
				value = owner.Properties;
			}
		}

		[ProtoMember(52)]
		[XmlAttribute("Version")]
		public int Version;

		[ProtoMember(55)]
		public List<GenerationProperty> Properties;
	}
}
