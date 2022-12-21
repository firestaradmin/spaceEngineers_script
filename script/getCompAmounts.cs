


private void AddCountToDict<T>(Dictionary<T, int> dic, T key, int amount)
{
    if (dic.ContainsKey(key))
    {
        dic[key] += amount;
    }
    else
    {
        dic[key] = amount;
    }
}

private int GetCountFromDic<T>(Dictionary<T, int> dic, T key)
{
    if (dic.ContainsKey(key))
    {
        return dic[key];
    }
    return 0;
}


public Program()
{
    // The constructor, called only once every session and
    // always before any other method is called. Use it to
    // initialize your script. 
    //     
    // The constructor is optional and can be removed if not
    // needed.
    // 
    // It's recommended to set RuntimeInfo.UpdateFrequency 
    // here, which will allow your script to run itself without a 
    // timer block.
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}

private readonly bool inventoryFromSubgrids = false; // consider inventories on subgrids when computing available materials


private List<KeyValuePair<string, int>> getAllComponents()
{
    var cubeBlocks = new List<IMyCubeBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyCubeBlock>(cubeBlocks, block => block.CubeGrid == Me.CubeGrid || inventoryFromSubgrids);

    Dictionary<string, int> componentAmounts = new Dictionary<string, int>();
    foreach (var b in cubeBlocks)
    {
        if (b.HasInventory)
        {
            for (int i = 0; i < b.InventoryCount; i++)
            {
                var itemList = new List<MyInventoryItem>();
                b.GetInventory(i).GetItems(itemList);
                foreach (var item in itemList)
                {
                    if (item.Type.TypeId.Equals("MyObjectBuilder_Component"))
                    {
                        AddCountToDict(componentAmounts, item.Type.SubtypeId, item.Amount.ToIntSafe());
                    }
                }
            }
        }
    }
    return componentAmounts.ToList();

    // List<KeyValuePair<string, int>> ret = new List<KeyValuePair<string, int>>();
    // foreach (var comp in compList)
    // {
    //     string subTypeId = comp.Key.Replace("MyObjectBuilder_BlueprintDefinition/", "").Replace("Component", "");
    //     ret.Add(new KeyValuePair<string, int>(comp.Key, Math.Max(0, comp.Value - GetCountFromDic(componentAmounts, subTypeId))));
    // }
    // return ret;
}



public void Main(string argument, UpdateType updateSource)
{
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.

    List<KeyValuePair<string, int>> compList = new List<KeyValuePair<string, int>>();
    compList = getAllComponents();

    // print result to customData View
    string output = "";
    foreach (var component in compList)
        output += component.Key.Replace("MyObjectBuilder_BlueprintDefinition/", "") + " " + component.Value.ToString() + "\n";
    Me.CustomData = output;

}



