using System;
using System.Text;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.Renders;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Lights;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_HeatVentBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyHeatVent),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyHeatVent)
	})]
	public class MyHeatVentBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyHeatVent, SpaceEngineers.Game.ModAPI.Ingame.IMyHeatVent, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyLightingLogicSync
	{
		private class MyChokeSubpart : MyEntitySubpart
		{
			private MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation m_subpartRotation;

			public MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation SubpartRotation
			{
				get
				{
					return m_subpartRotation;
				}
				set
				{
					m_subpartRotation = value;
					((MyRenderComponentHeatVent.MyChokeRenderComponent)base.Render).SubpartRotation = value;
				}
			}

			public new MyRenderComponentHeatVent.MyChokeRenderComponent Render => (MyRenderComponentHeatVent.MyChokeRenderComponent)base.Render;

			public new MyHeatVentBlock Parent => (MyHeatVentBlock)base.Parent;

			public override void InitComponents()
			{
				MyRenderComponentHeatVent.MyChokeRenderComponent myChokeRenderComponent = new MyRenderComponentHeatVent.MyChokeRenderComponent();
				myChokeRenderComponent.SubpartRotation = SubpartRotation;
				base.Render = myChokeRenderComponent;
				base.InitComponents();
			}
		}

		protected class _003CBlinkIntervalSecondsSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CBlinkIntervalSecondsSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkIntervalSecondsSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).BlinkIntervalSecondsSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkIntervalSecondsSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CBlinkLengthSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CBlinkLengthSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkLengthSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).BlinkLengthSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkLengthSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CBlinkOffsetSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CBlinkOffsetSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkOffsetSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).BlinkOffsetSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkOffsetSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightColorSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightColorSync_003Ek__BackingField;
				ISyncType result = (_003CLightColorSync_003Ek__BackingField = new Sync<Color, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).LightColorSync = (Sync<Color, SyncDirection.BothWays>)_003CLightColorSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CIntensitySync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CIntensitySync_003Ek__BackingField;
				ISyncType result = (_003CIntensitySync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).IntensitySync = (Sync<float, SyncDirection.BothWays>)_003CIntensitySync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightRadiusSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightRadiusSync_003Ek__BackingField;
				ISyncType result = (_003CLightRadiusSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).LightRadiusSync = (Sync<float, SyncDirection.BothWays>)_003CLightRadiusSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightFalloffSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightFalloffSync_003Ek__BackingField;
				ISyncType result = (_003CLightFalloffSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).LightFalloffSync = (Sync<float, SyncDirection.BothWays>)_003CLightFalloffSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightOffsetSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightOffsetSync_003Ek__BackingField;
				ISyncType result = (_003CLightOffsetSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).LightOffsetSync = (Sync<float, SyncDirection.BothWays>)_003CLightOffsetSync_003Ek__BackingField;
				return result;
			}
		}

		protected class m_colorMaximal_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType colorMaximal;
				ISyncType result = (colorMaximal = new Sync<Color, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_colorMaximal = (Sync<Color, SyncDirection.BothWays>)colorMaximal;
				return result;
			}
		}

		protected class m_colorMinimal_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType colorMinimal;
				ISyncType result = (colorMinimal = new Sync<Color, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_colorMinimal = (Sync<Color, SyncDirection.BothWays>)colorMinimal;
				return result;
			}
		}

		protected class m_powerDependency_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType powerDependency;
				ISyncType result = (powerDependency = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_powerDependency = (Sync<float, SyncDirection.BothWays>)powerDependency;
				return result;
			}
		}

		protected class m_lightIntensityDefault_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightIntensityDefault;
				ISyncType result = (lightIntensityDefault = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_lightIntensityDefault = (Sync<float, SyncDirection.BothWays>)lightIntensityDefault;
				return result;
			}
		}

		protected class m_lightRadiusDefault_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightRadiusDefault;
				ISyncType result = (lightRadiusDefault = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_lightRadiusDefault = (Sync<float, SyncDirection.BothWays>)lightRadiusDefault;
				return result;
			}
		}

		protected class m_lightFalloffDefault_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightFalloffDefault;
				ISyncType result = (lightFalloffDefault = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_lightFalloffDefault = (Sync<float, SyncDirection.BothWays>)lightFalloffDefault;
				return result;
			}
		}

		protected class m_lightOffsetDefault_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightOffsetDefault;
				ISyncType result = (lightOffsetDefault = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyHeatVentBlock)P_0).m_lightOffsetDefault = (Sync<float, SyncDirection.BothWays>)lightOffsetDefault;
				return result;
			}
		}

		private Sync<Color, SyncDirection.BothWays> m_colorMaximal;

		private Sync<Color, SyncDirection.BothWays> m_colorMinimal;

		private Sync<float, SyncDirection.BothWays> m_powerDependency;

		private Sync<float, SyncDirection.BothWays> m_lightIntensityDefault;

		private Sync<float, SyncDirection.BothWays> m_lightRadiusDefault;

		private Sync<float, SyncDirection.BothWays> m_lightFalloffDefault;

		private Sync<float, SyncDirection.BothWays> m_lightOffsetDefault;

		private bool m_hardRecount;

		private float m_currentPowerLevel;

		private bool m_inited;

		private float m_requiredPower;

		private float m_lastUsedGridPowerRatio;

		private string m_emissiveMaterialName = "Emissive";

		private string m_lightDummyName = "EmissiveSpotlight";

		private MyLightingLogic m_lightingLogic;

		private MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation[] m_subpartRotations;

		public Sync<float, SyncDirection.BothWays> BlinkIntervalSecondsSync { get; set; }

		public Sync<float, SyncDirection.BothWays> BlinkLengthSync { get; set; }

		public Sync<float, SyncDirection.BothWays> BlinkOffsetSync { get; set; }

		public Sync<Color, SyncDirection.BothWays> LightColorSync { get; set; }

		public Sync<float, SyncDirection.BothWays> IntensitySync { get; set; }

		public Sync<float, SyncDirection.BothWays> LightRadiusSync { get; set; }

		public Sync<float, SyncDirection.BothWays> LightFalloffSync { get; set; }

		public Sync<float, SyncDirection.BothWays> LightOffsetSync { get; set; }

		public Color ColorMinimal
		{
			get
			{
				return m_colorMinimal;
			}
			set
			{
				m_colorMinimal.ValidateAndSet(value);
			}
		}

		public Color ColorMaximal
		{
			get
			{
				return m_colorMaximal;
			}
			set
			{
				m_colorMaximal.ValidateAndSet(value);
			}
		}

		public float PowerDependency
		{
			get
			{
				return m_powerDependency;
			}
			set
			{
				m_powerDependency.ValidateAndSet(value);
			}
		}

		public Color ColorCurrent
		{
			get
			{
				return m_lightingLogic.Color;
			}
			set
			{
				if (m_lightingLogic.Color != value)
				{
					m_lightingLogic.Color = value;
				}
			}
		}

		public float CurrentPowerLevel => m_currentPowerLevel;

		public bool IsLargeLight { get; private set; }

		public new MyHeatVentBlockDefinition BlockDefinition => (MyHeatVentBlockDefinition)base.BlockDefinition;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyObjectBuilder_HeatVentBlock myObjectBuilder_HeatVentBlock = (MyObjectBuilder_HeatVentBlock)objectBuilder;
			if (float.IsNaN(myObjectBuilder_HeatVentBlock.RequiredPowerInput))
			{
				myObjectBuilder_HeatVentBlock.PowerDependency = BlockDefinition.PowerDependency;
				myObjectBuilder_HeatVentBlock.RequiredPowerInput = BlockDefinition.PowerDependency;
				myObjectBuilder_HeatVentBlock.ColorMaximalPower = BlockDefinition.ColorMaximalPower;
				myObjectBuilder_HeatVentBlock.ColorMinimalPower = BlockDefinition.ColorMinimalPower;
				myObjectBuilder_HeatVentBlock.SubpartRotations = BlockDefinition.SubpartRotations;
				myObjectBuilder_HeatVentBlock.Falloff = BlockDefinition.LightFalloffBounds.Default;
				myObjectBuilder_HeatVentBlock.Intensity = BlockDefinition.LightIntensityBounds.Default;
				myObjectBuilder_HeatVentBlock.Radius = BlockDefinition.LightRadiusBounds.Default;
				myObjectBuilder_HeatVentBlock.Offset = BlockDefinition.LightOffsetBounds.Default;
			}
			m_requiredPower = myObjectBuilder_HeatVentBlock.RequiredPowerInput;
			m_subpartRotations = myObjectBuilder_HeatVentBlock.SubpartRotations;
			m_emissiveMaterialName = BlockDefinition.EmissiveMaterialName;
			m_lightDummyName = BlockDefinition.LightDummyName;
			base.Init(objectBuilder, cubeGrid);
			ColorMinimal = myObjectBuilder_HeatVentBlock.ColorMinimalPower;
			ColorMaximal = myObjectBuilder_HeatVentBlock.ColorMaximalPower;
			m_powerDependency.ValidateRange(0f, 1f);
			m_powerDependency.SetLocalValue(myObjectBuilder_HeatVentBlock.PowerDependency);
			LightFalloffSync.ValidateRange(BlockDefinition.LightFalloffBounds);
			LightFalloffSync.Value = myObjectBuilder_HeatVentBlock.Falloff;
			m_lightFalloffDefault.Value = (float.IsNaN(myObjectBuilder_HeatVentBlock.Falloff) ? 0f : myObjectBuilder_HeatVentBlock.Falloff);
			IntensitySync.ValidateRange(BlockDefinition.LightIntensityBounds);
			IntensitySync.Value = myObjectBuilder_HeatVentBlock.Intensity;
			m_lightIntensityDefault.Value = (float.IsNaN(myObjectBuilder_HeatVentBlock.Intensity) ? 0f : myObjectBuilder_HeatVentBlock.Intensity);
			LightRadiusSync.ValidateRange(BlockDefinition.LightRadiusBounds);
			LightRadiusSync.Value = myObjectBuilder_HeatVentBlock.Radius;
			m_lightRadiusDefault.Value = (float.IsNaN(myObjectBuilder_HeatVentBlock.Radius) ? 0f : myObjectBuilder_HeatVentBlock.Radius);
			LightOffsetSync.ValidateRange(BlockDefinition.LightOffsetBounds);
			LightOffsetSync.Value = myObjectBuilder_HeatVentBlock.Offset;
			m_lightOffsetDefault.Value = (float.IsNaN(myObjectBuilder_HeatVentBlock.Offset) ? 0f : myObjectBuilder_HeatVentBlock.Offset);
			m_lightingLogic = new MyLightingLogic(this, BlockDefinition, this);
			m_lightingLogic.OnPropertiesChanged += base.RaisePropertiesChanged;
			m_lightingLogic.OnUpdateEnabled += UpdateEnabled;
			m_lightingLogic.OnInitLight += InitLight;
			m_lightingLogic.Radius = BlockDefinition.LightRadiusBounds.Default;
			m_lightingLogic.Initialize();
			IsLargeLight = cubeGrid.GridSizeEnum == MyCubeSize.Large;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.IsWorkingChanged += CubeBlock_OnWorkingChanged;
			MyCubeGrid cubeGrid2 = base.CubeGrid;
			cubeGrid2.IsPreviewChanged = (Action<bool>)Delegate.Combine(cubeGrid2.IsPreviewChanged, new Action<bool>(OnIsPreviewChanged));
			CreateTerminalControls();
			foreach (MyChokeSubpart value in base.Subparts.Values)
			{
				value.Render.SetSpeed(1f);
			}
			base.PropertiesChanged += MyHeatVentBlock_PropertiesChanged;
			m_inited = true;
		}

		private void MyHeatVentBlock_PropertiesChanged(MyTerminalBlock obj)
		{
			m_hardRecount = true;
		}

		private void InitLight(MyLight light, Vector4 color, float radius, float falloff)
		{
			light.Start(color, base.CubeGrid.GridScale * radius, DisplayNameText);
			light.ReflectorOn = true;
			light.LightType = MyLightType.SPOTLIGHT;
			light.Falloff = LightFalloffSync;
			light.GlossFactor = 0f;
			light.ReflectorGlossFactor = 1f;
			light.ReflectorFalloff = 0.5f;
			light.GlareOn = light.LightOn;
			light.GlareType = MyGlareTypeEnum.Directional;
			base.Render.NeedsDrawFromParent = true;
		}

		public override void InitComponents()
		{
			base.Render = new MyRenderComponentHeatVent();
			base.InitComponents();
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			MyChokeSubpart myChokeSubpart = new MyChokeSubpart();
			if (m_subpartRotations != null)
			{
				MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation[] subpartRotations = m_subpartRotations;
				foreach (MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation subpartRotation in subpartRotations)
				{
					if (subpartRotation.SubpartName.ToLower() == subpartDummy.Name.ToLower())
					{
						myChokeSubpart.SubpartRotation = subpartRotation;
						break;
					}
				}
			}
			return myChokeSubpart;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyHeatVentBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlColor<MyHeatVentBlock>("ColorMin", MySpaceTexts.BlockPropertyTitle_LightColorAtMinimalLoad)
				{
					Getter = (MyHeatVentBlock x) => x.m_colorMinimal,
					Setter = delegate(MyHeatVentBlock x, Color v)
					{
						x.m_colorMinimal.Value = v;
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlColor<MyHeatVentBlock>("ColorMax", MySpaceTexts.BlockPropertyTitle_LightColorAtMaximalLoad)
				{
					Getter = (MyHeatVentBlock x) => x.m_colorMaximal,
					Setter = delegate(MyHeatVentBlock x, Color v)
					{
						x.m_colorMaximal.Value = v;
					}
				});
				MyTerminalControlSlider<MyHeatVentBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyHeatVentBlock>("Radius", MySpaceTexts.BlockPropertyTitle_LightRadius, MySpaceTexts.BlockPropertyDescription_LightRadius);
				myTerminalControlSlider.SetLimits((MyHeatVentBlock x) => x.m_lightingLogic.RadiusBounds.Min, (MyHeatVentBlock x) => x.m_lightingLogic.RadiusBounds.Max);
				myTerminalControlSlider.DefaultValueGetter = (MyHeatVentBlock x) => x.m_lightingLogic.RadiusBounds.Default;
				myTerminalControlSlider.Getter = (MyHeatVentBlock x) => x.m_lightRadiusDefault.Value;
				myTerminalControlSlider.Setter = delegate(MyHeatVentBlock x, float v)
				{
					x.m_lightRadiusDefault.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyHeatVentBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightRadiusDefault.Value, 1)).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyHeatVentBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MyHeatVentBlock>("Falloff", MySpaceTexts.BlockPropertyTitle_LightFalloff, MySpaceTexts.BlockPropertyDescription_LightFalloff);
				myTerminalControlSlider2.SetLimits((MyHeatVentBlock x) => x.m_lightingLogic.FalloffBounds.Min, (MyHeatVentBlock x) => x.m_lightingLogic.FalloffBounds.Max);
				myTerminalControlSlider2.DefaultValueGetter = (MyHeatVentBlock x) => x.m_lightingLogic.FalloffBounds.Default;
				myTerminalControlSlider2.Getter = (MyHeatVentBlock x) => x.m_lightFalloffDefault.Value;
				myTerminalControlSlider2.Setter = delegate(MyHeatVentBlock x, float v)
				{
					x.m_lightFalloffDefault.Value = v;
				};
				myTerminalControlSlider2.Writer = delegate(MyHeatVentBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightFalloffDefault.Value, 1));
				};
				myTerminalControlSlider2.Visible = (MyHeatVentBlock x) => true;
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyHeatVentBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MyHeatVentBlock>("Intensity", MySpaceTexts.BlockPropertyTitle_LightIntensity, MySpaceTexts.BlockPropertyDescription_LightIntensity);
				myTerminalControlSlider3.SetLimits((MyHeatVentBlock x) => x.m_lightingLogic.IntensityBounds.Min, (MyHeatVentBlock x) => x.m_lightingLogic.IntensityBounds.Max);
				myTerminalControlSlider3.DefaultValueGetter = (MyHeatVentBlock x) => x.m_lightingLogic.IntensityBounds.Default;
				myTerminalControlSlider3.Getter = (MyHeatVentBlock x) => x.m_lightIntensityDefault.Value;
				myTerminalControlSlider3.Setter = delegate(MyHeatVentBlock x, float v)
				{
					x.m_lightIntensityDefault.Value = v;
				};
				myTerminalControlSlider3.Writer = delegate(MyHeatVentBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightIntensityDefault.Value, 1));
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MyHeatVentBlock> myTerminalControlSlider4 = new MyTerminalControlSlider<MyHeatVentBlock>("Offset", MySpaceTexts.BlockPropertyTitle_LightOffset, MySpaceTexts.BlockPropertyDescription_LightOffset);
				myTerminalControlSlider4.SetLimits((MyHeatVentBlock x) => x.m_lightingLogic.OffsetBounds.Min, (MyHeatVentBlock x) => x.m_lightingLogic.OffsetBounds.Max);
				myTerminalControlSlider4.DefaultValueGetter = (MyHeatVentBlock x) => x.m_lightingLogic.OffsetBounds.Default;
				myTerminalControlSlider4.Getter = (MyHeatVentBlock x) => x.m_lightOffsetDefault.Value;
				myTerminalControlSlider4.Setter = delegate(MyHeatVentBlock x, float v)
				{
					x.m_lightOffsetDefault.Value = v;
				};
				myTerminalControlSlider4.Writer = delegate(MyHeatVentBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightOffsetDefault.Value, 1));
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MyHeatVentBlock> myTerminalControlSlider5 = new MyTerminalControlSlider<MyHeatVentBlock>("PowerDependency", MySpaceTexts.BlockPropertyTitle_PowerDependency, MySpaceTexts.Blank);
				myTerminalControlSlider5.SetLimits((MyHeatVentBlock x) => 0f, (MyHeatVentBlock x) => 100f);
				myTerminalControlSlider5.DefaultValue = 0f;
				myTerminalControlSlider5.Getter = (MyHeatVentBlock x) => (float)x.m_powerDependency * 100f;
				myTerminalControlSlider5.Setter = delegate(MyHeatVentBlock x, float v)
				{
					x.m_powerDependency.Value = v / 100f;
				};
				myTerminalControlSlider5.Writer = delegate(MyHeatVentBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat((float)x.m_powerDependency * 100f, 1)).Append(" %");
				};
				myTerminalControlSlider5.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation();
			RecalculateEmmitedLight(m_hardRecount);
		}

		public override void UpdateAfterSimulation100()
		{
			m_lightingLogic.UpdateLightProperties();
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			m_lightingLogic.UpdateOnceBeforeFrame();
		}

		private void UpdateEnabled(bool state)
		{
			if (m_lightingLogic.Lights == null)
			{
				return;
			}
			bool flag = state && base.CubeGrid.Projector == null;
			foreach (MyLight light in m_lightingLogic.Lights)
			{
				light.ReflectorOn = flag;
				light.LightOn = flag;
				light.GlareOn = flag;
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			m_lightingLogic?.UpdateVisual();
			foreach (MyChokeSubpart value in base.Subparts.Values)
			{
				value.Render.SetSpeed(1f);
			}
		}

		private void LightUpdateEnabled(bool state)
		{
			if (m_lightingLogic.Lights == null)
			{
				return;
			}
			bool flag = state && base.CubeGrid.Projector == null;
			foreach (MyLight light in m_lightingLogic.Lights)
			{
				light.ReflectorOn = flag;
				light.LightOn = flag;
				light.GlareOn = flag;
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			if (base.Render.GetRenderObjectID() != uint.MaxValue)
			{
				RecalculateEmmitedLight(forceRecalc: true);
			}
		}

		private void OnIsPreviewChanged(bool isPreview)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.MarkedForClose)
				{
					MyCubeGrid cubeGrid = base.CubeGrid;
					if (cubeGrid != null && !cubeGrid.MarkedForClose)
					{
						UpdateVisual();
					}
				}
			}, "LightPreviewUpdate");
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void Closing()
		{
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Remove(cubeGrid.IsPreviewChanged, new Action<bool>(OnIsPreviewChanged));
			}
			if (base.Render.GetRenderObjectID() != uint.MaxValue)
			{
				MyRenderProxy.UpdateModelProperties(base.Render.GetRenderObjectID(), m_emissiveMaterialName, (RenderFlags)0, (RenderFlags)0, ColorCurrent, 0f);
			}
			foreach (MyLight light in m_lightingLogic.Lights)
			{
				light.LightOn = false;
			}
			m_lightingLogic.CloseLights();
			m_lightingLogic.OnPropertiesChanged -= base.RaisePropertiesChanged;
			m_lightingLogic.OnUpdateEnabled -= LightUpdateEnabled;
			m_lightingLogic.OnInitLight -= InitLight;
			base.Closing();
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			if (m_lightingLogic != null)
			{
				m_lightingLogic.IsEmissiveMaterialDirty = true;
			}
			UpdateVisual();
			m_hardRecount = true;
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			if (m_lightingLogic != null)
			{
				m_lightingLogic.IsEmissiveMaterialDirty = true;
			}
			UpdateVisual();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (Enabled)
			{
				OnStartWorking();
			}
			m_lightingLogic?.OnAddedToScene();
		}

		private void CubeBlock_OnWorkingChanged(MyCubeBlock block)
		{
			if (m_lightingLogic != null)
			{
				m_lightingLogic.IsPositionDirty = true;
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			if (m_lightingLogic != null)
			{
				m_lightingLogic.IsPositionDirty = true;
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_lightingLogic?.OnModelChange();
			m_hardRecount = true;
		}

		private float GetCurrentPowerRatio()
		{
			if (base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				float num = base.CubeGrid.GridSystems.ResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId, base.CubeGrid);
				float num2 = base.CubeGrid.GridSystems.ResourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId, base.CubeGrid);
				return (float)Math.Round((num > 0f) ? (num2 / num) : 0f, 3);
			}
			return 0f;
		}

		protected override bool CheckIsWorking()
		{
			return Enabled;
		}

		private void RecalculateEmmitedLight(bool forceRecalc = false)
		{
			if (Sync.IsDedicated)
			{
				return;
			}
			float currentPowerRatio = GetCurrentPowerRatio();
			forceRecalc = forceRecalc || (!base.IsFunctional && m_lightingLogic.Lights[0].LightOn);
			if (!m_inited || (!forceRecalc && currentPowerRatio == m_lastUsedGridPowerRatio))
			{
				return;
			}
			m_hardRecount = false;
			float num = 0f;
			if (Enabled && base.IsBuilt && base.IsFunctional)
			{
				m_lastUsedGridPowerRatio = currentPowerRatio;
				num = m_lastUsedGridPowerRatio / (float)m_powerDependency;
				if (object.Equals(num, float.NaN))
				{
					num = 0f;
				}
				if (num > 1f)
				{
					num = 1f;
				}
			}
			else
			{
				if (m_lastUsedGridPowerRatio != 0f)
				{
					OnStopWorking();
				}
				m_lastUsedGridPowerRatio = 0f;
			}
			m_lightingLogic.CurrentLightPower = num;
			if (num == 0f)
			{
				foreach (MyLight light in m_lightingLogic.Lights)
				{
					light.LightOn = false;
				}
			}
			else
			{
				ColorCurrent = BlendColors(ColorMaximal, ColorMinimal, num);
				m_lightingLogic.Falloff = BlockDefinition.LightFalloffBounds.Clamp(m_lightFalloffDefault.Value * num);
				m_lightingLogic.Intensity = BlockDefinition.LightIntensityBounds.Clamp(m_lightIntensityDefault.Value * num);
				m_lightingLogic.Radius = BlockDefinition.LightRadiusBounds.Clamp(m_lightRadiusDefault.Value);
				m_lightingLogic.Offset = BlockDefinition.LightOffsetBounds.Clamp(m_lightOffsetDefault.Value);
				foreach (MyLight light2 in m_lightingLogic.Lights)
				{
					light2.LightOn = true;
					light2.Intensity = m_lightingLogic.Intensity;
					light2.Falloff = m_lightingLogic.Falloff;
					light2.PointLightOffset = m_lightingLogic.Offset;
					light2.Color = ColorCurrent;
				}
			}
			if (base.Render.GetRenderObjectID() == uint.MaxValue)
			{
				return;
			}
			MyRenderProxy.UpdateModelProperties(base.Render.GetRenderObjectID(), m_emissiveMaterialName, (RenderFlags)0, (RenderFlags)0, (num == 0f) ? Color.DarkGray : ColorCurrent, (num == 0f) ? 0f : m_lightingLogic.Intensity);
			foreach (MyChokeSubpart value in base.Subparts.Values)
			{
				value.Render.SetDesiredPosition(num);
			}
		}

		private Color BlendColors(Color colorMaximal, Color colorMinimal, float amount)
		{
			byte r = (byte)((float)(int)colorMaximal.R * amount + (float)(int)colorMinimal.R * (1f - amount));
			byte g = (byte)((float)(int)colorMaximal.G * amount + (float)(int)colorMinimal.G * (1f - amount));
			byte b = (byte)((float)(int)colorMaximal.B * amount + (float)(int)colorMinimal.B * (1f - amount));
			return new Color(r, g, b);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_HeatVentBlock obj = (MyObjectBuilder_HeatVentBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.PowerDependency = m_powerDependency.Value;
			obj.ColorMaximalPower = ColorMaximal;
			obj.ColorMinimalPower = ColorMinimal;
			obj.RequiredPowerInput = m_requiredPower;
			obj.SubpartRotations = m_subpartRotations;
			obj.Falloff = LightFalloffSync.Value;
			obj.Intensity = IntensitySync.Value;
			obj.Offset = LightOffsetSync.Value;
			obj.Radius = LightRadiusSync.Value;
			return obj;
		}
	}
}
