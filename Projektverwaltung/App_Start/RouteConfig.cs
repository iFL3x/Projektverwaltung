// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
//   Class to register MapRoutes and Custom Routing
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Projektverwaltung
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Projekte", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
