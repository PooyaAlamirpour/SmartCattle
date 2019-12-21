using System.Collections.Generic;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class ToolBar : HtmlElement
    {
        public ToolBar()
            : base("div")
        {
            EnsureClass("btn-toolbar");
        }

        public ToolBar HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public ToolBar HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
