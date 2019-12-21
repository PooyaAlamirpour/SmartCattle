using MvcSiteMapProvider.Web.UrlResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider;
using System.Threading;
using SmartCattle.Web.CustomFilters;

namespace SmartCattle.Web.Models
{
    public class CustomSiteMapUrlResolver : SiteMapNodeUrlResolver
    {
        public CustomSiteMapUrlResolver(IMvcContextFactory mvcContextFactory, IUrlPath urlPath) : base(mvcContextFactory, urlPath)
        {
            var ack = urlPath;
        }

        protected override string ResolveRouteUrl(ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues)
        { 
            //controller = Thread.CurrentThread.CurrentCulture.ToString() +"/"+ controller;
            controller = LocalizationAttribute.Current_Language_Culture + "/" + controller;
            return base.ResolveRouteUrl(node, area, controller, action, routeValues);
        }
    }
}