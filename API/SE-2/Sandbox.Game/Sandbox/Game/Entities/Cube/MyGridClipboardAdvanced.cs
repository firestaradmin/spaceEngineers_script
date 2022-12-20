using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	/// <summary>
	/// Enhanced clipboard which supports dynamic placing, allow rotation of static grid.
	/// </summary>
	public class MyGridClipboardAdvanced : MyGridClipboard
	{
		private static List<Vector3D> m_tmpCollisionPoints = new List<Vector3D>();

		protected bool m_dynamicBuildAllowed;

		protected override bool AnyCopiedGridIsStatic => false;

		public MyGridClipboardAdvanced(MyPlacementSettings settings, bool calculateVelocity = true)
			: base(settings, calculateVelocity)
		{
			m_useDynamicPreviews = false;
			m_dragDistance = 0f;
		}

		public override void Update()
		{
			if (!base.IsActive || !m_visible)
			{
				return;
			}
			bool flag = UpdateHitEntity(canPasteLargeOnSmall: false);
			if (MyFakes.ENABLE_VR_BUILDING && !flag)
			{
				Hide();
				return;
			}
			if (!m_visible)
			{
				Hide();
				return;
			}
			Show();
			if (m_dragDistance == 0f)
			{
				SetupDragDistance();
			}
			UpdatePastePosition();
			UpdateGridTransformations();
			if (MyCubeBuilder.Static.CubePlacementMode != MyCubeBuilder.CubePlacementModeEnum.FreePlacement)
			{
				FixSnapTransformationBase6();
			}
			if (m_calculateVelocity)
			{
				m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
			}
			m_canBePlaced = TestPlacement();
			TestBuildingMaterials();
			UpdatePreview();
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
			}
		}

		public override void Activate(Action callback = null)
		{
			base.Activate(callback);
			SetupDragDistance();
		}

		/// <summary>
		/// Converts the given grid to static with the world matrix. Instead of grid (which must have identity rotation for static grid) we transform blocks in the grid.
		/// </summary>
		/// <param name="originalGrid">grid to be transformed</param>
		/// <param name="worldMatrix">target world transform</param>
		private static void ConvertGridBuilderToStatic(MyObjectBuilder_CubeGrid originalGrid, MatrixD worldMatrix)
		{
			originalGrid.IsStatic = true;
			originalGrid.PositionAndOrientation = new MyPositionAndOrientation(originalGrid.PositionAndOrientation.Value.Position, Vector3.Forward, Vector3.Up);
			Vector3 vec = worldMatrix.Forward;
			Vector3 vec2 = worldMatrix.Up;
			Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(vec);
			Base6Directions.Direction direction = Base6Directions.GetClosestDirection(vec2);
			if (direction == closestDirection)
			{
				direction = Base6Directions.GetPerpendicular(closestDirection);
			}
			MatrixI transform = new MatrixI(Vector3I.Zero, closestDirection, direction);
			foreach (MyObjectBuilder_CubeBlock cubeBlock in originalGrid.CubeBlocks)
			{
				if (cubeBlock is MyObjectBuilder_CompoundCubeBlock)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
					ConvertRotatedGridCompoundBlockToStatic(ref transform, myObjectBuilder_CompoundCubeBlock);
					for (int i = 0; i < myObjectBuilder_CompoundCubeBlock.Blocks.Length; i++)
					{
						MyObjectBuilder_CubeBlock origBlock = myObjectBuilder_CompoundCubeBlock.Blocks[i];
						ConvertRotatedGridBlockToStatic(ref transform, origBlock);
					}
				}
				else
				{
					ConvertRotatedGridBlockToStatic(ref transform, cubeBlock);
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
			bool flag = m_hitEntity is MyCubeGrid && !((MyCubeGrid)m_hitEntity).IsStatic && !MyCubeBuilder.Static.DynamicMode;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				return PasteGridsInDynamicMode(deactivate);
			}
			if (flag)
			{
				return PasteGridInternal(deactivate);
			}
			if (MyFakes.ENABLE_VR_BUILDING)
			{
				return PasteGridInternal(deactivate);
			}
			return PasteGridsInStaticMode(deactivate);
		}

		private bool PasteGridsInDynamicMode(bool deactivate)
		{
			List<bool> list = new List<bool>();
			foreach (MyObjectBuilder_CubeGrid copiedGrid in base.CopiedGrids)
			{
				list.Add(copiedGrid.IsStatic);
				copiedGrid.IsStatic = false;
			}
			bool result = PasteGridInternal(deactivate);
			for (int i = 0; i < base.CopiedGrids.Count; i++)
			{
				base.CopiedGrids[i].IsStatic = list[i];
			}
			return result;
		}

		private bool PasteGridsInStaticMode(bool deactivate)
		{
			MyObjectBuilder_CubeGrid originalGrid = base.CopiedGrids[0];
			MatrixD worldMatrix = base.PreviewGrids[0].WorldMatrix;
			ConvertGridBuilderToStatic(originalGrid, worldMatrix);
			base.PreviewGrids[0].WorldMatrix = MatrixD.CreateTranslation(worldMatrix.Translation);
			for (int i = 1; i < base.CopiedGrids.Count; i++)
			{
				if (base.CopiedGrids[i].IsStatic)
				{
					MyObjectBuilder_CubeGrid originalGrid2 = base.CopiedGrids[i];
					MatrixD worldMatrix2 = base.PreviewGrids[i].WorldMatrix;
					ConvertGridBuilderToStatic(originalGrid2, worldMatrix2);
					base.PreviewGrids[i].WorldMatrix = MatrixD.CreateTranslation(worldMatrix2.Translation);
				}
			}
			List<MyObjectBuilder_CubeGrid> pastedBuilders = new List<MyObjectBuilder_CubeGrid>();
			bool num = PasteGridInternal(deactivate: true, pastedBuilders, m_touchingGrids, delegate(List<MyObjectBuilder_CubeGrid> pastedBuildersInCallback)
			{
				UpdateAfterPaste(deactivate, pastedBuildersInCallback);
			});
			if (num)
			{
				UpdateAfterPaste(deactivate, pastedBuilders);
			}
			return num;
		}

		private void UpdateAfterPaste(bool deactivate, List<MyObjectBuilder_CubeGrid> pastedBuilders)
		{
			if (base.CopiedGrids.Count == pastedBuilders.Count)
			{
				m_copiedGridOffsets.Clear();
				for (int i = 0; i < base.CopiedGrids.Count; i++)
				{
					base.CopiedGrids[i].PositionAndOrientation = pastedBuilders[i].PositionAndOrientation;
					m_copiedGridOffsets.Add((Vector3D)base.CopiedGrids[i].PositionAndOrientation.Value.Position - (Vector3D)base.CopiedGrids[0].PositionAndOrientation.Value.Position);
				}
				m_pasteOrientationAngle = 0f;
				m_pasteDirForward = Vector3I.Forward;
				m_pasteDirUp = Vector3I.Up;
				if (!deactivate)
				{
					Activate();
				}
			}
		}

		/// <summary>
		/// Converts the given block with the given matrix for static grid.
		/// </summary>
		private static void ConvertRotatedGridBlockToStatic(ref MatrixI transform, MyObjectBuilder_CubeBlock origBlock)
		{
			MyDefinitionId defId = new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out var blockDefinition);
			if (blockDefinition != null)
			{
				MyBlockOrientation orientation = origBlock.BlockOrientation;
				Vector3I min = origBlock.Min;
				MySlimBlock.ComputeMax(blockDefinition, orientation, ref min, out var max);
				Vector3I.Transform(ref min, ref transform, out var result);
				Vector3I.Transform(ref max, ref transform, out var result2);
				Base6Directions.Direction direction = transform.GetDirection(orientation.Forward);
				Base6Directions.Direction direction2 = transform.GetDirection(orientation.Up);
				new MyBlockOrientation(direction, direction2).GetQuaternion(out var result3);
				origBlock.Orientation = result3;
				origBlock.Min = Vector3I.Min(result, result2);
			}
		}

		/// <summary>
		/// Transforms given compound block with matrix for static grid. Rotation of block is not changed.
		/// </summary>
		private static void ConvertRotatedGridCompoundBlockToStatic(ref MatrixI transform, MyObjectBuilder_CompoundCubeBlock origBlock)
		{
			MyDefinitionId defId = new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out var blockDefinition);
			if (blockDefinition != null)
			{
				MyBlockOrientation orientation = origBlock.BlockOrientation;
				Vector3I min = origBlock.Min;
				MySlimBlock.ComputeMax(blockDefinition, orientation, ref min, out var max);
				Vector3I.Transform(ref min, ref transform, out var result);
				Vector3I.Transform(ref max, ref transform, out var result2);
				origBlock.Min = Vector3I.Min(result, result2);
			}
		}

		protected override void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				m_visible = true;
				base.IsSnapped = false;
				m_pastePosition = MyBlockBuilderBase.IntersectionStart + m_dragDistance * MyBlockBuilderBase.IntersectionDirection;
				Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
				Vector3D vector3D = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix);
				m_pastePosition += vector3D;
				return;
			}
			m_visible = true;
			if (!base.IsSnapped)
			{
				m_pasteOrientationAngle = 0f;
				m_pasteDirForward = Vector3I.Forward;
				m_pasteDirUp = Vector3I.Up;
			}
			base.IsSnapped = true;
			MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
			Vector3 vector = pasteMatrix.Forward * m_dragDistance;
			if (!TrySnapToSurface(m_settings.GetGridPlacementSettings(base.PreviewGrids[0].GridSizeEnum).SnapMode))
			{
				m_pastePosition = pasteMatrix.Translation + vector;
				Matrix firstGridOrientationMatrix2 = GetFirstGridOrientationMatrix();
				Vector3D vector3D2 = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix2);
				m_pastePosition += vector3D2;
				base.IsSnapped = true;
			}
			if (!MyFakes.ENABLE_VR_BUILDING)
			{
				double num = base.PreviewGrids[0].GridSize;
				if (m_settings.StaticGridAlignToCenter)
				{
					m_pastePosition = Vector3I.Round(m_pastePosition / num) * num;
				}
				else
				{
					m_pastePosition = Vector3I.Round(m_pastePosition / num + 0.5) * num - 0.5 * num;
				}
			}
		}

		public void SetDragDistance(float dragDistance)
		{
			m_dragDistance = dragDistance;
		}

		private static double DistanceFromCharacterPlane(ref Vector3D point)
		{
			return Vector3D.Dot(point - MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionDirection);
		}

		private new bool TestPlacement()
		{
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_066f: Unknown result type (might be due to invalid IL or missing references)
			bool flag = true;
			m_touchingGrids.Clear();
			for (int i = 0; i < base.PreviewGrids.Count; i++)
			{
				MyCubeGrid myCubeGrid = base.PreviewGrids[i];
				m_touchingGrids.Add(null);
				if (MyCubeBuilder.Static.DynamicMode)
				{
					if (!m_dynamicBuildAllowed)
					{
						MyGridPlacementSettings settings = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, isStatic: false);
						BoundingBoxD localAabb = myCubeGrid.PositionComp.LocalAABB;
						MatrixD worldMatrix = myCubeGrid.WorldMatrix;
						if (MyFakes.ENABLE_VOXEL_MAP_AABB_CORNER_TEST)
						{
							flag = flag && MyCubeGrid.TestPlacementVoxelMapOverlap(null, ref settings, ref localAabb, ref worldMatrix);
						}
						flag = flag && MyCubeGrid.TestPlacementArea(myCubeGrid, targetGridIsStatic: false, ref settings, localAabb, dynamicBuildMode: true);
						if (!flag)
						{
							break;
						}
					}
				}
				else if (i == 0 && m_hitEntity is MyCubeGrid && base.IsSnapped && base.SnapMode == SnapMode.Base6Directions)
				{
					MyGridPlacementSettings settings2 = ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid);
					MyCubeGrid myCubeGrid2 = m_hitEntity as MyCubeGrid;
					if (myCubeGrid2.GridSizeEnum == MyCubeSize.Small && myCubeGrid.GridSizeEnum == MyCubeSize.Large)
					{
						flag = false;
						break;
					}
					bool flag2 = myCubeGrid2.GridSizeEnum == MyCubeSize.Large && myCubeGrid.GridSizeEnum == MyCubeSize.Small;
					if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && flag2)
					{
						if (!myCubeGrid2.IsStatic)
						{
							flag = false;
							break;
						}
						Enumerator<MySlimBlock> enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MySlimBlock current = enumerator.get_Current();
								if (current.FatBlock is MyCompoundCubeBlock)
								{
<<<<<<< HEAD
									flag = flag && TestBlockPlacement(block, ref settings2);
									if (!flag)
=======
									foreach (MySlimBlock block in (current.FatBlock as MyCompoundCubeBlock).GetBlocks())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									{
										flag = flag && TestBlockPlacement(block, ref settings2);
										if (!flag)
										{
											break;
										}
									}
								}
								else
								{
									flag = flag && TestBlockPlacement(current, ref settings2);
								}
								if (!flag)
								{
									break;
								}
							}
