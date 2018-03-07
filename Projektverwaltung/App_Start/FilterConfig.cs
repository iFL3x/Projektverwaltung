// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
//   Class to register filters
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Web;
using System.Web.Mvc;

namespace Projektverwaltung
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
