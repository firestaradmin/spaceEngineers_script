using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyGuiBlueprintScreen_Reworked : MyGuiScreenDebugBase
	{
		public enum SortOption
		{
			None,
			Alphabetical,
			CreationDate,
			UpdateDate
		}

		private enum WaitForScreenshotOptions
		{
			None,
			TakeScreenshotLocal,
			CreateNewBlueprintCloud,
			TakeScreenshotCloud
		}

		private class MyWaitForScreenshotData
		{
			private bool m_isSet;

			public WaitForScreenshotOptions Option { get; private set; }

			public MyGuiControlContentButton UsedButton { get; private set; }

			public MyObjectBuilder_Definitions Prefab { get; private set; }

			public string PathRel { get; private set; }

			public string PathFull { get; private set; }

			public MyWaitForScreenshotData()
			{
				Clear();
			}

			public bool SetData_TakeScreenshotLocal(MyGuiControlContentButton button)
			{
				if (m_isSet)
				{
					return false;
				}
				m_isSet = true;
				Option = WaitForScreenshotOptions.TakeScreenshotLocal;
				UsedButton = button;
				return true;
			}

			public bool SetData_CreateNewBlueprintCloud(string pathRel, string pathFull)
			{
				if (m_isSet)
				{
					return false;
				}
				m_isSet = true;
				Option = WaitForScreenshotOptions.CreateNewBlueprintCloud;
				PathRel = pathRel;
				PathFull = pathFull;
				return true;
			}

			public bool SetData_TakeScreenshotCloud(string pathRel, string pathFull, MyGuiControlContentButton button)
			{
				if (m_isSet)
				{
					return false;
				}
				m_isSet = true;
				Option = WaitForScreenshotOptions.TakeScreenshotCloud;
				PathRel = pathRel;
				PathFull = pathFull;
				UsedButton = button;
				return true;
			}

			public bool IsWaiting()
			{
				return m_isSet;
			}

			public void Clear()
			{
				m_isSet = false;
				Option = WaitForScreenshotOptions.None;
				UsedButton = null;
				PathRel = string.Empty;
				PathFull = string.Empty;
				UsedButton = null;
			}
		}

		private enum ScrollTestResult
		{
			Ok,
			Higher,
			Lower
		}

		private class CloudBlueprintRenamer
		{
			private readonly Action<CloudResult, string> m_onComplete;

			private string m_newFilePath;

			private int m_filesSaved;

			private int m_filesToSave = 2;

			public CloudBlueprintRenamer(MyObjectBuilder_Definitions blueprint, string localImagePath, string bpNewName, Action<CloudResult, string> onComplete)
			{
				m_onComplete = onComplete;
				StartWorkflow(blueprint, bpNewName, localImagePath);
			}

			private void StartWorkflow(MyObjectBuilder_Definitions m_blueprint, string bpNewName, string localImagePath)
			{
				string path = Path.Combine("Blueprints/cloud", bpNewName);
				m_newFilePath = Path.Combine(path, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
				string fileName = "Blueprints/cloud/" + m_blueprint.ShipBlueprints[0].Id.SubtypeId + "/" + MyBlueprintUtils.THUMB_IMAGE_NAME;
				m_newFilePath = m_newFilePath.Replace("\\", "/");
				string fileName2 = m_newFilePath.Replace("bp.sbc", "thumb.png");
				m_blueprint.ShipBlueprints[0].Id.SubtypeId = bpNewName;
				m_blueprint.ShipBlueprints[0].Id.SubtypeName = bpNewName;
				m_blueprint.ShipBlueprints[0].CubeGrids[0].DisplayName = bpNewName;
				MyBlueprintUtils.SaveToCloud(m_blueprint, m_newFilePath, replace: true, delegate(string _, CloudResult result)
				{
					RenameBlueprintStepFinsihed(result);
				});
				byte[] array = ((!File.Exists(localImagePath)) ? MyGameService.LoadFromCloud(fileName) : File.ReadAllBytes(localImagePath));
				if (array != null && array.Length > 10)
				{
					MyGameService.SaveToCloudAsync(fileName2, array, RenameBlueprintStepFinsihed);
					MyGameService.DeleteFromCloud(fileName);
				}
				else
				{
					m_filesToSave--;
				}
			}

			private void RenameBlueprintStepFinsihed(CloudResult result)
			{
				if (result != 0)
				{
					m_onComplete?.Invoke(result, m_newFilePath);
					return;
				}
				m_filesSaved++;
				if (m_filesSaved == m_filesToSave)
				{
					m_onComplete?.Invoke(CloudResult.Ok, m_newFilePath);
				}
			}
		}

		protected sealed class ShareBlueprintRequest_003C_003ESystem_UInt64_0023System_String_0023System_String_0023System_UInt64_0023System_String : ICallSite<IMyEventOwner, ulong, string, string, ulong, string, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong workshopId, in string workshopServiceName, in string name, in ulong sendToId, in string senderName, in DBNull arg6)
			{
				ShareBlueprintRequest(workshopId, workshopServiceName, name, sendToId, senderName);
			}
		}

		protected sealed class ShareBlueprintRequestClient_003C_003ESystem_UInt64_0023System_String_0023System_String_0023System_UInt64_0023System_String : ICallSite<IMyEventOwner, ulong, string, string, ulong, string, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong workshopId, in string workshopServiceName, in string name, in ulong sendToId, in string senderName, in DBNull arg6)
			{
				ShareBlueprintRequestClient(workshopId, workshopServiceName, name, sendToId, senderName);
			}
		}

		public static readonly float MAGIC_SPACING_BIG = 0.00535f;

		public static readonly float MAGIC_SPACING_SMALL = 0.00888f;

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.95f, 0.97f);

		private static readonly float MARGIN_TOP = 0.22f;

		private const float MAX_BLUEPRINT_NAME_LABEL_WIDTH = 0.4f;

