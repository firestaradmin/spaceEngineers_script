using System;
using VRage.Network;
using VRage.Sync;

namespace VRage.Game.ModAPI.Network
{
	/// <summary>
	/// Implementation of synchronized property value.
	/// </summary>
	/// <typeparam name="T">Property type</typeparam>
	/// <typeparam name="TSyncDirection">Allowed synchronization direction, can be <see cref="T:VRage.Sync.SyncDirection.BothWays" /> or <see cref="T:VRage.Sync.SyncDirection.FromServer" />.</typeparam>
	public class MySync<T, TSyncDirection> : Sync<T, TSyncDirection> where TSyncDirection : SyncDirection
	{
		/// <summary>
		/// The unique Id for this Sync object. This is allocated per-entity.
		/// </summary>
		public new int Id => base.Id;

		/// <summary>
		/// The type of the <see cref="P:VRage.Game.ModAPI.Network.MySync`2.Value" /> property.
		/// </summary>
		public new Type ValueType => base.ValueType;

		/// <summary>
		/// Value setter/getter. Does not validate.
		/// </summary>
		public new T Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				base.Value = value;
			}
		}

		/// <summary>
		/// Only synchronizes with server/client if enabled. Default is <see langword="true" />.
		/// </summary>
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		/// <summary>
		/// Validate handler is raised on server after deserialization.
		/// </summary>
		public new SyncValidate<T> Validate
		{
			get
			{
				return base.Validate;
			}
			set
			{
				base.Validate = value;
			}
		}

		/// <summary>
		/// ValueChanged event is raised when value is set locally (setting <see cref="P:VRage.Game.ModAPI.Network.MySync`2.Value" /> property) or remotely (through deserialization).
		/// If validation fails, value is not changed and ValueChanged is not raised.
		/// </summary>
		public new event Action<MySync<T, TSyncDirection>> ValueChanged;

		internal MySync(int id, ISerializerInfo serializeInfo)
			: base(id, serializeInfo)
		{
		}

		/// <summary>
		/// Validates the value and sets it (when valid).
		/// </summary>
		public new void ValidateAndSet(T newValue)
		{
			base.ValidateAndSet(newValue);
		}

		/// <summary>
		/// Sets new value only locally if on client. USE ONLY WITH VALUES YOU GOT FROM THE SERVER VIA OTHER CHANNELS! (e.g. when initializing from object builder)
		/// Behaves like regular set on the server
		/// </summary>
		public new void SetLocalValue(T newValue)
		{
			base.SetLocalValue(newValue);
		}

		protected override void RaiseValueChanged(bool notify)
		{
			base.RaiseValueChanged(notify);
			this.ValueChanged.InvokeIfNotNull(this);
		}

		public static implicit operator T(MySync<T, TSyncDirection> sync)
		{
			return sync.Value;
		}
	}
}
