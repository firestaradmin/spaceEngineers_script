using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using SharpDX.Direct3D11;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRageRender;

namespace VRage.Render11.Profiler
{
	internal static class MyGpuProfiler
	{
		private static readonly Queue<MyFrameProfiling> m_pooledFrames;

		private static readonly Queue<MyFrameProfiling> m_frames;

		private static MyFrameProfiling m_currentFrame;

		private static readonly Stack<ulong> m_timestampStack;

		public static bool Paused;

		public static event Action<string, string, string> OnBlockStart;

		public static event Action<MyTimeSpan, float, string, string> OnBlockEnd;

		public static event Action OnCommit;

		public static event Func<bool> IsPaused;

		static MyGpuProfiler()
		{
			m_pooledFrames = new Queue<MyFrameProfiling>(8);
			m_frames = new Queue<MyFrameProfiling>(8);
			m_currentFrame = null;
			m_timestampStack = new Stack<ulong>();
			for (int i = 0; i < 8; i++)
			{
				m_pooledFrames.Enqueue(new MyFrameProfiling());
			}
		}

		private static void WaitForLastFrame()
		{
<<<<<<< HEAD
			if (m_frames.Count != 0)
			{
				MyFrameProfiling myFrameProfiling = m_frames.ElementAt(0);
=======
			if (m_frames.get_Count() != 0)
			{
				MyFrameProfiling myFrameProfiling = Enumerable.ElementAt<MyFrameProfiling>((IEnumerable<MyFrameProfiling>)m_frames, 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				while (!myFrameProfiling.IsFinished())
				{
					Thread.Sleep(0);
				}
			}
		}

		private static void GatherFinishedFrames()
		{
<<<<<<< HEAD
			if (m_frames.Count == 0)
=======
			if (m_frames.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			bool flag = true;
<<<<<<< HEAD
			while (flag && m_frames.Count > 0)
			{
				MyFrameProfiling myFrameProfiling = m_frames.ElementAt(0);
=======
			while (flag && m_frames.get_Count() > 0)
			{
				MyFrameProfiling myFrameProfiling = Enumerable.ElementAt<MyFrameProfiling>((IEnumerable<MyFrameProfiling>)m_frames, 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				flag = myFrameProfiling.IsFinished();
				if (flag)
				{
					m_frames.Dequeue();
					GatherFrame(myFrameProfiling);
					m_pooledFrames.Enqueue(myFrameProfiling);
				}
			}
		}

		private static void GatherFrame(MyFrameProfiling frame)
		{
			QueryDataTimestampDisjoint data = MyRender11.RCForQueries.GetData<QueryDataTimestampDisjoint>(frame.Disjoint.Query, AsynchronousFlags.DoNotFlush);
			if (!data.Disjoint)
			{
				long frequency = data.Frequency;
				double num = 1.0 / (double)frequency;
				m_timestampStack.Clear();
				int num2 = 0;
<<<<<<< HEAD
				while (frame.Issued.Count > 0)
=======
				while (frame.Issued.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyIssuedQuery myIssuedQuery = frame.Issued.Dequeue();
					MyImmediateRC.RC.GetData<ulong>((Query)myIssuedQuery.Query, AsynchronousFlags.DoNotFlush, out var result);
					if (myIssuedQuery.Info == MyIssuedQueryEnum.BlockStart)
					{
						num2++;
						MyGpuProfiler.OnBlockStart(myIssuedQuery.Tag, myIssuedQuery.Member, myIssuedQuery.File);
						m_timestampStack.Push(result);
					}
					else if (myIssuedQuery.Info == MyIssuedQueryEnum.BlockEnd)
					{
						num2--;
						ulong num3 = m_timestampStack.Pop();
						double seconds = (double)(result - num3) * num;
						MyGpuProfiler.OnBlockEnd(MyTimeSpan.FromSeconds(seconds), myIssuedQuery.CustomValue, myIssuedQuery.Member, myIssuedQuery.File);
					}
					MyQueryFactory.RelaseTimestampQuery(myIssuedQuery.Query);
				}
			}
			MyGpuProfiler.OnCommit();
			frame.Clear();
			Paused = MyGpuProfiler.IsPaused();
		}

		internal static void IC_Enqueue(MyIssuedQuery q)
		{
			if (m_currentFrame != null)
			{
				m_currentFrame.Issued.Enqueue(q);
			}
		}

		internal static void Join(MyFrameProfilingContext context)
		{
			if (context != null)
			{
<<<<<<< HEAD
				while (context.Issued.Count > 0)
=======
				while (context.Issued.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					IC_Enqueue(context.Issued.Dequeue());
				}
			}
		}

		private static void FrameBlock(bool start)
		{
			if (start)
			{
<<<<<<< HEAD
				IC_BeginBlock("Frame", "FrameBlock", "E:\\Repo1\\Sources\\VRage.Render11\\Profiler\\MyGpuProfiler.cs");
			}
			else
			{
				IC_EndBlock(0f, "FrameBlock", "E:\\Repo1\\Sources\\VRage.Render11\\Profiler\\MyGpuProfiler.cs");
=======
				IC_BeginBlock("Frame", "FrameBlock", "E:\\Repo3\\Sources\\VRage.Render11\\Profiler\\MyGpuProfiler.cs");
			}
			else
			{
				IC_EndBlock(0f, "FrameBlock", "E:\\Repo3\\Sources\\VRage.Render11\\Profiler\\MyGpuProfiler.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal static void StartFrame()
		{
<<<<<<< HEAD
			if (m_pooledFrames.Count == 0)
=======
			if (m_pooledFrames.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				WaitForLastFrame();
				GatherFinishedFrames();
			}
			m_currentFrame = m_pooledFrames.Dequeue();
			MyQuery myQuery = MyQueryFactory.CreateDisjointQuery();
			MyImmediateRC.RC.Begin((Query)myQuery);
			m_currentFrame.Disjoint = myQuery;
			FrameBlock(start: true);
		}

		internal static void EndFrame()
		{
			if (m_currentFrame != null)
			{
				FrameBlock(start: false);
				MyImmediateRC.RC.End((Query)m_currentFrame.Disjoint);
				m_frames.Enqueue(m_currentFrame);
			}
		}

		internal static void IC_BeginBlock(string tag, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
		}

		internal static void IC_BeginNextBlock(string tag, float previousBlockCustomValue = 0f, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
		}

		internal static void IC_EndBlock(float customValue = 0f, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
		}

		/// <summary>
		/// IC_BeginBlock that works even when PerformanceProfilingSymbol is false
		/// </summary>
		internal static void IC_BeginBlockAlways(string tag, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
			MyImmediateRC.RC.BeginProfilingBlockAlways(tag, member, file);
		}

		/// <summary>
		/// IC_EndBlock that works even when PerformanceProfilingSymbol is false
		/// </summary>
		internal static void IC_EndBlockAlways(float customValue = 0f, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
			MyImmediateRC.RC.EndProfilingBlockAlways(customValue, member, file);
		}
	}
}
