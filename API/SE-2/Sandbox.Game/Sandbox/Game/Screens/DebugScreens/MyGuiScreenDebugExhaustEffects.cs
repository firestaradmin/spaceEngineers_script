using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.FileSystem;
using VRage.Filesystem.FindFilesRegEx;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Exhaust Effects")]
	internal class MyGuiScreenDebugExhaustEffects : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_exhaustEffectsCombo;

		private MyGuiControlCombobox m_pipesCombo;

		private MyGuiControlTextbox m_dummyTextbox;

		private MyGuiControlCombobox m_effectsCombo;

		private MyGuiControlSlider m_effectIntensity;

		private MyGuiControlSlider m_powerToRadius;

		private MyGuiControlSlider m_powerToBirth;

		private MyGuiControlSlider m_powerToVelocity;

		private MyGuiControlSlider m_powerToColorIntensity;

		private MyGuiControlSlider m_powerToLife;

		private int m_selectedEffectIndex;

		private bool m_canUpdateValues = true;

		private MyExhaustEffectDefinition m_selectedEffectDefinition;

		private int m_selectedPipe;

		private MyObjectBuilder_ExhaustEffectDefinition.Pipe SelectedPipe
		{
			get
			{
				if (m_selectedEffectDefinition == null)
				{
					return null;
				}
				if (m_selectedPipe == -1)
				{
					return null;
				}
				return m_selectedEffectDefinition.ExhaustPipes[m_selectedPipe];
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugExhaustEffects";
		}

		public MyGuiScreenDebugExhaustEffects()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Exhaust effects", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_exhaustEffectsCombo = AddCombo();
			AddExhaustEffects(m_exhaustEffectsCombo);
			m_exhaustEffectsCombo.ItemSelected += exhaustEffect_ItemSelected;
			AddLabel("Pipe: ", Color.Yellow.ToVector4(), 1.2f);
			m_pipesCombo = AddCombo();
			m_pipesCombo.ItemSelected += pipe_ItemSelected;
			AddLabel("Dummy: ", Color.Yellow.ToVector4(), 1.2f);
			m_dummyTextbox = AddTextbox("emitter", null);
			m_dummyTextbox.FocusChanged += dummiesCombo_FocusChanged;
			m_dummyTextbox.EnterPressed += dummiesCombo_EnterPressed;
			m_dummyTextbox.TextChanged += dummyTextbox_TextChanged;
			AddLabel("Effect: ", Color.Yellow.ToVector4(), 1.2f);
			m_effectsCombo = AddCombo();
			AddEffects(m_effectsCombo);
			m_effectsCombo.ItemSelected += effect_ItemSelected;
			m_effectIntensity = AddSlider("Effect intensity", 0f, 0f, 1f);
			m_effectIntensity.ValueChanged = OnEffectValueChanged;
			m_currentPosition += new Vector2(0f, 0.015f);
			m_powerToRadius = AddSlider("Power to radius", 0f, 0f, 1f);
			m_powerToRadius.ValueChanged = OnEffectValueChanged;
			m_powerToBirth = AddSlider("Power to birth", 0f, 0f, 1f);
			m_powerToBirth.ValueChanged = OnEffectValueChanged;
			m_powerToVelocity = AddSlider("Power to velocity", 0f, 0f, 1f);
			m_powerToVelocity.ValueChanged = OnEffectValueChanged;
			m_powerToColorIntensity = AddSlider("Power to color intensity", 0f, 0f, 1f);
			m_powerToColorIntensity.ValueChanged = OnEffectValueChanged;
			m_powerToLife = AddSlider("Power to life", 0f, 0f, 1f);
			m_powerToLife.ValueChanged = OnEffectValueChanged;
			m_currentPosition += new Vector2(0f, 0.015f);
			AddButton(new StringBuilder("Save exhaust effects"), onClick_Save).VisualStyle = MyGuiControlButtonStyleEnum.Default;
			AddButton(new StringBuilder("Reload definitions"), onClick_Reload).VisualStyle = MyGuiControlButtonStyleEnum.Default;
			if (m_exhaustEffectsCombo.GetItemsCount() > 0)
			{
				m_exhaustEffectsCombo.SelectItemByIndex(m_selectedEffectIndex);
			}
		}

		private void dummyTextbox_TextChanged(MyGuiControlTextbox obj)
		{
			SelectedPipe.Dummy = m_dummyTextbox.Text;
		}

		private void dummiesCombo_EnterPressed(MyGuiControlTextbox obj)
		{
			base.CanHaveFocus = false;
		}

		private void dummiesCombo_FocusChanged(MyGuiControlBase arg1, bool arg2)
		{
			base.CanHaveFocus = true;
		}

		private void exhaustEffect_ItemSelected()
		{
			m_selectedEffectIndex = m_exhaustEffectsCombo.GetSelectedIndex();
<<<<<<< HEAD
			m_selectedEffectDefinition = (from x in MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>()
				where x.Id.SubtypeName == m_exhaustEffectsCombo.GetSelectedValue().ToString()
				select x).FirstOrDefault();
=======
			m_selectedEffectDefinition = Enumerable.FirstOrDefault<MyExhaustEffectDefinition>(Enumerable.Where<MyExhaustEffectDefinition>(MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>(), (Func<MyExhaustEffectDefinition, bool>)((MyExhaustEffectDefinition x) => x.Id.SubtypeName == m_exhaustEffectsCombo.GetSelectedValue().ToString())));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_selectedEffectDefinition != null)
			{
				AddPipes();
			}
		}

		private void AddPipes()
		{
			m_pipesCombo.Clear();
			if (m_selectedEffectDefinition.ExhaustPipes == null)
			{
				return;
			}
			long num = 0L;
			foreach (MyObjectBuilder_ExhaustEffectDefinition.Pipe exhaustPipe in m_selectedEffectDefinition.ExhaustPipes)
			{
				m_pipesCombo.AddItem(num++, exhaustPipe.Name);
			}
			if (m_pipesCombo.GetItemsCount() > 0)
			{
				m_pipesCombo.SelectItemByIndex(m_selectedPipe);
			}
		}

		private void pipe_ItemSelected()
		{
			m_selectedPipe = m_pipesCombo.GetSelectedIndex();
			m_effectsCombo.SelectItemByKey(m_selectedEffectDefinition.ExhaustPipes[m_selectedPipe].Effect.GetHashCode64());
			m_dummyTextbox.SetText(new StringBuilder(m_selectedEffectDefinition.ExhaustPipes[m_selectedPipe].Dummy));
			UpdateSliderValues();
		}

		private void effect_ItemSelected()
		{
			SelectedPipe.Effect = m_effectsCombo.GetSelectedValue().ToString();
		}

		private void UpdateSliderValues()
		{
			if (m_canUpdateValues)
			{
				m_canUpdateValues = false;
				m_effectIntensity.Value = SelectedPipe.EffectIntensity;
				m_powerToRadius.Value = SelectedPipe.PowerToRadius;
				m_powerToBirth.Value = SelectedPipe.PowerToBirth;
				m_powerToVelocity.Value = SelectedPipe.PowerToVelocity;
				m_powerToColorIntensity.Value = SelectedPipe.PowerToColorIntensity;
				m_powerToLife.Value = SelectedPipe.PowerToLife;
				m_canUpdateValues = true;
			}
		}

		private void AddExhaustEffects(MyGuiControlCombobox combo)
		{
			combo.Clear();
			IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
			if (allDefinitions == null)
			{
				return;
			}
			foreach (MyExhaustEffectDefinition item in allDefinitions)
			{
				combo.AddItem(item.Id.GetHashCodeLong(), item.Id.SubtypeName);
			}
		}

		private void AddEffects(MyGuiControlCombobox combo)
		{
			combo.Clear();
			foreach (string name in MyParticleEffectsLibrary.GetNames())
			{
				combo.AddItem(name.GetHashCode64(), name);
			}
		}

		private void onClick_Save(MyGuiControlButton sender)
		{
			Save("ExhaustEffects.*");
		}

		private void Save(string filePattern = "*.*")
		{
<<<<<<< HEAD
			Regex regex = FindFilesPatternToRegex.Convert(filePattern);
=======
			Regex val = FindFilesPatternToRegex.Convert(filePattern);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Dictionary<string, List<MyDefinitionBase>> dictionary = new Dictionary<string, List<MyDefinitionBase>>();
			IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
			if (allDefinitions == null)
			{
				return;
			}
			List<MyDefinitionBase> list = null;
			foreach (MyExhaustEffectDefinition item in allDefinitions)
			{
				if (item.Context == null || string.IsNullOrEmpty(item.Context.CurrentFile))
				{
					continue;
				}
				string fileName = Path.GetFileName(item.Context.CurrentFile);
<<<<<<< HEAD
				if (regex.IsMatch(fileName))
=======
				if (val.IsMatch(fileName))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (!dictionary.ContainsKey(item.Context.CurrentFile))
					{
						dictionary.Add(item.Context.CurrentFile, list = new List<MyDefinitionBase>());
					}
					else
					{
						list = dictionary[item.Context.CurrentFile];
					}
					list.Add(item);
				}
			}
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			List<MyObjectBuilder_DefinitionBase> list2 = new List<MyObjectBuilder_DefinitionBase>();
<<<<<<< HEAD
			if (list != null)
			{
				foreach (MyDefinitionBase item2 in list)
				{
					MyObjectBuilder_DefinitionBase objectBuilder = item2.GetObjectBuilder();
					list2.Add(objectBuilder);
				}
			}
			MyObjectBuilder_DefinitionBase[] array = (myObjectBuilder_Definitions.Definitions = list2.OfType<MyObjectBuilder_ExhaustEffectDefinition>().ToArray());
