using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sandbox.Engine.Utils;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	internal abstract class MyCompositeShapeProviderBase : IMyStorageDataProvider
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct MaxOp : MyStorageData.IOperator
		{
			public void Op(ref byte a, byte b)
			{
				a = Math.Max(a, b);
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct DiffOp : MyStorageData.IOperator
		{
			public void Op(ref byte a, byte b)
			{
				a = (byte)Math.Min(a, 255 - b);
			}
		}

		protected IMyCompositionInfoProvider m_infoProvider;

		[ThreadStatic]
		private static List<IMyCompositeDeposit> m_overlappedDeposits;

		[ThreadStatic]
		private static List<IMyCompositeShape> m_overlappedFilledShapes;

		[ThreadStatic]
		private static List<IMyCompositeShape> m_overlappedRemovedShapes;

		[ThreadStatic]
		private static MyStorageData m_storageCache;

		public abstract int SerializedSize { get; }

		public abstract void WriteTo(Stream stream);

		public abstract void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat);

		bool IMyStorageDataProvider.Intersect(ref LineD line, out double startOffset, out double endOffset)
		{
			startOffset = 0.0;
			endOffset = 0.0;
			return true;
		}

		public ContainmentType Intersect(BoundingBoxI box, int lod)
		{
			return Intersect(m_infoProvider, box, lod);
		}

		public static ContainmentType Intersect(IMyCompositionInfoProvider infoProvider, BoundingBoxI box, int lod)
		{
			ContainmentType containmentType = ContainmentType.Disjoint;
			BoundingBox queryBox = new BoundingBox(box);
			BoundingSphere querySphere = new BoundingSphere(queryBox.Center, queryBox.Extents.Length() / 2f);
			IMyCompositeShape[] filledShapes = infoProvider.FilledShapes;
			for (int i = 0; i < filledShapes.Length; i++)
			{
				ContainmentType containmentType2 = filledShapes[i].Contains(ref queryBox, ref querySphere, 1);
				switch (containmentType2)
				{
				case ContainmentType.Contains:
					break;
				case ContainmentType.Intersects:
					containmentType = ContainmentType.Intersects;
					continue;
				default:
					continue;
				}
				containmentType = containmentType2;
				break;
			}
			if (containmentType != 0)
			{
				filledShapes = infoProvider.RemovedShapes;
				for (int i = 0; i < filledShapes.Length; i++)
				{
					switch (filledShapes[i].Contains(ref queryBox, ref querySphere, 1))
					{
					case ContainmentType.Contains:
						break;
					case ContainmentType.Intersects:
						containmentType = ContainmentType.Intersects;
						continue;
					default:
						continue;
					}
					containmentType = ContainmentType.Disjoint;
					break;
				}
			}
			return containmentType;
		}

<<<<<<< HEAD
		void IMyStorageDataProvider.ReadRange(ref MyVoxelDataRequest req, bool detectOnly)
=======
		void IMyStorageDataProvider.ReadRange(ref MyVoxelDataRequest req, bool detectOnly = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (req.RequestedData.Requests(MyStorageDataTypeEnum.Content))
			{
				req.Flags = ReadContentRange(req.Target, ref req.Offset, req.Lod, ref req.MinInLod, ref req.MaxInLod, detectOnly);
			}
			else
			{
				req.Flags = ReadMaterialRange(req.Target, ref req.Offset, req.Lod, ref req.MinInLod, ref req.MaxInLod, detectOnly, (req.RequestFlags & MyVoxelRequestFlags.ConsiderContent) > (MyVoxelRequestFlags)0);
			}
			req.Flags |= req.RequestFlags & MyVoxelRequestFlags.RequestFlags;
		}

		void IMyStorageDataProvider.ReadRange(MyStorageData target, MyStorageDataTypeFlags dataType, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod)
		{
			if (dataType.Requests(MyStorageDataTypeEnum.Content))
			{
				ReadContentRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, detectOnly: false);
			}
			else
			{
				ReadMaterialRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, detectOnly: false, considerContent: false);
			}
		}

		public virtual void DebugDraw(ref MatrixD worldMatrix)
		{
			Color green = Color.Green;
			Color red = Color.Red;
			Color cornflowerBlue = Color.CornflowerBlue;
			IMyCompositeShape[] filledShapes = m_infoProvider.FilledShapes;
			for (int i = 0; i < filledShapes.Length; i++)
			{
				filledShapes[i].DebugDraw(ref worldMatrix, green);
			}
			filledShapes = m_infoProvider.RemovedShapes;
			for (int i = 0; i < filledShapes.Length; i++)
			{
				filledShapes[i].DebugDraw(ref worldMatrix, red);
			}
			IMyCompositeDeposit[] deposits = m_infoProvider.Deposits;
			for (int i = 0; i < deposits.Length; i++)
			{
				deposits[i].DebugDraw(ref worldMatrix, cornflowerBlue);
			}
		}

		void IMyStorageDataProvider.ReindexMaterials(Dictionary<byte, byte> oldToNewIndexMap)
		{
		}

		void IMyStorageDataProvider.PostProcess(VrVoxelMesh mesh, MyStorageDataTypeFlags dataTypes)
		{
		}

		void IMyStorageDataProvider.Close()
		{
<<<<<<< HEAD
			foreach (IMyCompositeShape item in m_infoProvider.Deposits.Concat(m_infoProvider.FilledShapes).Concat(m_infoProvider.RemovedShapes))
=======
			foreach (IMyCompositeShape item in Enumerable.Concat<IMyCompositeShape>(Enumerable.Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>)m_infoProvider.Deposits, (IEnumerable<IMyCompositeShape>)m_infoProvider.FilledShapes), (IEnumerable<IMyCompositeShape>)m_infoProvider.RemovedShapes))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				item.Close();
			}
			m_infoProvider.Close();
			m_infoProvider = null;
		}

		private static MyStorageData GetTempStorage(ref Vector3I min, ref Vector3I max)
		{
			MyStorageData myStorageData = m_storageCache;
			if (myStorageData == null)
			{
				myStorageData = (m_storageCache = new MyStorageData(MyStorageDataTypeFlags.Content));
			}
			myStorageData.Resize(min, max);
			return myStorageData;
		}

		internal MyVoxelRequestFlags ReadContentRange(MyStorageData target, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, bool detectOnly)
		{
			SetupReading(lodIndex, ref minInLod, ref maxInLod, out var lodVoxelSize, out var queryBox, out var querySphere);
			using (MyUtils.ReuseCollection(ref m_overlappedFilledShapes))
			{
				using (MyUtils.ReuseCollection(ref m_overlappedRemovedShapes))
				{
					List<IMyCompositeShape> overlappedFilledShapes = m_overlappedFilledShapes;
					List<IMyCompositeShape> overlappedRemovedShapes = m_overlappedRemovedShapes;
					ContainmentType containmentType = ContainmentType.Disjoint;
					IMyCompositeShape[] removedShapes = m_infoProvider.RemovedShapes;
					foreach (IMyCompositeShape myCompositeShape in removedShapes)
					{
						switch (myCompositeShape.Contains(ref queryBox, ref querySphere, lodVoxelSize))
						{
						case ContainmentType.Contains:
							break;
						case ContainmentType.Intersects:
							containmentType = ContainmentType.Intersects;
							overlappedRemovedShapes.Add(myCompositeShape);
							continue;
						default:
							continue;
						}
						containmentType = ContainmentType.Contains;
						break;
					}
					if (containmentType == ContainmentType.Contains)
					{
						if (!detectOnly)
						{
							target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), 0);
						}
						return MyVoxelRequestFlags.EmptyData;
					}
					ContainmentType containmentType2 = ContainmentType.Disjoint;
					removedShapes = m_infoProvider.FilledShapes;
					foreach (IMyCompositeShape myCompositeShape2 in removedShapes)
					{
						switch (myCompositeShape2.Contains(ref queryBox, ref querySphere, lodVoxelSize))
						{
						case ContainmentType.Contains:
							break;
						case ContainmentType.Intersects:
							overlappedFilledShapes.Add(myCompositeShape2);
							containmentType2 = ContainmentType.Intersects;
							continue;
						default:
							continue;
						}
						overlappedFilledShapes.Clear();
						containmentType2 = ContainmentType.Contains;
						break;
					}
					if (containmentType2 == ContainmentType.Disjoint)
					{
						if (!detectOnly)
						{
							target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), 0);
						}
						return MyVoxelRequestFlags.EmptyData;
					}
					if (containmentType == ContainmentType.Disjoint && containmentType2 == ContainmentType.Contains)
					{
						if (!detectOnly)
						{
							target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), byte.MaxValue);
						}
						return MyVoxelRequestFlags.FullContent;
					}
					if (detectOnly)
					{
						return (MyVoxelRequestFlags)0;
					}
					MyStorageData tempStorage = GetTempStorage(ref minInLod, ref maxInLod);
					bool flag = containmentType2 == ContainmentType.Contains;
					target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), (byte)(flag ? byte.MaxValue : 0));
					if (!flag)
					{
						foreach (IMyCompositeShape item in overlappedFilledShapes)
						{
							item.ComputeContent(tempStorage, lodIndex, minInLod, maxInLod, lodVoxelSize);
							target.OpRange<MaxOp>(tempStorage, Vector3I.Zero, maxInLod - minInLod, writeOffset, MyStorageDataTypeEnum.Content);
						}
					}
					if (containmentType != 0)
					{
						foreach (IMyCompositeShape item2 in overlappedRemovedShapes)
						{
							item2.ComputeContent(tempStorage, lodIndex, minInLod, maxInLod, lodVoxelSize);
							target.OpRange<DiffOp>(tempStorage, Vector3I.Zero, maxInLod - minInLod, writeOffset, MyStorageDataTypeEnum.Content);
						}
					}
				}
			}
			return (MyVoxelRequestFlags)0;
		}

		internal virtual MyVoxelRequestFlags ReadMaterialRange(MyStorageData target, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, bool detectOnly, bool considerContent)
		{
			SetupReading(lodIndex, ref minInLod, ref maxInLod, out var lodVoxelSize, out var queryBox, out var querySphere);
			using (MyUtils.ReuseCollection(ref m_overlappedDeposits))
			{
				List<IMyCompositeDeposit> overlappedDeposits = m_overlappedDeposits;
				MyVoxelMaterialDefinition defaultMaterial = m_infoProvider.DefaultMaterial;
				ContainmentType containmentType = ContainmentType.Disjoint;
				IMyCompositeDeposit[] deposits = m_infoProvider.Deposits;
				foreach (IMyCompositeDeposit myCompositeDeposit in deposits)
				{
					if (myCompositeDeposit.Contains(ref queryBox, ref querySphere, lodVoxelSize) != 0)
					{
						overlappedDeposits.Add(myCompositeDeposit);
						containmentType = ContainmentType.Intersects;
					}
				}
				if (containmentType == ContainmentType.Disjoint)
				{
					if (!detectOnly)
					{
						if (considerContent)
						{
							target.BlockFillMaterialConsiderContent(writeOffset, writeOffset + (maxInLod - minInLod), defaultMaterial.Index);
						}
						else
						{
							target.BlockFillMaterial(writeOffset, writeOffset + (maxInLod - minInLod), defaultMaterial.Index);
						}
					}
					return MyVoxelRequestFlags.EmptyData;
				}
				if (detectOnly)
				{
					return (MyVoxelRequestFlags)0;
				}
				Vector3I vector3I = default(Vector3I);
				vector3I.Z = minInLod.Z;
				while (vector3I.Z <= maxInLod.Z)
				{
					vector3I.Y = minInLod.Y;
					while (vector3I.Y <= maxInLod.Y)
					{
						vector3I.X = minInLod.X;
						while (vector3I.X <= maxInLod.X)
						{
							Vector3I p = vector3I - minInLod + writeOffset;
							if (considerContent && target.Content(ref p) == 0)
							{
								target.Material(ref p, byte.MaxValue);
							}
							else
							{
								Vector3 localPos = vector3I * lodVoxelSize;
								float num = 1f;
								byte materialIdx = defaultMaterial.Index;
								if (!MyFakes.DISABLE_COMPOSITE_MATERIAL)
								{
									foreach (IMyCompositeDeposit item in overlappedDeposits)
									{
										float num2 = item.SignedDistance(ref localPos, 1);
										if (num2 < 0f && num2 <= num)
										{
											num = num2;
											materialIdx = item.GetMaterialForPosition(ref localPos, lodVoxelSize)?.Index ?? defaultMaterial.Index;
										}
									}
								}
								target.Material(ref p, materialIdx);
							}
							vector3I.X++;
						}
						vector3I.Y++;
					}
					vector3I.Z++;
				}
			}
			return (MyVoxelRequestFlags)0;
		}

		protected static void SetupReading(int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, out int lodVoxelSize, out BoundingBox queryBox, out BoundingSphere querySphere)
		{
			float num = 0.5f * (float)(1 << lodIndex);
			lodVoxelSize = (int)(num * 2f);
			Vector3I voxelCoord = minInLod << lodIndex;
			Vector3I voxelCoord2 = maxInLod << lodIndex;
			MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord, out var localPosition);
			Vector3 min = localPosition;
			MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord2, out localPosition);
			Vector3 max = localPosition;
			min -= num;
			max += num;
			queryBox = new BoundingBox(min, max);
			BoundingSphere.CreateFromBoundingBox(ref queryBox, out querySphere);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static float ContentToSignedDistance(byte content)
		{
			return ((float)(int)content / 255f - 0.5f) * -2f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static byte SignedDistanceToContent(float signedDistance)
		{
			signedDistance = MathHelper.Clamp(signedDistance, -1f, 1f);
			return (byte)((signedDistance / -2f + 0.5f) * 255f);
		}
	}
}
