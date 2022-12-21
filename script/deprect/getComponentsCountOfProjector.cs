弃用，因为数据更新太麻烦了，游戏更新数据可能会变



//========================================================================// 
// Dictionaries of component info.
//   Add new blocks, change component totals and volumes etc here.
//   Would appreciate if you submitted changes to the discussions thread
//     in the steam workshop so I can update it too!
//========================================================================// 
// Component Volumes
Dictionary<string, int> dVolumes = new Dictionary<string, int>()
{
    {"BulletproofGlass", 8},
    {"Computer", 1},
    {"Construction", 2},
    {"Detector", 6},
    {"Display", 6},
    {"Explosives", 2},
    {"GravityGenerator", 200},
    {"Girder", 2},
    {"InteriorPlate", 5},
    {"LargeTube", 38},
    {"Medical", 160},
    {"MetalGrid", 15},
    {"Motor", 8},
    {"PowerCell", 45},
    {"RadioCommunication", 140},
    {"Reactor", 8},
    {"SmallTube", 2},
    {"SolarCell", 50},
    {"SteelPlate", 3},
    {"Superconductor",8},
    {"Thrust", 10}
};

// 游戏中的编译器存在白名单，即用户只能使用特定的类与方法，白名单如下：

// Sandbox.ModAPI.Ingame;
// Sandbox.ModAPI.Interfaces;
// Sandbox.Common.ObjectBuilders;
// VRageMath;
// VRage;



