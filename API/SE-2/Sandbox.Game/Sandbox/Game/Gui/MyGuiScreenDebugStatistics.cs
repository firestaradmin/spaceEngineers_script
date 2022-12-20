using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenDebugStatistics : MyGuiScreenDebugBase
	{
		private readonly struct Tab
		{
			public readonly StringBuilder Name;

			public readonly StringBuilder NameUpper;

			public readonly Action Draw;

			public Tab(string name, Action draw)
			{
				Name = new StringBuilder(name);
				NameUpper = new StringBuilder(name.ToUpper());
				Draw = draw;
			}
		}

		private static StringBuilder m_frameDebugText = new StringBuilder(1024);

		private static StringBuilder m_frameDebugTextRA = new StringBuilder(2048);

		private static List<StringBuilder> m_texts = new List<StringBuilder>(32);

		private static List<StringBuilder> m_rightAlignedtexts = new List<StringBuilder>(32);

		private List<Tab> m_tabs;

		private int m_currentTab;

		private List<MyKeys> m_pressedKeys = new List<MyKeys>(10);

		private static List<StringBuilder> m_statsStrings = new List<StringBuilder>();

		private static int m_stringIndex = 0;

		public static StringBuilder StringBuilderCache
		{
			get
			{
				if (m_stringIndex >= m_statsStrings.Count)
				{
					m_statsStrings.Add(new StringBuilder(1024));
				}
				return m_statsStrings[m_stringIndex++].Clear();
			}
		}

		public MyGuiScreenDebugStatistics()
			: base(new Vector2(0.5f, 0.5f), default(Vector2), null, isTopMostScreen: true)
		{
			m_isTopMostScreen = true;
			m_drawEvenWithoutFocus = true;
			base.CanHaveFocus = false;
			m_canShareInput = false;
			m_tabs = new List<Tab>
			{
				new Tab("Stats", DrawStats),
				new Tab("Keys", DrawKeys),
				new Tab("Sounds", DrawSounds),
				new Tab("Network", DrawNetworkStats)
			};
		}

		public bool Cycle()
		{
			m_currentTab++;
			if (m_currentTab == m_tabs.Count)
			{
				m_currentTab = 0;
				return false;
			}
			return true;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugStatistics";
		}

		public void AddToFrameDebugText(string s)
		{
			m_frameDebugText.AppendLine(s);
		}

		public void AddToFrameDebugText(StringBuilder s)
		{
			m_frameDebugText.AppendStringBuilder(s);
			m_frameDebugText.AppendLine();
		}

		public void AddDebugTextRA(string s)
		{
			m_frameDebugTextRA.Append(s);
			m_frameDebugTextRA.AppendLine();
		}

		public void AddDebugTextRA(StringBuilder s)
		{
			m_frameDebugTextRA.AppendStringBuilder(s);
			m_frameDebugTextRA.AppendLine();
		}

		public void ClearFrameDebugText()
		{
			m_frameDebugText.Clear();
			m_frameDebugTextRA.Clear();
		}

		public Vector2 GetScreenLeftTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			MyGuiManager.GetSafeFullscreenRectangle();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, num));
		}

		public Vector2 GetScreenRightTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float)MyGuiManager.GetSafeFullscreenRectangle().Width - num, num));
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			m_tabs[m_currentTab].Draw();
			return true;
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			float dEBUG_STATISTICS_ROW_DISTANCE = MyGuiConstants.DEBUG_STATISTICS_ROW_DISTANCE;
			float num = MyGuiConstants.DEBUG_STATISTICS_TEXT_SCALE * 0.9f;
			m_texts.Add(StringBuilderCache.Clear());
			m_texts.Add(m_frameDebugText);
			m_rightAlignedtexts.Add(m_frameDebugTextRA);
			Vector2 screenLeftTopPosition = GetScreenLeftTopPosition();
			Vector2 screenRightTopPosition = GetScreenRightTopPosition();
			string font = "Debug";
			float num2 = 0f;
			for (int i = 0; i < m_tabs.Count; i++)
			{
				Tab tab = m_tabs[i];
				bool flag = i == m_currentTab;
				StringBuilder stringBuilder = (flag ? tab.NameUpper : tab.Name);
				MyGuiManager.DrawString(font, stringBuilder.ToString(), screenLeftTopPosition + new Vector2(num2, 0f), num * 1.4f, flag ? Color.White : Color.Yellow);
				num2 += MyGuiManager.MeasureString(font, stringBuilder, num * 1.4f).X + 0.01f;
			}
			screenLeftTopPosition.Y += dEBUG_STATISTICS_ROW_DISTANCE * 1.4f;
			for (int j = 0; j < m_texts.Count; j++)
			{
				MyGuiManager.DrawString(font, m_texts[j].ToString(), screenLeftTopPosition + new Vector2(0f, (float)j * dEBUG_STATISTICS_ROW_DISTANCE), num, Color.Yellow);
			}
			for (int k = 0; k < m_rightAlignedtexts.Count; k++)
			{
				MyGuiManager.DrawString(font, m_rightAlignedtexts[k].ToString(), screenRightTopPosition + new Vector2(-0.3f, (float)k * dEBUG_STATISTICS_ROW_DISTANCE), num, Color.Yellow);
			}
			ClearFrameDebugText();
			m_stringIndex = 0;
			m_texts.Clear();
			m_rightAlignedtexts.Clear();
			return true;
		}

		private static StringBuilder GetFormatedVector3(StringBuilder sb, string before, Vector3D value, string after = "")
		{
			sb.Clear();
			sb.Append(before);
			sb.Append("{");
			sb.ConcatFormat("{0: #,000} ", value.X);
			sb.ConcatFormat("{0: #,000} ", value.Y);
			sb.ConcatFormat("{0: #,000} ", value.Z);
			sb.Append("}");
			sb.Append(after);
			return sb;
		}

		private void DrawStats()
		{
			m_texts.Add(StringBuilderCache.GetFormatedFloat("FPS: ", MyFpsManager.GetFps()));
			m_texts.Add(new StringBuilder("Renderer: ").Append(MyRenderProxy.RendererInterfaceName()));
			if (MySector.MainCamera != null)
			{
				m_texts.Add(GetFormatedVector3(StringBuilderCache, "Camera pos: ", MySector.MainCamera.Position));
			}
			m_texts.Add(MyScreenManager.GetGuiScreensForDebug());
			m_texts.Add(StringBuilderCache.GetFormatedBool("Paused: ", MySandboxGame.IsPaused));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total GAME-PLAY Time: ", TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds)));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Session Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.ElapsedPlayTime));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Foot Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimeOnFoot));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Jetpack Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimeOnJetpack));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Small Ship Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimePilotingSmallShip));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Big Ship Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimePilotingBigShip));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Time: ", TimeSpan.FromMilliseconds(MySandboxGame.TotalTimeInMilliseconds)));
			m_texts.Add(StringBuilderCache.GetFormatedLong("GC.GetTotalMemory: ", GC.GetTotalMemory(forceFullCollection: false), " bytes"));
			m_texts.Add(StringBuilderCache.GetFormatedFloat("Allocated videomemory: ", 0f, " MB"));
		}

		private static void DrawSounds()
		{
			m_texts.Add(StringBuilderCache.GetFormatedInt("Sound Instances Total: ", MyAudio.Static.GetSoundInstancesTotal2D()).Append(" 2d / ").AppendInt32(MyAudio.Static.GetSoundInstancesTotal3D())
				.Append(" 3d"));
			if (MyMusicController.Static != null)
			{
				if (MyMusicController.Static.CategoryPlaying.Equals(MyStringId.NullOrEmpty))
				{
					m_texts.Add(StringBuilderCache.Append("No music playing, last category: " + MyMusicController.Static.CategoryLast.ToString() + ", next track in ").AppendDecimal(Math.Max(0f, MyMusicController.Static.NextMusicTrackIn), 1).Append("s"));
				}
				else
				{
					m_texts.Add(StringBuilderCache.Append("Playing music category: " + MyMusicController.Static.CategoryPlaying.ToString()));
				}
			}
			if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
			{
				m_texts.Add(StringBuilderCache.Append("Current reverb effect: " + (MyAudio.Static.EnableReverb ? MyEntityReverbDetectorComponent.CurrentReverbPreset.ToLower() : "disabled")));
			}
			StringBuilder stringBuilderCache = StringBuilderCache;
			MyAudio.Static.WriteDebugInfo(stringBuilderCache);
			m_texts.Add(stringBuilderCache);
			for (int i = 0; i < 8; i++)
			{
				m_texts.Add(StringBuilderCache.Clear());
			}
			m_texts.Add(new StringBuilder("Last played sounds:"));
			MyAudio.Static.EnumerateLastSounds(delegate(StringBuilder name, bool colored)
			{
				m_texts.Add(name);
			});
		}

		private void DrawKeys()
		{
			MyInput.Static.GetPressedKeys(m_pressedKeys);
			AddPressedKeys("Current keys              : ", m_pressedKeys);
		}

		private void AddPressedKeys(string groupName, List<MyKeys> keys)
		{
			StringBuilder stringBuilderCache = StringBuilderCache;
			stringBuilderCache.Append(groupName);
			for (int i = 0; i < keys.Count; i++)
			{
				if (i > 0)
				{
					stringBuilderCache.Append(", ");
				}
				stringBuilderCache.Append(MyInput.Static.GetKeyName(keys[i]));
			}
			m_texts.Add(stringBuilderCache);
		}

		private void DrawNetworkStats()
		{
			string value = MyGameService.Peer2Peer?.DetailedStats;
			if (!string.IsNullOrEmpty(value))
			{
				m_texts.Add(new StringBuilder(value));
			}
		}

		private StringBuilder GetShadowText(string text, int cascade, int value)
		{
			StringBuilder stringBuilderCache = StringBuilderCache;
			stringBuilderCache.Clear();
			stringBuilderCache.ConcatFormat("{0} (c {1}): ", text, cascade);
			stringBuilderCache.Concat(value);
			return stringBuilderCache;
		}

		private StringBuilder GetLodText(string text, int lod, int value)
		{
			StringBuilder stringBuilderCache = StringBuilderCache;
			stringBuilderCache.Clear();
			stringBuilderCache.ConcatFormat("{0}_LOD{1}: ", text, lod);
			stringBuilderCache.Concat(value);
			return stringBuilderCache;
		}
	}
}
