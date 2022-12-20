using System;
using System.Collections.Generic;
using System.Text;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Definitions.GUI;
using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.GUI;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenHudSpace : MyGuiScreenHudBase
	{
		public static MyGuiScreenHudSpace Static;

		private const float ALTITUDE_CHANGE_THRESHOLD = 500f;

		public const int PING_THRESHOLD_MILLISECONDS = 250;

		public bool EnableDrawAsync = true;

		private MyGuiControlToolbar m_toolbarControl;

		private MyGuiControlDPad m_DPadControl;

		private MyGuiControlContextHelp m_contextHelp;

		private MyGuiControlBlockInfo m_blockInfo;

		private MyGuiControlLabel m_rotatingWheelLabel;

		private MyGuiControlRotatingWheel m_rotatingWheelControl;

		private MyGuiControlMultilineText m_cameraInfoMultilineControl;

		private MyGuiControlQuestlog m_questlogControl;

		private MyGuiControlLabel m_buildModeLabel;

		private MyGuiControlLabel m_blocksLeft;

		private MyHudCameraOverlay m_overlay;

		private MyHudControlChat m_chatControl;

		private MyHudMarkerRender m_markerRender;

		private int m_oreHudMarkerStyle;

		private int m_gpsHudMarkerStyle;

		private int m_buttonPanelHudMarkerStyle;

		private MyHudEntityParams m_tmpHudEntityParams;

		private MyTuple<Vector3D, MyEntityOreDeposit>[] m_nearestOreDeposits;

		private float[] m_nearestDistanceSquared;

		private MyHudControlGravityIndicator m_gravityIndicator;

		private MyObjectBuilder_GuiTexture m_visorOverlayTexture;

		private readonly List<MyStatControls> m_statControls = new List<MyStatControls>();

<<<<<<< HEAD
		private HashSet<MyStringId> m_suppressedWarnings = new HashSet<MyStringId>(MyStringId.Comparer);
=======
		private HashSet<MyStringId> m_suppressedWarnings = new HashSet<MyStringId>((IEqualityComparer<MyStringId>)MyStringId.Comparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private bool m_hiddenToolbar;

		public float m_gravityHudWidth;

		private float m_altitude;

		private List<MyStringId> m_warningNotifications = new List<MyStringId>();

		private readonly byte m_warningFrameCount = 200;

		private byte m_currentFrameCount;

		private readonly MyHudWeaponHitIndicator m_hitIndicator = new MyHudWeaponHitIndicator();

		private Task? m_hudTask;

		private MyRenderMessageDrawCommands m_drawAsyncMessages;

		private List<MyDamageIndicator> m_damageIndicators = new List<MyDamageIndicator>();

		private readonly TimeSpan DAMAGE_INDICATOR_VISIBILITY_TIME = TimeSpan.FromSeconds(1.5);

		public MyGuiScreenHudSpace()
		{
			Static = this;
			RecreateControls(constructor: true);
			m_markerRender = new MyHudMarkerRender(this);
			m_oreHudMarkerStyle = m_markerRender.AllocateMarkerStyle("White", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_neutral, Color.White);
			m_gpsHudMarkerStyle = m_markerRender.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.GPS_COLOR);
			m_buttonPanelHudMarkerStyle = m_markerRender.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.GPS_COLOR);
			m_tmpHudEntityParams = new MyHudEntityParams
			{
				Text = new StringBuilder(),
				FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL
			};
			if (m_contextHelp == null)
			{
				m_contextHelp = new MyGuiControlContextHelp(GetDefaultStyle());
				m_contextHelp.BlockInfo = MyHud.BlockInfo;
			}
		}

		private MyGuiControlBlockInfo.MyControlBlockInfoStyle GetDefaultStyle()
		{
			MyGuiControlBlockInfo.MyControlBlockInfoStyle result = default(MyGuiControlBlockInfo.MyControlBlockInfoStyle);
			result.BackgroundColormask = new Vector4(13f / 85f, 52f / 255f, 59f / 255f, 0.9f);
			result.BlockNameLabelFont = "Blue";
			result.EnableBlockTypeLabel = true;
			result.ComponentsLabelText = MySpaceTexts.HudBlockInfo_Components;
			result.ComponentsLabelFont = "Blue";
			result.InstalledRequiredLabelText = MySpaceTexts.HudBlockInfo_Installed_Required;
			result.InstalledRequiredLabelFont = "Blue";
			result.RequiredLabelText = MyCommonTexts.HudBlockInfo_Required;
			result.IntegrityLabelFont = "White";
			result.IntegrityBackgroundColor = new Vector4(4f / 15f, 77f / 255f, 86f / 255f, 0.9f);
			result.IntegrityForegroundColor = new Vector4(23f / 51f, 23f / 85f, 16f / 51f, 1f);
			result.IntegrityForegroundColorOverCritical = new Vector4(122f / 255f, 28f / 51f, 154f / 255f, 1f);
			result.LeftColumnBackgroundColor = new Vector4(46f / 255f, 76f / 255f, 94f / 255f, 1f);
			result.TitleBackgroundColor = new Vector4(53f / 255f, 4f / 15f, 76f / 255f, 0.9f);
			result.ComponentLineMissingFont = "Red";
			result.ComponentLineAllMountedFont = "White";
			result.ComponentLineAllInstalledFont = "Blue";
			result.ComponentLineDefaultFont = "Blue";
			result.ComponentLineDefaultColor = new Vector4(0.6f, 0.6f, 0.6f, 1f);
			result.ShowAvailableComponents = false;
			result.EnableBlockTypePanel = false;
			return result;
		}

		public override void UnloadData()
		{
			m_DPadControl?.Dispose();
			base.UnloadData();
			if (m_DPadControl != null)
			{
				m_DPadControl.UnregisterEvents();
			}
			Static = null;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			if (MyHud.Static != null)
			{
				Elements.Add(new MyGuiControlBloodOverlay());
				InitHudStatControls();
				MyHudDefinition hudDefinition = MyHud.HudDefinition;
				m_gravityIndicator = new MyHudControlGravityIndicator(hudDefinition.GravityIndicator);
				if (hudDefinition.VisorOverlayTexture.HasValue)
				{
					m_visorOverlayTexture = MyGuiTextures.Static.GetTexture(hudDefinition.VisorOverlayTexture.Value);
				}
				m_toolbarControl = new MyGuiControlToolbar(hudDefinition.Toolbar, blockPlayerUseForShownToolbar: false);
				m_toolbarControl.Position = hudDefinition.Toolbar.CenterPosition;
				m_toolbarControl.OriginAlign = hudDefinition.Toolbar.OriginAlign;
				m_toolbarControl.IsActiveControl = false;
				Elements.Add(m_toolbarControl);
				MyObjectBuilder_DPadControlVisualStyle myObjectBuilder_DPadControlVisualStyle = null;
				myObjectBuilder_DPadControlVisualStyle = ((hudDefinition.DPad == null) ? MyObjectBuilder_DPadControlVisualStyle.DefaultStyle() : hudDefinition.DPad);
				m_DPadControl = new MyGuiControlDPad(myObjectBuilder_DPadControlVisualStyle);
				m_DPadControl.Position = myObjectBuilder_DPadControlVisualStyle.CenterPosition;
				m_DPadControl.OriginAlign = myObjectBuilder_DPadControlVisualStyle.OriginAlign;
				m_DPadControl.IsActiveControl = false;
				Elements.Add(m_DPadControl);
				m_textScale = 0.8f * MyGuiManager.LanguageTextScale;
				MyGuiControlBlockInfo.MyControlBlockInfoStyle defaultStyle = GetDefaultStyle();
				m_contextHelp = new MyGuiControlContextHelp(defaultStyle);
				m_contextHelp.IsActiveControl = false;
				Controls.Add(m_contextHelp);
				m_blockInfo = new MyGuiControlBlockInfo(defaultStyle);
				m_blockInfo.IsActiveControl = false;
				MyGuiControlBlockInfo.ShowComponentProgress = true;
				MyGuiControlBlockInfo.CriticalIntegrityColor = new Color(115, 69, 80);
				MyGuiControlBlockInfo.OwnershipIntegrityColor = new Color(56, 67, 147);
				Controls.Add(m_blockInfo);
				m_questlogControl = new MyGuiControlQuestlog(new Vector2(20f, 20f));
				m_questlogControl.IsActiveControl = false;
				m_questlogControl.RecreateControls();
				Controls.Add(m_questlogControl);
				m_chatControl = new MyHudControlChat(MyHud.Chat, Vector2.Zero, new Vector2(0.339f, 0.28f), null, "White", 0.7f);
				Elements.Add(m_chatControl);
				m_cameraInfoMultilineControl = new MyGuiControlMultilineText(Vector2.Zero, new Vector2(0.4f, 0.25f), null, "White", 0.7f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, drawScrollbarV: false, drawScrollbarH: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				m_cameraInfoMultilineControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
				Elements.Add(m_cameraInfoMultilineControl);
				m_rotatingWheelControl = new MyGuiControlRotatingWheel(new Vector2(0.5f, 0.8f));
				Controls.Add(m_rotatingWheelControl);
				Controls.Add(m_rotatingWheelLabel = new MyGuiControlLabel());
				Vector2 hudPos = new Vector2(0.5f, 0.02f);
				hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
				m_buildModeLabel = new MyGuiControlLabel(hudPos, null, MyTexts.GetString(MyCommonTexts.Hud_BuildMode), null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				Controls.Add(m_buildModeLabel);
				m_blocksLeft = new MyGuiControlLabel(new Vector2(0.238f, 0.89f), null, MyHud.BlocksLeft.GetStringBuilder().ToString(), null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				Controls.Add(m_blocksLeft);
				m_overlay = new MyHudCameraOverlay();
				Controls.Add(m_overlay);
				RegisterAlphaMultiplier(VisualStyleCategory.Background, MySandboxGame.Config.HUDBkOpacity);
				MyHud.ReloadTexts();
				Controls.Add(m_hitIndicator.GuiControlImage);
			}
		}

		private void InitHudStatControls()
		{
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			m_statControls.Clear();
			if (hudDefinition.StatControls != null)
			{
				MyObjectBuilder_StatControls[] statControls = hudDefinition.StatControls;
				foreach (MyObjectBuilder_StatControls myObjectBuilder_StatControls in statControls)
				{
					float uiScale = (myObjectBuilder_StatControls.ApplyHudScale ? (MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier) : MyGuiManager.GetSafeScreenScale());
					MyStatControls myStatControls = new MyStatControls(myObjectBuilder_StatControls, uiScale);
					Vector2 coordScreen = myObjectBuilder_StatControls.Position * MySandboxGame.ScreenSize;
					myStatControls.Position = MyUtils.AlignCoord(coordScreen, MySandboxGame.ScreenSize, myObjectBuilder_StatControls.OriginAlign);
					m_statControls.Add(myStatControls);
				}
			}
		}

		private void RefreshRotatingWheel()
		{
			m_rotatingWheelLabel.Visible = MyHud.RotatingWheelVisible;
			m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible;
			if (MyHud.RotatingWheelVisible && m_rotatingWheelLabel.TextToDraw != MyHud.RotatingWheelText)
			{
				m_rotatingWheelLabel.Position = m_rotatingWheelControl.Position + new Vector2(0f, 0.05f);
				m_rotatingWheelLabel.TextToDraw = MyHud.RotatingWheelText;
				Vector2 textSize = m_rotatingWheelLabel.GetTextSize();
				m_rotatingWheelLabel.PositionX -= textSize.X / 2f;
			}
		}

<<<<<<< HEAD
		public void AddDamageIndicator(float damage, MyHitInfo hitInfo, Vector3D origin)
=======
		public void AddDamageIndicator(float damage, MyHitInfo hitInfo, Vector3 origin)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (TryComputeScreenPoint(origin, out var projectedPoint2D))
			{
				m_damageIndicators.Add(new MyDamageIndicator
				{
					IndicatorCreationTime = MySession.Static.ElapsedGameTime,
					Damage = damage,
					ScreenPosition = projectedPoint2D
				});
			}
		}

		public void AddPlayerMarker(MyEntity target, MyRelationsBetweenPlayers relation, bool isAlwaysVisible)
		{
			m_markerRender.AddPlayerIndicator(target, relation, isAlwaysVisible);
		}

<<<<<<< HEAD
		public void AddOffscreenTargetMarker(Vector3 targetWorldPosition, MyRelationsBetweenPlayerAndBlock targetPlayerRelation)
		{
			MyHud.OffscreenTargetMarker.Position = targetWorldPosition;
			MyHud.OffscreenTargetMarker.TargetPlayerRelation = targetPlayerRelation;
			MyHud.OffscreenTargetMarker.Visible = true;
		}

		public void RemoveOffscreenTargetMarker()
		{
			MyHud.OffscreenTargetMarker.Visible = false;
		}

		/// <summary>
		/// Sets up the alpha multiplier for all stat controls in the hud.
		/// </summary>
		/// <param name="category">Category of visual styles.</param>
		/// <param name="multiplier">Multiplier value.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void RegisterAlphaMultiplier(VisualStyleCategory category, float multiplier)
		{
			m_statControls.ForEach(delegate(MyStatControls c)
			{
				c.RegisterAlphaMultiplier(category, multiplier);
			});
		}

<<<<<<< HEAD
		/// <inheritdoc />
		public override void PrepareDraw()
		{
			if (!(m_transitionAlpha < 1f) && MyHud.IsVisible)
			{
				if (MySession.Static.ControlledEntity != null && MySession.Static.CameraController != null)
				{
					MySession.Static.ControlledEntity.DrawHud(MySession.Static.CameraController, MySession.Static.LocalPlayerId);
				}
				bool flag = MyHud.BlockInfo.Components.Count > 0;
				IMyHudStat stat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode"));
				m_contextHelp.Visible = MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud;
				if (stat.CurrentValue == 1f)
				{
					m_contextHelp.Visible &= !string.IsNullOrEmpty(MyHud.BlockInfo.ContextHelp);
				}
				if (stat.CurrentValue == 2f)
				{
					m_contextHelp.Visible &= flag;
				}
				m_contextHelp.BlockInfo = (MyHud.BlockInfo.Visible ? MyHud.BlockInfo : null);
				Vector2 hudPos = new Vector2(0.99f, 0.985f);
				if (MySession.Static.ControlledEntity is MyShipController)
				{
					hudPos.Y = 0.65f;
				}
				hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
				if (MyVideoSettingsManager.IsTripleHead())
				{
					hudPos.X += 1f;
				}
				if (stat.CurrentValue == 2f)
				{
					m_contextHelp.Position = new Vector2(hudPos.X, 0.38f);
					m_contextHelp.ShowJustTitle = true;
				}
				else
				{
					m_contextHelp.Position = new Vector2(hudPos.X, 0.28f);
					m_contextHelp.ShowJustTitle = false;
				}
				m_contextHelp.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_contextHelp.ShowBuildInfo = flag;
				m_blockInfo.Visible = MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud && flag;
				m_blockInfo.BlockInfo = (m_blockInfo.Visible ? MyHud.BlockInfo : null);
				m_blockInfo.Position = m_contextHelp.Position + new Vector2(0f, m_contextHelp.Size.Y + 0.006f);
				m_blockInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_questlogControl.Visible = MyHud.Questlog.Visible && !MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud;
				m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible && !MyHud.MinimalHud && !MyHud.CutsceneHud;
				m_rotatingWheelLabel.Visible = m_rotatingWheelControl.Visible;
				m_chatControl.Visible = !MyHud.MinimalHud || m_chatControl.HasFocus || MyHud.CutsceneHud;
			}
		}

		public override bool Draw()
		{
			if (m_transitionAlpha < 1f || !MyHud.IsVisible)
			{
				return false;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.J) && MyFakes.ENABLE_OBJECTIVE_LINE)
			{
				MyHud.ObjectiveLine.AdvanceObjective();
			}
			MyGuiControlToolbar toolbarControl = m_toolbarControl;
			bool visible = (m_DPadControl.Visible = !m_hiddenToolbar && !MyHud.MinimalHud && !MyHud.CutsceneHud);
			toolbarControl.Visible = visible;
			if (!EnableDrawAsync)
			{
				AsyncUpdate(startTask: false);
				DrawAsync();
			}
			else
			{
				if (m_drawAsyncMessages == null)
				{
					AsyncUpdate(startTask: false);
					DrawAsync();
				}
				m_hudTask?.WaitOrExecute();
				m_hudTask = null;
			}
			MyRenderProxy.ExecuteCommands(m_drawAsyncMessages);
			if (EnableDrawAsync)
=======
		public override void PrepareDraw()
		{
			if (m_transitionAlpha < 1f || !MyHud.IsVisible)
			{
				return;
			}
			if (MySession.Static.ControlledEntity != null && MySession.Static.CameraController != null)
			{
				MySession.Static.ControlledEntity.DrawHud(MySession.Static.CameraController, MySession.Static.LocalPlayerId);
			}
			bool flag = MyHud.BlockInfo.Components.Count > 0;
			IMyHudStat stat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode"));
			m_contextHelp.Visible = MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud;
			if (stat.CurrentValue == 1f)
			{
				m_contextHelp.Visible &= !string.IsNullOrEmpty(MyHud.BlockInfo.ContextHelp);
			}
			if (stat.CurrentValue == 2f)
			{
				m_contextHelp.Visible &= flag;
			}
			m_contextHelp.BlockInfo = (MyHud.BlockInfo.Visible ? MyHud.BlockInfo : null);
			Vector2 hudPos = new Vector2(0.99f, 0.985f);
			if (MySession.Static.ControlledEntity is MyShipController)
			{
				hudPos.Y = 0.65f;
			}
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			if (MyVideoSettingsManager.IsTripleHead())
			{
				hudPos.X += 1f;
			}
			if (stat.CurrentValue == 2f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				AsyncUpdate();
			}
			if ((!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud) || MyPetaInputComponent.SHOW_HUD_ALWAYS)
			{
<<<<<<< HEAD
				m_markerRender.DrawTargetIndicatorRender();
			}
			m_hitIndicator.Update();
=======
				if (!MyHud.ShipInfo.Visible)
				{
					m_contextHelp.Position = new Vector2(hudPos.X, 0.28f);
				}
				else
				{
					m_contextHelp.Position = new Vector2(hudPos.X, 0.1f);
				}
				m_contextHelp.ShowJustTitle = false;
			}
			m_contextHelp.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_contextHelp.ShowBuildInfo = flag;
			m_blockInfo.Visible = MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud && flag;
			m_blockInfo.BlockInfo = (m_blockInfo.Visible ? MyHud.BlockInfo : null);
			m_blockInfo.Position = m_contextHelp.Position + new Vector2(0f, m_contextHelp.Size.Y + 0.006f);
			m_blockInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_questlogControl.Visible = MyHud.Questlog.Visible && !MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud;
			m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible && !MyHud.MinimalHud && !MyHud.CutsceneHud;
			m_rotatingWheelLabel.Visible = m_rotatingWheelControl.Visible;
			m_chatControl.Visible = !MyHud.MinimalHud || m_chatControl.HasFocus || MyHud.CutsceneHud;
		}

		public override bool Draw()
		{
			if (m_transitionAlpha < 1f || !MyHud.IsVisible)
			{
				return false;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.J) && MyFakes.ENABLE_OBJECTIVE_LINE)
			{
				MyHud.ObjectiveLine.AdvanceObjective();
			}
			MyGuiControlToolbar toolbarControl = m_toolbarControl;
			bool visible = (m_DPadControl.Visible = !m_hiddenToolbar && !MyHud.MinimalHud && !MyHud.CutsceneHud);
			toolbarControl.Visible = visible;
			if (!EnableDrawAsync)
			{
				AsyncUpdate(startTask: false);
				DrawAsync();
			}
			else
			{
				if (m_drawAsyncMessages == null)
				{
					AsyncUpdate(startTask: false);
					DrawAsync();
				}
				m_hudTask?.WaitOrExecute();
				m_hudTask = null;
			}
			MyRenderProxy.ExecuteCommands(m_drawAsyncMessages);
			if (EnableDrawAsync)
			{
				AsyncUpdate();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!base.Draw())
			{
				return false;
			}
			Vector2 hudPos = new Vector2(0.014f, 0.81f);
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			m_chatControl.Position = hudPos + new Vector2(0.002f, -0.07f);
			m_chatControl.TextScale = 0.7f;
			hudPos = new Vector2(0.03f, 0.1f);
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			m_cameraInfoMultilineControl.Position = hudPos;
			m_cameraInfoMultilineControl.TextScale = 0.9f;
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				bool flag2 = false;
				MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
				if (myShipController != null)
				{
					flag2 = MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(myShipController.PositionComp.GetPosition()) != 0f;
				}
				if (flag2 && !MySession.Static.IsCameraUserAnySpectator())
				{
					DrawArtificialHorizonAndAltitude();
				}
			}
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				m_buildModeLabel.Visible = MyHud.IsBuildMode;
				if (MyHud.BlocksLeft.Visible)
				{
					StringBuilder stringBuilder = MyHud.BlocksLeft.GetStringBuilder();
					if (!m_blocksLeft.Text.EqualsStrFast(stringBuilder))
					{
						m_blocksLeft.Text = stringBuilder.ToString();
					}
					m_blocksLeft.Visible = true;
				}
				else
				{
					m_blocksLeft.Visible = false;
				}
				if (MyHud.ObjectiveLine.Visible && MyFakes.ENABLE_OBJECTIVE_LINE)
				{
					DrawObjectiveLine(MyHud.ObjectiveLine);
				}
			}
			else
			{
				m_buildModeLabel.Visible = false;
				m_blocksLeft.Visible = false;
			}
			m_blockInfo.BlockInfo = null;
			MyGuiScreenHudBase.HandleSelectedObjectHighlight(MyHud.SelectedObjectHighlight, new MyHudObjectHighlightStyleData
			{
				AtlasTexture = m_atlas,
				TextureCoord = GetTextureCoord(MyHudTexturesEnum.corner)
			});
			if (MyPetaInputComponent.DRAW_WARNINGS && m_warningNotifications.Count != 0)
