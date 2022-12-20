using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 900, typeof(MyObjectBuilder_CutsceneSessionComponent), null, false)]
	public class MySessionComponentCutscenes : MySessionComponentBase
	{
		private MyObjectBuilder_CutsceneSessionComponent m_objectBuilder;

		private Dictionary<string, Cutscene> m_cutsceneLibrary = new Dictionary<string, Cutscene>();

		private Cutscene m_currentCutscene;

		private CutsceneSequenceNode m_currentNode;

		private float m_currentTime;

		private float m_currentFOV = 70f;

		private int m_currentNodeIndex;

		private bool m_nodeActivated;

		private float MINIMUM_FOV = 10f;

		private float MAXIMUM_FOV = 300f;

		private float m_eventDelay = float.MaxValue;

		private bool m_releaseCamera;

		private bool m_copyToSpectator;

		private bool m_overlayEnabled;

		private bool m_registerEvents = true;

		private string m_cameraOverlay = "";

		private string m_cameraOverlayOriginal = "";

		private MatrixD m_nodeStartMatrix;

		private float m_nodeStartFOV = 70f;

		private MatrixD m_nodeEndMatrix;

		private MatrixD m_currentCameraMatrix;

		private MyEntity m_lookTarget;

		private MyEntity m_rotateTarget;

		private MyEntity m_moveTarget;

		private MyEntity m_attachedPositionTo;

		private Vector3D m_attachedPositionOffset = Vector3D.Zero;

		private MyEntity m_attachedRotationTo;

		private MatrixD m_attachedRotationOffset;

		private Vector3D m_lastUpVector = Vector3D.Up;

		private IMyCameraController m_originalCameraController;

		private MyCutsceneCamera m_cameraEntity = new MyCutsceneCamera();

		public MatrixD CameraMatrix => m_currentCameraMatrix;

		public bool IsCutsceneRunning => m_currentCutscene != null;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_objectBuilder = sessionComponent as MyObjectBuilder_CutsceneSessionComponent;
			if (m_objectBuilder == null || m_objectBuilder.Cutscenes == null || m_objectBuilder.Cutscenes.Length == 0)
			{
				return;
			}
			Cutscene[] cutscenes = m_objectBuilder.Cutscenes;
			foreach (Cutscene cutscene in cutscenes)
			{
				if (cutscene.Name != null && cutscene.Name.Length > 0 && !m_cutsceneLibrary.ContainsKey(cutscene.Name))
				{
					m_cutsceneLibrary.Add(cutscene.Name, cutscene);
				}
			}
		}

		public override void BeforeStart()
		{
			if (m_objectBuilder == null)
			{
				return;
			}
			foreach (string voxelPrecachingWaypointName in m_objectBuilder.VoxelPrecachingWaypointNames)
			{
				if (MyEntities.TryGetEntityByName(voxelPrecachingWaypointName, out var entity))
				{
					MyRenderProxy.PointsForVoxelPrecache.Add(entity.PositionComp.GetPosition());
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (m_releaseCamera && MySession.Static.ControlledEntity != null)
			{
				m_releaseCamera = false;
				if (MySession.Static.CameraController is MyCutsceneCamera)
				{
					MySession.Static.CameraController = m_originalCameraController;
				}
				MyHud.CutsceneHud = false;
				MyGuiScreenGamePlay.DisableInput = false;
				if (m_copyToSpectator)
				{
					MySpectatorCameraController.Static.Position = m_cameraEntity.PositionComp.WorldMatrixRef.Translation;
					MySpectatorCameraController.Static.SetTarget(m_cameraEntity.PositionComp.WorldMatrixRef.Translation + m_cameraEntity.PositionComp.WorldMatrixRef.Forward, m_cameraEntity.PositionComp.WorldMatrixRef.Up);
				}
			}
			if (IsCutsceneRunning)
			{
				if (MySession.Static.CameraController != m_cameraEntity)
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, m_cameraEntity);
				}
				if (m_currentCutscene.SequenceNodes != null && m_currentCutscene.SequenceNodes.Count > m_currentNodeIndex)
				{
					m_currentNode = m_currentCutscene.SequenceNodes[m_currentNodeIndex];
					CutsceneUpdate();
				}
				else if (m_currentCutscene.NextCutscene != null && m_currentCutscene.NextCutscene.Length > 0)
				{
					PlayCutscene(m_currentCutscene.NextCutscene, m_registerEvents);
				}
				else
				{
					CutsceneEnd();
				}
				m_cameraEntity.WorldMatrix = m_currentCameraMatrix;
			}
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES)
			{
				return;
			}
			foreach (Cutscene value in m_cutsceneLibrary.Values)
			{
				if (value.SequenceNodes == null)
				{
					continue;
				}
				foreach (CutsceneSequenceNode sequenceNode in value.SequenceNodes)
				{
					if (sequenceNode.Waypoints == null || sequenceNode.Waypoints.Count <= 2)
					{
						continue;
					}
					Vector3D pointFrom = Vector3D.Zero;
					int num = 0;
					for (float num2 = 0f; num2 <= 1f; num2 += 0.01f)
					{
						Vector3D bezierPosition = sequenceNode.GetBezierPosition(num2);
						MatrixD bezierOrientation = sequenceNode.GetBezierOrientation(num2);
						bezierOrientation.Translation = bezierPosition;
						MyRenderProxy.DebugDrawAxis(bezierOrientation, 0.4f, depthRead: false);
						if (num > 0)
						{
							MyRenderProxy.DebugDrawLine3D(pointFrom, bezierPosition, Color.Yellow, Color.Yellow, depthRead: false);
						}
						pointFrom = bezierPosition;
						num++;
					}
				}
			}
		}

		public void CutsceneUpdate()
		{
			if (!m_nodeActivated)
			{
				m_nodeActivated = true;
				m_nodeStartMatrix = m_currentCameraMatrix;
				m_nodeEndMatrix = m_currentCameraMatrix;
				m_nodeStartFOV = m_currentFOV;
				m_moveTarget = null;
				m_rotateTarget = null;
				m_eventDelay = float.MaxValue;
				if (m_registerEvents && m_currentNode.Event != null && m_currentNode.Event.Length > 0 && MyVisualScriptLogicProvider.CutsceneNodeEvent != null)
				{
					if (m_currentNode.EventDelay <= 0f)
					{
						MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentNode.Event);
					}
					else
					{
						m_eventDelay = m_currentNode.EventDelay;
					}
				}
				if (m_currentNode.LookAt != null && m_currentNode.LookAt.Length > 0)
				{
					MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.LookAt);
					if (entityByName != null)
					{
						Matrix m = MatrixD.CreateLookAtInverse(m_currentCameraMatrix.Translation, entityByName.PositionComp.GetPosition(), m_currentCameraMatrix.Up);
						m_nodeStartMatrix = m;
						m_nodeEndMatrix = m_nodeStartMatrix;
					}
				}
				if (m_currentNode.SetRotationLike != null && m_currentNode.SetRotationLike.Length > 0)
				{
					MyEntity entityByName2 = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.SetRotationLike);
					if (entityByName2 != null)
					{
						m_nodeStartMatrix = entityByName2.WorldMatrix;
						m_nodeEndMatrix = m_nodeStartMatrix;
					}
				}
				if (m_currentNode.RotateLike != null && m_currentNode.RotateLike.Length > 0)
				{
					MyEntity entityByName3 = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.RotateLike);
					if (entityByName3 != null)
					{
						m_nodeEndMatrix = entityByName3.WorldMatrix;
					}
				}
				if (m_currentNode.RotateTowards != null && m_currentNode.RotateTowards.Length > 0)
				{
					m_rotateTarget = ((m_currentNode.RotateTowards.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.RotateTowards) : null);
				}
				if (m_currentNode.LockRotationTo != null)
				{
					m_lookTarget = ((m_currentNode.LockRotationTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.LockRotationTo) : null);
				}
				m_nodeStartMatrix.Translation = m_currentCameraMatrix.Translation;
				m_nodeEndMatrix.Translation = m_currentCameraMatrix.Translation;
				if (m_currentNode.SetPositionTo != null && m_currentNode.SetPositionTo.Length > 0)
				{
					MyEntity entityByName4 = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.SetPositionTo);
					if (entityByName4 != null)
					{
						m_nodeStartMatrix.Translation = entityByName4.WorldMatrix.Translation;
						m_nodeEndMatrix.Translation = entityByName4.WorldMatrix.Translation;
					}
				}
				if (m_currentNode.AttachTo != null)
				{
					m_attachedRotationOffset = MatrixD.Identity;
					m_attachedPositionOffset = Vector3D.Zero;
					m_attachedPositionTo = ((m_currentNode.AttachTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.AttachTo) : null);
					if (m_attachedPositionTo != null)
					{
						m_attachedPositionOffset = Vector3D.Transform(m_currentCameraMatrix.Translation, m_attachedPositionTo.PositionComp.WorldMatrixInvScaled);
						m_attachedRotationTo = m_attachedPositionTo;
						m_attachedRotationOffset = m_currentCameraMatrix * m_attachedRotationTo.PositionComp.WorldMatrixInvScaled;
						m_attachedRotationOffset.Translation = Vector3D.Zero;
					}
				}
				else
				{
					if (m_currentNode.AttachPositionTo != null)
					{
						m_attachedPositionTo = ((m_currentNode.AttachPositionTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.AttachPositionTo) : null);
						m_attachedPositionOffset = ((m_attachedPositionTo != null) ? Vector3D.Transform(m_currentCameraMatrix.Translation, m_attachedPositionTo.PositionComp.WorldMatrixInvScaled) : Vector3D.Zero);
					}
					if (m_currentNode.AttachRotationTo != null)
					{
						m_attachedRotationOffset = MatrixD.Identity;
						m_attachedRotationTo = ((m_currentNode.AttachRotationTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.AttachRotationTo) : null);
						if (m_attachedRotationTo != null)
						{
							m_attachedRotationOffset = m_currentCameraMatrix * m_attachedRotationTo.PositionComp.WorldMatrixInvScaled;
							m_attachedRotationOffset.Translation = Vector3D.Zero;
						}
					}
				}
				if (m_currentNode.MoveTo != null && m_currentNode.MoveTo.Length > 0)
				{
					m_moveTarget = ((m_currentNode.MoveTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.MoveTo) : null);
				}
				if (m_currentNode.Waypoints != null && m_currentNode.Waypoints.Count > 0)
				{
					if (m_currentNode.Waypoints.Count == 2)
					{
						m_nodeStartMatrix = m_currentNode.Waypoints[0].GetWorldMatrix();
						m_nodeEndMatrix = m_currentNode.Waypoints[1].GetWorldMatrix();
					}
					else if (m_currentNode.Waypoints.Count < 3)
					{
						m_nodeEndMatrix = m_currentNode.Waypoints[m_currentNode.Waypoints.Count - 1].GetWorldMatrix();
					}
				}
				m_currentCameraMatrix = m_nodeStartMatrix;
			}
			m_currentTime += 0.0166666675f;
			float num = ((m_currentNode.Time > 0f) ? MathHelper.Clamp(m_currentTime / m_currentNode.Time, 0f, 1f) : 1f);
			if (m_registerEvents && m_currentTime >= m_eventDelay)
			{
				m_eventDelay = float.MaxValue;
				MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentNode.Event);
			}
			if (m_moveTarget != null)
			{
				m_nodeEndMatrix.Translation = m_moveTarget.PositionComp.GetPosition();
			}
			Vector3D vector3D = m_currentCameraMatrix.Translation;
			if (m_attachedPositionTo != null)
			{
				if (!m_attachedPositionTo.Closed)
				{
					vector3D = Vector3D.Transform(m_attachedPositionOffset, m_attachedPositionTo.PositionComp.WorldMatrixRef);
				}
			}
			else if (m_currentNode.Waypoints != null && m_currentNode.Waypoints.Count > 2)
			{
				vector3D = m_currentNode.GetBezierPosition(num);
			}
			else if (m_nodeStartMatrix.Translation != m_nodeEndMatrix.Translation)
			{
				vector3D = new Vector3D(MathHelper.SmoothStep(m_nodeStartMatrix.Translation.X, m_nodeEndMatrix.Translation.X, num), MathHelper.SmoothStep(m_nodeStartMatrix.Translation.Y, m_nodeEndMatrix.Translation.Y, num), MathHelper.SmoothStep(m_nodeStartMatrix.Translation.Z, m_nodeEndMatrix.Translation.Z, num));
			}
			if (m_rotateTarget != null)
			{
				Matrix m = MatrixD.CreateLookAtInverse(m_currentCameraMatrix.Translation, m_rotateTarget.PositionComp.GetPosition(), m_nodeStartMatrix.Up);
				m_nodeEndMatrix = m;
			}
			if (m_lookTarget != null && m_currentNode.Waypoints != null)
			{
				if (!m_lookTarget.Closed)
				{
<<<<<<< HEAD
					m_currentCameraMatrix = MatrixD.CreateLookAtInverse(vector3D, m_lookTarget.PositionComp.GetPosition(), (m_currentNode.Waypoints.Count > 2) ? m_currentNode.Waypoints[m_currentNode.Waypoints.Count - 1].GetWorldMatrix().Up : m_currentCameraMatrix.Up);
=======
					Matrix m = MatrixD.CreateLookAtInverse(vector3D, m_lookTarget.PositionComp.GetPosition(), (m_currentNode.Waypoints.Count > 2) ? m_currentNode.Waypoints[m_currentNode.Waypoints.Count - 1].GetWorldMatrix().Up : m_currentCameraMatrix.Up);
					m_currentCameraMatrix = m;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			else if (m_attachedRotationTo != null)
			{
				m_currentCameraMatrix = m_attachedRotationOffset * m_attachedRotationTo.WorldMatrix;
			}
			else if (m_currentNode.Waypoints != null && m_currentNode.Waypoints.Count > 2)
			{
				m_currentCameraMatrix = m_currentNode.GetBezierOrientation(num);
			}
			else if (!m_nodeStartMatrix.EqualsFast(ref m_nodeEndMatrix))
			{
				QuaternionD quaternion = QuaternionD.CreateFromRotationMatrix(m_nodeStartMatrix);
				QuaternionD quaternion2 = QuaternionD.CreateFromRotationMatrix(m_nodeEndMatrix);
				QuaternionD quaternion3 = QuaternionD.Slerp(quaternion, quaternion2, MathHelper.SmoothStepStable((double)num));
				m_currentCameraMatrix = MatrixD.CreateFromQuaternion(quaternion3);
			}
			m_currentCameraMatrix.Translation = vector3D;
			if (m_currentNode.ChangeFOVTo > MINIMUM_FOV)
			{
				m_currentFOV = MathHelper.SmoothStep(m_nodeStartFOV, MathHelper.Clamp(m_currentNode.ChangeFOVTo, MINIMUM_FOV, MAXIMUM_FOV), num);
			}
			m_cameraEntity.FOV = m_currentFOV;
			if (m_currentTime >= m_currentNode.Time)
			{
				CutsceneNext(setToZero: false);
			}
		}

		public void CutsceneEnd(bool releaseCamera = true, bool copyToSpectator = false)
		{
			MyHudWarnings.EnableWarnings = true;
			if (m_currentCutscene != null)
			{
				string name = m_currentCutscene.Name;
				m_currentCutscene = null;
				if (releaseCamera)
				{
					m_cameraEntity.FOV = MathHelper.ToDegrees(MySandboxGame.Config.FieldOfView);
					m_releaseCamera = true;
					m_copyToSpectator = copyToSpectator;
				}
				MyHudCameraOverlay.TextureName = m_cameraOverlayOriginal;
				MyHudCameraOverlay.Enabled = m_overlayEnabled;
				if (MyVisualScriptLogicProvider.CutsceneEnded != null)
				{
					MyVisualScriptLogicProvider.CutsceneEnded(name);
				}
			}
		}

		public void CutsceneNext(bool setToZero)
		{
			m_nodeActivated = false;
			m_currentNodeIndex++;
			m_currentTime -= (setToZero ? m_currentTime : m_currentNode.Time);
		}

		public void CutsceneSkip()
		{
			if (m_currentCutscene == null)
			{
				return;
			}
			if (m_currentCutscene.CanBeSkipped)
			{
				if (m_currentCutscene.FireEventsDuringSkip && MyVisualScriptLogicProvider.CutsceneNodeEvent != null && m_registerEvents)
				{
					if (m_currentNode != null && m_currentNode.EventDelay > 0f && m_eventDelay != float.MaxValue)
					{
						MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentNode.Event);
					}
					for (int i = m_currentNodeIndex + 1; i < m_currentCutscene.SequenceNodes.Count; i++)
					{
						if (!string.IsNullOrEmpty(m_currentCutscene.SequenceNodes[i].Event))
						{
							MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentCutscene.SequenceNodes[i].Event);
						}
					}
				}
				m_currentNodeIndex = m_currentCutscene.SequenceNodes.Count;
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
			}
			else
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_cutsceneLibrary.Clear();
			MyHudWarnings.EnableWarnings = true;
			m_cameraEntity.Close();
			m_cameraEntity = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			m_objectBuilder.Cutscenes = new Cutscene[m_cutsceneLibrary.Count];
			int num = 0;
			foreach (Cutscene value in m_cutsceneLibrary.Values)
			{
				m_objectBuilder.Cutscenes[num] = value;
				num++;
			}
			return m_objectBuilder;
		}

		public bool PlayCutscene(string cutsceneName, bool registerEvents = true, string overlay = "")
		{
			if (m_cutsceneLibrary.ContainsKey(cutsceneName))
			{
				return PlayCutscene(m_cutsceneLibrary[cutsceneName], registerEvents, overlay);
			}
			CutsceneEnd();
			return false;
		}

		public bool PlayCutscene(Cutscene cutscene, bool registerEvents = true, string overlay = "")
		{
			if (cutscene != null)
			{
				MySandboxGame.Log.WriteLineAndConsole("Cutscene start: " + cutscene.Name);
				if (IsCutsceneRunning)
				{
					CutsceneEnd(releaseCamera: false);
				}
				else
				{
					m_cameraOverlayOriginal = MyHudCameraOverlay.TextureName;
					m_overlayEnabled = MyHudCameraOverlay.Enabled;
				}
				m_registerEvents = registerEvents;
				m_cameraOverlay = overlay;
				m_currentCutscene = cutscene;
				m_currentNode = null;
				m_currentNodeIndex = 0;
				m_currentTime = 0f;
				m_nodeActivated = false;
				m_lookTarget = null;
				m_attachedPositionTo = null;
				m_attachedRotationTo = null;
				m_rotateTarget = null;
				m_moveTarget = null;
				m_currentFOV = MathHelper.Clamp(m_currentCutscene.StartingFOV, MINIMUM_FOV, MAXIMUM_FOV);
				MyGuiScreenGamePlay.DisableInput = true;
				if (MyCubeBuilder.Static.IsActivated)
				{
					MyCubeBuilder.Static.Deactivate();
				}
				MyHud.CutsceneHud = true;
				MyHudCameraOverlay.TextureName = overlay;
				MyHudCameraOverlay.Enabled = overlay.Length > 0;
				MyHudWarnings.EnableWarnings = false;
				MatrixD matrixD = MatrixD.Identity;
				MyEntity myEntity = ((m_currentCutscene.StartEntity.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentCutscene.StartEntity) : null);
				if (myEntity != null)
				{
					matrixD = myEntity.WorldMatrix;
				}
				if (m_currentCutscene.StartLookAt.Length > 0 && !m_currentCutscene.StartLookAt.Equals(m_currentCutscene.StartEntity))
				{
					myEntity = MyVisualScriptLogicProvider.GetEntityByName(m_currentCutscene.StartLookAt);
					if (myEntity != null)
					{
						Matrix m = MatrixD.CreateLookAtInverse(matrixD.Translation, myEntity.PositionComp.GetPosition(), matrixD.Up);
						matrixD = m;
					}
				}
				m_nodeStartMatrix = matrixD;
				m_currentCameraMatrix = matrixD;
				if (!(MySession.Static.CameraController is MyCutsceneCamera))
				{
					m_originalCameraController = MySession.Static.CameraController;
				}
				m_cameraEntity.WorldMatrix = matrixD;
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, m_cameraEntity);
				return true;
			}
			CutsceneEnd();
			return false;
		}

		public Dictionary<string, Cutscene> GetCutscenes()
		{
			return m_cutsceneLibrary;
		}

		public Cutscene GetCutscene(string name)
		{
			if (m_cutsceneLibrary.ContainsKey(name))
			{
				return m_cutsceneLibrary[name];
			}
			return null;
		}

		public Cutscene GetCutsceneCopy(string name)
		{
			if (m_cutsceneLibrary.ContainsKey(name))
			{
				Cutscene cutscene = m_cutsceneLibrary[name];
				Cutscene cutscene2 = new Cutscene();
				cutscene2.CanBeSkipped = cutscene.CanBeSkipped;
				cutscene2.FireEventsDuringSkip = cutscene.FireEventsDuringSkip;
				cutscene2.Name = cutscene.Name;
				cutscene2.NextCutscene = cutscene.NextCutscene;
				cutscene2.StartEntity = cutscene.StartEntity;
				cutscene2.StartingFOV = cutscene.StartingFOV;
				cutscene2.StartLookAt = cutscene.StartLookAt;
				if (cutscene.SequenceNodes != null)
				{
					cutscene2.SequenceNodes = new List<CutsceneSequenceNode>();
					for (int i = 0; i < cutscene.SequenceNodes.Count; i++)
					{
						cutscene2.SequenceNodes.Add(new CutsceneSequenceNode());
						cutscene2.SequenceNodes[i].AttachPositionTo = cutscene.SequenceNodes[i].AttachPositionTo;
						cutscene2.SequenceNodes[i].AttachRotationTo = cutscene.SequenceNodes[i].AttachRotationTo;
						cutscene2.SequenceNodes[i].AttachTo = cutscene.SequenceNodes[i].AttachTo;
						cutscene2.SequenceNodes[i].ChangeFOVTo = cutscene.SequenceNodes[i].ChangeFOVTo;
						cutscene2.SequenceNodes[i].Event = cutscene.SequenceNodes[i].Event;
						cutscene2.SequenceNodes[i].EventDelay = cutscene.SequenceNodes[i].EventDelay;
						cutscene2.SequenceNodes[i].LockRotationTo = cutscene.SequenceNodes[i].LockRotationTo;
						cutscene2.SequenceNodes[i].LookAt = cutscene.SequenceNodes[i].LookAt;
						cutscene2.SequenceNodes[i].MoveTo = cutscene.SequenceNodes[i].MoveTo;
						cutscene2.SequenceNodes[i].RotateLike = cutscene.SequenceNodes[i].RotateLike;
						cutscene2.SequenceNodes[i].RotateTowards = cutscene.SequenceNodes[i].RotateTowards;
						cutscene2.SequenceNodes[i].SetPositionTo = cutscene.SequenceNodes[i].SetPositionTo;
						cutscene2.SequenceNodes[i].SetRotationLike = cutscene.SequenceNodes[i].SetRotationLike;
						cutscene2.SequenceNodes[i].Time = cutscene.SequenceNodes[i].Time;
						if (cutscene.SequenceNodes[i].Waypoints != null && cutscene.SequenceNodes[i].Waypoints.Count > 0)
						{
							cutscene2.SequenceNodes[i].Waypoints = new List<CutsceneSequenceNodeWaypoint>();
							for (int j = 0; j < cutscene.SequenceNodes[i].Waypoints.Count; j++)
							{
								cutscene2.SequenceNodes[i].Waypoints.Add(new CutsceneSequenceNodeWaypoint());
								cutscene2.SequenceNodes[i].Waypoints[j].Name = cutscene.SequenceNodes[i].Waypoints[j].Name;
								cutscene2.SequenceNodes[i].Waypoints[j].Time = cutscene.SequenceNodes[i].Waypoints[j].Time;
							}
						}
					}
				}
				return cutscene2;
			}
			return null;
		}
	}
}
