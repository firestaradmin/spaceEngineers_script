using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public abstract class MyVoxelBase : MyEntity, IMyVoxelDrawable, IMyVoxelBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyDecalProxy, IMyEventProxy, IMyEventOwner
	{
		[Serializable]
		private struct MyRampShapeParams
		{
			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_003C_003EBox_003C_003EAccessor : IMemberAccessor<MyRampShapeParams, BoundingBoxD>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRampShapeParams owner, in BoundingBoxD value)
				{
					owner.Box = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRampShapeParams owner, out BoundingBoxD value)
				{
					value = owner.Box;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_003C_003ERampNormal_003C_003EAccessor : IMemberAccessor<MyRampShapeParams, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRampShapeParams owner, in Vector3D value)
				{
					owner.RampNormal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRampShapeParams owner, out Vector3D value)
				{
					value = owner.RampNormal;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_003C_003ERampNormalW_003C_003EAccessor : IMemberAccessor<MyRampShapeParams, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRampShapeParams owner, in double value)
				{
					owner.RampNormalW = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRampShapeParams owner, out double value)
				{
					value = owner.RampNormalW;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_003C_003ETransformation_003C_003EAccessor : IMemberAccessor<MyRampShapeParams, MatrixD>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRampShapeParams owner, in MatrixD value)
				{
					owner.Transformation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRampShapeParams owner, out MatrixD value)
				{
					value = owner.Transformation;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyRampShapeParams, byte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRampShapeParams owner, in byte value)
				{
					owner.Material = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRampShapeParams owner, out byte value)
				{
					value = owner.Material;
				}
			}

			public BoundingBoxD Box;

			public Vector3D RampNormal;

			public double RampNormalW;

			public MatrixD Transformation;

			public byte Material;
		}

		[Serializable]
		private struct MyCapsuleShapeParams
		{
			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_003C_003EA_003C_003EAccessor : IMemberAccessor<MyCapsuleShapeParams, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCapsuleShapeParams owner, in Vector3D value)
				{
					owner.A = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCapsuleShapeParams owner, out Vector3D value)
				{
					value = owner.A;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_003C_003EB_003C_003EAccessor : IMemberAccessor<MyCapsuleShapeParams, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCapsuleShapeParams owner, in Vector3D value)
				{
					owner.B = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCapsuleShapeParams owner, out Vector3D value)
				{
					value = owner.B;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_003C_003ERadius_003C_003EAccessor : IMemberAccessor<MyCapsuleShapeParams, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCapsuleShapeParams owner, in float value)
				{
					owner.Radius = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCapsuleShapeParams owner, out float value)
				{
					value = owner.Radius;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_003C_003ETransformation_003C_003EAccessor : IMemberAccessor<MyCapsuleShapeParams, MatrixD>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCapsuleShapeParams owner, in MatrixD value)
				{
					owner.Transformation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCapsuleShapeParams owner, out MatrixD value)
				{
					value = owner.Transformation;
				}
			}

			protected class Sandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyCapsuleShapeParams, byte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyCapsuleShapeParams owner, in byte value)
				{
					owner.Material = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyCapsuleShapeParams owner, out byte value)
				{
					value = owner.Material;
				}
			}

			public Vector3D A;

			public Vector3D B;

			public float Radius;

			public MatrixD Transformation;

			public byte Material;
		}

		public enum OperationType : byte
		{
			Fill,
			Paint,
			Cut,
			Revert
		}

		public delegate void StorageChanged(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData);

		public delegate void OnCutOutResults(float voxelsCountInPercent, MyVoxelMaterialDefinition voxelMaterial, Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials);

		protected sealed class VoxelCutoutSphere_Implementation_003C_003EVRageMath_Vector3D_0023System_Single_0023System_Boolean_0023System_Boolean : ICallSite<MyVoxelBase, Vector3D, float, bool, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in Vector3D center, in float radius, in bool createDebris, in bool damage, in DBNull arg5, in DBNull arg6)
			{
				@this.VoxelCutoutSphere_Implementation(center, radius, createDebris, damage);
			}
		}

		protected sealed class VoxelOperationCapsule_Implementation_003C_003ESystem_Int64_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<IMyEventOwner, long, MyCapsuleShapeParams, OperationType, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in MyCapsuleShapeParams capsuleParams, in OperationType Type, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				VoxelOperationCapsule_Implementation(entityId, capsuleParams, Type);
			}
		}

		protected sealed class PerformVoxelOperationCapsule_Implementation_003C_003ESandbox_Game_Entities_MyVoxelBase_003C_003EMyCapsuleShapeParams_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<MyVoxelBase, MyCapsuleShapeParams, OperationType, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in MyCapsuleShapeParams capsuleParams, in OperationType Type, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformVoxelOperationCapsule_Implementation(capsuleParams, Type);
			}
		}

		protected sealed class VoxelOperationSphere_Implementation_003C_003ESystem_Int64_0023VRageMath_Vector3D_0023System_Single_0023System_Byte_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<IMyEventOwner, long, Vector3D, float, byte, OperationType, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3D center, in float radius, in byte material, in OperationType Type, in DBNull arg6)
			{
				VoxelOperationSphere_Implementation(entityId, center, radius, material, Type);
			}
		}

		protected sealed class PerformVoxelOperationSphere_Implementation_003C_003EVRageMath_Vector3D_0023System_Single_0023System_Byte_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<MyVoxelBase, Vector3D, float, byte, OperationType, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in Vector3D center, in float radius, in byte material, in OperationType Type, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformVoxelOperationSphere_Implementation(center, radius, material, Type);
			}
		}

		protected sealed class VoxelOperationBox_Implementation_003C_003ESystem_Int64_0023VRageMath_BoundingBoxD_0023VRageMath_MatrixD_0023System_Byte_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<IMyEventOwner, long, BoundingBoxD, MatrixD, byte, OperationType, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in BoundingBoxD box, in MatrixD Transformation, in byte material, in OperationType Type, in DBNull arg6)
			{
				VoxelOperationBox_Implementation(entityId, box, Transformation, material, Type);
			}
		}

		protected sealed class PerformVoxelOperationBox_Implementation_003C_003EVRageMath_BoundingBoxD_0023VRageMath_MatrixD_0023System_Byte_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<MyVoxelBase, BoundingBoxD, MatrixD, byte, OperationType, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in BoundingBoxD box, in MatrixD Transformation, in byte material, in OperationType Type, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformVoxelOperationBox_Implementation(box, Transformation, material, Type);
			}
		}

		protected sealed class VoxelOperationRamp_Implementation_003C_003ESystem_Int64_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<IMyEventOwner, long, MyRampShapeParams, OperationType, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in MyRampShapeParams shapeParams, in OperationType Type, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				VoxelOperationRamp_Implementation(entityId, shapeParams, Type);
			}
		}

		protected sealed class PerformVoxelOperationRamp_Implementation_003C_003ESandbox_Game_Entities_MyVoxelBase_003C_003EMyRampShapeParams_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<MyVoxelBase, MyRampShapeParams, OperationType, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in MyRampShapeParams shapeParams, in OperationType Type, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformVoxelOperationRamp_Implementation(shapeParams, Type);
			}
		}

		protected sealed class VoxelOperationElipsoid_Implementation_003C_003ESystem_Int64_0023VRageMath_Vector3_0023VRageMath_MatrixD_0023System_Byte_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<IMyEventOwner, long, Vector3, MatrixD, byte, OperationType, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3 radius, in MatrixD Transformation, in byte material, in OperationType Type, in DBNull arg6)
			{
				VoxelOperationElipsoid_Implementation(entityId, radius, Transformation, material, Type);
			}
		}

		protected sealed class PerformVoxelOperationElipsoid_Implementation_003C_003EVRageMath_Vector3_0023VRageMath_MatrixD_0023System_Byte_0023Sandbox_Game_Entities_MyVoxelBase_003C_003EOperationType : ICallSite<MyVoxelBase, Vector3, MatrixD, byte, OperationType, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in Vector3 radius, in MatrixD Transformation, in byte material, in OperationType Type, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformVoxelOperationElipsoid_Implementation(radius, Transformation, material, Type);
			}
		}

		protected sealed class RevertVoxelAccess_003C_003EVRageMath_Vector3I_0023VRage_Voxels_MyStorageDataTypeFlags : ICallSite<MyVoxelBase, Vector3I, MyStorageDataTypeFlags, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in Vector3I key, in MyStorageDataTypeFlags flags, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RevertVoxelAccess(key, flags);
			}
		}

		protected sealed class PerformCutOutSphereFast_003C_003EVRageMath_Vector3D_0023System_Single_0023System_Boolean : ICallSite<MyVoxelBase, Vector3D, float, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in Vector3D center, in float radius, in bool notify, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformCutOutSphereFast(center, radius, notify);
			}
		}

		protected sealed class PerformShipCutout_003C_003ESandbox_Game_GameSystems_MyShipMiningSystem_003C_003ENetworkCutoutData : ICallSite<MyVoxelBase, MyShipMiningSystem.NetworkCutoutData, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in MyShipMiningSystem.NetworkCutoutData data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PerformShipCutout(data);
			}
		}

		protected sealed class CreateVoxelMeteorCrater_Implementation_003C_003EVRageMath_Vector3D_0023System_Single_0023VRageMath_Vector3_0023System_Byte : ICallSite<MyVoxelBase, Vector3D, float, Vector3, byte, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVoxelBase @this, in Vector3D center, in float radius, in Vector3 direction, in byte material, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateVoxelMeteorCrater_Implementation(center, radius, direction, material);
			}
		}

		public int VoxelMapPruningProxyId = -1;

		protected Vector3I m_storageMin = new Vector3I(0, 0, 0);

		protected Vector3I m_storageMax;

		public bool? IsSeedOpen;

		private VRage.Game.Voxels.IMyStorage m_storageInternal;

		public bool CreateStorageCopyOnWrite;

		private bool m_contentChanged;

		private bool m_beforeContentChanged;

		[ThreadStatic]
		protected static MyStorageData m_tempStorage;

		private static readonly MyShapeSphere m_sphereShape = new MyShapeSphere();

		private static readonly MyShapeBox m_boxShape = new MyShapeBox();

		private static readonly MyShapeRamp m_rampShape = new MyShapeRamp();

		private static readonly MyShapeCapsule m_capsuleShape = new MyShapeCapsule();

		private static readonly MyShapeEllipsoid m_ellipsoidShape = new MyShapeEllipsoid();

		private static readonly List<MyEntity> m_foundElements = new List<MyEntity>();

		public MyBoulderInformation? BoulderInfo;

		private bool m_createdByUser;

		private bool m_voxelShapeInProgress;

		public Vector3I StorageMin => m_storageMin;

		public Vector3I StorageMax => m_storageMax;

		public string StorageName { get; protected set; }

		public float VoxelSize { get; private set; }

		protected VRage.Game.Voxels.IMyStorage m_storage
		{
			get
			{
				return m_storageInternal;
			}
			set
			{
				if (value != null && !value.Shared)
				{
					MyStorageBase myStorageBase = value as MyStorageBase;
					if (myStorageBase != null && !myStorageBase.CachedWrites)
					{
						myStorageBase.InitWriteCache();
					}
				}
				m_storageInternal = value;
			}
		}

		public new virtual VRage.Game.Voxels.IMyStorage Storage
		{
			get
			{
				return m_storage;
			}
			set
			{
				m_storage = value;
			}
		}

		public bool DelayRigidBodyCreation { get; set; }

		/// <summary>
		/// Size of voxel map (in voxels)
		/// </summary>
		public Vector3I Size => m_storageMax - m_storageMin;

		public Vector3I SizeMinusOne => Size - 1;

