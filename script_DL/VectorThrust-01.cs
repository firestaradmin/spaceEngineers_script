    List<IMyThrust> blocks = new List<IMyThrust>();
List<string> orientations = new List<string>();

List<IMyTerminalBlock> up = new List<IMyTerminalBlock>();  
List<IMyTerminalBlock> down = new List<IMyTerminalBlock>();   


List<IMyTerminalBlock> left = new List<IMyTerminalBlock>();   
List<IMyTerminalBlock> right = new List<IMyTerminalBlock>();   

List<IMyTerminalBlock> forward = new List<IMyTerminalBlock>();   
List<IMyTerminalBlock> backward = new List<IMyTerminalBlock>();   


int cu;
int cd;
int cl;
int cr;
int cf;
int cb;
IMyTextPanel LCDDebug;



public void Main(string argument) { 
up.Clear();
down.Clear();
left.Clear();
right.Clear();
forward.Clear();
backward.Clear();

GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks);

foreach( IMyTerminalBlock block in blocks) 
{ 


     if(block != null)
        {
            var reference = block as IMyCubeBlock;
            var  TBack = reference.Orientation.TransformDirection(Base6Directions.Direction.Backward); 
            string d = TBack.ToString();
            orientations.Add(d);
            var  Treference = reference as IMyTerminalBlock;


            if(d.ToLower().Contains("up"))
                  {
                   
                    up.Add(Treference);
                 }
             if(d.ToLower().Contains("down")) 
                  { 
                    
                    down.Add(Treference); 
                 }




             if(d.ToLower().Contains("left"))  
                  {  
                     
                    left.Add(Treference);  
                 }
             if(d.ToLower().Contains("right"))  
                  {  
                     
                    right.Add(Treference);  
                 }



             if(d.ToLower().Contains("forward"))  
                  {  
                     
                   forward.Add(Treference);  
                 }
             if(d.ToLower().Contains("backward"))  
                  {  
                     
                    backward.Add(Treference);  
                 }

                    ShowDebug();
           }
     }
}

public void ShowDebug()
{
LCDDebug = GridTerminalSystem.GetBlockWithName("LCDDebug") as IMyTextPanel;

if(LCDDebug != null)
    {
  
    LCDDebug.ShowPublicTextOnScreen();
    LCDDebug.WritePublicText("",true);
    LCDDebug.WritePublicText("Debug:   " + "\n" +  
    "UP:  " + "\n" + "\n" +   
    up.Count.ToString() + "\n" +      
    "DOWN:  " + "\n" + "\n" +                    
    down.Count.ToString() + "\n" +  

   "RIGHT:  " + "\n" + "\n" +   
    right.Count.ToString() + "\n" +  
   "LEFT:  " + "\n" + "\n" +   
    left.Count.ToString() + "\n" +  

   "BACKWARD:  " + "\n" + "\n" +   
    backward.Count.ToString() + "\n" +  
   "FOWARD:  " + "\n" + "\n" +   
    forward.Count.ToString() + "\n" ,

   false


);


    }


}
   

