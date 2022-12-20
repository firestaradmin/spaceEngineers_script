using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyLightingBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyLightingBlock)
	})]
<<<<<<< HEAD
	public abstract class MyLightingBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, IMyParallelUpdateable, IMyLightingLogicSync
=======
	public abstract class MyLightingBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, IMyParallelUpdateable
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		protected class _003CBlinkIntervalSecondsSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CBlinkIntervalSecondsSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkIntervalSecondsSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).BlinkIntervalSecondsSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkIntervalSecondsSync_003Ek__BackingField;
=======
				ISyncType blinkIntervalSeconds;
				ISyncType result = (blinkIntervalSeconds = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_blinkIntervalSeconds = (Sync<float, SyncDirection.BothWays>)blinkIntervalSeconds;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CBlinkLengthSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CBlinkLengthSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkLengthSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).BlinkLengthSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkLengthSync_003Ek__BackingField;
=======
				ISyncType blinkLength;
				ISyncType result = (blinkLength = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_blinkLength = (Sync<float, SyncDirection.BothWays>)blinkLength;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CBlinkOffsetSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CBlinkOffsetSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkOffsetSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).BlinkOffsetSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkOffsetSync_003Ek__BackingField;
=======
				ISyncType blinkOffset;
				ISyncType result = (blinkOffset = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_blinkOffset = (Sync<float, SyncDirection.BothWays>)blinkOffset;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CIntensitySync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CIntensitySync_003Ek__BackingField;
				ISyncType result = (_003CIntensitySync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).IntensitySync = (Sync<float, SyncDirection.BothWays>)_003CIntensitySync_003Ek__BackingField;
=======
				ISyncType intensity;
				ISyncType result = (intensity = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_intensity = (Sync<float, SyncDirection.BothWays>)intensity;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CLightColorSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CLightColorSync_003Ek__BackingField;
				ISyncType result = (_003CLightColorSync_003Ek__BackingField = new Sync<Color, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).LightColorSync = (Sync<Color, SyncDirection.BothWays>)_003CLightColorSync_003Ek__BackingField;
=======
				ISyncType lightColor;
				ISyncType result = (lightColor = new Sync<Color, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_lightColor = (Sync<Color, SyncDirection.BothWays>)lightColor;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CLightRadiusSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CLightRadiusSync_003Ek__BackingField;
				ISyncType result = (_003CLightRadiusSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).LightRadiusSync = (Sync<float, SyncDirection.BothWays>)_003CLightRadiusSync_003Ek__BackingField;
=======
				ISyncType lightRadius;
				ISyncType result = (lightRadius = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_lightRadius = (Sync<float, SyncDirection.BothWays>)lightRadius;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CLightFalloffSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CLightFalloffSync_003Ek__BackingField;
				ISyncType result = (_003CLightFalloffSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).LightFalloffSync = (Sync<float, SyncDirection.BothWays>)_003CLightFalloffSync_003Ek__BackingField;
=======
				ISyncType lightFalloff;
				ISyncType result = (lightFalloff = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_lightFalloff = (Sync<float, SyncDirection.BothWays>)lightFalloff;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		protected class _003CLightOffsetSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
<<<<<<< HEAD
				ISyncType _003CLightOffsetSync_003Ek__BackingField;
				ISyncType result = (_003CLightOffsetSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).LightOffsetSync = (Sync<float, SyncDirection.BothWays>)_003CLightOffsetSync_003Ek__BackingField;
=======
				ISyncType lightOffset;
				ISyncType result = (lightOffset = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyLightingBlock)P_0).m_lightOffset = (Sync<float, SyncDirection.BothWays>)lightOffset;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}
		}

		private const int NUM_DECIMALS = 1;

		protected MyLightingLogic m_lightingLogic;

		protected bool HasSubPartLights;

		private const int MaxLightUpdateDistance = 5000;

		private MyParallelUpdateFlag m_parallelFlag;

		private MyParallelUpdateFlag m_parallelFlag;

		public new MyLightingBlockDefinition BlockDefinition => (MyLightingBlockDefinition)base.BlockDefinition;

		public Vector4 LightColorDef => (IsLargeLight ? new Color(255, 255, 222) : new Color(206, 235, 255)).ToVector4();

		public bool IsLargeLight { get; private set; }

		protected abstract bool SupportsFalloff { get; }

		public Sync<float, SyncDirection.BothWays> BlinkIntervalSecondsSync { get; }

		public Sync<float, SyncDirection.BothWays> BlinkLengthSync { get; }

		public Sync<float, SyncDirection.BothWays> BlinkOffsetSync { get; }

		public Sync<float, SyncDirection.BothWays> IntensitySync { get; }

		public Sync<Color, SyncDirection.BothWays> LightColorSync { get; }

		public Sync<float, SyncDirection.BothWays> LightRadiusSync { get; }

		public Sync<float, SyncDirection.BothWays> LightFalloffSync { get; }

<<<<<<< HEAD
		public Sync<float, SyncDirection.BothWays> LightOffsetSync { get; }

		protected virtual bool NeedPerFrameUpdate => m_lightingLogic.NeedPerFrameUpdate;

		public float CurrentLightPower => m_lightingLogic.CurrentLightPower;

		public MyBounds ReflectorRadiusBounds => m_lightingLogic.ReflectorRadiusBounds;
=======
		public bool IsLargeLight { get; private set; }

		public abstract bool IsReflector { get; }

		protected abstract bool SupportsFalloff { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public Color Color
		{
			get
			{
				return m_lightingLogic.Color;
			}
			set
			{
				m_lightingLogic.Color = value;
			}
		}

		public float Radius
		{
			get
			{
				return m_lightingLogic.Radius;
			}
			set
			{
				m_lightingLogic.Radius = value;
			}
		}

		public float ReflectorRadius
		{
			get
			{
				return m_lightingLogic.ReflectorRadius;
			}
			set
			{
				m_lightingLogic.ReflectorRadius = value;
			}
		}

		public float BlinkLength
		{
			get
			{
				return m_lightingLogic.BlinkLength;
			}
			set
			{
				m_lightingLogic.BlinkLength = value;
			}
		}

		public float BlinkOffset
		{
			get
			{
				return m_lightingLogic.BlinkOffset;
			}
			set
			{
				m_lightingLogic.BlinkOffset = value;
			}
		}

		public float BlinkIntervalSeconds
		{
			get
			{
				return m_lightingLogic.BlinkIntervalSeconds;
			}
			set
			{
				m_lightingLogic.BlinkIntervalSeconds = value;
			}
		}

		public virtual float Falloff
		{
			get
			{
				return m_lightingLogic.Falloff;
			}
			set
			{
				m_lightingLogic.Falloff = value;
			}
		}

		public float Intensity
		{
			get
			{
				return m_lightingLogic.Intensity;
			}
			set
			{
				m_lightingLogic.Intensity = value;
			}
		}

		public float Offset
		{
			get
			{
				return m_lightingLogic.Offset;
			}
			set
			{
<<<<<<< HEAD
				m_lightingLogic.Offset = value;
=======
				if ((float)m_lightOffset != value)
				{
					m_lightOffset.Value = value;
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		protected virtual bool NeedPerFrameUpdate => false | ((float)m_blinkIntervalSeconds > 0f && base.IsWorking) | (GetNewLightPower() != CurrentLightPower);

		public float CurrentLightPower
		{
			get
			{
				return m_currentLightPower;
			}
			set
			{
				if (m_currentLightPower != value)
				{
					m_currentLightPower = value;
					m_emissiveMaterialDirty = true;
				}
			}
		}

		public Color BulbColor
		{
			get
			{
				return m_bulbColor;
			}
			set
			{
				if (m_bulbColor != value)
				{
					m_bulbColor = value;
					m_emissiveMaterialDirty = true;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.ReflectorRadius => ReflectorRadius;

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkLenght => BlinkLength;

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.Radius
		{
			get
			{
				if (!m_lightingLogic.IsReflector)
				{
					return Radius;
				}
				return ReflectorRadius;
			}
			set
			{
				value = (m_lightingLogic.IsReflector ? MathHelper.Clamp(value, m_lightingLogic.ReflectorRadiusBounds.Min, m_lightingLogic.ReflectorRadiusBounds.Max) : MathHelper.Clamp(value, m_lightingLogic.RadiusBounds.Min, m_lightingLogic.RadiusBounds.Max));
				LightRadiusSync.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.Intensity
		{
			get
			{
				return IntensitySync;
			}
			set
			{
				value = MathHelper.Clamp(value, m_lightingLogic.IntensityBounds.Min, m_lightingLogic.IntensityBounds.Max);
				IntensitySync.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.Falloff
		{
			get
			{
				return LightFalloffSync;
			}
			set
			{
				value = MathHelper.Clamp(value, m_lightingLogic.FalloffBounds.Min, m_lightingLogic.FalloffBounds.Max);
				LightFalloffSync.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkIntervalSeconds
		{
			get
			{
				return BlinkIntervalSeconds;
			}
			set
			{
				value = MathHelper.Clamp(value, m_lightingLogic.BlinkIntervalSecondsBounds.Min, m_lightingLogic.BlinkIntervalSecondsBounds.Max);
				BlinkIntervalSeconds = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkLength
		{
			get
			{
				return BlinkLength;
			}
			set
			{
				value = MathHelper.Clamp(value, m_lightingLogic.BlinkLengthBounds.Min, m_lightingLogic.BlinkLengthBounds.Max);
				BlinkLength = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkOffset
		{
			get
			{
				return BlinkOffset;
			}
			set
			{
				value = MathHelper.Clamp(value, m_lightingLogic.BlinkOffsetBounds.Min, m_lightingLogic.BlinkOffsetBounds.Max);
				BlinkOffset = value;
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyLightingBlock.Color
		{
			get
			{
				return Color;
			}
			set
			{
				LightColorSync.Value = value;
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyParallelUpdateFlags UpdateFlags => m_parallelFlag.GetFlags(this);

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLightingBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlColor<MyLightingBlock>("Color", MySpaceTexts.BlockPropertyTitle_LightColor)
				{
					Getter = (MyLightingBlock x) => x.Color,
					Setter = delegate(MyLightingBlock x, Color v)
					{
						x.LightColorSync.Value = v;
					}
				});
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyLightingBlock>("Radius", MySpaceTexts.BlockPropertyTitle_LightRadius, MySpaceTexts.BlockPropertyDescription_LightRadius);
<<<<<<< HEAD
				myTerminalControlSlider.SetLimits((MyLightingBlock x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.RadiusBounds.Min : x.m_lightingLogic.ReflectorRadiusBounds.Min, (MyLightingBlock x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.RadiusBounds.Max : x.m_lightingLogic.ReflectorRadiusBounds.Max);
				myTerminalControlSlider.DefaultValueGetter = (MyLightingBlock x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.RadiusBounds.Default : x.m_lightingLogic.ReflectorRadiusBounds.Default;
				myTerminalControlSlider.Getter = (MyLightingBlock x) => (!x.m_lightingLogic.IsReflector) ? x.Radius : x.ReflectorRadius;
=======
				myTerminalControlSlider.SetLimits((MyLightingBlock x) => (!x.IsReflector) ? x.RadiusBounds.Min : x.ReflectorRadiusBounds.Min, (MyLightingBlock x) => (!x.IsReflector) ? x.RadiusBounds.Max : x.ReflectorRadiusBounds.Max);
				myTerminalControlSlider.DefaultValueGetter = (MyLightingBlock x) => (!x.IsReflector) ? x.RadiusBounds.Default : x.ReflectorRadiusBounds.Default;
				myTerminalControlSlider.Getter = (MyLightingBlock x) => (!x.IsReflector) ? x.Radius : x.ReflectorRadius;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider.Setter = delegate(MyLightingBlock x, float v)
				{
					x.LightRadiusSync.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.IsReflector ? x.ReflectorRadius : x.Radius, 1)).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MyLightingBlock>("Falloff", MySpaceTexts.BlockPropertyTitle_LightFalloff, MySpaceTexts.BlockPropertyDescription_LightFalloff);
<<<<<<< HEAD
				myTerminalControlSlider2.SetLimits((MyLightingBlock x) => x.m_lightingLogic.FalloffBounds.Min, (MyLightingBlock x) => x.m_lightingLogic.FalloffBounds.Max);
				myTerminalControlSlider2.DefaultValueGetter = (MyLightingBlock x) => x.m_lightingLogic.FalloffBounds.Default;
=======
				myTerminalControlSlider2.SetLimits((MyLightingBlock x) => x.FalloffBounds.Min, (MyLightingBlock x) => x.FalloffBounds.Max);
				myTerminalControlSlider2.DefaultValueGetter = (MyLightingBlock x) => x.FalloffBounds.Default;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider2.Getter = (MyLightingBlock x) => x.Falloff;
				myTerminalControlSlider2.Setter = delegate(MyLightingBlock x, float v)
				{
					x.LightFalloffSync.Value = v;
				};
				myTerminalControlSlider2.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.Falloff, 1));
				};
				myTerminalControlSlider2.Visible = (MyLightingBlock x) => x.SupportsFalloff;
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MyLightingBlock>("Intensity", MySpaceTexts.BlockPropertyTitle_LightIntensity, MySpaceTexts.BlockPropertyDescription_LightIntensity);
<<<<<<< HEAD
				myTerminalControlSlider3.SetLimits((MyLightingBlock x) => x.m_lightingLogic.IntensityBounds.Min, (MyLightingBlock x) => x.m_lightingLogic.IntensityBounds.Max);
				myTerminalControlSlider3.DefaultValueGetter = (MyLightingBlock x) => x.m_lightingLogic.IntensityBounds.Default;
=======
				myTerminalControlSlider3.SetLimits((MyLightingBlock x) => x.IntensityBounds.Min, (MyLightingBlock x) => x.IntensityBounds.Max);
				myTerminalControlSlider3.DefaultValueGetter = (MyLightingBlock x) => x.IntensityBounds.Default;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider3.Getter = (MyLightingBlock x) => x.Intensity;
				myTerminalControlSlider3.Setter = delegate(MyLightingBlock x, float v)
				{
					x.Intensity = v;
				};
				myTerminalControlSlider3.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.Intensity, 1));
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider4 = new MyTerminalControlSlider<MyLightingBlock>("Offset", MySpaceTexts.BlockPropertyTitle_LightOffset, MySpaceTexts.BlockPropertyDescription_LightOffset);
<<<<<<< HEAD
				myTerminalControlSlider4.SetLimits((MyLightingBlock x) => x.m_lightingLogic.OffsetBounds.Min, (MyLightingBlock x) => x.m_lightingLogic.OffsetBounds.Max);
				myTerminalControlSlider4.DefaultValueGetter = (MyLightingBlock x) => x.m_lightingLogic.OffsetBounds.Default;
=======
				myTerminalControlSlider4.SetLimits((MyLightingBlock x) => x.OffsetBounds.Min, (MyLightingBlock x) => x.OffsetBounds.Max);
				myTerminalControlSlider4.DefaultValueGetter = (MyLightingBlock x) => x.OffsetBounds.Default;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider4.Getter = (MyLightingBlock x) => x.Offset;
				myTerminalControlSlider4.Setter = delegate(MyLightingBlock x, float v)
				{
					x.LightOffsetSync.Value = v;
				};
				myTerminalControlSlider4.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.Offset, 1));
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider5 = new MyTerminalControlSlider<MyLightingBlock>("Blink Interval", MySpaceTexts.BlockPropertyTitle_LightBlinkInterval, MySpaceTexts.BlockPropertyDescription_LightBlinkInterval);
<<<<<<< HEAD
				myTerminalControlSlider5.SetLimits((MyLightingBlock x) => x.m_lightingLogic.BlinkIntervalSecondsBounds.Min, (MyLightingBlock x) => x.m_lightingLogic.BlinkIntervalSecondsBounds.Max);
				myTerminalControlSlider5.DefaultValueGetter = (MyLightingBlock x) => x.m_lightingLogic.BlinkIntervalSecondsBounds.Default;
=======
				myTerminalControlSlider5.SetLimits((MyLightingBlock x) => x.BlinkIntervalSecondsBounds.Min, (MyLightingBlock x) => x.BlinkIntervalSecondsBounds.Max);
				myTerminalControlSlider5.DefaultValueGetter = (MyLightingBlock x) => x.BlinkIntervalSecondsBounds.Default;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider5.Getter = (MyLightingBlock x) => x.BlinkIntervalSeconds;
				myTerminalControlSlider5.Setter = delegate(MyLightingBlock x, float v)
				{
					x.BlinkIntervalSeconds = v;
				};
				myTerminalControlSlider5.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkIntervalSeconds, 1)).Append(" s");
				};
				myTerminalControlSlider5.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider6 = new MyTerminalControlSlider<MyLightingBlock>("Blink Lenght", MySpaceTexts.BlockPropertyTitle_LightBlinkLenght, MySpaceTexts.BlockPropertyDescription_LightBlinkLenght, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
<<<<<<< HEAD
				myTerminalControlSlider6.SetLimits((MyLightingBlock x) => x.m_lightingLogic.BlinkLengthBounds.Min, (MyLightingBlock x) => x.m_lightingLogic.BlinkLengthBounds.Max);
				myTerminalControlSlider6.DefaultValueGetter = (MyLightingBlock x) => x.m_lightingLogic.BlinkLengthBounds.Default;
=======
				myTerminalControlSlider6.SetLimits((MyLightingBlock x) => x.BlinkLenghtBounds.Min, (MyLightingBlock x) => x.BlinkLenghtBounds.Max);
				myTerminalControlSlider6.DefaultValueGetter = (MyLightingBlock x) => x.BlinkLenghtBounds.Default;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider6.Getter = (MyLightingBlock x) => x.BlinkLength;
				myTerminalControlSlider6.Setter = delegate(MyLightingBlock x, float v)
				{
					x.BlinkLength = v;
				};
				myTerminalControlSlider6.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkLength, 1)).Append(" %");
				};
				myTerminalControlSlider6.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider6);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider7 = new MyTerminalControlSlider<MyLightingBlock>("Blink Offset", MySpaceTexts.BlockPropertyTitle_LightBlinkOffset, MySpaceTexts.BlockPropertyDescription_LightBlinkOffset, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
<<<<<<< HEAD
				myTerminalControlSlider7.SetLimits((MyLightingBlock x) => x.m_lightingLogic.BlinkOffsetBounds.Min, (MyLightingBlock x) => x.m_lightingLogic.BlinkOffsetBounds.Max);
				myTerminalControlSlider7.DefaultValueGetter = (MyLightingBlock x) => x.m_lightingLogic.BlinkOffsetBounds.Default;
=======
				myTerminalControlSlider7.SetLimits((MyLightingBlock x) => x.BlinkOffsetBounds.Min, (MyLightingBlock x) => x.BlinkOffsetBounds.Max);
				myTerminalControlSlider7.DefaultValueGetter = (MyLightingBlock x) => x.BlinkOffsetBounds.Default;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myTerminalControlSlider7.Getter = (MyLightingBlock x) => x.BlinkOffset;
				myTerminalControlSlider7.Setter = delegate(MyLightingBlock x, float v)
				{
					x.BlinkOffset = v;
				};
				myTerminalControlSlider7.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkOffset, 1)).Append(" %");
				};
				myTerminalControlSlider7.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider7);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			m_lightingLogic = new MyLightingLogic(this, BlockDefinition, this);
			m_lightingLogic.OnPropertiesChanged += base.RaisePropertiesChanged;
			m_lightingLogic.OnUpdateEnabled += UpdateEnabled;
			m_lightingLogic.OnIntensityUpdated += UpdateIntensity;
			m_lightingLogic.OnInitLight += InitLight;
			m_lightingLogic.OnEmissivityUpdated += UpdateEmissivity;
			m_lightingLogic.OnRadiusUpdated += UpdateRadius;
			CreateTerminalControls();
			base.Init(objectBuilder, cubeGrid);
			IsLargeLight = cubeGrid.GridSizeEnum == MyCubeSize.Large;
			MyObjectBuilder_LightingBlock myObjectBuilder_LightingBlock = (MyObjectBuilder_LightingBlock)objectBuilder;
<<<<<<< HEAD
			m_lightingLogic.Color = ((myObjectBuilder_LightingBlock.ColorAlpha == -1f) ? LightColorDef : new Vector4(myObjectBuilder_LightingBlock.ColorRed, myObjectBuilder_LightingBlock.ColorGreen, myObjectBuilder_LightingBlock.ColorBlue, myObjectBuilder_LightingBlock.ColorAlpha));
			LightRadiusSync.ValidateRange(m_lightingLogic.IsReflector ? BlockDefinition.LightReflectorRadius : BlockDefinition.LightRadius);
			LightFalloffSync.ValidateRange(BlockDefinition.LightFalloff);
			m_lightingLogic.Radius = m_lightingLogic.RadiusBounds.Clamp((myObjectBuilder_LightingBlock.Radius == -1f) ? m_lightingLogic.RadiusBounds.Default : myObjectBuilder_LightingBlock.Radius);
			m_lightingLogic.ReflectorRadius = m_lightingLogic.ReflectorRadiusBounds.Clamp((myObjectBuilder_LightingBlock.ReflectorRadius == -1f) ? m_lightingLogic.ReflectorRadiusBounds.Default : myObjectBuilder_LightingBlock.ReflectorRadius);
			m_lightingLogic.Falloff = m_lightingLogic.FalloffBounds.Clamp((myObjectBuilder_LightingBlock.Falloff == -1f) ? m_lightingLogic.FalloffBounds.Default : myObjectBuilder_LightingBlock.Falloff);
			BlinkIntervalSecondsSync.ValidateRange(BlockDefinition.BlinkIntervalSeconds);
			BlinkIntervalSecondsSync.SetLocalValue(m_lightingLogic.BlinkIntervalSecondsBounds.Clamp((myObjectBuilder_LightingBlock.BlinkIntervalSeconds == -1f) ? m_lightingLogic.BlinkIntervalSecondsBounds.Default : myObjectBuilder_LightingBlock.BlinkIntervalSeconds));
			BlinkLengthSync.ValidateRange(BlockDefinition.BlinkLenght);
			BlinkLengthSync.SetLocalValue(m_lightingLogic.BlinkLengthBounds.Clamp((myObjectBuilder_LightingBlock.BlinkLenght == -1f) ? m_lightingLogic.BlinkLengthBounds.Default : myObjectBuilder_LightingBlock.BlinkLenght));
			BlinkOffsetSync.ValidateRange(BlockDefinition.BlinkOffset);
			BlinkOffsetSync.SetLocalValue(m_lightingLogic.BlinkOffsetBounds.Clamp((myObjectBuilder_LightingBlock.BlinkOffset == -1f) ? m_lightingLogic.BlinkOffsetBounds.Default : myObjectBuilder_LightingBlock.BlinkOffset));
			IntensitySync.ValidateRange(BlockDefinition.LightIntensity);
			IntensitySync.SetLocalValue(m_lightingLogic.IntensityBounds.Clamp((myObjectBuilder_LightingBlock.Intensity == -1f) ? m_lightingLogic.IntensityBounds.Default : myObjectBuilder_LightingBlock.Intensity));
			LightOffsetSync.ValidateRange(BlockDefinition.LightOffset);
			LightOffsetSync.SetLocalValue(m_lightingLogic.OffsetBounds.Clamp((myObjectBuilder_LightingBlock.Offset == -1f) ? m_lightingLogic.OffsetBounds.Default : myObjectBuilder_LightingBlock.Offset));
			m_lightingLogic.Initialize();
=======
			m_color = ((myObjectBuilder_LightingBlock.ColorAlpha == -1f) ? LightColorDef : new Vector4(myObjectBuilder_LightingBlock.ColorRed, myObjectBuilder_LightingBlock.ColorGreen, myObjectBuilder_LightingBlock.ColorBlue, myObjectBuilder_LightingBlock.ColorAlpha));
			m_radius = RadiusBounds.Clamp((myObjectBuilder_LightingBlock.Radius == -1f) ? RadiusBounds.Default : myObjectBuilder_LightingBlock.Radius);
			m_reflectorRadius = ReflectorRadiusBounds.Clamp((myObjectBuilder_LightingBlock.ReflectorRadius == -1f) ? ReflectorRadiusBounds.Default : myObjectBuilder_LightingBlock.ReflectorRadius);
			m_falloff = FalloffBounds.Clamp((myObjectBuilder_LightingBlock.Falloff == -1f) ? FalloffBounds.Default : myObjectBuilder_LightingBlock.Falloff);
			m_blinkIntervalSeconds.SetLocalValue(BlinkIntervalSecondsBounds.Clamp((myObjectBuilder_LightingBlock.BlinkIntervalSeconds == -1f) ? BlinkIntervalSecondsBounds.Default : myObjectBuilder_LightingBlock.BlinkIntervalSeconds));
			m_blinkLength.SetLocalValue(BlinkLenghtBounds.Clamp((myObjectBuilder_LightingBlock.BlinkLenght == -1f) ? BlinkLenghtBounds.Default : myObjectBuilder_LightingBlock.BlinkLenght));
			m_blinkOffset.SetLocalValue(BlinkOffsetBounds.Clamp((myObjectBuilder_LightingBlock.BlinkOffset == -1f) ? BlinkOffsetBounds.Default : myObjectBuilder_LightingBlock.BlinkOffset));
			m_intensity.SetLocalValue(IntensityBounds.Clamp((myObjectBuilder_LightingBlock.Intensity == -1f) ? IntensityBounds.Default : myObjectBuilder_LightingBlock.Intensity));
			m_lightOffset.SetLocalValue(OffsetBounds.Clamp((myObjectBuilder_LightingBlock.Offset == -1f) ? OffsetBounds.Default : myObjectBuilder_LightingBlock.Offset));
			UpdateLightData();
			m_positionDirty = true;
			CreateLights();
			UpdateIntensity();
			UpdateLightPosition();
			UpdateLightBlink();
			UpdateEnabled();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += CubeBlock_OnWorkingChanged;
			MyCubeGrid cubeGrid2 = base.CubeGrid;
			cubeGrid2.IsPreviewChanged = (Action<bool>)Delegate.Combine(cubeGrid2.IsPreviewChanged, new Action<bool>(OnIsPreviewChanged));
<<<<<<< HEAD
=======
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
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected abstract void UpdateEnabled(bool state);

		protected abstract void UpdateIntensity();

		protected abstract void InitLight(MyLight light, Vector4 color, float radius, float falloff);

		protected virtual void UpdateRadius(float value)
		{
<<<<<<< HEAD
=======
			m_lightLocalData.Clear();
			HasSubPartLights = false;
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies)
			{
				string text = dummy.Key.ToLower();
				if (!text.Contains("subpart") && text.Contains("light"))
				{
					m_lightLocalData.Add(new LightLocalData
					{
						LocalMatrix = Matrix.Normalize(dummy.Value.Matrix),
						Subpart = null
					});
				}
			}
			foreach (KeyValuePair<string, MyEntitySubpart> subpart in base.Subparts)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy2 in subpart.Value.Model.Dummies)
				{
					if (dummy2.Key.ToLower().Contains("light"))
					{
						m_lightLocalData.Add(new LightLocalData
						{
							LocalMatrix = Matrix.Normalize(dummy2.Value.Matrix),
							Subpart = subpart.Value
						});
						HasSubPartLights = true;
					}
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected virtual void UpdateEmissivity(bool force = false)
		{
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

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_LightingBlock obj = (MyObjectBuilder_LightingBlock)base.GetObjectBuilderCubeBlock(copy);
			Vector4 vector = m_lightingLogic.Color.ToVector4();
			obj.ColorRed = vector.X;
			obj.ColorGreen = vector.Y;
			obj.ColorBlue = vector.Z;
			obj.ColorAlpha = vector.W;
			obj.Radius = m_lightingLogic.Radius;
			obj.ReflectorRadius = m_lightingLogic.ReflectorRadius;
			obj.Falloff = Falloff;
			obj.Intensity = IntensitySync;
			obj.BlinkIntervalSeconds = BlinkIntervalSecondsSync;
			obj.BlinkLenght = BlinkLengthSync;
			obj.BlinkOffset = BlinkOffsetSync;
			obj.Offset = LightOffsetSync;
			return obj;
		}

		protected override void Closing()
		{
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Remove(cubeGrid.IsPreviewChanged, new Action<bool>(OnIsPreviewChanged));
			}
<<<<<<< HEAD
			m_lightingLogic.CloseLights();
			m_lightingLogic.OnPropertiesChanged -= base.RaisePropertiesChanged;
			m_lightingLogic.OnUpdateEnabled -= UpdateEnabled;
			m_lightingLogic.OnIntensityUpdated -= UpdateIntensity;
			m_lightingLogic.OnInitLight -= InitLight;
			m_lightingLogic.OnEmissivityUpdated -= UpdateEmissivity;
			m_lightingLogic.OnRadiusUpdated -= UpdateRadius;
=======
			m_lights.Clear();
		}

		protected override void Closing()
		{
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Remove(cubeGrid.IsPreviewChanged, new Action<bool>(OnIsPreviewChanged));
			}
			CloseLights();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.Closing();
		}

		public MyLightingBlock()
		{
<<<<<<< HEAD
=======
			CreateTerminalControls();
			m_intensity.ValueChanged += delegate
			{
				IntensityChanged();
			};
			m_lightColor.ValueChanged += delegate
			{
				LightColorChanged();
			};
			m_lightRadius.ValueChanged += delegate
			{
				LightRadiusChanged();
			};
			m_lightFalloff.ValueChanged += delegate
			{
				LightFalloffChanged();
			};
			m_lightOffset.ValueChanged += delegate
			{
				LightOffsetChanged();
			};
		}

		private void IntensityChanged()
		{
			UpdateIntensity();
			UpdateLightProperties();
		}

		private void LightFalloffChanged()
		{
			Falloff = m_lightFalloff.Value;
		}

		private void LightOffsetChanged()
		{
			UpdateLightProperties();
		}

		protected virtual void UpdateRadius(float value)
		{
			if (IsReflector)
			{
				ReflectorRadius = value;
			}
			else
			{
				Radius = value;
			}
		}

		private void LightRadiusChanged()
		{
			UpdateRadius(m_lightRadius.Value);
		}

		private void LightColorChanged()
		{
			Color = m_lightColor.Value;
		}

		private float GetNewLightPower()
		{
			return MathHelper.Clamp(CurrentLightPower + (float)(base.IsWorking ? 1 : (-1)) * m_lightTurningOnSpeed, 0f, 1f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
<<<<<<< HEAD
			m_lightingLogic.IsEmissiveMaterialDirty = true;
=======
			m_emissiveMaterialDirty = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_parallelFlag.Enable(this);
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
<<<<<<< HEAD
			m_lightingLogic.IsEmissiveMaterialDirty = true;
=======
			m_emissiveMaterialDirty = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_parallelFlag.Enable(this);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			m_lightingLogic.UpdateOnceBeforeFrame();
			if (m_lightingLogic.NeedPerFrameUpdate)
			{
				m_parallelFlag.Enable(this);
			}
<<<<<<< HEAD
		}

		/// <inheritdoc />
		public virtual void UpdateAfterSimulationParallel()
		{
			if (!((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0))
			{
				m_lightingLogic.UpdateAfterSimulation();
			}
		}

		public override void UpdateAfterSimulation100()
=======
			UpdateParents();
			UpdateLightProperties();
			if (NeedPerFrameUpdate)
			{
				m_parallelFlag.Enable(this);
			}
		}

		public virtual void UpdateAfterSimulationParallel()
		{
			if ((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0)
			{
				return;
			}
			uint parentCullObject = base.CubeGrid.Render.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in m_lights)
			{
				light.ParentID = parentCullObject;
			}
			float newLightPower = GetNewLightPower();
			if (newLightPower != CurrentLightPower)
			{
				CurrentLightPower = newLightPower;
				UpdateIntensity();
			}
			UpdateLightBlink();
			UpdateEnabled();
			UpdateLightPosition();
			UpdateLightProperties();
			UpdateEmissivity();
			UpdateEmissiveMaterial();
		}

		public override void UpdateAfterSimulation100()
		{
			if ((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0)
			{
				m_parallelFlag.Disable(this);
				return;
			}
			bool needPerFrameUpdate = NeedPerFrameUpdate;
			m_parallelFlag.Set(this, needPerFrameUpdate);
			UpdateLightProperties();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			uint parentCullObject = base.CubeGrid.Render.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in m_lights)
			{
				light.ParentID = parentCullObject;
			}
			UpdateLightPosition();
			UpdateLightProperties();
			UpdateEmissivity(force: true);
		}

		private void UpdateEnabled()
		{
			UpdateEnabled(CurrentLightPower * Intensity > 0f && m_blinkOn && !base.IsPreview && !base.CubeGrid.IsPreview);
		}

		protected abstract void UpdateEnabled(bool state);

		protected abstract void UpdateIntensity();

		private void UpdateLightBlink()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if ((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0)
			{
<<<<<<< HEAD
				m_parallelFlag.Disable(this);
				return;
=======
				ulong num = (ulong)((float)m_blinkIntervalSeconds * 1000f);
				float num2 = (float)num * (float)m_blinkOffset * 0.01f;
				ulong num3 = (ulong)(MySession.Static.ElapsedGameTime.TotalMilliseconds - (double)num2) % num;
				ulong num4 = (ulong)((float)num * (float)m_blinkLength * 0.01f);
				m_blinkOn = num4 > num3;
			}
			else
			{
				m_blinkOn = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			bool needPerFrameUpdate = m_lightingLogic.NeedPerFrameUpdate;
			m_parallelFlag.Set(this, needPerFrameUpdate);
			m_lightingLogic.UpdateLightProperties();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			m_lightingLogic.OnAddedToScene();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void CubeBlock_OnWorkingChanged(MyCubeBlock block)
		{
<<<<<<< HEAD
			m_lightingLogic.IsPositionDirty = true;
=======
			m_positionDirty = true;
		}

		protected Color ComputeBulbColor()
		{
			float num = IntensityBounds.Normalize(Intensity);
			float num2 = 0.125f + num * 0.25f;
			return new Color((float)(int)Color.R * 0.5f + num2, (float)(int)Color.G * 0.5f + num2, (float)(int)Color.B * 0.5f + num2);
		}

		private void UpdateLightProperties()
		{
			foreach (MyLight light in m_lights)
			{
				light.Range = m_radius;
				light.ReflectorRange = m_reflectorRadius;
				light.Color = m_color;
				light.ReflectorColor = m_color;
				light.Falloff = m_falloff;
				light.PointLightOffset = Offset;
				light.UpdateLight();
			}
		}

		private void UpdateLightPosition()
		{
			if (m_lights == null || m_lights.Count == 0 || !m_positionDirty)
			{
				return;
			}
			m_positionDirty = false;
			_ = (MatrixD)base.PositionComp.LocalMatrixRef;
			for (int i = 0; i < m_lightLocalData.Count; i++)
			{
				MatrixD matrixD = base.PositionComp.LocalMatrixRef;
				if (m_lightLocalData[i].Subpart != null)
				{
					matrixD = m_lightLocalData[i].Subpart.PositionComp.LocalMatrixRef * matrixD;
				}
				MyLight myLight = m_lights[i];
				myLight.Position = Vector3D.Transform(m_lightLocalData[i].LocalMatrix.Translation, matrixD);
				myLight.ReflectorDirection = Vector3D.TransformNormal(m_lightLocalData[i].LocalMatrix.Forward, matrixD);
				myLight.ReflectorUp = Vector3D.TransformNormal(m_lightLocalData[i].LocalMatrix.Right, matrixD);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			m_lightingLogic.IsPositionDirty = true;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_lightingLogic.OnModelChange();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			m_lightingLogic.UpdateVisual();
		}

		/// <inheritdoc />
		public void UpdateBeforeSimulationParallel()
		{
<<<<<<< HEAD
=======
			if (!m_emissiveMaterialDirty)
			{
				return;
			}
			uint[] renderObjectIDs = base.Render.RenderObjectIDs;
			foreach (uint renderId in renderObjectIDs)
			{
				UpdateEmissiveMaterial(renderId);
			}
			foreach (LightLocalData lightLocalDatum in m_lightLocalData)
			{
				if (lightLocalDatum.Subpart != null && lightLocalDatum.Subpart.Render != null)
				{
					renderObjectIDs = lightLocalDatum.Subpart.Render.RenderObjectIDs;
					foreach (uint renderId2 in renderObjectIDs)
					{
						UpdateEmissiveMaterial(renderId2);
					}
				}
			}
			m_emissiveMaterialDirty = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void DisableUpdates()
		{
			base.DisableUpdates();
			m_parallelFlag.Disable(this);
		}

		public void UpdateBeforeSimulationParallel()
		{
		}

		public override void DisableUpdates()
		{
			base.DisableUpdates();
			m_parallelFlag.Disable(this);
		}
	}
}
