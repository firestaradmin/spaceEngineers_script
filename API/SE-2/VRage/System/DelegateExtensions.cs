using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
<<<<<<< HEAD
=======
using VRage.Library;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

namespace System
{
	public static class DelegateExtensions
	{
		private const bool ProfileDelegateInvocations = false;

		private static Func<object, int> m_getInvocationCount;

		private static Func<object, object[]> m_getInvocationList;

		[ThreadStatic]
		private static List<Delegate> m_invocationList;

		private const bool __Dummy_ = false;

		private static Action<string> m_profilerBegin = delegate
		{
		};

		private static Action<int> m_profilerEnd = delegate
		{
		};

<<<<<<< HEAD
		private static ConcurrentDictionary<MethodInfo, string> m_methodNameCache = new ConcurrentDictionary<MethodInfo, string>(Environment.ProcessorCount * 4, 100);
=======
		private static ConcurrentDictionary<MethodInfo, string> m_methodNameCache = new ConcurrentDictionary<MethodInfo, string>(MyEnvironment.ProcessorCount * 4, 100);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InvokeIfNotNull(this Action handler)
		{
			handler?.Invoke();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InvokeIfNotNull<T1>(this Action<T1> handler, T1 arg1)
		{
			handler?.Invoke(arg1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InvokeIfNotNull<T1, T2>(this Action<T1, T2> handler, T1 arg1, T2 arg2)
		{
			handler?.Invoke(arg1, arg2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InvokeIfNotNull<T1, T2, T3>(this Action<T1, T2, T3> handler, T1 arg1, T2 arg2, T3 arg3)
		{
			handler?.Invoke(arg1, arg2, arg3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InvokeIfNotNull<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			handler?.Invoke(arg1, arg2, arg3, arg4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InvokeIfNotNull<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			handler?.Invoke(arg1, arg2, arg3, arg4, arg5);
		}

		public static void GetInvocationList(this MulticastDelegate @delegate, List<Delegate> result)
		{
			if (m_getInvocationList == null)
			{
				FieldInfo field = typeof(MulticastDelegate).GetField("_invocationList", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field == null)
				{
					field = typeof(MulticastDelegate).GetField("delegates", BindingFlags.Instance | BindingFlags.NonPublic);
				}
				Func<object, object> getter2 = field.CreateGetter<object, object>();
				m_getInvocationList = (object x) => getter2(x) as object[];
			}
			if (m_getInvocationCount == null)
			{
				FieldInfo field2 = typeof(MulticastDelegate).GetField("_invocationCount", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field2 != null)
				{
					Func<object, IntPtr> getter = field2.CreateGetter<object, IntPtr>();
					m_getInvocationCount = (object x) => (int)getter(x);
				}
				else
				{
					m_getInvocationCount = (object x) => m_getInvocationList(x).Length;
				}
			}
			object[] array = m_getInvocationList(@delegate);
			if (array == null)
			{
				result.Add(@delegate);
				return;
			}
			int num = m_getInvocationCount(@delegate);
			for (int i = 0; i < num; i++)
			{
				result.Add((Delegate)array[i]);
			}
		}

		public static void SetupProfiler(Action<string> begin, Action<int> end)
		{
			m_profilerBegin = begin;
			m_profilerEnd = end;
		}

		private static void InvokeProfiled<TDelegate, T1, T2, T3, T4, T5>(Action<TDelegate, T1, T2, T3, T4, T5> handler, TDelegate @delegate, T1 _1, T2 _2, T3 _3, T4 _4, T5 _5)
		{
			if (m_invocationList == null)
			{
				m_invocationList = new List<Delegate>();
			}
			int count = m_invocationList.Count;
			m_profilerBegin("DelegateInvoke");
			try
			{
				((MulticastDelegate)(object)@delegate).GetInvocationList(m_invocationList);
				int count2 = m_invocationList.Count;
				for (int i = count; i < count2; i++)
				{
					Delegate delegate2 = m_invocationList[i];
					m_profilerBegin(GetMethodName(delegate2.Method));
					try
					{
						handler((TDelegate)(object)delegate2, _1, _2, _3, _4, _5);
					}
					finally
					{
						m_profilerEnd(0);
					}
				}
			}
			finally
			{
				int num = m_invocationList.Count - count;
				m_invocationList.RemoveRange(count, num);
				m_profilerEnd(num);
			}
		}

		private static string GetMethodName(MethodInfo method)
		{
<<<<<<< HEAD
			if (!m_methodNameCache.TryGetValue(method, out var value))
			{
				string text = (method.IsStatic ? "." : "#");
				value = method.DeclaringType.Name + text + method.Name;
				return m_methodNameCache.GetOrAdd(method, value);
			}
			return value;
=======
			string result = default(string);
			if (!m_methodNameCache.TryGetValue(method, ref result))
			{
				string text = (method.IsStatic ? "." : "#");
				result = method.DeclaringType.Name + text + method.Name;
				return m_methodNameCache.GetOrAdd(method, result);
			}
			return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
