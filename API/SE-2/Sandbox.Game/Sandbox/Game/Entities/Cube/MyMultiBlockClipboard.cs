using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	/// <summary>
	///  Multiblock clipboard for building multiblocks. Can be used for building only (not copy/paste) because it uses definitions not real tile grid/block data.
	/// </summary>
	public class MyMultiBlockClipboard : MyGridClipboardAdvanced
	{
		private static List<Vector3D> m_tmpCollisionPoints = new List<Vector3D>();

		private static List<MyEntity> m_tmpNearEntities = new List<MyEntity>();

		private MyMultiBlockDefinition m_multiBlockDefinition;

		public MySlimBlock RemoveBlock;

		public ushort? BlockIdInCompound;

		private Vector3I m_addPos;

		public HashSet<Tuple<MySlimBlock, ushort?>> RemoveBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();

		private HashSet<Vector3I> m_tmpBlockPositionsSet = new HashSet<Vector3I>();

		private bool m_lastVoxelState;

		protected override bool AnyCopiedGridIsStatic => false;

		public MyMultiBlockClipboard(MyPlacementSettings settings, bool calculateVelocity = true)
			: base(settings, calculateVelocity)
		{
			m_useDynamicPreviews = false;
		}

		public override void Deactivate(bool afterPaste = false)
		{
			m_multiBlockDefinition = null;
			base.Deactivate(afterPaste);
		}

		public override void Update()
		{
			if (!base.IsActive)
			{
				return;
			}
			UpdateHitEntity();
			if (!m_visible)
			{
				ShowPreview(show: false);
			}
			else
			{
				if (base.PreviewGrids.Count == 0)
				{
					return;
				}
				if (m_dragDistance == 0f)
				{
					SetupDragDistance();
				}
				if (m_dragDistance > MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
				{
					m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
				}
				UpdatePastePosition();
				UpdateGridTransformations();
				FixSnapTransformationBase6();
				if (m_calculateVelocity)
				{
					m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
				}
				m_canBePlaced = TestPlacement();
				if (!m_visible)
				{
					ShowPreview(show: false);
					return;
				}
				ShowPreview(show: true);
				TestBuildingMaterials();
				m_canBePlaced &= base.CharacterHasEnoughMaterials;
				UpdatePreview();
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
				}
			}
		}

		public override bool PasteGrid(bool deactivate = true, bool showWarning = true)
		{
			if (base.CopiedGrids.Count > 0 && !base.IsActive)
			{
				Activate();
				return true;
			}
			if (!m_canBePlaced)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			if (base.PreviewGrids.Count == 0)
			{
				return false;
			}
			bool flag = RemoveBlock != null && !RemoveBlock.CubeGrid.IsStatic;
			if (MyCubeBuilder.Static.DynamicMode || flag)
			{
				return PasteGridsInDynamicMode(deactivate);
			}
			return PasteGridsInStaticMode(deactivate);
		}

		public override bool EntityCanPaste(MyEntity pastingEntity)
		{
			if (base.CopiedGrids.Count < 1)
			{
				return false;
			}
			if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				return true;
			}
			MyCubeBuilder.BuildComponent.GetMultiBlockPlacementMaterials(m_multiBlockDefinition);
			return MyCubeBuilder.BuildComponent.HasBuildingMaterials(pastingEntity);
		}

		private bool PasteGridsInDynamicMode(bool deactivate)
		{
			List<bool> list = new List<bool>();
			foreach (MyObjectBuilder_CubeGrid copiedGrid in base.CopiedGrids)
			{
				list.Add(copiedGrid.IsStatic);
				copiedGrid.IsStatic = false;
				BeforeCreateGrid(copiedGrid);
			}
			bool result = PasteGridInternal(deactivate, null, null, null, multiBlock: true);
			for (int i = 0; i < base.CopiedGrids.Count; i++)
			{
				base.CopiedGrids[i].IsStatic = list[i];
			}
			return result;
		}

		private bool PasteGridsInStaticMode(bool deactivate)
		{
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			List<MatrixD> list2 = new List<MatrixD>();
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = base.CopiedGrids[0];
			BeforeCreateGrid(myObjectBuilder_CubeGrid);
			list.Add(myObjectBuilder_CubeGrid);
			MatrixD worldMatrix = base.PreviewGrids[0].WorldMatrix;
			MyObjectBuilder_CubeGrid value = MyCubeBuilder.ConvertGridBuilderToStatic(myObjectBuilder_CubeGrid, worldMatrix);
			base.CopiedGrids[0] = value;
			list2.Add(worldMatrix);
			for (int i = 1; i < base.CopiedGrids.Count; i++)
			{
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid2 = base.CopiedGrids[i];
				BeforeCreateGrid(myObjectBuilder_CubeGrid2);
				list.Add(myObjectBuilder_CubeGrid2);
				MatrixD worldMatrix2 = base.PreviewGrids[i].WorldMatrix;
				list2.Add(worldMatrix2);
				if (base.CopiedGrids[i].IsStatic)
				{
					MyObjectBuilder_CubeGrid value2 = MyCubeBuilder.ConvertGridBuilderToStatic(myObjectBuilder_CubeGrid2, worldMatrix2);
					base.CopiedGrids[i] = value2;
				}
			}
			bool result = PasteGridInternal(deactivate, null, m_touchingGrids, null, multiBlock: true);
			base.CopiedGrids.Clear();
			base.CopiedGrids.AddRange(list);
			for (int j = 0; j < base.PreviewGrids.Count; j++)
			{
				base.PreviewGrids[j].WorldMatrix = list2[j];
			}
			return result;
		}

		protected override void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			if (MyCubeBuilder.Static.HitInfo.HasValue)
			{
				m_pastePosition = MyCubeBuilder.Static.HitInfo.Value.Position;
			}
			else
			{
				m_pastePosition = MyCubeBuilder.Static.FreePlacementTarget;
			}
			double gridSize = MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
			MyCoordinateSystem.CoordSystemData coordSystemData = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref m_pastePosition, gridSize, m_settings.StaticGridAlignToCenter);
			base.EnableStationRotation = MyCubeBuilder.Static.DynamicMode;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				AlignClipboardToGravity();
				m_visible = true;
				base.IsSnapped = false;
				m_lastVoxelState = false;
			}
			else if (RemoveBlock != null)
			{
				m_pastePosition = Vector3D.Transform(m_addPos * RemoveBlock.CubeGrid.GridSize, RemoveBlock.CubeGrid.WorldMatrix);
				if (!base.IsSnapped && RemoveBlock.CubeGrid.IsStatic)
				{
					m_pasteOrientationAngle = 0f;
					m_pasteDirForward = RemoveBlock.CubeGrid.WorldMatrix.Forward;
					m_pasteDirUp = RemoveBlock.CubeGrid.WorldMatrix.Up;
				}
				base.IsSnapped = true;
				m_lastVoxelState = false;
			}
			else
			{
				if (!MyFakes.ENABLE_BLOCK_PLACEMENT_ON_VOXEL || !(m_hitEntity is MyVoxelBase))
				{
					return;
				}
				if (MyCoordinateSystem.Static.LocalCoordExist)
				{
					m_pastePosition = coordSystemData.SnappedTransform.Position;
					if (!m_lastVoxelState)
					{
						AlignRotationToCoordSys();
					}
				}
				base.IsSnapped = true;
				m_lastVoxelState = true;
			}
		}

		public override void MoveEntityFurther()
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				base.MoveEntityFurther();
				if (m_dragDistance > MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
				{
					m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
				}
			}
		}

		public override void MoveEntityCloser()
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				base.MoveEntityCloser();
				if (m_dragDistance < MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
				{
					m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
				}
			}
		}

		protected override void ChangeClipboardPreview(bool visible, List<MyCubeGrid> previewGrids, List<MyObjectBuilder_CubeGrid> copiedGrids)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			base.ChangeClipboardPreview(visible, previewGrids, copiedGrids);
			if (!visible || !MySession.Static.SurvivalMode)
			{
				return;
			}
			foreach (MyCubeGrid previewGrid in base.PreviewGrids)
			{
<<<<<<< HEAD
				foreach (MySlimBlock block in previewGrid.GetBlocks())
				{
					MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
						{
							SetBlockToFullIntegrity(block2);
=======
				Enumerator<MySlimBlock> enumerator2 = previewGrid.GetBlocks().GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MySlimBlock current = enumerator2.get_Current();
						MyCompoundCubeBlock myCompoundCubeBlock = current.FatBlock as MyCompoundCubeBlock;
						if (myCompoundCubeBlock != null)
						{
							foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
							{
								SetBlockToFullIntegrity(block);
							}
						}
						else
						{
							SetBlockToFullIntegrity(current);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					else
					{
						SetBlockToFullIntegrity(block);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
		}

		private static void SetBlockToFullIntegrity(MySlimBlock block)
		{
			float buildRatio = block.ComponentStack.BuildRatio;
			block.ComponentStack.SetIntegrity(block.ComponentStack.MaxIntegrity, block.ComponentStack.MaxIntegrity);
			if (block.BlockDefinition.ModelChangeIsNeeded(buildRatio, block.ComponentStack.BuildRatio))
			{
				block.UpdateVisual();
			}
		}

		private void UpdateHitEntity()
		{
			m_closestHitDistSq = float.MaxValue;
			m_hitPos = new Vector3(0f, 0f, 0f);
			m_hitNormal = new Vector3(1f, 0f, 0f);
			m_hitEntity = null;
			m_addPos = Vector3I.Zero;
			RemoveBlock = null;
			BlockIdInCompound = null;
			RemoveBlocksInMultiBlock.Clear();
			m_dynamicBuildAllowed = false;
			m_visible = false;
			m_canBePlaced = false;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				m_visible = true;
				return;
			}
			MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
			if (MyCubeBuilder.Static.CurrentGrid == null && MyCubeBuilder.Static.CurrentVoxelBase == null)
			{
				MyCubeBuilder.Static.ChooseHitObject();
			}
			if (MyCubeBuilder.Static.HitInfo.HasValue)
			{
				float cubeSize = MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
				MyCubeGrid myCubeGrid = MyCubeBuilder.Static.HitInfo.Value.HkHitInfo.GetHitEntity() as MyCubeGrid;
				bool placingSmallGridOnLargeStatic = myCubeGrid != null && myCubeGrid.IsStatic && myCubeGrid.GridSizeEnum == MyCubeSize.Large && base.CopiedGrids[0].GridSizeEnum == MyCubeSize.Small && MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE;
				if (MyCubeBuilder.Static.GetAddAndRemovePositions(cubeSize, placingSmallGridOnLargeStatic, out m_addPos, out var _, out var addDir, out var _, out RemoveBlock, out BlockIdInCompound, RemoveBlocksInMultiBlock))
				{
					if (RemoveBlock != null)
					{
						m_hitPos = MyCubeBuilder.Static.HitInfo.Value.Position;
						m_closestHitDistSq = (float)(m_hitPos - pasteMatrix.Translation).LengthSquared();
						m_hitNormal = addDir;
						m_hitEntity = RemoveBlock.CubeGrid;
						double num = MyDefinitionManager.Static.GetCubeSize(RemoveBlock.CubeGrid.GridSizeEnum);
						double num2 = MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
						if (num / num2 < 1.0)
						{
							RemoveBlock = null;
						}
						m_visible = RemoveBlock != null;
					}
					else if (MyFakes.ENABLE_BLOCK_PLACEMENT_ON_VOXEL && MyCubeBuilder.Static.HitInfo.Value.HkHitInfo.GetHitEntity() is MyVoxelBase)
					{
						m_hitPos = MyCubeBuilder.Static.HitInfo.Value.Position;
						m_closestHitDistSq = (float)(m_hitPos - pasteMatrix.Translation).LengthSquared();
						m_hitNormal = addDir;
						m_hitEntity = MyCubeBuilder.Static.HitInfo.Value.HkHitInfo.GetHitEntity() as MyVoxelBase;
						m_visible = true;
					}
					else
					{
						m_visible = false;
					}
				}
				else
				{
					m_visible = false;
				}
			}
			else
			{
				m_visible = false;
			}
		}

		private new void FixSnapTransformationBase6()
		{
			if (base.CopiedGrids.Count == 0)
			{
				return;
			}
			MyCubeGrid myCubeGrid = m_hitEntity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			Matrix rotationDeltaMatrixToHitGrid = GetRotationDeltaMatrixToHitGrid(myCubeGrid);
			foreach (MyCubeGrid previewGrid in base.PreviewGrids)
			{
				MatrixD m = previewGrid.WorldMatrix.GetOrientation();
				Matrix matrix = m;
				matrix *= rotationDeltaMatrixToHitGrid;
				MatrixD worldMatrix = MatrixD.CreateWorld(m_pastePosition, matrix.Forward, matrix.Up);
				previewGrid.PositionComp.SetWorldMatrix(ref worldMatrix);
			}
			if (myCubeGrid.GridSizeEnum == MyCubeSize.Large && base.PreviewGrids[0].GridSizeEnum == MyCubeSize.Small)
			{
				Vector3 vector = MyCubeBuilder.TransformLargeGridHitCoordToSmallGrid(m_hitPos, myCubeGrid.PositionComp.WorldMatrixNormalizedInv, myCubeGrid.GridSize);
				m_pastePosition = myCubeGrid.GridIntegerToWorld(vector);
			}
			else
			{
				Vector3I vector2 = Vector3I.Round(m_hitNormal);
				Vector3I vector3I = myCubeGrid.WorldToGridInteger(m_pastePosition);
				Vector3I min = base.PreviewGrids[0].Min;
				Vector3I vector3 = Vector3I.Abs(Vector3I.Round(Vector3D.TransformNormal(Vector3D.TransformNormal((Vector3D)(base.PreviewGrids[0].Max - min + Vector3I.One), base.PreviewGrids[0].WorldMatrix), myCubeGrid.PositionComp.WorldMatrixNormalizedInv)));
				int num = Math.Abs(Vector3I.Dot(ref vector2, ref vector3));
				int i;
				for (i = 0; i < num; i++)
				{
					if (myCubeGrid.CanMergeCubes(base.PreviewGrids[0], vector3I))
					{
						break;
					}
					vector3I += vector2;
				}
				if (i == num)
				{
					vector3I = myCubeGrid.WorldToGridInteger(m_pastePosition);
				}
				m_pastePosition = myCubeGrid.GridIntegerToWorld(vector3I);
			}
			for (int j = 0; j < base.PreviewGrids.Count; j++)
			{
				MyCubeGrid myCubeGrid2 = base.PreviewGrids[j];
				MatrixD worldMatrix2 = myCubeGrid2.WorldMatrix;
				worldMatrix2.Translation = m_pastePosition + Vector3.Transform(m_copiedGridOffsets[j], rotationDeltaMatrixToHitGrid);
				myCubeGrid2.PositionComp.SetWorldMatrix(ref worldMatrix2);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawLine3D(m_hitPos, m_hitPos + m_hitNormal, Color.Red, Color.Green, depthRead: false);
			}
		}

		public Matrix GetRotationDeltaMatrixToHitGrid(MyCubeGrid hitGrid)
		{
			MatrixD m = hitGrid.WorldMatrix.GetOrientation();
			Matrix axisDefinitionMatrix = m;
			m = base.PreviewGrids[0].WorldMatrix.GetOrientation();
			Matrix toAlign = m;
			Matrix matrix = Matrix.AlignRotationToAxes(ref toAlign, ref axisDefinitionMatrix);
			return Matrix.Invert(toAlign) * matrix;
		}

		private new bool TestPlacement()
		{
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			bool flag = true;
			m_touchingGrids.Clear();
			for (int i = 0; i < base.PreviewGrids.Count; i++)
			{
				MyCubeGrid myCubeGrid = base.PreviewGrids[i];
				if (!MyEntities.IsInsideWorld(myCubeGrid.PositionComp.GetPosition()))
				{
					return false;
				}
				MyGridPlacementSettings settings = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum);
				m_touchingGrids.Add(null);
				if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					if (i == 0 && MyBlockBuilderBase.CameraControllerSpectator)
					{
						m_visible = false;
						return false;
					}
					if (i == 0 && !MyCubeBuilder.Static.DynamicMode)
					{
						MatrixD invGridWorldMatrix = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
						if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, myCubeGrid.PositionComp.LocalAABB, myCubeGrid.GridSize, MyBlockBuilderBase.IntersectionDistance))
						{
							m_visible = false;
							return false;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (MyCubeBuilder.Static.DynamicMode)
				{
					MyGridPlacementSettings settings2 = ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? m_settings.LargeGrid : m_settings.SmallGrid);
					bool flag2 = false;
					Enumerator<MySlimBlock> enumerator = myCubeGrid.GetBlocks().GetEnumerator();
					try
					{
<<<<<<< HEAD
						Vector3 vector = block.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
						Vector3 vector2 = block.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
						BoundingBoxD localAabb = new BoundingBoxD(vector, vector2);
						if (!flag2)
						{
							flag2 = MyGridClipboardAdvanced.TestVoxelPlacement(block, ref settings, dynamicMode: true);
						}
						flag = flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings2, localAabb, dynamicBuildMode: true, null, testVoxel: false);
						if (!flag)
						{
							break;
						}
					}
=======
						while (enumerator.MoveNext())
						{
							MySlimBlock current = enumerator.get_Current();
							Vector3 vector = current.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
							Vector3 vector2 = current.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
							BoundingBoxD localAabb = new BoundingBoxD(vector, vector2);
							if (!flag2)
							{
								flag2 = MyGridClipboardAdvanced.TestVoxelPlacement(current, ref settings, dynamicMode: true);
							}
							flag = flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings2, localAabb, dynamicBuildMode: true, null, testVoxel: false);
							if (!flag)
							{
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					flag = flag && flag2;
				}
				else if (i == 0 && m_hitEntity is MyCubeGrid && base.IsSnapped)
				{
					MyCubeGrid myCubeGrid2 = m_hitEntity as MyCubeGrid;
					MyGridPlacementSettings settings3 = m_settings.GetGridPlacementSettings(myCubeGrid2.GridSizeEnum, myCubeGrid2.IsStatic);
					flag = ((myCubeGrid2.GridSizeEnum != 0 || myCubeGrid.GridSizeEnum != MyCubeSize.Small) ? (flag && TestGridPlacementOnGrid(myCubeGrid, ref settings3, myCubeGrid2)) : (flag && MyCubeGrid.TestPlacementArea(myCubeGrid, ref settings, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false)));
					m_touchingGrids.Clear();
					m_touchingGrids.Add(myCubeGrid2);
				}
				else if (i == 0 && m_hitEntity is MyVoxelMap)
				{
					bool flag3 = false;
					Enumerator<MySlimBlock> enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MySlimBlock current2 = enumerator.get_Current();
							if (current2.FatBlock is MyCompoundCubeBlock)
							{
								foreach (MySlimBlock block in (current2.FatBlock as MyCompoundCubeBlock).GetBlocks())
								{
									if (!flag3)
									{
										flag3 = MyGridClipboardAdvanced.TestVoxelPlacement(block, ref settings, dynamicMode: false);
									}
									flag = flag && MyGridClipboardAdvanced.TestBlockPlacementArea(block, ref settings, dynamicMode: false, testVoxel: false);
									if (!flag)
									{
										break;
									}
								}
<<<<<<< HEAD
								flag = flag && MyGridClipboardAdvanced.TestBlockPlacementArea(block2, ref settings, dynamicMode: false, testVoxel: false);
								if (!flag)
=======
							}
							else
							{
								if (!flag3)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									flag3 = MyGridClipboardAdvanced.TestVoxelPlacement(current2, ref settings, dynamicMode: false);
								}
								flag = flag && MyGridClipboardAdvanced.TestBlockPlacementArea(current2, ref settings, dynamicMode: false, testVoxel: false);
							}
							if (!flag)
							{
								break;
							}
<<<<<<< HEAD
							flag = flag && MyGridClipboardAdvanced.TestBlockPlacementArea(cubeBlock, ref settings, dynamicMode: false, testVoxel: false);
						}
						if (!flag)
						{
							break;
						}
					}
=======
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					flag = flag && flag3;
					m_touchingGrids[i] = DetectTouchingGrid();
				}
				else
				{
					MyGridPlacementSettings settings4 = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic && !MyCubeBuilder.Static.DynamicMode);
					flag = flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings4, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false);
				}
				BoundingBoxD boundingBoxD = myCubeGrid.PositionComp.LocalAABB;
				MatrixD worldMatrixNormalizedInv = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
				if (MySector.MainCamera != null)
				{
					Vector3D point = Vector3D.Transform(MySector.MainCamera.Position, worldMatrixNormalizedInv);
					flag = flag && boundingBoxD.Contains(point) != ContainmentType.Contains;
<<<<<<< HEAD
				}
				if (!flag)
				{
					continue;
				}
				m_tmpCollisionPoints.Clear();
				MyCubeBuilder.PrepareCharacterCollisionPoints(m_tmpCollisionPoints);
				foreach (Vector3D tmpCollisionPoint in m_tmpCollisionPoints)
				{
=======
				}
				if (!flag)
				{
					continue;
				}
				m_tmpCollisionPoints.Clear();
				MyCubeBuilder.PrepareCharacterCollisionPoints(m_tmpCollisionPoints);
				foreach (Vector3D tmpCollisionPoint in m_tmpCollisionPoints)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Vector3D point2 = Vector3D.Transform(tmpCollisionPoint, worldMatrixNormalizedInv);
					flag = flag && boundingBoxD.Contains(point2) != ContainmentType.Contains;
					if (!flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		/// <summary>
		/// Detects a grid where multiblock can be merged.
		/// </summary>
		private MyCubeGrid DetectTouchingGrid()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (base.PreviewGrids == null || base.PreviewGrids.Count == 0)
			{
				return null;
			}
			Enumerator<MySlimBlock> enumerator = base.PreviewGrids[0].CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					MyCubeGrid myCubeGrid = DetectTouchingGrid(current);
					if (myCubeGrid != null)
					{
						return myCubeGrid;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		private MyCubeGrid DetectTouchingGrid(MySlimBlock block)
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				return null;
			}
			if (block == null)
			{
				return null;
			}
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					MyCubeGrid myCubeGrid = DetectTouchingGrid(block2);
					if (myCubeGrid != null)
					{
						return myCubeGrid;
					}
				}
				return null;
			}
			float gridSize = block.CubeGrid.GridSize;
			block.GetWorldBoundingBox(out var aabb);
			aabb.Inflate(gridSize / 2f);
			m_tmpNearEntities.Clear();
			MyEntities.GetElementsInBox(ref aabb, m_tmpNearEntities);
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
			try
			{
				for (int i = 0; i < m_tmpNearEntities.Count; i++)
				{
					MyCubeGrid myCubeGrid2 = m_tmpNearEntities[i] as MyCubeGrid;
					if (myCubeGrid2 == null || myCubeGrid2 == block.CubeGrid || myCubeGrid2.Physics == null || !myCubeGrid2.Physics.Enabled || !myCubeGrid2.IsStatic || myCubeGrid2.GridSizeEnum != block.CubeGrid.GridSizeEnum)
					{
						continue;
					}
					Vector3I gridOffset = myCubeGrid2.WorldToGridInteger(m_pastePosition);
					if (myCubeGrid2.CanMergeCubes(block.CubeGrid, gridOffset))
					{
						MatrixI transformation = myCubeGrid2.CalculateMergeTransform(block.CubeGrid, gridOffset);
						Base6Directions.Direction direction = transformation.GetDirection(block.Orientation.Forward);
						Base6Directions.Direction direction2 = transformation.GetDirection(block.Orientation.Up);
						new MyBlockOrientation(direction, direction2).GetQuaternion(out var result);
						Vector3I position = Vector3I.Transform(block.Position, transformation);
						if (MyCubeGrid.CheckConnectivity(myCubeGrid2, block.BlockDefinition, buildProgressModelMountPoints, ref result, ref position))
						{
							return myCubeGrid2;
						}
					}
				}
			}
			finally
			{
				m_tmpNearEntities.Clear();
			}
			return null;
		}

		private void UpdatePreview()
		{
<<<<<<< HEAD
=======
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (base.PreviewGrids == null || !m_visible || !HasPreviewBBox)
			{
				return;
			}
			MyStringId value = (m_canBePlaced ? MyGridClipboard.ID_GIZMO_DRAW_LINE : MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
			Color color = Color.White;
			foreach (MyCubeGrid previewGrid in base.PreviewGrids)
			{
				BoundingBoxD localbox = previewGrid.PositionComp.LocalAABB;
				MatrixD worldMatrix = previewGrid.PositionComp.WorldMatrixRef;
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value);
			}
			Vector4 vector = new Vector4(Color.Red.ToVector3() * 0.8f, 1f);
<<<<<<< HEAD
			if (RemoveBlocksInMultiBlock.Count > 0)
			{
				m_tmpBlockPositionsSet.Clear();
				MyCubeBuilder.GetAllBlocksPositions(RemoveBlocksInMultiBlock, m_tmpBlockPositionsSet);
				foreach (Vector3I item in m_tmpBlockPositionsSet)
				{
					MyCubeBuilder.DrawSemiTransparentBox(item, item, RemoveBlock.CubeGrid, vector, onlyWireframe: false, MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
=======
			if (RemoveBlocksInMultiBlock.get_Count() > 0)
			{
				m_tmpBlockPositionsSet.Clear();
				MyCubeBuilder.GetAllBlocksPositions(RemoveBlocksInMultiBlock, m_tmpBlockPositionsSet);
				Enumerator<Vector3I> enumerator2 = m_tmpBlockPositionsSet.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Vector3I current2 = enumerator2.get_Current();
						MyCubeBuilder.DrawSemiTransparentBox(current2, current2, RemoveBlock.CubeGrid, vector, onlyWireframe: false, MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_tmpBlockPositionsSet.Clear();
			}
			else if (RemoveBlock != null)
			{
				MyCubeBuilder.DrawSemiTransparentBox(RemoveBlock.CubeGrid, RemoveBlock, vector, onlyWireframe: false, MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
			}
		}

		protected override void SetupDragDistance()
		{
			if (base.IsActive)
			{
				base.SetupDragDistance();
				if (MySession.Static.SurvivalMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					m_dragDistance = MyBlockBuilderBase.IntersectionDistance;
				}
			}
		}

		public void SetGridFromBuilder(MyMultiBlockDefinition multiBlockDefinition, MyObjectBuilder_CubeGrid grid, Vector3 dragPointDelta, float dragVectorLength)
		{
			ChangeClipboardPreview(visible: false, m_previewGrids, m_copiedGrids);
			m_multiBlockDefinition = multiBlockDefinition;
			SetGridFromBuilder(grid, dragPointDelta, dragVectorLength);
			ChangeClipboardPreview(visible: true, m_previewGrids, m_copiedGrids);
		}

		public static void TakeMaterialsFromBuilder(List<MyObjectBuilder_CubeGrid> blocksToBuild, MyEntity builder)
		{
			if (blocksToBuild.Count == 0)
			{
				return;
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = Enumerable.FirstOrDefault<MyObjectBuilder_CubeBlock>((IEnumerable<MyObjectBuilder_CubeBlock>)blocksToBuild[0].CubeBlocks);
			if (myObjectBuilder_CubeBlock == null)
			{
				return;
			}
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = myObjectBuilder_CubeBlock as MyObjectBuilder_CompoundCubeBlock;
			MyDefinitionId id;
			if (myObjectBuilder_CompoundCubeBlock != null)
			{
				if (myObjectBuilder_CompoundCubeBlock.Blocks == null || myObjectBuilder_CompoundCubeBlock.Blocks.Length == 0 || !myObjectBuilder_CompoundCubeBlock.Blocks[0].MultiBlockDefinition.HasValue)
				{
					return;
				}
				id = myObjectBuilder_CompoundCubeBlock.Blocks[0].MultiBlockDefinition.Value;
			}
			else
			{
				if (!myObjectBuilder_CubeBlock.MultiBlockDefinition.HasValue)
				{
					return;
				}
				id = myObjectBuilder_CubeBlock.MultiBlockDefinition.Value;
			}
			MyMultiBlockDefinition myMultiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(id);
			if (myMultiBlockDefinition != null)
			{
				MyCubeBuilder.BuildComponent.GetMultiBlockPlacementMaterials(myMultiBlockDefinition);
				MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builder, instantBuild: false);
			}
		}
	}
}