<<<<<<< HEAD
			{
				DrawPerformanceWarning();
			}
			DrawCameraInfo(MyHud.CameraInfo);
			if (MyHud.VoiceChat.Visible)
			{
				DrawVoiceChat(MyHud.VoiceChat);
			}
			return true;
		}

		private void UpdatePerfWarnings()
		{
			List<string> suppressedWarnings = MySession.Static.Settings.SuppressedWarnings;
			if (suppressedWarnings != null && m_suppressedWarnings.Count == 0)
			{
				foreach (string item in suppressedWarnings)
				{
					m_suppressedWarnings.Add(MyStringId.GetOrCompute(item));
				}
			}
			if (!MySandboxGame.Config.EnablePerformanceWarnings)
			{
				return;
			}
			if (MySession.Static.IsRunningExperimental)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode, check: true);
			}
			MyReplicationClient myReplicationClient;
			if ((myReplicationClient = MyMultiplayer.Static?.ReplicationLayer as MyReplicationClient) != null && myReplicationClient.ReplicationRange.HasValue)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_ReducedReplicationRange, check: true);
			}
			if (!MyGameService.Service.GetInstallStatus(out var _))
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_InstallInProgress, check: true);
			}
			if (MyUnsafeGridsSessionComponent.UnsafeGrids.Count > 0)
			{
=======
			{
				DrawPerformanceWarning();
			}
			DrawCameraInfo(MyHud.CameraInfo);
			if (MyHud.VoiceChat.Visible)
			{
				DrawVoiceChat(MyHud.VoiceChat);
			}
			m_hitIndicator.Update();
			return true;
		}

		private void UpdatePerfWarnings()
		{
			List<string> suppressedWarnings = MySession.Static.Settings.SuppressedWarnings;
			if (suppressedWarnings != null && m_suppressedWarnings.get_Count() == 0)
			{
				foreach (string item in suppressedWarnings)
				{
					m_suppressedWarnings.Add(MyStringId.GetOrCompute(item));
				}
			}
			if (!MySandboxGame.Config.EnablePerformanceWarnings)
			{
				return;
			}
			if (MySession.Static.IsRunningExperimental)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode, check: true);
			}
			MyReplicationClient myReplicationClient;
			if ((myReplicationClient = MyMultiplayer.Static?.ReplicationLayer as MyReplicationClient) != null && myReplicationClient.ReplicationRange.HasValue)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_ReducedReplicationRange, check: true);
			}
			if (!MyGameService.Service.GetInstallStatus(out var _))
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_InstallInProgress, check: true);
			}
			if (MyUnsafeGridsSessionComponent.UnsafeGrids.Count > 0)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				AddWarning(MyCommonTexts.PerformanceWarningHeading_UnsafeGrids, check: true);
			}
			foreach (KeyValuePair<MySimpleProfiler.MySimpleProfilingBlock, MySimpleProfiler.PerformanceWarning> currentWarning in MySimpleProfiler.CurrentWarnings)
			{
				if (m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading))
				{
					break;
				}
				if (currentWarning.Value.Time < 120)
				{
					AddWarning(MyCommonTexts.PerformanceWarningHeading, check: false);
					break;
				}
			}
			if (MyGeneralStats.Static.LowNetworkQuality || !MySession.Static.MultiplayerDirect || (!MySession.Static.MultiplayerAlive && !MySession.Static.ServerSaving) || (!Sync.IsServer && MySession.Static.MultiplayerPing.Milliseconds > 250.0))
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_Connection, check: true);
			}
			if (!MySession.Static.HighSimulationQualityNotification)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_SimSpeed, check: true);
			}
			if (MySession.Static.ServerSaving)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_Saving, check: true);
			}
			if (MyPlatformGameSettings.PUBLIC_BETA_MP_TEST)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_ExperimentalBetaBuild, check: true);
			}
			if (!MyGameService.IsOnline)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_SteamOffline, check: true);
			}
			if (MySession.Static.GetComponent<MySessionComponentDLC>().UsedUnownedDLCs.Count > 0)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_PaidContent, check: true);
			}
			if (MySession.Static.MultiplayerLastMsg > 1.0)
			{
				AddWarning(MyCommonTexts.PerformanceWarningHeading_Connection, check: true);
			}
			void AddWarning(MyStringId id, bool check)
			{
				if (!m_suppressedWarnings.Contains(id) && (!check || !m_warningNotifications.Contains(id)))
				{
					m_warningNotifications.Add(id);
				}
<<<<<<< HEAD
			}
		}

		private void DrawAsync()
		{
			MyRenderProxy.BeginRecordingDeferredMessages();
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				foreach (MyStatControls statControl in m_statControls)
				{
					statControl.Draw(m_transitionAlpha, m_backgroundTransition);
				}
				DrawTexts();
			}
			if ((!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud) || MyPetaInputComponent.SHOW_HUD_ALWAYS)
			{
				m_markerRender.Draw();
			}
			if (!MyHud.IsHudMinimal)
			{
				MyHud.Notifications.Draw();
			}
			m_drawAsyncMessages = MyRenderProxy.FinishRecordingDeferredMessages();
		}

		private static void DrawIdentification()
		{
			if (!MyGameService.IsActive || !MyGameService.IsOnline)
			{
				return;
			}
			ulong userId = MyGameService.UserId;
			if (userId == 76561198005765400L)
			{
				return;
			}
			string text = "-" + userId + "-";
			Vector2 vector = MyGuiManager.MeasureString("GameCredits", text, 1f);
			vector.Y *= 1.25f;
			for (float num = 0f; num < 1f; num += vector.X)
			{
				for (float num2 = 0f; num2 < 1f; num2 += vector.Y)
				{
					MyGuiManager.DrawString("GameCredits", text, new Vector2(num, num2), 1f, new Color(Color.Red, 0.005f), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: true);
				}
			}
=======
			}
		}

		private void DrawAsync()
		{
			MyRenderProxy.BeginRecordingDeferredMessages();
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				foreach (MyStatControls statControl in m_statControls)
				{
					statControl.Draw(m_transitionAlpha, m_backgroundTransition);
				}
				DrawTexts();
			}
			if ((!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud) || MyPetaInputComponent.SHOW_HUD_ALWAYS)
			{
				m_markerRender.Draw();
			}
			if (!MyHud.IsHudMinimal)
			{
				MyHud.Notifications.Draw();
			}
			m_drawAsyncMessages = MyRenderProxy.FinishRecordingDeferredMessages();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override bool Update(bool hasFocus)
		{
			RefreshRotatingWheel();
			return base.Update(hasFocus);
		}

		private void AsyncUpdate(bool startTask = true)
		{
			m_markerRender.Update();
			if ((!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud) || MyPetaInputComponent.SHOW_HUD_ALWAYS)
			{
				if (MyHud.SinkGroupInfo.Visible && MyFakes.LEGACY_HUD)
				{
					DrawPowerGroupInfo(MyHud.SinkGroupInfo);
				}
				if (MyHud.LocationMarkers.Visible)
				{
					m_markerRender.DrawLocationMarkers(MyHud.LocationMarkers);
				}
				if (MyHud.GpsMarkers.Visible && MyFakes.ENABLE_GPS)
				{
					DrawGpsMarkers(MyHud.GpsMarkers);
				}
				if (MyHud.ButtonPanelMarkers.Visible)
				{
					DrawButtonPanelMarkers(MyHud.ButtonPanelMarkers);
				}
				if (MyHud.OreMarkers.Visible)
				{
					DrawOreMarkers(MyHud.OreMarkers);
				}
				if (MyHud.LargeTurretTargets.Visible)
				{
					DrawLargeTurretTargets(MyHud.LargeTurretTargets);
				}
				DrawWorldBorderIndicator(MyHud.WorldBorderChecker);
				if (MyHud.HackingMarkers.Visible)
				{
					DrawHackingMarkers(MyHud.HackingMarkers);
				}
<<<<<<< HEAD
				if (MyHud.OffscreenTargetMarker.Visible)
				{
					m_markerRender.AddOffscreenTarget(MyHud.OffscreenTargetMarker.Position, MyHud.OffscreenTargetMarker.TargetPlayerRelation);
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_gravityIndicator.Draw(m_transitionAlpha);
			}
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				UpdatePerfWarnings();
			}
			if (!MyHud.IsHudMinimal)
			{
				MyHud.Notifications.Update();
			}
			DrawDamageIndicators(m_damageIndicators);
<<<<<<< HEAD
			DrawTargetInfo();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (startTask && !(m_transitionAlpha < 1f) && MyHud.IsVisible)
			{
				m_hudTask?.WaitOrExecute();
				m_hudTask = Parallel.Start(DrawAsync, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "HUD"), null, WorkPriority.VeryHigh);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenHudSpace";
		}

		/// <summary>
		/// Return position on middle screen based on real desired position on gamescreen.
		/// "Middle" make sense only for tripple monitors. For every else is middle screen all screen.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static Vector2 GetRealPositionOnCenterScreen(Vector2 value)
		{
			Vector2 result = ((!MyGuiManager.FullscreenHudEnabled) ? MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(value) : MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(value));
			if (MyVideoSettingsManager.IsTripleHead())
			{
				result.X += 1f;
			}
			return result;
		}

		public void SetToolbarVisible(bool visible)
		{
			if (m_toolbarControl != null)
			{
				m_toolbarControl.Visible = visible;
				m_hiddenToolbar = !visible;
			}
		}

		private void DrawVoiceChat(MyHudVoiceChat voiceChat)
		{
			MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			MyGuiPaddedTexture tEXTURE_VOICE_CHAT = MyGuiConstants.TEXTURE_VOICE_CHAT;
			Vector2 hudPos = new Vector2(0.01f, 0.99f);
			Vector2 normalizedCoord = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			MyGuiManager.DrawSpriteBatch(tEXTURE_VOICE_CHAT.Texture, normalizedCoord, tEXTURE_VOICE_CHAT.SizeGui, Color.White, drawAlign);
		}

		private void DrawPowerGroupInfo(MyHudSinkGroupInfo info)
		{
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			float num = -0.25f / ((float)safeFullscreenRectangle.Width / (float)safeFullscreenRectangle.Height);
			Vector2 hudPos = new Vector2(0.985f, 0.65f);
			Vector2 hudPos2 = new Vector2(hudPos.X + num, hudPos.Y);
			Vector2 valuesBottomRight = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			Vector2 namesBottomLeft = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
			info.Data.DrawBottomUp(namesBottomLeft, valuesBottomRight, m_textScale);
		}

		private float FindDistanceToNearestPlanetSeaLevel(BoundingBoxD worldBB, out MyPlanet closestPlanet)
		{
			closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref worldBB);
			double num = double.MaxValue;
			if (closestPlanet != null)
			{
				num = (worldBB.Center - closestPlanet.PositionComp.GetPosition()).Length() - (double)closestPlanet.AverageRadius;
			}
			return (float)num;
		}

		private void DrawArtificialHorizonAndAltitude()
		{
			MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
			if (myCubeBlock == null || myCubeBlock.CubeGrid.Physics == null)
			{
				return;
			}
			Vector3D globalPos = myCubeBlock.CubeGrid.Physics.CenterOfMassWorld;
			Vector3D centerOfMassWorld = myCubeBlock.GetTopMostParent().Physics.CenterOfMassWorld;
			MyShipController myShipController = myCubeBlock as MyShipController;
			if (myShipController != null && !myShipController.HorizonIndicatorEnabled)
			{
				return;
			}
			FindDistanceToNearestPlanetSeaLevel(myCubeBlock.PositionComp.WorldAABB, out var closestPlanet);
			if (closestPlanet != null)
			{
				float num = (float)Vector3D.Distance(closestPlanet.GetClosestSurfacePointGlobal(ref globalPos), globalPos);
				string font = "Blue";
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				float num2 = num;
				if (Math.Abs(num2 - m_altitude) > 500f && myCubeBlock.CubeGrid.GridSystems.GasSystem != null)
				{
					myCubeBlock.CubeGrid.GridSystems.GasSystem.OnAltitudeChanged();
					m_altitude = num2;
				}
				StringBuilder stringBuilder = new StringBuilder().AppendDecimal(num2, 0).Append(" m");
				float num3 = 0.03f;
				int num4 = MyGuiManager.GetFullscreenRectangle().Width / MyGuiManager.GetSafeFullscreenRectangle().Width;
				int num5 = MyGuiManager.GetFullscreenRectangle().Height / MyGuiManager.GetSafeFullscreenRectangle().Height;
				Vector2 normalizedCoord = new Vector2(MyHud.Crosshair.Position.X * (float)num4 / MyGuiManager.GetHudSize().X, MyHud.Crosshair.Position.Y * (float)num5 / MyGuiManager.GetHudSize().Y + num3);
				if (MyVideoSettingsManager.IsTripleHead())
				{
					normalizedCoord.X -= 1f;
				}
				MyGuiManager.DrawString(font, stringBuilder.ToString(), normalizedCoord, m_textScale, null, drawAlign, useFullClientArea: true);
				Vector3 v = -closestPlanet.Components.Get<MyGravityProviderComponent>().GetWorldGravity(centerOfMassWorld);
				v.Normalize();
				double num6 = v.Dot(myCubeBlock.WorldMatrix.Forward);
				float num7 = 0.4f;
				Vector2 vector = MyHud.Crosshair.Position / MyGuiManager.GetHudSize() * new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
				MyGuiPaddedTexture tEXTURE_HUD_GRAVITY_HORIZON = MyGuiConstants.TEXTURE_HUD_GRAVITY_HORIZON;
				float num8 = ((MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator) ? (0.35f * MySector.MainCamera.Viewport.Height) : (0.45f * MySector.MainCamera.Viewport.Height));
				double num9 = num6 * (double)num8;
				Vector2D vector2D = new Vector2D(myCubeBlock.WorldMatrix.Right.Dot(v), myCubeBlock.WorldMatrix.Up.Dot(v));
				float num10 = ((vector2D.LengthSquared() > 9.9999997473787516E-06) ? ((float)Math.Atan2(vector2D.Y, vector2D.X)) : 0f);
				Vector2 vector2 = tEXTURE_HUD_GRAVITY_HORIZON.SizePx * num7;
				RectangleF destination = new RectangleF(vector - vector2 * 0.5f + new Vector2(0f, (float)num9), vector2);
				Rectangle? sourceRectangle = null;
				Vector2 rightVector = new Vector2((float)Math.Sin(num10), (float)Math.Cos(num10));
				Vector2 origin = vector;
				MyRenderProxy.DrawSpriteExt(tEXTURE_HUD_GRAVITY_HORIZON.Texture, ref destination, sourceRectangle, Color.White, ref rightVector, ref origin, ignoreBounds: false, waitTillLoaded: true);
			}
		}

		internal void ActivateHitIndicator(MySession.MyHitIndicatorTarget hitTarget)
		{
			m_hitIndicator.Hit(hitTarget);
		}

		private void DrawObjectiveLine(MyHudObjectiveLine objective)
		{
			MyGuiDrawAlignEnum myGuiDrawAlignEnum = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			Color aliceBlue = Color.AliceBlue;
			Vector2 hudPos = new Vector2(0.45f, 0.01f);
			Vector2 vector = new Vector2(0f, 0.02f);
			Vector2 vector2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			string title = objective.Title;
			Vector2 normalizedCoord = vector2;
			MyGuiDrawAlignEnum drawAlign = myGuiDrawAlignEnum;
			MyGuiManager.DrawString("Debug", title, normalizedCoord, 1f, aliceBlue, drawAlign);
			hudPos += vector;
			vector2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			MyGuiManager.DrawString("Debug", "- " + objective.CurrentObjective, vector2, 1f, null, myGuiDrawAlignEnum);
		}

		private void DrawGpsMarkers(MyHudGpsMarkers gpsMarkers)
		{
			m_tmpHudEntityParams.FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL;
			MySession.Static.Gpss.updateForHud();
			foreach (MyGps markerEntity in gpsMarkers.MarkerEntities)
			{
				m_markerRender.AddGPS(markerEntity);
			}
		}

		private void DrawDamageIndicators(List<MyDamageIndicator> damageIndicators)
		{
			List<MyDamageIndicator> list = new List<MyDamageIndicator>();
			Vector2I vector2I = new Vector2I(128, 128);
			Vector2I vector2I2 = new Vector2I(22, 38);
			Rectangle value = new Rectangle(vector2I.X - vector2I2.X, (vector2I.Y - vector2I2.Y) / 2, vector2I2.X, vector2I2.Y);
			Vector2I vector2I3 = new Vector2I(MyGuiManager.GetSafeFullscreenRectangle().Width / 2, MyGuiManager.GetSafeFullscreenRectangle().Height / 2);
			int num = vector2I3.Y / 3;
			foreach (MyDamageIndicator damageIndicator in damageIndicators)
			{
				TimeSpan timeSpan = MySession.Static.ElapsedGameTime - damageIndicator.IndicatorCreationTime;
				if (timeSpan <= DAMAGE_INDICATOR_VISIBILITY_TIME)
				{
<<<<<<< HEAD
					double totalMilliseconds = timeSpan.TotalMilliseconds;
					TimeSpan dAMAGE_INDICATOR_VISIBILITY_TIME = DAMAGE_INDICATOR_VISIBILITY_TIME;
					float a = (float)(1.0 - EaseInBack(totalMilliseconds / dAMAGE_INDICATOR_VISIBILITY_TIME.TotalMilliseconds)) * 0.8f;
=======
					float a = (float)(1.0 - EaseInBack(timeSpan.TotalMilliseconds / DAMAGE_INDICATOR_VISIBILITY_TIME.TotalMilliseconds)) * 0.8f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Vector2 screenPosition = damageIndicator.ScreenPosition;
					float rotation = MyMath.ArcTanAngle(screenPosition.X, screenPosition.Y);
					screenPosition = vector2I3 + screenPosition * num;
					RectangleF destination = new RectangleF(screenPosition - vector2I2 / 2, vector2I2);
					MyRenderProxy.DrawSprite("Textures\\GUI\\Indicators\\DamageIndicator03.png", ref destination, value, new Color(Color.Red, a), rotation, ignoreBounds: false, waitTillLoaded: false);
				}
				else
				{
					list.Add(damageIndicator);
				}
			}
			foreach (MyDamageIndicator item in list)
			{
				damageIndicators.Remove(item);
			}
		}

