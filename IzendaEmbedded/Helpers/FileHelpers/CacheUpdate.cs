using System.Configuration;
using System.IO;
using System.Net;

namespace IzendaEmbedded.Helpers.FileHelpers
{
	public class CacheUpdate
	{
		private static readonly string endPoint = ConfigurationManager.AppSettings["CachedListEndPoint"];

		public static string GetCSV()
		{
			var req = (HttpWebRequest) WebRequest.Create(endPoint);
			var resp = (HttpWebResponse) req.GetResponse();

			var sr = new StreamReader(resp.GetResponseStream());
			var results = sr.ReadToEnd();
			sr.Close();

			return results;
		}
	}
}