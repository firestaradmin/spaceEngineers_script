



public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update1;
}
public void Main(string argument, UpdateType updateSource)
{
    //获取当前玩家所在驾驶舱
    List<IMyShipController> cockpits=new List<IMyShipController>();
    GridTerminalSystem.GetBlocksOfType<IMyShipController>(cockpits,Controller=>Controller.IsUnderControl);
    if(cockpits.Count<1)return;
    IMyShipController cockpit=cockpits[0];
    Vector3D targetThrustPower=new Vector3D(cockpit.MoveIndicator.X*100f,cockpit.MoveIndicator.Y*100f,cockpit.MoveIndicator.Z*100f);
    //自动减速(简单)
    //转换为相对速度（坐标系转换）
    MatrixD refLookAtMatrix =MatrixD.CreateLookAt(Vector3D.Zero,cockpit.WorldMatrix.Forward,cockpit.WorldMatrix.Up);
    if(cockpit.DampenersOverride&&targetThrustPower==Vector3D.Zero)targetThrustPower=-Vector3D.TransformNormal(cockpit.GetShipVelocities().LinearVelocity,refLookAtMatrix);
    cockpit.IsMainCockpit=false;
    log("引擎控制接管中...\n"+targetThrustPower.ToString());
    //获取所有引擎
    List<IMyThrust> thrusts=new List<IMyThrust>();
    GridTerminalSystem.GetBlocksOfType<IMyThrust>(thrusts);
    if(thrusts.Count<1)return;
    //确定引擎方向
    foreach (var thrust in thrusts)
    {
        //判断引擎喷口对于驾驶舱的相对方向
        switch (cockpit.WorldMatrix.GetClosestDirection(thrust.WorldMatrix.Backward))
        {
            //设定推力
            case Base6Directions.Direction.Backward:
                thrust.CustomName="Backward";
                thrust.ThrustOverridePercentage=(targetThrustPower.Z>0)?(float)targetThrustPower.Z:0;
                thrust.Enabled=(targetThrustPower.Z>0)?true:false;
            break;
            case Base6Directions.Direction.Forward:
                thrust.CustomName="Forward";
                thrust.ThrustOverridePercentage=(targetThrustPower.Z<0)?(float)-targetThrustPower.Z:0;
                thrust.Enabled=(targetThrustPower.Z<0)?true:false;
            break;
            case Base6Directions.Direction.Right:
                thrust.CustomName="Right";
                thrust.ThrustOverridePercentage=(targetThrustPower.X>0)?(float)targetThrustPower.X:0;
                thrust.Enabled=(targetThrustPower.X>0)?true:false;
            break;
            case Base6Directions.Direction.Left:
                thrust.CustomName="Left";
                thrust.ThrustOverridePercentage=(targetThrustPower.X<0)?(float)-targetThrustPower.X:0;
                thrust.Enabled=(targetThrustPower.X<0)?true:false;
            break;
            case Base6Directions.Direction.Up:
                thrust.CustomName="Up";
                thrust.ThrustOverridePercentage=(targetThrustPower.Y>0)?(float)targetThrustPower.Y:0;
                thrust.Enabled=(targetThrustPower.Y>0)?true:false;
            break;
            case Base6Directions.Direction.Down:
                thrust.CustomName="Down";
                thrust.ThrustOverridePercentage=(targetThrustPower.Y<0)?(float)-targetThrustPower.Y:0;
                thrust.Enabled=(targetThrustPower.Y<0)?true:false;
            break;
        }
    }
}

private void log(string log){
    IMyTextPanel debug=(IMyTextPanel)GridTerminalSystem.GetBlockWithName("debug");
    if(debug==null)Echo(log);
    else {
        debug.ShowPublicTextOnScreen();
        debug.WritePublicText(log);
    }
}