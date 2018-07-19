using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
		public ActionResult DashboardViewer(string id)
		{
			ViewBag.Id = id;
			return View();
		}
    }
}