<<<<<<< HEAD
		private const float MAX_BLUEPRINT_PIXEL_COUNT = 262140f;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static HashSet<WorkshopId> m_downloadQueued = new HashSet<WorkshopId>();

		private static MyConcurrentHashSet<WorkshopId> m_downloadFinished = new MyConcurrentHashSet<WorkshopId>();

		private static MyWaitForScreenshotData m_waitingForScreenshot = new MyWaitForScreenshotData();

		private static bool m_downloadFromSteam = true;

		private static bool m_needsExtract = false;

		private static bool m_showDlcIcons = false;

		private static List<MyBlueprintItemInfo> m_recievedBlueprints = new List<MyBlueprintItemInfo>();

		private readonly List<MyGuiControlImage> m_dlcIcons = new List<MyGuiControlImage>();

		private static LoadPrefabData m_LoadPrefabData;

		private static Dictionary<Content, List<MyWorkshopItem>> m_subscribedItemsListDict = new Dictionary<Content, List<MyWorkshopItem>>();

		private static Dictionary<Content, string> m_currentLocalDirectoryDict = new Dictionary<Content, string>();

		private static Dictionary<Content, SortOption> m_selectedSortDict = new Dictionary<Content, SortOption>();

		private static Dictionary<Content, MyBlueprintTypeEnum> m_selectedBlueprintTypeDict = new Dictionary<Content, MyBlueprintTypeEnum>();

		private static Dictionary<Content, bool> m_thumbnailsVisibleDict = new Dictionary<Content, bool>();

		public static Task Task;

		public static readonly FastResourceLock SubscribedItemsLock = new FastResourceLock();

		private float m_guiMultilineHeight;

		private float m_guiAdditionalInfoOffset;

		private MyGridClipboard m_clipboard;

		private MyBlueprintAccessType m_accessType;

		private bool m_allowCopyToClipboard;

		private MyObjectBuilder_Definitions m_loadedPrefab;

		private bool m_blueprintBeingLoaded;

		private Action<string> m_onScriptOpened;

		private Func<string> m_getCodeFromEditor;

		private Action m_onCloseAction;

		private MyBlueprintItemInfo m_selectedBlueprint;

		private bool m_wasJoystickLastUsed;

		private float m_margin_left;

		private float ratingButtonsGap = 0.01f;

		private MyGuiControlContentButton m_selectedButton;

		private MyGuiControlRadioButtonGroup m_BPTypesGroup;

		private MyGuiControlList m_BPList;

		private Content m_content;

		private bool m_wasPublished;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlMultilineText m_multiline;

		private MyGuiControlPanel m_detailsBackground;

		private MyGuiControlLabel m_detailName;

		private MyGuiControlLabel m_detailBlockCount;

		private MyGuiControlLabel m_detailBlockCountValue;

		private MyGuiControlLabel m_detailGridTypeValue;

		private MyGuiControlLabel m_detailSize;

		private MyGuiControlLabel m_detailSizeValue;

		private MyGuiControlLabel m_detailAuthor;

		private MyGuiControlLabel m_detailAuthorName;

		private MyGuiControlLabel m_detailDLC;

		private MyGuiControlRating m_detailRatingDisplay;

		private MyGuiControlButton m_button_RateUp;

		private MyGuiControlButton m_button_RateDown;

		private MyGuiControlImage m_icon_RateUp;

		private MyGuiControlImage m_icon_RateDown;

		private MyGuiControlLabel m_detailGridType;

		private MyGuiControlLabel m_detailSendTo;

		private MyGuiControlButton m_button_Refresh;

		private MyGuiControlButton m_button_GroupSelection;

		private MyGuiControlButton m_button_Sorting;

		private MyGuiControlButton m_button_OpenWorkshop;

		private MyGuiControlButton m_button_NewBlueprint;

		private MyGuiControlButton m_button_DirectorySelection;

		private MyGuiControlButton m_button_HideThumbnails;

		private MyGuiControlButton m_button_OpenInWorkshop;

		private MyGuiControlButton m_button_CopyToClipboard;

		private MyGuiControlButton m_button_Rename;

		private MyGuiControlButton m_button_Replace;

		private MyGuiControlButton m_button_Delete;

		private MyGuiControlButton m_button_TakeScreenshot;

		private MyGuiControlButton m_button_Publish;

		private MyGuiControlCombobox m_sendToCombo;

		private MyGuiControlImage m_icon_Refresh;

		private MyGuiControlImage m_icon_GroupSelection;

		private MyGuiControlImage m_icon_Sorting;

		private MyGuiControlImage m_icon_OpenWorkshop;

		private MyGuiControlImage m_icon_DirectorySelection;

		private MyGuiControlImage m_icon_NewBlueprint;

		private MyGuiControlImage m_icon_HideThumbnails;

		private MyGuiControlImage m_thumbnailImage;

		private MyGuiControlLabel m_workshopError;

		private static bool m_newScreenshotTaken;

		private bool m_multipleServices;

		private string m_mixedIcon;

		private int m_workshopIndex;

		private bool m_workshopPermitted;

		private float m_leftSideSizeX;

		private MyBlueprintItemInfo SelectedBlueprint
		{
			get
			{
				return m_selectedBlueprint;
			}
			set
			{
				if (m_selectedBlueprint != value)
				{
					m_selectedBlueprint = value;
					SelectedBlueprintChanged();
				}
			}
		}

		private void SelectedBlueprintChanged()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				string @string = MyTexts.GetString((m_selectedBlueprint != null && (m_selectedBlueprint.Type == MyBlueprintTypeEnum.LOCAL || m_selectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)) ? MySpaceTexts.BlueprintScreen_Help_Screen_Local : MySpaceTexts.BlueprintScreen_Help_Screen);
				base.GamepadHelpText = string.Format(@string, (m_selectedBlueprint?.Item == null) ? MyGameService.GetDefaultUGC().ServiceName : m_selectedBlueprint.Item.ServiceName);
			}
			m_wasJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
		}

		public List<MyWorkshopItem> GetSubscribedItemsList()
		{
			return GetSubscribedItemsList(m_content);
		}

		public static List<MyWorkshopItem> GetSubscribedItemsList(Content content)
		{
			if (!m_subscribedItemsListDict.ContainsKey(content))
			{
				m_subscribedItemsListDict.Add(content, new List<MyWorkshopItem>());
			}
			return m_subscribedItemsListDict[content];
		}

		public void SetSubscribeItemList(ref List<MyWorkshopItem> list)
		{
			SetSubscribeItemList(ref list, m_content);
		}

		public static void SetSubscribeItemList(ref List<MyWorkshopItem> list, Content content)
		{
			if (m_subscribedItemsListDict.ContainsKey(content))
			{
				m_subscribedItemsListDict[content] = list;
			}
			else
			{
				m_subscribedItemsListDict.Add(content, list);
			}
		}

		public string GetCurrentLocalDirectory()
		{
			return GetCurrentLocalDirectory(m_content);
		}

		public static string GetCurrentLocalDirectory(Content content)
		{
			if (!m_currentLocalDirectoryDict.ContainsKey(content))
			{
				m_currentLocalDirectoryDict.Add(content, string.Empty);
			}
			return m_currentLocalDirectoryDict[content];
		}

		public void SetCurrentLocalDirectory(string path)
		{
			SetCurrentLocalDirectory(m_content, path);
		}

		public static void SetCurrentLocalDirectory(Content content, string path)
		{
			if (m_currentLocalDirectoryDict.ContainsKey(content))
			{
				m_currentLocalDirectoryDict[content] = path;
			}
			else
			{
				m_currentLocalDirectoryDict.Add(content, path);
			}
		}

		private SortOption GetSelectedSort()
		{
			return GetSelectedSort(m_content);
		}

		private SortOption GetSelectedSort(Content content)
		{
			if (!m_selectedSortDict.ContainsKey(content))
			{
				m_selectedSortDict.Add(content, SortOption.None);
			}
			return m_selectedSortDict[content];
		}

		public MyBlueprintTypeEnum GetSelectedBlueprintType()
		{
			return GetSelectedBlueprintType(m_content);
		}

		public MyBlueprintTypeEnum GetSelectedBlueprintType(Content content)
		{
			if (!m_selectedBlueprintTypeDict.ContainsKey(content))
			{
				m_selectedBlueprintTypeDict.Add(content, MyBlueprintTypeEnum.MIXED);
			}
			return m_selectedBlueprintTypeDict[content];
		}

		public bool GetThumbnailVisibility()
		{
			return GetThumbnailVisibility(m_content);
		}

		public bool GetThumbnailVisibility(Content content)
		{
			if (!m_thumbnailsVisibleDict.ContainsKey(content))
			{
				m_thumbnailsVisibleDict.Add(content, value: true);
			}
			return m_thumbnailsVisibleDict[content];
		}

		private void SetSelectedSort(SortOption option)
		{
			SetSelectedSort(m_content, option);
		}

		private static void SetSelectedSort(Content content, SortOption option)
		{
			if (m_selectedSortDict.ContainsKey(content))
			{
				m_selectedSortDict[content] = option;
			}
			else
			{
				m_selectedSortDict.Add(content, option);
			}
		}

		public void SetSelectedBlueprintType(MyBlueprintTypeEnum option)
		{
			SetSelectedBlueprintType(m_content, option);
		}

		public static void SetSelectedBlueprintType(Content content, MyBlueprintTypeEnum option)
		{
			if (m_selectedBlueprintTypeDict.ContainsKey(content))
			{
				m_selectedBlueprintTypeDict[content] = option;
			}
			else
			{
				m_selectedBlueprintTypeDict.Add(content, option);
			}
		}

		public void SetThumbnailVisibility(bool option)
		{
			SetThumbnailVisibility(m_content, option);
		}

		public static void SetThumbnailVisibility(Content content, bool option)
		{
			if (m_thumbnailsVisibleDict.ContainsKey(content))
			{
				m_thumbnailsVisibleDict[content] = option;
			}
			else
			{
				m_thumbnailsVisibleDict.Add(content, option);
			}
		}

		public static MyGuiBlueprintScreen_Reworked CreateBlueprintScreen(MyGridClipboard clipboard, bool allowCopyToClipboard, MyBlueprintAccessType accessType)
		{
			MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = new MyGuiBlueprintScreen_Reworked();
			myGuiBlueprintScreen_Reworked.SetBlueprintInitData(clipboard, allowCopyToClipboard, accessType);
			myGuiBlueprintScreen_Reworked.FinishInitialization();
			return myGuiBlueprintScreen_Reworked;
		}

		public static MyGuiBlueprintScreen_Reworked CreateScriptScreen(Action<string> onScriptOpened, Func<string> getCodeFromEditor, Action onCloseAction)
		{
			MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = new MyGuiBlueprintScreen_Reworked();
			myGuiBlueprintScreen_Reworked.SetScriptInitData(onScriptOpened, getCodeFromEditor, onCloseAction);
			myGuiBlueprintScreen_Reworked.FinishInitialization();
			return myGuiBlueprintScreen_Reworked;
		}

		private MyGuiBlueprintScreen_Reworked()
			: base(new Vector2(0.5f, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen: false)
		{
			List<IMyUGCService> aggregates = MyGameService.WorkshopService.GetAggregates();
			if (aggregates.Count > 1)
			{
				m_multipleServices = true;
				m_mixedIcon = "BP_Mixed.png";
			}
			else
			{
				m_multipleServices = false;
				m_mixedIcon = "BP_" + aggregates[0].ServiceName + "_Mixed.png";
			}
			base.CanHideOthers = true;
			m_canShareInput = false;
			base.CanBeHidden = true;
			m_canCloseInCloseAllScreenCalls = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			m_margin_left = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			InitializeBPList();
			m_BPList.Clear();
			m_BPTypesGroup.Clear();
		}

		private void SetBlueprintInitData(MyGridClipboard clipboard, bool allowCopyToClipboard, MyBlueprintAccessType accessType)
		{
			m_content = Content.Blueprint;
			m_accessType = accessType;
			m_clipboard = clipboard;
			m_allowCopyToClipboard = allowCopyToClipboard;
			CheckCurrentLocalDirectory_Blueprint();
			GetLocalNames_Blueprints(m_downloadFromSteam);
			ApplyFiltering();
		}

		private void SetScriptInitData(Action<string> onScriptOpened, Func<string> getCodeFromEditor, Action onCloseAction)
		{
			m_content = Content.Script;
			m_onScriptOpened = onScriptOpened;
			m_getCodeFromEditor = getCodeFromEditor;
			m_onCloseAction = onCloseAction;
			CheckCurrentLocalDirectory_Blueprint();
			using (SubscribedItemsLock.AcquireSharedUsing())
			{
				GetLocalNames_Scripts(m_downloadFromSteam);
				ApplyFiltering();
			}
		}

		private void FinishInitialization()
		{
			if (m_downloadFromSteam)
			{
				m_downloadFromSteam = false;
			}
			RecreateControls(constructor: true);
			TrySelectFirstBlueprint();
		}

		private void InitializeBPList()
		{
			m_BPTypesGroup = new MyGuiControlRadioButtonGroup();
			m_BPTypesGroup.SelectedChanged += OnSelectItem;
			m_BPTypesGroup.MouseDoubleClick += OnMouseDoubleClickItem;
			m_BPList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = -m_size.Value / 2f + new Vector2(m_margin_left, 0.307f),
				Size = new Vector2(0.2f, m_size.Value.Y - 0.307f - 0.068f)
			};
		}

		private void OnMouseDoubleClickItem(MyGuiControlRadioButton obj)
		{
			CopyToClipboard();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 vector = -m_size.Value / 2f + new Vector2(m_margin_left, MARGIN_TOP);
			m_guiMultilineHeight = 0.29f;
			m_guiAdditionalInfoOffset = 0.111f;
			float num = m_size.Value.X - 2f * m_margin_left;
			MyGuiControlMultilineText control = new MyGuiControlMultilineText
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.5f * m_size.Value.X + m_margin_left, -0.345f),
				Size = new Vector2(num, 0.05f),
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextEnum = ((m_content == Content.Blueprint) ? MyCommonTexts.BlueprintsScreen_Description : MyCommonTexts.ScriptsScreen_Description),
				Font = "Blue"
			};
			Controls.Add(control);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			float num2 = m_size.Value.X - m_margin_left - 0.275f;
			float num3 = -0.2f;
			myGuiControlSeparatorList.AddHorizontal(new Vector2(vector.X, -0.39f), num);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(vector.X, -0.299999982f), num);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(num3, -0.225000009f), num2);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(num3, 0.354f), num2);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(num3, 0.212f), num2);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(vector.X, m_size.Value.Y / 2f - 0.05f), num);
			Controls.Add(myGuiControlSeparatorList);
			MyStringId id = MySpaceTexts.ScreenBlueprintsRew_Caption;
			switch (m_content)
			{
			case Content.Blueprint:
				id = MySpaceTexts.ScreenBlueprintsRew_Caption_Blueprint;
				break;
			case Content.Script:
				id = MySpaceTexts.ScreenBlueprintsRew_Caption_Script;
				break;
			}
			AddCaption(MyTexts.GetString(id), Color.White.ToVector4(), new Vector2(0f, 0.02f));
			m_detailName = AddCaption("Blueprint Name", Color.White.ToVector4(), new Vector2(0.1035f, 0.175f));
			m_detailName.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_detailName.PositionX -= num2 / 2f - 0.007f;
			m_detailRatingDisplay = new MyGuiControlRating(10)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			Controls.Add(m_detailRatingDisplay);
			m_button_RateUp = CreateRateButton(positive: true);
			Controls.Add(m_button_RateUp);
			m_icon_RateUp = CreateRateIcon(m_button_RateUp, "Textures\\GUI\\Icons\\Blueprints\\like_test.png");
			Controls.Add(m_icon_RateUp);
			m_button_RateDown = CreateRateButton(positive: false);
			Controls.Add(m_button_RateDown);
			m_icon_RateDown = CreateRateIcon(m_button_RateDown, "Textures\\GUI\\Icons\\Blueprints\\dislike_test.png");
			Controls.Add(m_icon_RateDown);
			m_multiline = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, selectable: false, showTextShadow: false, null, new MyGuiBorderThickness(0.005f, 0f, 0f, 0f));
			m_multiline.CanHaveFocus = true;
			m_multiline.VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBordered;
			Controls.Add(m_multiline);
			m_detailsBackground = new MyGuiControlPanel();
			m_detailsBackground.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			Controls.Add(m_detailsBackground);
			m_searchBox = new MyGuiControlSearchBox(new Vector2(-0.382f, -0.21f), new Vector2(m_BPList.Size.X, 0.032f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_searchBox.OnTextChanged += OnSearchTextChange;
			Controls.Add(m_searchBox);
			m_detailBlockCount = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NumOfBlocks), string.Empty));
			Controls.Add(m_detailBlockCount);
			m_detailBlockCountValue = new MyGuiControlLabel(null, null, "0");
			Controls.Add(m_detailBlockCountValue);
			m_detailGridType = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_GridType), string.Empty));
			Controls.Add(m_detailGridType);
			m_detailGridTypeValue = new MyGuiControlLabel(null, null, "Unknown");
			Controls.Add(m_detailGridTypeValue);
			m_detailAuthor = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Author), string.Empty));
			Controls.Add(m_detailAuthor);
			m_detailAuthorName = new MyGuiControlLabel(null, null, "N/A");
			Controls.Add(m_detailAuthorName);
			m_detailSize = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Size), string.Empty));
			Controls.Add(m_detailSize);
			m_detailSizeValue = new MyGuiControlLabel(null, null, "Unknown");
			Controls.Add(m_detailSizeValue);
			m_detailDLC = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Dlc), string.Empty));
			Controls.Add(m_detailDLC);
			m_detailSendTo = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_PCU), string.Empty));
			Controls.Add(m_detailSendTo);
			if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
			{
				Controls.Add(new MySCloudStorageQuotaBar(new Vector2(-0.109f, 0.395f)));
			}
			UpdatePrefab(null, loadPrefab: false);
			UpdateInfo(null, null);
			m_sendToCombo = AddCombo(null, null, new Vector2(0.215f, 0.1f));
			m_sendToCombo.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_SendToPlayer);
			m_sendToCombo.AddItem(0L, new StringBuilder("   "));
			foreach (MyNetworkClient client in Sync.Clients.GetClients())
			{
				if (client.SteamUserId != Sync.MyId)
				{
					m_sendToCombo.AddItem(Convert.ToInt64(client.SteamUserId), new StringBuilder(client.DisplayName));
				}
			}
			m_sendToCombo.ItemSelected += OnSendToPlayer;
			CreateButtons();
			Controls.Add(m_BPList);
			switch (m_content)
			{
			case Content.Blueprint:
				m_detailSendTo.Visible = true;
				break;
			case Content.Script:
				m_sendToCombo.Visible = false;
				m_detailAuthor.Visible = false;
				m_detailAuthorName.Visible = false;
				m_detailBlockCount.Visible = false;
				m_detailBlockCountValue.Visible = false;
				m_detailGridType.Visible = false;
				m_detailGridTypeValue.Visible = false;
				m_detailSize.Visible = false;
				m_detailSizeValue.Visible = false;
				m_detailRatingDisplay.Visible = false;
				m_button_RateUp.Visible = false;
				m_icon_RateUp.Visible = false;
				m_button_RateDown.Visible = false;
				m_icon_RateDown.Visible = false;
				m_detailDLC.Visible = false;
				m_detailSendTo.Visible = false;
				break;
			}
			m_searchBox.Position = new Vector2(m_button_Refresh.Position.X - m_button_Refresh.Size.X * 0.5f - 0.002f, m_searchBox.Position.Y);
			m_searchBox.Size = new Vector2(m_leftSideSizeX, m_searchBox.Size.Y);
			m_BPList.Position = new Vector2(m_searchBox.Position.X, m_BPList.Position.Y);
			m_BPList.Size = new Vector2(m_searchBox.Size.X, m_BPList.Size.Y);
			m_workshopError = new MyGuiControlLabel(null, null, null, Color.Red);
			m_workshopError.Visible = false;
			m_workshopError.VisibleChanged += delegate
			{
				UpdateHintsPositions();
			};
			Controls.Add(m_workshopError);
			m_workshopPermitted = true;
			MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: false, delegate(bool granted)
			{
				if (!granted)
				{
					SetWorkshopErrorText(MyTexts.GetString(MySpaceTexts.WorkshopRestricted));
					m_workshopPermitted = false;
				}
			});
			RefreshThumbnail();
			Controls.Add(m_thumbnailImage);
			RepositionDetailedPage(num3, num2);
			SetDetailPageTexts();
			UpdateDetailKeyEnable();
			m_gamepadHelpLabel = new MyGuiControlLabel();
			m_gamepadHelpLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			m_gamepadHelpLabel.VisibleChanged += delegate
			{
				UpdateHintsPositions();
			};
			Controls.Add(m_gamepadHelpLabel);
			SelectedBlueprintChanged();
			base.FocusedControl = m_searchBox.TextBox;
			float num4 = 0.03f;
			foreach (MyGuiControlBase control2 in Controls)
			{
				control2.PositionY -= num4;
			}
			foreach (MyGuiControlBase element in Elements)
			{
				if (element != m_closeButton)
				{
					element.PositionY -= num4;
				}
			}
			UpdateHintsPositions();
			CheckUGCServices();
		}

		public void SetWorkshopErrorText(string text = "", bool visible = true, bool skipUGCCheck = false)
		{
			if (!skipUGCCheck && string.IsNullOrEmpty(text))
			{
				CheckUGCServices();
<<<<<<< HEAD
			}
			else if (m_workshopError != null)
			{
				m_workshopError.Text = text;
				m_workshopError.Visible = visible;
			}
=======
				return;
			}
			m_workshopError.Text = text;
			m_workshopError.Visible = visible;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void CreateButtons()
		{
			Vector2 vector = new Vector2(-0.402f, -0.27f);
			new Vector2(-0.0955f, -0.25f);
			Vector2 vector2 = new Vector2(-0.104f, 0.235f);
			Vector2 vector3 = new Vector2(-0.104f, 0.375f);
			new Vector2(0.144f, 0.035f);
			float num = 0.029f;
			float num2 = 0.188f;
			float textScale = 0.8f;
			int num3 = 0;
			m_button_Refresh = MyBlueprintUtils.CreateButton(this, num, null, OnButton_Refresh, enabled: true, null, textScale);
			m_button_Refresh.Position = vector + new Vector2(num, 0f) * num3++;
			m_button_Refresh.ShowTooltipWhenDisabled = true;
			m_button_GroupSelection = MyBlueprintUtils.CreateButton(this, num, null, OnButton_GroupSelection, enabled: true, null, textScale);
			m_button_GroupSelection.Position = vector + new Vector2(num, 0f) * num3++;
			m_button_GroupSelection.ShowTooltipWhenDisabled = true;
			m_button_Sorting = MyBlueprintUtils.CreateButton(this, num, null, OnButton_Sorting, enabled: true, null, textScale);
			m_button_Sorting.Position = vector + new Vector2(num, 0f) * num3++;
			m_button_Sorting.ShowTooltipWhenDisabled = true;
			m_button_NewBlueprint = MyBlueprintUtils.CreateButton(this, num, null, OnButton_NewBlueprint, enabled: true, null, textScale);
			m_button_NewBlueprint.Position = vector + new Vector2(num, 0f) * num3++;
			m_button_NewBlueprint.ShowTooltipWhenDisabled = true;
			if (MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE)
			{
				m_button_DirectorySelection = MyBlueprintUtils.CreateButton(this, num, null, OnButton_DirectorySelection, enabled: true, null, textScale);
				m_button_DirectorySelection.Position = vector + new Vector2(num, 0f) * num3++;
				m_button_DirectorySelection.ShowTooltipWhenDisabled = true;
			}
			m_button_HideThumbnails = MyBlueprintUtils.CreateButton(this, num, null, OnButton_HideThumbnails, enabled: true, null, textScale);
			m_button_HideThumbnails.Position = vector + new Vector2(num, 0f) * num3++;
			m_button_HideThumbnails.ShowTooltipWhenDisabled = true;
			m_button_OpenWorkshop = MyBlueprintUtils.CreateButton(this, num, null, OnButton_OpenWorkshop, enabled: true, null, textScale);
			m_button_OpenWorkshop.Position = vector + new Vector2(num, 0f) * num3++;
			m_button_OpenWorkshop.ShowTooltipWhenDisabled = true;
			m_leftSideSizeX = m_button_OpenWorkshop.Position.X + m_button_OpenWorkshop.Size.X - m_button_Refresh.Position.X;
			if (!MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE)
			{
				m_leftSideSizeX += num;
			}
			float num4 = 0.027f;
			m_button_Rename = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Rename, enabled: true, null, textScale);
			m_button_Rename.Position = vector2 + new Vector2(num2 + num4, 0f) * 0f;
			m_button_Rename.Size = new Vector2(m_button_Rename.Size.X, m_button_Rename.Size.Y * 1.3f);
			m_button_Rename.ShowTooltipWhenDisabled = true;
			m_button_Replace = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Replace, enabled: true, null, textScale);
			m_button_Replace.Position = vector2 + new Vector2(num2 + num4, 0f) * 1f;
			m_button_Replace.Size = new Vector2(m_button_Replace.Size.X, m_button_Replace.Size.Y * 1.3f);
			m_button_Replace.ShowTooltipWhenDisabled = true;
			m_button_Delete = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Delete, enabled: true, null, textScale);
			m_button_Delete.Position = vector2 + new Vector2(num2 + num4, 0f) * 2f;
			m_button_Delete.Size = new Vector2(m_button_Delete.Size.X, m_button_Delete.Size.Y * 1.3f);
			m_button_Delete.ShowTooltipWhenDisabled = true;
			m_button_TakeScreenshot = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_TakeScreenshot, enabled: true, null, textScale);
			m_button_TakeScreenshot.Position = vector2 + new Vector2(num2 + num4, 0f) * 1f + new Vector2(0f, 0.055f);
			m_button_TakeScreenshot.Size = new Vector2(m_button_TakeScreenshot.Size.X, m_button_TakeScreenshot.Size.Y * 1.3f);
			m_button_TakeScreenshot.ShowTooltipWhenDisabled = true;
			m_button_Publish = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Publish, enabled: true, null, textScale);
			m_button_Publish.Position = vector2 + new Vector2(num2 + num4, 0f) * 2f + new Vector2(0f, 0.055f);
			m_button_Publish.Size = new Vector2(m_button_Publish.Size.X, m_button_Publish.Size.Y * 1.3f);
			m_button_Publish.ShowTooltipWhenDisabled = true;
			m_button_OpenInWorkshop = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_OpenInWorkshop, enabled: true, null, textScale);
			m_button_OpenInWorkshop.Position = vector3 + new Vector2(num2 + num4, 0f) * 1f;
			m_button_OpenInWorkshop.Size = new Vector2(m_button_OpenInWorkshop.Size.X, m_button_OpenInWorkshop.Size.Y * 1.3f);
			m_button_OpenInWorkshop.ShowTooltipWhenDisabled = true;
			m_button_CopyToClipboard = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_CopyToClipboard, enabled: true, null, textScale);
			m_button_CopyToClipboard.Position = vector3 + new Vector2(num2 + num4, 0f) * 2f;
			m_button_CopyToClipboard.Size = new Vector2(m_button_CopyToClipboard.Size.X, m_button_CopyToClipboard.Size.Y * 1.3f);
			m_button_CopyToClipboard.ShowTooltipWhenDisabled = true;
			base.CloseButtonEnabled = true;
			m_icon_Refresh = CreateButtonIcon(m_button_Refresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			m_icon_GroupSelection = CreateButtonIcon(m_button_GroupSelection, "");
			SetIconForGroupSelection();
			m_icon_Sorting = CreateButtonIcon(m_button_Sorting, "");
			SetIconForSorting();
			m_icon_OpenWorkshop = CreateButtonIcon(m_button_OpenWorkshop, "Textures\\GUI\\Icons\\Browser\\WorkshopBrowser.dds");
			m_icon_DirectorySelection = CreateButtonIcon(m_button_NewBlueprint, "Textures\\GUI\\Icons\\Blueprints\\BP_New.png");
			m_icon_NewBlueprint = CreateButtonIcon(m_button_DirectorySelection, "Textures\\GUI\\Icons\\Blueprints\\FolderIcon.png");
			m_icon_HideThumbnails = CreateButtonIcon(m_button_HideThumbnails, "");
			SetIconForHideThubnails();
		}

		private MyGuiControlImage CreateButtonIcon(MyGuiControlButton butt, string texture)
		{
			if (butt == null)
			{
				return null;
			}
			butt.Size = new Vector2(butt.Size.X, butt.Size.X * 4f / 3f);
			float num = 0.95f * Math.Min(butt.Size.X, butt.Size.Y);
			MyGuiControlImage icon = new MyGuiControlImage(size: new Vector2(num * 0.75f, num), position: butt.Position + new Vector2(-0.0016f, 0.018f), backgroundColor: null, backgroundTexture: null, textures: new string[1] { texture });
			Controls.Add(icon);
			butt.FocusChanged += delegate(MyGuiControlBase x, bool focus)
			{
				icon.ColorMask = (focus ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
			};
			return icon;
		}

		private MyGuiControlButton CreateRateButton(bool positive)
		{
			return new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, onButtonClick: positive ? new Action<MyGuiControlButton>(OnRateUpClicked) : new Action<MyGuiControlButton>(OnRateDownClicked), size: new Vector2((m_detailRatingDisplay.GetWidth() - ratingButtonsGap) / 2f, 0.0342f), colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
		}

		private MyGuiControlImage CreateRateIcon(MyGuiControlButton button, string texture)
		{
			MyGuiControlImage icon = new MyGuiControlImage(null, new Vector2(button.Size.Y / 4f * 3f, button.Size.Y) * 0.6f, null, null, new string[1] { texture });
			button.HighlightChanged += delegate(MyGuiControlBase x)
			{
				icon.ColorMask = (x.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
			};
			return icon;
		}

		private void RepositionDetailedPage(float posX, float widthFromRight)
		{
			Vector2 vector = new Vector2(-0.168f, m_guiAdditionalInfoOffset);
			Vector2 vector2 = new Vector2(posX, -0.2655f);
			Vector2 vector3 = new Vector2(widthFromRight, m_guiMultilineHeight);
			Vector2 zero = Vector2.Zero;
			m_button_Rename.Visible = true;
			m_button_Replace.Visible = true;
			m_button_Delete.Visible = true;
			m_button_TakeScreenshot.Visible = m_content == Content.Blueprint;
			m_button_Publish.Visible = true;
			zero = new Vector2(0.394f, 0f) + new Vector2(-0.024f, 0.04f);
			m_multiline.Position = vector2 + 0.5f * vector3 + new Vector2(0f, 0.05f);
			m_multiline.Size = vector3;
			m_detailRatingDisplay.Position = new Vector2(posX + widthFromRight - m_detailRatingDisplay.GetWidth(), -0.2825f);
			m_button_RateUp.Position = m_detailRatingDisplay.Position + new Vector2((m_detailRatingDisplay.GetWidth() - ratingButtonsGap) / 2f, 0.034f);
			m_icon_RateUp.Position = m_button_RateUp.Position + new Vector2(-0.0015f, -0.0025f) - new Vector2(m_button_RateUp.Size.X / 2f, 0f);
			m_button_RateDown.Position = m_detailRatingDisplay.Position + new Vector2(m_detailRatingDisplay.GetWidth(), 0.034f);
			m_icon_RateDown.Position = m_button_RateDown.Position + new Vector2(-0.0015f, -0.0025f) - new Vector2(m_button_RateDown.Size.X / 2f, 0f);
			float x = m_detailBlockCount.Position.X + Math.Max(Math.Max(m_detailBlockCount.Size.X, m_detailGridType.Size.X), m_detailAuthor.Size.X) + 0.001f;
			m_detailAuthor.Position = vector + new Vector2(0f, 0f);
			m_detailBlockCount.Position = vector + new Vector2(0f, 0.03f);
			m_detailGridType.Position = vector + new Vector2(0f, 0.06f);
			m_detailAuthorName.Position = new Vector2(x, m_detailAuthor.Position.Y);
			m_detailBlockCountValue.Position = new Vector2(x, m_detailBlockCount.Position.Y);
			m_detailGridTypeValue.Position = new Vector2(x, m_detailGridType.Position.Y);
			float x2 = 0.27f;
			float x3 = (vector + zero).X;
			m_detailDLC.Position = vector + new Vector2(x2, 0f);
			m_detailSize.Position = vector + new Vector2(x2, 0.03f);
			m_detailSizeValue.Position = new Vector2(x3, m_detailSize.Position.Y);
			m_detailSendTo.Position = vector + new Vector2(x2, 0.06f);
			m_sendToCombo.Position = vector + zero;
			vector3 = m_sendToCombo.Position - vector;
			m_detailsBackground.Position = m_multiline.Position + new Vector2(0f, m_multiline.Size.Y / 2f + 0.0715f);
			m_detailsBackground.Size = new Vector2(m_multiline.Size.X, vector3.Y + m_sendToCombo.Size.Y + 0.02f);
			foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
			{
				Vector2 position = dlcIcon.Position;
				position.Y = vector.Y;
				dlcIcon.Position = position;
			}
		}

		private void UpdateHintsPositions()
		{
			float x = m_BPList.Position.X;
			if (m_gamepadHelpLabel.Visible && m_workshopError.Visible)
			{
				Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
				m_gamepadHelpLabel.Position = new Vector2(x, m_button_CopyToClipboard.Position.Y + minSizeGui.Y * 1.66f);
				m_workshopError.Position = new Vector2(x, 0.46f);
			}
			else
			{
				Vector2 position = new Vector2(x, 0.44f);
				m_workshopError.Position = position;
				m_gamepadHelpLabel.Position = position;
			}
		}

		private void SetDetailPageTexts()
		{
			m_button_Refresh.Text = null;
			m_button_Refresh.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButRefresh);
			m_button_GroupSelection.Text = null;
			List<IMyUGCService> aggregates = MyGameService.WorkshopService.GetAggregates();
			string serviceName = aggregates[0].ServiceName;
			string arg = "";
			MyStringId id;
			if (m_multipleServices)
			{
				arg = aggregates[1].ServiceName;
				id = (MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE ? MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping_Aggregator : MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping_NoLocal_Aggregator);
			}
			else
			{
				id = (MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE ? MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping : MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping_NoLocal);
			}
			m_button_GroupSelection.SetToolTip(string.Format(MyTexts.GetString(id), serviceName, arg));
			m_button_Sorting.Text = null;
			m_button_Sorting.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButSort);
			m_button_OpenWorkshop.Text = null;
			m_button_OpenWorkshop.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop);
