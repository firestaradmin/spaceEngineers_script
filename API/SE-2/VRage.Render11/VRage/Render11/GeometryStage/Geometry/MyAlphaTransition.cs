using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VRage.Library.Collections;
using VRage.Render;
using VRageRender;

namespace VRage.Render11.GeometryStage.Geometry
{
	internal static class MyAlphaTransition
	{
		[StructLayout(LayoutKind.Auto)]
		private struct ProxyInfo
		{
			/// <summary>
			/// Object actively under transition.
			///
			/// When the process is complete the proxy is notified.
			/// </summary>
			public IMyAlphaTransitionProxy Proxy;

			/// <summary>
			/// Total transition time in milliseconds.
			///
			/// If this is positive it ticks to fully visible, fully invisible otherwise.
			/// </summary>
			public short TransitionTime;

			/// <summary>
			/// Current dithering time in milliseconds.
			/// </summary>
			public short Current;

			/// <summary>
			/// Callback to call when fade is finished.
			/// </summary>
			public Action<uint> FadeFinishedCallback;

			public MyAlphaTransitionDirection NextDir;
		}

		private static readonly MyFreeList<ProxyInfo> m_transitionInfo = new MyFreeList<ProxyInfo>();

		private static readonly Dictionary<IMyAlphaTransitionProxy, int> m_proxyIndex = new Dictionary<IMyAlphaTransitionProxy, int>();

		/// <summary>
		/// Add a new proxy for transition.
		///
		/// If the proxy was already added we take replace the existing transition.
		/// </summary>
		/// <param name="proxy">The transition proxy.</param>
		/// <param name="transitionTime">The time over which to perform the blending.</param>
		/// <param name="dir">Blending direction.</param>
		/// <param name="finishedCallback">Optional user callback for when the process finishes.</param>
		/// <param name="continuePrevious">Set the progress based on the transition time of any existing operation.</param>
<<<<<<< HEAD
		/// <param name="finishPrevious"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void Add(IMyAlphaTransitionProxy proxy, int transitionTime, MyAlphaTransitionDirection dir, Action<uint> finishedCallback = null, bool continuePrevious = false, bool finishPrevious = false)
		{
			int num = 0;
			MyAlphaTransitionDirection nextDir = MyAlphaTransitionDirection.None;
			if (m_proxyIndex.TryGetValue(proxy, out var value))
			{
				ProxyInfo info = m_transitionInfo[value];
				if (finishPrevious)
				{
					info.NextDir = dir;
					m_transitionInfo[value] = info;
					return;
				}
				if (continuePrevious)
				{
					num = (int)(GetTransitionRate(ref info) * (float)transitionTime);
					if (dir == MyAlphaTransitionDirection.FadeOut)
					{
						num = transitionTime - num;
					}
					if (GetDirection(info.TransitionTime) != dir)
					{
						CompleteOnly(ref info);
					}
				}
				else
				{
					CompleteOnly(ref info);
				}
			}
			else
			{
				value = m_transitionInfo.Allocate();
			}
			m_proxyIndex[proxy] = value;
			m_transitionInfo[value] = new ProxyInfo
			{
				TransitionTime = (short)((dir == MyAlphaTransitionDirection.FadeIn) ? transitionTime : (~transitionTime)),
				Current = (short)num,
				Proxy = proxy,
				FadeFinishedCallback = finishedCallback,
				NextDir = nextDir
			};
			if (dir == MyAlphaTransitionDirection.FadeIn)
			{
				proxy.SetAlpha(MyAlphaMode.DitherIn, (float)num / (float)transitionTime);
			}
			else
			{
				proxy.SetAlpha(MyAlphaMode.DitherOut, 1f - (float)num / (float)transitionTime);
			}
			proxy.TransitionStart(dir);
		}

		/// <summary>
		/// Remove any transitions involving the provided proxy.
		/// </summary>
		/// <param name="proxy">The object to remove.</param>
		/// <param name="executeCompletion">Whether to invoke the completion method before removing.</param>
		public static void Remove(IMyAlphaTransitionProxy proxy, bool executeCompletion = true)
		{
			if (m_proxyIndex.TryGetValue(proxy, out var value))
			{
				ProxyInfo info = m_transitionInfo[value];
				if (executeCompletion)
				{
					Complete(ref info);
				}
				m_transitionInfo.Free(value);
				m_proxyIndex.Remove(proxy);
			}
		}

		/// <summary>
		/// Whether a proxy is undergoing a transition at the moment.
		/// </summary>
		/// <param name="proxy"></param>
		/// <returns></returns>
		public static bool Contains(IMyAlphaTransitionProxy proxy)
		{
			return m_proxyIndex.ContainsKey(proxy);
		}