<<<<<<< HEAD
		/// <summary>
		/// Size of voxel map (in metres)
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Vector3 SizeInMetres { get; protected set; }

		public Vector3 SizeInMetresHalf { get; protected set; }

<<<<<<< HEAD
		/// <summary>
		/// Position of left/bottom corner of this voxel map, in world space (not relative to sector)
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public virtual Vector3D PositionLeftBottomCorner { get; set; }

		public Matrix Orientation => base.PositionComp.WorldMatrixRef;

		public bool ContentChanged
		{
			get
			{
				return m_contentChanged;
			}
			set
			{
				m_contentChanged = value;
				BeforeContentChanged = false;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Gets root voxel, for asteroids and planets itself.
		/// For MyVoxelPhysics, should return owning planet.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public abstract MyVoxelBase RootVoxel { get; }

		public bool BeforeContentChanged
		{
			get
			{
				return m_beforeContentChanged;
			}
			set
			{
				if (m_beforeContentChanged != value)
				{
					m_beforeContentChanged = value;
					if (m_beforeContentChanged && Storage != null && Storage.Shared && m_storage != null)
					{
						Storage = m_storage.Copy();
						StorageName = MyVoxelMap.GetNewStorageName(StorageName, base.EntityId);
					}
				}
			}
		}

		public bool CreatedByUser
		{
			get
			{
				return m_createdByUser;
			}
			set
			{
				m_createdByUser = value;
			}
		}

		public string AsteroidName { get; set; }

		VRage.ModAPI.IMyStorage IMyVoxelBase.Storage => Storage;

		string IMyVoxelBase.StorageName => StorageName;

		public virtual MyClipmapScaleEnum ScaleGroup => MyClipmapScaleEnum.Normal;

		public event StorageChanged RangeChanged;

		protected internal void OnRangeChanged(Vector3I voxelRangeMin, Vector3I voxelRangeMax, MyStorageDataTypeFlags changedData)
		{
			if (this.RangeChanged != null)
			{
				this.RangeChanged(this, voxelRangeMin, voxelRangeMax, changedData);
			}
		}

		public bool IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref BoundingBoxD boundingBox)
		{
			base.PositionComp.WorldAABB.Intersects(ref boundingBox, out var result);
			return result;
		}

		public MyVoxelBase()
		{
			VoxelSize = 1f;
		}

		public abstract void Init(MyObjectBuilder_EntityBase builder, VRage.Game.Voxels.IMyStorage storage);

		public void Init(string storageName, VRage.Game.Voxels.IMyStorage storage, Vector3D positionMinCorner)
		{
			MatrixD worldMatrix = MatrixD.CreateTranslation(positionMinCorner + storage.Size / 2);
			Init(storageName, storage, worldMatrix);
		}

		public virtual void Init(string storageName, VRage.Game.Voxels.IMyStorage storage, MatrixD worldMatrix, bool useVoxelOffset = true)
		{
			if (base.Name == null)
			{
				base.Init(null);
			}
			StorageName = storageName;
			m_storage = storage;
			InitVoxelMap(worldMatrix, storage.Size, useVoxelOffset);
		}

		protected virtual void InitVoxelMap(MatrixD worldMatrix, Vector3I size, bool useOffset = true)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			SizeInMetres = size * 1f;
			SizeInMetresHalf = SizeInMetres / 2f;
			base.PositionComp.LocalAABB = new BoundingBox(-SizeInMetresHalf, SizeInMetresHalf);
			if (MyPerGameSettings.OffsetVoxelMapByHalfVoxel && useOffset)
			{
				worldMatrix.Translation += 0.5f;
				PositionLeftBottomCorner += 0.5f;
			}
			base.PositionComp.SetWorldMatrix(ref worldMatrix);
			ContentChanged = false;
		}

		protected override void BeforeDelete()
		{
			base.BeforeDelete();
			this.RangeChanged = null;
			if (Storage != null && !Storage.Shared && !(this is MyVoxelPhysics))
			{
				Storage.Close();
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_VoxelMap myObjectBuilder_VoxelMap = (MyObjectBuilder_VoxelMap)base.GetObjectBuilder(copy);
			Vector3D positionLeftBottomCorner = PositionLeftBottomCorner;
			PositionLeftBottomCorner = base.WorldMatrix.Translation - Vector3D.TransformNormal(SizeInMetresHalf, base.WorldMatrix);
			if (MyPerGameSettings.OffsetVoxelMapByHalfVoxel)
			{
				positionLeftBottomCorner -= 0.5;
			}
			myObjectBuilder_VoxelMap.PositionAndOrientation = new MyPositionAndOrientation(positionLeftBottomCorner, base.WorldMatrix.Forward, base.WorldMatrix.Up);
			myObjectBuilder_VoxelMap.StorageName = StorageName;
			myObjectBuilder_VoxelMap.MutableStorage = true;
			myObjectBuilder_VoxelMap.ContentChanged = ContentChanged;
			myObjectBuilder_VoxelMap.CreatedByUser = CreatedByUser;
			if (BoulderInfo.HasValue)
			{
				myObjectBuilder_VoxelMap.BoulderPlanetId = BoulderInfo.Value.PlanetId;
				myObjectBuilder_VoxelMap.BoulderSectorId = BoulderInfo.Value.SectorId;
				myObjectBuilder_VoxelMap.BoulderItemId = BoulderInfo.Value.ItemId;
			}
			else
			{
				myObjectBuilder_VoxelMap.BoulderPlanetId = (myObjectBuilder_VoxelMap.BoulderSectorId = (myObjectBuilder_VoxelMap.BoulderItemId = null));
			}
			return myObjectBuilder_VoxelMap;
		}

		protected void WorldPositionChanged(object source)
		{
			PositionLeftBottomCorner = base.WorldMatrix.Translation - Vector3D.TransformNormal(SizeInMetresHalf, base.WorldMatrix);
		}

		/// <summary>
		/// Calculates amount of volume of a bounding box in voxels.
		/// </summary>
		/// <param name="localAabb">Local bounding box to query for.</param>
		/// <param name="worldMatrix">World matrix of the bounding box.</param>
		/// <param name="stopIfFindAtLeastOneContent"></param>
		/// <param name="threshold">Wipes out TOO small voxels</param>
		/// <returns>Pair of floats where 1st value is Volume amount and 2nd value is ratio of Volume amount to Whole volume.</returns>
		public MyTuple<float, float> GetVoxelContentInBoundingBox_Fast(BoundingBoxD localAabb, MatrixD worldMatrix, bool stopIfFindAtLeastOneContent = false, float threshold = 0.1f)
		{
			MatrixD matrix = worldMatrix * base.PositionComp.WorldMatrixNormalizedInv;
			MatrixD.Invert(ref matrix, out var _);
<<<<<<< HEAD
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(localAabb.GetInflated(0.11999999731779099), worldMatrix);
=======
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(localAabb, worldMatrix);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			BoundingBoxD boundingBoxD = myOrientedBoundingBoxD.GetAABB().TransformFast(base.PositionComp.WorldMatrixNormalizedInv);
			boundingBoxD.Translate(SizeInMetresHalf + StorageMin);
			Vector3I vector3I = Vector3I.Floor(boundingBoxD.Min);
			Vector3I vector3I2 = Vector3I.Ceiling(boundingBoxD.Max);
			int num = Math.Max((MathHelper.Log2Ceiling((int)(localAabb.Volume / 1.0)) - MathHelper.Log2Ceiling(100)) / 3, 0);
			float num2 = 1f * (float)(1 << num);
			float num3 = num2 * num2 * num2;
			vector3I >>= num;
			vector3I2 >>= num;
			_ = (Size >> 1) + StorageMin >> num;
			if (m_tempStorage == null)
			{
				m_tempStorage = new MyStorageData();
			}
			m_tempStorage.Resize(vector3I2 - vector3I + 1);
			Storage.ReadRange(m_tempStorage, MyStorageDataTypeFlags.Content, num, vector3I, vector3I2);
			float num4 = 0f;
			float num5 = 0f;
			int num6 = 0;
<<<<<<< HEAD
			MatrixD worldMatrix2 = base.WorldMatrix;
			worldMatrix2.Translation = Vector3D.Zero;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector3I vector3I3 = default(Vector3I);
			vector3I3.Z = vector3I.Z;
			Vector3I p = default(Vector3I);
			p.Z = 0;
			while (vector3I3.Z <= vector3I2.Z)
			{
				vector3I3.Y = vector3I.Y;
				p.Y = 0;
				while (vector3I3.Y <= vector3I2.Y)
				{
					vector3I3.X = vector3I.X;
					p.X = 0;
					while (vector3I3.X <= vector3I2.X)
					{
<<<<<<< HEAD
						BoundingBoxD box = default(BoundingBoxD);
						box.Min = ((Vector3D)vector3I3 - 0.5) * num2 - SizeInMetresHalf;
						box.Max = ((Vector3D)vector3I3 + 0.5) * num2 - SizeInMetresHalf;
						MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(box, worldMatrix2);
						other.Center += base.WorldMatrix.Translation;
						ContainmentType containmentType = myOrientedBoundingBoxD.Contains(ref other);
						if (containmentType != 0)
						{
							float num7 = 1f;
							if (containmentType == ContainmentType.Intersects)
							{
								num7 = (myOrientedBoundingBoxD.Contains(ref other.Center) ? 0.75f : 0.5f);
							}
							float num8 = (float)(int)m_tempStorage.Content(ref p) / 255f * num7;
							if (num8 < threshold)
							{
								num8 = 0f;
							}
							num4 += num8 * num3;
							num5 += num8;
							num6++;
							if (stopIfFindAtLeastOneContent && num8 > 0f)
							{
								num5 /= (float)num6;
								return new MyTuple<float, float>(num4, num5);
=======
						float num7 = (float)(int)m_tempStorage.Content(ref p) / 255f;
						if (!(num7 <= 0f))
						{
							BoundingBoxD box = default(BoundingBoxD);
							box.Min = ((Vector3D)vector3I3 - 0.5) * num2 - SizeInMetresHalf;
							box.Max = ((Vector3D)vector3I3 + 0.5) * num2 - SizeInMetresHalf;
							MatrixD worldMatrix2 = base.WorldMatrix;
							worldMatrix2.Translation = Vector3D.Zero;
							MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(box, worldMatrix2);
							other.Center += base.WorldMatrix.Translation;
							if (myOrientedBoundingBoxD.Contains(ref other) != 0)
							{
								num4 += num7 * num3;
								num5 += num7;
								num6++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
						vector3I3.X++;
						p.X++;
					}
					vector3I3.Y++;
					p.Y++;
				}
				vector3I3.Z++;
				p.Z++;
			}
			num5 /= (float)num6;
			return new MyTuple<float, float>(num4, num5);
		}

		public override bool GetIntersectionWithLine(ref LineD worldLine, out MyIntersectionResultLineTriangleEx? t, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			t = null;
			if (!base.PositionComp.WorldAABB.Intersects(ref worldLine, out var _))
			{
				return false;
			}
			try
			{
				Line localLine = new Line(worldLine.From - PositionLeftBottomCorner, worldLine.To - PositionLeftBottomCorner);
				if (Storage.GetGeometry().Intersect(ref localLine, out var result, flags))
				{
					t = new MyIntersectionResultLineTriangleEx(result, this, ref localLine);
					_ = t.Value;
					return true;
				}
				t = null;
				return false;
			}
			finally
			{
			}
		}

		public override bool GetIntersectionWithLine(ref LineD worldLine, out Vector3D? v, bool useCollisionModel = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			GetIntersectionWithLine(ref worldLine, out var t, IntersectionFlags.ALL_TRIANGLES);
			v = null;
			if (t.HasValue)
			{
				v = t.Value.IntersectionPointInWorldSpace;
				return true;
			}
			return false;
		}

		public bool AreAllAabbCornersInside(ref MatrixD aabbWorldTransform, BoundingBoxD aabb)
		{
			return CountCornersInside(ref aabbWorldTransform, ref aabb) == 8;
		}

		public bool IsAnyAabbCornerInside(ref MatrixD aabbWorldTransform, BoundingBoxD aabb)
		{
			return CountCornersInside(ref aabbWorldTransform, ref aabb) > 0;
		}

		private unsafe int CountCornersInside(ref MatrixD aabbWorldTransform, ref BoundingBoxD aabb)
		{
			Vector3D* ptr = stackalloc Vector3D[8];
			aabb.GetCornersUnsafe(ptr);
			for (int i = 0; i < 8; i++)
			{
				Vector3D.Transform(ref ptr[i], ref aabbWorldTransform, out ptr[i]);
			}
			return CountPointsInside(ptr, 8);
		}

		public unsafe int CountPointsInside(Vector3D* worldPoints, int pointCount)
		{
			if (m_tempStorage == null)
			{
				m_tempStorage = new MyStorageData();
			}
			MatrixD matrix = base.PositionComp.WorldMatrixInvScaled;
			int num = 0;
			Vector3I vector3I = new Vector3I(int.MaxValue);
			Vector3I vector3I2 = new Vector3I(int.MinValue);
			for (int i = 0; i < pointCount; i++)
			{
				Vector3D.Transform(ref worldPoints[i], ref matrix, out var result);
				Vector3D o = result + (Vector3D)(Size / 2);
				Vector3I vector3I3 = Vector3D.Floor(o);
				Vector3D.Fract(ref o, out o);
				vector3I3 += StorageMin;
				Vector3I vector3I4 = vector3I3 + 1;
				if (vector3I3 != vector3I && vector3I4 != vector3I2)
				{
					m_tempStorage.Resize(vector3I3, vector3I4);
					Storage.ReadRange(m_tempStorage, MyStorageDataTypeFlags.Content, 0, vector3I3, vector3I4);
					vector3I = vector3I3;
					vector3I2 = vector3I4;
				}
				double num2 = (int)m_tempStorage.Content(0, 0, 0);
				double num3 = (int)m_tempStorage.Content(1, 0, 0);
				double num4 = (int)m_tempStorage.Content(0, 1, 0);
				double num5 = (int)m_tempStorage.Content(1, 1, 0);
				double num6 = (int)m_tempStorage.Content(0, 0, 1);
				double num7 = (int)m_tempStorage.Content(1, 0, 1);
				double num8 = (int)m_tempStorage.Content(0, 1, 1);
				double num9 = (int)m_tempStorage.Content(1, 1, 1);
				num2 += (num3 - num2) * o.X;
				num4 += (num5 - num4) * o.X;
				num6 += (num7 - num6) * o.X;
				num8 += (num9 - num8) * o.X;
				num2 += (num4 - num2) * o.Y;
				num6 += (num8 - num6) * o.Y;
				num2 += (num6 - num2) * o.Z;
				if (num2 >= 127.0)
				{
					num++;
				}
			}
			return num;
		}

		public virtual bool IsOverlapOverThreshold(BoundingBoxD worldAabb, float thresholdPercentage = 0.9f)
		{
			return false;
		}

		public override bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos)
		{
			if (Storage.Closed)
			{
				return false;
			}
			spherePos = Vector3D.Transform(spherePos, base.PositionComp.WorldMatrixInvScaled);
			spherePos /= (double)VoxelSize;
			sphereRadius /= VoxelSize;
			spherePos += SizeInMetresHalf;
			return OverlapsSphereLocal(sphereRadius, spherePos);
		}

		internal void RevertProceduralAsteroidVoxelSettings()
		{
			if (!CreatedByUser)
			{
				base.Save = false;
			}
			ContentChanged = false;
			BeforeContentChanged = false;
		}

		protected bool OverlapsSphereLocal(float sphereRadius, Vector3D spherePos)
		{
			double num = sphereRadius * sphereRadius;
			Vector3I voxelCoord = new Vector3I(spherePos - sphereRadius);
			Vector3I voxelCoord2 = new Vector3I(spherePos + sphereRadius);
			Storage.ClampVoxelCoord(ref voxelCoord);
			Storage.ClampVoxelCoord(ref voxelCoord2);
			BoundingBoxI box = new BoundingBoxI(voxelCoord, voxelCoord2);
			if (Storage.Intersect(ref box, 0) == ContainmentType.Disjoint)
			{
				return false;
			}
			if (m_tempStorage == null)
			{
				m_tempStorage = new MyStorageData();
			}
			m_tempStorage.Resize(voxelCoord, voxelCoord2);
			Storage.ReadRange(m_tempStorage, MyStorageDataTypeFlags.Content, 0, voxelCoord, voxelCoord2);
			Vector3I vector3I = default(Vector3I);
			vector3I.Z = voxelCoord.Z;
			Vector3I p = default(Vector3I);
			p.Z = 0;
			while (vector3I.Z <= voxelCoord2.Z)
			{
				vector3I.Y = voxelCoord.Y;
				p.Y = 0;
				while (vector3I.Y <= voxelCoord2.Y)
				{
					vector3I.X = voxelCoord.X;
					p.X = 0;
					while (vector3I.X <= voxelCoord2.X)
					{
						if (m_tempStorage.Content(ref p) >= 127)
						{
							Vector3 max = vector3I + VoxelSize;
							if (new BoundingBox(vector3I, max).Contains(spherePos) == ContainmentType.Contains)
							{
								return true;
							}
							if (Vector3D.DistanceSquared(vector3I, spherePos) < num)
							{
								return true;
							}
						}
						vector3I.X++;
						p.X++;
					}
					vector3I.Y++;
					p.Y++;
				}
				vector3I.Z++;
				p.Z++;
			}
			return false;
		}

		public bool GetContainedVoxelCoords(ref BoundingBoxD worldAabb, out Vector3I min, out Vector3I max)
		{
			min = default(Vector3I);
			max = default(Vector3I);
			if (!IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref worldAabb))
			{
				return false;
			}
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldAabb.Min, out min);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldAabb.Max, out max);
			min += StorageMin;
			max += StorageMin;
			Storage.ClampVoxelCoord(ref min);
			Storage.ClampVoxelCoord(ref max);
			return true;
		}

		/// We use this to ensure voxels overlapping planets will have priority when spawning debris/ores.
		public virtual int GetOrePriority()
		{
			return 1;
		}

<<<<<<< HEAD
		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, Vector3 forwardDirection, object customdata, IMyDecalHandler decalHandler, MyStringHash physicalMaterial, MyStringHash voxelMaterial, bool isTrail, MyDecalFlags flags, int aliveUntil, List<uint> decals)
