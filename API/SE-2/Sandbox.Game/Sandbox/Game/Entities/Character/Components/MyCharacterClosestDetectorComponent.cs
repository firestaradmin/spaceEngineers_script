using System;
using System.Collections.Generic;
using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	internal class MyCharacterClosestDetectorComponent : MyCharacterRaycastDetectorComponent
	{
		private class MyHitInfoWrapper
		{
			public MyPhysics.HitInfo Value;

			public MyHitInfoWrapper(MyPhysics.HitInfo hitInfo)
			{
				Value = hitInfo;
			}
		}

		private class Sandbox_Game_Entities_Character_Components_MyCharacterClosestDetectorComponent_003C_003EActor : IActivator, IActivator<MyCharacterClosestDetectorComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterClosestDetectorComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterClosestDetectorComponent CreateInstance()
			{
				return new MyCharacterClosestDetectorComponent();
			}

			MyCharacterClosestDetectorComponent IActivator<MyCharacterClosestDetectorComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private float m_castRadius = 0.5f;

		private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();

		private List<Tuple<MyUseObjectsComponentBase, object>> m_useComponents = new List<Tuple<MyUseObjectsComponentBase, object>>();

		private Dictionary<IMyUseObject, Tuple<float, object>> m_useObjectDistances = new Dictionary<IMyUseObject, Tuple<float, object>>();

		private bool m_useTemporaryUseObject = true;

		private IMyUseObject m_temporaryUseObject;

		private bool UseTemporaryUseObject
		{
			get
			{
				return m_useTemporaryUseObject;
			}
			set
			{
				if (m_useTemporaryUseObject != value)
				{
					m_useTemporaryUseObject = value;
					if (m_useTemporaryUseObject)
					{
						m_temporaryUseObject = UseObject;
					}
					else
					{
						UseObject = m_temporaryUseObject;
					}
				}
			}
		}

		public override IMyUseObject UseObject
		{
			get
			{
				if (m_useTemporaryUseObject)
				{
					return m_temporaryUseObject;
				}
				return m_interactiveObject;
			}
			set
			{
				if (m_useTemporaryUseObject)
				{
					if (m_temporaryUseObject != value)
					{
						m_temporaryUseObject = value;
					}
				}
				else if (m_interactiveObject != value)
				{
					if (m_interactiveObject != null)
					{
						UseClose();
						m_interactiveObject.OnSelectionLost();
						InteractiveObjectRemoved();
					}
					m_interactiveObject = value;
					InteractiveObjectChanged();
				}
			}
		}

		private float ComputeDistanceFromLine(IMyEntity entity, Vector3D from, Vector3 dir)
		{
			MyPositionComponentBase myPositionComponentBase = entity.Components.Get<MyPositionComponentBase>();
			RayD ray = new RayD(from, dir);
			MatrixD identity = MatrixD.Identity;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD;
			if (entity is IMyUseObject)
			{
				identity = myPositionComponentBase.WorldMatrixRef;
				myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(entity.Model.BoundingBox, identity);
			}
			else
			{
				identity = myPositionComponentBase.WorldMatrixRef;
				myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(identity);
			}
			double? num = myOrientedBoundingBoxD.Intersects(ref ray);
			if (num.HasValue)
			{
				return (float)(0.0 - num.Value);
			}
			return myOrientedBoundingBoxD.Distance(ray);
		}

		protected override void DoDetection(bool useHead)
		{
			UseTemporaryUseObject = true;
			m_hits.Clear();
			m_useComponents.Clear();
			m_useObjectDistances.Clear();
			UseObject = null;
			base.DoDetection(useHead);
			if (UseObject != null)
			{
				UseTemporaryUseObject = false;
				return;
			}
			if (!MySandboxGame.Config.AreaInteraction || (MyCubeBuilder.Static != null && MyCubeBuilder.Static.IsActivated))
			{
				UseTemporaryUseObject = false;
				return;
			}
			if (base.Character.IsOnLadder)
			{
				UseTemporaryUseObject = false;
				return;
			}
			if (base.Character == MySession.Static.ControlledEntity)
			{
				MyHud.SelectedObjectHighlight.RemoveHighlight();
			}
			MatrixD headMatrix = base.Character.GetHeadMatrix(includeY: false);
			Vector3D planePoint = headMatrix.Translation - headMatrix.Forward * 0.3;
			if (MySession.Static.IsCameraUserControlledSpectator())
			{
				UseTemporaryUseObject = false;
				return;
			}
			Vector3 vector;
			Vector3D vector3D;
			if (!useHead)
			{
				MatrixD worldMatrix = MySector.MainCamera.WorldMatrix;
				vector = worldMatrix.Forward;
				vector3D = MyUtils.LinePlaneIntersection(planePoint, vector, worldMatrix.Translation, vector);
				vector3D = worldMatrix.Translation;
			}
			else
			{
				vector = headMatrix.Forward;
				vector3D = MySector.MainCamera.WorldMatrix.Translation;
			}
			Vector3D to = vector3D + vector * Math.Max(MyConstants.DEFAULT_INTERACTIVE_DISTANCE - m_castRadius, 0f);
			base.StartPosition = vector3D;
			MatrixD transform = MatrixD.CreateTranslation(vector3D);
			HkShape shape = new HkSphereShape(m_castRadius);
			MyPhysics.CastShapeReturnContactBodyDatas(to, shape, ref transform, 0u, 0f, m_hits);
			if (m_hits.Count <= 0)
			{
				UseObject = null;
				UseTemporaryUseObject = false;
				return;
			}
			foreach (MyPhysics.HitInfo hit in m_hits)
			{
				IMyEntity myEntity = hit.HkHitInfo.GetHitEntity();
				if (myEntity == base.Character || myEntity.Model == null)
				{
					continue;
				}
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = myEntity as MyCubeGrid) != null)
				{
					HkHitInfo hkHitInfo = hit.HkHitInfo;
					if (hkHitInfo.ShapeKeyCount > 0)
					{
						MyGridShape shape2 = myCubeGrid.Physics.Shape;
						hkHitInfo = hit.HkHitInfo;
						HkHitInfo hkHitInfo2 = hit.HkHitInfo;
						MySlimBlock blockFromShapeKey = shape2.GetBlockFromShapeKey(hkHitInfo.GetShapeKey(hkHitInfo2.ShapeKeyCount - 1));
						if (blockFromShapeKey != null && blockFromShapeKey.FatBlock != null)
						{
							myEntity = blockFromShapeKey.FatBlock;
						}
					}
				}
				MyHitInfoWrapper myHitInfoWrapper = new MyHitInfoWrapper(hit);
				int count = m_useComponents.Count;
				GetUseComponentsFromParentStructure(myEntity, m_useComponents, myHitInfoWrapper);
				IMyUseObject key;
				if (count != m_useComponents.Count || (key = myEntity as IMyUseObject) == null)
				{
					continue;
				}
				float num = ComputeDistanceFromLine(myEntity, vector3D, vector);
				if (m_useObjectDistances.ContainsKey(key))
				{
					if (num < m_useObjectDistances[key].Item1)
					{
						m_useObjectDistances[key] = new Tuple<float, object>(num, myHitInfoWrapper);
					}
				}
				else
				{
					m_useObjectDistances.Add(key, new Tuple<float, object>(num, myHitInfoWrapper));
				}
			}
			if (m_useComponents.Count <= 0 && m_useObjectDistances.Count <= 0)
			{
				UseObject = null;
				UseTemporaryUseObject = false;
				return;
			}
			foreach (Tuple<MyUseObjectsComponentBase, object> useComponent in m_useComponents)
			{
				useComponent.Item1.ProcessComponentToUseObjectsAndDistances(ref m_useObjectDistances, vector3D, vector, useComponent.Item2);
			}
			IMyUseObject closest = null;
			float distance = 0f;
			object obj = null;
			while (m_useObjectDistances.Count > 0)
			{
				if (!GetClosestNonNull(m_useObjectDistances, ref closest, ref distance, ref obj))
				{
					continue;
				}
				m_useObjectDistances.Remove(closest);
				MyUseObjectsComponentBase myUseObjectsComponentBase = closest.Owner.Components.Get<MyUseObjectsComponentBase>();
				MyFloatingObject detectedEntity;
				if ((detectedEntity = closest as MyFloatingObject) != null)
				{
					MyHitInfoWrapper myHitInfoWrapper2 = obj as MyHitInfoWrapper;
					if (myHitInfoWrapper2 != null && RaycastAllCornersFloating(closest, vector3D))
					{
						base.HitBody = myHitInfoWrapper2.Value.HkHitInfo.Body;
						base.HitMaterial = base.HitBody.GetBody().GetMaterialAt(base.HitPosition);
						base.HitPosition = myHitInfoWrapper2.Value.GetFixedPosition();
						base.DetectedEntity = detectedEntity;
						UseObject = closest;
						UseTemporaryUseObject = false;
						if (base.Character == MySession.Static.ControlledEntity && UseObject.SupportedActions != 0 && !base.Character.IsOnLadder)
						{
							MyCharacterDetectorComponent.HandleInteractiveObject(UseObject);
						}
						return;
					}
				}
				else
				{
					if ((myUseObjectsComponentBase == null && closest.Owner is MyCharacter) || !MyFakes.ENABLE_AREA_INTERACTIONS_BLOCKS || !(closest.Owner is MyCubeBlock) || myUseObjectsComponentBase == null)
					{
						continue;
					}
					MyHitInfoWrapper myHitInfoWrapper3 = obj as MyHitInfoWrapper;
					if (myHitInfoWrapper3 == null)
					{
						continue;
					}
					_ = (Vector3D)myHitInfoWrapper3.Value.GetFixedPosition();
					MatrixD? detectorTransformation = myUseObjectsComponentBase.GetDetectorTransformation(closest);
					if (RaycastAllCorners(myUseObjectsComponentBase, closest, vector3D))
					{
						base.HitBody = myHitInfoWrapper3.Value.HkHitInfo.Body;
						base.HitMaterial = base.HitBody.GetBody().GetMaterialAt(base.HitPosition);
						base.HitPosition = (detectorTransformation.HasValue ? detectorTransformation.Value.Translation : ((Vector3D)myHitInfoWrapper3.Value.GetFixedPosition()));
						base.DetectedEntity = myUseObjectsComponentBase.Entity;
						UseObject = closest;
						UseTemporaryUseObject = false;
						if (base.Character == MySession.Static.ControlledEntity && UseObject.SupportedActions != 0 && !base.Character.IsOnLadder)
						{
							MyCharacterDetectorComponent.HandleInteractiveObject(UseObject);
						}
						return;
					}
				}
			}
			UseTemporaryUseObject = false;
		}

		private bool RaycastAllCorners(MyUseObjectsComponentBase component, IMyUseObject closest, Vector3D from)
		{
			float num = 0.0100000007f;
			MatrixD? detectorTransformation = component.GetDetectorTransformation(closest);
			if (!detectorTransformation.HasValue)
			{
				return false;
			}
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(detectorTransformation.Value);
			Vector3D[] array = new Vector3D[8];
			myOrientedBoundingBoxD.GetCorners(array, 0);
			Vector3D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Vector3D vector3D = array2[i] - from;
				float num2 = (float)vector3D.LengthSquared();
				Vector3D vector3D2 = vector3D / Math.Sqrt(num2);
				Vector3D to = from + vector3D2 * 2.0 * MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
				LineD line = new LineD(from, to);
<<<<<<< HEAD
				MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Character, null, ignoreChildren: false, ignoreFloatingObjects: false, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: true, ignoreSubgridsOfIgnoredEntities: false, ignoreCharacters: true);
