using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;

namespace VRage.Platform.Windows.Win32
{
	internal class Processes
	{
		internal class Win32API
		{
			public enum ObjectInformationClass
			{
				ObjectBasicInformation,
				ObjectNameInformation,
				ObjectTypeInformation,
				ObjectAllTypesInformation,
				ObjectHandleInformation
			}

			[Flags]
			public enum ProcessAccessFlags : uint
			{
				All = 0x1F0FFFu,
				Terminate = 0x1u,
				CreateThread = 0x2u,
				VMOperation = 0x8u,
				VMRead = 0x10u,
				VMWrite = 0x20u,
				DupHandle = 0x40u,
				SetInformation = 0x200u,
				QueryInformation = 0x400u,
				Synchronize = 0x100000u
			}

			public struct OBJECT_BASIC_INFORMATION
			{
				public int Attributes;

				public int GrantedAccess;

				public int HandleCount;

				public int PointerCount;

				public int PagedPoolUsage;

				public int NonPagedPoolUsage;

				public int Reserved1;

				public int Reserved2;

				public int Reserved3;

				public int NameInformationLength;

				public int TypeInformationLength;

				public int SecurityDescriptorLength;

				public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
			}

			public struct OBJECT_TYPE_INFORMATION
			{
				public UNICODE_STRING Name;

				public int ObjectCount;

				public int HandleCount;

				public int Reserved1;

				public int Reserved2;

				public int Reserved3;

				public int Reserved4;

				public int PeakObjectCount;

				public int PeakHandleCount;

				public int Reserved5;

				public int Reserved6;

				public int Reserved7;

				public int Reserved8;

				public int InvalidAttributes;

				public GENERIC_MAPPING GenericMapping;

				public int ValidAccess;

				public byte Unknown;

				public byte MaintainHandleDatabase;

				public int PoolType;

				public int PagedPoolUsage;

				public int NonPagedPoolUsage;
			}

			public struct OBJECT_NAME_INFORMATION
			{
				public UNICODE_STRING Name;
			}

			[StructLayout(LayoutKind.Sequential, Pack = 1)]
			public struct UNICODE_STRING
			{
				public ushort Length;

				public ushort MaximumLength;

				public IntPtr Buffer;
			}

			public struct GENERIC_MAPPING
			{
				public int GenericRead;

				public int GenericWrite;

				public int GenericExecute;

				public int GenericAll;
			}

			[StructLayout(LayoutKind.Sequential, Pack = 1)]
			public struct SYSTEM_HANDLE_INFORMATION
			{
				public int ProcessID;

				public byte ObjectTypeNumber;

				public byte Flags;

				public ushort Handle;

				public int Object_Pointer;

				public uint GrantedAccess;
			}

			public const int MAX_PATH = 260;

			public const uint STATUS_INFO_LENGTH_MISMATCH = 3221225476u;

			public const int DUPLICATE_SAME_ACCESS = 2;

			public const uint FILE_SEQUENTIAL_ONLY = 4u;

			[DllImport("ntdll.dll")]
			public static extern int NtQueryObject(IntPtr ObjectHandle, int ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength, ref int returnLength);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

			[DllImport("ntdll.dll")]
			public static extern uint NtQuerySystemInformation(int SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, ref int returnLength);

			[DllImport("kernel32.dll")]
			public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

			[DllImport("kernel32.dll")]
			public static extern int CloseHandle(IntPtr hObject);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, ushort hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

			[DllImport("kernel32.dll")]
			public static extern IntPtr GetCurrentProcess();
		}

		private const int CNST_SYSTEM_HANDLE_INFORMATION = 16;

