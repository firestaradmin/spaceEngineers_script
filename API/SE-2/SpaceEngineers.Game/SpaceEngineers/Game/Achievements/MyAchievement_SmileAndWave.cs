using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_SmileAndWave : MySteamAchievementBase
	{
		private const string WAVE_ANIMATION_NAME = "RightHand/Emote";

		private readonly string ANIMATION_CLIP_NAME = "AnimStack::Astronaut_character_wave_final";

		private MyStringId m_waveAnimationId;

		private MyCharacter m_localCharacter;

		private MyCharacter LocalCharacter
		{
			get
			{
				return m_localCharacter;
			}
			set
			{
				if (m_localCharacter != value)
				{
					if (m_localCharacter != null)
					{
						m_localCharacter.CharacterDied -= OnTrackedCharacterDied;
						m_localCharacter.OnMarkForClose -= OnTrackedCharacterClosed;
						m_localCharacter.AnimationController.ActionTriggered -= AnimationControllerOnActionTriggered;
					}
					m_localCharacter = value;
					if (m_localCharacter != null)
					{
						m_localCharacter.CharacterDied += OnTrackedCharacterDied;
						m_localCharacter.OnMarkForClose += OnTrackedCharacterClosed;
						m_localCharacter.AnimationController.ActionTriggered += AnimationControllerOnActionTriggered;
					}
				}
			}
		}

		public override bool NeedsUpdate => LocalCharacter == null;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_SmileAndWave", null, 0f);
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved && !MySession.Static.CreativeMode)
			{
				m_waveAnimationId = MyStringId.GetOrCompute("Wave");
			}
		}

		public override void SessionUpdate()
		{
			if (!base.IsAchieved)
			{
				LocalCharacter = MySession.Static.LocalCharacter;
			}
		}

		private void OnTrackedCharacterClosed(MyEntity entity)
		{
			OnTrackedCharacterDied(entity as MyCharacter);
		}

		private void OnTrackedCharacterDied(MyCharacter character)
		{
			LocalCharacter = null;
		}

		private void AnimationControllerOnActionTriggered(MyStringId animationAction)
		{
			if (animationAction != m_waveAnimationId)
			{
				return;
			}
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			Vector3D value = localCharacter.PositionComp.GetPosition();
			long factionId = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId)?.FactionId ?? 0;
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Character == null || onlinePlayer.Character == localCharacter)
				{
					continue;
				}
				Vector3D value2 = onlinePlayer.Character.PositionComp.GetPosition();
				Vector3D.DistanceSquared(ref value2, ref value, out var result);
				if (result < 25.0)
				{
					long factionId2 = MySession.Static.Factions.GetPlayerFaction(onlinePlayer.Identity.IdentityId)?.FactionId ?? 0;
					if (MySession.Static.Factions.AreFactionsEnemies(factionId, factionId2) && IsPlayerWaving(onlinePlayer.Character) && PlayersLookingFaceToFace(localCharacter, onlinePlayer.Character))
					{
						NotifyAchieved();
						localCharacter.AnimationController.ActionTriggered -= AnimationControllerOnActionTriggered;
						break;
					}
				}
			}
		}

		private bool PlayersLookingFaceToFace(MyCharacter firstCharacter, MyCharacter secondCharacter)
		{
			Vector3D vector = firstCharacter.GetHeadMatrix(includeY: false).Forward;
			Vector3D vector2 = secondCharacter.GetHeadMatrix(includeY: false).Forward;
			Vector3D.Dot(ref vector, ref vector2, out var result);
			return result < -0.5;
		}

		private bool IsPlayerWaving(MyCharacter character)
		{
			MyAnimationController controller = character.AnimationController.Controller;
			for (int i = 0; i < controller.GetLayerCount(); i++)
			{
				MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(i);
				MyAnimationStateMachineNode myAnimationStateMachineNode;
				MyAnimationTreeNodeTrack myAnimationTreeNodeTrack;
				if (layerByIndex.CurrentNode != null && layerByIndex.CurrentNode.Name != null && layerByIndex.CurrentNode.Name == "RightHand/Emote" && (myAnimationStateMachineNode = layerByIndex.CurrentNode as MyAnimationStateMachineNode) != null && (myAnimationTreeNodeTrack = myAnimationStateMachineNode.RootAnimationNode as MyAnimationTreeNodeTrack) != null && myAnimationTreeNodeTrack.AnimationClip.Name == ANIMATION_CLIP_NAME)
				{
					return true;
				}
			}
			return false;
		}
	}
}
