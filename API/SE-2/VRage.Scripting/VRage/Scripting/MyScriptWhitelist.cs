using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VRage.Collections;

namespace VRage.Scripting
{
	/// <summary>
	///     The script whitelist contains information about which types and type members are allowed in the
	///     various types of scripts.
	/// </summary>
	public class MyScriptWhitelist : IMyScriptBlacklist
	{
		private abstract class Batch : IDisposable
		{
			private readonly Dictionary<string, IAssemblySymbol> m_assemblyMap;

			private bool m_isDisposed;

			protected MyScriptWhitelist Whitelist { get; private set; }

			protected Batch(MyScriptWhitelist whitelist)
			{
				Whitelist = whitelist;
				CSharpCompilation cSharpCompilation = Whitelist.CreateCompilation();
				m_assemblyMap = Enumerable.ToDictionary<IAssemblySymbol, string>(Enumerable.OfType<IAssemblySymbol>((IEnumerable)Enumerable.Select<MetadataReference, ISymbol>(cSharpCompilation.References, (Func<MetadataReference, ISymbol>)cSharpCompilation.GetAssemblyOrModuleSymbol)), (Func<IAssemblySymbol, string>)((IAssemblySymbol symbol) => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
			}

			~Batch()
			{
				m_isDisposed = true;
			}

			[DebuggerNonUserCode]
			protected void AssertVitality()
			{
				if (m_isDisposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
			}

			protected INamedTypeSymbol ResolveTypeSymbol(Type type)
			{
				if (!m_assemblyMap.TryGetValue(type.Assembly.FullName, out var value))
				{
					throw new MyWhitelistException($"Cannot add {type.FullName} to the batch because {type.Assembly.FullName} has not been added to the compiler.");
				}
				return value.GetTypeByMetadataName(type.FullName) ?? throw new MyWhitelistException($"Cannot add {type.FullName} to the batch because its symbol variant could not be found.");
			}

			public void Dispose()
			{
				if (!m_isDisposed)
				{
					m_isDisposed = true;
					OnDispose();
					GC.SuppressFinalize(this);
				}
			}

			protected virtual void OnDispose()
			{
			}
		}

		private class MyScriptBlacklistBatch : Batch, IMyScriptBlacklistBatch, IDisposable
		{
			public MyScriptBlacklistBatch(MyScriptWhitelist whitelist)
				: base(whitelist)
			{
			}

			public void AddNamespaceOfTypes(params Type[] types)
			{
				if (types.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one type");
				}
				AssertVitality();
				for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (type == null)
					{
						throw new MyWhitelistException("The type in index " + i + " is null");
					}
					INamespaceSymbol containingNamespace = ResolveTypeSymbol(type).ContainingNamespace;
					if (containingNamespace != null && !containingNamespace.IsGlobalNamespace)
					{
						base.Whitelist.m_ingameBlacklist.Add(containingNamespace.GetWhitelistKey(TypeKeyQuantity.AllMembers));
					}
				}
			}

			public void RemoveNamespaceOfTypes(params Type[] types)
			{
				if (types.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one type");
				}
				AssertVitality();
				for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (type == null)
					{
						throw new MyWhitelistException("The type in index " + i + " is null");
					}
					INamespaceSymbol containingNamespace = ResolveTypeSymbol(type).ContainingNamespace;
					if (containingNamespace != null && !containingNamespace.IsGlobalNamespace)
					{
						base.Whitelist.m_ingameBlacklist.Remove(containingNamespace.GetWhitelistKey(TypeKeyQuantity.AllMembers));
					}
				}
			}

			public void AddTypes(params Type[] types)
			{
				if (types.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one type");
				}
				AssertVitality();
				for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (type == null)
					{
						throw new MyWhitelistException("The type in index " + i + " is null");
					}
					INamedTypeSymbol symbol = ResolveTypeSymbol(type);
					base.Whitelist.m_ingameBlacklist.Add(symbol.GetWhitelistKey(TypeKeyQuantity.AllMembers));
				}
			}