=======
		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, Vector3 forwardDirection, object customdata, IMyDecalHandler decalHandler, MyStringHash physicalMaterial, MyStringHash voxelMaterial, bool isTrail)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			MyDecalRenderInfo myDecalRenderInfo = default(MyDecalRenderInfo);
			myDecalRenderInfo.Flags = MyDecalFlags.World | flags;
			myDecalRenderInfo.AliveUntil = aliveUntil;
			myDecalRenderInfo.Position = hitInfo.Position;
			myDecalRenderInfo.Normal = hitInfo.Normal;
			myDecalRenderInfo.Source = source;
			myDecalRenderInfo.RenderObjectIds = RootVoxel.Render.RenderObjectIDs;
<<<<<<< HEAD
			myDecalRenderInfo.PhysicalMaterial = ((physicalMaterial == MyStringHash.NullOrEmpty) ? base.Physics.GetMaterialAt(hitInfo.Position) : physicalMaterial);
=======
			myDecalRenderInfo.PhysicalMaterial = physicalMaterial;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myDecalRenderInfo.VoxelMaterial = voxelMaterial;
			myDecalRenderInfo.Forward = forwardDirection;
			myDecalRenderInfo.IsTrail = isTrail;
			MyDecalRenderInfo renderInfo = myDecalRenderInfo;
			decalHandler.AddDecal(ref renderInfo, decals);
		}

		public void RequestVoxelCutoutSphere(Vector3D center, float radius, bool createDebris, bool damage)
		{
			MyMultiplayer.RaiseEvent(RootVoxel, (MyVoxelBase x) => x.VoxelCutoutSphere_Implementation, center, radius, createDebris, damage);
		}

