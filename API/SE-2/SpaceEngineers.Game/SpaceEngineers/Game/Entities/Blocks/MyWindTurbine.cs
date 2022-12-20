using System;
using System.Collections.Generic;
using System.Text;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Localization;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using SpaceEngineers.Game.EntityComponents.Renders;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_WindTurbine))]
	public class MyWindTurbine : MyEnvironmentalPowerProducer
	{
		public class TurbineSubpart : MyEntitySubpart
		{
			public new MyWindTurbine Parent => (MyWindTurbine)base.Parent;

			public new MyRenderComponentWindTurbine.TurbineRenderComponent Render => (MyRenderComponentWindTurbine.TurbineRenderComponent)base.Render;

			public override void InitComponents()
			{
				base.Render = new MyRenderComponentWindTurbine.TurbineRenderComponent();
				base.InitComponents();
			}
		}

		private HashSet<IMyEntity> m_children = new HashSet<IMyEntity>();

		private int m_nextUpdateRay;

		private float m_effectivity;

		private bool m_paralleRaycastRunning;

		private readonly Action<MyPhysics.HitInfo?> m_onRaycastCompleted;

		private readonly Action<List<MyPhysics.HitInfo>> m_onRaycastCompletedList;

		private List<MyPhysics.HitInfo> m_cachedHitList = new List<MyPhysics.HitInfo>();

		private Action m_updateEffectivity;

		protected float Effectivity
		{
			get
			{
				return m_effectivity;
			}
			set
			{
				if (m_effectivity != value)
				{
					m_effectivity = value;
					OnProductionChanged();
					UpdateVisuals();
				}
			}
		}

		protected override float CurrentProductionRatio
		{
			get
			{
				if (!Enabled || !base.IsWorking)
				{
					return 0f;
				}
				return m_effectivity * Math.Min(1f, GetOrCreateSharedComponent().WindSpeed / BlockDefinition.OptimalWindSpeed);
			}
		}

		public new MyWindTurbineDefinition BlockDefinition => (MyWindTurbineDefinition)base.BlockDefinition;

		public float[] RayEffectivities { get; private set; }
<<<<<<< HEAD

		private bool IsInAtmosphere
		{
			get
			{
				Vector3D position = base.PositionComp.GetPosition();
				MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(position);
				if (closestPlanet == null)
				{
					return false;
				}
				return closestPlanet.GetAirDensity(position) > 0f;
			}
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyWindTurbine()
		{
			m_updateEffectivity = UpdateEffectivity;
			m_onRaycastCompleted = OnRaycastCompleted;
			m_onRaycastCompletedList = OnRaycastCompleted;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyObjectBuilder_WindTurbine myObjectBuilder_WindTurbine = (MyObjectBuilder_WindTurbine)objectBuilder;
			RayEffectivities = myObjectBuilder_WindTurbine.ImmediateEffectivities;
			if (RayEffectivities == null)
			{
				RayEffectivities = new float[BlockDefinition.RaycastersCount];
			}
			base.Init(objectBuilder, cubeGrid);
<<<<<<< HEAD
			base.SourceComp.Enabled = Enabled;
=======
			base.SourceComp.Enabled = base.Enabled;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override void InitComponents()
		{
			base.Render = new MyRenderComponentWindTurbine();
			base.InitComponents();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_WindTurbine obj = (MyObjectBuilder_WindTurbine)base.GetObjectBuilderCubeBlock(copy);
			obj.ImmediateEffectivities = (float[])RayEffectivities.Clone();
			return obj;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			GetOrCreateSharedComponent().UpdateWindSpeed();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			GetOrCreateSharedComponent().Update10();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			GetOrCreateSharedComponent().UpdateWindSpeed();
		}

		public void UpdateNextRay()
		{
			if (!m_paralleRaycastRunning)
			{
				m_paralleRaycastRunning = true;
				GetRaycaster(m_nextUpdateRay, out var start, out var end);
				if (m_nextUpdateRay != 0)
				{
					MyPhysics.CastRayParallel(ref start, ref end, 0, m_onRaycastCompleted);
					return;
				}
				m_cachedHitList.AssertEmpty();
				MyPhysics.CastRayParallel(ref start, ref end, m_cachedHitList, 28, m_onRaycastCompletedList);
			}
		}

		private void OnRaycastCompleted(List<MyPhysics.HitInfo> hitList)
		{
			using (hitList.GetClearToken())
			{
				foreach (MyPhysics.HitInfo hit in hitList)
				{
					if (hit.HkHitInfo.Body.GetEntity(0u) is MyVoxelBase)
					{
						OnRaycastCompleted(hit);
						return;
					}
				}
				OnRaycastCompleted((MyPhysics.HitInfo?)null);
			}
		}

		private void OnRaycastCompleted(MyPhysics.HitInfo? hitInfo)
		{
			float num = 1f;
			if (hitInfo.HasValue)
			{
				float hitFraction = hitInfo.Value.HkHitInfo.HitFraction;
				float minRaycasterClearance = BlockDefinition.MinRaycasterClearance;
				num = ((!(hitFraction <= minRaycasterClearance)) ? ((hitFraction - minRaycasterClearance) / (1f - minRaycasterClearance)) : 0f);
			}
			RayEffectivities[m_nextUpdateRay] = num;
			m_nextUpdateRay++;
			if (m_nextUpdateRay >= BlockDefinition.RaycastersCount)
			{
				m_nextUpdateRay = 0;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.MarkedForClose)
				{
					UpdateEffectivity();
					m_paralleRaycastRunning = false;
				}
			}, "Turbine update");
		}

		private void UpdateEffectivity()
		{
			if (!base.IsWorking || !IsInAtmosphere)
			{
				Effectivity = 0f;
				return;
			}
			float num = 0f;
			for (int i = 1; i < RayEffectivities.Length; i++)
			{
				num += RayEffectivities[i];
			}
			num /= BlockDefinition.RaycastersToFullEfficiency;
			num *= MathHelper.Lerp(0.5f, 1f, RayEffectivities[0]);
			num = (Effectivity = num * GetOrCreateSharedComponent().WindSpeedModifier);
		}

		public void GetRaycaster(int id, out Vector3D start, out Vector3D end)
		{
			MatrixD worldMatrix = base.WorldMatrix;
			start = worldMatrix.Translation;
			if (id == 0)
			{
				end = start + GetOrCreateSharedComponent().GravityNormal * BlockDefinition.OptimalGroundClearance;
				return;
			}
			int num = RayEffectivities.Length - 1;
			float angle = (float)Math.PI * 2f / (float)num * (float)(id - 1);
			int raycasterSize = BlockDefinition.RaycasterSize;
			end = start + raycasterSize * (MyMath.FastSin(angle) * worldMatrix.Left + MyMath.FastCos(angle) * worldMatrix.Forward);
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			GetOrCreateSharedComponent().Register(this);
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			GetOrCreateSharedComponent().Unregister(this);
		}

		private MySharedWindComponent GetOrCreateSharedComponent()
		{
			MyEntityComponentContainer components = base.CubeGrid.Components;
			MySharedWindComponent mySharedWindComponent = components.Get<MySharedWindComponent>();
			if (mySharedWindComponent == null)
			{
				mySharedWindComponent = new MySharedWindComponent();
				components.Add(mySharedWindComponent);
			}
			return mySharedWindComponent;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.IsWorking)
			{
				OnStartWorking();
			}
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			OnIsWorkingChanged();
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			OnIsWorkingChanged();
		}

		private void OnIsWorkingChanged()
		{
			float effectivity = Effectivity;
			UpdateEffectivity();
			if (Effectivity == effectivity)
			{
				UpdateVisuals();
			}
		}

		public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
		{
<<<<<<< HEAD
			base.Hierarchy.GetChildrenRecursive(m_children);
			foreach (MyEntity child in m_children)
			{
				MyModel model = child.Model;
				if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB(child, ref aabb))
				{
					return true;
				}
			}
