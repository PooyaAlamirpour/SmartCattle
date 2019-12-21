using System.Collections.Generic;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class ButtonGroup : HtmlElement
    {
        public ButtonGroup()
            : base("div")
        {
            EnsureClass("btn-group");
        }

        public ButtonGroup HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public ButtonGroup HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
