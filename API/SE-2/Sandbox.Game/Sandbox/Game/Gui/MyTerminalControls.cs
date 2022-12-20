using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Game;
using VRage.Game.Components;
using VRage.Plugins;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyTerminalControls : MySessionComponentBase, IMyTerminalControls
	{
		private static MyTerminalControls m_instance;

		private Dictionary<Type, Type> m_interfaceCache = new Dictionary<Type, Type>();

		public static MyTerminalControls Static => m_instance;

		private event CustomControlGetDelegate m_customControlGetter;

		public event CustomControlGetDelegate CustomControlGetter
		{
			add
			{
				m_customControlGetter += value;
			}
			remove
			{
				m_customControlGetter -= value;
			}
		}

		private event CustomActionGetDelegate m_customActionGetter;

		public event CustomActionGetDelegate CustomActionGetter
		{
			add
			{
				m_customActionGetter += value;
			}
			remove
			{
				m_customActionGetter -= value;
			}
		}

		public MyTerminalControls()
		{
			m_instance = this;
			m_instance.ScanAssembly(MyPlugins.SandboxGameAssembly);
			m_instance.ScanAssembly(MyPlugins.GameAssembly);
		}

		protected override void UnloadData()
		{
			this.m_customControlGetter = null;
			m_instance = null;
		}

		public List<ITerminalControl> GetControls(Sandbox.ModAPI.IMyTerminalBlock block)
		{
			if (this.m_customControlGetter != null)
			{
				List<IMyTerminalControl> list = Enumerable.ToList<IMyTerminalControl>(Enumerable.Cast<IMyTerminalControl>((IEnumerable)MyTerminalControlFactory.GetControls(block.GetType())));
				this.m_customControlGetter(block, list);
				return Enumerable.ToList<ITerminalControl>(Enumerable.Cast<ITerminalControl>((IEnumerable)list));
			}
			return Enumerable.ToList<ITerminalControl>((IEnumerable<ITerminalControl>)MyTerminalControlFactory.GetControls(block.GetType()));
		}

		public void GetControls<TBlock>(out List<IMyTerminalControl> items)
		{
			items = new List<IMyTerminalControl>();
			if (!IsTypeValid<TBlock>())
			{
				return;
			}
			Type producedType = GetProducedType<TBlock>();
			if (producedType == null)
			{
				return;
			}
			foreach (ITerminalControl control in MyTerminalControlFactory.GetList(producedType).Controls)
			{
				items.Add((IMyTerminalControl)control);
			}
		}

		public void AddControl<TBlock>(IMyTerminalControl item)
		{
			if (IsTypeValid<TBlock>())
			{
				Type producedType = GetProducedType<TBlock>();
				if (!(producedType == null))
				{
					MyTerminalControlFactory.AddControl(producedType, (ITerminalControl)item);
					MyTerminalControlFactory.AddActions(producedType, (ITerminalControl)item);
				}
			}
		}

		public void RemoveControl<TBlock>(IMyTerminalControl item)
		{
			if (IsTypeValid<TBlock>())
			{
				Type producedType = GetProducedType<TBlock>();
				if (!(producedType == null))
				{
					MyTerminalControlFactory.RemoveControl(producedType, item);
				}
			}
		}

		/// <summary>
		/// This will create a control to be added to the terminal screen.  This only really applies to ModAPI, as MyTerminalControlFactory class isn't whitelist
		/// </summary>
		/// <typeparam name="TControl">Interface of control type</typeparam>        
		/// <typeparam name="TBlock"></typeparam>
		/// <param name="id">Identifier of this control</param>
		/// <returns>Interface of created control</returns>
		public TControl CreateControl<TControl, TBlock>(string id)
		{
			if (!IsTypeValid<TBlock>())
			{
				return default(TControl);
			}
			Type producedType = GetProducedType<TBlock>();
			if (producedType == null)
			{
				return default(TControl);
			}
			if (!typeof(MyTerminalBlock).IsAssignableFrom(producedType))
			{
				return default(TControl);
			}
			if (!typeof(IMyTerminalControl).IsAssignableFrom(typeof(TControl)))
			{
				return default(TControl);
			}
			if (!MyTerminalControlFactory.AreControlsCreated(producedType))
			{
				MyTerminalControlFactory.EnsureControlsAreCreated(producedType);
			}
			Type typeFromHandle = typeof(TControl);
			if (typeFromHandle == typeof(IMyTerminalControlTextbox))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlTextbox<>), producedType, new object[3]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlButton))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlButton<>), producedType, new object[5]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					null,
					false
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlCheckbox))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlCheckbox<>), producedType, new object[9]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					false,
					false,
					false,
					1f
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlColor))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlColor<>), producedType, new object[5]
				{
					id,
					MyStringId.NullOrEmpty,
					false,
					1f,
					false
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlCombobox))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlCombobox<>), producedType, new object[3]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlListbox))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlListbox<>), producedType, new object[5]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					false,
					0
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlOnOffSwitch))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlOnOffSwitch<>), producedType, new object[8]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					float.PositiveInfinity,
					false,
					false
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlSeparator))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlSeparator<>), producedType, new object[0]);
			}
			if (typeFromHandle == typeof(IMyTerminalControlSlider))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlSlider<>), producedType, new object[5]
				{
					id,
					MyStringId.NullOrEmpty,
					MyStringId.NullOrEmpty,
					false,
					false
				});
			}
			if (typeFromHandle == typeof(IMyTerminalControlLabel))
			{
				return CreateGenericControl<TControl>(typeof(MyTerminalControlLabel<>), producedType, new object[1] { MyStringId.NullOrEmpty });
			}
			return default(TControl);
		}

		public IMyTerminalControlProperty<TValue> CreateProperty<TValue, TBlock>(string id)
		{
			if (!IsTypeValid<TBlock>())
			{
				return null;
			}
			Type producedType = GetProducedType<TBlock>();
			if (producedType == null)
			{
				return null;
			}
			return (IMyTerminalControlProperty<TValue>)Activator.CreateInstance(typeof(MyTerminalControlProperty<, >).MakeGenericType(producedType, typeof(TValue)), id);
		}

		private Type GetProducedType<TBlock>()
		{
			if (typeof(TBlock).IsInterface)
			{
				return FindTerminalTypeFromInterface<TBlock>();
			}
			return MyCubeBlockFactory.GetProducedType(typeof(TBlock));
		}

		private Type FindTerminalTypeFromInterface<TBlock>()
		{
			Type typeFromHandle = typeof(TBlock);
			if (!typeFromHandle.IsInterface)
			{
				throw new ArgumentException("Given type is not an interface!");
			}
			if (m_interfaceCache.TryGetValue(typeFromHandle, out var value))
			{
				return value;
			}
			ScanAssembly(Assembly.GetExecutingAssembly());
			if (m_interfaceCache.TryGetValue(typeFromHandle, out value))
			{
				return value;
			}
			return null;
		}

		/// <summary>
		/// Searches through an assembly to find Types with the MyTerminalInterfaceAttribute
		/// which allows us to link Mod/Ingame interfaces to the implemneting classes
		/// </summary>
		/// <param name="assembly"></param>
		private void ScanAssembly(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				MyTerminalInterfaceAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<MyTerminalInterfaceAttribute>(type);
				if (customAttribute != null)
				{
					Type[] linkedTypes = customAttribute.LinkedTypes;
					foreach (Type key in linkedTypes)
					{
						m_interfaceCache[key] = type;
					}
				}
			}
		}

		private bool IsTypeValid<TBlock>()
		{
			if (!typeof(TBlock).IsInterface)
			{
				if (!typeof(MyObjectBuilder_TerminalBlock).IsAssignableFrom(typeof(TBlock)))
				{
					return true;
				}
			}
			else if (typeof(Sandbox.ModAPI.Ingame.IMyTerminalBlock).IsAssignableFrom(typeof(TBlock)))
			{
				return true;
			}
			return false;
		}

		private TControl CreateGenericControl<TControl>(Type controlType, Type blockType, object[] args)
		{
			return (TControl)(IMyTerminalControl)Activator.CreateInstance(controlType.MakeGenericType(blockType), args);
		}

		public List<ITerminalAction> GetActions(Sandbox.ModAPI.IMyTerminalBlock block)
		{
			if (this.m_customActionGetter != null)
			{
				List<IMyTerminalAction> list = Enumerable.ToList<IMyTerminalAction>(Enumerable.Cast<IMyTerminalAction>((IEnumerable)MyTerminalControlFactory.GetActions(block.GetType())));
				this.m_customActionGetter(block, list);
				return Enumerable.ToList<ITerminalAction>(Enumerable.Cast<ITerminalAction>((IEnumerable)list));
			}
			return Enumerable.ToList<ITerminalAction>((IEnumerable<ITerminalAction>)MyTerminalControlFactory.GetActions(block.GetType()));
		}

		public void GetActions<TBlock>(out List<IMyTerminalAction> items)
		{
			items = new List<IMyTerminalAction>();
			if (!IsTypeValid<TBlock>())
			{
				return;
			}
			Type producedType = GetProducedType<TBlock>();
			if (producedType == null)
			{
				return;
			}
			foreach (ITerminalAction action in MyTerminalControlFactory.GetList(producedType).Actions)
			{
				items.Add((IMyTerminalAction)action);
			}
		}

		public void AddAction<TBlock>(IMyTerminalAction action)
		{
			if (IsTypeValid<TBlock>())
			{
				Type producedType = GetProducedType<TBlock>();
				if (!(producedType == null))
				{
					MyTerminalControlFactory.GetList(producedType).Actions.Add((ITerminalAction)action);
				}
			}
		}

		public void RemoveAction<TBlock>(IMyTerminalAction action)
		{
			if (IsTypeValid<TBlock>())
			{
				Type producedType = GetProducedType<TBlock>();
				if (!(producedType == null))
				{
					MyTerminalControlFactory.GetList(producedType).Actions.Remove((ITerminalAction)action);
				}
			}
		}

		public IMyTerminalAction CreateAction<TBlock>(string id)
		{
			if (!IsTypeValid<TBlock>())
			{
				return null;
			}
			Type producedType = GetProducedType<TBlock>();
			if (producedType == null)
			{
				return null;
			}
			return (IMyTerminalAction)Activator.CreateInstance(typeof(MyTerminalAction<>).MakeGenericType(producedType), id, new StringBuilder(""), "");
		}
	}
}
