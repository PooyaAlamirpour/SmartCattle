using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Controls;

namespace BeyondThemes.Bootstrap.BootstrapMethods
{
    public partial class Bootstrap<TModel>
    {
        public HtmlHelper<TModel> Html;

        public Bootstrap(HtmlHelper<TModel> _html)
        {
            this.Html = _html;
        }

        public BootstrapFormGroup<TModel> FormGroup()
        {
            return new BootstrapFormGroup<TModel>(Html);
        }

        public BootstrapHelpText HelpText(string text, HelpTextStyle style)
        {
            return new BootstrapHelpText(text, style);
        }

        public BootstrapDropDownMenu DropDownMenu()
        {
            return new BootstrapDropDownMenu();
        }

        public SidebarMenuItem SidebarMenuItem(string text, string url, string areaName, string actionName, string controllerName, IDictionary<string, object> routeValues)
        {
            return new SidebarMenuItem(Html, text, url, areaName, actionName, controllerName, routeValues);
        }

        public InlineLabel InlineLabel(string text)
        {
            return new InlineLabel(text);
        }

        public Badge Badge(string text)
        {
            return new Badge(text);
        }

        [Obsolete("Container() is deprecated and will be removed in the future versions, please use Begin() instead.")]
        public BootstrapContainer Container()
        {
            return new BootstrapContainer(Html);
        }

        public BootstrapIcon Icon(string icon)
        {
            return new BootstrapIcon(icon);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return null; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
