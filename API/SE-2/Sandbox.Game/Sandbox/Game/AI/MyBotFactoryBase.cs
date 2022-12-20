using System;
using System.Collections.Generic;
using System.Reflection;
using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.AI;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRageMath;

namespace Sandbox.Game.AI
{
	public abstract class MyBotFactoryBase
	{
		protected class BehaviorData
		{
			public readonly Type BotActionsType;

			public Type LogicType;

			public BehaviorData(Type t)
			{
				BotActionsType = t;
			}
		}

		protected class LogicData
		{
			public readonly Type LogicType;

			public LogicData(Type t)
			{
				LogicType = t;
			}
		}

		protected class BehaviorTypeData
		{
			public Type BotType;

			public BehaviorTypeData(Type botType)
			{
				BotType = botType;
			}
		}

		protected Dictionary<string, Type> m_TargetTypeByName;

		protected Dictionary<string, BehaviorData> m_botDataByBehaviorType;

		protected Dictionary<string, LogicData> m_logicDataByBehaviorSubtype;

		private readonly Type[] m_tmpTypeArray;
<<<<<<< HEAD

		private readonly object[] m_tmpConstructorParamArray;

		private static readonly MyObjectFactory<MyBotTypeAttribute, IMyBot> m_objectFactory;
=======

		private readonly object[] m_tmpConstructorParamArray;

		private static readonly MyObjectFactory<MyBotTypeAttribute, IMyBot> m_objectFactory;

		public abstract int MaximumUncontrolledBotCount { get; }

		public abstract int MaximumBotPerPlayer { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		static MyBotFactoryBase()
		{
			m_objectFactory = new MyObjectFactory<MyBotTypeAttribute, IMyBot>();
			Assembly assembly = Assembly.GetAssembly(typeof(MyAgentBot));
			m_objectFactory.RegisterFromAssembly(assembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			foreach (IPlugin plugin in MyPlugins.Plugins)
			{
				m_objectFactory.RegisterFromAssembly(plugin.GetType().Assembly);
			}
		}

		protected MyBotFactoryBase()
		{
			m_TargetTypeByName = new Dictionary<string, Type>();
			m_botDataByBehaviorType = new Dictionary<string, BehaviorData>();
			m_logicDataByBehaviorSubtype = new Dictionary<string, LogicData>();
			m_tmpTypeArray = new Type[1];
			m_tmpConstructorParamArray = new object[1];
			Assembly assembly = Assembly.GetAssembly(typeof(MyAgentBot));
			LoadBotData(assembly);
			LoadBotData(MyPlugins.GameAssembly);
			foreach (IPlugin plugin in MyPlugins.Plugins)
			{
				LoadBotData(plugin.GetType().Assembly);
			}
		}

		protected void LoadBotData(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!type.IsAbstract && type.IsSubclassOf(typeof(MyBotActionsBase)))
				{
					object[] customAttributes = type.GetCustomAttributes(inherit: true);
					string text = "";
					BehaviorData behaviorData = new BehaviorData(type);
					object[] array = customAttributes;
					foreach (object obj in array)
					{
						MyBehaviorDescriptorAttribute myBehaviorDescriptorAttribute;
						BehaviorActionImplAttribute behaviorActionImplAttribute;
						if ((myBehaviorDescriptorAttribute = obj as MyBehaviorDescriptorAttribute) != null)
						{
							text = myBehaviorDescriptorAttribute.DescriptorCategory;
						}
						else if ((behaviorActionImplAttribute = obj as BehaviorActionImplAttribute) != null)
						{
							behaviorData.LogicType = behaviorActionImplAttribute.LogicType;
						}
					}
					if (!string.IsNullOrEmpty(text) && behaviorData.LogicType != null)
					{
						m_botDataByBehaviorType[text] = behaviorData;
					}
				}
				else if (!type.IsAbstract && type.IsSubclassOf(typeof(MyBotLogic)))
				{
					object[] array = type.GetCustomAttributes(typeof(BehaviorLogicAttribute), inherit: true);
					for (int j = 0; j < array.Length; j++)
					{
						BehaviorLogicAttribute behaviorLogicAttribute = array[j] as BehaviorLogicAttribute;
						m_logicDataByBehaviorSubtype[behaviorLogicAttribute.BehaviorSubtype] = new LogicData(type);
					}
				}
				else if (!type.IsAbstract && typeof(MyAiTargetBase).IsAssignableFrom(type))
				{
					object[] array = type.GetCustomAttributes(typeof(TargetTypeAttribute), inherit: true);
					for (int j = 0; j < array.Length; j++)
					{
						TargetTypeAttribute targetTypeAttribute = array[j] as TargetTypeAttribute;
						m_TargetTypeByName[targetTypeAttribute.TargetType] = type;
					}
				}
			}
		}

