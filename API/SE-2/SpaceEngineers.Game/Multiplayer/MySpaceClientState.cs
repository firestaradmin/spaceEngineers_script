using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Collections;

namespace Multiplayer
{
	internal class MySpaceClientState : MyClientState
	{
		private static MyContextKind GetContextByPage(MyTerminalPageEnum page)
		{
<<<<<<< HEAD
			switch (page)
			{
			case MyTerminalPageEnum.Inventory:
				return MyContextKind.Inventory;
			case MyTerminalPageEnum.ControlPanel:
				return MyContextKind.Terminal;
			case MyTerminalPageEnum.Production:
				return MyContextKind.Production;
			default:
				return MyContextKind.None;
			}
=======
			return page switch
			{
				MyTerminalPageEnum.Inventory => MyContextKind.Inventory, 
				MyTerminalPageEnum.ControlPanel => MyContextKind.Terminal, 
				MyTerminalPageEnum.Production => MyContextKind.Production, 
				_ => MyContextKind.None, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected override void WriteInternal(BitStream stream, MyEntity controlledEntity)
		{
			MyContextKind contextByPage = GetContextByPage(MyGuiScreenTerminal.GetCurrentScreen());
			stream.WriteInt32((int)contextByPage, 2);
			if (contextByPage != 0)
			{
				long value = ((MyGuiScreenTerminal.InteractedEntity != null) ? MyGuiScreenTerminal.InteractedEntity.EntityId : 0);
				stream.WriteInt64(value);
			}
		}

		protected override void ReadInternal(BitStream stream, MyEntity controlledEntity)
		{
			base.Context = (MyContextKind)stream.ReadInt32(2);
			if (base.Context != 0)
			{
				long entityId = stream.ReadInt64();
				base.ContextEntity = MyEntities.GetEntityByIdOrDefault(entityId, null, allowClosed: true);
				if (base.ContextEntity != null && base.ContextEntity.GetTopMostParent().MarkedForClose)
				{
					base.ContextEntity = null;
				}
			}
			else
			{
				base.ContextEntity = null;
			}
		}
	}
}
