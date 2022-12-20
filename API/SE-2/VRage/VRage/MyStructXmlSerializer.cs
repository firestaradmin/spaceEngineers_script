using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace VRage
{
	/// <summary>
	/// Deserializes structs using a specified default value (see StructDefaultAttribute).
	/// </summary>
	public class MyStructXmlSerializer<TStruct> : MyXmlSerializerBase<TStruct> where TStruct : struct
	{
		/// <summary>
		/// Abstract accessor for both fields and properties
		/// </summary>
		private abstract class Accessor
		{
			public abstract Type Type { get; }

			public Type SerializerType { get; private set; }

			public bool IsPrimitiveType
			{
				get
				{
					Type type = Type;
					if (!type.IsPrimitive)
					{
						return type == typeof(string);
					}
					return true;
				}
			}

			public abstract object GetValue(object obj);

			public abstract void SetValue(object obj, object value);

			protected void CheckXmlElement(MemberInfo info)
			{
<<<<<<< HEAD
				XmlElementAttribute xmlElementAttribute = CustomAttributeExtensions.GetCustomAttribute(info, typeof(XmlElementAttribute), inherit: false) as XmlElementAttribute;
				if (xmlElementAttribute != null && xmlElementAttribute.Type != null && typeof(IMyXmlSerializable).IsAssignableFrom(xmlElementAttribute.Type))
=======
				Attribute customAttribute = info.GetCustomAttribute(typeof(XmlElementAttribute), inherit: false);
				XmlElementAttribute val = (XmlElementAttribute)(object)((customAttribute is XmlElementAttribute) ? customAttribute : null);
				if (val != null && val.get_Type() != null && typeof(IMyXmlSerializable).IsAssignableFrom(val.get_Type()))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					SerializerType = val.get_Type();
				}
			}
		}

		private class FieldAccessor : Accessor
		{
			public FieldInfo Field { get; private set; }

			public override Type Type => Field.FieldType;

			public FieldAccessor(FieldInfo field)
			{
				Field = field;
				CheckXmlElement(field);
			}

			public override object GetValue(object obj)
			{
				return Field.GetValue(obj);
			}

			public override void SetValue(object obj, object value)
			{
				Field.SetValue(obj, value);
			}
		}

		private class PropertyAccessor : Accessor
		{
			public PropertyInfo Property { get; private set; }

			public override Type Type => Property.PropertyType;

			public PropertyAccessor(PropertyInfo property)
			{
				Property = property;
				CheckXmlElement(property);
			}

			public override object GetValue(object obj)
			{
				return Property.GetValue(obj);
			}

			public override void SetValue(object obj, object value)
			{
				Property.SetValue(obj, value);
			}
		}

		public static FieldInfo m_defaultValueField;

		private static Dictionary<string, Accessor> m_accessorMap;

		public MyStructXmlSerializer()
		{
		}

		public MyStructXmlSerializer(ref TStruct data)
		{
			m_data = data;
		}

		public override void ReadXml(XmlReader reader)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Invalid comparison between Unknown and I4
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Invalid comparison between Unknown and I4
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			BuildAccessorsInfo();
			object obj = (TStruct)m_defaultValueField.GetValue(null);
			reader.MoveToElement();
			if (reader.get_IsEmptyElement())
			{
				reader.Skip();
				return;
			}
			reader.ReadStartElement();
			reader.MoveToContent();
			while ((int)reader.get_NodeType() != 15 && (int)reader.get_NodeType() != 0)
			{
				if ((int)reader.get_NodeType() == 1)
				{
<<<<<<< HEAD
					if (m_accessorMap.TryGetValue(reader.LocalName, out var value))
=======
					if (m_accessorMap.TryGetValue(reader.get_LocalName(), out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						object value2;
						if (value.IsPrimitiveType)
						{
							string text = reader.ReadElementString();
							value2 = TypeDescriptor.GetConverter(value.Type).ConvertFrom((ITypeDescriptorContext)null, CultureInfo.InvariantCulture, (object)text);
						}
						else if (value.SerializerType != null)
						{
							IMyXmlSerializable obj2 = Activator.CreateInstance(value.SerializerType) as IMyXmlSerializable;
							((IXmlSerializable)obj2).ReadXml(reader.ReadSubtree());
							value2 = obj2.Data;
							reader.ReadEndElement();
						}
						else
						{
							XmlSerializer orCreateSerializer = MyXmlSerializerManager.GetOrCreateSerializer(value.Type);
							string serializedName = MyXmlSerializerManager.GetSerializedName(value.Type);
							value2 = Deserialize(reader, orCreateSerializer, serializedName);
						}
						value.SetValue(obj, value2);
					}
					else
					{
						reader.Skip();
					}
				}
				reader.MoveToContent();
			}
			reader.ReadEndElement();
			m_data = (TStruct)obj;
		}

		private static void BuildAccessorsInfo()
		{
			if (m_defaultValueField != null)
			{
				return;
			}
			lock (typeof(TStruct))
			{
				if (m_defaultValueField != null)
				{
					return;
				}
				m_defaultValueField = MyStructDefault.GetDefaultFieldInfo(typeof(TStruct));
				if (m_defaultValueField == null)
				{
					throw new Exception("Missing default value for struct " + typeof(TStruct).FullName + ". Decorate one static read-only field with StructDefault attribute");
				}
				m_accessorMap = new Dictionary<string, Accessor>();
				FieldInfo[] fields = typeof(TStruct).GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo fieldInfo in fields)
				{
<<<<<<< HEAD
					if (CustomAttributeExtensions.GetCustomAttribute(fieldInfo, typeof(XmlIgnoreAttribute)) == null)
=======
					if (fieldInfo.GetCustomAttribute(typeof(XmlIgnoreAttribute)) == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_accessorMap.Add(fieldInfo.Name, new FieldAccessor(fieldInfo));
					}
				}
				PropertyInfo[] properties = typeof(TStruct).GetProperties(BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in properties)
				{
<<<<<<< HEAD
					if (CustomAttributeExtensions.GetCustomAttribute(propertyInfo, typeof(XmlIgnoreAttribute)) == null && propertyInfo.GetIndexParameters().Length == 0)
=======
					if (propertyInfo.GetCustomAttribute(typeof(XmlIgnoreAttribute)) == null && propertyInfo.GetIndexParameters().Length == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_accessorMap.Add(propertyInfo.Name, new PropertyAccessor(propertyInfo));
					}
				}
			}
		}

		public static implicit operator MyStructXmlSerializer<TStruct>(TStruct data)
		{
			return new MyStructXmlSerializer<TStruct>(ref data);
		}
	}
}