		public MyObjectBuilder_Bot GetBotObjectBuilder(IMyBot myAgentBot)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Bot>(myAgentBot);
		}

		public IMyBot CreateBot(MyPlayer player, MyObjectBuilder_Bot botBuilder, MyBotDefinition botDefinition)
		{
			MyObjectBuilderType typeId;
			if (botBuilder == null)
			{
				typeId = botDefinition.Id.TypeId;
				botBuilder = m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Bot>(m_objectFactory.GetProducedType(typeId));
			}
			else
			{
				typeId = botBuilder.TypeId;
			}
			if (!m_botDataByBehaviorType.ContainsKey(botDefinition.BehaviorType))
			{
				return null;
			}
			BehaviorData behaviorData = m_botDataByBehaviorType[botDefinition.BehaviorType];
			IMyBot myBot = CreateBot(m_objectFactory.GetProducedType(typeId), player, botDefinition);
			CreateActions(myBot, behaviorData.BotActionsType);
			CreateLogic(myBot, behaviorData.LogicType, botDefinition.BehaviorSubtype);
			myBot.Init(botBuilder);
			return myBot;
		}

		private void CreateLogic(IMyBot output, Type defaultLogicType, string definitionLogicType)
		{
			Type type;
			if (m_logicDataByBehaviorSubtype.ContainsKey(definitionLogicType))
			{
				type = m_logicDataByBehaviorSubtype[definitionLogicType].LogicType;
				if (!type.IsSubclassOf(defaultLogicType) && type != defaultLogicType)
				{
					type = defaultLogicType;
				}
			}
			else
			{
				type = defaultLogicType;
			}
			MyBotLogic logic = Activator.CreateInstance(type, output) as MyBotLogic;
			output.InitLogic(logic);
		}

		private void CreateActions(IMyBot bot, Type actionImplType)
		{
			m_tmpTypeArray[0] = bot.GetType();
			if (actionImplType.GetConstructor(m_tmpTypeArray) == null)
			{
				bot.BotActions = Activator.CreateInstance(actionImplType) as MyBotActionsBase;
			}
			else
			{
				bot.BotActions = Activator.CreateInstance(actionImplType, bot) as MyBotActionsBase;
			}
			m_tmpTypeArray[0] = null;
		}

		public MyAiTargetBase CreateTargetForBot(MyAgentBot bot)
		{
			MyAiTargetBase result = null;
			m_tmpConstructorParamArray[0] = bot;
			m_TargetTypeByName.TryGetValue(bot.AgentDefinition.TargetType, out var value);
			if (value != null)
			{
				result = Activator.CreateInstance(value, m_tmpConstructorParamArray) as MyAiTargetBase;
			}
			m_tmpConstructorParamArray[0] = null;
			return result;
		}

		private static IMyBot CreateBot(Type botType, MyPlayer player, MyBotDefinition botDefinition)
		{
			return Activator.CreateInstance(botType, player, botDefinition) as IMyBot;
		}

		public abstract bool CanCreateBotOfType(string behaviorType, bool load);

		public abstract bool GetBotSpawnPosition(string behaviorType, out Vector3D spawnPosition);

		public abstract bool GetBotGroupSpawnPositions(string behaviorType, int count, List<Vector3D> spawnPositions);
	}
}