			public void RemoveTypes(params Type[] types)
			{
				if (types.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one type");
				}
				AssertVitality();
				for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (type == null)
					{
						throw new MyWhitelistException("The type in index " + i + " is null");
					}
					INamedTypeSymbol symbol = ResolveTypeSymbol(type);
					base.Whitelist.m_ingameBlacklist.Remove(symbol.GetWhitelistKey(TypeKeyQuantity.AllMembers));
				}
			}

			public void AddMembers(Type type, params string[] memberNames)
			{
				if (type == null)
				{
					throw new MyWhitelistException("Must specify the target type");
				}
				if (memberNames.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one member name");
				}
				AssertVitality();
				List<string> list = new List<string>();
				GetMemberWhitelistKeys(type, memberNames, list);
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i];
					base.Whitelist.m_ingameBlacklist.Add(text);
				}
			}

			public void RemoveMembers(Type type, params string[] memberNames)
			{
				if (type == null)
				{
					throw new MyWhitelistException("Must specify the target type");
				}
				if (memberNames.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one member name");
				}
				AssertVitality();
				List<string> list = new List<string>();
				GetMemberWhitelistKeys(type, memberNames, list);
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i];
					base.Whitelist.m_ingameBlacklist.Remove(text);
				}
			}

			private void GetMemberWhitelistKeys(Type type, string[] memberNames, List<string> members)
			{
				INamedTypeSymbol namedTypeSymbol = ResolveTypeSymbol(type);
				foreach (string text in memberNames)
				{
					int count = members.Count;
					ImmutableArray<ISymbol>.Enumerator enumerator = namedTypeSymbol.GetMembers().GetEnumerator();
					while (enumerator.MoveNext())
					{
						ISymbol current = enumerator.Current;
						if (!(current.Name != text))
						{
							Accessibility declaredAccessibility = current.DeclaredAccessibility;
							if (declaredAccessibility == Accessibility.Protected || (uint)(declaredAccessibility - 5) <= 1u)
							{
								members.Add(current.GetWhitelistKey(TypeKeyQuantity.ThisOnly));
							}
						}
					}
					if (count == members.Count)
					{
						throw new MyWhitelistException("Cannot find any members named " + text);
					}
				}
			}
		}

		private class MyWhitelistBatch : Batch, IMyWhitelistBatch, IDisposable
		{
			public MyWhitelistBatch(MyScriptWhitelist whitelist)
				: base(whitelist)
			{
			}

			/// <summary>
			///     Adds the entire namespace of one or more given types.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="types"></param>
			public void AllowNamespaceOfTypes(MyWhitelistTarget target, params Type[] types)
			{
				if (types.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one type");
				}
				AssertVitality();
				for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (type == null)
					{
						throw new MyWhitelistException("The type in index " + i + " is null");
					}
					INamespaceSymbol containingNamespace = ResolveTypeSymbol(type).ContainingNamespace;
					if (containingNamespace != null && !containingNamespace.IsGlobalNamespace)
					{
						base.Whitelist.Register(target, containingNamespace, type);
					}
				}
			}

			/// <summary>
			///     Adds one or more specific types and all their members to the whitelist.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="types"></param>
			public void AllowTypes(MyWhitelistTarget target, params Type[] types)
			{
				if (types.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one type");
				}
				AssertVitality();
				for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (type == null)
					{
						throw new MyWhitelistException("The type in index " + i + " is null");
					}
					INamedTypeSymbol symbol = ResolveTypeSymbol(type);
					base.Whitelist.Register(target, symbol, type);
				}
			}

			/// <summary>
			///     Adds only the specified members to the whitelist.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="members"></param>
			public void AllowMembers(MyWhitelistTarget target, params MemberInfo[] members)
			{
				if (members.IsNullOrEmpty())
				{
					throw new MyWhitelistException("Needs at least one member");
				}
				AssertVitality();
				for (int i = 0; i < members.Length; i++)
				{
					MemberInfo member = members[i];
					if (member == null)
					{
						throw new MyWhitelistException("Element " + i + " is null");
					}
					List<ISymbol> list = Enumerable.ToList<ISymbol>(from m in ResolveTypeSymbol(member.DeclaringType).GetMembers()
						where m.MetadataName == member.Name
						select m);
					MethodInfo methodInfo = member as MethodInfo;
					ParameterInfo[] methodParameters = null;
					if (methodInfo != null)
					{
						methodParameters = methodInfo.GetParameters();
						list.RemoveAll((ISymbol s) => ((IMethodSymbol)s).Parameters.Length != methodParameters.Length);
						if (methodInfo.IsGenericMethodDefinition)
						{
							list.RemoveAll((ISymbol s) => !((IMethodSymbol)s).IsGenericMethod);
						}
						else
						{
							list.RemoveAll((ISymbol s) => ((IMethodSymbol)s).IsGenericMethod);
						}
						if (methodInfo.IsSpecialName && (methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_")))
						{
							throw new MyWhitelistException("Whitelist the actual properties, not their access methods");
						}
					}
					switch (list.Count)
					{
					case 0:
						throw new MyWhitelistException($"Cannot add {member} to the whitelist because its symbol variant could not be found.");
					case 1:
						base.Whitelist.RegisterMember(target, list[0], member);
						continue;
					}
					IMethodSymbol methodSymbol = FindMethodOverload(list, methodParameters);
					if (methodSymbol == null)
					{
						throw new MyWhitelistException($"Cannot add {member} to the whitelist because its symbol variant could not be found.");
					}
					base.Whitelist.RegisterMember(target, methodSymbol, member);
				}
			}

			private IMethodSymbol FindMethodOverload(IEnumerable<ISymbol> candidates, ParameterInfo[] methodParameters)
			{
				foreach (IMethodSymbol candidate in candidates)
				{
					ImmutableArray<IParameterSymbol> parameters = candidate.Parameters;
					bool flag = true;
					for (int i = 0; i < parameters.Length; i++)
					{
						ParameterInfo obj = methodParameters[i];
						Type type = obj.ParameterType;
						IParameterSymbol parameterSymbol = parameters[i];
						ITypeSymbol typeSymbol = parameterSymbol.Type;
						if (obj.IsOut && parameterSymbol.RefKind != RefKind.Out)
						{
							flag = false;
							break;
						}
						if (type.IsByRef)
						{
							if (parameterSymbol.RefKind != RefKind.Ref)
							{
								flag = false;
								break;
							}
							type = type.GetElementType();
						}
						if (type.IsPointer)
						{
							if (!(typeSymbol is IPointerTypeSymbol))
							{
								flag = false;
								break;
							}
							typeSymbol = ((IPointerTypeSymbol)typeSymbol).PointedAtType;
							type = type.GetElementType();
						}
						if (type.IsArray)
						{
							if (!(typeSymbol is IArrayTypeSymbol))
							{
								flag = false;
								break;
							}
							typeSymbol = ((IArrayTypeSymbol)typeSymbol).ElementType;
							type = type.GetElementType();
						}
						if (!object.Equals(ResolveTypeSymbol(type), typeSymbol))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return candidate;
					}
				}
				return null;
			}
		}

		private readonly HashSet<string> m_ingameBlacklist = new HashSet<string>();

		private readonly MyScriptCompiler m_scriptCompiler;

		private readonly Dictionary<string, MyWhitelistTarget> m_whitelist = new Dictionary<string, MyWhitelistTarget>();

		public MyScriptWhitelist(MyScriptCompiler scriptCompiler)
		{
			m_scriptCompiler = scriptCompiler;
<<<<<<< HEAD
			using (IMyWhitelistBatch myWhitelistBatch = OpenBatch())
			{
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(IEnumerator), typeof(HashSet<>), typeof(LinkedList<>), typeof(IEnumerator<>), typeof(StringBuilder), typeof(Regex), typeof(Calendar));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(Enumerable), typeof(ConcurrentBag<>), typeof(ConcurrentDictionary<, >));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Ingame, (from x in typeof(Enumerable).Assembly.GetTypes()
					where x.Namespace == "System.Linq"
					where !StringExtensions.Contains(x.Name, "parallel", StringComparison.InvariantCultureIgnoreCase)
					select x).ToArray());
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(Timer));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(TraceEventType), typeof(AssemblyProductAttribute), typeof(AssemblyDescriptionAttribute), typeof(AssemblyConfigurationAttribute), typeof(AssemblyCompanyAttribute), typeof(AssemblyCultureAttribute), typeof(AssemblyVersionAttribute), typeof(AssemblyFileVersionAttribute), typeof(AssemblyCopyrightAttribute), typeof(AssemblyTrademarkAttribute), typeof(AssemblyTitleAttribute), typeof(ComVisibleAttribute), typeof(DefaultValueAttribute), typeof(SerializableAttribute), typeof(GuidAttribute), typeof(StructLayoutAttribute), typeof(LayoutKind), typeof(Guid));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(object), typeof(IDisposable), typeof(string), typeof(StringComparison), typeof(Math), typeof(Enum), typeof(int), typeof(short), typeof(long), typeof(uint), typeof(ushort), typeof(ulong), typeof(double), typeof(float), typeof(bool), typeof(char), typeof(byte), typeof(sbyte), typeof(decimal), typeof(DateTime), typeof(TimeSpan), typeof(Array), typeof(XmlElementAttribute), typeof(XmlAttributeAttribute), typeof(XmlArrayAttribute), typeof(XmlArrayItemAttribute), typeof(XmlAnyAttributeAttribute), typeof(XmlAnyElementAttribute), typeof(XmlAnyElementAttributes), typeof(XmlArrayItemAttributes), typeof(XmlAttributeEventArgs), typeof(XmlAttributeOverrides), typeof(XmlAttributes), typeof(XmlChoiceIdentifierAttribute), typeof(XmlElementAttributes), typeof(XmlElementEventArgs), typeof(XmlEnumAttribute), typeof(XmlIgnoreAttribute), typeof(XmlIncludeAttribute), typeof(XmlRootAttribute), typeof(XmlTextAttribute), typeof(XmlTypeAttribute), typeof(RuntimeHelpers), typeof(BinaryReader), typeof(BinaryWriter), typeof(NullReferenceException), typeof(ArgumentException), typeof(ArgumentNullException), typeof(InvalidOperationException), typeof(FormatException), typeof(Exception), typeof(DivideByZeroException), typeof(InvalidCastException), typeof(FileNotFoundException), typeof(NotSupportedException), typeof(Nullable<>), typeof(StringComparer), typeof(IEquatable<>), typeof(IComparable), typeof(IComparable<>), typeof(BitConverter), typeof(FlagsAttribute), typeof(Path), typeof(Random), typeof(Convert), typeof(StringSplitOptions), typeof(DateTimeKind), typeof(MidpointRounding), typeof(EventArgs), typeof(Buffer), typeof(INotifyPropertyChanging), typeof(PropertyChangingEventHandler), typeof(PropertyChangingEventArgs), typeof(INotifyPropertyChanged), typeof(PropertyChangedEventHandler), typeof(PropertyChangedEventArgs));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(Stream), typeof(TextWriter), typeof(TextReader));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(MemberInfo).GetProperty("Name"));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(Type).GetProperty("FullName"), typeof(Type).GetMethod("GetTypeFromHandle"), typeof(Type).GetMethod("GetFields", new Type[1] { typeof(BindingFlags) }), typeof(Type).GetMethod("IsEquivalentTo"), typeof(Type).GetMethod("op_Equality"), typeof(Type).GetMethod("ToString"));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(ValueType).GetMethod("Equals"), typeof(ValueType).GetMethod("GetHashCode"), typeof(ValueType).GetMethod("ToString"));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(Environment).GetProperty("CurrentManagedThreadId", BindingFlags.Static | BindingFlags.Public), typeof(Environment).GetProperty("NewLine", BindingFlags.Static | BindingFlags.Public), typeof(Environment).GetProperty("ProcessorCount", BindingFlags.Static | BindingFlags.Public));
				Type type = typeof(Type).Assembly.GetType("System.RuntimeType");
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, type.GetMethod("op_Inequality"), type.GetMethod("GetFields", new Type[1] { typeof(BindingFlags) }));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, (from m in AllDeclaredMembers(typeof(Delegate))
					where m.Name != "CreateDelegate"
					select m).ToArray());
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(Action), typeof(Action<>), typeof(Action<, >), typeof(Action<, , >), typeof(Action<, , , >), typeof(Action<, , , , >), typeof(Action<, , , , , >), typeof(Action<, , , , , , >), typeof(Action<, , , , , , , >), typeof(Action<, , , , , , , , >), typeof(Action<, , , , , , , , , >), typeof(Action<, , , , , , , , , , >), typeof(Action<, , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , , , , >), typeof(Func<>), typeof(Func<, >), typeof(Func<, , >), typeof(Func<, , , >), typeof(Func<, , , , >), typeof(Func<, , , , , >), typeof(Func<, , , , , , >), typeof(Func<, , , , , , , >), typeof(Func<, , , , , , , , >), typeof(Func<, , , , , , , , , >), typeof(Func<, , , , , , , , , , >), typeof(Func<, , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , , , , >));
			}
