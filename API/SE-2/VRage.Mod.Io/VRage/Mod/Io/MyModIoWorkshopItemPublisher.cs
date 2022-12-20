using VRage.GameServices;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal class MyModIoWorkshopItemPublisher : MyWorkshopItemPublisher
	{
		private MyModIoServiceInternal m_service;

		private MyModIoWorkshopItem m_item;

		public MyModIoWorkshopItemPublisher(MyModIoServiceInternal service)
		{
			m_service = service;
		}

		public MyModIoWorkshopItemPublisher(MyModIoServiceInternal service, MyWorkshopItem item)
		{
			m_service = service;
			m_item = (MyModIoWorkshopItem)item;
			Init(item);
		}

		public override void Publish()
		{
			base.Publish();
			MyModIo.AddOrEditMod(base.Id, base.Title, base.Description, base.Visibility, Tags, MyModMetadataLoader.Serialize(base.Metadata), base.Thumbnail, base.Folder, OnPublished);
		}

		private void OnPublished(ModProfile mod, Modfile modFile, MyGameServiceCallResult result)
		{
			MyLog.Default.Info("Workshop item with id {0} update finished. Result: {1}", modFile?.mod_id ?? 0, result);
			if (result == MyGameServiceCallResult.OK)
			{
				if (m_item != null)
				{
					m_item.NameId = mod.name_id;
				}
				base.Id = (ulong)modFile.mod_id;
				MyModIo.Subscribe(base.Id, state: true);
			}
			MyWorkshopItem myWorkshopItem = m_service.CreateWorkshopItem();
			myWorkshopItem.Init(this);
			if (result == MyGameServiceCallResult.OK)
			{
				((MyModIoWorkshopItem)myWorkshopItem).NameId = mod.name_id;
			}
			OnItemPublished(result, myWorkshopItem);
		}
	}
}
