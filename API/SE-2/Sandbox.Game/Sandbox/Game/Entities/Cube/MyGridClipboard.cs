using System;
using System.Collections.Generic;
using System.Text;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridClipboard
	{
		private struct CopiedGridInfo
		{
			public int PCUs;

			public int TotalBlocks;

			public bool HasGridOverLimits;
		}

		private struct CopiedGridInfo
		{
			public int PCUs;

			public int TotalBlocks;

			public bool HasGridOverLimits;
		}

		protected delegate void UpdateAfterPasteCallback(List<MyObjectBuilder_CubeGrid> pastedBuilders);

		protected static readonly MyStringId ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");

		protected static readonly MyStringId ID_GIZMO_DRAW_LINE_RED = MyStringId.GetOrCompute("GizmoDrawLineRed");

		private static bool BUG_SE_19306_FIRED = false;

		[ThreadStatic]
		private static HashSet<IMyEntity> m_cacheEntitySet;

		private CopiedGridInfo m_copiedGridsInfo;

		private CopiedGridInfo m_copiedGridsInfo;

		protected readonly List<MyObjectBuilder_CubeGrid> m_copiedGrids = new List<MyObjectBuilder_CubeGrid>();

		protected readonly List<Vector3> m_copiedGridOffsets = new List<Vector3>();

		protected List<MyCubeGrid> m_previewGrids = new List<MyCubeGrid>();

		private List<MyCubeGrid> m_previewGridsParallel = new List<MyCubeGrid>();

		private List<MyObjectBuilder_CubeGrid> m_copiedGridsParallel = new List<MyObjectBuilder_CubeGrid>();

		private readonly MyComponentList m_buildComponents = new MyComponentList();

		protected Vector3D m_pastePosition;

		protected Vector3D m_pastePositionPrevious;

		protected bool m_calculateVelocity = true;

		protected Vector3 m_objectVelocity = Vector3.Zero;

		protected float m_pasteOrientationAngle;

		protected Vector3 m_pasteDirUp = new Vector3(1f, 0f, 0f);

		protected Vector3 m_pasteDirForward = new Vector3(0f, 1f, 0f);

		protected float m_dragDistance;

		protected const float m_maxDragDistance = 20000f;

		protected Vector3 m_dragPointToPositionLocal;

		protected bool m_canBePlaced;

		private bool m_canBePlacedNeedsRefresh = true;

		protected bool m_characterHasEnoughMaterials;

		protected MyPlacementSettings m_settings;

		private long? m_spawnerId;

		private Vector3D m_originalSpawnerPosition = Vector3D.Zero;

		private readonly List<MyPhysics.HitInfo> m_raycastCollisionResults = new List<MyPhysics.HitInfo>();

		protected float m_closestHitDistSq = float.MaxValue;

		protected Vector3D m_hitPos = new Vector3(0f, 0f, 0f);

		protected Vector3 m_hitNormal = new Vector3(1f, 0f, 0f);

		protected IMyEntity m_hitEntity;

		protected bool m_visible = true;

		private bool m_allowSwitchCameraMode = true;

		protected bool m_useDynamicPreviews;

		protected Dictionary<string, int> m_blocksPerType = new Dictionary<string, int>();

		/// <summary>
		/// Grids that are around pasted grid. (In proximity, possible for merge)
		/// </summary>
		protected List<MyCubeGrid> m_touchingGrids = new List<MyCubeGrid>();

		private Task ActivationTask;

		private readonly List<IMyEntity> m_resultIDs = new List<IMyEntity>();

		private bool m_isBeingAdded;

		protected bool m_enableUpdateHitEntity = true;

		private bool m_enableStationRotation;

		public bool ShowModdedBlocksWarning = true;

		private bool m_isAligning;

		private int m_lastFrameAligned;

		public Vector3D PastePosition => m_pastePosition - Vector3.TransformNormal(m_dragPointToPositionLocal, GetFirstGridOrientationMatrix());

		protected virtual bool CanBePlaced
		{
			get
			{
				if (m_canBePlacedNeedsRefresh)
				{
					m_canBePlaced = TestPlacement();
				}
				return m_canBePlaced;
			}
		}

		public bool CharacterHasEnoughMaterials => m_characterHasEnoughMaterials;

		public virtual bool HasPreviewBBox
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool IsActive { get; protected set; }

		public bool AllowSwitchCameraMode
		{
			get
			{
				return m_allowSwitchCameraMode;
			}
			private set
			{
				m_allowSwitchCameraMode = value;
			}
		}

		public bool IsSnapped { get; protected set; }

		public List<MyObjectBuilder_CubeGrid> CopiedGrids => m_copiedGrids;

		public SnapMode SnapMode
		{
			get
			{
				if (m_previewGrids.Count == 0)
				{
					return SnapMode.Base6Directions;
				}
				return m_settings.GetGridPlacementSettings(m_previewGrids[0].GridSizeEnum).SnapMode;
			}
		}

		public bool EnablePreciseRotationWhenSnapped
		{
			get
			{
				if (m_previewGrids.Count == 0)
				{
					return false;
				}
				if (m_settings.GetGridPlacementSettings(m_previewGrids[0].GridSizeEnum).EnablePreciseRotationWhenSnapped)
				{
					return EnableStationRotation;
				}
				return false;
			}
		}

		public bool OneAxisRotationMode
		{
			get
			{
				if (IsSnapped)
				{
					return SnapMode == SnapMode.OneFreeAxis;
				}
				return false;
			}
		}

		public List<MyCubeGrid> PreviewGrids => m_previewGrids;

		protected virtual bool AnyCopiedGridIsStatic
		{
			get
			{
				foreach (MyCubeGrid previewGrid in m_previewGrids)
				{
					if (previewGrid.IsStatic)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool EnableStationRotation
		{
			get
			{
				if (m_enableStationRotation)
				{
					return MyFakes.ENABLE_STATION_ROTATION;
				}
				return false;
			}
			set
			{
				if (m_enableStationRotation != value)
				{
					m_enableStationRotation = value;
					if (IsActive && m_enableStationRotation)
					{
						AlignClipboardToGravity();
						MyCoordinateSystem.Static.Visible = false;
					}
					else if (IsActive && !m_enableStationRotation)
					{
						AlignRotationToCoordSys();
						MyCoordinateSystem.Static.Visible = true;
					}
				}
			}
		}

		public bool CreationMode { get; set; }

		public MyCubeSize CubeSize { get; set; }

		public bool IsStatic { get; set; }

		public bool IsBeingAdded
		{
			get
			{
				return m_isBeingAdded;
			}
			set
			{
				m_isBeingAdded = value;
			}
		}

		public int MaxVisiblePCU { get; set; } = int.MaxValue;


		protected virtual float Transparency => 0.25f;

		public string CopiedGridsName
		{
			get
			{
				if (HasCopiedGrids())
				{
					return m_copiedGrids[0].DisplayName;
				}
				return null;
			}
		}

		public event Action<MyGridClipboard, bool> Deactivated;

		public float GetGridHalfExtent(int axis)
		{
			if (m_previewGrids.Count <= 0)
			{
				return 0f;
			}
<<<<<<< HEAD
			switch (axis)
			{
			case 0:
				return m_previewGrids[0].PositionComp.LocalAABB.Extents.X;
			case 1:
				return m_previewGrids[0].PositionComp.LocalAABB.Extents.Y;
			case 2:
				return m_previewGrids[0].PositionComp.LocalAABB.Extents.Z;
			default:
				return 0f;
			}
=======
			return axis switch
			{
				0 => m_previewGrids[0].PositionComp.LocalAABB.Extents.X, 
				1 => m_previewGrids[0].PositionComp.LocalAABB.Extents.Y, 
				2 => m_previewGrids[0].PositionComp.LocalAABB.Extents.Z, 
				_ => 0f, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public MyGridClipboard(MyPlacementSettings settings, bool calculateVelocity = true)
		{
			m_calculateVelocity = calculateVelocity;
			m_settings = settings;
		}

		public MyCubeBlockDefinition GetFirstBlockDefinition(MyObjectBuilder_CubeGrid grid = null)
		{
			if (grid == null)
			{
				if (m_copiedGrids.Count <= 0)
				{
					return null;
				}
				grid = m_copiedGrids[0];
			}
			if (grid.CubeBlocks.Count > 0)
			{
				MyDefinitionId id = grid.CubeBlocks[0].GetId();
				return MyDefinitionManager.Static.GetCubeBlockDefinition(id);
			}
			return null;
		}

		public virtual void ActivateNoAlign(Action callback = null)
		{
			if (!ActivationTask.IsComplete || m_isBeingAdded)
			{
				return;
			}
			m_isBeingAdded = true;
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.PrepareSwapData();
				MyEntityIdentifier.SwapPerThreadData();
			}
			m_copiedGridsParallel.Clear();
			m_copiedGridsParallel.AddRange(m_copiedGrids);
			ActivationTask = Parallel.Start(delegate
			{
				ChangeClipboardPreview(visible: true, m_previewGridsParallel, m_copiedGridsParallel);
			}, delegate
			{
				if (m_visible)
				{
					foreach (MyCubeGrid item in m_previewGridsParallel)
					{
						MyEntities.Add(item);
						DisablePhysicsRecursively(item);
					}
				}
				List<MyCubeGrid> previewGridsParallel = m_previewGridsParallel;
				m_previewGridsParallel = m_previewGrids;
				m_previewGrids = previewGridsParallel;
				if (m_visible)
				{
					if (callback != null)
					{
						callback();
					}
					IsActive = true;
				}
				m_isBeingAdded = false;
			});
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.ClearSwapDataAndRestore();
			}
		}

		public virtual void Activate(Action callback = null)
		{
			if (!ActivationTask.IsComplete || m_isBeingAdded)
			{
				return;
			}
			MyHud.PushRotatingWheelVisible();
			m_isBeingAdded = true;
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.PrepareSwapData();
				MyEntityIdentifier.SwapPerThreadData();
			}
			m_copiedGridsParallel.Clear();
			m_copiedGridsParallel.AddRange(m_copiedGrids);
			ActivationTask = Parallel.Start(ActivateInternal, delegate
			{
				if (m_visible)
				{
					foreach (IMyEntity resultID in m_resultIDs)
					{
						MyEntityIdentifier.TryGetEntity(resultID.EntityId, out var entity);
						if (entity == null)
						{
							MyEntityIdentifier.AddEntityWithId(resultID);
						}
					}
					m_resultIDs.Clear();
					foreach (MyCubeGrid item in m_previewGridsParallel)
					{
						MyEntities.Add(item);
						DisablePhysicsRecursively(item);
					}
					if (callback != null)
					{
						callback();
					}
					IsActive = true;
				}
				List<MyCubeGrid> previewGridsParallel = m_previewGridsParallel;
				m_previewGridsParallel = m_previewGrids;
				m_previewGrids = previewGridsParallel;
				m_isBeingAdded = false;
				MyHud.PopRotatingWheelVisible();
			});
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.ClearSwapDataAndRestore();
			}
		}

		private void ActivateInternal()
		{
			ChangeClipboardPreview(visible: true, m_previewGridsParallel, m_copiedGridsParallel);
			if (EnableStationRotation)
			{
				AlignClipboardToGravity();
			}
			if (MyClipboardComponent.Static.Clipboard == this)
			{
				if (!EnableStationRotation)
				{
					MyCoordinateSystem.Static.Visible = true;
					AlignRotationToCoordSys();
				}
				MyCoordinateSystem.OnCoordinateChange += OnCoordinateChange;
			}
		}

		private void OnCoordinateChange()
		{
			if (EnableStationRotation)
			{
				return;
			}
			if (MyCoordinateSystem.Static.LocalCoordExist && AnyCopiedGridIsStatic)
			{
				EnableStationRotation = false;
				MyCoordinateSystem.Static.Visible = true;
			}
			if (!MyCoordinateSystem.Static.LocalCoordExist)
			{
				EnableStationRotation = true;
				MyCoordinateSystem.Static.Visible = false;
			}
			else
			{
				EnableStationRotation = false;
				MyCoordinateSystem.Static.Visible = true;
			}
			if (!m_enableStationRotation && IsActive && !m_isAligning)
			{
				m_isAligning = true;
				int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
				if (sessionTotalFrames - m_lastFrameAligned >= 12)
				{
					AlignRotationToCoordSys();
					m_lastFrameAligned = sessionTotalFrames;
				}
				m_isAligning = false;
			}
		}

		public virtual void Deactivate(bool afterPaste = false)
		{
			CreationMode = false;
			bool isActive = IsActive;
			ChangeClipboardPreview(visible: false, m_previewGrids, m_copiedGrids);
			IsActive = false;
			Action<MyGridClipboard, bool> deactivated = this.Deactivated;
			if (isActive)
			{
				deactivated?.Invoke(this, afterPaste);
			}
			if (MyClipboardComponent.Static.Clipboard == this)
			{
				if (MyCoordinateSystem.Static != null)
				{
					MyCoordinateSystem.Static.Visible = false;
					MyCoordinateSystem.Static.ResetSelection();
				}
				MyCoordinateSystem.OnCoordinateChange -= OnCoordinateChange;
			}
		}

		public void Hide()
		{
			if (MyFakes.ENABLE_VR_BUILDING)
			{
				ShowPreview(show: false);
			}
			else
			{
				ChangeClipboardPreview(visible: false, m_previewGrids, m_copiedGrids);
			}
		}

		public void Show()
		{
			if (IsActive && !m_isBeingAdded)
			{
				if (m_previewGrids.Count == 0)
				{
					ChangeClipboardPreview(visible: true, m_previewGrids, m_copiedGrids);
				}
				if (MyFakes.ENABLE_VR_BUILDING)
				{
					ShowPreview(show: true);
				}
			}
		}

		protected void ShowPreview(bool show)
		{
<<<<<<< HEAD
=======
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (PreviewGrids.Count == 0 || PreviewGrids[0].Render.Visible == show)
			{
				return;
			}
			foreach (MyCubeGrid previewGrid in PreviewGrids)
			{
				previewGrid.Render.Visible = show;
<<<<<<< HEAD
				foreach (MySlimBlock block in previewGrid.GetBlocks())
				{
					MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						myCompoundCubeBlock.Render.UpdateRenderObject(show);
						foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
						{
							if (block2.FatBlock != null)
							{
								block2.FatBlock.Render.UpdateRenderObject(show);
							}
						}
					}
					else if (block.FatBlock != null)
					{
						block.FatBlock.Render.UpdateRenderObject(show);
=======
				Enumerator<MySlimBlock> enumerator2 = previewGrid.GetBlocks().GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MySlimBlock current2 = enumerator2.get_Current();
						MyCompoundCubeBlock myCompoundCubeBlock = current2.FatBlock as MyCompoundCubeBlock;
						if (myCompoundCubeBlock != null)
						{
							myCompoundCubeBlock.Render.UpdateRenderObject(show);
							foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
							{
								if (block.FatBlock != null)
								{
									block.FatBlock.Render.UpdateRenderObject(show);
								}
							}
						}
						else if (current2.FatBlock != null)
						{
							current2.FatBlock.Render.UpdateRenderObject(show);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
		}

		public void ClearClipboard()
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedGrids.Clear();
			m_copiedGridOffsets.Clear();
		}

		public void CopyGroup(MyCubeGrid gridInGroup, GridLinkTypeEnum groupType)
		{
			if (gridInGroup == null)
			{
				return;
			}
			m_copiedGrids.Clear();
			m_copiedGridOffsets.Clear();
			if (MyFakes.ENABLE_COPY_GROUP && MyFakes.ENABLE_LARGE_STATIC_GROUP_COPY_FIRST)
			{
				List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(gridInGroup);
				MyCubeGrid myCubeGrid = null;
				MyCubeGrid myCubeGrid2 = null;
				MyCubeGrid myCubeGrid3 = null;
				if (gridInGroup.GridSizeEnum == MyCubeSize.Large)
				{
					myCubeGrid2 = gridInGroup;
					if (gridInGroup.IsStatic)
					{
						myCubeGrid = gridInGroup;
					}
				}
				else if (gridInGroup.GridSizeEnum == MyCubeSize.Small && gridInGroup.IsStatic)
				{
					myCubeGrid3 = gridInGroup;
				}
				foreach (MyCubeGrid item in groupNodes)
				{
					if (myCubeGrid2 == null && item.GridSizeEnum == MyCubeSize.Large)
					{
						myCubeGrid2 = item;
					}
					if (myCubeGrid == null && item.GridSizeEnum == MyCubeSize.Large && item.IsStatic)
					{
						myCubeGrid = item;
					}
					if (myCubeGrid3 == null && item.GridSizeEnum == MyCubeSize.Small && item.IsStatic)
					{
						myCubeGrid3 = item;
					}
				}
				MyCubeGrid myCubeGrid4 = ((myCubeGrid != null) ? myCubeGrid : null);
				myCubeGrid4 = ((myCubeGrid4 != null) ? myCubeGrid4 : ((myCubeGrid2 != null) ? myCubeGrid2 : null));
				myCubeGrid4 = ((myCubeGrid4 != null) ? myCubeGrid4 : ((myCubeGrid3 != null) ? myCubeGrid3 : null));
				myCubeGrid4 = ((myCubeGrid4 != null) ? myCubeGrid4 : gridInGroup);
				List<MyCubeGrid> groupNodes2 = MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(myCubeGrid4);
				CopyGridInternal(myCubeGrid4);
				foreach (MyCubeGrid item2 in groupNodes2)
				{
					if (item2 != myCubeGrid4)
					{
						CopyGridInternal(item2);
					}
				}
				return;
			}
			CopyGridInternal(gridInGroup);
			if (!MyFakes.ENABLE_COPY_GROUP)
<<<<<<< HEAD
			{
				return;
			}
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(gridInGroup))
			{
=======
			{
				return;
			}
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(gridInGroup))
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (groupNode != gridInGroup)
				{
					CopyGridInternal(groupNode);
				}
			}
		}

		public void CutGrid(MyCubeGrid grid)
		{
			if (grid != null)
			{
				CopyGrid(grid);
				DeleteGrid(grid);
			}
		}

		public void DeleteGrid(MyCubeGrid grid)
		{
			grid?.SendGridCloseRequest();
		}

		public void CopyGrid(MyCubeGrid grid)
		{
			if (grid != null)
			{
				m_copiedGrids.Clear();
				m_copiedGridOffsets.Clear();
				CopyGridInternal(grid);
			}
		}

		public void CutGroup(MyCubeGrid grid, GridLinkTypeEnum groupType)
		{
			if (grid != null)
			{
				CopyGroup(grid, groupType);
				DeleteGroup(grid, groupType);
			}
		}

		public void DeleteGroup(MyCubeGrid grid, GridLinkTypeEnum groupType)
		{
<<<<<<< HEAD
			if (grid == null)
			{
				return;
			}
			if (MyFakes.ENABLE_COPY_GROUP)
			{
				foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(grid))
				{
					foreach (MySlimBlock block in groupNode.GetBlocks())
					{
						MyCockpit myCockpit = block.FatBlock as MyCockpit;
						if (myCockpit != null && myCockpit.Pilot != null)
						{
							myCockpit.RequestRemovePilot();
						}
					}
					groupNode.SendGridCloseRequest();
				}
			}
			else
			{
				grid.SendGridCloseRequest();
=======
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (grid == null)
			{
				return;
			}
			if (MyFakes.ENABLE_COPY_GROUP)
			{
				foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(grid))
				{
					Enumerator<MySlimBlock> enumerator2 = groupNode.GetBlocks().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyCockpit myCockpit = enumerator2.get_Current().FatBlock as MyCockpit;
							if (myCockpit != null && myCockpit.Pilot != null)
							{
								myCockpit.RequestRemovePilot();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					groupNode.SendGridCloseRequest();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				grid.SendGridCloseRequest();
			}
		}

		private void CopyGridInternal(MyCubeGrid toCopy)
		{
			if (MySession.Static.CameraController.Equals(toCopy))
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, toCopy.PositionComp.GetPosition());
			}
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)toCopy.GetObjectBuilder(copy: true);
			myObjectBuilder_CubeGrid.GridGeneralDamageModifier = 1f;
			myObjectBuilder_CubeGrid.Immune = false;
			m_copiedGrids.Add(myObjectBuilder_CubeGrid);
			RemovePilots(myObjectBuilder_CubeGrid);
			if (m_copiedGrids.Count == 1)
			{
				MatrixD pasteMatrix = GetPasteMatrix();
				Vector3I? vector3I = toCopy.RayCastBlocks(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * 1000.0);
				Vector3D vector3D = (vector3I.HasValue ? toCopy.GridIntegerToWorld(vector3I.Value) : toCopy.WorldMatrix.Translation);
				m_dragPointToPositionLocal = Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - vector3D, toCopy.PositionComp.WorldMatrixNormalizedInv);
				m_dragDistance = (float)(vector3D - pasteMatrix.Translation).Length();
				m_pasteDirUp = toCopy.WorldMatrix.Up;
				m_pasteDirForward = toCopy.WorldMatrix.Forward;
				m_pasteOrientationAngle = 0f;
			}
			m_copiedGridOffsets.Add(toCopy.WorldMatrix.Translation - m_copiedGrids[0].PositionAndOrientation.Value.Position);
		}

		public virtual bool PasteGrid(bool deactivate = true, bool showWarning = true)
		{
			try
			{
				UpdateTouchingGrids(Sync.MyId);
				return PasteGridInternal(deactivate, null, m_touchingGrids, null, multiBlock: false, showWarning);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error during grid init:" + ex);
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				MyHud.Notifications.Add(MyNotificationSingletons.PasteFailed);
				return false;
			}
		}

		protected bool PasteGridInternal(bool deactivate, List<MyObjectBuilder_CubeGrid> pastedBuilders = null, List<MyCubeGrid> touchingGrids = null, UpdateAfterPasteCallback updateAfterPasteCallback = null, bool multiBlock = false, bool showWarning = true)
		{
			if (m_copiedGrids.Count == 0 && showWarning)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Blueprints_EmptyClipboardMessageHeader), messageText: MyTexts.Get(MyCommonTexts.Blueprints_EmptyClipboardMessage)));
				return false;
			}
			if (m_copiedGrids.Count > 0 && !IsActive)
			{
				Activate();
				return true;
			}
			if (!CanBePlaced)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			if (!CheckLimitsAndNotify())
			{
				return false;
			}
			if (m_previewGrids.Count == 0)
			{
				return false;
			}
			foreach (MyCubeGrid previewGrid in m_previewGrids)
			{
				if (!MySessionComponentSafeZones.IsActionAllowed(previewGrid.PositionComp.WorldAABB, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, 0uL))
				{
					return false;
				}
			}
			if (!MySession.Static.IsRunningExperimental)
			{
				bool flag = false;
				foreach (MyCubeGrid previewGrid2 in m_previewGrids)
				{
					if (previewGrid2.UnsafeBlocks.Count > 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Blueprints_UnsafeClipboardMessageHeader), messageText: MyTexts.Get(MyCommonTexts.Blueprints_UnsafeClipboardMessage)));
					return false;
				}
			}
			if (!MySession.Static.IsUserAdmin(Sync.MyId) || !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.KeepOriginalOwnershipOnPaste))
			{
				foreach (MyObjectBuilder_CubeGrid copiedGrid in m_copiedGrids)
				{
					foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
					{
						cubeBlock.BuiltBy = MySession.Static.LocalPlayerId;
						if (cubeBlock.Owner != 0L && Sync.Players.IdentityIsNpc(cubeBlock.Owner))
						{
							cubeBlock.Owner = MySession.Static.LocalPlayerId;
						}
					}
				}
			}
			bool missingBlockDefinitions = false;
			if (ShowModdedBlocksWarning)
			{
				missingBlockDefinitions = !CheckPastedBlocks();
			}
			if (missingBlockDefinitions)
			{
				AllowSwitchCameraMode = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToPasteGridWithMissingBlocks), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						PasteInternal(missingBlockDefinitions, deactivate, pastedBuilders, null, updateAfterPasteCallback, multiBlock);
					}
					AllowSwitchCameraMode = true;
				}));
				return false;
			}
			MyDLCs.MyDLC missingDLC = CheckPastedDLCBlocks();
			if (missingDLC != null)
			{
				AllowSwitchCameraMode = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO_CANCEL, MyTexts.Get(MyCommonTexts.MessageBoxTextMissingDLCWhenPasting), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, MyCommonTexts.VisitStore, MyCommonTexts.PasteAnyway, delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					switch (result)
					{
					case MyGuiScreenMessageBox.ResultEnum.YES:
						MyGameService.OpenDlcInShop(missingDLC.AppId);
						break;
					case MyGuiScreenMessageBox.ResultEnum.NO:
						PasteInternal(missingBlockDefinitions, deactivate, pastedBuilders, null, updateAfterPasteCallback, multiBlock);
						break;
					}
					AllowSwitchCameraMode = true;
				}));
				return false;
			}
			if (!MySession.Static.IsUserScripter(Sync.MyId) && !CheckPastedScripts())
			{
				MyHud.Notifications.Add(MyNotificationSingletons.BlueprintScriptsRemoved);
			}
			return PasteInternal(missingBlockDefinitions, deactivate, pastedBuilders, touchingGrids, updateAfterPasteCallback, multiBlock, keepRelativeOffset: true);
		}

		private bool PasteInternal(bool missingDefinitions, bool deactivate, List<MyObjectBuilder_CubeGrid> pastedBuilders = null, List<MyCubeGrid> touchingGrids = null, UpdateAfterPasteCallback updateAfterPasteCallback = null, bool multiBlock = false, bool keepRelativeOffset = false)
		{
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			foreach (MyObjectBuilder_CubeGrid copiedGrid in m_copiedGrids)
			{
				list.Add((MyObjectBuilder_CubeGrid)copiedGrid.Clone());
			}
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = list[0];
			bool num = IsSnapped && SnapMode == SnapMode.Base6Directions && m_hitEntity is MyCubeGrid && myObjectBuilder_CubeGrid != null && ((MyCubeGrid)m_hitEntity).GridSizeEnum == myObjectBuilder_CubeGrid.GridSizeEnum;
			MyCubeGrid myCubeGrid = null;
			if (num)
			{
				myCubeGrid = m_hitEntity as MyCubeGrid;
			}
			bool flag = num || (touchingGrids != null && touchingGrids.Count > 0);
			if (myCubeGrid == null && touchingGrids != null && touchingGrids.Count > 0)
			{
				myCubeGrid = touchingGrids[0];
			}
			if (num)
			{
				int mergedBlocksCount = myCubeGrid?.BlocksCount ?? list[0].CubeBlocks.Count;
				if (!CheckLimitsAndNotify(mergedBlocksCount))
				{
					return false;
				}
			}
			MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
			int num2 = 0;
			foreach (MyObjectBuilder_CubeGrid item in list)
			{
				item.CreatePhysics = true;
				item.EnableSmallToLargeConnections = true;
				item.PositionAndOrientation = new MyPositionAndOrientation(m_previewGrids[num2].WorldMatrix);
				item.PositionAndOrientation.Value.Orientation.Normalize();
				num2++;
			}
			long num3 = 0L;
			bool flag2 = MySession.Static.CreativeToolsEnabled(Sync.MyId);
			if (flag && myCubeGrid != null)
			{
<<<<<<< HEAD
				myCubeGrid.PasteBlocksToGrid(list, num3, multiBlock, flag2, MySession.Static.GetComponent<MySessionComponentDLC>().GetAvailableClientDLCsIds());
=======
				myCubeGrid.PasteBlocksToGrid(list, num3, multiBlock, flag2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (CreationMode)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyCubeGrid.TryCreateGrid_Implementation, CubeSize, IsStatic, list[0].PositionAndOrientation.Value, num3, flag2);
				CreationMode = false;
			}
			else if (MySession.Static.CreativeMode || MySession.Static.HasCreativeRights)
			{
				bool flag3 = false;
				bool flag4 = false;
				foreach (MyCubeGrid previewGrid in m_previewGrids)
				{
					flag4 |= previewGrid.GridSizeEnum == MyCubeSize.Small;
					MyGridPlacementSettings gridPlacementSettings = m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, previewGrid.IsStatic);
					flag3 |= MyCubeGrid.IsAabbInsideVoxel(previewGrid.PositionComp.WorldMatrixRef, previewGrid.PositionComp.LocalAABB, gridPlacementSettings);
				}
				foreach (MyObjectBuilder_CubeGrid item2 in list)
				{
<<<<<<< HEAD
					item2.IsStatic = flag3 || (MySession.Static.EnableConvertToStation && item2.IsStatic);
=======
					item2.IsStatic = flag5 || flag3 || (MySession.Static.EnableConvertToStation && item2.IsStatic);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyHud.PushRotatingWheelVisible();
				}
				MyCubeGrid.RelativeOffset offset = default(MyCubeGrid.RelativeOffset);
				if (keepRelativeOffset && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					offset.Use = true;
					if (m_spawnerId.HasValue)
					{
						offset.RelativeToEntity = true;
						offset.SpawnerId = m_spawnerId.Value;
					}
					else
					{
						offset.RelativeToEntity = false;
						offset.SpawnerId = 0L;
					}
					offset.OriginalSpawnPoint = m_originalSpawnerPosition;
				}
				else
				{
					offset.Use = false;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyCubeGrid.TryPasteGrid_Implementation, new MyCubeGrid.MyPasteGridParameters(list, missingDefinitions, multiBlock, m_objectVelocity, flag2, offset, MySession.Static.GetComponent<MySessionComponentDLC>().GetAvailableClientDLCsIds()));
			}
			if (deactivate)
			{
				Deactivate(afterPaste: true);
			}
			updateAfterPasteCallback?.Invoke(pastedBuilders);
			return true;
		}

		private bool CheckLimitsAndNotify(int mergedBlocksCount = 0)
		{
			int pCUs = m_copiedGridsInfo.PCUs;
			int totalBlocks = m_copiedGridsInfo.TotalBlocks;
			bool flag = !m_copiedGridsInfo.HasGridOverLimits;
			if (mergedBlocksCount > 0 && !MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, null, 0, 0, mergedBlocksCount))
			{
				return false;
			}
			return MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, null, pCUs, totalBlocks, (!flag) ? (MySession.Static.MaxGridSize + 1) : 0, m_blocksPerType);
		}

		/// <summary>
		/// Checks the pasted object builder for non-existent blocks (e.g. copying from world with a cube block mod to a world without it) or blocks/blueprints that ware modified externally
		/// </summary>
		/// <returns>True when the grid can be pasted</returns>
		protected bool CheckPastedBlocks()
		{
			return CheckPastedBlocks(m_copiedGrids);
		}

		public static bool CheckPastedBlocks(IEnumerable<MyObjectBuilder_CubeGrid> pastedGrids)
		{
			foreach (MyObjectBuilder_CubeGrid pastedGrid in pastedGrids)
			{
				bool flag = !MySession.Static.Settings.EnableSupergridding;
				foreach (MyObjectBuilder_CubeBlock cubeBlock in pastedGrid.CubeBlocks)
				{
					MyDefinitionId defId = new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId);
					if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out var blockDefinition))
					{
						return false;
					}
					if (flag && blockDefinition.CubeSize != pastedGrid.GridSizeEnum)
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Checks the pasted object builder for scripts inside programmable blocks
		/// </summary>
		/// <returns></returns>
		protected bool CheckPastedScripts()
		{
			if (MySession.Static.IsUserScripter(Sync.MyId))
			{
				return true;
			}
			foreach (MyObjectBuilder_CubeGrid copiedGrid in m_copiedGrids)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
				{
					MyObjectBuilder_MyProgrammableBlock myObjectBuilder_MyProgrammableBlock = cubeBlock as MyObjectBuilder_MyProgrammableBlock;
					if (myObjectBuilder_MyProgrammableBlock != null && myObjectBuilder_MyProgrammableBlock.Program != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		protected MyDLCs.MyDLC CheckPastedDLCBlocks()
		{
			MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
			foreach (MyObjectBuilder_CubeGrid copiedGrid in m_copiedGrids)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
				{
					MyDefinitionId id = new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId);
					MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(id);
					MyDLCs.MyDLC firstMissingDefinitionDLC = component.GetFirstMissingDefinitionDLC(definition, Sync.MyId);
					if (firstMissingDefinitionDLC != null)
					{
						return firstMissingDefinitionDLC;
					}
				}
			}
			return null;
		}

		public void SetGridFromBuilder(MyObjectBuilder_CubeGrid grid, Vector3 dragPointDelta, float dragVectorLength)
		{
			m_copiedGrids.Clear();
			m_copiedGridOffsets.Clear();
			m_dragPointToPositionLocal = dragPointDelta;
			m_dragDistance = dragVectorLength;
			MyPositionAndOrientation myPositionAndOrientation = grid.PositionAndOrientation ?? MyPositionAndOrientation.Default;
			m_pasteDirUp = myPositionAndOrientation.Up;
			m_pasteDirForward = myPositionAndOrientation.Forward;
			SetGridFromBuilderInternal(grid, Vector3.Zero);
		}

		public void SetGridFromBuilders(MyObjectBuilder_CubeGrid[] grids, Vector3 dragPointDelta, float dragVectorLength, bool deactivate = true)
		{
			ShowModdedBlocksWarning = true;
			if (IsActive && deactivate)
			{
				Deactivate();
			}
			m_copiedGrids.Clear();
			m_copiedGridOffsets.Clear();
			if (grids.Length != 0)
			{
				m_dragPointToPositionLocal = dragPointDelta;
				m_dragDistance = dragVectorLength;
				MyPositionAndOrientation myPositionAndOrientation = grids[0].PositionAndOrientation ?? MyPositionAndOrientation.Default;
				m_pasteDirUp = myPositionAndOrientation.Up;
				m_pasteDirForward = myPositionAndOrientation.Forward;
				SetGridFromBuilderInternal(grids[0], Vector3.Zero);
				MatrixD matrix = (grids[0].PositionAndOrientation.HasValue ? grids[0].PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity);
				matrix = MatrixD.Invert(matrix);
				for (int i = 1; i < grids.Length; i++)
				{
					Vector3D position = (grids[i].PositionAndOrientation.HasValue ? ((Vector3D)grids[i].PositionAndOrientation.Value.Position) : Vector3D.Zero);
					position = Vector3D.Transform(position, matrix);
					SetGridFromBuilderInternal(grids[i], position);
				}
			}
		}

		private void SetGridFromBuilderInternal(MyObjectBuilder_CubeGrid grid, Vector3 offset)
		{
			BeforeCreateGrid(grid);
			m_copiedGrids.Add(grid);
			m_copiedGridOffsets.Add(offset);
			RemovePilots(grid);
		}

		protected void BeforeCreateGrid(MyObjectBuilder_CubeGrid grid)
		{
			foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
			{
				MyDefinitionId id = cubeBlock.GetId();
				MyCubeBlockDefinition blockDefinition = null;
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition);
				if (blockDefinition != null)
				{
					MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, GetClipboardBuilder(), cubeBlock, MySession.Static.CreativeToolsEnabled(Sync.MyId));
				}
			}
		}

		protected virtual void ChangeClipboardPreview(bool visible, List<MyCubeGrid> previewGrids, List<MyObjectBuilder_CubeGrid> copiedGrids)
		{
			foreach (MyCubeGrid previewGrid in previewGrids)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewGrid, enable: false);
				}
				previewGrid.SetFadeOut(state: false);
				previewGrid.Close();
			}
			m_visible = false;
			previewGrids.Clear();
			m_buildComponents.Clear();
			if (copiedGrids.Count == 0 || !visible)
			{
				return;
			}
			CalculateItemRequirements(copiedGrids, m_buildComponents);
			MyEntities.RemapObjectBuilderCollection(copiedGrids);
			MyCubeGrid.CleanCubeGridsBeforePaste(copiedGrids);
			m_copiedGridsInfo = default(CopiedGridInfo);
			int maxGridSize = MySession.Static.MaxGridSize;
			foreach (MyObjectBuilder_CubeGrid copiedGrid in m_copiedGrids)
			{
				m_copiedGridsInfo.PCUs += copiedGrid.CalculatePCUs();
				m_copiedGridsInfo.TotalBlocks += copiedGrid.CubeBlocks.Count;
				if (maxGridSize != 0 && copiedGrid.CubeBlocks.Count > maxGridSize)
				{
					m_copiedGridsInfo.HasGridOverLimits = true;
				}
			}
			Vector3D vector3D = Vector3D.Zero;
			m_blocksPerType.Clear();
			MyEntityIdentifier.InEntityCreationBlock = true;
			MyEntityIdentifier.LazyInitPerThreadStorage(2048);
			bool flag = m_copiedGridsInfo.PCUs > MaxVisiblePCU;
			bool flag2 = true;
			foreach (MyObjectBuilder_CubeGrid copiedGrid2 in copiedGrids)
			{
				List<MyObjectBuilder_CubeBlock> cubeBlocks = copiedGrid2.CubeBlocks;
				bool isStatic = copiedGrid2.IsStatic;
				if (m_useDynamicPreviews)
				{
					copiedGrid2.IsStatic = false;
				}
				copiedGrid2.CreatePhysics = false;
				copiedGrid2.EnableSmallToLargeConnections = false;
				foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid2.CubeBlocks)
				{
					cubeBlock.BuiltBy = 0L;
					MyDefinitionId defId = new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId);
					if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out var blockDefinition))
					{
						string blockPairName = blockDefinition.BlockPairName;
						if (m_blocksPerType.ContainsKey(blockPairName))
						{
							m_blocksPerType[blockPairName]++;
						}
						else
						{
							m_blocksPerType.Add(blockPairName, 1);
						}
					}
				}
				if (copiedGrid2.PositionAndOrientation.HasValue)
				{
					MyPositionAndOrientation value = copiedGrid2.PositionAndOrientation.Value;
					if (flag2)
					{
						flag2 = false;
						vector3D = value.Position;
					}
					ref SerializableVector3D position = ref value.Position;
					position -= vector3D;
					copiedGrid2.PositionAndOrientation = value;
				}
				if (flag)
				{
					copiedGrid2.CubeBlocks = new List<MyObjectBuilder_CubeBlock>();
<<<<<<< HEAD
				}
				MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(copiedGrid2, fadeIn: false) as MyCubeGrid;
				copiedGrid2.IsStatic = isStatic;
				copiedGrid2.CubeBlocks = cubeBlocks;
				if (myCubeGrid == null)
				{
					ChangeClipboardPreview(visible: false, previewGrids, copiedGrids);
					return;
				}
				MakeTransparent(myCubeGrid);
				if (myCubeGrid.CubeBlocks.Count == 0 && !flag)
