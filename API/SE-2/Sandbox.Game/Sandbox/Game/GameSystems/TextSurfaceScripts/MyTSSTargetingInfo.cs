using System.Linq;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_TargetingInfo", "DisplayName_TSS_TargetingInfo")]
	internal class MyTSSTargetingInfo : MyTSSCommon
	{
		private class MyTSSTargetingLayout
		{
			public float AspectRatio;

			public float FontScale;

			public Vector2 ShipNamePos;

			public Vector2 OwnerPos;

			public Vector2 DistancePos;

			public Vector2 RelVelocityPos;

			public Vector2 ApproachVelocityPos;

			public Vector2 MassPos;

			public Vector2 ErrorMessagePos;

			public float ShipNameScale;

			public int MaxNameLength;
		}

		public static float ASPECT_RATIO = 3f;

		public static float DECORATION_RATIO = 0.25f;

		public static float TEXT_RATIO = 0.25f;

		private static MyTSSTargetingLayout DEFAULT_LAYOUT = new MyTSSTargetingLayout
		{
			AspectRatio = 1.3f,
			FontScale = 0.0026f,
			ShipNamePos = new Vector2(0.5f, 0.26f),
			OwnerPos = new Vector2(0.5f, 0.1f),
			DistancePos = new Vector2(0.45f, 0.5f),
			RelVelocityPos = new Vector2(0.45f, 0.62f),
			ApproachVelocityPos = new Vector2(0.45f, 0.74f),
			MassPos = new Vector2(0.45f, 0.86f),
			ErrorMessagePos = new Vector2(0.5f, 0.5f),
			ShipNameScale = 1f,
			MaxNameLength = 19
		};

		private static MyTSSTargetingLayout WIDE_SCREEN_LAYOUT = new MyTSSTargetingLayout
		{
			AspectRatio = 4f,
			FontScale = 0.0011f,
			ShipNamePos = new Vector2(0.5f, 0.15f),
			OwnerPos = new Vector2(0.5f, 0.39f),
			DistancePos = new Vector2(0.05f, 0.75f),
			RelVelocityPos = new Vector2(0.3f, 0.75f),
			ApproachVelocityPos = new Vector2(0.57f, 0.75f),
			MassPos = new Vector2(0.85f, 0.75f),
			ErrorMessagePos = new Vector2(0.5f, 0.5f),
			ShipNameScale = 1.5f,
			MaxNameLength = 19
		};

		private MyTSSTargetingLayout m_layout;

		private float FontScale;

		private Vector2 ShipNamePos;

		private Vector2 OwnerPos;

		private Vector2 DistancePos;

		private Vector2 RelVelocityPos;

		private Vector2 ApproachVelocityPos;

		private Vector2 MassPos;

		public Vector2 ErrorMessagePos;

		private StringBuilder m_sb = new StringBuilder();

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		private Vector2 MultiplyPerComponent(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X * b.X, a.Y * b.Y);
		}

		public MyTSSTargetingInfo(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_layout = DEFAULT_LAYOUT;
			if ((double)(surface.SurfaceSize.X / surface.SurfaceSize.Y) > 2.5)
			{
				m_layout = WIDE_SCREEN_LAYOUT;
			}
			Vector2 rect = new Vector2(m_layout.AspectRatio, 1f);
			MyTextSurfaceScriptBase.FitRect(surface.SurfaceSize, ref rect);
			Vector2 vector = (size - rect) / 2f;
			FontScale = rect.X * m_layout.FontScale;
			ShipNamePos = MultiplyPerComponent(rect, m_layout.ShipNamePos) + vector;
			OwnerPos = MultiplyPerComponent(rect, m_layout.OwnerPos) + vector;
			DistancePos = MultiplyPerComponent(rect, m_layout.DistancePos) + vector;
			RelVelocityPos = MultiplyPerComponent(rect, m_layout.RelVelocityPos) + vector;
			ApproachVelocityPos = MultiplyPerComponent(rect, m_layout.ApproachVelocityPos) + vector;
			MassPos = MultiplyPerComponent(rect, m_layout.MassPos) + vector;
			ErrorMessagePos = MultiplyPerComponent(rect, m_layout.ErrorMessagePos) + vector;
		}

		private void DrawText(MySpriteDrawFrame frame, Vector2 position, StringBuilder text, float scale = 1f, TextAlignment alignment = TextAlignment.CENTER)
		{
			DrawText(frame, position, text.ToString(), scale, alignment);
		}

		private void DrawText(MySpriteDrawFrame frame, Vector2 position, string text, float scale = 1f, TextAlignment alignment = TextAlignment.CENTER)
		{
			MySprite mySprite = default(MySprite);
			mySprite.Position = position;
			mySprite.Type = SpriteType.TEXT;
			mySprite.FontId = m_fontId;
			mySprite.Alignment = alignment;
			mySprite.Color = m_foregroundColor;
			mySprite.RotationOrScale = FontScale * scale;
			mySprite.Data = text;
			MySprite sprite = mySprite;
			frame.Add(sprite);
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				long? num = (base.Block as MyCockpit)?.TargetData.TargetId;
				if (num.HasValue)
				{
					goto IL_009b;
				}
				MyCockpit myCockpit = (base.Block.CubeGrid as MyCubeGrid).MainCockpit as MyCockpit;
				if (myCockpit == null)
				{
					DrawText(frame, ErrorMessagePos, MyTexts.Get(MySpaceTexts.TssTargetingInfo_NoMainCockpit), 0.8f);
					return;
				}
				num = myCockpit.TargetData.TargetId;
				goto IL_009b;
				IL_009b:
				string text = MyTexts.Get(MySpaceTexts.TssTargetingInfo_NoTargetLocked).ToString();
				string text2 = "";
				string text3 = "-";
				string text4 = "-";
				string text5 = "-";
				string text6 = "-";
				MyCubeGrid myCubeGrid = ((!num.HasValue) ? null : (MyEntities.GetEntityById(num.Value) as MyCubeGrid));
				if (myCubeGrid != null)
				{
					text = ((myCubeGrid.DisplayName.Length > m_layout.MaxNameLength) ? myCubeGrid.DisplayName.Substring(0, m_layout.MaxNameLength) : myCubeGrid.DisplayName);
					m_sb.Clear();
					long num2 = myCubeGrid.BigOwners.FirstOrDefault();
					string text7 = MySession.Static.Players.TryGetIdentity(num2)?.Character?.DisplayNameText;
					string text8 = MySession.Static.Factions.TryGetPlayerFaction(num2)?.Tag;
					if (text7 != null)
					{
						if (text8 != null)
						{
							m_sb.Append("[").Append(text8).Append("] ")
								.Append(text7);
						}
						else
						{
							m_sb.Append(text7);
						}
					}
					else if (text8 != null)
					{
						m_sb.Append("[").Append(text8).Append("]");
					}
					else
					{
						m_sb.Append("No faction");
					}
					text2 = m_sb.ToString();
					if (text2.Length > m_layout.MaxNameLength)
					{
						text2 = text2.Substring(0, m_layout.MaxNameLength);
					}
					m_sb.Clear();
					Vector3D vector3D = myCubeGrid.PositionComp.GetPosition() - MyHudMarkerRender.GetDistanceMeasuringMatrix().Translation;
					m_sb.Append(vector3D.Length().ToString("F0")).Append((object)MyTexts.Get(MySpaceTexts.MeasurementUnit_Meter));
					text3 = m_sb.ToString();
					m_sb.Clear();
					if (myCubeGrid.Mass > 0f)
					{
						m_sb.Append(myCubeGrid.Mass.ToString("F0")).Append((object)MyTexts.Get(MySpaceTexts.MeasurementUnit_Kg));
					}
					else
					{
						m_sb.Append((object)MyTexts.Get(MySpaceTexts.TssTargetingInfo_StaticGrid));
					}
					text6 = m_sb.ToString();
					m_sb.Clear();
					MyCubeGrid myCubeGrid2 = base.Block.CubeGrid as MyCubeGrid;
					float num3 = (myCubeGrid.LinearVelocity - myCubeGrid2.LinearVelocity).Length();
					m_sb.Append(num3.ToString("F1")).Append((object)MyTexts.Get(MySpaceTexts.MeasurementUnit_Mps));
					text4 = m_sb.ToString();
					m_sb.Clear();
					Vector3D vector3D2 = myCubeGrid.PositionComp.GetPosition() - myCubeGrid2.PositionComp.GetPosition();
					vector3D2.Normalize();
					double num4 = vector3D2.Dot(myCubeGrid2.LinearVelocity);
					m_sb.Append(num4.ToString("F1")).Append((object)MyTexts.Get(MySpaceTexts.MeasurementUnit_Mps));
					text5 = m_sb.ToString();
				}
				AddBackground(frame, new Color(m_backgroundColor, 0.66f));
				DrawText(frame, ShipNamePos, text, m_layout.ShipNameScale);
				DrawText(frame, OwnerPos, text2);
				DrawText(frame, DistancePos, "d: ", 1f, TextAlignment.RIGHT);
				DrawText(frame, DistancePos, text3, 1f, TextAlignment.LEFT);
				DrawText(frame, RelVelocityPos, "rv: ", 1f, TextAlignment.RIGHT);
				DrawText(frame, RelVelocityPos, text4, 1f, TextAlignment.LEFT);
				DrawText(frame, ApproachVelocityPos, "av: ", 1f, TextAlignment.RIGHT);
				DrawText(frame, ApproachVelocityPos, text5, 1f, TextAlignment.LEFT);
				DrawText(frame, MassPos, "m: ", 1f, TextAlignment.RIGHT);
				DrawText(frame, MassPos, text6, 1f, TextAlignment.LEFT);
			}
		}
	}
}
