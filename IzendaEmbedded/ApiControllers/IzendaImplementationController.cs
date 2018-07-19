using IzendaEmbedded.IzendaBoundary;
using IzendaEmbedded.Models;
using System.Web.Http;

namespace IzendaEmbedded.ApiControllers
{
	[RoutePrefix("api/IzendaImplementation")]
	public class IzendaImplementationController : ApiController
	{
		[HttpGet]
		[Route("validateIzendaAuthToken")]
		public UserInfo ValidateIzendaAuthToken(string access_token)
		{
			var userInfo = IzendaTokenAuthorization.GetUserInfo(access_token);
			return userInfo;
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("getIzendaAccessToken")]
		public IHttpActionResult GetIzendaAccessToken(string message)
		{
			var userInfo = IzendaTokenAuthorization.DecryptIzendaAuthenticationMessage(message);
			var token = IzendaTokenAuthorization.GetToken(userInfo);
			//return token;
			return Ok(new {Token = token});
		}
	}
}