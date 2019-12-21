﻿using System.Collections.Generic;
using System.ComponentModel;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class Nav : HtmlElement
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _activeLinksByController;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _activeLinksByControllerAndAction;

        public Nav()
            : base("ul")
        {
            EnsureClass("nav");
        }

        public Nav Style(NavType type)
        {
            switch (type)
            {
                case NavType.Tabs:
                    EnsureClass("nav-tabs");
                    break;
                case NavType.Pills:
                    EnsureClass("nav-pills");
                    break;
                case NavType.List:
                    EnsureClass("nav-list");
                    break;
            }
            return this;
        }

        public Nav Id(string id)
        {
            MergeHtmlAttribute("id", id);
            return this;
        }

        public Nav Stacked()
        {
            EnsureClass("nav-stacked");
            return this;
        }

        public Nav SetLinksActiveByController()
        {
            _activeLinksByController = true;
            return this;
        }

        public Nav SetLinksActiveByControllerAndAction()
        {
            _activeLinksByControllerAndAction = true;
            return this;
        }

        public Nav HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Nav HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
