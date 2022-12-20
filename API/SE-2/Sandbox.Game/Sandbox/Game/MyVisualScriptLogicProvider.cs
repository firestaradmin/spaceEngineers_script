using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Game.AI;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.AI;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.VisualScripting;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
	[StaticEventOwner]
	public static class MyVisualScriptLogicProvider
	{
		private class AllowedAchievementsHelper
		{
			public static readonly List<string> AllowedAchievements;

			static AllowedAchievementsHelper()
			{
				AllowedAchievements = new List<string>();
				AllowedAchievements.Add("Promoted_engineer");
				AllowedAchievements.Add("Engineering_degree");
				AllowedAchievements.Add("Planetesphobia");
				AllowedAchievements.Add("Rapid_disassembly");
				AllowedAchievements.Add("It_takes_but_one");
				AllowedAchievements.Add("I_see_dead_drones");
				AllowedAchievements.Add("Bring_it_on");
				AllowedAchievements.Add("Im_doing_my_part");
				AllowedAchievements.Add("Scrap_delivery");
				AllowedAchievements.Add("Joint_operation");
				AllowedAchievements.Add("Flak_fodde");
				AllowedAchievements.Add("MyAchievement_PlayingItCool");
			}
		}

		protected sealed class MusicPlayMusicCueSync_003C_003ESystem_String_0023System_Boolean : ICallSite<IMyEventOwner, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string cueName, in bool playAtLeastOnce, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				MusicPlayMusicCueSync(cueName, playAtLeastOnce);
			}
		}

		protected sealed class MusicPlayMusicCategorySync_003C_003ESystem_String_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string categoryName, in bool playAtLeastOnce, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				MusicPlayMusicCategorySync(categoryName, playAtLeastOnce, playerId);
			}
		}

		protected sealed class MusicSetMusicCategorySync_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string categoryName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				MusicSetMusicCategorySync(categoryName);
			}
		}

		protected sealed class MusicSetDynamicMusicSync_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool enabled, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				MusicSetDynamicMusicSync(enabled);
			}
		}

		protected sealed class PlaySingleSoundAtEntitySync_003C_003ESystem_String_0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string soundName, in string entityName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlaySingleSoundAtEntitySync(soundName, entityName);
			}
		}

		protected sealed class PlayHudSoundSync_003C_003EVRage_Audio_MyGuiSounds_0023System_Int64 : ICallSite<IMyEventOwner, MyGuiSounds, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyGuiSounds sound, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlayHudSoundSync(sound, playerId);
			}
		}

		protected sealed class PlaySingleSoundAtPositionSync_003C_003ESystem_String_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string soundName, in Vector3D position, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlaySingleSoundAtPositionSync(soundName, position);
			}
		}

		protected sealed class CreateSoundEmitterAtEntitySync_003C_003ESystem_String_0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string newEmitterId, in string entityName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateSoundEmitterAtEntitySync(newEmitterId, entityName);
			}
		}

		protected sealed class CreateSoundEmitterAtPositionSync_003C_003ESystem_String_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string newEmitterId, in Vector3D position, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateSoundEmitterAtPositionSync(newEmitterId, position);
			}
		}

		protected sealed class PlaySoundSync_003C_003ESystem_String_0023System_String_0023System_Boolean : ICallSite<IMyEventOwner, string, string, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string emitterId, in string soundName, in bool playIn2D, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlaySoundSync(emitterId, soundName, playIn2D);
			}
		}

		protected sealed class StopSoundSync_003C_003ESystem_String_0023System_Boolean : ICallSite<IMyEventOwner, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string emitterId, in bool forced, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StopSoundSync(emitterId, forced);
			}
		}

		protected sealed class RemoveSoundEmitterSync_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string emitterId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveSoundEmitterSync(emitterId);
			}
		}

		protected sealed class PlaySoundAmbientSync_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string soundName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlaySoundAmbientSync(soundName);
			}
		}

		protected sealed class SetVolumeSync_003C_003ESystem_Single_0023System_Single_0023System_Single : ICallSite<IMyEventOwner, float, float, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float gameVolume, in float musicVolume, in float voiceChatVolume, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetVolumeSync(gameVolume, musicVolume, voiceChatVolume);
			}
		}

		protected sealed class StartCutsceneSync_003C_003ESystem_String_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string cutsceneName, in bool registerEvents, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StartCutsceneSync(cutsceneName, registerEvents, playerId);
			}
		}

		protected sealed class NextCutsceneNodeSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				NextCutsceneNodeSync(playerId);
			}
		}

		protected sealed class EndCutsceneSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EndCutsceneSync(playerId);
			}
		}

		protected sealed class SkipCutsceneSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SkipCutsceneSync(playerId);
			}
		}

		protected sealed class RequestVicinityCacheSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestVicinityCacheSync(entityId);
			}
		}

		protected sealed class CreateExplosionInternalAll_003C_003EVRageMath_Vector3D_0023System_Single_0023System_Int32 : ICallSite<IMyEventOwner, Vector3D, float, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in float radius, in int damage, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateExplosionInternalAll(position, radius, damage);
			}
		}

		protected sealed class CreateParticleEffectAtPositionInternalAll_003C_003ESystem_String_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string effectName, in Vector3D position, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateParticleEffectAtPositionInternalAll(effectName, position);
			}
		}

		protected sealed class CreateParticleEffectAtEntityInternalAll_003C_003ESystem_String_0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string effectName, in string entityName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateParticleEffectAtEntityInternalAll(effectName, entityName);
			}
		}

		protected sealed class ScreenColorFadingStartInternal_003C_003ESystem_Single_0023System_Boolean : ICallSite<IMyEventOwner, float, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float time, in bool toOpaque, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingStartInternal(time, toOpaque);
			}
		}

		protected sealed class ScreenColorFadingStartInternalAll_003C_003ESystem_Single_0023System_Boolean : ICallSite<IMyEventOwner, float, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float time, in bool toOpaque, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingStartInternalAll(time, toOpaque);
			}
		}

		protected sealed class ScreenColorFadingSetColorInternal_003C_003EVRageMath_Color : ICallSite<IMyEventOwner, Color, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Color color, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingSetColorInternal(color);
			}
		}

		protected sealed class ScreenColorFadingSetColorInternalAll_003C_003EVRageMath_Color : ICallSite<IMyEventOwner, Color, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Color color, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingSetColorInternalAll(color);
			}
		}

		protected sealed class ScreenColorFadingStartSwitchInternal_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float time, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingStartSwitchInternal(time);
			}
		}

		protected sealed class ScreenColorFadingStartSwitchInternalAll_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float time, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingStartSwitchInternalAll(time);
			}
		}

		protected sealed class ScreenColorFadingMinimalizeHUDInternal_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool minimalize, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingMinimalizeHUDInternal(minimalize);
			}
		}

		protected sealed class ScreenColorFadingMinimalizeHUDInternalAll_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool minimalize, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ScreenColorFadingMinimalizeHUDInternalAll(minimalize);
			}
		}

		protected sealed class ShowHudInternal_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool flag, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowHudInternal(flag);
			}
		}

		protected sealed class ShowHudInternalAll_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool flag, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowHudInternalAll(flag);
			}
		}

		protected sealed class SetHudStateInternal_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int state, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetHudStateInternal(state);
			}
		}

		protected sealed class SetHudStateInternalAll_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int state, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetHudStateInternalAll(state);
			}
		}

		protected sealed class FogSetAllInternalAll_003C_003ESystem_Single_0023System_Single_0023VRageMath_Vector3_0023System_Single_0023System_Single : ICallSite<IMyEventOwner, float, float, Vector3, float, float, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float density, in float multiplier, in Vector3 color, in float skyboxMultiplier, in float atmoMultiplier, in DBNull arg6)
			{
				FogSetAllInternalAll(density, multiplier, color, skyboxMultiplier, atmoMultiplier);
			}
		}

		protected sealed class EnableSavingSync_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool enable, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EnableSavingSync(enable);
			}
		}

		protected sealed class EnableCustomRespawnSync_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool enable, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EnableCustomRespawnSync(enable, playerId);
			}
		}

		protected sealed class CustomRespawnRequestServer_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CustomRespawnRequestServer(playerId);
			}
		}

		protected sealed class SetHighlightSync_003C_003ESystem_String_0023System_Int32_0023System_Int32_0023VRageMath_Color_0023System_Int64_0023System_String : ICallSite<IMyEventOwner, string, int, int, Color, long, string>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string entityName, in int thickness, in int pulseTimeInFrames, in Color color, in long playerId, in string subPartNames)
			{
				SetHighlightSync(entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
			}
		}

		protected sealed class OpenSteamOverlaySync_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string url, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OpenSteamOverlaySync(url);
			}
		}

		protected sealed class OpenPlayerScreenAllBroadcast_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OpenPlayerScreenAllBroadcast();
			}
		}

		protected sealed class OpenFactionVictoryScreenAllBroadcast_003C_003ESystem_String_0023System_Single : ICallSite<IMyEventOwner, string, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string factionTag, in float timeInSec, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OpenFactionVictoryScreenAllBroadcast(factionTag, timeInSec);
			}
		}

		protected sealed class ShowNotificationSync_003C_003ESystem_String_0023System_Int32_0023System_String : ICallSite<IMyEventOwner, string, int, string, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string message, in int disappearTimeMs, in string font, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowNotificationSync(message, disappearTimeMs, font);
			}
		}

		protected sealed class ShowNotificationToAllSync_003C_003ESystem_String_0023System_Int32_0023System_String : ICallSite<IMyEventOwner, string, int, string, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string message, in int disappearTimeMs, in string font, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowNotificationToAllSync(message, disappearTimeMs, font);
			}
		}

		protected sealed class AddNotificationSync_003C_003ESystem_String_0023System_String_0023System_Int32 : ICallSite<IMyEventOwner, string, string, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string message, in string font, in int notificationId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddNotificationSync(message, font, notificationId);
			}
		}

		protected sealed class AddNotificationToAllSync_003C_003ESystem_String_0023System_String_0023System_Int32 : ICallSite<IMyEventOwner, string, string, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string message, in string font, in int notificationId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddNotificationToAllSync(message, font, notificationId);
			}
		}

		protected sealed class RemoveNotificationSync_003C_003ESystem_Int32_0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int messageId, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveNotificationSync(messageId, playerId);
			}
		}

		protected sealed class ClearNotificationSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ClearNotificationSync(playerId);
			}
		}

		protected sealed class DisplayCongratulationScreenInternal_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int congratulationMessageId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DisplayCongratulationScreenInternal(congratulationMessageId);
			}
		}

		protected sealed class DisplayCongratulationScreenInternalAll_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int congratulationMessageId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DisplayCongratulationScreenInternalAll(congratulationMessageId);
			}
		}

		protected sealed class CloseRespawnScreen_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CloseRespawnScreen();
			}
		}

		protected sealed class SetPlayerInputBlacklistStateSync_003C_003ESystem_String_0023System_Int64_0023System_Boolean : ICallSite<IMyEventOwner, string, long, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string controlStringId, in long playerId, in bool enabled, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetPlayerInputBlacklistStateSync(controlStringId, playerId, enabled);
			}
		}

		protected sealed class ToggleAbilityToSprintSync_003C_003ESystem_Int64_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in bool canSprint, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ToggleAbilityToSprintSync(playerId, canSprint);
			}
		}

		protected sealed class SetQuestlogSync_003C_003ESystem_Boolean_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, bool, string, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool visible, in string questName, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetQuestlogSync(visible, questName, playerId);
			}
		}

		protected sealed class SetQuestlogTitleSync_003C_003ESystem_String_0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string questName, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetQuestlogTitleSync(questName, playerId);
			}
		}

		protected sealed class AddQuestlogDetailSync_003C_003ESystem_String_0023System_Boolean_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, bool, bool, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string questDetailRow, in bool completePrevious, in bool useTyping, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				AddQuestlogDetailSync(questDetailRow, completePrevious, useTyping, playerId);
			}
		}

		protected sealed class AddQuestlogObjectiveSync_003C_003ESystem_String_0023System_Boolean_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, bool, bool, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string questDetailRow, in bool completePrevious, in bool useTyping, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				AddQuestlogObjectiveSync(questDetailRow, completePrevious, useTyping, playerId);
			}
		}

		protected sealed class SetQuestlogDetailCompletedSync_003C_003ESystem_Int32_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, int, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int lineId, in bool completed, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetQuestlogDetailCompletedSync(lineId, completed, playerId);
			}
		}

		protected sealed class SetAllQuestlogDetailsCompletedSync_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool completed, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetAllQuestlogDetailsCompletedSync(completed, playerId);
			}
		}

		protected sealed class ReplaceQuestlogDetailSync_003C_003ESystem_Int32_0023System_String_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, int, string, bool, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int id, in string newDetail, in bool useTyping, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				ReplaceQuestlogDetailSync(id, newDetail, useTyping, playerId);
			}
		}

		protected sealed class RemoveQuestlogDetailsSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveQuestlogDetailsSync(playerId);
			}
		}

		protected sealed class SetQuestlogPageSync_003C_003ESystem_Int32_0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int value, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetQuestlogPageSync(value, playerId);
			}
		}

		protected sealed class SetQuestlogVisibleSync_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool value, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetQuestlogVisibleSync(value, playerId);
			}
		}

		protected sealed class EnableHighlightSync_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool enable, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EnableHighlightSync(enable, playerId);
			}
		}

		protected sealed class SetToolbarSlotToItemSync_003C_003ESystem_Int32_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Int64 : ICallSite<IMyEventOwner, int, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int slot, in SerializableDefinitionId itemId, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetToolbarSlotToItemSync(slot, itemId, playerId);
			}
		}

		protected sealed class SwitchToolbarToSlotSync_003C_003ESystem_Int32_0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int slot, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SwitchToolbarToSlotSync(slot, playerId);
			}
		}

		protected sealed class ClearToolbarSlotSync_003C_003ESystem_Int32_0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int slot, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ClearToolbarSlotSync(slot, playerId);
			}
		}

		protected sealed class ClearAllToolbarSlotsSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ClearAllToolbarSlotsSync(playerId);
			}
		}

		protected sealed class SetToolbarPageSync_003C_003ESystem_Int32_0023System_Int64 : ICallSite<IMyEventOwner, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int page, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetToolbarPageSync(page, playerId);
			}
		}

		protected sealed class ReloadToolbarDefaultsSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReloadToolbarDefaultsSync(playerId);
			}
		}

		protected sealed class UnlockAchievementInternal_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int achievementId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockAchievementInternal(achievementId);
			}
		}

		protected sealed class UnlockAchievementInternalAll_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int achievementId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockAchievementInternalAll(achievementId);
			}
		}

		protected sealed class CreateUIStringSync_003C_003ESystem_Int64_0023Sandbox_Game_MyUIString_0023System_Int64 : ICallSite<IMyEventOwner, long, MyUIString, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long id, in MyUIString UIString, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateUIStringSync(id, UIString, playerId);
			}
		}

		protected sealed class RemoveUIStringSync_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long id, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveUIStringSync(id, playerId);
			}
		}

		protected sealed class CreateBoardScreenSync_003C_003ESystem_String_0023System_Single_0023System_Single_0023System_Single_0023System_Single_0023System_Int64 : ICallSite<IMyEventOwner, string, float, float, float, float, long>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in float normalizedPosX, in float normalizedPosY, in float normalizedSizeX, in float normalizedSizeY, in long playerId)
			{
				CreateBoardScreenSync(boardId, normalizedPosX, normalizedPosY, normalizedSizeX, normalizedSizeY, playerId);
			}
		}

		protected sealed class RemoveBoardScreenSync_003C_003ESystem_String_0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveBoardScreenSync(boardId, playerId);
			}
		}

		protected sealed class AddColumnSync_003C_003ESystem_String_0023System_String_0023Sandbox_Game_Gui_MyGuiScreenBoard_003C_003EMyColumn_0023System_Int64 : ICallSite<IMyEventOwner, string, string, MyGuiScreenBoard.MyColumn, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string columnId, in MyGuiScreenBoard.MyColumn column, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				AddColumnSync(boardId, columnId, column, playerId);
			}
		}

		protected sealed class RemoveColumnSync_003C_003ESystem_String_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string columnId, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveColumnSync(boardId, columnId, playerId);
			}
		}

		protected sealed class AddRowSync_003C_003ESystem_String_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string rowId, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddRowSync(boardId, rowId, playerId);
			}
		}

		protected sealed class RemoveRowSync_003C_003ESystem_String_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string rowId, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveRowSync(boardId, rowId, playerId);
			}
		}

		protected sealed class SetCellSync_003C_003ESystem_String_0023System_String_0023System_String_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, string, string, string, string, long, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string rowId, in string columnId, in string text, in long playerId, in DBNull arg6)
			{
				SetCellSync(boardId, rowId, columnId, text, playerId);
			}
		}

		protected sealed class SetRowRankingSync_003C_003ESystem_String_0023System_String_0023System_Int32_0023System_Int64 : ICallSite<IMyEventOwner, string, string, int, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string rowId, in int ranking, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				SetRowRankingSync(boardId, rowId, ranking, playerId);
			}
		}

		protected sealed class SortByColumnSync_003C_003ESystem_String_0023System_String_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, string, bool, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string columnId, in bool ascending, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				SortByColumnSync(boardId, columnId, ascending, playerId);
			}
		}

		protected sealed class SortByRankingSync_003C_003ESystem_String_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in bool ascending, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SortByRankingSync(boardId, ascending, playerId);
			}
		}

		protected sealed class ShowOrderInColumnSync_003C_003ESystem_String_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, string, string, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string columnId, in long playerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowOrderInColumnSync(boardId, columnId, playerId);
			}
		}

		protected sealed class SetColumnVisibilitySync_003C_003ESystem_String_0023System_String_0023System_Boolean_0023System_Int64 : ICallSite<IMyEventOwner, string, string, bool, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string boardId, in string columnId, in bool visible, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				SetColumnVisibilitySync(boardId, columnId, visible, playerId);
			}
		}

		private static readonly Dictionary<Vector3I, bool> m_thrustDirections = new Dictionary<Vector3I, bool>();

		private static readonly Dictionary<int, MyHudNotification> m_addedNotificationsById = new Dictionary<int, MyHudNotification>();

		private static int m_notificationIdCounter;

		private static List<MyEntity> m_tmpEntities = new List<MyEntity>();

		private static Dictionary<string, (int Time, bool Running)> m_timers = new Dictionary<string, (int, bool)>();

		[Display(Name = "Player Left Cockpit", Description = "When player leaves cockpit.")]
		public static DoubleKeyPlayerEvent PlayerLeftCockpit;

		[Display(Name = "Player Entered Cockpit", Description = "When player leaves cockpit.")]
		public static DoubleKeyPlayerEvent PlayerEnteredCockpit;

		[Display(Name = "Respawn Ship Spawned", Description = "Called right after a respawn ship is created for a player.")]
		public static RespawnShipSpawnedEvent RespawnShipSpawned;

		[Display(Name = "Cutscene Node Event", Description = "")]
		public static CutsceneEvent CutsceneNodeEvent;

		[Display(Name = "Cutscene Ended", Description = "When cutscene ended.")]
		public static CutsceneEvent CutsceneEnded;

		[Display(Name = "Player Spawned", Description = "When player spawns in the world.")]
		public static SingleKeyPlayerEvent PlayerSpawned;

		[Display(Name = "Team Balancer Player Sorted", Description = "Player was sorted into faction by Team Balancer")]
		public static TeamBalancerSortEvent TeamBalancerPlayerSorted;

		[Display(Name = "Player Respawn request", Description = "When player requests respawn.")]
		public static SingleKeyPlayerEvent PlayerRequestsRespawn;

		[Display(Name = "Player Died", Description = "When player dies in the world.")]
		public static SingleKeyPlayerEvent PlayerDied;

		[Display(Name = "Player Connected", Description = "When player connects.")]
		public static SingleKeyPlayerEvent PlayerConnected;

		[Display(Name = "Player Disconnected", Description = "When player disconnects.")]
		public static SingleKeyPlayerEvent PlayerDisconnected;

		[Display(Name = "Player Connect Request", Description = "When player tries to connect.")]
		public static SingleKeyPlayerConnectRequestEvent PlayerConnectRequest;

		[Display(Name = "Player Respawned", Description = "When player respawns.")]
		public static SingleKeyPlayerEvent PlayerRespawnRequest;

		[Display(Name = "NPC Died", Description = "When player dies.")]
		public static SingleKeyEntityNameEvent NPCDied;

		[Display(Name = "Player Health Recharging", Description = "When player is recharging health.")]
		public static PlayerHealthRechargeEvent PlayerHealthRecharging;

		[Display(Name = "Player Suit Recharging", Description = "When suit is recharging power/oxygen/hydrogen.")]
		public static PlayerSuitRechargeEvent PlayerSuitRecharging;

		[Display(Name = "Timer Block Triggered", Description = "When timer block triggers.")]
		public static SingleKeyEntityNameGridNameEvent TimerBlockTriggered;

		[Display(Name = "Timer Block Triggered Entity Name", Description = "")]
		public static SingleKeyEntityNameGridNameEvent TimerBlockTriggeredEntityName;

		[Display(Name = "Player Picked Up Item", Description = "When player picks up item.")]
		public static FloatingObjectPlayerEvent PlayerPickedUp;

		[Display(Name = "Player Dropped Item", Description = "When player drops item.")]
		public static PlayerItemEvent PlayerDropped;

		[Display(Name = "Item Spawned", Description = "When item spawns.")]
		public static ItemSpawnedEvent ItemSpawned;

		[Display(Name = "Button Pressed Entity Name", Description = "When someone press the button.")]
		public static ButtonPanelEvent ButtonPressedEntityName;

		[Display(Name = "Button Pressed Terminal Name", Description = "When someone press the button.")]
		public static ButtonPanelEvent ButtonPressedTerminalName;

		[Display(Name = "Area Trigger Entity Left", Description = "When entity leaves area of the trigger.")]
		public static TriggerEventComplex AreaTrigger_EntityLeft;

		[Display(Name = "Area Trigger Entity Entered", Description = "When entity enters area of the trigger.")]
		public static TriggerEventComplex AreaTrigger_EntityEntered;

		[Display(Name = "Area Trigger Left", Description = "When player leaves area of the trigger.")]
		public static SingleKeyTriggerEvent AreaTrigger_Left;

		[Display(Name = "Area Trigger Entered", Description = "When player enters area of the trigger.")]
		public static SingleKeyTriggerEvent AreaTrigger_Entered;

		[Display(Name = "Screen Added", Description = "When screen is added.")]
		public static ScreenManagerEvent ScreenAdded;

		[Display(Name = "Screen Removed", Description = "When screen is removed.")]
		public static ScreenManagerEvent ScreenRemoved;

		[Display(Name = "Block Destroyed", Description = "When block is removed from the grid, either damage or grinding.")]
		public static SingleKeyEntityNameGridNameEvent BlockDestroyed;

		[Display(Name = "Block Integrity Changed ", Description = "When block is removed from the grid, either damage or grinding.")]
		public static SingleKeyEntityNameGridNameEvent BlockIntegrityChanged;

		[Display(Name = "Block Damaged", Description = "When block is damaged.")]
		public static BlockDamagedEvent BlockDamaged;

		[Display(Name = "Block Built", Description = "When block is build.")]
		public static BlockEvent BlockBuilt;

		[Display(Name = "Prefab Spawned", Description = "When prefab is spawned.")]
		public static SingleKeyEntityNameEvent PrefabSpawned;

		[Display(Name = "Prefab Spawned Detailed", Description = "When prefab is spawned, includes prefab name.")]
		public static PrefabSpawnedEvent PrefabSpawnedDetailed;

		[Display(Name = "Grid Spawned", Description = "When grid is spawned.")]
		public static SingleKeyEntityNameEvent GridSpawned;

		[Display(Name = "Block Functionality Changed", Description = "When block function state is changed.")]
		public static BlockFunctionalityChangedEvent BlockFunctionalityChanged;

		[Display(Name = "Tool Equipped", Description = "When tool is equipped.")]
		public static ToolEquipedEvent ToolEquipped;

		[Display(Name = "Landing Gear Unlocked", Description = "When landing gear is unlocked.")]
		public static LandingGearUnlockedEvent LandingGearUnlocked;

		[Display(Name = "Grid Power Generation State Changed", Description = "When grid power generation state is changed.")]
		public static GridPowerGenerationStateChangedEvent GridPowerGenerationStateChanged;

		[Display(Name = "Room Fully Pressurized", Description = "When room is fully pressurized.")]
		public static RoomFullyPressurizedEvent RoomFullyPressurized;

		[Display(Name = "NewBuiltItem", Description = "When new item is build.")]
		public static NewBuiltItemEvent NewItemBuilt;

		[Display(Name = "WeaponBlockActivated", Description = "When gatling gun or missile launcher shoots.")]
		public static WeaponBlockActivatedEvent WeaponBlockActivated;

		[Display(Name = "ConnectorStateChanged", Description = "When Two connectors dis/connect.")]
		public static ConnectorStateChangedEvent ConnectorStateChanged;

		[Display(Name = "GridJumped", Description = "When grid uses jumpdrive to jump.")]
		public static GridJumpedEvent GridJumped;

		[Display(Name = "ShipDrillDrilled", Description = "When drill obtains ore by mining voxels.")]
		public static ShipDrillCollectedEvent ShipDrillCollected;

		[Display(Name = "RemoteControlChanged", Description = "When remote control block get controlled by player.")]
		public static RemoteControlChangedEvent RemoteControlChanged;

		[Display(Name = "ToolbarItemChanged", Description = "When an item on a toolbar is changed.")]
		public static ToolbarItemChangedEvent ToolbarItemChanged;

		[Display(Name = "MatchStateStarted", Description = "When new match state started")]
		public static MatchStateStartedEvent MatchStateStarted;

		[Display(Name = "MatchStateStarted", Description = "When new match state ended")]
		public static MatchStateEndedEvent MatchStateEnded;

		[Display(Name = "MatchStateStarted", Description = "When new match state changed")]
		public static MatchStateChangedEvent MatchStateChanged;

		[Display(Name = "ContractAccepted", Description = "When contract has been accepted. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.")]
		public static ContractAcceptedEvent ContractAccepted;

		[Display(Name = "ContractFinished", Description = "When contract has been finished. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.")]
		public static ContractFinishedEvent ContractFinished;

		[Display(Name = "ContractFailed", Description = "When contract has been failed. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.")]
		public static ContractFailedEvent ContractFailed;

		[Display(Name = "ContractAbandoned", Description = "When contract has been abandoned. 'startingFactionId' and 'startingStationId' are only for non-player-made contracts, 'startingBlockId' is only for player-made contracts.")]
		public static ContractAbandonedEvent ContractAbandoned;

		[Display(Name = "BeforeMatchStateChanged", Description = "Called before match state ends. Set interrupt to true to stop state from ending. (If end was caused by timeout and interruption happens, time will still be out and end will be called again the next frame. )")]
		public static MatchStateEndingEvent MatchStateEnding;

		private static MyStringId MUSIC = MyStringId.GetOrCompute("Music");

		private static MyStringHash DAMAGE_TYPE_SCRIPT = MyStringHash.GetOrCompute("Script");

		public static bool GameIsReady = false;

		private static bool m_registered = false;

		private static bool m_exitGameDialogOpened = false;

		private static readonly Dictionary<long, List<MyTuple<long, int>>> m_playerIdsToHighlightData = new Dictionary<long, List<MyTuple<long, int>>>();

		private static readonly Color DEFAULT_HIGHLIGHT_COLOR = new Color(0, 96, 209, 25);

		private static readonly MyDefinitionId ElectricityId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Electricity");

		public static void Init()
		{
			MyCubeGrids.BlockBuilt += delegate(MyCubeGrid grid, MySlimBlock block)
			{
				if (BlockBuilt != null)
				{
					BlockEvent blockBuilt2 = BlockBuilt;
					MyObjectBuilderType typeId2 = block.BlockDefinition.Id.TypeId;
					blockBuilt2(typeId2.ToString(), block.BlockDefinition.Id.SubtypeName, grid.Name, (block.FatBlock != null) ? block.FatBlock.EntityId : 0);
				}
			};
			if (m_registered)
			{
				return;
			}
			m_registered = true;
			MySession.OnLoading += delegate
			{
				m_addedNotificationsById.Clear();
				m_playerIdsToHighlightData.Clear();
			};
			MyEntities.OnEntityAdd += delegate(MyEntity entity)
			{
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null && BlockBuilt != null && myCubeGrid.BlocksCount == 1)
				{
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(Vector3I.Zero);
					if (cubeBlock != null)
					{
						BlockEvent blockBuilt = BlockBuilt;
						MyObjectBuilderType typeId = cubeBlock.BlockDefinition.Id.TypeId;
						blockBuilt(typeId.ToString(), cubeBlock.BlockDefinition.Id.SubtypeName, myCubeGrid.Name, (cubeBlock.FatBlock != null) ? cubeBlock.FatBlock.EntityId : 0);
					}
				}
			};
			MyScreenManager.ScreenRemoved += delegate(MyGuiScreenBase screen)
			{
				if (ScreenRemoved != null)
				{
					ScreenRemoved(screen);
				}
			};
			MyScreenManager.ScreenAdded += delegate(MyGuiScreenBase screen)
			{
				if (ScreenAdded != null)
				{
					ScreenAdded(screen);
				}
			};
			MyRespawnComponentBase.RespawnRequested += delegate(MyPlayer player)
			{
				if (PlayerRespawnRequest != null)
				{
					PlayerRespawnRequest(player.Identity.IdentityId);
				}
			};
			MyVisualScriptingProxy.RegisterType(typeof(MyGuiSounds));
			MyVisualScriptingProxy.RegisterType(typeof(MyKeys));
			MyVisualScriptingProxy.RegisterType(typeof(FlightMode));
			MyVisualScriptingProxy.RegisterType(typeof(Base6Directions.Direction));
			MyVisualScriptingProxy.RegisterType(typeof(MyGuiDrawAlignEnum));
			MyVisualScriptingProxy.RegisterType(typeof(JoinResult));
			MyVisualScriptingProxy.WhitelistExtensions(typeof(MyVisualScriptLogicProvider));
			m_timers.Clear();
		}

		public static SerializableDictionary<string, (int Time, bool Running)> GetTimers()
		{
			return new SerializableDictionary<string, (int, bool)>(m_timers);
		}

		private static bool TryGetGrid(MyEntity entity, out MyCubeGrid grid)
		{
			if (entity is MyCubeGrid)
			{
				grid = (MyCubeGrid)entity;
				return true;
			}
			if (entity is MyCubeBlock)
			{
				grid = ((MyCubeBlock)entity).CubeGrid;
				return true;
			}
			grid = null;
			return false;
		}

		private static bool TryGetGrid(string entityName, out MyCubeGrid grid)
		{
			grid = null;
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
			{
				return false;
			}
			if (entityByName is MyCubeGrid)
			{
				grid = (MyCubeGrid)entityByName;
				return true;
			}
			if (entityByName is MyCubeBlock)
			{
				grid = ((MyCubeBlock)entityByName).CubeGrid;
				return true;
			}
			return false;
		}

		private static MyFixedPoint GetInventoryItemAmount(MyEntity entity, MyDefinitionId itemId)
		{
			MyFixedPoint result = 0;
			if (entity != null && entity.HasInventory)
			{
				for (int i = 0; i < entity.InventoryCount; i++)
				{
					MyInventory inventory = entity.GetInventory(i);
					if (inventory != null)
					{
						result += inventory.GetItemAmount(itemId);
					}
				}
			}
			return result;
		}

		private static MyFixedPoint RemoveInventoryItems(MyEntity entity, MyDefinitionId itemId, MyFixedPoint amountToRemove)
		{
			MyFixedPoint result = 0;
			MyFixedPoint myFixedPoint = 0;
			if (entity != null && entity.HasInventory && amountToRemove > 0)
			{
				for (int i = 0; i < entity.InventoryCount; i++)
				{
					MyInventory inventory = entity.GetInventory(i);
					if (inventory != null)
					{
						myFixedPoint = inventory.GetItemAmount(itemId);
						if (myFixedPoint > 0)
						{
							myFixedPoint = MyFixedPoint.Min(amountToRemove, myFixedPoint);
							inventory.RemoveItemsOfType(myFixedPoint, itemId);
							result += myFixedPoint;
							amountToRemove -= myFixedPoint;
						}
					}
					if (amountToRemove <= 0)
					{
						break;
					}
				}
			}
			return result;
		}

		[VisualScriptingMiscData("AI", "Adds specific drone behavior from preset to a drone. (Reduced parameters)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetDroneBehaviourBasic(string entityName, string presetName = "Default")
		{
			if (!string.IsNullOrEmpty(presetName))
			{
				SetDroneBehaviourMethod(entityName, presetName, null, null, activate: true, assignToPirates: true, 10, TargetPrioritization.PriorityRandom, 10000f, cycleWaypoints: false);
			}
		}

		[VisualScriptingMiscData("AI", "Adds specific drone behavior from preset to a drone. (Extended parameters)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetDroneBehaviourAdvanced(string entityName, string presetName = "Default", bool activate = true, bool assignToPirates = true, List<MyEntity> waypoints = null, bool cycleWaypoints = false, List<MyEntity> targets = null)
		{
			if (!string.IsNullOrEmpty(presetName))
			{
				List<DroneTarget> targets2 = DroneProcessTargets(targets);
				SetDroneBehaviourMethod(entityName, presetName, waypoints, targets2, activate, assignToPirates, 10, TargetPrioritization.PriorityRandom, 10000f, cycleWaypoints);
			}
		}

		[VisualScriptingMiscData("AI", "Adds specific drone behavior from preset to a drone. (Full parameters)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetDroneBehaviourFull(string entityName, string presetName = "Default", bool activate = true, bool assignToPirates = true, List<MyEntity> waypoints = null, bool cycleWaypoints = false, List<MyEntity> targets = null, int playerPriority = 10, float maxPlayerDistance = 10000f, TargetPrioritization prioritizationStyle = TargetPrioritization.PriorityRandom)
		{
			if (!string.IsNullOrEmpty(presetName))
			{
				List<DroneTarget> targets2 = DroneProcessTargets(targets);
				SetDroneBehaviourMethod(entityName, presetName, waypoints, targets2, activate, assignToPirates, playerPriority, prioritizationStyle, maxPlayerDistance, cycleWaypoints);
			}
		}

		public static List<DroneTarget> DroneProcessTargets(List<MyEntity> targets)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			List<DroneTarget> list = new List<DroneTarget>();
			if (targets != null)
			{
				foreach (MyEntity target in targets)
				{
					if (target is MyCubeGrid)
					{
						Enumerator<MySlimBlock> enumerator2 = ((MyCubeGrid)target).GetBlocks().GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MySlimBlock current2 = enumerator2.get_Current();
								if (current2.FatBlock is MyShipController)
								{
									list.Add(new DroneTarget(current2.FatBlock, 8));
								}
								if (current2.FatBlock is MyReactor)
								{
									list.Add(new DroneTarget(current2.FatBlock, 6));
								}
								if (current2.FatBlock is MyUserControllableGun)
								{
									list.Add(new DroneTarget(current2.FatBlock, 10));
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					else
					{
						list.Add(new DroneTarget(target));
					}
				}
				return list;
			}
			return list;
		}

		private static MyRemoteControl DroneGetRemote(string entityName)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			MyEntity myEntity = GetEntityByName(entityName);
			if (myEntity == null)
			{
				return null;
			}
			MyRemoteControl myRemoteControl = myEntity as MyRemoteControl;
			if (myEntity is MyCubeBlock && myRemoteControl == null)
			{
				myEntity = ((MyCubeBlock)myEntity).CubeGrid;
			}
			if (myEntity is MyCubeGrid)
			{
				Enumerator<MySlimBlock> enumerator = ((MyCubeGrid)myEntity).GetBlocks().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current = enumerator.get_Current();
						if (current.FatBlock is MyRemoteControl)
						{
							return current.FatBlock as MyRemoteControl;
						}
					}
					return myRemoteControl;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return myRemoteControl;
		}

		private static void SetDroneBehaviourMethod(string entityName, string presetName, List<MyEntity> waypoints, List<DroneTarget> targets, bool activate, bool assignToPirates, int playerPriority, TargetPrioritization prioritizationStyle, float maxPlayerDistance, bool cycleWaypoints)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl == null)
			{
				return;
			}
			if (waypoints != null)
			{
				int num = 0;
				while (num < waypoints.Count)
				{
					if (waypoints[num] == null)
					{
						waypoints.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
				}
			}
			if (assignToPirates)
			{
				myRemoteControl.CubeGrid.ChangeGridOwnership(GetPirateId(), MyOwnershipShareModeEnum.Faction);
			}
			MyDroneAI automaticBehaviour = new MyDroneAI(myRemoteControl, presetName, activate, waypoints, targets, playerPriority, prioritizationStyle, maxPlayerDistance, cycleWaypoints);
			myRemoteControl.SetAutomaticBehaviour(automaticBehaviour);
			if (activate)
			{
				myRemoteControl.SetAutoPilotEnabled(enabled: true);
			}
		}

		[VisualScriptingMiscData("AI", "Gets number of waypoints for specific drone. Returns -1 if drone has no remote or AI behavior.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int DroneGetWaypointCount(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				return myRemoteControl.AutomaticBehaviour.WaypointList.Count + (myRemoteControl.AutomaticBehaviour.WaypointActive ? 1 : 0);
			}
			return -1;
		}

		[VisualScriptingMiscData("AI", "Gets position of curret waypoint of specific drone. If current waypoint exists, returns it position and 'waypointName' will be name of the waypoint. If waypoint does not exists, return current position and 'waypointName' will be empty string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D AutopilotGetCurrentWaypoint(string entityName, out string waypointName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			waypointName = "";
			if (myRemoteControl != null)
			{
				if (myRemoteControl.CurrentWaypoint != null)
				{
					waypointName = myRemoteControl.CurrentWaypoint.Name;
					return myRemoteControl.CurrentWaypoint.Coords;
				}
				return myRemoteControl.PositionComp.GetPosition();
			}
			return Vector3D.Zero;
		}

		[VisualScriptingMiscData("AI", "Orders drone to immediately skip current waypoint and go directly to the next one.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotSkipCurrentWaypoint(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.CurrentWaypoint != null)
			{
				myRemoteControl.AdvanceWaypoint();
			}
		}

		[VisualScriptingMiscData("AI", "Gets count of targets for specific drone. Returns -1 if drone lacks remote or AI behavior.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int DroneGetTargetsCount(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				return myRemoteControl.AutomaticBehaviour.TargetList.Count;
			}
			return -1;
		}

		[VisualScriptingMiscData("AI", "Returns true if specific drone has both remote and AI behavior, false otherwise.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool DroneHasAI(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("AI", "Gets AI behavior of specific drone. Returns empty string if drone lacks remote or AI behavior.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string DroneGetCurrentAIBehavior(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				return myRemoteControl.AutomaticBehaviour.ToString();
			}
			return "";
		}

		[VisualScriptingMiscData("AI", "Sets current target of drone to specific entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetTarget(string entityName, MyEntity target)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null && target != null)
			{
				myRemoteControl.AutomaticBehaviour.CurrentTarget = target;
			}
		}

		[VisualScriptingMiscData("AI", "Activates/deactivates ambush mode for specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetAmbushMode(string entityName, bool ambushModeOn = true)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.Ambushing = ambushModeOn;
			}
		}

		[VisualScriptingMiscData("AI", "Sets maximum speed limit of specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetSpeedLimit(string entityName, float speedLimit = 100f)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.SpeedLimit = speedLimit;
				myRemoteControl.SetAutoPilotSpeedLimit(speedLimit);
			}
		}

		[VisualScriptingMiscData("AI", "Gets speed limit of specific drone.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float DroneGetSpeedLimit(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				return myRemoteControl.AutomaticBehaviour.SpeedLimit;
			}
			return 0f;
		}

		[VisualScriptingMiscData("AI", "Returns true if drone is in ambush mode, false otherwise.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool DroneIsInAmbushMode(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				return myRemoteControl.AutomaticBehaviour.Ambushing;
			}
			return false;
		}

		[VisualScriptingMiscData("AI", "Returns true if specific drone has both working remoteand have operational AI behavior, false otherwise.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool DroneIsOperational(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				if (myRemoteControl.IsWorking)
				{
					return myRemoteControl.AutomaticBehaviour.Operational;
				}
				return false;
			}
			return false;
		}

		[VisualScriptingMiscData("AI", "Adds specific waypoint to specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneWaypointAdd(string entityName, MyEntity waypoint)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.WaypointAdd(waypoint);
			}
		}

		[VisualScriptingMiscData("AI", "Enables/disables waypoint cycling for specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneWaypointSetCycling(string entityName, bool cycleWaypoints = true)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.CycleWaypoints = cycleWaypoints;
			}
		}

		[VisualScriptingMiscData("AI", "Sets player targeting priority of specific drone. (All player controlled entities will be considered a target if priority is greater than 0)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetPlayerPriority(string entityName, int priority)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.PlayerPriority = priority;
			}
		}

		[VisualScriptingMiscData("AI", "Sets target prioritization for specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetPrioritizationStyle(string entityName, TargetPrioritization style)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.PrioritizationStyle = style;
			}
		}

		[VisualScriptingMiscData("AI", "Deletes all waypoints of specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneWaypointClear(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.WaypointClear();
			}
		}

		[VisualScriptingMiscData("AI", "Adds specific entity into targets of specific drone. Priority specifies order in which targets will be dealt with (higher is more important).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneTargetAdd(string entityName, MyEntity target, int priority = 1)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl == null || myRemoteControl.AutomaticBehaviour == null)
			{
				return;
			}
			if (target is MyCubeGrid)
			{
				foreach (DroneTarget item in DroneProcessTargets(new List<MyEntity> { target }))
				{
					myRemoteControl.AutomaticBehaviour.TargetAdd(item);
				}
			}
			else
			{
				myRemoteControl.AutomaticBehaviour.TargetAdd(new DroneTarget(target, priority));
			}
		}

		[VisualScriptingMiscData("AI", "Clears all targets of specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneTargetClear(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.TargetClear();
			}
		}

		[VisualScriptingMiscData("AI", "Removes specific entity from drone's targets", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneTargetRemove(string entityName, MyEntity target)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.TargetRemove(target);
			}
		}

		[VisualScriptingMiscData("AI", "Sets current target of specific drone to none.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneTargetLoseCurrent(string entityName)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.TargetLoseCurrent();
			}
		}

		[VisualScriptingMiscData("AI", "Enables/disables collision avoidance for specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetCollisionAvoidance(string entityName, bool collisionAvoidance = true)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.SetCollisionAvoidance(collisionAvoidance);
				myRemoteControl.AutomaticBehaviour.CollisionAvoidance = collisionAvoidance;
			}
		}

		[VisualScriptingMiscData("AI", "Sets origin point of specific drone. (Once non-kamikaze drone has no weapons, it will retreat to that point.)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetRetreatPosition(string entityName, Vector3D position)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.OriginPoint = position;
			}
		}

		[VisualScriptingMiscData("AI", "Enables/disables if drone should rotate toward it's target.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DroneSetRotateToTarget(string entityName, bool rotateToTarget = true)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null && myRemoteControl.AutomaticBehaviour != null)
			{
				myRemoteControl.AutomaticBehaviour.RotateToTarget = rotateToTarget;
			}
		}

		[VisualScriptingMiscData("AI", "Adds grid with specific name into drone's targets.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGridToTargetList(string gridName, string targetGridname)
		{
			if (TryGetGrid(gridName, out var grid) && TryGetGrid(targetGridname, out var grid2))
			{
				grid.TargetingAddId(grid2.EntityId);
			}
		}

		[VisualScriptingMiscData("AI", "Removes specific grid from drone's targets.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveGridFromTargetList(string gridName, string targetGridname)
		{
			if (TryGetGrid(gridName, out var grid) && TryGetGrid(targetGridname, out var grid2))
			{
				grid.TargetingRemoveId(grid2.EntityId);
			}
		}

		[VisualScriptingMiscData("AI", "Sets whitelist targeting mode. If true, entities in whitelist will be considered a target, if false, entities not in whitelist will be considered a target.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void TargetingSetWhitelist(string gridName, bool whitelistMode = true)
		{
			if (TryGetGrid(gridName, out var grid))
			{
				grid.TargetingSetWhitelist(whitelistMode);
			}
		}

		[VisualScriptingMiscData("AI", "Enables drone's autopilot, sets it to one-way go to waypoint and adds that one waypoint. All previous waypoints will be cleared.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotGoToPosition(string entityName, Vector3D position, string waypointName = "Waypoint", float speedLimit = 120f, bool collisionAvoidance = true, bool precisionMode = false)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null)
			{
				myRemoteControl.SetCollisionAvoidance(collisionAvoidance);
				myRemoteControl.SetAutoPilotSpeedLimit(speedLimit);
				myRemoteControl.ChangeFlightMode(FlightMode.OneWay);
				myRemoteControl.SetDockingMode(precisionMode);
				myRemoteControl.ClearWaypoints();
				myRemoteControl.AddWaypoint(position, waypointName);
				myRemoteControl.SetAutoPilotEnabled(enabled: true);
			}
		}

		[VisualScriptingMiscData("AI", "Clears all waypoints of specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotClearWaypoints(string entityName)
		{
			DroneGetRemote(entityName)?.ClearWaypoints();
		}

		[VisualScriptingMiscData("AI", "Adds new waypoint for specific drone.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotAddWaypoint(string entityName, Vector3D position, string waypointName = "Waypoint")
		{
			DroneGetRemote(entityName)?.AddWaypoint(position, waypointName);
		}

		[VisualScriptingMiscData("AI", "Adds list of waypoints to specific drone. All waypoints will be called 'waypointName' followed by space and number. (given by order, starts with 1)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotSetWaypoints(string entityName, List<Vector3D> positions, string waypointName = "Waypoint")
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl == null)
			{
				return;
			}
			myRemoteControl.ClearWaypoints();
			if (positions != null)
			{
				for (int i = 0; i < positions.Count; i++)
				{
					myRemoteControl.AddWaypoint(positions[i], waypointName + " " + (i + 1));
				}
			}
		}

		[VisualScriptingMiscData("AI", "Enables/disables autopilot of specific drone", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotEnabled(string entityName, bool enabled = true)
		{
			DroneGetRemote(entityName)?.SetAutoPilotEnabled(enabled);
		}

		[VisualScriptingMiscData("AI", "Activates autopilot of specific drone and set all required parameters. Waypoints will not be cleared.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AutopilotActivate(string entityName, FlightMode mode = FlightMode.OneWay, float speedLimit = 120f, bool collisionAvoidance = true, bool precisionMode = false)
		{
			MyRemoteControl myRemoteControl = DroneGetRemote(entityName);
			if (myRemoteControl != null)
			{
				myRemoteControl.SetCollisionAvoidance(collisionAvoidance);
				myRemoteControl.SetAutoPilotSpeedLimit(speedLimit);
				myRemoteControl.ChangeFlightMode(mode);
				myRemoteControl.SetDockingMode(precisionMode);
				myRemoteControl.SetAutoPilotEnabled(enabled: true);
			}
		}

		[VisualScriptingMiscData("Audio", "Plays specific music cue.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void MusicPlayMusicCue(string cueName, bool playAtLeastOnce = true)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MusicPlayMusicCueSync, cueName, playAtLeastOnce);
		}

		[Event(null, 1135)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void MusicPlayMusicCueSync(string cueName, bool playAtLeastOnce = true)
		{
			if (MyAudio.Static == null)
			{
				return;
			}
			if (MyMusicController.Static == null)
			{
				MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
				MyAudio.Static.MusicAllowed = false;
				MyMusicController.Static.Active = true;
			}
			MyCueId cueId = MyAudio.Static.GetCueId(cueName);
			if (!cueId.IsNull)
			{
				MySoundData cue = MyAudio.Static.GetCue(cueId);
				if (cue == null || cue.Category.Equals(MUSIC))
				{
					MyMusicController.Static.PlaySpecificMusicTrack(cueId, playAtLeastOnce);
				}
			}
		}

		public static void MusicPlayMusicCueLocal(string cueName, bool playAtLeastOnce = true)
		{
			MusicPlayMusicCueSync(cueName, playAtLeastOnce);
		}

		[VisualScriptingMiscData("Audio", "Sets currently selected category to specific category and play a track from it.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void MusicPlayMusicCategory(string categoryName, bool playAtLeastOnce = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MusicPlayMusicCategorySync, categoryName, playAtLeastOnce, arg);
		}

		[Event(null, 1182)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void MusicPlayMusicCategorySync(string categoryName, bool playAtLeastOnce = true, long playerId = -1L)
		{
			if (MyAudio.Static != null && (playerId == -1 || MySession.Static.LocalPlayerId == playerId))
			{
				if (MyMusicController.Static == null)
				{
					MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
					MyAudio.Static.MusicAllowed = false;
					MyMusicController.Static.Active = true;
				}
				MyStringId orCompute = MyStringId.GetOrCompute(categoryName);
				if (orCompute.Id != 0)
				{
					MyMusicController.Static.PlaySpecificMusicCategory(orCompute, playAtLeastOnce);
				}
			}
		}

		public static void MusicPlayMusicCategoryLocal(string categoryName, bool playAtLeastOnce = true)
		{
			MusicPlayMusicCategorySync(categoryName, playAtLeastOnce, -1L);
		}

		[VisualScriptingMiscData("Audio", "Sets currently selected category to specific music category.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void MusicSetMusicCategory(string categoryName)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MusicSetMusicCategorySync, categoryName);
		}

		[Event(null, 1219)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void MusicSetMusicCategorySync(string categoryName)
		{
			if (MyAudio.Static != null)
			{
				if (MyMusicController.Static == null)
				{
					MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
					MyAudio.Static.MusicAllowed = false;
					MyMusicController.Static.Active = true;
				}
				MyStringId orCompute = MyStringId.GetOrCompute(categoryName);
				if (orCompute.Id != 0)
				{
					MyMusicController.Static.SetSpecificMusicCategory(orCompute);
				}
			}
		}

		public static void MusicSetMusicCategoryLocal(string categoryName)
		{
			MusicSetMusicCategorySync(categoryName);
		}

		[VisualScriptingMiscData("Audio", "Enables/disables dynamic music category changes.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void MusicSetDynamicMusic(bool enabled)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MusicSetDynamicMusicSync, enabled);
		}

		[Event(null, 1251)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void MusicSetDynamicMusicSync(bool enabled)
		{
			if (MyAudio.Static != null)
			{
				if (MyMusicController.Static == null)
				{
					MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
					MyAudio.Static.MusicAllowed = false;
					MyMusicController.Static.Active = true;
				}
				MyMusicController.Static.CanChangeCategoryGlobal = enabled;
			}
		}

		public static void MusicSetDynamicMusicLocal(bool enabled)
		{
			MusicSetDynamicMusicSync(enabled);
		}

		[VisualScriptingMiscData("Audio", "Plays single sound on emitter attached to specific entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlaySingleSoundAtEntity(string soundName, string entityName)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PlaySingleSoundAtEntitySync, soundName, entityName);
		}

		[Event(null, 1279)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlaySingleSoundAtEntitySync(string soundName, string entityName)
		{
			if (MyAudio.Static == null || soundName.Length <= 0)
			{
				return;
			}
			MySoundPair mySoundPair = new MySoundPair(soundName);
			if (mySoundPair == MySoundPair.Empty)
			{
				return;
			}
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName != null)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.Entity = entityByName;
					myEntity3DSoundEmitter.PlaySound(mySoundPair);
				}
			}
		}

		public static void PlaySingleSoundAtEntityLocal(string soundName, string entityName)
		{
			PlaySingleSoundAtEntitySync(soundName, entityName);
		}

		[VisualScriptingMiscData("Audio", "Plays specific 2D HUD sound.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlayHudSound(MyGuiSounds sound = MyGuiSounds.HudClick, long playerId = 0L)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PlayHudSoundSync, sound, playerId);
		}

		[Event(null, 1316)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlayHudSoundSync(MyGuiSounds sound = MyGuiSounds.HudClick, long playerId = 0L)
		{
			if (MyAudio.Static != null && ((MySession.Static != null && MySession.Static.LocalPlayerId == playerId) || playerId == 0L))
			{
				MyGuiAudio.PlaySound(sound);
			}
		}

		public static void PlayHudSoundLocal(MyGuiSounds sound = MyGuiSounds.HudClick, long playerId = 0L)
		{
			PlayHudSoundSync(sound, playerId);
		}

		[VisualScriptingMiscData("Audio", "Plays specific 3D sound at specific point.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlaySingleSoundAtPosition([Nullable] string soundName, Vector3D position)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PlaySingleSoundAtPositionSync, soundName, position);
		}

		[Event(null, 1339)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlaySingleSoundAtPositionSync([Nullable] string soundName, Vector3D position)
		{
			if (MyAudio.Static == null || soundName.Length <= 0)
			{
				return;
			}
			MySoundPair mySoundPair = new MySoundPair(soundName);
			if (mySoundPair != MySoundPair.Empty)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.SetPosition(position);
					myEntity3DSoundEmitter.PlaySound(mySoundPair);
				}
			}
		}

		public static void PlaySingleSoundAtPositionLocal(string soundName, Vector3D position)
		{
			PlaySingleSoundAtPositionSync(soundName, position);
		}

		[VisualScriptingMiscData("Audio", "Creates new 3D sound emitter at entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateSoundEmitterAtEntity([Nullable] string newEmitterId, string entityName)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateSoundEmitterAtEntitySync, newEmitterId, entityName);
		}

		[Event(null, 1373)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void CreateSoundEmitterAtEntitySync([Nullable] string newEmitterId, string entityName)
		{
			if (MyAudio.Static != null && newEmitterId != null && newEmitterId.Length > 0)
			{
				MyEntity entityByName = GetEntityByName(entityName);
				if (entityByName != null)
				{
					MyAudioComponent.CreateNewLibraryEmitter(newEmitterId, entityByName);
				}
			}
		}

		public static void CreateSoundEmitterAtEntityLocal(string newEmitterId, string entityName)
		{
			CreateSoundEmitterAtEntitySync(newEmitterId, entityName);
		}

		[VisualScriptingMiscData("Audio", "Creates new 3D sound emitter at specific location.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateSoundEmitterAtPosition([Nullable] string newEmitterId, Vector3D position)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateSoundEmitterAtPositionSync, newEmitterId, position);
		}

		[Event(null, 1405)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void CreateSoundEmitterAtPositionSync([Nullable] string newEmitterId, Vector3D position)
		{
			if (MyAudio.Static != null && newEmitterId != null && newEmitterId.Length > 0)
			{
				MyAudioComponent.CreateNewLibraryEmitter(newEmitterId)?.SetPosition(position);
			}
		}

		public static void CreateSoundEmitterAtPositionLocal([Nullable] string newEmitterId, Vector3D position)
		{
			CreateSoundEmitterAtPositionSync(newEmitterId, position);
		}

		[VisualScriptingMiscData("Audio", "Plays sound on specific emitter. If 'playIn2D' is true, sound will be forced 2D.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlaySound([Nullable] string emitterId, string soundName, bool playIn2D = false)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PlaySoundSync, emitterId, soundName, playIn2D);
		}

		[Event(null, 1434)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlaySoundSync([Nullable] string emitterId, string soundName, bool playIn2D = false)
		{
			if (MyAudio.Static != null && emitterId != null && emitterId.Length > 0)
			{
				MySoundPair mySoundPair = new MySoundPair(soundName);
				if (mySoundPair != MySoundPair.Empty)
				{
					MyAudioComponent.GetLibraryEmitter(emitterId)?.PlaySound(mySoundPair, stopPrevious: true, skipIntro: false, playIn2D);
				}
			}
		}

		public static void PlaySoundLocal([Nullable] string emitterId, string soundName, bool playIn2D = false)
		{
			PlaySoundSync(emitterId, soundName, playIn2D);
		}

		[VisualScriptingMiscData("Audio", "Stops sound played by specific emitter.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StopSound([Nullable] string emitterId, bool forced = false)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StopSoundSync, emitterId, forced);
		}

		[Event(null, 1469)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void StopSoundSync([Nullable] string emitterId, bool forced = false)
		{
			if (MyAudio.Static != null && emitterId != null && emitterId.Length > 0)
			{
				MyAudioComponent.GetLibraryEmitter(emitterId)?.StopSound(forced);
			}
		}

		public static void StopSoundLocal(string emitterId, bool forced = false)
		{
			StopSoundSync(emitterId, forced);
		}

		[VisualScriptingMiscData("Audio", "Removes specific sound emitter.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveSoundEmitter([Nullable] string emitterId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveSoundEmitterSync, emitterId);
		}

		[Event(null, 1499)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveSoundEmitterSync([Nullable] string emitterId)
		{
			if (MyAudio.Static != null && emitterId != null && emitterId.Length > 0)
			{
				MyAudioComponent.RemoveLibraryEmitter(emitterId);
			}
		}

		public static void RemoveSoundEmitterLocal(string emitterId)
		{
			RemoveSoundEmitterSync(emitterId);
		}

		[VisualScriptingMiscData("Audio", "Play ambient 2D sound.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlaySoundAmbient(string soundName)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PlaySoundAmbientSync, soundName);
		}

		[Event(null, 1527)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlaySoundAmbientSync(string soundName)
		{
			if (MyAudio.Static != null && MySession.Static.LocalCharacter != null)
<<<<<<< HEAD
=======
			{
				MySoundPair mySoundPair = new MySoundPair(soundName);
				if (mySoundPair != MySoundPair.Empty)
				{
					MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
					myEntity3DSoundEmitter.Entity = MySession.Static.LocalCharacter;
					myEntity3DSoundEmitter?.PlaySound(mySoundPair, stopPrevious: false, skipIntro: false, force2D: true, alwaysHearOnRealistic: true);
				}
			}
		}

		public static void PlaySoundAmbientLocal(string soundName)
		{
			PlaySoundAmbientSync(soundName);
		}

		[VisualScriptingMiscData("Audio", "Set volume for game audio. Use -1 for no change. 0 min, 1 max.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetVolume(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetVolumeSync, gameVolume, musicVolume, voiceChatVolume);
		}

		[Event(null, 1568)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetVolumeSync(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
		{
			if (MyAudio.Static != null)
			{
				if (gameVolume != -1f)
				{
					MyAudio.Static.VolumeGame = gameVolume;
				}
				if (musicVolume != -1f)
				{
					MyAudio.Static.VolumeMusic = musicVolume;
				}
				if (voiceChatVolume != -1f)
				{
					MyAudio.Static.VolumeVoiceChat = voiceChatVolume;
				}
			}
		}

		public static void SetVolumeLocal(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
		{
			SetVolumeSync(gameVolume, musicVolume, voiceChatVolume);
		}

		[VisualScriptingMiscData("Audio", "Get game volume.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetGameVolume()
		{
			if (MyAudio.Static == null)
			{
				return 0f;
			}
			return MyAudio.Static.VolumeGame;
		}

		[VisualScriptingMiscData("Audio", "Get music volume.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetMusicVolume()
		{
			if (MyAudio.Static == null)
			{
				return 0f;
			}
			return MyAudio.Static.VolumeMusic;
		}

		[VisualScriptingMiscData("Audio", "Get voice chat volume.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetVoiceChatVolume()
		{
			if (MyAudio.Static == null)
			{
				return 0f;
			}
			return MyAudio.Static.VolumeVoiceChat;
		}

		[VisualScriptingMiscData("Blocks Generic", "Enables functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EnableBlock(string blockName)
		{
			SetBlockState(blockName, state: true);
		}

		[VisualScriptingMiscData("Blocks Generic", "Disables functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DisableBlock(string blockName)
		{
			SetBlockState(blockName, state: false);
		}

		[VisualScriptingMiscData("Blocks Generic", "Return true if 'secondBlock' is reachable from 'firstBlock'. (Can be only onle-way) ", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsConveyorConnected(string firstBlock, string secondBlock)
		{
			if (firstBlock.Equals(secondBlock))
			{
				return true;
			}
			if (MyEntities.TryGetEntityByName(firstBlock, out var entity))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MySoundPair mySoundPair = new MySoundPair(soundName);
				if (mySoundPair != MySoundPair.Empty)
				{
					MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
					myEntity3DSoundEmitter.Entity = MySession.Static.LocalCharacter;
					myEntity3DSoundEmitter?.PlaySound(mySoundPair, stopPrevious: false, skipIntro: false, force2D: true, alwaysHearOnRealistic: true);
				}
			}
		}

		public static void PlaySoundAmbientLocal(string soundName)
		{
<<<<<<< HEAD
			PlaySoundAmbientSync(soundName);
=======
			if (MyEntities.TryGetEntityByName(name, out var entity) && entity is MyFunctionalBlock)
			{
				(entity as MyFunctionalBlock).Enabled = state;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[VisualScriptingMiscData("Audio", "Set volume for game audio. Use -1 for no change. 0 min, 1 max.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetVolume(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetVolumeSync, gameVolume, musicVolume, voiceChatVolume);
		}

		[Event(null, 1568)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetVolumeSync(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
		{
			if (MyAudio.Static != null)
			{
				if (gameVolume != -1f)
				{
					MyAudio.Static.VolumeGame = gameVolume;
				}
				if (musicVolume != -1f)
				{
					MyAudio.Static.VolumeMusic = musicVolume;
				}
				if (voiceChatVolume != -1f)
				{
					MyAudio.Static.VolumeVoiceChat = voiceChatVolume;
				}
			}
		}

		public static void SetVolumeLocal(float gameVolume = -1f, float musicVolume = -1f, float voiceChatVolume = -1f)
		{
			SetVolumeSync(gameVolume, musicVolume, voiceChatVolume);
		}

		[VisualScriptingMiscData("Audio", "Get game volume.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetGameVolume()
		{
			if (MyAudio.Static == null)
			{
				return 0f;
			}
			return MyAudio.Static.VolumeGame;
		}

		[VisualScriptingMiscData("Audio", "Get music volume.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetMusicVolume()
		{
			if (MyAudio.Static == null)
			{
				return 0f;
			}
			return MyAudio.Static.VolumeMusic;
		}

		[VisualScriptingMiscData("Audio", "Get voice chat volume.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetVoiceChatVolume()
		{
			if (MyAudio.Static == null)
			{
				return 0f;
			}
			return MyAudio.Static.VolumeVoiceChat;
		}

		[VisualScriptingMiscData("Blocks Generic", "Enables functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EnableBlock(string blockName)
		{
			SetBlockState(blockName, state: true);
		}

		[VisualScriptingMiscData("Blocks Generic", "Disables functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DisableBlock(string blockName)
		{
			SetBlockState(blockName, state: false);
		}

		[VisualScriptingMiscData("Blocks Generic", "Return true if 'secondBlock' is reachable from 'firstBlock'. (Can be only onle-way) ", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsConveyorConnected(string firstBlock, string secondBlock)
		{
			if (firstBlock.Equals(secondBlock))
			{
				return true;
			}
			if (MyEntities.TryGetEntityByName(firstBlock, out var entity))
			{
				IMyConveyorEndpointBlock myConveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null && MyEntities.TryGetEntityByName(secondBlock, out entity))
				{
					IMyConveyorEndpointBlock myConveyorEndpointBlock2 = entity as IMyConveyorEndpointBlock;
					if (myConveyorEndpointBlock2 != null)
					{
						return MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint);
					}
				}
			}
			return false;
		}

		private static void SetBlockState(string name, bool state)
		{
			if (MyEntities.TryGetEntityByName(name, out var entity) && entity is MyFunctionalBlock)
			{
				(entity as MyFunctionalBlock).Enabled = state;
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Enables/disables functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockEnabled(string blockName, bool enabled = true)
		{
			SetBlockState(blockName, enabled);
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets custom name of specific terminal block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockCustomName(string blockName, string newName)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity) && entity is MyTerminalBlock)
			{
				(entity as MyTerminalBlock).SetCustomName(newName);
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets whether or not terminal block should be shown in terminal screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockShowInTerminal(string blockName, bool showInTerminal = true)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity) && entity is MyTerminalBlock)
			{
				(entity as MyTerminalBlock).ShowInTerminal = showInTerminal;
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets whether or not terminal block should be shown in inventory terminal screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockShowInInventory(string blockName, bool showInInventory = true)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity) && entity is MyTerminalBlock)
			{
				(entity as MyTerminalBlock).ShowInInventory = showInInventory;
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets whether or not terminal block should be seen in HUD.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockShowOnHUD(string blockName, bool showOnHUD = true)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity) && entity is MyTerminalBlock)
			{
				(entity as MyTerminalBlock).ShowOnHUD = showOnHUD;
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns true if specific cube block exists and is in functional state, otherwise false.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsBlockFunctional(string name)
		{
			if (MyEntities.TryGetEntityByName(name, out var entity) && entity is MyCubeBlock)
			{
				return (entity as MyCubeBlock).IsFunctional;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns true if specific cube block exists and is in functional state, otherwise false. Access block by Id", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsBlockFunctionalById(long id)
		{
			if (MyEntities.TryGetEntityById(id, out var entity) && entity is MyCubeBlock)
			{
				return (entity as MyCubeBlock).IsFunctional;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns true if specific functional block exist and is powered, otherwise false. This does not relate to Enabled status, it indicates that there is power available for the block. ", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsBlockPowered(string name)
		{
			if (MyEntities.TryGetEntityByName(name, out var entity) && entity is MyFunctionalBlock)
			{
				if ((entity as MyFunctionalBlock).ResourceSink != null)
				{
					return (entity as MyFunctionalBlock).ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
				}
				return false;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns true if functional block exists and is enabled, otherwise false.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsBlockEnabled(string name)
		{
			if (MyEntities.TryGetEntityByName(name, out var entity) && entity is MyFunctionalBlock)
			{
				return (entity as MyFunctionalBlock).Enabled;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns true if specific functional block exists and is working, otherwise false.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsBlockWorking(string name)
		{
			if (MyEntities.TryGetEntityByName(name, out var entity) && entity is MyFunctionalBlock)
			{
				return (entity as MyFunctionalBlock).IsWorking;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets damage multiplier for specific block. (Value above 1 increase damage taken by the block, values in range <0;1> decrease damage taken. )", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockGeneralDamageModifier(string blockName, float modifier = 1f)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity) && entity is MyCubeBlock)
			{
				((MyCubeBlock)entity).SlimBlock.BlockGeneralDamageModifier = modifier;
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns grid EntityId of grid that contains block with specific name. Returns 0 if name does not refer to a cube block. (If more entities have same name, only the first one created will be tested.)", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetGridIdOfBlock(string entityName)
		{
			return (GetEntityByName(entityName) as MyCubeBlock)?.CubeGrid.EntityId ?? 0;
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns current integrity of block in interval <0;1>.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetBlockHealth(string entityName, bool buildIntegrity = true)
		{
			MyCubeBlock myCubeBlock = GetEntityByName(entityName) as MyCubeBlock;
			if (myCubeBlock != null)
			{
				if (buildIntegrity)
				{
					return myCubeBlock.SlimBlock.BuildIntegrity;
				}
				return myCubeBlock.SlimBlock.Integrity;
			}
			return 0f;
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets block integrity to specific value in range <0;1>. 'damageChange' says if the change is treated as damage or repair (Build integrity won't change in case of damage). 'changeOwner' is id of the one who causes the change.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetBlockHealth(string entityName, float integrity = 1f, bool damageChange = true, long changeOwner = 0L)
		{
			MyCubeBlock myCubeBlock = GetEntityByName(entityName) as MyCubeBlock;
			if (myCubeBlock != null)
			{
				if (damageChange)
				{
					myCubeBlock.SlimBlock.SetIntegrity(myCubeBlock.SlimBlock.BuildIntegrity, integrity, MyIntegrityChangeEnum.Damage, changeOwner);
				}
				else
				{
					myCubeBlock.SlimBlock.SetIntegrity(integrity, integrity, MyIntegrityChangeEnum.Repair, changeOwner);
				}
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Applies damage to specific block from specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DamageBlock(string entityName, float damage = 0f, long damageOwner = 0L)
		{
			(GetEntityByName(entityName) as MyCubeBlock)?.SlimBlock.DoDamage(damage, MyDamageType.Destruction, sync: true, null, damageOwner, 0L);
		}

		[VisualScriptingMiscData("Blocks Generic", "Returns ids of attached modules. Output parameters will contain additional informations.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetBlockAttachedUpgradeModules(string blockName, out int modulesCount, out int workingCount, out int slotsUsed, out int slotsTotal, out int incompatibleCount)
		{
			List<long> list = new List<long>();
			modulesCount = 0;
			workingCount = 0;
			slotsUsed = 0;
			slotsTotal = 0;
			incompatibleCount = 0;
			MyEntities.TryGetEntityByName(blockName, out var entity);
			if (entity == null)
			{
				return list;
			}
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				slotsTotal = myCubeBlock.GetComponent().ConnectionPositions.get_Count();
				if (myCubeBlock.CurrentAttachedUpgradeModules != null)
				{
					modulesCount = myCubeBlock.CurrentAttachedUpgradeModules.Count;
					lock (myCubeBlock.CurrentAttachedUpgradeModules)
					{
						foreach (MyCubeBlock.AttachedUpgradeModule value in myCubeBlock.CurrentAttachedUpgradeModules.Values)
						{
							list.Add(value.Block.EntityId);
							slotsUsed += value.SlotCount;
							incompatibleCount += ((!value.Compatible) ? 1 : 0);
							workingCount += ((value.Compatible && value.Block.IsWorking) ? 1 : 0);
						}
						return list;
					}
				}
			}
			return list;
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets color of specific block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ColorBlock(string blockName, Color color)
		{
			MyEntity entityByName = GetEntityByName(blockName);
			if (entityByName != null)
			{
				MyCubeBlock myCubeBlock = entityByName as MyCubeBlock;
				if (myCubeBlock != null)
				{
					Vector3 value = color.ColorToHSVDX11();
					myCubeBlock.CubeGrid.ChangeColorAndSkin(myCubeBlock.SlimBlock, value);
				}
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets skin of specific block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SkinBlock(string blockName, string skin)
		{
			MyEntity entityByName = GetEntityByName(blockName);
			if (entityByName != null)
			{
				MyCubeBlock myCubeBlock = entityByName as MyCubeBlock;
				myCubeBlock?.CubeGrid.ChangeColorAndSkin(myCubeBlock.SlimBlock, null, MyStringHash.GetOrCompute(skin));
			}
		}

		[VisualScriptingMiscData("Blocks Generic", "Sets color and skin of specific block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ColorAndSkinBlock(string blockName, Color color, string skin)
		{
			MyEntity entityByName = GetEntityByName(blockName);
			if (entityByName != null)
			{
				MyCubeBlock myCubeBlock = entityByName as MyCubeBlock;
				if (myCubeBlock != null)
				{
					Vector3 value = color.ColorToHSVDX11();
					myCubeBlock.CubeGrid.ChangeColorAndSkin(myCubeBlock.SlimBlock, value, MyStringHash.GetOrCompute(skin));
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Returns merge block status ( -1 - block don't exist, 2 - Locked, 1 - Constrained, 0 - Otherwise).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetMergeBlockStatus(string mergeBlockName)
		{
			MyEntities.TryGetEntityByName(mergeBlockName, out var entity);
			if (entity == null)
			{
				return -1;
			}
			return (entity as MyFunctionalBlock)?.GetBlockSpecificState() ?? (-1);
		}

		[VisualScriptingMiscData("Blocks Specific", "Orders specific weapon block (UserControllableGun) to shoot once.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void WeaponShootOnce(string weaponName)
		{
			if (MyEntities.TryGetEntityByName(weaponName, out var entity) && entity is MyUserControllableGun)
			{
				(entity as MyUserControllableGun).ShootFromTerminal(entity.WorldMatrix.Forward);
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Turns on/off shooting for specific weapon block (UserControllableGun)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void WeaponSetShooting(string weaponName, bool shooting = true)
		{
			if (MyEntities.TryGetEntityByName(weaponName, out var entity) && entity is MyUserControllableGun)
			{
				(entity as MyUserControllableGun).SetShooting(shooting);
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Calls 'Start' action on specific functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StartTimerBlock(string blockName)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity))
			{
				Sandbox.ModAPI.IMyFunctionalBlock myFunctionalBlock = entity as Sandbox.ModAPI.IMyFunctionalBlock;
				if (myFunctionalBlock != null)
				{
					TerminalActionExtensions.ApplyAction(myFunctionalBlock, "Start");
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets lock state of specific Landing gear.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetLandingGearLock(string entityName, bool locked = true)
		{
			(GetEntityByName(entityName) as IMyLandingGear)?.RequestLock(locked);
		}

		[VisualScriptingMiscData("Blocks Specific", "Returns true if Landing gear is locked, false otherwise.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsLandingGearLocked(string entityName)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
			{
				return false;
			}
			IMyLandingGear myLandingGear = entityByName as IMyLandingGear;
			if (myLandingGear != null && myLandingGear.LockMode == LandingGearMode.Locked)
			{
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Specific", "Gets information about specific landing gear. Returns true if informations were obtained, false if no such Landing gear exists.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetLandingGearInformation(string entityName, out bool locked, out bool inConstraint, out string attachedType, out string attachedName)
		{
			locked = false;
			inConstraint = false;
			attachedType = "";
			attachedName = "";
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
			{
				return false;
			}
			IMyLandingGear myLandingGear = entityByName as IMyLandingGear;
			if (myLandingGear != null)
			{
				locked = myLandingGear.LockMode == LandingGearMode.Locked;
				inConstraint = myLandingGear.LockMode == LandingGearMode.ReadyToLock;
				if (locked)
				{
					MyEntity myEntity = myLandingGear.GetAttachedEntity() as MyEntity;
					if (myEntity != null)
					{
						attachedType = ((myEntity is MyCubeBlock) ? "Block" : ((myEntity is MyCubeGrid) ? "Grid" : ((myEntity is MyVoxelBase) ? "Voxel" : "Other")));
						attachedName = myEntity.Name;
					}
				}
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Specific", "Gets information about specific landing gear. Returns true if informations were obtained, false if entity is not a Landing gear.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetLandingGearInformationFromEntity(MyEntity entity, out bool locked, out bool inConstraint, out string attachedType, out string attachedName)
		{
			locked = false;
			inConstraint = false;
			attachedType = "";
			attachedName = "";
			if (entity == null)
			{
				return false;
			}
			IMyLandingGear myLandingGear = entity as IMyLandingGear;
			if (myLandingGear != null)
			{
				locked = myLandingGear.LockMode == LandingGearMode.Locked;
				inConstraint = myLandingGear.LockMode == LandingGearMode.ReadyToLock;
				if (locked)
				{
					MyEntity myEntity = myLandingGear.GetAttachedEntity() as MyEntity;
					if (myEntity != null)
					{
						attachedType = ((myEntity is MyCubeBlock) ? "Block" : ((myEntity is MyCubeGrid) ? "Grid" : ((myEntity is MyVoxelBase) ? "Voxel" : "Other")));
						attachedName = myEntity.Name;
					}
				}
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Specific", "Returns true if specific connector is locked. False if unlocked of no such connector exists.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsConnectorLocked(string connectorName)
		{
			MyEntity entityByName = GetEntityByName(connectorName);
			if (entityByName == null)
			{
				return false;
			}
			Sandbox.ModAPI.IMyShipConnector myShipConnector = entityByName as Sandbox.ModAPI.IMyShipConnector;
			if (myShipConnector != null)
			{
				return myShipConnector.Status == MyShipConnectorStatus.Connected;
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Specific", "Calls 'Stop' action on specific functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StopTimerBlock(string blockName)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity))
			{
				Sandbox.ModAPI.IMyFunctionalBlock myFunctionalBlock = entity as Sandbox.ModAPI.IMyFunctionalBlock;
				if (myFunctionalBlock != null)
				{
					TerminalActionExtensions.ApplyAction(myFunctionalBlock, "Stop");
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Calls 'TriggerNow' action on specific functional block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void TriggerTimerBlock(string blockName)
		{
			if (MyEntities.TryGetEntityByName(blockName, out var entity))
			{
				Sandbox.ModAPI.IMyFunctionalBlock myFunctionalBlock = entity as Sandbox.ModAPI.IMyFunctionalBlock;
				if (myFunctionalBlock != null)
				{
					TerminalActionExtensions.ApplyAction(myFunctionalBlock, "TriggerNow");
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets specific doors to open/close state. (Doors, SlidingDoors, AirtightDoors)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ChangeDoorState(string doorBlockName, bool open = true)
		{
			if (MyEntities.TryGetEntityByName(doorBlockName, out var entity))
			{
				if (entity is MyAdvancedDoor)
				{
					(entity as MyAdvancedDoor).Open = open;
				}
				if (entity is MyAirtightDoorGeneric)
				{
					(entity as MyAirtightDoorGeneric).ChangeOpenClose(open);
				}
				if (entity is MyDoor)
				{
					(entity as MyDoor).Open = open;
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Returns true if specific doors are open false if closed or door does not exist.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsDoorOpen(string doorBlockName)
		{
			if (MyEntities.TryGetEntityByName(doorBlockName, out var entity))
			{
				if (entity is MyAdvancedDoor)
				{
					return (entity as MyAdvancedDoor).Open;
				}
				if (entity is MyAirtightDoorGeneric)
				{
					return (entity as MyAirtightDoorGeneric).Open;
				}
				if (entity is MyDoor)
				{
					return (entity as MyDoor).Open;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets description of specific Text panel.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetTextPanelDescription(string panelName, string description, bool publicDescription = true)
		{
			if (!MyEntities.TryGetEntityByName(panelName, out var entity))
			{
				return;
			}
			MyTextPanel myTextPanel = entity as MyTextPanel;
			if (myTextPanel != null)
			{
				MyMultiplayer.RaiseEvent(myTextPanel, (MyTextPanel x) => x.OnChangeDescription, MyStatControlText.SubstituteTexts(description.ToString()), publicDescription);
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets colors of specific Text panel.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetTextPanelColors(string panelName, Color fontColor, Color backgroundColor)
		{
			if (!MyEntities.TryGetEntityByName(panelName, out var entity))
			{
				return;
			}
			MyTextPanel myTextPanel = entity as MyTextPanel;
			if (myTextPanel != null)
			{
				if (fontColor != Color.Transparent)
				{
					myTextPanel.FontColor = fontColor;
				}
				if (backgroundColor != Color.Transparent)
				{
					myTextPanel.BackgroundColor = backgroundColor;
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets title of specific Text panel.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetTextPanelTitle(string panelName, string title, bool publicTitle = true)
		{
			if (!MyEntities.TryGetEntityByName(panelName, out var entity))
			{
				return;
			}
			MyTextPanel myTextPanel = entity as MyTextPanel;
			if (myTextPanel != null)
			{
				if (publicTitle)
				{
					myTextPanel.PublicDescription = new StringBuilder(title);
				}
				else
				{
					myTextPanel.PrivateDescription = new StringBuilder(title);
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Removes pilot from specific Cockpit.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CockpitRemovePilot(string cockpitName)
		{
			if (MyEntities.TryGetEntityByName(cockpitName, out var entity))
			{
				(entity as MyCockpit)?.RemovePilot();
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Forces player into specific Cockpit.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CockpitInsertPilot(string cockpitName, bool keepOriginalPlayerPosition = true, long playerId = 0L)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId == null || !MyEntities.TryGetEntityByName(cockpitName, out var entity))
			{
				return;
			}
			MyCockpit myCockpit = entity as MyCockpit;
			if (myCockpit != null)
			{
				myCockpit.RemovePilot();
				if (characterFromPlayerId.Parent is MyCockpit)
				{
					(characterFromPlayerId.Parent as MyCockpit).RemovePilot();
				}
				if (characterFromPlayerId.ControllerInfo.Controller == null)
				{
					GetPlayerFromPlayerId(playerId).Controller.TakeControl(characterFromPlayerId);
				}
				myCockpit.AttachPilot(characterFromPlayerId, keepOriginalPlayerPosition);
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Returns identity Id of player occupying cockpit or 0, if no one is in. ", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long CockpitGetPilotId(string cockpitName, out bool occupied)
		{
			occupied = false;
			if (MyEntities.TryGetEntityByName(cockpitName, out var entity))
			{
				MyCockpit myCockpit = entity as MyCockpit;
				if (myCockpit != null && myCockpit.Pilot != null)
				{
					occupied = myCockpit.Pilot != null;
					return myCockpit.Pilot.GetPlayerIdentityId();
				}
			}
			return 0L;
		}

		[VisualScriptingMiscData("Blocks Specific", "Get all cockpits in the grid", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<string> GetAllCockpits(string gridName)
		{
			List<string> list = new List<string>();
			if (MyEntities.TryGetEntityByName(gridName, out MyCubeGrid entity))
			{
<<<<<<< HEAD
				foreach (MyCockpit item in entity.GetFatBlocks().OfType<MyCockpit>())
=======
				foreach (MyCockpit item in Enumerable.OfType<MyCockpit>((IEnumerable)entity.GetFatBlocks()))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					list.Add(item.Name);
				}
				return list;
			}
			return list;
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets color of specific Lighting block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetLigtingBlockColor(string lightBlockName, Color color)
		{
			if (MyEntities.TryGetEntityByName(lightBlockName, out var entity))
			{
				MyLightingBlock myLightingBlock = entity as MyLightingBlock;
				if (myLightingBlock != null)
				{
					myLightingBlock.Color = color;
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets intensity of specific Lighting block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetLigtingBlockIntensity(string lightBlockName, float intensity)
		{
			if (MyEntities.TryGetEntityByName(lightBlockName, out var entity))
			{
				MyLightingBlock myLightingBlock = entity as MyLightingBlock;
				if (myLightingBlock != null)
				{
					myLightingBlock.Intensity = intensity;
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "Sets radius of specific Lighting block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetLigtingBlockRadius(string lightBlockName, float radius)
		{
			if (MyEntities.TryGetEntityByName(lightBlockName, out var entity))
			{
				MyLightingBlock myLightingBlock = entity as MyLightingBlock;
				if (myLightingBlock != null)
				{
					myLightingBlock.Radius = radius;
				}
			}
		}

		[VisualScriptingMiscData("Blocks Specific", "True if block is part of airtight room (Best used for AirVents).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool IsBlockPositionAirtight(string blockName)
		{
			if (!MyEntities.TryGetEntityByName(blockName, out var entity))
			{
				return false;
			}
			Sandbox.ModAPI.IMyFunctionalBlock myFunctionalBlock = entity as Sandbox.ModAPI.IMyFunctionalBlock;
			return myFunctionalBlock?.CubeGrid.IsRoomAtPositionAirtight(myFunctionalBlock.Position) ?? false;
		}

		[VisualScriptingMiscData("Cutscenes", "Starts specific cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StartCutscene(string cutsceneName, bool registerEvents = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StartCutsceneSync, cutsceneName, registerEvents, arg);
		}

		[Event(null, 2450)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void StartCutsceneSync(string cutsceneName, bool registerEvents = true, long playerId = -1L)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				if (playerId == -1 && MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer)
				{
					registerEvents = false;
				}
				MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(cutsceneName, registerEvents);
			}
		}

		public static void StartCutsceneLocal(string cutsceneName, bool registerEvents = true, long playerId = -1L)
		{
			StartCutsceneSync(cutsceneName, registerEvents, playerId);
		}

		[VisualScriptingMiscData("Cutscenes", "Goes to next node in current cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void NextCutsceneNode(long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => NextCutsceneNodeSync, arg);
		}

		[Event(null, 2476)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void NextCutsceneNodeSync(long playerId = -1L)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneNext(setToZero: true);
			}
		}

		public static void NextCutsceneNodeLocal(long playerId = -1L)
		{
			NextCutsceneNodeSync(playerId);
		}

		[VisualScriptingMiscData("Cutscenes", "Ends current cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EndCutscene(long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EndCutsceneSync, arg);
		}

		[Event(null, 2499)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void EndCutsceneSync(long playerId = -1L)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneEnd();
			}
		}

		public static void EndCutsceneLocal(long playerId = -1L)
		{
			EndCutsceneSync(playerId);
		}

		[VisualScriptingMiscData("Cutscenes", "Skips current cutscene. If 'playerId' is -1, apply for all players, otherwise only for specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SkipCutscene(long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SkipCutsceneSync, arg);
		}

		[Event(null, 2522)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SkipCutsceneSync(long playerId = -1L)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneSkip();
			}
		}

		public static void SkipCutsceneLocal(long playerId = -1L)
		{
			SkipCutsceneSync(playerId);
		}

		[VisualScriptingMiscData("Cutscenes", "Preload entity on clients to prevent streaming and popping", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RequestVicinityCache(long entityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestVicinityCacheSync, entityId);
		}

		[Event(null, 2544)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void RequestVicinityCacheSync(long entityId = -1L)
		{
			MySession.RequestVicinityCache(entityId);
			(MyMultiplayer.ReplicationLayer as MyReplicationClient)?.RequestReplicable(entityId, 0, add: true);
		}

		public static void RequestVicinityCacheLocal(long entityId = -1L)
		{
			RequestVicinityCacheSync(entityId);
		}

		[VisualScriptingMiscData("Effects", "Creates explosion at specific point with specified radius, causing damage to everything in range.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateExplosion(Vector3D position, float radius, int damage = 5000)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateExplosionInternalAll, position, radius, damage);
		}

		[Event(null, 2574)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void CreateExplosionInternalAll(Vector3D position, float radius, int damage = 5000)
		{
			MyExplosionTypeEnum explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_50;
			if (radius < 2f)
			{
				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;
			}
			else if (radius < 15f)
			{
				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_15;
			}
			else if (radius < 30f)
			{
				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_30;
			}
			MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
			myExplosionInfo.PlayerDamage = 0f;
			myExplosionInfo.Damage = damage;
			myExplosionInfo.ExplosionType = explosionType;
			myExplosionInfo.ExplosionSphere = new BoundingSphereD(position, radius);
			myExplosionInfo.LifespanMiliseconds = 700;
			myExplosionInfo.ParticleScale = 1f;
			myExplosionInfo.Direction = Vector3.Down;
			myExplosionInfo.VoxelExplosionCenter = position;
			myExplosionInfo.ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION;
			myExplosionInfo.VoxelCutoutScale = 1f;
			myExplosionInfo.PlaySound = true;
			myExplosionInfo.ApplyForceAndDamage = true;
			myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
			MyExplosionInfo explosionInfo = myExplosionInfo;
			MyExplosions.AddExplosion(ref explosionInfo);
		}

		[VisualScriptingMiscData("Effects", "Creates specific particle effect at position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateParticleEffectAtPosition(string effectName, Vector3D position)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateParticleEffectAtPositionInternalAll, effectName, position);
		}

		[Event(null, 2622)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void CreateParticleEffectAtPositionInternalAll(string effectName, Vector3D position)
		{
			MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(position), out var _);
		}

		[VisualScriptingMiscData("Effects", "Creates specific particle effect at entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateParticleEffectAtEntity(string effectName, string entityName)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateParticleEffectAtEntityInternalAll, effectName, entityName);
		}

		[Event(null, 2640)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void CreateParticleEffectAtEntityInternalAll(string effectName, string entityName)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName != null)
			{
				MyParticlesManager.TryCreateParticleEffect(effectName, entityByName.WorldMatrix, out var _);
			}
		}

		[VisualScriptingMiscData("Effects", "Fades/shows screen over period of time.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ScreenColorFadingStart(float time = 1f, bool toOpaque = true, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ScreenColorFadingStart(time, toOpaque);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingStartInternalAll, time, toOpaque);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingStartInternal, time, toOpaque, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2679)]
		[Reliable]
		[ServerInvoked]
		private static void ScreenColorFadingStartInternal(float time = 1f, bool toOpaque = true)
		{
			ScreenColorFadingStart(time, toOpaque);
		}

		[Event(null, 2685)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ScreenColorFadingStartInternalAll(float time = 1f, bool toOpaque = true)
		{
			ScreenColorFadingStart(time, toOpaque);
		}

		private static void ScreenColorFadingStart(float time = 1f, bool toOpaque = true)
		{
			MyHud.ScreenEffects.FadeScreen(toOpaque ? 0f : 1f, time);
		}

		[VisualScriptingMiscData("Effects", "Sets target color for screen fading.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ScreenColorFadingSetColor(Color color = default(Color), long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ScreenColorFadingSetColor(color);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingSetColorInternalAll, color);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingSetColorInternal, color, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2724)]
		[Reliable]
		[ServerInvoked]
		private static void ScreenColorFadingSetColorInternal(Color color)
		{
			ScreenColorFadingSetColor(color);
		}

		[Event(null, 2730)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ScreenColorFadingSetColorInternalAll(Color color)
		{
			ScreenColorFadingSetColor(color);
		}

		private static void ScreenColorFadingSetColor(Color color)
		{
			MyHud.ScreenEffects.BlackScreenColor = new Color(color, 0f);
		}

		[VisualScriptingMiscData("Effects", "Switches screen fade state. Screen will un/fade over specified time.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ScreenColorFadingStartSwitch(float time = 1f, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ScreenColorFadingStartSwitch(time);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingStartSwitchInternalAll, time);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingStartSwitchInternal, time, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2769)]
		[Reliable]
		[ServerInvoked]
		private static void ScreenColorFadingStartSwitchInternal(float time = 1f)
<<<<<<< HEAD
		{
			ScreenColorFadingStartSwitch(time);
		}

		[Event(null, 2775)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ScreenColorFadingStartSwitchInternalAll(float time = 1f)
		{
			ScreenColorFadingStartSwitch(time);
		}

		private static void ScreenColorFadingStartSwitch(float time = 1f)
		{
			MyHud.ScreenEffects.SwitchFadeScreen(time);
		}

		[VisualScriptingMiscData("Effects", "Sets if screen fade should minimize HUD.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ScreenColorFadingMinimalizeHUD(bool minimalize = true, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ScreenColorFadingMinimalizeHUD(minimalize);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingMinimalizeHUDInternalAll, minimalize);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingMinimalizeHUDInternal, minimalize, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2814)]
		[Reliable]
		[ServerInvoked]
		private static void ScreenColorFadingMinimalizeHUDInternal(bool minimalize)
		{
			ScreenColorFadingMinimalizeHUD(minimalize);
		}

		[Event(null, 2820)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ScreenColorFadingMinimalizeHUDInternalAll(bool minimalize)
		{
			ScreenColorFadingMinimalizeHUD(minimalize);
		}

		private static void ScreenColorFadingMinimalizeHUD(bool minimalize)
		{
			MyHud.ScreenEffects.BlackScreenMinimalizeHUD = minimalize;
		}

		[VisualScriptingMiscData("Effects", "False to force minimize HUD, true to disable force minimization. (Force minimization overrides HUD state without actually changing it so you can revert back safely.)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ShowHud(bool flag = true, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ShowHud(flag);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowHudInternalAll, flag);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowHudInternal, flag, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2860)]
		[Reliable]
		[ServerInvoked]
		private static void ShowHudInternal(bool flag = true)
		{
			ScreenColorFadingMinimalizeHUD(flag);
		}

		[Event(null, 2866)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ShowHudInternalAll(bool flag = true)
		{
			ShowHud(flag);
		}

		private static void ShowHud(bool flag = true)
		{
			MyHud.MinimalHud = !flag;
		}

		[VisualScriptingMiscData("Effects", "Set state of HUD to specific state. 0 - minimal hud.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetHudState(int state = 0, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				SetHudState(state);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetHudStateInternalAll, state);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetHudStateInternal, state, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2905)]
		[Reliable]
		[ServerInvoked]
		private static void SetHudStateInternal(int state)
		{
			SetHudState(state);
		}

		[Event(null, 2911)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void SetHudStateInternalAll(int state)
		{
			SetHudState(state);
		}

		private static void SetHudState(int state)
		{
			MyHud.HudState = state;
		}

		[VisualScriptingMiscData("Entity", "Gets specific entity by name. If there are more entities by same name, the first one created will be taken.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyEntity GetEntityByName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			if (MyEntities.TryGetEntityByName(name, out var entity))
			{
				return entity;
			}
			if (long.TryParse(name, out var result) && MyEntities.TryGetEntityById(result, out entity))
			{
				return entity;
			}
			return null;
		}

		[VisualScriptingMiscData("Entity", "Gets specific entity by id.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyEntity GetEntityById(long id)
		{
			if (!MyEntities.TryGetEntityById(id, out var entity))
			{
				return null;
			}
			return entity;
		}

		[VisualScriptingMiscData("Entity", "Returns entity id of specific entity ", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetEntityIdFromName(string name)
		{
			if (!MyEntities.TryGetEntityByName(name, out var entity))
			{
				return 0L;
			}
			return entity.EntityId;
		}

		[VisualScriptingMiscData("Entity", "Gets entity id from specific entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetEntityIdFromEntity(MyEntity entity)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			ScreenColorFadingStartSwitch(time);
		}

		[Event(null, 2775)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ScreenColorFadingStartSwitchInternalAll(float time = 1f)
		{
			ScreenColorFadingStartSwitch(time);
		}

		private static void ScreenColorFadingStartSwitch(float time = 1f)
		{
			MyHud.ScreenEffects.SwitchFadeScreen(time);
		}

		[VisualScriptingMiscData("Effects", "Sets if screen fade should minimize HUD.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ScreenColorFadingMinimalizeHUD(bool minimalize = true, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ScreenColorFadingMinimalizeHUD(minimalize);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingMinimalizeHUDInternalAll, minimalize);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ScreenColorFadingMinimalizeHUDInternal, minimalize, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2814)]
		[Reliable]
		[ServerInvoked]
		private static void ScreenColorFadingMinimalizeHUDInternal(bool minimalize)
		{
			ScreenColorFadingMinimalizeHUD(minimalize);
		}

		[Event(null, 2820)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ScreenColorFadingMinimalizeHUDInternalAll(bool minimalize)
		{
			ScreenColorFadingMinimalizeHUD(minimalize);
		}

		private static void ScreenColorFadingMinimalizeHUD(bool minimalize)
		{
			MyHud.ScreenEffects.BlackScreenMinimalizeHUD = minimalize;
		}

		[VisualScriptingMiscData("Effects", "False to force minimize HUD, true to disable force minimization. (Force minimization overrides HUD state without actually changing it so you can revert back safely.)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ShowHud(bool flag = true, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				ShowHud(flag);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowHudInternalAll, flag);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowHudInternal, flag, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2860)]
		[Reliable]
		[ServerInvoked]
		private static void ShowHudInternal(bool flag = true)
		{
			ScreenColorFadingMinimalizeHUD(flag);
		}

		[Event(null, 2866)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ShowHudInternalAll(bool flag = true)
		{
			ShowHud(flag);
		}

		private static void ShowHud(bool flag = true)
		{
			MyHud.MinimalHud = !flag;
		}

		[VisualScriptingMiscData("Effects", "Set state of HUD to specific state. 0 - minimal hud.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetHudState(int state = 0, long playerId = -1L)
		{
			switch (playerId)
			{
			case 0L:
				SetHudState(state);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetHudStateInternalAll, state);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetHudStateInternal, state, new EndpointId(result.SteamId));
			}
		}

		[Event(null, 2905)]
		[Reliable]
		[ServerInvoked]
		private static void SetHudStateInternal(int state)
		{
			SetHudState(state);
		}

		[Event(null, 2911)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void SetHudStateInternalAll(int state)
		{
			SetHudState(state);
		}

		private static void SetHudState(int state)
		{
			MyHud.HudState = state;
		}

		[VisualScriptingMiscData("Entity", "Gets specific entity by name. If there are more entities by same name, the first one created will be taken.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyEntity GetEntityByName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			if (MyEntities.TryGetEntityByName(name, out var entity))
			{
				return entity;
			}
			if (long.TryParse(name, out var result) && MyEntities.TryGetEntityById(result, out entity))
			{
				return entity;
			}
			return null;
		}

		[VisualScriptingMiscData("Entity", "Gets specific entity by id.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyEntity GetEntityById(long id)
		{
			if (!MyEntities.TryGetEntityById(id, out var entity))
			{
				return null;
			}
			return entity;
		}

		[VisualScriptingMiscData("Entity", "Returns entity id of specific entity ", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetEntityIdFromName(string name)
		{
			if (!MyEntities.TryGetEntityByName(name, out var entity))
			{
				return 0L;
			}
			return entity.EntityId;
		}

		[VisualScriptingMiscData("Entity", "Gets entity id from specific entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetEntityIdFromEntity(MyEntity entity)
		{
			return entity?.EntityId ?? 0;
		}

		[VisualScriptingMiscData("Entity", "Gets position of specific entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D GetEntityPosition(string entityName)
		{
			return GetEntityByName(entityName)?.PositionComp.GetPosition() ?? Vector3D.Zero;
		}

		[VisualScriptingMiscData("Entity", "Gets world matrix of specific entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MatrixD GetEntityWorldMatrix(MyEntity entity)
		{
			return entity?.WorldMatrix ?? MatrixD.Identity;
		}

		[VisualScriptingMiscData("Entity", "Breaks and returns world matrix of specific entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static void GetEntityVectors(string entityName, out Vector3D position, out Vector3D forward, out Vector3D up)
		{
			position = Vector3D.Zero;
			forward = Vector3D.Forward;
			up = Vector3D.Up;
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName != null)
			{
				position = entityByName.PositionComp.WorldMatrixRef.Translation;
				forward = entityByName.PositionComp.WorldMatrixRef.Forward;
				up = entityByName.PositionComp.WorldMatrixRef.Up;
			}
		}

		[VisualScriptingMiscData("Entity", "Sets world position of specific entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetEntityPosition(string entityName, Vector3D position)
		{
			GetEntityByName(entityName)?.PositionComp.SetPosition(position);
		}

		[VisualScriptingMiscData("Entity", "Gets vector in world coordination system representing entity's direction (e.g. Direction.Forward will return real forward vector of entity in world coordination system.)", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D GetEntityDirection(string entityName, Base6Directions.Direction direction = Base6Directions.Direction.Forward)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
			{
				return Vector3D.Forward;
			}
			return direction switch
			{
				Base6Directions.Direction.Backward => entityByName.WorldMatrix.Backward, 
				Base6Directions.Direction.Up => entityByName.WorldMatrix.Up, 
				Base6Directions.Direction.Down => entityByName.WorldMatrix.Down, 
				Base6Directions.Direction.Left => entityByName.WorldMatrix.Left, 
				Base6Directions.Direction.Right => entityByName.WorldMatrix.Right, 
				_ => entityByName.WorldMatrix.Forward, 
			};
		}

		[VisualScriptingMiscData("Entity", "Returns true if point is in natural gravity close to planet(eg. if nearest planet exists).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsPlanetNearby(Vector3D position)
		{
			if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).LengthSquared() > 0f)
			{
				return MyGamePruningStructure.GetClosestPlanet(position) != null;
			}
			return false;
		}

		[VisualScriptingMiscData("Entity", "Returns name of a planet if point is close to a plane (in its natural gravity). Else returns 'Void'. !!!BEWARE 'Void' is just for English as this string is localized. For checking if there really is a planet or not use 'IsPlanetNearby(...)' function as output here might be inconsistent between localizations.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetNearestPlanet(Vector3D position)
		{
			if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).LengthSquared() > 0f)
			{
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
				if (closestPlanet != null && closestPlanet.Generator != null)
				{
					return closestPlanet.Generator.FolderName;
				}
			}
			return MyTexts.GetString(MyCommonTexts.Void);
		}

		[VisualScriptingMiscData("Entity", "Returns List of ids of entities in defined sphere", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetEntitiesInSphere(Vector3D position, float radius)
		{
			BoundingSphereD sphere = new BoundingSphereD(position, radius);
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, list);
<<<<<<< HEAD
			return list.Select((MyEntity x) => x.EntityId).ToList();
=======
			return Enumerable.ToList<long>(Enumerable.Select<MyEntity, long>((IEnumerable<MyEntity>)list, (Func<MyEntity, long>)((MyEntity x) => x.EntityId)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[VisualScriptingMiscData("Entity", "Returns List of ids of entities in hit by a line", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetAllEntitiesInRay(Vector3D from, Vector3D to, out List<float> distances)
		{
			LineD ray = new LineD(from, to);
			List<MyLineSegmentOverlapResult<MyEntity>> list = new List<MyLineSegmentOverlapResult<MyEntity>>();
			MyGamePruningStructure.GetAllEntitiesInRay(ref ray, list);
<<<<<<< HEAD
			distances = list.Select((MyLineSegmentOverlapResult<MyEntity> x) => (float)x.Distance).ToList();
			return list.Select((MyLineSegmentOverlapResult<MyEntity> x) => x.Element.EntityId).ToList();
=======
			distances = Enumerable.ToList<float>(Enumerable.Select<MyLineSegmentOverlapResult<MyEntity>, float>((IEnumerable<MyLineSegmentOverlapResult<MyEntity>>)list, (Func<MyLineSegmentOverlapResult<MyEntity>, float>)((MyLineSegmentOverlapResult<MyEntity> x) => (float)x.Distance)));
			return Enumerable.ToList<long>(Enumerable.Select<MyLineSegmentOverlapResult<MyEntity>, long>((IEnumerable<MyLineSegmentOverlapResult<MyEntity>>)list, (Func<MyLineSegmentOverlapResult<MyEntity>, long>)((MyLineSegmentOverlapResult<MyEntity> x) => x.Element.EntityId)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[VisualScriptingMiscData("Entity", "Adds item defined by id in specific quantity into inventory of entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddToInventory(string entityname, MyDefinitionId itemId, int amount = 1)
		{
			MyEntity entityByName = GetEntityByName(entityname);
			if (entityByName != null)
			{
				MyInventoryBase inventoryBase = entityByName.GetInventoryBase();
				if (inventoryBase != null)
				{
					MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(itemId);
					MyFixedPoint myFixedPoint = default(MyFixedPoint);
					myFixedPoint = amount;
					inventoryBase.AddItems(myFixedPoint, objectBuilder);
				}
			}
		}

		[VisualScriptingMiscData("Entity", "Adds item defined by id in specific quantity into inventory of entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddToInventoryFloat(string entityname, MyDefinitionId itemId, float amount = 1f)
		{
			MyEntity entityByName = GetEntityByName(entityname);
			if (entityByName != null)
			{
				MyInventoryBase inventoryBase = entityByName.GetInventoryBase();
				if (inventoryBase != null)
				{
					MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(itemId);
					MyFixedPoint myFixedPoint = default(MyFixedPoint);
					myFixedPoint = (MyFixedPoint)amount;
					inventoryBase.AddItems(myFixedPoint, objectBuilder);
				}
			}
		}

		[VisualScriptingMiscData("Entity", "Removes item defined by id in specific quantity from inventory of entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveFromEntityInventory(string entityName, MyDefinitionId itemId = default(MyDefinitionId), float amount = 1f)
		{
			MyFixedPoint myFixedPoint = (MyFixedPoint)amount;
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
<<<<<<< HEAD
			{
				return;
			}
			if (entityByName is MyCubeGrid)
			{
=======
			{
				return;
			}
			if (entityByName is MyCubeGrid)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyCubeBlock fatBlock in ((MyCubeGrid)entityByName).GetFatBlocks())
				{
					if (fatBlock != null && fatBlock.HasInventory)
					{
						myFixedPoint -= RemoveInventoryItems(fatBlock, itemId, myFixedPoint);
					}
					if (myFixedPoint <= 0)
					{
						break;
					}
				}
<<<<<<< HEAD
			}
			else
			{
				RemoveInventoryItems(entityByName, itemId, myFixedPoint);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				RemoveInventoryItems(entityByName, itemId, myFixedPoint);
			}
		}

		[VisualScriptingMiscData("Entity", "Gets amount of specific items in inventory of entity. (rounded)", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetEntityInventoryItemAmount(string entityName, MyDefinitionId itemId)
		{
			return (int)Math.Round(GetEntityInventoryItemAmountPrecise(entityName, itemId));
		}

		[VisualScriptingMiscData("Entity", "Gets amount of specific items in inventory of entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetEntityInventoryItemAmountPrecise(string entityName, MyDefinitionId itemId)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null || (!entityByName.HasInventory && !(entityByName is MyCubeGrid)))
			{
				return 0f;
			}
			MyFixedPoint myFixedPoint = 0;
			if (entityByName is MyCubeGrid)
			{
				foreach (MyCubeBlock fatBlock in ((MyCubeGrid)entityByName).GetFatBlocks())
				{
					if (fatBlock != null && fatBlock.HasInventory)
					{
						myFixedPoint += GetInventoryItemAmount(fatBlock, itemId);
					}
				}
			}
			else
			{
				myFixedPoint = GetInventoryItemAmount(entityByName, itemId);
			}
			return (float)myFixedPoint;
		}

		[VisualScriptingMiscData("Entity", "Returns true if entity has item in specific inventory on specific slot. Also return definition id of that item and its amount.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetEntityInventoryItemAtSlot(string entityName, out MyDefinitionId itemId, out float amount, int slot = 0, int inventoryId = 0)
		{
			itemId = default(MyDefinitionId);
			amount = 0f;
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null || !entityByName.HasInventory)
			{
				return false;
			}
			inventoryId = Math.Max(inventoryId, 0);
			if (inventoryId >= entityByName.InventoryCount)
			{
				return false;
			}
			MyInventory inventory = entityByName.GetInventory(inventoryId);
			if (inventory != null)
			{
				MyPhysicalInventoryItem? itemByIndex = inventory.GetItemByIndex(slot);
				if (itemByIndex.HasValue)
				{
					amount = (float)itemByIndex.Value.Amount;
					itemId = itemByIndex.Value.Content.GetObjectId();
					return true;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Entity", "Changes ownership of a specific block (if entity is block) or ownership of all functional blocks (if entity is grid) to specific player and modify its/theirs share settings.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool ChangeOwner(string entityName, long playerId = 0L, bool factionShare = false, bool allShare = false)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return false;
			}
			MyOwnershipShareModeEnum shareMode = MyOwnershipShareModeEnum.None;
			if (factionShare)
			{
				shareMode = MyOwnershipShareModeEnum.Faction;
			}
			if (allShare)
			{
				shareMode = MyOwnershipShareModeEnum.All;
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.ChangeBlockOwnerRequest(0L, shareMode);
					if (playerId > 0)
					{
						myCubeBlock.ChangeBlockOwnerRequest(playerId, shareMode);
					}
					return true;
				}
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null)
				{
					foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
					{
						if (!(fatBlock is MyLightingBlock) && (fatBlock is MyFunctionalBlock || fatBlock is MyShipController || fatBlock is MyTerminalBlock))
						{
							myCubeGrid.ChangeOwnerRequest(myCubeGrid, fatBlock, 0L, shareMode);
							if (playerId > 0)
							{
								myCubeGrid.ChangeOwnerRequest(myCubeGrid, fatBlock, playerId, shareMode);
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Entity", "Get owner of specific entity. 0 for nobody.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static long GetOwner(string entityName)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					return myCubeBlock.OwnerId;
				}
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null)
				{
					if (myCubeGrid.BigOwners.Count <= 0)
					{
						return 0L;
					}
					return myCubeGrid.BigOwners[0];
				}
			}
			return 0L;
		}

		[VisualScriptingMiscData("Entity", "Renames specific entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RenameEntity(string oldName, string newName = null)
		{
			if (!(oldName == newName))
			{
				MyEntity entityByName = GetEntityByName(oldName);
				if (entityByName != null)
				{
					entityByName.Name = newName;
					MyEntities.SetEntityName(entityByName);
				}
			}
		}

		[VisualScriptingMiscData("Entity", "Gets name of specific entity defined by id.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetName(long entityId, string name)
		{
			if (MyEntities.TryGetEntityById(entityId, out var entity) && GetEntityByName(name) == null)
			{
				entity.Name = name;
				MyEntities.SetEntityName(entity);
			}
		}

		[VisualScriptingMiscData("Entity", "Gets name of specific entity defined by id.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetEntityName(long entityId)
		{
			if (!MyEntities.TryGetEntityById(entityId, out var entity))
			{
				return string.Empty;
			}
			return entity.Name;
		}

		[VisualScriptingMiscData("Entity", "Gets linear velocity of specific entity.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D GetEntitySpeed(string entityName)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName != null && entityByName.Physics != null)
			{
				return entityByName.Physics.LinearVelocity;
			}
			return Vector3D.Zero;
		}

		[VisualScriptingMiscData("Entity", "Gets DefinitionId from typeId and subtypeId", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyDefinitionId GetDefinitionId(string typeId, string subtypeId)
		{
			if (!MyObjectBuilderType.TryParse(typeId, out var result))
			{
				MyObjectBuilderType.TryParse("MyObjectBuilder_" + typeId, out result);
				return new MyDefinitionId(result, subtypeId);
			}
			return new MyDefinitionId(result, subtypeId);
		}

		[VisualScriptingMiscData("Entity", "Gets typeId and subtypeId out of DefinitionId.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static void GetDataFromDefinition(MyDefinitionId definitionId, out string typeId, out string subtypeId)
		{
			MyObjectBuilderType typeId2 = definitionId.TypeId;
			typeId = typeId2.ToString();
			MyStringHash subtypeId2 = definitionId.SubtypeId;
			subtypeId = subtypeId2.ToString();
		}

		[VisualScriptingMiscData("Entity", "Removes specific entity from world.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveEntity(string entityName, bool removeGroup = true, string typeRestriction = null)
		{
			RemoveEntity(GetEntityByName(entityName), removeGroup, typeRestriction);
		}

		[VisualScriptingMiscData("Entity", "Removes specific entity from world.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveEntity(long entityId, bool removeGroup = true, string typeRestriction = null)
		{
			RemoveEntity(GetEntityById(entityId), removeGroup, typeRestriction);
		}

		[VisualScriptingMiscData("Entity", "Removes specific entity from world.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveEntity(MyEntity entity, bool removeGroup = true, string typeRestriction = null)
		{
			IEnumerable<string> allowedTypes = null;
			IEnumerable<string> disallowedTypes = null;
			if (!string.IsNullOrEmpty(typeRestriction))
			{
<<<<<<< HEAD
				IEnumerable<string> source = from type in typeRestriction.Split(new char[1] { ',' })
					select type.Trim();
				allowedTypes = source.Where((string x) => !x.StartsWith("!"));
				disallowedTypes = source.Where((string x) => x.StartsWith("!"));
=======
				IEnumerable<string> enumerable = Enumerable.Select<string, string>((IEnumerable<string>)typeRestriction.Split(new char[1] { ',' }), (Func<string, string>)((string type) => type.Trim()));
				allowedTypes = Enumerable.Where<string>(enumerable, (Func<string, bool>)((string x) => !x.StartsWith("!")));
				disallowedTypes = Enumerable.Where<string>(enumerable, (Func<string, bool>)((string x) => x.StartsWith("!")));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			RemoveEntity(entity, removeGroup, allowedTypes, disallowedTypes);
		}

		private static void RemoveEntity(MyEntity entity, bool removeGroup, IEnumerable<string> allowedTypes, IEnumerable<string> disallowedTypes)
		{
			if (entity == null)
			{
				return;
			}
			if (allowedTypes != null && disallowedTypes != null)
			{
				foreach (string disallowedType in disallowedTypes)
				{
					if (entity.GetType().Name == disallowedType)
					{
						return;
					}
				}
				bool flag = false;
				foreach (string allowedType in allowedTypes)
				{
					if (entity.GetType().Name == allowedType)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			if (entity is MyCubeBlock)
			{
				MyCubeBlock myCubeBlock = (MyCubeBlock)entity;
				myCubeBlock.CubeGrid.RemoveBlock(myCubeBlock.SlimBlock, updatePhysics: true);
			}
			else if (entity is MyCubeGrid && removeGroup)
			{
				MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot((MyCubeGrid)entity);
				MyGridPhysicalHierarchy.Static.ApplyOnChildren(root, delegate(MyCubeGrid x)
				{
					x.Close();
				});
				root.Close();
			}
			else
			{
				entity.Close();
			}
		}

		[VisualScriptingMiscData("Entity", "Removes entities in designated area.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveEntities(Vector3D position, double radius, bool removePlanets = true, bool removeGroup = true, string typeRestriction = null)
		{
			if (radius <= 0.0)
			{
				return;
			}
			BoundingSphereD sphere = new BoundingSphereD(position, radius);
			m_tmpEntities.Clear();
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_tmpEntities);
			if (!removePlanets)
			{
				typeRestriction = ((!string.IsNullOrEmpty(typeRestriction)) ? (typeRestriction + ",!MyPlanet") : "!MyPlanet");
			}
			IEnumerable<string> allowedTypes = null;
			IEnumerable<string> disallowedTypes = null;
			if (!string.IsNullOrEmpty(typeRestriction))
			{
<<<<<<< HEAD
				IEnumerable<string> source = from type in typeRestriction.Split(new char[1] { ',' })
					select type.Trim();
				allowedTypes = source.Where((string x) => !x.StartsWith("!"));
				disallowedTypes = source.Where((string x) => x.StartsWith("!"));
=======
				IEnumerable<string> enumerable = Enumerable.Select<string, string>((IEnumerable<string>)typeRestriction.Split(new char[1] { ',' }), (Func<string, string>)((string type) => type.Trim()));
				allowedTypes = Enumerable.Where<string>(enumerable, (Func<string, bool>)((string x) => !x.StartsWith("!")));
				disallowedTypes = Enumerable.Where<string>(enumerable, (Func<string, bool>)((string x) => x.StartsWith("!")));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			foreach (MyEntity tmpEntity in m_tmpEntities)
			{
				RemoveEntity(tmpEntity, removeGroup, allowedTypes, disallowedTypes);
			}
		}

		[VisualScriptingMiscData("Entity", "Returns true if specific entity is present in the world.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool EntityExists(string entityName)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName is MyCubeGrid)
			{
				if (((MyCubeGrid)entityByName).InScene)
				{
					return !((MyCubeGrid)entityByName).MarkedForClose;
				}
				return false;
			}
			return entityByName != null;
		}

		private static MyEntityThrustComponent GetThrustComponentByEntityName(string entityName)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
			{
				return null;
			}
			MyComponentBase component = null;
			entityByName.Components.TryGet(typeof(MyEntityThrustComponent), out component);
			return component as MyEntityThrustComponent;
		}

		[VisualScriptingMiscData("Entity", "Returns true if entity has dampeners enabled, false otherwise.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetDampenersEnabled(string entityName)
		{
			return GetThrustComponentByEntityName(entityName)?.DampenersEnabled ?? false;
		}

		[VisualScriptingMiscData("Entity", "Turns dampeners of specific entity on/off.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetDampenersEnabled(string entityName, bool state)
		{
			MyEntityThrustComponent thrustComponentByEntityName = GetThrustComponentByEntityName(entityName);
			if (thrustComponentByEntityName != null)
			{
				thrustComponentByEntityName.DampenersEnabled = state;
			}
		}

		[VisualScriptingMiscData("Entity", "Finds free place around the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool FindFreePlace(Vector3D position, out Vector3D newPosition, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f)
		{
			Vector3D? vector3D = MyEntities.FindFreePlace(position, radius, maxTestCount, testsPerDistance, stepSize);
			newPosition = (vector3D.HasValue ? vector3D.Value : Vector3D.Zero);
			return vector3D.HasValue;
		}

		[VisualScriptingMiscData("Environment", "Sets time of day.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SunRotationSetTime(float time)
		{
			MyTimeOfDayHelper.UpdateTimeOfDay(time);
		}

		[VisualScriptingMiscData("Environment", "Enables/disable sun rotation.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SunRotationEnabled(bool enabled)
		{
			MySession.Static.Settings.EnableSunRotation = enabled;
		}

		[VisualScriptingMiscData("Environment", "Sets length of day.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SunRotationSetDayLength(float length)
		{
			MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval = length;
		}

		[VisualScriptingMiscData("Environment", "Gets length of day.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float SunRotationGetDayLength()
		{
			return MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval;
		}

		[VisualScriptingMiscData("Environment", "Gets current time of day.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float SunRotationGetCurrentTime()
		{
			return MyTimeOfDayHelper.TimeOfDay;
		}

		[VisualScriptingMiscData("Environment", "Gets current sun rotation.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3 GetSunDirection()
		{
			return MySector.SunProperties.SunDirectionNormalized;
<<<<<<< HEAD
=======
		}

		[VisualScriptingMiscData("Environment", "Returns true if position is on dark side of a planet.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsOnDarkSide(MyPlanet planet, Vector3D position)
		{
			return MySectorWeatherComponent.IsThereNight(planet, ref position);
		}

		[VisualScriptingMiscData("Environment", "Sets density, multiplier, color, skybox multiplier, and atmosphere multiplier of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetAll(float? density, float? multiplier, Vector3? color, float? skyboxMultiplier = 0f, float? atmoMultiplier = 0f)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => FogSetAllInternalAll, density ?? MyFogProperties.Default.FogDensity, multiplier ?? MyFogProperties.Default.FogMultiplier, color ?? MyFogProperties.Default.FogColor, skyboxMultiplier ?? MyFogProperties.Default.FogSkybox, atmoMultiplier ?? MyFogProperties.Default.FogAtmo);
		}

		[Event(null, 3654)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void FogSetAllInternalAll(float density, float multiplier, Vector3 color, float skyboxMultiplier = 0f, float atmoMultiplier = 0f)
		{
			MySector.FogProperties.FogMultiplier = multiplier;
			MySector.FogProperties.FogDensity = density;
			MySector.FogProperties.FogColor = color;
			MySector.FogProperties.FogSkybox = skyboxMultiplier;
			MySector.FogProperties.FogAtmo = atmoMultiplier;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[VisualScriptingMiscData("Environment", "Returns true if position is on dark side of a planet.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsOnDarkSide(MyPlanet planet, Vector3D position)
		{
<<<<<<< HEAD
			return MySectorWeatherComponent.IsThereNight(planet, ref position);
=======
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogDensity = density;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[VisualScriptingMiscData("Environment", "Sets density, multiplier, color, skybox multiplier, and atmosphere multiplier of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetAll(float? density, float? multiplier, Vector3? color, float? skyboxMultiplier = 0f, float? atmoMultiplier = 0f)
		{
<<<<<<< HEAD
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => FogSetAllInternalAll, density ?? MyFogProperties.Default.FogDensity, multiplier ?? MyFogProperties.Default.FogMultiplier, color ?? MyFogProperties.Default.FogColor, skyboxMultiplier ?? MyFogProperties.Default.FogSkybox, atmoMultiplier ?? MyFogProperties.Default.FogAtmo);
		}

		[Event(null, 3654)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void FogSetAllInternalAll(float density, float multiplier, Vector3 color, float skyboxMultiplier = 0f, float atmoMultiplier = 0f)
		{
			MySector.FogProperties.FogMultiplier = multiplier;
			MySector.FogProperties.FogDensity = density;
			MySector.FogProperties.FogColor = color;
			MySector.FogProperties.FogSkybox = skyboxMultiplier;
			MySector.FogProperties.FogAtmo = atmoMultiplier;
		}

		[VisualScriptingMiscData("Environment", "Sets density of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetDensity(float density)
		{
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogDensity = density;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
		}

		[VisualScriptingMiscData("Environment", "Sets multiplier of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetMultiplier(float multiplier)
		{
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogMultiplier = multiplier;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
		}

=======
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogMultiplier = multiplier;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[VisualScriptingMiscData("Environment", "Sets color of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetColor(Vector3 color)
		{
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogColor = color;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
		}

		[VisualScriptingMiscData("Environment", "Sets skybox multiplier of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetSkyboxMultiplier(float skyboxMultiplier)
		{
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogSkybox = skyboxMultiplier;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
		}

		[VisualScriptingMiscData("Environment", "Sets atmosphere multiplier of fog.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FogSetAtmoMultiplier(float atmoMultiplier)
		{
			MyFogProperties fogProperties = MySector.FogProperties;
			fogProperties.FogAtmo = atmoMultiplier;
			FogSetAll(fogProperties.FogDensity, fogProperties.FogMultiplier, fogProperties.FogColor, fogProperties.FogSkybox, fogProperties.FogAtmo);
		}

		[VisualScriptingMiscData("Environment", "Get temperature based on position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static float GetTemperatureInPoint(Vector3D position)
		{
			return MySectorWeatherComponent.GetTemperatureInPoint(position);
		}

		[VisualScriptingMiscData("Environment", "Creates lightning on exact position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateLightning(Vector3D position)
		{
			MySession.Static.GetComponent<MySectorWeatherComponent>().CreateLightning(position, new MyObjectBuilder_WeatherLightning());
		}

		[VisualScriptingMiscData("Environment", "Set weather based on character position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetWeatherC(string weatherCommand, string weatherEffect, float radius)
		{
			MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(weatherEffect, radius, null, verbose: false, Vector3.Zero);
		}

		[VisualScriptingMiscData("Environment", "Set weather based on exact position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetWeatherP(string weatherCommand, string weatherEffect, float radius, Vector3D position)
		{
			MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(weatherEffect, radius, position, verbose: false, Vector3.Zero);
		}

		[VisualScriptingMiscData("Environment", "Get weather based on position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static string GetWeather(Vector3D position)
		{
			return MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(position);
		}

		[VisualScriptingMiscData("Environment", "Get weather intensity based on position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static float GetWeatherIntensity(Vector3D position)
		{
			return MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeatherIntensity(position);
		}

		[VisualScriptingMiscData("Environment", "Remove a weather based on position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveWeather(Vector3D position)
		{
			MySession.Static.GetComponent<MySectorWeatherComponent>().RemoveWeather(position);
		}

		[VisualScriptingMiscData("Environment", "Summons lightning on a position", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateLightning(Vector3D position, float boltLength, byte boltParts, short boltVariation, float boltRadius, short maxLife, int damage)
		{
			MyObjectBuilder_WeatherLightning myObjectBuilder_WeatherLightning = new MyObjectBuilder_WeatherLightning();
			myObjectBuilder_WeatherLightning.BoltLength = boltLength;
			myObjectBuilder_WeatherLightning.BoltParts = boltParts;
			myObjectBuilder_WeatherLightning.BoltVariation = boltVariation;
			myObjectBuilder_WeatherLightning.BoltRadius = boltRadius;
			myObjectBuilder_WeatherLightning.MaxLife = maxLife;
			myObjectBuilder_WeatherLightning.Damage = damage;
			MySession.Static.GetComponent<MySectorWeatherComponent>().CreateLightning(position, myObjectBuilder_WeatherLightning);
		}

		[VisualScriptingMiscData("Environment", "Returns true if position is underground", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool IsUnderGround(Vector3D position)
		{
			return MyGamePruningStructure.GetClosestPlanet(position)?.IsUnderGround(position) ?? false;
		}

		[VisualScriptingMiscData("Factions", "Gets id of local player. Works only on Lobby and clients. On Dedicated server returns 0.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetLocalPlayerId()
		{
			return MySession.Static.LocalPlayerId;
		}

		[VisualScriptingMiscData("Factions", "Gets id of pirate faction.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetPirateId()
		{
			return MyPirateAntennas.GetPiratesId();
		}

		[VisualScriptingMiscData("Factions", "Gets tag of faction, specific player is in.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetPlayersFactionTag(long playerId = 0L)
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId) as MyFaction;
			if (myFaction == null)
			{
				return "";
			}
			return myFaction.Tag;
		}

		[VisualScriptingMiscData("Factions", "Gets name of faction, specific player is in.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetPlayersFactionName(long playerId = 0L)
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId) as MyFaction;
			if (myFaction == null)
			{
				return "";
			}
			return myFaction.Name;
		}

		[VisualScriptingMiscData("Factions", "Forces join player into a faction specified by tag. Returns false if faction does not exist, true otherwise. If player was in any faction before, he will be removed from that faction.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool SetPlayersFaction(long playerId = 0L, string factionTag = "")
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(factionTag);
			if (myFaction == null)
			{
				return false;
			}
			KickPlayerFromFaction(playerId);
			MyFactionCollection.SendJoinRequest(myFaction.FactionId, playerId);
			return true;
		}

		[VisualScriptingMiscData("Factions", "Returns list of all members (of theirs ids) of specific faction.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetFactionMembers(string factionTag = "")
		{
			List<long> list = new List<long>();
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(factionTag);
			if (myFaction == null)
			{
				return list;
			}
			foreach (KeyValuePair<long, MyFactionMember> member in myFaction.Members)
			{
				list.Add(member.Key);
			}
			return list;
		}

		[VisualScriptingMiscData("Factions", "Kicks specific player from faction he is in.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void KickPlayerFromFaction(long playerId = 0L)
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyFactionCollection.KickMember((MySession.Static.Factions.TryGetPlayerFaction(playerId) as MyFaction)?.FactionId ?? 0, playerId);
		}

		[VisualScriptingMiscData("Factions", "Creates new faction.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateFaction(long founderId, string factionTag, string factionName = "", string factionDescription = "", string factionPrivateText = "")
		{
			MySession.Static.Factions.CreateFaction(founderId, factionTag, factionName, factionDescription, factionPrivateText, MyFactionTypes.None);
		}

		[VisualScriptingMiscData("Factions", "Returns true if specified two factions are enemies.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool AreFactionsEnemies(string firstFactionTag, string secondFactionTag)
		{
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(firstFactionTag);
			if (myFaction == null)
			{
				return false;
			}
			MyFaction myFaction2 = MySession.Static.Factions.TryGetFactionByTag(firstFactionTag);
			if (myFaction2 == null)
			{
				return false;
			}
			return MySession.Static.Factions.AreFactionsEnemies(myFaction.FactionId, myFaction2.FactionId);
		}

		[VisualScriptingMiscData("Factions", "Returns current reputation between two factions. Returns int.MinValue (-2147483648) if either of factions is not found.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetRelationBetweenFactions(string tagA, string tagB)
		{
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(tagA);
			MyFaction myFaction2 = MySession.Static.Factions.TryGetFactionByTag(tagB);
			if (myFaction == null || myFaction2 == null)
			{
				return int.MinValue;
			}
			return MySession.Static.Factions.GetRelationBetweenFactions(myFaction.FactionId, myFaction2.FactionId).Item2;
		}

		[VisualScriptingMiscData("Factions", "Returns current reputation between player and faction. Returns int.MinValue (-2147483648) if player or faction is not found.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetRelationBetweenPlayerAndFaction(long playerId, string tagB)
		{
			MyPlayer.PlayerId result;
			bool num = MySession.Static.Players.TryGetPlayerId(playerId, out result);
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(tagB);
			if (!num || myFaction == null)
			{
				return int.MinValue;
			}
			return MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, myFaction.FactionId).Item2;
		}

		[VisualScriptingMiscData("Factions", "Set reputation between two factions. Reputation will be automatically clamped to allwed range.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetRelationBetweenFactions(string tagA, string tagB, int reputation)
		{
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(tagA);
			MyFaction myFaction2 = MySession.Static.Factions.TryGetFactionByTag(tagB);
			if (myFaction != null && myFaction2 != null)
			{
				MySession.Static.Factions.SetReputationBetweenFactions(myFaction.FactionId, myFaction2.FactionId, MySession.Static.Factions.ClampReputation(reputation));
			}
		}

		[VisualScriptingMiscData("Factions", "Set reputation between player and faction. Reputation will be automatically clamped to allwed range.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetRelationBetweenPlayerAndFaction(long playerId, string tagB, int reputation)
		{
			MyPlayer.PlayerId result;
			bool num = MySession.Static.Players.TryGetPlayerId(playerId, out result);
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(tagB);
			if (num && myFaction != null)
			{
				MySession.Static.Factions.SetReputationBetweenPlayerAndFaction(playerId, myFaction.FactionId, MySession.Static.Factions.ClampReputation(reputation));
			}
		}

		[VisualScriptingMiscData("Gameplay", "Returns true if world is creative.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsCreative()
		{
			return MySession.Static.CreativeMode;
		}

		[VisualScriptingMiscData("Gameplay", "Returns true if world is survival.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsSurvival()
		{
			return MySession.Static.SurvivalMode;
		}

		[VisualScriptingMiscData("Gameplay", "Returns simulation quality (0 normal, 1 low, 2 verylow) currect platform can handle", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetSimulationQuality()
		{
<<<<<<< HEAD
			switch (MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE ?? MyVRage.Platform.System.SimulationQuality)
			{
			case SimulationQuality.Normal:
				return 0;
			case SimulationQuality.Low:
				return 1;
			case SimulationQuality.VeryLow:
				return 2;
			default:
				throw new InvalidBranchException();
			}
=======
			return (MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE ?? MyVRage.Platform.System.SimulationQuality) switch
			{
				SimulationQuality.Normal => 0, 
				SimulationQuality.Low => 1, 
				SimulationQuality.VeryLow => 2, 
				_ => throw new InvalidBranchException(), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[VisualScriptingMiscData("Gameplay", "Enables terminal screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EnableTerminal(bool flag)
		{
			MyPerGameSettings.GUI.EnableTerminalScreen = flag;
		}

		[VisualScriptingMiscData("Gameplay", "Saves the game.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool SaveSession()
		{
			if (!MyAsyncSaving.InProgress)
			{
				MyAsyncSaving.Start();
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Gameplay", "Saves the game under specific name.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool SaveSessionAs(string saveName)
		{
			if (!MyAsyncSaving.InProgress)
			{
				MyAsyncSaving.Start(null, MyStatControlText.SubstituteTexts(saveName));
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Gameplay", "Allows all players to save", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EnableSaving(bool enable)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EnableSavingSync, enable);
		}

		[Event(null, 4032)]
		[Reliable]
		[Broadcast]
		[Server]
		private static void EnableSavingSync(bool enable)
		{
			MySession.Static.Settings.EnableSaving = enable;
		}

		public static void EnableSavingLocal(bool enable)
		{
			EnableSavingSync(enable);
		}

		[VisualScriptingMiscData("Gameplay", "Allows all players to save", -10510688)]
		public static bool IsSavingEnabled()
		{
			return MySession.Static.Settings.EnableSaving;
		}

		[VisualScriptingMiscData("Gameplay", "Provides information if game is currently being saved", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGameSaving()
		{
			return MyAsyncSaving.InProgress;
		}

		[VisualScriptingMiscData("Gameplay", "Displays reload dialog with specific caption and message to load save defined by path.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SessionReloadDialog(string caption, string message, string savePath = null)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: new StringBuilder(caption), messageText: new StringBuilder(message), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MySessionLoader.LoadSingleplayerSession(savePath ?? MySession.Static.CurrentPath, null, MyCampaignManager.Static.ActiveCampaignName);
				}
				else
				{
					MySessionLoader.UnloadAndExitToMenu();
				}
			}));
		}

		[VisualScriptingMiscData("Gameplay", "Closes active session after the specific time (in ms).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SessionClose(int fadeTimeMs = 10000, bool showCredits = true, bool closeSession = true)
		{
			if (fadeTimeMs < 0)
			{
				fadeTimeMs = 10000;
			}
			if (!(closeSession || showCredits))
			{
				return;
			}
			if (!MyCampaignManager.Static.IsCampaignRunning)
			{
				MyGuiScreenFade myGuiScreenFade = new MyGuiScreenFade(Color.Black, (uint)fadeTimeMs, 0u);
				myGuiScreenFade.Shown += delegate
				{
					MySandboxGame.Static.Invoke(delegate
					{
						if (closeSession)
						{
							MySessionLoader.UnloadAndExitToMenu();
						}
						if (showCredits)
						{
							MyScreenManager.AddScreen(new MyGuiScreenGameCredits());
						}
					}, "MyVisualScriptLogicProvider::SessionClose");
				};
				MyHud.MinimalHud = true;
				MyScreenManager.AddScreen(myGuiScreenFade);
				return;
			}
			MyGuiScreenFade myGuiScreenFade2 = new MyGuiScreenFade(Color.Black, (uint)fadeTimeMs, 0u);
			myGuiScreenFade2.Shown += delegate
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MySession.Static.GetComponent<MyCampaignSessionComponent>().LoadNextCampaignMission(closeSession, showCredits);
				}, "MyVisualScriptLogicProvider::SessionClose");
			};
			MyHud.MinimalHud = true;
			MyScreenManager.AddScreen(myGuiScreenFade2);
		}

		[VisualScriptingMiscData("Gameplay", "Reloads last checkpoint while displaying message on screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SessionReloadLastCheckpoint(int fadeTimeMs = 10000, string message = null, float textScale = 1f, string font = "Blue")
		{
			if (fadeTimeMs < 0)
			{
				fadeTimeMs = 10000;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.LocalCharacter.DeactivateRespawn();
			}
			MyGuiScreenFade myGuiScreenFade = new MyGuiScreenFade(Color.Black, (uint)fadeTimeMs, 0u);
			myGuiScreenFade.Shown += delegate
			{
				MySessionLoader.LoadSingleplayerSession(MySession.Static.CurrentPath, null, MyCampaignManager.Static.ActiveCampaignName);
				MyHud.MinimalHud = false;
			};
			if (!string.IsNullOrEmpty(message))
			{
				StringBuilder stringBuilder = MyTexts.SubstituteTexts(new StringBuilder(message));
				MyGuiControls controls = myGuiScreenFade.Controls;
				Vector2? position = new Vector2(0.5f);
				Vector2? size = new Vector2(0.6f, 0.3f);
				StringBuilder contents = stringBuilder;
				controls.Add(new MyGuiControlMultilineText(position, size, null, "Red", textScale, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, contents));
			}
			MyHud.MinimalHud = true;
			MyScreenManager.AddScreen(myGuiScreenFade);
		}

		[VisualScriptingMiscData("Gameplay", "Displays player the dialog to exit game to main menu (for non-campaign) or continue next campaign mission (for campaign).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SessionExitGameDialog(string caption, string message)
		{
			if (m_exitGameDialogOpened)
<<<<<<< HEAD
			{
				return;
			}
			m_exitGameDialogOpened = true;
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder(caption), messageText: new StringBuilder(message), okButtonText: MyCampaignManager.Static.IsCampaignRunning ? MyCommonTexts.ScreenMenuButtonContinue : MyCommonTexts.ScreenMenuButtonExitToMainMenu, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
=======
			{
				return;
			}
			m_exitGameDialogOpened = true;
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder(caption), messageText: new StringBuilder(message), okButtonText: MyCampaignManager.Static.IsCampaignRunning ? MyCommonTexts.ScreenMenuButtonContinue : MyCommonTexts.ScreenMenuButtonExitToMainMenu, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					if (MyCampaignManager.Static.IsCampaignRunning)
					{
						MySession.Static.GetComponent<MyCampaignSessionComponent>().LoadNextCampaignMission(closeSession: true, showCredits: false);
					}
					else
					{
						MySessionLoader.UnloadAndExitToMenu();
					}
				}
				m_exitGameDialogOpened = false;
			}));
		}

		[VisualScriptingMiscData("Gameplay", "Gets path of the session (game/mission) currently being played.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetCurrentSessionPath()
		{
			return MySession.Static.CurrentPath;
		}

		[VisualScriptingMiscData("Gameplay", "Returns true if session is fully loaded.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGameLoaded()
		{
			return GameIsReady;
		}

		[VisualScriptingMiscData("Gameplay", "[Obsolete, use SetMissionOutcome] Sets the state of campaign. Necessary for transitions between missions in campaign.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetCampaignLevelOutcome(string outcome)
		{
			SetMissionOutcome(outcome);
		}

		[VisualScriptingMiscData("Gameplay", "Finishes active mission (state Mission Complete) with fadeout (ms).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void FinishMission(string outcome = "Mission Complete", int fadeTimeMs = 5000)
		{
			SetMissionOutcome(outcome);
			SessionClose(fadeTimeMs);
		}

		[VisualScriptingMiscData("Gameplay", "Sets the state of the mission. Necessary for transitions between missions in the scenario.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetMissionOutcome(string outcome = "Mission Complete")
		{
			MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
			if (component != null)
			{
				component.CampaignLevelOutcome = outcome;
			}
		}

		[VisualScriptingMember(false, false)]
		[VisualScriptingMiscData("Gameplay", "Get Steam ID from player ID.", -10510688)]
		public static ulong GetSteamId(long playerId)
		{
			if (MySession.Static != null)
			{
				return MySession.Static.Players.TryGetSteamId(playerId);
			}
			return 0uL;
		}

		[VisualScriptingMember(true, false)]
		[VisualScriptingMiscData("Gameplay", "Disconnect player.", -10510688)]
		public static void DisconnectPlayer(ulong steamId)
		{
			if (MySession.Static != null && MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.DisconnectClient(steamId);
			}
		}

		[VisualScriptingMember(true, false)]
		[VisualScriptingMiscData("Gameplay", "Kick player.", -10510688)]
		public static void KickPlayer(ulong steamId, bool kick = true, bool add = true)
		{
			if (MySession.Static != null && MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.KickClient(steamId, kick, add);
			}
		}

		[VisualScriptingMember(true, false)]
		[VisualScriptingMiscData("Gameplay", "Enable custom respawn. Needed for PlayerRequestsRespawn to work properly.", -10510688)]
		public static void EnableCustomRespawn(bool enable = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EnableCustomRespawnSync, enable, arg);
		}

		[Event(null, 4281)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void EnableCustomRespawnSync(bool enable, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
				if (component != null)
				{
					component.CustomRespawnEnabled = enable;
				}
			}
		}

		public static void CustomRespawnRequest(long playerId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CustomRespawnRequestServer, playerId);
		}

		[Event(null, 4299)]
		[Reliable]
		[Server]
		private static void CustomRespawnRequestServer(long playerId)
		{
			PlayerRequestsRespawn?.Invoke(playerId);
		}

		[VisualScriptingMember(true, false)]
		[VisualScriptingMiscData("Gameplay", "Stop dedicated server (autorestart if enabled).", -10510688)]
		public static void StopDedicatedServer()
		{
			if (Sync.IsDedicated)
			{
				MySandboxGame.Log.WriteLineAndConsole("The script stopped DS.");
				MySandboxGame.ExitThreadSafe();
			}
		}

		[VisualScriptingMiscData("Input", "Checks if input control has been pressed this frame.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsNewGameControlPressed(string controlStringId)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyStringId context = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty);
				MyStringId context2 = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty);
				if (MyControllerHelper.IsControl(context, MyStringId.GetOrCompute(controlStringId)))
				{
					return true;
				}
				return MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId));
			}
			return MyInput.Static.IsNewGameControlPressed(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control is currently pressed.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGameControlPressed(string controlStringId)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyStringId context = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty);
				MyStringId context2 = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty);
				if (MyControllerHelper.IsControl(context, MyStringId.GetOrCompute(controlStringId), MyControlStateType.PRESSED))
				{
					return true;
				}
				return MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId), MyControlStateType.PRESSED);
			}
			return MyInput.Static.IsGameControlPressed(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control has been released this frame.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsNewGameControlReleased(string controlStringId)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyStringId context = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty);
				MyStringId context2 = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty);
				if (MyControllerHelper.IsControl(context, MyStringId.GetOrCompute(controlStringId), MyControlStateType.NEW_RELEASED))
				{
					return true;
				}
				return MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId), MyControlStateType.NEW_RELEASED);
			}
			return MyInput.Static.IsNewGameControlReleased(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control is currently released.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGameControlReleased(string controlStringId)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyStringId context = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty);
				MyStringId context2 = ((MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty);
				if (!MyControllerHelper.IsControl(context, MyStringId.GetOrCompute(controlStringId), MyControlStateType.PRESSED))
				{
					return true;
				}
				return !MyControllerHelper.IsControl(context2, MyStringId.GetOrCompute(controlStringId), MyControlStateType.PRESSED);
			}
			return MyInput.Static.IsGameControlReleased(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetHighlight([Nullable] string entityName, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, [Nullable] string subPartNames = null)
<<<<<<< HEAD
		{
			thickness = (enabled ? thickness : (-1));
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetHighlightSync, entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
		}

		[Event(null, 4425)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetHighlightSync([Nullable] string entityName, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, [Nullable] string subPartNames = null)
		{
=======
		{
			thickness = (enabled ? thickness : (-1));
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetHighlightSync, entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
		}

		[Event(null, 4425)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetHighlightSync([Nullable] string entityName, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, [Nullable] string subPartNames = null)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (color == default(Color))
				{
					color = DEFAULT_HIGHLIGHT_COLOR;
				}
				if (playerId == -1)
				{
					playerId = GetLocalPlayerId();
				}
				MyHighlightData highlightData = default(MyHighlightData);
				highlightData.EntityId = entity.EntityId;
				highlightData.OutlineColor = color;
				highlightData.PulseTimeInFrames = (ulong)pulseTimeInFrames;
				highlightData.Thickness = thickness;
				highlightData.PlayerId = playerId;
				highlightData.IgnoreUseObjectData = subPartNames == null;
				highlightData.SubPartNames = (string.IsNullOrEmpty(subPartNames) ? "" : subPartNames);
				SetHighlight(highlightData, playerId);
			}
		}

		public static void SetHighlightLocal(string entityName, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, string subPartNames = null)
		{
			SetHighlightSync(entityName, thickness, pulseTimeInFrames, color, playerId, subPartNames);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for local player. You can set alpha of color too.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetAlphaHighlight(string entityName, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, string subPartNames = null, float alpha = 1f)
		{
			Color color2 = color;
			color2.A = (byte)(alpha * 255f);
			SetHighlight(entityName, enabled, thickness, pulseTimeInFrames, color2, playerId, subPartNames);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetHighlightForAll(string entityName, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), string subPartNames = null)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				SetHighlight(entityName, enabled, thickness, pulseTimeInFrames, color, item.Identity.IdentityId, subPartNames);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight of specific object for all players. You can set alpha of color too.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetAlphaHighlightForAll(string entityName, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), string subPartNames = null, float alpha = 1f)
		{
			Color color2 = color;
			color2.A = (byte)(alpha * 255f);
			SetHighlightForAll(entityName, enabled, thickness, pulseTimeInFrames, color2, subPartNames);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For local player only.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGPSHighlight(string entityName, string GPSName, string GPSDescription, Color GPSColor, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, string subPartNames = null)
		{
			SetGPSHighlightInternal(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, playerId, subPartNames);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGPSHighlightForAll(string entityName, string GPSName, string GPSDescription, Color GPSColor, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), string subPartNames = null)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				SetGPSHighlight(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, item.Identity.IdentityId, subPartNames);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For local player only.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGPSHighlightNoSound(string entityName, string GPSName, string GPSDescription, Color GPSColor, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, string subPartNames = null)
		{
			SetGPSHighlightInternal(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, playerId, subPartNames, playSound: false);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Enables/disables highlight for specific entity and creates/deletes GPS attached to it. For all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGPSHighlightNoSoundForAll(string entityName, string GPSName, string GPSDescription, Color GPSColor, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), string subPartNames = null)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				SetGPSHighlightNoSound(entityName, GPSName, GPSDescription, GPSColor, enabled, thickness, pulseTimeInFrames, color, item.Identity.IdentityId, subPartNames);
			}
		}

		private static void SetGPSHighlightInternal(string entityName, string GPSName, string GPSDescription, Color GPSColor, bool enabled = true, int thickness = 10, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L, string subPartNames = null, bool playSound = true)
		{
			if (!MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				return;
			}
			if (playerId == -1)
			{
				playerId = GetLocalPlayerId();
			}
			new MyTuple<string, string>(entityName, GPSName);
			if (enabled)
			{
				MyGps gps = new MyGps
				{
					ShowOnHud = true,
					Name = GPSName,
					Description = GPSDescription,
					AlwaysVisible = true,
					IsObjective = true
				};
				if (GPSColor != Color.Transparent)
				{
					gps.GPSColor = GPSColor;
				}
				MySession.Static.Gpss.SendAddGps(playerId, ref gps, entity.EntityId, playSound);
			}
			else
			{
				IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, GPSName);
				if (gpsByName != null)
				{
					MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
				}
			}
			if (entity.Model != null)
			{
				SetHighlight(entityName, enabled, thickness, pulseTimeInFrames, color, playerId, subPartNames);
			}
		}

		public static void SetHighlight(MyHighlightData highlightData, long playerId)
		{
			MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
			bool flag = highlightData.Thickness > -1;
			int exclusiveKey = -1;
			if (m_playerIdsToHighlightData.ContainsKey(playerId))
			{
				exclusiveKey = m_playerIdsToHighlightData[playerId].Find((MyTuple<long, int> tuple) => tuple.Item1 == highlightData.EntityId).Item2;
				if (exclusiveKey == 0)
				{
					exclusiveKey = -1;
				}
			}
			if (exclusiveKey == -1)
			{
				if (!flag)
				{
					return;
				}
				component.ExclusiveHighlightAccepted += OnExclusiveHighlightAccepted;
				component.ExclusiveHighlightRejected += OnExclusiveHighlightRejected;
				if (!m_playerIdsToHighlightData.ContainsKey(playerId))
				{
					m_playerIdsToHighlightData.Add(playerId, new List<MyTuple<long, int>>());
				}
				m_playerIdsToHighlightData[playerId].Add(new MyTuple<long, int>(highlightData.EntityId, -1));
			}
			else if (!flag)
			{
				m_playerIdsToHighlightData[playerId].RemoveAll((MyTuple<long, int> tuple) => tuple.Item2 == exclusiveKey);
			}
			component.RequestHighlightChangeExclusive(highlightData, exclusiveKey);
		}

		private static void OnExclusiveHighlightRejected(MyHighlightData data, int exclusiveKey)
		{
			m_playerIdsToHighlightData[data.PlayerId].RemoveAll((MyTuple<long, int> tuple) => tuple.Item1 == data.EntityId);
			MySession.Static.GetComponent<MyHighlightSystem>().ExclusiveHighlightAccepted -= OnExclusiveHighlightAccepted;
		}

		private static void OnExclusiveHighlightAccepted(MyHighlightData data, int exclusiveKey)
		{
			if ((float)data.Thickness != -1f)
			{
				List<MyTuple<long, int>> list = m_playerIdsToHighlightData[data.PlayerId];
				int num = list.FindIndex((MyTuple<long, int> tuple) => tuple.Item1 == data.EntityId);
				if (num != -1)
				{
					MyTuple<long, int> myTuple = list[num];
					m_playerIdsToHighlightData[data.PlayerId][num] = new MyTuple<long, int>(myTuple.Item1, exclusiveKey);
					MySession.Static.GetComponent<MyHighlightSystem>().ExclusiveHighlightRejected -= OnExclusiveHighlightRejected;
				}
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Sets color of GPS for specific player. If 'playerId' is less or equal to 0, GPS will be modified for local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGPSColor(string name, Color newColor, long playerId = -1L)
		{
			IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName((playerId > 0) ? playerId : MySession.Static.LocalPlayerId, name);
			if (gpsByName != null)
			{
				MySession.Static.Gpss.ChangeColor((playerId > 0) ? playerId : MySession.Static.LocalPlayerId, gpsByName.Hash, newColor);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Adds GPS for specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPS(string name, string description, Vector3D position, Color GPSColor, int disappearsInS = 0, long playerId = -1L)
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyGps gps = new MyGps
			{
				ShowOnHud = true,
				Coords = position,
				Name = name,
				Description = description,
				AlwaysVisible = true
			};
			if (disappearsInS > 0)
			{
				TimeSpan value = TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double)disappearsInS);
				gps.DiscardAt = value;
			}
			else
			{
				gps.DiscardAt = null;
			}
			if (GPSColor != Color.Transparent)
			{
				gps.GPSColor = GPSColor;
			}
			MySession.Static.Gpss.SendAddGps(playerId, ref gps, 0L);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Adds GPS for all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSForAll(string name, string description, Vector3D position, Color GPSColor, int disappearsInS = 0)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				AddGPS(name, description, position, GPSColor, disappearsInS, item.Identity.IdentityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Removes GPS from specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveGPS(string name, long playerId = -1L)
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, name);
			if (gpsByName != null)
			{
				MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Removes GPS from all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveGPSForAll(string name)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				RemoveGPS(name, item.Identity.IdentityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for local player only.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSToEntity(string entityName, string GPSName, string GPSDescription, Color GPSColor, long playerId = -1L)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (playerId == -1)
				{
					playerId = GetLocalPlayerId();
				}
				new MyTuple<string, string>(entityName, GPSName);
				MyGps gps = new MyGps
				{
					ShowOnHud = true,
					Name = GPSName,
					DisplayName = GPSName,
					Description = GPSDescription,
					AlwaysVisible = true
				};
				if (GPSColor != Color.Transparent)
				{
					gps.GPSColor = GPSColor;
				}
				gps.DiscardAt = null;
				MySession.Static.Gpss.SendAddGps(playerId, ref gps, entity.EntityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for all players", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSToEntityForAll(string entityName, string GPSName, string GPSDescription, Color GPSColor)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				AddGPSToEntity(entityName, GPSName, GPSDescription, GPSColor, item.Identity.IdentityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Removes specific GPS from specific entity for local player only. ('GPSDescription' is not used. Cant remove due to backward compatibility.)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveGPSFromEntity(string entityName, string GPSName, string GPSDescription, long playerId = -1L)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var _))
			{
				if (playerId == -1)
				{
					playerId = GetLocalPlayerId();
				}
				new MyTuple<string, string>(entityName, GPSName);
				IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, GPSName);
				if (gpsByName != null)
				{
					MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
				}
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Removes specific GPS from specific entity for all players. ('GPSDescription' is not used. Cant remove due to backward compatibility.)", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveGPSFromEntityForAll(string entityName, string GPSName, string GPSDescription)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				RemoveGPSFromEntity(entityName, GPSName, GPSDescription, item.Identity.IdentityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Adds GPS for specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSObjective(string name, string description, Vector3D position, Color GPSColor, int disappearsInS = 0, long playerId = -1L)
		{
			if (playerId <= 0)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyGps gps = new MyGps
			{
				ShowOnHud = true,
				Coords = position,
				Name = name,
				Description = description,
				AlwaysVisible = true,
				IsObjective = true
			};
			if (disappearsInS > 0)
			{
				TimeSpan value = TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double)disappearsInS);
				gps.DiscardAt = value;
			}
			else
			{
				gps.DiscardAt = null;
			}
			if (GPSColor != Color.Transparent)
			{
				gps.GPSColor = GPSColor;
			}
			MySession.Static.Gpss.SendAddGps(playerId, ref gps, 0L);
		}

		[VisualScriptingMiscData("GPS and Highlights", "Adds GPS for all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSObjectiveForAll(string name, string description, Vector3D position, Color GPSColor, int disappearsInS = 0)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				AddGPSObjective(name, description, position, GPSColor, disappearsInS, item.Identity.IdentityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for local player only.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSObjectiveToEntity(string entityName, string GPSName, string GPSDescription, Color GPSColor, long playerId = -1L)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (playerId == -1)
				{
					playerId = GetLocalPlayerId();
				}
				new MyTuple<string, string>(entityName, GPSName);
				MyGps gps = new MyGps
				{
					ShowOnHud = true,
					Name = GPSName,
					Description = GPSDescription,
					AlwaysVisible = true,
					IsObjective = true
				};
				if (GPSColor != Color.Transparent)
				{
					gps.GPSColor = GPSColor;
				}
				gps.DiscardAt = null;
				MySession.Static.Gpss.SendAddGps(playerId, ref gps, entity.EntityId);
			}
		}

		[VisualScriptingMiscData("GPS and Highlights", "Creates GPS and attach it to entity for all players", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddGPSObjectiveToEntityForAll(string entityName, string GPSName, string GPSDescription, Color GPSColor)
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers == null || onlinePlayers.Count == 0)
			{
				return;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				AddGPSObjectiveToEntity(entityName, GPSName, GPSDescription, GPSColor, item.Identity.IdentityId);
			}
		}

		[VisualScriptingMiscData("Grid", "Returns list of all blocks of type 'blockId' on specific grid.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetIdListOfSpecificGridBlocks(string gridName, MyDefinitionId blockId)
		{
			List<long> list = new List<long>();
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
					{
						if (fatBlock != null && fatBlock.BlockDefinition != null && fatBlock.BlockDefinition.Id == blockId)
						{
							list.Add(fatBlock.EntityId);
						}
					}
					return list;
				}
			}
			return list;
		}

		[VisualScriptingMiscData("Grid", "Returns sums of current integrities, max integrities, block counts.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetGridStatistics(string gridName, out float currentIntegrity, out float maxIntegrity, out int blockCount)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			currentIntegrity = 0f;
			maxIntegrity = 0f;
			blockCount = 0;
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					Enumerator<MySlimBlock> enumerator = myCubeGrid.GetBlocks().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MySlimBlock current = enumerator.get_Current();
							currentIntegrity += current.Integrity;
							maxIntegrity += current.MaxIntegrity;
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					blockCount = myCubeGrid.BlocksCount;
					return true;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Grid", "Colors all blocks of specific grid.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ColorAllGridBlocks(string gridName, Color color, bool playSound = true)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					Vector3 newHSV = color.ColorToHSVDX11();
					myCubeGrid.ColorGrid(newHSV, playSound, validateOwnership: false);
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Returns entity id of main cockpit or first cockpit found on grid. Also returns other info such as if cockpit is main or if any cockpit was found.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetGridCockpitId(string gridName, out bool isMainCockpit, out bool found, bool checkForEnabledShipControl = true)
		{
			isMainCockpit = false;
			found = true;
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					if (myCubeGrid.MainCockpit != null)
					{
						isMainCockpit = true;
						return myCubeGrid.MainCockpit.EntityId;
					}
					foreach (MyCockpit fatBlock in myCubeGrid.GetFatBlocks<MyCockpit>())
					{
						if (fatBlock != null && (!checkForEnabledShipControl || fatBlock.EnableShipControl))
						{
							return fatBlock.EntityId;
						}
					}
				}
			}
			found = false;
			return 0L;
		}

		[VisualScriptingMiscData("Grid", "Returns count of all blocks of type 'blockId' on specific grid.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetCountOfSpecificGridBlocks(string gridName, MyDefinitionId blockId)
		{
			int result = -2;
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				result = -1;
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					result = 0;
					{
						foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
						{
							if (fatBlock != null && fatBlock.BlockDefinition != null && fatBlock.BlockDefinition.Id == blockId)
							{
								result++;
							}
						}
						return result;
					}
				}
			}
			return result;
		}

		[VisualScriptingMiscData("Grid", "Returns id of first block of type 'blockId' on specific grid.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetIdOfFirstSpecificGridBlock(string gridName, MyDefinitionId blockId)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
					{
						if (fatBlock != null && fatBlock.BlockDefinition != null && fatBlock.BlockDefinition.Id == blockId)
						{
							return fatBlock.EntityId;
						}
					}
				}
			}
			return 0L;
		}

		[VisualScriptingMiscData("Grid", "Sets state of Landing gears for whole grid.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridLandingGearsLock(string gridName, bool gearLock = true)
		{
			(GetEntityByName(gridName) as MyCubeGrid)?.GridSystems.LandingSystem.Switch(gearLock);
		}

		[VisualScriptingMiscData("Grid", "Returns true if any Landing gear of specific grid is in locked state.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGridLockedWithLandingGear(string gridName)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (myCubeGrid.GridSystems.LandingSystem.Locked != MyMultipleEnabledEnum.Mixed)
				{
					return myCubeGrid.GridSystems.LandingSystem.Locked == MyMultipleEnabledEnum.AllEnabled;
				}
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Grid", "Turns reactors of specific grid on/off.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridReactors(string gridName, bool turnOn = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				long playerId = -1L;
				if (myCubeGrid.BigOwners != null && myCubeGrid.BigOwners.Count > 0)
				{
					playerId = myCubeGrid.BigOwners[0];
				}
				if (turnOn)
				{
					myCubeGrid.GridSystems.SyncObject_PowerProducerStateChanged(MyMultipleEnabledEnum.AllEnabled, playerId);
				}
				else
				{
					myCubeGrid.GridSystems.SyncObject_PowerProducerStateChanged(MyMultipleEnabledEnum.AllDisabled, playerId);
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Enables/disables all weapons(MyUserControllableGun) on the specific grid.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridWeaponStatus(string gridName, bool enabled = true)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (MySlimBlock block in myCubeGrid.GetBlocks())
			{
				if (block.FatBlock is MyUserControllableGun)
				{
					((MyUserControllableGun)block.FatBlock).Enabled = enabled;
=======
			{
				return;
			}
			Enumerator<MySlimBlock> enumerator = myCubeGrid.GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock is MyUserControllableGun)
					{
						((MyUserControllableGun)current.FatBlock).Enabled = enabled;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		[VisualScriptingMiscData("Grid", "Gets number of blocks of specified type on the specific grid.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetNumberOfGridBlocks(string entityName, string blockTypeId, string blockSubtypeId)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(entityName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				int num = 0;
				bool flag = !string.IsNullOrEmpty(blockTypeId);
				bool flag2 = !string.IsNullOrEmpty(blockSubtypeId);
				{
					foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
					{
						if (flag2 && flag)
						{
							if (fatBlock.BlockDefinition.Id.SubtypeName == blockSubtypeId)
							{
								MyObjectBuilderType typeId = fatBlock.BlockDefinition.Id.TypeId;
								if (typeId.ToString() == blockTypeId)
								{
									num++;
								}
							}
						}
						else if (flag)
						{
							MyObjectBuilderType typeId = fatBlock.BlockDefinition.Id.TypeId;
							if (typeId.ToString() == blockTypeId)
							{
								num++;
							}
						}
						else if (flag2 && fatBlock.BlockDefinition.Id.SubtypeName == blockSubtypeId)
						{
							num++;
						}
					}
					return num;
				}
			}
			return 0;
		}

		[VisualScriptingMiscData("Grid", "Returns true if entity has thrusters in all directions.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool HasThrusterInAllDirections(string entityName)
		{
			MyEntity entityByName = GetEntityByName(entityName);
			if (entityByName == null)
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return false;
			}
			ResetThrustDirections();
			foreach (MyThrust fatBlock in myCubeGrid.GetFatBlocks<MyThrust>())
			{
				if (fatBlock.Enabled && Math.Abs(fatBlock.ThrustOverride) < 0.0001f && fatBlock.IsFunctional)
				{
					m_thrustDirections[fatBlock.ThrustForwardVector] = true;
				}
			}
			foreach (bool value in m_thrustDirections.Values)
			{
				if (!value)
				{
					return false;
				}
			}
			return true;
		}

		[VisualScriptingMiscData("Grid", "Returns true if grid has enough power or is in 'adaptable-overload'. (grid is overloaded by adaptable block, that won't cause blackout, such as thrusters or batteries)", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool HasPower(string gridName)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName == null)
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyResourceStateEnum myResourceStateEnum = myCubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
				if (myResourceStateEnum == MyResourceStateEnum.Ok || myResourceStateEnum == MyResourceStateEnum.OverloadAdaptible)
				{
					return true;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Grid", "Sets grid's power state.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridPowerState(string gridName, bool enabled)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName == null)
			{
				return;
			}
			MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
			if (myCubeGrid == null || myCubeGrid.GridSystems.ResourceDistributor == null)
			{
				return;
			}
			MyMultipleEnabledEnum state = ((!enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
			foreach (long bigOwner in myCubeGrid.BigOwners)
			{
				myCubeGrid.GridSystems.ResourceDistributor.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, state, bigOwner);
			}
		}

		[VisualScriptingMiscData("Grid", "Sets grid's power state by the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridPowerStateByPlayer(string gridName, bool enabled, long playerId)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.GridSystems.ResourceDistributor != null)
				{
					MyMultipleEnabledEnum state = ((!enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
					myCubeGrid.GridSystems.ResourceDistributor.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, state, playerId);
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Returns true if the specific grid has at least one gyro that is enabled, powered and not-overridden.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool HasOperationalGyro(string gridName)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName == null)
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return false;
			}
			MyFatBlockReader<MyGyro> fatBlocks = myCubeGrid.GetFatBlocks<MyGyro>();
			bool result = false;
			foreach (MyGyro item in fatBlocks)
			{
				if (item.Enabled && item.IsPowered && !item.GyroOverride)
				{
					return true;
				}
			}
			return result;
		}

		[VisualScriptingMiscData("Grid", "Returns true if the specific grid has at least one cockpit that enables ship control.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool HasOperationalCockpit(string gridName)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName == null)
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyFatBlockReader<MyCockpit> fatBlocks = myCubeGrid.GetFatBlocks<MyCockpit>();
				bool result = false;
				{
					foreach (MyCockpit item in fatBlocks)
					{
						if (item.EnableShipControl)
						{
							return true;
						}
					}
					return result;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Grid", "Returns true if the specified grid has at least one Remote in functional state.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool HasWorkingRemote(string gridName)
		{
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName == null)
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
			if (myCubeGrid != null)
			{
				foreach (MyRemoteControl fatBlock in myCubeGrid.GetFatBlocks<MyRemoteControl>())
				{
					if (fatBlock.IsFunctional)
					{
						return true;
					}
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Grid", "Returns true if the specified grid has at least one functional gyro, at least one controlling block (cockpit/remote) and thrusters in all directions.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsFlyable(string entityName)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(entityName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyResourceStateEnum myResourceStateEnum = myCubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
				if (myResourceStateEnum == MyResourceStateEnum.OverloadBlackout || myResourceStateEnum == MyResourceStateEnum.NoPower)
				{
					return false;
				}
				MyFatBlockReader<MyGyro> fatBlocks = myCubeGrid.GetFatBlocks<MyGyro>();
				bool flag = false;
				foreach (MyGyro item in fatBlocks)
				{
					if (item.Enabled && item.IsPowered && !item.GyroOverride)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				MyFatBlockReader<MyShipController> fatBlocks2 = myCubeGrid.GetFatBlocks<MyShipController>();
				bool flag2 = false;
				foreach (MyShipController item2 in fatBlocks2)
				{
					if (item2.EnableShipControl)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return false;
				}
				ResetThrustDirections();
				foreach (MyThrust fatBlock in myCubeGrid.GetFatBlocks<MyThrust>())
				{
					if (fatBlock.IsPowered && fatBlock.Enabled && Math.Abs(fatBlock.ThrustOverride) < 0.0001f)
					{
						m_thrustDirections[fatBlock.ThrustForwardVector] = true;
					}
				}
				foreach (bool value in m_thrustDirections.Values)
				{
					if (!value)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private static void ResetThrustDirections()
		{
			if (m_thrustDirections.Count == 0)
			{
				m_thrustDirections.Add(Vector3I.Forward, value: false);
				m_thrustDirections.Add(Vector3I.Backward, value: false);
				m_thrustDirections.Add(Vector3I.Left, value: false);
				m_thrustDirections.Add(Vector3I.Right, value: false);
				m_thrustDirections.Add(Vector3I.Up, value: false);
				m_thrustDirections.Add(Vector3I.Down, value: false);
			}
			else
			{
				m_thrustDirections[Vector3I.Forward] = false;
				m_thrustDirections[Vector3I.Backward] = false;
				m_thrustDirections[Vector3I.Left] = false;
				m_thrustDirections[Vector3I.Right] = false;
				m_thrustDirections[Vector3I.Up] = false;
				m_thrustDirections[Vector3I.Down] = false;
			}
		}

		[VisualScriptingMiscData("Grid", "Creates local blueprint for player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateLocalBlueprint(string gridName, string blueprintName, string blueprintDisplayName = null)
		{
			string text = Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "local");
			string path = Path.Combine(text, blueprintName, "bp.sbc");
			if (!MyFileSystem.DirectoryExists(text))
			{
				MyFileSystem.CreateDirectoryRecursive(text);
			}
			if (!MyFileSystem.DirectoryExists(text))
			{
				return;
			}
			if (blueprintDisplayName == null)
			{
				blueprintDisplayName = blueprintName;
			}
			MyEntity entityByName = GetEntityByName(gridName);
			if (entityByName != null)
			{
				MyCubeGrid myCubeGrid = entityByName as MyCubeGrid;
				if (myCubeGrid != null)
				{
					MyClipboardComponent.Static.Clipboard.CopyGroup(myCubeGrid, GridLinkTypeEnum.Logical);
					MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
					myObjectBuilder_ShipBlueprintDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(blueprintName));
					myObjectBuilder_ShipBlueprintDefinition.CubeGrids = MyClipboardComponent.Static.Clipboard.CopiedGrids.ToArray();
					myObjectBuilder_ShipBlueprintDefinition.RespawnShip = false;
					myObjectBuilder_ShipBlueprintDefinition.DisplayName = blueprintDisplayName;
					myObjectBuilder_ShipBlueprintDefinition.CubeGrids[0].DisplayName = blueprintDisplayName;
					MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
					myObjectBuilder_Definitions.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
					myObjectBuilder_Definitions.ShipBlueprints[0] = myObjectBuilder_ShipBlueprintDefinition;
					MyObjectBuilderSerializer.SerializeXML(path, compress: false, myObjectBuilder_Definitions);
					MyClipboardComponent.Static.Clipboard.Deactivate();
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Sets projection highlight for the specific projector block.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetHighlightForProjection(string projectorName, bool enabled = true, int thickness = 5, int pulseTimeInFrames = 120, Color color = default(Color), long playerId = -1L)
		{
			MyEntity entityByName = GetEntityByName(projectorName);
			if (entityByName == null || !(entityByName is MyProjectorBase))
			{
				return;
			}
			MyProjectorBase obj = (MyProjectorBase)entityByName;
			if (color == default(Color))
			{
				color = Color.Blue;
			}
			if (color == default(Color))
			{
				color = Color.Blue;
			}
			if (playerId == -1)
			{
				playerId = MySession.Static.LocalPlayerId;
			}
			MyHighlightData highlightData = new MyHighlightData
			{
				OutlineColor = color,
				PulseTimeInFrames = (ulong)pulseTimeInFrames,
				Thickness = (enabled ? thickness : (-1)),
				PlayerId = playerId,
				IgnoreUseObjectData = true
			};
			foreach (MyCubeGrid previewGrid in obj.Clipboard.PreviewGrids)
			{
				highlightData.EntityId = previewGrid.EntityId;
				SetHighlight(highlightData, highlightData.PlayerId);
			}
		}

		[VisualScriptingMiscData("Grid", "Returns true if grid is marked as destructible.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGridDestructible(string entityName)
		{
			return (GetEntityByName(entityName) as MyCubeGrid)?.DestructibleBlocks ?? true;
		}

		[VisualScriptingMiscData("Grid", "Un/Marks the specific grid as destructible. Such grid cannot be destroyed, but can be grinded", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridDestructible(string entityName, bool destructible = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(entityName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.DestructibleBlocks = destructible;
			}
		}

		[VisualScriptingMiscData("Grid", "Returns true if grid is marked as destructible.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGridImmune(string entityName)
		{
			return (GetEntityByName(entityName) as MyCubeGrid)?.Immune ?? true;
		}

		[VisualScriptingMiscData("Grid", "Un/Marks the specific grid as immune. Such grid cannot be destroyed or grinded.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridImmune(string entityName, bool immune = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(entityName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.Immune = immune;
			}
		}

		[VisualScriptingMiscData("Grid", "Returns true if the specific grid is marked as editable.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGridEditable(string entityName)
		{
			return (GetEntityByName(entityName) as MyCubeGrid)?.Editable ?? true;
		}

		[VisualScriptingMiscData("Grid", "Un/Marks the specific grid as editable.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridEditable(string entityName, bool editable = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(entityName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.Editable = editable;
			}
		}

		[VisualScriptingMiscData("Grid", "Sets the specific grid as static/dynamic.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridStatic(string gridName, bool isStatic = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (isStatic)
				{
					myCubeGrid.RequestConversionToStation();
				}
				else
				{
					myCubeGrid.RequestConversionToShip(null);
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Sets grid general damage modifier that multiplies all damage received by that grid.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridGeneralDamageModifier(string gridName, float modifier = 1f)
		{
			if (MyEntities.TryGetEntityByName(gridName, out var entity) && entity is MyCubeGrid)
			{
				((MyCubeGrid)entity).GridGeneralDamageModifier.Value = modifier;
			}
		}

		[VisualScriptingMiscData("Grid", "Enables/disables all functional blocks on the specified grid.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridBlocksEnabled(string gridName, bool enabled = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
			{
				if (fatBlock is MyFunctionalBlock)
				{
					((MyFunctionalBlock)fatBlock).Enabled = enabled;
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Sets all terminal blocks of specified grid to be (not) shown in terminal screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridBlocksShowInTerminal(string gridName, bool showInTerminal = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
			{
				if (fatBlock is MyTerminalBlock)
				{
					((MyTerminalBlock)fatBlock).ShowInTerminal = showInTerminal;
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Sets all terminal blocks of specified grid to be (not) shown in inventory screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridBlocksShowInInventory(string gridName, bool showInInventory = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
			{
				if (fatBlock is MyTerminalBlock)
				{
					((MyTerminalBlock)fatBlock).ShowInInventory = showInInventory;
				}
			}
		}

		[VisualScriptingMiscData("Grid", "Sets all terminal blocks of specified grid to be (not) shown on HUD.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetGridBlocksShowOnHUD(string gridName, bool showOnHUD = true)
		{
			MyCubeGrid myCubeGrid = GetEntityByName(gridName) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
			{
				if (fatBlock is MyTerminalBlock)
				{
					((MyTerminalBlock)fatBlock).ShowOnHUD = showOnHUD;
				}
			}
		}

		[VisualScriptingMiscData("G-Screen", "Enables/disables toolbar config screen (G-screen).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EnableToolbarConfig(bool flag)
		{
			MyPerGameSettings.GUI.EnableToolbarConfigScreen = flag;
		}

		[VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Hide empty groups.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ToolbarConfigGroupsHideEmpty()
		{
			MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideEmpty;
		}

		[VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Hide all.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ToolbarConfigGroupsHideAll()
		{
			MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideAll;
		}

		[VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Hide block groups.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ToolbarConfigGroupsHideBlockGroups()
		{
			MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideBlockGroups;
		}

		[VisualScriptingMiscData("G-Screen", "Sets group mode of toolbar config screen (G-screen) to Default.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ToolbarConfigGroupsDefualtBehavior()
		{
			MyGuiScreenToolbarConfigBase.GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.Default;
		}

		[VisualScriptingMiscData("G-Screen", "Adds specific item into research.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ResearchListAddItem(MyDefinitionId itemId)
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.AddRequiredResearch(itemId);
			}
		}

		[VisualScriptingMiscData("G-Screen", "Removes specific item from research.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ResearchListRemoveItem(MyDefinitionId itemId)
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.RemoveRequiredResearch(itemId);
			}
		}

		[VisualScriptingMiscData("G-Screen", "Clears required research list for all.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ResearchListClear()
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.ClearRequiredResearch();
			}
		}

		[VisualScriptingMiscData("G-Screen", "Resets research for all.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlayerResearchClearAll()
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.ResetResearchForAll();
			}
		}

		[VisualScriptingMiscData("G-Screen", "Resets research for the specific player. If 'playerId' equals -1, resets research for the local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlayerResearchClear(long playerId = -1L)
		{
			if (playerId == -1 && MySession.Static.LocalCharacter != null)
			{
				playerId = MySession.Static.LocalCharacter.GetPlayerIdentityId();
			}
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.ResetResearch(playerId);
			}
		}

		[VisualScriptingMiscData("G-Screen", "[OBSOLETE] Enables/disables research whitelist mode.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ResearchListWhitelist(bool whitelist)
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.SwitchWhitelistMode(whitelist);
			}
		}

		[VisualScriptingMiscData("G-Screen", "Unlocks the specific research for the specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlayerResearchUnlock(long playerId, MyDefinitionId itemId)
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.UnlockResearchDirect(playerId, itemId);
			}
		}

		[VisualScriptingMiscData("G-Screen", "Locks the specific research for the specific player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void PlayerResearchLock(long playerId, MyDefinitionId itemId)
		{
			if (MySessionComponentResearch.Static != null)
			{
				MySessionComponentResearch.Static.LockResearch(playerId, itemId);
			}
		}

		[VisualScriptingMiscData("GUI", "Gets whole item grid and find index of specific item in it. If no item was found, method will still return the item grid and index will be set to last index in it (GetItemsCount() - 1). Works only when ToolbarConfig screen is opened and focused.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static void GetToolbarConfigGridItemIndexAndControl(MyDefinitionId itemDefinition, out MyGuiControlBase control, out int index)
		{
			control = null;
			index = -1;
			MyGuiScreenToolbarConfigBase openedToolbarConfig = GetOpenedToolbarConfig();
			if (openedToolbarConfig == null)
			{
				return;
			}
			control = openedToolbarConfig.GetControlByName("ScrollablePanel\\Grid");
			MyGuiControlGrid myGuiControlGrid = control as MyGuiControlGrid;
			if (myGuiControlGrid == null)
			{
				return;
			}
			for (index = 0; index < myGuiControlGrid.GetItemsCount(); index++)
			{
				MyGuiGridItem itemAt = myGuiControlGrid.GetItemAt(index);
				if (itemAt != null && itemAt.UserData != null)
				{
					MyObjectBuilder_ToolbarItemDefinition myObjectBuilder_ToolbarItemDefinition = ((MyGuiScreenToolbarConfigBase.GridItemUserData)itemAt.UserData).ItemData() as MyObjectBuilder_ToolbarItemDefinition;
					if (myObjectBuilder_ToolbarItemDefinition != null && myObjectBuilder_ToolbarItemDefinition.DefinitionId == itemDefinition)
					{
						break;
					}
				}
			}
		}

		[VisualScriptingMiscData("GUI", "Gets whole inventory grid of player and find index of specific item in it. If no item was found, method will still return inventory grid and index will be set to last index in it (GetItemsCount() - 1). Works only when Terminal screen is opened and focused.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static void GetPlayersInventoryItemIndexAndControl(MyDefinitionId itemDefinition, out MyGuiControlBase control, out int index)
		{
			control = null;
			index = -1;
			MyGuiScreenTerminal openedTerminal = GetOpenedTerminal();
			if (openedTerminal == null)
			{
				return;
			}
			control = openedTerminal.GetControlByName("TerminalTabs\\PageInventory\\LeftInventory\\MyGuiControlInventoryOwner\\InventoryGrid");
			MyGuiControlGrid myGuiControlGrid = control as MyGuiControlGrid;
			if (myGuiControlGrid != null)
			{
				index = 0;
				while (index < myGuiControlGrid.GetItemsCount() && !(((MyPhysicalInventoryItem)myGuiControlGrid.GetItemAt(index).UserData).GetDefinitionId() == itemDefinition))
				{
					index++;
				}
			}
		}

		[VisualScriptingMiscData("GUI", "Gets whole inventory grid of interacted entity and find index of specific item in it. If no item was found, method will still return inventory grid and index will be set to last index in it (GetItemsCount() - 1). Works only when Terminal screen is opened and focused.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static void GetInteractedEntityInventoryItemIndexAndControl(MyDefinitionId itemDefinition, out MyGuiControlBase control, out int index)
		{
			control = null;
			index = -1;
			MyGuiScreenTerminal openedTerminal = GetOpenedTerminal();
			if (openedTerminal == null)
<<<<<<< HEAD
			{
				return;
			}
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = openedTerminal.GetControlByName("TerminalTabs\\PageInventory\\RightInventory\\MyGuiControlInventoryOwner") as MyGuiControlInventoryOwner;
			if (myGuiControlInventoryOwner == null)
			{
				return;
			}
			foreach (MyGuiControlGrid contentGrid in myGuiControlInventoryOwner.ContentGrids)
			{
=======
			{
				return;
			}
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = openedTerminal.GetControlByName("TerminalTabs\\PageInventory\\RightInventory\\MyGuiControlInventoryOwner") as MyGuiControlInventoryOwner;
			if (myGuiControlInventoryOwner == null)
			{
				return;
			}
			foreach (MyGuiControlGrid contentGrid in myGuiControlInventoryOwner.ContentGrids)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (contentGrid == null)
				{
					continue;
				}
				control = contentGrid;
				for (index = 0; index < contentGrid.GetItemsCount(); index++)
				{
					if (((MyPhysicalInventoryItem)contentGrid.GetItemAt(index).UserData).GetDefinitionId() == itemDefinition)
					{
						return;
					}
				}
			}
		}

		[VisualScriptingMiscData("GUI", "Opens service overlay. If playerID is 0, open it for local player else open it for targeted player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void OpenSteamOverlay(string url, long playerId = 0L)
		{
			MyPlayer.PlayerId result;
			if (playerId == 0L)
			{
				OpenSteamOverlaySync(url);
			}
			else if (MySession.Static.Players.TryGetPlayerId(playerId, out result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OpenSteamOverlaySync, url, new EndpointId(result.SteamId));
			}
		}

<<<<<<< HEAD
		[Event(null, 5807)]
=======
		[Event(null, 5804)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void OpenSteamOverlaySync(string url)
		{
			if (MyGuiSandbox.IsUrlWhitelisted(url))
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
			}
		}

		public static void OpenSteamOverlayLocal(string url)
		{
			OpenSteamOverlaySync(url);
		}

		[VisualScriptingMiscData("GUI", "Highlights specific GUI element in specific screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void HighlightGuiControl(string controlName, string activeScreenName)
		{
			foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
			{
				if (!(screen.Name == activeScreenName))
				{
					continue;
				}
				foreach (MyGuiControlBase control2 in screen.Controls)
				{
					if (control2.Name == controlName)
					{
						MyGuiScreenHighlight.MyHighlightControl control = default(MyGuiScreenHighlight.MyHighlightControl);
						control.Control = control2;
						MyGuiScreenHighlight.HighlightControl(control);
					}
				}
			}
		}

		[VisualScriptingMiscData("GUI", "Highlights specific GUI element. If the element is of type MyGuiControlGrid, 'indicies' may be used to select which items should be highlighted. 'customToolTipMessage' can be used for custom tooltip of highlighted element.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void HighlightGuiControl(MyGuiControlBase control, List<int> indicies = null, string customToolTipMessage = null)
		{
			if (control != null)
			{
				MyGuiScreenHighlight.MyHighlightControl myHighlightControl = default(MyGuiScreenHighlight.MyHighlightControl);
				myHighlightControl.Control = control;
				MyGuiScreenHighlight.MyHighlightControl control2 = myHighlightControl;
				if (indicies != null)
				{
					control2.Indices = indicies.ToArray();
				}
				if (!string.IsNullOrEmpty(customToolTipMessage))
				{
					control2.CustomToolTips = new MyToolTips(customToolTipMessage);
				}
				MyGuiScreenHighlight.HighlightControl(control2);
			}
		}

		[VisualScriptingMiscData("GUI", "Gets GUI element by name from the specific screen. You may search through hierarchy of controls by connecting element names with '\\\\'. Such as 'GrandParent\\\\Parent\\\\Child' will return element of name 'Child' that is under element 'Parent' that is under element 'GrandParent' which is in screen. In case specific element was not found, returned element will be the closest parent that was found.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyGuiControlBase GetControlByName(this MyGuiScreenBase screen, string controlName)
		{
			if (string.IsNullOrEmpty(controlName) || screen == null)
			{
				return null;
			}
			string[] array = controlName.Split(new char[1] { '\\' });
			MyGuiControlBase controlByName = screen.Controls.GetControlByName(array[0]);
			for (int i = 1; i < array.Length; i++)
			{
				MyGuiControlParent myGuiControlParent = controlByName as MyGuiControlParent;
				if (myGuiControlParent != null)
				{
					controlByName = myGuiControlParent.Controls.GetControlByName(array[i]);
					continue;
				}
				MyGuiControlScrollablePanel myGuiControlScrollablePanel = controlByName as MyGuiControlScrollablePanel;
				if (myGuiControlScrollablePanel != null)
				{
					controlByName = myGuiControlScrollablePanel.Controls.GetControlByName(array[i]);
					continue;
				}
				if (controlByName != null)
				{
					controlByName = controlByName.Elements.GetControlByName(array[i]);
				}
				break;
			}
			return controlByName;
		}

		[VisualScriptingMiscData("GUI", "Gets GUI element by name from the specific Gui element. You may search through hierarchy of controls by connecting element names with '\\'. Such as 'GrandParent\\Parent\\Child' will return element of name 'Child' that is under element 'Parent' that is under element 'GrandParent' which is in screen. In case specific element was not found, returned element will be the closest parent that was found.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyGuiControlBase GetControlByName(this MyGuiControlParent control, string controlName)
		{
			if (string.IsNullOrEmpty(controlName) || control == null)
			{
				return null;
			}
			string[] array = controlName.Split(new char[1] { '\\' });
			MyGuiControlBase controlByName = control.Controls.GetControlByName(array[0]);
			for (int i = 1; i < array.Length; i++)
			{
				MyGuiControlParent myGuiControlParent = controlByName as MyGuiControlParent;
				if (myGuiControlParent != null)
				{
					controlByName = myGuiControlParent.Controls.GetControlByName(array[i]);
					continue;
				}
				MyGuiControlScrollablePanel myGuiControlScrollablePanel = controlByName as MyGuiControlScrollablePanel;
				if (myGuiControlScrollablePanel != null)
				{
					controlByName = myGuiControlScrollablePanel.Controls.GetControlByName(array[i]);
					continue;
				}
				if (controlByName != null)
				{
					controlByName = controlByName.Elements.GetControlByName(array[i]);
				}
				break;
			}
			return controlByName;
		}

		[VisualScriptingMiscData("GUI", "Sets tooltip of specific GUI element.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetTooltip(this MyGuiControlBase control, string text)
		{
			control?.SetToolTip(text);
		}

		public static void SetEnabledByExperimental(this MyGuiControlBase control)
		{
			if (!MySandboxGame.Config.ExperimentalMode)
			{
				control.Enabled = false;
				control.ShowTooltipWhenDisabled = true;
				control.SetToolTip(MyTexts.GetString(MyCommonTexts.ExperimentalRequired));
			}
		}

		public static void SetDisabledByExperimental(this MyGuiControlBase control)
		{
			if (!MySandboxGame.Config.ExperimentalMode)
			{
				control.Enabled = false;
				control.ShowTooltipWhenDisabled = true;
				control.SetToolTip(MyTexts.GetString(MyCommonTexts.ExperimentalRequiredToDisable));
			}
		}

		[VisualScriptingMiscData("GUI", "Gets currently opened terminal screen. (only if it is focused)", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyGuiScreenTerminal GetOpenedTerminal()
		{
			return MyScreenManager.GetScreenWithFocus() as MyGuiScreenTerminal;
		}

		[VisualScriptingMiscData("GUI", "Gets tab on specific index of specified TabControl element.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyGuiControlTabPage GetTab(this MyGuiControlTabControl tabs, int key)
		{
			return tabs?.GetTabSubControl(key);
		}

		[VisualScriptingMiscData("GUI", "Gets TabControl elements of specific terminal screen.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyGuiControlTabControl GetTabs(this MyGuiScreenTerminal terminal)
		{
			if (terminal == null)
			{
				return null;
			}
			return terminal.Controls.GetControlByName("TerminalTabs") as MyGuiControlTabControl;
		}

		[VisualScriptingMiscData("GUI", "Gets currently opened ToolbarConfig screen (G-Screen). (only if it is focused)", -10510688)]
		[VisualScriptingMember(false, false)]
		public static MyGuiScreenToolbarConfigBase GetOpenedToolbarConfig()
		{
			return MyScreenManager.GetScreenWithFocus() as MyGuiScreenToolbarConfigBase;
		}

		[VisualScriptingMiscData("GUI", "Returns true if specific key was pressed in this frame.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsNewKeyPressed(MyKeys key)
		{
			return MyInput.Static.IsNewKeyPressed(key);
		}

		[VisualScriptingMiscData("GUI", "Gets friendly name of the specific screen.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetFriendlyName(this MyGuiScreenBase screen)
		{
			return screen.GetFriendlyName();
		}

		[VisualScriptingMiscData("GUI", "Changes selected page of TabControl element to specific page.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPage(this MyGuiControlTabControl pageControl, int pageNumber)
		{
			pageControl.SelectedPage = pageNumber;
		}

		[VisualScriptingMiscData("GUI", "Changes faction Score", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetFactionScore(long factionId, int score)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				myFaction.Score = score;
			}
		}

		[VisualScriptingMiscData("GUI", "Changes escape pod finished percentage in scoreCounter.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetFactionObjectivePercentageCompleted(long factionId, float finished)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				myFaction.ObjectivePercentageCompleted = finished;
			}
		}

		[VisualScriptingMiscData("GUI", "Open Player screen to all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void OpenPlayerScreenAll()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OpenPlayerScreenAllBroadcast);
		}

<<<<<<< HEAD
		[Event(null, 6036)]
=======
		[Event(null, 6033)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		public static void OpenPlayerScreenAllBroadcast()
		{
			if (!Sync.IsDedicated && MyMultiplayer.Static != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenPlayers>(Array.Empty<object>()));
			}
		}

		[VisualScriptingMiscData("GUI", "Open Player screen to all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void OpenFactionVictoryScreenAll(string factionTag, float timeInSec)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OpenFactionVictoryScreenAllBroadcast, factionTag, timeInSec);
		}

<<<<<<< HEAD
		[Event(null, 6058)]
=======
		[Event(null, 6055)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		public static void OpenFactionVictoryScreenAllBroadcast(string factionTag, float timeInSec)
		{
			if (!Sync.IsDedicated)
			{
				MyGuiScreenVictory myGuiScreenVictory = MyGuiSandbox.CreateScreen<MyGuiScreenVictory>(Array.Empty<object>());
				myGuiScreenVictory.SetWinner(factionTag);
				myGuiScreenVictory.SetDuration(timeInSec);
				MyGuiSandbox.AddScreen(myGuiScreenVictory);
			}
		}

		[VisualScriptingMiscData("Misc", "Takes a screenshot and saves it to specific destination.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void TakeScreenshot(string destination, string name)
		{
			string pathToSave = Path.Combine(destination, name, ".png");
			MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), pathToSave, debug: false, ignoreSprites: true, showNotification: false);
		}

		[VisualScriptingMiscData("Misc", "Returns path to where game content is located.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetContentPath()
		{
			return MyFileSystem.ContentPath;
		}

		[VisualScriptingMiscData("Misc", "Returns path to where game is being saved.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetSavesPath()
		{
			return MyFileSystem.SavesPath;
		}

		[VisualScriptingMiscData("Misc", "Returns path to where mods are being stored.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetModsPath()
		{
			return MyFileSystem.ModsPath;
		}

		[VisualScriptingMiscData("Misc", "Sets custom image for a loading screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetCustomLoadingScreenImage(string imagePath)
		{
			MySession.Static.CustomLoadingScreenImage = imagePath;
		}

		[VisualScriptingMiscData("Misc", "Sets custom loading text for a loading screen", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetCustomLoadingScreenText(string text)
		{
			MySession.Static.CustomLoadingScreenText = text;
		}

		[VisualScriptingMiscData("Misc", "Sets custom skybox for the current game.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetCustomSkybox(string skyboxPath)
		{
			MySession.Static.CustomSkybox = skyboxPath;
		}

		[VisualScriptingMiscData("Misc", "Gets name of the control element (keyboard, mouse, gamepad buttons) that is binded to the specific action called 'keyName'. Names are defined in class MyControlsSpace, such as 'STRAFE_LEFT' or 'CUBE_ROTATE_ROLL_POSITIVE'.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetUserControlKey(string keyName)
		{
			MyStringId orCompute = MyStringId.GetOrCompute(keyName);
			MyControl gameControl = MyInput.Static.GetGameControl(orCompute);
			if (gameControl != null)
			{
				return gameControl.ToString();
			}
			return "";
		}

		[VisualScriptingMiscData("Misc", "Creates a new color out of red, green and blue. All values must be in range <0;1>.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Color GetColor(float r = 0f, float g = 0f, float b = 0f)
		{
			r = MathHelper.Clamp(r, 0f, 1f);
			g = MathHelper.Clamp(g, 0f, 1f);
			b = MathHelper.Clamp(b, 0f, 1f);
			return new Color(r, g, b);
		}

		[VisualScriptingMiscData("Notifications", "Shows a notification with specific message and font for the specific player for a defined time. If playerId is equal to 0, notification will be show to local player, otherwise it will be shown to specific player. If playerId is -1, notification is shown to all players.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ShowNotification(string message, int disappearTimeMs, string font = "White", long playerId = 0L)
		{
			switch (playerId)
			{
			case 0L:
				if (MyAPIGateway.Utilities != null)
				{
					MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
				}
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowNotificationToAllSync, message, disappearTimeMs, font);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowNotificationSync, message, disappearTimeMs, font, new EndpointId(result.SteamId));
			}
		}

		[VisualScriptingMiscData("Notifications", "Shows a notification with specific message and font to all players for a defined time.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ShowNotificationToAll(string message, int disappearTimeMs, string font = "White")
		{
			if (MyMultiplayer.Static == null)
			{
				if (MyAPIGateway.Utilities != null)
				{
					MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowNotificationToAllSync, message, disappearTimeMs, font);
			}
		}

		[VisualScriptingMiscData("Notifications", "Sends a scripted chat message under name 'author' to all players (if playerId equal to 0), or to one specific player. In case of singleplayer, message will shown to local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SendChatMessage(string message, string author = "", long playerId = 0L, string font = "Blue")
		{
			if (MyMultiplayer.Static != null)
			{
				ScriptedChatMsg msg = default(ScriptedChatMsg);
				msg.Text = message;
				msg.Author = author;
				msg.Target = playerId;
				msg.Font = font;
				msg.Color = Color.White;
				MyMultiplayerBase.SendScriptedChatMessage(ref msg);
			}
			else
			{
				MyHud.Chat.multiplayer_ScriptedChatMessageReceived(message, author, font, Color.White);
			}
		}

		[VisualScriptingMiscData("Notifications", "Sends a scripted chat message under name 'author' to all players (if playerId equal to 0), or to one specific player. In case of singleplayer, message will shown to local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SendChatMessageColored(string message, Color color, string author = "", long playerId = 0L, string font = "White")
		{
			if (MyMultiplayer.Static != null)
			{
				ScriptedChatMsg msg = default(ScriptedChatMsg);
				msg.Text = message;
				msg.Author = author;
				msg.Target = playerId;
				msg.Font = font;
				msg.Color = color;
				MyMultiplayerBase.SendScriptedChatMessage(ref msg);
			}
			else
			{
				MyHud.Chat.multiplayer_ScriptedChatMessageReceived(message, author, font, color);
			}
		}

		[VisualScriptingMiscData("Notifications", "Sets for how long chat messages should be shown before fading out.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetChatMessageDuration(int durationS = 15)
		{
			MyHudChat.MaxMessageTime = durationS * 1000;
		}

		[VisualScriptingMiscData("Notifications", "[Obsolete] Sets maximum count of messages in chat. [Has no effect anymore as whole history is being kept. Number of shown messages is dependant on number of rows they cover.]", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetChatMaxMessageCount(int count = 10)
		{
			MyHudChat.MaxMessageCount = count;
		}

<<<<<<< HEAD
		[Event(null, 6250)]
=======
		[Event(null, 6247)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void ShowNotificationSync(string message, int disappearTimeMs, string font = "White")
		{
			if (MyAPIGateway.Utilities != null)
			{
				MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
			}
		}

		public static void ShowNotificationLocal(string message, int disappearTimeMs, string font = "White")
		{
			ShowNotificationSync(message, disappearTimeMs, font);
		}

<<<<<<< HEAD
		[Event(null, 6263)]
=======
		[Event(null, 6260)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		[Server]
		private static void ShowNotificationToAllSync(string message, int disappearTimeMs, string font = "White")
		{
			if (MyAPIGateway.Utilities != null)
			{
				MyAPIGateway.Utilities.ShowNotification(message, disappearTimeMs, font);
			}
		}

		[VisualScriptingMiscData("Notifications", "Adds a new notification for the specific player and returns if of the notification. Returns -1 if no player corresponds to 'playerId'. For 'playerId' equal to 0 use local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static int AddNotification(string message, string font = "White", long playerId = 0L)
		{
			int num = m_notificationIdCounter++;
			switch (playerId)
			{
			case 0L:
				AddNotificationLocal(message, font, num);
				return num;
			case -1L:
				AddNotificationToAllSync(message, font, num);
				return num;
			default:
			{
				if (!MySession.Static.Players.TryGetPlayerId(playerId, out var result))
				{
					return -1;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AddNotificationSync, message, font, num, new EndpointId(result.SteamId));
				return num;
			}
			}
		}

<<<<<<< HEAD
		[Event(null, 6301)]
=======
		[Event(null, 6298)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void AddNotificationSync(string message, string font, int notificationId)
		{
			MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute(message), 0, font);
			MyHud.Notifications.Add(myHudNotification);
			m_addedNotificationsById[notificationId] = myHudNotification;
		}

		public static void AddNotificationLocal(string message, string font, int notificationId)
<<<<<<< HEAD
		{
			AddNotificationSync(message, font, notificationId);
		}

		[Event(null, 6316)]
		[Reliable]
		[Broadcast]
		[Server]
		private static void AddNotificationToAllSync(string message, string font, int notificationId)
		{
			MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute(message), 0, font);
			MyHud.Notifications.Add(myHudNotification);
			m_addedNotificationsById[notificationId] = myHudNotification;
		}

		[VisualScriptingMiscData("Notifications", "Removes the specific notification referenced by its id from the specific player. If 'playerId' is equal to 0, apply on local player, if -1, apply to all.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveNotification(int messageId, long playerId = -1L)
		{
=======
		{
			AddNotificationSync(message, font, notificationId);
		}

		[Event(null, 6313)]
		[Reliable]
		[Broadcast]
		[Server]
		private static void AddNotificationToAllSync(string message, string font, int notificationId)
		{
			MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute(message), 0, font);
			MyHud.Notifications.Add(myHudNotification);
			m_addedNotificationsById[notificationId] = myHudNotification;
		}

		[VisualScriptingMiscData("Notifications", "Removes the specific notification referenced by its id from the specific player. If 'playerId' is equal to 0, apply on local player, if -1, apply to all.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveNotification(int messageId, long playerId = -1L)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (playerId == 0L)
			{
				RemoveNotificationSync(messageId, -1L);
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveNotificationSync, messageId, playerId);
		}

<<<<<<< HEAD
		[Event(null, 6339)]
=======
		[Event(null, 6336)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveNotificationSync(int messageId, long playerId = -1L)
		{
			if ((playerId == -1 || MySession.Static.LocalPlayerId == playerId) && m_addedNotificationsById.TryGetValue(messageId, out var value))
			{
				MyHud.Notifications.Remove(value);
				m_addedNotificationsById.Remove(messageId);
			}
		}

		public static void RemoveNotificationLocal(int messageId, long playerId = -1L)
		{
			RemoveNotificationSync(messageId, playerId);
		}

		[VisualScriptingMiscData("Notifications", "Clears all added notifications.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ClearNotifications(long playerId = -1L)
		{
			if (playerId == 0L)
			{
				ClearNotificationSync(-1L);
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ClearNotificationSync, playerId);
		}

<<<<<<< HEAD
		[Event(null, 6374)]
=======
		[Event(null, 6371)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ClearNotificationSync(long playerId = -1L)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				MyHud.Notifications.Clear();
				m_notificationIdCounter = 0;
				m_addedNotificationsById.Clear();
			}
		}

		public static void ClearNotificationLocal(long playerId = -1L)
		{
			ClearNotificationSync(playerId);
		}

		[VisualScriptingMiscData("Notifications", "Display congratulation screen to playet/s. Use MessageId to select which message should be shown. If player id is 1-, show to all. If it is 0, show to local player. Else it will be used as player identity id.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void DisplayCongratulationScreen(int congratulationMessageId, long playerId)
		{
			switch (playerId)
			{
			case 0L:
				DisplayCongratulationScreenInternal(congratulationMessageId);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DisplayCongratulationScreenInternalAll, congratulationMessageId);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DisplayCongratulationScreenInternal, congratulationMessageId, new EndpointId(result.SteamId));
			}
		}

<<<<<<< HEAD
		[Event(null, 6414)]
=======
		[Event(null, 6411)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		private static void DisplayCongratulationScreenInternal(int congratulationMessageId)
		{
			DisplayCongratulationScreen(congratulationMessageId);
		}

<<<<<<< HEAD
		[Event(null, 6420)]
=======
		[Event(null, 6417)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void DisplayCongratulationScreenInternalAll(int congratulationMessageId)
		{
			DisplayCongratulationScreen(congratulationMessageId);
		}

		private static void DisplayCongratulationScreen(int congratulationMessageId)
		{
			MyScreenManager.AddScreen(new MyGuiScreenCongratulation(congratulationMessageId));
		}

		private static MyCharacter GetCharacterFromPlayerId(long playerId = 0L)
		{
			if (playerId != 0L)
			{
				return MySession.Static.Players.TryGetIdentity(playerId)?.Character;
			}
			return MySession.Static.LocalCharacter;
		}

		private static MyIdentity GetIdentityFromPlayerId(long playerId = 0L)
		{
			if (playerId != 0L)
			{
				return MySession.Static.Players.TryGetIdentity(playerId);
			}
			if (MySession.Static.LocalHumanPlayer != null)
			{
				return MySession.Static.LocalHumanPlayer.Identity;
			}
			return null;
		}

		private static MyPlayer GetPlayerFromPlayerId(long playerId = 0L)
		{
			if (playerId != 0L)
			{
				MyPlayer player = null;
				if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
				{
					MySession.Static.Players.TryGetPlayerById(result, out player);
					return player;
				}
				return null;
			}
			return MySession.Static.LocalHumanPlayer;
		}

		[VisualScriptingMiscData("Player", "Gets online players.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetOnlinePlayers()
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			List<long> list = new List<long>();
			if (onlinePlayers != null && onlinePlayers.Count > 0)
			{
				foreach (MyPlayer item in onlinePlayers)
				{
					list.Add(item.Identity.IdentityId);
				}
				return list;
			}
			return list;
		}

		[VisualScriptingMiscData("Player", "Gets online and local players count.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetPlayersCount()
		{
			return GetPlayers().Count;
		}

		[VisualScriptingMiscData("Player", "Gets online and local players.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetPlayers()
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			List<long> list = new List<long>();
			if (onlinePlayers != null && onlinePlayers.Count > 0)
			{
				foreach (MyPlayer item in onlinePlayers)
				{
					list.Add(item.Identity.IdentityId);
				}
			}
			if (MySession.Static.LocalPlayerId != 0L && !list.Contains(MySession.Static.LocalPlayerId))
			{
				list.Add(MySession.Static.LocalPlayerId);
			}
			return list;
		}

		[VisualScriptingMiscData("Player", "Get admins.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> GetAdmins()
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			List<long> list = new List<long>();
			if (onlinePlayers != null && onlinePlayers.Count > 0)
			{
				foreach (MyPlayer item in onlinePlayers)
				{
					if (MySession.Static.GetUserPromoteLevel(item.Id.SteamId) == MyPromoteLevel.Admin)
					{
						list.Add(item.Identity.IdentityId);
					}
				}
				return list;
			}
			return list;
		}

		[VisualScriptingMiscData("Player", "Gets oxygen level at player's position.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetOxygenLevelAtPlayersPosition(long playerId = 0L)
		{
			if (MySession.Static.Settings.EnableOxygenPressurization && MySession.Static.Settings.EnableOxygen)
			{
				MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
				if (characterFromPlayerId != null && characterFromPlayerId.OxygenComponent != null)
				{
					return characterFromPlayerId.OxygenLevelAtCharacterLocation;
				}
			}
			return 1f;
		}

		[VisualScriptingMiscData("Player", "Gets player's helmet status.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetPlayersHelmetStatus(long playerId = 0L)
		{
			if (MySession.Static.Settings.EnableOxygenPressurization && MySession.Static.Settings.EnableOxygen)
			{
				MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
				if (characterFromPlayerId != null && characterFromPlayerId.OxygenComponent != null)
				{
					return characterFromPlayerId.OxygenComponent.HelmetEnabled;
				}
			}
			return false;
		}

		[VisualScriptingMiscData("Player", "Toggle broadcasting for the player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void TogglePlayersBroadcasting(long playerId = 0L, bool enabled = true)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId != null && Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(characterFromPlayerId, (MyCharacter x) => x.EnableBroadcasting, enabled);
			}
		}

		[VisualScriptingMiscData("Player", "Toggle player's ability to toggle broadcasting. Server still can change player's broadcasting state.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void TogglePlayersBroadcastEnabling(long playerId = 0L, bool enabled = true)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId != null && Sync.IsServer)
			{
				characterFromPlayerId.EnableBroadcastingPlayerToggle(enabled);
			}
		}

		[VisualScriptingMiscData("Player", "Sets player's helmet status.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersHelmetStatus(long playerId = 0L, bool status = true)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId != null && characterFromPlayerId.OxygenComponent != null && characterFromPlayerId.OxygenComponent.HelmetEnabled != status)
			{
				((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)characterFromPlayerId).SwitchHelmet();
			}
		}

		[VisualScriptingMiscData("Player", "Gets player's controlled cube block (grid).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool GetPlayerControlledBlockData(out string controlType, out long blockId, out string blockName, out long gridId, out string gridName, out bool isRespawnShip, long playerId = 0L)
		{
			controlType = null;
			blockId = 0L;
			blockName = null;
			gridId = 0L;
			gridName = null;
			isRespawnShip = false;
			MyPlayer playerFromPlayerId = GetPlayerFromPlayerId(playerId);
			if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null && playerFromPlayerId.Controller.ControlledEntity.Entity is MyCubeBlock)
			{
				MyCubeBlock myCubeBlock = (MyCubeBlock)playerFromPlayerId.Controller.ControlledEntity.Entity;
				controlType = ((myCubeBlock is MyCockpit) ? "Cockpit" : ((myCubeBlock is MyRemoteControl) ? "Remote" : ((myCubeBlock is MyUserControllableGun) ? "Turret" : "Other")));
				blockId = myCubeBlock.EntityId;
				blockName = myCubeBlock.Name;
				gridId = myCubeBlock.CubeGrid.EntityId;
				gridName = myCubeBlock.CubeGrid.Name;
				isRespawnShip = myCubeBlock.CubeGrid.IsRespawnGrid;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Player", "Gets players entity ID.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long GetPlayersEntityId(long playerId = 0L)
		{
			return GetIdentityFromPlayerId(playerId)?.Character.EntityId ?? 0;
		}

		[VisualScriptingMiscData("Player", "Gets players entity name.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetPlayersEntityName(long playerId = 0L)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId != null)
			{
				return characterFromPlayerId.Name;
			}
			return "";
		}

		[VisualScriptingMiscData("Player", "Gets player's speed (linear velocity).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D GetPlayersSpeed(long playerId = 0L)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId != null)
			{
				return characterFromPlayerId.Physics.LinearVelocity;
			}
			return Vector3D.Zero;
		}

		[VisualScriptingMiscData("Player", "Sets player's speed (linear velocity).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersSpeed(Vector3D speed = default(Vector3D), long playerId = 0L)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId == null)
			{
				return;
			}
			if (speed != Vector3D.Zero)
			{
				float num = Math.Max(characterFromPlayerId.Definition.MaxSprintSpeed, Math.Max(characterFromPlayerId.Definition.MaxRunSpeed, characterFromPlayerId.Definition.MaxBackrunSpeed));
				float num2 = MyGridPhysics.ShipMaxLinearVelocity() + num;
				if (speed.LengthSquared() > (double)(num2 * num2))
				{
					speed.Normalize();
					speed *= (double)num2;
				}
			}
			characterFromPlayerId.Physics.LinearVelocity = speed;
		}

		[VisualScriptingMiscData("Player", "Sets player's color (RGB).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersColorInRGB(long playerId = 0L, Color colorRBG = default(Color))
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			characterFromPlayerId?.ChangeModelAndColor(characterFromPlayerId.ModelName, colorRBG.ColorToHSVDX11(), resetToDefault: false, 0L);
		}

		[VisualScriptingMiscData("Player", "Sets player's color (HSV).", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersColorInHSV(long playerId = 0L, Vector3 colorHSV = default(Vector3))
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			characterFromPlayerId?.ChangeModelAndColor(characterFromPlayerId.ModelName, colorHSV, resetToDefault: false, 0L);
		}

		[VisualScriptingMiscData("Player", "Checks if player is in cockpit.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsPlayerInCockpit(long playerId = 0L, string gridName = null, string cockpitName = null)
		{
			MyPlayer playerFromPlayerId = GetPlayerFromPlayerId(playerId);
			MyCockpit myCockpit = null;
			if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null)
			{
				myCockpit = playerFromPlayerId.Controller.ControlledEntity.Entity as MyCockpit;
			}
			if (myCockpit == null)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(gridName) && myCockpit.CubeGrid.Name != gridName)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(cockpitName) && myCockpit.Name != cockpitName)
			{
				return false;
			}
			return true;
		}

		[VisualScriptingMiscData("Player", "Checks if player is controlling something over remote.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsPlayerInRemote(long playerId = 0L, string gridName = null, string remoteName = null)
		{
			MyPlayer playerFromPlayerId = GetPlayerFromPlayerId(playerId);
			MyRemoteControl myRemoteControl = null;
			if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null)
			{
				myRemoteControl = playerFromPlayerId.Controller.ControlledEntity.Entity as MyRemoteControl;
			}
			if (myRemoteControl == null)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(gridName) && myRemoteControl.CubeGrid.Name != gridName)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(remoteName) && myRemoteControl.Name != remoteName)
			{
				return false;
			}
			return true;
		}

		[VisualScriptingMiscData("Player", "Checks if player is controlling weapon.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsPlayerInWeapon(long playerId = 0L, string gridName = null, string weaponName = null)
		{
			MyPlayer playerFromPlayerId = GetPlayerFromPlayerId(playerId);
			MyUserControllableGun myUserControllableGun = null;
			if (playerFromPlayerId != null && playerFromPlayerId.Controller != null && playerFromPlayerId.Controller.ControlledEntity != null)
			{
				myUserControllableGun = playerFromPlayerId.Controller.ControlledEntity.Entity as MyUserControllableGun;
			}
			if (myUserControllableGun == null)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(gridName) && myUserControllableGun.CubeGrid.Name != gridName)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(weaponName) && myUserControllableGun.Name != weaponName)
			{
				return false;
			}
			return true;
		}

		[VisualScriptingMiscData("Player", "Checks if player is dead.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsPlayerDead(long playerId = 0L)
		{
			return GetCharacterFromPlayerId(playerId)?.IsDead ?? false;
		}

		[VisualScriptingMiscData("Player", "Gets player's name.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetPlayersName(long playerId = 0L)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null)
			{
				return identityFromPlayerId.DisplayName;
			}
			return "";
		}

		[VisualScriptingMiscData("Player", "Gets player's health.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetPlayersHealth(long playerId = 0L)
		{
			return GetIdentityFromPlayerId(playerId)?.Character.StatComp.Health.Value ?? (-1f);
		}

		[VisualScriptingMiscData("Player", "Checks if player's jetpack is enabled.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsPlayersJetpackEnabled(long playerId = 0L)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null && identityFromPlayerId.Character != null && identityFromPlayerId.Character.JetpackComp != null)
			{
				return identityFromPlayerId.Character.JetpackComp.TurnedOn;
			}
			return false;
		}

		[VisualScriptingMiscData("Player", "Gets oxygen level of player's suit.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetPlayersOxygenLevel(long playerId = 0L)
		{
			return GetIdentityFromPlayerId(playerId)?.Character.OxygenComponent.SuitOxygenLevel ?? (-1f);
		}

		[VisualScriptingMiscData("Player", "Gets hydrogen level of player's suit.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetPlayersHydrogenLevel(long playerId = 0L)
		{
			return GetIdentityFromPlayerId(playerId)?.Character.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) ?? (-1f);
		}

		[VisualScriptingMiscData("Player", "Gets energy level of player's suit.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetPlayersEnergyLevel(long playerId = 0L)
		{
			return GetIdentityFromPlayerId(playerId)?.Character.SuitEnergyLevel ?? (-1f);
		}

		[VisualScriptingMiscData("Player", "Sets player's health.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersHealth(long playerId = 0L, float value = 100f)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null && identityFromPlayerId.Character != null && identityFromPlayerId.Character.StatComp != null)
			{
				float value2 = identityFromPlayerId.Character.StatComp.Health.Value;
				if (value < value2)
				{
					float num = value2 - value;
					identityFromPlayerId.Character.StatComp.DoDamage(num, new MyDamageInformation(isDeformation: false, num, DAMAGE_TYPE_SCRIPT, 0L));
				}
				else
				{
					identityFromPlayerId.Character.StatComp.Health.Value = value;
				}
			}
		}

		[VisualScriptingMiscData("Player", "Sets oxygen level of the player's suit.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersOxygenLevel(long playerId = 0L, float value = 1f)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null && identityFromPlayerId.Character != null && identityFromPlayerId.Character.OxygenComponent != null)
			{
				identityFromPlayerId.Character.OxygenComponent.SuitOxygenLevel = value;
			}
		}

		[VisualScriptingMiscData("Player", "Sets hydrogen level of the player's suit.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersHydrogenLevel(long playerId = 0L, float value = 1f)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null)
			{
				MyDefinitionId gasId = MyCharacterOxygenComponent.HydrogenId;
				identityFromPlayerId?.Character?.OxygenComponent?.UpdateStoredGasLevel(ref gasId, value);
			}
		}

		[VisualScriptingMiscData("Player", "Sets energy level of the player's suit.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersEnergyLevel(long playerId = 0L, float value = 1f)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null)
			{
				identityFromPlayerId?.Character?.SuitBattery?.ResourceSource?.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, value * 1E-05f);
			}
		}

		[VisualScriptingMiscData("Player", "Sets player's position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayersPosition(long playerId = 0L, Vector3D position = default(Vector3D))
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId == null)
			{
				return;
			}
			if (identityFromPlayerId?.Character != null)
			{
				if (identityFromPlayerId.Character.IsOnLadder)
				{
					identityFromPlayerId.Character.GetOffLadder();
				}
				if (identityFromPlayerId.Character.IsUsing is MyCockpit)
				{
					((MyCockpit)identityFromPlayerId.Character.IsUsing).RemovePilot();
				}
			}
			identityFromPlayerId?.Character?.PositionComp?.SetPosition(position);
		}

		[VisualScriptingMiscData("Player", "Gets player's position.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D GetPlayersPosition(long playerId = 0L)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null && identityFromPlayerId.Character != null && identityFromPlayerId.Character.PositionComp != null)
			{
				return identityFromPlayerId.Character.PositionComp.GetPosition();
			}
			return Vector3D.Zero;
		}

		[VisualScriptingMiscData("Player", "Gets player's inventory item amount.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetPlayersInventoryItemAmount(long playerId = 0L, MyDefinitionId itemId = default(MyDefinitionId))
		{
			return (int)Math.Round(GetPlayersInventoryItemAmountPrecise(playerId, itemId));
		}

		[VisualScriptingMiscData("Player", "Gets player's inventory item amount (precise).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetPlayersInventoryItemAmountPrecise(long playerId = 0L, MyDefinitionId itemId = default(MyDefinitionId))
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null && !itemId.TypeId.IsNull && identityFromPlayerId.Character != null)
			{
				return (float)GetInventoryItemAmount(identityFromPlayerId.Character, itemId);
			}
			return 0f;
		}

		[VisualScriptingMiscData("Player", "Adds the specified item to the player's inventory.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddToPlayersInventory(long playerId = 0L, MyDefinitionId itemId = default(MyDefinitionId), int amount = 1)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null && identityFromPlayerId.Character != null)
			{
				MyInventory inventory = identityFromPlayerId.Character.GetInventory();
				if (inventory != null)
				{
					MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(itemId);
					MyFixedPoint myFixedPoint = default(MyFixedPoint);
					myFixedPoint = amount;
					inventory.AddItems(myFixedPoint, objectBuilder);
				}
			}
		}

		[VisualScriptingMiscData("Player", "Removes the specified item from the player's inventory.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveFromPlayersInventory(long playerId = 0L, MyDefinitionId itemId = default(MyDefinitionId), int amount = 1)
		{
			MyIdentity identityFromPlayerId = GetIdentityFromPlayerId(playerId);
			if (identityFromPlayerId != null)
			{
				MyInventory inventory = identityFromPlayerId.Character.GetInventory();
				if (inventory != null)
				{
					MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(itemId);
					MyFixedPoint myFixedPoint = default(MyFixedPoint);
					myFixedPoint = amount;
					MyFixedPoint itemAmount = inventory.GetItemAmount(itemId);
					inventory.RemoveItemsOfType((myFixedPoint < itemAmount) ? myFixedPoint : itemAmount, objectBuilder);
				}
			}
		}

		[VisualScriptingMiscData("Player", "Sets player's damage modifier.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayerGeneralDamageModifier(long playerId = 0L, float modifier = 1f)
		{
			MyCharacter myCharacter = null;
			if (playerId > 0)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(playerId);
				if (myIdentity != null)
				{
					myCharacter = myIdentity.Character;
				}
			}
			else
			{
				myCharacter = MySession.Static.LocalCharacter;
			}
			if (myCharacter != null)
			{
				myCharacter.CharacterGeneralDamageModifier = modifier;
			}
		}

		[VisualScriptingMiscData("Player", "Spawns player on the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnPlayer(MatrixD worldMatrix, Vector3D velocity = default(Vector3D), long playerId = 0L)
		{
			if (!MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				return;
			}
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
			if (playerById == null)
<<<<<<< HEAD
			{
				return;
			}
			if (playerById.Character != null && !playerById.Character.IsDead)
			{
				if (playerById.Character.IsOnLadder)
				{
					playerById.Character.GetOffLadder();
				}
				if (playerById.Character.IsUsing is MyCockpit)
				{
					((MyCockpit)playerById.Character.IsUsing).RemovePilot();
				}
=======
			{
				return;
			}
			if (playerById.Character != null && !playerById.Character.IsDead)
			{
				if (playerById.Character.IsOnLadder)
				{
					playerById.Character.GetOffLadder();
				}
				if (playerById.Character.IsUsing is MyCockpit)
				{
					((MyCockpit)playerById.Character.IsUsing).RemovePilot();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				playerById.Character.PositionComp.SetWorldMatrix(ref worldMatrix);
			}
			else
			{
				playerById.SpawnAt(worldMatrix, velocity, null, null);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CloseRespawnScreen, new EndpointId(playerById.Id.SteamId));
			}
		}

<<<<<<< HEAD
		[Event(null, 7062)]
=======
		[Event(null, 7060)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void CloseRespawnScreen()
		{
			Sync.Players.RespawnComponent.CloseRespawnScreenNow();
		}

		[VisualScriptingMiscData("Player", "Sets player's input black list. Enables/Disables specified control of the character.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetPlayerInputBlacklistState(string controlStringId, long playerId = -1L, bool enabled = false)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetPlayerInputBlacklistStateSync, controlStringId, playerId, enabled);
		}

<<<<<<< HEAD
		[Event(null, 7075)]
=======
		[Event(null, 7073)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetPlayerInputBlacklistStateSync(string controlStringId, long playerId = -1L, bool enabled = false)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				MyInput.Static.SetControlBlock(MyStringId.GetOrCompute(controlStringId), !enabled);
			}
		}

		[VisualScriptingMiscData("Player", "Toggles the players ability to sprint", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ToggleAbilityToSprint(long playerId = 0L, bool canSprint = true)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ToggleAbilityToSprintSync, playerId, canSprint);
		}

<<<<<<< HEAD
		[Event(null, 7090)]
=======
		[Event(null, 7088)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		public static void ToggleAbilityToSprintSync(long playerId = 0L, bool canSprint = true)
		{
			MyCharacter characterFromPlayerId = GetCharacterFromPlayerId(playerId);
			if (characterFromPlayerId != null)
			{
				characterFromPlayerId.CanSprint = canSprint;
			}
		}

		[VisualScriptingMiscData("Questlog", "Sets title and visibility of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetQuestlog(bool visible = true, string questName = "", long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetQuestlogSync, visible, questName, arg);
		}

<<<<<<< HEAD
		[Event(null, 7111)]
=======
		[Event(null, 7109)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetQuestlogSync(bool visible = true, string questName = "", long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.TryCancelObjective();
				if (visible && !MyHud.Questlog.Visible)
				{
					PlayHudSound(MyGuiSounds.HudGPSNotification3, playerId);
				}
				MyHud.Questlog.QuestTitle = questName;
				MyHud.Questlog.CleanDetails();
				MyHud.Questlog.Visible = visible;
				MyHud.Questlog.IsUsedByVisualScripting = visible;
			}
		}

		public static void SetQuestlogLocal(bool visible = true, string questName = "", long playerId = -1L)
		{
			SetQuestlogSync(visible, questName, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Sets title of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetQuestlogTitle(string questName = "", long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetQuestlogTitleSync, questName, arg);
		}

<<<<<<< HEAD
		[Event(null, 7147)]
=======
		[Event(null, 7145)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetQuestlogTitleSync(string questName = "", long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MyHud.Questlog.QuestTitle = questName;
			}
		}

		public static void SetQuestlogTitleLocal(string questName = "", long playerId = -1L)
		{
			SetQuestlogTitleSync(questName, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Sets detail of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static int AddQuestlogDetail(string questDetailRow = "", bool completePrevious = true, bool useTyping = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AddQuestlogDetailSync, questDetailRow, completePrevious, useTyping, arg);
			return MyHud.Questlog.GetQuestGetails().Length - 1;
		}

<<<<<<< HEAD
		[Event(null, 7171)]
=======
		[Event(null, 7169)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void AddQuestlogDetailSync(string questDetailRow = "", bool completePrevious = true, bool useTyping = true, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					PlayHudSound(MyGuiSounds.HudQuestlogDetail, playerId);
				}
				MyHud.Questlog.AddDetail(questDetailRow, useTyping);
				int num = MyHud.Questlog.GetQuestGetails().Length - 1;
				if (completePrevious)
				{
					PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
					MyHud.Questlog.SetCompleted(num - 1);
				}
			}
		}

		public static void AddQuestlogDetailLocal(string questDetailRow = "", bool completePrevious = true, bool useTyping = true, long playerId = -1L)
		{
			AddQuestlogDetailSync(questDetailRow, completePrevious, useTyping, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Sets objective of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static int AddQuestlogObjective(string questDetailRow = "", bool completePrevious = true, bool useTyping = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AddQuestlogObjectiveSync, questDetailRow, completePrevious, useTyping, arg);
			return MyHud.Questlog.GetQuestGetails().Length - 1;
		}

<<<<<<< HEAD
		[Event(null, 7209)]
=======
		[Event(null, 7207)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void AddQuestlogObjectiveSync(string questDetailRow = "", bool completePrevious = true, bool useTyping = true, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					PlayHudSound(MyGuiSounds.HudQuestlogDetail, playerId);
				}
				MyHud.Questlog.AddDetail(questDetailRow, useTyping, isObjective: true);
				int num = MyHud.Questlog.GetQuestGetails().Length - 1;
				if (completePrevious)
				{
					PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
					MyHud.Questlog.SetCompleted(num - 1);
				}
			}
		}

		public static void AddQuestlogObjectiveLocal(string questDetailRow = "", bool completePrevious = true, bool useTyping = true, long playerId = -1L)
		{
			AddQuestlogObjectiveSync(questDetailRow, completePrevious, useTyping, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Sets completed of the quest detail for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetQuestlogDetailCompleted(int lineId = 0, bool completed = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetQuestlogDetailCompletedSync, lineId, completed, arg);
		}

<<<<<<< HEAD
		[Event(null, 7246)]
=======
		[Event(null, 7244)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetQuestlogDetailCompletedSync(int lineId = 0, bool completed = true, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				if (completed && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
				}
				MyHud.Questlog.SetCompleted(lineId, completed);
			}
		}

		public static void SetQuestlogDetailCompletedLocal(int lineId = 0, bool completed = true, long playerId = -1L)
		{
			SetQuestlogDetailCompletedSync(lineId, completed, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Sets completed on all quest details for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetAllQuestlogDetailsCompleted(bool completed = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetAllQuestlogDetailsCompletedSync, completed, arg);
		}

<<<<<<< HEAD
		[Event(null, 7271)]
=======
		[Event(null, 7269)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetAllQuestlogDetailsCompletedSync(bool completed = true, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				if (completed && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					PlayHudSound(MyGuiSounds.HudObjectiveComplete, playerId);
				}
				MyHud.Questlog.SetAllCompleted(completed);
			}
		}

		public static void SetAllQuestlogDetailsCompletedLocal(bool completed = true, long playerId = -1L)
		{
			SetAllQuestlogDetailsCompletedSync(completed, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Replaces detail of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ReplaceQuestlogDetail(int id = 0, string newDetail = "", bool useTyping = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ReplaceQuestlogDetailSync, id, newDetail, useTyping, arg);
		}

<<<<<<< HEAD
		[Event(null, 7295)]
=======
		[Event(null, 7293)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ReplaceQuestlogDetailSync(int id = 0, string newDetail = "", bool useTyping = true, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MyHud.Questlog.ModifyDetail(id, newDetail, useTyping);
			}
		}

		public static void ReplaceQuestlogDetailLocal(int id = 0, string newDetail = "", bool useTyping = true, long playerId = -1L)
		{
			ReplaceQuestlogDetailSync(id, newDetail, useTyping, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Removes details of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveQuestlogDetails(long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveQuestlogDetailsSync, arg);
		}

<<<<<<< HEAD
		[Event(null, 7316)]
=======
		[Event(null, 7314)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveQuestlogDetailsSync(long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MyHud.Questlog.CleanDetails();
			}
		}

		public static void RemoveQuestlogDetailsLocal(long playerId = -1L)
		{
			RemoveQuestlogDetailsSync(playerId);
		}

		[VisualScriptingMiscData("Questlog", "Obsolete. Does not do anything.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetQuestlogPage(int value = 0, long playerId = -1L)
		{
<<<<<<< HEAD
		}

		[Event(null, 7340)]
=======
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetQuestlogPageSync, value, arg);
		}

		[Event(null, 7335)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		[Obsolete]
		private static void SetQuestlogPageSync(int value = 0, long playerId = -1L)
		{
		}

		[VisualScriptingMiscData("Questlog", "Obsolete. Returns -1.", -10510688)]
		[VisualScriptingMember(false, false)]
		[Obsolete]
		public static int GetQuestlogPage()
		{
			return -1;
		}

		[VisualScriptingMiscData("Questlog", "Obsolete. Returns -1.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetQuestlogMaxPages()
		{
			return -1;
		}

		[VisualScriptingMiscData("Questlog", "Sets visible of the quest for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetQuestlogVisible(bool value = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetQuestlogVisibleSync, value, arg);
		}

<<<<<<< HEAD
		[Event(null, 7374)]
=======
		[Event(null, 7369)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetQuestlogVisibleSync(bool value = true, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				if (value && !MyHud.Questlog.Visible && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					PlayHudSound(MyGuiSounds.HudGPSNotification3, playerId);
				}
				MyHud.Questlog.Visible = value;
			}
		}

		public static void SetQuestlogVisibleLocal(bool value = true, long playerId = -1L)
		{
			SetQuestlogVisibleSync(value, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Obsolete. Returns -1.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetQuestlogPageFromMessage(int id = 0)
		{
			return -1;
		}

		[VisualScriptingMiscData("Questlog", "Enables highlight of the questlog for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void EnableHighlight(bool enable = true, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EnableHighlightSync, enable, arg);
		}

<<<<<<< HEAD
		[Event(null, 7406)]
=======
		[Event(null, 7401)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void EnableHighlightSync(bool enable = true, long playerId = -1L)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && (playerId == -1 || MySession.Static.LocalPlayerId == playerId))
			{
				MyHud.Questlog.HighlightChanges = enable;
			}
		}

		public static void EnableHighlightLocal(bool enable = true, long playerId = -1L)
		{
			EnableHighlightSync(enable, playerId);
		}

		[VisualScriptingMiscData("Questlog", "Returns true if all essential hints have been completed.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AreEssentialGoodbotHintsDone()
		{
			List<string> tutorialsFinished = MySandboxGame.Config.TutorialsFinished;
			HashSet<string> essentialObjectiveIds = MySessionComponentIngameHelp.EssentialObjectiveIds;
			int num = 0;
			foreach (string item in tutorialsFinished)
			{
				if (essentialObjectiveIds.Contains(item))
				{
					num++;
				}
			}
			return num == essentialObjectiveIds.get_Count();
		}

		[VisualScriptingMiscData("Spawn", "Spawns the group of prefabs at the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnGroup(string subtypeId, Vector3D position, Vector3D direction, Vector3D up, long ownerId = 0L, string newGridName = null)
		{
			ListReader<MySpawnGroupDefinition> spawnGroupDefinitions = MyDefinitionManager.Static.GetSpawnGroupDefinitions();
			MySpawnGroupDefinition mySpawnGroupDefinition = null;
			foreach (MySpawnGroupDefinition item in spawnGroupDefinitions)
			{
				if (item.Id.SubtypeName == subtypeId)
				{
					mySpawnGroupDefinition = item;
					break;
				}
			}
			if (mySpawnGroupDefinition == null)
			{
				return;
			}
			List<MyCubeGrid> tmpGridList = new List<MyCubeGrid>();
			direction.Normalize();
			up.Normalize();
			MatrixD matrix = MatrixD.CreateWorld(position, direction, up);
<<<<<<< HEAD
			Action item = delegate
=======
			Action action = delegate
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (newGridName != null && tmpGridList.Count > 0)
				{
					tmpGridList[0].Name = newGridName;
					MyEntities.SetEntityName(tmpGridList[0]);
				}
			};
<<<<<<< HEAD
			Stack<Action> stack = new Stack<Action>();
			stack.Push(item);
			foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in mySpawnGroupDefinition.Prefabs)
			{
				Vector3D position2 = Vector3D.Transform((Vector3D)prefab.Position, matrix);
				MyPrefabManager.Static.SpawnPrefab(tmpGridList, prefab.SubtypeId, position2, direction, up, prefab.Speed * direction, default(Vector3), prefab.BeaconText, null, SpawningOptions.RotateFirstCockpitTowardsDirection, ownerId, updateSync: true, stack);
=======
			Stack<Action> val = new Stack<Action>();
			val.Push(action);
			foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in mySpawnGroupDefinition.Prefabs)
			{
				Vector3D position2 = Vector3D.Transform((Vector3D)prefab.Position, matrix);
				MyPrefabManager.Static.SpawnPrefab(tmpGridList, prefab.SubtypeId, position2, direction, up, prefab.Speed * direction, default(Vector3), prefab.BeaconText, null, SpawningOptions.RotateFirstCockpitTowardsDirection, ownerId, updateSync: true, val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		[VisualScriptingMiscData("Spawn", "Spawns local blueprint at the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnLocalBlueprint(string name, Vector3D position, Vector3D direction = default(Vector3D), string newGridName = null, long ownerId = 0L)
		{
			SpawnAlignedToGravityWithOffset(name, position, direction, newGridName, ownerId);
		}

		[VisualScriptingMiscData("Spawn", "Spawns local blueprint at the specified position and aligned to gravity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnLocalBlueprintInGravity(string name, Vector3D position, float rotationAngle = 0f, float gravityOffset = 0f, string newGridName = null, long ownerId = 0L)
		{
			SpawnAlignedToGravityWithOffset(name, position, default(Vector3D), newGridName, ownerId, gravityOffset, rotationAngle);
		}

		[VisualScriptingMiscData("Spawn", "Spawns the item at the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnItem(MyDefinitionId itemId, Vector3D position, string inheritsVelocityFrom = "", float amount = 1f)
		{
			MyFixedPoint amount2 = (MyFixedPoint)amount;
			MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(itemId);
			if (myObjectBuilder_PhysicalObject != null)
			{
				MyPhysicalInventoryItem item = new MyPhysicalInventoryItem(amount2, myObjectBuilder_PhysicalObject);
				MyPhysicsComponentBase component = null;
				if (!string.IsNullOrEmpty(inheritsVelocityFrom) && MyEntities.TryGetEntityByName(inheritsVelocityFrom, out var entity))
				{
					entity.Components.TryGet<MyPhysicsComponentBase>(out component);
				}
				Vector3D forward = Vector3D.Forward;
				Vector3D vector3D = Vector3D.Up;
				Vector3D vector3D2 = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
				if (vector3D2 != Vector3D.Zero)
				{
					vector3D = Vector3D.Normalize(vector3D2) * -1.0;
					forward = ((vector3D != Vector3D.Right) ? Vector3D.Cross(vector3D, Vector3D.Right) : Vector3D.Forward);
				}
				MyFloatingObjects.Spawn(item, position, forward, vector3D, component);
			}
		}

		private static void SpawnAlignedToGravityWithOffset(string name, Vector3D position, Vector3D direction, string newGridName, long ownerId = 0L, float gravityOffset = 0f, float gravityRotation = 0f)
		{
			string path = Path.Combine(Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "local"), name, "bp.sbc");
			MyObjectBuilder_ShipBlueprintDefinition[] array = null;
			if (MyFileSystem.FileExists(path))
			{
				if (!MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_Definitions objectBuilder))
				{
					return;
				}
				array = objectBuilder.ShipBlueprints;
			}
			if (array == null)
			{
				return;
			}
			Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
			if (vector == Vector3.Zero)
			{
				vector = MyGravityProviderSystem.CalculateArtificialGravityInPoint(position);
			}
			Vector3D vector3D;
			if (vector != Vector3.Zero)
			{
				vector.Normalize();
				vector3D = -vector;
				position += vector * gravityOffset;
				if (direction == Vector3D.Zero)
				{
					direction = Vector3D.CalculatePerpendicularVector(vector);
					if (gravityRotation != 0f)
					{
						MatrixD matrix = MatrixD.CreateFromAxisAngle(vector3D, gravityRotation);
						direction = Vector3D.Transform(direction, matrix);
					}
				}
			}
			else if (direction == Vector3D.Zero)
			{
				direction = Vector3D.Right;
				vector3D = Vector3D.Up;
			}
			else
			{
				vector3D = Vector3D.CalculatePerpendicularVector(-direction);
			}
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			MyObjectBuilder_ShipBlueprintDefinition[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				MyObjectBuilder_CubeGrid[] cubeGrids = array2[i].CubeGrids;
				foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in cubeGrids)
				{
					myObjectBuilder_CubeGrid.CreatePhysics = true;
					myObjectBuilder_CubeGrid.EnableSmallToLargeConnections = true;
					if (!string.IsNullOrEmpty(newGridName))
					{
						myObjectBuilder_CubeGrid.Name = ((list.Count > 0) ? (newGridName + list.Count) : newGridName);
					}
					list.Add(myObjectBuilder_CubeGrid);
				}
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyHud.PushRotatingWheelVisible();
			}
			MatrixD worldMatrix = MatrixD.CreateWorld(position, direction, vector3D);
			MyCubeGrid.RelocateGrids(list, worldMatrix);
			MyCubeGrid.RelativeOffset offset = default(MyCubeGrid.RelativeOffset);
			offset.Use = false;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyCubeGrid.TryPasteGrid_Implementation, new MyCubeGrid.MyPasteGridParameters(list, detectDisconnects: false, multiBlock: false, Vector3.Zero, instantBuild: true, offset, MySession.Static.GetComponent<MySessionComponentDLC>().GetAvailableClientDLCsIds()));
		}

		[VisualScriptingMiscData("Spawn", "Spawns the bot at the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnBot(string subtypeName, Vector3D position)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_AnimalBot), subtypeName);
			if (MyDefinitionManager.Static.TryGetBotDefinition(id, out var botDefinition) && botDefinition != null)
			{
				MyAIComponent.Static.SpawnNewBot(botDefinition as MyAgentDefinition, position, createdByPlayer: false);
			}
		}

		[VisualScriptingMiscData("Spawn", "Spawns the bot at the specified position, orientation and specific data.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static long SpawnBot(string subtypeName, Vector3D position, Vector3D direction, Vector3D up, string name)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_AnimalBot), subtypeName);
			if (!MyDefinitionManager.Static.TryGetBotDefinition(id, out var botDefinition))
			{
				return 0L;
			}
			long result = 0L;
			Vector3? up2 = null;
			Vector3? direction2 = null;
			if (!direction.IsZero())
			{
				direction2 = direction;
			}
			if (!up.IsZero())
			{
				up2 = up;
			}
			if (botDefinition != null)
			{
				result = MyAIComponent.Static.SpawnNewBotSync(botDefinition as MyAgentDefinition, Sync.MyId, position, direction2, up2, name);
			}
			return result;
		}

		[VisualScriptingMiscData("Spawn", "Spawns the prefab at the specified position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnPrefab(string prefabName, Vector3D position, Vector3D direction, Vector3D up, long ownerId = 0L, string beaconName = null, string entityName = null, SpawningOptions spawningOptions = SpawningOptions.RotateFirstCockpitTowardsDirection)
		{
			if (MyPrefabManager.Static == null)
			{
				MyLog.Default.WriteLine("Spawn Prefab failed. Prefab manager is not initialized.");
				return;
			}
			direction.Normalize();
			up.Normalize();
			if (direction.IsValid() && up.IsValid() && position.IsValid())
			{
				MyPrefabManager.Static.SpawnPrefab(prefabName, position, direction, up, default(Vector3), default(Vector3), beaconName, entityName, spawningOptions, ownerId, updateSync: true);
			}
		}

		[VisualScriptingMiscData("Spawn", "Spawns the prefab at the specified position aligned to gravity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SpawnPrefabInGravity(string prefabName, Vector3D position, Vector3D direction, long ownerId = 0L, string beaconName = null, string entityName = null, float gravityOffset = 0f, float gravityRotation = 0f, SpawningOptions spawningOptions = SpawningOptions.RotateFirstCockpitTowardsDirection)
		{
			if (MyPrefabManager.Static == null)
			{
				MyLog.Default.WriteLine("Spawn Prefab failed. Prefab manager is not initialized.");
			}
			else
			{
				if (!direction.IsValid() || !position.IsValid())
				{
					return;
				}
				Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
				if (vector == Vector3.Zero)
				{
					vector = MyGravityProviderSystem.CalculateArtificialGravityInPoint(position);
				}
				Vector3D vector3D;
				if (vector != Vector3.Zero)
				{
					vector.Normalize();
					vector3D = -vector;
					position += vector * gravityOffset;
					if (direction == Vector3D.Zero)
					{
						direction = Vector3D.CalculatePerpendicularVector(vector);
						if (gravityRotation != 0f)
						{
							MatrixD matrix = MatrixD.CreateFromAxisAngle(vector3D, gravityRotation);
							direction = Vector3D.Transform(direction, matrix);
						}
					}
				}
				else if (direction == Vector3D.Zero)
				{
					direction = Vector3D.Right;
					vector3D = Vector3D.Up;
				}
				else
				{
					vector3D = Vector3D.CalculatePerpendicularVector(-direction);
				}
				direction.Normalize();
				vector3D.Normalize();
				if (direction.IsValid() && vector3D.IsValid())
				{
					MyPrefabManager.Static.SpawnPrefab(prefabName, position, direction, vector3D, default(Vector3), default(Vector3), beaconName, entityName, spawningOptions, ownerId, updateSync: true);
				}
			}
		}

		[VisualScriptingMiscData("State Machines", "Starts the specified state machine.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StartStateMachine(string stateMachineName, long ownerId = 0L)
		{
			MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SMManager.Run(stateMachineName, ownerId);
		}

		[VisualScriptingMiscData("Toolbar", "Sets item to the specified slot for the player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetToolbarSlotToItem(int slot, MyDefinitionId itemId, long playerId = -1L)
		{
			if (!itemId.TypeId.IsNull)
			{
				MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<int, SerializableDefinitionId, long>>)((IMyEventOwner s) => SetToolbarSlotToItemSync), slot, (SerializableDefinitionId)itemId, playerId, default(EndpointId), (Vector3D?)null);
			}
		}

<<<<<<< HEAD
		[Event(null, 7830)]
=======
		[Event(null, 7825)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetToolbarSlotToItemSync(int slot, SerializableDefinitionId itemId, long playerId = -1L)
		{
			if ((playerId == -1 || MySession.Static.LocalPlayerId == playerId) && MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(itemId, out var definition))
			{
				MyToolbarItem item = MyToolbarItemFactory.CreateToolbarItem(MyToolbarItemFactory.ObjectBuilderFromDefinition(definition));
				if (MyToolbarComponent.CurrentToolbar.SelectedSlot.HasValue && MyToolbarComponent.CurrentToolbar.SelectedSlot == slot)
				{
					MyToolbarComponent.CurrentToolbar.Unselect(unselectSound: false);
				}
				MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, item);
			}
		}

		public static void SetToolbarSlotToItemLocal(int slot, SerializableDefinitionId itemId, long playerId = -1L)
		{
			SetToolbarSlotToItemSync(slot, itemId, playerId);
		}

		[VisualScriptingMiscData("Toolbar", "Switches the specified toolbar slot for the player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SwitchToolbarToSlot(int slot, long playerId = -1L)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SwitchToolbarToSlotSync, slot, playerId);
		}

<<<<<<< HEAD
		[Event(null, 7859)]
=======
		[Event(null, 7854)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SwitchToolbarToSlotSync(int slot, long playerId = -1L)
		{
			if ((playerId == -1 || MySession.Static.LocalPlayerId == playerId) && slot >= 0 && slot < MyToolbarComponent.CurrentToolbar.SlotCount)
			{
				if (MyToolbarComponent.CurrentToolbar.SelectedSlot.HasValue && MyToolbarComponent.CurrentToolbar.SelectedSlot.Value == slot)
				{
					MyToolbarComponent.CurrentToolbar.Unselect(unselectSound: false);
				}
				MyToolbarComponent.CurrentToolbar.ActivateItemAtSlot(slot);
			}
		}

		public static void SwitchToolbarToSlotLocal(int slot, long playerId = -1L)
		{
			SwitchToolbarToSlotSync(slot, playerId);
		}

		[VisualScriptingMiscData("Toolbar", "Clears the toolbar slot for the player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ClearToolbarSlot(int slot, long playerId = -1L)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ClearToolbarSlotSync, slot, playerId);
		}

<<<<<<< HEAD
		[Event(null, 7885)]
=======
		[Event(null, 7880)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ClearToolbarSlotSync(int slot, long playerId = -1L)
		{
			if (slot >= 0 && slot < MyToolbarComponent.CurrentToolbar.SlotCount && (playerId == -1 || MySession.Static.LocalPlayerId == playerId))
			{
				MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, null);
			}
		}

		public static void ClearToolbarSlotLocal(int slot, long playerId = -1L)
		{
			ClearToolbarSlotSync(slot, playerId);
		}

		[VisualScriptingMiscData("Toolbar", "Clears all toolbar slots for the specified player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ClearAllToolbarSlots(long playerId = -1L)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ClearAllToolbarSlotsSync, playerId);
		}

<<<<<<< HEAD
		[Event(null, 7908)]
=======
		[Event(null, 7903)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ClearAllToolbarSlotsSync(long playerId = -1L)
		{
			if (playerId != -1 && MySession.Static.LocalPlayerId != playerId)
			{
				return;
			}
			int currentPage = MyToolbarComponent.CurrentToolbar.CurrentPage;
			for (int i = 0; i < MyToolbarComponent.CurrentToolbar.PageCount; i++)
			{
				MyToolbarComponent.CurrentToolbar.SwitchToPage(i);
				for (int j = 0; j < MyToolbarComponent.CurrentToolbar.SlotCount; j++)
				{
					MyToolbarComponent.CurrentToolbar.SetItemAtSlot(j, null);
				}
			}
			MyToolbarComponent.CurrentToolbar.SwitchToPage(currentPage);
		}

		public static void ClearAllToolbarSlotsLocal(long playerId = -1L)
		{
			ClearAllToolbarSlotsSync(playerId);
		}

		[VisualScriptingMiscData("Toolbar", "Sets the specified page for the toolbar.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetToolbarPage(int page, long playerId = -1L)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetToolbarPageSync, page, playerId);
		}

<<<<<<< HEAD
		[Event(null, 7938)]
=======
		[Event(null, 7933)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetToolbarPageSync(int page, long playerId = -1L)
		{
			if ((playerId == -1 || MySession.Static.LocalPlayerId == playerId) && page >= 0 && page < MyToolbarComponent.CurrentToolbar.PageCount)
			{
				MyToolbarComponent.CurrentToolbar.SwitchToPage(page);
			}
		}

		public static void SetToolbarPageLocal(int page, long playerId = -1L)
		{
			SetToolbarPageSync(page, playerId);
		}

		[VisualScriptingMiscData("Toolbar", "Reloads default settings for the toolbar", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ReloadToolbarDefaults(long playerId = -1L)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ReloadToolbarDefaultsSync, playerId);
		}

<<<<<<< HEAD
		[Event(null, 7961)]
=======
		[Event(null, 7956)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ReloadToolbarDefaultsSync(long playerId = -1L)
		{
			if (playerId == -1 || MySession.Static.LocalPlayerId == playerId)
			{
				MyToolbarComponent.CurrentToolbar.SetDefaults();
			}
		}

		public static void ReloadToolbarDefaultsLocal(long playerId = -1L)
		{
			ReloadToolbarDefaultsSync(playerId);
		}

		[VisualScriptingMiscData("Triggers", "Creates area trigger at the position of specified entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateAreaTriggerOnEntity(string entityName, float radius, string name)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(name);
			myAreaTriggerComponent.Radius = radius;
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (!entity.Components.Contains(typeof(MyTriggerAggregate)))
				{
					entity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
				}
				entity.Components.Get<MyTriggerAggregate>().AddComponent(myAreaTriggerComponent);
				myAreaTriggerComponent.Center = entity.PositionComp.GetPosition();
			}
		}

		[VisualScriptingMiscData("Triggers", "Creates area trigger at the relative position to the specified entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateAreaTriggerRelativeToEntity(Vector3D position, string entityName, float radius, string name)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(name);
			myAreaTriggerComponent.Radius = radius;
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (!entity.Components.Contains(typeof(MyTriggerAggregate)))
				{
					entity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
				}
				entity.Components.Get<MyTriggerAggregate>().AddComponent(myAreaTriggerComponent);
				myAreaTriggerComponent.Center = position;
			}
		}

		[VisualScriptingMiscData("Triggers", "Creates area trigger at the position.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static long CreateAreaTriggerOnPosition(Vector3D position, float radius, string name)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(name);
			MyEntity myEntity = new MyEntity();
			myEntity.PositionComp.SetPosition(position);
			myEntity.EntityId = MyEntityIdentifier.AllocateId();
			MyEntities.Add(myEntity);
			if (!myEntity.Components.Contains(typeof(MyTriggerAggregate)))
			{
				myEntity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
			}
			myEntity.Components.Get<MyTriggerAggregate>().AddComponent(myAreaTriggerComponent);
			myAreaTriggerComponent.Center = position;
			myAreaTriggerComponent.Radius = radius;
			return myEntity.EntityId;
		}

		[VisualScriptingMiscData("Triggers", "Removes all area triggers from the specified entity.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveAllTriggersFromEntity(string entityName)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				entity.Components.Remove(typeof(MyTriggerAggregate));
			}
		}

		[VisualScriptingMiscData("Triggers", "Remove area trigger with the specified name.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveTrigger(string triggerName)
		{
			if (MySessionComponentTriggerSystem.Static == null)
			{
				return;
			}
			MyTriggerComponent foundTrigger;
			MyEntity triggersEntity = MySessionComponentTriggerSystem.Static.GetTriggersEntity(triggerName, out foundTrigger);
			if (triggersEntity != null && foundTrigger != null)
			{
				if (triggersEntity.Components.TryGet<MyTriggerAggregate>(out var component))
				{
					component.RemoveComponent(foundTrigger);
				}
				else
				{
					triggersEntity.Components.Remove(typeof(MyAreaTriggerComponent), foundTrigger as MyAreaTriggerComponent);
				}
			}
		}

		[VisualScriptingMiscData("Achievements", "Award player achievement. Id ID is -1, unlock to all, if ID is 0, unlock to local player, if anything else, it unlocks to player with that ID", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void UnlockAchievementById(int achievementId, long playerId)
		{
			switch (playerId)
			{
			case 0L:
				UnlockAchievementInternal(achievementId);
				return;
			case -1L:
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => UnlockAchievementInternalAll, achievementId);
				return;
			}
			if (MySession.Static.Players.TryGetPlayerId(playerId, out var result))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => UnlockAchievementInternal, achievementId, new EndpointId(result.SteamId));
			}
		}

<<<<<<< HEAD
		[Event(null, 8095)]
=======
		[Event(null, 8090)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		private static void UnlockAchievementInternal(int achievementId)
		{
			UnlockAchievement(achievementId);
		}

<<<<<<< HEAD
		[Event(null, 8101)]
=======
		[Event(null, 8096)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void UnlockAchievementInternalAll(int achievementId)
		{
			UnlockAchievement(achievementId);
		}

		private static void UnlockAchievement(int achievementId)
		{
			if (achievementId >= 0 && achievementId < AllowedAchievementsHelper.AllowedAchievements.Count && !Sync.IsDedicated)
			{
				MyGameService.GetAchievement(AllowedAchievementsHelper.AllowedAchievements[achievementId], null, 0f).Unlock();
			}
		}

		[VisualScriptingMiscData("Definitions", "Returns true if the type id and subtype id match.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool DefinitionIdMatch(string typeId, string subtypeId, string matchTypeId, string matchSubtypeId)
		{
			if (string.Equals(typeId, matchTypeId))
			{
				return string.Equals(subtypeId, matchSubtypeId);
			}
			return false;
		}

		[VisualScriptingMiscData("Store", "Cancels listed item in specified store.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool CancelStoreItem(string storeEntityName, long id)
		{
			return (GetEntityByName(storeEntityName) as Sandbox.ModAPI.IMyStoreBlock)?.CancelStoreItem(id) ?? false;
		}

		[VisualScriptingMiscData("Store", "Inserts offer to specified store.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static MyStoreInsertResults InsertOffer(string storeEntityName, MyStoreItemData item, out long id)
		{
			Sandbox.ModAPI.IMyStoreBlock myStoreBlock = GetEntityByName(storeEntityName) as Sandbox.ModAPI.IMyStoreBlock;
			if (myStoreBlock != null)
			{
				return myStoreBlock.InsertOffer(item, out id);
			}
			id = 0L;
			return MyStoreInsertResults.Error;
		}

		[VisualScriptingMiscData("Store", "Inserts order to specified store.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static MyStoreInsertResults InsertOrder(string storeEntityName, MyStoreItemData item, out long id)
		{
			Sandbox.ModAPI.IMyStoreBlock myStoreBlock = GetEntityByName(storeEntityName) as Sandbox.ModAPI.IMyStoreBlock;
			if (myStoreBlock != null)
			{
				return myStoreBlock.InsertOrder(item, out id);
			}
			id = 0L;
			return MyStoreInsertResults.Error;
		}

		[VisualScriptingMiscData("Contract", "Create and add new Hauling contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. End block is contract block where package is to be delivered.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AddHaulingContract(long startBlockId, int moneyReward, int collateral, int duration, long endBlockId, out long id)
		{
			id = 0L;
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContractHauling contract = new MyContractHauling(startBlockId, moneyReward, collateral, duration, endBlockId);
			MyAddContractResultWrapper myAddContractResultWrapper = component.AddContract(contract);
			if (myAddContractResultWrapper.Success)
			{
				id = myAddContractResultWrapper.ContractId;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Contract", "Create and add new Acquisition contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. End block is contract block where items of type 'itemType' in quantity 'itemAmount' are to be delivered.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AddAcquisitionContract(long startBlockId, int moneyReward, int collateral, int duration, long endBlockId, MyDefinitionId itemType, int itemAmount, out long id)
		{
			id = 0L;
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContractAcquisition contract = new MyContractAcquisition(startBlockId, moneyReward, collateral, duration, endBlockId, itemType, itemAmount);
			MyAddContractResultWrapper myAddContractResultWrapper = component.AddContract(contract);
			if (myAddContractResultWrapper.Success)
			{
				id = myAddContractResultWrapper.ContractId;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Contract", "Create and add new Escort contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. Escort ship will start from 'start' flying towards the 'end'. Escorted ship will be owned by 'ownerIdentityId'", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AddEscortContract(long startBlockId, int moneyReward, int collateral, int duration, Vector3D start, Vector3D end, long ownerIdentityId, out long id)
		{
			id = 0L;
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContractEscort contract = new MyContractEscort(startBlockId, moneyReward, collateral, duration, start, end, ownerIdentityId);
			MyAddContractResultWrapper myAddContractResultWrapper = component.AddContract(contract);
			if (myAddContractResultWrapper.Success)
			{
				id = myAddContractResultWrapper.ContractId;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Contract", "Create and add new Search contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id. 'targetGridId' is id of grid that will be searched and 'searchRadius' is radius of sphere around searched grid where GPS will be randomly placed in", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AddSearchContract(long startBlockId, int moneyReward, int collateral, int duration, long targetGridId, double searchRadius, out long id)
		{
			id = 0L;
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContractSearch contract = new MyContractSearch(startBlockId, moneyReward, collateral, duration, targetGridId, searchRadius);
			MyAddContractResultWrapper myAddContractResultWrapper = component.AddContract(contract);
			if (myAddContractResultWrapper.Success)
			{
				id = myAddContractResultWrapper.ContractId;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Contract", "Create and add new Bounty contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AddBountyContract(long startBlockId, int moneyReward, int collateral, int duration, long targetIdentityId, out long id)
		{
			id = 0L;
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContractBounty contract = new MyContractBounty(startBlockId, moneyReward, collateral, duration, targetIdentityId);
			MyAddContractResultWrapper myAddContractResultWrapper = component.AddContract(contract);
			if (myAddContractResultWrapper.Success)
			{
				id = myAddContractResultWrapper.ContractId;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Contract", "Create and add new Repair contract. Returns true if contract creation was successful. Id of newly created contract is stored in out variable id.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool AddRepairContract(long startBlockId, int moneyReward, int collateral, int duration, long targetGridId, out long id)
		{
			id = 0L;
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContractRepair contract = new MyContractRepair(startBlockId, moneyReward, collateral, duration, targetGridId);
			MyAddContractResultWrapper myAddContractResultWrapper = component.AddContract(contract);
			if (myAddContractResultWrapper.Success)
			{
				id = myAddContractResultWrapper.ContractId;
				return true;
			}
			return false;
		}

		[VisualScriptingMiscData("Contract", "Remove inactive contract. Does not work if contract has already been accepted.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool RemoveContract(long id)
		{
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (!component.IsContractInInactive(id))
			{
				return false;
			}
			return component.RemoveContract(id);
		}

		[VisualScriptingMiscData("Entity Storage", "Stores string in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityString(string entityName, string key, string value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores boolean in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityBool(string entityName, string key, bool value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores integer in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityInteger(string entityName, string key, int value)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores long integer in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityLong(string entityName, string key, long value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores float in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityFloat(string entityName, string key, float value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores Vector3D (doubles) in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityVector(string entityName, string key, Vector3D value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Loads string from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string LoadEntityString(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return "";
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return "";
				}
				return entity.EntityStorage.ReadString(key);
			}
			return "";
		}

		[VisualScriptingMiscData("Entity Storage", "Loads boolean from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool LoadEntityBool(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return false;
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return false;
				}
				return entity.EntityStorage.ReadBool(key);
			}
			return false;
		}

		[VisualScriptingMiscData("Entity Storage", "Loads integer from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int LoadEntityInteger(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return 0;
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return 0;
				}
				return entity.EntityStorage.ReadInt(key);
			}
			return 0;
		}

		[VisualScriptingMiscData("Entity Storage", "Loads long integer from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static long LoadEntityLong(string entityName, string key)
		{
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return 0L;
				}
				return entity.EntityStorage.ReadLong(key);
			}
			return 0L;
		}

		[VisualScriptingMiscData("Entity Storage", "Loads float from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float LoadEntityFloat(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return 0f;
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return 0f;
				}
				return entity.EntityStorage.ReadFloat(key);
			}
			return 0f;
		}

		[VisualScriptingMiscData("Entity Storage", "Loads Vector3D (doubles) from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D LoadEntityVector(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return Vector3D.Zero;
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return Vector3D.Zero;
				}
				return entity.EntityStorage.ReadVector3D(key);
			}
			return Vector3D.Zero;
		}

		[VisualScriptingMiscData("Entity Storage", "Loads string list from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<string> LoadEntityStringList(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return new List<string>();
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					return new List<string>();
				}
				return entity.EntityStorage.ReadStringList(key);
			}
			return new List<string>();
		}

		[VisualScriptingMiscData("Entity Storage", "Loads boolean list from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<bool> LoadEntityBoolList(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return new List<bool>();
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
					entity.EntityStorage.Write(key, new List<bool>());
				}
				return entity.EntityStorage.ReadBoolList(key);
			}
			return new List<bool>();
		}

		[VisualScriptingMiscData("Entity Storage", "Loads integer list from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<int> LoadEntityIntegerList(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return new List<int>();
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
					entity.EntityStorage.Write(key, new List<int>());
				}
				return entity.EntityStorage.ReadIntList(key);
			}
			return new List<int>();
		}

		[VisualScriptingMiscData("Entity Storage", "Loads long list from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<long> LoadEntityLongList(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return new List<long>();
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
					entity.EntityStorage.Write(key, new List<long>());
				}
				return entity.EntityStorage.ReadLongList(key);
			}
			return new List<long>();
		}

		[VisualScriptingMiscData("Entity Storage", "Loads float from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<float> LoadEntityFloatList(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return new List<float>();
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
					entity.EntityStorage.Write(key, new List<float>());
				}
				return entity.EntityStorage.ReadFloatList(key);
			}
			return new List<float>();
		}

		[VisualScriptingMiscData("Entity Storage", "Loads Vector3D (doubles) from the entity storage.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static List<Vector3D> LoadEntityVectorList(string entityName, string key)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return new List<Vector3D>();
			}
			if (MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
					entity.EntityStorage.Write(key, new List<Vector3D>());
				}
				return entity.EntityStorage.ReadVector3DList(key);
			}
			return new List<Vector3D>();
		}

		[VisualScriptingMiscData("Entity Storage", "Stores list of string in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityStringList(string entityName, string key, List<string> value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores boolean list in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityBoolList(string entityName, string key, List<bool> value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores integer list in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityIntegerList(string entityName, string key, List<int> value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores long list in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityLongList(string entityName, string key, List<long> value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores float list in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityFloatList(string entityName, string key, List<float> value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Entity Storage", "Stores Vector3D list in the entity storage. This value is accessible from all scripts.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StoreEntityVectorList(string entityName, string key, List<Vector3D> value)
		{
			if (!string.IsNullOrEmpty(entityName) && MyEntities.TryGetEntityByName(entityName, out var entity))
			{
				if (entity.EntityStorage == null)
				{
					entity.EntityStorage = new MyEntityStorageComponent();
				}
				entity.EntityStorage.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Timers", "Start a timer with a specified key.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StartGameplayTimer(string key, bool reset)
		{
			if (!m_timers.ContainsKey(key) || reset)
			{
				m_timers[key] = (MySandboxGame.TotalGamePlayTimeInMilliseconds, true);
			}
			else if (!m_timers[key].Running)
			{
				m_timers[key] = (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_timers[key].Time, true);
			}
		}

		[VisualScriptingMiscData("Timers", "Stop timer running.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void StopGameplayTimer(string key, bool reset = false)
		{
			if (!reset)
			{
				if (m_timers.ContainsKey(key) && m_timers[key].Running)
				{
					m_timers[key] = (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_timers[key].Time, false);
				}
			}
			else
			{
				m_timers[key] = (0, false);
			}
		}

		[VisualScriptingMiscData("Timers", "Return elapsed time in milliseconds for given timer.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int GetGameplayElapsedTime(string key)
		{
			if (m_timers.ContainsKey(key))
			{
				if (!m_timers[key].Running)
				{
					return m_timers[key].Time;
				}
				return MySandboxGame.TotalGamePlayTimeInMilliseconds - m_timers[key].Time;
			}
			return 0;
		}

		[VisualScriptingMiscData("Timers", "Return elapsed time in milliseconds for given timer. Format according to https://docs.microsoft.com/cs-cz/dotnet/api/system.timespan.tostring", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string GetGameplayElapsedTimeToString(string key, string format)
		{
			return TimeSpan.FromMilliseconds(GetGameplayElapsedTime(key)).ToString(format, CultureInfo.InvariantCulture);
		}

		[VisualScriptingMiscData("UIString", "Create UI string on specific coordinates. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateUIString(long id, string text, float normalizedXPos, float normalizedYPos, float scale = 1f, string font = "White", MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyUIString myUIString = default(MyUIString);
			myUIString.Text = text;
			myUIString.NormalizedCoord = new Vector2(normalizedXPos, normalizedYPos);
			myUIString.Scale = scale;
			myUIString.Font = font;
			myUIString.DrawAlign = drawAlign;
			MyUIString arg2 = myUIString;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateUIStringSync, id, arg2, arg);
		}

<<<<<<< HEAD
		[Event(null, 8897)]
=======
		[Event(null, 8892)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void CreateUIStringSync(long id, MyUIString UIString, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.CreateUIString(id, UIString);
			}
		}

		[VisualScriptingMiscData("UIString", "Remove UI string with specific id. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveUIString(long id, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveUIStringSync, id, arg);
		}

<<<<<<< HEAD
		[Event(null, 8918)]
=======
		[Event(null, 8913)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveUIStringSync(long id, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveUIString(id);
			}
		}

		[VisualScriptingMiscData("Board screen", "Create board screen on specific coordinates. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void CreateBoardScreen(string boardId, float normalizedPosX, float normalizedPosY, float normalizedSizeX, float normalizedSizeY, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateBoardScreenSync, boardId, normalizedPosX, normalizedPosY, normalizedSizeX, normalizedSizeY, arg);
		}

<<<<<<< HEAD
		[Event(null, 8943)]
=======
		[Event(null, 8938)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void CreateBoardScreenSync(string boardId, float normalizedPosX, float normalizedPosY, float normalizedSizeX, float normalizedSizeY, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.CreateBoardScreen(boardId, normalizedPosX, normalizedPosY, normalizedSizeX, normalizedSizeY);
			}
		}

		[VisualScriptingMiscData("Board screen", "Remove board screen with specific id. PlayerId -1 stands for all players, 0 for local player.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveBoardScreen(string boardId, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveBoardScreenSync, boardId, arg);
		}

<<<<<<< HEAD
		[Event(null, 8965)]
=======
		[Event(null, 8960)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveBoardScreenSync(string boardId, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveBoardScreen(boardId);
			}
		}

		[VisualScriptingMiscData("Board screen", "Add column to board screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddColumn(string boardId, string columnId, float width, string headerText, MyGuiDrawAlignEnum headerDrawAlign, MyGuiDrawAlignEnum columnDrawAlign, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyGuiScreenBoard.MyColumn myColumn = default(MyGuiScreenBoard.MyColumn);
			myColumn.ColumnDrawAlign = columnDrawAlign;
			myColumn.HeaderDrawAlign = headerDrawAlign;
			myColumn.HeaderText = headerText;
			myColumn.Width = width;
			myColumn.Visible = true;
			MyGuiScreenBoard.MyColumn arg2 = myColumn;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AddColumnSync, boardId, columnId, arg2, arg);
		}

<<<<<<< HEAD
		[Event(null, 8996)]
=======
		[Event(null, 8991)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void AddColumnSync(string boardId, string columnId, MyGuiScreenBoard.MyColumn column, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.AddColumn(boardId, columnId, column);
			}
		}

		[VisualScriptingMiscData("Board screen", "Remove column from board screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveColumn(string boardId, string columnId, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveColumnSync, boardId, columnId, arg);
		}

<<<<<<< HEAD
		[Event(null, 9018)]
=======
		[Event(null, 9013)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveColumnSync(string boardId, string columnId, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveColumn(boardId, columnId);
			}
		}

		[VisualScriptingMiscData("Board screen", "Add row to board screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddRow(string boardId, string rowId, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AddRowSync, boardId, rowId, arg);
		}

<<<<<<< HEAD
		[Event(null, 9040)]
=======
		[Event(null, 9035)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void AddRowSync(string boardId, string rowId, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.AddRow(boardId, rowId);
			}
		}

		[VisualScriptingMiscData("Board screen", "Add row to board screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void RemoveRow(string boardId, string rowId, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveRowSync, boardId, rowId, arg);
		}

<<<<<<< HEAD
		[Event(null, 9062)]
=======
		[Event(null, 9057)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveRowSync(string boardId, string rowId, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.RemoveRow(boardId, rowId);
			}
		}

		[VisualScriptingMiscData("Board screen", "Set cell text in the board screen.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetCell(string boardId, string rowId, string columnId, string text, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetCellSync, boardId, rowId, columnId, text, arg);
		}

<<<<<<< HEAD
		[Event(null, 9084)]
=======
		[Event(null, 9079)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetCellSync(string boardId, string rowId, string columnId, string text, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SetCell(boardId, rowId, columnId, text);
			}
		}

		[VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetRowRanking(string boardId, string rowId, int ranking, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetRowRankingSync, boardId, rowId, ranking, arg);
		}

<<<<<<< HEAD
		[Event(null, 9106)]
=======
		[Event(null, 9101)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetRowRankingSync(string boardId, string rowId, int ranking, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SetRowRanking(boardId, rowId, ranking);
			}
		}

		[VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SortByColumn(string boardId, string columnId, bool ascending, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SortByColumnSync, boardId, columnId, ascending, arg);
		}

<<<<<<< HEAD
		[Event(null, 9128)]
=======
		[Event(null, 9123)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SortByColumnSync(string boardId, string columnId, bool ascending, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SortByColumn(boardId, columnId, ascending);
			}
		}

		[VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SortByRanking(string boardId, bool ascending, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SortByRankingSync, boardId, ascending, arg);
		}

<<<<<<< HEAD
		[Event(null, 9150)]
=======
		[Event(null, 9145)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SortByRankingSync(string boardId, bool ascending, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SortByRanking(boardId, ascending);
			}
		}

		[VisualScriptingMiscData("Board screen", "Set row ranking used for sorting.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void ShowOrderInColumn(string boardId, string columnId, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowOrderInColumnSync, boardId, columnId, arg);
		}

<<<<<<< HEAD
		[Event(null, 9172)]
=======
		[Event(null, 9167)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ShowOrderInColumnSync(string boardId, string columnId, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.ShowOrderInColumn(boardId, columnId);
			}
		}

		[VisualScriptingMiscData("Board screen", "Set column visible state.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetColumnVisibility(string boardId, string columnId, bool visible, long playerId = -1L)
		{
			long arg = ((playerId == 0L && MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.GetPlayerIdentityId() : playerId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetColumnVisibilitySync, boardId, columnId, visible, arg);
		}

<<<<<<< HEAD
		[Event(null, 9195)]
=======
		[Event(null, 9190)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void SetColumnVisibilitySync(string boardId, string columnId, bool visible, long playerId = -1L)
		{
			if (playerId == -1 || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalPlayerId == playerId)
			{
				MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SetColumnVisibility(boardId, columnId, visible);
			}
		}

		[VisualScriptingMember(false, false)]
		[VisualScriptingMiscData("Other", "Enum constants", -10510688)]
		public static JoinResult JoinResult(JoinResult value)
		{
			return value;
		}

		[VisualScriptingMiscData("Other", "Get name of the current state of the match.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static string GetMatchState()
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component == null)
			{
				return string.Empty;
			}
			return component.State.ToString();
		}

		[VisualScriptingMiscData("Other", "Immediately progress match into next phase. Returns name of new state.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static string AdvanceMatchState()
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component == null)
			{
				return string.Empty;
			}
			component.AdvanceToNextState();
			return component.State.ToString();
		}

		[VisualScriptingMiscData("Other", "Get remaining time of the current match state. Time is in minutes.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static float GetMatchStateRemainingDuration()
		{
			return MySession.Static.GetComponent<MySessionComponentMatch>()?.RemainingMinutes ?? 0f;
		}

		[VisualScriptingMiscData("Other", "Set remaining time of the current match state. Time is in minutes. If the value is lower or equal to zero, next match phase will start.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetMatchStateRemainingDuration(float time)
		{
			MySession.Static.GetComponent<MySessionComponentMatch>()?.SetRemainingTime(time);
		}

		[VisualScriptingMiscData("Other", "Get remaining time of the current match state. Time is in minutes. Positive value will lenghten the phase, negative will shorten it. If remaining duration becomes lower or equal to zero, next match phase will start", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void AddMatchStateRemainingDuration(float time)
		{
			MySession.Static.GetComponent<MySessionComponentMatch>()?.AddRemainingTime(time);
		}

		[VisualScriptingMiscData("Other", "Sets whether match component should be runing or not. When component is running, remaining time of the state will be ticking out and states will be changing. If component is not running, it will be effectively paused until running again.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetMatchIsRunningState(bool isRunning)
		{
			MySession.Static.GetComponent<MySessionComponentMatch>()?.SetIsRunning(isRunning);
		}

		[VisualScriptingMiscData("Other", "Get information whether component handling the match is running or not. Running component will have the time advancing and states will be changing.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static bool GetMatchIsRunningState()
		{
			return MySession.Static.GetComponent<MySessionComponentMatch>()?.IsRunning ?? false;
		}
	}
}
