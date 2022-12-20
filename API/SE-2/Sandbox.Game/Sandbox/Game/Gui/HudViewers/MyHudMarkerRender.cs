using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Text;
using Sandbox.Definitions;
=======
using System.Text;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Generics;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.HudViewers
{
	public class MyHudMarkerRender : MyHudMarkerRenderBase
	{
		public enum SignalMode
		{
			DefaultMode,
			FullDisplay,
			NoNames,
			Off,
			MaxSignalModes
		}

		public enum MyTargetLockingState
		{
			None,
			Focused,
			Locking,
			Locked,
			LosingLock
		}

		private class PointOfInterest
		{
			public enum PointOfInterestState
			{
				NonDirectional,
				Directional
			}

			public enum PointOfInterestType
			{
				/// <summary>
				/// Used for anything unknown
				/// </summary>
				Unknown,
				/// <summary>
				/// Used for turret targets
				/// </summary>
				Target,
				/// <summary>
				/// Used for grouped POIs
				/// </summary>
				Group,
				/// <summary>
				/// Used for ore
				/// </summary>
				Ore,
				/// <summary>
				/// Used for hacked blocks
				/// </summary>
				Hack,
				/// <summary>
				/// Used for grids outside of grid identification range
				/// </summary>
				UnknownEntity,
				/// <summary>
				/// Used for characters
				/// </summary>
				Character,
				/// <summary>
				/// Used for small entities
				/// </summary>
				SmallEntity,
				/// <summary>
				/// Used for large entities
				/// </summary>
				LargeEntity,
				/// <summary>
				/// Used for static entities (Stations, etc)
				/// </summary>
				StaticEntity,
				/// <summary>
				/// Used for GPS coordinates
				/// </summary>
				GPS,
				/// <summary>
				/// Used for Button Markers
				/// </summary>
				ButtonMarker,
				/// <summary>
				/// Used for GPS objectives
				/// </summary>
				Objective,
				/// <summary>
				/// Used for important scenario goals (different marker and color and spawn mechanism than Objective) (PS it is just pile of hacks)
				/// </summary>
				Scenario,
				ContractGPS,
				/// <summary>
				/// Used for showing a locked target outside FOV
				/// </summary>
				OffscreenTarget
			}

			public const double ClusterAngle = 10.0;

			public const int MaxTextLength = 64;

			public const double ClusterNearDistance = 3500.0;

			public const double ClusterScaleDistance = 20000.0;

			public const double MinimumTargetRange = 2000.0;

			public const double OreDistance = 200.0;

			private const double AngleConversion = Math.PI / 360.0;

			public static readonly Color DefaultColor = new Color(117, 201, 241);

			public Color Color = DefaultColor;

			public List<PointOfInterest> m_group = new List<PointOfInterest>(10);

			private bool m_alwaysVisible;

			public Vector3D WorldPosition { get; private set; }

			public PointOfInterestType POIType { get; private set; }

			public MyRelationsBetweenPlayerAndBlock Relationship { get; private set; }

			public MyEntity Entity { get; private set; }

			public StringBuilder Text { get; private set; }

			public double Distance { get; private set; }

			public double DistanceToCam { get; private set; }

			public string ContainerRemainingTime { get; set; }

			public bool AlwaysVisible
			{
				get
				{
					if (POIType == PointOfInterestType.Ore && Distance < 200.0)
					{
						return true;
					}
					return m_alwaysVisible;
				}
				set
				{
					m_alwaysVisible = value;
				}
			}

			public bool AllowsCluster
			{
				get
				{
					if (AlwaysVisible)
					{
						return false;
					}
					if (POIType == PointOfInterestType.Target)
					{
						return false;
					}
					if (POIType == PointOfInterestType.Ore && Distance < 200.0)
					{
						return false;
					}
					return true;
				}
			}

			public PointOfInterest()
			{
				WorldPosition = Vector3D.Zero;
				POIType = PointOfInterestType.Unknown;
				Relationship = MyRelationsBetweenPlayerAndBlock.Owner;
				Text = new StringBuilder(64, 64);
			}

			public override string ToString()
			{
				return string.Concat(POIType.ToString(), ": ", Text, " (", Distance, ")");
			}

			/// <summary>
			/// Clears out all data and resets the POI for re-use.
			/// </summary>
			public void Reset()
			{
				WorldPosition = Vector3D.Zero;
				POIType = PointOfInterestType.Unknown;
				Relationship = MyRelationsBetweenPlayerAndBlock.Owner;
				Entity = null;
				Text.Clear();
				m_group.Clear();
				Distance = 0.0;
				DistanceToCam = 0.0;
				AlwaysVisible = false;
				ContainerRemainingTime = null;
				Color = DefaultColor;
			}

			/// <summary>
			/// Sets the POI state
			/// </summary>
			/// <param name="position">World position of the POI.</param>
			/// <param name="type">POI Type, grid, ore, gps, etc.</param>
			/// <param name="relationship">Relationship of the local player to this POI</param>
			public void SetState(Vector3D position, PointOfInterestType type, MyRelationsBetweenPlayerAndBlock relationship)
			{
				WorldPosition = position;
				POIType = type;
				Relationship = relationship;
				Distance = (position - GetDistanceMeasuringMatrix().Translation).Length();
				DistanceToCam = (WorldPosition - CameraMatrix.Translation).Length();
			}

			/// <summary>
			/// Stores the entity that goes with this POI
			/// </summary>
			/// <param name="entity">MyEntity that this POI is for</param>
			public void SetEntity(MyEntity entity)
			{
				Entity = entity;
			}

			/// <summary>
			/// Sets the text message for this POI, limited to MaxTextLength characters.
			/// </summary>
			/// <param name="text">The text message to set, limited to MaxTextLength characters.</param>
			public void SetText(StringBuilder text)
			{
				Text.Clear();
				if (text != null)
				{
					Text.AppendSubstring(text, 0, Math.Min(text.Length, 64));
				}
			}

			/// <summary>
			/// Sets the text message for this POI, limited to MaxTextLength characters.
			/// </summary>
			/// <param name="text">The text message to set, limited to MaxTextLength characters.</param>
			public void SetText(string text)
			{
				Text.Clear();
				if (!string.IsNullOrWhiteSpace(text))
				{
					Text.Append(text, 0, Math.Min(text.Length, 64));
				}
			}

			/// <summary>
			/// Adds another POI to this POI, turning this into a POI group.
			/// </summary>
			/// <param name="poi"></param>
			/// <returns>True if POI was added, false if not. Probable cause for failure is that this is not a group POI.</returns>
			public bool AddPOI(PointOfInterest poi)
			{
				if (POIType != PointOfInterestType.Group)
				{
					return false;
				}
				Vector3D worldPosition = WorldPosition;
				worldPosition *= (double)m_group.Count;
				m_group.Add(poi);
				Text.Clear();
				Text.Append(m_group.Count);
				switch (GetGroupRelation())
				{
				case MyRelationsBetweenPlayerAndBlock.Owner:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Own));
					break;
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Friendly));
					break;
				case MyRelationsBetweenPlayerAndBlock.Neutral:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Neutral));
					break;
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Enemy));
					break;
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Mixed));
					break;
				}
				worldPosition += poi.WorldPosition;
				WorldPosition = worldPosition / m_group.Count;
				Distance = (WorldPosition - GetDistanceMeasuringMatrix().Translation).Length();
				DistanceToCam = (WorldPosition - CameraMatrix.Translation).Length();
				if (poi.Relationship > Relationship)
				{
					Relationship = poi.Relationship;
				}
				return true;
			}

			/// <summary>
			/// Checks if some given POI is within a radius matching an angle from the POV of a given location.
			/// </summary>
			/// <param name="poi">POI to check</param>
			/// <param name="cameraPosition">Position from which to check.</param>
			/// <param name="angle">Angle within the POI must fall from the camera position's POV, defaults to ClusterAngle.</param>
			/// <returns>True if it is within the radius, false otherwise.</returns>
			public bool IsPOINearby(PointOfInterest poi, Vector3D cameraPosition, double angle = 10.0)
			{
				Vector3D vector3D = 0.5 * (WorldPosition - poi.WorldPosition);
				double num = vector3D.LengthSquared();
				double num2 = (cameraPosition - (poi.WorldPosition + vector3D)).Length();
				double num3 = Math.Sin(angle * (Math.PI / 360.0)) * num2;
				double num4 = num3 * num3;
				return num <= num4;
			}

			/// <summary>
			/// Retrieves font and colouring information for a relationship.
			/// </summary>
			/// <param name="relationship"></param>
			/// <param name="color"></param>
			/// <param name="fontColor"></param>
			/// <param name="font"></param>
			public void GetColorAndFontForRelationship(MyRelationsBetweenPlayerAndBlock relationship, out Color color, out Color fontColor, out string font)
			{
				color = Color.White;
				fontColor = Color.White;
				font = "White";
				switch (relationship)
				{
				case MyRelationsBetweenPlayerAndBlock.Owner:
					color = new Color(117, 201, 241);
					fontColor = new Color(117, 201, 241);
					font = "Blue";
					break;
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
				case MyRelationsBetweenPlayerAndBlock.Friends:
					color = new Color(101, 178, 90);
					font = "Green";
					break;
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					color = new Color(227, 62, 63);
					font = "Red";
					break;
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
				case MyRelationsBetweenPlayerAndBlock.Neutral:
					break;
				}
			}

