using System;
using System.Xml;

namespace VRageRender.Animations
{
	public interface IMyConstProperty
	{
		string Name { get; set; }

		string Description { get; }

		string ValueType { get; }

		string BaseValueType { get; }

		bool Animated { get; }

		bool Is2D { get; }

		bool IsDefault { get; }

		void Serialize(XmlWriter writer);

		void Deserialize(XmlReader reader);

		GenerationProperty SerializeToObjectBuilder();

		void DeserializeFromObjectBuilder(GenerationProperty property);

		void SerializeValue(XmlWriter writer, object value);

		void DeserializeValue(XmlReader reader, out object value);

		void SetValue(object val);

		void SetDefaultValue(object val);

		object GetValue();

		object GetDefaultValue();

		IMyConstProperty Duplicate();

		Type GetValueType();
	}
}
