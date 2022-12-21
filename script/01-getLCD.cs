


int tick=0;

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

    Runtime.UpdateFrequency = UpdateFrequency.Update1;       // 1time/1tick
    // Runtime.UpdateFrequency = UpdateFrequency.Update10;      // 1time/10tick
    // Runtime.UpdateFrequency = UpdateFrequency.Update100;     // 1time/100tick

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



public void Main(string argument, UpdateType updateSource)
{
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.

    // 先获取一个TextPanel
    List<IMyTextPanel> panels=new List<IMyTextPanel>();
    GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(panels);
    Echo("LCD Count: "+panels.Count);
    if(panels.Count<1)return;
    // 取第一个
    IMyTextPanel panel=panels[0];
    panel.ContentType = ContentType.TEXT_AND_IMAGE;

    Echo("target LCD: "+panel.CustomName);
    // 开启LCD显示
    panel.ShowPublicTextOnScreen();
    // 输出字符
    panel.WritePublicText("hello my friends!");
    // 追加字符
    panel.WritePublicText(tick++.ToString(),true);
}





// 游戏中的编译器存在白名单，即用户只能使用特定的类与方法，白名单如下：

// Sandbox.ModAPI.Ingame;
// Sandbox.ModAPI.Interfaces;
// Sandbox.Common.ObjectBuilders;
// VRageMath;
// VRage;
