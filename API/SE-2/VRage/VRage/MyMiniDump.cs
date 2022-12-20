using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.FileSystem;

namespace VRage
{
	public static class MyMiniDump
	{
		[Flags]
		public enum Options : uint
		{
			Normal = 0x0u,
			WithDataSegs = 0x1u,
			WithFullMemory = 0x2u,
			WithHandleData = 0x4u,
			FilterMemory = 0x8u,
			ScanMemory = 0x10u,
			WithUnloadedModules = 0x20u,
			WithIndirectlyReferencedMemory = 0x40u,
			FilterModulePaths = 0x80u,
			WithProcessThreadData = 0x100u,
			WithPrivateReadWriteMemory = 0x200u,
			WithoutOptionalData = 0x400u,
			WithFullMemoryInfo = 0x800u,
			WithThreadInfo = 0x1000u,
			WithCodeSegs = 0x2000u,
			WithoutAuxiliaryState = 0x4000u,
			WithFullAuxiliaryState = 0x8000u,
			WithPrivateWriteCopyMemory = 0x10000u,
			IgnoreInaccessibleMemory = 0x20000u
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct ExceptionInformation
		{
			public uint ThreadId;

			public IntPtr ExceptionPointers;

			[MarshalAs(UnmanagedType.Bool)]
			public bool ClientPointers;
		}

		[ThreadStatic]
		private static long m_lastDumpTimestamp;

		public static void CollectExceptionDump(Exception ex)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = new DateTime(m_lastDumpTimestamp);
			if ((utcNow - dateTime).TotalSeconds > 15.0)
			{
				string dumpPath = Path.Combine(MyFileSystem.UserDataPath, "MinidumpT" + Thread.get_CurrentThread().get_ManagedThreadId() + ".dmp");
				Options dumpFlags = Options.WithProcessThreadData | Options.WithThreadInfo;
				MyVRage.Platform.CrashReporting.WriteMiniDump(dumpPath, dumpFlags, IntPtr.Zero);
				m_lastDumpTimestamp = utcNow.Ticks;
			}
		}

		public static void CollectCrashDump(IntPtr exceptionPointers)
		{
			string dumpPath = Path.Combine(MyFileSystem.UserDataPath, "Minidump.dmp");
			Options dumpFlags = Options.WithProcessThreadData | Options.WithThreadInfo;
			MyVRage.Platform.CrashReporting.WriteMiniDump(dumpPath, dumpFlags, exceptionPointers);
		}

		public static void CollectStateDump()
		{
			string dumpPath = Path.Combine(MyFileSystem.UserDataPath, $"Minidump_State_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_fff}.dmp");
			Options dumpFlags = Options.WithProcessThreadData | Options.WithThreadInfo;
			MyVRage.Platform.CrashReporting.WriteMiniDump(dumpPath, dumpFlags, IntPtr.Zero);
		}

		public static IEnumerable<string> FindActiveDumps(string directory)
		{
			DateTime now = DateTime.Now;
			string[] files = Directory.GetFiles(directory, "Minidump*.dmp", (SearchOption)0);
			foreach (string text in files)
			{
				if (text != null && File.Exists(text) && (File.GetCreationTime(text) - now).Minutes < 5)
				{
					yield return text;
				}
			}
		}

		public static void CleanupOldDumps()
		{
<<<<<<< HEAD
			HashSet<FileInfo> hashSet = new HashSet<FileInfo>();
			foreach (FileInfo item in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles("Minidump*.dmp", SearchOption.TopDirectoryOnly))
			{
				if (item.Name.StartsWith("Minidump_State_"))
				{
					hashSet.Add(item);
				}
				else
				{
					item.Delete();
				}
			}
			if (hashSet.Count <= 1)
			{
				return;
			}
			hashSet.Remove(hashSet.MaxBy((FileInfo x) => x.LastWriteTime));
			foreach (FileInfo item2 in hashSet)
			{
				item2.Delete();
=======
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			HashSet<FileInfo> val = new HashSet<FileInfo>();
			foreach (FileInfo item in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles("Minidump*.dmp", (SearchOption)0))
			{
				if (((FileSystemInfo)item).get_Name().StartsWith("Minidump_State_"))
				{
					val.Add(item);
				}
				else
				{
					((FileSystemInfo)item).Delete();
				}
			}
			if (val.get_Count() <= 1)
			{
				return;
			}
			val.Remove(((IEnumerable<FileInfo>)val).MaxBy((FileInfo x) => ((FileSystemInfo)x).get_LastWriteTime()));
			Enumerator<FileInfo> enumerator2 = val.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					((FileSystemInfo)enumerator2.get_Current()).Delete();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
