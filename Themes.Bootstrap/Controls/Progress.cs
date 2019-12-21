using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class Progress : HtmlElement
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Id { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _vertical { get; set; }

       

        public Progress Vertical()
        {
            _vertical = true;
            EnsureClass("progress-vertical");
            return this;
        }

        public Progress Size(BootstrapSizes size)
        {
            switch (size)
            {
                case BootstrapSizes.XLarge:
                    EnsureClass("progress-xlg");
                    break;
                case BootstrapSizes.Large:
                    EnsureClass("progress-lg");
                    break;
                case BootstrapSizes.Small:
                    EnsureClass("progress-sm");
                    break;
                case BootstrapSizes.XSmall:
                    EnsureClass("progress-xs");
                    break;
                case BootstrapSizes.XXSmall:
                    EnsureClass("progress-xxs");
                    break;
                default:
                    break;
            }
            return this;
        }

        public Progress Active()
        {
            EnsureClass("progress-striped");
            EnsureClass("active");
            return this;
        }

        public Progress Stripped()
        {
            EnsureClass("progress-striped");
            return this;
        }

        public Progress Bordered()
        {
            EnsureClass("progress-bordered");
            return this;
        }

        public Progress NoRadiusBordered()
        {
            EnsureClass("progress-no-radius");
            return this;
        }

        public Progress Shadowed()
        {
            EnsureClass("progress-shadowed");
            return this;
        }

        public Progress Align(Direction align)
        {
            switch (align)
            {
                case Direction.Right:
                    EnsureClass("progress-right");
                    break;
                case Direction.Bottom:
                    EnsureClass("progress-bottom");
                    break;
                default:
                    break;
            }
            return this;
        }

        public Progress(string id) : base("div")
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            Id = HtmlHelper.GenerateIdFromName(id);

            EnsureClass("progress");

            EnsureHtmlAttribute("id", Id);
        }



        public Progress HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Progress HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}