=======
				}
				MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(copiedGrid2, fadeIn: false) as MyCubeGrid;
				copiedGrid2.IsStatic = isStatic;
				copiedGrid2.CubeBlocks = cubeBlocks;
				if (myCubeGrid == null)
				{
					ChangeClipboardPreview(visible: false, previewGrids, copiedGrids);
					return;
				}
				MakeTransparent(myCubeGrid);
				if (myCubeGrid.CubeBlocks.get_Count() == 0 && !flag)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					copiedGrids.Remove(copiedGrid2);
					ChangeClipboardPreview(visible: false, previewGrids, copiedGrids);
					return;
				}
				myCubeGrid.IsPreview = true;
				myCubeGrid.Save = false;
				previewGrids.Add(myCubeGrid);
				myCubeGrid.OnClose += previewGrid_OnClose;
				myCubeGrid.PositionComp.LocalAABB = copiedGrid2.CalculateBoundingBox();
<<<<<<< HEAD
			}
			if (flag)
			{
				MyHudNotification notification = new MyHudNotification(MySpaceTexts.ClipboardBlueprintIsTooBig, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 1, MyNotificationLevel.Control);
				MyHud.Notifications.Add(notification);
			}
=======
			}
			if (flag)
			{
				MyHudNotification notification = new MyHudNotification(MySpaceTexts.ClipboardBlueprintIsTooBig, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 1, MyNotificationLevel.Control);
				MyHud.Notifications.Add(notification);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_resultIDs.Clear();
			MyEntityIdentifier.GetPerThreadEntities(m_resultIDs);
			MyEntityIdentifier.ClearPerThreadEntities();
			MyEntityIdentifier.InEntityCreationBlock = false;
			m_visible = visible;
		}

		private void previewGrid_OnClose(MyEntity obj)
		{
			m_previewGrids.Remove(obj as MyCubeGrid);
			_ = m_previewGrids.Count;
		}

		public static void CalculateItemRequirements(List<MyObjectBuilder_CubeGrid> blocksToBuild, MyComponentList buildComponents)
		{
			buildComponents.Clear();
			foreach (MyObjectBuilder_CubeGrid item in blocksToBuild)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in item.CubeBlocks)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
					if (myObjectBuilder_CompoundCubeBlock != null)
					{
						MyObjectBuilder_CubeBlock[] blocks = myObjectBuilder_CompoundCubeBlock.Blocks;
						for (int i = 0; i < blocks.Length; i++)
						{
							AddSingleBlockRequirements(blocks[i], buildComponents);
						}
					}
					else
					{
						AddSingleBlockRequirements(cubeBlock, buildComponents);
					}
				}
			}
		}

		public static void CalculateItemRequirements(MyObjectBuilder_CubeGrid[] blocksToBuild, MyComponentList buildComponents)
		{
			buildComponents.Clear();
			for (int i = 0; i < blocksToBuild.Length; i++)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in blocksToBuild[i].CubeBlocks)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
					if (myObjectBuilder_CompoundCubeBlock != null)
					{
						MyObjectBuilder_CubeBlock[] blocks = myObjectBuilder_CompoundCubeBlock.Blocks;
						for (int j = 0; j < blocks.Length; j++)
						{
							AddSingleBlockRequirements(blocks[j], buildComponents);
						}
					}
					else
					{
						AddSingleBlockRequirements(cubeBlock, buildComponents);
					}
				}
			}
		}

		private static void AddSingleBlockRequirements(MyObjectBuilder_CubeBlock block, MyComponentList buildComponents)
		{
			MyComponentStack.GetMountedComponents(buildComponents, block);
			if (block.ConstructionStockpile == null)
			{
				return;
			}
			MyObjectBuilder_StockpileItem[] items = block.ConstructionStockpile.Items;
			foreach (MyObjectBuilder_StockpileItem myObjectBuilder_StockpileItem in items)
			{
				if (myObjectBuilder_StockpileItem.PhysicalContent != null)
				{
					buildComponents.AddMaterial(myObjectBuilder_StockpileItem.PhysicalContent.GetId(), myObjectBuilder_StockpileItem.Amount, 0, addToDisplayList: false);
				}
			}
		}

		private void MakeTransparent(MyCubeGrid grid)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			grid.Render.Transparency = Transparency;
			grid.Render.CastShadows = false;
			using (MyUtils.ReuseCollection(ref m_cacheEntitySet))
			{
<<<<<<< HEAD
				grid.Hierarchy.GetChildrenRecursive(m_cacheEntitySet);
				foreach (IMyEntity item in m_cacheEntitySet)
				{
					MyCubeBlock myCubeBlock = item as MyCubeBlock;
=======
				m_cacheEntitySet = new HashSet<IMyEntity>();
			}
			grid.Hierarchy.GetChildrenRecursive(m_cacheEntitySet);
			Enumerator<IMyEntity> enumerator = m_cacheEntitySet.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyEntity current = enumerator.get_Current();
					MyCubeBlock myCubeBlock = current as MyCubeBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (myCubeBlock != null)
					{
						myCubeBlock.SlimBlock.Dithering = Transparency;
					}
<<<<<<< HEAD
					item.Render.Transparency = Transparency;
					item.Render.CastShadows = false;
				}
