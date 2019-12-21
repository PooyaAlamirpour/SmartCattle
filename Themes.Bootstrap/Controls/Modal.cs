using System.Collections.Generic;
using System.ComponentModel;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class Modal : HtmlElement
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _closeable { get; set; }
        public BootstrapSizes _size { get; set; }
        public BootstrapColors _color { get; set; }

        public Modal()
            : base("div")
        {
            EnsureClass("modal");
        }

        public Modal Id(string id)
        {
            MergeHtmlAttribute("id", id);
            return this;
        }

        public Modal HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Modal HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Modal Fade()
        {
            EnsureClass("fade");
            return this;
        }

        public Modal Closeable()
        {
            this._closeable = true;
            return this;
        }

        public Modal Size(BootstrapSizes size)
        {
            this._size = size;
            return this;
        }

        public Modal Color(BootstrapColors color)
        {
            switch (color)
            {
                case BootstrapColors.Danger:
                    EnsureClass("modal-danger");
                    break;
                case BootstrapColors.Default:
                    break;
                case BootstrapColors.Info:
                    EnsureClass("modal-info");
                    break;
                case BootstrapColors.Primary:
                    EnsureClass("modal-primary");
                    break;
                case BootstrapColors.Success:
                    EnsureClass("modal-success");
                    break;
                case BootstrapColors.Warning:
                    EnsureClass("modal-warning");
                    break;
                case BootstrapColors.Sky:
                    EnsureClass("modal-sky");
                    break;
                case BootstrapColors.Blueberry:
                    EnsureClass("modal-blueberry");
                    break;
                case BootstrapColors.Yellow:
                    EnsureClass("modal-yellow");
                    break;
                case BootstrapColors.Darkorange:
                    EnsureClass("modal-darkorange");
                    break;
                case BootstrapColors.Magenta:
                    EnsureClass("modal-magenta");
                    break;
                case BootstrapColors.Purple:
                    EnsureClass("modal-purple");
                    break;
                case BootstrapColors.Maroon:
                    EnsureClass("modal-maroon");
                    break;
                case BootstrapColors.Darkpink:
                    EnsureClass("modal-darkpink");
                    break;
                case BootstrapColors.Pink:
                    EnsureClass("modal-pink");
                    break;
                case BootstrapColors.Azure:
                    EnsureClass("modal-azure");
                    break;
                case BootstrapColors.Orange:
                    EnsureClass("modal-orange");
                    break;
                case BootstrapColors.Palegreen:
                    EnsureClass("modal-palegreen");
                    break;
                case BootstrapColors.Inverse:
                    EnsureClass("modal-inverse");
                    break;
                case BootstrapColors.Blue:
                    EnsureClass("modal-blue");
                    break;
                default:
                    break;
            }
            return this;
        }

    }
}
