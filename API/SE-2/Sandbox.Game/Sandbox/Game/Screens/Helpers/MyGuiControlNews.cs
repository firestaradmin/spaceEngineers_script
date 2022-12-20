using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
<<<<<<< HEAD
using System.Net;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage;
using VRage.Game;
using VRage.Game.News;
using VRage.Http;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlNews : MyGuiControlParent
	{
		public enum StateEnum
		{
			Entries,
			Loading,
			Error
		}

		private static StringBuilder m_stringCache = new StringBuilder(100);

		private List<MyNewsEntry> m_news;

		private int m_currentEntryIndex;

		private StateEnum m_state;

		private MyGuiControlLabel m_labelTitle;

		private MyGuiControlLabel m_labelDate;

		private MyGuiControlSeparatorList m_separator;

		private MyGuiControlMultilineText m_textNewsEntry;

		private MyGuiControlPanel m_backgroundPanel;

		private MyGuiControlPanel m_backgroundPanel_BlueLine;

		private MyGuiControlPanel m_bottomPanel;

		private MyGuiControlLabel m_labelPages;

		private MyGuiControlButton m_buttonNext;

		private MyGuiControlButton m_buttonPrev;

		private MyGuiControlMultilineText m_textError;

		private MyGuiControlRotatingWheel m_wheelLoading;

		private Task m_downloadNewsTask;

		private MyNews m_downloadedNews;

		private XmlSerializer m_newsSerializer;

		private bool m_downloadedNewsOK;

		private static readonly char[] m_trimArray = new char[4] { ' ', '\r', '\r', '\n' };

<<<<<<< HEAD
=======
		private bool m_pauseGame;

		private static readonly char[] m_trimArray = new char[4] { ' ', '\r', '\r', '\n' };

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static readonly char[] m_splitArray = new char[2] { '\r', '\n' };

		public MyNewsLink[] NewsLinks
		{
			get
			{
				if (m_news.IsValidIndex(m_currentEntryIndex))
				{
					return m_news[m_currentEntryIndex].Links;
				}
				return null;
			}
		}

		public StateEnum State
		{
			get
			{
				return m_state;
			}
			set
			{
				if (m_state != value)
				{
					m_state = value;
					RefreshState();
				}
			}
		}

		public StringBuilder ErrorText
		{
			get
			{
				return m_textError.Text;
			}
			set
			{
				m_textError.Text = value;
			}
		}

		public MyGuiControlNews()
		{
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Expected O, but got Unknown
			m_news = new List<MyNewsEntry>();
			m_labelTitle = new MyGuiControlLabel(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "Title"
			};
			m_labelDate = new MyGuiControlLabel(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "Date"
			};
			m_separator = new MyGuiControlSeparatorList
			{
				Name = "Separator",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
			};
			m_textNewsEntry = new MyGuiControlMultilineText(null, null, null, "Blue", 0.68f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "NewsEntry",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_textNewsEntry.OnLinkClicked += OnLinkClicked;
			m_textNewsEntry.CanHaveFocus = true;
			m_textNewsEntry.BorderEnabled = true;
			m_bottomPanel = new MyGuiControlPanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				ColorMask = new Vector4(1f, 1f, 1f, 0f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_PAGING_BACKGROUND,
				Name = "BottomPanel"
			};
			m_labelPages = new MyGuiControlLabel(null, null, new StringBuilder("{0}/{1}  ").ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM)
			{
				Name = "Pages"
			};
			m_buttonPrev = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ArrowLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, MyTexts.GetString(MyCommonTexts.PreviousNews), null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
			{
				UpdateCurrentEntryIndex(-1);
			})
			{
				Name = "Previous"
			};
			m_buttonPrev.GamepadHelpTextId = MySpaceTexts.NewsControl_Help_Next;
			m_buttonNext = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ArrowRight, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, MyTexts.GetString(MyCommonTexts.NextNews), null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
			{
				UpdateCurrentEntryIndex(1);
			})
			{
				Name = "Next"
			};
			m_buttonNext.GamepadHelpTextId = MySpaceTexts.NewsControl_Help_Previous;
			m_textError = new MyGuiControlMultilineText(null, null, null, "Red", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Name = "Error"
			};
			m_backgroundPanel = new MyGuiControlCompositePanel
			{
				ColorMask = new Vector4(1f, 1f, 1f, 0.8f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND
			};
			m_backgroundPanel_BlueLine = new MyGuiControlCompositePanel
			{
				ColorMask = new Vector4(1f, 1f, 1f, 1f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND_BlueLine
			};
			m_wheelLoading = new MyGuiControlRotatingWheel(null, null, 0.36f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, "Textures\\GUI\\screens\\screen_loading_wheel.dds", manualRotationUpdate: true, MyPerGameSettings.GUI.MultipleSpinningWheels);
			Elements.Add(m_backgroundPanel);
			Elements.Add(m_backgroundPanel_BlueLine);
			Elements.Add(m_labelTitle);
			Elements.Add(m_labelDate);
			Elements.Add(m_separator);
			Elements.Add(m_bottomPanel);
			Elements.Add(m_labelPages);
			Elements.Add(m_textError);
			Elements.Add(m_wheelLoading);
			base.Controls.Add(m_textNewsEntry);
			base.Controls.Add(m_buttonPrev);
			base.Controls.Add(m_buttonNext);
			RefreshState();
			UpdatePositionsAndSizes();
			RefreshShownEntry();
			try
			{
				m_newsSerializer = new XmlSerializer(typeof(MyNews));
			}
			finally
			{
				if (MyGameService.UserId != 0L)
				{
					DownloadNews();
				}
			}
			base.CanFocusChildren = true;
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
		}

		protected override void OnSizeChanged()
		{
			UpdatePositionsAndSizes();
			base.OnSizeChanged();
		}

		public override MyGuiControlBase HandleInput()
		{
			base.HandleInput();
			return HandleInputElements();
		}

		private void UpdatePositionsAndSizes()
		{
			float num = 0.03f;
			float num2 = 0.004f;
			float num3 = -0.5f * base.Size.Y + num;
			float num4 = -0.5f * base.Size.X + num;
			float num5 = 0.5f * base.Size.X - num;
			m_labelTitle.Position = new Vector2(num4, num3);
			m_labelDate.Position = new Vector2(num5, num3);
			num3 += Math.Max(m_labelTitle.Size.Y, m_labelDate.Size.Y) + num2;
			m_separator.Size = base.Size;
			m_separator.Clear();
			num3 += num2;
			m_textNewsEntry.Position = new Vector2(num4, num3);
			m_buttonPrev.Position = new Vector2(m_textNewsEntry.Position.X + 0.02f, 0.5f * base.Size.Y - 0.5f * num);
			m_labelPages.Position = new Vector2(m_buttonPrev.Position.X + 8.9f * num2, m_buttonPrev.Position.Y - 0.003f);
			m_buttonNext.Position = new Vector2(m_buttonPrev.Position.X + 20f * num2, m_buttonPrev.Position.Y);
			m_textNewsEntry.Size = new Vector2(num5 - num4 + 0.013f, m_buttonNext.Position.Y - m_textNewsEntry.Position.Y - num);
			m_textError.Size = base.Size - 2f * num;
			m_bottomPanel.Size = new Vector2(0.125f, m_buttonPrev.Size.Y + 0.015f);
			m_backgroundPanel.Size = base.Size;
			m_backgroundPanel_BlueLine.Size = base.Size;
			m_backgroundPanel_BlueLine.Position = new Vector2(base.Size.X - num2, 0f);
		}

		internal void Show(MyNews news)
		{
			m_news.Clear();
			m_news.AddRange(news.Entry);
			m_currentEntryIndex = 0;
			RefreshShownEntry();
		}

		private void UpdateCurrentEntryIndex(int delta)
		{
			m_currentEntryIndex += delta;
			if (m_currentEntryIndex < 0)
			{
				m_currentEntryIndex = 0;
			}
			if (m_currentEntryIndex >= m_news.Count)
			{
				m_currentEntryIndex = m_news.Count - 1;
			}
			RefreshShownEntry();
		}

		private void RefreshShownEntry()
		{
			m_textNewsEntry.Clear();
			if (m_downloadedNewsOK && m_news.IsValidIndex(m_currentEntryIndex))
			{
				MyNewsEntry myNewsEntry = m_news[m_currentEntryIndex];
				m_labelTitle.Text = myNewsEntry.Title;
				string[] array = myNewsEntry.Date.Split(new char[1] { '/' });
				if (array[1].Length == 1)
				{
					m_labelDate.Text = array[0] + "/0" + array[1] + "/" + array[2];
				}
				else
				{
					m_labelDate.Text = array[0] + "/" + array[1] + "/" + array[2];
				}
				MyWikiMarkupParser.ParseText(myNewsEntry.Text, ref m_textNewsEntry);
				m_textNewsEntry.AppendLine();
				m_labelPages.UpdateFormatParams(m_currentEntryIndex + 1, m_news.Count);
				m_buttonNext.Enabled = ((m_currentEntryIndex + 1 != m_news.Count) ? true : false);
				m_buttonPrev.Enabled = ((m_currentEntryIndex + 1 != 1) ? true : false);
			}
			else
			{
				m_labelTitle.Text = null;
				m_labelDate.Text = null;
				m_labelPages.UpdateFormatParams(0, 0);
			}
		}

		private void RefreshState()
		{
			bool visible = m_state == StateEnum.Entries;
			bool visible2 = m_state == StateEnum.Error;
			bool visible3 = m_state == StateEnum.Loading;
			m_labelTitle.Visible = visible;
			m_labelDate.Visible = visible;
			m_separator.Visible = visible;
			m_textNewsEntry.Visible = visible;
			m_labelPages.Visible = visible;
			m_bottomPanel.Visible = visible;
			m_buttonPrev.Visible = visible;
			m_buttonNext.Visible = visible;
			m_textError.Visible = visible2;
			m_wheelLoading.Visible = visible3;
		}

		public void DownloadNews()
		{
			if (m_downloadNewsTask == null || m_downloadNewsTask.IsCompleted)
			{
				State = StateEnum.Loading;
				m_downloadNewsTask = Task.Run(delegate
				{
					DownloadNewsAsync();
				}).ContinueWith(delegate
				{
					MySandboxGame.Static.Invoke(DownloadNewsCompleted, "DownloadNewsCompleted");
				});
			}
		}

		private void DownloadNewsCompleted()
		{
			CheckVersion();
			if (m_downloadedNewsOK)
			{
				State = StateEnum.Entries;
				Show(m_downloadedNews);
			}
			else
			{
				State = StateEnum.Error;
				ErrorText = MyTexts.Get(MyCommonTexts.NewsDownloadingFailed);
			}
		}

		private void DownloadNewsAsync()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			try
			{
				string url = string.Format(MyPerGameSettings.ChangeLogUrl, MySandboxGame.Config.Language.ToString());
<<<<<<< HEAD
				if (MyVRage.Platform.Http.SendRequest(url, null, HttpMethod.GET, out var content) != HttpStatusCode.OK)
=======
				if ((int)MyVRage.Platform.Http.SendRequest(url, null, HttpMethod.GET, out var content) != 200)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return;
				}
				try
				{
<<<<<<< HEAD
					using (StringReader textReader = new StringReader(content))
					{
						m_downloadedNews = (MyNews)m_newsSerializer.Deserialize(textReader);
						m_downloadedNews.Entry = m_downloadedNews.Entry.Where((MyNewsEntry x) => x.Public).ToList();
=======
					StringReader val = new StringReader(content);
					try
					{
						m_downloadedNews = (MyNews)m_newsSerializer.Deserialize((TextReader)(object)val);
						m_downloadedNews.Entry = Enumerable.ToList<MyNewsEntry>(Enumerable.Where<MyNewsEntry>((IEnumerable<MyNewsEntry>)m_downloadedNews.Entry, (Func<MyNewsEntry, bool>)((MyNewsEntry x) => x.Public)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < m_downloadedNews.Entry.Count; i++)
						{
							MyNewsEntry myNewsEntry = m_downloadedNews.Entry[i];
							string[] array = myNewsEntry.Text.Trim(m_trimArray).Split(m_splitArray);
							stringBuilder.Clear();
							string[] array2 = array;
							for (int j = 0; j < array2.Length; j++)
							{
								string value = array2[j].Trim();
								stringBuilder.AppendLine(value);
							}
							m_downloadedNews.Entry[i] = new MyNewsEntry
							{
								Title = myNewsEntry.Title,
								Version = myNewsEntry.Version,
								Date = myNewsEntry.Date,
								Text = stringBuilder.ToString(),
								Links = myNewsEntry.Links,
								Public = myNewsEntry.Public
							};
<<<<<<< HEAD
						}
						if (MyFakes.TEST_NEWS)
						{
							MyNewsEntry myNewsEntry2 = m_downloadedNews.Entry[m_downloadedNews.Entry.Count - 1];
							myNewsEntry2.Title = "Test";
							base.ColorMask = new Vector4(1f, 1f, 1f, 0f);
							myNewsEntry2.Text = "ASDF\nASDF\n[www.spaceengineersgame.com Space engineers web]\n[[File:Textures\\GUI\\MouseCursor.dds|64x64px]]\n";
							m_downloadedNews.Entry.Add(myNewsEntry2);
						}
						m_downloadedNewsOK = true;
=======
						}
						if (MyFakes.TEST_NEWS)
						{
							MyNewsEntry myNewsEntry2 = m_downloadedNews.Entry[m_downloadedNews.Entry.Count - 1];
							myNewsEntry2.Title = "Test";
							base.ColorMask = new Vector4(1f, 1f, 1f, 0f);
							myNewsEntry2.Text = "ASDF\nASDF\n[www.spaceengineersgame.com Space engineers web]\n[[File:Textures\\GUI\\MouseCursor.dds|64x64px]]\n";
							m_downloadedNews.Entry.Add(myNewsEntry2);
						}
						m_downloadedNewsOK = true;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
<<<<<<< HEAD
=======
				finally
				{
					m_downloadedNewsFinished = true;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex2)
			{
				MyLog.Default.WriteLine("Error while downloading news: " + ex2.ToString());
<<<<<<< HEAD
=======
				m_downloadedNewsFinished = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void CheckVersion()
		{
			int result = 0;
			if (m_downloadedNews != null && m_downloadedNews.Entry.Count > 0 && int.TryParse(m_downloadedNews.Entry[0].Version, out result) && result > (int)MyFinalBuildConstants.APP_VERSION && MySandboxGame.Config.LastCheckedVersion != (int)MyFinalBuildConstants.APP_VERSION)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.NewVersionAvailable), MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: true, null, useOpacity: true, null, focusable: true, canBeHidden: true));
				MySandboxGame.Config.LastCheckedVersion = MyFinalBuildConstants.APP_VERSION;
				MySandboxGame.Config.Save();
				MyVRage.Platform.System.ResetColdStartRegister();
			}
		}

		public void CloseNewVersionScreen()
		{
			foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox;
				if ((myGuiScreenMessageBox = screen as MyGuiScreenMessageBox) != null && myGuiScreenMessageBox.MessageText == MyTexts.Get(MySpaceTexts.NewVersionAvailable))
				{
					myGuiScreenMessageBox.CloseScreen();
				}
			}
		}
	}
}