<<<<<<< HEAD
		private void DrawTargetInfo()
		{
			MyTargetLockingComponent myTargetLockingComponent = MySession.Static.LocalCharacter?.TargetLockingComp;
			MyTargetFocusComponent myTargetFocusComponent = MySession.Static.LocalCharacter?.TargetFocusComp;
			bool flag = (MySession.Static.ControlledEntity as IMyTargetingCapableBlock)?.IsShipToolSelected() ?? false;
			if (myTargetLockingComponent == null || myTargetFocusComponent == null || flag)
			{
				m_markerRender.SetTarget(null, MyHudMarkerRender.MyTargetLockingState.None);
			}
			else if (myTargetLockingComponent.Target != null)
			{
				MyCubeGrid target = myTargetLockingComponent.Target;
				if (target.MarkedForClose)
				{
					m_markerRender.SetTarget(null, MyHudMarkerRender.MyTargetLockingState.Focused);
					return;
				}
				if (!myTargetLockingComponent.IsTargetLocked)
				{
					m_markerRender.SetTarget(target, MyHudMarkerRender.MyTargetLockingState.Locking, myTargetLockingComponent.LockingProgressPercent);
					return;
				}
				MyHudMarkerRender.MyTargetLockingState state = (myTargetLockingComponent.IsLosingLock ? MyHudMarkerRender.MyTargetLockingState.LosingLock : MyHudMarkerRender.MyTargetLockingState.Locked);
				m_markerRender.SetTarget(target, state, myTargetLockingComponent.LockingProgressPercent);
			}
			else
			{
				MyCubeGrid myCubeGrid = myTargetFocusComponent?.CurrentTarget as MyCubeGrid;
				if (myCubeGrid == null || myCubeGrid.MarkedForClose)
				{
					m_markerRender.SetTarget(null, MyHudMarkerRender.MyTargetLockingState.None);
				}
				else
				{
					m_markerRender.SetTarget(myCubeGrid, MyHudMarkerRender.MyTargetLockingState.Focused);
				}
			}
		}

		/// <summary>
		/// https://easings.net/en#easeInBack
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private double EaseInBack(double x)
		{
			double num = 1.7015800476074219;
			return (num + 1.0) * x * x * x - num * x * x;
		}