<<<<<<< HEAD
			/// <summary>
			/// Returns the POI color and font information.
			/// </summary>
			/// <param name="poiColor">The colour of the POI.</param>
			/// <param name="fontColor">The colour that should be used with this font.</param>
			/// <param name="font">The font to be used for this POI.</param>
			/// <param name="text"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void GetPOIColorAndFontInformation(out Color poiColor, out Color fontColor, out string font, string text = null)
			{
				poiColor = Color.White;
				fontColor = Color.White;
				font = "White";
				switch (POIType)
				{
				default:
					GetColorAndFontForRelationship(Relationship, out poiColor, out fontColor, out font);
					break;
				case PointOfInterestType.Ore:
					poiColor = Color.Khaki;
					font = "White";
					fontColor = Color.Khaki;
					break;
				case PointOfInterestType.Unknown:
					poiColor = Color.White;
					font = "White";
					fontColor = Color.White;
					break;
				case PointOfInterestType.Group:
				{
					bool flag = true;
					PointOfInterestType pointOfInterestType = PointOfInterestType.Unknown;
					if (text != null && text.Contains(MyTexts.Get(MySpaceTexts.Signal_Own).ToString()))
					{
						poiColor = Color;
						fontColor = Color;
						font = "Blue";
						break;
					}
					if (m_group.Count > 0)
					{
						m_group[0].GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
						pointOfInterestType = m_group[0].POIType;
					}
					for (int i = 1; i < m_group.Count; i++)
					{
						if (m_group[i].POIType != pointOfInterestType)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						MyRelationsBetweenPlayerAndBlock groupRelation = GetGroupRelation();
						GetColorAndFontForRelationship(groupRelation, out poiColor, out fontColor, out font);
					}
					break;
				}
				case PointOfInterestType.GPS:
				case PointOfInterestType.ContractGPS:
					poiColor = Color;
					fontColor = Color;
					font = "Blue";
					break;
				case PointOfInterestType.Objective:
					poiColor = Color * 1.3f;
					fontColor = Color * 1.3f;
					font = "Blue";
					break;
				case PointOfInterestType.Scenario:
					poiColor = Color.DarkOrange;
					fontColor = Color.DarkOrange;
					font = "White";
					break;
				}
			}

			private MyRelationsBetweenPlayerAndBlock GetGroupRelation()
			{
				if (m_group == null || m_group.Count == 0)
				{
					return MyRelationsBetweenPlayerAndBlock.NoOwnership;
				}
				MyRelationsBetweenPlayerAndBlock myRelationsBetweenPlayerAndBlock = m_group[0].Relationship;
				for (int i = 1; i < m_group.Count; i++)
				{
					if (m_group[i].Relationship == myRelationsBetweenPlayerAndBlock)
					{
						continue;
					}
					if (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Owner && m_group[i].Relationship == MyRelationsBetweenPlayerAndBlock.FactionShare)
					{
						myRelationsBetweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.FactionShare;
						continue;
					}
					if (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.FactionShare && m_group[i].Relationship == MyRelationsBetweenPlayerAndBlock.Owner)
					{
						myRelationsBetweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.FactionShare;
						continue;
					}
					return MyRelationsBetweenPlayerAndBlock.NoOwnership;
				}
				if (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.NoOwnership)
				{
					return MyRelationsBetweenPlayerAndBlock.Neutral;
				}
				return myRelationsBetweenPlayerAndBlock;
			}

			/// <summary>
			/// Draws this POI
			/// </summary>
			/// <param name="renderer">MyHudMarkerRender instance that performs the rendering.</param>
			/// <param name="alphaMultiplierMarker"></param>
			/// <param name="alphaMultiplierText"></param>
			/// <param name="scale"></param>
			/// <param name="drawBox"></param>
			public void Draw(MyHudMarkerRender renderer, float alphaMultiplierMarker = 1f, float alphaMultiplierText = 1f, float scale = 1f, bool drawBox = true)
			{
				Vector2 projectedPoint2D = Vector2.Zero;
				bool isBehind = false;
				if (!TryComputeScreenPoint(WorldPosition, out projectedPoint2D, out isBehind))
				{
					return;
				}
				Vector2 vector = new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
				Vector2 hudSize = MyGuiManager.GetHudSize();
				Vector2 hudSizeHalf = MyGuiManager.GetHudSizeHalf();
				float uIScale = MyGuiManager.UIScale;
				float num = vector.Y / 1080f;
				projectedPoint2D *= hudSize;
				Color poiColor = Color.White;
				Color fontColor = Color.White;
				string font = "White";
				GetPOIColorAndFontInformation(out poiColor, out fontColor, out font, Text.ToString());
				Vector2 vector2 = projectedPoint2D - hudSizeHalf;
				Vector3D vector3D = Vector3D.Transform(WorldPosition, MySector.MainCamera.ViewMatrix);
				Vector2 vector3 = hudSize * (1f - uIScale) / 2f + 0.04f;
				if (projectedPoint2D.X < vector3.X || projectedPoint2D.X > hudSize.X - vector3.X || projectedPoint2D.Y < vector3.Y || projectedPoint2D.Y > hudSize.Y - vector3.Y || vector3D.Z > 0.0)
				{
					if (POIType == PointOfInterestType.Target)
					{
						return;
					}
					Vector2 vector4 = Vector2.Normalize(vector2);
					projectedPoint2D = hudSizeHalf + hudSizeHalf * vector4 * 0.77f * uIScale;
					vector2 = projectedPoint2D - hudSizeHalf;
					if (vector2.LengthSquared() > 9.99999944E-11f)
					{
						vector2.Normalize();
					}
					else
					{
						vector2 = new Vector2(1f, 0f);
					}
					float num2 = 0.0053336f;
					num2 /= num;
					num2 /= num;
					renderer.AddTexturedQuad(MyHudTexturesEnum.DirectionIndicator, projectedPoint2D, vector2, poiColor, num2, num2);
					projectedPoint2D -= vector2 * 0.006667f * 2f;
					if (POIType == PointOfInterestType.OffscreenTarget)
					{
						MyStatControlTargetingProgressBar offscreenTargetCircle = MyHud.TargetingMarkers.OffscreenTargetCircle;
						if (offscreenTargetCircle != null)
						{
							float num3 = vector.X / MyGuiManager.GetHudSize().X;
							float num4 = vector.Y / MyGuiManager.GetHudSize().Y;
							Vector2 vector5 = projectedPoint2D;
							if (MyVideoSettingsManager.IsTripleHead())
							{
								vector5.X += 1f;
							}
							Vector2 vector6 = offscreenTargetCircle.Size / 2f;
							vector5.X *= num3;
							vector5.Y *= num4;
							offscreenTargetCircle.Position = vector5 - vector6;
						}
					}
				}
				else
				{
<<<<<<< HEAD
					if (POIType == PointOfInterestType.OffscreenTarget)
					{
						MyStatControlTargetingProgressBar offscreenTargetCircle2 = MyHud.TargetingMarkers.OffscreenTargetCircle;
						if (offscreenTargetCircle2 != null)
						{
							offscreenTargetCircle2.Position = MyTargetIndicatorRender.CalculateCircularBarPosition(new Vector2(2f, 2f), offscreenTargetCircle2);
						}
					}
					float num5 = scale * 0.006667f / num;
					num5 /= num;
					if (POIType == PointOfInterestType.Target)
					{
						renderer.AddTexturedQuad(MyHudTexturesEnum.TargetTurret, projectedPoint2D, -Vector2.UnitY, Color.White, num5, num5);
=======
					float num3 = scale * 0.006667f / num;
					num3 /= num;
					if (POIType == PointOfInterestType.Target)
					{
						renderer.AddTexturedQuad(MyHudTexturesEnum.TargetTurret, projectedPoint2D, -Vector2.UnitY, Color.White, num3, num3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						return;
					}
					if (drawBox)
					{
<<<<<<< HEAD
						renderer.AddTexturedQuad(MyHudTexturesEnum.Target_neutral, projectedPoint2D, -Vector2.UnitY, poiColor, num5, num5);
					}
				}
				float num6 = 0.03f;
				float num7 = 0.07f;
				float num8 = 0.15f;
				int num9 = 0;
				float num10 = 1f;
				float num11 = 1f;
				float num12 = vector2.Length();
				if (num12 <= num6)
				{
					num10 = 1f;
					num11 = 1f;
					num9 = 0;
				}
				else if (num12 > num6 && num12 < num7)
				{
					float num13 = num8 - num6;
					num10 = 1f - (num12 - num6) / num13;
					num10 *= num10;
					num13 = num7 - num6;
					num11 = 1f - (num12 - num6) / num13;
					num11 *= num11;
					num9 = 1;
				}
				else if (num12 >= num7 && num12 < num8)
				{
					float num14 = num8 - num6;
					num10 = 1f - (num12 - num6) / num14;
					num10 *= num10;
					num14 = num8 - num7;
					num11 = 1f - (num12 - num7) / num14;
					num11 *= num11;
					num9 = 2;
				}
				else
				{
					num10 = 0f;
					num11 = 0f;
					num9 = 2;
				}
				float value = (num12 - 0.2f) / 0.5f;
				value = MathHelper.Clamp(value, 0f, 1f);
				num10 = MyMath.Clamp(num10, 0f, 1f);
				if (m_disableFading || SignalDisplayMode == SignalMode.FullDisplay || AlwaysVisible)
				{
					num10 = 1f;
					num11 = 1f;
					value = 1f;
					num9 = 0;
				}
				Vector2 vector7 = new Vector2(0f, scale * num * 24f / (float)MyGuiManager.GetFullscreenRectangle().Width);
				if ((SignalDisplayMode != SignalMode.NoNames || POIType == PointOfInterestType.ButtonMarker || m_disableFading || AlwaysVisible) && num10 > float.Epsilon && Text.Length > 0)
=======
						renderer.AddTexturedQuad(MyHudTexturesEnum.Target_neutral, projectedPoint2D, -Vector2.UnitY, poiColor, num3, num3);
					}
				}
				float num4 = 0.03f;
				float num5 = 0.07f;
				float num6 = 0.15f;
				int num7 = 0;
				float num8 = 1f;
				float num9 = 1f;
				float num10 = vector2.Length();
				if (num10 <= num4)
				{
					num8 = 1f;
					num9 = 1f;
					num7 = 0;
				}
				else if (num10 > num4 && num10 < num5)
				{
					float num11 = num6 - num4;
					num8 = 1f - (num10 - num4) / num11;
					num8 *= num8;
					num11 = num5 - num4;
					num9 = 1f - (num10 - num4) / num11;
					num9 *= num9;
					num7 = 1;
				}
				else if (num10 >= num5 && num10 < num6)
				{
					float num12 = num6 - num4;
					num8 = 1f - (num10 - num4) / num12;
					num8 *= num8;
					num12 = num6 - num5;
					num9 = 1f - (num10 - num5) / num12;
					num9 *= num9;
					num7 = 2;
				}
				else
				{
					num8 = 0f;
					num9 = 0f;
					num7 = 2;
				}
				float value = (num10 - 0.2f) / 0.5f;
				value = MathHelper.Clamp(value, 0f, 1f);
				num8 = MyMath.Clamp(num8, 0f, 1f);
				if (m_disableFading || SignalDisplayMode == SignalMode.FullDisplay || AlwaysVisible)
				{
					num8 = 1f;
					num9 = 1f;
					value = 1f;
					num7 = 0;
				}
				Vector2 vector5 = new Vector2(0f, scale * num * 24f / (float)MyGuiManager.GetFullscreenRectangle().Width);
				if ((SignalDisplayMode != SignalMode.NoNames || POIType == PointOfInterestType.ButtonMarker || m_disableFading || AlwaysVisible) && num8 > float.Epsilon && Text.Length > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyHudText myHudText = renderer.m_hudScreen.AllocateText();
					if (myHudText != null)
					{
<<<<<<< HEAD
						fontColor.A = (byte)(255f * alphaMultiplierText * num10);
						myHudText.Start(font, projectedPoint2D - vector7, fontColor, 0.7f / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
=======
						fontColor.A = (byte)(255f * alphaMultiplierText * num8);
						myHudText.Start(font, projectedPoint2D - vector5, fontColor, 0.7f / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						myHudText.Append(Text);
					}
				}
				MyHudText myHudText2 = null;
				if (POIType != PointOfInterestType.Group && POIType != PointOfInterestType.OffscreenTarget)
				{
					byte a = poiColor.A;
					poiColor.A = (byte)(255f * alphaMultiplierMarker * value);
					DrawIcon(renderer, POIType, Relationship, projectedPoint2D, poiColor, scale);
					poiColor.A = a;
					myHudText2 = renderer.m_hudScreen.AllocateText();
					if (myHudText2 != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						AppendDistance(stringBuilder, Distance);
						fontColor.A = (byte)(alphaMultiplierText * 255f);
<<<<<<< HEAD
						myHudText2.Start(font, projectedPoint2D + vector7 * (0.7f + 0.3f * num10), fontColor, (0.5f + 0.2f * num10) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
=======
						myHudText2.Start(font, projectedPoint2D + vector5 * (0.7f + 0.3f * num8), fontColor, (0.5f + 0.2f * num8) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						myHudText2.Append(stringBuilder);
					}
					if (!string.IsNullOrEmpty(ContainerRemainingTime))
					{
						MyHudText myHudText3 = renderer.m_hudScreen.AllocateText();
<<<<<<< HEAD
						myHudText3.Start(font, projectedPoint2D + vector7 * (1.6f + 0.3f * num10), fontColor, (0.5f + 0.2f * num10) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
=======
						myHudText3.Start(font, projectedPoint2D + vector5 * (1.6f + 0.3f * num8), fontColor, (0.5f + 0.2f * num8) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						myHudText3.Append(ContainerRemainingTime);
					}
					return;
				}
				Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> significantGroupPOIs = GetSignificantGroupPOIs();
				Vector2[] array = new Vector2[5]
				{
					new Vector2(-6f, -4f),
					new Vector2(6f, -4f),
					new Vector2(-6f, 4f),
					new Vector2(6f, 4f),
					new Vector2(0f, 12f)
				};
				Vector2[] array2 = new Vector2[5]
				{
					new Vector2(16f, -4f),
					new Vector2(16f, 4f),
					new Vector2(16f, 12f),
					new Vector2(16f, 20f),
					new Vector2(16f, 28f)
				};
				for (int i = 0; i < array.Length; i++)
				{
<<<<<<< HEAD
					float num15 = ((num9 < 2) ? 1f : num11);
					float y = array[i].Y;
					array[i].X = (array[i].X + 22f * num15) / (float)MyGuiManager.GetFullscreenRectangle().Width;
=======
					float num13 = ((num7 < 2) ? 1f : num9);
					float y = array[i].Y;
					array[i].X = (array[i].X + 22f * num13) / (float)MyGuiManager.GetFullscreenRectangle().Width;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					array[i].Y = y / 1080f / num;
					if (MyVideoSettingsManager.IsTripleHead())
					{
						array[i].X /= 0.33f;
					}
					if (array[i].Y <= float.Epsilon)
					{
						array[i].Y = y / 1080f;
					}
					y = array2[i].Y;
					array2[i].X = array2[i].X / (float)MyGuiManager.GetFullscreenRectangle().Width / num;
					array2[i].Y = y / 1080f / num;
					if (MyVideoSettingsManager.IsTripleHead())
					{
						array2[i].X /= 0.33f;
					}
					if (array2[i].Y <= float.Epsilon)
					{
						array2[i].Y = y / 1080f;
					}
				}
<<<<<<< HEAD
				int num16 = 0;
=======
				int num14 = 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (significantGroupPOIs.Count > 1)
				{
					MyRelationsBetweenPlayerAndBlock[] array3 = new MyRelationsBetweenPlayerAndBlock[4]
					{
						MyRelationsBetweenPlayerAndBlock.Owner,
						MyRelationsBetweenPlayerAndBlock.FactionShare,
						MyRelationsBetweenPlayerAndBlock.Neutral,
						MyRelationsBetweenPlayerAndBlock.Enemies
					};
					foreach (MyRelationsBetweenPlayerAndBlock myRelationsBetweenPlayerAndBlock in array3)
					{
						if (!significantGroupPOIs.ContainsKey(myRelationsBetweenPlayerAndBlock))
						{
							continue;
						}
						List<PointOfInterest> list = significantGroupPOIs[myRelationsBetweenPlayerAndBlock];
						if (list.Count == 0)
						{
							continue;
						}
						PointOfInterest pointOfInterest = list[0];
						if (pointOfInterest == null)
						{
							continue;
						}
						if (pointOfInterest.POIType == PointOfInterestType.ContractGPS)
						{
							pointOfInterest.GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
						}
						else
						{
							GetColorAndFontForRelationship(myRelationsBetweenPlayerAndBlock, out poiColor, out fontColor, out font);
						}
<<<<<<< HEAD
						float amount = ((num9 == 0) ? 1f : num11);
						if (num9 >= 2)
						{
							amount = 0f;
						}
						Vector2 vector8 = Vector2.Lerp(array[num16], array2[num16], amount);
						string iconForRelationship = GetIconForRelationship(myRelationsBetweenPlayerAndBlock);
						poiColor.A = (byte)(alphaMultiplierMarker * (float)(int)poiColor.A);
						DrawIcon(renderer, iconForRelationship, projectedPoint2D + vector8, poiColor, 0.75f / num);
=======
						float amount = ((num7 == 0) ? 1f : num9);
						if (num7 >= 2)
						{
							amount = 0f;
						}
						Vector2 vector6 = Vector2.Lerp(array[num14], array2[num14], amount);
						string iconForRelationship = GetIconForRelationship(myRelationsBetweenPlayerAndBlock);
						poiColor.A = (byte)(alphaMultiplierMarker * (float)(int)poiColor.A);
						DrawIcon(renderer, iconForRelationship, projectedPoint2D + vector6, poiColor, 0.75f / num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (IsPoiAtHighAlert(pointOfInterest))
						{
							Color white = Color.White;
							white.A = (byte)(alphaMultiplierMarker * 255f);
<<<<<<< HEAD
							DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", projectedPoint2D + vector8, white, 0.75f / num);
=======
							DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", projectedPoint2D + vector6, white, 0.75f / num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						if ((SignalDisplayMode != SignalMode.NoNames || m_disableFading || AlwaysVisible) && pointOfInterest.Text.Length > 0)
						{
							MyHudText myHudText4 = renderer.m_hudScreen.AllocateText();
							if (myHudText4 != null)
							{
<<<<<<< HEAD
								float num17 = 1f;
								if (num9 == 1)
								{
									num17 = num11;
								}
								else if (num9 > 1)
								{
									num17 = 0f;
								}
								fontColor.A = (byte)(255f * alphaMultiplierText * num17);
								Vector2 vector9 = new Vector2(8f / (float)MyGuiManager.GetFullscreenRectangle().Width, 0f);
								vector9.X /= num;
								myHudText4.Start(font, projectedPoint2D + vector8 + vector9, fontColor, 0.55f / num, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
								myHudText4.Append(pointOfInterest.Text);
							}
						}
						num16++;
=======
								float num15 = 1f;
								if (num7 == 1)
								{
									num15 = num9;
								}
								else if (num7 > 1)
								{
									num15 = 0f;
								}
								fontColor.A = (byte)(255f * alphaMultiplierText * num15);
								Vector2 vector7 = new Vector2(8f / (float)MyGuiManager.GetFullscreenRectangle().Width, 0f);
								vector7.X /= num;
								myHudText4.Start(font, projectedPoint2D + vector6 + vector7, fontColor, 0.55f / num, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
								myHudText4.Append(pointOfInterest.Text);
							}
						}
						num14++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				else
				{
					foreach (KeyValuePair<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> item in significantGroupPOIs)
					{
						MyRelationsBetweenPlayerAndBlock key = item.Key;
						if (!significantGroupPOIs.ContainsKey(key))
<<<<<<< HEAD
						{
							continue;
						}
						List<PointOfInterest> value2 = item.Value;
						for (int k = 0; k < 4 && k < value2.Count; k++)
						{
=======
						{
							continue;
						}
						List<PointOfInterest> value2 = item.Value;
						for (int k = 0; k < 4 && k < value2.Count; k++)
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							PointOfInterest pointOfInterest2 = value2[k];
							if (pointOfInterest2 == null)
							{
								continue;
							}
							if (pointOfInterest2.POIType == PointOfInterestType.Scenario || pointOfInterest2.POIType == PointOfInterestType.ContractGPS || pointOfInterest2.POIType == PointOfInterestType.Ore)
							{
								pointOfInterest2.GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
							}
							else
<<<<<<< HEAD
							{
								GetColorAndFontForRelationship(key, out poiColor, out fontColor, out font);
								fontColor = pointOfInterest2.Color;
							}
							float amount2 = ((num9 == 0) ? 1f : num11);
							if (num9 >= 2)
							{
								amount2 = 0f;
							}
							Vector2 vector10 = Vector2.Lerp(array[num16], array2[num16], amount2);
							string centerIconSprite = ((pointOfInterest2.POIType != PointOfInterestType.Scenario) ? GetIconForRelationship(key) : "Textures\\HUD\\marker_scenario.dds");
							poiColor.A = (byte)(alphaMultiplierMarker * (float)(int)poiColor.A);
							DrawIcon(renderer, centerIconSprite, projectedPoint2D + vector10, poiColor, 0.75f / num);
							if (ShouldDrawHighAlertMark(pointOfInterest2))
							{
								Color white2 = Color.White;
								white2.A = (byte)(alphaMultiplierMarker * 255f);
								DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", projectedPoint2D + vector10, white2, 0.75f / num);
=======
							{
								GetColorAndFontForRelationship(key, out poiColor, out fontColor, out font);
								fontColor = pointOfInterest2.Color;
							}
							float amount2 = ((num7 == 0) ? 1f : num9);
							if (num7 >= 2)
							{
								amount2 = 0f;
							}
							Vector2 vector8 = Vector2.Lerp(array[num14], array2[num14], amount2);
							string centerIconSprite = ((pointOfInterest2.POIType != PointOfInterestType.Scenario) ? GetIconForRelationship(key) : "Textures\\HUD\\marker_scenario.dds");
							poiColor.A = (byte)(alphaMultiplierMarker * (float)(int)poiColor.A);
							DrawIcon(renderer, centerIconSprite, projectedPoint2D + vector8, poiColor, 0.75f / num);
							if (ShouldDrawHighAlertMark(pointOfInterest2))
							{
								Color white2 = Color.White;
								white2.A = (byte)(alphaMultiplierMarker * 255f);
								DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", projectedPoint2D + vector8, white2, 0.75f / num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
							if ((SignalDisplayMode != SignalMode.NoNames || m_disableFading || AlwaysVisible) && pointOfInterest2.Text.Length > 0)
							{
								MyHudText myHudText5 = renderer.m_hudScreen.AllocateText();
								if (myHudText5 != null)
								{
<<<<<<< HEAD
									float num18 = 1f;
									if (num9 == 1)
									{
										num18 = num11;
									}
									else if (num9 > 1)
									{
										num18 = 0f;
									}
									fontColor.A = (byte)(255f * alphaMultiplierText * num18);
									Vector2 vector11 = new Vector2(8f / (float)MyGuiManager.GetFullscreenRectangle().Width, 0f);
									vector11.X /= num;
									myHudText5.Start(font, projectedPoint2D + vector10 + vector11, fontColor, 0.55f / num, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
									myHudText5.Append(pointOfInterest2.Text);
								}
							}
							num16++;
=======
									float num16 = 1f;
									if (num7 == 1)
									{
										num16 = num9;
									}
									else if (num7 > 1)
									{
										num16 = 0f;
									}
									fontColor.A = (byte)(255f * alphaMultiplierText * num16);
									Vector2 vector9 = new Vector2(8f / (float)MyGuiManager.GetFullscreenRectangle().Width, 0f);
									vector9.X /= num;
									myHudText5.Start(font, projectedPoint2D + vector8 + vector9, fontColor, 0.55f / num, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
									myHudText5.Append(pointOfInterest2.Text);
								}
							}
							num14++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
<<<<<<< HEAD
				float amount3 = ((num9 == 0) ? 1f : num11);
				if (num9 >= 2)
				{
					amount3 = 0f;
				}
				Vector2 vector12 = Vector2.Lerp(array[4], array2[num16], amount3);
				Vector2 vector13 = Vector2.Lerp(Vector2.Zero, new Vector2(0.0222222228f / num, 0.00370370364f / num), amount3);
				if (POIType != PointOfInterestType.OffscreenTarget)
				{
					myHudText2 = renderer.m_hudScreen.AllocateText();
					if (myHudText2 != null)
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						AppendDistance(stringBuilder2, Distance);
						myHudText2.Start(font, projectedPoint2D + vector12 + vector13, Color, (0.5f + 0.2f * num10) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						myHudText2.Append(stringBuilder2);
					}
=======
				float amount3 = ((num7 == 0) ? 1f : num9);
				if (num7 >= 2)
				{
					amount3 = 0f;
				}
				Vector2 vector10 = Vector2.Lerp(array[4], array2[num14], amount3);
				Vector2 vector11 = Vector2.Lerp(Vector2.Zero, new Vector2(0.0222222228f / num, 0.00370370364f / num), amount3);
				myHudText2 = renderer.m_hudScreen.AllocateText();
				if (myHudText2 != null)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					AppendDistance(stringBuilder2, Distance);
					myHudText2.Start(font, projectedPoint2D + vector10 + vector11, Color, (0.5f + 0.2f * num8) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					myHudText2.Append(stringBuilder2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}

			/// <summary>
			/// Returns the most significant POI for each relationship type within the group.
			/// </summary>
			/// <returns></returns>
			private Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> GetSignificantGroupPOIs()
			{
				Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> dictionary = new Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>>();
				if (m_group == null || m_group.Count == 0)
				{
					return dictionary;
				}
				bool flag = true;
				MyRelationsBetweenPlayerAndBlock relationship = m_group[0].Relationship;
				for (int i = 1; i < m_group.Count; i++)
				{
					if (m_group[i].Relationship != relationship)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					m_group.Sort(ComparePointOfInterest);
					dictionary[relationship] = new List<PointOfInterest>();
					for (int num = m_group.Count - 1; num >= 0; num--)
					{
						dictionary[relationship].Add(m_group[num]);
						if (dictionary[relationship].Count >= 4)
						{
							break;
						}
					}
				}
				else
				{
					for (int j = 0; j < m_group.Count; j++)
					{
						PointOfInterest pointOfInterest = m_group[j];
						relationship = pointOfInterest.Relationship;
						if (relationship == MyRelationsBetweenPlayerAndBlock.NoOwnership)
						{
							relationship = MyRelationsBetweenPlayerAndBlock.Neutral;
						}
						if (dictionary.ContainsKey(relationship))
						{
							if (ComparePointOfInterest(pointOfInterest, dictionary[relationship][0]) > 0)
							{
								dictionary[relationship].Clear();
								dictionary[relationship].Add(pointOfInterest);
							}
						}
						else
						{
							dictionary[relationship] = new List<PointOfInterest>();
							dictionary[relationship].Add(pointOfInterest);
						}
					}
				}
				return dictionary;
			}

			private bool IsRelationHostile(MyRelationsBetweenPlayerAndBlock relationshipA, MyRelationsBetweenPlayerAndBlock relationshipB)
			{
				if (relationshipA == MyRelationsBetweenPlayerAndBlock.Owner || relationshipA == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					return relationshipB == MyRelationsBetweenPlayerAndBlock.Enemies;
				}
				if (relationshipB == MyRelationsBetweenPlayerAndBlock.Owner || relationshipB == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					return relationshipA == MyRelationsBetweenPlayerAndBlock.Enemies;
				}
				return false;
			}

			/// <summary>
			/// Checks if this POI has any kind of hostile activity nearby, by comparing it against the other elements in the group.
			/// </summary>
			private bool IsPoiAtHighAlert(PointOfInterest poi)
			{
				if (poi.Relationship == MyRelationsBetweenPlayerAndBlock.Neutral)
				{
					return false;
				}
				if (poi.POIType == PointOfInterestType.Scenario)
				{
					return true;
				}
				foreach (PointOfInterest item in m_group)
				{
					if (IsRelationHostile(poi.Relationship, item.Relationship) && ((Vector3)(item.WorldPosition - poi.WorldPosition)).LengthSquared() < 1000000f)
					{
						return true;
					}
				}
				return false;
			}

			private bool ShouldDrawHighAlertMark(PointOfInterest poi)
			{
				if (poi.POIType != PointOfInterestType.Scenario)
				{
					return IsPoiAtHighAlert(poi);
				}
				return false;
			}

			/// <summary>
			/// Checks if the current POI is a grid.
			/// </summary>
			/// <returns>True if it's a grid, false otherwise.</returns>
			private bool IsGrid()
			{
				if (POIType != PointOfInterestType.SmallEntity && POIType != PointOfInterestType.LargeEntity)
				{
					return POIType == PointOfInterestType.StaticEntity;
				}
				return true;
			}

			/// <summary>
			/// Draws an icon for the POI
			/// </summary>
			private static void DrawIcon(MyHudMarkerRender renderer, PointOfInterestType poiType, MyRelationsBetweenPlayerAndBlock relationship, Vector2 screenPosition, Color markerColor, float sizeScale = 1f)
			{
				MyHudTexturesEnum myHudTexturesEnum = MyHudTexturesEnum.corner;
				string empty = string.Empty;
				Vector2 vector = new Vector2(12f, 12f);
				switch (poiType)
				{
				default:
					return;
				case PointOfInterestType.OffscreenTarget:
					return;
				case PointOfInterestType.Hack:
					myHudTexturesEnum = MyHudTexturesEnum.hit_confirmation;
					break;
				case PointOfInterestType.Target:
					myHudTexturesEnum = MyHudTexturesEnum.TargetTurret;
					break;
				case PointOfInterestType.Ore:
					myHudTexturesEnum = MyHudTexturesEnum.HudOre;
					markerColor = Color.Khaki;
					break;
				case PointOfInterestType.Unknown:
				case PointOfInterestType.UnknownEntity:
				case PointOfInterestType.Character:
				case PointOfInterestType.SmallEntity:
				case PointOfInterestType.LargeEntity:
				case PointOfInterestType.StaticEntity:
				{
					string iconForRelationship = GetIconForRelationship(relationship);
					DrawIcon(renderer, iconForRelationship, screenPosition, markerColor, sizeScale);
					return;
				}
				case PointOfInterestType.Scenario:
				{
					string centerIconSprite2 = "Textures\\HUD\\marker_scenario.dds";
					DrawIcon(renderer, centerIconSprite2, screenPosition, markerColor, sizeScale);
					return;
				}
				case PointOfInterestType.GPS:
				case PointOfInterestType.Objective:
				{
					string centerIconSprite = "Textures\\HUD\\marker_gps.dds";
					DrawIcon(renderer, centerIconSprite, screenPosition, markerColor, sizeScale);
					return;
				}
				}
				if (!string.IsNullOrWhiteSpace(empty))
				{
					vector *= sizeScale;
					renderer.AddTexturedQuad(empty, screenPosition, -Vector2.UnitY, markerColor, vector.X, vector.Y);
				}
				else
				{
					float num = 0.0053336f * sizeScale;
					renderer.AddTexturedQuad(myHudTexturesEnum, screenPosition, -Vector2.UnitY, markerColor, num, num);
				}
			}

			/// <summary>
			/// Returns the icon path for the marker images for each relationship.
			/// </summary>
			public static string GetIconForRelationship(MyRelationsBetweenPlayerAndBlock relationship)
			{
				string result = string.Empty;
				switch (relationship)
				{
				case MyRelationsBetweenPlayerAndBlock.Owner:
					result = "Textures\\HUD\\marker_self.dds";
					break;
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
				case MyRelationsBetweenPlayerAndBlock.Friends:
					result = "Textures\\HUD\\marker_friendly.dds";
					break;
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
				case MyRelationsBetweenPlayerAndBlock.Neutral:
					result = "Textures\\HUD\\marker_neutral.dds";
					break;
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					result = "Textures\\HUD\\marker_enemy.dds";
					break;
				}
				return result;
			}

			/// <summary>
			/// Draws a texture based icon
			/// </summary>
			private static void DrawIcon(MyHudMarkerRender renderer, string centerIconSprite, Vector2 screenPosition, Color markerColor, float sizeScale = 1f)
			{
				Vector2 vector = new Vector2(8f, 8f);
				vector *= sizeScale;
				renderer.AddTexturedQuad(centerIconSprite, screenPosition, -Vector2.UnitY, markerColor, vector.X, vector.Y);
			}

<<<<<<< HEAD
			/// <summary>
			/// Compares two POIs according to some pre-defined metrics
			/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			private int ComparePointOfInterest(PointOfInterest poiA, PointOfInterest poiB)
			{
				bool flag = IsPoiAtHighAlert(poiA);
				bool value = IsPoiAtHighAlert(poiB);
				int num = flag.CompareTo(value);
				if (num != 0)
				{
					return num;
				}
				if (poiA.POIType >= PointOfInterestType.UnknownEntity && poiB.POIType >= PointOfInterestType.UnknownEntity)
				{
					int num2 = poiA.POIType.CompareTo(poiB.POIType);
					if (num2 != 0)
					{
						return num2;
					}
				}
				if (poiA.IsGrid() && poiB.IsGrid())
				{
					MyCubeBlock myCubeBlock = poiA.Entity as MyCubeBlock;
					MyCubeBlock myCubeBlock2 = poiB.Entity as MyCubeBlock;
					if (myCubeBlock != null && myCubeBlock2 != null)
					{
						int num3 = myCubeBlock.CubeGrid.BlocksCount.CompareTo(myCubeBlock2.CubeGrid.BlocksCount);
						if (num3 != 0)
						{
							return num3;
						}
					}
				}
				return poiB.Distance.CompareTo(poiA.Distance);
			}
		}

		private class MyPlayerIndicator
		{
			private enum MyPlayerIndicatorStatus
			{
				Visible,
				Fading
			}

			private MyPlayerIndicatorStatus m_status;

			private Color m_relationIndicatorColor_original = Color.White;

			private Color m_relationIndicatorColor_toDraw = Color.White;

			private MyRelationsBetweenPlayers m_targetRelation = MyRelationsBetweenPlayers.Neutral;

			public MyEntity TargetEntity { get; }

<<<<<<< HEAD
			/// <summary>
			/// Is Always Visible is used for friendly players (in the same faction)
			/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public bool IsAlwaysVisible { get; set; }

			public MyPlayerIndicator(MyEntity targetEntity, MyRelationsBetweenPlayers relation, bool isAlwaysVisible)
			{
				TargetEntity = targetEntity;
				m_targetRelation = relation;
				m_relationIndicatorColor_original = GetRelationIndicatorColor(relation);
				m_relationIndicatorColor_toDraw = GetRelationIndicatorColor(relation);
				m_status = MyPlayerIndicatorStatus.Visible;
				IsAlwaysVisible = isAlwaysVisible;
			}

			private static Color GetRelationIndicatorColor(MyRelationsBetweenPlayers relation)
			{
<<<<<<< HEAD
				switch (relation)
				{
				case MyRelationsBetweenPlayers.Self:
					return new Color(61, 180, 255, 255);
				case MyRelationsBetweenPlayers.Allies:
					return new Color(106, 248, 0, 255);
				case MyRelationsBetweenPlayers.Neutral:
					return new Color(255, 210, 0, 255);
				default:
					return new Color(255, 70, 61, 255);
				}
=======
				return relation switch
				{
					MyRelationsBetweenPlayers.Self => new Color(61, 180, 255, 255), 
					MyRelationsBetweenPlayers.Allies => new Color(106, 248, 0, 255), 
					MyRelationsBetweenPlayers.Neutral => new Color(255, 210, 0, 255), 
					_ => new Color(255, 70, 61, 255), 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			private byte FadeColorChannel(byte original, byte current, float secToFade = 2f)
			{
				float num = (float)(int)original / (secToFade * 60f);
				float num2 = (float)(int)current - num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				return (byte)num2;
			}

			public void ResetFadeout(MyRelationsBetweenPlayers relation)
			{
				m_status = MyPlayerIndicatorStatus.Visible;
				if (m_targetRelation != relation)
				{
					m_targetRelation = relation;
					m_relationIndicatorColor_original = GetRelationIndicatorColor(relation);
				}
				m_relationIndicatorColor_toDraw.A = m_relationIndicatorColor_original.A;
				m_relationIndicatorColor_toDraw.R = m_relationIndicatorColor_original.R;
				m_relationIndicatorColor_toDraw.G = m_relationIndicatorColor_original.G;
				m_relationIndicatorColor_toDraw.B = m_relationIndicatorColor_original.B;
			}

			public bool Draw()
			{
				if (MyHud.HudState == 0 || MyGuiScreenHudSpace.Static.State != MyGuiScreenState.OPENED)
				{
					return true;
				}
				MyCharacter myCharacter;
				if ((myCharacter = TargetEntity as MyCharacter) == null || myCharacter.IsDead)
				{
					return false;
				}
				if (myCharacter.RadioBroadcaster != null && myCharacter.RadioBroadcaster.Enabled)
				{
					return true;
				}
				float num = 0.02f;
				float num2 = -0.02f;
				float scale = 1f;
				if (!TryComputeScreenPoint(TargetEntity.PositionComp.GetPosition() + (TargetEntity.PositionComp.LocalAABB.Height + num) * TargetEntity.PositionComp.WorldMatrixRef.Up, out var projectedPoint2D, out var isBehind))
				{
					return false;
				}
				if (!isBehind)
				{
					Vector2 position = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref projectedPoint2D);
					position.Y += num2;
					string text = myCharacter.CustomNameWithFaction.ToString();
					if (text == "")
					{
						text = myCharacter.UpdateCustomNameWithFaction().ToString();
					}
					Vector2 textSize = MyGuiManager.MeasureString("Blue", text, MyGuiSandbox.GetDefaultTextScaleWithLanguage());
					MyGuiTextShadows.DrawShadow(ref position, ref textSize, null, (float)(int)m_relationIndicatorColor_toDraw.A / 255f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, ignoreBounds: true);
					MyGuiManager.DrawString("Blue", text, position, scale, m_relationIndicatorColor_toDraw, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, float.PositiveInfinity, ignoreBounds: true);
				}
				if (m_status == MyPlayerIndicatorStatus.Fading)
				{
					if (m_relationIndicatorColor_toDraw.A <= 0)
					{
						return false;
					}
					float secToFade = 2f;
					m_relationIndicatorColor_toDraw.A = FadeColorChannel(m_relationIndicatorColor_original.A, m_relationIndicatorColor_toDraw.A, secToFade);
					m_relationIndicatorColor_toDraw.R = FadeColorChannel(m_relationIndicatorColor_original.R, m_relationIndicatorColor_toDraw.R, secToFade);
					m_relationIndicatorColor_toDraw.G = FadeColorChannel(m_relationIndicatorColor_original.G, m_relationIndicatorColor_toDraw.G, secToFade);
					m_relationIndicatorColor_toDraw.B = FadeColorChannel(m_relationIndicatorColor_original.B, m_relationIndicatorColor_toDraw.B, secToFade);
				}
				m_status = MyPlayerIndicatorStatus.Fading;
				if (IsAlwaysVisible)
				{
					ResetFadeout(MyRelationsBetweenPlayers.Allies);
				}
				return true;
			}
		}

<<<<<<< HEAD
		private class MyTargetIndicatorRender
		{
			public class MyTargetInfo
			{
				private MyTargetLockingState m_state;

				public float ProgressPercent;

				public MyCubeGrid Grid;

				public MyTargetLockingState State
				{
					get
					{
						return m_state;
					}
					set
					{
						m_state = value;
					}
				}

				public bool IsSet => m_state != MyTargetLockingState.None;

				public void Set(MyCubeGrid grid, MyTargetLockingState state, float progress)
				{
					Grid = grid;
					if (state != 0 && Grid.DisplayName != null)
					{
						_ = Grid.Physics.CenterOfMassWorld;
						m_state = state;
						ProgressPercent = progress;
					}
					else
					{
						Unset();
					}
				}

				public void Unset()
				{
					m_state = MyTargetLockingState.None;
				}
			}

			public readonly Color LOCKED_COLOR = Color.Red;

			private Vector2 TITLE_OFFSET = new Vector2(0.02f, 0f);

			private Vector2 DETAILS_LINE_OFFSET_VERTICAL = new Vector2(0f, 0.016f);

			private static Color ENEMY_COLOR = Color.FromNonPremultiplied(new Vector4(0.92f, 0.21f, 0.2f, 1f));

			private static Color NEUTRAL_COLOR = Color.FromNonPremultiplied(new Vector4(0.86f, 0.86f, 0.86f, 1f));

			private static Color FRIENDLY_COLOR = Color.FromNonPremultiplied(new Vector4(0.18f, 0.67f, 0.2f, 1f));

			private MyTargetLeadRender m_targetLeadRender;

			private MyHudNotification m_targetLockNotification;

			private StringBuilder m_targetRangeBuilder = new StringBuilder();

			public MyTargetInfo TargetInfo { get; set; } = new MyTargetInfo();


			public bool HasTargetLock
			{
				get
				{
					if (TargetInfo != null && TargetInfo.IsSet)
					{
						if (TargetInfo.State != MyTargetLockingState.Locked)
						{
							return TargetInfo.State == MyTargetLockingState.LosingLock;
						}
						return true;
					}
					return false;
				}
			}

			public long? TargetGridId
			{
				get
				{
					if (TargetInfo.State != 0)
					{
						return TargetInfo.Grid.EntityId;
					}
					return null;
				}
			}

			public void SetTarget(MyCubeGrid grid, MyTargetLockingState state, float lockingProgressPercent)
			{
				TargetInfo.Set(grid, state, lockingProgressPercent);
				if ((grid != null && m_targetLeadRender == null) || (m_targetLeadRender != null && this != m_targetLeadRender.TargetIndicator))
				{
					m_targetLeadRender = new MyTargetLeadRender(this);
				}
				if (grid == null)
				{
					m_targetLeadRender = null;
				}
			}

			public void ClearTarget()
			{
				TargetInfo.Unset();
				m_targetLeadRender = null;
			}

			public static Color GetTargetingColor(MyStatControlTargetingProgressBar.ProgressBarTargetType targetType)
			{
				switch (targetType)
				{
				case MyStatControlTargetingProgressBar.ProgressBarTargetType.Enemy:
					return ENEMY_COLOR;
				case MyStatControlTargetingProgressBar.ProgressBarTargetType.Neutral:
					return NEUTRAL_COLOR;
				case MyStatControlTargetingProgressBar.ProgressBarTargetType.Friendly:
					return FRIENDLY_COLOR;
				default:
					return NEUTRAL_COLOR;
				}
			}

			private static Vector2 AdjustVectorFromCenter(Vector2 halfSize, MyGuiDrawAlignEnum align)
			{
				switch (align)
				{
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
					halfSize.X *= -1f;
					break;
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
					halfSize.X = 0f;
					break;
				}
				switch (align)
				{
				case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
					halfSize.Y *= -1f;
					break;
				case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
					halfSize.Y = 0f;
					break;
				}
				return halfSize;
			}

			private static void DrawText(string text, MyFontEnum font, float scale, Vector2 position, Color color, MyGuiDrawAlignEnum align)
			{
				float scale2 = MyGuiSandbox.GetDefaultTextScaleWithLanguage() * scale;
				Vector2 textSize = MyGuiManager.MeasureString(font, text, scale2);
				Vector2 position2 = position + AdjustVectorFromCenter(textSize / 2f, align);
				MyGuiTextShadows.DrawShadow(ref position2, ref textSize, null, 1f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, ignoreBounds: true);
				MyGuiManager.DrawString(font, text, position2, scale2, color, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, float.PositiveInfinity, ignoreBounds: true);
			}

			public void Draw(MyHudMarkerRender renderer)
			{
				if (!TargetInfo.IsSet || TargetInfo?.Grid?.Physics == null)
				{
					if (m_targetLockNotification != null)
					{
						MyHud.Notifications.Remove(m_targetLockNotification);
						m_targetLockNotification = null;
					}
					MyGuiScreenHudSpace.Static.RemoveOffscreenTargetMarker();
					return;
				}
				MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
				if (MySession.Static.ControlledGrid != null && myCubeBlock != null && (!MySession.Static.ControlledGrid.IsPowered || !myCubeBlock.IsWorking))
				{
					if (m_targetLockNotification != null)
					{
						MyHud.Notifications.Remove(m_targetLockNotification);
						m_targetLockNotification = null;
					}
					MyGuiScreenHudSpace.Static.RemoveOffscreenTargetMarker();
					if (MyHud.TargetingMarkers.TargetingCircle != null)
					{
						MyHud.TargetingMarkers.TargetingCircle.State = MyStatControlState.Invisible;
					}
					return;
				}
				Vector3D lockingPosition = MyTargetingHelper.Instance.GetLockingPosition(TargetInfo.Grid);
				if (!TryComputeScreenPoint(lockingPosition, out var projectedPoint2D, out var isBehind))
				{
					return;
				}
				MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(TargetInfo.Grid.BigOwners.FirstOrDefault(), MySession.Static.LocalHumanPlayer.Identity.IdentityId, MyOwnershipShareModeEnum.Faction);
				MyStatControlTargetingProgressBar offscreenTargetCircle = MyHud.TargetingMarkers.OffscreenTargetCircle;
				if (TargetInfo.State == MyTargetLockingState.Locked || TargetInfo.State == MyTargetLockingState.Locking || TargetInfo.State == MyTargetLockingState.LosingLock)
				{
					MyGuiScreenHudSpace.Static.AddOffscreenTargetMarker(lockingPosition, relationPlayerBlock);
				}
				else
				{
					if (offscreenTargetCircle != null)
					{
						offscreenTargetCircle.Position = CalculateCircularBarPosition(new Vector2(2f, 2f), offscreenTargetCircle);
					}
					MyGuiScreenHudSpace.Static.RemoveOffscreenTargetMarker();
				}
				MyStatControlTargetingProgressBar targetingCircle = MyHud.TargetingMarkers.TargetingCircle;
				if (targetingCircle != null)
				{
					targetingCircle.State = MyStatControlState.Visible;
					targetingCircle.Position = CalculateCircularBarPosition(new Vector2(2f, 2f), targetingCircle);
				}
				if (!isBehind && targetingCircle != null && targetingCircle.State == MyStatControlState.Visible)
				{
					targetingCircle.Position = CalculateCircularBarPosition(projectedPoint2D, targetingCircle);
					targetingCircle.SetTargetType(relationPlayerBlock);
					offscreenTargetCircle?.SetTargetType(relationPlayerBlock);
					UpdateTargetLockNotification();
					DrawTargetRange(targetingCircle);
					m_targetLeadRender?.Draw(renderer, projectedPoint2D, TargetInfo, targetingCircle.TargetType);
				}
			}

			private void DrawTargetRange(MyStatControlTargetingProgressBar circle)
			{
				Vector3D vector3D = TargetInfo.Grid.PositionComp.GetPosition() - GetDistanceMeasuringMatrix().Translation;
				m_targetRangeBuilder.Clear();
				AppendDistance(m_targetRangeBuilder, vector3D.Length());
				Vector2 normalizedCoordinateFromScreenCoordinate = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(circle.Position + new Vector2(circle.Size.X / 2f, circle.Size.Y + 10f));
				DrawText(m_targetRangeBuilder.ToString(), "White", 0.8f, normalizedCoordinateFromScreenCoordinate, GetTargetingColor(circle.TargetType), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}

			private void UpdateTargetLockNotification()
			{
				MyStringId myStringId = ((TargetInfo.State != MyTargetLockingState.Focused) ? MySpaceTexts.NotificationHintUnlockTarget : MySpaceTexts.NotificationHintLockTarget);
				if (m_targetLockNotification == null || !(m_targetLockNotification.Text == myStringId))
				{
					if (m_targetLockNotification != null)
					{
						MyHud.Notifications.Remove(m_targetLockNotification);
						m_targetLockNotification = null;
					}
					m_targetLockNotification = new MyHudNotification(myStringId, 0);
					if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
					{
						string text = "[" + MyInput.Static.GetGameControl(MyControlsSpace.SECONDARY_TOOL_ACTION).GetControlButtonName(MyGuiInputDeviceEnum.Mouse) + "]";
						m_targetLockNotification.SetTextFormatArguments(text);
					}
					else
					{
						m_targetLockNotification.SetTextFormatArguments(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.SECONDARY_TOOL_ACTION));
					}
					m_targetLockNotification.Level = MyNotificationLevel.Control;
					MyHud.Notifications.Add(m_targetLockNotification);
				}
			}

			public static Vector2 CalculateCircularBarPosition(Vector2 screenPosNormalized, MyStatControlCircularProgressBar progressBar)
			{
				Vector2 vector = screenPosNormalized * MyGuiManager.GetHudSize();
				Vector2 vector2 = new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
				float num = vector2.X / MyGuiManager.GetHudSize().X;
				float num2 = vector2.Y / MyGuiManager.GetHudSize().Y;
				if (MyVideoSettingsManager.IsTripleHead())
				{
					vector.X += 1f;
				}
				vector.X *= num;
				vector.Y *= num2;
				return vector - progressBar.Size / 2f;
			}
		}

		private class MyTargetLeadRender
		{
			private readonly MyLargeTurretTargetingSystem.MyPositionPrediction m_positionPrediction;

			private readonly Vector2 PREDICTION_MARKER_SIZE = new Vector2(128f, 128f);

			private const float LEAD_LINE_THICKNESS = 3f;

			public MyTargetIndicatorRender TargetIndicator { get; }

			public MyTargetLeadRender(MyTargetIndicatorRender targetIndicator)
			{
				TargetIndicator = targetIndicator;
				IMyShootOrigin originEntity;
				if ((originEntity = MySession.Static.ControlledEntity.Entity as IMyShootOrigin) != null)
				{
					m_positionPrediction = new MyLargeTurretTargetingSystem.MyPositionPrediction(originEntity);
					m_positionPrediction.PreferedGridTargetingOption = MyLargeTurretTargetingSystem.MyPositionPrediction.MyGridTargetingOption.AABBCenter;
				}
			}

			public void Draw(MyHudMarkerRender renderer, Vector2 targetPos, MyTargetIndicatorRender.MyTargetInfo targetInfo, MyStatControlTargetingProgressBar.ProgressBarTargetType targetType)
			{
				if (TargetIndicator.TargetInfo.Grid.IsStatic)
				{
					m_positionPrediction.PreferedGridTargetingOption = MyLargeTurretTargetingSystem.MyPositionPrediction.MyGridTargetingOption.AABBCenter;
				}
				else
				{
					m_positionPrediction.PreferedGridTargetingOption = MyLargeTurretTargetingSystem.MyPositionPrediction.MyGridTargetingOption.CenterOfMass;
				}
				IMyTargetingCapableBlock myTargetingCapableBlock;
				bool num = (myTargetingCapableBlock = MySession.Static.ControlledEntity as IMyTargetingCapableBlock) != null && myTargetingCapableBlock.CanActiveToolShoot();
				IMyTargetingCapableBlock myTargetingCapableBlock2;
				bool flag = (myTargetingCapableBlock2 = MySession.Static.ControlledEntity as IMyTargetingCapableBlock) != null && myTargetingCapableBlock2.IsTargetLockingEnabled();
				if (!num || !flag || TargetIndicator?.TargetInfo?.Grid == null || m_positionPrediction == null)
				{
					return;
				}
				MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
				if (MySession.Static.ControlledGrid != null && myCubeBlock != null && (!MySession.Static.ControlledGrid.IsPowered || !myCubeBlock.IsWorking))
				{
					return;
				}
				MyEntity entity = MySession.Static.ControlledEntity.Entity;
				IMyShootOrigin origin;
				if (entity != null && (origin = entity as IMyShootOrigin) != null)
				{
					m_positionPrediction.Origin = origin;
				}
				m_positionPrediction.UsePrediction = true;
				Vector2 projectedPoint2D = default(Vector2);
				bool isBehind = default(bool);
				if (!(!TargetIndicator.HasTargetLock || !TryComputeScreenPoint(m_positionPrediction.GetTargetCoordinates(TargetIndicator.TargetInfo.Grid), out projectedPoint2D, out isBehind) || isBehind))
				{
					MyAmmoDefinition currentAmmoDefininiton = m_positionPrediction.CurrentAmmoDefininiton;
					float num2 = m_positionPrediction.Origin.MaxShootRange;
					if (currentAmmoDefininiton != null)
					{
						num2 = currentAmmoDefininiton.MaxTrajectory;
					}
					num2 *= num2;
					if ((targetInfo.Grid.PositionComp.GetPosition() - GetDistanceMeasuringMatrix().Translation).LengthSquared() <= (double)num2)
					{
						Vector2 position = projectedPoint2D * MyGuiManager.GetHudSize();
						Color targetingColor = MyTargetIndicatorRender.GetTargetingColor(targetType);
						renderer.AddTexturedQuad("Textures\\GUI\\TargetingPredictionMarker.dds", position, -Vector2.UnitY, targetingColor, PREDICTION_MARKER_SIZE.X * 0.25f, PREDICTION_MARKER_SIZE.Y * 0.25f);
						DrawLine(targetPos, projectedPoint2D, MyHud.TargetingMarkers.TargetingCircle.Size, PREDICTION_MARKER_SIZE * 0.5f, targetingColor, 3f, renderer);
					}
				}
			}

			public static void DrawLine(Vector2 start, Vector2 end, Vector2 startSizePixels, Vector2 endSizePixels, Color color, float size, MyHudMarkerRender renderer)
			{
				Vector2 hudPixelCoordFromNormalizedCoord = MyGuiManager.GetHudPixelCoordFromNormalizedCoord(start);
				Vector2 hudPixelCoordFromNormalizedCoord2 = MyGuiManager.GetHudPixelCoordFromNormalizedCoord(end);
				Vector2 value = hudPixelCoordFromNormalizedCoord2 - hudPixelCoordFromNormalizedCoord;
				Vector2 vector = Vector2.Normalize(value);
				float num = startSizePixels.X / 2f;
				float num2 = endSizePixels.X / 2f;
				if (!(value.LengthSquared() < num * num * 1.2f))
				{
					Vector2 vector2 = hudPixelCoordFromNormalizedCoord + vector * num;
					Vector2 vector3 = hudPixelCoordFromNormalizedCoord2 - vector * num2;
					Vector2 vector4 = new Vector2((vector3.X + vector2.X) / 2f, (vector3.Y + vector2.Y) / 2f);
					float rotation = (float)(Math.Atan2(value.Y, value.X) - Math.Atan2(Vector3.Right.Y, Vector3.Right.X));
					float num3 = (vector3 - vector2).Length() / 2f;
					float num4 = size / 2f;
					RectangleF destination = new RectangleF(vector4.X - num3, vector4.Y - num4, num3 * 2f, num4 * 2f);
					MyRenderProxy.DrawSprite("Textures\\GUI\\TargetingLine.dds", ref destination, null, color, rotation, ignoreBounds: true, waitTillLoaded: true);
				}
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static float m_friendAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private static bool m_disableFading = false;

		private MyTargetIndicatorRender m_targetIndicatorRender;

		private static MyHudNotification m_signalModeNotification = null;

		private ConcurrentDictionary<MyEntity, MyPlayerIndicator> m_playerIndicatorsDict = new ConcurrentDictionary<MyEntity, MyPlayerIndicator>();
<<<<<<< HEAD
=======

		private static float m_ownerAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private static float m_enemyAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static MatrixD m_distanceMeasuringMatrix;

		private MyDynamicObjectPool<PointOfInterest> m_pointOfInterestPool = new MyDynamicObjectPool<PointOfInterest>(32);

		private List<PointOfInterest> m_pointsOfInterest = new List<PointOfInterest>();

<<<<<<< HEAD
		private static float m_ownerAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private static float m_enemyAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private static MatrixD m_distanceMeasuringMatrix;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static SignalMode SignalDisplayMode { get; private set; }

		public static float FriendAntennaRange
		{
			get
			{
				return NormalizeLog(m_friendAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
			}
			set
			{
				m_friendAntennaRange = Denormalize(value);
			}
		}

		public static float OwnerAntennaRange
		{
			get
			{
				return NormalizeLog(m_ownerAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
			}
			set
			{
				m_ownerAntennaRange = Denormalize(value);
			}
		}

		public static float EnemyAntennaRange
		{
			get
			{
				return NormalizeLog(m_enemyAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
			}
			set
			{
				m_enemyAntennaRange = Denormalize(value);
			}
		}

		private static MatrixD? ControlledEntityMatrix
		{
			get
			{
				if (MySession.Static.ControlledEntity != null)
				{
					return MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef;
				}
				return null;
			}
		}

		private static MatrixD? LocalCharacterMatrix
		{
			get
			{
				if (MySession.Static.LocalCharacter != null)
				{
					return MySession.Static.LocalCharacter.WorldMatrix;
				}
				return null;
			}
		}

		private static MatrixD CameraMatrix => MySector.MainCamera.WorldMatrix;

		public override void Update()
		{
			MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			m_disableFading = MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED);
			_ = MySession.Static.ControlledEntity;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.TOGGLE_SIGNALS) && !MyInput.Static.IsAnyCtrlKeyPressed() && MyScreenManager.FocusedControl == null)
			{
				ChangeSignalMode();
			}
			MatrixD? controlledEntityMatrix = ControlledEntityMatrix;
			if (!controlledEntityMatrix.HasValue || (!MySession.Static.CameraOnCharacter && MySession.Static.IsCameraUserControlledSpectator()))
			{
				m_distanceMeasuringMatrix = CameraMatrix;
				return;
			}
			MatrixD? localCharacterMatrix = LocalCharacterMatrix;
			if (MySession.Static.CameraOnCharacter && localCharacterMatrix.HasValue)
			{
				m_distanceMeasuringMatrix = localCharacterMatrix.Value;
			}
			else
			{
				m_distanceMeasuringMatrix = controlledEntityMatrix.Value;
			}
		}

		public static void ChangeSignalMode()
		{
			if (!MyHud.IsHudMinimal && !MyHud.MinimalHud)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				}, "Play Sound");
				SignalDisplayMode++;
				if (SignalDisplayMode >= SignalMode.MaxSignalModes)
				{
					SignalDisplayMode = SignalMode.DefaultMode;
				}
				if (m_signalModeNotification != null)
				{
					MyHud.Notifications.Remove(m_signalModeNotification);
					m_signalModeNotification = null;
				}
				switch (SignalDisplayMode)
				{
				case SignalMode.DefaultMode:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_DefaultMode, 1000);
					break;
				case SignalMode.FullDisplay:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_FullDisplay, 1000);
					break;
				case SignalMode.NoNames:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_NoNames, 1000);
					break;
				case SignalMode.Off:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_Off, 1000);
					break;
				}
				if (m_signalModeNotification != null)
				{
					MyHud.Notifications.Add(m_signalModeNotification);
				}
			}
		}

		public MyHudMarkerRender(MyGuiScreenHudBase hudScreen)
			: base(hudScreen)
		{
			m_targetIndicatorRender = new MyTargetIndicatorRender();
		}

		public override void DrawLocationMarkers(MyHudLocationMarkers locationMarkers)
		{
			if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null || MySession.Static.LocalHumanPlayer.Identity == null)
			{
				return;
			}
			locationMarkers.ProcessChanges();
			float num = m_ownerAntennaRange * m_ownerAntennaRange;
			float num2 = m_friendAntennaRange * m_friendAntennaRange;
			float num3 = m_enemyAntennaRange * m_enemyAntennaRange;
			MatrixD distanceMeasuringMatrix = GetDistanceMeasuringMatrix();
			foreach (MyHudEntityParams markerEntity in locationMarkers.MarkerEntities)
			{
				if (markerEntity.ShouldDraw != null && !markerEntity.ShouldDraw())
				{
					continue;
				}
				double num4 = (markerEntity.Position - distanceMeasuringMatrix.Translation).LengthSquared();
				MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(markerEntity.Owner, MySession.Static.LocalHumanPlayer.Identity.IdentityId, markerEntity.Share);
				switch (relationPlayerBlock)
				{
				case MyRelationsBetweenPlayerAndBlock.Owner:
					if (num4 > (double)num)
					{
						continue;
					}
					break;
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
				case MyRelationsBetweenPlayerAndBlock.Friends:
					if (num4 > (double)num2)
					{
						continue;
					}
					break;
				case MyRelationsBetweenPlayerAndBlock.Neutral:
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					if (num4 > (double)num3)
					{
						continue;
					}
					break;
				}
				MyEntity myEntity = markerEntity.Entity as MyEntity;
				if (myEntity != null)
				{
					AddEntity(myEntity, relationPlayerBlock, markerEntity.Text, IsScenarioObjective(myEntity));
				}
				else
				{
					AddProxyEntity(markerEntity.Position, relationPlayerBlock, markerEntity.Text);
				}
			}
			m_hudScreen?.DrawTexts();
		}

		private static bool IsScenarioObjective(MyEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (entity.Name != null && entity.Name.Length >= 13 && entity.Name.Substring(0, 13).Equals("MissionStart_"))
			{
				return true;
			}
			return false;
		}

		public static ref readonly MatrixD GetDistanceMeasuringMatrix()
		{
			return ref m_distanceMeasuringMatrix;
		}

		public void AddPlayerIndicator(MyEntity target, MyRelationsBetweenPlayers relation, bool isAlwaysVisible)
		{
<<<<<<< HEAD
			m_playerIndicatorsDict.TryGetValue(target, out var value);
			if (value == null)
			{
				value = new MyPlayerIndicator(target, relation, isAlwaysVisible);
				m_playerIndicatorsDict[target] = value;
			}
			else
			{
				value.ResetFadeout(relation);
=======
			MyPlayerIndicator myPlayerIndicator = default(MyPlayerIndicator);
			m_playerIndicatorsDict.TryGetValue(target, ref myPlayerIndicator);
			if (myPlayerIndicator == null)
			{
				myPlayerIndicator = new MyPlayerIndicator(target, relation, isAlwaysVisible);
				m_playerIndicatorsDict.set_Item(target, myPlayerIndicator);
			}
			else
			{
				myPlayerIndicator.ResetFadeout(relation);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// Adds a generic POI, styled like a GPS coordinate.
		/// Currently only used to draw a center-of-the-world marker.
		/// </summary>
		public void AddPOI(Vector3D worldPosition, StringBuilder name, MyRelationsBetweenPlayerAndBlock relationship)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.GPS, relationship);
				pointOfInterest.SetText(name);
			}
		}

		public void AddEntity(MyEntity entity, MyRelationsBetweenPlayerAndBlock relationship, StringBuilder entityName, bool IsScenarioMarker = false)
		{
			if (SignalDisplayMode == SignalMode.Off || entity == null)
			{
				return;
			}
			Vector3D position = entity.PositionComp.GetPosition();
			long? num = (entity as MyCubeBlock)?.CubeGrid.EntityId;
			long? targetGridId = m_targetIndicatorRender.TargetGridId;
			if (num.HasValue && num == targetGridId)
			{
				return;
			}
			PointOfInterest.PointOfInterestType type = PointOfInterest.PointOfInterestType.UnknownEntity;
			if (entity is MyCharacter)
			{
				if (entity == MySession.Static.LocalCharacter)
				{
					return;
				}
				type = PointOfInterest.PointOfInterestType.Character;
				position += entity.WorldMatrix.Up * 1.2999999523162842;
			}
			else
			{
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null && myCubeBlock.CubeGrid != null)
				{
					type = ((myCubeBlock.CubeGrid.GridSizeEnum != MyCubeSize.Small) ? (myCubeBlock.CubeGrid.IsStatic ? PointOfInterest.PointOfInterestType.StaticEntity : PointOfInterest.PointOfInterestType.LargeEntity) : PointOfInterest.PointOfInterestType.SmallEntity);
				}
			}
			PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
			m_pointsOfInterest.Add(pointOfInterest);
			pointOfInterest.Reset();
			if (IsScenarioMarker)
			{
				type = PointOfInterest.PointOfInterestType.Scenario;
			}
			pointOfInterest.SetState(position, type, relationship);
			pointOfInterest.SetEntity(entity);
			pointOfInterest.SetText(entityName);
		}

		public void AddGPS(MyGps gps)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.Color = gps.GPSColor;
				pointOfInterest.SetState(gps.Coords, (gps.ContractId != 0L) ? PointOfInterest.PointOfInterestType.ContractGPS : (gps.IsObjective ? PointOfInterest.PointOfInterestType.Objective : PointOfInterest.PointOfInterestType.GPS), MyRelationsBetweenPlayerAndBlock.Owner);
				if (string.IsNullOrEmpty(gps.DisplayName))
				{
					pointOfInterest.SetText(gps.Name);
				}
				else
				{
					pointOfInterest.SetText(gps.DisplayName);
				}
				pointOfInterest.AlwaysVisible = gps.AlwaysVisible;
				pointOfInterest.ContainerRemainingTime = gps.ContainerRemainingTime;
			}
		}

		public void AddButtonMarker(Vector3D worldPosition, string name)
		{
			PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
			pointOfInterest.Reset();
			pointOfInterest.AlwaysVisible = true;
			pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.ButtonMarker, MyRelationsBetweenPlayerAndBlock.Owner);
			pointOfInterest.SetText(name);
			m_pointsOfInterest.Add(pointOfInterest);
		}

		public void AddOre(Vector3D worldPosition, string name)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.Ore, MyRelationsBetweenPlayerAndBlock.NoOwnership);
				pointOfInterest.SetText(name);
			}
		}

		public void AddTarget(Vector3D worldPosition)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.Target, MyRelationsBetweenPlayerAndBlock.Enemies);
			}
		}

		public void AddOffscreenTarget(Vector3D worldPosition, MyRelationsBetweenPlayerAndBlock targetPlayerRelation)
		{
			PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
			m_pointsOfInterest.Add(pointOfInterest);
			pointOfInterest.Reset();
			pointOfInterest.AlwaysVisible = true;
			pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.OffscreenTarget, targetPlayerRelation);
		}

		public void AddHacking(Vector3D worldPosition, StringBuilder name)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.Hack, MyRelationsBetweenPlayerAndBlock.Owner);
				pointOfInterest.SetText(name);
			}
		}

		public void SetTarget(MyCubeGrid targetGrid, MyTargetLockingState state, float progressPercent = 0f)
		{
			if (state == MyTargetLockingState.None || targetGrid?.Physics == null)
			{
				m_targetIndicatorRender.ClearTarget();
			}
			else
			{
				m_targetIndicatorRender.SetTarget(targetGrid, state, progressPercent);
			}
		}

		public void AddProxyEntity(Vector3D worldPosition, MyRelationsBetweenPlayerAndBlock relationship, StringBuilder name)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.UnknownEntity, relationship);
				pointOfInterest.SetText(name);
			}
		}

		/// <summary>
		/// Appends the distance in meters, kilometers, light seconds or light years to the string builder.
		/// Rounded to 2 decimals, i.e. 12.34 meters.
		/// </summary>
		/// <param name="stringBuilder">The string builder to be appended to.</param>
		/// <param name="distance">The distance in meters to be appended.</param>
		public static void AppendDistance(StringBuilder stringBuilder, double distance)
		{
			if (stringBuilder == null)
			{
				return;
			}
			distance = Math.Abs(distance);
			if (distance > 9.460730473E+15)
			{
				stringBuilder.AppendDecimal(Math.Round(distance / 9.460730473E+15, 2), 2);
				stringBuilder.Append("ly");
			}
			else if (distance > 299792458.00013667)
			{
				stringBuilder.AppendDecimal(Math.Round(distance / 299792458.00013667, 2), 2);
				stringBuilder.Append("ls");
			}
			else if (distance > 1000.0)
			{
				if (distance > 1000000.0)
				{
					stringBuilder.AppendDecimal(Math.Round(distance / 1000.0, 2), 1);
				}
				else
				{
					stringBuilder.AppendDecimal(Math.Round(distance / 1000.0, 2), 2);
				}
				stringBuilder.Append("km");
			}
			else
			{
				stringBuilder.AppendDecimal(Math.Round(distance, 2), 1);
				stringBuilder.Append("m");
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Tries to compute the screenpoint for this world position from the main camera's PoV. May fail if the projection is invalid.
		/// projectedPoint2D will be set to Vector2.Zero if it was not possible to project.
		/// </summary>
		/// <param name="worldPosition">The world position to project to the screen.</param>
		/// <param name="projectedPoint2D">The screen position [-1, 1] by [-1, 1]</param>
		/// <param name="isBehind">Whether or not the position is behind the camera.</param>
		/// <returns>True if it could project, false otherwise.</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool TryComputeScreenPoint(Vector3D worldPosition, out Vector2 projectedPoint2D, out bool isBehind)
		{
			Vector3D position = Vector3D.Transform(worldPosition, MySector.MainCamera.ViewMatrix);
			Vector4D vector4D = Vector4D.Transform(position, MySector.MainCamera.ProjectionMatrix);
			if (position.Z > 0.0)
			{
				vector4D.X *= -1.0;
				vector4D.Y *= -1.0;
			}
			if (vector4D.W == 0.0)
			{
				projectedPoint2D = Vector2.Zero;
				isBehind = false;
				return false;
			}
			projectedPoint2D = new Vector2((float)(vector4D.X / vector4D.W / 2.0) + 0.5f, (float)((0.0 - vector4D.Y) / vector4D.W) / 2f + 0.5f);
			if (MyVideoSettingsManager.IsTripleHead())
			{
				projectedPoint2D.X = (projectedPoint2D.X - 0.333333343f) / 0.333333343f;
			}
			Vector3D vector = worldPosition - CameraMatrix.Translation;
			vector.Normalize();
			double num = Vector3D.Dot(MySector.MainCamera.ForwardVector, vector);
			isBehind = num < 0.0;
			return true;
		}

		public override void Draw()
		{
			Vector3D position = MySector.MainCamera.Position;
			List<PointOfInterest> list = new List<PointOfInterest>();
			if (SignalDisplayMode == SignalMode.FullDisplay)
			{
				list.AddRange(m_pointsOfInterest);
			}
			else
			{
				for (int i = 0; i < m_pointsOfInterest.Count; i++)
				{
					PointOfInterest pointOfInterest = m_pointsOfInterest[i];
					PointOfInterest pointOfInterest2 = null;
					if (pointOfInterest.AlwaysVisible)
					{
						list.Add(pointOfInterest);
						continue;
					}
					if (pointOfInterest.AllowsCluster)
					{
						int num = i + 1;
						while (num < m_pointsOfInterest.Count)
						{
							PointOfInterest pointOfInterest3 = m_pointsOfInterest[num];
							if (pointOfInterest3 == pointOfInterest)
							{
								num++;
							}
							else if (!pointOfInterest3.AllowsCluster)
							{
								num++;
							}
							else if (pointOfInterest.IsPOINearby(pointOfInterest3, position))
							{
								if (pointOfInterest2 == null)
								{
									pointOfInterest2 = m_pointOfInterestPool.Allocate();
									pointOfInterest2.Reset();
									pointOfInterest2.SetState(Vector3D.Zero, PointOfInterest.PointOfInterestType.Group, MyRelationsBetweenPlayerAndBlock.NoOwnership);
									pointOfInterest2.AddPOI(pointOfInterest);
								}
								pointOfInterest2.AddPOI(pointOfInterest3);
								m_pointsOfInterest.RemoveAt(num);
							}
							else
							{
								num++;
							}
						}
					}
					else if (pointOfInterest.POIType == PointOfInterest.PointOfInterestType.Target && (position - pointOfInterest.WorldPosition).Length() > 2000.0)
					{
						continue;
					}
					if (pointOfInterest2 != null)
					{
						list.Add(pointOfInterest2);
					}
					else
					{
						list.Add(pointOfInterest);
					}
				}
			}
			list.Sort((PointOfInterest a, PointOfInterest b) => b.DistanceToCam.CompareTo(a.DistanceToCam));
			List<Vector2D> list2 = new List<Vector2D>(list.Count);
			List<Vector2> list3 = new List<Vector2>(list.Count);
			List<bool> list4 = new List<bool>(list.Count);
			if (!m_disableFading && SignalDisplayMode != SignalMode.FullDisplay)
			{
				for (int num2 = list.Count - 1; num2 >= 0; num2--)
				{
					Vector3D worldPos = list[num2].WorldPosition;
					worldPos = MySector.MainCamera.WorldToScreen(ref worldPos);
					Vector2D vector2D = new Vector2D(worldPos.X, worldPos.Y);
					bool flag = Vector3D.Dot(list[num2].WorldPosition - CameraMatrix.Translation, CameraMatrix.Forward) < 0.0;
					float num3 = float.MaxValue;
					for (int j = 0; j < list2.Count; j++)
					{
						if (flag == list4[j])
						{
							float num4 = (float)(list2[j] - vector2D).LengthSquared();
							if (num4 < num3)
							{
								num3 = num4;
							}
						}
					}
					float x;
					float y;
					if (num3 > 0.022f)
					{
						x = 1f;
						y = 1f;
					}
					else if (num3 > 0.011f)
					{
						x = 81.81f * num3 - 0.8f;
						y = 90f * num3 - 0.98f;
					}
					else
					{
						x = 0.1f;
						y = 0.01f;
					}
					list2.Add(vector2D);
					list3.Add(new Vector2(x, y));
					list4.Add(flag);
				}
			}
			if (m_disableFading || SignalDisplayMode == SignalMode.FullDisplay)
			{
				for (int k = 0; k < list.Count; k++)
				{
					_ = list.Count;
					list[k].Draw(this, 1f, 1f, (list[k].POIType != PointOfInterest.PointOfInterestType.Objective) ? 1 : 2, list[k].POIType != PointOfInterest.PointOfInterestType.Objective && list[k].POIType != PointOfInterest.PointOfInterestType.OffscreenTarget);
				}
			}
			else
			{
				for (int l = 0; l < list.Count; l++)
				{
					int index = list.Count - l - 1;
					list[l].Draw(this, list3[index].X, list3[index].Y, (list[l].POIType != PointOfInterest.PointOfInterestType.Objective) ? 1 : 2, list[l].POIType != PointOfInterest.PointOfInterestType.Objective && list[l].POIType != PointOfInterest.PointOfInterestType.OffscreenTarget);
				}
			}
			foreach (PointOfInterest item in m_pointsOfInterest)
			{
				item.Reset();
				m_pointOfInterestPool.Deallocate(item);
			}
			MyPlayerIndicator myPlayerIndicator = null;
<<<<<<< HEAD
			foreach (MyPlayerIndicator value in m_playerIndicatorsDict.Values)
=======
			foreach (MyPlayerIndicator value in m_playerIndicatorsDict.get_Values())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (SignalDisplayMode == SignalMode.Off)
				{
					if (!value.IsAlwaysVisible)
					{
						myPlayerIndicator = value;
					}
				}
				else if (!value.Draw())
				{
					myPlayerIndicator = value;
				}
			}
			if (myPlayerIndicator != null)
			{
<<<<<<< HEAD
				m_playerIndicatorsDict.Remove(myPlayerIndicator.TargetEntity);
=======
				m_playerIndicatorsDict.Remove<MyEntity, MyPlayerIndicator>(myPlayerIndicator.TargetEntity);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_pointsOfInterest.Clear();
		}

		public void DrawTargetIndicatorRender()
		{
			m_targetIndicatorRender?.Draw(this);
		}

		public static float Normalize(float value)
		{
			return NormalizeLog(value, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
		}

		public static float Denormalize(float value)
		{
			return DenormalizeLog(value, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
		}

		private static float NormalizeLog(float f, float min, float max)
		{
			return MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0f, 1f);
		}

		private static float DenormalizeLog(float f, float min, float max)
		{
			return MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max);
		}
	}
}
