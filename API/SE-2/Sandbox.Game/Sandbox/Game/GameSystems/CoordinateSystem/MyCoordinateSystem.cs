using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.GameSystems.CoordinateSystem
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof(MyObjectBuilder_CoordinateSystem), null, false)]
	[StaticEventOwner]
	public class MyCoordinateSystem : MySessionComponentBase
	{
		/// <summary>
		/// Used for transporting information about coord system to clients.
		/// </summary>
		[Serializable]
		private struct MyCreateCoordSysBuffer
		{
			protected class Sandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem_003C_003EMyCreateCoordSysBuffer_003C_003EId_003C_003EAccessor : IMemberAccessor<MyCreateCoordSysBuffer, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCreateCoordSysBuffer owner, in long value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCreateCoordSysBuffer owner, out long value)
				{
					value = owner.Id;
				}
			}

			protected class Sandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem_003C_003EMyCreateCoordSysBuffer_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyCreateCoordSysBuffer, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCreateCoordSysBuffer owner, in Vector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCreateCoordSysBuffer owner, out Vector3D value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem_003C_003EMyCreateCoordSysBuffer_003C_003ERotation_003C_003EAccessor : IMemberAccessor<MyCreateCoordSysBuffer, Quaternion>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCreateCoordSysBuffer owner, in Quaternion value)
				{
					owner.Rotation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCreateCoordSysBuffer owner, out Quaternion value)
				{
					value = owner.Rotation;
				}
			}

			public long Id;

			public Vector3D Position;

			[Serialize(MyPrimitiveFlags.Normalized)]
			public Quaternion Rotation;
		}

		public struct CoordSystemData
		{
			public long Id;

			public MyTransformD SnappedTransform;

			public MyTransformD Origin;

			public Vector3D LocalSnappedPos;
		}

		protected sealed class CoordSysCreated_Client_003C_003ESandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem_003C_003EMyCreateCoordSysBuffer : ICallSite<IMyEventOwner, MyCreateCoordSysBuffer, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyCreateCoordSysBuffer createBuffer, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CoordSysCreated_Client(createBuffer);
			}
		}

		protected sealed class CoorSysRemoved_Client_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long coordSysId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CoorSysRemoved_Client(coordSysId);
			}
		}

		public static MyCoordinateSystem Static;

		private double m_angleTolerance = 0.0001;

		private double m_positionTolerance = 0.001;

		private int m_coorsSystemSize = 1000;

		private int m_coorsSystemSizeSq = 1000000;

		private Dictionary<long, MyLocalCoordSys> m_localCoordSystems = new Dictionary<long, MyLocalCoordSys>();

		private long m_lastCoordSysId = 1L;

		private bool m_drawBoundingBox;

		private long m_selectedCoordSys;

		private long m_lastSelectedCoordSys;

		private bool m_localCoordExist;

		private bool m_visible;

		/// <summary>
		/// Returns id of selected coord system.
		/// </summary>
		public long SelectedCoordSys => m_selectedCoordSys;

		/// <summary>
		/// Returns id of last selected coord system.
		/// </summary>
		public long LastSelectedCoordSys => m_lastSelectedCoordSys;

		/// <summary>
		/// Indicates if there is any selected coord system.
		/// </summary>
		public bool LocalCoordExist => m_localCoordExist;

		/// <summary>
		/// Indicates if LCS graphic representation is visible.
		/// </summary>
		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				m_visible = value;
			}
		}

		public int CoordSystemSize => m_coorsSystemSize;

		public int CoordSystemSizeSquared => m_coorsSystemSizeSq;

		public static event Action OnCoordinateChange;

		public MyCoordinateSystem()
		{
			Static = this;
			if (Sync.IsServer)
			{
				MyEntities.OnEntityAdd += MyEntities_OnEntityCreate;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_CoordinateSystem myObjectBuilder_CoordinateSystem = sessionComponent as MyObjectBuilder_CoordinateSystem;
			m_lastCoordSysId = myObjectBuilder_CoordinateSystem.LastCoordSysId;
			foreach (MyObjectBuilder_CoordinateSystem.CoordSysInfo coordSystem in myObjectBuilder_CoordinateSystem.CoordSystems)
			{
				MyTransformD origin = default(MyTransformD);
				origin.Position = coordSystem.Position;
				origin.Rotation = coordSystem.Rotation;
				MyLocalCoordSys myLocalCoordSys = new MyLocalCoordSys(origin, m_coorsSystemSize);
				myLocalCoordSys.Id = coordSystem.Id;
				m_localCoordSystems.Add(coordSystem.Id, myLocalCoordSys);
			}
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			MyCoordinateSystemDefinition myCoordinateSystemDefinition = definition as MyCoordinateSystemDefinition;
			if (myCoordinateSystemDefinition != null)
			{
				m_coorsSystemSize = myCoordinateSystemDefinition.CoordSystemSize;
				m_coorsSystemSizeSq = m_coorsSystemSize * m_coorsSystemSize;
				m_angleTolerance = myCoordinateSystemDefinition.AngleTolerance;
				m_positionTolerance = myCoordinateSystemDefinition.PositionTolerance;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_lastCoordSysId = 1L;
			m_localCoordSystems.Clear();
			m_drawBoundingBox = false;
			m_selectedCoordSys = 0L;
			m_lastSelectedCoordSys = 0L;
			Static = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_CoordinateSystem myObjectBuilder_CoordinateSystem = base.GetObjectBuilder() as MyObjectBuilder_CoordinateSystem;
			myObjectBuilder_CoordinateSystem.LastCoordSysId = m_lastCoordSysId;
			foreach (KeyValuePair<long, MyLocalCoordSys> localCoordSystem in m_localCoordSystems)
			{
				MyObjectBuilder_CoordinateSystem.CoordSysInfo item = default(MyObjectBuilder_CoordinateSystem.CoordSysInfo);
				item.Id = localCoordSystem.Value.Id;
				item.EntityCount = localCoordSystem.Value.EntityCounter;
				item.Position = localCoordSystem.Value.Origin.Position;
				item.Rotation = localCoordSystem.Value.Origin.Rotation;
				myObjectBuilder_CoordinateSystem.CoordSystems.Add(item);
			}
			return myObjectBuilder_CoordinateSystem;
		}

		/// <summary>
		/// Returns closest local coord system.
		/// </summary>
		/// <param name="position">World position.</param>
		/// <param name="checkContain"></param>
		/// <returns>Local coord system.</returns>
		private MyLocalCoordSys GetClosestCoordSys(ref Vector3D position, bool checkContain = true)
		{
			MyLocalCoordSys result = null;
			double num = double.MaxValue;
			foreach (MyLocalCoordSys value in m_localCoordSystems.Values)
			{
				if (!checkContain || value.Contains(ref position))
				{
					double num2 = (value.Origin.Position - position).LengthSquared();
					if (num2 < num)
					{
						result = value;
						num = num2;
					}
				}
			}
			return result;
		}

<<<<<<< HEAD
		/// <summary>
		/// Method on client that is called by server. Will Trigger creation of coord system.
		/// </summary>
		/// <param name="createBuffer"></param>        
		[Event(null, 242)]
=======
		[Event(null, 244)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[BroadcastExcept]
		private static void CoordSysCreated_Client(MyCreateCoordSysBuffer createBuffer)
		{
			MyTransformD transform = default(MyTransformD);
			transform.Position = createBuffer.Position;
			transform.Rotation = createBuffer.Rotation;
			Static.CreateCoordSys_ClientInternal(ref transform, createBuffer.Id);
		}

		/// <summary>
		/// Only creates coord system. Call only on client in reaction on server create.
		/// </summary>
		/// <param name="transform">Origin of the coord system.</param>
		/// <param name="coordSysId">Coord system id that should be used in creation.</param>
		private void CreateCoordSys_ClientInternal(ref MyTransformD transform, long coordSysId)
		{
			MyLocalCoordSys myLocalCoordSys = new MyLocalCoordSys(transform, m_coorsSystemSize);
			myLocalCoordSys.Id = coordSysId;
			m_localCoordSystems.Add(coordSysId, myLocalCoordSys);
		}

		/// <summary>
		/// Creates coord system and sends it to clients. Should be called only on server.
		/// </summary>
		/// <param name="cubeGrid">Cube grid that is an origin.</param>
		/// <param name="staticGridAlignToCenter">Indicates if grid should be aligned to center or no.</param>
		/// <param name="sync"></param>
		public void CreateCoordSys(MyCubeGrid cubeGrid, bool staticGridAlignToCenter, bool sync = false)
		{
			MyTransformD origin = new MyTransformD(cubeGrid.PositionComp.WorldMatrixRef);
			origin.Rotation.Normalize();
			float gridSize = cubeGrid.GridSize;
			if (!staticGridAlignToCenter)
			{
				origin.Position -= (origin.Rotation.Forward + origin.Rotation.Right + origin.Rotation.Up) * gridSize * 0.5f;
			}
			MyLocalCoordSys myLocalCoordSys = new MyLocalCoordSys(origin, m_coorsSystemSize);
			long num;
			do
			{
				num = m_lastCoordSysId++;
			}
			while (m_localCoordSystems.ContainsKey(num));
			myLocalCoordSys.Id = num;
			m_localCoordSystems.Add(num, myLocalCoordSys);
			if (cubeGrid.LocalCoordSystem != 0L)
			{
				UnregisterCubeGrid(cubeGrid);
			}
			RegisterCubeGrid(cubeGrid, myLocalCoordSys);
			MyCreateCoordSysBuffer arg = default(MyCreateCoordSysBuffer);
			arg.Position = origin.Position;
			arg.Rotation = origin.Rotation;
			arg.Id = num;
			if (sync)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CoordSysCreated_Client, arg);
			}
		}

		public static void GetPosRoundedToGrid(ref Vector3D vecToRound, double gridSize, bool isStaticGridAlignToCenter)
		{
			if (isStaticGridAlignToCenter)
			{
				vecToRound = Vector3L.Round(vecToRound / gridSize) * gridSize;
			}
			else
			{
				vecToRound = Vector3L.Round(vecToRound / gridSize + 0.5) * gridSize - 0.5 * gridSize;
			}
		}

<<<<<<< HEAD
		[Event(null, 325)]
=======
		[Event(null, 326)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void CoorSysRemoved_Client(long coordSysId)
		{
			Static.RemoveCoordSys(coordSysId);
		}

		/// <summary>
		/// Removes coord system.
		/// </summary>
		/// <param name="coordSysId">Coord system id.</param>
		private void RemoveCoordSys(long coordSysId)
		{
			m_localCoordSystems.Remove(coordSysId);
		}

		private void MyEntities_OnEntityCreate(MyEntity obj)
		{
			MyCubeGrid myCubeGrid = obj as MyCubeGrid;
			if (myCubeGrid != null && myCubeGrid.LocalCoordSystem != 0L)
			{
				MyLocalCoordSys coordSysById = GetCoordSysById(myCubeGrid.LocalCoordSystem);
				if (coordSysById != null)
				{
					RegisterCubeGrid(myCubeGrid, coordSysById);
				}
			}
		}

		/// <summary>
		/// Registers cube grid under closest coord system.
		/// </summary>
		/// <param name="cubeGrid">Cube grid to register.</param>
		public void RegisterCubeGrid(MyCubeGrid cubeGrid)
		{
			if (cubeGrid.LocalCoordSystem == 0L)
			{
				Vector3D position = cubeGrid.PositionComp.GetPosition();
				MyLocalCoordSys closestCoordSys = GetClosestCoordSys(ref position);
				if (closestCoordSys != null)
				{
					RegisterCubeGrid(cubeGrid, closestCoordSys);
				}
			}
		}

		/// <summary>
		/// Registers cube grid under given local coord system.
		/// </summary>
		/// <param name="cubeGrid">Cube grid to register.</param>
		/// <param name="coordSys">Local coord system.</param>
		private void RegisterCubeGrid(MyCubeGrid cubeGrid, MyLocalCoordSys coordSys)
		{
			cubeGrid.OnClose += CubeGrid_OnClose;
			cubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			cubeGrid.LocalCoordSystem = coordSys.Id;
			coordSys.EntityCounter++;
		}

		/// <summary>
		/// Unregisters cube grid from given local coord system.
		/// </summary>
		/// <param name="cubeGrid">Cube grid to be unregistered.</param>
		private void UnregisterCubeGrid(MyCubeGrid cubeGrid)
		{
			cubeGrid.OnClose -= CubeGrid_OnClose;
			cubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			long localCoordSystem = cubeGrid.LocalCoordSystem;
			MyLocalCoordSys coordSysById = GetCoordSysById(localCoordSystem);
			cubeGrid.LocalCoordSystem = 0L;
			if (coordSysById == null)
			{
				return;
			}
			coordSysById.EntityCounter--;
			if (coordSysById.EntityCounter <= 0)
			{
				RemoveCoordSys(coordSysById.Id);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CoorSysRemoved_Client, localCoordSystem);
			}
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			MyCubeGrid myCubeGrid = obj as MyCubeGrid;
			if (myCubeGrid != null && !myCubeGrid.IsStatic)
			{
				UnregisterCubeGrid(myCubeGrid);
			}
		}

		private void CubeGrid_OnClose(MyEntity obj)
		{
			MyCubeGrid myCubeGrid = obj as MyCubeGrid;
			if (myCubeGrid != null)
			{
				UnregisterCubeGrid(myCubeGrid);
			}
		}

		private MyLocalCoordSys GetCoordSysById(long id)
		{
			if (m_localCoordSystems.ContainsKey(id))
			{
				return m_localCoordSystems[id];
			}
			return null;
		}

		/// <summary>
		/// Converts world position to be snapped to closest grid.
		/// </summary>
		/// <param name="worldPos">World position.</param>
		/// <param name="gridSize">Grid size.</param>
		/// <param name="staticGridAlignToCenter">Is grid align to static.</param>
		/// <param name="id">Id of a concrete coord system.</param>
		/// <returns></returns>
		public CoordSystemData SnapWorldPosToClosestGrid(ref Vector3D worldPos, double gridSize, bool staticGridAlignToCenter, long? id = null)
		{
			m_lastSelectedCoordSys = m_selectedCoordSys;
			MyLocalCoordSys myLocalCoordSys = ((id.HasValue && id.Value != 0L) ? GetCoordSysById(id.Value) : GetClosestCoordSys(ref worldPos));
			if (myLocalCoordSys == null)
			{
				myLocalCoordSys = new MyLocalCoordSys(new MyTransformD(Vector3L.Round(worldPos / gridSize) * gridSize), m_coorsSystemSize);
				m_selectedCoordSys = 0L;
			}
			else
			{
				m_selectedCoordSys = myLocalCoordSys.Id;
			}
			if (m_selectedCoordSys == 0L)
			{
				m_localCoordExist = false;
			}
			else
			{
				m_localCoordExist = true;
			}
			if (m_selectedCoordSys != m_lastSelectedCoordSys && MyCoordinateSystem.OnCoordinateChange != null)
			{
				MyCoordinateSystem.OnCoordinateChange();
			}
			CoordSystemData result = default(CoordSystemData);
			Quaternion rotation = myLocalCoordSys.Origin.Rotation;
			Quaternion rotation2 = Quaternion.Inverse(rotation);
			Vector3D position = myLocalCoordSys.Origin.Position;
			Vector3D value = worldPos - position;
			value = Vector3D.Transform(value, rotation2);
			GetPosRoundedToGrid(ref value, gridSize, staticGridAlignToCenter);
			result.Id = m_selectedCoordSys;
			result.LocalSnappedPos = value;
			value = Vector3D.Transform(value, rotation);
			MyTransformD snappedTransform = default(MyTransformD);
			snappedTransform.Position = position + value;
			snappedTransform.Rotation = rotation;
			result.SnappedTransform = snappedTransform;
			result.Origin = myLocalCoordSys.Origin;
			return result;
		}

		/// <summary>
		/// Indicates if position is inside of local coordinates area.
		/// </summary>
		/// <param name="worldPos">World position.</param>
		/// <returns>If true, position is inside of closest local coordinate system.</returns>
		public bool IsAnyLocalCoordSysExist(ref Vector3D worldPos)
		{
			foreach (MyLocalCoordSys value in m_localCoordSystems.Values)
			{
				if (value.Contains(ref worldPos))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Naive way of checking if transform is snaped to coord sys. In second iteration update for better way.
		/// </summary>
		/// <param name="tranform">Transform to check</param>
		/// <param name="gridSize"></param>
		/// <returns>Indicates if transform is aligned correctly to any grid system.</returns>
		public bool IsLocalCoordSysExist(ref MatrixD tranform, double gridSize)
		{
			foreach (MyLocalCoordSys value2 in m_localCoordSystems.Values)
			{
				Vector3D vec = tranform.Translation;
				if (!value2.Contains(ref vec))
<<<<<<< HEAD
				{
					continue;
				}
				double num = Math.Abs(Vector3D.Dot(value2.Origin.Rotation.Forward, tranform.Forward));
				double num2 = Math.Abs(Vector3D.Dot(value2.Origin.Rotation.Up, tranform.Up));
				if ((num < m_angleTolerance || num > 1.0 - m_angleTolerance) && (num2 < m_angleTolerance || num2 > 1.0 - m_angleTolerance))
				{
=======
				{
					continue;
				}
				double num = Math.Abs(Vector3D.Dot(value2.Origin.Rotation.Forward, tranform.Forward));
				double num2 = Math.Abs(Vector3D.Dot(value2.Origin.Rotation.Up, tranform.Up));
				if ((num < m_angleTolerance || num > 1.0 - m_angleTolerance) && (num2 < m_angleTolerance || num2 > 1.0 - m_angleTolerance))
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Vector3D value = vec - value2.Origin.Position;
					Quaternion rotation = Quaternion.Inverse(value2.Origin.Rotation);
					Vector3D vector3D = Vector3D.Transform(value, rotation);
					double num3 = gridSize / 2.0;
					double num4 = Math.Abs(vector3D.X % num3);
					double num5 = Math.Abs(vector3D.Y % num3);
					double num6 = Math.Abs(vector3D.Z % num3);
					if ((num4 < m_positionTolerance || num4 > num3 - m_positionTolerance) && (num5 < m_positionTolerance || num5 > num3 - m_positionTolerance) && (num6 < m_positionTolerance || num6 > num3 - m_positionTolerance))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Sets last and current selected coordinate system to none.
		/// </summary>
		public void ResetSelection()
		{
			m_lastSelectedCoordSys = 0L;
			m_selectedCoordSys = 0L;
			m_drawBoundingBox = false;
		}

		public override void Draw()
		{
			if (!m_visible)
			{
				return;
			}
			if (m_selectedCoordSys == 0L)
			{
				m_drawBoundingBox = false;
			}
			else if (m_selectedCoordSys != 0L)
			{
				m_drawBoundingBox = true;
			}
			if (MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS)
			{
				foreach (MyLocalCoordSys value in m_localCoordSystems.Values)
				{
					value.Draw();
				}
			}
			else if (m_drawBoundingBox)
			{
				GetCoordSysById(m_selectedCoordSys)?.Draw();
			}
			base.Draw();
		}

		/// <summary>
		/// Gets local coordinate system indication color.
		/// </summary>
		/// <param name="coordSysId">Local coordinate system ID.</param>
		/// <returns>Indication color.</returns>
		public Color GetCoordSysColor(long coordSysId)
		{
			if (m_localCoordSystems.ContainsKey(coordSysId))
			{
				return m_localCoordSystems[coordSysId].RenderColor;
			}
			return Color.White;
		}
	}
}
