using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;
using VRageRender;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ProfilerSnapshot : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003EProfilers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, List<MyObjectBuilder_Profiler>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in List<MyObjectBuilder_Profiler> value)
			{
				owner.Profilers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out List<MyObjectBuilder_Profiler> value)
			{
				value = owner.Profilers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003ESimulationFrames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, List<MyRenderProfiler.FrameInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in List<MyRenderProfiler.FrameInfo> value)
			{
				owner.SimulationFrames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out List<MyRenderProfiler.FrameInfo> value)
			{
				value = owner.SimulationFrames;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ProfilerSnapshot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProfilerSnapshot>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProfilerSnapshot();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProfilerSnapshot CreateInstance()
			{
				return new MyObjectBuilder_ProfilerSnapshot();
			}

			MyObjectBuilder_ProfilerSnapshot IActivator<MyObjectBuilder_ProfilerSnapshot>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_Profiler> Profilers;

		public List<MyRenderProfiler.FrameInfo> SimulationFrames;

		public static MyObjectBuilder_ProfilerSnapshot GetObjectBuilder(MyRenderProfiler profiler)
		{
			MyObjectBuilder_ProfilerSnapshot myObjectBuilder_ProfilerSnapshot = new MyObjectBuilder_ProfilerSnapshot();
			List<MyProfiler> threadProfilers = MyRenderProfiler.ThreadProfilers;
			lock (threadProfilers)
			{
				myObjectBuilder_ProfilerSnapshot.Profilers = new List<MyObjectBuilder_Profiler>(threadProfilers.Count);
				myObjectBuilder_ProfilerSnapshot.Profilers.AddRange(Enumerable.Select<MyProfiler, MyObjectBuilder_Profiler>((IEnumerable<MyProfiler>)threadProfilers, (Func<MyProfiler, MyObjectBuilder_Profiler>)MyObjectBuilder_Profiler.GetObjectBuilder));
			}
			myObjectBuilder_ProfilerSnapshot.SimulationFrames = Enumerable.ToList<MyRenderProfiler.FrameInfo>((IEnumerable<MyRenderProfiler.FrameInfo>)MyRenderProfiler.FrameTimestamps);
			return myObjectBuilder_ProfilerSnapshot;
		}

		public void Init(MyRenderProfiler profiler, SnapshotType type, bool subtract)
		{
			List<MyProfiler> list = Enumerable.ToList<MyProfiler>(Enumerable.Select<MyObjectBuilder_Profiler, MyProfiler>((IEnumerable<MyObjectBuilder_Profiler>)Profilers, (Func<MyObjectBuilder_Profiler, MyProfiler>)MyObjectBuilder_Profiler.Init));
			ConcurrentQueue<MyRenderProfiler.FrameInfo> frameTimestamps = new ConcurrentQueue<MyRenderProfiler.FrameInfo>((IEnumerable<MyRenderProfiler.FrameInfo>)SimulationFrames);
			if (subtract)
			{
				MyRenderProfiler.SubtractOnlineSnapshot(type, list, frameTimestamps);
			}
			else
			{
				MyRenderProfiler.PushOnlineSnapshot(type, list, frameTimestamps);
			}
			MyRenderProfiler.SelectedProfiler = ((list.Count > 0) ? list[0] : null);
		}

		private static void SaveToFile(int index)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				SaveCurrentViewToCsv(index);
				MyObjectBuilder_ProfilerSnapshot profilerBuilder = GetObjectBuilder(MyRenderProxy.GetRenderProfiler());
				new Thread((ThreadStart)delegate
				{
					MyObjectBuilderSerializer.SerializeXML(GetProfilerDumpPath(index), compress: true, profilerBuilder);
					Action onProfilerSnapshotSaved = MyRenderProfiler.OnProfilerSnapshotSaved;
					if (onProfilerSnapshotSaved != null)
					{
						MyRenderProxy.EnqueueMainThreadCallback(onProfilerSnapshotSaved);
					}
				}).Start();
			}
			catch (Exception)
			{
			}
		}

		private static void SaveCurrentViewToCsv(int index)
		{
			string text = Path.Combine(MyFileSystem.UserDataPath, "Profiler-" + index);
			MyProfiler selectedProfiler = MyRenderProfiler.SelectedProfiler;
			int lastValidFrame;
			using (selectedProfiler.LockHistory(out lastValidFrame))
			{
				lastValidFrame = (lastValidFrame + 1) % MyProfiler.MAX_FRAMES;
				string text2 = "-" + (selectedProfiler.SelectedRoot?.Name ?? selectedProfiler.DisplayName);
				List<MyProfilerBlock> sortedChildren = MyRenderProfiler.GetSortedChildren(lastValidFrame);
<<<<<<< HEAD
				using (StreamWriter streamWriter = new StreamWriter(text + text2 + "-time.csv"))
				{
					using (StreamWriter streamWriter2 = new StreamWriter(text + text2 + "-custom.csv"))
					{
						using (StreamWriter streamWriter3 = new StreamWriter(text + text2 + "-calls.csv"))
						{
							string value = string.Join(", ", sortedChildren.Select((MyProfilerBlock x) => x.Name));
							streamWriter.WriteLine(value);
							streamWriter2.WriteLine(value);
							streamWriter3.WriteLine(value);
							int num = (lastValidFrame + 1) % MyProfiler.MAX_FRAMES;
							while (num != lastValidFrame)
							{
								for (int i = 0; i < sortedChildren.Count; i++)
								{
									if (i > 0)
									{
										streamWriter.Write(", ");
										streamWriter2.Write(", ");
										streamWriter3.Write(",");
									}
									MyProfilerBlock myProfilerBlock = sortedChildren[i];
									streamWriter.Write(myProfilerBlock.GetMillisecondsReader(useOptimizations: false)[num]);
									streamWriter2.Write(myProfilerBlock.CustomValues[num]);
									streamWriter3.Write(myProfilerBlock.NumCallsArray[num]);
								}
								streamWriter.WriteLine();
								streamWriter2.WriteLine();
								streamWriter3.WriteLine();
								num++;
								if (num >= MyProfiler.MAX_FRAMES)
								{
									num = 0;
								}
							}
						}
=======
				using StreamWriter streamWriter = new StreamWriter(text + text2 + "-time.csv");
				using StreamWriter streamWriter2 = new StreamWriter(text + text2 + "-custom.csv");
				using StreamWriter streamWriter3 = new StreamWriter(text + text2 + "-calls.csv");
				string value = string.Join(", ", Enumerable.Select<MyProfilerBlock, string>((IEnumerable<MyProfilerBlock>)sortedChildren, (Func<MyProfilerBlock, string>)((MyProfilerBlock x) => x.Name)));
				streamWriter.WriteLine(value);
				streamWriter2.WriteLine(value);
				streamWriter3.WriteLine(value);
				int num = (lastValidFrame + 1) % MyProfiler.MAX_FRAMES;
				while (num != lastValidFrame)
				{
					for (int i = 0; i < sortedChildren.Count; i++)
					{
						if (i > 0)
						{
							streamWriter.Write(", ");
							streamWriter2.Write(", ");
							streamWriter3.Write(",");
						}
						MyProfilerBlock myProfilerBlock = sortedChildren[i];
						streamWriter.Write(myProfilerBlock.GetMillisecondsReader(useOptimizations: false)[num]);
						streamWriter2.Write(myProfilerBlock.CustomValues[num]);
						streamWriter3.Write(myProfilerBlock.NumCallsArray[num]);
					}
					streamWriter.WriteLine();
					streamWriter2.WriteLine();
					streamWriter3.WriteLine();
					num++;
					if (num >= MyProfiler.MAX_FRAMES)
					{
						num = 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		private static void LoadFromFile(int index, bool subtract)
		{
			try
			{
				MyObjectBuilderSerializer.DeserializeXML(GetProfilerDumpPath(index), out MyObjectBuilder_ProfilerSnapshot objectBuilder);
				objectBuilder.Init(MyRenderProxy.GetRenderProfiler(), SnapshotType.Snapshot, subtract);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Get the path where a profiler dump with <paramref name="index" /> should be stored.
		/// </summary>
		/// <param name="index">The index of the profiler dump.</param>
		/// <returns>The path where the dump should be stored.</returns>
		public static string GetProfilerDumpPath(int index)
		{
			return Path.Combine(MyFileSystem.UserDataPath, "FullProfiler-" + index);
		}

		public static int GetProfilerDumpCount()
		{
<<<<<<< HEAD
			return Directory.EnumerateFiles(MyFileSystem.UserDataPath, "FullProfiler-*").Count();
=======
			return Enumerable.Count<string>(Directory.EnumerateFiles(MyFileSystem.UserDataPath, "FullProfiler-*"));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void ClearProfilerDumps()
		{
<<<<<<< HEAD
			string[] array = Directory.EnumerateFiles(MyFileSystem.UserDataPath, "FullProfiler-*").ToArray();
=======
			string[] array = Enumerable.ToArray<string>(Directory.EnumerateFiles(MyFileSystem.UserDataPath, "FullProfiler-*"));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < array.Length; i++)
			{
				File.Delete(array[i]);
			}
		}

		public static void SetDelegates()
		{
			MyRenderProfiler.SaveProfilerToFile = SaveToFile;
			MyRenderProfiler.LoadProfilerFromFile = LoadFromFile;
		}
	}
}
