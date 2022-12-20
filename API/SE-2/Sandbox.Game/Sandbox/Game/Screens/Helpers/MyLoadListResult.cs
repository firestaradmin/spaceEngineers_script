using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ParallelTasks;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	public abstract class MyLoadListResult : IMyAsyncResult
	{
		public List<Tuple<string, MyWorldInfo>> AvailableSaves = new List<Tuple<string, MyWorldInfo>>();

		public bool ContainsCorruptedWorlds;

		public readonly List<string> CustomPaths;

		public bool IsCompleted => Task.IsComplete;

		public Task Task { get; private set; }

		public MyLoadListResult(List<string> customPaths = null)
		{
			CustomPaths = customPaths;
			Task = Parallel.Start(delegate
			{
				LoadListAsync();
			});
		}

		private void LoadListAsync()
		{
			AvailableSaves = GetAvailableSaves();
			ContainsCorruptedWorlds = false;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Tuple<string, MyWorldInfo> availableSafe in AvailableSaves)
			{
				if (availableSafe.Item2 != null && availableSafe.Item2.IsCorrupted)
				{
					stringBuilder.Append(Path.GetFileNameWithoutExtension(availableSafe.Item1)).Append("\n");
					ContainsCorruptedWorlds = true;
				}
			}
			AvailableSaves.RemoveAll((Tuple<string, MyWorldInfo> x) => x == null || x.Item2 == null);
			if (ContainsCorruptedWorlds && MyLog.Default != null)
			{
				MyLog.Default.WriteLine("Corrupted worlds: ");
				MyLog.Default.WriteLine(stringBuilder.ToString());
			}
			if (AvailableSaves.Count != 0)
			{
				AvailableSaves.Sort((Tuple<string, MyWorldInfo> a, Tuple<string, MyWorldInfo> b) => b.Item2.LastSaveTime.CompareTo(a.Item2.LastSaveTime));
			}
		}

		protected abstract List<Tuple<string, MyWorldInfo>> GetAvailableSaves();

		[Conditional("DEBUG")]
		private void VerifyUniqueWorldID(List<Tuple<string, MyWorldInfo>> availableWorlds)
		{
			if (MyLog.Default == null)
			{
				return;
			}
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
			foreach (Tuple<string, MyWorldInfo> availableWorld in availableWorlds)
			{
				MyWorldInfo item = availableWorld.Item2;
				if (hashSet.Contains(availableWorld.Item1))
				{
					MyLog.Default.WriteLine(string.Format("Non-unique WorldID detected. WorldID = {0}; World Folder Path = '{2}', World Name = '{1}'", availableWorld.Item1, item.SessionName, availableWorld.Item1));
				}
				hashSet.Add(availableWorld.Item1);
=======
			HashSet<string> val = new HashSet<string>();
			foreach (Tuple<string, MyWorldInfo> availableWorld in availableWorlds)
			{
				MyWorldInfo item = availableWorld.Item2;
				if (val.Contains(availableWorld.Item1))
				{
					MyLog.Default.WriteLine(string.Format("Non-unique WorldID detected. WorldID = {0}; World Folder Path = '{2}', World Name = '{1}'", availableWorld.Item1, item.SessionName, availableWorld.Item1));
				}
				val.Add(availableWorld.Item1);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
