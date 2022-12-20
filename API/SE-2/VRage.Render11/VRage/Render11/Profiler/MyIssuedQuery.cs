using VRage.Render11.Common;

namespace VRage.Render11.Profiler
{
	internal struct MyIssuedQuery
	{
		internal readonly string Tag;

		internal readonly MyQuery Query;

		internal readonly MyIssuedQueryEnum Info;

		internal readonly float CustomValue;

		internal readonly string Member;

		internal readonly string File;

		internal MyIssuedQuery(MyQuery query, string tag, MyIssuedQueryEnum info, float customValue, string member, string file)
		{
			Tag = tag;
			Query = query;
			Info = info;
			CustomValue = customValue;
			Member = member;
			File = file;
		}
	}
}
