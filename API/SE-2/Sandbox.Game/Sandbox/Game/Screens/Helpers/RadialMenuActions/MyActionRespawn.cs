using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionRespawn : MyActionBase
	{
		public override void ExecuteAction()
		{
			MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
			if (component != null && component.CustomRespawnEnabled)
			{
				if (MySession.Static.ControlledEntity != null)
				{
					MyVisualScriptLogicProvider.CustomRespawnRequest(MySession.Static.ControlledEntity.ControllerInfo.ControllingIdentityId);
				}
			}
			else
			{
				MySession.Static.ControlledEntity?.Die();
			}
		}

		public override bool IsEnabled()
		{
			if (!(MySession.Static.ControlledEntity is MyCharacter))
			{
				return false;
			}
			return true;
		}

		public override MyRadialLabelText GetLabel(string shortcut, string name)
		{
			MyRadialLabelText label = base.GetLabel(shortcut, name);
			if (!(MySession.Static.ControlledEntity is MyCharacter))
			{
				label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CharacterOnly);
			}
			return label;
		}
	}
}
