using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Jukebox))]
	public class MyJukebox : MySoundBlock, IMyTextSurfaceProvider, IMyTextPanelComponentOwner
	{
		protected sealed class StopJukeboxSound_003C_003E : ICallSite<MyJukebox, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJukebox @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.StopJukeboxSound();
			}
		}

		protected sealed class PlayJukeboxSound_003C_003ESystem_Boolean : ICallSite<MyJukebox, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJukebox @this, in bool isLoopable, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PlayJukeboxSound(isLoopable);
			}
		}

		protected sealed class OnAddSoundsToSelectionRequest_003C_003ESystem_Collections_Generic_List_00601_003CSystem_String_003E : ICallSite<MyJukebox, List<string>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJukebox @this, in List<string> selection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAddSoundsToSelectionRequest(selection);
			}
		}

		protected sealed class OnRemoveSoundsFromSelectionRequest_003C_003ESystem_Collections_Generic_List_00601_003CSystem_String_003E : ICallSite<MyJukebox, List<string>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJukebox @this, in List<string> selection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSoundsFromSelectionRequest(selection);
			}
		}

		protected class m_isJukeboxPlaying_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isJukeboxPlaying;
				ISyncType result = (isJukeboxPlaying = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyJukebox)P_0).m_isJukeboxPlaying = (Sync<bool, SyncDirection.BothWays>)isJukeboxPlaying;
				return result;
			}
		}

		protected class m_currentSound_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentSound;
				ISyncType result = (currentSound = new Sync<int, SyncDirection.BothWays>(P_1, P_2));
				((MyJukebox)P_0).m_currentSound = (Sync<int, SyncDirection.BothWays>)currentSound;
				return result;
			}
		}

		private readonly Sync<bool, SyncDirection.BothWays> m_isJukeboxPlaying;

		private Sync<int, SyncDirection.BothWays> m_currentSound;

		private List<string> m_selectedSounds = new List<string>();

		private List<string> m_soundBlockListSelection = new List<string>();

		private List<string> m_jukeboxListSelection = new List<string>();

		private string m_localCueIdString;

		public new MyJukeboxDefinition BlockDefinition => (MyJukeboxDefinition)base.BlockDefinition;

		public bool IsJukeboxPlaying => m_isJukeboxPlaying;

<<<<<<< HEAD
=======
		public MyTextPanelComponent PanelComponent => m_multiPanel?.PanelComponent;

		public bool IsTextPanelOpen => false;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override MyCubeBlockHighlightModes HighlightMode => MyCubeBlockHighlightModes.AlwaysCanUse;

		public MyJukebox()
		{
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_Jukebox myObjectBuilder_Jukebox = objectBuilder as MyObjectBuilder_Jukebox;
			if (myObjectBuilder_Jukebox.SelectedSounds != null)
			{
				m_selectedSounds = Enumerable.ToList<string>((IEnumerable<string>)myObjectBuilder_Jukebox.SelectedSounds);
			}
			m_isJukeboxPlaying.SetLocalValue(myObjectBuilder_Jukebox.IsJukeboxPlaying);
			m_currentSound.SetLocalValue(myObjectBuilder_Jukebox.CurrentSound);
			if ((bool)m_isJukeboxPlaying)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
<<<<<<< HEAD
			MyObjectBuilder_Jukebox obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Jukebox;
			obj.SelectedSounds = m_selectedSounds.ToList();
			obj.IsJukeboxPlaying = m_isJukeboxPlaying;
			obj.CurrentSound = m_currentSound;
			return obj;
=======
			MyObjectBuilder_Jukebox myObjectBuilder_Jukebox = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Jukebox;
			if (m_multiPanel != null)
			{
				myObjectBuilder_Jukebox.TextPanels = m_multiPanel.Serialize();
			}
			myObjectBuilder_Jukebox.SelectedSounds = Enumerable.ToList<string>((IEnumerable<string>)m_selectedSounds);
			myObjectBuilder_Jukebox.IsJukeboxPlaying = m_isJukeboxPlaying;
			myObjectBuilder_Jukebox.CurrentSound = m_currentSound;
			return myObjectBuilder_Jukebox;
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_multiPanel != null)
			{
				m_multiPanel.Reset();
			}
			UpdateScreen();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (!Sync.IsDedicated && MyAudio.Static.CanPlay && !m_soundEmitter.IsPlaying && IsJukeboxPlaying && base.IsWorking && base.IsFunctional)
			{
<<<<<<< HEAD
				PlayNextLocally();
=======
				if (m_multiPanel != null)
				{
					m_multiPanel.UpdateAfterSimulation(base.IsWorking);
				}
				if (MyAudio.Static.CanPlay && !m_soundEmitter.IsPlaying && IsJukeboxPlaying && base.IsWorking && base.IsFunctional)
				{
					PlayNextLocally();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (!Sync.IsDedicated && MySession.Static.LocalCharacter != null && Vector3D.DistanceSquared(base.PositionComp.WorldMatrixRef.Translation, MySession.Static.LocalCharacter.PositionComp.GetPosition()) < (double)(m_soundRadius.Value * m_soundRadius.Value))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (base.IsWorking && base.IsFunctional)
			{
				if ((bool)m_isJukeboxPlaying && !m_isPlaying)
				{
					PlaySound(isLoopable: false);
				}
			}
			else
			{
				StopSound();
			}
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

<<<<<<< HEAD
		protected override void CreateTerminalControls()
=======
		public void UpdateScreen()
		{
			m_multiPanel?.UpdateScreen(base.IsWorking);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 219)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		[DistanceRadius(32f)]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 235)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyJukebox>())
			{
				return;
			}
