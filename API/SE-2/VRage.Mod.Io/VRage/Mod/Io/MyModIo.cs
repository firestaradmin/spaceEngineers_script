using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using LitJson;
using VRage.Compression;
using VRage.GameServices;
using VRage.Http;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal static class MyModIo
	{
		public enum Sort
		{
			None,
			Name,
			Subscribers,
			Popular,
			DateUpdated,
			Rating,
			Count
		}

		[Flags]
		private enum AuthenticatedFor
		{
			Consuming = 0x1,
			Sharing = 0x2
		}

		private enum ServiceType
		{
			Steam,
			XboxLive,
			Unknown
		}

		private class MyRequestSetup
		{
			public string Url;

			public HttpMethod Method;

			public List<HttpData> Parameters;

			public void AddParameter(string paramName, object value, HttpDataType type = HttpDataType.GetOrPost)
			{
				Parameters.Add(new HttpData(paramName, value, type));
			}

			public void AddParameters(string paramName, List<string> list, HttpDataType type = HttpDataType.GetOrPost)
			{
				if (paramName == null)
				{
					return;
				}
				foreach (string item in list)
				{
					Parameters.Add(new HttpData(paramName, item, type));
				}
			}
		}

		public enum ReportType
		{
			Generic,
			DMCA,
			Not_Working,
			Rude_Content,
			Illegal_Content,
			Stolen_Content,
			False_Information,
			Other
		}

		private static IMyGameService m_service;

		private static MyModIoServiceInternal m_modIoService;

		private static string m_gameName;

		private static string m_gameId;

		private static string m_apiKey;

		private static string m_testGameId;

		private static string m_testApiKey;

		private static string m_liveGameId;

		private static string m_liveApiKey;

		private const string URL_BASE_LIVE = "mod.io";

		private const string URL_BASE_TEST = "test.mod.io";

		private static string URL_APP_TICKET;

		private static string URL_BASE;

		private static string URL_BASE_API;

		private static string URL_API;

		private static string URL_WEB;

		private static ulong EMAIL_AUTH;

		private const string MODS_API = "games/{0}/mods";

		private const string EDIT_MOD_API = "games/{{0}}/mods/{0}";

		private const string TAGS_API = "games/{{0}}/mods/{0}/tags";

		private const string ADD_MOD_MEDIA_API = "games/{{0}}/mods/{0}/media";

		private const string MY_SUBSCRIPTIONS_API = "me/subscribed";

		private const string ME_API = "me";

		private const string MY_RATINGS_API = "me/ratings";

		private const string RATE_API = "games/{{0}}/mods/{0}/ratings";

		private const string SUBSCRIBE_API = "games/{{0}}/mods/{0}/subscribe";

		private const string MOD_DEPENDENCY_API = "games/{{0}}/mods/{0}/dependencies";

		private const string AUTHENTICATE_STEAM_API = "external/steamauth";

		private const string AUTHENTICATE_XBOX_LIVE_API = "external/xboxauth";

		private const string ADD_MOD_FILE_API = "games/{{0}}/mods/{0}/files";

		private const string LINK_ACCOUNT_API = "external/link";

		private const string EMAIL_REQUEST_API = "oauth/emailrequest";

		private const string EMAIL_EXCHANGE_API = "oauth/emailexchange";

		private const string REPORT_API = "report";

		private const string CONTENT_TYPE_MULTIPART_DATA = "multipart/form-data";

		private const string CONTENT_TYPE_FORM_URLENCODED = "application/x-www-form-urlencoded";

		private static readonly ConcurrentQueue<Action> m_invoke;

		private static readonly string[] m_sortValues;

		private static readonly List<string> m_paramsTemp;

		private static bool m_suspendDownloads;

		private static AuthenticatedFor m_authenticated;

		private static ulong m_authenticatedUserId;

		private static AccessToken m_authenticatedToken;

		private static UserProfile m_authenticatedUserProfile;

		private static readonly Dictionary<int, int> m_ratings;

		public static bool IsAuthenticated => m_authenticated != (AuthenticatedFor)0;

		public static UserProfile UserProfile => m_authenticatedUserProfile;

		public static ulong ServiceUserId => m_authenticatedUserId;

		static MyModIo()
		{
			EMAIL_AUTH = ulong.MaxValue;
			m_invoke = new ConcurrentQueue<Action>();
			m_sortValues = new string[6] { null, "name", "subscribers", "-popular", "-date_updated", "rating" };
			m_paramsTemp = new List<string>();
			m_ratings = new Dictionary<int, int>();
		}

		public static void SuspendDownloads(bool state)
		{
			m_suspendDownloads = state;
		}

		public static void Init(IMyGameService service, MyModIoServiceInternal modIoService, string gameName, string liveGameId, string liveApiKey, string testGameId, string testApiKey, bool testEnabled)
		{
			m_testGameId = testGameId;
			m_testApiKey = testApiKey;
			m_liveGameId = liveGameId;
			m_liveApiKey = liveApiKey;
			SetTestEnvironment(testEnabled);
			m_service = service;
			m_modIoService = modIoService;
			m_gameName = gameName;
			service.OnUserChanged += ServiceOnOnUserChanged;
		}

		private static void ServiceOnOnUserChanged(bool differentUserLoggedIn)
		{
			LogOut();
		}

		public static void InvokeOnMainThread(Action action)
		{
			m_invoke.Enqueue(action);
		}

		public static void Update()
		{
<<<<<<< HEAD
			Action result;
			while (m_invoke.TryDequeue(out result))
			{
				result.InvokeIfNotNull();
=======
			Action handler = default(Action);
			while (m_invoke.TryDequeue(ref handler))
			{
				handler.InvokeIfNotNull();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static ServiceType GetServiceType()
		{
			if (m_service.ServiceName.ToLower() == "steam")
			{
				return ServiceType.Steam;
			}
			if (m_service.ServiceName.ToLower() == "xbox live")
			{
				return ServiceType.XboxLive;
			}
			return ServiceType.Unknown;
		}

		private static string GetUrl(string api, params string[] p)
		{
			string text = string.Format(URL_API + api + "?api_key=" + m_apiKey, m_gameId);
			foreach (string text2 in p)
			{
				if (text2 != null)
				{
					text = text + "&" + text2;
				}
			}
			return text;
		}

		private static void AddAuthorizationHeader(MyRequestSetup request)
		{
			int num = request.Parameters.FindIndex((HttpData x) => x.Name == "Authorization");
			if (num != -1)
			{
				request.Parameters.RemoveAt(num);
			}
			if (m_authenticated != 0)
			{
				request.Parameters.Add(new HttpData("Authorization", "Bearer " + m_authenticatedToken.access_token, HttpDataType.HttpHeader));
			}
		}

		private static MyRequestSetup CreateRequest(string function, HttpMethod method, string contentType, params string[] p)
		{
			MyRequestSetup myRequestSetup = new MyRequestSetup
			{
				Url = GetUrl(function, p),
				Method = method,
				Parameters = new List<HttpData>
				{
					new HttpData("Accept", "application/json", HttpDataType.HttpHeader)
				}
			};
			if (contentType != null)
			{
				myRequestSetup.Parameters.Add(new HttpData("Content-Type", contentType, HttpDataType.HttpHeader));
			}
			AddAuthorizationHeader(myRequestSetup);
			return myRequestSetup;
		}

		private static void SendRequestBlocking<T>(MyRequestSetup request, Action<T, MyGameServiceCallResult> action) where T : class
		{
<<<<<<< HEAD
=======
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string content;
			HttpStatusCode code = MyVRage.Platform.Http.SendRequest(request.Url, request.Parameters.ToArray(), request.Method, out content);
			ResponseJson(request, code, content, action, invokeOnMainThread: false);
		}

		private static void SendRequest<T>(MyRequestSetup request, Action<T, MyGameServiceCallResult> action) where T : class
		{
			MyVRage.Platform.Http.SendRequestAsync(request.Url, request.Parameters.ToArray(), request.Method, delegate(HttpStatusCode x, string y)
			{
<<<<<<< HEAD
=======
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ResponseJson(request, x, y, action);
			});
		}

		private static void ResponseJson<T>(MyRequestSetup request, HttpStatusCode code, string content, Action<T, MyGameServiceCallResult> onDone, bool invokeOnMainThread = true) where T : class
		{
<<<<<<< HEAD
			if (code == HttpStatusCode.Unauthorized)
=======
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			if ((int)code == 401)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				LogOut();
			}
			T data = null;
			MyGameServiceCallResult result = GetError(code, content);
			if (result == MyGameServiceCallResult.OK)
			{
				if (content != null)
				{
					try
					{
						data = JsonMapper.ToObject<T>(content);
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine(ex);
						result = MyGameServiceCallResult.InvalidParam;
					}
				}
				else
				{
					result = MyGameServiceCallResult.InvalidParam;
				}
			}
			string text = "";
			if (request.Parameters != null)
			{
				foreach (HttpData parameter in request.Parameters)
				{
					text = string.Concat(text, parameter.Type, ": ", parameter.Name, " / ", parameter.Value, "\n");
				}
			}
			MyLog.Default.WriteLine(string.Concat("ModIo API call\n-- Request:\nUrl: ", request.Url, "\nMethod: ", request.Method, "\nParameters:\n", text, "\n-- Response:\nCode: ", code, " / ", result, "\nContent: ", content));
			if (onDone == null)
			{
				return;
			}
			if (invokeOnMainThread)
			{
				InvokeOnMainThread(delegate
				{
					onDone(data, result);
				});
			}
			else
			{
				onDone(data, result);
			}
		}

		private static void ResponseDownload(MyRequestSetup request, HttpStatusCode code, Action<MyGameServiceCallResult> onDone)
		{
<<<<<<< HEAD
=======
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGameServiceCallResult result = GetError(code, null);
			InvokeOnMainThread(delegate
			{
				onDone(result);
			});
		}

		private static MyGameServiceCallResult GetError(HttpStatusCode code, string content)
		{
<<<<<<< HEAD
			MyGameServiceCallResult result = MyGameServiceCallResult.OK;
			switch (code)
			{
			case HttpStatusCode.OK:
			case HttpStatusCode.Created:
				return result;
			default:
				return MyGameServiceCallResult.ServiceUnavailable;
			case HttpStatusCode.NoContent:
				return MyGameServiceCallResult.FileNotFound;
			case HttpStatusCode.BadRequest:
				return MyGameServiceCallResult.InvalidParam;
			case HttpStatusCode.Unauthorized:
				return MyGameServiceCallResult.InvalidLoginAuthCode;
			case HttpStatusCode.Forbidden:
				return MyGameServiceCallResult.ParentalControlRestricted;
			case HttpStatusCode.NotFound:
				return MyGameServiceCallResult.FileNotFound;
			case HttpStatusCode.MethodNotAllowed:
				return MyGameServiceCallResult.InvalidParam;
			case HttpStatusCode.NotAcceptable:
				return MyGameServiceCallResult.InvalidParam;
			case HttpStatusCode.Gone:
				return MyGameServiceCallResult.FileNotFound;
			}
=======
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected I4, but got Unknown
			MyGameServiceCallResult result = MyGameServiceCallResult.OK;
			if ((int)code == 200 || (int)code == 201)
			{
				return result;
			}
			if ((int)code != 204)
			{
				return (code - 400) switch
				{
					0 => MyGameServiceCallResult.InvalidParam, 
					1 => MyGameServiceCallResult.InvalidLoginAuthCode, 
					3 => MyGameServiceCallResult.ParentalControlRestricted, 
					4 => MyGameServiceCallResult.FileNotFound, 
					5 => MyGameServiceCallResult.InvalidParam, 
					6 => MyGameServiceCallResult.InvalidParam, 
					10 => MyGameServiceCallResult.FileNotFound, 
					_ => MyGameServiceCallResult.ServiceUnavailable, 
				};
			}
			return MyGameServiceCallResult.FileNotFound;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static string[] CreateParams(Sort sort, string searchString, List<ulong> itemIds, List<string> requiredTags, List<string> excludedTags, uint page, uint itemsPerPage, bool gameId = false)
		{
			m_paramsTemp.Clear();
			if (!string.IsNullOrWhiteSpace(searchString))
			{
				m_paramsTemp.Add("_q=" + searchString);
			}
			if (requiredTags != null && requiredTags.Count > 0)
			{
				m_paramsTemp.Add("tags=" + string.Join(",", requiredTags));
			}
			if (excludedTags != null && excludedTags.Count > 0)
			{
				m_paramsTemp.Add("tags-not-in=" + string.Join(",", excludedTags));
			}
			if (itemIds != null && itemIds.Count > 0)
			{
				m_paramsTemp.Add("id-in=" + string.Join(",", itemIds));
			}
			if (sort != 0)
			{
				m_paramsTemp.Add("_sort=" + m_sortValues[(int)sort]);
			}
			m_paramsTemp.Add("_offset=" + page * itemsPerPage);
			m_paramsTemp.Add("_limit=" + itemsPerPage);
			if (gameId)
			{
				m_paramsTemp.Add("game_id=" + m_gameId);
			}
			return m_paramsTemp.ToArray();
		}

		public static void GetMods(Action<RequestPage<ModProfile>, MyGameServiceCallResult> onDone, Sort sort, string searchString, List<ulong> itemIds, List<string> requiredTags, List<string> excludedTags, uint page, uint itemsPerPage)
		{
			string[] p = CreateParams(sort, searchString, itemIds, requiredTags, excludedTags, page, itemsPerPage);
			SendRequest(CreateRequest("games/{0}/mods", HttpMethod.GET, null, p), onDone);
		}

		public static void GetModDependenciesBlocking(ulong id, Action<RequestPage<ModDependency>, MyGameServiceCallResult> onDone)
		{
			SendRequestBlocking(CreateRequest($"games/{{0}}/mods/{id}/dependencies", HttpMethod.GET, null), onDone);
		}

		public static void DownloadFile(string url, string filename, Action<MyGameServiceCallResult> onDone, Action<ulong> onProgress)
		{
			MyRequestSetup request = new MyRequestSetup
			{
				Method = HttpMethod.GET,
				Url = url
			};
			MyVRage.Platform.Http.DownloadAsync(url, filename, onProgress, delegate(HttpStatusCode x)
			{
<<<<<<< HEAD
=======
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ResponseDownload(request, x, onDone);
			});
		}

		private static void LogOut()
		{
			m_ratings.Clear();
			m_authenticated = (AuthenticatedFor)0;
			m_authenticatedUserId = 0uL;
			m_authenticatedUserProfile = new UserProfile();
		}

		private static void Authenticate(bool share, Action<MyGameServiceCallResult> onDone)
		{
			MyLog.Default.WriteLine("ModIo: Authentication started");
			if (((uint)m_authenticated & (((!share) ? true : true) ? 1u : 0u)) != 0 && (m_authenticatedUserId == EMAIL_AUTH || m_service.UserId == m_authenticatedUserId))
			{
				onDone(MyGameServiceCallResult.OK);
			}
			else if (!m_service.IsActive)
			{
				MyLog.Default.WriteLine("ModIo: No User");
				onDone(MyGameServiceCallResult.NoUser);
			}
			else if (GetServiceType() == ServiceType.Unknown)
			{
				MyLog.Default.WriteLine("ModIo: Unknown service type");
				onDone(MyGameServiceCallResult.SSOUnsupported);
			}
			else
			{
				m_service.RequestEncryptedAppTicket(URL_APP_TICKET, delegate(bool x, string y)
				{
					AuthenticateWithTicket(x, y, share, onDone);
				});
			}
		}

		private static void AuthenticateWithTicket(bool success, string data, bool share, Action<MyGameServiceCallResult> onDone)
		{
			if (success)
			{
				MyRequestSetup myRequestSetup;
				switch (GetServiceType())
				{
				case ServiceType.Steam:
				{
					myRequestSetup = CreateRequest("external/steamauth", HttpMethod.POST, "application/x-www-form-urlencoded");
					myRequestSetup.Parameters.Add(new HttpData("appdata", data, HttpDataType.GetOrPost));
					uint num = DateTime.UtcNow.ToUnixTimestamp() + 86400;
					myRequestSetup.Parameters.Add(new HttpData("date_expires", num, HttpDataType.GetOrPost));
					break;
				}
				case ServiceType.XboxLive:
					myRequestSetup = CreateRequest("external/xboxauth", HttpMethod.POST, "application/x-www-form-urlencoded");
					myRequestSetup.Parameters.Add(new HttpData("xbox_token", data, HttpDataType.GetOrPost));
					myRequestSetup.Parameters.Add(new HttpData("ao_feature", "WMbghHrmTsg70TQcYlQWO9", HttpDataType.GetOrPost));
					break;
				default:
					onDone(MyGameServiceCallResult.SSOUnsupported);
					return;
				}
				SendRequest(myRequestSetup, delegate(AccessToken z, MyGameServiceCallResult w)
				{
					OnAuthenticated(m_service.UserId, share, z, w, onDone);
				});
			}
			else
			{
				MyLog.Default.WriteLine("ModIo: App ticket request failed - " + data);
				onDone(MyGameServiceCallResult.SSOUnsupported);
			}
		}

		private static void OnAuthenticated(ulong userId, bool share, AccessToken token, MyGameServiceCallResult result, Action<MyGameServiceCallResult> onDone)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				m_authenticated |= (AuthenticatedFor)((!share) ? 1 : 2);
				m_authenticatedUserId = userId;
				m_authenticatedToken = token;
				GetMe(delegate(UserProfile x, MyGameServiceCallResult y)
				{
					OnAuthenticatedMe(delegate(MyGameServiceCallResult z)
					{
						if (z == MyGameServiceCallResult.OK)
						{
							UpdateRatingsInternal(0u, onDone);
						}
						else
						{
							onDone.InvokeIfNotNull(z);
						}
					}, x, y);
				});
			}
			else
			{
				LogOut();
				onDone.InvokeIfNotNull(result);
			}
		}

		private static void GetMe(Action<UserProfile, MyGameServiceCallResult> onDone)
		{
			SendRequest(CreateRequest("me", HttpMethod.GET, null), onDone);
		}

		private static void OnAuthenticatedMe(Action<MyGameServiceCallResult> onDone, UserProfile userProfile, MyGameServiceCallResult result)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				m_authenticatedUserProfile = userProfile;
			}
			else
			{
				LogOut();
			}
			onDone.InvokeIfNotNull(result);
		}

		public static void GetMySubscriptions(Action<RequestPage<ModProfile>, MyGameServiceCallResult> onDone, Sort sort, string searchString, List<ulong> itemIds, List<string> requiredTags, List<string> excludedTags, uint page, uint itemsPerPage)
		{
			string[] custom = CreateParams(sort, searchString, itemIds, requiredTags, excludedTags, page, itemsPerPage, gameId: true);
			Authenticate(share: false, delegate(MyGameServiceCallResult x)
			{
				GetMySubscriptionsInternal(x, custom, onDone);
			});
		}

		private static void GetMySubscriptionsInternal(MyGameServiceCallResult result, string[] custom, Action<RequestPage<ModProfile>, MyGameServiceCallResult> onDone)
		{
			if (result != MyGameServiceCallResult.OK)
			{
				onDone.InvokeIfNotNull(null, result);
			}
			else
			{
				SendRequest(CreateRequest("me/subscribed", HttpMethod.GET, null, custom), onDone);
			}
		}

		public static void Subscribe(ulong modId, bool state)
		{
			string function = $"games/{{0}}/mods/{modId}/subscribe";
			Authenticate(share: false, delegate(MyGameServiceCallResult failReason)
			{
				SubscribeInternal(failReason, function, state);
			});
		}

		private static void SubscribeInternal(MyGameServiceCallResult result, string function, bool state)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				SendRequest<ModProfile>(CreateRequest(function, state ? HttpMethod.POST : HttpMethod.DELETE, "application/x-www-form-urlencoded"), null);
			}
		}

		public static void Rate(ulong modId, bool state)
		{
			string function = $"games/{{0}}/mods/{modId}/ratings";
			m_ratings[(int)modId] = (state ? 1 : (-1));
			Authenticate(share: false, delegate(MyGameServiceCallResult failReason)
			{
				RateInternal(failReason, function, state);
			});
		}

		private static void RateInternal(MyGameServiceCallResult result, string function, bool state)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				MyRequestSetup myRequestSetup = CreateRequest(function, HttpMethod.POST, "application/x-www-form-urlencoded");
				myRequestSetup.Parameters.Add(new HttpData("rating", state ? "1" : "-1", HttpDataType.GetOrPost));
				SendRequest<ModProfile>(myRequestSetup, null);
			}
		}

		private static void UpdateRatingsInternal(uint pageIndex, Action<MyGameServiceCallResult> onDone)
		{
			if (pageIndex == 0)
			{
				m_ratings.Clear();
			}
			string[] p = CreateParams(Sort.None, null, null, null, null, pageIndex, 100u, gameId: true);
			SendRequest(CreateRequest("me/ratings", HttpMethod.GET, null, p), delegate(RequestPage<Ratings> x, MyGameServiceCallResult y)
			{
				UpdatedRatingsInternal(pageIndex, onDone, x, y);
			});
		}

		private static void UpdatedRatingsInternal(uint pageIndex, Action<MyGameServiceCallResult> onDone, RequestPage<Ratings> ratings, MyGameServiceCallResult result)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				Ratings[] data = ratings.data;
				foreach (Ratings ratings2 in data)
				{
					m_ratings[ratings2.mod_id] = ratings2.rating;
				}
				if (ratings.result_limit == ratings.data.Length)
				{
					UpdateRatingsInternal(pageIndex + 1, onDone);
					return;
				}
			}
			onDone.InvokeIfNotNull(result);
		}

		public static int GetMyRating(ulong id)
		{
			if (!m_ratings.TryGetValue((int)id, out var value))
			{
				return 0;
			}
			return value;
		}

		private static void EditTagsInternal(int id, ModTag[] originalTags, List<string> newTags)
		{
			string function = $"games/{{0}}/mods/{id}/tags";
			List<string> list = new List<string>();
			ModTag[] array;
			foreach (string newTag in newTags)
			{
				bool flag = false;
				array = originalTags;
				foreach (ModTag modTag in array)
				{
					if (newTag.ToLower() == modTag.name.ToLower())
					{
						flag = true;
						modTag.date_added = -1;
						break;
					}
				}
				if (!flag)
				{
					list.Add(newTag);
				}
			}
			if (list.Count != 0)
			{
				MyRequestSetup myRequestSetup = CreateRequest(function, HttpMethod.POST, "application/x-www-form-urlencoded");
				myRequestSetup.AddParameters("tags[]", list);
				SendRequest<GenericResponse>(myRequestSetup, null);
			}
			List<string> list2 = new List<string>();
			array = originalTags;
			foreach (ModTag modTag2 in array)
			{
				if (modTag2.date_added != -1)
				{
					list2.Add(modTag2.name);
				}
			}
			if (list2.Count != 0)
			{
				MyRequestSetup myRequestSetup2 = CreateRequest(function, HttpMethod.DELETE, "application/x-www-form-urlencoded");
				myRequestSetup2.AddParameters("tags[]", list2);
				SendRequest<GenericResponse>(myRequestSetup2, null);
			}
		}

		public static void AddOrEditMod(ulong id, string name, string description, MyPublishedFileVisibility visibility, List<string> tags, string metadata, string thumbnailFile, string contentFolder, Action<ModProfile, Modfile, MyGameServiceCallResult> onPublished)
		{
			MyRequestSetup request;
			if (id == 0L)
			{
				request = CreateRequest("games/{0}/mods", HttpMethod.POST, "multipart/form-data");
				if (string.IsNullOrEmpty(thumbnailFile))
				{
					onPublished.InvokeIfNotNull(null, null, MyGameServiceCallResult.FileNotFound);
					return;
				}
				request.Parameters.Add(new HttpData("logo", thumbnailFile, HttpDataType.Filename));
			}
			else
			{
				if (!string.IsNullOrEmpty(thumbnailFile))
				{
					string function = $"games/{{0}}/mods/{id}/media";
					MyRequestSetup requestLogo = CreateRequest(function, HttpMethod.POST, "multipart/form-data");
					requestLogo.Parameters.Add(new HttpData("logo", thumbnailFile, HttpDataType.Filename));
					Authenticate(share: true, delegate(MyGameServiceCallResult failReason)
					{
						AddModLogoInternal(failReason, requestLogo, delegate(MyGameServiceCallResult result)
						{
							if (result == MyGameServiceCallResult.OK)
							{
								AddOrEditMod(id, name, description, visibility, tags, metadata, null, contentFolder, onPublished);
							}
							else
							{
								onPublished.InvokeIfNotNull(null, null, result);
							}
						});
					});
					return;
				}
				string function2 = $"games/{{0}}/mods/{id}";
				request = CreateRequest(function2, HttpMethod.PUT, "application/x-www-form-urlencoded");
			}
			request.Parameters.Add(new HttpData("name", name, HttpDataType.GetOrPost));
			request.Parameters.Add(new HttpData("summary", name, HttpDataType.GetOrPost));
			if (!string.IsNullOrEmpty(description))
			{
				request.Parameters.Add(new HttpData("description", description, HttpDataType.GetOrPost));
			}
			request.Parameters.Add(new HttpData("visibility", ((visibility != MyPublishedFileVisibility.Private) ? 1 : 0).ToString(), HttpDataType.GetOrPost));
			request.AddParameters("tags[]", tags);
			if (!string.IsNullOrEmpty(metadata))
			{
				request.Parameters.Add(new HttpData("metadata_blob", metadata, HttpDataType.GetOrPost));
			}
			Authenticate(share: true, delegate(MyGameServiceCallResult failReason)
			{
				AddOrEditModInternal(failReason, tags, request, contentFolder, onPublished);
			});
		}

		private static void AddModLogoInternal(MyGameServiceCallResult result, MyRequestSetup request, Action<MyGameServiceCallResult> action)
		{
			if (result != MyGameServiceCallResult.OK || (m_authenticated & AuthenticatedFor.Sharing) == 0)
			{
				action.InvokeIfNotNull(result);
				return;
			}
			AddAuthorizationHeader(request);
			SendRequest(request, delegate(GenericResponse x, MyGameServiceCallResult y)
			{
				action(y);
			});
		}

		private static void AddOrEditModInternal(MyGameServiceCallResult result, List<string> tags, MyRequestSetup request, string contentFolder, Action<ModProfile, Modfile, MyGameServiceCallResult> onPublished)
		{
			if (result != MyGameServiceCallResult.OK || (m_authenticated & AuthenticatedFor.Sharing) == 0)
			{
				onPublished.InvokeIfNotNull(null, null, result);
				return;
			}
			AddAuthorizationHeader(request);
			SendRequest(request, delegate(ModProfile z, MyGameServiceCallResult w)
			{
				OnAddOrEditMod(tags, z, w, contentFolder, onPublished);
			});
		}

		private static void OnAddOrEditMod(List<string> tags, ModProfile mod, MyGameServiceCallResult result, string contentFolder, Action<ModProfile, Modfile, MyGameServiceCallResult> onPublished)
		{
			if (result != MyGameServiceCallResult.OK)
			{
				onPublished.InvokeIfNotNull(null, null, result);
				return;
			}
			EditTagsInternal(mod.id, mod.tags, tags);
			AddModFile((ulong)mod.id, contentFolder, delegate(Modfile x, MyGameServiceCallResult y)
			{
				onPublished(mod, x, y);
			});
		}

		public static void AddModFile(ulong modId, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			Authenticate(share: true, delegate(MyGameServiceCallResult failReason)
			{
				AddModFileInternal(failReason, modId, contentFolder, onPublished);
			});
		}

		private static void AddModFileInternal(MyGameServiceCallResult result, ulong modId, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			if (result != MyGameServiceCallResult.OK || (m_authenticated & AuthenticatedFor.Sharing) == 0)
			{
				onPublished.InvokeIfNotNull(null, result);
				return;
			}
			string text = contentFolder;
			try
			{
				if (!File.Exists(contentFolder))
				{
					text += ".zip";
					File.Delete(text);
<<<<<<< HEAD
					using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(text, ZipArchiveMode.Create))
					{
						int startIndex = contentFolder.Length + 1;
						string[] files = Directory.GetFiles(contentFolder, "*.*", SearchOption.AllDirectories);
						foreach (string text2 in files)
						{
							using (FileStream fileStream = File.Open(text2, FileMode.Open, FileAccess.Read, FileShare.Read))
							{
								using (Stream destination = myZipArchive.AddFile(text2.Substring(startIndex), CompressionLevel.Optimal).GetStream())
								{
									fileStream.CopyTo(destination, 4096);
								}
							}
						}
=======
					using MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(text, ZipArchiveMode.Create);
					int startIndex = contentFolder.Length + 1;
					string[] files = Directory.GetFiles(contentFolder, "*.*", (SearchOption)1);
					foreach (string text2 in files)
					{
						using FileStream fileStream = File.Open(text2, FileMode.Open, FileAccess.Read, FileShare.Read);
						using Stream destination = myZipArchive.AddFile(text2.Substring(startIndex), (CompressionLevel)0).GetStream();
						fileStream.CopyTo(destination, 4096);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				MyRequestSetup myRequestSetup = CreateRequest($"games/{{0}}/mods/{modId}/files", HttpMethod.POST, "multipart/form-data");
				myRequestSetup.Parameters.Add(new HttpData("filedata", text, HttpDataType.Filename));
				SendRequest(myRequestSetup, onPublished);
			}
			catch
			{
				onPublished(null, MyGameServiceCallResult.AccessDenied);
			}
		}

		public static void LinkAccount(string email, Action<MyGameServiceCallResult> onDone)
		{
			Authenticate(share: false, delegate(MyGameServiceCallResult failReason)
			{
				LinkAccountInternal(failReason, email, onDone);
			});
		}

		private static void LinkAccountInternal(MyGameServiceCallResult result, string email, Action<MyGameServiceCallResult> onDone)
		{
			if (result != MyGameServiceCallResult.OK || (m_authenticated & AuthenticatedFor.Consuming) == 0)
			{
				onDone.InvokeIfNotNull(result);
				return;
			}
			MyRequestSetup myRequestSetup = CreateRequest("external/link", HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.Parameters.Add(new HttpData("service", GetServiceType().ToString().ToLower(), HttpDataType.GetOrPost));
			myRequestSetup.Parameters.Add(new HttpData("service_id", m_service.UserId, HttpDataType.GetOrPost));
			myRequestSetup.Parameters.Add(new HttpData("email", email, HttpDataType.GetOrPost));
			SendRequest(myRequestSetup, delegate(GenericResponse z, MyGameServiceCallResult w)
			{
				onDone(w);
			});
		}

		public static void EmailRequest(string email, Action<MyGameServiceCallResult> onCompleted)
		{
			MyRequestSetup myRequestSetup = CreateRequest("oauth/emailrequest", HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.Parameters.Add(new HttpData("email", email, HttpDataType.GetOrPost));
			SendRequest(myRequestSetup, delegate(GenericResponse z, MyGameServiceCallResult w)
			{
				MyLog.Default.WriteLine("EmailRequest: " + w);
				onCompleted(w);
			});
		}

		public static void EmailExchange(string code, Action<MyGameServiceCallResult> onCompleted)
		{
			MyRequestSetup myRequestSetup = CreateRequest("oauth/emailexchange", HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.Parameters.Add(new HttpData("security_code", code, HttpDataType.GetOrPost));
			SendRequest(myRequestSetup, delegate(AccessToken z, MyGameServiceCallResult w)
			{
				EmailExchangeResponse(z, w, onCompleted);
			});
		}

		private static void EmailExchangeResponse(AccessToken token, MyGameServiceCallResult result, Action<MyGameServiceCallResult> onCompleted)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				m_authenticated = AuthenticatedFor.Consuming | AuthenticatedFor.Sharing;
				m_authenticatedUserId = EMAIL_AUTH;
				m_authenticatedToken = token;
				GetMe(delegate(UserProfile x, MyGameServiceCallResult y)
				{
					EmailExchangeResponseWithMe(x, y, onCompleted);
				});
			}
			else
			{
				onCompleted(result);
			}
		}

		private static void EmailExchangeResponseWithMe(UserProfile userProfile, MyGameServiceCallResult result, Action<MyGameServiceCallResult> onCompleted)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				m_authenticatedUserProfile = userProfile;
			}
			else
			{
				LogOut();
			}
			onCompleted(result);
		}

		private static string GetServiceName()
		{
			switch (GetServiceType())
			{
			case ServiceType.Steam:
				return "steam";
			case ServiceType.XboxLive:
				if (!string.IsNullOrEmpty(m_service.BranchName))
				{
					return "xbox&sbx=" + m_service.BranchName;
				}
				return "xbox";
			default:
				return string.Empty;
			}
		}

		public static string GetWebUrl()
		{
			return string.Format(URL_WEB, m_gameName, GetServiceName());
		}

		public static void SetTestEnvironment(bool testEnabled)
		{
			URL_BASE = (testEnabled ? "test.mod.io" : "mod.io");
			UpdateUrls();
			LogOut();
			URL_APP_TICKET = (testEnabled ? URL_BASE_API : ("https://" + URL_BASE + "/"));
			m_gameId = (testEnabled ? m_testGameId : m_liveGameId);
			m_apiKey = (testEnabled ? m_testApiKey : m_liveApiKey);
		}

		private static void UpdateUrls()
		{
			URL_BASE_API = "https://api." + URL_BASE + "/";
			URL_API = URL_BASE_API + "v1/";
			URL_WEB = "https://{0}." + URL_BASE + "/{{0}}?ref={1}&login=auto";
		}

		public static void ReportMod(Modfile modFile, ReportType reportType, string reason)
		{
			MyRequestSetup myRequestSetup = CreateRequest("report", HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.AddParameter("resource", "mods");
			myRequestSetup.AddParameter("id", modFile.mod_id);
			myRequestSetup.AddParameter("type", (int)reportType);
			myRequestSetup.AddParameter("summary", reason);
			SendRequest<GenericResponse>(myRequestSetup, null);
		}
	}
}