<<<<<<< HEAD
			m_button_OpenWorkshop.Enabled = MyGameService.IsActive;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_button_DirectorySelection != null)
			{
				m_button_DirectorySelection.Text = null;
				m_button_DirectorySelection.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButFolders);
			}
			m_button_HideThumbnails.Text = null;
			m_button_HideThumbnails.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButVisibility);
			m_button_Rename.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButRename);
			m_button_Rename.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButRename);
			m_button_Replace.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButReplace);
			m_button_Replace.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButReplace);
			m_button_Delete.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButDelete);
			m_button_Delete.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButDelete);
			m_button_TakeScreenshot.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButScreenshot);
			m_button_TakeScreenshot.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButScreenshot);
			m_button_Publish.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButPublish);
			m_button_Publish.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButPublish);
			m_button_OpenInWorkshop.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButOpenInWorkshop);
			m_button_OpenInWorkshop.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenInWorkshop);
			switch (m_content)
			{
			case Content.Blueprint:
				m_button_NewBlueprint.Text = null;
				m_button_NewBlueprint.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButNewBlueprint);
				m_button_CopyToClipboard.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButToClipboard);
				m_button_CopyToClipboard.IsAutoScaleEnabled = true;
				m_button_CopyToClipboard.IsAutoEllipsisEnabled = true;
				m_button_CopyToClipboard.IsAutoScaleEnabled = true;
				m_button_CopyToClipboard.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButToClipboard);
				break;
			case Content.Script:
				m_button_NewBlueprint.Text = null;
				m_button_NewBlueprint.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButNewScript);
				m_button_CopyToClipboard.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButToEditor);
				m_button_CopyToClipboard.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButToEditor);
				break;
			}
		}

		private void SetIconForGroupSelection()
		{
			switch (GetSelectedBlueprintType())
			{
			case MyBlueprintTypeEnum.LOCAL:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Local.png");
				break;
			case MyBlueprintTypeEnum.WORKSHOP:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_" + MyGameService.WorkshopService.GetAggregates()[m_workshopIndex].ServiceName + ".png");
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Cloud.png");
				break;
			default:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\" + m_mixedIcon);
				break;
			}
		}

		private void SetIconForSorting()
		{
			switch (GetSelectedSort())
			{
			case SortOption.None:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			case SortOption.Alphabetical:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\Alphabetical.png");
				break;
			case SortOption.CreationDate:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByCreationDate.png");
				break;
			case SortOption.UpdateDate:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByUpdateDate.png");
				break;
			default:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			}
		}

		private void SetIconForHideThubnails()
		{
			if (GetThumbnailVisibility())
			{
				m_icon_HideThumbnails.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png");
			}
			else
			{
				m_icon_HideThumbnails.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ThumbnailsOFF.png");
			}
		}

		private void UpdateInfo(XDocument doc, MyBlueprintItemInfo data)
		{
			int num = 0;
			string text = string.Empty;
<<<<<<< HEAD
			string @string = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
			string text2 = @string;
			string text3 = @string;
=======
			string text2 = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
			string text3 = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyBlueprintItemInfo selectedBlueprint = SelectedBlueprint;
			MyGuiControlContentButton selectedButton = m_selectedButton;
			if (data != null && SelectedBlueprint != null && data.Equals(SelectedBlueprint))
			{
				text3 = MyValueFormatter.GetFormattedFileSizeInMB(data.Size);
				switch (m_content)
				{
				case Content.Blueprint:
					if (doc == null)
					{
						break;
					}
					try
					{
<<<<<<< HEAD
						IEnumerable<XElement> enumerable = doc.Descendants("GridSizeEnum");
						IEnumerable<XElement> enumerable2 = doc.Descendants("DisplayName");
						IEnumerable<XElement> enumerable3 = doc.Descendants("CubeBlocks");
						IEnumerable<XElement> enumerable4 = doc.Descendants("DLC");
						text2 = ((enumerable != null && enumerable.Count() > 0) ? ((string)enumerable.First()) : @string);
						text = ((enumerable2 != null && enumerable2.Count() > 0) ? ((string)enumerable2.First()) : @string);
						num = 0;
						if (enumerable3 != null && enumerable3.Count() > 0)
						{
							foreach (XElement item in enumerable3)
							{
								num += item.Elements().Count();
=======
						IEnumerable<XElement> enumerable = ((XContainer)doc).Descendants(XName.op_Implicit("GridSizeEnum"));
						IEnumerable<XElement> enumerable2 = ((XContainer)doc).Descendants(XName.op_Implicit("DisplayName"));
						IEnumerable<XElement> enumerable3 = ((XContainer)doc).Descendants(XName.op_Implicit("CubeBlocks"));
						IEnumerable<XElement> enumerable4 = ((XContainer)doc).Descendants(XName.op_Implicit("DLC"));
						text2 = ((enumerable != null && Enumerable.Count<XElement>(enumerable) > 0) ? ((string)Enumerable.First<XElement>(enumerable)) : "N/A");
						text = ((enumerable2 != null && Enumerable.Count<XElement>(enumerable2) > 0) ? ((string)Enumerable.First<XElement>(enumerable2)) : "N/A");
						num = 0;
						if (enumerable3 != null && Enumerable.Count<XElement>(enumerable3) > 0)
						{
							foreach (XElement item in enumerable3)
							{
								num += Enumerable.Count<XElement>(((XContainer)item).Elements());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
						if (enumerable4 == null)
						{
							break;
						}
<<<<<<< HEAD
						HashSet<uint> hashSet = new HashSet<uint>();
						foreach (XElement item2 in enumerable4)
						{
							if (!string.IsNullOrEmpty(item2.Value) && MyDLCs.TryGetDLC(item2.Value, out var dlc))
=======
						HashSet<uint> val = new HashSet<uint>();
						foreach (XElement item2 in enumerable4)
						{
							if (!string.IsNullOrEmpty(item2.get_Value()) && MyDLCs.TryGetDLC(item2.get_Value(), out var dlc))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								val.Add(dlc.AppId);
							}
						}
						if (val.get_Count() > 0)
						{
							selectedBlueprint.Data.DLCs = Enumerable.ToArray<uint>((IEnumerable<uint>)val);
						}
					}
					catch
					{
					}
					break;
				case Content.Script:
					text = ((data.Item != null) ? data.Item.OwnerId.ToString() : @string);
					break;
				}
			}
			if (selectedBlueprint == SelectedBlueprint && selectedButton == m_selectedButton)
			{
				m_detailDLC.Visible = false;
				m_detailRatingDisplay.Visible = false;
				m_button_RateUp.Visible = false;
				m_icon_RateUp.Visible = false;
				m_button_RateDown.Visible = false;
				m_icon_RateDown.Visible = false;
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					Controls.Remove(dlcIcon);
				}
				m_dlcIcons.Clear();
				if (selectedBlueprint != null)
				{
					if (!selectedBlueprint.Data.DLCs.IsNullOrEmpty())
					{
						m_detailDLC.Visible = true;
						Vector2 position = m_detailDLC.Position + new Vector2(MyGuiManager.MeasureString(m_detailDLC.Font, m_detailDLC.TextToDraw, m_detailDLC.TextScale).X, 0f);
						uint[] dLCs = selectedBlueprint.Data.DLCs;
						foreach (uint id in dLCs)
						{
							if (MyDLCs.TryGetDLC(id, out var dlc2))
							{
								MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1] { dlc2.Icon }, MyDLCs.GetRequiredDLCTooltip(id))
								{
									OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
									Size = new Vector2(32f) / MyGuiConstants.GUI_OPTIMAL_SIZE
								};
								myGuiControlImage.Position = position;
								position.X += myGuiControlImage.Size.X + 0.002f;
								m_dlcIcons.Add(myGuiControlImage);
								Controls.Add(myGuiControlImage);
							}
						}
					}
					if (selectedBlueprint.Item != null)
					{
						m_detailRatingDisplay.Visible = true;
						m_button_RateUp.Visible = true;
						m_icon_RateUp.Visible = true;
						m_button_RateDown.Visible = true;
						m_icon_RateDown.Visible = true;
						m_detailRatingDisplay.Value = (int)Math.Round(selectedBlueprint.Item.Score * 10f);
						selectedBlueprint.Item.UpdateRating();
						int myRating = selectedBlueprint.Item.MyRating;
						m_button_RateUp.Checked = myRating == 1;
						m_button_RateDown.Checked = myRating == -1;
					}
				}
				m_detailBlockCountValue.Text = num.ToString();
<<<<<<< HEAD
				if (text2.Equals(@string))
				{
					m_detailGridTypeValue.Text = text2;
				}
				else
				{
					m_detailGridTypeValue.Text = MyTexts.GetString("Blueprint_GridType_" + text2);
				}
=======
				m_detailGridTypeValue.Text = text2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_detailAuthorName.Text = text;
				m_detailSizeValue.Text = text3;
				m_detailSendTo.Text = MyTexts.GetString(MySpaceTexts.BlueprintInfo_SendTo);
				float x = m_detailBlockCount.Position.X + Math.Max(Math.Max(m_detailBlockCount.Size.X, m_detailGridType.Size.X), m_detailAuthor.Size.X) + 0.001f;
				m_detailBlockCountValue.Position = new Vector2(x, m_detailBlockCount.Position.Y);
				m_detailGridTypeValue.Position = new Vector2(x, m_detailGridType.Position.Y);
				m_detailAuthorName.Position = new Vector2(x, m_detailAuthor.Position.Y);
			}
			if (m_loadedPrefab != null)
			{
				UpdateDetailKeyEnable();
			}
		}

		private XDocument LoadXDocument(Stream sbcStream)
		{
			try
			{
				return XDocument.Load(sbcStream);
			}
			catch
			{
				return null;
			}
		}

		public void UpdateDetailKeyEnable()
		{
			if (SelectedBlueprint == null)
			{
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = false;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				return;
			}
			switch (SelectedBlueprint.Type)
			{
			case MyBlueprintTypeEnum.WORKSHOP:
				m_button_OpenInWorkshop.Enabled = true;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = true;
				break;
			case MyBlueprintTypeEnum.SHARED:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			case MyBlueprintTypeEnum.LOCAL:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = true;
				m_button_Replace.Enabled = true;
				m_button_Delete.Enabled = true;
				m_button_TakeScreenshot.Enabled = true;
				m_button_Publish.Enabled = m_workshopPermitted && MyFakes.ENABLE_WORKSHOP_PUBLISH;
				m_sendToCombo.Enabled = false;
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = true;
				m_button_Replace.Enabled = true;
				m_button_Delete.Enabled = true;
				m_button_TakeScreenshot.Enabled = true;
				m_button_Publish.Enabled = m_workshopPermitted && MyFakes.ENABLE_WORKSHOP_PUBLISH;
				m_sendToCombo.Enabled = false;
				break;
			case MyBlueprintTypeEnum.DEFAULT:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			default:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = false;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			}
		}

		private void TogglePreviewVisibility()
		{
			foreach (MyGuiControlBase control in m_BPList.Controls)
			{
				(control as MyGuiControlContentButton)?.SetPreviewVisibility(GetThumbnailVisibility());
			}
			m_BPList.Recalculate();
		}

		private void AddBlueprintButton(MyBlueprintItemInfo data, bool filter = false)
		{
			string blueprintName = data.BlueprintName;
			string imagePath = GetImagePath(data);
			if (File.Exists(imagePath))
			{
				MyRenderProxy.PreloadTextures(new string[1] { imagePath }, TextureType.GUIWithoutPremultiplyAlpha);
			}
			MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(blueprintName, imagePath)
			{
				UserData = data,
				Key = m_BPTypesGroup.Count
			};
			myGuiControlContentButton.SetModType(data.Type, data.Item?.ServiceName);
			myGuiControlContentButton.MouseOverChanged += OnMouseOverItem;
			myGuiControlContentButton.FocusChanged += OnFocusedItem;
			myGuiControlContentButton.SetTooltip(blueprintName);
			myGuiControlContentButton.SetPreviewVisibility(GetThumbnailVisibility());
			m_BPTypesGroup.Add(myGuiControlContentButton);
			m_BPList.Controls.Add(myGuiControlContentButton);
			if (filter)
			{
				ApplyFiltering(myGuiControlContentButton);
			}
		}

		public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
		{
			base.OnScreenOrderChanged(oldLast, newLast);
			CheckUGCServices();
		}

		private void CheckUGCServices()
		{
			string text = "";
			foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
			{
				if (!aggregate.IsConsentGiven)
				{
					text = aggregate.ServiceName;
				}
			}
			if (text != "")
			{
				SetWorkshopErrorText(text + MyTexts.GetString(MySpaceTexts.UGC_ServiceNotAvailable_NoConsent), visible: true, skipUGCCheck: true);
			}
			else
			{
				SetWorkshopErrorText("", visible: false, skipUGCCheck: true);
			}
		}

		private void AddBlueprintButtons(ref List<MyBlueprintItemInfo> data, bool filter = false)
		{
			int num = (MyVRage.Platform.System.IsMemoryLimited ? 10 : int.MaxValue);
			List<string> list = new List<string>(Math.Min(num, data.Count));
			List<string> list2 = new List<string>();
			for (int i = 0; i < data.Count; i++)
			{
				string imagePath = GetImagePath(data[i]);
				list2.Add(imagePath);
				if (list.Count < num && File.Exists(imagePath))
				{
					list.Add(imagePath);
				}
			}
			MyRenderProxy.PreloadTextures(list, TextureType.GUIWithoutPremultiplyAlpha);
			for (int j = 0; j < data.Count; j++)
			{
				string name = data[j].Data.Name;
				MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(name, File.Exists(list2[j]) ? list2[j] : "")
				{
					UserData = data[j],
					Key = m_BPTypesGroup.Count
				};
				myGuiControlContentButton.SetModType(data[j].Type, data[j].Item?.ServiceName);
				if (m_showDlcIcons)
				{
					if (data[j].Item != null && data[j].Item.DLCs.Count > 0)
					{
						foreach (uint dLC in data[j].Item.DLCs)
						{
							string dLCIcon = MyDLCs.GetDLCIcon(dLC);
							if (!string.IsNullOrEmpty(dLCIcon))
							{
								myGuiControlContentButton.AddDlcIcon(dLCIcon);
							}
						}
					}
					else if (data[j].Data.DLCs != null && data[j].Data.DLCs.Length != 0)
					{
						uint[] dLCs = data[j].Data.DLCs;
						for (int k = 0; k < dLCs.Length; k++)
						{
							string dLCIcon2 = MyDLCs.GetDLCIcon(dLCs[k]);
							if (!string.IsNullOrEmpty(dLCIcon2))
							{
								myGuiControlContentButton.AddDlcIcon(dLCIcon2);
							}
						}
					}
				}
				myGuiControlContentButton.MouseOverChanged += OnMouseOverItem;
				myGuiControlContentButton.FocusChanged += OnFocusedItem;
				myGuiControlContentButton.SetTooltip(name);
				myGuiControlContentButton.SetPreviewVisibility(GetThumbnailVisibility());
				m_BPTypesGroup.Add(myGuiControlContentButton);
				m_BPList.Controls.Add(myGuiControlContentButton);
				if (filter)
				{
					ApplyFiltering(myGuiControlContentButton);
				}
			}
		}

		private void TrySelectFirstBlueprint()
		{
			if (m_BPTypesGroup.Count <= 0 || ((!m_BPTypesGroup.SelectedIndex.HasValue || !m_BPTypesGroup.SelectedButton.Visible) && !m_BPTypesGroup.TrySelectFirstVisible()))
			{
				m_multiline.Clear();
				m_detailName.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
				MyBlueprintTypeEnum selectedBlueprintType = GetSelectedBlueprintType();
				if (selectedBlueprintType == MyBlueprintTypeEnum.WORKSHOP || selectedBlueprintType == MyBlueprintTypeEnum.MIXED)
				{
					m_multiline.AppendText(MyTexts.GetString((m_content == Content.Blueprint) ? MySpaceTexts.ScreenBlueprintsRew_NoWorkshopBlueprints : MySpaceTexts.ScreenBlueprintsRew_NoWorkshopScripts), "Blue", m_multiline.TextScale, Vector4.One);
					m_multiline.OnLinkClicked += OnLinkClicked;
				}
				else
				{
					m_multiline.AppendText(MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_NoBlueprints), "Blue", m_multiline.TextScale, Vector4.One);
				}
				m_multiline.ScrollbarOffsetV = 1f;
				UpdateInfo(null, null);
			}
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrlWithFallback(url, "Space Engineers Workshop");
		}

		private void OnButton_Refresh(MyGuiControlButton button)
		{
			bool flag = false;
			MyBlueprintItemInfo itemInfo = null;
			if (SelectedBlueprint != null)
			{
				flag = true;
				itemInfo = SelectedBlueprint;
			}
			m_selectedButton = null;
			SelectedBlueprint = null;
			UpdateDetailKeyEnable();
			m_downloadFinished.Clear();
			m_downloadQueued.Clear();
			RefreshAndReloadItemList();
			TrySelectFirstBlueprint();
			if (flag)
			{
				SelectBlueprint(itemInfo);
			}
			UpdateDetailKeyEnable();
		}

		private void OnButton_GroupSelection(MyGuiControlButton button)
		{
			MyBlueprintTypeEnum groupSelection = MyBlueprintTypeEnum.MIXED;
			switch (GetSelectedBlueprintType())
			{
			case MyBlueprintTypeEnum.MIXED:
				groupSelection = (MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE ? MyBlueprintTypeEnum.LOCAL : MyBlueprintTypeEnum.WORKSHOP);
				m_workshopIndex = 0;
				break;
			case MyBlueprintTypeEnum.LOCAL:
				groupSelection = MyBlueprintTypeEnum.WORKSHOP;
				m_workshopIndex = 0;
				break;
			case MyBlueprintTypeEnum.WORKSHOP:
				if (m_multipleServices && m_workshopIndex == 0)
				{
					groupSelection = MyBlueprintTypeEnum.WORKSHOP;
					m_workshopIndex++;
				}
				else
				{
					groupSelection = MyBlueprintTypeEnum.CLOUD;
					m_workshopIndex = 0;
				}
				break;
			case MyBlueprintTypeEnum.CLOUD:
				groupSelection = MyBlueprintTypeEnum.MIXED;
				break;
			}
			SetGroupSelection(groupSelection);
		}

		private void OnButton_Sorting(MyGuiControlButton button)
		{
			switch (GetSelectedSort())
			{
			case SortOption.None:
				SetSelectedSort(SortOption.Alphabetical);
				break;
			case SortOption.Alphabetical:
				SetSelectedSort(SortOption.CreationDate);
				break;
			case SortOption.CreationDate:
				SetSelectedSort(SortOption.UpdateDate);
				break;
			case SortOption.UpdateDate:
				SetSelectedSort(SortOption.None);
				break;
			}
			SetIconForSorting();
			OnReload(null);
		}

		private void OnButton_OpenWorkshop(MyGuiControlButton button)
		{
			MyWorkshop.OpenWorkshopBrowser((m_content == Content.Blueprint) ? MySteamConstants.TAG_BLUEPRINTS : MySteamConstants.TAG_SCRIPTS);
		}

		public override bool UnhideScreen()
		{
			if (Task.IsComplete)
			{
				GetWorkshopItems();
			}
			return base.UnhideScreen();
		}

		private void OnButton_NewBlueprint(MyGuiControlButton button)
		{
			switch (m_content)
			{
			case Content.Blueprint:
				CreateBlueprintFromClipboard(withScreenshot: true);
				break;
			case Content.Script:
				CreateScriptFromEditor();
				break;
			}
		}

		private void OnButton_DirectorySelection(MyGuiControlButton button)
		{
			string rootPath = string.Empty;
			Func<string, bool> isItem = null;
			switch (m_content)
			{
			case Content.Blueprint:
				rootPath = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
				isItem = MyBlueprintUtils.IsItem_Blueprint;
				break;
			case Content.Script:
				rootPath = MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
				isItem = MyBlueprintUtils.IsItem_Script;
				break;
			}
			MyGuiSandbox.AddScreen(new MyGuiFolderScreen(hideOthers: false, OnPathSelected, rootPath, GetCurrentLocalDirectory(), isItem, visibleFolderSelect: true));
		}

		private void OnButton_HideThumbnails(MyGuiControlButton button)
		{
			SetThumbnailVisibility(!GetThumbnailVisibility());
			SetIconForHideThubnails();
			TogglePreviewVisibility();
		}

		private void OnButton_OpenInWorkshop(MyGuiControlButton button)
		{
			if (SelectedBlueprint?.Item != null)
			{
				MyGuiSandbox.OpenUrlWithFallback(SelectedBlueprint.Item.GetItemUrl(), SelectedBlueprint.Item.ServiceName + " Workshop");
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Invalid workshop id"), messageText: new StringBuilder("")));
			}
		}

		private void OnButton_CopyToClipboard(MyGuiControlButton button)
		{
			CopyToClipboard();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (!m_wasJoystickLastUsed && MyInput.Static.IsJoystickLastUsed)
			{
				SelectedBlueprintChanged();
<<<<<<< HEAD
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				OnButton_Refresh(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				OnButton_NewBlueprint(null);
			}
			if (SelectedBlueprint?.Item != null && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
			{
				OnButton_OpenInWorkshop(null);
			}
			if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU).IsNewReleased())
			{
				OnButton_OpenWorkshop(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				OnButton_Sorting(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
			{
				OnButton_GroupSelection(null);
			}
			if (base.FocusedControl != null && base.FocusedControl.Owner == m_BPList && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
			{
				CopyToClipboard();
			}
=======
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				OnButton_Refresh(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				OnButton_NewBlueprint(null);
			}
			if (SelectedBlueprint?.Item != null && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
			{
				OnButton_OpenInWorkshop(null);
			}
			if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU).IsNewReleased())
			{
				OnButton_OpenWorkshop(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				OnButton_Sorting(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
			{
				OnButton_GroupSelection(null);
			}
			if (base.FocusedControl != null && base.FocusedControl.Owner == m_BPList && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
			{
				CopyToClipboard();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_button_CopyToClipboard.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_button_OpenInWorkshop.Visible = !MyInput.Static.IsJoystickLastUsed;
		}

		private void CopyToClipboard()
		{
			if (SelectedBlueprint == null)
			{
				return;
			}
			switch (m_content)
			{
			case Content.Blueprint:
				if (m_blueprintBeingLoaded)
				{
					break;
				}
				if (SelectedBlueprint.IsDirectory)
				{
					if (string.IsNullOrEmpty(SelectedBlueprint.BlueprintName))
					{
						string[] array = GetCurrentLocalDirectory().Split(new char[1] { Path.DirectorySeparatorChar });
						if (array.Length > 1)
						{
							array[array.Length - 1] = string.Empty;
							SetCurrentLocalDirectory(Path.Combine(array));
						}
						else
						{
							SetCurrentLocalDirectory(string.Empty);
						}
					}
					else
					{
						SetCurrentLocalDirectory(Path.Combine(GetCurrentLocalDirectory(), SelectedBlueprint.BlueprintName));
					}
					CheckCurrentLocalDirectory_Blueprint();
					RefreshAndReloadItemList();
					break;
				}
				m_blueprintBeingLoaded = true;
				switch (SelectedBlueprint.Type)
				{
				case MyBlueprintTypeEnum.WORKSHOP:
					m_blueprintBeingLoaded = true;
					Task = Parallel.Start(delegate
					{
						if (!MyWorkshop.IsUpToDate(SelectedBlueprint.Item))
						{
							DownloadBlueprintFromSteam(SelectedBlueprint.Item);
						}
					}, delegate
					{
						CopyBlueprintAndClose();
					});
					break;
				case MyBlueprintTypeEnum.LOCAL:
				case MyBlueprintTypeEnum.DEFAULT:
				case MyBlueprintTypeEnum.CLOUD:
					m_blueprintBeingLoaded = true;
					CopyBlueprintAndClose();
					break;
				case MyBlueprintTypeEnum.SHARED:
					OpenSharedBlueprint(SelectedBlueprint);
					break;
				}
				break;
			case Content.Script:
				OpenSelectedSript();
				break;
			}
		}

		private void OnButton_Rename(MyGuiControlButton button)
		{
			if (SelectedBlueprint == null)
<<<<<<< HEAD
			{
				return;
			}
			MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(m_position, delegate(string result)
			{
=======
			{
				return;
			}
			MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(m_position, delegate(string result)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (result != null)
				{
					result = MyUtils.StripInvalidChars(result);
					if (!string.Equals(result, SelectedBlueprint.Data.Name, StringComparison.InvariantCulture) && !string.IsNullOrEmpty(result))
					{
						if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
						{
							ChangeBlueprintNameCloud(result);
						}
						else
						{
							ChangeName(result);
						}
					}
				}
			}, caption: MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), defaultName: SelectedBlueprint.Data.Name, maxLenght: 40, textBoxWidth: 0.3f));
		}

		private void OnButton_Replace(MyGuiControlButton button)
		{
			if (SelectedBlueprint == null)
			{
				return;
			}
			if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD && !MySandboxGame.Config.EnableSteamCloud)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOff), MyGameService.Service.ServiceName)), new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOff_Caption), MyGameService.Service.ServiceName))));
				return;
			}
			if (SelectedBlueprint.Type == MyBlueprintTypeEnum.LOCAL && MySandboxGame.Config.EnableSteamCloud)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOn), MyGameService.Service.ServiceName)), new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOn_Caption), MyGameService.Service.ServiceName))));
				return;
			}
			switch (m_content)
			{
			case Content.Blueprint:
				if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace), messageText: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
					{
						CreateBlueprintFromClipboard(withScreenshot: false, replace: true, SelectedBlueprint);
					}));
					break;
				}
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace), messageText: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES && m_clipboard != null && m_clipboard.CopiedGrids != null && m_clipboard.CopiedGrids.Count != 0)
					{
						string name = SelectedBlueprint.Data.Name;
						string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), name, "bp.sbc");
						if (File.Exists(text))
						{
							MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyBlueprintUtils.LoadPrefab(text);
							m_clipboard.CopiedGrids[0].DisplayName = name;
							myObjectBuilder_Definitions.ShipBlueprints[0].CubeGrids = m_clipboard.CopiedGrids.ToArray();
							myObjectBuilder_Definitions.ShipBlueprints[0].DLCs = GetNecessaryDLCs(myObjectBuilder_Definitions.ShipBlueprints[0].CubeGrids);
							MyBlueprintUtils.SavePrefabToFile(myObjectBuilder_Definitions, m_clipboard.CopiedGridsName, GetCurrentLocalDirectory(), replace: true);
							RefreshBlueprintList();
						}
					}
				}));
				break;
			case Content.Script:
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
<<<<<<< HEAD
						string contents = m_getCodeFromEditor();
						if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
						{
							string text2 = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, GetCurrentLocalDirectory(), SelectedBlueprint.Data.Name);
							string cloudPath = MyCloudHelper.Combine("Scripts/cloud", SelectedBlueprint.BlueprintName);
							MyCloudHelper.ExtractFilesTo(cloudPath, text2, unpack: false);
							File.WriteAllText(Path.Combine(text2, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), contents, Encoding.UTF8);
							MyCloudHelper.UploadFiles(cloudPath, text2, compress: false);
						}
						else
						{
							string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, SelectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION);
							if (File.Exists(path))
							{
								File.WriteAllText(path, contents, Encoding.UTF8);
=======
						string text2 = m_getCodeFromEditor();
						if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
						{
							string text3 = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, GetCurrentLocalDirectory(), SelectedBlueprint.Data.Name);
							string cloudPath = MyCloudHelper.Combine("Scripts/cloud", SelectedBlueprint.BlueprintName);
							MyCloudHelper.ExtractFilesTo(cloudPath, text3, unpack: false);
							File.WriteAllText(Path.Combine(text3, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), text2, Encoding.UTF8);
							MyCloudHelper.UploadFiles(cloudPath, text3, compress: false);
						}
						else
						{
							string text4 = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, SelectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION);
							if (File.Exists(text4))
							{
								File.WriteAllText(text4, text2, Encoding.UTF8);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
					}
				}));
				break;
			}
		}

		private void OnButton_Delete(MyGuiControlButton button)
		{
			if (SelectedBlueprint == null)
			{
				return;
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Delete), messageText: MyTexts.Get(MySpaceTexts.DeleteBlueprintQuestion), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES && SelectedBlueprint != null)
				{
					switch (m_content)
					{
					case Content.Blueprint:
						switch (SelectedBlueprint.Type)
						{
						case MyBlueprintTypeEnum.LOCAL:
						case MyBlueprintTypeEnum.DEFAULT:
						{
							string path2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), SelectedBlueprint.BlueprintName);
							if (DeleteItem(path2))
							{
								SelectedBlueprint = null;
								ResetBlueprintUI();
							}
							break;
						}
						case MyBlueprintTypeEnum.CLOUD:
							DeleteBlueprintCloud();
							break;
						}
						break;
					case Content.Script:
						switch (SelectedBlueprint.Type)