<<<<<<< HEAD
		[Event(null, 805)]
=======
		[Event(null, 755)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		[RefreshReplicable]
		private void VoxelCutoutSphere_Implementation(Vector3D center, float radius, bool createDebris, bool damage = false)
		{
			BeforeContentChanged = true;
			MyExplosion.CutOutVoxelMap(radius, center, this, createDebris && MySession.Static.Ready, damage);
		}

		public void RequestVoxelOperationCapsule(Vector3D A, Vector3D B, float radius, MatrixD Transformation, byte material, OperationType Type)
		{
			MyCapsuleShapeParams arg = default(MyCapsuleShapeParams);
			arg.A = A;
			arg.B = B;
			arg.Radius = radius;
			arg.Transformation = Transformation;
			arg.Material = material;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => VoxelOperationCapsule_Implementation, base.EntityId, arg, Type);
		}

<<<<<<< HEAD
		[Event(null, 824)]
=======
		[Event(null, 774)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[RefreshReplicable]
		private static void VoxelOperationCapsule_Implementation(long entityId, MyCapsuleShapeParams capsuleParams, OperationType Type)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			m_capsuleShape.Transformation = capsuleParams.Transformation;
			m_capsuleShape.A = capsuleParams.A;
			m_capsuleShape.B = capsuleParams.B;
			m_capsuleShape.Radius = capsuleParams.Radius;
			if (!CanPlaceInArea(Type, m_capsuleShape))
			{
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && !myVoxelBase.m_voxelShapeInProgress)
			{
				myVoxelBase.BeforeContentChanged = true;
				MyMultiplayer.RaiseEvent(myVoxelBase.RootVoxel, (MyVoxelBase x) => x.PerformVoxelOperationCapsule_Implementation, capsuleParams, Type);
				myVoxelBase.UpdateVoxelShape(Type, m_capsuleShape, capsuleParams.Material);
			}
		}

