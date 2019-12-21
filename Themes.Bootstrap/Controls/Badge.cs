using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap
{
    public class Badge : IHtmlString
    {
        private string _text
        {
            get;
            set;
        }

        private BootstrapColors _color
        {
            get;
            set;
        }

        private bool _graded
        {
            get;
            set;
        }
        private bool _square
        {
            get;
            set;
        }

        public Badge Color(BootstrapColors color)
        {
            _color = color;
            return this;
        }

        public Badge Graded()
        {
            _graded = true;
            return this;
        }

        public Badge Square()
        {
            _square = true;
            return this;
        }
        public Badge(string text)
        {
            _text = text;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ToHtmlString()
        {
            var builder = new TagBuilder("span");

            switch (_color)
            {
                case BootstrapColors.Danger:
                    builder.AddCssClass("badge badge-danger");
                    break;
                case BootstrapColors.Default:
                    builder.AddCssClass("badge");
                    break;
                case BootstrapColors.Info:
                    builder.AddCssClass("badge badge-info");
                    break;
                case BootstrapColors.Primary:
                    builder.AddCssClass("badge badge-primary");
                    break;
                case BootstrapColors.Success:
                    builder.AddCssClass("badge badge-success");
                    break;
                case BootstrapColors.Warning:
                    builder.AddCssClass("badge badge-warning");
                    break;
                case BootstrapColors.Sky:
                    builder.AddCssClass("badge badge-sky");
                    break;
                case BootstrapColors.Blueberry:
                    builder.AddCssClass("badge badge-blueberry");
                    break;
                case BootstrapColors.Yellow:
                    builder.AddCssClass("badge badge-yellow");
                    break;
                case BootstrapColors.Darkorange:
                    builder.AddCssClass("badge badge-darkorange");
                    break;
                case BootstrapColors.Magenta:
                    builder.AddCssClass("badge badge-magenta");
                    break;
                case BootstrapColors.Purple:
                    builder.AddCssClass("badge badge-purple");
                    break;
                case BootstrapColors.Maroon:
                    builder.AddCssClass("badge badge-maroon");
                    break;
                case BootstrapColors.Darkpink:
                    builder.AddCssClass("badge badge-darkpink");
                    break;
                case BootstrapColors.Pink:
                    builder.AddCssClass("badge badge-pink");
                    break;
                case BootstrapColors.Azure:
                    builder.AddCssClass("badge badge-azure");
                    break;
                case BootstrapColors.Orange:
                    builder.AddCssClass("badge badge-orange");
                    break;
                case BootstrapColors.Palegreen:
                    builder.AddCssClass("badge badge-palegreen");
                    break;
                case BootstrapColors.Inverse:
                    builder.AddCssClass("badge badge-inverse");
                    break;
                case BootstrapColors.Blue:
                    builder.AddCssClass("badge badge-blue");
                    break;
                default:
                    builder.AddCssClass("badge");
                    break;
            }
            if (_graded)
                builder.AddCssClass("graded");

            if (_square)
                builder.AddCssClass("badge-square");

            builder.InnerHtml = _text;

            return MvcHtmlString.Create(builder.ToString()).ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return ToHtmlString(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
