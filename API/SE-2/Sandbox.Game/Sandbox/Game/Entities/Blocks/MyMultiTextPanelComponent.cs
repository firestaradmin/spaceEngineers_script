using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ObjectBuilders;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	public class MyMultiTextPanelComponent : IMyTextSurfaceProvider, IMyTextPanelProvider
	{
		private List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();

		private MyRenderComponentScreenAreas m_render;

		private MyTerminalBlock m_block;

		private int m_selectedPanel;

		private bool m_wasInRange;

		private bool m_wasInRange;

		private Action<int, int[]> m_addImagesToSelectionRequest;

		private Action<int, int[]> m_removeImagesFromSelectionRequest;

		private Action<int, string> m_changeTextRequest;

		private Action<int, MySerializableSpriteCollection> m_updateSpriteCollection;

		private float m_maxRenderDistanceSquared;

		private MySessionComponentPanels m_panelsComponent;

		private bool m_texturesReleased = true;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_panels.Count != 0)
				{
					return m_panels[m_selectedPanel];
				}
				return null;
			}
		}

		public int SurfaceCount
		{
			get
			{
				if (m_panels == null)
				{
					return 0;
				}
				return m_panels.Count;
			}
		}

		public int SelectedPanelIndex => m_selectedPanel;

		int IMyTextSurfaceProvider.SurfaceCount => SurfaceCount;

		public int PanelTexturesByteCount
		{
			get
			{
				int num = 0;
				foreach (MyTextPanelComponent panel in m_panels)
				{
					num += panel.TextureByteCount;
				}
				return num;
			}
		}

		public Vector3D WorldPosition => m_block.PositionComp.WorldMatrixRef.Translation;

		public int RangeIndex { get; set; }

<<<<<<< HEAD
		public bool UseGenericLcd => true;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyTextPanelComponent GetPanelComponent(int panelIndex)
		{
			return m_panels[panelIndex];
		}

		public static void CreateTerminalControls<T>() where T : MyTerminalBlock, IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner
		{
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("PanelList", MyStringId.GetOrCompute("LCD Panels"), MySpaceTexts.Blank)
			{
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					FillPanels(x.MultiTextPanel, list1, list2, focusedItem);
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.SelectPanel(y);
				},
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 1 && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 1,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("Content", MySpaceTexts.BlockPropertyTitle_PanelContent, MySpaceTexts.Blank)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					MyTextPanelComponent.FillContentComboBoxContent(x);
				},
				Getter = (T x) => (long)((x.PanelComponent == null) ? ContentType.NONE : x.PanelComponent.ContentType),
				Setter = delegate(T x, long y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.ContentType = (ContentType)y;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType != 0 && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType != ContentType.NONE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("Script", MySpaceTexts.BlockPropertyTitle_PanelScript, MySpaceTexts.Blank)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FillScriptsContent(list1, list2, focusedItem);
					}
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.SelectScriptToDraw(y);
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("ScriptForegroundColor", MySpaceTexts.BlockPropertyTitle_FontColor)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				Getter = (T x) => (x.PanelComponent == null) ? Color.White : x.PanelComponent.ScriptForegroundColor,
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.ScriptForegroundColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("ScriptBackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, isAutoscaleEnabled: true, 0.055f, isAutosEllipsisEnabled: true)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				Getter = (T x) => (x.PanelComponent == null) ? Color.Black : x.PanelComponent.ScriptBackgroundColor,
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.ScriptBackgroundColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("ShowTextPanel", MySpaceTexts.BlockPropertyTitle_TextPanelShowPublicTextPanel, MySpaceTexts.Blank, delegate(T x)
			{
				x.OpenWindow(isEditable: true, sync: true, isPublic: true);
			})
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0 && !x.IsTextPanelOpen,
				SupportsMultipleBlocks = false
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("Font", MySpaceTexts.BlockPropertyTitle_Font, MySpaceTexts.Blank)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					MyTextPanelComponent.FillFontComboBoxContent(x);
				},
				Getter = (T x) => (x.PanelComponent == null) ? 0 : ((int)x.PanelComponent.Font.SubtypeId),
				Setter = delegate(T x, long y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.Font = new MyDefinitionId(typeof(MyObjectBuilder_FontDefinition), MyStringHash.TryGet((int)y));
					}
				}
			});
			MyTerminalControlSlider<T> myTerminalControlSlider = new MyTerminalControlSlider<T>("FontSize", MySpaceTexts.BlockPropertyTitle_LCDScreenTextSize, MySpaceTexts.Blank);
