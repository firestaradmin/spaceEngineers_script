using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VRage.Library.Collections;
using VRage.Serialization;

namespace VRage.Network
{
	public class MyEventTable
	{
		private class EventReturn
		{
			public MethodInfo Method;

			public EventAttribute Event;

			public EventReturn(EventAttribute _event, MethodInfo _method)
			{
				Event = _event;
				Method = _method;
			}
		}

		private MethodInfo m_createCallSite = typeof(MyEventTable).GetMethod("CreateCallSite", BindingFlags.Instance | BindingFlags.NonPublic);

		private Dictionary<uint, CallSite> m_idToEvent;

		private Dictionary<MethodInfo, CallSite> m_methodInfoLookup;

		private ConcurrentDictionary<object, CallSite> m_associateObjectLookup;

		public readonly MySynchronizedTypeInfo Type;

		public int Count => m_idToEvent.Count;

		public MyEventTable(MySynchronizedTypeInfo type)
		{
			Type = type;
			if (type != null && type.BaseType != null)
			{
				m_idToEvent = new Dictionary<uint, CallSite>(type.BaseType.EventTable.m_idToEvent);
				m_methodInfoLookup = new Dictionary<MethodInfo, CallSite>(type.BaseType.EventTable.m_methodInfoLookup);
				m_associateObjectLookup = new ConcurrentDictionary<object, CallSite>((IEnumerable<KeyValuePair<object, CallSite>>)type.BaseType.EventTable.m_associateObjectLookup);
			}
			else
			{
				m_idToEvent = new Dictionary<uint, CallSite>();
				m_methodInfoLookup = new Dictionary<MethodInfo, CallSite>();
				m_associateObjectLookup = new ConcurrentDictionary<object, CallSite>();
			}
			if (Type != null)
			{
				RegisterEvents();
			}
		}

		public CallSite Get(uint id)
		{
			return m_idToEvent[id];
		}

