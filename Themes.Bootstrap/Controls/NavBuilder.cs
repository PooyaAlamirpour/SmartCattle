﻿using System.Threading.Tasks;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class NavBuilder<TModel> : BuilderBase<TModel, Nav>
    {
        private readonly UrlHelper urlHelper;
        private bool _wrapTagControllerAware;
        private bool _wrapTagControllerAndActionAware;

        internal NavBuilder(HtmlHelper<TModel> htmlHelper, Nav nav)
            : base(htmlHelper, nav)
        {
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
 
            if (nav._activeLinksByController) _wrapTagControllerAware = true;
            if (nav._activeLinksByControllerAndAction) _wrapTagControllerAndActionAware = true;
        }

        public BootstrapLink Link(string linkText, string url)
        {
            return new BootstrapLink(base.htmlHelper, linkText, url).WrapInto("li");
        }

        public BootstrapActionLink ActionLink(string linkText, ActionResult result)
        {
            return new BootstrapActionLink(htmlHelper, linkText, result)
                .WrapInto("li")
                .WrapTagControllerAware(_wrapTagControllerAware)
                .WrapTagControllerAndActionAware(_wrapTagControllerAndActionAware);
        }

        public BootstrapActionLink ActionLink(string linkText, Task<ActionResult> taskResult)
        {
            return new BootstrapActionLink(htmlHelper, linkText, taskResult)
                .WrapInto("li")
                .WrapTagControllerAware(_wrapTagControllerAware)
                .WrapTagControllerAndActionAware(_wrapTagControllerAndActionAware);
        }

        public BootstrapActionLink ActionLink(string linkText, string actionName)
        {
            return new BootstrapActionLink(htmlHelper, linkText, actionName)
                .WrapInto("li")
                .WrapTagControllerAware(_wrapTagControllerAware)
                .WrapTagControllerAndActionAware(_wrapTagControllerAndActionAware);
        }

        public BootstrapActionLink ActionLink(string linkText, string actionName, string controllerName)
        {
            return new BootstrapActionLink(htmlHelper, linkText, actionName, controllerName)
                .WrapInto("li")
                .WrapTagControllerAware(_wrapTagControllerAware)
                .WrapTagControllerAndActionAware(_wrapTagControllerAndActionAware);
        }

        public void Divider()
        {
            textWriter.Write(@"<li class=""divider""></li>");
        }

        public void Header(string header)
        {
            textWriter.Write(string.Format(@"<li class=""nav-header"">{0}</li>", header));
        }
    }
}
