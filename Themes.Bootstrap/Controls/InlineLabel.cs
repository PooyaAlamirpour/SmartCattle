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
    public class InlineLabel : IHtmlString
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

        public InlineLabel Color(BootstrapColors color)
        {
            _color = color;
            return this;
        }

        public InlineLabel Graded()
        {
            _graded = true;
            return this;
        }
        public InlineLabel(string text)
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
                    builder.AddCssClass("label label-danger");
                    break;
                case BootstrapColors.Default:
                    builder.AddCssClass("label label-default");
                    break;
                case BootstrapColors.Info:
                    builder.AddCssClass("label label-info");
                    break;
                case BootstrapColors.Primary:
                    builder.AddCssClass("label label-primary");
                    break;
                case BootstrapColors.Success:
                    builder.AddCssClass("label label-success");
                    break;
                case BootstrapColors.Warning:
                    builder.AddCssClass("label label-warning");
                    break;
                case BootstrapColors.Sky:
                    builder.AddCssClass("label label-sky");
                    break;
                case BootstrapColors.Blueberry:
                    builder.AddCssClass("label label-blueberry");
                    break;
                case BootstrapColors.Yellow:
                    builder.AddCssClass("label label-yellow");
                    break;
                case BootstrapColors.Darkorange:
                    builder.AddCssClass("label label-darkorange");
                    break;
                case BootstrapColors.Magenta:
                    builder.AddCssClass("label label-magenta");
                    break;
                case BootstrapColors.Purple:
                    builder.AddCssClass("label label-purple");
                    break;
                case BootstrapColors.Maroon:
                    builder.AddCssClass("label label-maroon");
                    break;
                case BootstrapColors.Darkpink:
                    builder.AddCssClass("label label-darkpink");
                    break;
                case BootstrapColors.Pink:
                    builder.AddCssClass("label label-pink");
                    break;
                case BootstrapColors.Azure:
                    builder.AddCssClass("label label-azure");
                    break;
                case BootstrapColors.Orange:
                    builder.AddCssClass("label label-orange");
                    break;
                case BootstrapColors.Palegreen:
                    builder.AddCssClass("label label-palegreen");
                    break;
                case BootstrapColors.Inverse:
                    builder.AddCssClass("label label-inverse");
                    break;
                case BootstrapColors.Blue:
                    builder.AddCssClass("label label-blue");
                    break;
                default:
                    builder.AddCssClass("label");
                    break;
            }
            if (_graded)
                builder.AddCssClass("graded");

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
