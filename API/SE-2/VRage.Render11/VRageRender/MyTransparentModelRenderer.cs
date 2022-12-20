using System;
using System.Collections.Generic;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal class MyTransparentModelRenderer
	{
		public static readonly List<MyRenderCullResultFlat> Renderables = new List<MyRenderCullResultFlat>();

		private static readonly List<ulong> m_sortedKeys = new List<ulong>();

		private static readonly Dictionary<ulong, List<MyRenderableProxy>> m_batches = new Dictionary<ulong, List<MyRenderableProxy>>();

		private static readonly List<Tuple<MyRenderCullResultFlat, float>> m_squaredDistances = new List<Tuple<MyRenderCullResultFlat, float>>();

		internal static void Render(MyRenderContext rc, Func<MyRenderCullResultFlat, double, bool> handleTransparentDepth)
		{
			if (Renderables.Count == 0)
			{
				m_squaredDistances.Clear();
				return;
			}
			m_squaredDistances.Clear();
			m_sortedKeys.Clear();
			foreach (MyRenderCullResultFlat renderable in Renderables)
			{
				float num = renderable.RenderProxy.CommonObjectData.LocalMatrixTranslation.LengthSquared();
				if (handleTransparentDepth(renderable, num))
				{
					m_squaredDistances.Add(Tuple.Create(renderable, num));
				}
				if (!m_batches.TryGetValue(renderable.SortKey, out var value))
				{
					value = new List<MyRenderableProxy>();
					m_batches.Add(renderable.SortKey, value);
				}
				value.Add(renderable.RenderProxy);
				m_sortedKeys.Add(renderable.SortKey);
			}
			m_sortedKeys.Sort();
			MyTransparentModelPass instance = MyTransparentModelPass.Instance;
			instance.SetDeferred(rc);
			instance.ViewProjection = MyRender11.Environment.Matrices.ViewProjectionAt0;
			instance.Viewport = new MyViewport(MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			instance.Begin();
			instance.Locals.BindConstantBuffersBatched = false;
			foreach (ulong sortedKey in m_sortedKeys)
			{
				List<MyRenderableProxy> list = m_batches[sortedKey];
				foreach (MyRenderableProxy item in list)
				{
					instance.RecordCommands(item, null, 0);
				}
				list.Clear();
			}
			instance.End();
			instance.Init();
			m_squaredDistances.Sort(DistanceComparer);
			Renderables.Clear();
		}

		/// <summary>Render depth and normals of windows to the specified target</summary>
		/// <returns>True if transparent model to be rendered found</returns>
<<<<<<< HEAD
		/// <param name="rc"></param>
=======
		/// <param name="stencil"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <param name="depthStencil"></param>
		/// <param name="gbuffer1"></param>
		/// <param name="squaredDistanceMin">Squared distance internal minor</param>
		/// <param name="squaredDistanceMax"></param>
		internal static bool RenderDepthOnly(MyRenderContext rc, IDepthStencil depthStencil, IRtvBindable gbuffer1, float squaredDistanceMin, float squaredDistanceMax)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < m_squaredDistances.Count; i++)
			{
				double num3 = m_squaredDistances[i].Item2;
				if (num3 >= (double)squaredDistanceMax)
				{
					break;
				}
				if (num3 < (double)squaredDistanceMin)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
			if (num2 == 0)
			{
				return false;
			}
			MyTransparentModelPass instance = MyTransparentModelPass.Instance;
			instance.SetDeferred(rc);
			instance.ViewProjection = MyRender11.Environment.Matrices.ViewProjectionAt0;
			instance.Viewport = new MyViewport(MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			instance.BeginDepthOnly();
			rc.SetRtv(depthStencil.Dsv, gbuffer1);
			for (int j = num; j < num2; j++)
			{
				instance.RecordCommandsDepthOnly(m_squaredDistances[j].Item1.RenderProxy);
			}
			instance.End();
			return true;
		}

		private static int DistanceComparer(Tuple<MyRenderCullResultFlat, float> x, Tuple<MyRenderCullResultFlat, float> y)
		{
			double num = x.Item2;
			double num2 = y.Item2;
			if (num > num2)
			{
				return 1;
			}
			if (num == num2)
			{
				return 0;
			}
			return -1;
		}
	}
}
