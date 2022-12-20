using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace VRage
{
	public interface IMyImeCandidateList : IVRageGuiControl
	{
		Vector2 Position { get; set; }

		MyGuiDrawAlignEnum OriginAlign { get; set; }

		event Action<IMyImeCandidateList, int> ItemClicked;

		void Activate(bool autoPositionOnMouseTip, Vector2? offset = null);

		void Deactivate();

		void Clear();

		Vector2 GetListBoxSize();

		void AddItem(StringBuilder text, string tooltip = "", string icon = "", object userData = null);

		void CreateNewContextMenu();

		bool IsGuiControlEqual(IVRageGuiControl focusedControl);
	}
}