<<<<<<< HEAD
							else
							{
								flag = flag && TestBlockPlacement(cubeBlock, ref settings2);
							}
							if (!flag)
							{
								break;
							}
=======
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					else
					{
						flag = flag && TestGridPlacementOnGrid(myCubeGrid, ref settings2, myCubeGrid2);
					}
				}
				else
				{
					MyCubeGrid myCubeGrid3 = null;
					MyGridPlacementSettings settings3 = ((i != 0) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum) : ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid));
					if (myCubeGrid.IsStatic)
					{
						if (i == 0)
						{
							MatrixD m = myCubeGrid.WorldMatrix.GetOrientation();
							Matrix gridLocalMatrix = m;
							flag = flag && MyCubeBuilder.CheckValidBlocksRotation(gridLocalMatrix, myCubeGrid);
						}
						Enumerator<MySlimBlock> enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MySlimBlock current3 = enumerator.get_Current();
								if (current3.FatBlock is MyCompoundCubeBlock)
								{
<<<<<<< HEAD
									MyCubeGrid touchingGrid = null;
									flag = flag && TestBlockPlacementNoAABBInflate(block2, ref settings3, out touchingGrid);
									if (flag && touchingGrid != null && myCubeGrid3 == null)
=======
									foreach (MySlimBlock block2 in (current3.FatBlock as MyCompoundCubeBlock).GetBlocks())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									{
										MyCubeGrid touchingGrid = null;
										flag = flag && TestBlockPlacementNoAABBInflate(block2, ref settings3, out touchingGrid);
										if (flag && touchingGrid != null && myCubeGrid3 == null)
										{
											myCubeGrid3 = touchingGrid;
										}
										if (!flag)
										{
											break;
										}
									}
								}
								else
								{
									MyCubeGrid touchingGrid2 = null;
									flag = flag && TestBlockPlacementNoAABBInflate(current3, ref settings3, out touchingGrid2);
									if (flag && touchingGrid2 != null && myCubeGrid3 == null)
									{
										myCubeGrid3 = touchingGrid2;
									}
								}
<<<<<<< HEAD
							}
							else
							{
								MyCubeGrid touchingGrid2 = null;
								flag = flag && TestBlockPlacementNoAABBInflate(cubeBlock2, ref settings3, out touchingGrid2);
								if (flag && touchingGrid2 != null && myCubeGrid3 == null)
=======
								if (!flag)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									break;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
						}
						if (flag && myCubeGrid3 != null)
						{
							m_touchingGrids[i] = myCubeGrid3;
						}
					}
					else
					{
						Enumerator<MySlimBlock> enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
						try
						{
<<<<<<< HEAD
							Vector3 vector = cubeBlock3.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
							Vector3 vector2 = cubeBlock3.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
							BoundingBoxD localAabb2 = new BoundingBoxD(vector, vector2);
							flag = flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings3, localAabb2, dynamicBuildMode: false);
							if (!flag)
=======
							while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								MySlimBlock current5 = enumerator.get_Current();
								Vector3 vector = current5.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
								Vector3 vector2 = current5.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
								BoundingBoxD localAabb2 = new BoundingBoxD(vector, vector2);
								flag = flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings3, localAabb2, dynamicBuildMode: false);
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
						m_touchingGrids[i] = null;
					}
					if (flag && m_touchingGrids[i] != null)
					{
						MyGridPlacementSettings settings4 = ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid);
						flag = flag && TestGridPlacementOnGrid(myCubeGrid, ref settings4, m_touchingGrids[i]);
					}
					if (flag && i == 0)
					{
						if ((myCubeGrid.GridSizeEnum == MyCubeSize.Small && myCubeGrid.IsStatic) || !myCubeGrid.IsStatic)
						{
							MyGridPlacementSettings settings5 = ((i == 0) ? m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, isStatic: false) : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid);
							bool flag3 = true;
							Enumerator<MySlimBlock> enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
							try
							{
<<<<<<< HEAD
								Vector3 vector3 = cubeBlock4.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
								Vector3 vector4 = cubeBlock4.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
								BoundingBoxD localAabb3 = new BoundingBoxD(vector3, vector4);
								flag3 = flag3 && MyCubeGrid.TestPlacementArea(myCubeGrid, targetGridIsStatic: false, ref settings5, localAabb3, dynamicBuildMode: false);
								if (!flag3)
=======
								while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									MySlimBlock current6 = enumerator.get_Current();
									Vector3 vector3 = current6.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
									Vector3 vector4 = current6.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
									BoundingBoxD localAabb3 = new BoundingBoxD(vector3, vector4);
									flag3 = flag3 && MyCubeGrid.TestPlacementArea(myCubeGrid, targetGridIsStatic: false, ref settings5, localAabb3, dynamicBuildMode: false);
									if (!flag3)
									{
										break;
									}
								}
							}