<<<<<<< HEAD
		/// <summary>i
		/// Tries to compute the screenpoint for this POI from the main camera's PoV. May fail if the projection is invalid.
		/// projectedPoint2D will be set to Vector2.Zero if it was not possible to project.
		/// </summary>
		/// <param name="worldPosition">The world position to project to the screen.</param>
		/// <param name="projectedPoint2D">The screen position [-1, 1] by [-1, 1]</param>        
		/// <returns>True if it could project, false otherwise.</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool TryComputeScreenPoint(Vector3D worldPosition, out Vector2 projectedPoint2D)
		{
			if (MySession.Static.LocalCharacter == null)
			{
				projectedPoint2D = Vector2.Zero;
				return false;
			}
			MatrixD matrix = MySession.Static.LocalCharacter.GetSpineInvertedWorldMatrix();
			MatrixD.Invert(ref matrix);
			MatrixD m;
			if (!MySession.Static.LocalCharacter.IsInFirstPersonView)
			{
				m = MatrixD.Invert(MySector.MainCamera.ViewMatrix);
				m.Translation = matrix.Translation;
			}
			else
			{
				m = MySession.Static.LocalCharacter.WorldMatrix;
				m.Translation = matrix.Translation;
			}
			Matrix m2 = Matrix.Invert(m);
			m = m2;
			Vector3D vector3D = Vector3D.Transform(worldPosition, m);
			double x = vector3D.X;
			double z = vector3D.Z;
			projectedPoint2D = new Vector2((float)x, (float)z);
			projectedPoint2D.Normalize();
			return true;
		}

		private void DrawButtonPanelMarkers(MyHudGpsMarkers buttonPanelMarkers)
		{
			foreach (MyGps markerEntity in buttonPanelMarkers.MarkerEntities)
			{
				m_markerRender.AddButtonMarker(markerEntity.Coords, markerEntity.Name);
			}
		}

		private void DrawOreMarkers(MyHudOreMarkers oreMarkers)
		{
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			if (m_nearestOreDeposits == null || m_nearestOreDeposits.Length < MyDefinitionManager.Static.VoxelMaterialCount)
			{
				m_nearestOreDeposits = new MyTuple<Vector3D, MyEntityOreDeposit>[MyDefinitionManager.Static.VoxelMaterialCount];
				m_nearestDistanceSquared = new float[m_nearestOreDeposits.Length];
			}
			for (int i = 0; i < m_nearestOreDeposits.Length; i++)
			{
				m_nearestOreDeposits[i] = default(MyTuple<Vector3D, MyEntityOreDeposit>);
				m_nearestDistanceSquared[i] = float.MaxValue;
			}
			Vector3D vector3D = Vector3D.Zero;
			if (MySession.Static != null && MySession.Static.ControlledEntity != null)
			{
				vector3D = (MySession.Static.ControlledEntity as MyEntity).WorldMatrix.Translation;
			}
			Enumerator<MyEntityOreDeposit> enumerator = oreMarkers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					MyEntityOreDeposit.Data data = oreMarker.Materials[j];
					MyVoxelMaterialDefinition material = data.Material;
					data.ComputeWorldPosition(oreMarker.VoxelMap, out var oreWorldPosition);
					float num = (float)(vector3D - oreWorldPosition).LengthSquared();
					float num2 = m_nearestDistanceSquared[material.Index];
					if (num < num2)
=======
					MyEntityOreDeposit current = enumerator.get_Current();
					for (int j = 0; j < current.Materials.Count; j++)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyEntityOreDeposit.Data data = current.Materials[j];
						MyVoxelMaterialDefinition material = data.Material;
						data.ComputeWorldPosition(current.VoxelMap, out var oreWorldPosition);
						float num = (float)(vector3D - oreWorldPosition).LengthSquared();
						float num2 = m_nearestDistanceSquared[material.Index];
						if (num < num2)
						{
							m_nearestOreDeposits[material.Index] = MyTuple.Create(oreWorldPosition, current);
							m_nearestDistanceSquared[material.Index] = num;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			for (int k = 0; k < m_nearestOreDeposits.Length; k++)
			{
				MyTuple<Vector3D, MyEntityOreDeposit> myTuple = m_nearestOreDeposits[k];
				if (myTuple.Item2 != null && myTuple.Item2.VoxelMap != null && !myTuple.Item2.VoxelMap.Closed)
				{
					MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte)k);
					m_markerRender.AddOre(myTuple.Item1, oreMarkers.GetOreName(voxelMaterialDefinition));
				}
			}
		}

		private void DrawCameraInfo(MyHudCameraInfo cameraInfo)
		{
			cameraInfo.Draw(m_cameraInfoMultilineControl);
		}

		private void DrawLargeTurretTargets(MyHudLargeTurretTargets largeTurretTargets)
		{
			foreach (KeyValuePair<MyEntity, MyHudEntityParams> target in largeTurretTargets.Targets)
			{
				MyHudEntityParams value = target.Value;
				if (value.ShouldDraw == null || value.ShouldDraw())
				{
					m_markerRender.AddTarget(target.Key.PositionComp.WorldAABB.Center);
				}
			}
		}

		private void DrawWorldBorderIndicator(MyHudWorldBorderChecker checker)
		{
			if (checker.WorldCenterHintVisible)
			{
				m_markerRender.AddPOI(Vector3D.Zero, MyHudWorldBorderChecker.HudEntityParams.Text, MyRelationsBetweenPlayerAndBlock.Enemies);
			}
		}

		private void DrawHackingMarkers(MyHudHackingMarkers hackingMarkers)
		{
			try
			{
				hackingMarkers.UpdateMarkers();
				if (MySandboxGame.TotalTimeInMilliseconds % 200 > 100)
<<<<<<< HEAD
				{
					return;
				}
				foreach (KeyValuePair<long, MyHudEntityParams> markerEntity in hackingMarkers.MarkerEntities)
				{
=======
				{
					return;
				}
				foreach (KeyValuePair<long, MyHudEntityParams> markerEntity in hackingMarkers.MarkerEntities)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyHudEntityParams value = markerEntity.Value;
					if (value.ShouldDraw == null || value.ShouldDraw())
					{
						m_markerRender.AddHacking(markerEntity.Value.Position, value.Text);
					}
				}
			}
			finally
			{
			}
		}

		private void DrawPerformanceWarning()
		{
			Vector2 vector = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 4, 42);
			vector -= new Vector2(MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE.SizeGui.X / 1.5f, 0f);
			MyGuiPaddedTexture tEXTURE_HUD_BG_PERFORMANCE = MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE;
			MyGuiManager.DrawSpriteBatch(tEXTURE_HUD_BG_PERFORMANCE.Texture, vector, tEXTURE_HUD_BG_PERFORMANCE.SizeGui / 1.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			string text = MyTexts.GetString(m_warningNotifications[0]);
			if (m_warningNotifications[0] == MyCommonTexts.PerformanceWarningHeading_SteamOffline)
			{
				text = string.Format(text, MyGameService.Service.ServiceName);
			}
			MyGuiManager.DrawString("White", text, vector + new Vector2(0.09f, -0.011f), 0.7f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("White", MyInput.Static.IsJoystickLastUsed ? string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningCombinationGamepad), MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_BASE, MyControlsSpace.WARNING_SCREEN)) : string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningCombination), MyGuiSandbox.GetKeyName(MyControlsSpace.HELP_SCREEN)), vector + new Vector2(0.09f, 0.018f), 0.6f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("White", $"({m_warningNotifications.Count})", vector + new Vector2(0.177f, -0.023f), 0.55f, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			if (m_currentFrameCount < m_warningFrameCount)
			{
				m_currentFrameCount++;
				return;
			}
			m_currentFrameCount = 0;
			m_warningNotifications.RemoveAt(0);
		}

		protected override void OnHide()
		{
			base.OnHide();
			if (MyHud.VoiceChat.Visible)
			{
				MyHud.VoiceChat.Hide();
			}
		}
	}
}
