using System;

namespace VRage.Native
{
	public static class NativeCall
	{
		public static void Function(IntPtr address)
		{
			NativeCallHelper<Action<IntPtr>>.Invoke(address);
		}

		public static void Function<TArg1>(IntPtr address, TArg1 arg1)
		{
			NativeCallHelper<Action<IntPtr, TArg1>>.Invoke(address, arg1);
		}

		public static void Function<TArg1, TArg2>(IntPtr address, TArg1 arg1, TArg2 arg2)
		{
			NativeCallHelper<Action<IntPtr, TArg1, TArg2>>.Invoke(address, arg1, arg2);
		}

		public static void Function<TArg1, TArg2, TArg3>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			NativeCallHelper<Action<IntPtr, TArg1, TArg2, TArg3>>.Invoke(address, arg1, arg2, arg3);
		}

		public static void Function<TArg1, TArg2, TArg3, TArg4>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
		{
			NativeCallHelper<Action<IntPtr, TArg1, TArg2, TArg3, TArg4>>.Invoke(address, arg1, arg2, arg3, arg4);
		}

		public static void Function<TArg1, TArg2, TArg3, TArg4, TArg5>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
		{
			NativeCallHelper<Action<IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5>>.Invoke(address, arg1, arg2, arg3, arg4, arg5);
		}

		public static void Function<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
		{
			NativeCallHelper<Action<IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>>.Invoke(address, arg1, arg2, arg3, arg4, arg5, arg6);
		}

		public static void Method(IntPtr instance, int methodOffset)
		{
			NativeCallHelper<Action<IntPtr, IntPtr>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance);
		}

		public static void Method<TArg1>(IntPtr instance, int methodOffset, TArg1 arg1)
		{
			NativeCallHelper<Action<IntPtr, IntPtr, TArg1>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1);
		}

		public static void Method<TArg1, TArg2>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2)
		{
			NativeCallHelper<Action<IntPtr, IntPtr, TArg1, TArg2>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2);
		}

		public static void Method<TArg1, TArg2, TArg3>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			NativeCallHelper<Action<IntPtr, IntPtr, TArg1, TArg2, TArg3>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3);
		}

		public static void Method<TArg1, TArg2, TArg3, TArg4>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
		{
			NativeCallHelper<Action<IntPtr, IntPtr, TArg1, TArg2, TArg3, TArg4>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3, arg4);
		}

		public static void Method<TArg1, TArg2, TArg3, TArg4, TArg5>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
		{
			NativeCallHelper<Action<IntPtr, IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3, arg4, arg5);
		}

		public static void Method<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
		{
			NativeCallHelper<Action<IntPtr, IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3, arg4, arg5, arg6);
		}
	}
	public static class NativeCall<TResult>
	{
		public static TResult Function(IntPtr address)
		{
			return NativeCallHelper<Func<IntPtr, TResult>>.Invoke(address);
		}

		public static TResult Function<TArg1>(IntPtr address, TArg1 arg1)
		{
			return NativeCallHelper<Func<IntPtr, TArg1, TResult>>.Invoke(address, arg1);
		}

		public static TResult Function<TArg1, TArg2>(IntPtr address, TArg1 arg1, TArg2 arg2)
		{
			return NativeCallHelper<Func<IntPtr, TArg1, TArg2, TResult>>.Invoke(address, arg1, arg2);
		}

		public static TResult Function<TArg1, TArg2, TArg3>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			return NativeCallHelper<Func<IntPtr, TArg1, TArg2, TArg3, TResult>>.Invoke(address, arg1, arg2, arg3);
		}

		public static TResult Function<TArg1, TArg2, TArg3, TArg4>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
		{
			return NativeCallHelper<Func<IntPtr, TArg1, TArg2, TArg3, TArg4, TResult>>.Invoke(address, arg1, arg2, arg3, arg4);
		}

		public static TResult Function<TArg1, TArg2, TArg3, TArg4, TArg5>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
		{
			return NativeCallHelper<Func<IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>.Invoke(address, arg1, arg2, arg3, arg4, arg5);
		}

		public static TResult Function<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(IntPtr address, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
		{
			return NativeCallHelper<Func<IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>>.Invoke(address, arg1, arg2, arg3, arg4, arg5, arg6);
		}

		public static TResult Method(IntPtr instance, int methodOffset)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance);
		}

		public static TResult Method<TArg1>(IntPtr instance, int methodOffset, TArg1 arg1)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TArg1, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1);
		}

		public static TResult Method<TArg1, TArg2>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TArg1, TArg2, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2);
		}

		public static TResult Method<TArg1, TArg2, TArg3>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TArg1, TArg2, TArg3, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3);
		}

		public static TResult Method<TArg1, TArg2, TArg3, TArg4>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TArg1, TArg2, TArg3, TArg4, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3, arg4);
		}

		public static TResult Method<TArg1, TArg2, TArg3, TArg4, TArg5>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3, arg4, arg5);
		}

		public static TResult Method<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(IntPtr instance, int methodOffset, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
		{
			return NativeCallHelper<Func<IntPtr, IntPtr, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>>.Invoke(NativeMethod.CalculateAddress(instance, methodOffset), instance, arg1, arg2, arg3, arg4, arg5, arg6);
		}
	}
}