<<<<<<< HEAD
						{
						case MyBlueprintTypeEnum.LOCAL:
						case MyBlueprintTypeEnum.DEFAULT:
						{
=======
						{
						case MyBlueprintTypeEnum.LOCAL:
						case MyBlueprintTypeEnum.DEFAULT:
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), SelectedBlueprint.Data.Name);
							if (DeleteItem(path))
							{
								SelectedBlueprint = null;
								ResetBlueprintUI();
							}
							break;
						}
						case MyBlueprintTypeEnum.CLOUD:
							DeleteScriptCloud();
							break;
						}
						break;
					}
					RefreshBlueprintList();
				}
			}));
		}

		private void OnButton_TakeScreenshot(MyGuiControlButton button)
		{
			if (SelectedBlueprint != null)
			{
				switch (SelectedBlueprint.Type)
				{
				case MyBlueprintTypeEnum.LOCAL:
					TakeScreenshotLocalBP(SelectedBlueprint.Data.Name, m_selectedButton);
					break;
				case MyBlueprintTypeEnum.CLOUD:
				{
					string text = Path.Combine("Blueprints/cloud", SelectedBlueprint.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
					string pathFull = Path.Combine(MyFileSystem.UserDataPath, text);
					TakeScreenshotCloud(text, pathFull, m_selectedButton);
					break;
				}
				}
			}
		}

		private void OnButton_Publish(MyGuiControlButton button)
		{
			string localDirectory = GetCurrentLocalDirectory();
			switch (m_content)
			{
			case Content.Blueprint:
			{
				if (SelectedBlueprint == null)
<<<<<<< HEAD
				{
					break;
				}
				string path;
				if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
				{
					path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_TEMP, localDirectory, SelectedBlueprint.Data.Name);
					MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine("Blueprints/cloud", SelectedBlueprint.BlueprintName) + "/", path, unpack: true);
				}
				else
				{
					path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, SelectedBlueprint.Data.Name);
				}
				string path2 = Path.Combine(path, "bp.sbc");
				if (!File.Exists(path2))
				{
					break;
				}
				m_LoadPrefabData = new LoadPrefabData(null, path2, null);
				Action<WorkData> completionCallback = delegate(WorkData workData)
				{
=======
				{
					break;
				}
				string path;
				if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
				{
					path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_TEMP, localDirectory, SelectedBlueprint.Data.Name);
					MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine("Blueprints/cloud", SelectedBlueprint.BlueprintName) + "/", path, unpack: true);
				}
				else
				{
					path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, SelectedBlueprint.Data.Name);
				}
				string text2 = Path.Combine(path, "bp.sbc");
				if (!File.Exists(text2))
				{
					break;
				}
				m_LoadPrefabData = new LoadPrefabData(null, text2, null);
				Action<WorkData> completionCallback = delegate(WorkData workData)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					LoadPrefabData loadPrefabData = workData as LoadPrefabData;
					if (loadPrefabData.Prefab != null)
					{
						MyBlueprintUtils.PublishBlueprint(loadPrefabData.Prefab, SelectedBlueprint.Data.Name, localDirectory, path, SelectedBlueprint.Type);
					}
					LoadPrefabData.CallOnPrefabLoaded(loadPrefabData);
				};
				Task = Parallel.Start(LoadPrefabData.CallLoadPrefab, completionCallback, m_LoadPrefabData);
				break;
			}
			case Content.Script:
				if (SelectedBlueprint != null)
				{
					string text;
					if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
					{
						text = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, SelectedBlueprint.Data.Name);
						MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine("Scripts/cloud", SelectedBlueprint.BlueprintName) + "/", text, unpack: false);
					}
					else
					{
						text = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, localDirectory, SelectedBlueprint.Data.Name);
					}
					MyBlueprintUtils.PublishScript(text, SelectedBlueprint, delegate
					{
						m_wasPublished = true;
					});
				}
				break;
			}
		}

		private void OnSendToPlayer()
		{
			if (m_sendToCombo.GetSelectedIndex() == 0)
			{
				return;
			}
			if (SelectedBlueprint?.Item == null)
			{
				m_sendToCombo.SelectItemByIndex(0);
				return;
			}
			ulong selectedKey = (ulong)m_sendToCombo.GetSelectedKey();
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShareBlueprintRequest, SelectedBlueprint.Item.Id, SelectedBlueprint.Item.ServiceName, SelectedBlueprint.Data.Name, selectedKey, MySession.Static.LocalHumanPlayer.DisplayName);
		}

		private void OnSelectItem(MyGuiControlRadioButtonGroup args)
		{
			switch (m_content)
			{
			case Content.Blueprint:
			{
				SelectedBlueprint = null;
				m_selectedButton = null;
				m_loadedPrefab = null;
				m_LoadPrefabData = null;
				UpdateDetailKeyEnable();
				MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
				if (myGuiControlContentButton != null)
				{
					MyBlueprintItemInfo myBlueprintItemInfo2 = myGuiControlContentButton.UserData as MyBlueprintItemInfo;
					if (myBlueprintItemInfo2 == null)
					{
						break;
					}
					m_selectedButton = myGuiControlContentButton;
					SelectedBlueprint = myBlueprintItemInfo2;
				}
				UpdatePrefab(SelectedBlueprint, loadPrefab: false);
				break;
			}
			case Content.Script:
			{
				SelectedBlueprint = null;
				m_selectedButton = null;
				MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
				if (myGuiControlContentButton != null)
				{
					MyBlueprintItemInfo myBlueprintItemInfo = myGuiControlContentButton.UserData as MyBlueprintItemInfo;
					if (myBlueprintItemInfo == null)
					{
						break;
					}
					m_selectedButton = myGuiControlContentButton;
					SelectedBlueprint = myBlueprintItemInfo;
				}
				UpdateNameAndDescription();
				UpdateInfo(null, SelectedBlueprint);
				UpdateDetailKeyEnable();
				break;
			}
			}
		}

		private void OnSearchTextChange(string message)
		{
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		private void OnRateUpClicked(MyGuiControlButton button)
		{
			UpdateRateState(positive: true);
		}

		private void OnRateDownClicked(MyGuiControlButton button)
		{
			UpdateRateState(positive: false);
		}

		private void UpdateRateState(bool positive)
		{
			if (SelectedBlueprint != null && SelectedBlueprint.Item != null)
			{
				SelectedBlueprint.Item.Rate(positive);
				SelectedBlueprint.Item.ChangeRatingValue(positive);
				m_button_RateUp.Checked = positive;
				m_button_RateDown.Checked = !positive;
			}
		}

		private void OnReload(MyGuiControlButton button)
		{
			m_selectedButton = null;
			SelectedBlueprint = null;
			UpdateDetailKeyEnable();
			m_downloadFinished.Clear();
			m_downloadQueued.Clear();
			RefreshAndReloadItemList();
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		private void ResetBlueprintUI()
		{
			SelectedBlueprint = null;
			UpdateDetailKeyEnable();
		}

		public void RefreshBlueprintList(bool fromTask = false)
		{
			bool flag = false;
			MyBlueprintItemInfo itemInfo = null;
			if (SelectedBlueprint != null)
			{
				flag = true;
				itemInfo = SelectedBlueprint;
			}
			m_BPList.Clear();
			m_BPTypesGroup.Clear();
			switch (m_content)
			{
			case Content.Blueprint:
				GetLocalNames_Blueprints(fromTask);
				break;
			case Content.Script:
				GetLocalNames_Scripts(fromTask);
				break;
			}
			ApplyFiltering();
			m_selectedButton = null;
			SelectedBlueprint = null;
			TrySelectFirstBlueprint();
			if (flag)
			{
				SelectBlueprint(itemInfo);
			}
			UpdateDetailKeyEnable();
		}

		public void RefreshAndReloadItemList()
		{
			m_BPList.Clear();
			m_BPTypesGroup.Clear();
			switch (m_content)
			{
			case Content.Blueprint:
				GetLocalNames_Blueprints(reload: true);
				break;
			case Content.Script:
				GetLocalNames_Scripts(reload: true);
				break;
			}
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		private void SetGroupSelection(MyBlueprintTypeEnum option)
		{
			SetSelectedBlueprintType(option);
			SetIconForGroupSelection();
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			switch (m_content)
			{
			case Content.Blueprint:
				if (m_blueprintBeingLoaded)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_StillLoading), messageText: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_StillLoading), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							m_blueprintBeingLoaded = false;
							Task.valid = false;
							CloseScreen(isUnloading);
						}
					}));
					return false;
				}
				return base.CloseScreen(isUnloading);
			case Content.Script:
				if (m_onCloseAction != null)
				{
					m_onCloseAction();
				}
				return base.CloseScreen(isUnloading);
			default:
				return base.CloseScreen(isUnloading);
			}
		}

		private void OpenSelectedSript()
		{
			if (SelectedBlueprint.Type == MyBlueprintTypeEnum.WORKSHOP)
			{
				OpenSharedScript(SelectedBlueprint);
			}
			else if (m_onScriptOpened != null)
			{
				string text;
				if (SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
				{
					text = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, GetCurrentLocalDirectory(), SelectedBlueprint.Data.Name);
					MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine(MyCloudHelper.Combine("Scripts/cloud", SelectedBlueprint.BlueprintName), MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), text, unpack: false);
				}
				else
				{
					text = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), SelectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION);
				}
				m_onScriptOpened(text);
			}
			CloseScreen();
		}

		private void OpenSharedScript(MyBlueprintItemInfo itemInfo)
		{
			m_BPList.Enabled = false;
			Task = Parallel.Start(DownloadScriptFromSteam, OnScriptDownloaded);
		}

		private void DownloadScriptFromSteam()
		{
			if (SelectedBlueprint != null)
			{
				MyWorkshop.DownloadScriptBlocking(SelectedBlueprint.Item);
			}
		}

		private void OnScriptDownloaded()
		{
			if (m_onScriptOpened != null && SelectedBlueprint != null)
			{
				m_onScriptOpened(SelectedBlueprint.Item.Folder);
			}
			m_BPList.Enabled = true;
		}

		private bool DeleteItem(string path)
		{
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
				return true;
			}
			return false;
		}

		private bool ValidateSelecteditem()
		{
			if (SelectedBlueprint == null)
			{
				return false;
			}
			if (SelectedBlueprint.Data.Name == null)
			{
				return false;
			}
			return true;
		}

		internal void OnPrefabLoaded(MyObjectBuilder_Definitions prefab)
		{
			m_blueprintBeingLoaded = false;
			if (prefab != null)
			{
				if (MySandboxGame.Static.SessionCompatHelper != null)
				{
					MySandboxGame.Static.SessionCompatHelper.CheckAndFixPrefab(prefab);
				}
				if (!CheckBlueprintForModsAndModifiedBlocks(prefab))
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToPasteGridWithMissingBlocks), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							CloseScreen();
							if (CopyBlueprintPrefabToClipboard(prefab, m_clipboard) && m_accessType == MyBlueprintAccessType.NORMAL)
							{
								if (MySession.Static.IsCopyPastingEnabled)
								{
									MySandboxGame.Static.Invoke(delegate
									{
										MyClipboardComponent.Static.Paste();
									}, "BlueprintSelectionAutospawn2");
								}
								else
								{
									MyClipboardComponent.ShowCannotPasteWarning();
								}
							}
						}
						_ = 1;
					}));
					return;
				}
				CloseScreen();
				if (!CopyBlueprintPrefabToClipboard(prefab, m_clipboard) || m_accessType != 0)
				{
					return;
				}
				if (MySession.Static.IsCopyPastingEnabled)
				{
					MySandboxGame.Static.Invoke(delegate
					{
						MyClipboardComponent.Static.Paste();
					}, "BlueprintSelectionAutospawn1");
				}
				else
				{
					MyClipboardComponent.ShowCannotPasteWarning();
				}
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Error), messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint)));
			}
		}

		private static bool CheckBlueprintForModsAndModifiedBlocks(MyObjectBuilder_Definitions prefab)
		{
			MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = prefab.ShipBlueprints;
			if (shipBlueprints != null)
			{
				return MyGridClipboard.CheckPastedBlocks(shipBlueprints[0].CubeGrids);
			}
			return true;
		}

		private bool IsExtracted(MyWorkshopItem subItem)
		{
			return m_content switch
			{
<<<<<<< HEAD
			case Content.Blueprint:
				return Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString()));
			case Content.Script:
				return true;
			default:
				return false;
			}
