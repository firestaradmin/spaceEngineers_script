namespace VRage.Analytics
{
	public class MyReportTypeData
	{
		public MyReportType ReportType;

		public string Arg1;

		public string Arg2;

		public MyReportTypeData()
		{
		}

		public MyReportTypeData(MyReportType reportType, string arg1, string arg2)
		{
			ReportType = reportType;
			Arg1 = arg1;
			Arg2 = arg2;
		}
	}
}
