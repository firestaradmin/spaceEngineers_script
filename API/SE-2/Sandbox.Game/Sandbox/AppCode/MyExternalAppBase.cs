using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.AppCode
{
	public class MyExternalAppBase : IExternalApp
	{
		public static MySandboxGame Static;

		private static bool m_isEditorActive;

		private static bool m_isPresent;

		public static bool IsEditorActive
		{
			get
			{
				return m_isEditorActive;
			}
			set
			{
				m_isEditorActive = value;
			}
		}

		public static bool IsPresent
		{
			get
			{
				return m_isPresent;
			}
			set
			{
				m_isPresent = value;
			}
		}

		public void Run(IntPtr windowHandle, bool customRenderLoop = false, MySandboxGame game = null)
		{
			MyLog.Default = MySandboxGame.Log;
			MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = false;
			MyNullGameService serviceInstance = new MyNullGameService();
			MyServiceManager.Instance.AddService((IMyGameService)serviceInstance);
			MyNullUGCService serviceInstance2 = new MyNullUGCService();
			MyServiceManager.Instance.AddService((IMyUGCService)serviceInstance2);
			if (game == null)
			{
				Static = new MySandboxExternal(this, null, windowHandle);
			}
			else
			{
				Static = game;
			}
			Initialize(Static);
			Static.OnGameLoaded += GameLoaded;
			Static.OnGameExit += GameExit;
			MySession.AfterLoading += MySession_AfterLoading;
			MySession.BeforeLoading += MySession_BeforeLoading;
			Static.Run(customRenderLoop);
			if (!customRenderLoop)
			{
				Dispose();
			}
		}

		public virtual void GameExit()
		{
		}

		public void Dispose()
		{
			Static.Dispose();
			Static = null;
		}

		public void RunSingleFrame()
		{
			Static.RunSingleFrame();
		}

		public void EndLoop()
		{
			Static.EndLoop();
		}

		void IExternalApp.Draw()
		{
			Draw(canDraw: false);
		}

		void IExternalApp.Update()
		{
			Update(canDraw: true);
		}

		void IExternalApp.UpdateMainThread()
		{
			UpdateMainThread();
		}

		public virtual void Initialize(Sandbox.Engine.Platform.Game game)
		{
		}

		public virtual void UpdateMainThread()
		{
		}

		public virtual void Update(bool canDraw)
		{
		}

		public virtual void Draw(bool canDraw)
		{
		}

		public virtual void GameLoaded(object sender, EventArgs e)
		{
			IsEditorActive = true;
			IsPresent = true;
		}

		public virtual void MySession_AfterLoading()
		{
		}

		public virtual void MySession_BeforeLoading()
		{
		}

		public void RemoveEffect(MyParticleEffect effect)
		{
			MyParticlesManager.RemoveParticleEffect(effect);
		}

		public void LoadDefinitions()
		{
			MyDefinitionManager.Static.LoadData(new List<MyObjectBuilder_Checkpoint.ModItem>());
		}

		public float GetStepInSeconds()
		{
			return 0.0166666675f;
		}
	}
}