=======
				Content.Blueprint => Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString())), 
				Content.Script => true, 
				_ => false, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private string GetImagePath(MyBlueprintItemInfo data)
		{
			string text = string.Empty;
			if (data.Type == MyBlueprintTypeEnum.LOCAL)
			{
				switch (m_content)
				{
				case Content.Blueprint:
					text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), data.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
					break;
				case Content.Script:
					text = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), data.Data.Name, MyBlueprintUtils.THUMB_IMAGE_NAME);
					break;
				}
			}
			else if (data.Type == MyBlueprintTypeEnum.CLOUD)
			{
				text = data.Data.CloudImagePath;
				if (string.IsNullOrEmpty(text))
				{
					if (data.CloudPathXML != null)
					{
<<<<<<< HEAD
						text = Path.Combine(MyFileSystem.UserDataPath, data.CloudPathXML.Replace(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME, "thumb.png"));
					}
					else if (data.CloudPathCS != null)
					{
						text = Path.Combine(MyFileSystem.UserDataPath, data.CloudPathCS.Replace(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION, "thumb.png"));
=======
						text = data.CloudPathXML.Replace(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME, "thumb.png");
					}
					else if (data.CloudPathCS != null)
					{
						text = data.CloudPathCS.Replace(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION, "thumb.png");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			else if (data.Type == MyBlueprintTypeEnum.WORKSHOP)
			{
				if (m_content == Content.Script)
				{
<<<<<<< HEAD
					if (data.Item != null)
					{
						if (data.Item.Folder != null && MyFileSystem.IsDirectory(data.Item.Folder))
						{
							text = Path.Combine(data.Item.Folder, MyBlueprintUtils.THUMB_IMAGE_NAME);
							if (MyFileSystem.FileExists(text))
							{
								return text;
							}
							text = Path.Combine(data.Item.Folder, "thumb.jpg");
							if (MyFileSystem.FileExists(text))
							{
								return text;
							}
						}
						string text2 = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP, data.Item.ServiceName, data.Item.Id.ToString());
						if (Directory.Exists(text2))
						{
							text = Path.Combine(text2, MyBlueprintUtils.THUMB_IMAGE_NAME);
							if (MyFileSystem.FileExists(text))
							{
								return text;
							}
							text = Path.Combine(text2, "thumb.jpg");
							if (MyFileSystem.FileExists(text))
							{
								return text;
							}
						}
						if (data.Item.Folder != null && Path.GetExtension(data.Item.Folder) == ".bin" && File.Exists(data.Item.Folder))
						{
							MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(data.Item.Folder);
							if (myZipArchive != null)
							{
								if (Directory.Exists(text2))
								{
									Directory.Delete(text2, recursive: true);
								}
								Directory.CreateDirectory(text2);
								bool flag = false;
								string text3 = string.Empty;
								if (myZipArchive.FileExists(MyBlueprintUtils.THUMB_IMAGE_NAME))
								{
									text3 = MyBlueprintUtils.THUMB_IMAGE_NAME;
									flag = true;
								}
								else if (myZipArchive.FileExists("thumb.jpg"))
								{
									text3 = "thumb.jpg";
									flag = true;
								}
								if (flag)
								{
									using (Stream stream = myZipArchive.GetFile(text3).GetStream())
									{
										if (stream != null)
										{
											string text4 = Path.Combine(text2, text3);
											using (FileStream destination = File.Create(text4))
											{
												stream.CopyTo(destination);
											}
											MyRenderProxy.UnloadTexture(text4);
											return text4;
										}
									}
								}
							}
						}
					}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return Path.Combine(MyFileSystem.ContentPath, MyBlueprintUtils.STEAM_THUMBNAIL_NAME);
				}
				if (data.Item != null)
				{
					bool flag2 = false;
					if (data.Item.Folder != null && MyFileSystem.IsDirectory(data.Item.Folder))
					{
						text = Path.Combine(data.Item.Folder, MyBlueprintUtils.THUMB_IMAGE_NAME);
<<<<<<< HEAD
						flag2 = true;
=======
						flag = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						text = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, data.Item.ServiceName, data.Item.Id.ToString(), MyBlueprintUtils.THUMB_IMAGE_NAME);
					}
					bool num = m_downloadQueued.Contains(new WorkshopId(data.Item.Id, data.Item.ServiceName));
<<<<<<< HEAD
					bool flag3 = m_downloadFinished.Contains(new WorkshopId(data.Item.Id, data.Item.ServiceName));
=======
					bool flag2 = m_downloadFinished.Contains(new WorkshopId(data.Item.Id, data.Item.ServiceName));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyWorkshopItem worshopData = data.Item;
					if (flag3 && !IsExtracted(worshopData) && !flag2)
					{
						m_BPList.Enabled = false;
						ExtractWorkshopItem(worshopData);
						m_BPList.Enabled = true;
					}
					if (!num && !flag3)
					{
						m_BPList.Enabled = false;
						m_downloadQueued.Add(new WorkshopId(data.Item.Id, data.Item.ServiceName));
						Task = Parallel.Start(delegate
						{
							DownloadBlueprintFromSteam(worshopData);
						}, delegate
						{
							OnBlueprintDownloadedThumbnail(worshopData);
						});
						text = string.Empty;
					}
				}
			}
			else if (data.Type == MyBlueprintTypeEnum.DEFAULT)
			{
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, data.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
			}
			return text;
		}

		private void ExtractWorkshopItem(MyWorkshopItem subItem)
		{
			if (!MyFileSystem.IsDirectory(subItem.Folder))
			{
				try
				{
					string folder = subItem.Folder;
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString());
					if (Directory.Exists(text))
					{
						Directory.Delete(text, true);
					}
					Directory.CreateDirectory(text);
					MyObjectBuilder_ModInfo myObjectBuilder_ModInfo = new MyObjectBuilder_ModInfo();
					myObjectBuilder_ModInfo.SubtypeName = subItem.Title;
					myObjectBuilder_ModInfo.WorkshopIds = new WorkshopId[1]
					{
						new WorkshopId(subItem.Id, subItem.ServiceName)
					};
					myObjectBuilder_ModInfo.SteamIDOwner = subItem.OwnerId;
<<<<<<< HEAD
					string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString(), "info.temp");
					if (File.Exists(path))
=======
					string text2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString(), "info.temp");
					if (File.Exists(text2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						File.Delete(text2);
					}
					MyObjectBuilderSerializer.SerializeXML(text2, compress: false, myObjectBuilder_ModInfo);
					if (!string.IsNullOrEmpty(folder))
					{
						using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(folder))
						{
							if (myZipArchive != null && myZipArchive.FileExists(MyBlueprintUtils.THUMB_IMAGE_NAME))
							{
<<<<<<< HEAD
								using (Stream stream = myZipArchive.GetFile(MyBlueprintUtils.THUMB_IMAGE_NAME).GetStream())
=======
								string text3 = Path.Combine(text, MyBlueprintUtils.THUMB_IMAGE_NAME);
								using (FileStream destination = File.Create(text3))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									if (stream != null)
									{
										string text2 = Path.Combine(text, MyBlueprintUtils.THUMB_IMAGE_NAME);
										using (FileStream destination = File.Create(text2))
										{
											stream.CopyTo(destination);
										}
										MyRenderProxy.UnloadTexture(text2);
									}
								}
								MyRenderProxy.UnloadTexture(text3);
							}
						}
					}
					else
					{
						MyLog.Default.Critical(new StringBuilder("Path in Folder directory of blueprint \"" + subItem.Title + "\" " + subItem.Id + " is null, it shouldn't be and who knows what problems it causes. "));
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
			MyBlueprintItemInfo info = new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
			{
				BlueprintName = subItem.Title,
				Size = subItem.Size,
				Item = subItem
			};
			MyGuiControlListbox.Item listItem = new MyGuiControlListbox.Item(null, null, null, info);
			MySandboxGame.Static.Invoke(delegate
			{
				if (m_BPList.Controls.FindIndex((MyGuiControlBase item) => (item.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.WORKSHOP && (item.UserData as MyBlueprintItemInfo).Item?.Id == (listItem.UserData as MyBlueprintItemInfo).Item?.Id && (item.UserData as MyBlueprintItemInfo).Item?.ServiceName == (listItem.UserData as MyBlueprintItemInfo).Item?.ServiceName) == -1)
				{
					AddBlueprintButton(info, filter: true);
				}
			}, "AddBlueprintButton");
		}

		private void DownloadBlueprintFromSteam(MyWorkshopItem item)
		{
			if (!MyWorkshop.IsUpToDate(item))
			{
				MyWorkshop.DownloadBlueprintBlockingUGC(item, check: false);
				ExtractWorkshopItem(item);
			}
		}

		private void OnBlueprintDownloadedThumbnail(MyWorkshopItem item)
		{
			m_downloadQueued.Remove(new WorkshopId(item.Id, item.ServiceName));
			m_downloadFinished.Add(new WorkshopId(item.Id, item.ServiceName));
		}

		private void GetBlueprints(string directory, MyBlueprintTypeEnum type)
		{
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			if (!Directory.Exists(directory))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(directory);
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] array = directories;
			foreach (string text in array)
			{
				list.Add(text + "\\bp.sbc");
				string[] array2 = text.Split(new char[1] { '\\' });
				list2.Add(array2[array2.Length - 1]);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				string text2 = list2[j];
				string text3 = list[j];
				if (File.Exists(text3))
				{
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(type)
					{
<<<<<<< HEAD
						TimeCreated = File.GetCreationTimeUtc(path),
						TimeUpdated = File.GetLastWriteTimeUtc(path),
=======
						TimeCreated = File.GetCreationTimeUtc(text3),
						TimeUpdated = File.GetLastWriteTimeUtc(text3),
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						BlueprintName = text2,
						Size = DirectoryExtensions.GetStorageSize(directories[j])
					};
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(text2, text2);
					data.Add(myBlueprintItemInfo);
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.LOCAL);
			AddBlueprintButtons(ref data);
		}

		private void GetLocalNames_Blueprints(bool reload = false)
		{
			string directory = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory());
			GetBlueprints(directory, MyBlueprintTypeEnum.LOCAL);
			if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
			{
				GetBlueprintsFromCloud("Blueprints/cloud", scripts: false);
			}
			if (Task.IsComplete)
			{
				if (reload)
				{
					GetWorkshopItems();
				}
				else
				{
					GetWorkshopItemsSteam();
				}
			}
			SortBlueprints(m_recievedBlueprints, MyBlueprintTypeEnum.LOCAL);
			AddBlueprintButtons(ref m_recievedBlueprints);
			if (MyFakes.ENABLE_DEFAULT_BLUEPRINTS)
			{
				GetBlueprints(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, MyBlueprintTypeEnum.DEFAULT);
			}
		}

		private void GetLocalNames_Scripts(bool reload = false)
		{
			if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
			{
				GetBlueprintsFromCloud("Scripts/cloud", scripts: true);
			}
<<<<<<< HEAD
			string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory());
			if (!Directory.Exists(path))
=======
			string text = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory());
			if (!Directory.Exists(text))
			{
				MyFileSystem.CreateDirectoryRecursive(text);
			}
			if (!Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_TEMP))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyFileSystem.CreateDirectoryRecursive(MyBlueprintUtils.SCRIPT_FOLDER_TEMP);
			}