=======
			foreach (MyDefinitionBase item2 in list)
			{
				MyObjectBuilder_DefinitionBase objectBuilder = item2.GetObjectBuilder();
				list2.Add(objectBuilder);
			}
			MyObjectBuilder_DefinitionBase[] array = (myObjectBuilder_Definitions.Definitions = Enumerable.ToArray<MyObjectBuilder_ExhaustEffectDefinition>(Enumerable.OfType<MyObjectBuilder_ExhaustEffectDefinition>((IEnumerable)list2)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.ContentPath, "Data\\ExhaustEffects.sbc"), compress: false, myObjectBuilder_Definitions);
		}

		private void createEmptyExhaustEffect()
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			List<MyObjectBuilder_DefinitionBase> list = new List<MyObjectBuilder_DefinitionBase>();
			MyExhaustEffectDefinition myExhaustEffectDefinition = new MyExhaustEffectDefinition();
			myExhaustEffectDefinition.Id = new MyDefinitionId(typeof(MyObjectBuilder_ExhaustEffectDefinition), "Exhaust");
			myExhaustEffectDefinition.ExhaustPipes = new List<MyObjectBuilder_ExhaustEffectDefinition.Pipe>();
			myExhaustEffectDefinition.ExhaustPipes.Add(new MyObjectBuilder_ExhaustEffectDefinition.Pipe
			{
				Name = "Pipe0",
				Dummy = "emitter",
				Effect = "VehicleDust",
				EffectIntensity = 1f,
				PowerToBirth = 0f,
				PowerToRadius = 0f,
				PowerToLife = 0f,
				PowerToColorIntensity = 0f,
				PowerToVelocity = 0f
			});
			MyObjectBuilder_DefinitionBase objectBuilder = myExhaustEffectDefinition.GetObjectBuilder();
			string path = Path.Combine(MyFileSystem.ContentPath, "Data\\ExhaustEffects.sbc");
			list.Add(objectBuilder);
			myObjectBuilder_Definitions.Definitions = list.ToArray();
			MyObjectBuilderSerializer.SerializeXML(path, compress: false, myObjectBuilder_Definitions);
		}

		private void onClick_Reload(MyGuiControlButton sender)
		{
			MyDefinitionManager.Static.UnloadData(clearPreloaded: true);
			List<MyObjectBuilder_Checkpoint.ModItem> mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
			MyDefinitionManager.Static.LoadData(mods);
			RecreateControls(constructor: false);
		}

		private void OnEffectValueChanged(MyGuiControlSlider slider)
		{
			if (m_canUpdateValues)
			{
				SelectedPipe.EffectIntensity = m_effectIntensity.Value;
				SelectedPipe.PowerToBirth = m_powerToBirth.Value;
				SelectedPipe.PowerToColorIntensity = m_powerToColorIntensity.Value;
				SelectedPipe.PowerToLife = m_powerToLife.Value;
				SelectedPipe.PowerToRadius = m_powerToRadius.Value;
				SelectedPipe.PowerToVelocity = m_powerToVelocity.Value;
			}
		}
	}
}