=======
				MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Character, null, ignoreChildren: false, ignoreFloatingObjects: false, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: true, ignoreCharacters: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!intersectionWithLine.HasValue)
				{
					return true;
				}
				float num3 = (float)(intersectionWithLine.Value.IntersectionPointInWorldSpace - from).LengthSquared();
				if (num2 <= num3 + num)
				{
					return true;
				}
			}
			return false;
		}

		private bool RaycastAllCornersFloating(IMyUseObject closest, Vector3D from)
		{
			float num = 0.0100000007f;
			IMyEntity myEntity = closest as IMyEntity;
			if (myEntity == null)
			{
				return false;
			}
			MyPositionComponentBase myPositionComponentBase = myEntity.Components.Get<MyPositionComponentBase>();
			MatrixD identity = MatrixD.Identity;
			identity = myPositionComponentBase.WorldMatrixRef;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(myEntity.Model.BoundingBox, identity);
			Vector3D[] array = new Vector3D[8];
			myOrientedBoundingBoxD.GetCorners(array, 0);
			Vector3D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Vector3D vector3D = array2[i] - from;
				float num2 = (float)vector3D.LengthSquared();
				Vector3D vector3D2 = vector3D / Math.Sqrt(num2);
				Vector3D to = from + vector3D2 * MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
				LineD line = new LineD(from, to);
<<<<<<< HEAD
				MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Character, null, ignoreChildren: false, ignoreFloatingObjects: false, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: true, ignoreSubgridsOfIgnoredEntities: false, ignoreCharacters: true);
