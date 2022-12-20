using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
<<<<<<< HEAD
	/// <summary> 
	/// Base class for accepted SE analytics events and their associated properties
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public abstract class MyAnalyticsEvent : IMyAnalyticsEvent
	{
		private static readonly List<TypeInfo> m_subtypes;

		private static readonly HashSet<string> m_allEventProperties;

		private static readonly List<Type> m_supportedTypes;

		static MyAnalyticsEvent()
		{
			m_subtypes = GetSubtypes();
			m_allEventProperties = GetAllEventProperties();
			m_supportedTypes = new List<Type>
			{
				typeof(string),
				typeof(bool?),
				typeof(bool),
				typeof(int?),
				typeof(uint?),
				typeof(long?),
				typeof(ulong?),
				typeof(int),
				typeof(uint),
				typeof(long),
				typeof(ulong),
				typeof(double?),
				typeof(double),
				typeof(DateTime?),
				typeof(byte?),
				typeof(byte)
			};
			foreach (Type item in EnumerateTypesWithSupportedTypeAttribute())
			{
				m_supportedTypes.Add(item);
			}
		}

		private static IEnumerable<Type> EnumerateTypesWithSupportedTypeAttribute()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(MyAnalyticsEvent));
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.GetCustomAttributes(typeof(SupportedTypeAttribute), inherit: true).Length != 0)
				{
					yield return type;
				}
			}
		}

		private static List<TypeInfo> GetSubtypes()
		{
			List<TypeInfo> list = new List<TypeInfo>();
			Type[] types = typeof(MyAnalyticsEvent).Assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsSubclassOf(typeof(MyAnalyticsEvent)))
				{
<<<<<<< HEAD
					list.Add(IntrospectionExtensions.GetTypeInfo(type));
=======
					list.Add(type.GetTypeInfo());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return list;
		}

		private static HashSet<string> GetAllEventProperties()
		{
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
=======
			HashSet<string> val = new HashSet<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (TypeInfo subtype in m_subtypes)
			{
				PropertyInfo[] properties = subtype.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in properties)
				{
<<<<<<< HEAD
					if (hashSet.Contains(propertyInfo.Name) && !m_subtypes.Contains(IntrospectionExtensions.GetTypeInfo(propertyInfo.PropertyType)))
					{
						throw new MyAnalyticsSpecificationException("Duplicate declaration of MyAnalyticsEvent property '" + propertyInfo.Name + "'. All event properties should by unique");
					}
					hashSet.Add(propertyInfo.Name);
				}
			}
			return hashSet;
=======
					if (val.Contains(propertyInfo.Name) && !m_subtypes.Contains(propertyInfo.PropertyType.GetTypeInfo()))
					{
						throw new MyAnalyticsSpecificationException("Duplicate declaration of MyAnalyticsEvent property '" + propertyInfo.Name + "'. All event properties should by unique");
					}
					val.Add(propertyInfo.Name);
				}
			}
			return val;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public MyAnalyticsEvent()
		{
		}

		public abstract string GetEventName();

		public virtual MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionUndefined, "Game", GetEventName());
		}

		public Dictionary<string, object> GetPropertiesDictionary()
		{
			PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				object value = propertyInfo.GetValue(this, null);
				if (value == null)
				{
					if (Attribute.IsDefined(propertyInfo, typeof(RequiredAttribute)))
					{
						throw new MyAnalyticsSpecificationException("The [Required] property '" + propertyInfo.Name + "' is null");
					}
					continue;
				}
				foreach (KeyValuePair<string, object> item in FlattenAndValidateProperty(propertyInfo, value))
				{
					if (item.Value != null)
					{
						dictionary.Add(item.Key, item.Value);
					}
				}
			}
			return dictionary;
		}

		private Dictionary<string, object> FlattenAndValidateProperty(PropertyInfo propertyInfo, object propertyValue)
		{
<<<<<<< HEAD
			if (m_subtypes.Contains(IntrospectionExtensions.GetTypeInfo(propertyInfo.PropertyType)))
=======
			if (m_subtypes.Contains(propertyInfo.PropertyType.GetTypeInfo()))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return ((MyAnalyticsEvent)propertyValue).GetPropertiesDictionary();
			}
			if (m_supportedTypes.Contains(propertyInfo.PropertyType))
			{
				return new Dictionary<string, object> { { propertyInfo.Name, propertyValue } };
			}
			if (propertyInfo.PropertyType.IsEnum)
			{
				return new Dictionary<string, object> { 
				{
					propertyInfo.Name,
					propertyValue.ToString()
				} };
			}
			if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<, >) && propertyInfo.PropertyType.GetGenericArguments()[0] == typeof(string) && m_supportedTypes.Contains(propertyInfo.PropertyType.GetGenericArguments()[1]))
			{
				IDictionary obj = propertyValue as IDictionary;
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				{
					foreach (DictionaryEntry item in obj)
					{
						dictionary[propertyInfo.Name + "." + item.Key] = item.Value;
					}
					return dictionary;
				}
			}
			if (propertyInfo.PropertyType.IsArray && m_supportedTypes.Contains(propertyInfo.PropertyType.GetElementType()))
			{
				return new Dictionary<string, object> { { propertyInfo.Name, propertyValue } };
			}
			if (propertyInfo.PropertyType.IsGenericType && typeof(IEnumerable<object>).IsAssignableFrom(propertyInfo.PropertyType) && m_supportedTypes.Contains(propertyInfo.PropertyType.GetGenericArguments()[0]))
			{
				IEnumerable<object> value = (IEnumerable<object>)propertyValue;
				return new Dictionary<string, object> { { propertyInfo.Name, value } };
			}
			throw new MyAnalyticsSpecificationException($"Property '{propertyInfo.Name}' is of unsupported type '{propertyInfo.PropertyType}'");
		}
	}
}
