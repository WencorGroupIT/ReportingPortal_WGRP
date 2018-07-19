using System;
using System.Linq;
using System.Web.Mvc;
using IzendaEmbedded.IzendaBoundary;

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

		[Route("izenda/settings")]
		[Route("izenda/new")]
		[Route("izenda/dashboard")]
		[Route("izenda/report")]
		[Route("izenda/reportviewer")]
		[Route("izenda/reportviewerpopup")]
		[Route("izenda")]
		public ActionResult Izenda()
		{
			return View();
		}

		[AllowAnonymous]
		[Route("viewer/reportpart/{id}")]
		public ActionResult ReportPart(Guid id, string token)
		{
			
			//can we validate the token here
			//validates token
			var user = IzendaTokenAuthorization.GetUserInfo(token);

			if(user != null)
			{
				ViewBag.Id = id;
				ViewBag.Token = token;
			}
			else
			{
				return HttpNotFound(); // is invalid user roles 
			}
			

			return View();
		}
	}
}