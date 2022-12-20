using ParallelTasks;
using VRage;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Sprites;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal class MyVideoPlayer
	{
		private IVideoPlayer m_player;

		private MySrvWrapper m_texture;

		private Task m_updateTask;

		private MyFinishedContext m_updateFC;

		private bool m_frameReady;

		private int VideoWidth => m_player.VideoWidth;

		private int VideoHeight => m_player.VideoHeight;

		public float Volume
		{
			get
			{
				return m_player?.Volume ?? 1f;
			}
			set
			{
				if (m_player != null)
				{
					m_player.Volume = value;
				}
			}
		}

		public VideoState CurrentState
		{
			get
			{
				if (!Started)
				{
					return VideoState.Playing;
				}
				return m_player?.CurrentState ?? VideoState.Playing;
			}
		}

		public bool Started { get; private set; }

		public void Init(string filename, IVideoPlayer player)
		{
			player.Init(filename);
			m_texture = new MySrvWrapper(player.TextureSrv, new Vector2I(player.VideoWidth, player.VideoHeight));
			m_player = player;
		}

		public void Play()
		{
			if (m_player != null)
			{
				m_player.Play();
				Started = true;
			}
		}

		public void Stop()
		{
			m_player?.Stop();
		}

		public void Dispose()
		{
			if (m_player != null)
			{
				WaitForUpdateTask();
				m_player?.Dispose();
				m_player = null;
			}
		}

		private void WaitForUpdateTask()
		{
			if (m_updateTask.valid)
			{
				m_updateTask.Wait(blocking: true);
				if (m_updateFC.CommandList != null)
				{
<<<<<<< HEAD
					MyRender11.RC.ExecuteContext(ref m_updateFC, "WaitForUpdateTask", 86, "E:\\Repo1\\Sources\\VRage.Render11\\Render\\Utils\\MyVideoPlayer.cs");
=======
					MyRender11.RC.ExecuteContext(ref m_updateFC, "WaitForUpdateTask", 86, "E:\\Repo3\\Sources\\VRage.Render11\\Render\\Utils\\MyVideoPlayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_updateTask = default(Task);
			}
		}

		internal void Draw(MySpritesRenderer renderer, Rectangle rect, Color color, MyVideoRectangleFitMode fitMode, bool ignoreBounds)
		{
			if (m_player == null || !Started || !m_frameReady)
			{
				return;
			}
			Rectangle rectangle = rect;
			Rectangle rectangle2 = new Rectangle(0, 0, VideoWidth, VideoHeight);
			Vector2 textureSize = new Vector2(VideoWidth, VideoHeight);
			float num = textureSize.X / textureSize.Y;
			float num2 = (float)rect.Width / (float)rect.Height;
			if (fitMode == MyVideoRectangleFitMode.AutoFit)
			{
				fitMode = ((!(num > num2)) ? MyVideoRectangleFitMode.FitWidth : MyVideoRectangleFitMode.FitHeight);
			}
			float num3 = 0f;
			switch (fitMode)
			{
			case MyVideoRectangleFitMode.FitWidth:
				num3 = (float)rectangle.Width / textureSize.X;
				rectangle.Height = (int)(num3 * textureSize.Y);
				if (rectangle.Height > rect.Height)
				{
					int num5 = rectangle.Height - rect.Height;
					rectangle.Height = rect.Height;
					num5 = (int)((float)num5 / num3);
					rectangle2.Y -= (int)((float)num5 * 0.5f);
					rectangle2.Height -= num5;
				}
				break;
			case MyVideoRectangleFitMode.FitHeight:
				num3 = (float)rectangle.Height / textureSize.Y;
				rectangle.Width = (int)(num3 * textureSize.X);
				if (rectangle.Width > rect.Width)
				{
					int num4 = rectangle.Width - rect.Width;
					rectangle.Width = rect.Width;
					num4 = (int)((float)num4 / num3);
					rectangle2.X += (int)((float)num4 * 0.5f);
					rectangle2.Width -= num4;
				}
				break;
			}
			rectangle.X = rect.Left + (rect.Width - rectangle.Width) / 2;
			rectangle.Y = rect.Top + (rect.Height - rectangle.Height) / 2;
			renderer.AddSingleSprite(destinationRect: new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), textureSrv: m_texture, textureSize: textureSize, color: color, origin: Vector2.Zero, tangent: Vector2.UnitX, sourceRect: null, ignoreBounds: ignoreBounds, premultipliedAlpha: true);
		}

		public void Update()
		{
			if (m_player == null)
			{
				return;
			}
			if (!Started)
			{
				Play();
			}
			bool flag = false;
			if (m_updateTask.valid)
			{
				if (m_updateTask.IsComplete)
				{
					flag = true;
<<<<<<< HEAD
					MyRender11.RC.ExecuteContext(ref m_updateFC, "Update", 163, "E:\\Repo1\\Sources\\VRage.Render11\\Render\\Utils\\MyVideoPlayer.cs");
=======
					MyRender11.RC.ExecuteContext(ref m_updateFC, "Update", 163, "E:\\Repo3\\Sources\\VRage.Render11\\Render\\Utils\\MyVideoPlayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_frameReady = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				MyRenderContext rc = MyManagers.DeferredRCs.AcquireRC("UpdateVideo");
				m_updateTask = Parallel.Start(delegate
				{
					m_player.Update(rc.DeviceContext);
					m_updateFC = rc.FinishDeferredContext();
				}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "UpdateVideo"), WorkPriority.VeryHigh);
			}
		}
	}
}
