using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
<<<<<<< HEAD
=======
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_AdvancedDoor))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyAdvancedDoor),
		typeof(Sandbox.ModAPI.Ingame.IMyAdvancedDoor)
	})]
	public class MyAdvancedDoor : MyDoorBase, Sandbox.ModAPI.IMyAdvancedDoor, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAdvancedDoor
	{
		private class Sandbox_Game_Entities_MyAdvancedDoor_003C_003EActor : IActivator, IActivator<MyAdvancedDoor>
		{
			private sealed override object CreateInstance()
			{
				return new MyAdvancedDoor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAdvancedDoor CreateInstance()
			{
				return new MyAdvancedDoor();
			}

			MyAdvancedDoor IActivator<MyAdvancedDoor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float CLOSED_DISSASEMBLE_RATIO = 3.3f;

		private static readonly float EPSILON = 1E-09f;

		private int m_lastUpdateTime;

		private float m_time;

		private float m_totalTime = 99999f;

		private bool m_stateChange;

		private readonly List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();

		private readonly List<int> m_subpartIDs = new List<int>();

		private readonly List<float> m_currentOpening = new List<float>();

		private readonly List<bool> m_currentlyAtLimit = new List<bool>();

		private readonly List<float> m_currentSpeed = new List<float>();

		private readonly List<MyEntity3DSoundEmitter> m_emitter = new List<MyEntity3DSoundEmitter>();

		private readonly List<Vector3> m_hingePosition = new List<Vector3>();

		private readonly List<MyObjectBuilder_AdvancedDoorDefinition.Opening> m_openingSequence = new List<MyObjectBuilder_AdvancedDoorDefinition.Opening>();

		private Matrix[] transMat = new Matrix[1];

		private Matrix[] rotMat = new Matrix[1];

		private int m_sequenceCount;

		private int m_subpartCount;

		private bool[] m_isSubpartAtLimits;

		public override float DisassembleRatio => base.DisassembleRatio * (base.Open ? 1f : 3.3f);

		DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status
		{
			get
			{
				float openRatio = OpenRatio;
				if ((bool)m_open)
				{
					if (!(1f - openRatio < EPSILON))
					{
						return DoorStatus.Opening;
					}
					return DoorStatus.Open;
				}
				if (!(openRatio < EPSILON))
				{
					return DoorStatus.Closing;
				}
				return DoorStatus.Closed;
			}
		}

		bool Sandbox.ModAPI.IMyDoor.IsFullyClosed
		{
			get
			{
				for (int i = 0; i < m_currentOpening.Count; i++)
				{
					if (m_currentOpening[i] != 0f)
					{
						return false;
					}
				}
				return true;
			}
		}

		[Obsolete("Use Sandbox.ModAPI.IMyDoor.IsFullyClosed")]
		public bool FullyClosed
		{
			get
			{
				for (int i = 0; i < m_currentOpening.Count; i++)
				{
					if (m_currentOpening[i] != 0f)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool FullyOpen
		{
			get
			{
				for (int i = 0; i < m_currentOpening.Count; i++)
				{
					if (m_openingSequence[i].MaxOpen != m_currentOpening[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		public float OpenRatio
		{
			get
			{
				for (int i = 0; i < m_currentOpening.Count; i++)
				{
					if (m_currentOpening[i] > 0f)
					{
						return m_currentOpening[i];
					}
				}
				return 0f;
			}
		}

		public float OpeningSpeed
		{
			get
			{
				for (int i = 0; i < m_currentSpeed.Count; i++)
				{
					if (m_currentSpeed[i] > 0f)
					{
						return m_currentSpeed[i];
					}
				}
				return 0f;
			}
		}

		private new MyAdvancedDoorDefinition BlockDefinition => (MyAdvancedDoorDefinition)base.BlockDefinition;

		public event Action<bool> DoorStateChanged;

		public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public MyAdvancedDoor()
		{
			m_subparts.Clear();
			m_subpartIDs.Clear();
			m_currentOpening.Clear();
			m_currentlyAtLimit.Clear();
			m_currentSpeed.Clear();
			m_emitter.Clear();
			m_hingePosition.Clear();
			m_openingSequence.Clear();
			m_open.ValueChanged += delegate
			{
				OnStateChange();
			};
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		private void UpdateEmissivity()
		{
			if (Enabled && base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 1f, Color.Green, Color.White);
				OnStateChange();
			}
			else
			{
				MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 0f, Color.Red, Color.White);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.OpenDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((Sandbox.ModAPI.Ingame.IMyDoor)this).Status;
				if ((uint)status > 1u)
				{
					((Sandbox.ModAPI.Ingame.IMyDoor)this).ToggleDoor();
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.CloseDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((Sandbox.ModAPI.Ingame.IMyDoor)this).Status;
				if ((uint)(status - 2) > 1u)
				{
					((Sandbox.ModAPI.Ingame.IMyDoor)this).ToggleDoor();
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.ToggleDoor()
		{
			if (base.IsWorking)
			{
				SetOpenRequest(!base.Open, base.OwnerId);
			}
		}

		private void GetOpenCloseStatus(out bool fullyOpen, out bool fullyClosed)
		{
			fullyOpen = true;
			fullyClosed = true;
			for (int i = 0; i < m_currentOpening.Count; i++)
			{
				if (m_currentOpening[i] > 0f)
				{
<<<<<<< HEAD
					fullyClosed = false;
				}
				if (m_currentOpening[i] < m_openingSequence[i].MaxOpen)
				{
					fullyOpen = false;
				}
=======
					Getter = (MyAdvancedDoor x) => x.Open,
					Setter = delegate(MyAdvancedDoor x, bool v)
					{
						x.SetOpenRequest(v, x.OwnerId);
					}
				};
				obj.EnableToggleAction();
				obj.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void GetOpenCloseStatus(out bool fullyOpen, out bool fullyClosed)
		{
			fullyOpen = true;
			fullyClosed = true;
			for (int i = 0; i < m_currentOpening.Count; i++)
			{
				if (m_currentOpening[i] > 0f)
				{
					fullyClosed = false;
				}
				if (m_currentOpening[i] < m_openingSequence[i].MaxOpen)
				{
					fullyOpen = false;
				}
			}
		}

		private void OnStateChange()
		{
			for (int i = 0; i < m_openingSequence.Count; i++)
			{
				float speed = m_openingSequence[i].Speed;
				m_currentSpeed[i] = (m_open ? speed : (0f - speed));
			}
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds - 1;
			bool isPowered = base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			UpdateCurrentOpening(isPowered);
			UpdateDoorPosition(isPowered, simplicicationOverride: true);
			if ((bool)m_open)
			{
				this.DoorStateChanged.InvokeIfNotNull(m_open);
				this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
			}
			m_stateChange = true;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.PowerConsumptionMoving, UpdatePowerInput, this);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(builder, cubeGrid);
			MyObjectBuilder_AdvancedDoor myObjectBuilder_AdvancedDoor = (MyObjectBuilder_AdvancedDoor)builder;
			m_open.SetLocalValue(myObjectBuilder_AdvancedDoor.Open);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.Update();
			bool flag = base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
<<<<<<< HEAD
			if (!Enabled || !flag)
=======
			if (!base.Enabled || !flag)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				UpdateDoorPosition(flag, simplicicationOverride: true);
			}
			OnStateChange();
			if ((bool)m_open)
			{
				UpdateDoorPosition(flag, simplicicationOverride: true);
			}
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
		}

		private MyEntitySubpart LoadSubpartFromName(string name)
		{
			base.Subparts.TryGetValue(name, out var value);
			if (value != null)
			{
				return value;
			}
			value = new MyEntitySubpart();
			string model = Path.Combine(Path.GetDirectoryName(base.Model.AssetName), name) + ".mwm";
			value.Render.EnableColorMaskHsv = base.Render.EnableColorMaskHsv;
			value.Render.ColorMaskHsv = base.Render.ColorMaskHsv;
			value.Render.TextureChanges = base.Render.TextureChanges;
			value.Render.MetalnessColorable = base.Render.MetalnessColorable;
			value.Init(null, model, this, null);
			base.Subparts[name] = value;
			if (base.InScene)
			{
				value.OnAddedToScene(this);
			}
			return value;
		}

		private void InitSubparts()
		{
			if (!base.CubeGrid.CreatePhysics)
			{
				return;
			}
			m_subparts.Clear();
			m_subpartIDs.Clear();
			m_currentOpening.Clear();
			m_currentlyAtLimit.Clear();
			m_currentSpeed.Clear();
			m_emitter.Clear();
			m_hingePosition.Clear();
			m_openingSequence.Clear();
			for (int i = 0; i < BlockDefinition.Subparts.Length; i++)
			{
				MyEntitySubpart myEntitySubpart = LoadSubpartFromName(BlockDefinition.Subparts[i].Name);
				if (myEntitySubpart == null)
				{
					continue;
				}
				m_subparts.Add(myEntitySubpart);
				if (!BlockDefinition.Subparts[i].PivotPosition.HasValue)
				{
					MyModelBone myModelBone = Enumerable.First<MyModelBone>((IEnumerable<MyModelBone>)myEntitySubpart.Model.Bones, (Func<MyModelBone, bool>)((MyModelBone b) => !b.Name.Contains("Root")));
					if (myModelBone != null)
					{
						m_hingePosition.Add(myModelBone.Transform.Translation);
					}
				}
				else
				{
					m_hingePosition.Add(BlockDefinition.Subparts[i].PivotPosition.Value);
				}
			}
			int num = BlockDefinition.OpeningSequence.Length;
			for (int j = 0; j < num; j++)
			{
				if (!string.IsNullOrEmpty(BlockDefinition.OpeningSequence[j].IDs))
				{
					string[] array = BlockDefinition.OpeningSequence[j].IDs.Split(new char[1] { ',' });
					for (int k = 0; k < array.Length; k++)
					{
						string[] array2 = array[k].Split(new char[1] { '-' });
						if (array2.Length == 2)
						{
							for (int l = Convert.ToInt32(array2[0]); l <= Convert.ToInt32(array2[1]); l++)
							{
								m_openingSequence.Add(BlockDefinition.OpeningSequence[j]);
								m_subpartIDs.Add(l);
							}
						}
						else
						{
							m_openingSequence.Add(BlockDefinition.OpeningSequence[j]);
							m_subpartIDs.Add(Convert.ToInt32(array[k]));
						}
					}
				}
				else
				{
					m_openingSequence.Add(BlockDefinition.OpeningSequence[j]);
					m_subpartIDs.Add(BlockDefinition.OpeningSequence[j].ID);
				}
			}
			for (int m = 0; m < m_openingSequence.Count; m++)
			{
				m_currentOpening.Add(0f);
				m_currentlyAtLimit.Add(item: false);
				m_currentSpeed.Add(0f);
				m_emitter.Add(new MyEntity3DSoundEmitter(this, useStaticList: true));
				if (m_openingSequence[m].MaxOpen < 0f)
				{
					m_openingSequence[m].MaxOpen *= -1f;
					m_openingSequence[m].InvertRotation = !m_openingSequence[m].InvertRotation;
				}
			}
			m_sequenceCount = m_openingSequence.Count;
			m_subpartCount = m_subparts.Count;
			m_isSubpartAtLimits = new bool[m_subpartCount];
<<<<<<< HEAD
			for (int n = 0; n < m_isSubpartAtLimits.Length; n++)
=======
			for (int n = 0; n > m_isSubpartAtLimits.Length; n++)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_isSubpartAtLimits[n] = false;
			}
			Array.Resize(ref transMat, m_subpartCount);
			Array.Resize(ref rotMat, m_subpartCount);
			UpdateDoorPosition(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId), simplicicationOverride: true);
			if (base.CubeGrid.Projector != null)
			{
				return;
			}
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				if (subpart.Physics != null)
				{
					subpart.Physics.Close();
					subpart.Physics = null;
				}
				if (subpart != null && subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
				{
					HkShape[] havokCollisionShapes = subpart.ModelCollision.HavokCollisionShapes;
					HkListShape hkListShape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
					subpart.Physics = new MyPhysicsBody(subpart, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
					subpart.Physics.IsPhantom = false;
					(subpart.Physics as MyPhysicsBody).CreateFromCollisionObject(hkListShape, Vector3.Zero, base.WorldMatrix);
					subpart.Physics.Enabled = true;
					hkListShape.Base.RemoveReference();
				}
			}
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_HavokSystemIDChanged;
			base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
			}
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			if (m_subparts != null && m_subparts.Count != 0 && obj.Physics != null && m_subparts[0].Physics != null)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateHavokCollisionSystemID(base.CubeGrid);
		}

		private void UpdateHavokCollisionSystemID(MyEntity obj)
		{
			if (obj != null && !obj.MarkedForClose && obj.GetPhysicsBody() != null && m_subparts[0].GetPhysicsBody() != null && obj.GetPhysicsBody().HavokCollisionSystemID != m_subparts[0].GetPhysicsBody().HavokCollisionSystemID)
			{
				UpdateHavokCollisionSystemID(obj.GetPhysicsBody().HavokCollisionSystemID);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_AdvancedDoor obj = (MyObjectBuilder_AdvancedDoor)base.GetObjectBuilderCubeBlock(copy);
			obj.Open = m_open;
			return obj;
		}

		protected float UpdatePowerInput()
		{
			if (!Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			if (OpeningSpeed == 0f)
			{
				return BlockDefinition.PowerConsumptionIdle;
			}
			return BlockDefinition.PowerConsumptionMoving;
		}

		private void StartSound(int emitterId, MySoundPair cuePair)
		{
			if (m_emitter[emitterId].Sound == null || !m_emitter[emitterId].Sound.IsPlaying || (!(m_emitter[emitterId].SoundId == cuePair.Arcade) && !(m_emitter[emitterId].SoundId == cuePair.Realistic)))
			{
				m_emitter[emitterId].StopSound(forced: true);
				m_emitter[emitterId].PlaySingleSound(cuePair);
			}
		}

		public override void UpdateSoundEmitters()
		{
			for (int i = 0; i < m_emitter.Count; i++)
			{
				if (m_emitter[i] != null)
				{
					m_emitter[i].Update();
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			bool isPowered = base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			SequencePreparation(isPowered);
			if (base.CubeGrid.Physics != null)
			{
				UpdateDoorPosition(isPowered);
			}
		}

		private void SequencePreparation(bool isPowered)
		{
			GetOpenCloseStatus(out var fullyOpen, out var fullyClosed);
			bool flag = false;
			if (fullyClosed)
			{
				m_time = 0f;
				if (!m_open)
				{
					flag = true;
				}
			}
			else if (fullyOpen)
			{
				if (m_totalTime != m_time)
				{
					m_totalTime = m_time;
				}
				if ((bool)m_open)
				{
					flag = true;
				}
			}
			for (int i = 0; i < m_openingSequence.Count; i++)
			{
				float maxOpen = m_openingSequence[i].MaxOpen;
				if ((base.Open && m_currentOpening[i] == maxOpen) || (!base.Open && m_currentOpening[i] == 0f))
				{
					if (!Sync.IsDedicated && m_emitter[i] != null && m_emitter[i].IsPlaying && m_emitter[i].Loop)
					{
						m_emitter[i].StopSound(forced: false);
					}
					m_currentSpeed[i] = 0f;
				}
				if (!Sync.IsDedicated)
				{
					UpdateSounds(i, isPowered);
				}
			}
			if (m_stateChange && (((bool)m_open && fullyOpen) || (!m_open && fullyClosed)))
			{
				base.ResourceSink.Update();
				RaisePropertiesChanged();
				if (!m_open)
				{
					this.DoorStateChanged.InvokeIfNotNull(m_open);
					this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
				}
				m_stateChange = false;
			}
			if (!flag)
			{
				UpdateCurrentOpening(isPowered);
			}
			else
			{
				for (int j = 0; j < m_currentlyAtLimit.Count; j++)
				{
					m_currentlyAtLimit[j] = true;
				}
			}
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void UpdateSounds(int index, bool isPowered)
		{
<<<<<<< HEAD
			if (Enabled && base.ResourceSink != null && isPowered && m_currentSpeed[index] != 0f)
=======
			if (base.Enabled && base.ResourceSink != null && isPowered && m_currentSpeed[index] != 0f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				string text = "";
				text = ((!base.Open) ? m_openingSequence[index].CloseSound : m_openingSequence[index].OpenSound);
				if (!string.IsNullOrEmpty(text))
				{
					StartSound(index, new MySoundPair(text));
				}
			}
			else if (m_emitter[index] != null)
			{
				m_emitter[index].StopSound(forced: false);
			}
		}

		private void UpdateCurrentOpening(bool isPowered)
		{
<<<<<<< HEAD
			if (!(Enabled && base.ResourceSink != null && isPowered))
=======
			if (!(base.Enabled && base.ResourceSink != null && isPowered))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) / 1000f;
			m_time += (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) / 1000f * (m_open ? 1f : (-1f));
			m_time = MathHelper.Clamp(m_time, 0f, m_totalTime);
			for (int i = 0; i < m_openingSequence.Count; i++)
			{
				float num2 = (m_open ? m_openingSequence[i].OpenDelay : m_openingSequence[i].CloseDelay);
				if (((bool)m_open && m_time > num2) || (!m_open && m_time < m_totalTime - num2))
				{
					float num3 = m_currentSpeed[i] * num;
					float maxOpen = m_openingSequence[i].MaxOpen;
					if (m_openingSequence[i].SequenceType == MyObjectBuilder_AdvancedDoorDefinition.Opening.Sequence.Linear)
					{
						float num4 = MathHelper.Clamp(m_currentOpening[i] + num3, 0f, maxOpen);
						m_currentlyAtLimit[i] = Math.Abs(num4 - m_currentOpening[i]) < 0.001f;
						m_currentOpening[i] = num4;
					}
				}
			}
		}

		private void UpdateDoorPosition(bool isPowered, bool simplicicationOverride = false)
		{
			if (base.CubeGrid.Physics == null)
			{
				return;
			}
<<<<<<< HEAD
			if ((!Enabled || base.ResourceSink == null || !isPowered) && !simplicicationOverride)
=======
			if ((!base.Enabled || base.ResourceSink == null || !isPowered) && !simplicicationOverride)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				UpdateLinearVelocities();
				return;
			}
			GetSubpartLimits(ref m_isSubpartAtLimits);
			for (int i = 0; i < m_subpartCount; i++)
			{
				if (!m_isSubpartAtLimits[i] || simplicicationOverride)
				{
					transMat[i] = Matrix.Identity;
					rotMat[i] = Matrix.Identity;
				}
			}
			for (int j = 0; j < m_sequenceCount; j++)
			{
				int num = m_subpartIDs[j];
				if (m_currentlyAtLimit[j] && m_isSubpartAtLimits[num] && !simplicicationOverride)
				{
					continue;
				}
				MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType move = m_openingSequence[j].Move;
				float num2 = m_currentOpening[j];
				if (m_subparts.Count == 0 || num < 0)
				{
					break;
				}
				if (m_subparts[num] == null || m_subparts[num].Physics == null)
				{
					continue;
				}
				switch (move)
				{
				case MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType.Slide:
					transMat[num].Translation += m_openingSequence[j].SlideDirection * new Vector3(num2);
					break;
				case MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType.Rotate:
				{
					float num3 = (m_openingSequence[j].InvertRotation ? (-1f) : 1f);
					float num4 = 0f;
					float num5 = 0f;
					float num6 = 0f;
					if (m_openingSequence[j].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.X)
					{
						num4 = MathHelper.ToRadians(num2 * num3);
					}
					else if (m_openingSequence[j].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.Y)
					{
						num5 = MathHelper.ToRadians(num2 * num3);
					}
					else if (m_openingSequence[j].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.Z)
					{
						num6 = MathHelper.ToRadians(num2 * num3);
					}
					Vector3 vector = ((!m_openingSequence[j].PivotPosition.HasValue) ? m_hingePosition[num] : ((Vector3)m_openingSequence[j].PivotPosition.Value));
					bool num7 = Vector3.IsZero(vector);
					if (!num7)
					{
						rotMat[num].Translation -= vector;
					}
					if (num4 != 0f)
					{
						rotMat[num] *= Matrix.CreateRotationX(num4);
					}
					else if (num5 != 0f)
					{
						rotMat[num] *= Matrix.CreateRotationY(num5);
					}
					else if (num6 != 0f)
					{
						rotMat[num] *= Matrix.CreateRotationZ(num6);
					}
					if (!num7)
					{
						rotMat[num].Translation += vector;
					}
					break;
				}
				}
			}
			UpdateLinearVelocities();
			for (int k = 0; k < m_subpartCount; k++)
			{
				if (!m_isSubpartAtLimits[k] || simplicicationOverride)
				{
					Matrix localMatrix = rotMat[k] * transMat[k];
					m_subparts[k].PositionComp.SetLocalMatrix(ref localMatrix);
				}
			}
		}

		private void UpdateLinearVelocities()
		{
			for (int i = 0; i < m_subpartCount; i++)
			{
				if (m_subparts[i].Physics != null)
				{
					if (m_subparts[i].Physics.LinearVelocity != base.CubeGrid.Physics.LinearVelocity)
					{
						m_subparts[i].Physics.LinearVelocity = base.CubeGrid.Physics.LinearVelocity;
					}
					if (m_subparts[i].Physics.AngularVelocity != base.CubeGrid.Physics.AngularVelocity)
					{
						m_subparts[i].Physics.AngularVelocity = base.CubeGrid.Physics.AngularVelocity;
					}
				}
			}
		}

		private bool IsSubpartAtLimit(int subpartId)
		{
			bool flag = true;
			for (int i = 0; i < m_openingSequence.Count; i++)
<<<<<<< HEAD
			{
				if (m_subpartIDs[i] == subpartId)
				{
					flag &= m_currentlyAtLimit[i];
				}
			}
			return flag;
		}

		private void GetSubpartLimits(ref bool[] limits)
		{
			if (limits == null || limits.Length < m_subparts.Count)
			{
=======
			{
				if (m_subpartIDs[i] == subpartId)
				{
					flag &= m_currentlyAtLimit[i];
				}
			}
			return flag;
		}

		private void GetSubpartLimits(ref bool[] limits)
		{
			if (limits == null || limits.Length < m_subparts.Count)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				limits = new bool[m_subparts.Count];
			}
			for (int i = 0; i < limits.Length; i++)
			{
				limits[i] = true;
			}
			for (int j = 0; j < m_openingSequence.Count; j++)
			{
				if (!m_currentlyAtLimit[j])
				{
					limits[m_subpartIDs[j]] = m_currentlyAtLimit[j];
				}
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			oldGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_HavokSystemIDChanged;
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
			}
			base.OnCubeGridChanged(oldGrid);
		}

		private void CubeGrid_HavokSystemIDChanged(int id)
		{
			bool flag = true;
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				flag &= subpart.Physics?.IsInWorld ?? false;
			}
			if (flag)
			{
				UpdateHavokCollisionSystemID(id);
			}
		}

		internal void UpdateHavokCollisionSystemID(int HavokCollisionSystemID)
		{
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				if (subpart != null && subpart.Physics != null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
				{
					uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(15, HavokCollisionSystemID, 1, 1);
					subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
					if (subpart.GetPhysicsBody().HavokWorld != null)
					{
						MyPhysics.RefreshCollisionFilter(subpart.GetPhysicsBody());
					}
				}
			}
		}

		protected override void Closing()
		{
			for (int i = 0; i < m_emitter.Count; i++)
			{
				if (m_emitter[i] != null)
				{
					m_emitter[i].StopSound(forced: true);
				}
			}
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
			base.Closing();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			InitSubparts();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateEmissivity();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public override void ContactCallbackInternal()
		{
			base.ContactCallbackInternal();
		}

		public override bool EnableContactCallbacks()
		{
			return false;
		}

		public override bool IsClosing()
		{
			if (!m_open)
			{
				return OpenRatio > 0f;
			}
			return false;
		}
	}
}
