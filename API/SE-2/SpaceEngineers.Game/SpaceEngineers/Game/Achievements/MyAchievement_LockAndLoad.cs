using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Game;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_LockAndLoad : MySteamAchievementBase
	{
		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_LockAndLoad", null, 0f);
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved)
			{
				MyCharacter.OnCharacterDied += MyCharacter_OnCharacterDied;
			}
		}

		private void MyCharacter_OnCharacterDied(MyCharacter character)
		{
			MyEntities.TryGetEntityById(character.StatComp.LastDamage.AttackerId, out var entity);
			MyAutomaticRifleGun myAutomaticRifleGun;
			if ((myAutomaticRifleGun = entity as MyAutomaticRifleGun) != null && character.GetPlayerIdentityId() != MySession.Static.LocalHumanPlayer.Identity.IdentityId && character.StatComp.LastDamage.Type == MyDamageType.Bullet && myAutomaticRifleGun.Owner.GetPlayerIdentityId() == MySession.Static.LocalHumanPlayer.Identity.IdentityId)
			{
				NotifyAchieved();
				MyCharacter.OnCharacterDied -= MyCharacter_OnCharacterDied;
			}
		}
	}
}
