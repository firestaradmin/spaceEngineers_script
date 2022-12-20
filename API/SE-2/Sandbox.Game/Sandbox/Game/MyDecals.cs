using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game
{
	public class MyDecals : IMyDecalHandler
	{
		private const string DEFAULT = "Default";

		[ThreadStatic]
		private static MyCubeGrid.MyCubeGridHitInfo m_gridHitInfo;

		private static readonly IMyDecalHandler m_handler = new MyDecals();

		private readonly Vector3 defaultVector;

		private MyDecals()
		{
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds decal
		/// </summary>
		/// <param name="entity">Entity that should have decal</param>
		/// <param name="hitInfo">Describes where it should be placed</param>
		/// <param name="source">Decal material</param>
		/// <param name="forwardDirection">Use for rotation of decal</param>
		/// <param name="customdata">Extra information about how decal should be positioned</param>
		/// <param name="damage">Not used</param>
		/// <param name="physicalMaterial">Physical material</param>
		/// <param name="voxelMaterial">Voxel material</param>
		/// <param name="isTrail">Is it trail, that wheels are leaving</param>
		/// <param name="flags"><see cref="T:VRageRender.MyDecalFlags" /></param>
		/// <param name="aliveUntil">Time in frames. When it is less than <see cref="P:VRage.Game.ModAPI.IMySession.GameplayFrameCounter" />, it would be removed</param>
		/// <param name="decals">If not null, generated decal ids would be added to that list</param>
		public static void HandleAddDecal(IMyEntity entity, MyHitInfo hitInfo, Vector3 forwardDirection, MyStringHash physicalMaterial = default(MyStringHash), MyStringHash source = default(MyStringHash), object customdata = null, float damage = -1f, MyStringHash voxelMaterial = default(MyStringHash), bool isTrail = false, MyDecalFlags flags = MyDecalFlags.None, int aliveUntil = int.MaxValue, List<uint> decals = null)
=======
		public static void HandleAddDecal(IMyEntity entity, MyHitInfo hitInfo, Vector3 forwardDirection, MyStringHash physicalMaterial = default(MyStringHash), MyStringHash source = default(MyStringHash), object customdata = null, float damage = -1f, MyStringHash voxelMaterial = default(MyStringHash), bool isTrail = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			IMyDecalProxy myDecalProxy = entity as IMyDecalProxy;
			if (myDecalProxy != null)
			{
<<<<<<< HEAD
				myDecalProxy.AddDecals(ref hitInfo, source, forwardDirection, customdata, m_handler, physicalMaterial, voxelMaterial, isTrail, flags, aliveUntil, decals);
=======
				myDecalProxy.AddDecals(ref hitInfo, source, forwardDirection, customdata, m_handler, physicalMaterial, voxelMaterial, isTrail);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			MySlimBlock mySlimBlock = null;
			if (myCubeBlock != null)
			{
				myCubeGrid = myCubeBlock.CubeGrid;
				mySlimBlock = myCubeBlock.SlimBlock;
			}
			else if (myCubeGrid != null)
			{
				mySlimBlock = myCubeGrid.GetTargetedBlock(hitInfo.Position - 0.001f * hitInfo.Normal);
			}
			if (myCubeGrid != null)
			{
				if (mySlimBlock != null && !mySlimBlock.BlockDefinition.PlaceDecals)
				{
					return;
				}
				MyCubeGrid.MyCubeGridHitInfo myCubeGridHitInfo = customdata as MyCubeGrid.MyCubeGridHitInfo;
				if (myCubeGridHitInfo == null)
				{
					if (mySlimBlock == null)
					{
						return;
					}
					if (m_gridHitInfo == null)
					{
						m_gridHitInfo = new MyCubeGrid.MyCubeGridHitInfo();
					}
					m_gridHitInfo.Position = mySlimBlock.Position;
					customdata = m_gridHitInfo;
				}
				else
				{
					if (!myCubeGrid.TryGetCube(myCubeGridHitInfo.Position, out var cube))
					{
						return;
					}
					mySlimBlock = cube.CubeBlock;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = ((mySlimBlock != null) ? (mySlimBlock.FatBlock as MyCompoundCubeBlock) : null);
				myDecalProxy = ((myCompoundCubeBlock != null) ? ((IMyDecalProxy)myCompoundCubeBlock) : ((IMyDecalProxy)mySlimBlock));
			}
<<<<<<< HEAD
			myDecalProxy?.AddDecals(ref hitInfo, source, forwardDirection, customdata, m_handler, physicalMaterial, voxelMaterial, isTrail, flags, aliveUntil, decals);
=======
			myDecalProxy?.AddDecals(ref hitInfo, source, forwardDirection, customdata, m_handler, physicalMaterial, voxelMaterial, isTrail);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Updates decals position and matrix by id 
		/// </summary>
		/// <param name="decals">Decals to update</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void UpdateDecals(List<MyDecalPositionUpdate> decals)
		{
			MyRenderProxy.UpdateDecals(decals);
		}

		/// <summary>
		/// Removes decals with specified Id
		/// </summary>
		/// <param name="decalId">Id, that was returned on <see cref="M:Sandbox.Game.MyDecals.HandleAddDecal(VRage.ModAPI.IMyEntity,VRage.Game.ModAPI.MyHitInfo,VRageMath.Vector3,VRage.Utils.MyStringHash,VRage.Utils.MyStringHash,System.Object,System.Single,VRage.Utils.MyStringHash,System.Boolean,VRageRender.MyDecalFlags,System.Int32,System.Collections.Generic.List{System.UInt32})" /></param>
		/// <param name="immediately">When it is false - slowly disappears</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RemoveDecal(uint decalId, bool immediately = false)
		{
			MyRenderProxy.RemoveDecal(decalId, immediately);
		}

		/// <summary>
		/// Add decal raw
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="decals">If not null, generated decal ids would be added to that list</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddDecal(ref MyDecalRenderInfo data, List<uint> decals)
		{
			m_handler.AddDecal(ref data, decals);
		}

		void IMyDecalHandler.AddDecal(ref MyDecalRenderInfo data, List<uint> ids)
		{
			if (data.RenderObjectIds == null)
			{
				return;
			}
			List<MyDecalMaterial> decalMaterials;
			bool flag = MyDecalMaterials.TryGetDecalMaterial(data.Source.String, data.PhysicalMaterial.String, out decalMaterials, data.VoxelMaterial);
			if (!flag)
			{
				if (MyFakes.ENABLE_USE_DEFAULT_DAMAGE_DECAL)
				{
					flag = MyDecalMaterials.TryGetDecalMaterial("Default", "Default", out decalMaterials, data.VoxelMaterial);
				}
				if (!flag)
				{
					return;
				}
			}
			MyDecalBindingInfo myDecalBindingInfo2;
			if (!data.Binding.HasValue)
			{
				MyDecalBindingInfo myDecalBindingInfo = default(MyDecalBindingInfo);
				myDecalBindingInfo.Position = data.Position;
				myDecalBindingInfo.Normal = data.Normal;
				myDecalBindingInfo.Transformation = Matrix.Identity;
				myDecalBindingInfo2 = myDecalBindingInfo;
			}
			else
			{
				myDecalBindingInfo2 = data.Binding.Value;
			}
			int num = (int)Math.Round(MyRandom.Instance.NextFloat() * (float)(decalMaterials.Count - 1));
			MyDecalMaterial myDecalMaterial = decalMaterials[num];
			data.RenderDistance = decalMaterials[num].RenderDistance;
			float angle = myDecalMaterial.Rotation;
			if (float.IsPositiveInfinity(myDecalMaterial.Rotation))
			{
				angle = MyRandom.Instance.NextFloat() * ((float)Math.PI * 2f);
			}
			Vector3 vector = Vector3.CalculatePerpendicularVector(myDecalBindingInfo2.Normal);
			if (data.Forward.LengthSquared() > 0f)
			{
				vector = Vector3.Normalize(data.Forward);
			}
			vector = Quaternion.CreateFromAxisAngle(myDecalBindingInfo2.Normal, angle) * vector;
			float num2 = myDecalMaterial.MinSize;
			if (myDecalMaterial.MaxSize > myDecalMaterial.MinSize)
			{
				num2 += MyRandom.Instance.NextFloat() * (myDecalMaterial.MaxSize - myDecalMaterial.MinSize);
			}
			float depth = myDecalMaterial.Depth;
			Vector3 vector2 = new Vector3(num2, num2, depth);
			MyDecalTopoData data2 = default(MyDecalTopoData);
			MatrixD matrixD;
			Vector3D worldPosition;
			if (data.Flags.HasFlag(MyDecalFlags.World))
			{
				matrixD = MatrixD.CreateWorld(Vector3D.Zero, myDecalBindingInfo2.Normal, vector);
				worldPosition = data.Position;
			}
			else
			{
				matrixD = MatrixD.CreateWorld(myDecalBindingInfo2.Position - myDecalBindingInfo2.Normal * depth * 0.45f, myDecalBindingInfo2.Normal, vector);
				worldPosition = Vector3.Invalid;
			}
			MatrixD m = MatrixD.CreateScale(vector2) * matrixD;
			data2.MatrixBinding = m;
			data2.WorldPosition = worldPosition;
			m = myDecalBindingInfo2.Transformation * data2.MatrixBinding;
			data2.MatrixCurrent = m;
			data2.BoneIndices = data.BoneIndices;
			data2.BoneWeights = data.BoneWeights;
			MyDecalFlags myDecalFlags = (myDecalMaterial.Transparent ? MyDecalFlags.Transparent : MyDecalFlags.None);
			string stringId = MyDecalMaterials.GetStringId(data.Source, data.PhysicalMaterial);
<<<<<<< HEAD
			uint item = MyRenderProxy.CreateDecal((uint[])data.RenderObjectIds.Clone(), ref data2, data.Flags | myDecalFlags, stringId, myDecalMaterial.StringId, num, data.RenderDistance, data.IsTrail, data.AliveUntil);
=======
			uint item = MyRenderProxy.CreateDecal((uint[])data.RenderObjectIds.Clone(), ref data2, data.Flags | myDecalFlags, stringId, myDecalMaterial.StringId, num, data.RenderDistance, data.IsTrail);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ids?.Add(item);
		}
	}
}
