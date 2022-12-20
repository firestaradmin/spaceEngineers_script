using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using VRage.FileSystem;

namespace VRage.Trace
{
	internal class MyWintraceWrapper : ITrace
	{
		private static readonly Type m_winTraceType;

		private static Type m_ttraceType;

		private static readonly object m_winWatches;

		private readonly object m_trace;

		private readonly Action m_clearAll;

		private readonly Action<string, object> m_send;

		private readonly Action<string, string> m_debugSend;

		/// <inheritdoc />
		public bool Enabled { get; set; } = true;


		static MyWintraceWrapper()
		{
<<<<<<< HEAD
			Assembly assembly = TryLoad("TraceTool.dll") ?? TryLoad(MyFileSystem.ExePath + "/../../../../../../3rd/TraceTool/TraceTool.dll") ?? TryLoad(MyFileSystem.ExePath + "/../../../3rd/TraceTool/TraceTool.dll") ?? TryLoad(Environment.CurrentDirectory + "/../../../../../3rd/TraceTool/TraceTool.dll");
=======
			Assembly assembly = TryLoad("TraceTool.dll") ?? TryLoad(MyFileSystem.ExePath + "/../../../../../../3rd/TraceTool/TraceTool.dll") ?? TryLoad(MyFileSystem.ExePath + "/../../../3rd/TraceTool/TraceTool.dll") ?? TryLoad(Environment.get_CurrentDirectory() + "/../../../../../3rd/TraceTool/TraceTool.dll");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (assembly != null)
			{
				m_winTraceType = assembly.GetType("TraceTool.WinTrace");
				m_ttraceType = assembly.GetType("TraceTool.TTrace");
				m_winWatches = m_ttraceType.GetProperty("Watches").GetGetMethod().Invoke(null, new object[0]);
			}
		}

		private static Assembly TryLoad(string assembly)
		{
			if (!File.Exists(assembly))
			{
				return null;
			}
			try
			{
				return Assembly.LoadFrom(assembly);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static ITrace CreateTrace(string id, string name)
		{
			if (m_winTraceType != null)
			{
				return new MyWintraceWrapper(Activator.CreateInstance(m_winTraceType, id, name));
			}
			return new MyNullTrace();
		}

		private MyWintraceWrapper(object trace)
		{
			m_trace = trace;
			m_clearAll = Expression.Lambda<Action>((Expression)(object)Expression.Call((Expression)(object)Expression.Constant(m_trace), trace.GetType().GetMethod("ClearAll")), Array.Empty<ParameterExpression>()).Compile();
			m_clearAll();
<<<<<<< HEAD
			ParameterExpression parameterExpression = Expression.Parameter(typeof(string));
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
			MethodCallExpression body = Expression.Call(Expression.Constant(m_winWatches), m_winWatches.GetType().GetMethod("Send"), parameterExpression, parameterExpression2);
			m_send = Expression.Lambda<Action<string, object>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(string));
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(string));
			MemberExpression memberExpression = Expression.PropertyOrField(Expression.Constant(m_trace), "Debug");
			MethodCallExpression body2 = Expression.Call(memberExpression, memberExpression.Expression.Type.GetMethod("Send", new Type[2]
			{
				typeof(string),
				typeof(string)
			}), parameterExpression3, parameterExpression4);
			m_debugSend = Expression.Lambda<Action<string, string>>(body2, new ParameterExpression[2] { parameterExpression3, parameterExpression4 }).Compile();
=======
			ParameterExpression val = Expression.Parameter(typeof(string));
			ParameterExpression val2 = Expression.Parameter(typeof(object));
			MethodCallExpression val3 = Expression.Call((Expression)(object)Expression.Constant(m_winWatches), m_winWatches.GetType().GetMethod("Send"), (Expression)(object)val, (Expression)(object)val2);
			m_send = Expression.Lambda<Action<string, object>>((Expression)(object)val3, (ParameterExpression[])(object)new ParameterExpression[2] { val, val2 }).Compile();
			ParameterExpression val4 = Expression.Parameter(typeof(string));
			ParameterExpression val5 = Expression.Parameter(typeof(string));
			MemberExpression val6 = Expression.PropertyOrField((Expression)(object)Expression.Constant(m_trace), "Debug");
			MethodCallExpression val7 = Expression.Call((Expression)(object)val6, val6.get_Expression().get_Type().GetMethod("Send", new Type[2]
			{
				typeof(string),
				typeof(string)
			}), (Expression)(object)val4, (Expression)(object)val5);
			m_debugSend = Expression.Lambda<Action<string, string>>((Expression)(object)val7, (ParameterExpression[])(object)new ParameterExpression[2] { val4, val5 }).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void Send(string msg, string comment = null)
		{
			if (Enabled)
			{
				try
				{
					m_debugSend(msg, comment);
				}
				catch
				{
				}
			}
		}

		/// <inheritdoc />
		public void Flush()
		{
			m_ttraceType.GetMethod("Flush").Invoke(null, null);
		}

		public void Watch(string name, object value)
		{
			if (Enabled)
			{
				try
				{
					m_send(name, value);
				}
				catch
				{
				}
			}
		}
	}
}
