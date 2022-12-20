using System.Collections.Generic;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "GBuffer Debug")]
	internal class MyGuiScreenDebugRenderGBufferDebug : MyGuiScreenDebugBase
	{
		private List<MyGuiControlCheckbox> m_cbs = new List<MyGuiControlCheckbox>();

		private bool m_radioUpdate;

		public MyGuiScreenDebugRenderGBufferDebug()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("GBuffer Debug", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			AddLabel("Gbuffer", Color.Yellow.ToVector4(), 1.2f);
			m_cbs.Clear();
			m_cbs.Add(AddCheckBox("Base color", MyRenderProxy.Settings.DisplayGbufferColor, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferColor = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Albedo", MyRenderProxy.Settings.DisplayGbufferAlbedo, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferAlbedo = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Normals", MyRenderProxy.Settings.DisplayGbufferNormal, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferNormal = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Normals view", MyRenderProxy.Settings.DisplayGbufferNormalView, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferNormalView = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Glossiness", MyRenderProxy.Settings.DisplayGbufferGlossiness, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferGlossiness = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Metalness", MyRenderProxy.Settings.DisplayGbufferMetalness, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferMetalness = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("NDotL", MyRenderProxy.Settings.DisplayNDotL, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayNDotL = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("LOD", MyRenderProxy.Settings.DisplayGbufferLOD, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferLOD = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Mipmap", MyRenderProxy.Settings.DisplayMipmap, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayMipmap = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Ambient occlusion", MyRenderProxy.Settings.DisplayGbufferAO, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayGbufferAO = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Emissive", MyRenderProxy.Settings.DisplayEmissive, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayEmissive = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Edge mask", MyRenderProxy.Settings.DisplayEdgeMask, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayEdgeMask = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Depth", MyRenderProxy.Settings.DisplayDepth, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayDepth = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Stencil", MyRenderProxy.Settings.DisplayStencil, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayStencil = x.IsChecked;
			}));
			m_currentPosition.Y += 0.01f;
			m_cbs.Add(AddCheckBox("Reprojection test", MyRenderProxy.Settings.DisplayReprojectedDepth, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayReprojectedDepth = x.IsChecked;
			}));
			m_currentPosition.Y += 0.01f;
			AddLabel("Environment light", Color.Yellow.ToVector4(), 1.2f);
			m_cbs.Add(AddCheckBox("Ambient diffuse", MyRenderProxy.Settings.DisplayAmbientDiffuse, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayAmbientDiffuse = x.IsChecked;
			}));
			m_cbs.Add(AddCheckBox("Ambient specular", MyRenderProxy.Settings.DisplayAmbientSpecular, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayAmbientSpecular = x.IsChecked;
			}));
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderGBufferDebug";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
			if (m_radioUpdate)
			{
				return;
			}
			m_radioUpdate = true;
			foreach (MyGuiControlCheckbox cb in m_cbs)
			{
				if (cb != sender)
				{
					cb.IsChecked = false;
				}
			}
			m_radioUpdate = false;
		}
	}
}
