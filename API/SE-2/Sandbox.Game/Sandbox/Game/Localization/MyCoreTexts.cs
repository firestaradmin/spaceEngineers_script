using System.CodeDom.Compiler;
using VRage.Utils;

namespace Sandbox.Game.Localization
{
	[GeneratedCode("Core Localization Generator Template", "1.0.0.0")]
	public static class MyCoreTexts
	{
		public static readonly MyStringId Blank = default(MyStringId);

		/// <summary>
		///   To help us resolve the problem, please provide a description of what you were doing when it occurred (optional):
		/// </summary>
		public static readonly MyStringId CrashScreen_Detail = MyStringId.GetOrCompute("CrashScreen_Detail");

		/// <summary>
		/// Email (optional)
		/// </summary>
		public static readonly MyStringId CrashScreen_Email = MyStringId.GetOrCompute("CrashScreen_Email");

		/// <summary>
		/// Additionally, you can send us your email in case a member of our support staff needs more information about this error.  By sending the log, I grant my consent to the processing of my personal data (E-mail, {0} ID and IP address) to Keen SWH LTD. United Kingdom and it subsidiaries, in order for these data to be processed for the purpose of tracking the crash and requesting feedback with the intent to improve the game performance. I grant this consent for an indefinite term until my express revocation thereof. I confirm that I have been informed that the provision of these data is voluntary, and that I have the right to request their deletion. Registration is non-transferable. More information about the processing of my personal data in the scope required by legal regulations, in particular Regulation (EU) 2016/679 of the European Parliament and of the Council, can be found as of 25 May 2018 here.
		/// </summary>
		public static readonly MyStringId CrashScreen_EmailText = MyStringId.GetOrCompute("CrashScreen_EmailText");

		/// <summary>
		/// log
		/// </summary>
		public static readonly MyStringId CrashScreen_Log = MyStringId.GetOrCompute("CrashScreen_Log");

		/// <summary>
		/// Space Engineers had a problem and crashed!  We apologize for the inconvenience. Please click Send Log if you would like to help us analyze and fix the problem. For more information, check the log below.
		/// </summary>
		public static readonly MyStringId CrashScreen_MainText = MyStringId.GetOrCompute("CrashScreen_MainText");

		/// <summary>
		/// Space Engineers Crash
		/// </summary>
		public static readonly MyStringId CrashScreen_Title = MyStringId.GetOrCompute("CrashScreen_Title");

		/// <summary>
		/// Send Log
		/// </summary>
		public static readonly MyStringId CrashScreen_Yes = MyStringId.GetOrCompute("CrashScreen_Yes");

		/// <summary>
		/// Sorry, but {0} is already running on your computer. Only one instance allowed.
		/// </summary>
		public static readonly MyStringId Error_AlreadyRunning = MyStringId.GetOrCompute("Error_AlreadyRunning");

<<<<<<< HEAD
		/// <summary>
		/// DirectX Input not initialized
		/// </summary>
		public static readonly MyStringId Error_DirectInputNotInitialized = MyStringId.GetOrCompute("Error_DirectInputNotInitialized");

		/// <summary>
		/// DirectX Input was not initialized. Please, report that on support.keenswh.com.
		/// </summary>
		public static readonly MyStringId Error_DirectInputNotInitialized_Description = MyStringId.GetOrCompute("Error_DirectInputNotInitialized_Description");

		/// <summary>
		/// {1} - application error occurred. For more information please see application log at {0}\n\nThis problem may be caused by your graphics card, because it does not meet minimum requirements. DirectX 11 GPU is required. Please see minimum requirements at {2}\n\nDo you want to submit your configuration to developers?\n\nThank You!\nKeen Software House
		/// </summary>
=======
		public static readonly MyStringId Error_DirectInputNotInitialized = MyStringId.GetOrCompute("Error_DirectInputNotInitialized");

