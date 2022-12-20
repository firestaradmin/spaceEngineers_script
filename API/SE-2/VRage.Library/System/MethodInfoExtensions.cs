using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
	public static class MethodInfoExtensions
	{
		public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object instance) where TDelegate : Delegate
		{
			return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, method);
		}

		public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method) where TDelegate : Delegate
		{
			return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), method);
		}

		public static ParameterExpression[] ExtractParameterExpressionsFrom<TDelegate>()
		{
			return Enumerable.ToArray<ParameterExpression>(Enumerable.Select<ParameterInfo, ParameterExpression>((IEnumerable<ParameterInfo>)typeof(TDelegate).GetMethod("Invoke").GetParameters(), (Func<ParameterInfo, ParameterExpression>)((ParameterInfo s) => Expression.Parameter(s.ParameterType))));
		}
	}
}
