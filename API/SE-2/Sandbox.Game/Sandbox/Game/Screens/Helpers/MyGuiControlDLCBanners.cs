using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using LitJson;
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Http;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlDLCBanners : MyGuiControlParent
	{
		private enum MyBannerStatus
		{
			Offline,
			Installed,
			NotInstalled
		}

		private class MyBanner
		{
			public bool Enabled;

			public MyBannerStatus Status;

			public uint PackageID;

			public string PackageURL;

			public string Image;

			public string HighlightImage;

			public string CaptionLine1;

			public string CaptionLine2;

			public string Tooltip;

			public string CustomData;
		}

		private class MyBannerResponse
		{
			public string Status;

			public string Version;

			public string Language;

			public string Platform;

			public double CycleInterval;

			public double FadeDuration;

			public List<MyBanner> Data = new List<MyBanner>();

			public int NotInstalledCount => Enumerable.Count<MyBanner>((IEnumerable<MyBanner>)Data, (Func<MyBanner, bool>)((MyBanner x) => x.Status != MyBannerStatus.Installed && x.Enabled));

			public MyBanner this[int index]
			{
				get
				{
					if (index < 0 && index >= NotInstalledCount)
					{
						throw new IndexOutOfRangeException();
					}
					for (int i = 0; i < Data.Count; i++)
					{
						if (Data[i].Enabled && Data[i].Status != MyBannerStatus.Installed)
						{
							if (index == 0)
							{
								return Data[i];
							}
							index--;
						}
					}
					return null;
				}
			}

			public int IndexOf(MyBanner data)
			{
				for (int i = 0; i < NotInstalledCount; i++)
				{
					if (this[i] == data)
					{
						return i;
					}
				}
				return -1;
			}
		}

		private class MyImageDownloadTaskData : WorkData
		{
			public Dictionary<string, string> ImagesToTest = new Dictionary<string, string>();
		}

		private const string PROMO_URL = "https://crashlogs.keenswh.com/api/promotions?format_version=1.0&platform={0}&language={1}&game={2}&game_version={3}";

		public static readonly Vector4 Transparency = new Vector4(0.65f);

<<<<<<< HEAD
		private readonly string ZIP_EXTENSION = ".zip";

		private readonly string ZIP_API_PARAMETER = "/zip";

		private readonly string DDS_EXTENSION = ".dds";

		private MyGuiControlImageButton m_image;

		private MyGuiControlButton m_button;

		private MyGuiControlLabel m_firstLineText;

=======
		private MyGuiControlImageButton m_image;

		private MyGuiControlButton m_button;

		private MyGuiControlLabel m_firstLineText;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyGuiControlLabel m_secondLineText;

		private float m_cycleInterval = 5f;

		private float m_fadeDuration = 0.6f;

		private float m_timeTillNextDLC = 5f;

		private float m_transition;

		private bool m_isTransitioning;

		private MyGuiControlImageButton m_oldImage;

		private static MyBannerResponse m_cachedData = null;

		private MyGuiControlCompositePanel m_backgroundPanel;

		private MyGuiControlCompositePanel m_backgroundPanel_BlueLine;

		private MyGuiControlButton m_buttonNext;

		private MyGuiControlButton m_buttonPrev;

		public MyGuiControlDLCBanners()
		{
			m_backgroundPanel = new MyGuiControlCompositePanel
			{
				ColorMask = new Vector4(1f, 1f, 1f, 0.8f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND
			};
			base.Controls.Add(m_backgroundPanel);
			m_backgroundPanel_BlueLine = new MyGuiControlCompositePanel
			{
				ColorMask = new Vector4(1f, 1f, 1f, 1f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND_BlueLine
			};
			base.Controls.Add(m_backgroundPanel_BlueLine);
			m_buttonPrev = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnPrevButtonClicked)
			{
				Name = "Previous",
				Icon = MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE,
				IconRotation = 3.141593f
			};
			m_buttonPrev.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Previous;
			m_buttonNext = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnNextButtonClicked)
			{
				Name = "Next",
				Icon = MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE
			};
			m_buttonNext.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Next;
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture()
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture()
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture()
				}
			};
			m_image = new MyGuiControlImageButton("Button", null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnImageClicked);
			m_image.CanHaveFocus = false;
			m_image.BackgroundTexture = null;
			m_image.ApplyStyle(style);
			m_image.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Open;
			base.Controls.Add(m_image);
			m_oldImage = new MyGuiControlImageButton("Button", null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnImageClicked);
			m_oldImage.CanHaveFocus = false;
			m_oldImage.BackgroundTexture = null;
			m_oldImage.Alpha = 0f;
			m_oldImage.ApplyStyle(style);
			m_oldImage.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Open;
			base.Controls.Add(m_oldImage);
			m_button = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.StripeLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnLabelClicked);
			m_button.VisualStyle = MyGuiControlButtonStyleEnum.UrlTextNoLineBanner;
			m_button.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_button.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Open;
			base.Controls.Add(m_button);
			m_firstLineText = new MyGuiControlLabel(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
			m_secondLineText = new MyGuiControlLabel(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
			m_button.FocusChanged += delegate
			{
				UpdateLabelColorsFocus();
			};
			m_button.HighlightChanged += delegate
			{
				UpdateLabelColorsFocus();
			};
			base.Controls.Add(m_firstLineText);
			base.Controls.Add(m_secondLineText);
			base.Controls.Add(m_buttonPrev);
			base.Controls.Add(m_buttonNext);
			RequestData();
			ResizeElements();
		}

		private void UpdateLabelColorsFocus()
		{
			if (m_button.HasFocus && !m_button.HasHighlight)
			{
				m_firstLineText.ColorMask = MyGuiConstants.FOCUS_TEXT_COLOR;
				m_secondLineText.ColorMask = MyGuiConstants.FOCUS_TEXT_COLOR;
			}
			else
			{
				m_firstLineText.ColorMask = Vector4.One;
				m_secondLineText.ColorMask = Vector4.One;
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			ResizeElements();
		}

		private void ResizeElements()
		{
			m_backgroundPanel.Size = base.Size;
			m_backgroundPanel_BlueLine.Size = base.Size;
			m_backgroundPanel_BlueLine.Position = new Vector2(base.Size.X - 0.004f, 0f);
			Vector2 size = base.Size - new Vector2(0.004f, 0.052f);
			m_image.Size = size;
			m_image.Position = new Vector2(-0.5f, -0.5f) * base.Size;
			float num = 0.052f;
			float num2 = 0.4f;
			m_buttonPrev.Size = new Vector2(num2 * num, num);
			m_buttonNext.Size = new Vector2(num2 * num, num);
			m_buttonPrev.IconScale = 1f / num2;
			m_buttonNext.IconScale = 1f / num2;
			m_oldImage.Size = size;
			m_oldImage.Position = new Vector2(-0.5f, -0.5f) * base.Size;
			m_button.Size = new Vector2(base.Size.X - 2f * m_buttonPrev.Size.X - 0.002f, num);
			m_button.Position = new Vector2(-0.0015f, 0.5f * base.Size.Y);
			float num3 = 0.1875f;
			float num4 = 0.001f;
			float y = 0.0025f;
			m_buttonPrev.Position = m_button.Position + new Vector2(0f - num3 + num4, y);
			m_buttonNext.Position = m_button.Position + new Vector2(num3 + num4, y);
			m_buttonPrev.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			m_buttonNext.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			float num5 = -0.003f;
			m_firstLineText.Position = m_button.Position + new Vector2(0f, num5 - 0.5f * m_button.Size.Y);
			m_secondLineText.Position = m_button.Position + new Vector2(0f, num5);
			m_buttonPrev.ColorMask = Transparency;
			m_buttonNext.ColorMask = Transparency;
			m_button.ColorMask = Transparency;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (m_isTransitioning)
			{
				m_transition += 0.0166666675f;
				if (m_transition >= m_fadeDuration)
				{
					m_transition = m_fadeDuration;
					m_isTransitioning = false;
				}
				float num = m_transition / m_fadeDuration;
				num = 1f - num;
				num *= num;
				num = 1f - num;
				m_oldImage.Alpha = 1f - num;
				m_image.Alpha = num;
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public override void Update()
		{
			base.Update();
			if (m_cachedData != null && m_cachedData.NotInstalledCount > 1)
			{
				if (!m_image.HasHighlight)
				{
					m_timeTillNextDLC -= 0.0166666675f;
				}
				if (m_timeTillNextDLC <= 0f)
				{
					m_timeTillNextDLC = m_cycleInterval;
					OnNextButtonClicked(null);
				}
			}
		}

		public void RequestData()
		{
			string url = $"https://crashlogs.keenswh.com/api/promotions?format_version=1.0&platform={MySession.GameServiceName}&language={MySandboxGame.Config.Language.ToString()}&game={MyPerGameSettings.BasicGameInfo.GameAcronym}&game_version={MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString()}";
			MyVRage.Platform.Http.SendRequestAsync(url, null, HttpMethod.GET, OnResponseReceived);
		}

		private void OnResponseReceived(HttpStatusCode statusCode, string content)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			if ((int)statusCode == 200)
			{
				MyBannerResponse data = null;
				try
				{
					data = JsonMapper.ToObject<MyBannerResponse>(content);
				}
				catch (Exception arg)
				{
					MyLog.Default.WriteLine($"MyBannerResponse reponse error: {arg}\n{content}");
				}
				if (data != null)
				{
					MySandboxGame.Static.Invoke(delegate
					{
						UpdateData(data);
					}, "MyGuiControlDLCBanners");
					return;
				}
			}
			MySandboxGame.Static.Invoke(RequestDataFailed, "MyGuiControlDLCBanners");
		}

		private void RequestDataFailed()
		{
		}

		private void DownloadImages(WorkData workData)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			MyImageDownloadTaskData imageData = workData as MyImageDownloadTaskData;
			if (imageData == null)
			{
				return;
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Promo");
			try
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
			}
			catch
			{
				return;
			}
			int pendingImages = 0;
			HashSet<string> obj2 = new HashSet<string>((IEnumerable<string>)imageData.ImagesToTest.Keys);
			bool flag = false;
			Enumerator<string> enumerator = obj2.GetEnumerator();
			try
			{
<<<<<<< HEAD
				string text2 = Path.GetFileNameWithoutExtension(image) + ZIP_EXTENSION;
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				string text3 = text + directorySeparatorChar + text2;
				if (!File.Exists(text3))
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					string image = enumerator.get_Current();
					string fileName = Path.GetFileName(image);
					string text2 = text + Path.DirectorySeparatorChar + fileName;
					if (!File.Exists(text2))
					{
<<<<<<< HEAD
						imageData.ImagesToTest[image] = text3;
						flag = true;
						Interlocked.Increment(ref pendingImages);
						MyVRage.Platform.Http.DownloadAsync(image + ZIP_API_PARAMETER, text3, null, delegate(HttpStatusCode x)
=======
						try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							imageData.ImagesToTest[image] = text2;
							flag = true;
							Interlocked.Increment(ref pendingImages);
							MyVRage.Platform.Http.DownloadAsync(image, text2, null, delegate(HttpStatusCode x)
							{
<<<<<<< HEAD
								imageData.ImagesToTest[image] = string.Empty;
							}
							if (Interlocked.Decrement(ref pendingImages) == 0)
							{
								UnpackImages(imageData);
								MySandboxGame.Static.Invoke(delegate
=======
								//IL_0000: Unknown result type (might be due to invalid IL or missing references)
								//IL_0006: Invalid comparison between Unknown and I4
								if ((int)x != 200)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									imageData.ImagesToTest[image] = string.Empty;
								}
								if (Interlocked.Decrement(ref pendingImages) == 0)
								{
									MySandboxGame.Static.Invoke(delegate
									{
										OnImagesDownloaded(workData);
										ShowDLC(m_cachedData[0]);
									}, "MyGuiControlDLCBanners");
								}
							});
						}
						catch
						{
							imageData.ImagesToTest[image] = string.Empty;
						}
					}
					else
					{
						imageData.ImagesToTest[image] = text2;
					}
				}
<<<<<<< HEAD
				else
				{
					imageData.ImagesToTest[image] = text3;
				}
=======
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (!flag)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					OnImagesDownloaded(workData);
					ShowDLC(m_cachedData[0]);
				}, "MyGuiControlDLCBanners");
			}
		}

		/// <summary>
		/// This method unpacks images. We have them packed, because it can happen that file is not downloaded correctly. If unpacking fails, we know that zip is not valid.
		/// </summary>
		/// <param name="imageData"></param>
		private void UnpackImages(MyImageDownloadTaskData imageData)
		{
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (KeyValuePair<string, string> item in imageData.ImagesToTest)
			{
				string value = item.Value;
				if (string.IsNullOrEmpty(value))
				{
					continue;
				}
				try
				{
					using (ZipArchive zipArchive = ZipFile.OpenRead(value))
					{
						foreach (ZipArchiveEntry entry in zipArchive.Entries)
						{
							string destinationFileName = Path.Combine(Path.GetDirectoryName(value), entry.FullName);
							entry.ExtractToFile(destinationFileName);
							hashSet2.Add(item.Key);
						}
					}
				}
				catch (Exception ex) when (ex is InvalidDataException || ex is IOException)
				{
					MyLog.Default.WriteLine("Error: Zip file is not valid - " + value);
					hashSet.Add(item.Key);
				}
			}
			foreach (string item2 in hashSet)
			{
				imageData.ImagesToTest[item2] = string.Empty;
			}
			foreach (string item3 in hashSet2)
			{
				string value2 = imageData.ImagesToTest[item3].Replace(ZIP_EXTENSION, DDS_EXTENSION);
				imageData.ImagesToTest[item3] = value2;
			}
		}

		private void UpdateData(MyBannerResponse data)
		{
			m_cachedData = data;
			m_cycleInterval = ((m_cachedData.CycleInterval == 0.0) ? 5f : ((float)m_cachedData.CycleInterval));
			m_fadeDuration = ((m_cachedData.FadeDuration == 0.0) ? 0.6f : ((float)m_cachedData.FadeDuration));
			foreach (MyBanner datum in m_cachedData.Data)
			{
				if (datum.PackageID != 0)
				{
					datum.Status = ((MyGameService.IsDlcInstalled(datum.PackageID) || MyGameService.HasInventoryItemWithDefinitionId((int)datum.PackageID)) ? MyBannerStatus.Installed : MyBannerStatus.NotInstalled);
				}
				else
				{
					datum.Status = MyBannerStatus.Offline;
				}
			}
			base.Visible = m_cachedData.NotInstalledCount > 0;
			m_buttonNext.Visible = base.Visible;
			m_buttonPrev.Visible = base.Visible;
			if (m_cachedData.NotInstalledCount == 0)
			{
				return;
			}
			MyImageDownloadTaskData myImageDownloadTaskData = new MyImageDownloadTaskData();
			for (int i = 0; i < m_cachedData.NotInstalledCount; i++)
			{
				MyBanner myBanner = m_cachedData[i];
				if (myBanner.Image.StartsWith("http"))
				{
					try
					{
						string text = ConvertImageURLToFile(myBanner.Image);
						if (File.Exists(text))
						{
							myBanner.Image = text;
						}
						else
						{
							myImageDownloadTaskData.ImagesToTest.Add(myBanner.Image, "");
						}
					}
					catch
					{
						myImageDownloadTaskData.ImagesToTest.Add(myBanner.Image, "");
					}
				}
				if (!myBanner.HighlightImage.StartsWith("http"))
				{
					continue;
				}
				try
				{
					string text2 = ConvertImageURLToFile(myBanner.HighlightImage);
					if (File.Exists(text2))
					{
						myBanner.HighlightImage = text2;
					}
					else
					{
						myImageDownloadTaskData.ImagesToTest[myBanner.HighlightImage] = "";
					}
				}
				catch
				{
					myImageDownloadTaskData.ImagesToTest[myBanner.HighlightImage] = "";
				}
			}
			Parallel.Start(DownloadImages, null, myImageDownloadTaskData);
		}

		private void OnImagesDownloaded(WorkData workData)
		{
			MyImageDownloadTaskData myImageDownloadTaskData = workData as MyImageDownloadTaskData;
			foreach (MyBanner datum in m_cachedData.Data)
			{
				if (myImageDownloadTaskData.ImagesToTest.TryGetValue(datum.Image, out var value))
				{
					datum.Image = value;
				}
				if (myImageDownloadTaskData.ImagesToTest.TryGetValue(datum.HighlightImage, out var value2))
				{
					datum.HighlightImage = value2;
				}
			}
		}

		private void ShowDLC(MyBanner dlc)
		{
			if (m_image.UserData is MyBanner)
			{
				m_oldImage.ApplyStyle(m_image.CurrentStyle);
				m_oldImage.SetToolTip(MyTexts.GetString(dlc.Tooltip));
				m_oldImage.UserData = dlc;
				m_transition = 0f;
				m_isTransitioning = true;
			}
			m_firstLineText.Text = MyTexts.GetString(dlc.CaptionLine1);
			m_secondLineText.Text = string.Format(MyTexts.GetString(dlc.CaptionLine2), MySession.GameServiceName);
			m_button.SetToolTip(MyTexts.GetString(dlc.Tooltip));
			string centerTexture = (dlc.Image.StartsWith("http") ? "" : dlc.Image);
			string centerTexture2 = (dlc.HighlightImage.StartsWith("http") ? "" : dlc.HighlightImage);
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture2)
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture2)
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture)
				}
			};
			m_image.ApplyStyle(style);
			m_image.SetToolTip(MyTexts.GetString(dlc.Tooltip));
			m_image.UserData = dlc;
			m_button.UserData = dlc;
			m_timeTillNextDLC = m_cycleInterval;
		}

		private string ConvertImageURLToFile(string imageUrl)
		{
			string text = Path.Combine(MyFileSystem.UserDataPath, "Promo");
			string fileName = Path.GetFileName(imageUrl);
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			return text + directorySeparatorChar + fileName;
		}

		private void OnImageClicked(MyGuiControlImageButton imageButton)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			MyBanner myBanner = imageButton.UserData as MyBanner;
			if (myBanner == null)
			{
				return;
			}
			MySpaceAnalytics.Instance?.ReportBannerClick(myBanner.CaptionLine1, myBanner.PackageID);
			if (!string.IsNullOrEmpty(myBanner.CustomData))
			{
				if (!string.IsNullOrWhiteSpace(myBanner.PackageURL))
				{
					MyVRage.Platform.Http.SendRequest(myBanner.PackageURL, null, HttpMethod.GET, out var _);
				}
				MyGameService.OpenInShop(myBanner.CustomData);
			}
			else if (!string.IsNullOrWhiteSpace(myBanner.PackageURL))
			{
				MyGuiSandbox.OpenUrl(myBanner.PackageURL, UrlOpenMode.SteamOrExternalWithConfirm);
			}
		}

		private void OnLabelClicked(MyGuiControlButton labelButton)
		{
			MyBanner myBanner = labelButton.UserData as MyBanner;
			if (myBanner != null)
			{
				MySpaceAnalytics.Instance?.ReportBannerClick(myBanner.CaptionLine1, myBanner.PackageID);
				if (!string.IsNullOrEmpty(myBanner.CustomData))
				{
					MyGameService.OpenInShop(myBanner.CustomData);
				}
				else if (!string.IsNullOrWhiteSpace(myBanner.PackageURL))
				{
					MyGuiSandbox.OpenUrl(myBanner.PackageURL, UrlOpenMode.SteamOrExternalWithConfirm);
				}
			}
		}

		private void OnNextButtonClicked(MyGuiControlButton button)
		{
			MyBanner data = m_image.UserData as MyBanner;
			int num = m_cachedData.IndexOf(data);
			num++;
			if (num >= m_cachedData.NotInstalledCount)
			{
				num = 0;
			}
			MyBanner dlc = m_cachedData[num];
			ShowDLC(dlc);
		}

		private void OnPrevButtonClicked(MyGuiControlButton button)
		{
			MyBanner data = m_image.UserData as MyBanner;
			int num = m_cachedData.IndexOf(data);
			num--;
			if (num < 0)
			{
				num = m_cachedData.NotInstalledCount - 1;
			}
			MyBanner dlc = m_cachedData[num];
			ShowDLC(dlc);
		}
	}
}
