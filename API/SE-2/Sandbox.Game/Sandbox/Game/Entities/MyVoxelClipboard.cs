using System;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	internal class MyVoxelClipboard
	{
		private List<MyObjectBuilder_EntityBase> m_copiedVoxelMaps = new List<MyObjectBuilder_EntityBase>();

		private List<IMyStorage> m_copiedStorages = new List<IMyStorage>();

		private List<Vector3> m_copiedVoxelMapOffsets = new List<Vector3>();

		private List<MyVoxelBase> m_previewVoxelMaps = new List<MyVoxelBase>();

		protected float m_pasteOrientationAngle;

		protected Vector3 m_pasteDirUp = new Vector3(1f, 0f, 0f);

		protected Vector3 m_pasteDirForward = new Vector3(0f, 1f, 0f);

		private Vector3D m_pastePosition;

		private float m_dragDistance;

		private Vector3 m_dragPointToPositionLocal;

		private bool m_canBePlaced;

		private bool m_visible = true;

		private bool m_shouldMarkForClose = true;

		private bool m_planetMode;

		private static readonly MyStringId ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");

		private List<MyEntity> m_tmpResultList = new List<MyEntity>();

		public bool IsActive { get; private set; }

		private void Activate()
		{
			ChangeClipboardPreview(visible: true);
			IsActive = true;
		}

		public void Deactivate()
		{
			ChangeClipboardPreview(visible: false);
			IsActive = false;
			m_planetMode = false;
		}

		public void Hide()
		{
			ChangeClipboardPreview(visible: false);
			m_planetMode = false;
		}

		public void Show()
		{
			if (IsActive && m_previewVoxelMaps.Count == 0)
			{
				ChangeClipboardPreview(visible: true);
			}
		}

		public void ClearClipboard()
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedVoxelMapOffsets.Clear();
			m_copiedVoxelMaps.Clear();
		}

		public void CutVoxelMap(MyVoxelBase voxelMap)
		{
			if (voxelMap != null)
			{
				CopyVoxelMap(voxelMap);
				MyEntities.SendCloseRequest(voxelMap);
				Deactivate();
			}
		}

		public void CopyVoxelMap(MyVoxelBase voxelMap)
		{
			if (voxelMap != null)
			{
				m_copiedVoxelMaps.Clear();
				m_copiedVoxelMapOffsets.Clear();
				CopyVoxelMapInternal(voxelMap);
				Activate();
			}
		}

		private void CopyVoxelMapInternal(MyVoxelBase toCopy)
		{
			m_copiedVoxelMaps.Add(toCopy.GetObjectBuilder(copy: true));
			if (m_copiedVoxelMaps.Count == 1)
			{
				MatrixD pasteMatrix = GetPasteMatrix();
				Vector3D translation = toCopy.WorldMatrix.Translation;
				m_dragPointToPositionLocal = Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - translation, toCopy.PositionComp.WorldMatrixNormalizedInv);
				m_dragDistance = (float)(translation - pasteMatrix.Translation).Length();
				m_pasteDirUp = toCopy.WorldMatrix.Up;
				m_pasteDirForward = toCopy.WorldMatrix.Forward;
				m_pasteOrientationAngle = 0f;
			}
			m_copiedVoxelMapOffsets.Add(toCopy.WorldMatrix.Translation - m_copiedVoxelMaps[0].PositionAndOrientation.Value.Position);
		}

		public bool PasteVoxelMap(MyInventory buildInventory = null)
		{
			if (m_planetMode)
			{
				if (!m_canBePlaced)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteAsteoridObstructed);
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					return false;
				}
				MyEntities.RemapObjectBuilderCollection(m_copiedVoxelMaps);
				for (int i = 0; i < m_copiedVoxelMaps.Count; i++)
				{
					MyGuiScreenDebugSpawnMenu.SpawnPlanet(m_pastePosition - m_copiedVoxelMapOffsets[i]);
				}
				Deactivate();
				return true;
			}
			if (m_copiedVoxelMaps.Count == 0)
			{
				return false;
			}
			if (m_copiedVoxelMaps.Count > 0 && !IsActive)
			{
				Activate();
				return true;
			}
			if (!m_canBePlaced)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteAsteoridObstructed);
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
			MyGuiScreenDebugSpawnMenu.RecreateAsteroidBeforePaste((float)m_previewVoxelMaps[0].PositionComp.GetPosition().Length());
			MyEntities.RemapObjectBuilderCollection(m_copiedVoxelMaps);
			foreach (MyVoxelBase previewVoxelMap in m_previewVoxelMaps)
			{
				if (Sync.IsServer)
				{
					previewVoxelMap.CreatedByUser = true;
					previewVoxelMap.AsteroidName = MyGuiScreenDebugSpawnMenu.GetAsteroidName();
					EnablePhysicsRecursively(previewVoxelMap);
					previewVoxelMap.Save = true;
					MakeVisible(previewVoxelMap);
					m_shouldMarkForClose = false;
					MyEntities.RaiseEntityCreated(previewVoxelMap);
				}
				else
				{
					m_shouldMarkForClose = true;
<<<<<<< HEAD
					MyGuiScreenDebugSpawnMenu.SpawnAsteroid(previewVoxelMap.PositionComp.WorldMatrixRef);
=======
					MyGuiScreenDebugSpawnMenu.SpawnAsteroid(previewVoxelMap.PositionComp.WorldMatrix);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				previewVoxelMap.AfterPaste();
			}
			Deactivate();
			return true;
		}

		public void SetVoxelMapFromBuilder(MyObjectBuilder_EntityBase voxelMap, IMyStorage storage, Vector3 dragPointDelta, float dragVectorLength)
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedVoxelMaps.Clear();
			m_copiedVoxelMapOffsets.Clear();
			m_copiedStorages.Clear();
			m_dragPointToPositionLocal = dragPointDelta;
			m_dragDistance = dragVectorLength;
			Vector3 offset = Vector3.Zero;
			if (voxelMap is MyObjectBuilder_Planet)
			{
				offset = storage.Size / 2f;
			}
			SetVoxelMapFromBuilderInternal(voxelMap, storage, offset);
			Activate();
		}

		private void SetVoxelMapFromBuilderInternal(MyObjectBuilder_EntityBase voxelMap, IMyStorage storage, Vector3 offset)
		{
			m_copiedVoxelMaps.Add(voxelMap);
			m_copiedStorages.Add(storage);
			m_copiedVoxelMapOffsets.Add(offset);
		}

		private void ChangeClipboardPreview(bool visible)
		{
			if (m_copiedVoxelMaps.Count == 0 || !visible)
			{
				foreach (MyVoxelBase previewVoxelMap in m_previewVoxelMaps)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewVoxelMap, enable: false);
					if (m_shouldMarkForClose)
					{
						previewVoxelMap.Close();
					}
				}
				m_previewVoxelMaps.Clear();
				m_visible = false;
				return;
			}
			MyEntities.RemapObjectBuilderCollection(m_copiedVoxelMaps);
			for (int i = 0; i < m_copiedVoxelMaps.Count; i++)
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = m_copiedVoxelMaps[i];
				IMyStorage storage = m_copiedStorages[i];
				MyVoxelBase myVoxelBase = null;
				if (myObjectBuilder_EntityBase is MyObjectBuilder_VoxelMap)
				{
					myVoxelBase = new MyVoxelMap();
				}
				if (myVoxelBase != null)
				{
					if (myObjectBuilder_EntityBase is MyObjectBuilder_Planet)
					{
						m_planetMode = true;
						IsActive = visible;
						m_visible = visible;
						continue;
					}
					_ = myObjectBuilder_EntityBase.PositionAndOrientation.Value;
					myVoxelBase.Init(myObjectBuilder_EntityBase, storage);
					myVoxelBase.BeforePaste();
					DisablePhysicsRecursively(myVoxelBase);
					MakeTransparent(myVoxelBase);
					MyEntities.Add(myVoxelBase);
					myVoxelBase.PositionLeftBottomCorner = m_pastePosition - myVoxelBase.Storage.Size * 0.5f;
					MatrixD worldMatrix = GetFirstOrientationMatrix();
					myVoxelBase.PositionComp.SetWorldMatrix(ref worldMatrix);
					myVoxelBase.PositionComp.SetPosition(m_pastePosition);
					myVoxelBase.Save = false;
					m_previewVoxelMaps.Add(myVoxelBase);
					IsActive = visible;
					m_visible = visible;
					m_shouldMarkForClose = true;
				}
