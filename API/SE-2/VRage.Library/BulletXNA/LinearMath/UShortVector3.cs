namespace BulletXNA.LinearMath
{
	public struct UShortVector3
	{
		public ushort X;

		public ushort Y;

		public ushort Z;

		public ushort this[int i]
		{
			get
			{
<<<<<<< HEAD
				switch (i)
				{
				case 0:
					return X;
				case 1:
					return Y;
				case 2:
					return Z;
				default:
					return 0;
				}
=======
				return i switch
				{
					0 => X, 
					1 => Y, 
					2 => Z, 
					_ => 0, 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			set
			{
				switch (i)
				{
				case 0:
					X = value;
					break;
				case 1:
					Y = value;
					break;
				case 2:
					Z = value;
					break;
				}
			}
		}
	}
}
