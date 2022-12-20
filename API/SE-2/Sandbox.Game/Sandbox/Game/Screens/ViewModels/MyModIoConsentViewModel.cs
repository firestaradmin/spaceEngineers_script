using System;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.GameServices;
using VRage.Input;
using VRageRender;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyModIoConsentViewModel : MyViewModelBase
	{
		private string m_consentCaption;

		private string m_consentTextPart1;

		private string m_consentTextPart2;

		private string m_consentTextPart3;

		private bool m_steamControlsVisible;

		private bool m_steamTOURequired;

		private bool m_modioTOURequired;

		private bool m_agreeButtonEnabled;

		private ICommand m_agreeCommand;

		private ICommand m_optOutCommand;

		private ICommand m_modioTermsOfUseCommand;

		private ICommand m_modioPrivacyPolicyCommand;

		private ICommand m_steamTermsOfUseCommand;

		private ICommand m_steamPrivacyPolicyCommand;

		private Action m_onConsentAgree;

		private Action m_onConsentOptOut;

		private float m_width;

		private float m_height;

		private bool m_leaveActionStarted;

		public float Width
		{
			get
			{
				return m_width;
			}
			set
			{
				SetProperty(ref m_width, value, "Width");
			}
		}

		public float Height
		{
			get
			{
				return m_height;
			}
			set
			{
				SetProperty(ref m_height, value, "Height");
			}
		}

		public ICommand AgreeCommand
		{
			get
			{
				return m_agreeCommand;
			}
			set
			{
				SetProperty(ref m_agreeCommand, value, "AgreeCommand");
			}
		}

		public ICommand OptOutCommand
		{
			get
			{
				return m_optOutCommand;
			}
			set
			{
				SetProperty(ref m_optOutCommand, value, "OptOutCommand");
			}
		}

		public ICommand ModioTermsOfUseCommand
		{
			get
			{
				return m_modioTermsOfUseCommand;
			}
			set
			{
				SetProperty(ref m_modioTermsOfUseCommand, value, "ModioTermsOfUseCommand");
			}
		}

		public ICommand ModioPrivacyPolicyCommand
		{
			get
			{
				return m_modioPrivacyPolicyCommand;
			}
			set
			{
				SetProperty(ref m_modioPrivacyPolicyCommand, value, "ModioPrivacyPolicyCommand");
			}
		}

		public ICommand SteamTermsOfUseCommand
		{
			get
			{
				return m_steamTermsOfUseCommand;
			}
			set
			{
				SetProperty(ref m_steamTermsOfUseCommand, value, "SteamTermsOfUseCommand");
			}
		}

		public ICommand SteamPrivacyPolicyCommand
		{
			get
			{
				return m_steamPrivacyPolicyCommand;
			}
			set
			{
				SetProperty(ref m_steamPrivacyPolicyCommand, value, "SteamPrivacyPolicyCommand");
			}
		}

		public string ConsentCaption
		{
			get
			{
				return m_consentCaption;
			}
			private set
			{
				m_consentCaption = value;
				RaisePropertyChanged("ConsentCaption");
			}
		}

		public string ConsentTextPart1
		{
			get
			{
				return m_consentTextPart1;
			}
			private set
			{
				m_consentTextPart1 = value;
				RaisePropertyChanged("ConsentTextPart1");
			}
		}

		public string ConsentTextPart2
		{
			get
			{
				return m_consentTextPart2;
			}
			private set
			{
				m_consentTextPart2 = value;
				RaisePropertyChanged("ConsentTextPart2");
			}
		}

		public string ConsentTextPart3
		{
			get
			{
				return m_consentTextPart3;
			}
			private set
			{
				m_consentTextPart3 = value;
				RaisePropertyChanged("ConsentTextPart3");
			}
		}

		public bool SteamControls
		{
			get
			{
				return m_steamControlsVisible;
			}
			private set
			{
				m_steamControlsVisible = value;
				RaisePropertyChanged("SteamControls");
			}
		}

		public bool SteamTOURequired
		{
			get
			{
				return m_steamTOURequired;
			}
			set
			{
				m_steamTOURequired = value;
				RaisePropertyChanged("SteamTOURequired");
				EnableAgreeButtonChecked();
			}
		}

		public bool ModioTOURequired
		{
			get
			{
				return m_modioTOURequired;
			}
			set
			{
				m_modioTOURequired = value;
				RaisePropertyChanged("ModioTOURequired");
				EnableAgreeButtonChecked();
			}
		}

		public bool AgreeButtonEnabled
		{
			get
			{
				return m_agreeButtonEnabled;
			}
			set
			{
				m_agreeButtonEnabled = value;
				RaisePropertyChanged("AgreeButtonEnabled");
				WarningVisible = (m_agreeButtonEnabled ? Visibility.Hidden : Visibility.Visible);
				RaisePropertyChanged("WarningVisible");
				AgreeHelpTextForeground = new SolidColorBrush(new ColorW(m_agreeButtonEnabled ? 4291222756u : 4284311918u));
				RaisePropertyChanged("AgreeHelpTextForeground");
			}
		}

		public Brush AgreeHelpTextForeground { get; private set; }

		public Visibility WarningVisible { get; private set; }

		public MyModIoConsentViewModel(Action onConsentAgree = null, Action onConsentOptOut = null)
		{
			AgreeCommand = new RelayCommand(OnAgree);
			OptOutCommand = new RelayCommand(OnOptOut);
			ModioTermsOfUseCommand = new RelayCommand(OnModioTermsOfUse);
			ModioPrivacyPolicyCommand = new RelayCommand(OnModioPrivacyPolicy);
			SteamTermsOfUseCommand = new RelayCommand(OnSteamTermsOfUse);
			SteamPrivacyPolicyCommand = new RelayCommand(OnSteamPrivacyPolicy);
			m_onConsentAgree = onConsentAgree;
			m_onConsentOptOut = onConsentOptOut;
			SteamControls = false;
			ConsentCaption = string.Format(MyTexts.GetString(SteamControls ? MySpaceTexts.ScreenCaptionSteamAndModIoConsent : MySpaceTexts.ScreenCaptionModIoConsent));
			ConsentTextPart1 = string.Format(MyTexts.GetString(SteamControls ? MySpaceTexts.ScreenSteamAndModIoConsent_ConsentTextPart1 : MySpaceTexts.ScreenModIoConsent_ConsentTextPart1));
			ConsentTextPart2 = string.Format(MyTexts.GetString(SteamControls ? MySpaceTexts.ScreenSteamAndModIoConsent_ConsentTextPart2 : MySpaceTexts.ScreenModIoConsent_ConsentTextPart2), MyGameService.Service.ServiceName);
			ConsentTextPart3 = string.Format(MyTexts.GetString(SteamControls ? MySpaceTexts.ScreenSteamAndModIoConsent_ConsentTextPart3 : MySpaceTexts.ScreenModIoConsent_ConsentTextPart3));
			m_steamTOURequired = SteamControls && !MySandboxGame.Config.SteamConsent;
			m_modioTOURequired = !MySandboxGame.Config.ModIoConsent;
			AgreeButtonEnabled = false;
			EnableAgreeButtonChecked();
			SetScreenSize();
		}

		public override void OnScreenClosed()
		{
			if (!m_leaveActionStarted)
			{
				m_leaveActionStarted = true;
				OnExit(this);
				m_onConsentOptOut?.Invoke();
			}
			base.OnScreenClosed();
		}

		private void SetScreenSize()
		{
			Width = 700f;
			int num = (MyInput.Static.IsJoystickLastUsed ? 145 : 190);
			if (!m_steamControlsVisible)
			{
				num -= 70;
			}
			MyRenderDeviceSettings currentDeviceSettings = MyVideoSettingsManager.CurrentDeviceSettings;
			switch (MyVideoSettingsManager.GetClosestAspectRatio((float)currentDeviceSettings.BackBufferWidth / (float)currentDeviceSettings.BackBufferHeight))
			{
			case MyAspectRatioEnum.Unsupported_5_4:
				Width = 850f;
				Height = 775 + num;
				break;
			case MyAspectRatioEnum.Normal_4_3:
			case MyAspectRatioEnum.Dual_4_3:
			case MyAspectRatioEnum.Triple_4_3:
				Width = 800f;
				Height = 775 + num;
				break;
			case MyAspectRatioEnum.Normal_16_10:
			case MyAspectRatioEnum.Dual_16_10:
			case MyAspectRatioEnum.Triple_16_10:
				Height = 700 + num;
				break;
			case MyAspectRatioEnum.Normal_16_9:
			case MyAspectRatioEnum.Dual_16_9:
			case MyAspectRatioEnum.Triple_16_9:
				Height = 600 + num;
				break;
			default:
				Height = 700 + num;
				break;
			}
		}

		private void OnModioPrivacyPolicy(object obj)
		{
			MyGuiSandbox.OpenUrlWithFallback("https://mod.io/privacy", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_PrivacyPolicy_UrlFriendlyName));
		}

		private void OnModioTermsOfUse(object obj)
		{
			MyGuiSandbox.OpenUrlWithFallback("https://mod.io/terms", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_TermsOfUse_UrlFriendlyName), useWhitelist: false, delegate(bool success)
			{
				if (success)
				{
					ModioTOURequired = false;
				}
			});
		}

		private void OnSteamPrivacyPolicy(object obj)
		{
			MyGuiSandbox.OpenUrlWithFallback("https://store.steampowered.com/privacy_agreement/", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_SteamPrivacyPolicy_UrlFriendlyName));
		}

		private void OnSteamTermsOfUse(object obj)
		{
			MyGuiSandbox.OpenUrlWithFallback("http://steamcommunity.com/sharedfiles/workshoplegalagreement", MyTexts.GetString(MySpaceTexts.ScreenModIoConsent_SteamTermsOfUse_UrlFriendlyName), useWhitelist: false, delegate(bool success)
			{
				if (success)
				{
					SteamTOURequired = false;
				}
			});
		}

		private void OnOptOut(object obj)
		{
			IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate("mod.io");
			if (aggregate != null)
			{
				aggregate.IsConsentGiven = false;
			}
			MySandboxGame.Config.ModIoConsent = false;
			if (SteamControls)
			{
				MySandboxGame.Config.SteamConsent = false;
			}
			MySandboxGame.Config.Save();
			Action onConsentOptOut = m_onConsentOptOut;
			m_onConsentOptOut = null;
			m_leaveActionStarted = true;
			OnExit(this);
			onConsentOptOut?.Invoke();
		}

		private void OnAgree(object obj)
		{
			if (AgreeButtonEnabled)
			{
				IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate("mod.io");
				if (aggregate != null)
				{
					aggregate.IsConsentGiven = true;
				}
				MySandboxGame.Config.ModIoConsent = true;
				if (SteamControls)
				{
					MySandboxGame.Config.SteamConsent = true;
				}
				MySandboxGame.Config.Save();
				m_leaveActionStarted = true;
				if (m_onConsentAgree != null)
				{
					m_onConsentAgree();
				}
				OnExit(this);
			}
		}

		private void EnableAgreeButtonChecked()
		{
			if (!m_modioTOURequired && !m_steamTOURequired)
			{
				AgreeButtonEnabled = true;
			}
		}
	}
}
