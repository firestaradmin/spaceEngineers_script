using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
	public static class MySpaceBindingCreator
	{
		private class JoystickBindingEvaluator : ITextEvaluator
		{
			public static void ParseToken(ref string token, out MyStringId controlContext, out MyStringId control)
			{
				int num = token.IndexOf(':');
				if (num >= 0)
				{
					controlContext = MyStringId.GetOrCompute(token.Substring(0, num));
					token = token.Substring(num + 1);
					control = MyStringId.GetOrCompute(token);
					return;
				}
				control = MyStringId.GetOrCompute(token);
				IMyControllableEntity myControllableEntity = MySession.Static?.ControlledEntity;
				if (myControllableEntity == null)
				{
					controlContext = CX_BASE;
					return;
				}
				MyStringId auxiliaryContext = myControllableEntity.AuxiliaryContext;
				if (MyControllerHelper.IsDefined(auxiliaryContext, control))
				{
					controlContext = auxiliaryContext;
				}
				else
				{
					controlContext = myControllableEntity.ControlContext;
				}
			}

			public string TokenEvaluate(string token, string context)
			{
				ParseToken(ref token, out var controlContext, out var control);
				return MyControllerHelper.GetCodeForControl(controlContext, control);
			}
		}

		private class SpaceBindingEvaluator : ITextEvaluator
		{
			public string TokenEvaluate(string token, string context)
			{
				if (MyInput.Static.IsJoystickLastUsed)
				{
					return JoystickEvaluator.TokenEvaluate(token, context);
				}
				ITextEvaluator textEvaluator;
				if ((textEvaluator = MyInput.Static as ITextEvaluator) != null)
				{
					JoystickBindingEvaluator.ParseToken(ref token, out var _, out var _);
					return "[" + textEvaluator.TokenEvaluate(token, context) + "]";
				}
				return token;
			}
		}

		public static readonly MyStringId CX_BASE = MyControllerHelper.CX_BASE;

		public static readonly MyStringId CX_GUI = MyControllerHelper.CX_GUI;

		public static readonly MyStringId CX_CHARACTER = MyControllerHelper.CX_CHARACTER;

		public static readonly MyStringId CX_SPACESHIP = MyControllerHelper.CX_SPACESHIP;

		public static readonly MyStringId CX_JETPACK = MyControllerHelper.CX_JETPACK;

		public static readonly MyStringId CX_SPECTATOR = MyStringId.GetOrCompute("SPECTATOR");

		public static readonly MyStringId CX_SPECTATOR = MyStringId.GetOrCompute("SPECTATOR");

		public static readonly MyStringId AX_BASE = MyStringId.GetOrCompute("ABASE");

		public static readonly MyStringId AX_TOOLS = MyStringId.GetOrCompute("TOOLS");

		public static readonly MyStringId AX_BUILD = MyStringId.GetOrCompute("BUILD");

		public static readonly MyStringId AX_SYMMETRY = MyStringId.GetOrCompute("AX_SYMMETRY");

		public static readonly MyStringId AX_VOXEL = MyStringId.GetOrCompute("VOXEL");

		public static readonly MyStringId AX_ACTIONS = MyStringId.GetOrCompute("ACTIONS");

		public static readonly MyStringId AX_COLOR_PICKER = MyStringId.GetOrCompute("COLOR_PICKER");

		public static readonly MyStringId AX_CLIPBOARD = MyStringId.GetOrCompute("CLIPBOARD");

		private static MyFlightBindingScheme m_flightBindingScheme = new MyFlightBindingScheme();

		private static MyFlightAlternativeBindingScheme m_flightAltBindingScheme = new MyFlightAlternativeBindingScheme();

		public static readonly ITextEvaluator BindingEvaluator = new SpaceBindingEvaluator();

		public static readonly ITextEvaluator JoystickEvaluator = new JoystickBindingEvaluator();

		public static void CreateBindingDefault()
<<<<<<< HEAD
		{
			MyControllerHelper.ClearBindings();
			bool flag = false;
			List<MyControllerSchemeDefinition> list = new List<MyControllerSchemeDefinition>();
			if (!MyFakes.FORCE_HARDCODED_GAMEPAD_CONTROLS)
			{
				ListReader<MyControllerSchemeDefinition> controllerSchemesAll = MyDefinitionManager.Static.GetControllerSchemesAll();
				List<MyControllerSchemeDefinition> list2 = new List<MyControllerSchemeDefinition>();
				foreach (MyControllerSchemeDefinition item in controllerSchemesAll)
				{
					if (item.IsSelectable)
					{
						list2.Add(item);
					}
					else
					{
						list.Add(item);
					}
				}
				string gamepadSchemeName = MySandboxGame.Config.GamepadSchemeName;
				int gamepadSchemeId = MySandboxGame.Config.GamepadSchemeId;
				MyControllerSchemeDefinition myControllerSchemeDefinition = null;
				MyControllerSchemeDefinition myControllerSchemeDefinition2 = null;
				MyControllerSchemeDefinition myControllerSchemeDefinition3 = null;
				for (int i = 0; i < list2.Count; i++)
				{
					MyControllerSchemeDefinition myControllerSchemeDefinition4 = list2[i];
					if (myControllerSchemeDefinition == null && myControllerSchemeDefinition4.Id.SubtypeName == gamepadSchemeName)
					{
						myControllerSchemeDefinition = myControllerSchemeDefinition4;
					}
					if (myControllerSchemeDefinition2 == null && i == gamepadSchemeId)
					{
						myControllerSchemeDefinition2 = myControllerSchemeDefinition4;
					}
					if (myControllerSchemeDefinition == null && myControllerSchemeDefinition4.IsDefault)
					{
						myControllerSchemeDefinition3 = myControllerSchemeDefinition4;
					}
				}
				if (myControllerSchemeDefinition != null || myControllerSchemeDefinition2 != null || myControllerSchemeDefinition3 != null)
				{
					list.Add((myControllerSchemeDefinition != null) ? myControllerSchemeDefinition : ((myControllerSchemeDefinition2 != null) ? myControllerSchemeDefinition2 : myControllerSchemeDefinition3));
				}
			}
			else
			{
				flag = true;
			}
			if (list.Count > 0)
			{
				HashSet<MyStringId> contextExceptions = new HashSet<MyStringId>();
				if (!CreateFromDefinitions(list, invertYAxisCharacter: false, invertYAxisJetpackVehicle: false, contextExceptions))
				{
					flag = true;
				}
			}
			if (list.Count == 0 || flag)
			{
				CreateBindingDefault(invertYAxisCharacter: false, invertYAxisJetpackVehicle: false);
			}
		}

		public static void CreateBindingDefault(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
=======
		{
			CreateBinding(MyInput.Static.GetJoystickYInversionCharacter(), MyInput.Static.GetJoystickYInversionVehicle());
		}

		public static void CreateBinding(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyControllerHelper.ClearBindings();
			switch (MySandboxGame.Config.GamepadSchemeId)
			{
			case 0:
				m_flightBindingScheme.CreateBinding(invertYAxisCharacter, invertYAxisJetpackVehicle);
				break;
			case 1:
				m_flightAltBindingScheme.CreateBinding(invertYAxisCharacter, invertYAxisJetpackVehicle);
				break;
			default:
				m_flightBindingScheme.CreateBinding(invertYAxisCharacter, invertYAxisJetpackVehicle);
				break;
			}
			CreateForGUI();
			CreateForAuxiliaryBase();
			CreateForTools();
			CreateForBuildMode();
			CreateForSymmetry();
			CreateForVoxelHands();
			CreateForClipboard();
			CreateForActions();
			CreateForColorPicker();
<<<<<<< HEAD
		}

		private static bool CreateFromDefinitions(List<MyControllerSchemeDefinition> definitions, bool invertYAxisCharacter, bool invertYAxisJetpackVehicle, HashSet<MyStringId> contextExceptions)
		{
			InitializeContexts();
			int num = 0;
			int num2 = 0;
			try
			{
				num = -1;
				foreach (MyControllerSchemeDefinition definition in definitions)
				{
					num++;
					num2 = -1;
					foreach (MyObjectBuilder_ControlBindingContext context in definition.Contexts)
					{
						foreach (MyObjectBuilder_ControlBinding controlBinding in context.ControlBindings)
						{
							num2++;
							MyStringId myStringId = MyStringId.Get(context.Id.SubtypeName);
							MyStringId myStringId2 = MyStringId.Get(controlBinding.Id.SubtypeName);
							if (contextExceptions.Contains(myStringId))
							{
								MyLog.Default.Error(string.Format("Using forbidden context in ControllerSchemeDefinition! Context: {0} schema: {1} bind: {2}", myStringId.String, num, num2, controlBinding.Id.SubtypeName));
							}
							else if (controlBinding.Actions.Count > 0 || controlBinding.GamepadAxes.Count > 0)
							{
								if (controlBinding.GamepadAxes.Count == 1 && ((invertYAxisCharacter && myStringId == MyControllerHelper.CX_CHARACTER) || (invertYAxisJetpackVehicle && (myStringId == MyControllerHelper.CX_JETPACK || myStringId == MyControllerHelper.CX_SPACESHIP))) && (myStringId2 == MyControlsSpace.ROTATION_DOWN || myStringId2 == MyControlsSpace.ROTATION_UP || myStringId2 == MyControlsSpace.LOOK_UP || myStringId2 == MyControlsSpace.LOOK_DOWN))
								{
									controlBinding.GamepadAxes[0] = InvertAxis(controlBinding.GamepadAxes[0]);
								}
								Func<bool> condition = null;
								switch (controlBinding.Condition)
								{
								case MyPredefinedContitions.Missing:
									MyLog.Default.Error($"Missing condition in controller sbc in schema {num} bind {num2} control {controlBinding.Id.SubtypeName}.");
									break;
								default:
									condition = GetPredefinedCondition(controlBinding.Condition);
									break;
								case MyPredefinedContitions.None:
									break;
								}
								switch (controlBinding.BindingType)
								{
								case MyControlBindingType.Fake:
								{
									MyObjectBuilder_FakeAction myObjectBuilder_FakeAction = controlBinding.Actions[0] as MyObjectBuilder_FakeAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_FakeAction.FakeString);
									break;
								}
								case MyControlBindingType.SimpleAxis:
									MyControllerHelper.AddControl(myStringId, myStringId2, TranslateToJoystick(controlBinding.GamepadAxes[0]), condition);
									break;
								case MyControlBindingType.PressedOneModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction36 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction36.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), pressed: true, condition);
									break;
								}
								case MyControlBindingType.PressedTwoModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction34 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction35 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction34.JoystickButton, myObjectBuilder_ControllerButtonAction35.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), pressed: true, condition);
									break;
								}
								case MyControlBindingType.PressedThreeModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction31 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction32 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction33 = controlBinding.Modifiers[2] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction31.JoystickButton, myObjectBuilder_ControllerButtonAction32.JoystickButton, myObjectBuilder_ControllerButtonAction33.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), pressed: true, condition);
									break;
								}
								case MyControlBindingType.PressedOneOfTwoModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction29 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction30 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction29.JoystickButton, myObjectBuilder_ControllerButtonAction30.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), condition);
									break;
								}
								case MyControlBindingType.ReleasedOneModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction28 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction28.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), pressed: false, condition);
									break;
								}
								case MyControlBindingType.ReleasedTwoModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction26 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction27 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction26.JoystickButton, myObjectBuilder_ControllerButtonAction27.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), pressed: false, condition);
									break;
								}
								case MyControlBindingType.ReleasedThreeModifierAxis:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction23 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction24 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction25 = controlBinding.Modifiers[2] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction23.JoystickButton, myObjectBuilder_ControllerButtonAction24.JoystickButton, myObjectBuilder_ControllerButtonAction25.JoystickButton, TranslateToJoystick(controlBinding.GamepadAxes[0]), pressed: false, condition);
									break;
								}
								case MyControlBindingType.SimpleButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction22 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction22.JoystickButton, condition);
									break;
								}
								case MyControlBindingType.PressedOneModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction20 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction21 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction20.JoystickButton, myObjectBuilder_ControllerButtonAction21.JoystickButton, pressed: true, condition);
									break;
								}
								case MyControlBindingType.PressedTwoModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction17 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction18 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction19 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction17.JoystickButton, myObjectBuilder_ControllerButtonAction18.JoystickButton, myObjectBuilder_ControllerButtonAction19.JoystickButton, pressed: true, condition);
									break;
								}
								case MyControlBindingType.PressedThreeModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction13 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction14 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction15 = controlBinding.Modifiers[2] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction16 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction13.JoystickButton, myObjectBuilder_ControllerButtonAction14.JoystickButton, myObjectBuilder_ControllerButtonAction15.JoystickButton, myObjectBuilder_ControllerButtonAction16.JoystickButton, pressed: true, condition);
									break;
								}
								case MyControlBindingType.PressedOneOfTwoModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction10 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction11 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction12 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction10.JoystickButton, myObjectBuilder_ControllerButtonAction11.JoystickButton, myObjectBuilder_ControllerButtonAction12.JoystickButton, condition);
									break;
								}
								case MyControlBindingType.ReleasedOneModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction8 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction9 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction8.JoystickButton, myObjectBuilder_ControllerButtonAction9.JoystickButton, pressed: false, condition);
									break;
								}
								case MyControlBindingType.ReleasedTwoModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction5 = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction6 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction7 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction5.JoystickButton, myObjectBuilder_ControllerButtonAction6.JoystickButton, myObjectBuilder_ControllerButtonAction7.JoystickButton, pressed: false, condition);
									break;
								}
								case MyControlBindingType.ReleasedThreeModifierButton:
								{
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction = controlBinding.Modifiers[0] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction2 = controlBinding.Modifiers[1] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction3 = controlBinding.Modifiers[2] as MyObjectBuilder_ControllerButtonAction;
									MyObjectBuilder_ControllerButtonAction myObjectBuilder_ControllerButtonAction4 = controlBinding.Actions[0] as MyObjectBuilder_ControllerButtonAction;
									MyControllerHelper.AddControl(myStringId, myStringId2, myObjectBuilder_ControllerButtonAction.JoystickButton, myObjectBuilder_ControllerButtonAction2.JoystickButton, myObjectBuilder_ControllerButtonAction3.JoystickButton, myObjectBuilder_ControllerButtonAction4.JoystickButton, pressed: false, condition);
									break;
								}
								default:
									MyLog.Default.Error($"Unable to deserialize controller sbc in schema {num} bind {num2} control {controlBinding.Id.SubtypeName}.");
									break;
								case MyControlBindingType.Empty:
									break;
								}
							}
							else
							{
								MyLog.Default.Error($"ControlBinding has no button nor axis assigned! Context: {myStringId.String} schema: {num} bind: {num2} control {controlBinding.Id.SubtypeName}");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.Critical($"Unable to deserialize controller sbc in schema {num} bind {num2}. Error {ex.Message}");
				return false;
			}
			return true;
		}

		public static void RecreateBindingXmls()
		{
			Dictionary<string, MyObjectBuilder_ControllerSchemeDefinition> dictionary = new Dictionary<string, MyObjectBuilder_ControllerSchemeDefinition>();
			MyControllerHelper.ClearBindings();
			m_flightBindingScheme.CreateBinding(MyInput.Static.GetJoystickYInversionCharacter(), MyInput.Static.GetJoystickYInversionVehicle());
			AddExistingControlsToScheme(dictionary, "Flight");
			MyControllerHelper.ClearBindings();
			m_flightAltBindingScheme.CreateBinding(MyInput.Static.GetJoystickYInversionCharacter(), MyInput.Static.GetJoystickYInversionVehicle());
			AddExistingControlsToScheme(dictionary, "FlightAlternative");
			MyControllerHelper.ClearBindings();
			CreateForGUI();
			CreateForAuxiliaryBase();
			CreateForTools();
			CreateForBuildMode();
			CreateForSymmetry();
			CreateForVoxelHands();
			CreateForClipboard();
			CreateForActions();
			CreateForColorPicker();
			AddExistingControlsToScheme(dictionary, "General");
			MyControllerHelper.ClearBindings();
			foreach (KeyValuePair<string, MyObjectBuilder_ControllerSchemeDefinition> item in dictionary)
			{
				item.Value.Save($"SavedSchemes/SavedScheme_{item.Key}.sbc");
			}
		}

		public static T GetBuilder<T>(string subtype) where T : MyObjectBuilder_Base
		{
			return MyObjectBuilderSerializer.CreateNewObject(new SerializableDefinitionId(typeof(T), subtype)) as T;
		}

		public static void AddExistingControlsToScheme(Dictionary<string, MyObjectBuilder_ControllerSchemeDefinition> contextsBySchemes, string schemeName = "")
		{
			Dictionary<MyStringId, MyControllerHelper.Context> all = MyControllerHelper.GetAll();
			MyObjectBuilder_ControllerSchemeDefinition builder = GetBuilder<MyObjectBuilder_ControllerSchemeDefinition>(schemeName);
			builder.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_ControllerSchemeDefinition), schemeName);
			builder.IsDefault = false;
			builder.IsSelectable = true;
			builder.SubtypeName = schemeName;
			contextsBySchemes.Add(schemeName, builder);
			foreach (KeyValuePair<MyStringId, MyControllerHelper.Context> item in all)
			{
				MyObjectBuilder_ControlBindingContext myObjectBuilder_ControlBindingContext = new MyObjectBuilder_ControlBindingContext();
				if (item.Value.Bindings.Count <= 0)
				{
					continue;
				}
				myObjectBuilder_ControlBindingContext.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_ControlBindingContext), item.Key.ToString());
				myObjectBuilder_ControlBindingContext.ControlBindings = new MySerializableList<MyObjectBuilder_ControlBinding>();
				contextsBySchemes[schemeName].Contexts.Add(myObjectBuilder_ControlBindingContext);
				foreach (KeyValuePair<MyStringId, IMyControllerControl> binding in item.Value.Bindings)
				{
					MyObjectBuilder_ControlBinding myObjectBuilder_ControlBinding = new MyObjectBuilder_ControlBinding();
					myObjectBuilder_ControlBinding.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_ControlBinding), binding.Key.ToString());
					myObjectBuilder_ControlBinding.GamepadAxes = new MySerializableList<MyGamepadAxes>();
					myObjectBuilder_ControlBinding.Actions = new MySerializableList<MyObjectBuilder_Action>();
					myObjectBuilder_ControlBinding.Modifiers = new MySerializableList<MyObjectBuilder_Action>();
					IMyControllerControl value = binding.Value;
					if (value != null && value.Condition != null)
					{
						myObjectBuilder_ControlBinding.Condition = MyPredefinedContitions.Missing;
					}
					if (binding.Value is MyControllerHelper.EmptyControl)
					{
						myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.Empty;
						myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction());
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.FakeControl)
					{
						myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.Fake;
						myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_FakeAction(((MyControllerHelper.FakeControl)binding.Value).m_fakeCode));
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickAxis)
					{
						myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.SimpleAxis;
						myObjectBuilder_ControlBinding.GamepadAxes.Add(TranslateToGamepad(((MyControllerHelper.JoystickAxis)binding.Value).Axis));
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickButton)
					{
						myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.SimpleButton;
						myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction(((MyControllerHelper.JoystickButton)binding.Value).Button));
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickPressedModifier)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedModifier)binding.Value).Modifier.Code));
						if (((MyControllerHelper.JoystickPressedModifier)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedOneModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickPressedModifier)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedOneModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedModifier)binding.Value).Control.Code, ButtonAction.NewPressed));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickReleasedModifier)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedModifier)binding.Value).Modifier.Code));
						if (((MyControllerHelper.JoystickReleasedModifier)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.ReleasedOneModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickReleasedModifier)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.ReleasedOneModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedModifier)binding.Value).Control.Code, ButtonAction.NewReleased));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickPressedTwoModifiers)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedTwoModifiers)binding.Value).Modifier1.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedTwoModifiers)binding.Value).Modifier2.Code));
						if (((MyControllerHelper.JoystickPressedTwoModifiers)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedTwoModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickPressedTwoModifiers)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedTwoModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedTwoModifiers)binding.Value).Control.Code, ButtonAction.NewPressed));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickReleasedTwoModifiers)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedTwoModifiers)binding.Value).Modifier1.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedTwoModifiers)binding.Value).Modifier2.Code));
						if (((MyControllerHelper.JoystickReleasedTwoModifiers)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.ReleasedTwoModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickReleasedTwoModifiers)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.ReleasedTwoModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedTwoModifiers)binding.Value).Control.Code, ButtonAction.NewReleased));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickPressedThreeModifiers)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedThreeModifiers)binding.Value).Modifier1.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedThreeModifiers)binding.Value).Modifier2.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedThreeModifiers)binding.Value).Modifier3.Code));
						if (((MyControllerHelper.JoystickPressedThreeModifiers)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedThreeModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickPressedThreeModifiers)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedThreeModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickPressedThreeModifiers)binding.Value).Control.Code, ButtonAction.NewPressed));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
						continue;
					}
					if (binding.Value is MyControllerHelper.JoystickReleasedThreeModifiers)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedThreeModifiers)binding.Value).Modifier1.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedThreeModifiers)binding.Value).Modifier2.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedThreeModifiers)binding.Value).Modifier3.Code));
						if (((MyControllerHelper.JoystickReleasedThreeModifiers)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.ReleasedThreeModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickReleasedThreeModifiers)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.ReleasedThreeModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickReleasedThreeModifiers)binding.Value).Control.Code, ButtonAction.NewReleased));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
					}
					if (binding.Value is MyControllerHelper.JoystickOneOfTwoModifiers)
					{
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickOneOfTwoModifiers)binding.Value).PressedModifier.Code));
						myObjectBuilder_ControlBinding.Modifiers.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickOneOfTwoModifiers)binding.Value).ReleasedModifier.Code, ButtonAction.Released));
						if (((MyControllerHelper.JoystickOneOfTwoModifiers)binding.Value).Control is MyControllerHelper.JoystickAxis)
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedOneOfTwoModifierAxis;
							myObjectBuilder_ControlBinding.GamepadAxes.Add((MyGamepadAxes)((MyControllerHelper.JoystickOneOfTwoModifiers)binding.Value).Control.Code);
						}
						else
						{
							myObjectBuilder_ControlBinding.BindingType = MyControlBindingType.PressedOneOfTwoModifierButton;
							myObjectBuilder_ControlBinding.Actions.Add(new MyObjectBuilder_ControllerButtonAction((MyJoystickButtonsEnum)((MyControllerHelper.JoystickOneOfTwoModifiers)binding.Value).Control.Code, ButtonAction.NewPressed));
						}
						myObjectBuilder_ControlBindingContext.ControlBindings.Add(myObjectBuilder_ControlBinding);
					}
				}
			}
		}

		public static MyJoystickButtonsEnum TranslateToJoystick(MyGamepadButtons gamepad)
		{
			return (MyJoystickButtonsEnum)gamepad;
		}

		public static MyGamepadButtons TranslateToGamepad(MyJoystickButtonsEnum joystick)
		{
			return (MyGamepadButtons)joystick;
		}

		public static MyJoystickAxesEnum TranslateToJoystick(MyGamepadAxes gamepad)
		{
			return (MyJoystickAxesEnum)gamepad;
		}

		public static MyGamepadAxes TranslateToGamepad(MyJoystickAxesEnum joystick)
		{
			return (MyGamepadAxes)joystick;
		}

		private static MyGamepadAxes InvertAxis(MyGamepadAxes axis)
		{
			return (MyGamepadAxes)InvertAxis((MyJoystickAxesEnum)axis);
		}

		private static MyJoystickAxesEnum InvertAxis(MyJoystickAxesEnum axis)
		{
			switch (axis)
			{
			case MyJoystickAxesEnum.RotationXneg:
				return MyJoystickAxesEnum.RotationXpos;
			case MyJoystickAxesEnum.RotationXpos:
				return MyJoystickAxesEnum.RotationXneg;
			case MyJoystickAxesEnum.Xneg:
				return MyJoystickAxesEnum.Xpos;
			case MyJoystickAxesEnum.Xpos:
				return MyJoystickAxesEnum.Xneg;
			case MyJoystickAxesEnum.RotationYneg:
				return MyJoystickAxesEnum.RotationYpos;
			case MyJoystickAxesEnum.RotationYpos:
				return MyJoystickAxesEnum.RotationYneg;
			case MyJoystickAxesEnum.Yneg:
				return MyJoystickAxesEnum.Ypos;
			case MyJoystickAxesEnum.Ypos:
				return MyJoystickAxesEnum.Yneg;
			case MyJoystickAxesEnum.RotationZneg:
				return MyJoystickAxesEnum.RotationZpos;
			case MyJoystickAxesEnum.RotationZpos:
				return MyJoystickAxesEnum.RotationZneg;
			case MyJoystickAxesEnum.Zneg:
				return MyJoystickAxesEnum.Zpos;
			case MyJoystickAxesEnum.Zpos:
				return MyJoystickAxesEnum.Zneg;
			default:
				return axis;
			}
		}

		public static Func<bool> GetPredefinedCondition(MyPredefinedContitions condition)
		{
			switch (condition)
			{
			case MyPredefinedContitions.CreativeOnly:
				return () => MySession.Static.CreativeMode;
			case MyPredefinedContitions.SurvivalOnly:
				return () => MySession.Static.SurvivalMode;
			case MyPredefinedContitions.NotLookingAround:
				return () => !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED);
			case MyPredefinedContitions.NotLookingAroundNorRolling:
				return () => !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED);
			default:
				return null;
			}
		}

		private static void InitializeContexts()
		{
			MyControllerHelper.AddContext(CX_BASE);
			MyControllerHelper.AddContext(MyControllerHelper.CX_CHARACTER, CX_BASE);
			MyControllerHelper.AddContext(MyControllerHelper.CX_JETPACK, CX_BASE);
			MyControllerHelper.AddContext(MyControllerHelper.CX_SPACESHIP, CX_BASE);
			MyControllerHelper.AddContext(CX_SPECTATOR, CX_BASE);
			MyControllerHelper.AddContext(CX_GUI, CX_BASE);
			MyControllerHelper.AddContext(AX_BASE);
			MyControllerHelper.AddContext(AX_TOOLS, AX_BASE);
			MyControllerHelper.AddContext(AX_BUILD, AX_BASE);
			MyControllerHelper.AddContext(AX_SYMMETRY, AX_BASE);
			MyControllerHelper.AddContext(AX_VOXEL, AX_BASE);
			MyControllerHelper.AddContext(AX_CLIPBOARD, AX_BASE);
			MyControllerHelper.AddContext(AX_COLOR_PICKER, AX_BASE);
			MyControllerHelper.AddContext(AX_ACTIONS, AX_BASE);
		}

		public static void RegisterEvaluators()
		{
			MyTexts.RegisterEvaluator("CONTROL", BindingEvaluator);
			ITextEvaluator eval;
			if ((eval = MyInput.Static as ITextEvaluator) != null)
			{
				MyTexts.RegisterEvaluator("GAME_CONTROL", eval);
			}
			MyTexts.RegisterEvaluator("GAMEPAD", MyControllerHelper.ButtonTextEvaluator);
			MyTexts.RegisterEvaluator("GAMEPAD_CONTROL", JoystickEvaluator);
		}

