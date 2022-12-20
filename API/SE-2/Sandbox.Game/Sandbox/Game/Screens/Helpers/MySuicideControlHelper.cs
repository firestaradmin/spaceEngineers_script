using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MySuicideControlHelper : MyAbstractControlMenuItem
	{
		private MyCharacter m_character;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CommitSuicide);

		public MySuicideControlHelper()
			: base(MyControlsSpace.SUICIDE)
		{
		}

		public override void Activate()
		{
			MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
			if (component != null && component.CustomRespawnEnabled)
			{
				if (MySession.Static.ControlledEntity != null)
				{
					MyVisualScriptLogicProvider.CustomRespawnRequest(m_character.ControllerInfo.ControllingIdentityId);
				}
			}
			else
			{
				m_character.Die();
			}
		}

		public void SetCharacter(MyCharacter character)
		{
			m_character = character;
		}
	}
}
