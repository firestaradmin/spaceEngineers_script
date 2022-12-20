using System;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_Weather", "DisplayName_TSS_Weather")]
	internal class MyTSSWeather : MyTSSCommon
	{
		public static float ASPECT_RATIO = 3f;

		public static float DECORATION_RATIO = 0.25f;

		public static float TEXT_RATIO = 0.25f;

		private Vector2 m_innerSize;

		private Vector2 m_decorationSize;

		private float m_firstLine;

		private float m_secondLine;

		private StringBuilder m_sb = new StringBuilder();

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update1000;

		public MyTSSWeather(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_innerSize = new Vector2(ASPECT_RATIO, 1f);
			MyTextSurfaceScriptBase.FitRect(surface.SurfaceSize, ref m_innerSize);
			m_decorationSize = new Vector2(0.012f * m_innerSize.X, DECORATION_RATIO * m_innerSize.Y);
			m_sb.Clear();
			m_sb.Append(string.Concat(MyTexts.Get(MySpaceTexts.Weather), ":"));
			Vector2 vector = MyGuiManager.MeasureStringRaw("Monospace", m_sb, 1f);
			float val = TEXT_RATIO * m_innerSize.Y / vector.Y;
			m_fontScale = Math.Min(m_innerSize.X * 0.72f / vector.X, val);
			m_firstLine = m_halfSize.Y - m_decorationSize.Y * 0.55f;
			m_secondLine = m_halfSize.Y + m_decorationSize.Y * 0.55f;
		}

		public override void Run()
		{
			base.Run();
<<<<<<< HEAD
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				AddBackground(frame, new Color(m_backgroundColor, 0.66f));
				if (m_block == null)
				{
					return;
				}
				m_block.GetPosition();
				MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(m_block.GetPosition(), out var weatherEffect);
				string value = "Clear";
				if (weatherEffect != null)
				{
					foreach (MyWeatherEffectDefinition weatherDefinition in MyDefinitionManager.Static.GetWeatherDefinitions())
					{
						string weather = weatherEffect.Weather;
						MyStringHash subtypeId = weatherDefinition.Id.SubtypeId;
						if (weather == subtypeId.ToString())
						{
							value = weatherDefinition.DisplayNameText;
						}
					}
				}
				m_sb.Clear();
				m_sb.Append(string.Concat(MyTexts.Get(MySpaceTexts.Weather), ":"));
				Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
				MySprite mySprite = default(MySprite);
				mySprite.Position = new Vector2(m_halfSize.X, m_firstLine - vector.Y * 0.5f);
				mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
				mySprite.Type = SpriteType.TEXT;
				mySprite.FontId = m_fontId;
				mySprite.Alignment = TextAlignment.CENTER;
				mySprite.Color = m_foregroundColor;
				mySprite.RotationOrScale = m_fontScale;
				mySprite.Data = m_sb.ToString();
				MySprite sprite = mySprite;
				frame.Add(sprite);
				m_sb.Clear();
				m_sb.Append(value);
				vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
				mySprite = default(MySprite);
				mySprite.Position = new Vector2(m_halfSize.X, m_secondLine - vector.Y * 0.5f);
				mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
				mySprite.Type = SpriteType.TEXT;
				mySprite.FontId = m_fontId;
				mySprite.Alignment = TextAlignment.CENTER;
				mySprite.Color = m_foregroundColor;
				mySprite.RotationOrScale = m_fontScale;
				mySprite.Data = m_sb.ToString();
				MySprite sprite2 = mySprite;
				frame.Add(sprite2);
				float scale = m_innerSize.Y / 256f * 0.9f;
				float offsetX = (m_size.X - m_innerSize.X) / 2f;
				AddBrackets(frame, new Vector2(64f, 256f), scale, offsetX);
			}
=======
			using MySpriteDrawFrame frame = m_surface.DrawFrame();
			AddBackground(frame, new Color(m_backgroundColor, 0.66f));
			if (m_block == null)
			{
				return;
			}
			m_block.GetPosition();
			MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(m_block.GetPosition(), out var weatherEffect);
			string value = "Clear";
			if (weatherEffect != null)
			{
				foreach (MyWeatherEffectDefinition weatherDefinition in MyDefinitionManager.Static.GetWeatherDefinitions())
				{
					string weather = weatherEffect.Weather;
					MyStringHash subtypeId = weatherDefinition.Id.SubtypeId;
					if (weather == subtypeId.ToString())
					{
						value = weatherDefinition.DisplayNameText;
					}
				}
			}
			m_sb.Clear();
			m_sb.Append(string.Concat(MyTexts.Get(MySpaceTexts.Weather), ":"));
			Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
			MySprite mySprite = default(MySprite);
			mySprite.Position = new Vector2(m_halfSize.X, m_firstLine - vector.Y * 0.5f);
			mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
			mySprite.Type = SpriteType.TEXT;
			mySprite.FontId = m_fontId;
			mySprite.Alignment = TextAlignment.CENTER;
			mySprite.Color = m_foregroundColor;
			mySprite.RotationOrScale = m_fontScale;
			mySprite.Data = m_sb.ToString();
			MySprite sprite = mySprite;
			frame.Add(sprite);
			m_sb.Clear();
			m_sb.Append(value);
			vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
			mySprite = default(MySprite);
			mySprite.Position = new Vector2(m_halfSize.X, m_secondLine - vector.Y * 0.5f);
			mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
			mySprite.Type = SpriteType.TEXT;
			mySprite.FontId = m_fontId;
			mySprite.Alignment = TextAlignment.CENTER;
			mySprite.Color = m_foregroundColor;
			mySprite.RotationOrScale = m_fontScale;
			mySprite.Data = m_sb.ToString();
			MySprite sprite2 = mySprite;
			frame.Add(sprite2);
			float scale = m_innerSize.Y / 256f * 0.9f;
			float offsetX = (m_size.X - m_innerSize.X) / 2f;
			AddBrackets(frame, new Vector2(64f, 256f), scale, offsetX);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
