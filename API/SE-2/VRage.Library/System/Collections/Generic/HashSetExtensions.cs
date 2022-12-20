namespace System.Collections.Generic
{
	public static class HashSetExtensions
	{
		public static T FirstElement<T>(this HashSet<T> hashset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<T> enumerator = hashset.GetEnumerator();
			try
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException();
				}
				return enumerator.get_Current();
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