=======
				MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Character, null, ignoreChildren: false, ignoreFloatingObjects: false, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: true, ignoreCharacters: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!intersectionWithLine.HasValue)
				{
					return true;
				}
				float num3 = (float)(intersectionWithLine.Value.IntersectionPointInWorldSpace - from).LengthSquared();
				if (num2 <= num3 + num)
				{
					return true;
				}
			}
			return false;
		}

		private bool GetClosestNonNull(Dictionary<IMyUseObject, Tuple<float, object>> useObjects, ref IMyUseObject closest, ref float distance, ref object obj)
		{
			float num = float.MaxValue;
			IMyUseObject myUseObject = null;
			object obj2 = null;
			bool result = false;
			bool flag = false;
			foreach (KeyValuePair<IMyUseObject, Tuple<float, object>> useObject in useObjects)
			{
				float item = useObject.Value.Item1;
				if (item > 0f)
				{
					if (!flag && !(item >= num))
					{
						num = item;
						obj2 = useObject.Value.Item2;
						myUseObject = useObject.Key;
						result = true;
					}
				}
				else if (flag)
				{
					if (!(item <= num))
					{
						num = item;
						obj2 = useObject.Value.Item2;
						myUseObject = useObject.Key;
						result = true;
					}
				}
				else
				{
					num = item;
					obj2 = useObject.Value.Item2;
					myUseObject = useObject.Key;
					result = true;
					flag = true;
				}
			}
			closest = myUseObject;
			distance = num;
			obj = obj2;
			return result;
		}

		private void GetUseComponentsFromParentStructure(IMyEntity currentEntity, List<Tuple<MyUseObjectsComponentBase, object>> useComponents, object hit)
		{
			MyUseObjectsComponentBase myUseObjectsComponentBase = currentEntity.Components.Get<MyUseObjectsComponentBase>();
			if (myUseObjectsComponentBase != null)
			{
				useComponents.Add(new Tuple<MyUseObjectsComponentBase, object>(myUseObjectsComponentBase, hit));
			}
			if (currentEntity.Parent != null)
			{
				GetUseComponentsFromParentStructure(currentEntity.Parent, useComponents, hit);
			}
		}
	}
}
