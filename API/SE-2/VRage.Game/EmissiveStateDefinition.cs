using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;

[ProtoContract]
public struct EmissiveStateDefinition
{
	protected class _EmissiveStateDefinition_003C_003EStateName_003C_003EAccessor : IMemberAccessor<EmissiveStateDefinition, string>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Set(ref EmissiveStateDefinition owner, in string value)
		{
			owner.StateName = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Get(ref EmissiveStateDefinition owner, out string value)
		{
			value = owner.StateName;
		}
	}

	protected class _EmissiveStateDefinition_003C_003EEmissiveColorName_003C_003EAccessor : IMemberAccessor<EmissiveStateDefinition, string>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Set(ref EmissiveStateDefinition owner, in string value)
		{
			owner.EmissiveColorName = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Get(ref EmissiveStateDefinition owner, out string value)
		{
			value = owner.EmissiveColorName;
		}
	}

	protected class _EmissiveStateDefinition_003C_003EDisplayColorName_003C_003EAccessor : IMemberAccessor<EmissiveStateDefinition, string>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Set(ref EmissiveStateDefinition owner, in string value)
		{
			owner.DisplayColorName = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Get(ref EmissiveStateDefinition owner, out string value)
		{
			value = owner.DisplayColorName;
		}
	}

	protected class _EmissiveStateDefinition_003C_003EEmissivity_003C_003EAccessor : IMemberAccessor<EmissiveStateDefinition, float>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Set(ref EmissiveStateDefinition owner, in float value)
		{
			owner.Emissivity = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override void Get(ref EmissiveStateDefinition owner, out float value)
		{
			value = owner.Emissivity;
		}
	}

	private class _EmissiveStateDefinition_003C_003EActor : IActivator, IActivator<EmissiveStateDefinition>
	{
		private sealed override object CreateInstance()
		{
			return default(EmissiveStateDefinition);
		}

		object IActivator.CreateInstance()
		{
			//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
			return this.CreateInstance();
		}

		private sealed override EmissiveStateDefinition CreateInstance()
		{
			return (EmissiveStateDefinition)(object)default(EmissiveStateDefinition);
		}

		EmissiveStateDefinition IActivator<EmissiveStateDefinition>.CreateInstance()
		{
			//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
			return this.CreateInstance();
		}
	}

	[ProtoMember(4)]
	[XmlAttribute("StateName")]
	public string StateName;

	[ProtoMember(7)]
	[XmlAttribute("EmissiveColorName")]
	public string EmissiveColorName;

	[ProtoMember(10)]
	[XmlAttribute("DisplayColorName")]
	public string DisplayColorName;

	[ProtoMember(13)]
	[XmlAttribute("Emissivity")]
	public float Emissivity;
}
