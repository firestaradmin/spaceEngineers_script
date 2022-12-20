using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentBuilder(typeof(MyObjectBuilder_ShipSoundComponent), true)]
	public class MyShipSoundComponent : MyEntityComponentBase
	{
		private enum ShipStateEnum
		{
			NoPower,
			Slow,
			Medium,
			Fast
		}

		private enum ShipEmitters
		{
			MainSound,
			SingleSounds,
			IonThrusters,
			HydrogenThrusters,
			AtmosphericThrusters,
			IonThrustersIdle,
			HydrogenThrustersIdle,
			AtmosphericThrustersIdle,
			WheelsMain,
			WheelsSecondary,
			ShipIdle,
			ShipEngine,
			IonThrusterSpeedUp,
			HydrogenThrusterSpeedUp
		}

		private enum ShipThrusters
		{
			Ion,
			Hydrogen,
			Atmospheric
		}

		private enum ShipTimers
		{
			SpeedUp,
			SpeedDown
		}

		private class Sandbox_Game_EntityComponents_MyShipSoundComponent_003C_003EActor : IActivator, IActivator<MyShipSoundComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipSoundComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipSoundComponent CreateInstance()
			{
				return new MyShipSoundComponent();
			}

			MyShipSoundComponent IActivator<MyShipSoundComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static Dictionary<MyDefinitionId, MyShipSoundsDefinition> m_categories = new Dictionary<MyDefinitionId, MyShipSoundsDefinition>();

		private static MyShipSoundSystemDefinition m_definition = new MyShipSoundSystemDefinition();

		private bool m_initialized;

		private bool m_shouldPlay2D;

		private bool m_shouldPlay2DChanged;

		private bool m_insideShip;

		private float m_distanceToShip = float.MaxValue;

		public bool ShipHasChanged = true;

		private MyEntity m_shipSoundSource;

		private MyCubeGrid m_shipGrid;

		private MyEntityThrustComponent m_shipThrusters;

		private MyGridWheelSystem m_shipWheels;

		private bool m_isDebris = true;

		private MyDefinitionId m_shipCategory;

		private MyShipSoundsDefinition m_groupData;

		private bool m_categoryChange;

		private bool m_forceSoundCheck;

		private float m_wheelVolumeModifierEngine;

		private float m_wheelVolumeModifierWheels;

		private HashSet<MySlimBlock> m_detectedBlocks = new HashSet<MySlimBlock>();

		private ShipStateEnum m_shipState;

		private float m_shipEngineModifier;

		private float m_singleSoundsModifier = 1f;

		private bool m_playingSpeedUpOrDown;

		private MyEntity3DSoundEmitter[] m_emitters = new MyEntity3DSoundEmitter[Enum.GetNames(typeof(ShipEmitters)).Length];

		private float[] m_thrusterVolumes;

		private float[] m_thrusterVolumeTargets;

		private bool m_singleThrusterTypeShip;

		private static MyStringHash m_thrusterIon = MyStringHash.GetOrCompute("Ion");

		private static MyStringHash m_thrusterHydrogen = MyStringHash.GetOrCompute("Hydrogen");

		private static MyStringHash m_thrusterAtmospheric = MyStringHash.GetOrCompute("Atmospheric");

		private static MyStringHash m_crossfade = MyStringHash.GetOrCompute("CrossFade");

		private static MyStringHash m_fadeOut = MyStringHash.GetOrCompute("FadeOut");

		private float[] m_timers = new float[Enum.GetNames(typeof(ShipTimers)).Length];

		private float m_lastFrameShipSpeed;

		private int m_speedChange = 15;

		private float m_shipCurrentPower;

		private float m_shipCurrentPowerTarget;

		private const float POWER_CHANGE_SPEED_UP = 0.00666666729f;

		private const float POWER_CHANGE_SPEED_DOWN = 0.0100000007f;

		private bool m_lastWheelUpdateStart;

		private bool m_lastWheelUpdateStop;

		private DateTime m_lastContactWithGround = DateTime.UtcNow;

		private bool m_shipWheelsAction;

		private bool m_scheduled;

		public override string ComponentTypeDebugString => "ShipSoundSystem";

		public static void ClearShipSounds()
		{
			m_categories.Clear();
		}

		public static void SetDefinition(MyShipSoundSystemDefinition def)
		{
			m_definition = def;
		}

		public static void AddShipSounds(MyShipSoundsDefinition shipSoundGroup)
		{
			if (m_categories.ContainsKey(shipSoundGroup.Id))
			{
				m_categories.Remove(shipSoundGroup.Id);
			}
			m_categories.Add(shipSoundGroup.Id, shipSoundGroup);
		}

		public static void ActualizeGroups()
		{
			foreach (MyShipSoundsDefinition value in m_categories.Values)
			{
				value.WheelsSpeedCompensation = m_definition.FullSpeed / value.WheelsFullSpeed;
			}
		}

		public MyShipSoundComponent()
		{
			for (int i = 0; i < m_emitters.Length; i++)
			{
				m_emitters[i] = null;
			}
			for (int j = 0; j < m_timers.Length; j++)
			{
				m_timers[j] = 0f;
			}
		}

		public bool InitComponent(MyCubeGrid shipGrid)
		{
			if (shipGrid.GridSizeEnum == MyCubeSize.Small && !MyFakes.ENABLE_NEW_SMALL_SHIP_SOUNDS)
			{
				return false;
			}
			if (shipGrid.GridSizeEnum == MyCubeSize.Large && !MyFakes.ENABLE_NEW_LARGE_SHIP_SOUNDS)
			{
				return false;
			}
			if (m_definition == null)
			{
				return false;
			}
			m_shipGrid = shipGrid;
			m_shipThrusters = m_shipGrid.Components.Get<MyEntityThrustComponent>();
			m_shipWheels = m_shipGrid.GridSystems.WheelSystem;
			m_thrusterVolumes = new float[Enum.GetNames(typeof(ShipThrusters)).Length];
			m_thrusterVolumeTargets = new float[Enum.GetNames(typeof(ShipThrusters)).Length];
			for (int i = 1; i < m_thrusterVolumes.Length; i++)
			{
				m_thrusterVolumes[i] = 0f;
				m_thrusterVolumeTargets[i] = 0f;
			}
			m_thrusterVolumes[0] = 1f;
			m_thrusterVolumeTargets[0] = 1f;
			for (int j = 0; j < m_emitters.Length; j++)
			{
				m_emitters[j] = new MyEntity3DSoundEmitter(m_shipGrid, useStaticList: true);
				m_emitters[j].Force2D = m_shouldPlay2D;
				m_emitters[j].Force3D = !m_shouldPlay2D;
			}
			m_initialized = true;
			shipGrid.OnPhysicsChanged += ShipGridOnOnPhysicsChanged;
			base.Container.ComponentAdded += ContainerOnComponentAdded;
			base.Container.ComponentRemoved += ContainerOnComponentRemoved;
			shipGrid.OnBlockRemoved += ShipGridOnOnBlockRemoved;
			shipGrid.OnBlockAdded += ShipGridOnOnBlockAdded;
			if (!m_shipGrid.IsStatic && (m_shipWheels.WheelCount > 0 || m_shipThrusters != null))
			{
				Schedule();
			}
			return true;
		}

		private void ShipGridOnOnPhysicsChanged(MyEntity obj)
		{
			if (m_shipGrid.IsStatic)
			{
				DeSchedule();
			}
			else if (m_shipWheels.WheelCount > 0 || m_shipThrusters != null)
			{
				Schedule();
			}
		}

		private void ShipGridOnOnBlockAdded(MySlimBlock block)
		{
			if (!m_shipGrid.IsStatic && m_shipWheels.WheelCount > 0)
			{
				Schedule();
			}
		}

		private void ShipGridOnOnBlockRemoved(MySlimBlock block)
		{
			MyEntityThrustComponent shipThrusters = m_shipThrusters;
			if ((shipThrusters == null || shipThrusters.ThrustCount <= 0) && m_shipWheels.WheelCount == 0)
			{
				DeSchedule();
			}
		}

		private void ContainerOnComponentRemoved(Type type, MyEntityComponentBase component)
		{
			if (m_shipWheels.WheelCount == 0 && component is MyThrusterBlockThrustComponent)
			{
				DeSchedule();
				m_shipThrusters = null;
			}
		}

		private void ContainerOnComponentAdded(Type type, MyEntityComponentBase component)
		{
			MyThrusterBlockThrustComponent shipThrusters;
			if (!m_shipGrid.IsStatic && (shipThrusters = component as MyThrusterBlockThrustComponent) != null)
			{
				Schedule();
				m_shipThrusters = shipThrusters;
			}
		}

		protected void Schedule()
		{
			if (!m_scheduled)
			{
				m_shipGrid.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, Update, 13);
				m_scheduled = true;
			}
		}

		protected void DeSchedule()
		{
			if (m_scheduled)
			{
				m_shipGrid.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, Update);
				m_scheduled = false;
			}
		}

		public void Update()
		{
			if (m_initialized && m_shipGrid.Physics != null && !m_shipGrid.IsStatic && (m_shipThrusters != null || m_shipWheels != null) && m_groupData != null)
			{
				UpdateLastGroundContact();
				float shipSpeed;
				float originalSpeed;
				ShipStateEnum lastState;
				bool flag = UpdateState(out shipSpeed, out originalSpeed, out lastState);
				UpdateSpeedBasedShipSound(flag);
				UpdateShouldPlay2D();
				CorrectThrusterVolumes();
				if (flag)
				{
					m_wheelVolumeModifierEngine = Math.Min(m_wheelVolumeModifierEngine + 0.01f, 1f);
					m_wheelVolumeModifierWheels = Math.Min(m_wheelVolumeModifierWheels + 0.03f, 1f);
				}
				else
				{
					m_wheelVolumeModifierEngine = Math.Max(m_wheelVolumeModifierEngine - 0.005f, 0f);
					m_wheelVolumeModifierWheels = Math.Max(m_wheelVolumeModifierWheels - 0.03f, 0f);
				}
				if (m_shipState != lastState || m_categoryChange || m_forceSoundCheck)
				{
					PlaySoundOnChange(lastState);
				}
				if (m_shouldPlay2DChanged)
				{
					UpdateSoundDimension();
				}
				if (m_shipState != 0)
				{
					UpdateVolumes(shipSpeed, originalSpeed);
				}
				else if (m_shipEngineModifier > 0f)
				{
					m_shipEngineModifier = Math.Max(0f, m_shipEngineModifier - 0.0166666675f / m_groupData.EngineTimeToTurnOff);
				}
				if (m_shipThrusters != null && m_shipThrusters.ThrustCount <= 0)
				{
					m_shipThrusters = null;
				}
				if (Math.Abs(shipSpeed - m_lastFrameShipSpeed) > 0.01f && shipSpeed >= 3f)
				{
					m_speedChange = (int)MyMath.Clamp(m_speedChange + ((shipSpeed > m_lastFrameShipSpeed) ? 1 : (-1)), 0f, 30f);
				}
				else if (m_speedChange != 15)
				{
					m_speedChange += ((m_speedChange <= 15) ? 1 : (-1));
				}
				if (shipSpeed >= m_lastFrameShipSpeed && m_timers[1] > 0f)
				{
					m_timers[1] -= 0.0166666675f;
				}
				if (shipSpeed <= m_lastFrameShipSpeed && m_timers[0] > 0f)
				{
					m_timers[0] -= 0.0166666675f;
				}
				m_lastFrameShipSpeed = shipSpeed;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateVolumes(float shipSpeed, float originalSpeed)
		{
			if (m_shipEngineModifier < 1f)
			{
				m_shipEngineModifier = Math.Min(1f, m_shipEngineModifier + 0.0166666675f / m_groupData.EngineTimeToTurnOn);
			}
			float num = Math.Min(shipSpeed / m_definition.FullSpeed, 1f);
			float num2 = CalculateVolumeFromSpeed(num, ref m_groupData.EngineVolumes) * m_shipEngineModifier * m_singleSoundsModifier;
			float num3 = 1f;
			if (m_emitters[0].IsPlaying)
			{
				m_emitters[0].VolumeMultiplier = num2;
				float semitones = m_groupData.EnginePitchRangeInSemitones_h + m_groupData.EnginePitchRangeInSemitones * num;
				m_emitters[0].Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(semitones);
			}
			float val = CalculateVolumeFromSpeed(num, ref m_groupData.ThrusterVolumes);
			val = Math.Max(Math.Min(val, 1f) - m_wheelVolumeModifierEngine * m_groupData.WheelsLowerThrusterVolumeBy, 0f);
			num3 = MyMath.Clamp(1.2f - val * 3f, 0f, 1f) * m_shipEngineModifier * m_singleSoundsModifier;
			val *= m_shipEngineModifier * m_singleSoundsModifier;
			m_emitters[11].VolumeMultiplier = (MySandboxGame.Config.ShipSoundsAreBasedOnSpeed ? Math.Max(0f, num2 - num3) : originalSpeed);
			m_emitters[10].VolumeMultiplier = (MySandboxGame.Config.ShipSoundsAreBasedOnSpeed ? num3 : MyMath.Clamp(1.2f - originalSpeed * 3f, 0f, 1f)) * m_shipEngineModifier * m_singleSoundsModifier;
			float frequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(m_groupData.ThrusterPitchRangeInSemitones_h + m_groupData.ThrusterPitchRangeInSemitones * val);
			if (m_emitters[2].IsPlaying)
			{
				float num4 = m_thrusterVolumes[0];
				m_emitters[2].VolumeMultiplier = val * num4;
				m_emitters[2].Sound.FrequencyRatio = frequencyRatio;
				MySoundPair shipSound = GetShipSound(ShipSystemSoundsEnum.IonThrusterPush);
				MyEntity3DSoundEmitter emiter = m_emitters[12];
				PlayThrusterPushSound(shipSpeed, num4, shipSound, emiter);
			}
			if (m_emitters[5].IsPlaying)
			{
				m_emitters[5].VolumeMultiplier = num3 * m_thrusterVolumes[0];
			}
			if (m_emitters[3].IsPlaying)
			{
				float num5 = m_thrusterVolumes[1];
				m_emitters[3].VolumeMultiplier = val * num5;
				m_emitters[3].Sound.FrequencyRatio = frequencyRatio;
				MySoundPair shipSound2 = GetShipSound(ShipSystemSoundsEnum.HydrogenThrusterPush);
				MyEntity3DSoundEmitter emiter2 = m_emitters[13];
				PlayThrusterPushSound(shipSpeed, num5, shipSound2, emiter2);
			}
			if (m_emitters[6].IsPlaying)
			{
				m_emitters[6].VolumeMultiplier = num3 * m_thrusterVolumes[1];
			}
			if (m_emitters[4].IsPlaying)
			{
				m_emitters[4].VolumeMultiplier = val * m_thrusterVolumes[2];
				m_emitters[4].Sound.FrequencyRatio = frequencyRatio;
			}
			if (m_emitters[7].IsPlaying)
			{
				m_emitters[7].VolumeMultiplier = num3 * m_thrusterVolumes[2];
			}
			if (m_emitters[8].IsPlaying)
			{
				m_emitters[0].VolumeMultiplier = Math.Max(num2 - m_wheelVolumeModifierEngine * m_groupData.WheelsLowerThrusterVolumeBy, 0f);
				m_emitters[8].VolumeMultiplier = val * m_wheelVolumeModifierEngine * m_singleSoundsModifier;
				m_emitters[8].Sound.FrequencyRatio = frequencyRatio;
				m_emitters[9].VolumeMultiplier = CalculateVolumeFromSpeed(num, ref m_groupData.WheelsVolumes) * m_shipEngineModifier * m_wheelVolumeModifierWheels * m_singleSoundsModifier;
			}
			float volumeMultiplier = 0.5f + val / 2f;
			m_playingSpeedUpOrDown = m_playingSpeedUpOrDown && m_emitters[1].IsPlaying;
			if (m_speedChange >= 20 && m_timers[0] <= 0f && m_wheelVolumeModifierEngine <= 0f)
			{
				m_timers[0] = ((m_shipGrid.GridSizeEnum == MyCubeSize.Large) ? 8f : 1f);
				if (m_emitters[1].IsPlaying && m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)))
				{
					FadeOutSound(ShipEmitters.SingleSounds, 1000);
				}
				m_emitters[1].VolumeMultiplier = volumeMultiplier;
				PlayShipSound(ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesSpeedUp, checkIfAlreadyPlaying: false, stopPrevious: false);
				m_playingSpeedUpOrDown = true;
			}
			else if (m_speedChange <= 15 && m_emitters[1].IsPlaying && m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
			{
				FadeOutSound(ShipEmitters.SingleSounds, 1000);
			}
			if (m_speedChange <= 10 && m_timers[1] <= 0f && m_wheelVolumeModifierEngine <= 0f)
			{
				m_timers[1] = ((m_shipGrid.GridSizeEnum == MyCubeSize.Large) ? 8f : 2f);
				if (m_emitters[1].IsPlaying && m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
				{
					FadeOutSound(ShipEmitters.SingleSounds, 1000);
				}
				m_emitters[1].VolumeMultiplier = volumeMultiplier;
				PlayShipSound(ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesSpeedDown, checkIfAlreadyPlaying: false, stopPrevious: false);
				m_playingSpeedUpOrDown = true;
			}
			else if (m_speedChange >= 15 && m_emitters[1].IsPlaying && m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)))
			{
				FadeOutSound(ShipEmitters.SingleSounds, 1000);
			}
			float num6 = 1f;
			if (m_playingSpeedUpOrDown && m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)))
			{
				num6 = m_groupData.SpeedDownSoundChangeVolumeTo;
			}
			if (m_playingSpeedUpOrDown && m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
			{
				num6 = m_groupData.SpeedUpSoundChangeVolumeTo;
			}
			if (m_singleSoundsModifier < num6)
			{
				m_singleSoundsModifier = Math.Min(m_singleSoundsModifier + m_groupData.SpeedUpDownChangeSpeed, num6);
			}
			else if (m_singleSoundsModifier > num6)
			{
				m_singleSoundsModifier = Math.Max(m_singleSoundsModifier - m_groupData.SpeedUpDownChangeSpeed, num6);
			}
			if (m_emitters[1].IsPlaying && (m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)) || m_emitters[1].SoundPair.Equals(GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp))))
			{
				m_emitters[1].VolumeMultiplier = volumeMultiplier;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateSoundDimension()
		{
			for (int i = 0; i < m_emitters.Length; i++)
			{
				m_emitters[i].Force2D = m_shouldPlay2D;
				m_emitters[i].Force3D = !m_shouldPlay2D;
				if (m_emitters[i].IsPlaying && m_emitters[i].Plays2D != m_shouldPlay2D && m_emitters[i].Loop)
				{
					m_emitters[i].StopSound(forced: true);
					m_emitters[i].PlaySound(m_emitters[i].SoundPair, stopPrevious: true, skipIntro: true, m_shouldPlay2D);
				}
			}
			m_shouldPlay2DChanged = false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PlaySoundOnChange(ShipStateEnum lastState)
		{
			if (m_shipState == ShipStateEnum.NoPower)
			{
				if (m_shipState != lastState)
				{
					for (int i = 0; i < m_emitters.Length; i++)
					{
						m_emitters[i].StopSound(forced: false);
					}
					m_emitters[1].VolumeMultiplier = 1f;
					PlayShipSound(ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesEnd);
				}
			}
			else
			{
				if (m_shipState == ShipStateEnum.Slow)
<<<<<<< HEAD
				{
					PlayShipSound(ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopSlow);
				}
				else if (m_shipState == ShipStateEnum.Medium)
				{
					PlayShipSound(ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopMedium);
				}
				else if (m_shipState == ShipStateEnum.Fast)
				{
					PlayShipSound(ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopFast);
				}
				PlayShipSound(ShipEmitters.ShipEngine, ShipSystemSoundsEnum.ShipEngine);
				PlayShipSound(ShipEmitters.ShipIdle, ShipSystemSoundsEnum.ShipIdle);
				if (m_thrusterVolumes[0] > 0f)
				{
					PlayShipSound(ShipEmitters.IonThrusters, ShipSystemSoundsEnum.IonThrusters);
					PlayShipSound(ShipEmitters.IonThrustersIdle, ShipSystemSoundsEnum.IonThrustersIdle);
				}
				if (m_thrusterVolumes[1] > 0f)
				{
					PlayShipSound(ShipEmitters.HydrogenThrusters, ShipSystemSoundsEnum.HydrogenThrusters);
					PlayShipSound(ShipEmitters.HydrogenThrustersIdle, ShipSystemSoundsEnum.HydrogenThrustersIdle);
				}
				if (m_thrusterVolumes[2] > 0f)
				{
					if (m_shipState == ShipStateEnum.Slow)
					{
						PlayShipSound(ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersSlow);
					}
					else if (m_shipState == ShipStateEnum.Medium)
					{
						PlayShipSound(ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersMedium);
					}
					else if (m_shipState == ShipStateEnum.Fast)
					{
						PlayShipSound(ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersFast);
					}
					PlayShipSound(ShipEmitters.AtmosphericThrustersIdle, ShipSystemSoundsEnum.AtmoThrustersIdle);
				}
				if (m_shipWheels.WheelCount > 0)
				{
					PlayShipSound(ShipEmitters.WheelsMain, ShipSystemSoundsEnum.WheelsEngineRun);
					PlayShipSound(ShipEmitters.WheelsSecondary, ShipSystemSoundsEnum.WheelsSecondary);
				}
				if (lastState == ShipStateEnum.NoPower)
				{
					m_emitters[1].VolumeMultiplier = 1f;
					PlayShipSound(ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesStart);
				}
			}
			m_categoryChange = false;
			m_forceSoundCheck = false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool UpdateState(out float shipSpeed, out float originalSpeed, out ShipStateEnum lastState)
		{
			bool flag = (DateTime.UtcNow - m_lastContactWithGround).TotalSeconds <= 0.20000000298023224;
			shipSpeed = ((!flag) ? m_shipGrid.Physics.LinearVelocity.Length() : (m_shipGrid.Physics.LinearVelocity * m_groupData.WheelsSpeedCompensation).Length());
			originalSpeed = Math.Min(shipSpeed / m_definition.FullSpeed, 1f);
			if (!MySandboxGame.Config.ShipSoundsAreBasedOnSpeed)
			{
				shipSpeed = m_shipCurrentPower * m_definition.FullSpeed;
			}
			lastState = m_shipState;
			if (m_shipGrid.GridSystems.ResourceDistributor.ResourceState == MyResourceStateEnum.NoPower || m_isDebris || ((m_shipThrusters == null || m_shipThrusters.ThrustCount <= 0) && (m_shipWheels == null || m_shipWheels.WheelCount <= 0)))
			{
				m_shipState = ShipStateEnum.NoPower;
			}
			else if (shipSpeed < m_definition.SpeedThreshold1)
			{
				m_shipState = ShipStateEnum.Slow;
			}
			else if (shipSpeed < m_definition.SpeedThreshold2)
			{
				m_shipState = ShipStateEnum.Medium;
			}
			else
			{
				m_shipState = ShipStateEnum.Fast;
			}
			return flag;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateLastGroundContact()
		{
			if (m_shipWheels == null)
			{
				return;
			}
			foreach (MyMotorSuspension wheel in m_shipWheels.Wheels)
			{
				MyWheel myWheel = wheel.TopBlock as MyWheel;
				if (myWheel != null && myWheel.LastContactTime > m_lastContactWithGround)
				{
					m_lastContactWithGround = myWheel.LastContactTime;
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateSpeedBasedShipSound(bool driving)
		{
			if (MySandboxGame.Config.ShipSoundsAreBasedOnSpeed)
			{
				return;
			}
			m_shipCurrentPowerTarget = 0f;
			if (driving)
			{
				if (m_shipWheels != null && m_shipWheels.WheelCount > 0)
				{
					if (Math.Abs(m_shipWheels.AngularVelocity.Z) >= 0.9f)
					{
						m_shipCurrentPowerTarget = 1f;
					}
					else if (m_shipGrid.Physics.LinearVelocity.LengthSquared() > 5f)
					{
=======
				{
					PlayShipSound(ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopSlow);
				}
				else if (m_shipState == ShipStateEnum.Medium)
				{
					PlayShipSound(ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopMedium);
				}
				else if (m_shipState == ShipStateEnum.Fast)
				{
					PlayShipSound(ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopFast);
				}
				PlayShipSound(ShipEmitters.ShipEngine, ShipSystemSoundsEnum.ShipEngine);
				PlayShipSound(ShipEmitters.ShipIdle, ShipSystemSoundsEnum.ShipIdle);
				if (m_thrusterVolumes[0] > 0f)
				{
					PlayShipSound(ShipEmitters.IonThrusters, ShipSystemSoundsEnum.IonThrusters);
					PlayShipSound(ShipEmitters.IonThrustersIdle, ShipSystemSoundsEnum.IonThrustersIdle);
				}
				if (m_thrusterVolumes[1] > 0f)
				{
					PlayShipSound(ShipEmitters.HydrogenThrusters, ShipSystemSoundsEnum.HydrogenThrusters);
					PlayShipSound(ShipEmitters.HydrogenThrustersIdle, ShipSystemSoundsEnum.HydrogenThrustersIdle);
				}
				if (m_thrusterVolumes[2] > 0f)
				{
					if (m_shipState == ShipStateEnum.Slow)
					{
						PlayShipSound(ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersSlow);
					}
					else if (m_shipState == ShipStateEnum.Medium)
					{
						PlayShipSound(ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersMedium);
					}
					else if (m_shipState == ShipStateEnum.Fast)
					{
						PlayShipSound(ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersFast);
					}
					PlayShipSound(ShipEmitters.AtmosphericThrustersIdle, ShipSystemSoundsEnum.AtmoThrustersIdle);
				}
				if (m_shipWheels.WheelCount > 0)
				{
					PlayShipSound(ShipEmitters.WheelsMain, ShipSystemSoundsEnum.WheelsEngineRun);
					PlayShipSound(ShipEmitters.WheelsSecondary, ShipSystemSoundsEnum.WheelsSecondary);
				}
				if (lastState == ShipStateEnum.NoPower)
				{
					m_emitters[1].VolumeMultiplier = 1f;
					PlayShipSound(ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesStart);
				}
			}
			m_categoryChange = false;
			m_forceSoundCheck = false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool UpdateState(out float shipSpeed, out float originalSpeed, out ShipStateEnum lastState)
		{
			bool flag = (DateTime.UtcNow - m_lastContactWithGround).TotalSeconds <= 0.20000000298023224;
			shipSpeed = ((!flag) ? m_shipGrid.Physics.LinearVelocity.Length() : (m_shipGrid.Physics.LinearVelocity * m_groupData.WheelsSpeedCompensation).Length());
			originalSpeed = Math.Min(shipSpeed / m_definition.FullSpeed, 1f);
			if (!MySandboxGame.Config.ShipSoundsAreBasedOnSpeed)
			{
				shipSpeed = m_shipCurrentPower * m_definition.FullSpeed;
			}
			lastState = m_shipState;
			if (m_shipGrid.GridSystems.ResourceDistributor.ResourceState == MyResourceStateEnum.NoPower || m_isDebris || ((m_shipThrusters == null || m_shipThrusters.ThrustCount <= 0) && (m_shipWheels == null || m_shipWheels.WheelCount <= 0)))
			{
				m_shipState = ShipStateEnum.NoPower;
			}
			else if (shipSpeed < m_definition.SpeedThreshold1)
			{
				m_shipState = ShipStateEnum.Slow;
			}
			else if (shipSpeed < m_definition.SpeedThreshold2)
			{
				m_shipState = ShipStateEnum.Medium;
			}
			else
			{
				m_shipState = ShipStateEnum.Fast;
			}
			return flag;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateLastGroundContact()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (m_shipWheels == null)
			{
				return;
			}
			Enumerator<MyMotorSuspension> enumerator = m_shipWheels.Wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyWheel myWheel = enumerator.get_Current().TopBlock as MyWheel;
					if (myWheel != null && myWheel.LastContactTime > m_lastContactWithGround)
					{
						m_lastContactWithGround = myWheel.LastContactTime;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateSpeedBasedShipSound(bool driving)
		{
			if (MySandboxGame.Config.ShipSoundsAreBasedOnSpeed)
			{
				return;
			}
			m_shipCurrentPowerTarget = 0f;
			if (driving)
			{
				if (m_shipWheels != null && m_shipWheels.WheelCount > 0)
				{
					if (Math.Abs(m_shipWheels.AngularVelocity.Z) >= 0.9f)
					{
						m_shipCurrentPowerTarget = 1f;
					}
					else if (m_shipGrid.Physics.LinearVelocity.LengthSquared() > 5f)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						m_shipCurrentPowerTarget = 0.33f;
					}
				}
			}
			else if (m_shipThrusters != null)
			{
				if (m_shipThrusters.FinalThrust.LengthSquared() >= 100f)
				{
					m_shipCurrentPowerTarget = 1f;
				}
				else if (m_shipGrid.Physics.Gravity != Vector3.Zero && m_shipThrusters.DampenersEnabled && m_shipGrid.Physics.LinearVelocity.LengthSquared() < 4f)
				{
					m_shipCurrentPowerTarget = 0.33f;
				}
				else
				{
					m_shipCurrentPowerTarget = 0f;
				}
			}
			if (m_shipCurrentPower < m_shipCurrentPowerTarget)
			{
				m_shipCurrentPower = Math.Min(m_shipCurrentPower + 0.00666666729f, m_shipCurrentPowerTarget);
			}
			else if (m_shipCurrentPower > m_shipCurrentPowerTarget)
			{
				m_shipCurrentPower = Math.Max(m_shipCurrentPower - 0.0100000007f, m_shipCurrentPowerTarget);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void CorrectThrusterVolumes()
		{
			for (int i = 0; i < m_thrusterVolumes.Length; i++)
			{
				if (m_thrusterVolumes[i] < m_thrusterVolumeTargets[i])
				{
					m_thrusterVolumes[i] = Math.Min(m_thrusterVolumes[i] + m_groupData.ThrusterCompositionChangeSpeed, m_thrusterVolumeTargets[i]);
				}
				else if (m_thrusterVolumes[i] > m_thrusterVolumeTargets[i])
				{
					m_thrusterVolumes[i] = Math.Max(m_thrusterVolumes[i] - m_groupData.ThrusterCompositionChangeSpeed, m_thrusterVolumeTargets[i]);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateShouldPlay2D()
		{
			bool shouldPlay2D = m_shouldPlay2D;
			if (m_shipGrid.GridSizeEnum == MyCubeSize.Large)
			{
				m_shouldPlay2D = m_insideShip && MySession.Static.ControlledEntity != null && (MySession.Static.ControlledEntity.Entity is MyCockpit || MySession.Static.ControlledEntity.Entity is MyRemoteControl || MySession.Static.ControlledEntity.Entity is MyShipController);
				if (m_shouldPlay2D)
				{
					MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity.Entity as MyCubeBlock;
					m_shouldPlay2D &= myCubeBlock != null && myCubeBlock.CubeGrid != null && myCubeBlock.CubeGrid.GridSizeEnum == MyCubeSize.Large;
				}
			}
			else if (MySession.Static.ControlledEntity != null && !MySession.Static.IsCameraUserControlledSpectator() && MySession.Static.ControlledEntity.Entity != null && MySession.Static.ControlledEntity.Entity.Parent == m_shipGrid)
			{
				m_shouldPlay2D = (MySession.Static.ControlledEntity.Entity is MyCockpit && (MySession.Static.ControlledEntity.Entity as MyCockpit).IsInFirstPersonView) || (MySession.Static.ControlledEntity.Entity is MyRemoteControl && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.IsUsing is MyCockpit && (MySession.Static.LocalCharacter.IsUsing as MyCockpit).Parent == m_shipGrid) || (MySession.Static.CameraController is MyCameraBlock && (MySession.Static.CameraController as MyCameraBlock).Parent == m_shipGrid);
			}
			else
			{
				m_shouldPlay2D = false;
			}
			m_shouldPlay2DChanged = shouldPlay2D != m_shouldPlay2D;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PlayThrusterPushSound(float shipSpeed, float volume, MySoundPair soundPair, MyEntity3DSoundEmitter emiter)
		{
			if (m_shipThrusters != null && m_shipThrusters.ControlThrust.LengthSquared() >= 1f)
			{
				if (!emiter.IsPlaying)
				{
					emiter.VolumeMultiplier = volume;
					emiter.PlaySound(soundPair, stopPrevious: true, skipIntro: true, m_shouldPlay2D);
				}
				else
				{
					emiter.VolumeMultiplier *= 0.995f;
				}
			}
			else if (emiter.IsPlaying)
			{
				emiter.VolumeMultiplier *= 0.5f;
				if (emiter.VolumeMultiplier < 0.1f)
				{
					emiter.StopSound(forced: true);
				}
			}
		}

		public void Update100()
		{
			if (m_shipGrid.IsStatic || (m_shipThrusters == null && m_shipWheels.WheelCount == 0))
			{
				DeSchedule();
				return;
			}
			m_distanceToShip = ((!m_initialized || m_shipGrid.Physics == null) ? float.MaxValue : (m_shouldPlay2D ? 0f : ((float)m_shipGrid.PositionComp.WorldAABB.DistanceSquared(MySector.MainCamera.Position))));
			if (m_distanceToShip < m_definition.MaxUpdateRange_sq)
			{
				Schedule();
				UpdateCategory();
				UpdateSounds();
				UpdateWheels();
			}
			else
			{
				DeSchedule();
			}
		}

		private void UpdateCategory()
		{
			if (!m_initialized || m_shipGrid == null || m_shipGrid.Physics == null || m_shipGrid.IsStatic || m_definition == null || !(m_distanceToShip < m_definition.MaxUpdateRange_sq))
			{
				return;
			}
			if (m_shipWheels == null)
			{
				m_shipWheels = m_shipGrid.GridSystems.WheelSystem;
			}
			CalculateShipCategory();
			if (!m_isDebris && m_shipState != 0 && (!m_singleThrusterTypeShip || ShipHasChanged || m_shipThrusters == null || m_shipThrusters.FinalThrust == Vector3.Zero || (m_shipWheels != null && m_shipWheels.HasWorkingWheels(propulsion: false))))
			{
				CalculateThrusterComposition();
			}
			if (m_shipSoundSource == null)
			{
				m_shipSoundSource = m_shipGrid;
			}
			if (m_shipGrid.MainCockpit != null && m_shipGrid.GridSizeEnum == MyCubeSize.Small)
			{
				m_shipSoundSource = m_shipGrid.MainCockpit;
			}
			if (m_shipGrid.GridSizeEnum == MyCubeSize.Large && MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				if (MySession.Static.LocalCharacter.ReverbDetectorComp != null && (!MySession.Static.Settings.RealisticSound || (MySession.Static.LocalCharacter.AtmosphereDetectorComp != null && (MySession.Static.LocalCharacter.AtmosphereDetectorComp.InAtmosphere || MySession.Static.LocalCharacter.AtmosphereDetectorComp.InShipOrStation))))
				{
					m_insideShip = MySession.Static.LocalCharacter.ReverbDetectorComp.Grids > 0;
				}
				else
				{
					m_insideShip = false;
				}
			}
			_ = m_groupData;
		}

		private void UpdateSounds()
		{
			for (int i = 0; i < m_emitters.Length; i++)
			{
				if (m_emitters[i] != null)
				{
					m_emitters[i].Entity = m_shipSoundSource;
					m_emitters[i].Update();
				}
			}
		}

		private void UpdateWheels()
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			if (m_shipGrid == null || m_shipGrid.Physics == null || m_shipWheels == null || m_shipWheels.WheelCount <= 0)
			{
				return;
			}
			bool flag = m_distanceToShip < m_definition.WheelsCallbackRangeCreate_sq && !m_isDebris;
			bool flag2 = m_distanceToShip > m_definition.WheelsCallbackRangeRemove_sq || m_isDebris;
			if (!(flag || flag2) || (m_lastWheelUpdateStart == flag && m_lastWheelUpdateStop == flag2))
<<<<<<< HEAD
			{
				return;
			}
			foreach (MyMotorSuspension wheel in m_shipWheels.Wheels)
			{
				if (wheel != null && wheel.RotorGrid != null && wheel.RotorGrid.Physics != null && !(wheel.RotorGrid.Physics.RigidBody == null))
				{
					if (!wheel.RotorGrid.HasShipSoundEvents && flag)
					{
						wheel.RotorGrid.Physics.RigidBody.ContactPointCallback += RigidBody_ContactPointCallback;
						wheel.RotorGrid.Physics.RigidBody.CallbackLimit = 1;
						wheel.RotorGrid.OnClosing += RotorGrid_OnClosing;
						wheel.RotorGrid.HasShipSoundEvents = true;
					}
					else if (wheel.RotorGrid.HasShipSoundEvents && flag2)
					{
						wheel.RotorGrid.HasShipSoundEvents = false;
						wheel.RotorGrid.Physics.RigidBody.ContactPointCallback -= RigidBody_ContactPointCallback;
						wheel.RotorGrid.OnClosing -= RotorGrid_OnClosing;
					}
				}
=======
			{
				return;
			}
			Enumerator<MyMotorSuspension> enumerator = m_shipWheels.Wheels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyMotorSuspension current = enumerator.get_Current();
					if (current != null && current.RotorGrid != null && current.RotorGrid.Physics != null && !(current.RotorGrid.Physics.RigidBody == null))
					{
						if (!current.RotorGrid.HasShipSoundEvents && flag)
						{
							current.RotorGrid.Physics.RigidBody.ContactPointCallback += RigidBody_ContactPointCallback;
							current.RotorGrid.Physics.RigidBody.CallbackLimit = 1;
							current.RotorGrid.OnClosing += RotorGrid_OnClosing;
							current.RotorGrid.HasShipSoundEvents = true;
						}
						else if (current.RotorGrid.HasShipSoundEvents && flag2)
						{
							current.RotorGrid.HasShipSoundEvents = false;
							current.RotorGrid.Physics.RigidBody.ContactPointCallback -= RigidBody_ContactPointCallback;
							current.RotorGrid.OnClosing -= RotorGrid_OnClosing;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_lastWheelUpdateStart = flag;
			m_lastWheelUpdateStop = flag2;
			if (flag && !m_shipWheelsAction)
			{
				m_shipWheels.OnMotorUnregister += m_shipWheels_OnMotorUnregister;
				m_shipWheelsAction = true;
			}
			else if (flag2 && m_shipWheelsAction)
			{
				m_shipWheels.OnMotorUnregister -= m_shipWheels_OnMotorUnregister;
				m_shipWheelsAction = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_lastWheelUpdateStart = flag;
			m_lastWheelUpdateStop = flag2;
			if (flag && !m_shipWheelsAction)
			{
				m_shipWheels.OnMotorUnregister += m_shipWheels_OnMotorUnregister;
				m_shipWheelsAction = true;
			}
			else if (flag2 && m_shipWheelsAction)
			{
				m_shipWheels.OnMotorUnregister -= m_shipWheels_OnMotorUnregister;
				m_shipWheelsAction = false;
			}
		}

		private void m_shipWheels_OnMotorUnregister(MyCubeGrid obj)
		{
			if (obj.HasShipSoundEvents)
			{
				obj.HasShipSoundEvents = false;
				RotorGrid_OnClosing(obj);
			}
		}

		private void RotorGrid_OnClosing(MyEntity obj)
		{
			if (obj.Physics != null)
			{
				obj.Physics.RigidBody.ContactPointCallback -= RigidBody_ContactPointCallback;
				obj.OnClose -= RotorGrid_OnClosing;
			}
		}

		private void RigidBody_ContactPointCallback(ref HkContactPointEvent A_0)
		{
			m_lastContactWithGround = DateTime.UtcNow;
		}

		private void CalculateThrusterComposition()
		{
			if (m_shipThrusters == null)
			{
				m_thrusterVolumeTargets[0] = 0f;
				m_thrusterVolumeTargets[1] = 0f;
				m_thrusterVolumeTargets[2] = 0f;
				return;
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (MyThrust fatBlock in m_shipGrid.GetFatBlocks<MyThrust>())
			{
				if (fatBlock != null)
				{
					if (fatBlock.BlockDefinition.ThrusterType == m_thrusterHydrogen)
					{
						num2 += fatBlock.CurrentStrength * (Math.Abs(fatBlock.ThrustForce.X) + Math.Abs(fatBlock.ThrustForce.Y) + Math.Abs(fatBlock.ThrustForce.Z));
						flag3 = flag3 || (fatBlock.IsFunctional && fatBlock.Enabled && fatBlock.IsPowered);
					}
					else if (fatBlock.BlockDefinition.ThrusterType == m_thrusterAtmospheric)
					{
						num3 += fatBlock.CurrentStrength * (Math.Abs(fatBlock.ThrustForce.X) + Math.Abs(fatBlock.ThrustForce.Y) + Math.Abs(fatBlock.ThrustForce.Z));
						flag2 = flag2 || (fatBlock.IsFunctional && fatBlock.Enabled);
					}
					else
					{
						num += fatBlock.CurrentStrength * (Math.Abs(fatBlock.ThrustForce.X) + Math.Abs(fatBlock.ThrustForce.Y) + Math.Abs(fatBlock.ThrustForce.Z));
						flag = flag || (fatBlock.IsFunctional && fatBlock.Enabled);
					}
				}
			}
			ShipHasChanged = false;
			m_singleThrusterTypeShip = !(flag && flag2) && !(flag && flag3) && !(flag3 && flag2);
			if (m_singleThrusterTypeShip)
			{
				m_thrusterVolumeTargets[0] = (flag ? 1f : 0f);
				m_thrusterVolumeTargets[1] = (flag3 ? 1f : 0f);
				m_thrusterVolumeTargets[2] = (flag2 ? 1f : 0f);
				if (!flag && !flag3 && !flag2)
				{
					ShipHasChanged = true;
				}
			}
			else if (num + num2 + num3 > 0f)
			{
				float num4 = num2 + num + num3;
				num = ((num > 0f) ? ((m_groupData.ThrusterCompositionMinVolume_c + num / num4) / (1f + m_groupData.ThrusterCompositionMinVolume_c)) : 0f);
				num2 = ((num2 > 0f) ? ((m_groupData.ThrusterCompositionMinVolume_c + num2 / num4) / (1f + m_groupData.ThrusterCompositionMinVolume_c)) : 0f);
				num3 = ((num3 > 0f) ? ((m_groupData.ThrusterCompositionMinVolume_c + num3 / num4) / (1f + m_groupData.ThrusterCompositionMinVolume_c)) : 0f);
				m_thrusterVolumeTargets[0] = num;
				m_thrusterVolumeTargets[1] = num2;
				m_thrusterVolumeTargets[2] = num3;
			}
			if (m_thrusterVolumes[0] <= 0f && m_emitters[2].IsPlaying)
			{
				m_emitters[2].StopSound(forced: false);
				m_emitters[5].StopSound(forced: false);
			}
			if (m_thrusterVolumes[1] <= 0f && m_emitters[3].IsPlaying)
			{
				m_emitters[3].StopSound(forced: false);
				m_emitters[6].StopSound(forced: false);
			}
			if (m_thrusterVolumes[2] <= 0f && m_emitters[4].IsPlaying)
			{
				m_emitters[4].StopSound(forced: false);
				m_emitters[7].StopSound(forced: false);
			}
			if ((m_thrusterVolumeTargets[0] > 0f && !m_emitters[2].IsPlaying) || (m_thrusterVolumeTargets[1] > 0f && !m_emitters[3].IsPlaying) || (m_thrusterVolumeTargets[2] > 0f && !m_emitters[4].IsPlaying))
			{
				m_forceSoundCheck = true;
			}
		}

		private void CalculateShipCategory()
		{
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			bool isDebris = m_isDebris;
			MyDefinitionId shipCategory = m_shipCategory;
			if (m_shipThrusters == null && (m_shipWheels == null || m_shipWheels.WheelCount <= 0))
			{
				m_isDebris = true;
			}
			else
			{
				bool flag = false;
				foreach (MyCubeBlock fatBlock in m_shipGrid.GetFatBlocks())
				{
					if (fatBlock is MyShipController)
					{
						if (m_shipGrid.MainCockpit == null && m_shipGrid.GridSizeEnum == MyCubeSize.Small)
						{
							m_shipSoundSource = fatBlock;
						}
						flag = true;
						break;
					}
				}
				if (flag)
				{
					float currentMass = m_shipGrid.GetCurrentMass(GridLinkTypeEnum.Mechanical);
					float num = float.MinValue;
					MyDefinitionId? myDefinitionId = null;
					Enumerator<MyMotorSuspension> enumerator2 = m_shipWheels.Wheels.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyMotorSuspension current2 = enumerator2.get_Current();
							if (current2.BlockDefinition.SoundDefinitionId.HasValue)
							{
								myDefinitionId = current2.BlockDefinition.SoundDefinitionId.Value;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					foreach (MyShipSoundsDefinition value in m_categories.Values)
					{
						if (value.MinWeight < currentMass && ((value.AllowSmallGrid && m_shipGrid.GridSizeEnum == MyCubeSize.Small) || (value.AllowLargeGrid && m_shipGrid.GridSizeEnum == MyCubeSize.Large)) && (num == float.MinValue || value.MinWeight > num))
						{
							num = value.MinWeight;
							m_shipCategory = value.Id;
							m_groupData = value;
						}
						if (myDefinitionId.HasValue && value.Id.Equals(myDefinitionId.Value))
						{
							num = value.MinWeight;
							m_shipCategory = value.Id;
							m_groupData = value;
							break;
						}
					}
					if (num == float.MinValue)
					{
						m_isDebris = true;
					}
					else
					{
						m_isDebris = false;
					}
				}
				else
				{
					m_isDebris = true;
				}
			}
			if (m_groupData == null)
			{
				m_isDebris = true;
			}
			if (shipCategory != m_shipCategory || m_isDebris != isDebris)
			{
				m_categoryChange = true;
				if (m_isDebris)
				{
					for (int i = 0; i < m_emitters.Length; i++)
					{
						if (m_emitters[i].IsPlaying && m_emitters[i].Loop)
						{
							if (i == 8 || i == 9)
							{
								m_emitters[i].StopSound(m_shipWheels == null);
							}
							else
							{
								m_emitters[i].StopSound(m_shipThrusters == null);
							}
						}
					}
				}
				else
				{
					for (int j = 0; j < m_emitters.Length; j++)
					{
						if (m_emitters[j].IsPlaying && m_emitters[j].Loop)
						{
							m_emitters[j].StopSound(forced: true);
						}
					}
				}
			}
			if (m_isDebris)
			{
				SetGridSounds(silent: false);
			}
			else
			{
				SetGridSounds(silent: true);
			}
		}

		private void SetGridSounds(bool silent)
		{
			foreach (MyCubeBlock fatBlock in m_shipGrid.GetFatBlocks())
			{
				if (fatBlock.BlockDefinition.SilenceableByShipSoundSystem && fatBlock.IsSilenced != silent)
				{
					bool silenceInChange = fatBlock.SilenceInChange;
					fatBlock.SilenceInChange = true;
					fatBlock.IsSilenced = silent;
					if (!silenceInChange)
					{
						fatBlock.UsedUpdateEveryFrame = (fatBlock.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0;
						fatBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					}
				}
			}
		}

		private float CalculateVolumeFromSpeed(float speedRatio, ref List<MyTuple<float, float>> pairs)
		{
			float result = 1f;
			if (pairs.Count > 0)
			{
				result = pairs[pairs.Count - 1].Item2;
			}
			for (int i = 1; i < pairs.Count; i++)
			{
				if (speedRatio < pairs[i].Item1)
				{
					result = pairs[i - 1].Item2 + (pairs[i].Item2 - pairs[i - 1].Item2) * ((speedRatio - pairs[i - 1].Item1) / (pairs[i].Item1 - pairs[i - 1].Item1));
					break;
				}
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void FadeOutSound(ShipEmitters emitter = ShipEmitters.SingleSounds, int duration = 2000)
		{
			if (m_emitters[(int)emitter].IsPlaying)
			{
				IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(m_emitters[(int)emitter].Sound, m_fadeOut, new MyCueId[0], duration);
				m_emitters[(int)emitter].SetSound(myAudioEffect.OutputSound, "FadeOutSound");
			}
			if (emitter == ShipEmitters.SingleSounds)
			{
				m_playingSpeedUpOrDown = false;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PlayShipSound(ShipEmitters emitter, ShipSystemSoundsEnum sound, bool checkIfAlreadyPlaying = true, bool stopPrevious = true, bool useForce2D = true, bool useFadeOut = false)
		{
			MySoundPair shipSound = GetShipSound(sound);
			if (shipSound != MySoundPair.Empty && m_emitters[(int)emitter] != null && (!checkIfAlreadyPlaying || !m_emitters[(int)emitter].IsPlaying || m_emitters[(int)emitter].SoundPair != shipSound))
			{
				if (m_emitters[(int)emitter].IsPlaying && useFadeOut)
				{
					IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(m_emitters[(int)emitter].Sound, MyStringHash.GetOrCompute("CrossFade"), new MyCueId[1] { shipSound.SoundId }, 1500f);
					m_emitters[(int)emitter].SetSound(myAudioEffect.OutputSound, "PlayShipSound");
				}
				else
				{
					m_emitters[(int)emitter].PlaySound(shipSound, stopPrevious, skipIntro: false, useForce2D && m_shouldPlay2D);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private MySoundPair GetShipSound(ShipSystemSoundsEnum sound)
		{
			if (m_isDebris)
			{
				return MySoundPair.Empty;
			}
			if (m_categories.TryGetValue(m_shipCategory, out var value))
			{
				if (value.Sounds.TryGetValue(sound, out var value2))
				{
					return value2;
				}
				return MySoundPair.Empty;
			}
			return MySoundPair.Empty;
		}

		public void DestroyComponent()
		{
			base.Container.ComponentAdded -= ContainerOnComponentAdded;
			base.Container.ComponentRemoved -= ContainerOnComponentRemoved;
			if (m_shipGrid != null)
			{
				m_shipGrid.OnPhysicsChanged -= ShipGridOnOnPhysicsChanged;
				m_shipGrid.OnBlockRemoved -= ShipGridOnOnBlockRemoved;
				m_shipGrid.OnBlockAdded -= ShipGridOnOnBlockAdded;
			}
			for (int i = 0; i < m_emitters.Length; i++)
			{
				if (m_emitters[i] != null)
				{
					m_emitters[i].StopSound(forced: true);
					m_emitters[i].CleanEmitterMethods();
					m_emitters[i] = null;
				}
			}
			m_shipGrid = null;
			m_shipThrusters = null;
			m_shipWheels = null;
			base.Container.Remove(GetType());
		}
	}
}
