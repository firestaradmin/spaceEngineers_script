using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyPetaInputComponent : MyDebugComponent
	{
		public static float SI_DYNAMICS_MULTIPLIER = 1f;

		public static bool SHOW_HUD_ALWAYS = false;

		public static bool DRAW_WARNINGS = true;

		public static int DEBUG_INDEX = 0;

		public static Vector3D MovementDistanceStart;

		public static float MovementDistance = 1f;

		public static int MovementDistanceCounter = -1;

		private static bool m_columnVisible = true;

		private static bool m_columnVisible = true;

		private MyConcurrentDictionary<MyCubePart, List<uint>> m_cubeParts = new MyConcurrentDictionary<MyCubePart, List<uint>>();

		private const int N = 9;

		private const int NT = 181;

		private MyVoxelMap m_voxelMap;

		private List<MySkinnedEntity> m_skins = new List<MySkinnedEntity>();

		private string m_response;

		private ResponseType m_responseType;

		public override string GetName()
		{
			return "Peta";
		}

		public MyPetaInputComponent()
		{
			AddShortcut(MyKeys.OemBackslash, newPress: true, control: true, shift: false, alt: false, () => "Debug draw physics clusters: " + MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS, delegate
			{
				MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS;
				return true;
			});
			AddShortcut(MyKeys.OemBackslash, newPress: false, control: false, shift: false, alt: false, () => "Advance all moving entities", delegate
			{
				AdvanceEntities();
				return true;
			});
			AddShortcut(MyKeys.S, newPress: true, control: true, shift: false, alt: false, () => "Insert safe zone", delegate
			{
				InsertSafeZone();
				return true;
			});
			AddShortcut(MyKeys.Back, newPress: true, control: true, shift: false, alt: false, () => "Freeze gizmo: " + MyCubeBuilder.Static.FreezeGizmo, delegate
			{
				MyCubeBuilder.Static.FreezeGizmo = !MyCubeBuilder.Static.FreezeGizmo;
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Test movement distance: " + MovementDistance, delegate
			{
				if (MovementDistance != 0f)
				{
					MovementDistance = 0f;
					MovementDistanceStart = ((MyEntity)MySession.Static.ControlledEntity).PositionComp.GetPosition();
					MovementDistanceCounter = (int)SI_DYNAMICS_MULTIPLIER;
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Show warnings: " + DRAW_WARNINGS, delegate
			{
				DRAW_WARNINGS = !DRAW_WARNINGS;
				return true;
			});
<<<<<<< HEAD
=======
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Reload Good.bot stats", delegate
			{
				ReloadGoodbotStats();
				return true;
			});
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Import GoodBot csv", delegate
			{
				ImportGoodBotCSV();
				return true;
			});
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Reset Ingame Help", delegate
			{
				MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.Reset();
				return true;
			});
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Move VCs to ships and fly at 20m/s speed", delegate
			{
				MoveVCToShips();
				return true;
			});
			AddShortcut(MyKeys.Left, newPress: true, control: false, shift: false, alt: false, () => "Debug index--", delegate
			{
				DEBUG_INDEX--;
				if (DEBUG_INDEX < 0)
				{
					DEBUG_INDEX = 7;
				}
				MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES = true;
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				return true;
			});
			AddShortcut(MyKeys.Right, newPress: true, control: false, shift: false, alt: false, () => "Debug index++", delegate
			{
				DEBUG_INDEX++;
				if (DEBUG_INDEX > 7)
				{
					DEBUG_INDEX = 0;
				}
				MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES = true;
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Teleport other clients to me", delegate
			{
				TeleportOtherClientsToMe();
				return true;
			});
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Test board screen", delegate
			{
				TestBoardScreen();
				return true;
			});
			AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Column visible : " + m_columnVisible, delegate
			{
				m_columnVisible = !m_columnVisible;
				TestBoardScreenVisibility(m_columnVisible);
				return true;
			});
		}

		private void TestParallelDictionary()
		{
			Parallel.For(0, 1000, delegate
			{
				switch (MyRandom.Instance.Next(5))
				{
				case 0:
					m_cubeParts.TryAdd(new MyCubePart(), new List<uint> { 0u, 1u, 2u });
					break;
				case 1:
					foreach (KeyValuePair<MyCubePart, List<uint>> cubePart in m_cubeParts)
					{
						_ = cubePart;
						Thread.Sleep(10);
					}
					break;
				case 2:
					if (m_cubeParts.Count > 0)
					{
						m_cubeParts.Remove(Enumerable.First<KeyValuePair<MyCubePart, List<uint>>>((IEnumerable<KeyValuePair<MyCubePart, List<uint>>>)m_cubeParts).Key);
					}
					break;
				case 3:
					foreach (KeyValuePair<MyCubePart, List<uint>> cubePart2 in m_cubeParts)
					{
						_ = cubePart2;
						Thread.Sleep(1);
					}
					break;
				}
			});
		}

		private static void AdvanceEntities()
		{
			foreach (MyEntity item in Enumerable.ToList<MyEntity>((IEnumerable<MyEntity>)MyEntities.GetEntities()))
			{
				if (item.Physics != null && item.Physics.LinearVelocity.Length() > 0.1f)
				{
					Vector3D vector3D = item.Physics.LinearVelocity * SI_DYNAMICS_MULTIPLIER * 100000f;
					MatrixD worldMatrix = item.WorldMatrix;
					worldMatrix.Translation += vector3D;
					item.WorldMatrix = worldMatrix;
				}
			}
		}

		public override bool HandleInput()
		{
			if (base.HandleInput())
			{
				return true;
			}
			return false;
		}

		private int viewNumber(int i, int j)
		{
			return i * (19 - Math.Abs(i)) + j + 90;
		}

		private void findViews(int species, Vector3 cDIR, out Vector3I vv, out Vector3 rr)
		{
			Vector3 vector = new Vector3(cDIR.X, Math.Max(0f - cDIR.Y, 0.01f), cDIR.Z);
			float num = ((Math.Abs(vector.X) > Math.Abs(vector.Z)) ? ((0f - vector.Z) / vector.X) : ((0f - vector.X) / (0f - vector.Z)));
			float num2 = 9f * (1f - num) * (float)Math.Acos(MathHelper.Clamp(vector.Y, -1f, 1f)) / (float)Math.PI;
			float num3 = 9f * (1f + num) * (float)Math.Acos(MathHelper.Clamp(vector.Y, -1f, 1f)) / (float)Math.PI;
			int num4 = (int)Math.Floor(num2);
			int num5 = (int)Math.Floor(num3);
			float num6 = num2 - (float)num4;
			float num7 = num3 - (float)num5;
			float num8 = 1f - num6 - num7;
			bool flag = (double)num8 > 0.0;
			Vector3I vector3I = new Vector3I(flag ? num4 : (num4 + 1), num4 + 1, num4);
			Vector3I vector3I2 = new Vector3I(flag ? num5 : (num5 + 1), num5, num5 + 1);
			rr = new Vector3(Math.Abs(num8), flag ? ((double)num6) : (1.0 - (double)num7), flag ? ((double)num7) : (1.0 - (double)num6));
			if (Math.Abs(vector.Z) >= Math.Abs(vector.X))
			{
				Vector3I vector3I3 = vector3I;
				vector3I = -vector3I2;
				vector3I2 = vector3I3;
			}
			if (Math.Abs(vector.X + (0f - vector.Z)) > 1E-05f)
			{
				vector3I *= Math.Sign(vector.X + (0f - vector.Z));
				vector3I2 *= Math.Sign(vector.X + (0f - vector.Z));
			}
			vv = new Vector3I(species * 181) + new Vector3I(viewNumber(vector3I.X, vector3I2.X), viewNumber(vector3I.Y, vector3I2.Y), viewNumber(vector3I.Z, vector3I2.Z));
		}

		public override void Draw()
		{
			if (MySector.MainCamera == null)
			{
				return;
			}
			base.Draw();
			if (m_voxelMap != null)
			{
				MyRenderProxy.DebugDrawAxis(m_voxelMap.WorldMatrix, 100f, depthRead: false);
			}
			if (!MyDebugDrawSettings.DEBUG_DRAW_FRACTURED_PIECES)
			{
				return;
			}
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyFracturedPiece myFracturedPiece = entity as MyFracturedPiece;
				if (myFracturedPiece != null)
				{
<<<<<<< HEAD
					MyPhysicsDebugDraw.DebugDrawBreakable(myFracturedPiece.Physics.BreakableBody, myFracturedPiece.Physics.ClusterToWorld(Vector3.Zero));
=======
					MyPhysicsDebugDraw.DebugDrawBreakable(myFracturedPiece.Physics.BreakableBody, myFracturedPiece.Physics.ClusterToWorld(Vector3D.Zero));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void InsertTree()
		{
			MyDefinitionId id = new MyDefinitionId(MyObjectBuilderType.Parse("MyObjectBuilder_Tree"), "Tree04_v2");
			MyEnvironmentItemDefinition environmentItemDefinition = MyDefinitionManager.Static.GetEnvironmentItemDefinition(id);
			if (MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes != null)
			{
				HkdBreakableShape hkdBreakableShape = MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes[0].Clone();
				MatrixD worldMatrix = MatrixD.CreateWorld(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + 2.0 * MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward, Vector3.Forward, Vector3.Up);
				List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
				hkdBreakableShape.GetChildren(list);
				list[0].Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
				MyDestructionHelper.CreateFracturePiece(hkdBreakableShape, ref worldMatrix, isStatic: false, environmentItemDefinition.Id, sync: true);
			}
		}

		private void TestIngameHelp()
		{
			MyHud.Questlog.Visible = true;
		}

		private void SpawnSimpleSkinnedObject()
		{
			MySkinnedEntity mySkinnedEntity = new MySkinnedEntity();
			MyObjectBuilder_Character myObjectBuilder_Character = new MyObjectBuilder_Character();
			myObjectBuilder_Character.EntityDefinitionId = new SerializableDefinitionId(typeof(MyObjectBuilder_Character), "Medieval_barbarian");
			myObjectBuilder_Character.PositionAndOrientation = new MyPositionAndOrientation(MySector.MainCamera.Position + 2f * MySector.MainCamera.ForwardVector, MySector.MainCamera.ForwardVector, MySector.MainCamera.UpVector);
			mySkinnedEntity.Init(null, "Models\\Characters\\Basic\\ME_barbar.mwm", null, null);
			mySkinnedEntity.Init(myObjectBuilder_Character);
			MyEntities.Add(mySkinnedEntity);
			MyAnimationCommand myAnimationCommand = default(MyAnimationCommand);
			myAnimationCommand.AnimationSubtypeName = "IdleBarbar";
			myAnimationCommand.FrameOption = MyFrameOption.Loop;
			myAnimationCommand.TimeScale = 1f;
			MyAnimationCommand command = myAnimationCommand;
			mySkinnedEntity.AddCommand(command);
			m_skins.Add(mySkinnedEntity);
		}

		private static void HighlightGScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			MyGuiControlBase control = screenWithFocus.GetControlByName("ScrollablePanel").Elements[0];
			MyGuiControlBase controlByName = screenWithFocus.GetControlByName("MyGuiControlGridDragAndDrop");
			MyGuiControlBase control2 = screenWithFocus.GetControlByName("MyGuiControlToolbar").Elements[2];
			MyGuiScreenHighlight.MyHighlightControl[] array = new MyGuiScreenHighlight.MyHighlightControl[3];
			MyGuiScreenHighlight.MyHighlightControl myHighlightControl = new MyGuiScreenHighlight.MyHighlightControl
			{
				Control = control,
				Indices = new int[3] { 0, 1, 2 }
			};
			array[0] = myHighlightControl;
			myHighlightControl = new MyGuiScreenHighlight.MyHighlightControl
			{
				Control = controlByName
			};
			array[1] = myHighlightControl;
			myHighlightControl = new MyGuiScreenHighlight.MyHighlightControl
			{
				Control = control2,
				Indices = new int[1]
			};
			array[2] = myHighlightControl;
			MyGuiScreenHighlight.HighlightControls(array);
		}

		private void MoveVCToShips()
		{
			List<MyCharacter> list = new List<MyCharacter>();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null && !myCharacter.ControllerInfo.IsLocallyHumanControlled() && myCharacter.ControllerInfo.IsLocallyControlled())
				{
					list.Add(myCharacter);
				}
			}
			List<MyCubeGrid> list2 = new List<MyCubeGrid>();
			foreach (MyEntity entity2 in MyEntities.GetEntities())
			{
				MyCubeGrid myCubeGrid = entity2 as MyCubeGrid;
				if (myCubeGrid != null && !myCubeGrid.GridSystems.ControlSystem.IsControlled && myCubeGrid.GridSizeEnum == MyCubeSize.Large && !myCubeGrid.IsStatic)
				{
					list2.Add(myCubeGrid);
				}
			}
			while (list.Count > 0 && list2.Count > 0)
			{
				MyCharacter user = list[0];
				list.RemoveAt(0);
				MyCubeGrid myCubeGrid2 = list2[0];
				list2.RemoveAt(0);
				List<MyCockpit> list3 = new List<MyCockpit>();
				foreach (MyCubeBlock fatBlock in myCubeGrid2.GetFatBlocks())
				{
					MyCockpit myCockpit = fatBlock as MyCockpit;
					if (myCockpit != null && myCockpit.BlockDefinition.EnableShipControl)
					{
						list3.Add(myCockpit);
					}
				}
				list3[0].RequestUse(UseActionEnum.Manipulate, user);
			}
		}

		private void InsertSafeZone()
		{
			((MyEntity)MySession.Static.ControlledEntity).PositionComp.SetPosition(((MyEntity)MySession.Static.ControlledEntity).PositionComp.GetPosition() + new Vector3D(double.PositiveInfinity));
		}

