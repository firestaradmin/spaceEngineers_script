<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using Sandbox.Definitions;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Utils;

namespace VRage.Audio
{
	public static class MyAudioExtensions
	{
		public static readonly MySoundErrorDelegate OnSoundError = delegate(MySoundData cue, string message)
		{
			MyAudioDefinition soundDefinition = MyDefinitionManager.Static.GetSoundDefinition(cue.SubtypeId);
			MyDefinitionErrors.Add((soundDefinition != null) ? soundDefinition.Context : MyModContext.UnknownContext, message, TErrorSeverity.Error);
		};

		public static MyCueId GetCueId(this IMyAudio self, string cueName)
		{
			if (self == null || !MyStringHash.TryGet(cueName, out var id))
			{
				id = MyStringHash.NullOrEmpty;
			}
			return new MyCueId(id);
		}

		internal static ListReader<MySoundData> GetSoundDataFromDefinitions()
		{
			return Enumerable.ToList<MySoundData>(Enumerable.Select<MyAudioDefinition, MySoundData>(Enumerable.Where<MyAudioDefinition>((IEnumerable<MyAudioDefinition>)MyDefinitionManager.Static.GetSoundDefinitions(), (Func<MyAudioDefinition, bool>)((MyAudioDefinition x) => x.Enabled)), (Func<MyAudioDefinition, MySoundData>)((MyAudioDefinition x) => x.SoundData)));
		}

		internal static ListReader<MyAudioEffect> GetEffectData()
		{
			return Enumerable.ToList<MyAudioEffect>(Enumerable.Select<MyAudioEffectDefinition, MyAudioEffect>((IEnumerable<MyAudioEffectDefinition>)MyDefinitionManager.Static.GetAudioEffectDefinitions(), (Func<MyAudioEffectDefinition, MyAudioEffect>)((MyAudioEffectDefinition x) => x.Effect)));
		}
	}
}
