using System;
using System.Linq;

namespace VRage.Game.VisualScripting
{
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
	public class VisualScriptingEvent : Attribute
	{
		public readonly bool[] IsKey;

		public readonly KeyTypeEnum[] KeyTypes;

		public bool HasKeys
		{
			get
			{
				if (IsKey == null)
				{
					return false;
				}
				bool[] isKey = IsKey;
				for (int i = 0; i < isKey.Length; i++)
				{
					if (isKey[i])
					{
						return true;
					}
				}
				return false;
			}
		}

		public VisualScriptingEvent()
			: this(null)
		{
		}

		public VisualScriptingEvent(bool[] @params, KeyTypeEnum[] keyTypes = null)
		{
			IsKey = @params;
			if (keyTypes == null && IsKey != null)
			{
<<<<<<< HEAD
				keyTypes = Enumerable.Repeat(KeyTypeEnum.Unknown, IsKey.Length).ToArray();
=======
				keyTypes = Enumerable.ToArray<KeyTypeEnum>(Enumerable.Repeat<KeyTypeEnum>(KeyTypeEnum.Unknown, IsKey.Length));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			KeyTypes = keyTypes;
		}
	}
}