<<<<<<< HEAD
=======
		private void ImportGoodBotCSV()
		{
			string[] array = File.ReadAllText("c:\\Users\\admin\\Downloads\\GoodBot English - Questions Bot.csv").Split(new string[2]
			{
				",",
				Environment.get_NewLine()
			}, StringSplitOptions.RemoveEmptyEntries);
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = new MyObjectBuilder_Definitions();
			List<MyObjectBuilder_ChatBotResponseDefinition> list = new List<MyObjectBuilder_ChatBotResponseDefinition>();
			MyObjectBuilder_ChatBotResponseDefinition myObjectBuilder_ChatBotResponseDefinition = null;
			List<string> list2 = new List<string>();
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.StartsWith("Description_"))
				{
					if (myObjectBuilder_ChatBotResponseDefinition != null)
					{
						myObjectBuilder_ChatBotResponseDefinition.Questions = list2.ToArray();
						list2.Clear();
					}
					myObjectBuilder_ChatBotResponseDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ChatBotResponseDefinition>();
					myObjectBuilder_ChatBotResponseDefinition.SubtypeName = text.Replace("Description_", "");
					myObjectBuilder_ChatBotResponseDefinition.Id = new SerializableDefinitionId(myObjectBuilder_ChatBotResponseDefinition.TypeId, myObjectBuilder_ChatBotResponseDefinition.SubtypeName);
					myObjectBuilder_ChatBotResponseDefinition.Response = text;
					list2.Clear();
					list.Add(myObjectBuilder_ChatBotResponseDefinition);
				}
				else if (myObjectBuilder_ChatBotResponseDefinition != null)
				{
					list2.Add(text);
				}
			}
			if (myObjectBuilder_ChatBotResponseDefinition != null)
			{
				myObjectBuilder_ChatBotResponseDefinition.Questions = list2.ToArray();
				list2.Clear();
			}
			myObjectBuilder_Definitions.ChatBot = list.ToArray();
			MyObjectBuilderSerializer.SerializeXML("c:\\Users\\admin\\Downloads\\GoodBot.sbc", compress: false, myObjectBuilder_Definitions);
		}

		private void ReloadGoodbotStats()
		{
			MyChatBot component = MySession.Static.GetComponent<MyChatBot>();
			IMyChatBotResponder chatBotResponder = component.ChatBotResponder;
			chatBotResponder.OnResponse = (ChatBotResponseDelegate)Delegate.Combine(chatBotResponder.OnResponse, new ChatBotResponseDelegate(OnChatBotResponse));
			string text = File.ReadAllText("c:\\Users\\admin\\Downloads\\GoodBot English - Log from final 190.tsv");
			string text2 = "c:\\Users\\admin\\Downloads\\GoodBot English - Log from final 195.csv";
			string text3 = "";
			string[] array = text.Split(new string[2]
			{
				"\t",
				Environment.get_NewLine()
			}, StringSplitOptions.None);
			int i = 0;
			for (int j = 0; j < 3; j++)
			{
				text3 = text3 + array[j] + "\t";
				i++;
			}
			text3 += "Local response type\tLocal Response TextId\n";
			for (; i < array.Length; i += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					text3 = text3 + array[k + i] + "\t";
				}
				text3 = ((!component.FilterMessage("? " + array[1 + i], null)) ? (text3 + "\t\t\n") : string.Concat(text3, m_responseType, "\t", m_response, "\n"));
			}
			File.WriteAllText(text2, text3);
			IMyChatBotResponder chatBotResponder2 = component.ChatBotResponder;
			chatBotResponder2.OnResponse = (ChatBotResponseDelegate)Delegate.Remove(chatBotResponder2.OnResponse, new ChatBotResponseDelegate(OnChatBotResponse));
		}

		private void OnChatBotResponse(string originalQuestion, string responseId, ResponseType responseType, Action<string> responseAction)
		{
			m_response = responseId;
			m_responseType = responseType;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void FixScenario(string scenarioName)
		{
			foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Scenarios", scenarioName), "*.sbs", MySearchOption.AllDirectories))
			{
				if (!file.EndsWith("B5"))
				{
<<<<<<< HEAD
					string path = file + "B5";
					if (MyFileSystem.FileExists(path))
					{
						File.Delete(path);
=======
					string text = file + "B5";
					if (MyFileSystem.FileExists(text))
					{
						File.Delete(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					ulong fileSize = 0uL;
					MyObjectBuilder_Sector objectBuilder = null;
					MyObjectBuilderSerializer.DeserializeXML(file, out objectBuilder, out fileSize);
					FixScenario(MyLocalCache.LoadCheckpoint(Path.GetDirectoryName(file), out var _), objectBuilder);
					MyObjectBuilderSerializer.SerializeXML(file, compress: false, objectBuilder, out fileSize);
<<<<<<< HEAD
					MyObjectBuilderSerializer.SerializePB(path, compress: false, objectBuilder);
				}
			}
			void FixScenario(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
=======
					MyObjectBuilderSerializer.SerializePB(text, compress: false, objectBuilder);
				}
			}
			static void FixScenario(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
				{
					MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = sectorObject as MyObjectBuilder_CubeGrid;
					if (myObjectBuilder_CubeGrid != null)
					{
						foreach (MyObjectBuilder_CubeBlock block in myObjectBuilder_CubeGrid.CubeBlocks)
						{
<<<<<<< HEAD
							if (checkpoint.Identities.FirstOrDefault((MyObjectBuilder_Identity x) => x.IdentityId == block.BuiltBy) == null)
							{
								block.BuiltBy = 0L;
							}
							if (checkpoint.Identities.FirstOrDefault((MyObjectBuilder_Identity x) => x.IdentityId == block.Owner) == null)
=======
							if (Enumerable.FirstOrDefault<MyObjectBuilder_Identity>((IEnumerable<MyObjectBuilder_Identity>)checkpoint.Identities, (Func<MyObjectBuilder_Identity, bool>)((MyObjectBuilder_Identity x) => x.IdentityId == block.BuiltBy)) == null)
							{
								block.BuiltBy = 0L;
							}
							if (Enumerable.FirstOrDefault<MyObjectBuilder_Identity>((IEnumerable<MyObjectBuilder_Identity>)checkpoint.Identities, (Func<MyObjectBuilder_Identity, bool>)((MyObjectBuilder_Identity x) => x.IdentityId == block.Owner)) == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								block.Owner = 0L;
							}
						}
					}
				}
			}
		}

		private void TestBoardScreen()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				MyVisualScriptLogicProvider.RemoveBoardScreen("ETA", -1L);
				MyVisualScriptLogicProvider.RemoveBoardScreen("RACERS", -1L);
				MyVisualScriptLogicProvider.CreateBoardScreen("ETA", 0.005f, 0.005f, 0.4f, 0.1f, -1L);
				MyVisualScriptLogicProvider.AddColumn("ETA", "TIME", 1f, "Race ending in 1:59", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, -1L);
				MyVisualScriptLogicProvider.CreateBoardScreen("RACERS", 0.005f, 0.05f, 0.4f, 0.3f, -1L);
				MyVisualScriptLogicProvider.AddColumn("RACERS", "RANK", 0.25f, "Rank", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, -1L);
				MyVisualScriptLogicProvider.AddColumn("RACERS", "PLAYER", 0.5f, "Player", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, -1L);
				MyVisualScriptLogicProvider.AddColumn("RACERS", "LAP", 0.25f, "Lap", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P01", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P01", "RANK", "a1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P01", "PLAYER", "palmray", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P01", "LAP", "2/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P01", 0, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P02", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P02", "RANK", "c1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P02", "PLAYER", "KevinAlone", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P02", "LAP", "1/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P02", 50, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P03", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P03", "RANK", "b1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P03", "PLAYER", "KevinOrtiz", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P03", "LAP", "2/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P03", 10, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P04", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P04", "RANK", "c1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P04", "PLAYER", "wells_craig", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P04", "LAP", "2/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P04", 20, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P05", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P05", "RANK", "c1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P05", "PLAYER", "ChavezG", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P05", "LAP", "1/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P05", 30, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P06", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P06", "RANK", "c1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P06", "PLAYER", "ChavezHugo", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P06", "LAP", "1/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P06", 70, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P07", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P07", "RANK", "c1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P07", "PLAYER", "palmray", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P07", "LAP", "1/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P07", 40, -1L);
				MyVisualScriptLogicProvider.AddRow("RACERS", "P08", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P08", "RANK", "c1", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P08", "PLAYER", "wells_bad", -1L);
				MyVisualScriptLogicProvider.SetCell("RACERS", "P08", "LAP", "1/3", -1L);
				MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P08", 60, -1L);
				MyVisualScriptLogicProvider.ShowOrderInColumn("RACERS", "RANK", -1L);
				MyVisualScriptLogicProvider.SortByRanking("RACERS", ascending: true, -1L);
			}, "TestBoardScreen");
		}

		private void TestBoardScreenVisibility(bool visible)
		{
			MyVisualScriptLogicProvider.SetColumnVisibility("RACERS", "PLAYER", visible, -1L);
		}

		private void TeleportOtherClientsToMe()
		{
			Vector3D right = Vector3D.Right;
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer != MySession.Static.LocalHumanPlayer && onlinePlayer.Character != null)
				{
					onlinePlayer.Character.PositionComp.SetPosition(MySession.Static.LocalHumanPlayer.GetPosition() + right);
					right += Vector3D.Right;
				}
			}
		}
	}
}
