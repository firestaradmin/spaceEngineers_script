using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MyEntity3DSoundEmitter : IMy3DSoundEmitter
	{
		internal struct LastTimePlayingData
		{
			public int LastTime;

			public MyEntity3DSoundEmitter Emitter;
		}

		public enum MethodsEnum
		{
			CanHear,
			ShouldPlay2D,
			CueType,
			ImplicitEffect
		}

		internal static readonly ConcurrentDictionary<MyCueId, LastTimePlayingData> LastTimePlaying = new ConcurrentDictionary<MyCueId, LastTimePlayingData>();

		private static List<MyEntity3DSoundEmitter> m_entityEmitters = new List<MyEntity3DSoundEmitter>();

		private static int m_lastUpdate = int.MinValue;

		private static MyStringHash m_effectHasHelmetInOxygen = MyStringHash.GetOrCompute("LowPassHelmet");

		private static MyStringHash m_effectNoHelmetNoOxygen = MyStringHash.GetOrCompute("LowPassNoHelmetNoOxy");

		private static MyStringHash m_effectEnclosedCockpitInSpace = MyStringHash.GetOrCompute("LowPassCockpitNoOxy");

		private static MyStringHash m_effectEnclosedCockpitInAir = MyStringHash.GetOrCompute("LowPassCockpit");

		private MyCueId m_cueEnum = new MyCueId(MyStringHash.NullOrEmpty);

		private readonly MyCueId myEmptyCueId = new MyCueId(MyStringHash.NullOrEmpty);

		private MySoundPair m_soundPair = MySoundPair.Empty;

		private IMySourceVoice m_sound;

		private IMySourceVoice m_secondarySound;

		private MyCueId m_secondaryCueEnum = new MyCueId(MyStringHash.NullOrEmpty);

		private float m_secondaryVolumeRatio;

		private bool m_secondaryEnabled;

		private float m_secondaryBaseVolume = 1f;

		private float m_baseVolume = 1f;

		private MyEntity m_entity;

		private Vector3D? m_position;

		private Vector3? m_velocity;

		private List<MyCueId> m_soundsQueue = new List<MyCueId>();

		private bool m_playing2D;

		private bool m_usesDistanceSounds;

		private bool m_useRealisticByDefault;

		private bool m_alwaysHearOnRealistic;

		private MyCueId m_closeSoundCueId = new MyCueId(MyStringHash.NullOrEmpty);

		private MySoundPair m_closeSoundSoundPair = MySoundPair.Empty;

		private bool m_realistic;

		private float m_volumeMultiplier = 1f;

		private bool m_volumeChanging;

		private MySoundData m_lastSoundData;

		private readonly object m_soundUpdateLock = new object();

		private readonly object m_lastSoundDataLock = new object();

		private MyStringHash m_activeEffect = MyStringHash.NullOrEmpty;

		private int m_lastPlayedWaveNumber = -1;

		private float? m_customVolume;

		private readonly Action<IMySourceVoice> m_OnSoundVoiceStopped;

		public Dictionary<int, ConcurrentCachingList<Delegate>> EmitterMethods = new Dictionary<int, ConcurrentCachingList<Delegate>>();

		public bool CanPlayLoopSounds = true;

		private string m_lastNullSetter;

		bool IMy3DSoundEmitter.Realistic => m_realistic;

		public bool Loop { get; private set; }

		public bool IsPlaying => Sound?.IsPlaying ?? false;

		public MyCueId SoundId
		{
			get
			{
				return m_cueEnum;
			}
			set
			{
				if (m_cueEnum != value)
				{
					m_cueEnum = value;
					if (m_cueEnum.Hash == MyStringHash.GetOrCompute("None"))
					{
						Debugger.Break();
					}
				}
			}
		}

		public MySoundData LastSoundData => m_lastSoundData;

		private float RealisticVolumeChange
		{
			get
			{
				if (!m_realistic || m_lastSoundData == null)
				{
					return 1f;
				}
				return m_lastSoundData.RealisticVolumeChange;
			}
		}

		public float VolumeMultiplier
		{
			get
			{
				return m_volumeMultiplier;
			}
			set
			{
				m_volumeMultiplier = value;
				if (Sound != null)
				{
					Sound.VolumeMultiplier = m_volumeMultiplier;
				}
			}
		}

		public MySoundPair SoundPair => m_closeSoundSoundPair;

		public IMySourceVoice Sound => m_sound;

		public Vector3D SourcePosition
		{
			get
			{
				if (m_position.HasValue)
				{
					return m_position.Value;
				}
				if (m_entity != null && MySector.MainCamera != null)
				{
					return m_entity.PositionComp.WorldAABB.Center - MySector.MainCamera.Position;
				}
				return Vector3D.Zero;
			}
		}

		public Vector3 Velocity
		{
			get
			{
				if (m_velocity.HasValue)
				{
					return m_velocity.Value;
				}
				if (m_entity != null)
				{
					if (m_entity.Physics != null)
					{
						return m_entity.Physics.LinearVelocity;
					}
					if (m_entity.Parent != null && m_entity.Parent.Physics != null)
					{
						return m_entity.Parent.Physics.LinearVelocity;
					}
				}
				return Vector3.Zero;
			}
		}

		public MyEntity Entity
		{
			get
			{
				return m_entity;
			}
			set
			{
				m_entity = value;
			}
		}

		public float? CustomMaxDistance { get; set; }

		public float? CustomVolume
		{
			get
			{
				return m_customVolume;
			}
			set
			{
				m_customVolume = value;
				if (m_customVolume.HasValue)
				{
					Sound?.SetVolume(RealisticVolumeChange * m_customVolume.Value);
				}
			}
		}

		public bool Force3D { get; set; }

		public bool Force2D { get; set; }

		public bool Plays2D => m_playing2D;

		public int SourceChannels { get; set; }

		int IMy3DSoundEmitter.LastPlayedWaveNumber
		{
			get
			{
				return m_lastPlayedWaveNumber;
			}
			set
			{
				m_lastPlayedWaveNumber = value;
			}
		}

		object IMy3DSoundEmitter.SyncRoot => m_soundUpdateLock;

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public object DebugData { get; set; }

		public float DopplerScaler { get; private set; }

		public event Action<MyEntity3DSoundEmitter> StoppedPlaying;

		public void SetSound(IMySourceVoice value, [CallerMemberName] string caller = null)
		{
			if (value == null)
			{
				m_lastNullSetter = caller;
			}
			lock (m_soundUpdateLock)
			{
				if (m_sound != null && m_sound != value)
				{
					m_sound.StoppedPlaying = null;
				}
				m_sound = value;
			}
		}

		public void SetPosition(Vector3D? position)
		{
			if (!position.HasValue)
			{
				m_position = position;
			}
			else
			{
				m_position = position.Value - MySector.MainCamera.Position;
			}
		}

		public void SetVelocity(Vector3? velocity)
		{
			m_velocity = velocity;
		}

		public MyEntity3DSoundEmitter(MyEntity entity, bool useStaticList = false, float dopplerScaler = 1f)
		{
			m_entity = entity;
			DopplerScaler = dopplerScaler;
			m_OnSoundVoiceStopped = OnSoundVoiceStopped;
			foreach (object value in Enum.GetValues(typeof(MethodsEnum)))
			{
				EmitterMethods.Add((int)value, new ConcurrentCachingList<Delegate>());
			}
			EmitterMethods[1].Add(new Func<bool>(IsControlledEntity));
			if (MySession.Static != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
			{
				EmitterMethods[0].Add(new Func<bool>(IsInAtmosphere));
				EmitterMethods[0].Add(new Func<bool>(IsCurrentWeapon));
				EmitterMethods[0].Add(new Func<bool>(IsOnSameGrid));
				EmitterMethods[0].Add(new Func<bool>(IsControlledEntity));
				EmitterMethods[1].Add(new Func<bool>(IsCurrentWeapon));
				EmitterMethods[2].Add(new Func<MySoundPair, MyCueId>(SelectCue));
				EmitterMethods[3].Add(new Func<MyStringHash>(SelectEffect));
			}
			UpdateEmitterMethods();
			m_useRealisticByDefault = MySession.Static != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS;
			if (MySession.Static != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS && useStaticList && entity != null && MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE)
			{
				lock (m_entityEmitters)
				{
					m_entityEmitters.Add(this);
				}
			}
		}

		public void Update()
		{
			UpdateEmitterMethods();
			bool isPlaying = IsPlaying;
			if (!CanHearSound())
			{
				if (isPlaying)
				{
					StopSound(forced: true, cleanUp: false);
					SetSound(null, "Update");
				}
				return;
			}
			if (!isPlaying && Loop)
			{
				PlaySound(m_closeSoundSoundPair, stopPrevious: true, skipIntro: true);
			}
			else if (isPlaying && Loop && m_playing2D != ShouldPlay2D() && ((Force2D && !m_playing2D) || (Force3D && m_playing2D)))
			{
				StopSound(forced: true, cleanUp: false);
				PlaySound(m_closeSoundSoundPair, stopPrevious: true, skipIntro: true);
			}
			else if (isPlaying && Loop && !m_playing2D && m_usesDistanceSounds)
			{
				MyCueId myCueId = (m_secondaryEnabled ? m_secondaryCueEnum : myEmptyCueId);
				MyCueId myCueId2 = CheckDistanceSounds(m_closeSoundCueId);
				if (myCueId2 != m_cueEnum || myCueId != m_secondaryCueEnum)
				{
					PlaySoundWithDistance(myCueId2, stopPrevious: true, skipIntro: true, force2D: false, useDistanceCheck: false);
				}
				else if (m_secondaryEnabled)
				{
					Sound?.SetVolume(RealisticVolumeChange * m_baseVolume * (1f - m_secondaryVolumeRatio));
					m_secondarySound?.SetVolume(RealisticVolumeChange * m_secondaryBaseVolume * m_secondaryVolumeRatio);
				}
			}
			if (isPlaying && Loop)
			{
				MyCueId soundId = SelectCue(m_soundPair);
				if (!soundId.Equals(m_cueEnum))
				{
					PlaySoundWithDistance(soundId, stopPrevious: true, skipIntro: true);
				}
				MyStringHash myStringHash = SelectEffect();
				if (m_activeEffect != myStringHash)
				{
					PlaySoundWithDistance(soundId, stopPrevious: true, skipIntro: true);
				}
			}
		}

		public bool FastUpdate(bool silenced)
		{
			if (silenced)
			{
				VolumeMultiplier = Math.Max(0f, m_volumeMultiplier - 0.01f);
				if (m_volumeMultiplier == 0f)
				{
					return false;
				}
			}
			else
			{
				VolumeMultiplier = Math.Min(1f, m_volumeMultiplier + 0.01f);
				if (m_volumeMultiplier == 1f)
				{
					return false;
				}
			}
			return true;
		}

		private void UpdateEmitterMethods()
		{
			foreach (ConcurrentCachingList<Delegate> value in EmitterMethods.Values)
			{
				value.ApplyChanges();
			}
		}

		private bool ShouldPlay2D()
		{
			bool flag = EmitterMethods[1].Count == 0;
			foreach (Delegate item in EmitterMethods[1])
			{
				if ((object)item != null)
				{
					flag |= ((Func<bool>)item)();
				}
			}
			return flag;
		}

		private bool CanHearSound()
		{
			bool flag = false;
			if (EmitterMethods != null && EmitterMethods.TryGetValue(0, out var value))
			{
				flag = value.Count == 0 || (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS && m_alwaysHearOnRealistic);
				if (!flag)
				{
					foreach (Func<bool> item in value)
					{
						if (item != null && item())
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					flag = IsCloseEnough();
				}
			}
			return flag;
		}

		private bool IsOnSameGrid()
		{
			if (Entity == null || Entity.EntityId == 0L)
			{
				return false;
			}
			if (Entity is MyCubeBlock || Entity is MyCubeGrid)
			{
				MyCubeGrid entity = null;
				if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity is MyCockpit)
				{
					entity = (MySession.Static.ControlledEntity.Entity as MyCockpit).CubeGrid;
				}
				else if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.SoundComp != null)
				{
					entity = MySession.Static.LocalCharacter.SoundComp.StandingOnGrid;
				}
				if (entity == null)
				{
					if (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.AtmosphereDetectorComp == null)
					{
						return false;
					}
					if (MySession.Static.LocalCharacter.AtmosphereDetectorComp.InShipOrStation)
					{
						MyEntities.TryGetEntityById((long)MySession.Static.LocalCharacter.OxygenSourceGridEntityId, out entity, allowClosed: false);
					}
				}
				MyCubeGrid myCubeGrid = ((Entity is MyCubeBlock) ? (Entity as MyCubeBlock).CubeGrid : (Entity as MyCubeGrid));
				if (entity == null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.SoundComp != null && MySession.Static.LocalCharacter.SoundComp.StandingOnVoxel != null)
				{
					if (myCubeGrid.IsStatic)
					{
						return true;
					}
					foreach (IMyEntity attachedEntity in myCubeGrid.GridSystems.LandingSystem.GetAttachedEntities())
					{
						if (attachedEntity is MyVoxelBase && attachedEntity as MyVoxelBase == MySession.Static.LocalCharacter.SoundComp.StandingOnVoxel)
						{
							return true;
						}
					}
				}
				if (entity == null)
				{
					return false;
				}
				if (entity == myCubeGrid)
				{
					return true;
				}
				if (MyCubeGridGroups.Static.Physical.HasSameGroup(entity, myCubeGrid))
				{
					return true;
				}
			}
			else if (Entity is MyVoxelBase)
			{
				if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity is MyCockpit)
				{
					return false;
				}
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.SoundComp != null)
				{
					if (MySession.Static.LocalCharacter.SoundComp.StandingOnVoxel == Entity as MyVoxelBase)
					{
						return true;
					}
					if (MySession.Static.LocalCharacter.SoundComp.StandingOnGrid != null)
					{
						if (MySession.Static.LocalCharacter.SoundComp.StandingOnGrid.IsStatic)
						{
							return true;
						}
						foreach (IMyEntity attachedEntity2 in MySession.Static.LocalCharacter.SoundComp.StandingOnGrid.GridSystems.LandingSystem.GetAttachedEntities())
						{
							if (attachedEntity2 is MyVoxelBase && attachedEntity2 as MyVoxelBase == Entity as MyVoxelBase)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private bool IsCurrentWeapon()
		{
			if (Entity is IMyHandheldGunObject<MyDeviceBase>)
			{
				if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity is MyCharacter)
				{
					return (MySession.Static.ControlledEntity.Entity as MyCharacter).CurrentWeapon == Entity;
				}
				return false;
			}
			return false;
		}

		private bool IsCloseEnough()
		{
			if (!m_playing2D)
			{
				return MyAudio.Static.SourceIsCloseEnoughToPlaySound(SourcePosition, SoundId, CustomMaxDistance);
			}
			return true;
		}

		private bool IsInTerminal()
		{
			if (MyGuiScreenTerminal.IsOpen && MyGuiScreenTerminal.InteractedEntity != null)
			{
				return MyGuiScreenTerminal.InteractedEntity == Entity;
			}
			return false;
		}

		private bool IsControlledEntity()
		{
			if (MySession.Static.ControlledEntity != null)
			{
				return m_entity == MySession.Static.ControlledEntity.Entity;
			}
			return false;
		}

		private bool IsBeingWelded()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			if (MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			MyCharacter myCharacter = MySession.Static.ControlledEntity.Entity as MyCharacter;
			if (myCharacter == null)
			{
				return false;
			}
			MyEngineerToolBase myEngineerToolBase = myCharacter.CurrentWeapon as MyEngineerToolBase;
			if (myEngineerToolBase == null)
			{
				return false;
			}
			MyCubeGrid targetGrid = myEngineerToolBase.GetTargetGrid();
			MyCubeBlock myCubeBlock = Entity as MyCubeBlock;
			if (targetGrid == null || myCubeBlock == null || targetGrid != myCubeBlock.CubeGrid || !myEngineerToolBase.HasHitBlock)
			{
				return false;
			}
			MySlimBlock cubeBlock = targetGrid.GetCubeBlock(myEngineerToolBase.TargetCube);
			if (cubeBlock == null)
			{
				return false;
			}
			if (cubeBlock.FatBlock == myCubeBlock)
			{
				return myEngineerToolBase.IsShooting;
			}
			return false;
		}

		private bool IsThereAir()
		{
			if (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.AtmosphereDetectorComp == null)
			{
				return false;
			}
			return !MySession.Static.LocalCharacter.AtmosphereDetectorComp.InVoid;
		}

		private bool IsInAtmosphere()
		{
			if (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.AtmosphereDetectorComp == null)
			{
				return false;
			}
			return MySession.Static.LocalCharacter.AtmosphereDetectorComp.InAtmosphere;
		}

		private MyCueId SelectCue(MySoundPair sound)
		{
			if (m_useRealisticByDefault)
			{
				if (m_lastSoundData == null)
				{
					m_lastSoundData = MyAudio.Static.GetCue(sound.Realistic);
				}
				if (m_lastSoundData != null && m_lastSoundData.AlwaysUseOneMode)
				{
					m_realistic = true;
					return sound.Realistic;
				}
				MyCockpit myCockpit = ((MySession.Static.LocalCharacter != null) ? (MySession.Static.LocalCharacter.Parent as MyCockpit) : null);
				bool flag = myCockpit != null && myCockpit.CubeGrid.GridSizeEnum == MyCubeSize.Large && myCockpit.BlockDefinition.IsPressurized;
				if (IsThereAir() || flag)
				{
					m_realistic = false;
					return sound.Arcade;
				}
				m_realistic = true;
				return sound.Realistic;
			}
			m_realistic = false;
			return sound.Arcade;
		}

		private MyStringHash SelectEffect()
		{
			if (m_lastSoundData != null && !m_lastSoundData.ModifiableByHelmetFilters)
			{
				return MyStringHash.NullOrEmpty;
			}
			if (MySession.Static == null || MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.OxygenComponent == null || !MyFakes.ENABLE_NEW_SOUNDS || !MySession.Static.Settings.RealisticSound)
			{
				return MyStringHash.NullOrEmpty;
			}
			bool flag = IsThereAir();
			MyCockpit myCockpit = MySession.Static.LocalCharacter.Parent as MyCockpit;
			bool flag2 = myCockpit != null && myCockpit.BlockDefinition != null && myCockpit.BlockDefinition.IsPressurized;
			if (flag && flag2)
			{
				return m_effectEnclosedCockpitInAir;
			}
			if (!flag && flag2 && myCockpit.CubeGrid != null && myCockpit.CubeGrid.GridSizeEnum == MyCubeSize.Large)
			{
				return m_effectEnclosedCockpitInSpace;
			}
			if (MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && flag)
			{
				return m_effectHasHelmetInOxygen;
			}
			if (m_lastSoundData != null && MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && !flag)
			{
				return m_lastSoundData.RealisticFilter;
			}
			if (!MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && !flag && (myCockpit == null || myCockpit.BlockDefinition == null || !myCockpit.BlockDefinition.IsPressurized))
			{
				return m_effectNoHelmetNoOxygen;
			}
			if (m_lastSoundData != null && myCockpit != null && myCockpit.BlockDefinition != null && myCockpit.BlockDefinition.IsPressurized && myCockpit.CubeGrid != null && myCockpit.CubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				return m_lastSoundData.RealisticFilter;
			}
			return MyStringHash.NullOrEmpty;
		}

		private bool CheckForSynchronizedSounds()
		{
			if (m_lastSoundData != null && m_lastSoundData.PreventSynchronization >= 0)
			{
				LastTimePlayingData lastTimePlayingData = default(LastTimePlayingData);
				bool flag = LastTimePlaying.TryGetValue(SoundId, ref lastTimePlayingData);
				if (!flag)
				{
					lastTimePlayingData.LastTime = 0;
					lastTimePlayingData.Emitter = this;
					LastTimePlaying.TryAdd(SoundId, lastTimePlayingData);
				}
				int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
				if (sessionTotalFrames - lastTimePlayingData.LastTime <= m_lastSoundData.PreventSynchronization && flag)
				{
					_ = (Vector3D)MyAudio.Static.GetListenerPosition();
					double num = SourcePosition.LengthSquared();
					double num2 = lastTimePlayingData.Emitter.SourcePosition.LengthSquared();
					if (num > num2)
					{
						return false;
					}
				}
				lastTimePlayingData.LastTime = sessionTotalFrames;
				lastTimePlayingData.Emitter = this;
				LastTimePlaying.set_Item(SoundId, lastTimePlayingData);
			}
			return true;
		}

		public void PlaySound(byte[] buffer, float volume = 1f, float maxDistance = 0f, MySoundDimensions dimension = MySoundDimensions.D3)
		{
			MyEntity entity = Entity;
			if (entity != null && !entity.InScene)
			{
				return;
			}
			CustomMaxDistance = maxDistance;
			CustomVolume = volume;
			if (Sound == null || !Sound.IsValid)
			{
				SetSound(MyAudio.Static.GetSound(this, dimension), "PlaySound");
			}
			if (Sound != null)
			{
				Sound.SubmitBuffer(buffer);
				if (!Sound.IsPlaying)
				{
					Sound.StartBuffered();
				}
			}
		}

		public void PlaySingleSound(MyCueId soundId, bool stopPrevious = false, bool skipIntro = false, bool? force3D = null)
		{
			if (!(m_cueEnum == soundId))
			{
				PlaySoundWithDistance(soundId, stopPrevious, skipIntro, force2D: false, useDistanceCheck: true, alwaysHearOnRealistic: false, skipToEnd: false, force3D);
			}
		}

		public bool PlaySingleSound(MySoundPair soundId, bool stopPrevious = false, bool skipIntro = false, bool skipToEnd = false, bool? force3D = null)
		{
			MyEntity entity = Entity;
			if (entity != null && !entity.InScene)
			{
				return false;
			}
			m_closeSoundSoundPair = soundId;
			m_soundPair = soundId;
			MyCueId myCueId = (m_useRealisticByDefault ? soundId.Realistic : soundId.Arcade);
			if (EmitterMethods[2].Count > 0)
			{
				myCueId = ((Func<MySoundPair, MyCueId>)EmitterMethods[2][0])(soundId);
			}
			if (m_cueEnum.Equals(myCueId))
			{
				return true;
			}
			PlaySoundWithDistance(myCueId, stopPrevious, skipIntro, force2D: false, useDistanceCheck: true, alwaysHearOnRealistic: false, skipToEnd, force3D);
			return true;
		}

		public bool PlaySound(MySoundPair soundId, bool stopPrevious = false, bool skipIntro = false, bool force2D = false, bool alwaysHearOnRealistic = false, bool skipToEnd = false, bool? force3D = null)
		{
			MyEntity entity = Entity;
			if (entity != null && !entity.InScene)
			{
				return false;
			}
			m_closeSoundSoundPair = soundId;
			m_soundPair = soundId;
			MyCueId soundId2 = (m_useRealisticByDefault ? soundId.Realistic : soundId.Arcade);
			if (EmitterMethods[2].Count > 0)
			{
				soundId2 = ((Func<MySoundPair, MyCueId>)EmitterMethods[2][0])(soundId);
			}
			PlaySoundWithDistance(soundId2, stopPrevious, skipIntro, force2D, useDistanceCheck: true, alwaysHearOnRealistic, skipToEnd, force3D);
			return true;
		}

		public bool PlaySoundWithDistance(MyCueId soundId, bool stopPrevious = false, bool skipIntro = false, bool force2D = false, bool useDistanceCheck = true, bool alwaysHearOnRealistic = false, bool skipToEnd = false, bool? force3D = null)
		{
<<<<<<< HEAD
			if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
=======
			if (Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return false;
			}
			m_lastSoundData = MyAudio.Static.GetCue(soundId);
			if (useDistanceCheck)
			{
				m_closeSoundCueId = soundId;
				soundId = CheckDistanceSounds(soundId);
			}
			bool usesDistanceSounds = m_usesDistanceSounds;
			if (Sound != null)
			{
				if (stopPrevious)
				{
					StopSound(forced: true);
				}
				else if (Loop)
				{
					IMySourceVoice sound = Sound;
					StopSound(forced: true);
					m_soundsQueue.Add(sound.CueEnum);
				}
			}
			if (m_secondarySound != null)
			{
				m_secondarySound.Stop(force: true);
			}
			SoundId = soundId;
			bool skipIntro2 = skipIntro || skipToEnd;
			bool force2D2 = force2D;
			bool alwaysHearOnRealistic2 = alwaysHearOnRealistic;
			PlaySoundInternal(skipIntro2, skipToEnd, force2D2, alwaysHearOnRealistic2, force3D);
			m_usesDistanceSounds = usesDistanceSounds;
			return true;
		}

		private MyCueId CheckDistanceSounds(MyCueId soundId)
		{
			if (!soundId.IsNull)
			{
				lock (m_lastSoundDataLock)
				{
					if (m_lastSoundData != null && m_lastSoundData.DistantSounds != null && m_lastSoundData.DistantSounds.Count > 0)
					{
						double num = SourcePosition.LengthSquared();
						int num2 = -1;
						m_usesDistanceSounds = true;
						m_secondaryEnabled = false;
						for (int i = 0; i < m_lastSoundData.DistantSounds.Count; i++)
						{
							double num3 = m_lastSoundData.DistantSounds[i].Distance * m_lastSoundData.DistantSounds[i].Distance;
							if (num > num3)
							{
								num2 = i;
								continue;
							}
							float num4 = ((m_lastSoundData.DistantSounds[i].DistanceCrossfade >= 0f) ? (m_lastSoundData.DistantSounds[i].DistanceCrossfade * m_lastSoundData.DistantSounds[i].DistanceCrossfade) : float.MaxValue);
							if (!(num > (double)num4))
							{
								break;
							}
							m_secondaryVolumeRatio = (float)((num - (double)num4) / (num3 - (double)num4));
							m_secondaryEnabled = true;
							MySoundPair mySoundPair = new MySoundPair(m_lastSoundData.DistantSounds[i].Sound);
							if (mySoundPair != MySoundPair.Empty)
							{
								m_secondaryCueEnum = SelectCue(mySoundPair);
							}
							else if (num2 >= 0)
							{
								m_secondaryCueEnum = new MyCueId(MyStringHash.GetOrCompute(m_lastSoundData.DistantSounds[num2].Sound));
							}
							else
							{
								m_secondaryEnabled = false;
							}
						}
						if (num2 >= 0)
						{
							MySoundPair mySoundPair2 = new MySoundPair(m_lastSoundData.DistantSounds[num2].Sound);
							if (mySoundPair2 != MySoundPair.Empty)
							{
								m_soundPair = mySoundPair2;
								soundId = SelectCue(m_soundPair);
							}
							else
							{
								soundId = new MyCueId(MyStringHash.GetOrCompute(m_lastSoundData.DistantSounds[num2].Sound));
							}
						}
						else
						{
							m_soundPair = m_closeSoundSoundPair;
						}
					}
					else
					{
						m_usesDistanceSounds = false;
					}
				}
			}
			if (!m_secondaryEnabled)
			{
				m_secondaryCueEnum = myEmptyCueId;
			}
			return soundId;
		}

		private void PlaySoundInternal(bool skipIntro = false, bool skipToEnd = false, bool force2D = false, bool alwaysHearOnRealistic = false, bool? force3D = null)
		{
			Force2D = force2D;
			if (force3D.HasValue)
			{
				Force3D = force3D.Value;
			}
			m_alwaysHearOnRealistic = alwaysHearOnRealistic;
			Loop = false;
			try
			{
				lock (m_soundUpdateLock)
				{
					if (!SoundId.IsNull && CheckForSynchronizedSounds())
					{
						m_playing2D = (ShouldPlay2D() && !Force3D) || Force2D;
						Loop = MyAudio.Static.IsLoopable(SoundId) && !skipToEnd && CanPlayLoopSounds;
						if (Loop && MySession.Static.ElapsedPlayTime.TotalSeconds < 6.0)
						{
							skipIntro = true;
						}
						if (m_playing2D)
						{
							SetSound(MyAudio.Static.PlaySound(m_closeSoundCueId, this, MySoundDimensions.D2, skipIntro, skipToEnd), "PlaySoundInternal");
						}
						else if (CanHearSound())
						{
							SetSound(MyAudio.Static.PlaySound(SoundId, this, MySoundDimensions.D3, skipIntro, skipToEnd), "PlaySoundInternal");
						}
					}
					if (!IsPlaying)
					{
						return;
					}
					if (MyMusicController.Static != null && m_lastSoundData != null && m_lastSoundData.DynamicMusicCategory != MyStringId.NullOrEmpty && m_lastSoundData.DynamicMusicAmount > 0)
					{
						MyMusicController.Static.IncreaseCategory(m_lastSoundData.DynamicMusicCategory, m_lastSoundData.DynamicMusicAmount);
					}
					m_baseVolume = Sound.Volume;
					Sound.SetVolume(Sound.Volume * RealisticVolumeChange);
					if (!m_secondaryEnabled)
					{
						goto IL_024a;
					}
					_ = m_secondaryCueEnum;
					m_secondarySound = MyAudio.Static.PlaySound(m_secondaryCueEnum, this, MySoundDimensions.D3, skipIntro, skipToEnd);
					if (Sound == null)
					{
						return;
					}
					if (m_secondarySound != null)
					{
						m_secondaryBaseVolume = m_secondarySound.Volume;
						Sound.SetVolume(RealisticVolumeChange * m_baseVolume * (1f - m_secondaryVolumeRatio));
						m_secondarySound.SetVolume(RealisticVolumeChange * m_secondaryBaseVolume * m_secondaryVolumeRatio);
						m_secondarySound.VolumeMultiplier = m_volumeMultiplier;
					}
					goto IL_024a;
					IL_024a:
					Sound.VolumeMultiplier = m_volumeMultiplier;
					Sound.StoppedPlaying = m_OnSoundVoiceStopped;
					if (EmitterMethods[3].Count <= 0)
					{
						return;
					}
					m_activeEffect = MyStringHash.NullOrEmpty;
					MyStringHash myStringHash = ((Func<MyStringHash>)EmitterMethods[3][0])();
					if (myStringHash != MyStringHash.NullOrEmpty)
					{
						IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(Sound, myStringHash);
						if (myAudioEffect != null)
						{
							SetSound(myAudioEffect.OutputSound, "PlaySoundInternal");
							m_activeEffect = myStringHash;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyLog.Default.WriteLine("Last null written by: " + m_lastNullSetter);
				throw;
			}
		}

		public void StopSound(bool forced, bool cleanUp = true, bool cleanupSound = false)
		{
			m_usesDistanceSounds = false;
			if (Sound != null)
			{
				Sound.Stop(forced);
				if (Loop && !forced)
				{
					PlaySoundInternal(skipIntro: true, skipToEnd: true);
				}
				if (m_soundsQueue.Count == 0)
				{
					if (cleanupSound)
					{
						Cleanup();
					}
					SetSound(null, "StopSound");
					if (cleanUp)
					{
						Loop = false;
						SoundId = myEmptyCueId;
					}
				}
				else if (cleanUp)
				{
					SoundId = m_soundsQueue[0];
					PlaySoundInternal(skipIntro: true);
					m_soundsQueue.RemoveAt(0);
				}
			}
			else if (cleanUp)
			{
				Loop = false;
				SoundId = myEmptyCueId;
			}
			if (m_secondarySound != null)
			{
				m_secondarySound.Stop(force: true);
			}
		}

		public void Cleanup()
		{
			if (Sound != null)
			{
				Sound.Destroy();
				SetSound(null, "Cleanup");
			}
			if (m_secondarySound != null)
			{
				m_secondarySound.Destroy();
				m_secondarySound = null;
			}
			_ = m_secondaryCueEnum;
			m_secondaryCueEnum = new MyCueId(MyStringHash.NullOrEmpty);
		}

<<<<<<< HEAD
		public void ClearSecondaryCue()
		{
			_ = m_secondaryCueEnum;
			m_secondaryCueEnum = new MyCueId(MyStringHash.NullOrEmpty);
		}

		internal void CleanEmitterMethods()
		{
=======
		internal void CleanEmitterMethods()
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			EmitterMethods.Clear();
		}

		private void OnSoundVoiceStopped(IMySourceVoice voice)
		{
			voice.StoppedPlaying = (Action<IMySourceVoice>)Delegate.Remove(voice.StoppedPlaying, m_OnSoundVoiceStopped);
			OnStopPlaying();
			if (m_sound == voice)
			{
				SetSound(null, "OnSoundVoiceStopped");
			}
			else if (m_secondarySound == voice)
			{
				m_secondarySound = null;
			}
		}

		private void OnStopPlaying()
		{
			this.StoppedPlaying.InvokeIfNotNull(this);
		}

		public static void PreloadSound(MySoundPair soundId)
		{
			IMySourceVoice sound = MyAudio.Static.GetSound(soundId.SoundId);
			if (sound != null)
			{
				sound.Start(skipIntro: false);
				sound.Stop(force: true);
			}
		}

		public static void UpdateEntityEmitters(bool removeUnused, bool updatePlaying, bool updateNotPlaying)
		{
			int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
			if (sessionTotalFrames == 0 || Math.Abs(m_lastUpdate - sessionTotalFrames) < 5)
			{
				return;
			}
			m_lastUpdate = sessionTotalFrames;
			lock (m_entityEmitters)
			{
				for (int i = 0; i < m_entityEmitters.Count; i++)
				{
					if (m_entityEmitters[i] != null && m_entityEmitters[i].Entity != null && !m_entityEmitters[i].Entity.Closed)
					{
						if ((m_entityEmitters[i].IsPlaying && updatePlaying) || (!m_entityEmitters[i].IsPlaying && updateNotPlaying))
						{
							m_entityEmitters[i].Update();
						}
					}
					else if (removeUnused)
					{
						m_entityEmitters.RemoveAt(i);
						i--;
					}
				}
			}
		}

		public static void ClearEntityEmitters()
		{
			lock (m_entityEmitters)
			{
				m_entityEmitters.Clear();
			}
		}
	}
}
