using System.Collections.Generic;
using System.Threading;
using ParallelTasks;
using SharpDX.Mathematics.Interop;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Sprites;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Render
{
	internal static class MyOffscreenRenderer
	{
		private class MyData
		{
			public MyRenderMessageRenderOffscreenTexture Message;

			public MySpriteMessageData SpriteMessages;

			public void Dispose()
			{
				Message.Dispose();
				if (SpriteMessages == null)
				{
					return;
				}
				foreach (MySpriteDrawRenderMessage message in SpriteMessages.Messages)
				{
					message.Dispose();
				}
				MyObjectPoolManager.Deallocate(SpriteMessages);
			}
		}

		private class RenderTargetProxy
		{
			private int m_pinCount = 1;

			public IUserGeneratedTexture RenderTarget { get; private set; }

			public RenderTargetProxy(IUserGeneratedTexture renderTarget)
			{
				RenderTarget = renderTarget;
			}

			public void Pin()
			{
				Interlocked.Increment(ref m_pinCount);
			}

			public void Release()
			{
				if (Interlocked.Decrement(ref m_pinCount) == 0)
				{
					MyManagers.FileTextures.DestroyGeneratedTexture(RenderTarget);
					RenderTarget = null;
				}
			}
		}

		private static readonly Dictionary<string, MyData> m_toSubmitDict = new Dictionary<string, MyData>();

		private static readonly List<MyData> m_toSubmitSortedList = new List<MyData>();

		private static readonly List<Future<MyFinishedContext>> m_toConsume = new List<Future<MyFinishedContext>>();

		private static readonly Dictionary<IRtvBindable, RenderTargetProxy> m_renderTargetProxies = new Dictionary<IRtvBindable, RenderTargetProxy>();

		private static void RemoveOffscreenTexture(string name)
		{
			if (m_toSubmitDict.TryGetValue(name, out var value))
			{
				m_toSubmitDict.Remove(name);
				m_toSubmitSortedList.Remove(value);
				value.Dispose();
			}
		}

		public static void Add(MyRenderMessageRenderOffscreenTexture rMessage)
		{
			if (MyRender11.DebugOverrides.OffscreenRendering)
			{
				rMessage.AddRef();
				int num = -1;
				if (m_toSubmitDict.TryGetValue(rMessage.OffscreenTexture, out var value))
				{
					num = m_toSubmitSortedList.IndexOf(value);
					value.Dispose();
				}
				MySpriteMessageData spriteMessages = MyManagers.SpritesManager.AcquireDrawMessages(rMessage.OffscreenTexture);
				value = new MyData
				{
					Message = rMessage,
					SpriteMessages = spriteMessages
				};
				m_toSubmitDict[rMessage.OffscreenTexture] = value;
				if (num == -1)
				{
					m_toSubmitSortedList.Add(value);
				}
				else
				{
					m_toSubmitSortedList[num] = value;
				}
			}
		}

		public static void Render()
		{
			bool flag = false;
			int num = 0;
			while (num < m_toConsume.Count)
			{
				Future<MyFinishedContext> future = m_toConsume[num];
				if (future.IsComplete)
				{
					MyFinishedContext fc = future.GetResult();
<<<<<<< HEAD
					MyRender11.RC.ExecuteContext(ref fc, "Render", 94, "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyOffscreenRenderer.cs");
=======
					MyRender11.RC.ExecuteContext(ref fc, "Render", 94, "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyOffscreenRenderer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_toConsume.RemoveAtFast(num);
					flag = true;
				}
				else
				{
					num++;
				}
			}
			if (flag)
			{
				MyRender11.RC.ClearState();
			}
			while (m_toSubmitSortedList.Count > 0 && m_toConsume.Count < 5)
			{
				MyData myData = m_toSubmitSortedList[0];
				SubmitWork(myData);
				m_toSubmitDict.Remove(myData.Message.OffscreenTexture);
				m_toSubmitSortedList.Remove(myData);
			}
		}

		private static void SubmitWork(MyData data)
		{
			if (!MyManagers.FileTextures.TryGetTexture(data.Message.OffscreenTexture, out IUserGeneratedTexture texture))
			{
				data.Dispose();
				return;
			}
			if (data.SpriteMessages != null)
			{
				foreach (MySpriteDrawRenderMessage message in data.SpriteMessages.Messages)
				{
					var (textureToTouch2, textureToTouch3) = message.GetUsedTextures();
					TouchTexture(textureToTouch2);
					TouchTexture(textureToTouch3);
				}
			}
			if (texture.Rtv == null)
			{
				texture.Reset();
				MyRender11.RC.ClearRtv(texture, default(RawColor4));
			}
			if (!m_renderTargetProxies.TryGetValue(texture, out var renderTarget))
			{
				renderTarget = new RenderTargetProxy(texture);
				m_renderTargetProxies.Add(texture, renderTarget);
			}
			renderTarget.Pin();
			m_toConsume.Add(Parallel.Start(() => RenderWork(data, renderTarget), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "OffscreenRender"), null, WorkPriority.VeryLow));
<<<<<<< HEAD
			void TouchTexture(string textureToTouch)
=======
			static void TouchTexture(string textureToTouch)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (textureToTouch != null)
				{
					MyManagers.Textures.GetTempTexture(textureToTouch, MyFileTextureEnum.GUI, 10000);
				}
			}
		}

		private static MyFinishedContext RenderWork(MyData data, RenderTargetProxy renderTarget)
		{
			MyRenderContext myRenderContext = MyManagers.DeferredRCs.AcquireRC("MyOffscreenRenderer.RenderWork");
			RawColor4? clearColor = null;
			if (data.Message.BackgroundColor.HasValue)
			{
				Vector4 vector = data.Message.BackgroundColor.Value.ToVector4().ToLinearRGB();
				clearColor = new RawColor4(vector.X, vector.Y, vector.Z, vector.W);
			}
			MyRender11.DrawSpritesOffscreen(myRenderContext, renderTarget.RenderTarget, data.SpriteMessages, ref data.Message.AspectRatio, clearColor, MyBlendStateManager.BlendAlphaPremultNoAlphaChannel);
			renderTarget.RenderTarget.SetTextureReady();
			myRenderContext.GenerateMips(renderTarget.RenderTarget);
			renderTarget.Release();
			data.Dispose();
			return myRenderContext.FinishDeferredContext();
		}

		public static bool OnTextureDisposed(IUserGeneratedTexture generatedTexture)
		{
			RemoveOffscreenTexture(generatedTexture.Name);
			if (m_renderTargetProxies.TryGetValue(generatedTexture, out var value))
			{
				m_renderTargetProxies.Remove(generatedTexture);
				value.Release();
				return false;
			}
			return true;
		}
	}
}
