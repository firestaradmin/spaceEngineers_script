using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.IntergridCommunication;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Library.Collections;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 666, typeof(MyObjectBuilder_MyIGCSystemSessionComponent), null, false)]
	internal class MyIGCSystemSessionComponent : MySessionComponentBase
	{
		public struct Message
		{
			public readonly string Tag;

			public readonly object Data;

			public readonly TransmissionDistance TransmissionDistance;

			public readonly MyIntergridCommunicationContext Source;

			public readonly MyIntergridCommunicationContext UnicastDestination;

			public bool IsUnicast => UnicastDestination != null;

			private Message(object data, string tag, MyIntergridCommunicationContext source, MyIntergridCommunicationContext unicastDestination, TransmissionDistance transmissionDistance)
			{
				Tag = tag;
				Data = data;
				Source = source;
				UnicastDestination = unicastDestination;
				TransmissionDistance = transmissionDistance;
			}

			public static Message FromBroadcast(object data, string broadcastTag, TransmissionDistance transmissionDistance, MyIntergridCommunicationContext source)
			{
				if (broadcastTag == null)
				{
					throw new ArgumentNullException("broadcastTag", "Broadcast tag can't be null");
				}
				return new Message(data, broadcastTag, source, null, transmissionDistance);
			}

			public static Message FromUnicast(object data, string unicastTag, MyIntergridCommunicationContext source, MyIntergridCommunicationContext unicastDestination)
			{
				return new Message(data, unicastTag, source, unicastDestination, TransmissionDistance.AntennaRelay);
			}
		}

		/// <summary>
		/// Allows safe type or <see cref="T:VRage.MyTuple" /> of safe types.
		/// Safe type is any primitive, safe game struct or <see cref="T:VRage.MyTuple" /> of safe types.
		/// </summary>
		private static class MessageTypeChecker<TMessageType>
		{
			public static readonly bool IsAllowed = IsTypeAllowed(typeof(TMessageType), 25);

			private static bool IsTypeAllowed(Type type, int recursion)
			{
				if (recursion <= 0)
				{
					return false;
				}
				if (IsPrimitiveOfSafeStruct(type))
				{
					return true;
				}
				if (type.IsGenericType)
				{
					Type[] genericArguments = type.GetGenericArguments();
					Type genericTypeDefinition = type.GetGenericTypeDefinition();
					if (!IsMyTuple(genericTypeDefinition, genericArguments.Length) && !IsImmutableCollection(genericTypeDefinition))
					{
						return false;
					}
					Type[] array = genericArguments;
					for (int i = 0; i < array.Length; i++)
					{
						if (!IsTypeAllowed(array[i], recursion - 1))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}

			private static bool IsMyTuple(Type type, int genericArgs)
			{
				return genericArgs switch
				{
					1 => type == typeof(MyTuple<>), 
					2 => type == typeof(MyTuple<, >), 
					3 => type == typeof(MyTuple<, , >), 
					4 => type == typeof(MyTuple<, , , >), 
					5 => type == typeof(MyTuple<, , , , >), 
					6 => type == typeof(MyTuple<, , , , , >), 
					_ => false, 
				};
			}

			private static bool IsImmutableCollection(Type type)
			{
				if (!(type == typeof(ImmutableArray<>)) && !(type == typeof(ImmutableList<>)) && !(type == typeof(ImmutableQueue<>)) && !(type == typeof(ImmutableStack<>)) && !(type == typeof(ImmutableHashSet<>)) && !(type == typeof(ImmutableSortedSet<>)) && !(type == typeof(ImmutableDictionary<, >)))
				{
					return type == typeof(ImmutableSortedDictionary<, >);
				}
				return true;
			}

			private static bool IsPrimitiveOfSafeStruct(Type type)
			{
				if (type.IsPrimitive)
				{
					return true;
				}
				if (!(type == typeof(string)) && !(type == typeof(Ray)) && !(type == typeof(RayD)) && !(type == typeof(Line)) && !(type == typeof(LineD)) && !(type == typeof(Color)) && !(type == typeof(Plane)) && !(type == typeof(Point)) && !(type == typeof(PlaneD)) && !(type == typeof(MyQuad)) && !(type == typeof(Matrix)) && !(type == typeof(MatrixD)) && !(type == typeof(MatrixI)) && !(type == typeof(MyQuadD)) && !(type == typeof(Capsule)) && !(type == typeof(Vector2)) && !(type == typeof(Vector3)) && !(type == typeof(Vector4)) && !(type == typeof(CapsuleD)) && !(type == typeof(Vector2D)) && !(type == typeof(Vector2B)) && !(type == typeof(Vector3L)) && !(type == typeof(Vector4D)) && !(type == typeof(Vector3D)) && !(type == typeof(MyShort4)) && !(type == typeof(MyBounds)) && !(type == typeof(Vector3B)) && !(type == typeof(Vector3S)) && !(type == typeof(Vector2I)) && !(type == typeof(Vector4I)) && !(type == typeof(CubeFace)) && !(type == typeof(Vector3I)) && !(type == typeof(Matrix3x3)) && !(type == typeof(MyUShort4)) && !(type == typeof(Rectangle)) && !(type == typeof(Quaternion)) && !(type == typeof(RectangleF)) && !(type == typeof(BoundingBox)) && !(type == typeof(QuaternionD)) && !(type == typeof(MyTransform)) && !(type == typeof(BoundingBox2)) && !(type == typeof(BoundingBoxI)) && !(type == typeof(BoundingBoxD)) && !(type == typeof(MyTransformD)) && !(type == typeof(Vector3UByte)) && !(type == typeof(CurveTangent)) && !(type == typeof(Vector4UByte)) && !(type == typeof(BoundingBox2I)) && !(type == typeof(BoundingBox2D)) && !(type == typeof(Vector3Ushort)) && !(type == typeof(CurveLoopType)) && !(type == typeof(BoundingSphere)) && !(type == typeof(BoundingSphereD)) && !(type == typeof(ContainmentType)) && !(type == typeof(CurveContinuity)) && !(type == typeof(MyBlockOrientation)) && !(type == typeof(Base6Directions.Axis)) && !(type == typeof(MyOrientedBoundingBox)) && !(type == typeof(PlaneIntersectionType)) && !(type == typeof(MyOrientedBoundingBoxD)) && !(type == typeof(Vector3I_RangeIterator)) && !(type == typeof(Base6Directions.Direction)) && !(type == typeof(Base27Directions.Direction)) && !(type == typeof(CompressedPositionOrientation)) && !(type == typeof(Base6Directions.DirectionFlags)) && !(type == typeof(HalfVector3)) && !(type == typeof(HalfVector2)))
				{
					return type == typeof(HalfVector4);
				}
				return true;
			}
		}

		private static MyIGCSystemSessionComponent m_static;

		private MySwapList<Message> m_messagesForNextTick = new MySwapList<Message>();

		private Queue<MyTuple<int, Action>> m_debugDrawQueue;

		private Dictionary<long, MyIntergridCommunicationContext> m_perPBCommContexts = new Dictionary<long, MyIntergridCommunicationContext>();

		private CachingHashSet<MyIntergridCommunicationContext> m_contextsWithPendingCallbacks = new CachingHashSet<MyIntergridCommunicationContext>();

		private Dictionary<string, CachingHashSet<BroadcastListener>> m_activeBroadcastListeners = new Dictionary<string, CachingHashSet<BroadcastListener>>();

		private List<long> m_idsToInitialize;

		public static MyIGCSystemSessionComponent Static => m_static;

		public Action<MyCubeGrid, HashSet<MyDataBroadcaster>, long> BroadcasterProvider { get; private set; }

		public Func<MyProgrammableBlock, MyDataBroadcaster, long, bool> ConnectionProvider { get; private set; }

		public override bool IsRequiredByGame => true;

		public override Type[] Dependencies => new Type[1] { typeof(MyAntennaSystem) };

		public MyIntergridCommunicationContext GetContextForPB(long programmableBlockId)
		{
			m_perPBCommContexts.TryGetValue(programmableBlockId, out var value);
			return value;
		}

		public MyIntergridCommunicationContext GetOrMakeContextFor(MyProgrammableBlock block)
		{
			long entityId = block.EntityId;
			if (!m_perPBCommContexts.TryGetValue(entityId, out var value))
			{
				value = new MyIntergridCommunicationContext(block);
				m_perPBCommContexts.Add(entityId, value);
			}
			return value;
		}

		public void EvictContextFor(MyProgrammableBlock block)
		{
			long entityId = block.EntityId;
			MyIntergridCommunicationContext contextForPB = GetContextForPB(entityId);
			if (contextForPB != null)
			{
				contextForPB.DisposeContext();
				m_perPBCommContexts.Remove(entityId);
			}
		}

		public void RegisterBroadcastListener(BroadcastListener listener)
		{
			if (!m_activeBroadcastListeners.TryGetValue(listener.Tag, out var value))
			{
				value = new CachingHashSet<BroadcastListener>();
				m_activeBroadcastListeners.Add(listener.Tag, value);
			}
			value.Add(listener);
		}

		public void UnregisterBroadcastListener(BroadcastListener listener)
		{
			m_activeBroadcastListeners[listener.Tag].Remove(listener);
		}

		public void RegisterContextWithPendingCallbacks(MyIntergridCommunicationContext context)
		{
			m_contextsWithPendingCallbacks.Add(context);
		}

		public void UnregisterContextWithPendingCallbacks(MyIntergridCommunicationContext context)
		{
			m_contextsWithPendingCallbacks.Remove(context);
		}

		public void EnqueueMessage(Message message)
		{
			m_messagesForNextTick.Add(message);
		}

		public override void UpdateBeforeSimulation()
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateBeforeSimulation();
			m_messagesForNextTick.Swap();
			foreach (Message background in m_messagesForNextTick.BackgroundList)
			{
				if (!background.Source.IsActive)
				{
					continue;
				}
				MyIGCMessage message = new MyIGCMessage(background.Data, background.Tag, background.Source.GetAddressOfThisContext());
				if (background.IsUnicast)
				{
					MyIntergridCommunicationContext unicastDestination = background.UnicastDestination;
					if (unicastDestination.IsActive)
					{
						unicastDestination.UnicastListener.EnqueueMessage(message);
<<<<<<< HEAD
					}
				}
				else
				{
					if (!m_activeBroadcastListeners.TryGetValue(background.Tag, out var value))
					{
						continue;
					}
					value.ApplyChanges();
					if (value.Count <= 0)
					{
						continue;
					}
					foreach (BroadcastListener item in value)
					{
						if (item.Context != background.Source && background.Source.IsConnectedTo(item.Context, background.TransmissionDistance))
						{
							item.EnqueueMessage(message);
=======
					}
				}
				else
				{
					if (!m_activeBroadcastListeners.TryGetValue(background.Tag, out var value))
					{
						continue;
					}
					value.ApplyChanges();
					if (value.Count <= 0)
					{
						continue;
					}
					Enumerator<BroadcastListener> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							BroadcastListener current2 = enumerator2.get_Current();
							if (current2.Context != background.Source && background.Source.IsConnectedTo(current2.Context, background.TransmissionDistance))
							{
								current2.EnqueueMessage(message);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
			m_messagesForNextTick.BackgroundList.Clear();
			m_contextsWithPendingCallbacks.ApplyChanges();
			Enumerator<MyIntergridCommunicationContext> enumerator3 = m_contextsWithPendingCallbacks.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.get_Current().InvokeSinglePendingCallback();
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
			if (!MyDebugDrawSettings.DEBUG_DRAW_IGC)
			{
				return;
			}
			enumerator3 = m_contextsWithPendingCallbacks.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MyRenderProxy.DebugDrawSphere(enumerator3.get_Current().ProgrammableBlock.WorldMatrix.Translation, 2f, Color.Orange);
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
			foreach (CachingHashSet<BroadcastListener> value2 in m_activeBroadcastListeners.Values)
			{
				value2.ApplyChanges();
				Enumerator<BroadcastListener> enumerator2 = value2.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						BroadcastListener current4 = enumerator2.get_Current();
						MyRenderProxy.DebugDrawText3D(current4.Context.ProgrammableBlock.WorldMatrix.Translation, current4.Tag, Color.Blue, 0.7f, depthRead: false);
					}
				}
<<<<<<< HEAD
=======
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (m_debugDrawQueue == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyTuple<int, Action> item3 in m_debugDrawQueue)
			{
				item3.Item2();
			}
			while (m_debugDrawQueue.Count > 0 && m_debugDrawQueue.Peek().Item1 <= MySession.Static.GameplayFrameCounter)
			{
				m_debugDrawQueue.Dequeue();
=======
			Enumerator<MyTuple<int, Action>> enumerator5 = m_debugDrawQueue.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					enumerator5.get_Current().Item2();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator5).Dispose();
			}
			while (m_debugDrawQueue.get_Count() > 0 && m_debugDrawQueue.Peek().Item1 <= MySession.Static.GameplayFrameCounter)
			{
				m_debugDrawQueue.Dequeue();
			}
		}

		public void AddDebugDraw(Action action)
		{
			if (m_debugDrawQueue == null)
			{
				m_debugDrawQueue = new Queue<MyTuple<int, Action>>();
			}
			m_debugDrawQueue.Enqueue(MyTuple.Create(MySession.Static.GameplayFrameCounter + 30, action));
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_MyIGCSystemSessionComponent myObjectBuilder_MyIGCSystemSessionComponent = (MyObjectBuilder_MyIGCSystemSessionComponent)base.GetObjectBuilder();
			myObjectBuilder_MyIGCSystemSessionComponent.ActiveProgrammableBlocks = new List<long>(m_perPBCommContexts.Count);
			foreach (long key in m_perPBCommContexts.Keys)
			{
				myObjectBuilder_MyIGCSystemSessionComponent.ActiveProgrammableBlocks.Add(key);
			}
			return myObjectBuilder_MyIGCSystemSessionComponent;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_MyIGCSystemSessionComponent myObjectBuilder_MyIGCSystemSessionComponent = (MyObjectBuilder_MyIGCSystemSessionComponent)sessionComponent;
			m_idsToInitialize = myObjectBuilder_MyIGCSystemSessionComponent.ActiveProgrammableBlocks;
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!Sync.IsServer || m_idsToInitialize == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (long item in m_idsToInitialize)
			{
=======
			{
				return;
			}
			foreach (long item in m_idsToInitialize)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyProgrammableBlock myProgrammableBlock = (MyProgrammableBlock)MyEntities.GetEntityById(item);
				if (myProgrammableBlock != null)
				{
					GetOrMakeContextFor(myProgrammableBlock);
				}
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			m_static = this;
			BroadcasterProvider = MyAntennaSystem.GetCubeGridGroupBroadcasters;
			ConnectionProvider = (MyProgrammableBlock target, MyDataBroadcaster source, long rightsCheckedIdentity) => MyAntennaSystem.Static.CheckConnection(target, source, rightsCheckedIdentity, mutual: false);
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			foreach (MyIntergridCommunicationContext value in m_perPBCommContexts.Values)
			{
				value.DisposeContext();
			}
			m_debugDrawQueue = null;
			m_perPBCommContexts = null;
			ConnectionProvider = null;
			BroadcasterProvider = null;
			m_contextsWithPendingCallbacks = null;
			m_static = null;
		}

		/// <summary>
		/// Boxing here and forced unboxing on the other side makes sure that there is no reference shared between two PBs.
		/// </summary>
		public static object BoxMessage<TMessage>(TMessage message)
		{
			if (!MessageTypeChecker<TMessage>.IsAllowed)
			{
				throw new Exception(string.Concat("Message type ", typeof(TMessage), " is not allowed!"));
			}
			return message;
		}
	}
}
