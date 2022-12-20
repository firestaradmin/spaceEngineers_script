using System;
using System.Collections.Generic;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.Utils;
using VRage.Library.Collections;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
<<<<<<< HEAD
	/// <summary>
	/// Session component taking care of panels checking.
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof(MyObjectBuilder_MySessionComponentPanels), null, false)]
	public class MySessionComponentPanels : MySessionComponentBase
	{
		private struct MyPanelData
		{
			public float DistanceSquared;

			public int TextureMemorySize;

			public int VisibleCounter;

			public int OrderedIndex;

			public bool UpdateIsRendered(int budgetIndex)
			{
				if (OrderedIndex < budgetIndex)
				{
					VisibleCounter++;
				}
				else
				{
					VisibleCounter = 0;
				}
				return VisibleCounter > 5;
			}
		}

		private float m_maxDistanceMultiplierSquared;

		private ulong m_memoryBudget;

		private readonly MyFreeList<MyPanelData> m_insideList = new MyFreeList<MyPanelData>();

		private readonly List<int> m_orderedList = new List<int>();

		private bool m_insideListChange;

		private int m_insideListBudgetIndex;

		private int m_sortingPressureCount;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_memoryBudget = MyVRage.Platform.Render.GetMemoryBudgetForGeneratedTextures();
			MyVideoSettingsManager.OnSettingsChanged += OnSettingsChanged;
			OnSettingsChanged();
		}

		private void OnSettingsChanged()
		{
			float drawDistanceMultiplierForQuality = GetDrawDistanceMultiplierForQuality(MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality);
			m_maxDistanceMultiplierSquared = drawDistanceMultiplierForQuality * drawDistanceMultiplierForQuality;
		}

		protected override void UnloadData()
		{
			MyVideoSettingsManager.OnSettingsChanged -= OnSettingsChanged;
			base.UnloadData();
		}

		private static float GetDrawDistanceForQuality(MyTextureQuality quality)
		{
<<<<<<< HEAD
			switch (quality)
			{
			case MyTextureQuality.LOW:
				return 60f;
			case MyTextureQuality.MEDIUM:
				return 120f;
			case MyTextureQuality.HIGH:
				return 180f;
			default:
				return 120f;
			}
=======
			return quality switch
			{
				MyTextureQuality.LOW => 60f, 
				MyTextureQuality.MEDIUM => 120f, 
				MyTextureQuality.HIGH => 180f, 
				_ => 120f, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static float GetDrawDistanceMultiplierForQuality(MyTextureQuality quality)
		{
<<<<<<< HEAD
			switch (quality)
			{
			case MyTextureQuality.LOW:
				return 0.333333343f;
			case MyTextureQuality.MEDIUM:
				return 2f / 3f;
			case MyTextureQuality.HIGH:
				return 1f;
			default:
				return 2f / 3f;
			}
=======
			return quality switch
			{
				MyTextureQuality.LOW => 0.333333343f, 
				MyTextureQuality.MEDIUM => 2f / 3f, 
				MyTextureQuality.HIGH => 1f, 
				_ => 2f / 3f, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void UpdateAfterSimulation()
		{
			if (MySession.Static.GameplayFrameCounter % 10 == 4)
			{
				lock (m_orderedList)
				{
					UpdateBudget();
				}
			}
		}

		private void UpdateBudget()
		{
			int num = Math.Min(10, m_orderedList.Count / 10);
			int i = 0;
			int swaps = 0;
			bool swapsInBudget = false;
			if (m_orderedList.Count > 1)
			{
				for (; i < m_orderedList.Count - 1; i++)
				{
					int num2 = i + 1;
					if (m_insideList[m_orderedList[i]].DistanceSquared > m_insideList[m_orderedList[num2]].DistanceSquared)
					{
						ApplySwap(i, num2);
						if (swaps > num)
						{
							break;
						}
					}
					int num3 = m_orderedList.Count - i - 1;
					int num4 = num3 - 1;
					if (m_insideList[m_orderedList[num3]].DistanceSquared < m_insideList[m_orderedList[num4]].DistanceSquared)
					{
						ApplySwap(num3, num4);
						if (swaps > num)
						{
							break;
						}
					}
				}
				if (swaps > num)
				{
					m_sortingPressureCount++;
					if (m_sortingPressureCount > 4)
					{
						m_orderedList.Sort((int x, int y) => m_insideList[x].DistanceSquared.CompareTo(m_insideList[y].DistanceSquared));
						for (i = 0; i < m_orderedList.Count; i++)
						{
							m_insideList[m_orderedList[i]].OrderedIndex = i;
						}
						m_sortingPressureCount = 0;
						swapsInBudget = true;
					}
				}
				else
				{
					m_sortingPressureCount = 0;
				}
			}
			if (swapsInBudget || m_insideListChange)
			{
				ulong num5 = 0uL;
				i = 0;
				while (i < m_orderedList.Count && num5 < m_memoryBudget)
				{
					num5 += (ulong)m_insideList[m_orderedList[i++]].TextureMemorySize;
				}
				if (num5 > m_memoryBudget)
				{
					i--;
				}
				m_insideListBudgetIndex = i;
				m_insideListChange = false;
			}
			void ApplySwap(int current, int next)
			{
				int value = m_orderedList[next];
				m_orderedList[next] = m_orderedList[current];
				m_orderedList[current] = value;
				m_insideList[m_orderedList[current]].OrderedIndex = current;
				m_insideList[m_orderedList[next]].OrderedIndex = next;
				swaps++;
				if (current <= m_insideListBudgetIndex)
				{
					swapsInBudget = true;
				}
			}
		}

		public bool IsInRange(IMyTextPanelProvider panelProvider, float maxDistanceSquared)
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null || Sync.IsDedicated)
			{
				return false;
			}
			float num = ((Vector3)(panelProvider.WorldPosition - mainCamera.Position)).LengthSquared();
			maxDistanceSquared *= m_maxDistanceMultiplierSquared;
			bool flag = num < maxDistanceSquared;
			int rangeIndex = panelProvider.RangeIndex;
			bool flag2 = rangeIndex != -1;
			if (flag2 != flag)
			{
				lock (m_orderedList)
				{
					if (flag2)
					{
						RemovePanel(panelProvider);
					}
					else
					{
						AddPanel(panelProvider, num);
					}
				}
			}
			else if (flag2)
			{
				m_insideList[rangeIndex].DistanceSquared = num;
				return m_insideList[rangeIndex].UpdateIsRendered(m_insideListBudgetIndex);
			}
			return false;
		}

		private void AddPanel(IMyTextPanelProvider panelProvider, float distanceSquared)
		{
			int rangeIndex = panelProvider.RangeIndex;
			m_orderedList.Add(rangeIndex = m_insideList.Allocate());
			m_insideList[rangeIndex] = new MyPanelData
			{
				DistanceSquared = distanceSquared,
				TextureMemorySize = panelProvider.PanelTexturesByteCount,
				OrderedIndex = m_orderedList.Count - 1
			};
			panelProvider.RangeIndex = rangeIndex;
			m_insideListChange = true;
		}

		private void RemovePanel(IMyTextPanelProvider panelProvider)
		{
			int rangeIndex = panelProvider.RangeIndex;
			int orderedIndex = m_insideList[rangeIndex].OrderedIndex;
			m_orderedList.RemoveAtFast(orderedIndex);
			if (m_orderedList.Count > orderedIndex)
			{
				m_insideList[m_orderedList[orderedIndex]].OrderedIndex = orderedIndex;
			}
			m_insideList.Free(rangeIndex);
			panelProvider.RangeIndex = -1;
			m_insideListChange = true;
		}

		public void Remove(IMyTextPanelProvider panelProvider)
		{
			if (panelProvider.RangeIndex != -1)
			{
				lock (m_orderedList)
				{
					RemovePanel(panelProvider);
				}
			}
		}
	}
}