<<<<<<< HEAD
=======
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							flag = flag && !flag3;
						}
						else if (m_touchingGrids[i] == null)
						{
							MyGridPlacementSettings settings6 = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, i == 0 || myCubeGrid.IsStatic);
							MyCubeGrid touchingGrid3 = null;
							bool flag4 = false;
							Enumerator<MySlimBlock> enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									MySlimBlock current7 = enumerator.get_Current();
									if (current7.FatBlock is MyCompoundCubeBlock)
									{
										foreach (MySlimBlock block3 in (current7.FatBlock as MyCompoundCubeBlock).GetBlocks())
										{
											flag4 |= TestBlockPlacementNoAABBInflate(block3, ref settings6, out touchingGrid3);
											if (flag4)
											{
												break;
											}
										}
									}
									else
									{
										flag4 |= TestBlockPlacementNoAABBInflate(current7, ref settings6, out touchingGrid3);
									}
									if (flag4)
									{
										break;
									}
								}
							}
<<<<<<< HEAD
=======
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							flag = flag && flag4;
						}
					}
				}
				BoundingBoxD boundingBoxD = myCubeGrid.PositionComp.LocalAABB;
				MatrixD worldMatrixNormalizedInv = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
				if (MySector.MainCamera != null)
				{
					Vector3D point = Vector3D.Transform(MySector.MainCamera.Position, worldMatrixNormalizedInv);
					flag = flag && boundingBoxD.Contains(point) != ContainmentType.Contains;
				}
				if (flag)
				{
					m_tmpCollisionPoints.Clear();
					MyCubeBuilder.PrepareCharacterCollisionPoints(m_tmpCollisionPoints);
					foreach (Vector3D tmpCollisionPoint in m_tmpCollisionPoints)
					{
						Vector3D point2 = Vector3D.Transform(tmpCollisionPoint, worldMatrixNormalizedInv);
						flag = flag && boundingBoxD.Contains(point2) != ContainmentType.Contains;
						if (!flag)
						{
							break;
						}
					}
				}
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		protected bool TestGridPlacementOnGrid(MyCubeGrid previewGrid, ref MyGridPlacementSettings settings, MyCubeGrid hitGrid)
		{
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			bool flag = true;
			Vector3I gridOffset = hitGrid.WorldToGridInteger(previewGrid.PositionComp.WorldMatrixRef.Translation);
			MatrixI transform = hitGrid.CalculateMergeTransform(previewGrid, gridOffset);
			Matrix floatMatrix = transform.GetFloatMatrix();
			floatMatrix.Translation *= previewGrid.GridSize;
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 60f), "First grid offset: " + gridOffset.ToString(), Color.Red, 1f);
			}
			flag = flag && MyCubeBuilder.CheckValidBlocksRotation(floatMatrix, previewGrid) && hitGrid.GridSizeEnum == previewGrid.GridSizeEnum && hitGrid.CanMergeCubes(previewGrid, gridOffset) && MyCubeGrid.CheckMergeConnectivity(hitGrid, previewGrid, gridOffset);
			if (flag)
			{
				bool flag2 = false;
				Enumerator<MySlimBlock> enumerator = previewGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current = enumerator.get_Current();
						if (current.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block in (current.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								flag2 |= CheckConnectivityOnGrid(block, ref transform, ref settings, hitGrid);
								if (flag2)
								{
									break;
								}
							}
						}
						else
						{
							flag2 |= CheckConnectivityOnGrid(current, ref transform, ref settings, hitGrid);
						}
						if (flag2)
						{
							break;
						}
					}
				}
