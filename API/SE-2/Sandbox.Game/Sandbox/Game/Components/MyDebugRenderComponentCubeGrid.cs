using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentCubeGrid : MyDebugRenderComponent
	{
		private MyCubeGrid m_cubeGrid;

		private Dictionary<Vector3I, MyTimeSpan> m_dirtyBlocks = new Dictionary<Vector3I, MyTimeSpan>();

		private List<Vector3I> m_tmpRemoveList = new List<Vector3I>();

		private List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();

		private readonly List<MyCubeGrid.DebugUpdateRecord> m_gridDebugUpdateInfo = new List<MyCubeGrid.DebugUpdateRecord>();

		public MyDebugRenderComponentCubeGrid(MyCubeGrid cubeGrid)
			: base(cubeGrid)
		{
			m_cubeGrid = cubeGrid;
		}

		public override void PrepareForDraw()
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			base.PrepareForDraw();
			if (!MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)
			{
				return;
			}
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(1500.0);
			using (m_tmpRemoveList.GetClearToken())
			{
				foreach (KeyValuePair<Vector3I, MyTimeSpan> dirtyBlock in m_dirtyBlocks)
				{
					if (MySandboxGame.Static.TotalTime - dirtyBlock.Value > myTimeSpan)
					{
						m_tmpRemoveList.Add(dirtyBlock.Key);
					}
				}
				foreach (Vector3I tmpRemove in m_tmpRemoveList)
<<<<<<< HEAD
				{
					m_dirtyBlocks.Remove(tmpRemove);
				}
			}
			foreach (Vector3I dirtyBlock2 in m_cubeGrid.DirtyBlocks)
			{
				m_dirtyBlocks[dirtyBlock2] = MySandboxGame.Static.TotalTime;
=======
				{
					m_dirtyBlocks.Remove(tmpRemove);
				}
			}
			Enumerator<Vector3I> enumerator3 = m_cubeGrid.DirtyBlocks.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					Vector3I current3 = enumerator3.get_Current();
					m_dirtyBlocks[current3] = MySandboxGame.Static.TotalTime;
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public override void DebugDraw()
		{
<<<<<<< HEAD
=======
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_062f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
			//IL_1054: Unknown result type (might be due to invalid IL or missing references)
			//IL_1059: Unknown result type (might be due to invalid IL or missing references)
			//IL_1122: Unknown result type (might be due to invalid IL or missing references)
			//IL_1127: Unknown result type (might be due to invalid IL or missing references)
			//IL_119f: Unknown result type (might be due to invalid IL or missing references)
			//IL_11a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_137d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1382: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyPhysicsConfig.EnableGridSpeedDebugDraw && Entity.Physics != null)
			{
				Color color = ((!(Entity.Physics.RigidBody.MaxLinearVelocity > 190f)) ? Color.Red : Color.Green);
				MyRenderProxy.DebugDrawText3D(Entity.PositionComp.GetPosition(), Entity.Physics.LinearVelocity.Length().ToString("F2"), color, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(Entity.PositionComp.GetPosition() + Vector3.One * 3f, Entity.Physics.AngularVelocity.Length().ToString("F2"), color, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_UPDATES)
			{
				Vector3D vector3D = Vector3D.Transform(Entity.PositionComp.LocalAABB.Center, Entity.PositionComp.WorldMatrixRef);
				float scale = (float)MathHelper.Clamp(Entity.PositionComp.WorldVolume.Radius * 2.0 / Vector3D.Distance(MySector.MainCamera.Position, vector3D), 0.01, 1.0);
				m_gridDebugUpdateInfo.Clear();
				m_cubeGrid.GetDebugUpdateInfo(m_gridDebugUpdateInfo);
				if (m_gridDebugUpdateInfo.Count == 0)
				{
					MyRenderProxy.DebugDrawText3D(vector3D, "No Updates", Color.Gray, scale, depthRead: false);
				}
				else
				{
					MyRenderProxy.DebugDrawText3D(vector3D, string.Join("\n", m_gridDebugUpdateInfo), Color.Wheat, scale, depthRead: false);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_AABB)
			{
				BoundingBox localAABB = m_cubeGrid.PositionComp.LocalAABB;
				MatrixD worldMatrixRef = m_cubeGrid.PositionComp.WorldMatrixRef;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(localAABB, worldMatrixRef);
				Color yellow = Color.Yellow;
				MyRenderProxy.DebugDrawOBB(obb, yellow, 0.2f, depthRead: false, smooth: true);
				MyRenderProxy.DebugDrawAxis(worldMatrixRef, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_FIXED_BLOCK_QUERIES)
			{
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.GetBlocks().GetEnumerator();
				try
				{
<<<<<<< HEAD
					BoundingBox geometryLocalBox = block.FatBlock.GetGeometryLocalBox();
					Vector3 halfExtents = geometryLocalBox.Size / 2f;
					block.ComputeScaledCenter(out var scaledCenter);
					scaledCenter += geometryLocalBox.Center;
					scaledCenter = Vector3D.Transform(scaledCenter, m_cubeGrid.WorldMatrix);
					block.Orientation.GetMatrix(out var result);
					MatrixD matrix = result * m_cubeGrid.WorldMatrix.GetOrientation();
					Quaternion rotation = Quaternion.CreateFromRotationMatrix(in matrix);
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref scaledCenter, ref rotation, m_penetrations, 14);
					bool flag = false;
					foreach (HkBodyCollision penetration in m_penetrations)
=======
					while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MySlimBlock current = enumerator.get_Current();
						BoundingBox geometryLocalBox = current.FatBlock.GetGeometryLocalBox();
						Vector3 halfExtents = geometryLocalBox.Size / 2f;
						current.ComputeScaledCenter(out var scaledCenter);
						scaledCenter += geometryLocalBox.Center;
						scaledCenter = Vector3D.Transform(scaledCenter, m_cubeGrid.WorldMatrix);
						current.Orientation.GetMatrix(out var result);
						MatrixD matrix = result * m_cubeGrid.WorldMatrix.GetOrientation();
						Quaternion rotation = Quaternion.CreateFromRotationMatrix(in matrix);
						MyPhysics.GetPenetrationsBox(ref halfExtents, ref scaledCenter, ref rotation, m_penetrations, 14);
						bool flag = false;
						foreach (HkBodyCollision penetration in m_penetrations)
						{
							IMyEntity collisionEntity = penetration.GetCollisionEntity();
							if (collisionEntity != null && collisionEntity is MyVoxelMap)
							{
								flag = true;
								break;
							}
						}
						m_penetrations.Clear();
						MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(scaledCenter, halfExtents, rotation), flag ? Color.Green : Color.Red, 0.1f, depthRead: false, smooth: false);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES || MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)
			{
				string text = "";
				Color color2 = Color.White;
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES)
				{
					text = text + m_cubeGrid.ToString() + " ";
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)
				{
					MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(m_cubeGrid);
					if (controllingPlayer != null)
					{
						text = text + "Controlled by: " + controllingPlayer.DisplayName;
						color2 = Color.LightGreen;
					}
				}
				MyRenderProxy.DebugDrawText3D(m_cubeGrid.PositionComp.WorldAABB.Center, text, color2, 0.7f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
			_ = m_cubeGrid.Render;
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)
			{
				Vector3D translation = m_cubeGrid.PositionComp.WorldMatrixRef.Translation;
				foreach (MyBlockGroup blockGroup in m_cubeGrid.BlockGroups)
				{
					MyRenderProxy.DebugDrawText3D(translation, blockGroup.Name.ToString(), Color.Red, 1f, depthRead: false);
					translation += m_cubeGrid.PositionComp.WorldMatrixRef.Right * blockGroup.Name.Length * 0.10000000149011612;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)
			{
				foreach (KeyValuePair<Vector3I, MyTimeSpan> dirtyBlock in m_dirtyBlocks)
				{
					Vector3 vector = ((m_cubeGrid.GetCubeBlock(dirtyBlock.Key) != null) ? Color.Red.ToVector3() : Color.Yellow.ToVector3());
					MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(m_cubeGrid.GridSize) * Matrix.CreateTranslation(dirtyBlock.Key * m_cubeGrid.GridSize) * m_cubeGrid.WorldMatrix, vector, 0.15f, depthRead: false, smooth: true);
				}
			}
<<<<<<< HEAD
			if (MyDebugDrawSettings.DEBUG_DRAW_VERTICES_CACHE && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldMatrixRef.Translation).LengthSquared() < 600.0)
			{
				List<Vector3> list = new List<Vector3>();
				int num = 0;
				foreach (MySlimBlock cubeBlock in m_cubeGrid.CubeBlocks)
				{
					num++;
					list.Clear();
					if (cubeBlock.BlockDefinition.CubeDefinition == null)
					{
						continue;
					}
					Vector3 translation2 = cubeBlock.Position * m_cubeGrid.GridSize;
					cubeBlock.Orientation.GetMatrix(out var result2);
					result2.Translation = translation2;
					MyBlockVerticesCache.GetTopologySwitch(cubeBlock.BlockDefinition.CubeDefinition.CubeTopology, list);
					MyCubeGridDefinitions.TableEntry topologyInfo = MyCubeGridDefinitions.GetTopologyInfo(cubeBlock.BlockDefinition.CubeDefinition.CubeTopology);
					MyEdgeDefinition[] edges = topologyInfo.Edges;
					for (int i = 0; i < edges.Length; i++)
					{
						MyEdgeDefinition myEdgeDefinition = edges[i];
						Vector3 vector2 = Vector3.TransformNormal(myEdgeDefinition.Point0, cubeBlock.Orientation);
						Vector3 vector3 = Vector3.TransformNormal(myEdgeDefinition.Point1, cubeBlock.Orientation);
						Vector3 vector4 = (vector2 + vector3) * 0.5f;
						Vector3.Transform(myEdgeDefinition.Point0 * m_cubeGrid.GridSizeHalf, ref result2);
						vector3 = Vector3.Transform(myEdgeDefinition.Point1 * m_cubeGrid.GridSizeHalf, ref result2);
						if (myEdgeDefinition.Side0 < topologyInfo.Tiles.Length && myEdgeDefinition.Side1 < topologyInfo.Tiles.Length && myEdgeDefinition.Side0 >= 0 && myEdgeDefinition.Side1 >= 0)
						{
							Vector3 vector5 = Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side0].Normal, cubeBlock.Orientation);
							Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side1].Normal, cubeBlock.Orientation);
							MyRenderProxy.DebugDrawLine3D(vector4, vector4 + vector5, MyDebugDrawSettings.GetColor(num), MyDebugDrawSettings.GetColor(num), depthRead: true);
						}
					}
					cubeBlock.Orientation.GetMatrix(out var result3);
					for (int j = 0; j < list.Count - 1; j++)
					{
						Vector3D pointFrom = Vector3.Transform(Vector3.Transform(list[j], result3) + cubeBlock.Position * m_cubeGrid.GridSize, m_cubeGrid.WorldMatrix);
						Vector3D pointTo = Vector3.Transform(Vector3.Transform(list[j + 1], result3) + cubeBlock.Position * m_cubeGrid.GridSize, m_cubeGrid.WorldMatrix);
						MyRenderProxy.DebugDrawArrow3D(pointFrom, pointTo, Color.Violet, null, depthRead: true);
					}
					for (int k = 0; k < list.Count; k++)
					{
						Vector3D vector3D2 = Vector3.Transform(Vector3.Transform(list[k], result3) + cubeBlock.Position * m_cubeGrid.GridSize, m_cubeGrid.WorldMatrix);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.025f, Color.Green, 0.5f);
						MyRenderProxy.DebugDrawText3D(vector3D2, k.ToString(), Color.Green, 0.5f, depthRead: true);
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_SKETELON_CUBE_BONES && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldMatrixRef.Translation).LengthSquared() < 900.0 && MySession.Static.GameplayFrameCounter % 10 == 0)
			{
				Dictionary<MyCubeTopology, HashSet<Vector3I>> dictionary = new Dictionary<MyCubeTopology, HashSet<Vector3I>>();
				foreach (MySlimBlock cubeBlock2 in m_cubeGrid.CubeBlocks)
				{
					if (cubeBlock2.BlockDefinition.CubeDefinition == null)
					{
						continue;
					}
					MyCubeTopology cubeTopology = cubeBlock2.BlockDefinition.CubeDefinition.CubeTopology;
					if (!m_cubeGrid.TryGetCube(cubeBlock2.Position, out var cube) || dictionary.ContainsKey(cubeTopology))
					{
						continue;
					}
					MyCubeGridDefinitions.GetCubeTiles(cubeBlock2.BlockDefinition);
					cube.CubeBlock.Orientation.GetMatrix(out var _);
					dictionary.Add(cubeTopology, new HashSet<Vector3I>());
					MyCubePart[] parts = cube.Parts;
					foreach (MyCubePart myCubePart in parts)
					{
						if (myCubePart.Model.BoneMapping != null)
						{
							_ = Vector3D.Zero;
							for (int l = 0; l < Math.Min(myCubePart.Model.BoneMapping.Length, 9); l++)
							{
								Vector3I vector3I = myCubePart.Model.BoneMapping[l];
								Matrix orientation = myCubePart.InstanceData.LocalMatrix.GetOrientation();
								Vector3 vector6 = vector3I * 1f - Vector3.One;
								Vector3I vector3I2 = Vector3I.Round(Vector3.Transform(vector6 * 1f, orientation));
								Vector3I.Round(Vector3.Transform(vector6 * 1f, orientation) + Vector3.One);
								dictionary[cubeTopology].Add(vector3I2 + Vector3I.One);
							}
						}
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<MyCubeTopology, HashSet<Vector3I>> item in dictionary)
				{
					stringBuilder.AppendLine($"      <Skeleton><!--{item.Key}-->");
					foreach (Vector3I item2 in item.Value)
					{
						stringBuilder.AppendLine("        <BoneInfo>");
						stringBuilder.AppendLine($"          <BonePosition x=\"{item2.X}\" y=\"{item2.Y}\" z=\"{item2.Z}\" />");
						stringBuilder.AppendLine("          <BoneOffset x=\"127\" y=\"127\" z=\"127\" />");
						stringBuilder.AppendLine("        </BoneInfo>");
					}
					stringBuilder.AppendLine("      </Skeleton>");
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				MyLog.Default.WriteLine(stringBuilder.ToString());
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldMatrixRef.Translation).LengthSquared() < 600.0)
			{
				_ = MySector.MainCamera.Position;
				foreach (MySlimBlock cubeBlock3 in m_cubeGrid.CubeBlocks)
				{
					if (!m_cubeGrid.TryGetCube(cubeBlock3.Position, out var cube2))
					{
						continue;
					}
					int num2 = 0;
					MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(cubeBlock3.BlockDefinition);
					cube2.CubeBlock.Orientation.GetMatrix(out var result5);
					MyCubePart[] parts = cube2.Parts;
					foreach (MyCubePart myCubePart2 in parts)
					{
						if (myCubePart2.Model.BoneMapping != null && (num2 == MyPetaInputComponent.DEBUG_INDEX || num2 == 7))
						{
							Vector3D zero = Vector3D.Zero;
							for (int m = 0; m < Math.Min(myCubePart2.Model.BoneMapping.Length, 9); m++)
							{
								Vector3I vector3I3 = myCubePart2.Model.BoneMapping[m];
								Matrix orientation2 = myCubePart2.InstanceData.LocalMatrix.GetOrientation();
								Vector3 vector7 = vector3I3 * 1f - Vector3.One;
								Vector3I vector3I4 = Vector3I.Round(Vector3.Transform(vector7 * 1f, orientation2));
								Vector3I bonePos = Vector3I.Round(Vector3.Transform(vector7 * 1f, orientation2) + Vector3.One);
								Vector3 position = m_cubeGrid.GridSize * (cubeBlock3.Position + vector3I4 / 2f);
								Vector3 bone = m_cubeGrid.Skeleton.GetBone(cubeBlock3.Position, bonePos);
								Vector3D vector3D3 = Vector3D.Transform(position, m_cubeGrid.PositionComp.WorldMatrixRef);
								Vector3D vector3D4 = Vector3D.TransformNormal(bone, m_cubeGrid.PositionComp.WorldMatrixRef);
								MyRenderProxy.DebugDrawSphere(vector3D3, 0.025f, Color.Green, 0.5f, depthRead: false, smooth: true);
								MyRenderProxy.DebugDrawText3D(vector3D3, m.ToString() + "  (" + vector3I3.X + "," + vector3I3.Y + "," + vector3I3.Z + ")", Color.Green, 0.5f, depthRead: false);
								MyRenderProxy.DebugDrawArrow3D(vector3D3, vector3D3 + vector3D4, Color.Red);
								zero += vector3D3;
							}
							zero /= (double)Math.Min(myCubePart2.Model.BoneMapping.Length, 9);
							try
							{
								Vector3 vector8 = Vector3.TransformNormal(cubeTiles[num2].Normal, result5);
								MyRenderProxy.DebugDrawArrow3D(zero, zero + vector8, Color.Purple);
							}
							catch (Exception)
							{
							}
=======
			if (MyDebugDrawSettings.DEBUG_DRAW_VERTICES_CACHE && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldMatrix.Translation).LengthSquared() < 600.0)
			{
				List<Vector3> list = new List<Vector3>();
				int num = 0;
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current4 = enumerator.get_Current();
						num++;
						list.Clear();
						if (current4.BlockDefinition.CubeDefinition == null)
						{
							continue;
						}
						Vector3 translation2 = current4.Position * m_cubeGrid.GridSize;
						current4.Orientation.GetMatrix(out var result2);
						result2.Translation = translation2;
						MyBlockVerticesCache.GetTopologySwitch(current4.BlockDefinition.CubeDefinition.CubeTopology, list);
						MyCubeGridDefinitions.TableEntry topologyInfo = MyCubeGridDefinitions.GetTopologyInfo(current4.BlockDefinition.CubeDefinition.CubeTopology);
						MyEdgeDefinition[] edges = topologyInfo.Edges;
						for (int i = 0; i < edges.Length; i++)
						{
							MyEdgeDefinition myEdgeDefinition = edges[i];
							Vector3 vector2 = Vector3.TransformNormal(myEdgeDefinition.Point0, current4.Orientation);
							Vector3 vector3 = Vector3.TransformNormal(myEdgeDefinition.Point1, current4.Orientation);
							Vector3 vector4 = (vector2 + vector3) * 0.5f;
							Vector3.Transform(myEdgeDefinition.Point0 * m_cubeGrid.GridSizeHalf, ref result2);
							vector3 = Vector3.Transform(myEdgeDefinition.Point1 * m_cubeGrid.GridSizeHalf, ref result2);
							if (myEdgeDefinition.Side0 < topologyInfo.Tiles.Length && myEdgeDefinition.Side1 < topologyInfo.Tiles.Length && myEdgeDefinition.Side0 >= 0 && myEdgeDefinition.Side1 >= 0)
							{
								Vector3 vector5 = Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side0].Normal, current4.Orientation);
								Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side1].Normal, current4.Orientation);
								MyRenderProxy.DebugDrawLine3D(vector4, vector4 + vector5, MyDebugDrawSettings.GetColor(num), MyDebugDrawSettings.GetColor(num), depthRead: true);
							}
						}
						current4.Orientation.GetMatrix(out var result3);
						for (int j = 0; j < list.Count - 1; j++)
						{
							Vector3D pointFrom = Vector3.Transform(Vector3.Transform(list[j], result3) + current4.Position * m_cubeGrid.GridSize, m_cubeGrid.WorldMatrix);
							Vector3D pointTo = Vector3.Transform(Vector3.Transform(list[j + 1], result3) + current4.Position * m_cubeGrid.GridSize, m_cubeGrid.WorldMatrix);
							MyRenderProxy.DebugDrawArrow3D(pointFrom, pointTo, Color.Violet, null, depthRead: true);
						}
						for (int k = 0; k < list.Count; k++)
						{
							Vector3D vector3D2 = Vector3.Transform(Vector3.Transform(list[k], result3) + current4.Position * m_cubeGrid.GridSize, m_cubeGrid.WorldMatrix);
							MyRenderProxy.DebugDrawSphere(vector3D2, 0.025f, Color.Green, 0.5f);
							MyRenderProxy.DebugDrawText3D(vector3D2, k.ToString(), Color.Green, 0.5f, depthRead: true);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_SKETELON_CUBE_BONES && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldMatrix.Translation).LengthSquared() < 900.0 && MySession.Static.GameplayFrameCounter % 10 == 0)
			{
				Dictionary<MyCubeTopology, HashSet<Vector3I>> dictionary = new Dictionary<MyCubeTopology, HashSet<Vector3I>>();
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current5 = enumerator.get_Current();
						if (current5.BlockDefinition.CubeDefinition == null)
						{
							continue;
						}
						MyCubeTopology cubeTopology = current5.BlockDefinition.CubeDefinition.CubeTopology;
						if (!m_cubeGrid.TryGetCube(current5.Position, out var cube) || dictionary.ContainsKey(cubeTopology))
						{
							continue;
						}
						MyCubeGridDefinitions.GetCubeTiles(current5.BlockDefinition);
						cube.CubeBlock.Orientation.GetMatrix(out var _);
						dictionary.Add(cubeTopology, new HashSet<Vector3I>());
						MyCubePart[] parts = cube.Parts;
						foreach (MyCubePart myCubePart in parts)
						{
							if (myCubePart.Model.BoneMapping != null)
							{
								_ = Vector3D.Zero;
								for (int l = 0; l < Math.Min(myCubePart.Model.BoneMapping.Length, 9); l++)
								{
									Vector3I vector3I = myCubePart.Model.BoneMapping[l];
									Matrix orientation = myCubePart.InstanceData.LocalMatrix.GetOrientation();
									Vector3 vector6 = vector3I * 1f - Vector3.One;
									Vector3I vector3I2 = Vector3I.Round(Vector3.Transform(vector6 * 1f, orientation));
									Vector3I.Round(Vector3.Transform(vector6 * 1f, orientation) + Vector3.One);
									dictionary[cubeTopology].Add(vector3I2 + Vector3I.One);
								}
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<MyCubeTopology, HashSet<Vector3I>> item in dictionary)
				{
					stringBuilder.AppendLine($"      <Skeleton><!--{item.Key}-->");
					Enumerator<Vector3I> enumerator6 = item.Value.GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							Vector3I current7 = enumerator6.get_Current();
							stringBuilder.AppendLine("        <BoneInfo>");
							stringBuilder.AppendLine($"          <BonePosition x=\"{current7.X}\" y=\"{current7.Y}\" z=\"{current7.Z}\" />");
							stringBuilder.AppendLine("          <BoneOffset x=\"127\" y=\"127\" z=\"127\" />");
							stringBuilder.AppendLine("        </BoneInfo>");
						}
					}
					finally
					{
						((IDisposable)enumerator6).Dispose();
					}
					stringBuilder.AppendLine("      </Skeleton>");
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				MyLog.Default.WriteLine(stringBuilder.ToString());
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldMatrix.Translation).LengthSquared() < 600.0)
			{
				_ = MySector.MainCamera.Position;
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current8 = enumerator.get_Current();
						if (!m_cubeGrid.TryGetCube(current8.Position, out var cube2))
						{
							continue;
						}
						int num2 = 0;
						MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(current8.BlockDefinition);
						cube2.CubeBlock.Orientation.GetMatrix(out var result5);
						MyCubePart[] parts = cube2.Parts;
						foreach (MyCubePart myCubePart2 in parts)
						{
							if (myCubePart2.Model.BoneMapping != null && (num2 == MyPetaInputComponent.DEBUG_INDEX || num2 == 7))
							{
								Vector3D zero = Vector3D.Zero;
								for (int m = 0; m < Math.Min(myCubePart2.Model.BoneMapping.Length, 9); m++)
								{
									Vector3I vector3I3 = myCubePart2.Model.BoneMapping[m];
									Matrix orientation2 = myCubePart2.InstanceData.LocalMatrix.GetOrientation();
									Vector3 vector7 = vector3I3 * 1f - Vector3.One;
									Vector3I vector3I4 = Vector3I.Round(Vector3.Transform(vector7 * 1f, orientation2));
									Vector3I bonePos = Vector3I.Round(Vector3.Transform(vector7 * 1f, orientation2) + Vector3.One);
									Vector3 position = m_cubeGrid.GridSize * (current8.Position + vector3I4 / 2f);
									Vector3 bone = m_cubeGrid.Skeleton.GetBone(current8.Position, bonePos);
									Vector3D vector3D3 = Vector3D.Transform(position, m_cubeGrid.PositionComp.WorldMatrixRef);
									Vector3D vector3D4 = Vector3D.TransformNormal(bone, m_cubeGrid.PositionComp.WorldMatrixRef);
									MyRenderProxy.DebugDrawSphere(vector3D3, 0.025f, Color.Green, 0.5f, depthRead: false, smooth: true);
									MyRenderProxy.DebugDrawText3D(vector3D3, m.ToString() + "  (" + vector3I3.X + "," + vector3I3.Y + "," + vector3I3.Z + ")", Color.Green, 0.5f, depthRead: false);
									MyRenderProxy.DebugDrawArrow3D(vector3D3, vector3D3 + vector3D4, Color.Red);
									zero += vector3D3;
								}
								zero /= (double)Math.Min(myCubePart2.Model.BoneMapping.Length, 9);
								try
								{
									Vector3 vector8 = Vector3.TransformNormal(cubeTiles[num2].Normal, result5);
									MyRenderProxy.DebugDrawArrow3D(zero, zero + vector8, Color.Purple);
								}
								catch (Exception)
								{
								}
							}
							num2++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						num2++;
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CUBES)
			{
<<<<<<< HEAD
				foreach (MySlimBlock cubeBlock4 in m_cubeGrid.CubeBlocks)
				{
					cubeBlock4.GetLocalMatrix(out var localMatrix);
					MyRenderProxy.DebugDrawAxis(localMatrix * m_cubeGrid.WorldMatrix, 1f, depthRead: false);
					cubeBlock4.FatBlock?.DebugDraw();
=======
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current9 = enumerator.get_Current();
						current9.GetLocalMatrix(out var localMatrix);
						MyRenderProxy.DebugDrawAxis(localMatrix * m_cubeGrid.WorldMatrix, 1f, depthRead: false);
						current9.FatBlock?.DebugDraw();
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			m_cubeGrid.GridSystems.DebugDraw();
			_ = MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS;
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_ORIGINS)
			{
				MyRenderProxy.DebugDrawAxis(m_cubeGrid.PositionComp.WorldMatrixRef, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_ALL)
			{
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.GetBlocks().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current10 = enumerator.get_Current();
						if ((m_cubeGrid.GridIntegerToWorld(current10.Position) - MySector.MainCamera.Position).LengthSquared() < 200.0)
						{
							DebugDrawMountPoints(current10);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_ARMOR_BLOCK_TILE_NORMALS)
			{
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.GetBlocks().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current11 = enumerator.get_Current();
						if (!((m_cubeGrid.GridIntegerToWorld(current11.Position) - MySector.MainCamera.Position).LengthSquared() < 200.0) || !m_cubeGrid.TryGetCube(current11.Position, out var cube3))
						{
							continue;
						}
						cube3.CubeBlock.Orientation.GetMatrix(out var result6);
						MyTileDefinition[] cubeTiles2 = MyCubeGridDefinitions.GetCubeTiles(current11.BlockDefinition);
						for (int n = 0; n < cube3.Parts.Length; n++)
						{
							MyCubePart myCubePart3 = cube3.Parts[n];
							Vector3 pos = myCubePart3.InstanceData.Translation;
							if (!m_cubeGrid.Render.RenderData.GetCell(ref pos, create: false).HasCubePart(myCubePart3))
							{
								MyTileDefinition myTileDefinition = cubeTiles2[n];
								Vector3 vector9 = Vector3.TransformNormal(myTileDefinition.Normal, result6);
								Vector3 vector10 = Vector3.TransformNormal(myTileDefinition.Up, result6);
								MyRenderProxy.DebugDrawLine3D(current11.WorldPosition, current11.WorldPosition + vector10, Color.Red, Color.Red, depthRead: true);
								MyRenderProxy.DebugDrawLine3D(current11.WorldPosition, current11.WorldPosition + vector9, Color.Green, Color.Green, depthRead: true);
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_ARMOR_BLOCK_TILE_NORMALS)
			{
<<<<<<< HEAD
				foreach (MySlimBlock block3 in m_cubeGrid.GetBlocks())
				{
					if (!((m_cubeGrid.GridIntegerToWorld(block3.Position) - MySector.MainCamera.Position).LengthSquared() < 200.0) || !m_cubeGrid.TryGetCube(block3.Position, out var cube3))
					{
						continue;
					}
					cube3.CubeBlock.Orientation.GetMatrix(out var result6);
					MyTileDefinition[] cubeTiles2 = MyCubeGridDefinitions.GetCubeTiles(block3.BlockDefinition);
					for (int n = 0; n < cube3.Parts.Length; n++)
					{
						MyCubePart myCubePart3 = cube3.Parts[n];
						Vector3 pos = myCubePart3.InstanceData.Translation;
						if (!m_cubeGrid.Render.RenderData.GetCell(ref pos, create: false).HasCubePart(myCubePart3))
						{
							MyTileDefinition myTileDefinition = cubeTiles2[n];
							Vector3 vector9 = Vector3.TransformNormal(myTileDefinition.Normal, result6);
							Vector3 vector10 = Vector3.TransformNormal(myTileDefinition.Up, result6);
							MyRenderProxy.DebugDrawLine3D(block3.WorldPosition, block3.WorldPosition + vector10, Color.Red, Color.Red, depthRead: true);
							MyRenderProxy.DebugDrawLine3D(block3.WorldPosition, block3.WorldPosition + vector9, Color.Green, Color.Green, depthRead: true);
=======
				Enumerator<MySlimBlock> enumerator = m_cubeGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current12 = enumerator.get_Current();
						Vector3D vector3D5 = m_cubeGrid.GridIntegerToWorld(current12.Position);
						if (m_cubeGrid.GridSizeEnum != 0 && (MySector.MainCamera == null || !((MySector.MainCamera.Position - vector3D5).LengthSquared() < 9.0)))
						{
							continue;
						}
						float num3 = 0f;
						if (current12.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block in (current12.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								num3 += block.Integrity * block.BlockDefinition.MaxIntegrityRatio;
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_INTEGRITY && MySector.MainCamera != null && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldVolume.Center).Length() < 16.0 + m_cubeGrid.PositionComp.WorldVolume.Radius)
			{
				foreach (MySlimBlock cubeBlock5 in m_cubeGrid.CubeBlocks)
				{
					Vector3D vector3D5 = m_cubeGrid.GridIntegerToWorld(cubeBlock5.Position);
					if (m_cubeGrid.GridSizeEnum != 0 && (MySector.MainCamera == null || !((MySector.MainCamera.Position - vector3D5).LengthSquared() < 9.0)))
					{
						continue;
					}
					float num3 = 0f;
					if (cubeBlock5.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block4 in (cubeBlock5.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
<<<<<<< HEAD
							num3 += block4.Integrity * block4.BlockDefinition.MaxIntegrityRatio;
						}
=======
							num3 = current12.Integrity * current12.BlockDefinition.MaxIntegrityRatio;
						}
						MyRenderProxy.DebugDrawText3D(m_cubeGrid.GridIntegerToWorld(current12.Position), ((int)num3).ToString(), Color.White, (m_cubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.65f : 0.5f, depthRead: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						num3 = cubeBlock5.Integrity * cubeBlock5.BlockDefinition.MaxIntegrityRatio;
					}
					MyRenderProxy.DebugDrawText3D(m_cubeGrid.GridIntegerToWorld(cubeBlock5.Position), ((int)num3).ToString(), Color.White, (m_cubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.65f : 0.5f, depthRead: false);
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			base.DebugDraw();
		}

		private void DebugDrawMountPoints(MySlimBlock block)
		{
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					DebugDrawMountPoints(block2);
				}
				return;
			}
			block.GetLocalMatrix(out var localMatrix);
			MatrixD drawMatrix = localMatrix * m_cubeGrid.WorldMatrix;
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(block.BlockDefinition.Id, out var blockDefinition);
			if (MyFakes.ENABLE_FRACTURE_COMPONENT && block.FatBlock != null && block.FatBlock.Components.Has<MyFractureComponentBase>())
			{
				MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
				if (fractureComponent != null)
				{
					MyCubeBuilder.DrawMountPoints(m_cubeGrid.GridSize, blockDefinition, drawMatrix, Enumerable.ToArray<MyCubeBlockDefinition.MountPoint>((IEnumerable<MyCubeBlockDefinition.MountPoint>)fractureComponent.MountPoints));
				}
			}
			else
			{
				MyCubeBuilder.DrawMountPoints(m_cubeGrid.GridSize, blockDefinition, ref drawMatrix);
			}
		}

		public override void DebugDrawInvalidTriangles()
		{
			base.DebugDrawInvalidTriangles();
			foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in m_cubeGrid.Render.RenderData.Cells)
			{
				foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
				{
					MyModel model = cubePart.Key.Model;
					if (model == null)
<<<<<<< HEAD
					{
						continue;
					}
					int trianglesCount = model.GetTrianglesCount();
					for (int i = 0; i < trianglesCount; i++)
					{
=======
					{
						continue;
					}
					int trianglesCount = model.GetTrianglesCount();
					for (int i = 0; i < trianglesCount; i++)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MyTriangleVertexIndices triangle = model.GetTriangle(i);
						if (MyUtils.IsWrongTriangle(model.GetVertex(triangle.I0), model.GetVertex(triangle.I1), model.GetVertex(triangle.I2)))
						{
							Vector3 vector = Vector3.Transform(model.GetVertex(triangle.I0), (Matrix)m_cubeGrid.PositionComp.WorldMatrixRef);
							Vector3 vector2 = Vector3.Transform(model.GetVertex(triangle.I1), (Matrix)m_cubeGrid.PositionComp.WorldMatrixRef);
							Vector3 vector3 = Vector3.Transform(model.GetVertex(triangle.I2), (Matrix)m_cubeGrid.PositionComp.WorldMatrixRef);
							MyRenderProxy.DebugDrawLine3D(vector, vector2, Color.Purple, Color.Purple, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector2, vector3, Color.Purple, Color.Purple, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector3, vector, Color.Purple, Color.Purple, depthRead: false);
							Vector3 vector4 = (vector + vector2 + vector3) / 3f;
							MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitX, Color.Yellow, Color.Yellow, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitY, Color.Yellow, Color.Yellow, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitZ, Color.Yellow, Color.Yellow, depthRead: false);
						}
					}
				}
			}
		}
	}
}
