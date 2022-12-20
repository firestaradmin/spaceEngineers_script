using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_CharacterSoundComponent), true)]
	public class MyCharacterSoundComponent : MyCharacterComponent
	{
		private enum MySoundEmitterEnum
		{
			PrimaryState,
			SecondaryState,
			WalkState,
			Action,
			IdleJetState,
			JumpState,
			FallState
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct MovementSoundType
		{
			public static readonly MyStringId Walk = MyStringId.GetOrCompute("Walk");

			public static readonly MyStringId CrouchWalk = MyStringId.GetOrCompute("CrouchWalk");

			public static readonly MyStringId Run = MyStringId.GetOrCompute("Run");

			public static readonly MyStringId Sprint = MyStringId.GetOrCompute("Sprint");

			public static readonly MyStringId Fall = MyStringId.GetOrCompute("Fall");
		}

		private class Sandbox_Game_Components_MyCharacterSoundComponent_003C_003EActor : IActivator, IActivator<MyCharacterSoundComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterSoundComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterSoundComponent CreateInstance()
			{
				return new MyCharacterSoundComponent();
			}

			MyCharacterSoundComponent IActivator<MyCharacterSoundComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Dictionary<int, MySoundPair> CharacterSounds = new Dictionary<int, MySoundPair>();

		private static readonly MySoundPair EmptySoundPair = new MySoundPair();

		private static MyStringHash LowPressure = MyStringHash.GetOrCompute("LowPressure");

		private List<MyEntity3DSoundEmitter> m_soundEmitters;

		private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();

		private int m_lastScreamTime;

		private float m_jetpackSustainTimer;

		private float m_jetpackMinIdleTime;

		private const float JETPACK_TIME_BETWEEN_SOUNDS = 0.25f;

		private bool m_jumpReady;

		private const int SCREAM_DELAY_MS = 800;

		private const float DEFAULT_ANKLE_HEIGHT = 0.2f;

		private int m_lastStepTime;

		private int m_lastFootSound;

		private MyCharacterMovementEnum m_lastUpdateMovementState;

		private MyCharacter m_character;

		private MyCubeGrid m_standingOnGrid;

		private int m_lastContactCounter;

		private MyVoxelBase m_standingOnVoxel;

		private MyStringHash m_characterPhysicalMaterial = MyMaterialType.CHARACTER;

		private bool m_isWalking;

		private const float WIND_SPEED_LOW = 40f;

		private const float WIND_SPEED_HIGH = 80f;

		private const float WIND_SPEED_DIFF = 40f;

		private const float WIND_CHANGE_SPEED = 0.008333334f;

		private float m_windVolume;

		private bool m_windVolumeChanged;

		private float m_windTargetVolume;

		private bool m_inAtmosphere = true;

		private MyEntity3DSoundEmitter m_windEmitter;

		private bool m_windSystem;

		private MyEntity3DSoundEmitter m_oxygenEmitter;

		private MyEntity3DSoundEmitter m_movementEmitter;

		private MyEntity3DSoundEmitter m_magneticBootsEmitter;

		private MySoundPair m_lastActionSound;

		private MySoundPair m_lastPrimarySound;

		private MySoundPair m_selectedStateSound;

		private bool m_isFirstPerson;

		private bool m_isFirstPersonChanged;

		private bool m_needsUpdateEmitters;

<<<<<<< HEAD
=======
		private int Update;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyCubeGrid StandingOnGrid => m_standingOnGrid;

		public MyVoxelBase StandingOnVoxel => m_standingOnVoxel;

		private bool ShouldUpdateSoundEmitters
		{
			get
			{
				if (m_character == MySession.Static.LocalCharacter && m_character.AtmosphereDetectorComp != null && !m_character.AtmosphereDetectorComp.InAtmosphere && MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound)
				{
					return MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE;
				}
				return false;
			}
		}

		private bool IsInvulnerable
		{
			get
			{
				MySession @static = MySession.Static;
				MyPlayer.PlayerId? playerId = @static.ControlledEntity?.ControllerInfo?.Controller?.Player?.Id;
				if (playerId.HasValue && playerId.GetValueOrDefault().SerialId == 0 && @static.RemoteAdminSettings.TryGetValue(playerId.Value.SteamId, out var value) && (value & AdminSettingsEnum.Invulnerable) != 0)
				{
					return true;
				}
				return false;
			}
		}

		public override string ComponentTypeDebugString => "CharacterSound";

		public MyCharacterSoundComponent()
		{
			m_soundEmitters = new List<MyEntity3DSoundEmitter>(Enum.GetNames(typeof(MySoundEmitterEnum)).Length);
			string[] names = Enum.GetNames(typeof(MySoundEmitterEnum));
			for (int i = 0; i < names.Length; i++)
			{
				_ = names[i];
				m_soundEmitters.Add(new MyEntity3DSoundEmitter(base.Entity as MyEntity));
			}
			for (int j = 0; j < Enum.GetNames(typeof(CharacterSoundsEnum)).Length; j++)
			{
				CharacterSounds.Add(j, EmptySoundPair);
			}
			if (MySession.Static != null && (MySession.Static.Settings.EnableOxygen || MySession.Static.CreativeMode))
			{
				m_oxygenEmitter = new MyEntity3DSoundEmitter(base.Entity as MyEntity);
			}
		}

		private void InitSounds()
		{
			if (m_character.Definition.JumpSoundName != null)
			{
				CharacterSounds[0] = new MySoundPair(m_character.Definition.JumpSoundName);
			}
			if (m_character.Definition.JetpackIdleSoundName != null)
			{
				CharacterSounds[1] = new MySoundPair(m_character.Definition.JetpackIdleSoundName);
			}
			if (m_character.Definition.JetpackRunSoundName != null)
			{
				CharacterSounds[2] = new MySoundPair(m_character.Definition.JetpackRunSoundName);
			}
			if (m_character.Definition.CrouchDownSoundName != null)
			{
				CharacterSounds[3] = new MySoundPair(m_character.Definition.CrouchDownSoundName);
			}
			if (m_character.Definition.CrouchUpSoundName != null)
			{
				CharacterSounds[4] = new MySoundPair(m_character.Definition.CrouchUpSoundName);
			}
			if (m_character.Definition.PainSoundName != null)
			{
				CharacterSounds[5] = new MySoundPair(m_character.Definition.PainSoundName);
			}
			if (m_character.Definition.SuffocateSoundName != null)
			{
				CharacterSounds[6] = new MySoundPair(m_character.Definition.SuffocateSoundName);
			}
			if (m_character.Definition.DeathSoundName != null)
			{
				CharacterSounds[7] = new MySoundPair(m_character.Definition.DeathSoundName);
			}
			if (m_character.Definition.DeathBySuffocationSoundName != null)
			{
				CharacterSounds[8] = new MySoundPair(m_character.Definition.DeathBySuffocationSoundName);
			}
			if (m_character.Definition.IronsightActSoundName != null)
			{
				CharacterSounds[9] = new MySoundPair(m_character.Definition.IronsightActSoundName);
			}
			if (m_character.Definition.IronsightDeactSoundName != null)
			{
				CharacterSounds[10] = new MySoundPair(m_character.Definition.IronsightDeactSoundName);
			}
			if (m_character.Definition.FastFlySoundName != null)
			{
				m_windEmitter = new MyEntity3DSoundEmitter(base.Entity as MyEntity);
				m_windEmitter.Force3D = false;
				m_windSystem = true;
				CharacterSounds[11] = new MySoundPair(m_character.Definition.FastFlySoundName);
			}
			if (m_character.Definition.HelmetOxygenNormalSoundName != null)
			{
				CharacterSounds[12] = new MySoundPair(m_character.Definition.HelmetOxygenNormalSoundName);
			}
			if (m_character.Definition.HelmetOxygenLowSoundName != null)
			{
				CharacterSounds[13] = new MySoundPair(m_character.Definition.HelmetOxygenLowSoundName);
			}
			if (m_character.Definition.HelmetOxygenCriticalSoundName != null)
			{
				CharacterSounds[14] = new MySoundPair(m_character.Definition.HelmetOxygenCriticalSoundName);
			}
			if (m_character.Definition.HelmetOxygenNoneSoundName != null)
			{
				CharacterSounds[15] = new MySoundPair(m_character.Definition.HelmetOxygenNoneSoundName);
			}
			if (m_character.Definition.MovementSoundName != null)
			{
				CharacterSounds[16] = new MySoundPair(m_character.Definition.MovementSoundName);
				m_movementEmitter = new MyEntity3DSoundEmitter(base.Entity as MyEntity);
			}
			if (!string.IsNullOrEmpty(m_character.Definition.MagnetBootsStepsSoundName) || !string.IsNullOrEmpty(m_character.Definition.MagnetBootsStartSoundName) || !string.IsNullOrEmpty(m_character.Definition.MagnetBootsEndSoundName) || !string.IsNullOrEmpty(m_character.Definition.MagnetBootsProximitySoundName))
			{
				CharacterSounds[17] = new MySoundPair(m_character.Definition.MagnetBootsStepsSoundName);
				CharacterSounds[18] = new MySoundPair(m_character.Definition.MagnetBootsStartSoundName);
				CharacterSounds[19] = new MySoundPair(m_character.Definition.MagnetBootsEndSoundName);
				CharacterSounds[20] = new MySoundPair(m_character.Definition.MagnetBootsProximitySoundName);
				m_magneticBootsEmitter = new MyEntity3DSoundEmitter(base.Entity as MyEntity);
			}
		}

		public void Preload()
		{
			foreach (MySoundPair value in CharacterSounds.Values)
			{
				MyEntity3DSoundEmitter.PreloadSound(value);
			}
		}

		public void CharacterDied()
		{
			if (m_windEmitter.IsPlaying)
			{
				m_windEmitter.StopSound(forced: true);
			}
		}

		public void UpdateWindSounds()
		{
			if (m_character.IsDead && m_windVolume > 0f)
			{
				m_windVolume = 0f;
				m_windVolumeChanged = m_windVolume != m_windTargetVolume;
			}
			if (!m_windSystem || m_character.IsDead)
			{
				return;
			}
			if (m_inAtmosphere)
			{
				float num = m_character.Physics.LinearVelocity.Length();
				if (num < 40f)
				{
					m_windTargetVolume = 0f;
				}
				else if (num < 80f)
				{
					m_windTargetVolume = (num - 40f) / 40f;
				}
				else
				{
					m_windTargetVolume = 1f;
				}
			}
			else
			{
				m_windTargetVolume = 0f;
			}
			m_windVolumeChanged = m_windVolume != m_windTargetVolume;
			if (m_windVolume < m_windTargetVolume)
			{
				m_windVolume = Math.Min(m_windVolume + 0.008333334f, m_windTargetVolume);
			}
			else if (m_windVolume > m_windTargetVolume)
			{
				m_windVolume = Math.Max(m_windVolume - 0.008333334f, m_windTargetVolume);
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void UpdateAfterSimulationParallel()
		{
			if (!base.Entity.MarkedForClose && m_character?.Physics != null)
			{
				if (m_needsUpdateEmitters)
				{
					MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: true, updateNotPlaying: false);
				}
				m_selectedStateSound = SelectSound();
				UpdateBreath();
				UpdateWindSounds();
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			PlayStateSound();
			UpdateWindVolumeAndPlayback();
		}

		private void UpdateWindVolumeAndPlayback()
		{
			if (m_windEmitter.IsPlaying)
			{
				if (m_windVolume <= 0f)
				{
					m_windEmitter.StopSound(forced: true);
				}
				else
				{
					m_windEmitter.CustomVolume = m_windVolume;
				}
			}
			else if (m_windVolume > 0f)
			{
				m_windEmitter.PlaySound(CharacterSounds[11], stopPrevious: true, skipIntro: false, force2D: true);
				m_windEmitter.CustomVolume = m_windVolume;
			}
			if (m_windVolumeChanged)
			{
				m_windVolumeChanged = false;
				MySessionComponentPlanetAmbientSounds component = MySession.Static.GetComponent<MySessionComponentPlanetAmbientSounds>();
				if (component != null)
				{
					component.VolumeModifierGlobal = 1f - m_windVolume;
				}
			}
		}

		public void UpdateAfterSimulation100()
		{
			UpdateOxygenSounds();
			m_soundEmitters[0].Update();
			m_soundEmitters[4].Update();
			if (m_windSystem)
			{
				m_inAtmosphere = m_character.AtmosphereDetectorComp != null && m_character.AtmosphereDetectorComp.InAtmosphere;
				m_windEmitter.Update();
			}
			if (m_oxygenEmitter != null)
			{
				m_oxygenEmitter.Update();
			}
		}

		public void PlayActionSound(MySoundPair actionSound, bool? force3D = null)
		{
			m_lastActionSound = actionSound;
			m_soundEmitters[3].PlaySound(m_lastActionSound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, force3D);
		}

		private void PlayStateSound()
		{
			if ((m_character.IsClientOnline.HasValue && !m_character.IsClientOnline.Value) || base.Entity.MarkedForClose || m_character?.Physics == null)
			{
				return;
			}
			if (Sync.IsDedicated)
			{
				UpdateBreath();
				return;
			}
			if (m_isFirstPerson != (MySession.Static.LocalCharacter?.IsInFirstPersonView ?? false))
			{
				m_isFirstPerson = !m_isFirstPerson;
				m_isFirstPersonChanged = true;
			}
			else
			{
				m_isFirstPersonChanged = false;
			}
			m_character.Breath?.Update();
			MySoundPair selectedStateSound = m_selectedStateSound;
			if (m_movementEmitter != null && !CharacterSounds[16].Equals(MySoundPair.Empty))
			{
				if (m_isWalking && !m_movementEmitter.IsPlaying)
				{
					m_movementEmitter.PlaySound(CharacterSounds[16], stopPrevious: false, skipIntro: false, MyFakes.FORCE_CHARACTER_2D_SOUND, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
				}
				if (!m_isWalking && m_movementEmitter.IsPlaying)
				{
					m_movementEmitter.StopSound(forced: false);
				}
			}
			MyEntity3DSoundEmitter myEntity3DSoundEmitter = m_soundEmitters[0];
			MyEntity3DSoundEmitter myEntity3DSoundEmitter2 = m_soundEmitters[4];
			MyEntity3DSoundEmitter myEntity3DSoundEmitter3 = m_soundEmitters[2];
			bool num = selectedStateSound.Equals(myEntity3DSoundEmitter.SoundPair) && myEntity3DSoundEmitter.IsPlaying;
			if (m_isFirstPersonChanged)
			{
				myEntity3DSoundEmitter.StopSound(forced: true);
				bool isPlaying = myEntity3DSoundEmitter2.IsPlaying;
				myEntity3DSoundEmitter2.StopSound(forced: true);
				if (isPlaying)
				{
					myEntity3DSoundEmitter2.PlaySound(CharacterSounds[1], stopPrevious: false, skipIntro: true, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND);
				}
			}
			IMySourceVoice sound = myEntity3DSoundEmitter.Sound;
			MySoundData lastSoundData = myEntity3DSoundEmitter.LastSoundData;
			if (sound != null && lastSoundData != null)
			{
				float num2 = MathHelper.Clamp((m_character.Physics?.LinearVelocity.Length()).Value / 7.5f, 0.1f, 1f);
				float volume = lastSoundData.Volume * num2;
				sound.SetVolume(volume);
			}
			if (!num && (!m_isWalking || m_character.Definition.LoopingFootsteps))
			{
				MyCharacter myCharacter = base.Entity as MyCharacter;
				if (selectedStateSound != EmptySoundPair && selectedStateSound == CharacterSounds[2])
				{
					if (m_jetpackSustainTimer >= 0.25f)
					{
						if (myEntity3DSoundEmitter.Loop)
						{
							myEntity3DSoundEmitter.StopSound(forced: true);
						}
						myEntity3DSoundEmitter.PlaySound(selectedStateSound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND);
					}
				}
				else if (!myEntity3DSoundEmitter2.IsPlaying && selectedStateSound != EmptySoundPair && myCharacter != null && myCharacter.JetpackRunning)
				{
					if (m_jetpackSustainTimer <= 0f || selectedStateSound != CharacterSounds[1])
					{
						myEntity3DSoundEmitter2.PlaySound(CharacterSounds[1], stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND);
					}
				}
				else if (selectedStateSound == EmptySoundPair)
				{
					foreach (MyEntity3DSoundEmitter soundEmitter in m_soundEmitters)
					{
						if (soundEmitter.Loop)
						{
							soundEmitter.StopSound(forced: false);
						}
					}
				}
				else if (selectedStateSound != m_lastPrimarySound || (selectedStateSound != CharacterSounds[3] && selectedStateSound != CharacterSounds[4]))
				{
					if (myEntity3DSoundEmitter.Loop)
					{
						myEntity3DSoundEmitter.StopSound(forced: false);
					}
					if (selectedStateSound == CharacterSounds[2])
					{
						myEntity3DSoundEmitter.PlaySound(selectedStateSound, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND);
					}
					else if (selectedStateSound != CharacterSounds[1])
					{
						myEntity3DSoundEmitter.PlaySound(selectedStateSound, stopPrevious: true);
					}
				}
			}
			else if (!m_character.Definition.LoopingFootsteps && myEntity3DSoundEmitter3 != null && selectedStateSound != null)
			{
				IKFeetStepSounds(myEntity3DSoundEmitter3, selectedStateSound, m_isWalking && m_character.IsMagneticBootsEnabled);
			}
			if (m_character.JetpackComp != null && !m_character.JetpackComp.IsFlying && m_character.JetpackComp.DampenersEnabled && !m_character.Physics.LinearVelocity.Equals(Vector3.Zero))
			{
				float num3 = 0.98f;
				myEntity3DSoundEmitter.VolumeMultiplier *= num3;
			}
			else
			{
				myEntity3DSoundEmitter.VolumeMultiplier = 1f;
			}
			m_lastPrimarySound = selectedStateSound;
		}

		private void IKFeetStepSounds(MyEntity3DSoundEmitter walkEmitter, MySoundPair cueEnum, bool magneticBootsOn)
		{
			MyCharacterMovementEnum currentMovementState = m_character.GetCurrentMovementState();
			_ = m_character.IsCrouching;
			if (currentMovementState.GetSpeed() != m_lastUpdateMovementState.GetSpeed())
			{
				walkEmitter.StopSound(forced: true);
				m_lastStepTime = 0;
			}
			int num = int.MaxValue;
			if (currentMovementState.GetDirection() != 0)
			{
				switch (currentMovementState)
				{
				case MyCharacterMovementEnum.Crouching:
				case MyCharacterMovementEnum.Walking:
				case MyCharacterMovementEnum.CrouchWalking:
				case MyCharacterMovementEnum.BackWalking:
				case MyCharacterMovementEnum.CrouchBackWalking:
				case MyCharacterMovementEnum.WalkStrafingLeft:
				case MyCharacterMovementEnum.CrouchStrafingLeft:
				case MyCharacterMovementEnum.WalkingLeftFront:
				case MyCharacterMovementEnum.CrouchWalkingLeftFront:
				case MyCharacterMovementEnum.WalkingLeftBack:
				case MyCharacterMovementEnum.CrouchWalkingLeftBack:
				case MyCharacterMovementEnum.WalkStrafingRight:
				case MyCharacterMovementEnum.CrouchStrafingRight:
				case MyCharacterMovementEnum.WalkingRightFront:
				case MyCharacterMovementEnum.CrouchWalkingRightFront:
				case MyCharacterMovementEnum.WalkingRightBack:
				case MyCharacterMovementEnum.CrouchWalkingRightBack:
				case MyCharacterMovementEnum.Running:
				case MyCharacterMovementEnum.Backrunning:
				case MyCharacterMovementEnum.RunStrafingLeft:
				case MyCharacterMovementEnum.RunningLeftFront:
				case MyCharacterMovementEnum.RunningLeftBack:
				case MyCharacterMovementEnum.RunStrafingRight:
				case MyCharacterMovementEnum.RunningRightFront:
				case MyCharacterMovementEnum.RunningRightBack:
				case MyCharacterMovementEnum.Sprinting:
				case MyCharacterMovementEnum.CrouchRotatingLeft:
				case MyCharacterMovementEnum.CrouchRotatingRight:
					num = 100;
					break;
				}
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastStepTime > num)
			{
				int index;
				MyCharacterBone myCharacterBone = ((m_character.AnimationController != null) ? m_character.AnimationController.FindBone(m_character.Definition.LeftAnkleBoneName, out index) : null);
				int index2;
				MyCharacterBone myCharacterBone2 = ((m_character.AnimationController != null) ? m_character.AnimationController.FindBone(m_character.Definition.RightAnkleBoneName, out index2) : null);
				Vector3 vector = myCharacterBone?.AbsoluteTransform.Translation ?? m_character.PositionComp.LocalAABB.Center;
				Vector3 obj = myCharacterBone2?.AbsoluteTransform.Translation ?? m_character.PositionComp.LocalAABB.Center;
				float ankleHeightWhileStanding = m_character.Definition.AnkleHeightWhileStanding;
				float value = 0f;
				if (m_character.AnimationController != null)
				{
					m_character.AnimationController.Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeed, out value);
				}
				bool flag = vector.Y - ankleHeightWhileStanding < m_character.PositionComp.LocalAABB.Min.Y;
				bool flag2 = obj.Y - ankleHeightWhileStanding < m_character.PositionComp.LocalAABB.Min.Y;
				if (flag || flag2)
				{
					if (value > 0f)
					{
						if (!(flag && flag2))
						{
							if ((flag ? 1 : (-1)) != m_lastFootSound)
							{
								m_lastFootSound = (flag ? 1 : (-1));
								if (MyFakes.CHARACTER_FOOTS_DEBUG_DRAW)
								{
									MyRenderProxy.DebugDrawPoint(base.Character.WorldMatrix.Translation, flag ? Color.Green : Color.Yellow, depthRead: true, persistent: true);
								}
								walkEmitter.PlaySound(cueEnum, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
								if (walkEmitter.Sound != null)
								{
									if (magneticBootsOn)
									{
										walkEmitter.Sound.FrequencyRatio = walkEmitter.Sound.FrequencyRatio * 0.95f;
										if (m_magneticBootsEmitter != null && CharacterSounds[17] != MySoundPair.Empty)
										{
											m_magneticBootsEmitter.PlaySound(CharacterSounds[17], stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
										}
									}
									else
									{
										walkEmitter.Sound.FrequencyRatio = 1f;
									}
								}
								m_lastStepTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
							}
						}
						else
						{
							m_lastFootSound = 0;
							if (MyFakes.CHARACTER_FOOTS_DEBUG_DRAW)
							{
								MyRenderProxy.DebugDrawPoint(base.Character.WorldMatrix.Translation, Color.Red, depthRead: true, persistent: true);
							}
						}
					}
				}
				else if (MyFakes.CHARACTER_FOOTS_DEBUG_DRAW)
				{
					MyRenderProxy.DebugDrawPoint(base.Character.WorldMatrix.Translation, Color.Purple, depthRead: true, persistent: true);
				}
			}
			m_lastUpdateMovementState = currentMovementState;
		}

		public bool StopStateSound(bool forceStop = true)
		{
			m_soundEmitters[0].StopSound(forceStop);
			return true;
		}

		public void PlaySecondarySound(CharacterSoundsEnum soundEnum, bool stopPrevious = false, bool force2D = false, bool? force3D = null)
		{
			m_soundEmitters[1].PlaySound(CharacterSounds[(int)soundEnum], stopPrevious, skipIntro: false, force2D, alwaysHearOnRealistic: false, skipToEnd: false, force3D);
		}

		public void PlayDeathSound(MyStringHash damageType, bool stopPrevious = false)
		{
			if (damageType == LowPressure)
			{
				m_soundEmitters[1].PlaySound(CharacterSounds[8], stopPrevious);
			}
			else
			{
				m_soundEmitters[1].PlaySound(CharacterSounds[7], stopPrevious);
			}
		}

		public void StartSecondarySound(string cueName, bool sync = false)
		{
			StartSecondarySound(MySoundPair.GetCueId(cueName), sync);
		}

		public void StartSecondarySound(MyCueId cueId, bool sync = false)
		{
			if (!cueId.IsNull)
			{
				m_soundEmitters[1].PlaySoundWithDistance(cueId);
				if (sync)
				{
					m_character.PlaySecondarySound(cueId);
				}
			}
		}

		public bool StopSecondarySound(bool forceStop = true)
		{
			m_soundEmitters[1].StopSound(forceStop);
			return true;
		}

		private MySoundPair SelectSound()
		{
			MySoundPair result = EmptySoundPair;
			MyStringHash orCompute = MyStringHash.GetOrCompute(m_character.Definition.PhysicalMaterial);
			m_isWalking = false;
			MyCharacterMovementEnum currentMovementState = m_character.GetCurrentMovementState();
			switch (currentMovementState)
			{
			case MyCharacterMovementEnum.Walking:
			case MyCharacterMovementEnum.BackWalking:
			case MyCharacterMovementEnum.WalkStrafingLeft:
			case MyCharacterMovementEnum.WalkingLeftFront:
			case MyCharacterMovementEnum.WalkingLeftBack:
			case MyCharacterMovementEnum.WalkStrafingRight:
			case MyCharacterMovementEnum.WalkingRightFront:
			case MyCharacterMovementEnum.WalkingRightBack:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				}
				result = MyMaterialPropertiesHelper.Static.GetCollisionCue(MovementSoundType.Walk, orCompute, FindSupportingMaterial());
				m_isWalking = true;
				break;
			case MyCharacterMovementEnum.Running:
			case MyCharacterMovementEnum.Backrunning:
			case MyCharacterMovementEnum.RunStrafingLeft:
			case MyCharacterMovementEnum.RunningLeftFront:
			case MyCharacterMovementEnum.RunningLeftBack:
			case MyCharacterMovementEnum.RunStrafingRight:
			case MyCharacterMovementEnum.RunningRightFront:
			case MyCharacterMovementEnum.RunningRightBack:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Heated;
				}
				result = MyMaterialPropertiesHelper.Static.GetCollisionCue(MovementSoundType.Run, orCompute, FindSupportingMaterial());
				m_isWalking = true;
				break;
			case MyCharacterMovementEnum.CrouchWalking:
			case MyCharacterMovementEnum.CrouchBackWalking:
			case MyCharacterMovementEnum.CrouchStrafingLeft:
			case MyCharacterMovementEnum.CrouchWalkingLeftFront:
			case MyCharacterMovementEnum.CrouchWalkingLeftBack:
			case MyCharacterMovementEnum.CrouchStrafingRight:
			case MyCharacterMovementEnum.CrouchWalkingRightFront:
			case MyCharacterMovementEnum.CrouchWalkingRightBack:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				}
				result = MyMaterialPropertiesHelper.Static.GetCollisionCue(MovementSoundType.CrouchWalk, orCompute, FindSupportingMaterial());
				m_isWalking = true;
				break;
			case MyCharacterMovementEnum.Standing:
			case MyCharacterMovementEnum.Crouching:
			{
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				}
				MyCharacterMovementEnum previousMovementState = m_character.GetPreviousMovementState();
				MyCharacterMovementEnum currentMovementState2 = m_character.GetCurrentMovementState();
				if (previousMovementState != currentMovementState2 && (previousMovementState == MyCharacterMovementEnum.Standing || previousMovementState == MyCharacterMovementEnum.Crouching))
				{
					result = ((currentMovementState2 == MyCharacterMovementEnum.Standing) ? CharacterSounds[4] : CharacterSounds[3]);
				}
				FindSupportingMaterial();
				break;
			}
			case MyCharacterMovementEnum.Sprinting:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.VeryHeated;
				}
				result = MyMaterialPropertiesHelper.Static.GetCollisionCue(MovementSoundType.Sprint, orCompute, FindSupportingMaterial());
				m_isWalking = true;
				break;
			case MyCharacterMovementEnum.Jump:
				if (m_jumpReady)
				{
					m_jumpReady = false;
					m_character.SetPreviousMovementState(m_character.GetCurrentMovementState());
					MyEntity3DSoundEmitter myEntity3DSoundEmitter = m_soundEmitters[5];
					if (myEntity3DSoundEmitter != null)
					{
						myEntity3DSoundEmitter.Entity = m_character;
						myEntity3DSoundEmitter.PlaySound(CharacterSounds[0], stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: true, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
					}
					if ((m_standingOnGrid != null || m_standingOnVoxel != null) && ShouldUpdateSoundEmitters)
					{
						m_needsUpdateEmitters = true;
					}
					m_standingOnGrid = null;
					m_standingOnVoxel = null;
				}
				break;
			case MyCharacterMovementEnum.Flying:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				}
				if (m_character.JetpackComp != null && m_jetpackMinIdleTime <= 0f && m_character.JetpackComp.FinalThrust.LengthSquared() >= 50000f)
				{
					result = CharacterSounds[2];
					m_jetpackSustainTimer = Math.Min(0.25f, m_jetpackSustainTimer + 0.0166666675f);
				}
				else
				{
					result = CharacterSounds[1];
					m_jetpackSustainTimer = Math.Max(0f, m_jetpackSustainTimer - 0.0166666675f);
				}
				m_jetpackMinIdleTime -= 0.0166666675f;
				if ((m_standingOnGrid != null || m_standingOnVoxel != null) && ShouldUpdateSoundEmitters)
				{
					m_needsUpdateEmitters = true;
				}
				ResetStandingSoundStates();
				break;
			case MyCharacterMovementEnum.Falling:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				}
				if ((m_standingOnGrid != null || m_standingOnVoxel != null) && ShouldUpdateSoundEmitters)
				{
					m_needsUpdateEmitters = true;
				}
				ResetStandingSoundStates();
				break;
			case MyCharacterMovementEnum.Sitting:
				if (m_character.Breath != null)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				}
				break;
			}
			if (currentMovementState != MyCharacterMovementEnum.Flying)
			{
				m_jetpackSustainTimer = 0f;
				m_jetpackMinIdleTime = 0.5f;
			}
			return result;
		}

		private void ResetStandingSoundStates()
		{
			if (MyFakes.ENABLE_REALISTIC_ON_TOUCH)
			{
				if (m_standingOnGrid != null && m_lastContactCounter < 0)
				{
					m_standingOnGrid = null;
					MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: true, updateNotPlaying: false);
				}
				else
				{
					m_lastContactCounter--;
				}
			}
			else
			{
				m_standingOnGrid = null;
				m_standingOnVoxel = null;
			}
		}

		private void UpdateOxygenSounds()
		{
			if (m_oxygenEmitter == null)
			{
				return;
			}
			MySession @static = MySession.Static;
			if (!m_character.IsDead && @static != null && @static.Settings.EnableOxygen && !@static.CreativeMode && (m_character.OxygenComponent?.HelmetEnabled ?? false))
			{
				MySoundPair mySoundPair = ((@static.CreativeMode || IsInvulnerable) ? CharacterSounds[12] : ((m_character.OxygenComponent.SuitOxygenLevel > MyCharacterOxygenComponent.LOW_OXYGEN_RATIO) ? CharacterSounds[12] : ((m_character.OxygenComponent.SuitOxygenLevel > MyCharacterOxygenComponent.LOW_OXYGEN_RATIO / 3f) ? CharacterSounds[13] : ((!(m_character.OxygenComponent.SuitOxygenLevel > 0f)) ? CharacterSounds[15] : CharacterSounds[14]))));
				if (!m_oxygenEmitter.IsPlaying || m_oxygenEmitter.SoundPair != mySoundPair)
				{
					m_oxygenEmitter.PlaySound(mySoundPair, stopPrevious: true);
				}
			}
			else if (m_oxygenEmitter.IsPlaying)
			{
				m_oxygenEmitter.StopSound(forced: true);
			}
		}

		private void UpdateBreath()
		{
			if (IsInvulnerable)
			{
				return;
			}
			MySession @static = MySession.Static;
			if (m_character.OxygenComponent == null || m_character.Breath == null)
			{
				return;
			}
			if (@static.Settings.EnableOxygen && !@static.CreativeMode)
			{
				MyCockpit myCockpit;
				if ((myCockpit = m_character.Parent as MyCockpit) != null && myCockpit.BlockDefinition.IsPressurized)
				{
					if (m_character.OxygenComponent.HelmetEnabled)
					{
						if (m_character.OxygenComponent.SuitOxygenAmount > 0f)
						{
							m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
						}
						else
						{
							m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
						}
					}
					else if (m_character.EnvironmentOxygenLevel >= MyCharacterOxygenComponent.LOW_OXYGEN_RATIO)
					{
						m_character.Breath.CurrentState = MyCharacterBreath.State.NoBreath;
					}
					else
					{
						m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
					}
				}
				else if (m_character.OxygenComponent.HelmetEnabled)
				{
					if (m_character.OxygenComponent.SuitOxygenAmount <= 0f)
					{
						m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
					}
				}
				else if (m_character.EnvironmentOxygenLevel >= MyCharacterOxygenComponent.LOW_OXYGEN_RATIO)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.NoBreath;
				}
				else if (m_character.EnvironmentOxygenLevel > 0f)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.VeryHeated;
				}
				else
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
				}
			}
			else
			{
				m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
				if (!m_character.OxygenComponent.HelmetEnabled)
				{
					m_character.Breath.CurrentState = MyCharacterBreath.State.NoBreath;
				}
			}
		}

		public void PlayFallSound()
		{
			MyStringHash myStringHash = FindSupportingMaterial();
			if (!(myStringHash != MyStringHash.NullOrEmpty) || MyMaterialPropertiesHelper.Static == null)
			{
				return;
			}
			MySoundPair collisionCue = MyMaterialPropertiesHelper.Static.GetCollisionCue(MovementSoundType.Fall, m_characterPhysicalMaterial, myStringHash);
			if (!collisionCue.SoundId.IsNull)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = m_soundEmitters[6];
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.Entity = m_character;
					myEntity3DSoundEmitter.PlaySound(collisionCue, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: true, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Updates the standing physics (can the player jump, etc...)
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyEntity UpdateStandingPhysics()
		{
			List<HkRigidBody> list = m_character.Physics?.CharacterProxy?.CharacterRigidBody.GetSupportInfo();
			MyEntity myEntity = null;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					myEntity = (MyEntity)list[i].GetSingleEntity();
					if (myEntity != null)
					{
						break;
					}
				}
			}
			MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
			MyVoxelBase myVoxelBase = myEntity as MyVoxelBase;
			bool flag = myCubeGrid != null && m_standingOnGrid != myCubeGrid;
			bool flag2 = myVoxelBase != null && m_standingOnVoxel != myVoxelBase;
			m_standingOnGrid = myCubeGrid;
			m_standingOnVoxel = myVoxelBase;
			if (ShouldUpdateSoundEmitters && (flag || flag2))
			{
				MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: true, updateNotPlaying: true);
			}
			if (myCubeGrid != null || myVoxelBase != null)
			{
				m_jumpReady = true;
			}
			return myEntity;
		}

