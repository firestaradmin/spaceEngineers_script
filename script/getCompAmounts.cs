

// namespace LXG
// {

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
    Runtime.UpdateFrequency = UpdateFrequency.Update100;     // 1time/100tick
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


private List<KeyValuePair<string, int>> getAllComponentsInSys()
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


MyVerticalLcdArray lcd = new MyVerticalLcdArray();



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
    compList = getAllComponentsInSys();

    // print result to customData View
    List<KeyValuePair<string, int>> sNameCompList = new List<KeyValuePair<string, int>>();
    foreach (var component in compList){
        sNameCompList.Add(new KeyValuePair<string, int>(component.Key.Replace("MyObjectBuilder_BlueprintDefinition/", ""), component.Value));
    }

    string output = "";
    foreach (var component in sNameCompList){
        output += String.Format("{0,-23}", component.Key) + " [ " + component.Value.ToString() + " ]\n";
    }
    Me.CustomData = output;

    if(!lcd.initOK) {
        if(lcd.init(2,"LVLCD",1.5,GridTerminalSystem) == false){
            Echo(lcd.strError);
            return;
        }
    }
    lcd.clearAll();
    lcd.write("components Statistics: \n");
    lcd.write(output);



}





class MyVerticalLcdArray
{

    /* EXAMPLE
        MyVerticalLcdArray lcd = new MyVerticalLcdArray();
        if(!lcd.initOK) {
            if(lcd.init(2,"LVLCD",1.5,GridTerminalSystem) == false){
                Echo(lcd.strError);
                return;
            }
        }
        lcd.clearAll();
        lcd.write("components Statistics:\n");
        lcd.write(output);
    */
    public List<IMyTextPanel> panels = new List<IMyTextPanel>();
    public bool initOK = false;
    public string strError;
    IMyGridTerminalSystem _GridTerminalSystem = null;

    public int singleLCDMaxRow = 0;
    int _currentRow = 0;
    int _index = 0;

    public  MyVerticalLcdArray()
    {

    }

    public bool init(int LCDCnt, string nameExTag, double FontSize, IMyGridTerminalSystem _sys)
    {
        _GridTerminalSystem = _sys;
        IMyTextPanel panel;
        for (int i = 0; i < LCDCnt; i++)
        {
            panel = _GridTerminalSystem.GetBlockWithName(nameExTag+i) as IMyTextPanel;
            if(panel == null){
                strError = "error to init LCD, name: "+nameExTag+i;
                return false;
            }
            panels.Add(panel);
            panel.FontSize = (float)FontSize;
            panel.Font = "Monospace";
            panel.ContentType = ContentType.TEXT_AND_IMAGE;
            // panel.ShowPublicTextOnScreen();
            panel.WriteText("",false);


        }
        initOK = true;
        // Echo(panel.SurfaceSize.X+" | "+panel.SurfaceSize.Y);
        Vector2 singleC = panels[_index].MeasureStringInPixels(new StringBuilder("ABC", 50), panels[_index].Font, panels[_index].FontSize);
        // Echo(singleC.X+" | "+singleC.Y);
        singleLCDMaxRow = (int)panels[_index].SurfaceSize.Y / (int)singleC.Y;

        return true;
    }


    public void write_lcd(int index, string str)
    {
        if(!initOK) return;
        if(index >= panels.Count()){
            strError = "LCD index invaild!";
            return;
        }
        panels[index].WriteText(str,true);
    }

    public void write(string str)
    {
        if(!initOK) return;
        // int CRLFCount = System.Text.RegularExpressions.Regex.Matches(str, "\n").Count;
        if(str.Contains("\n")){
            string[] strLine = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            for (int i = 0; i < strLine.Count(); i++)
            {
                panels[_index].WriteText(strLine[i],true);
                if(i < strLine.Count() - 1){
                    _currentRow++;
                    panels[_index].WriteText("\n",true);
                    if(_currentRow >= singleLCDMaxRow){
                        _currentRow = 0;
                        _index++;
                        if(_index >= panels.Count()) _index = panels.Count()-1;
                    }
                }

            }

        }
        else{
            panels[_index].WriteText(str,true);
        }



    }

    public void clear_lcd(int index)
    {
        if(!initOK) return;
        if(index >= panels.Count()){
            strError = "LCD index invaild!";
            return;
        }
        panels[index].WriteText("",false);
    }
    public void clearAll()
    {
        if(!initOK) return;
        foreach(var p in panels){
            p.WriteText("",false);
        }
        _index = 0;
        _currentRow = 0;
    }


}
// } // end LXG namespace