<<<<<<< HEAD
=======
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				flag = flag && flag2;
			}
			if (flag)
			{
				Enumerator<MySlimBlock> enumerator = previewGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current3 = enumerator.get_Current();
						if (current3.FatBlock is MyCompoundCubeBlock)
						{
<<<<<<< HEAD
							flag = flag && TestBlockPlacementOnGrid(block2, ref transform, ref settings, hitGrid);
							if (!flag)
=======
							foreach (MySlimBlock block2 in (current3.FatBlock as MyCompoundCubeBlock).GetBlocks())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								flag = flag && TestBlockPlacementOnGrid(block2, ref transform, ref settings, hitGrid);
								if (!flag)
								{
									break;
								}
							}
						}
						else
						{
							flag = flag && TestBlockPlacementOnGrid(current3, ref transform, ref settings, hitGrid);
						}
						if (!flag)
						{
							return flag;
						}
					}
<<<<<<< HEAD
					else
					{
						flag = flag && TestBlockPlacementOnGrid(cubeBlock2, ref transform, ref settings, hitGrid);
					}
					if (!flag)
					{
						return flag;
					}
=======
					return flag;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return flag;
		}

		protected static bool CheckConnectivityOnGrid(MySlimBlock block, ref MatrixI transform, ref MyGridPlacementSettings settings, MyCubeGrid hitGrid)
		{
			Vector3I.Transform(ref block.Position, ref transform, out var result);
			Base6Directions.Direction direction = transform.GetDirection(block.Orientation.Forward);
			Base6Directions.Direction direction2 = transform.GetDirection(block.Orientation.Up);
			new MyBlockOrientation(direction, direction2).GetQuaternion(out var result2);
			MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
			return MyCubeGrid.CheckConnectivity(hitGrid, blockDefinition, blockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio), ref result2, ref result);
		}

		protected static bool TestBlockPlacementOnGrid(MySlimBlock block, ref MatrixI transform, ref MyGridPlacementSettings settings, MyCubeGrid hitGrid)
		{
			Vector3I.Transform(ref block.Min, ref transform, out var result);
			Vector3I.Transform(ref block.Max, ref transform, out var result2);
			Vector3I min = Vector3I.Min(result, result2);
			Vector3I max = Vector3I.Max(result, result2);
			Base6Directions.Direction direction = transform.GetDirection(block.Orientation.Forward);
			Base6Directions.Direction direction2 = transform.GetDirection(block.Orientation.Up);
			MyBlockOrientation orientation = new MyBlockOrientation(direction, direction2);
			return hitGrid.CanPlaceBlock(min, max, orientation, block.BlockDefinition, ref settings, 0uL);
		}

		protected static bool TestBlockPlacement(MySlimBlock block, ref MyGridPlacementSettings settings)
		{
			return MyCubeGrid.TestPlacementAreaCube(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, 0uL, block.CubeGrid);
		}

		protected static bool TestBlockPlacement(MySlimBlock block, ref MyGridPlacementSettings settings, out MyCubeGrid touchingGrid)
		{
			return MyCubeGrid.TestPlacementAreaCube(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, 0uL, block.CubeGrid);
		}

		protected static bool TestBlockPlacementNoAABBInflate(MySlimBlock block, ref MyGridPlacementSettings settings, out MyCubeGrid touchingGrid)
		{
			return MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, 0uL, block.CubeGrid);
		}

		protected static bool TestVoxelPlacement(MySlimBlock block, ref MyGridPlacementSettings settings, bool dynamicMode)
		{
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f);
			localAabb.Include(block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			return MyCubeGrid.TestVoxelPlacement(block.BlockDefinition, settings, dynamicMode, block.CubeGrid.WorldMatrix, localAabb);
		}

		protected static bool TestBlockPlacementArea(MySlimBlock block, ref MyGridPlacementSettings settings, bool dynamicMode, bool testVoxel = true)
		{
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f);
			localAabb.Include(block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			return MyCubeGrid.TestBlockPlacementArea(block.BlockDefinition, block.Orientation, block.CubeGrid.WorldMatrix, ref settings, localAabb, dynamicMode, block.CubeGrid, testVoxel);
		}

		private void UpdatePreview()
		{
			if (base.PreviewGrids == null || !m_visible || !HasPreviewBBox)
			{
				return;
			}
			MyStringId value = (m_canBePlaced ? MyGridClipboard.ID_GIZMO_DRAW_LINE : MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
			if (MyFakes.ENABLE_VR_BUILDING && m_canBePlaced)
			{
				return;
			}
			Color color = Color.White;
			foreach (MyCubeGrid previewGrid in base.PreviewGrids)
			{
				BoundingBoxD localbox = previewGrid.PositionComp.LocalAABB;
				MatrixD worldMatrix = previewGrid.PositionComp.WorldMatrixRef;
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value);
			}
		}

		internal void DynamicModeChanged()
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				SetupDragDistance();
			}
		}

		protected virtual void SetupDragDistance()
		{
			if (!base.IsActive)
			{
				return;
			}
			if (base.PreviewGrids.Count > 0)
			{
				double? currentRayIntersection = MyCubeBuilder.GetCurrentRayIntersection();
				if (currentRayIntersection.HasValue && (double)m_dragDistance > currentRayIntersection.Value)
				{
					m_dragDistance = (float)currentRayIntersection.Value;
				}
				float num = (float)base.PreviewGrids[0].PositionComp.WorldAABB.HalfExtents.Length();
				float num2 = 2.5f * num;
				if (m_dragDistance < num2)
				{
					m_dragDistance = num2;
				}
			}
			else
			{
				m_dragDistance = 0f;
			}
		}

		public override void MoveEntityCloser()
		{
			base.MoveEntityCloser();
			if (m_dragDistance < MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
			{
				m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
			}
		}
	}
}
