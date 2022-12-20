using System;
using VRage.GameServices;

namespace Sandbox.Game.Gui
{
	public class MyBlueprintItemInfo
	{
		public ulong Size;

		public MyBlueprintTypeEnum Type { get; set; }

		public MyWorkshopItem Item { get; set; }

		public DateTime? TimeCreated { get; set; }

		public DateTime? TimeUpdated { get; set; }

		public string BlueprintName { get; set; }

		public string CloudPathXML { get; set; }

		public string CloudPathPB { get; set; }

		public string CloudPathCS { get; set; }

		public bool IsDirectory { get; set; }

		public MyCloudFileInfo CloudInfo { get; set; }

		public AdditionalBlueprintData Data { get; set; }

		public MyBlueprintItemInfo(MyBlueprintTypeEnum type)
		{
			Type = type;
			Data = new AdditionalBlueprintData();
		}

		public void SetAdditionalBlueprintInformation(string name = null, string description = null, uint[] dlcs = null)
		{
			Data.Name = name ?? string.Empty;
			Data.Description = description ?? string.Empty;
			Data.CloudImagePath = string.Empty;
			Data.DLCs = dlcs;
		}

		public override bool Equals(object obj)
		{
			MyBlueprintItemInfo myBlueprintItemInfo = obj as MyBlueprintItemInfo;
			if (myBlueprintItemInfo == null)
			{
				return false;
			}
			if (Type.Equals(myBlueprintItemInfo.Type) && string.Equals(BlueprintName, myBlueprintItemInfo.BlueprintName, StringComparison.Ordinal) && string.Equals(Data.Name, myBlueprintItemInfo.Data.Name, StringComparison.Ordinal))
			{
				return string.Equals(Data.Description, myBlueprintItemInfo.Data.Description, StringComparison.Ordinal);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return BlueprintName.GetHashCode() ^ Data.Name.GetHashCode() ^ Data.Description.GetHashCode();
		}
	}
}
