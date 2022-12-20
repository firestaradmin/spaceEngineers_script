using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using EmptyKeys.UserInterface.Mvvm;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage;
using VRage.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public sealed class MyGuiScreenSimpleNewGame : MyGuiScreenBase
	{
		private class DataItem
		{
			public string Name;

			public Action<DataItem> Action;

			public StringBuilder CaptionText;

			public StringBuilder DescriptionText;

			public StringBuilder ButtonTextEnabled;

			public StringBuilder ButtonTextDisabled;

			public string Texture;

			public int Id;

			private Func<bool> m_isEnabledFunc;

			public bool IsEnabled
<<<<<<< HEAD
			{
				get
				{
					if (m_isEnabledFunc == null)
					{
						return true;
					}
					return m_isEnabledFunc();
				}
			}

			public StringBuilder ButtonText
			{
				get
				{
					StringBuilder stringBuilder = null;
					stringBuilder = ((!IsEnabled && ButtonTextDisabled != null) ? ButtonTextDisabled : ButtonTextEnabled);
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(string.Empty);
					}
					return stringBuilder;
				}
			}

			public DataItem(string name, StringBuilder captionText, StringBuilder descriptionText, StringBuilder buttonTextEnabled, string texture, Action<DataItem> action, int id, StringBuilder buttonTextDisabled = null, Func<bool> isEnabled = null)
			{
=======
			{
				get
				{
					if (m_isEnabledFunc == null)
					{
						return true;
					}
					return m_isEnabledFunc();
				}
			}

			public StringBuilder ButtonText
			{
				get
				{
					StringBuilder stringBuilder = null;
					stringBuilder = ((!IsEnabled && ButtonTextDisabled != null) ? ButtonTextDisabled : ButtonTextEnabled);
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(string.Empty);
					}
					return stringBuilder;
				}
			}

			public DataItem(string name, StringBuilder captionText, StringBuilder descriptionText, StringBuilder buttonTextEnabled, string texture, Action<DataItem> action, int id, StringBuilder buttonTextDisabled = null, Func<bool> isEnabled = null)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Name = name;
				Action = action;
				CaptionText = captionText;
				DescriptionText = descriptionText;
				ButtonTextEnabled = buttonTextEnabled;
				ButtonTextDisabled = buttonTextDisabled;
				Texture = texture;
				Id = id;
				m_isEnabledFunc = isEnabled;
			}
		}

		private class Item : MyGuiControlParent
		{
			private MyGuiControlLabel m_text;

			private MyGuiControlImage m_image;

			private MyGuiControlParent m_brightBackground;

			private MyGuiControlParent m_completeBackground;

			private Vector2 m_baseSize;

			private float m_baseCaptionSize;

			private float m_space;

			private Vector2 m_offset;

			private int m_dataIndex;

			private DataItem m_data;

			private Vector2 m_imageOffset = ITEM_IMAGE_POSITION;

			private Vector2 m_captionOffset = ITEM_CAPTION_POSITION;

			private Vector2 m_upperImageOffset = ITEM_UPPER_BACKGROUND_POSITION;

			private MyTimeSpan m_lastClickTime = new MyTimeSpan(0L);

			public Action<Item> OnItemClicked;

			public Action<Item> OnItemDoubleClicked;

			public Item(Vector2 size, float space, Vector2 offset)
			{
				m_baseSize = size;
				m_space = space;
				m_offset = offset;
				m_baseCaptionSize = ITEM_CAPTION_SCALE;
				base.Size = m_baseSize;
				base.Position = m_offset;
				base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				m_image = new MyGuiControlImage();
				m_image.SetTexture("Textures\\GUI\\Icons\\scenarios\\PreviewCustomWorld.jpg");
				m_image.Position = ITEM_IMAGE_POSITION;
				m_text = new MyGuiControlLabel
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
				};
				m_text.Size = m_text.GetTextSize();
				m_text.TextEnum = MyCommonTexts.SimpleNewGame_TheFirstJump;
				m_text.Position = m_captionOffset;
				m_brightBackground = new MyGuiControlParent(m_image.Position, new Vector2(m_baseSize.X, ITEM_BRIGHT_BACKGROUND_HEIGHT))
				{
					BackgroundTexture = BRIGHT_BACKGROUND_TEXTURE
				};
				m_completeBackground = new MyGuiControlParent(m_image.Position, new Vector2(m_baseSize.X, ITEM_COMPLETE_BACKGROUND_HEIGHT))
				{
					BackgroundTexture = DARK_BACKGROUND_TEXTURE
				};
				base.Controls.Add(m_completeBackground);
				base.Controls.Add(m_brightBackground);
				base.Controls.Add(m_image);
				base.Controls.Add(m_text);
			}

			public void SetData(DataItem data, int index)
			{
				m_data = data;
				m_dataIndex = index;
				m_image.SetTexture(data.Texture);
				float num = MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
				float num2 = 0.935f;
				Vector2 vector = new Vector2(1f, 0.5f);
				m_image.Size = new Vector2(base.Size.X * num2, num * base.Size.X * num2) * vector;
				m_text.Text = data.CaptionText.ToString();
			}

			public DataItem GetData()
			{
				return m_data;
			}

			public int GetDataIndex()
			{
				return m_dataIndex;
			}

			public void ActivateAction()
			{
				if (m_data != null && m_data.Action != null)
				{
					m_data.Action(m_data);
				}
			}

			public override MyGuiControlBase HandleInput()
			{
				if (MyInput.Static.IsNewPrimaryButtonPressed())
				{
					Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
					Vector2 size = GetPositionAbsoluteBottomRight() - positionAbsoluteTopLeft;
					if (new RectangleF(positionAbsoluteTopLeft, size).Contains(MyGuiManager.MouseCursorPosition))
					{
						bool flag = false;
						MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalTimeInMilliseconds);
						if (myTimeSpan - m_lastClickTime < MyTimeSpan.FromMilliseconds(200.0))
						{
							if (OnItemDoubleClicked != null)
							{
								OnItemDoubleClicked(this);
							}
							flag = true;
						}
						m_lastClickTime = myTimeSpan;
						if (!flag && OnItemClicked != null)
						{
							OnItemClicked(this);
						}
					}
				}
				return base.HandleInput();
			}

			public void SetScale(float scale)
			{
				base.Size = new Vector2(m_baseSize.X * scale, m_baseSize.Y * scale);
				float num = MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
				Vector2 vector = new Vector2(1f, 0.5f);
				m_image.Size = new Vector2(base.Size.X * IMAGE_SCALE, num * base.Size.X * IMAGE_SCALE) * vector;
				m_text.Size = m_text.GetTextSize() * scale;
				m_text.TextScale = m_baseCaptionSize * scale;
				m_image.Position = ITEM_IMAGE_POSITION * scale;
				m_text.Position = m_captionOffset * scale;
				m_brightBackground.Size = new Vector2(m_baseSize.X, ITEM_BRIGHT_BACKGROUND_HEIGHT) * scale;
				m_brightBackground.Position = m_image.Position;
				m_completeBackground.Size = new Vector2(m_baseSize.X, ITEM_COMPLETE_BACKGROUND_HEIGHT) * scale;
				m_completeBackground.Position = m_image.Position + ITEM_COMPLETE_BACKGROUND_POSITION * scale;
			}

			public void SetOpacity(bool enabled, int distanceFromPivot, float opacity, float opacity2, float opacityImg, float opacityBackground)
			{
				float num = 1f;
				Vector4 colorMask = new Vector4(opacity * num);
				Vector4 colorMask2 = new Vector4(opacity2 * num);
				Vector4 colorMask3 = new Vector4(opacityImg * num);
				Vector4 colorMask4 = new Vector4(opacityBackground * num);
				m_image.ColorMask = colorMask3;
				m_brightBackground.ColorMask = colorMask2;
				m_completeBackground.ColorMask = colorMask4;
				m_text.ColorMask = colorMask;
			}

			public void SetPosition(Vector2 position)
			{
				base.Position = position + m_offset;
			}

			internal void SuppressDoubleClick()
			{
				m_lastClickTime = new MyTimeSpan(0L);
			}
		}

		private static MyGuiCompositeTexture BUTTON_TEXTURE_LEFT = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\LeftArrow_focus.png");

		private static MyGuiCompositeTexture BUTTON_TEXTURE_RIGHT = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\RightArrow_focus.png");

		private static MyGuiControlImageButton.StyleDefinition STYLE_BUTTON_LEFT = new MyGuiControlImageButton.StyleDefinition
		{
			Active = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Disabled = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Normal = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Highlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			ActiveHighlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Padding = new MyGuiBorderThickness(0.005f, 0.005f)
		};

		private static MyGuiControlImageButton.StyleDefinition STYLE_BUTTON_RIGHT = new MyGuiControlImageButton.StyleDefinition
		{
			Active = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Disabled = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Normal = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Highlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			ActiveHighlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Padding = new MyGuiBorderThickness(0.005f, 0.005f)
		};

		private static readonly float ITEM_SCALE = 1.165f;

		private static readonly float ITEM_SPACING = 0.02f;

		private static readonly float IMAGE_SCALE = 0.965f;

		private static readonly Vector2 ITEM_SIZE = new Vector2(0.3f, 0.55f) * ITEM_SCALE;

		private static readonly Vector2 ITEM_POSITION_OFFSET = new Vector2(0f, -0.087f);

		private static readonly Vector2 ITEM_IMAGE_POSITION = new Vector2(0f, 0.166f);

		private static readonly Vector2 ITEM_CAPTION_POSITION = new Vector2(0f, 0f);

		private static readonly Vector2 ITEM_UPPER_BACKGROUND_POSITION = new Vector2(0f, -0.2f);

		private static readonly float ITEM_BRIGHT_BACKGROUND_HEIGHT = 0.21f * ITEM_SCALE;
