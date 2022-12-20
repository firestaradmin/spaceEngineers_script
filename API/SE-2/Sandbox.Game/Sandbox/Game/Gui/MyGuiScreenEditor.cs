using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Network;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyGuiScreenEditor : MyGuiScreenBase
	{
		protected sealed class CompileProgramServer_003C_003ESystem_Byte_003C_0023_003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in byte[] program, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CompileProgramServer(program);
			}
		}

		protected sealed class ReportCompilationResults_003C_003ESystem_Boolean_0023System_Collections_Generic_List_00601_003CSystem_String_003E : ICallSite<IMyEventOwner, bool, List<string>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool success, in List<string> messages, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReportCompilationResults(success, messages);
			}
		}

		private const string CODE_WRAPPER_BEFORE = "using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\nusing VRage.Game.ModAPI.Ingame;\nusing SpaceEngineers.Game.ModAPI.Ingame;\npublic class Program: MyGridProgram\n{\n";

		private const string CODE_WRAPPER_AFTER = "\n}";

		private Action<ResultEnum> m_resultCallback;

		private Action m_saveCodeCallback;

		private string m_description = "";

		private ResultEnum m_screenResult = ResultEnum.CANCEL;

		public const int MAX_NUMBER_CHARACTERS = 100000;

		private List<string> m_compilerErrors = new List<string>();

		private MyGuiControlMultilineText m_descriptionBox;

		private MyGuiControlCompositePanel m_descriptionBackgroundPanel;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_openWorkshopButton;

		private MyGuiControlButton m_checkCodeButton;

		private MyGuiControlButton m_help;

		private MyGuiControlLabel m_lineCounter;

		private MyGuiControlLabel m_TextTooLongMessage;

		private MyGuiControlLabel m_LetterCounter;

		private MyGuiControlMultilineEditableText m_editorWindow;

		private MyGuiScreenProgress m_progress;

		public MyGuiControlMultilineText Description => m_descriptionBox;

		public MyGuiScreenEditor(string description, Action<ResultEnum> resultCallback, Action saveCodeCallback)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1f, 0.9f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_description = description;
			m_saveCodeCallback = saveCodeCallback;
			m_resultCallback = resultCallback;
			base.CanBeHidden = true;
			base.CanHideOthers = true;
			m_closeOnEsc = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenEditor";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MySpaceTexts.ProgrammableBlock_CodeEditor_Title, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.905f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.905f);
			Vector2 start = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.905f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f);
			myGuiControlSeparatorList.AddHorizontal(start, m_size.Value.X * 0.905f);
			Controls.Add(myGuiControlSeparatorList);
			m_okButton = new MyGuiControlButton(new Vector2(-0.184f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OkButtonClicked, toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_CodeEditor_SaveExit_Tooltip));
			Controls.Add(m_okButton);
			m_checkCodeButton = new MyGuiControlButton(new Vector2(-0.001f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CheckCode), toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CheckCode_Tooltip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: CheckCodeButtonClicked);
			Controls.Add(m_checkCodeButton);
			m_help = new MyGuiControlButton(new Vector2(0.182f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_Help), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, HelpButtonClicked);
			m_help.SetToolTip(MySpaceTexts.ProgrammableBlock_Editor_HelpTooltip);
			Controls.Add(m_help);
			m_openWorkshopButton = new MyGuiControlButton(new Vector2(0.365f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.ProgrammableBlock_Editor_BrowseScripts), toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_BrowseWorkshop_Tooltip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OpenWorkshopButtonClicked);
			Controls.Add(m_openWorkshopButton);
			m_descriptionBackgroundPanel = new MyGuiControlCompositePanel();
			m_descriptionBackgroundPanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
			m_descriptionBackgroundPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_descriptionBackgroundPanel.Position = new Vector2(-0.451f, -0.356f);
			m_descriptionBackgroundPanel.Size = new Vector2(0.902f, 0.664f);
			Controls.Add(m_descriptionBackgroundPanel);
			m_descriptionBox = AddMultilineText(offset: new Vector2(-0.446f, -0.356f), size: new Vector2(0.5f, 0.44f));
			m_descriptionBox.TextPadding = new MyGuiBorderThickness(0.012f, 0f, 0f, 0f);
			m_descriptionBox.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_descriptionBox.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_descriptionBox.Text = new StringBuilder(m_description);
			m_descriptionBox.Position = Vector2.Zero;
			m_descriptionBox.Size = m_descriptionBackgroundPanel.Size - new Vector2(0f, 0.03f);
			m_descriptionBox.Position = new Vector2(0f, -0.024f);
			m_lineCounter = new MyGuiControlLabel(new Vector2(-0.45f, 0.357f), null, string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), 1, m_editorWindow.GetTotalNumLines()), null, 0.8f, "White");
			Elements.Add(m_lineCounter);
			m_LetterCounter = new MyGuiControlLabel(new Vector2(-0.45f, -0.397f), null, null, null, 0.8f, "White");
			Elements.Add(m_LetterCounter);
			Vector2 value = m_LetterCounter.Position - new Vector2(0f, m_LetterCounter.Size.Y);
			m_TextTooLongMessage = new MyGuiControlLabel(value, null, null, null, 0.8f, "Red");
			Elements.Add(m_TextTooLongMessage);
			_ = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_TextTooLongMessage.PositionX, m_lineCounter.PositionY));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.PbEditor_Help_Screen;
			base.FocusedControl = m_descriptionBox;
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
			}
		}

		protected MyGuiControlMultilineText AddMultilineText(Vector2? size = null, Vector2? offset = null, float textScale = 1f, bool selectable = false, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2 vector = size ?? base.Size ?? new Vector2(1.2f, 0.5f);
			MyGuiControlMultilineEditableText myGuiControlMultilineEditableText = new MyGuiControlMultilineEditableText(vector / 2f + (offset ?? Vector2.Zero), vector, Color.White.ToVector4(), "White", 0.8f, textAlign, null, drawScrollbarV: true, drawScrollbarH: true, textBoxAlign);
			myGuiControlMultilineEditableText.IgnoreOffensiveText = true;
			if (MyPlatformGameSettings.IsMultilineEditableByGamepad)
			{
				myGuiControlMultilineEditableText.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_MultiLineTextbox;
			}
			m_editorWindow = myGuiControlMultilineEditableText;
			Controls.Add(myGuiControlMultilineEditableText);
			return myGuiControlMultilineEditableText;
		}

		public bool TextTooLong()
		{
			return m_editorWindow.Text.Length > 100000;
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			CallResultCallback(m_screenResult);
			return base.CloseScreen(isUnloading);
		}

		public void SetDescription(string desc)
		{
			m_description = desc;
			m_descriptionBox.Clear();
			m_descriptionBox.Text = new StringBuilder(m_description);
		}

		public void AppendTextToDescription(string text, Vector4 color, string font = "White", float scale = 1f)
		{
			m_description += text;
			m_descriptionBox.AppendText(text, font, scale, color);
		}

		public void AppendTextToDescription(string text, string font = "White", float scale = 1f)
		{
			m_description += text;
			m_descriptionBox.AppendText(text, font, scale, Vector4.One);
		}

		private void HelpButtonClicked(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(MySteamConstants.URL_BROWSE_WORKSHOP_INGAMESCRIPTS_HELP, "Steam Workshop");
		}

		private void OkButtonClicked(MyGuiControlButton button)
		{
			m_screenResult = ResultEnum.OK;
			CloseScreen();
		}

		private void OpenWorkshopButtonClicked(MyGuiControlButton button)
		{
			m_openWorkshopButton.Enabled = false;
			m_checkCodeButton.Enabled = false;
			m_editorWindow.Enabled = false;
			m_okButton.Enabled = false;
			HideScreen();
			MyBlueprintUtils.OpenScriptScreen(ScriptSelected, GetCode, WorkshopWindowClosed);
		}

		private void OpenWorkshopButtonClicked()
		{
			OpenWorkshopButtonClicked(null);
		}

		private string GetCode()
		{
			return m_descriptionBox.Text.ToString();
		}

		private void WorkshopWindowClosed()
		{
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
			}
			UnhideScreen();
			base.FocusedControl = m_descriptionBox;
			m_openWorkshopButton.Enabled = true;
			m_checkCodeButton.Enabled = true;
			m_editorWindow.Enabled = true;
			m_okButton.Enabled = true;
		}

		private void ScriptSelected(string scriptPath)
		{
			string text = null;
			string extension = Path.GetExtension(scriptPath);
			if (extension == ".cs" && File.Exists(scriptPath))
			{
				text = File.ReadAllText(scriptPath);
			}
			else if (extension == ".bin")
			{
				foreach (string file in MyFileSystem.GetFiles(scriptPath, ".cs", MySearchOption.AllDirectories))
				{
					if (MyFileSystem.FileExists(file))
					{
<<<<<<< HEAD
						using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(file)))
						{
							text = streamReader.ReadToEnd();
						}
=======
						using StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(file));
						text = streamReader.ReadToEnd();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			else if (MyFileSystem.IsDirectory(scriptPath))
			{
				foreach (string file2 in MyFileSystem.GetFiles(scriptPath, "*.cs", MySearchOption.AllDirectories))
				{
					if (MyFileSystem.FileExists(file2))
					{
						using (StreamReader streamReader2 = new StreamReader(MyFileSystem.OpenRead(file2)))
						{
							text = streamReader2.ReadToEnd();
						}
						break;
					}
				}
			}
			else if (File.Exists(scriptPath))
			{
				try
				{
<<<<<<< HEAD
					using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(scriptPath))
					{
						foreach (ZipArchiveEntry file3 in myZipArchive.Files)
						{
							if (Path.GetExtension(file3.Name).ToLower() == ".cs")
							{
								using (StreamReader streamReader3 = new StreamReader(file3.Open()))
								{
									text = streamReader3.ReadToEnd();
								}
								break;
							}
=======
					using MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(scriptPath);
					foreach (ZipArchiveEntry file3 in myZipArchive.Files)
					{
						if (Path.GetExtension(file3.Name).ToLower() == ".cs")
						{
							using (StreamReader streamReader3 = new StreamReader(file3.Open()))
							{
								text = streamReader3.ReadToEnd();
							}
							break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
			if (text != null)
			{
				SetDescription(Regex.Replace(text, "\r\n", " \n"));
				m_lineCounter.Text = string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), m_editorWindow.GetCurrentCarriageLine(), m_editorWindow.GetTotalNumLines());
				m_openWorkshopButton.Enabled = true;
				m_checkCodeButton.Enabled = true;
				m_editorWindow.Enabled = true;
				m_okButton.Enabled = true;
			}
		}

		private void CheckCodeButtonClicked(MyGuiControlButton button)
		{
			string text = Description.Text.ToString();
			m_compilerErrors.Clear();
			if (MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
			{
				bool success = CompileProgram(text, m_compilerErrors);
				ProcessCompilationResults(success);
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CompileProgramServer, StringCompressor.CompressString(text));
			m_progress = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CheckingCode));
			MyScreenManager.AddScreen(m_progress);
		}

		private void ProcessCompilationResults(bool success)
		{
			if (success)
			{
				if (m_compilerErrors.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string compilerError in m_compilerErrors)
					{
						stringBuilder.Append(compilerError);
						stringBuilder.Append('\n');
					}
					MyScreenManager.AddScreen(new MyGuiScreenEditorError(stringBuilder.ToString()));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeEditor_Title), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CompilationOk)));
				}
			}
			else
			{
				MyScreenManager.AddScreen(new MyGuiScreenEditorError(string.Join("\n", m_compilerErrors)));
			}
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
			}
			base.FocusedControl = m_descriptionBox;
		}

		public static bool CompileProgram(string program, List<string> errors)
		{
			if (!MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
			{
				errors.Add(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_NotSupported));
				return false;
			}
			if (!string.IsNullOrEmpty(program))
			{
				List<Message> diagnostics;
				Assembly result = MyVRage.Platform.Scripting.CompileIngameScriptAsync(Path.Combine(MyFileSystem.UserDataPath, "EditorCode.dll"), program, out diagnostics, "PB Code editor", "Program", typeof(MyGridProgram).Name).Result;
				errors.Clear();
<<<<<<< HEAD
				errors.AddRange(from m in diagnostics
					orderby m.IsError ? 1 : 0 descending
					select m.Text);
=======
				errors.AddRange(Enumerable.Select<Message, string>((IEnumerable<Message>)Enumerable.OrderByDescending<Message, int>((IEnumerable<Message>)diagnostics, (Func<Message, int>)((Message m) => m.IsError ? 1 : 0)), (Func<Message, string>)((Message m) => m.Text)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result != null;
			}
			return false;
		}

<<<<<<< HEAD
		[Event(null, 434)]
=======
		[Event(null, 432)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void CompileProgramServer(byte[] program)
		{
			List<string> list = new List<string>();
			bool arg = CompileProgram(StringCompressor.DecompressString(program), list);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReportCompilationResults, arg, list, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 443)]
=======
		[Event(null, 441)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void ReportCompilationResults(bool success, List<string> messages)
		{
<<<<<<< HEAD
			MyGuiScreenEditor myGuiScreenEditor = MyScreenManager.Screens.OfType<MyGuiScreenEditor>().FirstOrDefault();
=======
			MyGuiScreenEditor myGuiScreenEditor = Enumerable.FirstOrDefault<MyGuiScreenEditor>(Enumerable.OfType<MyGuiScreenEditor>((IEnumerable)MyScreenManager.Screens));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myGuiScreenEditor != null)
			{
				myGuiScreenEditor.m_compilerErrors.AddRange(messages);
				myGuiScreenEditor.ProcessCompilationResults(success);
				MyScreenManager.RemoveScreen(myGuiScreenEditor.m_progress);
				myGuiScreenEditor.m_progress = null;
			}
		}

		private string FormatError(string error)
		{
			try
			{
				char[] separator = new char[4] { ':', ')', '(', ',' };
				string[] array = error.Split(separator);
				if (array.Length > 2)
				{
					int num = Convert.ToInt32(array[2]) - m_editorWindow.MeasureNumLines("using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\nusing VRage.Game.ModAPI.Ingame;\nusing SpaceEngineers.Game.ModAPI.Ingame;\npublic class Program: MyGridProgram\n{\n");
					string text = array[6];
					for (int i = 7; i < array.Length; i++)
					{
						if (!string.IsNullOrWhiteSpace(array[i]))
						{
							text = text + "," + array[i];
						}
					}
					return string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CompilationFailedErrorFormat), num, text);
				}
				return error;
			}
			catch (Exception)
			{
				return error;
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (hasFocus && m_editorWindow.CarriageMoved())
			{
				m_lineCounter.Text = string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), m_editorWindow.GetCurrentCarriageLine(), m_editorWindow.GetTotalNumLines());
			}
			if (hasFocus)
			{
				m_LetterCounter.Text = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CharacterLimit) + " " + $"{m_editorWindow.Text.Length} / {100000}";
				if (TextTooLong())
				{
					m_LetterCounter.Font = "Red";
				}
				else
				{
					m_LetterCounter.Font = "White";
				}
				m_TextTooLongMessage.Text = (TextTooLong() ? MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong) : "");
			}
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_checkCodeButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_help.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_openWorkshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
<<<<<<< HEAD
			if (m_gamepadHelpLabel != null)
			{
				m_gamepadHelpLabel.Position = m_lineCounter.Position + new Vector2(m_lineCounter.Size.X, 0f);
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				OkButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				CheckCodeButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				HelpButtonClicked(null);
			}
			if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON).IsNewReleased())
			{
				OpenWorkshopButtonClicked(null);
			}
		}

		protected override void Canceling()
		{
			base.Canceling();
			m_screenResult = ResultEnum.CANCEL;
		}

		protected void CallResultCallback(ResultEnum result)
		{
			if (m_resultCallback != null)
			{
				m_resultCallback(result);
			}
		}
	}
}
