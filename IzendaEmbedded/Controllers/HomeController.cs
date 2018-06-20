using System.Linq;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
	public class HomeController : Controller
	{
		[AllowAnonymous]
		public ActionResult Index()
		{
			if (!Request.IsAuthenticated)
			{
				return RedirectToAction("LoginExternal", "Account");
			}
			ViewBag.SystemAdmin = false;
			var identity = (System.Security.Claims.ClaimsIdentity) User.Identity;
			var claims = identity.Claims;
			string SystemAdmin = claims.FirstOrDefault(x => x.Type == "SystemAdmin")?.Value;

			if (SystemAdmin == "True")
			{
				ViewBag.SystemAdmin = true;
			}

			return View();
		}
		[AllowAnonymous]
		public ActionResult NoAccessToIzenda()
		{
			return View();
		}
	}
}