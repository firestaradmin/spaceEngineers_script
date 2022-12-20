using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRage.Voxels;
using VRage.Voxels.Clipmap;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Clipmap")]
	public class MyGuiScreenDebugClipmap : MyGuiScreenDebugBase
	{
		private struct Benchmark
		{
			public MyVoxelClipmap.StitchMode StitchMode;

			public int CellSize;

			public int Lod0Size;

			public int LodSize;

			public Benchmark(MyVoxelClipmap.StitchMode stitchMode, int cellSize, int lod0Size, int lodSize)
			{
				StitchMode = stitchMode;
				CellSize = cellSize;
				Lod0Size = lod0Size;
				LodSize = lodSize;
			}

			public override string ToString()
			{
				return $"{StitchMode} {1 << CellSize}-{Lod0Size}-{LodSize}";
			}
		}

<<<<<<< HEAD
=======
		private static MyRenderQualityEnum m_voxelRangesQuality;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool m_debugDrawGrid;

		private int m_debugDrawGridSize;

		private MyGuiControlLabel m_timeLabel;

		private MyGuiControlLabel m_benchmarkLabel;

		private MyGuiControlLabel m_cacheHitHistory;

		private int m_cellSize = 4;

		private int m_lod0Size = 2;

		private int m_highLodSize = 2;

		private MyStorageData m_data = new MyStorageData();

		private readonly Benchmark[] m_testSuite = new Benchmark[4]
		{
			new Benchmark(MyVoxelClipmap.StitchMode.Stitch, 4, 2, 2),
			new Benchmark(MyVoxelClipmap.StitchMode.Stitch, 5, 2, 2),
			new Benchmark(MyVoxelClipmap.StitchMode.Stitch, 4, 3, 2),
			new Benchmark(MyVoxelClipmap.StitchMode.Stitch, 4, 3, 3)
		};

		private readonly StringBuilder m_results = new StringBuilder();

		private int m_testCount = 6;

		private int m_testRemaining;

		private Queue<Benchmark> m_benchmarks = new Queue<Benchmark>();

		private Stopwatch m_testTimer = new Stopwatch();

		private Benchmark m_currentBenchmark;

		public bool EnableCache
		{
			get
			{
				foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
				{
					MyRenderComponentVoxelMap myRenderComponentVoxelMap;
					MyVoxelClipmap myVoxelClipmap;
					if ((myRenderComponentVoxelMap = instance.Render as MyRenderComponentVoxelMap) != null && (myVoxelClipmap = myRenderComponentVoxelMap.Clipmap as MyVoxelClipmap) != null)
					{
						return myVoxelClipmap.Cache != null;
					}
				}
				return false;
			}
			set
			{
				foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
				{
					MyRenderComponentVoxelMap myRenderComponentVoxelMap;
					MyVoxelClipmap myVoxelClipmap;
					if ((myRenderComponentVoxelMap = instance.Render as MyRenderComponentVoxelMap) != null && (myVoxelClipmap = myRenderComponentVoxelMap.Clipmap as MyVoxelClipmap) != null)
					{
						if (value && myVoxelClipmap.Cache == null)
						{
							myVoxelClipmap.Cache = MyVoxelClipmapCache.Instance;
						}
						else if (!value && myVoxelClipmap.Cache != null)
						{
							myVoxelClipmap.Cache = null;
						}
					}
				}
			}
		}

		public MyGuiScreenDebugClipmap()
		{
<<<<<<< HEAD
=======
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugClipmap";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_scale = 0.6f;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.08f);
			AddCaption("Clipmap", Color.Yellow.ToVector4());
			AddCheckBox("Update Clipmaps", null, MemberHelper.GetMember(() => MyClipmap.EnableUpdate));
			AddCheckBox("Update Clipmap Visibility", null, MemberHelper.GetMember(() => MyVoxelClipmap.UpdateVisibility));
			AddCheckBox("Debug draw clipmap cells", null, MemberHelper.GetMember(() => MyClipmap.EnableDebugDraw));
			AddCheckBox("Debug cell lod colors", MyClipmap.DebugDrawColors, delegate(MyGuiControlCheckbox x)
			{
				MyClipmap.DebugDrawColors = x.IsChecked;
			});
			AddCheckBox("Debug Draw Mesh Dependencies", null, MemberHelper.GetMember(() => MyVoxelClipmap.DebugDrawDependencies));
			AddCheckBox("Debug Draw Stitching", null, MemberHelper.GetMember(() => MyClipmapSewJob.DebugDrawDependencies));
			AddCheckBox("Debug Draw Vertex Generation", null, MemberHelper.GetMember(() => MyClipmapSewJob.DebugDrawGeneration));
			AddCombo<MyVoxelClipmap.StitchMode>(null, MemberHelper.GetMember(() => MyVoxelClipmap.ActiveStitchMode)).SetToolTip("Stitching Mode");
			AddCombo<VrTailor.GeneratedVertexProtocol>(null, MemberHelper.GetMember(() => MyClipmapSewJob.GeneratedVertexProtocol)).SetToolTip("Vertex Generation Protocol");
			AddCheckBox("Wireframe", MyRenderProxy.Settings.Wireframe, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.Wireframe = x.IsChecked;
				MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
			});
			m_currentPosition.Y += 0.01f;
			MyVoxelClipmapSettings settings = MyVoxelClipmapSettings.GetSettings("Planet");
			m_cellSize = settings.CellSizeLg2;
			int num = 1 << settings.CellSizeLg2;
			m_lod0Size = settings.LodRanges[0] / num;
			m_highLodSize = settings.LodRanges[1] / settings.LodRanges[0];
			AddSlider("Cell Log2", 3f, 6f, () => m_cellSize, delegate(float x)
			{
				m_cellSize = (int)x;
			});
			AddSlider("Lod 0 Size", 1f, 8f, () => m_lod0Size, delegate(float x)
			{
				m_lod0Size = (int)x;
			});
			AddSlider("Higher Lod Size", 1f, 4f, () => m_highLodSize, delegate(float x)
			{
				m_highLodSize = (int)x;
			});
			AddButton("Recalculate All Cells", delegate
			{
				foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
				{
					MyRenderComponentVoxelMap myRenderComponentVoxelMap2;
					if ((myRenderComponentVoxelMap2 = instance.Render as MyRenderComponentVoxelMap) != null)
					{
						myRenderComponentVoxelMap2.Clipmap.InvalidateRange(Vector3I.Zero, myRenderComponentVoxelMap2.Clipmap.Size);
					}
				}
			});
			AddButton("Reset All Clipmaps", delegate
			{
				foreach (MyVoxelBase instance2 in MySession.Static.VoxelMaps.Instances)
				{
					MyRenderComponentVoxelMap myRenderComponentVoxelMap;
					if ((myRenderComponentVoxelMap = instance2.Render as MyRenderComponentVoxelMap) != null)
					{
						MyVoxelClipmap myVoxelClipmap;
						if ((myVoxelClipmap = myRenderComponentVoxelMap.Clipmap as MyVoxelClipmap) != null)
						{
							if (!myVoxelClipmap.UpdateSettings(MyVoxelClipmapSettings.Create(m_cellSize, m_lod0Size, m_highLodSize)))
							{
								myRenderComponentVoxelMap.Clipmap.InvalidateAll();
							}
						}
						else
						{
							myRenderComponentVoxelMap.Clipmap.InvalidateAll();
						}
					}
				}
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Worker time:", Color.Yellow, 0.7f);
			m_timeLabel = AddLabel("0ms", Color.Yellow, 0.7f);
			AddButton("Reset Timings", delegate
			{
			});
			AddButton("Benchmark Current Settings", delegate
			{
				m_benchmarks.Enqueue(new Benchmark(MyVoxelClipmap.ActiveStitchMode, m_cellSize, m_lod0Size, m_highLodSize));
				FindClipmapAndBenchmark();
			});
			AddSlider("Test Count", 1f, 10f, () => m_testCount, delegate(float x)
			{
				m_testCount = (int)x;
			});
			AddCheckBox("Enable Cache", EnableCache, delegate(MyGuiControlCheckbox x)
			{
				EnableCache = x.IsChecked;
			});
			m_cacheHitHistory = AddLabel("Cache", Color.Yellow, 0.7f);
			m_currentPosition.Y += 0.07f;
			AddButton("Run Full Benchmark Suite", delegate
			{
				Benchmark[] testSuite = m_testSuite;
<<<<<<< HEAD
				foreach (Benchmark item in testSuite)
				{
					m_benchmarks.Enqueue(item);
=======
				foreach (Benchmark benchmark in testSuite)
				{
					m_benchmarks.Enqueue(benchmark);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				FindClipmapAndBenchmark();
			});
			m_benchmarkLabel = AddLabel("", Color.Yellow, 0.7f);
		}

		public override bool Draw()
		{
			m_timeLabel.Text = $"{MyClipmapTiming.Total.TotalMilliseconds:n} ms";
			float currentHitRatio = MyVoxelClipmapCache.Instance.DebugHitCounter.CurrentHitRatio;
<<<<<<< HEAD
			MyDebugHitCounter.Sample[] array = MyVoxelClipmapCache.Instance.DebugHitCounter.History.Where((MyDebugHitCounter.Sample x) => !float.IsNaN(x.Value)).ToArray();
			m_cacheHitHistory.Text = string.Format("Cache Hit Rate:\n Current: {0:F2}\n LastUpdates: {1}\n Historical Average: {2:F2}", float.IsNaN(currentHitRatio) ? 0f : currentHitRatio, string.Join(", ", array.Select((MyDebugHitCounter.Sample x) => x.Value.ToString("F2"))), (array.Length != 0) ? array.Average((MyDebugHitCounter.Sample x) => x.Value) : 0f);
=======
			MyDebugHitCounter.Sample[] array = Enumerable.ToArray<MyDebugHitCounter.Sample>(Enumerable.Where<MyDebugHitCounter.Sample>((IEnumerable<MyDebugHitCounter.Sample>)MyVoxelClipmapCache.Instance.DebugHitCounter.History, (Func<MyDebugHitCounter.Sample, bool>)((MyDebugHitCounter.Sample x) => !float.IsNaN(x.Value))));
			m_cacheHitHistory.Text = string.Format("Cache Hit Rate:\n Current: {0:F2}\n LastUpdates: {1}\n Historical Average: {2:F2}", float.IsNaN(currentHitRatio) ? 0f : currentHitRatio, string.Join(", ", Enumerable.Select<MyDebugHitCounter.Sample, string>((IEnumerable<MyDebugHitCounter.Sample>)array, (Func<MyDebugHitCounter.Sample, string>)((MyDebugHitCounter.Sample x) => x.Value.ToString("F2")))), (array.Length != 0) ? Enumerable.Average<MyDebugHitCounter.Sample>((IEnumerable<MyDebugHitCounter.Sample>)array, (Func<MyDebugHitCounter.Sample, float>)((MyDebugHitCounter.Sample x) => x.Value)) : 0f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_debugDrawGrid)
			{
				int num = 8 * (1 << m_debugDrawGridSize);
				Vector3D position = MySector.MainCamera.Position;
				BoundingBoxD box = new BoundingBoxD(position - num, position + num);
				List<MyVoxelBase> list = new List<MyVoxelBase>();
				MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, list);
				foreach (MyVoxelBase item in list)
				{
					Vector3D localPos = Vector3D.Transform(position, item.PositionComp.WorldMatrixInvScaled) / item.VoxelSize;
					DrawVoxelGrid(item, MyClipmap.LodColors[m_debugDrawGridSize], localPos, num, m_debugDrawGridSize);
				}
			}
			return base.Draw();
		}

		private void DrawVoxelGrid(MyVoxelBase voxel, Color col, Vector3D localPos, int range, int lod)
		{
			int num = 1 << lod;
			Vector3I value = new Vector3I((localPos - range) / num);
			Vector3I value2 = new Vector3I((localPos + range) / num) - 1;
			Vector3I vector3I = voxel.Size / num >> 1;
			value = Vector3I.Clamp(value, -vector3I, vector3I - 1);
			value2 = Vector3I.Clamp(value2, -vector3I, vector3I - 1);
			m_data.Resize(value2 - value + 3);
			Vector3I lodVoxelRangeMin = value - 1 + vector3I;
			Vector3I lodVoxelRangeMax = value2 + 1 + vector3I;
			MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.UseNativeProvider;
			voxel.Storage.ReadRange(m_data, MyStorageDataTypeFlags.Content, lod, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
<<<<<<< HEAD
			IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(voxel.PositionComp.WorldMatrixRef, new Color(col, 0.05f));
=======
			IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(voxel.PositionComp.WorldMatrix, new Color(col, 0.05f));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = value.X; i <= value2.X; i++)
			{
				for (int j = value.Y; j <= value2.Y; j++)
				{
					for (int k = value.Z; k <= value2.Z; k++)
					{
						if (IsNearIsoSurface(i - value.X, j - value.Y, k - value.Z))
						{
							Vector3D vector3D = new Vector3D(i, j, k) * num;
							BoundingBoxD aabb = new BoundingBoxD(vector3D, vector3D + num);
							myDebugDrawBatchAabb.Add(ref aabb);
						}
					}
				}
			}
			myDebugDrawBatchAabb.Dispose();
		}

		private bool IsNearIsoSurface(int x, int y, int z)
		{
			bool flag = m_data.Get(MyStorageDataTypeEnum.Content, x, y, z) < 127;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						if (m_data.Get(MyStorageDataTypeEnum.Content, x + i, y + j, z + k) < 127 != flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}

		private void FindClipmapAndBenchmark()
		{
			if (m_testRemaining != 0)
			{
				return;
			}
			MyVoxelClipmap myVoxelClipmap = null;
			foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
			{
				MyRenderComponentVoxelMap myRenderComponentVoxelMap = instance.Render as MyRenderComponentVoxelMap;
				if (myRenderComponentVoxelMap != null)
				{
					myVoxelClipmap = myRenderComponentVoxelMap.Clipmap as MyVoxelClipmap;
					if (myVoxelClipmap != null)
					{
						break;
					}
				}
			}
<<<<<<< HEAD
			if (myVoxelClipmap != null && QueueExtensions.TryDequeue(m_benchmarks, out var result))
=======
			if (myVoxelClipmap != null && m_benchmarks.TryDequeue<Benchmark>(out var result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				RunBenchmark(myVoxelClipmap, result);
			}
		}

		private void RunBenchmark(MyVoxelClipmap clipmap, Benchmark settings)
		{
			m_currentBenchmark = settings;
			m_results.AppendFormat("Clipmap Benchmark\nBuild: {4}\nStitch Mode: {0}\nLod Parameters: {1} {2} {3}\n", m_currentBenchmark.StitchMode, m_currentBenchmark.CellSize, m_currentBenchmark.Lod0Size, m_currentBenchmark.LodSize, MyCompilationSymbols.IsDebugBuild ? "Debug" : "Release");
			m_testRemaining = m_testCount;
			clipmap.Loaded += TestComplete;
			m_testTimer.Restart();
			MyVoxelClipmap.ActiveStitchMode = settings.StitchMode;
			if (!clipmap.UpdateSettings(MyVoxelClipmapSettings.Create(settings.CellSize, settings.Lod0Size, settings.LodSize)))
			{
				clipmap.InvalidateAll();
			}
			UpdateBenchmarkLabel();
		}

		private void FireTest(MyVoxelClipmap clipmap)
		{
			m_testTimer.Restart();
			clipmap.InvalidateAll();
		}

		private void TestComplete(IMyLodController clipmap)
		{
			m_testTimer.Stop();
<<<<<<< HEAD
			m_results.AppendFormat("{0:n} : {1:n}\n", MyClipmapTiming.Total.TotalMilliseconds, m_testTimer.Elapsed.TotalMilliseconds);
=======
			m_results.AppendFormat("{0:n} : {1:n}\n", MyClipmapTiming.Total.TotalMilliseconds, m_testTimer.get_Elapsed().TotalMilliseconds);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (--m_testRemaining <= 0)
			{
				clipmap.Loaded -= TestComplete;
				string path = string.Format("{0} {1}.txt", MyCompilationSymbols.IsDebugBuild ? "Debug" : "Release", m_currentBenchmark);
<<<<<<< HEAD
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Clipmap Benchmark");
				string path2 = Path.Combine(text, path);
				Directory.CreateDirectory(text);
				File.WriteAllText(path2, m_results.ToString());
				m_results.Clear();
				if (QueueExtensions.TryDequeue(m_benchmarks, out var result))
=======
				string text = Path.Combine(Environment.GetFolderPath((SpecialFolder)0), "Clipmap Benchmark");
				string text2 = Path.Combine(text, path);
				Directory.CreateDirectory(text);
				File.WriteAllText(text2, m_results.ToString());
				m_results.Clear();
				if (m_benchmarks.TryDequeue<Benchmark>(out var result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					RunBenchmark((MyVoxelClipmap)clipmap, result);
				}
			}
			else
			{
				FireTest((MyVoxelClipmap)clipmap);
			}
			UpdateBenchmarkLabel();
		}

		private void UpdateBenchmarkLabel()
		{
			if (m_testRemaining == 0)
			{
				m_benchmarkLabel.Text = "";
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Benchmarking: {0}\nExecuting run {1} out of {2}.\n", m_currentBenchmark, m_testRemaining, m_testCount);
<<<<<<< HEAD
			if (m_benchmarks.Count > 0)
			{
				stringBuilder.AppendFormat("{0} benchmark sets queued.", m_benchmarks.Count);
=======
			if (m_benchmarks.get_Count() > 0)
			{
				stringBuilder.AppendFormat("{0} benchmark sets queued.", m_benchmarks.get_Count());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_benchmarkLabel.Text = stringBuilder.ToString();
		}
	}
}
