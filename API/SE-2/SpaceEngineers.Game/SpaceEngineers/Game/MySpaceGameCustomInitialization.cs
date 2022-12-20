using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Sandbox;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Definitions.SafeZone;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Scripting;
using VRageMath;

namespace SpaceEngineers.Game
{
	public class MySpaceGameCustomInitialization : MySandboxGame.IGameCustomInitialization
	{
		public void InitIlChecker()
		{
<<<<<<< HEAD
			using (IMyWhitelistBatch myWhitelistBatch = MyVRage.Platform.Scripting.OpenWhitelistBatch())
			{
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel), typeof(LandingGearMode));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(SpaceEngineers.Game.ModAPI.IMyButtonPanel), typeof(MySafeZoneBlockDefinition));
			}
=======
			using IMyWhitelistBatch myWhitelistBatch = MyVRage.Platform.Scripting.OpenWhitelistBatch();
			myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel), typeof(LandingGearMode));
			myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(SpaceEngineers.Game.ModAPI.IMyButtonPanel), typeof(MySafeZoneBlockDefinition));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void InitIlCompiler()
		{
			List<string> list = new List<string>();
			list.Add(Path.Combine(Assembly.Load("netstandard").Location));
			list.Add(Path.Combine(MyFileSystem.ExePath, "Sandbox.Game.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "Sandbox.Common.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "Sandbox.Graphics.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "VRage.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "VRage.Library.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "VRage.Math.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "VRage.Game.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "VRage.Render.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "VRage.Input.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "SpaceEngineers.ObjectBuilders.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "SpaceEngineers.Game.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "System.Collections.Immutable.dll"));
			list.Add(Path.Combine(MyFileSystem.ExePath, "ProtoBuf.Net.Core.dll"));
			List<string> list2 = list;
<<<<<<< HEAD
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
=======
			Assembly[] assemblies = AppDomain.get_CurrentDomain().GetAssemblies();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (Assembly assembly in assemblies)
			{
				string name = assembly.GetName().Name;
				if (name == "System.Runtime" || name == "System.Collections")
				{
					list2.Add(assembly.Location);
				}
			}
			MyVRage.Platform.Scripting.Initialize(MySandboxGame.Static.UpdateThread, list2, new Type[14]
			{
				typeof(MyTuple),
				typeof(Vector2),
				typeof(VRage.Game.Game),
				typeof(ITerminalAction),
				typeof(IMyGridTerminalSystem),
				typeof(MyModelComponent),
				typeof(IMyComponentAggregate),
				typeof(ListReader<>),
				typeof(MyObjectBuilder_FactionDefinition),
				typeof(IMyCubeBlock),
				typeof(MyIni),
				typeof(ImmutableArray),
				typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent),
				typeof(MySprite)
			}, new string[6]
			{
				GetPrefixedBranchName(),
				"STABLE",
				string.Empty,
				string.Empty,
				"VERSION_" + ((Version)MyFinalBuildConstants.APP_VERSION).Minor,
				"BUILD_" + ((Version)MyFinalBuildConstants.APP_VERSION).Build
			}, MyFakes.ENABLE_ROSLYN_SCRIPT_DIAGNOSTICS ? Path.Combine(MyFileSystem.UserDataPath, "ScriptDiagnostics") : null, MyFakes.ENABLE_SCRIPTS_PDB);
		}

		private string GetPrefixedBranchName()
		{
			string branchName = MyGameService.BranchName;
			branchName = ((!string.IsNullOrEmpty(branchName)) ? Regex.Replace(branchName, "[^a-zA-Z0-9_]", "_").ToUpper() : "STABLE");
			return "BRANCH_" + branchName;
		}
	}
}
