using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using VRage.FileSystem;
using VRage.Platform.Windows.Forms;
using VRage.Platform.Windows.Win32;
using VRage.Utils;

namespace VRage.Platform.Windows.Sys
{
	internal sealed class MyWindowsSystem : IVRageSystem
	{
		private struct PROCESS_MEMORY_COUNTERS
		{
			public uint cb;

			public uint PageFaultCount;

			public ulong PeakWorkingSetSize;

			public ulong WorkingSetSize;

			public ulong QuotaPeakPagedPoolUsage;

			public ulong QuotaPagedPoolUsage;

			public ulong QuotaPeakNonPagedPoolUsage;

			public ulong QuotaNonPagedPoolUsage;

			public ulong PagefileUsage;

			public ulong PeakPagefileUsage;
		}

		private readonly MyLog m_log;

		private readonly string m_appDataPath;

		private Process m_process;

		private PerformanceCounter m_cpuCounter;

		private PerformanceCounter m_ramCounter;

		private (string Name, uint MaxClock, uint Cores) m_cpuInfo;

		private const string IE_PROCESS = "IExplore.exe";

		public SimulationQuality SimulationQuality => SimulationQuality.Normal;

		public bool IsDeprecatedOS
		{
			get
			{
				OperatingSystem oSVersion = Environment.OSVersion;
				Version version = oSVersion.Version;
				if (oSVersion.Platform.ToString().Contains("Win") && version.Major <= 6)
				{
					return version.Minor <= 1;
				}
				return false;
			}
		}

		public bool IsMemoryLimited => false;

		public float CPUCounter => (m_cpuCounter?.NextValue() ?? 0f) / 100f;

		public float RAMCounter => m_ramCounter?.NextValue() ?? 0f;

		public long RemainingMemoryForGame => long.MaxValue;

		public unsafe long ProcessPrivateMemory
		{
			get
			{
				PROCESS_MEMORY_COUNTERS counters = default(PROCESS_MEMORY_COUNTERS);
				counters.cb = (uint)sizeof(PROCESS_MEMORY_COUNTERS);
				if (GetProcessMemoryInfo(m_process.Handle, out counters, counters.cb))
				{
					return (long)counters.PagefileUsage;
				}
				return 0L;
			}
		}

		public string Clipboard
		{
			get
			{
				return VRage.Platform.Windows.Forms.MyClipboardHelper.GetClipboardText();
			}
			set
			{
				VRage.Platform.Windows.Forms.MyClipboardHelper.SetClipboard(value);
			}
		}

		public bool IsAllocationProfilingReady => MyManagedAllocationReader.IsReady;

		public bool IsSingleInstance => new MySingleProgramInstance(MyFileSystem.MainAssemblyName).IsSingleInstance;

		public bool IsRemoteDebuggingSupported => true;

		public string ThreeLetterISORegionName => WinApi.GetGeoInfo(WinApi.GeoTypeEnum.ISO3);

		public string TwoLetterISORegionName => WinApi.GetGeoInfo(WinApi.GeoTypeEnum.ISO2);

		public string RegionLatitude => WinApi.GetGeoInfo(WinApi.GeoTypeEnum.Latitude);

		public string RegionLongitude => WinApi.GetGeoInfo(WinApi.GeoTypeEnum.Longitude);

		public string TempPath => MyFileSystem.TempPath;

		public event Action<string> OnSystemProtocolActivated
		{
			add
			{
				throw new NotSupportedException("Not supported on PC");
			}
			remove
			{
			}
		}

		public event Action OnResuming;

