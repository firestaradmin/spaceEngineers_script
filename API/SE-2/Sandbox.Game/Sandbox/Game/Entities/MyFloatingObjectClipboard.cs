using System;
using System.Collections.Generic;
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	internal class MyFloatingObjectClipboard
	{
		private List<MyObjectBuilder_FloatingObject> m_copiedFloatingObjects = new List<MyObjectBuilder_FloatingObject>();

		private List<Vector3D> m_copiedFloatingObjectOffsets = new List<Vector3D>();

		private List<MyFloatingObject> m_previewFloatingObjects = new List<MyFloatingObject>();

		private Vector3D m_pastePosition;

		private Vector3D m_pastePositionPrevious;

		private bool m_calculateVelocity = true;

		private Vector3 m_objectVelocity = Vector3.Zero;

		private float m_pasteOrientationAngle;

		private Vector3 m_pasteDirUp = new Vector3(1f, 0f, 0f);

		private Vector3 m_pasteDirForward = new Vector3(0f, 1f, 0f);

		private float m_dragDistance;

		private Vector3D m_dragPointToPositionLocal;

		private bool m_canBePlaced;

		private List<MyPhysics.HitInfo> m_raycastCollisionResults = new List<MyPhysics.HitInfo>();

		private float m_closestHitDistSq = float.MaxValue;

		private Vector3D m_hitPos = new Vector3(0f, 0f, 0f);

		private Vector3 m_hitNormal = new Vector3(1f, 0f, 0f);

		private bool m_visible = true;

		private bool m_enableStationRotation;

		public bool IsActive { get; private set; }

		public List<MyFloatingObject> PreviewFloatingObjects => m_previewFloatingObjects;

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
				m_enableStationRotation = value;
			}
		}

		public string CopiedGridsName
		{
			get
			{
				if (HasCopiedFloatingObjects())
				{
					return m_copiedFloatingObjects[0].Name;
				}
				return null;
			}
		}

		public MyFloatingObjectClipboard(bool calculateVelocity = true)
		{
			m_calculateVelocity = calculateVelocity;
		}

		private void Activate()
		{
			ChangeClipboardPreview(visible: true);
			IsActive = true;
		}

		public void Deactivate()
		{
			ChangeClipboardPreview(visible: false);
			IsActive = false;
		}

		public void Hide()
		{
			ChangeClipboardPreview(visible: false);
		}

		public void Show()
		{
			if (IsActive && m_previewFloatingObjects.Count == 0)
			{
				ChangeClipboardPreview(visible: true);
			}
		}

		public void CutFloatingObject(MyFloatingObject floatingObject)
		{
			if (floatingObject != null)
			{
				CopyfloatingObject(floatingObject);
				DeleteFloatingObject(floatingObject);
			}
		}

		public void DeleteFloatingObject(MyFloatingObject floatingObject)
		{
			if (floatingObject != null)
			{
				MyFloatingObjects.RemoveFloatingObject(floatingObject, sync: true);
				Deactivate();
			}
		}

		public void CopyfloatingObject(MyFloatingObject floatingObject)
		{
			if (floatingObject != null)
			{
				m_copiedFloatingObjects.Clear();
				m_copiedFloatingObjectOffsets.Clear();
				CopyFloatingObjectInternal(floatingObject);
				Activate();
			}
		}

		private void CopyFloatingObjectInternal(MyFloatingObject toCopy)
		{
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = (MyObjectBuilder_FloatingObject)toCopy.GetObjectBuilder(copy: true);
			myObjectBuilder_FloatingObject.EntityId = 0L;
			myObjectBuilder_FloatingObject.Name = null;
			m_copiedFloatingObjects.Add(myObjectBuilder_FloatingObject);
			if (m_copiedFloatingObjects.Count == 1)
			{
				MatrixD pasteMatrix = GetPasteMatrix();
				Vector3D translation = toCopy.WorldMatrix.Translation;
				m_dragPointToPositionLocal = Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - translation, toCopy.PositionComp.WorldMatrixNormalizedInv);
				m_dragDistance = (float)(translation - pasteMatrix.Translation).Length();
				m_pasteDirUp = toCopy.WorldMatrix.Up;
				m_pasteDirForward = toCopy.WorldMatrix.Forward;
				m_pasteOrientationAngle = 0f;
			}
			m_copiedFloatingObjectOffsets.Add(toCopy.WorldMatrix.Translation - m_copiedFloatingObjects[0].PositionAndOrientation.Value.Position);
		}

		public bool PasteFloatingObject(MyInventory buildInventory = null)
		{
			if (m_copiedFloatingObjects.Count == 0)
			{
				return false;
			}
			if (m_copiedFloatingObjects.Count > 0 && !IsActive)
			{
				if (CheckPastedFloatingObjects())
				{
					Activate();
				}
				else
				{
					MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteBlockNotAvailable);
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				}
				return true;
			}
			if (!m_canBePlaced)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceItem);
			MyEntities.RemapObjectBuilderCollection(m_copiedFloatingObjects);
			bool result = false;
			int num = 0;
			foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in m_copiedFloatingObjects)
			{
				copiedFloatingObject.PersistentFlags = MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
				copiedFloatingObject.PositionAndOrientation = new MyPositionAndOrientation(m_previewFloatingObjects[num].WorldMatrix);
				num++;
				MyFloatingObjects.RequestSpawnCreative(copiedFloatingObject);
				result = true;
			}
			Deactivate();
			return result;
		}

		/// <summary>
		/// Checks the pasted object builder for non-existent blocks (e.g. copying from world with a cube block mod to a world without it)
		/// </summary>
		/// <returns>True when the grid can be pasted</returns>
		private bool CheckPastedFloatingObjects()
		{
			foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in m_copiedFloatingObjects)
			{
				MyDefinitionId id = copiedFloatingObject.Item.PhysicalContent.GetId();
				if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(id, out var _))
				{
					return false;
				}
			}
			return true;
		}

		public void SetFloatingObjectFromBuilder(MyObjectBuilder_FloatingObject floatingObject, Vector3D dragPointDelta, float dragVectorLength)
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedFloatingObjects.Clear();
			m_copiedFloatingObjectOffsets.Clear();
			MatrixD m = GetPasteMatrix();
			_ = (Matrix)m;
			m_dragPointToPositionLocal = dragPointDelta;
			m_dragDistance = dragVectorLength;
			MyPositionAndOrientation myPositionAndOrientation = floatingObject.PositionAndOrientation ?? MyPositionAndOrientation.Default;
			m_pasteDirUp = myPositionAndOrientation.Up;
			m_pasteDirForward = myPositionAndOrientation.Forward;
			SetFloatingObjectFromBuilderInternal(floatingObject, Vector3D.Zero);
			Activate();
		}

		private void SetFloatingObjectFromBuilderInternal(MyObjectBuilder_FloatingObject floatingObject, Vector3D offset)
		{
			m_copiedFloatingObjects.Add(floatingObject);
			m_copiedFloatingObjectOffsets.Add(offset);
		}

		private void ChangeClipboardPreview(bool visible)
		{
			if (m_copiedFloatingObjects.Count == 0 || !visible)
			{
				foreach (MyFloatingObject previewFloatingObject in m_previewFloatingObjects)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject, enable: false);
					previewFloatingObject.Close();
				}
				m_previewFloatingObjects.Clear();
				m_visible = false;
				return;
			}
			MyEntities.RemapObjectBuilderCollection(m_copiedFloatingObjects);
			foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in m_copiedFloatingObjects)
			{
				MyFloatingObject myFloatingObject = MyEntities.CreateFromObjectBuilder(copiedFloatingObject, fadeIn: false) as MyFloatingObject;
				if (myFloatingObject == null)
				{
					ChangeClipboardPreview(visible: false);
					break;
				}
				MakeTransparent(myFloatingObject);
				IsActive = visible;
				m_visible = visible;
				MyEntities.Add(myFloatingObject);
				MyFloatingObjects.UnregisterFloatingObject(myFloatingObject);
				myFloatingObject.Save = false;
				myFloatingObject.IsPreview = true;
				DisablePhysicsRecursively(myFloatingObject);
				m_previewFloatingObjects.Add(myFloatingObject);
			}
		}

		private void MakeTransparent(MyFloatingObject floatingObject)
		{
			floatingObject.Render.Transparency = 0.5f;
			if (floatingObject.Subparts.TryGetValue(MyAutomaticRifleGun.NAME_SUBPART_MAGAZINE, out var value) && value.Render != null)
			{
				value.Render.Transparency = 0.5f;
			}
		}

		private void DisablePhysicsRecursively(MyEntity entity)
		{
			if (entity.Physics != null && entity.Physics.Enabled)
			{
				entity.Physics.Enabled = false;
			}
			MyFloatingObject myFloatingObject = entity as MyFloatingObject;
			if (myFloatingObject != null)
			{
				myFloatingObject.NeedsUpdate = MyEntityUpdateEnum.NONE;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				DisablePhysicsRecursively(child.Container.Entity as MyEntity);
			}
		}

		public void Update()
		{
			if (IsActive && m_visible)
			{
				UpdateHitEntity();
				UpdatePastePosition();
				UpdateFloatingObjectTransformations();
				if (m_calculateVelocity)
				{
					m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
				}
				m_canBePlaced = TestPlacement();
				UpdatePreviewBBox();
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
				}
			}
		}

		private void UpdateHitEntity()
		{
			MatrixD pasteMatrix = GetPasteMatrix();
			MyPhysics.CastRay(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * m_dragDistance, m_raycastCollisionResults);
			m_closestHitDistSq = float.MaxValue;
			m_hitPos = new Vector3D(0.0, 0.0, 0.0);
			m_hitNormal = new Vector3(1f, 0f, 0f);
			foreach (MyPhysics.HitInfo raycastCollisionResult in m_raycastCollisionResults)
			{
				MyPhysicsBody myPhysicsBody = (MyPhysicsBody)raycastCollisionResult.HkHitInfo.Body.UserObject;
				if (myPhysicsBody == null)
				{
					continue;
				}
				IMyEntity entity = myPhysicsBody.Entity;
				if (entity is MyVoxelMap || (entity is MyCubeGrid && entity.EntityId != m_previewFloatingObjects[0].EntityId))
				{
					float num = (float)(raycastCollisionResult.Position - pasteMatrix.Translation).LengthSquared();
					if (num < m_closestHitDistSq)
					{
						m_closestHitDistSq = num;
						m_hitPos = raycastCollisionResult.Position;
						m_hitNormal = raycastCollisionResult.HkHitInfo.Normal;
					}
				}
			}
			m_raycastCollisionResults.Clear();
		}

		private bool TestPlacement()
		{
			if (m_previewFloatingObjects == null)
			{
				return false;
			}
			for (int i = 0; i < m_previewFloatingObjects.Count; i++)
			{
				MyFloatingObject myFloatingObject = m_previewFloatingObjects[i];
				if (myFloatingObject != null && (!MyEntities.IsInsideWorld(myFloatingObject.PositionComp.GetPosition()) || myFloatingObject.Physics == null))
				{
					return false;
				}
			}
			for (int j = 0; j < m_previewFloatingObjects.Count; j++)
			{
				MyFloatingObject myFloatingObject2 = m_previewFloatingObjects[j];
				if (myFloatingObject2 == null)
				{
					continue;
				}
				MatrixD matrix = myFloatingObject2.WorldMatrix;
				Quaternion rotation = Quaternion.CreateFromRotationMatrix(in matrix);
				Vector3D translation = myFloatingObject2.PositionComp.GetPosition() + Vector3D.Transform(myFloatingObject2.PositionComp.LocalVolume.Center, rotation);
				List<HkBodyCollision> list = new List<HkBodyCollision>();
				MyPhysics.GetPenetrationsShape(myFloatingObject2.Physics.RigidBody.GetShape(), ref translation, ref rotation, list, 23);
				foreach (HkBodyCollision item in list)
				{
					IMyEntity collisionEntity = item.GetCollisionEntity();
					if (collisionEntity != null && !collisionEntity.Closed && (collisionEntity.Physics == null || !collisionEntity.Physics.IsPhantom))
					{
						return false;
					}
				}
			}
			return true;
		}

		private void UpdateFloatingObjectTransformations()
		{
			Matrix m = GetFirstGridOrientationMatrix();
			MatrixD matrixD = m;
			MatrixD matrixD2 = MatrixD.Invert(m_copiedFloatingObjects[0].PositionAndOrientation.Value.GetMatrix()).GetOrientation() * matrixD;
			for (int i = 0; i < m_previewFloatingObjects.Count; i++)
			{
				MatrixD matrix = m_copiedFloatingObjects[i].PositionAndOrientation.Value.GetMatrix();
				Vector3D normal = matrix.Translation - m_copiedFloatingObjects[0].PositionAndOrientation.Value.Position;
				m_copiedFloatingObjectOffsets[i] = Vector3D.TransformNormal(normal, matrixD2);
				Vector3D translation = m_pastePosition + m_copiedFloatingObjectOffsets[i];
				matrix *= matrixD2;
				matrix.Translation = Vector3D.Zero;
				matrix = MatrixD.Orthogonalize(matrix);
				matrix.Translation = translation;
				m_previewFloatingObjects[i].PositionComp.SetWorldMatrix(ref matrix);
			}
		}

		private void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			MatrixD pasteMatrix = GetPasteMatrix();
			Vector3D vector3D = pasteMatrix.Forward * m_dragDistance;
			m_pastePosition = pasteMatrix.Translation + vector3D;
			Matrix m = GetFirstGridOrientationMatrix();
			MatrixD matrix = m;
			m_pastePosition += Vector3D.TransformNormal(m_dragPointToPositionLocal, matrix);
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawSphere(pasteMatrix.Translation + vector3D, 0.15f, Color.Pink, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(m_pastePosition, 0.15f, Color.Pink.ToVector3(), 1f, depthRead: false);
			}
		}

		private static MatrixD GetPasteMatrix()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
			}
			return MySector.MainCamera.WorldMatrix;
		}

		private Matrix GetFirstGridOrientationMatrix()
		{
			return Matrix.CreateWorld(Vector3.Zero, m_pasteDirForward, m_pasteDirUp) * Matrix.CreateFromAxisAngle(m_pasteDirUp, m_pasteOrientationAngle);
		}

		private void UpdatePreviewBBox()
		{
			if (m_previewFloatingObjects == null)
			{
				return;
			}
			if (!m_visible)
			{
				foreach (MyFloatingObject previewFloatingObject in m_previewFloatingObjects)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject, enable: false);
				}
				return;
			}
			Vector4 value = new Vector4(Color.Red.ToVector3() * 0.8f, 1f);
			if (m_canBePlaced)
			{
				value = Color.Gray.ToVector4();
			}
			Vector3 value2 = new Vector3(0.1f);
			foreach (MyFloatingObject previewFloatingObject2 in m_previewFloatingObjects)
			{
				MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject2, enable: true, value, 0.01f, value2);
			}
		}

		public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
		{
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = ((m_copiedFloatingObjects.Count > 0) ? m_copiedFloatingObjects[0] : null);
			if (myObjectBuilder_FloatingObject != null)
			{
				MatrixD matrix = myObjectBuilder_FloatingObject.PositionAndOrientation.Value.GetMatrix();
				Vector3D vector3D = Vector3D.TransformNormal(-m_dragPointToPositionLocal, matrix);
				matrix.Translation += vector3D;
				hints.CalculateRotationHints(matrix, !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && !MyInput.Static.IsJoystickLastUsed, isRotating);
			}
		}

		public bool HasCopiedFloatingObjects()
		{
			return m_copiedFloatingObjects.Count > 0;
		}

		public void HideWhenColliding(List<Vector3D> collisionTestPoints)
		{
			if (m_previewFloatingObjects.Count == 0)
			{
				return;
			}
			bool flag = true;
			foreach (Vector3D collisionTestPoint in collisionTestPoints)
			{
				foreach (MyFloatingObject previewFloatingObject in m_previewFloatingObjects)
				{
					Vector3D point = Vector3.Transform(collisionTestPoint, previewFloatingObject.PositionComp.WorldMatrixNormalizedInv);
					if (previewFloatingObject.PositionComp.LocalAABB.Contains(point) == ContainmentType.Contains)
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
			foreach (MyFloatingObject previewFloatingObject2 in m_previewFloatingObjects)
			{
				previewFloatingObject2.Render.Visible = flag;
			}
		}

		public void ClearClipboard()
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedFloatingObjects.Clear();
			m_copiedFloatingObjectOffsets.Clear();
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

		public void MoveEntityFurther()
		{
			m_dragDistance *= 1.1f;
		}

		public void MoveEntityCloser()
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
