using System;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;

namespace Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Projector))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyProjector),
		typeof(Sandbox.ModAPI.Ingame.IMyProjector)
	})]
	public class MySpaceProjector : MyProjectorBase
	{
		private new const float ROTATION_LIMIT = 180f;

		public MySpaceProjector()
		{
			CreateTerminalControls();
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MySpaceProjector>())
			{
				return;
			}
			base.CreateTerminalControls();
			if (!MyFakes.ENABLE_PROJECTOR_BLOCK)
			{
				return;
			}
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MySpaceProjector>("Blueprint", MyCommonTexts.Blueprints, MySpaceTexts.Blank, delegate(MySpaceProjector p)
			{
				p.SelectBlueprint();
			})
			{
				Enabled = (MySpaceProjector b) => b.CanProject(),
				SupportsMultipleBlocks = false
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MySpaceProjector>("Remove", MySpaceTexts.RemoveProjectionButton, MySpaceTexts.Blank, delegate(MySpaceProjector p)
			{
				p.SendRemoveProjection();
			})
			{
				Enabled = (MySpaceProjector b) => b.IsProjecting()
			});
			MyTerminalControlCheckbox<MySpaceProjector> obj = new MyTerminalControlCheckbox<MySpaceProjector>("KeepProjection", MySpaceTexts.KeepProjectionToggle, MySpaceTexts.KeepProjectionTooltip)
			{
				Getter = (MySpaceProjector x) => x.KeepProjection,
				Setter = delegate(MySpaceProjector x, bool v)
				{
					x.KeepProjection = v;
				}
			};
			obj.EnableAction();
			obj.Enabled = (MySpaceProjector b) => b.IsProjecting() && b.AllowWelding;
			MyTerminalControlFactory.AddControl(obj);
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySpaceProjector>("ShowOnlyBuildable", MySpaceTexts.ShowOnlyBuildableBlockToggle, MySpaceTexts.ShowOnlyBuildableTooltip, null, null, justify: false, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true, 0.2f)
			{
				Getter = (MySpaceProjector x) => x.m_showOnlyBuildable,
				Setter = delegate(MySpaceProjector x, bool v)
				{
					x.m_showOnlyBuildable = v;
					x.OnOffsetsChanged();
				},
				Enabled = (MySpaceProjector b) => b.IsProjecting() && b.AllowWelding
			});
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider = new MyTerminalControlSlider<MySpaceProjector>("X", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetX, MySpaceTexts.Blank);
			myTerminalControlSlider.SetLimits(-50f, 50f);
			myTerminalControlSlider.DefaultValue = 0f;
			myTerminalControlSlider.Getter = (MySpaceProjector x) => x.m_projectionOffset.X;
			myTerminalControlSlider.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionOffset.X = Convert.ToInt32(v);
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32(x.m_projectionOffset.X);
			};
			myTerminalControlSlider.EnableActions(0.01f);
			myTerminalControlSlider.Enabled = (MySpaceProjector x) => x.IsProjecting();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider2 = new MyTerminalControlSlider<MySpaceProjector>("Y", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetY, MySpaceTexts.Blank);
			myTerminalControlSlider2.SetLimits(-50f, 50f);
			myTerminalControlSlider2.DefaultValue = 0f;
			myTerminalControlSlider2.Getter = (MySpaceProjector x) => x.m_projectionOffset.Y;
			myTerminalControlSlider2.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionOffset.Y = Convert.ToInt32(v);
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider2.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32(x.m_projectionOffset.Y);
			};
			myTerminalControlSlider2.EnableActions(0.01f);
			myTerminalControlSlider2.Enabled = (MySpaceProjector x) => x.IsProjecting();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider3 = new MyTerminalControlSlider<MySpaceProjector>("Z", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetZ, MySpaceTexts.Blank);
			myTerminalControlSlider3.SetLimits(-50f, 50f);
			myTerminalControlSlider3.DefaultValue = 0f;
			myTerminalControlSlider3.Getter = (MySpaceProjector x) => x.m_projectionOffset.Z;
			myTerminalControlSlider3.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionOffset.Z = Convert.ToInt32(v);
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider3.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32(x.m_projectionOffset.Z);
			};
			myTerminalControlSlider3.EnableActions(0.01f);
			myTerminalControlSlider3.Enabled = (MySpaceProjector x) => x.IsProjecting();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider4 = new MyTerminalControlSlider<MySpaceProjector>("RotX", MySpaceTexts.BlockPropertyTitle_ProjectionRotationX, MySpaceTexts.Blank);
			myTerminalControlSlider4.SetLimits(-180f, 180f);
			myTerminalControlSlider4.DefaultValue = 0f;
			myTerminalControlSlider4.Getter = (MySpaceProjector x) => x.m_projectionRotation.X * x.BlockDefinition.RotationAngleStepDeg;
			myTerminalControlSlider4.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionRotation.X = Convert.ToInt32(v / (float)x.BlockDefinition.RotationAngleStepDeg);
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider4.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32(x.m_projectionRotation.X * x.BlockDefinition.RotationAngleStepDeg).Append("°");
			};
			myTerminalControlSlider4.EnableActions(0.25f);
			myTerminalControlSlider4.Enabled = (MySpaceProjector x) => x.IsProjecting();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider5 = new MyTerminalControlSlider<MySpaceProjector>("RotY", MySpaceTexts.BlockPropertyTitle_ProjectionRotationY, MySpaceTexts.Blank);
			myTerminalControlSlider5.SetLimits(-180f, 180f);
			myTerminalControlSlider5.DefaultValue = 0f;
			myTerminalControlSlider5.Getter = (MySpaceProjector x) => x.m_projectionRotation.Y * x.BlockDefinition.RotationAngleStepDeg;
			myTerminalControlSlider5.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionRotation.Y = Convert.ToInt32(v / (float)x.BlockDefinition.RotationAngleStepDeg);
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider5.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32(x.m_projectionRotation.Y * x.BlockDefinition.RotationAngleStepDeg).Append("°");
			};
			myTerminalControlSlider5.EnableActions(0.25f);
			myTerminalControlSlider5.Enabled = (MySpaceProjector x) => x.IsProjecting();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider6 = new MyTerminalControlSlider<MySpaceProjector>("RotZ", MySpaceTexts.BlockPropertyTitle_ProjectionRotationZ, MySpaceTexts.Blank);
			myTerminalControlSlider6.SetLimits(-180f, 180f);
			myTerminalControlSlider6.DefaultValue = 0f;
			myTerminalControlSlider6.Getter = (MySpaceProjector x) => x.m_projectionRotation.Z * x.BlockDefinition.RotationAngleStepDeg;
			myTerminalControlSlider6.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionRotation.Z = Convert.ToInt32(v / (float)x.BlockDefinition.RotationAngleStepDeg);
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider6.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32(x.m_projectionRotation.Z * x.BlockDefinition.RotationAngleStepDeg).Append("°");
			};
			myTerminalControlSlider6.EnableActions(0.25f);
			myTerminalControlSlider6.Enabled = (MySpaceProjector x) => x.IsProjecting();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider6);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider7 = new MyTerminalControlSlider<MySpaceProjector>("Scale", MySpaceTexts.BlockPropertyTitle_Scale, MySpaceTexts.Blank);
			myTerminalControlSlider7.SetLimits(0.02f, 1f);
			myTerminalControlSlider7.DefaultValue = 1f;
			myTerminalControlSlider7.Getter = (MySpaceProjector x) => x.m_projectionScale;
			myTerminalControlSlider7.Setter = delegate(MySpaceProjector x, float v)
			{
				x.m_projectionScale = v;
				x.OnOffsetsChanged();
			};
			myTerminalControlSlider7.Writer = delegate(MySpaceProjector x, StringBuilder result)
			{
				result.AppendInt32((int)(x.m_projectionScale * 100f)).Append("%");
			};
			myTerminalControlSlider7.EnableActions(0.01f);
			myTerminalControlSlider7.Enabled = (MySpaceProjector x) => x.IsProjecting() && x.AllowScaling;
			MyTerminalControlFactory.AddControl(myTerminalControlSlider7);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySpaceProjector>
			{
				Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled()
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlLabel<MySpaceProjector>(MySpaceTexts.TerminalScenarioSettingsLabel)
			{
				Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled()
			});
			MyTerminalControlButton<MySpaceProjector> obj2 = new MyTerminalControlButton<MySpaceProjector>("SpawnProjection", MySpaceTexts.BlockPropertyTitle_ProjectionSpawn, MySpaceTexts.Blank, delegate(MySpaceProjector p)
			{
				p.TrySpawnProjection();
			})
			{
				Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled(),
				Enabled = (MySpaceProjector p) => p.CanSpawnProjection()
			};
			obj2.EnableAction();
			MyTerminalControlFactory.AddControl(obj2);
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySpaceProjector>("InstantBuilding", MySpaceTexts.BlockPropertyTitle_Projector_InstantBuilding, MySpaceTexts.BlockPropertyTitle_Projector_InstantBuilding_Tooltip)
			{
				Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled(),
				Enabled = (MySpaceProjector p) => p.CanEnableInstantBuilding(),
				Getter = (MySpaceProjector p) => p.InstantBuildingEnabled,
				Setter = delegate(MySpaceProjector p, bool v)
				{
					p.TrySetInstantBuilding(v);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySpaceProjector>("GetOwnership", MySpaceTexts.BlockPropertyTitle_Projector_GetOwnership, MySpaceTexts.BlockPropertiesTooltip_Projector_GetOwnership)
			{
				Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled(),
				Enabled = (MySpaceProjector p) => p.CanEditInstantBuildingSettings(),
				Getter = (MySpaceProjector p) => p.GetOwnershipFromProjector,
				Setter = delegate(MySpaceProjector p, bool v)
				{
					p.TrySetGetOwnership(v);
				}
			});
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider8 = new MyTerminalControlSlider<MySpaceProjector>("NumberOfProjections", MySpaceTexts.BlockPropertyTitle_Projector_NumberOfProjections, MySpaceTexts.BlockPropertyTitle_Projector_NumberOfProjections_Tooltip);
			myTerminalControlSlider8.Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled();
			myTerminalControlSlider8.Enabled = (MySpaceProjector p) => p.CanEditInstantBuildingSettings();
			myTerminalControlSlider8.Getter = (MySpaceProjector p) => p.MaxNumberOfProjections;
			myTerminalControlSlider8.Setter = delegate(MySpaceProjector p, float v)
			{
				p.TryChangeNumberOfProjections(v);
			};
			myTerminalControlSlider8.Writer = delegate(MySpaceProjector p, StringBuilder s)
			{
				if (p.MaxNumberOfProjections == 1000)
				{
					s.AppendStringBuilder(MyTexts.Get(MySpaceTexts.ScreenTerminal_Infinite));
				}
				else
				{
					s.AppendInt32(p.MaxNumberOfProjections);
				}
			};
			myTerminalControlSlider8.SetLogLimits(1f, 1000f);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider8);
			MyTerminalControlSlider<MySpaceProjector> myTerminalControlSlider9 = new MyTerminalControlSlider<MySpaceProjector>("NumberOfBlocks", MySpaceTexts.BlockPropertyTitle_Projector_BlocksPerProjection, MySpaceTexts.BlockPropertyTitle_Projector_BlocksPerProjection_Tooltip);
			myTerminalControlSlider9.Visible = (MySpaceProjector p) => p.ScenarioSettingsEnabled();
			myTerminalControlSlider9.Enabled = (MySpaceProjector p) => p.CanEditInstantBuildingSettings();
			myTerminalControlSlider9.Getter = (MySpaceProjector p) => p.MaxNumberOfBlocksPerProjection;
			myTerminalControlSlider9.Setter = delegate(MySpaceProjector p, float v)
			{
				p.TryChangeMaxNumberOfBlocksPerProjection(v);
			};
			myTerminalControlSlider9.Writer = delegate(MySpaceProjector p, StringBuilder s)
			{
				if (p.MaxNumberOfBlocksPerProjection == 10000)
				{
					s.AppendStringBuilder(MyTexts.Get(MySpaceTexts.ScreenTerminal_Infinite));
				}
				else
				{
					s.AppendInt32(p.MaxNumberOfBlocksPerProjection);
				}
			};
			myTerminalControlSlider9.SetLogLimits(1f, 10000f);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider9);
<<<<<<< HEAD
=======
			MyMultiTextPanelComponent.CreateTerminalControls<MySpaceProjector>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink != null && !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return false;
			}
			return base.CheckIsWorking();
		}
	}
}
