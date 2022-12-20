using System;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentBuilder(typeof(MyObjectBuilder_EntityReverbDetectorComponent), true)]
	public class MyEntityReverbDetectorComponent : MyEntityComponentBase
	{
		public enum ReverbDetectedType
		{
			None,
			Voxel,
			Grid
		}

		private class Sandbox_Game_EntityComponents_MyEntityReverbDetectorComponent_003C_003EActor : IActivator, IActivator<MyEntityReverbDetectorComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityReverbDetectorComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityReverbDetectorComponent CreateInstance()
			{
				return new MyEntityReverbDetectorComponent();
			}

			MyEntityReverbDetectorComponent IActivator<MyEntityReverbDetectorComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float RAYCAST_LENGTH = 25f;

		private const float INFINITY_PENALTY = 50f;

		private const float REVERB_THRESHOLD_SMALL = 3f;

		private const float REVERB_THRESHOLD_MEDIUM = 7f;

		private const float REVERB_THRESHOLD_LARGE = 12f;

		private const int REVERB_NO_OBSTACLE_LIMIT = 3;

		private static Vector3[] m_directions = new Vector3[26];

		private static bool m_systemInitialized = false;

		private static int m_currentReverbPreset = -1;

		private float[] m_detectedLengths;

		private ReverbDetectedType[] m_detectedObjects;

		private MyEntity m_entity;

		private int m_currentDirectionIndex;

		private bool m_componentInitialized;

		private bool m_sendInformationToAudio;

		private float m_averageHitLength;

		private int m_voxels;

		private int m_grids;

		public bool Initialized
		{
			get
			{
				if (m_componentInitialized)
				{
					return m_systemInitialized;
				}
				return false;
			}
		}

		public static string CurrentReverbPreset
		{
			get
			{
				if (m_currentReverbPreset == 1)
				{
					return "Cave";
				}
				if (m_currentReverbPreset == 0)
				{
					return "Ship or station";
				}
				return "None (reverb is off)";
			}
		}

		public int Voxels => m_voxels;

		public int Grids => m_grids;

		public override string ComponentTypeDebugString => "EntityReverbDetector";

		public void InitComponent(MyEntity entity, bool sendInformationToAudio)
		{
			int num = 0;
			if (!m_systemInitialized)
			{
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						for (int k = -1; k <= 1; k++)
						{
							if (i != 0 || k != 0 || j != 0)
							{
								m_directions[num] = Vector3.Normalize(new Vector3(i, k, j));
								num++;
							}
						}
					}
				}
				m_systemInitialized = true;
			}
			m_entity = entity;
			m_detectedLengths = new float[m_directions.Length];
			m_detectedObjects = new ReverbDetectedType[m_directions.Length];
			for (num = 0; num < m_directions.Length; num++)
			{
				m_detectedLengths[num] = -1f;
				m_detectedObjects[num] = ReverbDetectedType.None;
			}
			m_sendInformationToAudio = sendInformationToAudio && MyPerGameSettings.UseReverbEffect;
			m_componentInitialized = true;
		}

		public void UpdateParallel()
		{
			if (!Initialized || m_entity == null)
			{
				return;
			}
			Vector3 vector = m_entity.PositionComp.WorldAABB.Center;
			Vector3 vector2 = vector + m_directions[m_currentDirectionIndex] * 25f;
			LineD lineD = new LineD(vector, vector2);
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(lineD.From, lineD.To, 30);
			IMyEntity myEntity = null;
			Vector3D vector3D = Vector3D.Zero;
			_ = Vector3.Zero;
			if (hitInfo.HasValue)
			{
				myEntity = hitInfo.Value.HkHitInfo.GetHitEntity() as MyEntity;
				vector3D = hitInfo.Value.Position;
				_ = hitInfo.Value;
			}
			if (myEntity != null)
			{
				float num = Vector3.Distance(vector, vector3D);
				m_detectedLengths[m_currentDirectionIndex] = num;
				m_detectedObjects[m_currentDirectionIndex] = ((!(myEntity is MyCubeGrid) && !(myEntity is MyCubeBlock)) ? ReverbDetectedType.Voxel : ReverbDetectedType.Grid);
			}
			else
			{
				m_detectedLengths[m_currentDirectionIndex] = -1f;
				m_detectedObjects[m_currentDirectionIndex] = ReverbDetectedType.None;
			}
			m_currentDirectionIndex++;
			if (m_currentDirectionIndex >= m_directions.Length)
			{
				m_currentDirectionIndex = 0;
				if (m_sendInformationToAudio)
				{
					m_averageHitLength = GetDetectedAverage();
					GetDetectedNumberOfObjects(out m_grids, out m_voxels);
				}
			}
		}

		public void Update()
		{
			if (m_currentDirectionIndex == 0 && m_sendInformationToAudio)
			{
				SetReverb(m_averageHitLength, Grids, Voxels);
			}
		}

		private static void SetReverb(float distance, int grids, int voxels)
		{
			if (MyAudio.Static == null)
			{
				return;
			}
			MySession @static = MySession.Static;
			bool flag = true;
			if (@static.Settings.RealisticSound)
			{
				MyAtmosphereDetectorComponent myAtmosphereDetectorComponent = @static.LocalCharacter?.AtmosphereDetectorComp;
				flag = myAtmosphereDetectorComponent != null && (myAtmosphereDetectorComponent.InShipOrStation || myAtmosphereDetectorComponent.InAtmosphere);
			}
			int num = m_directions.Length - grids - voxels;
			int num2 = -1;
			if (flag && distance <= 12f && num <= 3)
			{
				num2 = ((voxels > grids) ? 1 : 0);
			}
			if (num2 != m_currentReverbPreset)
			{
				m_currentReverbPreset = num2;
				if (m_currentReverbPreset <= -1)
				{
					MyAudio.Static.ApplyReverb = false;
					MySessionComponentPlanetAmbientSounds.SetAmbientOn();
				}
				else if (m_currentReverbPreset == 0)
				{
					MyAudio.Static.ApplyReverb = false;
					MySessionComponentPlanetAmbientSounds.SetAmbientOff();
				}
				else
				{
					MyAudio.Static.ApplyReverb = true;
					MySessionComponentPlanetAmbientSounds.SetAmbientOff();
				}
			}
		}

		public float GetDetectedAverage(bool onlyDetected = false)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < m_detectedLengths.Length; i++)
			{
				if (m_detectedLengths[i] >= 0f)
				{
					num += m_detectedLengths[i];
					num2++;
				}
				else if (!onlyDetected)
				{
					num += 50f;
				}
			}
			if (onlyDetected)
			{
				return (num2 > 0) ? (num / (float)num2) : 50f;
			}
			return num / (float)m_detectedLengths.Length;
		}

		public int GetDetectedNumberOfObjects(ReverbDetectedType type = ReverbDetectedType.Grid)
		{
			int num = 0;
			for (int i = 0; i < m_detectedObjects.Length; i++)
			{
				if (m_detectedObjects[i] == type)
				{
					num++;
				}
			}
			return num;
		}

		public void GetDetectedNumberOfObjects(out int gridCount, out int voxelCount)
		{
			gridCount = (voxelCount = 0);
			for (int i = 0; i < m_detectedObjects.Length; i++)
			{
				switch (m_detectedObjects[i])
				{
				case ReverbDetectedType.Voxel:
					voxelCount++;
					break;
				case ReverbDetectedType.Grid:
					gridCount++;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case ReverbDetectedType.None:
					break;
				}
			}
		}
	}
}
