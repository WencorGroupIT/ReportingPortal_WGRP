using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IzendaEmbedded.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        
        // GET: Dashboard
        public ActionResult DashboardViewer(string id)
		{
            var queryString = Request.QueryString;
            dynamic filters = new System.Dynamic.ExpandoObject();
            foreach (string key in queryString.AllKeys)
            {
                ((IDictionary<String, Object>)filters).Add(key, queryString[key]);
            }

            ViewBag.Id = id;
            ViewBag.filters = JsonConvert.SerializeObject(filters);

            return View();
		}
    }
}