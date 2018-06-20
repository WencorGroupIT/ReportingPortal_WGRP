using IzendaEmbedded.IzendaBoundary;
using IzendaEmbedded.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
	public class UserController : Controller
	{
		[HttpGet]
		[Authorize]
		public ActionResult GenerateToken()
		{
			
			var tenantName = ((ClaimsIdentity) User.Identity).FindFirstValue("tenantName");
			var username = ((ClaimsIdentity) User.Identity).FindFirstValue("UserName");

			var claimsIdentity = ((ClaimsIdentity) User.Identity);
			username = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
			var simpleClaims = claimsIdentity.Claims.Select( c => new{ c.Type, c.Value});

			var user = new UserInfo {UserName = username, TenantUniqueName = tenantName};
			var token = IzendaTokenAuthorization.GetToken(user);
			return Json(new 
				{
					token, username, tenantName, 
					simpleClaims
			  }, JsonRequestBehavior.AllowGet);
		}
	}
}