<<<<<<< HEAD
		/// <summary>
		/// Find the material the player is currently walking on.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyStringHash FindSupportingMaterial()
		{
			MyEntity myEntity = UpdateStandingPhysics();
			MyStringHash myStringHash = default(MyStringHash);
			if (myEntity != null)
			{
				Vector3D position = m_character.PositionComp.GetPosition();
				myStringHash = myEntity.Physics.GetMaterialAt(position + m_character.PositionComp.WorldMatrixRef.Down * 0.10000000149011612);
				if (myStringHash == MyStringHash.NullOrEmpty && myEntity.Parent != null)
				{
					MyCubeGrid myCubeGrid = myEntity.Parent as MyCubeGrid;
					MyCubeBlock myCubeBlock = myEntity.Parent as MyCubeBlock;
					if (myCubeGrid != null && myCubeGrid.Physics != null)
					{
						myStringHash = myEntity.Parent.Physics.MaterialType;
					}
					else if (myCubeBlock != null)
					{
						myStringHash = myCubeBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
					}
				}
			}
			if (myStringHash == MyStringHash.NullOrEmpty)
			{
				myStringHash = MyMaterialType.ROCK;
			}
			return myStringHash;
		}

		internal void UpdateEntityEmitters(MyCubeGrid cubeGrid)
		{
			m_standingOnGrid = cubeGrid;
			m_lastContactCounter = 10;
			MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: true, updateNotPlaying: true);
		}

		public void PlayDamageSound(float oldHealth)
		{
			if (MyFakes.ENABLE_NEW_SOUNDS && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastScreamTime > 800)
			{
				bool force2D = false;
				if (MySession.Static.LocalCharacter == base.Entity)
				{
					force2D = true;
				}
				m_lastScreamTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				if (m_character.StatComp != null && m_character.StatComp.LastDamage.Type == LowPressure)
				{
					PlaySecondarySound(CharacterSoundsEnum.SUFFOCATE_SOUND, stopPrevious: false, force2D);
				}
				else
				{
					PlaySecondarySound(CharacterSoundsEnum.PAIN_SOUND, stopPrevious: false, force2D);
				}
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_character = base.Entity as MyCharacter;
			foreach (MyEntity3DSoundEmitter soundEmitter in m_soundEmitters)
			{
				soundEmitter.Entity = base.Entity as MyEntity;
			}
			if (m_windEmitter != null)
			{
				m_windEmitter.Entity = base.Entity as MyEntity;
			}
			if (m_oxygenEmitter != null)
			{
				m_oxygenEmitter.Entity = base.Entity as MyEntity;
			}
			m_lastUpdateMovementState = m_character.GetCurrentMovementState();
			m_characterPhysicalMaterial = MyStringHash.GetOrCompute(m_character.Definition.PhysicalMaterial);
			InitSounds();
			base.NeedsUpdateAfterSimulation = true;
			base.NeedsUpdateAfterSimulationParallel = true;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			StopStateSound();
			m_character = null;
			base.OnBeforeRemovedFromContainer();
		}

		internal void PlayMagneticBootsStart()
		{
			if (m_magneticBootsEmitter != null && CharacterSounds[18] != MySoundPair.Empty)
			{
				m_magneticBootsEmitter.PlaySound(CharacterSounds[18], stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
			}
		}

		internal void PlayMagneticBootsEnd()
		{
			if (m_magneticBootsEmitter != null && CharacterSounds[19] != MySoundPair.Empty)
			{
				m_magneticBootsEmitter.PlaySound(CharacterSounds[19], stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
			}
		}

		internal void PlayMagneticBootsProximity()
		{
			if (m_magneticBootsEmitter != null && CharacterSounds[20] != MySoundPair.Empty)
			{
				m_magneticBootsEmitter.PlaySound(CharacterSounds[20], stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
			}
		}
	}
}