<<<<<<< HEAD
		[Event(null, 855)]
=======
		[Event(null, 805)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		[RefreshReplicable]
		private void PerformVoxelOperationCapsule_Implementation(MyCapsuleShapeParams capsuleParams, OperationType Type)
		{
			BeforeContentChanged = true;
			m_capsuleShape.Transformation = capsuleParams.Transformation;
			m_capsuleShape.A = capsuleParams.A;
			m_capsuleShape.B = capsuleParams.B;
			m_capsuleShape.Radius = capsuleParams.Radius;
			UpdateVoxelShape(Type, m_capsuleShape, capsuleParams.Material);
		}

		public void RequestVoxelOperationSphere(Vector3D center, float radius, byte material, OperationType Type)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => VoxelOperationSphere_Implementation, base.EntityId, center, radius, material, Type);
		}

<<<<<<< HEAD
		[Event(null, 873)]
=======
		[Event(null, 823)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void VoxelOperationSphere_Implementation(long entityId, Vector3D center, float radius, byte material, OperationType Type)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			m_sphereShape.Center = center;
			m_sphereShape.Radius = radius;
			if (!CanPlaceInArea(Type, m_sphereShape))
			{
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && !myVoxelBase.m_voxelShapeInProgress)
			{
				myVoxelBase.BeforeContentChanged = true;
				MyMultiplayer.RaiseEvent(myVoxelBase.RootVoxel, (MyVoxelBase x) => x.PerformVoxelOperationSphere_Implementation, center, radius, material, Type);
				myVoxelBase.UpdateVoxelShape(Type, m_sphereShape, material);
			}
		}

