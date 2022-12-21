





// 游戏中的编译器存在白名单，即用户只能使用特定的类与方法，白名单如下：

// Sandbox.ModAPI.Ingame;
// Sandbox.ModAPI.Interfaces;
// Sandbox.Common.ObjectBuilders;
// VRageMath;
// VRage;

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
    string txt="BuildableBlocksCount:" + projector.BuildableBlocksCount.ToString() + "\r\n";
    textAppend("BuildableBlocksCount:" + projector.BuildableBlocksCount.ToString() + "\r\n");

    // int RemainingBlocks { get; }
    textAppend("RemainingBlocks:" + projector.RemainingBlocks.ToString() + "\r\n");
    

    //Get the blocks list from the projo detailed infos
    string[] strDetailedInfo = (projector as IMyTerminalBlock).DetailedInfo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    List<string> blocks = new List<string>(strDetailedInfo);
    for(int i=1; i<blocks.Count; i++)
    {
        Echo(blocks[i]);
    }
}