//========================================================================// 
//Small ship blocks from projector
//========================================================================// 
Dictionary<string, Dictionary<string, int>> dSmallShipComponentPieces = new Dictionary<string, Dictionary<string, int>>()
{
    {"Advanced Rotor", new Dictionary<string, int>{
        {"SteelPlate",35},
        {"Construction",5},
        {"SmallTube",1},
        {"Motor",1},
        {"Computer",1},
        {"LargeTube",10}
    }}, //Includes Adv. rotor part
    {"Air Vent", new Dictionary<string, int>{{"SteelPlate",8},{"Construction",10},{"Motor",2},{"Computer",5}}},
    {"Antenna", new Dictionary<string, int>{
        {"RadioCommunication",4},
        {"Computer",1},
        {"Construction",2},
        {"SmallTube",1},
        {"SteelPlate",1}
    }},
    {"Armor blocks", new Dictionary<string, int>{{"SteelPlate",1}}},
    {"Artificial Mass", new Dictionary<string, int>{
        {"GravityGenerator",1},
        {"Computer",2},
        {"Construction",2},
        {"Superconductor",2},
        {"SteelPlate",3}
    }},
    {"Atmospheric Thruster", new Dictionary<string, int>{
        {"SteelPlate",3},
        {"Construction",22},
        {"LargeTube",1},
        {"MetalGrid",1},
        {"Motor", 18}
    }},
    {"Sci-Fi Atmospheric Thruster", new Dictionary<string, int>{
        {"SteelPlate",3},
        {"Construction",22},
        {"LargeTube",1},
        {"MetalGrid",1},
        {"Motor", 18}
    }},
    {"Battery", new Dictionary<string, int>{
        {"SteelPlate",25},
        {"Construction",5},
        {"PowerCell",20},
        {"Computer", 2}
    }},
    {"Beacon", new Dictionary<string, int> {
        {"RadioCommunication",4},
        {"Computer",1},
        {"SmallTube",1},
        {"Construction",1},
        {"SteelPlate",2}
    }},
    {"Blast doors", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Blast door corner", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Blast door corner inverted", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Blast door edge", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Button Panel", new Dictionary<string, int> {
        {"Computer", 1},
        {"Construction",2},
        {"InteriorPlate",2}
    }},
    {"Camera", new Dictionary<string, int>{{"Computer",3},{"SteelPlate",2}}},
    {"Cockpit", new Dictionary<string, int>{
        {"SteelPlate",30},
        {"Construction",20},
        {"Motor",1},
        {"Display",8},
        {"Computer",100},
        {"BulletproofGlass", 60}
    }},
    {"Industrial Cockpit", new Dictionary<string, int>{
        {"SmallTube",10},
        {"SteelPlate",20},
        {"Construction",30},
        {"MetalGrid", 15},
        {"Motor",2},
        {"Display",10},
        {"Computer",60},
        {"BulletproofGlass", 80}
    }},
    {"Collector", new Dictionary<string, int>{{"SteelPlate",35},{"Construction",35},{"SmallTube",12},{"Motor",8},{"Display",2},{"Computer",8}}},
    {"Connector", new Dictionary<string, int>{{"SteelPlate",21},{"Construction",12},{"SmallTube",6},{"Motor",6},{"Computer",6}}},
    {"Control Panel", new Dictionary<string, int>{
        {"Display",1},
        {"Computer",1},
        {"Construction",1},
        {"SteelPlate",1}
    }},
    {"Conveyor", new Dictionary<string, int> {{"InteriorPlate", 25}, {"Construction", 70}, {"SmallTube", 25}, {"Motor", 2}}},
    {"Conveyor Junction", new Dictionary<string, int> {{"InteriorPlate", 25}, {"Construction", 70}, {"SmallTube", 25}, {"Motor", 2}}},
    {"Conveyor Frame", new Dictionary<string, int> {{"SteelPlate", 4}, {"Construction", 12}, {"SmallTube", 5}, {"Motor", 2}}},
    {"Conveyor Sorter", new Dictionary<string, int> {{"Motor", 2}, {"Computer", 5}, {"SmallTube", 5}, {"Construction", 12}, {"InteriorPlate", 5}}},
    {"Conveyor Tube", new Dictionary<string, int> {{"SteelPlate", 8}, {"Construction", 30}, {"SmallTube", 10}, {"Motor", 6}}},
    {"Corner LCD Bottom", new Dictionary<string, int> {{"Display", 1}, {"Computer", 2}, {"Construction", 3}}},
    {"Corner LCD Top", new Dictionary<string, int> {{"Display", 1}, {"Computer", 2}, {"Construction", 3}}},
    {"Curved Conveyor Tube", new Dictionary<string, int> {{"SteelPlate", 7}, {"Construction", 30}, {"SmallTube", 10}, {"Motor", 6}}},
    {"Decoy", new Dictionary<string, int> {{"Construction", 1}, {"Computer", 1}, {"RadioCommunication", 1}, {"SmallTube", 2}, {"Girder", 1}}},
    {"Drill", new Dictionary<string, int> {{"SteelPlate", 32}, {"Construction", 8}, {"SmallTube", 8}, {"LargeTube", 4}, {"Motor", 1}, {"Computer", 1}}},
    {"Ejector", new Dictionary<string, int> {{"SteelPlate", 7}, {"Construction", 4}, {"SmallTube", 2}, {"Motor", 1}, {"Computer", 4}}},
    {"Fighter Cockpit", new Dictionary<string, int> {{"Construction", 20}, {"Motor", 1}, {"SteelPlate", 18}, {"InteriorPlate", 15}, {"Display", 4}, {"Computer", 20}, {"BulletproofGlass", 40}}},
    {"Gatling Gun", new Dictionary<string, int> {{"Construction", 1}, {"SmallTube", 3}, {"Motor", 1}, {"Computer", 1}, {"SteelPlate", 4}}},
    {"Gatling Turret", new Dictionary<string, int> {{"SteelPlate", 10}, {"Construction", 30}, {"MetalGrid", 5}, {"LargeTube", 1}, {"Motor", 4}, {"Computer", 10}}},
    {"Grinder", new Dictionary<string, int> {{"SteelPlate", 12}, {"Construction", 17}, {"SmallTube", 4}, {"LargeTube", 1}, {"Motor", 4}, {"Computer", 2}}},
    {"Gyroscope", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 5}, {"LargeTube", 1}, {"Motor", 2}, {"Computer", 3}}},
    {"Heavy Armor Block", new Dictionary<string, int> {{"SteelPlate", 3}, {"MetalGrid",2}}},
    {"Heavy Armor Corner", new Dictionary<string, int> {{"SteelPlate", 1}, {"MetalGrid",1}}},
    {"Heavy Armor Inv. Corner", new Dictionary<string, int> {{"SteelPlate", 2}, {"MetalGrid",1}}},
    {"Heavy Armor Slope", new Dictionary<string, int> {{"SteelPlate", 2}, {"MetalGrid",1}}},
    {"Hydrogen Tank Small", new Dictionary<string, int> {{"SteelPlate", 80}, {"LargeTube",40}, {"SmallTube",60}, {"Computer", 8}, {"Construction", 40}}},
    {"Hydrogen Thrusters", new Dictionary<string, int> {{"SteelPlate", 30}, {"Construction",30}, {"MetalGrid",22}, {"LargeTube",10}}},

    {"Ion Thrusters", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 5}, {"Thrust", 12}, {"SteelPlate", 5}}},
    {"Landing Gear", new Dictionary<string, int> {{"Construction", 1}, {"LargeTube", 1}, {"Motor", 1}, {"SteelPlate", 1}}},
    {"Large Atmospheric Thruster", new Dictionary<string, int> {{"SteelPlate", 20}, {"Construction", 30}, {"LargeTube",4}, {"MetalGrid", 8}, {"Motor", 90}}},
    {"Sci-Fi Large Atmospheric Thruster", new Dictionary<string, int> {{"SteelPlate", 20}, {"Construction", 30}, {"LargeTube",4}, {"MetalGrid", 8}, {"Motor", 90}}},
    {"Large Cargo Container", new Dictionary<string, int> {{"InteriorPlate", 75}, {"Construction", 25}, {"Computer", 6}, {"Motor", 8}, {"Display", 1}}},
    {"Large Hydrogen Thruster", new Dictionary<string, int> {{"SteelPlate", 30}, {"Construction",30}, {"MetalGrid",22}, {"LargeTube",10}}},
    {"Large Ion Thruster", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 5}, {"Thrust", 12}, {"SteelPlate", 5}}},
    {"Large Reactor", new Dictionary<string, int> {{"SteelPlate", 60}, {"Construction", 9}, {"MetalGrid", 9}, {"LargeTube", 3}, {"Reactor", 95}, {"Motor", 5}, {"Computer", 25}}},
    {"Laser Antenna", new Dictionary<string, int> {{"BulletproofGlass", 2}, {"Computer", 30}, {"RadioCommunication", 5}, {"Motor", 5}, {"Construction", 10}, {"SmallTube", 10}, {"SteelPlate", 10},{"Superconductor",10}}},
    {"LCD Panel", new Dictionary<string, int> {{"Construction", 4}, {"Computer", 4}, {"Display", 3}}},
    {"Light Armor Block", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Corner", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Corner 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Corner 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Slope", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Slope 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Slope 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Medium Cargo Container", new Dictionary<string, int> {{"InteriorPlate", 30}, {"Construction", 10}, {"Computer", 4}, {"Motor", 6}, {"Display", 1}}},
    {"Merge Block", new Dictionary<string, int> {{"Construction", 5}, {"Motor", 1}, {"SmallTube", 2}, {"Computer", 1}, {"SteelPlate", 4}}},
    {"Ore Detector", new Dictionary<string, int> {{"Construction", 2}, {"Motor", 1}, {"Computer", 1}, {"Detector", 1}, {"SteelPlate", 2}}},
    //{"Oxygen Generator", new Dictionary<string, int> {{"Computer", 3}, {"Motor", 1}, {"LargeTube", 2}, {"Construction", 8}, {"SteelPlate", 8}}},
    {"O2/H2 Generator", new Dictionary<string, int> {{"Computer", 3}, {"Motor", 1}, {"LargeTube", 2}, {"Construction", 8}, {"SteelPlate", 8}}},
    {"Oxygen Tank", new Dictionary<string, int> {{"Construction", 10}, {"Computer", 3}, {"SmallTube", 10}, {"LargeTube", 2}, {"SteelPlate", 14}}},
    {"Passenger Seat", new Dictionary<string, int> {{"InteriorPlate", 20}, {"Construction", 20}}},
    {"Piston", new Dictionary<string, int> {{"LargeTube", 2}, {"SteelPlate", 8}, {"Construction", 4}, {"SmallTube", 4}, {"Motor", 2}, {"Computer", 1}}}, //Includes piston head since it's not available as a single block
    {"Programmable block", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 2}, {"Motor", 1}, {"Display", 1}, {"Computer", 2}, {"SteelPlate", 2}}},
    {"Projector", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 2}, {"Motor", 1}, {"Computer", 2}, {"SteelPlate", 2}}},
    {"Remote Control", new Dictionary<string, int> {{"Construction", 1}, {"Motor", 1}, {"Computer", 1}, {"InteriorPlate", 2}}},
    {"Rocket Launcher", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 4}, {"Motor", 1}, {"Computer", 1}, {"SteelPlate", 4}, {"MetalGrid", 1}}},
    {"Reloadable Rocket Launcher", new Dictionary<string, int> {{"SteelPlate", 8}, {"SmallTube", 50}, {"InteriorPlate", 50}, {"Construction", 24}, {"LargeTube", 8}, {"Motor", 4}, {"Computer", 2}}},
    {"Rotor", new Dictionary<string, int> {{"SteelPlate", 5}, {"Construction", 5}, {"SmallTube", 1}, {"Motor", 1}, {"Computer", 1}}},
    {"Rotor Part", new Dictionary<string, int> {{"SteelPlate", 12}, {"SmallTube", 6}}},
    {"Sensor", new Dictionary<string, int> {{"InteriorPlate", 5}, {"Construction", 5}, {"Computer", 6}, {"RadioCommunication", 6}, {"Detector", 6}, {"SteelPlate", 2}}},
    {"Small Atmospheric Thruster", new Dictionary<string, int> {{"SteelPlate", 3}, {"LargeTube", 1}, {"MetalGrid", 1}, {"Motor", 18}, {"Construction", 2}}},

    {"Small Battery", new Dictionary<string, int>{
        {"Computer",2},
        {"PowerCell",2},
        {"Construction",2},
        {"SteelPlate",4}
    }},
    {"Small Cargo Container", new Dictionary<string, int> {{"InteriorPlate", 3}, {"Construction", 1}, {"Computer", 2}, {"Motor", 2}, {"Display", 1}}},
    {"Small Conveyor", new Dictionary<string, int> {{"InteriorPlate", 4}, {"Construction", 4}, {"Motor", 1}}},
    {"Small Conveyor Sorter", new Dictionary<string, int> {{"Motor", 2}, {"Computer", 5}, {"SmallTube", 5}, {"Construction", 12}, {"InteriorPlate", 5}}},
    {"Small Conveyor Tube", new Dictionary<string, int> {{"InteriorPlate", 1}, {"Construction", 1}, {"Motor", 1}}},
    {"Small Curved Tube", new Dictionary<string, int> {{"InteriorPlate", 1}, {"Motor", 1}, {"Construction", 1}}},
    {"Small Curved Conveyor Tube", new Dictionary<string, int> {{"InteriorPlate", 1}, {"Motor", 1}, {"Construction", 1}}},
    {"Small Hydrogen Thruster", new Dictionary<string, int> {{"SteelPlate",7},{"Construction",15}, {"MetalGrid",4}, {"LargeTube",2}}},
    {"Small Ion Thruster", new Dictionary<string, int> {{"LargeTube", 1}, {"Thrust", 1}, {"Construction", 1}, {"SteelPlate", 1}}},
    {"Small Reactor", new Dictionary<string, int> {{"Construction", 1}, {"MetalGrid", 1}, {"LargeTube", 1}, {"Reactor", 1}, {"Motor", 1}, {"Computer", 10}, {"SteelPlate", 2}}},
    {"Solar Panel", new Dictionary<string, int> {{"MetalGrid", 2}, {"SmallTube", 1}, {"SteelPlate", 1}, {"Computer", 1}, {"SolarCell", 16}, {"Construction", 2}}},
    {"Sound Block", new Dictionary<string, int> {{"InteriorPlate", 4}, {"Construction", 6}, {"Computer", 3}}},
    {"Space Ball", new Dictionary<string, int> {{"GravityGenerator", 1}, {"Computer", 7}, {"Construction", 10}, {"SteelPlate", 70}}},
    {"Spotlight", new Dictionary<string, int> {{"SteelPlate", 1}, {"Construction", 1}, {"InteriorPlate", 1}}},
    {"Survival kit", new Dictionary<string, int>{
        {"Computer",5},
        {"Display",1},
        {"Motor",4},
        {"Medical",3},
        {"Construction",2},
        {"SteelPlate",6}
    }},
    {"Text panel", new Dictionary<string, int> {{"Construction", 4}, {"Computer", 4}, {"Display", 3}}},
    {"Timer Block", new Dictionary<string, int> {{"InteriorPlate", 2}, {"Construction", 3}, {"Computer", 1}}},
    {"Warhead", new Dictionary<string, int> {{"Girder", 1}, {"Construction", 1}, {"SmallTube", 2}, {"Computer", 1}, {"Explosives", 2}}},
    {"Welder", new Dictionary<string, int> {{"SteelPlate", 12}, {"Construction", 17}, {"SmallTube", 6}, {"LargeTube", 1}, {"Motor", 2}, {"Computer", 2}}},
    {"Wheel 1x1", new Dictionary<string, int> {{"MetalGrid", 2}, {"Construction", 5}, {"LargeTube", 1}, {"SteelPlate", 2}}},
    {"Wheel 3x3", new Dictionary<string, int> {{"SteelPlate", 4}, {"MetalGrid", 3}, {"Construction", 15}, {"LargeTube", 2}}},
    {"Wheel 5x5", new Dictionary<string, int> {{"SteelPlate", 6}, {"MetalGrid", 6}, {"Construction", 30}, {"LargeTube", 3}}},
    {"Wheel Suspension 1x1", new Dictionary<string, int> {{"SteelPlate", 8}, {"Construction", 7}, {"SmallTube", 2}, {"Motor", 1}}},
    {"Wheel Suspension 3x3", new Dictionary<string, int> {{"SteelPlate", 8}, {"Construction", 7}, {"SmallTube", 2}, {"Motor", 1}}},
    {"Wheel Suspension 5x5", new Dictionary<string, int> {{"SteelPlate", 16}, {"Construction", 12}, {"SmallTube", 4}, {"Motor", 2}}},
    {"Wide LCD panel", new Dictionary<string, int> {{"Construction", 8}, {"Computer", 8}, {"Display", 6}}},
};
//========================================================================// 
//Large ship blocks from projector
//Projector doesn't differentiate between armor block, heavy armor and interior wall...
//========================================================================// 
Dictionary<string, Dictionary<string, int>> dLargeShipComponentPieces = new Dictionary<string, Dictionary<string, int>>()
{
    {"Basic Assembler", new Dictionary<string, int>{
        {"Computer",80},
        {"Display",4},
        {"Motor",10},
        {"Construction",40},
        {"SteelPlate",80}
    }},
    {"Basic Refinery", new Dictionary<string, int>{
        {"Computer",10},
        {"Motor",10},
        {"Construction",20},
        {"SteelPlate",120}
    }},
    {"Ladder", new Dictionary<string, int>{
        {"SmallTube",10},
        {"Construction",20},
        {"InteriorPlate",10}
    }},
    {"Survival kit", new Dictionary<string, int>{
        {"Computer",5},
        {"Display",1},
        {"Motor",4},
        {"Medical",3},
        {"Construction",2},
        {"SteelPlate",30}
    }},
    {"Wind Turbine", new Dictionary<string, int>{
        {"Computer",2},
        {"Girder", 24},
        {"Construction",20},
        {"Motor",8},
        {"InteriorPlate",40}
    }},
    {"Advanced Rotor", new Dictionary<string, int>{{"Computer",2},{"Motor",4},{"LargeTube",14},{"Construction",10},{"SteelPlate",45}}},//Includes Adv. rotor part
    {"Air Vent", new Dictionary<string, int>{{"Computer",5},{"Motor",10},{"Construction",20},{"SteelPlate",45}}},
    {"Airtight Hangar Door", new Dictionary<string, int>{{"Computer",2},{"Motor",16},{"SmallTube",40},{"Construction",40},{"SteelPlate",350}}},
    {"Antenna", new Dictionary<string, int>{{"RadioCommunication",40},{"Computer",8},{"Construction",30},{"SmallTube",60},{"LargeTube",40},{"SteelPlate",80}}},
    //{"Arc furnace", new Dictionary<string, int> {{"SteelPlate", 120}, {"Construction", 5}, {"LargeTube", 2}, {"Motor", 4}, {"Computer", 5}}},
    {"Armor blocks", new Dictionary<string, int>{{"SteelPlate",25}}},
    {"Artificial Mass", new Dictionary<string, int>{{"GravityGenerator",9},{"Computer",20},{"Construction",30},{"Superconductor",20},{"SteelPlate",90}}},
    {"Assembler", new Dictionary<string, int>{{"Computer",160},{"MetalGrid",10},{"Display",10},{"Motor",20},{"Construction",80},{"SteelPlate",140}}},
    {"Atmospheric Thrusters", new Dictionary<string, int>{{"Motor",110},{"MetalGrid",10},{"LargeTube",8},{"Construction",50},{"SteelPlate",35}}},
    {"Battery", new Dictionary<string, int>{{"Computer",25},{"PowerCell",80},{"Construction",30},{"SteelPlate",80}}},
    {"Beacon", new Dictionary<string, int>{{"RadioCommunication",40},{"Computer",10},{"LargeTube",20},{"Construction",30},{"SteelPlate",80}}},
    {"Blast doors", new Dictionary<string, int>{{"SteelPlate",140}}},
    {"Blast door corner", new Dictionary<string, int>{{"SteelPlate",120}}},
    {"Blast door corner inverted", new Dictionary<string, int>{{"SteelPlate",135}}},
    {"Blast door edge", new Dictionary<string, int>{{"SteelPlate",130}}},
    {"Button Panel", new Dictionary<string, int> {{"Computer",20},{"Construction",20},{"InteriorPlate",10}}},
    {"Camera", new Dictionary<string, int>{{"Computer",3},{"SteelPlate",2}}},
    {"Cockpit", new Dictionary<string, int>{{"BulletproofGlass",60},{"Computer",100},{"Display",8},{"Motor",1},{"Construction",20},{"InteriorPlate",30}}},
    {"Collector", new Dictionary<string, int>{{"Computer",10},{"Display",4},{"Motor",8},{"SmallTube",12},{"Construction",50},{"SteelPlate",45}}},
    {"Connector", new Dictionary<string, int>{{"Computer",20},{"Motor",8},{"SmallTube",12},{"Construction",40},{"SteelPlate",150}}},
    {"Control Panel", new Dictionary<string, int>{{"Display",1},{"Computer",1},{"Construction",1},{"SteelPlate",1}}},
    {"Control Station", new Dictionary<string, int>{{"Display",10},{"Computer",100},{"Motor",2},{"Construction",20},{"InteriorPlate",20}}},
    //{"Conveyor", new Dictionary<string, int> {{"InteriorPlate", 50}, {"Construction", 120}, {"SmallTube", 50}, {"Motor", 2}}},
    {"Conveyor Junction", new Dictionary<string, int>{{"InteriorPlate",20},{"Construction",30},{"SmallTube",20},{"Motor",6}}},
    {"Conveyor Sorter", new Dictionary<string, int>{{"Motor",2},{"Computer",20},{"SmallTube",50},{"Construction",120},{"InteriorPlate",50}}},
    {"Conveyor Tube", new Dictionary<string, int>{{"Motor",6},{"SmallTube",12},{"Construction",20},{"InteriorPlate",14}}},
    {"Corner LCD Bottom", new Dictionary<string, int>{{"Display",1},{"Computer",3},{"Construction",5}}},
    {"Corner LCD Top", new Dictionary<string, int>{{"Display",1},{"Computer",3},{"Construction",5}}},
    {"Corner Light", new Dictionary<string, int>{
        {"Construction",3}
    }},
    {"Corner Light - Double", new Dictionary<string, int>{
        {"Construction",6}
    }},

    {"Cover Walls", new Dictionary<string, int>{{"Construction",10},{"SteelPlate",4}}},
    {"Cryo Chamber", new Dictionary<string, int>{{"BulletproofGlass",10},{"Computer",30},{"Display",8},{"Motor",8},{"Construction",20},{"InteriorPlate",40}}},
    {"Curved Conveyor Tube", new Dictionary<string, int> {{"InteriorPlate",10},{"Construction",20},{"SmallTube",12},{"Motor",6}}},
    {"Door", new Dictionary<string, int>{{"SteelPlate",8},{"Computer",2},{"Display",1},{"Motor",2},{"SmallTube",4},{"Construction",40},{"InteriorPlate",10}}},
    {"Decoy", new Dictionary<string, int>{{"LargeTube",2},{"RadioCommunication",1},{"Computer",10},{"Construction",10},{"SteelPlate",30}}},
    {"Diagonal Window", new Dictionary<string, int>{{"InteriorPlate",16},{"Construction",12},{"SmallTube",6}}},
    {"Drill", new Dictionary<string, int>{{"SteelPlate",300},{"Construction",40},{"LargeTube",12},{"Motor",5},{"Computer",5}}},
    //{"Effectiveness Module", new Dictionary<string, int> {{"Motor", 5}, {"MetalGrid", 10}, {"SmallTube", 15}, {"Construction", 50}, {"SteelPlate", 100}}},
    {"Flight Seat", new Dictionary<string, int>{{"InteriorPlate",20},{"Construction",20},{"Motor",2},{"Computer",100},{"Display",4}}},
    //{"Full Cover Wall", new Dictionary<string, int> {{"SteelPlate", 4}, {"Construction", 10}}},
    {"Gatling Turret", new Dictionary<string, int>{{"SteelPlate",20},{"Construction",30},{"MetalGrid",15},{"SmallTube",6},{"Motor",8},{"Computer",10}}},
    {"Gravity Generator", new Dictionary<string, int>{{"SteelPlate",150},{"GravityGenerator",6},{"Construction",60},{"LargeTube",4},{"Motor",6},{"Computer",40}}},
    {"Grinder", new Dictionary<string, int>{{"SteelPlate",20},{"Construction",30},{"LargeTube",1},{"Motor",4},{"Computer",2}}},
    {"Gyroscope", new Dictionary<string, int>{{"SteelPlate",600},{"Construction",40},{"LargeTube",4},{"MetalGrid",50},{"Motor",4},{"Computer",5}}},
    {"Half Cover Wall", new Dictionary<string, int>{{"SteelPlate",2},{"Construction",6}}},
    //{"Heavy Armor Block", new Dictionary<string, int> {{"SteelPlate", 150}, {"MetalGrid",50}}},
    //{"Heavy Armor Corner", new Dictionary<string, int> {{"SteelPlate", 25}, {"MetalGrid",10}}},
    //{"Heavy Armor Inv. Corner", new Dictionary<string, int> {{"SteelPlate", 125}, {"MetalGrid",50}}},
    //{"Heavy Armor Slope", new Dictionary<string, int> {{"SteelPlate", 75}, {"MetalGrid",25}}},
    {"Hydrogen Tank", new Dictionary<string, int>{{"SteelPlate",280},{"LargeTube",80},{"SmallTube",60},{"Computer",8},{"Construction",40}}},
    {"Hydrogen Thrusters", new Dictionary<string,int>{{"SteelPlate",25},{"Construction",60},{"MetalGrid",40},{"LargeTube",8}}},
    {"Ion Thrusters", new Dictionary<string, int>{{"SteelPlate",25},{"Construction",60},{"LargeTube",8},{"Thrust",80}}},
    {"Interior Light", new Dictionary<string, int> {{"Construction", 2}}},
    {"Interior Pillar", new Dictionary<string, int>{{"InteriorPlate",25},{"Construction",10},{"SmallTube", 4}}},
    {"Interior Turret", new Dictionary<string, int>{{"InteriorPlate",6},{"Construction",20},{"SmallTube",1},{"Motor",2},{"Computer",5},{"SteelPlate",4}}},
    {"Jump Drive", new Dictionary<string, int>{{"SteelPlate",60},{"MetalGrid",50},{"GravityGenerator",20},{"Detector",20},{"PowerCell",120},{"Superconductor",1000},{"Computer",300},{"Construction",40}}},
    {"Landing Gear", new Dictionary<string, int>{{"SteelPlate",150},{"Construction",20},{"Motor", 6}}},
    {"Large Atmospheric Thruster", new Dictionary<string, int>{{"SteelPlate",230},{"Construction",60},{"LargeTube",50},{"MetalGrid",40},{"Motor",1100}}},
    {"Large Cargo Container", new Dictionary<string, int>{{"InteriorPlate",360},{"Construction",80},{"MetalGrid",24},{"SmallTube",60},{"Motor",20},{"Display",1},{"Computer",8}}},
    {"Large Hydrogen Thruster", new Dictionary<string,int>{{"SteelPlate",150},{"Construction",180},{"MetalGrid",250},{"LargeTube",40}}},
    {"Large Ion Thruster", new Dictionary<string, int>{{"SteelPlate",150},{"Construction",100},{"LargeTube",40},{"Thrust",960}}},
    {"Large Reactor", new Dictionary<string, int>{{"SteelPlate",1000},{"Construction",70},{"MetalGrid", 40},{"LargeTube",40},{"Reactor",2000},{"Motor",20},{"Computer",75},{"Superconductor",100}}},
    {"Laser Antenna", new Dictionary<string, int>{{"BulletproofGlass",4},{"Computer",50},{"Superconductor",100},{"RadioCommunication",20},{"Detector",30},{"Motor",16},{"Construction",40},{"SteelPlate",50}}},
    {"LCD Panel", new Dictionary<string, int>{{"BulletproofGlass",6},{"Display",10},{"Computer",6},{"Construction",6},{"InteriorPlate",1}}},
    {"Light Armor Block", new Dictionary<string, int> {{"SteelPlate", 25}}},
    {"Light Armor Corner", new Dictionary<string, int> {{"SteelPlate", 4}}},
    {"Light Armor Corner 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 10}}},
    {"Light Armor Corner 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 4}}},
    {"Light Armor Slope", new Dictionary<string, int> {{"SteelPlate", 13}}},
    {"Light Armor Slope 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 19}}},
    {"Light Armor Slope 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 4}}},
    {"Medical Room", new Dictionary<string, int>{{"InteriorPlate",240},{"Construction",80},{"MetalGrid",60},{"SmallTube",20},{"LargeTube",5},{"Display",10},{"Computer",10},{"Medical",15}}},
    {"Merge Block", new Dictionary<string, int>{{"SteelPlate",12},{"Construction",15},{"Motor",2},{"LargeTube",6},{"Computer",2}}},
    {"Missile Turret", new Dictionary<string, int>{{"SteelPlate",20},{"Construction",40},{"MetalGrid",5},{"LargeTube",6},{"Motor",16},{"Computer",12}}},
    {"Ore Detector", new Dictionary<string, int>{{"SteelPlate",50},{"Construction",40},{"Motor",5},{"Computer",25},{"Detector",20}}},
    {"Oxygen Farm", new Dictionary<string, int>{{"Computer",20},{"Construction",20},{"SmallTube",10},{"LargeTube",20},{"BulletproofGlass",100},{"SteelPlate", 40}}},
    //{"Oxygen Generator", new Dictionary<string, int> {{"Computer", 5}, {"Motor", 4}, {"LargeTube", 2}, {"Construction", 5}, {"SteelPlate", 120}}},
    {"O2/H2 Generator", new Dictionary<string, int>{{"Computer",5},{"Motor",4},{"LargeTube",2},{"Construction",5},{"SteelPlate",120}}},
    {"Oxygen Tank", new Dictionary<string, int>{{"Construction",40},{"Computer",8},{"SmallTube",60},{"LargeTube",40},{"SteelPlate",80}}},
    {"Passage", new Dictionary<string, int>{{"InteriorPlate",74},{"Construction",20},{"SmallTube",48}}},
    {"Passenger Seat", new Dictionary<string, int>{{"InteriorPlate",20},{"Construction",20}}},
    {"Piston", new Dictionary<string, int>{{"SteelPlate",25},{"Construction",10},{"LargeTube",14},{"Motor",4},{"Computer",2}}}, //Includes piston head
    {"Power Efficiency Module", new Dictionary<string, int>{{"Motor",4},{"PowerCell",20},{"SmallTube",20},{"Construction",40},{"SteelPlate",100}}},
    //{"Productivity Module", new Dictionary<string, int> {{"Motor", 2}, {"LargeTube", 10}, {"SmallTube", 20}, {"Construction", 40}, {"SteelPlate", 100}}},
    {"Programmable block", new Dictionary<string, int>{{"SteelPlate",21},{"Construction",4},{"LargeTube",2},{"Motor",1},{"Display",1},{"Computer",2}}},
    {"Projector", new Dictionary<string, int>{{"SteelPlate",21},{"Construction",4},{"LargeTube",2},{"Motor",1},{"Computer",2}}},
    {"Ramp", new Dictionary<string, int>{{"InteriorPlate",70},{"Construction",16}}},
    {"Refinery", new Dictionary<string, int>{{"SteelPlate",1200},{"Construction",40},{"LargeTube",20},{"Motor",16},{"MetalGrid",20},{"Computer",20}}},
    {"Remote Control", new Dictionary<string, int>{{"InteriorPlate",10},{"Construction",10},{"Motor",1},{"Computer",15}}},
    {"Rocket Launcher", new Dictionary<string, int>{{"SteelPlate",35},{"Construction",8},{"MetalGrid",30},{"LargeTube",25},{"Motor",6},{"Computer",4}}},
    {"Rotor", new Dictionary<string, int>{{"SteelPlate",15},{"Construction",10},{"LargeTube",4},{"Motor",4},{"Computer",2}}}, // does not include rotor part as it's added via addcomops
    {"Rotor Part", new Dictionary<string, int>{{"SteelPlate",30},{"LargeTube",6}}},
    {"Sensor", new Dictionary<string, int>{{"InteriorPlate",5},{"Construction",5},{"Computer",6},{"RadioCommunication",4},{"Detector",6},{"SteelPlate",2}}},
    {"Sliding Door", new Dictionary<string, int>{
        {"BulletproofGlass",15},
        {"Computer",2},
        {"Display",1},
        {"Motor",4},
        {"SmallTube",4},
        {"Construction",40},
        {"SteelPlate",20}
    }},
    {"Small Cargo Container", new Dictionary<string, int> {
        {"InteriorPlate",40},
        {"Construction", 40},
        {"MetalGrid",4},
        {"SmallTube",20},
        {"Motor",4},
        {"Display",1},
        {"Computer", 2}
    }},
    //{"Small Atmospheric Thruster", new Dictionary<string, int>{{"SteelPlate",35},{"Construction", 50}, {"LargeTube",8}, {"MetalGrid", 10}, {"Motor", 113}}},
    //{"Small Hydrogen Thruster", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 60}, {"LargeTube", 8}, {"MetalGrid",40}}},
    //{"Small Ion Thruster", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 60}, {"LargeTube", 8}, {"Thrust", 80}}},
    {"Small Reactor", new Dictionary<string, int> {
        {"SteelPlate",80},
        {"Construction",40},
        {"MetalGrid",4},
        {"LargeTube",8},
        {"Reactor",100},
        {"Motor",6},
        {"Computer",25}
    }},
    {"Solar Panel", new Dictionary<string, int> {
        {"BulletproofGlass", 4},
        {"SolarCell",32},
        {"Computer",4},
        {"Girder",12},
        {"Construction",14},
        {"SteelPlate",4}
    }},
    {"Sound Block", new Dictionary<string, int>{
        {"Computer",3},
        {"Construction",6},
        {"InteriorPlate",4},
    }},
    {"Space Ball", new Dictionary<string, int>{
        {"GravityGenerator",3},
        {"Computer",20},
        {"Construction",30},
        {"SteelPlate",225}
    }},
    {"Speed Module", new Dictionary<string, int>{
        {"Motor",4},
        {"Computer",60},
        {"SmallTube",20},
        {"Construction",40},
        {"SteelPlate",100}
    }},
    {"Spherical Gravity Generator", new Dictionary<string, int>{
        {"SteelPlate",150},
        {"GravityGenerator",6},
        {"Construction",60},
        {"LargeTube",4},
        {"Motor",6},
        {"Computer", 40}
    }},
    {"Spotlight", new Dictionary<string, int> {
        {"BulletproofGlass",4},
        {"Construction",15},
        {"InteriorPlate",20},
        {"LargeTube",2},
        {"SteelPlate",8}
    }},
    {"Stairs", new Dictionary<string, int>{
        {"Construction",30},
        {"InteriorPlate",50}
    }},
    //{"Steel Catwalk", new Dictionary<string, int>{{"Construction",5},{"MetalGrid",25},{"SmallTube",20},{"InteriorPlate", 2}}},
    {"Steel Catwalk Corner", new Dictionary<string, int>{
        {"SmallTube",25},
        {"Construction",7},
        {"InteriorPlate",32}
    }},
    {"Steel Catwalk Plate", new Dictionary<string, int>{
        {"SmallTube",25},
        {"Construction",7},
        {"InteriorPlate",23}
    }},
    {"Steel Catwalk Two Sides", new Dictionary<string, int>{
        {"SmallTube",25},
        {"Construction",7},
        {"InteriorPlate",32}
    }},
    {"Steel Catwalks", new Dictionary<string, int>{
        {"SmallTube",20},
        {"Construction",5},
        {"InteriorPlate",27}
    }},
    {"Timer Block", new Dictionary<string, int>{{"InteriorPlate",6},{"Construction",30},{"Computer",5}}},
    {"Text panel", new Dictionary<string, int>{
        {"BulletproofGlass",2},
        {"Display", 10},
        {"Computer",6},
        {"Construction",6},
        {"InteriorPlate",1}
    }},
    {"Warhead", new Dictionary<string, int>{
        {"SteelPlate",20},
        {"Girder",24},
        {"Construction",12},
        {"SmallTube",12},
        {"Computer",2},
        {"Explosives", 6}
    }},
    {"Welder", new Dictionary<string, int>{
        {"SteelPlate",20},
        {"Construction",30},
        {"LargeTube",1},
        {"Motor",2},
        {"Computer",2}
    }},
    {"Wheel 1x1", new Dictionary<string, int>{
        {"SteelPlate",8},
        {"Construction",20},
        {"LargeTube",4}
    }},
    {"Wheel 3x3", new Dictionary<string, int>{
        {"SteelPlate",12},
        {"Construction",25},
        {"LargeTube", 6}
    }},
    {"Wheel 5x5", new Dictionary<string, int> {
        {"SteelPlate",16},
        {"Construction",30},
        {"LargeTube",8}
    }},
    //{"Wheel Suspension 3x3", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 15}, {"LargeTube", 6}, {"SmallTube", 12}, {"Motor", 6}}},
    {"Wheel Suspension 1x1 Left", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 1x1 Right", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 3x3 Left", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 3x3 Right", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 5x5 Left", new Dictionary<string, int>{
        {"Motor",20},
        {"SmallTube",30},
        {"LargeTube",20},
        {"Construction",40},
        {"SteelPlate",70}
    }},
    {"Wheel Suspension 5x5 Right", new Dictionary<string, int>{
        {"Motor",20},
        {"SmallTube",30},
        {"LargeTube",20},
        {"Construction",40},
        {"SteelPlate",70}
    }},
    //{"Wheel Suspension 5x5", new Dictionary<string, int> {{"SteelPlate",70},{"Construction",40},{"LargeTube",20},{"SmallTube",30},{"Motor",20}}},
    {"Wide LCD panel", new Dictionary<string, int>{
        {"BulletproofGlass",12},
        {"Display", 20},
        {"Computer",12},
        {"Construction",12},
        {"InteriorPlate",2}
    }},
    {"Window 1x2 Slope", new Dictionary<string, int> {{"Girder", 16}, {"BulletproofGlass", 55}}},
    {"Window 1x2 Inv.", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 40}}},
    {"Window 1x2 Face", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 40}}},
    {"Window 1x2 Side Left", new Dictionary<string, int> {{"Girder", 13}, {"BulletproofGlass", 26}}},
    {"Window 1x2 Side Right", new Dictionary<string, int> {{"Girder", 13}, {"BulletproofGlass", 26}}},
    {"Window 1x1 Slope", new Dictionary<string, int> {{"Girder", 12}, {"BulletproofGlass", 35}}},
    {"Window 1x1 Face", new Dictionary<string, int> {{"Girder", 11}, {"BulletproofGlass", 24}}},
    {"Window 1x1 Side", new Dictionary<string, int> {{"Girder", 9}, {"BulletproofGlass", 17}}},
    {"Window 1x1 Inv.", new Dictionary<string, int> {{"Girder", 11}, {"BulletproofGlass", 24}}},
    {"Window 1x1 Flat", new Dictionary<string, int> {{"Girder", 10}, {"BulletproofGlass", 25}}},
    {"Window 1x1 Flat Inv.", new Dictionary<string, int> {{"Girder", 10}, {"BulletproofGlass", 25}}},
    {"Window 1x2 Flat", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 50}}},
    {"Window 1x2 Flat Inv.", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 50}}},
    {"Window 2x3 Flat", new Dictionary<string, int> {{"Girder", 25}, {"BulletproofGlass", 140}}},
    {"Window 2x3 Flat Inv.", new Dictionary<string, int> {{"Girder", 25}, {"BulletproofGlass", 140}}},
    {"Window 3x3 Flat", new Dictionary<string, int> {{"Girder", 40}, {"BulletproofGlass", 196}}},
    {"Window 3x3 Flat Inv.", new Dictionary<string, int> {{"Girder", 40}, {"BulletproofGlass", 196}}},
    {"Vertical Window", new Dictionary<string, int> {{"InteriorPlate", 12}, {"Construction", 8}, {"SmallTube", 4}}},
    {"Yield Module", new Dictionary<string, int>{
        {"Motor",4},
        {"Superconductor",20},
        {"SmallTube",15},
        {"Construction",50},
        {"SteelPlate", 100}
    }},

};









