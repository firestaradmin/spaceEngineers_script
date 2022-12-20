using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	public class MySkinnedEntity : MyEntity, IMySkinnedEntity, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		private class Sandbox_Game_Entities_MySkinnedEntity_003C_003EActor : IActivator, IActivator<MySkinnedEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MySkinnedEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySkinnedEntity CreateInstance()
			{
				return new MySkinnedEntity();
			}

			MySkinnedEntity IActivator<MySkinnedEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// VRAGE TODO: THIS IS TEMPORARY! Remove when by the time we use only the new animation system.
		/// </summary>
		public bool UseNewAnimationSystem;

		private const int MAX_BONE_DECALS_COUNT = 10;

		/// <summary>
		/// Shortcut to animation controller component.
		/// </summary>
		private MyAnimationControllerComponent m_compAnimationController;

		private Dictionary<int, List<uint>> m_boneDecals = new Dictionary<int, List<uint>>();

		protected ulong m_actualUpdateFrame;

		protected ulong m_actualDrawFrame;

		protected Dictionary<string, Quaternion> m_additionalRotations = new Dictionary<string, Quaternion>();

		private Dictionary<string, MyAnimationPlayerBlendPair> m_animationPlayers = new Dictionary<string, MyAnimationPlayerBlendPair>();

		private Queue<MyAnimationCommand> m_commandQueue = new Queue<MyAnimationCommand>();

		private List<MyAnimationSetData> m_continuingAnimSets = new List<MyAnimationSetData>();

		public MyAnimationControllerComponent AnimationController => m_compAnimationController;

		public Matrix[] BoneAbsoluteTransforms => m_compAnimationController.BoneAbsoluteTransforms;

		public Matrix[] BoneRelativeTransforms => m_compAnimationController.BoneRelativeTransforms;

		public List<MyBoneDecalUpdate> DecalBoneUpdates { get; private set; }

		internal ulong ActualUpdateFrame => m_actualUpdateFrame;

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyParallelUpdateFlags UpdateFlags => base.NeedsUpdate.GetParallel() | MyParallelUpdateFlags.EACH_FRAME_PARALLEL;

		public MySkinnedEntity()
		{
			base.Render = new MyRenderComponentSkinnedEntity();
			base.Render.EnableColorMaskHsv = true;
			base.Render.NeedsDraw = true;
			base.Render.CastShadows = true;
			base.Render.NeedsResolveCastShadow = false;
			base.Render.SkipIfTooSmall = false;
			MyEntityTerrainHeightProviderComponent myEntityTerrainHeightProviderComponent = new MyEntityTerrainHeightProviderComponent();
			base.Components.Add(myEntityTerrainHeightProviderComponent);
			m_compAnimationController = new MyAnimationControllerComponent(this, ObtainBones, myEntityTerrainHeightProviderComponent);
			base.Components.Add(m_compAnimationController);
			DecalBoneUpdates = new List<MyBoneDecalUpdate>();
		}

		public override void Init(StringBuilder displayName, string model, MyEntity parentObject, float? scale, string modelCollision = null)
		{
			base.Init(displayName, model, parentObject, scale, modelCollision);
			InitBones();
		}

		protected void InitBones()
		{
			ObtainBones();
			m_animationPlayers.Clear();
			AddAnimationPlayer("", null);
		}

		public void SetBoneLODs(Dictionary<float, string[]> boneLODs)
		{
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				animationPlayer.Value.SetBoneLODs(boneLODs);
			}
		}

		public virtual void UpdateControl(float distance)
		{
		}

		public virtual void UpdateAnimation(float distance)
		{
			m_compAnimationController.CameraDistance = distance;
			if ((!MyPerGameSettings.AnimateOnlyVisibleCharacters || Sandbox.Engine.Platform.Game.IsDedicated || (base.Render != null && base.Render.RenderObjectIDs.Length != 0 && MyRenderProxy.VisibleObjectsRead != null && MyRenderProxy.VisibleObjectsRead.Contains(base.Render.RenderObjectIDs[0]))) && distance < MyFakes.ANIMATION_UPDATE_DISTANCE)
			{
				if (UseNewAnimationSystem)
				{
					UpdateRenderObject();
				}
				else
				{
					UpdateContinuingSets();
					bool num = AdvanceAnimation();
					bool flag = ProcessCommands();
					UpdateAnimationState();
					if (num || flag)
					{
						CalculateTransforms(distance);
						UpdateRenderObject();
					}
				}
			}
			UpdateBoneDecals();
		}

		private void UpdateContinuingSets()
		{
			foreach (MyAnimationSetData continuingAnimSet in m_continuingAnimSets)
			{
				PlayAnimationSet(continuingAnimSet);
			}
		}

		private void UpdateBones(float distance)
		{
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				animationPlayer.Value.UpdateBones(distance);
			}
		}

		private bool AdvanceAnimation()
		{
			bool flag = false;
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				flag = animationPlayer.Value.Advance() || flag;
			}
			return flag;
		}

		private void UpdateAnimationState()
		{
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				animationPlayer.Value.UpdateAnimationState();
			}
		}

		/// <summary>
		/// Get the bones from the model and create a bone class object for
		/// each bone. We use our bone class to do the real animated bone work.
		/// </summary>
		public virtual void ObtainBones()
		{
			MyCharacterBone[] array = new MyCharacterBone[base.Model.Bones.Length];
			Matrix[] array2 = new Matrix[base.Model.Bones.Length];
			Matrix[] array3 = new Matrix[base.Model.Bones.Length];
			for (int i = 0; i < base.Model.Bones.Length; i++)
			{
				MyModelBone myModelBone = base.Model.Bones[i];
				Matrix transform = myModelBone.Transform;
				MyCharacterBone parent = ((myModelBone.Parent != -1) ? array[myModelBone.Parent] : null);
				MyCharacterBone myCharacterBone = (array[i] = new MyCharacterBone(myModelBone.Name, parent, transform, i, array2, array3));
			}
			m_compAnimationController.SetCharacterBones(array, array2, array3);
		}

		public Quaternion GetAdditionalRotation(string bone)
		{
			Quaternion value = Quaternion.Identity;
			if (string.IsNullOrEmpty(bone))
			{
				return value;
			}
			if (m_additionalRotations.TryGetValue(bone, out value))
			{
				return value;
			}
			return Quaternion.Identity;
		}

		internal void AddAnimationPlayer(string name, string[] bones)
		{
			m_animationPlayers.Add(name, new MyAnimationPlayerBlendPair(this, bones, null, name));
		}

		internal bool TryGetAnimationPlayer(string name, out MyAnimationPlayerBlendPair player)
		{
			if (name == null)
			{
				name = "";
			}
			if (name == "Body")
			{
				name = "";
			}
			return m_animationPlayers.TryGetValue(name, out player);
		}

		internal DictionaryReader<string, MyAnimationPlayerBlendPair> GetAllAnimationPlayers()
		{
			return m_animationPlayers;
		}

		private void PlayAnimationSet(MyAnimationSetData animationSetData)
		{
			if (!(MyRandom.Instance.NextFloat(0f, 1f) < animationSetData.AnimationSet.Probability))
			{
				return;
			}
			float num = Enumerable.Sum<AnimationItem>((IEnumerable<AnimationItem>)animationSetData.AnimationSet.AnimationItems, (Func<AnimationItem, float>)((AnimationItem x) => x.Ratio));
			if (!(num > 0f))
			{
				return;
			}
			float num2 = MyRandom.Instance.NextFloat(0f, 1f);
			float num3 = 0f;
			AnimationItem[] animationItems = animationSetData.AnimationSet.AnimationItems;
			for (int i = 0; i < animationItems.Length; i++)
			{
				AnimationItem animationItem = animationItems[i];
				num3 += animationItem.Ratio / num;
				if (num2 < num3)
				{
					MyAnimationCommand myAnimationCommand = default(MyAnimationCommand);
					myAnimationCommand.AnimationSubtypeName = animationItem.Animation;
					myAnimationCommand.PlaybackCommand = MyPlaybackCommand.Play;
					myAnimationCommand.Area = animationSetData.Area;
					myAnimationCommand.BlendTime = animationSetData.BlendTime;
					myAnimationCommand.TimeScale = 1f;
					myAnimationCommand.KeepContinuingAnimations = true;
					MyAnimationCommand command = myAnimationCommand;
					ProcessCommand(ref command);
					break;
				}
			}
		}

		internal void PlayersPlay(string bonesArea, MyAnimationDefinition animDefinition, bool firstPerson, MyFrameOption frameOption, float blendTime, float timeScale)
		{
			string[] array = bonesArea.Split(new char[1] { ' ' });
			if (animDefinition.AnimationSets != null)
			{
				AnimationSet[] animationSets = animDefinition.AnimationSets;
				for (int i = 0; i < animationSets.Length; i++)
				{
					AnimationSet animationSet = animationSets[i];
					MyAnimationSetData myAnimationSetData = default(MyAnimationSetData);
					myAnimationSetData.BlendTime = blendTime;
					myAnimationSetData.Area = bonesArea;
					myAnimationSetData.AnimationSet = animationSet;
					MyAnimationSetData myAnimationSetData2 = myAnimationSetData;
					if (animationSet.Continuous)
					{
						m_continuingAnimSets.Add(myAnimationSetData2);
					}
					else
					{
						PlayAnimationSet(myAnimationSetData2);
					}
				}
			}
			else
			{
				string[] array2 = array;
				foreach (string playerName in array2)
				{
					PlayerPlay(playerName, animDefinition, firstPerson, frameOption, blendTime, timeScale);
				}
			}
		}

		internal void PlayerPlay(string playerName, MyAnimationDefinition animDefinition, bool firstPerson, MyFrameOption frameOption, float blendTime, float timeScale)
		{
			if (TryGetAnimationPlayer(playerName, out var player))
			{
				player.Play(animDefinition, firstPerson, frameOption, blendTime, timeScale);
			}
		}

		internal void PlayerStop(string playerName, float blendTime)
		{
			if (TryGetAnimationPlayer(playerName, out var player))
			{
				player.Stop(blendTime);
			}
		}

		protected virtual void CalculateTransforms(float distance)
		{
			if (!UseNewAnimationSystem)
			{
				UpdateBones(distance);
			}
			AnimationController.UpdateTransformations();
		}

		/// <summary>
		/// Try getting animation definition matching given subtype name.
		/// VRage TODO: dependency on MyDefinitionManager, do we really need it here?
		///             backward compatibility is for modders?
		///             move backward compatibility to MyDefinitionManager.TryGetAnimationDefinition? then we do not need this method
		///
		///             marked as obsolete, needs to be resolved
		/// </summary>
		[Obsolete]
		protected bool TryGetAnimationDefinition(string animationSubtypeName, out MyAnimationDefinition animDefinition)
		{
			if (animationSubtypeName == null)
			{
				animDefinition = null;
				return false;
			}
			animDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(animationSubtypeName);
			if (animDefinition == null)
			{
				string text = Path.Combine(MyFileSystem.ContentPath, animationSubtypeName);
				if (MyFileSystem.FileExists(text))
				{
					animDefinition = new MyAnimationDefinition
					{
						AnimationModel = text,
						ClipIndex = 0
					};
					return true;
				}
				animDefinition = null;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Process all commands in the animation queue at once. 
		/// If any command is generated during flushing, it will be processed later.
		/// </summary>
		protected bool ProcessCommands()
		{
			if (m_commandQueue.get_Count() > 0)
			{
				MyAnimationCommand command = m_commandQueue.Dequeue();
				ProcessCommand(ref command);
				return true;
			}
			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="decalId"></param>
		/// <param name="boneIndex"></param>
		protected void AddBoneDecal(uint decalId, int boneIndex)
		{
			if (!m_boneDecals.TryGetValue(boneIndex, out var value))
			{
				value = new List<uint>(10);
				m_boneDecals.Add(boneIndex, value);
			}
			if (value.Count == value.Capacity)
			{
				MyDecals.RemoveDecal(value[0]);
				value.RemoveAt(0);
			}
			value.Add(decalId);
		}

		private void UpdateBoneDecals()
		{
			DecalBoneUpdates.Clear();
			foreach (KeyValuePair<int, List<uint>> boneDecal in m_boneDecals)
			{
				foreach (uint item in boneDecal.Value)
				{
					DecalBoneUpdates.Add(new MyBoneDecalUpdate
					{
						BoneID = boneDecal.Key,
						DecalID = item
					});
				}
			}
		}

		/// <summary>
		/// Process all commands in the animation queue at once. If any command is generated during flushing, it is processed as well.
		/// </summary>
		protected void FlushAnimationQueue()
		{
			while (m_commandQueue.get_Count() > 0)
			{
				ProcessCommands();
			}
		}

		/// <summary>
		/// Process single animation command.
		/// </summary>
		private void ProcessCommand(ref MyAnimationCommand command)
		{
			if (command.PlaybackCommand == MyPlaybackCommand.Play)
			{
				if (TryGetAnimationDefinition(command.AnimationSubtypeName, out var animDefinition))
				{
					string bonesArea = animDefinition.InfluenceArea;
					MyFrameOption frameOption = command.FrameOption;
					if (frameOption == MyFrameOption.Default)
					{
						frameOption = ((!animDefinition.Loop) ? MyFrameOption.PlayOnce : MyFrameOption.Loop);
					}
					bool useFirstPersonVersion = false;
					OnAnimationPlay(animDefinition, command, ref bonesArea, ref frameOption, ref useFirstPersonVersion);
					if (!string.IsNullOrEmpty(command.Area))
					{
						bonesArea = command.Area;
					}
					if (bonesArea == null)
					{
						bonesArea = "";
					}
					if (!command.KeepContinuingAnimations)
					{
						m_continuingAnimSets.Clear();
					}
					if (!UseNewAnimationSystem)
					{
						PlayersPlay(bonesArea, animDefinition, useFirstPersonVersion, frameOption, command.BlendTime, command.TimeScale);
					}
				}
			}
			else
			{
				if (command.PlaybackCommand != MyPlaybackCommand.Stop)
				{
					return;
				}
				string[] array = ((command.Area == null) ? "" : command.Area).Split(new char[1] { ' ' });
				if (!UseNewAnimationSystem)
				{
					string[] array2 = array;
					foreach (string playerName in array2)
					{
						PlayerStop(playerName, command.BlendTime);
					}
				}
			}
		}

		/// <summary>
		/// Enqueue animation command. Parameter sync is used in child classes.
		/// </summary>
		public virtual void AddCommand(MyAnimationCommand command, bool sync = false)
		{
			m_commandQueue.Enqueue(command);
		}

		/// <summary>
		/// Virtual method called when animation is started, used in MyCharacter.
		/// </summary>
		protected virtual void OnAnimationPlay(MyAnimationDefinition animDefinition, MyAnimationCommand command, ref string bonesArea, ref MyFrameOption frameOption, ref bool useFirstPersonVersion)
		{
		}

		protected void UpdateRenderObject()
		{
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public virtual void UpdateBeforeSimulationParallel()
		{
		}

<<<<<<< HEAD
		/// <inheritdoc />
		public virtual void UpdateAfterSimulationParallel()
		{
			float cameraDistance = AnimationController.CameraDistance;
=======
		public virtual void UpdateAfterSimulationParallel()
		{
			MyAnimationControllerComponent animationController = AnimationController;
			float cameraDistance = animationController.CameraDistance;
			UpdateControl(animationController.CameraDistance);
			animationController.Update();
			animationController.FinishUpdate();
			UpdateAnimation(animationController.CameraDistance);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			CalculateTransforms(cameraDistance);
		}
	}
}
