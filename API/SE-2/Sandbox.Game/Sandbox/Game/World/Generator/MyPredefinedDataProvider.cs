using System;
using System.IO;
using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using VRage.FileSystem;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	[MyStorageDataProvider(10102)]
	internal sealed class MyPredefinedDataProvider : MyCompositeShapeProviderBase, IMyStorageDataProvider
	{
		private class SingleShapeProvider : IMyCompositionInfoProvider
		{
			public IMyCompositeShape[] FilledShapes { get; }

			public IMyCompositeDeposit[] Deposits { get; }

			public IMyCompositeShape[] RemovedShapes => Array.Empty<IMyCompositeShape>();

			public MyVoxelMaterialDefinition DefaultMaterial => MyDefinitionManager.Static.GetVoxelMaterialDefinition("Stone");

			public SingleShapeProvider(IMyCompositeShape shape, IMyCompositeDeposit deposit)
			{
				FilledShapes = new IMyCompositeShape[1] { shape };
				Deposits = new IMyCompositeDeposit[1] { deposit };
			}

			public void Close()
			{
			}
		}

		private class SingleDeposit : IMyCompositeDeposit, IMyCompositeShape
		{
			private MyStorageBase m_storageBase;

			public SingleDeposit(MyStorageBase storage)
			{
				m_storageBase = storage;
			}

			public ContainmentType Contains(ref BoundingBox queryBox, ref BoundingSphere querySphere, int lodVoxelSize)
			{
				return ContainmentType.Contains;
			}

			public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
			{
				return -1f;
			}

			public void ComputeContent(MyStorageData storage, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax, int lodVoxelSize)
			{
				m_storageBase.ReadRange(storage, MyStorageDataTypeFlags.Material, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
			}

			public void DebugDraw(ref MatrixD worldMatrix, Color color)
			{
			}

			public void Close()
			{
			}

			public MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 localPos, float lodVoxelSize)
			{
				Vector3D localCoords = localPos;
				return m_storageBase.GetMaterialAt(ref localCoords);
			}
		}

		private const int CURRENT_VERSION = 0;

		private string m_storageName;

		private string m_voxelMaterial = "";

		private MyStorageBase m_storageBase;

		public override int SerializedSize => 4 + m_storageName.Get7bitEncodedSize() + m_voxelMaterial.Get7bitEncodedSize() + 4;

		public MyStorageBase Storage => m_storageBase;

		private void Init(string storageName, string voxelMaterial)
		{
			m_storageName = storageName;
			m_voxelMaterial = (string.IsNullOrEmpty(voxelMaterial) ? "" : voxelMaterial);
			if (MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(storageName, out var definition))
			{
				string name = Path.Combine(definition.Context.IsBaseGame ? MyFileSystem.ContentPath : definition.Context.ModPath, definition.StorageFile);
				m_storageBase = MyStorageBase.Load(name, cache: true, local: true);
			}
			m_infoProvider = new SingleShapeProvider((IMyCompositeShape)m_storageBase, new SingleDeposit(m_storageBase));
		}

		public override void WriteTo(Stream stream)
		{
			stream.WriteNoAlloc(0);
			stream.WriteNoAlloc(m_storageName);
			stream.WriteNoAlloc(m_voxelMaterial);
		}

		public override void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat)
		{
			stream.ReadInt32();
			string storageName = stream.ReadString();
			string voxelMaterial = stream.ReadString();
			Init(storageName, voxelMaterial);
		}

		public static MyPredefinedDataProvider CreatePredefinedShape(string storageName, string voxelMaterial)
		{
			MyPredefinedDataProvider myPredefinedDataProvider = new MyPredefinedDataProvider();
			myPredefinedDataProvider.Init(storageName, voxelMaterial);
			return myPredefinedDataProvider;
		}

		internal override MyVoxelRequestFlags ReadMaterialRange(MyStorageData target, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, bool detectOnly, bool considerContent)
		{
			if (string.IsNullOrEmpty(m_voxelMaterial))
			{
				return base.ReadMaterialRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, detectOnly, considerContent);
			}
			MyVoxelMaterialDefinition definition = null;
			if (!string.IsNullOrEmpty(m_voxelMaterial))
			{
				MyDefinitionManager.Static.TryGetVoxelMaterialDefinition(m_voxelMaterial, out definition);
				target.BlockFillMaterialConsiderContent(writeOffset, writeOffset + (maxInLod - minInLod), definition.Index);
			}
			return (MyVoxelRequestFlags)0;
		}
	}
}