<<<<<<< HEAD
			base.CreateTerminalControls();
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJukebox>("SelectSounds", MySpaceTexts.BlockPropertyTitle_JukeboxScreenSelectSounds, MySpaceTexts.Blank, delegate(MyJukebox x)
			{
				x.SendAddSoundsToSelection();
			}));
			MyTerminalControlListbox<MyJukebox> obj = new MyTerminalControlListbox<MyJukebox>("SelectedSoundsList", MySpaceTexts.BlockPropertyTitle_JukeboxScreenSelectedSounds, MySpaceTexts.Blank, multiSelect: true)
			{
=======
		}

		[Event(null, 246)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectItems(panelIndex, selection);
			}
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyJukebox>())
			{
				return;
			}
			base.CreateTerminalControls();
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJukebox>("SelectSounds", MySpaceTexts.BlockPropertyTitle_JukeboxScreenSelectSounds, MySpaceTexts.Blank, delegate(MyJukebox x)
			{
				x.SendAddSoundsToSelection();
			}));
			MyTerminalControlListbox<MyJukebox> obj = new MyTerminalControlListbox<MyJukebox>("SelectedSoundsList", MySpaceTexts.BlockPropertyTitle_JukeboxScreenSelectedSounds, MySpaceTexts.Blank, multiSelect: true)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ListContent = delegate(MyJukebox x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					x.FillSelectedSoundsContent(list1, list2);
				},
				ItemDoubleClicked = delegate(MyJukebox x, List<MyGuiControlListbox.Item> y)
				{
					x.SendRemoveSoundsFromSelection();
				}
			};
			obj.ItemSelected = (MyTerminalControlListbox<MyJukebox>.SelectItemDelegate)Delegate.Combine(obj.ItemSelected, (MyTerminalControlListbox<MyJukebox>.SelectItemDelegate)delegate(MyJukebox x, List<MyGuiControlListbox.Item> y)
			{
				x.SelectJukeboxItem(y);
			});
			MyTerminalControlFactory.AddControl(obj);
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJukebox>("RemoveSelectedSounds", MySpaceTexts.BlockPropertyTitle_JukeboxScreenRemoveSelectedSounds, MySpaceTexts.Blank, delegate(MyJukebox x)
			{
				x.SendRemoveSoundsFromSelection();
			}));
<<<<<<< HEAD
			MyTerminalControlListbox<MySoundBlock> myTerminalControlListbox = MyTerminalControlFactory.GetControls(typeof(MySoundBlock)).FirstOrDefault((ITerminalControl x) => x.Id == "SoundsList") as MyTerminalControlListbox<MySoundBlock>;
=======
			MyTerminalControlListbox<MySoundBlock> myTerminalControlListbox = Enumerable.FirstOrDefault<ITerminalControl>((IEnumerable<ITerminalControl>)MyTerminalControlFactory.GetControls(typeof(MySoundBlock)), (Func<ITerminalControl, bool>)((ITerminalControl x) => x.Id == "SoundsList")) as MyTerminalControlListbox<MySoundBlock>;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myTerminalControlListbox != null)
			{
				((IMyTerminalControlListbox)myTerminalControlListbox).Multiselect = true;
				myTerminalControlListbox.ItemDoubleClicked = (MyTerminalControlListbox<MySoundBlock>.SelectItemDelegate)Delegate.Combine(myTerminalControlListbox.ItemDoubleClicked, (MyTerminalControlListbox<MySoundBlock>.SelectItemDelegate)delegate(MySoundBlock x, List<MyGuiControlListbox.Item> y)
				{
					(x as MyJukebox)?.SendAddSoundsToSelection();
				});
			}