=======
					current.Render.Transparency = Transparency;
					current.Render.CastShadows = false;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void DisablePhysicsRecursively(MyEntity entity)
		{
			if (entity.Physics?.Enabled ?? false)
			{
				entity.Physics.Enabled = false;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = entity as MyCubeBlock) != null)
			{
				if (myCubeBlock.UseObjectsComponent.DetectorPhysics?.Enabled ?? false)
				{
					myCubeBlock.UseObjectsComponent.DetectorPhysics.Enabled = false;
				}
				myCubeBlock.DisableUpdates();
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				DisablePhysicsRecursively(child.Container.Entity as MyEntity);
			}
		}

		public virtual void Update()
		{
			if (IsActive && m_visible)
			{
				if (m_enableUpdateHitEntity)
				{
					UpdateHitEntity();
				}
				if (m_previewGrids.Count > 0)
				{
					UpdatePastePosition();
					UpdateGridTransformations();
				}
				if (IsSnapped && SnapMode == SnapMode.Base6Directions)
				{
					FixSnapTransformationBase6();
				}
				if (m_calculateVelocity)
				{
					m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
				}
				if (MyFpsManager.GetSessionTotalFrames() % 11 == 0)
				{
					m_canBePlaced = TestPlacement();
				}
				else
				{
					m_canBePlacedNeedsRefresh = true;
				}
				m_characterHasEnoughMaterials = true;
				UpdatePreviewBBox();
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
				}
			}
		}

		protected bool UpdateHitEntity(bool canPasteLargeOnSmall = true)
		{
			m_closestHitDistSq = float.MaxValue;
			m_hitPos = new Vector3(0f, 0f, 0f);
			m_hitNormal = new Vector3(1f, 0f, 0f);
			m_hitEntity = null;
			MatrixD pasteMatrix = GetPasteMatrix();
			if (MyFakes.ENABLE_VR_BUILDING && MyBlockBuilderBase.PlacementProvider != null)
			{
				if (!MyBlockBuilderBase.PlacementProvider.HitInfo.HasValue)
				{
					return false;
				}
				m_hitEntity = (IMyEntity)(((object)MyBlockBuilderBase.PlacementProvider.ClosestGrid) ?? ((object)MyBlockBuilderBase.PlacementProvider.ClosestVoxelMap));
				m_hitPos = MyBlockBuilderBase.PlacementProvider.HitInfo.Value.Position;
				m_hitNormal = MyBlockBuilderBase.PlacementProvider.HitInfo.Value.HkHitInfo.Normal;
				m_hitNormal = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(Vector3.TransformNormal(m_hitNormal, m_hitEntity.PositionComp.WorldMatrixNormalizedInv)));
				m_hitNormal = Vector3.TransformNormal(m_hitNormal, m_hitEntity.PositionComp.WorldMatrixRef);
				m_closestHitDistSq = (float)(m_hitPos - pasteMatrix.Translation).LengthSquared();
				return true;
			}
			MyPhysics.CastRay(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * m_dragDistance, m_raycastCollisionResults, 15);
			foreach (MyPhysics.HitInfo raycastCollisionResult in m_raycastCollisionResults)
			{
				if (raycastCollisionResult.HkHitInfo.Body == null)
<<<<<<< HEAD
				{
					continue;
				}
				IMyEntity hitEntity = raycastCollisionResult.HkHitInfo.GetHitEntity();
				if (hitEntity == null)
				{
					continue;
				}
=======
				{
					continue;
				}
				IMyEntity hitEntity = raycastCollisionResult.HkHitInfo.GetHitEntity();
				if (hitEntity == null)
				{
					continue;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
				if ((canPasteLargeOnSmall || m_previewGrids.Count == 0 || m_previewGrids[0].GridSizeEnum != 0 || myCubeGrid == null || myCubeGrid.GridSizeEnum != MyCubeSize.Small) && (hitEntity is MyVoxelBase || (myCubeGrid != null && m_previewGrids.Count != 0 && myCubeGrid.EntityId != m_previewGrids[0].EntityId)))
				{
					float num = (float)(raycastCollisionResult.Position - pasteMatrix.Translation).LengthSquared();
					if (num < m_closestHitDistSq)
					{
						m_closestHitDistSq = num;
						m_hitPos = raycastCollisionResult.Position;
						m_hitNormal = raycastCollisionResult.HkHitInfo.Normal;
						m_hitEntity = hitEntity;
					}
				}
			}
			m_raycastCollisionResults.Clear();
			return true;
		}

		protected virtual void TestBuildingMaterials()
		{
			m_characterHasEnoughMaterials = EntityCanPaste(GetClipboardBuilder());
		}

		protected virtual MyEntity GetClipboardBuilder()
		{
			return MySession.Static.LocalCharacter;
		}

		public virtual bool EntityCanPaste(MyEntity pastingEntity)
		{
			if (m_copiedGrids.Count < 1)
			{
				return false;
			}
			if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				return true;
			}
			MyCubeBuilder.BuildComponent.GetGridSpawnMaterials(m_copiedGrids[0]);
			return MyCubeBuilder.BuildComponent.HasBuildingMaterials(pastingEntity);
		}

		protected void UpdateTouchingGrids(ulong pastingPlayer = 0uL)
		{
			for (int i = 0; i < m_previewGrids.Count; i++)
			{
				MyCubeGrid myCubeGrid = m_previewGrids[i];
				MyGridPlacementSettings gridPlacementSettings = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic);
				GetTouchingGrids(myCubeGrid, gridPlacementSettings, pastingPlayer);
			}
		}

		protected virtual bool TestPlacement()
		{
			m_canBePlacedNeedsRefresh = false;
			if (MyFakes.DISABLE_CLIPBOARD_PLACEMENT_TEST)
			{
				return true;
			}
			if (!MyEntities.IsInsideWorld(m_pastePosition))
			{
				return false;
			}
			bool flag = true;
			for (int i = 0; i < m_previewGrids.Count; i++)
			{
				MyCubeGrid myCubeGrid = m_previewGrids[i];
				if (!MySessionComponentSafeZones.IsActionAllowed(myCubeGrid.PositionComp.WorldAABB, MySafeZoneAction.Building, 0L, Sync.MyId))
				{
					flag = false;
				}
				if (flag)
				{
					if (i == 0 && m_hitEntity is MyCubeGrid && IsSnapped && SnapMode == SnapMode.Base6Directions)
					{
						MyCubeGrid myCubeGrid2 = m_hitEntity as MyCubeGrid;
						bool flag2 = myCubeGrid2.GridSizeEnum == MyCubeSize.Large && myCubeGrid.GridSizeEnum == MyCubeSize.Small;
						MyGridPlacementSettings settings = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic);
						if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && myCubeGrid.IsStatic && flag2)
						{
							flag &= MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false, myCubeGrid2);
						}
						else
						{
							Vector3I gridOffset = myCubeGrid2.WorldToGridInteger(m_pastePosition);
							if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
							{
								MyRenderProxy.DebugDrawText2D(new Vector2(0f, 60f), "First grid offset: " + gridOffset.ToString(), Color.Red, 1f);
							}
							flag &= myCubeGrid2.GridSizeEnum == myCubeGrid.GridSizeEnum && myCubeGrid2.CanMergeCubes(myCubeGrid, gridOffset);
							flag &= MyCubeGrid.CheckMergeConnectivity(myCubeGrid2, myCubeGrid, gridOffset);
							flag &= MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false, myCubeGrid2);
						}
					}
					else
					{
						MyGridPlacementSettings settings2 = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic);
						flag &= MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings2, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false);
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return flag;
		}

		/// <summary>
		/// Gets grids that are touching given grid. It's used for diciding if grids should be merged on later stage.
		/// </summary>
		/// <param name="grid">Grid to be tested.</param>
		/// <param name="settings">Settings used for the test.</param>
		/// <param name="pastingPlayer"></param>
		private void GetTouchingGrids(MyCubeGrid grid, MyGridPlacementSettings settings, ulong pastingPlayer = 0uL)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_touchingGrids.Clear();
			Enumerator<MySlimBlock> enumerator = grid.CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock is MyCompoundCubeBlock)
					{
						bool flag = false;
						foreach (MySlimBlock block in (current.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							MyCubeGrid touchingGrid = null;
							MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, 0uL, block.CubeGrid);
							if (touchingGrid != null)
							{
								m_touchingGrids.Add(touchingGrid);
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					else
					{
						MyCubeGrid touchingGrid2 = null;
						MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(current.CubeGrid, ref settings, current.Min, current.Max, current.Orientation, current.BlockDefinition, out touchingGrid2, pastingPlayer, current.CubeGrid);
						if (touchingGrid2 != null)
						{
							m_touchingGrids.Add(touchingGrid2);
							break;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		protected virtual void UpdateGridTransformations()
		{
			if (m_copiedGrids.Count == 0)
			{
				return;
			}
			Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
			MatrixD m = m_copiedGrids[0].PositionAndOrientation.Value.GetMatrix();
			Matrix matrix = Matrix.Invert(m).GetOrientation() * firstGridOrientationMatrix;
			for (int i = 0; i < m_previewGrids.Count && i <= m_copiedGrids.Count - 1; i++)
			{
				if (m_copiedGrids[i].PositionAndOrientation.HasValue)
				{
					MatrixD matrix2 = m_copiedGrids[i].PositionAndOrientation.Value.GetMatrix();
					Vector3D normal = matrix2.Translation - m_copiedGrids[0].PositionAndOrientation.Value.Position;
					m_copiedGridOffsets[i] = Vector3.TransformNormal(normal, matrix);
					matrix2 *= matrix;
					Vector3D translation = m_pastePosition + m_copiedGridOffsets[i];
					matrix2.Translation = Vector3.Zero;
					matrix2 = MatrixD.Orthogonalize(matrix2);
					matrix2.Translation = translation;
					m_previewGrids[i].PositionComp.SetWorldMatrix(ref matrix2, null, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
				}
			}
		}

		protected virtual void UpdatePastePosition()
		{
			if (m_previewGrids.Count != 0)
			{
				m_pastePositionPrevious = m_pastePosition;
				MatrixD pasteMatrix = GetPasteMatrix();
				m_spawnerId = GetPasteSpawnerId();
				m_originalSpawnerPosition = GetPasteSpawnerPosition();
				Vector3 vector = pasteMatrix.Forward * m_dragDistance;
				if (!TrySnapToSurface(m_settings.GetGridPlacementSettings(m_previewGrids[0].GridSizeEnum).SnapMode))
				{
					m_pastePosition = pasteMatrix.Translation + vector;
					Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
					m_pastePosition += Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix);
				}
				double gridSize = m_previewGrids[0].GridSize;
				MyCoordinateSystem.CoordSystemData coordSystemData = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref m_pastePosition, gridSize, m_settings.StaticGridAlignToCenter);
				if (MyCoordinateSystem.Static.LocalCoordExist && !EnableStationRotation)
				{
					m_pastePosition = coordSystemData.SnappedTransform.Position;
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawSphere(pasteMatrix.Translation + vector, 0.15f, Color.Pink.ToVector3(), 1f, depthRead: false);
					MyRenderProxy.DebugDrawSphere(m_pastePosition, 0.15f, Color.Pink.ToVector3(), 1f, depthRead: false);
				}
			}
		}

		protected static MatrixD GetPasteMatrix()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
			}
			return MySector.MainCamera.WorldMatrix;
		}

		protected static long? GetPasteSpawnerId()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.Entity.EntityId;
			}
			return null;
		}

		public static Vector3D GetPasteSpawnerPosition()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.Entity.WorldMatrix.Translation;
			}
			return MySector.MainCamera.WorldMatrix.Translation;
		}

		public virtual Matrix GetFirstGridOrientationMatrix()
		{
			return Matrix.CreateWorld(Vector3.Zero, m_pasteDirForward, m_pasteDirUp);
		}

		public void AlignClipboardToGravity()
		{
			if (PreviewGrids.Count > 0)
			{
				Vector3 gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(PreviewGrids[0].WorldMatrix.Translation);
				AlignClipboardToGravity(gravity);
			}
		}

		public void AlignClipboardToGravity(Vector3 gravity)
		{
			if (PreviewGrids.Count > 0 && gravity.LengthSquared() > 0.0001f)
			{
				gravity.Normalize();
				Vector3 vector = (m_pasteDirForward = Vector3D.Reject(m_pasteDirForward, gravity));
				m_pasteDirUp = -gravity;
			}
		}

		protected void AlignRotationToCoordSys()
		{
			if (m_previewGrids.Count > 0)
			{
				double gridSize = m_previewGrids[0].GridSize;
				MyCoordinateSystem.CoordSystemData coordSystemData = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref m_pastePosition, gridSize, m_settings.StaticGridAlignToCenter);
				m_pastePosition = coordSystemData.SnappedTransform.Position;
				m_pasteDirForward = coordSystemData.SnappedTransform.Rotation.Forward;
				m_pasteDirUp = coordSystemData.SnappedTransform.Rotation.Up;
				m_pasteOrientationAngle = 0f;
			}
		}

		protected bool TrySnapToSurface(SnapMode snapMode)
		{
			if (m_closestHitDistSq < float.MaxValue)
			{
				Vector3D hitPos = m_hitPos;
				if ((double)m_hitNormal.Length() > 0.5)
				{
					MyCubeGrid myCubeGrid = m_hitEntity as MyCubeGrid;
					if (myCubeGrid != null)
					{
						MatrixD m = myCubeGrid.WorldMatrix.GetOrientation();
						Matrix axisDefinitionMatrix = m;
						Matrix toAlign = GetFirstGridOrientationMatrix();
						Matrix matrix = Matrix.AlignRotationToAxes(ref toAlign, ref axisDefinitionMatrix);
						_ = Matrix.Invert(toAlign) * matrix;
						m_pasteDirForward = matrix.Forward;
						m_pasteDirUp = matrix.Up;
						m_pasteOrientationAngle = 0f;
					}
				}
				Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
				Vector3 vector = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix);
				m_pastePosition = hitPos + vector;
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawSphere(hitPos, 0.08f, Color.Red.ToVector3(), 1f, depthRead: false);
					MyRenderProxy.DebugDrawSphere(m_pastePosition, 0.08f, Color.Red.ToVector3(), 1f, depthRead: false);
				}
				IsSnapped = true;
				return true;
			}
			IsSnapped = false;
			return false;
		}

		private void UpdatePreviewBBox()
		{
			if (m_previewGrids == null || m_isBeingAdded || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			if (!m_visible || !HasPreviewBBox)
			{
				foreach (MyCubeGrid previewGrid in m_previewGrids)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewGrid, enable: false);
				}
				return;
			}
			Vector4 value = new Vector4(Color.White.ToVector3(), 1f);
			MyStringId value2 = ID_GIZMO_DRAW_LINE_RED;
			bool flag = false;
			if (!MySession.Static.IsRunningExperimental)
			{
				foreach (MyCubeGrid previewGrid2 in m_previewGrids)
				{
					if (previewGrid2.UnsafeBlocks.Count > 0)
					{
						flag = true;
						break;
					}
				}
			}
			if (m_canBePlaced && !flag)
			{
				if (m_characterHasEnoughMaterials)
				{
					value2 = ID_GIZMO_DRAW_LINE;
				}
				else
				{
					value = Color.Gray.ToVector4();
				}
			}
			Vector3 value3 = new Vector3(0.1f);
			foreach (MyCubeGrid previewGrid3 in m_previewGrids)
			{
				MyEntities.EnableEntityBoundingBoxDraw(previewGrid3, enable: true, value, 0.04f, value3, value2, previewGrid3.BlocksCount == 0);
			}
			if (!Sync.IsDedicated)
			{
				StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.Clipboard_TotalPCU);
				StringBuilder stringBuilder2 = MyTexts.Get(MyCommonTexts.Clipboard_TotalBlocks);
				MyGuiManager.DrawString("White", stringBuilder.ToString() + m_copiedGridsInfo.PCUs + "\n" + stringBuilder2.ToString() + m_copiedGridsInfo.TotalBlocks, new Vector2(0.51f, 0.51f), 0.7f);
			}
		}

		protected void FixSnapTransformationBase6()
		{
			if (m_copiedGrids.Count == 0 || m_previewGrids.Count == 0)
			{
				return;
			}
			GetPasteMatrix();
			MyCubeGrid myCubeGrid = m_hitEntity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			if (m_copiedGridOffsets.Count < m_previewGrids.Count)
			{
				MyLog.Default.Critical($"SE-19306 : FixSnapTransformationBase6 m_previewGrids={0} m_copiedGrids={1} m_copiedGridOffsets={2}", m_previewGrids.Count, m_copiedGrids.Count, m_copiedGridOffsets.Count);
				return;
			}
			MatrixD m = myCubeGrid.WorldMatrix.GetOrientation();
			Matrix matrix = m;
			m = m_previewGrids[0].WorldMatrix.GetOrientation();
			Matrix matrix2 = m;
			matrix = Matrix.Normalize(matrix);
			matrix2 = Matrix.Normalize(matrix2);
			Matrix matrix3 = Matrix.AlignRotationToAxes(ref matrix2, ref matrix);
			Matrix matrix4 = Matrix.Invert(matrix2) * matrix3;
			int num = 0;
			foreach (MyCubeGrid previewGrid in m_previewGrids)
			{
				m = previewGrid.WorldMatrix.GetOrientation();
				Matrix matrix5 = m;
				matrix5 *= matrix4;
				Matrix.Invert(matrix5);
				MatrixD worldMatrix = MatrixD.CreateWorld(m_pastePosition, matrix5.Forward, matrix5.Up);
				previewGrid.PositionComp.SetWorldMatrix(ref worldMatrix);
			}
			if (myCubeGrid.GridSizeEnum == MyCubeSize.Large && m_previewGrids[0].GridSizeEnum == MyCubeSize.Small)
			{
				Vector3 vector = MyCubeBuilder.TransformLargeGridHitCoordToSmallGrid(m_pastePosition, myCubeGrid.PositionComp.WorldMatrixNormalizedInv, myCubeGrid.GridSize);
				m_pastePosition = myCubeGrid.GridIntegerToWorld(vector);
				if (MyFakes.ENABLE_VR_BUILDING)
				{
					Vector3 vector2 = Vector3I.Round(Vector3.TransformNormal(m_hitNormal, myCubeGrid.PositionComp.WorldMatrixNormalizedInv));
					Vector3 vector3 = vector2 * (m_previewGrids[0].GridSize / myCubeGrid.GridSize);
					Vector3 vector4 = Vector3I.Round(Vector3D.TransformNormal(Vector3D.TransformNormal(vector2, myCubeGrid.WorldMatrix), m_previewGrids[0].PositionComp.WorldMatrixNormalizedInv));
					BoundingBox localAABB = m_previewGrids[0].PositionComp.LocalAABB;
					localAABB.Min /= m_previewGrids[0].GridSize;
					localAABB.Max /= m_previewGrids[0].GridSize;
					Vector3 vector5 = m_dragPointToPositionLocal / m_previewGrids[0].GridSize;
					Vector3 zero = Vector3.Zero;
					Vector3 zero2 = Vector3.Zero;
					BoundingBox box = new BoundingBox(-Vector3.Half, Vector3.Half);
					box.Inflate(-0.05f);
					box.Translate(-vector5 + zero - vector4);
					while (localAABB.Contains(box) != 0)
					{
						zero -= vector4;
						zero2 -= vector3;
						box.Translate(-vector4);
					}
					m_pastePosition = myCubeGrid.GridIntegerToWorld(vector - zero2);
				}
			}
			else
			{
				Vector3I vector3I = Vector3I.Round(Vector3.TransformNormal(m_hitNormal, myCubeGrid.PositionComp.WorldMatrixNormalizedInv));
				Vector3I vector3I2 = myCubeGrid.WorldToGridInteger(m_pastePosition);
				int i;
				for (i = 0; i < 100; i++)
				{
					if (myCubeGrid.CanMergeCubes(m_previewGrids[0], vector3I2))
					{
						break;
					}
					vector3I2 += vector3I;
				}
				if (i == 0)
				{
					for (i = 0; i < 100; i++)
					{
						vector3I2 -= vector3I;
						if (!myCubeGrid.CanMergeCubes(m_previewGrids[0], vector3I2))
						{
							break;
						}
					}
					vector3I2 += vector3I;
				}
				if (i == 100)
				{
					vector3I2 = myCubeGrid.WorldToGridInteger(m_pastePosition);
				}
				m_pastePosition = myCubeGrid.GridIntegerToWorld(vector3I2);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawLine3D(m_hitPos, m_hitPos + m_hitNormal, Color.Red, Color.Green, depthRead: false);
			}
			num = 0;
			foreach (MyCubeGrid previewGrid2 in m_previewGrids)
			{
				MatrixD worldMatrix2 = previewGrid2.WorldMatrix;
				worldMatrix2.Translation = m_pastePosition + Vector3.Transform(m_copiedGridOffsets[num++], matrix4);
				previewGrid2.PositionComp.SetWorldMatrix(ref worldMatrix2);
			}
		}

		public void DrawHud()
		{
			if (m_previewGrids != null)
			{
				MyCubeBlockDefinition firstBlockDefinition = GetFirstBlockDefinition(m_copiedGrids[0]);
				MyHud.BlockInfo.LoadDefinition(firstBlockDefinition);
			}
		}

		public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
		{
			MyCubeGrid myCubeGrid = ((PreviewGrids.Count > 0) ? PreviewGrids[0] : null);
			if (myCubeGrid != null)
			{
				MatrixD worldMatrix = myCubeGrid.WorldMatrix;
				Vector3D vector3D = Vector3D.TransformNormal(-m_dragPointToPositionLocal, worldMatrix);
				worldMatrix.Translation += vector3D;
				hints.CalculateRotationHints(worldMatrix, !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && !MyInput.Static.IsJoystickLastUsed, isRotating, OneAxisRotationMode);
			}
		}

		private void RemovePilots(MyObjectBuilder_CubeGrid grid)
		{
			foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
			{
				MyObjectBuilder_Cockpit myObjectBuilder_Cockpit = cubeBlock as MyObjectBuilder_Cockpit;
				if (myObjectBuilder_Cockpit != null)
				{
					myObjectBuilder_Cockpit.ClearPilotAndAutopilot();
					if (myObjectBuilder_Cockpit.ComponentContainer != null && myObjectBuilder_Cockpit.ComponentContainer.Components != null)
					{
						foreach (MyObjectBuilder_ComponentContainer.ComponentData component in myObjectBuilder_Cockpit.ComponentContainer.Components)
						{
							if (component.TypeId == typeof(MyHierarchyComponentBase).Name)
							{
								((MyObjectBuilder_HierarchyComponentBase)component.Component).Children.RemoveAll((MyObjectBuilder_EntityBase x) => x is MyObjectBuilder_Character);
								break;
							}
						}
					}
					(myObjectBuilder_Cockpit as MyObjectBuilder_CryoChamber)?.Clear();
				}
				else
				{
					MyObjectBuilder_LandingGear myObjectBuilder_LandingGear = cubeBlock as MyObjectBuilder_LandingGear;
					if (myObjectBuilder_LandingGear != null)
					{
						myObjectBuilder_LandingGear.IsLocked = false;
						myObjectBuilder_LandingGear.MasterToSlave = null;
						myObjectBuilder_LandingGear.AttachedEntityId = null;
						myObjectBuilder_LandingGear.LockMode = LandingGearMode.Unlocked;
					}
				}
			}
		}

		public bool HasCopiedGrids()
		{
			return m_copiedGrids.Count > 0;
		}

		public void SaveClipboardAsPrefab(string name = null, string path = null)
		{
			if (m_copiedGrids.Count != 0)
			{
				name = name ?? (MyWorldGenerator.GetPrefabTypeName(m_copiedGrids[0]) + "_" + MyUtils.GetRandomInt(1000000, 9999999));
				if (path == null)
				{
					MyPrefabManager.SavePrefab(name, m_copiedGrids);
				}
				else
				{
					MyPrefabManager.SavePrefabToPath(name, path, m_copiedGrids);
				}
				MyHud.Notifications.Add(new MyHudNotificationDebug("Prefab saved: " + path, 10000));
			}
		}

		public void HideGridWhenColliding(List<Vector3D> collisionTestPoints)
		{
			if (m_previewGrids.Count == 0)
<<<<<<< HEAD
			{
				return;
			}
			bool flag = true;
			foreach (Vector3D collisionTestPoint in collisionTestPoints)
			{
=======
			{
				return;
			}
			bool flag = true;
			foreach (Vector3D collisionTestPoint in collisionTestPoints)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyCubeGrid previewGrid in m_previewGrids)
				{
					Vector3D point = Vector3.Transform(collisionTestPoint, previewGrid.PositionComp.WorldMatrixNormalizedInv);
					if (previewGrid.PositionComp.LocalAABB.Contains(point) == ContainmentType.Contains)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			foreach (MyCubeGrid previewGrid2 in m_previewGrids)
			{
				previewGrid2.Render.Visible = flag;
			}
		}

		public void RotateAroundAxis(int axisIndex, int sign, bool newlyPressed, float angleDelta)
		{
			if ((!EnableStationRotation || IsSnapped) && !EnablePreciseRotationWhenSnapped)
			{
				if (!newlyPressed)
				{
					return;
				}
				angleDelta = (float)Math.E * 449f / 777f;
			}
			switch (axisIndex)
			{
			case 0:
				if (sign < 0)
				{
					UpMinus(angleDelta);
				}
				else
				{
					UpPlus(angleDelta);
				}
				break;
			case 1:
				if (sign < 0)
				{
					AngleMinus(angleDelta);
				}
				else
				{
					AnglePlus(angleDelta);
				}
				break;
			case 2:
				if (sign < 0)
				{
					RightPlus(angleDelta);
				}
				else
				{
					RightMinus(angleDelta);
				}
				break;
			}
			ApplyOrientationAngle();
		}

		private void AnglePlus(float angle)
		{
			m_pasteOrientationAngle += angle;
			if (m_pasteOrientationAngle >= (float)Math.PI * 2f)
			{
				m_pasteOrientationAngle -= (float)Math.PI * 2f;
			}
		}

		private void AngleMinus(float angle)
		{
			m_pasteOrientationAngle -= angle;
			if (m_pasteOrientationAngle < 0f)
			{
				m_pasteOrientationAngle += (float)Math.PI * 2f;
			}
		}

		private void UpPlus(float angle)
		{
			if (!OneAxisRotationMode)
			{
				ApplyOrientationAngle();
				Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
				float num = (float)Math.Cos(angle);
				float num2 = (float)Math.Sin(angle);
				Vector3 pasteDirUp = m_pasteDirUp * num - m_pasteDirForward * num2;
				m_pasteDirForward = m_pasteDirUp * num2 + m_pasteDirForward * num;
				m_pasteDirUp = pasteDirUp;
			}
		}

		private void UpMinus(float angle)
		{
			UpPlus(0f - angle);
		}

		private void RightPlus(float angle)
		{
			if (!OneAxisRotationMode)
			{
				ApplyOrientationAngle();
				Vector3 vector = Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
				float num = (float)Math.Cos(angle);
				float num2 = (float)Math.Sin(angle);
				m_pasteDirUp = m_pasteDirUp * num + vector * num2;
			}
		}

		private void RightMinus(float angle)
		{
			RightPlus(0f - angle);
		}

		public virtual void MoveEntityFurther()
		{
			float value = m_dragDistance * 1.1f;
			m_dragDistance = MathHelper.Clamp(value, m_dragDistance, 20000f);
		}

		public virtual void MoveEntityCloser()
		{
			m_dragDistance /= 1.1f;
		}

		private void ApplyOrientationAngle()
		{
			m_pasteDirForward = Vector3.Normalize(m_pasteDirForward);
			m_pasteDirUp = Vector3.Normalize(m_pasteDirUp);
			Vector3 vector = Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
			float num = (float)Math.Cos(m_pasteOrientationAngle);
			float num2 = (float)Math.Sin(m_pasteOrientationAngle);
			m_pasteDirForward = m_pasteDirForward * num - vector * num2;
			m_pasteOrientationAngle = 0f;
		}
	}
}
