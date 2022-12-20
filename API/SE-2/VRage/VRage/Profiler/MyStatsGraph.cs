using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;

namespace VRage.Profiler
{
	/// <summary>
	/// Shortcut class for profiler.
	/// </summary>
	public static class MyStatsGraph
	{
		public static string PROFILER_NAME;

		private static readonly MyProfiler m_profiler;

		private static readonly Stack<float> m_stack;

		public static bool Started { get; private set; }

		static MyStatsGraph()
		{
			PROFILER_NAME = "Statistics";
			m_stack = new Stack<float>(32);
			m_profiler = MyRenderProfiler.CreateProfiler(PROFILER_NAME, "B");
			m_profiler.AutoCommit = false;
			m_profiler.SetNewLevelLimit(-1);
			m_profiler.AutoScale = true;
			m_profiler.IgnoreRoot = true;
		}

		private static MyTimeSpan? ToTime(this float customTime)
		{
			return MyTimeSpan.FromMilliseconds(customTime);
		}

		/// <summary>
		/// Starts profiling block.
		/// </summary>
		public static void Begin(string blockName = null, int forceOrder = int.MaxValue, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			if (!MyRenderProfiler.Paused)
			{
				m_profiler.StartBlock(blockName, member, line, file, forceOrder);
				m_stack.Push(0f);
			}
		}

		/// <summary>
		/// End profiling block.
		/// </summary>
		/// <param name="bytesTransfered">Specify number of bytes transferred or null to automatically calculate number of bytes from inner blocks.</param>
		/// <param name="customValue">You can put any number here.</param>
		/// <param name="customValueFormat">This is formatting string how the number will be written on screen, use something like: 'MyNumber: {0} foos/s'</param>
		/// <param name="byteFormat"></param>
		/// <param name="callFormat"></param>
		/// <param name="numCalls"></param>
		/// <param name="member"></param>
		/// <param name="line"></param>
		/// <param name="file"></param>
		public static void End(float? bytesTransfered = null, float customValue = 0f, string customValueFormat = "", string byteFormat = "{0} B", string callFormat = null, int numCalls = 0, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			if (!MyRenderProfiler.Paused)
			{
				float num = m_stack.Pop();
				float num2 = bytesTransfered ?? num;
				m_profiler.EndBlock(member, line, file, num2.ToTime(), customValue, byteFormat, customValueFormat, callFormat, numCalls);
<<<<<<< HEAD
				if (m_stack.Count > 0)
=======
				if (m_stack.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_stack.Push(m_stack.Pop() + num2);
				}
			}
		}

		public static void CustomTime(string name, float customTime, string timeFormat = null, float customValue = 0f, string customValueFormat = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			if (!MyRenderProfiler.Paused)
			{
				m_profiler.StartBlock(name, member, line, file);
				m_profiler.EndBlock(member, line, file, customTime.ToTime(), customValue, timeFormat, customValueFormat);
			}
		}

		public static void Commit()
		{
			if (MyRenderProfiler.Paused)
			{
				m_profiler.ClearFrame();
			}
			else
			{
				m_profiler.CommitFrame();
			}
		}

		public static void ProfileAdvanced(bool begin)
		{
			if (begin)
			{
<<<<<<< HEAD
				Begin("Advanced", int.MaxValue, "ProfileAdvanced", 96, "E:\\Repo1\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
			}
			else
			{
				End(0f, 0f, null, "{0}", null, 0, "ProfileAdvanced", 100, "E:\\Repo1\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
=======
				Begin("Advanced", int.MaxValue, "ProfileAdvanced", 95, "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
			}
			else
			{
				End(0f, 0f, null, "{0}", null, 0, "ProfileAdvanced", 99, "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static void ProfilePacketStatistics(bool begin)
		{
			if (begin)
			{
<<<<<<< HEAD
				Begin("Packet statistics", int.MaxValue, "ProfilePacketStatistics", 108, "E:\\Repo1\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
			}
			else
			{
				End(0f, 0f, null, "{0}", null, 0, "ProfilePacketStatistics", 112, "E:\\Repo1\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
=======
				Begin("Packet statistics", int.MaxValue, "ProfilePacketStatistics", 107, "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
			}
			else
			{
				End(0f, 0f, null, "{0}", null, 0, "ProfilePacketStatistics", 111, "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static void Start()
		{
			if (!Started && MyRenderProfiler.Paused)
			{
				MyRenderProfiler.SwitchPause();
			}
			Started = true;
		}
	}
}