<<<<<<< HEAD
		[Event(null, 902)]
=======
		[Event(null, 852)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		[RefreshReplicable]
		private void PerformVoxelOperationSphere_Implementation(Vector3D center, float radius, byte material, OperationType Type)
		{
			m_sphereShape.Center = center;
			m_sphereShape.Radius = radius;
			BeforeContentChanged = true;
			UpdateVoxelShape(Type, m_sphereShape, material);
		}

		public void RequestVoxelOperationBox(BoundingBoxD box, MatrixD Transformation, byte material, OperationType Type)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => VoxelOperationBox_Implementation, base.EntityId, box, Transformation, material, Type);
		}

<<<<<<< HEAD
		[Event(null, 919)]
=======
		[Event(null, 869)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[RefreshReplicable]
		private static void VoxelOperationBox_Implementation(long entityId, BoundingBoxD box, MatrixD Transformation, byte material, OperationType Type)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			m_boxShape.Transformation = Transformation;
			m_boxShape.Boundaries.Max = box.Max;
			m_boxShape.Boundaries.Min = box.Min;
			if (!CanPlaceInArea(Type, m_boxShape))
			{
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && !myVoxelBase.m_voxelShapeInProgress)
			{
				myVoxelBase.BeforeContentChanged = true;
				MyMultiplayer.RaiseEvent(myVoxelBase.RootVoxel, (MyVoxelBase x) => x.PerformVoxelOperationBox_Implementation, box, Transformation, material, Type);
				myVoxelBase.UpdateVoxelShape(Type, m_boxShape, material);
			}
		}

<<<<<<< HEAD
		[Event(null, 949)]
=======
		[Event(null, 899)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void PerformVoxelOperationBox_Implementation(BoundingBoxD box, MatrixD Transformation, byte material, OperationType Type)
		{
			BeforeContentChanged = true;
			m_boxShape.Transformation = Transformation;
			m_boxShape.Boundaries.Max = box.Max;
			m_boxShape.Boundaries.Min = box.Min;
			UpdateVoxelShape(Type, m_boxShape, material);
		}

		public void RequestVoxelOperationRamp(BoundingBoxD box, Vector3D rampNormal, double rampNormalW, MatrixD Transformation, byte material, OperationType Type)
		{
			MyRampShapeParams arg = default(MyRampShapeParams);
			arg.Box = box;
			arg.RampNormal = rampNormal;
			arg.RampNormalW = rampNormalW;
			arg.Transformation = Transformation;
			arg.Material = material;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => VoxelOperationRamp_Implementation, base.EntityId, arg, Type);
		}

<<<<<<< HEAD
		[Event(null, 973)]
=======
		[Event(null, 923)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[RefreshReplicable]
		private static void VoxelOperationRamp_Implementation(long entityId, MyRampShapeParams shapeParams, OperationType Type)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			m_rampShape.Transformation = shapeParams.Transformation;
			m_rampShape.Boundaries.Max = shapeParams.Box.Max;
			m_rampShape.Boundaries.Min = shapeParams.Box.Min;
			m_rampShape.RampNormal = shapeParams.RampNormal;
			m_rampShape.RampNormalW = shapeParams.RampNormalW;
			if (!CanPlaceInArea(Type, m_rampShape))
			{
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && !myVoxelBase.m_voxelShapeInProgress)
			{
				myVoxelBase.BeforeContentChanged = true;
				MyMultiplayer.RaiseEvent(myVoxelBase.RootVoxel, (MyVoxelBase x) => x.PerformVoxelOperationRamp_Implementation, shapeParams, Type);
				myVoxelBase.UpdateVoxelShape(Type, m_rampShape, shapeParams.Material);
			}
		}

<<<<<<< HEAD
		[Event(null, 1005)]
=======
		[Event(null, 955)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void PerformVoxelOperationRamp_Implementation(MyRampShapeParams shapeParams, OperationType Type)
		{
			BeforeContentChanged = true;
			m_rampShape.Transformation = shapeParams.Transformation;
			m_rampShape.Boundaries.Max = shapeParams.Box.Max;
			m_rampShape.Boundaries.Min = shapeParams.Box.Min;
			m_rampShape.RampNormal = shapeParams.RampNormal;
			m_rampShape.RampNormalW = shapeParams.RampNormalW;
			UpdateVoxelShape(Type, m_rampShape, shapeParams.Material);
		}

		public void RequestVoxelOperationElipsoid(Vector3 radius, MatrixD Transformation, byte material, OperationType Type)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => VoxelOperationElipsoid_Implementation, base.EntityId, radius, Transformation, material, Type);
		}