		public static List<string> GetProcessesLockingFile(string filePath)
		{
			List<string> list = new List<string>();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.Id > 4 && GetFilesLockedBy(process).Contains(filePath))
				{
					list.Add(process.ProcessName);
				}
			}
			return list;
		}

		public static List<string> GetFilesLockedBy(Process process)
		{
			List<string> outp = new List<string>();
			ThreadStart start = delegate
			{
				try
				{
					outp = UnsafeGetFilesLockedBy(process);
				}
				catch
				{
					Ignore();
				}
			};
			try
			{
				Thread thread = new Thread(start);
				thread.IsBackground = true;
				thread.Start();
				if (!thread.Join(250))
				{
					try
					{
						thread.Interrupt();
						thread.Abort();
					}
					catch
					{
						Ignore();
					}
				}
			}
			catch
			{
				Ignore();
			}
			return outp;
		}

		private static void Ignore()
		{
		}

		private static List<string> UnsafeGetFilesLockedBy(Process process)
		{
			try
			{
				IEnumerable<Win32API.SYSTEM_HANDLE_INFORMATION> handles = GetHandles(process);
				List<string> list = new List<string>();
				foreach (Win32API.SYSTEM_HANDLE_INFORMATION item in handles)
				{
					string filePath = GetFilePath(item, process);
					if (filePath != null)
					{
						list.Add(filePath);
					}
				}
				return list;
			}
			catch
			{
				return new List<string>();
			}
		}

		private static string GetFilePath(Win32API.SYSTEM_HANDLE_INFORMATION systemHandleInformation, Process process)
		{
			IntPtr hSourceProcessHandle = Win32API.OpenProcess(Win32API.ProcessAccessFlags.All, bInheritHandle: false, process.Id);
			Win32API.OBJECT_BASIC_INFORMATION oBJECT_BASIC_INFORMATION = default(Win32API.OBJECT_BASIC_INFORMATION);
			Win32API.OBJECT_TYPE_INFORMATION oBJECT_TYPE_INFORMATION = default(Win32API.OBJECT_TYPE_INFORMATION);
			Win32API.OBJECT_NAME_INFORMATION oBJECT_NAME_INFORMATION = default(Win32API.OBJECT_NAME_INFORMATION);
			string strRawName = "";
			int returnLength = 0;
			if (!Win32API.DuplicateHandle(hSourceProcessHandle, systemHandleInformation.Handle, Win32API.GetCurrentProcess(), out var lpTargetHandle, 0u, bInheritHandle: false, 2u))
			{
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(oBJECT_BASIC_INFORMATION));
			Win32API.NtQueryObject(lpTargetHandle, 0, intPtr, Marshal.SizeOf(oBJECT_BASIC_INFORMATION), ref returnLength);
			oBJECT_BASIC_INFORMATION = (Win32API.OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(intPtr, oBJECT_BASIC_INFORMATION.GetType());
			Marshal.FreeHGlobal(intPtr);
			IntPtr intPtr2 = Marshal.AllocHGlobal(oBJECT_BASIC_INFORMATION.TypeInformationLength);
			returnLength = oBJECT_BASIC_INFORMATION.TypeInformationLength;
			while (Win32API.NtQueryObject(lpTargetHandle, 2, intPtr2, returnLength, ref returnLength) == -1073741820)
			{
				if (returnLength == 0)
				{
					Console.WriteLine("nLength returned at zero! ");
					return null;
				}
				Marshal.FreeHGlobal(intPtr2);
				intPtr2 = Marshal.AllocHGlobal(returnLength);
			}
			oBJECT_TYPE_INFORMATION = (Win32API.OBJECT_TYPE_INFORMATION)Marshal.PtrToStructure(intPtr2, oBJECT_TYPE_INFORMATION.GetType());
			IntPtr ptr = ((!Is64Bits()) ? oBJECT_TYPE_INFORMATION.Name.Buffer : new IntPtr(Convert.ToInt64(oBJECT_TYPE_INFORMATION.Name.Buffer.ToString(), 10) >> 32));
			string text = Marshal.PtrToStringUni(ptr, oBJECT_TYPE_INFORMATION.Name.Length >> 1);
			Marshal.FreeHGlobal(intPtr2);
			if (text != "File")
			{
				return null;
			}
			returnLength = oBJECT_BASIC_INFORMATION.NameInformationLength;
			IntPtr intPtr3 = Marshal.AllocHGlobal(returnLength);
			while (Win32API.NtQueryObject(lpTargetHandle, 1, intPtr3, returnLength, ref returnLength) == -1073741820)
			{
				Marshal.FreeHGlobal(intPtr3);
				if (returnLength == 0)
				{
					Console.WriteLine("nLength returned at zero! " + text);
					return null;
				}
				intPtr3 = Marshal.AllocHGlobal(returnLength);
			}
			oBJECT_NAME_INFORMATION = (Win32API.OBJECT_NAME_INFORMATION)Marshal.PtrToStructure(intPtr3, oBJECT_NAME_INFORMATION.GetType());
			ptr = ((!Is64Bits()) ? oBJECT_NAME_INFORMATION.Name.Buffer : new IntPtr(Convert.ToInt64(oBJECT_NAME_INFORMATION.Name.Buffer.ToString(), 10) >> 32));
			if (ptr != IntPtr.Zero)
			{
				byte[] destination = new byte[returnLength];
				try
				{
					Marshal.Copy(ptr, destination, 0, returnLength);
					strRawName = Marshal.PtrToStringUni(Is64Bits() ? new IntPtr(ptr.ToInt64()) : new IntPtr(ptr.ToInt32()));
				}
				catch (AccessViolationException)
				{
					return null;
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr3);
					Win32API.CloseHandle(lpTargetHandle);
				}
			}
			string regularFileNameFromDevice = GetRegularFileNameFromDevice(strRawName);
			try
			{
				return regularFileNameFromDevice;
			}
			catch
			{
				return null;
			}
		}

		private static string GetRegularFileNameFromDevice(string strRawName)
		{
			string text = strRawName;
			string[] logicalDrives = Environment.GetLogicalDrives();
			foreach (string text2 in logicalDrives)
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				if (Win32API.QueryDosDevice(text2.Substring(0, 2), stringBuilder, 260) == 0)
				{
					return strRawName;
				}
				string text3 = stringBuilder.ToString();
				if (text.StartsWith(text3))
				{
					text = text.Replace(text3, text2.Substring(0, 2));
					break;
				}
			}
			return text;
		}

		private static IEnumerable<Win32API.SYSTEM_HANDLE_INFORMATION> GetHandles(Process process)
		{
			int num = 65536;
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			int returnLength = 0;
			while (Win32API.NtQuerySystemInformation(16, intPtr, num, ref returnLength) == 3221225476u)
			{
				num = returnLength;
				Marshal.FreeHGlobal(intPtr);
				intPtr = Marshal.AllocHGlobal(returnLength);
			}
			byte[] destination = new byte[returnLength];
			Marshal.Copy(intPtr, destination, 0, returnLength);
			long num2;
			IntPtr ptr;
			if (Is64Bits())
			{
				num2 = Marshal.ReadInt64(intPtr);
				ptr = new IntPtr(intPtr.ToInt64() + 8);
			}
			else
			{
				num2 = Marshal.ReadInt32(intPtr);
				ptr = new IntPtr(intPtr.ToInt32() + 4);
			}
			List<Win32API.SYSTEM_HANDLE_INFORMATION> list = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();
			for (long num3 = 0L; num3 < num2; num3++)
			{
				Win32API.SYSTEM_HANDLE_INFORMATION sYSTEM_HANDLE_INFORMATION = default(Win32API.SYSTEM_HANDLE_INFORMATION);
				if (Is64Bits())
				{
					sYSTEM_HANDLE_INFORMATION = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ptr, sYSTEM_HANDLE_INFORMATION.GetType());
					ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(sYSTEM_HANDLE_INFORMATION) + 8);
				}
				else
				{
					ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(sYSTEM_HANDLE_INFORMATION));
					sYSTEM_HANDLE_INFORMATION = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ptr, sYSTEM_HANDLE_INFORMATION.GetType());
				}
				if (sYSTEM_HANDLE_INFORMATION.ProcessID == process.Id)
				{
					list.Add(sYSTEM_HANDLE_INFORMATION);
				}
			}
			return list;
		}

		private static bool Is64Bits()
		{
			return Marshal.SizeOf(typeof(IntPtr)) == 8;
		}
	}
}
