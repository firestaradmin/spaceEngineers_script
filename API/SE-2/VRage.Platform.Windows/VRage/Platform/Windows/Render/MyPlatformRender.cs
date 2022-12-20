using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Platform.Windows.Render
{
	internal static class MyPlatformRender
	{
		internal struct MyDriverDetails
		{
			internal string Name;

			internal string DriverVersion;

			internal string DriverDate;
		}

		private static readonly MyRefreshRatePriorityComparer m_refreshRatePriorityComparer = new MyRefreshRatePriorityComparer();

		private static MyAdapterInfo[] m_adapterInfoList;

		private static readonly Dictionary<int, ModeDescription[]> m_adapterModes = new Dictionary<int, ModeDescription[]>();

		private const int NVIDIA_DRIVER_VERSION1 = 13;

		private const int NVIDIA_DRIVER_VERSION2 = 6909;

		private const string NVIDIA_DRIVER_LINK = "http://www.nvidia.com/Download/index.aspx";

		private const int INTEL_DRIVER_VERSION = 4501;

		private const string INTEL_DRIVER_LINK = "https://downloadcenter.intel.com/product/80939/Graphics-Drivers";

		private const int NVIDIA_DRIVER_AFTERMATH_VERSION = 9764;

		internal static MyLog Log;

		private static bool m_aftermathEnabled;

		private static Factory m_factory;

		private const int BUFFER_COUNT = 2;

		private const Format DX11_BACKBUFFER_FORMAT = Format.R8G8B8A8_UNorm_SRgb;

		private static MyRenderDeviceSettings m_settings;

		private static SwapChain m_swapchain;

		private static ModeDescription? m_changeToFullscreen = null;

		internal static SharpDX.Direct3D11.Device DeviceInstance { get; private set; }

		private static void CheckDrivers(List<MyAdapterInfo> adaptersList)
		{
			for (int i = 0; i < adaptersList.Count; i++)
			{
				MyAdapterInfo value = adaptersList[i];
				if (value.DriverVersion == null)
				{
					continue;
				}
				string[] array = value.DriverVersion.Split('.');
				switch (value.VendorId)
				{
				case VendorIds.Nvidia:
				{
					value.DriverUpdateLink = "http://www.nvidia.com/Download/index.aspx";
					value.DriverUpdateNecessary = true;
					if (array.Length == 4 && int.TryParse(array[2], out var result) && int.TryParse(array[3], out var result2))
					{
						value.DriverUpdateNecessary = result < 13 || (result == 13 && result2 < 6909);
						value.AftermathSupported = result >= 13 && result2 >= 9764;
					}
					break;
				}
				case VendorIds.Intel:
					value.DriverUpdateNecessary = array.Length == 4 && int.Parse(array[3]) < 4501;
					value.DriverUpdateLink = "https://downloadcenter.intel.com/product/80939/Graphics-Drivers";
					break;
				}
				adaptersList[i] = value;
			}
		}

		private static void LogAdapterInfo(MyLog log, ref MyAdapterInfo info)
		{
			log.WriteLine("AdapterInfo = {");
			log.IncreaseIndent();
			log.WriteLine("Name = " + info.Name);
			log.WriteLine("Description = " + info.Description);
			log.WriteLine("Driver version = " + info.DriverVersion);
			log.WriteLine("Driver date = " + info.DriverDate);
			log.WriteLine("DriverUpdateNecessary = " + info.DriverUpdateNecessary);
			log.WriteLine("AftermathSupported = " + info.AftermathSupported);
			log.WriteLine("Adapter id = " + info.AdapterDeviceId);
			log.WriteLine("Supported = " + info.IsDx11Supported);
			log.WriteLine("RAM = " + info.VRAM);
			log.WriteLine("Priority = " + info.Priority);
			log.WriteLine("Multithreaded rendering supported = " + info.MultithreadedRenderingSupported);
			log.WriteLine("Output is attached = " + info.IsOutputAttached);
			log.WriteLine("DesktopBounds = " + info.DesktopBounds);
			log.WriteLine("Display modes = {");
			log.IncreaseIndent();
			log.WriteLine("DXGIOutput id = " + info.OutputId);
			MyDisplayMode[] supportedDisplayModes = info.SupportedDisplayModes;
			for (int i = 0; i < supportedDisplayModes.Length; i++)
			{
				MyDisplayMode myDisplayMode = supportedDisplayModes[i];
				log.WriteLine(myDisplayMode.ToString());
			}
			log.DecreaseIndent();
			log.WriteLine("}");
			log.DecreaseIndent();
			log.WriteLine("}");
		}

		private static void LogAdapters(List<MyAdapterInfo> adaptersList)
		{
			foreach (MyAdapterInfo adapters in adaptersList)
			{
				MyAdapterInfo info = adapters;
				LogAdapterInfo(Log, ref info);
				LogAdapterInfo(MyLog.Default, ref info);
			}
		}

		private static int VendorPriority(VendorIds vendorId)
		{
			switch (vendorId)
			{
			case VendorIds.Amd:
			case VendorIds.Nvidia:
				return 2;
			case VendorIds.Intel:
				return 1;
			default:
				return 0;
			}
		}

		private static void FillFallbackDisplayModes(ref List<MyAdapterInfo> adaptersList)
		{
			MyDisplayMode[] array = null;
			foreach (MyAdapterInfo adapters in adaptersList)
			{
				if (adapters.IsOutputAttached)
				{
					array = adapters.SupportedDisplayModes;
					break;
				}
			}
			if (array == null)
			{
				array = new MyDisplayMode[20]
				{
					new MyDisplayMode(640, 480, 60000, 1000),
					new MyDisplayMode(720, 576, 60000, 1000),
					new MyDisplayMode(800, 600, 60000, 1000),
					new MyDisplayMode(1024, 768, 60000, 1000),
					new MyDisplayMode(1152, 864, 60000, 1000),
					new MyDisplayMode(1280, 720, 60000, 1000),
					new MyDisplayMode(1280, 768, 60000, 1000),
					new MyDisplayMode(1280, 800, 60000, 1000),
					new MyDisplayMode(1280, 960, 60000, 1000),
					new MyDisplayMode(1280, 1024, 60000, 1000),
					new MyDisplayMode(1360, 768, 60000, 1000),
					new MyDisplayMode(1360, 1024, 60000, 1000),
					new MyDisplayMode(1440, 900, 60000, 1000),
					new MyDisplayMode(1600, 900, 60000, 1000),
					new MyDisplayMode(1600, 1024, 60000, 1000),
					new MyDisplayMode(1600, 1200, 60000, 1000),
					new MyDisplayMode(1680, 1200, 60000, 1000),
					new MyDisplayMode(1680, 1050, 60000, 1000),
					new MyDisplayMode(1920, 1080, 60000, 1000),
					new MyDisplayMode(1920, 1200, 60000, 1000)
				};
			}
			for (int i = 0; i < adaptersList.Count; i++)
			{
				if (adaptersList[i].SupportedDisplayModes == null)
				{
					MyAdapterInfo value = adaptersList[i];
					value.SupportedDisplayModes = array;
					adaptersList[i] = value;
				}
			}
		}

		private static string GetReadableAdapterDesc(Adapter adapter)
		{
			string text = adapter.Description.Description;
			int num = text.IndexOf('\0');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			return $"{text} - VendorID={adapter.Description.VendorId}, SubsystemID={adapter.Description.SubsystemId}, DeviceID={adapter.Description.DeviceId}";
		}

		internal static void CreateAdaptersList()
		{
			List<MyAdapterInfo> adaptersList = new List<MyAdapterInfo>();
			Factory factory = GetFactory();
			FeatureLevel[] featureLevels = new FeatureLevel[1] { FeatureLevel.Level_11_0 };
			FeatureLevel[] featureLevels2 = new FeatureLevel[1] { FeatureLevel.Level_11_1 };
			int num = 0;
			bool flag = false;
			Rectangle validDesktopBounds = default(Rectangle);
			Vector2I validDesktopResolution = default(Vector2I);
			Log.WriteLine("All detected adapters:");
			Log.IncreaseIndent();
			Adapter[] adapters = factory.Adapters;
			foreach (Adapter adapter in adapters)
			{
				Log.WriteLine(GetReadableAdapterDesc(adapter));
			}
			Log.DecreaseIndent();
			for (int j = 0; j < factory.Adapters.Length; j++)
			{
				Adapter adapter2 = factory.Adapters[j];
				try
				{
					bool flag2 = false;
					bool supportsCommandLists = false;
					bool flag3 = false;
					SharpDX.Direct3D11.Device device = null;
					try
					{
						device = new SharpDX.Direct3D11.Device(adapter2, DeviceCreationFlags.None, featureLevels);
					}
					catch (Exception arg)
					{
						Log.WriteLine($"Adapter initialisation failed: {arg}");
					}
					flag2 = device != null;
					if (flag2)
					{
						device.CheckThreadingSupport(out var _, out supportsCommandLists);
					}
					device?.Dispose();
					device = null;
					try
					{
						device = new SharpDX.Direct3D11.Device(adapter2, DeviceCreationFlags.None, featureLevels2);
					}
					catch (Exception)
					{
					}
					flag3 = device != null;
					device?.Dispose();
					bool flag4 = adapter2.Description.VendorId == 5140;
					flag2 = flag2 && !flag4;
					ulong vRAM = GetVRAM(adapter2.Description.DedicatedVideoMemory, adapter2.Description.DedicatedSystemMemory);
					ulong sVRAM = GetSVRAM(adapter2.Description.SharedSystemMemory);
					flag2 = flag2 && (vRAM > 500000000 || sVRAM > 500000000);
					string description = $"dev id: {adapter2.Description.DeviceId}, mem: {vRAM}, shared mem: {sVRAM}, Luid: {adapter2.Description.Luid}, rev: {adapter2.Description.Revision}, subsys id: {adapter2.Description.SubsystemId}, vendor id: {adapter2.Description.VendorId}, feature_level 11.1: {flag3}";
					int num2 = adapter2.Description.Description.IndexOf('\0');
					if (num2 == -1)
					{
						num2 = adapter2.Description.Description.Length;
					}
					string text = adapter2.Description.Description.Substring(0, num2);
					VendorIds vendorId = (VendorIds)adapter2.Description.VendorId;
					MyAdapterInfo myAdapterInfo = default(MyAdapterInfo);
					myAdapterInfo.Name = text;
					myAdapterInfo.DeviceName = text;
					myAdapterInfo.VendorId = vendorId;
					myAdapterInfo.DeviceId = adapter2.Description.DeviceId;
					myAdapterInfo.Description = description;
					myAdapterInfo.IsDx11Supported = flag2;
					myAdapterInfo.AdapterDeviceId = j;
					myAdapterInfo.Priority = VendorPriority(vendorId);
					myAdapterInfo.MaxTextureSize = 16384;
					myAdapterInfo.VRAM = vRAM;
					myAdapterInfo.SVRAM = sVRAM;
					myAdapterInfo.Has512MBRam = vRAM > 500000000 || sVRAM > 500000000;
					myAdapterInfo.MultithreadedRenderingSupported = supportsCommandLists;
					myAdapterInfo.ParallelVertexBufferMapping = true;
					myAdapterInfo.BatchedConstantBufferMapping = vendorId == VendorIds.Nvidia && supportsCommandLists && !MyVRage.Platform.System.IsDeprecatedOS && flag3;
					MyAdapterInfo item = myAdapterInfo;
					if (flag2)
					{
						if (item.VendorId == VendorIds.Nvidia)
						{
							item.Quality = MyRenderPresetEnum.NORMAL;
							string text2 = item.DeviceName.ToLower();
							if (text2.Contains("titan") || text2.Contains("quadro"))
							{
								item.Quality = MyRenderPresetEnum.HIGH;
							}
							else
							{
								int k;
								for (k = 0; k < text2.Length && !char.IsDigit(text2[k]); k++)
								{
								}
								int num3 = k;
								for (; k < text2.Length && char.IsDigit(text2[k]); k++)
								{
								}
								int num4 = k - num3;
								int num5 = -1;
								if (num4 > 0)
								{
									try
									{
										num5 = int.Parse(text2.Substring(num3, num4));
									}
									catch (Exception)
									{
										num5 = -1;
									}
								}
								if (num5 > 400 && num5 < 9000)
								{
									int num6 = num5 / 100;
									int num7 = num5 % 100;
									if (Regex.IsMatch(text2, "[0-9][0-9][0-9]m"))
									{
										item.Mobile = true;
										if ((num6 < 9 && num7 <= 60) || (num6 > 8 && num7 < 50))
										{
											item.Quality = MyRenderPresetEnum.LOW;
										}
									}
									else if ((num6 < 6 && num7 <= 50) || (num6 > 5 && num6 < 10 && num7 < 50))
									{
										item.Quality = MyRenderPresetEnum.LOW;
									}
									else if ((num6 == 7 && num7 >= 80) || (num6 >= 8 && num7 >= 70) || (num6 >= 10 && num7 >= 60))
									{
										item.Quality = MyRenderPresetEnum.HIGH;
									}
								}
								else
								{
									Log.WriteLine("Unknown GPU name format: " + text2);
									item.Quality = MyRenderPresetEnum.LOW;
								}
							}
						}
						else if (item.VendorId == VendorIds.Amd)
						{
							item.Quality = MyRenderPresetEnum.NORMAL;
						}
						else
						{
							item.Quality = MyRenderPresetEnum.LOW;
						}
						if (adapter2.Outputs.Length != 0)
						{
							for (int l = 0; l < adapter2.Outputs.Length; l++)
							{
								Output output = adapter2.Outputs[l];
								int num8 = output.Description.DeviceName.IndexOf('\0');
								if (num8 == -1)
								{
									num8 = output.Description.DeviceName.Length;
								}
								string text3 = output.Description.DeviceName.Substring(0, num8);
								string text4 = "\\\\.\\";
								if (text3.StartsWith(text4))
								{
									text3 = text3.Substring(text4.Length, text3.Length - text4.Length);
								}
								item.OutputName = text3;
								item.Name = $"{item.DeviceName} + {item.OutputName}";
								item.OutputId = l;
								ModeDescription[] displayModeList = output.GetDisplayModeList(Format.R8G8B8A8_UNorm_SRgb, DisplayModeEnumerationFlags.Interlaced);
								MyDisplayMode[] array = new MyDisplayMode[displayModeList.Length];
								for (int m = 0; m < displayModeList.Length; m++)
								{
									ModeDescription modeDescription = displayModeList[m];
									array[m] = new MyDisplayMode
									{
										Height = modeDescription.Height,
										Width = modeDescription.Width,
										RefreshRate = modeDescription.RefreshRate.Numerator,
										RefreshRateDenominator = modeDescription.RefreshRate.Denominator
									};
								}
								Array.Sort(array, m_refreshRatePriorityComparer);
								item.SupportedDisplayModes = array;
								item.DesktopBounds = new Rectangle(output.Description.DesktopBounds.Left, output.Description.DesktopBounds.Top, output.Description.DesktopBounds.Right - output.Description.DesktopBounds.Left, output.Description.DesktopBounds.Bottom - output.Description.DesktopBounds.Top);
								item.DesktopResolution = new Vector2I(item.DesktopBounds.Width, item.DesktopBounds.Height);
								item.IsOutputAttached = true;
								if (!flag && item.DesktopBounds.Width != 0 && item.DesktopBounds.Height != 0)
								{
									flag = true;
									validDesktopBounds = item.DesktopBounds;
									validDesktopResolution = item.DesktopResolution;
								}
								m_adapterModes[num] = displayModeList;
								adaptersList.Add(item);
								num++;
							}
						}
						else
						{
							item.OutputName = "FallbackOutput";
							item.OutputId = 0;
							item.SupportedDisplayModes = null;
							item.IsOutputAttached = false;
							adaptersList.Add(item);
							num++;
						}
					}
					else
					{
						item.SupportedDisplayModes = new MyDisplayMode[0];
					}
				}
				catch (Exception ex3)
				{
					string msg = $"Error: Exception in 'CreateAdaptersList()' for device '{GetReadableAdapterDesc(adapter2)}': {ex3.Message}";
					Log.WriteLine(msg);
					Log.WriteLine(ex3.StackTrace);
				}
			}
			FillFallbackDisplayModes(ref adaptersList);
			FillDriverDetails(adaptersList);
			CheckDrivers(adaptersList);
			if (flag)
			{
				FillDesktopBounds(adaptersList, validDesktopBounds, validDesktopResolution);
			}
			else
			{
				Log.WriteLine("No valid Output found!");
				InvalidateAll(adaptersList);
			}
			for (int n = 0; n < adaptersList.Count; n++)
			{
				MyAdapterInfo myAdapterInfo2 = adaptersList[n];
				bool flag5 = myAdapterInfo2.VendorId == VendorIds.Nvidia;
				flag5 = (myAdapterInfo2.IsNvidiaNotebookGpu = flag5 & Regex.IsMatch(myAdapterInfo2.Name, "[0-9][0-9][0-9]M"));
			}
			LogAdapters(adaptersList);
			m_adapterInfoList = adaptersList.ToArray();
		}

		internal static List<MyDriverDetails> GetVideoDriverDetails()
		{
			ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");
			ObjectQuery query = new ObjectQuery("SELECT Name, DriverVersion, DriverDate FROM Win32_VideoController");
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(scope, query).Get();
			List<MyDriverDetails> list = new List<MyDriverDetails>();
			foreach (ManagementBaseObject item2 in managementObjectCollection)
			{
				object obj = item2["Name"];
				if (obj != null)
				{
					MyDriverDetails myDriverDetails = default(MyDriverDetails);
					myDriverDetails.Name = obj.ToString();
					MyDriverDetails item = myDriverDetails;
					object obj2 = item2["DriverVersion"];
					if (obj2 != null)
					{
						item.DriverVersion = obj2.ToString();
					}
					object obj3 = item2["DriverDate"];
					if (obj3 != null)
					{
						item.DriverDate = obj3.ToString();
					}
					list.Add(item);
				}
			}
			return list;
		}

		private static void FillDriverDetails(List<MyAdapterInfo> adaptersList)
		{
			try
			{
				foreach (MyDriverDetails videoDriverDetail in GetVideoDriverDetails())
				{
					for (int i = 0; i < adaptersList.Count; i++)
					{
						if (videoDriverDetail.Name == adaptersList[i].DeviceName)
						{
							MyAdapterInfo value = adaptersList[i];
							value.DriverVersion = videoDriverDetail.DriverVersion;
							value.DriverDate = videoDriverDetail.DriverDate;
							adaptersList[i] = value;
						}
					}
				}
			}
			catch
			{
			}
		}

		private unsafe static ulong GetSVRAM(PointerSize sharedSystemMemory)
		{
			return (ulong)((IntPtr)sharedSystemMemory).ToPointer();
		}

		private unsafe static ulong GetVRAM(PointerSize dedicatedVideoMemory, PointerSize dedicatedSystemMemory)
		{
			return (ulong)((IntPtr)((dedicatedSystemMemory != 0) ? dedicatedSystemMemory : dedicatedVideoMemory)).ToPointer();
		}

		private static void InvalidateAll(List<MyAdapterInfo> adaptersList)
		{
			for (int i = 0; i < adaptersList.Count; i++)
			{
				MyAdapterInfo value = adaptersList[i];
				value.IsDx11Supported = false;
				adaptersList[i] = value;
			}
		}

		private static void FillDesktopBounds(List<MyAdapterInfo> adaptersList, Rectangle validDesktopBounds, Vector2I validDesktopResolution)
		{
			for (int i = 0; i < adaptersList.Count; i++)
			{
				MyAdapterInfo value = adaptersList[i];
				if (!value.IsOutputAttached || value.DesktopBounds.Width == 0 || value.DesktopBounds.Height == 0)
				{
					value.DesktopBounds = validDesktopBounds;
					value.DesktopResolution = validDesktopResolution;
					adaptersList[i] = value;
				}
			}
		}

		internal static void ResetAdaptersList()
		{
			m_adapterInfoList = null;
		}

		internal static MyAdapterInfo[] GetAdaptersList()
		{
			if (m_adapterInfoList == null)
			{
				CreateAdaptersList();
			}
			if (m_adapterInfoList.Length == 0)
			{
				ThrowGpuNotSupported();
			}
			return m_adapterInfoList;
		}

		private static Factory GetFactory()
		{
			return m_factory ?? (m_factory = new Factory1());
		}

		private static void GetAdapter(int adapterOrdinal, out Adapter adapter, out MyAdapterInfo adapterInfo)
		{
			Factory factory = GetFactory();
			MyAdapterInfo[] adaptersList = GetAdaptersList();
			int adapterDeviceId = adaptersList[adapterOrdinal].AdapterDeviceId;
			if (adapterDeviceId >= factory.Adapters.Length)
			{
				throw new MyRenderException("Invalid adapter id binding!", MyRenderExceptionEnum.GpuNotSupported);
			}
			adapter = factory.Adapters[adapterDeviceId];
			adapterInfo = adaptersList[adapterOrdinal];
		}

		private static int ValidateAdapterIndex(int adapterIndex)
		{
			MyAdapterInfo[] adaptersList = GetAdaptersList();
			if (adapterIndex < 0 || adaptersList.Length <= adapterIndex || !adaptersList[adapterIndex].IsDx11Supported)
			{
				return 0;
			}
			return adapterIndex;
		}

		private static bool ValidateResolution(MyAdapterInfo adapter, int width, int height, int refreshRate)
		{
			return adapter.GetDisplayMode(width, height, refreshRate).HasValue;
		}

		private static ModeDescription GetCurrentModeDescriptor(MyRenderDeviceSettings settings)
		{
			if (m_adapterModes.TryGetValue(settings.AdapterOrdinal, out var value))
			{
				for (int i = 0; i < value.Length; i++)
				{
					if (value[i].Height == settings.BackBufferHeight && value[i].Width == settings.BackBufferWidth && (settings.RefreshRate == 0 || value[i].RefreshRate.Numerator == settings.RefreshRate))
					{
						return value[i];
					}
				}
			}
			Log.WriteLine("Mode not found: " + settings.BackBufferWidth + "x" + settings.BackBufferHeight + " @ " + settings.RefreshRate);
			ModeDescription result = default(ModeDescription);
			result.Format = Format.R8G8B8A8_UNorm_SRgb;
			result.Width = settings.BackBufferWidth;
			result.Height = settings.BackBufferHeight;
			result.RefreshRate.Numerator = settings.RefreshRate;
			result.RefreshRate.Denominator = 1000;
			result.Scaling = DisplayModeScaling.Unspecified;
			result.ScanlineOrdering = DisplayModeScanlineOrder.Unspecified;
			return result;
		}

		internal static void CreateRenderDevice(ref MyRenderDeviceSettings? settings, IntPtr windowHandle, out object deviceInstance, out object swapChain)
		{
			if (!settings.HasValue)
			{
				m_settings = GetDefaultDeviceSettings();
			}
			else
			{
				m_settings = settings.Value;
			}
			Log.WriteLine("CreateDeviceInternal - CheckSettings()");
			CheckSettings(m_settings);
			Log.WriteLine("CreateDeviceInternal - GetAdapter()");
			GetAdapter(m_settings.AdapterOrdinal, out var adapter, out var adapterInfo);
			Log.WriteLine("CreateDeviceInternal - FixSettings()");
			FixSettings(ref m_settings, adapter, adapterInfo, GetAdaptersList());
			Log.WriteLine("CreateDeviceInternal - Create device");
			DeviceCreationFlags flags = DeviceCreationFlags.None;
			DeviceInstance = new SharpDX.Direct3D11.Device(adapter, flags, FeatureLevel.Level_11_0);
			DeviceInstance = new SharpDX.Direct3D11.Device1(DeviceInstance.NativePointer);
			try
			{
				Log.WriteLine("CreateDeviceInternal Steam Overlay integration");
				using (new SharpDX.Direct3D11.Device(DriverType.Hardware, flags, FeatureLevel.Level_11_0))
				{
				}
				Log.WriteLine("CreateDeviceInternal Steam Overlay OK");
			}
			catch
			{
				Log.WriteLine("CreateDeviceInternal Steam Overlay Failed");
			}
			m_aftermathEnabled = adapterInfo.AftermathSupported;
			if (m_aftermathEnabled)
			{
				Log.WriteLine("Aftermath init");
				int num = MyVRage.Platform.AfterMath.Init(DeviceInstance.NativePointer);
				Log.WriteLine("Aftermath result: " + num);
			}
			CreateSwapChain(windowHandle);
			settings = m_settings;
			m_settings.WindowMode = MyWindowModeEnum.Window;
			deviceInstance = DeviceInstance;
			swapChain = m_swapchain;
		}

		internal static void DisposeRenderDevice()
		{
			if (m_aftermathEnabled)
			{
				Log.WriteLine("Aftermath shutdown");
				MyVRage.Platform.AfterMath.Shutdown();
			}
			DisposeSwapChain();
			if (DeviceInstance != null)
			{
				DeviceInstance.Dispose();
				DeviceInstance = null;
			}
			if (m_factory != null)
			{
				m_factory.Dispose();
				m_factory = null;
			}
		}

		private static void ThrowGpuNotSupported()
		{
			throw new MyRenderException("No supported device detected!\nPlease apply windows updates and update to latest graphics drivers.", MyRenderExceptionEnum.GpuNotSupported);
		}

		private static void CheckSettings(MyRenderDeviceSettings settings)
		{
			MyAdapterInfo[] adaptersList = GetAdaptersList();
			settings.AdapterOrdinal = ValidateAdapterIndex(settings.AdapterOrdinal);
			if (settings.AdapterOrdinal == -1)
			{
				ThrowGpuNotSupported();
			}
			if (settings.AdapterOrdinal >= adaptersList.Length)
			{
				ThrowGpuNotSupported();
			}
		}

		private static void FixSettingsForOldDriver(ref MyRenderDeviceSettings settings, MyAdapterInfo adapterInfo)
		{
			Log.WriteLine("!!! Forcing window mode because of old driver");
			settings.WindowMode = MyWindowModeEnum.Window;
			Vector2I fixedWindowResolution = MyRenderProxyUtils.GetFixedWindowResolution(new Vector2I(settings.BackBufferWidth, settings.BackBufferHeight), adapterInfo);
			settings.BackBufferWidth = fixedWindowResolution.X;
			settings.BackBufferHeight = fixedWindowResolution.Y;
		}

		private static void FixSettings(ref MyRenderDeviceSettings settings, Adapter adapter, MyAdapterInfo currentAdapterInfo, MyAdapterInfo[] adapterInfos)
		{
			if (!settings.DisableWindowedModeForOldDriver)
			{
				if (currentAdapterInfo.IsNvidiaNotebookGpu)
				{
					for (int i = 0; i < adapterInfos.Length; i++)
					{
						if (adapterInfos[i].DriverUpdateNecessary)
						{
							FixSettingsForOldDriver(ref settings, currentAdapterInfo);
							break;
						}
					}
				}
				else if (currentAdapterInfo.DriverUpdateNecessary)
				{
					FixSettingsForOldDriver(ref settings, currentAdapterInfo);
				}
			}
			int refreshRate = ((settings.WindowMode != MyWindowModeEnum.FullscreenWindow) ? settings.RefreshRate : 0);
			bool flag = ValidateResolution(currentAdapterInfo, settings.BackBufferWidth, settings.BackBufferHeight, refreshRate);
			if (settings.WindowMode != 0 && !flag)
			{
				Log.WriteLine("!!! Invalid resolution in settings - resetting to desktop resolution");
				settings.BackBufferWidth = currentAdapterInfo.DesktopResolution.X;
				settings.BackBufferHeight = currentAdapterInfo.DesktopResolution.Y;
				settings.RefreshRate = GetDefaultDeviceSettings().RefreshRate;
			}
			if (settings.WindowMode == MyWindowModeEnum.Fullscreen && adapter.Outputs.Length == 0)
			{
				Log.WriteLine("!!! Fullscreen is not acceptable (no output), therefore switching to fullscreen window");
				settings.WindowMode = MyWindowModeEnum.FullscreenWindow;
			}
		}

		private static MyRenderDeviceSettings GetDefaultDeviceSettings()
		{
			MyAdapterInfo[] adaptersList = GetAdaptersList();
			MyDisplayMode? displayMode = adaptersList[0].GetDisplayMode(adaptersList[0].DesktopResolution.X, adaptersList[0].DesktopResolution.Y, 0);
			MyRenderDeviceSettings result = default(MyRenderDeviceSettings);
			result.AdapterOrdinal = 0;
			result.BackBufferWidth = adaptersList[0].DesktopResolution.X;
			result.BackBufferHeight = adaptersList[0].DesktopResolution.Y;
			result.WindowMode = MyWindowModeEnum.FullscreenWindow;
			result.RefreshRate = (int)(displayMode.HasValue ? (displayMode.Value.RefreshRateF * 1000f) : 60000f);
			result.VSync = 0;
			return result;
		}

		private static void CreateSwapChain(IntPtr windowHandle)
		{
			DisposeSwapChain();
			Log.WriteLine("CreateDeviceInternal create swapchain");
			if (m_swapchain == null)
			{
				SwapChainDescription swapChainDescription = default(SwapChainDescription);
				swapChainDescription.BufferCount = 2;
				swapChainDescription.Flags = SwapChainFlags.AllowModeSwitch;
				swapChainDescription.IsWindowed = true;
				swapChainDescription.ModeDescription = GetCurrentModeDescriptor(m_settings);
				swapChainDescription.SampleDescription.Count = 1;
				swapChainDescription.SampleDescription.Quality = 0;
				swapChainDescription.OutputHandle = windowHandle;
				swapChainDescription.Usage = Usage.ShaderInput | Usage.RenderTargetOutput;
				swapChainDescription.SwapEffect = SwapEffect.Discard;
				SwapChainDescription swapChainDescription2 = swapChainDescription;
				Factory factory = GetFactory();
				try
				{
					m_swapchain = new SwapChain(factory, DeviceInstance, swapChainDescription2);
				}
				catch (Exception ex)
				{
					Log.WriteLine("SwapChain factory = " + factory);
					Log.WriteLine("SwapChain Device = " + DeviceInstance);
					PrintSwapChainDescriptionToLog(swapChainDescription2);
					throw ex;
				}
				factory.MakeWindowAssociation(windowHandle, WindowAssociationFlags.IgnoreAll);
			}
		}

		private static void DisposeSwapChain()
		{
			if (m_swapchain != null)
			{
				m_swapchain.Dispose();
				m_swapchain = null;
			}
		}

		internal static void RestoreFullscreenMode()
		{
			if (!m_changeToFullscreen.HasValue && m_swapchain != null && !m_swapchain.IsFullScreen && m_settings.WindowMode == MyWindowModeEnum.Fullscreen)
			{
				m_changeToFullscreen = GetCurrentModeDescriptor(m_settings);
			}
		}

		internal static void HandleFocusMessage(MyWindowFocusMessage msg)
		{
			if (MyRenderProxy.RenderThread.CurrentSettings.WindowMode == MyWindowModeEnum.Fullscreen && msg == MyWindowFocusMessage.SetFocus)
			{
				MyRenderProxy.RenderThread.UpdateSize();
				RestoreFullscreenMode();
			}
		}

		public static void ApplySettings(MyRenderDeviceSettings? settings)
		{
			if (settings.HasValue)
			{
				ModeDescription newTargetParametersRef = GetCurrentModeDescriptor(settings.Value);
				if (settings.Value.WindowMode == MyWindowModeEnum.Fullscreen)
				{
					if (settings.Value.WindowMode != m_settings.WindowMode)
					{
						m_changeToFullscreen = newTargetParametersRef;
					}
					else
					{
						m_swapchain.ResizeTarget(ref newTargetParametersRef);
						newTargetParametersRef.RefreshRate.Denominator = 0;
						newTargetParametersRef.RefreshRate.Numerator = 0;
						m_swapchain.ResizeTarget(ref newTargetParametersRef);
					}
				}
				else if (settings.Value.WindowMode != m_settings.WindowMode && m_settings.WindowMode == MyWindowModeEnum.Fullscreen)
				{
					m_swapchain.ResizeTarget(ref newTargetParametersRef);
					m_swapchain.SetFullscreenState(false, null);
				}
				else if (settings.Value.WindowMode == MyWindowModeEnum.FullscreenWindow)
				{
					m_swapchain.ResizeTarget(ref newTargetParametersRef);
					m_swapchain.SetFullscreenState(false, null);
				}
				m_settings = settings.Value;
			}
			else
			{
				TryChangeToFullscreen();
			}
		}

		private static void TryChangeToFullscreen()
		{
			if (!m_changeToFullscreen.HasValue)
			{
				return;
			}
			ModeDescription newTargetParametersRef = m_changeToFullscreen.Value;
			try
			{
				int adapterDeviceId = m_adapterInfoList[m_settings.AdapterOrdinal].AdapterDeviceId;
				int outputId = m_adapterInfoList[m_settings.AdapterOrdinal].OutputId;
				m_swapchain.ResizeTarget(ref newTargetParametersRef);
				m_swapchain.SetFullscreenState(true, (GetFactory().Adapters[adapterDeviceId].Outputs.Length > outputId) ? GetFactory().Adapters[adapterDeviceId].Outputs[outputId] : null);
				newTargetParametersRef.RefreshRate.Numerator = 0;
				newTargetParametersRef.RefreshRate.Denominator = 0;
				m_swapchain.ResizeTarget(ref newTargetParametersRef);
				m_changeToFullscreen = null;
				Log.WriteLine("DXGI SetFullscreenState succeded");
			}
			catch (SharpDXException ex)
			{
				if (ex.ResultCode == SharpDX.DXGI.ResultCode.Unsupported)
				{
					m_changeToFullscreen = null;
				}
				Log.WriteLine("TryChangeToFullscreen failed with " + ex.ResultCode);
			}
		}

		private static void PrintSwapChainDescriptionToLog(SwapChainDescription scDesc)
		{
			Log.WriteLine("SwapChainDescription.BufferCount = " + scDesc.BufferCount);
			Log.WriteLine("SwapChainDescription.Flags = " + scDesc.Flags);
			Log.WriteLine("SwapChainDescription.ModeDescription.Format = " + scDesc.ModeDescription.Format);
			Log.WriteLine("SwapChainDescription.ModeDescription.Height = " + scDesc.ModeDescription.Height);
			Log.WriteLine("SwapChainDescription.ModeDescription.Width = " + scDesc.ModeDescription.Width);
			Log.WriteLine("SwapChainDescription.ModeDescription.RefreshRate.Numerator = " + scDesc.ModeDescription.RefreshRate.Numerator);
			Log.WriteLine("SwapChainDescription.ModeDescription.RefreshRate.Denominator = " + scDesc.ModeDescription.RefreshRate.Denominator);
			Log.WriteLine("SwapChainDescription.ModeDescription.Scaling = " + scDesc.ModeDescription.Scaling);
			Log.WriteLine("SwapChainDescription.ModeDescription.ScanlineOrdering = " + scDesc.ModeDescription.ScanlineOrdering);
			Log.WriteLine("SwapChainDescription.SampleDescription.Count = " + scDesc.SampleDescription.Count);
			Log.WriteLine("SwapChainDescription.SampleDescription.Quality = " + scDesc.SampleDescription.Quality);
			Log.WriteLine("SwapChainDescription.BufferCount = " + scDesc.BufferCount);
			Log.WriteLine("SwapChainDescription.Usage = " + scDesc.Usage);
			Log.WriteLine("SwapChainDescription.SwapEffect = " + scDesc.SwapEffect);
		}
	}
}
