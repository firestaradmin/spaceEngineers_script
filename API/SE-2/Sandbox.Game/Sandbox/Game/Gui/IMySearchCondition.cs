using System.Collections.Generic;
using Sandbox.Definitions;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public interface IMySearchCondition
	{
		bool MatchesCondition(string itemId);

		bool MatchesCondition(MyDefinitionBase itemId);

		void AddDefinitionGroup(MyCubeBlockDefinitionGroup definitionGruop);

		HashSet<MyCubeBlockDefinitionGroup> GetSortedBlocks();

		void CleanDefinitionGroups();
	}
}
