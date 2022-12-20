using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GuiBlockCategoryDefinition), null)]
	public class MyGuiBlockCategoryDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyGuiBlockCategoryDefinition_003C_003EActor : IActivator, IActivator<MyGuiBlockCategoryDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGuiBlockCategoryDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGuiBlockCategoryDefinition CreateInstance()
			{
				return new MyGuiBlockCategoryDefinition();
			}

			MyGuiBlockCategoryDefinition IActivator<MyGuiBlockCategoryDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Name;

		public HashSet<string> ItemIds;

		public bool IsShipCategory;

		public bool IsBlockCategory = true;

		public bool SearchBlocks = true;

		public bool ShowAnimations;

		public bool ShowInCreative = true;

		public bool IsAnimationCategory;

		public bool IsToolCategory;

		public int ValidItems;

		public bool StrictSearch;

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_GuiBlockCategoryDefinition myObjectBuilder_GuiBlockCategoryDefinition = (MyObjectBuilder_GuiBlockCategoryDefinition)ob;
			Name = myObjectBuilder_GuiBlockCategoryDefinition.Name;
			ItemIds = new HashSet<string>((IEnumerable<string>)Enumerable.ToList<string>((IEnumerable<string>)myObjectBuilder_GuiBlockCategoryDefinition.ItemIds));
			IsBlockCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsBlockCategory;
			IsShipCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsShipCategory;
			SearchBlocks = myObjectBuilder_GuiBlockCategoryDefinition.SearchBlocks;
			ShowAnimations = myObjectBuilder_GuiBlockCategoryDefinition.ShowAnimations;
			ShowInCreative = myObjectBuilder_GuiBlockCategoryDefinition.ShowInCreative;
			Public = myObjectBuilder_GuiBlockCategoryDefinition.Public;
			IsAnimationCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsAnimationCategory;
			IsToolCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsToolCategory;
			StrictSearch = myObjectBuilder_GuiBlockCategoryDefinition.StrictSearch;
		}

		public bool HasItem(string itemId)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string> enumerator = ItemIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					if (itemId.EndsWith(current))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}
	}
}