		public CallSite Get<T>(object associatedObject, Func<T, Delegate> getter, T arg)
		{
<<<<<<< HEAD
			if (!m_associateObjectLookup.TryGetValue(associatedObject, out var value))
=======
			CallSite result = default(CallSite);
			if (!m_associateObjectLookup.TryGetValue(associatedObject, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MethodInfo method = getter(arg).Method;
				result = m_methodInfoLookup[method];
				return m_associateObjectLookup.GetOrAdd(associatedObject, result);
			}
			return result;
		}

		public bool TryGet<T>(object associatedObject, Func<T, Delegate> getter, T arg, out CallSite site)
		{
			if (!m_associateObjectLookup.TryGetValue(associatedObject, ref site))
			{
				MethodInfo method = getter(arg).Method;
				if (m_methodInfoLookup.TryGetValue(method, out site))
				{
					site = m_associateObjectLookup.GetOrAdd(associatedObject, site);
					return true;
				}
				return false;
			}
			return true;
		}

		public void AddStaticEvents(Type fromType)
		{
			RegisterEvents(fromType, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		private void RegisterEvents()
		{
			RegisterEvents(Type.Type, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		private void RegisterEvents(Type type, BindingFlags flags)
		{
<<<<<<< HEAD
			foreach (EventReturn item in from s in type.GetMethods(flags)
				select new EventReturn(CustomAttributeExtensions.GetCustomAttribute<EventAttribute>(s), s) into s
				where s.Event != null
				orderby s.Event.Order
				select s)
=======
			foreach (EventReturn item in (IEnumerable<EventReturn>)Enumerable.OrderBy<EventReturn, int>(Enumerable.Where<EventReturn>(Enumerable.Select<MethodInfo, EventReturn>((IEnumerable<MethodInfo>)type.GetMethods(flags), (Func<MethodInfo, EventReturn>)((MethodInfo s) => new EventReturn(s.GetCustomAttribute<EventAttribute>(), s))), (Func<EventReturn, bool>)((EventReturn s) => s.Event != null)), (Func<EventReturn, int>)((EventReturn s) => s.Event.Order)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MethodInfo method = item.Method;
				Type type2 = (method.IsStatic ? typeof(IMyEventOwner) : method.DeclaringType);
				Type[] array = new Type[7]
				{
					type2,
					typeof(DBNull),
					typeof(DBNull),
					typeof(DBNull),
					typeof(DBNull),
					typeof(DBNull),
					typeof(DBNull)
				};
				ParameterInfo[] parameters = method.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i + 1] = parameters[i].ParameterType;
				}
				CallSite callSite = (CallSite)m_createCallSite.MakeGenericMethod(array).Invoke(this, new object[2]
				{
					method,
					(uint)m_idToEvent.Count
				});
				if ((callSite.HasBroadcastExceptFlag ? 1 : 0) + (callSite.HasBroadcastFlag ? 1 : 0) + (callSite.HasClientFlag ? 1 : 0) > 1)
				{
					throw new InvalidOperationException($"Event '{callSite}' can have only one of [Client], [Broadcast], [BroadcastExcept] attributes");
				}
				m_idToEvent.Add(callSite.Id, callSite);
				m_methodInfoLookup.Add(method, callSite);
			}
		}

		private CallSite CreateCallSite<T1, T2, T3, T4, T5, T6, T7>(MethodInfo info, uint id)
		{
			CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7> handler = null;
			ICallSite callSite = CodegenUtils.GetCallSite(info);
			ICallSite<T1, T2, T3, T4, T5, T6, T7> callSite2;
			if ((callSite2 = callSite as ICallSite<T1, T2, T3, T4, T5, T6, T7>) != null)
			{
				handler = callSite2.Invoke;
			}
			if (callSite == null)
			{
				ParameterExpression[] array = Enumerable.ToArray<ParameterExpression>(Enumerable.Select<Type, ParameterExpression>((IEnumerable<Type>)new Type[7]
				{
					typeof(T1),
					typeof(T2),
					typeof(T3),
					typeof(T4),
					typeof(T5),
					typeof(T6),
					typeof(T7)
				}, (Func<Type, ParameterExpression>)((Type s) => Expression.Parameter(s.MakeByRefType()))));
				Expression val;
				if (info.IsStatic)
				{
					Expression[] array2 = (Expression[])(object)Enumerable.ToArray<ParameterExpression>(Enumerable.Where<ParameterExpression>(Enumerable.Skip<ParameterExpression>((IEnumerable<ParameterExpression>)array, 1), (Func<ParameterExpression, bool>)((ParameterExpression s) => ((Expression)s).get_Type() != typeof(DBNull))));
					val = (Expression)(object)Expression.Call(info, array2);
				}
				else
				{
					ParameterExpression obj = Enumerable.First<ParameterExpression>((IEnumerable<ParameterExpression>)array);
					Expression[] array2 = (Expression[])(object)Enumerable.ToArray<ParameterExpression>(Enumerable.Where<ParameterExpression>(Enumerable.Skip<ParameterExpression>((IEnumerable<ParameterExpression>)array, 1), (Func<ParameterExpression, bool>)((ParameterExpression s) => ((Expression)s).get_Type() != typeof(DBNull))));
					val = (Expression)(object)Expression.Call((Expression)(object)obj, info, array2);
				}
				handler = Expression.Lambda<CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7>>(val, array).Compile();
			}
<<<<<<< HEAD
			EventAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<EventAttribute>(info);
			ServerAttribute customAttribute2 = CustomAttributeExtensions.GetCustomAttribute<ServerAttribute>(info);
=======
			EventAttribute customAttribute = info.GetCustomAttribute<EventAttribute>();
			ServerAttribute customAttribute2 = info.GetCustomAttribute<ServerAttribute>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			float distanceRadius = 0f;
			CallSiteFlags callSiteFlags = CallSiteFlags.None;
			if (customAttribute2 != null)
			{
				callSiteFlags |= CallSiteFlags.Server;
			}
			if (customAttribute2 is ServerInvokedAttribute)
			{
				callSiteFlags |= CallSiteFlags.ServerInvoked;
			}
			if (info.HasAttribute<ClientAttribute>())
			{
				callSiteFlags |= CallSiteFlags.Client;
			}
			if (info.HasAttribute<BroadcastAttribute>())
			{
				callSiteFlags |= CallSiteFlags.Broadcast;
			}
			if (info.HasAttribute<BroadcastExceptAttribute>())
			{
				callSiteFlags |= CallSiteFlags.BroadcastExcept;
			}
			if (info.HasAttribute<ReliableAttribute>())
			{
				callSiteFlags |= CallSiteFlags.Reliable;
			}
			if (info.HasAttribute<RefreshReplicableAttribute>())
			{
				callSiteFlags |= CallSiteFlags.RefreshReplicable;
			}
			if (info.HasAttribute<BlockingAttribute>())
			{
				callSiteFlags |= CallSiteFlags.Blocking;
			}
			if (info.HasAttribute<DistanceRadiusAttribute>())
			{
<<<<<<< HEAD
				distanceRadius = CustomAttributeExtensions.GetCustomAttribute<DistanceRadiusAttribute>(info).DistanceRadius;
=======
				distanceRadius = info.GetCustomAttribute<DistanceRadiusAttribute>().DistanceRadius;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				callSiteFlags |= CallSiteFlags.DistanceRadius;
			}
			SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> serializeDelegate = null;
			Func<T1, T2, T3, T4, T5, T6, T7, bool> func = null;
			if (customAttribute.Serialization != null)
			{
				MethodInfo method = info.DeclaringType.GetMethod(customAttribute.Serialization, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method == null)
				{
					throw new InvalidOperationException($"Serialization method '{customAttribute.Serialization}' for event '{info.Name}' defined by type '{info.DeclaringType.Name}' not found");
				}
				if (!Enumerable.All<ParameterInfo>(Enumerable.Skip<ParameterInfo>((IEnumerable<ParameterInfo>)method.GetParameters(), 1), (Func<ParameterInfo, bool>)((ParameterInfo s) => s.ParameterType.IsByRef)))
				{
					throw new InvalidOperationException($"Serialization method '{customAttribute.Serialization}' for event '{info.Name}' defined by type '{info.DeclaringType.Name}' must have all arguments passed with 'ref' keyword (except BitStream)");
				}
				ParameterExpression[] array3 = MethodInfoExtensions.ExtractParameterExpressionsFrom<SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>>();
				ParameterExpression obj2 = Enumerable.First<ParameterExpression>((IEnumerable<ParameterExpression>)array3);
				Expression[] array2 = (Expression[])(object)Enumerable.ToArray<ParameterExpression>(Enumerable.Where<ParameterExpression>(Enumerable.Skip<ParameterExpression>((IEnumerable<ParameterExpression>)array3, 1), (Func<ParameterExpression, bool>)((ParameterExpression s) => ((Expression)s).get_Type() != typeof(DBNull))));
				serializeDelegate = Expression.Lambda<SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>>((Expression)(object)Expression.Call((Expression)(object)obj2, method, array2), array3).Compile();
			}
			if (customAttribute2 != null && customAttribute2.Validation != null)
			{
				MethodInfo method2 = info.DeclaringType.GetMethod(customAttribute2.Validation, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method2 == null)
				{
					throw new InvalidOperationException($"Validation method '{customAttribute2.Validation}' for event '{info.Name}' defined by type '{info.DeclaringType.Name}' not found");
				}
				ParameterExpression[] array4 = MethodInfoExtensions.ExtractParameterExpressionsFrom<Func<T1, T2, T3, T4, T5, T6, T7, bool>>();
				ParameterExpression obj3 = Enumerable.First<ParameterExpression>((IEnumerable<ParameterExpression>)array4);
				Expression[] array2 = (Expression[])(object)Enumerable.ToArray<ParameterExpression>(Enumerable.Where<ParameterExpression>(Enumerable.Skip<ParameterExpression>((IEnumerable<ParameterExpression>)array4, 1), (Func<ParameterExpression, bool>)((ParameterExpression s) => ((Expression)s).get_Type() != typeof(DBNull))));
				func = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, bool>>((Expression)(object)Expression.Call((Expression)(object)obj3, method2, array2), array4).Compile();
			}
			ValidationType validationFlags = ValidationType.None;
			if (customAttribute2 != null)
			{
				validationFlags = customAttribute2.ValidationFlags;
			}
			serializeDelegate = serializeDelegate ?? CreateSerializer<T1, T2, T3, T4, T5, T6, T7>(info);
			func = func ?? CreateValidator<T1, T2, T3, T4, T5, T6, T7>();
			return new CallSite<T1, T2, T3, T4, T5, T6, T7>(Type, id, info, callSiteFlags, handler, serializeDelegate, func, validationFlags, distanceRadius);
		}

		private SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> CreateSerializer<T1, T2, T3, T4, T5, T6, T7>(MethodInfo info)
		{
			MySerializer<T2> s2 = MyFactory.GetSerializer<T2>();
			MySerializer<T3> s3 = MyFactory.GetSerializer<T3>();
			MySerializer<T4> s4 = MyFactory.GetSerializer<T4>();
			MySerializer<T5> s5 = MyFactory.GetSerializer<T5>();
			MySerializer<T6> s6 = MyFactory.GetSerializer<T6>();
			MySerializer<T7> s7 = MyFactory.GetSerializer<T7>();
			ParameterInfo[] parameters = info.GetParameters();
			MySerializeInfo info2 = MySerializeInfo.CreateForParameter(parameters, 0);
			MySerializeInfo info3 = MySerializeInfo.CreateForParameter(parameters, 1);
			MySerializeInfo info4 = MySerializeInfo.CreateForParameter(parameters, 2);
			MySerializeInfo info5 = MySerializeInfo.CreateForParameter(parameters, 3);
			MySerializeInfo info6 = MySerializeInfo.CreateForParameter(parameters, 4);
			MySerializeInfo info7 = MySerializeInfo.CreateForParameter(parameters, 5);
			return delegate(T1 inst, BitStream stream, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7)
			{
				if (stream.Reading)
				{
					MySerializationHelpers.CreateAndRead(stream, out arg2, s2, info2);
					MySerializationHelpers.CreateAndRead(stream, out arg3, s3, info3);
					MySerializationHelpers.CreateAndRead(stream, out arg4, s4, info4);
					MySerializationHelpers.CreateAndRead(stream, out arg5, s5, info5);
					MySerializationHelpers.CreateAndRead(stream, out arg6, s6, info6);
					MySerializationHelpers.CreateAndRead(stream, out arg7, s7, info7);
				}
				else
				{
					MySerializationHelpers.Write(stream, ref arg2, s2, info2);
					MySerializationHelpers.Write(stream, ref arg3, s3, info3);
					MySerializationHelpers.Write(stream, ref arg4, s4, info4);
					MySerializationHelpers.Write(stream, ref arg5, s5, info5);
					MySerializationHelpers.Write(stream, ref arg6, s6, info6);
					MySerializationHelpers.Write(stream, ref arg7, s7, info7);
				}
			};
		}

		private Func<T1, T2, T3, T4, T5, T6, T7, bool> CreateValidator<T1, T2, T3, T4, T5, T6, T7>()
		{
			return (T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7) => true;
		}
	}
}
