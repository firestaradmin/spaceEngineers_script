using System;
using System.Collections.Generic;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public class MyBrushGUIPropertyNumberSlider : IMyVoxelBrushGUIProperty
	{
		public Action ValueChanged;

		public float Value;

		public float ValueMin;

		public float ValueMax;

		public float ValueStep;

		public MyGuiControlLabel Label { get; set; }

		public MyGuiControlLabel LabelValue { get; set; }

		public MyGuiControlSlider SliderValue { get; set; }

		public MyBrushGUIPropertyNumberSlider(float value, float valueMin, float valueMax, float valueStep, MyVoxelBrushGUIPropertyOrder order, MyStringId labelText)
		{
			Vector2 position = new Vector2(-0.1f, -0.2f);
			Vector2 position2 = new Vector2(0.16f, -0.2f);
			Vector2 position3 = new Vector2(-0.1f, -0.173f);
			switch (order)
			{
			case MyVoxelBrushGUIPropertyOrder.Second:
				position.Y = -0.116f;
				position2.Y = -0.116f;
				position3.Y = -0.089f;
				break;
			case MyVoxelBrushGUIPropertyOrder.Third:
				position.Y = -0.032f;
				position2.Y = -0.032f;
				position3.Y = -0.005f;
				break;
			}
			Value = value;
			ValueMin = valueMin;
			ValueMax = valueMax;
			ValueStep = valueStep;
			Label = new MyGuiControlLabel
			{
				Position = position,
				TextEnum = labelText,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			LabelValue = new MyGuiControlLabel
			{
				Position = position2,
				Text = Value.ToString(),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			SliderValue = new MyGuiControlSlider
			{
				Position = position3,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			SliderValue.Size = new Vector2(0.263f, 0.1f);
			SliderValue.MaxValue = ValueMax;
			SliderValue.Value = Value;
			SliderValue.MinValue = ValueMin;
			MyGuiControlSlider sliderValue = SliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(Slider_ValueChanged));
		}

		private void Slider_ValueChanged(MyGuiControlSlider sender)
		{
			float num = 1f / ValueStep;
			float num2 = SliderValue.Value * num;
			Value = MathHelper.Clamp((float)(int)num2 / num, ValueMin, ValueMax);
			LabelValue.Text = Value.ToString();
			if (ValueChanged != null)
			{
				ValueChanged();
			}
		}

		public void AddControlsToList(List<MyGuiControlBase> list)
		{
			list.Add(Label);
			list.Add(LabelValue);
			list.Add(SliderValue);
		}
	}
}
