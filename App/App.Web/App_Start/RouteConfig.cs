using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Categories",
                url: "Items/Category/{categoryId}",
                defaults: new { controller = "Items", action = "Index" },
                namespaces: new string[] { "App.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ItemDetails",
                url: "Items/{id}",
                defaults: new { controller = "Items", action = "Details" },
                namespaces: new string[] { "App.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Articles",
                url: "Articles/{id}",
                defaults: new { controller = "Articles", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "App.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Pages",
                url: "Pages/{id}",
                defaults: new { controller = "Pages", action = "Index"},
                namespaces: new string[] { "App.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "App.Web.Controllers" }
            );
        }
    }
}
