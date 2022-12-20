using System;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenFactoryService : IMyGuiScreenFactoryService
	{
		public bool IsAnyScreenOpen { get; set; }

		public void CreateScreen(ViewModelBase viewModel)
		{
			if (!IsAnyScreenOpen)
			{
				MyGuiScreenMvvmBase myGuiScreenMvvmBase = null;
				if (viewModel.GetType() == typeof(MyPlayerTradeViewModel))
				{
					myGuiScreenMvvmBase = MyGuiSandbox.CreateScreen<MyGuiScreenTradePlayer>(new object[1] { viewModel });
				}
				else if (viewModel.GetType() == typeof(MyEditFactionIconViewModel))
				{
					myGuiScreenMvvmBase = MyGuiSandbox.CreateScreen<MyGuiScreenEditFactionIcon>(new object[1] { viewModel });
				}
				else if (viewModel.GetType() == typeof(MyContractsActiveViewModel))
				{
					myGuiScreenMvvmBase = MyGuiSandbox.CreateScreen<MyGuiScreenActiveContracts>(new object[1] { viewModel });
				}
				else if (viewModel.GetType() == typeof(MyWorkshopBrowserViewModel))
				{
					myGuiScreenMvvmBase = MyGuiSandbox.CreateScreen<MyGuiScreenWorkshopBrowser>(new object[1] { viewModel });
				}
				else if (viewModel.GetType() == typeof(MyModIoConsentViewModel))
				{
					myGuiScreenMvvmBase = MyGuiSandbox.CreateScreen<MyGuiScreenModIoConsent>(new object[1] { viewModel });
				}
				if (viewModel is MyViewModelBase)
				{
					((MyViewModelBase)viewModel).ScreenBase = myGuiScreenMvvmBase;
				}
				AddScreen(myGuiScreenMvvmBase);
			}
		}

		public Type GetMyGuiScreenBase(Type viewModelType)
		{
			if (viewModelType == typeof(MyPlayerTradeViewModel))
			{
				return typeof(MyGuiScreenTradePlayer);
			}
			if (viewModelType == typeof(MyEditFactionIconViewModel))
			{
				return typeof(MyGuiScreenEditFactionIcon);
			}
			if (viewModelType == typeof(MyContractsActiveViewModel))
			{
				return typeof(MyGuiScreenActiveContracts);
			}
			if (viewModelType == typeof(MyWorkshopBrowserViewModel))
			{
				return typeof(MyGuiScreenWorkshopBrowser);
			}
			if (viewModelType == typeof(MyModIoConsentViewModel))
			{
				return typeof(MyGuiScreenModIoConsent);
			}
			return null;
		}

		private void AddScreen(MyGuiScreenBase screen)
		{
			MyGuiSandbox.AddScreen(screen);
			IsAnyScreenOpen = true;
			screen.Closed += Screen_Closed;
		}

		private void Screen_Closed(MyGuiScreenBase screen, bool isUnloading)
		{
			IsAnyScreenOpen = false;
			screen.Closed -= Screen_Closed;
		}
	}
}
