using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class Accordion : HtmlElement
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Id { get; set; }
        public int _activePanelIndex { get; set; }

        public Accordion(string id)
            : base("div")
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureClass("panel-group");
            EnsureClass("accordion");
            EnsureHtmlAttribute("id", this.Id);
        }

        public Accordion ActivePanel(int activePanelIndex)
        {
            _activePanelIndex = activePanelIndex;
            return this;
        }

        public Accordion HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Accordion HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
