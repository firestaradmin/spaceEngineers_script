using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.GUI;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects
{
	[MyUseObject("contract")]
	public class MyUseObjectContractBlock : MyUseObjectBase
	{
		public override MatrixD ActivationMatrix => base.Dummy.Matrix * base.Owner.WorldMatrix;

		public override MatrixD WorldMatrix => base.Owner.WorldMatrix;

		public override uint RenderObjectID => base.Owner.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions
		{
			get
			{
				UseActionEnum useActionEnum = PrimaryAction | SecondaryAction;
				IMyEntity owner = base.Owner;
				if (owner != null && owner.HasInventory)
				{
					useActionEnum |= UseActionEnum.OpenInventory;
				}
				return useActionEnum;
			}
		}

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public MyUseObjectContractBlock(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyCubeBlock myCubeBlock = base.Owner as MyCubeBlock;
			string text = ((myCubeBlock != null) ? myCubeBlock.DefinitionDisplayNameText : ((MyEntity)base.Owner).DisplayNameText);
			MyActionDescription result;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenContract;
				result.FormatParams = new object[1] { string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]") };
				result.IsTextControlHint = true;
				result.JoystickFormatParams = new object[1] { MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.USE) };
				result.ShowForGamepad = true;
				return result;
			default:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenTerminal;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), "]"),
					text
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintJoystickPressToOpenControlPanel;
				result.JoystickFormatParams = new object[1] { text };
				result.ShowForGamepad = true;
				return result;
			}
		}

		public override bool HandleInput()
		{
			return false;
		}

		public override void OnSelectionLost()
		{
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity userEntity)
		{
			MyCharacter myCharacter;
			if ((myCharacter = userEntity as MyCharacter) == null)
			{
				return;
			}
			MyPlayer.GetPlayerFromCharacter(myCharacter);
			MyContractBlock myContractBlock;
			if ((myContractBlock = base.Owner as MyContractBlock) == null)
			{
				return;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = myContractBlock.GetUserRelationToOwner(myCharacter.ControllerInfo.ControllingIdentityId);
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
			{
				bool flag = false;
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(myContractBlock.OwnerId);
				if (myFaction != null)
				{
					flag = MySession.Static.Factions.IsNpcFaction(myFaction.Tag);
				}
				if (!userRelationToOwner.IsFriendly() && (!myContractBlock.AnyoneCanUse || (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies && flag)) && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
				else if (myContractBlock.IsWorking)
				{
					IMyGuiScreenFactoryService service = ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>();
					if (service != null && !service.IsAnyScreenOpen)
					{
						service.IsAnyScreenOpen = true;
						MyContractsBlockViewModel myContractsBlockViewModel = new MyContractsBlockViewModel(myContractBlock);
						MyGuiScreenContractsBlock myGuiScreenContractsBlock = MyGuiSandbox.CreateScreen<MyGuiScreenContractsBlock>(new object[1] { myContractsBlockViewModel });
						myGuiScreenContractsBlock.Closed += Screen_Closed;
						MyGuiSandbox.AddScreen(myGuiScreenContractsBlock);
					}
				}
				break;
			}
			case UseActionEnum.OpenTerminal:
				if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
					{
						MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					}
				}
				else
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, myCharacter, (MyEntity)base.Owner);
				}
				break;
			case UseActionEnum.OpenInventory:
				if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
					{
						MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					}
				}
				else
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, base.Owner as MyEntity);
				}
				break;
			case UseActionEnum.Manipulate | UseActionEnum.OpenTerminal:
				break;
			}
		}

		private void Screen_Closed(MyGuiScreenBase screen, bool isUnloading)
		{
			screen.Closed -= Screen_Closed;
			IMyGuiScreenFactoryService service = ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>();
			if (service != null)
			{
				service.IsAnyScreenOpen = false;
			}
		}
	}
}
