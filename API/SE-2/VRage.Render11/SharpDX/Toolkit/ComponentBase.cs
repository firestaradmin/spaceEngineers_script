using System;
using System.ComponentModel;
<<<<<<< HEAD
=======
using System.Runtime.CompilerServices;
using System.Threading;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

namespace SharpDX.Toolkit
{
	/// <summary>
	/// A lightweight Component base class.
	/// </summary>
	public abstract class ComponentBase : IComponent, IDisposable, INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs while this component is disposing and before it is disposed.
		/// </summary>
		private string name;

		/// <summary>
		/// Gets or sets a value indicating whether the name of this instance is immutable.
		/// </summary>
		/// <value><c>true</c> if this instance is name immutable; otherwise, <c>false</c>.</value>
		private readonly bool isNameImmutable;

		private object tag;

		/// <summary>
		/// Gets the name of this component.
		/// </summary>
		/// <value>The name.</value>
		[DefaultValue(null)]
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (isNameImmutable)
				{
					throw new ArgumentException("Name property is immutable for this instance", "value");
				}
				if (!(name == value))
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		/// <summary>
		/// Gets or sets the tag associated to this object.
		/// </summary>
		/// <value>The tag.</value>
		[Browsable(false)]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return tag;
			}
			set
			{
				if (tag != value)
				{
					tag = value;
					OnPropertyChanged("Tag");
				}
			}
		}

		public ISite Site { get; set; }

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
<<<<<<< HEAD
		public event PropertyChangedEventHandler PropertyChanged;
=======
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PropertyChangedEventHandler val = this.PropertyChanged;
				PropertyChangedEventHandler val2;
				do
				{
					val2 = val;
					PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref Unsafe.As<PropertyChangedEventHandler, PropertyChangedEventHandler>(ref this.PropertyChanged), value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PropertyChangedEventHandler val = this.PropertyChanged;
				PropertyChangedEventHandler val2;
				do
				{
					val2 = val;
					PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref Unsafe.As<PropertyChangedEventHandler, PropertyChangedEventHandler>(ref this.PropertyChanged), value2, val2);
				}
				while (val != val2);
			}
		}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public event EventHandler Disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.ComponentBase" /> class with a mutable name.
		/// </summary>
		protected ComponentBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.ComponentBase" /> class with an immutable name.
		/// </summary>
		/// <param name="name">The name.</param>
		protected ComponentBase(string name)
		{
			if (name != null)
			{
				this.name = name;
				isNameImmutable = true;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
<<<<<<< HEAD
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
=======
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged.Invoke((object)this, new PropertyChangedEventArgs(propertyName));
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public abstract void Dispose();
	}
}