		public static void Update()
		{
			short num = (short)Math.Min(MyCommon.GetLastFrameDelta() * 1000f, 32767f);
			ProxyInfo[] internalArray = m_transitionInfo.GetInternalArray();
			int usedLength = m_transitionInfo.UsedLength;
			for (int i = 0; i < usedLength; i++)
			{
				if (internalArray[i].TransitionTime == 0)
				{
					continue;
				}
				internalArray[i].Current += num;
				MyAlphaMode mode;
				float value;
				if (internalArray[i].TransitionTime > 0)
				{
					mode = MyAlphaMode.DitherIn;
					if (internalArray[i].TransitionTime < internalArray[i].Current)
					{
						CompleteAndRemove(ref internalArray[i], i);
						continue;
					}
					value = (float)internalArray[i].Current / (float)internalArray[i].TransitionTime;
				}
				else
				{
					mode = MyAlphaMode.DitherOut;
					int num2 = ~internalArray[i].TransitionTime;
					if (num2 < internalArray[i].Current)
					{
						CompleteAndRemove(ref internalArray[i], i);
						continue;
					}
					value = 1f - (float)internalArray[i].Current / (float)num2;
				}
				internalArray[i].Proxy.SetAlpha(mode, value);
			}
		}

		public static void Unload()
		{
			ProxyInfo[] internalArray = m_transitionInfo.GetInternalArray();
<<<<<<< HEAD
			foreach (int item in m_proxyIndex.Values.ToList())
=======
			foreach (int item in Enumerable.ToList<int>((IEnumerable<int>)m_proxyIndex.Values))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				CompleteAndRemove(ref internalArray[item], item);
			}
			m_transitionInfo.Clear();
			m_proxyIndex.Clear();
		}

		private static MyAlphaTransitionDirection GetDirection(int time)
		{
			if (time <= 0)
			{
				return MyAlphaTransitionDirection.FadeOut;
			}
			return MyAlphaTransitionDirection.FadeIn;
		}

		private static float GetTransitionRate(ref ProxyInfo info)
		{
			if (info.TransitionTime > 0)
			{
				return (float)info.Current / (float)info.TransitionTime;
			}
			return (float)info.Current / (float)(~info.TransitionTime);
		}

		/// <summary>
		/// Fire the complete callback on the proxy, but do not set the alpha.
		/// </summary>
		/// <param name="info"></param>
		private static void CompleteOnly(ref ProxyInfo info)
		{
			if (info.TransitionTime > 0)
			{
				info.Proxy.TransitionComplete(MyAlphaTransitionDirection.FadeIn, info.FadeFinishedCallback);
			}
			else
			{
				info.Proxy.TransitionComplete(MyAlphaTransitionDirection.FadeOut, info.FadeFinishedCallback);
			}
		}

		private static void Complete(ref ProxyInfo info)
		{
			if (info.TransitionTime > 0)
			{
				info.Proxy.SetAlpha(MyAlphaMode.DitherIn, 1f);
				info.Proxy.TransitionComplete(MyAlphaTransitionDirection.FadeIn, info.FadeFinishedCallback);
			}
			else
			{
				info.Proxy.SetAlpha(MyAlphaMode.DitherOut, 0f);
				info.Proxy.TransitionComplete(MyAlphaTransitionDirection.FadeOut, info.FadeFinishedCallback);
			}
		}

		private static void CompleteAndRemove(ref ProxyInfo info, int index)
		{
			MyAlphaTransitionDirection nextDir = info.NextDir;
			IMyAlphaTransitionProxy proxy = info.Proxy;
			short transitionTime = Math.Abs(info.TransitionTime);
			m_proxyIndex.Remove(info.Proxy);
			Complete(ref info);
			m_transitionInfo.Free(index);
			if (nextDir != MyAlphaTransitionDirection.None)
			{
				Add(proxy, transitionTime, nextDir, info.FadeFinishedCallback);
			}
		}

		public static void Clone(MyVoxelCellComponent source, MyVoxelCellComponent dest)
		{
			if (m_proxyIndex.TryGetValue(source, out var value))
			{
				ProxyInfo proxyInfo = m_transitionInfo[value];
				value = m_transitionInfo.Allocate();
				m_proxyIndex[dest] = value;
				proxyInfo.Proxy = dest;
				m_transitionInfo[value] = proxyInfo;
				dest.TransitionStart(GetDirection(proxyInfo.TransitionTime));
			}
		}
	}
}