<<<<<<< HEAD
			MyTerminalControlButton<MySoundBlock> myTerminalControlButton = MyTerminalControlFactory.GetControls(typeof(MySoundBlock)).FirstOrDefault((ITerminalControl x) => x.Id == "PlaySound") as MyTerminalControlButton<MySoundBlock>;
=======
			MyTerminalControlButton<MySoundBlock> myTerminalControlButton = Enumerable.FirstOrDefault<ITerminalControl>((IEnumerable<ITerminalControl>)MyTerminalControlFactory.GetControls(typeof(MySoundBlock)), (Func<ITerminalControl, bool>)((ITerminalControl x) => x.Id == "PlaySound")) as MyTerminalControlButton<MySoundBlock>;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myTerminalControlButton != null)
			{
				myTerminalControlButton.Visible = (MySoundBlock x) => !(x is MyJukebox);
			}
<<<<<<< HEAD
			MyTerminalControlButton<MySoundBlock> myTerminalControlButton2 = MyTerminalControlFactory.GetControls(typeof(MySoundBlock)).FirstOrDefault((ITerminalControl x) => x.Id == "StopSound") as MyTerminalControlButton<MySoundBlock>;
=======
			MyTerminalControlButton<MySoundBlock> myTerminalControlButton2 = Enumerable.FirstOrDefault<ITerminalControl>((IEnumerable<ITerminalControl>)MyTerminalControlFactory.GetControls(typeof(MySoundBlock)), (Func<ITerminalControl, bool>)((ITerminalControl x) => x.Id == "StopSound")) as MyTerminalControlButton<MySoundBlock>;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myTerminalControlButton2 != null)
			{
				myTerminalControlButton2.Visible = (MySoundBlock x) => !(x is MyJukebox);
			}
		}

		public void PlayOrStop()
		{
			if ((bool)m_isJukeboxPlaying)
			{
				RequestJukeboxStopSound();
			}
			else
			{
				PlayCurrentSound();
			}
		}

		public void PlayNext()
		{
			m_currentSound.Value = (int)m_currentSound + 1;
			if ((bool)m_isJukeboxPlaying)
			{
				PlayCurrentSound();
				return;
			}
			m_localCueIdString = null;
			UpdateCue();
		}

		private void PlayNextLocally()
		{
			m_currentSound.SetLocalValue((int)m_currentSound + 1);
			UpdateCurrentSound();
			if ((int)m_currentSound >= 0)
			{
				m_localCueIdString = m_selectedSounds[m_currentSound];
				MySoundPair cueId = new MySoundPair(m_localCueIdString);
				PlaySingleSound(cueId);
			}
		}

		public void PlayPrevious()
		{
			m_currentSound.Value = (int)m_currentSound - 1;
			if ((bool)m_isJukeboxPlaying)
			{
				PlayCurrentSound();
				return;
			}
			UpdateCue();
			m_localCueIdString = null;
		}

		private void PlayCurrentSound()
		{
			UpdateCue();
			if (!string.IsNullOrEmpty(m_localCueIdString))
			{
				m_cueIdString.Value = m_localCueIdString;
				m_localCueIdString = null;
			}
			if (string.IsNullOrEmpty(m_cueIdString.Value))
			{
				m_isJukeboxPlaying.Value = false;
				if (Sync.IsServer)
				{
					RequestStopSound();
				}
				else
				{
					StopSound();
				}
			}
			else
			{
				RequestJukeboxPlaySound();
			}
		}

		public void RequestJukeboxStopSound()
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.StopJukeboxSound);
		}

<<<<<<< HEAD
		[Event(null, 279)]
=======
		[Event(null, 409)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void StopJukeboxSound()
		{
			StopSoundInternal();
			m_isJukeboxPlaying.SetLocalValue(newValue: false);
			m_localCueIdString = null;
		}

		public void RequestJukeboxPlaySound()
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.PlayJukeboxSound, arg2: false);
		}

<<<<<<< HEAD
		[Event(null, 292)]
