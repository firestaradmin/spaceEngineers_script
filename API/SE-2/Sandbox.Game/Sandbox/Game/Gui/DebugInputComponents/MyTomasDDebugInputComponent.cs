using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Input;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class MyTomasDDebugInputComponent : MyDebugComponent
	{
		private struct VoxelStreamingStats
		{
			public float FrameTime;

			public float GPUTimeSmooth;

			public float CPUTimeSmooth;

			public float TotalSwapsPerformed;

			public override string ToString()
			{
				return $"{FrameTime},{GPUTimeSmooth},{CPUTimeSmooth}";
			}
		}

		private bool m_profiling;

		private List<VoxelStreamingStats> m_statistics = new List<VoxelStreamingStats>();

		private DateTime m_profStart;

		private float m_progress;

		private float m_profTime = 120f;

		private bool m_use_replay = true;

		private VoxelStreamingStats m_median;

		public override string GetName()
		{
			return "Tomas Drinovsky";
		}

		public MyTomasDDebugInputComponent()
		{
			AddShortcut(MyKeys.P, newPress: true, control: false, shift: false, alt: false, () => "Profiling start/end", delegate
			{
				if (!m_profiling)
				{
					MyFpsManager.Reset();
					m_statistics.Clear();
					m_profStart = DateTime.UtcNow;
					if (m_use_replay)
					{
						MySessionComponentReplay.Static.StartReplay();
					}
					m_profiling = true;
				}
				else
				{
					FinishProfiling();
				}
				return true;
			});
			AddShortcut(MyKeys.O, newPress: true, control: false, shift: false, alt: false, () => "Use replay", delegate
			{
				m_use_replay = !m_use_replay;
				return true;
			});
			AddShortcut(MyKeys.L, newPress: true, control: false, shift: false, alt: false, () => "Reload world", delegate
			{
				MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
				return true;
			});
		}

		private void FinishProfiling()
		{
			MySessionComponentReplay.Static.StopReplay();
			PrintData();
			ComputeData();
			m_profiling = false;
		}

		private void ComputeData()
		{
			if (m_statistics.Count != 0)
			{
				int index = m_statistics.Count / 2;
				m_statistics.Sort((VoxelStreamingStats a, VoxelStreamingStats b) => a.FrameTime.CompareTo(b.FrameTime));
				m_median.FrameTime = m_statistics[index].FrameTime;
				m_statistics.Sort((VoxelStreamingStats a, VoxelStreamingStats b) => a.CPUTimeSmooth.CompareTo(b.CPUTimeSmooth));
				m_median.CPUTimeSmooth = m_statistics[index].CPUTimeSmooth;
				m_statistics.Sort((VoxelStreamingStats a, VoxelStreamingStats b) => a.GPUTimeSmooth.CompareTo(b.GPUTimeSmooth));
				m_median.GPUTimeSmooth = m_statistics[index].GPUTimeSmooth;
			}
		}

		private void PrintData()
		{
			double totalSeconds = (DateTime.UtcNow - m_profStart).TotalSeconds;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"FrameTime,GPUTimeSmooth,CPUTimeSmooth,,,{totalSeconds}");
			foreach (VoxelStreamingStats statistic in m_statistics)
			{
				stringBuilder.AppendLine(statistic.ToString());
			}
			File.WriteAllText("C:\\Temp\\frameStats.csv", stringBuilder.ToString());
		}

		public override void Draw()
		{
			base.Draw();
			int num = 100;
			float scale = 0.6f;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"Profiling in progress: {m_profiling} {(int)(m_progress * 100f)}%", Color.White, scale);
			num += 15;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"Use replay: {m_use_replay}", Color.White, scale);
			num += 20;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"MEDIAN FrameTime: {m_median.FrameTime}", Color.White, scale);
			num += 15;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"MEDIAN CPUTimeSmooth: {m_median.CPUTimeSmooth}", Color.White, scale);
			num += 15;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"MEDIAN GPUTimeSmooth: {m_median.GPUTimeSmooth}", Color.White, scale);
		}

		public override void DispatchUpdate()
		{
			base.DispatchUpdate();
			if (m_profiling)
			{
				m_statistics.Add(new VoxelStreamingStats
				{
					FrameTime = MyFpsManager.FrameTime,
					GPUTimeSmooth = MyRenderProxy.GPUTimeSmooth,
					CPUTimeSmooth = MyRenderProxy.CPUTimeSmooth
				});
			}
		}

		public override void Update100()
		{
			base.Update100();
			if (m_profiling)
			{
				float num = (float)(DateTime.UtcNow - m_profStart).TotalSeconds;
				if (num > m_profTime)
				{
					FinishProfiling();
				}
				m_progress = num / m_profTime;
			}
		}
	}
}
