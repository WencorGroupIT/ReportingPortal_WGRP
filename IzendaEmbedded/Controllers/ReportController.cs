using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
		public ActionResult ReportViewer(string id)
		{
			ViewBag.Id = id;
			return View();
		}

		public ActionResult ReportCustomFilterViewer()
		{
			return View();
		}

		public ActionResult ReportParts()
		{
			return View();
		}

		public ActionResult AdvancedReportParts()
		{
			return View();
		}
    }
}