<<<<<<< HEAD
=======
				_ = myObjectBuilder_EntityBase.PositionAndOrientation.Value;
				myVoxelBase.Init(myObjectBuilder_EntityBase, storage);
				myVoxelBase.BeforePaste();
				DisablePhysicsRecursively(myVoxelBase);
				MakeTransparent(myVoxelBase);
				MyEntities.Add(myVoxelBase);
				myVoxelBase.PositionLeftBottomCorner = m_pastePosition - myVoxelBase.Storage.Size * 0.5f;
				myVoxelBase.PositionComp.WorldMatrix = GetFirstOrientationMatrix();
				myVoxelBase.PositionComp.SetPosition(m_pastePosition);
				myVoxelBase.Save = false;
				m_previewVoxelMaps.Add(myVoxelBase);
				IsActive = visible;
				m_visible = visible;
				m_shouldMarkForClose = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void MakeTransparent(MyVoxelBase voxelMap)
		{
			voxelMap.Render.Transparency = 0.25f;
		}

		private void MakeVisible(MyVoxelBase voxelMap)
		{
			voxelMap.Render.Transparency = 0f;
			voxelMap.Render.UpdateTransparency();
		}

		private void DisablePhysicsRecursively(MyEntity entity)
		{
			if (entity.Physics != null && entity.Physics.Enabled)
			{
				entity.Physics.Enabled = false;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				DisablePhysicsRecursively(child.Container.Entity as MyEntity);
			}
		}

		private void EnablePhysicsRecursively(MyEntity entity)
		{
			if (entity.Physics != null && !entity.Physics.Enabled)
			{
				entity.Physics.Enabled = true;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				EnablePhysicsRecursively(child.Container.Entity as MyEntity);
			}
		}

		public void Update()
		{
			if (IsActive && m_visible)
			{
				UpdatePastePosition();
				UpdateVoxelMapTransformations();
				m_canBePlaced = TestPlacement();
			}
		}

		private void UpdateVoxelMapTransformations()
		{
			Color color = (m_canBePlaced ? Color.Green : Color.Red);
			if (m_planetMode)
			{
				for (int i = 0; i < m_copiedVoxelMaps.Count; i++)
				{
					MyObjectBuilder_Planet myObjectBuilder_Planet = m_copiedVoxelMaps[i] as MyObjectBuilder_Planet;
					if (myObjectBuilder_Planet != null)
					{
						MyRenderProxy.DebugDrawSphere(m_pastePosition, myObjectBuilder_Planet.Radius * 1.1f, color);
					}
				}
				return;
			}
			for (int j = 0; j < m_previewVoxelMaps.Count; j++)
			{
				if (m_previewVoxelMaps[j] != null && m_previewVoxelMaps[j].PositionComp != null)
				{
<<<<<<< HEAD
					MatrixD worldMatrix = GetFirstOrientationMatrix();
					m_previewVoxelMaps[j].PositionComp.SetWorldMatrix(ref worldMatrix);
					m_previewVoxelMaps[j].PositionLeftBottomCorner = m_pastePosition + m_copiedVoxelMapOffsets[j] - m_previewVoxelMaps[j].Storage.Size * 0.5f;
					m_previewVoxelMaps[j].PositionComp.SetPosition(m_pastePosition + m_copiedVoxelMapOffsets[j]);
					MatrixD worldMatrix2 = m_previewVoxelMaps[j].PositionComp.WorldMatrixRef;
					BoundingBoxD localbox = new BoundingBoxD(-m_previewVoxelMaps[j].Storage.Size * 0.5f, m_previewVoxelMaps[j].Storage.Size * 0.5f);
					MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix2, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, ID_GIZMO_DRAW_LINE, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
=======
					m_previewVoxelMaps[j].PositionComp.WorldMatrix = GetFirstOrientationMatrix();
					m_previewVoxelMaps[j].PositionLeftBottomCorner = m_pastePosition + m_copiedVoxelMapOffsets[j] - m_previewVoxelMaps[j].Storage.Size * 0.5f;
					m_previewVoxelMaps[j].PositionComp.SetPosition(m_pastePosition + m_copiedVoxelMapOffsets[j]);
					MatrixD worldMatrix = m_previewVoxelMaps[j].PositionComp.WorldMatrixRef;
					BoundingBoxD localbox = new BoundingBoxD(-m_previewVoxelMaps[j].Storage.Size * 0.5f, m_previewVoxelMaps[j].Storage.Size * 0.5f);
					MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, ID_GIZMO_DRAW_LINE, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void UpdatePastePosition()
		{
			MatrixD pasteMatrix = GetPasteMatrix();
			Vector3D vector3D = pasteMatrix.Forward * m_dragDistance;
			m_pastePosition = pasteMatrix.Translation + vector3D;
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawSphere(m_pastePosition, 0.15f, Color.Pink.ToVector3(), 1f, depthRead: false);
			}
		}

		private bool TestPlacement()
		{
			if (!MyEntities.IsInsideWorld(m_pastePosition))
			{
				return false;
			}
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator))
			{
				for (int i = 0; i < m_previewVoxelMaps.Count; i++)
				{
					if (!MyEntities.IsInsideWorld(m_previewVoxelMaps[i].PositionComp.GetPosition()))
					{
						return false;
					}
					BoundingBoxD box = m_previewVoxelMaps[i].PositionComp.WorldAABB;
					using (m_tmpResultList.GetClearToken())
					{
						MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, m_tmpResultList);
						if (!TestPlacement(m_tmpResultList))
						{
							return false;
						}
					}
				}
				if (m_planetMode)
				{
					for (int j = 0; j < m_copiedVoxelMaps.Count; j++)
					{
						MyObjectBuilder_Planet myObjectBuilder_Planet = m_copiedVoxelMaps[j] as MyObjectBuilder_Planet;
						if (myObjectBuilder_Planet == null)
<<<<<<< HEAD
						{
							continue;
						}
						using (m_tmpResultList.GetClearToken())
						{
=======
						{
							continue;
						}
						using (m_tmpResultList.GetClearToken())
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							BoundingSphereD sphere = new BoundingSphereD(m_pastePosition, myObjectBuilder_Planet.Radius * 1.1f);
							MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_tmpResultList);
							if (!TestPlacement(m_tmpResultList))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		private bool TestPlacement(List<MyEntity> entities)
		{
			foreach (MyEntity entity in entities)
			{
				if (!(entity is MyVoxelBase) && (!(entity is MyCubeGrid) || !(entity as MyCubeGrid).IsStatic))
				{
					entities.Clear();
					return false;
				}
			}
			return true;
		}

		private static MatrixD GetPasteMatrix()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
			}
			return MySector.MainCamera.WorldMatrix;
		}

		public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
		{
			MyVoxelBase myVoxelBase = ((m_previewVoxelMaps.Count > 0) ? m_previewVoxelMaps[0] : null);
			if (myVoxelBase != null)
			{
				MatrixD worldMatrix = myVoxelBase.WorldMatrix;
				Vector3D vector3D = Vector3D.TransformNormal(-m_dragPointToPositionLocal, worldMatrix);
				worldMatrix.Translation += vector3D;
				hints.CalculateRotationHints(worldMatrix, !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && !MyInput.Static.IsJoystickLastUsed, isRotating);
			}
		}

		public MatrixD GetFirstOrientationMatrix()
		{
			return MatrixD.CreateWorld(Vector3D.Zero, m_pasteDirForward, m_pasteDirUp);
		}

		public void RotateAroundAxis(int axisIndex, int sign, bool newlyPressed, float angleDelta)
		{
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
			ApplyOrientationAngle();
			Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
			float num = (float)Math.Cos(angle);
			float num2 = (float)Math.Sin(angle);
			Vector3 pasteDirUp = m_pasteDirUp * num - m_pasteDirForward * num2;
			m_pasteDirForward = m_pasteDirUp * num2 + m_pasteDirForward * num;
			m_pasteDirUp = pasteDirUp;
		}

		private void UpMinus(float angle)
		{
			UpPlus(0f - angle);
		}

		private void RightPlus(float angle)
		{
			ApplyOrientationAngle();
			Vector3 vector = Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
			float num = (float)Math.Cos(angle);
			float num2 = (float)Math.Sin(angle);
			m_pasteDirUp = m_pasteDirUp * num + vector * num2;
		}

		private void RightMinus(float angle)
		{
			RightPlus(0f - angle);
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

		public void MoveEntityFurther()
		{
			m_dragDistance *= 1.1f;
		}

		public void MoveEntityCloser()
		{
			m_dragDistance /= 1.1f;
		}
	}
}
