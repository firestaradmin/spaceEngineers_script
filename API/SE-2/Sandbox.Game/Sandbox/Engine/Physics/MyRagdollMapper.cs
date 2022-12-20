using System;
using System.Collections.Generic;
using System.Linq;
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Engine.Physics
{
	/// <summary>
	/// Maps Ragdoll instance to Character Bones
	/// </summary>
	public class MyRagdollMapper
	{
		public const float RAGDOLL_DEACTIVATION_TIME = 10f;

		/// <summary>
		/// Helper class to ease the animation blending based on per bone variables.
		/// </summary>
		private MyRagdollAnimWeightBlendingHelper m_animationBlendingHelper = new MyRagdollAnimWeightBlendingHelper();

		/// <summary>
		/// Dictionary map of rigid body indices to the corresponding list of the bones binded to the specific rigid body
		/// </summary>
		private Dictionary<int, List<int>> m_rigidBodiesToBonesIndices;

		/// <summary>
		/// Character associated with this ragdoll
		/// </summary>
		private MyCharacter m_character;

		private MyAnimationControllerComponent m_animController;

		/// <summary>
		/// List for storing matrices between method calls
		/// </summary>
		private Matrix[] m_ragdollRigidBodiesAbsoluteTransforms;

		/// <summary>
		/// Rigid Body transform matrices as on load
		/// </summary>
		public Matrix[] BodiesRigTransfoms;

		/// <summary>
		/// Bodies absolute rig transforms
		/// </summary>
		public Matrix[] BonesRigTransforms;

		/// <summary>
		/// Bodies rig tranfsforms inverted
		/// </summary>
		public Matrix[] BodiesRigTransfomsInverted;

		/// <summary>
		/// Bones absolute rig transforms inverted
		/// </summary>
		public Matrix[] BonesRigTransformsInverted;

		/// <summary>
		/// List of rig transfomrs from ragdoll rigid body to a correspondig bone. This is used to eliminate artiffacts, when rigid bodie's shape centers are not always aligned with the bones.
		/// </summary>
		private Matrix[] m_bodyToBoneRigTransforms;

		/// <summary>
		/// Inversion of m_bodyToBoneRigTransforms
		/// </summary>
		private Matrix[] m_boneToBodyRigTransforms;

		/// <summary>
		/// Dictionary of rigid body names to their indices
		/// </summary>
		private Dictionary<string, int> m_rigidBodies;

		/// <summary>
		/// true if position of ragdoll changed after simulation
		/// </summary>
		public bool PositionChanged;

<<<<<<< HEAD
		/// <summary>
		/// Indicates whether this mapper was initialized
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool m_initialized;

		/// <summary>
		/// List of Ragdoll Rigid Bodies indices which are set to be keyframed in simulation
		/// </summary>
		private List<int> m_keyframedBodies;

		/// <summary>
		/// List of Ragdoll Rigid Bodies indices which are set to be dynamic in simulation
		/// </summary>
		private List<int> m_dynamicBodies;

		/// <summary>
		/// List of mappings bodies to bones list
		/// </summary>
		private Dictionary<string, MyCharacterDefinition.RagdollBoneSet> m_ragdollBonesMappings;

		private MatrixD m_lastSyncedWorldMatrix = MatrixD.Identity;

		public float DeactivationCounter = 10f;

		private bool m_changed;

		/// <summary>
		/// Reference to bones which are used for mapping
		/// </summary>
		private MyCharacterBone[] m_bones => m_animController.CharacterBones;

		/// <summary>
		/// Indicates whether ragdoll associated with this mapper was set to keyframed or not
		/// </summary>
		public bool IsKeyFramed
		{
			get
			{
				if (Ragdoll == null)
				{
					return false;
				}
				return Ragdoll.IsKeyframed;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// True if at least some of the bones are simulated and ragdoll was added to world. Partly = some bodies set to dynamic, some keyframed
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsPartiallySimulated { get; private set; }

		/// <summary>
		/// Dictionary of rigid bodies indices to the list of bones that should be bound to these bodies
		/// </summary>
		public Dictionary<int, List<int>> RigidBodiesToBonesIndices => m_rigidBodiesToBonesIndices;

<<<<<<< HEAD
		/// <summary>
		/// True if the mapper was set to be active and can be used
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsActive { get; private set; }

		public HkRagdoll Ragdoll
		{
			get
			{
				if (m_character == null)
				{
					return null;
				}
				if (m_character.Physics == null)
				{
					return null;
				}
				return m_character.Physics.Ragdoll;
			}
		}

		/// <summary>
		/// Constructs the new mapper
		/// </summary>
		/// <param name="character"></param>        
		/// <param name="controller">Animation controller on the character.</param>
		public MyRagdollMapper(MyCharacter character, MyAnimationControllerComponent controller)
		{
			m_rigidBodiesToBonesIndices = new Dictionary<int, List<int>>();
			m_character = character;
			m_animController = controller;
			m_rigidBodies = new Dictionary<string, int>();
			m_keyframedBodies = new List<int>();
			m_dynamicBodies = new List<int>();
			IsActive = false;
			m_initialized = false;
			IsPartiallySimulated = false;
		}

		public int BodyIndex(string bodyName)
		{
			if (m_rigidBodies.TryGetValue(bodyName, out var value))
			{
				return value;
			}
			return 0;
		}

		/// <summary>
		/// Initializes the mapper
		/// </summary>
		/// <param name="ragdollBonesMappings">Dictionary containing rigid body names and the corresponding character bones names list</param>
		/// <returns></returns>
		public bool Init(Dictionary<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMappings)
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.Init");
			}
			m_ragdollBonesMappings = ragdollBonesMappings;
			foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMapping in ragdollBonesMappings)
			{
				try
				{
					string key = ragdollBonesMapping.Key;
					string[] bones = ragdollBonesMapping.Value.Bones;
					List<int> list = new List<int>();
					int num = Ragdoll.FindRigidBodyIndex(key);
					string[] array = bones;
					foreach (string bone in array)
					{
						int num2 = Array.FindIndex(m_bones, (MyCharacterBone x) => x.Name == bone);
						if (m_bones.IsValidIndex(num2))
						{
							list.Add(num2);
							continue;
						}
						return false;
					}
					if (Ragdoll.RigidBodies.IsValidIndex(num))
					{
						AddRigidBodyToBonesMap(num, list, key);
						continue;
					}
					return false;
				}
				catch (Exception)
				{
					return false;
				}
			}
			InitRigTransforms();
			m_initialized = true;
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.Init FINISHED");
			}
			return true;
		}

		/// <summary>
		/// This computes the rig transforms from rigid bodies to ragdoll bones. Must be called before any transforms are made on rigid bodies!
		/// </summary>
		private void InitRigTransforms()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.InitRigTransforms");
			}
			m_ragdollRigidBodiesAbsoluteTransforms = new Matrix[Ragdoll.RigidBodies.Count];
			m_bodyToBoneRigTransforms = new Matrix[Ragdoll.RigidBodies.Count];
			m_boneToBodyRigTransforms = new Matrix[Ragdoll.RigidBodies.Count];
			BodiesRigTransfoms = new Matrix[Ragdoll.RigidBodies.Count];
			BodiesRigTransfomsInverted = new Matrix[Ragdoll.RigidBodies.Count];
			foreach (int key in m_rigidBodiesToBonesIndices.Keys)
			{
				Matrix absoluteRigTransform = m_bones[Enumerable.First<int>((IEnumerable<int>)m_rigidBodiesToBonesIndices[key])].GetAbsoluteRigTransform();
				Matrix matrix = Ragdoll.RigTransforms[key];
				Matrix matrix2 = absoluteRigTransform * Matrix.Invert(matrix);
				Matrix matrix3 = matrix * Matrix.Invert(absoluteRigTransform);
				m_bodyToBoneRigTransforms[key] = matrix2;
				m_boneToBodyRigTransforms[key] = matrix3;
				BodiesRigTransfoms[key] = matrix;
				BodiesRigTransfomsInverted[key] = Matrix.Invert(matrix);
			}
			BonesRigTransforms = new Matrix[m_bones.Length];
			BonesRigTransformsInverted = new Matrix[m_bones.Length];
			for (int i = 0; i < BonesRigTransforms.Length; i++)
			{
				BonesRigTransforms[i] = m_bones[i].GetAbsoluteRigTransform();
				BonesRigTransformsInverted[i] = Matrix.Invert(m_bones[i].GetAbsoluteRigTransform());
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.InitRigTransforms - END");
			}
		}

		/// <summary>
		/// Adds mappings
		/// </summary>
		/// <param name="rigidBodyIndex"></param>
		/// <param name="bonesIndices"></param>
		/// <param name="rigidBodyName"></param>
		private void AddRigidBodyToBonesMap(int rigidBodyIndex, List<int> bonesIndices, string rigidBodyName)
		{
			foreach (int bonesIndex in bonesIndices)
			{
				_ = bonesIndex;
			}
			m_rigidBodiesToBonesIndices.Add(rigidBodyIndex, bonesIndices);
			m_rigidBodies.Add(rigidBodyName, rigidBodyIndex);
		}

		/// <summary>
		/// Sets the pose of the ragdoll to match the pose of the bones
		/// </summary>        
		public void UpdateRagdollPose()
		{
			if (Ragdoll != null && m_initialized && IsActive)
			{
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPose");
				}
				CalculateRagdollTransformsFromBones();
				UpdateRagdollRigidBodies();
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPose - END");
				}
			}
		}

		/// <summary>
		/// Compute the transforms that should be used for rigid bodies based on current bones tranforms in the havokWorld
		/// </summary>        
		private void CalculateRagdollTransformsFromBones()
		{
			if (Ragdoll == null || !m_initialized || !IsActive)
			{
				return;
			}
			foreach (int key in m_rigidBodiesToBonesIndices.Keys)
			{
				_ = Ragdoll.RigidBodies[key];
<<<<<<< HEAD
				List<int> source = m_rigidBodiesToBonesIndices[key];
				Matrix absoluteTransform = m_bones[source.First()].AbsoluteTransform;
=======
				List<int> list = m_rigidBodiesToBonesIndices[key];
				Matrix absoluteTransform = m_bones[Enumerable.First<int>((IEnumerable<int>)list)].AbsoluteTransform;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_ragdollRigidBodiesAbsoluteTransforms[key] = absoluteTransform;
			}
		}

		/// <summary>
		/// Set the ragdolls rigid bodies to computed transforms
		/// </summary>
		private void UpdateRagdollRigidBodies()
		{
			if (Ragdoll == null || !m_initialized || !IsActive)
			{
				return;
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollRigidBodies");
			}
			foreach (int keyframedBody in m_keyframedBodies)
			{
				_ = Ragdoll.RigidBodies[keyframedBody];
				if (m_ragdollRigidBodiesAbsoluteTransforms[keyframedBody].IsValid() && m_ragdollRigidBodiesAbsoluteTransforms[keyframedBody] != Matrix.Zero)
				{
					Matrix matrix = m_boneToBodyRigTransforms[keyframedBody] * m_ragdollRigidBodiesAbsoluteTransforms[keyframedBody];
					Quaternion quaternion = Quaternion.CreateFromRotationMatrix(matrix.GetOrientation());
					Vector3 translation = matrix.Translation;
					quaternion.Normalize();
					matrix = Matrix.CreateFromQuaternion(quaternion);
					matrix.Translation = translation;
					Ragdoll.SetRigidBodyLocalTransform(keyframedBody, matrix);
				}
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollRigidBodies - END");
			}
		}

		/// <summary>
		/// Sets the character's bones to the ragdoll's pose
		/// </summary>
		/// <param name="dynamicBodiesWeight">transform influence weight on dynamic bodies</param>
		/// <param name="keyframedBodiesWeight"></param>
		public void UpdateCharacterPose(float dynamicBodiesWeight = 1f, float keyframedBodiesWeight = 1f)
		{
			if (m_initialized && IsActive && (m_animationBlendingHelper.Initialized || m_animationBlendingHelper.Init(m_bones, m_character.AnimationController.Controller)))
			{
				m_animationBlendingHelper.Prepare(m_character.AnimationController.Controller.Variables);
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateCharacterPose");
				}
				float weight = dynamicBodiesWeight;
				if (m_keyframedBodies.Contains(Ragdoll.m_ragdollTree.m_rigidBodyIndex))
				{
					weight = keyframedBodiesWeight;
				}
				SetBoneTo(Ragdoll.m_ragdollTree, weight, dynamicBodiesWeight, keyframedBodiesWeight, translationEnabled: false);
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateCharacterPose - END");
				}
			}
		}

		private void SetBoneTo(RagdollBone ragdollBone, float weight, float dynamicChildrenWeight, float keyframedChildrenWeight, bool translationEnabled)
		{
			if (Ragdoll == null || !m_initialized || !IsActive)
			{
				return;
			}
			int characterBoneIndex = GetCharacterBoneIndex(ragdollBone);
			MyCharacterBone myCharacterBone = m_bones[characterBoneIndex];
			if (ragdollBone.m_parent != null)
			{
				RecalculateTransformsUpwardRecursive(ragdollBone, myCharacterBone);
			}
			Ragdoll.GetRigidBodyLocalTransform(ragdollBone.m_rigidBodyIndex, out var transform);
			Matrix.Multiply(ref m_bodyToBoneRigTransforms[ragdollBone.m_rigidBodyIndex], ref transform, out transform);
			Matrix matrix = myCharacterBone.Parent?.AbsoluteTransform ?? Matrix.Identity;
			Matrix matrix2 = Matrix.Invert(myCharacterBone.BindTransform * matrix);
			Matrix matrix3 = transform * matrix2;
			m_animationBlendingHelper.BlendWeight(ref weight, myCharacterBone, m_character.AnimationController.Controller.Variables);
			weight *= MyFakes.RAGDOLL_ANIMATION_WEIGHTING;
			weight = MathHelper.Clamp(weight, 0f, 1f);
			if (matrix3.IsValid() && matrix3 != Matrix.Zero)
			{
				if (weight == 1f)
				{
					myCharacterBone.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrix3.GetOrientation()));
					if (translationEnabled)
					{
						myCharacterBone.Translation = matrix3.Translation;
					}
				}
				else
				{
					myCharacterBone.Rotation = Quaternion.Slerp(myCharacterBone.Rotation, Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrix3.GetOrientation())), weight);
					if (translationEnabled)
					{
						myCharacterBone.Translation = Vector3.Lerp(myCharacterBone.Translation, matrix3.Translation, weight);
					}
				}
			}
			myCharacterBone.ComputeAbsoluteTransform(ragdollBone.m_children.Count == 0);
			foreach (RagdollBone child in ragdollBone.m_children)
			{
				float weight2 = dynamicChildrenWeight;
				if (m_keyframedBodies.Contains(child.m_rigidBodyIndex))
				{
					weight2 = keyframedChildrenWeight;
				}
				if (IsPartiallySimulated)
				{
					SetBoneTo(child, weight2, dynamicChildrenWeight, keyframedChildrenWeight, translationEnabled: false);
				}
				else
				{
					SetBoneTo(child, weight2, dynamicChildrenWeight, keyframedChildrenWeight, !Ragdoll.IsRigidBodyPalmOrFoot(child.m_rigidBodyIndex) && MyFakes.ENABLE_RAGDOLL_BONES_TRANSLATION);
				}
			}
		}

		private void RecalculateTransformsUpwardRecursive(RagdollBone ragdollBone, MyCharacterBone bone)
		{
			MyCharacterBone parent = bone.Parent;
			if (parent != null && parent.Index != GetCharacterBoneIndex(ragdollBone.m_parent))
			{
				RecalculateTransformsUpwardRecursive(ragdollBone, parent);
				parent.ComputeAbsoluteTransform(propagateTransformToChildren: false, force: true);
			}
		}

		private int GetCharacterBoneIndex(RagdollBone rb)
		{
			return m_rigidBodiesToBonesIndices[rb.m_rigidBodyIndex][0];
		}

		public void Activate()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.Activate");
			}
			if (Ragdoll == null)
			{
				IsActive = false;
				return;
			}
			IsActive = true;
			HkRagdoll ragdoll = m_character.Physics.Ragdoll;
			ragdoll.AddedToWorld = (Action<HkRagdoll>)Delegate.Remove(ragdoll.AddedToWorld, new Action<HkRagdoll>(OnRagdollAdded));
			HkRagdoll ragdoll2 = m_character.Physics.Ragdoll;
			ragdoll2.AddedToWorld = (Action<HkRagdoll>)Delegate.Combine(ragdoll2.AddedToWorld, new Action<HkRagdoll>(OnRagdollAdded));
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.Activate - END");
			}
		}

		public void Deactivate()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.Deactivate");
			}
			if (IsPartiallySimulated)
			{
				DeactivatePartialSimulation();
			}
			IsActive = false;
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.Deactivate -END");
			}
		}

		public void SetRagdollToKeyframed()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToKeyframed");
			}
			if (Ragdoll != null)
			{
				Ragdoll.SetToKeyframed();
				m_dynamicBodies.Clear();
				m_keyframedBodies.Clear();
				m_keyframedBodies.AddRange(m_rigidBodies.Values);
				IsPartiallySimulated = false;
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToKeyframed - END");
				}
			}
		}

		public void SetRagdollToDynamic()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToDynamic");
			}
			if (Ragdoll != null)
			{
				Ragdoll.SetToDynamic();
				m_keyframedBodies.Clear();
				m_dynamicBodies.Clear();
				m_dynamicBodies.AddRange(m_rigidBodies.Values);
				IsPartiallySimulated = false;
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToDynamic - END");
				}
			}
		}

		public List<int> GetBodiesBindedToBones(List<string> bones)
		{
			List<int> list = new List<int>();
			foreach (string bone in bones)
			{
				foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMapping in m_ragdollBonesMappings)
				{
					if (ragdollBonesMapping.Value.Bones.Contains(bone) && !list.Contains(m_rigidBodies[ragdollBonesMapping.Key]))
					{
						list.Add(m_rigidBodies[ragdollBonesMapping.Key]);
					}
				}
			}
			return list;
		}

		public void ActivatePartialSimulation(List<int> dynamicRigidBodies = null)
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.ActivatePartialSimulation");
			}
			if (m_initialized && Ragdoll != null && !IsPartiallySimulated)
			{
				if (dynamicRigidBodies != null)
				{
					m_dynamicBodies.Clear();
					m_dynamicBodies.AddRange(dynamicRigidBodies);
					m_keyframedBodies.Clear();
					m_keyframedBodies.AddRange(Enumerable.Except<int>((IEnumerable<int>)m_rigidBodies.Values, (IEnumerable<int>)dynamicRigidBodies));
				}
				m_animationBlendingHelper.ResetWeights();
				SetBodiesSimulationMode();
				if (Ragdoll.InWorld)
				{
					Ragdoll.EnableConstraints();
					Ragdoll.Activate();
				}
				IsActive = true;
				IsPartiallySimulated = true;
				UpdateRagdollPose();
				SetVelocities();
				HkRagdoll ragdoll = m_character.Physics.Ragdoll;
				ragdoll.AddedToWorld = (Action<HkRagdoll>)Delegate.Remove(ragdoll.AddedToWorld, new Action<HkRagdoll>(OnRagdollAdded));
				HkRagdoll ragdoll2 = m_character.Physics.Ragdoll;
				ragdoll2.AddedToWorld = (Action<HkRagdoll>)Delegate.Combine(ragdoll2.AddedToWorld, new Action<HkRagdoll>(OnRagdollAdded));
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.ActivatePartialSimulation - END");
				}
			}
		}

		private void SetBodiesSimulationMode()
		{
			foreach (int dynamicBody in m_dynamicBodies)
			{
				Ragdoll.SetToDynamic(dynamicBody);
				Ragdoll.SwitchRigidBodyToLayer(dynamicBody, 31);
			}
			foreach (int keyframedBody in m_keyframedBodies)
			{
				Ragdoll.SetToKeyframed(keyframedBody);
				Ragdoll.SwitchRigidBodyToLayer(keyframedBody, 31);
			}
		}

		private void OnRagdollAdded(HkRagdoll ragdoll)
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			if (IsPartiallySimulated)
			{
				SetBodiesSimulationMode();
			}
		}

		public void DeactivatePartialSimulation()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.DeactivatePartialSimulation");
			}
			if (IsPartiallySimulated && Ragdoll != null)
			{
				if (Ragdoll.InWorld)
				{
					Ragdoll.DisableConstraints();
					Ragdoll.Deactivate();
				}
				m_keyframedBodies.Clear();
				m_dynamicBodies.Clear();
				m_dynamicBodies.AddRange(m_rigidBodies.Values);
				SetBodiesSimulationMode();
				Ragdoll.ResetToRigPose();
				IsPartiallySimulated = false;
				IsActive = false;
				HkRagdoll ragdoll = m_character.Physics.Ragdoll;
				ragdoll.AddedToWorld = (Action<HkRagdoll>)Delegate.Remove(ragdoll.AddedToWorld, new Action<HkRagdoll>(OnRagdollAdded));
				m_animationBlendingHelper.ResetWeights();
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.DeactivatePartialSimulation - END");
				}
			}
		}

		public void DebugDraw(MatrixD worldMatrix)
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_ORIGINAL_RIG)
			{
				foreach (int key in m_rigidBodiesToBonesIndices.Keys)
				{
					MatrixD m = BodiesRigTransfoms[key] * worldMatrix;
					MyRenderProxy.DebugDrawSphere(((Matrix)m).Translation, 0.03f, Color.White, 0.1f, depthRead: false);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_ORIGINAL_RIG)
			{
				foreach (int key2 in m_rigidBodiesToBonesIndices.Keys)
				{
					MatrixD m = m_bodyToBoneRigTransforms[key2] * BodiesRigTransfoms[key2] * worldMatrix;
					_ = (Matrix)m;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_DESIRED)
			{
				foreach (int key3 in m_rigidBodiesToBonesIndices.Keys)
				{
					MatrixD m = m_bodyToBoneRigTransforms[key3] * Ragdoll.GetRigidBodyLocalTransform(key3) * worldMatrix;
					MyRenderProxy.DebugDrawSphere(((Matrix)m).Translation, 0.035f, Color.Blue, 0.8f, depthRead: false);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_COMPUTED_BONES)
			{
				MyCharacterBone[] bones = m_bones;
				for (int i = 0; i < bones.Length; i++)
				{
					MatrixD m = bones[i].AbsoluteTransform * worldMatrix;
					MyRenderProxy.DebugDrawSphere(((Matrix)m).Translation, 0.03f, Color.Red, 0.8f, depthRead: false);
				}
			}
			if (!MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_POSE)
			{
				return;
			}
			foreach (int key4 in m_rigidBodiesToBonesIndices.Keys)
			{
				Color color = new Color((key4 & 1) * 255, (key4 & 2) * 255, (key4 & 4) * 255);
				Matrix m2 = Ragdoll.GetRigidBodyLocalTransform(key4);
				MatrixD matrixD = (MatrixD)m2 * worldMatrix;
				DrawShape(Ragdoll.RigidBodies[key4].GetShape(), matrixD, color, 0.6f);
				MyRenderProxy.DebugDrawAxis(matrixD, 0.3f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(matrixD.Translation, 0.03f, Color.Green, 0.8f, depthRead: false);
			}
		}

		/// <summary>
		/// Update Ragdoll position in the Havok world to copy the Physics position
		/// </summary>
		public void UpdateRagdollPosition()
		{
			if (Ragdoll == null || !m_initialized || !IsActive || (!IsPartiallySimulated && !IsKeyFramed))
			{
				return;
			}
			Matrix matrix;
			if (m_character.IsDead)
			{
				MatrixD m = m_character.WorldMatrix;
				matrix = m;
				matrix.Translation = m_character.Physics.WorldToCluster(m_character.WorldMatrix.Translation);
				if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
				}
			}
			else
			{
				MatrixD m = m_character.Physics.GetWorldMatrix();
				matrix = m;
				matrix.Translation = m_character.Physics.WorldToCluster(m_character.WorldMatrix.Translation);
			}
			if (!matrix.IsValid() || !(matrix != Matrix.Zero))
			{
				return;
			}
			double num = (matrix.Translation - Ragdoll.WorldMatrix.Translation).LengthSquared();
			double num2 = (matrix.Forward - Ragdoll.WorldMatrix.Forward).LengthSquared();
			double num3 = (matrix.Up - Ragdoll.WorldMatrix.Up).LengthSquared();
			m_changed = num > 1.0000000116860974E-07 || num2 > 1.0000000116860974E-07 || num3 > 1.0000000116860974E-07;
			if (num > 10.0 || m_character.m_positionResetFromServer)
			{
				m_character.m_positionResetFromServer = false;
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPosition");
				}
				Ragdoll.SetWorldMatrix(matrix);
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			}
			else if (m_changed)
			{
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPosition");
				}
				Ragdoll.SetWorldMatrix(matrix, updateOnlyKeyframed: true, updateVelocities: false);
			}
		}

		public static void DrawShape(HkShape shape, MatrixD worldMatrix, Color color, float alpha, bool shaded = true)
		{
			color.A = (byte)(alpha * 255f);
			HkShapeType shapeType = shape.ShapeType;
			if (shapeType == HkShapeType.Capsule)
			{
				HkCapsuleShape hkCapsuleShape = (HkCapsuleShape)shape;
				Vector3 vector = Vector3.Transform(hkCapsuleShape.VertexA, worldMatrix);
				MyRenderProxy.DebugDrawCapsule(p1: (Vector3)Vector3.Transform(hkCapsuleShape.VertexB, worldMatrix), p0: vector, radius: hkCapsuleShape.Radius, color: color, depthRead: false);
			}
			else
			{
				MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, 0.05f, color, 1f, depthRead: false);
			}
		}

		public void SetVelocities(bool onlyKeyframed = false, bool onlyIfChanged = false)
		{
			if (m_initialized && IsActive && m_character != null && m_character.Physics != null)
			{
				MyPhysicsBody physics = m_character.Physics;
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
				if (m_changed || !onlyIfChanged)
				{
					physics.SetRagdollVelocities(onlyKeyframed ? m_keyframedBodies : null);
				}
			}
		}

		public void SetLimitedVelocities()
		{
			List<HkRigidBody> rigidBodies = Ragdoll.RigidBodies;
			if (rigidBodies[0] == null)
			{
				return;
			}
			HkRigidBody rigidBody = m_character.Physics.RigidBody;
			float maxLinearVelocity;
			float maxAngularVelocity;
			if (rigidBody != null)
			{
				maxLinearVelocity = rigidBody.MaxLinearVelocity + 5f;
				maxAngularVelocity = rigidBody.MaxAngularVelocity + 1f;
			}
			else
			{
				maxLinearVelocity = Math.Max(10f, rigidBodies[0].LinearVelocity.Length() + 5f);
				maxAngularVelocity = Math.Max((float)Math.PI * 4f, rigidBodies[0].AngularVelocity.Length() + 1f);
			}
			foreach (int dynamicBody in m_dynamicBodies)
			{
				if (IsPartiallySimulated)
				{
					rigidBodies[dynamicBody].MaxLinearVelocity = maxLinearVelocity;
					rigidBodies[dynamicBody].MaxAngularVelocity = maxAngularVelocity;
					rigidBodies[dynamicBody].LinearDamping = 0.2f;
					rigidBodies[dynamicBody].AngularDamping = 0.2f;
				}
				else
				{
					rigidBodies[dynamicBody].MaxLinearVelocity = Ragdoll.MaxLinearVelocity;
					rigidBodies[dynamicBody].MaxAngularVelocity = Ragdoll.MaxAngularVelocity;
					rigidBodies[dynamicBody].LinearDamping = 0.5f;
					rigidBodies[dynamicBody].AngularDamping = 0.5f;
				}
			}
		}

		public void UpdateRagdollAfterSimulation()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollAfterSimulation");
			}
			if (m_initialized && IsActive && Ragdoll != null && Ragdoll.InWorld)
			{
				Matrix worldMatrix = Ragdoll.WorldMatrix;
				Ragdoll.UpdateWorldMatrixAfterSimulation();
				Ragdoll.UpdateLocalTransforms();
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
				PositionChanged = worldMatrix != Ragdoll.WorldMatrix;
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollAfterSimulation - END");
				}
			}
		}

		internal void UpdateRigidBodiesTransformsSynced(int transformsCount, Matrix worldMatrix, Matrix[] transforms)
		{
			if (!m_initialized || !IsActive || Ragdoll == null || !Ragdoll.InWorld)
			{
				return;
			}
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			if (transformsCount == m_ragdollRigidBodiesAbsoluteTransforms.Length)
			{
				for (int i = 0; i < transformsCount; i++)
				{
					list.Add(Ragdoll.RigidBodies[i].LinearVelocity);
					list2.Add(Ragdoll.RigidBodies[i].AngularVelocity);
					Ragdoll.SetRigidBodyLocalTransform(i, transforms[i]);
				}
			}
			MatrixD m = worldMatrix;
			m.Translation = m_character.Physics.WorldToCluster(worldMatrix.Translation);
			Ragdoll.SetWorldMatrix(m, updateOnlyKeyframed: false, updateVelocities: false);
			foreach (int key in m_rigidBodiesToBonesIndices.Keys)
			{
				Ragdoll.RigidBodies[key].LinearVelocity = list[key];
				Ragdoll.RigidBodies[key].AngularVelocity = list2[key];
			}
		}

		public void SyncRigidBodiesTransforms(MatrixD worldTransform)
		{
			bool flag = m_lastSyncedWorldMatrix != worldTransform;
			foreach (int key in m_rigidBodiesToBonesIndices.Keys)
			{
				_ = Ragdoll.RigidBodies[key];
				Matrix rigidBodyLocalTransform = Ragdoll.GetRigidBodyLocalTransform(key);
				flag = m_ragdollRigidBodiesAbsoluteTransforms[key] != rigidBodyLocalTransform || flag;
				m_ragdollRigidBodiesAbsoluteTransforms[key] = rigidBodyLocalTransform;
			}
			if (flag && MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC)
			{
				m_character.SendRagdollTransforms(worldTransform, m_ragdollRigidBodiesAbsoluteTransforms);
				m_lastSyncedWorldMatrix = worldTransform;
			}
		}

		public HkRigidBody GetBodyBindedToBone(MyCharacterBone myCharacterBone)
		{
			if (Ragdoll == null)
			{
				return null;
			}
			if (myCharacterBone == null)
			{
				return null;
			}
			foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMapping in m_ragdollBonesMappings)
			{
				if (ragdollBonesMapping.Value.Bones.Contains(myCharacterBone.Name))
				{
					return Ragdoll.RigidBodies[m_rigidBodies[ragdollBonesMapping.Key]];
				}
			}
			return null;
		}
	}
}
