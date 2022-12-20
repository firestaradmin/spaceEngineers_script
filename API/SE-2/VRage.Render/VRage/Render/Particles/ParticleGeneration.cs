using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRageRender.Animations;

namespace VRage.Render.Particles
{
	[ProtoContract]
	[XmlType("ParticleGeneration")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class ParticleGeneration
	{
		protected class VRage_Render_Particles_ParticleGeneration_003C_003EName_003C_003EAccessor : IMemberAccessor<ParticleGeneration, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleGeneration owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleGeneration owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Render_Particles_ParticleGeneration_003C_003EVersion_003C_003EAccessor : IMemberAccessor<ParticleGeneration, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleGeneration owner, in int value)
			{
				owner.Version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleGeneration owner, out int value)
			{
				value = owner.Version;
			}
		}

		protected class VRage_Render_Particles_ParticleGeneration_003C_003EGenerationType_003C_003EAccessor : IMemberAccessor<ParticleGeneration, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleGeneration owner, in string value)
			{
				owner.GenerationType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleGeneration owner, out string value)
			{
				value = owner.GenerationType;
			}
		}

		protected class VRage_Render_Particles_ParticleGeneration_003C_003EProperties_003C_003EAccessor : IMemberAccessor<ParticleGeneration, List<GenerationProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleGeneration owner, in List<GenerationProperty> value)
			{
				owner.Properties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleGeneration owner, out List<GenerationProperty> value)
			{
				value = owner.Properties;
			}
		}

		protected class VRage_Render_Particles_ParticleGeneration_003C_003EEmitter_003C_003EAccessor : IMemberAccessor<ParticleGeneration, ParticleEmitter>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleGeneration owner, in ParticleEmitter value)
			{
				owner.Emitter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleGeneration owner, out ParticleEmitter value)
			{
				value = owner.Emitter;
			}
		}

		[ProtoMember(37)]
		[XmlAttribute("Name")]
		public string Name = "";

		[ProtoMember(40)]
		[XmlAttribute("Version")]
		public int Version;

		[ProtoMember(43)]
		public string GenerationType = "CPU";

		[ProtoMember(46)]
		public List<GenerationProperty> Properties;

		[ProtoMember(49)]
		public ParticleEmitter Emitter;
	}
}
