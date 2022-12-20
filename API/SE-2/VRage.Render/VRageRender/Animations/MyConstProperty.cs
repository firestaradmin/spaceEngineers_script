using System;
using System.Collections.Generic;
using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyConstProperty<T> : IMyConstProperty
	{
		private string m_name;

		private string m_description;

		private T m_value;

		private T m_defaultValue;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public string Description => m_description;

		public virtual string ValueType => typeof(T).Name;

		public virtual string BaseValueType => ValueType;

		public virtual bool Animated => false;

		public virtual bool Is2D => false;

		public bool IsDefault => EqualityComparer<T>.Default.Equals(m_value, m_defaultValue);

		public MyConstProperty()
		{
			Init();
		}

		public MyConstProperty(string name, string description)
			: this()
		{
			m_name = name;
			m_description = description;
		}

		protected virtual void Init()
		{
		}

		object IMyConstProperty.GetValue()
		{
			return m_value;
		}

		object IMyConstProperty.GetDefaultValue()
<<<<<<< HEAD
		{
			return m_defaultValue;
		}

		public T GetValue()
		{
			return m_value;
		}

		public T GetDefaultValue()
		{
			return m_defaultValue;
=======
		{
			return m_defaultValue;
		}

		public T GetValue()
		{
			return m_value;
		}

		public T GetDefaultValue()
		{
			return m_defaultValue;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public virtual void SetValue(object val)
		{
			SetValue((T)val);
		}

		public virtual void SetDefaultValue(object val)
		{
			SetDefaultValue((T)val);
		}

		public void SetValue(T val)
		{
			m_value = val;
		}

		public void SetDefaultValue(T val)
		{
			m_value = val;
			m_defaultValue = val;
		}

		public virtual IMyConstProperty Duplicate()
		{
			return null;
		}

		protected virtual void Duplicate(IMyConstProperty targetProp)
		{
			targetProp.SetDefaultValue(GetDefaultValue());
			targetProp.SetValue(GetValue());
		}

		Type IMyConstProperty.GetValueType()
		{
			return GetValueTypeInternal();
		}

		protected virtual Type GetValueTypeInternal()
		{
			return typeof(T);
		}

		public virtual void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Value" + ValueType);
			SerializeValue(writer, m_value);
			writer.WriteEndElement();
		}

		public virtual void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			reader.ReadStartElement();
			DeserializeValue(reader, out var value);
			m_value = (T)value;
			reader.ReadEndElement();
		}

		public virtual GenerationProperty SerializeToObjectBuilder()
<<<<<<< HEAD
		{
			GenerationProperty generationProperty = new GenerationProperty();
			generationProperty.Name = Name;
			generationProperty.Type = ValueType;
			generationProperty.AnimationType = PropertyAnimationType.Const;
			switch (m_value.GetType().Name)
			{
			case "Single":
			case "Float":
				generationProperty.Type = "Float";
				generationProperty.ValueFloat = (float)(object)m_value;
				break;
			case "Vector3":
				generationProperty.Type = "Vector3";
				generationProperty.ValueVector3 = (Vector3)(object)m_value;
				break;
			case "Vector4":
				generationProperty.Type = "Vector4";
				generationProperty.ValueVector4 = (Vector4)(object)m_value;
				break;
			default:
				generationProperty.Type = "Int";
				generationProperty.ValueInt = (int)(object)m_value;
				break;
			case "Boolean":
				generationProperty.Type = "Bool";
				generationProperty.ValueBool = (bool)(object)m_value;
				break;
			case "String":
				generationProperty.Type = "String";
				generationProperty.ValueString = (string)(object)m_value;
				break;
			case "MyTransparentMaterial":
				generationProperty.Type = "MyTransparentMaterial";
				generationProperty.ValueString = ((MyTransparentMaterial)(object)m_value).Id.String;
				break;
			}
			return generationProperty;
		}

		public virtual void DeserializeFromObjectBuilder(GenerationProperty property)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			GenerationProperty generationProperty = new GenerationProperty();
			generationProperty.Name = Name;
			generationProperty.Type = ValueType;
			generationProperty.AnimationType = PropertyAnimationType.Const;
			switch (m_value.GetType().Name)
			{
			case "Single":
			case "Float":
				generationProperty.Type = "Float";
				generationProperty.ValueFloat = (float)(object)m_value;
				break;
			case "Vector3":
				generationProperty.Type = "Vector3";
				generationProperty.ValueVector3 = (Vector3)(object)m_value;
				break;
			case "Vector4":
				generationProperty.Type = "Vector4";
				generationProperty.ValueVector4 = (Vector4)(object)m_value;
				break;
			default:
				generationProperty.Type = "Int";
				generationProperty.ValueInt = (int)(object)m_value;
				break;
			case "Boolean":
				generationProperty.Type = "Bool";
				generationProperty.ValueBool = (bool)(object)m_value;
				break;
			case "String":
				generationProperty.Type = "String";
				generationProperty.ValueString = (string)(object)m_value;
				break;
			case "MyTransparentMaterial":
				generationProperty.Type = "MyTransparentMaterial";
				generationProperty.ValueString = ((MyTransparentMaterial)(object)m_value).Id.String;
				break;
			}
			return generationProperty;
		}

		public virtual void DeserializeFromObjectBuilder(GenerationProperty property)
		{
			m_name = property.Name;
			m_value = (T)(property.Type switch
			{
				"Float" => property.ValueFloat, 
				"Vector3" => property.ValueVector3, 
				"Vector4" => property.ValueVector4, 
				"Bool" => property.ValueBool, 
				"String" => property.ValueString, 
				"MyTransparentMaterial" => MyTransparentMaterials.GetMaterial(MyStringId.GetOrCompute(property.ValueString)), 
				_ => property.ValueInt, 
			});
		}

		public virtual void SerializeValue(XmlWriter writer, object value)
		{
		}

		public virtual void DeserializeValue(XmlReader reader, out object value)
		{
			value = reader.get_Value();
			reader.Read();
		}
	}
}
