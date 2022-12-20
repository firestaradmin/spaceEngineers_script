using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Game.Entity;
using VRageMath;

namespace VRage.Game.VisualScripting
{
	public static class MyVisualScriptingProxy
	{
		private static readonly Dictionary<string, MethodInfo> m_visualScriptingMethodsBySignature = new Dictionary<string, MethodInfo>();

		private static readonly Dictionary<Type, HashSet<MethodInfo>> m_whitelistedMethods = new Dictionary<Type, HashSet<MethodInfo>>();

		private static readonly Dictionary<MethodInfo, bool> m_whitelistedMethodsSequenceDependency = new Dictionary<MethodInfo, bool>();

		private static readonly Dictionary<string, FieldInfo> m_visualScriptingEventFields = new Dictionary<string, FieldInfo>();

		private static readonly Dictionary<string, Type> m_registeredTypes = new Dictionary<string, Type>();

		private static readonly List<Type> m_supportedTypes = new List<Type>();

		private static bool m_initialized = false;

		public static IEnumerable<FieldInfo> EventFields => m_visualScriptingEventFields.Values;

		public static List<Type> SupportedTypes => m_supportedTypes;

		/// <summary>
		/// Loads reflection data.
		/// </summary>
		public static void Init()
		{
			if (!m_initialized)
			{
				m_supportedTypes.Add(typeof(int));
				m_supportedTypes.Add(typeof(float));
				m_supportedTypes.Add(typeof(double));
				m_supportedTypes.Add(typeof(string));
				m_supportedTypes.Add(typeof(Vector3D));
				m_supportedTypes.Add(typeof(bool));
				m_supportedTypes.Add(typeof(long));
				m_supportedTypes.Add(typeof(ulong));
				m_supportedTypes.Add(typeof(List<bool>));
				m_supportedTypes.Add(typeof(List<int>));
				m_supportedTypes.Add(typeof(List<float>));
				m_supportedTypes.Add(typeof(List<double>));
				m_supportedTypes.Add(typeof(List<string>));
				m_supportedTypes.Add(typeof(List<long>));
				m_supportedTypes.Add(typeof(List<ulong>));
				m_supportedTypes.Add(typeof(List<Vector3D>));
				m_supportedTypes.Add(typeof(List<MyEntity>));
				m_supportedTypes.Add(typeof(MyEntity));
				MyVisualScriptLogicProvider.Init();
				m_initialized = true;
			}
		}

		private static void RegisterMethod(Type declaringType, MethodInfo method, VisualScriptingMember attribute, bool? overrideSequenceDependency = null)
		{
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			if (declaringType.IsGenericType)
			{
				declaringType = declaringType.GetGenericTypeDefinition();
			}
			if (!m_whitelistedMethods.ContainsKey(declaringType))
			{
				m_whitelistedMethods[declaringType] = new HashSet<MethodInfo>();
			}
			m_whitelistedMethods[declaringType].Add(method);
			m_whitelistedMethodsSequenceDependency[method] = overrideSequenceDependency ?? attribute.Sequential;
			foreach (KeyValuePair<Type, HashSet<MethodInfo>> whitelistedMethod in m_whitelistedMethods)
			{
				if (whitelistedMethod.Key.IsAssignableFrom(declaringType))
				{
					whitelistedMethod.Value.Add(method);
				}
				else
				{
					if (!declaringType.IsAssignableFrom(whitelistedMethod.Key))
					{
						continue;
					}
<<<<<<< HEAD
					HashSet<MethodInfo> hashSet = m_whitelistedMethods[declaringType];
					foreach (MethodInfo item in whitelistedMethod.Value)
=======
					HashSet<MethodInfo> val = m_whitelistedMethods[declaringType];
					Enumerator<MethodInfo> enumerator2 = whitelistedMethod.Value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MethodInfo current2 = enumerator2.get_Current();
							val.Add(current2);
						}
					}
					finally
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
		}