		public static readonly MyStringId Error_DirectInputNotInitialized_Description = MyStringId.GetOrCompute("Error_DirectInputNotInitialized_Description");

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static readonly MyStringId Error_DX11 = MyStringId.GetOrCompute("Error_DX11");

		/// <summary>
		/// {0} - Application Error
		/// </summary>
		public static readonly MyStringId Error_Error_Caption = MyStringId.GetOrCompute("Error_Error_Caption");

		/// <summary>
		/// {0} - application error occurred. For more information please see application log at {1}\n\nIf you want to help us make {0} a better game, you can send us the log file. No personal data or any sensitive information will be submitted.\n\nDo you want to submit this log to developers?\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_Error_Message = MyStringId.GetOrCompute("Error_Error_Message");

		/// <summary>
		/// {0} log upload failed\n{1}\n\nIf you want to help us make {0} a better game, please send the application log to {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_Failed = MyStringId.GetOrCompute("Error_Failed");

		/// <summary>
		/// {1} - application error occurred. For more information please see application log at {0}\n\nIt seems that your graphics card driver is not installed or your graphics card does not meet minimum requirements. Please install driver and see minimum requirements at {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_GPU_Drivers = MyStringId.GetOrCompute("Error_GPU_Drivers");

		/// <summary>
		/// {1} - application error occurred. For more information please see application log at {0}\n\nThis problem may be caused by your graphics card, because it does not meet minimum requirements. Please see minimum requirements at {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_GPU_Low = MyStringId.GetOrCompute("Error_GPU_Low");

		/// <summary>
		/// {1} - application error occurred. For more information please see application log at {0}\n\nThis problem may be caused by your graphics card, because it does not meet minimum requirements. DirectX 11 GPU is required. Please see minimum requirements at {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_GPU_NotDX11 = MyStringId.GetOrCompute("Error_GPU_NotDX11");

		/// <summary>
		/// {0} - Warning!\n\nYour '{1}' graphics card driver is older then required.\n\nWe strongly recommend udpating the driver. Old drivers might crash the game. Would you like to update the driver?\n\nNo - do not display the message again\nCancel - hide the message\n\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_GPU_OldDriver = MyStringId.GetOrCompute("Error_GPU_OldDriver");

		/// <summary>
		/// {1} - application error occurred. For more information please see application log at {0}\n\nThis problem is caused by limited video memory on your system. Please make sure you Graphics card meets minimum requirements: at least 512 MB of video memoryIf you have enabled MODs or playing on server with MODs, this may be the cause. Some mods feature high quality textures which consumes great amount of video memory.\n\nPlease see minimum requirements at {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_GPU_OutOfMemory = MyStringId.GetOrCompute("Error_GPU_OutOfMemory");

		/// <summary>
		/// {1} - Warning!\n\nIt seems that your graphics card is currently unsupported because it does not meet minimum requirements. For more information please see application log at {0}\nPlease see minimum requirements at {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_GPU_Unsupported = MyStringId.GetOrCompute("Error_GPU_Unsupported");

		/// <summary>
		/// {0} Launcher
		/// </summary>
		public static readonly MyStringId Error_Message_Caption = MyStringId.GetOrCompute("Error_Message_Caption");

		/// <summary>
		/// {1} - application error occurred. For more information please see application log at {0}\n\nThis problem is caused by limited memory on your system. In case you're still using 32-bit operating system, upgrade is strongly recommended.\n\nIn case you're using 64-bit operating system, please close other applications (especially internet browser) and try again or install additional system memory.\n\nPlease see minimum requirements at {2}\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_OutOfMemmory = MyStringId.GetOrCompute("Error_OutOfMemmory");

		/// <summary>
		/// We will make sure the issue will be fixed as soon as possible!\n\nThank You!\nKeen Software House
		/// </summary>
		public static readonly MyStringId Error_ThankYou = MyStringId.GetOrCompute("Error_ThankYou");
	}
}
