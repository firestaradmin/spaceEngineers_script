<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Render11.Sprites;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Tools
{
	internal class MyStatsDisplay
	{
		internal enum MyLifetime
		{
			PERSISTENT,
			ONE_FRAME
		}

		internal class MyRecord
		{
			public int Value;

			public MyLifetime Lifetime;
		}

		private static readonly Color COLOR = Color.Yellow;

		private const float FONT_SCALE = 0.6f;

		private static readonly Vector2I SCREEN_OFFSET = new Vector2I(10, 10);

		private const int COLUMN_WIDTH = 300;

		private static readonly SortedSet<string> m_orderedPages = new SortedSet<string>();

		private static readonly Dictionary<string, Dictionary<string, Dictionary<string, MyRecord>>> m_records = new Dictionary<string, Dictionary<string, Dictionary<string, MyRecord>>>();

		private static IEnumerator<string> m_pageEnumerator = GetPageEnumerator();

		private static readonly StringBuilder m_tmpStringBuilder = new StringBuilder(4096);

		private static void WriteInternal(string pageName, string groupName, string name, MyLifetime lifetime, int value)
		{
			if (!m_records.TryGetValue(pageName, out var value2))
			{
				value2 = new Dictionary<string, Dictionary<string, MyRecord>>();
				m_records.Add(pageName, value2);
				m_orderedPages.Add(pageName);
			}
			if (!value2.TryGetValue(groupName, out var value3))
			{
				value3 = new Dictionary<string, MyRecord>();
				value2.Add(groupName, value3);
			}
			if (!value3.TryGetValue(name, out var value4))
			{
				value4 = new MyRecord();
				value3.Add(name, value4);
			}
			value4.Lifetime = lifetime;
			value4.Value = value;
		}

		private static void DrawText(StringBuilder text, int nColumn)
		{
			MyDebugTextHelpers.DrawText(SCREEN_OFFSET + new Vector2I(nColumn * 300, 0), text, COLOR, 0.6f);
		}

		public static void Draw()
		{
			string current = m_pageEnumerator.Current;
			if (current == null)
			{
				return;
			}
			Dictionary<string, Dictionary<string, MyRecord>> dictionary = m_records[current];
			int num = 0;
			int num2 = 0;
			m_tmpStringBuilder.Clear();
			m_tmpStringBuilder.Append(current);
			m_tmpStringBuilder.AppendLine(":");
			num2++;
			num2++;
			int num3 = (int)(MyRender11.ResolutionF.Y / 17f);
			int num4 = num3 - 5;
			foreach (KeyValuePair<string, Dictionary<string, MyRecord>> item in dictionary)
			{
				m_tmpStringBuilder.Append("  ");
				m_tmpStringBuilder.AppendLine(item.Key);
				num2++;
				foreach (KeyValuePair<string, MyRecord> item2 in item.Value)
				{
					MyRecord value = item2.Value;
					m_tmpStringBuilder.AppendFormat("    {0}: {1:#,0}", item2.Key, value.Value);
					m_tmpStringBuilder.AppendLine();
					num2++;
					if (num2 + 1 == num3)
					{
						DrawText(m_tmpStringBuilder, num);
						m_tmpStringBuilder.Clear();
						num++;
						num2 = 0;
					}
					if (value.Lifetime != 0)
					{
						value.Value = 0;
					}
				}
				m_tmpStringBuilder.AppendLine();
				num2++;
				if (num2 + 1 >= num4)
				{
					DrawText(m_tmpStringBuilder, num);
					m_tmpStringBuilder.Clear();
					num++;
					num2 = 0;
				}
			}
			DrawText(m_tmpStringBuilder, num);
			m_tmpStringBuilder.Clear();
		}

		public static void WriteTo(StringBuilder writeTo)
		{
<<<<<<< HEAD
			writeTo.Clear();
			foreach (string orderedPage in m_orderedPages)
			{
				Dictionary<string, Dictionary<string, MyRecord>> dictionary = m_records[orderedPage];
				writeTo.Clear();
				writeTo.Append(orderedPage);
				writeTo.AppendLine(":");
				foreach (KeyValuePair<string, Dictionary<string, MyRecord>> item in dictionary)
				{
					writeTo.Append("  ");
					writeTo.AppendLine(item.Key);
					foreach (KeyValuePair<string, MyRecord> item2 in item.Value)
					{
						MyRecord value = item2.Value;
						writeTo.AppendFormat("    {0}: {1:#,0}", item2.Key, value.Value);
						writeTo.AppendLine();
					}
					writeTo.AppendLine();
				}
			}
=======
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			writeTo.Clear();
			Enumerator<string> enumerator = m_orderedPages.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					Dictionary<string, Dictionary<string, MyRecord>> dictionary = m_records[current];
					writeTo.Clear();
					writeTo.Append(current);
					writeTo.AppendLine(":");
					foreach (KeyValuePair<string, Dictionary<string, MyRecord>> item in dictionary)
					{
						writeTo.Append("  ");
						writeTo.AppendLine(item.Key);
						foreach (KeyValuePair<string, MyRecord> item2 in item.Value)
						{
							MyRecord value = item2.Value;
							writeTo.AppendFormat("    {0}: {1:#,0}", item2.Key, value.Value);
							writeTo.AppendLine();
						}
						writeTo.AppendLine();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void Write(string group, string name, int value, string page = "Common")
		{
			WriteInternal(page, group, name, MyLifetime.ONE_FRAME, value);
		}

		public static void WritePersistent(string group, string name, int value, string page = "Common")
		{
			WriteInternal(page, group, name, MyLifetime.PERSISTENT, value);
		}

		public static void Remove(string groupName = null, string recordName = null, string pageName = null)
		{
			if (pageName == null)
			{
				m_records.Clear();
				return;
			}
			if (groupName == null)
			{
				m_records.Remove(pageName);
				return;
			}
			if (!m_records.TryGetValue(pageName, out var value))
			{
				value = new Dictionary<string, Dictionary<string, MyRecord>>();
				m_records.Add(pageName, value);
				m_orderedPages.Add(pageName);
			}
			if (recordName == null)
			{
				value.Remove(groupName);
				return;
			}
			if (!value.TryGetValue(groupName, out var value2))
			{
				value2 = new Dictionary<string, MyRecord>();
				value.Add(groupName, value2);
			}
			value2.Remove(recordName);
		}

		private static IEnumerator<string> GetPageEnumerator()
		{
<<<<<<< HEAD
			string currentPage = m_orderedPages.FirstOrDefault();
=======
			string currentPage = Enumerable.FirstOrDefault<string>((IEnumerable<string>)m_orderedPages);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (string.IsNullOrEmpty(currentPage))
			{
				yield break;
			}
			do
			{
				yield return currentPage;
<<<<<<< HEAD
				IEnumerable<string> source = m_orderedPages.SkipWhile((string s) => s != currentPage).Skip(1);
				currentPage = source.FirstOrDefault();
=======
				IEnumerable<string> enumerable = Enumerable.Skip<string>(Enumerable.SkipWhile<string>((IEnumerable<string>)m_orderedPages, (Func<string, bool>)((string s) => s != currentPage)), 1);
				currentPage = Enumerable.FirstOrDefault<string>(enumerable);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			while (!string.IsNullOrEmpty(currentPage));
		}

		public static bool MoveToNextPage()
		{
			bool num = m_pageEnumerator.MoveNext();
			if (!num)
			{
				m_pageEnumerator = GetPageEnumerator();
			}
			return num;
		}
	}
}