<<<<<<< HEAD
		[Event(null, 1024)]
=======
		[Event(null, 974)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[RefreshReplicable]
		private static void VoxelOperationElipsoid_Implementation(long entityId, Vector3 radius, MatrixD Transformation, byte material, OperationType Type)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.GetVoxelHandAvailable(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			m_ellipsoidShape.Transformation = Transformation;
			m_ellipsoidShape.Radius = radius;
			if (!CanPlaceInArea(Type, m_ellipsoidShape))
			{
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && !myVoxelBase.m_voxelShapeInProgress)
			{
				myVoxelBase.BeforeContentChanged = true;
				MyMultiplayer.RaiseEvent(myVoxelBase.RootVoxel, (MyVoxelBase x) => x.PerformVoxelOperationElipsoid_Implementation, radius, Transformation, material, Type);
				myVoxelBase.UpdateVoxelShape(Type, m_ellipsoidShape, material);
			}
		}

<<<<<<< HEAD
		[Event(null, 1053)]
=======
		[Event(null, 1003)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void PerformVoxelOperationElipsoid_Implementation(Vector3 radius, MatrixD Transformation, byte material, OperationType Type)
		{
			BeforeContentChanged = true;
			m_ellipsoidShape.Transformation = Transformation;
			m_ellipsoidShape.Radius = radius;
			UpdateVoxelShape(Type, m_ellipsoidShape, material);
		}

<<<<<<< HEAD
		[Event(null, 1063)]
=======
		[Event(null, 1013)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[BroadcastExcept]
		public void RevertVoxelAccess(Vector3I key, MyStorageDataTypeFlags flags)
		{
			if (Storage != null)
			{
				(Storage as MyStorageBase)?.AccessDelete(ref key, flags);
			}
		}

<<<<<<< HEAD
		[Event(null, 1082)]
=======
		[Event(null, 1027)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public void PerformCutOutSphereFast(Vector3D center, float radius, bool notify)
		{
			MyVoxelGenerator.CutOutSphereFast(this, ref center, radius, out var _, out var _, notify);
		}

		public void BroadcastShipCutout(in MyShipMiningSystem.NetworkCutoutData data)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyVoxelBase x) => x.PerformShipCutout, data);
			}
		}

<<<<<<< HEAD
		[Event(null, 1100)]