<<<<<<< HEAD
			if (!Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_TEMP))
			{
				MyFileSystem.CreateDirectoryRecursive(MyBlueprintUtils.SCRIPT_FOLDER_TEMP);
			}
			string[] directories = Directory.GetDirectories(path);
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			string[] array = directories;
			foreach (string text in array)
			{
				if (MyBlueprintUtils.IsItem_Script(text))
				{
					string fileName = Path.GetFileName(text);
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.LOCAL)
					{
						BlueprintName = fileName,
						Size = DirectoryExtensions.GetStorageSize(text)
=======
			string[] directories = Directory.GetDirectories(text);
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			string[] array = directories;
			foreach (string text2 in array)
			{
				if (MyBlueprintUtils.IsItem_Script(text2))
				{
					string fileName = Path.GetFileName(text2);
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.LOCAL)
					{
						BlueprintName = fileName,
						Size = DirectoryExtensions.GetStorageSize(text2)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					};
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(fileName);
					data.Add(myBlueprintItemInfo);
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.LOCAL);
			AddBlueprintButtons(ref data);
			if (Task.IsComplete)
			{
				if (reload)
				{
					GetWorkshopItems();
				}
				else
				{
					GetWorkshopItemsSteam();
				}
			}
		}

		private void GetWorkshopItems()
		{
			if (MyGameService.IsActive)
			{
				switch (m_content)
				{
				case Content.Blueprint:
					Task = Parallel.Start(DownloadBlueprints);
					break;
				case Content.Script:
					Task = Parallel.Start(GetScriptsInfo);
					break;
				}
			}
		}

		private void GetScriptsInfo()
		{
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			m_downloadFromSteam = true;
			(MyGameServiceCallResult, string) subscribedIngameScriptsBlocking = MyWorkshop.GetSubscribedIngameScriptsBlocking(list);
			if (Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP))
			{
				try
				{
<<<<<<< HEAD
					Directory.Delete(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP, recursive: true);
=======
					Directory.Delete(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				catch (IOException)
				{
				}
			}
			Directory.CreateDirectory(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP);
			using (SubscribedItemsLock.AcquireExclusiveUsing())
			{
				SetSubscribeItemList(ref list);
			}
			m_needsExtract = true;
			m_downloadFromSteam = false;
			UpdateWorkshopError(subscribedIngameScriptsBlocking.Item1, subscribedIngameScriptsBlocking.Item2);
		}

		private void DownloadBlueprints()
		{
			m_downloadFromSteam = true;
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			(MyGameServiceCallResult, string) subscribedBlueprintsBlocking = MyWorkshop.GetSubscribedBlueprintsBlocking(list);
			Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
			foreach (MyWorkshopItem item in list)
			{
				DownloadBlueprintFromSteam(item);
				m_downloadFinished.Add(new WorkshopId(item.Id, item.ServiceName));
			}
			using (SubscribedItemsLock.AcquireExclusiveUsing())
			{
				SetSubscribeItemList(ref list);
			}
			m_needsExtract = true;
			m_downloadFromSteam = false;
			UpdateWorkshopError(subscribedBlueprintsBlocking.Item1, subscribedBlueprintsBlocking.Item2);
		}

		private void UpdateWorkshopError(MyGameServiceCallResult result, string serviceName)
		{
			if (result != MyGameServiceCallResult.OK)
			{
				string workshopErrorText = MyWorkshop.GetWorkshopErrorText(result, serviceName, m_workshopPermitted);
				SetWorkshopErrorText(workshopErrorText);
			}
			else
			{
				SetWorkshopErrorText();
			}
		}

		private void GetBlueprintsFromCloud(string cloudDirectory, bool scripts)
		{
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(cloudDirectory);
			if (cloudFiles == null)
			{
				return;
			}
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			Dictionary<string, MyBlueprintItemInfo> dictionary = new Dictionary<string, MyBlueprintItemInfo>();
			foreach (MyCloudFileInfo item in cloudFiles)
			{
				string[] array = item.Name.Split(new char[1] { '/' });
				string name = array[array.Length - 2];
				string text = array[array.Length - 1];
				if (text.Equals(MyBlueprintUtils.THUMB_IMAGE_NAME))
				{
					string path = Path.Combine(cloudDirectory, name);
					string text2 = Path.Combine(MyFileSystem.UserDataPath, path);
					string imagePath = Path.Combine(text2, MyBlueprintUtils.THUMB_IMAGE_NAME);
					int fileSize = item.Size;
					try
					{
						Directory.CreateDirectory(text2);
						MyGameService.LoadFromCloudAsync(item.Name, delegate(byte[] x)
						{
							OnCloudImageDownloaded(x, name, imagePath, fileSize);
						});
					}
					catch (Exception)
					{
					}
				}
				else if ((text.Equals(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME) && !scripts) || (text.Equals(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION) && scripts))
				{
					if (!dictionary.TryGetValue(name, out var value))
					{
						value = new MyBlueprintItemInfo(MyBlueprintTypeEnum.CLOUD)
						{
							TimeCreated = DateTime.FromFileTimeUtc(item.Timestamp),
							TimeUpdated = DateTime.FromFileTimeUtc(item.Timestamp),
							BlueprintName = name,
							CloudInfo = item,
							Size = (ulong)item.Size
						};
						value.SetAdditionalBlueprintInformation(name, item.Name);
						dictionary.Add(name, value);
						data.Add(value);
<<<<<<< HEAD
=======
					}
					value.Size += (ulong)item.Size;
					if (item.Name.EndsWith("B5"))
					{
						value.CloudPathPB = item.Name;
					}
					else if (item.Name.EndsWith(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
					{
						value.CloudPathXML = item.Name;
					}
					else if (item.Name.EndsWith(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION))
					{
						value.CloudPathCS = item.Name;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					value.Size += (ulong)item.Size;
					if (item.Name.EndsWith("B5"))
					{
						value.CloudPathPB = item.Name;
					}
					else if (item.Name.EndsWith(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
					{
						value.CloudPathXML = item.Name;
					}
					else if (item.Name.EndsWith(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION))
					{
						value.CloudPathCS = item.Name;
					}
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.CLOUD);
			AddBlueprintButtons(ref data);
		}

		private void OnCloudImageDownloaded(byte[] data, string name, string imagePath, int fileSize, int writeAttempts = 1)
		{
			try
			{
				File.WriteAllBytes(imagePath, data);
				MyRenderProxy.UnloadTexture(imagePath);
				MyRenderProxy.PreloadTextures(new string[1] { imagePath }, TextureType.GUIWithoutPremultiplyAlpha);
				int num = m_BPList.Controls.FindIndex((MyGuiControlBase item) => ((MyBlueprintItemInfo)item.UserData).BlueprintName == name);
				if (num >= 0)
				{
					MyBlueprintItemInfo obj = (MyBlueprintItemInfo)m_BPList.Controls[num].UserData;
					obj.Data.CloudImagePath = imagePath;
					obj.Size += (ulong)fileSize;
					((MyGuiControlContentButton)m_BPList.Controls[num]).CreatePreview(imagePath, GetThumbnailVisibility());
				}
			}
			catch (Exception)
			{
				if (writeAttempts < 5)
				{
					Thread.CurrentThread.Join(50);
					OnCloudImageDownloaded(data, name, imagePath, fileSize, ++writeAttempts);
				}
<<<<<<< HEAD
=======
			}
			SortBlueprints(data, MyBlueprintTypeEnum.CLOUD);
			AddBlueprintButtons(ref data);
		}

		private void OnCloudImageDownloaded(byte[] data, string name, string imagePath, int fileSize, int writeAttempts = 1)
		{
			try
			{
				File.WriteAllBytes(imagePath, data);
				MyRenderProxy.UnloadTexture(imagePath);
				MyRenderProxy.PreloadTextures(new string[1] { imagePath }, TextureType.GUIWithoutPremultiplyAlpha);
				int num = m_BPList.Controls.FindIndex((MyGuiControlBase item) => ((MyBlueprintItemInfo)item.UserData).BlueprintName == name);
				if (num >= 0)
				{
					MyBlueprintItemInfo obj = (MyBlueprintItemInfo)m_BPList.Controls[num].UserData;
					obj.Data.CloudImagePath = imagePath;
					obj.Size += (ulong)fileSize;
					((MyGuiControlContentButton)m_BPList.Controls[num]).CreatePreview(imagePath);
				}
			}
			catch (Exception)
			{
				if (writeAttempts < 5)
				{
					Thread.get_CurrentThread().Join(50);
					OnCloudImageDownloaded(data, name, imagePath, fileSize, ++writeAttempts);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void GetWorkshopItemsSteam()
		{
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			using (SubscribedItemsLock.AcquireSharedUsing())
			{
				List<MyWorkshopItem> subscribedItemsList = GetSubscribedItemsList();
				for (int i = 0; i < subscribedItemsList.Count; i++)
				{
					MyWorkshopItem myWorkshopItem = subscribedItemsList[i];
					IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(myWorkshopItem.ServiceName);
					if (aggregate == null || aggregate.IsConsentGiven)
					{
						string title = myWorkshopItem.Title;
						MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
						{
							BlueprintName = title,
							Item = myWorkshopItem,
							Size = myWorkshopItem.Size
						};
<<<<<<< HEAD
						myBlueprintItemInfo.SetAdditionalBlueprintInformation(title, title, myWorkshopItem.DLCs.ToArray());
=======
						myBlueprintItemInfo.SetAdditionalBlueprintInformation(title, title, Enumerable.ToArray<uint>((IEnumerable<uint>)myWorkshopItem.DLCs));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						data.Add(myBlueprintItemInfo);
					}
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.WORKSHOP);
			AddBlueprintButtons(ref data);
		}

		private static void CheckCurrentLocalDirectory_Blueprint()
		{
			if (!Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(Content.Blueprint))))
			{
				SetCurrentLocalDirectory(Content.Blueprint, string.Empty);
			}
		}

		private bool UpdatePrefab(MyBlueprintItemInfo data, bool loadPrefab)
		{
			bool result = true;
			m_loadedPrefab = null;
			if (data != null)
			{
				switch (data.Type)
				{
				case MyBlueprintTypeEnum.LOCAL:
				{
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(Content.Blueprint), data.Data.Name, "bp.sbc");
					if (!File.Exists(text))
					{
						break;
					}
					if (loadPrefab)
					{
						m_loadedPrefab = MyBlueprintUtils.LoadPrefab(text);
					}
					try
					{
<<<<<<< HEAD
						using (FileStream sbcStream2 = new FileStream(text, FileMode.Open))
						{
							UpdateNameAndDescription();
							UpdateInfo(LoadXDocument(sbcStream2), data);
							UpdateDetailKeyEnable();
							return result;
						}
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine($"Failed to open {text}.\nException: {ex.Message}");
						return result;
					}
=======
						using FileStream sbcStream2 = new FileStream(text, FileMode.Open);
						UpdateNameAndDescription();
						UpdateInfo(LoadXDocument(sbcStream2), data);
						UpdateDetailKeyEnable();
						return result;
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine($"Failed to open {text}.\nException: {ex.Message}");
						return result;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				case MyBlueprintTypeEnum.WORKSHOP:
				{
					if (data.Item == null)
					{
						break;
					}
					result = false;
					MyWorkshopItem workshopData = data.Item;
					Task = Parallel.Start(delegate
					{
						if (!MyWorkshop.IsUpToDate(workshopData))
						{
							DownloadBlueprintFromSteam(workshopData);
						}
						OnBlueprintDownloadedDetails(workshopData, loadPrefab);
					}, delegate
					{
					});
					break;
				}
				case MyBlueprintTypeEnum.CLOUD:
				{
					if (loadPrefab)
					{
						m_loadedPrefab = MyBlueprintUtils.LoadPrefabFromCloud(data);
					}
					byte[] array = MyGameService.LoadFromCloud(data.CloudPathXML);
					if (array != null)
					{
						using (MemoryStream stream = new MemoryStream(array))
						{
							UpdateNameAndDescription();
							UpdateInfo(LoadXDocument(stream.UnwrapGZip()), data);
							UpdateDetailKeyEnable();
							return result;
						}
					}
					break;
				}
				case MyBlueprintTypeEnum.DEFAULT:
				{
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, data.Data.Name, "bp.sbc");
					if (File.Exists(text))
					{
						if (loadPrefab)
						{
							m_loadedPrefab = MyBlueprintUtils.LoadPrefab(text);
						}
<<<<<<< HEAD
						using (FileStream sbcStream = new FileStream(text, FileMode.Open))
						{
							UpdateNameAndDescription();
							UpdateInfo(LoadXDocument(sbcStream), data);
							UpdateDetailKeyEnable();
							return result;
						}
=======
						using FileStream sbcStream = new FileStream(text, FileMode.Open);
						UpdateNameAndDescription();
						UpdateInfo(LoadXDocument(sbcStream), data);
						UpdateDetailKeyEnable();
						return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				}
			}
			return result;
		}

		private void OnBlueprintDownloadedDetails(MyWorkshopItem workshopDetails, bool loadPrefab = true)
		{
			if (SelectedBlueprint == null || SelectedBlueprint.Item == null || workshopDetails.Id != SelectedBlueprint.Item.Id || workshopDetails.ServiceName != SelectedBlueprint.Item.ServiceName)
			{
				return;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				UpdateDetailKeyEnable();
				if (SelectedBlueprint != null && SelectedBlueprint.Item != null)
				{
					UpdateNameAndDescription();
				}
			}, "OnBlueprintDownloadedDetails");
			string folder = workshopDetails.Folder;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = null;
			if (loadPrefab)
			{
				myObjectBuilder_Definitions = MyBlueprintUtils.LoadWorkshopPrefab(folder, SelectedBlueprint.Item?.Id, SelectedBlueprint.Item?.ServiceName, isOldBlueprintScreen: false);
				if (myObjectBuilder_Definitions == null)
				{
					return;
				}
			}
			if (SelectedBlueprint == null || SelectedBlueprint.Item == null || workshopDetails.Id != SelectedBlueprint.Item.Id || workshopDetails.ServiceName != SelectedBlueprint.Item.ServiceName)
<<<<<<< HEAD
			{
				return;
			}
			if (loadPrefab)
			{
				m_loadedPrefab = myObjectBuilder_Definitions;
			}
			if (folder == null)
			{
				return;
			}
			string path = Path.Combine(folder, "bp.sbc");
			if (!MyFileSystem.FileExists(path))
=======
			{
				return;
			}
			if (loadPrefab)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
<<<<<<< HEAD
			using (Stream sbcStream = MyFileSystem.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				XDocument doc = LoadXDocument(sbcStream);
				MySandboxGame.Static.Invoke(delegate
				{
					UpdateInfo(doc, SelectedBlueprint);
				}, "OnBlueprintDownloadedDetails");
=======
			if (folder == null)
			{
				return;
			}
			string path = Path.Combine(folder, "bp.sbc");
			if (!MyFileSystem.FileExists(path))
			{
				return;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			using Stream sbcStream = MyFileSystem.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			XDocument doc = LoadXDocument(sbcStream);
			MySandboxGame.Static.Invoke(delegate
			{
				UpdateInfo(doc, SelectedBlueprint);
			}, "OnBlueprintDownloadedDetails");
		}

		private void UpdateNameAndDescription()
		{
			if (SelectedBlueprint == null || m_detailName == null || m_multiline == null)
<<<<<<< HEAD
			{
				return;
			}
			if (SelectedBlueprint.Item == null && SelectedBlueprint.Data != null)
			{
=======
			{
				return;
			}
			if (SelectedBlueprint.Item == null && SelectedBlueprint.Data != null)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_detailName.Text = SelectedBlueprint.Data.Name;
				StringBuilder text = new StringBuilder(SelectedBlueprint.Data.Description);
				if (SelectedBlueprint.Data.DLCs == null || SelectedBlueprint.Data.DLCs.Length == 0)
				{
					m_multiline.Text = text;
					CheckAndSplitLongName();
				}
				else
				{
					m_multiline.Text = text;
				}
			}
			else if (SelectedBlueprint.Item != null)
			{
				StringBuilder text2 = new StringBuilder(SelectedBlueprint.Item.Description);
				m_multiline.Text = text2;
				string text3 = SelectedBlueprint.Item.Title;
				if (text3.Length > 80)
				{
					text3 = text3.Substring(0, 80);
				}
				m_detailName.Text = text3;
				CheckAndSplitLongName();
			}
		}

		public void OnPathSelected(bool confirmed, string pathNew)
		{
			if (confirmed)
			{
				SetCurrentLocalDirectory(m_content, pathNew);
				RefreshAndReloadItemList();
			}
		}

		public void CreateBlueprintFromClipboard(bool withScreenshot = false, bool replace = false, MyBlueprintItemInfo info = null)
		{
			string text = info?.BlueprintName;
			if (m_clipboard.CopiedGrids == null || m_clipboard.CopiedGrids.Count <= 0 || (m_clipboard.CopiedGridsName == null && string.IsNullOrEmpty(text)))
			{
				return;
			}
			string text2 = (string.IsNullOrEmpty(text) ? MyUtils.StripInvalidChars(m_clipboard.CopiedGridsName) : text);
			string text3 = text2;
			ulong workshopId = 0uL;
			if (!replace)
			{
				int num = 1;
				if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
				{
					string text4 = "Blueprints/cloud/";
					List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(text4 + text2 + "/");
					while (cloudFiles != null && cloudFiles.Count > 0)
					{
						text3 = text2 + "_" + num;
						string directoryFilter = text4 + text3 + "/";
						num++;
						cloudFiles = MyGameService.GetCloudFiles(directoryFilter);
					}
				}
				else
				{
					string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), text2);
					while (MyFileSystem.DirectoryExists(path))
					{
						text3 = text2 + "_" + num;
						path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), text3);
						num++;
					}
				}
			}
			else if (MySandboxGame.Config.EnableSteamCloud)
			{
				MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyBlueprintUtils.LoadPrefabFromCloud(info);
				if (myObjectBuilder_Definitions != null)
				{
					MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = myObjectBuilder_Definitions.ShipBlueprints;
					if (shipBlueprints != null && shipBlueprints.Length != 0)
					{
						workshopId = myObjectBuilder_Definitions.ShipBlueprints[0].WorkshopId;
					}
				}
			}
			MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
			myObjectBuilder_ShipBlueprintDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(text2));
			myObjectBuilder_ShipBlueprintDefinition.CubeGrids = m_clipboard.CopiedGrids.ToArray();
			if (myObjectBuilder_ShipBlueprintDefinition.CubeGrids == null || myObjectBuilder_ShipBlueprintDefinition.CubeGrids.Length == 0)
			{
				return;
			}
			myObjectBuilder_ShipBlueprintDefinition.DLCs = GetNecessaryDLCs(myObjectBuilder_ShipBlueprintDefinition.CubeGrids);
			myObjectBuilder_ShipBlueprintDefinition.RespawnShip = false;
			myObjectBuilder_ShipBlueprintDefinition.DisplayName = MyGameService.UserName;
			myObjectBuilder_ShipBlueprintDefinition.OwnerSteamId = Sync.MyId;
			myObjectBuilder_ShipBlueprintDefinition.CubeGrids[0].DisplayName = text2;
			myObjectBuilder_ShipBlueprintDefinition.WorkshopId = workshopId;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			myObjectBuilder_Definitions2.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
			myObjectBuilder_Definitions2.ShipBlueprints[0] = myObjectBuilder_ShipBlueprintDefinition;
			if (MyGameService.IsActive && MySandboxGame.Config.EnableSteamCloud && withScreenshot)
			{
				SavePrefabToCloudWithScreenshot(myObjectBuilder_Definitions2, text3, GetCurrentLocalDirectory(), replace, SelectNewBlueprintAfterCloudSave);
				return;
			}
			MyBlueprintUtils.SavePrefabToFile(myObjectBuilder_Definitions2, text3, GetCurrentLocalDirectory(), replace);
			SetGroupSelection(MyBlueprintTypeEnum.MIXED);
			RefreshAndSelectNewBlueprint(text3, (!MySandboxGame.Config.EnableSteamCloud || !MyGameService.IsActive) ? MyBlueprintTypeEnum.LOCAL : MyBlueprintTypeEnum.CLOUD);
			if (withScreenshot)
			{
				TakeScreenshotLocalBP(text3, m_selectedButton);
			}
		}

		private string[] GetNecessaryDLCs(MyObjectBuilder_CubeGrid[] cubeGrids)
		{
			if (cubeGrids.IsNullOrEmpty())
			{
				return null;
			}
			HashSet<string> val = new HashSet<string>();
			for (int i = 0; i < cubeGrids.Length; i++)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrids[i].CubeBlocks)
				{
					MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(cubeBlock);
					if (cubeBlockDefinition != null && cubeBlockDefinition.DLCs != null && cubeBlockDefinition.DLCs.Length != 0)
					{
						for (int j = 0; j < cubeBlockDefinition.DLCs.Length; j++)
						{
							val.Add(cubeBlockDefinition.DLCs[j]);
						}
					}
				}
			}
			return Enumerable.ToArray<string>((IEnumerable<string>)val);
		}

		public void SelectBlueprint(MyBlueprintItemInfo itemInfo)
		{
			int num = -1;
			MyGuiControlContentButton myGuiControlContentButton = null;
			foreach (MyGuiControlRadioButton item in m_BPTypesGroup)
			{
				num++;
				MyBlueprintItemInfo myBlueprintItemInfo = item.UserData as MyBlueprintItemInfo;
				if (myBlueprintItemInfo != null && myBlueprintItemInfo.Equals(itemInfo))
				{
					myGuiControlContentButton = item as MyGuiControlContentButton;
					break;
				}
			}
			if (myGuiControlContentButton != null)
			{
				SelectButton(myGuiControlContentButton, num);
			}
		}

		public void RefreshAndSelectNewBlueprint(string name, MyBlueprintTypeEnum type)
		{
			RefreshBlueprintList();
			SelectNewBlueprint(name, type);
		}

		public void SelectNewBlueprint(string name, MyBlueprintTypeEnum type)
		{
			int num = -1;
			MyGuiControlContentButton myGuiControlContentButton = null;
			foreach (MyGuiControlRadioButton item in m_BPTypesGroup)
			{
				num++;
				MyBlueprintItemInfo myBlueprintItemInfo = item.UserData as MyBlueprintItemInfo;
				if (myBlueprintItemInfo != null && myBlueprintItemInfo.Type == type && myBlueprintItemInfo.Data.Name.Equals(name))
				{
					myGuiControlContentButton = item as MyGuiControlContentButton;
					break;
				}
			}
			if (myGuiControlContentButton != null)
			{
				SelectButton(myGuiControlContentButton, num);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="butt"></param>
		/// <param name="idx"></param>
		/// <param name="forceToTop">If true, newly selected item will be always put on the top.</param>
		/// <param name="alwaysScroll">If true, newly selected item will be always selected, even though it was already within the screen.</param>
		public void SelectButton(MyGuiControlContentButton butt, int idx = -1, bool forceToTop = true, bool alwaysScroll = true)
		{
			if (idx < 0)
			{
				bool flag = false;
				int num = -1;
				foreach (MyGuiControlRadioButton item in m_BPTypesGroup)
				{
					num++;
					if (butt == item)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
				idx = num;
			}
			if (m_selectedButton != butt)
			{
				m_BPTypesGroup.SelectByIndex(idx);
			}
			float min;
			float max;
			ScrollTestResult scrollTestResult = ShouldScroll(butt, idx, out min, out max);
			if (!alwaysScroll && scrollTestResult == ScrollTestResult.Ok)
			{
				return;
			}
			float scrollBarPage;
			if (forceToTop || scrollTestResult == ScrollTestResult.Lower)
			{
				scrollBarPage = min;
			}
			else
			{
				if (scrollTestResult != ScrollTestResult.Higher)
				{
					return;
				}
				scrollBarPage = max - 1f;
			}
			m_BPList.SetScrollBarPage(scrollBarPage);
		}

		private ScrollTestResult ShouldScroll(MyGuiControlContentButton butt, int idx, out float min, out float max)
		{
			float num = ((!GetThumbnailVisibility()) ? MAGIC_SPACING_SMALL : MAGIC_SPACING_BIG);
			int num2 = 0;
			for (int i = 0; i < idx; i++)
			{
				if (!m_BPList.Controls[i].Visible)
				{
					num2++;
				}
			}
			float num3 = butt.Size.Y + m_BPList.GetItemMargins().SizeChange.Y - num;
			float y = m_BPList.Size.Y;
			float num4 = (float)(idx - num2) * num3 / y;
			float num5 = ((float)(idx - num2) + 1f) * num3 / y;
			float page = m_BPList.GetScrollBar().GetPage();
			min = num4;
			max = num5;
			if (num4 < page)
			{
				return ScrollTestResult.Lower;
			}
			if (num5 > page + 1f)
			{
				return ScrollTestResult.Higher;
			}
			return ScrollTestResult.Ok;
		}

		public void SavePrefabToCloudWithScreenshot(MyObjectBuilder_Definitions prefab, string name, string currentDirectory, bool replace = false, Action<string, CloudResult> onCompleted = null)
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
<<<<<<< HEAD
			if (name.Any((char x) => invalidPathChars.Contains(x)))
=======
			if (Enumerable.Any<char>((IEnumerable<char>)name, (Func<char, bool>)((char x) => invalidPathChars.Contains(x))))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				StringBuilder stringBuilder = new StringBuilder(name);
				for (int i = 0; i < name.Length; i++)
				{
					if (invalidPathChars.Contains(name[i]))
					{
						stringBuilder[i] = '_';
					}
				}
				MyLog.Default.WriteLine("Blueprint name with invalid characters: " + name + ", renamed to " + stringBuilder);
				name = stringBuilder.ToString();
			}
			string path = Path.Combine("Blueprints/cloud", name);
			string filePath = Path.Combine(path, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
			string text = Path.Combine(path, MyBlueprintUtils.THUMB_IMAGE_NAME);
			if (m_waitingForScreenshot.IsWaiting())
			{
				onCompleted?.Invoke(name, CloudResult.Failed);
				return;
			}
			string text2 = Path.Combine(MyFileSystem.UserDataPath, text);
			m_waitingForScreenshot.SetData_CreateNewBlueprintCloud(text, text2);
			MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), text2, debug: false, ignoreSprites: true, showNotification: false);
			MyBlueprintUtils.SaveToCloud(prefab, filePath, replace, onCompleted);
		}

		private void CreateScriptFromEditor()
		{
			if (m_getCodeFromEditor == null)
			{
				return;
			}
			bool flag = MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive;
<<<<<<< HEAD
			string sourceFileName = Path.Combine(MyFileSystem.ContentPath, MyBlueprintUtils.STEAM_THUMBNAIL_NAME);
			string text = (flag ? MyBlueprintUtils.SCRIPT_FOLDER_TEMP : MyBlueprintUtils.SCRIPT_FOLDER_LOCAL);
			string contents = m_getCodeFromEditor();
			Directory.CreateDirectory(text);
			int num = 0;
			if (!Directory.Exists(text))
			{
				return;
			}
			string text2;
			string text3;
			do
			{
				text2 = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + num;
				text3 = Path.Combine(text, GetCurrentLocalDirectory(), text2);
				num++;
			}
			while (Directory.Exists(text3));
			Directory.CreateDirectory(text3);
			string text4 = Path.Combine(text3, MyBlueprintUtils.THUMB_IMAGE_NAME);
			File.Copy(sourceFileName, text4, overwrite: true);
			MyRenderProxy.UnloadTexture(text4);
			File.WriteAllText(Path.Combine(text3, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), contents, Encoding.UTF8);
			if (flag)
			{
				num = 0;
				string text5;
				string text6;
				List<MyCloudFileInfo> cloudFiles;
				do
				{
					text5 = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + num;
					text6 = "Scripts/cloud/" + text5 + "/";
					cloudFiles = MyGameService.GetCloudFiles(text6);
					num++;
				}
				while (cloudFiles != null && cloudFiles.Count > 0);
				MyCloudHelper.UploadFiles(text6, text3, compress: false);
				RefreshAndSelectNewBlueprint(text5, MyBlueprintTypeEnum.CLOUD);
			}
			else
			{
				RefreshAndSelectNewBlueprint(text2, MyBlueprintTypeEnum.LOCAL);
=======
			string text = Path.Combine(MyFileSystem.ContentPath, MyBlueprintUtils.STEAM_THUMBNAIL_NAME);
			string text2 = (flag ? MyBlueprintUtils.SCRIPT_FOLDER_TEMP : MyBlueprintUtils.SCRIPT_FOLDER_LOCAL);
			string text3 = m_getCodeFromEditor();
			Directory.CreateDirectory(text2);
			int num = 0;
			if (!Directory.Exists(text2))
			{
				return;
			}
			string text4;
			string text5;
			do
			{
				text4 = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + num;
				text5 = Path.Combine(text2, GetCurrentLocalDirectory(), text4);
				num++;
			}
			while (Directory.Exists(text5));
			Directory.CreateDirectory(text5);
			string text6 = Path.Combine(text5, MyBlueprintUtils.THUMB_IMAGE_NAME);
			File.Copy(text, text6, true);
			MyRenderProxy.UnloadTexture(text6);
			File.WriteAllText(Path.Combine(text5, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), text3, Encoding.UTF8);
			if (flag)
			{
				num = 0;
				string text7;
				string text8;
				List<MyCloudFileInfo> cloudFiles;
				do
				{
					text7 = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + num;
					text8 = "Scripts/cloud/" + text7 + "/";
					cloudFiles = MyGameService.GetCloudFiles(text8);
					num++;
				}
				while (cloudFiles != null && cloudFiles.Count > 0);
				MyCloudHelper.UploadFiles(text8, text5, compress: false);
				RefreshAndSelectNewBlueprint(text7, MyBlueprintTypeEnum.CLOUD);
			}
			else
			{
				RefreshAndSelectNewBlueprint(text4, MyBlueprintTypeEnum.LOCAL);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void ChangeName(string name)
		{
			name = MyUtils.StripInvalidChars(name);
			string path = string.Empty;
			switch (m_content)
			{
			case Content.Blueprint:
				path = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
				break;
			case Content.Script:
				path = MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
				break;
			}
			string name2 = SelectedBlueprint.Data.Name;
			string file = Path.Combine(path, GetCurrentLocalDirectory(), name2);
			string newFile = Path.Combine(path, GetCurrentLocalDirectory(), name);
			if (file == newFile || !Directory.Exists(file))
			{
				return;
			}
			if (Directory.Exists(newFile))
			{
				if (file.ToLower() == newFile.ToLower())
				{
					switch (m_content)
					{
					case Content.Blueprint:
					{
						UpdatePrefab(SelectedBlueprint, loadPrefab: true);
						m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
						m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
						m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
						string text = Path.Combine(path, "temp");
						if (Directory.Exists(text))
						{
							Directory.Delete(text, true);
						}
						Directory.Move(file, text);
						Directory.Move(text, newFile);
						MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, GetCurrentLocalDirectory(), replace: true);
						break;
					}
					case Content.Script:
						if (Directory.Exists(file))
						{
							Directory.Move(file, newFile);
						}
						if (Directory.Exists(file))
						{
							Directory.Delete(file, true);
						}
						break;
					}
					RefreshBlueprintList();
					UpdatePrefab(SelectedBlueprint, loadPrefab: false);
					using (FileStream sbcStream = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
					{
						UpdateInfo(LoadXDocument(sbcStream), SelectedBlueprint);
					}
					return;
				}
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Replace), messageText: new StringBuilder().Append((object)MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage1)).Append(name).Append((object)MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage2)), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						switch (m_content)
						{
						case Content.Blueprint:
							DeleteItem(newFile);
							UpdatePrefab(SelectedBlueprint, loadPrefab: true);
							m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
							m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
							m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
							Directory.Move(file, newFile);
							MyRenderProxy.UnloadTexture(Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME));
							MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, GetCurrentLocalDirectory(), replace: true);
							break;
						case Content.Script:
<<<<<<< HEAD
							Directory.Delete(newFile, recursive: true);
=======
							Directory.Delete(newFile, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							if (Directory.Exists(file))
							{
								Directory.Move(file, newFile);
							}
							if (Directory.Exists(file))
							{
<<<<<<< HEAD
								Directory.Delete(file, recursive: true);
=======
								Directory.Delete(file, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
							break;
						}
						RefreshBlueprintList();
						UpdatePrefab(SelectedBlueprint, loadPrefab: false);
<<<<<<< HEAD
						using (FileStream sbcStream3 = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
						{
							UpdateInfo(LoadXDocument(sbcStream3), SelectedBlueprint);
						}
=======
						using FileStream sbcStream3 = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open);
						UpdateInfo(LoadXDocument(sbcStream3), SelectedBlueprint);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}));
				return;
			}
			try
			{
				switch (m_content)
				{
				case Content.Blueprint:
					UpdatePrefab(SelectedBlueprint, loadPrefab: true);
					m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
					m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
					m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
					Directory.Move(file, newFile);
					MyRenderProxy.UnloadTexture(Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME));
					MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, GetCurrentLocalDirectory(), replace: true);
					break;
				case Content.Script:
					if (Directory.Exists(file))
					{
						Directory.Move(file, newFile);
					}
					if (Directory.Exists(file))
					{
<<<<<<< HEAD
						Directory.Delete(file, recursive: true);
=======
						Directory.Delete(file, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				RefreshBlueprintList();
				UpdatePrefab(SelectedBlueprint, loadPrefab: false);
<<<<<<< HEAD
				using (FileStream sbcStream2 = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
				{
					UpdateInfo(LoadXDocument(sbcStream2), SelectedBlueprint);
				}
=======
				using FileStream sbcStream2 = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open);
				UpdateInfo(LoadXDocument(sbcStream2), SelectedBlueprint);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (IOException)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Delete), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_DeleteMessage)));
			}
		}

		private void ChangeBlueprintNameCloud(string name)
		{
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyCloudHelper.Combine((m_content == Content.Blueprint) ? "Blueprints/cloud" : "Scripts/cloud", name) + "/");
			if (cloudFiles == null)
			{
				return;
			}
			if (SelectedBlueprint != null && SelectedBlueprint.Data != null && SelectedBlueprint.Data.CloudImagePath != null)
			{
				SelectedBlueprint.Data.CloudImagePath = SelectedBlueprint.Data.CloudImagePath.Replace("/", "\\");
			}
			if (cloudFiles.Count > 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Replace), messageText: new StringBuilder().Append((object)MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage1)).Append(name).Append((object)MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage2)), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						ChangeBlueprintNameCloudInternal(name);
					}
				}));
			}
			else
			{
				ChangeBlueprintNameCloudInternal(name);
			}
		}

		private void ChangeBlueprintNameCloudInternal(string name)
		{
			if (m_content == Content.Blueprint)
			{
				UpdatePrefab(SelectedBlueprint, loadPrefab: true);
				if (m_loadedPrefab == null)
				{
					ShowChangeNameCloudError(CloudResult.Failed);
				}
				else
				{
					new CloudBlueprintRenamer(m_loadedPrefab, GetImagePath(SelectedBlueprint), name, FinishChangeBlueprintNameCloud);
				}
			}
			else if (m_content == Content.Script)
			{
				string text = MyCloudHelper.Combine("Scripts/cloud", SelectedBlueprint.BlueprintName) + "/";
				string newSessionPath = MyCloudHelper.Combine("Scripts/cloud", name) + "/";
				CloudResult cloudResult = MyCloudHelper.CopyFiles(text, newSessionPath);
				if (cloudResult == CloudResult.Ok)
				{
					MyCloudHelper.Delete(text);
					SelectedBlueprint = null;
					ResetBlueprintUI();
					RefreshAndSelectNewBlueprint(name, MyBlueprintTypeEnum.CLOUD);
				}
				else
				{
					ShowChangeNameCloudError(cloudResult);
				}
			}
		}

		private void DeleteBlueprintCloud()
		{
			if (MyCloudHelper.Delete(MyCloudHelper.Combine("Blueprints/cloud", SelectedBlueprint.BlueprintName) + "/"))
			{
				SelectedBlueprint = null;
				ResetBlueprintUI();
			}
		}

		private void DeleteScriptCloud()
		{
			if (MyCloudHelper.Delete(MyCloudHelper.Combine("Scripts/cloud", SelectedBlueprint.BlueprintName) + "/"))
			{
				SelectedBlueprint = null;
				ResetBlueprintUI();
			}
		}

		private void FinishChangeBlueprintNameCloud(CloudResult result, string filePath)
		{
			if (result == CloudResult.Ok)
			{
				DeleteBlueprintCloud();
				SelectNewBlueprintAfterCloudSave(filePath, result);
			}
			else
			{
				ShowChangeNameCloudError(result);
			}
		}

		private void SelectNewBlueprintAfterCloudSave(string filePath, CloudResult result)
		{
			if (!MyCloudHelper.IsError(result, out var errorMessage))
			{
				string[] array = filePath.Split('\\', '/');
				if (array.Length > 2)
				{
					string name = array[array.Length - 2];
					RefreshAndSelectNewBlueprint(name, MyBlueprintTypeEnum.CLOUD);
				}
			}
			else
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox.SkipTransition = true;
				myGuiScreenMessageBox.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
			}
		}

		private void ShowChangeNameCloudError(CloudResult result)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCloudHelper.GetErrorMessage(result))));
		}

		public static bool CopyBlueprintPrefabToClipboard(MyObjectBuilder_Definitions prefab, MyGridClipboard clipboard, bool setOwner = true)
		{
			if (prefab.ShipBlueprints == null)
			{
				return false;
			}
			MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
			if (cubeGrids == null || cubeGrids.Length == 0)
			{
				return false;
			}
			BoundingSphere boundingSphere = cubeGrids[0].CalculateBoundingSphere();
			MyPositionAndOrientation value = cubeGrids[0].PositionAndOrientation.Value;
			MatrixD m = MatrixD.CreateWorld(value.Position, value.Forward, value.Up);
			MatrixD m2 = MatrixD.Invert(m);
			Matrix matrix = Matrix.Normalize(m2);
			BoundingSphere boundingSphere2 = boundingSphere.Transform(m);
			Vector3 dragPointDelta = Vector3.TransformNormal((Vector3)(Vector3D)value.Position - boundingSphere2.Center, matrix);
			float dragVectorLength = boundingSphere.Radius + 10f;
			if ((!MySession.Static.IsUserAdmin(Sync.MyId) || !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.KeepOriginalOwnershipOnPaste)) && setOwner)
			{
				MyObjectBuilder_CubeGrid[] array = cubeGrids;
				for (int i = 0; i < array.Length; i++)
				{
					foreach (MyObjectBuilder_CubeBlock cubeBlock in array[i].CubeBlocks)
					{
						if (cubeBlock.Owner != 0L)
						{
							cubeBlock.Owner = MySession.Static.LocalPlayerId;
						}
					}
				}
			}
			if (MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				for (int j = 0; j < cubeGrids.Length; j++)
				{
					cubeGrids[j] = MyFracturedBlock.ConvertFracturedBlocksToComponents(cubeGrids[j]);
				}
			}
			clipboard.SetGridFromBuilders(cubeGrids, dragPointDelta, dragVectorLength);
			clipboard.ShowModdedBlocksWarning = false;
			return true;
		}

		private void CopyBlueprintAndClose()
		{
			if (CopySelectedItemToClipboard())
			{
				CloseScreen();
			}
		}

		private bool CopySelectedItemToClipboard()
		{
			if (!ValidateSelecteditem())
			{
				return false;
			}
			string text = "";
			MyObjectBuilder_Definitions prefab = null;
			m_clipboard.Deactivate();
			switch (SelectedBlueprint.Type)
			{
			case MyBlueprintTypeEnum.WORKSHOP:
				text = SelectedBlueprint.Item.Folder;
				if (File.Exists(text) || MyFileSystem.IsDirectory(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this, SelectedBlueprint.Item.Id, SelectedBlueprint.Item.ServiceName);
					Task = Parallel.Start(LoadPrefabData.CallLoadWorkshopPrefab, LoadPrefabData.CallOnPrefabLoaded, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.LOCAL:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), SelectedBlueprint.Data.Name, "bp.sbc");
				if (File.Exists(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this);
					Task = Parallel.Start(LoadPrefabData.CallLoadPrefab, LoadPrefabData.CallOnPrefabLoaded, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.SHARED:
				return false;
			case MyBlueprintTypeEnum.DEFAULT:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, SelectedBlueprint.Data.Name, "bp.sbc");
				if (File.Exists(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this);
					Task = Parallel.Start(LoadPrefabData.CallLoadPrefab, LoadPrefabData.CallOnPrefabLoaded, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_LoadPrefabData = new LoadPrefabData(prefab, SelectedBlueprint, this);
				Task = Parallel.Start(LoadPrefabData.CallLoadPrefabFromCloud, LoadPrefabData.CallOnPrefabLoaded, m_LoadPrefabData);
				break;
			}
			return false;
		}

		private void SortBlueprints(List<MyBlueprintItemInfo> list, MyBlueprintTypeEnum type)
		{
			MyItemComparer_Rew myItemComparer_Rew = null;
			switch (type)
			{
			case MyBlueprintTypeEnum.LOCAL:
			case MyBlueprintTypeEnum.CLOUD:
				switch (GetSelectedSort())
				{
				case SortOption.Alphabetical:
					myItemComparer_Rew = new MyItemComparer_Rew((MyBlueprintItemInfo x, MyBlueprintItemInfo y) => x.BlueprintName.CompareTo(y.BlueprintName));
					break;
				case SortOption.UpdateDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime? timeUpdated = x.TimeUpdated;
						DateTime? timeUpdated2 = y.TimeUpdated;
						return (timeUpdated.HasValue && timeUpdated2.HasValue) ? (-1 * DateTime.Compare(timeUpdated.Value, timeUpdated2.Value)) : 0;
					});
					break;
				case SortOption.CreationDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime? timeCreated = x.TimeCreated;
						DateTime? timeCreated2 = y.TimeCreated;
						return (timeCreated.HasValue && timeCreated2.HasValue) ? (-1 * DateTime.Compare(timeCreated.Value, timeCreated2.Value)) : 0;
					});
					break;
				}
				break;
			case MyBlueprintTypeEnum.WORKSHOP:
				switch (GetSelectedSort())
				{
				case SortOption.Alphabetical:
					myItemComparer_Rew = new MyItemComparer_Rew((MyBlueprintItemInfo x, MyBlueprintItemInfo y) => x.BlueprintName.CompareTo(y.BlueprintName));
					break;
				case SortOption.CreationDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime timeCreated3 = x.Item.TimeCreated;
						DateTime timeCreated4 = y.Item.TimeCreated;
						if (timeCreated3 < timeCreated4)
						{
							return 1;
						}
						return (timeCreated3 > timeCreated4) ? (-1) : 0;
					});
					break;
				case SortOption.UpdateDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime timeUpdated3 = x.Item.TimeUpdated;
						DateTime timeUpdated4 = y.Item.TimeUpdated;
						if (timeUpdated3 < timeUpdated4)
						{
							return 1;
						}
						return (timeUpdated3 > timeUpdated4) ? (-1) : 0;
					});
					break;
				}
				break;
			}
			if (myItemComparer_Rew != null)
			{
				list.Sort(myItemComparer_Rew);
			}
		}