<<<<<<< HEAD

		private static readonly float ITEM_COMPLETE_BACKGROUND_HEIGHT = 0.29f * ITEM_SCALE;

		private static readonly Vector2 ITEM_COMPLETE_BACKGROUND_POSITION = new Vector2(0f, -0.041f);

		private static readonly float START_BUTTON_HEIGHT = 0.07f;

=======

		private static readonly float ITEM_COMPLETE_BACKGROUND_HEIGHT = 0.29f * ITEM_SCALE;

		private static readonly Vector2 ITEM_COMPLETE_BACKGROUND_POSITION = new Vector2(0f, -0.041f);

		private static readonly float START_BUTTON_HEIGHT = 0.07f;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static readonly float ITEM_CAPTION_SCALE = 1.25f;

		private static readonly MyGuiCompositeTexture DARK_BACKGROUND_TEXTURE = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\DarkBlueBackground.png");

		private static readonly MyGuiCompositeTexture BRIGHT_BACKGROUND_TEXTURE = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\rectangle_button_focus_center.dds");
<<<<<<< HEAD
=======

		private static readonly float DEFAULT_OPACITY = 0.92f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static readonly float DEFAULT_OPACITY = 0.92f;

		private readonly float SHIFT_SPEED = 0.045f;

		private DataItem m_activeItem;

		private int m_activeIndex;

		private List<DataItem> m_items = new List<DataItem>();

		private List<Item> m_guiItems = new List<Item>();

		private MyGuiControlButton m_buttonStart;

		private MyGuiControlImageButton m_buttonLeft;

		private MyGuiControlImageButton m_buttonRight;

		private DataItem m_campaignTheFirstJump;

		private DataItem m_campaignLearningToSurvive;

		private DataItem m_campaignNeverSurrender;

		private DataItem m_campaignFrostbite;

		private DataItem m_campaignScrapRace;