=======
		[Event(null, 1045)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void PerformShipCutout(MyShipMiningSystem.NetworkCutoutData data)
		{
			MyShipMiningSystem.PerformCutoutClient(this, in data);
		}

		public void CutOutShapeWithProperties(MyShape shape, out float voxelsCountInPercent, out MyVoxelMaterialDefinition voxelMaterial, Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials = null, bool updateSync = false, bool onlyCheck = false, bool applyDamageMaterial = false, bool onlyApplyMaterial = false)
		{
			BeforeContentChanged = true;
			MyVoxelGenerator.CutOutShapeWithProperties(this, shape, out voxelsCountInPercent, out voxelMaterial, exactCutOutMaterials, updateSync, onlyCheck, applyDamageMaterial, onlyApplyMaterial);
		}

		public void CutOutShapeWithPropertiesAsync(OnCutOutResults results, MyShape shape, bool updateSync = false, bool onlyCheck = false, bool applyDamageMaterial = false, bool onlyApplyMaterial = false, bool skipCache = true)
		{
			BeforeContentChanged = true;
			float voxelsCountInPercent = 0f;
			MyVoxelMaterialDefinition voxelMaterial = null;
			Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials = new Dictionary<MyVoxelMaterialDefinition, int>();
			Parallel.Start(delegate
			{
				using (Pin())
				{
					if (!base.MarkedForClose)
					{
						MyVoxelGenerator.CutOutShapeWithProperties(this, shape, out voxelsCountInPercent, out voxelMaterial, exactCutOutMaterials, updateSync, onlyCheck, applyDamageMaterial, onlyApplyMaterial, skipCache);
					}
				}
			}, delegate
			{
				if (results != null)
				{
					results(voxelsCountInPercent, voxelMaterial, exactCutOutMaterials);
				}
			});
		}

		private static bool CanPlaceInArea(OperationType type, MyShape Shape)
		{
			if (type == OperationType.Fill || type == OperationType.Revert)
			{
				m_foundElements.Clear();
				BoundingBoxD boundingBox = Shape.GetWorldBoundaries();
				MyEntities.GetElementsInBox(ref boundingBox, m_foundElements);
				foreach (MyEntity foundElement in m_foundElements)
				{
					if (IsForbiddenEntity(foundElement) && foundElement.PositionComp.WorldAABB.Intersects(boundingBox))
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool IsForbiddenEntity(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (!(entity is MyCharacter) && (myCubeGrid == null || myCubeGrid.IsStatic || myCubeGrid.IsPreview))
			{
				if (entity is MyCockpit)
				{
					return (entity as MyCockpit).Pilot != null;
				}
				return false;
			}
			return true;
		}

		private void UpdateVoxelShape(OperationType type, MyShape shape, byte Material)
		{
			MyShape localShape = shape.Clone();
			m_voxelShapeInProgress = true;
			switch (type)
			{
			case OperationType.Paint:
				if (MyFakes.VOXELHAND_PARALLEL)
				{
					Parallel.Start(delegate
					{
						MyVoxelGenerator.PaintInShape(this, localShape, Material);
					}, delegate
					{
						m_voxelShapeInProgress = false;
					});
				}
				else
				{
					MyVoxelGenerator.PaintInShape(this, localShape, Material);
				}
				break;
			case OperationType.Revert:
				if (MyFakes.VOXELHAND_PARALLEL)
				{
					Parallel.Start(delegate
					{
						MyVoxelGenerator.RevertShape(this, localShape);
					}, delegate
					{
						m_voxelShapeInProgress = false;
					});
				}
				else
				{
					MyVoxelGenerator.RevertShape(this, localShape);
				}
				break;
			case OperationType.Fill:
				if (MyFakes.VOXELHAND_PARALLEL)
				{
					Parallel.Start(delegate
					{
						MyVoxelGenerator.FillInShape(this, localShape, Material);
					}, delegate
					{
						m_voxelShapeInProgress = false;
					});
				}
				else
				{
					MyVoxelGenerator.FillInShape(this, localShape, Material);
				}
				break;
			case OperationType.Cut:
				if (MyFakes.VOXELHAND_PARALLEL)
				{
					Parallel.Start(delegate
					{
						MyVoxelGenerator.CutOutShape(this, localShape, voxelHand: true);
					}, delegate
					{
						m_voxelShapeInProgress = false;
					});
				}
				else
				{
					MyVoxelGenerator.CutOutShape(this, localShape, voxelHand: true);
				}
				break;
			default:
				m_voxelShapeInProgress = false;
				break;
			}
		}

		public void CreateVoxelMeteorCrater(Vector3D center, float radius, Vector3 direction, MyVoxelMaterialDefinition material)
		{
			BeforeContentChanged = true;
			MyMultiplayer.RaiseEvent(RootVoxel, (MyVoxelBase x) => x.CreateVoxelMeteorCrater_Implementation, center, radius, direction, material.Index);
		}

<<<<<<< HEAD
		[Event(null, 1265)]
=======
		[Event(null, 1189)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void CreateVoxelMeteorCrater_Implementation(Vector3D center, float radius, Vector3 direction, byte material)
		{
			BeforeContentChanged = true;
			MyVoxelGenerator.MakeCrater(this, new BoundingSphereD(center, radius), direction, MyDefinitionManager.Static.GetVoxelMaterialDefinition(material));
		}

		public void GetFilledStorageBounds(out Vector3I min, out Vector3I max)
		{
			min = Vector3I.MaxValue;
			max = Vector3I.MinValue;
			Vector3I size = Size;
			Vector3I vector3I = Size - 1;
			if (m_tempStorage == null)
			{
				m_tempStorage = new MyStorageData();
			}
			m_tempStorage.Resize(Size);
			Storage.ReadRange(m_tempStorage, MyStorageDataTypeFlags.Content, 0, Vector3I.Zero, vector3I);
			for (int i = 0; i < size.Z; i++)
			{
				for (int j = 0; j < size.Y; j++)
				{
					for (int k = 0; k < size.X; k++)
					{
						if (m_tempStorage.Content(k, j, i) > 127)
						{
							Vector3I value = Vector3I.Max(new Vector3I(k - 1, j - 1, i - 1), Vector3I.Zero);
							min = Vector3I.Min(value, min);
							Vector3I value2 = Vector3I.Min(new Vector3I(k + 1, j + 1, i + 1), vector3I);
							max = Vector3I.Max(value2, max);
						}
					}
				}
			}
		}

		public MyVoxelContentConstitution GetVoxelRangeTypeInBoundingBox(BoundingBoxD worldAabb)
		{
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldAabb.Min, out var voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldAabb.Max, out var voxelCoord2);
			voxelCoord += StorageMin;
			voxelCoord2 += StorageMin;
			Storage.ClampVoxelCoord(ref voxelCoord);
			Storage.ClampVoxelCoord(ref voxelCoord2);
			return MyVoxelContentConstitution.Mixed;
		}

		public HashSet<byte> GetMaterialsInShape(MyShape shape, int lod = 0)
		{
			BoundingBoxD worldBoundaries = shape.GetWorldBoundaries();
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldBoundaries.Min, out var voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldBoundaries.Max, out var voxelCoord2);
			Vector3I voxelCoord3 = voxelCoord - 1;
			Vector3I voxelCoord4 = voxelCoord2 + 1;
			Storage.ClampVoxelCoord(ref voxelCoord3);
			Storage.ClampVoxelCoord(ref voxelCoord4);
			voxelCoord3 >>= lod;
			voxelCoord3 -= 1;
			voxelCoord4 >>= lod;
			voxelCoord4 += 1;
			if (m_tempStorage == null)
			{
				m_tempStorage = new MyStorageData();
			}
			m_tempStorage.Resize(voxelCoord3, voxelCoord4);
			using (Storage.Pin())
			{
				Storage.ReadRange(m_tempStorage, MyStorageDataTypeFlags.Material, lod, voxelCoord3, voxelCoord4);
			}
<<<<<<< HEAD
			HashSet<byte> hashSet = new HashSet<byte>();
=======
			HashSet<byte> val = new HashSet<byte>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector3I vector3I = default(Vector3I);
			vector3I.X = voxelCoord3.X;
			while (vector3I.X <= voxelCoord4.X)
			{
				vector3I.Y = voxelCoord3.Y;
				while (vector3I.Y <= voxelCoord4.Y)
				{
					vector3I.Z = voxelCoord3.Z;
					while (vector3I.Z <= voxelCoord4.Z)
					{
						Vector3I p = vector3I - voxelCoord3;
						int linearIdx = m_tempStorage.ComputeLinear(ref p);
						byte b = m_tempStorage.Material(linearIdx);
						if (b != byte.MaxValue)
						{
							val.Add(b);
						}
						vector3I.Z++;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
			return val;
		}

		/// Intersect the storage
		///
		/// @param box WorldSpace bounding box to intersect with the storage.
		public ContainmentType IntersectStorage(ref BoundingBox box, bool lazy = true)
		{
			box.Transform(base.PositionComp.WorldMatrixInvScaled);
			box.Translate(SizeInMetresHalf + StorageMin);
			return Storage.Intersect(ref box, lazy);
		}

		/// <summary>
		/// Find a storage local position that is outside of voxel content within a given radius.
		/// </summary>
		/// <param name="localPosition"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public virtual Vector3D FindOutsidePosition(Vector3D localPosition, float radius)
		{
			Vector3D normal = MyGravityProviderSystem.CalculateTotalGravityInPoint(Vector3D.Transform(localPosition - SizeInMetresHalf, base.WorldMatrix));
			Vector3D vector3D;
			if (normal.LengthSquared() > 0.01)
			{
				normal = Vector3D.TransformNormal(normal, base.PositionComp.WorldMatrixNormalizedInv);
				normal.Normalize();
				vector3D = MyUtils.GetRandomPerpendicularVector(ref normal);
			}
			else
			{
				vector3D = localPosition - SizeInMetresHalf;
				vector3D.Normalize();
			}
			double num = radius;
			Vector3D result;
			while (OverlapsSphereLocal(radius, result = localPosition + vector3D * num))
			{
				num *= 2.0;
			}
			return result;
		}

		public override void OnReplicationStarted()
		{
			base.OnReplicationStarted();
			MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted(this);
		}

		public override void OnReplicationEnded()
		{
			base.OnReplicationEnded();
			MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded(this);
		}
<<<<<<< HEAD

		public MyVoxelMaterialDefinition GetMaterialAt(ref Vector3D worldPosition)
		{
			if (Storage == null)
			{
				return null;
			}
			MyVoxelCoordSystems.WorldPositionToLocalPosition(worldPosition, base.PositionComp.WorldMatrixRef, base.PositionComp.WorldMatrixInvScaled, SizeInMetresHalf, out var localPosition);
			Vector3I voxelCoords = new Vector3I(localPosition / 1f) + StorageMin;
			return Storage.GetMaterialAt(ref voxelCoords);
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
