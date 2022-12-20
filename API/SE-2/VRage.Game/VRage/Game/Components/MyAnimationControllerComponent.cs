using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Game.Entity;
using VRage.Game.SessionComponents;
using VRage.Generics;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace VRage.Game.Components
{
	/// <summary>
	/// Entity component containing animation controller.
	/// </summary>
	public class MyAnimationControllerComponent : MyEntityComponentBase
	{
		private class CopyOnWriteStorage<T> : IMyVariableStorage<T>, IEnumerable<KeyValuePair<MyStringId, T>>, IEnumerable
		{
			public IMyVariableStorage<T> Read { get; }

			public IMyVariableStorage<T> Write { get; }

			public CopyOnWriteStorage(IMyVariableStorage<T> read, IMyVariableStorage<T> write)
			{
				Read = read;
				Write = write;
			}

			public void SetValue(MyStringId key, T newValue)
			{
				Write.SetValue(key, newValue);
			}

			public bool GetValue(MyStringId key, out T value)
			{
				if (Write.GetValue(key, out value))
				{
					return true;
				}
				return Read.GetValue(key, out value);
			}

			public IEnumerator<KeyValuePair<MyStringId, T>> GetEnumerator()
			{
				foreach (KeyValuePair<MyStringId, T> item in Write)
				{
					yield return item;
				}
				foreach (KeyValuePair<MyStringId, T> item2 in Read)
				{
					if (!Write.GetValue(item2.Key, out var _))
					{
						yield return item2;
					}
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct BoneDataPack
		{
			public readonly MyCharacterBone[] Bones;

			public readonly MyCharacterBone[] SortedBones;

			public readonly Matrix[] RelativeTransforms;

			public readonly Matrix[] AbsoluteTransforms;

			public int Length => Bones.Length;

			public BoneDataPack(in BoneDataPack pack)
			{
				Bones = new MyCharacterBone[pack.Length];
				SortedBones = new MyCharacterBone[pack.Length];
				RelativeTransforms = new Matrix[pack.Length];
				Array.Copy(pack.RelativeTransforms, RelativeTransforms, pack.Length);
				AbsoluteTransforms = new Matrix[pack.Length];
				Array.Copy(pack.AbsoluteTransforms, AbsoluteTransforms, pack.Length);
				Dictionary<MyCharacterBone, int> index = new Dictionary<MyCharacterBone, int>();
				for (int i = 0; i < pack.Length; i++)
				{
					index[pack.SortedBones[i]] = i;
				}
				MyCharacterBone[] sortedBones = pack.SortedBones;
				MyCharacterBone[] copies = SortedBones;
				for (int j = 0; j < pack.Length; j++)
				{
					copies[j] = new MyCharacterBone(sortedBones[j].Name, Get(sortedBones[j].Parent), sortedBones[j].BindTransform, sortedBones[j].Index, RelativeTransforms, AbsoluteTransforms);
				}
				Array.Copy(SortedBones, Bones, Bones.Length);
				Array.Sort(Bones, (MyCharacterBone x, MyCharacterBone y) => x.Index.CompareTo(y.Index));
				MyCharacterBone Get(MyCharacterBone sourceBone)
				{
					if (sourceBone != null && index.TryGetValue(sourceBone, out var value))
					{
						return copies[value];
					}
					return null;
				}
			}

			public BoneDataPack(MyCharacterBone[] bones, Matrix[] relativeTransforms, Matrix[] absoluteTransforms)
			{
				Bones = bones;
				RelativeTransforms = relativeTransforms;
				AbsoluteTransforms = absoluteTransforms;
				SortedBones = new MyCharacterBone[bones.Length];
				Array.Copy(bones, SortedBones, SortedBones.Length);
				SortBones(SortedBones);
			}

			private static void SortBones(MyCharacterBone[] bones)
			{
				Array.Sort(bones, (MyCharacterBone x, MyCharacterBone y) => x.Depth.CompareTo(y.Depth));
			}

			public void CopyMatricesTo(in BoneDataPack pack)
			{
				Array.Copy(RelativeTransforms, pack.RelativeTransforms, pack.Length);
				Array.Copy(AbsoluteTransforms, pack.AbsoluteTransforms, pack.Length);
			}

			public void CopyTransformsTo(in BoneDataPack pack)
			{
				for (int i = 0; i < Bones.Length; i++)
				{
					pack.Bones[i].Rotation = Bones[i].Rotation;
					pack.Bones[i].Translation = Bones[i].Translation;
					pack.Bones[i].ComputeAbsoluteTransform(propagateTransformToChildren: false);
				}
			}
		}

		private class VRage_Game_Components_MyAnimationControllerComponent_003C_003EActor : IActivator, IActivator<MyAnimationControllerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyAnimationControllerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAnimationControllerComponent CreateInstance()
			{
				return new MyAnimationControllerComponent();
			}

			MyAnimationControllerComponent IActivator<MyAnimationControllerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly MyAnimationController m_controller = new MyAnimationController();

		private List<MyAnimationClip.BoneState> m_lastBoneResult;

		private BoneDataPack m_boneData;

		private bool m_componentValid;

		public readonly Action ReloadBonesNeeded;

		public float CameraDistance;

		private MyEntity m_entity;

		public float OtherLayersAnimationSpeed = 1f;

		public float MainLayerAnimationSpeed = 1f;

		/// <summary>
		/// Name of the component type for debug purposes (e.g.: "Position")
		/// </summary>
		public override string ComponentTypeDebugString => "AnimationControllerComp";

		/// <summary>
		/// Get the animation controller instance.
		/// </summary>
		public MyAnimationController Controller => m_controller;

		/// <summary>
		/// Get the variable storage of animation controller instance. Shortcut.
		/// </summary>
		public IMyVariableStorage<float> Variables => m_controller.Variables;

		/// <summary>
		/// Get reference to array of character pack and its contents.
		/// </summary>
		public MyCharacterBone[] CharacterBones => m_boneData.Bones;

		/// <summary>
		/// Get the instance of inverse kinematics.
		/// </summary>
		public MyAnimationInverseKinematics InverseKinematics => m_controller.InverseKinematics;

		public MyCharacterBone[] CharacterBonesSorted => m_boneData.SortedBones;

		public Matrix[] BoneRelativeTransforms => m_boneData.RelativeTransforms;

		public Matrix[] BoneAbsoluteTransforms => m_boneData.AbsoluteTransforms;

		public List<MyAnimationClip.BoneState> LastRawBoneResult => m_lastBoneResult;

		public MyDefinitionId SourceId { get; set; }

		public List<MyStringId> LastFrameActions
		{
			get
			{
				_ = m_entity.InScene;
				return null;
			}
		}

		public event Action<MyStringId> ActionTriggered;

		/// <summary>
		/// Component was added in the entity component container.
		/// </summary>
		public override void OnAddedToContainer()
		{
			_ = m_entity.InScene;
			MySessionComponentAnimationSystem.Static.RegisterEntityComponent(this);
		}

		/// <summary>
		/// Component will be removed from entity component container.
		/// </summary>
		public override void OnBeforeRemovedFromContainer()
		{
			_ = m_entity.InScene;
			MySessionComponentAnimationSystem.Static.UnregisterEntityComponent(this);
		}

		public List<MyAnimationTreeNode> GetKeyedAnimationTracks(string key)
		{
			return m_controller.GetKeyedTracks(key);
		}

		public void MarkAsValid()
		{
			_ = m_entity.InScene;
			m_componentValid = true;
		}

		private void MarkAsInvalid()
		{
			m_componentValid = false;
		}

		private MyAnimationControllerComponent()
		{
		}

		public MyAnimationControllerComponent(MyEntity entity, Action obtainBones, IMyTerrainHeightProvider heightProvider)
		{
			m_entity = entity;
			ReloadBonesNeeded = obtainBones;
			m_controller.InverseKinematics.TerrainHeightProvider = heightProvider;
		}

		public void AttachAnimationEventCallback(Action<List<string>> action)
		{
			MyAnimationController controller = m_controller;
			controller.OnAnimationEventTriggered = (Action<List<string>>)Delegate.Combine(controller.OnAnimationEventTriggered, action);
		}

		public void DetachAnimationEventCallback(Action<List<string>> action)
		{
			MyAnimationController controller = m_controller;
			controller.OnAnimationEventTriggered = (Action<List<string>>)Delegate.Remove(controller.OnAnimationEventTriggered, action);
		}

		public void AttachAnimationEventCallback(Action<List<string>> action)
		{
			MyAnimationController controller = m_controller;
			controller.OnAnimationEventTriggered = (Action<List<string>>)Delegate.Combine(controller.OnAnimationEventTriggered, action);
		}

		public void DetachAnimationEventCallback(Action<List<string>> action)
		{
			MyAnimationController controller = m_controller;
			controller.OnAnimationEventTriggered = (Action<List<string>>)Delegate.Remove(controller.OnAnimationEventTriggered, action);
		}

		public void SetCharacterBones(MyCharacterBone[] characterBones, Matrix[] relativeTransforms, Matrix[] absoluteTransforms)
		{
			_ = m_entity.InScene;
			m_controller.ResultBonesPool.Reset(characterBones);
			m_boneData = new BoneDataPack(characterBones, relativeTransforms, absoluteTransforms);
		}

		public bool Update()
		{
			if (CameraDistance > 200f)
			{
				return false;
			}
			if (!m_componentValid)
			{
				return false;
			}
			if (!(base.Entity is IMySkinnedEntity))
			{
				return false;
			}
			if (!base.Entity.InScene)
			{
				return false;
			}
			MyAnimationUpdateData animationUpdateData = default(MyAnimationUpdateData);
			animationUpdateData.DeltaTimeInSeconds = 0.0166666675f * OtherLayersAnimationSpeed;
			animationUpdateData.DeltaTimeInSecondsForMainLayer = 0.0166666675f * MainLayerAnimationSpeed;
<<<<<<< HEAD
			animationUpdateData.CharacterBones = m_boneDataSwap.Bones;
=======
			animationUpdateData.CharacterBones = m_boneData.Bones;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			animationUpdateData.Controller = null;
			animationUpdateData.BonesResult = null;
			lock (m_controller)
			{
				m_controller.Update(ref animationUpdateData);
			}
			m_lastBoneResult = animationUpdateData.BonesResult;
			_ = m_lastBoneResult;
			return true;
		}

<<<<<<< HEAD
		public void ApplyVariables()
		{
			MyAnimationVariableStorage variables = m_controller.Variables;
			foreach (KeyValuePair<MyStringId, float> item in m_variablesWrite)
			{
				variables.SetValue(item.Key, item.Value);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void FinishUpdate()
		{
			if (m_lastBoneResult != null && m_lastBoneResult.Count == m_boneData.Bones.Length)
			{
				for (int i = 0; i < m_lastBoneResult.Count; i++)
				{
					m_boneData.Bones[i].SetCompleteTransform(ref m_lastBoneResult[i].Translation, ref m_lastBoneResult[i].Rotation);
				}
			}
		}

		public void UpdateTransformations()
		{
			if (m_boneData.Bones != null)
			{
				MyCharacterBone.ComputeAbsoluteTransforms(m_boneData.SortedBones);
			}
		}

		public void UpdateInverseKinematics()
		{
			m_controller.UpdateInverseKinematics(m_boneData.Bones);
		}

		[Conditional("DEBUG")]
		private void CheckAccess()
		{
			_ = m_entity.InScene;
			if (Monitor.TryEnter(m_boneData.Bones))
			{
				Monitor.Exit(m_boneData.Bones);
			}
		}

		public MyCharacterBone FindBone(string name, out int index)
		{
			_ = m_entity.InScene;
			if (name != null)
			{
				MyCharacterBone[] bones = m_boneData.Bones;
				for (int i = 0; i < bones.Length; i++)
				{
					if (bones[i].Name == name)
					{
						index = i;
						return bones[i];
					}
				}
			}
			index = -1;
			return null;
		}

		/// <summary>
		/// Trigger an action in the layers specified. If no layers are specified, it is triggered for all instead.
		/// If there is a transition having given (non-null) name, it is followed immediatelly.
		/// Conditions of transition are ignored.
		/// This is a shortcut to Controller.TriggerAction.
		/// </summary>
		public void TriggerAction(MyStringId actionName, string[] layers = null)
		{
			_ = m_entity.InScene;
			if (m_componentValid)
			{
				if (layers != null)
				{
					m_controller.TriggerAction(actionName, layers);
				}
				else
				{
					m_controller.TriggerAction(actionName);
				}
				if (this.ActionTriggered != null)
				{
					this.ActionTriggered(actionName);
				}
			}
		}

		public void Clear()
		{
			_ = m_entity.InScene;
			MarkAsInvalid();
			m_controller.InverseKinematics.Clear();
			m_controller.DeleteAllLayers();
			m_controller.Variables.Clear();
			m_controller.ResultBonesPool.Reset(null);
		}
	}
}
