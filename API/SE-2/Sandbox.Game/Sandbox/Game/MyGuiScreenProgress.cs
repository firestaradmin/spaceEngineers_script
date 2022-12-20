using System;
using System.Text;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
	public class MyGuiScreenProgress : MyGuiScreenProgressBase
	{
		public StringBuilder Text
		{
			get
			{
				return m_progressTextLabel.TextToDraw;
			}
			set
			{
				m_progressTextLabel.TextToDraw = value;
			}
		}

		public event Action Tick;

		public MyGuiScreenProgress(StringBuilder text, MyStringId? cancelText = null, bool isTopMostScreen = true, bool enableBackgroundFade = true)
			: base(MySpaceTexts.Blank, cancelText, isTopMostScreen, enableBackgroundFade)
		{
			Text = new StringBuilder(text.ToString());
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_rotatingWheel.MultipleSpinningWheels = MyPerGameSettings.GUI.MultipleSpinningWheels;
		}

		protected override void ProgressStart()
		{
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenProgress";
		}

		public override bool Update(bool hasFocus)
		{
			Action tick = this.Tick;
			if (tick != null && !base.Cancelled)
			{
				tick();
			}
			return base.Update(hasFocus);
		}

		public override bool Draw()
		{
			if (base.SkipTransition)
			{
				base.RotatingWheel.ManualRotationUpdate = false;
				Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
				MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background.dds", safeFullscreenRectangle, new Color(new Vector4(1f, 1f, 1f, 1f)), ignoreBounds: true, waitTillLoaded: true);
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Screens\\main_menu_overlay.dds", safeFullscreenRectangle, new Color(new Vector4(1f, 1f, 1f, 1f)), ignoreBounds: true, waitTillLoaded: true);
			}
			return base.Draw();
		}

		public void DrawPaused()
		{
			m_transitionAlpha = 1f;
			base.SkipTransition = true;
			try
			{
				MyRenderProxy.PauseTimer(pause: false);
				MyRenderProxy.AfterUpdate(null, gate: false);
				MyRenderProxy.BeforeUpdate();
				MyGuiSandbox.Draw();
				MyRenderProxy.AfterUpdate(null, gate: false);
				MyRenderProxy.BeforeUpdate();
			}
			catch
			{
			}
		}
	}
}
