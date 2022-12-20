using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace World
{
	internal class MySpaceSessionCompatHelper : MySessionCompatHelper
	{
		public override void FixSessionObjectBuilders(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
			base.FixSessionObjectBuilders(checkpoint, sector);
			if (sector.SectorObjects == null)
			{
				return;
			}
			if (sector.AppVersion == 0)
			{
<<<<<<< HEAD
				HashSet<string> hashSet = new HashSet<string>();
				hashSet.Add("LargeBlockArmorBlock");
				hashSet.Add("LargeBlockArmorSlope");
				hashSet.Add("LargeBlockArmorCorner");
				hashSet.Add("LargeBlockArmorCornerInv");
				hashSet.Add("LargeRoundArmor_Slope");
				hashSet.Add("LargeRoundArmor_Corner");
				hashSet.Add("LargeRoundArmor_CornerInv");
				hashSet.Add("LargeHeavyBlockArmorBlock");
				hashSet.Add("LargeHeavyBlockArmorSlope");
				hashSet.Add("LargeHeavyBlockArmorCorner");
				hashSet.Add("LargeHeavyBlockArmorCornerInv");
				hashSet.Add("SmallBlockArmorBlock");
				hashSet.Add("SmallBlockArmorSlope");
				hashSet.Add("SmallBlockArmorCorner");
				hashSet.Add("SmallBlockArmorCornerInv");
				hashSet.Add("SmallHeavyBlockArmorBlock");
				hashSet.Add("SmallHeavyBlockArmorSlope");
				hashSet.Add("SmallHeavyBlockArmorCorner");
				hashSet.Add("SmallHeavyBlockArmorCornerInv");
				hashSet.Add("LargeBlockInteriorWall");
=======
				HashSet<string> val = new HashSet<string>();
				val.Add("LargeBlockArmorBlock");
				val.Add("LargeBlockArmorSlope");
				val.Add("LargeBlockArmorCorner");
				val.Add("LargeBlockArmorCornerInv");
				val.Add("LargeRoundArmor_Slope");
				val.Add("LargeRoundArmor_Corner");
				val.Add("LargeRoundArmor_CornerInv");
				val.Add("LargeHeavyBlockArmorBlock");
				val.Add("LargeHeavyBlockArmorSlope");
				val.Add("LargeHeavyBlockArmorCorner");
				val.Add("LargeHeavyBlockArmorCornerInv");
				val.Add("SmallBlockArmorBlock");
				val.Add("SmallBlockArmorSlope");
				val.Add("SmallBlockArmorCorner");
				val.Add("SmallBlockArmorCornerInv");
				val.Add("SmallHeavyBlockArmorBlock");
				val.Add("SmallHeavyBlockArmorSlope");
				val.Add("SmallHeavyBlockArmorCorner");
				val.Add("SmallHeavyBlockArmorCornerInv");
				val.Add("LargeBlockInteriorWall");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
				{
					MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = sectorObject as MyObjectBuilder_CubeGrid;
					if (myObjectBuilder_CubeGrid == null)
					{
						continue;
					}
					foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
					{
<<<<<<< HEAD
						if (cubeBlock.TypeId != typeof(MyObjectBuilder_CubeBlock) || !hashSet.Contains(cubeBlock.SubtypeName))
=======
						if (cubeBlock.TypeId != typeof(MyObjectBuilder_CubeBlock) || !val.Contains(cubeBlock.SubtypeName))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							cubeBlock.ColorMaskHSV = MyRenderComponentBase.OldGrayToHSV;
						}
					}
				}
			}
			if (sector.AppVersion <= 1100001)
			{
				CheckOxygenContainers(sector);
			}
			if (sector.AppVersion <= 1185000)
			{
				CheckPistonsMaxImpulse(sector);
			}
			if (sector.AppVersion <= 1195000)
			{
				CheckOldWaypoints(sector);
			}
		}

		private void CheckPistonsMaxImpulse(MyObjectBuilder_Sector sector)
		{
			foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
			{
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = sectorObject as MyObjectBuilder_CubeGrid;
				if (myObjectBuilder_CubeGrid == null)
				{
					continue;
				}
				foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
				{
					MyObjectBuilder_PistonBase myObjectBuilder_PistonBase = cubeBlock as MyObjectBuilder_PistonBase;
					if (myObjectBuilder_PistonBase != null)
					{
						if (!myObjectBuilder_PistonBase.MaxImpulseAxis.HasValue)
						{
							myObjectBuilder_PistonBase.MaxImpulseAxis = 3.40282347E+37f;
						}
						if (!myObjectBuilder_PistonBase.MaxImpulseNonAxis.HasValue)
						{
							myObjectBuilder_PistonBase.MaxImpulseNonAxis = 3.40282347E+37f;
						}
					}
				}
			}
		}

		private void CheckOxygenContainers(MyObjectBuilder_Sector sector)
		{
			foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
			{
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = sectorObject as MyObjectBuilder_CubeGrid;
				if (myObjectBuilder_CubeGrid != null)
				{
					foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
					{
						MyObjectBuilder_OxygenTank myObjectBuilder_OxygenTank = cubeBlock as MyObjectBuilder_OxygenTank;
						if (myObjectBuilder_OxygenTank != null)
						{
							CheckOxygenInventory(myObjectBuilder_OxygenTank.Inventory);
							continue;
						}
						MyObjectBuilder_OxygenGenerator myObjectBuilder_OxygenGenerator = cubeBlock as MyObjectBuilder_OxygenGenerator;
						if (myObjectBuilder_OxygenGenerator != null)
						{
							CheckOxygenInventory(myObjectBuilder_OxygenGenerator.Inventory);
						}
					}
				}
				MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = sectorObject as MyObjectBuilder_FloatingObject;
				if (myObjectBuilder_FloatingObject != null)
				{
					MyObjectBuilder_OxygenContainerObject myObjectBuilder_OxygenContainerObject = myObjectBuilder_FloatingObject.Item.PhysicalContent as MyObjectBuilder_OxygenContainerObject;
					if (myObjectBuilder_OxygenContainerObject != null)
					{
						FixOxygenContainer(myObjectBuilder_OxygenContainerObject);
					}
				}
			}
		}

		private void CheckOxygenInventory(MyObjectBuilder_Inventory inventory)
		{
			if (inventory == null)
			{
				return;
			}
			foreach (MyObjectBuilder_InventoryItem item in inventory.Items)
			{
				MyObjectBuilder_OxygenContainerObject myObjectBuilder_OxygenContainerObject = item.PhysicalContent as MyObjectBuilder_OxygenContainerObject;
				if (myObjectBuilder_OxygenContainerObject != null)
				{
					FixOxygenContainer(myObjectBuilder_OxygenContainerObject);
				}
			}
		}

		private void FixOxygenContainer(MyObjectBuilder_OxygenContainerObject oxygenContainer)
		{
			oxygenContainer.GasLevel = oxygenContainer.OxygenLevel;
		}

		private void CheckInventoryBagEntity(MyObjectBuilder_Sector sector)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < sector.SectorObjects.Count; i++)
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = sector.SectorObjects[i];
				if (myObjectBuilder_EntityBase is MyObjectBuilder_ReplicableEntity || myObjectBuilder_EntityBase is MyObjectBuilder_InventoryBagEntity)
				{
					MyObjectBuilder_EntityBase myObjectBuilder_EntityBase2 = ConvertInventoryBagToEntityBase(myObjectBuilder_EntityBase);
					if (myObjectBuilder_EntityBase2 != null)
					{
						sector.SectorObjects[i] = myObjectBuilder_EntityBase2;
					}
					else
					{
						list.Add(i);
					}
				}
			}
			for (int num = list.Count - 1; num >= 0; num--)
			{
				sector.SectorObjects.RemoveAtFast(list[num]);
			}
		}

		private void CheckOldWaypoints(MyObjectBuilder_Sector sector)
		{
			new List<int>();
			for (int i = 0; i < sector.SectorObjects.Count; i++)
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = sector.SectorObjects[i];
				if (myObjectBuilder_EntityBase.Name != null && myObjectBuilder_EntityBase.Name.StartsWith(MyVisualScriptManagerSessionComponent.WAYPOINT_NAME_PREFIX) && !(myObjectBuilder_EntityBase is MyObjectBuilder_Waypoint))
				{
					MyObjectBuilder_Waypoint myObjectBuilder_Waypoint = new MyObjectBuilder_Waypoint();
					myObjectBuilder_Waypoint.Name = myObjectBuilder_EntityBase.Name;
					myObjectBuilder_Waypoint.ComponentContainer = myObjectBuilder_EntityBase.ComponentContainer;
					myObjectBuilder_Waypoint.EntityId = myObjectBuilder_EntityBase.EntityId;
					myObjectBuilder_Waypoint.PositionAndOrientation = myObjectBuilder_EntityBase.PositionAndOrientation;
					myObjectBuilder_Waypoint.PersistentFlags = myObjectBuilder_EntityBase.PersistentFlags;
					sector.SectorObjects[i] = myObjectBuilder_Waypoint;
				}
			}
		}
	}
}