IMyTextSurface lcd_show;
public void textAppend(string argument)
{
    lcd_show.WriteText(argument, true);
}
public void textClear()
{
    lcd_show.WriteText("", false);
}

// int tick=0;
public Program()
{
    // Runtime.UpdateFrequency = UpdateFrequency.Update1;
}

List<string> blocks = new List<string>();
// List<IMyTerminalBlock> AllStorageBlocks=new List<IMyTerminalBlock>();
// List<IMyTerminalBlock> ProjectorBlocks=new List<IMyTerminalBlock>();

Dictionary<string, int> dReqComponents = new Dictionary<string, int>();
Dictionary<string, int> dComponentsCount = new Dictionary<string, int>();

public void Main(string argument, UpdateType updateSource)
{
    //先获取一个TextPanel
    int mySurfaceCount = 0;
    mySurfaceCount = Me.SurfaceCount;
    Echo("All lcd cnt:"+Me.SurfaceCount);

    // IMyTextSurface GetSurface(int index)
    IMyTextSurface lcd0;
    if(Me.SurfaceCount < 0)
    {
        return;
    }


    lcd0 = Me.GetSurface(0);
    lcd_show = lcd0;
    Echo("lcd name:"+lcd0.Name);
    Echo("lcd Displayname:"+lcd0.DisplayName);
    textClear();
    textAppend("firestaradmin is very cool!\r\n");


    IMyProjector projector = (IMyProjector)GridTerminalSystem.GetBlockWithName("myProjector");
    textAppend("projector:\r\n");

    // int BuildableBlocksCount { get; }
    textAppend("BuildableBlocksCount:" + projector.BuildableBlocksCount.ToString() + "\r\n");
    textAppend("RemainingBlocks:" + projector.RemainingBlocks.ToString() + "\r\n");

    // set partListTable
    Dictionary<string, Dictionary<string, int>> partListDict = new Dictionary<string, Dictionary<string, int>>();
    if((projector as IMyCubeBlock).BlockDefinition.ToString().Split('/')[1].Contains("Large"))
        partListDict = dLargeShipComponentPieces; //partsDict
    else
        partListDict = dSmallShipComponentPieces;


    dReqComponents.Clear();


    //Get the blocks list from the projo detailed infos
    string[] strDetailedInfo = (projector as IMyTerminalBlock).DetailedInfo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    blocks = new List<string>(strDetailedInfo);

    string blockName = "";
    int amount = 0;

    Echo("== out info ==");
    // get all block build progress, index:3
    string buildProgress = blocks[3].Split(':')[1];
    Echo("buildProgress"+buildProgress);


    // get every blocks amount,index [5:]
    for(int i=5; i<blocks.Count; i++)
    {
        blockName = blocks[i].Split(':')[0];
        amount = Int32.Parse(blocks[i].Split(':')[1]);
        Echo(blockName+"->  "+amount);

        // add component to dReqComponents List
        if (!partListDict.ContainsKey(blockName))
            Echo(blockName +" is not in the dictionary, this block will not be calculated");
        else
        {
            // if blocks are a two piece set add the second set.
            switch (blockName)
            {
                case "Wheel Suspension 1x1":
                    AddCompos(ref dReqComponents, amount, partListDict["Wheel 1x1"]);
                    AddCompos(ref dReqComponents, amount, partListDict[blockName]);
                    break;
                case "Wheel Suspension 3x3":
                    AddCompos(ref dReqComponents, amount, partListDict["Wheel 3x3"]);
                    AddCompos(ref dReqComponents, amount, partListDict[blockName]);
                    break;
                case "Wheel Suspension 5x5":
                    AddCompos(ref dReqComponents, amount, partListDict["Wheel 5x5"]);
                    AddCompos(ref dReqComponents, amount, partListDict[blockName]);
                    break;
                case "Rotor":
                    AddCompos(ref dReqComponents, amount, partListDict["Rotor Part"]);
                    AddCompos(ref dReqComponents, amount, partListDict[blockName]);
                    break;
                default:
                    AddCompos(ref dReqComponents, amount, partListDict[blockName]);
                    break; 
            }
        }
    }
}


