using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Campaign
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Campaign : MyObjectBuilder_Base
	{
		public struct MySupportedPlatform
		{
			public string Name;

			public string StateMachine;
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EStateMachine_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, MyObjectBuilder_CampaignSM>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in MyObjectBuilder_CampaignSM value)
			{
				owner.StateMachine = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out MyObjectBuilder_CampaignSM value)
			{
				value = owner.StateMachine;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003ESupportedPlatforms_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, MySupportedPlatform[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in MySupportedPlatform[] value)
			{
				owner.SupportedPlatforms = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out MySupportedPlatform[] value)
			{
				value = owner.SupportedPlatforms;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EStateMachines_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, MyObjectBuilder_CampaignSM[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in MyObjectBuilder_CampaignSM[] value)
			{
				owner.StateMachines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out MyObjectBuilder_CampaignSM[] value)
			{
				value = owner.StateMachines;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003ELocalizationPaths_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in List<string> value)
			{
				owner.LocalizationPaths = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out List<string> value)
			{
				value = owner.LocalizationPaths;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003ELocalizationLanguages_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in List<string> value)
			{
				owner.LocalizationLanguages = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out List<string> value)
			{
				value = owner.LocalizationLanguages;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EDefaultLocalizationLanguage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.DefaultLocalizationLanguage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.DefaultLocalizationLanguage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EDescriptionLocalizationFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.DescriptionLocalizationFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.DescriptionLocalizationFile;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003ECampaignPath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.CampaignPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.CampaignPath;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EForceDisableTrashRemoval_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in bool value)
			{
				owner.ForceDisableTrashRemoval = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out bool value)
			{
				value = owner.ForceDisableTrashRemoval;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.Description = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.Description;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EImagePath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.ImagePath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.ImagePath;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EIsMultiplayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in bool value)
			{
				owner.IsMultiplayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out bool value)
			{
				value = owner.IsMultiplayer;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EDifficulty_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.Difficulty = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.Difficulty;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EAuthor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.Author = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.Author;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EOrder_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in int value)
			{
				owner.Order = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out int value)
			{
				value = owner.Order;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EMaxPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in int value)
			{
				owner.MaxPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out int value)
			{
				value = owner.MaxPlayers;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EDLC_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.DLC = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.DLC;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EIsOfflineEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in bool value)
			{
				owner.IsOfflineEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out bool value)
			{
				value = owner.IsOfflineEnabled;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EIsVanilla_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in bool value)
			{
				owner.IsVanilla = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out bool value)
			{
				value = owner.IsVanilla;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EIsLocalMod_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in bool value)
			{
				owner.IsLocalMod = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out bool value)
			{
				value = owner.IsLocalMod;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EModFolderPath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.ModFolderPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.ModFolderPath;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EPublishedFileId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in ulong value)
			{
				owner.PublishedFileId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out ulong value)
			{
				value = owner.PublishedFileId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EPublishedServiceName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				owner.PublishedServiceName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				value = owner.PublishedServiceName;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EIsDebug_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in bool value)
			{
				owner.IsDebug = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out bool value)
			{
				value = owner.IsDebug;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Campaign owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Campaign owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_Campaign, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Campaign>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Campaign();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Campaign CreateInstance()
			{
				return new MyObjectBuilder_Campaign();
			}

			MyObjectBuilder_Campaign IActivator<MyObjectBuilder_Campaign>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyObjectBuilder_CampaignSM StateMachine;

		[XmlArrayItem("Platform")]
		public MySupportedPlatform[] SupportedPlatforms;

		[XmlArrayItem("StateMachine")]
		public MyObjectBuilder_CampaignSM[] StateMachines;

		[XmlArrayItem("Path")]
		public List<string> LocalizationPaths = new List<string>();

		[XmlArrayItem("Language")]
		public List<string> LocalizationLanguages = new List<string>();

		public string DefaultLocalizationLanguage;

		public string DescriptionLocalizationFile;

		public string CampaignPath;

		public string Name;

		public bool ForceDisableTrashRemoval;

		public string Description;

		public string ImagePath;

		public bool IsMultiplayer;

		public string Difficulty;

		public string Author;

		public int Order;

		public int MaxPlayers = 16;

		public string DLC;

		/// <summary>
		/// If offline mode is enabled for the scenario
		/// </summary>
		public bool IsOfflineEnabled = true;

		[XmlIgnore]
		public bool IsVanilla = true;

		[XmlIgnore]
		public bool IsLocalMod = true;

		[XmlIgnore]
		public string ModFolderPath;

		[XmlIgnore]
		public ulong PublishedFileId;

		[XmlIgnore]
		public string PublishedServiceName;

		[XmlIgnore]
		public bool IsDebug;

		public MyObjectBuilder_CampaignSM GetStateMachine(string platform = null, bool platformMandatory = false)
		{
			MyObjectBuilder_CampaignSM result = StateMachine;
			if (SupportedPlatforms != null && StateMachines != null)
			{
<<<<<<< HEAD
				MySupportedPlatform supportedPlatform = SupportedPlatforms.FirstOrDefault((MySupportedPlatform x) => x.Name == platform);
				MyObjectBuilder_CampaignSM myObjectBuilder_CampaignSM = StateMachines.FirstOrDefault((MyObjectBuilder_CampaignSM x) => x.Name == supportedPlatform.StateMachine);
=======
				MySupportedPlatform supportedPlatform = Enumerable.FirstOrDefault<MySupportedPlatform>((IEnumerable<MySupportedPlatform>)SupportedPlatforms, (Func<MySupportedPlatform, bool>)((MySupportedPlatform x) => x.Name == platform));
				MyObjectBuilder_CampaignSM myObjectBuilder_CampaignSM = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSM>((IEnumerable<MyObjectBuilder_CampaignSM>)StateMachines, (Func<MyObjectBuilder_CampaignSM, bool>)((MyObjectBuilder_CampaignSM x) => x.Name == supportedPlatform.StateMachine));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (myObjectBuilder_CampaignSM != null)
				{
					result = myObjectBuilder_CampaignSM;
				}
				else
				{
					if (platformMandatory)
					{
						return null;
					}
					if (StateMachines.Length != 0)
					{
						result = StateMachines[0];
					}
				}
			}
			return result;
		}
	}
}
