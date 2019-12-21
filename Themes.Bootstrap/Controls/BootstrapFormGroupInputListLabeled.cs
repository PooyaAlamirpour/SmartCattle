using System;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupInputListLabeled<TModel, TSource, SValue, SText> : BootstrapInputLabeled
    {
        public BootstrapFormGroupInputListLabeled(HtmlHelper html, object inputModel, BootstrapInputType inputType)
            : base(html, inputModel, inputType)
        {
            this._labelModel.htmlAttributes = _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            if (string.IsNullOrEmpty(_inputModel.htmlFieldName)) return null;

            string input = RendererList<TModel, TSource, SValue, SText>.RenderInputList(html, _inputModel);
            string label = Renderer.RenderLabel(html, _labelModel);

            return new BootstrapFormGroup(input, label, FormGroupType.textboxLike).ToHtmlString();
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
