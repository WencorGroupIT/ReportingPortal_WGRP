using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        // GET: Report
		public ActionResult ReportViewer(string id)
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
			
			var queryString = Request.QueryString;
            dynamic filters = new System.Dynamic.ExpandoObject();
            foreach (string key in queryString.AllKeys)
            {
                ((IDictionary<String, Object>)filters).Add(key, queryString[key]);
            }

            ViewBag.overridingFilterQueries = JsonConvert.SerializeObject(filters); 
            ViewBag.Id = id;

			return View();
		}
    }
}