<<<<<<< HEAD
=======
		private DataItem m_campaignLostColony;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyGuiControlMultilineText m_description;

		private float m_animationValueCurrent;

		private float m_animationLinearCurrent;

		private float m_animationLinearNext;

		private float m_animationSpeed;

		private float m_animationDelinearizingValue;

		private int m_guiItemsMiddle;

		private bool m_parallelLoadIsRunning;

		private bool IsAnimating
		{
			get
			{
				if (m_animationLinearCurrent == m_animationLinearNext)
				{
					return m_animationSpeed != 0f;
				}
				return true;
			}
		}

		public MyGuiScreenSimpleNewGame()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8f, 0.7f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
			m_backgroundColor = Color.Transparent;
			m_backgroundFadeColor = Vector4.Zero;
			Vector4 videoOverlayColor = new Vector4(0f, 0f, 0f, 1f);
			SetVideoOverlayColor(videoOverlayColor);
		}

		private void CampaignStarted(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				List<string> campaignsStarted = MySandboxGame.Config.CampaignsStarted;
				if (!campaignsStarted.Contains(name))
				{
					campaignsStarted.Add(name);
					MySandboxGame.Config.Save();
				}
			}
		}

		private bool WasCampaignStarted(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			return MySandboxGame.Config.CampaignsStarted.Contains(name);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyObjectBuilder_Campaign leaningToSurvive = null;
			MyObjectBuilder_Campaign neverSurrender = null;
			MyObjectBuilder_Campaign campaign = null;
			MyObjectBuilder_Campaign frostbite = null;
			MyObjectBuilder_Campaign scrapRace = null;
			foreach (MyObjectBuilder_Campaign campaign2 in MyCampaignManager.Static.Campaigns)
			{
				switch (campaign2.Name)
				{
				case "The First Jump":
					campaign = campaign2;
					break;
				case "Learning to Survive":
					leaningToSurvive = campaign2;
					break;
				case "Never Surrender":
					neverSurrender = campaign2;
					break;
				case "Frostbite":
					frostbite = campaign2;
					break;
				case "Scrap Race":
					scrapRace = campaign2;
					break;
				}
			}
			m_campaignTheFirstJump = AddItem(campaign.Name, MyCommonTexts.SimpleNewGame_TheFirstJump, MyCommonTexts.SimpleNewGame_TheFirstJump_Description, MyCommonTexts.SimpleNewGame_Start, campaign.ImagePath, delegate(DataItem x)
			{
				if (WasCampaignStarted(campaign.Name) || m_campaignLearningToSurvive == null)
				{
					MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
					MySandboxGame.Config.Save();
				}
				else
				{
					MySandboxGame.Config.NewNewGameScreenLastSelection = m_campaignLearningToSurvive.Id;
					MySandboxGame.Config.Save();
					CampaignStarted(campaign.Name);
				}
				StartScenario(campaign, preferOnline: false);
			});
			m_campaignLearningToSurvive = AddItem(leaningToSurvive.Name, MyCommonTexts.SimpleNewGame_LearningToSurvive, MyCommonTexts.SimpleNewGame_LearningToSurvive_Description, MyCommonTexts.SimpleNewGame_Start, leaningToSurvive.ImagePath, delegate(DataItem x)
			{
				if (WasCampaignStarted(leaningToSurvive.Name) || m_campaignNeverSurrender == null)
				{
					MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
					MySandboxGame.Config.Save();
				}
				else
				{
					MySandboxGame.Config.NewNewGameScreenLastSelection = m_campaignNeverSurrender.Id;
					MySandboxGame.Config.Save();
					CampaignStarted(leaningToSurvive.Name);
				}
				StartScenario(leaningToSurvive, preferOnline: false);
			});
			if (frostbite != null)
<<<<<<< HEAD
			{
				m_campaignFrostbite = AddItem(frostbite.Name, MySpaceTexts.DisplayName_DLC_Frostbite, MySpaceTexts.SimpleNewGame_Frostbite_Description, MyCommonTexts.SimpleNewGame_Start, frostbite.ImagePath, delegate(DataItem x)
				{
					if (x.IsEnabled)
					{
						MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
						MySandboxGame.Config.Save();
						CampaignStarted(frostbite.Name);
						StartScenario(frostbite, MyPlatformGameSettings.PREFER_ONLINE);
					}
					else
					{
						MyGameService.OpenDlcInShop(MyDLCs.GetDLC(MyDLCs.MyDLC.DLC_NAME_Frostbite).AppId);
					}
				}, MySpaceTexts.OpenDlcShop, () => MyGameService.IsDlcInstalled(MyDLCs.GetDLC(MyDLCs.MyDLC.DLC_NAME_Frostbite).AppId));
			}
			if (MySandboxGame.Config.ExperimentalMode)
			{
=======
			{
				m_campaignFrostbite = AddItem(frostbite.Name, MySpaceTexts.DisplayName_DLC_Frostbite, MySpaceTexts.SimpleNewGame_Frostbite_Description, MyCommonTexts.SimpleNewGame_Start, frostbite.ImagePath, delegate(DataItem x)
				{
					if (x.IsEnabled)
					{
						MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
						MySandboxGame.Config.Save();
						CampaignStarted(frostbite.Name);
						StartScenario(frostbite, MyPlatformGameSettings.PREFER_ONLINE);
					}
					else
					{
						MyGameService.OpenDlcInShop(MyDLCs.GetDLC(MyDLCs.MyDLC.DLC_NAME_Frostbite).AppId);
					}
				}, MySpaceTexts.OpenDlcShop, () => MyGameService.IsDlcInstalled(MyDLCs.GetDLC(MyDLCs.MyDLC.DLC_NAME_Frostbite).AppId));
			}
			if (MySandboxGame.Config.ExperimentalMode)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_campaignScrapRace = AddItem(scrapRace.Name, MyCommonTexts.SimpleNewGame_ScrapRace, MyCommonTexts.SimpleNewGame_ScrapRace_Description, MyCommonTexts.SimpleNewGame_Start, scrapRace.ImagePath, delegate(DataItem x)
				{
					MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
					MySandboxGame.Config.Save();
					CampaignStarted(scrapRace.Name);
					StartScenario(scrapRace, MyPlatformGameSettings.PREFER_ONLINE);
				});
			}
			m_campaignNeverSurrender = AddItem(neverSurrender.Name, MyCommonTexts.SimpleNewGame_NeverSurrender, MyCommonTexts.SimpleNewGame_NeverSurrender_Description, MyCommonTexts.SimpleNewGame_Start, neverSurrender.ImagePath, delegate(DataItem x)
			{
				MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
				MySandboxGame.Config.Save();
				CampaignStarted(neverSurrender.Name);
				StartScenario(neverSurrender, MyPlatformGameSettings.PREFER_ONLINE);
			});
			AddWorld(MyCommonTexts.SimpleNewGame_Creative, MyCommonTexts.SimpleNewGame_Creative_Description, MyCommonTexts.SimpleNewGame_Start, MyGameModeEnum.Creative, MyPlatformGameSettings.PREFER_ONLINE, "Red Ship");
			AddItem(string.Empty, MyCommonTexts.SimpleNewGame_Workshop, MyCommonTexts.WorkshopScreen_Description, MyCommonTexts.SimpleNewGame_Open, "Textures\\GUI\\Icons\\Workshop.jpg", delegate(DataItem x)
			{
				MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
				if (!MyGameService.AtLeastOneUGCServiceConsented)
				{
					MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(ShowNewWorkshopGameScreen);
					IMyGuiScreenFactoryService service = ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>();
					MyScreenManager.CloseScreenNow(service.GetMyGuiScreenBase(typeof(MyWorkshopBrowserViewModel)));
					service.CreateScreen(viewModel);
				}
				else
				{
					ShowNewWorkshopGameScreen();
				}
			});
			AddItem(string.Empty, MyCommonTexts.SimpleNewGame_Custom, MyCommonTexts.WorldSettingsScreen_Description, MyCommonTexts.SimpleNewGame_Open, "Textures\\GUI\\Icons\\scenarios\\PreviewEarth.jpg", delegate(DataItem x)
			{
				MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
				MySandboxGame.Config.Save();
				MyGuiSandbox.AddScreen(new MyGuiScreenWorldSettings());
			});
			Vector2 size = new Vector2(ITEM_SIZE.X, START_BUTTON_HEIGHT);
			new Vector2(0.03f, 0.04f);
			Vector2 vector = new Vector2(0.0017f, 0.185f);
			m_buttonStart = new MyGuiControlButton(vector + new Vector2(0f, 0.0025f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.SimpleNewGame_Start));
			m_buttonStart.VisualStyle = MyGuiControlButtonStyleEnum.RectangularBorderLess;
			m_buttonStart.Size = size;
			m_buttonStart.ButtonClicked += OnStartClicked;
			m_buttonStart.ColorMask = new Vector4(DEFAULT_OPACITY);
			m_buttonStart.TextScale = ITEM_CAPTION_SCALE;
			m_buttonStart.BorderSize = 0;
			m_buttonStart.BorderEnabled = false;
			_ = START_BUTTON_HEIGHT;
			_ = START_BUTTON_HEIGHT;
			Vector2 vector2 = new Vector2(0.02f, 0f);
			BUTTON_TEXTURE_LEFT.MarkDirty();
			BUTTON_TEXTURE_RIGHT.MarkDirty();
			Vector2 vector3 = new Vector2(0f, 0.02f);
			m_buttonLeft = new MyGuiControlImageButton("Button", vector3 - (new Vector2(0.5f * m_buttonStart.Size.X, 0f) + vector2))
			{
				CanHaveFocus = false
			};
			m_buttonLeft.Text = string.Empty;
			m_buttonLeft.ApplyStyle(STYLE_BUTTON_LEFT);
			m_buttonLeft.Size = new Vector2(0.75f, 1f) * 0.035f;
			m_buttonLeft.ButtonClicked += OnLeftClicked;
			m_buttonLeft.ColorMask = new Vector4(DEFAULT_OPACITY);
			m_buttonRight = new MyGuiControlImageButton("Button", vector3 + (new Vector2(0.5f * m_buttonStart.Size.X, 0f) + vector2))
			{
				CanHaveFocus = false
			};
			m_buttonRight.Text = string.Empty;
			m_buttonRight.ApplyStyle(STYLE_BUTTON_RIGHT);
			m_buttonRight.Size = new Vector2(0.75f, 1f) * 0.035f;
			m_buttonRight.ButtonClicked += OnRightClicked;
			m_buttonRight.ColorMask = new Vector4(DEFAULT_OPACITY);
			Controls.Add(m_buttonLeft);
			Controls.Add(m_buttonRight);
			Vector2 value = new Vector2(0f, 0.35f);
			m_description = new MyGuiControlMultilineText(size: new Vector2(0.7f, 0.12f), position: value)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBorderless
			};
			m_description.TextScale = 0.875f;
			m_description.TextPadding = new MyGuiBorderThickness(0.01f, 0.01f);
			for (int i = 0; i < m_items.Count; i++)
			{
				Item item = BuildGuiItem();
				m_guiItems.Add(item);
			}
			m_guiItemsMiddle = 0;
			Controls.Add(m_description);
			InitialWorldSelection();
			Controls.Add(m_buttonStart);
			base.FocusedControl = m_buttonStart;
			void AddWorld(MyStringId captionText, MyStringId descriptionText, MyStringId buttonText, MyGameModeEnum mode, bool preferOnline, string world)
			{
				string worldPath = Path.Combine(MyFileSystem.ContentPath, "CustomWorlds", world);
				AddItem(string.Empty, captionText, descriptionText, buttonText, Path.Combine(worldPath, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION), delegate(DataItem x)
				{
					MySandboxGame.Config.NewNewGameScreenLastSelection = x.Id;
					MySandboxGame.Config.Save();
					StartWorld(worldPath, mode, preferOnline);
				});
			}
		}

		private void ShowNewWorkshopGameScreen()
		{
			MySandboxGame.Config.Save();
			MyGuiSandbox.AddScreen(new MyGuiScreenNewWorkshopGame(displayTabScenario: true, MyGameService.AtLeastOneUGCServiceConsented));
		}

		private void InitialWorldSelection()
		{
			if (MySandboxGame.Config.NewNewGameScreenLastSelection >= 0 && MySandboxGame.Config.NewNewGameScreenLastSelection < m_items.Count)
			{
				int newNewGameScreenLastSelection = MySandboxGame.Config.NewNewGameScreenLastSelection;
				ResetActiveIndex(newNewGameScreenLastSelection);
				m_guiItemsMiddle = newNewGameScreenLastSelection;
			}
			else if (m_campaignTheFirstJump != null)
			{
				ResetActiveItem(m_campaignTheFirstJump);
			}
			else
			{
				ResetActiveIndex(0);
			}
		}

		private void OnStartClicked(MyGuiControlButton obj)
		{
			if (m_activeItem != null)
			{
				m_activeItem.Action(m_activeItem);
			}
		}

		private void OnLeftClicked(MyGuiControlImageButton obj)
		{
			int amount = 1;
			ShiftItems(amount, SHIFT_SPEED);
		}

		private void OnRightClicked(MyGuiControlImageButton obj)
		{
			int amount = -1;
			ShiftItems(amount, SHIFT_SPEED);
		}

		private DataItem AddItem(string campaignName, MyObjectBuilder_Campaign campaign, Action<DataItem> action)
		{
			MyCampaignManager.Static.ReloadMenuLocalization(campaign.Name);
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[campaign.Name];
			StringBuilder captionText;
			StringBuilder descriptionText;
			if (myLocalizationContext != null)
			{
				captionText = myLocalizationContext["Name"];
				descriptionText = myLocalizationContext["Description"];
			}
			else
			{
				captionText = new StringBuilder(campaign.Name);
				descriptionText = new StringBuilder(campaign.Description);
			}
			DataItem dataItem = new DataItem(campaignName, captionText, descriptionText, MyTexts.Get(MyCommonTexts.SimpleNewGame_Start), campaign.ImagePath, action, m_items.Count);
			m_items.Add(dataItem);
			return dataItem;
		}

		private DataItem AddItem(string name, MyStringId captionText, MyStringId descriptionText, MyStringId buttonText, string texture, Action<DataItem> action, MyStringId? buttonTextDisabled = null, Func<bool> isEnabled = null)
		{
			DataItem dataItem = new DataItem(name, MyTexts.Get(captionText), MyTexts.Get(descriptionText), MyTexts.Get(buttonText), texture, action, m_items.Count, buttonTextDisabled.HasValue ? MyTexts.Get(buttonTextDisabled.Value) : null, isEnabled);
			m_items.Add(dataItem);
			return dataItem;
		}

		private void StartScenario(MyObjectBuilder_Campaign scenario, bool preferOnline)
		{
			if (!m_parallelLoadIsRunning)
			{
				m_parallelLoadIsRunning = true;
				MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
				MyScreenManager.AddScreen(progressScreen);
				Parallel.StartBackground(delegate
				{
					StartScenarioInternal(scenario, preferOnline);
				}, delegate
				{
					progressScreen.CloseScreen();
					m_parallelLoadIsRunning = false;
				});
			}
		}

		private void StartScenarioInternal(MyObjectBuilder_Campaign scenario, bool preferOnline)
		{
			MyCampaignManager.Static.SwitchCampaign(scenario.Name, scenario.IsVanilla, scenario.PublishedFileId, scenario.PublishedServiceName, scenario.ModFolderPath);
			if (!preferOnline || !MyGameService.IsActive)
			{
				Run(granted: false);
				return;
			}
			bool granted2 = false;
			int done = 0;
			MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: false, delegate(bool x)
			{
				if (x)
				{
					MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: false, delegate(bool ugcGranted)
					{
						granted2 = ugcGranted;
						done = 1;
					});
				}
				else
				{
					granted2 = false;
					done = 1;
				}
			});
			WaitFor(ref done);
			Run(granted2);
			void Run(bool granted)
			{
				if (MyCloudHelper.IsError(MyCampaignManager.Static.RunNewCampaign(scenario.Name, granted ? MyOnlineModeEnum.FRIENDS : MyOnlineModeEnum.OFFLINE, MyMultiplayerLobby.MAX_PLAYERS, MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : null), out var errorMessage))
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
					myGuiScreenMessageBox.SkipTransition = true;
					myGuiScreenMessageBox.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
				}
			}
		}

		private void WaitFor(ref int done)
		{
			while (Interlocked.CompareExchange(ref done, 0, 0) == 0)
			{
				Thread.Sleep(10);
			}
		}

		private void StartWorld(string sessionPath, MyGameModeEnum gameMode, bool preferOnline)
		{
			if (!m_parallelLoadIsRunning)
			{
				m_parallelLoadIsRunning = true;
				MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
				MyScreenManager.AddScreen(progressScreen);
				Parallel.StartBackground(delegate
<<<<<<< HEAD
				{
					StartWorldInner();
				}, delegate
				{
=======
				{
					StartWorldInner();
				}, delegate
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					progressScreen.CloseScreen();
					m_parallelLoadIsRunning = false;
				});
			}
			void StartWorldInner()
			{
				bool granted = false;
				if (preferOnline && MyGameService.IsActive)
				{
					int done = 0;
					MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: false, delegate(bool x)
					{
						if (x)
						{
							MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: false, delegate(bool ugcGranted)
							{
								granted = ugcGranted;
								done = 1;
							});
						}
						else
						{
							granted = false;
							done = 1;
						}
					});
					WaitFor(ref done);
				}
				StartWorld(sessionPath, gameMode, granted ? MyOnlineModeEnum.FRIENDS : MyOnlineModeEnum.OFFLINE);
			}
		}

		private void StartWorld(string sessionPath, MyGameModeEnum gameMode, MyOnlineModeEnum onlineMode)
		{
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes, gameMode, onlineMode);
			if (myObjectBuilder_Checkpoint == null)
			{
				return;
<<<<<<< HEAD
			}
			if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(myObjectBuilder_Checkpoint))
			{
				MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(myObjectBuilder_Checkpoint));
				return;
			}
