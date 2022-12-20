












// public Dictionary<MyDefinitionBase, int> RemainingBlocksPerType { get; }
// public interface IMyProjector: IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider

public void Main() 
{
    IMyInteriorLight light;
    IMyProjector proj;

    light = GridTerminalSystem.GetBlockWithName("That Important Light") as IMyInteriorLight;
    if (light == null) 
    {
        Echo("Oh my! I couldn't find that block...");
        return;
    }

    light.Enabled = !light.Enabled;
    Echo("I have toggled the button!");
}