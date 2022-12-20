using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsAudio : MyGuiScreenBase
	{
		private class MyGuiScreenOptionsAudioSettings
		{
			public float GameVolume;

			public float MusicVolume;

			public float VoiceChatVolume;

			public float MicSensitivity;

			public bool HudWarnings;

			public bool EnableVoiceChat;

			public bool EnableMuteWhenNotInFocus;

			public bool EnableDynamicMusic;

			public bool EnableReverb;

			public bool ShipSoundsAreBasedOnSpeed;

			public bool EnableDoppler;

			public bool PushToTalk;

			public override bool Equals(object obj)
			{
				if (obj.GetType() != typeof(MyGuiScreenOptionsAudioSettings))
				{
					return false;
				}
				MyGuiScreenOptionsAudioSettings myGuiScreenOptionsAudioSettings = (MyGuiScreenOptionsAudioSettings)obj;
				if (GameVolume != myGuiScreenOptionsAudioSettings.GameVolume || MusicVolume != myGuiScreenOptionsAudioSettings.MusicVolume || VoiceChatVolume != myGuiScreenOptionsAudioSettings.VoiceChatVolume || HudWarnings != myGuiScreenOptionsAudioSettings.HudWarnings || EnableVoiceChat != myGuiScreenOptionsAudioSettings.EnableVoiceChat || EnableMuteWhenNotInFocus != myGuiScreenOptionsAudioSettings.EnableMuteWhenNotInFocus || EnableDynamicMusic != myGuiScreenOptionsAudioSettings.EnableDynamicMusic || EnableReverb != myGuiScreenOptionsAudioSettings.EnableReverb || ShipSoundsAreBasedOnSpeed != myGuiScreenOptionsAudioSettings.ShipSoundsAreBasedOnSpeed || EnableDoppler != myGuiScreenOptionsAudioSettings.EnableDoppler || PushToTalk != myGuiScreenOptionsAudioSettings.PushToTalk)
				{
					return false;
				}
				return true;
			}
<<<<<<< HEAD

			public override int GetHashCode()
			{
				return GameVolume.GetHashCode() ^ MusicVolume.GetHashCode() ^ VoiceChatVolume.GetHashCode() ^ HudWarnings.GetHashCode() ^ EnableVoiceChat.GetHashCode() ^ EnableMuteWhenNotInFocus.GetHashCode() ^ EnableDynamicMusic.GetHashCode() ^ EnableReverb.GetHashCode() ^ ShipSoundsAreBasedOnSpeed.GetHashCode() ^ EnableDoppler.GetHashCode() ^ PushToTalk.GetHashCode();
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private MyGuiControlSlider m_gameVolumeSlider;

		private MyGuiControlSlider m_musicVolumeSlider;

		private MyGuiControlSlider m_voiceChatVolumeSlider;

		private MyGuiControlSlider m_voiceMicSensitivitySlider;

		private MyGuiControlCheckbox m_hudWarnings;

		private MyGuiControlCheckbox m_enableVoiceChat;

		private MyGuiControlCheckbox m_enableMuteWhenNotInFocus;

		private MyGuiControlCheckbox m_enableDynamicMusic;

		private MyGuiControlCheckbox m_enableReverb;

		private MyGuiControlCheckbox m_enableDoppler;

		private MyGuiControlCheckbox m_shipSoundsAreBasedOnSpeed;

		private MyGuiControlCheckbox m_pushToTalk;

		private MyGuiScreenOptionsAudioSettings m_settingsOld = new MyGuiScreenOptionsAudioSettings();

		private MyGuiScreenOptionsAudioSettings m_settingsNew = new MyGuiScreenOptionsAudioSettings();

		private bool m_gameAudioPausedWhenOpen;

		private MyGuiControlElementGroup m_elementGroup;

		private MyGuiControlButton m_buttonOk;

		private MyGuiControlButton m_buttonCancel;

		private bool m_isLimitedMenu;

		public MyGuiScreenOptionsAudio(bool isLimitedMenu = false)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, GetScreenSize(isLimitedMenu), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_isLimitedMenu = isLimitedMenu;
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			if (!constructor)
			{
				return;
			}
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			AddCaption(MyCommonTexts.ScreenCaptionAudioOptions, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiDrawAlignEnum originAlign2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Vector2 vector = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num2 = 25f;
			float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
			float num3 = 0.0015f;
			Vector2 vector3 = new Vector2(0f, 0.045f);
			float num4 = 0f;
			Vector2 vector4 = new Vector2(0f, 0.008f);
			Vector2 vector5 = (m_size.Value / 2f - vector) * new Vector2(-1f, -1f) + new Vector2(0f, y);
			Vector2 vector6 = (m_size.Value / 2f - vector) * new Vector2(1f, -1f) + new Vector2(0f, y);
			Vector2 vector7 = (m_size.Value / 2f - vector2) * new Vector2(0f, 1f);
			Vector2 vector8 = new Vector2(vector6.X - (num + num3), vector6.Y);
			num4 -= 0.045f;
			MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MusicVolume))
			{
				Position = vector5 + num4 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_musicVolumeSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_MusicVolume), defaultValue: MySandboxGame.Config.MusicVolume)
			{
				Position = vector6 + num4 * vector3,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			m_musicVolumeSlider.ValueChanged = OnMusicVolumeChange;
			num4 += 1.08f;
			MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.GameVolume))
			{
				Position = vector5 + num4 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_gameVolumeSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_SoundVolume), defaultValue: MySandboxGame.Config.GameVolume)
			{
				Position = vector6 + num4 * vector3,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			m_gameVolumeSlider.ValueChanged = OnGameVolumeChange;
			num4 += 1.08f;
			MyGuiControlLabel myGuiControlLabel = null;
			MyGuiControlLabel control3 = null;
			MyGuiControlLabel control4 = null;
			MyGuiControlLabel control5 = null;
			if (MyPerGameSettings.VoiceChatEnabled)
			{
				num4 += 0.29f;
				if (!m_isLimitedMenu)
				{
					myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.EnableVoiceChat))
					{
						Position = vector5 + num4 * vector3 + vector4,
						OriginAlign = originAlign
					};
					m_enableVoiceChat = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_EnableVoiceChat))
					{
						Position = vector8 + num4 * vector3,
						OriginAlign = originAlign
					};
					m_enableVoiceChat.IsCheckedChanged = VoiceChatChecked;
					num4 += 1f;
				}
				control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.VoiceChatVolume))
				{
					Position = vector5 + num4 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_voiceChatVolumeSlider = new MyGuiControlSlider(null, 0f, 5f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_VoiceChatVolume), defaultValue: MySandboxGame.Config.VoiceChatVolume)
				{
					Position = vector6 + num4 * vector3,
					OriginAlign = originAlign2,
					Size = new Vector2(num, 0f)
				};
				m_voiceChatVolumeSlider.ValueChanged = OnVoiceChatVolumeChange;
				num4 += 1.08f;
				control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.PushToTalk))
				{
					Position = vector5 + num4 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_pushToTalk = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.PushToTalk))
				{
					Position = vector8 + num4 * vector3,
					OriginAlign = originAlign
				};
				m_pushToTalk.IsCheckedChanged = PushToTalkChecked;
				num4 += 1f;
				if (!MyGameService.IsMicrophoneFilteringSilence())
				{
					control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MicSensitivity))
					{
						Position = vector5 + num4 * vector3 + vector4,
						OriginAlign = originAlign
					};
					m_voiceMicSensitivitySlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.MicSensitivity), defaultValue: MySandboxGame.Config.MicSensitivity)
					{
						Position = vector6 + num4 * vector3,
						OriginAlign = originAlign2,
						Size = new Vector2(num, 0f)
					};
					m_voiceMicSensitivitySlider.ValueChanged = OnVoiceMicSensitivityChange;
					num4 += 1.08f;
				}
			}
			MyGuiControlLabel control6 = null;
			if (!m_isLimitedMenu)
			{
				control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MuteWhenNotInFocus))
				{
					Position = vector5 + num4 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_enableMuteWhenNotInFocus = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_MuteWhenInactive))
				{
					Position = vector8 + num4 * vector3,
					OriginAlign = originAlign
				};
				m_enableMuteWhenNotInFocus.IsCheckedChanged = EnableMuteWhenNotInFocusChecked;
				num4 += 1f;
			}
			MyGuiControlLabel control7 = null;
			if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
			{
				control7 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_EnableReverb))
				{
					Position = vector5 + num4 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_enableReverb = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipAudioOptionsEnableReverb))
				{
					Position = vector8 + num4 * vector3,
					OriginAlign = originAlign
				};
				m_enableReverb.IsCheckedChanged = EnableReverbChecked;
				m_enableReverb.Enabled = MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
				m_enableReverb.IsChecked = MyAudio.Static.EnableReverb && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
				num4 += 1f;
			}
			MyGuiControlLabel control8 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_EnableDoppler))
			{
				Position = vector5 + num4 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_enableDoppler = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipAudioOptionsEnableDoppler))
			{
				Position = vector8 + num4 * vector3,
				OriginAlign = originAlign
			};
			m_enableDoppler.IsCheckedChanged = EnableDopplerChecked;
			m_enableDoppler.Enabled = true;
			m_enableDoppler.IsChecked = MyAudio.Static.EnableDoppler;
			num4 += 1f;
			MyGuiControlLabel control9 = null;
			if (MyPerGameSettings.EnableShipSoundSystem)
			{
				control9 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_ShipSoundsBasedOnSpeed), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, 0.253f, isAutoScaleEnabled: true)
				{
					Position = vector5 + num4 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_shipSoundsAreBasedOnSpeed = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_SpeedBasedSounds))
				{
					Position = vector8 + num4 * vector3,
					OriginAlign = originAlign
				};
				m_shipSoundsAreBasedOnSpeed.IsCheckedChanged = ShipSoundsAreBasedOnSpeedChecked;
				num4 += 1f;
			}
			MyGuiControlLabel control10 = null;
			if (MyPerGameSettings.UseMusicController)
			{
				control10 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.AudioSettings_UseMusicController), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, 0.253f, isAutoScaleEnabled: true)
				{
					Position = vector5 + num4 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_enableDynamicMusic = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_UseContextualMusic))
				{
					Position = vector8 + num4 * vector3,
					OriginAlign = originAlign
				};
				num4 += 1f;
			}
			MyGuiControlLabel control11 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.HudWarnings))
			{
				Position = vector5 + num4 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_hudWarnings = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsAudio_HudWarnings))
			{
				Position = vector8 + num4 * vector3,
				OriginAlign = originAlign
			};
			m_hudWarnings.IsCheckedChanged = HudWarningsChecked;
			num4 += 1f;
			m_buttonOk = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkClick);
			m_buttonOk.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			m_buttonOk.Position = vector7 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonOk.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_buttonCancel = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnCancelClick);
			m_buttonCancel.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_buttonCancel.Position = vector7 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonCancel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(control2);
			Controls.Add(m_gameVolumeSlider);
			Controls.Add(control);
			Controls.Add(m_musicVolumeSlider);
			Controls.Add(control11);
			Controls.Add(m_hudWarnings);
			if (!m_isLimitedMenu)
			{
				Controls.Add(control6);
				Controls.Add(m_enableMuteWhenNotInFocus);
			}
			if (MyPerGameSettings.UseMusicController)
			{
				Controls.Add(control10);
				Controls.Add(m_enableDynamicMusic);
			}
			if (MyPerGameSettings.EnableShipSoundSystem)
			{
				Controls.Add(control9);
				Controls.Add(m_shipSoundsAreBasedOnSpeed);
			}
			if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
			{
				Controls.Add(control7);
				Controls.Add(m_enableReverb);
			}
			Controls.Add(control8);
			Controls.Add(m_enableDoppler);
			if (MyPerGameSettings.VoiceChatEnabled)
			{
				if (myGuiControlLabel != null)
				{
					Controls.Add(myGuiControlLabel);
				}
				if (m_enableVoiceChat != null)
				{
					Controls.Add(m_enableVoiceChat);
				}
				if (m_voiceMicSensitivitySlider != null)
				{
					Controls.Add(m_voiceMicSensitivitySlider);
					Controls.Add(control5);
				}
				Controls.Add(m_pushToTalk);
				Controls.Add(control3);
				Controls.Add(m_voiceChatVolumeSlider);
				Controls.Add(control4);
			}
			Controls.Add(m_buttonOk);
			m_elementGroup.Add(m_buttonOk);
			Controls.Add(m_buttonCancel);
			m_elementGroup.Add(m_buttonCancel);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(vector5.X, m_buttonOk.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel2.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel2);
			UpdateFromConfig(m_settingsOld);
			UpdateFromConfig(m_settingsNew);
			UpdateControls(m_settingsOld);
			base.FocusedControl = m_buttonOk;
			base.CloseButtonEnabled = true;
			base.GamepadHelpTextId = MySpaceTexts.AudioOptions_Help_Screen;
			m_gameAudioPausedWhenOpen = MyAudio.Static.GameSoundIsPaused;
			if (m_gameAudioPausedWhenOpen)
			{
				MyAudio.Static.ResumeGameSounds();
			}
		}

		private void VoiceChatChecked(MyGuiControlCheckbox checkbox)
		{
			m_settingsNew.EnableVoiceChat = checkbox.IsChecked;
		}

		private void PushToTalkChecked(MyGuiControlCheckbox checkbox)
		{
			m_settingsNew.PushToTalk = checkbox.IsChecked;
			if (m_voiceMicSensitivitySlider != null)
			{
				m_voiceMicSensitivitySlider.Enabled = !m_settingsNew.PushToTalk;
			}
		}

		private void HudWarningsChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.HudWarnings = obj.IsChecked;
		}

		private void EnableMuteWhenNotInFocusChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableMuteWhenNotInFocus = obj.IsChecked;
		}

		private void EnableDynamicMusicChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableDynamicMusic = obj.IsChecked;
		}

		private void ShipSoundsAreBasedOnSpeedChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.ShipSoundsAreBasedOnSpeed = obj.IsChecked;
		}

		private void EnableReverbChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableReverb = MyFakes.AUDIO_ENABLE_REVERB && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE && obj.IsChecked;
		}

		private void EnableDopplerChecked(MyGuiControlCheckbox obj)
		{
			m_settingsNew.EnableDoppler = obj.IsChecked;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsAudio";
		}

		private void UpdateFromConfig(MyGuiScreenOptionsAudioSettings settings)
		{
			settings.GameVolume = MySandboxGame.Config.GameVolume;
			settings.MusicVolume = MySandboxGame.Config.MusicVolume;
			settings.VoiceChatVolume = MySandboxGame.Config.VoiceChatVolume;
			settings.HudWarnings = MySandboxGame.Config.HudWarnings;
			settings.EnableVoiceChat = MySandboxGame.Config.EnableVoiceChat;
			settings.EnableMuteWhenNotInFocus = MySandboxGame.Config.EnableMuteWhenNotInFocus;
			settings.EnableReverb = MyFakes.AUDIO_ENABLE_REVERB && MySandboxGame.Config.EnableReverb && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
			settings.EnableDynamicMusic = MySandboxGame.Config.EnableDynamicMusic;
			settings.ShipSoundsAreBasedOnSpeed = MySandboxGame.Config.ShipSoundsAreBasedOnSpeed;
			settings.EnableDoppler = MySandboxGame.Config.EnableDoppler;
			settings.MicSensitivity = MySandboxGame.Config.MicSensitivity;
			settings.PushToTalk = !MySandboxGame.Config.AutomaticVoiceChatActivation;
		}

		private void UpdateControls(MyGuiScreenOptionsAudioSettings settings)
		{
			m_gameVolumeSlider.Value = settings.GameVolume;
			m_musicVolumeSlider.Value = settings.MusicVolume;
			m_voiceChatVolumeSlider.Value = settings.VoiceChatVolume;
			m_hudWarnings.IsChecked = settings.HudWarnings;
			if (m_enableVoiceChat != null)
			{
				m_enableVoiceChat.IsChecked = settings.EnableVoiceChat;
			}
			if (m_voiceMicSensitivitySlider != null)
			{
				m_voiceMicSensitivitySlider.Value = settings.MicSensitivity;
				m_voiceMicSensitivitySlider.Enabled = !settings.PushToTalk;
			}
			if (m_pushToTalk != null)
			{
				m_pushToTalk.IsChecked = settings.PushToTalk;
			}
			if (m_enableMuteWhenNotInFocus != null)
			{
				m_enableMuteWhenNotInFocus.IsChecked = settings.EnableMuteWhenNotInFocus;
			}
			if (MyFakes.AUDIO_ENABLE_REVERB)
			{
				m_enableReverb.IsChecked = settings.EnableReverb;
			}
			m_enableDynamicMusic.IsChecked = settings.EnableDynamicMusic;
			m_shipSoundsAreBasedOnSpeed.IsChecked = settings.ShipSoundsAreBasedOnSpeed;
			m_enableDoppler.IsChecked = settings.EnableDoppler;
		}

		private void Save()
		{
			MySandboxGame.Config.GameVolume = MyAudio.Static.VolumeGame;
			MySandboxGame.Config.MusicVolume = MyAudio.Static.VolumeMusic;
			MySandboxGame.Config.VoiceChatVolume = m_voiceChatVolumeSlider.Value;
			MySandboxGame.Config.HudWarnings = m_hudWarnings.IsChecked;
			MySandboxGame.Config.MicSensitivity = MyFakes.VOICE_CHAT_MIC_SENSITIVITY;
			if (m_enableVoiceChat != null)
			{
				MySandboxGame.Config.EnableVoiceChat = m_enableVoiceChat.IsChecked;
			}
			if (m_pushToTalk != null)
			{
				MySandboxGame.Config.AutomaticVoiceChatActivation = !m_pushToTalk.IsChecked;
			}
			if (m_enableMuteWhenNotInFocus != null)
			{
				MySandboxGame.Config.EnableMuteWhenNotInFocus = m_enableMuteWhenNotInFocus.IsChecked;
			}
			MySandboxGame.Config.EnableReverb = MyFakes.AUDIO_ENABLE_REVERB && m_enableReverb.IsChecked && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE;
			MyAudio.Static.EnableReverb = MySandboxGame.Config.EnableReverb;
			MySandboxGame.Config.EnableDynamicMusic = m_enableDynamicMusic.IsChecked;
			MySandboxGame.Config.ShipSoundsAreBasedOnSpeed = m_shipSoundsAreBasedOnSpeed.IsChecked;
			MySandboxGame.Config.EnableDoppler = m_enableDoppler.IsChecked;
			MyAudio.Static.EnableDoppler = MySandboxGame.Config.EnableDoppler;
			MySandboxGame.Config.Save();
			if (MySession.Static != null && MyGuiScreenGamePlay.Static != null)
			{
				if (MySandboxGame.Config.EnableDynamicMusic && MyMusicController.Static == null)
				{
					MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
					MyMusicController.Static.Active = true;
					MyAudio.Static.MusicAllowed = false;
					MyAudio.Static.StopMusic();
				}
				else if (!MySandboxGame.Config.EnableDynamicMusic && MyMusicController.Static != null)
				{
					MyMusicController.Static.Unload();
					MyMusicController.Static = null;
					MyAudio.Static.MusicAllowed = true;
					MyAudio.Static.PlayMusic(new MyMusicTrack
					{
						TransitionCategory = MyStringId.GetOrCompute("Default")
					});
				}
				if (MyFakes.AUDIO_ENABLE_REVERB && MyAudio.Static != null && MyAudio.Static.EnableReverb != m_enableReverb.IsChecked && MyAudio.Static.SampleRate <= MyAudio.MAX_SAMPLE_RATE)
				{
					MyAudio.Static.EnableReverb = m_enableReverb.IsChecked;
				}
			}
		}

		private static void UpdateValues(MyGuiScreenOptionsAudioSettings settings)
		{
			MyAudio.Static.VolumeGame = settings.GameVolume;
			MyAudio.Static.VolumeMusic = settings.MusicVolume;
			MyAudio.Static.VolumeVoiceChat = settings.VoiceChatVolume;
			MyAudio.Static.VolumeHud = MyAudio.Static.VolumeGame;
			MyAudio.Static.EnableVoiceChat = settings.EnableVoiceChat;
			MyGuiAudio.HudWarnings = settings.HudWarnings;
			MyFakes.VOICE_CHAT_MIC_SENSITIVITY = settings.MicSensitivity;
			MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION = !settings.PushToTalk;
		}

		public void OnOkClick(MyGuiControlButton sender)
		{
			Save();
			CloseScreen();
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			UpdateValues(m_settingsOld);
			CloseScreen();
		}

		private void OnGameVolumeChange(MyGuiControlSlider sender)
		{
			m_settingsNew.GameVolume = m_gameVolumeSlider.Value;
			UpdateValues(m_settingsNew);
		}

		private void OnMusicVolumeChange(MyGuiControlSlider sender)
		{
			m_settingsNew.MusicVolume = m_musicVolumeSlider.Value;
			UpdateValues(m_settingsNew);
		}

		private void OnVoiceChatVolumeChange(MyGuiControlSlider sender)
		{
			m_settingsNew.VoiceChatVolume = m_voiceChatVolumeSlider.Value;
			UpdateValues(m_settingsNew);
		}

		private void OnVoiceMicSensitivityChange(MyGuiControlSlider sender)
		{
			m_settingsNew.MicSensitivity = m_voiceMicSensitivitySlider.Value;
			UpdateValues(m_settingsNew);
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			UpdateFromConfig(m_settingsOld);
			UpdateValues(m_settingsOld);
			if (m_gameAudioPausedWhenOpen)
			{
				MyAudio.Static.PauseGameSounds();
			}
			return base.CloseScreen(isUnloading);
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (hasFocus)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					OnOkClick(null);
				}
				m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			return result;
		}

		private static Vector2 GetScreenSize(bool isLimited)
		{
			Vector2 result = new Vector2(183f / 280f, 175f / 262f);
			if (isLimited)
			{
				result -= new Vector2(0f, 0.07633588f);
			}
			if (MyPerGameSettings.VoiceChatEnabled)
			{
				if (!MyGameService.IsMicrophoneFilteringSilence())
				{
					result += new Vector2(0f, 0.05f);
				}
				result += new Vector2(0f, 0.03f);
			}
			return result;
		}
	}
}
