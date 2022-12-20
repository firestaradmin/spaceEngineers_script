using VRage.Voxels.Sewing;

namespace VRage.Voxels.Clipmap
{
	/// <summary>
	/// Signature of a cell's vicinity.
	/// </summary>
	public struct MyClipmapCellVicinity
	{
		public readonly uint Lods;

		public unsafe fixed sbyte Versions[8];

		/// <summary>
		/// Invalid vicinity mark.
		/// </summary>
		public static readonly MyClipmapCellVicinity Invalid = new MyClipmapCellVicinity(dummySelector: false);

		public unsafe MyClipmapCellVicinity(VrSewGuide[] guides, MyCellCoord[] coords)
		{
			Lods = 0u;
			fixed (MyClipmapCellVicinity* ptr = &this)
			{
				for (int i = 0; i < 8; i++)
				{
					int lod = MyVoxelClipmap.MakeFulfilled(coords[i]).Lod;
					if (guides[i] == null)
					{
						ptr->Versions[i] = -1;
						continue;
					}
					Lods |= (uint)(lod << i * 4);
					ptr->Versions[i] = (sbyte)(guides[i].Version & 0x7F);
				}
			}
		}

		private unsafe MyClipmapCellVicinity(bool dummySelector)
		{
			Lods = 0u;
			fixed (MyClipmapCellVicinity* ptr = &this)
			{
				for (int i = 0; i < 8; i++)
				{
					ptr->Versions[i] = -1;
				}
			}
		}

		public unsafe bool Equals(MyClipmapCellVicinity other)
		{
			fixed (MyClipmapCellVicinity* ptr = &this)
			{
				if (Lods == other.Lods)
				{
					return *(long*)ptr->Versions == *(long*)other.Versions;
				}
				return false;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is MyClipmapCellVicinity)
			{
				return Equals((MyClipmapCellVicinity)obj);
			}
			return false;
		}

		public unsafe override int GetHashCode()
		{
			fixed (MyClipmapCellVicinity* ptr = &this)
			{
				return (int)(Lods * 397) ^ (int)ptr->Versions;
			}
		}

		public static bool operator ==(MyClipmapCellVicinity left, MyClipmapCellVicinity right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MyClipmapCellVicinity left, MyClipmapCellVicinity right)
		{
			return !(left == right);
		}
	}
}
