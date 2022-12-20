using System;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Debris;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.GameSystems.Conveyors
{
	[MyEntityType(typeof(MyObjectBuilder_ConveyorPacket), true)]
	public class MyConveyorPacket : MyEntity
	{
		private class Sandbox_Game_GameSystems_Conveyors_MyConveyorPacket_003C_003EActor : IActivator, IActivator<MyConveyorPacket>
		{
			private sealed override object CreateInstance()
			{
				return new MyConveyorPacket();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyConveyorPacket CreateInstance()
			{
				return new MyConveyorPacket();
			}

			MyConveyorPacket IActivator<MyConveyorPacket>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyPhysicalInventoryItem Item;

		public int LinePosition;

		private float m_segmentLength;

		private Base6Directions.Direction m_segmentDirection;

		public void Init(MyObjectBuilder_ConveyorPacket builder, MyEntity parent)
		{
			Item = new MyPhysicalInventoryItem(builder.Item);
			LinePosition = builder.LinePosition;
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(Item.Content);
			MyObjectBuilder_Ore myObjectBuilder_Ore = Item.Content as MyObjectBuilder_Ore;
			string model = physicalItemDefinition.Model;
			float num = 1f;
			if (myObjectBuilder_Ore != null)
			{
				foreach (MyVoxelMaterialDefinition voxelMaterialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
				{
					if (voxelMaterialDefinition.MinedOre == myObjectBuilder_Ore.SubtypeName)
					{
						model = MyDebris.GetRandomDebrisVoxel();
						num = (float)Math.Pow((float)Item.Amount * physicalItemDefinition.Volume / MyDebris.VoxelDebrisModelVolume, 0.33300000429153442);
						break;
					}
				}
			}
			if (num < 0.05f)
			{
				num = 0.05f;
			}
			else if (num > 1f)
			{
				num = 1f;
			}
			bool allocationSuspended = MyEntityIdentifier.AllocationSuspended;
			MyEntityIdentifier.AllocationSuspended = false;
			Init(null, model, parent, null);
			MyEntityIdentifier.AllocationSuspended = allocationSuspended;
			base.PositionComp.Scale = num;
			base.Save = false;
		}

		public void SetSegmentLength(float length)
		{
			m_segmentLength = length;
		}

		public void SetLocalPosition(Vector3I sectionStart, int sectionStartPosition, float cubeSize, Base6Directions.Direction forward, Base6Directions.Direction offset)
		{
			int num = LinePosition - sectionStartPosition;
			Matrix localMatrix = base.PositionComp.LocalMatrixRef;
			Vector3 vector = base.PositionComp.LocalMatrixRef.GetDirectionVector(forward) * num + base.PositionComp.LocalMatrixRef.GetDirectionVector(offset) * 0.1f;
			localMatrix.Translation = (sectionStart + vector / base.PositionComp.Scale.Value) * cubeSize;
			base.PositionComp.SetLocalMatrix(ref localMatrix);
			m_segmentDirection = forward;
		}

		public void MoveRelative(float linePositionFraction)
		{
			base.PrepareForDraw();
			Matrix localMatrix = base.PositionComp.LocalMatrixRef;
			localMatrix.Translation += base.PositionComp.LocalMatrixRef.GetDirectionVector(m_segmentDirection) * m_segmentLength * linePositionFraction / base.PositionComp.Scale.Value;
			base.PositionComp.SetLocalMatrix(ref localMatrix);
		}
	}
}
