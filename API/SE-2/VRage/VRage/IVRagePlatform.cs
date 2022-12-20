using VRage.Analytics;
using VRage.Audio;
using VRage.Http;
using VRage.Input;
using VRage.Scripting;
using VRage.Serialization;

namespace VRage
{
	public interface IVRagePlatform
	{
		bool SessionReady { get; set; }

		IVRageWindows Windows { get; }

		IVRageHttp Http { get; }

		IVRageSystem System { get; }

		IVRageRender Render { get; }

		IAnsel Ansel { get; }

		IAfterMath AfterMath { get; }

		IVRageInput Input { get; }

		IVRageInput2 Input2 { get; }

		IMyAudio Audio { get; }

		IMyImeProcessor ImeProcessor { get; }

		IMyCrashReporting CrashReporting { get; }

		IVRageScripting Scripting { get; }

		void Init();

		void Update();

		void Done();

		bool CreateInput2();

		IVideoPlayer CreateVideoPlayer();

		IMyAnalytics InitAnalytics(string projectId, string version);

		/// <summary>
		/// Get the ProtoBuf type model for this platform.
		/// </summary>
		/// <returns>A type model compatible with this platform.</returns>
		IProtoTypeModel GetTypeModel();
	}
}