=======
		}

		public static void RegisterEvaluators()
		{
			MyTexts.RegisterEvaluator("CONTROL", BindingEvaluator);
			ITextEvaluator eval;
			if ((eval = MyInput.Static as ITextEvaluator) != null)
			{
				MyTexts.RegisterEvaluator("GAME_CONTROL", eval);
			}
			MyTexts.RegisterEvaluator("GAMEPAD", MyControllerHelper.ButtonTextEvaluator);
			MyTexts.RegisterEvaluator("GAMEPAD_CONTROL", JoystickEvaluator);
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static void CreateForAuxiliaryBase()
		{
			MyControllerHelper.AddContext(AX_BASE);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.ADMIN_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.BLUEPRINTS_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SWITCHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_2, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_3, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_4, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.VOXEL_SELECT_SPHERE, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, () => MySession.Static.CreativeMode);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.TOGGLE_SIGNALS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, () => MySession.Static.SurvivalMode);
		}

		private static void CreateForTools()
		{
			MyControllerHelper.AddContext(AX_TOOLS, AX_BASE);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOGGLE_HUD, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.CHAT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.PROGRESSION_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
		}

		private static void CreateForBuildMode()
		{
			MyControllerHelper.AddContext(AX_BUILD, AX_BASE);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.ROTATE_AXIS_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.NEXT_BLOCK_STAGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.USE_SYMMETRY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.MOVE_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.MOVE_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_CANCEL, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_REMOVE, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_ADD, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH_ALTERNATIVE, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.SurvivalMode);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.CreativeMode);
		}

		private static void CreateForSymmetry()
		{
			MyControllerHelper.AddContext(AX_SYMMETRY, AX_BASE);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.NEXT_BLOCK_STAGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.SurvivalMode);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.CreativeMode);
		}

		private static void CreateForVoxelHands()
		{
			MyControllerHelper.AddContext(AX_VOXEL, AX_BASE);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_PLACE_DUMMY_RELEASE, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zpos, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_REVERT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_PAINT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
		}

		private static void CreateForClipboard()
		{
			MyControllerHelper.AddContext(AX_CLIPBOARD, AX_BASE);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.COPY_PASTE_ACTION, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.COPY_PASTE_CANCEL, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.ROTATE_AXIS_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.SWITCH_BUILDING_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.MOVE_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.MOVE_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
		}

		private static void CreateForColorPicker()
		{
			MyControllerHelper.AddContext(AX_COLOR_PICKER, AX_BASE);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SATURATION_DECREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.VALUE_INCREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.VALUE_DECREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SATURATION_INCREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.COPY_COLOR, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zpos, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.RECOLOR, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.MEDIUM_COLOR_BRUSH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.LARGE_COLOR_BRUSH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.RECOLOR_WHOLE_GRID, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, pressed: true);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.SurvivalMode);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.CreativeMode);
		}

		private static void CreateForActions()
		{
			MyControllerHelper.AddContext(AX_ACTIONS, AX_BASE);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.CUBE_COLOR_CHANGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.TOGGLE_HUD, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.CHAT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J02, pressed: true);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
		}

		private static void CreateForGUI()
		{
			MyControllerHelper.AddContext(CX_GUI, CX_BASE);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACCEPT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.CANCEL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION2, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACCEPT_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.CANCEL_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION1_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION2_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_UP, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_DOWN, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_LEFT, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_RIGHT, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SHIFT_LEFT, MyJoystickButtonsEnum.J05);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyJoystickButtonsEnum.J06);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_UP, MyJoystickAxesEnum.Yneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_DOWN, MyJoystickAxesEnum.Ypos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_LEFT, MyJoystickAxesEnum.Xneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_RIGHT, MyJoystickAxesEnum.Xpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_UP, MyJoystickAxesEnum.RotationYneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_DOWN, MyJoystickAxesEnum.RotationYpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_LEFT, MyJoystickAxesEnum.RotationXneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_RIGHT, MyJoystickAxesEnum.RotationXpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON, MyJoystickButtonsEnum.J10);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_ITEM_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.BUTTON_A, MyJoystickButtonsEnum.J01);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.BUTTON_B, MyJoystickButtonsEnum.J02);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.BUTTON_X, MyJoystickButtonsEnum.J03);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.BUTTON_Y, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LEFT_BUTTON, MyJoystickButtonsEnum.J05);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.RIGHT_BUTTON, MyJoystickButtonsEnum.J06);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LISTBOX_TOGGLE_SELECTION, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LISTBOX_SELECT_RANGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LISTBOX_SELECT_ALL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LISTBOX_SELECT_ONLY_FOCUSED, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.LISTBOX_SIMPLE_SELECT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.FAKE_RS, '\ue00a'.ToString());
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MODIF_L, MyJoystickButtonsEnum.J05);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MODIF_R, MyJoystickButtonsEnum.J06);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.VIEW, MyJoystickButtonsEnum.J07);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MENU, MyJoystickButtonsEnum.J08);
		}
	}
}
