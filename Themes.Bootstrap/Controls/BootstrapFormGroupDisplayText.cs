using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupDisplayText : IBootstrapDisplayText
    {
        private HtmlHelper html;
        private BootstrapDisplayTextModel _model = new BootstrapDisplayTextModel();

        public BootstrapFormGroupDisplayText(HtmlHelper html, string htmlFieldName, ModelMetadata metadata)
        {
            this.html = html;
            this._model.htmlFieldName = htmlFieldName;
            this._model.metadata = metadata;
        }

        public IBootstrapDisplayText HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            this._model.htmlAttributes = htmlAttributes;
            return this;
        }

        public IBootstrapDisplayText HtmlAttributes(object htmlAttributes)
        {
            this._model.htmlAttributes = htmlAttributes.ToDictionary();
            return this;
        }

        public IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapFormGroupLabeled(html, _model, BootstrapInputType.Display);
            return l;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string ToHtmlString()
        {
            return Renderer.RenderFormGroupDisplayText(html, _model, null);
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