<<<<<<< HEAD
			myTerminalControlSlider.Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd;
=======
			myTerminalControlSlider.Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myTerminalControlSlider.Enabled = (T x) => x.SurfaceCount > 0;
			myTerminalControlSlider.SetLimits(0.1f, 10f);
			myTerminalControlSlider.DefaultValue = 1f;
			myTerminalControlSlider.Getter = (T x) => (x.PanelComponent == null) ? 1f : x.PanelComponent.FontSize;
			myTerminalControlSlider.Setter = delegate(T x, float v)
			{
				if (x.PanelComponent != null)
				{
					x.PanelComponent.FontSize = v;
				}
			};
			myTerminalControlSlider.Writer = delegate(T x, StringBuilder result)
			{
				if (x.PanelComponent != null)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.FontSize, 3));
				}
			};
			myTerminalControlSlider.EnableActions(0.05f, (T x) => x.SurfaceCount > 0, (T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("FontColor", MySpaceTexts.BlockPropertyTitle_FontColor)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				Getter = (T x) => (x.PanelComponent == null) ? Color.White : x.PanelComponent.FontColor,
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FontColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("alignment", MySpaceTexts.BlockPropertyTitle_Alignment, MySpaceTexts.Blank)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					MyTextPanelComponent.FillAlignmentComboBoxContent(x);
				},
				Getter = (T x) => (long)((x.PanelComponent == null) ? TextAlignment.LEFT : x.PanelComponent.Alignment),
				Setter = delegate(T x, long y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.Alignment = (TextAlignment)y;
					}
				}
			});
			MyTerminalControlSlider<T> myTerminalControlSlider2 = new MyTerminalControlSlider<T>("TextPaddingSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenTextPadding, MySpaceTexts.Blank);
<<<<<<< HEAD
			myTerminalControlSlider2.Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd;
=======
			myTerminalControlSlider2.Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myTerminalControlSlider2.Enabled = (T x) => x.SurfaceCount > 0;
			myTerminalControlSlider2.SetLimits(0f, 50f);
			myTerminalControlSlider2.DefaultValue = 0f;
			myTerminalControlSlider2.Getter = (T x) => (x.PanelComponent == null) ? 0f : x.PanelComponent.TextPadding;
			myTerminalControlSlider2.Setter = delegate(T x, float v)
			{
				if (x.PanelComponent != null)
				{
					x.PanelComponent.TextPadding = v;
				}
			};
			myTerminalControlSlider2.Writer = delegate(T x, StringBuilder result)
			{
				if (x.PanelComponent != null)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.TextPadding, 1)).Append("%");
				}
			};
			myTerminalControlSlider2.EnableActions(0.05f, (T x) => x.SurfaceCount > 0, (T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && (x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE || x.PanelComponent.ContentType == ContentType.SCRIPT) && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && (x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE || x.PanelComponent.ContentType == ContentType.SCRIPT),
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("BackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, isAutoscaleEnabled: true, 0.055f, isAutosEllipsisEnabled: true)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				Getter = (T x) => (x.PanelComponent == null) ? Color.Black : x.PanelComponent.BackgroundColor,
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.BackgroundColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("ImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenDefinitionsTextures, MySpaceTexts.Blank, multiSelect: true)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FillListContent(list1, list2);
					}
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.PanelComponent.SelectImageToDraw(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("SelectTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectTextures, MySpaceTexts.Blank, delegate(T x)
			{
				x.PanelComponent.AddImagesToSelection();
			})
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
				Enabled = (T x) => x.SurfaceCount > 0
			});
			MyTerminalControlSlider<T> myTerminalControlSlider3 = new MyTerminalControlSlider<T>("ChangeIntervalSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenRefreshInterval, MySpaceTexts.Blank, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
			myTerminalControlSlider3.Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd;
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				Enabled = (T x) => x.SurfaceCount > 0
			});
			MyTerminalControlSlider<T> myTerminalControlSlider3 = new MyTerminalControlSlider<T>("ChangeIntervalSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenRefreshInterval, MySpaceTexts.Blank, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
			myTerminalControlSlider3.Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myTerminalControlSlider3.Enabled = (T x) => x.SurfaceCount > 0;
			myTerminalControlSlider3.SetLimits(0f, 30f);
			myTerminalControlSlider3.DefaultValue = 0f;
			myTerminalControlSlider3.Getter = (T x) => (x.PanelComponent == null) ? 0f : x.PanelComponent.ChangeInterval;
			myTerminalControlSlider3.Setter = delegate(T x, float v)
			{
				if (x.PanelComponent != null)
				{
					x.PanelComponent.ChangeInterval = v;
				}
			};
			myTerminalControlSlider3.Writer = delegate(T x, StringBuilder result)
			{
				if (x.PanelComponent != null)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.ChangeInterval, 3)).Append(" s");
				}
			};
			myTerminalControlSlider3.EnableActions(0.05f, (T x) => x.SurfaceCount > 0, (T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("SelectedImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectedTextures, MySpaceTexts.Blank, multiSelect: true)
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0,
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FillSelectedListContent(list1, list2);
					}
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.SelectImage(y);
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("RemoveSelectedTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenRemoveSelectedTextures, MySpaceTexts.Blank, delegate(T x)
			{
				x.PanelComponent.RemoveImagesFromSelection();
			})
			{
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0
			});
			MyTerminalControlCheckbox<T> obj = new MyTerminalControlCheckbox<T>("PreserveAspectRatio", MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio, MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio)
			{
				Getter = (T x) => x.PanelComponent != null && x.PanelComponent.PreserveAspectRatio,
				Setter = delegate(T x, bool v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.PreserveAspectRatio = v;
					}
				},
<<<<<<< HEAD
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE && x.UseGenericLcd,
=======
				Visible = (T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Enabled = (T x) => x.SurfaceCount > 0
			};
			obj.EnableAction((T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(obj);
		}

		private static void FillPanels(MyMultiTextPanelComponent multiText, ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems, ICollection<MyGuiControlListbox.Item> lastFocused)
		{
			MyGuiControlListbox.Item item = null;
			listBoxContent.Clear();
			listBoxSelectedItems.Clear();
			if (multiText == null)
			{
				return;
			}
			for (int i = 0; i < multiText.m_panels.Count; i++)
			{
				MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(new StringBuilder(multiText.m_panels[i].DisplayName), null, null, i);
				listBoxContent.Add(item2);
				if (multiText.m_selectedPanel == i)
				{
					listBoxSelectedItems.Add(item2);
					item = item2;
				}
			}
			if (item != null)
			{
				lastFocused.Add(item);
			}
		}

		public MyMultiTextPanelComponent(MyTerminalBlock block, List<ScreenArea> screens, List<MySerializedTextPanelData> panels, bool useOnlineTexture = true)
		{
			m_block = block;
			m_block.OnClosing += OnClosing;
			m_render = block.Render as MyRenderComponentScreenAreas;
			if (screens.Count <= 0)
			{
				return;
			}
			m_panels = new List<MyTextPanelComponent>();
			for (int i = 0; i < screens.Count; i++)
			{
				ScreenArea screenArea = screens[i];
				string @string = MyTexts.GetString(screenArea.DisplayName);
				MyTextPanelComponent myTextPanelComponent = new MyTextPanelComponent(i, block, screenArea.Name, @string, screenArea.TextureResolution, screens.Count, screenArea.ScreenWidth, screenArea.ScreenHeight, useOnlineTexture);
				m_panels.Add(myTextPanelComponent);
				block.SyncType.Append(myTextPanelComponent);
				myTextPanelComponent.Init((panels != null && panels.Count > i) ? panels[i].Sprites : default(MySerializableSpriteCollection), screenArea.Script, AddImagesRequest, RemoveImagesRequest, ChangeTextRequest, SpriteCollectionUpdate);
			}
			if (panels != null)
			{
				MyDefinitionManager.Static.GetLCDTexturesDefinitions();
				for (int j = 0; j < panels.Count && j < screens.Count; j++)
				{
					SetPanelData(panels[j], j);
				}
			}
		}

		public void SetPanelData(MySerializedTextPanelData serializedData, int panelIndex)
		{
			MyTextPanelComponent.ContentMetadata contentMetadata = default(MyTextPanelComponent.ContentMetadata);
			contentMetadata.ContentType = serializedData.ContentType;
			contentMetadata.BackgroundColor = serializedData.BackgroundColor;
			contentMetadata.ChangeInterval = serializedData.ChangeInterval;
			contentMetadata.PreserveAspectRatio = serializedData.PreserveAspectRatio;
			contentMetadata.TextPadding = serializedData.TextPadding;
			MyTextPanelComponent.ContentMetadata content = contentMetadata;
			MyTextPanelComponent.FontData fontData = default(MyTextPanelComponent.FontData);
			fontData.Alignment = (TextAlignment)serializedData.Alignment;
			fontData.Size = serializedData.FontSize;
			fontData.TextColor = serializedData.FontColor;
			fontData.Name = serializedData.Font.SubtypeName;
			MyTextPanelComponent.FontData font = fontData;
			MyTextPanelComponent.ScriptData scriptData = default(MyTextPanelComponent.ScriptData);
			scriptData.Script = serializedData.SelectedScript ?? string.Empty;
			scriptData.CustomizeScript = serializedData.CustomizeScripts;
			scriptData.BackgroundColor = serializedData.ScriptBackgroundColor;
			scriptData.ForegroundColor = serializedData.ScriptForegroundColor;
			MyTextPanelComponent.ScriptData script = scriptData;
			m_panels[panelIndex].CurrentSelectedTexture = serializedData.CurrentShownTexture;
			if (serializedData.SelectedImages != null)
			{
				foreach (string selectedImage in serializedData.SelectedImages)
				{
					MyLCDTextureDefinition definition = MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(selectedImage);
					if (definition != null)
					{
						m_panels[panelIndex].SelectedTexturesToDraw.Add(definition);
					}
				}
				m_panels[panelIndex].CurrentSelectedTexture = Math.Min(m_panels[panelIndex].CurrentSelectedTexture, m_panels[panelIndex].SelectedTexturesToDraw.Count);
<<<<<<< HEAD
			}
			m_panels[panelIndex].Text = new StringBuilder(serializedData.Text);
			if (serializedData.ContentType == ContentType.IMAGE)
			{
				content.ContentType = ContentType.TEXT_AND_IMAGE;
			}
			else
			{
				content.ContentType = serializedData.ContentType;
			}
=======
			}
			m_panels[panelIndex].Text = new StringBuilder(serializedData.Text);
			if (serializedData.ContentType == ContentType.IMAGE)
			{
				content.ContentType = ContentType.TEXT_AND_IMAGE;
			}
			else
			{
				content.ContentType = serializedData.ContentType;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_panels[panelIndex].SetLocalValues(content, font, script);
		}

		public void Init(Action<int, int[]> addImagesRequest, Action<int, int[]> removeImagesRequest, Action<int, string> changeTextRequest, Action<int, MySerializableSpriteCollection> updateSpriteCollection, float maxRenderDistance = 120f)
		{
			m_panelsComponent = MySession.Static.GetComponent<MySessionComponentPanels>();
			RangeIndex = -1;
			m_addImagesToSelectionRequest = addImagesRequest;
			m_removeImagesFromSelectionRequest = removeImagesRequest;
			m_changeTextRequest = changeTextRequest;
			m_updateSpriteCollection = updateSpriteCollection;
			m_maxRenderDistanceSquared = maxRenderDistance * maxRenderDistance;
		}

		private void OnClosing(MyEntity e)
		{
			m_panelsComponent?.Remove(this);
		}

		private void AddImagesRequest(MyTextPanelComponent panel, int[] selection)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1 && m_addImagesToSelectionRequest != null)
				{
					m_addImagesToSelectionRequest(num, selection);
				}
			}
		}

		public void SelectItems(int panelIndex, int[] selection)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].SelectItems(selection);
			}
		}

		private void RemoveImagesRequest(MyTextPanelComponent panel, int[] selection)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1 && m_removeImagesFromSelectionRequest != null)
				{
					m_removeImagesFromSelectionRequest(num, selection);
				}
			}
		}

		public void RemoveItems(int panelIndex, int[] selection)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].RemoveItems(selection);
			}
		}

		private void ChangeTextRequest(MyTextPanelComponent panel, string text)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1)
				{
					m_changeTextRequest?.Invoke(num, text);
				}
			}
		}

		public void ChangeText(int panelIndex, string text)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].Text = new StringBuilder(text);
			}
		}

		private void SpriteCollectionUpdate(MyTextPanelComponent panel, MySerializableSpriteCollection sprites)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1)
				{
					m_updateSpriteCollection?.Invoke(num, sprites);
				}
			}
		}

		public void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].UpdateSpriteCollection(sprites);
			}
		}

		public void SetRender(MyRenderComponentScreenAreas render)
		{
			m_render = render;
			if (m_panels != null && m_panels.Count != 0)
			{
				for (int i = 0; i < m_panels.Count; i++)
				{
					m_panels[i].SetRender(m_render);
				}
			}
		}

		public void AddToScene(int? renderObjectIndex = null)
		{
			if (m_render == null)
			{
				return;
			}
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.SetRender(m_render);
				panel.Reset();
				m_render.AddScreenArea(m_render.RenderObjectIDs, panel.Name);
				if (renderObjectIndex.HasValue)
				{
					panel.SetRenderObjectIndex(renderObjectIndex.Value);
				}
			}
		}

		public void Reset()
		{
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.Reset();
			}
		}

		public List<MySerializedTextPanelData> Serialize()
		{
			if (m_panels.Count > 0)
			{
				List<MySerializedTextPanelData> list = new List<MySerializedTextPanelData>();
				for (int i = 0; i < m_panels.Count; i++)
				{
					MySerializedTextPanelData item = SerializePanel(i);
					list.Add(item);
				}
				return list;
			}
			return null;
		}

		public MySerializedTextPanelData SerializePanel(int panelIndex)
		{
			MySerializedTextPanelData mySerializedTextPanelData = new MySerializedTextPanelData();
			mySerializedTextPanelData.Alignment = (int)m_panels[panelIndex].Alignment;
			mySerializedTextPanelData.BackgroundColor = m_panels[panelIndex].BackgroundColor;
			mySerializedTextPanelData.ChangeInterval = m_panels[panelIndex].ChangeInterval;
			mySerializedTextPanelData.CurrentShownTexture = m_panels[panelIndex].CurrentSelectedTexture;
			mySerializedTextPanelData.Font = m_panels[panelIndex].Font;
			mySerializedTextPanelData.FontColor = m_panels[panelIndex].FontColor;
			mySerializedTextPanelData.FontSize = m_panels[panelIndex].FontSize;
			if (m_panels[panelIndex].SelectedTexturesToDraw.Count > 0)
			{
				mySerializedTextPanelData.SelectedImages = new List<string>();
				foreach (MyLCDTextureDefinition item in m_panels[panelIndex].SelectedTexturesToDraw)
				{
					mySerializedTextPanelData.SelectedImages.Add(item.Id.SubtypeName);
				}
			}
			mySerializedTextPanelData.Text = m_panels[panelIndex].Text.ToString();
			mySerializedTextPanelData.TextPadding = m_panels[panelIndex].TextPadding;
			mySerializedTextPanelData.PreserveAspectRatio = m_panels[panelIndex].PreserveAspectRatio;
			mySerializedTextPanelData.ContentType = ((m_panels[panelIndex].ContentType == ContentType.IMAGE) ? ContentType.TEXT_AND_IMAGE : m_panels[panelIndex].ContentType);
			mySerializedTextPanelData.SelectedScript = m_panels[panelIndex].Script;
			mySerializedTextPanelData.CustomizeScripts = m_panels[panelIndex].CustomizeScripts;
			mySerializedTextPanelData.ScriptBackgroundColor = m_panels[panelIndex].ScriptBackgroundColor;
			mySerializedTextPanelData.ScriptForegroundColor = m_panels[panelIndex].ScriptForegroundColor;
			if (MyReplicationLayer.CurrentSerializingReplicable != null)
			{
				mySerializedTextPanelData.Sprites = m_panels[panelIndex].ExternalSprites;
			}
			return mySerializedTextPanelData;
		}

		public void SelectPanel(int index)
		{
			m_selectedPanel = index;
		}

		public void UpdateScreen(bool isWorking)
		{
			if (!m_block.IsFunctional)
			{
				ReleaseTextures();
				return;
			}
			m_texturesReleased = false;
			bool flag = IsInRange();
			if (flag)
			{
				m_wasInRange = flag;
			}
			for (int i = 0; i < m_panels.Count; i++)
			{
				m_panels[i].UpdateAfterSimulation(isWorking, flag);
			}
		}

		public void UpdateAfterSimulation(bool isWorking = true)
		{
			if (m_block.IsFunctional)
			{
				UpdateScreen(isWorking);
			}
			else
			{
				ReleaseTextures();
			}
		}

		private bool IsInRange()
		{
			if (IsContentStatic())
			{
				m_panelsComponent.Remove(this);
				return true;
			}
			return m_panelsComponent.IsInRange(this, m_maxRenderDistanceSquared);
		}

		private void ReleaseTextures()
		{
			if (m_texturesReleased)
			{
				return;
			}
			m_texturesReleased = true;
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.ReleaseTexture(useEmptyTexture: false);
			}
		}

		public IMyTextSurface GetSurface(int index)
		{
			if (index >= 0 && m_panels != null && index < m_panels.Count)
			{
				return m_panels[index];
			}
			return null;
		}

		private bool IsContentStatic()
		{
			bool flag = m_panels.Count != 0;
			foreach (MyTextPanelComponent panel in m_panels)
			{
				if (panel != null)
				{
					flag &= panel.IsStatic;
				}
			}
			return flag;
		}

		IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index)
		{
			return GetSurface(index);
		}
	}
}
