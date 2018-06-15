using IzendaEmbedded.IzendaBoundary;
using IzendaEmbedded.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
	public class UserController : Controller
	{
		[HttpGet]
		[AllowAnonymous]
		public ActionResult GenerateToken()
		{
			var username = ConfigurationManager.AppSettings["IzendaAdminUser"];
			var tenantName = ((ClaimsIdentity) User.Identity).FindFirstValue("tenantName");

			var user = new UserInfo {UserName = username, TenantUniqueName = tenantName};
			var token = IzendaTokenAuthorization.GetToken(user);
			return Json(new {token}, JsonRequestBehavior.AllowGet);
		}
	}
}