//========================================================================// 
// AddCompos
//    Add the amount of compos from a Dictionary to another and update the total volume
//========================================================================// 
void AddCompos(ref Dictionary<string, int> compos, int amount, Dictionary<string, int> toAdd)
{
    //For this block, get a list of compos and a list of their respective amount
    List<string> listKey = new List<string>(toAdd.Keys);
    List<int> listValue = new List<int>(toAdd.Values);
    //For each compos needed on a block
    for(int i=0; i<listKey.Count; i++)
    {
        if(compos.ContainsKey(listKey[i]))
            compos[listKey[i]] += amount * listValue[i];
        else
            compos.Add(listKey[i], amount*listValue[i]);
    }
}



void MakedComponentsCount()
{
    dComponentsCount.Clear();
    PopulateDictionary(dComponentsCount, "BulletproofGlass", 0);
    PopulateDictionary(dComponentsCount, "Computer", 0);
    PopulateDictionary(dComponentsCount, "Construction", 0);
    PopulateDictionary(dComponentsCount, "Detector", 0);
    PopulateDictionary(dComponentsCount, "Display", 0);
    PopulateDictionary(dComponentsCount, "Explosives", 0);
    PopulateDictionary(dComponentsCount, "GravityGenerator", 0);
    PopulateDictionary(dComponentsCount, "Girder", 0);
    PopulateDictionary(dComponentsCount, "InteriorPlate", 0);
    PopulateDictionary(dComponentsCount, "LargeTube", 0);
    PopulateDictionary(dComponentsCount, "Medical", 0);
    PopulateDictionary(dComponentsCount, "MetalGrid", 0);
    PopulateDictionary(dComponentsCount, "Motor", 0);
    PopulateDictionary(dComponentsCount, "PowerCell", 0);
    PopulateDictionary(dComponentsCount, "RadioCommunication", 0);
    PopulateDictionary(dComponentsCount, "Reactor", 0);
    PopulateDictionary(dComponentsCount, "SmallTube", 0);
    PopulateDictionary(dComponentsCount, "SolarCell", 0);
    PopulateDictionary(dComponentsCount, "SteelPlate", 0);
    PopulateDictionary(dComponentsCount, "Superconductor", 0);
    PopulateDictionary(dComponentsCount, "Thrust", 0);
}

void PopulateDictionary(Dictionary<string, int> dComp, string strComponent, int intAmount)
{
    dComp.Add(strComponent, intAmount);
}