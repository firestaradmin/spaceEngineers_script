using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens.Game
{
	[MyDebugScreen("Game", "PCU")]
	internal class MyGuiScreenDebugPCU : MyGuiScreenDebugBase
	{
		private List<MyCubeGrid> m_selectedGrids = new List<MyCubeGrid>();

		public MyGuiScreenDebugPCU()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("PCU", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddVerticalSpacing(0.01f * m_scale);
			AddCheckBox("Use console PCU", MySession.Static.Settings, MemberHelper.GetMember(() => MySession.Static.Settings.UseConsolePCU));
			AddVerticalSpacing(0.01f * m_scale);
			MyGuiControlCombobox identities = AddCombo();
			identities.AddItem(0L, "Nobody");
			foreach (MyIdentity item in MySession.Static?.Players.GetAllIdentities() ?? Array.Empty<MyIdentity>())
			{
				identities.AddItem(item.IdentityId, item.DisplayName);
			}
			AddButton("Set Authorship", delegate
			{
				ForEachBlockOnSelectedGrids(delegate(MySlimBlock x)
				{
					x.TransferAuthorship(identities.GetSelectedKey());
				});
			});
		}

		public override bool Update(bool hasFocus)
		{
			m_selectedGrids.Clear();
			MyCubeGrid targetGrid;
			if ((MySession.Static?.Ready ?? false) && (targetGrid = MyCubeGrid.GetTargetGrid()) != null)
			{
				MyCubeGridGroups.Static.Mechanical.GetGroupNodes(targetGrid, m_selectedGrids);
				if (m_selectedGrids.Count > 0)
				{
					BoundingBoxD aabb = BoundingBoxD.CreateInvalid();
					foreach (MyCubeGrid selectedGrid in m_selectedGrids)
					{
						aabb.Include(selectedGrid.PositionComp.WorldAABB);
					}
					MyRenderProxy.DebugDrawAABB(aabb, Color.Red);
				}
			}
			return base.Update(hasFocus);
		}

		private void ForEachBlockOnSelectedGrids(Action<MySlimBlock> func)
		{
<<<<<<< HEAD
			foreach (MyCubeGrid selectedGrid in m_selectedGrids)
			{
				foreach (MySlimBlock cubeBlock in selectedGrid.CubeBlocks)
				{
					func(cubeBlock);
=======
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			foreach (MyCubeGrid selectedGrid in m_selectedGrids)
			{
				Enumerator<MySlimBlock> enumerator2 = selectedGrid.CubeBlocks.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MySlimBlock current = enumerator2.get_Current();
						func(current);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}
	}
}
