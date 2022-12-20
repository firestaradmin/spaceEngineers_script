using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace VRage.Native
{
	public class NativeCallHelper<TDelegate> where TDelegate : class
	{
		public static readonly TDelegate Invoke = Create();

		private unsafe static TDelegate Create()
		{
			MethodInfo method = typeof(TDelegate).GetMethod("Invoke");
			Type[] array = (from s in method.GetParameters()
				select s.ParameterType).ToArray();
			if (array.Length == 0 || array[0] != typeof(IntPtr))
			{
				throw new InvalidOperationException("First parameter must be function pointer");
			}
			Type[] parameterTypes = (from s in array.Skip(1)
				select (!(s == typeof(IntPtr))) ? s : typeof(void*)).ToArray();
			DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, method.ReturnType, array, Assembly.GetExecutingAssembly().ManifestModule);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			for (int i = 1; i < array.Length; i++)
			{
				iLGenerator.Emit(OpCodes.Ldarg, i);
			}
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Ldind_I);
			iLGenerator.EmitCalli(OpCodes.Calli, CallingConvention.StdCall, method.ReturnType, parameterTypes);
			iLGenerator.Emit(OpCodes.Ret);
			return CreateDelegate(dynamicMethod);
		}

		private static TDelegate CreateDelegate(MethodInfo method)
		{
			return CreateDelegate(method, (Type[] typeArguments, ParameterExpression[] parameterExpressions) => Expression.Call(method, ProvideStrongArgumentsFor(method, parameterExpressions)));
		}

		private static TDelegate CreateDelegate(MethodBase method, Func<Type[], ParameterExpression[], MethodCallExpression> getCallExpression)
		{
			ParameterExpression[] array = ExtractParameterExpressionsFrom();
			CheckParameterCountsAreEqual(array, method.GetParameters());
			return Expression.Lambda<TDelegate>(getCallExpression(GetTypeArgumentsFor(method), array), array).Compile();
		}

		private static ParameterExpression[] ExtractParameterExpressionsFrom()
		{
			return (from s in typeof(TDelegate).GetMethod("Invoke").GetParameters()
				select Expression.Parameter(s.ParameterType)).ToArray();
		}

		private static void CheckParameterCountsAreEqual(IEnumerable<ParameterExpression> delegateParameters, IEnumerable<ParameterInfo> methodParameters)
		{
			if (delegateParameters.Count() != methodParameters.Count())
			{
				throw new InvalidOperationException("The number of parameters of the requested delegate does not match the number parameters of the specified method.");
			}
		}

		private static Type[] GetTypeArgumentsFor(MethodBase method)
		{
			return null;
		}

		private static Expression[] ProvideStrongArgumentsFor(MethodInfo method, ParameterExpression[] parameterExpressions)
		{
			return method.GetParameters().Select((ParameterInfo parameter, int index) => Expression.Convert(parameterExpressions[index], parameter.ParameterType)).ToArray();
		}
	}
}
