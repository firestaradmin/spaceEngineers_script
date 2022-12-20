using System;

namespace BulletXNA.LinearMath
{
	public class ObjectArray<T> where T : new()
	{
		private const int _defaultCapacity = 4;

		private static T[] _emptyArray;

		private T[] _items;

		private int _size;

		private int _version;

		public int Capacity
		{
			get
			{
				return _items.Length;
			}
			set
			{
				if (value == _items.Length)
				{
					return;
				}
				if (value < _size)
				{
					throw new Exception("ExceptionResource ArgumentOutOfRange_SmallCapacity");
				}
				if (value > 0)
				{
					T[] array = new T[value];
					if (_size > 0)
					{
						Array.Copy(_items, 0, array, 0, _size);
					}
					_items = array;
				}
				else
				{
					_items = _emptyArray;
				}
			}
		}

		public int Count => _size;

		public T this[int index]
		{
			get
			{
				int num = index + 1 - _size;
				for (int i = 0; i < num; i++)
				{
					Add(new T());
				}
				if (index >= _size)
				{
					throw new Exception("ThrowHelper.ThrowArgumentOutOfRangeException()");
				}
				return _items[index];
			}
			set
			{
				int num = index + 1 - _size;
				for (int i = 0; i < num; i++)
				{
					Add(new T());
				}
				if (index >= _size)
				{
					throw new Exception("ThrowHelper.ThrowArgumentOutOfRangeException()");
				}
				_items[index] = value;
				_version++;
			}
		}

		static ObjectArray()
		{
			_emptyArray = new T[0];
		}

		public ObjectArray()
		{
			_items = _emptyArray;
		}

		public T[] GetRawArray()
		{
			return _items;
		}

		public ObjectArray(int capacity)
		{
			if (capacity < 0)
			{
				throw new Exception("ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_SmallCapacity");
			}
			_items = new T[capacity];
		}

		public void Add(T item)
		{
			if (_size == _items.Length)
			{
				EnsureCapacity(_size + 1);
			}
			_items[_size++] = item;
			_version++;
		}

		public void Swap(int index0, int index1)
		{
			T val = _items[index0];
			_items[index0] = _items[index1];
			_items[index1] = val;
		}

		public void Resize(int newsize)
		{
			Resize(newsize, allocate: true);
		}

		public void Resize(int newsize, bool allocate)
		{
			int count = Count;
			if (newsize < count)
			{
				if (allocate)
				{
					for (int i = newsize; i < count; i++)
					{
						_items[i] = new T();
					}
				}
				else
				{
					for (int j = newsize; j < count; j++)
					{
						_items[j] = default(T);
					}
				}
			}
			else
			{
				if (newsize > Count)
				{
					Capacity = newsize;
				}
				if (allocate)
				{
					for (int k = count; k < newsize; k++)
					{
						_items[k] = new T();
					}
				}
			}
			_size = newsize;
		}

		public void Clear()
		{
			if (_size > 0)
			{
				Array.Clear(_items, 0, _size);
				_size = 0;
			}
			_version++;
		}

		private void EnsureCapacity(int min)
		{
			if (_items.Length < min)
			{
				int num = ((_items.Length == 0) ? 4 : (_items.Length * 2));
				if (num < min)
				{
					num = min;
				}
				Capacity = num;
			}
		}
	}
}
