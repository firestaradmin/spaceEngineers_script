using System;
using System.Collections.Generic;
using System.IO;
using ParallelTasks;
using SharpDX;
using SharpDX.Multimedia;
using VRage.FileSystem;
using VRage.Library.Memory;
using VRage.Library.Threading;

namespace VRage.Audio
{
	public sealed class MyInMemoryWaveDataCache
	{
		public struct CacheData : IDisposable
		{
			public readonly DataStream DataStream;

			public readonly SoundStream SoundStream;

			public MyMemorySystem.AllocationRecord? AllocationRecord;

			public CacheData(SoundStream soundStream, string debugName)
			{
				SoundStream = soundStream;
				DataStream = soundStream.ToDataStream();
				AllocationRecord = Static.MemorySystem.RegisterAllocation(debugName, soundStream.Length);
			}

			public void Dispose()
			{
				DataStream?.Dispose();
				AllocationRecord?.Dispose();
			}
		}

		public static MyInMemoryWaveDataCache Static;

		private MyMemorySystem MemorySystem = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Audio");

		private SpinLockRef Lock = new SpinLockRef();

		private Dictionary<string, CacheData> Cache = new Dictionary<string, CacheData>(StringComparer.InvariantCultureIgnoreCase);

		static MyInMemoryWaveDataCache()
		{
			Static = new MyInMemoryWaveDataCache();
		}

		public CacheData Get(string path, out bool owns)
		{
			MakePath(ref path);
			using (Lock.Acquire())
			{
				if (Cache.TryGetValue(path, out var value))
				{
					owns = false;
					return value;
				}
			}
			owns = true;
			return Load(path);
		}

		public void Preload(string path)
		{
			MakePath(ref path);
			using (Lock.Acquire())
			{
				if (Cache.ContainsKey(path))
				{
					return;
				}
				Cache.Add(path, default(CacheData));
			}
			CacheData value = Load(path);
			using (Lock.Acquire())
			{
				Cache[path] = value;
			}
		}

		private CacheData Load(string path)
		{
			using (Stream stream = MyFileSystem.OpenRead(path))
			{
				CacheData result = default(CacheData);
				if (stream != null)
				{
					SoundStream soundStream = new SoundStream(stream);
					result = new CacheData(soundStream, path);
					soundStream.Close();
				}
				return result;
			}
		}

		public CacheData LoadCached(string path)
		{
			MakePath(ref path);
			using (Lock.Acquire())
			{
				if (Cache.TryGetValue(path, out var value))
				{
					return value;
				}
				value = Load(path);
				Cache.Add(path, value);
				return value;
			}
		}

		private void MakePath(ref string path)
		{
			if (!Path.IsPathRooted(path))
			{
				path = Path.Combine(MyFileSystem.ContentPath, "Audio", path);
			}
		}

		public void Dispose()
		{
			using (Lock.Acquire())
			{
				foreach (KeyValuePair<string, CacheData> item in Cache)
				{
					item.Value.Dispose();
				}
				Cache.Clear();
			}
		}
	}
}