=======
			}
			if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(myObjectBuilder_Checkpoint))
			{
				MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(myObjectBuilder_Checkpoint));
				return;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string saveName = MyStatControlText.SubstituteTexts(myObjectBuilder_Checkpoint.SessionName) + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
			MySessionLoader.LoadSingleplayerSession(myObjectBuilder_Checkpoint, sessionPath, sizeInBytes, delegate
			{
				string text = Path.Combine(MyFileSystem.SavesPath, saveName.Replace(':', '-'));
				MySession.Static.CurrentPath = text;
				MyAsyncSaving.DelayedSaveAfterLoad(text);
			});
		}

		private void ResetActiveIndex(int i)
		{
			if (m_items.Count > i && i >= 0)
			{
				SetActiveIndex(i);
				BindItemsToGUI();
			}
		}

		private void ResetActiveItem(DataItem item)
		{
			BindItemsToGUI();
			int num = -1;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i] == item)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				SetActiveIndex(num);
				m_guiItemsMiddle = num;
			}
		}

		private void SetActiveIndex(int i)
		{
			if (m_items.Count > i && i >= 0)
			{
				m_activeIndex = i;
				m_activeItem = m_items[i];
				m_description.Text = m_activeItem.DescriptionText;
				m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				m_buttonStart.Text = m_activeItem.ButtonText.ToString();
				if (i <= 0)
				{
					m_buttonLeft.Visible = false;
				}
				else
				{
					m_buttonLeft.Visible = true;
				}
				if (i >= m_items.Count - 1)
				{
					m_buttonRight.Visible = false;
				}
				else
				{
					m_buttonRight.Visible = true;
				}
			}
		}

		private void BindItemsToGUI()
		{
			int num = Math.Min(m_items.Count, m_guiItems.Count);
			for (int i = 0; i < num; i++)
			{
				m_guiItems[i].SetData(m_items[i], i);
			}
		}

		private void ShiftItems(int amount, float speed)
		{
			if (m_activeIndex - amount < 0 || m_activeIndex - amount >= m_items.Count)
			{
				return;
			}
			if (IsAnimating)
			{
				if (Math.Sign(amount) != Math.Sign(m_animationLinearNext))
				{
					return;
				}
				float num = m_animationLinearNext + ((amount > 0) ? 1f : (-1f));
				if (Math.Abs(num) >= 2f)
				{
					return;
				}
				m_animationSpeed = m_animationSpeed * ((m_animationLinearNext - m_animationLinearCurrent) / m_animationLinearNext) + speed;
				m_animationLinearNext = Math.Sign(num - m_animationLinearCurrent);
				m_animationDelinearizingValue = Math.Abs(num - m_animationValueCurrent);
				m_animationLinearCurrent = 0f;
				int activeIndex = (m_activeIndex + -amount % m_items.Count + m_items.Count) % m_items.Count;
				SetActiveIndex(activeIndex);
			}
			else
			{
				m_animationDelinearizingValue = Math.Abs(amount);
				m_animationSpeed = speed;
				m_animationLinearCurrent = 0f;
				m_animationLinearNext = ((amount > 0) ? 1f : (-1f));
				int activeIndex2 = (m_activeIndex + -amount % m_items.Count + m_items.Count) % m_items.Count;
				SetActiveIndex(activeIndex2);
			}
			MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (IsAnimating)
			{
				if (m_animationLinearCurrent < m_animationLinearNext)
				{
					if (m_animationLinearCurrent + m_animationSpeed >= m_animationLinearNext)
					{
						m_animationLinearCurrent = (m_animationLinearNext = 0f);
						m_animationSpeed = 0f;
						m_animationValueCurrent = (float)Math.Round(m_animationValueCurrent);
					}
					else
					{
						float num = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationLinearCurrent += m_animationSpeed;
						float num2 = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationValueCurrent += m_animationDelinearizingValue * (num2 - num);
					}
				}
				else if (m_animationLinearCurrent > m_animationLinearNext)
				{
					if (m_animationLinearCurrent - m_animationSpeed <= m_animationLinearNext)
					{
						m_animationLinearCurrent = (m_animationLinearNext = 0f);
						m_animationSpeed = 0f;
						m_animationValueCurrent = (float)Math.Round(m_animationValueCurrent);
					}
					else
					{
						float num3 = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationLinearCurrent -= m_animationSpeed;
						float num4 = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationValueCurrent += m_animationDelinearizingValue * (num4 - num3);
					}
				}
			}
			if (m_animationValueCurrent <= -1f)
			{
				m_animationValueCurrent += 1f;
				m_guiItemsMiddle++;
			}
			else if (m_animationValueCurrent >= 1f)
			{
				m_animationValueCurrent -= 1f;
				m_guiItemsMiddle--;
			}
			for (int i = 0; i < m_guiItems.Count; i++)
			{
				float num5 = ComputeScale((float)i + m_animationValueCurrent);
				float num6 = ComputeScale2((float)i + m_animationValueCurrent);
				Vector2 position = ComputePosition((float)i + m_animationValueCurrent, num5);
				m_guiItems[i].SetScale(num5);
				m_guiItems[i].SetOpacity(m_guiItems[i].GetData().IsEnabled, Math.Abs(i - m_guiItemsMiddle), num5 * DEFAULT_OPACITY, num6, num5 * ((1f - num6) * DEFAULT_OPACITY + num6), num5 * (1f - num6) * DEFAULT_OPACITY);
				m_guiItems[i].SetPosition(position);
			}
			float num7 = Math.Abs(m_animationLinearCurrent % 1f);
			if (num7 > 0.5f)
			{
				num7 = 1f - num7;
			}
			float value = 0f;
			if (num7 < 0.2f)
			{
				value = 1f - 5f * num7;
			}
			Vector4 colorMask = new Vector4(value);
			m_buttonLeft.ColorMask = colorMask;
			m_buttonRight.ColorMask = colorMask;
			return result;
		}

		private void AddItemToStartRemoveFromEnd()
		{
			int previoudIdx = 0;
			GetDataItemPrevious(m_guiItems[0], out var previousData, out previoudIdx);
			Item item = BuildGuiItem(previousData);
			item.SetData(previousData, previoudIdx);
			m_guiItems.Insert(0, item);
			Controls.Remove(m_guiItems[m_guiItems.Count - 1]);
			m_guiItems.RemoveAt(m_guiItems.Count - 1);
		}

		private void AddItemToEndRemoveFromStart()
		{
			int nextIdx = 0;
			GetDataItemNext(m_guiItems[m_guiItems.Count - 1], out var nextData, out nextIdx);
			Item item = BuildGuiItem(nextData);
			item.SetData(nextData, nextIdx);
			m_guiItems.Insert(m_guiItems.Count, item);
			Controls.Remove(m_guiItems[0]);
			m_guiItems.RemoveAt(0);
		}

		private void GetDataItemPrevious(Item item, out DataItem previousData, out int previoudIdx)
		{
			int num = (item.GetDataIndex() + m_items.Count - 1) % m_items.Count;
			previousData = m_items[num];
			previoudIdx = num;
		}

		private void GetDataItemNext(Item item, out DataItem nextData, out int nextIdx)
		{
			int num = (item.GetDataIndex() + 1) % m_items.Count;
			nextData = m_items[num];
			nextIdx = num;
		}

		private Item BuildGuiItem(DataItem data = null)
		{
			Item item = new Item(ITEM_SIZE, ITEM_SPACING, ITEM_POSITION_OFFSET);
			item.OnItemClicked = (Action<Item>)Delegate.Combine(item.OnItemClicked, new Action<Item>(OnItemClicked));
			item.OnItemDoubleClicked = (Action<Item>)Delegate.Combine(item.OnItemDoubleClicked, new Action<Item>(OnItemDoubleClicked));
			Controls.Add(item);
			return item;
		}

		private void OnItemClicked(Item item)
		{
			if (IsAnimating)
			{
				return;
			}
			if (item.GetData() != m_activeItem)
			{
				item.SuppressDoubleClick();
			}
			int num = -1;
			for (int i = 0; i < m_guiItems.Count; i++)
			{
				if (m_guiItems[i] == item)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				int num2 = num - m_guiItemsMiddle;
				if (num2 != 0)
				{
					ShiftItems(-num2, SHIFT_SPEED);
				}
			}
		}

		private void OnItemDoubleClicked(Item item)
		{
			if (m_activeItem == item.GetData())
			{
				m_activeItem.Action(m_activeItem);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Remap range -1;1 into -1;1 using sine curve
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float RescaleTransitionSineSymmetric(float input)
		{
			return (float)Math.Sign(input) * RescaleTransitionSine(Math.Abs(input));
		}

		/// <summary>
		/// Remap range 0;1 into 0;1 using sine curve
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public float RescaleTransitionSine(float input)
		{
			return (float)Math.Sin((double)input * Math.PI * 0.5);
		}

		public float ComputeScale(float coef)
		{
			float num = Math.Abs(coef - (float)m_guiItemsMiddle);
			return Math.Max(-0.007f * num * num * num + 0.1f * num * num + -0.41f * num + 1f, 0f);
		}

		public float ComputeScale2(float coef)
		{
			float value = coef - (float)m_guiItemsMiddle;
			return Math.Max(1f - Math.Min(Math.Abs(value), 1f), 0f);
		}

		public Vector2 ComputePosition(float coef, float scale)
		{
			float value = coef - (float)m_guiItemsMiddle;
			float num = Math.Abs(value);
			float x = (float)Math.Sign(value) * (0.0095f * num * num * num + -0.075f * num * num + 0.411f * num + 0f);
			float y = 0.0036f * num * num * num + -0.036f * num * num + 0.121f * num + -0.058f;
			return new Vector2(x, y);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_LEFT, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_LEFT, MyControlStateType.PRESSED) || m_keyThrottler.GetKeyStatus(MyKeys.Left) == ThrottledKeyStatus.PRESSED_AND_READY)
			{
				int amount = 1;
				ShiftItems(amount, SHIFT_SPEED);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_RIGHT, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_RIGHT, MyControlStateType.PRESSED) || m_keyThrottler.GetKeyStatus(MyKeys.Right) == ThrottledKeyStatus.PRESSED_AND_READY)
			{
				int amount2 = -1;
				ShiftItems(amount2, SHIFT_SPEED);
			}
			int num = MyInput.Static.DeltaMouseScrollWheelValue();
			if (num != 0)
			{
				if (!IsAnimating)
				{
					ShiftItems(num / 120, SHIFT_SPEED);
				}
				else
				{
					m_animationSpeed *= 2f;
				}
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			MyGuiSandbox.DrawGameLogoHandler(m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
			return result;
		}

		public override string GetFriendlyName()
		{
			return "SimpleNewGame";
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			SetVideoOverlayColor(new Vector4(1f, 1f, 1f, 1f));
			return base.CloseScreen(isUnloading);
		}

		private void SetVideoOverlayColor(Vector4 color)
		{
			MyGuiScreenIntroVideo firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>();
			if (firstScreenOfType != null)
			{
				firstScreenOfType.OverlayColorMask = color;
			}
		}
	}
}
