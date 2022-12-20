using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGroupControlSystem : IMyGridControlSystem
	{
		private MyShipController m_currentShipController;

		private readonly CachingHashSet<MyShipController> m_groupControllers = new CachingHashSet<MyShipController>();

		private readonly HashSet<MyCubeGrid> m_cubeGrids = new HashSet<MyCubeGrid>();

		private MyCubeGrid m_controllerGrid;

		private MyCubeGrid m_targetUpdateGrid;

		private bool m_controlDirty;

		private bool m_firstControlRecalculation;

		private MyEntity m_relativeDampeningEntity;

		private readonly Action m_updateControlDelegate;

		public MyEntity RelativeDampeningEntity
		{
			get
			{
				return m_relativeDampeningEntity;
			}
			set
			{
<<<<<<< HEAD
=======
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_relativeDampeningEntity == value)
				{
					return;
				}
				if (m_relativeDampeningEntity != null)
				{
					m_relativeDampeningEntity.OnClose -= RelativeDampeningEntityClosed;
				}
				m_relativeDampeningEntity = value;
				if (m_relativeDampeningEntity != null)
				{
					m_relativeDampeningEntity.OnClose += RelativeDampeningEntityClosed;
				}
<<<<<<< HEAD
				foreach (MyCubeGrid cubeGrid in m_cubeGrids)
				{
					cubeGrid.EntityThrustComponent?.SetRelativeDampeningEntity(value);
=======
				Enumerator<MyCubeGrid> enumerator = m_cubeGrids.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current().EntityThrustComponent?.SetRelativeDampeningEntity(value);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		private MyShipController CurrentShipController
		{
			get
			{
				return m_currentShipController;
			}
			set
			{
				if (value != m_currentShipController)
				{
					m_controllerGrid?.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, UpdateControls);
					if (value == null)
					{
						m_controllerGrid = null;
						MyShipController currentShipController = m_currentShipController;
						m_currentShipController = value;
						MyGridPhysicalHierarchy.Static.UpdateRoot(currentShipController.CubeGrid);
						currentShipController.CubeGrid.CheckPredictionFlagScheduling();
<<<<<<< HEAD
						this.IsControlledChanged.InvokeIfNotNull(arg1: false, m_cubeGrids.FirstOrDefault());
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						MyCubeGrid cubeGrid = value.CubeGrid;
						cubeGrid.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, UpdateControls, 4, parallel: true);
						m_targetUpdateGrid = (m_controllerGrid = cubeGrid);
						m_currentShipController = value;
						MyGridPhysicalHierarchy.Static.UpdateRoot(m_currentShipController.CubeGrid);
						cubeGrid.CheckPredictionFlagScheduling();
<<<<<<< HEAD
						this.IsControlledChanged.InvokeIfNotNull(arg1: true, cubeGrid);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public bool IsLocallyControlled => GetController()?.Player.IsLocalPlayer ?? false;

		public bool IsControlled => GetController() != null;

<<<<<<< HEAD
		bool IMyGridControlSystem.IsControlled => IsControlled;

		public event Action<bool, MyCubeGrid> IsControlledChanged;

		event Action<bool, IMyCubeGrid> IMyGridControlSystem.IsControlledChanged
		{
			add
			{
				IsControlledChanged += GetDelegate(value);
			}
			remove
			{
				IsControlledChanged -= GetDelegate(value);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void RelativeDampeningEntityClosed(MyEntity entity)
		{
			m_relativeDampeningEntity = null;
		}

		public MyGroupControlSystem()
		{
			CurrentShipController = null;
			m_controlDirty = false;
			m_firstControlRecalculation = true;
			m_updateControlDelegate = UpdateControl;
		}

		public void UpdateBeforeSimulation100()
		{
			if (RelativeDampeningEntity != null && CurrentShipController != null)
			{
				MyEntityThrustComponent.UpdateRelativeDampeningEntity(CurrentShipController, RelativeDampeningEntity);
			}
		}

		private void UpdateControl()
		{
<<<<<<< HEAD
=======
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_groupControllers.ApplyChanges();
			MyShipController myShipController = null;
			Enumerator<MyShipController> enumerator = m_groupControllers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyShipController current = enumerator.get_Current();
					if (myShipController == null)
					{
						myShipController = current;
					}
					else if (MyShipController.HasPriorityOver(current, myShipController))
					{
						myShipController = current;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			CurrentShipController = myShipController;
			if (Sync.IsServer && CurrentShipController != null)
			{
				_ = CurrentShipController.ControllerInfo.Controller;
				Enumerator<MyCubeGrid> enumerator2 = m_cubeGrids.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyCubeGrid current2 = enumerator2.get_Current();
						if (CurrentShipController.ControllerInfo.Controller != null)
						{
							Sync.Players.TryExtendControl(CurrentShipController, current2);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
				{
					CurrentShipController.GridWheels.InitControl(CurrentShipController.Entity);
				}
			}
			m_controlDirty = false;
			m_firstControlRecalculation = false;
		}

		public void RemoveControllerBlock(MyShipController controllerBlock)
		{
			m_groupControllers.ApplyAdditions();
			if (m_groupControllers.Contains(controllerBlock))
			{
				m_groupControllers.Remove(controllerBlock);
			}
			if (controllerBlock == CurrentShipController)
			{
				m_controlDirty = true;
				ScheduleControlUpdate();
			}
			if (Sync.IsServer && controllerBlock == CurrentShipController)
			{
				Sync.Players.ReduceAllControl(CurrentShipController);
				CurrentShipController = null;
			}
		}

		private void ScheduleControlUpdate()
		{
<<<<<<< HEAD
			m_targetUpdateGrid = m_controllerGrid ?? m_cubeGrids.FirstOrDefault();
=======
			m_targetUpdateGrid = m_controllerGrid ?? Enumerable.FirstOrDefault<MyCubeGrid>((IEnumerable<MyCubeGrid>)m_cubeGrids);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_targetUpdateGrid?.Schedule(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, m_updateControlDelegate);
		}

		public void AddControllerBlock(MyShipController controllerBlock)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			m_groupControllers.Add(controllerBlock);
			bool flag = false;
			if (CurrentShipController != null && CurrentShipController.CubeGrid != controllerBlock.CubeGrid)
			{
				MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(controllerBlock.CubeGrid);
				if (group != null)
				{
					Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.get_Current().NodeData == CurrentShipController.CubeGrid)
							{
								flag = true;
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
				}
			}
			if (!flag && CurrentShipController != null && CurrentShipController.CubeGrid != controllerBlock.CubeGrid)
			{
				RemoveControllerBlock(CurrentShipController);
				CurrentShipController = null;
			}
			bool flag2 = CurrentShipController == null || MyShipController.HasPriorityOver(controllerBlock, CurrentShipController);
			if (flag2)
			{
				m_controlDirty = true;
				ScheduleControlUpdate();
			}
			if (Sync.IsServer && CurrentShipController != null && flag2)
			{
				Sync.Players.ReduceAllControl(CurrentShipController);
			}
		}

		public void RemoveGrid(MyCubeGrid cubeGrid)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if (Sync.IsServer && CurrentShipController != null && CurrentShipController.ControllerInfo.Controller != null)
			{
				Sync.Players.ReduceControl(CurrentShipController, cubeGrid);
			}
			m_cubeGrids.Remove(cubeGrid);
			cubeGrid.EntityThrustComponent?.SetRelativeDampeningEntity(RelativeDampeningEntity);
<<<<<<< HEAD
			foreach (MyShipController groupController in m_groupControllers)
			{
				if (groupController.CubeGrid == cubeGrid)
				{
					m_groupControllers.Remove(groupController);
					if (m_currentShipController == groupController)
					{
						CurrentShipController = null;
					}
				}
			}
=======
			Enumerator<MyShipController> enumerator = m_groupControllers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyShipController current = enumerator.get_Current();
					if (current.CubeGrid == cubeGrid)
					{
						m_groupControllers.Remove(current);
						if (m_currentShipController == current)
						{
							CurrentShipController = null;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_targetUpdateGrid == cubeGrid)
			{
				m_targetUpdateGrid.DeSchedule(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, UpdateControl);
				if (m_controllerGrid == cubeGrid)
				{
					m_controllerGrid = null;
				}
				ScheduleControlUpdate();
			}
		}

		public void AddGrid(MyCubeGrid cubeGrid)
		{
			m_cubeGrids.Add(cubeGrid);
			cubeGrid.EntityThrustComponent?.SetRelativeDampeningEntity(null);
<<<<<<< HEAD
			if (m_controlDirty && m_cubeGrids.Count == 1)
=======
			if (m_controlDirty && m_cubeGrids.get_Count() == 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				ScheduleControlUpdate();
			}
			if (Sync.IsServer && !m_controlDirty && CurrentShipController != null && CurrentShipController.ControllerInfo.Controller != null)
			{
				Sync.Players.ExtendControl(CurrentShipController, cubeGrid);
			}
		}

		public MyEntityController GetController()
		{
			return CurrentShipController?.ControllerInfo.Controller;
		}

		public MyShipController GetShipController()
		{
			return CurrentShipController;
		}

		public void DebugDraw(float startYCoord)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "Controlled group controllers:", Color.GreenYellow, 0.5f);
			startYCoord += 13f;
			Enumerator<MyShipController> enumerator = m_groupControllers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyShipController current = enumerator.get_Current();
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "  " + current.ToString(), Color.LightYellow, 0.5f);
					startYCoord += 13f;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "Controlled group grids:", Color.GreenYellow, 0.5f);
			startYCoord += 13f;
			Enumerator<MyCubeGrid> enumerator2 = m_cubeGrids.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyCubeGrid current2 = enumerator2.get_Current();
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "  " + current2.ToString(), Color.LightYellow, 0.5f);
					startYCoord += 13f;
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "  " + CurrentShipController, Color.OrangeRed, 0.5f);
			startYCoord += 13f;
		}

		public void UpdateControls()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyShipController> enumerator = m_groupControllers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdateControls();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void Clear()
		{
			m_groupControllers.ApplyChanges();
			m_currentShipController = null;
			m_targetUpdateGrid = (m_controllerGrid = null);
			m_controlDirty = (m_firstControlRecalculation = true);
			m_cubeGrids.Clear();
			m_groupControllers.Clear();
			m_relativeDampeningEntity = null;
		}
<<<<<<< HEAD

		private Action<bool, MyCubeGrid> GetDelegate(Action<bool, IMyCubeGrid> value)
		{
			return (Action<bool, MyCubeGrid>)Delegate.CreateDelegate(typeof(Action<bool, MyCubeGrid>), value.Target, value.Method);
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
