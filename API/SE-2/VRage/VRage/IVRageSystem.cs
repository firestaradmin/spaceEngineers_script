using System;
using System.Collections.Generic;

namespace VRage
{
	public interface IVRageSystem
	{
		float CPUCounter { get; }

		float RAMCounter { get; }

		long RemainingMemoryForGame { get; }

		long ProcessPrivateMemory { get; }

		string Clipboard { get; set; }

		bool IsAllocationProfilingReady { get; }

		bool IsSingleInstance { get; }

		bool IsRemoteDebuggingSupported { get; }

		SimulationQuality SimulationQuality { get; }

		bool IsDeprecatedOS { get; }

		bool IsMemoryLimited { get; }

		string ThreeLetterISORegionName { get; }

		string TwoLetterISORegionName { get; }

		string RegionLatitude { get; }

		string RegionLongitude { get; }

		string TempPath { get; }

		event Action<string> OnSystemProtocolActivated;

		event Action OnResuming;

		string GetOsName();

		string GetInfoCPU(out uint frequency, out uint physicalCores);

		ulong GetTotalPhysicalMemory();

		void LogEnvironmentInformation();

		void GetGCMemory(out float allocated, out float used);

		List<string> GetProcessesLockingFile(string path);

		void ResetColdStartRegister();

		ulong GetThreadAllocationStamp();

		ulong GetGlobalAllocationsStamp();

		string GetAppDataPath();

		void WriteLineToConsole(string msg);

		void LogToExternalDebugger(string message);

		bool OpenUrl(string url);

		void OnThreadpoolInitialized();

		void LogRuntimeInfo(Action<string> log);

		void OnSessionStarted(SessionType sessionType);

		void OnSessionUnloaded();

		int? GetExperimentalPCULimit(int safePCULimit);

		/// <summary>
		/// Blocking call to get the current UTC time via NTP or equivalent.
		/// </summary>
		/// <remarks>This call may throw if Network time is not available.</remarks>
		/// <returns>A relatively accurate value for the current UTC time.</returns>
		DateTime GetNetworkTimeUTC();
	}
}
