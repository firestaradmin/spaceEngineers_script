namespace VRageRender
{
	internal struct MyVertexInputComponent
	{
		internal MyVertexInputComponentType Type;

		internal int Slot;

		internal MyVertexInputComponentFreq Freq;

		internal MyVertexInputComponent(MyVertexInputComponentType type)
		{
			Type = type;
			Slot = 0;
			Freq = MyVertexInputComponentFreq.PER_VERTEX;
		}

		internal MyVertexInputComponent(MyVertexInputComponentType type, MyVertexInputComponentFreq freq)
		{
			Type = type;
			Slot = 0;
			Freq = freq;
		}

		internal MyVertexInputComponent(MyVertexInputComponentType type, int slot)
		{
			Type = type;
			Slot = slot;
			Freq = MyVertexInputComponentFreq.PER_VERTEX;
		}

		internal MyVertexInputComponent(MyVertexInputComponentType type, int slot, MyVertexInputComponentFreq freq)
		{
			Type = type;
			Slot = slot;
			Freq = freq;
		}

		public override string ToString()
		{
			return $"<{Type}, {Slot}, {Freq}>";
		}

		public int CompareTo(MyVertexInputComponent item)
		{
			if (Type == item.Type)
			{
				if (Slot == item.Slot)
				{
					return Freq - item.Freq;
				}
				return Slot - item.Slot;
			}
			return Type - item.Type;
		}
	}
}
