using System.Collections.Generic;
using System.ComponentModel;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class Alert : HtmlElement
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _closeable { get; set; }

        public Alert()
            : base("div")
        {
            EnsureClass("alert");
        }

        public Alert Style(AlertColor style)
        {
            switch (style)
            {
                case AlertColor.Info:
                    EnsureClass("alert-info");
                    break;
                case AlertColor.Error:
                    EnsureClass("alert-danger");
                    break;
                case AlertColor.Success:
                    EnsureClass("alert-success");
                    break;
                case AlertColor.Warning:
                    EnsureClass("alert-warning");
                    break;
            }
            return this;
        }

        public Alert Closeable()
        {
            this._closeable = true;
            return this;
        }

        public Alert Block()
        {
            EnsureClass("alert-block");
            return this;
        }
        
        public Alert RadiusBordered()
        {
            EnsureClass("radius-bordered");
            return this;
        }

        public Alert Shadowed()
        {
            EnsureClass("alert-shadowed");
            return this;
        }

        public Alert HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Alert HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