		public static void RegisterType(Type type)
		{
			string key = type.Signature();
			if (!m_registeredTypes.ContainsKey(key))
			{
				m_registeredTypes.Add(key, type);
			}
		}

		public static void WhitelistExtensions(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo methodInfo in methods)
			{
				VisualScriptingMember customAttribute = CustomAttributeExtensions.GetCustomAttribute<VisualScriptingMember>(methodInfo);
				if (customAttribute != null && methodInfo.IsDefined(typeof(ExtensionAttribute), inherit: false))
				{
					RegisterMethod(methodInfo.GetParameters()[0].ParameterType, methodInfo, customAttribute);
				}
			}
			m_registeredTypes[type.Signature()] = type;
		}

		public static void WhitelistMethod(MethodInfo method, bool sequenceDependent)
		{
			Type declaringType = method.DeclaringType;
			if (!(declaringType == null))
			{
				RegisterMethod(declaringType, method, null, sequenceDependent);
			}
		}

		public static IEnumerable<MethodInfo> GetWhitelistedMethods(Type type)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			if (type == null)
			{
<<<<<<< HEAD
				HashSet<MethodInfo> hashSet = new HashSet<MethodInfo>();
				{
					foreach (HashSet<MethodInfo> value3 in m_whitelistedMethods.Values)
					{
						foreach (MethodInfo item in value3)
						{
							hashSet.Add(item);
						}
					}
					return hashSet;
=======
				HashSet<MethodInfo> val = new HashSet<MethodInfo>();
				{
					foreach (HashSet<MethodInfo> value3 in m_whitelistedMethods.Values)
					{
						Enumerator<MethodInfo> enumerator2 = value3.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MethodInfo current = enumerator2.get_Current();
								val.Add(current);
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					return (IEnumerable<MethodInfo>)val;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			if (m_whitelistedMethods.TryGetValue(type, out var value))
			{
				return (IEnumerable<MethodInfo>)value;
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				Type[] genericArguments = type.GetGenericArguments();
				if (m_whitelistedMethods.TryGetValue(genericTypeDefinition, out value))
				{
<<<<<<< HEAD
					HashSet<MethodInfo> hashSet2 = new HashSet<MethodInfo>();
					m_whitelistedMethods[type] = hashSet2;
					{
						foreach (MethodInfo item2 in value)
=======
					HashSet<MethodInfo> val2 = new HashSet<MethodInfo>();
					m_whitelistedMethods[type] = val2;
					Enumerator<MethodInfo> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							MethodInfo current2 = enumerator2.get_Current();
							MethodInfo methodInfo = null;
<<<<<<< HEAD
							methodInfo = ((!CustomAttributeExtensions.IsDefined(item2, typeof(ExtensionAttribute))) ? type.GetMethod(item2.Name) : item2.MakeGenericMethod(genericArguments));
							hashSet2.Add(methodInfo);
							bool value2 = m_whitelistedMethodsSequenceDependency[item2];
							m_whitelistedMethodsSequenceDependency[methodInfo] = value2;
							m_visualScriptingMethodsBySignature[methodInfo.Signature()] = methodInfo;
						}
						return hashSet2;
=======
							methodInfo = ((!current2.IsDefined(typeof(ExtensionAttribute))) ? type.GetMethod(current2.Name) : current2.MakeGenericMethod(genericArguments));
							val2.Add(methodInfo);
							bool value2 = m_whitelistedMethodsSequenceDependency[current2];
							m_whitelistedMethodsSequenceDependency[methodInfo] = value2;
							m_visualScriptingMethodsBySignature[methodInfo.Signature()] = methodInfo;
						}
						return (IEnumerable<MethodInfo>)val2;
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			return null;
		}

		public static void RegisterLogicProvider(Type type)
		{
			MethodInfo[] methods = type.GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				if (CustomAttributeExtensions.GetCustomAttribute<VisualScriptingMember>(methodInfo) != null)
				{
					string key = methodInfo.Signature();
					if (!m_visualScriptingMethodsBySignature.ContainsKey(key))
					{
						m_visualScriptingMethodsBySignature.Add(key, methodInfo);
					}
				}
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (CustomAttributeExtensions.GetCustomAttribute<VisualScriptingEvent>(fieldInfo.FieldType) != null && fieldInfo.FieldType.IsSubclassOf(typeof(MulticastDelegate)) && !m_visualScriptingEventFields.ContainsKey(fieldInfo.Signature()))
				{
					m_visualScriptingEventFields.Add(fieldInfo.Signature(), fieldInfo);
				}
			}
		}

		/// <summary>
		/// Looks for given type using executing assembly.
		/// </summary>
		/// <param name="typeFullName"></param>
		/// <returns></returns>
		public static Type GetType(string typeFullName)
		{
			if (typeFullName == null || typeFullName.Length == 0)
			{
				throw new Exception("Null type signature!");
			}
			if (m_registeredTypes.TryGetValue(typeFullName, out var value))
			{
				return value;
			}
			value = Type.GetType(typeFullName);
			if (value != null)
			{
				return value;
			}
			return typeof(Vector3D).Assembly.GetType(typeFullName);
		}

		/// <summary>
		/// Looks for methodInfo about method with given signature.
		/// </summary>
		/// <param name="signature">Full signature of a method.</param>
		/// <returns>null if not found.</returns>
		public static MethodInfo GetMethod(string signature)
		{
<<<<<<< HEAD
			MethodInfo methodInfo = GetWhitelistedMethods(null).FirstOrDefault((MethodInfo x) => x.Signature() == signature);
=======
			MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(GetWhitelistedMethods(null), (Func<MethodInfo, bool>)((MethodInfo x) => x.Signature() == signature));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (methodInfo != null)
			{
				return methodInfo;
			}
			m_visualScriptingMethodsBySignature.TryGetValue(signature, out var value);
			return value;
		}

		public static MethodInfo GetMethodCaseInvariant(string signature)
		{
			string signatureLowercase = signature.ToLower();
<<<<<<< HEAD
			MethodInfo methodInfo = GetWhitelistedMethods(null).FirstOrDefault((MethodInfo x) => x.Signature().ToLower() == signatureLowercase);
=======
			MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(GetWhitelistedMethods(null), (Func<MethodInfo, bool>)((MethodInfo x) => x.Signature().ToLower() == signatureLowercase));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (methodInfo != null)
			{
				return methodInfo;
			}
			MethodInfo result = null;
			foreach (KeyValuePair<string, MethodInfo> item in m_visualScriptingMethodsBySignature)
			{
				if (item.Key.ToLower() == signatureLowercase)
				{
					return item.Value;
				}
			}
			return result;
		}

		public static MethodInfo GetMethod(Type type, string signature)
		{
			if (!m_whitelistedMethods.ContainsKey(type))
			{
				GetWhitelistedMethods(type);
			}
			return GetMethod(signature);
		}

		/// <summary>
		/// All attributed methods from VisualScriptingProxy.
		/// </summary>
		/// <returns></returns>
		public static List<MethodInfo> GetMethods()
		{
			List<MethodInfo> list = new List<MethodInfo>();
			foreach (KeyValuePair<string, MethodInfo> item in m_visualScriptingMethodsBySignature)
			{
				list.Add(item.Value);
			}
			return list;
		}

		/// <summary>
		/// Returns event field with specified signature.
		/// </summary>
		/// <param name="signature"></param>
		/// <returns></returns>
		public static FieldInfo GetField(string signature)
		{
			m_visualScriptingEventFields.TryGetValue(signature, out var value);
			return value;
		}

		public static string Signature(this FieldInfo info)
		{
			return info.DeclaringType.Namespace + "." + info.DeclaringType.Name + "." + info.Name;
		}

		public static bool TryToRecoverMethodInfo(ref string oldSignature, Type declaringType, Type extensionType, out MethodInfo info)
		{
			info = null;
			int i;
			for (i = 0; i < oldSignature.Length && i < declaringType.FullName.Length && oldSignature[i] == declaringType.FullName[i]; i++)
			{
			}
			oldSignature = oldSignature.Remove(0, i + 1);
			oldSignature = oldSignature.Remove(oldSignature.IndexOf('('));
			if (extensionType != null && extensionType.IsGenericType)
			{
				Type[] genericArguments = extensionType.GetGenericArguments();
				MethodInfo method = declaringType.GetMethod(oldSignature);
				if (method != null)
				{
					info = method.MakeGenericMethod(genericArguments);
				}
			}
			else
			{
				info = declaringType.GetMethod(oldSignature);
			}
			if (info != null)
			{
				oldSignature = info.Signature();
			}
			return info != null;
		}

		public static bool TryToRecoverMethodInfo(ref string oldSignature, out MethodInfo info)
		{
			info = null;
			new List<string>();
			string value = oldSignature.Replace(")", "");
			foreach (KeyValuePair<string, MethodInfo> item in m_visualScriptingMethodsBySignature)
			{
				if (item.Value.Signature().StartsWith(value))
				{
					info = item.Value;
					break;
				}
			}
			return info != null;
		}

		public static string Signature(this MethodInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder(info.DeclaringType.Signature());
			ParameterInfo[] parameters = info.GetParameters();
			stringBuilder.Append('.').Append(info.Name).Append('(');
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].ParameterType.IsGenericType)
				{
					stringBuilder.Append(parameters[i].ParameterType.Signature());
				}
				else
				{
					stringBuilder.Append(parameters[i].ParameterType.Name);
				}
				stringBuilder.Append(' ').Append(parameters[i].Name);
				if (i < parameters.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		public static string MethodGroup(this MethodInfo info)
		{
			return CustomAttributeExtensions.GetCustomAttribute<VisualScriptingMiscData>(info)?.Group;
		}

		public static string Signature(this Type type)
		{
			if (type.IsEnum)
			{
				return type.FullName.Replace("+", ".");
			}
			if (type.IsByRef)
			{
				return "ref " + type.FullName.Replace("&", "");
			}
			return type.FullName;
		}

		public static bool IsSequenceDependent(this MethodInfo method)
		{
			VisualScriptingMember customAttribute = CustomAttributeExtensions.GetCustomAttribute<VisualScriptingMember>(method);
			if (customAttribute == null && !method.IsStatic)
			{
				bool value = true;
				if (m_whitelistedMethodsSequenceDependency.TryGetValue(method, out value))
				{
					return value;
				}
				return true;
			}
			return customAttribute?.Sequential ?? true;
		}

		public static string ReadableName(this Type type)
		{
			if (type == null)
			{
				Debugger.Break();
				return null;
			}
			if (type == typeof(bool))
			{
				return "Bool";
			}
			if (type == typeof(int))
			{
				return "Int";
			}
			if (type == typeof(string))
			{
				return "String";
			}
			if (type == typeof(float))
			{
				return "Float";
			}
			if (type == typeof(long))
			{
				return "Long";
			}
			if (type == typeof(ulong))
			{
				return "ULong";
			}
			if (type.IsGenericType)
			{
				StringBuilder stringBuilder = new StringBuilder(type.Name.Remove(type.Name.IndexOf('`')));
				Type[] genericArguments = type.GetGenericArguments();
				stringBuilder.Append(" - ");
				Type[] array = genericArguments;
				foreach (Type type2 in array)
				{
					stringBuilder.Append(type2.ReadableName());
					stringBuilder.Append(",");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				return stringBuilder.ToString();
			}
			return type.Name;
		}
	}
}
