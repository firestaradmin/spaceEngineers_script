using System;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
<<<<<<< HEAD
=======
using EmptyKeys.UserInterface.Debug;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using ParallelTasks;
using Sandbox;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Audio;
using VRage.Profiler;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.GUI
{
	public abstract class MyGuiScreenMvvmBase : MyGuiScreenBase
	{
<<<<<<< HEAD
=======
		private DebugViewModel m_debug;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_elapsedTime;

		private int m_previousTime;

		private bool m_layoutUpdated;

		private bool m_enableAsyncDraw;

		private MyRenderMessageDrawCommands m_drawAsyncMessages;

		private Task? m_drawTask;

		protected UIRoot m_view;

		protected MyViewModelBase m_viewModel;

		public MyGuiScreenMvvmBase(MyViewModelBase viewModel)
			: base(new Vector2(0.5f, 0.5f))
		{
			base.EnabledBackgroundFade = true;
			m_closeOnEsc = false;
			m_drawEvenWithoutFocus = true;
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			m_viewModel = viewModel;
			viewModel.MaxWidth = (float)MyGuiManager.GetSafeGuiRectangle().Width * (1f / UIElement.DpiScaleX);
			if (MySession.Static != null)
			{
				MySession.Static.LocalCharacter.CharacterDied += OnCharacterDied;
			}
		}

		public override void LoadContent()
		{
			base.LoadContent();
			RecreateControls(constructor: false);
		}

		public abstract UIRoot CreateView();

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_view = CreateView();
			if (m_view == null)
			{
				throw new NullReferenceException("View is empty");
			}
			RelayCommand command = new RelayCommand(OnExitScreen, CanExit);
			GamepadBinding item = new GamepadBinding(command, new GamepadGesture(GamepadInput.BButton));
			m_view.InputBindings.Add(item);
			KeyBinding item2 = new KeyBinding(command, new KeyGesture(KeyCode.Escape));
			m_view.InputBindings.Add(item2);
			m_viewModel.BackgroundOverlay = new ColorW(1f, 1f, 1f, MySandboxGame.Config.UIBkOpacity);
			ImageManager.Instance.LoadImages(null);
			SoundSourceCollection defaultValue = new SoundSourceCollection
			{
				new SoundSource
				{
					SoundType = SoundType.ButtonsClick,
					SoundAsset = GuiSounds.MouseClick.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.ButtonsHover,
					SoundAsset = GuiSounds.MouseOver.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.CheckBoxHover,
					SoundAsset = GuiSounds.MouseOver.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.TabControlSelect,
					SoundAsset = GuiSounds.MouseClick.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.TabControlMove,
					SoundAsset = GuiSounds.MouseClick.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.ListBoxSelect,
					SoundAsset = GuiSounds.MouseClick.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.FocusChanged,
					SoundAsset = GuiSounds.MouseOver.ToString()
				}
			};
			SoundManager.SoundsProperty.DefaultMetadata.DefaultValue = defaultValue;
			SoundManager.Instance.AddSound(GuiSounds.MouseClick.ToString());
			SoundManager.Instance.AddSound(GuiSounds.MouseOver.ToString());
			SoundManager.Instance.AddSound(GuiSounds.Item.ToString());
			SoundManager.Instance.LoadSounds(null);
			m_view.DataContext = m_viewModel;
			Parallel.Start(delegate
			{
				m_view.UpdateLayout(0.0);
			}, delegate
			{
				m_layoutUpdated = true;
				m_viewModel.InitializeData();
			});
		}

		protected virtual bool CanExit(object parameter)
		{
			return true;
		}

		private void OnExitScreen(object obj)
		{
			Canceling();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (m_layoutUpdated)
			{
				if (InputManager.Current.FocusedElement is TabControl)
				{
					InputManager.Current.NavigateTabNext(messageBoxVisible: false);
					m_view.ShowGamepadHelp(InputManager.Current.FocusedElement);
				}
				m_view.UpdateInput(m_elapsedTime);
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		private void OnCharacterDied(MyCharacter character)
		{
			CloseScreen();
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			m_viewModel.OnScreenClosing();
			m_layoutUpdated = false;
			VisualTreeHelper.Instance.ClearParentCache();
			if (MySession.Static != null)
			{
				MySession.Static.LocalCharacter.CharacterDied -= OnCharacterDied;
			}
			Action action = m_viewModel.OnScreenClosed;
			bool result = base.CloseScreen(isUnloading);
			action();
			return result;
		}

		public override bool Update(bool hasFocus)
		{
			Engine.Instance.Update();
			m_viewModel.Update();
			return base.Update(hasFocus);
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			if (!m_layoutUpdated)
			{
				return false;
			}
			m_elapsedTime = MySandboxGame.TotalTimeInMilliseconds - m_previousTime;
			bool flag = true;
			m_view.UpdateLayout(m_elapsedTime);
			if (!m_enableAsyncDraw)
			{
				m_view.Draw(m_elapsedTime);
			}
			else
			{
				if (m_drawAsyncMessages == null)
				{
					DrawAsync();
				}
				m_drawTask?.WaitOrExecute();
				MyRenderProxy.ExecuteCommands(m_drawAsyncMessages, flag);
				m_drawTask = null;
				if (flag)
				{
					m_drawTask = Parallel.Start(DrawAsync, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "EKScreensDraw"), WorkPriority.VeryHigh);
				}
			}
			m_previousTime = MySandboxGame.TotalTimeInMilliseconds;
			return true;
		}

		private void DrawAsync()
		{
			MyRenderProxy.BeginRecordingDeferredMessages();
			m_view.Draw(m_elapsedTime);
			m_drawAsyncMessages = MyRenderProxy.FinishRecordingDeferredMessages();
		}
	}
}