=======
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			base.Hierarchy.GetChildrenRecursive(m_children);
			Enumerator<IMyEntity> enumerator = m_children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyEntity myEntity = (MyEntity)enumerator.get_Current();
					MyModel model = myEntity.Model;
					if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB(myEntity, ref aabb))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return base.Model?.GetTrianglePruningStructure().GetIntersectionWithAABB(this, ref aabb) ?? false;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			if (!Enabled)
			{
				UpdateVisuals();
			}
		}

		public override void RefreshModels(string modelPath, string modelCollisionPath)
		{
			base.RefreshModels(modelPath, modelCollisionPath);
			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			if (!MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, GetEmissiveState(), out var result))
			{
				result.EmissiveColor = Color.Green;
			}
			float speed = CurrentProductionRatio * BlockDefinition.TurbineRotationSpeed;
			foreach (TurbineSubpart value in base.Subparts.Values)
			{
				value.Render.SetSpeed(speed);
				value.Render.SetColor(result.EmissiveColor);
			}
		}

		private MyStringHash GetEmissiveState()
		{
			CheckIsWorking();
			if (base.IsWorking)
			{
				if (GetOrCreateSharedComponent().IsEnabled && Effectivity > 0f)
				{
					return MyCubeBlock.m_emissiveNames.Working;
				}
				return MyCubeBlock.m_emissiveNames.Warning;
			}
			if (base.IsFunctional)
			{
				return MyCubeBlock.m_emissiveNames.Disabled;
			}
			return MyCubeBlock.m_emissiveNames.Damaged;
		}

		public void OnEnvironmentChanged()
		{
			UpdateVisuals();
			OnProductionChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder sb)
		{
			base.UpdateDetailedInfo(sb);
			MyTexts.AppendFormat(arg0: ((double)Effectivity > 0.95) ? MySpaceTexts.Turbine_WindClearanceOptimal : ((Effectivity > 0.6f) ? MySpaceTexts.Turbine_WindClearanceGood : ((!(Effectivity > 0f)) ? MySpaceTexts.Turbine_WindClearanceNone : MySpaceTexts.Turbine_WindClearancePoor)), stringBuilder: sb, textEnum: MySpaceTexts.Turbine_WindClearance);
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			return new TurbineSubpart();
		}
	}
}
