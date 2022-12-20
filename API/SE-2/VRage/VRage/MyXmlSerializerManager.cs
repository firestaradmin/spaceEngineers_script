using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using VRage.Utils;

namespace VRage
{
	public class MyXmlSerializerManager
	{
		private static readonly HashSet<Type> m_serializableBaseTypes = new HashSet<Type>();

		private static readonly Dictionary<Type, XmlSerializer> m_serializersByType = new Dictionary<Type, XmlSerializer>();

		private static readonly Dictionary<string, XmlSerializer> m_serializersBySerializedName = new Dictionary<string, XmlSerializer>();

		private static readonly Dictionary<Type, string> m_serializedNameByType = new Dictionary<Type, string>();

		private static HashSet<Assembly> m_registeredAssemblies = new HashSet<Assembly>();

		public static void RegisterSerializer(Type type)
		{
			if (!m_serializersByType.ContainsKey(type))
			{
				RegisterType(type, forceRegister: true, checkAttributes: false);
			}
		}

		public static void RegisterSerializableBaseType(Type type)
		{
			m_serializableBaseTypes.Add(type);
		}

		public static void RegisterFromAssembly(Assembly assembly)
		{
			if (assembly == null || m_registeredAssemblies.Contains(assembly))
			{
				return;
			}
			m_registeredAssemblies.Add(assembly);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				try
				{
					if (!m_serializersByType.ContainsKey(type))
					{
						RegisterType(type);
					}
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException("Error creating XML serializer for type " + type.Name, innerException);
				}
			}
		}

		public static XmlSerializer GetSerializer(Type type)
		{
			return m_serializersByType[type];
		}

		public static XmlSerializer GetOrCreateSerializer(Type type)
		{
			if (!m_serializersByType.TryGetValue(type, out var value))
			{
				return RegisterType(type, forceRegister: true);
			}
			return value;
		}

		public static string GetSerializedName(Type type)
		{
			return m_serializedNameByType[type];
		}

		public static bool TryGetSerializer(string serializedName, out XmlSerializer serializer)
		{
			return m_serializersBySerializedName.TryGetValue(serializedName, out serializer);
		}

		public static XmlSerializer GetSerializer(string serializedName)
		{
			return m_serializersBySerializedName[serializedName];
		}

		public static bool IsSerializerAvailable(string name)
		{
			return m_serializersBySerializedName.ContainsKey(name);
		}

		private static XmlSerializer RegisterType(Type type, bool forceRegister = false, bool checkAttributes = true)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			string text = null;
			if (checkAttributes)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(XmlTypeAttribute), inherit: false);
				if (customAttributes.Length != 0)
				{
					XmlTypeAttribute val = (XmlTypeAttribute)customAttributes[0];
					text = type.Name;
					if (!string.IsNullOrEmpty(val.get_TypeName()))
					{
						text = val.get_TypeName();
					}
				}
				else
				{
					Enumerator<Type> enumerator = m_serializableBaseTypes.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.get_Current().IsAssignableFrom(type))
							{
								text = type.Name;
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
				}
			}
			if (text == null)
			{
				if (!forceRegister)
				{
					return null;
				}
				text = type.Name;
			}
			XmlSerializer val2 = null;
			foreach (Attribute item in EnumerateThisAndParentAttributes(type))
			{
				Type type2 = item.GetType();
				if (type2.Name == "XmlSerializerAssemblyAttribute")
				{
					val2 = TryLoadSerializerFrom((string)type2.GetProperty("AssemblyName").GetValue(item), type.Name);
					if (val2 != null)
					{
						break;
					}
				}
			}
			if (val2 == null)
			{
				string text2 = type.Assembly.GetName().Name + ".XmlSerializers";
				MyLog.Default.Error("Type {0} is missing missing XmlSerializerAssemblyAttribute. Falling back to default {1}", type.Name, text2);
				val2 = TryLoadSerializerFrom(text2, type.Name);
			}
			if (val2 == null)
			{
				val2 = new XmlSerializer(type);
			}
			m_serializersByType.Add(type, val2);
			m_serializersBySerializedName.Add(text, val2);
			m_serializedNameByType.Add(type, text);
			return val2;
		}

		private static XmlSerializer TryLoadSerializerFrom(string assemblyName, string typeName)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load(new AssemblyName(assemblyName));
			}
			catch
			{
			}
			if (assembly == null)
			{
				try
				{
					assembly = Assembly.Load(assemblyName);
				}
				catch
				{
				}
			}
			if (assembly == null)
			{
				return null;
			}
			Type type = assembly.GetType("Microsoft.Xml.Serialization.GeneratedAssembly." + typeName + "Serializer");
			if (type != null)
			{
				return (XmlSerializer)Activator.CreateInstance(type);
			}
			return null;
		}

		private static IEnumerable<Attribute> EnumerateThisAndParentAttributes(Type type)
		{
			if (type == null)
			{
				yield break;
			}
<<<<<<< HEAD
			foreach (Attribute customAttribute in CustomAttributeExtensions.GetCustomAttributes(type))
=======
			foreach (Attribute customAttribute in type.GetCustomAttributes())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				yield return customAttribute;
			}
			foreach (Attribute item in EnumerateThisAndParentAttributes(type.DeclaringType))
			{
				yield return item;
			}
		}
	}
}
