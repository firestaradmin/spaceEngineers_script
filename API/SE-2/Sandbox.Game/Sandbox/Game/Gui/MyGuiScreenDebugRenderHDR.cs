<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "HDR")]
	internal class MyGuiScreenDebugRenderHDR : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderHDR()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("HDR", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddCheckBox("Enable", MyRenderProxy.Settings.HDREnabled, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.HDREnabled = x.IsChecked;
				if (MyRenderProxy.Settings.HDREnabled)
				{
					MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation = true;
					MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = 1f;
					MySector.PlanetProperties.AtmosphereIntensityMultiplier = 35f;
					MySector.PlanetProperties.CloudsIntensityMultiplier = 60f;
					MySector.SunProperties.SunIntensity = 150f;
					MyPostprocessSettingsWrapper.MarkDirty();
				}
				else
				{
					MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation = false;
					MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = 0.5f;
					MySector.SunProperties.SunIntensity = 5f;
					MySector.PlanetProperties.AtmosphereIntensityMultiplier = 1f;
					MySector.PlanetProperties.CloudsIntensityMultiplier = 1f;
					MyPostprocessSettingsWrapper.MarkDirty();
				}
				MyRenderProxy.SetSettingsDirty();
			});
			AddCheckBox("64bit target", MyRenderProxy.Settings.User.HqTarget, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.User.HqTarget = x.IsChecked;
				MyRenderProxy.SetSettingsDirty();
			});
<<<<<<< HEAD
			if (MySession.Static != null)
			{
				AddSlider("Time of day", 0f, MySession.Static.Settings.SunRotationIntervalMinutes, () => MyTimeOfDayHelper.TimeOfDay, MyTimeOfDayHelper.UpdateTimeOfDay);
			}
=======
			AddSlider("Time of day", 0f, MySession.Static.Settings.SunRotationIntervalMinutes, () => MyTimeOfDayHelper.TimeOfDay, MyTimeOfDayHelper.UpdateTimeOfDay);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AddCheckBox("Unlock PE", checkedState: false, delegate
			{
				MyGameService.GetAchievement("Promoted_engineer", null, 0f).Unlock();
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderHDR";
		}
	}
}
