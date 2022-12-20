using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NativeAftermath
{
	public class MyAfterMath
	{
		private static ConcurrentDictionary<string, IntPtr> m_markerTable = new ConcurrentDictionary<string, IntPtr>();

		private static ConcurrentDictionary<IntPtr, IntPtr> m_markerTableContext = new ConcurrentDictionary<IntPtr, IntPtr>();

		private static bool m_initialized = false;

		public unsafe static Result Init(IntPtr device)
		{
			GFSDK_Aftermath_Result gFSDK_Aftermath_Result = _003CModule_003E.GFSDK_Aftermath_DX11_Initialize((GFSDK_Aftermath_Version)19, (GFSDK_Aftermath_FeatureFlags)3, (ID3D11Device*)device.ToPointer());
			m_initialized = gFSDK_Aftermath_Result == (GFSDK_Aftermath_Result)1;
			return (Result)gFSDK_Aftermath_Result;
		}

		public unsafe static void Shutdown()
		{
			foreach (IntPtr value in m_markerTableContext.Values)
			{
				_003CModule_003E.GFSDK_Aftermath_ReleaseContextHandle((GFSDK_Aftermath_ContextHandle__*)value.ToPointer());
			}
		}

		public unsafe static Result SetEventMarker(IntPtr context, string marker)
		{
			if (m_initialized)
			{
				IntPtr value = default(IntPtr);
				GFSDK_Aftermath_ContextHandle__* ptr;
				if (!m_markerTable.TryGetValue(marker, out value))
				{
					IntPtr intPtr = Marshal.StringToHGlobalAnsi(marker);
					value = intPtr;
					m_markerTable.TryAdd(marker, intPtr);
					_003CModule_003E.GFSDK_Aftermath_DX11_CreateContextHandle((ID3D11DeviceContext*)context.ToPointer(), &ptr);
					IntPtr value2 = (IntPtr)ptr;
					m_markerTableContext.TryAdd(context, value2);
				}
				else
				{
					IntPtr value3 = default(IntPtr);
					m_markerTableContext.TryGetValue(context, out value3);
					ptr = (GFSDK_Aftermath_ContextHandle__*)value3.ToPointer();
				}
				if (marker != Marshal.PtrToStringAnsi(value))
				{
					return Result.FAIL_NotInitialized;
				}
				return (Result)_003CModule_003E.GFSDK_Aftermath_SetEventMarker(ptr, value.ToPointer(), 0u);
			}
			return Result.FAIL_NotInitialized;
		}

		public unsafe static ContextData GetInfo(IntPtr context)
		{
			//IL_001a: Expected I4, but got I8
			//IL_0079: Expected I, but got I8
			if (m_initialized)
			{
				GFSDK_Aftermath_ContextData* ptr = (GFSDK_Aftermath_ContextData*)_003CModule_003E.new_005B_005D(16uL);
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ptr, 0, 16);
				ContextData result = default(ContextData);
				IntPtr value = default(IntPtr);
				if (m_markerTableContext.TryGetValue(context, out value))
				{
					GFSDK_Aftermath_ContextHandle__* ptr2 = (GFSDK_Aftermath_ContextHandle__*)value.ToPointer();
					result.Result = (Result)_003CModule_003E.GFSDK_Aftermath_GetData(1u, &ptr2, ptr);
					GFSDK_Aftermath_Device_Status gFSDK_Aftermath_Device_Status;
					result.Status = (Status)_003CModule_003E.GFSDK_Aftermath_GetDeviceStatus(&gFSDK_Aftermath_Device_Status);
					result.ContextStatus = *(ContextStatus*)((long)(IntPtr)ptr + 12);
					ulong num = *(ulong*)ptr;
					if (num != 0L)
					{
						IntPtr ptr3 = (IntPtr)(void*)num;
						result.MarkerData = Marshal.PtrToStringAnsi(ptr3);
					}
					else
					{
						result.MarkerData = "";
					}
					_003CModule_003E.delete_005B_005D(ptr);
				}
				else
				{
					result.Result = Result.FAIL_Unknown;
					result.Status = Status.Unknown;
					result.ContextStatus = ContextStatus.Invalid;
					result.MarkerData = "Get Info fail";
				}
				return result;
			}
			ContextData result2 = default(ContextData);
			result2.Result = Result.FAIL_NotInitialized;
			result2.Status = Status.Unknown;
			result2.ContextStatus = ContextStatus.NotStarted;
			result2.MarkerData = "";
			return result2;
		}
	}
}