<<<<<<< HEAD
		[Event(null, 4438)]
=======
		[Event(null, 4358)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void ShareBlueprintRequest(ulong workshopId, string workshopServiceName, string name, ulong sendToId, string senderName)
		{
			if (Sync.IsServer && sendToId != Sync.MyId)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShareBlueprintRequestClient, workshopId, workshopServiceName, name, sendToId, senderName, new EndpointId(sendToId));
			}
			else
			{
				ShareBlueprintRequestClient(workshopId, workshopServiceName, name, sendToId, senderName);
			}
		}

<<<<<<< HEAD
		[Event(null, 4451)]
=======
		[Event(null, 4371)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void ShareBlueprintRequestClient(ulong workshopId, string workshopServiceName, string name, ulong sendToId, string senderName)
		{
			MyWorkshopItem myWorkshopItem = MyGameService.CreateWorkshopItem(workshopServiceName);
			if (myWorkshopItem != null)
			{
				myWorkshopItem.Id = workshopId;
				myWorkshopItem.Title = name;
				MyBlueprintItemInfo info = new MyBlueprintItemInfo(MyBlueprintTypeEnum.SHARED)
				{
					BlueprintName = name,
					Item = myWorkshopItem
				};
				info.SetAdditionalBlueprintInformation(name);
<<<<<<< HEAD
				if (!m_recievedBlueprints.Any((MyBlueprintItemInfo item2) => item2.Item?.Id == info.Item.Id && item2.Item?.ServiceName == info.Item.ServiceName))
=======
				if (!Enumerable.Any<MyBlueprintItemInfo>((IEnumerable<MyBlueprintItemInfo>)m_recievedBlueprints, (Func<MyBlueprintItemInfo, bool>)((MyBlueprintItemInfo item2) => item2.Item?.Id == info.Item.Id && item2.Item?.ServiceName == info.Item.ServiceName)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_recievedBlueprints.Add(info);
					MyHudNotificationDebug notification = new MyHudNotificationDebug(string.Format(MyTexts.Get(MySpaceTexts.SharedBlueprintNotify).ToString(), senderName));
					MyHud.Notifications.Add(notification);
				}
			}
		}

		private void OpenSharedBlueprint(MyBlueprintItemInfo itemInfo)
		{
			MyGuiSandbox.OpenUrl(itemInfo.Item.GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), itemInfo.Item.ServiceName), MyTexts.Get(MySpaceTexts.SharedBlueprint), new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), itemInfo.Item.ServiceName), MyTexts.Get(MySpaceTexts.SharedBlueprint), delegate
			{
				m_recievedBlueprints.Remove(SelectedBlueprint);
				SelectedBlueprint = null;
				UpdateDetailKeyEnable();
				RefreshBlueprintList();
			});
		}

		private void ApplyFiltering(MyGuiControlContentButton button)
		{
			if (button == null)
			{
				return;
			}
			bool flag = m_searchBox != null && m_searchBox.SearchText != "";
			string[] array = new string[0];
			if (flag)
			{
				array = m_searchBox.SearchText.Split(new char[1] { ' ' });
			}
			bool flag2 = true;
			MyBlueprintTypeEnum selectedBlueprintType = GetSelectedBlueprintType();
			MyBlueprintTypeEnum type = ((MyBlueprintItemInfo)button.UserData).Type;
			if ((selectedBlueprintType != MyBlueprintTypeEnum.MIXED && selectedBlueprintType != type) || (!MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE && type == MyBlueprintTypeEnum.LOCAL))
			{
				flag2 = false;
			}
			if (flag2 && flag)
			{
				string text = button.Title.ToString().ToLower();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text.Contains(text2.ToLower()))
					{
						flag2 = false;
						break;
					}
				}
			}
			if (flag2)
			{
				button.Visible = true;
			}
			else
			{
				button.Visible = false;
			}
		}

		private void ApplyFiltering()
		{
			bool flag = false;
			string[] array = new string[0];
			if (m_searchBox != null)
			{
				flag = m_searchBox.SearchText != "";
				array = m_searchBox.SearchText.Split(new char[1] { ' ' });
			}
			foreach (MyGuiControlBase control in m_BPList.Controls)
			{
				MyGuiControlContentButton myGuiControlContentButton = control as MyGuiControlContentButton;
				if (myGuiControlContentButton == null)
				{
					continue;
				}
				bool flag2 = true;
				MyBlueprintTypeEnum selectedBlueprintType = GetSelectedBlueprintType();
				MyBlueprintItemInfo myBlueprintItemInfo = (MyBlueprintItemInfo)myGuiControlContentButton.UserData;
				MyBlueprintTypeEnum type = myBlueprintItemInfo.Type;
				if ((selectedBlueprintType != MyBlueprintTypeEnum.MIXED && (selectedBlueprintType != type || (selectedBlueprintType == MyBlueprintTypeEnum.WORKSHOP && m_workshopIndex != MyGameService.GetUGCIndex(myBlueprintItemInfo.Item.ServiceName))) && (selectedBlueprintType != 0 || type != MyBlueprintTypeEnum.SHARED)) || (!MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE && type == MyBlueprintTypeEnum.LOCAL))
				{
					flag2 = false;
				}
				if (flag2 && flag)
				{
					string text = myGuiControlContentButton.Title.ToString().ToLower();
					string[] array2 = array;
					foreach (string text2 in array2)
					{
						if (!text.Contains(text2.ToLower()))
						{
							flag2 = false;
							break;
						}
					}
				}
				if (flag2)
				{
					control.Visible = true;
				}
				else
				{
					control.Visible = false;
				}
			}
			m_BPList.SetScrollBarPage();
		}

		public void TakeScreenshotLocalBP(string name, MyGuiControlContentButton button)
		{
			if (m_waitingForScreenshot.IsWaiting())
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken_Caption), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken)));
				return;
			}
			m_waitingForScreenshot.SetData_TakeScreenshotLocal(button);
			string pathToSave = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), name, MyBlueprintUtils.THUMB_IMAGE_NAME);
			MyGuiScreenGamePlay.Static.GetSize();
			float num = MyRenderProxy.MainViewport.Width / MyRenderProxy.MainViewport.Height;
			float num2 = (float)Math.Sqrt(262140f / num);
			Vector2 vector = new Vector2(num2 * num, num2);
			Vector2 sizeMultiplier = new Vector2(vector.X / MyRenderProxy.MainViewport.Width, vector.Y / MyRenderProxy.MainViewport.Height);
			if (sizeMultiplier.X > 1f)
			{
				sizeMultiplier.X = 1f;
			}
			if (sizeMultiplier.Y > 1f)
			{
				sizeMultiplier.Y = 1f;
			}
			MyRenderProxy.TakeScreenshot(sizeMultiplier, pathToSave, debug: false, ignoreSprites: true, showNotification: false);
		}

		public void TakeScreenshotCloud(string pathRel, string pathFull, MyGuiControlContentButton button)
		{
			if (m_waitingForScreenshot.IsWaiting())
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken_Caption), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken)));
				return;
			}
			m_waitingForScreenshot.SetData_TakeScreenshotCloud(pathRel, pathFull, button);
			MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), pathFull, debug: false, ignoreSprites: true, showNotification: false);
		}

		public static void ScreenshotTaken(bool success, string filename)
		{
			if (!m_waitingForScreenshot.IsWaiting())
			{
				return;
			}
			switch (m_waitingForScreenshot.Option)
			{
			case WaitForScreenshotOptions.TakeScreenshotLocal:
				if (success)
				{
					MyRenderProxy.UnloadTexture(filename);
					m_waitingForScreenshot.UsedButton?.CreatePreview(filename);
				}
				break;
			case WaitForScreenshotOptions.CreateNewBlueprintCloud:
				if (success)
				{
					if (File.Exists(m_waitingForScreenshot.PathFull))
					{
						MyBlueprintUtils.SaveToCloudFile(m_waitingForScreenshot.PathFull, m_waitingForScreenshot.PathRel);
					}
					m_newScreenshotTaken = true;
				}
				break;
			case WaitForScreenshotOptions.TakeScreenshotCloud:
				if (!success)
				{
					break;
				}
				if (File.Exists(m_waitingForScreenshot.PathFull))
				{
					MyBlueprintUtils.SaveToCloudFile(m_waitingForScreenshot.PathFull, m_waitingForScreenshot.PathRel);
				}
				MyRenderProxy.UnloadTexture(filename);
				if (m_waitingForScreenshot.UsedButton != null)
				{
					if (string.IsNullOrEmpty(m_waitingForScreenshot.UsedButton.PreviewImagePath))
					{
						m_waitingForScreenshot.UsedButton.CreatePreview(filename);
					}
					else
					{
						MyRenderProxy.UnloadTexture(m_waitingForScreenshot.UsedButton.PreviewImagePath);
					}
				}
				break;
			}
			m_waitingForScreenshot.Clear();
		}

		public override bool Update(bool hasFocus)
		{
			if (m_thumbnailImage.Visible)
			{
				UpdateThumbnailPosition();
			}
			if (Task.IsComplete && m_needsExtract)
			{
				GetWorkshopItemsSteam();
				m_needsExtract = false;
				RefreshBlueprintList();
			}
			if (m_wasPublished)
			{
				m_wasPublished = false;
				RefreshBlueprintList(fromTask: true);
			}
			if (m_newScreenshotTaken)
			{
				m_newScreenshotTaken = false;
				RefreshBlueprintList(fromTask: true);
			}
			return base.Update(hasFocus);
		}

		private void UpdateThumbnailPosition()
		{
			Vector2 vector = MyGuiManager.MouseCursorPosition + new Vector2(0.02f, 0.055f) + m_thumbnailImage.Size;
			if (vector.X <= 1f && vector.Y <= 1f)
			{
				m_thumbnailImage.Position = MyGuiManager.MouseCursorPosition + 0.5f * m_thumbnailImage.Size + new Vector2(-0.48f, -0.445f);
			}
			else
			{
				m_thumbnailImage.Position = MyGuiManager.MouseCursorPosition + new Vector2(0.5f * m_thumbnailImage.Size.X - 0.48f, -0.5f * m_thumbnailImage.Size.Y - 0.47f);
			}
		}

		private void OnMouseOverItem(MyGuiControlRadioButton butt, bool isMouseOver)
		{
			if (GetThumbnailVisibility())
			{
				return;
			}
			if (!isMouseOver)
			{
				m_thumbnailImage.SetTexture();
				m_thumbnailImage.Visible = false;
				return;
			}
			MyBlueprintItemInfo myBlueprintItemInfo = butt.UserData as MyBlueprintItemInfo;
			if (myBlueprintItemInfo == null)
			{
				m_thumbnailImage.SetTexture();
				m_thumbnailImage.Visible = false;
				return;
			}
			string imagePath = GetImagePath(myBlueprintItemInfo);
			if (File.Exists(imagePath))
			{
				MyRenderProxy.PreloadTextures(new string[1] { imagePath }, TextureType.GUIWithoutPremultiplyAlpha);
				m_thumbnailImage.SetTexture(imagePath);
				if (m_thumbnailImage.IsAnyTextureValid())
				{
					m_thumbnailImage.Visible = true;
					UpdateThumbnailPosition();
				}
			}
		}

		private void OnFocusedItem(MyGuiControlBase control, bool state)
		{
			if (state)
			{
				MyGuiControlContentButton butt = control as MyGuiControlContentButton;
				SelectButton(butt, -1, forceToTop: false, alwaysScroll: false);
			}
		}

		public void RefreshThumbnail()
		{
			m_thumbnailImage = new MyGuiControlImage();
			m_thumbnailImage.Position = new Vector2(-0.31f, -0.224f);
			m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
			m_thumbnailImage.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
			m_thumbnailImage.Visible = false;
			m_thumbnailImage.BorderEnabled = true;
			m_thumbnailImage.BorderSize = 1;
			m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
		}

		private void CheckAndSplitLongName()
		{
			if (!(m_detailName.GetTextSize().X > 0.4f))
			{
				return;
			}
			float num = 5f;
			float num2 = 1.2f;
			float num3 = 0.4f * (1f + 0.4f / num2) - 0.1f;
			while (m_detailName.GetTextSize().X > num3)
			{
				m_detailName.Text = m_detailName.Text.Remove(m_detailName.Text.Length - 1);
			}
			string text = m_detailName.Text;
			int num4 = m_detailName.Text.Length / 4 * 3;
			float num5 = (new string[2]
			{
				m_detailName.Text.Substring(0, num4 - 1),
				m_detailName.Text.Substring(num4 - 1)
			})[1].Length;
			float num6 = m_detailName.Text.Length;
			while (true)
			{
				num6 -= num5;
				m_detailName.Text = text.Insert((int)num6, "\n");
				string[] array = m_detailName.Text.Split(new char[1] { '\n' });
				float x = MyGuiManager.MeasureString(m_detailName.Font, new StringBuilder(array[0]), m_detailName.TextScaleWithLanguage).X;
				float x2 = MyGuiManager.MeasureString(m_detailName.Font, new StringBuilder(array[1]), m_detailName.TextScaleWithLanguage).X;
				float num7 = x / x2;
				if (num7 >= num && num5 < 0f)
				{
					num5 /= -2f;
				}
				else if (num7 <= num2 && num5 > 0f)
				{
					num5 /= -2f;
				}
				else if (m_detailName.GetTextSize().X <= 0.4f)
				{
					break;
				}
			}
			if (m_detailName.Text.Substring((int)num6, 2) == "\n ")
			{
				m_detailName.Text = m_detailName.Text.Remove((int)num6 + 1, 1);
			}
		}
	}
}
