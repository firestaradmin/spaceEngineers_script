using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.Definitions.Animation;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.ObjectBuilders;

namespace Sandbox.Game.GameSystems.Chat
{
	/// <summary>
	/// Convenient grouping for chat commands that play animations, and the logic around the definitions
	/// </summary>
	public static class AnimationCommands
	{
		internal static void LoadAnimations(MyChatCommandSystem system)
		{
			system.OnUnhandledCommand += GetAnimationCommands;
		}

		internal static void GetAnimationCommands(string command, string body, List<IMyChatCommand> executableCommands)
		{
			foreach (MyAnimationDefinition def2 in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (!string.IsNullOrWhiteSpace(def2.ChatCommand) && def2.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
				{
					MyChatCommand item = new MyChatCommand(def2.ChatCommand, def2.ChatCommandName, def2.ChatCommandDescription, delegate
					{
						MyAnimationActivator.Activate(def2);
					});
					executableCommands.Add(item);
					return;
				}
			}
<<<<<<< HEAD
			foreach (MyEmoteDefinition emoteDef in MyDefinitionManager.Static.GetEmoteDefinitions())
			{
				if (string.IsNullOrWhiteSpace(emoteDef.ChatCommand) || !emoteDef.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
				{
					continue;
				}
				if (emoteDef.Animations != null && emoteDef.Animations.Length != 0)
				{
					string winningAnimationName = GetWinningAnimationName(emoteDef.Animations);
					foreach (MyEmoteDefinition animDef in MyDefinitionManager.Static.GetEmoteDefinitions())
					{
						if (winningAnimationName == animDef.Id.SubtypeId.String)
						{
							MyChatCommand item2 = new MyChatCommand(emoteDef.ChatCommand, emoteDef.ChatCommandName, emoteDef.ChatCommandDescription, delegate
							{
								MyAnimationActivator.Activate(animDef);
							});
							executableCommands.Add(item2);
							break;
						}
					}
=======
			if (Sync.IsDedicated || MyGameService.InventoryItems == null)
			{
				return;
			}
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				if (inventoryItem == null || inventoryItem.ItemDefinition == null || inventoryItem.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote)
				{
					continue;
				}
				MyEmoteDefinition def = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(inventoryItem.ItemDefinition.AssetModifierId);
				if (def != null && MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(def, MySession.Static.LocalHumanPlayer.Id.SteamId) && !string.IsNullOrWhiteSpace(def.ChatCommand) && def.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
				{
					MyChatCommand item2 = new MyChatCommand(def.ChatCommand, def.ChatCommandName, def.ChatCommandDescription, delegate
					{
						MyAnimationActivator.Activate(def);
					});
					executableCommands.Add(item2);
					break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					MyChatCommand item3 = new MyChatCommand(emoteDef.ChatCommand, emoteDef.ChatCommandName, emoteDef.ChatCommandDescription, delegate
					{
						MyAnimationActivator.Activate(emoteDef);
					});
					executableCommands.Add(item3);
				}
				return;
			}
			if (Sync.IsDedicated || MyGameService.InventoryItems == null)
			{
				return;
			}
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				if (inventoryItem == null || inventoryItem.ItemDefinition == null || inventoryItem.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote)
				{
					continue;
				}
				MyEmoteDefinition def = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(inventoryItem.ItemDefinition.AssetModifierId);
				if (def != null && MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(def, MySession.Static.LocalHumanPlayer.Id.SteamId) && !string.IsNullOrWhiteSpace(def.ChatCommand) && def.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
				{
					MyChatCommand item4 = new MyChatCommand(def.ChatCommand, def.ChatCommandName, def.ChatCommandDescription, delegate
					{
						MyAnimationActivator.Activate(def);
					});
					executableCommands.Add(item4);
					break;
				}
			}
		}

		public static string GetWinningAnimationName(MyObjectBuilder_EmoteDefinition.Animation[] animations)
		{
			if (animations == null || (animations != null && animations.Length == 0))
			{
				return string.Empty;
			}
			Random random = new Random();
			NormalizeProbabilities(animations);
			double num = random.NextDouble();
			float num2 = 0f;
			for (int i = 0; i < animations.Length; i++)
			{
				MyObjectBuilder_EmoteDefinition.Animation animation = animations[i];
				num2 += animation.Probability;
				if (num < (double)num2)
				{
					SerializableDefinitionId animationId = animation.AnimationId;
					return animationId.SubtypeId;
				}
			}
			return animations[animations.Length - 1].AnimationId.SubtypeId;
		}

		private static void NormalizeProbabilities(MyObjectBuilder_EmoteDefinition.Animation[] animations)
		{
			float num = 0f;
			for (int i = 0; i < animations.Length; i++)
			{
				MyObjectBuilder_EmoteDefinition.Animation animation = animations[i];
				num += animation.Probability;
			}
			if (num < 0.99f || num > 1.01f)
			{
				for (int j = 0; j < animations.Length; j++)
				{
					animations[j].Probability /= num;
				}
			}
		}
	}
}
