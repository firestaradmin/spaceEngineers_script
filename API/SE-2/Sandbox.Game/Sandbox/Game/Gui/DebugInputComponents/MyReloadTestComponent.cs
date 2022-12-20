using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using VRage.Game.Components;

namespace Sandbox.Game.Gui.DebugInputComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	internal class MyReloadTestComponent : MySessionComponentBase
	{
		public static bool Enabled;

		public override void UpdateAfterSimulation()
		{
			if (Enabled && MySandboxGame.IsGameReady && MySession.Static != null && MySession.Static.ElapsedPlayTime.TotalSeconds > 5.0)
			{
				GC.Collect(2, GCCollectionMode.Forced);
				MySandboxGame.Log.WriteLine(string.Format("RELOAD TEST, Game GC: {0} B", GC.GetTotalMemory(forceFullCollection: false).ToString("##,#")));
				MySandboxGame.Log.WriteLine(string.Format("RELOAD TEST, Game WS: {0} B", Process.GetCurrentProcess().get_PrivateMemorySize64().ToString("##,#")));
				MySessionLoader.UnloadAndExitToMenu();
			}
		}

		public static void DoReload()
		{
			GC.Collect(2, GCCollectionMode.Forced);
			MySandboxGame.Log.WriteLine(string.Format("RELOAD TEST, Menu GC: {0} B", GC.GetTotalMemory(forceFullCollection: false).ToString("##,#")));
			MySandboxGame.Log.WriteLine(string.Format("RELOAD TEST, Menu WS: {0} B", Process.GetCurrentProcess().get_PrivateMemorySize64().ToString("##,#")));
			Tuple<string, MyWorldInfo> tuple = Enumerable.FirstOrDefault<Tuple<string, MyWorldInfo>>((IEnumerable<Tuple<string, MyWorldInfo>>)Enumerable.OrderByDescending<Tuple<string, MyWorldInfo>, DateTime>((IEnumerable<Tuple<string, MyWorldInfo>>)MyLocalCache.GetAvailableWorldInfos(), (Func<Tuple<string, MyWorldInfo>, DateTime>)((Tuple<string, MyWorldInfo> s) => s.Item2.LastSaveTime)));
			if (tuple != null)
			{
				MySessionLoader.LoadSingleplayerSession(tuple.Item1);
			}
		}
	}
}