=======
			using IMyWhitelistBatch myWhitelistBatch = OpenBatch();
			myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(IEnumerator), typeof(HashSet<>), typeof(Queue<>), typeof(IEnumerator<>), typeof(StringBuilder), typeof(Regex), typeof(Calendar));
			myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(Enumerable), typeof(ConcurrentBag<>), typeof(ConcurrentDictionary<, >));
			myWhitelistBatch.AllowTypes(MyWhitelistTarget.Ingame, Enumerable.ToArray<Type>(Enumerable.Where<Type>(Enumerable.Where<Type>((IEnumerable<Type>)typeof(Enumerable).Assembly.GetTypes(), (Func<Type, bool>)((Type x) => x.Namespace == "System.Linq")), (Func<Type, bool>)((Type x) => !StringExtensions.Contains(x.Name, "parallel", StringComparison.InvariantCultureIgnoreCase)))));
			myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(Timer));
			myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(TraceEventType), typeof(AssemblyProductAttribute), typeof(AssemblyDescriptionAttribute), typeof(AssemblyConfigurationAttribute), typeof(AssemblyCompanyAttribute), typeof(AssemblyCultureAttribute), typeof(AssemblyVersionAttribute), typeof(AssemblyFileVersionAttribute), typeof(AssemblyCopyrightAttribute), typeof(AssemblyTrademarkAttribute), typeof(AssemblyTitleAttribute), typeof(ComVisibleAttribute), typeof(DefaultValueAttribute), typeof(SerializableAttribute), typeof(GuidAttribute), typeof(StructLayoutAttribute), typeof(LayoutKind), typeof(Guid));
			myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(object), typeof(IDisposable), typeof(string), typeof(StringComparison), typeof(Math), typeof(Enum), typeof(int), typeof(short), typeof(long), typeof(uint), typeof(ushort), typeof(ulong), typeof(double), typeof(float), typeof(bool), typeof(char), typeof(byte), typeof(sbyte), typeof(decimal), typeof(DateTime), typeof(TimeSpan), typeof(Array), typeof(XmlElementAttribute), typeof(XmlAttributeAttribute), typeof(XmlArrayAttribute), typeof(XmlArrayItemAttribute), typeof(XmlAnyAttributeAttribute), typeof(XmlAnyElementAttribute), typeof(XmlAnyElementAttributes), typeof(XmlArrayItemAttributes), typeof(XmlAttributeEventArgs), typeof(XmlAttributeOverrides), typeof(XmlAttributes), typeof(XmlChoiceIdentifierAttribute), typeof(XmlElementAttributes), typeof(XmlElementEventArgs), typeof(XmlEnumAttribute), typeof(XmlIgnoreAttribute), typeof(XmlIncludeAttribute), typeof(XmlRootAttribute), typeof(XmlTextAttribute), typeof(XmlTypeAttribute), typeof(RuntimeHelpers), typeof(BinaryReader), typeof(BinaryWriter), typeof(NullReferenceException), typeof(ArgumentException), typeof(ArgumentNullException), typeof(InvalidOperationException), typeof(FormatException), typeof(Exception), typeof(DivideByZeroException), typeof(InvalidCastException), typeof(FileNotFoundException), typeof(NotSupportedException), typeof(Nullable<>), typeof(StringComparer), typeof(IEquatable<>), typeof(IComparable), typeof(IComparable<>), typeof(BitConverter), typeof(FlagsAttribute), typeof(Path), typeof(Random), typeof(Convert), typeof(StringSplitOptions), typeof(DateTimeKind), typeof(MidpointRounding), typeof(EventArgs), typeof(Buffer));
			myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(Stream), typeof(TextWriter), typeof(TextReader));
			myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(MemberInfo).GetProperty("Name"));
			myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(Type).GetProperty("FullName"), typeof(Type).GetMethod("GetTypeFromHandle"), typeof(Type).GetMethod("GetFields", new Type[1] { typeof(BindingFlags) }), typeof(Type).GetMethod("IsEquivalentTo"), typeof(Type).GetMethod("op_Equality"), typeof(Type).GetMethod("ToString"));
			myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(ValueType).GetMethod("Equals"), typeof(ValueType).GetMethod("GetHashCode"), typeof(ValueType).GetMethod("ToString"));
			myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(Environment).GetProperty("CurrentManagedThreadId", BindingFlags.Static | BindingFlags.Public), typeof(Environment).GetProperty("NewLine", BindingFlags.Static | BindingFlags.Public), typeof(Environment).GetProperty("ProcessorCount", BindingFlags.Static | BindingFlags.Public));
			Type type = typeof(Type).Assembly.GetType("System.RuntimeType");
			myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, type.GetMethod("op_Inequality"), type.GetMethod("GetFields", new Type[1] { typeof(BindingFlags) }));
			myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, Enumerable.ToArray<MemberInfo>(Enumerable.Where<MemberInfo>(AllDeclaredMembers(typeof(Delegate)), (Func<MemberInfo, bool>)((MemberInfo m) => m.Name != "CreateDelegate"))));
			myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(Action), typeof(Action<>), typeof(Action<, >), typeof(Action<, , >), typeof(Action<, , , >), typeof(Action<, , , , >), typeof(Action<, , , , , >), typeof(Action<, , , , , , >), typeof(Action<, , , , , , , >), typeof(Action<, , , , , , , , >), typeof(Action<, , , , , , , , , >), typeof(Action<, , , , , , , , , , >), typeof(Action<, , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , , , >), typeof(Action<, , , , , , , , , , , , , , , >), typeof(Func<>), typeof(Func<, >), typeof(Func<, , >), typeof(Func<, , , >), typeof(Func<, , , , >), typeof(Func<, , , , , >), typeof(Func<, , , , , , >), typeof(Func<, , , , , , , >), typeof(Func<, , , , , , , , >), typeof(Func<, , , , , , , , , >), typeof(Func<, , , , , , , , , , >), typeof(Func<, , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , , , >), typeof(Func<, , , , , , , , , , , , , , , , >));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static IEnumerable<MemberInfo> AllDeclaredMembers(Type type)
		{
			return Enumerable.Where<MemberInfo>((IEnumerable<MemberInfo>)type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public), (Func<MemberInfo, bool>)((MemberInfo m) => !IsPropertyMethod(m)));
		}

		private static bool IsPropertyMethod(MemberInfo memberInfo)
		{
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null && methodInfo.IsSpecialName)
			{
				if (!methodInfo.Name.StartsWith("get_"))
				{
					return methodInfo.Name.StartsWith("set_");
				}
				return true;
			}
			return false;
		}

		/// <summary>
		///     Opens the whitelist, allowing for addition of new members.
		/// </summary>
		/// <returns></returns>
		public IMyWhitelistBatch OpenBatch()
		{
			return new MyWhitelistBatch(this);
		}

		internal bool IsWhitelisted(ISymbol symbol, MyWhitelistTarget target)
		{
			INamedTypeSymbol namedTypeSymbol = symbol as INamedTypeSymbol;
			if (namedTypeSymbol != null)
			{
				return IsWhitelisted(namedTypeSymbol, target) != TypeKeyQuantity.None;
			}
			if (symbol.IsMemberSymbol())
			{
				return IsMemberWhitelisted(symbol, target);
			}
			return true;
		}

		private bool IsBlacklisted(ISymbol symbol)
		{
			if (symbol.IsMemberSymbol())
			{
				if (m_ingameBlacklist.Contains(symbol.GetWhitelistKey(TypeKeyQuantity.ThisOnly)))
				{
					return true;
				}
				symbol = symbol.ContainingType;
			}
			for (ITypeSymbol typeSymbol = symbol as ITypeSymbol; typeSymbol != null; typeSymbol = typeSymbol.ContainingType)
			{
				if (m_ingameBlacklist.Contains(typeSymbol.GetWhitelistKey(TypeKeyQuantity.AllMembers)))
				{
					return true;
				}
			}
			return false;
		}

		private TypeKeyQuantity IsWhitelisted(INamespaceSymbol namespaceSymbol, MyWhitelistTarget target)
		{
			if (m_whitelist.TryGetValue(namespaceSymbol.GetWhitelistKey(TypeKeyQuantity.AllMembers), out var value) && value.HasFlag(target))
			{
				return TypeKeyQuantity.AllMembers;
			}
			return TypeKeyQuantity.None;
		}

		private TypeKeyQuantity IsWhitelisted(INamedTypeSymbol typeSymbol, MyWhitelistTarget target)
		{
			if (target == MyWhitelistTarget.Ingame && IsBlacklisted(typeSymbol))
			{
				return TypeKeyQuantity.None;
			}
			TypeKeyQuantity typeKeyQuantity = IsWhitelisted(typeSymbol.ContainingNamespace, target);
			if (typeKeyQuantity == TypeKeyQuantity.AllMembers)
			{
				return typeKeyQuantity;
			}
			if (m_whitelist.TryGetValue(typeSymbol.GetWhitelistKey(TypeKeyQuantity.AllMembers), out var value) && value.HasFlag(target))
			{
				return TypeKeyQuantity.AllMembers;
			}
			if (m_whitelist.TryGetValue(typeSymbol.GetWhitelistKey(TypeKeyQuantity.ThisOnly), out value) && value.HasFlag(target))
			{
				return TypeKeyQuantity.ThisOnly;
			}
			return TypeKeyQuantity.None;
		}

		private bool IsMemberWhitelisted(ISymbol memberSymbol, MyWhitelistTarget target)
		{
			do
			{
				if (target == MyWhitelistTarget.Ingame && IsBlacklisted(memberSymbol))
				{
					return false;
				}
				if (IsWhitelisted(memberSymbol.ContainingType, target) == TypeKeyQuantity.AllMembers)
				{
					return true;
				}
				if (m_whitelist.TryGetValue(memberSymbol.GetWhitelistKey(TypeKeyQuantity.ThisOnly), out var value) && value.HasFlag(target))
				{
					return true;
				}
				if (!memberSymbol.IsOverride)
				{
					break;
				}
				memberSymbol = memberSymbol.GetOverriddenSymbol();
			}
			while (memberSymbol != null);
			return false;
		}

		private CSharpCompilation CreateCompilation()
		{
			return m_scriptCompiler.CreateCompilation(null, null, enableDebugInformation: false);
		}

		private void RegisterMember(MyWhitelistTarget target, ISymbol symbol, MemberInfo member)
		{
			if (!(symbol is IEventSymbol) && !(symbol is IFieldSymbol) && !(symbol is IPropertySymbol) && !(symbol is IMethodSymbol))
			{
				throw new MyWhitelistException("Unsupported symbol type " + symbol);
			}
			INamespaceSymbol containingNamespace = symbol.ContainingNamespace;
			if (containingNamespace != null && !containingNamespace.IsGlobalNamespace)
			{
				string whitelistKey = containingNamespace.GetWhitelistKey(TypeKeyQuantity.AllMembers);
				if (m_whitelist.TryGetValue(whitelistKey, out var value) && value >= target)
				{
					throw new MyWhitelistException(string.Concat("The member ", member, " is covered by the ", whitelistKey, " rule"));
				}
			}
			for (INamedTypeSymbol containingType = symbol.ContainingType; containingType != null; containingType = containingType.ContainingType)
			{
				string whitelistKey2 = containingType.GetWhitelistKey(TypeKeyQuantity.AllMembers);
				if (m_whitelist.TryGetValue(whitelistKey2, out var value2) && value2 >= target)
				{
					throw new MyWhitelistException(string.Concat("The member ", member, " is covered by the ", whitelistKey2, " rule"));
				}
				whitelistKey2 = containingType.GetWhitelistKey(TypeKeyQuantity.ThisOnly);
				if (!m_whitelist.TryGetValue(whitelistKey2, out value2) || value2 < target)
				{
					m_whitelist[whitelistKey2] = target;
				}
			}
			string whitelistKey3 = symbol.GetWhitelistKey(TypeKeyQuantity.ThisOnly);
			if (m_whitelist.ContainsKey(whitelistKey3))
			{
				throw new MyWhitelistException("Duplicate registration of the whitelist key " + whitelistKey3 + " retrieved from " + member);
			}
			m_whitelist.Add(whitelistKey3, target);
		}

		private void Register(MyWhitelistTarget target, INamespaceSymbol symbol, Type type)
		{
			string whitelistKey = symbol.GetWhitelistKey(TypeKeyQuantity.AllMembers);
			if (m_whitelist.ContainsKey(whitelistKey))
			{
				throw new MyWhitelistException("Duplicate registration of the whitelist key " + whitelistKey + " retrieved from " + type);
			}
			m_whitelist.Add(whitelistKey, target);
		}

		private void Register(MyWhitelistTarget target, ITypeSymbol symbol, Type type)
		{
			INamespaceSymbol containingNamespace = symbol.ContainingNamespace;
			if (containingNamespace != null && !containingNamespace.IsGlobalNamespace)
			{
				string whitelistKey = containingNamespace.GetWhitelistKey(TypeKeyQuantity.AllMembers);
				if (m_whitelist.TryGetValue(whitelistKey, out var value) && value >= target)
				{
					throw new MyWhitelistException(string.Concat("The type ", type, " is covered by the ", whitelistKey, " rule"));
				}
			}
			string whitelistKey2 = symbol.GetWhitelistKey(TypeKeyQuantity.AllMembers);
			if (m_whitelist.ContainsKey(whitelistKey2))
			{
				throw new MyWhitelistException("Duplicate registration of the whitelist key " + whitelistKey2 + " retrieved from " + type);
			}
			m_whitelist.Add(whitelistKey2, target);
		}

		/// <summary>
		///     Clears the whitelist.
		/// </summary>
		public void Clear()
		{
			m_whitelist.Clear();
			m_ingameBlacklist.Clear();
		}

		public DictionaryReader<string, MyWhitelistTarget> GetWhitelist()
		{
			return new DictionaryReader<string, MyWhitelistTarget>(m_whitelist);
		}

		public HashSetReader<string> GetBlacklistedIngameEntries()
		{
			return m_ingameBlacklist;
		}

		public IMyScriptBlacklistBatch OpenIngameBlacklistBatch()
		{
			return new MyScriptBlacklistBatch(this);
		}
	}
}
