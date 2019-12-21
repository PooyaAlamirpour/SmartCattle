using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Infrastructure.Enums;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupCustom<TModel> : IHtmlString
    {
        private HtmlHelper html;
        private BootstrapFormGroupCustomModel _model = new BootstrapFormGroupCustomModel();

        public BootstrapFormGroupCustom(HtmlHelper html, string input)
        {
            this._model.input = input;
            this.html = html;
        }

        public BootstrapFormGroupCustom<TModel> CustomLabel(string label)
        {
            this._model.labelString = label;
            return this;
        }

        public BootstrapFormGroupCustom<TModel> CustomLabel(params IHtmlString[] label)
        {
            string controlsString = string.Empty;
            label.ToList().ForEach(x => this._model.labelString += x.ToHtmlString());
            return this;
        }

        public IBootstrapLabel LabelFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            _model.htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            _model.metadata = ModelMetadata.FromStringExpression(_model.htmlFieldName, html.ViewData);
            IBootstrapLabel l = new BootstrapFormGroupLabeled(html, _model, BootstrapInputType.Custom);
            return l;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string ToHtmlString()
        {
            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("control-label");
            span.InnerHtml = _model.labelString;

            bool fieldIsValid = true;
            if (_model != null && _model.htmlFieldName != null) fieldIsValid = html.ViewData.ModelState.IsValidField(_model.htmlFieldName);
            return new BootstrapFormGroup(_model.input, span.ToString(TagRenderMode.Normal), FormGroupType.textboxLike, fieldIsValid).ToHtmlString();
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
