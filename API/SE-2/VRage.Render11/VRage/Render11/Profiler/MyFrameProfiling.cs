<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using SharpDX.Direct3D11;
using VRage.Render11.Common;

namespace VRage.Render11.Profiler
{
	internal class MyFrameProfiling
	{
		internal MyQuery Disjoint;

		internal readonly Queue<MyIssuedQuery> Issued = new Queue<MyIssuedQuery>(128);

		internal bool IsFinished()
		{
<<<<<<< HEAD
			if (MyImmediateRC.RC.IsDataAvailable(Disjoint.Query, AsynchronousFlags.DoNotFlush))
			{
				foreach (MyIssuedQuery item in Issued)
				{
					if (!MyImmediateRC.RC.IsDataAvailable((Query)item.Query, AsynchronousFlags.DoNotFlush))
					{
						return false;
					}
				}
=======
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (MyImmediateRC.RC.IsDataAvailable(Disjoint.Query, AsynchronousFlags.DoNotFlush))
			{
				Enumerator<MyIssuedQuery> enumerator = Issued.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyIssuedQuery current = enumerator.get_Current();
						if (!MyImmediateRC.RC.IsDataAvailable((Query)current.Query, AsynchronousFlags.DoNotFlush))
						{
							return false;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return true;
			}
			return false;
		}

		internal void Clear()
		{
			if (Disjoint != null)
			{
				MyQueryFactory.RelaseDisjointQuery(Disjoint);
				Disjoint = null;
			}
<<<<<<< HEAD
			while (Issued.Count > 0)
=======
			while (Issued.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyQueryFactory.RelaseTimestampQuery(Issued.Dequeue().Query);
			}
		}
	}
}
