using System;
<<<<<<< HEAD
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using System.Threading;
using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	internal class MyOndraInputComponent : MyDebugComponent
	{
		private bool m_gridDebugInfo;

		public override string GetName()
		{
			return "Ondra";
		}

		private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
		{
		}

		private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
		{
		}

		public override void Draw()
		{
			base.Draw();
		}

		public override bool HandleInput()
		{
			bool result = false;
			if (m_gridDebugInfo)
			{
				LineD line = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
				if (MyCubeGrid.GetLineIntersection(ref line, out var grid, out var position, out var _))
				{
					MatrixD worldMatrix = grid.WorldMatrix;
					MatrixD matrixD = Matrix.CreateTranslation(position * grid.GridSize) * worldMatrix;
					grid.GetCubeBlock(position);
					MyRenderProxy.DebugDrawText2D(default(Vector2), position.ToString(), Color.White, 0.7f);
					MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(new Vector3(grid.GridSize) + new Vector3(0.15f)) * matrixD, Color.Red.ToVector3(), 0.2f, depthRead: true, smooth: true);
				}
			}
			if (MyInput.Static.IsAnyAltKeyPressed())
			{
				return result;
			}
			MyInput.Static.IsAnyShiftKeyPressed();
			MyInput.Static.IsAnyCtrlKeyPressed();
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad6))
			{
				MatrixD m = MySector.MainCamera.ViewMatrix;
				Matrix matrix = Matrix.Invert(m);
				MyObjectBuilder_Ore content = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
				MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(1, content), matrix.Translation + matrix.Forward * 1f, matrix.Forward, matrix.Up);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad7))
			{
				foreach (MyCubeGrid item in Enumerable.OfType<MyCubeGrid>((IEnumerable)MyEntities.GetEntities()))
				{
					foreach (MyMotorStator item2 in Enumerable.OfType<MyMotorStator>((IEnumerable)Enumerable.Where<MyCubeBlock>(Enumerable.Select<MySlimBlock, MyCubeBlock>((IEnumerable<MySlimBlock>)item.CubeBlocks, (Func<MySlimBlock, MyCubeBlock>)((MySlimBlock s) => s.FatBlock)), (Func<MyCubeBlock, bool>)((MyCubeBlock s) => s != null))))
					{
						if (item2.Rotor != null)
						{
							Quaternion quaternion = Quaternion.CreateFromAxisAngle(item2.Rotor.WorldMatrix.Up, MathHelper.ToRadians(45f));
							item2.Rotor.CubeGrid.WorldMatrix = MatrixD.CreateFromQuaternion(quaternion) * item2.Rotor.CubeGrid.WorldMatrix;
						}
					}
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad8))
			{
				MatrixD m2 = MySector.MainCamera.ViewMatrix;
				Matrix matrix2 = Matrix.Invert(m2);
				MyObjectBuilder_Ore physicalContent = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
				MyEntities.CreateFromObjectBuilderAndAdd(new MyObjectBuilder_FloatingObject
				{
					Item = new MyObjectBuilder_InventoryItem
					{
						PhysicalContent = physicalContent,
						Amount = 1000
					},
					PositionAndOrientation = new MyPositionAndOrientation(matrix2.Translation + 2f * matrix2.Forward, matrix2.Forward, matrix2.Up),
					PersistentFlags = MyPersistentEntityFlags2.InScene
				}, fadeIn: false).Physics.LinearVelocity = Vector3.Normalize(matrix2.Forward) * 50f;
			}
			MyInput.Static.IsNewKeyPressed(MyKeys.Divide);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply))
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = !MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
				foreach (MyCubeGrid item3 in Enumerable.OfType<MyCubeGrid>((IEnumerable)MyEntities.GetEntities()))
				{
					_ = item3.IsStatic;
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad1))
			{
				MyCubeGrid myCubeGrid = Enumerable.FirstOrDefault<MyCubeGrid>(Enumerable.OfType<MyCubeGrid>((IEnumerable)MyEntities.GetEntities()));
				if (myCubeGrid != null)
				{
					myCubeGrid.Physics.RigidBody.MaxLinearVelocity = 1000f;
					if (myCubeGrid.Physics.RigidBody2 != null)
					{
						myCubeGrid.Physics.RigidBody2.MaxLinearVelocity = 1000f;
					}
					myCubeGrid.Physics.LinearVelocity = new Vector3(1000f, 0f, 0f);
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Decimal))
			{
				MyPrefabManager.Static.SpawnPrefab("respawnship", MySector.MainCamera.Position, MySector.MainCamera.ForwardVector, MySector.MainCamera.UpVector, default(Vector3), default(Vector3), null, null, SpawningOptions.None, 0L);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply) && MyInput.Static.IsAnyShiftKeyPressed())
			{
				GC.Collect(2);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad5))
			{
				Thread.Sleep(250);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9))
			{
				MyEntity myEntity = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.Entity : null);
				myEntity?.PositionComp.SetPosition(myEntity.PositionComp.GetPosition() + myEntity.WorldMatrix.Forward * 5.0);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad4))
			{
				MyEntity myEntity2 = MySession.Static.ControlledEntity as MyEntity;
				if (myEntity2 != null && myEntity2.HasInventory)
				{
					MyFixedPoint amount = 20000;
					MyObjectBuilder_Ore objectBuilder = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
					myEntity2.GetInventory().AddItems(amount, objectBuilder);
				}
				result = true;
			}
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Delete))
			{
				Enumerable.Count<MyFloatingObject>(Enumerable.OfType<MyFloatingObject>((IEnumerable)MyEntities.GetEntities()));
				foreach (MyFloatingObject item4 in Enumerable.OfType<MyFloatingObject>((IEnumerable)MyEntities.GetEntities()))
				{
					if (item4 == MySession.Static.ControlledEntity)
					{
						MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
					}
					item4.Close();
				}
				result = true;
			}
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Decimal))
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					if (entity != MySession.Static.ControlledEntity && (MySession.Static.ControlledEntity == null || entity != MySession.Static.ControlledEntity.Entity.Parent) && entity != MyCubeBuilder.Static.FindClosestGrid())
					{
						entity.Close();
					}
				}
				result = true;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9) || MyInput.Static.IsNewKeyPressed(MyKeys.NumPad5))
			{
				MyPhysicsComponentBase physics = MySession.Static.ControlledEntity.Entity.GetTopMostParent().Physics;
				if (physics.RigidBody != null)
				{
					physics.RigidBody.ApplyLinearImpulse(physics.Entity.WorldMatrix.Forward * physics.Mass * 2.0);
				}
				result = true;
			}
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.OemComma))
			{
				MyFloatingObject[] array = Enumerable.ToArray<MyFloatingObject>(Enumerable.OfType<MyFloatingObject>((IEnumerable)MyEntities.GetEntities()));
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Close();
				}
			}
			return result;
		}
	}
}