=======
		[Event(null, 422)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void PlayJukeboxSound(bool isLoopable)
		{
			PlaySoundInternal(isLoopable);
			m_isJukeboxPlaying.SetLocalValue(newValue: true);
		}

		private void UpdateCurrentSound()
		{
			if ((int)m_currentSound < 0)
			{
				m_currentSound.SetLocalValue(m_selectedSounds.Count - 1);
			}
			else if ((int)m_currentSound >= m_selectedSounds.Count)
			{
				m_currentSound.SetLocalValue(0);
			}
			if (m_selectedSounds.Count == 0)
			{
				m_currentSound.SetLocalValue(-1);
			}
		}

		private void UpdateCue()
		{
			UpdateCurrentSound();
			if ((int)m_currentSound >= 0)
			{
				m_cueIdString.Value = m_selectedSounds[m_currentSound];
			}
			else
			{
				m_cueIdString.Value = "";
			}
		}

		protected override void SelectSound(List<MyGuiControlListbox.Item> cuesId, bool sync)
		{
			m_soundBlockListSelection.Clear();
			foreach (MyGuiControlListbox.Item item in cuesId)
			{
				m_soundBlockListSelection.Add(item.UserData.ToString());
			}
		}

		private void SendAddSoundsToSelection()
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.OnAddSoundsToSelectionRequest, m_soundBlockListSelection);
		}

<<<<<<< HEAD
		[Event(null, 337)]
=======
		[Event(null, 467)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnAddSoundsToSelectionRequest(List<string> selection)
		{
			AddSoundsToSelection(selection);
		}

		private void AddSoundsToSelection(List<string> selection)
		{
			foreach (string item in selection)
			{
				m_selectedSounds.Add(item);
			}
			m_currentSound.Value = m_selectedSounds.Count - 1;
			UpdateCue();
			RaisePropertiesChanged();
		}

		private void SendRemoveSoundsFromSelection()
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.OnRemoveSoundsFromSelectionRequest, Enumerable.ToList<string>((IEnumerable<string>)m_jukeboxListSelection));
		}

<<<<<<< HEAD
		[Event(null, 362)]
=======
		[Event(null, 492)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSoundsFromSelectionRequest(List<string> selection)
		{
			RemoveSoundsFromSelection(selection);
		}

		private void RemoveSoundsFromSelection(List<string> selection)
		{
			foreach (string item in selection)
			{
				m_selectedSounds.Remove(item);
			}
			if (IsJukeboxPlaying)
			{
				PlayCurrentSound();
			}
			RaisePropertiesChanged();
		}

		private void SelectJukeboxItem(List<MyGuiControlListbox.Item> items)
		{
			m_jukeboxListSelection.Clear();
			foreach (MyGuiControlListbox.Item item in items)
			{
				int num = (int)item.UserData;
				if (num < m_selectedSounds.Count)
				{
					m_jukeboxListSelection.Add(m_selectedSounds[num]);
				}
			}
		}

		public MySoundCategoryDefinition.SoundDescription GetCurrentSoundDescription()
		{
			if (!string.IsNullOrEmpty(m_localCueIdString))
			{
				return GetSoundDescription(m_localCueIdString);
			}
			if (string.IsNullOrEmpty(m_cueIdString))
			{
				return null;
			}
			return GetSoundDescription(m_cueIdString);
		}

		private MySoundCategoryDefinition.SoundDescription GetSoundDescription(string soundName)
		{
			ListReader<MySoundCategoryDefinition> soundCategoryDefinitions = MyDefinitionManager.Static.GetSoundCategoryDefinitions();
			MySoundCategoryDefinition.SoundDescription soundDescription = null;
			foreach (MySoundCategoryDefinition item in soundCategoryDefinitions)
			{
				foreach (MySoundCategoryDefinition.SoundDescription sound in item.Sounds)
				{
					if (soundName == sound.SoundId)
					{
						soundDescription = sound;
						break;
					}
				}
				if (soundDescription != null)
				{
					return soundDescription;
				}
			}
			return soundDescription;
		}

		private void FillSelectedSoundsContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			int num = 0;
			foreach (string selectedSound in m_selectedSounds)
			{
				MySoundCategoryDefinition.SoundDescription soundDescription = GetSoundDescription(selectedSound);
				if (soundDescription != null)
				{
					listBoxContent.Add(new MyGuiControlListbox.Item(new StringBuilder(soundDescription.SoundText), soundDescription.SoundText, null, num++));
				}
			}
		}
<<<<<<< HEAD
=======

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyJukebox x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 580)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