		public MyWindowsSystem(string applicationName, string appDataPath, MyLog log)
		{
			m_appDataPath = appDataPath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), applicationName);
			m_log = log;
			m_process = Process.GetCurrentProcess();
		}

		public void Init()
		{
			try
			{
				m_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
				m_ramCounter = new PerformanceCounter("Memory", "Available MBytes");
			}
			catch
			{
				m_log.WriteLine("Error initializing PerformanceCounters. CPU and Memory statistics logging will be suspended.\nTry running \"lodctr /r\" in admin command line to fix it.");
			}
		}

		public string GetOsName()
		{
			string text = "";
			try
			{
				text = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>().FirstOrDefault()?.GetPropertyValue("Caption").ToString().Trim();
			}
			catch (Exception ex)
			{
				m_log.WriteLine("Couldn't get friendly OS name" + ex);
			}
			return string.Concat(text, " (", Environment.OSVersion, ")");
		}

		public string GetInfoCPU(out uint frequency, out uint physicalCores)
		{
			if (m_cpuInfo.Name == null)
			{
				try
				{
					using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select Name, MaxClockSpeed, NumberOfCores from Win32_Processor"))
					{
						foreach (ManagementObject item in managementObjectSearcher.Get())
						{
							m_cpuInfo.Name = item["Name"].ToString();
							m_cpuInfo.Cores = (uint)item["NumberOfCores"];
							m_cpuInfo.MaxClock = (uint)item["MaxClockSpeed"];
						}
					}
				}
				catch (Exception ex)
				{
					m_log.WriteLine("Couldn't get cpu info: " + ex);
					m_cpuInfo.Name = "UnknownCPU";
					m_cpuInfo.Cores = 0u;
					m_cpuInfo.MaxClock = 0u;
				}
			}
			frequency = m_cpuInfo.MaxClock;
			physicalCores = m_cpuInfo.Cores;
			return m_cpuInfo.Name;
		}

		public ulong GetTotalPhysicalMemory()
		{
			WinApi.MEMORYSTATUSEX mEMORYSTATUSEX = new WinApi.MEMORYSTATUSEX();
			WinApi.GlobalMemoryStatusEx(mEMORYSTATUSEX);
			return mEMORYSTATUSEX.ullTotalPhys;
		}

		private static bool IsVirtualized(string manufacturer, string model)
		{
			manufacturer = manufacturer.ToLower();
			if (!(manufacturer == "microsoft corporation") && !manufacturer.Contains("vmware") && !(model == "VirtualBox"))
			{
				return model.ToLower().Contains("virtual");
			}
			return true;
		}

		public void LogEnvironmentInformation()
		{
			m_log.WriteLine("MyVideoModeManager.LogEnvironmentInformation - START");
			m_log.IncreaseIndent();
			try
			{
				foreach (ManagementBaseObject item in new ManagementObjectSearcher("Select Manufacturer, Model from Win32_ComputerSystem").Get())
				{
					m_log.WriteLine("Win32_ComputerSystem.Manufacturer: " + item["Manufacturer"]);
					m_log.WriteLine("Win32_ComputerSystem.Model: " + item["Model"]);
					m_log.WriteLine("Virtualized: " + IsVirtualized(item["Manufacturer"].ToString(), item["Model"].ToString()));
				}
				foreach (ManagementObject item2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT Name FROM Win32_Processor").Get())
				{
					m_log.WriteLine("Environment.ProcessorName: " + item2["Name"]);
				}
				WinApi.MEMORYSTATUSEX mEMORYSTATUSEX = new WinApi.MEMORYSTATUSEX();
				WinApi.GlobalMemoryStatusEx(mEMORYSTATUSEX);
				m_log.WriteLine("ComputerInfo.TotalPhysicalMemory: " + MyValueFormatter.GetFormatedLong((long)mEMORYSTATUSEX.ullTotalPhys) + " bytes");
				m_log.WriteLine("ComputerInfo.TotalVirtualMemory: " + MyValueFormatter.GetFormatedLong((long)mEMORYSTATUSEX.ullTotalVirtual) + " bytes");
				m_log.WriteLine("ComputerInfo.AvailablePhysicalMemory: " + MyValueFormatter.GetFormatedLong((long)mEMORYSTATUSEX.ullAvailPhys) + " bytes");
				m_log.WriteLine("ComputerInfo.AvailableVirtualMemory: " + MyValueFormatter.GetFormatedLong((long)mEMORYSTATUSEX.ullAvailVirtual) + " bytes");
				ConnectionOptions options = new ConnectionOptions();
				ManagementScope scope = new ManagementScope("\\\\localhost", options);
				ObjectQuery query = new ObjectQuery("select FreeSpace,Size,Name from Win32_LogicalDisk where DriveType=3");
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query))
				{
					ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
					foreach (ManagementObject item3 in managementObjectCollection)
					{
						string formatedLong = MyValueFormatter.GetFormatedLong(Convert.ToInt64(item3["Size"]));
						string formatedLong2 = MyValueFormatter.GetFormatedLong(Convert.ToInt64(item3["FreeSpace"]));
						string text = item3["Name"].ToString();
						m_log.WriteLine("Drive " + text + " | Capacity: " + formatedLong + " bytes | Free space: " + formatedLong2 + " bytes");
					}
					managementObjectCollection.Dispose();
				}
			}
			catch (Exception ex)
			{
				m_log.WriteLine("Error occured during enumerating environment information. Application is continuing. Exception: " + ex.ToString());
			}
			m_log.DecreaseIndent();
			m_log.WriteLine("MyVideoModeManager.LogEnvironmentInformation - END");
		}

		public void GetGCMemory(out float allocated, out float used)
		{
			used = (float)GC.GetTotalMemory(forceFullCollection: false) / 1024f / 1024f;
			allocated = used;
		}

		public List<string> GetProcessesLockingFile(string path)
		{
			return Processes.GetProcessesLockingFile(path);
		}

		public void ResetColdStartRegister()
		{
			string path = Path.Combine(MyFileSystem.UserDataPath, "ColdStart.txt");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public ulong GetThreadAllocationStamp()
		{
			return MyManagedAllocationReader.GetThreadAllocationStamp();
		}

		public ulong GetGlobalAllocationsStamp()
		{
			return MyManagedAllocationReader.GetGlobalAllocationsStamp();
		}

		public string GetAppDataPath()
		{
			return m_appDataPath;
		}

		public void WriteLineToConsole(string msg)
		{
			Console.WriteLine(msg);
		}

		public void LogToExternalDebugger(string message)
		{
		}

		public void OnThreadpoolInitialized()
		{
		}

		public void LogRuntimeInfo(Action<string> log)
		{
		}

		public void OnSessionStarted(SessionType sessionType)
		{
		}

		public void OnSessionUnloaded()
		{
		}

		public int? GetExperimentalPCULimit(int safePCULimit)
		{
			return null;
		}

		public bool OpenUrl(string url)
		{
			try
			{
				try
				{
					Process.Start(url);
				}
				catch (Win32Exception)
				{
					Process.Start(new ProcessStartInfo("IExplore.exe", url));
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		[DllImport("psapi.dll", SetLastError = true)]
		private static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, uint size);

		public DateTime GetNetworkTimeUTC()
		{
			byte[] array = new byte[48];
			array[0] = 27;
			IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry("time.windows.com").AddressList[0], 123);
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				socket.Connect(remoteEP);
				socket.ReceiveTimeout = 3000;
				socket.Send(array);
				socket.Receive(array);
				socket.Close();
			}
			long x = BitConverter.ToUInt32(array, 40);
			ulong x2 = BitConverter.ToUInt32(array, 44);
			long num = SwapEndianness((ulong)x);
			x2 = SwapEndianness(x2);
			ulong num2 = (ulong)(num * 1000) + x2 * 1000 / 4294967296uL;
			return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)num2);
		}

		private static uint SwapEndianness(ulong x)
		{
			return (uint)(((x & 0xFF) << 24) + ((x & 0xFF00) << 8) + ((x & 0xFF0000) >> 8) + ((x & 0xFF000000u) >> 24));
		}
	}
}
