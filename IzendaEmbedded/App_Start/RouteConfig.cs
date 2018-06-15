using System.Web.Mvc;
using System.Web.Routing;

namespace IzendaEmbedded
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("api/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
			//routes.MapRoute("WencorAuth", "/api/wencorAuth",
			//	new {controller = "Account", action = "wencorAuth"});
		}
    }
}