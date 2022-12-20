using System;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyStatControlTargetingProgressBar : MyStatControlCircularProgressBar
	{
		public enum ProgressBarTargetType
		{
			Enemy,
			Neutral,
			Friendly
		}

		private MyGuiSizedTexture? m_filledTexture;

		public Vector4 EnemyFocusSegmentColorMask { get; set; }

		public Vector4 EnemyLockingSegmentColorMask { get; set; }

		public Vector4 NeutralFocusSegmentColorMask { get; set; }

		public Vector4 NeutralLockingSegmentColorMask { get; set; }

		public Vector4 FriendlyFocusSegmentColorMask { get; set; }

		public Vector4 FriendlyLockingSegmentColorMask { get; set; }

		public ProgressBarTargetType TargetType { get; private set; } = ProgressBarTargetType.Neutral;


		public MyStatControlTargetingProgressBar(MyStatControls parent, MyObjectBuilder_GuiTexture texture, MyObjectBuilder_GuiTexture backgroundTexture = null, MyObjectBuilder_GuiTexture firstTexture = null, MyObjectBuilder_GuiTexture lastTexture = null, MyObjectBuilder_GuiTexture filledTexture = null)
			: base(parent, texture, backgroundTexture, firstTexture, lastTexture)
		{
			if (filledTexture != null)
			{
				m_filledTexture = new MyGuiSizedTexture
				{
					Texture = filledTexture.Path,
					SizePx = filledTexture.SizePx
				};
			}
		}

		public void SetTargetType(MyRelationsBetweenPlayerAndBlock relationWithTarget)
		{
			switch (relationWithTarget)
			{
			case MyRelationsBetweenPlayerAndBlock.NoOwnership:
			case MyRelationsBetweenPlayerAndBlock.Neutral:
				TargetType = ProgressBarTargetType.Neutral;
				break;
			case MyRelationsBetweenPlayerAndBlock.Owner:
			case MyRelationsBetweenPlayerAndBlock.FactionShare:
			case MyRelationsBetweenPlayerAndBlock.Friends:
				TargetType = ProgressBarTargetType.Friendly;
				break;
			case MyRelationsBetweenPlayerAndBlock.Enemies:
				TargetType = ProgressBarTargetType.Enemy;
				break;
			}
		}

		public override void Draw(float transitionAlpha)
		{
			MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
			if (MySession.Static.ControlledGrid != null && myCubeBlock != null && (!MySession.Static.ControlledGrid.IsPowered || !myCubeBlock.IsWorking))
			{
				return;
			}
			Vector4 vector = Vector4.One;
			base.BlinkBehavior.UpdateBlink();
			if (base.BlinkBehavior.Blink)
			{
				transitionAlpha = base.BlinkBehavior.CurrentBlinkAlpha;
				if (base.BlinkBehavior.ColorMask.HasValue)
				{
					vector = MyGuiControlBase.ApplyColorMaskModifiers(base.BlinkBehavior.ColorMask.Value, enabled: true, transitionAlpha);
				}
			}
			float num = base.StatCurrent / base.StatMaxValue;
			Rectangle? sourceRectangle = null;
			RectangleF rectangleF = default(RectangleF);
			rectangleF.Position = base.Position + new Vector2(0f - base.SegmentOrigin.X, base.SegmentOrigin.Y);
			rectangleF.Size = base.SegmentSize;
			RectangleF destination = rectangleF;
			float num2 = 1f / (float)base.NumberOfSegments;
			for (int i = 0; i < base.NumberOfSegments; i++)
			{
				Vector2 rightVector = new Vector2((float)Math.Cos(m_textureRotationAngle * (float)i + m_textureRotationOffset), (float)Math.Sin(m_textureRotationAngle * (float)i + m_textureRotationOffset));
				Vector4 vector2 = Color.White;
				switch (TargetType)
				{
				case ProgressBarTargetType.Enemy:
					vector2 = EnemyFocusSegmentColorMask * vector;
					break;
				case ProgressBarTargetType.Neutral:
					vector2 = NeutralFocusSegmentColorMask * vector;
					break;
				case ProgressBarTargetType.Friendly:
					vector2 = FriendlyFocusSegmentColorMask * vector;
					break;
				}
				sourceRectangle = null;
				destination.Position = base.Position + new Vector2(0f - base.SegmentOrigin.X, base.SegmentOrigin.Y);
				destination.Size = base.SegmentSize;
				Vector2 origin = base.Position + base.Size / 2f;
				MyGuiSizedTexture myGuiSizedTexture = m_texture;
				if (i == 0 && m_firstTexture.HasValue)
				{
					myGuiSizedTexture = m_firstTexture.Value;
				}
				else if (i == base.NumberOfSegments - 1 && m_lastTexture.HasValue)
				{
					myGuiSizedTexture = m_lastTexture.Value;
				}
				if (base.ShowEmptySegments)
				{
					MyRenderProxy.DrawSpriteExt(myGuiSizedTexture.Texture, ref destination, sourceRectangle, vector2, ref rightVector, ref origin, ignoreBounds: false, waitTillLoaded: true);
				}
				bool flag = true;
				if ((float)i < num * (float)base.NumberOfSegments)
				{
					if (num / ((float)(i + 1) * num2) < 1f)
					{
						float num3 = num % num2 * (float)base.NumberOfSegments;
						float num4 = 1f - num3;
						sourceRectangle = new Rectangle(0, (int)(num4 * m_texture.SizePx.Y), (int)m_texture.SizePx.X, (int)(num3 * m_texture.SizePx.Y));
						destination.Size = base.SegmentSize * new Vector2(1f, num3);
						destination.Position = base.Position + new Vector2(0f - base.SegmentOrigin.X, base.SegmentOrigin.Y + base.SegmentSize.Y * num4);
					}
					switch (TargetType)
					{
					case ProgressBarTargetType.Enemy:
						vector2 = EnemyLockingSegmentColorMask * vector;
						break;
					case ProgressBarTargetType.Neutral:
						vector2 = NeutralLockingSegmentColorMask * vector;
						break;
					case ProgressBarTargetType.Friendly:
						vector2 = FriendlyLockingSegmentColorMask * vector;
						break;
					}
					flag = false;
				}
				if (!flag)
				{
					if (m_filledTexture.HasValue)
					{
						myGuiSizedTexture = m_filledTexture.Value;
					}
					MyRenderProxy.DrawSpriteExt(myGuiSizedTexture.Texture, ref destination, sourceRectangle, vector2, ref rightVector, ref origin, ignoreBounds: false, waitTillLoaded: true);
				}
			}
		}
	}
}
