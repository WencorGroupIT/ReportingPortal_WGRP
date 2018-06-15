using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IzendaEmbedded.IzendaBoundary
{
	public class WebApiService
	{
		private static WebApiService _instance;
		private readonly string _basedUri;

		private WebApiService(string basedUri)
		{
			_basedUri = basedUri;
		}

		public static WebApiService Instance =>
			_instance ?? (_instance = new WebApiService(ConfigurationManager.AppSettings["IzendaApiUrl"]));

		public async Task<T> GetAsync<T>(string action, string authToken = null, Dictionary<string, object> parameters = null)
		{
			using (var httpClient = GetHttpClient(authToken))
			{
				var url = BuildActionUri(action, parameters);

				var httpResponse = await httpClient.GetAsync(url);
				try
				{
					httpResponse.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new WebApiException(url, httpResponse.StatusCode, ex);
				}

				var responeJson = await httpResponse.Content.ReadAsStringAsync();
				if (responeJson != "null") return JsonConvert.DeserializeObject<T>(responeJson);
				return default(T);
			}
		}

		public async Task PostAsync<T>(string action, T data, string authToken = null)
		{
			using (var httpClient = GetHttpClient(authToken))
			{
				var url = BuildActionUri(action);
				var httpResponse = await httpClient.PostAsJsonAsync(url, data);
				try
				{
					httpResponse.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new WebApiException(url, httpResponse.StatusCode, ex);
				}
			}
		}

		public async Task<TResult> PostReturnValueAsync<TResult, T>(string action, T data, string authToken = null)
		{
			using (var httpClient = GetHttpClient(authToken))
			{
				var url = BuildActionUri(action);
				var httpResponse = await httpClient.PostAsJsonAsync(url, data);
				try
				{
					httpResponse.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new WebApiException(url, httpResponse.StatusCode, ex);
				}

				var settings = new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					MissingMemberHandling = MissingMemberHandling.Ignore
				};

				var responseJson = await httpResponse.Content.ReadAsStringAsync();
				if (responseJson != "null")
					return JsonConvert.DeserializeObject<TResult>(responseJson, settings);
				return default(TResult);
			}
		}

		private HttpClient GetHttpClient(string authToken = null)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (!string.IsNullOrWhiteSpace(authToken)) client.DefaultRequestHeaders.Add("access_token", authToken);

			return client;
		}

		public async Task DeleteAsync(string action, string authToken = null)
		{
			using (var httpClient = GetHttpClient(authToken))
			{
				var url = BuildActionUri(action);
				var httpResponse = await httpClient.DeleteAsync(url);
				try
				{
					httpResponse.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new WebApiException(url, httpResponse.StatusCode, ex);
				}
			}
		}

		public async Task<TResult> DeleteReturnValueAsync<TResult>(string action, string authToken = null)
		{
			using (var httpClient = GetHttpClient(authToken))
			{
				var url = BuildActionUri(action);
				var httpResponse = await httpClient.DeleteAsync(url);
				try
				{
					httpResponse.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new WebApiException(url, httpResponse.StatusCode, ex);
				}

				var responseJson = await httpResponse.Content.ReadAsStringAsync();
				return responseJson != "null" ? JsonConvert.DeserializeObject<TResult>(responseJson) : default(TResult);
			}
		}

		private string BuildActionUri(string action, Dictionary<string, object> parameters = null)
		{
			var url = _basedUri + action;
			if (parameters != null)
				url = AddUrlParams(url, parameters);

			return url;
		}

		private static string AddUrlParams(string url, Dictionary<string, object> parameters)
		{
			var stringBuilder = new StringBuilder(url);
			var hasFirstParam = url.Contains("?");

			foreach (var parameter in parameters)
			{
				var format = hasFirstParam ? "&{0}={1}" : "?{0}={1}";
				stringBuilder.AppendFormat(format, Uri.EscapeDataString(parameter.Key),
					Uri.EscapeDataString(parameter.Value.ToString()));
				hasFirstParam = true;
			}

			return stringBuilder.ToString();
		}
	}

	public class WebApiException : Exception
	{
		public WebApiException(string requestedUrl, HttpStatusCode statusCode, Exception innerException)
			: base("Error occured when calling WebApi", innerException)
		{
			RequestedUrl = requestedUrl;
			StatusCode = statusCode;
		}

		public string RequestedUrl { get; }
		public HttpStatusCode StatusCode { get; }
	}
}