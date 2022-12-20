using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Generics;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudNotifications
	{
		private class NotificationDrawData
		{
			public struct Element
			{
				public StringBuilder Text;

				public Vector2 Size;
			}

			public MyHudNotificationBase Notification;

			public Vector2 TextSize;

			public StringBuilder Text;

			public Element[] Elements;

			private string[] m_separators = new string[2] { "[", "]" };

			public bool HasFog => Notification.HasFog;

			public bool IsClear => Text == null;

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public NotificationDrawData(MyHudNotificationBase notification)
			{
				Notification = notification;
				PrepareElements();
			}

			public void PrepareElements()
			{
				if (m_textsPool == null)
				{
					m_textsPool = new MyObjectsPool<StringBuilder>(80);
				}
				if (Notification == null)
				{
					return;
				}
				string text = Notification.GetText();
				StringBuilder stringBuilder = m_textsPool.Allocate();
				if (stringBuilder == null)
				{
					return;
				}
				Text = stringBuilder.Clear().Append(text).UpdateControlsFromNotificationFriendly();
				if (Text == null || Notification.Font == null || string.IsNullOrEmpty(text))
				{
					return;
				}
				TextSize = MyGuiManager.MeasureString(Notification.Font, Text, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f);
				string[] array = text.Split(m_separators, StringSplitOptions.None);
				if (array == null || array.Length == 1)
				{
					return;
				}
				Elements = new Element[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					StringBuilder stringBuilder2 = m_textsPool.Allocate();
					if (stringBuilder2 == null)
					{
						break;
					}
					Elements[i].Text = stringBuilder2.Clear().Append(array[i]).UpdateControlsFromNotificationFriendly();
					Elements[i].Size = MyGuiManager.MeasureString(Notification.Font, Elements[i].Text, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f);
				}
			}

			public void Clear()
			{
				if (m_textsPool != null)
				{
					if (Elements != null)
					{
						for (int i = 0; i < Elements.Length; i++)
						{
							m_textsPool.Deallocate(Elements[i].Text);
						}
						Elements = null;
					}
					m_textsPool.Deallocate(Text);
				}
				else
				{
					m_textsPool = new MyObjectsPool<StringBuilder>(80);
				}
				Text = null;
			}

			internal void Update(MyHudNotificationBase notification)
			{
				Clear();
				Notification = notification;
				PrepareElements();
			}
		}

		public class ControlsHelper
		{
			private MyControl[] m_controls;

			public ControlsHelper(params MyControl[] controls)
			{
				m_controls = controls;
			}

			public override string ToString()
			{
				return string.Join(", ", Enumerable.Where<string>(Enumerable.Select<MyControl, string>((IEnumerable<MyControl>)m_controls, (Func<MyControl, string>)((MyControl s) => s.ButtonNamesIgnoreSecondary)), (Func<string, bool>)((string s) => !string.IsNullOrEmpty(s))));
			}
		}

		public const int MAX_PRIORITY = 5;

		private Predicate<NotificationDrawData> m_disappearedPredicate;

		private Dictionary<int, List<NotificationDrawData>> m_notificationsByPriority;

		private List<StringBuilder> m_texts;

		private readonly List<NotificationDrawData> m_toDraw = new List<NotificationDrawData>(8);

		private static MyObjectsPool<StringBuilder> m_textsPool;

		private HashSet<MyHudNotificationBase> m_toRemove = new HashSet<MyHudNotificationBase>();

		private HashSet<MyHudNotificationBase> m_toAdd = new HashSet<MyHudNotificationBase>();

		private object m_lockObject = new object();

		private MyHudNotificationBase[] m_singletons;

		public Vector2 Position;

		private int m_visibleCount;

		public event Action<MyNotificationSingletons> OnNotificationAdded;

		public MyHudNotificationBase Add(MyNotificationSingletons singleNotification)
		{
			Add(m_singletons[(int)singleNotification]);
			if (this.OnNotificationAdded != null)
			{
				this.OnNotificationAdded(singleNotification);
			}
			return m_singletons[(int)singleNotification];
		}

		public void Remove(MyNotificationSingletons singleNotification)
		{
			Remove(m_singletons[(int)singleNotification]);
		}

		public MyHudNotificationBase Get(MyNotificationSingletons singleNotification)
		{
			return m_singletons[(int)singleNotification];
		}

		public MyHudNotifications()
		{
			Position = MyNotificationConstants.DEFAULT_NOTIFICATION_MESSAGE_NORMALIZED_POSITION;
			m_disappearedPredicate = (NotificationDrawData x) => !x.Notification.Alive;
			m_notificationsByPriority = new Dictionary<int, List<NotificationDrawData>>();
			m_texts = new List<StringBuilder>(8);
			m_textsPool = new MyObjectsPool<StringBuilder>(80);
			m_singletons = new MyHudNotificationBase[Enum.GetValues(typeof(MyNotificationSingletons)).Length];
			Register(MyNotificationSingletons.GameOverload, new MyHudNotification(MyCommonTexts.NotificationMemoryOverload, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2));
			Register(MyNotificationSingletons.SuitEnergyLow, new MyHudNotification(MySpaceTexts.NotificationSuitEnergyLow, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2));
			Register(MyNotificationSingletons.SuitEnergyCritical, new MyHudNotification(MySpaceTexts.NotificationSuitEnergyCritical, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2));
			Register(MyNotificationSingletons.IncompleteGrid, new MyHudNotification(MyCommonTexts.NotificationIncompleteGrid, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2));
			Register(MyNotificationSingletons.DisabledWeaponsAndTools, new MyHudNotification(MyCommonTexts.NotificationToolDisabled, 0, "Red"));
			Register(MyNotificationSingletons.WeaponDisabledInWorldSettings, new MyHudNotification(MyCommonTexts.NotificationWeaponDisabledInSettings, 2500, "Red"));
			Register(MyNotificationSingletons.MultiplayerDisabled, new MyHudNotification(MyCommonTexts.NotificationMultiplayerDisabled, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 5));
			Register(MyNotificationSingletons.MissingComponent, new MyHudMissingComponentNotification(MyCommonTexts.NotificationMissingComponentToPlaceBlockFormat, 2500, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 1));
			Register(MyNotificationSingletons.WorldLoaded, new MyHudNotification(MyCommonTexts.WorldLoaded));
			Register(MyNotificationSingletons.ObstructingBlockDuringMerge, new MyHudNotification(MySpaceTexts.NotificationObstructingBlockDuringMerge, 2500, "Red"));
			Register(MyNotificationSingletons.HideHints, new MyHudNotification(MyCommonTexts.NotificationHideHintsInGameOptions, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2, MyNotificationLevel.Control));
			Register(MyNotificationSingletons.HelpHint, new MyHudNotification(MyCommonTexts.NotificationNeedShowHelpScreen, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 1, MyNotificationLevel.Control));
			Register(MyNotificationSingletons.ScreenHint, new MyHudNotification(MyCommonTexts.NotificationScreenFormat, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control));
			Register(MyNotificationSingletons.RespawnShipWarning, new MyHudNotification(MySpaceTexts.NotificationRespawnShipDelete, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.BuildingOnRespawnShipWarning, new MyHudNotification(MySpaceTexts.NotificationBuildingOnRespawnShip, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerDemotedNone, new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_None, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerDemotedScripter, new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_Scripter, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerDemotedModerator, new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_Moderator, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerDemotedSpaceMaster, new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_SpaceMaster, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerPromotedScripter, new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_Scripter, 10000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerPromotedModerator, new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_Moderator, 10000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerPromotedSpaceMaster, new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_SpaceMaster, 10000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PlayerPromotedAdmin, new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_Admin, 10000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.CopySucceeded, new MyHudNotification(MyCommonTexts.NotificationCopySucceeded, 1300, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.CopyFailed, new MyHudNotification(MyCommonTexts.NotificationCopyFailed, 1300, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.PasteFailed, new MyHudNotification(MyCommonTexts.NotificationPasteFailed, 1300, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.CutPermissionFailed, new MyHudNotification(MyCommonTexts.NotificationCutPermissionFailed, 1300, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.DeletePermissionFailed, new MyHudNotification(MyCommonTexts.NotificationDeletePermissionFailed, 1300, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.ClientCannotSave, new MyHudNotification(MyCommonTexts.NotificationClientCannotSave, 2500, "Red"));
			Register(MyNotificationSingletons.CannotSave, new MyHudNotification(MyCommonTexts.NotificationSavingDisabled, 2500, "Red"));
			Register(MyNotificationSingletons.WheelNotPlaced, new MyHudNotification(MySpaceTexts.NotificationWheelNotPlaced, 2500, "Red"));
			Register(MyNotificationSingletons.CopyPasteBlockNotAvailable, new MyHudNotification(MyCommonTexts.NotificationCopyPasteBlockNotAvailable, 2500, "Red"));
			Register(MyNotificationSingletons.CopyPasteFloatingObjectNotAvailable, new MyHudNotification(MyCommonTexts.NotificationCopyPasteFloatingObjectNotAvailable, 2500, "Red"));
			Register(MyNotificationSingletons.CopyPasteAsteoridObstructed, new MyHudNotification(MySpaceTexts.NotificationCopyPasteAsteroidObstructed, 2500, "Red"));
			Register(MyNotificationSingletons.TextPanelReadOnly, new MyHudNotification(MyCommonTexts.NotificationTextPanelReadOnly, 2500, "Red"));
			Register(MyNotificationSingletons.AccessDenied, new MyHudNotification(MyCommonTexts.AccessDenied, 2500, "Red"));
			Register(MyNotificationSingletons.AdminMenuNotAvailable, new MyHudNotification(MySpaceTexts.AdminMenuNotAvailable, 10000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.HeadNotPlaced, new MyHudNotification(MySpaceTexts.Notification_PistonHeadNotPlaced, 2500, "Red"));
			Register(MyNotificationSingletons.HeadAlreadyExists, new MyHudNotification(MySpaceTexts.Notification_PistonHeadAlreadyExists, 2500, "Red"));
			MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationLimitsGridSize, 5000, "Red");
			myHudNotification.SetTextFormatArguments(MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
			Register(MyNotificationSingletons.LimitsGridSize, myHudNotification);
			MyHudNotification myHudNotification2 = new MyHudNotification(MySpaceTexts.NotificationLimitsPerBlockType, 5000, "Red");
			myHudNotification2.SetTextFormatArguments(MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
			Register(MyNotificationSingletons.LimitsPerBlockType, myHudNotification2);
			MyHudNotification myHudNotification3 = new MyHudNotification(MySpaceTexts.NotificationLimitsPlayer, 5000, "Red");
			myHudNotification3.SetTextFormatArguments(MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
			Register(MyNotificationSingletons.LimitsPlayer, myHudNotification3);
			MyHudNotification myHudNotification4 = new MyHudNotification(MySpaceTexts.NotificationLimitsPCU, 5000, "Red");
			myHudNotification4.SetTextFormatArguments(MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
			Register(MyNotificationSingletons.LimitsPCU, myHudNotification4);
			MyHudNotification myHudNotification5 = new MyHudNotification(MySpaceTexts.NotificationLimitsNoFaction, 5000, "Red");
			myHudNotification5.SetTextFormatArguments(MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
			Register(MyNotificationSingletons.LimitsNoFaction, myHudNotification5);
			Register(MyNotificationSingletons.GridReachedPhysicalLimit, new MyHudNotification(MySpaceTexts.NotificationGridReachedPhysicalLimit, 2500, "Red"));
			Register(MyNotificationSingletons.BlockNotResearched, new MyHudNotification(MySpaceTexts.NotificationBlockNotResearched, 2500, "Red"));
			Register(MyNotificationSingletons.ManipulatingDoorFailed, new MyHudNotification(MyCommonTexts.Notification_CannotManipulateDoor, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			Register(MyNotificationSingletons.BlueprintScriptsRemoved, new MyHudNotification(MySpaceTexts.Notification_BlueprintScriptRemoved, 2500, "Red"));
			Register(MyNotificationSingletons.ConnectionProblem, new MyHudNotification(MyCommonTexts.PerformanceWarningHeading_Connection, 0, "Red"));
			Register(MyNotificationSingletons.MissingDLC, new MyHudNotification(MyCommonTexts.RequiresDlc, 2500, "Red"));
			Register(MyNotificationSingletons.BuildPlannerEmpty, new MyHudNotification(MySpaceTexts.NotificationBuildPlannerEmpty));
			Register(MyNotificationSingletons.WithdrawSuccessful, new MyHudNotification(MySpaceTexts.NotificationWithdrawSuccessful));
			Register(MyNotificationSingletons.WithdrawFailed, new MyHudNotification(MyStringId.GetOrCompute("{0}")));
			Register(MyNotificationSingletons.DepositSuccessful, new MyHudNotification(MySpaceTexts.NotificationDepositSuccessful));
			Register(MyNotificationSingletons.DepositFailed, new MyHudNotification(MySpaceTexts.NotificationDepositFailed));
			Register(MyNotificationSingletons.PutToProductionSuccessful, new MyHudNotification(MySpaceTexts.NotificationPutToProductionSuccessful));
			Register(MyNotificationSingletons.PutToProductionFailed, new MyHudNotification(MySpaceTexts.NotificationPutToProductionFailed));
			Register(MyNotificationSingletons.BuildPlannerCapacityReached, new MyHudNotification(MySpaceTexts.BuildPlannerCapacityReached));
			Register(MyNotificationSingletons.BuildPlannerComponentsAdded, new MyHudNotification(MySpaceTexts.BuildPlannerComponentsAdded));
			Register(MyNotificationSingletons.DamageTurnedOff, new MyHudNotification(MySpaceTexts.NotificationDamageTurnedOff));
			Register(MyNotificationSingletons.GridIsImmune, new MyHudNotification(MySpaceTexts.NotificationGridIsImmune));
			FormatNotifications(MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed && MyFakes.ENABLE_CONTROLLER_HINTS);
			MyInput.Static.JoystickConnected += Static_JoystickConnected;
		}

		private void Static_JoystickConnected(bool value)
		{
			FormatNotifications(value && MyFakes.ENABLE_CONTROLLER_HINTS);
		}

		public void Add(MyHudNotificationBase notification)
		{
			lock (m_lockObject)
			{
				List<NotificationDrawData> notificationGroup = GetNotificationGroup(notification.Priority);
<<<<<<< HEAD
				if (notificationGroup.All((NotificationDrawData x) => x.Notification != notification))
=======
				if (Enumerable.All<NotificationDrawData>((IEnumerable<NotificationDrawData>)notificationGroup, (Func<NotificationDrawData, bool>)((NotificationDrawData x) => x.Notification != notification)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					notification.BeforeAdd();
					notificationGroup.Add(new NotificationDrawData(notification));
				}
				notification.ResetAliveTime();
			}
		}

		public void Remove(MyHudNotificationBase notification)
		{
			if (notification == null)
			{
				return;
			}
			lock (m_lockObject)
			{
				GetNotificationGroup(notification.Priority).RemoveAll(delegate(NotificationDrawData x)
				{
					if (x.Notification == notification)
					{
						x.Notification.BeforeRemove();
						x.Clear();
						return true;
					}
					return false;
				});
			}
		}

		public void Update(MyHudNotificationBase notification)
		{
			if (notification == null)
			{
				return;
			}
			lock (m_lockObject)
			{
				GetNotificationGroup(notification.Priority).FindLast(delegate(NotificationDrawData x)
				{
					if (x.Notification == notification)
					{
						x.Update(notification);
						return true;
					}
					return false;
				});
			}
		}

		public void Clear()
		{
			MyInput.Static.JoystickConnected -= Static_JoystickConnected;
			lock (m_lockObject)
			{
				foreach (KeyValuePair<int, List<NotificationDrawData>> item in m_notificationsByPriority)
				{
					item.Value.Clear();
				}
			}
		}

		public void Update()
		{
			ProcessBeforeDraw(out m_visibleCount);
		}

		public void Draw()
		{
			DrawFog();
			DrawNotifications(m_visibleCount);
		}

		public void ReloadTexts()
		{
			FormatNotifications(MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed && MyFakes.ENABLE_CONTROLLER_HINTS);
			lock (m_lockObject)
			{
				foreach (KeyValuePair<int, List<NotificationDrawData>> item in m_notificationsByPriority)
				{
					foreach (NotificationDrawData item2 in item.Value)
					{
						item2.Notification.SetTextDirty();
						item2.Clear();
						item2.PrepareElements();
					}
				}
			}
		}

		public void UpdateBeforeSimulation()
		{
			lock (m_lockObject)
			{
				foreach (KeyValuePair<int, List<NotificationDrawData>> item in m_notificationsByPriority)
				{
					foreach (NotificationDrawData item2 in item.Value)
					{
						item2.Notification.AddAliveTime(16);
					}
				}
				foreach (KeyValuePair<int, List<NotificationDrawData>> item3 in m_notificationsByPriority)
				{
					foreach (NotificationDrawData item4 in item3.Value)
					{
						if (m_disappearedPredicate(item4))
						{
							item4.Notification.BeforeRemove();
							item4.Clear();
						}
					}
					item3.Value.RemoveAll(m_disappearedPredicate);
				}
			}
		}

		public void Register(MyNotificationSingletons singleton, MyHudNotificationBase notification)
		{
			m_singletons[(int)singleton] = notification;
		}

		public static MyHudNotification CreateControlNotification(MyStringId textId, params object[] args)
		{
			MyHudNotification myHudNotification = new MyHudNotification(textId, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
			myHudNotification.SetTextFormatArguments(args);
			return myHudNotification;
		}

		private void ProcessBeforeDraw(out int visibleCount)
		{
			m_toDraw.Clear();
			visibleCount = 0;
			lock (m_lockObject)
			{
				for (int num = 5; num >= 0; num--)
				{
					m_notificationsByPriority.TryGetValue(num, out var value);
					if (value != null)
					{
						foreach (NotificationDrawData item in value)
						{
							if (IsDrawn(item.Notification))
							{
								m_toDraw.Add(item);
								visibleCount++;
								if (visibleCount == 8)
								{
									return;
								}
							}
						}
					}
				}
			}
		}

		private void DrawFog()
		{
			Vector2 position = Position;
			for (int i = 0; i < m_toDraw.Count; i++)
			{
				NotificationDrawData notificationDrawData = m_toDraw[i];
				if (!notificationDrawData.IsClear && notificationDrawData.HasFog)
				{
					Vector2 textSize = notificationDrawData.TextSize;
					MyGuiTextShadows.DrawShadow(ref position, ref textSize);
					position.Y += textSize.Y;
				}
			}
		}

		private void DrawNotifications(int visibleCount)
		{
			lock (m_lockObject)
			{
				Vector2 position = Position;
				int num = Math.Max(visibleCount, m_toDraw.Count);
				for (int i = 0; i < num; i++)
				{
					NotificationDrawData notificationDrawData = m_toDraw[i];
					if (notificationDrawData.IsClear)
					{
						continue;
					}
					position.X = Position.X - notificationDrawData.TextSize.X / 2f;
					if (notificationDrawData.Elements != null)
					{
						bool flag = false;
						NotificationDrawData.Element[] elements = notificationDrawData.Elements;
						for (int j = 0; j < elements.Length; j++)
						{
							NotificationDrawData.Element element = elements[j];
							Color value = (flag ? Color.Yellow : Color.White);
							if (element.Text != null)
							{
								MyGuiManager.DrawString(notificationDrawData.Notification.Font, element.Text.ToString(), position, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f, value, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyVideoSettingsManager.IsTripleHead());
							}
							position.X += element.Size.X;
							flag = !flag;
						}
					}
					else
					{
						MyGuiManager.DrawString(notificationDrawData.Notification.Font, notificationDrawData.Text.ToString(), position, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyVideoSettingsManager.IsTripleHead());
					}
					position.Y += notificationDrawData.TextSize.Y;
				}
			}
		}

		private List<NotificationDrawData> GetNotificationGroup(int priority)
		{
			if (!m_notificationsByPriority.TryGetValue(priority, out var value))
			{
				value = new List<NotificationDrawData>();
				m_notificationsByPriority[priority] = value;
			}
			return value;
		}

		private bool IsDrawn(MyHudNotificationBase notification)
		{
			bool flag = notification.Alive;
			if (notification.IsControlsHint)
			{
				flag = flag && MySandboxGame.Config.ControlsHints;
			}
			if (MyHud.MinimalHud && !MyHud.CutsceneHud && notification.Level != MyNotificationLevel.Important)
			{
				flag = false;
			}
			if (MyHud.CutsceneHud && notification.Level == MyNotificationLevel.Control)
			{
				flag = false;
			}
			return flag;
		}

		private void SetNotificationTextAndArgs(MyNotificationSingletons type, MyStringId textId, params object[] args)
		{
			MyHudNotification myHudNotification = Get(type) as MyHudNotification;
			myHudNotification.Text = textId;
			myHudNotification.SetTextFormatArguments(args);
			Add(myHudNotification);
		}

		private void FormatNotifications(bool forJoystick)
		{
			if (forJoystick)
			{
				MyStringId cX_BASE = MySpaceBindingCreator.CX_BASE;
				MyStringId cX_CHARACTER = MySpaceBindingCreator.CX_CHARACTER;
				MyControllerHelper.GetCodeForControl(cX_BASE, MyControlsSpace.CONTROL_MENU);
				MyControllerHelper.GetCodeForControl(cX_CHARACTER, MyControlsSpace.TOOLBAR_NEXT_ITEM);
				MyControllerHelper.GetCodeForControl(cX_CHARACTER, MyControlsSpace.TOOLBAR_PREV_ITEM);
			}
			else
			{
				MyInput.Static.GetGameControl(MyControlsSpace.TOGGLE_HUD);
				MyInput.Static.GetGameControl(MyControlsSpace.SLOT1);
				MyInput.Static.GetGameControl(MyControlsSpace.SLOT2);
				MyInput.Static.GetGameControl(MyControlsSpace.SLOT3);
				MyInput.Static.GetGameControl(MyControlsSpace.BUILD_SCREEN);
				MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN);
				MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_COMPOUND);
			}
		}
	}
}
