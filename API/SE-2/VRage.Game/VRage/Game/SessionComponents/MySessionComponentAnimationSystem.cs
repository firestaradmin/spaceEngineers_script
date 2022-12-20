using System;
using System.Collections.Generic;
using System.IO;
using ParallelTasks;
using VRage.FileSystem;
using VRage.Game.Components;
using VRage.Game.Debugging;
using VRage.Game.Definitions;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Generics;
<<<<<<< HEAD
using VRage.ModAPI;
using VRage.Network;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;
using VRageRender.Animations;

namespace VRage.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 0)]
	public class MySessionComponentAnimationSystem : MySessionComponentBase
	{
<<<<<<< HEAD
		private class UpdateWork : ForEachLoopWork<MyAnimationControllerComponent>
		{
			private class VRage_Game_SessionComponents_MySessionComponentAnimationSystem_003C_003EUpdateWork_003C_003EActor : IActivator, IActivator<UpdateWork>
			{
				private sealed override object CreateInstance()
				{
					return new UpdateWork();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override UpdateWork CreateInstance()
				{
					return new UpdateWork();
				}

				UpdateWork IActivator<UpdateWork>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private MySessionComponentAnimationSystem m_animSystem;

			private Action<MyAnimationControllerComponent> m_workCallback;

			public UpdateWork()
			{
				throw new NotSupportedException();
			}

			public UpdateWork(MySessionComponentAnimationSystem animSystem)
			{
				m_animSystem = animSystem;
				m_workCallback = UpdateComponent;
			}

			public void BeforeRun()
			{
				HashSet<MyAnimationControllerComponent> skinnedEntityComponents = m_animSystem.m_skinnedEntityComponents;
				Prepare(m_workCallback, skinnedEntityComponents.GetEnumerator(), WorkPriority.Normal);
				int maxThreads = 1 + skinnedEntityComponents.Count / 5;
				Options = Parallel.DefaultOptions.WithMaxThreads(maxThreads);
			}

			private void UpdateComponent(MyAnimationControllerComponent component)
			{
				using (m_animSystem.m_lock.AcquireSharedUsing())
				{
					if (!m_animSystem.m_skinnedEntityComponentsToRemove.Contains(component) && (component.Entity.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0 && component.Update())
					{
						List<MyAnimationControllerComponent> updatedAnimationControllers = m_animSystem.m_updatedAnimationControllers;
						lock (updatedAnimationControllers)
						{
							updatedAnimationControllers.Add(component);
						}
					}
				}
			}

			protected override void FillDebugInfo(ref WorkOptions info)
			{
				info.DebugName = "AnimationSystem";
				info.TaskType = MyProfiler.TaskType.WorkItem;
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static MySessionComponentAnimationSystem Static;

		private readonly HashSet<MyAnimationControllerComponent> m_skinnedEntityComponents = new HashSet<MyAnimationControllerComponent>();

		private readonly HashSet<MyAnimationControllerComponent> m_skinnedEntityComponentsToAdd = new HashSet<MyAnimationControllerComponent>();

		private readonly HashSet<MyAnimationControllerComponent> m_skinnedEntityComponentsToRemove = new HashSet<MyAnimationControllerComponent>();

		private readonly FastResourceLock m_lock = new FastResourceLock();

		private int m_debuggingSendNameCounter;

		private const int m_debuggingSendNameCounterMax = 60;

		private string m_debuggingLastNameSent;

		private readonly List<MyStateMachineNode> m_debuggingAnimControllerCurrentNodes = new List<MyStateMachineNode>();

		private readonly List<int[]> m_debuggingAnimControllerTreePath = new List<int[]>();

		private List<MyAnimationControllerComponent> m_updatedAnimationControllers = new List<MyAnimationControllerComponent>();

		private List<MyAnimationControllerComponent> m_updatedControllersSwap = new List<MyAnimationControllerComponent>();

		public MyEntity EntitySelectedForDebug;

		public IEnumerable<MyAnimationControllerComponent> RegisteredAnimationComponents => (IEnumerable<MyAnimationControllerComponent>)m_skinnedEntityComponents;

		public override Type[] Dependencies => new Type[1] { typeof(MySessionComponentExtDebug) };

		public override void LoadData()
		{
			EntitySelectedForDebug = null;
			m_skinnedEntityComponents.Clear();
			m_skinnedEntityComponentsToAdd.Clear();
			m_skinnedEntityComponentsToRemove.Clear();
			Static = this;
			if (!MySessionComponentExtDebug.Static.IsHandlerRegistered(LiveDebugging_ReceivedMessageHandler))
			{
				MySessionComponentExtDebug.Static.ReceivedMsg += LiveDebugging_ReceivedMessageHandler;
			}
			MyAnimationTreeNodeDynamicTrack.OnAction = (Func<MyStringId, MyAnimationTreeNodeDynamicTrack.DynamicTrackData>)Delegate.Combine(MyAnimationTreeNodeDynamicTrack.OnAction, new Func<MyStringId, MyAnimationTreeNodeDynamicTrack.DynamicTrackData>(OnDynamicTrackAction));
		}

		protected override void UnloadData()
		{
			EntitySelectedForDebug = null;
			m_skinnedEntityComponents.Clear();
			m_skinnedEntityComponentsToAdd.Clear();
			m_skinnedEntityComponentsToRemove.Clear();
			MyAnimationTreeNodeDynamicTrack.OnAction = (Func<MyStringId, MyAnimationTreeNodeDynamicTrack.DynamicTrackData>)Delegate.Remove(MyAnimationTreeNodeDynamicTrack.OnAction, new Func<MyStringId, MyAnimationTreeNodeDynamicTrack.DynamicTrackData>(OnDynamicTrackAction));
			Static = null;
<<<<<<< HEAD
		}

		public override void UpdateBeforeSimulation()
		{
			m_lastStartedTask?.WaitOrExecute();
			PostProcessAnimations();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void UpdateAfterSimulation()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateAfterSimulation();
			using (m_lock.AcquireExclusiveUsing())
			{
				Enumerator<MyAnimationControllerComponent> enumerator = m_skinnedEntityComponentsToRemove.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyAnimationControllerComponent current = enumerator.get_Current();
						if (m_skinnedEntityComponents.Remove(current))
						{
							m_skinnedEntityComponentsToAdd.Remove(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				m_skinnedEntityComponentsToRemove.Clear();
				enumerator = m_skinnedEntityComponentsToAdd.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyAnimationControllerComponent current2 = enumerator.get_Current();
						m_skinnedEntityComponents.Add(current2);
					}
				}
<<<<<<< HEAD
				m_skinnedEntityComponentsToAdd.Clear();
			}
			foreach (MyAnimationControllerComponent skinnedEntityComponent in m_skinnedEntityComponents)
			{
				IMyEntity entity = skinnedEntityComponent.Entity;
				if (entity != null && (entity.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0)
				{
					IMySkinnedEntity obj = (IMySkinnedEntity)entity;
					obj.UpdateControl(skinnedEntityComponent.CameraDistance);
					obj.UpdateAnimation(skinnedEntityComponent.CameraDistance);
					skinnedEntityComponent.ApplyVariables();
=======
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_skinnedEntityComponentsToAdd.Clear();
			}
<<<<<<< HEAD
			m_work.BeforeRun();
			m_lastStartedTask = Parallel.Start(m_work);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			LiveDebugging();
		}

		private void PostProcessAnimations()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				MyUtils.Swap(ref m_updatedControllersSwap, ref m_updatedAnimationControllers);
			}
			foreach (MyAnimationControllerComponent item in m_updatedControllersSwap)
			{
				item.FinishUpdate();
			}
			m_updatedControllersSwap.Clear();
		}

		/// <summary>
		/// Register entity component.
		/// </summary>
		internal void RegisterEntityComponent(MyAnimationControllerComponent entityComponent)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_skinnedEntityComponentsToAdd.Add(entityComponent);
			}
		}

		/// <summary>
		/// Unregister entity component.
		/// </summary>
		internal void UnregisterEntityComponent(MyAnimationControllerComponent entityComponent)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_skinnedEntityComponentsToRemove.Add(entityComponent);
			}
		}

		private void LiveDebugging()
		{
			if (Session == null || MySessionComponentExtDebug.Static == null)
			{
				return;
			}
			MyEntity myEntity = EntitySelectedForDebug ?? ((Session.ControlledObject != null) ? (Session.ControlledObject.Entity as MyEntity) : null);
			if (myEntity == null)
			{
				return;
			}
			MyAnimationControllerComponent myAnimationControllerComponent = myEntity.Components.Get<MyAnimationControllerComponent>();
			if (myAnimationControllerComponent != null && !myAnimationControllerComponent.SourceId.TypeId.IsNull)
			{
				m_debuggingSendNameCounter--;
				if (myAnimationControllerComponent.SourceId.SubtypeName != m_debuggingLastNameSent)
				{
					m_debuggingSendNameCounter = 0;
				}
				if (m_debuggingSendNameCounter <= 0)
				{
					LiveDebugging_SendControllerNameToEditor(myAnimationControllerComponent.SourceId.SubtypeName);
					m_debuggingSendNameCounter = 60;
					m_debuggingLastNameSent = myAnimationControllerComponent.SourceId.SubtypeName;
				}
				LiveDebugging_SendAnimationStateChangesToEditor(myAnimationControllerComponent.Controller);
			}
		}

		private void LiveDebugging_SendControllerNameToEditor(string subtypeName)
		{
			ACConnectToEditorMsg aCConnectToEditorMsg = default(ACConnectToEditorMsg);
			aCConnectToEditorMsg.ACName = subtypeName;
			ACConnectToEditorMsg msg = aCConnectToEditorMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}

		private void LiveDebugging_SendAnimationStateChangesToEditor(MyAnimationController animController)
		{
			if (animController == null)
			{
				return;
			}
			int layerCount = animController.GetLayerCount();
			if (layerCount != m_debuggingAnimControllerCurrentNodes.Count)
			{
				m_debuggingAnimControllerCurrentNodes.Clear();
				for (int i = 0; i < layerCount; i++)
				{
					m_debuggingAnimControllerCurrentNodes.Add(null);
				}
				m_debuggingAnimControllerTreePath.Clear();
				for (int j = 0; j < layerCount; j++)
				{
					m_debuggingAnimControllerTreePath.Add(new int[animController.GetLayerByIndex(j).VisitedTreeNodesPath.Length]);
				}
			}
			for (int k = 0; k < layerCount; k++)
			{
				int[] visitedTreeNodesPath = animController.GetLayerByIndex(k).VisitedTreeNodesPath;
				if (animController.GetLayerByIndex(k).CurrentNode != m_debuggingAnimControllerCurrentNodes[k] || !LiveDebugging_CompareAnimTreePathSeqs(visitedTreeNodesPath, m_debuggingAnimControllerTreePath[k]))
				{
					Array.Copy(visitedTreeNodesPath, m_debuggingAnimControllerTreePath[k], visitedTreeNodesPath.Length);
					m_debuggingAnimControllerCurrentNodes[k] = animController.GetLayerByIndex(k).CurrentNode;
					if (m_debuggingAnimControllerCurrentNodes[k] != null)
					{
						ACSendStateToEditorMsg msg = ACSendStateToEditorMsg.Create(m_debuggingAnimControllerCurrentNodes[k].Name, m_debuggingAnimControllerTreePath[k]);
						MySessionComponentExtDebug.Static.SendMessageToClients(msg);
					}
				}
			}
		}

		private static bool LiveDebugging_CompareAnimTreePathSeqs(int[] seq1, int[] seq2)
		{
			if (seq1 == null || seq2 == null || seq1.Length != seq2.Length)
			{
				return false;
			}
			for (int i = 0; i < seq1.Length; i++)
			{
				if (seq1[i] != seq2[i])
				{
					return false;
				}
				if (seq1[i] == 0 && seq2[i] == 0)
				{
					return true;
				}
			}
			return true;
		}

		private void LiveDebugging_ReceivedMessageHandler(MyExternalDebugStructures.CommonMsgHeader messageHeader, byte[] messageData)
		{
<<<<<<< HEAD
			if (!MyExternalDebugStructures.ReadMessageFromPtr<ACReloadInGameMsg>(ref messageHeader, messageData, out var outMsg))
			{
				return;
			}
			try
			{
=======
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			if (!MyExternalDebugStructures.ReadMessageFromPtr<ACReloadInGameMsg>(ref messageHeader, messageData, out var outMsg))
			{
				return;
			}
			try
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				string aCContentAddress = outMsg.ACContentAddress;
				string aCAddress = outMsg.ACAddress;
				string aCName = outMsg.ACName;
				if (!MyObjectBuilderSerializer.DeserializeXML(aCAddress, out MyObjectBuilder_Definitions objectBuilder) || objectBuilder.Definitions == null || objectBuilder.Definitions.Length == 0)
<<<<<<< HEAD
				{
					return;
				}
				MyObjectBuilder_DefinitionBase builder = objectBuilder.Definitions[0];
				MyModContext myModContext = new MyModContext();
				myModContext.Init("AnimationControllerDefinition", aCAddress, aCContentAddress);
				MyAnimationControllerDefinition myAnimationControllerDefinition = new MyAnimationControllerDefinition();
				myAnimationControllerDefinition.Init(builder, myModContext);
				MyStringHash orCompute = MyStringHash.GetOrCompute(aCName);
				MyAnimationControllerDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>(orCompute);
				MyDefinitionPostprocessor postProcessor = MyDefinitionManagerBase.GetPostProcessor(typeof(MyObjectBuilder_AnimationControllerDefinition));
				if (postProcessor != null)
				{
					MyDefinitionPostprocessor.Bundle bundle = default(MyDefinitionPostprocessor.Bundle);
					bundle.Context = MyModContext.BaseGame;
					bundle.Definitions = new Dictionary<MyStringHash, MyDefinitionBase> { { orCompute, definition } };
					bundle.Set = new MyDefinitionSet();
					MyDefinitionPostprocessor.Bundle currentDefinitions = bundle;
					currentDefinitions.Set.AddDefinition(definition);
					bundle = default(MyDefinitionPostprocessor.Bundle);
					bundle.Context = myModContext;
					bundle.Definitions = new Dictionary<MyStringHash, MyDefinitionBase> { { orCompute, myAnimationControllerDefinition } };
					bundle.Set = new MyDefinitionSet();
					MyDefinitionPostprocessor.Bundle definitions = bundle;
					definitions.Set.AddDefinition(myAnimationControllerDefinition);
					postProcessor.AfterLoaded(ref definitions);
					postProcessor.OverrideBy(ref currentDefinitions, ref definitions);
				}
				foreach (MyAnimationControllerComponent skinnedEntityComponent in m_skinnedEntityComponents)
				{
					if (skinnedEntityComponent != null && skinnedEntityComponent.SourceId.SubtypeName == aCName)
					{
						skinnedEntityComponent.Clear();
						skinnedEntityComponent.InitFromDefinition(definition, forceReloadMwm: true);
						if (skinnedEntityComponent.ReloadBonesNeeded != null)
						{
							skinnedEntityComponent.ReloadBonesNeeded();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
=======
				{
					return;
				}
				MyObjectBuilder_DefinitionBase builder = objectBuilder.Definitions[0];
				MyModContext myModContext = new MyModContext();
				myModContext.Init("AnimationControllerDefinition", aCAddress, aCContentAddress);
				MyAnimationControllerDefinition myAnimationControllerDefinition = new MyAnimationControllerDefinition();
				myAnimationControllerDefinition.Init(builder, myModContext);
				MyStringHash orCompute = MyStringHash.GetOrCompute(aCName);
				MyAnimationControllerDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>(orCompute);
				MyDefinitionPostprocessor postProcessor = MyDefinitionManagerBase.GetPostProcessor(typeof(MyObjectBuilder_AnimationControllerDefinition));
				if (postProcessor != null)
				{
					MyDefinitionPostprocessor.Bundle bundle = default(MyDefinitionPostprocessor.Bundle);
					bundle.Context = MyModContext.BaseGame;
					bundle.Definitions = new Dictionary<MyStringHash, MyDefinitionBase> { { orCompute, definition } };
					bundle.Set = new MyDefinitionSet();
					MyDefinitionPostprocessor.Bundle currentDefinitions = bundle;
					currentDefinitions.Set.AddDefinition(definition);
					bundle = default(MyDefinitionPostprocessor.Bundle);
					bundle.Context = myModContext;
					bundle.Definitions = new Dictionary<MyStringHash, MyDefinitionBase> { { orCompute, myAnimationControllerDefinition } };
					bundle.Set = new MyDefinitionSet();
					MyDefinitionPostprocessor.Bundle definitions = bundle;
					definitions.Set.AddDefinition(myAnimationControllerDefinition);
					postProcessor.AfterLoaded(ref definitions);
					postProcessor.OverrideBy(ref currentDefinitions, ref definitions);
				}
				Enumerator<MyAnimationControllerComponent> enumerator = m_skinnedEntityComponents.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyAnimationControllerComponent current = enumerator.get_Current();
						if (current != null && current.SourceId.SubtypeName == aCName)
						{
							current.Clear();
							current.InitFromDefinition(definition, forceReloadMwm: true);
							if (current.ReloadBonesNeeded != null)
							{
								current.ReloadBonesNeeded();
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
			}
		}

		/// <summary>
		/// Reload all mwm tracks while in-game. Mwms from cache are not used. 
		/// </summary>
		public void ReloadMwmTracks()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyAnimationControllerComponent> enumerator = m_skinnedEntityComponents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyAnimationControllerComponent current = enumerator.get_Current();
					MyAnimationControllerDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>(MyStringHash.GetOrCompute(current.SourceId.SubtypeName));
					if (definition != null)
					{
						current.Clear();
						current.InitFromDefinition(definition, forceReloadMwm: true);
						if (current.ReloadBonesNeeded != null)
						{
							current.ReloadBonesNeeded();
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private MyAnimationTreeNodeDynamicTrack.DynamicTrackData OnDynamicTrackAction(MyStringId action)
		{
			MyAnimationTreeNodeDynamicTrack.DynamicTrackData result = default(MyAnimationTreeNodeDynamicTrack.DynamicTrackData);
			MyAnimationDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationDefinition>(action.ToString());
			if (definition == null)
			{
				return result;
			}
			string animationModel = definition.AnimationModel;
			if (string.IsNullOrEmpty(definition.AnimationModel))
			{
				return result;
			}
			if (!MyFileSystem.FileExists(Path.IsPathRooted(animationModel) ? animationModel : Path.Combine(MyFileSystem.ContentPath, animationModel)))
			{
				definition.Status = MyAnimationDefinition.AnimationStatus.Failed;
				return result;
			}
			MyModel modelOnlyAnimationData = MyModels.GetModelOnlyAnimationData(animationModel);
			if (modelOnlyAnimationData != null && (modelOnlyAnimationData.Animations == null || modelOnlyAnimationData.Animations.Clips.Count == 0))
			{
				return result;
			}
			result.Clip = modelOnlyAnimationData.Animations.Clips[definition.ClipIndex];
			result.Loop = definition.Loop;
			return result;
		}
	}
}
