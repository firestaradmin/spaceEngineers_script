using System;
using System.Text;

namespace ParallelTasks
{
	/// <summary>
	/// An exception thrown when an unhandled exception is thrown within a task.
	/// </summary>
	public class TaskException : Exception
	{
		/// <summary>
		/// Gets an array containing any unhandled exceptions that were thrown by the task.
		/// </summary>
		public Exception[] InnerExceptions { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="T:ParallelTasks.TaskException" /> class.
		/// </summary>
		/// <param name="inner">The unhandled exceptions thrown by the task.</param>
		public TaskException(Exception[] inner)
			: base("An exception(s) was thrown while executing a task.", null)
		{
			InnerExceptions = inner ?? Array.Empty<Exception>();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.ToString());
			int num = 0;
			while (true)
			{
				int num2 = num;
				Exception[] innerExceptions = InnerExceptions;
				if (num2 >= ((innerExceptions != null) ? innerExceptions.Length : 0))
				{
					break;
				}
				stringBuilder.AppendFormat("Task exception, inner exception {0}:", num.ToString());
				stringBuilder.AppendLine();
				Exception ex = InnerExceptions[num];
				try
				{
					stringBuilder.Append(ex.ToString());
				}
				catch
				{
					try
					{
						stringBuilder.Append(ex.StackTrace);
					}
					catch
					{
						stringBuilder.Append("Inner exception dump failed");
					}
				}
				stringBuilder.AppendLine();
				num++;
			}
			return stringBuilder.ToString();
		}
	}
}
