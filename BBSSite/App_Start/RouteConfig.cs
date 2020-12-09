using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BBSSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BBSSite.Controllers" }
            );

            routes.MapRoute(
               name: "Default1",
               url: "{controller}/{action}/{id}/{CurrentPageindex}/{search}",
               defaults: new { controller = "Home", action = "MainContent", id = UrlParameter.Optional, CurrentPageindex = UrlParameter.Optional, search = UrlParameter.Optional },
               namespaces: new string[] { "BBSSite.Controllers" }
           );
        }
    }
}
